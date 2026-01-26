using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTApprovedCandidatesWithTestRoll : PageBase
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
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            if(admUnitId > 0 && acaCalId > 0)
            {
                try
                {
                    List<DAL.SPGetApprovedCandidatesWithTestRollByAdmUnitIDAcaCalID_Result> list = null;
                    using (var db = new OfficeDataManager())
                    {
                        list = db.AdmissionDB.SPGetApprovedCandidatesWithTestRollByAdmUnitIDAcaCalID(admUnitId, acaCalId).ToList();
                    }

                    if(list != null)
                    {
                        if(list.Count() > 0)
                        {
                            ReportViewer1.LocalReport.EnableExternalImages = true;
                            

                            ReportDataSource rds = new ReportDataSource("DataSet1", list.ToList());

                            IList<ReportParameter> param1 = new List<ReportParameter>();
                            param1.Add(new ReportParameter("AdmUnit", ddlAdmUnit.SelectedItem.Text));
                            param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                            param1.Add(new ReportParameter("Count", list.Count.ToString()));

                            ReportViewer1.LocalReport.SetParameters(param1);

                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        else
                        {
                            ReportDataSource rds = new ReportDataSource("DataSet1", list);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            } //end ifif(admUnitId > 0 && acaCalId > 0)
        }
    }
}