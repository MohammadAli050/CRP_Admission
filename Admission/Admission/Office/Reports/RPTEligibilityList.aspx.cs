using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTEligibilityList : PageBase
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
                LoadDDL();
            }
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                //DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        protected void ddlAdmUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            long? admUnitId = null;
            if (ddlAdmUnit.SelectedValue == "-1")
            {
                admUnitId = null;
            }
            else
            {
                admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            }

            List<DAL.AdmissionSetup> admSetupList = null;
            if (admUnitId > 0 && admUnitId != null)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetupList = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading session. " + ex.Message + "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    return;
                }

            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {

        }
    }
}