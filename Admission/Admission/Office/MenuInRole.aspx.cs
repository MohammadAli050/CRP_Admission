using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class MenuInRole : PageBase
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
            }
        }

        private void ClearFields()
        {
            ddlRole.SelectedIndex = -1;
            ddlParentMenu.SelectedIndex = -1;
        }

        private void EnableFields()
        {
            ddlParentMenu.Enabled = true;
            ddlRole.Enabled = true;
            btnAddParentMenu.Enabled = true;
            //btnClearParentMenu.Enabled = true;
        }

        private void DisableFields()
        {
            ddlParentMenu.Enabled = false;
            ddlRole.Enabled = false;
            btnAddParentMenu.Enabled = false;
            //btnClearParentMenu.Enabled = false;
        }

        private void LoadDDL()
        {
            string roleName = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
            if (roleName == "SuperAdmin")
            {
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind<DAL.Role>(ddlRole, db.AdmissionDB.Roles.Where(a => a.IsActive == true).ToList(), "RoleName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.Menu>(ddlParentMenu, db.AdmissionDB.Menus.Where(a => a.ParentMenuID == null).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);
                }
            }
            else
            {
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind<DAL.Role>(ddlRole, db.AdmissionDB.Roles.Where(a => a.IsActive == true && a.IsSA == false).ToList(), "RoleName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.Menu>(ddlParentMenu, db.AdmissionDB.Menus.Where(a => a.ParentMenuID == null && a.IsAdmin == false).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------
        #region PARENT MENU

        private void ClearMessagePanelParent()
        {
            lblMessageParent.Text = string.Empty;
            messagePanelParent.CssClass = "";
            messagePanelParent.Visible = false;
        }

        protected void btnAddParentMenu_Click(object sender, EventArgs e)
        {
            long selectedParentMenu = -1;
            selectedParentMenu = Convert.ToInt32(ddlParentMenu.SelectedValue);
            LoadParentMenuLV(selectedParentMenu);
            lvParentMenu.Visible = true;
            lvChildMenu.Visible = false;
            DisableFields();
        }

        protected void btnClearParentMenu_Click(object sender, EventArgs e)
        {
            ClearFields();
            ClearMessagePanelParent();
            ClearMessagePanelChild();
            lvChildMenu.Visible = false;
            lvParentMenu.Visible = false;
            EnableFields();
        }

        private void LoadParentMenuLV(long? parentMenuId)
        {
            if (parentMenuId > 0)
            {
                using (var db = new OfficeDataManager())
                {
                    List<DAL.Menu> list = db.GetAllRootMenu()
                        .Where(a => a.ID == parentMenuId && a.ParentMenuID == null)
                        .ToList();
                    if (list != null && list.Any())
                    {
                        lvParentMenu.DataSource = list;
                    }
                    else
                    {
                        lvParentMenu.DataSource = null;
                    }
                    lvParentMenu.DataBind();
                }
            }
        }

        protected void lvParentMenu_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Menu menu = (DAL.Menu)((ListViewDataItem)(e.Item)).DataItem;

                Label lblParentMenuName = (Label)currentItem.FindControl("lblParentMenuName");

                LinkButton lnkParentMenuLoad = (LinkButton)currentItem.FindControl("lnkParentMenuLoad");
                LinkButton lnkParentMenuAdd = (LinkButton)currentItem.FindControl("lnkParentMenuAdd");
                LinkButton lnkParentMenuDelete = (LinkButton)currentItem.FindControl("lnkParentMenuDelete");

                lblParentMenuName.Text = menu.Name;

                using (var db = new OfficeDataManager())
                {
                    long selectedRoleId = Convert.ToInt64(ddlRole.SelectedValue);
                    if (selectedRoleId > 0)
                    {
                        DAL.MenuInRole menuInRole = db.GetMenuInRoleByRoleIDMenuID(selectedRoleId, menu.ID);
                        if (menuInRole != null && menuInRole.ID > 0)
                        {
                            lnkParentMenuLoad.Visible = true;
                            lnkParentMenuAdd.Visible = false;
                            lnkParentMenuDelete.Visible = true;
                        }
                        else
                        {
                            lnkParentMenuLoad.Visible = false;
                            lnkParentMenuAdd.Visible = true;
                            lnkParentMenuDelete.Visible = false;
                        }
                    }
                }

                lnkParentMenuLoad.CommandName = "LoadParent";
                lnkParentMenuLoad.CommandArgument = menu.ID.ToString();

                lnkParentMenuAdd.CommandName = "AddParent";
                lnkParentMenuAdd.CommandArgument = menu.ID.ToString();

                lnkParentMenuDelete.CommandName = "DeleteParent";
                lnkParentMenuDelete.CommandArgument = menu.ID.ToString();
            }
        }

        protected void lvParentMenu_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            long selectedParentMenuId = Convert.ToInt64(ddlParentMenu.SelectedValue);
            long selectedRoleId = Convert.ToInt64(ddlRole.SelectedValue);

            
            if (e.CommandName == "LoadParent")
            {
                LoadChildMenuLV(Convert.ToInt64(e.CommandArgument));
                lvChildMenu.Visible = true;
            }
            // end if(e.CommandName == "LoadParent")
            else if (e.CommandName == "AddParent")
            {
                if (selectedParentMenuId > 0 && selectedRoleId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        DAL.MenuInRole obj = new DAL.MenuInRole();
                        obj.RoleID = selectedRoleId;
                        obj.MenuID = selectedParentMenuId;
                        obj.ValidTill = null;
                        obj.IsActive = true;
                        obj.DateCreated = DateTime.Now;
                        obj.CreatedBy = 99;
                        obj.DateModified = null;
                        obj.ModifiedBy = null;

                        try
                        {
                            using (var db_temp = new OfficeDataManager())
                            {
                                var objExist = db_temp.AdmissionDB.MenuInRoles
                                    .Where(a => a.RoleID == selectedRoleId && a.MenuID == selectedParentMenuId)
                                    .FirstOrDefault();
                                if (objExist != null && objExist.ID > 0)
                                {
                                    lblMessageParent.Text = "Parent menu already exist";
                                    messagePanelParent.CssClass = "alert alert-warning";
                                    messagePanelParent.Visible = true;
                                }
                                else
                                {
                                    db.Insert<DAL.MenuInRole>(obj);

                                    lblMessageParent.Text = "Parent menu added";
                                    messagePanelParent.CssClass = "alert alert-success";
                                    messagePanelParent.Visible = true;

                                    LoadChildMenuLV(selectedParentMenuId);
                                    lvChildMenu.Visible = true;
                                    LoadParentMenuLV(selectedParentMenuId);
                                    //updatePanelChildMenu.Update();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessageParent.Text = "Unable to add parent menu";
                            messagePanelParent.CssClass = "alert alert-danger";
                            messagePanelParent.Visible = true;
                        }
                    }
                }
                lvParentMenu.Enabled = true;
            }
            // end else if(e.CommandName == "AddParent")
            else if (e.CommandName == "DeleteParent")
            {
                try
                {
                    using (var db_Child = new OfficeDataManager())
                    {
                        List<DAL.MenuInRole> childMenus = db_Child.GetAllChildMenuInRoleByParentMenuID(Convert.ToInt64(e.CommandArgument));
                        if (childMenus != null && childMenus.Any())
                        {
                            foreach (var item in childMenus)
                            {
                                using (var db_ChildRemove = new OfficeDataManager())
                                {
                                    var childMenuToRemove = db_ChildRemove.AdmissionDB.MenuInRoles.Find(item.ID);
                                    if (childMenuToRemove != null && childMenuToRemove.ID > 0)
                                    {
                                        db_ChildRemove.Delete<DAL.MenuInRole>(childMenuToRemove);
                                    }
                                }
                            }
                        }
                    }
                    using (var db_Parent = new OfficeDataManager())
                    {
                        DAL.MenuInRole parentMenu = db_Parent.GetParentMenuInRoleByMenuID(Convert.ToInt64(e.CommandArgument));
                        if (parentMenu != null && parentMenu.ID > 0)
                        {
                            db_Parent.Delete<DAL.MenuInRole>(parentMenu);
                        }
                    }
                    lblMessageParent.Text = "Menu deleted successfully.";
                    messagePanelParent.CssClass = "alert alert-warning";
                    messagePanelParent.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessageParent.Text = "Unable to delete.";
                    messagePanelParent.CssClass = "alert alert-danger";
                    messagePanelParent.Visible = true;
                }
                LoadParentMenuLV(Convert.ToInt64(ddlParentMenu.SelectedValue));
                LoadChildMenuLV(Convert.ToInt64(ddlParentMenu.SelectedValue));
            }
            
            //end else if(e.CommandName == "DeleteParent")
        }

        protected void lvParentMenu_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
        }

        protected void lvParentMenu_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }

        #endregion
        //-----------------------------------------------------------------------------------------------------------------------
        #region CHILD MENU

        private void ClearMessagePanelChild()
        {
            lblMessageChild.Text = string.Empty;
            messagePanelChild.CssClass = "";
            messagePanelChild.Visible = false;
        }

        private void LoadChildMenuLV(long? parentMenuId)
        {
            if (parentMenuId > 0)
            {
                using (var db = new OfficeDataManager())
                {
                    List<DAL.Menu> list = db.GetMenuByParentMenuID(parentMenuId).ToList();
                    if (list != null && list.Any())
                    {
                        lvChildMenu.DataSource = list;
                    }
                    else
                    {
                        lvChildMenu.DataSource = null;
                    }
                    lvChildMenu.DataBind();
                }
            }
        }

        protected void lvChildMenu_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Menu menu = (DAL.Menu)((ListViewDataItem)(e.Item)).DataItem;

                Label lblChildMenuName = (Label)currentItem.FindControl("lblChildMenuName");

                LinkButton lnkChildMenuAdd = (LinkButton)currentItem.FindControl("lnkChildMenuAdd");
                LinkButton lnkChildMenuDelete = (LinkButton)currentItem.FindControl("lnkChildMenuDelete");

                lblChildMenuName.Text = menu.Name;

                using (var db = new OfficeDataManager())
                {
                    long selectedRoleId = Convert.ToInt64(ddlRole.SelectedValue);
                    if (selectedRoleId > 0)
                    {
                        DAL.MenuInRole menuInRole = db.GetMenuInRoleByRoleIDMenuID(selectedRoleId, menu.ID);
                        if (menuInRole != null && menuInRole.ID > 0)
                        {
                            lnkChildMenuAdd.Visible = false;
                            lnkChildMenuDelete.Visible = true;
                        }
                        else
                        {
                            lnkChildMenuAdd.Visible = true;
                            lnkChildMenuDelete.Visible = false;
                        }
                    }
                }

                lnkChildMenuAdd.CommandName = "AddChild";
                lnkChildMenuAdd.CommandArgument = menu.ID.ToString();

                lnkChildMenuDelete.CommandName = "DeleteChild";
                lnkChildMenuDelete.CommandArgument = menu.ID.ToString();
            }
        }

        protected void lvChildMenu_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            long selectedChildMenuId = Convert.ToInt64(e.CommandArgument);
            long selectedParentMenuId = Convert.ToInt64(ddlParentMenu.SelectedValue);
            long selectedRoleId = Convert.ToInt64(ddlRole.SelectedValue);

            if (e.CommandName == "AddChild")
            {
                if (selectedChildMenuId > 0 && selectedRoleId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        DAL.MenuInRole obj = new DAL.MenuInRole();
                        obj.RoleID = selectedRoleId;
                        obj.MenuID = selectedChildMenuId;
                        obj.ValidTill = null;
                        obj.IsActive = true;
                        obj.DateCreated = DateTime.Now;
                        obj.CreatedBy = 99;
                        obj.DateModified = null;
                        obj.ModifiedBy = null;

                        try
                        {
                            using (var db_temp = new OfficeDataManager())
                            {
                                var objExist = db_temp.AdmissionDB.MenuInRoles
                                    .Where(a => a.RoleID == selectedRoleId && a.MenuID == selectedChildMenuId)
                                    .FirstOrDefault();
                                if (objExist != null && objExist.ID > 0)
                                {
                                    lblMessageParent.Text = "Child menu already exist";
                                    messagePanelParent.CssClass = "alert alert-warning";
                                    messagePanelParent.Visible = true;
                                }
                                else
                                {
                                    db.Insert<DAL.MenuInRole>(obj);

                                    lblMessageChild.Text = "Child menu added";
                                    messagePanelChild.CssClass = "alert alert-success";
                                    messagePanelChild.Visible = true;

                                    LoadChildMenuLV(selectedParentMenuId);
                                    lvChildMenu.Visible = true;
                                    //updatePanelChildMenu.Update();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessageChild.Text = "Unable to add child menu";
                            messagePanelChild.CssClass = "alert alert-danger";
                            messagePanelChild.Visible = true;
                        }
                    }
                }
            }
            // end if(e.CommandName == "AddChild")
            else if (e.CommandName == "DeleteChild")
            {
                try
                {
                    using (var db_Child = new OfficeDataManager())
                    {
                        DAL.MenuInRole childMenu = db_Child.GetChildMenuInRoleByParentMenuID(Convert.ToInt64(e.CommandArgument), selectedRoleId);
                        if(childMenu != null && childMenu.ID > 0)
                        {
                            db_Child.Delete<DAL.MenuInRole>(childMenu);
                        }
                    }
                    lblMessageChild.Text = "Menu deleted successfully.";
                    messagePanelChild.CssClass = "alert alert-warning";
                    messagePanelChild.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessageChild.Text = "Unable to delete.";
                    messagePanelChild.CssClass = "alert alert-danger";
                    messagePanelChild.Visible = true;
                }
                LoadChildMenuLV(Convert.ToInt64(ddlParentMenu.SelectedValue));
            }
            //end else if(e.CommandName == "DeleteChild")
        }

        protected void lvChildMenu_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvChildMenu_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        #endregion
        //-----------------------------------------------------------------------------------------------------------------------
    }
}