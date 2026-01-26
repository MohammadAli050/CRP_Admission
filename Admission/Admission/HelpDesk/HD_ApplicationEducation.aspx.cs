using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.HelpDesk
{
    public partial class HD_ApplicationEducation : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppAdditional.NavigateUrl = "HD_ApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "HD_ApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "HD_ApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "HD_ApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "HD_ApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                hrefAppPriority.NavigateUrl = "HD_ApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "HD_ApplicationRelation.aspx?val=" + queryVal;
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

                //ddlHigherSec_PassingYear.Items.Clear();
                //ddlHigherSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
                //ddlHigherSec_PassingYear.AppendDataBoundItems = true;
                //ddlSec_PassingYear.Items.Clear();
                //ddlSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
                //ddlSec_PassingYear.AppendDataBoundItems = true;
                //ddlUndergrad_PassingYear.Items.Clear();
                //ddlUndergrad_PassingYear.Items.Add(new ListItem("Select", "-1"));
                //ddlUndergrad_PassingYear.AppendDataBoundItems = true;
                //ddlGraduate_PassingYear.Items.Clear();
                //ddlGraduate_PassingYear.Items.Add(new ListItem("Select", "-1"));
                //ddlGraduate_PassingYear.AppendDataBoundItems = true;
                //for (int i = DateTime.Now.Year; i > 1950; i--)
                //{
                //    ddlHigherSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                //    ddlSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                //    ddlUndergrad_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                //    ddlGraduate_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                //}
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
                    LoadPassingYearDDLForGrad();

                }
                else //bachelors candidate, do not show panel_isUndergrad
                {
                    hfEduCat.Value = "bachelors";

                    panel_isUndergrad.Visible = false;

                    //do not let bachelors candidate change divisionClass in ssc and hsc
                    ddlSec_DivClass.SelectedItem.Text = "GPA";
                    ddlSec_DivClass.Enabled = false;
                    txtSec_CgpaScore.Enabled = true;

                    ddlHigherSec_DivClass.SelectedItem.Text = "GPA";
                    ddlHigherSec_DivClass.Enabled = false;
                    txtHigherSec_CgpaScore.Enabled = true;

                    //load different passing year for bachelors candidate
                    LoadPassingYearDDLForUndergrad();


                    List<DAL.CandidateOALevelGPA> temp = new List<DAL.CandidateOALevelGPA>();
                    using (var db = new CandidateDataManager())
                    {
                        temp = db.AdmissionDB.CandidateOALevelGPAs.ToList();
                    }

                    //DAL.CandidateOALevelGPA temp2 = new DAL.CandidateOALevelGPA();
                    temp = temp.Where(x => x.CandidateID == cId).ToList();

                    if (temp != null && temp.Count > 0)
                    {
                        txtSec_CgpaScore.Enabled = true;
                        txtHigherSec_CgpaScore.Enabled = true;

                        //hfSscGPA.Value = temp[0].ExamTypeID == 5 ? temp[0].SscAndHscGpa.ToString() : null;
                        //hfHscGPA.Value = temp[1].ExamTypeID == 7 ? temp[1].SscAndHscGpa.ToString() : null;

                        txtSec_CgpaScore.Text = temp[0].ExamTypeID == 5 ? temp[0].SscAndHscGpa.ToString() : temp[0].SscAndHscGpa.ToString();
                        txtHigherSec_CgpaScore.Text = temp[1].ExamTypeID == 7 ? temp[1].SscAndHscGpa.ToString() : temp[1].SscAndHscGpa.ToString();

                        ddlSec_ExamType.SelectedValue = temp[0].ExamTypeID.ToString();
                        ddlHigherSec_ExamType.SelectedValue = temp[1].ExamTypeID.ToString();

                        ddlSec_GrpOrSub.SelectedValue = temp[0].GroupID.ToString();
                        ddlHigherSec_GrpOrSub.SelectedValue = temp[1].GroupID.ToString();

                        ddlSec_PassingYear.SelectedValue = temp[0].PassingYear.ToString();
                        ddlHigherSec_PassingYear.SelectedValue = temp[1].PassingYear.ToString();
                    }
                    else
                    {
                        txtSec_CgpaScore.Enabled = true;
                        txtHigherSec_CgpaScore.Enabled = true;

                        ddlSec_ExamType.Enabled = true;
                        ddlHigherSec_ExamType.Enabled = true;

                        ddlSec_GrpOrSub.Enabled = true;
                        ddlHigherSec_GrpOrSub.Enabled = true;

                        ddlSec_PassingYear.Enabled = true;
                        ddlHigherSec_PassingYear.Enabled = true;
                    }


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
                                txtSec_CgpaScore.Enabled = false;
                                txtSec_CgpaScore.Text = null;
                            }
                            txtSec_Marks.Text = secExamDetail.Marks.ToString();
                            txtSec_CgpaW4S.Text = secExamDetail.GPAW4S.ToString();
                            ddlSec_PassingYear.SelectedValue = secExamDetail.PassingYear.ToString();

                            if (secExamDetail.AttributeDec1 != null)
                                txtOutofSec_Marks.Text = secExamDetail.AttributeDec1.ToString();
                            else
                                txtOutofSec_Marks.Text = string.Empty;

                            //if (secExamDetail.EducationBoardID == 13) // O Level Cambridge can not change total achived mark
                            //{
                            //    lnkAddResult.Visible = true;
                            //}
                            //else
                            //    lnkAddResult.Visible = false;

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
                                txtHigherSec_CgpaScore.Enabled = false;
                                txtHigherSec_CgpaScore.Text = null;
                            }
                            txtHigherSec_Marks.Text = higherSecExamDetail.Marks.ToString();
                            txtHigherSec_GpaW4S.Text = higherSecExamDetail.GPAW4S.ToString();
                            ddlHigherSec_PassingYear.SelectedValue = higherSecExamDetail.PassingYear.ToString();

                            txtOutofHigherSec_Marks.Text = higherSecExamDetail.AttributeDec1 == null ? "" : higherSecExamDetail.AttributeDec1.ToString();

                            //if (higherSecExamDetail.EducationBoardID == 13) // A Level Cambridge can not change total achived mark
                            //{
                            //    lnkAddALevelResult.Visible = true;
                            //}
                            //else
                            //    lnkAddALevelResult.Visible = false;

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

        private void LoadPassingYearDDLForUndergrad()
        {
            ddlHigherSec_PassingYear.Items.Clear();
            ddlHigherSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
            ddlHigherSec_PassingYear.AppendDataBoundItems = true;
            ddlSec_PassingYear.Items.Clear();
            ddlSec_PassingYear.Items.Add(new ListItem("Select", "-1"));
            ddlSec_PassingYear.AppendDataBoundItems = true;

            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 2; i--)
            {
                ddlHigherSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 5; i--)
            {
                ddlSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddlUndergrad_PassingYear.Items.Clear();
            ddlUndergrad_PassingYear.Items.Add(new ListItem("Select", "-1"));
            //ddlUndergrad_PassingYear.AppendDataBoundItems = true;
            ddlGraduate_PassingYear.Items.Clear();
            ddlGraduate_PassingYear.Items.Add(new ListItem("Select", "-1"));
            //ddlGraduate_PassingYear.AppendDataBoundItems = true;
        }

        private void LoadPassingYearDDLForGrad()
        {
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
            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                ddlHigherSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlSec_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlUndergrad_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlGraduate_PassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
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
    }
}