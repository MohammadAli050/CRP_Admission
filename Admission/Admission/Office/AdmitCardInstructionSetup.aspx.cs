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
    public partial class AdmitCardInstructionSetup : PageBase
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
                LoadDDL();
                CurrentNoticeID = 0;
                btnSave.Text = "Create Instruction";
                LoadListView();
            }
        }


        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.ToList(), "UnitName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
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
            txtNoticeDetails.Text = string.Empty;

            CurrentNoticeID = 0;
            btnSave.Text = "Create Notice";
        }


        private void LoadListView()
        {
            int admUnitId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);
            int acacalId = Convert.ToInt32(ddlSession.SelectedValue);

            List<DAL.AdmitCardInstruction> list = null;
            using (var db = new OfficeDataManager())
            {
                list = db.AdmissionDB.AdmitCardInstructions.Where(x => x.AcacalId == acacalId && (x.AdmissionUnitId == admUnitId || admUnitId == -1)).OrderByDescending(a => a.ID).ToList();

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
            int admUnitId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);
            int acacalId = Convert.ToInt32(ddlSession.SelectedValue);

            if (!string.IsNullOrEmpty(txtNoticeDetails.Text.Trim()) 
                && admUnitId > -1
                && acacalId > 0)
            {
                long id = -1;

                DAL.AdmitCardInstruction obj = new DAL.AdmitCardInstruction();

                obj.AdmissionUnitId = admUnitId;
                obj.AcacalId = acacalId;
                obj.InstructionDetails = txtNoticeDetails.Text;

                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = uId;
                obj.ID = CurrentNoticeID;

                try
                {
                    if (obj.ID > 0) //update
                    {
                        DAL.AdmitCardInstruction objectToUpdate = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                objectToUpdate = db.AdmissionDB.AdmitCardInstructions.Find(obj.ID);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageView("Error: " + ex.Message, "fail");
                            return;
                        }

                        if (objectToUpdate != null)
                        {
                            objectToUpdate.InstructionDetails = txtNoticeDetails.Text;

                            objectToUpdate.ModifiedBy = uId;
                            objectToUpdate.ModifiedDate = DateTime.Now;
                        }


                        using (var db = new GeneralDataManager())
                        {
                            db.Update<DAL.AdmitCardInstruction>(objectToUpdate);
                        }


                        MessageView("Instruction updated Successfully.", "success");

                    }
                    else //create new
                    {

                        using (var db = new GeneralDataManager())
                        {
                            db.Insert<DAL.AdmitCardInstruction>(obj);
                            id = obj.ID;

                        }
                        if (id > 0)
                        {
                            MessageView("Instruction saved Successfully.", "success");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageView("Unable to save/update Instruction. Error: " + ex.Message, "fail");
                    return;
                }


                btnSave.Text = "Create Instruction";

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
                DAL.AdmitCardInstruction notice = (DAL.AdmitCardInstruction)((ListViewDataItem)(e.Item)).DataItem;
                
                Label lblNoticeDetails = (Label)currentItem.FindControl("lblNoticeDetails");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");
                
                lblNoticeDetails.Text = notice.InstructionDetails;

                Label lblSchool = (Label)currentItem.FindControl("lblSchool");
                using (var db = new GeneralDataManager())
                {

                    DAL.AdmissionUnit admunit = db.AdmissionDB.AdmissionUnits.Where(x => x.ID == notice.AdmissionUnitId).FirstOrDefault();
                    if (admunit != null )
                    {
                        lblSchool.Text = admunit.UnitName;
                    }
                    else
                    {
                        lblSchool.Text = "";
                    }

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
                        var objToDelete = db.AdmissionDB.AdmitCardInstructions.Find(Convert.ToInt32(e.CommandArgument));
                        db.Delete<DAL.AdmitCardInstruction>(objToDelete);
                        CurrentNoticeID = 0;
                    }

                    LoadListView();

                    MessageView("Instruction deleted successfully.", "success");
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
                    var notice = db.AdmissionDB.AdmitCardInstructions.Find(Convert.ToInt32(e.CommandArgument));
                    ClearFields();
                    if (notice != null && notice.ID > 0)
                    {
                        CurrentNoticeID = notice.ID;
                        ddlAdmissionUnit.SelectedValue = notice.AdmissionUnitId.ToString();
                        txtNoticeDetails.Text = notice.InstructionDetails;
                        
                        btnSave.Text = "Update Instruction";
                    }
                }

                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "OpenCollapse", "openCollapse()", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenCollapse", "openCollapse()", true);
            }
        }

        protected void lvNotices_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

        protected void lvNotices_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }

        protected void ddlAdmissionUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadListView();
        }
    }
}