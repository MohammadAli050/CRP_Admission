using Admission.App_Start;
using CommonUtility;
using DAL;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class FacultyWiseQuota : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        string userName = "";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

            using (var db = new OfficeDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if (!IsPostBack)
            {
                LoadDDL();

                //hfCandidateID.Value = "0";
                //Session["AdmitCardFileName"] = null;
                //Session["AdmitCardData"] = null;


            }
        }


        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.All);
                DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(c => c.IsActive == true).ToList(), "Remarks", "ID", EnumCollection.ListItemType.All);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);

            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (acaCalId > 0)
                {
                    long? facultyId = null;
                    int? eduCatId = null;
                    if (Convert.ToInt64(ddlAdmUnit.SelectedValue) > 0)
                    {
                        facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

                        DAL.AdmissionSetup admSetModel = null;
                        using (var db = new OfficeDataManager())
                        {
                            admSetModel = db.AdmissionDB.AdmissionSetups.Where(x => x.AdmissionUnitID == facultyId && x.AcaCalID == acaCalId).FirstOrDefault();
                        }
                        if (admSetModel != null)
                        {
                            eduCatId = admSetModel.EducationCategoryID;
                        }
                    }
                    else
                    {
                        facultyId = null;
                        eduCatId = null;
                    }

                    int? quotaId = null;
                    if (Convert.ToInt32(ddlQuota.SelectedValue) > 0)
                    {
                        quotaId = Convert.ToInt32(ddlQuota.SelectedValue);
                    }
                    else
                    {
                        quotaId = null;
                    }

                    List<DAL.SPGetAllQuotaInfo_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPGetAllQuotaInfo(facultyId,
                                                                 acaCalId,
                                                                 eduCatId,
                                                                 true,
                                                                 quotaId).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        int StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                        if (StatusId == 0)
                        {
                            list = list.Where(x => x.IsVerifiedDocument == false).ToList();
                        }
                        else if (StatusId == 1)
                        {
                            list = list.Where(x => x.IsVerifiedDocument == true).ToList();
                        }

                        lvQuotaInfo.DataSource = list.OrderByDescending(x => x.PaymentId).ToList();
                        lblCount.Text = list.Count().ToString();
                    }
                    else
                    {
                        lvQuotaInfo.DataSource = null;
                        lblCount.Text = list.Count().ToString();
                    }
                    lvQuotaInfo.DataBind();

                }
                else
                {
                    MessageView("Please select session!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }


        protected void lvQuotaInfo_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetAllQuotaInfo_Result obj = (DAL.SPGetAllQuotaInfo_Result)((ListViewDataItem)(e.Item)).DataItem;

                //Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
                Label lblCandidateName = (Label)currentItem.FindControl("lblCandidateName");
                Label lblCandidatePhone = (Label)currentItem.FindControl("lblCandidatePhone");
                Label lblCandidateEmail = (Label)currentItem.FindControl("lblCandidateEmail");
                Label lblFaculty = (Label)currentItem.FindControl("lblFaculty");
                Label lblQuota = (Label)currentItem.FindControl("lblQuota");
                Label lblQuotaInfo = (Label)currentItem.FindControl("lblQuotaInfo");


                HyperLink hlDoc1 = (HyperLink)currentItem.FindControl("hlDoc1");
                HyperLink hlDoc2 = (HyperLink)currentItem.FindControl("hlDoc2");
                HyperLink hlDoc3 = (HyperLink)currentItem.FindControl("hlDoc3");
                CheckBox cbSingle = (CheckBox)currentItem.FindControl("cbSingle");
                Label lblVerifiedDocumentByName = (Label)currentItem.FindControl("lblVerifiedDocumentByName");

                HiddenField hfQuotaInfoId = (HiddenField)currentItem.FindControl("hfQuotaInfoId");

                HiddenField hdnQuotaId = (HiddenField)currentItem.FindControl("hdnQuotaId");


                lblPaymentId.Text = obj.PaymentId.ToString();
                lblCandidateName.Text = obj.CandidateName.ToString();
                lblCandidatePhone.Text = obj.SMSPhone.ToString();
                lblCandidateEmail.Text = obj.Email.ToString();
                lblFaculty.Text = obj.FacultyName.ToString();
                lblQuota.Text = obj.QuotaName.ToString();

                hfQuotaInfoId.Value = obj.QuotaInfoId.ToString();
                hdnQuotaId.Value = obj.QuotaID.ToString();

                string quotaInfo = "";

                if (obj.QuotaID == 7 || obj.QuotaID == 6)
                {
                    quotaInfo = "";
                }
                else if (obj.QuotaID == 8)
                {
                    quotaInfo += "<strong>";
                    quotaInfo += "Type of Disability: ";
                    quotaInfo += "</strong>";
                    quotaInfo += obj.TypeOfDisability.ToString();
                }
                else if (obj.QuotaID == 2)
                {
                    quotaInfo += "<strong>";
                    quotaInfo += "Name of Freedom Fighter: ";
                    quotaInfo += "</strong>";
                    quotaInfo += obj.FreedomFighterName.ToString();

                    quotaInfo += "<br>";

                    quotaInfo += "<strong>";
                    quotaInfo += "Relation With Applicant: ";
                    quotaInfo += "</strong>";
                    quotaInfo += obj.FreedomFighterRelationWithApplicant.ToString();

                    quotaInfo += "<br>";

                    quotaInfo += "<strong>";
                    quotaInfo += "Freedom Fighter No: ";
                    quotaInfo += "</strong>";
                    quotaInfo += obj.FreedomFighterNo.ToString();

                    if (obj.FreedomFighterGazetteReferenceNo.ToString() != "" && obj.FreedomFighterGazetteReferenceNo.ToString() != "---")
                    {
                        quotaInfo += "<br>";

                        quotaInfo += "<strong>";
                        quotaInfo += "Gazette Reference No: ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.FreedomFighterGazetteReferenceNo.ToString();
                    }

                }
                else if (obj.QuotaID == 4)
                {
                    if (obj.SpecialQuotaQuotaTypeId == 1 || obj.SpecialQuotaQuotaTypeId == 2)
                    {
                        quotaInfo += "<strong>";
                        quotaInfo += "Type of Special Quota: ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.FreedomFighterRelationWithApplicant.ToString();

                        quotaInfo += "<br>";

                        quotaInfo += "<strong>";
                        quotaInfo += "Serving / Retired: ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.SpecialQuotaServingRetired.ToString();

                        quotaInfo += "<br>";

                        quotaInfo += "<strong>";
                        quotaInfo += obj.SpecialQuotaInputOneLabel + ": ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.SpecialQuotaInputOne.ToString();

                        quotaInfo += "<br>";

                        quotaInfo += "<strong>";
                        quotaInfo += obj.SpecialQuotaInputTwoLabel + ": ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.SpecialQuotaInputTwo.ToString();

                        quotaInfo += "<br>";

                        quotaInfo += "<strong>";
                        quotaInfo += "Father's/Mother's Name: ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.SpecialQuotaFatherOrMotherName.ToString();

                        quotaInfo += "<br>";

                        quotaInfo += "<strong>";
                        quotaInfo += "Father's/Mother's (Rank/Designation): ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.SpecialQuotaFatherRankDesignation.ToString();

                    }
                    else if (obj.SpecialQuotaQuotaTypeId == 3)
                    {
                        quotaInfo += "<strong>";
                        quotaInfo += "Committee Member: ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.SpecialQuotaGoverningBodiesTypeName.ToString();

                        quotaInfo += "<br>";

                        quotaInfo += "<strong>";
                        quotaInfo += "Committee Member Name: ";
                        quotaInfo += "</strong>";
                        quotaInfo += obj.SpecialQuotaCommitteeMemberName.ToString();
                    }
                    else if (obj.SpecialQuotaQuotaTypeId == 7)
                    {
                        quotaInfo = "";
                    }






                }

                #region Father's Name added


                DAL.Relation fRelation = null;
                DAL.RelationDetail fRelationDtls = null;
                using (var db = new CandidateDataManager())
                {
                    fRelation = db.AdmissionDB.Relations.Where(c => c.CandidateID == obj.CandidateID && c.RelationTypeID == 2).FirstOrDefault();
                }

                if (fRelation != null)
                {
                    using (var db = new CandidateDataManager())
                    {
                        fRelationDtls = db.AdmissionDB.RelationDetails.Where(c => c.ID == fRelation.RelationDetailsID).FirstOrDefault();

                        if(fRelationDtls!=null)
                        {
                            quotaInfo += "<br>";

                            quotaInfo += "<strong>";
                            quotaInfo += "Father's Name: ";
                            quotaInfo += "</strong>";
                            quotaInfo += fRelationDtls.Name.ToString();
                        }

                    }
                }
                #endregion


                lblQuotaInfo.Text = quotaInfo;


                cbSingle.Checked = Convert.ToBoolean(obj.IsVerifiedDocument);
                if (Convert.ToBoolean(obj.IsVerifiedDocument) == true)
                {
                    lblVerifiedDocumentByName.Visible = true;
                    lblVerifiedDocumentByName.Text = obj.VerifiedDocumentByName;
                }
                else
                {
                    lblVerifiedDocumentByName.Visible = false;
                    lblVerifiedDocumentByName.Text = string.Empty;
                }


                #region Quota Doc URL
                List<DAL.QuotaDocument> qdList = null;
                using (var db = new CandidateDataManager())
                {
                    qdList = db.AdmissionDB.QuotaDocuments.Where(x => x.CandidateId == obj.CandidateID && x.QuotaId == obj.QuotaID).ToList();
                }

                if (qdList != null && qdList.Count > 0)
                {
                    int row = 1;

                    foreach (var tData in qdList)
                    {
                        if (!string.IsNullOrEmpty(tData.URL))
                        {
                            if (row == 1)
                            {
                                hlDoc1.Text = tData.Name;
                                hlDoc1.NavigateUrl = tData.URL;
                                hlDoc1.Visible = true;
                            }
                            else if (row == 2)
                            {
                                hlDoc2.Text = tData.Name;
                                hlDoc2.NavigateUrl = tData.URL;
                                hlDoc2.Visible = true;
                            }
                            else if (row == 3)
                            {
                                hlDoc3.Text = tData.Name;
                                hlDoc3.NavigateUrl = tData.URL;
                                hlDoc3.Visible = true;
                            }
                            else
                            {
                                hlDoc1.Visible = false;
                                hlDoc2.Visible = false;
                                hlDoc3.Visible = false;
                            }
                        }

                        row++;
                    }
                }
                else
                {
                    hlDoc1.Visible = false;
                    hlDoc2.Visible = false;
                    hlDoc3.Visible = false;
                }
                #endregion

            }
        }


        protected void lvQuotaInfo_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            btnLoad_Click(null, null);
        }

        protected void cbSingle_CheckedChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                ListViewDataItem row = (ListViewDataItem)(((CheckBox)sender)).NamingContainer;

                CheckBox cbSingle = (CheckBox)row.FindControl("cbSingle");
                HiddenField hfQuotaInfoId = (HiddenField)row.FindControl("hfQuotaInfoId");
                HiddenField hdnQuotaId = (HiddenField)row.FindControl("hdnQuotaId");
                Label lblPaymentId = (Label)row.FindControl("lblPaymentId");


                if (!string.IsNullOrEmpty(hfQuotaInfoId.Value))
                {
                    long quotaInfoId = Convert.ToInt64(hfQuotaInfoId.Value);

                    if (quotaInfoId > 0)
                    {
                        DAL.QuotaInfo qiModel = null;
                        using (var db = new CandidateDataManager())
                        {
                            qiModel = db.AdmissionDB.QuotaInfoes.Where(x => x.ID == quotaInfoId).FirstOrDefault();
                        }

                        if (qiModel != null)
                        {
                            qiModel.IsVerifiedDocument = cbSingle.Checked;
                            qiModel.VerifiedDocumentBy = uId;
                            qiModel.VerifiedDocumentDate = DateTime.Now;

                            using (var db = new CandidateDataManager())
                            {
                                db.Update<DAL.QuotaInfo>(qiModel);
                            }

                            btnLoad_Click(null, null);
                        }
                    }
                    else
                    {
                        long PaymentId = lblPaymentId == null ? 0 : Convert.ToInt64(lblPaymentId.Text);
                        int quotaId = Convert.ToInt32(hdnQuotaId.Value);

                        InsertNewEntry(PaymentId, cbSingle.Checked, quotaId);
                        btnLoad_Click(null, null);
                    }
                }
                else
                {
                    long PaymentId = lblPaymentId == null ? 0 : Convert.ToInt64(lblPaymentId.Text);
                    int quotaId = Convert.ToInt32(hdnQuotaId.Value);

                    InsertNewEntry(PaymentId, cbSingle.Checked, quotaId);
                    btnLoad_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        private void InsertNewEntry(long PaymentId,bool CheckBoxValue,int QuotaId)
        {
            DAL.QuotaInfo qiModel = new QuotaInfo();
            DAL.CandidatePayment cp = null;
            long CandidateId = 0;
            using (var db = new CandidateDataManager())
            {
                cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == PaymentId).FirstOrDefault();
            }
            if (cp != null)
            {
                CandidateId = Convert.ToInt64(cp.CandidateID);
            }

            if ( CandidateId > 0)
            {
                qiModel.CandidateID = CandidateId;
                qiModel.IsVerifiedDocument = CheckBoxValue;
                qiModel.QuotaTypeId = QuotaId;
                qiModel.VerifiedDocumentBy = uId;
                qiModel.VerifiedDocumentDate = DateTime.Now;
                qiModel.CreatedBy = uId;
                qiModel.CreatedDate = DateTime.Now;

                using (var db = new CandidateDataManager())
                {
                    db.Insert<DAL.QuotaInfo>(qiModel);
                }
            }
        }
    }
}