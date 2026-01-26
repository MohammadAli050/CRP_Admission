using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.HelpDesk
{
    public partial class HD_ApplicationRelation : PageBase
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
                DDLHelper.Bind<DAL.RelationType>(ddlGuardianRelation, db.AdmissionDB.RelationTypes.Where(a => a.IsActive == true && a.RelationTypeName != "Financial Guarantor").ToList(), "RelationTypeName", "ID", EnumCollection.ListItemType.Select);

                List<DAL.Country> countryList = db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList();
                if (countryList.Count() > 0)
                {
                    DDLHelper.Bind<DAL.Country>(ddlFatherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    DDLHelper.Bind<DAL.Country>(ddlMotherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    DDLHelper.Bind<DAL.Country>(ddlMotherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    DDLHelper.Bind<DAL.Country>(ddlGuardianNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
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

                DDLHelper.Bind<DAL.RelationOccupationType>(ddlFatherOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsFather == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.RelationOccupationType>(ddlMotherOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsMother == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.RelationOccupationType>(ddlGuardianOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsGuardian == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
            }
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    List<DAL.Relation> relationList = db.GetAllRelationByCandidateID_ND(cId);
                    if (relationList != null)
                    {
                        DAL.Relation fatherRelation = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();
                        DAL.Relation motherRelation = relationList.Where(a => a.RelationTypeID == 3).FirstOrDefault();
                        DAL.Relation guardianRelation = relationList.Where(a => a.RelationTypeID == 5).FirstOrDefault();
                        //DAL.Relation spouseRelation = relationList.Where(a => a.RelationTypeID == 7).FirstOrDefault();

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
                                //txtFatherOrganization.Text = father.Organization;
                                txtFatherDesignation.Text = father.Designation;
                                txtFatherEmail.Text = string.IsNullOrEmpty(father.Email) ? string.Empty : father.Email.ToLower();
                                txtFatherNationalId.Text = father.NationalIdNumber;
                                ddlFatherNationality.SelectedValue = father.NationalityID.ToString();
                                if (father.Attribute1 == null)
                                {
                                    ddlFatherOccupationType.SelectedValue = "-1";
                                }
                                else
                                {
                                    ddlFatherOccupationType.Items.FindByText(father.Attribute1).Selected = true;
                                }

                            }
                        }
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
                                //txtMotherOrganization.Text = mother.Organization;
                                txtMotherDesignation.Text = mother.Designation;
                                txtMotherNationalId.Text = mother.NationalIdNumber;
                                ddlMotherNationality.SelectedValue = mother.NationalityID.ToString();
                                if (mother.Attribute1 == null)
                                {
                                    ddlMotherOccupationType.SelectedValue = "-1";
                                }
                                else
                                {
                                    ddlMotherOccupationType.Items.FindByText(mother.Attribute1).Selected = true;
                                }
                            }
                        }
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
                                if (guardian.Attribute1 == null)
                                {
                                    ddlGuardianOccupationType.SelectedValue = "-1";
                                }
                                else
                                {
                                    ddlGuardianOccupationType.Items.FindByText(guardian.Attribute1).Selected = true;
                                }
                            }
                        }
                        //if(spouseRelation != null)
                        //{
                        //    DAL.RelationDetail spouse = db.GetRelationDetailByID_ND(spouseRelation.RelationDetailsID);
                        //    if(spouse != null)
                        //    {
                        //        txtSpouceName.Text = spouse.Name;
                        //        txtSpouseOccupation.Text = spouse.Occupation;
                        //        txtSpouseMailingAddress.Text = spouse.MailingAddress;
                        //        txtSpouseMobile.Text = spouse.Mobile;
                        //    }
                        //}
                    }
                }// end using
            }// if(cId > 0)
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
                txtFatherOrganization.Enabled = true;
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
                txtFatherOrganization.Enabled = false;

                txtFatherDesignation.Text = string.Empty;
                txtFatherEmail.Text = string.Empty;
                txtFatherMobile.Text = string.Empty;
                txtFatherNationalId.Text = string.Empty;
                txtFatherOccupation.Text = string.Empty;
                txtFatherOrganization.Text = string.Empty;
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
                txtFatherOrganization.Enabled = true;
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
            }
        }
    }
}