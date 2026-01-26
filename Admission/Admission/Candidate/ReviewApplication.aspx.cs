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
    public partial class ReviewApplication : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                LoadCandidateData(uId);
            }

        }

        private void LoadCandidateData(long uId)
        {
            long cId = -1;

            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
            }

            if (cId > 0)
            {
                List<DAL.Document> documentList = null;
                DAL.BasicInfo cand = null;
                //Priority
                List<DAL.Exam> exam = null;
                List<DAL.Address> address = null;
                List<DAL.Relation> relation = null;
                DAL.FinancialGuarantorInfo fgInfo = null;

                #region ATTACHMENT

                DAL.DocumentDetail photo = null;
                DAL.DocumentDetail sign = null;
                DAL.DocumentDetail fgSign = null;

                using (var db = new CandidateDataManager())
                {
                    documentList = db.GetAllDocumentByCandidateID_AD(cId);
                }

                if (documentList != null)
                {
                    photo = documentList.Where(c => c.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                    sign = documentList.Where(c => c.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();
                    fgSign = documentList.Where(c => c.DocumentTypeID == 7).Select(c => c.DocumentDetail).FirstOrDefault();
                }

                if (photo != null)
                {
                    ImagePhoto.ImageUrl = photo.URL;
                }

                if (sign != null)
                {
                    ImageSignature.ImageUrl = sign.URL;
                }

                if (fgSign != null)
                {
                    FGSignature.ImageUrl = fgSign.URL;
                }

                #endregion

                #region BASIC INFO

                DAL.AdmissionSetup admSetup = null;

                using (var db = new CandidateDataManager())
                {
                    cand = db.GetCandidateBasicInfoByID_AD(cId);
                    if (cand != null)
                    {
                        lblFirstName.Text = cand.FirstName;
                        lblDateOfBirth.Text = cand.DateOfBirth.ToString("dd/MM/yyyy");
                        //lblPlaceOfBirth.Text = cand.PlaceOfBirth;
                        
                        //lblNationality.Text = cand.Country.Name;
                        //if (cand.NationalityID == null) { lblNationality.Text = null; }
                        //else { lblNationality.Text = cand.Country.Name; }
                        
                        //lblLanguage.Text = cand.Language.LanguageName;
                        //if(cand.MotherTongueID == null) { lblLanguage.Text = null; }
                        //else { lblLanguage.Text = cand.Language.LanguageName; }

                        //lblGender.Text = cand.Gender.GenderName;
                        if(cand.GenderID == null) { lblGender.Text = null; }
                        else { lblGender.Text = cand.Gender.GenderName; }

                        if (admSetup != null)
                        {
                            if (admSetup.EducationCategoryID == 6)
                            {
                                tdMarital.Visible = true;
                                tdMaritallbl.Visible = true;
                            }
                            else
                            {
                                tdMarital.Visible = false;
                                tdMaritallbl.Visible = false;
                            }
                        }

                        //lblMaritalStatus.Text = cand.MaritalStatu.MaritalStatus;
                        if (cand.MaritalStatusID == null) { lblMaritalStatus.Text = null; }
                        else { lblMaritalStatus.Text = cand.MaritalStatu.MaritalStatus; }
                        
                        lblNationalId.Text = cand.NationalIdNumber;
                        lblBirthRegNo.Text = cand.BirthRegistrationNo;
                        
                        //lblBloodGroup.Text = cand.BloodGroup.BloodGroupName;
                        if(cand.BloodGroupID == null) { lblBloodGroup.Text = null; }
                        else { lblBloodGroup.Text = cand.BloodGroup.BloodGroupName; }

                        lblEmail.Text = cand.Email;
                        lblMobile.Text = cand.Mobile;

                        if (cand.ReligionID == null) { lblReligion.Text = null; }
                        else { lblReligion.Text = cand.Religion.ReligionName; }

                        if (cand.QuotaID == null) { lblQuota.Text = null; }
                        else { lblQuota.Text = cand.Quota.Remarks; }
                        int EducationCatID = db.GetCandidateEducationCategoryID(cId);
                        bool showProgramPriority = EducationCatID != 6;

                        if (showProgramPriority)
                        {
                            lblHall.Text = cand.AttributeBool == null ? "No" : Convert.ToBoolean(cand.AttributeBool) ? "Yes" : "No";
                            phHall.Visible = true;
                        }
                        else
                        {
                            phHall.Visible = false;
                        }

                    }
                }

                #endregion

                #region EDUCATION

                DAL.CandidateFormSl cFormSl = null;
                using (var db = new CandidateDataManager())
                {
                    cFormSl = db.GetCandidateFormSlByCandID_ND(cId);
                }
                if (cFormSl != null)
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.GetAdmissionSetupByID_ND(cFormSl.AdmissionSetupID);
                    }
                }

                if (admSetup != null)
                {
                    if (admSetup.EducationCategoryID == 6)
                    {
                        panel_ForMasters.Visible = true;
                    }
                    else
                    {
                        panel_ForMasters.Visible = false;
                    }
                }

                DAL.ExamDetail ssc = null;
                DAL.ExamDetail hsc = null;
                DAL.ExamDetail undgrad = null;
                DAL.ExamDetail grad = null;

                using (var db = new CandidateDataManager())
                {
                    exam = db.GetAllExamByCandidateID_AD(cId);
                    if (exam != null)
                    {

                        ssc = exam.Where(a => a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6).Select(a => a.ExamDetail).FirstOrDefault();
                        hsc = exam.Where(a => a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9).Select(a => a.ExamDetail).FirstOrDefault();
                        undgrad = exam.Where(a => a.ExamTypeID == 3 || a.ExamTypeID == 10).Select(a => a.ExamDetail).FirstOrDefault();
                        grad = exam.Where(a => a.ExamTypeID == 4).Select(a => a.ExamDetail).FirstOrDefault();
                    }

                    #region SSC/OLevel

                    if (ssc != null)
                    {
                        lblSec_ExamType.Text = exam.Where(a => a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6).Select(a => a.ExamType.ExamTypeName).FirstOrDefault();
                        lblSec_EducationBrd.Text = ssc.EducationBoard.BoardName;
                        lblSec_Institute.Text = ssc.Institute;
                        lblSec_RollNo.Text = ssc.RollNo;
                        lblSec_RegNo.Text = ssc.RegistrationNo;
                        lblSec_GrpOrSub.Text = ssc.GroupOrSubject.GroupOrSubjectName;
                        lblSec_DivClass.Text = ssc.ResultDivision.ResultDivisionName;
                        lblSec_GPA.Text = ssc.GPA.ToString();
                        //lblSec_CgpaScore.Text = ssc.CGPA.ToString();
                        //lblSec_Marks.Text = ssc.Marks.ToString();
                        lblSec_PassingYear.Text = ssc.PassingYear.ToString();
                    }

                    #endregion

                    #region HSC/ALevel

                    if (hsc != null)
                    {
                        lblHighSec_ExamType.Text = exam.Where(a => a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9).Select(a => a.ExamType.ExamTypeName).FirstOrDefault();
                        lblHighSec_EducationBrd.Text = hsc.EducationBoard.BoardName;
                        lblHighSec_Institute.Text = hsc.Institute;
                        lblHighSec_RollNo.Text = hsc.RollNo;
                        lblHighSec_RegNo.Text = hsc.RegistrationNo;
                        lblHighSec_GrpOrSub.Text = hsc.GroupOrSubject.GroupOrSubjectName;
                        lblHighSec_DivClass.Text = hsc.ResultDivision.ResultDivisionName;
                        lblHighSec_GPA.Text = hsc.GPA.ToString();
                        //lblHighSec_CgpaScore.Text = hsc.CGPA.ToString();
                        //lblHighSec_Marks.Text = hsc.Marks.ToString();
                        lblHighSec_PassingYear.Text = hsc.PassingYear.ToString();
                    }

                    #endregion

                    #region UNDERGRAD

                    if (undgrad != null && admSetup.EducationCategoryID == 6)
                    {
                        //lblUndergrad_ExamType.Text = exam.Where(a => a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9)
                        //    .Select(a => a.ExamType.ExamTypeName).FirstOrDefault();
                        //lblUndergrad_EducationBrd.Text = undgrad.EducationBoard.BoardName;
                        lblUndergrad_Institute.Text = undgrad.Institute;
                        //lblUndergrad_RollNo.Text = undgrad.RollNo;
                        //lblUndergrad_RegNo.Text = undgrad.RegistrationNo;
                        //lblUndergrad_GrpOrSub.Text = undgrad.GroupOrSubject.GroupOrSubjectName;
                        lblUndergrad_DivClass.Text = undgrad.ResultDivision.ResultDivisionName;
                        lblUndergrad_CgpaScore.Text = undgrad.CGPA.ToString();
                        //lblUndergrad_Marks.Text = undgrad.Marks.ToString();

                        //TODO: Change db ******************************************
                        //lblUndergrad_Grade.Text = undgrad.Grade;
                        lblUndergrad_PassingYear.Text = undgrad.PassingYear.ToString();
                    }

                    #endregion

                    #region GRAD

                    if (grad != null && admSetup.EducationCategoryID == 6)
                    {
                        //lblGrad_ExamType.Text = exam.Where(a => a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9)
                        //    .Select(a => a.ExamType.ExamTypeName).FirstOrDefault();
                        //lblGrad_EducationBrd.Text = grad.EducationBoard.BoardName;
                        lblGrad_Institute.Text = grad.Institute;
                        //lblGrad_RollNo.Text = grad.RollNo;
                        //lblGrad_RegNo.Text = grad.RegistrationNo;
                        //lblGrad_GrpOrSub.Text = grad.GroupOrSubject.GroupOrSubjectName;
                        lblGrad_DivClass.Text = grad.ResultDivision.ResultDivisionName;
                        lblGrad_CgpaScore.Text = grad.CGPA.ToString();
                        //lblGrad_Marks.Text = grad.Marks.ToString();

                        //TODO: Change db ******************************************
                        //lblGrad_Grade.Text = grad.Grade;
                        lblGrad_PassingYear.Text = grad.PassingYear.ToString();
                    }

                    #endregion
                }

                #endregion

                #region ADDRESS

                DAL.AddressDetail pres = null;
                DAL.AddressDetail per = null;

                using (var db = new CandidateDataManager())
                {
                    address = db.GetAllAddressByCandidateID_AD(cId);
                    if (address != null)
                    {
                        pres = address.Where(c => c.AddressTypeID == 2).Select(c => c.AddressDetail).FirstOrDefault();
                        per = address.Where(c => c.AddressTypeID == 3).Select(c => c.AddressDetail).FirstOrDefault();
                    }

                    #region PRESENT

                    if (pres != null)
                    {
                        lblPresentAddress.Text = pres.AddressLine;
                        lblPresentDivision.Text = pres.Division.Name;
                        lblPresentDistrict.Text = pres.District.Name;
                        lblPresentUpozela.Text = pres.Upazila.ToString();
                        lblPresentPostalCode.Text = pres.PostCode;
                        lblPresentCountry.Text = pres.Country.Name;
                        //lblPresentTelephone.Text = pres.Mobile;

                    }

                    #endregion

                    #region PERMANENT

                    if (per != null)
                    {
                        lblPermanentAddress.Text = per.AddressLine;
                        lblPermanentDivision.Text = per.Division.Name;
                        lblPermanentDistrict.Text = per.District.Name;
                        lblPermanentUpozela.Text = per.Upazila.ToString();
                        lblPermanentPostalCode.Text = per.PostCode;
                        lblPermanentCountry.Text = per.Country.Name;
                        //lblPermanentTelephone.Text = per.Mobile;
                    }

                    #endregion
                }

                #endregion



                #region PARENT/GUARDIAN

                DAL.RelationDetail father = null;
                DAL.RelationDetail mother = null;
                DAL.RelationDetail guardian = null;
                DAL.RelationDetail spouse = null;

                using (var db = new CandidateDataManager())
                {
                    relation = db.GetAllRelationByCandidateID_AD(cId);
                    if (relation != null)
                    {
                        father = relation.Where(c => c.RelationTypeID == 2).Select(c => c.RelationDetail).FirstOrDefault();
                        mother = relation.Where(c => c.RelationTypeID == 3).Select(c => c.RelationDetail).FirstOrDefault();
                        guardian = relation.Where(c => c.RelationTypeID == 5).Select(c => c.RelationDetail).FirstOrDefault();
                        spouse = relation.Where(c => c.RelationTypeID == 7).Select(c => c.RelationDetail).FirstOrDefault();
                    }

                    #region PARENT

                    if (father != null)
                    {
                        lblFatherName.Text = father.Name;
                        lblFatherOccupation.Text = father.Attribute1;
                        lblFatherMobile.Text = father.Mobile;
                    }

                    if (mother != null)
                    {
                        lblMotherName.Text = mother.Name;
                        lblMotherOccupation.Text = mother.Attribute1;
                        lblMotherMobile.Text = mother.Mobile;
                    }

                    #endregion

                    #region GUARDIAN

                    if (guardian != null)
                    {
                        lblGuardian_Name.Text = guardian.Name;
                        lblGuardianOtherRelation.Text = guardian.RelationWithGuardian;
                        lblGuardianOccupation.Text = guardian.Attribute1;
                        lblGuardianMailingAddress.Text = guardian.MailingAddress;
                        lblGuardianMobile.Text = guardian.Mobile;
                    }

                    #endregion

                    #region SPOUSE

                    if (spouse != null)
                    {

                    }

                    #endregion
                }
                #endregion

                #region FINANCIAL GUARANTOR

                using (var db = new CandidateDataManager())
                {
                    fgInfo = db.GetFinancialGuarantorByCandidateID_ND(cId);

                    if (fgInfo != null)
                    {
                        //lblFinGuarantorName.Text = fgInfo.Name;
                        //lblRelationWithGuarantor.Text = fgInfo.RelationWithCandidate;
                        //lblFinGuarantorOccupation.Text = fgInfo.Occupation;
                        //lblFinGuarantorOrganization.Text = fgInfo.Organization;
                        //lblFinGuarantorPosition.Text = fgInfo.Designation;
                        //lblFinGuarantorAddress.Text = fgInfo.MailingAddress;
                        //lblFinGuarantorEmail.Text = fgInfo.Email;
                        //lblFinGuarantorMobile.Text = fgInfo.Mobile;
                        //lblFinGuarantorSourceFund.Text = fgInfo.SourceOfFund;
                    }
                }

                #endregion

                #region Program Priority

                try
                {
                    List<DAL.SPGetCandidateProgramPriorities_Result> list = null;

                    using (var db = new CandidateDataManager())
                    {
                        list = db.AdmissionDB.SPGetCandidateProgramPriorities(cId).ToList();
                    }

                    bool showProgramPriority = admSetup?.EducationCategoryID != 6;
                    panel_ProgramPriority.Visible = showProgramPriority;

                    if (!showProgramPriority) return;

                    if (list != null && list.Count > 0)
                    {
                        var orderedList = list.OrderBy(c => c.cP_Priority)
                                             .ThenBy(c => c.admUnit_Name)
                                             .ToList();

                        rptProgramPriority.DataSource = orderedList;
                        rptProgramPriority.DataBind();
                        lblNoData.Visible = false;
                    }
                    else
                    {
                        rptProgramPriority.DataSource = null;
                        rptProgramPriority.DataBind();
                        lblNoData.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    panel_ProgramPriority.Visible = false;
                    return;
                }

                #endregion
            }
        }
    }
}