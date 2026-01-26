using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.HelpDesk
{
    public partial class PreviewApplicationHelpDesk : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                ClearAllMessages();
                HideMessagePanels();
            }
        }

        private void ClearAllMessages()
        {
            lblMessageFilter.Text = string.Empty;
            lblMessageBasic.Text = string.Empty;
            lblMessageImage.Text = string.Empty;
            lblMessageFormPay.Text = string.Empty;
        }

        private void HideMessagePanels()
        {
            messagePanelFilter.Visible = false;
            messagePanelBasic.Visible = false;
            messagePanelImage.Visible = false;
            messagePanelFormPay.Visible = false;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            string searchText = null;

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                searchText = txtSearch.Text;
            }
            else
            {
                lblMessageFilter.Text = "Please provide a search text.";
                messagePanelFilter.CssClass = "alert alert-danger";
                messagePanelFilter.Visible = true;
            }

            if(searchText != null)
            {
                #region BASIC INFO -
                List<DAL.SPGetCandidateBasicInfoByPaymentIdMobileNo_Result> list = null;
                try
                {
                    using(var db = new OfficeDataManager())
                    {
                        list = db.AdmissionDB.SPGetCandidateBasicInfoByPaymentIdMobileNo(searchText).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessageBasic.Text = "Error getting candidate info. " + ex.Message;
                    messagePanelBasic.CssClass = "alert alert-danger";
                    messagePanelBasic.Visible = true;
                }

                if(list != null)
                {
                    if(list.Count > 0)
                    {
                        gvBasicInfo.DataSource = list;
                    }
                    else
                    {
                        gvBasicInfo.DataSource = null;
                    }
                }
                else
                {
                    gvBasicInfo.DataSource = null;
                }
                gvBasicInfo.DataBind();
                #endregion

                #region DOCUMENTS
                DAL.SPGetCandidateDocumentDetailsByPaymentIdMobileNo_Result docs = null;

                try
                {
                    using(var db =new OfficeDataManager())
                    {
                        docs = db.AdmissionDB.SPGetCandidateDocumentDetailsByPaymentIdMobileNo(searchText).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    lblMessageImage.Text = "Error getting candidate documents. " + ex.Message;
                    messagePanelImage.CssClass = "alert alert-danger";
                    messagePanelImage.Visible = true;
                }

                if(docs != null)
                {
                    imgCtrl.Src = docs.PhotoUrl;
                    signCtrl.Src = docs.SignUrl;
                }
                else
                {
                    imgCtrl.Src = string.Empty;
                    signCtrl.Src = string.Empty;
                }

                #endregion

                #region FORM SERIAL & PAYMENT

                List<DAL.SPGetCandidateFormSerialPaymentByPaymentIdMobileNo_Result> formPaymentList = null;

                try
                {
                    using(var db = new OfficeDataManager())
                    {
                        formPaymentList = db.AdmissionDB.SPGetCandidateFormSerialPaymentByPaymentIdMobileNo(searchText).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessageFormPay.Text = "Error getting candidate form and payment info. " + ex.Message;
                    messagePanelFormPay.CssClass = "alert alert-danger";
                    messagePanelFormPay.Visible = true;
                }

                if(list != null)
                {
                    if(list.Count > 0)
                    {
                        gvFormPay.DataSource = formPaymentList;
                    }
                    else
                    {
                        gvFormPay.DataSource = null;
                    }
                }
                else
                {
                    gvFormPay.DataSource = null;
                }
                gvFormPay.DataBind();

                #endregion

                #region EDUCATION

                List<DAL.SPGetCandidateExamDetailsByPaymentIdMobileNo_Result> educationList = null;

                try
                {
                    using(var db = new OfficeDataManager())
                    {
                        educationList = db.AdmissionDB.SPGetCandidateExamDetailsByPaymentIdMobileNo(searchText).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessageEdu.Text = "Error getting candidate education info. " +ex.Message;
                    messagePanelEdu.CssClass = "alert alert-danger";
                    messagePanelEdu.Visible = true;
                }

                if(educationList != null)
                {
                    if(educationList.Count > 0)
                    {
                        gvEdu.DataSource = educationList;
                    }
                    else
                    {
                        gvEdu.DataSource = null;
                    }
                }
                else
                {
                    gvEdu.DataSource = null;
                }
                gvEdu.DataBind();

                #endregion

                #region RELATION

                List<DAL.SPGetCandidateRelationDetailsByPaymentIdMobileNo_Result> relationList = null;

                try
                {
                    using(var db = new OfficeDataManager())
                    {
                        relationList = db.AdmissionDB.SPGetCandidateRelationDetailsByPaymentIdMobileNo(searchText).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessageRelation.Text = "Error getting candidate relation info. " + ex.Message;
                    messagePanelRelation.CssClass = "alert alert-danger";
                    messagePanelRelation.Visible = true;
                }

                if(relationList != null)
                {
                    if(relationList.Count > 0)
                    {
                        gvRelation.DataSource = relationList;
                    }
                    else
                    {
                        gvRelation.DataSource = null;
                    }
                }
                else
                {
                    gvRelation.DataSource = null;
                }
                gvRelation.DataBind();

                #endregion

                #region ADDRESS

                #endregion

                #region ADDITIONAL INFO

                #endregion
            }
        }

        protected void gvBasicInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvFormPay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblIsPaid = (Label)e.Row.FindControl("lblIsPaid");

                if(lblIsPaid.Text == "Yes")
                {
                    lblIsPaid.ForeColor = Color.Green;
                }
                else if(lblIsPaid.Text == "NO")
                {
                    lblIsPaid.ForeColor = Color.Crimson;
                    lblIsPaid.Font.Bold = true;
                    lblIsPaid.Font.Size = FontUnit.Larger;
                }
            }
        }
    }
}