using Admission.App_Start;
using CommonUtility;
using DAL;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.CandidateInfo
{
    public partial class CandApplicationEducation : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        string userName = "";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key
            using (var db = new OfficeDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppAdditional.NavigateUrl = "CandApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "CandApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "CandApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "CandApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "CandApplicationEducation.aspx?val=" + queryVal;
                hrefAppPriority.NavigateUrl = "CandApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "CandApplicationRelation.aspx?val=" + queryVal;
                hrefAppDeclaration.NavigateUrl = "CandApplicationDeclaration.aspx?val=" + queryVal;
            }

            if (!IsPostBack)
            {
                LoadDDL();
                LoadCandidateData(cId);
            }
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.ExamType> examTypeList = db.GetAllExamType_ND();
                if (examTypeList != null && examTypeList.Any())
                {
                    DDLHelper.Bind<DAL.ExamType>(ddlSec_ExamType, examTypeList.Where(a => a.EducationCategory_ID == 2).ToList(), "ExamTypeName", "ID", EnumCollection.ListItemType.ExamType);
                    DDLHelper.Bind<DAL.ExamType>(ddlHigherSec_ExamType, examTypeList.Where(a => a.EducationCategory_ID == 3).ToList(), "ExamTypeName", "ID", EnumCollection.ListItemType.ExamType);
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
                    DDLHelper.Bind<DAL.GroupOrSubject>(ddlUndergrad_GrpOrSub, groupOrSubjectList.Where(a => a.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                    DDLHelper.Bind<DAL.GroupOrSubject>(ddlGraduate_GrpOrSub, groupOrSubjectList.Where(a => a.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                }

                List<DAL.ResultDivision> resultDivisionList = db.GetAllResultDivision_ND();
                if (resultDivisionList != null && resultDivisionList.Any())
                {
                    DDLHelper.Bind<DAL.ResultDivision>(ddlSec_DivClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlHigherSec_DivClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
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

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {

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

                }
                else //bachelors candidate, do not show panel_isUndergrad
                {
                    hfEduCat.Value = "bachelors";

                    panel_isUndergrad.Visible = false;

                    //do not let bachelors candidate change divisionClass in ssc and hsc
                    //ddlSec_DivClass.SelectedItem.Text = "GPA";
                    ddlSec_DivClass.Enabled = true;
                    txtSec_CgpaScore.Enabled = true;

                    //ddlHigherSec_DivClass.SelectedItem.Text = "GPA";
                    ddlHigherSec_DivClass.Enabled = true;
                    txtHigherSec_CgpaScore.Enabled = true;

                    //load different passing year for bachelors candidate
                    //LoadPassingYearDDLForUndergrad();



                    #region Getting data from CandidateOALevelGPA N/A
                    //List<DAL.CandidateOALevelGPA> temp = new List<DAL.CandidateOALevelGPA>();
                    //using (var db = new CandidateDataManager())
                    //{
                    //    temp = db.AdmissionDB.CandidateOALevelGPAs.ToList();
                    //}

                    ////DAL.CandidateOALevelGPA temp2 = new DAL.CandidateOALevelGPA();
                    //temp = temp.Where(x => x.CandidateID == cId).ToList();

                    //if (temp != null && temp.Count > 0)
                    //{
                    //    txtSec_CgpaScore.Enabled = true;
                    //    txtHigherSec_CgpaScore.Enabled = true;

                    //    //hfSscGPA.Value = temp[0].ExamTypeID == 5 ? temp[0].SscAndHscGpa.ToString() : null;
                    //    //hfHscGPA.Value = temp[1].ExamTypeID == 7 ? temp[1].SscAndHscGpa.ToString() : null;

                    //    txtSec_CgpaScore.Text = temp[0].ExamTypeID == 5 ? temp[0].SscAndHscGpa.ToString() : temp[0].SscAndHscGpa.ToString();
                    //    txtHigherSec_CgpaScore.Text = temp[1].ExamTypeID == 7 ? temp[1].SscAndHscGpa.ToString() : temp[1].SscAndHscGpa.ToString();

                    //    ddlSec_ExamType.SelectedValue = temp[0].ExamTypeID.ToString();
                    //    ddlHigherSec_ExamType.SelectedValue = temp[1].ExamTypeID.ToString();

                    //    ddlSec_GrpOrSub.SelectedValue = temp[0].GroupID.ToString();
                    //    ddlHigherSec_GrpOrSub.SelectedValue = temp[1].GroupID.ToString();

                    //    ddlSec_PassingYear.SelectedValue = temp[0].PassingYear.ToString();
                    //    ddlHigherSec_PassingYear.SelectedValue = temp[1].PassingYear.ToString();
                    //}
                    //else
                    //{
                    //    txtSec_CgpaScore.Enabled = true;
                    //    txtHigherSec_CgpaScore.Enabled = true;

                    //    ddlSec_ExamType.Enabled = true;
                    //    ddlHigherSec_ExamType.Enabled = true;

                    //    ddlSec_GrpOrSub.Enabled = true;
                    //    ddlHigherSec_GrpOrSub.Enabled = true;

                    //    ddlSec_PassingYear.Enabled = true;
                    //    ddlHigherSec_PassingYear.Enabled = true;
                    //} 
                    #endregion
                
                }


                using (var db = new CandidateDataManager())
                {

                    DAL.Exam secondaryExam = db.GetSecondaryExamByCandidateID_AD(cId);
                    DAL.Exam higherSecondaryExam = db.GetHigherSecdExamByCandidateID_AD(cId);
                    DAL.Exam undergradExam = db.GetUndergradExamByCandidateID_AD(cId);
                    DAL.Exam gradExam = db.GetGradExamByCandidateID_AD(cId);

                    if (secondaryExam != null)
                    {
                        DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                        if (secExamDetail != null && secExamDetail.ID > 0)
                        {
                            ddlSec_ExamType.SelectedValue = secondaryExam.ExamTypeID.ToString();
                            ddlSec_EducationBrd.SelectedValue = secExamDetail.EducationBoardID.ToString();
                            txtSec_Institute.Text = secExamDetail.Institute;
                            txtSec_RollNo.Text = secExamDetail.RollNo;
                            txtSec_RegNo.Text = secExamDetail.RegistrationNo;
                            ddlSec_GrpOrSub.SelectedValue = secExamDetail.GroupOrSubjectID.ToString();
                            ddlSec_DivClass.SelectedValue = secExamDetail.ResultDivisionID.ToString();
                            if (secExamDetail.ResultDivisionID == 5)
                            {
                                txtSec_CgpaScore.Enabled = true;
                                txtSec_CgpaScore.Text = secExamDetail.GPA.ToString();
                            }
                            else
                            {
                                txtSec_CgpaScore.Enabled = true;
                                txtSec_CgpaScore.Text = null;
                            }
                            txtSec_Marks.Text = secExamDetail.Marks.ToString();
                            txtSec_CgpaW4S.Text = secExamDetail.GPAW4S.ToString();
                            ddlSec_PassingYear.SelectedValue = secExamDetail.PassingYear.ToString();

                            if (secExamDetail.AttributeDec1 != null)
                                txtOutofSec_Marks.Text = secExamDetail.AttributeDec1.ToString();
                            else
                                txtOutofSec_Marks.Text = string.Empty;

                            if (secExamDetail.EducationBoardID == 13 || secExamDetail.EducationBoardID == 12) // O Level Cambridge And Edexel can not change total achived mark
                            {
                                lnkAddResult.Visible = true;
                            }
                            else
                                lnkAddResult.Visible = false;
                        }
                    }//end if(secondaryExam.ID > 0)
                    if (higherSecondaryExam != null)
                    {
                        DAL.ExamDetail higherSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);
                        if (higherSecExamDetail != null && higherSecExamDetail.ID > 0)
                        {
                            ddlHigherSec_ExamType.SelectedValue = higherSecondaryExam.ExamTypeID.ToString();
                            ddlHigherSec_EducationBrd.SelectedValue = higherSecExamDetail.EducationBoardID.ToString();
                            txtHigherSec_Institute.Text = higherSecExamDetail.Institute;
                            txtHigherSec_RollNo.Text = higherSecExamDetail.RollNo;
                            txtHigherSec_RegNo.Text = higherSecExamDetail.RegistrationNo;
                            ddlHigherSec_GrpOrSub.SelectedValue = higherSecExamDetail.GroupOrSubjectID.ToString();
                            ddlHigherSec_DivClass.SelectedValue = higherSecExamDetail.ResultDivisionID.ToString();
                            if (higherSecExamDetail.ResultDivisionID == 5)
                            {
                                txtHigherSec_CgpaScore.Enabled = true;
                                txtHigherSec_CgpaScore.Text = higherSecExamDetail.GPA.ToString();
                            }
                            else
                            {
                                txtHigherSec_CgpaScore.Enabled = true;
                                txtHigherSec_CgpaScore.Text = null;
                            }
                            txtHigherSec_Marks.Text = higherSecExamDetail.Marks.ToString();
                            txtHigherSec_GpaW4S.Text = higherSecExamDetail.GPAW4S.ToString();
                            ddlHigherSec_PassingYear.SelectedValue = higherSecExamDetail.PassingYear.ToString();

                            txtOutofHigherSec_Marks.Text = higherSecExamDetail.AttributeDec1 == null ? "" : higherSecExamDetail.AttributeDec1.ToString();

                            if (higherSecExamDetail.EducationBoardID == 13 || higherSecExamDetail.EducationBoardID == 12) // A Level Cambridge and Edexel can not change total achived mark
                            {
                                lnkAddALevelResult.Visible = true;
                            }
                            else
                                lnkAddALevelResult.Visible = false;

                        }
                    }// end if(higherSecondaryExam.ID > 0)
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
                                txtUndergrad_ProgOthers.Enabled = false;
                            }
                            ddlUndergrad_GrpOrSub.SelectedValue = undergradExamDetail.GroupOrSubjectID.ToString();
                            ddlUndergrad_DivClass.SelectedValue = undergradExamDetail.ResultDivisionID.ToString();
                            if (undergradExamDetail.ResultDivisionID == 5)
                            {
                                txtUndergrad_CgpaScore.Text = undergradExamDetail.CGPA.ToString();
                                txtUndergrad_CgpaScore.Enabled = true;
                            }
                            else
                            {
                                txtUndergrad_CgpaScore.Text = null;
                                txtUndergrad_CgpaScore.Enabled = false;
                            }
                            ddlUndergrad_PassingYear.SelectedValue = undergradExamDetail.PassingYear.ToString();
                        }
                    }//end if(undergradExam.ID > 0)
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
                                txtGraduate_ProgOthers.Enabled = false;
                            }
                            ddlGraduate_GrpOrSub.SelectedValue = gradExamDetail.GroupOrSubjectID.ToString();
                            ddlGraduate_DivClass.SelectedValue = gradExamDetail.ResultDivisionID.ToString();
                            if (gradExamDetail.ResultDivisionID == 5)
                            {
                                txtGraduate_CgpaScore.Text = gradExamDetail.CGPA.ToString();
                                txtGraduate_CgpaScore.Enabled = true;
                            }
                            else
                            {
                                txtGraduate_CgpaScore.Text = null;
                                txtGraduate_CgpaScore.Enabled = false;
                            }
                            ddlGraduate_PassingYear.SelectedValue = gradExamDetail.PassingYear.ToString();
                        }
                    }// end if(gradExam.ID > 0)



                    //#region New Added For Total Out of Marks

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

                    //#endregion

                }// end using
            }// if(cId > 0)
        }

        //private void LoadPassingYearDDLForUndergrad()
        //{
        //    ddlHigherSec_PassingYear.Items.Clear();
        //    ddlHigherSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlHigherSec_PassingYear.AppendDataBoundItems = true;
        //    ddlSec_PassingYear.Items.Clear();
        //    ddlSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlSec_PassingYear.AppendDataBoundItems = true;

        //    for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 2; i--)
        //    {
        //        ddlHigherSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }

        //    for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 5; i--)
        //    {
        //        ddlSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }

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
                txtSec_CgpaScore.Enabled = false;
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
                txtHigherSec_CgpaScore.Enabled = false;
                //txtHigSecCgpaRFV.IsValid = true;
                //txtHigSecCgpaRFV.Enabled = false;
            }
        }

        protected void ddlUndergrad_ProgramDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUndergrad_ProgramDegree.SelectedItem.Text == "OTHERS")
            {
                txtUndergrad_ProgOthers.Enabled = true;
                txtUndergrad_ProgOthers.Text = string.Empty;
                //txtUndergrad_ProgramOtherRFV.Enabled = true;
            }
            else
            {
                txtUndergrad_ProgOthers.Enabled = false;
                txtUndergrad_ProgOthers.Text = string.Empty;
                //txtUndergrad_ProgramOtherRFV.Enabled = false;
            }
        }

        protected void ddlUndergrad_DivClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUndergrad_DivClass.SelectedValue == "5")
            {
                txtUndergrad_CgpaScore.Enabled = true;
                txtUndergrad_CgpaScore.Text = string.Empty;
            }
            else
            {
                txtUndergrad_CgpaScore.Enabled = false;
                txtUndergrad_CgpaScore.Text = string.Empty;
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
                txtGraduate_ProgOthers.Enabled = false;
                txtGraduate_ProgOthers.Text = string.Empty;
                //txtUndergrad_ProgramOtherRFV.Enabled = false;
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
                txtGraduate_CgpaScore.Enabled = false;
                txtGraduate_CgpaScore.Text = string.Empty;
            }
        }

        protected void btnSave_Education_Click(object sender, EventArgs e)
        {
            long cId = -1;
            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);
            }

            try
            {

                if (cId > 0 && uId > 0)
                {
                    string logOldObject = string.Empty;
                    string logNewObject = string.Empty;


                    DAL.BasicInfo basicInfo = null;
                    DAL.CandidatePayment candidatePayment = null;

                    DAL.Exam secondaryExam = null;
                    DAL.Exam highSecondaryExam = null;
                    DAL.Exam undergradExam = null;
                    DAL.Exam gradExam = null;

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

                    #region Check Total Mark Is Less than or equal to total required mark

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

                        if (totalhscMark > totaloutofhscMark)
                        {
                            lblMessageEducation.Text = "Total HSC Marks must be less than or equal to total out of marks";
                            messagePanel_Education.CssClass = "alert alert-danger";
                            messagePanel_Education.Visible = true;
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion

                    #region SSC/O-Level ======================================================

                    if (secondaryExam != null) //secondary exam exist.
                    {
                        DAL.ExamDetail secExmDtlObj = secondaryExam.ExamDetail;

                        #region SSC Exam Details Update / Insurt
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

                            if (ddlSec_DivClass.Enabled == true)
                            {
                                secExmDtlObj.ResultDivisionID = Int32.Parse(ddlSec_DivClass.SelectedValue);
                            }
                            else
                            {
                                secExmDtlObj.ResultDivisionID = 5; //5 = GPA
                            }

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

                            secExmDtlObj.DateModified = DateTime.Now;
                            secExmDtlObj.ModifiedBy = uId;

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
                                dLog.EventName = "Education Info Update (SSC) (Admin)";
                                dLog.PageName = "CandApplicationEducation.aspx";
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

                            if (ddlSec_DivClass.Enabled == true)
                            {
                                newSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlSec_DivClass.SelectedValue);
                            }
                            else
                            {
                                newSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                            }

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

                            newSecExmDtlObj.DateCreated = DateTime.Now;
                            newSecExmDtlObj.CreatedBy = uId;

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
                                dLog.EventName = "Education Info Update (SSC) (Admin)";
                                dLog.PageName = "CandApplicationEducation.aspx";
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
                        }
                        #endregion

                        #region SSC Emxa Update
                        DAL.Exam exam = new DAL.Exam();
                        exam.ID = secondaryExam.ID;
                        exam.CandidateID = secondaryExam.CandidateID;
                        exam.ExamTypeID = Convert.ToInt32(ddlSec_ExamType.SelectedValue);
                        exam.ExamDetailsID = secondaryExam.ExamDetailsID;
                        exam.CreatedBy = uId;
                        exam.DateCreated = secondaryExam.DateCreated;

                        using (var dbUpdateSecExam = new CandidateDataManager())
                        {
                            dbUpdateSecExam.Update<DAL.Exam>(exam);
                        }

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
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

                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.CandidateId = cId;
                            dLog.EventName = "Education Info Update (SSC) (Admin)";
                            dLog.PageName = "CandApplicationEducation.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
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
                    else //secondary exam does not exist.
                    {
                        DAL.ExamDetail newSecExmDtlObj = new DAL.ExamDetail();

                        newSecExmDtlObj.EducationBoardID = Int32.Parse(ddlSec_EducationBrd.SelectedValue);
                        newSecExmDtlObj.Institute = txtSec_Institute.Text.Trim();
                        newSecExmDtlObj.UndgradGradProgID = 1;
                        newSecExmDtlObj.RollNo = txtSec_RollNo.Text.Trim();
                        newSecExmDtlObj.RegistrationNo = txtSec_RegNo.Text.Trim();
                        newSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlSec_GrpOrSub.SelectedValue);

                        if (ddlSec_DivClass.Enabled == true)
                        {
                            newSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlSec_DivClass.SelectedValue);
                        }
                        else
                        {
                            newSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                        }

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

                        newSecExmDtlObj.DateCreated = DateTime.Now;
                        newSecExmDtlObj.CreatedBy = uId;

                        DAL.Exam newSecExamObj = new DAL.Exam();

                        newSecExamObj.CandidateID = cId;
                        newSecExamObj.ExamTypeID = Int32.Parse(ddlSec_ExamType.SelectedValue);
                        newSecExamObj.CreatedBy = uId;
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
                            dLog.EventName = "Education Info Insert (SSC) (Admin)";
                            dLog.PageName = "CandApplicationEducation.aspx";
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
                    }

                    #endregion SSC/O-Level 

                    #region HSC/A-Level/Diploma ==============================================

                    if (highSecondaryExam != null) //higher secondary exam exist.
                    {
                        DAL.ExamDetail higherSecExmDtlObj = highSecondaryExam.ExamDetail;

                        #region HSC Exam Details Update / Insurt
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

                            if (ddlHigherSec_DivClass.Enabled == true)
                            {
                                higherSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                            }
                            else
                            {
                                higherSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                            }

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

                            if (!string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text.Trim()))
                            {
                                higherSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofHigherSec_Marks.Text.Trim());
                            }
                            else
                            {
                                higherSecExmDtlObj.AttributeDec1 = null;
                            }

                            higherSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);

                            higherSecExmDtlObj.DateModified = DateTime.Now;
                            higherSecExmDtlObj.ModifiedBy = uId;

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
                                dLog.EventName = "Education Info Update (HSC) (Admin)";
                                dLog.PageName = "CandApplicationEducation.aspx";
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

                            if (ddlHigherSec_DivClass.Enabled == true)
                            {
                                newHigherSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                            }
                            else
                            {
                                newHigherSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                            }

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

                            if (!string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text.Trim()))
                            {
                                higherSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofHigherSec_Marks.Text.Trim());
                            }
                            else
                            {
                                higherSecExmDtlObj.AttributeDec1 = null;
                            }

                            newHigherSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);

                            newHigherSecExmDtlObj.DateCreated = DateTime.Now;
                            newHigherSecExmDtlObj.CreatedBy = uId;

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
                                dLog.EventName = "Education Info Update (HSC) (Admin)";
                                dLog.PageName = "CandApplicationEducation.aspx";
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
                        }
                        #endregion

                        #region HSC Emxa Update
                        DAL.Exam exam = new DAL.Exam();
                        exam.ID = highSecondaryExam.ID;
                        exam.CandidateID = highSecondaryExam.CandidateID;
                        exam.ExamTypeID = Convert.ToInt32(ddlHigherSec_ExamType.SelectedValue);
                        exam.ExamDetailsID = highSecondaryExam.ExamDetailsID;
                        exam.CreatedBy = uId;
                        exam.DateCreated = highSecondaryExam.DateCreated;

                        using (var dbUpdateHighSecExam = new CandidateDataManager())
                        {
                            dbUpdateHighSecExam.Update<DAL.Exam>(exam);
                        }
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
                            dLog.EventName = "Education Info Update (HSC) (Admin)";
                            dLog.PageName = "CandApplicationEducation.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Education Information.";
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
                    else //higher secondary exam does not exist. Create new exam detail and then exam.
                    {
                        DAL.ExamDetail newHighSecExmDtlObj = new DAL.ExamDetail();

                        newHighSecExmDtlObj.EducationBoardID = Int32.Parse(ddlHigherSec_EducationBrd.SelectedValue);
                        newHighSecExmDtlObj.Institute = txtHigherSec_Institute.Text.Trim();
                        newHighSecExmDtlObj.UndgradGradProgID = 1;
                        newHighSecExmDtlObj.RollNo = txtHigherSec_RollNo.Text.Trim();
                        newHighSecExmDtlObj.RegistrationNo = txtHigherSec_RegNo.Text.Trim();
                        newHighSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlHigherSec_GrpOrSub.SelectedValue);

                        if (ddlHigherSec_DivClass.Enabled == true)
                        {
                            newHighSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                        }
                        else
                        {
                            newHighSecExmDtlObj.ResultDivisionID = 5; //5 = GPA
                        }

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

                        if (!string.IsNullOrEmpty(txtOutofHigherSec_Marks.Text.Trim()))
                        {
                            newHighSecExmDtlObj.AttributeDec1 = Decimal.Parse(txtOutofHigherSec_Marks.Text.Trim());
                        }
                        else
                        {
                            newHighSecExmDtlObj.AttributeDec1 = null;
                        }

                        newHighSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);

                        newHighSecExmDtlObj.DateCreated = DateTime.Now;
                        newHighSecExmDtlObj.CreatedBy = uId;

                        DAL.Exam newHighSecExamObj = new DAL.Exam();

                        newHighSecExamObj.CandidateID = cId;
                        newHighSecExamObj.ExamTypeID = Int32.Parse(ddlHigherSec_ExamType.SelectedValue);
                        newHighSecExamObj.CreatedBy = uId;
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
                            dLog.EventName = "Education Info Insert (HSC) (Admin)";
                            dLog.PageName = "CandApplicationEducation.aspx";
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
                                        undergradExmDtlObj.ModifiedBy = uId;

                                        using (var dbUpdateUndergradExamDetails = new CandidateDataManager())
                                        {
                                            dbUpdateUndergradExamDetails.Update<DAL.ExamDetail>(undergradExmDtlObj);

                                            logNewObject = string.Empty;
                                            logNewObject = GenerateLogStringFromObject(undergradExam, undergradExmDtlObj);
                                        }

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
                                            dLog.EventName = "Education Info Update (Undergrade) (Admin)";
                                            dLog.PageName = "CandApplicationEducation.aspx";
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
                                        newUndergradExmDtlObj.CreatedBy = uId;

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
                                            dLog.EventName = "Education Info Update (Undergrade) (Admin)";
                                            dLog.PageName = "CandApplicationEducation.aspx";
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
                                    newUndergradExmDtlObj.CreatedBy = uId;

                                    DAL.Exam newUndergradExamObj = new DAL.Exam();

                                    newUndergradExamObj.CandidateID = cId;
                                    newUndergradExamObj.ExamTypeID = 3;
                                    newUndergradExamObj.CreatedBy = uId;
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
                                        dLog.EventName = "Education Info Update (Undergrade) (Admin)";
                                        dLog.PageName = "CandApplicationEducation.aspx";
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
                                        gradExmDtlObj.ModifiedBy = uId;

                                        using (var dbUpdateGradExamDetails = new CandidateDataManager())
                                        {
                                            dbUpdateGradExamDetails.Update<DAL.ExamDetail>(gradExmDtlObj);

                                            logNewObject = string.Empty;
                                            logNewObject = GenerateLogStringFromObject(gradExam, gradExmDtlObj);
                                        }

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
                                            dLog.EventName = "Education Info Update (Grade) (Admin)";
                                            dLog.PageName = "CandApplicationEducation.aspx";
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
                                        newGradExmDtlObj.CreatedBy = uId;

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
                                            dLog.EventName = "Education Info Update (Grade) (Admin)";
                                            dLog.PageName = "CandApplicationEducation.aspx";
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
                                    newGradExmDtlObj.CreatedBy = uId;

                                    DAL.Exam newGradExamObj = new DAL.Exam();

                                    newGradExamObj.CandidateID = cId;
                                    newGradExamObj.ExamTypeID = 4;
                                    newGradExamObj.CreatedBy = uId;
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
                                        dLog.EventName = "Education Info Update (Grade) (Admin)";
                                        dLog.PageName = "CandApplicationEducation.aspx";
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
                                }

                            }

                            #endregion GRAD
                        }
                    }

                }


                lblMessageEducation.Text = "Education Info Updated successfully.";
                messagePanel_Education.CssClass = "alert alert-success";
                messagePanel_Education.Visible = true;

                //Response.Redirect("ApplicationRelation.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessageEducation.Text = "[Admin] Unable to save/update candidate information. " + ex.Message + "; " + ex.InnerException.Message;
                messagePanel_Education.CssClass = "alert alert-danger";
                messagePanel_Education.Visible = true;
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







        #region O Level Result Entry

        protected void lnkAddResult_Click(object sender, EventArgs e)
        {
            try
            {
                int boardId = Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);

                if (boardId == 13 || boardId == 12) // O Level Cambridge
                {
                    ClearOLevelGrid();

                    CheckAndBindGridViewOfOLevel();

                    modalPopupSubjectWiseResult.Show();
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

                        GridRebindOLevel(ExistingList);

                        modalPopupSubjectWiseResult.Show();
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
                    ClearOLevelGrid();
                    GenerateSubjectsGrid(QuestionNo);
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearOLevelGrid()
        {
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
                    modalPopupSubjectWiseResult.Show();
                    gvOLevelSubjectResult.DataSource = subjectList;
                    gvOLevelSubjectResult.DataBind();
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

                    modalPopupSubjectWiseResult.Hide();

                    showAlert("Result calculated successfully");
                }


            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region A Level Result Entry

        protected void lnkAddALevelResult_Click(object sender, EventArgs e)
        {
            try
            {
                int boardId = Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);

                if (boardId == 13 || boardId == 12) // A Level Cambridge
                {
                    ClearALevelGrid();

                    CheckAndBindGridViewOfALevel();

                    modalPopupSubjectWiseResultALevel.Show();
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

                        GridRebindALevel(ExistingList);

                        modalPopupSubjectWiseResultALevel.Show();
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
                    ClearALevelGrid();
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
                    modalPopupSubjectWiseResultALevel.Show();
                    gvALevelSubjectResult.DataSource = subjectList;
                    gvALevelSubjectResult.DataBind();
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

                    modalPopupSubjectWiseResultALevel.Hide();

                    showAlert("Result calculated successfully");

                }


            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

    }
}