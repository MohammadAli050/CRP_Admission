using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Admission.Admission.Office.Reports
{
    public partial class RPTCampusWiseSeatPlanAllCampus : PageBase
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
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.Campu>(ddlCampus, db.AdmissionDB.Campus.Where(c => c.IsActive == true).ToList(), "CampusName", "ID", EnumCollection.ListItemType.SelectAll);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;
            int campusId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            campusId = Convert.ToInt32(ddlCampus.SelectedValue);

            if (admUnitId > 0 && acaCalId > 0)
            {
                List<DAL.SPRptGetSeatPlanByAcaCalIDAdmUnitID_Result> list = null;
                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        if (campusId == -1)
                        {
                            list = db.AdmissionDB.SPRptGetSeatPlanByAcaCalIDAdmUnitID(acaCalId, admUnitId)
                            .Where(c => !string.IsNullOrEmpty(c.endRoll) &&
                                    !string.IsNullOrEmpty(c.startRoll))
                            .ToList();
                        }
                        else if (campusId > 0)
                        {
                            list = db.AdmissionDB.SPRptGetSeatPlanByAcaCalIDAdmUnitID(acaCalId, admUnitId)
                            .Where(c => !string.IsNullOrEmpty(c.endRoll) &&
                                    !string.IsNullOrEmpty(c.startRoll) && c.campusID == campusId)
                            .ToList();
                             //
                        }
                        else if (campusId == 0)
                        {
                            list = db.AdmissionDB.SPRptGetSeatPlanByAcaCalIDAdmUnitID(acaCalId, admUnitId)
                            .Where(c => !string.IsNullOrEmpty(c.endRoll) &&
                                    !string.IsNullOrEmpty(c.startRoll))
                            .ToList();
                            //&&c.campusID == campusId
                        }

                    }

                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == acaCalId &&
                                c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading seat plan";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }

                if (list != null && admSetup != null)
                {
                    if (list.Count > 0)
                    {
                        lblMessage.Text = "";
                        messagePanel.Visible = false;


                        ReportViewer1.Visible = true;

                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        ReportDataSource rds = new ReportDataSource("DataSet1", list);

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text));
                        //param1.Add(new ReportParameter("CentreName", ddlCampus.SelectedItem.Text));
                        param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                        param1.Add(new ReportParameter("ExamDate", Convert.ToDateTime(admSetup.ExamDate).ToString("dd/MM/yyyy")));
                        param1.Add(new ReportParameter("ExamTime", admSetup.ExamTime.ToString()));

                        ReportViewer1.LocalReport.SetParameters(param1);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        ReportViewer1.Visible = false;

                        lblMessage.Text = "No Data Found";
                        messagePanel.CssClass = "alert alert-warning";
                        messagePanel.Visible = true;
                    }
                }
            }
        }

    }
}