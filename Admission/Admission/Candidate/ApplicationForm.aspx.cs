using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class ApplicationForm : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            using (var dbGas = new OfficeDataManager())
            {
                DAL.GlobalAdmissionSetting gas = new DAL.GlobalAdmissionSetting();
                gas = dbGas.AdmissionDB.GlobalAdmissionSettings.Find(1);
                if (gas.ID > 0)
                {
                    if (gas.IsSystemAvailable == false)
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Sorry, system is not available now. Please try again later or contact administrator.&type=danger");
                    }
                }
                else
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Internal Error (Unable to find system).&type=danger");
                }
            }

            if (!IsPostBack)
            {
                LoadDDL();
                //LoadInstituteInfo();
                LoadCandidateData();
            }
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.Country>(ddlNationality, db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);
                DDLHelper.Bind<DAL.Language>(ddlLanguage, db.AdmissionDB.Languages.Where(a => a.IsActive == true).OrderBy(a => a.LanguageName).ToList(), "LanguageName", "ID", EnumCollection.ListItemType.MotherTongue);
                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).OrderBy(a => a.GenderName).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                DDLHelper.Bind<DAL.MaritalStatu>(ddlMaritalStatus, db.AdmissionDB.MaritalStatus.Where(a => a.IsActive == true).OrderBy(a => a.MaritalStatus).ToList(), "MaritalStatus", "ID", EnumCollection.ListItemType.MaritalStatus);
                DDLHelper.Bind<DAL.BloodGroup>(ddlBloodGroup, db.AdmissionDB.BloodGroups.OrderBy(a => a.ID).ToList(), "BloodGroupName", "ID", EnumCollection.ListItemType.BloodGroup);
                DDLHelper.Bind<DAL.Religion>(ddlReligion, db.AdmissionDB.Religions.Where(a => a.IsActive == true).OrderBy(a => a.ReligionName).ToList(), "ReligionName", "ID", EnumCollection.ListItemType.Religion);
                //DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(a => a.QuotaName).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);
                //----------------------------------------------
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
                }

                List<DAL.ResultDivision> resultDivisionList = db.GetAllResultDivision_ND();
                if (resultDivisionList != null && resultDivisionList.Any())
                {
                    DDLHelper.Bind<DAL.ResultDivision>(ddlSec_DivClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlHigherSec_DivClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
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

                //---------------------------------------------
                DDLHelper.Bind<DAL.RelationType>(ddlGuardianRelation, db.AdmissionDB.RelationTypes.Where(a => a.IsActive == true && a.RelationTypeName != "Financial Guarantor").ToList(), "RelationTypeName", "ID", EnumCollection.ListItemType.Select);
                //---------------------------------------------
                //division
                List<DAL.Division> divisionList = db.GetAllDivision_ND();
                DDLHelper.Bind<DAL.Division>(ddlPresentDivision, divisionList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Division);
                DDLHelper.Bind<DAL.Division>(ddlPermanentDivision, divisionList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Division);

                List<DAL.District> districtList = db.GetAllDistrict_ND();
                DDLHelper.Bind<DAL.District>(ddlPresentDistrict, districtList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
                DDLHelper.Bind<DAL.District>(ddlPermanentDistrict, districtList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
                //---------------------------------------------
                DDLHelper.Bind<DAL.RelationType>(ddlRelationWithGuarantor, db.AdmissionDB.RelationTypes.Where(a => a.IsActive == true && a.RelationTypeName != "Financial Guarantor").ToList(), "RelationTypeName", "ID", EnumCollection.ListItemType.Select);
                //---------------------------------------------
                //admitted before
                ddlAdmittedBefore.Items.Clear();
                ddlAdmittedBefore.Items.Add(new ListItem("Select", "-1"));
                ddlAdmittedBefore.Items.Add(new ListItem("Yes", "Yes"));
                ddlAdmittedBefore.Items.Add(new ListItem("No", "No"));
                //dismissed before
                ddlDismissedBefore.Items.Clear();
                ddlDismissedBefore.Items.Add(new ListItem("Select", "-1"));
                ddlDismissedBefore.Items.Add(new ListItem("Yes", "Yes"));
                ddlDismissedBefore.Items.Add(new ListItem("No", "No"));

            }
        }

        private void LoadCandidateData()
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
                    using (var db = new CandidateDataManager())
                    {
                        #region BASIC INFO
                        //basic info-------------------------------------------------------
                        DAL.BasicInfo candidate = db.GetCandidateBasicInfoByID_ND(cId);
                        if (candidate != null && candidate.ID > 0)
                        {
                            txtFirstName.Text = candidate.FirstName;
                            txtMiddleName.Text = candidate.MiddleName;
                            txtLastName.Text = candidate.LastName;
                            txtNickName.Text = candidate.NickName;
                            txtDateOfBirth.Text = candidate.DateOfBirth.ToString("dd/MM/yyyy");
                            txtPlaceOfBirth.Text = candidate.PlaceOfBirth;
                            ddlNationality.SelectedValue = candidate.NationalityID.ToString();
                            ddlLanguage.SelectedValue = candidate.MotherTongueID.ToString();
                            ddlGender.SelectedValue = candidate.GenderID.ToString();
                            ddlMaritalStatus.SelectedValue = candidate.MaritalStatusID.ToString();
                            txtNationalId.Text = candidate.NationalIdNumber;
                            ddlBloodGroup.SelectedValue = candidate.BloodGroupID.ToString();
                            txtEmail.Text = candidate.Email;
                            txtPhoneRes.Text = candidate.PhoneResidence;
                            txtPhoneEmergency.Text = candidate.EmergencyPhone;
                            //txtMobile.Text = candidate.Mobile; // not needed now
                            txtMobile.Text = candidate.SMSPhone;
                            ddlReligion.SelectedValue = candidate.ReligionID.ToString();
                            //ddlQuota.SelectedValue = candidate.QuotaID.ToString();  //required for BUP
                        }
                        #endregion

                        #region EDUCATION
                        //education--------------------------------------------------------

                        DAL.Exam secondaryExam = db.GetSecondaryExamByCandidateID_AD(cId);
                        DAL.Exam higherSecondaryExam = db.GetHigherSecdExamByCandidateID_AD(cId);
                        DAL.Exam undergradExam = db.GetUndergradExamByCandidateID_AD(cId);
                        DAL.Exam gradExam = db.GetGradExamByCandidateID_AD(cId);

                        if (secondaryExam != null)
                        {
                            DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                            if (secExamDetail != null && secExamDetail.ID > 0)
                            {
                                ddlSec_ExamType.SelectedValue = secExamDetail.ExamTypeID.ToString();
                                ddlSec_EducationBrd.SelectedValue = secExamDetail.EducationBoardID.ToString();
                                txtSec_Institute.Text = secExamDetail.Institute;
                                txtSec_RollNo.Text = secExamDetail.RollNo;
                                //txtSec_RegNo.Text = secExamDetail.RegistrationNo;
                                ddlSec_GrpOrSub.SelectedValue = secExamDetail.GroupOrSubjectID.ToString();
                                ddlSec_DivClass.SelectedValue = secExamDetail.ResultDivisionID.ToString();
                                txtSec_CgpaScore.Text = secExamDetail.CGPA.ToString();
                                txtSec_Marks.Text = secExamDetail.Marks.ToString();
                                ddlSec_PassingYear.SelectedValue = secExamDetail.PassingYear.ToString();
                            }
                        }//end if(secondaryExam.ID > 0)
                        if (higherSecondaryExam.ID > 0 || higherSecondaryExam != null)
                        {
                            DAL.ExamDetail higherSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);
                            if (higherSecExamDetail != null && higherSecExamDetail.ID > 0)
                            {
                                ddlHigherSec_ExamType.SelectedValue = higherSecExamDetail.ExamTypeID.ToString();
                                ddlHigherSec_EducationBrd.SelectedValue = higherSecExamDetail.EducationBoardID.ToString();
                                txtHigherSec_Institute.Text = higherSecExamDetail.Institute;
                                txtHigherSec_RollNo.Text = higherSecExamDetail.RollNo;
                                //txtHigherSec_RegNo.Text = higherSecExamDetail.RegistrationNo;
                                ddlHigherSec_GrpOrSub.SelectedValue = higherSecExamDetail.GroupOrSubjectID.ToString();
                                ddlHigherSec_DivClass.SelectedValue = higherSecExamDetail.ResultDivisionID.ToString();
                                txtHigherSec_CgpaScore.Text = higherSecExamDetail.CGPA.ToString();
                                txtHigherSec_Marks.Text = higherSecExamDetail.Marks.ToString();
                                ddlHigherSec_PassingYear.SelectedValue = higherSecExamDetail.PassingYear.ToString();
                            }
                        }// end if(higherSecondaryExam.ID > 0)
                        if (undergradExam != null)
                        {
                            DAL.ExamDetail undergradExamDetail = db.GetExamDetailByID_ND(undergradExam.ExamDetailsID);
                            if (undergradExamDetail != null && undergradExamDetail.ID > 0)
                            {
                                txtUndergrad_Institute.Text = undergradExamDetail.Institute;
                                ddlUndergrad_ProgramDegree.SelectedValue = undergradExamDetail.UndgradGradProgID.ToString();
                                txtUndergrad_ProgOthers.Text = undergradExamDetail.OtherProgram;
                                txtUndergrad_CgpaScore.Text = undergradExamDetail.CGPA.ToString();
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
                                txtGraduate_ProgOthers.Text = gradExamDetail.OtherProgram;
                                txtGraduate_CgpaScore.Text = gradExamDetail.CGPA.ToString();
                                ddlGraduate_PassingYear.SelectedValue = gradExamDetail.PassingYear.ToString();
                            }
                        }// end if(gradExam.ID > 0)
                        #endregion

                        #region PARENT
                        //parent/guardian--------------------------------------------------

                        List<DAL.Relation> relationList = db.GetAllRelationByCandidateID_ND(cId);
                        if (relationList.Any())
                        {
                            DAL.Relation fatherRelation = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();
                            DAL.Relation motherRelation = relationList.Where(a => a.RelationTypeID == 3).FirstOrDefault();
                            DAL.Relation guardianRelation = relationList.Where(a => a.RelationTypeID == 9).FirstOrDefault();

                            if (fatherRelation != null)
                            {
                                DAL.RelationDetail father = db.GetRelationDetailByID_ND(fatherRelation.RelationDetailsID);
                                if (father != null)
                                {
                                    txtFatherName.Text = father.Name;
                                    txtFatherMobile.Text = father.Mobile;
                                    txtFatherOccupation.Text = father.Occupation;
                                    txtFatherOrgAddress.Text = father.OrgAddress;
                                    //txtFatherOrganization.Text = father.Organization;
                                }
                            }
                            if (motherRelation != null)
                            {
                                DAL.RelationDetail mother = db.GetRelationDetailByID_ND(motherRelation.RelationDetailsID);
                                if (mother != null)
                                {
                                    txtMotherName.Text = mother.Name;
                                    txtMotherMobile.Text = mother.Mobile;
                                    txtMotherOccupation.Text = mother.Occupation;
                                    txtMotherMailingAddress.Text = mother.OrgAddress;
                                    //txtMotherOrganization.Text = mother.Organization;
                                }
                            }
                            if (guardianRelation != null)
                            {
                                DAL.RelationDetail guardian = db.GetRelationDetailByID_ND(guardianRelation.RelationDetailsID);
                                if (guardian != null)
                                {
                                    txtGuardian_Name.Text = guardian.Name;
                                    txtGuardianEmail.Text = guardian.Email;
                                    txtGuardianMailingAddress.Text = guardian.MailingAddress;
                                    txtGuardianMobile.Text = guardian.Mobile;
                                    if (guardian.RelationWithGuardian == "Father")
                                    {
                                        ddlGuardianRelation.SelectedValue = "2";
                                        txtGuardianOtherRelation.Enabled = false;
                                    }
                                    else if (guardian.RelationWithGuardian == "Mother")
                                    {
                                        ddlGuardianRelation.SelectedValue = "3";
                                        txtGuardianOtherRelation.Enabled = false;
                                    }
                                    else if (guardian.RelationWithGuardian == null)
                                    {
                                        ddlGuardianRelation.SelectedValue = "-1";
                                        txtGuardianOtherRelation.Enabled = false;
                                    }
                                    else
                                    {
                                        ddlGuardianRelation.SelectedValue = "10";
                                        txtGuardianOtherRelation.Text = guardian.RelationWithGuardian;
                                        txtGuardianOtherRelation.Enabled = true;
                                    }
                                    txtGuardianPhoneOffice.Text = guardian.PhoneOffice;
                                    txtGuardianPhoneRes.Text = guardian.PhoneResidence;
                                }
                            }
                        }
                        #endregion

                        #region ADDRESS
                        //address----------------------------------------------------------

                        List<DAL.Address> addressList = db.GetAllAddressByCandidateID_ND(cId);
                        if (addressList.Any())
                        {
                            DAL.Address presAdd = addressList.Where(a => a.AddressTypeID == 2).FirstOrDefault();
                            DAL.Address permAdd = addressList.Where(a => a.AddressTypeID == 3).FirstOrDefault();

                            if (presAdd != null)
                            {
                                DAL.AddressDetail presAddress = db.GetAddressDetailByID_ND(presAdd.AddressDetailsID);

                                txtPresentAddress.Text = presAddress.AddressLine;
                                ddlPresentDivision.SelectedValue = presAddress.DivisionID.ToString();
                                ddlPresentDistrict.SelectedValue = presAddress.DistrictID.ToString();
                                txtPresentUpazila.Text = presAddress.Upazila;
                            }

                            if (permAdd != null)
                            {
                                DAL.AddressDetail permAddress = db.GetAddressDetailByID_ND(presAdd.AddressDetailsID);

                                txtPermanentAddress.Text = permAddress.AddressLine;
                                ddlPermanentDivision.SelectedValue = permAddress.DivisionID.ToString();
                                ddlPermanentDistrict.SelectedValue = permAddress.DistrictID.ToString();
                                txtPermanentUpazila.Text = permAddress.Upazila;
                            }
                        }
                        #endregion

                        #region FINANCIAL GUARANTOR
                        //financial guarantor----------------------------------------------

                        if (relationList.Any())
                        {
                            DAL.Relation fgRelation = relationList.Where(a => a.RelationTypeID == 8).FirstOrDefault();
                            if (fgRelation != null)
                            {
                                DAL.RelationDetail finGuaran = db.GetRelationDetailByID_ND(fgRelation.RelationDetailsID);
                                if (finGuaran != null)
                                {
                                    txtFinGuarantorName.Text = finGuaran.Name;
                                    if (finGuaran.RelationWithGuardian == "Father")
                                    {
                                        ddlRelationWithGuarantor.SelectedValue = "2";
                                        txtRelationWithGuarantorOthers.Enabled = false;
                                    }
                                    else if (finGuaran.RelationWithGuardian == "Mother")
                                    {
                                        ddlRelationWithGuarantor.SelectedValue = "3";
                                        txtRelationWithGuarantorOthers.Enabled = false;
                                    }
                                    else if (finGuaran.RelationWithGuardian == null)
                                    {
                                        ddlRelationWithGuarantor.SelectedValue = "-1";
                                        txtRelationWithGuarantorOthers.Enabled = false;
                                    }
                                    else
                                    {
                                        ddlRelationWithGuarantor.SelectedValue = "10";
                                        txtRelationWithGuarantorOthers.Enabled = true;
                                        txtRelationWithGuarantorOthers.Text = finGuaran.RelationWithGuardian;
                                    }
                                    txtFinGuarantorOccupation.Text = finGuaran.Occupation;
                                    txtFinGuarantorOrganization.Text = finGuaran.Organization;
                                    txtFinGuarantorPosition.Text = finGuaran.Designation;
                                    txtFinGuarantorAddress.Text = finGuaran.MailingAddress;
                                    txtFinGuarantorEmail.Text = finGuaran.Email;
                                    txtFinGuarantorPhoneRes.Text = finGuaran.PhoneResidence;
                                    txtFinGuarantorPhoneOffice.Text = finGuaran.PhoneOffice;
                                }
                            }
                        }
                        #endregion

                        #region ADDITIONAL
                        //additional-------------------------------------------------------
                        DAL.AdditionalInfo addInfo = db.GetAdditionalInfoByCandidateID_ND(cId);
                        if (addInfo != null)
                        {
                            if (addInfo.CurrentStudentId == null)
                            {
                                ddlAdmittedBefore.SelectedValue = "No";
                                txtCurrentStudentId.Text = null;
                                txtCurrentStudentId.Enabled = false;
                            }
                            else if (addInfo.CurrentStudentId != null)
                            {
                                ddlAdmittedBefore.SelectedValue = "Yes";
                                txtCurrentStudentId.Text = addInfo.CurrentStudentId;
                                txtCurrentStudentId.Enabled = true;
                            }
                            else
                            {
                                ddlAdmittedBefore.SelectedValue = "-1";
                                txtCurrentStudentId.Text = null;
                                txtCurrentStudentId.Enabled = false;
                            }

                            if (addInfo.IsDismissedBefore == true)
                            {
                                ddlDismissedBefore.SelectedValue = "Yes";
                                txtDismissalStatement.Text = addInfo.DismissalStatement;
                                txtDismissalStatement.Enabled = true;
                            }
                            else if (addInfo.IsDismissedBefore == false)
                            {
                                ddlDismissedBefore.SelectedValue = "No";
                                txtDismissalStatement.Text = null;
                                txtDismissalStatement.Enabled = false;
                            }
                            else
                            {
                                ddlDismissedBefore.SelectedValue = "-1";
                                txtDismissalStatement.Text = null;
                                txtDismissalStatement.Enabled = false;
                            }
                        }
                        #endregion

                        //work exp---------------------------------------------------------
                        //photo------------------------------------------------------------
                    }// end using
                }// if(cId > 0)
            }// if(uId > 0)

        }

        protected void btnSave_Basic_Click(object sender, EventArgs e)
        {
            DAL.BasicInfo candidate = new DAL.BasicInfo();

            candidate.FirstName = txtFirstName.Text.ToUpper();
            candidate.MiddleName = txtMiddleName.Text.ToUpper();
            candidate.LastName = txtLastName.Text.ToUpper();
            candidate.Mobile = txtMobile.Text;
            candidate.PhoneResidence = txtPhoneRes.Text;
            candidate.EmergencyPhone = txtPhoneEmergency.Text;
            candidate.SMSPhone = txtMobile.Text;
            candidate.Email = txtEmail.Text;
            //DateTime dob = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
            candidate.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null); ;
            candidate.PlaceOfBirth = txtPlaceOfBirth.Text;
            candidate.NationalityID = ddlNationality.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlNationality.SelectedValue);
            candidate.MotherTongueID = ddlLanguage.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlLanguage.SelectedValue);
            candidate.MaritalStatusID = ddlMaritalStatus.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlMaritalStatus.SelectedValue);
            candidate.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
            //candidate.QuotaID = ddlQuota.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlQuota.SelectedValue); //only for BUP
            candidate.QuotaID = 1;
            candidate.BloodGroupID = ddlBloodGroup.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlBloodGroup.SelectedValue);
            candidate.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);
            candidate.GuardianPhone = null;
            candidate.NationalIdNumber = txtNationalId.Text;

            try
            {
                if (uId > 0)
                {
                    DAL.BasicInfo obj = new DAL.BasicInfo();
                    using (var db = new CandidateDataManager())
                    {
                        obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                    }// end using
                    if (obj != null && obj.ID > 0)  //update if object exists
                    {
                        candidate.ID = obj.ID;
                        candidate.CreatedBy = obj.CreatedBy;
                        candidate.DateCreated = obj.DateCreated;
                        candidate.DateModified = DateTime.Now;
                        candidate.CandidateUserID = obj.CandidateUserID;
                        candidate.ModifiedBy = obj.ID;
                        using (var updateDb = new CandidateDataManager())
                        {
                            updateDb.Update<DAL.BasicInfo>(candidate);
                        }
                    }
                    else //else save
                    {
                        candidate.DateCreated = DateTime.Now;
                        candidate.CreatedBy = -99;
                        using (var insertDb = new CandidateDataManager())
                        {
                            insertDb.Insert<DAL.BasicInfo>(candidate);
                        }
                    } // end if-else
                }
                lblMessageBasic.Text = "Basic Info updated successfully.";
                messagePanel_Basic.CssClass = "alert alert-success";
                messagePanel_Basic.Visible = true;

            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X001TC&type=danger", false);
                lblMessageBasic.Text = "Unable to save/update candidate information. Error Code : F01X001TC";
                messagePanel_Basic.CssClass = "alert alert-danger";
                messagePanel_Basic.Visible = true;
            }
        }

        protected void btnSave_Education_Click(object sender, EventArgs e)
        {
            try
            {
                #region SSC/OLevel
                DAL.ExamDetail secExmDtlObj = new DAL.ExamDetail();

                secExmDtlObj.EducationBoardID = Int32.Parse(ddlSec_EducationBrd.SelectedValue);
                secExmDtlObj.Institute = txtSec_Institute.Text;
                secExmDtlObj.UndgradGradProgID = 1;
                secExmDtlObj.RollNo = txtSec_RollNo.Text;
                secExmDtlObj.RegistrationNo = null;
                secExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlSec_GrpOrSub.SelectedValue);
                secExmDtlObj.ResultDivisionID = Int32.Parse(ddlSec_DivClass.SelectedValue);
                secExmDtlObj.CGPA = Decimal.Parse(txtSec_CgpaScore.Text.Trim());
                secExmDtlObj.Marks = Decimal.Parse(txtSec_Marks.Text);
                secExmDtlObj.PassingYear = Int32.Parse(ddlSec_PassingYear.SelectedValue);


                try
                {
                    if (uId > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            List<DAL.Exam> candExamList = db.GetAllExamByCandidateID_AD(cId);
                            if (candExamList.Any())
                            {
                                DAL.Exam secondaryExam = candExamList.Where(a => a.ExamDetail.ExamTypeID == 2 || a.ExamDetail.ExamTypeID == 3 || a.ExamDetail.ExamTypeID == 4)
                                    .FirstOrDefault();

                                if (secondaryExam.ID > 0)
                                {
                                    DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                                    if (secExamDetail != null && secExamDetail.ID > 0) //update
                                    {
                                        secExmDtlObj.DateCreated = secExamDetail.DateCreated;
                                        secExmDtlObj.CreatedBy = secExamDetail.CreatedBy;
                                        secExmDtlObj.DateModified = DateTime.Now;
                                        secExmDtlObj.ModifiedBy = cId;

                                        db.Update<DAL.ExamDetail>(secExmDtlObj);
                                    }
                                    else //save
                                    {
                                        secExmDtlObj.DateCreated = DateTime.Now;
                                        secExmDtlObj.CreatedBy = cId;

                                        db.Insert<DAL.ExamDetail>(secExmDtlObj);
                                    }
                                }//end if(secondaryExam.ID > 0)

                            }
                        }// end using

                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X002TC&type=danger", false);
                }//end try-catch  secondary school 
                 //----------------------------------------------------------------------------------------------------------------------------------------------------
                #endregion
                #region HSC/ALevel
                DAL.ExamDetail higherSecExmDtlObj = new DAL.ExamDetail();

                higherSecExmDtlObj.EducationBoardID = Int32.Parse(ddlHigherSec_EducationBrd.SelectedValue);
                higherSecExmDtlObj.Institute = txtHigherSec_Institute.Text;
                higherSecExmDtlObj.UndgradGradProgID = 1;
                higherSecExmDtlObj.RollNo = txtHigherSec_RollNo.Text;
                higherSecExmDtlObj.RegistrationNo = null;
                higherSecExmDtlObj.GroupOrSubjectID = Int32.Parse(ddlHigherSec_GrpOrSub.SelectedValue);
                higherSecExmDtlObj.ResultDivisionID = Int32.Parse(ddlHigherSec_DivClass.SelectedValue);
                higherSecExmDtlObj.CGPA = Decimal.Parse(txtHigherSec_CgpaScore.Text);
                higherSecExmDtlObj.Marks = Decimal.Parse(txtHigherSec_Marks.Text);
                higherSecExmDtlObj.PassingYear = Int32.Parse(ddlHigherSec_PassingYear.SelectedValue);

                try
                {
                    if (uId > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            List<DAL.Exam> candExamList = db.GetAllExamByCandidateID_ND(cId);
                            if (candExamList.Any())
                            {
                                DAL.Exam higherSecondaryExam = candExamList.Where(a => a.ExamDetail.ExamTypeID == 5 || a.ExamDetail.ExamTypeID == 6 || a.ExamDetail.ExamTypeID == 7 || a.ExamDetail.ExamTypeID == 8)
                                    .FirstOrDefault();

                                if (higherSecondaryExam.ID > 0)
                                {
                                    DAL.ExamDetail highSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);
                                    if (highSecExamDetail != null && highSecExamDetail.ID > 0) //update
                                    {
                                        higherSecExmDtlObj.DateCreated = highSecExamDetail.DateCreated;
                                        higherSecExmDtlObj.CreatedBy = highSecExamDetail.CreatedBy;
                                        higherSecExmDtlObj.DateModified = DateTime.Now;
                                        higherSecExmDtlObj.ModifiedBy = cId;

                                        db.Update<DAL.ExamDetail>(higherSecExmDtlObj);

                                    }
                                    else //save
                                    {
                                        higherSecExmDtlObj.DateCreated = DateTime.Now;
                                        higherSecExmDtlObj.CreatedBy = cId;

                                        db.Insert<DAL.ExamDetail>(higherSecExmDtlObj);

                                    }
                                }//end if(higherSecondaryExam.ID > 0)

                            }
                        }// end using
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X003TC&type=danger", false);
                }//end try-catch  higher secondary school 
                 //----------------------------------------------------------------------------------------------------------------------------------------------------
                #endregion
                //TODO: marker for saving undergrad and grad info
                //only for candidates applying for masters
                #region UNDERGRAD

                if ((!string.IsNullOrEmpty(txtUndergrad_Institute.Text.Trim()))
                    && (!string.IsNullOrEmpty(txtUndergrad_CgpaScore.Text.Trim()))
                    && (ddlUndergrad_PassingYear.SelectedValue == "-1"))
                {
                    DAL.ExamDetail undergradExmDtlObj = new DAL.ExamDetail();

                    undergradExmDtlObj.Institute = txtUndergrad_Institute.Text;
                    undergradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlUndergrad_ProgramDegree.SelectedValue);
                    undergradExmDtlObj.OtherProgram = txtUndergrad_ProgOthers.Text;
                    undergradExmDtlObj.CGPA = Decimal.Parse(txtUndergrad_CgpaScore.Text);
                    undergradExmDtlObj.PassingYear = Int32.Parse(ddlUndergrad_PassingYear.SelectedValue);

                    try
                    {
                        if (uId > 0)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                List<DAL.Exam> candExamList = db.GetAllExamByCandidateID_ND(cId);
                                if (candExamList.Any())
                                {
                                    DAL.Exam undergradExam = candExamList.Where(a => a.ExamDetail.ExamTypeID == 9 || a.ExamDetail.ExamTypeID == 10)
                                        .FirstOrDefault();

                                    if (undergradExam.ID > 0)
                                    {
                                        DAL.ExamDetail undergradExamDetail = db.GetExamDetailByID_ND(undergradExam.ExamDetailsID);
                                        if (undergradExamDetail != null && undergradExamDetail.ID > 0) //update
                                        {
                                            undergradExmDtlObj.DateCreated = undergradExamDetail.DateCreated;
                                            undergradExmDtlObj.CreatedBy = undergradExamDetail.CreatedBy;
                                            undergradExmDtlObj.DateModified = DateTime.Now;
                                            undergradExmDtlObj.ModifiedBy = cId;

                                            db.Update<DAL.ExamDetail>(undergradExmDtlObj);
                                        }
                                        else //save
                                        {
                                            undergradExmDtlObj.DateCreated = DateTime.Now;
                                            undergradExmDtlObj.CreatedBy = cId;

                                            db.Insert<DAL.ExamDetail>(undergradExmDtlObj);
                                        }
                                    }//end if(secondaryExam.ID > 0)

                                }
                            }// end using
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X004TC&type=danger", false);
                    }//end try-catch  undergrad
                }
                //----------------------------------------------------------------------------------------------------------------------------------------------------
                #endregion
                #region GRAD
                if ((!string.IsNullOrEmpty(txtGraduate_Institute.Text.Trim()))
                    && (!string.IsNullOrEmpty(txtGraduate_CgpaScore.Text.Trim()))
                    && (ddlGraduate_PassingYear.SelectedValue == "-1"))
                {
                    DAL.ExamDetail gradExmDtlObj = new DAL.ExamDetail();

                    gradExmDtlObj.Institute = txtUndergrad_Institute.Text;
                    gradExmDtlObj.UndgradGradProgID = Int32.Parse(ddlUndergrad_ProgramDegree.SelectedValue);
                    gradExmDtlObj.OtherProgram = txtUndergrad_ProgOthers.Text;
                    gradExmDtlObj.CGPA = Decimal.Parse(txtUndergrad_CgpaScore.Text);
                    gradExmDtlObj.PassingYear = Int32.Parse(ddlUndergrad_PassingYear.SelectedValue);

                    try
                    {
                        if (uId > 0)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                List<DAL.Exam> candExamList = db.GetAllExamByCandidateID_ND(cId);
                                if (candExamList.Any())
                                {
                                    DAL.Exam gradExam = candExamList.Where(a => a.ExamDetail.ExamTypeID == 11)
                                        .FirstOrDefault();

                                    if (gradExam.ID > 0)
                                    {
                                        DAL.ExamDetail gradExamDetail = db.GetExamDetailByID_ND(gradExam.ExamDetailsID);
                                        if (gradExamDetail != null && gradExamDetail.ID > 0) //update
                                        {
                                            gradExmDtlObj.DateCreated = gradExamDetail.DateCreated;
                                            gradExmDtlObj.CreatedBy = gradExamDetail.CreatedBy;
                                            gradExmDtlObj.DateModified = DateTime.Now;
                                            gradExmDtlObj.ModifiedBy = cId;

                                            db.Update<DAL.ExamDetail>(gradExmDtlObj);
                                        }
                                        else //save
                                        {
                                            gradExmDtlObj.DateCreated = DateTime.Now;
                                            gradExmDtlObj.CreatedBy = cId;

                                            db.Insert<DAL.ExamDetail>(gradExmDtlObj);
                                        }
                                    }//end if(secondaryExam.ID > 0)

                                }
                            }// end using
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X005TC&type=danger", false);
                    }//end try-catch  graduate
                }
                //----------------------------------------------------------------------------------------------------------------------------------------------------
                #endregion

                lblMessageEducation.Text = "Education Info Updated successfully.";
                messagePanel_Education.CssClass = "alert alert-success";
                messagePanel_Education.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSave_Parent_Click(object sender, EventArgs e)
        {
            try
            {
                #region FATHER
                try
                {

                    using (var db = new CandidateDataManager())
                    {
                        DAL.Relation father_r = db.GetAllRelationByCandidateID_ND(cId).Where(a => a.RelationTypeID == 2).FirstOrDefault();
                        if (father_r != null || father_r.ID > 0) // update relation
                        {
                            DAL.RelationDetail father_rd = db.GetRelationDetailByID_ND(father_r.RelationDetailsID);
                            if (father_rd != null && father_rd.ID > 0) // update relation details
                            {
                                father_rd.Name = txtFatherName.Text;
                                father_rd.Mobile = txtFatherMobile.Text;
                                father_rd.Occupation = txtFatherOccupation.Text;
                                father_rd.OrgAddress = txtFatherOrgAddress.Text;

                                father_rd.DateModified = DateTime.Now;
                                father_rd.ModifiedBy = cId;

                                using (var dbUpdate = new CandidateDataManager())
                                {
                                    dbUpdate.Update<DAL.RelationDetail>(father_rd);
                                }
                            }
                        }
                        else // insert relation
                        {
                            DAL.RelationDetail father_rd = new DAL.RelationDetail();

                            father_rd.Name = txtFatherName.Text;
                            father_rd.Mobile = txtFatherMobile.Text;
                            father_rd.Occupation = txtFatherOccupation.Text;
                            father_rd.OrgAddress = txtFatherOrgAddress.Text;

                            father_rd.CreatedBy = cId;
                            father_rd.DateCreated = DateTime.Now;


                            DAL.Relation insertFather_r = new DAL.Relation();

                            insertFather_r.CandidateID = cId;
                            insertFather_r.RelationTypeID = 2;
                            insertFather_r.CreatedBy = cId;
                            insertFather_r.DateCreated = DateTime.Now;

                            using (var dbRelationDetailsInsert = new CandidateDataManager())
                            {
                                dbRelationDetailsInsert.Insert<DAL.RelationDetail>(father_rd);
                                insertFather_r.RelationDetailsID = father_rd.ID;
                            }
                            using (var dbRelationInsert = new CandidateDataManager())
                            {
                                dbRelationInsert.Insert<DAL.Relation>(insertFather_r);
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X006TC&type=danger", false);
                    lblMessageParent.Text = "Unable to save/update candidate information. Error Code : F01X006TC.";
                    messagePanel_Parent.CssClass = "alert alert-danger";
                    messagePanel_Parent.Visible = true;
                }

                #endregion

                #region MOTHER
                try
                {

                    using (var db = new CandidateDataManager())
                    {
                        DAL.Relation mother_r = db.GetAllRelationByCandidateID_ND(cId).Where(a => a.RelationTypeID == 3).FirstOrDefault();
                        if (mother_r != null || mother_r.ID > 0) // update relation
                        {
                            DAL.RelationDetail mother_rd = db.GetRelationDetailByID_ND(mother_r.RelationDetailsID);
                            if (mother_rd != null && mother_rd.ID > 0) // update relation details
                            {
                                mother_rd.Name = txtMotherName.Text;
                                mother_rd.Mobile = txtMotherMobile.Text;
                                mother_rd.Occupation = txtMotherOccupation.Text;
                                mother_rd.MailingAddress = txtMotherMailingAddress.Text;

                                mother_rd.DateModified = DateTime.Now;
                                mother_rd.ModifiedBy = cId;

                                using (var dbUpdate = new CandidateDataManager())
                                {
                                    dbUpdate.Update<DAL.RelationDetail>(mother_rd);
                                }
                            }
                        }
                        else // insert relation
                        {
                            DAL.RelationDetail mother_rd = new DAL.RelationDetail();

                            mother_rd.Name = txtMotherName.Text;
                            mother_rd.Mobile = txtMotherMobile.Text;
                            mother_rd.Occupation = txtMotherOccupation.Text;
                            mother_rd.MailingAddress = txtMotherMailingAddress.Text;

                            mother_rd.CreatedBy = cId;
                            mother_rd.DateCreated = DateTime.Now;


                            DAL.Relation insertMother_r = new DAL.Relation();

                            insertMother_r.CandidateID = cId;
                            insertMother_r.RelationTypeID = 3;
                            insertMother_r.CreatedBy = cId;
                            insertMother_r.DateCreated = DateTime.Now;

                            using (var dbRelationDetailsInsert = new CandidateDataManager())
                            {
                                dbRelationDetailsInsert.Insert<DAL.RelationDetail>(mother_rd);
                                insertMother_r.RelationDetailsID = mother_rd.ID;
                            }
                            using (var dbRelationInsert = new CandidateDataManager())
                            {
                                dbRelationInsert.Insert<DAL.Relation>(insertMother_r);
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X007TC&type=danger", false);
                    lblMessageParent.Text = "Unable to save/update candidate information. Error Code : F01X007TC.";
                    messagePanel_Parent.CssClass = "alert alert-danger";
                    messagePanel_Parent.Visible = true;
                }

                #endregion

                #region GUARDIAN
                try
                {

                    using (var db = new CandidateDataManager())
                    {
                        DAL.Relation guardian_r = db.GetAllRelationByCandidateID_ND(cId).Where(a => a.RelationTypeID == 9).FirstOrDefault();
                        if (guardian_r != null || guardian_r.ID > 0) // update relation
                        {
                            DAL.RelationDetail guardian_rd = db.GetRelationDetailByID_ND(guardian_r.RelationDetailsID);
                            if (guardian_rd != null && guardian_rd.ID > 0) // update relation details
                            {
                                guardian_rd.Name = txtGuardian_Name.Text;
                                guardian_rd.Mobile = txtGuardianMobile.Text;
                                guardian_rd.Occupation = null;
                                guardian_rd.MailingAddress = txtGuardianMailingAddress.Text;
                                if (ddlGuardianRelation.SelectedItem.Text == "Father")
                                {
                                    guardian_rd.RelationWithGuardian = "Father";
                                }
                                else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                                {
                                    guardian_rd.RelationWithGuardian = "Mother";
                                }
                                else if (ddlGuardianRelation.SelectedItem.Text == "Other")
                                {
                                    guardian_rd.RelationWithGuardian = txtGuardianOtherRelation.Text;
                                }
                                guardian_rd.Email = txtGuardianEmail.Text;
                                guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                                guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;

                                guardian_rd.DateModified = DateTime.Now;
                                guardian_rd.ModifiedBy = cId;

                                using (var dbUpdate = new CandidateDataManager())
                                {
                                    dbUpdate.Update<DAL.RelationDetail>(guardian_rd);
                                }
                            }
                        }
                        else // insert relation
                        {
                            DAL.RelationDetail guardian_rd = new DAL.RelationDetail();

                            guardian_rd.Name = txtGuardian_Name.Text;
                            guardian_rd.Mobile = txtGuardianMobile.Text;
                            guardian_rd.Occupation = null;
                            guardian_rd.MailingAddress = txtGuardianMailingAddress.Text;
                            if (ddlGuardianRelation.SelectedItem.Text == "Father")
                            {
                                guardian_rd.RelationWithGuardian = "Father";
                            }
                            else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                            {
                                guardian_rd.RelationWithGuardian = "Mother";
                            }
                            else if (ddlGuardianRelation.SelectedItem.Text == "Other")
                            {
                                guardian_rd.RelationWithGuardian = txtGuardianOtherRelation.Text;
                            }
                            guardian_rd.Email = txtGuardianEmail.Text;
                            guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                            guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;

                            guardian_rd.CreatedBy = cId;
                            guardian_rd.DateCreated = DateTime.Now;


                            DAL.Relation insertGuardian_r = new DAL.Relation();

                            insertGuardian_r.CandidateID = cId;
                            insertGuardian_r.RelationTypeID = 2;
                            insertGuardian_r.CreatedBy = cId;
                            insertGuardian_r.DateCreated = DateTime.Now;

                            using (var dbRelationDetailsInsert = new CandidateDataManager())
                            {
                                dbRelationDetailsInsert.Insert<DAL.RelationDetail>(guardian_rd);
                                insertGuardian_r.RelationDetailsID = guardian_rd.ID;
                            }
                            using (var dbRelationInsert = new CandidateDataManager())
                            {
                                dbRelationInsert.Insert<DAL.Relation>(insertGuardian_r);
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X008TC&type=danger", false);
                    lblMessageParent.Text = "Unable to save/update candidate information. Error Code : F01X008TC.";
                    messagePanel_Parent.CssClass = "alert alert-danger";
                    messagePanel_Parent.Visible = true;
                }

                #endregion

                lblMessageParent.Text = "Parent Info Updated successfully.";
                messagePanel_Parent.CssClass = "alert alert-success";
                messagePanel_Parent.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessageParent.Text = "Unable to save/update candidate information. Error Code : F01X108TC.";
                messagePanel_Parent.CssClass = "alert alert-danger";
                messagePanel_Parent.Visible = true;
            }

        }

        protected void btnSave_Address_Click(object sender, EventArgs e)
        {

            #region PRESENT
            try
            {
                using (var db = new CandidateDataManager())
                {
                    DAL.Address presAdd = db.GetAllAddressByCandidateID_ND(cId).Where(a => a.AddressTypeID == 2).FirstOrDefault();
                    if (presAdd != null)
                    {
                        DAL.AddressDetail presAddDtl = db.GetAddressDetailByID_ND(presAdd.AddressDetailsID);
                        if (presAddDtl != null)
                        {
                            presAddDtl.AddressLine = txtPresentAddress.Text;
                            presAddDtl.DistrictID = Int32.Parse(ddlPresentDistrict.SelectedValue);
                            presAddDtl.DivisionID = Int32.Parse(ddlPresentDivision.SelectedValue);
                            presAddDtl.Upazila = txtPresentUpazila.Text;

                            presAddDtl.ModifiedBy = cId;
                            presAddDtl.DateModified = DateTime.Now;

                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.AddressDetail>(presAddDtl);
                            }
                        }
                    }
                    else
                    {
                        DAL.AddressDetail newAddDtl = new DAL.AddressDetail();

                        newAddDtl.AddressLine = txtPresentAddress.Text;
                        newAddDtl.DistrictID = Int32.Parse(ddlPresentDistrict.SelectedValue);
                        newAddDtl.DivisionID = Int32.Parse(ddlPresentDivision.SelectedValue);
                        newAddDtl.Upazila = txtPresentUpazila.Text;

                        newAddDtl.CreatedBy = cId;
                        newAddDtl.DateCreated = DateTime.Now;

                        DAL.Address newAddress = new DAL.Address();

                        newAddress.CandidateID = cId;
                        newAddress.AddressTypeID = 2; //present

                        using (var dbInsAddDtl = new CandidateDataManager())
                        {
                            dbInsAddDtl.Insert<DAL.AddressDetail>(newAddDtl);
                            newAddress.AddressDetailsID = newAddDtl.ID;
                        }
                        using (var dbInsAdd = new CandidateDataManager())
                        {
                            dbInsAdd.Insert<DAL.Address>(newAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X009TC&type=danger", false);
            }
            #endregion

            #region PERMANENT
            try
            {
                using (var db = new CandidateDataManager())
                {
                    DAL.Address permAdd = db.GetAllAddressByCandidateID_ND(cId).Where(a => a.AddressTypeID == 3).FirstOrDefault();
                    if (permAdd != null)
                    {
                        DAL.AddressDetail permAddDtl = db.GetAddressDetailByID_ND(permAdd.AddressDetailsID);
                        if (permAddDtl != null)
                        {
                            permAddDtl.AddressLine = txtPermanentAddress.Text;
                            permAddDtl.DistrictID = Int32.Parse(ddlPermanentDistrict.SelectedValue);
                            permAddDtl.DivisionID = Int32.Parse(ddlPermanentDivision.SelectedValue);
                            permAddDtl.Upazila = txtPermanentUpazila.Text;

                            permAddDtl.ModifiedBy = cId;
                            permAddDtl.DateModified = DateTime.Now;

                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.AddressDetail>(permAddDtl);
                            }
                        }
                    }
                    else
                    {
                        DAL.AddressDetail newAddDtl = new DAL.AddressDetail();

                        newAddDtl.AddressLine = txtPermanentAddress.Text;
                        newAddDtl.DistrictID = Int32.Parse(ddlPermanentDistrict.SelectedValue);
                        newAddDtl.DivisionID = Int32.Parse(ddlPermanentDivision.SelectedValue);
                        newAddDtl.Upazila = txtPermanentUpazila.Text;

                        newAddDtl.CreatedBy = cId;
                        newAddDtl.DateCreated = DateTime.Now;

                        DAL.Address newAddress = new DAL.Address();

                        newAddress.CandidateID = cId;
                        newAddress.AddressTypeID = 3; //permanent

                        using (var dbInsAddDtl = new CandidateDataManager())
                        {
                            dbInsAddDtl.Insert<DAL.AddressDetail>(newAddDtl);
                            newAddress.AddressDetailsID = newAddDtl.ID;
                        }
                        using (var dbInsAdd = new CandidateDataManager())
                        {
                            dbInsAdd.Insert<DAL.Address>(newAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X010TC&type=danger", false);
            }
            #endregion

            lblMessageAddress.Text = "Address Info Updated successfully.";
            messagePanel_Address.CssClass = "alert alert-success";
            messagePanel_Address.Visible = true;
        }

        protected void btnSave_Guarantor_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    DAL.Relation fg_relation = db.GetAllRelationByCandidateID_ND(cId).Where(a => a.RelationTypeID == 8).FirstOrDefault();
                    if (fg_relation != null || fg_relation.ID > 0)
                    {
                        DAL.RelationDetail fg_relDetail = db.GetRelationDetailByID_ND(fg_relation.RelationDetailsID);
                        if (fg_relDetail != null)
                        {
                            fg_relDetail.Name = txtFinGuarantorName.Text;
                            fg_relDetail.RelationWithGuardian = "Financial Guarantor";
                            fg_relDetail.Organization = txtFinGuarantorOrganization.Text;
                            fg_relDetail.Designation = txtFinGuarantorOccupation.Text;
                            fg_relDetail.MailingAddress = txtFinGuarantorAddress.Text;
                            fg_relDetail.Email = txtFinGuarantorEmail.Text;
                            fg_relDetail.PhoneResidence = txtFinGuarantorPhoneRes.Text;
                            fg_relDetail.PhoneOffice = txtFinGuarantorPhoneOffice.Text;

                            fg_relDetail.DateModified = DateTime.Now;
                            fg_relDetail.ModifiedBy = cId;

                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.RelationDetail>(fg_relDetail);
                            }
                        }
                    }
                    else
                    {
                        DAL.RelationDetail newFinGuar = new DAL.RelationDetail();

                        newFinGuar.Name = txtFinGuarantorName.Text;
                        newFinGuar.RelationWithGuardian = "Financial Guarantor";
                        newFinGuar.Organization = txtFinGuarantorOrganization.Text;
                        newFinGuar.Designation = txtFinGuarantorOccupation.Text;
                        newFinGuar.MailingAddress = txtFinGuarantorAddress.Text;
                        newFinGuar.Email = txtFinGuarantorEmail.Text;
                        newFinGuar.PhoneResidence = txtFinGuarantorPhoneRes.Text;
                        newFinGuar.PhoneOffice = txtFinGuarantorPhoneOffice.Text;
                        newFinGuar.CreatedBy = cId;
                        newFinGuar.DateCreated = DateTime.Now;

                        DAL.Relation newFgRel = new DAL.Relation();

                        newFgRel.CandidateID = cId;
                        newFgRel.RelationTypeID = 8;//financial guarantor
                        newFgRel.CreatedBy = cId;
                        newFgRel.DateCreated = DateTime.Now;

                        using (var dbInsertRD = new CandidateDataManager())
                        {
                            dbInsertRD.Insert<DAL.RelationDetail>(newFinGuar);
                            newFgRel.RelationDetailsID = newFinGuar.ID;
                        }
                        using (var dbInsertR = new CandidateDataManager())
                        {
                            dbInsertR.Insert<DAL.Relation>(newFgRel);
                        }
                    }
                }

                lblMessageFinGuar.Text = "Financial Guarantor Info Updated successfully.";
                messagePanel_FinGuar.CssClass = "alert alert-success";
                messagePanel_FinGuar.Visible = true;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X011TC&type=danger", false);
            }
        }

        protected void btnSave_Additional_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    DAL.AdditionalInfo addInfoObj = db.GetAdditionalInfoByCandidateID_ND(cId);
                    if (addInfoObj != null || addInfoObj.ID > 0)
                    {
                        if (ddlAdmittedBefore.SelectedValue == "Yes")
                        {
                            addInfoObj.CurrentStudentId = txtCurrentStudentId.Text.Trim();
                        }
                        else if (ddlAdmittedBefore.SelectedValue == "No")
                        {
                            addInfoObj.CurrentStudentId = null;
                        }

                        if (ddlDismissedBefore.SelectedValue == "Yes")
                        {
                            addInfoObj.IsDismissedBefore = true;
                            txtDismissalStatement.Text = txtDismissalStatement.Text.Trim();
                        }
                        else if (ddlDismissedBefore.SelectedValue == "No")
                        {
                            addInfoObj.IsDismissedBefore = false;
                            txtDismissalStatement.Text = null;
                        }

                        addInfoObj.DateModified = DateTime.Now;
                        addInfoObj.ModifiedBy = cId;

                        using (var dbUpdate = new CandidateDataManager())
                        {
                            dbUpdate.Update<DAL.AdditionalInfo>(addInfoObj);
                        }
                    }
                    else
                    {
                        DAL.AdditionalInfo _newAddInfo = new DAL.AdditionalInfo();

                        if (ddlAdmittedBefore.SelectedValue == "Yes")
                        {
                            _newAddInfo.CurrentStudentId = txtCurrentStudentId.Text.Trim();
                        }
                        else if (ddlAdmittedBefore.SelectedValue == "No")
                        {
                            _newAddInfo.CurrentStudentId = null;
                        }
                        else
                        {
                            _newAddInfo.CurrentStudentId = null;
                        }

                        if (ddlDismissedBefore.SelectedValue == "Yes")
                        {
                            _newAddInfo.IsDismissedBefore = true;
                            _newAddInfo.DismissalStatement = txtDismissalStatement.Text.Trim();
                        }
                        else if (ddlDismissedBefore.SelectedValue == "No")
                        {
                            _newAddInfo.IsDismissedBefore = false;
                            _newAddInfo.DismissalStatement = null;
                        }

                        _newAddInfo.CreatedBy = cId;
                        _newAddInfo.DateCreated = DateTime.Now;

                        using (var dbInsert = new CandidateDataManager())
                        {
                            dbInsert.Insert<DAL.AdditionalInfo>(_newAddInfo);
                        }
                    }
                }

                lblMessageAdditional.Text = "Financial Guarantor Info Updated successfully.";
                messagePanel_Additional.CssClass = "alert alert-success";
                messagePanel_Additional.Visible = true;

            }
            catch (Exception ex)
            {
                Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X012TC&type=danger", false);
            }
        }
    }
}