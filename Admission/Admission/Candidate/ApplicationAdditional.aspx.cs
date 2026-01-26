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
    public partial class ApplicationAdditional : PageBase
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
                    LoadInstituteData();
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void LoadDDL()
        {
            //admitted before
            ddlAdmittedBefore.Items.Clear();
            ddlAdmittedBefore.Items.Add(new ListItem("Select", "-1"));
            ddlAdmittedBefore.Items.Add(new ListItem("Yes", "Yes"));
            ddlAdmittedBefore.Items.Add(new ListItem("No", "No"));

            //dismissed before -------
            //ddlDismissedBefore.Items.Clear();
            //ddlDismissedBefore.Items.Add(new ListItem("Select", "-1"));
            //ddlDismissedBefore.Items.Add(new ListItem("Yes", "Yes"));
            //ddlDismissedBefore.Items.Add(new ListItem("No", "No"));

            //scholarsphip before -------
            //ddlEverAwardSchol.Items.Clear();
            //ddlEverAwardSchol.Items.Add(new ListItem("Select", "-1"));
            //ddlEverAwardSchol.Items.Add(new ListItem("Yes", "Yes"));
            //ddlEverAwardSchol.Items.Add(new ListItem("No", "No"));

            //transfer before --------
            //ddlIsTransfer.Items.Clear();
            //ddlIsTransfer.Items.Add(new ListItem("Select", "-1"));
            //ddlIsTransfer.Items.Add(new ListItem("Yes", "Yes"));
            //ddlIsTransfer.Items.Add(new ListItem("No", "No"));
        }

        private void LoadInstituteData()
        {
            using (var db = new GeneralDataManager())
            {
                DAL.Institute institute = new DAL.Institute();
                institute = db.AdmissionDB.Institutes.Find(1);
                if (institute != null || institute.ID > 0)
                {
                    lblUniShortName.Text = institute.ShortName;
                }
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
                                        btnSave_Additional.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Additional.Visible = false;
                                    }

                                }
                                else
                                {
                                    btnSave_Additional.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Additional.Visible = false;
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
                                        btnSave_Additional.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Additional.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnSave_Additional.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Additional.Visible = false;
                            }
                            #endregion

                            #endregion
                        }
                    }
                    else
                    {
                        btnSave_Additional.Visible = false;
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

                    List<DAL.CandidateFormSl> candidateFormSlList = null;
                    DAL.AdmissionSetup admSetup = null;

                    using (var db = new CandidateDataManager())
                    {
                        candidateFormSlList = db.GetAllCandidateFormSlByCandID_AD(cId);
                        if (candidateFormSlList != null)
                        {
                            //get only admSetup for masters.
                            admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6 || c.AdmissionSetup.EducationCategoryID == 4).Select(c => c.AdmissionSetup).FirstOrDefault();
                        }
                    }

                    if (admSetup != null) //if it is a masters candidate, then show occupation details and other info.
                    {
                        txtCandidateAnnualIncome.Enabled = true;
                        ddlAdmittedBefore.Enabled = true;
                        panel_Occupation.Visible = true;
                    }
                    else //bachelors candidate, do not show occupation details and other info
                    {
                        txtCandidateAnnualIncome.Enabled = false;
                        ddlAdmittedBefore.SelectedItem.Text = "No";
                        ddlAdmittedBefore.Enabled = false;
                        panel_Occupation.Visible = false;
                    }

                    //additional info
                    using (var db = new CandidateDataManager())
                    {
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

                            if (addInfo.CandidateIncome != null)
                            {
                                txtCandidateAnnualIncome.Text = addInfo.CandidateIncome.ToString();
                            }
                            else
                            {
                                txtCandidateAnnualIncome.Text = null;
                            }
                            if (addInfo.FatherAnnualIncome != null)
                            {
                                txtFatherAnnualIncome.Text = addInfo.FatherAnnualIncome.ToString();
                            }
                            else
                            {
                                txtFatherAnnualIncome.Text = null;
                            }
                            if (addInfo.MotherAnnualIncome != null)
                            {
                                txtMotherAnnualIncome.Text = addInfo.MotherAnnualIncome.ToString();
                            }
                            else
                            {
                                txtMotherAnnualIncome.Text = null;
                            }

                        }
                    }// end using additional info

                    //work experience
                    using (var db = new CandidateDataManager())
                    {
                        DAL.WorkExperience workExpObj = db.GetWorkExperienceByCandidateID_ND(cId);
                        if (workExpObj != null)
                        {
                            txtWorkDesignation.Text = workExpObj.Designation;
                            txtWorkOrganization.Text = workExpObj.Organization;
                            txtWorkAddress.Text = workExpObj.OrgAddress;
                            txtStartDateWE.Text = workExpObj.StartDate != null ? workExpObj.StartDate.Value.ToString("dd/MM/yyyy") : null;
                            txtEndDateWE.Text = workExpObj.EndDate != null ? workExpObj.EndDate.Value.ToString("dd/MM/yyyy") : null;
                        }

                    } // end using work experience

                    //extra curricular activity
                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.ExtraCurricularActivity> extraCurActList = db.GetAllExtraCurricularActivityByCandidateID_ND(cId);
                        if (extraCurActList != null)
                        {
                            DAL.ExtraCurricularActivity activity1 = extraCurActList.Where(c => c.Attribute1 == "1").FirstOrDefault();
                            DAL.ExtraCurricularActivity activity2 = extraCurActList.Where(c => c.Attribute1 == "2").FirstOrDefault();

                            if (activity1 != null)
                            {
                                txtActivity1.Text = activity1.Activity;
                                txtAward1.Text = activity1.Award;
                                txtEcaDate1.Text = activity1.DateRecieved != null ? activity1.DateRecieved.Value.ToString("dd/MM/yyyy") : null;
                            }

                            if (activity2 != null)
                            {
                                txtActivity2.Text = activity2.Activity;
                                txtAward2.Text = activity2.Award;
                                txtEcaDate2.Text = activity2.DateRecieved != null ? activity2.DateRecieved.Value.ToString("dd/MM/yyyy") : null;
                            }
                        }
                    } //end using extra curricular activity


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
                    //        #region BAchelors
                    //        DAL.SPGetCandidateEducationCategoryByCandidateID_Result undergradCandidate =
                    //                                list.Where(c => c.EduCatID == 4).Take(1).FirstOrDefault();



                    //        if (undergradCandidate != null)
                    //        {
                    //            btnSave_Additional.Enabled = false;
                    //            btnSave_Additional.Visible = false;

                    //            if (undergradCandidate.IsApproved != null)
                    //            {
                    //                if (undergradCandidate.IsApproved == true)
                    //                {
                    //                    btnSave_Additional.Enabled = false;
                    //                    btnSave_Additional.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Additional.Enabled = true;
                    //                    btnSave_Additional.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Additional.Enabled = true;
                    //                btnSave_Additional.Visible = true;
                    //            }

                    //            if (undergradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (undergradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Additional.Enabled = false;
                    //                    btnSave_Additional.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Additional.Enabled = true;
                    //                    btnSave_Additional.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Additional.Enabled = true;
                    //                btnSave_Additional.Visible = true;
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
                    //                    btnSave_Additional.Enabled = false;
                    //                    btnSave_Additional.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Additional.Enabled = true;
                    //                    btnSave_Additional.Visible = true;
                    //                }
                    //            }

                    //            if (gradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (gradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Additional.Enabled = false;
                    //                    btnSave_Additional.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Additional.Enabled = true;
                    //                    btnSave_Additional.Visible = true;
                    //                }
                    //            }
                    //        }
                    //        #endregion




                    //        #region Hide Save and Next Button for Bachelor Program Because Admission is closed
                    //        if (list.FirstOrDefault().EduCatID == 4)
                    //        {
                    //            btnSave_Additional.Visible = false;
                    //            btnNext.Visible = false;
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
                    //        btnSave_Additional.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).ToList().Count > 0)
                    //        {
                    //            if (educationCategoryId == 4)
                    //            {
                    //                btnSave_Additional.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
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
                    //                            btnSave_Additional.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                    //                        }
                    //                        else
                    //                        {
                    //                            btnSave_Additional.Visible = false;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        btnSave_Additional.Visible = false;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Additional.Visible = false;
                    //                }
                    //            }
                    //        }
                    //    }


                    //    #region N/A
                    //    ////... Save Button
                    //    //if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).FirstOrDefault() != null)
                    //    //{
                    //    //    using (var db = new CandidateDataManager())
                    //    //    {
                    //    //        DAL.AdditionalInfo addFsModel = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                    //    //        if ((addFsModel != null && Convert.ToBoolean(addFsModel.IsFinalSubmit) == false) || addFsModel == null)
                    //    //        {
                    //    //            DAL.CandidateFormSl fslModel = db.GetCandidateFormSlByCandID_AD(cId);
                    //    //            if (fslModel.AdmissionSetup.AdmissionUnitID == 6) //Master of Business Administration (Professional).
                    //    //            {
                    //    //                btnSave_Additional.Visible = true;
                    //    //            }
                    //    //            else
                    //    //            {
                    //    //                bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).FirstOrDefault().IsVisible);

                    //    //                btnSave_Additional.Visible = isVisible;
                    //    //            }
                    //    //        }
                    //    //        else
                    //    //        {
                    //    //            btnSave_Additional.Visible = false;
                    //    //        }
                    //    //    }
                    //    //} 
                    //    #endregion

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    #endregion


                    #region Candidate Annual Income (Only for masters candidate)

                    if (educationCategoryId == 6)
                    {
                        canAnnualIncome.Visible = true;
                        txtCandidateAnnualIncome.Visible = true;
                    }
                    else
                    {
                        canAnnualIncome.Visible = false;
                        txtCandidateAnnualIncome.Visible = false;
                    }

                    #endregion

                }// if(cId > 0)
            }// if(uId > 0)
        }

        protected void ddlAdmittedBefore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAdmittedBefore.SelectedValue == "Yes")
            {
                txtCurrentStudentId.Enabled = true;
            }
            else
            {
                txtCurrentStudentId.Enabled = false;
                txtCurrentStudentId.Text = null;
            }
        }

        //protected void ddlDismissedBefore_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlDismissedBefore.SelectedValue == "Yes")
        //    {
        //        txtDismissalStatement.Enabled = true;
        //    }
        //    else
        //    {
        //        txtDismissalStatement.Enabled = false;
        //    }
        //}

        //protected void ddlEverAwardSchol_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlEverAwardSchol.SelectedValue == "Yes")
        //    {
        //        txtAwardScholarshipDetails.Enabled = true;
        //    }
        //    else
        //    {
        //        txtAwardScholarshipDetails.Enabled = false;
        //    }
        //}

        protected void btnSave_Additional_Click(object sender, EventArgs e)
        {
            long cId = -1;
            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }// end using
            }
            try
            {
                if (cId > 0 && uId > 0)
                {
                    string logOldObject = string.Empty;
                    string logNewObject = string.Empty;

                    DAL.BasicInfo basicInfo = null;
                    DAL.CandidatePayment candidatePayment = null;

                    DAL.AdditionalInfo addInfoObj = null;
                    using (var db = new CandidateDataManager())
                    {
                        basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                        candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();

                        addInfoObj = db.GetAdditionalInfoByCandidateID_ND(cId);
                    }
                    if (addInfoObj != null)  //additional info exist. Update add info
                    {
                        if (ddlAdmittedBefore.SelectedValue == "Yes")
                        {
                            addInfoObj.CurrentStudentId = txtCurrentStudentId.Text.Trim();
                            addInfoObj.IsEnrolled = true;
                        }
                        else if (ddlAdmittedBefore.SelectedValue == "No")
                        {
                            addInfoObj.CurrentStudentId = null;
                            addInfoObj.IsEnrolled = false;
                        }

                        if (!string.IsNullOrEmpty(txtCandidateAnnualIncome.Text.Trim()))
                        {
                            addInfoObj.CandidateIncome = Decimal.Parse(txtCandidateAnnualIncome.Text.Trim());
                        }
                        else
                        {
                            addInfoObj.CandidateIncome = null;
                        }
                        if (!string.IsNullOrEmpty(txtFatherAnnualIncome.Text.Trim()))
                        {
                            addInfoObj.FatherAnnualIncome = Decimal.Parse(txtFatherAnnualIncome.Text.Trim());
                        }
                        else
                        {
                            addInfoObj.FatherAnnualIncome = null;
                        }
                        if (!string.IsNullOrEmpty(txtMotherAnnualIncome.Text.Trim()))
                        {
                            addInfoObj.MotherAnnualIncome = Decimal.Parse(txtMotherAnnualIncome.Text.Trim());
                        }
                        else
                        {
                            addInfoObj.MotherAnnualIncome = null;
                        }

                        #region N/A
                        //if (ddlDismissedBefore.SelectedValue == "Yes")
                        //{
                        //    addInfoObj.IsDismissedBefore = true;
                        //    txtDismissalStatement.Text = txtDismissalStatement.Text.Trim();
                        //}
                        //else if (ddlDismissedBefore.SelectedValue == "No")
                        //{
                        //    addInfoObj.IsDismissedBefore = false;
                        //    txtDismissalStatement.Text = null;
                        //}

                        //if (ddlEverAwardSchol.SelectedValue == "Yes")
                        //{
                        //    addInfoObj.IsScholarshipAwarded = true;
                        //    addInfoObj.ScholarshipAwardDetails = txtAwardScholarshipDetails.Text;
                        //}
                        //else if (ddlEverAwardSchol.SelectedValue == "No")
                        //{
                        //    addInfoObj.IsScholarshipAwarded = false;
                        //    addInfoObj.ScholarshipAwardDetails = null;
                        //}

                        //if (ddlIsTransfer.SelectedValue == "Yes")
                        //{
                        //    addInfoObj.IsTransfer = true;
                        //}
                        //else if (ddlIsTransfer.SelectedValue == "No")
                        //{
                        //    addInfoObj.IsTransfer = false;
                        //} 
                        #endregion

                        addInfoObj.DateModified = DateTime.Now;
                        addInfoObj.ModifiedBy = cId;

                        using (var dbUpdate = new CandidateDataManager())
                        {
                            dbUpdate.Update<DAL.AdditionalInfo>(addInfoObj);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(addInfoObj);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Additional Info Update (Admin)";
                            //dLog.PageName = "CandApplicationAdditional.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Additional Information.";
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
                            dLog.EventName = "Additional Info Update (Candidate)";
                            dLog.PageName = "ApplicationAdditional.aspx";
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
                        //    dLog.EventName = "Additional Info Update (Candidate)";
                        //    dLog.PageName = "ApplicationAdditional.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Additional Information.";
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
                    else //additional info does not exist. Create new.
                    {
                        DAL.AdditionalInfo _newAddInfo = new DAL.AdditionalInfo();

                        if (ddlAdmittedBefore.SelectedValue == "Yes")
                        {
                            _newAddInfo.CurrentStudentId = txtCurrentStudentId.Text.Trim();
                            _newAddInfo.IsEnrolled = true;
                        }
                        else if (ddlAdmittedBefore.SelectedValue == "No")
                        {
                            _newAddInfo.CurrentStudentId = null;
                            _newAddInfo.IsEnrolled = false;
                        }

                        if (!string.IsNullOrEmpty(txtCandidateAnnualIncome.Text.Trim()))
                        {
                            _newAddInfo.CandidateIncome = Decimal.Parse(txtCandidateAnnualIncome.Text.Trim());
                        }
                        else
                        {
                            _newAddInfo.CandidateIncome = null;
                        }
                        if (!string.IsNullOrEmpty(txtFatherAnnualIncome.Text.Trim()))
                        {
                            _newAddInfo.FatherAnnualIncome = Decimal.Parse(txtFatherAnnualIncome.Text.Trim());
                        }
                        else
                        {
                            _newAddInfo.FatherAnnualIncome = null;
                        }
                        if (!string.IsNullOrEmpty(txtMotherAnnualIncome.Text.Trim()))
                        {
                            _newAddInfo.MotherAnnualIncome = Decimal.Parse(txtMotherAnnualIncome.Text.Trim());
                        }
                        else
                        {
                            _newAddInfo.MotherAnnualIncome = null;
                        }

                        #region N/A
                        //if (ddlDismissedBefore.SelectedValue == "Yes")
                        //{
                        //    _newAddInfo.IsDismissedBefore = true;
                        //    _newAddInfo.DismissalStatement = txtDismissalStatement.Text.Trim();
                        //}
                        //else if (ddlDismissedBefore.SelectedValue == "No")
                        //{
                        //    _newAddInfo.IsDismissedBefore = false;
                        //    _newAddInfo.DismissalStatement = null;
                        //}

                        //if (ddlEverAwardSchol.SelectedValue == "Yes")
                        //{
                        //    _newAddInfo.IsScholarshipAwarded = true;
                        //    _newAddInfo.ScholarshipAwardDetails = txtAwardScholarshipDetails.Text;
                        //}
                        //else if (ddlEverAwardSchol.SelectedValue == "No")
                        //{
                        //    _newAddInfo.IsScholarshipAwarded = false;
                        //    _newAddInfo.ScholarshipAwardDetails = null;
                        //}

                        //if (ddlIsTransfer.SelectedValue == "Yes")
                        //{
                        //    _newAddInfo.IsTransfer = true;
                        //}
                        //else if (ddlIsTransfer.SelectedValue == "No")
                        //{
                        //    _newAddInfo.IsTransfer = false;
                        //} 
                        #endregion

                        _newAddInfo.CandidateID = cId;
                        _newAddInfo.CreatedBy = cId;
                        _newAddInfo.DateCreated = DateTime.Now;

                        using (var dbInsert = new CandidateDataManager())
                        {
                            dbInsert.Insert<DAL.AdditionalInfo>(_newAddInfo);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(_newAddInfo);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Additional Info Insert (Admin)";
                            //dLog.PageName = "CandApplicationAdditional.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Additional Information.";
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
                            dLog.EventName = "Additional Info Update (Candidate)";
                            dLog.PageName = "ApplicationAdditional.aspx";
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
                        //    dLog.EventName = "Additional Info Insert (Candidate)";
                        //    dLog.PageName = "ApplicationAdditional.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Additional Information.";
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

                    //for OccupationDetails
                    DAL.WorkExperience workExpObj = null;
                    using (var db = new CandidateDataManager())
                    {
                        workExpObj = db.GetWorkExperienceByCandidateID_ND(cId);
                    }
                    if (workExpObj != null) // work exp exist, update work exp
                    {
                        if (!string.IsNullOrEmpty(txtWorkDesignation.Text.Trim()) && !string.IsNullOrEmpty(txtWorkOrganization.Text.Trim()))
                        {
                            workExpObj.Designation = txtWorkDesignation.Text.Trim();
                            workExpObj.Organization = txtWorkOrganization.Text.Trim();
                            workExpObj.OrgAddress = txtWorkAddress.Text.Trim();

                            if (txtStartDateWE.Text.Trim() != "")
                            {
                                workExpObj.StartDate = DateTime.ParseExact(txtStartDateWE.Text, "dd/MM/yyyy", null);
                            }
                            else
                            {
                                workExpObj.StartDate = null;
                            }

                            if (txtEndDateWE.Text.Trim() != "")
                            {
                                workExpObj.EndDate = DateTime.ParseExact(txtEndDateWE.Text, "dd/MM/yyyy", null);
                            }
                            else
                            {
                                workExpObj.EndDate = null;
                            }

                            workExpObj.ModifiedBy = cId;
                            workExpObj.DateModified = DateTime.Now;

                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.WorkExperience>(workExpObj);
                            }
                        }
                    }
                    else // work exp does not exist, create new
                    {
                        if (!string.IsNullOrEmpty(txtWorkDesignation.Text.Trim()) && !string.IsNullOrEmpty(txtWorkOrganization.Text.Trim()))
                        {
                            DAL.WorkExperience newWorkExpObj = new DAL.WorkExperience();
                            newWorkExpObj.Designation = txtWorkDesignation.Text.Trim();
                            newWorkExpObj.Organization = txtWorkOrganization.Text.Trim();
                            newWorkExpObj.OrgAddress = txtWorkAddress.Text.Trim();

                            if (txtStartDateWE.Text.Trim() != "")
                            {
                                newWorkExpObj.StartDate = DateTime.ParseExact(txtStartDateWE.Text, "dd/MM/yyyy", null);
                            }
                            else
                            {
                                newWorkExpObj.StartDate = null;
                            }

                            if (txtEndDateWE.Text.Trim() != "")
                            {
                                newWorkExpObj.EndDate = DateTime.ParseExact(txtEndDateWE.Text, "dd/MM/yyyy", null);
                            }
                            else
                            {
                                newWorkExpObj.EndDate = null;
                            }

                            newWorkExpObj.CandidateID = cId;
                            newWorkExpObj.CreatedBy = cId;
                            newWorkExpObj.DateCreated = DateTime.Now;

                            using (var dbInsert = new CandidateDataManager())
                            {
                                dbInsert.Insert<DAL.WorkExperience>(newWorkExpObj);
                            }
                        }
                    }

                    //for Extracurricular Activity
                    List<DAL.ExtraCurricularActivity> extraCurricularList = null;

                    DAL.ExtraCurricularActivity extraCurricularAct1 = null;
                    DAL.ExtraCurricularActivity extraCurricularAct2 = null;

                    using (var db = new CandidateDataManager())
                    {
                        extraCurricularList = db.GetAllExtraCurricularActivityByCandidateID_ND(cId);
                        if (extraCurricularList != null)
                        {
                            extraCurricularAct1 = extraCurricularList.Where(c => c.Attribute1 == "1").FirstOrDefault();
                            extraCurricularAct2 = extraCurricularList.Where(c => c.Attribute1 == "2").FirstOrDefault();
                        }
                    }

                    //activity 1
                    if (!string.IsNullOrEmpty(txtActivity1.Text.Trim()))
                    {
                        if (extraCurricularAct1 != null) //extra curricular activity 1 exists, update.
                        {
                            extraCurricularAct1.Activity = string.IsNullOrEmpty(txtActivity1.Text) == true ? null : txtActivity1.Text;
                            extraCurricularAct1.Award = string.IsNullOrEmpty(txtAward1.Text) == true ? null : txtAward1.Text;
                            extraCurricularAct1.DateRecieved = DateTime.ParseExact(txtEcaDate1.Text, "dd/MM/yyyy", null);

                            extraCurricularAct1.DateModified = DateTime.Now;
                            extraCurricularAct1.ModifiedBy = cId;

                            using (var dbUpdateECA1 = new CandidateDataManager())
                            {
                                dbUpdateECA1.Update<DAL.ExtraCurricularActivity>(extraCurricularAct1);
                            }
                        }
                        else //extra curricula activity 1 does not exist. create new.
                        {
                            DAL.ExtraCurricularActivity newExtraCurricularAct1 = new DAL.ExtraCurricularActivity();

                            newExtraCurricularAct1.Activity = string.IsNullOrEmpty(txtActivity1.Text) == true ? null : txtActivity1.Text;
                            newExtraCurricularAct1.Award = string.IsNullOrEmpty(txtAward1.Text) == true ? null : txtAward1.Text;
                            newExtraCurricularAct1.DateRecieved = DateTime.ParseExact(txtEcaDate1.Text, "dd/MM/yyyy", null);
                            newExtraCurricularAct1.Attribute1 = "1";

                            newExtraCurricularAct1.CandidateID = cId;
                            newExtraCurricularAct1.CreatedBy = cId;
                            newExtraCurricularAct1.DateCreated = DateTime.Now;

                            if (!string.IsNullOrEmpty(txtActivity1.Text))
                            {
                                using (var dbInsertECA1 = new CandidateDataManager())
                                {
                                    dbInsertECA1.Insert<DAL.ExtraCurricularActivity>(newExtraCurricularAct1);
                                }
                            }
                        }
                    }


                    //activity2
                    if (!string.IsNullOrEmpty(txtActivity2.Text.Trim()))
                    {
                        if (extraCurricularAct2 != null) //extra curricular activity 2 exists, update.
                        {
                            extraCurricularAct2.Activity = string.IsNullOrEmpty(txtActivity2.Text) == true ? null : txtActivity2.Text;
                            extraCurricularAct2.Award = string.IsNullOrEmpty(txtAward2.Text) == true ? null : txtAward2.Text;
                            extraCurricularAct2.DateRecieved = DateTime.ParseExact(txtEcaDate2.Text, "dd/MM/yyyy", null);

                            extraCurricularAct2.DateModified = DateTime.Now;
                            extraCurricularAct2.ModifiedBy = cId;

                            using (var dbUpdateECA2 = new CandidateDataManager())
                            {
                                dbUpdateECA2.Update<DAL.ExtraCurricularActivity>(extraCurricularAct2);
                            }
                        }
                        else //extra curricula activity 2 does not exist. create new.
                        {
                            DAL.ExtraCurricularActivity newExtraCurricularAct2 = new DAL.ExtraCurricularActivity();

                            newExtraCurricularAct2.Activity = string.IsNullOrEmpty(txtActivity2.Text) == true ? null : txtActivity2.Text;
                            newExtraCurricularAct2.Award = string.IsNullOrEmpty(txtAward2.Text) == true ? null : txtAward2.Text;
                            newExtraCurricularAct2.DateRecieved = DateTime.ParseExact(txtEcaDate2.Text, "dd/MM/yyyy", null);
                            newExtraCurricularAct2.Attribute1 = "2";

                            newExtraCurricularAct2.CandidateID = cId;
                            newExtraCurricularAct2.CreatedBy = cId;
                            newExtraCurricularAct2.DateCreated = DateTime.Now;

                            if (!string.IsNullOrEmpty(txtActivity1.Text))
                            {
                                using (var dbInsertECA2 = new CandidateDataManager())
                                {
                                    dbInsertECA2.Insert<DAL.ExtraCurricularActivity>(newExtraCurricularAct2);
                                }
                            }
                        }
                    }

                }

                lblMessageAdditional.Text = "Info Updated successfully.";
                messagePanel_Additional.CssClass = "alert alert-success";
                messagePanel_Additional.Visible = true;


                Response.Redirect("ApplicationAttachment.aspx", false);
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X012TC&type=danger", false);
                lblMessageAdditional.Text = "Unable to save/update candidate information. Error Code : F01X012TC.";
                messagePanel_Additional.CssClass = "alert alert-danger";
                messagePanel_Additional.Visible = true;
            }


        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            btnSave_Additional_Click(sender, e);
            Response.Redirect("ApplicationAttachment.aspx", false);
        }


        protected string GenerateLogStringFromObject(DAL.AdditionalInfo additionalInfo)
        {

            string result = "";

            try
            {
                #region IsEnrolled
                //if (Convert.ToBoolean(additionalInfo.IsEnrolled) == true)
                //{
                //    result += "IsEnrolled: Yes; ";
                //}
                //else
                //{
                //    result += "AddressType: No; ";
                //}
                #endregion

                #region IsFinalSubmit
                if (Convert.ToBoolean(additionalInfo.IsFinalSubmit) == true)
                {
                    result += "IsFinalSubmit: Yes; ";
                }
                else
                {
                    result += "IsFinalSubmit: No; ";
                }
                #endregion

                #region FatherAnnualIncome
                if (additionalInfo.FatherAnnualIncome != null && Convert.ToInt32(additionalInfo.FatherAnnualIncome) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "FatherAnnualIncome: " + additionalInfo.FatherAnnualIncome + "; ";
                    }
                }
                else
                {
                    result += "FatherAnnualIncome: ; ";
                }
                #endregion

            }
            catch (Exception ex)
            {
                result += "Exception: " + ex.Message.ToString() + "; ";

            }



            return result;
        }


    }
}