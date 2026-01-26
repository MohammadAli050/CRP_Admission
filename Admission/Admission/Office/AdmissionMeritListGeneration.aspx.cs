using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DAL;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class AdmissionMeritListGeneration : PageBase
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
                lnkMeritListGenerate.Visible = false;
                lnkProgramAssign.Visible = false;
                divSeat.Visible = false;
                divGrid.Visible = false;
                LoadDDL();
                LoadFacultyDDL();
            }
        }

        private void LoadFilterDDL()
        {
            LoadFilterProgram();
            LoadFilterQuota();
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

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lnkMeritListGenerate.Visible = false;
                lnkProgramAssign.Visible = false;
                ClearGridView();
                LoadCapacity();
                LoadFilterDDL();
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        private void LoadCapacity()
        {
            try
            {
                divSeat.Visible = false;
                gvSeatCapacity.DataSource = null;
                gvSeatCapacity.DataBind();

                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
                int AdmissionSetupId = Convert.ToInt32(ddlFaculty.SelectedValue);

                if (SessionId > 0 && AdmissionSetupId > 0)
                {
                    List<DAL.ProgramWiseTotalSeat> list = new List<DAL.ProgramWiseTotalSeat>();
                    using (var db = new GeneralDataManager())
                    {

                        list = db.AdmissionDB.ProgramWiseTotalSeats.Where(x => x.AcaCalId == SessionId && x.AdmissionSetupId == AdmissionSetupId).ToList();

                        if (list != null && list.Any())
                        {

                            var ProgramList = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();
                            var QuotaList = db.AdmissionDB.Quotas.AsNoTracking().Where(x => x.IsActive == true).ToList();

                            #region MyRegion
                            foreach (var item in list)
                            {
                                try
                                {
                                    int OccupiedSeat = 0;

                                    var ProgramObj = ProgramList.Where(x => x.ProgramID == item.ProgramId).FirstOrDefault();
                                    if (ProgramObj != null)
                                        item.Attribute1 = ProgramObj.ShortName;
                                    else
                                        item.Attribute1 = "";

                                    string QuotaSeat = "";

                                    OccupiedSeat = item.MeritSeat == null ? 0 : Convert.ToInt32(item.MeritSeat);

                                    var QuotaSeatList = db.AdmissionDB.ProgramandQuotaWiseSeats.Where(x => x.ProgramWiseTotalSeatId == item.Id).ToList();

                                    if (QuotaSeatList != null && QuotaSeatList.Any())
                                    {
                                        foreach (var QuotaItem in QuotaSeatList)
                                        {
                                            try
                                            {
                                                OccupiedSeat = OccupiedSeat + Convert.ToInt32(QuotaItem.Seat);

                                                var QuotaObj = QuotaList.Where(x => x.ID == QuotaItem.QuotaId).FirstOrDefault();
                                                if (QuotaObj != null)
                                                {
                                                    if (QuotaSeat == "")
                                                        QuotaSeat = QuotaObj.Remarks.ToString() + " : " + QuotaItem.Seat.ToString();
                                                    else
                                                        QuotaSeat = QuotaSeat + ", " + QuotaObj.Remarks.ToString() + " : " + QuotaItem.Seat.ToString();

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }

                                    item.Attribute2 = QuotaSeat;

                                    item.Attribute3 = OccupiedSeat.ToString();


                                    #region Occupied Seat

                                    item.CreatedBy = 0;

                                    var OccupiedList = db.AdmissionDB.AdmissionTestMeritPositions.Where(x => x.SessionId == SessionId
                                      && x.AdmissionUnitID == AdmissionSetupId && x.ProgramId == item.ProgramId && x.StatusId == 1).ToList();

                                    if (OccupiedList != null && OccupiedList.Any())
                                    {
                                        item.CreatedBy = OccupiedList.Count;
                                    }
                                    else
                                    {
                                        item.CreatedBy = 0;
                                    }

                                    #endregion


                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            #endregion
                        }
                    }

                    if (list != null && list.Any())
                    {
                        divSeat.Visible = true;
                        gvSeatCapacity.DataSource = list.OrderBy(x => x.Attribute1);
                        gvSeatCapacity.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {

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

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkMeritListGenerate.Visible = false;
            lnkProgramAssign.Visible = false;
            ClearGridView();
            LoadFacultyDDL();
            LoadCapacity();
            LoadFilterDDL();
        }

        private void ClearGridView()
        {
            try
            {
                lblTotalCandidate.Text = string.Empty;
                divGrid.Visible = false;
                gvCandidateList.DataSource = null;
                gvCandidateList.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
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

                    if (list != null && list.Any())
                    {
                        if (ProgramId > 0)
                            list = list.Where(x => x.ProgramId == ProgramId).ToList();
                        if (QuotaId >= 0)
                            list = list.Where(x => x.PositionQuotaTypeId == QuotaId).ToList();
                        if (Status >= 0)
                            list = list.Where(x => x.StatusId == Status).ToList();
                        if (EligibleBy == 0)
                            list = list.Where(x => x.PositionQuotaTypeId == 0).ToList();
                        else if (EligibleBy > 0)
                            list = list.Where(x => x.PositionQuotaTypeId > 0).ToList();
                    }

                    if (list != null && list.Any())
                    {
                        lnkMeritListGenerate.Visible = true;
                        divGrid.Visible = true;
                        gvCandidateList.DataSource = list;
                        gvCandidateList.DataBind();

                        Session["CandidateList"] = list;

                        if (list.Where(x => x.MeritPosition != "0").Count() > 0)
                        {
                            lnkProgramAssign.Visible = true;
                        }

                        LoadCapacity();


                        string Msg = "Total Candidate : " + list.Count.ToString();

                        int Eligible = list.Where(x => x.StatusId == 1).Count();
                        int ByMerit = list.Where(x => x.StatusId == 1 && x.PositionQuotaTypeId == 0).Count();
                        int ByQuota = list.Where(x => x.StatusId == 1 && x.PositionQuotaTypeId > 0).Count();

                        if (Eligible > 0)
                            Msg = Msg + " , Total Eligible : " + Eligible;
                        if (ByMerit > 0)
                            Msg = Msg + " , Total Eligible by merit : " + ByMerit;
                        if (ByQuota > 0)
                            Msg = Msg + " , Total Eligible by quota : " + ByQuota;

                        lblTotalCandidate.Text = Msg;

                    }
                    else
                    {
                        showAlert("No data found");
                        gvCandidateList.DataSource = null;
                        gvCandidateList.DataBind();
                        lblTotalCandidate.Text = string.Empty;
                        return;
                    }
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
        protected void gvCandidateList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 1. Set the new page index
            gvCandidateList.PageIndex = e.NewPageIndex;

            // 2. Retrieve the full data list from the Session
            List<DAL.GetCandidateListWithTestRollAndMeritPosition_Result> listToBind =
                Session["CandidateList"] as List<DAL.GetCandidateListWithTestRollAndMeritPosition_Result>;

            // 3. Rebind the GridView
            if (listToBind != null)
            {
                gvCandidateList.DataSource = listToBind;
                gvCandidateList.DataBind();
            }
        }
        protected void lnkMeritListGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
                int UnitId = Convert.ToInt32(ddlFaculty.SelectedValue);

                if (SessionId > 0 && UnitId > 0)
                {
                    int InsertCount = 0;
                    using (var db = new GeneralDataManager())
                    {
                        int UserId = Convert.ToInt32(uId);
                        var returnvalue = db.AdmissionDB.GenerateMeritPosition(SessionId, UnitId, UserId).ToList();

                        if (returnvalue != null && returnvalue.Any())
                            InsertCount = returnvalue.Count();

                    }

                    if (InsertCount > 0)
                    {
                        lnkLoad_Click(null, null);
                        showAlert("Merit Position Generated Successfully");
                        lnkProgramAssign.Visible = true;
                    }
                    else
                    {
                        showAlert("No data found");
                        return;
                    }
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
            finally
            {
                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);
            }
        }

        protected void lnkProgramAssign_Click(object sender, EventArgs e)
        {
            try
            {
                int ProgramAssignCount = 0;

                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
                int UnitId = Convert.ToInt32(ddlFaculty.SelectedValue);

                if (SessionId > 0 && UnitId > 0)
                {


                    using (var db = new GeneralDataManager())
                    {

                        var CandidateList = db.AdmissionDB.AdmissionTestMeritPositions.AsNoTracking().Where(x => x.SessionId == SessionId && x.AdmissionUnitID == UnitId).ToList();


                        List<ProgramWiseSeatInfo> programWiseSeat = new List<ProgramWiseSeatInfo>();
                        List<ProgramQuotaWiseSeatInfo> programQuotaWiseSeat = new List<ProgramQuotaWiseSeatInfo>();


                        #region programQuotaWiseSeat

                        var ProgramWiseSeatList = db.AdmissionDB.ProgramWiseTotalSeats.Where(x => x.AcaCalId == SessionId && x.AdmissionSetupId == UnitId).ToList();

                        if (ProgramWiseSeatList != null && ProgramWiseSeatList.Any())
                        {
                            foreach (var SeatItem in ProgramWiseSeatList)
                            {
                                try
                                {
                                    ProgramWiseSeatInfo NewObj = new ProgramWiseSeatInfo();

                                    NewObj.ProgramId = Convert.ToInt32(SeatItem.ProgramId);
                                    NewObj.TotalSeat = Convert.ToInt32(SeatItem.TotalSeat);
                                    NewObj.MeritSeat = Convert.ToInt32(SeatItem.MeritSeat);
                                    NewObj.OccupiedSeat = 0;
                                    if (CandidateList != null && CandidateList.Any())
                                    {
                                        var AssignCount = CandidateList.Where(x => x.ProgramId == SeatItem.ProgramId
                                         && x.StatusId == 1 && x.PositionQuotaTypeId == 0).ToList();
                                        if (AssignCount != null && AssignCount.Any())
                                        {
                                            NewObj.OccupiedSeat = AssignCount.Count;
                                        }
                                    }

                                    programWiseSeat.Add(NewObj);

                                    var ProgramQuotaWiseList = db.AdmissionDB.ProgramandQuotaWiseSeats.Where(x => x.ProgramWiseTotalSeatId == SeatItem.Id
                                      && (x.Seat != null && x.Seat != 0)).ToList();

                                    if (ProgramQuotaWiseList != null && ProgramQuotaWiseList.Any())
                                    {
                                        foreach (var QuotaItem in ProgramQuotaWiseList)
                                        {
                                            int QuotaId = Convert.ToInt32(QuotaItem.QuotaId);
                                            ProgramQuotaWiseSeatInfo QuotaNewObj = new ProgramQuotaWiseSeatInfo();

                                            QuotaNewObj.ProgramId = Convert.ToInt32(SeatItem.ProgramId);
                                            QuotaNewObj.QuotaId = QuotaId;
                                            QuotaNewObj.TotalSeat = Convert.ToInt32(QuotaItem.Seat);
                                            QuotaNewObj.OccupiedSeat = 0;

                                            if (CandidateList != null && CandidateList.Any())
                                            {
                                                var AssignCount = CandidateList.Where(x => x.ProgramId == SeatItem.ProgramId
                                                 && x.StatusId == 1 && x.PositionQuotaTypeId == QuotaId).ToList();
                                                if (AssignCount != null && AssignCount.Any())
                                                {
                                                    QuotaNewObj.OccupiedSeat = AssignCount.Count;
                                                }
                                            }

                                            programQuotaWiseSeat.Add(QuotaNewObj);

                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        #endregion


                        if (CandidateList != null && CandidateList.Any())
                        {
                            CandidateList = CandidateList.OrderBy(x => x.MeritPosition).ToList();
                            foreach (var candidateItem in CandidateList)
                            {
                                try
                                {
                                    using (var db1 = new GeneralDataManager())
                                    {
                                        int CandidateId = Convert.ToInt32(candidateItem.CandidateId);

                                        var ProgramPriorityList = db1.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == CandidateId && x.AcaCalID == SessionId && x.AdmissionSetupID == UnitId && x.Priority != null).ToList();


                                        if (ProgramPriorityList != null && ProgramPriorityList.Any())
                                        {

                                            int AssignCount = AssignProgramBasedOnProgramPriority(CandidateId, candidateItem, ProgramPriorityList, programWiseSeat, programQuotaWiseSeat);

                                            if (AssignCount > 0)
                                                ProgramAssignCount++;
                                        }
                                        else
                                        {
                                            candidateItem.StatusId = 0;
                                            candidateItem.Remarks = "Program Priority Not Found";
                                            candidateItem.ModifiedBy = Convert.ToInt32(uId);
                                            candidateItem.ModifiedDate = DateTime.Now;

                                            db1.Update<DAL.AdmissionTestMeritPosition>(candidateItem);

                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                }


                            }
                        }

                    }

                    if (ProgramAssignCount > 0)
                    {
                        showAlert("Program assigned successfully");
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
            finally
            {
                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);

            }
        }

        private int AssignProgramBasedOnProgramPriority(int CandidateId, AdmissionTestMeritPosition candidateItem, List<ProgramPriority> ProgramPriorityList, List<ProgramWiseSeatInfo> programWiseSeat, List<ProgramQuotaWiseSeatInfo> programQuotaWiseSeat)
        {
            int ReturnValue = 0;
            try
            {

                ProgramPriorityList = ProgramPriorityList.OrderBy(x => x.Priority).ToList();

                int QuotaId = 0;

                using (var db2 = new GeneralDataManager())
                {
                    var CandidateBasicInfo = db2.AdmissionDB.BasicInfoes.Where(x => x.ID == CandidateId).FirstOrDefault();

                    foreach (var ProgramPriority in ProgramPriorityList)
                    {
                        try
                        {
                            int ProgramId = Convert.ToInt32(ProgramPriority.ProgramID);
                            if (ProgramId > 0)
                            {

                                candidateItem.StatusId = 0;

                                #region Check Merit Seat

                                var MeritObj = programWiseSeat.Where(x => x.ProgramId == ProgramId).FirstOrDefault();

                                if (MeritObj != null && (MeritObj.MeritSeat > MeritObj.OccupiedSeat))
                                {
                                    candidateItem.StatusId = 1;
                                    candidateItem.PositionQuotaTypeId = 0;//Merit
                                    candidateItem.ProgramId = ProgramId;
                                    candidateItem.ProgramPriority = ProgramPriority.Priority;

                                    candidateItem.ModifiedBy = Convert.ToInt32(uId);
                                    candidateItem.ModifiedDate = DateTime.Now;
                                    db2.Update<DAL.AdmissionTestMeritPosition>(candidateItem);

                                    ReturnValue++;

                                    programWiseSeat.Where(w => w.ProgramId == ProgramId).ToList().ForEach(u =>
                                    {
                                        u.OccupiedSeat = u.OccupiedSeat + 1;
                                    });

                                    return ReturnValue;

                                }

                                #endregion

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (ReturnValue == 0)
                    {
                        foreach (var ProgramPriority in ProgramPriorityList)
                        {
                            try
                            {
                                int ProgramId = Convert.ToInt32(ProgramPriority.ProgramID);
                                if (ProgramId > 0)
                                {
                                    candidateItem.StatusId = 0;

                                    #region Check Quota Seat

                                    if (candidateItem.StatusId == 0) // Merit Seat No Available then try Quota
                                    {

                                        if (CandidateBasicInfo.QuotaID != null && CandidateBasicInfo.QuotaID > 0)
                                        {
                                            QuotaId = Convert.ToInt32(CandidateBasicInfo.QuotaID);

                                            var QuotaObj = programQuotaWiseSeat.Where(x => x.ProgramId == ProgramId && x.QuotaId == QuotaId).FirstOrDefault();


                                            if (QuotaObj != null && (QuotaObj.TotalSeat > QuotaObj.OccupiedSeat))
                                            {
                                                candidateItem.StatusId = 1;
                                                candidateItem.PositionQuotaTypeId = QuotaId;
                                                candidateItem.ProgramId = ProgramId;
                                                candidateItem.ProgramPriority = ProgramPriority.Priority;

                                                candidateItem.ModifiedBy = Convert.ToInt32(uId);
                                                candidateItem.ModifiedDate = DateTime.Now;
                                                db2.Update<DAL.AdmissionTestMeritPosition>(candidateItem);

                                                ReturnValue++;

                                                programQuotaWiseSeat.Where(w => w.ProgramId == ProgramId && w.QuotaId == QuotaId).ToList().ForEach(u =>
                                                {
                                                    u.OccupiedSeat = u.OccupiedSeat + 1;
                                                });

                                                return ReturnValue;
                                            }
                                        }


                                    }

                                    #endregion
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }

                    if (ReturnValue == 0)
                    {
                        candidateItem.StatusId = 0;
                        if (QuotaId != 0 && QuotaId != 7)
                            candidateItem.PositionQuotaTypeId = QuotaId;
                        candidateItem.Remarks = "Program Not Assign";
                        candidateItem.ProgramId = ProgramPriorityList.FirstOrDefault().ProgramID;
                        candidateItem.ProgramPriority = ProgramPriorityList.FirstOrDefault().Priority;
                        candidateItem.ModifiedBy = Convert.ToInt32(uId);
                        candidateItem.ModifiedDate = DateTime.Now;

                        db2.Update<DAL.AdmissionTestMeritPosition>(candidateItem);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return ReturnValue;
        }



        protected void ddlProgramFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkLoad_Click(null, null);
        }

        protected void ddlFilterQuota_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkLoad_Click(null, null);
        }

        protected void ddlFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkLoad_Click(null, null);
        }

        protected void ddlFilterEligibleBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkLoad_Click(null, null);
        }


        #region Temporary Object 
        public class ProgramWiseSeatInfo
        {
            public int ProgramId { get; set; }
            public int TotalSeat { get; set; }
            public int MeritSeat { get; set; }
            public int OccupiedSeat { get; set; }
        }

        public class ProgramQuotaWiseSeatInfo
        {
            public int ProgramId { get; set; }
            public int QuotaId { get; set; }
            public int TotalSeat { get; set; }
            public int OccupiedSeat { get; set; }

        }
        #endregion


        #region Excel Download Methods

        protected void lnkDownloadExcel_Click(object sender, EventArgs e)
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

                    if (list != null && list.Any())
                    {
                        if (ProgramId > 0)
                            list = list.Where(x => x.ProgramId == ProgramId).ToList();
                        if (QuotaId >= 0)
                            list = list.Where(x => x.PositionQuotaTypeId == QuotaId).ToList();
                        if (Status >= 0)
                            list = list.Where(x => x.StatusId == Status).ToList();
                    }

                    if (list != null && list.Any())
                    {
                        DataTable dtExcel = ListToDataTableManager.ToDataTable(list);

                        if (dtExcel != null && dtExcel.Rows.Count > 0)
                        {
                            dtExcel.Columns.Remove("CandidateID");
                            dtExcel.Columns.Remove("ProgramId");
                            dtExcel.Columns.Remove("PositionQuotaTypeId");
                            dtExcel.Columns.Remove("StatusId");


                            using (XLWorkbook wb = new XLWorkbook())
                            {

                                IXLWorksheet sheet2;
                                sheet2 = wb.AddWorksheet(dtExcel, "Sheet");

                                sheet2.Table("Table1").ShowAutoFilter = false;

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=" + "CandidateList.xlsx");
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

                        }
                        ;
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

        #endregion



        #region Program Assign new Methods

        // Define a single object to hold all candidate-related data needed for processing.
        public class CandidateAdmissionData
        {
            public DAL.AdmissionTestMeritPosition MeritPositionInfo { get; set; }
            public DAL.BasicInfo BasicInfo { get; set; }
            public List<DAL.ProgramPriority> ProgramPriorities { get; set; }
        }

        // NOTE: ProgramWiseSeatInfo and ProgramQuotaWiseSeatInfo remain the same.
        protected void lnkProgramAssign_ClickNew(object sender, EventArgs e)
        {
            int ProgramAssignCount = 0;
            int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
            int UnitId = Convert.ToInt32(ddlFaculty.SelectedValue);

            if (SessionId <= 0 || UnitId <= 0)
            {
                showAlert("Please select session and faculty");
                // ... (Cookie handling)
                return;
            }

            try
            {
                using (var db = new GeneralDataManager())
                {
                    // --- STEP 1: PRE-FETCH ALL CANDIDATE DATA IN ONE GO ---
                    // 1a. Fetch Candidate Merit Positions
                    var CandidateList = db.AdmissionDB.AdmissionTestMeritPositions.AsNoTracking()
                        .Where(x => x.SessionId == SessionId && x.AdmissionUnitID == UnitId)
                        .OrderBy(x => x.MeritPosition) // Order here for immediate use
                        .ToList();

                    var candidateIds = CandidateList.Select(x => (long?)x.CandidateId).ToList();

                    // 1b. Fetch ALL Program Priorities for ALL candidates
                    var AllProgramPriorities = db.AdmissionDB.ProgramPriorities.AsNoTracking()
                        .Where(x => candidateIds.Contains(x.CandidateID) && x.AcaCalID == SessionId && x.AdmissionSetupID == UnitId && x.Priority != null)
                        .ToList()
                        .GroupBy(x => x.CandidateID) // Group for easy lookup
                        .ToDictionary(g => g.Key, g => g.OrderBy(p => p.Priority).ToList());

                    // 1c. Fetch ALL Basic Infoes for ALL candidates
                    var AllBasicInfoes = db.AdmissionDB.BasicInfoes.AsNoTracking()
                        .Where(x => candidateIds.Contains(x.ID))
                        .ToDictionary(x => x.ID, x => x); // Dictionary for easy lookup

                    // --- STEP 2: LOAD SEAT INFO (Original Logic, but simplified) ---
                    List<ProgramWiseSeatInfo> programWiseSeat = GetProgramWiseSeatInfo(db, SessionId, UnitId, CandidateList);
                    List<ProgramQuotaWiseSeatInfo> programQuotaWiseSeat = GetProgramQuotaWiseSeatInfo(db, SessionId, UnitId, CandidateList);

                    // --- STEP 3: PROCESS ASSIGNMENT IN MEMORY ---
                    if (CandidateList != null && CandidateList.Any())
                    {
                        // Create a list to hold all final updates to minimize database calls
                        List<DAL.AdmissionTestMeritPosition> updatesToPerform = new List<DAL.AdmissionTestMeritPosition>();

                        foreach (var candidateItem in CandidateList) // LOOP 1 (N iterations)
                        {
                            try
                            {
                                int CandidateId = Convert.ToInt32(candidateItem.CandidateId);

                                // Use pre-fetched data
                                var ProgramPriorityList = AllProgramPriorities.ContainsKey(CandidateId)
                                    ? AllProgramPriorities[CandidateId]
                                    : new List<DAL.ProgramPriority>();

                                var BasicInfo = AllBasicInfoes.ContainsKey(CandidateId)
                                    ? AllBasicInfoes[CandidateId]
                                    : null;


                                if (ProgramPriorityList.Any() && BasicInfo != null)
                                {
                                    // Call the simplified processing function
                                    int AssignCount = AssignProgramBasedOnProgramPriority_Optimized(
                                        candidateItem, ProgramPriorityList, BasicInfo, programWiseSeat, programQuotaWiseSeat);

                                    if (AssignCount > 0)
                                    {
                                        ProgramAssignCount++;
                                        updatesToPerform.Add(candidateItem);
                                    }
                                    else
                                    {
                                        updatesToPerform.Add(candidateItem);
                                    }
                                }
                                else
                                {
                                    // Mark for update if priority/basic info is missing
                                    candidateItem.StatusId = 0;
                                    candidateItem.Remarks = ProgramPriorityList.Any() ? "Basic Info Not Found" : "Program Priority Not Found";
                                    candidateItem.ModifiedBy = Convert.ToInt32(uId); // Assuming uId is defined
                                    candidateItem.ModifiedDate = DateTime.Now;
                                    updatesToPerform.Add(candidateItem);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Log the exception
                            }
                        }

                        DataTable dataNeedtoUpdate = ListToDataTableManager.ToDataTable(updatesToPerform);

                        // --- STEP 4: BULK DATABASE UPDATE ---
                        // Only update the records that were changed (assigned or marked not assigned)

                        List<SqlParameter> parameters = new List<SqlParameter>();
                        // Add the TVP
                        parameters.Add(new SqlParameter
                        {
                            ParameterName = "@AdmissionData",
                            SqlDbType = System.Data.SqlDbType.Structured,
                            Value = dataNeedtoUpdate, // IMPORTANT: Ensure 'marksToInsert' is converted to a DataTable if your GetDataFromQuery expects it.
                            TypeName = "dbo.AdmissionTestMeritPositionType"
                        });

                        DataTable dtUpdatedList = DataTableManager.GetDataFromQuery("BulkUpdate_AdmissionTestMeritPosition", parameters);

                        showAlert("Program Assigned Successfully.Please click Load button to see the updates");
                        return;


                    }
                }

                // ... (Post-processing/alert)
            }
            catch (Exception ex)
            {
                // ... (Exception handling)
            }
            finally
            {
                // ... (Cookie handling)
            }
        }

        private int AssignProgramBasedOnProgramPriority_Optimized(
    DAL.AdmissionTestMeritPosition candidateItem,
    List<DAL.ProgramPriority> ProgramPriorityList,
    DAL.BasicInfo CandidateBasicInfo, // Pass BasicInfo directly
    List<ProgramWiseSeatInfo> programWiseSeat,
    List<ProgramQuotaWiseSeatInfo> programQuotaWiseSeat)
        {
            int ReturnValue = 0;
            int QuotaId = 0;

            if (candidateItem.CandidateId == 658120)
            {

            }

            // --- PHASE 1: Try to assign by MERIT ---
            foreach (var ProgramPriority in ProgramPriorityList)
            {
                int ProgramId = Convert.ToInt32(ProgramPriority.ProgramID);
                if (ProgramId <= 0) continue;

                // Check Merit Seat in IN-MEMORY list
                var MeritObj = programWiseSeat.FirstOrDefault(x => x.ProgramId == ProgramId);

                if (MeritObj != null && (MeritObj.MeritSeat > MeritObj.OccupiedSeat))
                {
                    candidateItem.StatusId = 1;
                    candidateItem.PositionQuotaTypeId = 0; // Merit
                    candidateItem.ProgramId = ProgramId;
                    candidateItem.ProgramPriority = ProgramPriority.Priority;
                    candidateItem.Remarks = "";
                    // **Increment OccupiedSeat in memory**
                    MeritObj.OccupiedSeat += 1;

                    ReturnValue = 1;
                    return ReturnValue; // Exit immediately upon assignment
                }
            }

            // --- PHASE 2: If no Merit seat, try to assign by QUOTA ---
            if (ReturnValue == 0)
            {
                foreach (var ProgramPriority in ProgramPriorityList)
                {
                    int ProgramId = Convert.ToInt32(ProgramPriority.ProgramID);
                    if (ProgramId <= 0) continue;

                    if (CandidateBasicInfo.QuotaID != null && CandidateBasicInfo.QuotaID > 0)
                    {
                        QuotaId = Convert.ToInt32(CandidateBasicInfo.QuotaID);

                        // Check Quota Seat in IN-MEMORY list
                        var QuotaObj = programQuotaWiseSeat.FirstOrDefault(x => x.ProgramId == ProgramId && x.QuotaId == QuotaId);

                        if (QuotaObj != null && (QuotaObj.TotalSeat > QuotaObj.OccupiedSeat))
                        {
                            candidateItem.StatusId = 1;
                            candidateItem.PositionQuotaTypeId = QuotaId;
                            candidateItem.ProgramId = ProgramId;
                            candidateItem.ProgramPriority = ProgramPriority.Priority;
                            candidateItem.Remarks = "";
                            // **Increment OccupiedSeat in memory**
                            QuotaObj.OccupiedSeat += 1;

                            ReturnValue = 1;
                            return ReturnValue; // Exit immediately upon assignment
                        }
                    }
                }
            }

            // --- PHASE 3: If still not assigned, mark as unassigned ---
            if (ReturnValue == 0)
            {
                candidateItem.StatusId = 0;
                if (QuotaId != 0 && QuotaId != 7)
                    candidateItem.PositionQuotaTypeId = QuotaId;

                candidateItem.Remarks = "Program Not Assign";
                candidateItem.ProgramId = ProgramPriorityList.FirstOrDefault()?.ProgramID; // Use Null-conditional
                candidateItem.ProgramPriority = ProgramPriorityList.FirstOrDefault()?.Priority; // Use Null-conditional
            }

            // NOTE: The update to the database is now handled in the calling function via BulkUpdate.

            return ReturnValue;
        }

        private List<ProgramWiseSeatInfo> GetProgramWiseSeatInfo(GeneralDataManager db, int sessionId, int unitId, List<DAL.AdmissionTestMeritPosition> candidateList)
        {
            var programWiseSeat = new List<ProgramWiseSeatInfo>();
            var ProgramWiseSeatList = db.AdmissionDB.ProgramWiseTotalSeats
                .Where(x => x.AcaCalId == sessionId && x.AdmissionSetupId == unitId)
                .ToList();

            if (ProgramWiseSeatList.Any())
            {
                foreach (var SeatItem in ProgramWiseSeatList)
                {
                    var NewObj = new ProgramWiseSeatInfo
                    {
                        ProgramId = Convert.ToInt32(SeatItem.ProgramId),
                        TotalSeat = Convert.ToInt32(SeatItem.TotalSeat),
                        MeritSeat = Convert.ToInt32(SeatItem.MeritSeat),
                        OccupiedSeat = 0 // Will be calculated below
                    };

                    // This is the one place we check for initial occupied seats
                    if (candidateList.Any())
                    {
                        NewObj.OccupiedSeat = candidateList.Count(x =>
                            x.ProgramId == SeatItem.ProgramId &&
                            x.StatusId == 1 &&
                            x.PositionQuotaTypeId == 0);
                    }

                    programWiseSeat.Add(NewObj);
                }
            }
            return programWiseSeat;
        }

        private List<ProgramQuotaWiseSeatInfo> GetProgramQuotaWiseSeatInfo(GeneralDataManager db, int sessionId, int unitId, List<DAL.AdmissionTestMeritPosition> candidateList)
        {
            var programQuotaWiseSeat = new List<ProgramQuotaWiseSeatInfo>();

            // Efficiently fetch all quota seats for the session/unit programs
            var programWiseSeatIds = db.AdmissionDB.ProgramWiseTotalSeats
                .Where(x => x.AcaCalId == sessionId && x.AdmissionSetupId == unitId)
                .Select(x => x.Id)
                .ToList();

            var ProgramQuotaWiseList = db.AdmissionDB.ProgramandQuotaWiseSeats
                .Where(x => programWiseSeatIds.Contains(x.ProgramWiseTotalSeatId.Value) && (x.Seat != null && x.Seat != 0))
                .ToList();

            // Map ProgramID back to the quota seat
            var ProgramIdMap = db.AdmissionDB.ProgramWiseTotalSeats
                .Where(x => programWiseSeatIds.Contains(x.Id))
                .ToDictionary(x => x.Id, x => x.ProgramId);


            if (ProgramQuotaWiseList.Any())
            {
                foreach (var QuotaItem in ProgramQuotaWiseList)
                {
                    var programId = ProgramIdMap.ContainsKey(QuotaItem.ProgramWiseTotalSeatId.Value)
                        ? ProgramIdMap[QuotaItem.ProgramWiseTotalSeatId.Value]
                        : 0;

                    int QuotaId = Convert.ToInt32(QuotaItem.QuotaId);
                    var QuotaNewObj = new ProgramQuotaWiseSeatInfo
                    {
                        ProgramId = Convert.ToInt32(programId),
                        QuotaId = QuotaId,
                        TotalSeat = Convert.ToInt32(QuotaItem.Seat),
                        OccupiedSeat = 0
                    };

                    // This is the one place we check for initial occupied quota seats
                    if (candidateList.Any())
                    {
                        QuotaNewObj.OccupiedSeat = candidateList.Count(x =>
                            x.ProgramId == programId &&
                            x.StatusId == 1 &&
                            x.PositionQuotaTypeId == QuotaId);
                    }

                    programQuotaWiseSeat.Add(QuotaNewObj);
                }
            }
            return programQuotaWiseSeat;
        }

        #endregion


    }
}