using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonUtility.FileConversion;

namespace Admission.Admission.Office.Reports
{
    public partial class FacultyWiseQuotaCount : PageBase
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
            try
            {
                using (var db = new GeneralDataManager())
                {
                    DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
                    DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.IsActive == true).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.EducationCategory);

                    DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.Where(x=> x.IsActive == true).OrderBy(a => a.ID).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                    DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);
                    ddlQuotaType.Items.Clear();
                    ddlQuotaType.Items.Add(new ListItem("Select Quota", "-1"));
                    ddlQuotaType.DataBind();
                }
                
            }
            catch (Exception ex)
            {
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

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);


                long admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
                int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);
                int quotaTypeId = Convert.ToInt32(ddlQuotaType.SelectedValue);


                if (acaCalId > 0 && eduCatId > 0)
                {
                    List<DAL.SPFacultyWiseQuotaCount_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPFacultyWiseQuotaCount(acaCalId, eduCatId).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {

                        #region If Filter is selected
                        if (admUnitId > 0)
                        {
                            list = list.Where(x => x.AdmissionUnitID == admUnitId).OrderBy(x=> x.QuotaID)
                                                                                    .ThenBy(x=> x.AdmissionUnitID)
                                                                                    .ThenBy(x=> x.QuotaTypeId).ToList();
                        }
                        if (quotaId > 0)
                        {
                            list = list.Where(x => x.QuotaID == quotaId).OrderBy(x => x.QuotaID)
                                                                                    .ThenBy(x => x.AdmissionUnitID)
                                                                                    .ThenBy(x => x.QuotaTypeId).ToList();
                            using (var db = new GeneralDataManager())
                            {
                                ddlQuotaType.Items.Clear();
                                DDLHelper.Bind<DAL.QuotaType>(ddlQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == quotaId).ToList(), "Name", "ID", EnumCollection.ListItemType.QuotaType);
                            }

                        }
                        if (quotaTypeId > 0)
                        {
                            list = list.Where(x => x.QuotaTypeId == quotaTypeId).OrderBy(x => x.QuotaID)
                                                                                    .ThenBy(x => x.AdmissionUnitID)
                                                                                    .ThenBy(x => x.QuotaTypeId).ToList();
                        }
                        #endregion



                        panelForFilter.Visible = true;

                        ReportDataSource rds = new ReportDataSource("DataSet1", list);

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));


                        ReportViewer1.LocalReport.SetParameters(param1);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        panelForFilter.Visible = false;

                        ReportDataSource rds = new ReportDataSource("DataSet1");
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = false;

                        MessageView("No Data Fount !!", "fail");

                    }
                }
                else
                {
                    ReportDataSource rds = new ReportDataSource("DataSet1");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.Visible = false;

                    MessageView("Please select Session and Education Category !!", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void filterQuotaInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLoad_Click(null, null);
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ddlQuotaType.Items.Clear();
                ddlQuotaType.Items.Add(new ListItem("Select Quota", "-1"));
                ddlQuotaType.DataBind();
                
                ddlAdmissionUnit.SelectedValue = "-1";
                ddlQuota.SelectedValue = "-1";

                btnLoad_Click(null, null);

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}