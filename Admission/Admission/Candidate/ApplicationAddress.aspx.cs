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
    public partial class ApplicationAddress : PageBase
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
                //division
                List<DAL.Division> divisionList = db.GetAllDivision_ND();
                DDLHelper.Bind<DAL.Division>(ddlPresentDivision, divisionList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Division);
                DDLHelper.Bind<DAL.Division>(ddlPermanentDivision, divisionList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Division);

                List<DAL.District> districtList = db.GetAllDistrict_ND();
                DDLHelper.Bind<DAL.District>(ddlPresentDistrict, districtList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
                DDLHelper.Bind<DAL.District>(ddlPermanentDistrict, districtList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.District);

                // Only Bangladesh country is fixed
                DDLHelper.Bind<DAL.Country>(ddlPresentCountry, db.AdmissionDB.Countries.Where(c => c.IsActive == true && c.ID==2).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);
                DDLHelper.Bind<DAL.Country>(ddlPermanentCountry, db.AdmissionDB.Countries.Where(c => c.IsActive == true && c.ID == 2).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);

                // Bangladesh Auto Selected

                ddlPresentCountry.SelectedValue = "2";
                ddlPermanentCountry.SelectedValue = "2";


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
                                        btnSave_Address.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Address.Visible = false;
                                    }

                                }
                                else
                                {
                                    btnSave_Address.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Address.Visible = false;
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
                                        btnSave_Address.Visible = !isFinalSubmit;
                                    }
                                    else
                                    {
                                        btnSave_Address.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnSave_Address.Visible = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                btnSave_Address.Visible = false;
                            }
                            #endregion

                            #endregion
                        }
                    }
                    else
                    {
                        btnSave_Address.Visible = false;
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

                        List<DAL.Address> addressList = db.GetAllAddressByCandidateID_AD(cId);
                        if (addressList != null)
                        {
                            DAL.Address presAdd = addressList.Where(a => a.AddressTypeID == 2).FirstOrDefault();
                            DAL.Address permAdd = addressList.Where(a => a.AddressTypeID == 3).FirstOrDefault();

                            if (presAdd != null)
                            {
                                DAL.AddressDetail presAddress = presAdd.AddressDetail;

                                if (presAddress != null)
                                {
                                    txtPresentAddress.Text = presAddress.AddressLine;
                                    ddlPresentDivision.SelectedValue = presAddress.DivisionID.ToString();
                                    if (presAddress.DistrictID > 0 && presAddress.DivisionID > 0)
                                    {
                                        ddlPresentDistrict.SelectedValue = presAddress.DistrictID.ToString();
                                        ddlPresentDistrict.Enabled = true;
                                    }
                                    else
                                    {
                                        ddlPresentDistrict.Enabled = false;
                                    }
                                    txtPresentUpazila.Text = presAddress.Upazila;
                                    ddlPresentCountry.SelectedValue = presAddress.CountryID.ToString();
                                    txtPresentPostCode.Text = presAddress.PostCode;
                                    txtPresentPhone.Text = presAddress.Phone;
                                }
                            }

                            if (permAdd != null)
                            {
                                DAL.AddressDetail permAddress = permAdd.AddressDetail;

                                if (permAddress != null)
                                {
                                    txtPermanentAddress.Text = permAddress.AddressLine;
                                    ddlPermanentDivision.SelectedValue = permAddress.DivisionID.ToString();
                                    if (permAddress.DistrictID > 0 && permAddress.DivisionID > 0)
                                    {
                                        ddlPermanentDistrict.SelectedValue = permAddress.DistrictID.ToString();
                                        ddlPermanentDistrict.Enabled = true;
                                    }
                                    else
                                    {
                                        ddlPermanentDistrict.Enabled = false;
                                    }
                                    txtPermanentUpazila.Text = permAddress.Upazila;
                                    ddlPermanentCountry.SelectedValue = permAddress.CountryID.ToString();
                                    txtPermanentPostCode.Text = permAddress.PostCode;
                                    txtPermanentPhone.Text = permAddress.Phone;
                                }
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
                    //            btnSave_Address.Enabled = false;
                    //            btnSave_Address.Visible = false;

                    //            if (undergradCandidate.IsApproved != null)
                    //            {
                    //                if (undergradCandidate.IsApproved == true)
                    //                {
                    //                    btnSave_Address.Enabled = false;
                    //                    btnSave_Address.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Address.Enabled = true;
                    //                    btnSave_Address.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Address.Enabled = true;
                    //                btnSave_Address.Visible = true;
                    //            }

                    //            if (undergradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (undergradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Address.Enabled = false;
                    //                    btnSave_Address.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Address.Enabled = true;
                    //                    btnSave_Address.Visible = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                btnSave_Address.Enabled = true;
                    //                btnSave_Address.Visible = true;
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
                    //                    btnSave_Address.Enabled = false;
                    //                    btnSave_Address.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Address.Enabled = true;
                    //                    btnSave_Address.Visible = true;
                    //                }
                    //            }

                    //            if (gradCandidate.IsFinalSubmit != null)
                    //            {
                    //                if (gradCandidate.IsFinalSubmit == true)
                    //                {
                    //                    btnSave_Address.Enabled = false;
                    //                    btnSave_Address.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Address.Enabled = true;
                    //                    btnSave_Address.Visible = true;
                    //                }
                    //            }
                    //        }
                    //        #endregion




                    //        #region Hide Save and Next Button for Bachelor Program Because Admission is closed
                    //        //if (list.FirstOrDefault().EduCatID == 4)
                    //        //{
                    //        //    btnSave_Address.Visible = false;
                    //        //    btnNext.Visible = false;
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
                    //        btnSave_Address.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).ToList().Count > 0)
                    //        {
                    //            if (educationCategoryId == 4)
                    //            {
                    //                btnSave_Address.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
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
                    //                            btnSave_Address.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                    //                        }
                    //                        else
                    //                        {
                    //                            btnSave_Address.Visible = false;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        btnSave_Address.Visible = false;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    btnSave_Address.Visible = false;
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
                    //    //                btnSave_Address.Visible = true;
                    //    //            }
                    //    //            else
                    //    //            {
                    //    //                bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).FirstOrDefault().IsVisible);

                    //    //                btnSave_Address.Visible = isVisible;
                    //    //            }
                    //    //        }
                    //    //        else
                    //    //        {
                    //    //            btnSave_Address.Visible = false;
                    //    //        }
                    //    //    }
                    //    //} 
                    //    #endregion

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    #endregion
                    
                }// if(cId > 0)
            }// if(uId > 0)
        }

        protected void btnSave_Address_Click(object sender, EventArgs e)
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

                    DAL.Address presAdd = null;
                    DAL.Address permAdd = null;

                    using (var db = new CandidateDataManager())
                    {
                        basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                        candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();

                        List<DAL.Address> addressList = db.GetAllAddressByCandidateID_AD(cId);
                        if (addressList != null)
                        {
                            presAdd = addressList.Where(c => c.AddressTypeID == 2).FirstOrDefault(); //2 = present
                            permAdd = addressList.Where(c => c.AddressTypeID == 3).FirstOrDefault(); // 3 = permanent
                        }
                    }

                    #region PRESENT ADDRESS
                    if (presAdd != null) // address exist, check address details' existence
                    {
                        DAL.AddressDetail presAddDtl = presAdd.AddressDetail;

                        if (presAddDtl != null) // address details exists, update
                        {
                            logOldObject = string.Empty;
                            logOldObject = GenerateLogStringFromObject(presAdd, presAddDtl);

                            presAddDtl.AddressLine = txtPresentAddress.Text;
                            presAddDtl.DistrictID = Int32.Parse(ddlPresentDistrict.SelectedValue);
                            presAddDtl.DivisionID = Int32.Parse(ddlPresentDivision.SelectedValue);
                            presAddDtl.Area = null;
                            presAddDtl.Village = null;
                            presAddDtl.PostOffice = null;
                            presAddDtl.HouseNo = null;
                            presAddDtl.RoadNo = null;
                            presAddDtl.SectionBlockNo = null;
                            presAddDtl.Thana = null;
                            presAddDtl.CountryID = Int32.Parse(ddlPresentCountry.SelectedValue);
                            presAddDtl.PostCode = txtPresentPostCode.Text;
                            presAddDtl.Upazila = txtPresentUpazila.Text;
                            presAddDtl.Phone = txtPresentPhone.Text;

                            presAddDtl.ModifiedBy = cId;
                            presAddDtl.DateModified = DateTime.Now;

                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.AddressDetail>(presAddDtl);
                            }

                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(presAdd, presAddDtl);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Address Info Update (Present) (Admin)";
                                //dLog.PageName = "CandApplicationAddress.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                                dLog.EventName = "Address Info Update (Present) (Candidate)";
                                dLog.PageName = "ApplicationAddress.aspx";
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
                            //    dLog.EventName = "Address Info Update (Present) (Candidate)";
                            //    dLog.PageName = "ApplicationAddress.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                        else // address details does not exist. create new address details, then update address (probably unnecessary)
                        {
                            DAL.AddressDetail newAddressDetail = new DAL.AddressDetail();
                            newAddressDetail.AddressLine = txtPresentAddress.Text;
                            newAddressDetail.DistrictID = Int32.Parse(ddlPresentDistrict.SelectedValue);
                            newAddressDetail.DivisionID = Int32.Parse(ddlPresentDivision.SelectedValue);
                            newAddressDetail.Area = null;
                            newAddressDetail.Village = null;
                            newAddressDetail.PostOffice = null;
                            newAddressDetail.HouseNo = null;
                            newAddressDetail.RoadNo = null;
                            newAddressDetail.SectionBlockNo = null;
                            newAddressDetail.Thana = null;
                            newAddressDetail.CountryID = Int32.Parse(ddlPresentCountry.SelectedValue);
                            newAddressDetail.PostCode = txtPresentPostCode.Text;
                            newAddressDetail.Upazila = txtPresentUpazila.Text;
                            newAddressDetail.Phone = txtPresentPhone.Text;

                            long newAddressDetailsID = -1;
                            using (var dbInsertNewAddressDetails = new CandidateDataManager())
                            {
                                dbInsertNewAddressDetails.Insert<DAL.AddressDetail>(newAddressDetail);
                                newAddressDetailsID = newAddressDetail.ID;
                            }
                            if (newAddressDetailsID > 0)
                            {
                                DAL.Address addressObjToBeUpdated = null;
                                using (var dbGetAddressDetailObj = new CandidateDataManager())
                                {
                                    //probably an unnecessary call to db...
                                    addressObjToBeUpdated = dbGetAddressDetailObj.GetAddressByCandidateIDAddressTypeID_ND(cId, 2); // 2 = present
                                }
                                if (addressObjToBeUpdated != null)
                                {
                                    addressObjToBeUpdated.AddressDetailsID = newAddressDetailsID;
                                    using (var dbUpdateAddressObj = new CandidateDataManager())
                                    {
                                        dbUpdateAddressObj.Update<DAL.Address>(addressObjToBeUpdated);
                                    }
                                }

                                logNewObject = string.Empty;
                                logNewObject = GenerateLogStringFromObject(addressObjToBeUpdated, newAddressDetail);
                            }

                            


                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Address Info Update (Present) (Admin)";
                                //dLog.PageName = "CandApplicationAddress.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                                dLog.EventName = "Address Info Update (Present) (Candidate)";
                                dLog.PageName = "ApplicationAddress.aspx";
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
                            //    dLog.EventName = "Address Info Update (Present) (Candidate)";
                            //    dLog.PageName = "ApplicationAddress.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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

                        } //end if-else
                    }
                    else  // address does not exist, create new address details, then create new address with reference to address details
                    {
                        DAL.AddressDetail newAddDtl = new DAL.AddressDetail();

                        newAddDtl.AddressLine = txtPresentAddress.Text;
                        newAddDtl.DistrictID = Int32.Parse(ddlPresentDistrict.SelectedValue);
                        newAddDtl.DivisionID = Int32.Parse(ddlPresentDivision.SelectedValue);
                        newAddDtl.Area = null;
                        newAddDtl.Village = null;
                        newAddDtl.PostOffice = null;
                        newAddDtl.HouseNo = null;
                        newAddDtl.RoadNo = null;
                        newAddDtl.SectionBlockNo = null;
                        newAddDtl.Thana = null;
                        newAddDtl.CountryID = Int32.Parse(ddlPresentCountry.SelectedValue);
                        newAddDtl.PostCode = txtPresentPostCode.Text;
                        newAddDtl.Upazila = txtPresentUpazila.Text;
                        newAddDtl.Phone = txtPresentPhone.Text;

                        newAddDtl.CreatedBy = cId;
                        newAddDtl.DateCreated = DateTime.Now;

                        DAL.Address newAddress = new DAL.Address();

                        newAddress.CandidateID = cId;
                        newAddress.AddressTypeID = 2; //present
                        newAddress.CreatedBy = cId;
                        newAddress.DateCreated = DateTime.Now;

                        using (var dbInsAddDtl = new CandidateDataManager())
                        {
                            dbInsAddDtl.Insert<DAL.AddressDetail>(newAddDtl);
                            newAddress.AddressDetailsID = newAddDtl.ID;
                        }
                        using (var dbInsAdd = new CandidateDataManager())
                        {
                            dbInsAdd.Insert<DAL.Address>(newAddress);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(newAddress, newAddDtl);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Address Info Insert (Present) (Admin)";
                            //dLog.PageName = "CandApplicationAddress.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Address Information.";
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
                            dLog.EventName = "Address Info Insert (Present) (Candidate)";
                            dLog.PageName = "ApplicationAddress.aspx";
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
                        //    dLog.EventName = "Address Info Insert (Present) (Candidate)";
                        //    dLog.PageName = "ApplicationAddress.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Address Information.";
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

                    } //end if-else
                    #endregion

                    #region PERMANENT ADDRESS
                    if (permAdd != null) // address exist, check address details' existence
                    {
                        DAL.AddressDetail permAddDtl = permAdd.AddressDetail;
                        if (permAddDtl != null) // address details exists, update
                        {
                            logOldObject = string.Empty;
                            logOldObject = GenerateLogStringFromObject(permAdd, permAddDtl);

                            permAddDtl.AddressLine = txtPermanentAddress.Text;
                            permAddDtl.DistrictID = Int32.Parse(ddlPermanentDistrict.SelectedValue);
                            permAddDtl.DivisionID = Int32.Parse(ddlPermanentDivision.SelectedValue);
                            permAddDtl.Area = null;
                            permAddDtl.Village = null;
                            permAddDtl.PostOffice = null;
                            permAddDtl.HouseNo = null;
                            permAddDtl.RoadNo = null;
                            permAddDtl.SectionBlockNo = null;
                            permAddDtl.Thana = null;
                            permAddDtl.CountryID = Int32.Parse(ddlPermanentCountry.SelectedValue);
                            permAddDtl.PostCode = txtPermanentPostCode.Text;
                            permAddDtl.Upazila = txtPermanentUpazila.Text;
                            permAddDtl.Phone = txtPermanentPhone.Text;

                            permAddDtl.ModifiedBy = cId;
                            permAddDtl.DateModified = DateTime.Now;

                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.AddressDetail>(permAddDtl);
                            }


                            logNewObject = string.Empty;
                            logNewObject = GenerateLogStringFromObject(permAdd, permAddDtl);

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Address Info Update (Permanent) (Admin)";
                                //dLog.PageName = "CandApplicationAddress.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                                dLog.EventName = "Address Info Update (Permanent) (Candidate)";
                                dLog.PageName = "ApplicationAddress.aspx";
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
                            //    dLog.EventName = "Address Info Update (Permanent) (Candidate)";
                            //    dLog.PageName = "ApplicationAddress.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                        else // address details does not exist. create new address detials, then update address
                        {
                            DAL.AddressDetail newAddressDetail = new DAL.AddressDetail();
                            newAddressDetail.AddressLine = txtPermanentAddress.Text;
                            newAddressDetail.DistrictID = Int32.Parse(ddlPermanentDistrict.SelectedValue);
                            newAddressDetail.DivisionID = Int32.Parse(ddlPermanentDivision.SelectedValue);
                            newAddressDetail.Area = null;
                            newAddressDetail.Village = null;
                            newAddressDetail.PostOffice = null;
                            newAddressDetail.HouseNo = null;
                            newAddressDetail.RoadNo = null;
                            newAddressDetail.SectionBlockNo = null;
                            newAddressDetail.Thana = null;
                            newAddressDetail.CountryID = Int32.Parse(ddlPermanentCountry.SelectedValue);
                            newAddressDetail.PostCode = txtPermanentPostCode.Text;
                            newAddressDetail.Upazila = txtPermanentUpazila.Text;
                            newAddressDetail.Phone = txtPermanentPhone.Text;

                            long newAddressDetailsID = -1;
                            using (var dbInsertNewAddressDetails = new CandidateDataManager())
                            {
                                dbInsertNewAddressDetails.Insert<DAL.AddressDetail>(newAddressDetail);
                                newAddressDetailsID = newAddressDetail.ID;
                            }
                            if (newAddressDetailsID > 0)
                            {
                                DAL.Address addressObjToBeUpdated = null;
                                using (var dbGetAddressDetailObj = new CandidateDataManager())
                                {
                                    //probably an unnecessary call to db...
                                    addressObjToBeUpdated = dbGetAddressDetailObj.GetAddressByCandidateIDAddressTypeID_ND(cId, 3); // 3 = permanent
                                }
                                if (addressObjToBeUpdated != null)
                                {
                                    addressObjToBeUpdated.AddressDetailsID = newAddressDetailsID;
                                    using (var dbUpdateAddressObj = new CandidateDataManager())
                                    {
                                        dbUpdateAddressObj.Update<DAL.Address>(addressObjToBeUpdated);
                                    }
                                }

                                logNewObject = string.Empty;
                                logNewObject = GenerateLogStringFromObject(addressObjToBeUpdated, newAddressDetail);
                            }

                            

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "Address Info Update (Permanent) (Admin)";
                                //dLog.PageName = "CandApplicationAddress.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                                dLog.EventName = "Address Info Update (Permanent) (Candidate)";
                                dLog.PageName = "ApplicationAddress.aspx";
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
                            //    dLog.EventName = "Address Info Update (Permanent) (Candidate)";
                            //    dLog.PageName = "ApplicationAddress.aspx";
                            //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                        } // end if-else
                    }
                    else // address does not exist, create new address details, then create new address with reference to address details
                    {
                        DAL.AddressDetail newAddDtl = new DAL.AddressDetail();

                        newAddDtl.AddressLine = txtPermanentAddress.Text;
                        newAddDtl.DistrictID = Int32.Parse(ddlPermanentDistrict.SelectedValue);
                        newAddDtl.DivisionID = Int32.Parse(ddlPermanentDivision.SelectedValue);
                        newAddDtl.Area = null;
                        newAddDtl.Village = null;
                        newAddDtl.PostOffice = null;
                        newAddDtl.HouseNo = null;
                        newAddDtl.RoadNo = null;
                        newAddDtl.SectionBlockNo = null;
                        newAddDtl.Thana = null;
                        newAddDtl.CountryID = Int32.Parse(ddlPermanentCountry.SelectedValue);
                        newAddDtl.PostCode = txtPermanentPostCode.Text;
                        newAddDtl.Upazila = txtPermanentUpazila.Text;
                        newAddDtl.Phone = txtPermanentPhone.Text;

                        newAddDtl.CreatedBy = cId;
                        newAddDtl.DateCreated = DateTime.Now;

                        DAL.Address newAddress = new DAL.Address();

                        newAddress.CandidateID = cId;
                        newAddress.AddressTypeID = 3; //permanent
                        newAddress.CreatedBy = cId;
                        newAddress.DateCreated = DateTime.Now;

                        using (var dbInsAddDtl = new CandidateDataManager())
                        {
                            dbInsAddDtl.Insert<DAL.AddressDetail>(newAddDtl);
                            newAddress.AddressDetailsID = newAddDtl.ID;
                        }
                        using (var dbInsAdd = new CandidateDataManager())
                        {
                            dbInsAdd.Insert<DAL.Address>(newAddress);
                        }

                        logNewObject = string.Empty;
                        logNewObject = GenerateLogStringFromObject(newAddress, newAddDtl);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //dLog.DateCreated = DateTime.Now;
                            //dLog.EventName = "Address Info Insert (Permanent) (Admin)";
                            //dLog.PageName = "CandApplicationAddress.aspx";
                            //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Address Information.";
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
                            dLog.EventName = "Address Info Insert (Permanent) (Candidate)";
                            dLog.PageName = "ApplicationAddress.aspx";
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
                        //    dLog.EventName = "Address Info Insert (Permanent) (Candidate)";
                        //    dLog.PageName = "ApplicationAddress.aspx";
                        //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Address Information.";
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
                    } //end if-else
                    #endregion

                }
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X010TC&type=danger", false);
                lblMessageAddress.Text = "Unable to save/update candidate information. Error Code : F01X010TC.";
                messagePanel_Address.CssClass = "alert alert-danger";
                messagePanel_Address.Visible = true;
            }


            lblMessageAddress.Text = "Address Info Updated successfully.";
            messagePanel_Address.CssClass = "alert alert-success";
            messagePanel_Address.Visible = true;

            Response.Redirect("ApplicationAttachment.aspx", false);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            btnSave_Address_Click(sender, e);
            Response.Redirect("ApplicationAttachment.aspx", false);
        }

        protected void ddlPresentDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            int presentDivisionId = Int32.Parse(ddlPresentDivision.SelectedValue);
            if (presentDivisionId > 0)
            {
                ddlPresentDistrict.Enabled = true;
                using (var db = new GeneralDataManager())
                {
                    DDLHelper.Bind<DAL.District>(ddlPresentDistrict, db.AdmissionDB.Districts.Where(a => a.IsActive == true && a.DivisionID == presentDivisionId).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
                }
            }
            else
            {
                ddlPresentDistrict.Enabled = false;
            }
        }

        protected void ddlPermanentDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            int permanentDivisionId = Int32.Parse(ddlPermanentDivision.SelectedValue);
            if (permanentDivisionId > 0)
            {
                ddlPermanentDistrict.Enabled = true;
                using (var db = new GeneralDataManager())
                {
                    DDLHelper.Bind<DAL.District>(ddlPermanentDistrict, db.AdmissionDB.Districts.Where(a => a.IsActive == true && a.DivisionID == permanentDivisionId).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
                }
            }
            else
            {
                ddlPermanentDistrict.Enabled = false;
            }
        }

        protected void CheckBoxSameAsPresentAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxSameAsPresentAddress.Checked)
            {
                txtPermanentAddress.Text = txtPresentAddress.Text;
                ddlPermanentDivision.SelectedValue = ddlPresentDivision.SelectedValue;
                ddlPermanentDistrict.SelectedValue = ddlPresentDistrict.SelectedValue;
                txtPermanentUpazila.Text = txtPresentUpazila.Text;
                ddlPermanentCountry.SelectedValue = ddlPresentCountry.SelectedValue;
                txtPermanentPostCode.Text = txtPresentPostCode.Text;
                txtPermanentPhone.Text = txtPresentPhone.Text;
            }
        }


        protected string GenerateLogStringFromObject(DAL.Address address, DAL.AddressDetail addressDetail)
        {

            string result = "";

            try
            {
                #region AddressType
                if (address != null && Convert.ToInt32(address.AddressTypeID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "AddressType: " + (db.AdmissionDB.AddressTypes.Where(a => a.ID == address.AddressTypeID).Select(x => x.AddressTypeName).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "AddressType: ; ";
                }
                #endregion

                result += "Address: " + (!string.IsNullOrEmpty(addressDetail.AddressLine) ? addressDetail.AddressLine.ToString() : "") + "; ";
                result += "RoadNo: " + (!string.IsNullOrEmpty(addressDetail.RoadNo) ? addressDetail.RoadNo.ToString() : "") + "; ";
                result += "RoadName: " + (!string.IsNullOrEmpty(addressDetail.RoadName) ? addressDetail.RoadName.ToString() : "") + "; ";
                result += "HouseNo: " + (!string.IsNullOrEmpty(addressDetail.HouseNo) ? addressDetail.HouseNo.ToString() : "") + "; ";
                result += "HouseName: " + (!string.IsNullOrEmpty(addressDetail.HouseName) ? addressDetail.HouseName.ToString() : "") + "; ";
                result += "SectionBlockNo: " + (!string.IsNullOrEmpty(addressDetail.SectionBlockNo) ? addressDetail.SectionBlockNo.ToString() : "") + "; ";
                result += "Mobile: " + (!string.IsNullOrEmpty(addressDetail.Mobile) ? addressDetail.Mobile.ToString() : "") + "; ";
                result += "Area: " + (!string.IsNullOrEmpty(addressDetail.Area) ? addressDetail.Area.ToString() : "") + "; ";
                result += "Village: " + (!string.IsNullOrEmpty(addressDetail.Village) ? addressDetail.Village.ToString() : "") + "; ";
                result += "Email: " + (!string.IsNullOrEmpty(addressDetail.Email) ? addressDetail.Email.ToString() : "") + "; ";
                result += "Thana: " + (!string.IsNullOrEmpty(addressDetail.Thana) ? addressDetail.Thana.ToString() : "") + "; ";
                result += "Upazila: " + (!string.IsNullOrEmpty(addressDetail.Upazila) ? addressDetail.Upazila.ToString() : "") + "; ";
                result += "PostCode: " + (!string.IsNullOrEmpty(addressDetail.PostCode) ? addressDetail.PostCode.ToString() : "") + "; ";
                result += "PostOffice: " + (!string.IsNullOrEmpty(addressDetail.PostOffice) ? addressDetail.PostOffice.ToString() : "") + "; ";


                #region Division
                if (addressDetail.DivisionID != null && Convert.ToInt32(addressDetail.DivisionID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Division: " + (db.AdmissionDB.Divisions.Where(a => a.ID == addressDetail.DivisionID).Select(x => x.Name).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Division: ; ";
                }
                #endregion

                #region District
                if (addressDetail.DistrictID != null && Convert.ToInt32(addressDetail.DistrictID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "District: " + (db.AdmissionDB.Districts.Where(a => a.ID == addressDetail.DistrictID).Select(x => x.Name).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "District: ; ";
                }
                #endregion

                #region Country
                if (addressDetail.CountryID != null && Convert.ToInt32(addressDetail.CountryID) > 0)
                {
                    using (var db = new GeneralDataManager())
                    {
                        result += "Country: " + (db.AdmissionDB.Countries.Where(a => a.ID == addressDetail.CountryID).Select(x => x.Name).FirstOrDefault()) + "; ";
                    }
                }
                else
                {
                    result += "Country: ; ";
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