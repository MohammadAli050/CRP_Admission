using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Admin
{
    public partial class Key : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadListView();
            }
        }

        public int CurrentKeyID
        {
            get
            {
                if (ViewState["CurrentKeyID"] == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(ViewState["CurrentKeyID"].ToString());
                }
            }
            set
            {
                ViewState["CurrentKeyID"] = value;
            }
        }

        public int CurrentKeyIDEdit
        {
            get
            {
                if (ViewState["CurrentKeyIDEdit"] == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(ViewState["CurrentKeyIDEdit"].ToString());
                }
            }
            set
            {
                ViewState["CurrentKeyIDEdit"] = value;
            }
        }

        #region LISTVIEW METHODS

        private void LoadListView()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.Key> keyList = new List<DAL.Key>();
                keyList = db.AdmissionDB.Keys.ToList();
                if (keyList.Any())
                {
                    lvKey.DataSource = keyList;
                    lvKey.DataBind();
                }
            }
        }

        protected void lvKey_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Key key = (DAL.Key)((ListViewDataItem)(e.Item)).DataItem;

                //lvRowCount += 1;
                Label lblID = (Label)currentItem.FindControl("lblID");
                Label lblKeyName = (Label)currentItem.FindControl("lblKeyName");
                Label lblRemarks = (Label)currentItem.FindControl("lblRemarks");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblID.Text = key.ID.ToString();
                lblKeyName.Text = key.KeyName;
                lblRemarks.Text = key.Remarks;

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = key.ID.ToString();

                lnkEdit.CommandName = "Edit";
                lnkEdit.CommandArgument = key.ID.ToString();
            }
        }

        protected void lvKey_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var key = new DAL.Key();
            if (e.CommandName == "Delete")
            {
                using (var db = new GeneralDataManager())
                {
                    var objectToDelete = db.AdmissionDB.Keys.Find(Convert.ToInt32(e.CommandArgument));
                    db.Delete<DAL.Key>(objectToDelete);
                    CurrentKeyID = 0;
                    LoadListView();

                }
            }
            else if (e.CommandName == "Update")
            {

            }
        }

        protected void lvKey_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void lvKey_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        #endregion 

        #region BUTTON METHODS

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DAL.Key keyObj = new DAL.Key();

            keyObj.KeyName = txtKeyName.Text;
            keyObj.Remarks = txtRemarks.Text;

            int keyID = -1;
            using (var db = new GeneralDataManager())
            {
                db.Insert<DAL.Key>(keyObj);
                keyID = keyObj.ID;
                if (keyID > 0)
                {
                    lblMessage.Text = "Key added successfully";
                    lblMessage.CssClass = "label label-success";
                }
                else
                {
                    lblMessage.Text = "Unsuccessful";
                    lblMessage.CssClass = "label label-danger";
                }
            }
            LoadListView();
        }

        #endregion
        
    }
}