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
    public partial class TestRollDelete : PageBase
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
                btnDelete.Visible = false;
                LoadDDL();
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

        private List<DAL.SPGetTestRollByAcaCalIDAdmUnitID_Result> CurrentTestRollList
        {
            get
            {
                if (ViewState["CurrentTestRollList"] == null)
                    return null;
                else
                    return (List<DAL.SPGetTestRollByAcaCalIDAdmUnitID_Result>)ViewState["CurrentTestRollList"];
            }
            set
            {
                ViewState["CurrentTestRollList"] = value;
            }
        }

        private void LoadListView(long admUnitId, int acaCalId)
        {
            List<DAL.SPGetTestRollByAcaCalIDAdmUnitID_Result> list = null;
            try
            {
                using (var db = new OfficeDataManager())
                {
                    list = db.AdmissionDB.SPGetTestRollByAcaCalIDAdmUnitID(acaCalId, admUnitId).ToList();
                }
            }
            catch (Exception)
            {
                lblMessage.Text = "Error loading test rolls";
                lblMessage.ForeColor = Color.Crimson;
                panelMessage.CssClass = "alert alert-danger";
            }

            if (list != null)
            {
                if (list.Count > 0)
                {
                    gvTestRoll.DataSource = list.OrderBy(c => c.testRoll).ToList();
                    lblCount.Text = list.Count.ToString();
                    //CurrentTestRollList = list;
                    btnDelete.Visible = true;
                }
            }
            else
            {
                gvTestRoll.DataSource = null;
                lblCount.Text = "0";
                //CurrentTestRollList = null;
                btnDelete.Visible = false;
            }
            gvTestRoll.DataBind();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            LoadListView(admUnitId, acaCalId);
            //List<DAL.SPGetTestRollByAcaCalIDAdmUnitID_Result> list = null;
            //try
            //{
            //    using (var db = new OfficeDataManager())
            //    {
            //        list = db.AdmissionDB.SPGetTestRollByAcaCalIDAdmUnitID(acaCalId, admUnitId).ToList();
            //    }
            //}
            //catch (Exception)
            //{
            //    lblMessage.Text = "Error loading test rolls";
            //    lblMessage.ForeColor = Color.Crimson;
            //    panelMessage.CssClass = "alert alert-danger";
            //}

            //if (list != null)
            //{
            //    if (list.Count > 0)
            //    {
            //        gvTestRoll.DataSource = list.OrderBy(c => c.testRoll).ToList();
            //        lblCount.Text = list.Count.ToString();
            //        //CurrentTestRollList = list;
            //        btnDelete.Visible = true;
            //    }
            //}
            //else
            //{
            //    gvTestRoll.DataSource = null;
            //    lblCount.Text = "0";
            //    //CurrentTestRollList = null;
            //    btnDelete.Visible = false;
            //}
            //gvTestRoll.DataBind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            List<DAL.AdmissionTestRoll> list = null;

            if (admUnitId > 0 && acaCalId > 0)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        list = db.AdmissionDB.AdmissionTestRolls
                            .Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == acaCalId)
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error in list:" + ex.Message + "; " + ex.InnerException.Message;
                    panelMessage.CssClass = "alert alert-danger";
                    panelMessage.Visible = true;
                    return;
                }

                if(list != null)
                {
                    if(list.Count() > 0)
                    {
                        foreach(DAL.AdmissionTestRoll item in list)
                        {
                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Delete<DAL.AdmissionTestRoll>(item);
                                }
                            }
                            catch (Exception ex)
                            {
                                lblMessage.Text = "Error deleting test roll:" + ex.Message + "; " + ex.InnerException.Message;
                                panelMessage.CssClass = "alert alert-danger";
                                panelMessage.Visible = true;
                                return;
                            }
                        } //end foreach
                    } //end if(list.Count() > 0)
                } //end if(list != null)
                LoadListView(admUnitId, acaCalId);
            } //end if (admUnitId > 0 && acaCalId > 0)
        }
    }
}