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
    public partial class HowToApplySetup : PageBase
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
                messagePanel.Visible = false;
                lblMessage.Text = string.Empty;
                LoadListView();
            }
        }

        public long CurrentHowToApplyID
        {
            get
            {
                if (ViewState["CurrentHowToApplyID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentHowToApplyID"].ToString());
            }
            set
            {
                ViewState["CurrentHowToApplyID"] = value;
            }
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.HowToApply> list = db.AdmissionDB.HowToApplies
                    .OrderByDescending(c => c.ID).ToList();

                if (list.Count() > 0)
                {
                    lvData.DataSource = list;
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvData.DataSource = null;
                    lblCount.Text = "0";
                }
            }
            lvData.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string title = txtGETitle.Text;
            string desc = txtGEDesc.Text;

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(desc))
            {
                try
                {
                    if (CurrentHowToApplyID > 0)
                    {
                        DAL.HowToApply objToUpdate = null;

                        using (var db = new OfficeDataManager())
                        {
                            objToUpdate = db.AdmissionDB.HowToApplies.Find(CurrentHowToApplyID);
                        }

                        if (objToUpdate != null)
                        {
                            objToUpdate.Title_ENG = txtGETitle.Text;
                            objToUpdate.Description_ENG = txtGEDesc.Text;
                            objToUpdate.Title_BEN = txtGETitleBen.Text;
                            objToUpdate.Description_BEN = txtGEDescBen.Text;

                            objToUpdate.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                            objToUpdate.DateModified = DateTime.Now;

                            using (var db = new OfficeDataManager())
                            {
                                db.Update<DAL.HowToApply>(objToUpdate);

                                lblMessage.Text = "Updated successfully.";
                                messagePanel.CssClass = "alert alert-success";
                                messagePanel.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        using (var db = new OfficeDataManager())
                        {
                            DAL.HowToApply howToApply = new DAL.HowToApply();

                            howToApply.Title_ENG = txtGETitle.Text;
                            howToApply.Description_ENG = txtGEDesc.Text;
                            howToApply.Title_BEN = txtGETitleBen.Text;
                            howToApply.Description_BEN = txtGEDescBen.Text;
                            howToApply.IsActive = true;

                            howToApply.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                            howToApply.DateCreated = DateTime.Now;

                            db.Insert<DAL.HowToApply>(howToApply);

                            lblMessage.Text = "Saved successfully.";
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
            }
            LoadListView();
        }

        protected void lvData_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.HowToApply hta = (DAL.HowToApply)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblTitle = (Label)currentItem.FindControl("lblTitle");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblTitle.Text = hta.Title_ENG;

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = hta.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = hta.ID.ToString();
            }
        }

        protected void lvData_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objectToDelete = db.AdmissionDB.HowToApplies.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.HowToApply>(objectToDelete);
                        CurrentHowToApplyID = 0;
                    }
                    lblMessage.Text = "Deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to delete.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                LoadListView();
            }
            else if (e.CommandName == "Update")
            {
                btnSave.Text = "Update";
                using (var db = new OfficeDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.HowToApplies.Find(Convert.ToInt64(e.CommandArgument));
                    txtGETitle.Text = objectToUpdate.Title_ENG;
                    txtGEDesc.Text = objectToUpdate.Description_ENG;
                    txtGETitleBen.Text = objectToUpdate.Title_BEN;
                    txtGEDescBen.Text = objectToUpdate.Description_BEN;
                    //if (objectToUpdate.IsActive == true)
                    //{
                    //    ckbxIsActive.Checked = true;
                    //}
                    //else
                    //{
                    //    ckbxIsActive.Checked = false;
                    //}
                    CurrentHowToApplyID = objectToUpdate.ID;
                }
            }
        }

        protected void lvData_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {}

        protected void lvData_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {}

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            LoadListView();
        }
    }
}