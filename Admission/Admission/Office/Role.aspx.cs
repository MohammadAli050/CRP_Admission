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
    public partial class Role : PageBase
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
                LoadListView(null);
            }
        }

        private long CurrentRoleID
        {
            get
            {
                if (ViewState["CurrentRoleID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentRoleID"].ToString());
            }
            set
            {
                ViewState["CurrentRoleID"] = value;
            }
        }

        private void ClearFields()
        {
            txtRoleName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            chbxIsActive.Checked = false;
        }

        private void LoadListView(bool? isActive)
        {
            using(var db = new OfficeDataManager())
            {
                //TODO set for super admin
                List<DAL.Role> listRole = db.GetAllRole_ND()
                    .Where(a => a.IsSA == false && a.IsActive == true)
                    .OrderBy(a=>a.RoleName)
                    .ToList();
                if(listRole != null)
                {
                    lvRoleActive.DataSource = listRole;
                    lblCount.Text = listRole.Count().ToString();
                }
                else
                {
                    lvRoleActive.DataSource = null;
                    lblCount.Text = "0";
                }
                lvRoleActive.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            DAL.Role obj = new DAL.Role();

            obj.RoleName = txtRoleName.Text;
            obj.Description = txtDescription.Text;
            obj.GUID = Guid.NewGuid();
            obj.IsActive = chbxIsActive.Checked;
            obj.IsSA = false;

            obj.DateCreated = DateTime.Now;
            obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
            obj.DateModified = null;
            obj.ModifiedBy = null;

            obj.ID = CurrentRoleID;
            try
            {
                if (obj.ID > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        obj.DateModified = DateTime.Now;
                        obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK

                        db.Update<DAL.Role>(obj);
                    }
                    lblMessage.Text = "Role updated successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;
                }
                else
                {
                    using (var db = new OfficeDataManager())
                    {
                        db.Insert<DAL.Role>(obj);
                        id = obj.ID;
                        //List<DAL.SP_ProgramsGetAllFromUCAM_Result> obj1 = new List<SP_ProgramsGetAllFromUCAM_Result>();
                        //obj1 = db.AdmissionDB.SP_ProgramsGetAllFromUCAM().ToList();
                    }
                    if (id > 0)
                    {
                        lblMessage.Text = "Role saved successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unable to save/update data.";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
            btnSave.Text = "Save";
            ClearFields();
            LoadListView(null);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void lvRoleActive_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Role role = (DAL.Role)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblRoleName = (Label)currentItem.FindControl("lblRoleName");
                Label lblDescription = (Label)currentItem.FindControl("lblDescription");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblRoleName.Text = role.RoleName;
                lblDescription.Text = role.Description;

                if (role.IsActive == true)
                {
                    lblIsActive.Text = "YES";
                    lblIsActive.ForeColor = Color.Green;
                }
                else
                {
                    lblIsActive.Text = "NO";
                    lblIsActive.ForeColor = Color.Crimson;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = role.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = role.ID.ToString();
            }
        }

        protected void lvRoleActive_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objectToDelete = db.AdmissionDB.Roles.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.Role>(objectToDelete);
                        CurrentRoleID = 0;
                    }
                    lblMessage.Text = "Role deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to delete.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                LoadListView(null);
            }
            else if (e.CommandName == "Update")
            {
                btnSave.Text = "Update";
                using (var db = new OfficeDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.Roles.Find(Convert.ToInt64(e.CommandArgument));
                    ClearFields();
                    txtRoleName.Text = objectToUpdate.RoleName;
                    txtDescription.Text = objectToUpdate.Description;
                    if (objectToUpdate.IsActive == true)
                    {
                        chbxIsActive.Checked = true;
                    }
                    else
                    {
                        chbxIsActive.Checked = false;
                    }
                    CurrentRoleID = objectToUpdate.ID;
                }
            }
        }

        protected void lvRoleActive_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvRoleActive_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }
    }
}