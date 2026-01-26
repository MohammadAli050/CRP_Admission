using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks; // Required for background processing

namespace Admission.Admission.Office.Reports
{
    public partial class BulkAdmitCardGenerate : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        // Static variables allow the Timer to read progress while the loop runs
        private static int _totalCount = 0;
        private static int _currentProgress = 0;
        private static bool _isProcessing = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                LoadDDL();
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

        #region N/A -- btnFind_Click()
        //protected void btnFind_Click(object sender, EventArgs e)
        //{

        //    string paymentIdT = "";
        //    string testRollT = "";
        //    string facultyIdT = "";
        //    string facultyNameT = "";
        //    string acaCalIdT = "";
        //    string paramiterException = "";

        //    try
        //    {


        //        btnFind.Enabled = false;

        //        long admUnitId = -1;
        //        int acaCalId = -1;

        //        admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
        //        acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

        //        string admUnitStr = string.Empty;
        //        string sessionStr = string.Empty;

        //        string admUnitWithoutSpaces = ddlAdmUnit.SelectedItem.Text.Replace(" ", "_");
        //        string admUnitStrWithoutApm = admUnitWithoutSpaces.Replace("&", "And");
        //        admUnitStr = admUnitStrWithoutApm;

        //        string sessionStrWithoutHyphen = ddlSession.SelectedItem.Text.Replace(" - ", "_");
        //        string sessionStrWithoutSpace = sessionStrWithoutHyphen.Replace(" ", "_");
        //        sessionStr = sessionStrWithoutSpace;



        //        //DateTime? StartDate = null;
        //        //DateTime? EndDate = null;
        //        //if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
        //        //{
        //        //    StartDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", null);
        //        //    EndDate = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", null);
        //        //}

        //        List<DAL.SPGetApprovedCandidateFromTestRollForBulkGenerate_Result> testRollList = null;
        //        try
        //        {
        //            using (var db = new OfficeDataManager())
        //            {
        //                testRollList = db.AdmissionDB.SPGetApprovedCandidateFromTestRollForBulkGenerate(admUnitId, acaCalId).ToList();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            lblMessage.Text = "Error getting test roll list. " + ex.Message + "; " + ex.InnerException.Message;
        //            //panelMessege.CssClass = "alert alert-danger";
        //            //panelMessege.Visible = true;
        //            return;
        //        }

        //        if (testRollList != null)
        //        {
        //            if (testRollList.Count > 0)
        //            {
        //                foreach (DAL.SPGetApprovedCandidateFromTestRollForBulkGenerate_Result item in testRollList)
        //                {
        //                    List<DAL.SPGetAdmitCardDetailsForBulkAdmitCardGenerate_Result> admitCardDetailList = null;

        //                    DAL.SPGetAdmitCardDetailsForBulkAdmitCardGenerate_Result admitCardDetails = null;

        //                    using (var db = new OfficeDataManager())
        //                    {
        //                        admitCardDetailList = db.AdmissionDB
        //                            .SPGetAdmitCardDetailsForBulkAdmitCardGenerate(item.atrTestRoll, item.atrAdmUnitID, item.atrAcaCalID).ToList();

        //                        admitCardDetails = admitCardDetailList.FirstOrDefault();
        //                    }

        //                    if (admitCardDetails != null)
        //                    {
        //                        paymentIdT = admitCardDetails.PaymentId.ToString();
        //                        testRollT = admitCardDetails.TestRoll.ToString();
        //                        facultyIdT = admitCardDetails.admUnitID.ToString();
        //                        facultyNameT = admitCardDetails.UnitName.ToString();
        //                        acaCalIdT = admitCardDetails.AcaCalID.ToString();

        //                        string path = "~/ApplicationDocs/BULKAdmitCard/" + admUnitStr + "_" + sessionStr + "/";
        //                        string pathT = path + admitCardDetails.TestRoll + ".pdf";

        //                        if (!Directory.Exists(Server.MapPath(path)))
        //                        {
        //                            Directory.CreateDirectory(Server.MapPath(path));
        //                        }


        //                        ReportViewer1.LocalReport.EnableExternalImages = true;
        //                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCard.rdlc");


        //                        ReportDataSource rds = new ReportDataSource("DataSet1", admitCardDetailList);

        //                        string imgUrl = new Uri(Server.MapPath(admitCardDetails.PhotoPath)).AbsoluteUri;
        //                        string SignatureUrl = new Uri(Server.MapPath(admitCardDetails.SignPath)).AbsoluteUri;
        //                        //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
        //                        //string examCenter = admitCardDetails.campusName;

        //                        DAL.Room roomModel = null;
        //                        string floorName = "-";
        //                        using (var db = new GeneralDataManager())
        //                        {
        //                            roomModel = db.AdmissionDB.Rooms.Where(x => x.ID == admitCardDetails.prpRoomID).FirstOrDefault();
        //                        }
        //                        if (roomModel != null)
        //                        {
        //                            floorName = roomModel.FloorNumber.ToString();
        //                        }


        //                        string examCenter = admitCardDetails.campusName; //admitCardDetails.prpRoomName + ", " + floorName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;


        //                        string examDateTimeStr = string.Empty;
        //                        DateTime examDate = Convert.ToDateTime(admitCardDetails.ExamDate);
        //                        if (examDate != null)
        //                        {
        //                            examDateTimeStr = examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString();
        //                        }
        //                        else
        //                        {
        //                            examDateTimeStr = "";
        //                        }

