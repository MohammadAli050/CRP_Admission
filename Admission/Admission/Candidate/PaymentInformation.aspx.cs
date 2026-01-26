using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Script.Serialization;
using System.IO;

namespace Admission.Admission.Candidate
{
    public partial class PaymentInformation : System.Web.UI.Page
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        long paymentId = -1;

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            this.Page.Form.Enctype = "multipart/form-data";

            using (var db = new CandidateDataManager())
            {
                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                    //paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID && x.IsPaid == true).Select(x => x.PaymentId).FirstOrDefault();
                }// end if(obj != null && obj.ID > 0)
            }// end using

            if (!IsPostBack)
            {
                divPayment.Visible = false;
                if (uId > 0)
                {
                    LoadPaymentInfo();

                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void LoadPaymentInfo()
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

                            var CandidatePaymentObj = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                            if (CandidatePaymentObj != null)
                                NewObj.Amount = CandidatePaymentObj.Amount==null ? "" : "$"+ CandidatePaymentObj.Amount.ToString();

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




                                            #region If All Program Is paid then payment IsPaid=1 in candidate Payment Table

                                            bool AllPaid = false;

                                            var PaymentIdList = db.AdmissionDB.CandidatePayments.AsNoTracking().Where(x => x.CandidateID == cId).ToList();
                                            if (PaymentIdList != null && PaymentIdList.Any())
                                            {
                                                foreach (var PaymentId in PaymentIdList)
                                                {
                                                    var FormSerialList = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidatePaymentID == PaymentId.ID).ToList();
                                                    if (FormSerialList != null && FormSerialList.Any())
                                                    {
                                                        foreach (var FormSerial in FormSerialList)
                                                        {
                                                            var ObjIsPaid = db.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.FormSerialId == FormSerial.ID).FirstOrDefault();
                                                            if (ObjIsPaid != null && ObjIsPaid.IsPaid == true)
                                                                AllPaid = true;
                                                            else
                                                                AllPaid = false;

                                                        }
                                                    }

                                                    if (AllPaid)
                                                    {
                                                        try
                                                        {


                                                            PaymentId.IsPaid = true;
                                                            PaymentId.ModifiedBy = -99;
                                                            PaymentId.DateModified = DateTime.Now;
                                                            using (var dbUpdate = new CandidateDataManager())
                                                            {
                                                                dbUpdate.Update<DAL.CandidatePayment>(PaymentId);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                    }

                                                }
                                            }

                                            #endregion


                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    else
                                    {
                                    }



                                    LoadPaymentInfo();

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
            public string Amount { get; set; }
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
    }
}