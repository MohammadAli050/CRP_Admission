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
    public partial class Notice : PageBase
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
                LoadListView();
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
            txtNoticeDate.Text = string.Empty;
            txtNoticeDetails.Text = string.Empty;
            txtNoticeTitle.Text = string.Empty;
            //txtExternalUrl.Text = string.Empty;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            ckbxIsActive.Checked = false;
        }

        private void ClearMessage()
        {
            lblMessage.Text = string.Empty;
            messagePanel.CssClass = string.Empty;
            messagePanel.Visible = false;
        }

        private void LoadListView()
        {
            List<DAL.Notice> list = null;
            using (var db = new OfficeDataManager())
            {
                list = db.AdmissionDB.Notices.Where(x => x.NoticeType == 1).OrderByDescending(a => a.NoticeDate).ToList();

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
            long id = -1;
            DAL.Notice obj = new DAL.Notice();

            obj.NoticeTitle = txtNoticeTitle.Text;
            obj.NoticeDetails = txtNoticeDetails.Text;
            obj.NoticeDate = DateTime.ParseExact(txtNoticeDate.Text, "dd/MM/yyyy", null);
            obj.IsActive = ckbxIsActive.Checked;
            obj.ToDate = null;
            obj.FromDate = null;
            obj.Attachment = null;
            //obj.EID = txtExternalUrl.Text;

            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
            obj.ModifiedDate = null;
            obj.ModifiedBy = null;
            obj.NoticeType = 1;
            obj.Remarks = "Notice";

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
                        messagePanel.Visible = true;
                        lblMessage.Text = "Error: " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                    }

                    if(objectToUpdate != null)
                    {
                        objectToUpdate.NoticeTitle = txtNoticeTitle.Text;
                        objectToUpdate.NoticeDetails = txtNoticeDetails.Text;
                        objectToUpdate.NoticeDate = DateTime.ParseExact(txtNoticeDate.Text, "dd/MM/yyyy", null);
                        objectToUpdate.IsActive = ckbxIsActive.Checked;
                        objectToUpdate.ToDate = null;
                        objectToUpdate.FromDate = null;
                        //objectToUpdate.EID = txtExternalUrl.Text;
                        objectToUpdate.Attachment = null;

                        objectToUpdate.ModifiedDate = DateTime.Now;
                        objectToUpdate.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK;
                    }


                    #region FileUpload_Attachment
                    try
                    {
                        if (fuAttachment.HasFile)
                        {
                            string attachedFileName = fuAttachment.PostedFile.FileName;
                            int contentLength = int.Parse(fuAttachment.PostedFile.ContentLength.ToString());
                            string filePath = "~/Upload/Notices/";
                            string tempFilePath = "~/Upload/TEMP/";
                            string tempFileName = "_temp_" + attachedFileName;

                            if (contentLength > 0)//if file size greater than zero
                            {

                                if (File.Exists(Server.MapPath(filePath + attachedFileName)))
                                {
                                    File.Move(Server.MapPath(filePath + attachedFileName), Server.MapPath(tempFilePath + tempFileName));
                                    File.Delete(Server.MapPath(filePath + attachedFileName));
                                    fuAttachment.SaveAs(Server.MapPath(filePath + attachedFileName));

                                    objectToUpdate.Attachment = filePath + attachedFileName;

                                    if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
                                    {
                                        File.Delete(Server.MapPath(tempFilePath + tempFileName));
                                    }
                                }
                                else //if file does not exist.
                                {
                                    fuAttachment.SaveAs(Server.MapPath(filePath + attachedFileName));

                                    objectToUpdate.Attachment = filePath + attachedFileName;
                                }

                            }
                        }
                        else //if no file selected as attachment
                        {
                            objectToUpdate.Attachment = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        messagePanel.Visible = true;
                        lblMessage.Text = "Error: " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                    }
                    #endregion
                    using (var db = new GeneralDataManager())
                    {
                        db.Update<DAL.Notice>(objectToUpdate);
                    }
                    

                    lblMessage.Text = "Notice updated Successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;
                }
                else //create new
                {
                    #region FileUpload_Attachment
                    try
                    {
                        if (fuAttachment.HasFile)
                        {
                            string attachedFileName = fuAttachment.PostedFile.FileName;
                            int contentLength = int.Parse(fuAttachment.PostedFile.ContentLength.ToString());
                            string filePath = "~/Upload/Notices/";
                            string tempFilePath = "~/Upload/TEMP/";
                            string tempFileName = "_temp_" + attachedFileName;

                            if (contentLength > 0)//if file size greater than zero
                            {

                                if (File.Exists(Server.MapPath(filePath + attachedFileName)))
                                {
                                    File.Move(Server.MapPath(filePath + attachedFileName), Server.MapPath(tempFilePath + tempFileName));
                                    File.Delete(Server.MapPath(filePath + attachedFileName));
                                    fuAttachment.SaveAs(Server.MapPath(filePath + attachedFileName));

                                    obj.Attachment = filePath + attachedFileName;

                                    if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
                                    {
                                        File.Delete(Server.MapPath(tempFilePath + tempFileName));
                                    }
                                }
                                else //if file does not exist.
                                {
                                    fuAttachment.SaveAs(Server.MapPath(filePath + attachedFileName));

                                    obj.Attachment = filePath + attachedFileName;
                                }

                            }
                        }
                        else //if no file selected as attachment
                        {
                            obj.Attachment = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        messagePanel.Visible = true;
                        lblMessage.Text = "Error: " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                    }
                    #endregion
                    using (var db = new GeneralDataManager())
                    {
                        db.Insert<DAL.Notice>(obj);
                        id = obj.ID;
                        //List<DAL.SP_ProgramsGetAllFromUCAM_Result> obj1 = new List<SP_ProgramsGetAllFromUCAM_Result>();
                        //obj1 = db.AdmissionDB.SP_ProgramsGetAllFromUCAM().ToList();
                    }
                    if (id > 0)
                    {
                        lblMessage.Text = "Notice saved successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unable to save/update notice. " + ex.Message + "; " + ex.InnerException.Message;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
            btnSave.Text = "Save";
            ClearMessage();
            ClearFields();
            CurrentNoticeID = 0;
            LoadListView();
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
                Label lblNoticeDate = (Label)currentItem.FindControl("lblNoticeDate");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");
                //Label lblIsAttachmentAvailable = (Label)currentItem.FindControl("lblIsAttachmentAvailable");
                Label lblIsExternalUrlAvailable = (Label)currentItem.FindControl("lblIsExternalUrlAvailable");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                
                lblNoticeTitle.Text = notice.NoticeTitle;
                lblNoticeDate.Text = Convert.ToDateTime(notice.NoticeDate).ToString("dd/MM/yyyy");
                if (notice.IsActive == true)
                {
                    lblIsActive.Text = "✓";
                    lblIsActive.ForeColor = Color.Green;
                }
                else
                {
                    lblIsActive.Text = "✕";
                    lblIsActive.ForeColor = Color.Crimson;
                }
                //if(lblIsAttachmentAvailable != null)
                //{
                //    lblIsAttachmentAvailable.Text = "Yes";
                //    lblIsAttachmentAvailable.ForeColor = Color.Green;
                //}
                //else
                //{
                //    lblIsAttachmentAvailable.Text = "No";
                //    lblIsAttachmentAvailable.ForeColor = Color.Crimson;
                //}
                lblIsExternalUrlAvailable.Text = notice.Attachment;
                if (!string.IsNullOrEmpty(lblIsExternalUrlAvailable.Text))
                {
                    lblIsExternalUrlAvailable.Text = "Yes";
                    lblIsExternalUrlAvailable.ForeColor = Color.Green;
                }
                else
                {
                    lblIsExternalUrlAvailable.Text = "No";
                    lblIsExternalUrlAvailable.ForeColor = Color.Crimson;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = notice.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = notice.ID.ToString();

            }
        }

        protected void lvNotices_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                ClearMessage();
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.Notices.Find(Convert.ToInt32(e.CommandArgument));

                        if (objToDelete != null)
                        {
                            try
                            {
                                if (objToDelete.Attachment != null &&
                                File.Exists(Server.MapPath(objToDelete.Attachment)))
                                {
                                    File.Delete(Server.MapPath(objToDelete.Attachment));
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }

                            db.Delete<DAL.Notice>(objToDelete);
                            CurrentNoticeID = 0;
                        }
                        
                    }



                    LoadListView();
                    lblMessage.Text = "Notice deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to delete.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                ClearFields();
            }
            else if (e.CommandName == "Update")
            {
                ClearMessage();
                using (var db = new OfficeDataManager())
                {
                    var notice = db.AdmissionDB.Notices.Find(Convert.ToInt32(e.CommandArgument));
                    ClearFields();
                    if (notice != null && notice.ID > 0)
                    {
                        CurrentNoticeID = notice.ID;

                        txtNoticeTitle.Text = notice.NoticeTitle;
                        txtNoticeDetails.Text = notice.NoticeDetails;
                        txtNoticeDate.Text = Convert.ToDateTime(notice.NoticeDate).ToString("dd/MM/yyyy");
                        //txtExamDate.Text = Convert.ToDateTime(AdmissionSetupObj.ExamDate).ToString("dd/MM/yyyy");
                        //txtExternalUrl.Text = notice.EID;

                        if (notice.IsActive == true)
                        {
                            ckbxIsActive.Checked = true;
                        }
                        else
                        {
                            ckbxIsActive.Checked = false;
                        }
                        btnSave.Text = "Update";
                    }
                }
            }
        }

        protected void lvNotices_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

        protected void lvNotices_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }
    }
}