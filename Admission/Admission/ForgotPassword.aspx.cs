using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        string SessionLoginCaptcha = "SessionLoginCaptcha";

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO: implement turn off during maintenance
            if (!IsPostBack)
            {
                LoadCaptcha();
            }
        }

        private void ClearFields()
        {
            txtPaymentID.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCaptcha.Text = string.Empty;
            txtDob.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }

        public void LoadCaptcha()
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

        private void ShowMessage(string text, bool showMsgPanel, string style)
        {
            lblMessage.Text = text;
            messagePanel.CssClass = style;
            messagePanel.Visible = showMsgPanel;
        }

        protected void btnReLoadCaptcha_Click(object sender, ImageClickEventArgs e)
        {
            LoadCaptcha();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
            {
                LoadCaptcha();
                captchaMsg.Visible = true;
                return;
            }

            long paymentId = -1;
            string email = string.Empty;
            string phone = string.Empty;
            DateTime? dob = null;

            email = txtEmail.Text.Trim();


            bool IsForeign = false;
            string Name = "";
            string UserId = "", Password = "";
            long CId = 0;

            #region For Internation Student

            if (email != "")
            {
                using (var db = new CandidateDataManager())
                {

                    var UserObj = db.AdmissionDB.BasicInfoes.Where(x => x.Email == email).FirstOrDefault();

                    if (UserObj != null)
                    {
                        var UserPassword = db.AdmissionDB.CandidateUsers.Where(x => x.ID == UserObj.CandidateUserID).FirstOrDefault();

                        if (UserPassword != null)
                        {

                            var ForeignCheck = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == UserObj.ID).FirstOrDefault();

                            if (ForeignCheck != null && ForeignCheck.IsForeignStudent == true)
                            {
                                IsForeign = true;
                                Name = UserObj.FirstName + " " + UserObj.MiddleName + " " + UserObj.LastName;
                                UserId = UserPassword.UsernameLoginId;
                                Password = UserPassword.Password;
                                CId = UserObj.ID;
                            }
                        }


                    }
                }
            }

            #endregion

            if (IsForeign)
            {
                string mailbody = "<p>Dear " + Name + ",</p>" +
                                "<p>Please check your username and password given below: </p>" +
                                "<p><strong>Username:</strong> " + UserId + "<br/>" +
                                "<strong>Password:</strong> " + Password + "<br/></p>" +
                                "<br/> <p><strong>Bangladesh University of Professionals</strong></p>"
                                ;
                bool isEmailSent = EmailUtility.SendMail(email, "no-reply-2@bup.edu.bd", "BUP Admission", "Forgot Username and Password", mailbody);

                if (isEmailSent == true)
                {
                    DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    eLog.MessageBody = mailbody;
                    eLog.MessageSubject = "Username and Password";
                    eLog.Page = "ForgotPassword.aspx";
                    eLog.SentBy = CId.ToString();
                    eLog.StudentId = CId;
                    eLog.ToAddress = email;
                    eLog.ToName = Name;
                    eLog.DateSent = DateTime.Now;
                    eLog.FromAddress = "no-reply-2@bup.edu.bd";
                    eLog.FromName = "BUP Admission";
                    eLog.Attribute1 = "Success";

                    LogWriter.EmailLog(eLog);

                    ShowMessage("Successful. Please check your inbox.", true, "alert alert-success");
                }
                else if (isEmailSent == false)
                {
                    DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    eLog.MessageBody = mailbody;
                    eLog.MessageSubject = "Username and Password";
                    eLog.Page = "ForgotPassword.aspx";
                    eLog.SentBy = CId.ToString();
                    eLog.StudentId = CId;
                    eLog.ToAddress = email;
                    eLog.ToName = Name;
                    eLog.DateSent = DateTime.Now;
                    eLog.FromAddress = "no-reply-2@bup.edu.bd";
                    eLog.FromName = "BUP Admission";
                    eLog.Attribute1 = "Failed";

                    LogWriter.EmailLog(eLog);

                    ShowMessage("Error occurred while sending email.", true, "alert alert-danger");
                }
                ClearFields();
            }
            else
            {
               

                if (!string.IsNullOrEmpty(txtPaymentID.Text) && !string.IsNullOrEmpty(txtDob.Text) && !string.IsNullOrEmpty(txtPhone.Text))
                {

                    paymentId = Convert.ToInt64(txtPaymentID.Text.Trim());
                    phone = txtPhone.Text.Trim();
                    dob = DateTime.ParseExact(txtDob.Text, "dd/MM/yyyy", null);

                    #region For Local Student 

                    DAL.SPRetieveUsernamePasswordByPaymentID_Result obj = null;

                    if (paymentId > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            obj = db.AdmissionDB.SPRetieveUsernamePasswordByPaymentID(paymentId, true)
                                .FirstOrDefault();
                        }
                    }

                    if (obj != null) //if object with the specified payment id is found
                    {
                        if (obj.CP_PaymentId.Equals(paymentId) && obj.CandidateEmail.Equals(email)
                            && obj.CandidateSmsPhone.Equals(phone) && obj.CandidateDateOfBirth.Day.Equals(dob.Value.Day)
                            && obj.CandidateDateOfBirth.Month.Equals(dob.Value.Month)
                            && obj.CandidateDateOfBirth.Year.Equals(dob.Value.Year)) // paid candidate exist, and details provided match.
                        {
                            string mailbody = "<p>Dear " + obj.CandidateName + ",</p>" +
                                "<p>Please check your username and password given below: </p>" +
                                "<p><strong>Username:</strong> " + obj.UsernameLoginId + "<br/>" +
                                "<strong>Password:</strong> " + obj.Password + "<br/></p>" +
                                "<br/> <p><strong>Bangladesh University of Professionals</strong></p>"
                                ;
                            bool isEmailSent = EmailUtility.SendMail(obj.CandidateEmail, "no-reply-2@bup.edu.bd", "BUP Admission", "Forgot Username and Password", mailbody);

                            if (isEmailSent == true)
                            {
                                DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                eLog.MessageBody = mailbody;
                                eLog.MessageSubject = "Username and Password";
                                eLog.Page = "ForgotPassword.aspx";
                                eLog.SentBy = obj.CandidateID.ToString();
                                eLog.StudentId = obj.CandidateID;
                                eLog.ToAddress = obj.CandidateEmail;
                                eLog.ToName = obj.CandidateName;
                                eLog.DateSent = DateTime.Now;
                                eLog.FromAddress = "no-reply-2@bup.edu.bd";
                                eLog.FromName = "BUP Admission";
                                eLog.Attribute1 = "Success";

                                LogWriter.EmailLog(eLog);

                                ShowMessage("Successful. Please check your inbox.", true, "alert alert-success");
                            }
                            else if (isEmailSent == false)
                            {
                                DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                                eLog.MessageBody = mailbody;
                                eLog.MessageSubject = "Username and Password";
                                eLog.Page = "ForgotPassword.aspx";
                                eLog.SentBy = obj.CandidateID.ToString();
                                eLog.StudentId = obj.CandidateID;
                                eLog.ToAddress = obj.CandidateEmail;
                                eLog.ToName = obj.CandidateName;
                                eLog.DateSent = DateTime.Now;
                                eLog.FromAddress = "no-reply-2@bup.edu.bd";
                                eLog.FromName = "BUP Admission";
                                eLog.Attribute1 = "Failed";

                                LogWriter.EmailLog(eLog);

                                ShowMessage("Error occurred while sending email.", true, "alert alert-danger");
                            }
                            ClearFields();
                        }
                        else // paid candidate exist, but details provided does not match match.
                        {
                            ShowMessage("No matching candidate found. Please make sure that your Payment ID, Email, SMS Phone and Date of Birth is correct.", true, "alert alert-danger");
                            ClearFields();
                        }
                    }
                    else //paid candidate does not exist.
                    {
                        ShowMessage("Payment ID: " + txtPaymentID.Text + "not found", true, "alert alert-danger");
                        ClearFields();
                    }

                    #endregion
                }
                else
                {
                    ShowMessage("Please fill up all the fields", true, "alert alert-danger");

                }

            }

        }
    }
}