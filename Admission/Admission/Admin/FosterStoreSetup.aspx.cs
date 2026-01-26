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
    public partial class FosterStoreSetup : PageBase
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

        private StoreFoster StoreObj { get; set; }

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
            //txtStoreId.Text = string.Empty;
            txtStoreName.Text = string.Empty;
            txtAccessCodeId.Text = string.Empty;
            txtSecurityKey.Text = string.Empty;
            txtShopId.Text = string.Empty;
            txtMerchantShortName.Text = string.Empty;
            //txtStorePassword.Text = string.Empty;

            txtUrl.Text = string.Empty;
            txtSuccessUrl.Text = string.Empty;
            txtFailedUrl.Text = string.Empty;
            txtCancelledUrl.Text = string.Empty;

            ckbxIsActive.Checked = false;
            ckbxIsMultiple.Checked = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = -1;
            //DAL.Store obj = new Store();
            
            try
            {
                if (CurrentStoreId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        StoreObj = db.AdmissionDB.StoreFosters.Where(x => x.ID == CurrentStoreId).FirstOrDefault(); //.Find(Convert.ToInt32(CurrentStoreId));
                    }
                }

                if (StoreObj != null && CurrentStoreId > 0) //update
                {
                    #region N/A
                    //obj.ID = StoreObj.ID;

                    //obj.CancelledUrl = txtCancelledUrl.Text;
                    //obj.FailedUrl = txtFailedUrl.Text;
                    //obj.IsActive = ckbxIsActive.Checked;
                    //obj.IsMultipleAllowed = ckbxIsMultiple.Checked;
                    //obj.StoreId = Encrypt.EncryptString(txtStoreId.Text);
                    //obj.StoreName = txtStoreName.Text;
                    //obj.StorePass = Encrypt.EncryptString(txtStorePassword.Text);
                    //obj.SuccessUrl = txtSuccessUrl.Text;
                    //obj.URL = Encrypt.EncryptString(txtUrl.Text);


                    //obj.Attribute1 = null;
                    //obj.Attribute2 = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                    //obj.CreatedBy = StoreObj.CreatedBy;
                    //obj.DateCreated = StoreObj.DateCreated;
                    //obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                    //obj.DateModified = DateTime.Now; 
                    #endregion

                    StoreObj.StoreName = txtStoreName.Text;
                    StoreObj.AccessCode = txtAccessCodeId.Text;
                    StoreObj.SecurityKey = txtSecurityKey.Text;
                    StoreObj.ShopId = txtShopId.Text;
                    StoreObj.MerchantShortName = txtMerchantShortName.Text;
                    StoreObj.Uri = txtUrl.Text; // "https://payment.fosterpayments.com/fosterpayments/receivemerchantpaymentrequestwsfc.php?";
                    StoreObj.IsActive = ckbxIsActive.Checked;
                    StoreObj.IsMultipleAllowed = ckbxIsMultiple.Checked;

                    StoreObj.SuccessUrl = txtSuccessUrl.Text; // "https://admission.bup.edu.bd/Admission/successurlfpg"; //txtSuccessUrl.Text;
                    StoreObj.FailUrl = txtFailedUrl.Text; // "https://admission.bup.edu.bd/Admission/failurlfpg"; //txtSuccessUrl.Text;
                    StoreObj.CancelUrl = txtCancelledUrl.Text; //"https://admission.bup.edu.bd/Admission/cancelurlfpg"; //txtSuccessUrl.Text;


                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.StoreFoster>(StoreObj);
                    }
                    lblMessage.Text = "Store Foster Updated Successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;

                    StoreObj = null;
                    CurrentStoreId = 0;
                }
                else if (StoreObj == null && CurrentStoreId == 0) //create new
                {
                    DAL.StoreFoster obj = new StoreFoster();

                    obj.StoreName = txtStoreName.Text;
                    obj.AccessCode = txtAccessCodeId.Text;
                    obj.SecurityKey = txtSecurityKey.Text;
                    obj.ShopId = txtShopId.Text;
                    obj.MerchantShortName = txtMerchantShortName.Text;
                    obj.Uri = txtUrl.Text; // "https://payment.fosterpayments.com/fosterpayments/receivemerchantpaymentrequestwsfc.php?";
                    obj.IsActive = ckbxIsActive.Checked;
                    obj.IsMultipleAllowed = ckbxIsMultiple.Checked;
                    //obj.StoreId = Encrypt.EncryptString(txtStoreId.Text);
                    //obj.StoreName = txtStoreName.Text;
                    //obj.StorePass = Encrypt.EncryptString(txtStorePassword.Text);

                    obj.SuccessUrl = txtSuccessUrl.Text; // "https://admission.bup.edu.bd/Admission/successurlfpg"; //txtSuccessUrl.Text;
                    obj.FailUrl = txtFailedUrl.Text; // "https://admission.bup.edu.bd/Admission/failurlfpg"; //txtSuccessUrl.Text;
                    obj.CancelUrl = txtCancelledUrl.Text; //"https://admission.bup.edu.bd/Admission/cancelurlfpg"; //txtSuccessUrl.Text;

                    //obj.URL = Encrypt.EncryptString(txtUrl.Text);

                    obj.Attribute1 = null;
                    obj.Attribute2 = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

                    obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                    obj.DateCreated = DateTime.Now;
                    obj.ModifiedBy = null;
                    obj.DateModified = null;

                    using (var db = new OfficeDataManager())
                    {
                        db.Insert<DAL.StoreFoster>(obj);
                        id = obj.ID;
                    }
                    if (id > 0)
                    {
                        lblMessage.Text = "Store Foster Saved Successfully.";
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
                List<DAL.StoreFoster> list = db.GetAllStoreFosters().OrderByDescending(x=> x.ID).ToList();
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
                DAL.StoreFoster store = (DAL.StoreFoster)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblStoreName = (Label)currentItem.FindControl("lblStoreName");
                Label lblShopId = (Label)currentItem.FindControl("lblShopId");
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
                lblShopId.Text = store.ShopId;
                lblUrl.Text = store.Uri;
                lblSuccessUrl.Text = store.SuccessUrl;
                lblFailedUrl.Text = store.FailUrl;
                lblCancelledUrl.Text = store.CancelUrl;

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

                //lnkDelete.CommandName = "Delete";
                //lnkDelete.CommandArgument = store.ID.ToString();
            }
        }

        protected void lvStores_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                //lblMessage.Text = string.Empty;
                //messagePanel.CssClass = string.Empty;
                //messagePanel.Visible = false;
                //try
                //{
                //    using (var db = new OfficeDataManager())
                //    {
                //        var objToDelete = db.AdmissionDB.Stores.Find(Convert.ToInt32(e.CommandArgument));
                //        db.Delete<DAL.Store>(objToDelete);
                //        CurrentStoreId = 0;
                //        StoreObj = null;
                //    }
                //    LoadListView();
                //    lblMessage.Text = "Store deleted successfully.";
                //    messagePanel.CssClass = "alert alert-warning";
                //    messagePanel.Visible = true;

                //}
                //catch (Exception ex)
                //{
                //    lblMessage.Text = "Unable to delete.";
                //    messagePanel.CssClass = "alert alert-danger";
                //    messagePanel.Visible = true;
                //}
            }
            else if (e.CommandName == "Update")
            {
                int storeId = Convert.ToInt32(e.CommandArgument);

                using (var db = new OfficeDataManager())
                {

                    StoreObj = db.AdmissionDB.StoreFosters.Where(x=> x.ID == storeId).FirstOrDefault();
                    ClearFields();
                    if (StoreObj != null)
                    {
                        CurrentStoreId = StoreObj.ID;

                        txtUrl.Text = StoreObj.Uri;
                        txtSuccessUrl.Text = StoreObj.SuccessUrl;
                        txtFailedUrl.Text = StoreObj.FailUrl;
                        txtCancelledUrl.Text = StoreObj.CancelUrl;

                        txtStoreName.Text = StoreObj.StoreName;
                        txtAccessCodeId.Text = StoreObj.AccessCode;


                        txtSecurityKey.Text = StoreObj.SecurityKey;
                        txtShopId.Text = StoreObj.ShopId;
                        txtMerchantShortName.Text = StoreObj.MerchantShortName;

                        ckbxIsActive.Checked = Convert.ToBoolean(StoreObj.IsActive);
                        ckbxIsMultiple.Checked = Convert.ToBoolean(StoreObj.IsMultipleAllowed);
                        
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