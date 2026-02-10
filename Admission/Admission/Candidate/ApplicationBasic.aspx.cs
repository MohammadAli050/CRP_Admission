using Admission.App_Start;
using CommonUtility;
using DAL;
using DATAMANAGER;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class ApplicationBasic : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        long paymentId = -1;

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            this.Page.Form.Enctype = "multipart/form-data";

            using (var db = new CandidateDataManager())
            {
                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                    int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                    if (AllStepInOneTime == 1)
                    {
                        paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID).Select(x => x.PaymentId).FirstOrDefault();
                    }
                    else
                    {
                        paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID && x.IsPaid == true).Select(x => x.PaymentId).FirstOrDefault();

                    }
                }// end if(obj != null && obj.ID > 0)
            }// end using


            if (!IsPostBack)
            {
                divNidBirth.Visible = false;
                divDisabledAssistant.Visible = false;
                divSpecialAssistantOther.Visible = false;
                divHall.Visible = false;

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
                DDLHelper.Bind<DAL.Country>(ddlNationality, db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);
                ddlNationality.SelectedValue = "2";
                DDLHelper.Bind<DAL.Language>(ddlLanguage, db.AdmissionDB.Languages.Where(a => a.IsActive == true).OrderBy(a => a.LanguageName).ToList(), "LanguageName", "ID", EnumCollection.ListItemType.MotherTongue);
                ddlLanguage.SelectedValue = "20";
                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).OrderBy(a => a.GenderName).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                DDLHelper.Bind<DAL.MaritalStatu>(ddlMaritalStatus, db.AdmissionDB.MaritalStatus.Where(a => a.IsActive == true).OrderBy(a => a.ID).ToList(), "MaritalStatus", "ID", EnumCollection.ListItemType.MaritalStatus);
                DDLHelper.Bind<DAL.BloodGroup>(ddlBloodGroup, db.AdmissionDB.BloodGroups.OrderBy(a => a.ID).ToList(), "BloodGroupName", "ID", EnumCollection.ListItemType.BloodGroup);
                DDLHelper.Bind<DAL.Religion>(ddlReligion, db.AdmissionDB.Religions.Where(a => a.IsActive == true).OrderBy(a => a.ID).ToList(), "ReligionName", "ID", EnumCollection.ListItemType.Religion);
                DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(a => a.OrderQuota).ToList(), "Remarks", "ID", EnumCollection.ListItemType.Quota);
                DDLHelper.Bind<DAL.QuotaType>(ddlFFQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 2).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.QuotaType>(ddlPWDQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 8).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);

                ddlQuota.SelectedValue= "7";

            }
        }

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
                    divHall.Visible = false;

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
                                programId = formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.Where(x => x.IsActive == true && x.AcaCalID == formSerial.AcaCalID).FirstOrDefault().ProgramID;
                            }
                        }
                    }
                    #endregion

                    if (educationCategoryId == 4)
                    {
                        divHall.Visible = true;

                        using (var db = new GeneralDataManager())
                        {
                            DDLHelper.Bind<DAL.QuotaType>(ddlSQQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 4 && a.Name != "Military Personnel").OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);

                        }
                    }
                    else
                    {
                        using (var db = new GeneralDataManager())
                        {
                            DDLHelper.Bind<DAL.QuotaType>(ddlSQQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 4).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);

                        }
                    }

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
                                        btnSave_Basic.Visible = !isFinalSubmit;
                                        btnUploadFile.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Basic.Visible = false;
                                        btnUploadFile.Visible = false;
                                    }

                                }
                                else
                                {
                                    btnSave_Basic.Visible = false;
                                    btnUploadFile.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Basic.Visible = false;
                                btnUploadFile.Visible = false;
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
                                        btnSave_Basic.Visible = !isFinalSubmit;
                                        btnUploadFile.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Basic.Visible = false;
                                        btnUploadFile.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnSave_Basic.Visible = false;
                                    btnUploadFile.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Basic.Visible = false;
                                btnUploadFile.Visible = false;
                            }
                            #endregion

                            #endregion
                        }
                    }
                    else
                    {
                        btnSave_Basic.Visible = false;
                        btnUploadFile.Visible = false;
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
                    //            btnSave_Basic.Enabled = false;
                    //            btnSave_Basic.Visible = false;

                    //            if (undergradCandidate.IsApproved != null)
                    //            {
                    //                if (undergradCandidate.IsApproved == true)
                    //                {
                    //                    btnSave_Basic.Enabled = false;
                    //                    btnSave_Basic.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Basic.Enabled = true;
                    //                    btnSave_Basic.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Basic.Enabled = true;
                    //                btnSave_Basic.Visible = true;
                    //            }

                    //            if (undergradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (undergradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Basic.Enabled = false;
                    //                    btnSave_Basic.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Basic.Enabled = true;
                    //                    btnSave_Basic.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Basic.Enabled = true;
                    //                btnSave_Basic.Visible = true;
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
                    //                    btnSave_Basic.Enabled = false;
                    //                    btnSave_Basic.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Basic.Enabled = true;
                    //                    btnSave_Basic.Visible = true;
                    //                }
                    //            }

                    //            if (gradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (gradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Basic.Enabled = false;
                    //                    btnSave_Basic.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Basic.Enabled = true;
                    //                    btnSave_Basic.Visible = true;
                    //                }
                    //            }
                    //        }
                    //        #endregion



                    //        #region Hide Save and Next Button for Bachelor Program Because Admission is closed
                    //        try
                    //        {
                    //            List<DAL.PropertySetup> propertySetupList = null; //new DAL.CandidateFormSl();
                    //            int educationCategoryId = -1;
                    //            using (var db = new GeneralDataManager())
                    //            {
                    //                propertySetupList = db.AdmissionDB.PropertySetups.Where(x => x.IsActive == true).ToList();
                    //            }
                    //            using (var db = new CandidateDataManager())
                    //            {
                    //                educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                    //            }

                    //            ///<summary>
                    //            ///
                    //            /// IsActive == true && IsVisible == false
                    //            /// Kew Submit Button Dekte prbe na.
                    //            /// jokon admission date sas hoea jbe
                    //            /// 
                    //            /// 
                    //            /// IsActive == true && IsVisible == true 
                    //            /// sober jnno Open thkbe. Final Submit Dileo
                    //            /// 
                    //            /// 
                    //            /// IsActive == false && IsVisible == any
                    //            /// Sober jnno Open but final Submit dile r Show korbe na tader jnno
                    //            /// 
                    //            /// </summary>


                    //            DAL.AdditionalInfo addFsModel = null;
                    //            using (var db = new CandidateDataManager())
                    //            {
                    //                addFsModel = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                    //            }

                    //            if (addFsModel != null && Convert.ToBoolean(addFsModel.IsFinalSubmit) == true)
                    //            {
                    //                btnSave_Basic.Visible = false;
                    //            }
                    //            else
                    //            {
                    //                if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).ToList().Count > 0)
                    //                {
                    //                    if (educationCategoryId == 4)
                    //                    {
                    //                        // For Bachelor
                    //                        btnSave_Basic.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
                    //                    }
                    //                    else
                    //                    {
                    //                        // For Masters
                    //                        DAL.CandidateFormSl formSl = null;
                    //                        using (var db = new CandidateDataManager())
                    //                        {
                    //                            formSl = db.GetCandidateFormSlByCandID_AD(cId);
                    //                        }

                    //                        if (formSl != null && formSl.AdmissionSetup != null)
                    //                        {
                    //                            DAL.AdmissionUnitProgram admUnitProg = null;
                    //                            using (var db = new OfficeDataManager())
                    //                            {
                    //                                admUnitProg = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == formSl.AcaCalID
                    //                                                                                            && x.EducationCategoryID == educationCategoryId
                    //                                                                                            && x.AdmissionUnitID == formSl.AdmissionSetup.AdmissionUnitID
                    //                                                                                            && x.IsActive == true).FirstOrDefault();
                    //                            }

                    //                            if (admUnitProg != null)
                    //                            {
                    //                                int programId = admUnitProg.ProgramID;

                    //                                if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).FirstOrDefault() != null)
                    //                                {
                    //                                    btnSave_Basic.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                    //                                }
                    //                                else
                    //                                {
                    //                                    btnSave_Basic.Visible = false;
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                btnSave_Basic.Visible = false;
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            btnSave_Basic.Visible = false;
                    //                        }
                    //                    }
                    //                }
                    //            }

                    //        }
                    //        catch (Exception ex)
                    //        {
                    //        }
                    //        #endregion

                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //}
                    #endregion

                    using (var db = new CandidateDataManager())
                    {
                        //basic info-------------------------------------------------------
                        DAL.BasicInfo candidate = db.GetCandidateBasicInfoByID_ND(cId);
                        if (candidate != null && candidate.ID > 0)
                        {
                            txtFirstName.Text = candidate.FirstName;
                            //txtMiddleName.Text = candidate.MiddleName;
                            //txtLastName.Text = candidate.LastName;
                            //txtNickName.Text = candidate.NickName;
                            txtDateOfBirth.Text = candidate.DateOfBirth.ToString("dd/MM/yyyy");
                            txtPlaceOfBirth.Text = candidate.PlaceOfBirth;
                            ddlNationality.SelectedValue = candidate.NationalityID.ToString();
                            ddlLanguage.SelectedValue = candidate.MotherTongueID.ToString();
                            ddlGender.SelectedValue = candidate.GenderID.ToString();
                            ddlMaritalStatus.SelectedValue = candidate.MaritalStatusID.ToString();
                            //txtNationalId.Text = candidate.NationalIdNumber;
                            ddlBloodGroup.SelectedValue = candidate.BloodGroupID.ToString();
                            txtEmail.Text = candidate.Email.ToLower();
                            //txtPhoneRes.Text = candidate.PhoneResidence;
                            //txtPhoneEmergency.Text = candidate.EmergencyPhone;
                            //txtMobile.Text = candidate.Mobile; // not needed now
                            txtMobile.Text = candidate.SMSPhone;
                            ddlReligion.SelectedValue = candidate.ReligionID.ToString();
                            ddlQuota.SelectedValue = candidate.QuotaID.ToString();  //required for BUP

                            ddlQuota_SelectedIndexChanged(null, null);

                            if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) > 0)
                            {
                                ddlNationalIdOrBirthRegistration.SelectedValue = candidate.AttributeInt1.ToString();
                            }
                            else
                            {
                                ddlNationalIdOrBirthRegistration.SelectedValue = "0";
                            }

                            ddlNationalIdOrBirthRegistration_SelectedIndexChanged(null, null);

                            if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) == 1)
                            {
                                txtNationalIdOrBirthRegistration.Text = candidate.NationalIdNumber.ToString();
                            }
                            else if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) == 2)
                            {

                                txtNationalIdOrBirthRegistration.Text = candidate.BirthRegistrationNo.ToString();
                            }
                            else { }


                            #region Hall Accomodation

                            try
                            {
                                if (candidate.AttributeBool != null)
                                    rblHallAccomodation.SelectedValue = Convert.ToBoolean(candidate.AttributeBool) == true ? "1" : "0";
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion


                            LoadCandidateDataQuota(candidate);


                        }

                        bool hasExamTypeSSCHSC = db.IsCandidateHasExamTypeSSCHSC(cId);
                        if (hasExamTypeSSCHSC == true)
                        {
                            txtFirstName.Enabled = false;
                            txtDateOfBirth.Enabled = false;
                            ddlGender.Enabled = false;
                        }
                        else
                        {
                            txtFirstName.Enabled = true;
                            txtDateOfBirth.Enabled = true;
                            ddlGender.Enabled = true;
                        }

                        #region Load Data for Exam Venue Selection
                        try
                        {
                            //int educationCategoryId = -1;
                            //educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                            if (educationCategoryId == 4)
                            {
                                List<DAL.CandidateFormSl> cfsList = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId).ToList();
                                if (cfsList != null && cfsList.Count > 0)
                                {
                                    gvFacultyList.DataSource = cfsList.OrderBy(x => x.ID).ToList();
                                    gvFacultyList.DataBind();
                                }
                                else
                                {
                                    gvFacultyList.DataSource = null;
                                    gvFacultyList.DataBind();
                                }

                                PanelExamSeatInformation.Visible = true;
                            }
                            else
                            {
                                PanelExamSeatInformation.Visible = false;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion

                    }// end using

                    #region For Certificate Program Hide Quota and Quota Related Information

                    try
                    {
                        if (programId == 65 || programId == 66)
                        {
                            ddlQuota.SelectedValue = "7";
                            ddlQuota_SelectedIndexChanged(null, null);
                            divQuota.Visible = false;
                            divQuotaDetails.Visible = false;
                        }
                        else
                        {

                            divQuota.Visible = true;
                            divQuotaDetails.Visible = true;
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion

                }// if(cId > 0)
            }// if(uId > 0)

        }

        private void LoadCandidateDataQuota(DAL.BasicInfo candidate)
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidate.ID).FirstOrDefault();

                    if (candidate.QuotaID != null && candidate.QuotaID == 4)
                    {
                        if (qi != null)
                        {
                            ddlSQQuotaType.SelectedValue = qi.QuotaTypeId.ToString();

                            #region N/A
                            //txtSQFatherOrMotherName.Text = qi.FatherMotherName;
                            //txtSQRankOrDesignation.Text = qi.RankDesignation;
                            //txtSQSenaNoOrBUPNo.Text = qi.SenaNoBUPNo;
                            ////txtServingOrRetired.Text = qi.ServingRetired;
                            //ddlSQServingOrRetired.SelectedValue = qi.ServingRetiredId.ToString();
                            //txtSQJobLocation.Text = qi.JobLocation; 
                            #endregion


                            panelQuotaInfo.Visible = true;

                            if (qi.QuotaTypeId == 1 || qi.QuotaTypeId == 2)
                            {
                                panelChildrenOfMilitaryPersonnel.Visible = true;
                                rblServingRetired.SelectedValue = qi.ServingRetiredId.ToString();

                                panelChildrenOfMilitaryPersonnelServingRetired.Visible = true;
                                if (qi.QuotaTypeId == 1 && qi.ServingRetiredId == 1)
                                {
                                    lblName1.Text = "BA/BD/Personal No";
                                    txtInput1.Text = qi.InputOne;

                                    lblName2.Text = "Present Unit";
                                    txtInput2.Text = qi.InputTwo;

                                    lblshortnoteFM.Text = "(If both serving, mention one)";

                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = true;
                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;
                                    panel_QuotaDocUpload_Tribal_Note.Visible = false;
                                }
                                else if (qi.QuotaTypeId == 1 && qi.ServingRetiredId == 2)
                                {
                                    lblName1.Text = "TS/Personal No";
                                    txtInput1.Text = qi.InputOne;

                                    lblName2.Text = "Last Unit Served";
                                    txtInput2.Text = qi.InputTwo;

                                    lblshortnoteFM.Text = "(If both served, mention one)";

                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = true;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                                    panel_QuotaDocUpload_Tribal_Note.Visible = false;
                                }
                                else if (qi.QuotaTypeId == 2 && qi.ServingRetiredId == 1)
                                {
                                    lblName1.Text = "BUP No";
                                    txtInput1.Text = qi.InputOne;

                                    lblName2.Text = "Present Office/Department";
                                    txtInput2.Text = qi.InputTwo;

                                    lblshortnoteFM.Text = "(If both serving, mention one)";

                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = true;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                                    panel_QuotaDocUpload_Tribal_Note.Visible = false;
                                }
                                else if (qi.QuotaTypeId == 2 && qi.ServingRetiredId == 2)
                                {
                                    lblName1.Text = "BUP No";
                                    txtInput1.Text = qi.InputOne;

                                    lblName2.Text = "Last Office/Department Served";
                                    txtInput2.Text = qi.InputTwo;

                                    lblshortnoteFM.Text = "(If both served, mention one)";

                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = true;
                                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                                    panel_QuotaDocUpload_Tribal_Note.Visible = false;
                                }
                                else
                                {
                                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;

                                    lblName1.Text = string.Empty;
                                    lblName2.Text = string.Empty;
                                    lblshortnoteFM.Text = string.Empty;

                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                                    panel_QuotaDocUpload_Tribal_Note.Visible = false;
                                }


                                txtFatherName.Text = qi.FatherName;
                                txtFatherRankDesignation.Text = qi.FatherRankDesignation;
                                //txtMotherName.Text = qi.MotherName;
                                //txtMotherRankDesignation.Text = qi.MotherRankDesignation;


                            }
                            else if (qi.QuotaTypeId == 3)
                            {
                                panelChildrenOfSittingMembersOfBUPGoverningBodies.Visible = true;
                                #region N/A
                                //if (!string.IsNullOrEmpty(qi.SenateCommitteeMemberId.ToString()))
                                //{
                                //    ddlSenateCommitteeMember.SelectedValue = qi.SenateCommitteeMemberId.ToString();
                                //}
                                //if (!string.IsNullOrEmpty(qi.SyndicateCommitteeMemberId.ToString()))
                                //{
                                //    ddlSyndicateCommitteeMember.SelectedValue = qi.SyndicateCommitteeMemberId.ToString();
                                //}
                                //if (!string.IsNullOrEmpty(qi.AcademicCouncilMemberId.ToString()))
                                //{
                                //    ddlAcademicCouncilMember.SelectedValue = qi.AcademicCouncilMemberId.ToString();
                                //}
                                //if (!string.IsNullOrEmpty(qi.FinanceCommitteeMemberId.ToString()))
                                //{
                                //    ddlFinanceCommitteeMember.SelectedValue = qi.FinanceCommitteeMemberId.ToString();
                                //} 
                                #endregion

                                if (qi.GoverningBodiesTypeId != null)
                                {
                                    rblGoverningBodie.SelectedValue = qi.GoverningBodiesTypeId.ToString();
                                }

                                if (qi.GoverningBodiesTypeId != null && qi.GoverningBodiesTypeId == 1)
                                {
                                    DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 1).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                                    ddlGoverningBodie.SelectedValue = qi.SenateCommitteeMemberId.ToString();
                                }
                                else if (qi.GoverningBodiesTypeId != null && qi.GoverningBodiesTypeId == 2)
                                {
                                    DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 2).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                                    ddlGoverningBodie.SelectedValue = qi.SyndicateCommitteeMemberId.ToString();
                                }
                                else if (qi.GoverningBodiesTypeId != null && qi.GoverningBodiesTypeId == 3)
                                {
                                    DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 3).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                                    ddlGoverningBodie.SelectedValue = qi.AcademicCouncilMemberId.ToString();
                                }
                                else if (qi.GoverningBodiesTypeId != null && qi.GoverningBodiesTypeId == 4)
                                {
                                    DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 4).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                                    ddlGoverningBodie.SelectedValue = qi.FinanceCommitteeMemberId.ToString();
                                }
                                else
                                {
                                    btnClearRadioButton_Click(null, null);
                                }


                            }
                            else
                            {

                            }



                            panelQuotaNote.Visible = true;

                            panelQuotaNoteSpecialQuota.Visible = true;

                            panelQuotaNoteFreedomFighter.Visible = false;
                            panelFreedomFighterInfo.Visible = false;

                            panelQuotaNotePersonWithDisability.Visible = false;
                            panelPersonWithDisabilityInfo.Visible = false;

                            if (qi.QuotaTypeId == 1 || qi.QuotaTypeId == 2 || qi.QuotaTypeId == 7)
                            {
                                panelQuotaDocUpload.Visible = true;
                                LoadQuotaDocument();
                            }
                            else
                            {
                                panelQuotaDocUpload.Visible = false;
                            }

                        }
                    }
                    else if (candidate.QuotaID != null && candidate.QuotaID == 2) //Freedom Fighter
                    {
                        //DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidate.ID).FirstOrDefault();
                        if (qi != null)
                        {
                            ddlFFQuotaType.SelectedValue = qi.QuotaTypeId.ToString();
                            txtFFName.Text = qi.FatherMotherName;
                            txtFFQFreedomFighterNo.Text = qi.FreedomFighterNo;
                            txtFFQGazetteReferenceNo.Text = qi.GazetteReferenceNo;


                            panelQuotaNote.Visible = true;

                            panelQuotaNoteSpecialQuota.Visible = false;
                            panelQuotaInfo.Visible = false;

                            panelQuotaNoteFreedomFighter.Visible = true;
                            panelFreedomFighterInfo.Visible = true;

                            panelQuotaNotePersonWithDisability.Visible = false;
                            panelPersonWithDisabilityInfo.Visible = false;

                            panelQuotaDocUpload.Visible = true;
                            LoadQuotaDocument();
                        }
                    }
                    else if (candidate.QuotaID != null && candidate.QuotaID == 8) //Person with Disability (Physical)
                    {
                        //DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidate.ID).FirstOrDefault();
                        if (qi != null)
                        {
                            //ddlPWDQuotaType.SelectedValue = qi.QuotaTypeId.ToString();
                            txtPWDDisabilityName.Text = qi.DisabilityName;

                            panelQuotaNote.Visible = true;

                            panelQuotaNoteSpecialQuota.Visible = false;
                            panelQuotaInfo.Visible = false;

                            panelQuotaNoteFreedomFighter.Visible = false;
                            panelFreedomFighterInfo.Visible = false;

                            panelQuotaNotePersonWithDisability.Visible = true;
                            panelPersonWithDisabilityInfo.Visible = true;

                            panelQuotaDocUpload.Visible = true;
                            LoadQuotaDocument();

                            panel_QuotaDocUpload_PersonWithDisability_Note.Visible = true;
                            panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = true;

                            LoadSpecialAssistantInformation(cId);

                        }
                    }
                    else if (candidate.QuotaID != null && candidate.QuotaID == 6) //Tribal
                    {
                        panelQuotaNote.Visible = false;

                        panelQuotaNoteSpecialQuota.Visible = false;

                        panelQuotaInfo.Visible = false;

                        panelChildrenOfMilitaryPersonnel.Visible = false;
                        panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;
                        panelChildrenOfSittingMembersOfBUPGoverningBodies.Visible = false;

                        panelQuotaNoteFreedomFighter.Visible = false;
                        panelFreedomFighterInfo.Visible = false;

                        panelQuotaNotePersonWithDisability.Visible = false;
                        panelPersonWithDisabilityInfo.Visible = false;

                        panelQuotaDocUpload.Visible = true;
                        LoadQuotaDocument();

                        panel_QuotaDocUpload_Tribal_Note.Visible = true;
                    }
                    else
                    {
                        panelQuotaNote.Visible = false;

                        panelQuotaNoteSpecialQuota.Visible = false;

                        panelQuotaInfo.Visible = false;

                        panelChildrenOfMilitaryPersonnel.Visible = false;
                        panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;
                        panelChildrenOfSittingMembersOfBUPGoverningBodies.Visible = false;

                        panelQuotaNoteFreedomFighter.Visible = false;
                        panelFreedomFighterInfo.Visible = false;

                        panelQuotaNotePersonWithDisability.Visible = false;
                        panelPersonWithDisabilityInfo.Visible = false;

                        panelQuotaDocUpload.Visible = false;

                        panel_QuotaDocUpload_Tribal_Note.Visible = false;

                    }
                }

            }
            catch (Exception ex)
            {
            }
        }



        #region N/A -- DisableSaveButtonIfApproved
        //private void DisableSaveButtonIfApproved(long candidateId)
        //{
        //    if (candidateId > 0)
        //    {
        //        List<DAL.ApprovedList> candidateApprovedList = null;
        //        using (var db = new CandidateDataManager())
        //        {
        //            candidateApprovedList = db.AdmissionDB.ApprovedLists
        //                .Where(c => c.CandidateID == candidateId && c.IsApproved == true)
        //                .ToList();
        //        }

        //        if (candidateApprovedList != null) // if approved candidate found
        //        {
        //            if (candidateApprovedList.Count() > 0)
        //            {
        //                btnSave_Basic.Enabled = false;
        //            }
        //            else
        //            {
        //                btnSave_Basic.Enabled = true;
        //            }
        //        }
        //        else // else approved candidate not found
        //        {
        //            btnSave_Basic.Enabled = true;
        //        }
        //    }
        //    else //if candidate id is less than zero
        //    {
        //        btnSave_Basic.Visible = false;
        //    }
        //} 
        #endregion


        private Dictionary<int, string> ValidateAllFieldIsGiven(long cId)
        {

            Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
            int i = 1;

            if (txtFirstName.Text.ToString() == "")
            {
                dictErrorList.Add(i++, "Name is Empty !!");
            }

            //if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            //{
            //    dictErrorList.Add(i++, "Date of Birth is Empty !!");
            //}

            //if (txtEmail.Text.ToString() == "")
            //{
            //    dictErrorList.Add(i++, "Email is Empty !!");
            //}

            //if (txtMobile.Text.ToString() == "")
            //{
            //    dictErrorList.Add(i++, "Mobile No. for SMS is Empty !!");
            //}



            //if (ddlGender.SelectedValue == "-1")
            //{
            //    dictErrorList.Add(i++, "Gender hasn't Selected !!");
            //}



            //if (ddlMaritalStatus.SelectedValue == "-1")
            //{
            //    dictErrorList.Add(i++, "Marital Status hasn't Selected !!");
            //}

            //if (txtNationalId.Text.ToString() == "")
            //{
            //    dictErrorList.Add(i++, "National ID No. is Empty !!");
            //}

            if (ddlReligion.SelectedValue == "-1")
            {
                dictErrorList.Add(i++, "Religion hasn't Selected !!");
            }

            if (ddlBloodGroup.SelectedValue == "-1")
            {
                dictErrorList.Add(i++, "Blood Group hasn't Selected !!");
            }


            int educationCategoryId = -1;
            using (var db = new CandidateDataManager())
            {
                educationCategoryId = db.GetCandidateEducationCategoryID(cId);
            }
            if (educationCategoryId == 4)
            {
                int countSelectedDistrict = 0;
                int districtId = -1;
                foreach (GridViewRow gvrow in gvFacultyList.Rows)
                {
                    DropDownList ddlDistrict = (DropDownList)gvrow.FindControl("ddlDistrict");

                    //districtId = Convert.ToInt32(ddlDistrict.SelectedValue);

                    districtId = 1;

                    if (districtId > 0)
                    {
                        countSelectedDistrict++;
                    }

                }
                if (countSelectedDistrict == 0)
                {
                    dictErrorList.Add(i++, "Please select at least one District for Exam Seat !!");
                }
            }


            //if (txtPlaceOfBirth.Text.ToString() == "")
            //{
            //    dictErrorList.Add(i++, "Place Of Birth is Empty !!");
            //}

            //if (ddlNationality.SelectedValue == "-1")
            //{
            //    dictErrorList.Add(i++, "Nationality hasn't Selected !!");
            //}

            //if (ddlLanguage.SelectedValue == "-1")
            //{
            //    dictErrorList.Add(i++, "Mother Tongue hasn't Selected !!");
            //}


            return dictErrorList;

        }


        protected void btnSave_Basic_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");


            DAL.BasicInfo candidate = new DAL.BasicInfo();

            candidate.FirstName = txtFirstName.Text.Trim().ToUpper();

            //if (string.IsNullOrEmpty(candidate.MiddleName))
            //    candidate.MiddleName = "";

            //if (string.IsNullOrEmpty(candidate.LastName))
            //    candidate.LastName = "";

            //if (string.IsNullOrEmpty(candidate.Mobile))
            //    candidate.Mobile = "";

            //if (string.IsNullOrEmpty(candidate.PhoneResidence))
            //    candidate.PhoneResidence = "";

            //if (string.IsNullOrEmpty(candidate.EmergencyPhone))
            //    candidate.EmergencyPhone = "";

            //if (string.IsNullOrEmpty(candidate.SMSPhone))
            //    candidate.SMSPhone = "";

            //if (string.IsNullOrEmpty(candidate.Email))
            //    candidate.Email = "";


            //candidate.MiddleName = txtMiddleName.Text.ToUpper();
            //candidate.LastName = txtLastName.Text.ToUpper();
            candidate.Mobile = txtMobile.Text.Trim();
            //candidate.PhoneResidence = txtPhoneRes.Text;
            //candidate.EmergencyPhone = txtPhoneEmergency.Text;
            candidate.SMSPhone = txtMobile.Text.Trim();
            candidate.Email = txtEmail.Text.ToLower().Trim();


            try
            {
                candidate.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);

            }
            catch (Exception ex)
            {
                candidate.DateOfBirth = DateTime.Now;

            }
            //candidate.PlaceOfBirth = txtPlaceOfBirth.Text.Trim();

            candidate.NationalityID = ddlNationality.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlNationality.SelectedValue);
            candidate.MotherTongueID = ddlLanguage.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlLanguage.SelectedValue);
            candidate.MaritalStatusID = ddlMaritalStatus.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlMaritalStatus.SelectedValue);
            candidate.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
            candidate.QuotaID = ddlQuota.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlQuota.SelectedValue); //only for BUP
            candidate.BloodGroupID = ddlBloodGroup.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlBloodGroup.SelectedValue);
            candidate.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);
            candidate.GuardianPhone = null;

            if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 1)
            {
                candidate.NationalIdNumber = txtNationalIdOrBirthRegistration.Text.Trim();
                candidate.AttributeInt1 = 1;
                candidate.BirthRegistrationNo = null;
            }
            else if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 2)
            {
                candidate.BirthRegistrationNo = txtNationalIdOrBirthRegistration.Text.Trim();
                candidate.AttributeInt1 = 2;
                candidate.NationalIdNumber = null;
            }
            else { }


            try
            {

                int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);

                if (quotaId <= 0)
                {
                    MessageView("Please select Quota!", "fail");
                    return;
                }

                if (quotaId == 7) //7 = Non-Quota //quotaId == 4 || quotaId == 2 || quotaId == 8 //Special Quota - Freedom Fighter - Person with Disability (Physical)
                {
                    //Non-Quota
                }
                else
                {
                    Dictionary<int, string> dictErrorList = new Dictionary<int, string>();

                    #region Check all field is fillup in Form
                    dictErrorList = ValidateQuotaField();

                    if (dictErrorList.Count > 0)
                    {
                        string massageError = "";
                        foreach (var tData in dictErrorList)
                        {
                            massageError = massageError + tData.Key.ToString() + ") " + tData.Value.ToString() + "<br/>";
                        }

                        MessageView(massageError, "fail");
                        return;
                    }
                    #endregion
                }

                if (uId > 0)
                {

                    Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
                    dictErrorList = ValidateAllFieldIsGiven(cId);
                    if (dictErrorList.Count > 0)
                    {
                        string massageError = "";
                        foreach (var tData in dictErrorList)
                        {
                            massageError = massageError + tData.Key.ToString() + ") " + tData.Value.ToString() + "<br/>";
                        }

                        MessageView(massageError, "fail");
                        return;
                    }

                    string logOldObject = string.Empty;
                    string logNewObject = string.Empty;

                    DAL.BasicInfo obj = new DAL.BasicInfo();
                    using (var db = new CandidateDataManager())
                    {
                        obj = db.GetCandidateBasicInfoByUserID_ND(uId);

                        cId = obj.ID;

                        //to retain old info in log.
                        //logOldObject = ObjectToString.ConvertToString(obj);
                        logOldObject = GenerateLogStringFromObject(obj);


                    }// end using

                    #region Hall Accomodation Check for Only bachelor

                    if (obj.ID > 0)
                    {
                        int educationCategoryId = -1;
                        using (var db = new CandidateDataManager())
                        {
                            try
                            {
                                educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                            }
                            catch (Exception ex)
                            {
                            }
                        }

                        if (educationCategoryId == 4)
                        {
                            int selectedValue = -1;
                            try
                            {
                                selectedValue = Convert.ToInt32(rblHallAccomodation.SelectedValue);
                            }
                            catch (Exception ex)
                            {
                            }
                            if (selectedValue == -1)
                            {
                                MessageView("Please select Hall Accomodation option !!", "fail");
                                return;
                            }
                            candidate.AttributeBool = selectedValue == 1 ? true : false;
                        }
                    }
                    #endregion

                    if (obj != null && obj.ID > 0)  //update if object exists
                    {
                        candidate.ID = obj.ID;
                        candidate.CreatedBy = obj.CreatedBy;
                        candidate.DateCreated = obj.DateCreated;
                        candidate.ModifiedBy = obj.ID;
                        candidate.DateModified = DateTime.Now;
                        candidate.CandidateUserID = obj.CandidateUserID;

                        using (var updateDb = new CandidateDataManager())
                        {
                            updateDb.Update<DAL.BasicInfo>(candidate);

                            SaveOrUpdateQuotaInfo(candidate.ID);

                            //to retain new info in long
                            logNewObject = GenerateLogStringFromObject(candidate); //logNewObject = ObjectToString.ConvertToString(candidate);

                        }

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "btnSave_Basic_Click";
                            //dLog.PageName = "ApplicationBasic.aspx";
                            //dLog.OldData = logOldObject;
                            //dLog.NewData = logNewObject;
                            //dLog.UserId = uId;
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                            //LogWriter.DataLogWriter(dLog);

                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.CandidateId = cId;
                            dLog.EventName = "Basic Info Update (Candidate)";
                            dLog.PageName = "ApplicationBasic.aspx";
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
                    else //else save
                    {

                        candidate.CreatedBy = candidate.ID;
                        candidate.DateCreated = DateTime.Now;

                        using (var insertDb = new CandidateDataManager())
                        {
                            insertDb.Insert<DAL.BasicInfo>(candidate);

                            SaveOrUpdateQuotaInfo(candidate.ID);

                            logNewObject = GenerateLogStringFromObject(candidate);
                        }

                        string newLogObject2 = ObjectToString.ConvertToString(candidate);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "btnSave_Basic_Click";
                            //dLog.PageName = "ApplicationBasic.aspx";
                            //dLog.OldData = null;
                            //dLog.NewData = logNewObject;
                            //dLog.UserId = uId;
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                            //LogWriter.DataLogWriter(dLog);

                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.CandidateId = cId;
                            dLog.EventName = "Basic Info Update (Candidate)";
                            dLog.PageName = "ApplicationBasic.aspx";
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

                    } // end if-else




                    #region Exam Venue Selection
                    try
                    {
                        int educationCategoryId = -1;
                        using (var db = new CandidateDataManager())
                        {
                            educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                        }

                        if (educationCategoryId == 4)
                        {
                            int districtId = -1;
                            long admissionSetupId = -1;
                            long admUnitId = -1;
                            foreach (GridViewRow gvrow in gvFacultyList.Rows)
                            {
                                Label lblAdmissionUnitId = (Label)gvrow.FindControl("lblAdmissionUnitId");
                                Label lblAdmissionSetupId = (Label)gvrow.FindControl("lblAdmissionSetupId");
                                DropDownList ddlDistrict = (DropDownList)gvrow.FindControl("ddlDistrict");

                                //districtId = Convert.ToInt32(ddlDistrict.SelectedValue);
                                districtId = 1;
                                admissionSetupId = Convert.ToInt64(lblAdmissionSetupId.Text);
                                admUnitId = Convert.ToInt64(lblAdmissionUnitId.Text);

                                int acaCalId = -1;
                                using (var db = new OfficeDataManager())
                                {
                                    acaCalId = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == admissionSetupId).Select(x => x.AcaCalID).FirstOrDefault();
                                }

                                if (districtId > 0 && admUnitId > 0 && acaCalId > 0)
                                {
                                    DAL.CandidateFacultyWiseDistrictSeat model = null; //new DAL.CandidateFacultyWiseDistrictSeat();
                                    using (var insertDb = new CandidateDataManager())
                                    {
                                        model = insertDb.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == cId
                                                                                                             && x.AdmissionSetupId == admissionSetupId).FirstOrDefault();
                                    }

                                    if (model != null)
                                    {
                                        #region Update
                                        if (model.DistrictId == districtId)
                                        {

                                        }
                                        else
                                        {
                                            bool isBothIncreaseAndDecreaseAreOk = true;
                                            int newSeatCount = -1;
                                            int oldSeatCount = -1;


                                            DAL.DistrictSeatLimit dslNew = null;
                                            DAL.DistrictSeatLimit dslOld = null;

                                            using (var db = new OfficeDataManager())
                                            {
                                                dslNew = db.AdmissionDB.DistrictSeatLimits.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == districtId && x.IsActive == true).FirstOrDefault();
                                                dslOld = db.AdmissionDB.DistrictSeatLimits.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == model.DistrictId && x.IsActive == true).FirstOrDefault();
                                            }


                                            if (dslNew != null)
                                            {
                                                newSeatCount = Convert.ToInt32(dslNew.SeatFillup);
                                                newSeatCount = newSeatCount + 1;
                                                if (newSeatCount > 0 && newSeatCount <= Convert.ToInt32(dslNew.SeatLimit))
                                                {
                                                    dslNew.SeatFillup = newSeatCount;

                                                    using (var db = new OfficeDataManager())
                                                    {
                                                        db.Update<DAL.DistrictSeatLimit>(dslNew);
                                                    }
                                                }
                                                else
                                                {
                                                    isBothIncreaseAndDecreaseAreOk = false;

                                                    string newDistrictName = "";
                                                    List<DAL.ViewModels.DistrictSeatLimitVM> dslvmList = null;
                                                    using (var db = new OfficeDataManager())
                                                    {
                                                        dslvmList = db.GetDistrictSeatLimitVM(acaCalId);
                                                    }
                                                    if (dslvmList != null && dslvmList.Count > 0)
                                                    {
                                                        newDistrictName = dslvmList.Where(x => x.DistrictId == dslNew.DistrictId).Select(x => x.DistrictName).FirstOrDefault();
                                                    }

                                                    //lblMessageBasic.Text = "No more seat is available for District: " + newDistrictName + ". Please select another district.";
                                                    //messagePanel_Basic.CssClass = "alert alert-danger";
                                                    //messagePanel_Basic.Visible = true;
                                                    MessageView("No more seat is available for District: " + newDistrictName + ". Please select another district.", "fail");
                                                    return;
                                                }

                                            }




                                            if (dslOld != null)
                                            {
                                                oldSeatCount = Convert.ToInt32(dslOld.SeatFillup);
                                                oldSeatCount = oldSeatCount - 1;
                                                if (oldSeatCount >= 0)
                                                {
                                                    dslOld.SeatFillup = oldSeatCount;

                                                    using (var db = new OfficeDataManager())
                                                    {
                                                        db.Update<DAL.DistrictSeatLimit>(dslOld);
                                                    }
                                                }
                                                else
                                                {
                                                    isBothIncreaseAndDecreaseAreOk = false;
                                                }

                                            }


                                            if (isBothIncreaseAndDecreaseAreOk == true)
                                            {
                                                //model.CandidateId = cId;
                                                //model.AdmissionSetupId = admissionSetupId;
                                                model.DistrictId = districtId;
                                                model.ModifiedBy = cId;
                                                model.ModifiedDate = DateTime.Now;

                                                using (var insertDb = new CandidateDataManager())
                                                {
                                                    insertDb.Update<DAL.CandidateFacultyWiseDistrictSeat>(model);
                                                }
                                            }


                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Create
                                        //int newSeatCount = -1;

                                        DAL.DistrictSeatLimit dslNew = null;

                                        using (var db = new OfficeDataManager())
                                        {
                                            dslNew = db.AdmissionDB.DistrictSeatLimits.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == districtId && x.IsActive == true).FirstOrDefault();
                                        }


                                        if (dslNew != null)
                                        {
                                            #region N/A
                                            //newSeatCount = Convert.ToInt32(dslNew.SeatLimit);
                                            //newSeatCount = newSeatCount - 1;
                                            //if (newSeatCount >= 0)
                                            //{
                                            //    dslNew.SeatLimit = newSeatCount;

                                            //    using (var db = new OfficeDataManager())
                                            //    {
                                            //        db.Update<DAL.DistrictSeatLimit>(dslNew);
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    string newDistrictName = "";
                                            //    List<DAL.ViewModels.DistrictSeatLimitVM> dslvmList = null;
                                            //    using (var db = new OfficeDataManager())
                                            //    {
                                            //        dslvmList = db.GetDistrictSeatLimitVM();
                                            //    }
                                            //    if (dslvmList != null && dslvmList.Count > 0)
                                            //    {
                                            //        newDistrictName = dslvmList.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == dslNew.DistrictId).Select(x => x.DistrictName).FirstOrDefault();
                                            //    }

                                            //    lblMessageBasic.Text = "No more seat is available for District: " + newDistrictName + ". Please select another district."; ;
                                            //    messagePanel_Basic.CssClass = "alert alert-danger";
                                            //    messagePanel_Basic.Visible = true;
                                            //    return;
                                            //} 
                                            #endregion

                                            int seatLimit = (int)dslNew.SeatLimit;
                                            int seatFillup = (int)dslNew.SeatFillup;

                                            seatFillup = seatFillup + 1;

                                            if (seatFillup > seatLimit)
                                            {

                                            }
                                            else
                                            {
                                                dslNew.SeatFillup = seatFillup;

                                                using (var db = new OfficeDataManager())
                                                {
                                                    db.Update<DAL.DistrictSeatLimit>(dslNew);
                                                }


                                                #region Insert -- Candidate Faculty Wise District Seat
                                                DAL.CandidateFacultyWiseDistrictSeat modelNew = new DAL.CandidateFacultyWiseDistrictSeat();
                                                modelNew.CandidateId = cId;
                                                modelNew.AdmissionSetupId = admissionSetupId;
                                                modelNew.DistrictId = districtId;
                                                modelNew.CreatedBy = cId;
                                                modelNew.CreatedDate = DateTime.Now;

                                                using (var insertDb = new CandidateDataManager())
                                                {
                                                    insertDb.Insert<DAL.CandidateFacultyWiseDistrictSeat>(modelNew);
                                                }
                                                #endregion
                                            }

                                        }
                                        #endregion
                                    }

                                }
                                else
                                {
                                    //lblMessageBasic.Text = "Please Select all Faculties Exam Seat Venue !!";
                                    //messagePanel_Basic.CssClass = "alert alert-danger";
                                    //messagePanel_Basic.Visible = true;
                                    MessageView("Please Select all Faculties Exam Seat Venue !!", "fail");
                                    return;
                                }

                            } //END: foreach
                        }
                    }
                    catch (Exception ex)
                    {
                        //lblMessageBasic.Text = "Failed to Update Exam Seat Information!! Exception: " + ex.Message.ToString();
                        //messagePanel_Basic.CssClass = "alert alert-danger";
                        //messagePanel_Basic.Visible = true;
                        MessageView("Failed to Update Exam Seat Information!! Exception: " + ex.Message.ToString(), "fail");
                        return;
                    }
                    #endregion



                }
                //lblMessageBasic.Text = "Basic Info updated successfully.";
                //messagePanel_Basic.CssClass = "alert alert-success";
                //messagePanel_Basic.Visible = true;

                #region Additional Information Table insert
                int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                if (AllStepInOneTime == 1)
                {
                    try
                    {
                        DAL.AdditionalInfo addInfoObj = null;
                        using (var db = new CandidateDataManager())
                        {
                            addInfoObj = db.GetAdditionalInfoByCandidateID_ND(cId);
                        }

                        if (addInfoObj == null)
                        {
                            DAL.AdditionalInfo _newAddInfo = new DAL.AdditionalInfo();
                            _newAddInfo.CandidateID = cId;
                            _newAddInfo.IsFinalSubmit = false;
                            _newAddInfo.IsEnrolled = false;
                            _newAddInfo.CreatedBy = cId;
                            _newAddInfo.DateCreated = DateTime.Now;

                            using (var dbInsert = new CandidateDataManager())
                            {
                                dbInsert.Insert<DAL.AdditionalInfo>(_newAddInfo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                #endregion

                MessageView("Basic Info updated successfully.", "success");

                //Response.Redirect("ApplicationPriorityS.aspx", false);

                btnNext_Click(null, null);
            }
            catch (Exception ex)
            {
                ////Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X001TC&type=danger", false);
                //lblMessageBasic.Text = "Unable to save/update candidate information. Error Code : F01X001TC";
                //messagePanel_Basic.CssClass = "alert alert-danger";
                //messagePanel_Basic.Visible = true;

                MessageView("Unable to save/update candidate information. Error Code : F01X001TC", "fail");
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            //btnSave_Basic_Click(sender, e);
            MessageView("", "clear");
            try
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
                        List<DAL.SPGetCandidateEducationCategoryByCandidateID_Result> list = null;
                        using (var db = new CandidateDataManager())
                        {
                            list = db.AdmissionDB.SPGetCandidateEducationCategoryByCandidateID(cId).ToList();
                        }

                        if (list != null)
                        {
                            if (list.Count > 0)
                            {
                                #region Breadcrumbs for Bachelor and Masters
                                if (list.FirstOrDefault().EduCatID == 4)
                                {
                                    Response.Redirect("ApplicationEducation.aspx", false);
                                }
                                else
                                {
                                    Response.Redirect("ApplicationEducation.aspx", false);
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView(ex.Message.ToString(), "fail");
            }

        }

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessageBasic.Text = string.Empty;
                lblMessageBasic.Text = msg.ToString();
                lblMessageBasic.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessageBasic.Attributes.CssStyle.Add("color", "green");

                messagePanel_Basic.Visible = true;
                messagePanel_Basic.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessageBasic.Text = string.Empty;
                lblMessageBasic.Text = msg.ToString();
                lblMessageBasic.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessageBasic.Attributes.CssStyle.Add("color", "crimson");

                messagePanel_Basic.Visible = true;
                messagePanel_Basic.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessageBasic.Text = string.Empty;
                messagePanel_Basic.Visible = false;
            }

        }

        protected void ddlQuota_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");
            try
            {
                int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);

                if (quotaId == 4) //Special Quota
                {
                    panelQuotaNote.Visible = true;

                    panelQuotaNoteSpecialQuota.Visible = true;
                    panelQuotaInfo.Visible = true;

                    panelQuotaNoteFreedomFighter.Visible = false;
                    panelFreedomFighterInfo.Visible = false;

                    panelQuotaNotePersonWithDisability.Visible = false;
                    panelPersonWithDisabilityInfo.Visible = false;

                    panelQuotaDocUpload.Visible = false;
                    //LoadQuotaDocument();

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                    panel_QuotaDocUpload_Tribal_Note.Visible = false;


                    #region N/A -- If Quota change, then Delete previous Uploaded files
                    //try
                    //{
                    //    List<DAL.QuotaDocument> qdList = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        qdList = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId != quotaId).ToList();
                    //    }
                    //    if (qdList != null && qdList.Count > 0)
                    //    {
                    //        foreach (var tData in qdList)
                    //        {
                    //            #region Delete File From Folder
                    //            try
                    //            {
                    //                if (File.Exists(Server.MapPath(tData.URL)))
                    //                {
                    //                    File.Delete(Server.MapPath(tData.URL));
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion

                    //            #region Delete Data from DB
                    //            try
                    //            {
                    //                using (var db = new CandidateDataManager())
                    //                {
                    //                    db.Delete<DAL.QuotaDocument>(tData);
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion
                    //        }

                    //        LoadQuotaDocument();
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    #endregion


                }
                else if (quotaId == 2) //Freedom Fighter
                {
                    ddlFFQuotaType.SelectedValue = "4";
                    panelQuotaNote.Visible = true;

                    panelQuotaNoteSpecialQuota.Visible = false;
                    panelQuotaInfo.Visible = false;

                    panelQuotaNoteFreedomFighter.Visible = true;
                    panelFreedomFighterInfo.Visible = true;

                    panelQuotaNotePersonWithDisability.Visible = false;
                    panelPersonWithDisabilityInfo.Visible = false;

                    panelQuotaDocUpload.Visible = true;
                    LoadQuotaDocument();

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                    panel_QuotaDocUpload_Tribal_Note.Visible = false;


                    #region N/A -- If Quota change, then Delete previous Uploaded files
                    //try
                    //{
                    //    List<DAL.QuotaDocument> qdList = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        qdList = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId != quotaId).ToList();
                    //    }
                    //    if (qdList != null && qdList.Count > 0)
                    //    {
                    //        foreach (var tData in qdList)
                    //        {
                    //            #region Delete File From Folder
                    //            try
                    //            {
                    //                if (File.Exists(Server.MapPath(tData.URL)))
                    //                {
                    //                    File.Delete(Server.MapPath(tData.URL));
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion

                    //            #region Delete Data from DB
                    //            try
                    //            {
                    //                using (var db = new CandidateDataManager())
                    //                {
                    //                    db.Delete<DAL.QuotaDocument>(tData);
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion
                    //        }

                    //        LoadQuotaDocument();
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    #endregion

                }
                else if (quotaId == 8) //Person with Disability (Physical)
                {
                    panelQuotaNote.Visible = true;

                    panelQuotaNoteSpecialQuota.Visible = false;
                    panelQuotaInfo.Visible = false;

                    panelQuotaNoteFreedomFighter.Visible = false;
                    panelFreedomFighterInfo.Visible = false;

                    panelQuotaNotePersonWithDisability.Visible = true;
                    panelPersonWithDisabilityInfo.Visible = true;

                    panelQuotaDocUpload.Visible = true;
                    LoadQuotaDocument();

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = true;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = true;

                    panel_QuotaDocUpload_Tribal_Note.Visible = false;

                    #region N/A -- If Quota change, then Delete previous Uploaded files
                    //try
                    //{
                    //    List<DAL.QuotaDocument> qdList = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        qdList = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId != quotaId).ToList();
                    //    }
                    //    if (qdList != null && qdList.Count > 0)
                    //    {
                    //        foreach (var tData in qdList)
                    //        {
                    //            #region Delete File From Folder
                    //            try
                    //            {
                    //                if (File.Exists(Server.MapPath(tData.URL)))
                    //                {
                    //                    File.Delete(Server.MapPath(tData.URL));
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion

                    //            #region Delete Data from DB
                    //            try
                    //            {
                    //                using (var db = new CandidateDataManager())
                    //                {
                    //                    db.Delete<DAL.QuotaDocument>(tData);
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion
                    //        }

                    //        LoadQuotaDocument();
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    #endregion

                }
                else if (quotaId == 6) //Tribal
                {
                    panelQuotaNote.Visible = false;

                    panelQuotaNoteSpecialQuota.Visible = false;
                    panelQuotaInfo.Visible = false;

                    panelQuotaNoteFreedomFighter.Visible = false;
                    panelFreedomFighterInfo.Visible = false;

                    panelQuotaNotePersonWithDisability.Visible = false;
                    panelPersonWithDisabilityInfo.Visible = false;

                    panelQuotaDocUpload.Visible = true;
                    LoadQuotaDocument();

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                    panel_QuotaDocUpload_Tribal_Note.Visible = true;

                    #region N/A -- If Quota change, then Delete previous Uploaded files
                    //try
                    //{
                    //    List<DAL.QuotaDocument> qdList = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        qdList = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId != quotaId).ToList();
                    //    }
                    //    if (qdList != null && qdList.Count > 0)
                    //    {
                    //        foreach (var tData in qdList)
                    //        {
                    //            #region Delete File From Folder
                    //            try
                    //            {
                    //                if (File.Exists(Server.MapPath(tData.URL)))
                    //                {
                    //                    File.Delete(Server.MapPath(tData.URL));
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion

                    //            #region Delete Data from DB
                    //            try
                    //            {
                    //                using (var db = new CandidateDataManager())
                    //                {
                    //                    db.Delete<DAL.QuotaDocument>(tData);
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            #endregion
                    //        }

                    //        LoadQuotaDocument();
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    #endregion
                }
                else
                {
                    panelQuotaNote.Visible = false;

                    panelQuotaNoteSpecialQuota.Visible = false;
                    panelQuotaInfo.Visible = false;

                    panelQuotaNoteFreedomFighter.Visible = false;
                    panelFreedomFighterInfo.Visible = false;

                    panelQuotaNotePersonWithDisability.Visible = false;
                    panelPersonWithDisabilityInfo.Visible = false;

                    panelQuotaDocUpload.Visible = false;

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                    panel_QuotaDocUpload_Tribal_Note.Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageView(ex.Message.ToString(), "fail");
            }
            string script = "handleQuotaSelection();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuotaCheck", script, true);
        }

        private Dictionary<int, string> ValidateQuotaField()
        {

            Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
            int i = 1;

            int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);

            #region Special Quota
            if (quotaId == 4)
            {
                #region Special Quota Info
                if (ddlSQQuotaType.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "Quota Type hasn't Selected !!");
                }
                else
                {

                    if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 || Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2)
                    {
                        if (Convert.ToInt32(rblServingRetired.SelectedValue) == -1)
                        {
                            dictErrorList.Add(i++, "Serving / Retired hasn't Selected!");
                        }
                        else
                        {
                            // == Children of Military Personnel (Serving and Retired) => Serving
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide BA / BD / P. No.!");
                            }

                            // == Children of Military Personnel (Serving and Retired) => Serving
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide Present Unit !");
                            }

                            // == Children of Military Personnel (Serving and Retired) => Retired
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide TS/Personal No!");
                            }

                            // == Children of Military Personnel (Serving and Retired) => Retired
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide Last Unit Served!");
                            }




                            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Serving
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide BUP No.!");
                            }

                            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Serving
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide Present Office / Department !");
                            }

                            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Retired
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide BUP No.!");
                            }

                            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Retired
                            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                            {
                                dictErrorList.Add(i++, "Please provide Last Office / Department Served !");
                            }
                        }
                    }
                    else if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 3)
                    {
                        //if (ddlSenateCommitteeMember.SelectedValue == "-1" &&
                        //    ddlSyndicateCommitteeMember.SelectedValue == "-1" &&
                        //    ddlAcademicCouncilMember.SelectedValue == "-1" &&
                        //    ddlFinanceCommitteeMember.SelectedValue == "-1")
                        //{
                        //    dictErrorList.Add(i++, "Please selected at lest one from (Senate, Syndicate, Academic Council and Finance Committee)!");
                        //}

                        if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) > 0)
                        {
                            if (Convert.ToInt32(ddlGoverningBodie.SelectedValue) > 0)
                            {

                            }
                            else
                            {
                                dictErrorList.Add(i++, "Please Select A Committee Member Name !");
                            }
                        }
                        else
                        {
                            dictErrorList.Add(i++, "Please Select Committee Member !");
                        }


                    }
                    else
                    {

                    }

                }
                #endregion

                #region Special Quota Doc
                if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 3)
                {
                    //No Doc need to check
                }
                else
                {
                    int qdc = 0;
                    using (var db = new CandidateDataManager())
                    {
                        qdc = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId == quotaId).ToList().Count();
                    }

                    if (qdc > 0)
                    {
                        //Doc Uploaded OK
                    }
                    else
                    {
                        dictErrorList.Add(i++, "Special Quota Document is not Uploaded!");
                    }

                }


                #endregion


                #region N/A
                //if (txtSQFatherOrMotherName.Text.ToString() == "")
                //{
                //    dictErrorList.Add(i++, "Father/Mother Name is Empty !!");
                //}

                //if (txtSQRankOrDesignation.Text.ToString() == "")
                //{
                //    dictErrorList.Add(i++, "Rank/Designation is Empty !!");
                //}

                //if (txtSQSenaNoOrBUPNo.Text.ToString() == "")
                //{
                //    dictErrorList.Add(i++, "Sena No/BUP No is Empty !!");
                //}

                //if (ddlSQServingOrRetired.SelectedValue == "-1")
                //{
                //    dictErrorList.Add(i++, "Serving/Retired hasn't Selected !!");
                //}

                //if (txtSQJobLocation.Text.ToString() == "")
                //{
                //    dictErrorList.Add(i++, "Job Location is Empty !!");
                //} 
                #endregion

            }
            #endregion

            #region Freedom Fighter
            else if (quotaId == 2)
            {
                #region Freedom Fighter Info
                if (ddlFFQuotaType.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "Relation With Applicant hasn't Selected !!");
                }

                if (txtFFName.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Freedom Fighter Name is Empty !!");
                }

                if (txtFFQFreedomFighterNo.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Freedom Fighter No is Empty !!");
                }

                if(txtFFQGazetteReferenceNo.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Gazette Reference No is Empty !!");
                }
                #endregion

                #region Freedom Fighter Doc
                int qdc = 0;
                using (var db = new CandidateDataManager())
                {
                    qdc = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId == quotaId).ToList().Count();
                }

                if (qdc > 0)
                {
                    //Doc Uploaded OK
                }
                else
                {
                    dictErrorList.Add(i++, "Freedom Fighter Quota Document is not Uploaded!");
                }

                #endregion
            }
            #endregion

            #region Person with Disability (Physical)
            else if (quotaId == 8)
            {
                #region Person with Disability (Physical) Info
                //if (ddlPWDQuotaType.SelectedValue == "-1")
                //{
                //    dictErrorList.Add(i++, "Quota Type hasn't Selected !!");
                //}

                if (txtPWDDisabilityName.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Disability Name is Empty !!");
                }
                #endregion

                #region Person with Disability (Physical) Doc
                int qdc = 0;

                List<QuotaDocument> doclist = new List<QuotaDocument>();

                using (var db = new CandidateDataManager())
                {
                    doclist = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId == quotaId).ToList();
                    qdc = doclist.Count();
                }

                if (qdc > 0)
                {
                    //Doc Uploaded OK
                    var regDoc = doclist.Where(x => x.Name.ToLower().Contains("disable")).FirstOrDefault();
                    if (regDoc == null)
                    {
                        dictErrorList.Add(i++, "Person with Disability (Physical) Quota Document is not Uploaded!");
                        return dictErrorList;
                    }

                    var suborno = doclist.Where(x => x.Name.ToLower().Contains("suborno")).FirstOrDefault();
                    if (suborno == null)
                    {
                        dictErrorList.Add(i++, "Person with Disability (Physical) Suborno nagorik card Document is not Uploaded!");
                        return dictErrorList;
                    }


                    #region Check Special Assistant for Disable Quota

                    int SpecialAssistant = -1;
                    try
                    {
                        SpecialAssistant = Convert.ToInt32(rblSpecialAssistantRequired.SelectedValue);
                    }
                    catch (Exception ex)
                    {
                    }

                    if (SpecialAssistant == 1) //Yes
                    {
                        bool optionChecked = false, OtherTextBoxFillUp = true;
                        foreach (ListItem item in cblSpecialAssistantTypes.Items)
                        {
                            if (item.Selected)
                            {
                                optionChecked = true;
                                if (Convert.ToInt32(item.Value) == 4)
                                {
                                    if (string.IsNullOrWhiteSpace(txtOtherSpecialAssistant.Text))
                                        OtherTextBoxFillUp = false;
                                }
                            }
                        }

                        if (!optionChecked)
                        {
                            dictErrorList.Add(i++, "Please select at least one Special Assistant Type!");
                        }
                        else
                        {
                            if (!OtherTextBoxFillUp)
                            {
                                dictErrorList.Add(i++, "Please fill up the 'Other' Special Assistant Type textbox!");
                            }
                        }

                    }
                    else if (SpecialAssistant == -1)
                    {
                        dictErrorList.Add(i++, "Please select Special Assistant Needed Yes or No!");
                    }
                    #endregion


                }
                else
                {
                    dictErrorList.Add(i++, "Person with Disability (Physical) Quota Document is not Uploaded!");
                }

                #endregion

            }
            #endregion

            #region Tribal
            else if (quotaId == 6)
            {
                #region Tribal Doc
                int qdc = 0;
                using (var db = new CandidateDataManager())
                {
                    qdc = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId && x.QuotaId == quotaId).ToList().Count();
                }

                if (qdc > 0)
                {
                    //Doc Uploaded OK
                }
                else
                {
                    dictErrorList.Add(i++, "Tribal Quota Document is not Uploaded!");
                }

                #endregion
            }
            #endregion

            else
            {
            }

            return dictErrorList;

        }


        protected void SaveOrUpdateQuotaInfo(long candidateId)
        {
            try
            {
                int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);

                if (quotaId == 4) //Special Quota
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidateId).FirstOrDefault();

                        if (qi != null)
                        {
                            #region Update

                            qi.QuotaTypeId = Convert.ToInt32(ddlSQQuotaType.SelectedValue);

                            //qi.FatherMotherName = txtSQFatherOrMotherName.Text;
                            //qi.RankDesignation = txtSQRankOrDesignation.Text;
                            //qi.SenaNoBUPNo = txtSQSenaNoOrBUPNo.Text;
                            ////qi.ServingRetired = txtServingOrRetired.Text;
                            //qi.ServingRetiredId = Convert.ToInt32(ddlSQServingOrRetired.SelectedValue);
                            //qi.JobLocation = txtSQJobLocation.Text;

                            if (!string.IsNullOrEmpty(rblServingRetired.SelectedValue))
                            {
                                qi.ServingRetired = rblServingRetired.SelectedItem.Text.Trim();
                                qi.ServingRetiredId = Convert.ToInt32(rblServingRetired.SelectedValue);
                            }
                            else
                            {
                                qi.ServingRetired = null;
                                qi.ServingRetiredId = null;
                            }

                            if (!string.IsNullOrEmpty(txtInput1.Text.Trim()))
                            {
                                qi.InputOneLabel = lblName1.Text.Trim();
                                qi.InputOne = txtInput1.Text.Trim();
                            }
                            else
                            {
                                qi.InputOneLabel = null;
                                qi.InputOne = null;
                            }

                            if (!string.IsNullOrEmpty(txtInput2.Text.Trim()))
                            {
                                qi.InputTwoLabel = lblName2.Text.Trim();
                                qi.InputTwo = txtInput2.Text.Trim();
                            }
                            else
                            {
                                qi.InputTwoLabel = null;
                                qi.InputTwo = null;
                            }

                            if (!string.IsNullOrEmpty(txtFatherName.Text.Trim()))
                            {
                                qi.FatherName = txtFatherName.Text.Trim();
                            }
                            else
                            {
                                qi.FatherName = null;
                            }

                            if (!string.IsNullOrEmpty(txtFatherRankDesignation.Text.Trim()))
                            {
                                qi.FatherRankDesignation = txtFatherRankDesignation.Text.Trim();
                            }
                            else
                            {
                                qi.FatherRankDesignation = null;
                            }

                            //if (!string.IsNullOrEmpty(txtMotherName.Text.Trim()))
                            //{
                            //    qi.MotherName = txtMotherName.Text.Trim();
                            //}
                            //else
                            //{
                            //    qi.MotherName = string.Empty;
                            //}

                            //if (!string.IsNullOrEmpty(txtMotherRankDesignation.Text.Trim()))
                            //{
                            //    qi.MotherRankDesignation = txtMotherRankDesignation.Text.Trim();
                            //}
                            //else
                            //{
                            //    qi.MotherRankDesignation = string.Empty;
                            //}


                            //if (Convert.ToInt32(ddlSenateCommitteeMember.SelectedValue) > 0)
                            //{
                            //    qi.SenateCommitteeMemberId = Convert.ToInt32(ddlSenateCommitteeMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    qi.SenateCommitteeMemberId = null;
                            //}

                            //if (Convert.ToInt32(ddlSyndicateCommitteeMember.SelectedValue) > 0)
                            //{
                            //    qi.SyndicateCommitteeMemberId = Convert.ToInt32(ddlSyndicateCommitteeMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    qi.SyndicateCommitteeMemberId = null;
                            //}

                            //if (Convert.ToInt32(ddlAcademicCouncilMember.SelectedValue) > 0)
                            //{
                            //    qi.AcademicCouncilMemberId = Convert.ToInt32(ddlAcademicCouncilMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    qi.AcademicCouncilMemberId = null;
                            //}

                            //if (Convert.ToInt32(ddlFinanceCommitteeMember.SelectedValue) > 0)
                            //{
                            //    qi.FinanceCommitteeMemberId = Convert.ToInt32(ddlFinanceCommitteeMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    qi.FinanceCommitteeMemberId = null;
                            //} 

                            if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) > 0)
                            {
                                qi.GoverningBodiesTypeId = Convert.ToInt32(rblGoverningBodie.SelectedValue);

                            }
                            else
                            {
                                qi.GoverningBodiesTypeId = null;
                            }

                            if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 1)
                            {
                                qi.SenateCommitteeMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                                qi.SyndicateCommitteeMemberId = null;
                                qi.AcademicCouncilMemberId = null;
                                qi.FinanceCommitteeMemberId = null;
                            }
                            else if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 2)
                            {
                                qi.SenateCommitteeMemberId = null;
                                qi.SyndicateCommitteeMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                                qi.AcademicCouncilMemberId = null;
                                qi.FinanceCommitteeMemberId = null;
                            }
                            else if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 3)
                            {
                                qi.SenateCommitteeMemberId = null;
                                qi.SyndicateCommitteeMemberId = null;
                                qi.AcademicCouncilMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                                qi.FinanceCommitteeMemberId = null;
                            }
                            else if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 4)
                            {
                                qi.SenateCommitteeMemberId = null;
                                qi.SyndicateCommitteeMemberId = null;
                                qi.AcademicCouncilMemberId = null;
                                qi.FinanceCommitteeMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                            }
                            else
                            {
                                qi.SenateCommitteeMemberId = null;
                                qi.SyndicateCommitteeMemberId = null;
                                qi.AcademicCouncilMemberId = null;
                                qi.FinanceCommitteeMemberId = null;
                            }

                            qi.ModifiedBy = candidateId;
                            qi.ModifiedDate = DateTime.Now;

                            db.Update<DAL.QuotaInfo>(qi);
                            #endregion
                        }
                        else
                        {
                            #region Insert

                            DAL.QuotaInfo model = new DAL.QuotaInfo();

                            model.CandidateID = candidateId;
                            model.QuotaTypeId = Convert.ToInt32(ddlSQQuotaType.SelectedValue);

                            //model.FatherMotherName = txtSQFatherOrMotherName.Text;
                            //model.RankDesignation = txtSQRankOrDesignation.Text;
                            //model.SenaNoBUPNo = txtSQSenaNoOrBUPNo.Text;
                            ////model.ServingRetired = txtServingOrRetired.Text;
                            //model.ServingRetiredId = Convert.ToInt32(ddlSQServingOrRetired.SelectedValue);
                            //model.JobLocation = txtSQJobLocation.Text;

                            if (!string.IsNullOrEmpty(rblServingRetired.SelectedValue))
                            {
                                model.ServingRetired = rblServingRetired.SelectedItem.Text.Trim();
                                model.ServingRetiredId = Convert.ToInt32(rblServingRetired.SelectedValue);
                            }
                            else
                            {
                                model.ServingRetired = null;
                                model.ServingRetiredId = null;
                            }

                            if (!string.IsNullOrEmpty(txtInput1.Text.Trim()))
                            {
                                model.InputOne = txtInput1.Text.Trim();
                            }
                            else
                            {
                                model.InputOne = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(txtInput2.Text.Trim()))
                            {
                                model.InputTwo = txtInput2.Text.Trim();
                            }
                            else
                            {
                                model.InputTwo = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(txtFatherName.Text.Trim()))
                            {
                                model.FatherName = txtFatherName.Text.Trim();
                            }
                            else
                            {
                                model.FatherName = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(txtFatherRankDesignation.Text.Trim()))
                            {
                                model.FatherRankDesignation = txtFatherRankDesignation.Text.Trim();
                            }
                            else
                            {
                                model.FatherRankDesignation = string.Empty;
                            }

                            //if (!string.IsNullOrEmpty(txtMotherName.Text.Trim()))
                            //{
                            //    model.MotherName = txtMotherName.Text.Trim();
                            //}
                            //else
                            //{
                            //    model.MotherName = string.Empty;
                            //}

                            //if (!string.IsNullOrEmpty(txtMotherRankDesignation.Text.Trim()))
                            //{
                            //    model.MotherRankDesignation = txtMotherRankDesignation.Text.Trim();
                            //}
                            //else
                            //{
                            //    model.MotherRankDesignation = string.Empty;
                            //}

                            //if (Convert.ToInt32(ddlSenateCommitteeMember.SelectedValue) > 0)
                            //{
                            //    model.SenateCommitteeMemberId = Convert.ToInt32(ddlSenateCommitteeMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    model.SenateCommitteeMemberId = null;
                            //}

                            //if (Convert.ToInt32(ddlSyndicateCommitteeMember.SelectedValue) > 0)
                            //{
                            //    model.SyndicateCommitteeMemberId = Convert.ToInt32(ddlSyndicateCommitteeMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    model.SyndicateCommitteeMemberId = null;
                            //}

                            //if (Convert.ToInt32(ddlAcademicCouncilMember.SelectedValue) > 0)
                            //{
                            //    model.AcademicCouncilMemberId = Convert.ToInt32(ddlAcademicCouncilMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    model.AcademicCouncilMemberId = null;
                            //}

                            //if (Convert.ToInt32(ddlFinanceCommitteeMember.SelectedValue) > 0)
                            //{
                            //    model.FinanceCommitteeMemberId = Convert.ToInt32(ddlFinanceCommitteeMember.SelectedValue);
                            //}
                            //else
                            //{
                            //    model.FinanceCommitteeMemberId = null;
                            //}

                            if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) > 0)
                            {
                                model.GoverningBodiesTypeId = Convert.ToInt32(rblGoverningBodie.SelectedValue);

                            }
                            else
                            {
                                model.GoverningBodiesTypeId = null;
                            }

                            if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 1)
                            {
                                model.SenateCommitteeMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                                model.SyndicateCommitteeMemberId = null;
                                model.AcademicCouncilMemberId = null;
                                model.FinanceCommitteeMemberId = null;
                            }
                            else if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 2)
                            {
                                model.SenateCommitteeMemberId = null;
                                model.SyndicateCommitteeMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                                model.AcademicCouncilMemberId = null;
                                model.FinanceCommitteeMemberId = null;
                            }
                            else if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 3)
                            {
                                model.SenateCommitteeMemberId = null;
                                model.SyndicateCommitteeMemberId = null;
                                model.AcademicCouncilMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                                model.FinanceCommitteeMemberId = null;
                            }
                            else if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) == 4)
                            {
                                model.SenateCommitteeMemberId = null;
                                model.SyndicateCommitteeMemberId = null;
                                model.AcademicCouncilMemberId = null;
                                model.FinanceCommitteeMemberId = Convert.ToInt32(ddlGoverningBodie.SelectedValue);
                            }
                            else
                            {
                                model.SenateCommitteeMemberId = null;
                                model.SyndicateCommitteeMemberId = null;
                                model.AcademicCouncilMemberId = null;
                                model.FinanceCommitteeMemberId = null;
                            }


                            model.CreatedBy = candidateId;
                            model.CreatedDate = DateTime.Now;

                            db.Insert<DAL.QuotaInfo>(model);
                            #endregion
                        }
                    }
                }
                else if (quotaId == 2) //Freedom Fighter
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidateId).FirstOrDefault();
                        if (qi != null)
                        {
                            qi.QuotaTypeId = Convert.ToInt32(ddlFFQuotaType.SelectedValue);
                            qi.FatherMotherName = txtFFName.Text.Trim();
                            qi.FreedomFighterNo = txtFFQFreedomFighterNo.Text.Trim();
                            qi.GazetteReferenceNo = txtFFQGazetteReferenceNo.Text.Trim();
                            qi.ModifiedBy = candidateId;
                            qi.ModifiedDate = DateTime.Now;

                            db.Update<DAL.QuotaInfo>(qi);
                        }
                        else
                        {
                            DAL.QuotaInfo model = new DAL.QuotaInfo();

                            model.CandidateID = candidateId;
                            model.QuotaTypeId = Convert.ToInt32(ddlFFQuotaType.SelectedValue);
                            model.FatherMotherName = txtFFName.Text.Trim();
                            model.FreedomFighterNo = txtFFQFreedomFighterNo.Text.Trim();
                            model.GazetteReferenceNo = txtFFQGazetteReferenceNo.Text.Trim();
                            model.CreatedBy = candidateId;
                            model.CreatedDate = DateTime.Now;

                            db.Insert<DAL.QuotaInfo>(model);
                        }
                    }
                }
                else if (quotaId == 8) //Person with Disability (Physical)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidateId).FirstOrDefault();
                        if (qi != null)
                        {
                            qi.QuotaTypeId = 6; // Person with Disability (Physical) except Deaf, Dumb, Blind or Multiple //Convert.ToInt32(ddlPWDQuotaType.SelectedValue);
                            qi.DisabilityName = txtPWDDisabilityName.Text;
                            qi.ModifiedBy = candidateId;
                            qi.ModifiedDate = DateTime.Now;

                            db.Update<DAL.QuotaInfo>(qi);
                        }
                        else
                        {
                            DAL.QuotaInfo model = new DAL.QuotaInfo();

                            model.CandidateID = candidateId;
                            model.QuotaTypeId = 6; // Person with Disability (Physical) except Deaf, Dumb, Blind or Multiple //Convert.ToInt32(ddlPWDQuotaType.SelectedValue);
                            model.DisabilityName = txtPWDDisabilityName.Text;
                            model.CreatedBy = candidateId;
                            model.CreatedDate = DateTime.Now;

                            db.Insert<DAL.QuotaInfo>(model);
                        }
                    }



                    #region Insert / Update Special Assistant Information

                    using (var dbsp = new CandidateDataManager())
                    {
                        List<DAL.QuotaAssistantDetail> quotaAssistantDetails = dbsp.AdmissionDB.QuotaAssistantDetails.Where(x => x.CandidateID == candidateId).ToList();

                        int specialAssistantNedded = 0;

                        try
                        {
                            specialAssistantNedded = Convert.ToInt32(rblSpecialAssistantRequired.SelectedValue);
                        }
                        catch (Exception ex)
                        {
                        }
                        if (specialAssistantNedded == 1) // 
                        {
                            foreach (ListItem item in cblSpecialAssistantTypes.Items)
                            {
                                try
                                {
                                    int assistantId = Convert.ToInt32(item.Value);
                                    var alreadyExists = quotaAssistantDetails.Where(x => x.QuotaAssistantId == assistantId).FirstOrDefault();
                                    if (item.Selected)
                                    {
                                        string textBoxValue = "";
                                        if (assistantId == 4)
                                        {
                                            textBoxValue = txtOtherSpecialAssistant.Text.Trim();
                                        }

                                        if (alreadyExists != null)
                                        {
                                            alreadyExists.OthersDetails = textBoxValue;
                                            dbsp.Update<DAL.QuotaAssistantDetail>(alreadyExists);
                                        }
                                        else
                                        {
                                            DAL.QuotaAssistantDetail Newobj = new QuotaAssistantDetail();
                                            Newobj.CandidateID = candidateId;
                                            Newobj.QuotaAssistantId = assistantId;
                                            Newobj.OthersDetails = textBoxValue;
                                            Newobj.CreatedBy = candidateId;
                                            Newobj.DateCreated = DateTime.Now;

                                            dbsp.Insert<DAL.QuotaAssistantDetail>(Newobj);
                                        }

                                    }
                                    else
                                    {
                                        if (alreadyExists != null)
                                        {
                                            dbsp.AdmissionDB.QuotaAssistantDetails.Remove(alreadyExists);
                                            dbsp.AdmissionDB.SaveChanges();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                            }
                        }
                        else
                        {
                            /// Remove Existing List
                            dbsp.AdmissionDB.QuotaAssistantDetails.RemoveRange(quotaAssistantDetails);
                            dbsp.AdmissionDB.SaveChanges();

                        }

                    }

                    #endregion


                }
                else
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidateId).FirstOrDefault();
                        if (qi != null)
                        {
                            db.Delete<DAL.QuotaInfo>(qi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }



        protected void gvFacultyList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DAL.CandidateFormSl obj = (DAL.CandidateFormSl)e.Row.DataItem;


                Label lblAdmissionUnitId = (e.Row.FindControl("lblAdmissionUnitId") as Label);
                Label lblAdmissionSetupId = (e.Row.FindControl("lblAdmissionSetupId") as Label);
                Label lblFacultyName = (e.Row.FindControl("lblFacultyName") as Label);
                DropDownList ddlDistrict = (e.Row.FindControl("ddlDistrict") as DropDownList);

                long admissionSetupId = -1;
                long admissionUnitId = -1;
                int acaCalId = -1;
                DAL.AdmissionSetup admSetup = null;
                DAL.AdmissionUnit admUnit = null;
                using (var db = new OfficeDataManager())
                {
                    admSetup = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == obj.AdmissionSetupID).FirstOrDefault();
                    if (admSetup != null)
                    {
                        acaCalId = admSetup.AcaCalID;
                        admissionSetupId = admSetup.ID;
                        lblAdmissionSetupId.Text = admSetup.ID.ToString();
                        lblAdmissionUnitId.Text = admSetup.AdmissionUnitID.ToString();


                        admUnit = db.AdmissionDB.AdmissionUnits.Where(x => x.ID == admSetup.AdmissionUnitID).FirstOrDefault();
                    }
                }
                if (admUnit != null)
                {
                    admissionUnitId = admUnit.ID;
                    lblFacultyName.Text = admUnit.UnitName;
                }


                List<DAL.ViewModels.DistrictSeatLimitVM> dslvmList = null;
                using (var db = new OfficeDataManager())
                {
                    dslvmList = db.GetDistrictSeatLimitVM(acaCalId);
                }
                if ((dslvmList != null && dslvmList.Count > 0) && dslvmList.Where(x => x.AdmissionUnitId == admissionUnitId).ToList().Count > 0)
                {
                    ddlDistrict.Items.Add(new ListItem("-- Select District --", "-1"));
                    foreach (var tData in dslvmList.Where(x => x.AdmissionUnitId == admissionUnitId).OrderBy(x => x.DistrictName).ToList())
                    {
                        ddlDistrict.Items.Add(new ListItem(tData.DistrictName.ToString(), tData.DistrictId.ToString()));
                    }

                    ddlDistrict.SelectedValue = "1";
                    ddlDistrict.Enabled = false;
                }


                DAL.CandidateFacultyWiseDistrictSeat cfwds = null;
                using (var db = new CandidateDataManager())
                {
                    cfwds = db.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == obj.CandidateID && x.AdmissionSetupId == admissionSetupId).FirstOrDefault();
                }
                if (cfwds != null)
                {
                    ddlDistrict.SelectedValue = cfwds.DistrictId.ToString();
                }

            }
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel_Massage.Visible = false;
            districtMassage.Text = string.Empty;

            try
            {
                GridViewRow row = (GridViewRow)(((DropDownList)sender)).NamingContainer;


                Label lblAdmissionUnitId = (Label)row.FindControl("lblAdmissionUnitId");
                Label lblAdmissionSetupId = (Label)row.FindControl("lblAdmissionSetupId");
                DropDownList ddlDistrict = (DropDownList)row.FindControl("ddlDistrict");

                int districtValue = 1;// Convert.ToInt32(ddlDistrict.SelectedValue);
                string districtStringValue = ddlDistrict.SelectedItem.Text;
                string dropdownId = ddlDistrict.ClientID;
                long admUnitId = Convert.ToInt64(lblAdmissionUnitId.Text);
                long admSetupId = Convert.ToInt64(lblAdmissionSetupId.Text);

                int acaCalId = -1;
                using (var db = new OfficeDataManager())
                {
                    acaCalId = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == admSetupId).Select(x => x.AcaCalID).FirstOrDefault();
                }

                if (districtValue > 0 && admUnitId > 0 && acaCalId > 0)
                {
                    List<DAL.ViewModels.DistrictSeatLimitVM> dslvmList = null;
                    using (var db = new OfficeDataManager())
                    {
                        dslvmList = db.GetDistrictSeatLimitVM(acaCalId);
                    }

                    if (dslvmList != null && dslvmList.Count > 0)
                    {
                        var model = dslvmList.Where(x => x.AdmissionUnitId == admUnitId
                                                    && x.DistrictId == districtValue).FirstOrDefault();

                        if (model != null)
                        {
                            if (model.SeatFillup >= model.SeatLimit)
                            {
                                // ==== No Seat is Available

                                string unitName = string.Empty;
                                DAL.AdmissionUnit admUnit = null;
                                using (var db = new OfficeDataManager())
                                {
                                    admUnit = db.AdmissionDB.AdmissionUnits.Where(x => x.ID == admUnitId).FirstOrDefault();
                                }
                                if (admUnit != null)
                                {
                                    unitName = admUnit.UnitName;
                                }

                                //ddlDistrict.SelectedValue = "-1";
                                ddlDistrict.SelectedValue = "1";

                                panel_Massage.Visible = true;
                                districtMassage.Text = "No more seat is available for Faculty: " + unitName + "; District: " + districtStringValue + ". Please select another district.";
                                districtMassage.ForeColor = Color.Crimson;
                                return;
                            }
                            else
                            {
                                // ==== Seat is Available

                            }

                        }
                        else
                        {
                            // ==== Seat Setup is not created
                            panel_Massage.Visible = true;
                            districtMassage.Text = "Seat limit is not set yet. Please contact with administrator.";
                            districtMassage.ForeColor = Color.Crimson;
                            return;

                        }
                    }




                    #region N/A
                    //foreach (GridViewRow row2 in gvFacultyList.Rows)
                    //{
                    //    DropDownList ddlDistrict2 = (DropDownList)row2.FindControl("ddlDistrict");
                    //    int districtValue2 = Convert.ToInt32(ddlDistrict2.SelectedValue);
                    //    string dropdownId2 = ddlDistrict2.ClientID;

                    //    if (districtValue2 > 0)
                    //    {
                    //        if (dropdownId != dropdownId2)
                    //        {
                    //            if (districtValue == districtValue2)
                    //            {
                    //                ddlDistrict.SelectedValue = "-1";
                    //                panel_Massage.Visible = true;
                    //                districtMassage.Text = "Another program with same priority exists. Please select a different priority value.";
                    //                districtMassage.ForeColor = Color.Crimson;
                    //                return;
                    //            }
                    //        }
                    //    }



                    //    #region N/A
                    //    //if (programPriority2 > 0)
                    //    //    {
                    //    //        int rowIndex2 = row2.RowIndex;

                    //    //        if (rowIndex != rowIndex2)
                    //    //        {
                    //    //            if (programPriority == programPriority2)
                    //    //            {
                    //    //                ddlPriority2.SelectedValue = "-1";
                    //    //                panel_Massage.Visible = true;
                    //    //                programPriorityMassage.Text = "Another program with same priority exists. Please select a different priority value.";
                    //    //                programPriorityMassage.ForeColor = Color.Crimson;
                    //    //                return;
                    //    //            }
                    //    //        }
                    //    //        else
                    //    //        {
                    //    //            panel_Massage.Visible = false;
                    //    //        }

                    //    //    } 
                    //    #endregion

                    //} 
                    #endregion
                }
                else
                {
                    panel_Massage.Visible = true;
                    districtMassage.Text = "Something went wrong wrong with paramiters. Please contact with administrator.";
                    districtMassage.ForeColor = Color.Crimson;
                    return;
                }
            }
            catch (Exception ex)
            {
                panel_Massage.Visible = true;
                districtMassage.Text = "Exception: " + ex.Message.ToString();
                districtMassage.ForeColor = Color.Crimson;
                return;
            }
        }






        protected string GenerateLogStringFromObject(DAL.BasicInfo obj)
        {

            string result = "";

            try
            {
                result += "FirstName: " + (!string.IsNullOrEmpty(obj.FirstName) ? obj.FirstName.ToString() : "") + "; ";
                result += "Mobile: " + (!string.IsNullOrEmpty(obj.Mobile) ? obj.Mobile.ToString() : "") + "; ";
                result += "SMSPhone: " + (!string.IsNullOrEmpty(obj.SMSPhone) ? obj.SMSPhone.ToString() : "") + "; ";
                result += "Email: " + (!string.IsNullOrEmpty(obj.Email) ? obj.Email.ToString() : "") + "; ";
                result += "DateOfBirth: " + (!string.IsNullOrEmpty(obj.DateOfBirth.ToString()) ? Convert.ToDateTime(obj.DateOfBirth).ToString("dd-MM-yyyy") : "") + "; ";
                result += "PlaceOfBirth: " + (!string.IsNullOrEmpty(obj.PlaceOfBirth) ? obj.PlaceOfBirth.ToString() : "") + "; ";
                result += "BirthRegistrationNo: " + (!string.IsNullOrEmpty(obj.BirthRegistrationNo) ? obj.BirthRegistrationNo.ToString() : "") + "; ";
                result += "NationalIdNumber: " + (!string.IsNullOrEmpty(obj.NationalIdNumber) ? obj.NationalIdNumber.ToString() : "") + "; ";

                #region Nationality
                if (obj.NationalityID != null && Convert.ToInt32(obj.NationalityID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Nationality: " + (db.AdmissionDB.Countries.Where(a => a.ID == obj.NationalityID).Select(x => x.Name).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Nationality: ; ";
                }
                #endregion

                #region MotherTongue
                if (obj.MotherTongueID != null && Convert.ToInt32(obj.MotherTongueID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "MotherTongue: " + (db.AdmissionDB.Languages.Where(a => a.ID == obj.MotherTongueID).Select(x => x.LanguageName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "MotherTongue: ; ";
                }
                #endregion

                #region MaritalStatus
                if (obj.MaritalStatusID != null && Convert.ToInt32(obj.MaritalStatusID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "MaritalStatus: " + (db.AdmissionDB.MaritalStatus.Where(a => a.ID == obj.MaritalStatusID).Select(x => x.MaritalStatus).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "MaritalStatus: ; ";
                }
                #endregion

                #region Religion
                if (obj.ReligionID != null && Convert.ToInt32(obj.ReligionID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Religion: " + (db.AdmissionDB.Religions.Where(a => a.ID == obj.ReligionID).Select(x => x.ReligionName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Religion: ; ";
                }
                #endregion

                #region Quota
                if (obj.QuotaID != null && Convert.ToInt32(obj.QuotaID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Quota: " + (db.AdmissionDB.Quotas.Where(a => a.ID == obj.QuotaID).Select(x => x.QuotaName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Quota: ; ";
                }
                #endregion

                #region BloodGroup
                if (obj.BloodGroupID != null && Convert.ToInt32(obj.BloodGroupID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "BloodGroup: " + (db.AdmissionDB.BloodGroups.Where(a => a.ID == obj.BloodGroupID).Select(x => x.BloodGroupName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "BloodGroup: ; ";
                }
                #endregion

                #region Gender
                if (obj.GenderID != null && Convert.ToInt32(obj.GenderID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Gender: " + (db.AdmissionDB.Genders.Where(a => a.ID == obj.GenderID).Select(x => x.GenderName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Gender: ; ";
                }
                #endregion

            }
            catch (Exception ex)
            {
                result += "Exception: " + ex.Message.ToString() + "; ";

            }



            return result;
        }

        protected void ddlSQQuotaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblName1.Text = string.Empty;
                txtInput1.Text = string.Empty;

                lblName2.Text = string.Empty;
                txtInput2.Text = string.Empty;

                txtFatherName.Text = string.Empty;
                txtFatherRankDesignation.Text = string.Empty;

                //txtMotherName.Text = string.Empty;
                //txtMotherRankDesignation.Text = string.Empty;

                //ddlSenateCommitteeMember.SelectedValue = "-1";
                //ddlSyndicateCommitteeMember.SelectedValue = "-1";
                //ddlAcademicCouncilMember.SelectedValue = "-1";
                //ddlFinanceCommitteeMember.SelectedValue = "-1";
                btnClearRadioButton_Click(null, null);


                if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 || Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2)
                {
                    rblServingRetired.ClearSelection();
                    panelChildrenOfMilitaryPersonnel.Visible = true;

                    // == Will be False in this Section
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;

                    // == Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)
                    panelChildrenOfSittingMembersOfBUPGoverningBodies.Visible = false;

                    panelQuotaDocUpload.Visible = true;
                    LoadQuotaDocument();
                }
                else if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 7)
                {
                    rblServingRetired.ClearSelection();
                    panelChildrenOfMilitaryPersonnel.Visible = false;

                    // == Will be False in this Section
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;

                    // == Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)
                    panelChildrenOfSittingMembersOfBUPGoverningBodies.Visible = false;

                    panelQuotaDocUpload.Visible = true;
                    LoadQuotaDocument();


                }
                else if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 3)
                {
                    rblServingRetired.ClearSelection();
                    panelChildrenOfMilitaryPersonnel.Visible = false;

                    // == Will be False in this Section
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;

                    // == Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)
                    panelChildrenOfSittingMembersOfBUPGoverningBodies.Visible = true;

                    panelQuotaDocUpload.Visible = false;
                    //LoadQuotaDocument();


                }
                else
                {
                    rblServingRetired.ClearSelection();
                    panelChildrenOfMilitaryPersonnel.Visible = false;

                    // == Will be False in this Section
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;

                    // == Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)
                    panelChildrenOfSittingMembersOfBUPGoverningBodies.Visible = false;

                    panelQuotaDocUpload.Visible = false;
                    //LoadQuotaDocument();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void rblServingRetired_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblName1.Text = string.Empty;
                txtInput1.Text = string.Empty;

                lblName2.Text = string.Empty;
                txtInput2.Text = string.Empty;

                txtFatherName.Text = string.Empty;
                txtFatherRankDesignation.Text = string.Empty;

                //txtMotherName.Text = string.Empty;
                //txtMotherRankDesignation.Text = string.Empty;

                //ddlSenateCommitteeMember.SelectedValue = "-1";
                //ddlSyndicateCommitteeMember.SelectedValue = "-1";
                //ddlAcademicCouncilMember.SelectedValue = "-1";
                //ddlFinanceCommitteeMember.SelectedValue = "-1";
                btnClearRadioButton_Click(null, null);

                if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                {
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = true;

                    lblName1.Text = "BA/BD/Personal No";
                    lblName2.Text = "Present Unit";

                    lblshortnoteFM.Text = "(If both serving, mention one)";

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = true;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                }
                else if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                {
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = true;

                    lblName1.Text = "TS/Personal No";
                    lblName2.Text = "Last Unit Served";

                    lblshortnoteFM.Text = "(If both served, mention one)";

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = true;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;


                }
                else if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                {
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = true;

                    lblName1.Text = "BUP No";
                    lblName2.Text = "Present Office/Department";

                    lblshortnoteFM.Text = "(If both serving, mention one)";

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = true;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;


                }
                else if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                {
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = true;

                    lblName1.Text = "BUP No";
                    lblName2.Text = "Last Office/Department Served";

                    lblshortnoteFM.Text = "(If both served, mention one)";

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = true;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                }
                else
                {
                    panelChildrenOfMilitaryPersonnelServingRetired.Visible = false;

                    lblName1.Text = string.Empty;
                    lblName2.Text = string.Empty;
                    lblshortnoteFM.Text = string.Empty;

                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note.Visible = false;
                    panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note.Visible = false;
                    panel_QuotaDocUpload_PersonWithDisability_Note2.Visible = false;

                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void rblGoverningBodie_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblGoverningBodie.SelectedValue == "1")
                {
                    using (var db = new GeneralDataManager())
                    {
                        DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 1).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else if (rblGoverningBodie.SelectedValue == "2")
                {
                    using (var db = new GeneralDataManager())
                    {
                        DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 2).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else if (rblGoverningBodie.SelectedValue == "3")
                {
                    using (var db = new GeneralDataManager())
                    {
                        DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 3).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else if (rblGoverningBodie.SelectedValue == "4")
                {
                    using (var db = new GeneralDataManager())
                    {
                        DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 4).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else
                {
                    btnClearRadioButton_Click(null, null);
                }



            }
            catch (Exception ex)
            {

            }
        }

        protected void btnClearRadioButton_Click(object sender, EventArgs e)
        {
            try
            {
                //rblGoverningBodie.ClearSelection();
                rblGoverningBodie.SelectedValue = "-1";

                List<DAL.QuotaGoverningBodie> list = new List<DAL.QuotaGoverningBodie>();
                DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlGoverningBodie, list, "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
            }
            catch (Exception ex)
            {

            }
        }






        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                string quotaName = ddlQuota.SelectedItem.Text.Replace(" ", "-").ToString();
                int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);
                long candidateId = cId;

                if (candidateId > 0 && quotaId > 0 && paymentId > 0)
                {
                    if (quotaId == 7)
                    {

                    }
                    else
                    {
                        int qdc = 0;
                        using (var db = new CandidateDataManager())
                        {
                            qdc = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == candidateId && x.QuotaId == quotaId).ToList().Count();
                        }

                        if (qdc >= 0 && qdc < 3)
                        {
                            //Doc Uploaded OK
                        }
                        else
                        {
                            MessageView("Maximum 3 document you can upload. Your upload limit is over!", "fail");
                            return;
                        }

                        #region Tribal, Freedom Fighter, Person with Disability (Physical)
                        if (quotaId == 6 || quotaId == 2 || quotaId == 8)
                        {
                            string fileName = "PaymentID-" + paymentId.ToString() + "-" + quotaName.ToString() + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss");

                            QuotaFileUpload(candidateId, quotaId, fileName);
                        }
                        #endregion

                        #region Special Quota
                        else if (quotaId == 4)
                        {
                            if (((!string.IsNullOrEmpty(ddlSQQuotaType.SelectedValue) && Convert.ToInt32(ddlSQQuotaType.SelectedValue) > 0) &&
                                (!string.IsNullOrEmpty(rblServingRetired.SelectedValue) && Convert.ToInt32(rblServingRetired.SelectedValue) > 0)) ||
                                ((!string.IsNullOrEmpty(ddlSQQuotaType.SelectedValue) && Convert.ToInt32(ddlSQQuotaType.SelectedValue) > 0 && Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 7))
                                )
                            {
                                //string fileName = "PaymentID-" + paymentId.ToString() + "-" + quotaName.ToString() + "-" + ddlSQQuotaType.SelectedItem.Text.Replace(" ", "-").ToString() + "-" + rblServingRetired.SelectedItem.Text.Replace(" ", "-").ToString();
                                string fileName = "PaymentID-" + paymentId.ToString() + "-" + quotaName.ToString() + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                                QuotaFileUpload(candidateId, quotaId, fileName);
                            }
                            else
                            {
                                MessageView("Please select Type of Special Quota and Serving / Retired for Upload File !", "fail");
                                return;
                            }
                        }
                        #endregion

                        else
                        { }
                    }


                }
                else
                {
                    MessageView("Please select a Quota and File No. for Upload File !", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }
        }



        protected void LoadQuotaDocument()
        {
            try
            {
                List<DAL.QuotaDocument> qdList = null;
                using (var db = new CandidateDataManager())
                {
                    qdList = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == cId).ToList();
                }

                if (qdList != null && qdList.Count > 0)
                {
                    gvQuotaDoc.DataSource = qdList;
                    gvQuotaDoc.DataBind();
                }
                else
                {
                    gvQuotaDoc.DataSource = null;
                    gvQuotaDoc.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void QuotaFileUpload(long candidateId, int quotaId, string fileNameP)
        {
            try
            {
                if (FileUploadDocument.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUploadDocument.PostedFile.FileName).ToLower();
                    int contentlength = int.Parse(FileUploadDocument.PostedFile.ContentLength.ToString());
                    string fileName = fileNameP + fileExtension; // C for candidate, Ph for Photo
                    string filePath = "~/Upload/Candidate/QuotaDoc/";
                    string fullPath = filePath + fileName;



                    if (fileExtension == ".pdf" ||
                        fileExtension == ".jpg" ||
                        fileExtension == ".jpeg" ||
                        fileExtension == ".png")
                    {

                        if (contentlength < 20480000)  //20480000 = kilobites
                        {
                            try
                            {

                                if (File.Exists(Server.MapPath(filePath + fileName)))
                                {
                                    File.Delete(Server.MapPath(filePath + fileName));
                                }

                                FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                DAL.QuotaDocument newDocumentDetailObj = new DAL.QuotaDocument();
                                newDocumentDetailObj.CandidateId = candidateId;
                                newDocumentDetailObj.QuotaId = quotaId;
                                newDocumentDetailObj.URL = fullPath;
                                newDocumentDetailObj.Name = fileName;

                                newDocumentDetailObj.CreatedBy = uId;
                                newDocumentDetailObj.DateCreated = DateTime.Now;

                                long newDocumentDetailID = -1;
                                using (var dbInsertDocumentDetail = new OfficeDataManager())
                                {
                                    dbInsertDocumentDetail.Insert<DAL.QuotaDocument>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                #region Update Quota in Basic info
                                try
                                {
                                    using (var db = new CandidateDataManager())
                                    {
                                        DAL.BasicInfo bi = db.AdmissionDB.BasicInfoes.Where(x => x.ID == candidateId).FirstOrDefault();
                                        if (bi != null)
                                        {
                                            bi.QuotaID = quotaId;
                                            db.Update<DAL.BasicInfo>(bi);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                #endregion


                                #region N/A
                                //#region IF FILE EXISTS
                                ////check if file exists
                                //if (File.Exists(Server.MapPath(filePath + fileName)))
                                //{
                                //    ////move the file to TEMP
                                //    //File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));

                                //    //delete the original file
                                //    File.Delete(Server.MapPath(filePath + fileName));

                                //    FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                //    DAL.QuotaDocument documentObj = null;
                                //    using (var db = new CandidateDataManager())
                                //    {
                                //        documentObj = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == candidateId
                                //                                                            && x.QuotaId == quotaId).FirstOrDefault();
                                //    }
                                //    if (documentObj != null) //do not update document, document exists, only update document details
                                //    {
                                //        documentObj.CandidateId = candidateId;
                                //        documentObj.QuotaId = quotaId;
                                //        documentObj.URL = fullPath;
                                //        documentObj.Name = fileName;

                                //        documentObj.ModifiedBy = uId;
                                //        documentObj.DateModified = DateTime.Now;

                                //        using (var db = new OfficeDataManager())
                                //        {
                                //            db.Update<DAL.QuotaDocument>(documentObj);
                                //        }

                                //    }
                                //    else
                                //    {
                                //        FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                //        DAL.QuotaDocument newDocumentDetailObj = new DAL.QuotaDocument();
                                //        newDocumentDetailObj.CandidateId = candidateId;
                                //        newDocumentDetailObj.QuotaId = quotaId;
                                //        newDocumentDetailObj.URL = fullPath;
                                //        newDocumentDetailObj.Name = fileName;

                                //        newDocumentDetailObj.CreatedBy = uId;
                                //        newDocumentDetailObj.DateCreated = DateTime.Now;

                                //        long newDocumentDetailID = -1;
                                //        using (var dbInsertDocumentDetail = new OfficeDataManager())
                                //        {
                                //            dbInsertDocumentDetail.Insert<DAL.QuotaDocument>(newDocumentDetailObj);
                                //            newDocumentDetailID = newDocumentDetailObj.ID;
                                //        }

                                //    }//end if-else


                                //    LoadQuotaDocument();
                                //    MessageView("File Upload Successful", "success");

                                //}//end if
                                //#endregion
                                //#region IF FILE DOES NOT EXIST
                                ////else if file does not exist
                                //else
                                //{
                                //    FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                //    DAL.QuotaDocument documentObj = null;
                                //    using (var db = new CandidateDataManager())
                                //    {
                                //        documentObj = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == candidateId
                                //                                                            && x.QuotaId == quotaId).FirstOrDefault();
                                //    }
                                //    if (documentObj != null) //do not update document, document exists, only update document details
                                //    {
                                //        documentObj.CandidateId = candidateId;
                                //        documentObj.QuotaId = quotaId;
                                //        documentObj.URL = fullPath;
                                //        documentObj.Name = fileName;

                                //        documentObj.ModifiedBy = uId;
                                //        documentObj.DateModified = DateTime.Now;

                                //        using (var db = new OfficeDataManager())
                                //        {
                                //            db.Update<DAL.QuotaDocument>(documentObj);
                                //        }
                                //    }
                                //    else
                                //    {

                                //        DAL.QuotaDocument newDocumentDetailObj = new DAL.QuotaDocument();
                                //        newDocumentDetailObj.CandidateId = candidateId;
                                //        newDocumentDetailObj.QuotaId = quotaId;
                                //        newDocumentDetailObj.URL = fullPath;
                                //        newDocumentDetailObj.Name = fileName;

                                //        newDocumentDetailObj.CreatedBy = uId;
                                //        newDocumentDetailObj.DateCreated = DateTime.Now;

                                //        long newDocumentDetailID = -1;
                                //        using (var dbInsertDocumentDetail = new OfficeDataManager())
                                //        {
                                //            dbInsertDocumentDetail.Insert<DAL.QuotaDocument>(newDocumentDetailObj);
                                //            newDocumentDetailID = newDocumentDetailObj.ID;
                                //        }

                                //    }//end if-else

                                //}//end if-else
                                //#endregion 
                                #endregion

                                LoadQuotaDocument();
                                MessageView("File Upload Successful", "success");

                            }
                            catch (Exception ex)
                            {
                                //lblMessage.Text = "Unable to upload photo.";
                                //lblMessage.ForeColor = Color.Crimson;

                                MessageView("Exception: Failed to Upload File !! Error: " + ex.Message.ToString(), "fail");
                            }
                        }
                        else
                        {
                            MessageView("File size is to larger. !!", "fail");
                        }
                    }
                    else
                    {
                        MessageView("File should be .pdf, .jpg, .jpeg, .png!", "fail");
                    }
                }// end if (FileUploadBanner.HasFile)
                else
                {
                    MessageView("No File is Selected !!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((Button)sender)).NamingContainer;

                Label lblQuotaDocID = (Label)row.FindControl("lblQuotaDocID");
                Label lblCandidateId = (Label)row.FindControl("lblCandidateId");

                int quotaDocId = Convert.ToInt32(lblQuotaDocID.Text);
                long candidateId = Convert.ToInt32(lblCandidateId.Text);

                if (quotaDocId > 0 && candidateId > 0)
                {
                    try
                    {
                        DAL.QuotaDocument qdModel = null;
                        using (var db = new CandidateDataManager())
                        {
                            qdModel = db.AdmissionDB.QuotaDocuments.Where(x => x.ID == quotaDocId).FirstOrDefault();
                        }
                        if (qdModel != null)
                        {
                            #region Delete File From Folder
                            try
                            {
                                if (File.Exists(Server.MapPath(qdModel.URL)))
                                {
                                    File.Delete(Server.MapPath(qdModel.URL));
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion

                            #region Delete Data from DB
                            try
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    db.Delete<DAL.QuotaDocument>(qdModel);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion

                            LoadQuotaDocument();
                        }
                        else
                        {

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

        protected void DeleteForm_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);

                if (SetupId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {

                        //updateDb.Update<DAL.BasicInfo>(candidate);

                        var FormSlObj = db.AdmissionDB.CandidateFormSls.Find(SetupId);

                        if (FormSlObj != null)
                        {
                            long CId = Convert.ToInt64(FormSlObj.CandidateID);
                            long AdmSetupId = Convert.ToInt64(FormSlObj.AdmissionSetupID);
                            int AcacalId = Convert.ToInt32(FormSlObj.AcaCalID);

                            var remList = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId && x.ID != SetupId).ToList();

                            if (remList == null || remList.Count == 0)
                            {
                                showAlert("Can not remove.Only one faculty left");
                                return;
                            }

                            var CandidatePaymentObj = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == CId && x.AcaCalID == AcacalId).FirstOrDefault();

                            if (CandidatePaymentObj != null)
                            {
                                if (CandidatePaymentObj.IsPaid == true)
                                {
                                    showAlert("Can not remove.Already Paid");
                                    return;
                                }

                                var AdmissionSetupObj = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == AdmSetupId).FirstOrDefault();

                                if (AdmissionSetupObj != null)
                                {
                                    long AdmUnitId = AdmissionSetupObj.AdmissionUnitID;
                                    decimal Amount = AdmissionSetupObj.Fee;


                                    int DelCount = 0;
                                    var ProgramPriorityList = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == CId && x.AdmissionSetupID == AdmSetupId && x.AcaCalID == AcacalId && x.AdmissionUnitID == AdmUnitId).ToList();
                                    if (ProgramPriorityList != null && ProgramPriorityList.Any())
                                    {
                                        foreach (var item in ProgramPriorityList)
                                        {
                                            try
                                            {
                                                db.Delete<DAL.ProgramPriority>(item);
                                                DelCount++;
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    else
                                        DelCount++;


                                    if (DelCount > 0)
                                    {
                                        db.Delete<DAL.CandidateFormSl>(FormSlObj);




                                        List<DAL.CandidateFormSl> cfsList = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId).ToList();
                                        if (cfsList != null && cfsList.Count > 0)
                                        {
                                            gvFacultyList.DataSource = cfsList.OrderBy(x => x.ID).ToList();
                                            gvFacultyList.DataBind();
                                        }
                                        else
                                        {
                                            gvFacultyList.DataSource = null;
                                            gvFacultyList.DataBind();
                                        }

                                        var ExistingSlObj = db.AdmissionDB.CandidateFormSls.Find(SetupId);
                                        if (ExistingSlObj == null)
                                        {
                                            CandidatePaymentObj.Attribute2 = CandidatePaymentObj.Amount + " to " + (CandidatePaymentObj.Amount - Amount);
                                            CandidatePaymentObj.Amount = CandidatePaymentObj.Amount - Amount;
                                            db.Update<DAL.CandidatePayment>(CandidatePaymentObj);
                                        }

                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            var AdmisisonUintObj = db.AdmissionDB.AdmissionUnits.Find(AdmUnitId);
                                            string Fac = AdmisisonUintObj != null ? AdmisisonUintObj.UnitName : "";

                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = uId;
                                            dLog.CandidateId = cId;
                                            dLog.EventName = "Faculty Remove (Candidate)";
                                            dLog.PageName = "ApplicationBasic.aspx";
                                            dLog.OldData = "";
                                            dLog.NewData = "Remove a faculty and Its Information. Faculty Name : " + Fac;
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        showAlert("Removed successfully");

                                    }


                                }

                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);

        }

        protected void ddlNationalIdOrBirthRegistration_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue);
            if (Id > 0)
                divNidBirth.Visible = true;
            else
                divNidBirth.Visible = false;
        }

        protected void btnUplaodSuborno_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                string quotaName = ddlQuota.SelectedItem.Text.Replace(" ", "-").ToString();
                int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);
                long candidateId = cId;

                if (candidateId > 0 && quotaId > 0 && paymentId > 0)
                {
                    if (quotaId == 7)
                    {

                    }
                    else
                    {
                        int qdc = 0;
                        using (var db = new CandidateDataManager())
                        {
                            qdc = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == candidateId && x.QuotaId == quotaId).ToList().Count();
                        }

                        if (qdc >= 0 && qdc < 3)
                        {
                            //Doc Uploaded OK
                        }
                        else
                        {
                            MessageView("Maximum 3 document you can upload. Your upload limit is over!", "fail");
                            return;
                        }

                        #region Tribal, Freedom Fighter, Person with Disability (Physical)
                        if (quotaId == 8)
                        {
                            string fileName = "PaymentID-" + paymentId.ToString() + "-" + "Suborno Nagorik" + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss");

                            QuotaFileUploadSubornoFile(candidateId, quotaId, fileName);
                        }
                        #endregion

                    }


                }
                else
                {
                    MessageView("Please select a Quota and File No. for Upload File !", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void QuotaFileUploadSubornoFile(long candidateId, int quotaId, string fileNameP)
        {
            try
            {
                if (FileUploadDocumentSuborno.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUploadDocumentSuborno.PostedFile.FileName).ToLower();
                    int contentlength = int.Parse(FileUploadDocumentSuborno.PostedFile.ContentLength.ToString());
                    string fileName = fileNameP + fileExtension; // C for candidate, Ph for Photo
                    string filePath = "~/Upload/Candidate/QuotaDoc/";
                    string fullPath = filePath + fileName;



                    if (fileExtension == ".pdf" ||
                        fileExtension == ".jpg" ||
                        fileExtension == ".jpeg" ||
                        fileExtension == ".png")
                    {

                        if (contentlength < 20480000)  //20480000 = kilobites
                        {
                            try
                            {

                                if (File.Exists(Server.MapPath(filePath + fileName)))
                                {
                                    File.Delete(Server.MapPath(filePath + fileName));
                                }

                                FileUploadDocumentSuborno.SaveAs(Server.MapPath(filePath + fileName));

                                DAL.QuotaDocument newDocumentDetailObj = new DAL.QuotaDocument();
                                newDocumentDetailObj.CandidateId = candidateId;
                                newDocumentDetailObj.QuotaId = quotaId;
                                newDocumentDetailObj.URL = fullPath;
                                newDocumentDetailObj.Name = fileName;

                                newDocumentDetailObj.CreatedBy = uId;
                                newDocumentDetailObj.DateCreated = DateTime.Now;

                                long newDocumentDetailID = -1;
                                using (var dbInsertDocumentDetail = new OfficeDataManager())
                                {
                                    dbInsertDocumentDetail.Insert<DAL.QuotaDocument>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                #region Update Quota in Basic info
                                try
                                {
                                    using (var db = new CandidateDataManager())
                                    {
                                        DAL.BasicInfo bi = db.AdmissionDB.BasicInfoes.Where(x => x.ID == candidateId).FirstOrDefault();
                                        if (bi != null)
                                        {
                                            bi.QuotaID = quotaId;
                                            db.Update<DAL.BasicInfo>(bi);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                #endregion


                                LoadQuotaDocument();
                                MessageView("File Upload Successful", "success");

                            }
                            catch (Exception ex)
                            {
                                //lblMessage.Text = "Unable to upload photo.";
                                //lblMessage.ForeColor = Color.Crimson;

                                MessageView("Exception: Failed to Upload File !! Error: " + ex.Message.ToString(), "fail");
                            }
                        }
                        else
                        {
                            MessageView("File size is to larger. !!", "fail");
                        }
                    }
                    else
                    {
                        MessageView("File should be .pdf, .jpg, .jpeg, .png!", "fail");
                    }
                }// end if (FileUploadBanner.HasFile)
                else
                {
                    MessageView("No File is Selected !!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void rblSpecialAssistantRequired_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedValue = Convert.ToInt32(rblSpecialAssistantRequired.SelectedValue);
                if (selectedValue == 1) // Yes
                {
                    divDisabledAssistant.Visible = true;

                    BindspecialAssistantTypesDDL();

                }
                else
                {
                    divDisabledAssistant.Visible = false;
                    divSpecialAssistantOther.Visible = false;
                    cblSpecialAssistantTypes.DataSource = null;
                    cblSpecialAssistantTypes.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindspecialAssistantTypesDDL()
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    var specialAssistantTypes = db.AdmissionDB.QuotaAssistantTypes.Where(x => x.IsActive == true).ToList();
                    if (specialAssistantTypes != null && specialAssistantTypes.Count > 0)
                    {
                        cblSpecialAssistantTypes.DataSource = specialAssistantTypes;
                        cblSpecialAssistantTypes.DataTextField = "Name";
                        cblSpecialAssistantTypes.DataValueField = "ID";
                        cblSpecialAssistantTypes.DataBind();
                    }
                    else
                    {
                        cblSpecialAssistantTypes.DataSource = null;
                        cblSpecialAssistantTypes.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void cblSpecialAssistantTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedValues = "";
                foreach (ListItem item in cblSpecialAssistantTypes.Items)
                {
                    if (item.Selected)
                    {
                        if (Convert.ToInt32(item.Value) == 4)
                        {
                            selectedValues = item.Value;
                        }
                    }
                }
                if (selectedValues == "")
                {
                    divSpecialAssistantOther.Visible = false;
                }
                else
                {
                    divSpecialAssistantOther.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSpecialAssistantInformation(long cId)
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    var specialAssitantList = db.AdmissionDB.QuotaAssistantDetails.Where(x => x.CandidateID == cId).ToList();
                    if (specialAssitantList != null && specialAssitantList.Any())
                    {
                        rblSpecialAssistantRequired.SelectedValue = "1";
                        rblSpecialAssistantRequired_SelectedIndexChanged(null, null);

                        foreach (ListItem item in cblSpecialAssistantTypes.Items)
                        {
                            int Value = Convert.ToInt32(item.Value);
                            var exists = specialAssitantList.Where(x => x.QuotaAssistantId == Value).FirstOrDefault();
                            if (exists != null)
                            {
                                item.Selected = true;
                                if (Value == 4)
                                {
                                    txtOtherSpecialAssistant.Text = exists.OthersDetails;
                                    divSpecialAssistantOther.Visible = true;
                                }
                            }
                        }
                    }
                    else
                        rblSpecialAssistantRequired.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}