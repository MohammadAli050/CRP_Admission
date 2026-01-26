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
    public partial class SSCHSCVarification : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
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
                        //string ApIResponse = TeletalkEducationBoard.GetData(ResultType, BoardT, Roll, PassingYear, RegNo);

                        string ApIResponse = null;

                        if (ResultType == "ssc")
                        {
                            try
                            {
                                ObjectResult<string> Result = db.AdmissionDB.GetStudentTeletalkSSCData(BoardT, Roll, RegNo, PassingYear);

                                var sscObj = Result.FirstOrDefault();

                                if (sscObj != null)
                                    ApIResponse = sscObj;

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (ResultType == "hsc")
                        {
                            try
                            {
                                ObjectResult<string> Result = db.AdmissionDB.GetStudentTeletalkHSCData(BoardT, Roll, RegNo, PassingYear);

                                var hscObj = Result.FirstOrDefault();

                                if (hscObj != null)
                                    ApIResponse = hscObj;

                            }
                            catch (Exception ex)
                            {
                            }
                        }


                        #region Update Json

                        if (ApIResponse != null && ApIResponse != "")
                        {
                            try
                            {
                                var CandidateExamDetails = db.AdmissionDB.ExamDetails.Where(x => x.EducationBoardID == BoardId && x.RollNo == Roll && x.RegistrationNo == RegNo && x.PassingYear == PassingYearId).ToList();
                                if (CandidateExamDetails != null && CandidateExamDetails.Any())
                                {
                                    foreach (var item in CandidateExamDetails)
                                    {
                                        try
                                        {
                                            item.JsonDataObject = ApIResponse;

                                            db.Update<DAL.ExamDetail>(item);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        #endregion

                        if (!string.IsNullOrEmpty(ApIResponse)) //!string.IsNullOrEmpty(sscResultTER) && 
                        {
                            if (ResultType == "ssc")
                            {
                                try
                                {
                                    TelitalkEducationResultModelSSC ResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelSSC>(ApIResponse);
                                    if (ResultModel != null && ResultModel.responseCode == 1)
                                    {
                                        panelInformation.Visible = true;
                                        lblFullResponse.Text = ApIResponse;
                                        lblName.Text = "Name : " + ResultModel.name;
                                        lblFatherName.Text = "Father's Name : " + ResultModel.father;
                                        lblMotherName.Text = "Mother's Name : " + ResultModel.mother;
                                        lblGender.Text = "Gender : " + ResultModel.gender;
                                        lblGroup.Text = "Group : " + ResultModel.studGroup;
                                        lblGpa.Text = "GPA : " + ResultModel.gpa;
                                        lblResult.Text = "Result : " + ResultModel.result;
                                        lblTotalMark.Text = "Total Mark : " + ResultModel.totalObtMark;

                                        if (ResultModel.subject != null && ResultModel.subject.Any())
                                        {
                                            gvSubjectResult.DataSource = ResultModel.subject.OrderBy(x => x.subCode);
                                            gvSubjectResult.DataBind();
                                        }
                                    }
                                    else
                                    {
                                        showAlert(ResultModel.responseDesc);
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    showAlert(ex.Message);
                                    return;
                                }
                            }
                            else if (ResultType == "hsc")
                            {
                                try
                                {
                                    TelitalkEducationResultModelHSC ResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelHSC>(ApIResponse);
                                    if (ResultModel != null && ResultModel.responseCode == 1)
                                    {
                                        panelInformation.Visible = true;
                                        lblFullResponse.Text = ApIResponse;
                                        lblName.Text = "Name : " + ResultModel.name;
                                        lblFatherName.Text = "Father's Name : " + ResultModel.father;
                                        lblMotherName.Text = "Mother's Name : " + ResultModel.mother;
                                        lblGender.Text = "Gender : " + ResultModel.gender;
                                        lblGroup.Text = "Group : " + ResultModel.studGroup;
                                        lblGpa.Text = "GPA : " + ResultModel.gpa;
                                        lblResult.Text = "Result : " + ResultModel.result;
                                        lblTotalMark.Text = "Total Mark : " + ResultModel.totalObtMark;

                                        if (ResultModel.subject != null && ResultModel.subject.Any())
                                        {
                                            gvSubjectResult.DataSource = ResultModel.subject.OrderBy(x => x.subCode);
                                            gvSubjectResult.DataBind();
                                        }
                                    }
                                    else
                                    {
                                        showAlert(ResultModel.responseDesc);
                                        return;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    showAlert(ex.Message);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            showAlert("Failed to get data from Taletalk API.Error is : " + ApIResponse);
                            return;
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
                lblName.Text = string.Empty;
                lblFatherName.Text = string.Empty;
                lblMotherName.Text = string.Empty;
                lblGender.Text = string.Empty;
                lblGroup.Text = string.Empty;
                lblGpa.Text = string.Empty;
                lblResult.Text = string.Empty;
                lblTotalMark.Text = string.Empty;
                lblFullResponse.Text = string.Empty;
                gvSubjectResult.DataSource = null;
                gvSubjectResult.DataBind();
                divResponse.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            try
            {
                divResponse.Visible = !divResponse.Visible;
            }
            catch (Exception ex)
            {
            }
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
    }
}