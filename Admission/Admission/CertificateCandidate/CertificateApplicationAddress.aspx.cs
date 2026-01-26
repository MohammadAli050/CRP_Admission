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
    public partial class CertificateApplicationAddress : PageBase
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

                hrefAppAdditional.NavigateUrl = "CertificateApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "CertificateApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "CertificateApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "CertificateApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "CertificateApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                //hrefAppPriority.NavigateUrl = "CertificateApplicationPriority.aspx?val=" + cId;
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
                //division
                List<DAL.Division> divisionList = db.GetAllDivision_ND();
                DDLHelper.Bind<DAL.Division>(ddlPresentDivision, divisionList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Division);
                DDLHelper.Bind<DAL.Division>(ddlPermanentDivision, divisionList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Division);

                List<DAL.District> districtList = db.GetAllDistrict_ND();
                DDLHelper.Bind<DAL.District>(ddlPresentDistrict, districtList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
                DDLHelper.Bind<DAL.District>(ddlPermanentDistrict, districtList.Where(a => a.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.District);

                DDLHelper.Bind<DAL.Country>(ddlPresentCountry, db.AdmissionDB.Countries.Where(c => c.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);
                DDLHelper.Bind<DAL.Country>(ddlPermanentCountry, db.AdmissionDB.Countries.Where(c => c.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);
            }
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {
                using (var db = new CandidateDataManager())
                {

                    List<DAL.CertificateAddress> addressList = db.AdmissionDB.CertificateAddresses.Where(x=> x.CandidateID == cId).ToList();// db.GetAllAddressByCandidateID_AD(cId);
                    if (addressList != null)
                    {
                        DAL.CertificateAddress presAdd = addressList.Where(a => a.AddressTypeID == 2).FirstOrDefault();
                        DAL.CertificateAddress permAdd = addressList.Where(a => a.AddressTypeID == 3).FirstOrDefault();

                        if (presAdd != null)
                        {
                            DAL.CertificateAddressDetail presAddress = db.AdmissionDB.CertificateAddressDetails.Where(x => x.ID == presAdd.AddressDetailsID).FirstOrDefault();

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
                            DAL.CertificateAddressDetail permAddress = db.AdmissionDB.CertificateAddressDetails.Where(x => x.ID == permAdd.AddressDetailsID).FirstOrDefault();

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
            }// if(cId > 0)
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

        protected void btnSave_Address_Click(object sender, EventArgs e)
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
                    DAL.CertificateAddress presAdd = null;
                    DAL.CertificateAddress permAdd = null;

                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.CertificateAddress> addressList = db.AdmissionDB.CertificateAddresses.Where(x=> x.CandidateID == cId).ToList();//db.GetAllAddressByCandidateID_AD(cId);
                        if (addressList != null)
                        {
                            presAdd = addressList.Where(c => c.AddressTypeID == 2).FirstOrDefault(); //2 = present
                            permAdd = addressList.Where(c => c.AddressTypeID == 3).FirstOrDefault(); // 3 = permanent
                        }
                    }
                    #region PRESENT ADDRESS
                    if (presAdd != null) // address exist, check address details' existence
                    {
                        DAL.CertificateAddressDetail presAddDtl = new DAL.CertificateAddressDetail();
                        using (var db = new CandidateDataManager())
                        {
                            presAddDtl = db.AdmissionDB.CertificateAddressDetails.Where(x => x.ID == presAdd.AddressDetailsID).FirstOrDefault();
                        }

                            if (presAddDtl != null) // address details exists, update
                            {
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
                                    dbUpdate.Update<DAL.CertificateAddressDetail>(presAddDtl);
                                }
                            }
                            else // address details does not exist. create new address details, then update address (probably unnecessary)
                            {
                                DAL.CertificateAddressDetail newAddressDetail = new DAL.CertificateAddressDetail();
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
                                    dbInsertNewAddressDetails.Insert<DAL.CertificateAddressDetail>(newAddressDetail);
                                    newAddressDetailsID = newAddressDetail.ID;
                                }
                                if (newAddressDetailsID > 0)
                                {
                                    DAL.CertificateAddress addressObjToBeUpdated = null;
                                    using (var dbGetAddressDetailObj = new CandidateDataManager())
                                    {
                                    //probably an unnecessary call to db...
                                    addressObjToBeUpdated = dbGetAddressDetailObj.AdmissionDB.CertificateAddresses
                                                            .Where(c => c.CandidateID == cId && c.AddressTypeID == 2)
                                                            .FirstOrDefault(); //dbGetAddressDetailObj.GetAddressByCandidateIDAddressTypeID_ND(cId, 2); // 2 = present
                                    }
                                    if (addressObjToBeUpdated != null)
                                    {
                                        addressObjToBeUpdated.AddressDetailsID = newAddressDetailsID;
                                        using (var dbUpdateAddressObj = new CandidateDataManager())
                                        {
                                            dbUpdateAddressObj.Update<DAL.CertificateAddress>(addressObjToBeUpdated);
                                        }
                                    }
                                }

                            } //end if-else
                    }
                    else  // address does not exist, create new address details, then create new address with reference to address details
                    {
                        DAL.CertificateAddressDetail newAddDtl = new DAL.CertificateAddressDetail();

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

                        DAL.CertificateAddress newAddress = new DAL.CertificateAddress();

                        newAddress.CandidateID = cId;
                        newAddress.AddressTypeID = 2; //present
                        newAddress.CreatedBy = cId;
                        newAddress.DateCreated = DateTime.Now;

                        using (var dbInsAddDtl = new CandidateDataManager())
                        {
                            dbInsAddDtl.Insert<DAL.CertificateAddressDetail>(newAddDtl);
                            newAddress.AddressDetailsID = newAddDtl.ID;
                        }
                        using (var dbInsAdd = new CandidateDataManager())
                        {
                            dbInsAdd.Insert<DAL.CertificateAddress>(newAddress);
                        }
                    } //end if-else
                    #endregion

                    #region PERMANENT ADDRESS
                    if (permAdd != null) // address exist, check address details' existence
                    {
                        
                        DAL.CertificateAddressDetail permAddDtl = new DAL.CertificateAddressDetail();
                        using (var db = new CandidateDataManager())
                        {
                            permAddDtl = db.AdmissionDB.CertificateAddressDetails.Where(x => x.ID == permAdd.AddressDetailsID).FirstOrDefault();
                        }

                        if (permAddDtl != null) // address details exists, update
                        {
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
                                dbUpdate.Update<DAL.CertificateAddressDetail>(permAddDtl);
                            }
                        }
                        else // address details does not exist. create new address detials, then update address
                        {
                            DAL.CertificateAddressDetail newAddressDetail = new DAL.CertificateAddressDetail();
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
                                dbInsertNewAddressDetails.Insert<DAL.CertificateAddressDetail>(newAddressDetail);
                                newAddressDetailsID = newAddressDetail.ID;
                            }
                            if (newAddressDetailsID > 0)
                            {
                                DAL.CertificateAddress addressObjToBeUpdated = null;
                                using (var dbGetAddressDetailObj = new CandidateDataManager())
                                {
                                    //probably an unnecessary call to db...
                                    addressObjToBeUpdated = dbGetAddressDetailObj.AdmissionDB.CertificateAddresses
                                                            .Where(c => c.CandidateID == cId && c.AddressTypeID == 3)
                                                            .FirstOrDefault();//dbGetAddressDetailObj.GetAddressByCandidateIDAddressTypeID_ND(cId, 3); // 3 = permanent
                                }
                                if (addressObjToBeUpdated != null)
                                {
                                    addressObjToBeUpdated.AddressDetailsID = newAddressDetailsID;
                                    using (var dbUpdateAddressObj = new CandidateDataManager())
                                    {
                                        dbUpdateAddressObj.Update<DAL.CertificateAddress>(addressObjToBeUpdated);
                                    }
                                }
                            }
                        } // end if-else
                    }
                    else // address does not exist, create new address details, then create new address with reference to address details
                    {
                        DAL.CertificateAddressDetail newAddDtl = new DAL.CertificateAddressDetail();

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

                        DAL.CertificateAddress newAddress = new DAL.CertificateAddress();

                        newAddress.CandidateID = cId;
                        newAddress.AddressTypeID = 3; //permanent
                        newAddress.CreatedBy = cId;
                        newAddress.DateCreated = DateTime.Now;

                        using (var dbInsAddDtl = new CandidateDataManager())
                        {
                            dbInsAddDtl.Insert<DAL.CertificateAddressDetail>(newAddDtl);
                            newAddress.AddressDetailsID = newAddDtl.ID;
                        }
                        using (var dbInsAdd = new CandidateDataManager())
                        {
                            dbInsAdd.Insert<DAL.CertificateAddress>(newAddress);
                        }
                    } //end if-else
                    #endregion

                }
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X010TC&type=danger", false);
                lblMessageAddress.Text = "[Admin] Unable to save/update candidate information. " + ex.Message;
                messagePanel_Address.CssClass = "alert alert-danger";
                messagePanel_Address.Visible = true;
            }


            lblMessageAddress.Text = "Address Info Updated successfully.";
            messagePanel_Address.CssClass = "alert alert-success";
            messagePanel_Address.Visible = true;
        }
    }
}