        //                        IList<ReportParameter> param1 = new List<ReportParameter>();
        //                        param1.Add(new ReportParameter("FacultyName", admitCardDetails.UnitName.ToString()));
        //                        paramiterException += "FacultyName: " + admitCardDetails.UnitName.ToString() + ", ";

        //                        param1.Add(new ReportParameter("CandidateName", admitCardDetails.FirstName.ToString()));
        //                        paramiterException += "CandidateName: " + admitCardDetails.FirstName.ToString() + ", ";
        //                        //if (!string.IsNullOrEmpty(imgUrl))
        //                        //{
        //                        param1.Add(new ReportParameter("CandidateImagePath", imgUrl.ToString()));
        //                        paramiterException += "CandidateImagePath: " + imgUrl.ToString() + ", ";
        //                        //}

        //                        //if (!string.IsNullOrEmpty(SignatureUrl))
        //                        //{
        //                        param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl.ToString()));
        //                        paramiterException += "CandidateSignPath: " + SignatureUrl.ToString() + ", ";
        //                        //}

        //                        //if (!string.IsNullOrEmpty(admitCardDetails.FatherName))
        //                        //{
        //                        param1.Add(new ReportParameter("FatherName", admitCardDetails.FatherName.ToUpper().ToString()));
        //                        paramiterException += "FatherName: " + admitCardDetails.FatherName.ToString() + ", ";
        //                        //}

        //                        //if (!string.IsNullOrEmpty(admitCardDetails.TestRoll))
        //                        //{
        //                        param1.Add(new ReportParameter("RollNumber", admitCardDetails.TestRoll.ToString()));
        //                        paramiterException += "RollNumber: " + admitCardDetails.TestRoll.ToString() + ", ";
        //                        //}

        //                        //if (!string.IsNullOrEmpty(examCenter))
        //                        //{
        //                        param1.Add(new ReportParameter("ExamCenter", examCenter.ToString()));
        //                        paramiterException += "ExamCenter: " + examCenter.ToString() + ", ";
        //                        //}
        //                        param1.Add(new ReportParameter("ExamDateTime", examDateTimeStr.ToString()));
        //                        paramiterException += "ExamDateTime: " + examDateTimeStr.ToString() + ", ";

        //                        param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));
        //                        paramiterException += "PrintTime: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") + "; ";


        //                        ReportViewer1.LocalReport.SetParameters(param1);

        //                        ReportViewer1.LocalReport.DataSources.Clear();
        //                        ReportViewer1.LocalReport.DataSources.Add(rds);
        //                        ReportViewer1.Visible = true;

        //                        Warning[] warnings;
        //                        string[] streamids;
        //                        string mimeType;
        //                        string encoding;
        //                        string filenameExtension;

        //                        byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
        //                        ReportViewer1.LocalReport.Refresh();

        //                        //string path = "~/ApplicationDocs/BULKAdmitCard/" + admUnitStr + "_" +sessionStr + "/";
        //                        //if (!Directory.Exists(Server.MapPath(path)))
        //                        //{
        //                        //    Directory.CreateDirectory(Server.MapPath(path));
        //                        //}
        //                        using (FileStream fs = new FileStream(Server.MapPath(path) + admitCardDetails.TestRoll + ".pdf", FileMode.Create))
        //                        {
        //                            fs.Write(bytes, 0, bytes.Length);
        //                        }

        //                        if (!File.Exists(pathT))
        //                        {

        //                            try
        //                            {
        //                                DAL.BulkAdmitCardGenerateLOG model = new DAL.BulkAdmitCardGenerateLOG();
        //                                model.CandidateID = item.atrCandidateID;
        //                                model.AdmissionUnitID = item.atrAdmUnitID;
        //                                model.AcaCalID = item.atrAcaCalID;
        //                                model.FormSL = item.atrFormSerial;
        //                                model.TestRoll = item.atrTestRoll;
        //                                model.IsApproved = item.appIsApproved;
        //                                model.IsGenerated = true;
        //                                model.CreatedBy = uId;
        //                                model.DateCreated = DateTime.Now;

        //                                using (var db = new OfficeDataManager())
        //                                {
        //                                    db.Insert<DAL.BulkAdmitCardGenerateLOG>(model);
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
        //                                try
        //                                {
        //                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
        //                                    dLog.EventName = "BulkAdmitCardGenerate_GenerateLOG_Exception";
        //                                    dLog.PageName = "BulkAdmitCardGenerate.aspx";
        //                                    dLog.OldData = "";
        //                                    dLog.NewData = "Exception: " + ex.Message.ToString() + "; InnerException: " + ex.InnerException.Message.ToString() + "; "
        //                                        + "Data: PaymentID: " + paymentIdT + ", "
        //                                        + "AcaCalId: " + acaCalIdT + ", "
        //                                        + "TestRoll: " + testRollT + ", "
        //                                        + "AdmissionUnitId: " + facultyIdT + ", "
        //                                        + "UnitName: " + facultyNameT + "; "
        //                                        + "ParamiterException: " + paramiterException;
        //                                    dLog.UserId = uId;
        //                                    dLog.DateCreated = DateTime.Now;
        //                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
        //                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; UID: " + uId;
        //                                    LogWriter.DataLogWriter(dLog);
        //                                }
        //                                catch (Exception ex3)
        //                                {
        //                                }
        //                                #endregion
        //                            }

        //                        }
        //                    }
        //                }

