using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTPrintCandidateAdmitCard : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        string userName = "";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            using (var db = new OfficeDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if (!IsPostBack)
            {
                hfCandidateID.Value = "0";
                //LoadDDL();

                Session["AdmitCardFileName"] = null;
                Session["AdmitCardData"] = null;


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

        private void ClearField()
        {
            hfCandidateID.Value = "0";
            txtPaymentId.Text = string.Empty;

            DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, null, "UnitName", "ID", EnumCollection.ListItemType.Select);
        }

        private void LoadAdmitCard(long candidateId, long admUnitId)
        {
            try
            {
                List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list = null;
                using (var db = new CandidateDataManager())
                {
                    list = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateID(candidateId, null, null).ToList();
                    list = list.Where(x => x.admUnitID == admUnitId).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    Session["AdmitCardData"] = null;
                    Session["AdmitCardData"] = list;


                    int acaCalId = list.FirstOrDefault().AcaCalID;
                    int eduCatId = list.FirstOrDefault().EducationCategoryID;
                    long admUnitID = list.FirstOrDefault().admUnitID;

                    List<DAL.ProgramPriority> choices = null;
                    using (var db = new CandidateDataManager())
                    {
                        choices = db.AdmissionDB.ProgramPriorities
                            .Where(c => c.CandidateID == candidateId
                                        && c.AcaCalID == acaCalId
                                        && c.AdmissionUnitID == admUnitID
                                        && c.Priority == 1)
                                .ToList();
                    }

                    ReportViewerBachelor.ShowExportControls = false;
                    ReportViewerBachelor.LocalReport.EnableExternalImages = true;
                    ReportViewerBachelor.LocalReport.DataSources.Clear();
                    ReportViewerBachelor.Visible = false;

                    ReportViewerMBAProfessional.ShowExportControls = false;
                    ReportViewerMBAProfessional.LocalReport.EnableExternalImages = true;
                    ReportViewerMBAProfessional.LocalReport.DataSources.Clear();
                    ReportViewerMBAProfessional.Visible = false;

                    ReportViewerLLMProfessional.ShowExportControls = false;
                    ReportViewerLLMProfessional.LocalReport.EnableExternalImages = true;
                    ReportViewerLLMProfessional.LocalReport.DataSources.Clear();
                    ReportViewerLLMProfessional.Visible = false;

                    ReportViewerMPCHRS.ShowExportControls = false;
                    ReportViewerMPCHRS.LocalReport.EnableExternalImages = true;
                    ReportViewerMPCHRS.LocalReport.DataSources.Clear();
                    ReportViewerMPCHRS.Visible = false;

                    ReportViewerMICT.ShowExportControls = false;
                    ReportViewerMICT.LocalReport.EnableExternalImages = true;
                    ReportViewerMICT.LocalReport.DataSources.Clear();
                    ReportViewerMICT.Visible = false;

                    ReportViewerMISS.ShowExportControls = false;
                    ReportViewerMISS.LocalReport.EnableExternalImages = true;
                    ReportViewerMISS.LocalReport.DataSources.Clear();
                    ReportViewerMISS.Visible = false;

                    ReportViewerMPH.ShowExportControls = false;
                    ReportViewerMPH.LocalReport.EnableExternalImages = true;
                    ReportViewerMPH.LocalReport.DataSources.Clear();
                    ReportViewerMPH.Visible = false;

                    ReportViewerHospitalManagement.ShowExportControls = false;
                    ReportViewerHospitalManagement.LocalReport.EnableExternalImages = true;
                    ReportViewerHospitalManagement.LocalReport.DataSources.Clear();
                    ReportViewerHospitalManagement.Visible = false;

                    ReportViewerMCSE.ShowExportControls = false;
                    ReportViewerMCSE.LocalReport.EnableExternalImages = true;
                    ReportViewerMCSE.LocalReport.DataSources.Clear();
                    ReportViewerMCSE.Visible = false;

                    ReportViewerMDSProfessional.ShowExportControls = false;
                    ReportViewerMDSProfessional.LocalReport.EnableExternalImages = true;
                    ReportViewerMDSProfessional.LocalReport.DataSources.Clear();
                    ReportViewerMDSProfessional.Visible = false;

                    ReportViewerMCS.ShowExportControls = false;
                    ReportViewerMCS.LocalReport.EnableExternalImages = true;
                    ReportViewerMCS.LocalReport.DataSources.Clear();
                    ReportViewerMCS.Visible = false;

                    ReportViewerMESM.ShowExportControls = false;
                    ReportViewerMESM.LocalReport.EnableExternalImages = true;
                    ReportViewerMESM.LocalReport.DataSources.Clear();
                    ReportViewerMESM.Visible = false;

                    #region Get Data Paramiter
                    string imgUrl = new Uri(Server.MapPath(list.FirstOrDefault().PhotoPath)).AbsoluteUri;
                    string SignatureUrl = new Uri(Server.MapPath(list.FirstOrDefault().SignPath)).AbsoluteUri;
                    //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;

                    DAL.Room roomModel = null;
                    string floorName = "-";
                    using (var db = new GeneralDataManager())
                    {
                        int roomId = (int)list.FirstOrDefault().prpRoomID;
                        roomModel = db.AdmissionDB.Rooms.Where(x => x.ID == roomId).FirstOrDefault();
                    }
                    if (roomModel != null)
                    {
                        floorName = roomModel.FloorNumber.ToString();
                    }


                    //string examCenter = admitCardDetails.prpRoomName + ", " + floorName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                    //string examCenter = list.FirstOrDefault().campusName;
                    string examCenter = list.FirstOrDefault().prpRoomName + ", " + list.FirstOrDefault().buildingName + ", " + list.FirstOrDefault().campusName;

                    DateTime examDate = Convert.ToDateTime(list.FirstOrDefault().ExamDate);
                    #endregion

                    ReportDataSource rds = new ReportDataSource("DataSet1", list.Where(c => c.AcaCalID == acaCalId
                                                                                            && c.admUnitID == admUnitId
                                                                                            && c.IsPaid == true
                                                                                            && c.IsApproved == true).ToList());
                    IList<ReportParameter> param1 = new List<ReportParameter>();
                    param1.Add(new ReportParameter("FacultyName", list.FirstOrDefault().UnitName));
                    param1.Add(new ReportParameter("CandidateName", list.FirstOrDefault().FirstName));
                    param1.Add(new ReportParameter("CandidateImagePath", imgUrl));
                    param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl));
                    param1.Add(new ReportParameter("FatherName", list.FirstOrDefault().FatherName != null ? list.FirstOrDefault().FatherName.ToUpper() : ""));
                    param1.Add(new ReportParameter("RollNumber", list.FirstOrDefault().TestRoll != null ? list.FirstOrDefault().TestRoll : ""));
                    param1.Add(new ReportParameter("ExamCenter", examCenter));
                    param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().ExamTime.ToString()));
                    param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));


                    #region Admit Card Instruction Parameter


                    try
                    {
                        using (var db = new GeneralDataManager())
                        {
                            var insSetupObj = db.AdmissionDB.AdmitCardInstructions.Where(x => x.AcacalId == acaCalId
                            && x.AdmissionUnitId == admUnitId).FirstOrDefault();

                            if (insSetupObj != null)
                            {
                                param1.Add(new ReportParameter("Instruction", insSetupObj.InstructionDetails.ToString()));
                            }
                            else
                            {
                                param1.Add(new ReportParameter("Instruction", ""));
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    if (choices != null)
                    {
                        if (choices.Count() > 1)
                        {
                            //lblMessage.Text = "More than 1 program is selected as Choice 1. Please select only 1 choice for this faculty.";
                            //lblMessage.ForeColor = Color.Crimson;
                            //messagePanel.CssClass = "alert alert-danger";
                            //messagePanel.Visible = true;

                            MessageView("More than 1 program is selected as Choice 1. Please select only 1 choice for this faculty.", "fail");

                            return;
                        }
                        else if (choices.Count() == 1)
                        {

                            //#region Insurt Log when Candidate Click Download Button for Admit Card
                            //int educationCategoryId = admitCardDetails.EducationCategoryID;
                            //AdmitCardDownloadClickLog(paymentId, acaCalId, admUnitId, testRoll, educationCategoryId, cId, admSetId);
                            //#endregion


                            #region Load Report and Download

                            if (eduCatId > 0 && eduCatId == 4)
                            {
                                ReportViewerBachelor.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCard.rdlc");

                                ReportViewerBachelor.LocalReport.SetParameters(param1);

                                ReportViewerBachelor.LocalReport.DataSources.Clear();
                                ReportViewerBachelor.LocalReport.DataSources.Add(rds);
                                ReportViewerBachelor.Visible = true;


                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Save Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerBachelor.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerBachelor.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();


                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName)))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card (Admin)";
                                //            dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //} 

                                #endregion

                            }
                            else
                            {
                                //lblMessage.Text = "Education Category did not match for Bachelor !";
                                //lblMessage.ForeColor = Color.Crimson;
                                //messagePanel.CssClass = "alert alert-danger";
                                //messagePanel.Visible = true;

                                MessageView("Education Category did not match for Bachelor !", "fail");

                                return;
                            }
                            #endregion
                        }
                        else if (choices.Count() == 0)
                        {
                            #region Load Report and Download  MASTERS

                            if (admUnitId == 10) //MBA (Regular) [For this Faculty Admit Card Will Be Bachelor Admit Card]
                            {
                                ReportViewerBachelor.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCard.rdlc");

                                ReportViewerBachelor.LocalReport.SetParameters(param1);

                                ReportViewerBachelor.LocalReport.DataSources.Clear();
                                ReportViewerBachelor.LocalReport.DataSources.Add(rds);
                                ReportViewerBachelor.Visible = true;

                                //ReportViewerMasters.Visible = false;


                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion


                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerBachelor.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerBachelor.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();


                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCard.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion

                            }
                            else if (admUnitId == 6) //Master of Business Administration (Professional)
                            {
                                ReportViewerMBAProfessional.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMBAProfessional.rdlc");

                                ReportViewerMBAProfessional.LocalReport.SetParameters(param1);

                                ReportViewerMBAProfessional.LocalReport.DataSources.Clear();
                                ReportViewerMBAProfessional.LocalReport.DataSources.Add(rds);
                                ReportViewerMBAProfessional.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMBAProfessional.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMBAProfessional.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCard.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion
                            }
                            else if (admUnitId == 9) //Master of Laws (LLM-Professional)
                            {
                                ReportViewerLLMProfessional.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardLLMProfessional.rdlc");

                                ReportViewerLLMProfessional.LocalReport.SetParameters(param1);

                                ReportViewerLLMProfessional.LocalReport.DataSources.Clear();
                                ReportViewerLLMProfessional.LocalReport.DataSources.Add(rds);
                                ReportViewerLLMProfessional.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerLLMProfessional.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerLLMProfessional.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCard.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion
                            }
                            else if (admUnitId == 18) //Master of Peace, Conflict and Human Rights Studies
                            {
                                ReportViewerMPCHRS.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMPCHRS.rdlc");

                                // added for only one semester by asad bhai request

                                if (list.FirstOrDefault().AcaCalID == 1065)
                                {
                                    try
                                    {
                                        int Roll = Convert.ToInt32(list.FirstOrDefault().TestRoll.Substring(list.FirstOrDefault().TestRoll.Length - 5));
                                        if (Roll <= 75)
                                        {
                                            DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                            param1.Add(new ReportParameter("VivaDateTime", "20-Dec-2024, 09:00 AM"));
                                        }
                                        else
                                        {
                                            DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                            param1.Add(new ReportParameter("VivaDateTime", "21-Dec-24, 09:00 AM"));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                else
                                {
                                    DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                    param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));
                                }

                                ReportViewerMPCHRS.LocalReport.SetParameters(param1);

                                ReportViewerMPCHRS.LocalReport.DataSources.Clear();
                                ReportViewerMPCHRS.LocalReport.DataSources.Add(rds);
                                ReportViewerMPCHRS.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMPCHRS.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMPCHRS.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCard.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion
                            }
                            else if (admUnitId == 12) //MSc in Information & Communication Technology (MICT)
                            {
                                ReportViewerMICT.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMICT.rdlc");
                                DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));

                                ReportViewerMICT.LocalReport.SetParameters(param1);

                                ReportViewerMICT.LocalReport.DataSources.Clear();
                                ReportViewerMICT.LocalReport.DataSources.Add(rds);
                                ReportViewerMICT.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMICT.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMICT.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCardV2.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion



                            }
                            else if (admUnitId == 11) //Master of Information Systems Security (MISS)
                            {
                                ReportViewerMISS.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMISS.rdlc");
                                DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));

                                ReportViewerMISS.LocalReport.SetParameters(param1);

                                ReportViewerMISS.LocalReport.DataSources.Clear();
                                ReportViewerMISS.LocalReport.DataSources.Add(rds);
                                ReportViewerMISS.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {
                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMISS.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMISS.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCardV2.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion


                            }
                            else if (admUnitID == 22) //Masters in Environmental Science and Management (Professional)
                            {
                                ReportViewerMESM.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMESM.rdlc");
                                DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));

                                ReportViewerMESM.LocalReport.SetParameters(param1);

                                ReportViewerMESM.LocalReport.DataSources.Clear();
                                ReportViewerMESM.LocalReport.DataSources.Add(rds);
                                ReportViewerMESM.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {
                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMESM.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMESM.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCardV2.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion


                            }
                            else if (admUnitId == 19) //Master of Public Health (MPH)
                            {
                                ReportViewerMPH.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMPH.rdlc");
                                DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));


                                ReportViewerMPH.LocalReport.SetParameters(param1);

                                ReportViewerMPH.LocalReport.DataSources.Clear();
                                ReportViewerMPH.LocalReport.DataSources.Add(rds);
                                ReportViewerMPH.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {
                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMPH.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMPH.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCardV2.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion


                            }

                            else if (admUnitId == 20) //Master of Computer Science (MCSE)
                            {
                                ReportViewerMCSE.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMCSE.rdlc");
                                DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));

                                ReportViewerMCSE.LocalReport.SetParameters(param1);

                                ReportViewerMCSE.LocalReport.DataSources.Clear();
                                ReportViewerMCSE.LocalReport.DataSources.Add(rds);
                                ReportViewerMCSE.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {
                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMCSE.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMCSE.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCardV2.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion


                            }

                            else if (admUnitId == 21) //MCS
                            {
                                ReportViewerMCS.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMCS.rdlc");

                                ReportViewerMCS.LocalReport.SetParameters(param1);

                                ReportViewerMCS.LocalReport.DataSources.Clear();
                                ReportViewerMCS.LocalReport.DataSources.Add(rds);
                                ReportViewerMCS.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMCS.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMCS.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCard.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion
                            }

                            else if (admUnitId == 24) //Certificate Course in Hospital Management
                            {
                                ReportViewerHospitalManagement.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardHospitalM.rdlc");
                                DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));


                                ReportViewerHospitalManagement.LocalReport.SetParameters(param1);

                                ReportViewerHospitalManagement.LocalReport.DataSources.Clear();
                                ReportViewerHospitalManagement.LocalReport.DataSources.Add(rds);
                                ReportViewerHospitalManagement.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {
                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerHospitalManagement.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerHospitalManagement.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }

                                    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                    //Response.ClearHeaders();
                                    //Response.ClearContent();
                                    //Response.Buffer = true;
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                    //Response.Flush();
                                    ////Response.Close();
                                    //Response.End();

                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";
                                    //messagePanel.Visible = true;

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }
                                //finally
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                //        {
                                //            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //        try
                                //        {
                                //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //            dLog.EventName = "Download Admit Card";
                                //            dLog.PageName = "AdmitCardV2.aspx";
                                //            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                //            dLog.UserId = uId;
                                //            dLog.Attribute1 = "Failed";
                                //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //            dLog.DateCreated = DateTime.Now;
                                //            LogWriter.DataLogWriter(dLog);
                                //        }
                                //        catch (Exception ext)
                                //        {
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion


                            }



                            else if (admUnitId == 13) //MDS (Professional)
                            {
                                ReportViewerMDSProfessional.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMDSProfessional.rdlc");

                                ReportViewerMDSProfessional.LocalReport.SetParameters(param1);

                                ReportViewerMDSProfessional.LocalReport.DataSources.Clear();
                                ReportViewerMDSProfessional.LocalReport.DataSources.Add(rds);
                                ReportViewerMDSProfessional.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Download Admit Card (Admin)";
                                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Success";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                #region Download Admit Card

                                string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                Session["AdmitCardFileName"] = null;
                                Session["AdmitCardFileName"] = admitCardFileName;

                                try
                                {

                                    Warning[] warnings;
                                    string[] streamids;
                                    string mimeType;
                                    string encoding;
                                    string filenameExtension;

                                    byte[] bytes = ReportViewerMDSProfessional.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                    ReportViewerMDSProfessional.LocalReport.Refresh();

                                    using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName), FileMode.Create))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }


                                }
                                catch (Exception ex)
                                {

                                    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card (Admin)";
                                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                        dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        dLog.HostName = Request.UserHostName;
                                        dLog.IpAddress = Request.UserHostAddress;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion

                                    return;
                                }

                                #endregion
                            }

                            else
                            {
                                //lblMessage.Text = "Invalid Request for Download Admit Card !";
                                //lblMessage.ForeColor = Color.Crimson;
                                //messagePanel.CssClass = "alert alert-danger";
                                //messagePanel.Visible = true;

                                MessageView("Invalid Request for Download Admit Card !", "fail");
                                return;
                            }




                            #endregion
                        }
                    }
                    else
                    {
                        //lblMessage.Text = "Error: Unable to get candidate programs";
                        //lblMessage.ForeColor = Color.Crimson;
                        //messagePanel.CssClass = "alert alert-danger";
                        //messagePanel.Visible = true;

                        MessageView("Error: Unable to get candidate programs!", "fail");
                        return;
                    }


                }
                else
                {
                    MessageView("Failed to get candidate admit card information!", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }


            #region N/A
            //MessageView("", "clear");

            //try
            //{
            //    bool instructionImageHS = false;
            //    bool importantInstructionHS = false;

            //    int programId = -1;
            //    if (candidateId > 0)
            //    {
            //        List<DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result> admitCardDetailsList = null;

            //        try
            //        {
            //            using (var db = new OfficeDataManager())
            //            {
            //                admitCardDetailsList = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateIDForAdmin(candidateId).ToList();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageView("Error getting admit card info: " + ex.Message, "fail");
            //            return;
            //        }


            //        DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result admitCardDetails = null;
            //        try
            //        {
            //            if (admitCardDetailsList != null && admitCardDetailsList.Count > 0)
            //            {
            //                using (var db = new GeneralDataManager())
            //                {
            //                    List<DAL.AdmissionUnit> admUnitList = (from acdl in admitCardDetailsList
            //                                                           join admUnit in db.AdmissionDB.AdmissionUnits on acdl.admUnitID equals admUnit.ID
            //                                                           orderby admUnit.UnitName
            //                                                           select admUnit).ToList();


            //                    if (admUnitList != null && admUnitList.Count > 0)
            //                    {
            //                        DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, admUnitList.OrderBy(a => a.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
            //                    }
            //                }


            //                if (admUnitId > 0)
            //                {
            //                    admitCardDetails = admitCardDetailsList.Where(x => x.admUnitID == admUnitId).FirstOrDefault();
            //                }
            //                else
            //                {
            //                    admitCardDetails = admitCardDetailsList.Where(x => x.TestRoll != null).FirstOrDefault();
            //                }


            //            }
            //        }
            //        catch (Exception)
            //        {

            //        }




            //        //DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result admitCardDetails = null;
            //        //try
            //        //{
            //        //    using (var db = new OfficeDataManager())
            //        //    {
            //        //        admitCardDetails = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateIDForAdmin(candidateId).FirstOrDefault();
            //        //    }
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    lblMessage.Text = "Error getting admit card info: " + ex.Message;
            //        //    messagePanel.CssClass = "alert alert-danger";
            //        //    messagePanel.Visible = true;
            //        //}

            //        List<DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result> list =
            //            new List<DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result>();

            //        list.Add(admitCardDetails);

            //        if (admitCardDetails != null)
            //        {

            //            DAL.CandidateFormSl formSL = null;
            //            DAL.AdmissionSetup admSet = null;
            //            DAL.AdmissionUnitProgram admUnitProg = null;
            //            using (var db = new GeneralDataManager())
            //            {
            //                formSL = db.AdmissionDB.CandidateFormSls.Where(x => x.FormSerial == admitCardDetails.FormSerial).FirstOrDefault();
            //                if (formSL != null)
            //                {
            //                    admSet = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == formSL.AdmissionSetupID).FirstOrDefault();
            //                    if (admSet != null)
            //                    {
            //                        admUnitProg = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.IsActive == true
            //                                                                                  && x.EducationCategoryID == admSet.EducationCategoryID
            //                                                                                  && x.AcaCalID == admSet.AcaCalID
            //                                                                                  && x.AdmissionUnitID == admSet.AdmissionUnitID).FirstOrDefault();

            //                        if (admUnitProg != null)
            //                        {
            //                            programId = admUnitProg.ProgramID;
            //                        }
            //                    }
            //                }
            //            }

            //            if (programId > 0 && programId == 7) // EMBA Pro. = 7; jnno Image Isntruction ta dekhabe
            //            {
            //                instructionImageHS = true;
            //                importantInstructionHS = false;
            //            }
            //            else
            //            {
            //                instructionImageHS = false;
            //                importantInstructionHS = true;
            //            }


            //            string msgData = string.Empty;

            //            if (string.IsNullOrEmpty(admitCardDetails.TestRoll))
            //            {
            //                msgData += "Test roll not available; ";
            //            }
            //            if (admitCardDetails.IsFinalSubmit == false)
            //            {
            //                msgData += "Not final submitted.";
            //            }

            //            if (!string.IsNullOrEmpty(msgData))
            //            {
            //                lblMessage.Text = msgData;
            //                messagePanel.CssClass = "alert alert-danger";
            //                messagePanel.Visible = true;
            //            }

            //            try
            //            {
            //                ReportViewer1.LocalReport.EnableExternalImages = true;
            //                ReportViewerMasters.LocalReport.EnableExternalImages = true;

            //                ReportDataSource rds = new ReportDataSource("DataSet1", list);

            //                string imgUrl = new Uri(Server.MapPath(admitCardDetails.PhotoPath)).AbsoluteUri;
            //                string SignatureUrl = new Uri(Server.MapPath(admitCardDetails.SignPath)).AbsoluteUri;
            //                //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
            //                //string examCenter = admitCardDetails.campusName;



            //                DAL.Room roomModel = null;
            //                string floorName = "-";
            //                using (var db = new GeneralDataManager())
            //                {
            //                    roomModel = db.AdmissionDB.Rooms.Where(x => x.ID == admitCardDetails.prpRoomID).FirstOrDefault();
            //                }
            //                if (roomModel != null)
            //                {
            //                    floorName = roomModel.FloorNumber.ToString();
            //                }


            //                //string examCenter = admitCardDetails.prpRoomName + ", " + floorName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
            //                string examCenter = admitCardDetails.campusName;


            //                DateTime examDate = Convert.ToDateTime(admitCardDetails.ExamDate);

            //                IList<ReportParameter> param1 = new List<ReportParameter>();
            //                param1.Add(new ReportParameter("FacultyName", admitCardDetails.UnitName));
            //                param1.Add(new ReportParameter("CandidateName", admitCardDetails.FirstName));
            //                param1.Add(new ReportParameter("CandidateImagePath", imgUrl));
            //                param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl));
            //                param1.Add(new ReportParameter("FatherName", admitCardDetails.FatherName != null ? admitCardDetails.FatherName.ToUpper() : ""));
            //                param1.Add(new ReportParameter("RollNumber", admitCardDetails.TestRoll != null ? admitCardDetails.TestRoll : ""));
            //                param1.Add(new ReportParameter("ExamCenter", examCenter));
            //                param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
            //                param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
            //                param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));
            //                param1.Add(new ReportParameter("InstructionImageHS", instructionImageHS.ToString()));
            //                param1.Add(new ReportParameter("ImportantInstructionHS", importantInstructionHS.ToString()));



            //                int eduCatId = -1;
            //                using (var db = new CandidateDataManager())
            //                {
            //                    eduCatId = db.GetCandidateEducationCategoryID(candidateId);
            //                }
            //                if (eduCatId > 0)
            //                {
            //                    if (eduCatId == 4)
            //                    {
            //                        ReportViewer1.LocalReport.SetParameters(param1);

            //                        ReportViewer1.LocalReport.DataSources.Clear();
            //                        ReportViewer1.LocalReport.DataSources.Add(rds);
            //                        ReportViewer1.Visible = true;

            //                        ReportViewerMasters.Visible = false;
            //                    }
            //                    else
            //                    {
            //                        ReportViewerMasters.LocalReport.SetParameters(param1);

            //                        ReportViewerMasters.LocalReport.DataSources.Clear();
            //                        ReportViewerMasters.LocalReport.DataSources.Add(rds);
            //                        ReportViewerMasters.Visible = true;

            //                        ReportViewer1.Visible = false;
            //                    }
            //                }
            //                else
            //                {
            //                    ReportViewer1.LocalReport.SetParameters(param1);

            //                    ReportViewer1.LocalReport.DataSources.Clear();
            //                    ReportViewer1.LocalReport.DataSources.Add(rds);
            //                    ReportViewer1.Visible = true;

            //                    ReportViewerMasters.Visible = false;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                lblMessage1.Text = ex.Message;
            //                messagePanel1.CssClass = "alert alert-danger";
            //                messagePanel1.Visible = true;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblMessage1.Text = ex.Message;
            //    messagePanel1.CssClass = "alert alert-danger";
            //    messagePanel1.Visible = true;
            //} 
            #endregion
        }

        #region N/A -- btnMobileSearch_Click
        //protected void btnMobileSearch_Click(object sender, EventArgs e)
        //{
        //    string mobileNo = null;
        //    mobileNo = txtMobile.Text;

        //    List<DAL.BasicInfo> candidateList = null;
        //    if (!string.IsNullOrEmpty(mobileNo))
        //    {
        //        try
        //        {
        //            using (var db = new CandidateDataManager())
        //            {
        //                candidateList = db.AdmissionDB.BasicInfoes
        //                    .Where(c => c.SMSPhone == mobileNo)
        //                    .ToList();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            lblMessage.Text = "Error getting candidate object: " + ex.Message;
        //            messagePanel.CssClass = "alert alert-danger";
        //            messagePanel.Visible = true;
        //        }
        //    }

        //    DAL.BasicInfo candidateObj = null;
        //    if (candidateList != null)
        //    {
        //        if (candidateList.Count > 1)
        //        {
        //            lblMessage.Text = "Multiple candidate exist with mobile number " + txtMobile.Text;
        //            messagePanel.CssClass = "alert alert-danger";
        //            messagePanel.Visible = true;
        //        }
        //        else if (candidateList.Count == 1)
        //        {
        //            foreach (DAL.BasicInfo item in candidateList)
        //            {
        //                candidateObj = item;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        lblMessage.Text = "Candidate does not exist with mobile number " + txtMobile.Text;
        //        messagePanel.CssClass = "alert alert-danger";
        //        messagePanel.Visible = true;
        //        return;
        //    }

        //    if (candidateObj != null)
        //    {
        //        LoadAdmitCard(candidateObj.ID, 0);
        //    }

        //} 
        #endregion

        protected void btnPaymentIdSearch_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            hfCandidateID.Value = "0";

            try
            {
                if (!string.IsNullOrEmpty(txtPaymentId.Text.Trim()) || Convert.ToInt64(txtPaymentId.Text.Trim()) > 0)
                {

                    //bool containsAlphabet = false;

                    //foreach (char c in txtPaymentId.Text)
                    //{
                    //    if (char.IsLetter(c))
                    //    {
                    //        containsAlphabet = true;
                    //        break; // No need to check further once a letter is found
                    //    }
                    //}



                    DAL.CandidatePayment cp = null;
                    DAL.CandidateUser cu = null;
                    DAL.BasicInfo bi = null;
                    DAL.AdmissionTestRoll atr = null;


                    if (txtPaymentId.Text.Any(char.IsLetter))
                    {

                        string userName = txtPaymentId.Text;
                        using (var db = new CandidateDataManager())
                        {
                            cu = db.AdmissionDB.CandidateUsers.Where(x => x.UsernameLoginId == userName && x.IsActive == true).FirstOrDefault();
                            int cand_user_ID = Convert.ToInt32(cu.ID);

                            bi = db.AdmissionDB.BasicInfoes.Where(x => x.CandidateUserID == cand_user_ID).FirstOrDefault();
                            int candidateID = Convert.ToInt32(bi.ID);

                            cp = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == candidateID).FirstOrDefault();
                        }
                    }

                    else
                    {
                        long paymentId = Convert.ToInt64(txtPaymentId.Text.Trim());
                        string TestRoll = txtPaymentId.Text;

                        //  DAL.CandidatePayment cp = null;
                        using (var db = new CandidateDataManager())
                        {
                            cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == paymentId).FirstOrDefault();

                            if (cp == null)
                            {
                                atr = db.AdmissionDB.AdmissionTestRolls.Where(x => x.TestRoll == TestRoll).FirstOrDefault();

                                int candID = Convert.ToInt32(atr.CandidateID);

                                cp = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == candID).FirstOrDefault();

                            }
                        }

                    }
                    if (cp != null)
                    {
                        hfCandidateID.Value = cp.CandidateID.ToString();

                        List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();

                        List<DAL.CandidateFormSl> formSLList = null;
                        using (var db = new CandidateDataManager())
                        {
                            formSLList = db.GetAllCandidateFormSlByCandID_AD(Convert.ToInt64(cp.CandidateID)).ToList();

                            if (formSLList != null && formSLList.Count > 0)
                            {
                                foreach (var tData in formSLList)
                                {
                                    admUnitList.Add(tData.AdmissionSetup.AdmissionUnit);
                                }

                                if (admUnitList != null && admUnitList.Count > 0)
                                {
                                    DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, admUnitList, "UnitName", "ID", EnumCollection.ListItemType.Select);
                                }
                                else
                                {
                                    DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, admUnitList, "UnitName", "ID", EnumCollection.ListItemType.Select);
                                }
                            }
                            else
                            {
                                MessageView("No Data Found!", "fail");
                            }
                        }
                    }
                    else
                    {
                        MessageView("No Candidate Found!", "fail");
                    }
                }
                else
                {
                    MessageView("Invalid Input. Please provide valid input!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }

            #region N/A
            //long paymentId = -1;
            //paymentId = Convert.ToInt64(txtPaymentId.Text);

            //DAL.CandidatePayment cPayment = null;
            //if (paymentId > 0)
            //{
            //    try
            //    {
            //        using (var db = new CandidateDataManager())
            //        {
            //            cPayment = db.AdmissionDB.CandidatePayments
            //                .Where(c => c.PaymentId == paymentId)
            //                .FirstOrDefault();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        lblMessage.Text = "Error getting candidate payment object: " + ex.Message;
            //        messagePanel.CssClass = "alert alert-danger";
            //        messagePanel.Visible = true;
            //    }
            //}

            //if (cPayment != null)
            //{
            //    LoadAdmitCard(Convert.ToInt64(cPayment.CandidateID), 0);
            //}
            //else
            //{
            //    lblMessage.Text = "Candidate payment does not exist for payment id: " + txtPaymentId.Text;
            //    messagePanel.CssClass = "alert alert-danger";
            //    messagePanel.Visible = true;
            //    return;
            //} 
            #endregion
        }

        protected void ddlAdmissionUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");
            try
            {
                if ((!string.IsNullOrEmpty(txtPaymentId.Text.Trim()) || Convert.ToInt64(txtPaymentId.Text.Trim()) > 0)
                    && Convert.ToInt64(ddlAdmissionUnit.SelectedValue) > 0
                    && (!string.IsNullOrEmpty(hfCandidateID.Value) && Convert.ToInt64(hfCandidateID.Value) > 0))
                {
                    long candidateId = Convert.ToInt64(hfCandidateID.Value);
                    long admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);

                    LoadAdmitCard(candidateId, admUnitId);
                }
                else
                {
                    MessageView("Please Provide PaymentID and Select Faculty for Load Admit Card!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }


            #region N/A
            //try
            //{
            //    long admUnitId = -1;
            //    if (!string.IsNullOrEmpty(ddlAdmissionUnit.SelectedItem.Text))
            //    {
            //        admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            //    }


            //    long paymentId = -1;
            //    paymentId = Convert.ToInt64(txtPaymentId.Text);

            //    DAL.CandidatePayment cPayment = null;
            //    if (paymentId > 0)
            //    {
            //        try
            //        {
            //            using (var db = new CandidateDataManager())
            //            {
            //                cPayment = db.AdmissionDB.CandidatePayments
            //                    .Where(c => c.PaymentId == paymentId)
            //                    .FirstOrDefault();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            lblMessage.Text = "Error getting candidate payment object: " + ex.Message;
            //            messagePanel.CssClass = "alert alert-danger";
            //            messagePanel.Visible = true;
            //        }
            //    }

            //    if (cPayment != null && admUnitId > 0)
            //    {
            //        LoadAdmitCard(Convert.ToInt64(cPayment.CandidateID), admUnitId);
            //    }

            //}
            //catch (Exception ex)
            //{

            //} 
            #endregion
        }

        protected void btnDownloadAdmitCard_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AdmitCardFileName"] != null && Session["AdmitCardData"] != null)
                {
                    List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list = (List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result>)Session["AdmitCardData"];

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "Download Admit Card (Admin)";
                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                        dLog.NewData = "User: " + userName + ", Downloaded the Admit Card Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                                        "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                                        "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                                        "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                        dLog.UserId = uId;
                        dLog.Attribute1 = "Success";
                        dLog.HostName = Request.UserHostName;
                        dLog.IpAddress = Request.UserHostAddress;
                        dLog.DateCreated = DateTime.Now;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + hfCandidateID.Value;
                        dLog.DateCreated = DateTime.Now;
                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    #region Insurt Log when Candidate Click Download Button for Admit Card
                    try
                    {
                        int educationCategoryId = list.FirstOrDefault().EducationCategoryID;
                        long admSetId = -1;
                        long candidateId = Convert.ToInt64(hfCandidateID.Value);
                        long formSerl = list.FirstOrDefault().FormSerial;
                        int acaCalId = list.FirstOrDefault().AcaCalID;
                        using (var db = new GeneralDataManager())
                        {
                            DAL.CandidateFormSl formSer = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == candidateId && x.FormSerial == formSerl && x.AcaCalID == acaCalId).FirstOrDefault();

                            admSetId = formSer.AdmissionSetupID;
                        }

                        AdmitCardDownloadClickLog(list.FirstOrDefault().PaymentId, list.FirstOrDefault().AcaCalID, list.FirstOrDefault().admUnitID, list.FirstOrDefault().TestRoll, educationCategoryId, candidateId, admSetId);
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion


                    string admitCardFileName = (string)Session["AdmitCardFileName"];

                    System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + admitCardFileName);
                    Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName));
                    Response.Flush();
                    //Response.Close();
                    Response.End();
                }
                else
                {
                    MessageView("Invalid Request! Failed To Download Admit Card From Admin!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
            finally
            {
                try
                {
                    string admitCardFileName = (string)Session["AdmitCardFileName"];

                    if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName)))
                    {
                        File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/AdminDownload/" + admitCardFileName));
                    }
                }
                catch (Exception ex)
                {
                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "Download Admit Card (Admin)";
                        dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                        dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                        dLog.UserId = uId;
                        dLog.Attribute1 = "Failed";
                        dLog.HostName = Request.UserHostName;
                        dLog.IpAddress = Request.UserHostAddress;
                        dLog.DateCreated = DateTime.Now;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + hfCandidateID.Value;
                        dLog.DateCreated = DateTime.Now;
                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ext)
                    {
                    }
                    #endregion
                }
            }
        }


        private void AdmitCardDownloadClickLog(long? paymentId, int acaCalId, long admUnitId, string testRoll, int educationCategoryId, long cId, long admSetId)
        {
            try
            {
                if (paymentId > 0 && acaCalId > 0 && admUnitId > 0 && testRoll != "" && educationCategoryId > 0 && admSetId > 0)
                {
                    DAL_Log.CandidateAdmitCardDownloadClickLog cacdcl = new DAL_Log.CandidateAdmitCardDownloadClickLog();
                    cacdcl.PaymentId = paymentId;
                    cacdcl.AcaCalId = acaCalId;
                    cacdcl.AdmissionUnitId = admUnitId;
                    cacdcl.TestRoll = testRoll;
                    cacdcl.EducationCategoryId = educationCategoryId;
                    cacdcl.CreatedBy = Convert.ToInt32(cId);
                    cacdcl.CreatedDate = DateTime.Now;
                    cacdcl.AdmissionSetupId = admSetId;
                    cacdcl.Attribute1 = "User: " + userName + ", Downloaded The Admit Card";

                    using (var db = new LogDataManager())
                    {
                        db.Insert<DAL_Log.CandidateAdmitCardDownloadClickLog>(cacdcl);
                    }

                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    //try
                    //{
                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    //    dLog.EventName = "Download Admit Card (Admin)";
                    //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                    //    dLog.NewData = "AdmitCardDownloadClickLog()";
                    //    dLog.UserId = uId;
                    //    dLog.Attribute1 = "Success";
                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    //    LogWriter.DataLogWriter(dLog);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //#endregion

                }
            }
            catch (Exception ex)
            {
                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "Download Admit Card (Admin)";
                    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                    dLog.NewData = "AdmitCardDownloadClickLog(); Error: " + ex.Message.ToString();
                    dLog.UserId = uId;
                    dLog.Attribute1 = "Failed";
                    dLog.HostName = Request.UserHostName;
                    dLog.IpAddress = Request.UserHostAddress;
                    dLog.DateCreated = DateTime.Now;
                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ext)
                {
                }
                #endregion
            }
        }


    }
}