using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data.Entity.Core.Objects;
using System.IO;

namespace Admission.Admission.Candidate
{
    public partial class ForeignCandidateHome : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        long cId = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            this.Page.Form.Enctype = "multipart/form-data";

            using (var db = new CandidateDataManager())
            {
                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                    //paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID && x.IsPaid == true).Select(x => x.PaymentId).FirstOrDefault();
                }
            }

            if (!IsPostBack)
            {
                divPayment.Visible = false;
                if (uId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.BasicInfo candidateObj = db.GetCandidateBasicInfoByUserID_ND(uId);
                        if (candidateObj != null)
                        {
                            cId = candidateObj.ID;
                            LoadCandidateData(candidateObj);
                            ViewPhoto(cId);

                            var AdditionObj = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId && x.IsFinalSubmit == true).FirstOrDefault();
                            if (AdditionObj != null)
                            {
                                lblFinalSubmit.Text = "Yes";
                                DivProgramAdd.Visible = true;
                                LoadProgramData();
                                LoadSessionDDL(0);
                            }
                            else
                            {
                                lblFinalSubmit.Text = "No";
                                DivProgramAdd.Visible = false;
                            }

                            
                        }
                        else
                        {
                            Response.Redirect("~/Admission/Login.aspx");
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void LoadProgramData()
        {
            try
            {
                using (var db = new CandidateDataManager())
                {

                    DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId
                                                                                             && x.Priority == 1).FirstOrDefault();

                    List<DAL.ProgramPriority> pplist = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId).ToList();

                    List<TempObj> programList = new List<TempObj>();

                    if (pplist != null && pplist.Any())
                    {
                        List<DAL.SPAcademicCalendarGetAll_Result> sessions = db.AdmissionDB.SPAcademicCalendarGetAll().ToList();

                        int i = 0;
                        foreach (var item in pplist)
                        {
                            i = i + 1;
                            TempObj NewObj = new TempObj();

                            NewObj.SL = i;
                            NewObj.ProgramPriorityId = item.ID;
                            NewObj.ProgramName = item.ProgramName;
                            NewObj.Priority = item.Priority.ToString();
                            if (sessions != null && sessions.Any())
                            {
                                var Session = sessions.Where(x => x.AcademicCalenderID == item.AcaCalID).FirstOrDefault();
                                if (Session != null)
                                    NewObj.Session = Session.FullCode;
                            }

                            var PaymentObj = db.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.ProgramPriorityId == item.ID).FirstOrDefault();

                            if (PaymentObj != null && PaymentObj.IsPaid == true)
                            {
                                NewObj.PaymentStatus = "Paid";
                                NewObj.FilePath = PaymentObj.FileName;
                                NewObj.BankName = PaymentObj.BankName;
                                NewObj.TransactionId = PaymentObj.TransactionId;
                                NewObj.PaymentDate = PaymentObj.PaymentDate == null ? "" : Convert.ToDateTime(PaymentObj.PaymentDate).ToString("dd/MM/yyyy");
                            }

                            programList.Add(NewObj);

                        }

                        GridViewProgramList.DataSource = programList.OrderBy(x => x.Priority);
                        GridViewProgramList.DataBind();
                    }
                    else
                    {
                        GridViewProgramList.DataSource = null;
                        GridViewProgramList.DataBind();
                    }



                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSessionDDL(int programID)
        {
            using (var db = new GeneralDataManager())
            {

                List<DAL.SPAcademicCalendarGetAll_Result> sessions = new List<DAL.SPAcademicCalendarGetAll_Result>();

                List<DAL.SPAcademicCalendarGetAll_Result> Tempsessions = db.AdmissionDB.SPAcademicCalendarGetAll().ToList();

                if (Tempsessions != null && Tempsessions.Any())
                {
                    var SetupList = db.AdmissionDB.AdmissionSetups.Where(x => x.Attribute3.ToLower().StartsWith("A")).ToList();

                    if (SetupList != null && SetupList.Any())
                    {
                        foreach (var item in SetupList)
                        {
                            var ExistingObj = Tempsessions.Where(x => x.AcademicCalenderID == item.AcaCalID).FirstOrDefault();
                            if (ExistingObj != null)
                            {
                                var IsExists = sessions.Where(x => x.AcademicCalenderID == item.AcaCalID).FirstOrDefault();
                                if (IsExists == null)
                                {
                                    sessions.Add(ExistingObj);
                                }
                            }
                        }
                    }

                    //sessions = sessions.Where(x => x.IsActiveAdmission == true).OrderBy(x => x.AcademicCalenderID).ToList();
                    //if (ActiveAdmission != null)
                    //{
                    //    sessions = sessions.Where(x => x.AcademicCalenderID >= ActiveAdmission.AcademicCalenderID).ToList();
                    //}
                    sessions = sessions.OrderByDescending(z => z.AcademicCalenderID).ToList();
                    DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, sessions, "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
                }

                else
                {
                    ddlSession.Items.Clear();
                    ddlSession.Items.Add(new ListItem("N/A", "-1"));
                }
            }
        }
        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);


                using (var db = new CandidateDataManager())
                {
                    List<DAL.AdmissionUnit> unitList = new List<DAL.AdmissionUnit>();

                    var SetupList = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == SessionId && x.Attribute3 == "Active").ToList();

                    List<DAL.AdmissionUnit> AllList = db.AdmissionDB.AdmissionUnits.Where(x => x.Attribute3 == "Active").ToList();

                    if (SetupList != null && SetupList.Any())
                    {
                        foreach (var item in SetupList)
                        {

                            if (AllList != null && AllList.Any())
                            {
                                DAL.AdmissionUnit NewObj = null;

                                var list = AllList.Where(x => x.ID == item.AdmissionUnitID).ToList();
                                if (list != null && list.Any())
                                    NewObj = list.FirstOrDefault();


                                var ExistingObj = unitList.Where(x => x.ID == item.AdmissionUnitID).ToList();
                                if (ExistingObj == null || ExistingObj.Count == 0)
                                {
                                    if (NewObj != null)
                                        unitList.Add(NewObj);

                                }

                            }

                        }
                    }


                    ddlAdmissionUnit.Items.Clear();
                    ddlAdmissionUnit.AppendDataBoundItems = true;
                    ddlAdmissionUnit.Items.Add(new ListItem("Select", "0"));

                    ddlAdmissionUnit.DataTextField = "UnitName";
                    ddlAdmissionUnit.DataValueField = "ID";
                    ddlAdmissionUnit.DataSource = unitList;
                    ddlAdmissionUnit.DataBind();


                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void ddlAdmissionUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
                int UnitId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);

                int CategoryId = 4;

                if (SessionId > 0 && UnitId > 0)
                {

                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();

                        List<DAL.SPProgramsGetAllFromUCAM_Result> ProgramList = new List<DAL.SPProgramsGetAllFromUCAM_Result>();

                        var SetupList = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == SessionId && x.AdmissionUnitID == UnitId && x.Attribute3 == "Active").ToList();

                        var CandidateBasicInfo = db.AdmissionDB.BasicInfoes.AsNoTracking().Where(x => x.ID == cId).FirstOrDefault();
                        try
                        {
                            if (CandidateBasicInfo != null)
                                CategoryId = CandidateBasicInfo.AttributeInt2 == null ? 0 : Convert.ToInt32(CandidateBasicInfo.AttributeInt2);
                        }
                        catch (Exception ex)
                        {
                        }

                        if (SetupList != null && SetupList.Any())
                        {
                            foreach (var item in SetupList)
                            {

                                var NewObj = programs.Where(x => x.ProgramID == item.ProgramID).FirstOrDefault();

                                var ExistingObj = ProgramList.Where(x => x.ProgramID == item.ProgramID).ToList();

                                if (ExistingObj == null || ExistingObj.Count == 0)
                                {
                                    if (NewObj != null)
                                        ProgramList.Add(NewObj);
                                }
                            }
                        }

                        if (ProgramList != null && ProgramList.Any())
                        {
                            if (CategoryId == 4) // Undergraduate
                                ProgramList = ProgramList.Where(x => x.ProgramTypeID == 1).ToList();
                            else // Others
                                ProgramList = ProgramList.Where(x => x.ProgramTypeID != 1).ToList();
                        }


                        ddlProgram.Items.Clear();
                        ddlProgram.AppendDataBoundItems = true;
                        ddlProgram.Items.Add(new ListItem("Select", "0"));

                        ddlProgram.DataTextField = "DetailNShortName";
                        ddlProgram.DataValueField = "ProgramID";
                        ddlProgram.DataSource = ProgramList.OrderBy(x => x.DetailNShortName).ToList();
                        ddlProgram.DataBind();
                    }


                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            int progId = -1;
            progId = Convert.ToInt32(ddlProgram.SelectedValue);
            if (progId > 0)
            {
            }
        }

        private void ViewPhoto(long cId)
        {
            try
            {
                imgCtrl.Src = "";
                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            imgCtrl.Src = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCandidateData(BasicInfo candidateObj)
        {
            try
            {
                lblCandidateName.Text = string.Empty;
                lblEmail.Text = string.Empty;

                if (candidateObj != null)
                {
                    lblCandidateName.Text = candidateObj.FirstName;
                    lblEmail.Text = candidateObj.Email;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnApplicationForm_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    var AdditionObj = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId && x.IsFinalSubmit == true).FirstOrDefault();
                    if (AdditionObj == null)
                    {
                        Response.Redirect("~/Admission/Candidate/PersonalInfoAppForm.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("~/Admission/Candidate/FinalsubmitView.aspx", false);

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void btnAddProgram_Click(object sender, EventArgs e)
        {

            ModalPopupExtender1.Show();

           
        }

        public class TempObj
        {
            public int SL { get; set; }
            public long ProgramPriorityId { get; set; }
            public string ProgramName { get; set; }
            public string Priority { get; set; }
            public string Session { get; set; }
            public string PaymentStatus { get; set; }
            public string FilePath { get; set; }
            public string BankName { get; set; }
            public string TransactionId { get; set; }
            public string PaymentDate { get; set; }
        }



        protected void btndeposite_Click(object sender, EventArgs e)
        {
            try
            {
                int SetupId = Convert.ToInt32(hdnProgramPriorityId.Value);

                if (SetupId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {

                        var ProgramPriority = db.AdmissionDB.ProgramPriorities.Where(x => x.ID == SetupId).FirstOrDefault();
                        if (ProgramPriority != null)
                        {
                            var existingObj = db.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.ProgramPriorityId == SetupId).FirstOrDefault();

                            if (existingObj != null)
                            {


                                if (FileUploadDeposite.HasFile)
                                {
                                    String fileExtension = Path.GetExtension(FileUploadDeposite.PostedFile.FileName).ToLower();
                                    int contentlength = int.Parse(FileUploadDeposite.PostedFile.ContentLength.ToString());
                                    string fileName = "C-Deposit-" + SetupId + "-" + cId + fileExtension; // C for candidate
                                    string filePath = "~/Upload/Candidate/Others/";
                                    string tempFileName = "Temp-C-Deposit-" + SetupId + "-" + cId + fileExtension; // C for candidate
                                    string tempFilePath = "~/Upload/Candidate/TEMP/Others/";

                                    if (contentlength < 2097152) // 2048 KB
                                    {
                                        try
                                        {

                                            if (File.Exists(Server.MapPath(filePath + fileName)))
                                            {
                                                //move the file to TEMP
                                                File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                                                //delete the original file
                                                File.Delete(Server.MapPath(filePath + fileName));

                                                FileUploadDeposite.SaveAs(Server.MapPath(filePath + fileName));


                                                //delete the temp file
                                                if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
                                                {
                                                    File.Delete(Server.MapPath(tempFilePath + tempFileName));
                                                }

                                            }
                                            else
                                            {
                                                FileUploadDeposite.SaveAs(Server.MapPath(filePath + fileName));
                                            }

                                            existingObj.FileName = filePath + fileName;
                                            if (!string.IsNullOrEmpty(BankName.Text))
                                                existingObj.BankName = BankName.Text;
                                            if (!string.IsNullOrEmpty(TransID.Text))
                                                existingObj.TransactionId = TransID.Text;
                                            if (!string.IsNullOrEmpty(PaymentDate.Text))
                                                existingObj.PaymentDate = DateTime.ParseExact(PaymentDate.Text, "dd/MM/yyyy", null);
                                            existingObj.IsPaid = true;

                                            using (var dbUpdate = new CandidateDataManager())
                                            {
                                                dbUpdate.Update<DAL.ForeignCandidatePaymentInformation>(existingObj);
                                            }

                                            divPayment.Visible = false;




                                            //#region If All Program Is paid then payment IsPaid=1 in candidate Payment Table

                                            //bool AllPaid = false;

                                            //var PaymentIdList = db.AdmissionDB.CandidatePayments.AsNoTracking().Where(x => x.CandidateID == cId).ToList();
                                            //if (PaymentIdList != null && PaymentIdList.Any())
                                            //{
                                            //    foreach (var PaymentId in PaymentIdList)
                                            //    {
                                            //        var FormSerialList = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidatePaymentID == PaymentId.ID).ToList();
                                            //        if (FormSerialList != null && FormSerialList.Any())
                                            //        {
                                            //            foreach (var FormSerial in FormSerialList)
                                            //            {
                                            //                var ObjIsPaid = db.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.FormSerialId == FormSerial.ID).FirstOrDefault();
                                            //                if (ObjIsPaid != null && ObjIsPaid.IsPaid == true)
                                            //                    AllPaid = true;
                                            //                else
                                            //                    AllPaid = false;

                                            //            }
                                            //        }

                                            //        if (AllPaid)
                                            //        {
                                            //            try
                                            //            {


                                            //                PaymentId.IsPaid = true;
                                            //                PaymentId.ModifiedBy = -99;
                                            //                PaymentId.DateModified = DateTime.Now;
                                            //                using (var dbUpdate = new CandidateDataManager())
                                            //                {
                                            //                    dbUpdate.Update<DAL.CandidatePayment>(PaymentId);
                                            //                }
                                            //            }
                                            //            catch (Exception ex)
                                            //            {
                                            //            }
                                            //        }

                                            //    }
                                            //}

                                            //#endregion


                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    else
                                    {
                                    }

                                    LoadProgramData();

                                }




                            }
                        }


                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                hdnProgramPriorityId.Value = "0";
                divPayment.Visible = false;
                int SetupId = Convert.ToInt32((sender as LinkButton).CommandArgument);

                if (SetupId > 0)
                {
                    hdnProgramPriorityId.Value = SetupId.ToString();
                    divPayment.Visible = true;

                    using (var db = new CandidateDataManager())
                    {
                        var ExistingObj = db.AdmissionDB.ProgramPriorities.Where(x => x.ID == SetupId).FirstOrDefault();

                        if (ExistingObj != null)
                        {
                            txtProgram.Text = ExistingObj.ProgramName;

                            var PaymentObj = db.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.ProgramPriorityId == SetupId).FirstOrDefault();

                            if (PaymentObj != null)
                            {
                                BankName.Text = PaymentObj.BankName;
                                TransID.Text = PaymentObj.TransactionId;
                                PaymentDate.Text = PaymentObj.PaymentDate == null ? "" : Convert.ToDateTime(PaymentObj.PaymentDate).ToString("dd/MM/yyyy");
                            }

                        }





                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                divPayment.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }


        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int SetupId = Convert.ToInt32((sender as LinkButton).CommandArgument);

                using (var db = new CandidateDataManager())
                {
                    var PaymentObj = db.AdmissionDB.ForeignCandidatePaymentInformations.AsNoTracking().Where(x => x.ProgramPriorityId == SetupId).FirstOrDefault();
                    if (PaymentObj != null && PaymentObj.IsPaid == false)
                    {
                        DAL.ProgramPriority ExistingObj = db.AdmissionDB.ProgramPriorities.AsNoTracking().Where(x => x.ID == SetupId).FirstOrDefault();

                        var PaymentIdObj = db.AdmissionDB.CandidatePayments.AsNoTracking().Where(x => x.CandidateID == ExistingObj.CandidateID && x.AcaCalID == ExistingObj.AcaCalID).FirstOrDefault();

                        var FormSerialIdObj = db.AdmissionDB.CandidateFormSls.AsNoTracking().Where(x => x.CandidateID == ExistingObj.CandidateID && x.AcaCalID == ExistingObj.AcaCalID).FirstOrDefault();



                        if (ExistingObj != null)
                        {
                            using (var dbDelete = new CandidateDataManager())
                            {
                                dbDelete.Delete<DAL.ProgramPriority>(ExistingObj);
                            }


                            using (var dbDelete2 = new CandidateDataManager())
                            {
                                if (FormSerialIdObj != null)
                                {
                                    dbDelete2.Delete<DAL.CandidateFormSl>(FormSerialIdObj);
                                }
                            }


                            using (var dbRemove = new CandidateDataManager())
                            {

                                if (PaymentObj != null)
                                {
                                    dbRemove.Delete<DAL.ForeignCandidatePaymentInformation>(PaymentObj);
                                }

                            }
                            using (var dbDelete1 = new CandidateDataManager())
                            {
                                if (PaymentIdObj != null && (PaymentIdObj.IsPaid == null || Convert.ToBoolean(PaymentIdObj.IsPaid) == false))
                                {
                                    dbDelete1.Delete<DAL.CandidatePayment>(PaymentIdObj);
                                }

                            }



                            LoadProgramData();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LoadProgramData();
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            try
            {

                int FacultyId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);
                int ProgramId = Convert.ToInt32(ddlProgram.SelectedValue);
                int AcacalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (FacultyId > 0 && ProgramId > 0 && AcacalId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.AdmissionUnit Unit = db.AdmissionDB.AdmissionUnits.AsNoTracking().Where(x => x.ID == FacultyId).FirstOrDefault();

                        List<DAL.ProgramPriority> ppList = db.AdmissionDB.ProgramPriorities.AsNoTracking().Where(x => x.CandidateID == cId).ToList();

                        DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.AsNoTracking().Where(x => x.CandidateID == cId
                                                                                                      && x.ProgramID == ProgramId && x.AcaCalID == AcacalId).FirstOrDefault();

                        DAL.AdmissionSetup asetup = db.AdmissionDB.AdmissionSetups.Where(x => x.AdmissionUnitID == FacultyId && x.AcaCalID == AcacalId && x.Attribute3 != null).FirstOrDefault();

                        DAL.AdmissionUnitProgram adUnitProgram = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AdmissionUnitID == FacultyId && x.AcaCalID == AcacalId && x.ProgramID == ProgramId).FirstOrDefault();

                        int SetupId = 0, UnitProgramId = 0, batchId = 0;

                        if (asetup != null)
                            SetupId = Convert.ToInt32(asetup.ID);
                        if (adUnitProgram != null)
                        {
                            UnitProgramId = Convert.ToInt32(adUnitProgram.ID);
                            batchId = Convert.ToInt32(adUnitProgram.BatchID);
                        }

                        List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();

                        var ProgramObj = programs.Where(x => x.ProgramID == ProgramId).FirstOrDefault();

                        string Name = ddlProgram.SelectedItem.ToString(), Code = "";

                        if (ProgramObj != null)
                        {
                            Name = ProgramObj.DetailName;
                            Code = ProgramObj.ShortName;
                        }

                        if (SetupId > 0 && UnitProgramId > 0 && batchId > 0)
                        {
                            long ProgramPriorityId = 0;

                            if (pp != null) // Update Existing Object
                            {
                                pp.AdmissionUnitID = FacultyId;
                                pp.AdmissionUnitProgramID = UnitProgramId;
                                pp.AdmissionSetupID = SetupId;
                                pp.ProgramID = ProgramId;
                                pp.BatchID = batchId;
                                pp.AcaCalID = AcacalId;
                                pp.ProgramName = Name;
                                pp.ShortName = Code;
                                pp.ModifiedBy = uId;
                                pp.DateModified = DateTime.Now;

                                using (var dbUpdate = new CandidateDataManager())
                                {
                                    dbUpdate.Update<DAL.ProgramPriority>(pp);
                                }

                                ProgramPriorityId = pp.ID;
                            }
                            else // Insert a New Object
                            {

                                int Priority = 1;

                                if (ppList != null && ppList.Any())
                                    Priority = ppList.Count() + 1;


                                DAL.ProgramPriority NewObj = new DAL.ProgramPriority();

                                NewObj.CandidateID = cId;
                                NewObj.AdmissionUnitID = FacultyId;
                                NewObj.AdmissionUnitProgramID = UnitProgramId;
                                NewObj.AdmissionSetupID = SetupId;
                                NewObj.ProgramID = ProgramId;
                                NewObj.BatchID = batchId;
                                NewObj.AcaCalID = AcacalId;
                                NewObj.Priority = Priority;
                                NewObj.ProgramName = Name;
                                NewObj.ShortName = Code;
                                NewObj.CreatedBy = uId;
                                NewObj.DateCreated = DateTime.Now;

                                using (var dbInsert = new CandidateDataManager())
                                {
                                    dbInsert.Insert<DAL.ProgramPriority>(NewObj);

                                    ProgramPriorityId = NewObj.ID;
                                }




                                #region CandidatePayment/CandidateFormSerial

                                //---------------------------------------------------------------------------------
                                //insert candidate payment
                                //---------------------------------------------------------------------------------
                                long candidatePaymentIDLong = -1;
                                if (asetup != null)
                                {
                                    using (var db1 = new CandidateDataManager())
                                    {
                                        DAL.CandidatePayment PaymentExistingObj = db1.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId && x.AcaCalID == AcacalId).FirstOrDefault();
                                        if (PaymentExistingObj == null)
                                        {

                                            string FeeInfo = asetup.Attribute3 == null ? "0_0" : asetup.Attribute3;
                                            decimal Amount = 0;
                                            if (FeeInfo != "")
                                            {
                                                string[] fees = FeeInfo.Split('_');
                                                if (fees.Length > 0)
                                                {
                                                    try
                                                    {
                                                        Amount = Convert.ToDecimal(fees[1]);

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                            }

                                            ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                                            db1.AdmissionDB.SPCandidatePaymentInsert(id_param, cId, null, AcacalId, Convert.ToInt32(Unit.UnitCode1), false, Amount, -99, DateTime.Now);
                                            candidatePaymentIDLong = Convert.ToInt64(id_param.Value);
                                        }
                                        else
                                        {
                                            candidatePaymentIDLong = (long)PaymentExistingObj.ID;
                                        }
                                    }
                                }

                                //---------------------------------------------------------------------------------
                                //insert candidate form serial
                                //---------------------------------------------------------------------------------
                                long candidateFormSerialIDLong = -1;
                                if (asetup != null)
                                {
                                    using (var db1 = new CandidateDataManager())
                                    {
                                        DAL.CandidateFormSl FormSLExistingObj = db1.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId && x.AcaCalID == AcacalId && x.AdmissionSetupID == asetup.ID).FirstOrDefault();
                                        if (FormSLExistingObj == null)
                                        {
                                            ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                                            db1.AdmissionDB.SPCandidateFormSlInsert(id_param, cId, asetup.ID, asetup.AcaCalID, Convert.ToInt32(Unit.UnitCode1), null, candidatePaymentIDLong, DateTime.Now, -99);
                                            candidateFormSerialIDLong = Convert.ToInt64(id_param.Value);
                                        }
                                        else
                                        {
                                            candidateFormSerialIDLong = (long)FormSLExistingObj.ID;
                                        }

                                    }
                                }
                                #endregion


                                #region Candidate Payment Information Table

                                using (var db1 = new CandidateDataManager())
                                {

                                    DAL.ForeignCandidatePaymentInformation ExistingObj = db1.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.ProgramPriorityId == ProgramPriorityId).FirstOrDefault();

                                    if (ExistingObj == null)
                                    {
                                        DAL.ForeignCandidatePaymentInformation NewPaymentObj = new DAL.ForeignCandidatePaymentInformation();

                                        NewPaymentObj.ProgramPriorityId = ProgramPriorityId;
                                        NewPaymentObj.FormSerialId = candidateFormSerialIDLong;
                                        NewPaymentObj.IsPaid = false;
                                        NewPaymentObj.CreatedBy = -99;
                                        NewPaymentObj.CreatedDate = DateTime.Now;

                                        db1.AdmissionDB.ForeignCandidatePaymentInformations.Add(NewPaymentObj);
                                        db1.AdmissionDB.SaveChanges();

                                    }


                                }

                                #endregion

                            }
                        }


                    }// end using

                }
                LoadSessionDDL(0);
                LoadProgramData();
            }
            catch (Exception ex)
            {
            }
        }
    }
}