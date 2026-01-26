using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class NoticeForHomeImportant : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
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
                CurrentNoticeID = 0;
                btnSave.Text = "Create Notice";
                LoadListView();
            }
        }

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }
        private long CurrentNoticeID
        {
            get
            {
                if (ViewState["CurrentNoticeID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentNoticeID"].ToString());
            }
            set
            {
                ViewState["CurrentNoticeID"] = value;
            }
        }

        private void ClearFields()
        {
            txtNoticeTitle.Text = string.Empty;
            txtNoticeDetails.Text = string.Empty;
            ckbxIsActive.Checked = false;

            CurrentNoticeID = 0;
            btnSave.Text = "Create Notice";
        }


        private void LoadListView()
        {
            List<DAL.Notice> list = null;
            using (var db = new OfficeDataManager())
            {
                list = db.AdmissionDB.Notices.Where(x=> x.NoticeType == 2).OrderByDescending(a => a.ID).ToList();
                
            }

            if (list != null && list.Count > 0)
            {
                lvNotices.DataSource = list;
                lblCount.Text = list.Count().ToString();
            }
            else
            {
                lvNotices.DataSource = null;
                lblCount.Text = "0";
            }
            lvNotices.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            if (!string.IsNullOrEmpty(txtNoticeTitle.Text.Trim())) //&& !string.IsNullOrEmpty(txtNoticeDetails.Text)
            {
                long id = -1;
                DAL.Notice obj = new DAL.Notice();

                obj.NoticeTitle = txtNoticeTitle.Text.Trim();
                obj.NoticeDetails = txtNoticeDetails.Text;
                obj.NoticeDate = DateTime.Now;
                //obj.NoticeEndDate = DateTime.ParseExact(txtNoticeEndDate.Text, "dd/MM/yyyy", null);
                obj.IsActive = ckbxIsActive.Checked;
                obj.NoticeType = 2;
                obj.Remarks = "Important Notice";

                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = uId;
                obj.ID = CurrentNoticeID;

                try
                {
                    if (obj.ID > 0) //update
                    {
                        DAL.Notice objectToUpdate = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                objectToUpdate = db.AdmissionDB.Notices.Find(obj.ID);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageView("Error: " + ex.Message, "fail");
                            return;
                        }

                        if (objectToUpdate != null)
                        {
                            objectToUpdate.NoticeDetails = txtNoticeDetails.Text;                            
                            objectToUpdate.IsActive = ckbxIsActive.Checked;

                            objectToUpdate.ModifiedBy = uId;
                            objectToUpdate.ModifiedDate = DateTime.Now;
                        }

                        
                        using (var db = new GeneralDataManager())
                        {
                            db.Update<DAL.Notice>(objectToUpdate);
                        }


                        MessageView("Notice updated Successfully.", "success");

                    }
                    else //create new
                    {
                       
                        using (var db = new GeneralDataManager())
                        {
                            db.Insert<DAL.Notice>(obj);
                            id = obj.ID;
                            
                        }
                        if (id > 0)
                        {
                            MessageView("Notice saved Successfully.", "success");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageView("Unable to save/update notice. Error: " + ex.Message, "fail");
                    return;
                }


                btnSave.Text = "Create Notice";

                ClearFields();
                CurrentNoticeID = 0;
                LoadListView();
            }
            else
            {
                MessageView("Please Fill Up All Required Fields!", "fail");
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void lvNotices_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Notice notice = (DAL.Notice)((ListViewDataItem)(e.Item)).DataItem;
                
                Label lblNoticeTitle = (Label)currentItem.FindControl("lblNoticeTitle");
                Label lblNoticeDetails = (Label)currentItem.FindControl("lblNoticeDetails");
                Label lblNoticeDate = (Label)currentItem.FindControl("lblNoticeDate");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblNoticeTitle.Text = notice.NoticeTitle;
                lblNoticeDetails.Text = notice.NoticeDetails;
                lblNoticeDate.Text = Convert.ToDateTime(notice.NoticeDate).ToString("dd/MM/yyyy hh:mm tt");
                if (notice.IsActive == true)
                {
                    lblIsActive.Text = "YES";
                    lblIsActive.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblIsActive.Text = "NO";
                    lblIsActive.ForeColor = System.Drawing.Color.Crimson;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = notice.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = notice.ID.ToString();

                //lnkDetails.CommandName = "Details";
                //lnkDetails.CommandArgument = notice.ID.ToString();
            }
        }

        protected void lvNotices_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                MessageView("", "clear");

                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.Notices.Find(Convert.ToInt32(e.CommandArgument));
                        db.Delete<DAL.Notice>(objToDelete);
                        CurrentNoticeID = 0;
                    }

                    LoadListView();

                    MessageView("Notice deleted successfully.", "success");
                }
                catch (Exception ex)
                {
                    MessageView("Unable to delete !!", "fail");
                }
            }
            else if (e.CommandName == "Update")
            {
                MessageView("", "clear");

                using (var db = new OfficeDataManager())
                {
                    var notice = db.AdmissionDB.Notices.Find(Convert.ToInt32(e.CommandArgument));
                    ClearFields();
                    if (notice != null && notice.ID > 0)
                    {
                        CurrentNoticeID = notice.ID;

                        txtNoticeTitle.Text = notice.NoticeTitle;
                        txtNoticeDetails.Text = notice.NoticeDetails;

                        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "OpenCollapse", "assignDataInCollapse(" + notice + ")", true);

                        if (notice.IsActive == true)
                        {
                            ckbxIsActive.Checked = true;
                        }
                        else
                        {
                            ckbxIsActive.Checked = false;
                        }
                        btnSave.Text = "Update Notice";
                    }
                }

                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "OpenCollapse", "openCollapse()", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenCollapse", "openCollapse()", true);
            }
        }

        protected void lvNotices_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

        protected void lvNotices_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }


    }
}