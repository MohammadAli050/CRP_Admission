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
    public partial class ApplicationFinancialGuarantor : PageBase
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
                DDLHelper.Bind<DAL.RelationType>(ddlRelationWithGuarantor, db.AdmissionDB.RelationTypes.Where(a => a.IsActive == true).ToList(), "RelationTypeName", "ID", EnumCollection.ListItemType.Select);
            }
        }

        private void LoadCandidateData(long uId)
        {
            long cId = -1;
            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }// end using
                if (cId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.FinancialGuarantorInfo finGuarantorInfo = db.GetFinancialGuarantorByCandidateID_ND(cId);
                        if (finGuarantorInfo != null)
                        {
                            //2   Father
                            //3   Mother
                            //4   Other
                            txtFinGuarantorName.Text = finGuarantorInfo.Name;
                            if (finGuarantorInfo.RelationWithCandidate == "Father")
                            {
                                ddlRelationWithGuarantor.SelectedValue = "2";
                                txtRelationWithGuarantorOthers.Enabled = false;
                            }
                            else if (finGuarantorInfo.RelationWithCandidate == "Mother")
                            {
                                ddlRelationWithGuarantor.SelectedValue = "3";
                                txtRelationWithGuarantorOthers.Enabled = false;
                            }
                            else if (finGuarantorInfo.RelationWithCandidate == null)
                            {
                                ddlRelationWithGuarantor.SelectedValue = "-1";
                                txtRelationWithGuarantorOthers.Enabled = false;
                            }
                            else
                            {
                                ddlRelationWithGuarantor.SelectedValue = "4";
                                txtRelationWithGuarantorOthers.Enabled = true;
                                txtRelationWithGuarantorOthers.Text = finGuarantorInfo.RelationWithCandidate;
                            }
                            txtFinGuarantorOccupation.Text = finGuarantorInfo.Occupation;
                            txtFinGuarantorOrganization.Text = finGuarantorInfo.Organization;
                            txtFinGuarantorPosition.Text = finGuarantorInfo.Designation;
                            txtFinGuarantorAddress.Text = finGuarantorInfo.MailingAddress;
                            txtFinGuarantorEmail.Text = finGuarantorInfo.Email;
                            txtFinGuarantorMobile.Text = finGuarantorInfo.Mobile;
                            txtFinGuarantorSourceFund.Text = finGuarantorInfo.SourceOfFund;
                        }

                    }// end using
                }// if(cId > 0)
            }// if(uId > 0)
        }

        protected void btnSave_Guarantor_Click(object sender, EventArgs e)
        {
            long cId = -1;
            using (var db = new CandidateDataManager())
            {
                cId = db.GetCandidateIdByUserID_ND(uId);
            }
            try
            {
                if (cId > 0 && uId > 0)
                {
                    DAL.FinancialGuarantorInfo finGuarInfo = null;
                    using (var db = new CandidateDataManager())
                    {
                        finGuarInfo = db.GetFinancialGuarantorByCandidateID_ND(cId);
                    } //end using
                    if (finGuarInfo != null) //fin guarantor exists, update
                    {
                        finGuarInfo.Name = txtFinGuarantorName.Text;
                        if (ddlRelationWithGuarantor.SelectedItem.Text == "Father" || ddlRelationWithGuarantor.SelectedItem.Text == "Mother")
                        {
                            finGuarInfo.RelationWithCandidate = ddlRelationWithGuarantor.SelectedItem.Text;
                        }
                        else if (ddlRelationWithGuarantor.SelectedItem.Text == "Other")
                        {
                            finGuarInfo.RelationWithCandidate = txtRelationWithGuarantorOthers.Text;
                        }
                        finGuarInfo.Occupation = txtFinGuarantorOccupation.Text;
                        finGuarInfo.Organization = txtFinGuarantorOrganization.Text;
                        finGuarInfo.Designation = txtFinGuarantorPosition.Text;
                        finGuarInfo.MailingAddress = txtFinGuarantorAddress.Text;
                        finGuarInfo.Email = txtFinGuarantorEmail.Text;
                        finGuarInfo.Mobile = txtFinGuarantorMobile.Text;
                        finGuarInfo.SourceOfFund = txtFinGuarantorSourceFund.Text;

                        finGuarInfo.DateModified = DateTime.Now;
                        finGuarInfo.ModifiedBy = cId;

                        using (var dbUpdateFG = new CandidateDataManager())
                        {
                            dbUpdateFG.Update<DAL.FinancialGuarantorInfo>(finGuarInfo);
                        }
                    }
                    else //fin guarantor does not exist, create new
                    {
                        DAL.FinancialGuarantorInfo newFinGuarInfo = new DAL.FinancialGuarantorInfo();
                        newFinGuarInfo.Name = txtFinGuarantorName.Text;
                        if (ddlRelationWithGuarantor.SelectedItem.Text == "Father" || ddlRelationWithGuarantor.SelectedItem.Text == "Mother")
                        {
                            newFinGuarInfo.RelationWithCandidate = ddlRelationWithGuarantor.SelectedItem.Text;
                        }
                        else if (ddlRelationWithGuarantor.SelectedItem.Text == "Other")
                        {
                            newFinGuarInfo.RelationWithCandidate = txtRelationWithGuarantorOthers.Text;
                        }
                        newFinGuarInfo.Occupation = txtFinGuarantorOccupation.Text;
                        newFinGuarInfo.Organization = txtFinGuarantorOrganization.Text;
                        newFinGuarInfo.Designation = txtFinGuarantorPosition.Text;
                        newFinGuarInfo.MailingAddress = txtFinGuarantorAddress.Text;
                        newFinGuarInfo.Email = txtFinGuarantorEmail.Text;
                        newFinGuarInfo.Mobile = txtFinGuarantorMobile.Text;
                        newFinGuarInfo.SourceOfFund = txtFinGuarantorSourceFund.Text;

                        newFinGuarInfo.CandidateID = cId;
                        newFinGuarInfo.CreatedBy = cId;
                        newFinGuarInfo.DateCreated = DateTime.Now;

                        using(var dbInsertFG = new CandidateDataManager())
                        {
                            dbInsertFG.Insert<DAL.FinancialGuarantorInfo>(newFinGuarInfo);
                        }
                    }

                    lblMessageFinGuar.Text = "Financial Guarantor Info Updated successfully.";
                    messagePanel_FinGuar.CssClass = "alert alert-success";
                    messagePanel_FinGuar.Visible = true;

                } //end if cid > 0

            }
            catch (Exception ex)
            {
                Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X011TC&type=danger", false);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            btnSave_Guarantor_Click(sender, e);
            Response.Redirect("ApplicationAttachment.aspx", false);
        }

        protected void ddlRelationWithGuarantor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRelationWithGuarantor.SelectedItem.Text == "Other")
            {
                txtRelationWithGuarantorOthers.Enabled = true;
                txtRelationWithGuarantorOthers.Text = string.Empty;
            }
            else
            {
                txtRelationWithGuarantorOthers.Enabled = false;
                txtRelationWithGuarantorOthers.Text = ddlRelationWithGuarantor.SelectedItem.Text;
            }
        }
    }
}