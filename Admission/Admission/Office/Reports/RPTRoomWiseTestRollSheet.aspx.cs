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
    public partial class RPTRoomWiseTestRollSheet : PageBase
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

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");
            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
            }

        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            using (var db = new OfficeDataManager())
            {
                List<DAL.Building> list = null;
                list = db.AdmissionDB.Buildings.ToList();

                if (list != null)
                {
                    DDLHelper.Bind<DAL.Building>(ddlBuilding, list.ToList(), "BuildingName", "ID", EnumCollection.ListItemType.Select);
                }
            }

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string PrintData(string AdmUnit,string Session,string Building)
        {
            int admUnitId = -1;
            int acaCalId = -1;
            int buildingsId = -1;

            admUnitId = Convert.ToInt32(AdmUnit);
            acaCalId = Convert.ToInt32(Session);
            buildingsId = Convert.ToInt32(Building);

            return "ADM UNIT";
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            int admUnitId = -1;
            int acaCalId = -1;
            int buildingsId = -1;

            admUnitId = Convert.ToInt32(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            buildingsId = Convert.ToInt32(ddlBuilding.SelectedValue);

            if (admUnitId > 0 && acaCalId > 0 && buildingsId > 0)
            {

                List<DAL.SPRPTRoomWiseTestRollSheet_Result> rwtrsList = null;
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        rwtrsList = db.AdmissionDB.SPRPTRoomWiseTestRollSheet(admUnitId, acaCalId, buildingsId).ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageView(ex.Message.ToString(), "fail");
                }
                

                if (rwtrsList != null)
                {

                    if (rwtrsList.Count() > 0)
                    {
                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        ReportDataSource rds = new ReportDataSource("DataSet1", rwtrsList);

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("CampusName", rwtrsList.FirstOrDefault().CampusName));
                        param1.Add(new ReportParameter("BuildingName", rwtrsList.FirstOrDefault().BuildingName));
                        param1.Add(new ReportParameter("FacultyName", ddlAdmUnit.SelectedItem.Text));

                        ReportViewer1.LocalReport.SetParameters(param1);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        MessageView("No Data Found !!", "fail");
                    }


                }
            }
            else
            {
                MessageView("Please Select Faculty, Session and Buildings for Load Data !!", "fail");
            }
            
        }

        //protected void btnPrint_Click(object sender, EventArgs e)
        //{

        //}
    }
}