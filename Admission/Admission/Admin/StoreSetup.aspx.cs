using Admission.App_Start;
using CommonUtility;
using DAL;
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
    public partial class StoreSetup : PageBase
    {

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        long cId = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                LoadListView();
            }
        }

        private Store StoreObj { get; set; }

        private int CurrentStoreId
        {
            get
            {
                if (ViewState["CurrentStoreId"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["CurrentStoreId"].ToString());
            }
            set
            {
                ViewState["CurrentStoreId"] = value;
            }
        }

        private void ClearFields()
        {
            txtCancelledUrl.Text = string.Empty;
            txtFailedUrl.Text = string.Empty;
            txtStoreId.Text = string.Empty;
            txtStoreName.Text = string.Empty;
            txtStorePassword.Text = string.Empty;
            txtSuccessUrl.Text = string.Empty;
            txtUrl.Text = string.Empty;
            ckbxIsActive.Checked = false;
            ckbxIsMultiple.Checked = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = -1;
            DAL.Store obj = new Store();
            try
            {
                if (CurrentStoreId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        StoreObj = db.AdmissionDB.Stores.Find(Convert.ToInt32(CurrentStoreId));
                    }
                }

                if (StoreObj != null && CurrentStoreId > 0) //update
                {
                    obj.CancelledUrl = txtCancelledUrl.Text;
                    obj.FailedUrl = txtFailedUrl.Text;
                    obj.IsActive = ckbxIsActive.Checked;
                    obj.IsMultipleAllowed = ckbxIsMultiple.Checked;
                    obj.StoreId = Encrypt.EncryptString(txtStoreId.Text);
                    obj.StoreName = txtStoreName.Text;
                    obj.StorePass = Encrypt.EncryptString(txtStorePassword.Text);
                    obj.SuccessUrl = txtSuccessUrl.Text;
                    obj.URL = Encrypt.EncryptString(txtUrl.Text);

                    obj.ID = StoreObj.ID;

                    obj.Attribute1 = null;
                    obj.Attribute2 = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                    obj.CreatedBy = StoreObj.CreatedBy;
                    obj.DateCreated = StoreObj.DateCreated;
                    obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                    obj.DateModified = DateTime.Now;

                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.Store>(obj);
                    }
                    lblMessage.Text = "Store updated successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;

                    StoreObj = null;
                    CurrentStoreId = 0;
                }
                else if (StoreObj == null && CurrentStoreId == 0) //create new
                {
                    obj.CancelledUrl = txtCancelledUrl.Text;
                    obj.FailedUrl = txtFailedUrl.Text;
                    obj.IsActive = ckbxIsActive.Checked;
                    obj.IsMultipleAllowed = ckbxIsMultiple.Checked;
                    obj.StoreId = Encrypt.EncryptString(txtStoreId.Text);
                    obj.StoreName = txtStoreName.Text;
                    obj.StorePass = Encrypt.EncryptString(txtStorePassword.Text);
                    obj.SuccessUrl = txtSuccessUrl.Text;
                    obj.URL = Encrypt.EncryptString(txtUrl.Text);

                    obj.Attribute1 = null;
                    obj.Attribute2 = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

                    obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                    obj.DateCreated = DateTime.Now;
                    obj.ModifiedBy = null;
                    obj.DateModified = null;

                    using (var db = new OfficeDataManager())
                    {
                        db.Insert<DAL.Store>(obj);
                        id = obj.ID;
                    }
                    if (id > 0)
                    {
                        lblMessage.Text = "Store saved successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                    StoreObj = null;
                    CurrentStoreId = 0;
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
            LoadListView();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            StoreObj = null;
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.Store> list = db.GetAllStores().ToList();
                if (list != null)
                {
                    lvStores.DataSource = list.ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvStores.DataSource = null;
                    lblCount.Text = list.Count().ToString();
                }
                lvStores.DataBind();
            }
        }

        protected void lvStores_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Store store = (DAL.Store)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblStoreName = (Label)currentItem.FindControl("lblStoreName");
                Label lblStoreId = (Label)currentItem.FindControl("lblStoreId");
                Label lblUrl = (Label)currentItem.FindControl("lblUrl");
                Label lblSuccessUrl = (Label)currentItem.FindControl("lblSuccessUrl");
                Label lblFailedUrl = (Label)currentItem.FindControl("lblFailedUrl");
                Label lblCancelledUrl = (Label)currentItem.FindControl("lblCancelledUrl");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");
                Label lblIsMultiple = (Label)currentItem.FindControl("lblIsMultiple"); 
                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                lblStoreName.Text = store.StoreName;
                lblStoreId.Text = Decrypt.DecryptString(store.StoreId);
                lblUrl.Text = Decrypt.DecryptString(store.URL);
                lblSuccessUrl.Text = store.SuccessUrl;
                lblFailedUrl.Text = store.FailedUrl;
                lblCancelledUrl.Text = store.CancelledUrl;
                if (store.IsActive == true)
                {
                    lblIsActive.Text = "YES";
                    lblIsActive.ForeColor = Color.Green;
                }
                else
                {
                    lblIsActive.Text = "NO";
                    lblIsActive.ForeColor = Color.Crimson;
                }
                if (store.IsMultipleAllowed == true)
                {
                    lblIsMultiple.Text = "YES";
                    lblIsMultiple.ForeColor = Color.Green;
                }
                else
                {
                    lblIsMultiple.Text = "NO";
                    lblIsMultiple.ForeColor = Color.Crimson;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = store.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = store.ID.ToString();
            }
        }

        protected void lvStores_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                lblMessage.Text = string.Empty;
                messagePanel.CssClass = string.Empty;
                messagePanel.Visible = false;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.Stores.Find(Convert.ToInt32(e.CommandArgument));
                        db.Delete<DAL.Store>(objToDelete);
                        CurrentStoreId = 0;
                        StoreObj = null;
                    }
                    LoadListView();
                    lblMessage.Text = "Store deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;

                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to delete.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
            else if (e.CommandName == "Update")
            {
                using (var db = new OfficeDataManager())
                {
                    StoreObj = db.AdmissionDB.Stores.Find(Convert.ToInt32(e.CommandArgument));
                    ClearFields();
                    if (StoreObj != null)
                    {
                        CurrentStoreId = StoreObj.ID;

                        txtCancelledUrl.Text = StoreObj.CancelledUrl;
                        txtFailedUrl.Text = StoreObj.FailedUrl;
                        txtStoreId.Text = Decrypt.DecryptString(StoreObj.StoreId);
                        txtStoreName.Text = StoreObj.StoreName;
                        txtStorePassword.Text = Decrypt.DecryptString(StoreObj.StorePass);
                        txtSuccessUrl.Text = StoreObj.SuccessUrl;
                        txtUrl.Text = Decrypt.DecryptString(StoreObj.URL);
                        if (StoreObj.IsActive == true)
                        {
                            ckbxIsActive.Checked = true;
                        }
                        else
                        {
                            ckbxIsActive.Checked = false;
                        }
                        if (StoreObj.IsMultipleAllowed == true)
                        {
                            ckbxIsMultiple.Checked = true;
                        }
                        else
                        {
                            ckbxIsMultiple.Checked = false;
                        }
                        btnSave.Text = "Update";
                    }
                }
            }
        }

        protected void lvStores_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }

        protected void lvStores_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
        }
    }
}