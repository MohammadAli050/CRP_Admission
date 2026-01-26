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
using Microsoft.Reporting.WebForms;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTQuotaReportDetails : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        string userName = "";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

            using (var db = new OfficeDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if (!IsPostBack)
            {
                LoadDDL();
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

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.All);
                DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(c => c.IsActive == true).ToList(), "Remarks", "ID", EnumCollection.ListItemType.All);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.IsActive == true).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.EducationCategory);

                ddlEducationCategory.SelectedValue = "4";

            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (acaCalId > 0)
                {
                    int? facultyId = null;
                    int? eduCatId = null;

                    if (Convert.ToInt32(ddlAdmUnit.SelectedValue) > 0)
                    {
                        facultyId = Convert.ToInt32(ddlAdmUnit.SelectedValue);
                    }

                    
                    eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);

                    int? quotaId = null;
                    if (Convert.ToInt32(ddlQuota.SelectedValue) > 0)
                    {
                        quotaId = Convert.ToInt32(ddlQuota.SelectedValue);
                    }
                    else
                    {
                        quotaId = null;
                    }

                    List<DAL.GetAllQuotaInfoWithTypes_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.GetAllQuotaInfoWithTypes(facultyId,
                                                                 acaCalId,
                                                                 quotaId).ToList();
                    }
                    if (list != null && list.Count > 0)
                    {
                        ReportViewer1.LocalReport.EnableExternalImages = true;
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportDataSource rds = new ReportDataSource("DataSet1", list);
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Admission/Office/Reports/RPTQuotaReportDetails.rdlc");

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                        param1.Add(new ReportParameter("Faculty", ddlAdmUnit.SelectedItem.Text));
                        param1.Add(new ReportParameter("Quota", ddlQuota.SelectedItem.Text));

                        ReportViewer1.LocalReport.SetParameters(param1);
                        ReportViewer1.LocalReport.DisplayName = "QuotaReport" + ddlSession.SelectedItem.Text.ToString();
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                    }

                }
                else
                {
                    MessageView("Please select session!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

    }
}