        //                btnFind.Enabled = true;
        //                Response.Redirect("~/Admission/Office/BulkAdmitCardFileManager.aspx", false);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
        //        try
        //        {
        //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
        //            dLog.EventName = "BulkAdmitCardGenerate_Exception";
        //            dLog.PageName = "BulkAdmitCardGenerate.aspx";
        //            dLog.OldData = "";
        //            dLog.NewData = "Exception: " + ex.Message.ToString() + "; InnerException: " + ex.InnerException.Message.ToString() + "; "
        //                                        + "Data: PaymentID: " + paymentIdT + ", "
        //                                        + "AcaCalId: " + acaCalIdT + ", "
        //                                        + "TestRoll: " + testRollT + ", "
        //                                        + "AdmissionUnitId: " + facultyIdT + ", "
        //                                        + "UnitName: " + facultyNameT + "; "
        //                                        + "ParamiterException: " + paramiterException;
        //            dLog.UserId = uId;
        //            dLog.DateCreated = DateTime.Now;
        //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
        //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; UID: " + uId; ;
        //            LogWriter.DataLogWriter(dLog);
        //        }
        //        catch (Exception ex2)
        //        {
        //        }
        //        #endregion

        //        btnFind_Click(null, null);
        //    }

        //}
        #endregion


        #region N/A
        //protected void btnGetCountCandidate_Click(object sender, EventArgs e)
        //{
        //    lblGetCountCandidate.Text = "0";

        //    try
        //    {
        //        long admUnitId = -1;
        //        int acaCalId = -1;

        //        admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
        //        acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

        //        string admUnitStr = string.Empty;
        //        string sessionStr = string.Empty;

        //        string admUnitWithoutSpaces = ddlAdmUnit.SelectedItem.Text.Replace(" ", "_");
        //        string admUnitStrWithoutApm = admUnitWithoutSpaces.Replace("&", "And");
        //        admUnitStr = admUnitStrWithoutApm;

        //        string sessionStrWithoutHyphen = ddlSession.SelectedItem.Text.Replace(" - ", "_");
        //        string sessionStrWithoutSpace = sessionStrWithoutHyphen.Replace(" ", "_");
        //        sessionStr = sessionStrWithoutSpace;

        //        DateTime? StartDate = null;
        //        DateTime? EndDate = null;
        //        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
        //        {
        //            StartDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", null);
        //            EndDate = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", null);
        //        }

        //        List<DAL.SPGetApprovedCandidateFromTestRollForBulkGenerateV2_Result> testRollList = null;
        //        try
        //        {
        //            using (var db = new OfficeDataManager())
        //            {
        //                testRollList = db.AdmissionDB.SPGetApprovedCandidateFromTestRollForBulkGenerateV2(admUnitId, acaCalId, StartDate, EndDate).ToList();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            lblMessage.Text = "Error getting test roll list. " + ex.Message + "; " + ex.InnerException.Message;
        //            //panelMessege.CssClass = "alert alert-danger";
        //            //panelMessege.Visible = true;
        //            return;
        //        }

        //        if (testRollList != null && testRollList.Count > 0)
        //        {
        //            lblGetCountCandidate.Text = testRollList.Count.ToString();
        //        }
        //        else
        //        {
        //            lblGetCountCandidate.Text = "0";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //} 
        #endregion


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

