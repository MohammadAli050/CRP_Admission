using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Web.UI;
using Admission.App_Start;

namespace Admission.Admission.Office
{
    public partial class SSCHSCBasicInfoDataUpdate : PageBase
    {
        long uId = 0;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            if (!IsPostBack)
            {
                panelInformation.Visible = false;
                ClearData();
                LoadDDL();
                LoadPassingYear();
            }
        }

        private void LoadPassingYear()
        {
            try
            {
                ddlPassYear.Items.Clear();
                ddlPassYear.AppendDataBoundItems = true;
                ddlPassYear.Items.Add(new ListItem("-Select-", "0"));

                int EndYear = DateTime.Now.Year;

                int StartYear = 1950;

                for (int i = EndYear; i >= StartYear; i--)
                {
                    ddlPassYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.EducationBoard> educationBoardList = db.GetAllEducationBoard_ND();
                if (educationBoardList != null && educationBoardList.Any())
                {

                    DDLHelper.Bind<DAL.EducationBoard>(ddlBoard, educationBoardList.Where(a => a.IsActive == true && a.IsVisual == true).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                }
            }

        }

        protected void btnVerifyInformation_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();

                string ResultType = ddlExamType.SelectedValue.ToString();

                string Roll = txtRoll.Text.Trim();

                string RegNo = txtReg.Text.Trim();

                string PassingYear = ddlPassYear.SelectedValue.ToString();

                int BoardId = Convert.ToInt32(ddlBoard.SelectedValue);

                int PassingYearId = Convert.ToInt32(ddlPassYear.SelectedValue);
                string BoardT = "";

                #region EducationBoard (SSC-HSC)
                DAL.EducationBoard EducationBoard = null;

                using (var db = new GeneralDataManager())
                {
                    EducationBoard = db.AdmissionDB.EducationBoards.Where(x => x.ID == BoardId).FirstOrDefault();
                }

                if (EducationBoard != null)
                {
                    BoardT = EducationBoard.ShortName.Trim();
                }

                #endregion


                try
                {
                    using (var db = new CandidateDataManager())
                    {

                        if (ResultType == "ssc")
                        {
                            var BasicInfoData = db.AdmissionDB.GetSSCBasicDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();
                            if (BasicInfoData != null)
                            {
                                panelInformation.Visible = true;
                                try
                                {
                                    #region Basic Info

                                    try
                                    {
                                        txtName.Text = BasicInfoData.NAME;
                                        txtFatherName.Text = BasicInfoData.FATHER;
                                        txtMotherName.Text = BasicInfoData.MOTHER;
                                        ddlGender.SelectedValue = BasicInfoData.SEX.ToUpper().Trim();
                                        txtGroup.Text = BasicInfoData.STUD_GROUP;
                                        txtGPA.Text = BasicInfoData.GPA.HasValue ? BasicInfoData.GPA.Value.ToString("0.00") : "";
                                        txtResult.Text = BasicInfoData.RESULT;
                                    }
                                    catch (Exception ex)
                                    {
                                    }


                                    #endregion

                                    #region Total Marks

                                    try
                                    {

                                        var totalMarksData = db.AdmissionDB.GetSSCTotalMarksDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();
                                        if (totalMarksData != null)
                                        {
                                            txtTotalMarks.Text = totalMarksData.TotalMarks.HasValue ? totalMarksData.TotalMarks.Value.ToString("0.00") : "";
                                            txtObtained.Text = totalMarksData.TotalObtained.HasValue ? totalMarksData.TotalObtained.Value.ToString("0.00") : "";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    #region Subject Wise Result

                                    List<TelitalkEducationSubjectModel> subjectModels = new List<TelitalkEducationSubjectModel>();
                                    try
                                    {
                                        var subjectResultData = db.AdmissionDB.GetSSCSubjectResultDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();
                                        if (subjectResultData != null)
                                        {
                                            string ResultOne = subjectResultData.GradeString;
                                            string ResultTwo = subjectResultData.GradeString2;

                                            /// ResultOne and ResultTwo contains comma separated subject results


                                            string[] subjectResultsOne = ResultOne.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                            string[] subjectResultsTwo = ResultTwo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (string subjectResult in subjectResultsOne)
                                            {
                                                // Each subjectResult is in the format "subCode:subName:grade:gpoint:mark"
                                                string[] parts = subjectResult.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                                if (parts.Length == 2)
                                                {
                                                    TelitalkEducationSubjectModel subjectModel = new TelitalkEducationSubjectModel
                                                    {
                                                        subCode = parts[0],
                                                        grade = parts[1],
                                                        subName = "One"
                                                    };
                                                    subjectModels.Add(subjectModel);
                                                }
                                            }
                                            foreach (string subjectResult in subjectResultsTwo)
                                            {
                                                // Each subjectResult is in the format "subCode:subName:grade:gpoint:mark"
                                                string[] parts = subjectResult.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                                if (parts.Length == 2)
                                                {
                                                    TelitalkEducationSubjectModel subjectModel = new TelitalkEducationSubjectModel
                                                    {
                                                        subCode = parts[0],
                                                        grade = parts[1],
                                                        subName = "Two"
                                                    };
                                                    subjectModels.Add(subjectModel);
                                                }
                                            }



                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    gvSubjectResult.DataSource = subjectModels;
                                    gvSubjectResult.DataBind();

                                    #endregion

                                }
                                catch (Exception ex)
                                {
                                }

                            }
                            else
                            {
                                showAlert("No data found for your provided data!");
                                return;
                            }

                        }
                        else if (ResultType == "hsc")
                        {
                            var BasicInfoData = db.AdmissionDB.GetHSCBasicDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();
                            if (BasicInfoData != null)
                            {
                                panelInformation.Visible = true;
                                try
                                {
                                    #region Basic Info

                                    try
                                    {
                                        txtName.Text = BasicInfoData.NAME;
                                        txtFatherName.Text = BasicInfoData.FATHER;
                                        txtMotherName.Text = BasicInfoData.MOTHER;
                                        ddlGender.SelectedValue = BasicInfoData.SEX.ToUpper().Trim();
                                        txtGroup.Text = BasicInfoData.STUD_GROUP;
                                        txtGPA.Text = BasicInfoData.GPA.HasValue ? BasicInfoData.GPA.Value.ToString("0.00") : "";
                                        txtResult.Text = BasicInfoData.RESULT;
                                    }
                                    catch (Exception ex)
                                    {
                                    }


                                    #endregion

                                    #region Total Marks

                                    try
                                    {

                                        var totalMarksData = db.AdmissionDB.GetHSCTotalMarksDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();
                                        if (totalMarksData != null)
                                        {
                                            txtTotalMarks.Text = totalMarksData.TotalMarks.HasValue ? totalMarksData.TotalMarks.Value.ToString("0.00") : "";
                                            txtObtained.Text = totalMarksData.TotalObtained.HasValue ? totalMarksData.TotalObtained.Value.ToString("0.00") : "";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    #region Subject Wise Result

                                    List<TelitalkEducationSubjectModel> subjectModels = new List<TelitalkEducationSubjectModel>();
                                    try
                                    {
                                        var subjectResultData = db.AdmissionDB.GetHSCSubjectResultDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();
                                        if (subjectResultData != null)
                                        {
                                            string ResultOne = subjectResultData.GradeString;
                                            string ResultTwo = subjectResultData.GradeString2;

                                            /// ResultOne and ResultTwo contains comma separated subject results


                                            string[] subjectResultsOne = ResultOne.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                            string[] subjectResultsTwo = ResultTwo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (string subjectResult in subjectResultsOne)
                                            {
                                                // Each subjectResult is in the format "subCode:subName:grade:gpoint:mark"
                                                string[] parts = subjectResult.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                                if (parts.Length == 2)
                                                {
                                                    TelitalkEducationSubjectModel subjectModel = new TelitalkEducationSubjectModel
                                                    {
                                                        subCode = parts[0],
                                                        grade = parts[1],
                                                        subName = "One"
                                                    };
                                                    subjectModels.Add(subjectModel);
                                                }
                                            }
                                            foreach (string subjectResult in subjectResultsTwo)
                                            {
                                                // Each subjectResult is in the format "subCode:subName:grade:gpoint:mark"
                                                string[] parts = subjectResult.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                                if (parts.Length == 2)
                                                {
                                                    TelitalkEducationSubjectModel subjectModel = new TelitalkEducationSubjectModel
                                                    {
                                                        subCode = parts[0],
                                                        grade = parts[1],
                                                        subName = "Two"
                                                    };
                                                    subjectModels.Add(subjectModel);
                                                }
                                            }



                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    gvSubjectResult.DataSource = subjectModels;
                                    gvSubjectResult.DataBind();

                                    #endregion

                                }
                                catch (Exception ex)
                                {
                                }

                            }
                            else
                            {
                                showAlert("No data found for your provided data!");
                                return;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    showAlert("Something went wrong.Exception : " + ex.Message.ToString());
                    return;
                }


            }
            catch (Exception ex)
            {
            }

        }

        private void ClearData()
        {
            try
            {
                txtName.Text = "";
                txtFatherName.Text = "";
                txtMotherName.Text = "";
                ddlGender.SelectedValue = "MALE";
                txtGroup.Text = "";
                txtGPA.Text = "";
                txtResult.Text = "";
                txtTotalMarks.Text = "";
                txtTotalMarks.Text = "";


                gvSubjectResult.DataSource = null;
                gvSubjectResult.DataBind();
                panelInformation.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }


        protected void ddlExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        protected void txtRoll_TextChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        protected void txtReg_TextChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        protected void ddlPassYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        protected void ddlBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
        }


        #region Update basic Information

        protected void btnUpdateBasicInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string ResultType = ddlExamType.SelectedValue.ToString();
                string Roll = txtRoll.Text.Trim();
                string RegNo = txtReg.Text.Trim();
                string PassingYear = ddlPassYear.SelectedValue.ToString();
                int BoardId = Convert.ToInt32(ddlBoard.SelectedValue);
                int PassingYearId = Convert.ToInt32(ddlPassYear.SelectedValue);
                string BoardT = "";
                #region EducationBoard (SSC-HSC)
                DAL.EducationBoard EducationBoard = null;
                using (var db = new GeneralDataManager())
                {
                    EducationBoard = db.AdmissionDB.EducationBoards.Where(x => x.ID == BoardId).FirstOrDefault();
                }
                if (EducationBoard != null)
                {
                    BoardT = EducationBoard.ShortName.Trim();
                }
                #endregion


                #region Variable 

                string name = txtName.Text.Trim();
                string fatherName = txtFatherName.Text.Trim();
                string motherName = txtMotherName.Text.Trim();
                string gender = ddlGender.SelectedValue.ToString().Trim();
                string group = txtGroup.Text.Trim();
                string result = txtResult.Text.Trim();
                decimal gpa = Convert.ToDecimal(txtGPA.Text.Trim());
                decimal totalMarks = Convert.ToDecimal(txtTotalMarks.Text.Trim());
                decimal obtainedMarks = Convert.ToDecimal(txtObtained.Text.Trim());

                #endregion


                using (var db = new CandidateDataManager())
                {
                    if (ResultType == "ssc")
                    {
                        var updateResult = db.AdmissionDB.UpdateSSCBasicDataAndMarksFromEducationDB(BoardT, Roll, RegNo, PassingYear,
                           name, fatherName, motherName, gender, group, result, gpa, totalMarks, obtainedMarks);
                        if (updateResult > 0)
                        {
                            showAlert("Data updated successfully.");
                        }
                        else
                        {
                            showAlert("Data update failed.");
                        }
                    }
                    else if (ResultType == "hsc")
                    {
                        var updateResult = db.AdmissionDB.UpdateHSCBasicDataAndMarksFromEducationDB(BoardT, Roll, RegNo, PassingYear,
                           name, fatherName, motherName, gender, group, result, gpa, totalMarks, obtainedMarks);
                        if (updateResult > 0)
                        {
                            showAlert("Data updated successfully.");
                        }
                        else
                        {
                            showAlert("Data update failed.");
                        }
                    }


                    try
                    {
                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "Update Basic Information And Total Marks";
                        dLog.PageName = "SSCHSCBasicInfoDataUpdate.aspx";
                        dLog.OldData = "";
                        dLog.NewData = "Updated student information " + ResultType + " result for Roll : " + txtRoll.Text + " , RegNo : " + txtReg.Text
                            + " , Board : " + ddlBoard.SelectedItem.ToString() + " , Passing Year : " + ddlPassYear.SelectedItem.ToString()
                            + " , Name : " + name + " , Father Name : " + fatherName + " , Mother Name : " + motherName
                            + " , Gender : " + gender + " , Group : " + group + " , Result : " + result + " , GPA : " + gpa.ToString("0.00")
                            + " , Total Marks : " + totalMarks.ToString("0.00") + " , Obtained Marks : " + obtainedMarks.ToString("0.00");


                        dLog.UserId = uId;
                        dLog.SessionInformation ="";
                        dLog.DateCreated = DateTime.Now;
                        dLog.IpAddress = null;
                        dLog.DateTime = DateTime.Now;
                        dLog.HostName = Request.UserHostAddress + Request.UserHostName;
                        LogWriter.DataLogWriter(dLog);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                    }

                }



            }
            catch (Exception ex)
            {
                showAlert("Something went wrong.Exception : " + ex.Message.ToString());
                return;
            }

        }


        protected void btnUpdateSubjectGrade_Click(object sender, EventArgs e)
        {
            try
            {
                string ResultType = ddlExamType.SelectedValue.ToString();
                string Roll = txtRoll.Text.Trim();
                string RegNo = txtReg.Text.Trim();
                string PassingYear = ddlPassYear.SelectedValue.ToString();
                int BoardId = Convert.ToInt32(ddlBoard.SelectedValue);
                int PassingYearId = Convert.ToInt32(ddlPassYear.SelectedValue);
                string BoardT = "";
                #region EducationBoard (SSC-HSC)
                DAL.EducationBoard EducationBoard = null;
                using (var db = new GeneralDataManager())
                {
                    EducationBoard = db.AdmissionDB.EducationBoards.Where(x => x.ID == BoardId).FirstOrDefault();
                }
                if (EducationBoard != null)
                {
                    BoardT = EducationBoard.ShortName.Trim();
                }
                #endregion

                using (var db = new CandidateDataManager())
                {
                    if (ResultType == "ssc")
                    {
                        // Prepare grade strings
                        List<string> gradePartsOne = new List<string>();
                        List<string> gradePartsTwo = new List<string>();
                        foreach (GridViewRow row in gvSubjectResult.Rows)
                        {
                            string subCode = ((Label)row.FindControl("lblSubectCode")).Text.Trim();
                            string grade = ((TextBox)row.FindControl("txtSubjectGarde")).Text.Trim();
                            string subName = ((Label)row.FindControl("lblSubSeq")).Text.Trim();

                            if (grade != "A+" && grade != "A" && grade != "A-" && grade != "B" && grade != "C" && grade != "D" && grade != "F")
                            {
                                showAlert("You are entering a invalid grade for Subject :" + subCode + " and grade : " + grade);
                                return;
                            }

                            if (subName == "One")
                            {
                                gradePartsOne.Add($"{subCode}:{grade}");
                            }
                            else if (subName == "Two")
                            {
                                gradePartsTwo.Add($"{subCode}:{grade}");
                            }
                        }
                        string GradeString = string.Join(",", gradePartsOne);
                        string GradeString2 = string.Join(",", gradePartsTwo);

                        var logOldObject = db.AdmissionDB.GetSSCSubjectResultDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();

                        var updateResult = db.AdmissionDB.UpdateSSCSubjectResultDataFromEducationDB(BoardT, Roll, RegNo, PassingYear,
                            GradeString,
                            GradeString2);
                        if (updateResult > 0)
                        {
                            showAlert("Subject grades updated successfully.");

                            try
                            {
                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.EventName = "Update Grade Wise Board Result";
                                dLog.PageName = "SSCHSCBasicInfoDataUpdate.aspx";
                                dLog.OldData = ObjectToString.ConvertToString(logOldObject);
                                dLog.NewData = GradeString + "_" + GradeString2;
                                dLog.UserId = uId;
                                dLog.SessionInformation = "Updated subject wise grade for " + ResultType + " result for Roll : " + txtRoll.Text + " , RegNo : " + txtReg.Text
                                    + " , Board : " + ddlBoard.SelectedItem.ToString() + " , Passing Year : " + ddlPassYear.SelectedItem.ToString();
                                dLog.DateCreated = DateTime.Now;
                                dLog.IpAddress = null;
                                dLog.DateTime = DateTime.Now;
                                dLog.HostName = Request.UserHostAddress + Request.UserHostName;
                                LogWriter.DataLogWriter(dLog);
                                #endregion
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                        else
                        {
                            showAlert("Subject grades update failed.");
                        }

                    }
                    else if (ResultType == "hsc")
                    {
                        // Prepare grade strings
                        List<string> gradePartsOne = new List<string>();
                        List<string> gradePartsTwo = new List<string>();
                        foreach (GridViewRow row in gvSubjectResult.Rows)
                        {
                            string subCode = ((Label)row.FindControl("lblSubectCode")).Text.Trim();
                            string grade = ((TextBox)row.FindControl("txtSubjectGarde")).Text.Trim();
                            string subName = ((Label)row.FindControl("lblSubSeq")).Text.Trim();

                            if (grade != "A+" && grade != "A" && grade != "A-" && grade != "B" && grade != "C" && grade != "D" && grade != "F")
                            {
                                showAlert("You are entering a invalid grade for Subject :" + subCode + " and grade : " + grade);
                                return;
                            }

                            if (subName == "One")
                            {
                                gradePartsOne.Add($"{subCode}:{grade}");
                            }
                            else if (subName == "Two")
                            {
                                gradePartsTwo.Add($"{subCode}:{grade}");
                            }
                        }
                                var logOldObject = db.AdmissionDB.GetSSCSubjectResultDataFromEducationDB(BoardT, Roll, RegNo, PassingYear).FirstOrDefault();

                        string GradeString = string.Join(",", gradePartsOne);
                        string GradeString2 = string.Join(",", gradePartsTwo);
                        var updateResult = db.AdmissionDB.UpdateHSCSubjectResultDataFromEducationDB(BoardT, Roll, RegNo, PassingYear,
                            GradeString,
                            GradeString2);
                        if (updateResult > 0)
                        {
                            showAlert("Subject grades updated successfully.");

                            try
                            {

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.EventName = "Update Grade Wise Board Result";
                                dLog.PageName = "SSCHSCBasicInfoDataUpdate.aspx";
                                dLog.OldData = ObjectToString.ConvertToString(logOldObject);
                                dLog.NewData = GradeString + "_" + GradeString2;
                                dLog.UserId = uId;
                                dLog.SessionInformation = "Updated subject wise grade for " + ResultType + " result for Roll : " + txtRoll.Text + " , RegNo : " + txtReg.Text
                                    + " , Board : " + ddlBoard.SelectedItem.ToString() + " , Passing Year : " + ddlPassYear.SelectedItem.ToString();
                                dLog.DateCreated = DateTime.Now;
                                dLog.IpAddress = null;
                                dLog.DateTime = DateTime.Now;
                                dLog.HostName = Request.UserHostAddress + Request.UserHostName;
                                LogWriter.DataLogWriter(dLog);
                                #endregion
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                        else
                        {
                            showAlert("Subject grades update failed.");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                showAlert("Something went wrong.Exception : " + ex.Message.ToString());
                return;
            }
        }
        #endregion
    }
}