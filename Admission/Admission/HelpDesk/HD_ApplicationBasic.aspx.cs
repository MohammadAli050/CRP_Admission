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

namespace Admission.Admission.HelpDesk
{
    public partial class HD_ApplicationBasic : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppAdditional.NavigateUrl = "HD_ApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "HD_ApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "HD_ApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "HD_ApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "HD_ApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                hrefAppPriority.NavigateUrl = "HD_ApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "HD_ApplicationRelation.aspx?val=" + queryVal;
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
                DDLHelper.Bind<DAL.Language>(ddlLanguage, db.AdmissionDB.Languages.Where(a => a.IsActive == true).OrderBy(a => a.LanguageName).ToList(), "LanguageName", "ID", EnumCollection.ListItemType.MotherTongue);
                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).OrderBy(a => a.GenderName).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                DDLHelper.Bind<DAL.MaritalStatu>(ddlMaritalStatus, db.AdmissionDB.MaritalStatus.Where(a => a.IsActive == true).OrderBy(a => a.MaritalStatus).ToList(), "MaritalStatus", "ID", EnumCollection.ListItemType.MaritalStatus);
                DDLHelper.Bind<DAL.BloodGroup>(ddlBloodGroup, db.AdmissionDB.BloodGroups.OrderBy(a => a.ID).ToList(), "BloodGroupName", "ID", EnumCollection.ListItemType.BloodGroup);
                DDLHelper.Bind<DAL.Religion>(ddlReligion, db.AdmissionDB.Religions.Where(a => a.IsActive == true).OrderBy(a => a.ReligionName).ToList(), "ReligionName", "ID", EnumCollection.ListItemType.Religion);
                DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(a => a.QuotaName).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);
                DDLHelper.Bind<DAL.QuotaType>(ddlSQQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 4).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.QuotaType>(ddlFFQuotaType, db.AdmissionDB.QuotaTypes.Where(a => a.IsActive == true && a.QuotaId == 2).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);

            }
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
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
                        txtNationalId.Text = candidate.NationalIdNumber;
                        ddlBloodGroup.SelectedValue = candidate.BloodGroupID.ToString();
                        txtEmail.Text = candidate.Email.ToLower();
                        //txtPhoneRes.Text = candidate.PhoneResidence;
                        //txtPhoneEmergency.Text = candidate.EmergencyPhone;
                        //txtMobile.Text = candidate.Mobile; // not needed now
                        txtMobile.Text = candidate.SMSPhone;
                        ddlReligion.SelectedValue = candidate.ReligionID.ToString();
                        ddlQuota.SelectedValue = candidate.QuotaID.ToString();  //required for BUP

                        LoadCandidateDataQuota(candidate);
                    }


                    #region Load Data for Exam Venue Selection
                    try
                    {
                        int educationCategoryId = -1;
                        educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                        if (educationCategoryId == 4)
                        {
                            List<DAL.CandidateFormSl> cfsList = null;
                            cfsList = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId).ToList();
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


                }// end using
            }// if(cId > 0)
        }

        private void LoadCandidateDataQuota(DAL.BasicInfo candidate)
        {
            try
            {
                using (var db = new CandidateDataManager())
                {
                    if (candidate.QuotaID != null && candidate.QuotaID == 4)
                    {
                        DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidate.ID).FirstOrDefault();
                        if (qi != null)
                        {
                            ddlSQQuotaType.SelectedValue = qi.QuotaTypeId.ToString();
                            txtSQFatherOrMotherName.Text = qi.FatherMotherName;
                            txtSQRankOrDesignation.Text = qi.RankDesignation;
                            txtSQSenaNoOrBUPNo.Text = qi.SenaNoBUPNo;
                            //txtServingOrRetired.Text = qi.ServingRetired;
                            ddlSQServingOrRetired.SelectedValue = qi.ServingRetiredId.ToString();
                            txtSQJobLocation.Text = qi.JobLocation;



                            panelQuotaNote.Visible = true;

                            panelQuotaNoteSpecialQuota.Visible = true;
                            panelQuotaInfo.Visible = true;

                            panelQuotaNoteFreedomFighter.Visible = false;
                            panelFreedomFighterInfo.Visible = false;

                            panelQuotaNotePersonWithDisability.Visible = false;
                            panelPersonWithDisabilityInfo.Visible = false;
                        }
                    }
                    else if (candidate.QuotaID != null && candidate.QuotaID == 2) //Freedom Fighter
                    {
                        DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidate.ID).FirstOrDefault();
                        if (qi != null)
                        {
                            ddlFFQuotaType.SelectedValue = qi.QuotaTypeId.ToString();
                            txtFFName.Text = qi.FatherMotherName;



                            panelQuotaNote.Visible = true;

                            panelQuotaNoteSpecialQuota.Visible = false;
                            panelQuotaInfo.Visible = false;

                            panelQuotaNoteFreedomFighter.Visible = true;
                            panelFreedomFighterInfo.Visible = true;

                            panelQuotaNotePersonWithDisability.Visible = false;
                            panelPersonWithDisabilityInfo.Visible = false;
                        }
                    }
                    else if (candidate.QuotaID != null && candidate.QuotaID == 8) //Person with Disability (Physical)
                    {
                        DAL.QuotaInfo qi = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidate.ID).FirstOrDefault();
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
                        }
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
                long admissionSetupId = Convert.ToInt64(lblAdmissionSetupId.Text);

                int acaCalId = -1;
                using (var db = new OfficeDataManager())
                {
                    acaCalId = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == admissionSetupId).Select(x => x.AcaCalID).FirstOrDefault();
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

                        if (model != null && (model.SeatFillup >= model.SeatLimit))
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


















    }
}