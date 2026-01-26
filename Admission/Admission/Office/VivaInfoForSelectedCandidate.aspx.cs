using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonUtility.FileConversion;


namespace Admission.Admission.Office
{
    public partial class VivaInfoForSelectedCandidate : PageBase
    {
        FileConversion aFileConverterObj = new FileConversion();
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //this.Page.Form.Enctype = "multipart/form-data";

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (uRole.ToLower() == "candidate")
            {

                SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                Response.Redirect("~/Admission/Home.aspx", false);
            }

            if (!IsPostBack)
            {
                LoadDDL();
            }
        }



        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            ReportViewer1.Visible = false;

            try
            {
                long admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                if (admUnitId < 0 || acaCalId < 0)
                {
                    panelShowHide.Visible = false;
                    MessageView("Select Session and Faculty !", "fail");
                    return;
                }

                panelShowHide.Visible = true;

                //List<DAL.VivaInfoSelectedCandidate> list = null;
                //using (var db = new OfficeDataManager())
                //{
                //    list = db.AdmissionDB.VivaInfoSelectedCandidates.Where(x => x.AcaCalId == acaCalId && x.AdmissionUnitId == admUnitId).ToList();
                //}

                //if (list != null && list.Count > 0)
                //{
                //    panelGridView.Visible = true;
                    
                //    gvVivaCandidateList.Visible = true;
                //    gvVivaCandidateList.DataSource = list;
                //    gvVivaCandidateList.DataBind();
                //}
                //else
                //{
                //    //panelGridView.Visible = false;                    

                //    //gvVivaCandidateList.DataSource = null;
                //    //gvVivaCandidateList.DataBind();
                //    MessageView("No Data Found !", "fail");
                //}


            }
            catch (Exception ex)
            {
                //panelGridView.Visible = false;
                panelShowHide.Visible = false;                

                MessageView("Error: Something went wrong in Loading Data. Exception: " + ex.Message.ToString(), "fail");
            }

        }

        protected void btnUploadExcel_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                if (admUnitId < 0 || acaCalId < 0)
                {
                    panelShowHide.Visible = false;
                    MessageView("Select Session and Faculty !", "fail");
                    return;
                }

                if (fuExcel.HasFile)
                {
                    btnLoad.Enabled = false;
                    btnUploadExcel.Enabled = false;

                    try
                    {
                        string FileName = Path.GetFileName(fuExcel.FileName);
                        SessionSGD.SaveObjToSession(FileName, "fileName");
                        if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                        {
                            //File Upload to Upload Temp folder Start
                            string UploadFileLocation = string.Empty;
                            UploadFileLocation = Server.MapPath("~/Upload/TEMP/");
                            try
                            {

                                string savePath = UploadFileLocation + FileName;
                                fuExcel.SaveAs(savePath);
                                //File Upload to Upload Temp folder End

                                //Read file from Upload temp folder
                                SheetName sheet = aFileConverterObj.PassFileName(savePath).FirstOrDefault();
                                //Populate datatable from excel sheet
                                DataTable dt = aFileConverterObj.ReadExcelFileDOM(savePath, FileName, sheet.Id);

                                //Remove First Row (Header)
                                DataRow row = dt.Rows[0];
                                dt.Rows.Remove(row);

                                int countNumber = 0;
                                string notAbleToGetDataFromExcel = "";
                                foreach (DataRow dtRow in dt.Rows)
                                {
                                    string serialT = null;
                                    string testRollT = null;
                                    string nameT = null;
                                    string dateT = null;
                                    string timeT = null;
                                    string facultyT = null;


                                    serialT = dtRow.ItemArray[0].ToString().Trim();
                                    testRollT = dtRow.ItemArray[1].ToString().Trim();
                                    nameT = dtRow.ItemArray[2].ToString().Trim();
                                    dateT = dtRow.ItemArray[3].ToString().Trim();
                                    timeT = dtRow.ItemArray[4].ToString().Trim();
                                    facultyT = dtRow.ItemArray[5].ToString().Trim();

                                    if (!string.IsNullOrEmpty(serialT) &&
                                        !string.IsNullOrEmpty(testRollT) &&
                                        !string.IsNullOrEmpty(nameT) &&
                                        !string.IsNullOrEmpty(dateT) &&
                                        !string.IsNullOrEmpty(timeT) &&
                                        !string.IsNullOrEmpty(facultyT))
                                    {
                                        string candidatePhoneNo = null;
                                        DateTime? candidateDBO = null;
                                        DAL.CandidatePayment cpModel = null;
                                        DAL.AdmissionTestRoll atrModel = null;
                                        DAL.Relation fatherRelation = null;
                                        using (var db = new CandidateDataManager())
                                        {
                                            atrModel = db.AdmissionDB.AdmissionTestRolls.Where(x => x.TestRoll == testRollT).First();
                                        }

                                        if (atrModel != null)
                                        {
                                            using (var db = new CandidateDataManager())
                                            {
                                                cpModel = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == atrModel.CandidateID).FirstOrDefault();

                                                List<DAL.Relation> relationList = db.GetAllRelationByCandidateID_AD(Convert.ToInt64(atrModel.CandidateID));
                                                if (relationList != null)
                                                {
                                                    fatherRelation = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();

                                                    if (!string.IsNullOrEmpty(relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault().BasicInfo.SMSPhone))
                                                    {
                                                        candidatePhoneNo = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault().BasicInfo.SMSPhone;
                                                    }

                                                    if (relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault().BasicInfo.DateOfBirth != null)
                                                    {
                                                        candidateDBO = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault().BasicInfo.DateOfBirth;
                                                    }
                                                }
                                            }
                                        }

                                        if (cpModel != null && atrModel != null && fatherRelation != null)
                                        {
                                            DAL.VivaInfoSelectedCandidate existModel = null;
                                            using (var db = new OfficeDataManager())
                                            {
                                                existModel = db.AdmissionDB.VivaInfoSelectedCandidates.Where(x => x.AdmissionUnitId == admUnitId
                                                                                                               && x.AcaCalId == acaCalId
                                                                                                               && x.RollNo == testRollT).FirstOrDefault();
                                            }

                                            if (existModel != null)
                                            {
                                                // Do Update

                                                double d = double.Parse(dateT);
                                                DateTime conv = DateTime.FromOADate(d);

                                                if (existModel.VivaDate != conv)
                                                {

                                                    //existModel.SerialId = serialT;
                                                    //existModel.RollNo = testRollT;
                                                    //existModel.CandidateName = nameT;

                                                    existModel.VivaDate = Convert.ToDateTime(conv);

                                                    existModel.VivaTime = timeT;
                                                    //existModel.FacultyName = facultyT;
                                                    //existModel.FatherName = fatherRelation.RelationDetail.Name;
                                                    //existModel.DBO = candidateDBO;
                                                    //existModel.PhoneNo = candidatePhoneNo;
                                                    //existModel.CandidateId = atrModel.CandidateID;
                                                    //existModel.PaymentId = cpModel.PaymentId;
                                                    existModel.ModifiedBy = uId;
                                                    existModel.ModifiedDate = DateTime.Now;

                                                    try
                                                    {

                                                        using (var db2 = new OfficeDataManager())
                                                        {
                                                            db2.Update<DAL.VivaInfoSelectedCandidate>(existModel);
                                                            countNumber++;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageView("Error-Insert: testRoll: " + testRollT + "; serial: " + serialT + "; " + "; name: " + nameT + "; " + "; faculty: " + facultyT + "; Exception: " + ex.Message.ToString(), "fail");
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Do Insert

                                                DAL.VivaInfoSelectedCandidate model = new DAL.VivaInfoSelectedCandidate();
                                                model.AdmissionUnitId = admUnitId;
                                                model.AcaCalId = acaCalId;
                                                model.SerialId = serialT;
                                                model.RollNo = testRollT;
                                                model.CandidateName = nameT;

                                                double d = double.Parse(dateT);
                                                DateTime conv = DateTime.FromOADate(d);
                                                model.VivaDate = Convert.ToDateTime(conv);

                                                model.VivaTime = timeT;
                                                model.FacultyName = facultyT;
                                                model.FatherName = fatherRelation.RelationDetail.Name;
                                                model.DBO = candidateDBO;
                                                model.PhoneNo = candidatePhoneNo;
                                                model.CandidateId = atrModel.CandidateID;
                                                model.PaymentId = cpModel.PaymentId;
                                                model.IsActive = true;
                                                model.CreatedBy = uId;
                                                model.CreatedDate = DateTime.Now;

                                                try
                                                {

                                                    using (var db2 = new OfficeDataManager())
                                                    {
                                                        db2.Insert<DAL.VivaInfoSelectedCandidate>(model);
                                                        countNumber++;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageView("Error-Insert: testRoll: " + testRollT + "; serial: " + serialT + "; " + "; name: " + nameT + "; " + "; faculty: " + facultyT + "; Exception: " + ex.Message.ToString(), "fail");
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }


                                if (countNumber > 0)
                                {
                                    btnLoad_Click(null, null);
                                    MessageView(countNumber + " Candidate Uploaded Successfully.", "success");
                                }
                                else
                                {
                                    MessageView(countNumber + " Candidate Uploaded Successfully.", "success");
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageView("Error-2: Something went wrong in reading data from Excel. Exception: " + ex.Message.ToString(), "fail"); return;
                            }
                        }
                        else
                        {
                            MessageView("Wrong File format. Please upload excel file with .xls or .xlsx extension!", "fail");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageView("Error-1: Something went wrong in reading data from Excel. Exception: " + ex.Message.ToString(), "fail");
                    }
                }
                else
                {
                    MessageView("Please Select a File for Upload !", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong in Upload Excel. Exception: " + ex.Message.ToString(), "fail");
            }
            finally
            {
                btnLoad.Enabled = true;
                btnUploadExcel.Enabled = true;

                //HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                //cookie.Value = "Flag";
                //cookie.Expires = DateTime.Now.AddDays(1);
                //Response.AppendCookie(cookie);
            }
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            //panelGridView.Visible = false;

            //gvVivaCandidateList.DataSource = null;
            //gvVivaCandidateList.DataBind();

            try
            {
                //long AdmUnitId = 3;
                //int AcaCalId = 46;
                //DateTime? StartDate = Convert.ToDateTime("2019-11-06");
                //DateTime? EndDate = Convert.ToDateTime("2019-11-06");

                string fileName = "";

                long AdmUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int AcaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                DateTime? StartDate = null;
                DateTime? EndDate = null;

                string vivaDate = txtDate.Text.Trim();
                if (!string.IsNullOrEmpty(vivaDate))
                {
                    StartDate = DateTime.ParseExact(vivaDate, "dd/MM/yyyy", null);
                    EndDate = DateTime.ParseExact(vivaDate, "dd/MM/yyyy", null);
                }

                if (AdmUnitId < 0 || AcaCalId < 0 || string.IsNullOrEmpty(vivaDate))
                {
                    //panelShowHide.Visible = false;
                    MessageView("Select Faculty, Session and Viva Date!", "fail");
                    return;
                }

                //fileName = ddlAdmUnit.SelectedItem.Text.Replace(" ", "_") + "_" + ddlSession.SelectedItem.Text.Replace(" ", "_");

                List<DAL.SPVivaInformation_Result> list = null;
                using (var db = new GeneralDataManager())
                {
                    list = db.AdmissionDB.SPVivaInformation(AdmUnitId, AcaCalId, StartDate, EndDate).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    ReportViewer1.LocalReport.EnableExternalImages = true;

                    ReportDataSource rds = new ReportDataSource("DataSet1", list);

                    //IList<ReportParameter> param1 = new List<ReportParameter>();
                    //param1.Add(new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text));
                    //param1.Add(new ReportParameter("CampusName", progBuildPrior.CampusName));
                    //param1.Add(new ReportParameter("BuildingName", progRoomObj.BuildingName));
                    //param1.Add(new ReportParameter("RoomName", progRoomObj.RoomName));
                    //param1.Add(new ReportParameter("RoomCap", progRoomObj.Capacity.ToString()));
                    //param1.Add(new ReportParameter("ExamDateTime", Convert.ToDateTime(admSetup.ExamDate).ToString("dd-MMM-yyyy") + ", " + admSetup.ExamTime));
                    //param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));

                    //ReportViewer1.LocalReport.SetParameters(param1);

                    //ReportViewer1.LocalReport.DisplayName = fileName;
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.Visible = true;
                }
                else
                {
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.Visible = false;
                    MessageView("No Data Found !", "fail");
                }
            }
            catch (Exception ex)
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.Visible = false;
                MessageView("Error: Something went wrong View Report. Exception: " + ex.Message.ToString(), "fail");
            }
        }
    }
}