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

namespace Admission.Admission.Admin
{
    public partial class Quota : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        bool isSuper = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            try
            {
                isSuper = Authorize.AuthenticateSuperAdmin(uId);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error getting user information. ";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }

            if (!IsPostBack)
            {
                LoadListView();
            }
        }

        private int CurrentQuotaID
        {
            get
            {
                if (ViewState["CurrentQuotaID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["CurrentQuotaID"].ToString());
            }
            set
            {
                ViewState["CurrentQuotaID"] = value;
            }
        }

        private void ClearFields()
        {
            txtQuotaName.Text = null;
            txtRemarks.Text = null;
            ckbxIsActive.Checked = false;

            CurrentQuotaID = 0;
        }

        private void ClearMessage()
        {
            lblMessage.Text = string.Empty;
            messagePanel.CssClass = string.Empty;
            messagePanel.Visible = false;
        }

        private void LoadListView()
        {
            try
            {
                List<DAL.Quota> quotaList = null;
                using (var db = new GeneralDataManager())
                {
                    quotaList = db.GetAllQuotas();
                }
                if(quotaList != null)
                {
                    if(quotaList.Count() > 0)
                    {
                        lvQuotaList.DataSource = quotaList.OrderByDescending(c=>c.IsActive).ToList();
                        lblCount.Text = quotaList.Count.ToString();
                    }
                    else
                    {
                        lvQuotaList.DataSource = null;
                        lblCount.Text = "0";
                    }
                }
                else
                {
                    lvQuotaList.DataSource = null;
                    lblCount.Text = "0";
                }
                lvQuotaList.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error getting quota list. ";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ClearMessage();

            string logOldObject = string.Empty;
            string logNewObject = string.Empty;

            try
            {
                if(CurrentQuotaID > 0) //update
                {
                    DAL.Quota objectToUpdate = null;
                    try
                    {
                        using (var db = new GeneralDataManager())
                        {
                            objectToUpdate = db.GetQuotaById(CurrentQuotaID);

                            logOldObject = ObjectToString.ConvertToString(objectToUpdate);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting quota information. " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        return;
                    }

                    if(objectToUpdate != null)
                    {

                        objectToUpdate.QuotaName = txtQuotaName.Text;
                        if (!string.IsNullOrEmpty(txtRemarks.Text.Trim()))
                        {
                            objectToUpdate.Remarks = txtRemarks.Text;
                        }
                        else
                        {
                            objectToUpdate.Remarks = null;
                        }
                        objectToUpdate.IsActive = ckbxIsActive.Checked;

                        objectToUpdate.DateModified = DateTime.Now;
                        objectToUpdate.ModifiedBy = uId;

                        try
                        {
                            using (var db = new GeneralDataManager())
                            {
                                db.Update<DAL.Quota>(objectToUpdate);

                                logNewObject = ObjectToString.ConvertToString(objectToUpdate);
                            }
                            lblMessage.Text = "Quota Updated.";
                            messagePanel.CssClass = "alert alert-success";
                            messagePanel.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error updating quota. " + ex.Message + "; " + ex.InnerException.Message;
                            messagePanel.CssClass = "alert alert-danger";
                            messagePanel.Visible = true;
                            return;
                        }

                        #region LOG INSERT

                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "btnSave_Click";
                        dLog.PageName = "Quota.aspx";
                        dLog.OldData = logOldObject;
                        dLog.NewData = logNewObject;
                        dLog.UserId = uId;
                        dLog.SessionInformation = "SU-ID: " + SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; UserRole: " +
                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                        dLog.DateCreated = DateTime.Now;
                        dLog.IpAddress = null;
                        dLog.DateTime = DateTime.Now;
                        dLog.HostName = Request.UserHostAddress + Request.UserHostName;
                        LogWriter.DataLogWriter(dLog);

                        #endregion

                        btnSave.Text = "Save";
                        ClearFields();
                        CurrentQuotaID = 0;
                    }
                }
                else if(CurrentQuotaID == 0) //create new
                {
                    ClearMessage(); 

                    DAL.Quota quota = new DAL.Quota();

                    quota.QuotaName = txtQuotaName.Text;
                    if (!string.IsNullOrEmpty(txtRemarks.Text.Trim()))
                    {
                        quota.Remarks = txtRemarks.Text;
                    }
                    else
                    {
                        quota.Remarks = null;
                    }
                    quota.IsActive = ckbxIsActive.Checked;

                    quota.CreatedBy = uId;
                    quota.DateCreated = DateTime.Now;

                    quota.ModifiedBy = null;
                    quota.DateModified = null;

                    try
                    {
                        using(var db = new GeneralDataManager())
                        {
                            db.Insert<DAL.Quota>(quota);

                            logNewObject = ObjectToString.ConvertToString(quota);
                        }

                        lblMessage.Text = "Quota saved. ";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error saving quota. " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        return;
                    }

                    #region LOG INSERT

                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "btnSave_Click";
                    dLog.PageName = "Quota.aspx";
                    dLog.OldData = null;
                    dLog.NewData = logNewObject;
                    dLog.UserId = uId;
                    dLog.SessionInformation = "SU-ID: " + SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; UserRole: " +
                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                    dLog.DateCreated = DateTime.Now;
                    dLog.IpAddress = null;
                    dLog.DateTime = DateTime.Now;
                    dLog.HostName = Request.UserHostAddress + Request.UserHostName;
                    LogWriter.DataLogWriter(dLog);

                    #endregion

                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error saving/updating data. " + ex.Message + "; " + ex.InnerException.Message;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
                return;
            }
            LoadListView();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void lvQuotaList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Quota quota = (DAL.Quota)((ListViewDataItem)(e.Item)).DataItem;

                Label lblQuota = (Label)currentItem.FindControl("lblQuota");
                Label lblRemarks = (Label)currentItem.FindControl("lblRemarks");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblQuota.Text = quota.QuotaName;
                lblRemarks.Text = quota.Remarks;

                if (quota.IsActive == true)
                {
                    lblIsActive.Text = "✔️";
                    lblIsActive.ForeColor = Color.Green;
                }
                else
                {
                    lblIsActive.Text = "❌";
                    lblIsActive.ForeColor = Color.Crimson;
                }

                if(isSuper == true)
                {
                    lnkDelete.Visible = true;
                }
                else
                {
                    lnkDelete.Visible = false;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = quota.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = quota.ID.ToString();
            }
        }

        protected void lvQuotaList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                ClearMessage();
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.Quotas.Find(Convert.ToInt32(e.CommandArgument));
                        db.Delete<DAL.Quota>(objToDelete);
                        CurrentQuotaID = 0;
                    }
                    LoadListView();
                    lblMessage.Text = "Quota deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to delete. " + ex.Message + "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
            else if (e.CommandName == "Update")
            {
                ClearMessage();

                try
                {
                    DAL.Quota obj = null;
                    using (var db = new GeneralDataManager())
                    {
                        obj = db.GetQuotaById(Convert.ToInt32(e.CommandArgument));
                    }
                    if(obj != null)
                    {
                        txtQuotaName.Text = obj.QuotaName;
                        txtRemarks.Text = obj.Remarks;
                        ckbxIsActive.Checked = Convert.ToBoolean(obj.IsActive);

                        CurrentQuotaID = obj.ID;

                        btnSave.Text = "Update";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to get quota. " + ex.Message + "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
        }

        protected void lvQuotaList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {}

        protected void lvQuotaList_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {}
    }
}