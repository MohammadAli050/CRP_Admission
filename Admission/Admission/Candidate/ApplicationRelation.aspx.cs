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
    public partial class ApplicationRelation : PageBase
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
                if (uId > 0)
                {
                    LoadDDL();
                    LoadCandidateData(uId);
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
                DDLHelper.Bind<DAL.RelationType>(ddlGuardianRelation, db.AdmissionDB.RelationTypes.Where(a => a.IsActive == true && a.RelationTypeName != "Financial Guarantor").ToList(), "RelationTypeName", "ID", EnumCollection.ListItemType.Select);

                List<DAL.Country> countryList = db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList();
                if (countryList.Count() > 0)
                {
                    countryList = countryList.Where(x => x.ID == 2).ToList(); // Only Bangladeshi Nationality

                    foreach (var item in countryList)
                    {
                        item.Name = "BANGLADESHI";
                    }

                    DDLHelper.Bind<DAL.Country>(ddlFatherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    DDLHelper.Bind<DAL.Country>(ddlMotherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    //DDLHelper.Bind<DAL.Country>(ddlMotherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    DDLHelper.Bind<DAL.Country>(ddlGuardianNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);

                    #region Auto Select
                    ddlFatherNationality.SelectedValue = "2";
                    ddlMotherNationality.SelectedValue = "2";
                    ddlGuardianNationality.SelectedValue = "2";

                    #endregion
                }

                ddlIsLateFather.Items.Clear();
                ddlIsLateFather.Items.Add(new ListItem("Select", "-1"));
                ddlIsLateFather.Items.Add(new ListItem("No", "0"));
                ddlIsLateFather.Items.Add(new ListItem("Yes", "1"));

                ddlIsLateMother.Items.Clear();
                ddlIsLateMother.Items.Add(new ListItem("Select", "-1"));
                ddlIsLateMother.Items.Add(new ListItem("No", "0"));
                ddlIsLateMother.Items.Add(new ListItem("Yes", "1"));

                //ddlFatherOccupationType.Items.Clear();
                //ddlFatherOccupationType.Items.Add(new ListItem("Select", "-1"));
                //ddlFatherOccupationType.Items.Add(new ListItem("Government", "Government"));
                //ddlFatherOccupationType.Items.Add(new ListItem("Business", "Business"));
                //ddlFatherOccupationType.Items.Add(new ListItem("Private", "Private"));
                //ddlFatherOccupationType.Items.Add(new ListItem("Not Applicable", "Not Applicable"));

                var requiredOccupationIDs = new List<int> { 2, 7, 8, 9, 10 };
                var requiredMotherOccupationIDs = new List<int> { 2, 6, 7, 8, 9, 10 };

                var fatherOccupations = db.AdmissionDB.RelationOccupationTypes
                    .Where(c => c.IsActive == true && c.IsFather == true && requiredOccupationIDs.Contains(c.ID))
                    .OrderBy(c => c.OccupationTypeName)
                    .ToList();

                DDLHelper.Bind<DAL.RelationOccupationType>(ddlFatherOccupationType, fatherOccupations, "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);

                // Filter Mother's Occupation Types with specific IDs
                var motherOccupations = db.AdmissionDB.RelationOccupationTypes
                    .Where(c => c.IsActive == true && c.IsMother == true && requiredMotherOccupationIDs.Contains(c.ID))
                    .OrderBy(c => c.OccupationTypeName)
                    .ToList();

                DDLHelper.Bind<DAL.RelationOccupationType>(ddlMotherOccupationType, motherOccupations, "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);

                var requiredTypeOccupationIds = new List<int> { 1, 3 };

                var fatherOccupationsType = db.AdmissionDB.RelationOccupationTypes
                   .Where(c => c.IsActive == true && c.IsFather == true && requiredTypeOccupationIds.Contains(c.ID))
                   .OrderBy(c => c.OccupationTypeName)
                   .ToList();

                DDLHelper.Bind<DAL.RelationOccupationType>(ddlFatherServiceType, fatherOccupationsType, "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);

                // Filter Mother's Occupation Types with specific IDs
                var motherOccupationsType = db.AdmissionDB.RelationOccupationTypes
                    .Where(c => c.IsActive == true && c.IsMother == true && requiredTypeOccupationIds.Contains(c.ID))
                    .OrderBy(c => c.OccupationTypeName)
                    .ToList();

                DDLHelper.Bind<DAL.RelationOccupationType>(ddlMotherServiceType, motherOccupationsType, "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);

                //DDLHelper.Bind<DAL.RelationOccupationType>(ddlFatherOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsFather == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.RelationOccupationType>(ddlMotherOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsMother == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.RelationOccupationType>(ddlGuardianOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsGuardian == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);


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
                        if (educationCategoryId == 4)
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
                                        btnSave_Parent.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Parent.Visible = false;
                                    }

                                }
                                else
                                {
                                    btnSave_Parent.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Parent.Visible = false;
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
                                        btnSave_Parent.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Parent.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnSave_Parent.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Parent.Visible = false;
                            }
                            #endregion

                            #endregion
                        }
                    }
                    else
                    {
                        btnSave_Parent.Visible = false;
                    }
                    #endregion

                    #region Breadcrumbs for Bachelor and Masters
                    if (educationCategoryId == 4)
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

                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.Relation> relationList = db.GetAllRelationByCandidateID_ND(cId);
                        if (relationList != null)
                        {
                            DAL.Relation fatherRelation = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();
                            DAL.Relation motherRelation = relationList.Where(a => a.RelationTypeID == 3).FirstOrDefault();
                            DAL.Relation guardianRelation = relationList.Where(a => a.RelationTypeID == 5).FirstOrDefault();
                            //DAL.Relation spouseRelation = relationList.Where(a => a.RelationTypeID == 7).FirstOrDefault();

                            DAL.Exam secondaryExam = db.GetSecondaryExamByCandidateID_AD(cId);
                            DAL.Exam higherSecondaryExam = db.GetHigherSecdExamByCandidateID_AD(cId);


                            #region FATHER RELATION
                            if (fatherRelation != null)
                            {
                                DAL.RelationDetail father = db.GetRelationDetailByID_ND(fatherRelation.RelationDetailsID);
                                if (father != null)
                                {
                                    if (father.IsLate == null)
                                    {
                                        ddlIsLateFather.SelectedValue = "-1";
                                        txtFatherDesignation.Enabled = true;
                                        txtFatherEmail.Enabled = true;
                                        txtFatherMobile.Enabled = true;
                                        txtFatherNationalId.Enabled = true;
                                        txtFatherOccupation.Enabled = true;
                                        ddlFatherOccupationType.Enabled = true;
                                        txtFatherOrganization.Enabled = true;
                                    }
                                    else if (father.IsLate == false)
                                    {
                                        ddlIsLateFather.SelectedValue = "0";
                                        txtFatherDesignation.Enabled = true;
                                        txtFatherEmail.Enabled = true;
                                        txtFatherMobile.Enabled = true;
                                        txtFatherNationalId.Enabled = true;
                                        txtFatherOccupation.Enabled = true;
                                        ddlFatherOccupationType.Enabled = true;
                                        txtFatherOrganization.Enabled = true;
                                    }
                                    else if (father.IsLate == true)
                                    {
                                        ddlIsLateFather.SelectedValue = "1";
                                        txtFatherDesignation.Enabled = false;
                                        txtFatherEmail.Enabled = false;
                                        txtFatherMobile.Enabled = false;
                                        txtFatherNationalId.Enabled = false;
                                        txtFatherOccupation.Enabled = false;
                                        ddlFatherOccupationType.Enabled = false;
                                        txtFatherOrganization.Enabled = false;
                                    }

                                    txtFatherName.Text = father.Name;
                                    txtFatherMobile.Text = father.Mobile;
                                    txtFatherOccupation.Text = father.Occupation;
                                    //txtFatherOrgAddress.Text = father.OrgAddress;
                                    txtFatherOrganization.Text = father.Organization;
                                    txtFatherDesignation.Text = father.Designation;
                                    txtFatherEmail.Text = father.Email != null ? father.Email.ToLower() : "";
                                    txtFatherNationalId.Text = father.NationalIdNumber;
                                    ddlFatherNationality.SelectedValue = father.NationalityID.ToString();
                                    txtFatherPreviousService.Text = father.Attribute3;
                                    try
                                    {
                                        if (father.Attribute1 == null)
                                        {
                                            ddlFatherOccupationType.SelectedValue = "-1";
                                        }
                                        else
                                        {
                                            if (father.Attribute1 == "Government" || father.Attribute1 == "Private")
                                            {
                                                ddlFatherOccupationType.Items.FindByText("Service").Selected = true;
                                                ddlFatherServiceType.Items.FindByText(father.Attribute1).Selected = true;
                                                fatherService.Visible = true;
                                                fatherOrganization.Visible = true;
                                                fatherDesignation.Visible = true;
                                                fatherServiceTxt.Visible = true;
                                            }
                                            else
                                            {
                                                ddlFatherOccupationType.Items.FindByText(father.Attribute1).Selected = true;
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {

                                    }


                                }
                            }
                            #endregion
                            #region MOTHER RELATION
                            if (motherRelation != null)
                            {
                                DAL.RelationDetail mother = db.GetRelationDetailByID_ND(motherRelation.RelationDetailsID);
                                if (mother != null)
                                {
                                    if (mother.IsLate == null)
                                    {
                                        ddlIsLateMother.SelectedValue = "-1";
                                        txtMotherDesignation.Enabled = true;
                                        //txtMotherEmail.Enabled = true;
                                        txtMotherMobile.Enabled = true;
                                        txtMotherNationalId.Enabled = true;
                                        txtMotherOccupation.Enabled = true;
                                        ddlMotherOccupationType.Enabled = true;
                                        txtMotherOrganization.Enabled = true;
                                    }
                                    else if (mother.IsLate == false)
                                    {
                                        ddlIsLateMother.SelectedValue = "0";
                                        txtMotherDesignation.Enabled = true;
                                        //txtMotherEmail.Enabled = true;
                                        txtMotherMobile.Enabled = true;
                                        txtMotherNationalId.Enabled = true;
                                        txtMotherOccupation.Enabled = true;
                                        ddlMotherOccupationType.Enabled = true;
                                        txtMotherOrganization.Enabled = true;
                                    }
                                    else if (mother.IsLate == true)
                                    {
                                        ddlIsLateMother.SelectedValue = "1";
                                        txtMotherDesignation.Enabled = false;
                                        //txtMotherEmail.Enabled = false;
                                        txtMotherMobile.Enabled = false;
                                        txtMotherNationalId.Enabled = false;
                                        txtMotherOccupation.Enabled = false;
                                        ddlMotherOccupationType.Enabled = false;
                                        txtMotherOrganization.Enabled = false;
                                    }

                                    txtMotherName.Text = mother.Name;
                                    txtMotherMobile.Text = mother.Mobile;
                                    txtMotherOccupation.Text = mother.Occupation;
                                    //txtMotherMailingAddress.Text = mother.OrgAddress;
                                    txtMotherOrganization.Text = mother.Organization;
                                    txtMotherDesignation.Text = mother.Designation;
                                    txtMotherNationalId.Text = mother.NationalIdNumber;
                                    ddlMotherNationality.SelectedValue = mother.NationalityID.ToString();
                                    try
                                    {
                                        if (mother.Attribute1 == null)
                                        {
                                            ddlMotherOccupationType.SelectedValue = "-1";
                                        }
                                        else
                                        {
                                            if (mother.Attribute1 == "Government" || mother.Attribute1 == "Private")
                                            {
                                                ddlMotherOccupationType.Items.FindByText("Service").Selected = true;
                                                ddlMotherServiceType.Items.FindByText(mother.Attribute1).Selected = true;
                                                motherService.Visible = true;
                                                motherOrganization.Visible = true;
                                                motherDesignation.Visible = true;
                                                motherServiceTxt.Visible = true;
                                            }
                                            else
                                            {
                                                ddlMotherOccupationType.Items.FindByText(mother.Attribute1).Selected = true;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            #endregion
                            #region GUARDIAN RELATION
                            if (guardianRelation != null)
                            {
                                DAL.RelationDetail guardian = db.GetRelationDetailByID_ND(guardianRelation.RelationDetailsID);
                                if (guardian != null)
                                {
                                    txtGuardian_Name.Text = guardian.Name;
                                    txtGuardianMailingAddress.Text = guardian.MailingAddress;
                                    txtGuardianMobile.Text = guardian.Mobile;
                                    txtGuardianOccupation.Text = guardian.Occupation;

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
                                        ddlGuardianRelation.SelectedValue = "4";
                                        txtGuardianOtherRelation.Text = guardian.RelationWithGuardian;
                                        txtGuardianOtherRelation.Enabled = true;
                                    }
                                    txtGuardianEmail.Text = guardian.Email;
                                    txtGuardianNationalId.Text = guardian.NationalIdNumber;
                                    ddlGuardianNationality.SelectedValue = guardian.NationalityID.ToString();
                                    //txtGuardianPhoneOffice.Text = guardian.PhoneOffice;
                                    //txtGuardianPhoneRes.Text = guardian.PhoneResidence;
                                    try
                                    {
                                        if (guardian.Attribute1 == null)
                                        {
                                            ddlGuardianOccupationType.SelectedValue = "-1";
                                        }
                                        else
                                        {
                                            ddlGuardianOccupationType.Items.FindByText(guardian.Attribute1).Selected = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                            #endregion


                            if ((secondaryExam != null && secondaryExam.ExamTypeID == 5) || (higherSecondaryExam != null && higherSecondaryExam.ExamTypeID == 7) || educationCategoryId == 6) //O-Level || A-Level || Master's Program
                            {
                                txtFatherName.Enabled = true;
                                txtMotherName.Enabled = true;
                            }
                            else
                            {
                                txtFatherName.Enabled = false;
                                txtMotherName.Enabled = false;
                            }

                        }
                    }// end using




                    #region N/A -- Prevent Save if IsFinalSubmit or IsApproved
                    //try
                    //{
                    //    List<DAL.SPGetCandidateEducationCategoryByCandidateID_Result> list = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        list = db.AdmissionDB.SPGetCandidateEducationCategoryByCandidateID(cId).ToList();
                    //    }

                    //    if (list != null && list.Count > 0)
                    //    {
                    //        #region Bachelors
                    //        DAL.SPGetCandidateEducationCategoryByCandidateID_Result undergradCandidate =
                    //                                list.Where(c => c.EduCatID == 4).Take(1).FirstOrDefault();

                    //        if (undergradCandidate != null)
                    //        {
                    //            btnSave_Parent.Enabled = false;
                    //            btnSave_Parent.Visible = false;

                    //            if (undergradCandidate.IsApproved != null && undergradCandidate.IsApproved == true)
                    //            {
                    //                btnSave_Parent.Enabled = false;
                    //                btnSave_Parent.Visible = false;
                    //            }
                    //            else
                    //            {
                    //                btnSave_Parent.Enabled = true;
                    //                btnSave_Parent.Visible = true;
                    //            }

                    //            if (undergradCandidate.IsFinalSubmit != null && undergradCandidate.IsFinalSubmit == true)
                    //            {
                    //                btnSave_Parent.Enabled = false;
                    //                btnSave_Parent.Visible = false;
                    //            }
                    //            else
                    //            {
                    //                btnSave_Parent.Enabled = true;
                    //                btnSave_Parent.Visible = true;
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
                    //                    btnSave_Parent.Enabled = false;
                    //                    btnSave_Parent.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Parent.Enabled = true;
                    //                    btnSave_Parent.Visible = true;
                    //                }
                    //            }

                    //            if (gradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (gradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Parent.Enabled = false;
                    //                    btnSave_Parent.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Parent.Enabled = true;
                    //                    btnSave_Parent.Visible = true;
                    //                }
                    //            }
                    //        }
                    //        #endregion







                    //        #region Hide Save and Next Button for Bachelor Program Because Admission is closed
                    //        if (list.FirstOrDefault().EduCatID == 4)
                    //        {
                    //            //btnSave_Parent.Visible = false;
                    //            //btnNext.Visible = false;
                    //        }
                    //        #endregion


                    //    }
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}
                    #endregion

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
                    //        btnSave_Parent.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).ToList().Count > 0)
                    //        {
                    //            if (educationCategoryId == 4)
                    //            {
                    //                btnSave_Parent.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
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
                    //                            btnSave_Parent.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                    //                        }
                    //                        else
                    //                        {
                    //                            btnSave_Parent.Visible = false;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        btnSave_Parent.Visible = false;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Parent.Visible = false;
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

        protected void ddlIsLateFather_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIsLateFather.SelectedValue == "0")
            {
                txtFatherDesignation.Enabled = true;
                txtFatherEmail.Enabled = true;
                txtFatherMobile.Enabled = true;
                txtFatherNationalId.Enabled = true;
                txtFatherOccupation.Enabled = true;
                ddlFatherOccupationType.Enabled = true;
                ddlFatherServiceType.Enabled = true;
                txtFatherPreviousService.Enabled = true;
                txtFatherOrganization.Enabled = true;
                spanFatherMobile.Visible = true;
            }
            else if (ddlIsLateFather.SelectedValue == "1")
            {
                txtFatherDesignation.Enabled = false;
                txtFatherEmail.Enabled = false;
                txtFatherMobile.Enabled = false;
                txtFatherNationalId.Enabled = false;
                txtFatherOccupation.Enabled = false;
                ddlFatherOccupationType.SelectedValue = "-1";
                ddlFatherOccupationType.Enabled = false;
                ddlFatherServiceType.SelectedValue = "-1";
                ddlFatherServiceType.Enabled = false;
                txtFatherOrganization.Enabled = false;
                txtFatherPreviousService.Enabled = false;
                txtFatherDesignation.Text = string.Empty;
                txtFatherEmail.Text = string.Empty;
                txtFatherMobile.Text = string.Empty;
                txtFatherNationalId.Text = string.Empty;
                txtFatherOccupation.Text = string.Empty;
                txtFatherOrganization.Text = string.Empty;
                spanFatherMobile.Visible = false;
                txtFatherMobileREV.Enabled = false;
                //ddlFatherOccupationType.SelectedValue = "-1";
            }
            else if (ddlIsLateFather.SelectedValue == "-1")
            {
                txtFatherDesignation.Enabled = true;
                txtFatherEmail.Enabled = true;
                txtFatherMobile.Enabled = true;
                txtFatherNationalId.Enabled = true;
                txtFatherOccupation.Enabled = true;
                ddlFatherOccupationType.Enabled = true;
                ddlFatherServiceType.Enabled = true;
                txtFatherPreviousService.Enabled = true;
                txtFatherOrganization.Enabled = true;
                spanFatherMobile.Visible = true;
            }
        }

        protected void ddlIsLateMother_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIsLateMother.SelectedValue == "0")
            {
                txtMotherDesignation.Enabled = true;
                //txtMotherEmail.Enabled = true;
                txtMotherMobile.Enabled = true;
                txtMotherNationalId.Enabled = true;
                txtMotherOccupation.Enabled = true;
                txtMotherOrganization.Enabled = true;
                ddlMotherOccupationType.Enabled = true;
                ddlMotherServiceType.Enabled = true;
                txtMotherPreviousService.Enabled = true;
                spanMotherMobile.Visible = true;
            }
            else if (ddlIsLateMother.SelectedValue == "1")
            {
                txtMotherDesignation.Enabled = false;
                //txtMotherEmail.Enabled = false;
                txtMotherMobile.Enabled = false;
                txtMotherNationalId.Enabled = false;
                txtMotherOccupation.Enabled = false;
                txtMotherOrganization.Enabled = false;
                ddlMotherOccupationType.SelectedValue = "-1";
                ddlMotherOccupationType.Enabled = false;
                ddlMotherServiceType.SelectedValue = "-1";
                ddlMotherServiceType.Enabled = false;
                txtMotherPreviousService.Enabled = false;
                spanMotherMobile.Visible = false;
                txtMotherMobileREV.Enabled = false;
                txtMotherDesignation.Text = string.Empty;
                //txtMotherEmail.Text = string.Empty;
                txtMotherMobile.Text = string.Empty;
                txtMotherNationalId.Text = string.Empty;
                txtMotherOccupation.Text = string.Empty;
                txtMotherOrganization.Text = string.Empty;
            }
            else if (ddlIsLateMother.SelectedValue == "-1")
            {
                txtMotherDesignation.Enabled = true;
                //txtMotherEmail.Enabled = true;
                txtMotherMobile.Enabled = true;
                txtMotherNationalId.Enabled = true;
                txtMotherOccupation.Enabled = true;
                txtMotherOrganization.Enabled = true;
                ddlMotherOccupationType.Enabled = true;
                ddlMotherServiceType.Enabled = true;
                txtMotherPreviousService.Enabled = true;
                spanMotherMobile.Visible = true;
            }
        }

        protected void ddlGuardianRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGuardianRelation.SelectedValue == "4") // other
            {
                txtGuardianOtherRelation.Enabled = true;
                txtGuardianOtherRelation.Text = string.Empty;
                txtGuardian_Name.Text = null;
                txtGuardianOccupation.Text = null;
                txtGuardianMobile.Text = null;
                txtGuardianEmail.Text = null;
                txtGuardianMailingAddress.Text = null;
                txtGuardianNationalId.Text = null;
                ddlGuardianNationality.SelectedValue = null;
                ddlGuardianOccupationType.SelectedValue = null;
            }
            else if (ddlGuardianRelation.SelectedItem.Text == "Father") //father
            {
                txtGuardianOtherRelation.Enabled = false;
                txtGuardianOtherRelation.Text = string.Empty;
                txtGuardian_Name.Text = txtFatherName.Text;
                txtGuardianOccupation.Text = txtFatherOccupation.Text;
                txtGuardianMobile.Text = txtFatherMobile.Text;
                txtGuardianEmail.Text = txtFatherEmail.Text.ToLower();
                //txtGuardianMailingAddress.Text
                txtGuardianNationalId.Text = txtFatherNationalId.Text;
                ddlGuardianNationality.SelectedValue = ddlFatherNationality.SelectedValue;
                if (ddlFatherOccupationType.SelectedValue == "8")
                {
                    ddlGuardianOccupationType.SelectedValue = ddlFatherServiceType.SelectedValue;
                }
                else
                {
                    ddlGuardianOccupationType.SelectedValue = ddlFatherOccupationType.SelectedValue;
                }
            }
            else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
            {
                txtGuardianOtherRelation.Enabled = false;
                txtGuardianOtherRelation.Text = string.Empty;
                txtGuardian_Name.Text = txtMotherName.Text;
                txtGuardianOccupation.Text = txtMotherOccupation.Text;
                txtGuardianMobile.Text = txtMotherMobile.Text;
                //txtGuardianEmail.Text = .Text;
                //txtGuardianMailingAddress.Text
                txtGuardianNationalId.Text = txtMotherNationalId.Text;
                ddlGuardianNationality.SelectedValue = ddlMotherNationality.SelectedValue;
                if (ddlMotherOccupationType.SelectedValue == "8")
                {
                    ddlGuardianOccupationType.SelectedValue = ddlMotherServiceType.SelectedValue;
                }
                else
                {
                    ddlGuardianOccupationType.SelectedValue = ddlMotherOccupationType.SelectedValue;
                }
            }
            else if (ddlGuardianRelation.SelectedValue == "-1")
            {
                txtGuardianOtherRelation.Enabled = false;
                txtGuardianOtherRelation.Text = string.Empty;
                txtGuardian_Name.Text = null;
                txtGuardianOccupation.Text = null;
                txtGuardianMobile.Text = null;
                txtGuardianEmail.Text = null;
                txtGuardianMailingAddress.Text = null;
                txtGuardianNationalId.Text = null;
                ddlGuardianNationality.SelectedValue = null;
                ddlGuardianOccupationType.SelectedValue = null;
            }
        }

        protected void btnSave_Parent_Click(object sender, EventArgs e)
        {
            long cId = -1;
            try
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
            }
            catch (Exception)
            {
                lblMessageParent.Text = "Error getting candidate.";
                messagePanel_Parent.CssClass = "alert alert-danger";
                messagePanel_Parent.Visible = true;
            }

            //*** Check for national id no greater than 0
            long fatherNatIdNo = -1;
            long motherNatIdNo = -1;
            long guardianNatIdNo = -1;
            string natIdMsg = string.Empty;

            #region N/A
            //if (ddlIsLateFather.SelectedItem.Value == "0" || ddlIsLateMother.SelectedItem.Value == "0")
            //{
            //    if (!string.IsNullOrEmpty(txtFatherNationalId.Text))
            //    {
            //        fatherNatIdNo = Convert.ToInt64(txtFatherNationalId.Text);
            //    }
            //    else
            //    {
            //        fatherNatIdNo = -1;
            //    }

            //    if (!string.IsNullOrEmpty(txtMotherNationalId.Text))
            //    {
            //        motherNatIdNo = Convert.ToInt64(txtMotherNationalId.Text);
            //    }
            //    else
            //    {
            //        motherNatIdNo = -1;
            //    }

            //}

            //if (!string.IsNullOrEmpty(txtGuardianNationalId.Text))
            //{
            //    guardianNatIdNo = Convert.ToInt64(txtGuardianNationalId.Text);
            //}
            //else
            //{
            //    guardianNatIdNo = -1;
            //}

            //if ((ddlIsLateFather.SelectedValue == "0" && fatherNatIdNo < 0) ||
            //    (ddlIsLateMother.SelectedValue == "0" && motherNatIdNo < 0) ||
            //    guardianNatIdNo < 0)
            //{
            //    lblMessageParent.Text = "National ID is not valid";
            //    messagePanel_Parent.CssClass = "alert alert-danger";
            //    messagePanel_Parent.Visible = true;
            //    return;
            //} 
            #endregion

            if (ddlIsLateFather.SelectedValue == "0")
            {
                //Changed by Rafi on dec 12 2023

                //if (string.IsNullOrEmpty(txtFatherNationalId.Text.Trim()))
                //{
                //    //fatherNatIdNo = Convert.ToInt64(txtFatherNationalId.Text.Trim());
                //    //if (fatherNatIdNo <= 0 || fatherNatIdNo.ToString().Length < 6)
                //    //{
                //    //    natIdMsg += "Father National Id number is not valid<br/>";
                //    //}

                //    natIdMsg += "Please provide Father National ID / Birth Cert No. / Passport No.<br/>";
                //}
                //else
                //{
                //    fatherNatIdNo = -1;
                //}
            }
            else if (ddlIsLateFather.SelectedValue == "1")
            {
                fatherNatIdNo = -1;
                //natIdMsg += string.Empty;
            }
            else if (ddlIsLateFather.SelectedValue == "-1")
            {
                fatherNatIdNo = -1;
            }

            if (ddlIsLateMother.SelectedValue == "0")
            {
                //Changed by Rafi on dec 12 2023

                //if (string.IsNullOrEmpty(txtMotherNationalId.Text.Trim()))
                //{
                //    //motherNatIdNo = Convert.ToInt64(txtMotherNationalId.Text.Trim());
                //    //if (motherNatIdNo <= 0 || motherNatIdNo.ToString().Length < 6)
                //    //{
                //    //    natIdMsg += "Mother National Id number is not valid<br/>";
                //    //}

                //    natIdMsg += "Please provide Mother National ID / Birth Cert No. / Passport No.<br/>";
                //}
                //else
                //{
                //    motherNatIdNo = -1;
                //}
            }
            else if (ddlIsLateMother.SelectedValue == "1")
            {
                motherNatIdNo = -1;
                //natIdMsg += string.Empty;
            }
            else if (ddlIsLateMother.SelectedValue == "-1")
            {
                motherNatIdNo = -1;
            }

            //if (!string.IsNullOrEmpty(txtGuardianNationalId.Text.Trim()))
            //{
            //    guardianNatIdNo = Convert.ToInt64(txtGuardianNationalId.Text.Trim());
            //    if (guardianNatIdNo <= 0 || guardianNatIdNo.ToString().Length < 6)
            //    {
            //        natIdMsg += "Guardian National Id number is not valid<br/>";
            //    }
            //}
            //else
            //{
            //    guardianNatIdNo = -1;
            //    natIdMsg += "Please provide Guardian National Id number<br/>"; ;
            //}

            //Changed by Rafi on dec 12 2023


            //if (string.IsNullOrEmpty(txtGuardianNationalId.Text.Trim()))
            //{
            //    natIdMsg += "Please provide Guardian National ID / Birth Cert No. / Passport No.<br/>";
            //}

            if (!string.IsNullOrEmpty(natIdMsg))
            {
                lblMessageParent.Text = natIdMsg;
                messagePanel_Parent.CssClass = "alert alert-danger";
                messagePanel_Parent.Visible = true;
                return;
            }

            //***

            try
            {
                if (cId > 0)
                {

                    int guardianRelation = Convert.ToInt32(ddlGuardianRelation.SelectedValue);
                    if (guardianRelation < 0)
                    {
                        lblMessageParent.Text = "Please Select Relationship with the Applicant";
                        messagePanel_Parent.CssClass = "alert alert-danger";
                        messagePanel_Parent.Visible = true;
                        return;
                    }

                    string logOldObject = string.Empty;
                    string logNewObject = string.Empty;

                    DAL.BasicInfo basicInfo = null;
                    DAL.CandidatePayment candidatePayment = null;

                    DAL.Relation father = null;
                    DAL.Relation mother = null;
                    DAL.Relation guardian = null;

                    using (var db = new CandidateDataManager())
                    {
                        basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                        candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();

                        List<DAL.Relation> relationList = null;

                        relationList = db.GetAllRelationByCandidateID_AD(cId);

                        if (relationList != null)
                        {
                            father = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();
                            mother = relationList.Where(a => a.RelationTypeID == 3).FirstOrDefault();
                            guardian = relationList.Where(a => a.RelationTypeID == 5).FirstOrDefault();
                        }
                    }

                    #region FATHER

                    if (father != null) // relation (father) object exist
                    {
                        DAL.RelationDetail fatherRelationDetail = father.RelationDetail;

                        if (fatherRelationDetail != null) // relationDetails (father) object exist, update
                        {
                            logOldObject = string.Empty;
                            logOldObject = GenerateLogStringFromObject(father, fatherRelationDetail);

                            fatherRelationDetail.Name = txtFatherName.Text.ToUpper();
                            fatherRelationDetail.Mobile = txtFatherMobile.Text;
                            fatherRelationDetail.Occupation = txtFatherOccupation.Text;
                            //fatherRelationDetail.Occupation = ddlFatherOccupationType.SelectedItem.Text;
                            fatherRelationDetail.Organization = txtFatherOrganization.Text;
                            //fatherRelationDetail.OrgAddress = txtFatherOrgAddress.Text;
                            fatherRelationDetail.Designation = txtFatherDesignation.Text;
                            fatherRelationDetail.Email = txtFatherEmail.Text.ToLower();
                            fatherRelationDetail.NationalIdNumber = txtFatherNationalId.Text;
                            fatherRelationDetail.NationalityID = Int32.Parse(ddlFatherNationality.SelectedValue.ToString()) == -1 ? 1 : Int32.Parse(ddlFatherNationality.SelectedValue.ToString());
                            if (ddlFatherOccupationType.SelectedItem.Text == "-1")
                            {
                                fatherRelationDetail.Attribute1 = null;
                            }
                            else
                            {
                                if (ddlFatherOccupationType.SelectedValue == "8")
                                {
                                    fatherRelationDetail.Attribute1 = ddlFatherServiceType.SelectedItem.Text;
                                }
                                else
                                {
                                    fatherRelationDetail.Attribute1 = ddlFatherOccupationType.SelectedItem.Text;
                                }
                            }

                            if (ddlIsLateFather.SelectedValue == "0")
                            {
                                fatherRelationDetail.IsLate = false;
                            }
                            else if (ddlIsLateFather.SelectedValue == "1")
                            {
                                fatherRelationDetail.IsLate = true;
                            }
                            else if (ddlIsLateFather.SelectedValue == "-1")
                            {
                                fatherRelationDetail.IsLate = null;
                            }

                            fatherRelationDetail.Attribute3 = txtFatherPreviousService.Text;
                            fatherRelationDetail.DateModified = DateTime.Now;
                            fatherRelationDetail.ModifiedBy = cId;

                            using (var dbUpdateFatherRelationDetails = new CandidateDataManager())
                            {
                                dbUpdateFatherRelationDetails.Update<DAL.RelationDetail>(fatherRelationDetail);
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(father, fatherRelationDetail);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.CandidateId = cId;
                                dLog.EventName = "Relation Info Update (Father) (Candidate)";
                                dLog.PageName = "ApplicationRelation.aspx";
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
                            //    dLog.EventName = "Relation Info Update (Father) (Candidate)";
                            //    dLog.PageName = "ApplicationRelation.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                        else // relationDetails (father) does not exist, create new then update relation
                        {
                            DAL.RelationDetail newFatherRelationDetails = new DAL.RelationDetail();

                            newFatherRelationDetails.Name = txtFatherName.Text.ToUpper();
                            newFatherRelationDetails.Mobile = txtFatherMobile.Text;
                            newFatherRelationDetails.Occupation = txtFatherOccupation.Text;
                            //newFatherRelationDetails.Occupation = ddlFatherOccupationType.SelectedItem.Text;
                            newFatherRelationDetails.Organization = txtFatherOrganization.Text;
                            //fatherRelationDetail.OrgAddress = txtFatherOrgAddress.Text;
                            newFatherRelationDetails.Designation = txtFatherDesignation.Text;
                            newFatherRelationDetails.Email = txtFatherEmail.Text.ToLower();
                            newFatherRelationDetails.NationalIdNumber = txtFatherNationalId.Text;
                            newFatherRelationDetails.NationalityID = Int32.Parse(ddlFatherNationality.SelectedValue.ToString()) == -1 ? 1 : Int32.Parse(ddlFatherNationality.SelectedValue.ToString());
                            if (ddlFatherOccupationType.SelectedItem.Text == "-1")
                            {
                                newFatherRelationDetails.Attribute1 = null;
                            }
                            else
                            {
                                if (ddlFatherOccupationType.SelectedValue == "8")
                                {
                                    newFatherRelationDetails.Attribute1 = ddlFatherServiceType.SelectedItem.Text;
                                }
                                else
                                {
                                    newFatherRelationDetails.Attribute1 = ddlFatherOccupationType.SelectedItem.Text;
                                }

                            }

                            if (ddlIsLateFather.SelectedValue == "0")
                            {
                                newFatherRelationDetails.IsLate = false;
                            }
                            else if (ddlIsLateFather.SelectedValue == "1")
                            {
                                newFatherRelationDetails.IsLate = true;
                            }
                            else if (ddlIsLateFather.SelectedValue == "-1")
                            {
                                newFatherRelationDetails.IsLate = null;
                            }

                            newFatherRelationDetails.Attribute3 = txtFatherPreviousService.Text;
                            newFatherRelationDetails.CreatedBy = cId;
                            newFatherRelationDetails.DateCreated = DateTime.Now;

                            long newFatherRelationDetailsID = -1;
                            using (var dbInsertFatherRelationDetails = new CandidateDataManager())
                            {
                                dbInsertFatherRelationDetails.Insert<DAL.RelationDetail>(newFatherRelationDetails);
                                newFatherRelationDetailsID = newFatherRelationDetails.ID;
                            }
                            if (newFatherRelationDetailsID > 0)
                            {
                                father.RelationDetailsID = newFatherRelationDetailsID;

                                using (var dbUpdateFatherRelation = new CandidateDataManager())
                                {
                                    dbUpdateFatherRelation.Update<DAL.Relation>(father);
                                }
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(father, newFatherRelationDetails);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Relation Info Update (Father) (Admin)";
                                //dLog.PageName = "CandApplicationRelation.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                                dLog.EventName = "Relation Info Update (Father) (Candidate)";
                                dLog.PageName = "ApplicationRelation.aspx";
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
                            //    dLog.EventName = "Relation Info Update (Father) (Candidate)";
                            //    dLog.PageName = "ApplicationRelation.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                    else // relation (father) object does not exist, first create a relation details and then relation
                    {
                        DAL.RelationDetail newFatherRelationDetails = new DAL.RelationDetail();

                        newFatherRelationDetails.Name = txtFatherName.Text.ToUpper();
                        newFatherRelationDetails.Mobile = txtFatherMobile.Text;
                        newFatherRelationDetails.Occupation = txtFatherOccupation.Text;
                        //newFatherRelationDetails.Occupation = ddlFatherOccupationType.SelectedItem.Text;
                        newFatherRelationDetails.Organization = txtFatherOrganization.Text;
                        //newFatherRelationDetails.OrgAddress = txtFatherOrgAddress.Text;
                        newFatherRelationDetails.Designation = txtFatherDesignation.Text;
                        newFatherRelationDetails.Email = txtFatherEmail.Text.ToLower();
                        newFatherRelationDetails.NationalIdNumber = txtFatherNationalId.Text;
                        newFatherRelationDetails.NationalityID = Int32.Parse(ddlFatherNationality.SelectedValue.ToString()) == -1 ? 1 : Int32.Parse(ddlFatherNationality.SelectedValue.ToString());
                        if (ddlFatherOccupationType.SelectedItem.Text == "-1")
                        {
                            newFatherRelationDetails.Attribute1 = null;
                        }
                        else
                        {
                            if (ddlFatherOccupationType.SelectedValue == "8")
                            {
                                newFatherRelationDetails.Attribute1 = ddlFatherServiceType.SelectedItem.Text;
                            }
                            else
                            {
                                newFatherRelationDetails.Attribute1 = ddlFatherOccupationType.SelectedItem.Text;
                            }

                        }

                        if (ddlIsLateFather.SelectedValue == "0")
                        {
                            newFatherRelationDetails.IsLate = false;
                        }
                        else if (ddlIsLateFather.SelectedValue == "1")
                        {
                            newFatherRelationDetails.IsLate = true;
                        }
                        else if (ddlIsLateFather.SelectedValue == "-1")
                        {
                            newFatherRelationDetails.IsLate = null;
                        }
                        newFatherRelationDetails.Attribute3 = txtFatherPreviousService.Text;
                        newFatherRelationDetails.CreatedBy = cId;
                        newFatherRelationDetails.DateCreated = DateTime.Now;

                        DAL.Relation newFatherRelation = new DAL.Relation();

                        newFatherRelation.CandidateID = cId;
                        newFatherRelation.RelationTypeID = 2;
                        newFatherRelation.CreatedBy = cId;
                        newFatherRelation.DateCreated = DateTime.Now;

                        using (var dbRelationDetailsInsert = new CandidateDataManager())
                        {
                            dbRelationDetailsInsert.Insert<DAL.RelationDetail>(newFatherRelationDetails);
                            newFatherRelation.RelationDetailsID = newFatherRelationDetails.ID;
                        }
                        using (var dbRelationInsert = new CandidateDataManager())
                        {
                            dbRelationInsert.Insert<DAL.Relation>(newFatherRelation);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(newFatherRelation, newFatherRelationDetails);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Relation Info Insert (Father) (Admin)";
                            //dLog.PageName = "CandApplicationRelation.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Relation Information.";
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
                            dLog.EventName = "Relation Info Insert (Father) (Candidate)";
                            dLog.PageName = "ApplicationRelation.aspx";
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
                        //    dLog.EventName = "Relation Info Insert (Father) (Candidate)";
                        //    dLog.PageName = "ApplicationRelation.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Relation Information.";
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

                    #endregion

                    #region MOTHER

                    if (mother != null) // relation (mother) object exist
                    {
                        DAL.RelationDetail motherRelationDetail = mother.RelationDetail;

                        if (motherRelationDetail != null) // relationDetails (mother) object exist, update
                        {

                            logOldObject = string.Empty;
                            logOldObject = GenerateLogStringFromObject(mother, motherRelationDetail);

                            motherRelationDetail.Name = txtMotherName.Text.ToUpper();
                            motherRelationDetail.Mobile = txtMotherMobile.Text;
                            motherRelationDetail.Occupation = txtMotherOccupation.Text;
                            motherRelationDetail.Organization = txtMotherOrganization.Text;
                            //mother_rd.MailingAddress = txtMotherMailingAddress.Text;
                            motherRelationDetail.Designation = txtMotherDesignation.Text;
                            motherRelationDetail.NationalIdNumber = txtMotherNationalId.Text;
                            motherRelationDetail.NationalityID = Int32.Parse(ddlMotherNationality.SelectedValue.ToString()) == -1 ? 1 : Int32.Parse(ddlMotherNationality.SelectedValue.ToString());
                            if (ddlIsLateMother.SelectedValue == "0")
                            {
                                motherRelationDetail.IsLate = false;
                            }
                            else if (ddlIsLateMother.SelectedValue == "1")
                            {
                                motherRelationDetail.IsLate = true;
                            }
                            else if (ddlIsLateMother.SelectedValue == "-1")
                            {
                                motherRelationDetail.IsLate = null;
                            }

                            if (ddlMotherOccupationType.SelectedItem.Text == "-1")
                            {
                                motherRelationDetail.Attribute1 = null;
                            }
                            else
                            {
                                if (ddlMotherOccupationType.SelectedValue == "8")
                                {
                                    motherRelationDetail.Attribute1 = ddlMotherServiceType.SelectedItem.Text;
                                }
                                else
                                {
                                    motherRelationDetail.Attribute1 = ddlMotherOccupationType.SelectedItem.Text;
                                }
                            }
                            motherRelationDetail.Attribute3 = txtMotherPreviousService.Text;
                            motherRelationDetail.DateModified = DateTime.Now;
                            motherRelationDetail.ModifiedBy = cId;

                            using (var dbUpdateMotherRelationDetails = new CandidateDataManager())
                            {
                                dbUpdateMotherRelationDetails.Update<DAL.RelationDetail>(motherRelationDetail);
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(mother, motherRelationDetail);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Relation Info Update (Mother) (Admin)";
                                //dLog.PageName = "CandApplicationRelation.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                                dLog.EventName = "Relation Info Update (Mother) (Candidate)";
                                dLog.PageName = "ApplicationRelation.aspx";
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
                            //    dLog.EventName = "Relation Info Update (Mother) (Candidate)";
                            //    dLog.PageName = "ApplicationRelation.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                        else // relationDetails (mother) does not exist, create new then update relation
                        {
                            DAL.RelationDetail newMotherRelationDetails = new DAL.RelationDetail();

                            newMotherRelationDetails.Name = txtMotherName.Text.ToUpper();
                            newMotherRelationDetails.Mobile = txtMotherMobile.Text;
                            newMotherRelationDetails.Occupation = txtMotherOccupation.Text;
                            newMotherRelationDetails.Organization = txtMotherOrganization.Text;
                            //mother_rd.MailingAddress = txtMotherMailingAddress.Text;
                            newMotherRelationDetails.Designation = txtMotherDesignation.Text;
                            newMotherRelationDetails.NationalIdNumber = txtMotherNationalId.Text;
                            newMotherRelationDetails.NationalityID = Int32.Parse(ddlMotherNationality.SelectedValue.ToString()) == -1 ? 1 : Int32.Parse(ddlMotherNationality.SelectedValue.ToString());
                            if (ddlIsLateMother.SelectedValue == "0")
                            {
                                newMotherRelationDetails.IsLate = false;
                            }
                            else if (ddlIsLateMother.SelectedValue == "1")
                            {
                                newMotherRelationDetails.IsLate = true;
                            }
                            else if (ddlIsLateMother.SelectedValue == "-1")
                            {
                                newMotherRelationDetails.IsLate = null;
                            }

                            if (ddlMotherOccupationType.SelectedItem.Text == "-1")
                            {
                                newMotherRelationDetails.Attribute1 = null;
                            }
                            else
                            {
                                if (ddlMotherOccupationType.SelectedValue == "8")
                                {
                                    newMotherRelationDetails.Attribute1 = ddlMotherServiceType.SelectedItem.Text;
                                }
                                else
                                {
                                    newMotherRelationDetails.Attribute1 = ddlMotherOccupationType.SelectedItem.Text;
                                }
                            }
                            newMotherRelationDetails.Attribute3 = txtMotherPreviousService.Text;
                            newMotherRelationDetails.CreatedBy = cId;
                            newMotherRelationDetails.DateCreated = DateTime.Now;

                            long newMotherRelationDetailsID = -1;
                            using (var dbInsertFatherRelationDetails = new CandidateDataManager())
                            {
                                dbInsertFatherRelationDetails.Insert<DAL.RelationDetail>(newMotherRelationDetails);
                                newMotherRelationDetailsID = newMotherRelationDetails.ID;
                            }
                            if (newMotherRelationDetailsID > 0)
                            {
                                father.RelationDetailsID = newMotherRelationDetailsID;

                                using (var dbUpdateMotherRelation = new CandidateDataManager())
                                {
                                    dbUpdateMotherRelation.Update<DAL.Relation>(mother);
                                }
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(mother, newMotherRelationDetails);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Relation Info Update (Mother) (Admin)";
                                //dLog.PageName = "CandApplicationRelation.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                                dLog.EventName = "Relation Info Update (Mother) (Candidate)";
                                dLog.PageName = "ApplicationRelation.aspx";
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
                            //    dLog.EventName = "Relation Info Update (Mother) (Candidate)";
                            //    dLog.PageName = "ApplicationRelation.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                    else // relation (mohter) object does not exist, first create a relation details and then relation
                    {
                        DAL.RelationDetail newMotherRelationDetails = new DAL.RelationDetail();

                        newMotherRelationDetails.Name = txtMotherName.Text.ToUpper();
                        newMotherRelationDetails.Mobile = txtMotherMobile.Text;
                        newMotherRelationDetails.Occupation = txtMotherOccupation.Text;
                        newMotherRelationDetails.Organization = txtMotherOrganization.Text;
                        //mother_rd.MailingAddress = txtMotherMailingAddress.Text;
                        newMotherRelationDetails.Designation = txtMotherDesignation.Text;
                        newMotherRelationDetails.NationalIdNumber = txtMotherNationalId.Text;
                        newMotherRelationDetails.NationalityID = Int32.Parse(ddlMotherNationality.SelectedValue.ToString()) == -1 ? 1 : Int32.Parse(ddlMotherNationality.SelectedValue.ToString());
                        if (ddlIsLateMother.SelectedValue == "0")
                        {
                            newMotherRelationDetails.IsLate = false;
                        }
                        else if (ddlIsLateMother.SelectedValue == "1")
                        {
                            newMotherRelationDetails.IsLate = true;
                        }
                        else if (ddlIsLateMother.SelectedValue == "-1")
                        {
                            newMotherRelationDetails.IsLate = null;
                        }

                        if (ddlMotherOccupationType.SelectedItem.Text == "-1")
                        {
                            newMotherRelationDetails.Attribute1 = null;
                        }
                        else
                        {
                            if (ddlMotherOccupationType.SelectedValue == "8")
                            {
                                newMotherRelationDetails.Attribute1 = ddlMotherServiceType.SelectedItem.Text;
                            }
                            else
                            {
                                newMotherRelationDetails.Attribute1 = ddlMotherOccupationType.SelectedItem.Text;
                            }
                        }
                        newMotherRelationDetails.Attribute3 = txtMotherPreviousService.Text;
                        newMotherRelationDetails.CreatedBy = cId;
                        newMotherRelationDetails.DateCreated = DateTime.Now;

                        DAL.Relation newMotherRelation = new DAL.Relation();

                        newMotherRelation.CandidateID = cId;
                        newMotherRelation.RelationTypeID = 3;
                        newMotherRelation.CreatedBy = cId;
                        newMotherRelation.DateCreated = DateTime.Now;

                        using (var dbRelationDetailsInsert = new CandidateDataManager())
                        {
                            dbRelationDetailsInsert.Insert<DAL.RelationDetail>(newMotherRelationDetails);
                            newMotherRelation.RelationDetailsID = newMotherRelationDetails.ID;
                        }
                        using (var dbRelationInsert = new CandidateDataManager())
                        {
                            dbRelationInsert.Insert<DAL.Relation>(newMotherRelation);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(newMotherRelation, newMotherRelationDetails);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Relation Info Insert (Mother) (Admin)";
                            //dLog.PageName = "CandApplicationRelation.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Relation Information.";
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
                            dLog.EventName = "Relation Info Insert (Mother) (Candidate)";
                            dLog.PageName = "ApplicationRelation.aspx";
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
                        //    dLog.EventName = "Relation Info Insert (Mother) (Candidate)";
                        //    dLog.PageName = "ApplicationRelation.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Relation Information.";
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

                    #endregion

                    #region GUARDIAN

                    if (guardian != null) // relation (guardian) object exist
                    {
                        DAL.RelationDetail guardianRelationDetails = guardian.RelationDetail;

                        if (guardianRelationDetails != null) // relationDetails (guardian) object exist, update
                        {

                            logOldObject = string.Empty;
                            logOldObject = GenerateLogStringFromObject(guardian, guardianRelationDetails);

                            guardianRelationDetails.Name = txtGuardian_Name.Text.ToUpper();
                            guardianRelationDetails.Mobile = txtGuardianMobile.Text;
                            guardianRelationDetails.Occupation = txtGuardianOccupation.Text;
                            guardianRelationDetails.Organization = txtGuardianOrganization.Text;
                            guardianRelationDetails.MailingAddress = txtGuardianMailingAddress.Text;
                            if (ddlGuardianRelation.SelectedItem.Text == "Father")
                            {
                                guardianRelationDetails.RelationWithGuardian = "Father";
                            }
                            else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                            {
                                guardianRelationDetails.RelationWithGuardian = "Mother";
                            }
                            else if (ddlGuardianRelation.SelectedValue == "4")
                            {
                                guardianRelationDetails.RelationWithGuardian = txtGuardianOtherRelation.Text;
                            }

                            if (ddlGuardianOccupationType.SelectedItem.Text == "-1")
                            {
                                guardianRelationDetails.Attribute1 = null;
                            }
                            else
                            {
                                guardianRelationDetails.Attribute1 = ddlGuardianOccupationType.SelectedItem.Text;
                            }

                            guardianRelationDetails.Email = txtGuardianEmail.Text.ToLower();
                            //guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                            //guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;
                            guardianRelationDetails.NationalIdNumber = txtGuardianNationalId.Text;
                            guardianRelationDetails.NationalityID = Int32.Parse(ddlGuardianNationality.SelectedValue) == -1 ? 1 : Int32.Parse(ddlGuardianNationality.SelectedValue);

                            guardianRelationDetails.DateModified = DateTime.Now;
                            guardianRelationDetails.ModifiedBy = cId;

                            using (var dbUpdateGuardianRelationDetails = new CandidateDataManager())
                            {
                                dbUpdateGuardianRelationDetails.Update<DAL.RelationDetail>(guardianRelationDetails);
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(guardian, guardianRelationDetails);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Relation Info Update (Guardian) (Admin)";
                                //dLog.PageName = "CandApplicationRelation.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                                dLog.EventName = "Relation Info Update (Guardian) (Candidate)";
                                dLog.PageName = "ApplicationRelation.aspx";
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
                            //    dLog.EventName = "Relation Info Update (Guardian) (Candidate)";
                            //    dLog.PageName = "ApplicationRelation.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                        else // relationDetails (guardian) does not exist, create new then update relation
                        {
                            DAL.RelationDetail newGuardianRelationDetails = new DAL.RelationDetail();

                            newGuardianRelationDetails.Name = txtGuardian_Name.Text.ToUpper();
                            newGuardianRelationDetails.IsGuardian = true;
                            newGuardianRelationDetails.Mobile = txtGuardianMobile.Text;
                            newGuardianRelationDetails.Occupation = txtGuardianOccupation.Text;
                            newGuardianRelationDetails.Organization = txtGuardianOrganization.Text;
                            newGuardianRelationDetails.MailingAddress = txtGuardianMailingAddress.Text;
                            if (ddlGuardianRelation.SelectedItem.Text == "Father")
                            {
                                newGuardianRelationDetails.RelationWithGuardian = "Father";
                            }
                            else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                            {
                                newGuardianRelationDetails.RelationWithGuardian = "Mother";
                            }
                            else if (ddlGuardianRelation.SelectedValue == "4")
                            {
                                newGuardianRelationDetails.RelationWithGuardian = txtGuardianOtherRelation.Text;
                            }

                            if (ddlGuardianOccupationType.SelectedItem.Text == "-1")
                            {
                                newGuardianRelationDetails.Attribute1 = null;
                            }
                            else
                            {
                                newGuardianRelationDetails.Attribute1 = ddlGuardianOccupationType.SelectedItem.Text;
                            }

                            //guardian_rd.Email = txtGuardianEmail.Text;
                            //guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                            //guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;
                            newGuardianRelationDetails.NationalIdNumber = txtGuardianNationalId.Text;
                            guardianRelationDetails.NationalityID = Int32.Parse(ddlGuardianNationality.SelectedValue) == -1 ? 1 : Int32.Parse(ddlGuardianNationality.SelectedValue);

                            newGuardianRelationDetails.CreatedBy = cId;
                            newGuardianRelationDetails.DateCreated = DateTime.Now;

                            long newGuardianRelationDetailsID = -1;
                            using (var dbInsertGuardianRelationDetails = new CandidateDataManager())
                            {
                                dbInsertGuardianRelationDetails.Insert<DAL.RelationDetail>(newGuardianRelationDetails);
                                newGuardianRelationDetailsID = newGuardianRelationDetails.ID;
                            }
                            if (newGuardianRelationDetailsID > 0)
                            {
                                father.RelationDetailsID = newGuardianRelationDetailsID;

                                using (var dbUpdateGuardianRelation = new CandidateDataManager())
                                {
                                    dbUpdateGuardianRelation.Update<DAL.Relation>(guardian);
                                }
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(guardian, newGuardianRelationDetails);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Relation Info Update (Guardian) (Admin)";
                                //dLog.PageName = "CandApplicationRelation.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                                dLog.EventName = "Relation Info Update (Guardian) (Candidate)";
                                dLog.PageName = "ApplicationRelation.aspx";
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
                            //    dLog.EventName = "Relation Info Update (Guardian) (Candidate)";
                            //    dLog.PageName = "ApplicationRelation.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Relation Information.";
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
                    else // relation (guardian) object does not exist, first create a relation details and then relation
                    {
                        DAL.RelationDetail newGuardianRelationDetails = new DAL.RelationDetail();

                        newGuardianRelationDetails.Name = txtGuardian_Name.Text.ToUpper();
                        newGuardianRelationDetails.IsGuardian = true;
                        newGuardianRelationDetails.Mobile = txtGuardianMobile.Text;
                        newGuardianRelationDetails.Occupation = txtGuardianOccupation.Text;
                        newGuardianRelationDetails.Organization = txtGuardianOrganization.Text;
                        newGuardianRelationDetails.MailingAddress = txtGuardianMailingAddress.Text;
                        if (ddlGuardianRelation.SelectedItem.Text == "Father")
                        {
                            newGuardianRelationDetails.RelationWithGuardian = "Father";
                        }
                        else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                        {
                            newGuardianRelationDetails.RelationWithGuardian = "Mother";
                        }
                        else if (ddlGuardianRelation.SelectedValue == "4")
                        {
                            newGuardianRelationDetails.RelationWithGuardian = txtGuardianOtherRelation.Text;
                        }

                        if (ddlGuardianOccupationType.SelectedItem.Text == "-1")
                        {
                            newGuardianRelationDetails.Attribute1 = null;
                        }
                        else
                        {
                            newGuardianRelationDetails.Attribute1 = ddlGuardianOccupationType.SelectedItem.Text;
                        }

                        //guardian_rd.Email = txtGuardianEmail.Text;
                        //guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                        //guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;
                        newGuardianRelationDetails.NationalIdNumber = txtGuardianNationalId.Text;
                        newGuardianRelationDetails.NationalityID = Int32.Parse(ddlGuardianNationality.SelectedValue) == -1 ? 1 : Int32.Parse(ddlGuardianNationality.SelectedValue);

                        newGuardianRelationDetails.CreatedBy = cId;
                        newGuardianRelationDetails.DateCreated = DateTime.Now;

                        DAL.Relation newGuardianRelation = new DAL.Relation();

                        newGuardianRelation.CandidateID = cId;
                        newGuardianRelation.RelationTypeID = 5;
                        newGuardianRelation.CreatedBy = cId;
                        newGuardianRelation.DateCreated = DateTime.Now;

                        using (var dbRelationDetailsInsert = new CandidateDataManager())
                        {
                            dbRelationDetailsInsert.Insert<DAL.RelationDetail>(newGuardianRelationDetails);
                            newGuardianRelation.RelationDetailsID = newGuardianRelationDetails.ID;
                        }
                        using (var dbRelationInsert = new CandidateDataManager())
                        {
                            dbRelationInsert.Insert<DAL.Relation>(newGuardianRelation);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(newGuardianRelation, newGuardianRelationDetails);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Relation Info Insert (Guardian) (Admin)";
                            //dLog.PageName = "CandApplicationRelation.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Relation Information.";
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
                            dLog.EventName = "Relation Info Insert (Guardian) (Candidate)";
                            dLog.PageName = "ApplicationRelation.aspx";
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
                        //    dLog.EventName = "Relation Info Insert (Guardian) (Candidate)";
                        //    dLog.PageName = "ApplicationRelation.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Relation Information.";
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

                    #endregion

                    lblMessageParent.Text = "Info Updated successfully.";
                    messagePanel_Parent.CssClass = "alert alert-success";
                    messagePanel_Parent.Visible = true;

                    Response.Redirect("ApplicationAddress.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessageParent.Text = "Unable to save/update candidate information.";
                messagePanel_Parent.CssClass = "alert alert-danger";
                messagePanel_Parent.Visible = true;
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            btnSave_Parent_Click(sender, e);
            Response.Redirect("ApplicationAddress.aspx", false);
        }



        protected string GenerateLogStringFromObject(DAL.Relation relation, DAL.RelationDetail relationDetail)
        {

            string result = "";

            try
            {


                #region ExamType
                if (relation != null && Convert.ToInt32(relation.RelationTypeID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "RelationType: " + (db.AdmissionDB.RelationTypes.Where(a => a.ID == relation.RelationTypeID).Select(x => x.RelationTypeName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "RelationType: ; ";
                }
                #endregion

                result += "Name: " + (!string.IsNullOrEmpty(relationDetail.Name) ? relationDetail.Name.ToString() : "") + "; ";
                result += "RelationWithGuardian: " + (!string.IsNullOrEmpty(relationDetail.RelationWithGuardian) ? relationDetail.Mobile.ToString() : "") + "; ";
                result += "Occupation: " + (!string.IsNullOrEmpty(relationDetail.Occupation) ? relationDetail.Occupation.ToString() : "") + "; ";
                result += "Organization: " + (!string.IsNullOrEmpty(relationDetail.Organization) ? relationDetail.Organization.ToString() : "") + "; ";
                result += "OrgAddress: " + (!string.IsNullOrEmpty(relationDetail.OrgAddress) ? relationDetail.OrgAddress.ToString() : "") + "; ";
                result += "Designation: " + (!string.IsNullOrEmpty(relationDetail.Designation) ? relationDetail.Designation.ToString() : "") + "; ";
                result += "Mobile: " + (!string.IsNullOrEmpty(relationDetail.Mobile) ? relationDetail.Mobile.ToString() : "") + "; ";
                result += "PhoneResidence: " + (!string.IsNullOrEmpty(relationDetail.PhoneResidence) ? relationDetail.PhoneResidence.ToString() : "") + "; ";
                result += "PhoneOffice: " + (!string.IsNullOrEmpty(relationDetail.PhoneOffice) ? relationDetail.PhoneOffice.ToString() : "") + "; ";
                result += "Email: " + (!string.IsNullOrEmpty(relationDetail.Email) ? relationDetail.Email.ToString() : "") + "; ";
                result += "NationalIdNumber: " + (!string.IsNullOrEmpty(relationDetail.NationalIdNumber) ? relationDetail.NationalIdNumber.ToString() : "") + "; ";
                result += "MailingAddress: " + (!string.IsNullOrEmpty(relationDetail.MailingAddress) ? relationDetail.MailingAddress.ToString() : "") + "; ";
                result += "Upazila: " + (!string.IsNullOrEmpty(relationDetail.Upazila) ? relationDetail.Upazila.ToString() : "") + "; ";


                #region Nationality
                if (relationDetail.NationalityID != null && Convert.ToInt32(relationDetail.NationalityID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Nationality: " + (db.AdmissionDB.Countries.Where(a => a.ID == relationDetail.NationalityID).Select(x => x.Name).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Nationality: ; ";
                }
                #endregion

                #region Division
                if (relationDetail.DivisionID != null && Convert.ToInt32(relationDetail.DivisionID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Division: " + (db.AdmissionDB.Divisions.Where(a => a.ID == relationDetail.DivisionID).Select(x => x.Name).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Division: ; ";
                }
                #endregion

                #region District
                if (relationDetail.DistrictID != null && Convert.ToInt32(relationDetail.DistrictID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "District: " + (db.AdmissionDB.Districts.Where(a => a.ID == relationDetail.DistrictID).Select(x => x.Name).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "District: ; ";
                }
                #endregion

                #region IsLate
                if (Convert.ToBoolean(relationDetail.IsLate) == true)
                {
                    result += "IsLate: Yes; ";
                }
                else
                {
                    result += "IsLate: No; ";
                }
                #endregion


            }
            catch (Exception ex)
            {
                result += "Exception: " + ex.Message.ToString() + "; ";

            }



            return result;
        }

        protected void ddlFatherOccupationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = Convert.ToInt32(ddlFatherOccupationType.SelectedValue);

            if (selectedValue == 8)
            {
                fatherService.Visible = true;
                fatherServiceTxt.Visible = true;
                fatherOrganization.Visible = true;
                fatherDesignation.Visible = true;
            }
            else
            {
                fatherService.Visible = false;
                fatherServiceTxt.Visible = false;
                fatherOrganization.Visible = false;
                fatherDesignation.Visible = false;
            }
        }

        protected void ddlMotherOccupationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = Convert.ToInt32(ddlMotherOccupationType.SelectedValue);

            if (selectedValue == 8)
            {
                motherService.Visible = true;
                motherServiceTxt.Visible = true;
                motherOrganization.Visible = true;
                motherDesignation.Visible = true;
            }
            else
            {
                motherService.Visible = false;
                motherServiceTxt.Visible = false;
                motherOrganization.Visible = false;
                motherDesignation.Visible = false;
            }
        }
    }
}