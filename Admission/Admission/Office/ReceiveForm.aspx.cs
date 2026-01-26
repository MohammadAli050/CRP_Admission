using Admission.App_Start;
using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class ReceiveForm : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (uRole.ToLower() == "candidate")
            {

                SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                Response.Redirect("~/Admission/Home.aspx", false);
            }


            if (!IsPostBack)
            {
                LoadDDL();
                LoadListView();

                hfIsFilterClick.Value = "0";
            }
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlUnitProgram, db.GetAllAdmissionUnit(), "UnitName", "ID", EnumCollection.ListItemType.SelectAll);
                DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.IsActive == true).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.SelectAll);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(a => a.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
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



        private void LoadListView()
        {
            #region N/A
            //using (var db = new CandidateDataManager())
            //{
            //    List<FormRequestListViewObject> list = (from candPayment in db.AdmissionDB.CandidatePayments
            //                                            join candFormSl in db.AdmissionDB.CandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
            //                                            join candidate in db.AdmissionDB.BasicInfoes on candFormSl.CandidateID equals candidate.ID
            //                                            join admSetup in db.AdmissionDB.AdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
            //                                            join admUnit in db.AdmissionDB.AdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
            //                                            where candPayment.IsPaid == true
            //                                            select new FormRequestListViewObject
            //                                            {
            //                                                CandidateID = candidate.ID,
            //                                                Name = candidate.FirstName,
            //                                                CandidateFormSerialID = candFormSl.ID,
            //                                                FormSerial = candFormSl.FormSerial,
            //                                                CandidatePaymentID = candPayment.ID,
            //                                                PaymentId = candPayment.PaymentId,
            //                                                IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
            //                                                Mobile = candidate.SMSPhone,
            //                                                Email = candidate.Email,
            //                                                AdmissionSetupID = admSetup.ID,
            //                                                AdmissionUnitID = admUnit.ID,
            //                                                UNIT = admUnit.UnitName,
            //                                                DateApplied = candidate.DateCreated
            //                                            }).OrderByDescending(x=> x.CandidatePaymentID).Take(200).ToList(); //Take(50)
            //    if (list.Count() > 0)
            //    {
            //        lvFormRequest.DataSource = list.OrderByDescending(c => c.CandidatePaymentID).ToList();
            //        lblCount.Text = list.Count().ToString();
            //    }
            //    else
            //    {
            //        lvFormRequest.DataSource = null;
            //        lblCount.Text = "0";
            //    }
            //}
            //lvFormRequest.DataBind(); 
            #endregion

            MessageView("", "clear");

            try
            {
                int? acaCalId = null;
                long? admUnitId = null;
                int? eduCatId = null;

                string candidateName = null;
                string candidateMobile = null;
                string candidateEmail = null;
                long? candidatePaymentId = null;

                #region Get Session

                if (!string.IsNullOrEmpty(hfIsFilterClick.Value) && Convert.ToInt32(hfIsFilterClick.Value) == 1)
                {
                    acaCalId = null;
                }
                else
                {
                    if (Convert.ToInt32(ddlSession.SelectedValue) > 0)
                    {
                        acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                    }
                    else
                    {
                        List<DAL.AdmissionSetup> admSetList = null;
                        using (var db = new OfficeDataManager())
                        {
                            admSetList = db.AdmissionDB.AdmissionSetups.Where(x => x.IsActive == true).ToList();
                        }

                        if (admSetList != null && admSetList.Count > 0)
                        {
                            var idList = admSetList.Select(x => x.AcaCalID).Distinct();

                            if (idList != null && idList.Count() > 0)
                            {
                                acaCalId = idList.Max();
                            }
                        }
                    }
                }
                #endregion

                #region Get Faculty
                if (Convert.ToInt64(ddlUnitProgram.SelectedValue) > 0)
                {
                    admUnitId = Convert.ToInt64(ddlUnitProgram.SelectedValue);
                }
                else
                {
                    admUnitId = null;
                }
                #endregion

                #region Get Education Category
                if (Convert.ToInt32(ddlEducationCategory.SelectedValue) > 0)
                {
                    eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                }
                else
                {
                    eduCatId = null;
                }
                #endregion

                #region Candidate Name
                if (!string.IsNullOrEmpty(txtFilterName.Text.Trim()))
                {
                    candidateName = txtFilterName.Text.Trim();
                }
                #endregion

                #region Candidate Mobile
                if (!string.IsNullOrEmpty(txtFilterMobile.Text.Trim()))
                {
                    candidateMobile = txtFilterMobile.Text.Trim();
                }
                #endregion

                #region Candidate Email
                if (!string.IsNullOrEmpty(txtFilterEmail.Text.Trim()))
                {
                    candidateEmail = txtFilterEmail.Text.Trim();
                }
                #endregion

                #region Candidate PaymentID
                try
                {
                    if (!string.IsNullOrEmpty(txtFilterPaymentID.Text.Trim()) && Convert.ToInt64(txtFilterPaymentID.Text.Trim()) > 0)
                    {
                        candidatePaymentId = Convert.ToInt64(txtFilterPaymentID.Text.Trim());
                    }
                }
                catch (Exception ex)
                {
                    candidatePaymentId = null;
                }
                #endregion


                #region Cadiddate UserID
                string userLoginId = "";
                try
                {
                    userLoginId = txtFilterUserID.Text.Trim();
                }
                catch (Exception ex)
                {
                }
                #endregion

                //if (acaCalId > 0)
                //{
                List<DAL.SPGetCandidatePaidUnPaidFormInfo_Result> list = null;
                using (var db = new GeneralDataManager())
                {
                    list = db.AdmissionDB.SPGetCandidatePaidUnPaidFormInfo(true,
                                                                            acaCalId,
                                                                            admUnitId,
                                                                            eduCatId,
                                                                            candidatePaymentId,
                                                                            candidateMobile,
                                                                            candidateEmail,
                                                                            candidateName,userLoginId).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    lvFormRequest.DataSource = list.OrderByDescending(c => c.PaymentId).ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvFormRequest.DataSource = null;
                    lblCount.Text = "0";
                }
                lvFormRequest.DataBind();

                //}
                //else
                //{
                //    lvFormRequest.DataSource = null;
                //    lblCount.Text = "0";
                //    MessageView("No Data Found !", "fail");
                //}

            }
            catch (Exception ex)
            {
                lvFormRequest.DataSource = null;
                lblCount.Text = "0";
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }






            #region N/A
            //List<FormRequesAndReceiveFormtListViewObject> list = GetAllCandidateInfo().Take(500).ToList();

            //if (list.Count() > 0)
            //{
            //    lvFormRequest.DataSource = list.OrderByDescending(c => c.CandidateID).ToList();
            //    lblCount.Text = list.Count().ToString();
            //}
            //else
            //{
            //    lvFormRequest.DataSource = null;
            //    lblCount.Text = "0";
            //}

            //lvFormRequest.DataBind(); 
            #endregion

        }

        #region N/A
        //private void LoadListView(string searchText, int? acaCalId)
        //{
        //    #region N/A
        //    //if (searchText != null)//search using search text
        //    //{
        //    //    using (var db = new CandidateDataManager())
        //    //    {
        //    //        List<DAL.SPFormRequestGetBySearchText_Result> list = db.AdmissionDB.SPFormRequestGetBySearchText(searchText, true).ToList();
        //    //        if (list.Any())
        //    //        {
        //    //            List<FormRequestListViewObject> lvObjList = new List<FormRequestListViewObject>();
        //    //            foreach (var item in list)
        //    //            {
        //    //                FormRequestListViewObject obj = new FormRequestListViewObject();
        //    //                obj.CandidateID = item.CandidateID;
        //    //                obj.Name = item.Name;
        //    //                obj.CandidateFormSerialID = item.CandidateFormSerialID;
        //    //                obj.FormSerial = item.FormSerial;
        //    //                obj.CandidatePaymentID = item.CandidatePaymentID;
        //    //                obj.PaymentId = item.PaymentId;
        //    //                obj.Mobile = item.Mobile;
        //    //                obj.Email = item.Email;
        //    //                obj.AdmissionSetupID = item.AdmissionSetupID;
        //    //                obj.AdmissionUnitID = item.AdmissionUnitID;
        //    //                obj.UNIT = item.UNIT;
        //    //                obj.DateApplied = item.DateApplied;
        //    //                if (item.IsPaid == true)
        //    //                {
        //    //                    obj.IsPaid = "Yes";
        //    //                }
        //    //                else
        //    //                {
        //    //                    obj.IsPaid = "No";
        //    //                }
        //    //                lvObjList.Add(obj);
        //    //            }
        //    //            if (lvObjList.Any())
        //    //            {
        //    //                lvFormRequest.DataSource = lvObjList.OrderByDescending(c => c.CandidatePaymentID).ToList();
        //    //                lblCount.Text = lvObjList.Count.ToString();
        //    //            }
        //    //            else
        //    //            {
        //    //                lvFormRequest.DataSource = null;
        //    //                lblCount.Text = "0";
        //    //            }
        //    //            lvFormRequest.DataBind();
        //    //        }
        //    //        else
        //    //        {
        //    //            lvFormRequest.DataSource = null;
        //    //            lblCount.Text = "0";
        //    //            lvFormRequest.DataBind();
        //    //        }
        //    //    }
        //    //}
        //    //else if (acaCalId != null || acaCalId > 0)// load data using dropdowns (to filter)
        //    //{
        //    //    long admissionUnitId = Convert.ToInt64(ddlUnitProgram.SelectedValue);
        //    //    int educationCategoryId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
        //    //    int sessionId = Convert.ToInt32(ddlSession.SelectedValue);
        //    //    using (var db = new CandidateDataManager())
        //    //    {
        //    //        List<DAL.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId_Result> list =
        //    //            db.AdmissionDB.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId(admissionUnitId, sessionId, educationCategoryId, true).ToList();
        //    //        if (list.Any())
        //    //        {
        //    //            List<FormRequestListViewObject> lvObjList = new List<FormRequestListViewObject>();
        //    //            foreach (var item in list)
        //    //            {
        //    //                FormRequestListViewObject obj = new FormRequestListViewObject();
        //    //                obj.CandidateID = item.CandidateID;
        //    //                obj.Name = item.Name;
        //    //                obj.CandidateFormSerialID = item.CandidateFormSerialID;
        //    //                obj.FormSerial = item.FormSerial;
        //    //                obj.CandidatePaymentID = item.CandidatePaymentID;
        //    //                obj.PaymentId = item.PaymentId;
        //    //                obj.Mobile = item.Mobile;
        //    //                obj.Email = item.Email;
        //    //                obj.AdmissionSetupID = item.AdmissionSetupID;
        //    //                obj.AdmissionUnitID = item.AdmissionUnitID;
        //    //                obj.UNIT = item.UNIT;
        //    //                obj.DateApplied = item.DateApplied;
        //    //                if (item.IsPaid == true)
        //    //                {
        //    //                    obj.IsPaid = "Yes";
        //    //                }
        //    //                else
        //    //                {
        //    //                    obj.IsPaid = "No";
        //    //                }
        //    //                lvObjList.Add(obj);
        //    //            }
        //    //            if (lvObjList.Any())
        //    //            {
        //    //                lvFormRequest.DataSource = lvObjList.OrderByDescending(c => c.CandidatePaymentID).ToList();
        //    //                lblCount.Text = lvObjList.Count.ToString();
        //    //            }
        //    //            else
        //    //            {
        //    //                lvFormRequest.DataSource = null;
        //    //                lblCount.Text = "0";
        //    //            }
        //    //            lvFormRequest.DataBind();
        //    //        }
        //    //        else
        //    //        {
        //    //            lvFormRequest.DataSource = null;
        //    //            lblCount.Text = "0";
        //    //            lvFormRequest.DataBind();
        //    //        }
        //    //    }
        //    //} 
        //    #endregion


        //    if (searchText != null)//search using search text
        //    {


        //        List<FormRequesAndReceiveFormtListViewObject> list = GetAllCandidateInfo();

        //        if (list != null && list.Count > 0)
        //        {
        //            if(new EmailAddressAttribute().IsValid(searchText.Trim()))
        //            {
        //                list = list.Where(x => x.Email == searchText.Trim()).ToList();
        //            }
        //            else if (Regex.Match(searchText.Trim(), @"^(?:\+88|01)?\d{11}$").Success)
        //            {
        //                list = list.Where(x => x.Mobile == searchText.Trim()).ToList();
        //            }
        //            else
        //            {
        //                long paymentId = Convert.ToInt64(searchText.Trim());
        //                list = list.Where(x => x.PaymentId == paymentId).ToList();                        
        //            }


        //            if (list.Any())
        //            {
        //                lvFormRequest.DataSource = list.OrderByDescending(c => c.PaymentId).ToList();
        //                lblCount.Text = list.Count.ToString();
        //            }
        //            else
        //            {
        //                lvFormRequest.DataSource = null;
        //                lblCount.Text = "0";
        //            }
        //            lvFormRequest.DataBind();

        //        }
        //        else
        //        {
        //            lvFormRequest.DataSource = null;
        //            lblCount.Text = "0";
        //            lvFormRequest.DataBind();
        //        }


        //    }
        //    else if (acaCalId != null || acaCalId > 0)// load data using dropdowns (to filter)
        //    {
        //        long admissionUnitId = Convert.ToInt64(ddlUnitProgram.SelectedValue);
        //        int educationCategoryId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
        //        int sessionId = Convert.ToInt32(ddlSession.SelectedValue);


        //        List<DAL.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId_Result> candidateAllInfolist = null;
        //        using (var db = new CandidateDataManager())
        //        {
        //            candidateAllInfolist = db.AdmissionDB.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId(admissionUnitId, sessionId, educationCategoryId, true).ToList();
        //        }

        //        List<FormRequesAndReceiveFormtListViewObject> list = GetAllCandidateInfo();

        //        list = list.Where(x => candidateAllInfolist.Any(y => y.PaymentId == x.PaymentId)).ToList();

        //        if (list.Count > 0)
        //        {
        //            list = list.Where(x => x.AcaCalId == acaCalId).ToList();
        //            if (list.Any())
        //            {
        //                lvFormRequest.DataSource = list.OrderByDescending(c => c.PaymentId).ToList();
        //                lblCount.Text = list.Count.ToString();
        //            }
        //            else
        //            {
        //                lvFormRequest.DataSource = null;
        //                lblCount.Text = "0";
        //            }
        //            lvFormRequest.DataBind();
        //        }
        //        else
        //        {
        //            lvFormRequest.DataSource = null;
        //            lblCount.Text = "0";
        //            lvFormRequest.DataBind();
        //        }

        //    }


        //} 
        #endregion

        protected void btnLoad_Click(object sender, EventArgs e)
        {

            LoadListView();

            #region N/A
            //if (Convert.ToInt32(ddlSession.SelectedValue) > 0)
            //{
            //    LoadListView(null, Convert.ToInt32(ddlSession.SelectedValue));
            //} 
            #endregion
        }

        #region N/A
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    //if (!string.IsNullOrEmpty(txtSearchText.Text))
        //    //{
        //    //    LoadListView(txtSearchText.Text, null);
        //    //}
        //} 
        #endregion

        protected void lvFormRequest_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            #region N/A
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    ListViewDataItem currentItem = (ListViewDataItem)e.Item;
            //    FormRequestListViewObject obj = (FormRequestListViewObject)((ListViewDataItem)(e.Item)).DataItem;

            //    //Label lblSerial = (Label)currentItem.FindControl("lblSerial");
            //    Label lblName = (Label)currentItem.FindControl("lblName");
            //    Label lblFormSerial = (Label)currentItem.FindControl("lblFormSerial");
            //    Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
            //    Label lblMobile = (Label)currentItem.FindControl("lblMobile");
            //    Label lblEmail = (Label)currentItem.FindControl("lblEmail");
            //    Label lblUnit = (Label)currentItem.FindControl("lblUnit");
            //    Label lblDateApplied = (Label)currentItem.FindControl("lblDateApplied");
            //    Label lblPaid = (Label)currentItem.FindControl("lblPaid");

            //    LinkButton lbForm = (LinkButton)currentItem.FindControl("lbForm");

            //    //lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
            //    lblName.Text = obj.Name;
            //    lblFormSerial.Text = obj.FormSerial.ToString();
            //    lblPaymentId.Text = obj.PaymentId.ToString();
            //    lblMobile.Text = obj.Mobile;
            //    lblEmail.Text = obj.Email;
            //    lblUnit.Text = obj.UNIT;
            //    lblDateApplied.Text = obj.DateApplied.HasValue ? obj.DateApplied.Value.ToString("dd/MM/yyyy") : "N/A";
            //    if (obj.IsPaid == "Yes")
            //    {
            //        lblPaid.Text = "✓";
            //        lblPaid.ForeColor = Color.Green;
            //    }
            //    else
            //    {
            //        lblPaid.Text = "✕";
            //        lblPaid.Font.Bold = true;
            //        lblPaid.ForeColor = Color.Crimson;
            //    }

            //    if (!string.IsNullOrEmpty(txtSearchText.Text))
            //    {
            //        if (lblPaymentId.Text.Equals(txtSearchText.Text))
            //        {
            //            lblPaymentId.BackColor = Color.LightGray;
            //        }
            //        else if (lblMobile.Text.Equals(txtSearchText.Text))
            //        {
            //            lblMobile.BackColor = Color.LightGray;
            //        }
            //        else if (lblEmail.Text.Equals(txtSearchText.Text))
            //        {
            //            lblEmail.BackColor = Color.LightGray;
            //        }
            //    }

            //    lbForm.CommandName = "Form";
            //    lbForm.CommandArgument = obj.CandidateID.ToString();
            //} 
            #endregion

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetCandidatePaidUnPaidFormInfo_Result obj = (DAL.SPGetCandidatePaidUnPaidFormInfo_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblMobile = (Label)currentItem.FindControl("lblMobile");
                Label lblEmail = (Label)currentItem.FindControl("lblEmail");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
                Label lblUnit = (Label)currentItem.FindControl("lblUnit");
                Label lblEducationCategory = (Label)currentItem.FindControl("lblEducationCategory");
                Label lblSession = (Label)currentItem.FindControl("lblSession");
                Label lblDateApplied = (Label)currentItem.FindControl("lblDateApplied");
                Label lblPaid = (Label)currentItem.FindControl("lblPaid");
                Label lblfinalSubmited = (Label)currentItem.FindControl("lblfinalSubmited");
                //LinkButton lblLoginCredential = (LinkButton)currentItem.FindControl("lblLoginCredential");
                //LinkButton lbForm = (LinkButton)currentItem.FindControl("lbForm");
                HyperLink hlLoginCredential = (HyperLink)currentItem.FindControl("hlLoginCredential");
                HyperLink hlForm = (HyperLink)currentItem.FindControl("hlForm");
                HyperLink hlAdmitCard = (HyperLink)currentItem.FindControl("hlAdmitCard");


                lblName.Text = obj.FirstName;
                lblMobile.Text = obj.SMSPhone;
                lblEmail.Text = obj.Email;
                lblPaymentId.Text = obj.PaymentId.ToString();

                #region Get Faculty Names
                string facultyName = "";
                try
                {
                    List<DAL.CandidateFormSl> formList = null;
                    using (var db = new CandidateDataManager())
                    {
                        formList = db.GetAllCandidateFormSlByCandID_AD(Convert.ToInt64(obj.CandidateID)).ToList();
                    }


                    if (formList != null && formList.Count > 0)
                    {
                        foreach (var tData in formList)
                        {
                            facultyName += tData.AdmissionSetup.AdmissionUnit.UnitName + "<br/>";
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion

                lblUnit.Text = facultyName;
                lblEducationCategory.Text = obj.EducationCategory;
                lblSession.Text = obj.FullCode;
                lblDateApplied.Text = obj.AppliedDate.HasValue ? obj.AppliedDate.Value.ToString("dd-MM-yyyy hh:mm tt") : "N/A";
                if (obj.IsPaidBit == true)
                {
                    lblPaid.Text = obj.IsPaid;
                    lblPaid.ForeColor = Color.Green;
                }
                else
                {
                    lblPaid.Text = obj.IsPaid;
                    lblPaid.ForeColor = Color.Crimson;
                }

                if (obj.IsFinalSubmitBit == true)
                {
                    lblfinalSubmited.Text = "✔";
                    lblfinalSubmited.ForeColor = Color.Green;
                }
                else
                {
                    lblfinalSubmited.Text = "✘";
                    lblfinalSubmited.ForeColor = Color.Crimson;
                }

                #region N/A
                ////Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                //
                ////Label lblFormSerial = (Label)currentItem.FindControl("lblFormSerial");
                //
                //
                //






                //string facultyList = "";
                //long paymentId = Convert.ToInt64(obj.PaymentId);
                //using (var db = new CandidateDataManager())
                //{

                //    List<FormRequestListViewObject> list = (from candPayment in db.AdmissionDB.CandidatePayments
                //                                            join candFormSl in db.AdmissionDB.CandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
                //                                            join candidate in db.AdmissionDB.BasicInfoes on candFormSl.CandidateID equals candidate.ID
                //                                            join admSetup in db.AdmissionDB.AdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
                //                                            join admUnit in db.AdmissionDB.AdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
                //                                            where candPayment.IsPaid == true && candPayment.PaymentId == paymentId
                //                                            orderby candPayment.PaymentId descending
                //                                            select new FormRequestListViewObject
                //                                            {
                //                                                CandidateID = candidate.ID,
                //                                                Name = candidate.FirstName + " " + candidate.LastName,
                //                                                CandidateFormSerialID = candFormSl.ID,
                //                                                FormSerial = candFormSl.FormSerial,
                //                                                CandidatePaymentID = candPayment.ID,
                //                                                PaymentId = candPayment.PaymentId,
                //                                                IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
                //                                                Mobile = candidate.SMSPhone,
                //                                                AdmissionSetupID = admSetup.ID,
                //                                                AdmissionUnitID = admUnit.ID,
                //                                                UNIT = admUnit.UnitName,
                //                                                DateApplied = candidate.DateCreated
                //                                            }).ToList();  //.Take(500)


                //    if (list.Count > 0)
                //    {
                //        foreach (var tData in list)
                //        {
                //            facultyList = facultyList + tData.UNIT + "<br/>";
                //        }
                //    }
                //}


                //DAL.CandidatePayment cp = null;
                //DAL.AdditionalInfo ainfo = null;
                //using (var db = new CandidateDataManager())
                //{
                //    cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == paymentId).FirstOrDefault();
                //    if (cp != null)
                //    {
                //        ainfo = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cp.CandidateID).FirstOrDefault();
                //    }
                //}




                ////lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                //lblName.Text = obj.Name;
                ////lblFormSerial.Text = obj.FormSerial.ToString();
                //lblPaymentId.Text = obj.PaymentId.ToString();
                //lblMobile.Text = obj.Mobile;
                //lblUnit.Text = facultyList.ToString(); 
                #endregion

                //lblLoginCredential.CommandName = "LoginCredential";
                //lblLoginCredential.CommandArgument = obj.CandidateID.ToString();

                //lbForm.CommandName = "Form";
                //lbForm.CommandArgument = obj.CandidateID.ToString();

                string CandidateEncryptedId = Encrypt.EncryptString(obj.CandidateID.ToString());

                hlLoginCredential.NavigateUrl = "~/Admission/Office/CandidateLoginCredentials.aspx?val=" + obj.CandidateID.ToString();
                hlForm.NavigateUrl = "~/Admission/Office/CandidateInfo/CandApplicationBasic.aspx?val=" + CandidateEncryptedId;
                hlAdmitCard.NavigateUrl = "~/Admission/Candidate/Prints/AdmitCardV2.aspx?val=" + obj.CandidateID.ToString();


            }

        }

        protected void lvFormRequest_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            #region N/A
            //string searchText = txtSearchText.Text;
            //int sessionId = Convert.ToInt32(ddlSession.SelectedValue);
            //if ((string.IsNullOrEmpty(searchText.Trim())) && sessionId > 0)
            //{
            //    lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            //    LoadListView(null, sessionId);
            //}
            //else if ((!string.IsNullOrEmpty(searchText.Trim())) && sessionId < 1)
            //{
            //    lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            //    LoadListView(searchText, null);
            //}
            //else
            //{
            //lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            //LoadListView();
            //} 
            #endregion

            lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            LoadListView();
        }

        #region N/A -- lvFormRequest_ItemCommand
        //protected void lvFormRequest_ItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    #region N/A
        //    //if (e.CommandName == "Form")
        //    //{
        //    //    Response.Redirect("~/Admission/Office/CandidateInfo/CandApplicationBasic.aspx?val=" + e.CommandArgument.ToString(), false);
        //    //} 
        //    #endregion

        //    //if (e.CommandName == "Form")
        //    //{
        //    //    long candidateId = Int64.Parse(e.CommandArgument.ToString());
        //    //    string cipherCandidateId = candidateId.ToString(); //Encrypt.EncryptString(candidateId.ToString());
        //    //    //Response.Redirect("~/Admission/Office/CandidateInfo/CandApplicationBasic.aspx?val=" + cipherCandidateId, false);

        //    //    string url = "CandidateInfo/CandApplicationBasic.aspx?val=" + cipherCandidateId;
        //    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + url + "','_blank');", true); //_newtab
        //    //}
        //    //else if (e.CommandName == "LoginCredential")
        //    //{
        //    //    long candidateId = Int64.Parse(e.CommandArgument.ToString());
        //    //    string cipherCandidateId = candidateId.ToString();
        //    //    //Response.Redirect("~/Admission/Office/CandidateLoginCredentials.aspx?val=" + cipherCandidateId, false);

        //    //    string url = "CandidateLoginCredentials.aspx?val=" + cipherCandidateId;
        //    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + url + "','_blank');", true); //_newtab

        //    //}
        //    //else
        //    //{

        //    //}

        //} 
        #endregion

        #region N/A
        //protected List<FormRequesAndReceiveFormtListViewObject> GetAllCandidateInfo()
        //{
        //    List<FormRequesAndReceiveFormtListViewObject> list = null;
        //    using (var db = new CandidateDataManager())
        //    {

        //        list = (from candPayment in db.AdmissionDB.CandidatePayments
        //                    //join candFormSl in db.AdmissionDB.CandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
        //                join candidate in db.AdmissionDB.BasicInfoes on candPayment.CandidateID equals candidate.ID
        //                //join admSetup in db.AdmissionDB.AdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
        //                //join admUnit in db.AdmissionDB.AdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
        //                where candPayment.IsPaid == true
        //                orderby candPayment.PaymentId descending
        //                select new FormRequesAndReceiveFormtListViewObject
        //                {
        //                    CandidateID = candidate.ID,
        //                    Name = candidate.FirstName + " " + candidate.LastName,
        //                    //CandidateFormSerialID = candFormSl.ID,
        //                    //FormSerial = candFormSl.FormSerial,
        //                    CandidatePaymentID = candPayment.ID,
        //                    PaymentId = candPayment.PaymentId,
        //                    IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
        //                    Mobile = candidate.SMSPhone,
        //                    //AdmissionSetupID = admSetup.ID,
        //                    //AdmissionUnitID = admUnit.ID,
        //                    //UNIT = admUnit.UnitName,
        //                    DateApplied = candidate.DateCreated,
        //                    AcaCalId = candPayment.AcaCalID,
        //                    Email = candidate.Email
        //                }).ToList();  //.Take(50)
        //    }

        //    return list;
        //} 
        #endregion

        protected void btnFilterData_Click(object sender, EventArgs e)
        {
            int count = 0;
            #region Check Filter Input Is Given
            if (!string.IsNullOrEmpty(txtFilterName.Text.Trim()))
            {
                count++;
            }
            if (!string.IsNullOrEmpty(txtFilterMobile.Text.Trim()))
            {
                count++;
            }
            if (!string.IsNullOrEmpty(txtFilterEmail.Text.Trim()))
            {
                count++;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtFilterPaymentID.Text.Trim()) && Convert.ToInt64(txtFilterPaymentID.Text.Trim()) > 0)
                {
                    count++;
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (!string.IsNullOrEmpty(txtFilterUserID.Text.Trim()))
                {
                    count++;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            if (count > 0)
            {
                hfIsFilterClick.Value = "1";
                LoadListView();
            }

        }


        protected void ClearFilter()
        {
            txtFilterName.Text = string.Empty;
            txtFilterMobile.Text = string.Empty;
            txtFilterEmail.Text = string.Empty;
            txtFilterPaymentID.Text = string.Empty;
        }

        protected void btnFilterClear_Click(object sender, EventArgs e)
        {
            int count = 0;
            #region Check Filter Input Is Given
            if (!string.IsNullOrEmpty(txtFilterName.Text.Trim()))
            {
                count++;
            }
            if (!string.IsNullOrEmpty(txtFilterMobile.Text.Trim()))
            {
                count++;
            }
            if (!string.IsNullOrEmpty(txtFilterEmail.Text.Trim()))
            {
                count++;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtFilterPaymentID.Text.Trim()) && Convert.ToInt64(txtFilterPaymentID.Text.Trim()) > 0)
                {
                    count++;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            if (count > 0)
            {
                hfIsFilterClick.Value = "0";
                ClearFilter();
                LoadListView();
            }
        }
    }
}