using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Script.Serialization;
using System.IO;

namespace Admission.Admission.Candidate
{
    public partial class PersonalInfoAppForm : PageBase
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
                    //paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID && x.IsPaid == true).Select(x => x.PaymentId).FirstOrDefault();
                }// end if(obj != null && obj.ID > 0)
            }// end using

            if (!IsPostBack)
            {
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

                divSc.Visible = false;
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

        private void LoadCandidateData(long uId)
        {
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
                        using (var db = new CandidateDataManager())
                        {
                            //basic info-------------------------------------------------------
                            DAL.BasicInfo candidate = db.GetCandidateBasicInfoByID_ND(cId);
                            if (candidate != null && candidate.ID > 0)
                            {
                                txtName.Text = candidate.FirstName;
                                txtMiddleName.Text = candidate.MiddleName == null ? "" : candidate.MiddleName;
                                txtLastName.Text = candidate.LastName == null ? "" : candidate.LastName;
                                txtDateOfBirth.Text = candidate.DateOfBirth.ToString("dd/MM/yyyy");
                                //ddlNationality.SelectedValue = candidate.NationalityID.ToString();
                                //ddlLanguage.SelectedValue = candidate.MotherTongueID.ToString();
                                ddlGender.SelectedValue = candidate.GenderID.ToString();
                                //ddlMaritalStatus.SelectedValue = candidate.MaritalStatusID.ToString();
                                //txtNationalId.Text = candidate.NationalIdNumber;
                                //ddlBloodGroup.SelectedValue = candidate.BloodGroupID.ToString();
                                txtEmail.Text = candidate.Email.ToLower();
                                //txtPhoneRes.Text = candidate.PhoneResidence;
                                //txtPhoneEmergency.Text = candidate.EmergencyPhone;


                                txtIntMobile.Text = candidate.Mobile; // not needed now
                                txtPhone.Text = candidate.SMSPhone;

                                ddlReligion.SelectedValue = candidate.ReligionID.ToString();
                                ddlCountry.SelectedValue = candidate.NationalityID.ToString();

                                DAL.AdditionalInfo additionalinfo = new DAL.AdditionalInfo();

                                additionalinfo = db.GetAdditionalInfoByCandidateID_ND((long)cId);

                                if (additionalinfo != null)
                                {
                                    txtPassportNo.Text = additionalinfo.PassportNumber == null ? "" : additionalinfo.PassportNumber.ToString();
                                }
                                if (candidate.AttributeStr1 != null && candidate.AttributeStr1 != "")
                                {
                                    RadioButtonList1.SelectedValue = "1";
                                    divSc.Visible = true;
                                    lbldonorname.Text = candidate.AttributeStr1;

                                }
                                if (candidate.AttributeStr2 != null)
                                {
                                    RadioButtonList1.SelectedValue = "0";
                                    lblreference.Text = candidate.AttributeStr2;
                                }

                                //if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) == 1)
                                //{
                                //    txtNationalIdOrBirthRegistration.Text = candidate.NationalIdNumber.ToString();
                                //}
                                //else if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) == 2)
                                //{

                                //    txtNationalIdOrBirthRegistration.Text = candidate.BirthRegistrationNo.ToString();
                                //}
                                //else { }


                                #region Relation Info


                                List<DAL.Relation> RelationInfoList = new List<DAL.Relation>();

                                DAL.RelationDetail Father = null;
                                DAL.RelationDetail Mother = null;

                                RelationInfoList = db.GetAllRelationByCandidateID_AD((long)cId);

                                if (RelationInfoList != null && RelationInfoList.Any())
                                {
                                    Father = RelationInfoList.Where(x => x.RelationTypeID == 2).Select(x => x.RelationDetail).FirstOrDefault();
                                    Mother = RelationInfoList.Where(x => x.RelationTypeID == 3).Select(x => x.RelationDetail).FirstOrDefault();

                                    if (Father != null)
                                        txtFatherName.Text = Father.Name;
                                    if (Mother != null)
                                        txtMotherName.Text = Mother.Name;

                                }

                                #endregion

                                #region Parents Info


                                List<DAL.Address> addressList = new List<DAL.Address>();

                                DAL.AddressDetail PresentAddress = null;
                                DAL.AddressDetail ParmanentAddress = null;

                                addressList = db.GetAllAddressByCandidateID_ND((long)cId);

                                if (addressList != null && addressList.Any())
                                {
                                    PresentAddress = addressList.Where(x => x.AddressTypeID == 2).Select(x => x.AddressDetail).FirstOrDefault();
                                    ParmanentAddress = addressList.Where(x => x.AddressTypeID == 3).Select(x => x.AddressDetail).FirstOrDefault();

                                    if (PresentAddress != null)
                                        txtPresentAddress.Text = PresentAddress.AddressLine;
                                    if (ParmanentAddress != null)
                                        txtParmamentAddress.Text = ParmanentAddress.AddressLine;

                                }

                                #endregion

                            }

                            bool hasExamTypeSSCHSC = db.IsCandidateHasExamTypeSSCHSC(cId);
                            if (hasExamTypeSSCHSC == true)
                            {
                                ddlGender.Enabled = false;
                            }
                            else
                            {
                                ddlGender.Enabled = true;
                            }


                        }// end using

                    }
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {

                ddlReligion.Items.Clear();
                ddlReligion.AppendDataBoundItems = true;
                ddlGender.Items.Clear();
                ddlGender.AppendDataBoundItems = true;
                ddlCountry.Items.Clear();
                ddlCountry.AppendDataBoundItems = true;

                DDLHelper.Bind<DAL.Religion>(ddlReligion, db.AdmissionDB.Religions.Where(a => a.IsActive == true).OrderBy(a => a.ID).ToList(), "ReligionName", "ID", EnumCollection.ListItemType.Religion);
                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).OrderBy(a => a.GenderName).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                DDLHelper.Bind<DAL.Country>(ddlCountry, db.AdmissionDB.Countries.Where(a => a.IsActive == true && a.ID != 2).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);

                //DDLHelper.Bind<DAL.Country>(ddlNationality, db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);
                //ddlNationality.SelectedValue = "2";
                //DDLHelper.Bind<DAL.Language>(ddlLanguage, db.AdmissionDB.Languages.Where(a => a.IsActive == true).OrderBy(a => a.LanguageName).ToList(), "LanguageName", "ID", EnumCollection.ListItemType.MotherTongue);
                //ddlLanguage.SelectedValue = "20";
                //DDLHelper.Bind<DAL.MaritalStatu>(ddlMaritalStatus, db.AdmissionDB.MaritalStatus.Where(a => a.IsActive == true).OrderBy(a => a.ID).ToList(), "MaritalStatus", "ID", EnumCollection.ListItemType.MaritalStatus);
                //DDLHelper.Bind<DAL.BloodGroup>(ddlBloodGroup, db.AdmissionDB.BloodGroups.OrderBy(a => a.ID).ToList(), "BloodGroupName", "ID", EnumCollection.ListItemType.BloodGroup);
                //DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(a => a.OrderQuota).ToList(), "Remarks", "ID", EnumCollection.ListItemType.Quota);
                //DDLHelper.Bind<DAL.QuotaType>(ddlFFQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 2).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.QuotaType>(ddlPWDQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 8).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);



            }
        }

        protected void ddlDay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DAL.BasicInfo obj = new DAL.BasicInfo();
                using (var db = new CandidateDataManager())
                {
                    obj = db.GetCandidateBasicInfoByUserID_ND(uId);

                    cId = obj.ID;

                }// end using

                if (obj != null)
                {
                    obj.FirstName = txtName.Text.Trim();
                    obj.MiddleName = string.IsNullOrEmpty(txtMiddleName.Text) ? "" : txtMiddleName.Text;
                    obj.LastName = string.IsNullOrEmpty(txtLastName.Text) ? "" : txtLastName.Text;
                    obj.Email = txtEmail.Text.Trim();
                    obj.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
                    obj.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
                    obj.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);
                    obj.Mobile = txtIntMobile.Text.Trim();
                    obj.SMSPhone = txtPhone.Text.Trim();
                    obj.NationalityID = ddlCountry.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlCountry.SelectedValue);


                    if (!string.IsNullOrEmpty(lbldonorname.Text))
                        obj.AttributeStr1 = lbldonorname.Text.Trim();
                    else
                        obj.AttributeStr1 = "";
                    if (!string.IsNullOrEmpty(lblreference.Text))
                        obj.AttributeStr2 = lblreference.Text.Trim();
                    else
                        obj.AttributeStr2 = "";

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            db.Update<DAL.BasicInfo>(obj);

                        }// end using

                    }
                    catch (Exception ex)
                    {
                    }


                    try
                    {
                        DAL.AdditionalInfo additionalinfo = new DAL.AdditionalInfo();
                        using (var db = new CandidateDataManager())
                        {
                            additionalinfo = db.GetAdditionalInfoByCandidateID_ND((long)cId);

                            if (additionalinfo != null)
                            {
                                additionalinfo.PassportNumber = txtPassportNo.Text.Trim();
                                additionalinfo.ModifiedBy = uId;
                                additionalinfo.DateModified = DateTime.Now;

                                db.Update<DAL.AdditionalInfo>(additionalinfo);
                            }

                        }// end using
                    }
                    catch (Exception ex)
                    {
                    }


                    #region Father Mother Information 


                    List<DAL.Relation> RelationInfoList = new List<DAL.Relation>();
                    using (var db = new CandidateDataManager())
                    {
                        try
                        {
                            DAL.Relation Father = null;
                            DAL.Relation Mother = null;


                            RelationInfoList = db.GetAllRelationByCandidateID_AD((long)cId);

                            if (RelationInfoList != null && RelationInfoList.Any())
                            {

                                Father = RelationInfoList.Where(x => x.RelationTypeID == 2).FirstOrDefault();
                                Mother = RelationInfoList.Where(x => x.RelationTypeID == 3).FirstOrDefault();

                            }

                            if (Father != null)
                            {
                                var FatherObj = Father.RelationDetail;

                                if (FatherObj != null)
                                {
                                    FatherObj.Name = txtFatherName.Text.Trim();
                                    FatherObj.RelationWithGuardian = "Father";
                                    FatherObj.ModifiedBy = uId;
                                    FatherObj.DateModified = DateTime.Now;

                                    db.Update<DAL.RelationDetail>(FatherObj);
                                }

                            }
                            else
                            {
                                DAL.RelationDetail FatherDetailObj = new DAL.RelationDetail();

                                FatherDetailObj.Name = txtFatherName.Text.Trim();
                                FatherDetailObj.RelationWithGuardian = "Father";
                                FatherDetailObj.CreatedBy = uId;
                                FatherDetailObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.RelationDetail>(FatherDetailObj);

                                long Id = FatherDetailObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Relation NewObj = new DAL.Relation();

                                    NewObj.CandidateID = cId;
                                    NewObj.RelationTypeID = 2;
                                    NewObj.RelationDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Relation>(NewObj);

                                }

                            }

                            if (Mother != null)
                            {
                                var MotherObj = Mother.RelationDetail;

                                if (MotherObj != null)
                                {
                                    MotherObj.Name = txtMotherName.Text.Trim();
                                    MotherObj.RelationWithGuardian = "Mother";
                                    MotherObj.ModifiedBy = uId;
                                    MotherObj.DateModified = DateTime.Now;

                                    db.Update<DAL.RelationDetail>(MotherObj);
                                }
                            }
                            else
                            {
                                DAL.RelationDetail MotherDetailObj = new DAL.RelationDetail();

                                MotherDetailObj.Name = txtMotherName.Text.Trim();
                                MotherDetailObj.RelationWithGuardian = "Mother";
                                MotherDetailObj.CreatedBy = uId;
                                MotherDetailObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.RelationDetail>(MotherDetailObj);

                                long Id = MotherDetailObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Relation NewObj = new DAL.Relation();

                                    NewObj.CandidateID = cId;
                                    NewObj.RelationTypeID = 3;
                                    NewObj.RelationDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Relation>(NewObj);

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }// end using
                    #endregion


                    #region Address Information

                    List<DAL.Address> AddressDetailList = new List<DAL.Address>();
                    using (var db = new CandidateDataManager())
                    {
                        try
                        {
                            DAL.AddressDetail PresentAddress = null;
                            DAL.AddressDetail ParmanentAddress = null;

                            AddressDetailList = db.GetAllAddressByCandidateID_ND((long)cId);

                            if (AddressDetailList != null && AddressDetailList.Any())
                            {
                                PresentAddress = AddressDetailList.Where(x => x.AddressTypeID == 2).Select(x => x.AddressDetail).FirstOrDefault();
                                ParmanentAddress = AddressDetailList.Where(x => x.AddressTypeID == 3).Select(x => x.AddressDetail).FirstOrDefault();
                            }


                            if (PresentAddress != null)
                            {
                                PresentAddress.AddressLine = txtPresentAddress.Text.Trim();
                                PresentAddress.ModifiedBy = uId;
                                PresentAddress.DateModified = DateTime.Now;

                                db.Update<DAL.AddressDetail>(PresentAddress);

                            }
                            else
                            {
                                DAL.AddressDetail PresentObj = new DAL.AddressDetail();

                                PresentObj.AddressLine = txtPresentAddress.Text.Trim();
                                PresentObj.CreatedBy = uId;
                                PresentObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.AddressDetail>(PresentObj);

                                long Id = PresentObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Address NewObj = new DAL.Address();

                                    NewObj.CandidateID = cId;
                                    NewObj.AddressTypeID = 2;
                                    NewObj.AddressDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Address>(NewObj);

                                }
                            }

                            if (ParmanentAddress != null)
                            {
                                ParmanentAddress.AddressLine = txtParmamentAddress.Text.Trim();
                                ParmanentAddress.ModifiedBy = uId;
                                ParmanentAddress.DateModified = DateTime.Now;

                                db.Update<DAL.AddressDetail>(ParmanentAddress);
                            }
                            else
                            {
                                DAL.AddressDetail ParmanentObj = new DAL.AddressDetail();

                                ParmanentObj.AddressLine = txtParmamentAddress.Text.Trim();
                                ParmanentObj.CreatedBy = uId;
                                ParmanentObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.AddressDetail>(ParmanentObj);

                                long Id = ParmanentObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Address NewObj = new DAL.Address();

                                    NewObj.CandidateID = cId;
                                    NewObj.AddressTypeID = 3;
                                    NewObj.AddressDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Address>(NewObj);

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }


                    #endregion


                }


                Response.Redirect("~/Admission/Candidate/academicInfoAppForm.aspx", false);



            }
            catch (Exception ex)
            {
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(RadioButtonList1.SelectedValue);

            if (Id == 0)
            {
                divSc.Visible = false;
            }
            else
            {
                divSc.Visible = true;

            }
        }
    }
}