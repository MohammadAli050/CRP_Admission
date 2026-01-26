using Admission.App_Start;
using AjaxControlToolkit;
using CommonUtility;
using DAL;
using DAL_UCAM;
using DATAMANAGER;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace Admission.Admission.Office
{
    public partial class EnrollCandidateV2 : PageBase
    {

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
                divViewButton.Visible = false;
                hdnCId.Value = "0";
                clearAllField();
            }
        }

        private void clearAllField()
        {
            try
            {
                lblFullName.Text = string.Empty;
                lblEmail.Text = string.Empty;
                lblPhone.Text = string.Empty;
                lblGender.Text = string.Empty;
                lblDOB.Text = string.Empty;
                lblGuardianPhone.Text = string.Empty;

                lblSSCBoard.Text = string.Empty;
                lblSSCGroup.Text = string.Empty;
                lblSSCYear.Text = string.Empty;
                lblSSCDivClass.Text = string.Empty;
                lblSSCExamType.Text = string.Empty;
                lblSSCgpa.Text = string.Empty;
                lblSSCInstitute.Text = string.Empty;

                lblHSCBoard.Text = string.Empty;
                lblHSCGroup.Text = string.Empty;
                lblHSCYear.Text = string.Empty;
                lblHSCDivClass.Text = string.Empty;
                lblHSCExamType.Text = string.Empty;
                lblHSCgpa.Text = string.Empty;
                lblHSCInstitute.Text = string.Empty;

                lblBInstitute.Text = string.Empty;
                lblBProgram.Text = string.Empty;
                lblBOthers.Text = string.Empty;
                lblBYear.Text = string.Empty;
                lblBdivClass.Text = string.Empty;
                lblBcgpa.Text = string.Empty;

                lblMcgpa.Text = string.Empty;
                lblMInstitute.Text = string.Empty;
                lblMProgram.Text = string.Empty;
                lblMOthers.Text = string.Empty;
                lblMyear.Text = string.Empty;
                lblMdivClass.Text = string.Empty;

                lblProgram.Text = string.Empty;
                lblBatch.Text = string.Empty;
                lblStudentID.Text = string.Empty;

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                clearAllField();
                long paymentId = 0;
                if (!string.IsNullOrEmpty(txtPaymentId.Text.Trim()))
                {
                    paymentId = long.Parse(txtPaymentId.Text.Trim());

                    using (var context = new CandidateDataManager())
                    {
                        var candidatePayment = context.AdmissionDB.CandidatePayments.Where(c => c.PaymentId == paymentId).FirstOrDefault();

                        if (candidatePayment != null)
                        {
                            if (candidatePayment.IsPaid != null && candidatePayment.IsPaid == true)
                            {
                                long CandidateID = candidatePayment.CandidateID.Value;
                                var candidateAdditionalInfo = context.AdmissionDB.AdditionalInfoes.Where(ca => ca.CandidateID == CandidateID).FirstOrDefault();

                                #region Final Submit Check

                                //if (candidateAdditionalInfo == null)
                                //{
                                //    showAlert("Additional Info not found for the given Candidate.");
                                //    return;
                                //}
                                //if (candidateAdditionalInfo.IsFinalSubmit == null || candidateAdditionalInfo.IsFinalSubmit == false)
                                //{
                                //    showAlert("Candidate has not final submit the application yet.");
                                //    return;
                                //}
                                #endregion

                                bool isEnrolled = CheckStudentAlreadyEnrolled(CandidateID);
                                if (!isEnrolled)
                                {
                                    var ProgramPriority = context.AdmissionDB.ProgramPriorities.Where(pp => pp.CandidateID == CandidateID).OrderBy(pp => pp.Priority).FirstOrDefault();

                                    if (ProgramPriority == null)
                                    {
                                        showAlert("Program Priority not found for the given Candidate.");
                                        return;
                                    }

                                    var candidate = context.AdmissionDB.BasicInfoes.Where(c => c.ID == CandidateID).FirstOrDefault();

                                    var ExamType = context.AdmissionDB.ExamTypes.ToList();
                                    var Boards = context.AdmissionDB.EducationBoards.ToList();
                                    var Groups = context.AdmissionDB.GroupOrSubjects.ToList();
                                    var Gender = context.AdmissionDB.Genders.ToList();

                                    var ProgramObj = context.AdmissionDB.SPProgramsGetByIdFromUCAM(ProgramPriority.ProgramID).FirstOrDefault();
                                    var BatchObj = context.AdmissionDB.SPBatchGetAllByProgram(ProgramPriority.ProgramID)
                                        .Where(x => x.BatchId == ProgramPriority.BatchID).FirstOrDefault();

                                    if (candidate != null)
                                    {
                                        divViewButton.Visible = true;
                                        hdnCId.Value = CandidateID.ToString();

                                        lblFullName.Text = candidate.FirstName;
                                        lblEmail.Text = candidate.Email;
                                        lblPhone.Text = candidate.SMSPhone;
                                        lblGender.Text = Gender.Where(g => g.ID == candidate.GenderID).Select(g => g.GenderName).FirstOrDefault();
                                        lblDOB.Text = candidate.DateOfBirth != null ? Convert.ToDateTime(candidate.DateOfBirth).ToString("dd-MM-yyyy") : string.Empty;
                                        lblGuardianPhone.Text = candidate.GuardianPhone;

                                        LoadCandidateImageAndSignData(CandidateID);

                                        #region Exam Details

                                        try
                                        {
                                            DAL.Exam secondaryExam = context.GetSecondaryExamByCandidateID_AD(CandidateID);
                                            DAL.Exam higherSecondaryExam = context.GetHigherSecdExamByCandidateID_AD(CandidateID);
                                            DAL.Exam undergradExam = context.GetUndergradExamByCandidateID_AD(CandidateID);
                                            DAL.Exam gradExam = context.GetGradExamByCandidateID_AD(CandidateID);
                                            List<DAL.ResultDivision> resultDivisionList = context.GetAllResultDivision_ND();
                                            List<DAL.UndergradGradProgram> undergradGradProgramList = context.GetAllUndergradGradProgram_ND();

                                            if (secondaryExam != null)
                                            {
                                                var secDetails = context.AdmissionDB.ExamDetails.Where(ed => ed.ID == secondaryExam.ExamDetailsID).FirstOrDefault();
                                                lblSSCBoard.Text = Boards.Where(b => b.ID == secDetails.EducationBoardID).Select(b => b.BoardName).FirstOrDefault();
                                                lblSSCGroup.Text = Groups.Where(g => g.ID == secDetails.GroupOrSubjectID).Select(g => g.GroupOrSubjectName).FirstOrDefault();
                                                lblSSCYear.Text = secDetails.PassingYear.ToString();
                                                lblSSCDivClass.Text = resultDivisionList.Where(rd => rd.ID == secDetails.ResultDivisionID).Select(rd => rd.ResultDivisionName).FirstOrDefault();
                                                lblSSCExamType.Text = ExamType.Where(et => et.ID == secondaryExam.ExamTypeID).Select(et => et.Code).FirstOrDefault();
                                                lblSSCgpa.Text = secDetails.GPA != null ? secDetails.GPA.Value.ToString("0.00") : string.Empty;
                                                lblSSCInstitute.Text = secDetails.Institute;
                                            }

                                            if (higherSecondaryExam != null)
                                            {
                                                var hscDetails = context.AdmissionDB.ExamDetails.Where(ed => ed.ID == higherSecondaryExam.ExamDetailsID).FirstOrDefault();
                                                lblHSCBoard.Text = Boards.Where(b => b.ID == hscDetails.EducationBoardID).Select(b => b.BoardName).FirstOrDefault();
                                                lblHSCGroup.Text = Groups.Where(g => g.ID == hscDetails.GroupOrSubjectID).Select(g => g.GroupOrSubjectName).FirstOrDefault();
                                                lblHSCYear.Text = hscDetails.PassingYear.ToString();
                                                lblHSCDivClass.Text = resultDivisionList.Where(rd => rd.ID == hscDetails.ResultDivisionID).Select(rd => rd.ResultDivisionName).FirstOrDefault();
                                                lblHSCExamType.Text = ExamType.Where(et => et.ID == higherSecondaryExam.ExamTypeID).Select(et => et.Code).FirstOrDefault();
                                                lblHSCgpa.Text = hscDetails.GPA != null ? hscDetails.GPA.Value.ToString("0.00") : string.Empty;
                                                lblHSCInstitute.Text = hscDetails.Institute;
                                            }

                                            if (undergradExam != null)
                                            {
                                                var ugDetails = context.AdmissionDB.ExamDetails.Where(ed => ed.ID == undergradExam.ExamDetailsID).FirstOrDefault();
                                                lblBInstitute.Text = ugDetails.Institute;
                                                lblBProgram.Text = undergradGradProgramList.Where(ugp => ugp.ID == ugDetails.UndgradGradProgID).Select(ugp => ugp.ProgramName).FirstOrDefault();
                                                lblBOthers.Text = ugDetails.OtherProgram;
                                                lblBYear.Text = ugDetails.PassingYear.ToString();
                                                lblBdivClass.Text = resultDivisionList.Where(rd => rd.ID == ugDetails.ResultDivisionID).Select(rd => rd.ResultDivisionName).FirstOrDefault();
                                                lblBcgpa.Text = ugDetails.GPA != null ? ugDetails.GPA.Value.ToString("0.00") : ugDetails.CGPA != null ? ugDetails.CGPA.Value.ToString("0.00") : string.Empty;
                                            }
                                            if (gradExam != null)
                                            {
                                                var gDetails = context.AdmissionDB.ExamDetails.Where(ed => ed.ID == gradExam.ExamDetailsID).FirstOrDefault();
                                                lblMInstitute.Text = gDetails.Institute;
                                                lblMProgram.Text = undergradGradProgramList.Where(ugp => ugp.ID == gDetails.UndgradGradProgID).Select(ugp => ugp.ProgramName).FirstOrDefault();
                                                lblMOthers.Text = gDetails.OtherProgram;
                                                lblMyear.Text = gDetails.PassingYear.ToString();
                                                lblMdivClass.Text = resultDivisionList.Where(rd => rd.ID == gDetails.ResultDivisionID).Select(rd => rd.ResultDivisionName).FirstOrDefault();
                                                lblMcgpa.Text = gDetails.GPA != null ? gDetails.GPA.Value.ToString("0.00") : gDetails.CGPA != null ? gDetails.CGPA.Value.ToString("0.00") : string.Empty;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        #endregion

                                        #region Program Batch Information

                                        if (ProgramObj != null)
                                            lblProgram.Text = ProgramObj.ShortName;
                                        if (BatchObj != null)
                                            lblBatch.Text = BatchObj.fullBatchDetailsStr;

                                        lblStudentID.Text = GetMaximumRollNoByProgramBatch(ProgramObj, BatchObj);

                                        #endregion

                                    }
                                }
                            }
                            else
                            {
                                showAlert("Payment is not completed for the given Payment Id.");
                                return;
                            }
                        }
                        else
                        {
                            showAlert("No candidate found for the given Payment Id.");
                            return;
                        }

                    }

                }
                else
                {
                    showAlert("Please enter a valid Payment Id.");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCandidateImageAndSignData(long cId)
        {
            if (cId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    List<DAL.Document> documentList = db.GetAllDocumentByCandidateID_AD(cId);
                    if (documentList != null)
                    {
                        DAL.DocumentDetail photoDocDetailObj = documentList.Where(c => c.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                        DAL.DocumentDetail signDocDetailObj = documentList.Where(c => c.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();

                        if (photoDocDetailObj != null)
                        {
                            ImagePhoto.ImageUrl = photoDocDetailObj.URL + "?v=" + DateTime.Now.Ticks;
                        }

                        if (signDocDetailObj != null)
                        {
                            ImageSignature.ImageUrl = signDocDetailObj.URL + "?v=" + DateTime.Now.Ticks;
                        }
                    }
                }
            }
        }

        private string GetMaximumRollNoByProgramBatch(SPProgramsGetByIdFromUCAM_Result programObj, SPBatchGetAllByProgram_Result batchObj)
        {
            string newRollNo = string.Empty;

            try
            {
                //CRP ID Format--
                //112240824
                //(Organization code + Department + Degree type + session + number of student in that department and degree)
                //Here, 
                //organization code: BHPI = 1, 
                //Department(PT = 1, OT = 2, SLT = 3, Lab Medicine = 4, P & O = 6, Rehab Science = 8),
                //Degree type(Bsc= 2, MSc= 1, Diploma= 3)

                string OrgCode = "1"; // BHPI
                string DeptCode = "0", DegreeCode = "0", Session = "";

                using (var db = new UCAMDataManager())
                {
                    var deptObj = db.UCAMDb.Departments.Where(d => d.DeptID == programObj.DeptID).FirstOrDefault();
                    var SessionObj = db.UCAMDb.AcademicCalenders.Where(a => a.AcademicCalenderID == batchObj.AcaCalId).FirstOrDefault();
                    if (deptObj != null)
                    {
                        DeptCode = deptObj.Code; // Department Code
                    }
                    if (SessionObj != null)
                    {
                        // Session Year (e.g., 2024).Get Right 2 digits.ex 24
                        Session = SessionObj.Year.ToString().Length >= 2 ? SessionObj.Year.ToString().Substring(SessionObj.Year.ToString().Length - 2) : SessionObj.Year.ToString();
                    }
                    if (programObj.ProgramTypeID == 1)//Undergraduate
                        DegreeCode = "2";
                    else if (programObj.ProgramTypeID == 2) // Gradudate
                        DegreeCode = "1";
                    else
                        DegreeCode = "3";

                    var maxRollNo = db.UCAMDb.Students
                        .Where(s => s.ProgramID == programObj.ProgramID && s.BatchId == batchObj.BatchId)
                        .Max(s => s.Roll);

                    if (!string.IsNullOrEmpty(maxRollNo))
                    {
                        int numericPart = int.Parse(maxRollNo.Substring(maxRollNo.Length - 4));
                        numericPart++;
                        newRollNo = OrgCode + DeptCode + DegreeCode + Session + numericPart.ToString("D4");
                    }
                    else
                    {
                        newRollNo = OrgCode + DeptCode + DegreeCode + Session + "0001";
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return newRollNo;

        }

        private bool CheckStudentAlreadyEnrolled(long value)
        {
            using (var db = new UCAMDataManager())
            {
                var student = db.UCAMDb.Students.Where(s => s.CandidateID == value).FirstOrDefault();
                if (student != null)
                {
                    showAlert("Student is already enrolled for the given Candidate.");
                    return true;
                }
                return false;
            }
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void btnEnroll_Click(object sender, EventArgs e)
        {
            try
            {
                long CandidateID = long.Parse(hdnCId.Value);
                if (CandidateID > 0)
                {
                    using (var context = new CandidateDataManager())
                    {
                        var candidatePayment = context.AdmissionDB.CandidatePayments.Where(c => c.CandidateID == CandidateID).FirstOrDefault();

                        if (candidatePayment != null)
                        {
                            if (candidatePayment.IsPaid != null && candidatePayment.IsPaid == true)
                            {
                                #region Final Submit Check

                                //if (candidateAdditionalInfo == null)
                                //{
                                //    showAlert("Additional Info not found for the given Candidate.");
                                //    return;
                                //}
                                //if (candidateAdditionalInfo.IsFinalSubmit == null || candidateAdditionalInfo.IsFinalSubmit == false)
                                //{
                                //    showAlert("Candidate has not final submit the application yet.");
                                //    return;
                                //}
                                #endregion

                                bool isEnrolled = CheckStudentAlreadyEnrolled(CandidateID);
                                if (!isEnrolled)
                                {
                                    var ProgramPriority = context.AdmissionDB.ProgramPriorities.Where(pp => pp.CandidateID == CandidateID).OrderBy(pp => pp.Priority).FirstOrDefault();

                                    if (ProgramPriority == null)
                                    {
                                        showAlert("Program Priority not found for the given Candidate.");
                                        return;
                                    }

                                    int ProgramID = ProgramPriority.ProgramID != null ? ProgramPriority.ProgramID.Value : 0;
                                    int BatchID = ProgramPriority.BatchID != null ? ProgramPriority.BatchID.Value : 0;
                                    int AcacalId = ProgramPriority.AcaCalID != null ? ProgramPriority.AcaCalID.Value : 0;

                                    string Roll = lblStudentID.Text.Trim();

                                    using (var ucamcontext = new UCAMDataManager())
                                    {
                                        var NewStudent = ucamcontext.UCAMDb.Students.Where(s => s.Roll == Roll).FirstOrDefault();
                                        if (NewStudent != null)
                                        {
                                            showAlert("Generated Student ID already exists. Please reload the page to get a new Student ID.");
                                            return;
                                        }
                                    }


                                    try
                                    {


                                        DAL_UCAM.Person person = new DAL_UCAM.Person();

                                        DAL_UCAM.Student studentObj = new DAL_UCAM.Student();

                                        DAL_UCAM.User userObj = new DAL_UCAM.User();

                                        DAL_UCAM.UserInPerson userInPersonObj = new DAL_UCAM.UserInPerson();

                                        var candidate = context.AdmissionDB.BasicInfoes.Where(c => c.ID == CandidateID).FirstOrDefault();
                                        var BG = context.AdmissionDB.BloodGroups.ToList();
                                        var Gender = context.AdmissionDB.Genders.ToList();
                                        var MG = context.AdmissionDB.MaritalStatus.ToList();
                                        var Country = context.AdmissionDB.Countries.ToList();

                                        List<DAL.Relation> relationList = context.GetAllRelationByCandidateID_ND(CandidateID);
                                        DAL.Relation fatherRelation = null;
                                        DAL.Relation motherRelation = null;
                                        DAL.Relation guardianRelation = null;


                                        if (relationList != null)
                                        {
                                            fatherRelation = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();
                                            motherRelation = relationList.Where(a => a.RelationTypeID == 3).FirstOrDefault();
                                            guardianRelation = relationList.Where(a => a.RelationTypeID == 5).FirstOrDefault();
                                        }

                                        int PersonId = 0, StudentId = 0;


                                        #region Person

                                        person.BloodGroup = BG.Where(b => b.ID == candidate.BloodGroupID).Select(b => b.BloodGroupName).FirstOrDefault();
                                        person.CreatedBy = -99;
                                        person.CreatedDate = DateTime.Now;
                                        person.DOB = candidate.DateOfBirth;
                                        person.Email = candidate.Email;

                                        if (fatherRelation != null)
                                        {
                                            person.FatherName = fatherRelation.RelationDetail.Name;
                                            person.FatherProfession = fatherRelation.RelationDetail.Occupation;
                                        }
                                        if (guardianRelation != null)
                                        {
                                            person.GuardianName = guardianRelation.RelationDetail.Name;
                                            person.SMSContactGuardian = guardianRelation.RelationDetail.Mobile;
                                        }
                                        if (motherRelation != null)
                                        {
                                            person.MotherName = motherRelation.RelationDetail.Name;
                                            person.MotherProfession = motherRelation.RelationDetail.Occupation;
                                        }

                                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                        person.FullName = textInfo.ToTitleCase(candidate.FirstName.ToLower());

                                        person.Gender = Gender.Where(g => g.ID == candidate.GenderID).Select(g => g.GenderName).FirstOrDefault();

                                        person.IsActive = true;
                                        person.IsDeleted = null;
                                        person.MatrialStatus = MG.Where(m => m.ID == candidate.MaritalStatusID).Select(m => m.MaritalStatus).FirstOrDefault();
                                        person.ModifiedBy = null;
                                        person.ModifiedDate = null;

                                        person.Nationality = Country.Where(c => c.ID == candidate.NationalityID).Select(c => c.Name).FirstOrDefault();
                                        person.Phone = candidate.Mobile;
                                        person.TypeId = 12;

                                        using (var perContext = new UCAMDataManager())
                                        {
                                            perContext.Insert<DAL_UCAM.Person>(person);
                                            PersonId = person.PersonID;
                                        }

                                        #endregion

                                        #region Student 

                                        if (PersonId > 0)
                                        {
                                            studentObj.CandidateID = Convert.ToInt32(CandidateID);
                                            studentObj.PersonID = PersonId;
                                            studentObj.ProgramID = ProgramID;
                                            studentObj.BatchId = BatchID;
                                            studentObj.SessionId = AcacalId;
                                            studentObj.Roll = Roll;
                                            studentObj.CreatedBy = -99;
                                            studentObj.CreatedDate = DateTime.Now;
                                            studentObj.IsActive = true;
                                            studentObj.IsDeleted = null;
                                            studentObj.ModifiedBy = null;
                                            studentObj.ModifiedDate = null;
                                            using (var stuContext = new UCAMDataManager())
                                            {
                                                stuContext.Insert<DAL_UCAM.Student>(studentObj);
                                                StudentId = studentObj.StudentID;
                                            }
                                        }

                                        #endregion

                                        int UserId = 0;

                                        #region User

                                        using (var userContext = new UCAMDataManager())
                                        {
                                            UserId = userContext.UCAMDb.Users.Max(u => u.User_ID) + 1;

                                            userObj.User_ID = UserId;
                                            userObj.LogInID = Roll;
                                            userObj.Password = "12345@#";
                                            userObj.IsActive = true;
                                            userObj.RoleID = 4; // Student Role
                                            userObj.CreatedBy = -99;
                                            userObj.CreatedDate = DateTime.Now;
                                            userObj.ModifiedBy = -99;
                                            userObj.ModifiedDate = DateTime.Now;
                                            userContext.Insert<DAL_UCAM.User>(userObj);


                                            if (UserId > 0)
                                            {
                                                userInPersonObj.User_ID = UserId;
                                                userInPersonObj.PersonID = PersonId;
                                                userInPersonObj.CreatedBy = -99;
                                                userInPersonObj.CreatedDate = DateTime.Now;
                                                userInPersonObj.ModifiedBy = -99;
                                                userInPersonObj.ModifiedDate = DateTime.Now;
                                                userContext.Insert<DAL_UCAM.UserInPerson>(userInPersonObj);
                                            }

                                        }

                                        #endregion


                                        #region Image Transfer Admission to UCAM

                                        #region Get URL Property
                                        List<DAL.URLProperty> urlPropertyList = null;
                                        using (var db = new GeneralDataManager())
                                        {
                                            urlPropertyList = db.AdmissionDB.URLProperties.Where(x => x.IsActive == true).ToList();
                                        }
                                        #endregion

                                        try
                                        {
                                            #region Get Info
                                            string admissiSourcePathImage = string.Empty;
                                            string ucamDestinationPathImage = string.Empty;

                                            string admissiSourcePathSignature = string.Empty;
                                            string ucamDestinationPathSignature = string.Empty;


                                            if (urlPropertyList != null && urlPropertyList.Count > 0)
                                            {
                                                admissiSourcePathImage = urlPropertyList.Where(x => x.URLType == 2 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoRootPath
                                                ucamDestinationPathImage = urlPropertyList.Where(x => x.URLType == 3 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoDestinationPath

                                                admissiSourcePathSignature = urlPropertyList.Where(x => x.URLType == 4 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoRootPath
                                                ucamDestinationPathSignature = urlPropertyList.Where(x => x.URLType == 5 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoDestinationPath
                                            }
                                            #endregion

                                            #region Image Transfer
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(admissiSourcePathImage) && !string.IsNullOrEmpty(ucamDestinationPathImage))
                                                {
                                                    List<DAL.SPGetAllImageInfoForImageTransfer_Result> list = null;
                                                    using (var db = new GeneralDataManager())
                                                    {
                                                        list = db.AdmissionDB.SPGetAllImageInfoForImageTransfer(2, candidatePayment.PaymentId).ToList(); // 2 = Image
                                                    }

                                                    if (list != null && list.Count > 0)
                                                    {
                                                        foreach (var tData in list)
                                                        {
                                                            string sourcePathAdmission = admissiSourcePathImage + tData.AdmissionImageName;

                                                            if (File.Exists(sourcePathAdmission))
                                                            {
                                                                string ucamImageName = (PersonId.ToString() + ".jpg");
                                                                string destinationPathUCAMPhoto = ucamDestinationPathImage + ucamImageName;

                                                                if (!File.Exists(destinationPathUCAMPhoto))
                                                                {
                                                                    File.Copy(sourcePathAdmission, destinationPathUCAMPhoto, true);
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            #endregion

                                            #region Signature Transfer
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(admissiSourcePathSignature) && !string.IsNullOrEmpty(ucamDestinationPathSignature))
                                                {
                                                    List<DAL.SPGetAllImageInfoForImageTransfer_Result> list = null;
                                                    using (var db = new GeneralDataManager())
                                                    {
                                                        list = db.AdmissionDB.SPGetAllImageInfoForImageTransfer(3, candidatePayment.PaymentId).ToList(); // 3 = Signature
                                                    }

                                                    if (list != null && list.Count > 0)
                                                    {
                                                        foreach (var tData in list)
                                                        {
                                                            string sourcePathAdmission = admissiSourcePathSignature + tData.AdmissionImageName;

                                                            if (File.Exists(sourcePathAdmission))
                                                            {
                                                                string ucamImageName = (PersonId.ToString() + ".jpg");
                                                                string destinationPathUCAMPhoto = ucamDestinationPathSignature + ucamImageName;

                                                                if (!File.Exists(destinationPathUCAMPhoto))
                                                                {
                                                                    File.Copy(sourcePathAdmission, destinationPathUCAMPhoto, true);
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            #endregion

                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion


                                        if (PersonId > 0)
                                        {
                                            showAlert("Student enrolled successfully.");
                                            divViewButton.Visible = true;
                                            hdnCId.Value = CandidateID.ToString();
                                            btnEnroll.Enabled = false;

                                        }
                                        else
                                        {
                                            showAlert("Error occurred while enrolling the student.");
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                else
                                {
                                    showAlert("Student is already enrolled for the given Candidate.");
                                    return;

                                }

                            }
                            else
                            {
                                showAlert("Payment is not completed for the given Candidate.");
                                return;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }

        protected void lnkViewFullForm_Click(object sender, EventArgs e)
        {
            try
            {
                long cId = long.Parse(hdnCId.Value);
                if (cId > 0)
                {
                    string CandidateEncryptedId = Encrypt.EncryptString(cId.ToString());
                    string url = ResolveUrl("~/Admission/Office/CandidateInfo/CandApplicationBasic.aspx?val=" + CandidateEncryptedId);

                    // Create the JavaScript string
                    string script = $"window.open('{url}', '_blank');";

                    // Inject and execute the script
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenNewTab", script, true);
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}