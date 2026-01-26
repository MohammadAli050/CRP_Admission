using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using DATAMANAGER;
using System.Drawing;
using CommonUtility;
using Admission.App_Start;

namespace Admission.Admission.HelpDesk
{
    public partial class DistrictSeatLimitSetup : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
            if (!IsPostBack)
            {
                //LoadDDL();
                LoadListView();
            }
        }

        private void LoadDDL()
        {
            //try
            //{
            //    using (var db = new GeneralDataManager())
            //    {
            //        DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.Where(x => x.IsActive == true).OrderBy(a => a.ID).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
            //    }

            //}
            //catch (Exception ex)
            //{
            //}
        }

        protected void MessageView(string msg, string status)
        {

            //if (status == "success")
            //{
            //    lblMessage.Text = string.Empty;
            //    lblMessage.Text = msg.ToString();
            //    lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
            //    lblMessage.Attributes.CssStyle.Add("color", "green");

            //    messagePanel.Visible = true;
            //    messagePanel.CssClass = "alert alert-success";


            //}
            //else if (status == "fail")
            //{
            //    lblMessage.Text = string.Empty;
            //    lblMessage.Text = msg.ToString();
            //    lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
            //    lblMessage.Attributes.CssStyle.Add("color", "crimson");

            //    messagePanel.Visible = true;
            //    messagePanel.CssClass = "alert alert-danger";
            //}
            //else if (status == "clear")
            //{
            //    lblMessage.Text = string.Empty;
            //    messagePanel.Visible = false;
            //}

        }


        private void LoadListView()
        {
            List<DAL.DistrictSeatLimit> list = null;
            using (var db = new GeneralDataManager())
            {
                list = db.AdmissionDB.DistrictSeatLimits.ToList();
            }

            if (list != null && list.Count > 0)
            {
                lblCount.Text = list.Count().ToString();

                lvDistrictSeatLimitSetup.DataSource = list.ToList();
                lvDistrictSeatLimitSetup.DataBind();
            }
            else
            {
                lblCount.Text = "0";
            }
        }

        private void Clear()
        {
            //ddlAdmissionUnit.SelectedValue = "-1";
            //ddlDistrict.SelectedValue = "-1";
            //txtSeatLimit.Text = string.Empty;
            ////ckbxIsActive.Checked = false;
            //DistrictSeatLimitSetupID = 0;

        }

        //public int DistrictSeatLimitSetupID
        //{
        //    //get
        //    //{
        //    //    if (ViewState["DistrictSeatLimitSetupID"] == null)
        //    //        return 0;
        //    //    else
        //    //        return Convert.ToInt32(ViewState["DistrictSeatLimitSetupID"].ToString());
        //    //}
        //    //set
        //    //{
        //    //    ViewState["DistrictSeatLimitSetupID"] = value;
        //    //}
        //}

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //btnSave.Text = "Save";
            //Clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //MessageView("", "clear");

            //int id = -1;
            //DAL.DistrictSeatLimit obj = new DAL.DistrictSeatLimit();

            //long admissionUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            //int districtId = Convert.ToInt32(ddlDistrict.SelectedValue);
            //string seatLimit = txtSeatLimit.Text.Trim();

            //if (districtId > 0 && admissionUnitId > 0 && (!string.IsNullOrEmpty(seatLimit) && Convert.ToInt32(seatLimit) > 0))
            //{
            //    obj.AdmissionUnitId = admissionUnitId;
            //    obj.DistrictId = districtId;
            //    obj.SeatLimit = Convert.ToInt32(seatLimit);

            //    obj.IsActive = true;
            //    obj.CreatedBy = (int)uId;
            //    obj.CreatedDate = DateTime.Now;

            //    obj.ID = DistrictSeatLimitSetupID;
            //    try
            //    {
            //        if (obj.ID > 0) //update
            //        {
            //            using (var db = new GeneralDataManager())
            //            {
            //                obj.ModifiedDate = DateTime.Now;
            //                obj.ModifiedBy = (int)uId;

            //                db.Update<DAL.DistrictSeatLimit>(obj);
            //            }

            //            btnSave.Text = "Save";
            //            Clear();
            //            LoadListView();

            //            #region Log
            //            try
            //            {
            //                DAL_Log.DataLog thDLog = new DAL_Log.DataLog();
            //                thDLog.DateTime = DateTime.Now;
            //                thDLog.UserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
            //                thDLog.PageName = "DistrictSeatLimitSetup.aspx";
            //                thDLog.EventName = "Update District Seat Limit";
            //                thDLog.HostName = Request.UserHostName;
            //                thDLog.IpAddress = Request.UserHostAddress;
            //                thDLog.NewData = "Faculty:" + ddlAdmissionUnit.SelectedItem.Text + "_" +
            //                                 "District:" + ddlDistrict.SelectedItem.Text + "_" +
            //                                 "SeatLimit:" + txtSeatLimit.Text.Trim();
            //                thDLog.OldData = null;

            //                thDLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + "; "
            //                    + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
            //                thDLog.DateCreated = DateTime.Now;

            //                LogWriter.DataLogWriter(thDLog);
            //            }
            //            catch (Exception ex)
            //            {
            //            }
            //            #endregion

            //            MessageView("Update successful", "success");
            //        }
            //        else //create new
            //        {
            //            #region Dublicate Check
            //            DAL.DistrictSeatLimit model = null;
            //            using (var db = new OfficeDataManager())
            //            {
            //                model = db.AdmissionDB.DistrictSeatLimits.Where(x => x.AdmissionUnitId == admissionUnitId && x.DistrictId == districtId).FirstOrDefault();
            //            }
            //            if (model != null)
            //            {
            //                MessageView("Same Faculty and District is already exist !!", "fail");
            //                return;
            //            }
            //            #endregion

            //            using (var db = new GeneralDataManager())
            //            {
            //                obj.SeatFillup = 0;

            //                db.Insert<DAL.DistrictSeatLimit>(obj);
            //                id = obj.ID;
            //            }
            //            if (id > 0)
            //            {
            //                btnSave.Text = "Save";
            //                Clear();
            //                LoadListView();

            //                #region Log
            //                try
            //                {
            //                    DAL_Log.DataLog thDLog = new DAL_Log.DataLog();
            //                    thDLog.DateTime = DateTime.Now;
            //                    thDLog.UserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
            //                    thDLog.PageName = "DistrictSeatLimitSetup.aspx";
            //                    thDLog.EventName = "Create District Seat Limit";
            //                    thDLog.HostName = Request.UserHostName;
            //                    thDLog.IpAddress = Request.UserHostAddress;
            //                    thDLog.NewData = "Faculty:" + ddlAdmissionUnit.SelectedItem.Text + "_" +
            //                                     "District:" + ddlDistrict.SelectedItem.Text + "_" +
            //                                     "SeatLimit:" + txtSeatLimit.Text.Trim();
            //                    thDLog.OldData = null;

            //                    thDLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + "; "
            //                        + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
            //                    thDLog.DateCreated = DateTime.Now;

            //                    LogWriter.DataLogWriter(thDLog);
            //                }
            //                catch (Exception ex)
            //                {

            //                }
            //                #endregion


            //                MessageView("Save successful", "success");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageView("Exception: " + ex.Message.ToString(), "fail");
            //    }

            //}
            //else
            //{
            //    MessageView("Please provide all inputs and seat limit should be greater than zero !!", "fail");
            //}


        }

        protected void lvDistrictSeatLimitSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.DistrictSeatLimit obj = (DAL.DistrictSeatLimit)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblFaculty = (Label)currentItem.FindControl("lblFaculty");
                Label lblDistrictName = (Label)currentItem.FindControl("lblDistrictName");
                Label lblSeatLimit = (Label)currentItem.FindControl("lblSeatLimit");
                Label lblSeatFillup = (Label)currentItem.FindControl("lblSeatFillup");
                //Label lblUnitCode1 = (Label)currentItem.FindControl("lblUnitCode1");
                //Label lblUnitCode2 = (Label)currentItem.FindControl("lblUnitCode2");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                //LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                //LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                if (obj.DistrictId == 1)
                {
                    lblDistrictName.Text = "Dhaka";
                }
                else if (obj.DistrictId == 2)
                {
                    lblDistrictName.Text = "Chattogram";
                }
                else if (obj.DistrictId == 3)
                {
                    lblDistrictName.Text = "Bogura";
                }
                else if (obj.DistrictId == 4)
                {
                    lblDistrictName.Text = "Khulna";
                }
                else
                {
                    lblDistrictName.Text = "";
                }

                lblSeatLimit.Text = obj.SeatLimit.ToString();


                DAL.AdmissionUnit admUnit = null;
                using (var db = new OfficeDataManager())
                {
                    admUnit = db.AdmissionDB.AdmissionUnits.Where(x => x.ID == obj.AdmissionUnitId).FirstOrDefault();
                }
                if (admUnit != null)
                {
                    lblFaculty.Text = admUnit.UnitName;
                }

                if (obj.SeatFillup > 0)
                {
                    lblSeatFillup.Text = obj.SeatFillup.ToString();
                }


                if (obj.IsActive == true)
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

                //lnkEdit.CommandName = "Update";
                //lnkEdit.CommandArgument = obj.ID.ToString();

                //lnkDelete.CommandName = "Delete";
                //lnkDelete.CommandArgument = obj.ID.ToString();
            }
        }

        protected void lvDistrictSeatLimitSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //MessageView("", "clear");

            ////DAL.AdmissionUnit admissionUnit = new DAL.AdmissionUnit();
            //if (e.CommandName == "Delete")
            //{
            //    try
            //    {
            //        using (var db = new GeneralDataManager())
            //        {
            //            var objectToDelete = db.AdmissionDB.DistrictSeatLimits.Find(Convert.ToInt64(e.CommandArgument));
            //            db.Delete<DAL.DistrictSeatLimit>(objectToDelete);
            //            DistrictSeatLimitSetupID = 0;
            //        }

            //        LoadListView();
            //        MessageView("Data delete successful", "success");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageView("Exception: " + ex.Message.ToString(), "fail");
            //    }

            //}
            //else if (e.CommandName == "Update")
            //{
            //    btnSave.Text = "Update";
            //    using (var db = new GeneralDataManager())
            //    {
            //        var objectToUpdate = db.AdmissionDB.DistrictSeatLimits.Find(Convert.ToInt64(e.CommandArgument));
            //        Clear();
            //        ddlDistrict.SelectedValue = objectToUpdate.DistrictId.ToString();
            //        txtSeatLimit.Text = objectToUpdate.SeatLimit.ToString();
            //        ddlAdmissionUnit.SelectedValue = objectToUpdate.AdmissionUnitId.ToString();

            //        //if (objectToUpdate.IsActive == true)
            //        //{
            //        //    ckbxIsActive.Checked = true;
            //        //}
            //        //else
            //        //{
            //        //    ckbxIsActive.Checked = false;
            //        //}
            //        DistrictSeatLimitSetupID = objectToUpdate.ID;
            //    }
            //}
            ////LoadListView();
        }

        protected void lvDistrictSeatLimitSetup_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvDistrictSeatLimitSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }
    }
}