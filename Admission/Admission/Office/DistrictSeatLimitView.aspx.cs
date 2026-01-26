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

namespace Admission.Admission.Office
{
    public partial class DistrictSeatLimitView : PageBase
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
                //LoadDDL();
                LoadListView();
                DistrictSeatLimitSetupID = 0;
            }
        }

        private void LoadDDL()
        {
            //try
            //{
            //    using (var db = new GeneralDataManager())
            //    {
            //        DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.Where(x => x.IsActive == true).OrderBy(a => a.ID).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
            //        DDLHelper.Bind<DAL.DistrictForSeatPlan>(ddlDistrict, db.AdmissionDB.DistrictForSeatPlans.Where(x => x.IsActive == true).OrderBy(x => x.ID).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
            //        DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);

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
                list = db.AdmissionDB.DistrictSeatLimits.Where(x=> x.IsActive == true).ToList();
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
            //ddlSession.SelectedValue = "-1";
            //ddlAdmissionUnit.SelectedValue = "-1";
            //ddlDistrict.SelectedValue = "-1";
            //txtSeatLimit.Text = string.Empty;
            //ckbxIsActive.Checked = false;
            DistrictSeatLimitSetupID = 0;

        }

        public int DistrictSeatLimitSetupID
        {
            get
            {
                if (ViewState["DistrictSeatLimitSetupID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["DistrictSeatLimitSetupID"].ToString());
            }
            set
            {
                ViewState["DistrictSeatLimitSetupID"] = value;
            }
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

                Label lblSession = (Label)currentItem.FindControl("lblSession");

                //LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                //LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();

                DAL.DistrictForSeatPlan dis = null;
                using (var db = new GeneralDataManager())
                {
                    dis = db.AdmissionDB.DistrictForSeatPlans.Where(x => x.ID == obj.DistrictId).FirstOrDefault();
                }
                if (dis != null)
                {
                    lblDistrictName.Text = dis.Name;
                }
                else
                {
                    lblDistrictName.Text = "";
                }
                #region N/A
                //if (obj.DistrictId == 1)
                //{
                //    lblDistrictName.Text = "Dhaka";
                //}
                //else if (obj.DistrictId == 2)
                //{
                //    lblDistrictName.Text = "Chattogram";
                //}
                //else if (obj.DistrictId == 3)
                //{
                //    lblDistrictName.Text = "Bogura";
                //}
                //else if (obj.DistrictId == 4)
                //{
                //    lblDistrictName.Text = "Khulna";
                //} 
                #endregion


                using (var dbSession = new GeneralDataManager())
                {
                    var session = dbSession.AdmissionDB.SPAcademicCalendarGetById(obj.AcaCalId).FirstOrDefault();
                    if (session != null)
                    {
                        if (session.AcademicCalenderID > 0)
                        {
                            lblSession.Text = session.FullCode;
                        }
                    }
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
            MessageView("", "clear");

            //DAL.AdmissionUnit admissionUnit = new DAL.AdmissionUnit();
            if (e.CommandName == "Delete")
            {
                //try
                //{
                //    using (var db = new GeneralDataManager())
                //    {
                //        var objectToDelete = db.AdmissionDB.DistrictSeatLimits.Find(Convert.ToInt64(e.CommandArgument));
                //        db.Delete<DAL.DistrictSeatLimit>(objectToDelete);
                //        DistrictSeatLimitSetupID = 0;
                //    }

                //    LoadListView();
                //    MessageView("Data delete successful", "success");
                //}
                //catch (Exception ex)
                //{
                //    MessageView("Exception: " + ex.Message.ToString(), "fail");
                //}

            }
            else if (e.CommandName == "Update")
            {
                //btnSave.Text = "Update";
                //using (var db = new GeneralDataManager())
                //{
                //    var objectToUpdate = db.AdmissionDB.DistrictSeatLimits.Find(Convert.ToInt64(e.CommandArgument));
                //    Clear();
                //    ddlSession.SelectedValue = objectToUpdate.AcaCalId.ToString();
                //    ddlAdmissionUnit.SelectedValue = objectToUpdate.AdmissionUnitId.ToString();
                //    ddlDistrict.SelectedValue = objectToUpdate.DistrictId.ToString();
                //    txtSeatLimit.Text = objectToUpdate.SeatLimit.ToString();
                //    cbIsActive.Checked = Convert.ToBoolean(objectToUpdate.IsActive);


                //    DistrictSeatLimitSetupID = objectToUpdate.ID;
                //}
            }
        }

        protected void lvDistrictSeatLimitSetup_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvDistrictSeatLimitSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }
    }
}