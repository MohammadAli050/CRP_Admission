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
    public partial class RPTStatusReport : PageBase
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
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);

            }
        }


        protected void btnLoadData_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                if (Convert.ToInt64(ddlSession.SelectedValue) > 0)
                {
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                    long? facultyId = null;
                    string facultyName = "---";
                    if (!string.IsNullOrEmpty(ddlAdmUnit.SelectedValue) && Convert.ToInt64(ddlAdmUnit.SelectedValue) > 0)
                    {
                        facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                        facultyName = ddlAdmUnit.SelectedItem.Text;
                    }


                    List<DAL.SPStatusReport_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPStatusReport(acaCalId,
                                                                 facultyId,
                                                                 null,
                                                                 true,
                                                                 null).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        ReportViewer1.LocalReport.EnableExternalImages = true;
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportDataSource rds = new ReportDataSource("DataSet1", list);
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Admission/Office/Reports/RPTStatusReport.rdlc");

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                        param1.Add(new ReportParameter("Faculty", facultyName));
                        //param1.Add(new ReportParameter("ExamDateTime", Convert.ToDateTime(list.FirstOrDefault().ExamDate).ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().ExamTime)); //
                        //param1.Add(new ReportParameter("CampusName", ""));
                        //param1.Add(new ReportParameter("BuildingName", ""));
                        //param1.Add(new ReportParameter("RoomName", ""));
                        //param1.Add(new ReportParameter("RoomCap", ""));

                        ReportViewer1.LocalReport.SetParameters(param1);
                        ReportViewer1.LocalReport.DisplayName = "StatusReport" + ddlSession.SelectedItem.Text.ToString();
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
                    MessageView("Please select Session for load!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }
    }
}