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
    public partial class OfficeHome : PageBase
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
                LoadStatisticsToday();
                LoadStatisticsThisMonth();
                LoadAdmissionStatus();
            }

            //string msg = "Hello World";
            //string title = "Hello";
            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrSuccess", "toastrSuccess('" + msg + "','" + title + "');", true);
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.EducationCategory>(ddlEducation, db.AdmissionDB.EducationCategories.Where(x => x.IsActive == true).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.Select);
            }
        }

        private void LoadAdmissionStatus()
        {
            using (var db = new OfficeDataManager())
            {
                var schoolCount = db.AdmissionDB.AdmissionUnits
                    .Where(c => c.IsActive == true).ToList().Count();
                lblNoSchoolsOpen.Text = schoolCount.ToString();

                var programCount = db.AdmissionDB.AdmissionUnitPrograms
                    .Where(c => c.IsActive == true).ToList().Count();
                lblNoProgAvailable.Text = programCount.ToString();

                var examCount = db.AdmissionDB.AdmissionSetups
                    .Where(c => c.IsActive == true).ToList().Count();
                lblNoAdmissionOpen.Text = examCount.ToString();

                var activeExpiredAdmCount = db.GetAllActiveAdmissionSetup_AD(true)
                    .Where(a => a.EndDate <= DateTime.Now)
                    .OrderBy(a => a.AdmissionUnit.UnitName)
                    .ToList().Count();
                lblActiveExpiredAdmission.Text = activeExpiredAdmCount.ToString();
                if (activeExpiredAdmCount > 0)
                {
                    lblActiveExpiredAdmission.Font.Bold = true;
                    lblActiveExpiredAdmission.CssClass = "label label-danger";
                }
                else
                {
                    lblActiveExpiredAdmission.ForeColor = Color.Green;
                    lblActiveExpiredAdmission.CssClass = "";
                }
            }
        }

        private void LoadStatisticsThisMonth()
        {
            //who applied
            using (var db = new OfficeDataManager())
            {
                var appliedTodayCount = db.AdmissionDB.CandidateFormSls
                    .Where(c => c.DateCreated.Month == DateTime.Today.Month && c.DateCreated.Year == DateTime.Today.Year).ToList().Count();

                if (appliedTodayCount > 0)
                {
                    lblNoAppliedThisMonth.Text = appliedTodayCount.ToString();
                }
                else
                {
                    lblNoAppliedThisMonth.Text = "0";
                }

            }

            //who paid
            using (var db = new OfficeDataManager())
            {
                var paidTodayCount = db.AdmissionDB.CandidatePayments
                    .Where(c => c.DateModified.Value.Month == DateTime.Today.Month &&
                            c.DateModified.Value.Year == DateTime.Today.Year &&
                            c.IsPaid == true).ToList().Count();

                if (paidTodayCount > 0)
                {
                    lblNoPaidThisMonth.Text = paidTodayCount.ToString();
                }
                else
                {
                    lblNoPaidThisMonth.Text = "0";
                }

            }
        }

        private void LoadStatisticsToday()  // for panel Today
        {
            //who applied
            using (var db = new OfficeDataManager())
            {
                var appliedTodayCount = db.AdmissionDB.CandidateFormSls
                    .Where(c => c.DateCreated.Day == DateTime.Today.Day && c.DateCreated.Month == DateTime.Today.Month && c.DateCreated.Year == DateTime.Today.Year).ToList().Count();

                if (appliedTodayCount > 0)
                {
                    lblNoAppliedToday.Text = appliedTodayCount.ToString();
                }
                else
                {
                    lblNoAppliedToday.Text = "0";
                }

            }

            //who paid
            using (var db = new OfficeDataManager())
            {
                var paidTodayCount = db.AdmissionDB.CandidatePayments
                    .Where(c => c.DateModified.Value.Day == DateTime.Today.Day &&
                        c.DateModified.Value.Month == DateTime.Today.Month &&
                        c.DateModified.Value.Year == DateTime.Today.Year &&
                        c.IsPaid == true).ToList().Count();

                if (paidTodayCount > 0)
                {
                    lblNoPaidToday.Text = paidTodayCount.ToString();
                }
                else
                {
                    lblNoPaidToday.Text = "0";
                }

            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            int eduCatId = Convert.ToInt32(ddlEducation.SelectedValue);

            if (acaCalId > 0 && eduCatId > 0)
            {
                LoadAdmissionCandidatePaidCount(acaCalId, eduCatId);
            }
            else
            {
                lvCandidatePaidCountSchoolWise.Visible = false;
            }
        }

        private void LoadAdmissionCandidatePaidCount(int acaCalId, int eduCatId)
        {
            using (var db = new OfficeDataManager())
            {
                //List<DAL.SPGetCandidatePaidCountSchoolWise_Result> list = db.AdmissionDB.SPGetCandidatePaidCountSchoolWise(true,acaCalId).ToList();
                List<DAL.SPGetCandidatePaidApprovedAppearedCountList_Result> list = db.AdmissionDB.SPGetCandidatePaidApprovedAppearedCountList(acaCalId, eduCatId).ToList();
                if (list != null && list.Count() > 0)
                {
                    lvCandidatePaidCountSchoolWise.DataSource = list.OrderBy(x => x.FacultyName).ToList();
                    lvCandidatePaidCountSchoolWise.Visible = true;

                    panelShowSessionTotal.Visible = true;
                    lblSessionName.Text = ddlSession.SelectedItem.Text;
                    lblSessionTotal.Text = (list.Sum(c => c.TotalPaidCount) + list.Sum(c => c.TotalNotSubmit)).ToString();
                    lbltotalCandidate.Text = list.Sum(c => c.TotalNotSubmit).ToString();
                    lblTotalCandidateFinal.Text = list.Sum(c => c.TotalPaidCount).ToString();
                    lblTotalAppearedCandidateCount.Text = list.Sum(c => c.AppearedCount).ToString();
                    lblTotalApprevedCandidateCount.Text = list.Sum(c => c.ApprovedCount).ToString();

                }
                else
                {
                    lvCandidatePaidCountSchoolWise.DataSource = null;

                    panelShowSessionTotal.Visible = true;
                    lblSessionName.Text = ddlSession.SelectedItem.Text;
                    lblSessionTotal.Text = "No candidates found";
                    lbltotalCandidate.Text = "";
                    lblTotalCandidateFinal.Text = "";
                    lblTotalAppearedCandidateCount.Text = "";
                    lblTotalApprevedCandidateCount.Text = "";
                }
                lvCandidatePaidCountSchoolWise.DataBind();
            }
        }

        protected void lvCandidatePaidCountSchoolWise_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            int TotalCand = 0;
            int TotalFinalSub = 0;
            int TotalAppreved = 0;
            int TotalAppeared = 0;

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;

                //DAL.SPGetCandidatePaidCountSchoolWise_Result item = (DAL.SPGetCandidatePaidCountSchoolWise_Result)((ListViewDataItem)(e.Item)).DataItem;
                DAL.SPGetCandidatePaidApprovedAppearedCountList_Result item = (DAL.SPGetCandidatePaidApprovedAppearedCountList_Result)((ListViewDataItem)(e.Item)).DataItem;

                //Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
                Label lblTotalPaidCount = (Label)currentItem.FindControl("lblTotalPaidCount");
                Label lblPaidCandidateCount = (Label)currentItem.FindControl("lblPaidCandidateCount");
                Label lblApprevedCandidateCount = (Label)currentItem.FindControl("lblApprevedCandidateCount");
                Label lblAppearedCandidateCount = (Label)currentItem.FindControl("lblAppearedCandidateCount");

                if (item != null)
                {
                    lblUnitName.Text = item.FacultyName;
                    lblTotalPaidCount.Text = item.TotalNotSubmit.ToString();
                    lblPaidCandidateCount.Text = item.TotalPaidCount.ToString();
                    lblApprevedCandidateCount.Text = item.ApprovedCount.ToString();
                    lblAppearedCandidateCount.Text = item.AppearedCount.ToString();

                    TotalCand = TotalCand + item.TotalNotSubmit.GetValueOrDefault();
                    TotalFinalSub = TotalFinalSub + item.TotalPaidCount.GetValueOrDefault();
                    TotalAppeared = TotalAppeared + item.AppearedCount.GetValueOrDefault();
                    TotalAppreved = TotalAppreved + item.ApprovedCount.GetValueOrDefault();


                    //if (item.Number_applied == null || item.Number_applied == 0)
                    if (item.TotalPaidCount == null || item.TotalPaidCount == 0)
                    {
                        //lblUnitName.ForeColor = Color.Crimson;
                        lblPaidCandidateCount.ForeColor = Color.Crimson;
                    }

                    if (item.ApprovedCount == null || item.ApprovedCount == 0)
                    {
                        lblApprevedCandidateCount.ForeColor = Color.Crimson;
                    }

                    if (item.AppearedCount == null || item.AppearedCount == 0)
                    {
                        lblAppearedCandidateCount.ForeColor = Color.Crimson;
                    }
                }
                
            }


        }


    }
}