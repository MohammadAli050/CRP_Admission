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
using System.IO;

namespace Admission.Admission.Office.CandidateInfo
{
    public partial class CandApplicationBasic : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        string userName = "";
        long paymentId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Page.Form.Enctype = "multipart/form-data";

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key
            using (var db = new OfficeDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                using (var db = new CandidateDataManager())
                {
                    if (cId > 0)
                    {
                        paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).Select(x => x.PaymentId).FirstOrDefault();
                    }// end if(obj != null && obj.ID > 0)
                }// end using


                hrefAppAdditional.NavigateUrl = "CandApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "CandApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "CandApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "CandApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "CandApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + queryVal;
                hrefAppPriority.NavigateUrl = "CandApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "CandApplicationRelation.aspx?val=" + queryVal;
                hrefAppDeclaration.NavigateUrl = "CandApplicationDeclaration.aspx?val=" + queryVal;
            }

            if (!IsPostBack)
            {
                LoadDDL();
                LoadCandidateData(cId);
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
            }
            else if (status == "clear")
            {
                lblMessageBasic.Text = string.Empty;
                messagePanel_Basic.Visible = false;
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
                DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(a => a.QuotaName).ToList(), "Remarks", "ID", EnumCollection.ListItemType.Quota);
                DDLHelper.Bind<DAL.QuotaType>(ddlSQQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 4).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.QuotaType>(ddlFFQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 2).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);




                //DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlSenateCommitteeMember, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 1).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlSyndicateCommitteeMember, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 2).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlAcademicCouncilMember, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 3).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.QuotaGoverningBodie>(ddlFinanceCommitteeMember, db.AdmissionDB.QuotaGoverningBodies.Where(a => a.IsActive == true && a.GoverningBodiesTypeId == 4).OrderBy(a => a.ID).ToList(), "GoverningBodiesName", "ID", EnumCollection.ListItemType.Select);


            }
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0 && uId > 0)
            {
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

                        if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) > 0)
                        {
                            ddlNationalIdOrBirthRegistration.SelectedValue = candidate.AttributeInt1.ToString();
                        }
                        else
                        {
                            ddlNationalIdOrBirthRegistration.SelectedValue = "1";
                        }

                        if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) == 1)
                        {
                            txtNationalIdOrBirthRegistration.Text = candidate.NationalIdNumber.ToString();
                        }
                        else if (candidate.AttributeInt1 != null && Convert.ToInt32(candidate.AttributeInt1) == 2)
                        {
                            txtNationalIdOrBirthRegistration.Text = candidate.BirthRegistrationNo.ToString();
                        }
                        else { }



                        #region Hideshow according to Candidate program

                        var isUndergradCandidate = true;

                        List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                                = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, true).ToList();

                        if (programsAppliedList.Count() > 0)
                        {
                            foreach (var item in programsAppliedList)
                            {
                                if (item.EducationCategoryID == 6)
                                {
                                    isUndergradCandidate = false;
                                }
                            }
                        }

                        if (isUndergradCandidate == true)
                        {
                            NationalityMotherTonge.Visible = false;
                            POBtdspan.Visible = false;
                            POBtdtxt.Visible = false;
                            MaritalStatusddl.Visible = false;
                            MaritalStatusspan.Visible = false;
                        }

                        #endregion

                        #region Load Quota Iinfo
                        LoadCandidateDataQuota(candidate);
                        #endregion

                        #region Load Data for Exam Venue Selection
                        try
                        {
                            int educationCategoryId = -1;
                            educationCategoryId = db.GetCandidateEducationCategoryID(candidate.ID);
                            if (educationCategoryId == 4)
                            {
                                List<DAL.CandidateFormSl> cfsList = null;
                                cfsList = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == candidate.ID).ToList();
                                if (cfsList != null && cfsList.Count > 0)
                                {
                                    gvFacultyList.DataSource = cfsList.OrderBy(x => x.ID).ToList();
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



                        #region Hide Quota and Quota Related Info For Certificate

                        try
                        {
                            DAL.CandidateFormSl formSerial = db.GetCandidateFormSlByCandID_AD(cId);

                            if (formSerial != null && formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms != null)
                            {
                                int programId = formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.Where(x => x.IsActive == true && x.AcaCalID == formSerial.AcaCalID).FirstOrDefault().ProgramID;

                                if (programId == 65 || programId == 66)
                                {

                                }
                                else
                                {

                                }

                            }

                        }
                        catch (Exception ex)
                        {
                        }

                        #endregion


                        #region For Certificate Program Hide Quota and Quota Related Information
                        try
                        {
                            if (isUndergradCandidate == false)
                            {
                                DAL.CandidateFormSl formSerial = db.GetCandidateFormSlByCandID_AD(cId);

                                if (formSerial != null && formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms != null)
                                {
                                    int programId = formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms
                                        .Where(x => x.IsActive == true && x.AcaCalID == formSerial.AcaCalID).FirstOrDefault().ProgramID;
                                    if (programId == 65 || programId == 66)
                                    {
                                        ddlQuota.SelectedValue = "7";
                                        ddlQuota_SelectedIndexChanged(null, null);
                                        divQuota.Visible = false;
                                        ddlQuota.Enabled = false;
                                    }
                                    else
                                    {
                                        divQuota.Visible = true;
                                        ddlQuota.Enabled = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion
                    }
                }// end using
            }// if(cId > 0)
            else
            {
                MessageView("Invalid Request!", "fail");
            }
        }


        #region N/A
        //protected string ObjectToString(DAL.BasicInfo obj)
        //{
        //    string result = "";
        //    List<string> messageList = new List<string>();

        //    try
        //    {
        //        if (obj != null)
        //        {
        //            messageList.Add("Name: " + obj.FirstName.ToString());
        //            messageList.Add("DBO: " + obj.DateOfBirth.ToString());
        //            messageList.Add("Email: " + obj.Email.ToString());
        //            messageList.Add("SMSPhone: " + obj.SMSPhone.ToString());
        //            #region NID & BirthNumber
        //            if (obj.AttributeInt1 != null && Convert.ToInt32(obj.AttributeInt1) == 1)
        //            {
        //                messageList.Add("NID: " + obj.NationalIdNumber.ToString());
        //            }
        //            else if (obj.AttributeInt1 != null && Convert.ToInt32(obj.AttributeInt1) == 2)
        //            {
        //                messageList.Add("BirthNumber: " + obj.BirthRegistrationNo.ToString());
        //            }
        //            else
        //            {
        //                messageList.Add("NID: " + obj.NationalIdNumber.ToString());
        //            } 
        //            #endregion
        //            if (obj.BloodGroupID != null)
        //            {
        //                messageList.Add("BloodGroupName: " + obj.BloodGroup.BloodGroupName.ToString());
        //            }
        //            if (!string.IsNullOrEmpty(obj.PlaceOfBirth))
        //            {
        //                messageList.Add("PlaceOfBirth: " + obj.PlaceOfBirth.ToString());
        //            }
        //            if (obj.NationalityID != null)
        //            {
        //                messageList.Add("Nationality: " + obj.Country.Name.ToString());
        //            }
        //            if (obj.MotherTongueID != null)
        //            {
        //                messageList.Add("MotherTongue: " + obj.Language.LanguageName.ToString());
        //            }
        //            if (obj.MaritalStatusID != null)
        //            {
        //                messageList.Add("MaritalStatus: " + obj.MaritalStatu.MaritalStatus.ToString());
        //            }
        //            if (obj.ReligionID != null)
        //            {
        //                messageList.Add("Religion: " + obj.Religion.ReligionName.ToString());
        //            }
        //            if (obj.QuotaID != null)
        //            {
        //                messageList.Add("Quota: " + obj.Quota.QuotaName.ToString());
        //            }
        //            if (obj.GenderID != null)
        //            {
        //                messageList.Add("Gender: " + obj.Gender.GenderName.ToString());
        //            }

        //        }

        //        if (messageList != null && messageList.Count > 0)
        //        {
        //            foreach (var tData in messageList)
        //            {
        //                result = result + tData.ToString() + "; ";
        //            }

        //        }

        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        return result;
        //    }
        //} 
        #endregion


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

        private Dictionary<int, string> ValidateAllFieldIsGiven(long cId)
        {

            Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
            int i = 1;

            if (txtFirstName.Text.ToString() == "")
            {
                dictErrorList.Add(i++, "Name is Empty !!");
            }

            if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            {
                dictErrorList.Add(i++, "Date of Birth is Empty !!");
            }

            if (txtEmail.Text.ToString() == "")
            {
                dictErrorList.Add(i++, "Email is Empty !!");
            }

            if (txtMobile.Text.ToString() == "")
            {
                dictErrorList.Add(i++, "Mobile No. for SMS is Empty !!");
            }

            if (txtPlaceOfBirth.Text.ToString() == "")
            {
                dictErrorList.Add(i++, "Place Of Birth is Empty !!");
            }

            if (ddlGender.SelectedValue == "-1")
            {
                dictErrorList.Add(i++, "Gender hasn't Selected !!");
            }

            if (ddlNationality.SelectedValue == "-1")
            {
                dictErrorList.Add(i++, "Nationality hasn't Selected !!");
            }

            if (ddlLanguage.SelectedValue == "-1")
            {
                dictErrorList.Add(i++, "Mother Tongue hasn't Selected !!");
            }

            if (ddlMaritalStatus.SelectedValue == "-1")
            {
                dictErrorList.Add(i++, "Marital Status hasn't Selected !!");
            }

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

                    districtId = Convert.ToInt32(ddlDistrict.SelectedValue);

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





            return dictErrorList;

        }



        protected void btnSave_Basic_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            long cId = -1;

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);
            }

            if (cId > 0 && uId > 0)
            {

                //int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);

                //if (quotaId <= 0)
                //{
                //    MessageView("Please select Quota!", "fail");
                //    return;
                //}

                //if (quotaId == 7) //7 = Non-Quota //quotaId == 4 || quotaId == 2 || quotaId == 8 //Special Quota - Freedom Fighter - Person with Disability (Physical)
                //{
                //    //Non-Quota
                //}
                //else
                //{
                //    Dictionary<int, string> dictErrorListQ = new Dictionary<int, string>();

                //    #region Check all field is fillup in Form
                //    dictErrorListQ = ValidateQuotaField();

                //    if (dictErrorListQ.Count > 0)
                //    {
                //        string massageError = "";
                //        foreach (var tData in dictErrorListQ)
                //        {
                //            massageError = massageError + tData.Key.ToString() + ") " + tData.Value.ToString() + "<br/>";
                //        }

                //        MessageView(massageError, "fail");
                //        return;
                //    }
                //    #endregion
                //}


                //Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
                //dictErrorList = ValidateAllFieldIsGiven(cId);
                //if (dictErrorList.Count > 0)
                //{
                //    string massageError = "";
                //    foreach (var tData in dictErrorList)
                //    {
                //        massageError = massageError + tData.Key.ToString() + ") " + tData.Value.ToString() + "<br/>";
                //    }

                //    MessageView(massageError, "fail");
                //    return;
                //}


                #region Check if underGraduate


                var isUndergradCandidate = true;

                using (var db = new CandidateDataManager())
                {
                    List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                        = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, true).ToList();

                    if (programsAppliedList.Count() > 0)
                    {
                        foreach (var item in programsAppliedList)
                        {
                            if (item.EducationCategoryID == 6)
                            {
                                isUndergradCandidate = false;
                            }
                        }
                    }
                }
                #endregion




                //string logObject = string.Empty;
                string logOldObject = string.Empty;
                string logNewObject = string.Empty;

                int educationCategoryId = -1;
                DAL.BasicInfo obj = null;
                DAL.CandidatePayment candidatePayment = null;
                using (var db = new CandidateDataManager())
                {
                    obj = db.GetCandidateBasicInfoByID_ND(cId);
                    educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                    candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                }

                if (obj != null)
                {
                    #region Update
                    logOldObject = GenerateLogStringFromObject(obj);

                    obj.FirstName = txtFirstName.Text.Trim().ToUpper();
                    obj.Mobile = txtMobile.Text.Trim();
                    obj.SMSPhone = txtMobile.Text.Trim();
                    obj.Email = txtEmail.Text.ToLower().Trim();
                    if (isUndergradCandidate == true)
                    {
                        obj.NationalityID = null;
                        obj.MotherTongueID = null;
                        obj.MaritalStatusID = null;
                        obj.PlaceOfBirth = null;
                    }
                    else
                    {
                        obj.NationalityID = ddlNationality.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlNationality.SelectedValue);
                        obj.MotherTongueID = ddlLanguage.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlLanguage.SelectedValue);
                        obj.MaritalStatusID = ddlMaritalStatus.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlMaritalStatus.SelectedValue);
                        obj.PlaceOfBirth = txtPlaceOfBirth.Text.Trim();
                    }
                    obj.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
                    obj.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
                    obj.QuotaID = ddlQuota.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlQuota.SelectedValue); //only for BUP
                    obj.BloodGroupID = ddlBloodGroup.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlBloodGroup.SelectedValue);
                    obj.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);

                    if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 1)
                    {
                        obj.NationalIdNumber = txtNationalIdOrBirthRegistration.Text.Trim();
                        obj.AttributeInt1 = 1;
                        obj.BirthRegistrationNo = null;
                    }
                    else if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 2)
                    {
                        obj.BirthRegistrationNo = txtNationalIdOrBirthRegistration.Text.Trim();
                        obj.AttributeInt1 = 2;
                        obj.NationalIdNumber = null;
                    }
                    else { }

                    obj.ModifiedBy = uId;
                    obj.DateModified = DateTime.Now;

                    using (var db = new CandidateDataManager())
                    {
                        db.Update<DAL.BasicInfo>(obj);

                        //to retain new info in long
                        logNewObject = GenerateLogStringFromObject(obj); //ObjectToString.ConvertToString(candidate);
                    }

                    // ==== Update / Insert Quota Information
                    SaveOrUpdateQuotaInfo(cId);

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = uId;
                        dLog.CandidateId = cId;
                        dLog.EventName = "Basic Info Update (Admin)";
                        dLog.PageName = "CandApplicationBasic.aspx";
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


                    MessageView("[Admin] Basic info updated successfully.", "success");
                    #endregion
                }
                else
                {
                    #region Insert
                    DAL.BasicInfo candidate = new DAL.BasicInfo();

                    candidate.FirstName = txtFirstName.Text.Trim().ToUpper();
                    candidate.Mobile = txtMobile.Text.Trim();
                    candidate.SMSPhone = txtMobile.Text.Trim();
                    candidate.Email = txtEmail.Text.ToLower().Trim();
                    candidate.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
                    if (isUndergradCandidate == true)
                    {
                        candidate.PlaceOfBirth = null;
                        candidate.NationalityID = null;
                        candidate.MotherTongueID = null;
                        candidate.MaritalStatusID = null;
                    }
                    else
                    {
                        candidate.PlaceOfBirth = txtPlaceOfBirth.Text.Trim();
                        candidate.NationalityID = ddlNationality.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlNationality.SelectedValue);
                        candidate.MotherTongueID = ddlLanguage.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlLanguage.SelectedValue);
                        candidate.MaritalStatusID = ddlMaritalStatus.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlMaritalStatus.SelectedValue);
                    }
                    candidate.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
                    candidate.QuotaID = ddlQuota.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlQuota.SelectedValue); //only for BUP
                    candidate.BloodGroupID = ddlBloodGroup.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlBloodGroup.SelectedValue);
                    candidate.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);

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

                    // ==== Update / Insert Quota Information
                    SaveOrUpdateQuotaInfo(cId);

                    candidate.CreatedBy = uId;
                    candidate.DateCreated = DateTime.Now;

                    using (var db = new CandidateDataManager())
                    {
                        db.Insert<DAL.BasicInfo>(candidate);

                        //to retain new info in long
                        logNewObject = GenerateLogStringFromObject(candidate); //ObjectToString.ConvertToString(candidate);
                    }

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = uId;
                        dLog.CandidateId = cId;
                        dLog.EventName = "Basic Info Insert (Admin)";
                        dLog.PageName = "CandApplicationBasic.aspx";
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

                    MessageView("[Admin] Basic info created successfully.", "success");

                    #endregion
                }

                #region Exam Venue Selection
                try
                {

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

                            districtId = Convert.ToInt32(ddlDistrict.SelectedValue);
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

                                                lblMessageBasic.Text = "No more seat is available for District: " + newDistrictName + ". Please select another district."; ;
                                                messagePanel_Basic.CssClass = "alert alert-danger";
                                                messagePanel_Basic.Visible = true;
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
                                            model.ModifiedBy = uId;
                                            model.ModifiedDate = DateTime.Now;

                                            using (var insertDb = new CandidateDataManager())
                                            {
                                                insertDb.Update<DAL.CandidateFacultyWiseDistrictSeat>(model);
                                            }

                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = uId;
                                                dLog.CandidateId = cId;
                                                dLog.EventName = "Basic Info (Venue) Update (Admin)";
                                                dLog.PageName = "CandApplicationBasic.aspx";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "User: " + userName + "; Candidate: " + obj.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Exam Venue.";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            #endregion
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
                                            modelNew.CreatedBy = uId;
                                            modelNew.CreatedDate = DateTime.Now;

                                            using (var insertDb = new CandidateDataManager())
                                            {
                                                insertDb.Insert<DAL.CandidateFacultyWiseDistrictSeat>(modelNew);
                                            }

                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = uId;
                                                dLog.CandidateId = cId;
                                                dLog.EventName = "Basic Info (Venue) Insert (Admin)";
                                                dLog.PageName = "CandApplicationBasic.aspx";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "User: " + userName + "; Candidate: " + obj.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Exam Venue.";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            #endregion


                                            #endregion
                                        }

                                    }
                                    #endregion
                                }

                            }
                            else
                            {
                                lblMessageBasic.Text = "Please Select all Faculties Exam Seat Venue !!";
                                messagePanel_Basic.CssClass = "alert alert-danger";
                                messagePanel_Basic.Visible = true;
                                return;
                            }

                        } //END: foreach
                    }
                }
                catch (Exception ex)
                {
                    lblMessageBasic.Text = "Failed to Update Exam Seat Information!! Exception: " + ex.Message.ToString();
                    messagePanel_Basic.CssClass = "alert alert-danger";
                    messagePanel_Basic.Visible = true;
                    return;
                }
                #endregion


                #region N/A
                //DAL.BasicInfo candidate = new DAL.BasicInfo();

                //candidate.FirstName = txtFirstName.Text.Trim().ToUpper();
                ////candidate.MiddleName = txtMiddleName.Text.ToUpper();
                ////candidate.LastName = txtLastName.Text.ToUpper();
                //candidate.Mobile = txtMobile.Text.Trim();
                ////candidate.PhoneResidence = txtPhoneRes.Text;
                ////candidate.EmergencyPhone = txtPhoneEmergency.Text;
                //candidate.SMSPhone = txtMobile.Text.Trim();
                //candidate.Email = txtEmail.Text.ToLower().Trim();
                //candidate.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
                //candidate.PlaceOfBirth = txtPlaceOfBirth.Text.Trim();
                //candidate.NationalityID = ddlNationality.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlNationality.SelectedValue);
                //candidate.MotherTongueID = ddlLanguage.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlLanguage.SelectedValue);
                //candidate.MaritalStatusID = ddlMaritalStatus.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlMaritalStatus.SelectedValue);
                //candidate.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
                //candidate.QuotaID = ddlQuota.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlQuota.SelectedValue); //only for BUP
                //candidate.BloodGroupID = ddlBloodGroup.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlBloodGroup.SelectedValue);
                //candidate.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);
                //candidate.GuardianPhone = null;
                ////candidate.NationalIdNumber = txtNationalId.Text.Trim();

                //if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 1)
                //{
                //    candidate.NationalIdNumber = txtNationalIdOrBirthRegistration.Text.Trim();
                //    candidate.AttributeInt1 = 1;
                //    candidate.BirthRegistrationNo = null;
                //}
                //else if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 2)
                //{
                //    candidate.BirthRegistrationNo = txtNationalIdOrBirthRegistration.Text.Trim();
                //    candidate.AttributeInt1 = 2;
                //    candidate.NationalIdNumber = null;
                //}
                //else { }

                //try
                //{
                //    if (cId > 0)
                //    {
                //        string logObject = string.Empty;
                //        string logOldObject = string.Empty;
                //        string logNewObject = string.Empty;

                //        DAL.BasicInfo obj = new DAL.BasicInfo();
                //        using (var db = new CandidateDataManager())
                //        {
                //            obj = db.GetCandidateBasicInfoByID_ND(cId);

                //            //logObject = ObjectToString(obj);
                //            //string result = ObjectToString.ConvertObjectToString<DAL.BasicInfo>(obj, "basicinfo");
                //            //string result = ObjectToString.ConvertObjectToString(obj, "basicinfo");



                //            //to retain old info in log.
                //            if (obj != null)
                //            {
                //                logOldObject = GenerateLogStringFromObject(obj); //ObjectToString.ConvertToString(obj);
                //            }
                //            else
                //            {
                //                logOldObject = string.Empty;
                //            }

                //        }// end using

                //        if (obj != null && obj.ID > 0)  //update if object exists
                //        {
                //            candidate.ID = obj.ID;
                //            candidate.CreatedBy = obj.CreatedBy;
                //            candidate.DateCreated = obj.DateCreated;
                //            candidate.DateModified = DateTime.Now;
                //            candidate.CandidateUserID = obj.CandidateUserID;
                //            candidate.ModifiedBy = obj.ID;

                //            using (var updateDb = new CandidateDataManager())
                //            {
                //                updateDb.Update<DAL.BasicInfo>(candidate);

                //                //to retain new info in long
                //                logNewObject = GenerateLogStringFromObject(candidate); //ObjectToString.ConvertToString(candidate);
                //            }

                //            // ==== Update / Insert Quota Information
                //            SaveOrUpdateQuotaInfo(candidate.ID);



                //            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                //            try
                //            {
                //                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                //                dLog.DateCreated = DateTime.Now;
                //                dLog.UserId = uId;
                //                dLog.CandidateId = cId;
                //                dLog.EventName = "btnSave_Basic_Click";
                //                dLog.PageName = "CandApplicationBasic.aspx";
                //                dLog.OldData = logOldObject;
                //                dLog.NewData = logNewObject;
                //                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                //                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                //                LogWriter.DataLogWriter(dLog);
                //            }
                //            catch (Exception ex)
                //            {
                //            }
                //            #endregion


                //            lblMessageBasic.Text = "[Admin] Basic Info updated successfully.";
                //            lblMessageBasic.Focus();
                //            messagePanel_Basic.CssClass = "alert alert-success";
                //            messagePanel_Basic.Visible = true;

                //        }
                //        else //else save
                //        {

                //            candidate.DateCreated = DateTime.Now;
                //            candidate.CreatedBy = -99;
                //            using (var insertDb = new CandidateDataManager())
                //            {
                //                //insertDb.Insert<DAL.BasicInfo>(candidate);
                //            }

                //            SaveOrUpdateQuotaInfo(cId);

                //            string newLogObject2 = GenerateLogStringFromObject(candidate); //ObjectToString.ConvertToString(candidate);

                //            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                //            try
                //            {
                //                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                //                dLog.DateCreated = DateTime.Now;
                //                dLog.UserId = uId;
                //                dLog.CandidateId = cId;
                //                dLog.EventName = "btnSave_Basic_Click";
                //                dLog.PageName = "CandApplicationBasic.aspx";
                //                dLog.OldData = logOldObject;
                //                dLog.NewData = newLogObject2;
                //                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                //                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                //                LogWriter.DataLogWriter(dLog);
                //            }
                //            catch (Exception ex)
                //            {

                //            }
                //            #endregion


                //            lblMessageBasic.Text = "[Admin] Basic Info insert successfully.";
                //            lblMessageBasic.Focus();
                //            messagePanel_Basic.CssClass = "alert alert-success";
                //            messagePanel_Basic.Visible = true;

                //        } // end if-else






                //        #region Exam Venue Selection
                //        try
                //        {
                //            int educationCategoryId = -1;
                //            using (var db = new CandidateDataManager())
                //            {
                //                educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                //            }

                //            if (educationCategoryId == 4)
                //            {
                //                int districtId = -1;
                //                long admissionSetupId = -1;
                //                long admUnitId = -1;
                //                foreach (GridViewRow gvrow in gvFacultyList.Rows)
                //                {
                //                    Label lblAdmissionUnitId = (Label)gvrow.FindControl("lblAdmissionUnitId");
                //                    Label lblAdmissionSetupId = (Label)gvrow.FindControl("lblAdmissionSetupId");
                //                    DropDownList ddlDistrict = (DropDownList)gvrow.FindControl("ddlDistrict");

                //                    districtId = Convert.ToInt32(ddlDistrict.SelectedValue);
                //                    admissionSetupId = Convert.ToInt64(lblAdmissionSetupId.Text);
                //                    admUnitId = Convert.ToInt64(lblAdmissionUnitId.Text);

                //                    if (districtId > 0 && admUnitId > 0)
                //                    {
                //                        DAL.CandidateFacultyWiseDistrictSeat model = null; //new DAL.CandidateFacultyWiseDistrictSeat();
                //                        using (var insertDb = new CandidateDataManager())
                //                        {
                //                            model = insertDb.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == cId
                //                                                                                                 && x.AdmissionSetupId == admissionSetupId).FirstOrDefault();
                //                        }

                //                        if (model != null)
                //                        {
                //                            #region Update
                //                            if (model.DistrictId == districtId)
                //                            {

                //                            }
                //                            else
                //                            {
                //                                bool isBothIncreaseAndDecreaseAreOk = true;
                //                                int newSeatCount = -1;
                //                                int oldSeatCount = -1;


                //                                DAL.DistrictSeatLimit dslNew = null;
                //                                DAL.DistrictSeatLimit dslOld = null;

                //                                using (var db = new OfficeDataManager())
                //                                {
                //                                    dslNew = db.AdmissionDB.DistrictSeatLimits.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == districtId).FirstOrDefault();
                //                                    dslOld = db.AdmissionDB.DistrictSeatLimits.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == model.DistrictId).FirstOrDefault();
                //                                }


                //                                if (dslNew != null)
                //                                {
                //                                    newSeatCount = Convert.ToInt32(dslNew.SeatFillup);
                //                                    newSeatCount = newSeatCount + 1;
                //                                    if (newSeatCount > 0 && newSeatCount <= Convert.ToInt32(dslNew.SeatLimit))
                //                                    {
                //                                        dslNew.SeatFillup = newSeatCount;

                //                                        using (var db = new OfficeDataManager())
                //                                        {
                //                                            db.Update<DAL.DistrictSeatLimit>(dslNew);
                //                                        }
                //                                    }
                //                                    else
                //                                    {
                //                                        isBothIncreaseAndDecreaseAreOk = false;

                //                                        string newDistrictName = "";
                //                                        List<DAL.ViewModels.DistrictSeatLimitVM> dslvmList = null;
                //                                        using (var db = new OfficeDataManager())
                //                                        {
                //                                            dslvmList = db.GetDistrictSeatLimitVM();
                //                                        }
                //                                        if (dslvmList != null && dslvmList.Count > 0)
                //                                        {
                //                                            newDistrictName = dslvmList.Where(x => x.DistrictId == dslNew.DistrictId).Select(x => x.DistrictName).FirstOrDefault();
                //                                        }

                //                                        lblMessageBasic.Text = "No more seat is available for District: " + newDistrictName + ". Please select another district."; ;
                //                                        messagePanel_Basic.CssClass = "alert alert-danger";
                //                                        messagePanel_Basic.Visible = true;
                //                                        return;
                //                                    }

                //                                }




                //                                if (dslOld != null)
                //                                {
                //                                    oldSeatCount = Convert.ToInt32(dslOld.SeatFillup);
                //                                    oldSeatCount = oldSeatCount - 1;
                //                                    if (oldSeatCount >= 0)
                //                                    {
                //                                        dslOld.SeatFillup = oldSeatCount;

                //                                        using (var db = new OfficeDataManager())
                //                                        {
                //                                            db.Update<DAL.DistrictSeatLimit>(dslOld);
                //                                        }
                //                                    }
                //                                    else
                //                                    {
                //                                        isBothIncreaseAndDecreaseAreOk = false;
                //                                    }

                //                                }


                //                                if (isBothIncreaseAndDecreaseAreOk == true)
                //                                {
                //                                    //model.CandidateId = cId;
                //                                    //model.AdmissionSetupId = admissionSetupId;
                //                                    model.DistrictId = districtId;
                //                                    model.ModifiedBy = cId;
                //                                    model.ModifiedDate = DateTime.Now;

                //                                    using (var insertDb = new CandidateDataManager())
                //                                    {
                //                                        insertDb.Update<DAL.CandidateFacultyWiseDistrictSeat>(model);
                //                                    }
                //                                }


                //                            }
                //                            #endregion
                //                        }
                //                        else
                //                        {
                //                            #region Create
                //                            //int newSeatCount = -1;

                //                            DAL.DistrictSeatLimit dslNew = null;

                //                            using (var db = new OfficeDataManager())
                //                            {
                //                                dslNew = db.AdmissionDB.DistrictSeatLimits.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == districtId).FirstOrDefault();
                //                            }


                //                            if (dslNew != null)
                //                            {
                //                                #region N/A
                //                                //newSeatCount = Convert.ToInt32(dslNew.SeatLimit);
                //                                //newSeatCount = newSeatCount - 1;
                //                                //if (newSeatCount >= 0)
                //                                //{
                //                                //    dslNew.SeatLimit = newSeatCount;

                //                                //    using (var db = new OfficeDataManager())
                //                                //    {
                //                                //        db.Update<DAL.DistrictSeatLimit>(dslNew);
                //                                //    }
                //                                //}
                //                                //else
                //                                //{
                //                                //    string newDistrictName = "";
                //                                //    List<DAL.ViewModels.DistrictSeatLimitVM> dslvmList = null;
                //                                //    using (var db = new OfficeDataManager())
                //                                //    {
                //                                //        dslvmList = db.GetDistrictSeatLimitVM();
                //                                //    }
                //                                //    if (dslvmList != null && dslvmList.Count > 0)
                //                                //    {
                //                                //        newDistrictName = dslvmList.Where(x => x.AdmissionUnitId == admUnitId && x.DistrictId == dslNew.DistrictId).Select(x => x.DistrictName).FirstOrDefault();
                //                                //    }

                //                                //    lblMessageBasic.Text = "No more seat is available for District: " + newDistrictName + ". Please select another district."; ;
                //                                //    messagePanel_Basic.CssClass = "alert alert-danger";
                //                                //    messagePanel_Basic.Visible = true;
                //                                //    return;
                //                                //} 
                //                                #endregion

                //                                int seatLimit = (int)dslNew.SeatLimit;
                //                                int seatFillup = (int)dslNew.SeatFillup;

                //                                seatFillup = seatFillup + 1;

                //                                if (seatFillup > seatLimit)
                //                                {

                //                                }
                //                                else
                //                                {
                //                                    dslNew.SeatFillup = seatFillup;

                //                                    using (var db = new OfficeDataManager())
                //                                    {
                //                                        db.Update<DAL.DistrictSeatLimit>(dslNew);
                //                                    }


                //                                    #region Insert -- Candidate Faculty Wise District Seat
                //                                    DAL.CandidateFacultyWiseDistrictSeat modelNew = new DAL.CandidateFacultyWiseDistrictSeat();
                //                                    modelNew.CandidateId = cId;
                //                                    modelNew.AdmissionSetupId = admissionSetupId;
                //                                    modelNew.DistrictId = districtId;
                //                                    modelNew.CreatedBy = cId;
                //                                    modelNew.CreatedDate = DateTime.Now;

                //                                    using (var insertDb = new CandidateDataManager())
                //                                    {
                //                                        insertDb.Insert<DAL.CandidateFacultyWiseDistrictSeat>(modelNew);
                //                                    }
                //                                    #endregion
                //                                }

                //                            }
                //                            #endregion
                //                        }

                //                    }
                //                    else
                //                    {
                //                        lblMessageBasic.Text = "Please Select all Faculties Exam Seat Venue !!";
                //                        messagePanel_Basic.CssClass = "alert alert-danger";
                //                        messagePanel_Basic.Visible = true;
                //                        return;
                //                    }

                //                } //END: foreach
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            lblMessageBasic.Text = "Failed to Update Exam Seat Information!! Exception: " + ex.Message.ToString();
                //            messagePanel_Basic.CssClass = "alert alert-danger";
                //            messagePanel_Basic.Visible = true;
                //            return;
                //        }
                //        #endregion



                //    }


                //}
                //catch (Exception ex)
                //{
                //    //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X001TC&type=danger", false);
                //    lblMessageBasic.Text = "[Admin] Unable to save/update candidate information. " + ex.Message;
                //    lblMessageBasic.Focus();
                //    messagePanel_Basic.CssClass = "alert alert-danger";
                //    messagePanel_Basic.Visible = true;
                //}

                //#region N/A
                ////lblMessageBasic.Text = "Basic Info updated successfully.";
                ////messagePanel_Basic.CssClass = "alert alert-success";
                ////messagePanel_Basic.Visible = true;

                ////catch (Exception ex)
                ////{
                ////    //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X001TC&type=danger", false);
                ////    lblMessageBasic.Text = "Unable to save/update candidate information. Error Code : F01X001TC";
                ////    messagePanel_Basic.CssClass = "alert alert-danger";
                ////    messagePanel_Basic.Visible = true;
                ////} 
                //#endregion 
                #endregion
            }
            else
            {
                MessageView("Invalid Request!", "fail");
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
                    panel_QuotaDocUpload_Tribal_Note.Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageView(ex.Message.ToString(), "fail");
            }
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

                int districtValue = Convert.ToInt32(ddlDistrict.SelectedValue);
                string districtStringValue = ddlDistrict.SelectedItem.Text;
                string dropdownId = ddlDistrict.ClientID;
                long admUnitId = Convert.ToInt64(lblAdmissionUnitId.Text);
                long admSetupId = Convert.ToInt64(lblAdmissionSetupId.Text);
                int acaCalId = -1;
                using (var db = new OfficeDataManager())
                {
                    acaCalId = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == admSetupId).Select(x => x.AcaCalID).FirstOrDefault();
                }

                if (districtValue > 0 && admUnitId > 0)
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

                                ddlDistrict.SelectedValue = "-1";

                                panel_Massage.Visible = true;
                                districtMassage.Text = "No more seat is available for Faculty: " + unitName + "; District: " + districtStringValue + ". Please select another district.";
                                districtMassage.ForeColor = Color.Crimson;
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
            }
            catch (Exception ex)
            {
                panel_Massage.Visible = true;
                districtMassage.Text = "Exception: " + ex.Message.ToString();
                districtMassage.ForeColor = Color.Crimson;
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
                            if ((!string.IsNullOrEmpty(ddlSQQuotaType.SelectedValue) && Convert.ToInt32(ddlSQQuotaType.SelectedValue) > 0) &&
                                (!string.IsNullOrEmpty(rblServingRetired.SelectedValue) && Convert.ToInt32(rblServingRetired.SelectedValue) > 0)
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


    }

}