        protected void btnAdmitCardGenerate_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            try
            {
                long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int sessionId = Convert.ToInt32(ddlSession.SelectedValue);

                if (facultyId > 0 && sessionId > 0)
                {
                    string admUnitStr = string.Empty;
                    string sessionStr = string.Empty;

                    string admUnitWithoutSpaces = ddlAdmUnit.SelectedItem.Text.Replace(" ", "_");
                    string admUnitStrWithoutApm = admUnitWithoutSpaces.Replace("&", "And");
                    admUnitStr = admUnitStrWithoutApm;

                    string sessionStrWithoutHyphen = ddlSession.SelectedItem.Text.Replace(" - ", "_");
                    string sessionStrWithoutSpace = sessionStrWithoutHyphen.Replace(" ", "_");
                    sessionStr = sessionStrWithoutSpace;

                    long admUnitId = facultyId;

                    List<DAL.ApprovedList> approveList = null;
                    using (var db = new CandidateDataManager())
                    {
                        approveList = db.AdmissionDB.ApprovedLists.Where(x => x.AcaCalID == sessionId
                                                                        && x.AdmissionUnitID == facultyId
                                                                        && x.IsApproved == true
                                                                        && x.AttributeBit1 != true).OrderBy(x => x.PaymentID).ToList();
                    }

                    //int i = 1;
                    //foreach (var item in approveList)
                    //{
                    //    item.AttributeInt1 = i;
                    //    i++;
                    //}

                    //int min = 0, max = 0;
                    //try
                    //{

                    //    if (!string.IsNullOrEmpty(txtMin.Text))
                    //        min = Convert.ToInt32(txtMin.Text.Trim());

                    //    if (!string.IsNullOrEmpty(txtMax.Text))
                    //        max = Convert.ToInt32(txtMax.Text.Trim());

                    //}
                    //catch (Exception ex)
                    //{
                    //}


                    if (approveList != null && approveList.Count > 0)
                    {
                        //if (min > 0 && max > 0)
                        //{
                        //    approveList = approveList.Where(x => x.AttributeInt1 >= min && x.AttributeInt1 <= max).ToList();
                        //}

                        string path = "~/ApplicationDocs/BULKAdmitCard/" + admUnitStr + "_" + sessionStr + "/";

                        if (!Directory.Exists(Server.MapPath(path)))
                        {
                            Directory.CreateDirectory(Server.MapPath(path));
                        }

                        DirectoryInfo info = new DirectoryInfo(Server.MapPath(path));


                        foreach (var candidateModel in approveList)
                        {
                            List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list = null;
                            try
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    list = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateID(candidateModel.CandidateID, candidateModel.AcaCalID, candidateModel.AdmissionUnitID).ToList();
                                }
                            }
                            catch (Exception)
                            {

                            }
                            if (list != null && list.Count > 0)
                            {
                                try
                                {
                                    int acaCalId = list.FirstOrDefault().AcaCalID;
                                    int eduCatId = list.FirstOrDefault().EducationCategoryID;
                                    long admUnitID = list.FirstOrDefault().admUnitID;

                                    string candidateTestRoll = list.FirstOrDefault().TestRoll;

                                    string pathT = path + candidateTestRoll + ".pdf";

                                    // For Multiple time generate need to update AttributeBit1 column to 0 in Approved List Table

                                    #region If Valid File Exists Then do not need to generate again

                                    if (File.Exists(Server.MapPath(pathT)))
                                    {
                                        try
                                        {
                                            FileInfo file = info.GetFiles().Where(x => x.Name == candidateTestRoll + ".pdf").FirstOrDefault();

                                            //if (file != null && file.Length > 200000) // if file is greater than 200kb then its ok.otherwise generate a new file
                                            if (file != null)
                                            {
                                                using (var dbcand2 = new CandidateDataManager())
                                                {
                                                    candidateModel.AttributeBit1 = true;
                                                    dbcand2.Update<DAL.ApprovedList>(candidateModel);
                                                }
                                            }
                                            else
                                            {
                                                File.Delete(Server.MapPath(pathT));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                    }

                                    #endregion


                                    // If Admit Card Need To Generate Multiple Time Then Comment this IF* condition. This If Is for to restrict second time admit card generation
                                    if (File.Exists(Server.MapPath(pathT)))
                                    {

                                    }
                                    else
                                    {

                                        List<DAL.ProgramPriority> choices = null;
                                        using (var db = new CandidateDataManager())
                                        {
                                            choices = db.AdmissionDB.ProgramPriorities
                                                .Where(c => c.CandidateID == candidateModel.CandidateID
                                                            && c.AcaCalID == acaCalId
                                                            && c.AdmissionUnitID == admUnitID
                                                            && c.Priority == 1)
                                                    .ToList();
                                        }





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


                                        #region N/A
                                        //string examCenter = admitCardDetails.prpRoomName + ", " + floorName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                                        //string examCenter = list.FirstOrDefault().campusName; 
                                        #endregion
                                        string examCenter = list.FirstOrDefault().prpRoomName + ", " + list.FirstOrDefault().buildingName + ", " + list.FirstOrDefault().campusName;

                                        DateTime examDate = Convert.ToDateTime(list.FirstOrDefault().ExamDate);
                                        #endregion

                                        ReportDataSource rds = new ReportDataSource("DataSet1", list.Where(c => c.AcaCalID == acaCalId
                                                                                                                && c.admUnitID == candidateModel.AdmissionUnitID
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
                                                MessageView("More than 1 program is selected as Choice 1. Please select only 1 choice for this faculty. Test Roll: " + candidateTestRoll, "fail");

                                                //return;
                                            }
                                            else if (choices.Count() == 1)
                                            {
                                                ReportViewerBachelor.ShowExportControls = false;
                                                ReportViewerBachelor.LocalReport.EnableExternalImages = true;
                                                ReportViewerBachelor.LocalReport.DataSources.Clear();
                                                ReportViewerBachelor.Visible = false;

                                                #region Load Report and Download

                                                if (eduCatId > 0 && eduCatId == 4)
                                                {
                                                    if (File.Exists(Server.MapPath(pathT)))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        ReportViewerBachelor.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCard.rdlc");

                                                        ReportViewerBachelor.LocalReport.SetParameters(param1);

                                                        ReportViewerBachelor.LocalReport.DataSources.Clear();
                                                        ReportViewerBachelor.LocalReport.DataSources.Add(rds);
                                                        ReportViewerBachelor.Visible = true;


                                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Save Admit Card

                                                        #region N/A
                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName; 
                                                        #endregion

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerBachelor.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerBachelor.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
                                                            {
                                                                fs.Write(bytes, 0, bytes.Length);
                                                            }

                                                            #region N/A
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
                                                            #endregion

                                                            using (var dbcand2 = new CandidateDataManager())
                                                            {
                                                                candidateModel.AttributeBit1 = true;
                                                                dbcand2.Update<DAL.ApprovedList>(candidateModel);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            #region N/A
                                                            //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //lblMessage.ForeColor = Color.Crimson;
                                                            //messagePanel.CssClass = "alert alert-danger";
                                                            //messagePanel.Visible = true; 
                                                            #endregion

                                                            MessageView("Exception: Unable to download admit card; Test Roll: " + candidateTestRoll + "; Error: " + ex.Message.ToString(), "fail");

                                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            //return;
                                                        }
                                                        #region N/A
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

                                                        #endregion
                                                    }



                                                }
                                                else
                                                {
                                                    //lblMessage.Text = "Education Category did not match for Bachelor !";
                                                    //lblMessage.ForeColor = Color.Crimson;
                                                    //messagePanel.CssClass = "alert alert-danger";
                                                    //messagePanel.Visible = true;

                                                    MessageView("Education Category did not match for Bachelor! Test Roll: " + candidateTestRoll, "fail");

                                                    //return;
                                                }
                                                #endregion
                                            }
                                            else if (choices.Count() == 0)
                                            {

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

                                                #region Load Report and Download  MASTERS

                                                if (File.Exists(Server.MapPath(pathT)))
                                                {

                                                }
                                                else
                                                {

                                                    if (admUnitId == 10) //MBA (Regular) [For this Faculty Admit Card Will Be Bachelor Admit Card]
                                                    {
                                                        ReportViewerBachelor.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCard.rdlc");

                                                        ReportViewerBachelor.LocalReport.SetParameters(param1);

                                                        ReportViewerBachelor.LocalReport.DataSources.Clear();
                                                        ReportViewerBachelor.LocalReport.DataSources.Add(rds);
                                                        ReportViewerBachelor.Visible = true;

                                                        //ReportViewerMasters.Visible = false;


                                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion


                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerBachelor.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerBachelor.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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

                                                            MessageView("Exception: Unable to download admit card Test Roll: " + candidateTestRoll + "; Error: " + ex.Message.ToString(), "fail");

                                                            //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            //#endregion

                                                            // return;
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
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMBAProfessional.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMBAProfessional.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerLLMProfessional.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerLLMProfessional.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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

                                                        ReportViewerMPCHRS.LocalReport.SetParameters(param1);

                                                        ReportViewerMPCHRS.LocalReport.DataSources.Clear();
                                                        ReportViewerMPCHRS.LocalReport.DataSources.Add(rds);
                                                        ReportViewerMPCHRS.Visible = true;

                                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMPCHRS.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMPCHRS.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMICT.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMICT.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {
                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMISS.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMISS.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                    else if (admUnitId == 22) //Masters in Environmental Science and Management (Professional) 
                                                    {
                                                        ReportViewerMESM.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMESM.rdlc");
                                                        DateTime vivadate = Convert.ToDateTime(list.FirstOrDefault().VivaDate);
                                                        param1.Add(new ReportParameter("VivaDateTime", vivadate.ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().VivaTime.ToString()));

                                                        ReportViewerMESM.LocalReport.SetParameters(param1);

                                                        ReportViewerMESM.LocalReport.DataSources.Clear();
                                                        ReportViewerMESM.LocalReport.DataSources.Add(rds);
                                                        ReportViewerMESM.Visible = true;

                                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {
                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMESM.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMESM.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                    else if (admUnitId == 19) //Master of Public Health
                                                    {
                                                        #region N/A
                                                        //ReportViewerMPH.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMPH.rdlc");

                                                        //ReportViewerMPH.LocalReport.SetParameters(param1);

                                                        //ReportViewerMPH.LocalReport.DataSources.Clear();
                                                        //ReportViewerMPH.LocalReport.DataSources.Add(rds);
                                                        //ReportViewerMPH.Visible = true;

                                                        //#region Download Admit Card

                                                        ////string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        ////Session["AdmitCardFileName"] = null;
                                                        ////Session["AdmitCardFileName"] = admitCardFileName;

                                                        //try
                                                        //{

                                                        //    Warning[] warnings;
                                                        //    string[] streamids;
                                                        //    string mimeType;
                                                        //    string encoding;
                                                        //    string filenameExtension;

                                                        //    byte[] bytes = ReportViewerMICT.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                        //    ReportViewerMICT.LocalReport.Refresh();

                                                        //    using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
                                                        //    {
                                                        //        fs.Write(bytes, 0, bytes.Length);
                                                        //    }

                                                        //    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                                        //    //Response.ClearHeaders();
                                                        //    //Response.ClearContent();
                                                        //    //Response.Buffer = true;
                                                        //    //Response.Clear();
                                                        //    //Response.ContentType = "application/pdf";
                                                        //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                                        //    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                        //    //Response.Flush();
                                                        //    ////Response.Close();
                                                        //    //Response.End();

                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                                        //    //lblMessage.ForeColor = Color.Crimson;
                                                        //    //messagePanel.CssClass = "alert alert-danger";
                                                        //    //messagePanel.Visible = true;

                                                        //    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                                        //    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //    //try
                                                        //    //{
                                                        //    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                        //    //    dLog.UserId = uId;
                                                        //    //    dLog.Attribute1 = "Failed";
                                                        //    //    dLog.HostName = Request.UserHostName;
                                                        //    //    dLog.IpAddress = Request.UserHostAddress;
                                                        //    //    dLog.DateCreated = DateTime.Now;
                                                        //    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    //    dLog.DateCreated = DateTime.Now;
                                                        //    //    LogWriter.DataLogWriter(dLog);
                                                        //    //}
                                                        //    //catch (Exception ext)
                                                        //    //{
                                                        //    //}
                                                        //    #endregion

                                                        //    return;
                                                        //}
                                                        ////finally
                                                        ////{
                                                        ////    try
                                                        ////    {
                                                        ////        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                                        ////        {
                                                        ////            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                        ////        }
                                                        ////    }
                                                        ////    catch (Exception ex)
                                                        ////    {
                                                        ////        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        ////        try
                                                        ////        {
                                                        ////            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        ////            dLog.EventName = "Download Admit Card";
                                                        ////            dLog.PageName = "AdmitCardV2.aspx";
                                                        ////            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                        ////            dLog.UserId = uId;
                                                        ////            dLog.Attribute1 = "Failed";
                                                        ////            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        ////                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                        ////            dLog.DateCreated = DateTime.Now;
                                                        ////            LogWriter.DataLogWriter(dLog);
                                                        ////        }
                                                        ////        catch (Exception ext)
                                                        ////        {
                                                        ////        }
                                                        ////        #endregion
                                                        ////    }
                                                        ////}
                                                        //#endregion 
                                                        #endregion

                                                        ReportViewerMPH.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMPH.rdlc");

                                                        ReportViewerMPH.LocalReport.SetParameters(param1);

                                                        ReportViewerMPH.LocalReport.DataSources.Clear();
                                                        ReportViewerMPH.LocalReport.DataSources.Add(rds);
                                                        ReportViewerMPH.Visible = true;

                                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {
                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMPH.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMPH.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                    else if (admUnitId == 24) //Certificate Course in Hospital Management
                                                    {
                                                        #region N/A
                                                        //ReportViewerHospitalManagement.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMPH.rdlc");

                                                        //ReportViewerHospitalManagement.LocalReport.SetParameters(param1);

                                                        //ReportViewerHospitalManagement.LocalReport.DataSources.Clear();
                                                        //ReportViewerHospitalManagement.LocalReport.DataSources.Add(rds);
                                                        //ReportViewerHospitalManagement.Visible = true;

                                                        //#region Download Admit Card

                                                        ////string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        ////Session["AdmitCardFileName"] = null;
                                                        ////Session["AdmitCardFileName"] = admitCardFileName;

                                                        //try
                                                        //{

                                                        //    Warning[] warnings;
                                                        //    string[] streamids;
                                                        //    string mimeType;
                                                        //    string encoding;
                                                        //    string filenameExtension;

                                                        //    byte[] bytes = ReportViewerMICT.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                        //    ReportViewerMICT.LocalReport.Refresh();

                                                        //    using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
                                                        //    {
                                                        //        fs.Write(bytes, 0, bytes.Length);
                                                        //    }

                                                        //    //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                                        //    //Response.ClearHeaders();
                                                        //    //Response.ClearContent();
                                                        //    //Response.Buffer = true;
                                                        //    //Response.Clear();
                                                        //    //Response.ContentType = "application/pdf";
                                                        //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                                        //    //Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                        //    //Response.Flush();
                                                        //    ////Response.Close();
                                                        //    //Response.End();

                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //    //lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                                        //    //lblMessage.ForeColor = Color.Crimson;
                                                        //    //messagePanel.CssClass = "alert alert-danger";
                                                        //    //messagePanel.Visible = true;

                                                        //    MessageView("Exception: Unable to download admit card; Error: " + ex.Message.ToString(), "fail");

                                                        //    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //    //try
                                                        //    //{
                                                        //    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                        //    //    dLog.UserId = uId;
                                                        //    //    dLog.Attribute1 = "Failed";
                                                        //    //    dLog.HostName = Request.UserHostName;
                                                        //    //    dLog.IpAddress = Request.UserHostAddress;
                                                        //    //    dLog.DateCreated = DateTime.Now;
                                                        //    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    //    dLog.DateCreated = DateTime.Now;
                                                        //    //    LogWriter.DataLogWriter(dLog);
                                                        //    //}
                                                        //    //catch (Exception ext)
                                                        //    //{
                                                        //    //}
                                                        //    #endregion

                                                        //    return;
                                                        //}
                                                        ////finally
                                                        ////{
                                                        ////    try
                                                        ////    {
                                                        ////        if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                                        ////        {
                                                        ////            File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                        ////        }
                                                        ////    }
                                                        ////    catch (Exception ex)
                                                        ////    {
                                                        ////        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        ////        try
                                                        ////        {
                                                        ////            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        ////            dLog.EventName = "Download Admit Card";
                                                        ////            dLog.PageName = "AdmitCardV2.aspx";
                                                        ////            dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                        ////            dLog.UserId = uId;
                                                        ////            dLog.Attribute1 = "Failed";
                                                        ////            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        ////                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                        ////            dLog.DateCreated = DateTime.Now;
                                                        ////            LogWriter.DataLogWriter(dLog);
                                                        ////        }
                                                        ////        catch (Exception ext)
                                                        ////        {
                                                        ////        }
                                                        ////        #endregion
                                                        ////    }
                                                        ////}
                                                        //#endregion 
                                                        #endregion

                                                        ReportViewerHospitalManagement.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardHospitalM.rdlc");

                                                        ReportViewerHospitalManagement.LocalReport.SetParameters(param1);

                                                        ReportViewerHospitalManagement.LocalReport.DataSources.Clear();
                                                        ReportViewerHospitalManagement.LocalReport.DataSources.Add(rds);
                                                        ReportViewerHospitalManagement.Visible = true;

                                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {
                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerHospitalManagement.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerHospitalManagement.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {
                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMCSE.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMCSE.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMCS.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMCS.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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
                                                    else if (admUnitId == 13) //MDS (Professional)
                                                    {
                                                        ReportViewerMDSProfessional.LocalReport.ReportPath = Server.MapPath("~/Admission/Candidate/Prints/AdmitCardMDSProfessional.rdlc");

                                                        ReportViewerMDSProfessional.LocalReport.SetParameters(param1);

                                                        ReportViewerMDSProfessional.LocalReport.DataSources.Clear();
                                                        ReportViewerMDSProfessional.LocalReport.DataSources.Add(rds);
                                                        ReportViewerMDSProfessional.Visible = true;

                                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                        //try
                                                        //{
                                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        //    dLog.EventName = "Download Admit Card (Admin)";
                                                        //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                        //    dLog.NewData = "Admit Card Download Successful. Name:" + list.FirstOrDefault().FirstName + ";" +
                                                        //                                                    "PaymentId: " + list.FirstOrDefault().PaymentId + ";" +
                                                        //                                                    "FacultyName: " + list.FirstOrDefault().UnitName + ";" +
                                                        //                                                    "FormSerial: " + list.FirstOrDefault().FormSerial + ";";
                                                        //    dLog.UserId = uId;
                                                        //    dLog.Attribute1 = "Success";
                                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                        //    dLog.DateCreated = DateTime.Now;
                                                        //    LogWriter.DataLogWriter(dLog);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //}
                                                        #endregion

                                                        #region Download Admit Card

                                                        //string admitCardFileName = "BUPAdmitCard_" + list.FirstOrDefault().AcaCalID + "_" + list.FirstOrDefault().admUnitID + "_" + list.FirstOrDefault().TestRoll + ".pdf";
                                                        //Session["AdmitCardFileName"] = null;
                                                        //Session["AdmitCardFileName"] = admitCardFileName;

                                                        try
                                                        {

                                                            Warning[] warnings;
                                                            string[] streamids;
                                                            string mimeType;
                                                            string encoding;
                                                            string filenameExtension;

                                                            byte[] bytes = ReportViewerMDSProfessional.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                                            ReportViewerMDSProfessional.LocalReport.Refresh();

                                                            using (FileStream fs = new FileStream(Server.MapPath(path) + candidateTestRoll + ".pdf", FileMode.Create))
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
                                                            //try
                                                            //{
                                                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                            //    dLog.EventName = "Download Admit Card (Admin)";
                                                            //    dLog.PageName = "RPTPrintCandidateAdmitCard.aspx";
                                                            //    dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                            //    dLog.UserId = uId;
                                                            //    dLog.Attribute1 = "Failed";
                                                            //    dLog.HostName = Request.UserHostName;
                                                            //    dLog.IpAddress = Request.UserHostAddress;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                            //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + candidateId;
                                                            //    dLog.DateCreated = DateTime.Now;
                                                            //    LogWriter.DataLogWriter(dLog);
                                                            //}
                                                            //catch (Exception ext)
                                                            //{
                                                            //}
                                                            #endregion

                                                            // return;
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

                                                    else
                                                    {
                                                        //lblMessage.Text = "Invalid Request for Download Admit Card !";
                                                        //lblMessage.ForeColor = Color.Crimson;
                                                        //messagePanel.CssClass = "alert alert-danger";
                                                        //messagePanel.Visible = true;

                                                        MessageView("Invalid Request for Download Admit Card; Test Roll: " + candidateTestRoll, "fail");
                                                        // return;
                                                    }
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
                                            //return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card BULK";
                                        dLog.PageName = "BulkAdmitCardGenerate.aspx";
                                        dLog.NewData = "CandidateID: " + candidateModel.CandidateID + " ;" + ex.Message.ToString();
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Failed";
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ext)
                                    {
                                    }
                                    #endregion
                                }







                            }
                            else
                            {
                                MessageView("Failed to get candidate admit card information!", "fail.C:" + candidateModel.CandidateID);
                            }


                        }


                    }
                    else
                    {
                        MessageView("No data found for generate!", "fail");
                    }
                }
                else
                {
                    MessageView("Please select Faculty and Session!", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnAdmitCardGenerate_ClickNew(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            int sessionId = Convert.ToInt32(ddlSession.SelectedValue);

            if (facultyId > 0 && sessionId > 0)
            {
                // Fetch list using your exact existing query logic
                List<DAL.ApprovedList> approveList;
                using (var db = new CandidateDataManager())
                {
                    approveList = db.AdmissionDB.ApprovedLists.Where(x => x.AcaCalID == sessionId
                                                                && x.AdmissionUnitID == facultyId
                                                                && x.IsApproved == true
                                                                && x.AttributeBit1 != true).OrderBy(x => x.PaymentID).ToList();
                }

                if (approveList != null && approveList.Count > 0)
                {
                    // Initialize Progress Bar
                    _totalCount = approveList.Count;
                    _currentProgress = 0;
                    _isProcessing = true;

                    lblTotal.Text = _totalCount.ToString();
                    lblCurrent.Text = "0";
                    pnlStatus.Visible = true;
                    ProgressTimer.Enabled = true;

                    // Capture current context variables for the background thread
                    string mappedPath = Server.MapPath("~/Admission/Candidate/Prints/");
                    string docPathRoot = Server.MapPath("~/ApplicationDocs/BULKAdmitCard/");
                    string unitText = ddlAdmUnit.SelectedItem.Text;
                    string sessionText = ddlSession.SelectedItem.Text;

                    // Execute your actual logic in the background
                    Task.Run(() => RunGenerationLogic(approveList, facultyId, sessionId, mappedPath, docPathRoot, unitText, sessionText));
                }
                else
                {
                    MessageView("No data found for generate!", "fail");
                }
            }
        }

        // THIS IS YOUR ACTUAL LOGIC MOVED TO A BACKGROUND METHOD
        private void RunGenerationLogic(List<DAL.ApprovedList> approveList, long facultyId, int sessionId, string mappedPath, string docPathRoot, string unitText, string sessionText)
        {
            try
            {
                string admUnitStr = unitText.Replace(" ", "_").Replace("&", "And");
                string sessionStr = sessionText.Replace(" - ", "_").Replace(" ", "_");
                string folderPath = docPathRoot + admUnitStr + "_" + sessionStr + "/";

                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                LocalReport localReport = new LocalReport();

                foreach (var candidateModel in approveList)
                {
                    List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list;
                    using (var db = new CandidateDataManager())
                    {
                        list = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateID(candidateModel.CandidateID, candidateModel.AcaCalID, candidateModel.AdmissionUnitID).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        var firstRecord = list.FirstOrDefault();
                        string candidateTestRoll = firstRecord.TestRoll;
                        string filePath = folderPath + candidateTestRoll + ".pdf";

                        // Your logic: Skip if valid file exists
                        if (File.Exists(filePath) && new FileInfo(filePath).Length > 200000)
                        {
                            UpdateAttributeBit(candidateModel);
                        }
                        else
                        {
                            // Process Report Generation
                            localReport.DataSources.Clear();
                            localReport.EnableExternalImages = true;
                            localReport.ReportPath = GetReportPath(facultyId, mappedPath);

                            // Add your exact parameters
                            ReportDataSource rds = new ReportDataSource("DataSet1", list);
                            localReport.DataSources.Add(rds);

                            List<ReportParameter> param1 = new List<ReportParameter>();
                            param1.Add(new ReportParameter("FacultyName", firstRecord.UnitName));
                            param1.Add(new ReportParameter("CandidateName", firstRecord.FirstName));
                            param1.Add(new ReportParameter("CandidateImagePath", new Uri(docPathRoot.Replace("ApplicationDocs/BULKAdmitCard/", "") + firstRecord.PhotoPath.Replace("~/", "")).AbsoluteUri));
                            param1.Add(new ReportParameter("CandidateSignPath", new Uri(docPathRoot.Replace("ApplicationDocs/BULKAdmitCard/", "") + firstRecord.SignPath.Replace("~/", "")).AbsoluteUri));
                            param1.Add(new ReportParameter("FatherName", firstRecord.FatherName?.ToUpper() ?? ""));
                            param1.Add(new ReportParameter("RollNumber", firstRecord.TestRoll ?? ""));
                            param1.Add(new ReportParameter("ExamCenter", firstRecord.prpRoomName + ", " + firstRecord.buildingName + ", " + firstRecord.campusName));
                            param1.Add(new ReportParameter("ExamDateTime", Convert.ToDateTime(firstRecord.ExamDate).ToString("dd-MMM-yyyy") + ", " + firstRecord.ExamTime));
                            param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));

                            // Handling instructions (your logic)
                            using (var db = new GeneralDataManager())
                            {
                                var ins = db.AdmissionDB.AdmitCardInstructions.FirstOrDefault(x => x.AcacalId == sessionId && x.AdmissionUnitId == facultyId);
                                param1.Add(new ReportParameter("Instruction", ins != null ? ins.InstructionDetails.ToString() : ""));
                            }

                            localReport.SetParameters(param1);
                            byte[] bytes = localReport.Render("PDF");
                            File.WriteAllBytes(filePath, bytes);

                            UpdateAttributeBit(candidateModel);
                        }
                    _currentProgress++; // Increment progress
                    }
                }
            }
            catch (Exception ex) { /* Log error if needed */ }
            finally
            {
                _isProcessing = false;
            }
        }

        protected void ProgressTimer_Tick(object sender, EventArgs e)
        {
            lblCurrent.Text = _currentProgress.ToString();
            lblTotal.Text = _totalCount.ToString();

            if (_totalCount > 0)
            {
                double percentage = ((double)_currentProgress / _totalCount) * 100;
                progressBar.Style["width"] = percentage.ToString("0") + "%";
            }

            if (!_isProcessing && _currentProgress >= _totalCount)
            {
                ProgressTimer.Enabled = false;
                MessageView("Bulk Generation Complete!", "success");
                btnAdmitCardGenerate.Enabled = true;
            }
        }

        private void UpdateAttributeBit(DAL.ApprovedList model)
        {
            using (var db = new CandidateDataManager())
            {
                model.AttributeBit1 = true;
                db.Update<DAL.ApprovedList>(model);
            }
        }

        private string GetReportPath(long unitId, string basePath)
        {
            // Switch logic based on your existing ReportViewer ID logic
            if (unitId == 6) return basePath + "AdmitCardMBAProfessional.rdlc";
            if (unitId == 9) return basePath + "AdmitCardLLMProfessional.rdlc";
            if (unitId == 18) return basePath + "AdmitCardMPCHRS.rdlc";
            if (unitId == 12) return basePath + "AdmitCardMICT.rdlc";
            if (unitId == 11) return basePath + "AdmitCardMISS.rdlc";
            if (unitId == 19) return basePath + "AdmitCardMPH.rdlc";
            if (unitId == 20) return basePath + "AdmitCardMCSE.rdlc";
            if (unitId == 13) return basePath + "AdmitCardMDSProfessional.rdlc";
            if (unitId == 21) return basePath + "AdmitCardMCS.rdlc";
            if (unitId == 22) return basePath + "AdmitCardMESM.rdlc";
            if (unitId == 24) return basePath + "AdmitCardHospitalM.rdlc";
            return basePath + "AdmitCard.rdlc";
        }


        private void LogError(long candidateId, string errorMessage)
        {
            try
            {
                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                dLog.EventName = "Download Admit Card BULK Error";
                dLog.PageName = "BulkAdmitCardGenerate.aspx";
                dLog.NewData = $"CandidateID: {candidateId}; Error: {errorMessage}";

                // Retrieve UserID from Session (assuming uId is available globally in your class)
                // If not, use: long userId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                dLog.UserId = uId;

                dLog.Attribute1 = "Failed";
                dLog.DateCreated = DateTime.Now;

                // Call your existing static log writer
                LogWriter.DataLogWriter(dLog);
            }
            catch
            {
                // Fail silently to avoid stopping the whole bulk generation 
                // if the logging database itself is having issues.
            }
        }

    }
}