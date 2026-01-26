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
    public partial class RPTPaidandFinalSumbitReport : PageBase
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

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);

            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSessionIsSelected();
            clearReport();
        }

        protected void ddlAdmUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSessionIsSelected();
            clearReport();
        }

        protected void CheckSessionIsSelected()
        {
            MessageView("", "clear");

            try
            {
                if (Convert.ToInt32(ddlSession.SelectedValue) == -1)
                {
                    MessageView("Please Select Session!", "fail");
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                clearReport();
                int AdmissionUnitId = Convert.ToInt32(ddlAdmUnit.SelectedValue);
                int AcacalId = Convert.ToInt32(ddlSession.SelectedValue);
                int SubmitStatus = Convert.ToInt32(ddlSubmitStatus.SelectedValue);
                int PaymentStatus = Convert.ToInt32(ddlPaymentStatus.SelectedValue);

                if(AcacalId>0)
                {
                    List<DAL.GetPaidandFinalSumbittedCandidateListReport_Result> list = null;
                    using (var db = new OfficeDataManager())
                    {
                        list = db.AdmissionDB.GetPaidandFinalSumbittedCandidateListReport(AdmissionUnitId, AcacalId,SubmitStatus,PaymentStatus).ToList();
                    }

                    if(list!=null && list.Any())
                    {
                        ReportViewer1.LocalReport.EnableExternalImages = true;


                        ReportDataSource rds = new ReportDataSource("DataSet1", list.ToList());
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Admission/Office/Reports/RPTPaidandFinalSumbitReport.rdlc");

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("AdmUnit", ddlAdmUnit.SelectedItem.Text));
                        param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                        param1.Add(new ReportParameter("SS", ddlSubmitStatus.SelectedItem.Text));
                        param1.Add(new ReportParameter("PS", ddlPaymentStatus.SelectedItem.Text));
                        param1.Add(new ReportParameter("Count", list.Count.ToString()));

                        ReportViewer1.LocalReport.SetParameters(param1);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;


                    }

                }
                else
                {
                    MessageView("Please Select Session!", "fail");
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void clearReport()
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.Visible = false;
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

        protected void ddlSubmitStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearReport();
        }

        protected void ddlPaymentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearReport();
        }
    }
}