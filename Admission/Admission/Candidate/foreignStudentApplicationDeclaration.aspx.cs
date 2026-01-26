using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class foreignStudentApplicationDeclaration : PageBase
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

                btnFinalSubmit.Visible = false;

                using (var db = new CandidateDataManager())
                {
                    var AdditionObj = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId && x.IsFinalSubmit == true).FirstOrDefault();

                    if (AdditionObj == null)
                    {
                        divMain.Visible = true;
                        divFinalSubmit.Visible = false;
                    }
                    else
                    {
                        divFinalSubmit.Visible = true;
                        divMain.Visible = false;
                    }
                }

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

                    //if (propertySetupList != null && propertySetupList.Count > 0)
                    //{
                    //    if (educationCategoryId == 4)
                    //    {
                    //        #region Bachelor

                    //        #region Candidate Submit Button Show/Hide
                    //        try
                    //        {
                    //            var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)).FirstOrDefault();
                    //            if (candidateSubmitButtonSetup != null)
                    //            {
                    //                bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);

                    //                //if (showHide == true)
                    //                //{
                    //                //    //btnSave_Declaration.Visible = !isFinalSubmit;
                    //                //}
                    //                //else
                    //                //{
                    //                //    btnSave_Declaration.Visible = false;
                    //                //}

                    //            }
                    //            else
                    //            {
                    //                btnSave_Declaration.Visible = false;
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            btnSave_Declaration.Visible = false;
                    //        }
                    //        #endregion


                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        #region Masters

                    //        #region Candidate Submit Button Show/Hide
                    //        try
                    //        {
                    //            var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)
                    //                                                                        && x.ProgramId == programId).FirstOrDefault();
                    //            if (candidateSubmitButtonSetup != null)
                    //            {
                    //                bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);
                    //                if (showHide == true)
                    //                {
                    //                    btnSave_Declaration.Visible = !isFinalSubmit;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Declaration.Visible = false;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Declaration.Visible = false;
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            btnSave_Declaration.Visible = false;
                    //        }
                    //        #endregion

                    //        #endregion
                    //    }
                    //}
                    //else
                    //{
                    //    btnSave_Declaration.Visible = false;
                    //}
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
                //lblUniName1.Text = institute.Name;
                //lblUniShortName1.Text = institute.ShortName;
                //lblUniShortName2.Text = institute.ShortName;
                //lblUniShortName3.Text = institute.ShortName;
                //lblUniShortName4.Text = institute.ShortName;
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admission/Candidate/upDocumentv2.aspx", false);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admission/Candidate/previewForm.aspx", false);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnPayOnline_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admission/Candidate/PaymentInformation.aspx", false);

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnFinalSubmit_Click1(object sender, EventArgs e)
        {
            ModalPopupExtender.Show();
        }

        protected void btnFinalSubmitConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    var FinalSubmitObj = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();

                    string Message =  CheckAllFiledIsGiven();

                    if (Message == "")
                    {
                        if (FinalSubmitObj != null && FinalSubmitObj.IsFinalSubmit == false)
                        {
                            FinalSubmitObj.IsFinalSubmit = true;
                            FinalSubmitObj.ModifiedBy = -99;
                            FinalSubmitObj.DateModified = DateTime.Now;

                            db.Update<DAL.AdditionalInfo>(FinalSubmitObj);
                            ModalPopupExtender.Hide();

                           

                            #region Email Send After Final Submission

                            var BasicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();

                            bool IsSent = SendEmail(BasicInfo.FirstName + " " + BasicInfo.MiddleName + " " + BasicInfo.LastName, BasicInfo.Email);

                            #endregion


                           // OnLoad(null);

                            Response.Redirect("~/Admission/Candidate/FinalsubmitView.aspx", false);

                            Label1.Visible = false;
                            lblNotGivenField.Text = "Final Submission Done! Please check your Email.";
                            ModalPopupExtender1.Show();
                        }
                    }
                    else
                    {
                        lblNotGivenField.Text = Message;
                        ModalPopupExtender1.Show();
                    }




                }
            }
            catch (Exception ex)
            {
            }
        }

        private string CheckAllFiledIsGiven()
        {

            string Message = "";
            try
            {
                using (var db = new CandidateDataManager())
                {
                    DAL.BasicInfo obj = new DAL.BasicInfo();
                    obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                    if (obj == null)
                    {
                        Message = Message + " , All Personal Information";
                    }
                    else
                    {

                        if (obj.DateOfBirth == null)
                            Message = Message + " , Date Of Birth";
                        if (obj.ReligionID == null)
                            Message = Message + " , Religion";
                        if (obj.GenderID == null)
                            Message = Message + " , Gender";
                        if (obj.NationalityID == null)
                            Message = Message + " , Country";
                    }

                    DAL.AdditionalInfo additionalinfo = new DAL.AdditionalInfo();
                    additionalinfo = db.GetAdditionalInfoByCandidateID_ND((long)cId);
                    if (additionalinfo == null)
                        Message = Message + " , Passport Number";
                    else if (additionalinfo.PassportNumber == null)
                        Message = Message + " , Passport Number";



                    List<DAL.Relation> RelationInfoList = new List<DAL.Relation>();
                    RelationInfoList = db.GetAllRelationByCandidateID_AD((long)cId);
                    if (RelationInfoList == null || RelationInfoList.Count == 0)
                    {
                        Message = Message + " , Father and Mother Name";
                    }

                    List<DAL.ProgramPriority> pplist = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId).ToList();
                    if (pplist == null || pplist.Count == 0)
                    {
                        Message = Message + " , Minimum One Program";
                    }

                    List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);
                    if (examList == null || examList.Count == 0)
                    {
                        Message = Message + " , Academic Information";
                    }

                    List<DAL.Document> documnetList = new List<DAL.Document>();
                    documnetList = db.GetAllDocumentByCandidateID_AD(cId);
                    if (documnetList == null || documnetList.Count < 14)
                    {
                        Message = Message + " , All Types of Document File";
                    }

                    if (pplist != null && pplist.Any())
                    {
                        bool Payment = true;
                        foreach (var item in pplist)
                        {
                            var ExistingPaymentObj = db.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.ProgramPriorityId == item.ID).FirstOrDefault();
                            if (ExistingPaymentObj != null && ExistingPaymentObj.IsPaid == false)
                            {
                                Payment = false;
                                break;
                            }
                        }
                        if (!Payment)
                            Message = Message + " , Upload Payment Deposit Slip";
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return Message;
        }


        private bool SendEmail(string candidateName, string email)
        {
            string mailbody = "<p>Dear " + candidateName + ",</p>" +
                           "<p>Your application has been successfully sent to Office of International Affairs(OIA), BUP for further precessing.</p>" + "<br/>" +
                           "<p>You will be notified by OIA through email(from dir.oia@bup.edu.bd) for further necessary action.</p>" + "<br/>" +
                           "<p>-BUP.</p>" + "<br/>";
                           
                           

            bool isEmailSent = EmailUtility.SendMail(email, "no-reply-2@bup.edu.bd", "BUP Admission", "Application Final Submission", mailbody);

            return isEmailSent;
        }

        protected void chbxAgreed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbxAgreed.Checked)
                    btnFinalSubmit.Visible = true;
                else
                    btnFinalSubmit.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
    }
}