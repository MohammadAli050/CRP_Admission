using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Admin
{
    public partial class Menu : System.Web.UI.Page
    {
        int CurrentPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDDL();
                LoadListView();
            }
        }

        private void ClearFields()
        {
            txtMenuName.Text = string.Empty;
            txtMenuOrder.Text = string.Empty;
            //txtTier.Text = string.Empty;
            txtUrl.Text = string.Empty;
            ddlParentMenu.SelectedIndex = -1;
        }

        private long CurrentMenuID
        {
            get
            {
                if (ViewState["CurrentMenuID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentMenuID"].ToString());
            }
            set
            {
                ViewState["CurrentMenuID"] = value;
            }
        }

        private void LoadDDL()
        {
            List<DAL.Menu> menuList = new List<DAL.Menu>();
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.Menu>(ddlParentMenu, 
                        db.AdmissionDB.Menus
                        .Where(a => a.ParentMenuID == null)
                        .OrderBy(a => a.Name)
                        .ToList(), "Name", "ID", EnumCollection.ListItemType.Select);


                DDLHelper.Bind<DAL.Menu>(ddlMenuFilter,
                        db.AdmissionDB.Menus
                        .Where(a => a.ParentMenuID == null)
                        .OrderBy(a => a.Name)
                        .ToList(), "Name", "ID", EnumCollection.ListItemType.SelectAll);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            DAL.Menu obj = new DAL.Menu();

            obj.Name = txtMenuName.Text;
            //obj.GUID = Guid.NewGuid();
            obj.IsAdmin = chbxIsAdmin.Checked;
            obj.MenuOrder = Convert.ToInt32(txtMenuOrder.Text);
            if(ddlParentMenu.SelectedValue.ToString() == "-1")
            {
                obj.ParentMenuID = null;
            }
            else
            {
                obj.ParentMenuID = Convert.ToInt64(ddlParentMenu.SelectedValue);
            }
            obj.Tier = Convert.ToInt32(txtTier.Text);
            obj.URL = txtUrl.Text;

            obj.ID = CurrentMenuID;
            try
            {

                if (obj.ID > 0 && CurrentMenuID > 0)
                {
                    using (var tempDb = new GeneralDataManager())
                    {
                        var tempObj = tempDb.AdmissionDB.Menus.Find(Convert.ToInt64(CurrentMenuID));
                        if (tempObj.ID > 0)
                        {
                            obj.GUID = tempObj.GUID;
                            obj.CreatedBy = tempObj.CreatedBy;
                            obj.DateCreated = tempObj.DateCreated;
                        }
                    }
                    obj.ModifiedBy = -99;
                    obj.DateModified = DateTime.Now;

                    using (var db = new GeneralDataManager())
                    {
                        db.Update<DAL.Menu>(obj);
                    }
                    lblMessage.Text = "Menu updated successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;

                    CurrentMenuID = 0;
                    LoadDDL();
                    LoadListView();
                    ClearFields();
                    btnSave.Text = "Save";
                }
                else if (CurrentMenuID == 0)
                {
                    obj.GUID = Guid.NewGuid();
                    obj.CreatedBy = -99;
                    obj.DateCreated = DateTime.Now;
                    obj.ModifiedBy = -99;
                    obj.DateModified = DateTime.Now;
                    using (var db = new GeneralDataManager())
                    {
                        db.Insert<DAL.Menu>(obj);
                        id = obj.ID;
                    }
                    if (id > 0)
                    {
                        lblMessage.Text = "Menu saved successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;

                        LoadDDL();
                        LoadListView();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unable to save/update data.";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void btnMenuFilter_Click(object sender, EventArgs e)
        {
            long menuId = -1;
            menuId = Convert.ToInt64(ddlMenuFilter.SelectedValue);
            if(menuId < 0)
            {
                LoadListView();
            }
            else if(menuId > 0)
            {
                LoadListView(menuId);
            }
        }

        private void LoadListView()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.Menu> list = db.GetAllMenu_AD();
                if (list != null)
                {
                    lvMenu.DataSource = list;
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvMenu.DataSource = null;
                    lblCount.Text = "0";
                }
                lvMenu.DataBind();
            }
        }

        private void LoadListView(long menuId)
        {
            if (menuId > 0)
            {
                using (var db = new GeneralDataManager())
                {
                    List<DAL.Menu> list = db.GetAllMenu_AD()
                        .Where(a => a.ParentMenuID == menuId || a.ID == menuId)
                        .ToList();
                    if (list != null)
                    {
                        lvMenu.DataSource = list;
                        lblCount.Text = list.Count().ToString();
                    }
                    else
                    {
                        lvMenu.DataSource = null;
                        lblCount.Text = "0";
                    }
                    lvMenu.DataBind();
                }
            }
        }

        protected void lvMenu_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Menu menu = (DAL.Menu)((ListViewDataItem)(e.Item)).DataItem;

                //Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblUrl = (Label)currentItem.FindControl("lblUrl");
                Label lblParentMenu = (Label)currentItem.FindControl("lblParentMenu");
                Label lblOrder = (Label)currentItem.FindControl("lblOrder");
                Label lblIsAdmin = (Label)currentItem.FindControl("lblIsAdmin");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                //lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblName.Text = menu.Name;
                lblUrl.Text = menu.URL;
                if (menu.ParentMenuID != null)
                {
                    lblParentMenu.Text = menu.Menu2.Name;
                }
                else
                {
                    lblParentMenu.Text = "root";
                }
                lblOrder.Text = menu.MenuOrder.ToString();

                if (menu.IsAdmin == true)
                {
                    lblIsAdmin.Text = "YES";
                    lblIsAdmin.ForeColor = Color.Green;
                }
                else
                {
                    lblIsAdmin.Text = "NO";
                    lblIsAdmin.ForeColor = Color.Crimson;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = menu.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = menu.ID.ToString();
            }
        }

        protected void lvMenu_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                using (var db = new GeneralDataManager())
                {
                    ClearFields();
                    var objToUpdate = db.AdmissionDB.Menus.Find(Convert.ToInt64(e.CommandArgument.ToString()));
                    txtMenuName.Text = objToUpdate.Name;
                    txtMenuOrder.Text = objToUpdate.MenuOrder.ToString();
                    txtTier.Text = objToUpdate.Tier.ToString();
                    txtUrl.Text = objToUpdate.URL;
                    if(objToUpdate.ParentMenuID == null)
                    {
                        ddlParentMenu.SelectedValue = "-1";
                    }
                    else
                    {
                        ddlParentMenu.SelectedValue = objToUpdate.ParentMenuID.ToString();
                    }
                    chbxIsAdmin.Checked = objToUpdate.IsAdmin;

                    CurrentMenuID = objToUpdate.ID;
                    btnSave.Text = "Update";
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    using (var db = new GeneralDataManager())
                    {
                        var objToDelete = db.AdmissionDB.Menus.Find(Convert.ToInt64(e.CommandArgument.ToString()));
                        db.Delete<DAL.Menu>(objToDelete);
                        CurrentMenuID = 0;
                    }
                    lblMessage.Text = "Menu deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;
                    LoadListView();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to delete.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
        }

        protected void lvMenu_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvMenu_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void lvMenu_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            long menuId = -1;
            menuId = Convert.ToInt64(ddlMenuFilter.SelectedValue);
            if (menuId > 0)
            {
                lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                LoadListView(menuId);
            }
            else
            {
                lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                LoadListView();
            }

        }
    }
}