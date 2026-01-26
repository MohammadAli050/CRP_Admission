using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Admission.Admission.PostgraduateDiploma
{
    public partial class PostgraduateDiplomaPurchaseNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["value"]))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator." +
                    " Note : Please do not make any payment without contacting the administrator. Error Code = PN01X001PL ?type=danger", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    bool? isMultiple = null;

                    //string value = Decrypt.DecryptString(this.Request.QueryString["value"].ToString());
                    //string[] values = value.Split(';');
                    string value = Request.QueryString["value"].ToString();
                    string[] values = value.Split(';');
                    long candidateId = Convert.ToInt64(values[0]);
                    long candidatePaymentID = Convert.ToInt64(values[1]);
                    long? candidateFormSerialID = Convert.ToInt64(values[2]);
                    if (values[3] == "0")
                    {
                        isMultiple = false;
                    }
                    else if (values[3] == "1")
                    {
                        isMultiple = true;
                    }

                    long admUnitId = Convert.ToInt64(values[4]);
                    int educationCategoryId = Convert.ToInt32(values[5]);

                    if (isMultiple == false) //if not multiple purchase...single form purchase from purchaseForm.aspx.
                    {
                        using (var db = new CandidateDataManager())
                        {
                            DAL.PostgraduateDiplomaAdmissionUnit admUnit = db.AdmissionDB.PostgraduateDiplomaAdmissionUnits.Find(admUnitId);
                            DAL.PostgraduateDiplomaCandidatePayment candidatePaymentObj = db.AdmissionDB.PostgraduateDiplomaCandidatePayments.Find(candidatePaymentID);
                            DAL.PostgraduateDiplomaCandidateFormSl candidateFormSlObj = db.AdmissionDB.PostgraduateDiplomaCandidateFormSls.Find(Convert.ToInt64(candidateFormSerialID));
                            DAL.PostgraduateDiplomaBasicInfo candidateBasicInfoObj = db.AdmissionDB.PostgraduateDiplomaBasicInfoes.Find(candidatePaymentObj.CandidateID);
                            if (candidatePaymentObj != null && admUnit != null && candidateFormSlObj != null && candidateBasicInfoObj != null)
                            {

                                DAL.PostgraduateDiplomaAdmissionSetup admSetupObj = null;
                                using (var dbAdmSetup = new OfficeDataManager())
                                {
                                    admSetupObj = dbAdmSetup.AdmissionDB.PostgraduateDiplomaAdmissionSetups.Find(candidateFormSlObj.AdmissionSetupID);
                                }

                                if (admSetupObj != null)
                                {
                                    if (admSetupObj.Attribute1 == null)
                                    {
                                        //btnSubmit_Bkash.Visible = false;
                                        //btnSubmit.Visible = true;
                                    }
                                    else if (admSetupObj.Attribute1 != null && (admSetupObj.Attribute1 == "bKash" || admSetupObj.Attribute1 == "01769028780"))
                                    {
                                        //btnSubmit_Bkash.Visible = false; //btnSubmit_Bkash.Visible = true; its change to false, Because aggriment with BKash is canceled.
                                        //btnSubmit.Visible = true;

                                    }
                                    else if (admSetupObj.Attribute1 != null && admSetupObj.Attribute1 == "fpg")
                                    {
                                        //btnSubmit_Bkash.Visible = false; //btnSubmit_Bkash.Visible = true; its change to false, Because aggriment with BKash is canceled.
                                        //btnSubmit_Fpg.Visible = true;

                                    }
                                }

                                lblCandidateName.Text = candidateBasicInfoObj.FirstName;
                                string candidateName = candidateBasicInfoObj.FirstName;
                                string candidateEmail = candidateBasicInfoObj.Email;
                                lblPaymentID.Text = candidatePaymentObj.PaymentId.ToString();
                                lblAmount.Text = candidatePaymentObj.Amount.ToString();
                                lblPrograms.Text = admUnit.UnitName;
                                lblNote.Text = "Please note down your Payment ID and save this number for future reference. " +
                                    "An email will be sent to <strong>" + candidateEmail + "</strong> containing your Payment ID " +
                                    "and an SMS will be sent with <strong>UserId and Password</strong>. " +
                                    "Using this Credentials to Login and fillup further information.";





                                List<DAL.SPPostgraduateDiplomaGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification_Result> result = null;
                                using (var db1 = new OfficeDataManager())
                                {
                                    result = db1.AdmissionDB.SPPostgraduateDiplomaGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification(candidatePaymentObj.PaymentId, null).ToList();
                                }

                                if (result[0].Count_SMS > 0 && result[0].Count_Email > 0)
                                {
                                    //....Candidate already have Email and SMS. dont send again
                                }
                                else
                                {

                                    DAL.PostgraduateDiplomaCandidateUser pgdcu = null;
                                    DAL.PostgraduateDiplomaBasicInfo pgdbi = null;
                                    using (var db1 = new CandidateDataManager())
                                    {
                                        pgdbi = db1.AdmissionDB.PostgraduateDiplomaBasicInfoes.Where(x => x.ID == candidateId).FirstOrDefault();

                                        if (pgdbi != null)
                                        {
                                            pgdcu = db1.AdmissionDB.PostgraduateDiplomaCandidateUsers.Where(x => x.ID == pgdbi.CandidateUserID).FirstOrDefault();
                                        }
                                    }

                                    string mailBody = "Dear " + candidateBasicInfoObj.FirstName + ", <br/><br/>" +
                                                        "Thank you for applying at Bangladesh University of Professionals (BUP). Please proceed for payment or you can pay later following the link : " +  //"Thank you for applying at Bangladesh University of Professionals (BUP). Please check the information below about your payment: " +
                                                        "<br/><br/>" +
                                                        "<br/><br/>" +
                                                        "Applied For :" + "<br/>" + admUnit.UnitName + ". <br/>" +
                                                        "Payment ID: <strong>" + candidatePaymentObj.PaymentId.ToString() +
                                                        "<br/>" +
                                                        "User ID : " + pgdcu.UsernameLoginId.ToString() +
                                                        "<br/>" +
                                                        "Password : " + pgdcu.Password.ToString() +
                                                        "<br/>" +
                                                        "Using this Credentials to Login and fillup further information." +
                                                        "<br/>" +
                                                        "</p>" +
                                                        "<br/>" +
                                                        "Regards," +
                                                        "<br/>" +
                                                        "Admin" +
                                                        "Bangladesh University of Professionals (BUP)" +
                                                        "</p>"; //"<br/>" + "<p style='color: green;'>Important : You must login and fill up the Application Form after successful payment.</p>" + "<p>" +

                                    string fromAddress = "no-reply-2@bup.edu.bd";
                                    string senderName = "BUP Admission";
                                    string subject = "Payment ID for Your Application";

                                    bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);

                                    if (isSentEmail == true)
                                    {
                                        DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                        eLog.MessageBody = mailBody;
                                        eLog.MessageSubject = subject;
                                        eLog.Page = "PostgraduateDiplomaPurchaseNotification.aspx";
                                        eLog.SentBy = "System";
                                        eLog.StudentId = candidatePaymentObj.CandidateID;
                                        eLog.ToAddress = candidateEmail;
                                        eLog.ToName = candidateName;
                                        eLog.DateSent = DateTime.Now;
                                        eLog.FromAddress = fromAddress;
                                        eLog.FromName = senderName;
                                        eLog.Attribute1 = "Success";

                                        LogWriter.EmailLog(eLog);
                                    }
                                    else if (isSentEmail == false)
                                    {
                                        DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                        eLog.MessageBody = mailBody;
                                        eLog.MessageSubject = subject;
                                        eLog.Page = "PostgraduateDiplomaPurchaseNotification.aspx";
                                        eLog.SentBy = "System";
                                        eLog.StudentId = candidatePaymentObj.CandidateID;
                                        eLog.ToAddress = candidateEmail;
                                        eLog.ToName = candidateName;
                                        eLog.DateSent = DateTime.Now;
                                        eLog.FromAddress = fromAddress;
                                        eLog.FromName = senderName;
                                        eLog.Attribute1 = "Failed";

                                        LogWriter.EmailLog(eLog);
                                    }

                                    GetSendingInfo(candidatePaymentObj.CandidateID, value, candidatePaymentObj, candidateBasicInfoObj);
                                }


                            }
                            else
                            {
                                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code = PN01X002IE ?type=danger", false);
                            }
                        }//end using
                    }
                    //else if (isMultiple == true) //if multiple purchase...multiple form purchase from MultipleApplication.aspx.
                    //{
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        DAL.CandidatePayment candidatePaymentObj = db.GetCandidatePaymentByID_AD(candidatePaymentID);
                    //        if (candidatePaymentObj != null)
                    //        {
                    //            List<DAL.CandidateFormSl> candFormSerialList = candidatePaymentObj.CandidateFormSls.ToList();
                    //            List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
                    //            List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();


                    //            bool showBkashButton = true;
                    //            if (candFormSerialList.Count() > 0)
                    //            {
                    //                foreach (var item in candFormSerialList)
                    //                {
                    //                    DAL.AdmissionSetup admSetupItem = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                    //                    if (admSetupItem != null)
                    //                    {
                    //                        admSetupList.Add(admSetupItem);
                    //                        if (admSetupItem.Attribute1 == null)
                    //                        {
                    //                            showBkashButton = false;
                    //                        }
                    //                    }
                    //                }
                    //            }

                    //            if (showBkashButton == false)
                    //            {
                    //                btnSubmit_Bkash.Visible = false;
                    //            }


                    //            if (admSetupList.Count() > 0)
                    //            {
                    //                foreach (var item in admSetupList)
                    //                {
                    //                    List<DAL.AdmissionUnit> admUnitListForEachAdmSetup = db.AdmissionDB.AdmissionUnits
                    //                        .Where(a => a.ID == item.AdmissionUnitID).ToList();
                    //                    if (admUnitListForEachAdmSetup.Count() > 0)
                    //                    {
                    //                        admUnitList.AddRange(admUnitListForEachAdmSetup);
                    //                    }
                    //                }
                    //            }

                    //            string admUnitProgramStr = string.Empty;
                    //            if (admUnitList.Count() > 0)
                    //            {
                    //                foreach (var item in admUnitList)
                    //                {
                    //                    admUnitProgramStr += item.UnitName + "<br />";
                    //                }
                    //            }


                    //            lblCandidateName.Text = candidatePaymentObj.BasicInfo.FirstName;
                    //            string candidateName = candidatePaymentObj.BasicInfo.FirstName;
                    //            string candidateEmail = candidatePaymentObj.BasicInfo.Email;
                    //            lblPaymentID.Text = candidatePaymentObj.PaymentId.ToString();
                    //            lblAmount.Text = candidatePaymentObj.Amount.ToString();
                    //            lblPrograms.Text = admUnitProgramStr;
                    //            lblNote.Text = "Please note down your Payment ID and save this number for future reference. " +
                    //                "An email will be sent to <strong>" + candidateEmail + "</strong> containing your Payment ID.<br/>" +
                    //                "Using this Payment ID you can pay later.";





                    //            List<DAL.SPGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification_Result> result = null;
                    //            using (var db1 = new OfficeDataManager())
                    //            {
                    //                result = db1.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification(candidatePaymentObj.PaymentId, null).ToList();
                    //            }

                    //            if (result[0].Count_SMS > 0 && result[0].Count_Email > 0)
                    //            {
                    //                //....Candidate already have Email and SMS. dont send again
                    //            }
                    //            else
                    //            {
                    //                //....Candidate dont have Email and SMS. Sending First time Email and SMS

                    //                string mailBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + ", <br/><br/>" +
                    //                    "Thank you for applying at Bangladesh University of Professionals (BUP). Please proceed for payment or you can pay later following the link : " +  //"Thank you for applying at Bangladesh University of Professionals (BUP). Please check the information below about your payment: " +
                    //                    "<br/><br/>" +
                    //                    "<b>Link : </b><a href='https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value + "'> https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=value </a>" +
                    //                    "<br/><br/>" +
                    //                    "Applied For :" + "<br/>" + admUnitProgramStr + ". <br/>" +
                    //                    "Payment ID: <strong>" + candidatePaymentObj.PaymentId.ToString() +
                    //                    "<br/>" +
                    //                    "</p>" +
                    //                    "<p style='color: green;'>Important : You must login and fill up the Application Form after successful payment.</p>" +
                    //                    "<p>" +
                    //                    "<br/>" +
                    //                    "Regards," +
                    //                    "<br/>" +
                    //                    "Admin" +
                    //                    "<br/>" +
                    //                    "Bangladesh University of Professionals (BUP)" +
                    //                    "</p>";
                    //                //"Regards, <br/>" ICT Centre,
                    //                string fromAddress = "no-reply-2@bup.edu.bd";
                    //                string senderName = "BUP Admission";
                    //                string subject = "Payment ID for Your Application";

                    //                //bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);
                    //                bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);

                    //                if (isSentEmail == true)
                    //                {
                    //                    DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    //                    eLog.MessageBody = mailBody;
                    //                    eLog.MessageSubject = subject;
                    //                    eLog.Page = "PurchaseNotification.aspx";
                    //                    eLog.SentBy = "System";
                    //                    eLog.StudentId = candidatePaymentObj.CandidateID;
                    //                    eLog.ToAddress = candidateEmail;
                    //                    eLog.ToName = candidateName;
                    //                    eLog.DateSent = DateTime.Now;
                    //                    eLog.FromAddress = fromAddress;
                    //                    eLog.FromName = senderName;
                    //                    eLog.Attribute1 = "Success";

                    //                    LogWriter.EmailLog(eLog);
                    //                }
                    //                else if (isSentEmail == false)
                    //                {
                    //                    DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    //                    eLog.MessageBody = mailBody;
                    //                    eLog.MessageSubject = subject;
                    //                    eLog.Page = "PurchaseNotification.aspx";
                    //                    eLog.SentBy = "System";
                    //                    eLog.StudentId = candidatePaymentObj.CandidateID;
                    //                    eLog.ToAddress = candidateEmail;
                    //                    eLog.ToName = candidateName;
                    //                    eLog.DateSent = DateTime.Now;
                    //                    eLog.FromAddress = fromAddress;
                    //                    eLog.FromName = senderName;
                    //                    eLog.Attribute1 = "Failed";

                    //                    LogWriter.EmailLog(eLog);
                    //                }

                    //                GetSendingInfo(candidatePaymentObj.CandidateID, value, candidatePaymentObj);
                    //            }


                    //        }
                    //        else
                    //        {
                    //            Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code = PN01X002IE ?type=danger", false);
                    //        }
                    //    }//end using
                    //}//end if-else isMultiple
                }
            }
        }






        private void GetSendingInfo(long? candidateId, string value, DAL.PostgraduateDiplomaCandidatePayment candidatePaymentObj, DAL.PostgraduateDiplomaBasicInfo candidateBasicInfoObj)
        {
            if (candidateId != null)
            {
                if (candidateId > 0)
                {
                    DAL.PostgraduateDiplomaBasicInfo candidate = null;
                    //DAL.CandidateUser candidateUser = null;

                    //string candidateUsername = null;
                    //string candidatePassword = null;
                    string candidateSmsPhone = null;
                    string candidateEmail = null;

                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.PostgraduateDiplomaBasicInfoes.Find(candidateId);
                    }

                    if (candidate != null)
                    {
                        candidateEmail = candidate.Email;
                        candidateSmsPhone = candidate.SMSPhone;
                        //using (var db = new CandidateDataManager())
                        //{
                        //    candidateUser = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                        //}
                    }

                    //if (candidateUser != null)
                    //{
                    //    candidateUsername = candidateUser.UsernameLoginId;
                    //    candidatePassword = candidateUser.Password;
                    //}

                    if (!string.IsNullOrEmpty(candidateSmsPhone) && !string.IsNullOrEmpty(candidateEmail))  //!string.IsNullOrEmpty(candidateUsername) && !string.IsNullOrEmpty(candidatePassword) &&
                    {
                        SendSms(candidateSmsPhone, candidate.ID, value, candidatePaymentObj, candidateBasicInfoObj);
                        //SendEmail(candidateEmail, candidateUsername, candidatePassword, candidate.ID);
                    }
                }
            }
        }



        private void SendSms(string smsPhone, long candidateId, string value, DAL.PostgraduateDiplomaCandidatePayment candidatePaymentObj, DAL.PostgraduateDiplomaBasicInfo candidateBasicInfoObj)
        {
            if (!string.IsNullOrEmpty(smsPhone) && smsPhone.Count() == 14 && smsPhone.Contains("+"))
            {

                DAL.PostgraduateDiplomaCandidateUser pgdcu = null;
                DAL.PostgraduateDiplomaBasicInfo pgdbi = null;
                using (var db = new CandidateDataManager())
                {
                    pgdbi = db.AdmissionDB.PostgraduateDiplomaBasicInfoes.Where(x => x.ID == candidateId).FirstOrDefault();

                    if (pgdbi != null)
                    {
                        pgdcu = db.AdmissionDB.PostgraduateDiplomaCandidateUsers.Where(x => x.ID == pgdbi.CandidateUserID).FirstOrDefault();
                    }
                }

                //string messageBody = "Dear " + candidateBasicInfoObj.FirstName + "," + "\n" +
                //                    "Payment ID : " + candidatePaymentObj.PaymentId.ToString() + "\n" +
                //                    "User ID : " + pgdcu.UsernameLoginId.ToString() + "\n" +
                //                    "Password : " + pgdcu.Password.ToString() + "\n" +
                //                    "Using this Credentials to Login and fillup further information." + "\n" +
                //                    "BUP";

                string messageBody = "Dear " + candidateBasicInfoObj.FirstName + "," + 
                                        " Payment ID : " + candidatePaymentObj.PaymentId.ToString() + 
                                        " User ID : " + pgdcu.UsernameLoginId.ToString() + 
                                        " Password : " + pgdcu.Password.ToString() + 
                                        " Using this Credentials to Login and fillup further information." + 
                                        " BUP";

                string stringData = SMSUtility.Send(smsPhone, messageBody);

                string statusT = JObject.Parse(stringData)["statusCode"].ToString();

                if (statusT != "200") //if sms sending fails
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending failed in PostGraduateDiplomaPurchaseNotification.aspx";
                    smsLog.Attribute2 = "Failed";
                    smsLog.Attribute3 = null;
                    smsLog.CreatedBy = candidateId;
                    smsLog.CreatedDate = DateTime.Now;
                    smsLog.CurrentSMSReferenceNo = stringData;
                    smsLog.Message = messageBody;
                    smsLog.StudentId = candidateId;
                    smsLog.PhoneNo = smsPhone;
                    smsLog.SenderUserId = -99;
                    smsLog.SentReferenceId = null;
                    smsLog.SentSMSId = null;
                    smsLog.SmsSendDate = DateTime.Now;
                    smsLog.SmsType = -1;

                    LogWriter.SmsLog(smsLog);
                }
                else //if sms sending passed
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending successful PostGraduateDiplomaPurchaseNotification.aspx";
                    smsLog.Attribute2 = "Success";
                    smsLog.Attribute3 = null;
                    smsLog.CreatedBy = candidateId;
                    smsLog.CreatedDate = DateTime.Now;
                    smsLog.CurrentSMSReferenceNo = stringData;
                    smsLog.Message = messageBody;
                    smsLog.StudentId = candidateId;
                    smsLog.PhoneNo = smsPhone;
                    smsLog.SenderUserId = -99;
                    smsLog.SentReferenceId = null;
                    smsLog.SentSMSId = null;
                    smsLog.SmsSendDate = DateTime.Now;
                    smsLog.SmsType = -1;

                    LogWriter.SmsLog(smsLog);
                }
            }
        }



        //payment submit for ssl/foster
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DAL.CertificateBasicInfo candidate = null;
            DAL.CertificateCandidateFormSl cFormSl = null;
            DAL.CertificateCandidatePayment cPayment = null;
            DAL.Store store = null;
            DAL.StoreFoster storeFoster = null;
            DAL.CertificateAdmissionSetup admSetup = null;
            List<DAL.CertificateCandidateFormSl> cFormSlMultiple = null;
            List<DAL.CertificateAdmissionSetup> admSetupMultiple = null;

            if (string.IsNullOrEmpty(Request.QueryString["value"]))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator." +
                    "Note : Please do not make any payment without contacting the administrator.[Error Code = PN01X001PL]?type=danger", false);
            }
            else //if url value is present.
            {
                //TODO decrypt url parameter
                //string value = Decrypt.DecryptString(this.Request.QueryString["value"].ToString());
                //string[] values = value.Split(';');
                string value = Request.QueryString["value"].ToString();
                string[] values = value.Split(';');

                bool? isMultiple = null;

                long candidateId = Convert.ToInt64(values[0]);
                long candidatePaymentID = Convert.ToInt64(values[1]);
                long candidateFormSerialID = Convert.ToInt64(values[2]);
                if (values[3] == "0")
                {
                    isMultiple = false;
                }
                else if (values[3] == "1")
                {
                    isMultiple = true;
                }
                string admUnitName = values[4].ToString();
                int educationCategoryId = Convert.ToInt32(values[5]);


                string successUrlStr = null;
                string failUrlStr = null;
                string cancelUrlStr = null;

                #region isMultiple False
                if (isMultiple == false)//not a multiple purchase, so it can be for undergrad or masters.
                {

                    if (candidateId > 0 && candidatePaymentID > 0 && candidateFormSerialID > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            candidate = db.AdmissionDB.CertificateBasicInfoes.Find(candidateId);
                            cFormSl = db.AdmissionDB.CertificateCandidateFormSls.Find(candidateFormSerialID);
                            cPayment = db.AdmissionDB.CertificateCandidatePayments.Find(candidatePaymentID);
                            admSetup = db.AdmissionDB.CertificateAdmissionSetups.Find(cFormSl.AdmissionSetupID);

                            if (admSetup != null)
                            {
                                if (admSetup.Attribute2 == "SSL")
                                {
                                    store = db.AdmissionDB.Stores.Find(admSetup.StoreID);
                                    storeFoster = null;
                                }
                                else if (admSetup.Attribute2 == "FPG")
                                {
                                    store = null;
                                    storeFoster = db.AdmissionDB.StoreFosters
                                        .Where(c => c.ID == admSetup.StoreID && c.IsActive == true)
                                        .FirstOrDefault();
                                }
                            }
                        }
                    }

                    if (educationCategoryId == 1) //if Certificate Program
                    {
                        if (store != null && storeFoster == null) //if ssl
                        {
                            successUrlStr = store.SuccessUrl;
                            failUrlStr = store.FailedUrl;
                            cancelUrlStr = store.CancelledUrl;

                            if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                            {
                                if (store.IsMultipleAllowed == false)
                                {

                                    NameValueCollection PostData = new NameValueCollection
                                    {
                                        {"total_amount", cPayment.Amount.ToString()},
                                        {"tran_id", cPayment.PaymentId.ToString()},
                                        {"success_url", store.SuccessUrl},
                                        {"fail_url", store.FailedUrl},
                                        {"cancel_url", store.CancelledUrl},
                                        {"version", "3.00"},
                                        {"cus_name", candidate.FirstName},
                                        {"cus_email", candidate.Email},
                                        {"cus_phone", candidate.SMSPhone},
                                        {"value_a", candidate.ID.ToString()},
                                        {"value_b", candidate.FirstName},
                                        {"value_c", cPayment.Amount.ToString()},
                                        {"value_d", store.ID.ToString()}
                                    };

                                    string storeId = Decrypt.DecryptString(store.StoreId);
                                    string password = Decrypt.DecryptString(store.StorePass);

                                    if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                                    {
                                        SSLCommerz sslcz = new SSLCommerz(storeId, password);
                                        String response = sslcz.InitiateTransaction(PostData);
                                        Response.Redirect(response);
                                    }
                                }
                            }
                        }
                        else if (store == null && storeFoster != null) // if fpg
                        {
                            if (storeFoster.IsMultipleAllowed == true)  //(false) was set because MSC cand buy multiple form. but for now its set true
                            {
                                string secretKey = storeFoster.SecurityKey;
                                string mcnt_AccessCode = storeFoster.AccessCode;
                                string mcnt_TxnNo = storeFoster.MerchantShortName + cPayment.PaymentId.ToString();
                                string mcnt_ShortName = storeFoster.MerchantShortName;
                                string mcnt_OrderNo = cPayment.PaymentId.ToString();
                                string mcnt_ShopId = storeFoster.ShopId;
                                string mcnt_Amount = cPayment.Amount.ToString();
                                string mcnt_Currency = "BDT";

                                string successURL = "https://admission.bup.edu.bd/Admission/successurlfpgCertificate";
                                //string cancleURL = "https://admission.bup.edu.bd/Admission/cancelurlfpg";
                                //string failURL = "https://admission.bup.edu.bd/Admission/failurlfpg";


                                string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                                    mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower();

                                NameValueCollection urlParam = new NameValueCollection
                                    {
                                        {"mcnt_TxnNo", mcnt_TxnNo},
                                        {"mcnt_ShortName", mcnt_ShortName},
                                        {"mcnt_OrderNo", mcnt_OrderNo},
                                        {"mcnt_ShopId", mcnt_ShopId},
                                        {"mcnt_Amount", mcnt_Amount},
                                        {"mcnt_Currency", mcnt_Currency},
                                        {"cust_InvoiceTo", candidate.FirstName},
                                        {"cust_CustomerServiceName", "BUP Admission"},
                                        {"cust_CustomerName", candidate.FirstName},
                                        {"cust_CustomerEmail", candidate.Email},
                                        {"cust_CustomerAddress", "Dhaka"},
                                        {"cust_CustomerContact", candidate.SMSPhone },
                                        {"cust_CustomerGender", candidate.GenderID.ToString()},
                                        {"cust_CustomerCity", "Dhaka"},
                                        {"cust_CustomerState", "Dhaka"},
                                        {"cust_CustomerPostcode", "1216"},
                                        {"cust_CustomerCountry", "Bangladesh"},
                                        {"cust_Billingaddress", "Bangladesh"},
                                        {"cust_ShippingAddress", "Bangladesh"},
                                        {"cust_orderitems", cPayment.PaymentId.ToString()},
                                        {"success_url", successURL},
                                        {"cancel_url", storeFoster.CancelUrl},
                                        {"fail_url", storeFoster.FailUrl},
                                        {"merchentdomainname", "admission.bup.edu.bd"},
                                        {"merchentip", "202.79.20.181"},
                                        {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                                    }; ////storeFoster.SuccessUrl

                                string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                                string response = storeFoster.Uri + queryString;
                                Response.Redirect(response);
                            }
                            else if (storeFoster.IsMultipleAllowed == false) //---for those who IsMultiple false set in AdmissionSetup
                            {
                                string secretKey = storeFoster.SecurityKey;
                                string mcnt_AccessCode = storeFoster.AccessCode;
                                string mcnt_TxnNo = storeFoster.MerchantShortName + cPayment.PaymentId.ToString();
                                string mcnt_ShortName = storeFoster.MerchantShortName;
                                string mcnt_OrderNo = cPayment.PaymentId.ToString();
                                string mcnt_ShopId = storeFoster.ShopId;
                                string mcnt_Amount = cPayment.Amount.ToString();
                                string mcnt_Currency = "BDT";


                                string successURL = "https://admission.bup.edu.bd/Admission/successurlfpgCertificate";
                                //string cancleURL = "https://admission.bup.edu.bd/Admission/cancelurlfpg";
                                //string failURL = "https://admission.bup.edu.bd/Admission/failurlfpg";


                                string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                                    mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower();

                                NameValueCollection urlParam = new NameValueCollection
                                    {
                                        {"mcnt_TxnNo", mcnt_TxnNo},
                                        {"mcnt_ShortName", mcnt_ShortName},
                                        {"mcnt_OrderNo", mcnt_OrderNo},
                                        {"mcnt_ShopId", mcnt_ShopId},
                                        {"mcnt_Amount", mcnt_Amount},
                                        {"mcnt_Currency", mcnt_Currency},
                                        {"cust_InvoiceTo", candidate.FirstName},
                                        {"cust_CustomerServiceName", "BUP Admission"},
                                        {"cust_CustomerName", candidate.FirstName},
                                        {"cust_CustomerEmail", candidate.Email},
                                        {"cust_CustomerAddress", "Dhaka"},
                                        {"cust_CustomerContact", candidate.SMSPhone },
                                        {"cust_CustomerGender", candidate.GenderID.ToString()},
                                        {"cust_CustomerCity", "Dhaka"},
                                        {"cust_CustomerState", "Dhaka"},
                                        {"cust_CustomerPostcode", "1216"},
                                        {"cust_CustomerCountry", "Bangladesh"},
                                        {"cust_Billingaddress", "Bangladesh"},
                                        {"cust_ShippingAddress", "Bangladesh"},
                                        {"cust_orderitems", cPayment.PaymentId.ToString()},
                                        {"success_url", successURL},
                                        {"cancel_url", storeFoster.CancelUrl},
                                        {"fail_url", storeFoster.FailUrl},
                                        {"merchentdomainname", "admission.bup.edu.bd"},
                                        {"merchentip", "202.79.20.181"},
                                        {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                                    };  ////storeFoster.SuccessUrl

                                string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                                string response = storeFoster.Uri + queryString;
                                Response.Redirect(response);
                            }
                        }
                    }

                }
                #endregion
                //-----------------------------------------
                #region isMultiple true
                //else if (isMultiple == true)
                //{
                //    DAL.Store sslStoreMultiple = null;
                //    DAL.StoreFoster fosterStoreMultiple = null;


                //    if (candidateId > 0 && candidatePaymentID > 0)
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                //            cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                //            cFormSlMultiple = db.AdmissionDB.CandidateFormSls.Where(c => c.CandidatePaymentID == candidatePaymentID).ToList();
                //        }
                //    }

                //    admSetupMultiple = new List<DAL.AdmissionSetup>();
                //    if (cFormSlMultiple != null)
                //    {
                //        if (cFormSlMultiple.Count > 0)
                //        {
                //            foreach (var cform in cFormSlMultiple)
                //            {
                //                DAL.AdmissionSetup tempAdmSetup = null;
                //                using (var db = new OfficeDataManager())
                //                {
                //                    tempAdmSetup = db.AdmissionDB.AdmissionSetups.Find(cform.AdmissionSetupID);
                //                }
                //                if (tempAdmSetup != null)
                //                {
                //                    admSetupMultiple.Add(tempAdmSetup);
                //                }
                //            }
                //        }
                //    }

                //    if (admSetupMultiple != null)
                //    {
                //        if (admSetupMultiple.Count > 0)
                //        {
                //            if (admSetupMultiple.Where(c => c.Attribute2 == "SSL").ToList().Count > 0)
                //            {
                //                using (var db = new OfficeDataManager())
                //                {
                //                    sslStoreMultiple = db.GetActiveSSLStoreForMultiplepurchase(true, true);
                //                }
                //            }
                //            else if (admSetupMultiple.Where(c => c.Attribute2 == "FPG").ToList().Count > 0)
                //            {
                //                using (var db = new OfficeDataManager())
                //                {
                //                    fosterStoreMultiple = db.GetFPGStoreActiveMultiplePurchaseStore(true, true);
                //                }
                //            }//end if-else
                //        }//end if admSetupMultiple.Count > 0
                //    }//end if admSetupMultiple != null

                //    if (sslStoreMultiple != null && fosterStoreMultiple == null) //if ssl
                //    {
                //        if (cPayment != null && sslStoreMultiple != null && candidate != null)
                //        {
                //            successUrlStr = sslStoreMultiple.SuccessUrl;
                //            failUrlStr = sslStoreMultiple.FailedUrl;
                //            cancelUrlStr = sslStoreMultiple.CancelledUrl;

                //            if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                //            {
                //                if (sslStoreMultiple.IsMultipleAllowed == true)
                //                {

                //                    NameValueCollection PostData = new NameValueCollection {
                //                    {"total_amount", cPayment.Amount.ToString()},
                //                    {"tran_id", cPayment.PaymentId.ToString()},
                //                    {"success_url", sslStoreMultiple.SuccessUrl},
                //                    {"fail_url", sslStoreMultiple.FailedUrl},
                //                    {"cancel_url", sslStoreMultiple.CancelledUrl},
                //                    {"version", "3.00"},
                //                    {"cus_name", candidate.FirstName},
                //                    {"cus_email", candidate.Email},
                //                    {"cus_phone", candidate.SMSPhone},
                //                    {"value_a", candidate.ID.ToString()},
                //                    {"value_b", candidate.FirstName},
                //                    {"value_c", cPayment.Amount.ToString()},
                //                    {"value_d", sslStoreMultiple.ID.ToString()} };

                //                    string storeId = Decrypt.DecryptString(sslStoreMultiple.StoreId);
                //                    string password = Decrypt.DecryptString(sslStoreMultiple.StorePass);

                //                    if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                //                    {
                //                        SSLCommerz sslcz = new SSLCommerz(storeId, password);
                //                        String response = sslcz.InitiateTransaction(PostData);
                //                        Response.Redirect(response);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    else if (sslStoreMultiple == null && fosterStoreMultiple != null) //if fpg
                //    {
                //        if (cPayment != null && fosterStoreMultiple != null && candidate != null)
                //        {
                //            if (fosterStoreMultiple.IsMultipleAllowed == true)
                //            {
                //                string secretKey = fosterStoreMultiple.SecurityKey;
                //                string mcnt_AccessCode = fosterStoreMultiple.AccessCode;
                //                string mcnt_TxnNo = fosterStoreMultiple.MerchantShortName + cPayment.PaymentId.ToString();
                //                string mcnt_ShortName = fosterStoreMultiple.MerchantShortName;
                //                string mcnt_OrderNo = cPayment.PaymentId.ToString();
                //                string mcnt_ShopId = fosterStoreMultiple.ShopId;
                //                string mcnt_Amount = cPayment.Amount.ToString();
                //                string mcnt_Currency = "BDT";

                //                string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                //                    mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey);

                //                NameValueCollection urlParam = new NameValueCollection
                //                    {
                //                        {"mcnt_TxnNo", mcnt_TxnNo},
                //                        {"mcnt_ShortName", mcnt_ShortName},
                //                        {"mcnt_OrderNo", mcnt_OrderNo},
                //                        {"mcnt_ShopId", mcnt_ShopId},
                //                        {"mcnt_Amount", mcnt_Amount},
                //                        {"mcnt_Currency", mcnt_Currency},
                //                        {"cust_InvoiceTo", candidate.FirstName},
                //                        {"cust_CustomerServiceName", "BUP Admission"},
                //                        {"cust_CustomerName", candidate.FirstName},
                //                        {"cust_CustomerEmail", candidate.Email},
                //                        {"cust_CustomerAddress", "Dhaka"},
                //                        {"cust_CustomerContact", candidate.SMSPhone },
                //                        {"cust_CustomerGender", candidate.GenderID.ToString()},
                //                        {"cust_CustomerCity", "Dhaka"},
                //                        {"cust_CustomerState", "Dhaka"},
                //                        {"cust_CustomerPostcode", "1216"},
                //                        {"cust_CustomerCountry", "Bangladesh"},
                //                        {"cust_Billingaddress", "Bangladesh"},
                //                        {"cust_ShippingAddress", "Bangladesh"},
                //                        {"cust_orderitems", cPayment.PaymentId.ToString()},
                //                        {"success_url", fosterStoreMultiple.SuccessUrl},
                //                        {"cancel_url", fosterStoreMultiple.CancelUrl},
                //                        {"fail_url", fosterStoreMultiple.FailUrl},
                //                        {"merchentdomainname", "admission.bup.edu.bd"},
                //                        {"merchentip", "202.79.20.181"},
                //                        {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                //                    };

                //                string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                //                string response = fosterStoreMultiple.Uri + queryString;
                //                Response.Redirect(response);
                //            }
                //        }
                //    }

                //    //if (candidatePaymentID > 0 && educationCategoryId == 4)//for bachelors only.
                //    //{
                //    //    using (var db = new CandidateDataManager())
                //    //    {
                //    //        cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                //    //        store = db.AdmissionDB.Stores.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
                //    //    }

                //    //    if (cPayment != null)
                //    //    {
                //    //        using (var db = new CandidateDataManager())
                //    //        {
                //    //            candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                //    //        }
                //    //    }
                //    //}

                //    //if (cPayment != null && store != null && candidate != null)
                //    //{
                //    //    successUrlStr = store.SuccessUrl;
                //    //    failUrlStr = store.FailedUrl;
                //    //    cancelUrlStr = store.CancelledUrl;

                //    //    if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                //    //    {
                //    //        if (store.IsMultipleAllowed == true)
                //    //        {

                //    //            NameValueCollection PostData = new NameValueCollection {
                //    //            {"total_amount", cPayment.Amount.ToString()},
                //    //            {"tran_id", cPayment.PaymentId.ToString()},
                //    //            {"success_url", store.SuccessUrl},
                //    //            {"fail_url", store.FailedUrl},
                //    //            {"cancel_url", store.CancelledUrl},
                //    //            {"version", "3.00"},
                //    //            {"cus_name", candidate.FirstName},
                //    //            {"cus_email", candidate.Email},
                //    //            {"cus_phone", candidate.SMSPhone},
                //    //            {"value_a", candidate.ID.ToString()},
                //    //            {"value_b", candidate.FirstName},
                //    //            {"value_c", cPayment.Amount.ToString()},
                //    //            {"value_d", store.ID.ToString()} };

                //    //            string storeId = Decrypt.DecryptString(store.StoreId);
                //    //            string password = Decrypt.DecryptString(store.StorePass);

                //    //            if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                //    //            {
                //    //                SSLCommerz sslcz = new SSLCommerz(storeId, password);
                //    //                String response = sslcz.InitiateTransaction(PostData);
                //    //                Response.Redirect(response);
                //    //            }
                //    //        }
                //    //    }
                //    //}

                //} //end if-else isMultiple
                #endregion

            }//end if-else check url param
        }

        protected void btnSubmitPayLater_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://admission.bup.edu.bd/Admission/Home", true);
        }


    }
}