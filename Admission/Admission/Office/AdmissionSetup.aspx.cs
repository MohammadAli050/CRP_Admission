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

namespace Admission.Admission.Office
{
    public partial class AdmissionSetup : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
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
                LoadListViewInactive();
            }
        }

        private DAL.AdmissionSetup AdmissionSetupObj { get; set; }

        private long CurrentAdmissionSetupID
        {
            get
            {
                if (ViewState["CurrentAdmissionSetupID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentAdmissionSetupID"].ToString());
            }
            set
            {
                ViewState["CurrentAdmissionSetupID"] = value;
            }
        }

        private void ClearFields()
        {
            ddlAdmissionUnit.SelectedIndex = -1;
            ddlEducationCategory.SelectedIndex = -1;
            ddlStore.SelectedIndex = -1;
            ddlHour.SelectedIndex = -1;
            ddlMinute.SelectedIndex = -1;
            ddlAmPm.SelectedIndex = -1;
            //ddlBatch.SelectedIndex = -1;
            ddlSession.SelectedIndex = -1;
            txtEndDate.Text = string.Empty;
            txtExamDate.Text = string.Empty;
            txtFee.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtTestNumber.Text = string.Empty;
            ckbxIsActive.Checked = false;
            txtVivaDate.Text = string.Empty;
            ddlAmPmViva.SelectedIndex = -1;
            ddlHourViva.SelectedIndex = -1;
            ddlMinuteViva.SelectedIndex = -1;
            txtBkashNumber.Text = string.Empty;

            ddlGateway.SelectedValue = "-1";
            ddlStore.Items.Clear();
        }

        private void ClearMessage()
        {
            lblMessage.Text = string.Empty;
            messagePanel.CssClass = string.Empty;
            messagePanel.Visible = false;
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.ToList(), "UnitName", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.Store>(ddlStore, db.AdmissionDB.Stores.Where(c=>c.IsActive == true).ToList(), "StoreName", "ID", EnumCollection.ListItemType.Select);

                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);

                //get only Bachelors/Pass, Masters & Certificate/Professional
                DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.ID == 4 || a.ID == 6 || a.ID == 8).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.Select);
            }

            List<string> hour = new List<string>();
            List<string> minute = new List<string>();

            for (int i = 1; i < 13; i++)
            {
                hour.Add(i.ToString("00"));
            }
            ddlHour.Items.Clear();
            ddlHour.Items.Add(new ListItem("Hour", "-1"));
            ddlHour.AppendDataBoundItems = true;
            ddlHour.DataSource = hour.ToList();
            ddlHour.DataBind();

            ddlHourViva.Items.Clear();
            ddlHourViva.Items.Add(new ListItem("Hour", "-1"));
            ddlHourViva.AppendDataBoundItems = true;
            ddlHourViva.DataSource = hour.ToList();
            ddlHourViva.DataBind();

            for (int i = 0; i < 60; i++)
            {
                minute.Add(i.ToString("00"));
            }
            ddlMinute.Items.Clear();
            ddlMinute.Items.Add(new ListItem("Minute", "-1"));
            ddlMinute.AppendDataBoundItems = true;
            ddlMinute.DataSource = minute.ToList();
            ddlMinute.DataBind();

            ddlMinuteViva.Items.Clear();
            ddlMinuteViva.Items.Add(new ListItem("Minute", "-1"));
            ddlMinuteViva.AppendDataBoundItems = true;
            ddlMinuteViva.DataSource = minute.ToList();
            ddlMinuteViva.DataBind();

            ddlAmPm.Items.Clear();
            ddlAmPm.Items.Add(new ListItem("AM/PM", "-1"));
            ddlAmPm.Items.Add(new ListItem("AM", "AM"));
            ddlAmPm.Items.Add(new ListItem("PM", "PM"));
            ddlAmPm.DataBind();

            ddlAmPmViva.Items.Clear();
            ddlAmPmViva.Items.Add(new ListItem("AM/PM", "-1"));
            ddlAmPmViva.Items.Add(new ListItem("AM", "AM"));
            ddlAmPmViva.Items.Add(new ListItem("PM", "PM"));
            ddlAmPmViva.DataBind();

            ddlGateway.Items.Clear();
            ddlGateway.Items.Add(new ListItem("Select Gateway", "-1"));
            ddlGateway.Items.Add(new ListItem("SSL", "SSL"));
            ddlGateway.Items.Add(new ListItem("Foster", "FPG"));
            ddlGateway.Items.Add(new ListItem("EkPay", "EkPay"));
            ddlGateway.DataBind();

        }

        protected void ddlGateway_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGateway.SelectedValue == "-1")
            {
                ddlStore.Items.Clear();
            }
            else if (ddlGateway.SelectedValue == "SSL")
            {
                ddlStore.Items.Clear();
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind(ddlStore, db.GetAllActiveSSLStore(true), "StoreName", "ID", EnumCollection.ListItemType.Select);
                }
            }
            else if (ddlGateway.SelectedValue == "FPG")
            {
                ddlStore.Items.Clear();
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind(ddlStore, db.GetAllActiveFPGStore(true), "StoreName", "ID", EnumCollection.ListItemType.Select);
                }
            }
            else if (ddlGateway.SelectedValue == "EkPay")
            {
                ddlStore.Items.Clear();
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind(ddlStore, db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).ToList(), "StoreName", "ID", EnumCollection.ListItemType.Select);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            DAL.AdmissionSetup obj = new DAL.AdmissionSetup();
            AdmissionSetupObj = null;

            string logOldObject = string.Empty;
            string logNewObject = string.Empty;

            try
            {
                if (CurrentAdmissionSetupID > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        AdmissionSetupObj = db.AdmissionDB.AdmissionSetups.Find(Convert.ToInt64(CurrentAdmissionSetupID));

                        //old data for log
                        logOldObject = ObjectToString.ConvertToString(AdmissionSetupObj);
                    }
                }
                if (AdmissionSetupObj != null && CurrentAdmissionSetupID > 0) //update
                {
                    obj.AcaCalID = Convert.ToInt32(ddlSession.SelectedValue);
                    obj.AdmissionUnitID = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);
                    obj.EducationCategoryID = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                    obj.BatchID = -1;
                    obj.EndDate = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", null);
                    obj.ExamDate = DateTime.ParseExact(txtExamDate.Text, "dd/MM/yyyy", null);
                    obj.Fee = Convert.ToDecimal(txtFee.Text);
                    obj.FileUrl = null;
                    obj.IsActive = ckbxIsActive.Checked;


                    obj.StartDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", null);

                    obj.Attribute2 = ddlGateway.SelectedValue;
                    obj.StoreID = Convert.ToInt32(ddlStore.SelectedValue);

                    obj.TestNo = Convert.ToInt32(txtTestNumber.Text);
                    obj.ExamTime = ddlHour.SelectedValue.ToString() + ":" + ddlMinute.SelectedValue.ToString() + ":" + ddlAmPm.SelectedValue.ToString();
                    obj.VivaDate = DateTime.ParseExact(txtVivaDate.Text, "dd/MM/yyyy", null);
                    obj.VivaTime = ddlHourViva.SelectedValue.ToString() + ":" + ddlMinuteViva.SelectedValue.ToString() + ":" + ddlAmPmViva.SelectedValue.ToString();
                    if (!string.IsNullOrEmpty(txtBkashNumber.Text.Trim()))
                    {
                        obj.Attribute1 = txtBkashNumber.Text; //keep bkash merchant account number in attribute 1
                    }
                    else
                    {
                        obj.Attribute1 = null;
                    }

                    obj.ID = AdmissionSetupObj.ID;

                    obj.DateCreated = AdmissionSetupObj.DateCreated;
                    obj.CreatedBy = AdmissionSetupObj.CreatedBy;
                    obj.DateModified = DateTime.Now;
                    obj.ModifiedBy = uId;

                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.AdmissionSetup>(obj);

                        logNewObject = ObjectToString.ConvertToString(obj);
                    }

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "btnSave_Click";
                    dLog.PageName = "AdmissionSetup.aspx";
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


                    LoadListView();

                    lblMessage.Text = "Admisstion setup updated successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;

                    AdmissionSetupObj = null;
                    CurrentAdmissionSetupID = 0;
                }
                else if (AdmissionSetupObj == null && CurrentAdmissionSetupID == 0) //create new
                {
                    DAL.AdmissionSetup admStpExist = null;
                    using (var db = new OfficeDataManager())
                    {
                        int acaCalId = Int32.Parse(ddlSession.SelectedValue);
                        long admUnitId = Int64.Parse(ddlAdmissionUnit.SelectedValue);
                        int eduCatId = Int32.Parse(ddlEducationCategory.SelectedValue);
                        admStpExist = db.AdmissionDB.AdmissionSetups
                            .Where(c =>
                               // c.AcaCalID == acaCalId &&
                                c.AdmissionUnitID == admUnitId &&
                                c.EducationCategoryID == eduCatId &&
                                c.IsActive == true)
                            .FirstOrDefault();
                    }
                    if (admStpExist != null) //if active with same params exists then do not proceed with save.
                    {
                        lblMessage.Text = "Active admission setup with provided Faculty and Education category if alredy exists.";
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        return;
                    }
                    else // else proceed with save.
                    {

                        obj.AcaCalID = Convert.ToInt32(ddlSession.SelectedValue);
                        obj.AdmissionUnitID = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);
                        obj.EducationCategoryID = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                        obj.BatchID = -1;
                        obj.EndDate = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", null);
                        obj.ExamDate = DateTime.ParseExact(txtExamDate.Text, "dd/MM/yyyy", null);
                        obj.Fee = Convert.ToDecimal(txtFee.Text);
                        obj.FileUrl = null;
                        obj.IsActive = ckbxIsActive.Checked;


                        obj.StartDate = DateTime.ParseExact(txtStartDate.Text.ToString(), "dd/MM/yyyy", null);

                        obj.Attribute2 = ddlGateway.SelectedValue;
                        obj.StoreID = Convert.ToInt32(ddlStore.SelectedValue);

                        obj.TestNo = Convert.ToInt32(txtTestNumber.Text);
                        obj.ExamTime = ddlHour.SelectedValue.ToString() + ":" + ddlMinute.SelectedValue.ToString() + ":" + ddlAmPm.SelectedValue.ToString();
                        obj.VivaDate = DateTime.ParseExact(txtVivaDate.Text, "dd/MM/yyyy", null);
                        obj.VivaTime = ddlHourViva.SelectedValue.ToString() + ":" + ddlMinuteViva.SelectedValue.ToString() + ":" + ddlAmPmViva.SelectedValue.ToString();
                        if (!string.IsNullOrEmpty(txtBkashNumber.Text.Trim()))
                        {
                            obj.Attribute1 = txtBkashNumber.Text; //keep bkash merchant account number in attribute 1
                        }
                        else
                        {
                            obj.Attribute1 = null;
                        }

                        obj.DateCreated = DateTime.Now;
                        obj.CreatedBy = uId;
                        obj.DateModified = DateTime.Now;
                        obj.ModifiedBy = uId;

                        using (var db = new OfficeDataManager())
                        {
                            db.Insert<DAL.AdmissionSetup>(obj);
                            id = obj.ID;

                            logNewObject = ObjectToString.ConvertToString(obj);
                        }

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "btnSave_Click";
                        dLog.PageName = "AdmissionSetup.aspx";
                        dLog.OldData = null;
                        dLog.NewData = logNewObject;
                        dLog.UserId = uId;
                        dLog.SessionInformation = "SU-ID: " + SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; UserRole: " +
                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                        LogWriter.DataLogWriter(dLog);
                        #endregion

                        if (id > 0)
                        {
                            LoadListView();

                            lblMessage.Text = "Admission Setup saved successfully.";
                            messagePanel.CssClass = "alert alert-success";
                            messagePanel.Visible = true;
                        }
                        AdmissionSetupObj = null;
                        CurrentAdmissionSetupID = 0;
                    }
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
            LoadListViewInactive();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            AdmissionSetupObj = null;
            CurrentAdmissionSetupID = 0;
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.AdmissionSetup> list = db.AdmissionDB.AdmissionSetups.Where(x=>x.IsActive==true 
                || (x.Attribute3!=null && x.Attribute3.ToLower().ToString().StartsWith("active"))).ToList();

                if (list != null)
                {
                    lvAdmSetup.DataSource = list.OrderBy(c => c.StartDate).ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvAdmSetup.DataSource = null;
                    lblCount.Text = list.Count().ToString();
                }
                lvAdmSetup.DataBind();
            }
        }

        protected void lvAdmSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.AdmissionSetup admSetup = (DAL.AdmissionSetup)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblAdmissionUnit = (Label)currentItem.FindControl("lblAdmissionUnit");
                //Label lblStore = (Label)currentItem.FindControl("lblStore");
                Label lblStartDate = (Label)currentItem.FindControl("lblStartDate");
                Label lblEndDate = (Label)currentItem.FindControl("lblEndDate");
                Label lblSession = (Label)currentItem.FindControl("lblSession");
                //Label lblBatch = (Label)currentItem.FindControl("lblBatch");
                Label lblEduCat = (Label)currentItem.FindControl("lblEducationCategory");
                Label lblFee = (Label)currentItem.FindControl("lblFee");
                Label lblTestNo = (Label)currentItem.FindControl("lblTestNo");
                Label lblExamDateTime = (Label)currentItem.FindControl("lblExamDateTime");
                Label lblVivaExamDateTime = (Label)currentItem.FindControl("lblVivaExamDateTime");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");
                Label lblStore = (Label)currentItem.FindControl("lblStore");
                Label lblBkashStore = (Label)currentItem.FindControl("lblBkashStore");

                

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                lblAdmissionUnit.Text = admSetup.AdmissionUnit.UnitName;
                //lblStore.Text = admSetup.Store.StoreName;
                lblStartDate.Text = admSetup.StartDate.ToString("dd/MM/yyyy");
                lblEndDate.Text = admSetup.EndDate.ToString("dd/MM/yyyy");

                //lblBatch.Text = admSetup.BatchID.ToString();
                using (var dbSession = new GeneralDataManager())
                {
                    var session = dbSession.AdmissionDB.SPAcademicCalendarGetAll().Where(c => c.AcademicCalenderID == admSetup.AcaCalID).FirstOrDefault();
                    if (session != null)
                    {
                        if (session.AcademicCalenderID > 0)
                        {
                            lblSession.Text = session.FullCode;
                        }
                    }
                }

                using (var dbEduCat = new GeneralDataManager())
                {
                    string catName = null;
                    var eduCat = dbEduCat.AdmissionDB.EducationCategories.Where(a => a.ID == 4 || a.ID == 6 || a.ID == 8).ToList();
                    foreach (var item in eduCat)
                    {
                        if (admSetup.EducationCategoryID == item.ID)
                        {
                            catName = item.CategoryName;
                        }
                    }
                    if (catName != null)
                    {
                        lblEduCat.Text = catName;
                    }
                }

                //store ----------------------------------------------
                if (admSetup.Attribute2 == "SSL")
                {
                    using (var dbStore = new OfficeDataManager())
                    {
                        var store = dbStore.AdmissionDB.Stores.Find(admSetup.StoreID);
                        if (store != null)
                        {
                            lblStore.Text = "(SSL) " + store.StoreName;
                        }
                        else
                        {
                            lblStore.Text = "N/A";
                            lblStore.ForeColor = Color.Crimson;
                        }
                    }
                }
                else if (admSetup.Attribute2 == "FPG")
                {
                    using (var dbStoreFPG = new OfficeDataManager())
                    {
                        var store = dbStoreFPG.AdmissionDB.StoreFosters.Find(admSetup.StoreID);
                        if (store != null)
                        {
                            lblStore.Text = "(FPG) " + store.StoreName;
                        }
                        else
                        {
                            lblStore.Text = "N/A";
                            lblStore.ForeColor = Color.Crimson;
                        }
                    }
                }
                else if (admSetup.Attribute2 == "EkPay")
                {
                    using (var dbStoreFPG = new OfficeDataManager())
                    {
                        var store = dbStoreFPG.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                        if (store != null)
                        {
                            lblStore.Text = "(EkPay) " + store.StoreName;
                        }
                        else
                        {
                            lblStore.Text = "N/A";
                            lblStore.ForeColor = Color.Crimson;
                        }
                    }
                }
                //-----------------------------------------------------

                if (admSetup.Attribute1 == null)
                {
                    lblBkashStore.Text = "N/A";
                    lblBkashStore.ForeColor = Color.Crimson;
                }
                else
                {
                    lblBkashStore.Text = admSetup.Attribute1; //attribute 1 is where bkash merchant account number is kept.
                }

                lblFee.Text = admSetup.Fee.ToString();
                lblTestNo.Text = admSetup.TestNo.ToString();
                lblExamDateTime.Text = Convert.ToDateTime(admSetup.ExamDate).ToString("dd/MM/yyyy") + " " + admSetup.ExamTime;
                lblVivaExamDateTime.Text = Convert.ToDateTime(admSetup.VivaDate).ToString("dd/MM/yyyy") + " " + admSetup.VivaTime;
                if (admSetup.IsActive == true)
                {
                    lblIsActive.Text = "✓";
                    lblIsActive.ForeColor = Color.Green;
                    lblIsActive.Font.Bold = true;
                }
                else
                {
                    lblIsActive.Text = "❌";
                    lblIsActive.ForeColor = Color.Crimson;
                }


                DateTime currentDate = DateTime.Now;

                if (currentDate > admSetup.EndDate)
                {
                    lblEndDate.ForeColor = Color.Crimson;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = admSetup.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = admSetup.ID.ToString();
            }
        }

        protected void lvAdmSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                ClearMessage();
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.AdmissionSetups.Find(Convert.ToInt32(e.CommandArgument));
                        db.Delete<DAL.AdmissionSetup>(objToDelete);
                        CurrentAdmissionSetupID = 0;
                        AdmissionSetupObj = null;
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
                ClearMessage();
                using (var db = new OfficeDataManager())
                {
                    AdmissionSetupObj = db.AdmissionDB.AdmissionSetups.Find(Convert.ToInt32(e.CommandArgument));
                    ClearFields();
                    LoadDDL();
                    if (AdmissionSetupObj != null)
                    {
                        CurrentAdmissionSetupID = AdmissionSetupObj.ID;

                        ddlAdmissionUnit.SelectedValue = AdmissionSetupObj.AdmissionUnitID.ToString();
                        ddlEducationCategory.SelectedValue = AdmissionSetupObj.EducationCategoryID.ToString();
                        txtStartDate.Text = AdmissionSetupObj.StartDate.ToString("dd/MM/yyyy");
                        txtEndDate.Text = AdmissionSetupObj.EndDate.ToString("dd/MM/yyyy");
                        ddlSession.SelectedValue = AdmissionSetupObj.AcaCalID.ToString();
                        //ddlBatch.SelectedValue = AdmissionSetupObj.BatchID.ToString();
                        txtFee.Text = AdmissionSetupObj.Fee.ToString("000");
                        txtTestNumber.Text = AdmissionSetupObj.TestNo.ToString();
                        txtExamDate.Text = Convert.ToDateTime(AdmissionSetupObj.ExamDate).ToString("dd/MM/yyyy");
                        txtVivaDate.Text = Convert.ToDateTime(AdmissionSetupObj.VivaDate).ToString("dd/MM/yyyy");
                        if (AdmissionSetupObj.Attribute1 == null)
                        {
                            txtBkashNumber.Text = string.Empty;
                        }
                        else
                        {
                            txtBkashNumber.Text = AdmissionSetupObj.Attribute1;
                        }


                        string examTimeString = AdmissionSetupObj.ExamTime;
                        string[] splitTime = examTimeString.Split(':');
                        //--- exam time
                        string hours, minutes, amPm;
                        if (splitTime.Any())
                        {
                            hours = splitTime[0].ToString();
                            minutes = splitTime[1].ToString();
                            amPm = splitTime[2].ToString();
                        }
                        else
                        {
                            hours = "-1";
                            minutes = "-1";
                            amPm = "-1";
                        }

                        ddlHour.SelectedValue = hours;
                        ddlMinute.SelectedValue = minutes;
                        ddlAmPm.SelectedValue = amPm;
                        //--- viva time
                        string vivaTimeString = AdmissionSetupObj.VivaTime;
                        string[] vivaSplitTime = vivaTimeString.Split(':');

                        string vHours, vMinutes, vAmPm;
                        if (vivaSplitTime.Any())
                        {
                            vHours = vivaSplitTime[0].ToString();
                            vMinutes = vivaSplitTime[1].ToString();
                            vAmPm = vivaSplitTime[2].ToString();
                        }
                        else
                        {
                            vHours = "-1";
                            vMinutes = "-1";
                            vAmPm = "-1";
                        }

                        ddlHourViva.SelectedValue = vHours;
                        ddlMinuteViva.SelectedValue = vMinutes;
                        ddlAmPmViva.SelectedValue = vAmPm;
                        //---
                        if (AdmissionSetupObj.IsActive == true)
                        {
                            ckbxIsActive.Checked = true;
                        }
                        else
                        {
                            ckbxIsActive.Checked = false;
                        }



                        //store ----------------------------------
                        ddlGateway.SelectedValue = AdmissionSetupObj.Attribute2;
                        if (AdmissionSetupObj.Attribute2 == "SSL")
                        {
                            ddlStore.Items.Clear();
                            using (var dbStoreSSL = new OfficeDataManager())
                            {
                                DDLHelper.Bind(ddlStore, dbStoreSSL.GetAllActiveSSLStore(true), "StoreName", "ID", EnumCollection.ListItemType.Select);
                            }
                            ddlStore.SelectedValue = AdmissionSetupObj.StoreID.ToString();
                        }
                        else if (AdmissionSetupObj.Attribute2 == "FPG")
                        {
                            ddlStore.Items.Clear();
                            using (var dbStoreFPG = new OfficeDataManager())
                            {
                                DDLHelper.Bind(ddlStore, dbStoreFPG.GetAllActiveFPGStore(true), "StoreName", "ID", EnumCollection.ListItemType.Select);
                            }
                            ddlStore.SelectedValue = AdmissionSetupObj.StoreID.ToString();
                        }
                        else if (AdmissionSetupObj.Attribute2 == "EkPay")
                        {
                            ddlStore.Items.Clear();
                            using (var dbStoreFPG = new OfficeDataManager())
                            {
                                DDLHelper.Bind(ddlStore, dbStoreFPG.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).ToList(), "StoreName", "ID", EnumCollection.ListItemType.Select);
                            }
                            ddlStore.SelectedValue = AdmissionSetupObj.StoreID.ToString();
                        }
                        //----------------------------------------

                        ddlSession.Enabled = false;
                        ddlSession.Attributes.CssStyle.Add("background-color", "#e6e6e6");

                        btnSave.Text = "Update";
                    }
                }
            }
        }

        protected void lvAdmSetup_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

        protected void lvAdmSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }

        private void LoadListViewInactive()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.AdmissionSetup> list = db.GetAllActiveAdmissionSetup_AD(false).OrderByDescending(x=>x.AcaCalID).Take(20).ToList();

                if (list != null)
                {
                    lvAdmSetupInactive.DataSource = list.OrderByDescending(c => c.EndDate).ToList();
                    lblRecordInactive.Text = list.Count().ToString();
                }
                else
                {
                    lvAdmSetupInactive.DataSource = null;
                    lblRecordInactive.Text = list.Count().ToString();
                }
                lvAdmSetupInactive.DataBind();
            }
        }

        protected void lvAdmSetupInactive_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.AdmissionSetup admSetup = (DAL.AdmissionSetup)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerialIn");
                Label lblAdmissionUnit = (Label)currentItem.FindControl("lblAdmissionUnitIn");
                //Label lblStore = (Label)currentItem.FindControl("lblStore");
                Label lblStartDate = (Label)currentItem.FindControl("lblStartDateIn");
                Label lblEndDate = (Label)currentItem.FindControl("lblEndDateIn");
                Label lblSession = (Label)currentItem.FindControl("lblSessionIn");
                //Label lblBatch = (Label)currentItem.FindControl("lblBatch");
                Label lblEduCat = (Label)currentItem.FindControl("lblEducationCategoryIn");
                Label lblFee = (Label)currentItem.FindControl("lblFeeIn");
                Label lblTestNo = (Label)currentItem.FindControl("lblTestNoIn");
                Label lblExamDate = (Label)currentItem.FindControl("lblExamDateIn");
                Label lblExamTime = (Label)currentItem.FindControl("lblExamTimeIn");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActiveIn");


                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                lblAdmissionUnit.Text = admSetup.AdmissionUnit.UnitName;
                //lblStore.Text = admSetup.Store.StoreName;
                lblStartDate.Text = admSetup.StartDate.ToString("dd/MM/yyyy");
                lblEndDate.Text = admSetup.EndDate.ToString("dd/MM/yyyy");

                //lblBatch.Text = admSetup.BatchID.ToString();
                using (var dbSession = new GeneralDataManager())
                {
                    var session = dbSession.AdmissionDB.SPAcademicCalendarGetAll().Where(c => c.AcademicCalenderID == admSetup.AcaCalID).FirstOrDefault();
                    if (session != null && session.AcademicCalenderID > 0)
                    {
                        lblSession.Text = session.FullCode;
                    }
                }

                using (var dbEduCat = new GeneralDataManager())
                {
                    string catName = null;
                    var eduCat = dbEduCat.AdmissionDB.EducationCategories.Where(a => a.ID == 4 || a.ID == 6 || a.ID == 8).ToList();
                    foreach (var item in eduCat)
                    {
                        if (admSetup.EducationCategoryID == item.ID)
                        {
                            catName = item.CategoryName;
                        }
                    }
                    if (catName != null)
                    {
                        lblEduCat.Text = catName;
                    }
                }


                lblFee.Text = admSetup.Fee.ToString();
                lblTestNo.Text = admSetup.TestNo.ToString();
                lblExamDate.Text = Convert.ToDateTime(admSetup.ExamDate).ToString("dd/MM/yyyy");
                lblExamTime.Text = admSetup.ExamTime;
                if (admSetup.IsActive == true)
                {
                    lblIsActive.Text = "YES";
                    lblIsActive.ForeColor = Color.Green;
                }
                else
                {
                    lblIsActive.Text = "NO";
                    lblIsActive.ForeColor = Color.Crimson;
                }

                DateTime currentDate = DateTime.Now;

                if (currentDate > admSetup.EndDate)
                {
                    lblEndDate.ForeColor = Color.Crimson;
                }

                // Set up the Edit button
                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                if (lnkEdit != null)
                {
                    lnkEdit.CommandName = "Update"; // Matches the check in ItemCommand
                    lnkEdit.CommandArgument = admSetup.ID.ToString();
                }

            }
        }


        protected void btnInActiveAll_Click(object sender, EventArgs e)
        {
            try
            {
                List<DAL.AdmissionSetup> admSetList = null;
                using (var db = new OfficeDataManager())
                {
                    admSetList = db.AdmissionDB.AdmissionSetups.Where(x => x.IsActive == true).ToList();
                }

                if (admSetList != null && admSetList.Count > 0)
                {
                    foreach (var tData in admSetList)
                    {
                        tData.IsActive = false;
                        tData.ModifiedBy = uId;
                        tData.DateModified = DateTime.Now;

                        using (var db = new OfficeDataManager())
                        {
                            db.Update<DAL.AdmissionSetup>(tData);
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
            LoadListViewInactive();
        }


        protected void lvAdmSetupInactive_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                ClearMessage();
                using (var db = new OfficeDataManager())
                {
                    // Fetch the object using the ID passed from CommandArgument
                    long setupID = Convert.ToInt64(e.CommandArgument);
                    var setupObj = db.AdmissionDB.AdmissionSetups.Find(setupID);

                    if (setupObj != null)
                    {
                        ClearFields();
                        LoadDDL();

                        // Store ID for Save button logic
                        CurrentAdmissionSetupID = setupObj.ID;

                        // Populate Fields
                        ddlAdmissionUnit.SelectedValue = setupObj.AdmissionUnitID.ToString();
                        ddlEducationCategory.SelectedValue = setupObj.EducationCategoryID.ToString();
                        txtStartDate.Text = setupObj.StartDate.ToString("dd/MM/yyyy");
                        txtEndDate.Text = setupObj.EndDate.ToString("dd/MM/yyyy");
                        ddlSession.SelectedValue = setupObj.AcaCalID.ToString();
                        txtFee.Text = setupObj.Fee.ToString("0");
                        txtTestNumber.Text = setupObj.TestNo.ToString();
                        txtExamDate.Text = setupObj.ExamDate.HasValue ? setupObj.ExamDate.Value.ToString("dd/MM/yyyy") : "";
                        txtVivaDate.Text = setupObj.VivaDate.HasValue ? setupObj.VivaDate.Value.ToString("dd/MM/yyyy") : "";

                        txtBkashNumber.Text = setupObj.Attribute1 ?? string.Empty;

                        // Exam Time Split
                        if (!string.IsNullOrEmpty(setupObj.ExamTime))
                        {
                            string[] splitTime = setupObj.ExamTime.Split(':');
                            if (splitTime.Length == 3)
                            {
                                ddlHour.SelectedValue = splitTime[0];
                                ddlMinute.SelectedValue = splitTime[1];
                                ddlAmPm.SelectedValue = splitTime[2];
                            }
                        }

                        // Viva Time Split
                        if (!string.IsNullOrEmpty(setupObj.VivaTime))
                        {
                            string[] vivaSplitTime = setupObj.VivaTime.Split(':');
                            if (vivaSplitTime.Length == 3)
                            {
                                ddlHourViva.SelectedValue = vivaSplitTime[0];
                                ddlMinuteViva.SelectedValue = vivaSplitTime[1];
                                ddlAmPmViva.SelectedValue = vivaSplitTime[2];
                            }
                        }

                        ckbxIsActive.Checked = setupObj.IsActive ?? false;

                        // Handle Gateway and Store
                        ddlGateway.SelectedValue = setupObj.Attribute2 ?? "-1";
                        if (ddlGateway.SelectedValue != "-1")
                        {
                            // Trigger the manual store loading logic
                            ddlGateway_SelectedIndexChanged(null, null);
                            ddlStore.SelectedValue = setupObj.StoreID.ToString();
                        }

                        ddlSession.Enabled = false;
                        ddlSession.Attributes.CssStyle.Add("background-color", "#e6e6e6");
                        btnSave.Text = "Update";

                        // Scroll to top so user sees the populated form
                        ScriptManager.RegisterStartupScript(this, GetType(), "ScrollTop", "window.scrollTo(0,0);", true);
                    }
                }
            }
        }
        protected void lvAdmSetupInactive_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            // This event is automatically raised by CommandName="Update"
            // Leave empty because logic is handled in ItemCommand
        }

    }
}