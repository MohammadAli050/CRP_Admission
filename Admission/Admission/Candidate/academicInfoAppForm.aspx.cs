using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class academicInfoAppForm : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);


            using (var db = new CandidateDataManager())
            {
                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                    //paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID && x.IsPaid == true).Select(x => x.PaymentId).FirstOrDefault();
                }// end if(obj != null && obj.ID > 0)
            }// end using

            if (!IsPostBack)
            {

                using (var db = new CandidateDataManager())
                {
                    var AdditionObj = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId && x.IsFinalSubmit == true).FirstOrDefault();

                    if (AdditionObj == null)
                    {
                        divMain.Visible = true;
                        divFinalSubmit.Visible = false;

                    }
                    else
                    {
                        divFinalSubmit.Visible = true;
                        divMain.Visible = false;
                    }
                }

                LoadSessionDDL(0);
                //LoadDDL();
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    List<DAL.ProgramPriority> pplist = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId).ToList();

                    if (pplist != null && pplist.Any())
                    {
                        List<DAL.SPAcademicCalendarGetAll_Result> sessions = db.AdmissionDB.SPAcademicCalendarGetAll().ToList();

                        int i = 0;
                        foreach (var item in pplist)
                        {
                            i = i + 1;
                            item.CreatedBy = i;

                            if (sessions != null && sessions.Any())
                            {
                                var Session = sessions.Where(x => x.AcademicCalenderID == item.AcaCalID).FirstOrDefault();
                                if (Session != null)
                                    item.Attribute1 = Session.FullCode;
                            }
                        }

                        GridViewProgramList.DataSource = pplist.OrderBy(x => x.Priority);
                        GridViewProgramList.DataBind();
                    }
                    else
                    {
                        GridViewProgramList.DataSource = null;
                        GridViewProgramList.DataBind();
                    }

                    BindEducation();


                    //DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId).FirstOrDefault();

                    //if (pp != null)
                    //{
                    //    ddlAdmissionUnit.SelectedValue = pp.AdmissionUnitID.ToString();

                    //    ddlProgram.SelectedValue = pp.ProgramID.ToString();


                    //    LoadSessionDDL(Convert.ToInt32(pp.ProgramID));

                    //    ddlSession.SelectedValue = pp.AcaCalID.ToString();


                    //}

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindEducation()
        {
            try
            {
                List<TempInfo> list = new List<TempInfo>();

                using (var db = new CandidateDataManager())
                {
                    List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);
                    int j = 0;
                    foreach (var Examitem in examList)
                    {
                        j = j + 1;
                        TempInfo NewObj = new TempInfo();

                        var ExamTypeObj = db.AdmissionDB.ExamTypes.Where(x => x.ID == Examitem.ExamTypeID).FirstOrDefault();
                        if (ExamTypeObj != null)
                        {
                            NewObj.SL = j;
                            NewObj.ExamId = ExamTypeObj.ID;
                            NewObj.ExamName = ExamTypeObj.ExamTypeName;

                            if (examList != null && examList.Any())
                            {
                                var ExamObj = examList.Where(x => x.ExamTypeID == Examitem.ExamTypeID).FirstOrDefault();

                                if (ExamObj != null)
                                {
                                    if (ExamObj.ExamDetail != null)
                                    {
                                        NewObj.InstituteName = ExamObj.ExamDetail.Institute;
                                        NewObj.PassingYear = ExamObj.ExamDetail.PassingYear.ToString();
                                        NewObj.Division = ExamObj.ExamDetail.ResultDivisionID.ToString();
                                        NewObj.GpaMarks = ExamObj.ExamDetail.GPA == null ? ExamObj.ExamDetail.Marks.ToString() : ExamObj.ExamDetail.GPA.ToString();
                                        NewObj.Grade = ExamObj.ExamDetail.Attribute2;

                                        if (ExamObj.ExamDetail.JsonDataObject != null)
                                            NewObj.ExamName = ExamObj.ExamDetail.JsonDataObject;
                                    }

                                }

                            }

                            list.Add(NewObj);
                        }
                    }

                    gvEducationList.DataSource = list;
                    gvEducationList.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadDDL()
        {
            try
            {
                using (var db = new GeneralDataManager())
                {
                    DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.Where(x => x.ID == 2 || x.ID == 3 || x.ID == 4 || x.ID == 5 || x.ID == 19).OrderBy(a => a.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);

                    List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();
                    DDLHelper.Bind<DAL.SPProgramsGetAllFromUCAM_Result>(ddlProgram, programs.OrderBy(a => a.DetailName).ToList(), "DetailNShortName", "ProgramID", EnumCollection.ListItemType.Program);

                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSessionDDL(int programID)
        {
            using (var db = new GeneralDataManager())
            {

                List<DAL.SPAcademicCalendarGetAll_Result> sessions = new List<DAL.SPAcademicCalendarGetAll_Result>();

                List<DAL.SPAcademicCalendarGetAll_Result> Tempsessions = db.AdmissionDB.SPAcademicCalendarGetAll().ToList();

                if (Tempsessions != null && Tempsessions.Any())
                {
                    var SetupList = db.AdmissionDB.AdmissionSetups.Where(x => x.Attribute3.ToLower().StartsWith("A")).ToList();

                    if (SetupList != null && SetupList.Any())
                    {
                        foreach (var item in SetupList)
                        {
                            var ExistingObj = Tempsessions.Where(x => x.AcademicCalenderID == item.AcaCalID).FirstOrDefault();
                            if (ExistingObj != null)
                            {
                                var IsExists = sessions.Where(x => x.AcademicCalenderID == item.AcaCalID).FirstOrDefault();
                                if (IsExists == null)
                                {
                                    sessions.Add(ExistingObj);
                                }
                            }
                        }
                    }

                    //sessions = sessions.Where(x => x.IsActiveAdmission == true).OrderBy(x => x.AcademicCalenderID).ToList();
                    //if (ActiveAdmission != null)
                    //{
                    //    sessions = sessions.Where(x => x.AcademicCalenderID >= ActiveAdmission.AcademicCalenderID).ToList();
                    //}
                    sessions = sessions.OrderByDescending(z => z.AcademicCalenderID).ToList();
                    DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, sessions, "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
                }

                else
                {
                    ddlSession.Items.Clear();
                    ddlSession.Items.Add(new ListItem("N/A", "-1"));
                }
            }
        }


        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);


                using (var db = new CandidateDataManager())
                {
                    List<DAL.AdmissionUnit> unitList = new List<DAL.AdmissionUnit>();

                    var SetupList = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == SessionId && x.Attribute3 == "Active").ToList();

                    List<DAL.AdmissionUnit> AllList = db.AdmissionDB.AdmissionUnits.Where(x => x.Attribute3 == "Active").ToList();

                    if (SetupList != null && SetupList.Any())
                    {
                        foreach (var item in SetupList)
                        {

                            if (AllList != null && AllList.Any())
                            {
                                DAL.AdmissionUnit NewObj = null;

                                var list = AllList.Where(x => x.ID == item.AdmissionUnitID).ToList();
                                if (list != null && list.Any())
                                    NewObj = list.FirstOrDefault();


                                var ExistingObj = unitList.Where(x => x.ID == item.AdmissionUnitID).ToList();
                                if (ExistingObj == null || ExistingObj.Count == 0)
                                {
                                    if (NewObj != null)
                                        unitList.Add(NewObj);

                                }

                            }

                        }
                    }


                    ddlAdmissionUnit.Items.Clear();
                    ddlAdmissionUnit.AppendDataBoundItems = true;
                    ddlAdmissionUnit.Items.Add(new ListItem("Select", "0"));

                    ddlAdmissionUnit.DataTextField = "UnitName";
                    ddlAdmissionUnit.DataValueField = "ID";
                    ddlAdmissionUnit.DataSource = unitList;
                    ddlAdmissionUnit.DataBind();


                }

            }
            catch (Exception ex)
            {

            }
        }


        protected void ddlAdmissionUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int SessionId = Convert.ToInt32(ddlSession.SelectedValue);
                int UnitId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);

                int CategoryId = 4;

                if (SessionId > 0 && UnitId > 0)
                {

                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();

                        List<DAL.SPProgramsGetAllFromUCAM_Result> ProgramList = new List<DAL.SPProgramsGetAllFromUCAM_Result>();

                        var SetupList = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == SessionId && x.AdmissionUnitID == UnitId && x.Attribute3 == "Active").ToList();

                        var CandidateBasicInfo = db.AdmissionDB.BasicInfoes.AsNoTracking().Where(x => x.ID == cId).FirstOrDefault();
                        try
                        {
                            if (CandidateBasicInfo != null)
                                CategoryId = CandidateBasicInfo.AttributeInt2 == null ? 0 : Convert.ToInt32(CandidateBasicInfo.AttributeInt2);
                        }
                        catch (Exception ex)
                        {
                        }

                        if (SetupList != null && SetupList.Any())
                        {
                            foreach (var item in SetupList)
                            {

                                var NewObj = programs.Where(x => x.ProgramID == item.ProgramID).FirstOrDefault();

                                var ExistingObj = ProgramList.Where(x => x.ProgramID == item.ProgramID).ToList();

                                if (ExistingObj == null || ExistingObj.Count == 0)
                                {
                                    if (NewObj != null)
                                        ProgramList.Add(NewObj);
                                }
                            }
                        }

                        if (ProgramList != null && ProgramList.Any())
                        {
                            if (CategoryId == 4) // Undergraduate
                                ProgramList = ProgramList.Where(x => x.ProgramTypeID == 1).ToList();
                            else // Others
                                ProgramList = ProgramList.Where(x => x.ProgramTypeID != 1).ToList();
                        }


                        ddlProgram.Items.Clear();
                        ddlProgram.AppendDataBoundItems = true;
                        ddlProgram.Items.Add(new ListItem("Select", "0"));

                        ddlProgram.DataTextField = "DetailNShortName";
                        ddlProgram.DataValueField = "ProgramID";
                        ddlProgram.DataSource = ProgramList.OrderBy(x => x.DetailNShortName).ToList();
                        ddlProgram.DataBind();
                    }


                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            int progId = -1;
            progId = Convert.ToInt32(ddlProgram.SelectedValue);
            if (progId > 0)
            {
                //LoadSessionDDL(progId);

                BindEducationGridView();
            }
            //else
            //{
            //    ddlSession.Items.Clear();
            //    ddlSession.Items.Add(new ListItem("N/A", "-1"));
            //}
        }

        private void BindEducationGridView()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ddlProgram.SelectedValue);
                if (ProgramId > 0)
                {

                    List<TempInfo> list = new List<TempInfo>();

                    using (var db = new CandidateDataManager())
                    {

                        DAL.SPProgramsGetAllFromUCAM_Result programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().Where(x => x.ProgramID == ProgramId).FirstOrDefault();

                        if (programs != null)
                        {
                            int i = 0;

                            if (programs.ProgramTypeID == 1) // Bechelor
                            {
                                i = 2;
                            }
                            else // Masters
                            {
                                i = 3;
                            }

                            for (int j = 0; j < i; j++)
                            {
                                int ExamType = 0;
                                if (j == 0)
                                    ExamType = 5;
                                else if (j == 1)
                                    ExamType = 7;
                                else
                                    ExamType = 3;




                                List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);


                                TempInfo NewObj = new TempInfo();

                                var ExamTypeObj = db.AdmissionDB.ExamTypes.Where(x => x.ID == ExamType).FirstOrDefault();
                                if (ExamTypeObj != null)
                                {
                                    NewObj.SL = j + 1;
                                    NewObj.ExamId = ExamTypeObj.ID;
                                    NewObj.ExamName = ExamTypeObj.ExamTypeName;

                                    if (examList != null && examList.Any())
                                    {
                                        var ExamObj = examList.Where(x => x.ExamTypeID == ExamType).FirstOrDefault();

                                        if (ExamObj != null)
                                        {
                                            if (ExamObj.ExamDetail != null)
                                            {
                                                NewObj.InstituteName = ExamObj.ExamDetail.Institute;
                                                NewObj.PassingYear = ExamObj.ExamDetail.PassingYear.ToString();
                                                NewObj.Division = ExamObj.ExamDetail.ResultDivisionID.ToString();
                                                NewObj.GpaMarks = ExamObj.ExamDetail.GPA == null ? ExamObj.ExamDetail.Marks.ToString() : ExamObj.ExamDetail.GPA.ToString();
                                                NewObj.Grade = ExamObj.ExamDetail.Attribute2;

                                                if (ExamObj.ExamDetail.JsonDataObject != null)
                                                    NewObj.ExamName = ExamObj.ExamDetail.JsonDataObject;

                                            }

                                        }

                                    }

                                    list.Add(NewObj);
                                }


                            }

                        }

                    }

                    gvEducationList.DataSource = list.OrderBy(x => x.SL);
                    gvEducationList.DataBind();
                }
                else
                {
                    gvEducationList.DataSource = null;
                    gvEducationList.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void buttonsave_Click(object sender, EventArgs e)
        {

            try
            {
                using (var db = new CandidateDataManager())
                {
                    #region Save Educational Information



                    List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);



                    foreach (GridViewRow row in gvEducationList.Rows)
                    {

                        DAL.Exam ExamObj = null;


                        HiddenField hdnExamId = (HiddenField)row.FindControl("ExamTypeId");
                        HiddenField hdnYear = (HiddenField)row.FindControl("hdnYear");
                        HiddenField hdnDivisionId = (HiddenField)row.FindControl("hdnDivisionId");

                        TextBox txtins = (TextBox)row.FindControl("txtInst");
                        TextBox GPAMarks = (TextBox)row.FindControl("txtGpa");
                        TextBox Grade = (TextBox)row.FindControl("txtLgD");


                        string Ins = txtins.Text == null ? "" : txtins.Text;


                        DropDownList ddlDivision = (DropDownList)row.FindControl("ddlDvision");
                        DropDownList ddlyear = (DropDownList)row.FindControl("ddlPassingYear");

                        TextBox lblExam = (TextBox)row.FindControl("lblExam");

                        int DivisionId = Int32.Parse(ddlDivision.SelectedValue);
                        int YearId = Int32.Parse(ddlyear.SelectedValue);


                        int ExamId = Convert.ToInt32(hdnExamId.Value);

                        if (examList != null)
                        {
                            ExamObj = examList.Where(x => x.ExamTypeID == ExamId).FirstOrDefault();
                        }

                        if (ExamObj != null) // Update
                        {
                            var ExamDetailsObj = db.AdmissionDB.ExamDetails.AsNoTracking().Where(x => x.ID == ExamObj.ExamDetailsID).FirstOrDefault();
                            if (ExamDetailsObj != null)
                            {
                                ExamDetailsObj.Institute = Ins;
                                ExamDetailsObj.ResultDivisionID = DivisionId;
                                ExamDetailsObj.PassingYear = YearId;

                                try
                                {
                                    if (!string.IsNullOrEmpty(GPAMarks.Text))
                                        ExamDetailsObj.Marks = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);

                                }
                                catch (Exception ex)
                                {
                                }
                                try
                                {
                                    if (!string.IsNullOrEmpty(GPAMarks.Text))
                                    {
                                        ExamDetailsObj.GPA = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Grade.Text))
                                    {
                                        ExamDetailsObj.Attribute2 = Grade.Text == null ? null : Grade.Text;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                try
                                {
                                    if (!string.IsNullOrEmpty(lblExam.Text))
                                    {
                                        ExamDetailsObj.JsonDataObject = lblExam.Text;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }


                                ExamDetailsObj.ModifiedBy = uId;
                                ExamDetailsObj.DateModified = DateTime.Now;

                                using (var dbUpdateExamDetails = new CandidateDataManager())
                                {
                                    dbUpdateExamDetails.Update<DAL.ExamDetail>(ExamDetailsObj);
                                }
                            }
                        }
                        else // Insert
                        {

                            #region Detail Insert

                            DAL.ExamDetail ExmDtlObj = new DAL.ExamDetail();

                            ExmDtlObj.EducationBoardID = 1;
                            ExmDtlObj.Institute = Ins;
                            ExmDtlObj.UndgradGradProgID = 1;
                            ExmDtlObj.ResultDivisionID = DivisionId;
                            ExmDtlObj.PassingYear = YearId;


                            try
                            {
                                if (!string.IsNullOrEmpty(GPAMarks.Text))
                                    ExmDtlObj.Marks = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);
                                else
                                    ExmDtlObj.Marks = null;
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(GPAMarks.Text))
                                    ExmDtlObj.GPA = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);
                                else
                                    ExmDtlObj.GPA = null;
                            }
                            catch (Exception ex)
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Grade.Text))
                                    ExmDtlObj.Attribute2 = Grade.Text == null ? null : Grade.Text;
                                else
                                    ExmDtlObj.Attribute2 = null;
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(lblExam.Text))
                                {
                                    ExmDtlObj.JsonDataObject = lblExam.Text;
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            ExmDtlObj.DateCreated = DateTime.Now;
                            ExmDtlObj.CreatedBy = cId;

                            long Id = 0;

                            using (var dbInsertSecExamDtl = new CandidateDataManager())
                            {
                                dbInsertSecExamDtl.Insert<DAL.ExamDetail>(ExmDtlObj);
                                Id = ExmDtlObj.ID;
                            }

                            #endregion

                            #region Master Insert

                            if (Id > 0)
                            {


                                DAL.Exam newExamObj = new DAL.Exam();
                                newExamObj.ExamDetailsID = Id;
                                newExamObj.CandidateID = cId;
                                newExamObj.ExamTypeID = ExamId;
                                newExamObj.CreatedBy = cId;
                                newExamObj.DateCreated = DateTime.Now;

                                using (var dbInsertSecExam = new CandidateDataManager())
                                {
                                    dbInsertSecExam.Insert<DAL.Exam>(newExamObj);
                                }

                            }

                            #endregion

                        }


                    }



                    #endregion
                }
            }
            catch (Exception ex)
            {
            }


            Response.Redirect("~/Admission/Candidate/Listofdocument.aspx", false);

        }


        public class TempInfo
        {
            public int SL { get; set; }
            public int ExamId { get; set; }
            public string ExamName { get; set; }
            public string PassingYear { get; set; }
            public string InstituteName { get; set; }
            public string Grade { get; set; }
            public string Division { get; set; }

            public string GpaMarks { get; set; }



        }

        protected void gvEducationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var ddl = e.Row.FindControl("ddlPassingYear") as DropDownList;
                    ddl.Items.Clear();
                    ddl.AppendDataBoundItems = true;

                    int startYear = 2000;
                    var YearList = Enumerable.Range(startYear, DateTime.Now.Year - startYear + 1).ToList();
                    ddl.DataSource = YearList.OrderByDescending(x => x.ToString());
                    ddl.DataBind();

                    #region Selected Year And Division
                    using (var db = new CandidateDataManager())
                    {

                        List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);

                        HiddenField hdnExamId = (HiddenField)e.Row.FindControl("ExamTypeId");

                        DropDownList ddlDivision = (DropDownList)e.Row.FindControl("ddlDvision");
                        DropDownList ddlyear = (DropDownList)e.Row.FindControl("ddlPassingYear");


                        try
                        {
                            int ExamTypeId = Convert.ToInt32(hdnExamId.Value);
                            if (ExamTypeId > 0)
                            {
                                if (examList != null && examList.Any())
                                {

                                    var ExistingObj = examList.Where(x => x.ExamTypeID == ExamTypeId).FirstOrDefault();
                                    if (ExistingObj != null)
                                    {
                                        var ExamdetailExam = ExistingObj.ExamDetail;
                                        if (ExamdetailExam != null)
                                        {

                                            ddlDivision.SelectedValue = ExamdetailExam.ResultDivisionID.ToString();
                                            ddlyear.SelectedValue = ExamdetailExam.PassingYear.ToString();

                                        }
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }



                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAddProgram_Click(object sender, EventArgs e)
        {
            try
            {

                int FacultyId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);
                int ProgramId = Convert.ToInt32(ddlProgram.SelectedValue);
                int AcacalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (FacultyId > 0 && ProgramId > 0 && AcacalId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.AdmissionUnit Unit = db.AdmissionDB.AdmissionUnits.AsNoTracking().Where(x => x.ID == FacultyId).FirstOrDefault();

                        List<DAL.ProgramPriority> ppList = db.AdmissionDB.ProgramPriorities.AsNoTracking().Where(x => x.CandidateID == cId).ToList();

                        DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.AsNoTracking().Where(x => x.CandidateID == cId
                                                                                                      && x.ProgramID == ProgramId && x.AcaCalID == AcacalId).FirstOrDefault();

                        DAL.AdmissionSetup asetup = db.AdmissionDB.AdmissionSetups.Where(x => x.AdmissionUnitID == FacultyId && x.AcaCalID == AcacalId && x.Attribute3 != null).FirstOrDefault();

                        DAL.AdmissionUnitProgram adUnitProgram = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AdmissionUnitID == FacultyId && x.AcaCalID == AcacalId && x.ProgramID == ProgramId).FirstOrDefault();

                        int SetupId = 0, UnitProgramId = 0, batchId = 0;

                        if (asetup != null)
                            SetupId = Convert.ToInt32(asetup.ID);
                        if (adUnitProgram != null)
                        {
                            UnitProgramId = Convert.ToInt32(adUnitProgram.ID);
                            batchId = Convert.ToInt32(adUnitProgram.BatchID);
                        }

                        List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();

                        var ProgramObj = programs.Where(x => x.ProgramID == ProgramId).FirstOrDefault();

                        string Name = ddlProgram.SelectedItem.ToString(), Code = "";

                        if (ProgramObj != null)
                        {
                            Name = ProgramObj.DetailName;
                            Code = ProgramObj.ShortName;
                        }

                        if (SetupId > 0 && UnitProgramId > 0 && batchId > 0)
                        {
                            long ProgramPriorityId = 0;

                            if (pp != null) // Update Existing Object
                            {
                                pp.AdmissionUnitID = FacultyId;
                                pp.AdmissionUnitProgramID = UnitProgramId;
                                pp.AdmissionSetupID = SetupId;
                                pp.ProgramID = ProgramId;
                                pp.BatchID = batchId;
                                pp.AcaCalID = AcacalId;
                                pp.ProgramName = Name;
                                pp.ShortName = Code;
                                pp.ModifiedBy = uId;
                                pp.DateModified = DateTime.Now;

                                using (var dbUpdate = new CandidateDataManager())
                                {
                                    dbUpdate.Update<DAL.ProgramPriority>(pp);
                                }

                                ProgramPriorityId = pp.ID;
                            }
                            else // Insert a New Object
                            {

                                int Priority = 1;

                                if (ppList != null && ppList.Any())
                                    Priority = ppList.Count() + 1;


                                DAL.ProgramPriority NewObj = new DAL.ProgramPriority();

                                NewObj.CandidateID = cId;
                                NewObj.AdmissionUnitID = FacultyId;
                                NewObj.AdmissionUnitProgramID = UnitProgramId;
                                NewObj.AdmissionSetupID = SetupId;
                                NewObj.ProgramID = ProgramId;
                                NewObj.BatchID = batchId;
                                NewObj.AcaCalID = AcacalId;
                                NewObj.Priority = Priority;
                                NewObj.ProgramName = Name;
                                NewObj.ShortName = Code;
                                NewObj.CreatedBy = uId;
                                NewObj.DateCreated = DateTime.Now;

                                using (var dbInsert = new CandidateDataManager())
                                {
                                    dbInsert.Insert<DAL.ProgramPriority>(NewObj);

                                    ProgramPriorityId = NewObj.ID;
                                }




                                #region CandidatePayment/CandidateFormSerial

                                //---------------------------------------------------------------------------------
                                //insert candidate payment
                                //---------------------------------------------------------------------------------
                                long candidatePaymentIDLong = -1;
                                if (asetup != null)
                                {
                                    using (var db1 = new CandidateDataManager())
                                    {
                                        DAL.CandidatePayment PaymentExistingObj = db1.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId && x.AcaCalID == AcacalId).FirstOrDefault();
                                        if (PaymentExistingObj == null)
                                        {

                                            string FeeInfo = asetup.Attribute3 == null ? "0_0" : asetup.Attribute3;
                                            decimal Amount = 0;
                                            if (FeeInfo != "")
                                            {
                                                string[] fees = FeeInfo.Split('_');
                                                if (fees.Length > 0)
                                                {
                                                    try
                                                    {
                                                        Amount = Convert.ToDecimal(fees[1]);

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                            }

                                            ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                                            db1.AdmissionDB.SPCandidatePaymentInsert(id_param, cId, null, AcacalId, Convert.ToInt32(Unit.UnitCode1), false, Amount, -99, DateTime.Now);
                                            candidatePaymentIDLong = Convert.ToInt64(id_param.Value);
                                        }
                                        else
                                        {
                                            candidatePaymentIDLong = (long)PaymentExistingObj.ID;
                                        }
                                    }
                                }

                                //---------------------------------------------------------------------------------
                                //insert candidate form serial
                                //---------------------------------------------------------------------------------
                                long candidateFormSerialIDLong = -1;
                                if (asetup != null)
                                {
                                    using (var db1 = new CandidateDataManager())
                                    {
                                        DAL.CandidateFormSl FormSLExistingObj = db1.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId && x.AcaCalID == AcacalId && x.AdmissionSetupID == asetup.ID).FirstOrDefault();
                                        if (FormSLExistingObj == null)
                                        {
                                            ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                                            db1.AdmissionDB.SPCandidateFormSlInsert(id_param, cId, asetup.ID, asetup.AcaCalID, Convert.ToInt32(Unit.UnitCode1), null, candidatePaymentIDLong, DateTime.Now, -99);
                                            candidateFormSerialIDLong = Convert.ToInt64(id_param.Value);
                                        }
                                        else
                                        {
                                            candidateFormSerialIDLong = (long)FormSLExistingObj.ID;
                                        }

                                    }
                                }
                                #endregion


                                #region Candidate Payment Information Table

                                using (var db1 = new CandidateDataManager())
                                {

                                    DAL.ForeignCandidatePaymentInformation ExistingObj = db1.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.ProgramPriorityId == ProgramPriorityId).FirstOrDefault();

                                    if (ExistingObj == null)
                                    {
                                        DAL.ForeignCandidatePaymentInformation NewPaymentObj = new DAL.ForeignCandidatePaymentInformation();

                                        NewPaymentObj.ProgramPriorityId = ProgramPriorityId;
                                        NewPaymentObj.FormSerialId = candidateFormSerialIDLong;
                                        NewPaymentObj.IsPaid = false;
                                        NewPaymentObj.CreatedBy = -99;
                                        NewPaymentObj.CreatedDate = DateTime.Now;

                                        db1.AdmissionDB.ForeignCandidatePaymentInformations.Add(NewPaymentObj);
                                        db1.AdmissionDB.SaveChanges();

                                    }


                                }

                                #endregion

                            }
                        }


                    }// end using

                }
                LoadSessionDDL(0);
                LoadData();
            }
            catch (Exception ex)
            {
                LoadData();
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int SetupId = Convert.ToInt32((sender as LinkButton).CommandArgument);

                using (var db = new CandidateDataManager())
                {

                    DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.Where(x => x.ID == SetupId).FirstOrDefault();

                    if (pp != null)
                    {
                        ddlAdmissionUnit.SelectedValue = pp.AdmissionUnitID.ToString();

                        ddlProgram.SelectedValue = pp.ProgramID.ToString();

                        LoadSessionDDL(Convert.ToInt32(pp.ProgramID));

                        ddlSession.SelectedValue = pp.AcaCalID.ToString();
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int SetupId = Convert.ToInt32((sender as LinkButton).CommandArgument);

                using (var db = new CandidateDataManager())
                {
                    var PaymentObj = db.AdmissionDB.ForeignCandidatePaymentInformations.AsNoTracking().Where(x => x.ProgramPriorityId == SetupId).FirstOrDefault();
                    if (PaymentObj != null && PaymentObj.IsPaid == false)
                    {
                        DAL.ProgramPriority ExistingObj = db.AdmissionDB.ProgramPriorities.AsNoTracking().Where(x => x.ID == SetupId).FirstOrDefault();

                        var PaymentIdObj = db.AdmissionDB.CandidatePayments.AsNoTracking().Where(x => x.CandidateID == ExistingObj.CandidateID && x.AcaCalID == ExistingObj.AcaCalID).FirstOrDefault();

                        var FormSerialIdObj = db.AdmissionDB.CandidateFormSls.AsNoTracking().Where(x => x.CandidateID == ExistingObj.CandidateID && x.AcaCalID == ExistingObj.AcaCalID).FirstOrDefault();



                        if (ExistingObj != null)
                        {
                            using (var dbDelete = new CandidateDataManager())
                            {
                                dbDelete.Delete<DAL.ProgramPriority>(ExistingObj);
                            }

                            using (var dbDelete2 = new CandidateDataManager())
                            {
                                if (FormSerialIdObj != null)
                                {
                                    dbDelete2.Delete<DAL.CandidateFormSl>(FormSerialIdObj);
                                }
                            }

                            using (var dbRemove = new CandidateDataManager())
                            {

                                if (PaymentObj != null)
                                {
                                    dbRemove.Delete<DAL.ForeignCandidatePaymentInformation>(PaymentObj);
                                }

                            }

                            using (var dbDelete1 = new CandidateDataManager())
                            {
                                if (PaymentIdObj != null && (PaymentIdObj.IsPaid == null || Convert.ToBoolean(PaymentIdObj.IsPaid) == false))
                                {
                                    dbDelete1.Delete<DAL.CandidatePayment>(PaymentIdObj);
                                }

                            }

                            LoadData();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}