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
using System.Net.Mail;
using Newtonsoft.Json.Linq;

namespace Admission.Admission.Office
{
    public partial class ResendSms : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
                if (uId == 98)
                    divSendSms.Visible = true;
                else
                    divSendSms.Visible = false;

                //LoadListView();
                lblMessage.Text = "";
                lblCount.Text = "";
            }

        }


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMobile.Text.Trim()) && string.IsNullOrEmpty(txtPaymentId.Text.Trim()))
            {
                lblMessage.Text = "Please provide either a mobile number or payment ID";
                lblMessage.ForeColor = Color.Crimson;
                lblMessage.Font.Bold = true;
                return;
            }
            else if (string.IsNullOrEmpty(txtMobile.Text.Trim()) && !string.IsNullOrEmpty(txtPaymentId.Text.Trim())) //if searching using paymentId
            {
                long? paymentId = Int64.Parse(txtPaymentId.Text.Trim());

                List<DAL.SPGetCandidateSMSEmailCntByPmtIdMob_Result> result = null;
                using (var db = new OfficeDataManager())
                {
                    result = db.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMob(paymentId, null).ToList();
                }

                if (result.Count() > 0)
                {
                    lvSmsList.DataSource = result;
                    lblCount.Text = result.Count().ToString();
                }
                else
                {
                    lvSmsList.DataSource = null;
                    lblCount.Text = "0";
                }
                lvSmsList.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtMobile.Text.Trim()) && string.IsNullOrEmpty(txtPaymentId.Text.Trim())) //if seaching using mobile
            {
                string mobile = txtMobile.Text.Trim();

                List<DAL.SPGetCandidateSMSEmailCntByPmtIdMob_Result> result = null;
                using (var db = new OfficeDataManager())
                {
                    result = db.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMob(null, mobile).ToList();
                }

                if (result.Count() > 0)
                {
                    lvSmsList.DataSource = result;
                    lblCount.Text = result.Count().ToString();
                }
                else
                {
                    lvSmsList.DataSource = null;
                    lblCount.Text = "0";
                }
                lvSmsList.DataBind();
            }
            else
            {
                string mobile = txtMobile.Text.Trim();
                long? paymentId = Int64.Parse(txtPaymentId.Text.Trim());

                List<DAL.SPGetCandidateSMSEmailCntByPmtIdMob_Result> result = null;
                using (var db = new OfficeDataManager())
                {
                    result = db.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMob(paymentId, mobile).ToList();
                }

                if (result.Count() > 0)
                {
                    lvSmsList.DataSource = result;
                    lblCount.Text = result.Count().ToString();
                }
                else
                {
                    lvSmsList.DataSource = null;
                    lblCount.Text = "0";
                }
                lvSmsList.DataBind();
            }
        }

        protected void lvSmsList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetCandidateSMSEmailCntByPmtIdMob_Result result = (DAL.SPGetCandidateSMSEmailCntByPmtIdMob_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblEmail = (Label)currentItem.FindControl("lblEmail");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
                Label lblMobile = (Label)currentItem.FindControl("lblMobile");
                Label lblPaid = (Label)currentItem.FindControl("lblPaid");
                Label lblNoSmsSent = (Label)currentItem.FindControl("lblNoSmsSent");
                Label lblNoEmailSent = (Label)currentItem.FindControl("lblNoEmailSent");

                LinkButton lnkSendSms = (LinkButton)currentItem.FindControl("lnkSendSms");
                LinkButton lnkSendEmail = (LinkButton)currentItem.FindControl("lnkSendEmail");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                lblName.Text = result.FirstName;
                lblEmail.Text = result.Email;
                lblPaymentId.Text = result.PaymentId.ToString();
                lblMobile.Text = result.SMSPhone;
                lblNoSmsSent.Text = result.Count_SMS.ToString();
                lblNoEmailSent.Text = result.Count_Email.ToString();

                if (result.IsPaid == true)
                {
                    lblPaid.Text = "YES";
                    lblPaid.ForeColor = Color.Green;
                    lblPaid.Font.Bold = true;
                }
                else
                {
                    lblPaid.Text = "NO";
                    lblPaid.ForeColor = Color.Crimson;
                }

                lnkSendSms.CommandName = "SendSms";
                lnkSendSms.CommandArgument = result.cID.ToString();

                lnkSendEmail.CommandName = "SendEmail";
                lnkSendEmail.CommandArgument = result.cID.ToString();
            }
        }

        protected void lvSmsList_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void lvSmsList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "SendSms")
            {
                long candidateId = Int64.Parse(e.CommandArgument.ToString());
                if (candidateId > 0)
                {
                    DAL.BasicInfo candidate = null;
                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                    }

                    if (candidate != null)
                    {
                        DAL.CandidateUser cu = null;
                        using (var db = new CandidateDataManager())
                        {
                            cu = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                        }

                        if (cu != null)
                        {
                            string msgBody = "BUP Admission. Login to https://admission.bup.edu.bd. Username: " + cu.UsernameLoginId.ToString() + " ; Password: " + cu.Password.ToString();

                            //string msgBody = "BUP Admission. Username: " + cu.UsernameLoginId + "  Password: " + cu.Password;
                            string smsRespose = SMSUtility.Send(candidate.SMSPhone, msgBody);

                            string statusT = JObject.Parse(smsRespose)["statusCode"].ToString();

                            if (statusT != "200") //if sms sending fails
                            {
                                DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                                smsLog.AcaCalId = null;
                                smsLog.Attribute1 = "Sms sending failed in ResendSms.aspx";
                                smsLog.Attribute2 = null;
                                smsLog.Attribute3 = null;
                                smsLog.CreatedBy = candidateId;
                                smsLog.CreatedDate = DateTime.Now;
                                smsLog.CurrentSMSReferenceNo = smsRespose;
                                smsLog.Message = msgBody;
                                smsLog.StudentId = candidateId;
                                smsLog.PhoneNo = candidate.SMSPhone;
                                smsLog.SenderUserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                                smsLog.SentReferenceId = null;
                                smsLog.SentSMSId = null;
                                smsLog.SmsSendDate = DateTime.Now;
                                smsLog.SmsType = -1;

                                //LogWriter.SmsLog(smsLog);

                                lblMessageLv.Text = "SMS sending failed.";
                                lblMessageLv.ForeColor = Color.Crimson;
                                //btnLoad_Click(null, null);
                            }
                            else //if sms sending passed
                            {
                                DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                                smsLog.AcaCalId = null;
                                smsLog.Attribute1 = "Sms sending successful ResendSms.aspx";
                                smsLog.Attribute2 = null;
                                smsLog.Attribute3 = null;
                                smsLog.CreatedBy = candidateId;
                                smsLog.CreatedDate = DateTime.Now;
                                smsLog.CurrentSMSReferenceNo = smsRespose;
                                smsLog.Message = msgBody;
                                smsLog.StudentId = candidateId;
                                smsLog.PhoneNo = candidate.SMSPhone;
                                smsLog.SenderUserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                                smsLog.SentReferenceId = null;
                                smsLog.SentSMSId = null;
                                smsLog.SmsSendDate = DateTime.Now;
                                smsLog.SmsType = -1;

                                LogWriter.SmsLog(smsLog);

                                lblMessageLv.Text = "SMS sent.";
                                lblMessageLv.ForeColor = Color.Green;
                                //btnLoad_Click(null, null);
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "SendEmail")
            {
                long candidateId = Int64.Parse(e.CommandArgument.ToString());
                if (candidateId > 0)
                {
                    DAL.BasicInfo candidate = null;
                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                    }

                    if (candidate != null)
                    {
                        DAL.CandidateUser cu = null;
                        using (var db = new CandidateDataManager())
                        {
                            cu = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                        }

                        if (cu != null)
                        {
                            //"<p>Please check your username and password given below: </p>" +

                            string mailbody = "<p>Dear " + candidate.FirstName + ",</p>" +
                                "<p>Login to https://admission.bup.edu.bd/Admission/Login .</p>" + "<br/>" +

                                "<p><strong>Username:</strong> " + cu.UsernameLoginId + "<br/>" +
                                "<strong>Password:</strong> " + cu.Password + "<br/></p>" +
                                "<br/> <p><strong>Bangladesh University of Professionals</strong></p>";
                            bool isEmailSent = EmailUtility.SendMail(candidate.Email, "no-reply-2@bup.edu.bd", "BUP Admission", "Resend Username And Password", mailbody);
                            //bool isEmailSent = SendMail(candidate.Email, "no-reply-2@bup.edu.bd", "BUP Admission", "Resend Username And Password",  mailbody);

                            if (isEmailSent == true)
                            {
                                DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                eLog.MessageBody = mailbody;
                                eLog.MessageSubject = "Resend Username and Password";
                                eLog.Page = "ResendSms.aspx";
                                eLog.SentBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString();
                                eLog.StudentId = candidateId;
                                eLog.ToAddress = candidate.Email;
                                eLog.ToName = candidate.FirstName;
                                eLog.DateSent = DateTime.Now;
                                eLog.FromAddress = "no-reply-2@bup.edu.bd";
                                eLog.FromName = "BUP Admission";
                                eLog.Attribute1 = "Success";

                                LogWriter.EmailLog(eLog);

                                lblMessageLv.Text = "Email sent.";
                                lblMessageLv.ForeColor = Color.Green;
                                //btnLoad_Click(null, null);
                            }
                            else if (isEmailSent == false)
                            {
                                DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                eLog.MessageBody = mailbody;
                                eLog.MessageSubject = "Resend Username and Password";
                                eLog.Page = "ResendSms.aspx";
                                eLog.SentBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString();
                                eLog.StudentId = candidateId;
                                eLog.ToAddress = candidate.Email;
                                eLog.ToName = candidate.FirstName;
                                eLog.DateSent = DateTime.Now;
                                eLog.FromAddress = "no-reply-2@bup.edu.bd";
                                eLog.FromName = "BUP Admission";
                                eLog.Attribute1 = "Failed";

                                LogWriter.EmailLog(eLog);

                                lblMessageLv.Text = "Email sending failed.";
                                lblMessageLv.ForeColor = Color.Crimson;
                                //btnLoad_Click(null, null);
                            }
                        }
                    }
                }
            }
        }

        protected void btnSendSMS_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dblist = new CandidateDataManager())
                {
                    var CandidateList = dblist.AdmissionDB.GetTempTableCandidateList().ToList();

                    if (CandidateList != null && CandidateList.Any())
                    {
                        foreach (var item in CandidateList)
                        {
                            try
                            {
                                long candidateId = Convert.ToInt64(item.CandidateId);
                                long paymentId = Convert.ToInt64(item.PaymentId);

                                var result = dblist.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMob(paymentId, null).FirstOrDefault();

                                if (result.Count_SMS == 0)
                                {
                                    if (candidateId > 0)
                                    {
                                        DAL.BasicInfo candidate = null;
                                        using (var db = new CandidateDataManager())
                                        {
                                            candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                                        }

                                        if (candidate != null)
                                        {
                                            DAL.CandidateUser cu = null;
                                            using (var db = new CandidateDataManager())
                                            {
                                                cu = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                                            }

                                            if (cu != null)
                                            {
                                                string msgBody = "BUP Admission. Login to https://admission.bup.edu.bd. Username: " + cu.UsernameLoginId.ToString() + " ; Password: " + cu.Password.ToString();

                                                //string msgBody = "BUP Admission. Username: " + cu.UsernameLoginId + "  Password: " + cu.Password;
                                                string smsRespose = SMSUtility.Send(candidate.SMSPhone, msgBody);

                                                string statusT = JObject.Parse(smsRespose)["statusCode"].ToString();

                                                if (statusT != "200") //if sms sending fails
                                                {
                                                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                                                    smsLog.AcaCalId = null;
                                                    smsLog.Attribute1 = "Sms sending failed in ResendSms BM.aspx";
                                                    smsLog.Attribute2 = null;
                                                    smsLog.Attribute3 = null;
                                                    smsLog.CreatedBy = candidateId;
                                                    smsLog.CreatedDate = DateTime.Now;
                                                    smsLog.CurrentSMSReferenceNo = smsRespose;
                                                    smsLog.Message = msgBody;
                                                    smsLog.StudentId = candidateId;
                                                    smsLog.PhoneNo = candidate.SMSPhone;
                                                    smsLog.SenderUserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                                                    smsLog.SentReferenceId = null;
                                                    smsLog.SentSMSId = null;
                                                    smsLog.SmsSendDate = DateTime.Now;
                                                    smsLog.SmsType = -1;

                                                    //LogWriter.SmsLog(smsLog);

                                                    lblMessageLv.Text = "SMS sending failed.";
                                                    lblMessageLv.ForeColor = Color.Crimson;
                                                    //btnLoad_Click(null, null);
                                                }
                                                else //if sms sending passed
                                                {
                                                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                                                    smsLog.AcaCalId = null;
                                                    smsLog.Attribute1 = "Sms sending successful ResendSms BM.aspx";
                                                    smsLog.Attribute2 = null;
                                                    smsLog.Attribute3 = null;
                                                    smsLog.CreatedBy = candidateId;
                                                    smsLog.CreatedDate = DateTime.Now;
                                                    smsLog.CurrentSMSReferenceNo = smsRespose;
                                                    smsLog.Message = msgBody;
                                                    smsLog.StudentId = candidateId;
                                                    smsLog.PhoneNo = candidate.SMSPhone;
                                                    smsLog.SenderUserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                                                    smsLog.SentReferenceId = null;
                                                    smsLog.SentSMSId = null;
                                                    smsLog.SmsSendDate = DateTime.Now;
                                                    smsLog.SmsType = -1;

                                                    LogWriter.SmsLog(smsLog);

                                                    lblMessageLv.Text = "SMS sent.";
                                                    lblMessageLv.ForeColor = Color.Green;
                                                    //btnLoad_Click(null, null);
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
                    }

                   

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dblist = new CandidateDataManager())
                {
                    var CandidateList = dblist.AdmissionDB.GetTempTableCandidateList().ToList();

                    if (CandidateList != null && CandidateList.Any())
                    {
                        foreach (var item in CandidateList)
                        {
                            try
                            {
                                long candidateId = Convert.ToInt64(item.CandidateId);
                                long paymentId = Convert.ToInt64(item.PaymentId);

                                var result = dblist.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMob(paymentId, null).FirstOrDefault();

                                if (result.Count_Email == 0)
                                {
                                    if (candidateId > 0)
                                    {
                                        DAL.BasicInfo candidate = null;
                                        using (var db = new CandidateDataManager())
                                        {
                                            candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                                        }

                                        if (candidate != null)
                                        {
                                            DAL.CandidateUser cu = null;
                                            using (var db = new CandidateDataManager())
                                            {
                                                cu = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                                            }

                                            if (cu != null)
                                            {
                                                //"<p>Please check your username and password given below: </p>" +

                                                string mailbody = "<p>Dear " + candidate.FirstName + ",</p>" +
                                                    "<p>Login to https://admission.bup.edu.bd/Admission/Login .</p>" + "<br/>" +

                                                    "<p><strong>Username:</strong> " + cu.UsernameLoginId + "<br/>" +
                                                    "<strong>Password:</strong> " + cu.Password + "<br/></p>" +
                                                    "<br/> <p><strong>Bangladesh University of Professionals</strong></p>";
                                                bool isEmailSent = EmailUtility.SendMail(candidate.Email, "no-reply-2@bup.edu.bd", "BUP Admission", "Resend Username And Password", mailbody);
                                                //bool isEmailSent = SendMail(candidate.Email, "no-reply-2@bup.edu.bd", "BUP Admission", "Resend Username And Password",  mailbody);

                                                if (isEmailSent == true)
                                                {
                                                    DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                                    eLog.MessageBody = mailbody;
                                                    eLog.MessageSubject = "Resend Username and Password";
                                                    eLog.Page = "ResendSms BEM.aspx";
                                                    eLog.SentBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString();
                                                    eLog.StudentId = candidateId;
                                                    eLog.ToAddress = candidate.Email;
                                                    eLog.ToName = candidate.FirstName;
                                                    eLog.DateSent = DateTime.Now;
                                                    eLog.FromAddress = "no-reply-2@bup.edu.bd";
                                                    eLog.FromName = "BUP Admission";
                                                    eLog.Attribute1 = "Success";

                                                    LogWriter.EmailLog(eLog);

                                                    lblMessageLv.Text = "Email sent.";
                                                    lblMessageLv.ForeColor = Color.Green;
                                                    //btnLoad_Click(null, null);
                                                }
                                                else if (isEmailSent == false)
                                                {
                                                    DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                                    eLog.MessageBody = mailbody;
                                                    eLog.MessageSubject = "Resend Username and Password";
                                                    eLog.Page = "ResendSms BEM.aspx";
                                                    eLog.SentBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString();
                                                    eLog.StudentId = candidateId;
                                                    eLog.ToAddress = candidate.Email;
                                                    eLog.ToName = candidate.FirstName;
                                                    eLog.DateSent = DateTime.Now;
                                                    eLog.FromAddress = "no-reply-2@bup.edu.bd";
                                                    eLog.FromName = "BUP Admission";
                                                    eLog.Attribute1 = "Failed";

                                                    LogWriter.EmailLog(eLog);

                                                    lblMessageLv.Text = "Email sending failed.";
                                                    lblMessageLv.ForeColor = Color.Crimson;
                                                    //btnLoad_Click(null, null);
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
                    }



                }

            }
            catch (Exception ex)
            {
            }
        }



        //public static bool SendMail(string toAddress, string fromAddress, string name, string subject, string body)
        //{
        //    MailMessage msg = new MailMessage();
        //    msg.To.Add(new MailAddress(toAddress));
        //    msg.From = new MailAddress(fromAddress, "BUP Admission");
        //    msg.Subject = subject;
        //    msg.Body = body;
        //    msg.IsBodyHtml = true;

        //    //use app password instead of account password.

        //    SmtpClient client = new SmtpClient();
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = new System.Net.NetworkCredential("no-reply-2@bup.edu.bd", "B@up#-2018"); //ADM@2017
        //    client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
        //    client.Host = "smtp.office365.com";
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.EnableSsl = true;

        //    //SmtpClient client = new SmtpClient();
        //    //client.UseDefaultCredentials = false;
        //    //client.Credentials = new System.Net.NetworkCredential("","");
        //    //client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
        //    //client.Host = "smtp.gmail.com";
        //    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //client.EnableSsl = true;
        //    try
        //    {
        //        client.Send(msg);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}



    }
}