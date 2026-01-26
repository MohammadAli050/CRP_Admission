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
    public partial class HD_ApplicationAddress : PageBase
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
    }
}