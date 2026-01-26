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
    public partial class HD_ApplicationAdditional : PageBase
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
                hrefAppPriority.NavigateUrl = "HD_ApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "HD_ApplicationRelation.aspx?val=" + queryVal;
            }

            if (!IsPostBack)
            {
                LoadDDL();
                LoadCandidateData(cId);
                LoadInstituteData();
            }
        }

        private void LoadDDL()
        {
            ddlAdmittedBefore.Items.Clear();
            ddlAdmittedBefore.Items.Add(new ListItem("Select", "-1"));
            ddlAdmittedBefore.Items.Add(new ListItem("Yes", "Yes"));
            ddlAdmittedBefore.Items.Add(new ListItem("No", "No"));
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {

                List<DAL.CandidateFormSl> candidateFormSlList = null;
                DAL.AdmissionSetup admSetup = null;

                using (var db = new CandidateDataManager())
                {
                    candidateFormSlList = db.GetAllCandidateFormSlByCandID_AD(cId);
                    if (candidateFormSlList != null)
                    {
                        //get only admSetup for masters.
                        admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6 || c.AdmissionSetup.EducationCategoryID == 4).Select(c => c.AdmissionSetup).FirstOrDefault();
                    }
                }

                if (admSetup != null) //if it is a masters candidate, then show occupation details and other info.
                {
                    txtCandidateAnnualIncome.Enabled = true;
                    ddlAdmittedBefore.Enabled = true;
                    panel_Occupation.Visible = true;
                }
                else //bachelors candidate, do not show occupation details and other info
                {
                    txtCandidateAnnualIncome.Enabled = false;
                    ddlAdmittedBefore.SelectedItem.Text = "No";
                    ddlAdmittedBefore.Enabled = false;
                    panel_Occupation.Visible = false;
                }

                //additional info
                using (var db = new CandidateDataManager())
                {
                    DAL.AdditionalInfo addInfo = db.GetAdditionalInfoByCandidateID_ND(cId);

                    if (addInfo != null)
                    {
                        if (addInfo.CurrentStudentId == null)
                        {
                            ddlAdmittedBefore.SelectedValue = "No";
                            txtCurrentStudentId.Text = null;
                            txtCurrentStudentId.Enabled = false;
                        }
                        else if (addInfo.CurrentStudentId != null)
                        {
                            ddlAdmittedBefore.SelectedValue = "Yes";
                            txtCurrentStudentId.Text = addInfo.CurrentStudentId;
                            txtCurrentStudentId.Enabled = true;
                        }
                        else
                        {
                            ddlAdmittedBefore.SelectedValue = "-1";
                            txtCurrentStudentId.Text = null;
                            txtCurrentStudentId.Enabled = false;
                        }

                        if (addInfo.CandidateIncome != null)
                        {
                            txtCandidateAnnualIncome.Text = addInfo.CandidateIncome.ToString();
                        }
                        else
                        {
                            txtCandidateAnnualIncome.Text = null;
                        }
                        if (addInfo.FatherAnnualIncome != null)
                        {
                            txtFatherAnnualIncome.Text = addInfo.FatherAnnualIncome.ToString();
                        }
                        else
                        {
                            txtFatherAnnualIncome.Text = null;
                        }
                        if (addInfo.MotherAnnualIncome != null)
                        {
                            txtMotherAnnualIncome.Text = addInfo.MotherAnnualIncome.ToString();
                        }
                        else
                        {
                            txtMotherAnnualIncome.Text = null;
                        }
                    }
                }// end using additional info

                //work experience
                using (var db = new CandidateDataManager())
                {
                    DAL.WorkExperience workExpObj = db.GetWorkExperienceByCandidateID_ND(cId);
                    if (workExpObj != null)
                    {
                        txtWorkDesignation.Text = workExpObj.Designation;
                        txtWorkOrganization.Text = workExpObj.Organization;
                        txtWorkAddress.Text = workExpObj.OrgAddress;
                        txtStartDateWE.Text = workExpObj.StartDate != null ? workExpObj.StartDate.Value.ToString("dd/MM/yyyy") : null;
                        txtEndDateWE.Text = workExpObj.EndDate != null ? workExpObj.EndDate.Value.ToString("dd/MM/yyyy") : null;
                    }

                } // end using work experience

                //extra curricular activity
                using (var db = new CandidateDataManager())
                {
                    List<DAL.ExtraCurricularActivity> extraCurActList = db.GetAllExtraCurricularActivityByCandidateID_ND(cId);
                    if (extraCurActList != null)
                    {
                        DAL.ExtraCurricularActivity activity1 = extraCurActList.Where(c => c.Attribute1 == "1").FirstOrDefault();
                        DAL.ExtraCurricularActivity activity2 = extraCurActList.Where(c => c.Attribute1 == "2").FirstOrDefault();

                        if (activity1 != null)
                        {
                            txtActivity1.Text = activity1.Activity;
                            txtAward1.Text = activity1.Award;
                            txtEcaDate1.Text = activity1.DateRecieved != null ? activity1.DateRecieved.Value.ToString("dd/MM/yyyy") : null;
                        }

                        if (activity2 != null)
                        {
                            txtActivity2.Text = activity2.Activity;
                            txtAward2.Text = activity2.Award;
                            txtEcaDate2.Text = activity2.DateRecieved != null ? activity2.DateRecieved.Value.ToString("dd/MM/yyyy") : null;
                        }
                    }
                } //end using extra curricular activity

            }// if(cId > 0)
        }

        private void LoadInstituteData()
        {
            using (var db = new GeneralDataManager())
            {
                DAL.Institute institute = new DAL.Institute();
                institute = db.AdmissionDB.Institutes.Find(1);
                if (institute != null || institute.ID > 0)
                {
                    lblUniShortName.Text = institute.ShortName;
                }
            }
        }

        protected void ddlAdmittedBefore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAdmittedBefore.SelectedValue == "Yes")
            {
                txtCurrentStudentId.Enabled = true;
            }
            else
            {
                txtCurrentStudentId.Enabled = false;
                txtCurrentStudentId.Text = null;
            }
        }
    }
}