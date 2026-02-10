using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Admission.Admission.Candidate
{
    public partial class ApplicationEducation : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);


            if (!IsPostBack)
            {

                divSubjectWiseGradeOLevel.Visible = false;
                divSubjectWiseGradeALevel.Visible = false;
                divCalculateOLevel.Visible = false;
                divCalculateALevel.Visible = false;

                ClearOLevelGrid();
                ClearALevelGrid();

                //txtSec_CgpaScore.Enabled = false;
                //txtHigherSec_CgpaScore.Enabled = false;

                //ddlSec_ExamType.Enabled = false;
                //ddlHigherSec_ExamType.Enabled = false;

                //ddlSec_GrpOrSub.Enabled = false;
                //ddlHigherSec_GrpOrSub.Enabled = false;

                //ddlSec_PassingYear.Enabled = false;
                //ddlHigherSec_PassingYear.Enabled = false;

                if (uId > 0)
                {
                    LoadDDL();
                    LoadCandidateData(uId);
                    //panel_isUndergrad.Visible = ShowUndergraduatePanel();
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.ExamType> examTypeList = db.GetAllExamType_ND();
                if (examTypeList != null && examTypeList.Any())
                {
                    DDLHelper.Bind<DAL.ExamType>(ddlSec_ExamType, examTypeList.Where(a => a.EducationCategory_ID == 2 && a.IsActive==true).ToList(), "ExamTypeName", "ID", EnumCollection.ListItemType.ExamType);
                    DDLHelper.Bind<DAL.ExamType>(ddlHigherSec_ExamType, examTypeList.Where(a => a.EducationCategory_ID == 3 && a.IsActive==true).ToList(), "ExamTypeName", "ID", EnumCollection.ListItemType.ExamType);
                }

                List<DAL.EducationBoard> educationBoardList = db.GetAllEducationBoard_ND();
                if (educationBoardList != null && educationBoardList.Any())
                {
                    DDLHelper.Bind<DAL.EducationBoard>(ddlSec_EducationBrd, educationBoardList.Where(a => a.IsActive == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                    DDLHelper.Bind<DAL.EducationBoard>(ddlHigherSec_EducationBrd, educationBoardList.Where(a => a.IsActive == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                }

                List<DAL.GroupOrSubject> groupOrSubjectList = db.GetAllGroupOrSubject_ND();
                if (groupOrSubjectList != null && groupOrSubjectList.Any())
                {
                    DDLHelper.Bind<DAL.GroupOrSubject>(ddlSec_GrpOrSub, groupOrSubjectList.Where(a => a.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                    DDLHelper.Bind<DAL.GroupOrSubject>(ddlHigherSec_GrpOrSub, groupOrSubjectList.Where(a => a.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                    //DDLHelper.Bind<DAL.GroupOrSubject>(ddlUndergrad_GrpOrSub, groupOrSubjectList.Where(a => a.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                    //DDLHelper.Bind<DAL.GroupOrSubject>(ddlGraduate_GrpOrSub, groupOrSubjectList.Where(a => a.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                }

                List<DAL.ResultDivision> resultDivisionList = db.GetAllResultDivision_ND();
                if (resultDivisionList != null && resultDivisionList.Any())
                {
                    DDLHelper.Bind<DAL.ResultDivision>(ddlSec_DivClass, resultDivisionList.Where(a => a.IsActive == true && a.ID != 6).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlHigherSec_DivClass, resultDivisionList.Where(a => a.IsActive == true && a.ID != 6).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlUndergrad_DivClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlGraduate_DivClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                }

                List<DAL.UndergradGradProgram> undergradGradProgramList = db.GetAllUndergradGradProgram_ND();
                if (undergradGradProgramList != null && undergradGradProgramList.Any())
                {
                    DDLHelper.Bind<DAL.UndergradGradProgram>(ddlUndergrad_ProgramDegree, undergradGradProgramList.Where(a => a.EducationCategoryID == 4 && a.IsActive == true).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Program);
                    DDLHelper.Bind<DAL.UndergradGradProgram>(ddlGraduate_ProgramDegree, undergradGradProgramList.Where(a => a.EducationCategoryID == 6 && a.IsActive == true).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Program);
                }

                ddlHigherSec_PassingYear.Items.Clear();
                ddlHigherSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
                ddlHigherSec_PassingYear.AppendDataBoundItems = true;
                ddlSec_PassingYear.Items.Clear();
                ddlSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
                ddlSec_PassingYear.AppendDataBoundItems = true;
                ddlUndergrad_PassingYear.Items.Clear();
                ddlUndergrad_PassingYear.Items.Add(new ListItem("Select", "-1"));
                ddlUndergrad_PassingYear.AppendDataBoundItems = true;
                ddlGraduate_PassingYear.Items.Clear();
                ddlGraduate_PassingYear.Items.Add(new ListItem("Select", "-1"));
                ddlGraduate_PassingYear.AppendDataBoundItems = true;
                for (int i = DateTime.Now.Year; i >= 1950; i--)
                {
                    ddlHigherSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    ddlSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    ddlUndergrad_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    ddlGraduate_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
        }

        #region N/A
        //private void LoadPassingYearDDLForUndergrad()
        //{

        //    #region SSC Passing Years
        //    ddlSec_PassingYear.Items.Clear();
        //    ddlSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlSec_PassingYear.AppendDataBoundItems = true;
        //    for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 5; i--)
        //    {
        //        ddlSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }
        //    #endregion

        //    #region HSC Passing Years
        //    ddlHigherSec_PassingYear.Items.Clear();
        //    ddlHigherSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlHigherSec_PassingYear.AppendDataBoundItems = true;

        //    for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 3; i--)
        //    {
        //        ddlHigherSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }
        //    #endregion

        //    ddlUndergrad_PassingYear.Items.Clear();
        //    ddlUndergrad_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    //ddlUndergrad_PassingYear.AppendDataBoundItems = true;
        //    ddlGraduate_PassingYear.Items.Clear();
        //    ddlGraduate_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    //ddlGraduate_PassingYear.AppendDataBoundItems = true;
        //}

        //private void LoadPassingYearDDLForGrad()
        //{
        //    ddlHigherSec_PassingYear.Items.Clear();
        //    ddlHigherSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlHigherSec_PassingYear.AppendDataBoundItems = true;
        //    ddlSec_PassingYear.Items.Clear();
        //    ddlSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlSec_PassingYear.AppendDataBoundItems = true;
        //    ddlUndergrad_PassingYear.Items.Clear();
        //    ddlUndergrad_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlUndergrad_PassingYear.AppendDataBoundItems = true;
        //    ddlGraduate_PassingYear.Items.Clear();
        //    ddlGraduate_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlGraduate_PassingYear.AppendDataBoundItems = true;
        //    for (int i = DateTime.Now.Year; i > 1950; i--)
        //    {
        //        ddlHigherSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //        ddlSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //        ddlUndergrad_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //        ddlGraduate_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }
        //} 
        #endregion


        private void LoadCandidateData(long uId)
        {
            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                    if (obj != null && obj.ID > 0)
                    {
                        cId = obj.ID;
                    }// end if(obj != null && obj.ID > 0)
                }// end using


                if (cId > 0)
                {
                    #region Get EducationCategoryId & ProgramId
                    int educationCategoryId = -1;
                    int programId = -1;
                    using (var db = new CandidateDataManager())
                    {
                        educationCategoryId = db.GetCandidateEducationCategoryID(cId);

                        if (educationCategoryId != 4)
                        {
                            DAL.CandidateFormSl formSerial = db.GetCandidateFormSlByCandID_AD(cId);

                            if (formSerial != null && formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms != null)
                            {
                                programId = formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.FirstOrDefault().ProgramID;
                            }
                        }
                    }
                    #endregion

                    #region Get IsFinalSubmit
                    bool isFinalSubmit = false;
                    DAL.AdditionalInfo additionalInfo = null;
                    using (var db = new CandidateDataManager())
                    {
                        additionalInfo = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                    }
                    if (additionalInfo != null)
                    {
                        isFinalSubmit = Convert.ToBoolean(additionalInfo.IsFinalSubmit);
                    }
                    #endregion

                    #region Property Setup (Candidate Submit Button Show/Hide)
                    List<DAL.PropertySetup> propertySetupList = null;
                    using (var db = new OfficeDataManager())
                    {
                        propertySetupList = db.AdmissionDB.PropertySetups.Where(x => x.IsActive == true
                                                                                    && x.EducationCategoryID == educationCategoryId).ToList();
                    }

                    if (propertySetupList != null && propertySetupList.Count > 0)
                    {
                        if (educationCategoryId == 4 || educationCategoryId == 6)
                        {
                            #region Bachelor

                            #region Candidate Submit Button Show/Hide
                            try
                            {
                                var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)).FirstOrDefault();
                                if (candidateSubmitButtonSetup != null)
                                {
                                    bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);

                                    if (showHide == true)
                                    {
                                        btnSave_Education.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Education.Visible = false;
                                    }

                                }
                                else
                                {
                                    btnSave_Education.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Education.Visible = false;
                            }
                            #endregion


                            #endregion
                        }
                        else
                        {
                            #region Masters

                            #region Candidate Submit Button Show/Hide
                            try
                            {
                                var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)
                                                                                            && x.ProgramId == programId).FirstOrDefault();
                                if (candidateSubmitButtonSetup != null)
                                {
                                    bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);
                                    if (showHide == true)
                                    {
                                        btnSave_Education.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Education.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnSave_Education.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Education.Visible = false;
                            }
                            #endregion

                            #endregion
                        }
                    }
                    else
                    {
                        btnSave_Education.Visible = false;
                    }
                    #endregion


                    #region Breadcrumbs for Bachelor and Masters
                    if (educationCategoryId == 4 || educationCategoryId == 6)
                    {
                        bachelorsBreadcrumb.Visible = true;
                        mastersBreadcrumb.Visible = false;
                    }
                    else
                    {
                        bachelorsBreadcrumb.Visible = false;
                        mastersBreadcrumb.Visible = true;
                    }
                    #endregion


                    #region N/A -- Prevent Save if IsFinalSubmit or IsApproved
                    //try
                    //{
                    //    List<DAL.SPGetCandidateEducationCategoryByCandidateID_Result> list = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        list = db.AdmissionDB.SPGetCandidateEducationCategoryByCandidateID(cId).ToList();
                    //    }

                    //    if (list != null)
                    //    {
                    //        #region Bachelors
                    //        DAL.SPGetCandidateEducationCategoryByCandidateID_Result undergradCandidate =
                    //                                list.Where(c => c.EduCatID == 4).Take(1).FirstOrDefault();

                    //        if (undergradCandidate != null)
                    //        {
                    //            btnSave_Education.Enabled = false;
                    //            btnSave_Education.Visible = false;

                    //            if (undergradCandidate.IsApproved != null)
                    //            {
                    //                if (undergradCandidate.IsApproved == true)
                    //                {
                    //                    btnSave_Education.Enabled = false;
                    //                    btnSave_Education.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Education.Enabled = true;
                    //                    btnSave_Education.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Education.Enabled = true;
                    //                btnSave_Education.Visible = true;
                    //            }

                    //            if (undergradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (undergradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Education.Enabled = false;
                    //                    btnSave_Education.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Education.Enabled = true;
                    //                    btnSave_Education.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Education.Enabled = true;
                    //                btnSave_Education.Visible = true;
                    //            }
                    //        }
                    //        #endregion

                    //        #region Masters
                    //        DAL.SPGetCandidateEducationCategoryByCandidateID_Result gradCandidate =
                    //                                list.Where(c => c.EduCatID == 6).Take(1).FirstOrDefault();

                    //        if (gradCandidate != null)
                    //        {
                    //            if (gradCandidate.IsApproved != null)
                    //            {
                    //                if (gradCandidate.IsApproved == true)
                    //                {
                    //                    btnSave_Education.Enabled = false;
                    //                    btnSave_Education.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Education.Enabled = true;
                    //                    btnSave_Education.Visible = true;
                    //                }
                    //            }

                    //            if (gradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (gradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Education.Enabled = false;
                    //                    btnSave_Education.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Education.Enabled = true;
                    //                    btnSave_Education.Visible = true;
                    //                }
                    //            }
                    //        }
                    //        #endregion



                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //    throw;
                    //}
                    #endregion

                    List<DAL.CandidateFormSl> candidateFormSlList = null;
                    DAL.AdmissionSetup admSetup = null;

                    using (var db = new CandidateDataManager())
                    {
                        candidateFormSlList = db.GetAllCandidateFormSlByCandID_AD(cId);
                        if (candidateFormSlList != null)
                        {
                            //get only admSetup for masters.
                            admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6).Select(c => c.AdmissionSetup).FirstOrDefault();
                        }
                    }

                    if (admSetup != null) //if it is a masters candidate, then show undergrad and grad fields, i.e. show panel_isUndergrad.
                    {
                        hfEduCat.Value = "masters";

                        panel_isUndergrad.Visible = true;

                        //let masters candidate change divisionClass in ssc and hsc
                        ddlSec_DivClass.Enabled = true;

                        ddlHigherSec_DivClass.Enabled = true;

                        //load different passing year for masters candidate
                        //LoadPassingYearDDLForGrad();

                        //txtUndergrad_Institute_ReqV.Enabled = true;
                        //ddlUndergrad_ProgramDegree_ComV.Enabled = true;
                        //ddlUndergrad_GrpOrSub_ComV.Enabled = true;
                        //ddlUndergrad_DivClass_ComV.Enabled = true;
                        //ddlUndergrad_PassingYear_ComV.Enabled = true;

                        txtSec_CgpaScore.Enabled = true;
                        txtHigherSec_CgpaScore.Enabled = true;

                        ddlSec_ExamType.Enabled = true;
                        ddlHigherSec_ExamType.Enabled = true;

                        ddlSec_GrpOrSub.Enabled = true;
                        ddlHigherSec_GrpOrSub.Enabled = true;

                        ddlSec_PassingYear.Enabled = true;
                        ddlHigherSec_PassingYear.Enabled = true;

                    }
                    else //bachelors candidate, do not show panel_isUndergrad
                    {
                        hfEduCat.Value = "bachelors";

                        panel_isUndergrad.Visible = false;

                        //...For SSC
                        #region For SSC Enable(true/false)
                        //ddlSec_ExamType.Enabled = false;
                        //ddlSec_GrpOrSub.Enabled = false;
                        ddlSec_DivClass.SelectedItem.Text = "GPA";
                        //ddlSec_DivClass.Enabled = false;
                        //txtSec_CgpaScore.Enabled = false;
                        //ddlSec_PassingYear.Enabled = false;


                        //ddlSec_EducationBrd.Enabled = false;
                        //txtSec_Institute.Enabled = false;
                        //txtSec_RollNo.Enabled = false;
                        //txtSec_CgpaW4S.Enabled = false;
                        #endregion

                        //...For HSC
                        #region For HSC Enable(true/false)
                        //ddlHigherSec_ExamType.Enabled = false;
                        //ddlHigherSec_GrpOrSub.Enabled = false;
                        ddlHigherSec_DivClass.SelectedItem.Text = "GPA";
                        //ddlHigherSec_DivClass.Enabled = false;
                        //txtHigherSec_CgpaScore.Enabled = false;
                        //ddlHigherSec_PassingYear.Enabled = false;


                        //ddlHigherSec_EducationBrd.Enabled = false;
                        //txtHigherSec_Institute.Enabled = false;
                        //txtHigherSec_RollNo.Enabled = false;
                        //txtHigherSec_GpaW4S.Enabled = false;
                        #endregion


                    }

                    if (educationCategoryId == 6)
                    {
                        lblSSCTotalMark.Visible = false;
                        lblHSCTotalMark.Visible = false;
                    }
                    else
                    {
                        lblSSCTotalMark.Visible = true;
                        lblHSCTotalMark.Visible = true;
                    }


                    using (var db = new CandidateDataManager())
                    {

                        DAL.Exam secondaryExam = db.GetSecondaryExamByCandidateID_AD(cId);
                        DAL.Exam higherSecondaryExam = db.GetHigherSecdExamByCandidateID_AD(cId);
                        DAL.Exam undergradExam = db.GetUndergradExamByCandidateID_AD(cId);
                        DAL.Exam gradExam = db.GetGradExamByCandidateID_AD(cId);

                        #region SSC
                        if (secondaryExam != null)
                        {
                            if (secondaryExam.ExamTypeID == 14) // International Baccalaureate
                            {
                                DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                                if (secExamDetail != null && secExamDetail.ID > 0)
                                {
                                    ddlSec_ExamType.SelectedValue = "14"; // International Baccalaureate

                                    ddlSec_EducationBrd.SelectedValue = secExamDetail.EducationBoardID.ToString();
                                    ddlSec_EducationBrd.Visible = false;
                                    txtSec_EducationBrd.Text = secExamDetail.Attribute1;
                                    txtSec_EducationBrd.Visible = true;

                                    txtSec_Institute.Text = secExamDetail.Institute;
                                    //txtSec_Institute.Enabled = true;

                                    txtSec_RollNo.Text = secExamDetail.RollNo;
                                    //txtSec_RollNo.Enabled = true;

                                    txtSec_RegNo.Text = secExamDetail.RegistrationNo;

                                    ddlSec_GrpOrSub.SelectedValue = secExamDetail.GroupOrSubjectID.ToString();
                                    trGroupOrSubjectSSC.Visible = false;

                                    ddlSec_DivClass.SelectedValue = secExamDetail.ResultDivisionID.ToString();
                                    trDivisionClassSSC.Visible = false;

                                    txtSec_CgpaScore.Text = secExamDetail.GPA.ToString();
                                    txtSec_CgpaScore.Enabled = true;

                                    txtSec_Marks.Text = secExamDetail.Marks.ToString();
                                    //trMarksForOLevelSSC.Visible = false;
                                    //trOutofMarksForOLevelSSC.Visible = false;


                                    if (secExamDetail.AttributeDec1 != null)
                                        txtOutofSec_Marks.Text = secExamDetail.AttributeDec1.ToString();
                                    else
                                        txtOutofSec_Marks.Text = string.Empty;

                                    txtSec_CgpaW4S.Text = secExamDetail.GPAW4S.ToString();
                                    //trGPA4thSubjectSSC.Visible = false;

                                    ddlSec_PassingYear.SelectedValue = secExamDetail.PassingYear.ToString();
                                    ddlSec_StudentCat.SelectedValue = secExamDetail.AttributeInt2.ToString();
                                    ddlSec_PassingYear.Enabled = true;
                                }
                            }
                            else
                            {
                                DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                                if (secExamDetail != null && secExamDetail.ID > 0)
                                {
                                    ddlSec_ExamType.SelectedValue = secondaryExam.ExamTypeID.ToString();
                                    ddlSec_EducationBrd.SelectedValue = secExamDetail.EducationBoardID.ToString();
                                    txtSec_Institute.Text = secExamDetail.Institute;
                                    //if (secondaryExam.ExamTypeID == 5) //O-Level
                                    //{
                                    //    txtSec_RollNo.Enabled = true;
                                    //}
                                    txtSec_RollNo.Text = secExamDetail.RollNo;
                                    txtSec_RegNo.Text = secExamDetail.RegistrationNo;
                                    ddlSec_GrpOrSub.SelectedValue = secExamDetail.GroupOrSubjectID.ToString();
                                    ddlSec_DivClass.SelectedValue = secExamDetail.ResultDivisionID.ToString();
                                    #region outofmarks ssc code remove in future
                                    //if (secondaryExam.ExamTypeID == 1)
                                    //    trOutofMarksForOLevelSSC.Visible = false;
                                    #endregion
                                    if (secExamDetail.ResultDivisionID == 5)
                                    {
                                        //txtSec_CgpaScore.Enabled = true;
                                        txtSec_CgpaScore.Text = secExamDetail.GPA.ToString();
                                    }
                                    else
                                    {
                                        //txtSec_CgpaScore.Enabled = false;
                                        txtSec_CgpaScore.Text = null;
                                    }
                                    txtSec_Marks.Text = secExamDetail.Marks.ToString();

                                    txtOutofSec_Marks.Text = secExamDetail.AttributeDec1 == null ? "" : secExamDetail.AttributeDec1.ToString();

                                    //if (secondaryExam.ExamTypeID == 5)
                                    //{
                                    //    txtSec_Marks.Enabled = false;
                                    //}
                                    //else
                                    //{
                                    //    txtSec_Marks.Enabled = true;
                                    //}
                                    txtSec_CgpaW4S.Text = secExamDetail.GPAW4S.ToString();
                                    ddlSec_PassingYear.SelectedValue = secExamDetail.PassingYear.ToString();
                                    ddlSec_StudentCat.SelectedValue = secExamDetail.AttributeInt2.ToString();



                                    #region Disabled All Field

                                    //ddlSec_ExamType.Enabled = false;
                                    //ddlSec_EducationBrd.Enabled = false;
                                    //txtSec_Institute.Enabled = false;
                                    //ddlSec_GrpOrSub.Enabled = false;
                                    //ddlSec_DivClass.Enabled = false;
                                    //txtSec_CgpaScore.Enabled = false;
                                    //ddlSec_PassingYear.Enabled = false;

                                    if (secondaryExam.ExamTypeID == 5 || educationCategoryId == 6) //O-Level & Master's Program
                                    {
                                        txtSec_RollNo.Enabled = true;
                                        txtSec_CgpaW4S.Enabled = true;
                                        txtSec_Marks.Enabled = true;
                                        if (secExamDetail.EducationBoardID == 13 || secExamDetail.EducationBoardID == 12 || secExamDetail.EducationBoardID == 11) // O Level Cambridge And Edexel can not change total achived mark
                                        {
                                            //txtSec_Marks.Enabled = false;
                                            //txtOutofSec_Marks.Enabled = false;
                                            lnkAddResult.Visible = true;
                                            divSubjectWiseGradeOLevel.Visible = true;

                                            ClearOLevelGrid();

                                            CheckAndBindGridViewOfOLevel();

                                        }
                                        else
                                        {
                                            lnkAddResult.Visible = false;
                                            divSubjectWiseGradeOLevel.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        //txtSec_RollNo.Enabled = false;
                                        //txtSec_CgpaW4S.Enabled = false;
                                        //txtSec_Marks.Enabled = false;
                                        //txtOutofSec_Marks.Enabled = false;
                                        lnkAddResult.Visible = false;
                                        divSubjectWiseGradeOLevel.Visible = false;
                                    }

                                    #endregion

                                }
                            }
                        }//end if(secondaryExam.ID > 0) 
                        #endregion
                        #region HSC
                        if (higherSecondaryExam != null)
                        {
                            if (higherSecondaryExam.ExamTypeID == 15) // International Baccalaureate
                            {
                                DAL.ExamDetail higherSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);
                                if (higherSecExamDetail != null && higherSecExamDetail.ID > 0)
                                {
                                    ddlHigherSec_ExamType.SelectedValue = "15"; // International Baccalaureate

                                    ddlHigherSec_EducationBrd.SelectedValue = higherSecExamDetail.EducationBoardID.ToString();
                                    ddlHigherSec_EducationBrd.Visible = false;
                                    txtHigherSec_EducationBrd.Text = higherSecExamDetail.Attribute1;
                                    txtHigherSec_EducationBrd.Visible = true;

                                    txtHigherSec_Institute.Text = higherSecExamDetail.Institute;
                                    txtHigherSec_Institute.Enabled = true;

                                    txtHigherSec_RollNo.Text = higherSecExamDetail.RollNo;
                                    //txtHigherSec_RollNo.Enabled = true;

                                    txtHigherSec_RegNo.Text = higherSecExamDetail.RegistrationNo;

                                    ddlHigherSec_GrpOrSub.SelectedValue = higherSecExamDetail.GroupOrSubjectID.ToString();
                                    trGroupOrSubjectHSC.Visible = false;

                                    ddlHigherSec_DivClass.SelectedValue = higherSecExamDetail.ResultDivisionID.ToString();
                                    trDivisionClassHSC.Visible = false;

                                    txtHigherSec_CgpaScore.Text = higherSecExamDetail.GPA.ToString();
                                    txtHigherSec_CgpaScore.Enabled = true;

                                    txtHigherSec_Marks.Text = higherSecExamDetail.Marks.ToString();

                                    txtOutofHigherSec_Marks.Text = higherSecExamDetail.AttributeDec1 == null ? "" : higherSecExamDetail.AttributeDec1.ToString();

                                    //trMarksForOLevelHSC.Visible = false;
                                    //trOutofMarksForOLevelHSC.Visible = false;


                                    txtHigherSec_GpaW4S.Text = higherSecExamDetail.GPAW4S.ToString();
                                    //trGPA4thSubjectHSC.Visible = false;

                                    ddlHigherSec_PassingYear.SelectedValue = higherSecExamDetail.PassingYear.ToString();
                                    ddlhsc_StudentCat.SelectedValue = higherSecExamDetail.AttributeInt2.ToString();
                                    ddlHigherSec_PassingYear.Enabled = true;
                                }
                            }
                            else
                            {

                                DAL.ExamDetail higherSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);
                                if (higherSecExamDetail != null && higherSecExamDetail.ID > 0)
                                {
                                    ddlHigherSec_ExamType.SelectedValue = higherSecondaryExam.ExamTypeID.ToString();
                                    ddlHigherSec_EducationBrd.SelectedValue = higherSecExamDetail.EducationBoardID.ToString();
                                    txtHigherSec_Institute.Text = higherSecExamDetail.Institute;
                                    //if (higherSecondaryExam.ExamTypeID == 7) //A-Level
                                    //{
                                    //    txtHigherSec_RollNo.Enabled = true;
                                    //}
                                    txtHigherSec_RollNo.Text = higherSecExamDetail.RollNo;
                                    txtHigherSec_RegNo.Text = higherSecExamDetail.RegistrationNo;
                                    ddlHigherSec_GrpOrSub.SelectedValue = higherSecExamDetail.GroupOrSubjectID.ToString();
                                    ddlHigherSec_DivClass.SelectedValue = higherSecExamDetail.ResultDivisionID.ToString();
                                    #region outofmarks hsc code remove in future
                                    //if (higherSecondaryExam.ExamTypeID == 2)
                                    //    trOutofMarksForOLevelHSC.Visible = false;
                                    #endregion
                                    if (higherSecExamDetail.ResultDivisionID == 5)
                                    {
                                        //txtHigherSec_CgpaScore.Enabled = true;
                                        txtHigherSec_CgpaScore.Text = higherSecExamDetail.GPA.ToString();
                                    }
                                    else
                                    {
                                        //txtHigherSec_CgpaScore.Enabled = false;
                                        txtHigherSec_CgpaScore.Text = null;
                                    }
                                    txtHigherSec_Marks.Text = higherSecExamDetail.Marks.ToString();
                                    txtOutofHigherSec_Marks.Text = higherSecExamDetail.AttributeDec1 == null ? "" : higherSecExamDetail.AttributeDec1.ToString();
                                    //if (higherSecondaryExam.ExamTypeID == 7)
                                    //{
                                    //    txtHigherSec_Marks.Enabled = false;
                                    //}
                                    //else
                                    //{
                                    //    txtHigherSec_Marks.Enabled = true;
                                    //}
                                    txtHigherSec_GpaW4S.Text = higherSecExamDetail.GPAW4S.ToString();
                                    ddlHigherSec_PassingYear.SelectedValue = higherSecExamDetail.PassingYear.ToString();
                                    ddlhsc_StudentCat.SelectedValue = higherSecExamDetail.AttributeInt2.ToString();


                                    #region Disabled All Field

                                    //ddlHigherSec_ExamType.Enabled = false;
                                    //ddlHigherSec_EducationBrd.Enabled = false;
                                    //txtHigherSec_Institute.Enabled = false;
                                    //ddlHigherSec_GrpOrSub.Enabled = false;
                                    //ddlHigherSec_DivClass.Enabled = false;
                                    //txtHigherSec_CgpaScore.Enabled = false;
                                    //ddlHigherSec_PassingYear.Enabled = false;

                                    if (higherSecondaryExam.ExamTypeID == 7 || educationCategoryId == 6) //A-Level && Master's Program
                                    {
                                        txtHigherSec_RollNo.Enabled = true;
                                        txtHigherSec_GpaW4S.Enabled = true;
                                        txtHigherSec_Marks.Enabled = true;
                                        if (higherSecExamDetail.EducationBoardID == 13 || higherSecExamDetail.EducationBoardID == 12 || higherSecExamDetail.EducationBoardID == 11) // A Level Cambridge And Edexel can not change total achived mark
                                        {
                                            //txtHigherSec_Marks.Enabled = false;
                                            //txtOutofHigherSec_Marks.Enabled = false;
                                            lnkAddALevelResult.Visible = true;
                                            divSubjectWiseGradeALevel.Visible = true;
                                            ClearALevelGrid();

                                            CheckAndBindGridViewOfALevel();
                                        }
                                        else
                                        {
                                            lnkAddALevelResult.Visible = false;
                                            divSubjectWiseGradeALevel.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        //txtHigherSec_RollNo.Enabled = false;
                                        //txtHigherSec_GpaW4S.Enabled = false;
                                        //txtHigherSec_Marks.Enabled = false;
                                        //txtOutofHigherSec_Marks.Enabled = false;
                                        lnkAddALevelResult.Visible = false;
                                        divSubjectWiseGradeALevel.Visible = false;
                                    }

                                    #endregion


                                }
                            }
                        }// end if(higherSecondaryExam.ID > 0) 
                        #endregion
                        #region Bachelor
                        if (undergradExam != null)
                        {
                            DAL.ExamDetail undergradExamDetail = db.GetExamDetailByID_ND(undergradExam.ExamDetailsID);
                            if (undergradExamDetail != null && undergradExamDetail.ID > 0)
                            {
                                txtUndergrad_Institute.Text = undergradExamDetail.Institute;
                                ddlUndergrad_ProgramDegree.SelectedValue = undergradExamDetail.UndgradGradProgID.ToString();
                                if (ddlUndergrad_ProgramDegree.SelectedItem.Text == "OTHERS")
                                {
                                    txtUndergrad_ProgOthers.Text = undergradExamDetail.OtherProgram;
                                    txtUndergrad_ProgOthers.Enabled = true;
                                }
                                else
                                {
                                    txtUndergrad_ProgOthers.Text = null;
                                    //txtUndergrad_ProgOthers.Enabled = false;
                                }
                                //ddlUndergrad_GrpOrSub.SelectedValue = undergradExamDetail.GroupOrSubjectID.ToString();
                                ddlUndergrad_DivClass.SelectedValue = undergradExamDetail.ResultDivisionID.ToString();
                                if (undergradExamDetail.ResultDivisionID == 5)
                                {
                                    txtUndergrad_CgpaScore.Text = undergradExamDetail.CGPA.ToString();
                                    txtUndergrad_CgpaScore.Enabled = true;
                                }
                                else
                                {
                                    txtUndergrad_CgpaScore.Text = null;
                                    //txtUndergrad_CgpaScore.Enabled = false;
                                }
                                ddlUndergrad_PassingYear.SelectedValue = undergradExamDetail.PassingYear.ToString();
                            }
                        }//end if(undergradExam.ID > 0) 
                        #endregion
                        #region Masters
                        if (gradExam != null)
                        {
                            DAL.ExamDetail gradExamDetail = db.GetExamDetailByID_ND(gradExam.ExamDetailsID);
                            if (gradExamDetail != null && gradExamDetail.ID > 0)
                            {
                                txtGraduate_Institute.Text = gradExamDetail.Institute;
                                ddlGraduate_ProgramDegree.SelectedValue = gradExamDetail.UndgradGradProgID.ToString();
                                if (ddlGraduate_ProgramDegree.SelectedItem.Text == "OTHERS")
                                {
                                    txtGraduate_ProgOthers.Text = gradExamDetail.OtherProgram;
                                    txtGraduate_ProgOthers.Enabled = true;
                                }
                                else
                                {
                                    txtGraduate_ProgOthers.Text = null;
                                    //txtGraduate_ProgOthers.Enabled = false;
                                }
                                //ddlGraduate_GrpOrSub.SelectedValue = gradExamDetail.GroupOrSubjectID.ToString();
                                ddlGraduate_DivClass.SelectedValue = gradExamDetail.ResultDivisionID.ToString();
                                if (gradExamDetail.ResultDivisionID == 5)
                                {
                                    txtGraduate_CgpaScore.Text = gradExamDetail.CGPA.ToString();
                                    txtGraduate_CgpaScore.Enabled = true;
                                }
                                else
                                {
                                    txtGraduate_CgpaScore.Text = null;
                                    //txtGraduate_CgpaScore.Enabled = false;
                                }
                                ddlGraduate_PassingYear.SelectedValue = gradExamDetail.PassingYear.ToString();
                            }
                        }// end if(gradExam.ID > 0) 
                        #endregion

                        #region Visible False Gpa without 4 and total Mark

                        try
                        {
                            int sscTypeId = Convert.ToInt32(ddlSec_ExamType.SelectedValue);
                            int hscTypeId = Convert.ToInt32(ddlHigherSec_ExamType.SelectedValue);

                            //trGPA4thSubjectSSC.Visible = false;

                            //trGPA4thSubjectHSC.Visible = false;

                            if (sscTypeId == 1 || sscTypeId == 6 || sscTypeId == 12) // SSC, Dakhil, SSC (Vocational)
                            {
                                txtSec_CgpaW4S.Text = "0";
                                txtSec_Marks.Text = "0";

                                //trMarksForOLevelSSC.Visible = false;
                            }
                            else
                            {
                                //trMarksForOLevelSSC.Visible = true;
                            }

                            if (hscTypeId == 2 || hscTypeId == 8 || hscTypeId == 9 || hscTypeId == 13) // HSC, Alim, Diploma, HSC (Vocational)
                            {
                                txtHigherSec_GpaW4S.Text = "0";
                                txtHigherSec_Marks.Text = "0";

                                //trMarksForOLevelHSC.Visible = false;
                            }
                            else
                            {
                                //trMarksForOLevelHSC.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        #endregion




                        if ((secondaryExam != null && secondaryExam.ExamTypeID == 5) || (higherSecondaryExam != null && higherSecondaryExam.ExamTypeID == 7))
                            btnSave_Education.Visible = true;

                        //#region New Added For Total Out of Marks

                        //int NeedToSave = 0;

                        //if (string.IsNullOrEmpty(txtOutofSec_Marks.Text))
                        //    NeedToSave = 1;
                        //if (string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text))
                        //    NeedToSave = 1;

                        //#region SSC or Equivalent


                        //if (secondaryExam != null)
                        //{
                        //    DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                        //    if (secExamDetail != null)
                        //    {
                        //        int ExamTypeId = Convert.ToInt32(secondaryExam.ExamTypeID);
                        //        int BoardId = Convert.ToInt32(secExamDetail.EducationBoardID);
                        //        int PassingYearId = Convert.ToInt32(secExamDetail.PassingYear);
                        //        int GroupId = Convert.ToInt32(secExamDetail.GroupOrSubjectID);

                        //        var totalMarkObj = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.Where(x => x.ExamTypeID == ExamTypeId
                        //          && x.EducationBoardID == BoardId && x.Year == PassingYearId && (x.GroupOrSubjectID == GroupId || x.GroupOrSubjectID == null)).FirstOrDefault();

                        //        if (totalMarkObj != null)
                        //            txtOutofSec_Marks.Text = totalMarkObj.TotalMarks.ToString();

                        //    }

                        //}

                        //#endregion

                        //#region HSC or Equivalent

                        //if (higherSecondaryExam != null)
                        //{
                        //    DAL.ExamDetail higherSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);
                        //    if (higherSecExamDetail != null)
                        //    {
                        //        int ExamTypeId = Convert.ToInt32(higherSecondaryExam.ExamTypeID);
                        //        int BoardId = Convert.ToInt32(higherSecExamDetail.EducationBoardID);
                        //        int PassingYearId = Convert.ToInt32(higherSecExamDetail.PassingYear);
                        //        int GroupId = Convert.ToInt32(higherSecExamDetail.GroupOrSubjectID);

                        //        var totalMarkObj = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.Where(x => x.ExamTypeID == ExamTypeId
                        //          && x.EducationBoardID == BoardId && x.Year == PassingYearId && (x.GroupOrSubjectID == GroupId || x.GroupOrSubjectID == null)).FirstOrDefault();

                        //        if (totalMarkObj != null)
                        //            txtOutofHigherSec_Marks.Text = totalMarkObj.TotalMarks.ToString();

                        //    }

                        //}
                        //#endregion

                        //if (NeedToSave == 1)
                        //{
                        //    showAlert("Please save and next after fill up all information");
                        //}

                        //#endregion


                    }// end using

                    #region N/A -- Hide Save and Next Button for Bachelor Program Because Admission is closed
                    //try
                    //{
                    //    List<DAL.PropertySetup> propertySetupList = null; //new DAL.CandidateFormSl();
                    //    int educationCategoryId = -1;
                    //    using (var db = new GeneralDataManager())
                    //    {
                    //        propertySetupList = db.AdmissionDB.PropertySetups.Where(x => x.IsActive == true).ToList();
                    //    }
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                    //    }

                    //    ///<summary>
                    //    ///
                    //    /// IsActive == true && IsVisible == false
                    //    /// Kew Submit Button Dekte prbe na.
                    //    /// jokon admission date sas hoea jbe
                    //    /// 
                    //    /// 
                    //    /// IsActive == true && IsVisible == true 
                    //    /// sober jnno Open thkbe. Final Submit Dileo
                    //    /// 
                    //    /// 
                    //    /// IsActive == false && IsVisible == any
                    //    /// Sober jnno Open but final Submit dile r Show korbe na tader jnno
                    //    /// 
                    //    /// </summary>

                    //    DAL.AdditionalInfo addFsModel = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        addFsModel = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                    //    }

                    //    if (addFsModel != null && Convert.ToBoolean(addFsModel.IsFinalSubmit) == true)
                    //    {
                    //        btnSave_Education.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).ToList().Count > 0)
                    //        {
                    //            if (educationCategoryId == 4)
                    //            {
                    //                btnSave_Education.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
                    //            }
                    //            else
                    //            {
                    //                DAL.CandidateFormSl formSl = null;
                    //                using (var db = new CandidateDataManager())
                    //                {
                    //                    formSl = db.GetCandidateFormSlByCandID_AD(cId);
                    //                }

                    //                if (formSl != null && formSl.AdmissionSetup != null)
                    //                {
                    //                    DAL.AdmissionUnitProgram admUnitProg = null;
                    //                    using (var db = new OfficeDataManager())
                    //                    {
                    //                        admUnitProg = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == formSl.AcaCalID
                    //                                                                                    && x.EducationCategoryID == educationCategoryId
                    //                                                                                    && x.AdmissionUnitID == formSl.AdmissionSetup.AdmissionUnitID
                    //                                                                                    && x.IsActive == true).FirstOrDefault();
                    //                    }

                    //                    if (admUnitProg != null)
                    //                    {
                    //                        int programId = admUnitProg.ProgramID;

                    //                        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).FirstOrDefault() != null)
                    //                        {
                    //                            btnSave_Education.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                    //                        }
                    //                        else
                    //                        {
                    //                            btnSave_Education.Visible = false;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        btnSave_Education.Visible = false;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Education.Visible = false;
                    //                }
                    //            }
                    //        }
                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    #endregion


                }// if(cId > 0)
            }// if(uId > 0)
        }

        protected void btnSave_Education_Click(object sender, EventArgs e)
        {
            long cId = -1;

            try
            {

                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }

                int educationCategoryId = -1;

                if (cId > 0 && uId > 0)
                {
                    string logOldObject = string.Empty;
                    string logNewObject = string.Empty;

                    #region Remove this code in future
                    int ddlSSC = Convert.ToInt32(ddlSec_ExamType.SelectedValue);
                    int ddlHSC = Convert.ToInt32(ddlHigherSec_ExamType.SelectedValue);
                    #endregion

                    DAL.BasicInfo basicInfo = null;
                    DAL.CandidatePayment candidatePayment = null;

                    DAL.Exam secondaryExam = null;
                    DAL.Exam highSecondaryExam = null;
                    DAL.Exam undergradExam = null;
                    DAL.Exam gradExam = null;

                    int AlevelAppearedCandidate = 0;

                    using (var db = new CandidateDataManager())
                    {
                        educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                    }


                    using (var db = new CandidateDataManager())
                    {
                        basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                        candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();

                        List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);

                        if (examList != null)
                        {
                            secondaryExam = db.GetSecondaryExamByCandidateID_AD(cId); //examList.Where(a => a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6).FirstOrDefault();
                            highSecondaryExam = db.GetHigherSecdExamByCandidateID_AD(cId); //examList.Where(a => a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9).FirstOrDefault();
                            undergradExam = db.GetUndergradExamByCandidateID_AD(cId); //examList.Where(a => a.ExamTypeID == 3 || a.ExamTypeID == 10).FirstOrDefault();
                            gradExam = db.GetGradExamByCandidateID_AD(cId); //examList.Where(a => a.ExamTypeID == 4).FirstOrDefault();
                        }
                    }


                    #region O level And A Level Student Required Obtained Marks is 200(O level) and 80(A level) . New req given by asad (16-11-2024)

                    try
                    {
                        if (educationCategoryId != 6) // without masters for all TotalMarks is required
                        {
                            if (secondaryExam != null && secondaryExam.ExamTypeID == 5) // O level
                            {
                                decimal totalObt = Convert.ToDecimal(txtSec_Marks.Text);
                                if (totalObt < 200)
                                {
                                    lblMessageEducation.Text = "Total Obtained Marks must be greater than or equal to 200 for O level Student";
                                    messagePanel_Education.CssClass = "alert alert-danger";
                                    messagePanel_Education.Visible = true;
                                    return;
                                }
                            }

                            if (highSecondaryExam != null && highSecondaryExam.ExamTypeID == 7) // A level
                            {
                                if (highSecondaryExam.Attribute1 == null || highSecondaryExam.Attribute1 == "")
                                {
                                    decimal totalObt = Convert.ToDecimal(txtHigherSec_Marks.Text);
                                    if (totalObt < 80)
                                    {
                                        lblMessageEducation.Text = "Total Obtained Marks must be greater than or equal to 80 for A level Student";
                                        messagePanel_Education.CssClass = "alert alert-danger";
                                        messagePanel_Education.Visible = true;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessageEducation.Text = "Total Obtained Marks must be greater than or equal to 200 for O level & 80 for A level Student";
                        messagePanel_Education.CssClass = "alert alert-danger";
                        messagePanel_Education.Visible = true;
                        return;
                    }

                    #endregion


                    if (highSecondaryExam != null && highSecondaryExam.ExamTypeID == 7 && highSecondaryExam.Attribute1 != null && highSecondaryExam.Attribute1 == "Appeared")
                        AlevelAppearedCandidate = 1;

                    if (educationCategoryId != 6) // without masters for all TotalMarks is required
                    {
                        if (string.IsNullOrEmpty(txtSec_Marks.Text) || (string.IsNullOrEmpty(txtHigherSec_Marks.Text) && AlevelAppearedCandidate == 0))
                        {
                            lblMessageEducation.Text = "Total Marks is required";
                            messagePanel_Education.CssClass = "alert alert-danger";
                            messagePanel_Education.Visible = true;
                            return;
                        }

                        //ssc and hsc outofmarks check null

                        if (ddlSSC != 1 && ddlHSC != 2)
                        {
                            if (string.IsNullOrEmpty(txtOutofSec_Marks.Text) || (string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text) && AlevelAppearedCandidate == 0))
                            {
                                lblMessageEducation.Text = "Total Out of Marks is required";
                                messagePanel_Education.CssClass = "alert alert-danger";
                                messagePanel_Education.Visible = true;
                                return;
                            }
                        }
                    }


                    // need to make sure this code does not work for ssc and hsc


                    #region Check Total Mark Is Less than or equal to total required mark
                    if (ddlSSC != 1 && ddlHSC != 2)
                    {
                        try
                        {
                            decimal totalsscMark = Convert.ToDecimal(txtSec_Marks.Text);
                            decimal totaloutofsscMark = Convert.ToDecimal(txtOutofSec_Marks.Text);
                            decimal totalhscMark = Convert.ToDecimal(txtHigherSec_Marks.Text);
                            decimal totaloutofhscMark = Convert.ToDecimal(txtOutofHigherSec_Marks.Text);

                            if (totalsscMark > totaloutofsscMark)
                            {
                                lblMessageEducation.Text = "Total SSC Marks must be less than or equal to total out of marks";
                                messagePanel_Education.CssClass = "alert alert-danger";
                                messagePanel_Education.Visible = true;
                                return;
                            }
                            if (AlevelAppearedCandidate == 0)
                            {
                                if (totalhscMark > totaloutofhscMark)
                                {
                                    lblMessageEducation.Text = "Total HSC Marks must be less than or equal to total out of marks";
                                    messagePanel_Education.CssClass = "alert alert-danger";
                                    messagePanel_Education.Visible = true;
                                    return;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    #endregion


                    #region Check Student Category Is give or not

                    if (ddlSec_StudentCat.SelectedValue == "0" || ddlhsc_StudentCat.SelectedValue == "0")
                    {
                        lblMessageEducation.Text = "Please select Student Category";
                        messagePanel_Education.CssClass = "alert alert-danger";
                        messagePanel_Education.Visible = true;
                        return;
                    }


                    #endregion

                    #region Biology GPA is mandatory

                    if (string.IsNullOrEmpty(txtSec_CgpaW4S.Text) || string.IsNullOrEmpty(txtHigherSec_GpaW4S.Text) || Convert.ToDecimal(txtSec_CgpaW4S.Text) <= 0 || Convert.ToDecimal(txtHigherSec_GpaW4S.Text) <= 0)
                    {
                        lblMessageEducation.Text = "GPA of Biology is required";
                        messagePanel_Education.CssClass = "alert alert-danger";
                        messagePanel_Education.Visible = true;
                        return;
                    }

                    #endregion




                    //string hfsscgpa = hfSscGPA.Value;
                    //string hfhscgpa = hfHscGPA.Value;

                    #region SSC/O-Level ======================================================

                    if (secondaryExam != null) //secondary exam exist.
                    {
                        if (secondaryExam.ExamTypeID == 14) // International Baccalaureate
                        {
                            #region IB (SSC Equivalent)
                            DAL.ExamDetail secondaryExamDetails = new DAL.ExamDetail();

                            secondaryExamDetails.ID = secondaryExam.ExamDetail.ID;

                            secondaryExamDetails.EducationBoardID = 11; // Other
                            secondaryExamDetails.Attribute1 = txtSec_EducationBrd.Text; // Education Board Name
                            secondaryExamDetails.Institute = txtSec_Institute.Text;
                            secondaryExamDetails.UndgradGradProgID = 1; // N/A
                            secondaryExamDetails.GroupOrSubjectID = 2; // Other
                            secondaryExamDetails.ResultDivisionID = 5; // GPA

                            if (!string.IsNullOrEmpty(txtSec_RollNo.Text))
                            {
                                secondaryExamDetails.RollNo = txtSec_RollNo.Text;
                            }
                            if (!string.IsNullOrEmpty(txtSec_CgpaScore.Text))
                            {
                                secondaryExamDetails.GPA = Convert.ToDecimal(txtSec_CgpaScore.Text);
                            }
                            //secondaryExamDetails.Marks = Convert.ToDecimal(lblOLevelResult.Text);


                            secondaryExamDetails.PassingYear = Convert.ToInt32(ddlSec_PassingYear.SelectedValue);
                            secondaryExamDetails.AttributeInt2 = Convert.ToInt32(ddlhsc_StudentCat.SelectedValue);

                            secondaryExamDetails.CreatedBy = secondaryExam.ExamDetail.CreatedBy;
                            secondaryExamDetails.DateCreated = secondaryExam.ExamDetail.DateCreated;

                            secondaryExamDetails.DateModified = DateTime.Now;
                            secondaryExamDetails.ModifiedBy = cId;


                            using (var dbSecExmDet = new CandidateDataManager())
                            {
                                dbSecExmDet.Update<DAL.ExamDetail>(secondaryExamDetails);
                            }
                            #endregion
                        }
                        else
                        {
                            DAL.ExamDetail secExmDtlObj = secondaryExam.ExamDetail;

                            if (secExmDtlObj != null) // secondary exam detail exist. Update.
                            {
                                logOldObject = string.Empty;
                                logOldObject = GenerateLogStringFromObject(secondaryExam, secExmDtlObj);

                                secExmDtlObj.EducationBoardID = Int32.Parse(ddlSec_EducationBrd.SelectedValue);
                                secExmDtlObj.Institute = txtSec_Institute.Text.Trim();
                                secExmDtlObj.UndgradGradProgID = 1;
                                secExmDtlObj.RollNo = txtSec_RollNo.Text.Trim();
                                secExmDtlObj.RegistrationNo = txtSec_RegNo.Text.Trim();
                                secExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlSec_GrpOrSub.SelectedValue);

                                //if (ddlSec_DivClass.Enabled == true)
                                //{
                                secExmDtlObj.ResultDivisionID = Int32.Parse(ddlSec_DivClass.SelectedValue);
                                //}
                                //else
                                //{
                                //    secExmDtlObj.ResultDivisionID = 5; //5 = GPA
                                //}

                                if (ddlSec_DivClass.SelectedItem.Text == "GPA")
                                {
                                    secExmDtlObj.GPA = Decimal.Parse(txtSec_CgpaScore.Text.Trim());
                                }
                                else
                                {
                                    secExmDtlObj.GPA = null;
                                }

                                if (!string.IsNullOrEmpty(txtSec_CgpaW4S.Text.Trim()))
                                {
                                    secExmDtlObj.GPAW4S = Decimal.Parse(txtSec_CgpaW4S.Text.Trim());
                                }
                                else
                                {
                                    secExmDtlObj.GPAW4S = null;
                                }

                                if (!string.IsNullOrEmpty(txtSec_Marks.Text.Trim()))
                                {
                                    secExmDtlObj.Marks = Decimal.Parse(txtSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    secExmDtlObj.Marks = null;
                                }

                                if (!string.IsNullOrEmpty(txtOutofSec_Marks.Text.Trim()))
                                {
                                    secExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    secExmDtlObj.AttributeDec1 = null;
                                }

                                secExmDtlObj.PassingYear = Int32.Parse(ddlSec_PassingYear.SelectedValue);
                                secExmDtlObj.AttributeInt2 = Int32.Parse(ddlSec_StudentCat.SelectedValue);

                                secExmDtlObj.DateModified = DateTime.Now;
                                secExmDtlObj.ModifiedBy = cId;

                                using (var dbUpdateSecExamDetails = new CandidateDataManager())
                                {
                                    dbUpdateSecExamDetails.Update<DAL.ExamDetail>(secExmDtlObj);

                                    logNewObject = string.Empty;
                                    logNewObject = GenerateLogStringFromObject(secondaryExam, secExmDtlObj);
                                }

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    #region N/A
                                    //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //dLog.DateCreated = DateTime.Now;
                                    //dLog.EventName = "Education Info Update (SSC) (Admin)";
                                    //dLog.PageName = "CandApplicationEducation.aspx";
                                    //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                    //dLog.UserId = uId;
                                    //dLog.Attribute1 = "Success";
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //LogWriter.DataLogWriter(dLog); 
                                    #endregion

                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "Education Info Update (SSC) (Candidate)";
                                    dLog.PageName = "ApplicationEducation.aspx";
                                    dLog.OldData = logOldObject;
                                    dLog.NewData = logNewObject;
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //try
                                //{
                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.EventName = "Education Info Update (SSC) (Candidate)";
                                //    dLog.PageName = "ApplicationEducation.aspx";
                                //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = "Success";
                                //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion
                            }
                            else //secondary exam detail does not exist. create update exam details.
                            {
                                DAL.ExamDetail newSecExmDtlObj = new DAL.ExamDetail();

                                newSecExmDtlObj.EducationBoardID = Int32.Parse(ddlSec_EducationBrd.SelectedValue);
                                newSecExmDtlObj.Institute = txtSec_Institute.Text.Trim();
                                newSecExmDtlObj.UndgradGradProgID = 1;
                                newSecExmDtlObj.RollNo = txtSec_RollNo.Text.Trim();
                                newSecExmDtlObj.RegistrationNo = txtSec_RegNo.Text.Trim();
                                newSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlSec_GrpOrSub.SelectedValue);

                                //if (ddlSec_DivClass.Enabled == true)
                                //{
                                newSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlSec_DivClass.SelectedValue);
                                //}
                                //else
                                //{
                                //    newSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                                //}

                                if (ddlSec_DivClass.SelectedItem.Text == "GPA")
                                {
                                    newSecExmDtlObj.GPA = Decimal.Parse(txtSec_CgpaScore.Text.Trim());
                                }
                                else
                                {
                                    newSecExmDtlObj.GPA = null;
                                }

                                if (!string.IsNullOrEmpty(txtSec_CgpaW4S.Text.Trim()))
                                {
                                    newSecExmDtlObj.GPAW4S = Decimal.Parse(txtSec_CgpaW4S.Text.Trim());
                                }
                                else
                                {
                                    newSecExmDtlObj.GPAW4S = null;
                                }

                                if (!string.IsNullOrEmpty(txtSec_Marks.Text.Trim()))
                                {
                                    newSecExmDtlObj.Marks = Decimal.Parse(txtSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    newSecExmDtlObj.Marks = null;
                                }

                                if (!string.IsNullOrEmpty(txtOutofSec_Marks.Text.Trim()))
                                {
                                    newSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    newSecExmDtlObj.AttributeDec1 = null;
                                }

                                newSecExmDtlObj.PassingYear = Int32.Parse(ddlSec_PassingYear.SelectedValue);
                                newSecExmDtlObj.AttributeInt2 = Int32.Parse(ddlSec_StudentCat.SelectedValue);

                                newSecExmDtlObj.DateCreated = DateTime.Now;
                                newSecExmDtlObj.CreatedBy = cId;

                                long newSecExmDtlObjID = -1;
                                using (var dbInsertSecExamDetails = new CandidateDataManager())
                                {
                                    dbInsertSecExamDetails.Insert<DAL.ExamDetail>(newSecExmDtlObj);
                                    newSecExmDtlObjID = newSecExmDtlObj.ID;
                                }
                                if (newSecExmDtlObjID > 0)
                                {
                                    secondaryExam.ExamDetailsID = newSecExmDtlObjID;

                                    using (var dbUpdateSecExam = new CandidateDataManager())
                                    {
                                        dbUpdateSecExam.Update<DAL.Exam>(secondaryExam);
                                    }

                                    logNewObject = string.Empty;
                                    logNewObject = GenerateLogStringFromObject(secondaryExam, newSecExmDtlObj);

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        #region N/A
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Education Info Update (SSC) (Admin)";
                                        //dLog.PageName = "CandApplicationEducation.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                        //dLog.UserId = uId;
                                        //dLog.Attribute1 = "Success";
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //LogWriter.DataLogWriter(dLog); 
                                        #endregion

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Education Info Update (SSC) (Candidate)";
                                        dLog.PageName = "ApplicationEducation.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = logNewObject;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Education Info Update (SSC) (Candidate)";
                                    //    dLog.PageName = "ApplicationEducation.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion
                                }
                            }
                        }
                    }
                    else //secondary exam does not exist.
                    {

                        DAL.ExamDetail newSecExmDtlObj = new DAL.ExamDetail();

                        newSecExmDtlObj.EducationBoardID = Int32.Parse(ddlSec_EducationBrd.SelectedValue);
                        newSecExmDtlObj.Institute = txtSec_Institute.Text.Trim();
                        newSecExmDtlObj.UndgradGradProgID = 1;
                        newSecExmDtlObj.RollNo = txtSec_RollNo.Text.Trim();
                        newSecExmDtlObj.RegistrationNo = txtSec_RegNo.Text.Trim();
                        newSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlSec_GrpOrSub.SelectedValue);

                        //if (ddlSec_DivClass.Enabled == true)
                        //{
                        newSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlSec_DivClass.SelectedValue);
                        //}
                        //else
                        //{
                        //    newSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                        //}

                        if (ddlSec_DivClass.SelectedItem.Text == "GPA")
                        {
                            newSecExmDtlObj.GPA = Decimal.Parse(txtSec_CgpaScore.Text.Trim());
                        }
                        else
                        {
                            newSecExmDtlObj.GPA = null;
                        }

                        if (!string.IsNullOrEmpty(txtSec_CgpaW4S.Text.Trim()))
                        {
                            newSecExmDtlObj.GPAW4S = Decimal.Parse(txtSec_CgpaW4S.Text.Trim());
                        }
                        else
                        {
                            newSecExmDtlObj.GPAW4S = null;
                        }

                        if (!string.IsNullOrEmpty(txtSec_Marks.Text.Trim()))
                        {
                            newSecExmDtlObj.Marks = Decimal.Parse(txtSec_Marks.Text.Trim());
                        }
                        else
                        {
                            newSecExmDtlObj.Marks = null;
                        }

                        if (!string.IsNullOrEmpty(txtOutofSec_Marks.Text.Trim()))
                        {
                            newSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofSec_Marks.Text.Trim());
                        }
                        else
                        {
                            newSecExmDtlObj.AttributeDec1 = null;
                        }

                        newSecExmDtlObj.PassingYear = Int32.Parse(ddlSec_PassingYear.SelectedValue);
                        newSecExmDtlObj.AttributeInt2 = Int32.Parse(ddlSec_StudentCat.SelectedValue);

                        newSecExmDtlObj.DateCreated = DateTime.Now;
                        newSecExmDtlObj.CreatedBy = cId;

                        DAL.Exam newSecExamObj = new DAL.Exam();

                        newSecExamObj.CandidateID = cId;
                        newSecExamObj.ExamTypeID = Int32.Parse(ddlSec_ExamType.SelectedValue);
                        newSecExamObj.CreatedBy = cId;
                        newSecExamObj.DateCreated = DateTime.Now;

                        using (var dbInsertSecExamDtl = new CandidateDataManager())
                        {
                            dbInsertSecExamDtl.Insert<DAL.ExamDetail>(newSecExmDtlObj);
                            newSecExamObj.ExamDetailsID = newSecExmDtlObj.ID;
                        }
                        using (var dbInsertSecExam = new CandidateDataManager())
                        {
                            dbInsertSecExam.Insert<DAL.Exam>(newSecExamObj);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(newSecExamObj, newSecExmDtlObj);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Education Info Insert (SSC) (Admin)";
                            //dLog.PageName = "CandApplicationEducation.aspx";
                            //dLog.NewData = logNewObject;
                            //dLog.UserId = uId;
                            //dLog.Attribute1 = "Success";
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                            //LogWriter.DataLogWriter(dLog);

                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.CandidateId = cId;
                            dLog.EventName = "Education Info Insert (SSC) (Candidate)";
                            dLog.PageName = "ApplicationEducation.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = logNewObject;
                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion

                        //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        //try
                        //{
                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        //    dLog.DateCreated = DateTime.Now;
                        //    dLog.EventName = "Education Info Insert (SSC) (Candidate)";
                        //    dLog.PageName = "ApplicationEducation.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Education Information.";
                        //    dLog.UserId = uId;
                        //    dLog.Attribute1 = "Success";
                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                        //    LogWriter.DataLogWriter(dLog);
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //#endregion
                    }

                    #endregion SSC/O-Level 

                    #region HSC/A-Level/Diploma ==============================================

                    if (highSecondaryExam != null) //higher secondary exam exist.
                    {
                        if (highSecondaryExam.ExamTypeID == 15) // International Baccalaureate
                        {
                            #region IB (HSC Equivalent)

                            //logOldObject = string.Empty;
                            //logOldObject = GenerateLogStringFromObject(highSecondaryExam, higherSecExmDtlObj);

                            DAL.ExamDetail higherSecondaryExamDetails = new DAL.ExamDetail();

                            higherSecondaryExamDetails.ID = highSecondaryExam.ExamDetail.ID;

                            higherSecondaryExamDetails.EducationBoardID = 11; // Other
                            higherSecondaryExamDetails.Attribute1 = txtHigherSec_EducationBrd.Text; // Education Board Name
                            higherSecondaryExamDetails.Institute = txtHigherSec_Institute.Text;
                            higherSecondaryExamDetails.UndgradGradProgID = 1; // N/A
                            higherSecondaryExamDetails.GroupOrSubjectID = 2; // Other
                            higherSecondaryExamDetails.ResultDivisionID = 5; // GPA

                            if (!string.IsNullOrEmpty(txtHigherSec_RollNo.Text))
                            {
                                higherSecondaryExamDetails.RollNo = txtHigherSec_RollNo.Text;
                            }
                            if (!string.IsNullOrEmpty(txtHigherSec_CgpaScore.Text))
                            {
                                higherSecondaryExamDetails.GPA = Convert.ToDecimal(txtHigherSec_CgpaScore.Text);
                            }
                            //higherSecondaryExamDetails.Marks = Convert.ToDecimal(lblALevelResult.Text);

                            higherSecondaryExamDetails.PassingYear = Convert.ToInt32(ddlHigherSec_PassingYear.SelectedValue);
                            higherSecondaryExamDetails.AttributeInt2 = Convert.ToInt32(ddlhsc_StudentCat.SelectedValue);

                            higherSecondaryExamDetails.CreatedBy = highSecondaryExam.ExamDetail.CreatedBy;
                            higherSecondaryExamDetails.DateCreated = highSecondaryExam.ExamDetail.DateCreated;

                            higherSecondaryExamDetails.DateModified = DateTime.Now;
                            higherSecondaryExamDetails.ModifiedBy = cId;

                            using (var dbHighSecExmDtl = new CandidateDataManager())
                            {
                                dbHighSecExmDtl.Update<DAL.ExamDetail>(higherSecondaryExamDetails);
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(highSecondaryExam, higherSecondaryExamDetails);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Education Info Update (HSC) (Admin)";
                                //dLog.PageName = "CandApplicationEducation.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                //dLog.UserId = uId;
                                //dLog.Attribute1 = "Success";
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //LogWriter.DataLogWriter(dLog);

                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.CandidateId = cId;
                                dLog.EventName = "Education Info Update (HSC) (Candidate)";
                                dLog.PageName = "ApplicationEducation.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = logNewObject;
                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            #endregion
                        }
                        else
                        {
                            DAL.ExamDetail higherSecExmDtlObj = highSecondaryExam.ExamDetail;

                            if (higherSecExmDtlObj != null) // higher secondary exam detail exist. Update.
                            {

                                logOldObject = string.Empty;
                                logOldObject = GenerateLogStringFromObject(highSecondaryExam, higherSecExmDtlObj);

                                higherSecExmDtlObj.EducationBoardID = Int32.Parse(ddlHigherSec_EducationBrd.SelectedValue);
                                higherSecExmDtlObj.Institute = txtHigherSec_Institute.Text.Trim();
                                higherSecExmDtlObj.UndgradGradProgID = 1;
                                higherSecExmDtlObj.RollNo = txtHigherSec_RollNo.Text.Trim();
                                higherSecExmDtlObj.RegistrationNo = txtHigherSec_RegNo.Text.Trim();
                                higherSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlHigherSec_GrpOrSub.SelectedValue);

                                //if (ddlHigherSec_DivClass.Enabled == true)
                                //{
                                higherSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                                //}
                                //else
                                //{
                                //    higherSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                                //}

                                if (ddlHigherSec_DivClass.SelectedItem.Text == "GPA")
                                {
                                    higherSecExmDtlObj.GPA = Decimal.Parse(txtHigherSec_CgpaScore.Text.Trim());
                                }
                                else
                                {
                                    higherSecExmDtlObj.GPA = null;
                                }

                                if (!string.IsNullOrEmpty(txtHigherSec_GpaW4S.Text.Trim()))
                                {
                                    higherSecExmDtlObj.GPAW4S = Decimal.Parse(txtHigherSec_GpaW4S.Text.Trim());
                                }
                                else
                                {
                                    higherSecExmDtlObj.GPAW4S = null;
                                }

                                if (!string.IsNullOrEmpty(txtHigherSec_Marks.Text.Trim()))
                                {
                                    higherSecExmDtlObj.Marks = Decimal.Parse(txtHigherSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    higherSecExmDtlObj.Marks = null;
                                }
                                //send empty string here
                                if (!string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text.Trim()))
                                {
                                    higherSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofHigherSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    higherSecExmDtlObj.AttributeDec1 = null;
                                }

                                higherSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);
                                higherSecExmDtlObj.AttributeInt2 = Int32.Parse(ddlhsc_StudentCat.SelectedValue);

                                higherSecExmDtlObj.DateModified = DateTime.Now;
                                higherSecExmDtlObj.ModifiedBy = cId;

                                using (var dbUpdateHighSecExamDetails = new CandidateDataManager())
                                {
                                    dbUpdateHighSecExamDetails.Update<DAL.ExamDetail>(higherSecExmDtlObj);
                                }

                                logNewObject = string.Empty;
                                logNewObject = GenerateLogStringFromObject(highSecondaryExam, higherSecExmDtlObj);

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //dLog.DateCreated = DateTime.Now;
                                    //dLog.EventName = "Education Info Update (HSC) (Admin)";
                                    //dLog.PageName = "CandApplicationEducation.aspx";
                                    //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                    //dLog.UserId = uId;
                                    //dLog.Attribute1 = "Success";
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //LogWriter.DataLogWriter(dLog);

                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "Education Info Update (HSC) (Candidate)";
                                    dLog.PageName = "ApplicationEducation.aspx";
                                    dLog.OldData = logOldObject;
                                    dLog.NewData = logNewObject;
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //try
                                //{
                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.EventName = "Education Info Update (HSC) (Candidate)";
                                //    dLog.PageName = "ApplicationEducation.aspx";
                                //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = "Success";
                                //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion
                            }
                            else //higher secondary exam detail does not exist. create update exam details.
                            {
                                DAL.ExamDetail newHigherSecExmDtlObj = new DAL.ExamDetail();

                                newHigherSecExmDtlObj.EducationBoardID = Int32.Parse(ddlHigherSec_EducationBrd.SelectedValue);
                                newHigherSecExmDtlObj.Institute = txtHigherSec_Institute.Text.Trim();
                                newHigherSecExmDtlObj.UndgradGradProgID = 1;
                                newHigherSecExmDtlObj.RollNo = txtHigherSec_RollNo.Text.Trim();
                                newHigherSecExmDtlObj.RegistrationNo = txtHigherSec_RegNo.Text.Trim();
                                newHigherSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlHigherSec_GrpOrSub.SelectedValue);

                                //if (ddlHigherSec_DivClass.Enabled == true)
                                //{
                                newHigherSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                                //}
                                //else
                                //{
                                //    newHigherSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                                //}

                                //newHigherSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                                if (ddlHigherSec_DivClass.SelectedItem.Text == "GPA")
                                {
                                    newHigherSecExmDtlObj.GPA = Decimal.Parse(txtHigherSec_CgpaScore.Text.Trim());
                                }
                                else
                                {
                                    newHigherSecExmDtlObj.GPA = null;
                                }

                                if (!string.IsNullOrEmpty(txtHigherSec_GpaW4S.Text.Trim()))
                                {
                                    newHigherSecExmDtlObj.GPAW4S = Decimal.Parse(txtHigherSec_GpaW4S.Text.Trim());
                                }
                                else
                                {
                                    newHigherSecExmDtlObj.GPAW4S = null;
                                }

                                if (!string.IsNullOrEmpty(txtHigherSec_Marks.Text.Trim()))
                                {
                                    newHigherSecExmDtlObj.Marks = Decimal.Parse(txtHigherSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    newHigherSecExmDtlObj.Marks = null;
                                }
                                //send empty string here
                                if (!string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text.Trim()))
                                {
                                    newHigherSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofHigherSec_Marks.Text.Trim());
                                }
                                else
                                {
                                    newHigherSecExmDtlObj.AttributeDec1 = null;
                                }

                                newHigherSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);
                                newHigherSecExmDtlObj.AttributeInt2 = Int32.Parse(ddlhsc_StudentCat.SelectedValue);

                                newHigherSecExmDtlObj.DateCreated = DateTime.Now;
                                newHigherSecExmDtlObj.CreatedBy = cId;

                                long newHighSecExmDtlObjID = -1;
                                using (var dbInsertHighSecExamDetails = new CandidateDataManager())
                                {
                                    dbInsertHighSecExamDetails.Insert<DAL.ExamDetail>(newHigherSecExmDtlObj);
                                    newHighSecExmDtlObjID = newHigherSecExmDtlObj.ID;
                                }
                                if (newHighSecExmDtlObjID > 0)
                                {
                                    highSecondaryExam.ExamDetailsID = newHighSecExmDtlObjID;

                                    using (var dbUpdateHighSecExam = new CandidateDataManager())
                                    {
                                        dbUpdateHighSecExam.Update<DAL.Exam>(highSecondaryExam);
                                    }
                                }

                                logNewObject = string.Empty;
                                logNewObject = GenerateLogStringFromObject(highSecondaryExam, newHigherSecExmDtlObj);


                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //dLog.DateCreated = DateTime.Now;
                                    //dLog.UserId = uId;
                                    //dLog.CandidateId = cId;
                                    //dLog.EventName = "Education Info Update (HSC) (Admin)";
                                    //dLog.PageName = "CandApplicationEducation.aspx";
                                    //dLog.NewData = logNewObject;
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    //LogWriter.DataLogWriter(dLog);

                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "Education Info Update (HSC) (Candidate)";
                                    dLog.PageName = "ApplicationEducation.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = logNewObject;
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //try
                                //{
                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.EventName = "Education Info Update (HSC) (Candidate)";
                                //    dLog.PageName = "ApplicationEducation.aspx";
                                //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = "Success";
                                //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion
                            }
                        }
                    }
                    else //higher secondary exam does not exist. Create new exam detail and then exam.
                    {
                        DAL.ExamDetail newHighSecExmDtlObj = new DAL.ExamDetail();

                        newHighSecExmDtlObj.EducationBoardID = Int32.Parse(ddlHigherSec_EducationBrd.SelectedValue);
                        newHighSecExmDtlObj.Institute = txtHigherSec_Institute.Text.Trim();
                        newHighSecExmDtlObj.UndgradGradProgID = 1;
                        newHighSecExmDtlObj.RollNo = txtHigherSec_RollNo.Text.Trim();
                        newHighSecExmDtlObj.RegistrationNo = txtHigherSec_RegNo.Text.Trim();
                        newHighSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlHigherSec_GrpOrSub.SelectedValue);

                        //if (ddlHigherSec_DivClass.Enabled == true)
                        //{
                        newHighSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                        //}
                        //else
                        //{
                        //    newHighSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                        //}

                        //newHighSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                        if (ddlHigherSec_DivClass.SelectedItem.Text == "GPA")
                        {
                            newHighSecExmDtlObj.GPA = Decimal.Parse(txtHigherSec_CgpaScore.Text.Trim());
                        }
                        else
                        {
                            newHighSecExmDtlObj.GPA = null;
                        }

                        if (!string.IsNullOrEmpty(txtHigherSec_GpaW4S.Text.Trim()))
                        {
                            newHighSecExmDtlObj.GPAW4S = Decimal.Parse(txtHigherSec_GpaW4S.Text.Trim());
                        }
                        else
                        {
                            newHighSecExmDtlObj.GPAW4S = null;
                        }

                        if (!string.IsNullOrEmpty(txtHigherSec_Marks.Text.Trim()))
                        {
                            newHighSecExmDtlObj.Marks = Decimal.Parse(txtHigherSec_Marks.Text.Trim());
                        }
                        else
                        {
                            newHighSecExmDtlObj.Marks = null;
                        }
                        //send empty string here
                        if (!string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text.Trim()))
                        {
                            newHighSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofHigherSec_Marks.Text.Trim());
                        }
                        else
                        {
                            newHighSecExmDtlObj.AttributeDec1 = null;
                        }

                        newHighSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);
                        newHighSecExmDtlObj.AttributeInt2 = Int32.Parse(ddlhsc_StudentCat.SelectedValue);

                        newHighSecExmDtlObj.DateCreated = DateTime.Now;
                        newHighSecExmDtlObj.CreatedBy = cId;

                        DAL.Exam newHighSecExamObj = new DAL.Exam();

                        newHighSecExamObj.CandidateID = cId;
                        newHighSecExamObj.ExamTypeID = Int32.Parse(ddlHigherSec_ExamType.SelectedValue);
                        newHighSecExamObj.CreatedBy = cId;
                        newHighSecExamObj.DateCreated = DateTime.Now;

                        using (var dbInsertHighSecExamDtl = new CandidateDataManager())
                        {
                            dbInsertHighSecExamDtl.Insert<DAL.ExamDetail>(newHighSecExmDtlObj);
                            newHighSecExamObj.ExamDetailsID = newHighSecExmDtlObj.ID;
                        }
                        using (var dbInsertHighSecExam = new CandidateDataManager())
                        {
                            dbInsertHighSecExam.Insert<DAL.Exam>(newHighSecExamObj);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(newHighSecExamObj, newHighSecExmDtlObj);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Education Info Insert (HSC) (Admin)";
                            //dLog.PageName = "CandApplicationEducation.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Education Information.";
                            //dLog.UserId = uId;
                            //dLog.Attribute1 = "Success";
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                            //LogWriter.DataLogWriter(dLog);

                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.CandidateId = cId;
                            dLog.EventName = "Education Info Insert (HSC) (Candidate)";
                            dLog.PageName = "ApplicationEducation.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = logNewObject;
                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion

                        //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        //try
                        //{
                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        //    dLog.DateCreated = DateTime.Now;
                        //    dLog.EventName = "Education Info Insert (HSC) (Candidate)";
                        //    dLog.PageName = "ApplicationEducation.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Education Information.";
                        //    dLog.UserId = uId;
                        //    dLog.Attribute1 = "Success";
                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                        //    LogWriter.DataLogWriter(dLog);
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //#endregion
                    }

                    #endregion HSC/A-Level/Diploma


                    List<DAL.CandidateFormSl> candidateFormSlList = null;
                    DAL.AdmissionSetup admSetup = null;

                    using (var db = new CandidateDataManager())
                    {
                        candidateFormSlList = db.GetAllCandidateFormSlByCandID_AD(cId);
                        if (candidateFormSlList != null)
                        {
                            //get only admSetup for masters.
                            admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6).Select(c => c.AdmissionSetup).FirstOrDefault();
                        }
                    }

                    if (admSetup != null)
                    {
                        if (admSetup.EducationCategoryID == 6) //applying for masters...hence undergrad info is saved/updated.
                        {
                            #region UNDERGRAD ================================================

                            //check whether candidate is providing any details. if not dont save.
                            if (!string.IsNullOrEmpty(txtUndergrad_Institute.Text.Trim()) ||
                                !string.IsNullOrEmpty(txtUndergrad_CgpaScore.Text.Trim()) ||
                                Int32.Parse(ddlUndergrad_PassingYear.SelectedValue) > 0)
                            {

                                if (undergradExam != null) //undergrad exam exist.
                                {
                                    DAL.ExamDetail undergradExmDtlObj = undergradExam.ExamDetail;

                                    if (undergradExmDtlObj != null) // undergrad exam detail exist. Update.
                                    {

                                        logOldObject = string.Empty;
                                        logOldObject = GenerateLogStringFromObject(undergradExam, undergradExmDtlObj);


                                        undergradExmDtlObj.EducationBoardID = 1; //N/A
                                        undergradExmDtlObj.Institute = txtUndergrad_Institute.Text.Trim();
                                        undergradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlUndergrad_ProgramDegree.SelectedValue);
                                        if (ddlUndergrad_ProgramDegree.SelectedItem.Text == "OTHERS")
                                        {
                                            undergradExmDtlObj.OtherProgram = txtUndergrad_ProgOthers.Text.Trim();
                                        }
                                        else
                                        {
                                            undergradExmDtlObj.OtherProgram = null;
                                        }
                                        //undergradExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlUndergrad_GrpOrSub.SelectedValue);
                                        undergradExmDtlObj.GroupOrSubjectID = 1; //n/a
                                        undergradExmDtlObj.ResultDivisionID = Int32.Parse(ddlUndergrad_DivClass.SelectedValue);
                                        if (ddlUndergrad_DivClass.SelectedItem.Text == "GPA")
                                        {
                                            undergradExmDtlObj.CGPA = Decimal.Parse(txtUndergrad_CgpaScore.Text.Trim());
                                        }
                                        else
                                        {
                                            undergradExmDtlObj.CGPA = null;
                                        }
                                        undergradExmDtlObj.PassingYear = Int32.Parse(ddlUndergrad_PassingYear.SelectedValue);

                                        undergradExmDtlObj.DateModified = DateTime.Now;
                                        undergradExmDtlObj.ModifiedBy = cId;

                                        using (var dbUpdateUndergradExamDetails = new CandidateDataManager())
                                        {
                                            dbUpdateUndergradExamDetails.Update<DAL.ExamDetail>(undergradExmDtlObj);
                                        }

                                        logNewObject = string.Empty;
                                        logNewObject = GenerateLogStringFromObject(undergradExam, undergradExmDtlObj);

                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            //dLog.DateCreated = DateTime.Now;
                                            //dLog.EventName = "Education Info Update (Undergrade) (Admin)";
                                            //dLog.PageName = "CandApplicationEducation.aspx";
                                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                            //dLog.UserId = uId;
                                            //dLog.Attribute1 = "Success";
                                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            //LogWriter.DataLogWriter(dLog);

                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = uId;
                                            dLog.CandidateId = cId;
                                            dLog.EventName = "Education Info Update (Undergrade) (Candidate)";
                                            dLog.PageName = "ApplicationEducation.aspx";
                                            dLog.OldData = logOldObject;
                                            dLog.NewData = logNewObject;
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        //try
                                        //{
                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //    dLog.DateCreated = DateTime.Now;
                                        //    dLog.EventName = "Education Info Update (Undergrade) (Candidate)";
                                        //    dLog.PageName = "ApplicationEducation.aspx";
                                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                        //    dLog.UserId = uId;
                                        //    dLog.Attribute1 = "Success";
                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //    LogWriter.DataLogWriter(dLog);
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //}
                                        //#endregion
                                    }
                                    else //undergrad exam detail does not exist. create new exam details.
                                    {
                                        DAL.ExamDetail newUndergradExmDtlObj = new DAL.ExamDetail();

                                        newUndergradExmDtlObj.EducationBoardID = 1; //N/A
                                        newUndergradExmDtlObj.Institute = txtUndergrad_Institute.Text.Trim();
                                        newUndergradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlUndergrad_ProgramDegree.SelectedValue);
                                        if (ddlUndergrad_ProgramDegree.SelectedItem.Text == "OTHERS")
                                        {
                                            newUndergradExmDtlObj.OtherProgram = txtUndergrad_ProgOthers.Text.Trim();
                                        }
                                        else
                                        {
                                            newUndergradExmDtlObj.OtherProgram = null;
                                        }
                                        //newUndergradExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlUndergrad_GrpOrSub.SelectedValue);
                                        newUndergradExmDtlObj.GroupOrSubjectID = 1; // n/a
                                        newUndergradExmDtlObj.ResultDivisionID = Int32.Parse(ddlUndergrad_DivClass.SelectedValue);
                                        if (ddlUndergrad_DivClass.SelectedItem.Text == "GPA")
                                        {
                                            newUndergradExmDtlObj.CGPA = Decimal.Parse(txtUndergrad_CgpaScore.Text.Trim());
                                        }
                                        else
                                        {
                                            newUndergradExmDtlObj.CGPA = null;
                                        }
                                        newUndergradExmDtlObj.PassingYear = Int32.Parse(ddlUndergrad_PassingYear.SelectedValue);

                                        newUndergradExmDtlObj.DateCreated = DateTime.Now;
                                        newUndergradExmDtlObj.CreatedBy = cId;

                                        long newUndergradExmDtlObjID = -1;
                                        using (var dbInsertUndergradExamDetails = new CandidateDataManager())
                                        {
                                            dbInsertUndergradExamDetails.Insert<DAL.ExamDetail>(newUndergradExmDtlObj);
                                            newUndergradExmDtlObjID = newUndergradExmDtlObj.ID;
                                        }
                                        if (newUndergradExmDtlObjID > 0)
                                        {
                                            undergradExam.ExamDetailsID = newUndergradExmDtlObjID;

                                            using (var dbUpdateUndergradExam = new CandidateDataManager())
                                            {
                                                dbUpdateUndergradExam.Update<DAL.Exam>(undergradExam);
                                            }
                                        }

                                        logNewObject = string.Empty;
                                        logNewObject = GenerateLogStringFromObject(undergradExam, newUndergradExmDtlObj);

                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            //dLog.DateCreated = DateTime.Now;
                                            //dLog.EventName = "Education Info Update (Undergrade) (Admin)";
                                            //dLog.PageName = "CandApplicationEducation.aspx";
                                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                            //dLog.UserId = uId;
                                            //dLog.Attribute1 = "Success";
                                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            //LogWriter.DataLogWriter(dLog);

                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = uId;
                                            dLog.CandidateId = cId;
                                            dLog.EventName = "Education Info Update (Undergrade) (Candidate)";
                                            dLog.PageName = "ApplicationEducation.aspx";
                                            //dLog.OldData = logOldObject;
                                            dLog.NewData = logNewObject;
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        //try
                                        //{
                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //    dLog.DateCreated = DateTime.Now;
                                        //    dLog.EventName = "Education Info Update (Undergrade) (Candidate)";
                                        //    dLog.PageName = "ApplicationEducation.aspx";
                                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                        //    dLog.UserId = uId;
                                        //    dLog.Attribute1 = "Success";
                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //    LogWriter.DataLogWriter(dLog);
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //}
                                        //#endregion
                                    }
                                }
                                else //undergrad exam does not exist. Create new exam detail and then exam.
                                {
                                    DAL.ExamDetail newUndergradExmDtlObj = new DAL.ExamDetail();

                                    newUndergradExmDtlObj.EducationBoardID = 1; //N/A
                                    newUndergradExmDtlObj.Institute = txtUndergrad_Institute.Text.Trim();
                                    newUndergradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlUndergrad_ProgramDegree.SelectedValue);
                                    if (ddlUndergrad_ProgramDegree.SelectedItem.Text == "OTHERS")
                                    {
                                        newUndergradExmDtlObj.OtherProgram = txtUndergrad_ProgOthers.Text.Trim();
                                    }
                                    else
                                    {
                                        newUndergradExmDtlObj.OtherProgram = null;
                                    }
                                    //newUndergradExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlUndergrad_GrpOrSub.SelectedValue);
                                    newUndergradExmDtlObj.GroupOrSubjectID = 1; //n/a
                                    newUndergradExmDtlObj.ResultDivisionID = Int32.Parse(ddlUndergrad_DivClass.SelectedValue);
                                    if (ddlUndergrad_DivClass.SelectedItem.Text == "GPA")
                                    {
                                        newUndergradExmDtlObj.CGPA = Decimal.Parse(txtUndergrad_CgpaScore.Text.Trim());
                                    }
                                    else
                                    {
                                        newUndergradExmDtlObj.CGPA = null;
                                    }
                                    newUndergradExmDtlObj.PassingYear = Int32.Parse(ddlUndergrad_PassingYear.SelectedValue);

                                    newUndergradExmDtlObj.DateCreated = DateTime.Now;
                                    newUndergradExmDtlObj.CreatedBy = cId;

                                    DAL.Exam newUndergradExamObj = new DAL.Exam();

                                    newUndergradExamObj.CandidateID = cId;
                                    newUndergradExamObj.ExamTypeID = 3;
                                    newUndergradExamObj.CreatedBy = cId;
                                    newUndergradExamObj.DateCreated = DateTime.Now;

                                    using (var dbInsertUndergradExamDtl = new CandidateDataManager())
                                    {
                                        dbInsertUndergradExamDtl.Insert<DAL.ExamDetail>(newUndergradExmDtlObj);
                                        newUndergradExamObj.ExamDetailsID = newUndergradExmDtlObj.ID;
                                    }
                                    using (var dbInsertUndergradExam = new CandidateDataManager())
                                    {
                                        dbInsertUndergradExam.Insert<DAL.Exam>(newUndergradExamObj);
                                    }

                                    logNewObject = string.Empty;
                                    logNewObject = GenerateLogStringFromObject(newUndergradExamObj, newUndergradExmDtlObj);

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Education Info Insert (Undergrade) (Admin)";
                                        //dLog.PageName = "CandApplicationEducation.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Education Information.";
                                        //dLog.UserId = uId;
                                        //dLog.Attribute1 = "Success";
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //LogWriter.DataLogWriter(dLog);

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Education Info Update (Undergrade) (Candidate)";
                                        dLog.PageName = "ApplicationEducation.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = logNewObject;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Education Info Insert (Undergrade) (Candidate)";
                                    //    dLog.PageName = "ApplicationEducation.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Education Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    ////#endregion
                                }
                            }

                            #endregion UNDERGRAD

                            #region GRAD =====================================================

                            //check whether candidate is providing any details. if not then dont save/update.
                            if (!string.IsNullOrEmpty(txtGraduate_Institute.Text.Trim()) ||
                                !string.IsNullOrEmpty(txtGraduate_CgpaScore.Text.Trim()) ||
                                Int32.Parse(ddlGraduate_PassingYear.SelectedValue) > 0)
                            {

                                if (gradExam != null) //grad exam exist.
                                {
                                    DAL.ExamDetail gradExmDtlObj = gradExam.ExamDetail;

                                    if (gradExmDtlObj != null) // grad exam detail exist. Update.
                                    {
                                        logOldObject = string.Empty;
                                        logOldObject = GenerateLogStringFromObject(gradExam, gradExmDtlObj);

                                        gradExmDtlObj.EducationBoardID = 1; //N/A
                                        gradExmDtlObj.Institute = txtGraduate_Institute.Text.Trim();
                                        gradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlGraduate_ProgramDegree.SelectedValue);
                                        if (ddlGraduate_ProgramDegree.SelectedItem.Text == "OTHERS")
                                        {
                                            gradExmDtlObj.OtherProgram = txtGraduate_ProgOthers.Text.Trim();
                                        }
                                        else
                                        {
                                            gradExmDtlObj.OtherProgram = null;
                                        }
                                        //gradExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlGraduate_GrpOrSub.SelectedValue);
                                        gradExmDtlObj.GroupOrSubjectID = 1;
                                        gradExmDtlObj.ResultDivisionID = Int32.Parse(ddlGraduate_DivClass.SelectedValue);
                                        if (ddlGraduate_DivClass.SelectedItem.Text == "GPA")
                                        {
                                            gradExmDtlObj.CGPA = Decimal.Parse(txtGraduate_CgpaScore.Text.Trim());
                                        }
                                        else
                                        {
                                            gradExmDtlObj.CGPA = null;
                                        }
                                        gradExmDtlObj.PassingYear = Int32.Parse(ddlGraduate_PassingYear.SelectedValue);

                                        gradExmDtlObj.DateModified = DateTime.Now;
                                        gradExmDtlObj.ModifiedBy = cId;

                                        using (var dbUpdateGradExamDetails = new CandidateDataManager())
                                        {
                                            dbUpdateGradExamDetails.Update<DAL.ExamDetail>(gradExmDtlObj);
                                        }

                                        logNewObject = string.Empty;
                                        logNewObject = GenerateLogStringFromObject(gradExam, gradExmDtlObj);


                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            //dLog.DateCreated = DateTime.Now;
                                            //dLog.EventName = "Education Info Update (Grade) (Admin)";
                                            //dLog.PageName = "CandApplicationEducation.aspx";
                                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                            //dLog.UserId = uId;
                                            //dLog.Attribute1 = "Success";
                                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            //LogWriter.DataLogWriter(dLog);

                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = uId;
                                            dLog.CandidateId = cId;
                                            dLog.EventName = "Education Info Update (Grade) (Candidate)";
                                            dLog.PageName = "ApplicationEducation.aspx";
                                            dLog.OldData = logOldObject;
                                            dLog.NewData = logNewObject;
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);

                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        //try
                                        //{
                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //    dLog.DateCreated = DateTime.Now;
                                        //    dLog.EventName = "Education Info Update (Grade) (Candidate)";
                                        //    dLog.PageName = "ApplicationEducation.aspx";
                                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                        //    dLog.UserId = uId;
                                        //    dLog.Attribute1 = "Success";
                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //    LogWriter.DataLogWriter(dLog);
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //}
                                        ////#endregion
                                    }
                                    else //grad exam detail does not exist. create new exam details.
                                    {
                                        DAL.ExamDetail newGradExmDtlObj = new DAL.ExamDetail();

                                        newGradExmDtlObj.EducationBoardID = 1; //N/A
                                        newGradExmDtlObj.Institute = txtGraduate_Institute.Text.Trim();
                                        newGradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlGraduate_ProgramDegree.SelectedValue);
                                        if (ddlGraduate_ProgramDegree.SelectedItem.Text == "OTHERS")
                                        {
                                            newGradExmDtlObj.OtherProgram = txtGraduate_ProgOthers.Text.Trim();
                                        }
                                        else
                                        {
                                            newGradExmDtlObj.OtherProgram = null;
                                        }
                                        //newGradExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlGraduate_GrpOrSub.SelectedValue);
                                        newGradExmDtlObj.GroupOrSubjectID = 1;
                                        newGradExmDtlObj.ResultDivisionID = Int32.Parse(ddlGraduate_DivClass.SelectedValue);
                                        if (ddlGraduate_DivClass.SelectedItem.Text == "GPA")
                                        {
                                            newGradExmDtlObj.CGPA = Decimal.Parse(txtGraduate_CgpaScore.Text.Trim());
                                        }
                                        else
                                        {
                                            newGradExmDtlObj.CGPA = null;
                                        }
                                        newGradExmDtlObj.PassingYear = Int32.Parse(ddlGraduate_PassingYear.SelectedValue);

                                        newGradExmDtlObj.DateCreated = DateTime.Now;
                                        newGradExmDtlObj.CreatedBy = cId;

                                        long newGradExmDtlObjID = -1;
                                        using (var dbInsertGradExamDetails = new CandidateDataManager())
                                        {
                                            dbInsertGradExamDetails.Insert<DAL.ExamDetail>(newGradExmDtlObj);
                                            newGradExmDtlObjID = newGradExmDtlObj.ID;
                                        }
                                        if (newGradExmDtlObjID > 0)
                                        {
                                            gradExam.ExamDetailsID = newGradExmDtlObjID;

                                            using (var dbUpdateGradExam = new CandidateDataManager())
                                            {
                                                dbUpdateGradExam.Update<DAL.Exam>(gradExam);
                                            }
                                        }

                                        logNewObject = string.Empty;
                                        logNewObject = GenerateLogStringFromObject(gradExam, newGradExmDtlObj);

                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            //dLog.DateCreated = DateTime.Now;
                                            //dLog.EventName = "Education Info Update (Grade) (Admin)";
                                            //dLog.PageName = "CandApplicationEducation.aspx";
                                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                            //dLog.UserId = uId;
                                            //dLog.Attribute1 = "Success";
                                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            //LogWriter.DataLogWriter(dLog);

                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = uId;
                                            dLog.CandidateId = cId;
                                            dLog.EventName = "Education Info Update (Grade) (Candidate)";
                                            dLog.PageName = "ApplicationEducation.aspx";
                                            //dLog.OldData = logOldObject;
                                            dLog.NewData = logNewObject;
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        //try
                                        //{
                                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //    dLog.DateCreated = DateTime.Now;
                                        //    dLog.EventName = "Education Info Update (Grade) (Candidate)";
                                        //    dLog.PageName = "ApplicationEducation.aspx";
                                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
                                        //    dLog.UserId = uId;
                                        //    dLog.Attribute1 = "Success";
                                        //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //    LogWriter.DataLogWriter(dLog);
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //}
                                        //#endregion
                                    }
                                }
                                else //undergrad exam does not exist. Create new exam detail and then exam.
                                {
                                    DAL.ExamDetail newGradExmDtlObj = new DAL.ExamDetail();

                                    newGradExmDtlObj.EducationBoardID = 1; //N/A
                                    newGradExmDtlObj.Institute = txtGraduate_Institute.Text.Trim();
                                    newGradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlGraduate_ProgramDegree.SelectedValue);
                                    if (ddlGraduate_ProgramDegree.SelectedItem.Text == "OTHERS")
                                    {
                                        newGradExmDtlObj.OtherProgram = txtGraduate_ProgOthers.Text.Trim();
                                    }
                                    else
                                    {
                                        newGradExmDtlObj.OtherProgram = null;
                                    }
                                    //newGradExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlGraduate_GrpOrSub.SelectedValue);
                                    newGradExmDtlObj.GroupOrSubjectID = 1;
                                    newGradExmDtlObj.ResultDivisionID = Int32.Parse(ddlGraduate_DivClass.SelectedValue);
                                    if (ddlGraduate_DivClass.SelectedItem.Text == "GPA")
                                    {
                                        newGradExmDtlObj.CGPA = Decimal.Parse(txtGraduate_CgpaScore.Text.Trim());
                                    }
                                    else
                                    {
                                        newGradExmDtlObj.CGPA = null;
                                    }
                                    newGradExmDtlObj.PassingYear = Int32.Parse(ddlGraduate_PassingYear.SelectedValue);

                                    newGradExmDtlObj.DateCreated = DateTime.Now;
                                    newGradExmDtlObj.CreatedBy = cId;

                                    DAL.Exam newGradExamObj = new DAL.Exam();

                                    newGradExamObj.CandidateID = cId;
                                    newGradExamObj.ExamTypeID = 4;
                                    newGradExamObj.CreatedBy = cId;
                                    newGradExamObj.DateCreated = DateTime.Now;

                                    using (var dbInsertHighSecExamDtl = new CandidateDataManager())
                                    {
                                        dbInsertHighSecExamDtl.Insert<DAL.ExamDetail>(newGradExmDtlObj);
                                        newGradExamObj.ExamDetailsID = newGradExmDtlObj.ID;
                                    }
                                    using (var dbInsertHighSecExam = new CandidateDataManager())
                                    {
                                        dbInsertHighSecExam.Insert<DAL.Exam>(newGradExamObj);
                                    }

                                    logNewObject = string.Empty;
                                    logNewObject = GenerateLogStringFromObject(newGradExamObj, newGradExmDtlObj);

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Education Info Insert (Grade) (Admin)";
                                        //dLog.PageName = "CandApplicationEducation.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Education Information.";
                                        //dLog.UserId = uId;
                                        //dLog.Attribute1 = "Success";
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //LogWriter.DataLogWriter(dLog);


                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Education Info Update (Grade) (Candidate)";
                                        dLog.PageName = "ApplicationEducation.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = logNewObject;
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Education Info Insert (Grade) (Candidate)";
                                    //    dLog.PageName = "ApplicationEducation.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Education Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion
                                }

                            }

                            #endregion GRAD
                        }
                    }




                }


                lblMessageEducation.Text = "Education Info Updated successfully.";
                messagePanel_Education.CssClass = "alert alert-success";
                messagePanel_Education.Visible = true;

                Response.Redirect("ApplicationPriorityS.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessageEducation.Text = "Unable to save/update candidate information.";
                messagePanel_Education.CssClass = "alert alert-danger";
                messagePanel_Education.Visible = true;
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            btnSave_Education_Click(sender, e);
            Response.Redirect("ApplicationPriorityS.aspx", false);
        }

        protected void ddlSec_DivClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSec_DivClass.SelectedValue == "5")// if gpa
            {
                txtSec_CgpaScore.Text = string.Empty;
                txtSec_CgpaScore.Enabled = true;
                //txtSec_CgpaScore.IsValid = false;
                //txtSec_CgpaScore.Enabled = true;
            }
            else
            {
                txtSec_CgpaScore.Text = string.Empty;
                //txtSec_CgpaScore.Enabled = false;
                //txtSecCgpaRFV.IsValid = true;
                //txtSecCgpaRFV.Enabled = false;
            }
        }

        protected void ddlHigherSec_DivClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHigherSec_DivClass.SelectedValue == "5")// if gpa
            {
                txtHigherSec_CgpaScore.Text = string.Empty;
                txtHigherSec_CgpaScore.Enabled = true;
                //txtHigSecCgpaRFV.IsValid = false;
                //txtHigSecCgpaRFV.Enabled = true;
            }
            else
            {
                txtHigherSec_CgpaScore.Text = string.Empty;
                //txtHigherSec_CgpaScore.Enabled = false;
                //txtHigSecCgpaRFV.IsValid = true;
                //txtHigSecCgpaRFV.Enabled = false;
            }
        }

        protected void ddlUndergrad_DivClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUndergrad_DivClass.SelectedValue == "5")
            {
                txtUndergrad_CgpaScore.Enabled = true;
                txtUndergrad_CgpaScore.Text = string.Empty;
                //txtUndergrad_CgpaScore_ReqV.Enabled = true;
            }
            else
            {
                //txtUndergrad_CgpaScore.Enabled = false;
                txtUndergrad_CgpaScore.Text = string.Empty;
                //txtUndergrad_CgpaScore_ReqV.Enabled = false;
            }
        }

        protected void ddlGraduate_DivClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGraduate_DivClass.SelectedValue == "5")
            {
                txtGraduate_CgpaScore.Enabled = true;
                txtGraduate_CgpaScore.Text = string.Empty;
            }
            else
            {
                //txtGraduate_CgpaScore.Enabled = false;
                txtGraduate_CgpaScore.Text = string.Empty;
            }
        }

        protected void ddlUndergrad_ProgramDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUndergrad_ProgramDegree.SelectedItem.Text == "OTHERS")
            {
                txtUndergrad_ProgOthers.Enabled = true;
                txtUndergrad_ProgOthers.Text = string.Empty;
                //txtUndergrad_ProgOthers_ReqV.Enabled = true;
            }
            else
            {
                //txtUndergrad_ProgOthers.Enabled = false;
                txtUndergrad_ProgOthers.Text = string.Empty;
                //txtUndergrad_ProgOthers_ReqV.Enabled = false;
            }
        }

        protected void ddlGraduate_ProgramDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGraduate_ProgramDegree.SelectedItem.Text == "OTHERS")
            {
                txtGraduate_ProgOthers.Enabled = true;
                txtGraduate_ProgOthers.Text = string.Empty;
                //txtUndergrad_ProgramOtherRFV.Enabled = true;
            }
            else
            {
                //txtGraduate_ProgOthers.Enabled = false;
                txtGraduate_ProgOthers.Text = string.Empty;
                //txtUndergrad_ProgramOtherRFV.Enabled = false;
            }
        }



        protected string GenerateLogStringFromObject(DAL.Exam objExam, DAL.ExamDetail objExamDetails)
        {

            string result = "";

            try
            {
                #region ExamType
                if (objExam != null && Convert.ToInt32(objExam.ExamTypeID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "ExamType: " + (db.AdmissionDB.ExamTypes.Where(a => a.ID == objExam.ExamTypeID).Select(x => x.ExamTypeName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "ExamType: ; ";
                }
                #endregion

                #region EducationBoard
                if (objExamDetails.EducationBoardID != null && Convert.ToInt32(objExamDetails.EducationBoardID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "EducationBoard: " + (db.AdmissionDB.EducationBoards.Where(a => a.ID == objExamDetails.EducationBoardID).Select(x => x.BoardName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "EducationBoard: ; ";
                }
                #endregion

                result += "Institute: " + (!string.IsNullOrEmpty(objExamDetails.Institute) ? objExamDetails.Institute.ToString() : "") + "; ";

                #region UndgradGradProg
                if (objExamDetails.UndgradGradProgID != null && Convert.ToInt32(objExamDetails.UndgradGradProgID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "UndgradGradProgram: " + (db.AdmissionDB.UndergradGradPrograms.Where(a => a.ID == objExamDetails.UndgradGradProgID).Select(x => x.ProgramName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "UndgradGradProgram: ; ";
                }
                #endregion

                result += "RollNo: " + (!string.IsNullOrEmpty(objExamDetails.RollNo) ? objExamDetails.RollNo.ToString() : "") + "; ";
                result += "RegistrationNo: " + (!string.IsNullOrEmpty(objExamDetails.RegistrationNo) ? objExamDetails.RegistrationNo.ToString() : "") + "; ";

                #region GroupOrSubject
                if (objExamDetails.GroupOrSubjectID != null && Convert.ToInt32(objExamDetails.GroupOrSubjectID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "GroupOrSubject: " + (db.AdmissionDB.GroupOrSubjects.Where(a => a.ID == objExamDetails.GroupOrSubjectID).Select(x => x.GroupOrSubjectName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "GroupOrSubject: ; ";
                }
                #endregion

                #region ResultDivision
                if (objExamDetails.ResultDivisionID != null && Convert.ToInt32(objExamDetails.ResultDivisionID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "ResultDivision: " + (db.AdmissionDB.ResultDivisions.Where(a => a.ID == objExamDetails.ResultDivisionID).Select(x => x.ResultDivisionName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "ResultDivision: ; ";
                }
                #endregion

                #region GPA
                if (objExamDetails.GPA != null && Convert.ToInt32(objExamDetails.GPA) > 0)
                {
                    result += "GPA: " + objExamDetails.GPA.ToString() + "; ";
                }
                else
                {
                    result += "GPA: ; ";
                }
                #endregion

                #region GPAW4S
                if (objExamDetails.GPAW4S != null && Convert.ToInt32(objExamDetails.GPAW4S) > 0)
                {
                    result += "GPAW4S: " + objExamDetails.GPAW4S.ToString() + "; ";
                }
                else
                {
                    result += "GPAW4S: ; ";
                }
                #endregion

                #region CGPA
                if (objExamDetails.CGPA != null && Convert.ToInt32(objExamDetails.CGPA) > 0)
                {
                    result += "CGPA: " + objExamDetails.CGPA.ToString() + "; ";
                }
                else
                {
                    result += "CGPA: ; ";
                }
                #endregion

                #region Marks
                if (objExamDetails.Marks != null && Convert.ToInt32(objExamDetails.Marks) > 0)
                {
                    result += "Marks: " + objExamDetails.Marks.ToString() + "; ";
                }
                else
                {
                    result += "Marks: ; ";
                }
                #endregion

                #region Marks
                if (objExamDetails.PassingYear != null && Convert.ToInt32(objExamDetails.PassingYear) > 0)
                {
                    result += "PassingYear: " + objExamDetails.PassingYear.ToString() + "; ";
                }
                else
                {
                    result += "PassingYear: ; ";
                }
                #endregion

                result += "OtherProgram: " + (!string.IsNullOrEmpty(objExamDetails.OtherProgram) ? objExamDetails.OtherProgram.ToString() : "") + "; ";

            }
            catch (Exception ex)
            {
                result += "Exception: " + ex.Message.ToString() + "; ";

            }



            return result;
        }

        //protected void txtOutofHigherSec_Marks_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int ExamTypeId = Convert.ToInt32(ddlHigherSec_ExamType.SelectedValue);
        //        int BoardId = Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);
        //        int PassingYearId = Convert.ToInt32(ddlHigherSec_PassingYear.SelectedValue);
        //        int GroupId = Convert.ToInt32(ddlHigherSec_GrpOrSub.SelectedValue);
        //        if (BoardId == 13) // Only A Level Cambridge
        //        {
        //            if (!string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text))
        //            {
        //                try
        //                {

        //                    decimal GPA = 0;
        //                    if (!string.IsNullOrEmpty(txtHigherSec_CgpaScore.Text))
        //                    {
        //                        GPA = Convert.ToDecimal(txtHigherSec_CgpaScore.Text);
        //                        using (var db = new CandidateDataManager())
        //                        {
        //                            var SetupObj = db.AdmissionDB.ExamTypeAndGpaRangeWisePercentageInformations.Where(x => x.ExamTypeID == ExamTypeId
        //                                       && x.EducationBoardID == BoardId && x.Year == PassingYearId && (x.GroupOrSubjectID == GroupId || x.GroupOrSubjectID == null)
        //                                       && GPA >= x.MinimumGPA && GPA <= x.MaximumGPA).FirstOrDefault();

        //                            if (SetupObj != null)
        //                            {
        //                                decimal Percentage = Convert.ToDecimal(SetupObj.Percentage);

        //                                string TotalMark = ((Percentage / 100) * Convert.ToDecimal(txtOutofHigherSec_Marks.Text)).ToString();

        //                                txtHigherSec_Marks.Text = TotalMark;
        //                            }

        //                        }
        //                    }
        //                    else
        //                    {
        //                        lblMessageEducation.Text = "Unable to calculate total mark.GPA needed.";
        //                        messagePanel_Education.CssClass = "alert alert-danger";
        //                        messagePanel_Education.Visible = true;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                }

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //protected void txtOutofSec_Marks_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int ExamTypeId = Convert.ToInt32(ddlSec_ExamType.SelectedValue);
        //        int BoardId = Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);
        //        int PassingYearId = Convert.ToInt32(ddlSec_PassingYear.SelectedValue);
        //        int GroupId = Convert.ToInt32(ddlSec_GrpOrSub.SelectedValue);
        //        if (BoardId == 13) // Only O Level Cambridge
        //        {
        //            if (!string.IsNullOrEmpty(txtOutofSec_Marks.Text))
        //            {
        //                try
        //                {

        //                    decimal GPA = 0;
        //                    if (!string.IsNullOrEmpty(txtSec_CgpaScore.Text))
        //                    {
        //                        GPA = Convert.ToDecimal(txtSec_CgpaScore.Text);
        //                        using (var db = new CandidateDataManager())
        //                        {
        //                            var SetupObj = db.AdmissionDB.ExamTypeAndGpaRangeWisePercentageInformations.Where(x => x.ExamTypeID == ExamTypeId
        //                                       && x.EducationBoardID == BoardId && x.Year == PassingYearId && (x.GroupOrSubjectID == GroupId || x.GroupOrSubjectID == null)
        //                                       && GPA >= x.MinimumGPA && GPA <= x.MaximumGPA).FirstOrDefault();

        //                            if (SetupObj != null)
        //                            {
        //                                decimal Percentage = Convert.ToDecimal(SetupObj.Percentage);

        //                                string TotalMark = ((Percentage / 100) * Convert.ToDecimal(txtOutofSec_Marks.Text)).ToString();

        //                                txtSec_Marks.Text = TotalMark;
        //                            }

        //                        }
        //                    }
        //                    else
        //                    {
        //                        lblMessageEducation.Text = "Unable to calculate total mark.GPA needed.";
        //                        messagePanel_Education.CssClass = "alert alert-danger";
        //                        messagePanel_Education.Visible = true;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                }

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}


        #region O Level Result Entry

        protected void lnkAddResult_Click(object sender, EventArgs e)
        {
            try
            {
                int boardId = Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);

                if (boardId == 13 || boardId == 12 || boardId == 11) // O Level Cambridge and Edexel
                {
                    ClearOLevelGrid();

                    CheckAndBindGridViewOfOLevel();

                    //modalPopupSubjectWiseResult.Show();
                }

            }
            catch (Exception exx)
            {
            }
        }

        private void CheckAndBindGridViewOfOLevel()
        {
            try
            {

                int ExamtypeId = Convert.ToInt32(ddlSec_ExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);

                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
                using (var db = new CandidateDataManager())
                {
                    var ExistingList = db.AdmissionDB.OandALevelCandidateSubjectWiseResults.Where(x => x.CandidateId == cId
                      && x.ExamTypeId == ExamtypeId && x.EducationBoardId == EducationBoard).ToList();
                    if (ExistingList != null && ExistingList.Any())
                    {
                        txtOlevelSubjetNo.Text = ExistingList.Count.ToString();

                        gvOLevelSubjectResult.DataSource = ExistingList;
                        gvOLevelSubjectResult.DataBind();
                        divCalculateOLevel.Visible = true;
                        GridRebindOLevel(ExistingList);

                        //modalPopupSubjectWiseResult.Show();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void GridRebindOLevel(List<OandALevelCandidateSubjectWiseResult> existingList)
        {
            try
            {
                for (int i = 0; i < gvOLevelSubjectResult.Rows.Count; i++)
                {
                    try
                    {
                        GridViewRow row = gvOLevelSubjectResult.Rows[i];
                        Label lblSubjectNo = (Label)row.FindControl("lblSubjectNo");

                        TextBox txtOLevelMark = (TextBox)row.FindControl("txtOLevelMark");
                        DropDownList ddlOLevelGrade = (DropDownList)row.FindControl("ddlOLevelGrade");

                        int SubjectNo = lblSubjectNo != null && lblSubjectNo.Text != "" ? Convert.ToInt32(lblSubjectNo.Text) : 0;

                        if (SubjectNo > 0)
                        {
                            var SingleObj = existingList.Where(x => x.SubjectNo == SubjectNo).FirstOrDefault();

                            txtOLevelMark.Text = SingleObj.ExamMark != null ? SingleObj.ExamMark.ToString() : "";

                            try
                            {
                                double TotalMark = 0;
                                TotalMark = Convert.ToDouble(SingleObj.ObtainedMark) / Convert.ToDouble(SingleObj.ExamMark) * 100.00;
                                ddlOLevelGrade.SelectedValue = TotalMark.ToString();
                            }
                            catch (Exception ex)
                            {
                            }

                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkGenerateOlevel_Click(object sender, EventArgs e)
        {
            try
            {
                int QuestionNo = Convert.ToInt32(txtOlevelSubjetNo.Text.Trim());

                if (QuestionNo > 0)
                {
                    if (QuestionNo < 5)
                    {
                        showAlert("You need to enter minimum 5 subject for O level");
                        return;
                    }
                    ClearOLevelGrid();
                    txtOlevelSubjetNo.Text = QuestionNo.ToString();
                    GenerateSubjectsGrid(QuestionNo);
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearOLevelGrid()
        {
            divCalculateOLevel.Visible = false;
            txtOlevelSubjetNo.Text = string.Empty;
            gvOLevelSubjectResult.DataSource = null;
            gvOLevelSubjectResult.DataBind();
        }

        private void GenerateSubjectsGrid(int questionNo)
        {
            try
            {
                int ExamtypeId = Convert.ToInt32(ddlSec_ExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);


                List<DAL.OandALevelCandidateSubjectWiseResult> subjectList = new List<DAL.OandALevelCandidateSubjectWiseResult>();

                for (int i = 1; i <= questionNo; i++)
                {
                    DAL.OandALevelCandidateSubjectWiseResult NewObj = new DAL.OandALevelCandidateSubjectWiseResult();

                    NewObj.ExamTypeId = ExamtypeId;
                    NewObj.EducationBoardId = EducationBoard;
                    NewObj.CandidateId = cId;
                    NewObj.SubjectNo = i;
                    NewObj.ObtainedGrade = "";

                    subjectList.Add(NewObj);

                }

                if (subjectList != null && subjectList.Any())
                {
                    //modalPopupSubjectWiseResult.Show();
                    gvOLevelSubjectResult.DataSource = subjectList;
                    gvOLevelSubjectResult.DataBind();
                    divCalculateOLevel.Visible = true;
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkOLevelSave_Click(object sender, EventArgs e)
        {
            try
            {
                int ExamtypeId = Convert.ToInt32(ddlSec_ExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);

                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }

                int EntryCount = GetgridViewGradeEntryCountOLevel(gvOLevelSubjectResult);

                if (EntryCount < 5)
                {
                    showAlert("You need to enter minimum 5 subject for O level");
                    return;
                }

                using (var db = new CandidateDataManager())
                {
                    var ExistingList = db.AdmissionDB.OandALevelCandidateSubjectWiseResults.Where(x => x.CandidateId == cId
                      && x.ExamTypeId == ExamtypeId && x.EducationBoardId == EducationBoard).ToList();
                    if (ExistingList != null && ExistingList.Any())
                    {
                        foreach (var item in ExistingList)
                        {
                            using (var dbdelete = new CandidateDataManager())
                            {
                                dbdelete.Delete<DAL.OandALevelCandidateSubjectWiseResult>(item);
                            }

                        }
                    }

                    decimal TotalMarks = 0, TotalTaken = 0;

                    for (int i = 0; i < gvOLevelSubjectResult.Rows.Count; i++)
                    {
                        try
                        {
                            GridViewRow row = gvOLevelSubjectResult.Rows[i];
                            Label lblSubjectNo = (Label)row.FindControl("lblSubjectNo");
                            TextBox txtOLevelMark = (TextBox)row.FindControl("txtOLevelMark");
                            DropDownList ddlOLevelGrade = (DropDownList)row.FindControl("ddlOLevelGrade");

                            int SubNo = lblSubjectNo != null && lblSubjectNo.Text != "" ? Convert.ToInt32(lblSubjectNo.Text) : 0;
                            decimal Marks = txtOLevelMark != null && txtOLevelMark.Text != "" ? Convert.ToDecimal(txtOLevelMark.Text) : 0;

                            decimal GradeValue = Convert.ToDecimal(ddlOLevelGrade.SelectedValue);
                            string Grade = ddlOLevelGrade.SelectedItem.ToString();

                            if (Marks > 0 && GradeValue > 0)
                            {
                                DAL.OandALevelCandidateSubjectWiseResult NewObj = new DAL.OandALevelCandidateSubjectWiseResult();

                                NewObj.ExamTypeId = ExamtypeId;
                                NewObj.EducationBoardId = EducationBoard;
                                NewObj.CandidateId = cId;
                                NewObj.SubjectNo = SubNo;
                                NewObj.ExamMark = Marks;
                                NewObj.ObtainedGrade = Grade;
                                NewObj.ObtainedMark = (GradeValue / 100) * Marks;
                                try
                                {
                                    NewObj.CreatedBy = Convert.ToInt32(cId);
                                }
                                catch (Exception ex)
                                {
                                    NewObj.CreatedBy = -12;
                                }

                                NewObj.CreatedDate = DateTime.Now;

                                using (var dbInsert = new CandidateDataManager())
                                {
                                    dbInsert.Insert<DAL.OandALevelCandidateSubjectWiseResult>(NewObj);
                                    if (NewObj.Id > 0)
                                    {
                                        TotalMarks = TotalMarks + Convert.ToDecimal(NewObj.ObtainedMark);
                                        TotalTaken = TotalTaken + Marks;
                                    }
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }

                    txtSec_Marks.Text = Math.Round(TotalMarks, 2).ToString();
                    txtOutofSec_Marks.Text = Math.Round(TotalTaken, 2).ToString();

                    //modalPopupSubjectWiseResult.Hide();

                    showAlert("Result calculated successfully");
                }


            }
            catch (Exception ex)
            {
            }
        }

        private int GetgridViewGradeEntryCountOLevel(GridView gvOLevelSubjectResult)
        {
            int count = 0;
            try
            {
                for (int i = 0; i < gvOLevelSubjectResult.Rows.Count; i++)
                {
                    GridViewRow row = gvOLevelSubjectResult.Rows[i];
                    TextBox txtOLevelMark = (TextBox)row.FindControl("txtOLevelMark");
                    DropDownList ddlOLevelGrade = (DropDownList)row.FindControl("ddlOLevelGrade");
                    if (txtOLevelMark != null && !string.IsNullOrEmpty(txtOLevelMark.Text)
                        && ddlOLevelGrade != null && ddlOLevelGrade.SelectedIndex > 0)
                    {
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return count;
        }

        #endregion

        #region A Level Result Entry

        protected void lnkAddALevelResult_Click(object sender, EventArgs e)
        {
            try
            {
                int boardId = Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);

                if (boardId == 13 || boardId == 12 || boardId == 11) // A Level Cambridge and Edexel
                {
                    ClearALevelGrid();

                    CheckAndBindGridViewOfALevel();

                    //modalPopupSubjectWiseResultALevel.Show();
                }

            }
            catch (Exception exx)
            {
            }
        }


        private void CheckAndBindGridViewOfALevel()
        {
            try
            {
                int ExamtypeId = Convert.ToInt32(ddlHigherSec_ExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);

                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
                using (var db = new CandidateDataManager())
                {
                    var ExistingList = db.AdmissionDB.OandALevelCandidateSubjectWiseResults.Where(x => x.CandidateId == cId
                      && x.ExamTypeId == ExamtypeId && x.EducationBoardId == EducationBoard).ToList();
                    if (ExistingList != null && ExistingList.Any())
                    {
                        txtAlevelSubjetNo.Text = ExistingList.Count.ToString();

                        gvALevelSubjectResult.DataSource = ExistingList;
                        gvALevelSubjectResult.DataBind();
                        divCalculateALevel.Visible = true;
                        GridRebindALevel(ExistingList);

                        //modalPopupSubjectWiseResultALevel.Show();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void GridRebindALevel(List<OandALevelCandidateSubjectWiseResult> existingList)
        {
            try
            {
                for (int i = 0; i < gvALevelSubjectResult.Rows.Count; i++)
                {
                    try
                    {
                        GridViewRow row = gvALevelSubjectResult.Rows[i];

                        Label lblASubjectNo = (Label)row.FindControl("lblASubjectNo");
                        TextBox txtALevelMark = (TextBox)row.FindControl("txtALevelMark");
                        DropDownList ddlALevelGrade = (DropDownList)row.FindControl("ddlALevelGrade");


                        int SubjectNo = lblASubjectNo != null && lblASubjectNo.Text != "" ? Convert.ToInt32(lblASubjectNo.Text) : 0;

                        if (SubjectNo > 0)
                        {
                            var SingleObj = existingList.Where(x => x.SubjectNo == SubjectNo).FirstOrDefault();

                            txtALevelMark.Text = SingleObj.ExamMark != null ? SingleObj.ExamMark.ToString() : "";
                            try
                            {
                                double TotalMark = 0;
                                TotalMark = Convert.ToDouble(SingleObj.ObtainedMark) / Convert.ToDouble(SingleObj.ExamMark) * 100.00;
                                ddlALevelGrade.SelectedValue = TotalMark.ToString();
                            }
                            catch (Exception ex)
                            {
                            }

                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void ClearALevelGrid()
        {
            divCalculateALevel.Visible = false;
            txtAlevelSubjetNo.Text = string.Empty;
            gvALevelSubjectResult.DataSource = null;
            gvALevelSubjectResult.DataBind();
        }

        protected void lnkGenerateAlevel_Click(object sender, EventArgs e)
        {
            try
            {
                int QuestionNo = Convert.ToInt32(txtAlevelSubjetNo.Text.Trim());

                if (QuestionNo > 0)
                {
                    if (QuestionNo < 2)
                    {
                        showAlert("You need to enter minimum 2 subject for A level");
                        return;
                    }
                    ClearALevelGrid();
                    txtAlevelSubjetNo.Text = QuestionNo.ToString();
                    GenerateALevelSubjectsGrid(QuestionNo);
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void GenerateALevelSubjectsGrid(int questionNo)
        {
            try
            {
                int ExamtypeId = Convert.ToInt32(ddlHigherSec_ExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);


                List<DAL.OandALevelCandidateSubjectWiseResult> subjectList = new List<DAL.OandALevelCandidateSubjectWiseResult>();

                for (int i = 1; i <= questionNo; i++)
                {
                    DAL.OandALevelCandidateSubjectWiseResult NewObj = new DAL.OandALevelCandidateSubjectWiseResult();

                    NewObj.ExamTypeId = ExamtypeId;
                    NewObj.EducationBoardId = EducationBoard;
                    NewObj.CandidateId = cId;
                    NewObj.SubjectNo = i;
                    NewObj.ObtainedGrade = "";

                    subjectList.Add(NewObj);

                }

                if (subjectList != null && subjectList.Any())
                {
                    //modalPopupSubjectWiseResultALevel.Show();
                    gvALevelSubjectResult.DataSource = subjectList;
                    gvALevelSubjectResult.DataBind();
                    divCalculateALevel.Visible = true;
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkALevelSave_Click(object sender, EventArgs e)
        {
            try
            {
                int ExamtypeId = Convert.ToInt32(ddlHigherSec_ExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);

                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }

                int EntryCount = GetgridViewGradeEntryCountALevel(gvALevelSubjectResult);
                if (EntryCount < 2)
                {
                    showAlert("You need to enter minimum 2 subject for A level");
                    return;
                }


                using (var db = new CandidateDataManager())
                {
                    var ExistingList = db.AdmissionDB.OandALevelCandidateSubjectWiseResults.Where(x => x.CandidateId == cId
                      && x.ExamTypeId == ExamtypeId && x.EducationBoardId == EducationBoard).ToList();
                    if (ExistingList != null && ExistingList.Any())
                    {
                        foreach (var item in ExistingList)
                        {
                            using (var dbdelete = new CandidateDataManager())
                            {
                                dbdelete.Delete<DAL.OandALevelCandidateSubjectWiseResult>(item);
                            }

                        }
                    }

                    decimal TotalMarks = 0, TotalTaken = 0;

                    for (int i = 0; i < gvALevelSubjectResult.Rows.Count; i++)
                    {
                        try
                        {
                            GridViewRow row = gvALevelSubjectResult.Rows[i];
                            Label lblASubjectNo = (Label)row.FindControl("lblASubjectNo");
                            TextBox txtALevelMark = (TextBox)row.FindControl("txtALevelMark");
                            DropDownList ddlALevelGrade = (DropDownList)row.FindControl("ddlALevelGrade");

                            int SubNo = lblASubjectNo != null && lblASubjectNo.Text != "" ? Convert.ToInt32(lblASubjectNo.Text) : 0;
                            decimal Marks = txtALevelMark != null && txtALevelMark.Text != "" ? Convert.ToDecimal(txtALevelMark.Text) : 0;

                            decimal GradeValue = Convert.ToDecimal(ddlALevelGrade.SelectedValue);
                            string Grade = ddlALevelGrade.SelectedItem.ToString();

                            if (Marks > 0 && GradeValue > 0)
                            {
                                DAL.OandALevelCandidateSubjectWiseResult NewObj = new DAL.OandALevelCandidateSubjectWiseResult();

                                NewObj.ExamTypeId = ExamtypeId;
                                NewObj.EducationBoardId = EducationBoard;
                                NewObj.CandidateId = cId;
                                NewObj.SubjectNo = SubNo;
                                NewObj.ExamMark = Marks;
                                NewObj.ObtainedGrade = Grade;
                                NewObj.ObtainedMark = (GradeValue / 100) * Marks;
                                try
                                {
                                    NewObj.CreatedBy = Convert.ToInt32(cId);
                                }
                                catch (Exception ex)
                                {
                                    NewObj.CreatedBy = -12;
                                }

                                NewObj.CreatedDate = DateTime.Now;

                                using (var dbInsert = new CandidateDataManager())
                                {
                                    dbInsert.Insert<DAL.OandALevelCandidateSubjectWiseResult>(NewObj);
                                    if (NewObj.Id > 0)
                                    {
                                        TotalMarks = TotalMarks + Convert.ToDecimal(NewObj.ObtainedMark);
                                        TotalTaken = TotalTaken + Marks;
                                    }
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }

                    txtHigherSec_Marks.Text = Math.Round(TotalMarks, 2).ToString();
                    txtOutofHigherSec_Marks.Text = Math.Round(TotalTaken, 2).ToString();

                    //modalPopupSubjectWiseResultALevel.Hide();

                    showAlert("Result calculated successfully");

                }


            }
            catch (Exception ex)
            {
            }
        }

        private int GetgridViewGradeEntryCountALevel(GridView gvALevelSubjectResult)
        {
            int count = 0;
            try
            {
                for (int i = 0; i < gvALevelSubjectResult.Rows.Count; i++)
                {
                    GridViewRow row = gvALevelSubjectResult.Rows[i];
                    TextBox txtALevelMark = (TextBox)row.FindControl("txtALevelMark");
                    DropDownList ddlALevelGrade = (DropDownList)row.FindControl("ddlALevelGrade");
                    if (txtALevelMark != null && !string.IsNullOrEmpty(txtALevelMark.Text)
                        && ddlALevelGrade != null && ddlALevelGrade.SelectedIndex > 0)
                    {
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return count;
        }

        #endregion

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

    }
}