using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class Login : System.Web.UI.Page
    {
        string SessionLoginCaptcha = "SessionLoginCaptcha";

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO: implement turn off during maintenance
            if (!IsPostBack)
            {

                string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

                if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
                {
                    captchaReq.Enabled = true;
                }
                else
                {
                    captchaReq.Enabled = false;
                }

                LoadCaptcha();
            }
        }

        private void ClearFields()
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtCaptcha.Text = string.Empty;
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

        protected void btnReLoadCaptcha_Click(object sender, ImageClickEventArgs e)
        {
            LoadCaptcha();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

            if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
            {
                if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
                {
                    LoadCaptcha();
                    captchaMsg.Visible = true;
                    return;
                }
            }

            string uId = string.Empty;
            string pWd = string.Empty;

            uId = txtUserName.Text.Trim(); // this is username
            pWd = txtPassword.Text.Trim();

            DAL.CandidateUser candidateUser = new DAL.CandidateUser();
            DAL.SystemUser systemUser = new DAL.SystemUser();
            DAL.CertificateCandidateUser certificateCandidateUser = new DAL.CertificateCandidateUser();
            DAL.PostgraduateDiplomaCandidateUser postgraduateDiplomaCandidateUser = new DAL.PostgraduateDiplomaCandidateUser();

            DAL.AdditionalInfo additionalInfo = new DAL.AdditionalInfo();
            try
            {
                using (var db = new CandidateDataManager())
                {
                    candidateUser = db.GetCandidateUserByUsername_ND(uId);
                }
                using (var db = new OfficeDataManager())
                {
                    systemUser = db.GetSystemUserByUsername_AD(uId);
                }
                using (var db = new CandidateDataManager())
                {
                    certificateCandidateUser = db.GetCertificateCandidateUserByUsername_ND(uId);
                }
                using (var db = new CandidateDataManager())
                {
                    postgraduateDiplomaCandidateUser = db.GetPostgraduateDiplomaCandidateUserByUsername_ND(uId);
                }

                if (candidateUser != null && candidateUser.ID > 0)
                {
                    if (candidateUser.Password.Equals(pWd) && candidateUser.UsernameLoginId.Equals(uId))
                    {

                        if (candidateUser.ValidTill > DateTime.Now) //if user is within valid date.
                        {
                            #region A-LOG
                            DAL_Log.AccessLog aLog = new DAL_Log.AccessLog();
                            aLog.DateTime = DateTime.Now;
                            aLog.LoginID = "CU-ID: " + candidateUser.ID;
                            aLog.loginStatus = "Successful";
                            aLog.Message = "Candidate attempted login and is successful.";
                            aLog.IpAddress = Request.UserHostAddress + "/" + Request.UserHostName;
                            aLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_UserG);
                            LogWriter.AccessLogWriter(aLog);
                            #endregion

                            SessionSGD.SaveObjToSession<long>(candidateUser.ID, SessionName.Common_UserId); //this is candidate user primary key
                            SessionSGD.SaveObjToSession<string>(candidateUser.UsernameLoginId, SessionName.Common_LoginID); //this is the username
                            SessionSGD.SaveObjToSession<string>("Candidate", SessionName.Common_RoleName);
                            if (!string.IsNullOrEmpty(SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage)))
                            {
                                string url = SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage);
                                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                                Response.Redirect(url, false);
                            }
                            else
                            {
                                DAL.BasicInfo basicInfo = new DAL.BasicInfo();
                                using (var db = new CandidateDataManager())
                                {
                                    basicInfo = db.GetCandidateBasicInfoByUserID_ND(candidateUser.ID);
                                }
                                using (var db = new CandidateDataManager())
                                {
                                    int cId = 0;
                                    if (basicInfo != null)
                                        cId = Convert.ToInt32(basicInfo.ID);

                                    additionalInfo = db.GetAdditionalInfoByCandidateID_ND(cId);
                                }
                                if (additionalInfo != null && additionalInfo.IsForeignStudent != null && Convert.ToBoolean(additionalInfo.IsForeignStudent))
                                {
                                    Response.Redirect("~/Admission/Candidate/ForeignCandidateHome.aspx", false);
                                }
                                else
                                    Response.Redirect("~/Admission/Candidate/CandidateHome.aspx", false);
                            }
                        }
                        else if (candidateUser.ValidTill < DateTime.Now) //if user is outside the valid date.
                        {
                            #region A-LOG
                            DAL_Log.AccessLog aLogExp = new DAL_Log.AccessLog();
                            aLogExp.DateTime = DateTime.Now;
                            aLogExp.LoginID = "CU-ID: " + candidateUser.ID;
                            aLogExp.loginStatus = "Unsuccessful/User credentials expired.";
                            aLogExp.Message = "Candidate attempted login.";
                            aLogExp.IpAddress = Request.UserHostAddress + "/" + Request.UserHostName;
                            aLogExp.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_UserG);
                            LogWriter.AccessLogWriter(aLogExp);
                            #endregion

                            lblMessage.Text = "<strong>Login failed...</strong>Your username and password has expired.";
                            messagePanel.Visible = true;
                            ClearFields();
                        }
                    }
                    else
                    {
                        lblMessage.Text = "<strong>Login failed...</strong>Incorrect Username or Password.";
                        messagePanel.Visible = true;
                        ClearFields();
                    }
                }
                else if (systemUser != null && systemUser.ID > 0)
                {
                    if (systemUser.Password.Equals(pWd) && systemUser.Username.Equals(uId))
                    {

                        DAL.SystemUserInRole sysUserRole = new DAL.SystemUserInRole();
                        using (var db = new OfficeDataManager())
                        {
                            sysUserRole = db.GetSysUserInRoleByUserID_AD(systemUser.ID);
                        }
                        if (sysUserRole != null && sysUserRole.ID > 0)
                        {
                            SessionSGD.SaveObjToSession<long>(systemUser.ID, SessionName.Common_UserId); //this is system user primary key
                            SessionSGD.SaveObjToSession<string>(systemUser.Username, SessionName.Common_LoginID); //this is username
                            SessionSGD.SaveObjToSession<string>(sysUserRole.Role.RoleName, SessionName.Common_RoleName);
                            SessionSGD.SaveObjToSession<string>(systemUser.GUID.ToString(), SessionName.Common_UserG);

                            #region LOG
                            DAL_Log.AccessLog aLog = new DAL_Log.AccessLog();
                            aLog.DateTime = DateTime.Now;
                            aLog.LoginID = " SU-ID: " + systemUser.ID;
                            aLog.loginStatus = "Successful";
                            aLog.Message = "System User attempted login.";
                            aLog.IpAddress = Request.UserHostAddress + "/" + Request.UserHostName;
                            //Request.ServerVariables["REMOTE_ADDR"]
                            aLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_UserG);
                            LogWriter.AccessLogWriter(aLog);
                            #endregion

                            if (!string.IsNullOrEmpty(SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage)))
                            {
                                string url = SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage);
                                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                                Response.Redirect(url, false);
                            }
                            else
                            {
                                string role = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                                if (systemUser.IsSA == true)
                                {
                                    Response.Redirect("~/Admission/Admin/AdminHome", false);
                                }
                                else if (systemUser.IsSA == false)
                                {
                                    Response.Redirect("~/Admission/Office/OfficeHome.aspx", false);
                                }
                            }
                        }
                        else
                        {
                            lblMessage.Text = "<strong>Login failed...</strong>User role does not exist.";
                            messagePanel.Visible = true;
                            ClearFields();
                        }
                    }
                    else
                    {
                        lblMessage.Text = "<strong>Login failed...</strong>Incorrect Username or Password.";
                        messagePanel.Visible = true;
                        ClearFields();
                    }
                }
                else if (certificateCandidateUser != null && certificateCandidateUser.ID > 0)
                {
                    if (certificateCandidateUser.Password.Equals(pWd) && certificateCandidateUser.UsernameLoginId.Equals(uId))
                    {

                        if (certificateCandidateUser.ValidTill > DateTime.Now) //if user is within valid date.
                        {
                            #region A-LOG
                            DAL_Log.AccessLog aLog = new DAL_Log.AccessLog();
                            aLog.DateTime = DateTime.Now;
                            aLog.LoginID = "CU-ID: " + certificateCandidateUser.ID;
                            aLog.loginStatus = "Successful";
                            aLog.Message = "Certificate Candidate attempted login and is successful.";
                            aLog.IpAddress = Request.UserHostAddress + "/" + Request.UserHostName;
                            aLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_UserG);
                            LogWriter.AccessLogWriter(aLog);
                            #endregion

                            SessionSGD.SaveObjToSession<long>(certificateCandidateUser.ID, SessionName.Common_UserId); //this is candidate user primary key
                            SessionSGD.SaveObjToSession<string>(certificateCandidateUser.UsernameLoginId, SessionName.Common_LoginID); //this is the username
                            SessionSGD.SaveObjToSession<string>("CertificateCandidate", SessionName.Common_RoleName);
                            if (!string.IsNullOrEmpty(SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage)))
                            {
                                string url = SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage);
                                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                                Response.Redirect(url, false);
                            }
                            else
                            {
                                Response.Redirect("~/Admission/CertificateCandidate/CertificateApplicationBasic.aspx", false);
                            }
                        }
                        else if (certificateCandidateUser.ValidTill < DateTime.Now) //if user is outside the valid date.
                        {
                            #region A-LOG
                            DAL_Log.AccessLog aLogExp = new DAL_Log.AccessLog();
                            aLogExp.DateTime = DateTime.Now;
                            aLogExp.LoginID = "CU-ID: " + certificateCandidateUser.ID;
                            aLogExp.loginStatus = "Unsuccessful/User credentials expired.";
                            aLogExp.Message = "Certificate Candidate attempted login.";
                            aLogExp.IpAddress = Request.UserHostAddress + "/" + Request.UserHostName;
                            aLogExp.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_UserG);
                            LogWriter.AccessLogWriter(aLogExp);
                            #endregion

                            lblMessage.Text = "<strong>Login failed...</strong>Your username and password has expired.";
                            messagePanel.Visible = true;
                            ClearFields();
                        }
                    }
                    else
                    {
                        lblMessage.Text = "<strong>Login failed...</strong>Incorrect Username or Password.";
                        messagePanel.Visible = true;
                        ClearFields();
                    }
                }
                else if (postgraduateDiplomaCandidateUser != null && postgraduateDiplomaCandidateUser.ID > 0)
                {
                    if (postgraduateDiplomaCandidateUser.Password.Equals(pWd) && postgraduateDiplomaCandidateUser.UsernameLoginId.Equals(uId))
                    {

                        if (postgraduateDiplomaCandidateUser.ValidTill > DateTime.Now) //if user is within valid date.
                        {
                            #region A-LOG
                            DAL_Log.AccessLog aLog = new DAL_Log.AccessLog();
                            aLog.DateTime = DateTime.Now;
                            aLog.LoginID = "CU-ID: " + postgraduateDiplomaCandidateUser.ID;
                            aLog.loginStatus = "Successful";
                            aLog.Message = "Postgraduate Diploma Candidate attempted login and is successful.";
                            aLog.IpAddress = Request.UserHostAddress + "/" + Request.UserHostName;
                            aLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_UserG);
                            LogWriter.AccessLogWriter(aLog);
                            #endregion

                            SessionSGD.SaveObjToSession<long>(postgraduateDiplomaCandidateUser.ID, SessionName.Common_UserId); //this is candidate user primary key
                            SessionSGD.SaveObjToSession<string>(postgraduateDiplomaCandidateUser.UsernameLoginId, SessionName.Common_LoginID); //this is the username
                            SessionSGD.SaveObjToSession<string>("PostgraduateDiploma", SessionName.Common_RoleName);
                            if (!string.IsNullOrEmpty(SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage)))
                            {
                                string url = SessionSGD.GetObjFromSession<string>(SessionName.Common_RedirectPage);
                                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                                Response.Redirect(url, false);
                            }
                            else
                            {
                                Response.Redirect("~/Admission/PostgraduateDiploma/PostgraduateDiplomaApplicationBasic.aspx", false);
                            }
                        }
                        else if (postgraduateDiplomaCandidateUser.ValidTill < DateTime.Now) //if user is outside the valid date.
                        {
                            #region A-LOG
                            DAL_Log.AccessLog aLogExp = new DAL_Log.AccessLog();
                            aLogExp.DateTime = DateTime.Now;
                            aLogExp.LoginID = "CU-ID: " + postgraduateDiplomaCandidateUser.ID;
                            aLogExp.loginStatus = "Unsuccessful/User credentials expired.";
                            aLogExp.Message = "postgraduate Diploma Candidate attempted login.";
                            aLogExp.IpAddress = Request.UserHostAddress + "/" + Request.UserHostName;
                            aLogExp.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";" +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_UserG);
                            LogWriter.AccessLogWriter(aLogExp);
                            #endregion

                            lblMessage.Text = "<strong>Login failed...</strong>Your username and password has expired.";
                            messagePanel.Visible = true;
                            ClearFields();
                        }
                    }
                    else
                    {
                        lblMessage.Text = "<strong>Login failed...</strong>Incorrect Username or Password.";
                        messagePanel.Visible = true;
                        ClearFields();
                    }
                }
                else if (candidateUser == null && systemUser == null)
                {
                    lblMessage.Text = "User not found.";
                    messagePanel.Visible = true;
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Admission/Message.aspx?msg=LoginError", false);
            }
        }

    }
}