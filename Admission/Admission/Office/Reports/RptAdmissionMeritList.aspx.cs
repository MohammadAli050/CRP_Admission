using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.Reports
{
    public partial class RptAdmissionMeritList : PageBase
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
                LoadFacultyDDL();
                LoadFilterProgram();
                LoadFilterQuota();
            }
        }
        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }
        private void LoadFacultyDDL()
        {
            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                List<DAL.AdmissionSetup> admSetup = new List<DAL.AdmissionSetup>();

                ddlFaculty.Items.Clear();
                ddlFaculty.Items.Add(new ListItem("-Select-", "0"));
                ddlFaculty.AppendDataBoundItems = true;

                using (var db = new OfficeDataManager())
                {
                    admSetup = db.GetAllAdmissionSetup_AD();
                    admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive == true).ToList();  // only for Bachelors

                    if (admSetup != null && admSetup.Any())
                    {
                        foreach (var item in admSetup)
                        {
                            var admUnit = db.GetAllAdmissionUnit().Where(x => x.ID == item.AdmissionUnitID).FirstOrDefault();
                            ddlFaculty.Items.Add(new ListItem(admUnit.UnitName, item.ID.ToString()));
                        }
                    }

                }

            }
            catch (Exception ex)
            {
            }
        }
        private void LoadFilterQuota()
        {
            try
            {

                ddlFilterQuota.Items.Clear();
                ddlFilterQuota.AppendDataBoundItems = true;

                using (var db = new GeneralDataManager())
                {

                    var List = db.AdmissionDB.Quotas.AsNoTracking().Where(x => x.IsActive == true).ToList();

                    ddlFilterQuota.Items.Add(new ListItem("-All-", "-5"));

                    if (List != null && List.Any())
                    {
                        ddlFilterQuota.DataTextField = "Remarks";
                        ddlFilterQuota.DataValueField = "ID";

                        ddlFilterQuota.DataSource = List.OrderBy(a => a.OrderQuota);
                        ddlFilterQuota.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        private void LoadFilterProgram()
        {
            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int facultyId = Convert.ToInt32(ddlFaculty.SelectedValue);

                List<DAL.AdmissionUnitProgram> admUp = new List<DAL.AdmissionUnitProgram>();
                List<DAL.AdmissionSetup> admSetup = new List<DAL.AdmissionSetup>();


                ddlProgramFilter.Items.Clear();
                ddlProgramFilter.Items.Add(new ListItem("-All-", "0"));
                ddlProgramFilter.AppendDataBoundItems = true;


                if (acaCalId > 0 && facultyId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        var setUp = db.AdmissionDB.AdmissionSetups.Where(c => c.ID == facultyId).FirstOrDefault();
                        admUp = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == setUp.AdmissionUnitID && c.IsActive == true && c.EducationCategoryID == 4).ToList();

                        if (admUp != null && admUp.Count > 0)
                        {
                            foreach (var item in admUp)
                            {
                                ddlProgramFilter.Items.Add(new ListItem(item.ShortCode, item.ProgramID.ToString()));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilterProgram();
            LoadFacultyDDL();
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilterProgram();
        }

        protected void ddlProgramFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddlFilterQuota_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddlFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddlFilterEligibleBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void lnkLoad_Click(object sender, EventArgs e)
        {
            try
            {

                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
                int UnitId = Convert.ToInt32(ddlFaculty.SelectedValue);

                if (SessionId > 0 && UnitId > 0)
                {
                    List<DAL.GetCandidateListWithTestRollAndMeritPosition_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.GetCandidateListWithTestRollAndMeritPosition(SessionId, UnitId).ToList();
                    }

                    int ProgramId = Convert.ToInt32(ddlProgramFilter.SelectedValue);
                    int QuotaId = Convert.ToInt32(ddlFilterQuota.SelectedValue);
                    int Status = Convert.ToInt32(ddlFilterStatus.SelectedValue);
                    int EligibleBy = Convert.ToInt32(ddlFilterEligibleBy.SelectedValue);

                }
                else
                {
                    showAlert("Please select session and faculty");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void lnkDownloadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
                int UnitId = Convert.ToInt32(ddlFaculty.SelectedValue);
                int ProgramId = Convert.ToInt32(ddlProgramFilter.SelectedValue);
                int QuotaId = Convert.ToInt32(ddlFilterQuota.SelectedValue);
                int Status = Convert.ToInt32(ddlFilterStatus.SelectedValue);
                int SelectedBy = Convert.ToInt32(ddlFilterEligibleBy.SelectedValue);

                if (SessionId > 0 && UnitId > 0)
                {
                    List<DAL.GetAdmissitonMeritListForExcelDownload_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.GetAdmissitonMeritListForExcelDownload(SessionId, UnitId, ProgramId, QuotaId, Status, SelectedBy).ToList();
                    }

                    if (list != null && list.Any())
                    {
                        DataTable dtExcel = ListToDataTableManager.ToDataTable(list);

                        if (dtExcel != null && dtExcel.Rows.Count > 0)
                        {

                            using (XLWorkbook wb = new XLWorkbook())
                            {

                                IXLWorksheet sheet2;
                                sheet2 = wb.AddWorksheet(dtExcel, "Sheet");

                                sheet2.Table("Table1").ShowAutoFilter = false;

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=" + "MeritList.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);

                                    HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                                    cookie.Value = "Flag";
                                    cookie.Expires = DateTime.Now.AddDays(1);
                                    Response.AppendCookie(cookie);

                                    Response.Flush();
                                    Response.SuppressContent = true;
                                }
                            }

                        };
                    }
                    else
                    {
                        showAlert("No data found");
                        HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                        cookie.Value = "Flag";
                        cookie.Expires = DateTime.Now.AddDays(1);
                        Response.AppendCookie(cookie);
                        return;
                    }
                }
                else
                {
                    showAlert("Please select session and faculty");
                    HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                    cookie.Value = "Flag";
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.AppendCookie(cookie);
                    return;
                }
            }
            catch (Exception ex)
            {
                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);
            }
        }
    }
}