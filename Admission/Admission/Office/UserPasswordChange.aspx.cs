using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class UserPasswordChange : PageBase
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
                LoadSystemUserData();
            }
        }

        private long GetIdOfLoggedInSystemUser()
        {
            long systemUserId = -1;
            systemUserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
            return systemUserId;
        }

        private string GetUserNameOfLoggedInSystemUser()
        {
            string userName = null;
            userName = SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID);
            return userName;
        }

        private void LoadSystemUserData()
        {
            string username = null;
            using(var db = new OfficeDataManager())
            {
                username = db.GetSysterUserNameByID_ND(GetIdOfLoggedInSystemUser());
            }
            if(username != null)
            {
                lblUserName.Text = username;
            }
            else
            {
                lblUserName.Text = "Error retrieving user. Contact system administrator.";
                lblUserName.ForeColor = Color.Crimson;
                btnSubmit.Enabled = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string newPasswordRetype = txtNewPasswordRetype.Text.Trim();

            long userId = GetIdOfLoggedInSystemUser();

            try
            {
                DAL.SystemUser systemUser = null;
                using (var db = new OfficeDataManager())
                {
                    //systemUser = db.AdmissionDB.SystemUsers.AsNoTracking().Where(c => c.ID == userId).FirstOrDefault();
                    systemUser = db.AdmissionDB.SystemUsers.Where(c => c.ID == userId).FirstOrDefault();
                }
                if (systemUser != null)
                {
                    if (oldPassword.Equals(systemUser.Password)) // if password matches
                    {
                        if (newPassword.Equals(newPasswordRetype)) //if new passwords match
                        {
                            systemUser.Password = newPassword;
                            systemUser.DateModified = DateTime.Now;
                            systemUser.ModifiedBy = GetIdOfLoggedInSystemUser();
                            using (var dbUpdate = new OfficeDataManager())
                            {
                                dbUpdate.Update<DAL.SystemUser>(systemUser);
                            }
                            lblMessage.Text = "Password changed successfully.";
                            messagePanel.CssClass = "alert alert-success";
                            messagePanel.Visible = true;
                        }
                        else  // new passwords dont match
                        {
                            lblMessage.Text = "New password do not match.";
                            messagePanel.CssClass = "alert alert-danger";
                            messagePanel.Visible = true;
                            return;
                        }
                    }
                    else //if no match.
                    {
                        lblMessage.Text = "Old password does not match.";
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        return;
                    }
                }
            }catch(Exception ex)
            {
                lblMessage.Text = "Something went wrong. Please contact system administrator.";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }
    }
}