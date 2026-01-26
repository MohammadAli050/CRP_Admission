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


namespace Admission.Admission.PostgraduateDiploma.Report
{
    public partial class RPTPostgraduateDiplomaPaidCandidateList : PageBase
    {

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            //uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            //bool isAdmin = PageAuthorization.AdminPageAuthorization(uRole, "RPTPaidCandidateList", _pageUrl);
            //if (isAdmin == false)
            //{
            //    Response.Redirect("~/Admission/Message.aspx?message=Access Denied&type=danger", false);
            //}

            if (!IsPostBack)
            {
                //LoadDDL();
                LoadCandidateData();
            }
        }


        private void LoadCandidateData()
        {
            //long admUnit = -1;
            //int session = -1;
            //int eduCat = -1;

            //admUnit = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            //session = Convert.ToInt32(ddlSession.SelectedValue);
            //eduCat = Convert.ToInt32(ddlEduCat.SelectedValue);

            //messagePanel.Visible = false;

            //if (admUnit > 0 && session > 0 && eduCat > 0)
            //{
            List<DAL.SPRPTGetPostgraduateDiplomaCandidateList_Result> list = null;
            try
            {
                using (var db = new OfficeDataManager())
                {
                    //list = db.AdmissionDB.SPRPTGetPaidCandidatesByAdmUnitIdAcaCalIdEduCatId(admUnit, session, eduCat).ToList();
                    list = db.AdmissionDB.SPRPTGetPostgraduateDiplomaCandidateList().ToList();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unable to get candidate list" + ex.Message + "<br/>" + ex.InnerException.Message;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ReportDataSource rds = new ReportDataSource("DataSet1", list);

                    IList<ReportParameter> param1 = new List<ReportParameter>();
                    param1.Add(new ReportParameter("TotalCandidate", list.Count.ToString()));
                    //param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                    //param1.Add(new ReportParameter("EduCat", ddlEduCat.SelectedItem.Text));

                    ReportViewer1.LocalReport.SetParameters(param1);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.Visible = true;
                }
                else
                {
                    lblMessage.Text = "No data found";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;

                    ReportViewer1.Visible = false;
                }
            }
            else
            {
                lblMessage.Text = "No data found";
                messagePanel.CssClass = "alert alert-warning";
                messagePanel.Visible = true;

                ReportViewer1.Visible = false;
            }
        }
    }

        //private void LoadDDL()
        //{
        //    using (var db = new OfficeDataManager())
        //    {
        //        List<DAL.SPAcademicCalendarGetAll_Result> sessionList = new List<DAL.SPAcademicCalendarGetAll_Result>();
        //        sessionList = db.AdmissionDB.SPAcademicCalendarGetAll(1).ToList();
        //        if (sessionList.Count() > 0)
        //        {
        //            DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, sessionList.OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
        //        }
        //        DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
        //        DDLHelper.Bind<DAL.EducationCategory>(ddlEduCat, db.AdmissionDB.EducationCategories.Where(c => c.ID == 4 || c.ID == 6).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.EducationCategory);
        //    }
        //}

        //protected void btnLoad_Click(object sender, EventArgs e)
        //{
        //    //long admUnit = -1;
        //    //int session = -1;
        //    //int eduCat = -1;

        //    //admUnit = Convert.ToInt64(ddlAdmUnit.SelectedValue);
        //    //session = Convert.ToInt32(ddlSession.SelectedValue);
        //    //eduCat = Convert.ToInt32(ddlEduCat.SelectedValue);

        //    //messagePanel.Visible = false;

        //    //if (admUnit > 0 && session > 0 && eduCat > 0)
        //    //{
        //    //    List<DAL.SPRPTGetPaidCandidatesByAdmUnitIdAcaCalIdEduCatIdV2_Result> list = null;
        //    //    try
        //    //    {
        //    //        using (var db = new OfficeDataManager())
        //    //        {
        //    //            //list = db.AdmissionDB.SPRPTGetPaidCandidatesByAdmUnitIdAcaCalIdEduCatId(admUnit, session, eduCat).ToList();
        //    //            list = db.AdmissionDB.SPRPTGetPaidCandidatesByAdmUnitIdAcaCalIdEduCatIdV2(admUnit, session, eduCat, true).ToList();
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        lblMessage.Text = "Unable to get candidate list" + ex.Message + "<br/>" + ex.InnerException.Message;
        //    //        messagePanel.CssClass = "alert alert-danger";
        //    //        messagePanel.Visible = true;
        //    //    }

        //    //    if (list != null)
        //    //    {
        //    //        if (list.Count > 0)
        //    //        {
        //    //            ReportDataSource rds = new ReportDataSource("DataSet1", list);

        //    //            IList<ReportParameter> param1 = new List<ReportParameter>();
        //    //            param1.Add(new ReportParameter("AdmUnit", ddlAdmUnit.SelectedItem.Text));
        //    //            param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
        //    //            param1.Add(new ReportParameter("EduCat", ddlEduCat.SelectedItem.Text));

        //    //            ReportViewer1.LocalReport.SetParameters(param1);

        //    //            ReportViewer1.LocalReport.DataSources.Clear();
        //    //            ReportViewer1.LocalReport.DataSources.Add(rds);
        //    //            ReportViewer1.Visible = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            lblMessage.Text = "No data found";
        //    //            messagePanel.CssClass = "alert alert-warning";
        //    //            messagePanel.Visible = true;

        //    //            ReportViewer1.Visible = false;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        lblMessage.Text = "No data found";
        //    //        messagePanel.CssClass = "alert alert-warning";
        //    //        messagePanel.Visible = true;

        //    //        ReportViewer1.Visible = false;
        //    //    }
        //    //}
        //}



}
