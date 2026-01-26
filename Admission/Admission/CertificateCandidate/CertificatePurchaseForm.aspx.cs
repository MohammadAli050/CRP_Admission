using CommonUtility;
using DATAMANAGER;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.CertificateCandidate
{
    public partial class CertificatePurchaseForm : System.Web.UI.Page
    {

        string SessionLoginCaptcha = "SessionLoginCaptcha";

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                using (var db = new OfficeDataManager())
                {
                    long admissionSetupId = Convert.ToInt64(Request.QueryString["asi"]);
                    DAL.CertificateAdmissionSetup admissionSetup = db.AdmissionDB.CertificateAdmissionSetups.Find(admissionSetupId);
                    if (admissionSetup.StartDate >= DateTime.Now && admissionSetup.EndDate <= DateTime.Now)
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                    }
                }
            }


            if (!IsPostBack)
            {
                LoadCaptcha();
                LoadDDL();
                //undergraduateInfoPanel.Visible = ShowUndergraduatePanel();
            }
        }


        private void LoadCaptcha()
        {
            Captcha captchaObj = new Captcha();

            SessionSGD.SaveObjToSession<string>(Captcha.GetStringForCaptcha(), SessionLoginCaptcha);
            //Bitmap bm = captchaObj.MakeCaptchaImage(Captcha.GetStringForCaptcha(), 160, 80, "Arial");
            Bitmap bm = captchaObj.MakeCaptchaImage(SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha), 160, 80, "Arial");

            MemoryStream ms = new MemoryStream();
            bm.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            imgCtrl.Src = "data:image/gif;base64," + base64Data;

            txtCaptcha.Text = "";
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.GroupOrSubject> groupSubjectList = db.AdmissionDB.GroupOrSubjects.Where(a => a.IsActive == true).ToList();
                List<DAL.EducationBoard> educationBoardList = db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true).ToList();
                List<DAL.ResultDivision> resultDivisionList = db.AdmissionDB.ResultDivisions.Where(a => a.IsActive == true).ToList();

                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                //DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);
            }

            DAL.CertificateAdmissionSetup admissionSetup = null;
            DAL.CertificateAdmissionUnit admissionUnit = null;

            long admissionSetupIDLong = -1;
            long admissionUnitIDLong = -1;

            if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                admissionSetupIDLong = Convert.ToInt64(Request.QueryString["asi"]);
                admissionUnitIDLong = Convert.ToInt64(Request.QueryString["aui"]);
            }
            try
            {
                using (var db = new OfficeDataManager())
                {
                    admissionSetup = db.AdmissionDB.CertificateAdmissionSetups.Find(admissionSetupIDLong);
                    admissionUnit = db.AdmissionDB.CertificateAdmissionUnits.Find(admissionUnitIDLong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }//end inner try-catch 1

            if (admissionSetup != null)
            {
                if (admissionSetup.EducationCategoryID == 1) //for Certificate Program Candidate.
                {
                    //LoadPassingYearDDLForMasters();
                }
                else
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code PF01X001IE?type=danger", false);
                }
            }
            else
            {
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code PF01X001IE?type=danger", false);
            }

        }

        //private void LoadPassingYearDDLForMasters()
        //{
        //    ddlPassingYear.Items.Clear();
        //    ddlPassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlPassingYear.AppendDataBoundItems = true;
        //    for (int i = DateTime.Now.Year; i > 1950; i--)
        //    {
        //        ddlPassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }
        //}

        private void EligibleMessage(string msg, string css)
        {
            lblEligibleMsg.Text = msg;
            lblEligibleMsg.CssClass = css;
            lblEligibleMsg.Focus();
        }





        private void PurchaseApplicationForm()
        {

            DAL.BasicInfo candExist = null;

            DateTime dob = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
            int genderId = Int32.Parse(ddlGender.SelectedValue);
            //int quotaId = Int32.Parse(ddlQuota.SelectedValue);

            using (var db = new CandidateDataManager())
            {
                candExist = db.AdmissionDB.BasicInfoes
                    .Where(c =>
                        c.DateOfBirth == dob &&
                        c.SMSPhone == txtSmsMobile.Text
                        )
                    .FirstOrDefault();
            }

            try
            {
                //---------------------------------------------------------------------------------
                DAL.CertificateAdmissionSetup admissionSetup = null;
                DAL.CertificateAdmissionUnit admissionUnit = null;

                long admissionSetupIDLong = -1;
                long admissionUnitIDLong = -1;

                if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                }
                else
                {
                    admissionSetupIDLong = Convert.ToInt64(Request.QueryString["asi"]);
                    admissionUnitIDLong = Convert.ToInt64(Request.QueryString["aui"]);
                }
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admissionSetup = db.AdmissionDB.CertificateAdmissionSetups.Find(admissionSetupIDLong);
                        admissionUnit = db.AdmissionDB.CertificateAdmissionUnits.Find(admissionUnitIDLong);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }



                #region CandidateUser/BasicInfo

                //---------------------------------------------------------------------------------
                //insert candidate user
                //---------------------------------------------------------------------------------
                DAL.CertificateCandidateUser candidateUser = new DAL.CertificateCandidateUser();
                candidateUser.UsernameLoginId = CommonLogic.AdmissionLoginId();
                candidateUser.Password = CommonLogic.AdmissionPassword();
                candidateUser.IsConfirmed = false;
                candidateUser.IsLocked = true;
                candidateUser.ValidTill = DateTime.Now.AddMonths(4);
                candidateUser.IsSentSms = false;
                candidateUser.IsSentEmail = false;
                candidateUser.RoleID = 2;
                candidateUser.CreatedBy = -99;
                candidateUser.DateCreated = DateTime.Now;

                long candidateUserIdLong = -1;
                using (var db = new CandidateDataManager())
                {
                    db.Insert<DAL.CertificateCandidateUser>(candidateUser);
                    candidateUserIdLong = candidateUser.ID;
                }

                //---------------------------------------------------------------------------------
                //insert candidate basic info
                //---------------------------------------------------------------------------------
                DAL.CertificateBasicInfo candidate = new DAL.CertificateBasicInfo();
                candidate.FirstName = txtName.Text.ToUpper();
                //candidate.MiddleName = ;
                //candidate.LastName = ;
                candidate.Mobile = txtSmsMobile.Text.Trim();
                candidate.SMSPhone = txtSmsMobile.Text.Trim();
                candidate.Email = txtEmail.Text.Trim();
                candidate.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
                candidate.CandidateUserID = candidateUserIdLong;
                candidate.IsActive = false;
                candidate.UniqueIdentifier = Guid.NewGuid();
                candidate.GenderID = Convert.ToInt32(ddlGender.SelectedValue);
                candidate.QuotaID = -1;
                candidate.CreatedBy = -99;
                candidate.DateCreated = DateTime.Now;
                candidate.GuardianPhone = "";

                long candidateIdLong = -1;
                using (var db = new CandidateDataManager())
                {
                    db.Insert<DAL.CertificateBasicInfo>(candidate);
                    candidateIdLong = candidate.ID;
                }

                #endregion

                #region CandidatePayment/CandidateFormSerial

                //---------------------------------------------------------------------------------
                //insert candidate payment
                //---------------------------------------------------------------------------------
                long candidatePaymentIDLong = -1;
                if (admissionSetup != null)
                {
                    using (var db = new CandidateDataManager())
                    {
                        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                        db.AdmissionDB.SPCertificateCandidatePaymentInsert(id_param, candidateIdLong, null, admissionSetup.AcaCalID, Convert.ToInt32(admissionUnit.UnitCode1), false, admissionSetup.Fee, -99, DateTime.Now);
                        candidatePaymentIDLong = Convert.ToInt64(id_param.Value);
                    }
                }

                //---------------------------------------------------------------------------------
                //insert candidate form serial
                //---------------------------------------------------------------------------------
                long candidateFormSerialIDLong = -1;
                if (admissionSetup != null)
                {
                    using (var db = new CandidateDataManager())
                    {
                        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                        db.AdmissionDB.SPCertificateCandidateFormSlInsert(id_param, candidateIdLong, admissionSetup.ID, admissionSetup.AcaCalID, Convert.ToInt32(admissionUnit.UnitCode1), null, candidatePaymentIDLong, DateTime.Now, -99);
                        candidateFormSerialIDLong = Convert.ToInt64(id_param.Value);
                    }
                }
                #endregion

                if (candidateIdLong > 0 && candidatePaymentIDLong > 0 && candidateFormSerialIDLong > 0)
                {

                    string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                        + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
                        + admissionSetup.EducationCategoryID + ";";

                    SendSMSAndEmail(candidateIdLong, urlParam);

                    Response.Redirect("~/Admission/CertificateCandidate/CertificatePurchaseNotification.aspx?value=" + urlParam, false);
                }
                else
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code PF01X001IE?type=danger", false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code PF01X002TC?type=danger", false);
            }
        }

        protected void SendSMSAndEmail(long candidateId, string paramValue)
        {
            try
            {

                DAL.CandidatePayment cp = null;
                DAL.CandidatePayment candidatePaymentObj = null;
                string admUnitProgramStr = string.Empty;
                using (var db = new CandidateDataManager())
                {
                    cp = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == candidateId).FirstOrDefault();
                    candidatePaymentObj = db.GetCandidatePaymentByID_AD(cp.ID);
                    if (candidatePaymentObj != null)
                    {
                        List<DAL.CandidateFormSl> candFormSerialList = candidatePaymentObj.CandidateFormSls.ToList();
                        List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
                        List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();


                        bool showBkashButton = true;
                        if (candFormSerialList.Count() > 0)
                        {
                            foreach (var item in candFormSerialList)
                            {
                                DAL.AdmissionSetup admSetupItem = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                                if (admSetupItem != null)
                                {
                                    admSetupList.Add(admSetupItem);
                                    if (admSetupItem.Attribute1 == null)
                                    {
                                        showBkashButton = false;
                                    }
                                }
                            }
                        }


                        if (admSetupList.Count() > 0)
                        {
                            foreach (var item in admSetupList)
                            {
                                List<DAL.AdmissionUnit> admUnitListForEachAdmSetup = db.AdmissionDB.AdmissionUnits
                                    .Where(a => a.ID == item.AdmissionUnitID).ToList();
                                if (admUnitListForEachAdmSetup.Count() > 0)
                                {
                                    admUnitList.AddRange(admUnitListForEachAdmSetup);
                                }
                            }
                        }


                        if (admUnitList.Count() > 0)
                        {
                            foreach (var item in admUnitList)
                            {
                                admUnitProgramStr += item.UnitName + "<br />";
                            }
                        }


                    }

                    #region Send EMAIL

                    try
                    {
                        string mailBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + ", <br/><br/>" +
                            "Thank you for applying at Bangladesh University of Professionals (BUP). Please proceed for payment or you can pay later following the link : " +  //"Thank you for applying at Bangladesh University of Professionals (BUP). Please check the information below about your payment: " +
                            "<br/><br/>" +
                            "<b>Link : </b><a href='https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + paramValue + "'> https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=value </a>" +
                            "<strong style='color: Tomato;'>Payment ID: " + candidatePaymentObj.PaymentId.ToString() + "</strong>" +
                            "<br/><br/>" +
                            "Applied For :" + "<br/>" + admUnitProgramStr + ". <br/>" +
                            "<br/>" +
                            "</p>" +
                            "<p style='color: green;'>Important : You must login, fill up and submit the Application Form after successful Payment.</p>" +
                            "<p>" +
                            "<br/>" +
                            "Regards," +
                            "<br/>" +
                            "Bangladesh University of Professionals (BUP)" +
                            "</p>";
                        //"Regards, <br/>" ICT Centre,
                        //"Admin" +
                        //"<br/>" +
                        string fromAddress = "no-reply-2@bup.edu.bd";
                        string senderName = "BUP Admission";
                        string subject = "Payment ID for Your Application";

                        //bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);
                        bool isSentEmail = EmailUtility.SendMail(candidatePaymentObj.BasicInfo.Email, fromAddress, senderName, subject, mailBody);

                        if (isSentEmail == true)
                        {
                            DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                            eLog.MessageBody = mailBody;
                            eLog.MessageSubject = subject;
                            eLog.Page = "CertificatePurchaseForm.aspx";
                            eLog.SentBy = "System";
                            eLog.StudentId = candidatePaymentObj.CandidateID;
                            eLog.ToAddress = candidatePaymentObj.BasicInfo.Email;
                            eLog.ToName = candidatePaymentObj.BasicInfo.FirstName;
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
                            eLog.Page = "CertificatePurchaseForm.aspx";
                            eLog.SentBy = "System";
                            eLog.StudentId = candidatePaymentObj.CandidateID;
                            eLog.ToAddress = candidatePaymentObj.BasicInfo.Email;
                            eLog.ToName = candidatePaymentObj.BasicInfo.FirstName;
                            eLog.DateSent = DateTime.Now;
                            eLog.FromAddress = fromAddress;
                            eLog.FromName = senderName;
                            eLog.Attribute1 = "Failed";

                            LogWriter.EmailLog(eLog);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion

                    #region Send SMS
                    try
                    {
                        GetSendingInfo(candidatePaymentObj.CandidateID, paramValue, candidatePaymentObj);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {

            }
        }
        private void GetSendingInfo(long? candidateId, string value, DAL.CandidatePayment candidatePaymentObj)
        {
            if (candidateId != null)
            {
                if (candidateId > 0)
                {
                    DAL.BasicInfo candidate = null;
                    DAL.CandidateUser candidateUser = null;

                    string candidateUsername = null;
                    string candidatePassword = null;
                    string candidateSmsPhone = null;
                    string candidateEmail = null;

                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                    }

                    if (candidate != null)
                    {
                        candidateEmail = candidate.Email;
                        candidateSmsPhone = candidate.SMSPhone;
                        using (var db = new CandidateDataManager())
                        {
                            candidateUser = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                        }
                    }

                    if (candidateUser != null)
                    {
                        candidateUsername = candidateUser.UsernameLoginId;
                        candidatePassword = candidateUser.Password;
                    }

                    if (!string.IsNullOrEmpty(candidateUsername) && !string.IsNullOrEmpty(candidatePassword) &&
                        !string.IsNullOrEmpty(candidateSmsPhone) && !string.IsNullOrEmpty(candidateEmail))
                    {
                        SendSms(candidateSmsPhone, candidate.ID, value, candidatePaymentObj);
                        //SendEmail(candidateEmail, candidateUsername, candidatePassword, candidate.ID);
                    }
                }
            }
        }
        private void SendSms(string smsPhone, long candidateId, string value, DAL.CandidatePayment candidatePaymentObj)
        {
            if (!string.IsNullOrEmpty(smsPhone) && smsPhone.Count() == 14 && smsPhone.Contains("+"))
            {
                //string messageBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + "," + "\n" +
                //                    " Payment ID : " + candidatePaymentObj.PaymentId.ToString() + "\n" +
                //                    " and Payment Link : https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value + "\n" +
                //                    " BUP";

                string messageBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + "," + 
                                    " Payment ID : " + candidatePaymentObj.PaymentId.ToString() + 
                                    " and Payment Link : https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value + 
                                    " BUP";

                string stringData = SMSUtility.Send(smsPhone, messageBody);

                string statusT = JObject.Parse(stringData)["statusCode"].ToString();

                if (statusT != "200") //if sms sending fails
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending failed in CertificatePurchaseForm.aspx";
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
                    smsLog.Attribute1 = "Sms sending successful CertificatePurchaseForm.aspx";
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


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
            {
                LoadCaptcha();
                captchaMsg.Visible = true;
                return;
            }

            int admissionSetupIDLong = -1;

            admissionSetupIDLong = Convert.ToInt32(Request.QueryString["asi"]);

            using (var db = new OfficeDataManager())
            {
                DAL.CertificateAdmissionSetup admissionSetup = db.AdmissionDB.CertificateAdmissionSetups.Find(admissionSetupIDLong);

                if (admissionSetup.ID > 0 && admissionSetup.IsActive == true)
                {
                    DAL.CertificateAdmissionUnit admissionUnit = db.AdmissionDB.CertificateAdmissionUnits.Find(admissionSetup.AdmissionUnitID);
                    if (admissionUnit.ID > 0 && admissionUnit.IsActive == true)
                    {
                        PurchaseApplicationForm();
                    }
                    else
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
                    }
                }
                else
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
                }

            }

        }



    }
}