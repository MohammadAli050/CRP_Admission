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

namespace Admission.Admission
{
    public partial class AdmissionUnitPrograms : PageBase
    {  //DAL.AdmissionUnitProgram admissionUnitProg = null;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); 
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
                LoadListView();
            }
        }

        private long CurrentAdmissionUnitProgramID
        {
            get
            {
                if (ViewState["CurrentAdmissionUnitProgramID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentAdmissionUnitProgramID"].ToString());
            }
            set
            {
                ViewState["CurrentAdmissionUnitProgramID"] = value;
            }
        }

        private void ClearFields()
        {
            ddlAdmissionUnit.SelectedIndex = -1;
            ddlProgram.SelectedIndex = -1;
            ddlEducationCategory.SelectedIndex = -1;
            ddlSession.SelectedIndex = -1;
            ddlBatch.SelectedIndex = -1;
            ckbxIsActive.Checked = false;

        }

        private void LoadDDL()
        {
            try
            {
                using (var db = new GeneralDataManager())
                {
                    DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.OrderBy(a => a.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);

                    List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();
                    DDLHelper.Bind<DAL.SPProgramsGetAllFromUCAM_Result>(ddlProgram, programs.OrderBy(a => a.DetailName).ToList(), "DetailNShortName", "ProgramID", EnumCollection.ListItemType.Program);

                    DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.IsActive==true).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.EducationCategory);
                }

                ddlSession.Items.Clear();
                ddlSession.Items.Add(new ListItem("N/A", "-1"));

                ddlBatch.Items.Clear();
                ddlBatch.Items.Add(new ListItem("N/A", "-1"));
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSessionDDL(int programID)
        {
            if (programID > 0)
            {
                using (var db = new GeneralDataManager())
                {
                    var sessions = db.AdmissionDB.SPAcademicCalendarGetAllByProgram(programID).ToList();
                    sessions = sessions.OrderByDescending(z => z.AcademicCalenderID).ToList();
                    DDLHelper.Bind<DAL.SPAcademicCalendarGetAllByProgram_Result>(ddlSession, sessions, "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
                }
            }
            else
            {
                ddlSession.Items.Clear();
                ddlSession.Items.Add(new ListItem("N/A", "-1"));
            }
            ddlBatch.Items.Clear();
            ddlBatch.Items.Add(new ListItem("N/A", "-1"));
        }

        private void LoadBatchDDL(int programID, int acaCalID)
        {
            if (programID > 0 && acaCalID > 0)
            {
                using (var db = new GeneralDataManager())
                {
                    var batch = db.AdmissionDB.SPBatchGetAllByProgram(programID).ToList();
                    batch = batch.Where(x => x.AcaCalId == acaCalID).OrderByDescending(x => x.BatchId).ToList();
                    DDLHelper.Bind<DAL.SPBatchGetAllByProgram_Result>(ddlBatch, batch, "BatchNO", "BatchId", EnumCollection.ListItemType.Batch);
                }
            }
            else
            {
                ddlBatch.Items.Clear();
                ddlBatch.Items.Add(new ListItem("N/A", "-1"));
            }
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            int progId = -1;
            progId = Convert.ToInt32(ddlProgram.SelectedValue);
            if (progId > 0)
            {
                using (var db = new GeneralDataManager())
                {
                    var sessions = db.AdmissionDB.SPAcademicCalendarGetAllByProgram(progId).ToList();
                    sessions = sessions.OrderByDescending(z => z.AcademicCalenderID).ToList();
                    DDLHelper.Bind<DAL.SPAcademicCalendarGetAllByProgram_Result>(ddlSession, sessions, "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
                }
            }
            else
            {
                ddlSession.Items.Clear();
                ddlSession.Items.Add(new ListItem("N/A", "-1"));
            }
            ddlBatch.Items.Clear();
            ddlBatch.Items.Add(new ListItem("N/A", "-1"));
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sessionId = -1;
            sessionId = Convert.ToInt32(ddlSession.SelectedValue);
            int progId = -1;
            progId = Convert.ToInt32(ddlProgram.SelectedValue);
            if (sessionId > 0 && progId > 0)
            {
                using (var db = new GeneralDataManager())
                {
                    var batch = db.AdmissionDB.SPBatchGetAllByProgram(progId).ToList();
                    batch = batch.Where(x => x.AcaCalId == sessionId).OrderByDescending(x => x.BatchId).ToList();
                    DDLHelper.Bind<DAL.SPBatchGetAllByProgram_Result>(ddlBatch, batch, "BatchNO", "BatchId", EnumCollection.ListItemType.Batch);
                }
            }
            else
            {
                ddlBatch.Items.Clear();
                ddlBatch.Items.Add(new ListItem("N/A", "-1"));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            DAL.AdmissionUnitProgram obj = new DAL.AdmissionUnitProgram();

            string programName = null;
            string programShortName = null;

            int progId = Convert.ToInt32(ddlProgram.SelectedValue);
            int sessionId = Convert.ToInt32(ddlSession.SelectedValue);
            int batchId = Convert.ToInt32(ddlBatch.SelectedValue);
            long admissionUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            int educationCategoryId = Convert.ToInt32(ddlEducationCategory.SelectedValue);

            DAL.AdmissionUnitProgram admUnitProgExist = null;



            if (progId > 0 && sessionId > 0 && batchId > 0 && admissionUnitId > 0)
            {
                using (var dbProg = new GeneralDataManager())
                {
                    DAL.SPProgramsGetAllFromUCAM_Result program = dbProg.AdmissionDB
                        .SPProgramsGetAllFromUCAM()
                        .Where(a => a.ProgramID == progId)
                        .FirstOrDefault();

                    if (program != null && program.ProgramID > 0)
                    {
                        programName = program.DetailName;
                        programShortName = program.ShortName;
                    }
                    else
                    {
                        programName = "N/A";
                    }
                }


                obj.ProgramID = progId;
                obj.EducationCategoryID = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                obj.AcaCalID = sessionId;
                obj.BatchID = batchId;
                obj.AdmissionUnitID = admissionUnitId;
                obj.IsActive = ckbxIsActive.Checked;


                obj.ShortCode = programShortName;
                obj.ProgramName = programName;

                obj.ID = CurrentAdmissionUnitProgramID;

                try
                {
                    if (obj.ID > 0) //update
                    {
                        using (var db1 = new GeneralDataManager())
                        {
                            DAL.AdmissionUnitProgram tempObj = new DAL.AdmissionUnitProgram();
                            tempObj = db1.AdmissionDB.AdmissionUnitPrograms.Find(obj.ID);

                            if (tempObj != null)
                            {
                                obj.DateCreated = tempObj.DateCreated;
                                obj.CreatedBy = tempObj.CreatedBy;
                            }
                        }
                        using (var db = new GeneralDataManager())
                        {
                            obj.DateModified = DateTime.Now;
                            obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                            db.Update<DAL.AdmissionUnitProgram>(obj);
                        }
                        lblMessage.Text = "Updated successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                    else //create new.
                    {
                        using (var dbCheck = new OfficeDataManager())
                        {
                            admUnitProgExist = dbCheck.AdmissionDB.AdmissionUnitPrograms
                                .Where(c =>
                                    c.ProgramID == progId &&
                                    c.AcaCalID == sessionId &&
                                    c.BatchID == batchId &&
                                    c.AdmissionUnitID == admissionUnitId &&
                                    c.EducationCategoryID == educationCategoryId &&
                                    c.IsActive == true)
                                .FirstOrDefault();
                        }

                        if (admUnitProgExist != null) //admission unit program already exist for given params, do not proceed.
                        {
                            lblMessage.Text = "Active admission unit program already exist with the same program, session, batch, admission unit and education category.";
                            messagePanel.CssClass = "alert alert-danger";
                            messagePanel.Visible = true;
                            return;
                        }
                        else
                        {
                            using (var db = new GeneralDataManager())
                            {
                                obj.DateCreated = DateTime.Now;
                                obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                                obj.DateModified = null;
                                obj.ModifiedBy = null;
                                db.Insert<DAL.AdmissionUnitProgram>(obj);
                                id = obj.ID;
                            }
                            if (id > 0)
                            {
                                lblMessage.Text = "Saved successfully.";
                                messagePanel.CssClass = "alert alert-success";
                                messagePanel.Visible = true;
                            }
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
            btnSave.Text = "Save";
            ClearFields();
            LoadListView();

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            lblMessage.Text = string.Empty;
            messagePanel.CssClass = string.Empty;
            messagePanel.Visible = false;
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                //List<DAL.AdmissionUnitProgram> list = new List<DAL.AdmissionUnitProgram>();
                List<DAL.AdmissionUnitProgram> list = db.GetAllAdmissionUnitProgram_AD()
                    .OrderByDescending(a => a.ID)
                    .OrderByDescending(a => a.IsActive)
                    .ToList();


                if (list != null)
                {
                    lvAdmissionUnitProgram.DataSource = list.ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvAdmissionUnitProgram.DataSource = null;
                    lblCount.Text = list.Count().ToString();
                }
                lvAdmissionUnitProgram.DataBind();
            }
        }

        protected void lvAdmissionUnitProgram_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.AdmissionUnitProgram admUnitProg = (DAL.AdmissionUnitProgram)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblAdmissionUnitName = (Label)currentItem.FindControl("lblAdmissionUnitName");
                Label lblProgramName = (Label)currentItem.FindControl("lblProgramName");
                Label lblEducationCategory = (Label)currentItem.FindControl("lblEducationCategory");
                Label lblSession = (Label)currentItem.FindControl("lblSession");
                Label lblBatch = (Label)currentItem.FindControl("lblBatch");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                lblAdmissionUnitName.Text = admUnitProg.AdmissionUnit.UnitName;

                List<DAL.SPAcademicCalendarGetAllByProgram_Result> sessions = new List<DAL.SPAcademicCalendarGetAllByProgram_Result>();
                List<DAL.SPBatchGetAllByProgram_Result> batches = new List<DAL.SPBatchGetAllByProgram_Result>();
                List<DAL.SPProgramsGetAllFromUCAM_Result> programs = new List<DAL.SPProgramsGetAllFromUCAM_Result>();
                using (var db = new GeneralDataManager())
                {
                    programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();
                    sessions = db.AdmissionDB.SPAcademicCalendarGetAllByProgram(admUnitProg.ProgramID).ToList();
                    batches = db.AdmissionDB.SPBatchGetAllByProgram(admUnitProg.ProgramID).ToList();
                }
                lblProgramName.Text = programs.Where(a => a.ProgramID == admUnitProg.ProgramID)
                        .Select(a => a.ShortName).FirstOrDefault();

                lblEducationCategory.Text = admUnitProg.EducationCategory.CategoryName;

                lblSession.Text = sessions.Where(a => a.AcademicCalenderID == admUnitProg.AcaCalID)
                    .Select(a => a.FullCode).FirstOrDefault();

                using (var db = new OfficeDataManager())
                {

                    List<DAL.SPBatchGetAllByProgram_Result> batchList = db.AdmissionDB.SPBatchGetAllByProgram(admUnitProg.ProgramID).ToList();
                    DAL.SPBatchGetAllByProgram_Result batchObj = batchList.Where(x => x.AcaCalId == admUnitProg.AcaCalID).FirstOrDefault();

                    if (batchObj != null)
                    {
                        lblBatch.Text = batchObj.BatchNO.ToString();
                    }
                }

                if (admUnitProg.IsActive == true)
                {
                    lblIsActive.Text = "✓";
                    lblIsActive.ForeColor = Color.Green;
                }
                else
                {
                    lblIsActive.Text = "✕";
                    lblIsActive.Font.Bold = true;
                    lblIsActive.ForeColor = Color.Crimson;
                }


                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = admUnitProg.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = admUnitProg.ID.ToString();
            }
        }

        protected void lvAdmissionUnitProgram_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                lblMessage.Text = string.Empty;
                messagePanel.CssClass = string.Empty;
                messagePanel.Visible = false;
                try
                {
                    using (var db = new GeneralDataManager())
                    {
                        var objectToDelete = db.AdmissionDB.AdmissionUnitPrograms.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.AdmissionUnitProgram>(objectToDelete);
                        CurrentAdmissionUnitProgramID = 0;
                    }
                    lblMessage.Text = "Admission unit deleted successfully.";
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
                using (var db = new GeneralDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.AdmissionUnitPrograms.Find(Convert.ToInt64(e.CommandArgument));

                    ClearFields();

                    ddlAdmissionUnit.SelectedValue = objectToUpdate.AdmissionUnitID.ToString();

                    ddlProgram.SelectedValue = objectToUpdate.ProgramID.ToString();
                    LoadSessionDDL(Convert.ToInt32(ddlProgram.SelectedValue));

                    ddlSession.SelectedValue = objectToUpdate.AcaCalID.ToString();
                    LoadBatchDDL(Convert.ToInt32(ddlProgram.SelectedValue), Convert.ToInt32(ddlSession.SelectedValue));

                    ddlBatch.SelectedValue = objectToUpdate.BatchID.ToString();

                    ddlEducationCategory.SelectedValue = objectToUpdate.EducationCategoryID.ToString();

                    if (objectToUpdate.IsActive == true)
                    {
                        ckbxIsActive.Checked = true;
                    }
                    else
                    {
                        ckbxIsActive.Checked = false;
                    }


                    CurrentAdmissionUnitProgramID = objectToUpdate.ID;


                    ddlSession.Enabled = false;
                    ddlSession.Attributes.CssStyle.Add("background-color", "#e6e6e6");
                    ddlBatch.Enabled = false;
                    ddlBatch.Attributes.CssStyle.Add("background-color", "#e6e6e6");


                    ddlAdmissionUnit.Focus();

                    btnSave.Text = "Update";
                }
            }
            //LoadListView();
        }

        protected void lvAdmissionUnitProgram_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }

        protected void lvAdmissionUnitProgram_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
        }

        protected void btnInActiveAll_Click(object sender, EventArgs e)
        {
            try
            {
                List<DAL.AdmissionUnitProgram> admUnitProgList = null;
                using (var db = new OfficeDataManager())
                {
                    admUnitProgList = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.IsActive == true).ToList();
                }

                if (admUnitProgList != null && admUnitProgList.Count > 0)
                {
                    foreach (var tData in admUnitProgList)
                    {
                        tData.IsActive = false;
                        tData.ModifiedBy = uId;
                        tData.DateModified = DateTime.Now;

                        using (var db = new OfficeDataManager())
                        {
                            db.Update<DAL.AdmissionUnitProgram>(tData);
                        }
                    }
                }

                LoadListView();

                lblMessage.Text = "In-Active successfully.";
                messagePanel.CssClass = "alert alert-success";
                messagePanel.Visible = true;


            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadListView();
        }
    }
}