using Admission.App_Start;
using CommonUtility;
using DAL;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class ApplicationDeclaration : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        string userName = "";

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            using (var db = new CandidateDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);

                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                    //paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID && x.IsPaid == true).Select(x => x.PaymentId).FirstOrDefault();
                }
            }

            if (!IsPostBack)
            {
                LoadCandidateData(uId);
                LoadInstituteInfo();
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

                    #region Get EducationCategoryId & ProgramId
                    int educationCategoryId = -1;
                    int programId = -1;
                    using (var db = new CandidateDataManager())
                    {
                        educationCategoryId = db.GetCandidateEducationCategoryID(cId);

                        if (educationCategoryId != 4)
                        {
                            btnSave_Declaration.Text = "Final Submit";
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
                                        btnSave_Declaration.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Declaration.Visible = false;
                                    }

                                }
                                else
                                {
                                    btnSave_Declaration.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Declaration.Visible = false;
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
                                        btnSave_Declaration.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Declaration.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnSave_Declaration.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Declaration.Visible = false;
                            }
                            #endregion

                            #endregion
                        }
                    }
                    else
                    {
                        btnSave_Declaration.Visible = false;
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

                    #region AdditionalInfo
                    DAL.AdditionalInfo addInfo = null;
                    using (var db = new CandidateDataManager())
                    {
                        addInfo = db.AdmissionDB.AdditionalInfoes.Where(c => c.CandidateID == cId).FirstOrDefault();
                    }

                    if (addInfo != null && Convert.ToBoolean(addInfo.IsFinalSubmit) == true)
                    {
                        chbxAgreed.Checked = true;
                        chbxAgreed.Enabled = false;
                    }
                    else
                    {
                        chbxAgreed.Checked = false;
                        chbxAgreed.Enabled = true;
                    }
                    #endregion

                    #region N/A -- Photo and Signature Upload off for Bachelor Candidates

                    //DAL.CandidateFormSl cfsl = new DAL.CandidateFormSl();
                    //DAL.AdmissionSetup aS = new DAL.AdmissionSetup();

                    //using (var db = new CandidateDataManager())
                    //{
                    //    cfsl = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId).FirstOrDefault();
                    //}

                    //if (cfsl != null)
                    //{
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        aS = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == cfsl.AdmissionSetupID).FirstOrDefault();
                    //    }
                    //}

                    //if (aS != null)
                    //{
                    //    if (aS.EducationCategoryID == 4)
                    //    {
                    //        btnSave_Declaration.Visible = false;
                    //    }
                    //}

                    #endregion

                    #region N/A -- Prevent Save if IsFinalSubmit or IsApproved
                    //try
                    //{
                    //    List<DAL.SPGetCandidateEducationCategoryByCandidateID_Result> list = null;
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        list = db.AdmissionDB.SPGetCandidateEducationCategoryByCandidateID(cId).ToList();
                    //    }

                    //    if (list != null & list.Count > 0)
                    //    {
                    //        #region Bachelors
                    //        DAL.SPGetCandidateEducationCategoryByCandidateID_Result undergradCandidate =
                    //                                list.Where(c => c.EduCatID == 4).Take(1).FirstOrDefault();

                    //        if (undergradCandidate != null)
                    //        {

                    //            btnSave_Declaration.Enabled = false;
                    //            btnSave_Declaration.Visible = false;

                    //            if (undergradCandidate.IsApproved != null)
                    //            {
                    //                if (undergradCandidate.IsApproved == true)
                    //                {
                    //                    btnSave_Declaration.Enabled = false;
                    //                    btnSave_Declaration.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Declaration.Enabled = true;
                    //                    btnSave_Declaration.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Declaration.Enabled = true;
                    //                btnSave_Declaration.Visible = true;
                    //            }

                    //            if (undergradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (undergradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Declaration.Enabled = false;
                    //                    btnSave_Declaration.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Declaration.Enabled = true;
                    //                    btnSave_Declaration.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Declaration.Enabled = true;
                    //                btnSave_Declaration.Visible = true;
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
                    //                    btnSave_Declaration.Enabled = false;
                    //                    btnSave_Declaration.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Declaration.Enabled = true;
                    //                    btnSave_Declaration.Visible = true;
                    //                }
                    //            }

                    //            if (gradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (gradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Declaration.Enabled = false;
                    //                    btnSave_Declaration.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Declaration.Enabled = true;
                    //                    btnSave_Declaration.Visible = true;
                    //                }
                    //            }
                    //        }
                    //        #endregion




                    //        #region Hide Save and Next Button for Bachelor Program Because Admission is closed
                    //        //if (list.FirstOrDefault().EduCatID == 4)
                    //        //{
                    //        //    btnSave_Declaration.Visible = false;
                    //        //}
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
                    //        btnSave_Declaration.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).ToList().Count > 0)
                    //        {
                    //            if (educationCategoryId == 4)
                    //            {
                    //                btnSave_Declaration.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
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
                    //                            btnSave_Declaration.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                    //                        }
                    //                        else
                    //                        {
                    //                            btnSave_Declaration.Visible = false;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        btnSave_Declaration.Visible = false;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Declaration.Visible = false;
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
                    //    //                btnSave_Declaration.Visible = true;
                    //    //            }
                    //    //            else
                    //    //            {
                    //    //                bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).FirstOrDefault().IsVisible);

                    //    //                btnSave_Declaration.Visible = isVisible;
                    //    //            }
                    //    //        }
                    //    //        else
                    //    //        {
                    //    //            btnSave_Declaration.Visible = false;
                    //    //        }
                    //    //    }
                    //    //} 
                    //    #endregion


                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    #endregion


                }//if (cId > 0)
            }
        }

        private void LoadInstituteInfo()
        {
            DAL.Institute institute = null;
            using (var db = new OfficeDataManager())
            {
                institute = db.AdmissionDB.Institutes.Find(1);
            }

            if (institute != null)
            {
                lblUniName1.Text = institute.Name;
                lblUniShortName1.Text = institute.ShortName;
                lblUniShortName2.Text = institute.ShortName;
                lblUniShortName3.Text = institute.ShortName;
                lblUniShortName4.Text = institute.ShortName;
            }
        }

        private Dictionary<int, string> ValidateQuotaField(int quotaId)
        {

            Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
            int i = 1;

            DAL.QuotaInfo qiModel = null;
            using (var db = new CandidateDataManager())
            {
                qiModel = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
            }

            #region Special Quota
            if (quotaId == 4)
            {
                #region Special Quota Info
                //if (ddlSQQuotaType.SelectedValue == "-1")
                //{
                //    dictErrorList.Add(i++, "Quota Type hasn't Selected !!");
                //}
                //else
                //{

                //    if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 || Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2)
                //    {
                //        if (Convert.ToInt32(rblServingRetired.SelectedValue) == -1)
                //        {
                //            dictErrorList.Add(i++, "Serving / Retired hasn't Selected!");
                //        }
                //        else
                //        {
                //            // == Children of Military Personnel (Serving and Retired) => Serving
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                //                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide BA / BD / P. No.!");
                //            }

                //            // == Children of Military Personnel (Serving and Retired) => Serving
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                //                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide Present Unit !");
                //            }

                //            // == Children of Military Personnel (Serving and Retired) => Retired
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                //                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide TS/Personal No!");
                //            }

                //            // == Children of Military Personnel (Serving and Retired) => Retired
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 1 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                //                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide Last Unit Served!");
                //            }




                //            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Serving
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                //                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide BUP No.!");
                //            }

                //            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Serving
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 1)
                //                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide Present Office / Department !");
                //            }

                //            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Retired
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                //                && string.IsNullOrEmpty(txtInput1.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide BUP No.!");
                //            }

                //            // == Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired) => Retired
                //            if ((Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 2 && Convert.ToInt32(rblServingRetired.SelectedValue) == 2)
                //                && string.IsNullOrEmpty(txtInput2.Text.Trim()))
                //            {
                //                dictErrorList.Add(i++, "Please provide Last Office / Department Served !");
                //            }
                //        }
                //    }
                //    else if (Convert.ToInt32(ddlSQQuotaType.SelectedValue) == 3)
                //    {
                //        //if (ddlSenateCommitteeMember.SelectedValue == "-1" &&
                //        //    ddlSyndicateCommitteeMember.SelectedValue == "-1" &&
                //        //    ddlAcademicCouncilMember.SelectedValue == "-1" &&
                //        //    ddlFinanceCommitteeMember.SelectedValue == "-1")
                //        //{
                //        //    dictErrorList.Add(i++, "Please selected at lest one from (Senate, Syndicate, Academic Council and Finance Committee)!");
                //        //}

                //        if (!string.IsNullOrEmpty(rblGoverningBodie.SelectedValue) && Convert.ToInt32(rblGoverningBodie.SelectedValue) > 0)
                //        {
                //            if (Convert.ToInt32(ddlGoverningBodie.SelectedValue) > 0)
                //            {

                //            }
                //            else
                //            {
                //                dictErrorList.Add(i++, "Please Select A Committee Member Name !");
                //            }
                //        }
                //        else
                //        {
                //            dictErrorList.Add(i++, "Please Select Committee Member !");
                //        }


                //    }
                //    else
                //    {

                //    }

                //}
                #endregion


                if (qiModel != null && qiModel.QuotaTypeId == null)
                {
                    dictErrorList.Add(i++, "QuotaType is Empty !!");
                }

                #region Special Quota Doc
                try
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
                catch (Exception ex)
                {

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
                if (qiModel != null)
                {
                    //#region Freedom Fighter Info
                    //if (ddlFFQuotaType.SelectedValue == "-1")
                    //{
                    //    dictErrorList.Add(i++, "Relation With Applicant hasn't Selected !!");
                    //}

                    //if (txtFFName.Text.ToString() == "")
                    //{
                    //    dictErrorList.Add(i++, "Freedom Fighter Name is Empty !!");
                    //}

                    //if (txtFFQFreedomFighterNo.Text.ToString() == "")
                    //{
                    //    dictErrorList.Add(i++, "Freedom Fighter No is Empty !!");
                    //}
                    //#endregion

                    if (qiModel != null && string.IsNullOrEmpty(qiModel.FatherMotherName))
                    {
                        dictErrorList.Add(i++, "FatherMotherName is Empty !!");
                    }

                    if (qiModel != null && qiModel.QuotaTypeId == null)
                    {
                        dictErrorList.Add(i++, "QuotaType is Empty !!");
                    }

                    if (qiModel != null && string.IsNullOrEmpty(qiModel.FreedomFighterNo))
                    {
                        dictErrorList.Add(i++, "Freedom Fighter No is Empty !!");
                    }
                    if (qiModel != null && string.IsNullOrEmpty(qiModel.GazetteReferenceNo))
                    {
                        dictErrorList.Add(i++, "Gazette Reference No is Empty !!");
                    }

                    //if (qiModel != null && string.IsNullOrEmpty(qiModel.GazetteReferenceNo))
                    //{
                    //    dictErrorList.Add(i++, "GazetteReferenceNo is Empty !!");
                    //}


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

            }
            #endregion

            #region Person with Disability (Physical)
            else if (quotaId == 8)
            {
                //#region Person with Disability (Physical) Info
                ////if (ddlPWDQuotaType.SelectedValue == "-1")
                ////{
                ////    dictErrorList.Add(i++, "Quota Type hasn't Selected !!");
                ////}

                //if (txtPWDDisabilityName.Text.ToString() == "")
                //{
                //    dictErrorList.Add(i++, "Disability Name is Empty !!");
                //}
                //#endregion

                if (qiModel != null && string.IsNullOrEmpty(qiModel.DisabilityName))
                {
                    dictErrorList.Add(i++, "Disability Name is Empty !!");
                }


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


        protected void btnSave_Declaration_Click(object sender, EventArgs e)
        {
            //btnSave_Declaration.Enabled = false;

            long cId = -1;
            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
            }

            if (chbxAgreed.Checked == false)
            {
                lblMessage.Text = "You must agree to the terms.";
                return;
            }
            else
            {
                if (cId > 0 && uId > 0)
                {
                    bool eligibitlityprogramCheck = false;

                    int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);


                    #region Get EducationCategoryId & ProgramId
                    int educationCategoryId = -1;
                    int programId = -1;
                    long? paymentID = -1;
                    using (var db = new CandidateDataManager())
                    {
                        paymentID = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).Select(x => x.PaymentId).FirstOrDefault();

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


                    #region BASIC INFO

                    string basicStr = string.Empty;

                    DAL.BasicInfo candidate = null;
                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(cId);
                    }

                    if (candidate != null)
                    {
                        if (
                            string.IsNullOrEmpty(candidate.FirstName) ||
                            //candidate.DateOfBirth == null ||
                            //string.IsNullOrEmpty(candidate.PlaceOfBirth) ||
                            //candidate.NationalityID == null ||
                            //candidate.MotherTongueID == null ||
                            //candidate.GenderID == null ||
                            //candidate.MaritalStatusID == null ||
                            candidate.BloodGroupID == null ||
                            //string.IsNullOrEmpty(candidate.Email) ||
                            //string.IsNullOrEmpty(candidate.Mobile) ||
                            candidate.ReligionID == null ||
                            candidate.QuotaID == null
                            )
                        {
                            basicStr = "Some basic candidate information is missing.";
                        }
                        else
                        {
                            Dictionary<int, string> dictErrorList = new Dictionary<int, string>();

                            #region Check all field is fillup in Form
                            dictErrorList = ValidateQuotaField(Convert.ToInt32(candidate.QuotaID));

                            if (dictErrorList != null && dictErrorList.Count > 0)
                            {
                                basicStr = "Some basic information of Quota is missing.";
                            }
                            else
                            {
                                basicStr = string.Empty;
                            }
                            #endregion

                            #region Hall Accomodation Check for Only bachelor

                            if (educationCategoryId == 4)
                            {

                                if (candidate.AttributeBool == null)
                                {
                                    basicStr = "Please select Hall Accomodation option !!";
                                }
                            }

                            #endregion

                        }
                    }
                    else
                    {
                        lblMessage.Text = "Error getting candidate info.";
                        return;
                    }

                    #endregion

                    #region PRIORITY

                    string choiceStr = string.Empty;

                    if (educationCategoryId == 4)
                    {
                        //List<DAL.CandidateFormSl> cformserList = null;
                        //using (var db = new CandidateDataManager())
                        //{
                        //    cformserList = db.GetAllCandidateFormSlByCandID_AD(cId);

                        //    if (cformserList != null && cformserList.Count > 0)
                        //    {
                        //        foreach (var tData in cformserList)
                        //        {
                        //            DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId
                        //                                                                              && x.AdmissionSetupID == tData.AdmissionSetupID
                        //                                                                              && x.Priority == 1).FirstOrDefault();

                        //            if (pp == null)
                        //            {
                        //                string msg = "Faculty: " + tData.AdmissionSetup.AdmissionUnit.UnitName + " need a 1st Choice!";
                        //                choiceStr += msg;
                        //            }
                        //        }
                        //    }
                        //}

                        eligibitlityprogramCheck = CheckAllEligiblePriority();

                        if (eligibitlityprogramCheck == false)
                        {
                            string msg = "Please Prioritize all Eligible Programs!";
                            choiceStr += msg;
                        }

                    }
                    else
                    {
                        choiceStr = string.Empty;
                        eligibitlityprogramCheck = true;
                    }


                    #region N/A
                    //List<DAL.CandidateFormSl> candidateFormSlList = null;
                    //DAL.AdmissionSetup admSetup = null;

                    //using (var db = new CandidateDataManager())
                    //{
                    //    candidateFormSlList = db.GetAllCandidateFormSlByCandID_AD(cId);
                    //    if (candidateFormSlList != null && candidateFormSlList.Count > 0)
                    //    {
                    //        ////get only admSetup for masters.
                    //        //admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6).Select(c => c.AdmissionSetup).FirstOrDefault();

                    //        admSetup = candidateFormSlList.Select(c => c.AdmissionSetup).FirstOrDefault();
                    //    }
                    //}

                    //if (admSetup != null && admSetup.EducationCategoryID == 4)
                    //{
                    //    if (candidateFormSlList != null && candidateFormSlList.Count > 0)
                    //    {

                    //        if (candidateFormSlList.FirstOrDefault().AdmissionSetup.EducationCategoryID == 4)
                    //        {
                    //            List<DAL.ProgramPriority> choices = null;
                    //            DAL.ProgramPriority firstChoice = null;

                    //            using (var db = new CandidateDataManager())
                    //            {
                    //                choices = db.AdmissionDB.ProgramPriorities.Where(c => c.CandidateID == cId).ToList();
                    //            }

                    //            if (choices != null)
                    //            {
                    //                if (choices.Count > 0)
                    //                {
                    //                    firstChoice = choices.Where(c => c.Priority == 1).FirstOrDefault();
                    //                }
                    //            }

                    //            if (firstChoice == null)
                    //            {
                    //                choiceStr = "You have not selected any program as 1st Choice.";
                    //            }
                    //            else
                    //            {
                    //                choiceStr = string.Empty;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            choiceStr = string.Empty;
                    //        }

                    //    }
                    //}
                    //else
                    //{
                    //    choiceStr = string.Empty;
                    //} 
                    #endregion


                    #endregion

                    #region PARENT/GUARDIAN

                    string relationStr = string.Empty;

                    List<DAL.Relation> relation = null;
                    using (var db = new CandidateDataManager())
                    {
                        relation = db.GetAllRelationByCandidateID_AD(cId);
                    }

                    if (relation != null)
                    {
                        DAL.RelationDetail father = relation.Where(c => c.RelationTypeID == 2).Select(c => c.RelationDetail).FirstOrDefault();
                        DAL.RelationDetail mother = relation.Where(c => c.RelationTypeID == 3).Select(c => c.RelationDetail).FirstOrDefault();
                        DAL.RelationDetail guardian = relation.Where(c => c.RelationTypeID == 5).Select(c => c.RelationDetail).FirstOrDefault();

                        if (father != null)
                        {
                            //if ((father.IsLate == null || father.IsLate == false) &&
                            //    (string.IsNullOrEmpty(father.Name) || string.IsNullOrEmpty(father.Occupation) ||
                            //    string.IsNullOrEmpty(father.Mobile))
                            //    )
                            //{
                            //    relationStr += "Some information about your father is missing.";
                            //}
                            //else if (father.IsLate == true)
                            //{
                            //    if (string.IsNullOrEmpty(father.Name))
                            //    {
                            //        relationStr += "father name is missing.";
                            //    }
                            //}
                            //else
                            //{
                            //    relationStr += string.Empty;
                            //}
                            //if (string.IsNullOrEmpty(father.Name))
                            //{
                            //    relationStr += "Father name is missing.";
                            //}
                            //else
                            //{
                            //    relationStr += string.Empty;
                            //}
                        }
                        else
                        {
                            lblMessage.Text = "Error getting Parent/Guardian (Father) information.";
                            return;
                        }

                        if (mother != null)
                        {
                            //if ( (mother.IsLate == null || mother.IsLate == false) &&
                            //    (string.IsNullOrEmpty(mother.Name) || string.IsNullOrEmpty(mother.Occupation) ||
                            //    string.IsNullOrEmpty(mother.Mobile))
                            //    )
                            //{
                            //    relationStr += "Some information about your mother is missing.";
                            //}
                            //else if(mother.IsLate == true)
                            //{
                            //    if (string.IsNullOrEmpty(mother.Name))
                            //    {
                            //        relationStr += "Mother name is missing.";
                            //    }
                            //}
                            //else
                            //{
                            //    relationStr += string.Empty;
                            //}
                            //if (string.IsNullOrEmpty(mother.Name))
                            //{
                            //    relationStr += "Mother name is missing.";
                            //}
                            //else
                            //{
                            //    relationStr += string.Empty;
                            //}
                        }
                        else
                        {
                            lblMessage.Text = "Error getting Parent/Guardian (Mother) information.";
                            return;
                        }

                        //if (guardian != null)
                        //{
                        //    if (
                        //        string.IsNullOrEmpty(guardian.Name) ||
                        //        string.IsNullOrEmpty(guardian.Mobile) ||
                        //        string.IsNullOrEmpty(guardian.RelationWithGuardian) ||
                        //        string.IsNullOrEmpty(guardian.MailingAddress)
                        //        )
                        //    {
                        //        relationStr += "Some information about your guardian is missing.";
                        //    }
                        //    else
                        //    {
                        //        relationStr += string.Empty;
                        //    }
                        //}
                        //else
                        //{
                        //    lblMessage.Text = "Error getting Parent/Guardian (Guardian) information.";
                        //    return;
                        //}
                    }
                    else
                    {
                        lblMessage.Text = "Error getting Parent/Guardian information.";
                        return;
                    }

                    #endregion

                    #region ADDRESS

                    string addressStr = string.Empty;

                    List<DAL.Address> address = null;
                    using (var db = new CandidateDataManager())
                    {
                        address = db.GetAllAddressByCandidateID_AD(cId);
                    }

                    if (address != null)
                    {
                        DAL.AddressDetail prs = address.Where(c => c.AddressTypeID == 2).Select(c => c.AddressDetail).FirstOrDefault();
                        DAL.AddressDetail prm = address.Where(c => c.AddressTypeID == 3).Select(c => c.AddressDetail).FirstOrDefault();

                        if (prs != null)
                        {
                            if (
                                string.IsNullOrEmpty(prs.AddressLine) ||
                                prs.DistrictID == null ||
                                prs.DivisionID == null ||
                                prs.CountryID == null ||
                                string.IsNullOrEmpty(prs.PostCode)
                                )
                            {
                                addressStr += "Some present address infromation missing.";
                            }
                            else
                            {
                                addressStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting present address";
                            return;
                        }

                        if (prm != null)
                        {
                            if (
                                string.IsNullOrEmpty(prm.AddressLine) ||
                                prm.DistrictID == null ||
                                prm.DivisionID == null ||
                                prm.CountryID == null ||
                                string.IsNullOrEmpty(prm.PostCode)
                                )
                            {
                                addressStr += "Some permanent address infromation missing.";
                            }
                            else
                            {
                                addressStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting permanent address.";
                            return;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Error getting address.";
                        return;
                    }

                    #endregion

                    #region ATTACHMENT

                    string attachStr = string.Empty;

                    List<DAL.Document> doc = null;
                    using (var db = new CandidateDataManager())
                    {
                        doc = db.GetAllDocumentByCandidateID_AD(cId);
                    }

                    if (doc != null)
                    {
                        DAL.DocumentDetail photo = doc.Where(c => c.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                        DAL.DocumentDetail sign = doc.Where(c => c.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();
                        //DAL.DocumentDetail fgsig = doc.Where(c => c.DocumentTypeID == 7).Select(c => c.DocumentDetail).FirstOrDefault();

                        if (photo != null)
                        {
                            if (
                                string.IsNullOrEmpty(photo.URL) ||
                                string.IsNullOrEmpty(photo.Name)
                                )
                            {
                                attachStr += "Photo is missing";
                            }
                            else
                            {
                                attachStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Photo is not available.";
                            return;
                        }

                        if (sign != null)
                        {
                            if (
                                string.IsNullOrEmpty(sign.URL) ||
                                string.IsNullOrEmpty(sign.Name)
                                )
                            {
                                attachStr += "Signature is missing";
                            }
                            else
                            {
                                attachStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Signature is not available.";
                            return;
                        }

                        //if (fgsig != null)
                        //{
                        //    if (
                        //        string.IsNullOrEmpty(fgsig.URL) ||
                        //        string.IsNullOrEmpty(fgsig.Name)
                        //        )
                        //    {
                        //        attachStr += "Financial Guarantor signature is missing.";
                        //    }
                        //    else
                        //    {
                        //        attachStr += string.Empty;
                        //    }
                        //}
                        //else
                        //{
                        //    lblMessage.Text = "Error getting financial guarantor signature.";
                        //    return;
                        //}
                    }
                    else
                    {
                        lblMessage.Text = "Photo and signature is not available. Please upload photo and signature.";
                        return;
                    }

                    #endregion

                    #region Exam Venue Selection
                    string examVenueSelectionStr = string.Empty;
                    try
                    {
                        //int educationCategoryId = -1;
                        //using (var db = new CandidateDataManager())
                        //{
                        //    educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                        //}
                        if (educationCategoryId == 4)
                        {
                            int notSelectedVenueCount = 0;
                            List<DAL.CandidateFacultyWiseDistrictSeat> fwdsList = null;
                            List<DAL.CandidateFormSl> formSerialList = null;
                            using (var db = new CandidateDataManager())
                            {
                                fwdsList = db.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == cId).ToList();
                                formSerialList = db.GetAllCandidateFormSlByCandID_AD(cId).ToList();
                            }

                            if (fwdsList != null && fwdsList.Count > 0)
                            {
                                foreach (var tData in formSerialList)
                                {
                                    DAL.CandidateFacultyWiseDistrictSeat model = fwdsList.Where(x => x.AdmissionSetupId == tData.AdmissionSetupID).FirstOrDefault();

                                    if (model != null)
                                    {

                                    }
                                    else
                                    {
                                        notSelectedVenueCount++;

                                    }
                                }

                                //if (notSelectedVenueCount > 0)
                                //{
                                //    examVenueSelectionStr = "Please select exam venue selection information for all faculties in Basic information section.";
                                //}
                                //else
                                //{
                                //    examVenueSelectionStr = string.Empty;
                                //}
                            }
                            else
                            {
                                //examVenueSelectionStr = "Please select exam venue selection information for all faculties in Basic information section.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        examVenueSelectionStr = "Failed to get exam venue selection info";
                    }
                    #endregion


                    #region EDUCATION

                    string educationStr = string.Empty;

                    List<DAL.Exam> exam = null;
                    using (var db = new CandidateDataManager())
                    {
                        exam = db.GetAllExamByCandidateID_AD(cId);
                    }

                    if (exam != null)
                    {
                        DAL.ExamDetail ssc = exam.Where(c => c.ExamTypeID == 1 || c.ExamTypeID == 5 || c.ExamTypeID == 6 || c.ExamTypeID == 12 || c.ExamTypeID == 14).Select(c => c.ExamDetail).FirstOrDefault();
                        DAL.ExamDetail hsc = exam.Where(c => c.ExamTypeID == 2 || c.ExamTypeID == 7 || c.ExamTypeID == 8 || c.ExamTypeID == 9 || c.ExamTypeID == 13 || c.ExamTypeID == 15).Select(c => c.ExamDetail).FirstOrDefault();


                        if (ssc != null)
                        {
                            if (
                                ssc.EducationBoardID < 0 ||
                                string.IsNullOrEmpty(ssc.Institute) ||
                                string.IsNullOrEmpty(ssc.RollNo) ||
                                ssc.GroupOrSubjectID == null ||
                                (ssc.ResultDivisionID == null || (ssc.ResultDivisionID == 5 && string.IsNullOrEmpty(ssc.GPA.ToString()))) ||
                                string.IsNullOrEmpty(ssc.PassingYear.ToString())
                                || (ssc.AttributeInt2 == null || ssc.AttributeInt2 <= 0)
                                || (ssc.GPAW4S == null || ssc.GPAW4S <= 0)
                                )
                            {
                                educationStr += "Some SSC/O-Level/Dakhil information missing.";
                            }
                            else
                            {
                                if (educationCategoryId != 6)
                                {
                                    //changes by rafi for outof marks
                                    if (exam.Where(x => x.ExamTypeID == 1) == null)
                                    {
                                        if (ssc.AttributeDec1 == null || ssc.AttributeDec1 == 0)
                                        {
                                            educationStr += "SSC/O-Level/Dakhil out of marks information missing.";
                                        }
                                        else
                                            educationStr += string.Empty;
                                    }
                                    else
                                    {
                                        educationStr += string.Empty;
                                    }
                                }
                                else
                                    educationStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting education info (SSC/O-Level/Dakhil)";
                            return;
                        }

                        if (hsc != null)
                        {
                            if (
                                hsc.EducationBoardID < 0 ||
                                string.IsNullOrEmpty(hsc.Institute) ||
                                string.IsNullOrEmpty(hsc.RollNo) ||
                                hsc.GroupOrSubjectID == null ||
                                (hsc.ResultDivisionID == null || (hsc.ResultDivisionID == 5 && string.IsNullOrEmpty(hsc.GPA.ToString()))) ||
                                string.IsNullOrEmpty(hsc.PassingYear.ToString())
                                || (hsc.AttributeInt2 == null || hsc.AttributeInt2 <= 0)
                                || (hsc.GPAW4S == null || hsc.GPAW4S <= 0)
                                )
                            {
                                educationStr += "Some HSC/A-Level/Alim information missing.";
                            }
                            else
                            {
                                if (educationCategoryId != 6)
                                {
                                    //changes by rafi
                                    if (exam.Where(x => x.ExamTypeID == 2) == null)
                                    {
                                        try
                                        {
                                            int AppearedCandidate = 0;
                                            var ALevelObj = exam.Where(x => x.ExamTypeID == 7).FirstOrDefault();

                                            if (ALevelObj != null && ALevelObj.ExamTypeID == 7 && ALevelObj.Attribute1 != null && ALevelObj.Attribute1 == "Appeared")
                                                AppearedCandidate = 1;

                                            if (AppearedCandidate == 0 && (ssc.AttributeDec1 == null || ssc.AttributeDec1 == 0))
                                            {
                                                educationStr += "HSC/A-Level/Alim out of marks information missing.";
                                            }
                                            else
                                                educationStr += string.Empty;
                                        }
                                        catch (Exception ex)
                                        {
                                            educationStr += "HSC/A-Level/Alim out of marks information missing.";
                                        }



                                    }
                                    else
                                        educationStr += string.Empty;
                                }
                                else
                                    educationStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting education info (HSC/A-Level/Alim)";
                            return;
                        }

                        //TODO: Check for undergraduate...then check for information in undergrad and grad...
                    }
                    else
                    {
                        lblMessage.Text = "Error getting education info.";
                        return;
                    }

                    #endregion

                    #region ADDITIONAL

                    //string additionalStr = string.Empty;

                    //DAL.AdditionalInfo additional = null;
                    //using (var db = new CandidateDataManager())
                    //{
                    //    additional = db.GetAdditionalInfoByCandidateID_ND(cId);
                    //}

                    ////if (additional != null)
                    ////{
                    ////    if (
                    ////        additional.IsEnrolled == null || (additional.IsEnrolled == true && string.IsNullOrEmpty(additional.CurrentStudentId))
                    ////        )
                    ////    {
                    ////        additionalStr += "Some additional information missing.";
                    ////    }
                    ////    else
                    ////    {
                    ////        additionalStr += string.Empty;
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    lblMessage.Text = "Error getting additional information.";
                    ////    return;
                    ////}

                    ////TODO: check for masters, then check work experience info.

                    #endregion

                    #region FINANCIAL GUARANTOR

                    ////string fingStr = string.Empty;

                    ////DAL.FinancialGuarantorInfo fing = null;
                    ////using(var db = new CandidateDataManager())
                    ////{
                    ////    fing = db.GetFinancialGuarantorByCandidateID_ND(cId);
                    ////}

                    ////if(fing != null)
                    ////{
                    ////    if (
                    ////        string.IsNullOrEmpty(fing.Name) ||
                    ////        string.IsNullOrEmpty(fing.RelationWithCandidate) ||
                    ////        string.IsNullOrEmpty(fing.Occupation) ||
                    ////        string.IsNullOrEmpty(fing.Organization) ||
                    ////        string.IsNullOrEmpty(fing.Designation) ||
                    ////        string.IsNullOrEmpty(fing.MailingAddress) ||
                    ////        string.IsNullOrEmpty(fing.Email) ||
                    ////        string.IsNullOrEmpty(fing.Mobile) ||
                    ////        string.IsNullOrEmpty(fing.SourceOfFund) 
                    ////        )
                    ////    {
                    ////        fingStr += "Some Financial Guarantor information missing.";
                    ////    }
                    ////    else
                    ////    {
                    ////        fingStr += string.Empty;
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    lblMessage.Text = "Error getting Financial Guarantor.";
                    ////    return;
                    ////}

                    #endregion
                    //-------------------------------
                    //TODO: NEED TO ADD PRIORITY
                    //if one or more string is empty, then dont update
                    if (
                        !string.IsNullOrEmpty(basicStr) ||
                        !string.IsNullOrEmpty(educationStr) ||
                        !string.IsNullOrEmpty(choiceStr) ||
                        !string.IsNullOrEmpty(relationStr) ||
                        !string.IsNullOrEmpty(addressStr) ||
                        //!string.IsNullOrEmpty(additionalStr) ||
                        !string.IsNullOrEmpty(attachStr) ||
                        !string.IsNullOrEmpty(examVenueSelectionStr) ||
                        eligibitlityprogramCheck == false
                        )
                    {
                        lblMessage.Text = "<strong>Your application form is not complete.<strong><br/>";
                        if (!string.IsNullOrEmpty(basicStr))
                        {
                            lblMessage.Text += "* " + basicStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(choiceStr))
                        {
                            lblMessage.Text += "* " + choiceStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(educationStr))
                        {
                            lblMessage.Text += "* " + educationStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(relationStr))
                        {
                            lblMessage.Text += "* " + relationStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(addressStr))
                        {
                            lblMessage.Text += "* " + addressStr + "<br/>";
                        }
                        //if (!string.IsNullOrEmpty(additionalStr))
                        //{
                        //    lblMessage.Text += "* " + additionalStr + "<br/>";
                        //}
                        //if (!string.IsNullOrEmpty(fingStr))
                        //{
                        //    lblMessage.Text += "* " + fingStr + "<br/>";
                        //}
                        if (!string.IsNullOrEmpty(attachStr))
                        {
                            lblMessage.Text += "* " + attachStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(examVenueSelectionStr))
                        {
                            lblMessage.Text += "* " + examVenueSelectionStr + "<br/>";
                        }
                        if (eligibitlityprogramCheck == false)
                        {
                            lblMessage.Text += "* Not All Eligible Programs are Prioritized <br/>";
                        }
                    }
                    else // strings are empty..update.
                    {
                        DAL.AdditionalInfo additionalInfoObj = null;
                        using (var db = new CandidateDataManager())
                        {
                            additionalInfoObj = db.GetAdditionalInfoByCandidateID_ND(cId);
                        }

                        if (additionalInfoObj != null)
                        {
                            long additionalInfo = -1;
                            additionalInfo = additionalInfoObj.ID;


                            additionalInfoObj.IsFinalSubmit = true;
                            additionalInfoObj.DateModified = DateTime.Now;
                            additionalInfoObj.ModifiedBy = cId;
                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.AdditionalInfo>(additionalInfoObj);

                                lblMessage.Text = "Your Application has been submitted successfully";

                                OnLoad(null);
                            }
                           
                        }
                    }
                } //end if (cid > 0 && uid > 0)
            }// end if else (chbxAgree.Checked)
             //btnSave_Declaration.Enabled = true;
        }





        #region N/A -- Sending SMS

        //private static string userName = "bup789";
        //private static string password = "01769021586";
        //private static string sender = "BUP";

        //public static string Send(string phoneNo, string message)
        //{

        //    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://app.planetgroupbd.com/api/v3/sendsms/plain?user="
        //        + userName + "&password=" + password + "&sender=BUP"
        //        + "&SMSText=" + System.Web.HttpUtility.UrlEncode(message) + "&GSM=" + phoneNo + "&type=longSMS");

        //    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
        //    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
        //    string responseString = respStreamReader.ReadToEnd();
        //    respStreamReader.Close();
        //    myResp.Close();
        //    return responseString;
        //}

        #endregion


        private bool CheckAllEligiblePriority()
        {
            try
            {
                #region facultylogic

                long cId = -1;

                if (uId > 0)
                {
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cId = db.GetCandidateIdByUserID_ND(uId);
                        }
                    }
                    catch (Exception ex)
                    {
                        //lblMessage_Masters.Text = "Error: Unable to get candidate.";
                        //Panel_Master.CssClass = "alert alert-danger";
                        //Panel_Master.Visible = true;
                        //return;
                        return false;
                    }
                }

                if (cId > 0)
                {
                    List<DAL.CandidateFormSl> cFormList = null;

                    try
                    {

                        using (var db = new CandidateDataManager())
                        {
                            int educationCategoryId = db.GetCandidateEducationCategoryID(cId);

                            int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                            if (AllStepInOneTime == 1 && educationCategoryId == 4)
                            {
                                bool Ispaid = false;
                                var CandidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                                if (CandidatePayment != null && CandidatePayment.IsPaid != null && Convert.ToBoolean(CandidatePayment.IsPaid))
                                    Ispaid = true;

                                cFormList = db.GetAllCandidateFormSlByCandIDIsPaid_AD(cId, Ispaid).ToList();
                            }
                            else
                                cFormList = db.GetAllCandidateFormSlByCandIDIsPaid_AD(cId, true).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        //lblMessage_Masters.Text = "Error: Unable to get candidate form serial.";
                        //Panel_Master.CssClass = "alert alert-danger";
                        //Panel_Master.Visible = true;
                        //return;
                        return false;
                    }

                    List<DAL.AdmissionSetup> admSetupList = null;

                    if (cFormList != null)
                    {
                        if (cFormList.Count > 0)
                        {
                            admSetupList = cFormList.Select(c => c.AdmissionSetup).ToList();
                        }
                    }

                    List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();

                    if (admSetupList != null)
                    {
                        if (admSetupList.Count > 0)
                        {
                            foreach (var item in admSetupList)
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    List<DAL.AdmissionUnit> _temp = new List<DAL.AdmissionUnit>();
                                    _temp = db.AdmissionDB.AdmissionUnits.Where(c => c.ID == item.AdmissionUnit.ID).ToList();
                                    if (_temp != null && _temp.Count > 0)
                                    {
                                        admUnitList.AddRange(_temp);
                                    }
                                }
                            }//end foreach
                        }
                    }//end admSetupList != null

                    List<DAL.AdmissionUnitProgram> list = new List<DAL.AdmissionUnitProgram>();
                    List<DAL.AdmissionUnitProgram> _temp2 = null;
                    List<DAL.AdmissionUnitProgram> NotEligibleProgramList = new List<DAL.AdmissionUnitProgram>();
                    int acaCalId = 0;
                    using (var db = new CandidateDataManager())
                    {
                        //cId = db.GetCandidateIdByUserID_ND(uId);
                        if (cId > 0)
                        {
                            DAL.CandidatePayment _tempCandidatePaymentObj = db.GetCandidatePaymentByCandidateID(cId);
                            if (_tempCandidatePaymentObj != null)
                            {
                                acaCalId = Convert.ToInt32(_tempCandidatePaymentObj.AcaCalID);
                            }
                        }
                    }


                    if (admUnitList != null)
                    {
                        if (admUnitList.Count() > 0)
                        {
                            foreach (var item in admUnitList)
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    _temp2 = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == item.ID
                                                                                        && c.AcaCalID == acaCalId
                                                                                        && c.EducationCategoryID == 4
                                                                                        && c.IsActive == true).ToList();
                                }
                                if (_temp2 != null && _temp2.Count > 0)
                                {
                                    list.AddRange(_temp2);
                                }
                            }
                        }
                    }

                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in admUnitList)
                        {

                            using (var db = new CandidateDataManager())
                            {
                                try
                                {
                                    DAL.Exam secondaryExam = db.GetSecondaryExamByCandidateID_AD(cId);
                                    DAL.Exam higherSecondaryExam = db.GetHigherSecdExamByCandidateID_AD(cId);
                                    DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                                    DAL.ExamDetail higherSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);

                                    List<DAL.ViewModels.TelitalkEducationSubjectModel> hscsubjects = new List<DAL.ViewModels.TelitalkEducationSubjectModel>();
                                    List<DAL.ViewModels.TelitalkEducationSubjectModel> sscsubjects = new List<DAL.ViewModels.TelitalkEducationSubjectModel>();

                                    #region SSC and HSC Education Board Result


                                    if (higherSecExamDetail != null && higherSecExamDetail.JsonDataObject != null)
                                    {
                                        try
                                        {
                                            DAL.ViewModels.TelitalkEducationResultModelHSC hscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DAL.ViewModels.TelitalkEducationResultModelHSC>(higherSecExamDetail.JsonDataObject);
                                            if (hscResultModel != null)
                                            {
                                                hscsubjects = hscResultModel.subject.ToList();
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    if (secExamDetail != null && secExamDetail.JsonDataObject != null)
                                    {
                                        try
                                        {
                                            DAL.ViewModels.TelitalkEducationResultModelHSC sscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DAL.ViewModels.TelitalkEducationResultModelHSC>(secExamDetail.JsonDataObject);
                                            if (sscResultModel != null)
                                            {
                                                sscsubjects = sscResultModel.subject.ToList();
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    #endregion

                                    bool OLevel = false, ALevel = false;

                                    if (secondaryExam != null && secondaryExam.ExamTypeID == 5)// O Level
                                        OLevel = true;
                                    if (higherSecondaryExam != null && higherSecondaryExam.ExamTypeID == 7)// A Level
                                        ALevel = true;

                                    //ECONOMICS   141(SSC)
                                    //ACCOUNTING    146(SSC)
                                    //HIGHER MATHEMATICS 126(SSC)
                                    //ENGLISH - I 107(SSC)

                                    //ECONOMICS - I   109(HSC)
                                    //ACCOUNTING - I  253(HSC)
                                    //STATISTICS - I  129(HSC)
                                    //HIGHER MATHEMATICS -I 265(HSC)
                                    //ENGLISH - I 107(HSC)

                                    //MATHEMATICS - I  127(HSC)
                                    //BIOLOGY - I 178(HSC)

                                    if (OLevel == false && ALevel == false)
                                    {
                                        DAL.ViewModels.TelitalkEducationSubjectModel hscHigherMath = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel hscStat = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel hscEco = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel hscAcc = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel hscEng = null;

                                        DAL.ViewModels.TelitalkEducationSubjectModel hscMath = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel hscBiology = null;

                                        DAL.ViewModels.TelitalkEducationSubjectModel hscPhy = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel hscChem = null;


                                        DAL.ViewModels.TelitalkEducationSubjectModel sscHigherMath = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel sscEco = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel sscAcc = null;
                                        DAL.ViewModels.TelitalkEducationSubjectModel sscEng = null;


                                        if (hscsubjects != null && hscsubjects.Any())
                                        {
                                            hscHigherMath = hscsubjects.Where(x => x.subCode == "265" || x.subCode == "228" || x.subCode == "228+229" || x.subCode == "81421").FirstOrDefault();
                                            hscStat = hscsubjects.Where(x => x.subCode == "129").FirstOrDefault();
                                            hscEco = hscsubjects.Where(x => x.subCode == "109" || x.subCode == "213" || x.subCode == "213+214").FirstOrDefault();
                                            hscAcc = hscsubjects.Where(x => x.subCode == "253" || x.subCode == "21815" || x.subCode == "21816 ").FirstOrDefault();
                                            hscEng = hscsubjects.Where(x => x.subCode == "107" || x.subCode == "238" || x.subCode == "238+239" || x.subCode == "81112" || x.subCode == "81122" || x.subCode == "21812" || x.subCode == "21822 ").FirstOrDefault();

                                            hscMath = hscsubjects.Where(x => x.subCode == "265" || x.subCode == "127" || x.subCode == "228" || x.subCode == "81411" || x.subCode == "81421").FirstOrDefault();
                                            hscBiology = hscsubjects.Where(x => x.subCode == "178" || x.subCode == "230" || x.subCode == "230+231").FirstOrDefault();

                                            hscPhy = hscsubjects.Where(x => x.subCode == "224+225" || x.subCode == "81422" || x.subCode == "174").FirstOrDefault();
                                            hscChem = hscsubjects.Where(x => x.subCode == "226+227" || x.subCode == "81423" || x.subCode == "176").FirstOrDefault();

                                        }

                                        if (sscsubjects != null && sscsubjects.Any())
                                        {
                                            sscHigherMath = sscsubjects.Where(x => x.subCode == "126" || x.subCode == "1321" || x.subCode == "1923" || x.subCode == "1311").FirstOrDefault();
                                            sscEco = sscsubjects.Where(x => x.subCode == "141" || x.subCode == "115").FirstOrDefault();
                                            sscAcc = sscsubjects.Where(x => x.subCode == "146" || x.subCode == "1322 " || x.subCode == "1312" || x.subCode == "1322").FirstOrDefault();
                                            sscEng = sscsubjects.Where(x => x.subCode == "107" || x.subCode == "136" || x.subCode == "136+137" || x.subCode == "1922 " || x.subCode == "1912 ").FirstOrDefault();


                                        }

                                        if (item.ID == 3) // FASS
                                        {
                                            bool IsDMREligible = false, IsEcoEligible = false, IsEnglishEligible = false;

                                            #region DMR

                                            if (hscHigherMath != null && (hscHigherMath.grade == "A-" || hscHigherMath.grade == "A" || hscHigherMath.grade == "A+"))
                                                IsDMREligible = true;

                                            if (IsDMREligible == false && hscStat != null && (hscStat.grade == "A-" || hscStat.grade == "A" || hscStat.grade == "A+"))
                                                IsDMREligible = true;

                                            if (!IsDMREligible)
                                            {
                                                var DMRObj = list.Where(x => x.ProgramID == 50).FirstOrDefault();
                                                if (DMRObj != null)
                                                {
                                                    list = list.Where(x => x.ProgramID != 50).ToList(); // DMR program exclude


                                                    DMRObj.Attribute3 = "Candidates applying for admission in the Department of Disaster Management and Resilience must have studied Higher Mathematics/ Statistics in HSC / equivalent level and scored minimum 'A-'(A Minus) grade in the examination.";

                                                    NotEligibleProgramList.Add(DMRObj);
                                                }
                                            }
                                            #endregion

                                            #region Economics

                                            if (hscHigherMath != null && (hscHigherMath.grade == "A-" || hscHigherMath.grade == "A" || hscHigherMath.grade == "A+"))
                                                IsEcoEligible = true;

                                            if (IsEcoEligible == false && hscStat != null && (hscStat.grade == "A-" || hscStat.grade == "A" || hscStat.grade == "A+"))
                                                IsEcoEligible = true;

                                            if (IsEcoEligible == false && hscEco != null && (hscEco.grade == "A-" || hscEco.grade == "A" || hscEco.grade == "A+"))
                                                IsEcoEligible = true;

                                            if (IsEcoEligible == false && hscAcc != null && (hscAcc.grade == "A-" || hscAcc.grade == "A" || hscAcc.grade == "A+"))
                                                IsEcoEligible = true;

                                            if (IsEcoEligible == false && sscHigherMath != null && (sscHigherMath.grade == "A-" || sscHigherMath.grade == "A" || sscHigherMath.grade == "A+"))
                                                IsEcoEligible = true;

                                            if (IsEcoEligible == false && sscEco != null && (sscEco.grade == "A-" || sscEco.grade == "A" || sscEco.grade == "A+"))
                                                IsEcoEligible = true;

                                            if (IsEcoEligible == false && sscAcc != null && (sscAcc.grade == "A-" || sscAcc.grade == "A" || sscAcc.grade == "A+"))
                                                IsEcoEligible = true;


                                            if (!IsEcoEligible)
                                            {
                                                var ECOObj = list.Where(x => x.ProgramID == 19).FirstOrDefault();
                                                if (ECOObj != null)
                                                {
                                                    list = list.Where(x => x.ProgramID != 19).ToList(); // ECO program exclude

                                                    ECOObj.Attribute3 = "Candidates applying for admission in the Department of Economics must have minimum 'A-'(A Minus) grade in Economics / Accounting / Statistics / Higher Mathematics either in SSC / equivalent or HSC/ equivalent examination.";

                                                    NotEligibleProgramList.Add(ECOObj);
                                                }
                                            }

                                            #endregion

                                            #region English

                                            if (hscEng != null && (hscEng.grade == "A-" || hscEng.grade == "A" || hscEng.grade == "A+") && sscEng != null && (sscEng.grade == "A-" || sscEng.grade == "A" || sscEng.grade == "A+"))
                                                IsEnglishEligible = true;

                                            if (!IsEnglishEligible)
                                            {
                                                var ENGObj = list.Where(x => x.ProgramID == 8).FirstOrDefault();
                                                if (ENGObj != null)
                                                {
                                                    list = list.Where(x => x.ProgramID != 8).ToList(); // English program exclude

                                                    ENGObj.Attribute3 = "Candidates applying for admission in the Department of English must have minimum 'A-'(A Minus) grade in English both in SSC / equivalent and HSC / equivalent examinations.";

                                                    NotEligibleProgramList.Add(ENGObj);
                                                }
                                            }

                                            #endregion

                                        }
                                        else if (item.ID == 5) // FST
                                        {
                                            bool IsICECSEEligible = false, IsESEligible = false;

                                            #region Condition

                                            // Condition - 1:
                                            //Minimum A- in following subjects:

                                            //224 + 225 - Physics
                                            //226 + 227 - Chemistry
                                            //230 + 231 - Biology or 228 + 229 - Higher Math
                                            //238 + 239 - English


                                            //If condition-1 is OK then condition-2:

                                            //For Eligibility in ICE and CSE: 228 + 229
                                            //For Eligibility in Environmental Science: 230 + 231
                                            //For Eligibility in ICE, CSE and Environmental Science: 228 + 229 & 230 + 231
                                            #endregion

                                            if ((hscPhy != null && (hscPhy.grade == "A-" || hscPhy.grade == "A" || hscPhy.grade == "A+"))
                                                && (hscChem != null && (hscChem.grade == "A-" || hscChem.grade == "A" || hscChem.grade == "A+"))
                                                && (hscEng != null && (hscEng.grade == "A-" || hscEng.grade == "A" || hscEng.grade == "A+"))
                                                && ((hscHigherMath != null && (hscHigherMath.grade == "A-" || hscHigherMath.grade == "A" || hscHigherMath.grade == "A+"))
                                                        || (hscBiology != null && (hscBiology.grade == "A-" || hscBiology.grade == "A" || hscBiology.grade == "A+"))))
                                            {
                                                #region ICE AND CSE

                                                if (hscHigherMath != null)
                                                    IsICECSEEligible = true;


                                                if (!IsICECSEEligible)
                                                {
                                                    var ICECSEList = list.Where(x => x.ProgramID == 18 || x.ProgramID == 48).ToList();
                                                    if (ICECSEList != null && ICECSEList.Any())
                                                    {
                                                        list = list.Where(x => x.ProgramID != 18 && x.ProgramID != 48).ToList(); // ICE AND CSE program exclude

                                                        ICECSEList.Where(w => w.AdmissionUnitID == item.ID).ToList().ForEach(u =>
                                                        {
                                                            u.Attribute3 = "For ICE & CSE candidate must have Mathmatics subject in HSC";
                                                        });

                                                        NotEligibleProgramList.AddRange(ICECSEList);
                                                    }
                                                }
                                                #endregion

                                                #region ES

                                                if (hscBiology != null)
                                                    IsESEligible = true;

                                                if (!IsESEligible)
                                                {
                                                    var ESObj = list.Where(x => x.ProgramID == 26).FirstOrDefault();
                                                    if (ESObj != null)
                                                    {
                                                        list = list.Where(x => x.ProgramID != 26).ToList(); // ES program exclude

                                                        ESObj.Attribute3 = "For ES candidate must have Biology subject in HSC";

                                                        NotEligibleProgramList.Add(ESObj);
                                                    }
                                                }

                                                #endregion
                                            }

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return false;
                                }
                            }

                        }
                    }
                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.SPGetCandidateProgramPriorities_Result> list1 = null;

                        list1 = db.AdmissionDB.SPGetCandidateProgramPriorities(cId).ToList();

                        if (list1 != null && list1.Count > 0)
                        {
                            if (list.Count == list1.Count)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }
                }
                else
                {
                    //lblMessage_Masters.Text = "Error: Unable to get candidate (1).";
                    //Panel_Master.CssClass = "alert alert-danger";
                    //Panel_Master.Visible = true;
                    //return;
                    return false;
                }

                #endregion

            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}