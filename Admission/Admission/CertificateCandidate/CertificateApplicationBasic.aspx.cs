using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.CertificateCandidate
{
    public partial class CertificateApplicationBasic : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key

            DAL.CertificateBasicInfo certificateBasicInfo = new DAL.CertificateBasicInfo();
            using (var db = new CandidateDataManager())
            {
                certificateBasicInfo = db.AdmissionDB.CertificateBasicInfoes.Where(x=> x.CandidateUserID == uId).FirstOrDefault();
            }

            if (certificateBasicInfo == null) //(string.IsNullOrEmpty(Request.QueryString["val"]))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppBasic.NavigateUrl = "CertificateApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "CertificateApplicationEducation.aspx?val=" + queryVal;
                hrefAppAdditional.NavigateUrl = "CertificateApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "CertificateApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "CertificateApplicationAttachment.aspx?val=" + queryVal;
                //hrefAppPriority.NavigateUrl = "CandApplicationPriority.aspx?val=" + cId;
                hrefAppRelation.NavigateUrl = "CertificateApplicationRelation.aspx?val=" + queryVal;
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
                DDLHelper.Bind<DAL.Country>(ddlNationality, db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);
                //DDLHelper.Bind<DAL.Language>(ddlLanguage, db.AdmissionDB.Languages.Where(a => a.IsActive == true).OrderBy(a => a.LanguageName).ToList(), "LanguageName", "ID", EnumCollection.ListItemType.MotherTongue);
                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).OrderBy(a => a.GenderName).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                DDLHelper.Bind<DAL.MaritalStatu>(ddlMaritalStatus, db.AdmissionDB.MaritalStatus.Where(a => a.IsActive == true).OrderBy(a => a.MaritalStatus).ToList(), "MaritalStatus", "ID", EnumCollection.ListItemType.MaritalStatus);
                DDLHelper.Bind<DAL.BloodGroup>(ddlBloodGroup, db.AdmissionDB.BloodGroups.OrderBy(a => a.ID).ToList(), "BloodGroupName", "ID", EnumCollection.ListItemType.BloodGroup);
                DDLHelper.Bind<DAL.Religion>(ddlReligion, db.AdmissionDB.Religions.Where(a => a.IsActive == true).OrderBy(a => a.ReligionName).ToList(), "ReligionName", "ID", EnumCollection.ListItemType.Religion);
                //DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(a => a.QuotaName).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);
            }
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    //basic info-------------------------------------------------------
                    DAL.CertificateBasicInfo candidate = db.AdmissionDB.CertificateBasicInfoes.Where(x=> x.ID == cId).FirstOrDefault();
                    if (candidate != null && candidate.ID > 0)
                    {
                        txtFirstName.Text = candidate.FirstName;
                        //txtMiddleName.Text = candidate.MiddleName;
                        //txtLastName.Text = candidate.LastName;
                        //txtNickName.Text = candidate.NickName;
                        txtDateOfBirth.Text = candidate.DateOfBirth.ToString("dd/MM/yyyy");
                        //txtPlaceOfBirth.Text = candidate.PlaceOfBirth;
                        ddlNationality.SelectedValue = candidate.NationalityID.ToString();
                        //ddlLanguage.SelectedValue = candidate.MotherTongueID.ToString();
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
                        //ddlQuota.SelectedValue = candidate.QuotaID.ToString();  //required for BUP
                    }
                }// end using
            }// if(cId > 0)
        }

        protected void btnSave_Basic_Click(object sender, EventArgs e)
        {
            long cId = -1;
            DAL.CertificateBasicInfo certificateBasicInfo = new DAL.CertificateBasicInfo();
            using (var db = new CandidateDataManager())
            {
                certificateBasicInfo = db.AdmissionDB.CertificateBasicInfoes.Where(x => x.CandidateUserID == uId).FirstOrDefault();
            }

            if (certificateBasicInfo == null) //(string.IsNullOrEmpty(Request.QueryString["val"]))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                cId = certificateBasicInfo.ID;
            }

            DAL.CertificateBasicInfo candidate = new DAL.CertificateBasicInfo();

            candidate.FirstName = txtFirstName.Text.Trim().ToUpper();
            //candidate.MiddleName = txtMiddleName.Text.ToUpper();
            //candidate.LastName = txtLastName.Text.ToUpper();
            candidate.Mobile = txtMobile.Text.Trim();
            //candidate.PhoneResidence = txtPhoneRes.Text;
            //candidate.EmergencyPhone = txtPhoneEmergency.Text;
            candidate.SMSPhone = txtMobile.Text.Trim();
            candidate.Email = txtEmail.Text.ToLower().Trim();
            candidate.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
            candidate.PlaceOfBirth = "";
            candidate.NationalityID = ddlNationality.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlNationality.SelectedValue);
            candidate.MotherTongueID = (int?)null; //ddlLanguage.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlLanguage.SelectedValue);
            candidate.MaritalStatusID = ddlMaritalStatus.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlMaritalStatus.SelectedValue);
            candidate.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
            candidate.QuotaID = (int?)null; //ddlQuota.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlQuota.SelectedValue); //only for BUP
            candidate.BloodGroupID = ddlBloodGroup.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlBloodGroup.SelectedValue);
            candidate.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);
            candidate.GuardianPhone = null;
            candidate.NationalIdNumber = ""; //txtNationalId.Text.Trim();

            try
            {
                if (cId > 0)
                {
                    string logOldObject = string.Empty;
                    string logNewObject = string.Empty;

                    DAL.CertificateBasicInfo obj = new DAL.CertificateBasicInfo();
                    using (var db = new CandidateDataManager())
                    {
                        obj = db.AdmissionDB.CertificateBasicInfoes.Where(x=> x.ID ==  cId).FirstOrDefault();

                        //to retain old info in log.
                        if (obj != null)
                        {
                            logOldObject = ObjectToString.ConvertToString(obj);
                        }
                        else
                        {
                            logOldObject = string.Empty;
                        }

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
                            updateDb.Update<DAL.CertificateBasicInfo>(candidate);

                            //to retain new info in long
                            logNewObject = ObjectToString.ConvertToString(candidate);
                        }

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "btnSave_Basic_Click";
                        dLog.PageName = "CertificateApplicationBasic.aspx; " + _pageUrl;
                        dLog.OldData = logOldObject;
                        dLog.NewData = logNewObject;
                        dLog.UserId = uId;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                        //LogWriter.DataLogWriter(dLog);
                        #endregion

                    }
                    else //else save
                    {

                        candidate.DateCreated = DateTime.Now;
                        candidate.CreatedBy = -99;
                        using (var insertDb = new CandidateDataManager())
                        {
                            //insertDb.Insert<DAL.BasicInfo>(candidate);
                        }

                        string newLogObject2 = ObjectToString.ConvertToString(candidate);

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "btnSave_Basic_Click";
                        dLog.PageName = "CertificateApplicationBasic.aspx; " + _pageUrl;
                        dLog.OldData = null;
                        dLog.NewData = logNewObject;
                        dLog.UserId = uId;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                        //LogWriter.DataLogWriter(dLog);
                        #endregion

                    } // end if-else
                }
                lblMessageBasic.Text = "Basic Info updated successfully.";
                lblMessageBasic.Focus();
                messagePanel_Basic.CssClass = "alert alert-success";
                messagePanel_Basic.Visible = true;

                Response.Redirect("CertificateApplicationEducation.aspx", false);

            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X001TC&type=danger", false);
                lblMessageBasic.Text = "Unable to save/update candidate information. " + ex.Message;
                lblMessageBasic.Focus();
                messagePanel_Basic.CssClass = "alert alert-danger";
                messagePanel_Basic.Visible = true;
            }

            //lblMessageBasic.Text = "Basic Info updated successfully.";
            //messagePanel_Basic.CssClass = "alert alert-success";
            //messagePanel_Basic.Visible = true;

            //catch (Exception ex)
            //{
            //    //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X001TC&type=danger", false);
            //    lblMessageBasic.Text = "Unable to save/update candidate information. Error Code : F01X001TC";
            //    messagePanel_Basic.CssClass = "alert alert-danger";
            //    messagePanel_Basic.Visible = true;
            //}
        }
    }
}