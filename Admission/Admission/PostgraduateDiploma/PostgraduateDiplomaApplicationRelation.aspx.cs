using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.PostgraduateDiploma
{
    public partial class PostgraduateDiplomaApplicationRelation : PageBase
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

                hrefAppAdditional.NavigateUrl = "PostgraduateDiplomaApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "PostgraduateDiplomaApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "PostgraduateDiplomaApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "PostgraduateDiplomaApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "PostgraduateDiplomaApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                //hrefAppPriority.NavigateUrl = "CertificateApplicationPriority.aspx?val=" + cId;
                hrefAppRelation.NavigateUrl = "PostgraduateDiplomaApplicationRelation.aspx?val=" + queryVal;
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
                //DDLHelper.Bind<DAL.RelationType>(ddlGuardianRelation, db.AdmissionDB.RelationTypes.Where(a => a.IsActive == true && a.RelationTypeName != "Financial Guarantor").ToList(), "RelationTypeName", "ID", EnumCollection.ListItemType.Select);

                List<DAL.Country> countryList = db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList();
                if (countryList.Count() > 0)
                {
                    DDLHelper.Bind<DAL.Country>(ddlFatherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    DDLHelper.Bind<DAL.Country>(ddlMotherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    DDLHelper.Bind<DAL.Country>(ddlMotherNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                    //DDLHelper.Bind<DAL.Country>(ddlGuardianNationality, countryList, "Name", "ID", EnumCollection.ListItemType.Country);
                }

                ddlIsLateFather.Items.Clear();
                ddlIsLateFather.Items.Add(new ListItem("Select", "-1"));
                ddlIsLateFather.Items.Add(new ListItem("No", "0"));
                ddlIsLateFather.Items.Add(new ListItem("Yes", "1"));

                ddlIsLateMother.Items.Clear();
                ddlIsLateMother.Items.Add(new ListItem("Select", "-1"));
                ddlIsLateMother.Items.Add(new ListItem("No", "0"));
                ddlIsLateMother.Items.Add(new ListItem("Yes", "1"));

                DDLHelper.Bind<DAL.RelationOccupationType>(ddlFatherOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsFather == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.RelationOccupationType>(ddlMotherOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsMother == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.RelationOccupationType>(ddlGuardianOccupationType, db.AdmissionDB.RelationOccupationTypes.Where(c => c.IsActive == true && c.IsGuardian == true).OrderBy(c => c.OccupationTypeName).ToList(), "OccupationTypeName", "ID", EnumCollection.ListItemType.Select);
            }
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    List<DAL.PostgraduateDiplomaRelation> relationList = db.AdmissionDB.PostgraduateDiplomaRelations.Where(x => x.CandidateID == cId).ToList(); //db.GetAllRelationByCandidateID_ND(cId);
                    if (relationList != null)
                    {
                        DAL.PostgraduateDiplomaRelation fatherRelation = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();
                        DAL.PostgraduateDiplomaRelation motherRelation = relationList.Where(a => a.RelationTypeID == 3).FirstOrDefault();
                        //DAL.CertificateRelation guardianRelation = relationList.Where(a => a.RelationTypeID == 5).FirstOrDefault();
                        //DAL.Relation spouseRelation = relationList.Where(a => a.RelationTypeID == 7).FirstOrDefault();

                        #region FATHER RELATION
                        if (fatherRelation != null)
                        {
                            DAL.PostgraduateDiplomaRelationDetail father = db.AdmissionDB.PostgraduateDiplomaRelationDetails.Where(x => x.ID == fatherRelation.RelationDetailsID).FirstOrDefault();//db.GetRelationDetailByID_ND(fatherRelation.RelationDetailsID);
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
                                txtFatherOrganization.Text = father.Organization;
                                txtFatherDesignation.Text = father.Designation;
                                txtFatherEmail.Text = father.Email.ToLower();
                                txtFatherNationalId.Text = father.NationalIdNumber;
                                ddlFatherNationality.SelectedValue = father.NationalityID.ToString();
                                if (father.Attribute1 == null)
                                {
                                    ddlFatherOccupationType.SelectedValue = "-1";
                                }
                                else
                                {
                                    //ddlFatherOccupationType.SelectedValue = father.Attribute1;
                                    ddlFatherOccupationType.Items.FindByText(father.Attribute1).Selected = true;
                                }

                            }
                        }
                        #endregion
                        #region MOTHER RELATION
                        if (motherRelation != null)
                        {
                            DAL.PostgraduateDiplomaRelationDetail mother = db.AdmissionDB.PostgraduateDiplomaRelationDetails.Where(x => x.ID == motherRelation.RelationDetailsID).FirstOrDefault();//db.GetRelationDetailByID_ND(motherRelation.RelationDetailsID);
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
                                txtMotherOrganization.Text = mother.Organization;
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
                        #endregion
                        //#region GUARDIAN RELATION
                        //if (guardianRelation != null)
                        //{
                        //    DAL.RelationDetail guardian = db.GetRelationDetailByID_ND(guardianRelation.RelationDetailsID);
                        //    if (guardian != null)
                        //    {
                        //        txtGuardian_Name.Text = guardian.Name;
                        //        txtGuardianMailingAddress.Text = guardian.MailingAddress;
                        //        txtGuardianMobile.Text = guardian.Mobile;
                        //        txtGuardianOccupation.Text = guardian.Occupation;

                        //        if (guardian.RelationWithGuardian == "Father")
                        //        {
                        //            ddlGuardianRelation.SelectedValue = "2";
                        //            txtGuardianOtherRelation.Enabled = false;
                        //        }
                        //        else if (guardian.RelationWithGuardian == "Mother")
                        //        {
                        //            ddlGuardianRelation.SelectedValue = "3";
                        //            txtGuardianOtherRelation.Enabled = false;
                        //        }
                        //        else if (guardian.RelationWithGuardian == null)
                        //        {
                        //            ddlGuardianRelation.SelectedValue = "-1";
                        //            txtGuardianOtherRelation.Enabled = false;
                        //        }
                        //        else
                        //        {
                        //            ddlGuardianRelation.SelectedValue = "4";
                        //            txtGuardianOtherRelation.Text = guardian.RelationWithGuardian;
                        //            txtGuardianOtherRelation.Enabled = true;
                        //        }
                        //        txtGuardianEmail.Text = guardian.Email;
                        //        txtGuardianNationalId.Text = guardian.NationalIdNumber;
                        //        ddlGuardianNationality.SelectedValue = guardian.NationalityID.ToString();
                        //        //txtGuardianPhoneOffice.Text = guardian.PhoneOffice;
                        //        //txtGuardianPhoneRes.Text = guardian.PhoneResidence;
                        //        if (guardian.Attribute1 == null)
                        //        {
                        //            ddlGuardianOccupationType.SelectedValue = "-1";
                        //        }
                        //        else
                        //        {
                        //            ddlGuardianOccupationType.Items.FindByText(guardian.Attribute1).Selected = true;
                        //        }
                        //    }
                        //}
                        //#endregion
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
            }
            else if (ddlIsLateFather.SelectedValue == "1")
            {
                txtFatherDesignation.Enabled = false;
                txtFatherEmail.Enabled = false;
                txtFatherMobile.Enabled = false;
                txtFatherNationalId.Enabled = false;
                txtFatherOccupation.Enabled = false;
                ddlFatherOccupationType.Enabled = false;

                txtFatherDesignation.Text = string.Empty;
                txtFatherEmail.Text = string.Empty;
                txtFatherMobile.Text = string.Empty;
                txtFatherNationalId.Text = string.Empty;
                txtFatherOccupation.Text = string.Empty;
                //ddlFatherOccupationType.SelectedValue = 0;
            }
            else if (ddlIsLateFather.SelectedValue == "-1")
            {
                txtFatherDesignation.Enabled = true;
                txtFatherEmail.Enabled = true;
                txtFatherMobile.Enabled = true;
                txtFatherNationalId.Enabled = true;
                txtFatherOccupation.Enabled = true;
                ddlFatherOccupationType.Enabled = true;
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
                ddlMotherOccupationType.Enabled = true;
            }
            else if (ddlIsLateMother.SelectedValue == "1")
            {
                txtMotherDesignation.Enabled = false;
                //txtMotherEmail.Enabled = false;
                txtMotherMobile.Enabled = false;
                txtMotherNationalId.Enabled = false;
                txtMotherOccupation.Enabled = false;
                ddlMotherOccupationType.Enabled = false;

                txtMotherDesignation.Text = string.Empty;
                //txtMotherEmail.Text = string.Empty;
                txtMotherMobile.Text = string.Empty;
                txtMotherNationalId.Text = string.Empty;
                txtMotherOccupation.Text = string.Empty;
            }
            else if (ddlIsLateMother.SelectedValue == "-1")
            {
                txtMotherDesignation.Enabled = true;
                //txtMotherEmail.Enabled = true;
                txtMotherMobile.Enabled = true;
                txtMotherNationalId.Enabled = true;
                txtMotherOccupation.Enabled = true;
                ddlMotherOccupationType.Enabled = true;
            }
        }

        //protected void ddlGuardianRelation_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlGuardianRelation.SelectedItem.Text == "Other") // other
        //    {
        //        txtGuardianOtherRelation.Enabled = true;
        //        txtGuardianOtherRelation.Text = string.Empty;
        //        txtGuardian_Name.Text = null;
        //        txtGuardianOccupation.Text = null;
        //        txtGuardianMobile.Text = null;
        //        txtGuardianEmail.Text = null;
        //        txtGuardianMailingAddress.Text = null;
        //        txtGuardianNationalId.Text = null;
        //        ddlGuardianNationality.SelectedValue = null;
        //        ddlGuardianOccupationType.SelectedValue = null;
        //    }
        //    else if (ddlGuardianRelation.SelectedItem.Text == "Father") //father
        //    {
        //        txtGuardianOtherRelation.Enabled = false;
        //        txtGuardianOtherRelation.Text = string.Empty;
        //        txtGuardian_Name.Text = txtFatherName.Text;
        //        txtGuardianOccupation.Text = txtFatherOccupation.Text;
        //        txtGuardianMobile.Text = txtFatherMobile.Text;
        //        txtGuardianEmail.Text = txtFatherEmail.Text.ToLower();
        //        //txtGuardianMailingAddress.Text
        //        txtGuardianNationalId.Text = txtFatherNationalId.Text;
        //        ddlGuardianNationality.SelectedValue = ddlFatherNationality.SelectedValue;
        //        ddlGuardianOccupationType.SelectedValue = ddlFatherOccupationType.SelectedValue;
        //    }
        //    else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
        //    {
        //        txtGuardianOtherRelation.Enabled = false;
        //        txtGuardianOtherRelation.Text = string.Empty;
        //        txtGuardian_Name.Text = txtMotherName.Text;
        //        txtGuardianOccupation.Text = txtMotherOccupation.Text;
        //        txtGuardianMobile.Text = txtMotherMobile.Text;
        //        //txtGuardianEmail.Text = .Text;
        //        //txtGuardianMailingAddress.Text
        //        txtGuardianNationalId.Text = txtMotherNationalId.Text;
        //        ddlGuardianNationality.SelectedValue = ddlMotherNationality.SelectedValue;
        //        ddlGuardianOccupationType.SelectedValue = ddlMotherOccupationType.SelectedValue;
        //    }
        //    else if (ddlGuardianRelation.SelectedValue == "-1")
        //    {
        //        txtGuardianOtherRelation.Enabled = false;
        //        txtGuardianOtherRelation.Text = string.Empty;
        //        txtGuardian_Name.Text = null;
        //        txtGuardianOccupation.Text = null;
        //        txtGuardianMobile.Text = null;
        //        txtGuardianEmail.Text = null;
        //        txtGuardianMailingAddress.Text = null;
        //        txtGuardianNationalId.Text = null;
        //        ddlGuardianNationality.SelectedValue = null;
        //        ddlGuardianOccupationType.SelectedValue = null;
        //    }
        //}

        protected void btnSave_Parent_Click(object sender, EventArgs e)
        {
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

            try
            {
                if (cId > 0)
                {
                    DAL.PostgraduateDiplomaRelation father = null;
                    DAL.PostgraduateDiplomaRelation mother = null;
                    //DAL.Relation guardian = null;

                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.PostgraduateDiplomaRelation> relationList = null;

                        relationList = db.AdmissionDB.PostgraduateDiplomaRelations.Where(x => x.CandidateID == cId).ToList();//db.GetAllRelationByCandidateID_AD(cId);

                        if (relationList != null)
                        {
                            father = relationList.Where(a => a.RelationTypeID == 2).FirstOrDefault();
                            mother = relationList.Where(a => a.RelationTypeID == 3).FirstOrDefault();
                            //guardian = relationList.Where(a => a.RelationTypeID == 5).FirstOrDefault();
                        }
                    }

                    #region FATHER

                    if (father != null) // relation (father) object exist
                    {
                        //DAL.RelationDetail fatherRelationDetail = father.RelationDetail;

                        DAL.PostgraduateDiplomaRelationDetail fatherRelationDetail = new DAL.PostgraduateDiplomaRelationDetail(); //father.RelationDetail;
                        List<DAL.PostgraduateDiplomaRelationDetail> fatherRelationDetailList = new List<DAL.PostgraduateDiplomaRelationDetail>();
                        using (var db = new CandidateDataManager())
                        {
                            fatherRelationDetailList = db.AdmissionDB.PostgraduateDiplomaRelationDetails.ToList();
                            if (fatherRelationDetailList.Count > 0 && fatherRelationDetailList != null)
                            {
                                fatherRelationDetail = fatherRelationDetailList.Where(x => x.ID == father.RelationDetailsID).FirstOrDefault();
                            }
                        }



                        if (fatherRelationDetail != null) // relationDetails (father) object exist, update
                        {
                            fatherRelationDetail.Name = txtFatherName.Text.ToUpper();
                            fatherRelationDetail.Mobile = txtFatherMobile.Text;
                            fatherRelationDetail.Occupation = txtFatherOccupation.Text.Trim();
                            //fatherRelationDetail.OrgAddress = txtFatherOrgAddress.Text;
                            fatherRelationDetail.Designation = txtFatherDesignation.Text;
                            fatherRelationDetail.Organization = txtFatherOrganization.Text;
                            fatherRelationDetail.Email = txtFatherEmail.Text.ToLower();
                            fatherRelationDetail.NationalIdNumber = txtFatherNationalId.Text;
                            fatherRelationDetail.NationalityID = Int32.Parse(ddlFatherNationality.SelectedValue.ToString());
                            if (ddlFatherOccupationType.SelectedItem.Text == "-1")
                            {
                                fatherRelationDetail.Attribute1 = null;
                            }
                            else
                            {
                                fatherRelationDetail.Attribute1 = ddlFatherOccupationType.SelectedItem.Text;
                            }

                            if (ddlIsLateFather.SelectedValue == "0")
                            {
                                fatherRelationDetail.IsLate = false;
                            }
                            else if (ddlIsLateFather.SelectedValue == "1")
                            {
                                fatherRelationDetail.IsLate = true;
                            }
                            else if (ddlIsLateFather.SelectedValue == "-1")
                            {
                                fatherRelationDetail.IsLate = null;
                            }


                            fatherRelationDetail.DateModified = DateTime.Now;
                            fatherRelationDetail.ModifiedBy = cId;

                            using (var dbUpdateFatherRelationDetails = new CandidateDataManager())
                            {
                                dbUpdateFatherRelationDetails.Update<DAL.PostgraduateDiplomaRelationDetail>(fatherRelationDetail);
                            }
                        }
                        else // relationDetails (father) does not exist, create new then update relation
                        {
                            DAL.PostgraduateDiplomaRelationDetail newFatherRelationDetails = new DAL.PostgraduateDiplomaRelationDetail();

                            newFatherRelationDetails.Name = txtFatherName.Text.ToUpper();
                            newFatherRelationDetails.Mobile = txtFatherMobile.Text;
                            newFatherRelationDetails.Occupation = txtFatherOccupation.Text.Trim();
                            //fatherRelationDetail.OrgAddress = txtFatherOrgAddress.Text;
                            newFatherRelationDetails.Designation = txtFatherDesignation.Text;
                            newFatherRelationDetails.Organization = txtFatherOrganization.Text;
                            newFatherRelationDetails.Email = txtFatherEmail.Text.ToLower();
                            newFatherRelationDetails.NationalIdNumber = txtFatherNationalId.Text;
                            newFatherRelationDetails.NationalityID = Int32.Parse(ddlFatherNationality.SelectedValue.ToString());
                            if (ddlFatherOccupationType.SelectedItem.Text == "-1")
                            {
                                newFatherRelationDetails.Attribute1 = null;
                            }
                            else
                            {
                                newFatherRelationDetails.Attribute1 = ddlFatherOccupationType.SelectedItem.Text;
                            }

                            if (ddlIsLateFather.SelectedValue == "0")
                            {
                                newFatherRelationDetails.IsLate = false;
                            }
                            else if (ddlIsLateFather.SelectedValue == "1")
                            {
                                newFatherRelationDetails.IsLate = true;
                            }
                            else if (ddlIsLateFather.SelectedValue == "-1")
                            {
                                newFatherRelationDetails.IsLate = null;
                            }

                            newFatherRelationDetails.CreatedBy = cId;
                            newFatherRelationDetails.DateCreated = DateTime.Now;

                            long newFatherRelationDetailsID = -1;
                            using (var dbInsertFatherRelationDetails = new CandidateDataManager())
                            {
                                dbInsertFatherRelationDetails.Insert<DAL.PostgraduateDiplomaRelationDetail>(newFatherRelationDetails);
                                newFatherRelationDetailsID = newFatherRelationDetails.ID;
                            }
                            if (newFatherRelationDetailsID > 0)
                            {
                                father.RelationDetailsID = newFatherRelationDetailsID;

                                using (var dbUpdateFatherRelation = new CandidateDataManager())
                                {
                                    dbUpdateFatherRelation.Update<DAL.PostgraduateDiplomaRelation>(father);
                                }
                            }
                        }
                    }
                    else // relation (father) object does not exist, first create a relation details and then relation
                    {
                        DAL.PostgraduateDiplomaRelationDetail newFatherRelationDetails = new DAL.PostgraduateDiplomaRelationDetail();

                        newFatherRelationDetails.Name = txtFatherName.Text.ToUpper();
                        newFatherRelationDetails.Mobile = txtFatherMobile.Text;
                        newFatherRelationDetails.Occupation = txtFatherOccupation.Text.Trim();
                        //newFatherRelationDetails.OrgAddress = txtFatherOrgAddress.Text;
                        newFatherRelationDetails.Designation = txtFatherDesignation.Text;
                        newFatherRelationDetails.Organization = txtFatherOrganization.Text;
                        newFatherRelationDetails.Email = txtFatherEmail.Text.ToLower();
                        newFatherRelationDetails.NationalIdNumber = txtFatherNationalId.Text;
                        newFatherRelationDetails.NationalityID = Int32.Parse(ddlFatherNationality.SelectedValue.ToString());
                        if (ddlFatherOccupationType.SelectedItem.Text == "-1")
                        {
                            newFatherRelationDetails.Attribute1 = null;
                        }
                        else
                        {
                            newFatherRelationDetails.Attribute1 = ddlFatherOccupationType.SelectedItem.Text;
                        }

                        if (ddlIsLateFather.SelectedValue == "0")
                        {
                            newFatherRelationDetails.IsLate = false;
                        }
                        else if (ddlIsLateFather.SelectedValue == "1")
                        {
                            newFatherRelationDetails.IsLate = true;
                        }
                        else if (ddlIsLateFather.SelectedValue == "-1")
                        {
                            newFatherRelationDetails.IsLate = null;
                        }

                        newFatherRelationDetails.CreatedBy = cId;
                        newFatherRelationDetails.DateCreated = DateTime.Now;

                        DAL.PostgraduateDiplomaRelation newFatherRelation = new DAL.PostgraduateDiplomaRelation();

                        newFatherRelation.CandidateID = cId;
                        newFatherRelation.RelationTypeID = 2;
                        newFatherRelation.CreatedBy = cId;
                        newFatherRelation.DateCreated = DateTime.Now;

                        using (var dbRelationDetailsInsert = new CandidateDataManager())
                        {
                            dbRelationDetailsInsert.Insert<DAL.PostgraduateDiplomaRelationDetail>(newFatherRelationDetails);
                            newFatherRelation.RelationDetailsID = newFatherRelationDetails.ID;
                        }
                        using (var dbRelationInsert = new CandidateDataManager())
                        {
                            dbRelationInsert.Insert<DAL.PostgraduateDiplomaRelation>(newFatherRelation);
                        }
                    }

                    #endregion

                    #region MOTHER

                    if (mother != null) // relation (mother) object exist
                    {
                        //DAL.RelationDetail motherRelationDetail = mother.RelationDetail;
                        DAL.PostgraduateDiplomaRelationDetail motherRelationDetail = new DAL.PostgraduateDiplomaRelationDetail(); //father.RelationDetail;
                        List<DAL.PostgraduateDiplomaRelationDetail> motherRelationDetailList = new List<DAL.PostgraduateDiplomaRelationDetail>();
                        using (var db = new CandidateDataManager())
                        {
                            motherRelationDetailList = db.AdmissionDB.PostgraduateDiplomaRelationDetails.ToList();
                            if (motherRelationDetailList.Count > 0 && motherRelationDetailList != null)
                            {
                                motherRelationDetail = motherRelationDetailList.Where(x => x.ID == mother.RelationDetailsID).FirstOrDefault();
                            }
                        }

                        if (motherRelationDetail != null) // relationDetails (mother) object exist, update
                        {
                            motherRelationDetail.Name = txtMotherName.Text.ToUpper();
                            motherRelationDetail.Mobile = txtMotherMobile.Text;
                            motherRelationDetail.Occupation = txtMotherOccupation.Text.Trim();
                            //mother_rd.MailingAddress = txtMotherMailingAddress.Text;
                            motherRelationDetail.Designation = txtMotherDesignation.Text;
                            motherRelationDetail.Organization = txtMotherOrganization.Text;
                            motherRelationDetail.NationalIdNumber = txtMotherNationalId.Text;
                            motherRelationDetail.NationalityID = Int32.Parse(ddlMotherNationality.SelectedValue.ToString());
                            if (ddlMotherOccupationType.SelectedItem.Text == "-1")
                            {
                                motherRelationDetail.Attribute1 = null;
                            }
                            else
                            {
                                motherRelationDetail.Attribute1 = ddlMotherOccupationType.SelectedItem.Text;
                            }

                            if (ddlIsLateMother.SelectedValue == "0")
                            {
                                motherRelationDetail.IsLate = false;
                            }
                            else if (ddlIsLateMother.SelectedValue == "1")
                            {
                                motherRelationDetail.IsLate = true;
                            }
                            else if (ddlIsLateMother.SelectedValue == "-1")
                            {
                                motherRelationDetail.IsLate = null;
                            }

                            motherRelationDetail.DateModified = DateTime.Now;
                            motherRelationDetail.ModifiedBy = cId;

                            using (var dbUpdateMotherRelationDetails = new CandidateDataManager())
                            {
                                dbUpdateMotherRelationDetails.Update<DAL.PostgraduateDiplomaRelationDetail>(motherRelationDetail);
                            }
                        }
                        else // relationDetails (mother) does not exist, create new then update relation
                        {
                            DAL.PostgraduateDiplomaRelationDetail newMotherRelationDetails = new DAL.PostgraduateDiplomaRelationDetail();

                            newMotherRelationDetails.Name = txtMotherName.Text.ToUpper();
                            newMotherRelationDetails.Mobile = txtMotherMobile.Text;
                            newMotherRelationDetails.Occupation = txtMotherOccupation.Text.Trim();
                            //mother_rd.MailingAddress = txtMotherMailingAddress.Text;
                            newMotherRelationDetails.Designation = txtMotherDesignation.Text;
                            newMotherRelationDetails.Organization = txtMotherOrganization.Text;
                            newMotherRelationDetails.NationalIdNumber = txtMotherNationalId.Text;
                            newMotherRelationDetails.NationalityID = Int32.Parse(ddlMotherNationality.SelectedValue.ToString());
                            if (ddlMotherOccupationType.SelectedItem.Text == "-1")
                            {
                                newMotherRelationDetails.Attribute1 = null;
                            }
                            else
                            {
                                newMotherRelationDetails.Attribute1 = ddlMotherOccupationType.SelectedItem.Text;
                            }

                            if (ddlIsLateMother.SelectedValue == "0")
                            {
                                newMotherRelationDetails.IsLate = false;
                            }
                            else if (ddlIsLateMother.SelectedValue == "1")
                            {
                                newMotherRelationDetails.IsLate = true;
                            }
                            else if (ddlIsLateMother.SelectedValue == "-1")
                            {
                                newMotherRelationDetails.IsLate = null;
                            }

                            newMotherRelationDetails.CreatedBy = cId;
                            newMotherRelationDetails.DateCreated = DateTime.Now;

                            long newMotherRelationDetailsID = -1;
                            using (var dbInsertFatherRelationDetails = new CandidateDataManager())
                            {
                                dbInsertFatherRelationDetails.Insert<DAL.PostgraduateDiplomaRelationDetail>(newMotherRelationDetails);
                                newMotherRelationDetailsID = newMotherRelationDetails.ID;
                            }
                            if (newMotherRelationDetailsID > 0)
                            {
                                mother.RelationDetailsID = newMotherRelationDetailsID;

                                using (var dbUpdateMotherRelation = new CandidateDataManager())
                                {
                                    dbUpdateMotherRelation.Update<DAL.PostgraduateDiplomaRelation>(mother);
                                }
                            }
                        }
                    }
                    else // relation (mohter) object does not exist, first create a relation details and then relation
                    {
                        DAL.PostgraduateDiplomaRelationDetail newMotherRelationDetails = new DAL.PostgraduateDiplomaRelationDetail();

                        newMotherRelationDetails.Name = txtMotherName.Text.ToUpper();
                        newMotherRelationDetails.Mobile = txtMotherMobile.Text;
                        newMotherRelationDetails.Occupation = txtMotherOccupation.Text.Trim();
                        //mother_rd.MailingAddress = txtMotherMailingAddress.Text;
                        newMotherRelationDetails.Designation = txtMotherDesignation.Text;
                        newMotherRelationDetails.Organization = txtMotherOrganization.Text;
                        newMotherRelationDetails.NationalIdNumber = txtMotherNationalId.Text;
                        newMotherRelationDetails.NationalityID = Int32.Parse(ddlMotherNationality.SelectedValue.ToString());
                        if (ddlMotherOccupationType.SelectedItem.Text == "-1")
                        {
                            newMotherRelationDetails.Attribute1 = null;
                        }
                        else
                        {
                            newMotherRelationDetails.Attribute1 = ddlMotherOccupationType.SelectedItem.Text;
                        }

                        if (ddlIsLateMother.SelectedValue == "0")
                        {
                            newMotherRelationDetails.IsLate = false;
                        }
                        else if (ddlIsLateMother.SelectedValue == "1")
                        {
                            newMotherRelationDetails.IsLate = true;
                        }
                        else if (ddlIsLateMother.SelectedValue == "-1")
                        {
                            newMotherRelationDetails.IsLate = null;
                        }

                        newMotherRelationDetails.CreatedBy = cId;
                        newMotherRelationDetails.DateCreated = DateTime.Now;

                        DAL.PostgraduateDiplomaRelation newMotherRelation = new DAL.PostgraduateDiplomaRelation();

                        newMotherRelation.CandidateID = cId;
                        newMotherRelation.RelationTypeID = 3;
                        newMotherRelation.CreatedBy = cId;
                        newMotherRelation.DateCreated = DateTime.Now;

                        using (var dbRelationDetailsInsert = new CandidateDataManager())
                        {
                            dbRelationDetailsInsert.Insert<DAL.PostgraduateDiplomaRelationDetail>(newMotherRelationDetails);
                            newMotherRelation.RelationDetailsID = newMotherRelationDetails.ID;
                        }
                        using (var dbRelationInsert = new CandidateDataManager())
                        {
                            dbRelationInsert.Insert<DAL.PostgraduateDiplomaRelation>(newMotherRelation);
                        }
                    }

                    #endregion

                    //#region GUARDIAN

                    //if (guardian != null) // relation (guardian) object exist
                    //{
                    //    DAL.RelationDetail guardianRelationDetails = guardian.RelationDetail;

                    //    if (guardianRelationDetails != null) // relationDetails (guardian) object exist, update
                    //    {
                    //        guardianRelationDetails.Name = txtGuardian_Name.Text.ToUpper();
                    //        guardianRelationDetails.Mobile = txtGuardianMobile.Text;
                    //        guardianRelationDetails.Occupation = txtGuardianOccupation.Text;
                    //        guardianRelationDetails.Organization = txtGuardianOrganization.Text;
                    //        guardianRelationDetails.MailingAddress = txtGuardianMailingAddress.Text;
                    //        if (ddlGuardianOccupationType.SelectedItem.Text == "-1")
                    //        {
                    //            guardianRelationDetails.Attribute1 = null;
                    //        }
                    //        else
                    //        {
                    //            guardianRelationDetails.Attribute1 = ddlGuardianOccupationType.SelectedItem.Text;
                    //        }

                    //        if (ddlGuardianRelation.SelectedItem.Text == "Father")
                    //        {
                    //            guardianRelationDetails.RelationWithGuardian = "Father";
                    //        }
                    //        else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                    //        {
                    //            guardianRelationDetails.RelationWithGuardian = "Mother";
                    //        }
                    //        else if (ddlGuardianRelation.SelectedItem.Text == "Other")
                    //        {
                    //            guardianRelationDetails.RelationWithGuardian = txtGuardianOtherRelation.Text;
                    //        }
                    //        guardianRelationDetails.Email = txtGuardianEmail.Text.ToLower();
                    //        //guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                    //        //guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;
                    //        guardianRelationDetails.NationalIdNumber = txtGuardianNationalId.Text;
                    //        guardianRelationDetails.NationalityID = Int32.Parse(ddlGuardianNationality.SelectedValue);

                    //        guardianRelationDetails.DateModified = DateTime.Now;
                    //        guardianRelationDetails.ModifiedBy = cId;

                    //        using (var dbUpdateGuardianRelationDetails = new CandidateDataManager())
                    //        {
                    //            dbUpdateGuardianRelationDetails.Update<DAL.RelationDetail>(guardianRelationDetails);
                    //        }
                    //    }
                    //    else // relationDetails (guardian) does not exist, create new then update relation
                    //    {
                    //        DAL.RelationDetail newGuardianRelationDetails = new DAL.RelationDetail();

                    //        newGuardianRelationDetails.Name = txtGuardian_Name.Text.ToUpper();
                    //        newGuardianRelationDetails.IsGuardian = true;
                    //        newGuardianRelationDetails.Mobile = txtGuardianMobile.Text;
                    //        newGuardianRelationDetails.Occupation = txtGuardianOccupation.Text;
                    //        newGuardianRelationDetails.Organization = txtGuardianOrganization.Text;
                    //        newGuardianRelationDetails.MailingAddress = txtGuardianMailingAddress.Text;
                    //        if (ddlGuardianOccupationType.SelectedItem.Text == "-1")
                    //        {
                    //            newGuardianRelationDetails.Attribute1 = null;
                    //        }
                    //        else
                    //        {
                    //            newGuardianRelationDetails.Attribute1 = ddlGuardianOccupationType.SelectedItem.Text;
                    //        }

                    //        if (ddlGuardianRelation.SelectedItem.Text == "Father")
                    //        {
                    //            newGuardianRelationDetails.RelationWithGuardian = "Father";
                    //        }
                    //        else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                    //        {
                    //            newGuardianRelationDetails.RelationWithGuardian = "Mother";
                    //        }
                    //        else if (ddlGuardianRelation.SelectedItem.Text == "Other")
                    //        {
                    //            newGuardianRelationDetails.RelationWithGuardian = txtGuardianOtherRelation.Text;
                    //        }
                    //        //guardian_rd.Email = txtGuardianEmail.Text;
                    //        //guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                    //        //guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;
                    //        newGuardianRelationDetails.NationalIdNumber = txtGuardianNationalId.Text;
                    //        newGuardianRelationDetails.NationalityID = Int32.Parse(ddlGuardianNationality.SelectedValue);

                    //        newGuardianRelationDetails.CreatedBy = cId;
                    //        newGuardianRelationDetails.DateCreated = DateTime.Now;

                    //        long newGuardianRelationDetailsID = -1;
                    //        using (var dbInsertGuardianRelationDetails = new CandidateDataManager())
                    //        {
                    //            dbInsertGuardianRelationDetails.Insert<DAL.RelationDetail>(newGuardianRelationDetails);
                    //            newGuardianRelationDetailsID = newGuardianRelationDetails.ID;
                    //        }
                    //        if (newGuardianRelationDetailsID > 0)
                    //        {
                    //            guardian.RelationDetailsID = newGuardianRelationDetailsID;

                    //            using (var dbUpdateGuardianRelation = new CandidateDataManager())
                    //            {
                    //                dbUpdateGuardianRelation.Update<DAL.Relation>(guardian);
                    //            }
                    //        }
                    //    }
                    //}
                    //else // relation (guardian) object does not exist, first create a relation details and then relation
                    //{
                    //    DAL.RelationDetail newGuardianRelationDetails = new DAL.RelationDetail();

                    //    newGuardianRelationDetails.Name = txtGuardian_Name.Text.ToUpper();
                    //    newGuardianRelationDetails.IsGuardian = true;
                    //    newGuardianRelationDetails.Mobile = txtGuardianMobile.Text;
                    //    newGuardianRelationDetails.Occupation = txtGuardianOccupation.Text;
                    //    newGuardianRelationDetails.Organization = txtGuardianOrganization.Text;
                    //    newGuardianRelationDetails.MailingAddress = txtGuardianMailingAddress.Text;
                    //    if (ddlGuardianOccupationType.SelectedItem.Text == "-1")
                    //    {
                    //        newGuardianRelationDetails.Attribute1 = null;
                    //    }
                    //    else
                    //    {
                    //        newGuardianRelationDetails.Attribute1 = ddlGuardianOccupationType.SelectedItem.Text;
                    //    }

                    //    if (ddlGuardianRelation.SelectedItem.Text == "Father")
                    //    {
                    //        newGuardianRelationDetails.RelationWithGuardian = "Father";
                    //    }
                    //    else if (ddlGuardianRelation.SelectedItem.Text == "Mother")
                    //    {
                    //        newGuardianRelationDetails.RelationWithGuardian = "Mother";
                    //    }
                    //    else if (ddlGuardianRelation.SelectedItem.Text == "Other")
                    //    {
                    //        newGuardianRelationDetails.RelationWithGuardian = txtGuardianOtherRelation.Text;
                    //    }
                    //    //guardian_rd.Email = txtGuardianEmail.Text;
                    //    //guardian_rd.PhoneOffice = txtGuardianPhoneOffice.Text;
                    //    //guardian_rd.PhoneResidence = txtGuardianPhoneRes.Text;
                    //    newGuardianRelationDetails.NationalIdNumber = txtGuardianNationalId.Text;
                    //    newGuardianRelationDetails.NationalityID = Int32.Parse(ddlGuardianNationality.SelectedValue);

                    //    newGuardianRelationDetails.CreatedBy = cId;
                    //    newGuardianRelationDetails.DateCreated = DateTime.Now;

                    //    DAL.Relation newGuardianRelation = new DAL.Relation();

                    //    newGuardianRelation.CandidateID = cId;
                    //    newGuardianRelation.RelationTypeID = 5;
                    //    newGuardianRelation.CreatedBy = cId;
                    //    newGuardianRelation.DateCreated = DateTime.Now;

                    //    using (var dbRelationDetailsInsert = new CandidateDataManager())
                    //    {
                    //        dbRelationDetailsInsert.Insert<DAL.RelationDetail>(newGuardianRelationDetails);
                    //        newGuardianRelation.RelationDetailsID = newGuardianRelationDetails.ID;
                    //    }
                    //    using (var dbRelationInsert = new CandidateDataManager())
                    //    {
                    //        dbRelationInsert.Insert<DAL.Relation>(newGuardianRelation);
                    //    }
                    //}

                    //#endregion

                    lblMessageParent.Text = "Info Updated successfully.";
                    messagePanel_Parent.CssClass = "alert alert-success";
                    messagePanel_Parent.Visible = true;

                    //Response.Redirect("ApplicationAddress.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessageParent.Text = "[Admin] Unable to save/update candidate information. " + ex.Message + "; " + ex.InnerException.Message;
                messagePanel_Parent.CssClass = "alert alert-danger";
                messagePanel_Parent.Visible = true;
            }
        }

    }
}