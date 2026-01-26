using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.PostgraduateDiploma.PDCandidateInfo
{
    public partial class CandApplicationEducation : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppAdditional.NavigateUrl = "CandApplicationAdditional.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppAddress.NavigateUrl = "CandApplicationAddress.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppAttachment.NavigateUrl = "CandApplicationAttachment.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppBasic.NavigateUrl = "CandApplicationBasic.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppEducation.NavigateUrl = "CandApplicationEducation.aspx?val=" + Request.QueryString["val"].ToString();
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + Request.QueryString["val"].ToString();
                //hrefAppPriority.NavigateUrl = "CandApplicationPriority.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppRelation.NavigateUrl = "CandApplicationRelation.aspx?val=" + Request.QueryString["val"].ToString();


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
                for (int i = DateTime.Now.Year; i > 1950; i--)
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

                List<DAL.PostgraduateDiplomaCandidateFormSl> candidateFormSlList = null;
                DAL.PostgraduateDiplomaAdmissionSetup admSetup = null;

                using (var db = new CandidateDataManager())
                {
                    candidateFormSlList = db.AdmissionDB.PostgraduateDiplomaCandidateFormSls.Where(x => x.CandidateID == cId).ToList();
                    if (candidateFormSlList != null)
                    {
                        //get only admSetup for masters.
                        admSetup = db.AdmissionDB.PostgraduateDiplomaAdmissionSetups.Where(x => x.EducationCategoryID == 1).FirstOrDefault();

                        //admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6).Select(c => c.AdmissionSetup).FirstOrDefault();
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

                    txtSec_CgpaScore.Enabled = true;
                    txtHigherSec_CgpaScore.Enabled = true;

                    ddlSec_ExamType.Enabled = true;
                    ddlHigherSec_ExamType.Enabled = true;

                    ddlSec_GrpOrSub.Enabled = true;
                    ddlHigherSec_GrpOrSub.Enabled = true;

                    ddlSec_PassingYear.Enabled = true;
                    ddlHigherSec_PassingYear.Enabled = true;



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

                    //}




                }


                using (var db = new CandidateDataManager())
                {

                    DAL.PostgraduateDiplomaExam secondaryExam = db.GetPostgraduateDiplomaSecondaryExamByCandidateID_AD(cId);
                    DAL.PostgraduateDiplomaExam higherSecondaryExam = db.GetPostgraduateDiplomaHigherSecdExamByCandidateID_AD(cId);
                    DAL.PostgraduateDiplomaExam undergradExam = db.GetPostgraduateDiplomaUndergradExamByCandidateID_AD(cId);
                    DAL.PostgraduateDiplomaExam gradExam = db.GetPostgraduateDiplomaGradExamByCandidateID_AD(cId);

                    if (secondaryExam != null)
                    {
                        DAL.PostgraduateDiplomaExamDetail secExamDetail = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == secondaryExam.ExamDetailsID).FirstOrDefault();
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
                        }
                    }//end if(secondaryExam.ID > 0)
                    if (higherSecondaryExam != null)
                    {
                        DAL.PostgraduateDiplomaExamDetail higherSecExamDetail = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == higherSecondaryExam.ExamDetailsID).FirstOrDefault();
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
                        }
                    }// end if(higherSecondaryExam.ID > 0)
                    if (undergradExam != null)
                    {
                        DAL.PostgraduateDiplomaExamDetail undergradExamDetail = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == undergradExam.ExamDetailsID).FirstOrDefault();
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
                        DAL.PostgraduateDiplomaExamDetail gradExamDetail = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == gradExam.ExamDetailsID).FirstOrDefault();
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

                if (cId > 0)
                {
                    //DAL.Exam secondaryExam = null;
                    //DAL.Exam highSecondaryExam = null;
                    //DAL.Exam undergradExam = null;
                    //DAL.Exam gradExam = null;

                    DAL.PostgraduateDiplomaExam secondaryExam = null;
                    DAL.PostgraduateDiplomaExam highSecondaryExam = null;
                    DAL.PostgraduateDiplomaExam undergradExam = null;
                    DAL.PostgraduateDiplomaExam gradExam = null;

                    using (var db = new CandidateDataManager())
                    {
                        //List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);

                        List<DAL.PostgraduateDiplomaExam> examList = db.AdmissionDB.PostgraduateDiplomaExams.Where(x => x.CandidateID == cId).ToList();

                        //if (examList != null)
                        //{
                        //    secondaryExam = examList.Where(a => a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6).FirstOrDefault();
                        //    highSecondaryExam = examList.Where(a => a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9).FirstOrDefault();
                        //    undergradExam = examList.Where(a => a.ExamTypeID == 3 || a.ExamTypeID == 10).FirstOrDefault();
                        //    gradExam = examList.Where(a => a.ExamTypeID == 4).FirstOrDefault();
                        //}

                        if (examList != null)
                        {
                            secondaryExam = examList.Where(a => a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6).FirstOrDefault();
                            highSecondaryExam = examList.Where(a => a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9).FirstOrDefault();
                            undergradExam = examList.Where(a => a.ExamTypeID == 3 || a.ExamTypeID == 10).FirstOrDefault();
                            gradExam = examList.Where(a => a.ExamTypeID == 4).FirstOrDefault();
                        }
                    }

                    #region SSC/O-Level ======================================================

                    if (secondaryExam != null) //secondary exam exist.
                    {
                        DAL.PostgraduateDiplomaExamDetail secExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

                        using (var db = new CandidateDataManager())
                        {
                            secExmDtlObj = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == secondaryExam.ExamDetailsID).FirstOrDefault();
                        }


                        if (secExmDtlObj != null) // secondary exam detail exist. Update.
                        {
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

                            secExmDtlObj.PassingYear = Int32.Parse(ddlSec_PassingYear.SelectedValue);

                            secExmDtlObj.DateModified = DateTime.Now;
                            secExmDtlObj.ModifiedBy = cId;

                            using (var dbUpdateSecExamDetails = new CandidateDataManager())
                            {
                                dbUpdateSecExamDetails.Update<DAL.PostgraduateDiplomaExamDetail>(secExmDtlObj);
                            }
                        }
                        else //secondary exam detail does not exist. create update exam details.
                        {
                            DAL.PostgraduateDiplomaExamDetail newSecExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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

                            newSecExmDtlObj.PassingYear = Int32.Parse(ddlSec_PassingYear.SelectedValue);

                            newSecExmDtlObj.DateCreated = DateTime.Now;
                            newSecExmDtlObj.CreatedBy = cId;

                            long newSecExmDtlObjID = -1;
                            using (var dbInsertSecExamDetails = new CandidateDataManager())
                            {
                                dbInsertSecExamDetails.Insert<DAL.PostgraduateDiplomaExamDetail>(newSecExmDtlObj);
                                newSecExmDtlObjID = newSecExmDtlObj.ID;
                            }
                            if (newSecExmDtlObjID > 0)
                            {
                                secondaryExam.ExamDetailsID = newSecExmDtlObjID;

                                using (var dbUpdateSecExam = new CandidateDataManager())
                                {
                                    dbUpdateSecExam.Update<DAL.PostgraduateDiplomaExam>(secondaryExam);
                                }
                            }
                        }
                    }
                    else //secondary exam does not exist.
                    {
                        DAL.PostgraduateDiplomaExamDetail newSecExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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
                        newSecExmDtlObj.PassingYear = Int32.Parse(ddlSec_PassingYear.SelectedValue);

                        newSecExmDtlObj.DateCreated = DateTime.Now;
                        newSecExmDtlObj.CreatedBy = cId;

                        DAL.PostgraduateDiplomaExam newSecExamObj = new DAL.PostgraduateDiplomaExam();

                        newSecExamObj.CandidateID = cId;
                        newSecExamObj.ExamTypeID = Int32.Parse(ddlSec_ExamType.SelectedValue);
                        newSecExamObj.CreatedBy = cId;
                        newSecExamObj.DateCreated = DateTime.Now;

                        using (var dbInsertSecExamDtl = new CandidateDataManager())
                        {
                            dbInsertSecExamDtl.Insert<DAL.PostgraduateDiplomaExamDetail>(newSecExmDtlObj);
                            newSecExamObj.ExamDetailsID = newSecExmDtlObj.ID;
                        }
                        using (var dbInsertSecExam = new CandidateDataManager())
                        {
                            dbInsertSecExam.Insert<DAL.PostgraduateDiplomaExam>(newSecExamObj);
                        }
                    }

                    #endregion SSC/O-Level 

                    #region HSC/A-Level/Diploma ==============================================

                    if (highSecondaryExam != null) //higher secondary exam exist.
                    {
                        DAL.PostgraduateDiplomaExamDetail higherSecExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

                        using (var db = new CandidateDataManager())
                        {
                            higherSecExmDtlObj = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == highSecondaryExam.ExamDetailsID).FirstOrDefault();
                        }
                        //DAL.ExamDetail higherSecExmDtlObj = highSecondaryExam.ExamDetail;

                        if (higherSecExmDtlObj != null) // higher secondary exam detail exist. Update.
                        {
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
                            higherSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);

                            higherSecExmDtlObj.DateModified = DateTime.Now;
                            higherSecExmDtlObj.ModifiedBy = cId;

                            using (var dbUpdateHighSecExamDetails = new CandidateDataManager())
                            {
                                dbUpdateHighSecExamDetails.Update<DAL.PostgraduateDiplomaExamDetail>(higherSecExmDtlObj);
                            }
                        }
                        else //higher secondary exam detail does not exist. create update exam details.
                        {
                            DAL.PostgraduateDiplomaExamDetail newHigherSecExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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
                            newHigherSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);

                            newHigherSecExmDtlObj.DateCreated = DateTime.Now;
                            newHigherSecExmDtlObj.CreatedBy = cId;

                            long newHighSecExmDtlObjID = -1;
                            using (var dbInsertHighSecExamDetails = new CandidateDataManager())
                            {
                                dbInsertHighSecExamDetails.Insert<DAL.PostgraduateDiplomaExamDetail>(newHigherSecExmDtlObj);
                                newHighSecExmDtlObjID = newHigherSecExmDtlObj.ID;
                            }
                            if (newHighSecExmDtlObjID > 0)
                            {
                                highSecondaryExam.ExamDetailsID = newHighSecExmDtlObjID;

                                using (var dbUpdateHighSecExam = new CandidateDataManager())
                                {
                                    dbUpdateHighSecExam.Update<DAL.PostgraduateDiplomaExam>(highSecondaryExam);
                                }
                            }
                        }
                    }
                    else //higher secondary exam does not exist. Create new exam detail and then exam.
                    {
                        DAL.PostgraduateDiplomaExamDetail newHighSecExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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
                        newHighSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);

                        newHighSecExmDtlObj.DateCreated = DateTime.Now;
                        newHighSecExmDtlObj.CreatedBy = cId;

                        DAL.PostgraduateDiplomaExam newHighSecExamObj = new DAL.PostgraduateDiplomaExam();

                        newHighSecExamObj.CandidateID = cId;
                        newHighSecExamObj.ExamTypeID = Int32.Parse(ddlHigherSec_ExamType.SelectedValue);
                        newHighSecExamObj.CreatedBy = cId;
                        newHighSecExamObj.DateCreated = DateTime.Now;

                        using (var dbInsertHighSecExamDtl = new CandidateDataManager())
                        {
                            dbInsertHighSecExamDtl.Insert<DAL.PostgraduateDiplomaExamDetail>(newHighSecExmDtlObj);
                            newHighSecExamObj.ExamDetailsID = newHighSecExmDtlObj.ID;
                        }
                        using (var dbInsertHighSecExam = new CandidateDataManager())
                        {
                            dbInsertHighSecExam.Insert<DAL.PostgraduateDiplomaExam>(newHighSecExamObj);
                        }
                    }

                    #endregion HSC/A-Level/Diploma


                    List<DAL.PostgraduateDiplomaCandidateFormSl> candidateFormSlList = new List<DAL.PostgraduateDiplomaCandidateFormSl>();
                    List<DAL.PostgraduateDiplomaAdmissionSetup> admSetupList = new List<DAL.PostgraduateDiplomaAdmissionSetup>();
                    DAL.PostgraduateDiplomaAdmissionSetup admSetup = new DAL.PostgraduateDiplomaAdmissionSetup();

                    using (var db = new CandidateDataManager())
                    {
                        candidateFormSlList = db.AdmissionDB.PostgraduateDiplomaCandidateFormSls.Where(x => x.CandidateID == cId).ToList(); //db.GetAllCandidateFormSlByCandID_AD(cId);
                        if (candidateFormSlList != null)
                        {
                            admSetupList = db.AdmissionDB.PostgraduateDiplomaAdmissionSetups.ToList();
                            //get only admSetup for masters.
                            //admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6).Select(c => c.AdmissionSetup).FirstOrDefault();
                            admSetup = admSetupList.Where(c => c.ID == candidateFormSlList[0].AdmissionSetupID).FirstOrDefault();
                        }
                    }

                    if (admSetup != null)
                    {
                        if (admSetup.EducationCategoryID == 1) //applying for masters...hence undergrad info is saved/updated.
                        {
                            #region UNDERGRAD ================================================

                            //check whether candidate is providing any details. if not dont save.
                            if (!string.IsNullOrEmpty(txtUndergrad_Institute.Text.Trim()) ||
                                !string.IsNullOrEmpty(txtUndergrad_CgpaScore.Text.Trim()) ||
                                Int32.Parse(ddlUndergrad_PassingYear.SelectedValue) > 0)
                            {

                                if (undergradExam != null) //undergrad exam exist.
                                {
                                    //DAL.ExamDetail undergradExmDtlObj = undergradExam.ExamDetail;
                                    DAL.PostgraduateDiplomaExamDetail undergradExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

                                    using (var db = new CandidateDataManager())
                                    {
                                        undergradExmDtlObj = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == undergradExam.ExamDetailsID).FirstOrDefault();
                                    }


                                    if (undergradExmDtlObj != null) // undergrad exam detail exist. Update.
                                    {
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
                                            dbUpdateUndergradExamDetails.Update<DAL.PostgraduateDiplomaExamDetail>(undergradExmDtlObj);
                                        }
                                    }
                                    else //undergrad exam detail does not exist. create new exam details.
                                    {
                                        DAL.PostgraduateDiplomaExamDetail newUndergradExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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
                                            dbInsertUndergradExamDetails.Insert<DAL.PostgraduateDiplomaExamDetail>(newUndergradExmDtlObj);
                                            newUndergradExmDtlObjID = newUndergradExmDtlObj.ID;
                                        }
                                        if (newUndergradExmDtlObjID > 0)
                                        {
                                            undergradExam.ExamDetailsID = newUndergradExmDtlObjID;

                                            using (var dbUpdateUndergradExam = new CandidateDataManager())
                                            {
                                                dbUpdateUndergradExam.Update<DAL.PostgraduateDiplomaExam>(undergradExam);
                                            }
                                        }
                                    }
                                }
                                else //undergrad exam does not exist. Create new exam detail and then exam.
                                {
                                    DAL.PostgraduateDiplomaExamDetail newUndergradExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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

                                    DAL.PostgraduateDiplomaExam newUndergradExamObj = new DAL.PostgraduateDiplomaExam();

                                    newUndergradExamObj.CandidateID = cId;
                                    newUndergradExamObj.ExamTypeID = 3;
                                    newUndergradExamObj.CreatedBy = cId;
                                    newUndergradExamObj.DateCreated = DateTime.Now;

                                    using (var dbInsertUndergradExamDtl = new CandidateDataManager())
                                    {
                                        dbInsertUndergradExamDtl.Insert<DAL.PostgraduateDiplomaExamDetail>(newUndergradExmDtlObj);
                                        newUndergradExamObj.ExamDetailsID = newUndergradExmDtlObj.ID;
                                    }
                                    using (var dbInsertUndergradExam = new CandidateDataManager())
                                    {
                                        dbInsertUndergradExam.Insert<DAL.PostgraduateDiplomaExam>(newUndergradExamObj);
                                    }
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
                                    //DAL.ExamDetail gradExmDtlObj = gradExam.ExamDetail;

                                    DAL.PostgraduateDiplomaExamDetail gradExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

                                    using (var db = new CandidateDataManager())
                                    {
                                        gradExmDtlObj = db.AdmissionDB.PostgraduateDiplomaExamDetails.Where(x => x.ID == gradExam.ExamDetailsID).FirstOrDefault();
                                    }


                                    if (gradExmDtlObj != null) // grad exam detail exist. Update.
                                    {
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
                                            dbUpdateGradExamDetails.Update<DAL.PostgraduateDiplomaExamDetail>(gradExmDtlObj);
                                        }
                                    }
                                    else //grad exam detail does not exist. create new exam details.
                                    {
                                        DAL.PostgraduateDiplomaExamDetail newGradExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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
                                            dbInsertGradExamDetails.Insert<DAL.PostgraduateDiplomaExamDetail>(newGradExmDtlObj);
                                            newGradExmDtlObjID = newGradExmDtlObj.ID;
                                        }
                                        if (newGradExmDtlObjID > 0)
                                        {
                                            gradExam.ExamDetailsID = newGradExmDtlObjID;

                                            using (var dbUpdateGradExam = new CandidateDataManager())
                                            {
                                                dbUpdateGradExam.Update<DAL.PostgraduateDiplomaExam>(gradExam);
                                            }
                                        }
                                    }
                                }
                                else //undergrad exam does not exist. Create new exam detail and then exam.
                                {
                                    DAL.PostgraduateDiplomaExamDetail newGradExmDtlObj = new DAL.PostgraduateDiplomaExamDetail();

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

                                    DAL.PostgraduateDiplomaExam newGradExamObj = new DAL.PostgraduateDiplomaExam();

                                    newGradExamObj.CandidateID = cId;
                                    newGradExamObj.ExamTypeID = 4;
                                    newGradExamObj.CreatedBy = cId;
                                    newGradExamObj.DateCreated = DateTime.Now;

                                    using (var dbInsertHighSecExamDtl = new CandidateDataManager())
                                    {
                                        dbInsertHighSecExamDtl.Insert<DAL.PostgraduateDiplomaExamDetail>(newGradExmDtlObj);
                                        newGradExamObj.ExamDetailsID = newGradExmDtlObj.ID;
                                    }
                                    using (var dbInsertHighSecExam = new CandidateDataManager())
                                    {
                                        dbInsertHighSecExam.Insert<DAL.PostgraduateDiplomaExam>(newGradExamObj);
                                    }
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
    }
}