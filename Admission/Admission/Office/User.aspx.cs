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

namespace Admission.Admission.Office
{
    public partial class User : PageBase
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
                LoadDDL();
                LoadListView(null);
            }
        }

        private long CurrentSystemUserID
        {
            get
            {
                if (ViewState["CurrentSystemUserID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentSystemUserID"].ToString());
            }
            set
            {
                ViewState["CurrentSystemUserID"] = value;
            }
        }

        private long CurrentSystemUserInRoleID
        {
            get
            {
                if (ViewState["CurrentSystemUserInRoleID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentSystemUserInRoleID"].ToString());
            }
            set
            {
                ViewState["CurrentSystemUserInRoleID"] = value;
            }
        }

        private long CurrentSystemUserInRoleSystemUserID
        {
            get
            {
                if (ViewState["CurrentSystemUserInRoleSystemUserID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentSystemUserInRoleSystemUserID"].ToString());
            }
            set
            {
                ViewState["CurrentSystemUserInRoleSystemUserID"] = value;
            }
        }

        private void LoadDDL()
        {
            string roleName = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
            //filter for system admin
            if (roleName != null && roleName == "SuperAdmin")
            {
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind<DAL.Role>(ddlRole,
                        db.AdmissionDB.Roles
                        .OrderBy(a => a.RoleName).ToList(),
                        "RoleName", "ID", EnumCollection.ListItemType.Select);
                }
            }
            else if(roleName != null && roleName != "SuperAdmin")
            {
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind<DAL.Role>(ddlRole,
                        db.AdmissionDB.Roles
                        .Where(a=>a.IsActive == true && a.IsSA == false)
                        .OrderBy(a => a.RoleName).ToList(),
                        "RoleName", "ID", EnumCollection.ListItemType.Select);
                }
            }
        }

        private void ClearFields()
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            //txtEmail.Text = string.Empty;
            //txtMobile.Text = string.Empty;
        }

        private void ClearFieldsRole()
        {
            ddlRole.SelectedIndex = -1;
            //txtEndDate.Text = string.Empty;
            //txtStartDate.Text = string.Empty;
        }

        private void LoadListView(long? roleId)
        {
            string roleName = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
            using (var db = new OfficeDataManager())
            {
                //filter for system admin
                if (roleName != null && roleName == "SuperAdmin")
                {
                    List<DAL.SystemUser> list = db.GetAllSystemUser_AD();
                    if (list != null && list.Any())
                    {
                        lvUser.DataSource = list;
                        lblCount.Text = list.Count().ToString();
                    }
                    else
                    {
                        lvUser.DataSource = null;
                        lblCount.Text = "0";
                    }
                    lvUser.DataBind();
                }
                else if (roleName != null && roleName != "SuperAdmin")
                {
                    List<DAL.SystemUser> list = db.GetAllSystemUser_AD().Where(a=>a.IsActive == true && a.IsSA == false).ToList();
                    if (list != null && list.Any())
                    {
                        lvUser.DataSource = list;
                        lblCount.Text = list.Count().ToString();
                    }
                    else
                    {
                        lvUser.DataSource = null;
                        lblCount.Text = "0";
                    }
                    lvUser.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long sysUserId = -1;

            DAL.SystemUser sysUser = new DAL.SystemUser();

            sysUser.Username = txtUsername.Text;
            //TODO encrypt password
            sysUser.Password = txtPassword.Text;
            sysUser.IsActive = chbxIsActive.Checked;
            sysUser.IsSA = false;

            sysUser.ID = CurrentSystemUserID;
            try
            {
                if (sysUser.ID > 0 && CurrentSystemUserID > 0)
                {
                    using (var tempdb = new OfficeDataManager())
                    {
                        var tempSysUser = tempdb.AdmissionDB.SystemUsers.Find(CurrentSystemUserID);
                        //sysUser.Password = tempSysUser.Password;
                        sysUser.CreatedBy = tempSysUser.CreatedBy;
                        sysUser.DateCreated = tempSysUser.DateCreated;
                    }
                    using (var db = new OfficeDataManager())
                    {
                        sysUser.DateModified = DateTime.Now;
                        sysUser.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK

                        db.Update<DAL.SystemUser>(sysUser);
                    }
                    lblMessageUser.Text = "User updated successfully.";
                    messagePanelUser.CssClass = "alert alert-success";
                    messagePanelUser.Visible = true;
                }
                else
                {
                    using (var db = new OfficeDataManager())
                    {
                        sysUser.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                        sysUser.DateCreated = DateTime.Now;
                        db.Insert<DAL.SystemUser>(sysUser);
                        sysUserId = sysUser.ID;
                    }

                    if (sysUserId > 0)
                    {
                        lblMessageUser.Text = "User saved successfully.";
                        messagePanelUser.CssClass = "alert alert-success";
                        messagePanelUser.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessageUser.Text = "Unable to save/update data.";
                messagePanelUser.CssClass = "alert alert-danger";
                messagePanelUser.Visible = true;
            }
            btnSave.Text = "Save";
            ClearFields();
            LoadListView(null);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void btnSaveUserInRole_Click(object sender, EventArgs e)
        {
            long systemUserInRoleId = -1;

            DAL.SystemUserInRole sysUserInRole = new DAL.SystemUserInRole();

            sysUserInRole.SystemUserID = CurrentSystemUserInRoleSystemUserID;
            sysUserInRole.RoleID = Convert.ToInt64(ddlRole.SelectedValue);
            //sysUserInRole.StartDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", null);
            //sysUserInRole.EndDate = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", null); ;

            sysUserInRole.ID = CurrentSystemUserInRoleID;
            try
            {
                //add role
                if (CurrentSystemUserInRoleSystemUserID > 0 && CurrentSystemUserInRoleID == 0)
                {
                    sysUserInRole.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                    sysUserInRole.DateCreated = DateTime.Now;
                    sysUserInRole.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                    sysUserInRole.DateModified = DateTime.Now;

                    using (var db_temp = new OfficeDataManager())
                    {
                        DAL.SystemUserInRole obj_Temp = db_temp.AdmissionDB.SystemUserInRoles
                            .Where(a => a.SystemUserID == CurrentSystemUserInRoleSystemUserID)
                            .FirstOrDefault();
                        if (obj_Temp != null && obj_Temp.ID > 0)
                        {
                            lblMessageUserInRole.Text = "User with role already exist";
                            messagePanelUserInRole.CssClass = "alert alert-danger";
                            messagePanelUserInRole.Visible = true;
                        }
                        else
                        {
                            using (var db = new OfficeDataManager())
                            {
                                db.Insert<DAL.SystemUserInRole>(sysUserInRole);
                                systemUserInRoleId = sysUserInRole.ID;
                            }
                            if (systemUserInRoleId > 0)
                            {
                                lblMessageUserInRole.Text = "User role added successfully.";
                                messagePanelUserInRole.CssClass = "alert alert-success";
                                messagePanelUserInRole.Visible = true;
                            }
                        }
                    }
                }
                //edit role
                else if (CurrentSystemUserInRoleSystemUserID > 0 && CurrentSystemUserInRoleID > 0)
                {
                    using (var tempdb = new OfficeDataManager())
                    {
                        var tempSysUserInRole = tempdb.AdmissionDB.SystemUserInRoles.Find(CurrentSystemUserInRoleID);
                        sysUserInRole.CreatedBy = tempSysUserInRole.CreatedBy;
                        sysUserInRole.DateCreated = tempSysUserInRole.DateCreated;
                    }
                    using (var db = new OfficeDataManager())
                    {
                        sysUserInRole.DateModified = DateTime.Now;
                        sysUserInRole.ModifiedBy = 999;

                        db.Update<DAL.SystemUserInRole>(sysUserInRole);
                    }
                    lblMessageUserInRole.Text = "User role updated successfully.";
                    messagePanelUserInRole.CssClass = "alert alert-success";
                    messagePanelUserInRole.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessageUserInRole.Text = "Unable to update user role.";
                messagePanelUserInRole.CssClass = "alert alert-danger";
                messagePanelUserInRole.Visible = true;
            }
            btnSaveUserInRole.Text = "Save";
            ClearFieldsRole();
            LoadListView(null);
        }

        protected void btnClearUserInRole_Click(object sender, EventArgs e)
        {
            ClearFieldsRole();
        }

        protected void lvUser_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SystemUser sysUser = (DAL.SystemUser)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblUsername = (Label)currentItem.FindControl("lblUsername");
                Label lblRole = (Label)currentItem.FindControl("lblRole");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                LinkButton lnkAddRole = (LinkButton)currentItem.FindControl("lnkAddRole");
                LinkButton lnkEditRole = (LinkButton)currentItem.FindControl("lnkEditRole");
                LinkButton lnkDeleteRole = (LinkButton)currentItem.FindControl("lnkDeleteRole");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblUsername.Text = sysUser.Username;

                using (var db = new OfficeDataManager())
                {
                    DAL.SystemUserInRole sysUserInRole = db.GetSysUserInRoleByUserID_AD(sysUser.ID);
                    if (sysUserInRole != null && sysUserInRole.ID > 0)
                    {
                        lblRole.Text = sysUserInRole.Role.RoleName;
                        lnkAddRole.Visible = false;
                        lnkEditRole.Visible = true;
                        lnkDeleteRole.Visible = true;
                    }
                    else
                    {
                        lnkAddRole.Visible = true;
                        lnkAddRole.CssClass = "btn btn-warning";
                        lnkEditRole.Visible = false;
                        lnkDeleteRole.Visible = false;
                    }
                }


                if (sysUser.IsActive == true)
                {
                    lblIsActive.Text = "YES";
                    lblIsActive.ForeColor = Color.Green;
                }
                else if (sysUser.IsActive == false)
                {
                    lblIsActive.Text = "NO";
                    lblIsActive.ForeColor = Color.Crimson;
                }

                if (sysUser.IsSA == true)
                {
                    lnkAddRole.Visible = false;
                    lnkDeleteRole.Visible = false;
                    lnkEditRole.Visible = false;

                    lnkEdit.Visible = false;
                    lnkDelete.Visible = false;
                }
                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = sysUser.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = sysUser.ID.ToString();

                lnkAddRole.CommandName = "AddRole";
                lnkAddRole.CommandArgument = sysUser.ID.ToString();

                lnkEditRole.CommandName = "EditRole";
                lnkEditRole.CommandArgument = sysUser.ID.ToString();

                lnkDeleteRole.CommandName = "DeleteRole";
                lnkDeleteRole.CommandArgument = sysUser.ID.ToString();
            }
        }

        protected void lvUser_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                btnSave.Text = "Update";
                using (var db = new OfficeDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.SystemUsers.Find(Convert.ToInt64(e.CommandArgument));
                    //LoadDDL();
                    ClearFields();
                    txtUsername.Text = objectToUpdate.Username;
                    txtPassword.Text = objectToUpdate.Password;
                    //txtPassword.Enabled = false;
                    if (objectToUpdate.IsActive == true)
                    {
                        chbxIsActive.Checked = true;
                    }
                    else
                    {
                        chbxIsActive.Checked = false;
                    }
                    CurrentSystemUserID = objectToUpdate.ID;
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        long sysUserId = Convert.ToInt64(e.CommandArgument);

                        var objectRoleToDelete = db.AdmissionDB.SystemUsers.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.SystemUser>(objectRoleToDelete);

                        CurrentSystemUserID = 0;
                    }
                    lblMessageUser.Text = "User deleted successfully.";
                    messagePanelUser.CssClass = "alert alert-warning";
                    messagePanelUser.Visible = true;
                    ClearFields();
                }
                catch (Exception ex)
                {
                    lblMessageUser.Text = "Unable to delete.";
                    messagePanelUser.CssClass = "alert alert-danger";
                    messagePanelUser.Visible = true;
                }
                LoadListView(null);
            }
            else if (e.CommandName == "AddRole")
            {
                using (var db = new OfficeDataManager())
                {
                    DAL.SystemUser sysUserObj = db.AdmissionDB.SystemUsers.Find(Convert.ToInt64(e.CommandArgument));
                    if (sysUserObj != null && sysUserObj.ID > 0)
                    {
                        lblUsername.Text = sysUserObj.Username;
                        CurrentSystemUserInRoleSystemUserID = sysUserObj.ID;
                        CurrentSystemUserInRoleID = 0;
                        ddlRole.Focus();
                    }
                }
            }
            else if (e.CommandName == "EditRole")
            {
                btnSaveUserInRole.Text = "Update Role";
                long sysUserId = Convert.ToInt64(e.CommandArgument);
                using (var db = new OfficeDataManager())
                {
                    DAL.SystemUserInRole sysUserInRoleObj = db.AdmissionDB.SystemUserInRoles
                        .Where(a => a.SystemUserID == sysUserId)
                        .FirstOrDefault();
                    if (sysUserInRoleObj != null && sysUserInRoleObj.ID > 0)
                    {
                        using (var db_temp = new OfficeDataManager())
                        {
                            DAL.SystemUser sysUser = db.AdmissionDB.SystemUsers.Find(sysUserInRoleObj.SystemUserID);
                            if (sysUser != null && sysUser.ID > 0)
                            {
                                lblUsername.Text = sysUser.Username;
                                ddlRole.SelectedValue = sysUserInRoleObj.RoleID.ToString();
                                //txtStartDate.Text = sysUserInRoleObj.StartDate.ToString();
                                //txtEndDate.Text = sysUserInRoleObj.EndDate.ToString();
                                CurrentSystemUserInRoleSystemUserID = sysUser.ID;
                                CurrentSystemUserInRoleID = sysUserInRoleObj.ID;
                            }
                            else
                            {
                                lblMessageUserInRole.Text = "User not found";
                                messagePanelUserInRole.CssClass = "alert alert-warning";
                                messagePanelUserInRole.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        lblMessageUserInRole.Text = "User with role not found";
                        messagePanelUserInRole.CssClass = "alert alert-warning";
                        messagePanelUserInRole.Visible = true;
                    }
                }
            }
            else if (e.CommandName == "DeleteRole")
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        long sysUserId = Convert.ToInt64(e.CommandArgument);

                        var objectRoleToDelete = db.AdmissionDB.SystemUserInRoles.Where(a => a.SystemUserID == sysUserId).FirstOrDefault();
                        db.Delete<DAL.SystemUserInRole>(objectRoleToDelete);

                        CurrentSystemUserInRoleID = 0;
                        CurrentSystemUserInRoleSystemUserID = 0;
                    }
                    lblMessageUserInRole.Text = "Role deleted successfully.";
                    messagePanelUserInRole.CssClass = "alert alert-warning";
                    messagePanelUserInRole.Visible = true;
                    ClearFieldsRole();
                }
                catch (Exception ex)
                {
                    lblMessageUserInRole.Text = "Unable to delete.";
                    messagePanelUserInRole.CssClass = "alert alert-danger";
                    messagePanelUserInRole.Visible = true;
                }
                LoadListView(null);
            }
        }

        protected void lvUser_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvUser_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

    }
}