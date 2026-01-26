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
    public partial class GeneralInstructionSetup : PageBase
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

        public long CurrentGenInsID
        {
            get
            {
                if (ViewState["CurrentGenInsID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentGenInsID"].ToString());
            }
            set
            {
                ViewState["CurrentGenInsID"] = value;
            }
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.GeneralInstruction> list = db.AdmissionDB.GeneralInstructions
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
                    if (CurrentGenInsID > 0)
                    {
                        DAL.GeneralInstruction objToUpdate = null;

                        using (var db = new OfficeDataManager())
                        {
                            objToUpdate = db.AdmissionDB.GeneralInstructions.Find(CurrentGenInsID);
                        }

                        if (objToUpdate != null)
                        {
                            objToUpdate.Title = txtGETitle.Text;
                            objToUpdate.Description = txtGEDesc.Text;
                            objToUpdate.Title_BEN = txtGETitleBen.Text;
                            objToUpdate.Description_BEN = txtGEDescBen.Text;

                            objToUpdate.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                            objToUpdate.DateModified = DateTime.Now;

                            using(var db = new OfficeDataManager())
                            {
                                db.Update<DAL.GeneralInstruction>(objToUpdate);

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
                            DAL.GeneralInstruction genIns = new DAL.GeneralInstruction();

                            genIns.Title = txtGETitle.Text;
                            genIns.Description = txtGEDesc.Text;
                            genIns.Title_BEN = txtGETitleBen.Text;
                            genIns.Description_BEN = txtGEDescBen.Text;
                            genIns.IsActive = true;

                            genIns.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                            genIns.DateCreated = DateTime.Now;

                            db.Insert<DAL.GeneralInstruction>(genIns);

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
                DAL.GeneralInstruction genIns = (DAL.GeneralInstruction)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblTitle = (Label)currentItem.FindControl("lblTitle");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblTitle.Text = genIns.Title;

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = genIns.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = genIns.ID.ToString();
            }
        }

        protected void lvData_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    using (var db = new GeneralDataManager())
                    {
                        var objectToDelete = db.AdmissionDB.GeneralInstructions.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.GeneralInstruction>(objectToDelete);
                        CurrentGenInsID = 0;
                    }
                    lblMessage.Text = "General Instruction deleted successfully.";
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
                using (var db = new GeneralDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.GeneralInstructions.Find(Convert.ToInt64(e.CommandArgument));
                    txtGETitle.Text = objectToUpdate.Title;
                    txtGEDesc.Text = objectToUpdate.Description;
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
                    CurrentGenInsID = objectToUpdate.ID;
                }
            }
            //LoadListView();
        }

        protected void lvData_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        { }

        protected void lvData_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        { }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            LoadListView();
        }
    }
}