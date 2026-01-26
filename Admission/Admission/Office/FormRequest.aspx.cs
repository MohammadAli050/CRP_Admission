using Admission.App_Start;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.ViewModels;
using CommonUtility;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Admission.Admission.Office
{
    public partial class FormRequest : PageBase
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

        private void LoadListView()
        {
            #region N/A
            //using (var db = new CandidateDataManager())
            //{
            //    List<FormRequestListViewObject> list = (from candPayment in db.AdmissionDB.CandidatePayments
            //                join candFormSl in db.AdmissionDB.CandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
            //                join candidate in db.AdmissionDB.BasicInfoes on candFormSl.CandidateID equals candidate.ID
            //                join admSetup in db.AdmissionDB.AdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
            //                join admUnit in db.AdmissionDB.AdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
            //                where candPayment.IsPaid == false
            //                select new FormRequestListViewObject
            //                {
            //                    CandidateID = candidate.ID,
            //                    Name = candidate.FirstName,
            //                    CandidateFormSerialID = candFormSl.ID,
            //                    FormSerial = candFormSl.FormSerial,
            //                    CandidatePaymentID = candPayment.ID,
            //                    PaymentId = candPayment.PaymentId,
            //                    IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
            //                    Mobile = candidate.SMSPhone,
            //                    Email = candidate.Email,
            //                    AdmissionSetupID = admSetup.ID,
            //                    AdmissionUnitID = admUnit.ID,
            //                    UNIT = admUnit.UnitName,
            //                    DateApplied = candidate.DateCreated
            //                }).Take(50).ToList();
            //    if (list.Count() > 0)
            //    {
            //        lvFormRequest.DataSource = list.OrderByDescending(c => c.CandidateFormSerialID).ToList();
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

            List<FormRequesAndReceiveFormtListViewObject> list = GetAllCandidateInfo().Take(500).ToList();

            if (list.Count() > 0)
            {
                lvFormRequest.DataSource = list.OrderByDescending(c => c.CandidateID).ToList();
                lblCount.Text = list.Count().ToString();
            }
            else
            {
                lvFormRequest.DataSource = null;
                lblCount.Text = "0";
            }

            lvFormRequest.DataBind();

        }

        private void LoadListView(string searchText, int? acaCalId)
        {
            #region N/A
            //if (searchText != null)//search using search text
            //{
            //    using (var db = new CandidateDataManager())
            //    {
            //        List<DAL.SPFormRequestGetBySearchText_Result> list = db.AdmissionDB.SPFormRequestGetBySearchText(searchText, false).ToList();
            //        if (list.Any())
            //        {
            //            List<FormRequestListViewObject> lvObjList = new List<FormRequestListViewObject>();
            //            foreach (var item in list)
            //            {
            //                FormRequestListViewObject obj = new FormRequestListViewObject();
            //                obj.CandidateID = item.CandidateID;
            //                obj.Name = item.Name;
            //                obj.CandidateFormSerialID = item.CandidateFormSerialID;
            //                obj.FormSerial = item.FormSerial;
            //                obj.CandidatePaymentID = item.CandidatePaymentID;
            //                obj.PaymentId = item.PaymentId;
            //                obj.Mobile = item.Mobile;
            //                obj.Email = item.Email;
            //                obj.AdmissionSetupID = item.AdmissionSetupID;
            //                obj.AdmissionUnitID = item.AdmissionUnitID;
            //                obj.UNIT = item.UNIT;
            //                obj.DateApplied = item.DateApplied;
            //                if(item.IsPaid == true)
            //                {
            //                    obj.IsPaid = "Yes";
            //                }
            //                else
            //                {
            //                    obj.IsPaid = "No";
            //                }
            //                lvObjList.Add(obj);
            //            }
            //            if (lvObjList.Any())
            //            {
            //                lvFormRequest.DataSource = lvObjList.OrderByDescending(c => c.CandidateFormSerialID).ToList();
            //                lblCount.Text = lvObjList.Count.ToString();
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
            //else if (acaCalId != null || acaCalId > 0)// load data using dropdowns (to filter)
            //{
            //    long admissionUnitId = Convert.ToInt64(ddlUnitProgram.SelectedValue);
            //    int educationCategoryId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
            //    int sessionId = Convert.ToInt32(ddlSession.SelectedValue);
            //    using (var db = new CandidateDataManager())
            //    {
            //        List<DAL.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId_Result> list =
            //            db.AdmissionDB.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId(admissionUnitId, sessionId, educationCategoryId, false).ToList();
            //        if (list.Any())
            //        {
            //            List<FormRequestListViewObject> lvObjList = new List<FormRequestListViewObject>();
            //            foreach (var item in list)
            //            {
            //                FormRequestListViewObject obj = new FormRequestListViewObject();
            //                obj.CandidateID = item.CandidateID;
            //                obj.Name = item.Name;
            //                obj.CandidateFormSerialID = item.CandidateFormSerialID;
            //                obj.FormSerial = item.FormSerial;
            //                obj.CandidatePaymentID = item.CandidatePaymentID;
            //                obj.PaymentId = item.PaymentId;
            //                obj.Mobile = item.Mobile;
            //                obj.Email = item.Email;
            //                obj.AdmissionSetupID = item.AdmissionSetupID;
            //                obj.AdmissionUnitID = item.AdmissionUnitID;
            //                obj.UNIT = item.UNIT;
            //                obj.DateApplied = item.DateApplied;
            //                if (item.IsPaid == true)
            //                {
            //                    obj.IsPaid = "Yes";
            //                }
            //                else
            //                {
            //                    obj.IsPaid = "No";
            //                }
            //                lvObjList.Add(obj);
            //            }
            //            if (lvObjList.Any())
            //            {
            //                lvFormRequest.DataSource = lvObjList.OrderByDescending(c => c.CandidateFormSerialID).ToList();
            //                lblCount.Text = lvObjList.Count.ToString();
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


            if (searchText != null)//search using search text
            {


                List<FormRequesAndReceiveFormtListViewObject> list = GetAllCandidateInfo();

                if (list.Count > 0)
                {
                    if (searchText.Contains("@")) // Email
                    {
                        list = list.Where(x => x.Email == searchText.Trim()).ToList();
                    }
                    else if (searchText.Contains("+"))
                    {
                        list = list.Where(x => x.Mobile == searchText.Trim()).ToList();
                    }
                    else
                    {
                        long paymentId = Convert.ToInt64(searchText.Trim());
                        list = list.Where(x => x.PaymentId == paymentId).ToList();
                    }

                    //if (new EmailAddressAttribute().IsValid(searchText.Trim()))
                    //{

                    //}
                    //else if (Regex.Match(searchText.Trim(), @"^(?:\+88|01)?\d{11}$").Success)
                    //{

                    //}
                    //else
                    //{

                    //}

                    if (!string.IsNullOrEmpty(txtToDate.Text) && !string.IsNullOrEmpty(txtFromDate.Text))
                    {
                        DateTime fromdate = DateTime.ParseExact(txtFromDate.Text.ToString(), "dd/MM/yyyy", null);
                        DateTime Todate = DateTime.ParseExact(txtToDate.Text.ToString(), "dd/MM/yyyy", null);

                        list = list.Where(x => x.DateApplied >= fromdate && x.DateApplied <= Todate).ToList();
                    }

                    if (list.Any())
                    {
                        lvFormRequest.DataSource = list.OrderByDescending(c => c.PaymentId).ToList();
                        lblCount.Text = list.Count.ToString();
                    }
                    else
                    {
                        lvFormRequest.DataSource = null;
                        lblCount.Text = "0";
                    }
                    lvFormRequest.DataBind();


                }
                else
                {
                    lvFormRequest.DataSource = null;
                    lblCount.Text = "0";
                    lvFormRequest.DataBind();
                }



            }
            else if (acaCalId != null || acaCalId > 0)// load data using dropdowns (to filter)
            {
                long admissionUnitId = Convert.ToInt64(ddlUnitProgram.SelectedValue);
                int educationCategoryId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                int sessionId = Convert.ToInt32(ddlSession.SelectedValue);

                List<DAL.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId_Result> candidateAllInfolist = null;
                using (var db = new CandidateDataManager())
                {
                    candidateAllInfolist = db.AdmissionDB.SPFormRequestGetByAdmUnitIdAcaCalIdEduCatId(admissionUnitId, sessionId, educationCategoryId, false).ToList();
                }

                List<FormRequesAndReceiveFormtListViewObject> list = GetAllCandidateInfo();

                list = list.Where(x => candidateAllInfolist.Any(y => y.PaymentId == x.PaymentId)).ToList();
                if (!string.IsNullOrEmpty(txtToDate.Text) && !string.IsNullOrEmpty(txtFromDate.Text))
                {
                    DateTime fromdate = DateTime.ParseExact(txtFromDate.Text.ToString(), "dd/MM/yyyy", null);
                    DateTime Todate = DateTime.ParseExact(txtToDate.Text.ToString(), "dd/MM/yyyy", null);

                    list = list.Where(x => x.DateApplied >= fromdate && x.DateApplied <= Todate).ToList();

                }
                if (list.Count > 0)
                {
                    list = list.Where(x => x.AcaCalId == acaCalId).ToList();
                    if (list.Any())
                    {
                        lvFormRequest.DataSource = list.OrderByDescending(c => c.PaymentId).ToList();
                        lblCount.Text = list.Count.ToString();
                    }
                    else
                    {
                        lvFormRequest.DataSource = null;
                        lblCount.Text = "0";
                    }
                    lvFormRequest.DataBind();
                }
                else
                {
                    lvFormRequest.DataSource = null;
                    lblCount.Text = "0";
                    lvFormRequest.DataBind();
                }


            }

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlSession.SelectedValue) > 0)
            {
                LoadListView(null, Convert.ToInt32(ddlSession.SelectedValue));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchText.Text))
            {
                LoadListView(txtSearchText.Text, null);
            }
        }

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

            //} 
            #endregion

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                FormRequesAndReceiveFormtListViewObject obj = (FormRequesAndReceiveFormtListViewObject)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblName = (Label)currentItem.FindControl("lblName");
                //Label lblFormSerial = (Label)currentItem.FindControl("lblFormSerial");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
                Label lblMobile = (Label)currentItem.FindControl("lblMobile");
                Label lblUnit = (Label)currentItem.FindControl("lblUnit");
                Label lblDateApplied = (Label)currentItem.FindControl("lblDateApplied");
                Label lblPaid = (Label)currentItem.FindControl("lblPaid");

                LinkButton lnkPrintPurchaseReciept = (LinkButton)currentItem.FindControl("lnkPrintPurchaseReciept");
                //LinkButton lnkPrintMoneyReciept = (LinkButton)currentItem.FindControl("lnkPrintMoneyReciept");
                HyperLink lnkFormView = (HyperLink)currentItem.FindControl("lnkFormView");

                string facultyList = "";
                long paymentId = Convert.ToInt64(obj.PaymentId);
                using (var db = new CandidateDataManager())
                {

                    List<FormRequestListViewObject> list = (from candPayment in db.AdmissionDB.CandidatePayments
                                                            join candFormSl in db.AdmissionDB.CandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
                                                            join candidate in db.AdmissionDB.BasicInfoes on candFormSl.CandidateID equals candidate.ID
                                                            join admSetup in db.AdmissionDB.AdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
                                                            join admUnit in db.AdmissionDB.AdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
                                                            where candPayment.IsPaid == false && candPayment.PaymentId == paymentId
                                                            select new FormRequestListViewObject
                                                            {
                                                                CandidateID = candidate.ID,
                                                                Name = candidate.FirstName + " " + candidate.LastName,
                                                                CandidateFormSerialID = candFormSl.ID,
                                                                FormSerial = candFormSl.FormSerial,
                                                                CandidatePaymentID = candPayment.ID,
                                                                PaymentId = candPayment.PaymentId,
                                                                IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
                                                                Mobile = candidate.SMSPhone,
                                                                AdmissionSetupID = admSetup.ID,
                                                                AdmissionUnitID = admUnit.ID,
                                                                UNIT = admUnit.UnitName,
                                                                DateApplied = candidate.DateCreated
                                                            }).ToList();  //.Take(500)



                    if (list.Count > 0)
                    {
                        foreach (var tData in list)
                        {
                            facultyList = facultyList + tData.UNIT + "<br/>";
                        }
                    }
                }

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblName.Text = obj.Name;
                //lblFormSerial.Text = obj.FormSerial.ToString();
                lblPaymentId.Text = obj.PaymentId.ToString();
                lblMobile.Text = obj.Mobile;
                lblUnit.Text = facultyList.ToString();
                lblDateApplied.Text = obj.DateApplied.HasValue ? obj.DateApplied.Value.ToString("dd/MM/yyyy") : "N/A";
                if (obj.IsPaid == "Yes")
                {
                    lblPaid.Text = obj.IsPaid;
                    lblPaid.ForeColor = Color.Green;
                }
                else
                {
                    lblPaid.Text = obj.IsPaid;
                    lblPaid.ForeColor = Color.Crimson;
                }

                lnkPrintPurchaseReciept.CommandName = "PurchaseReciept";
                lnkPrintPurchaseReciept.CommandArgument = obj.PaymentId.ToString();

                lnkFormView.NavigateUrl = "~/Admission/Office/CandidateInfo/CandApplicationBasic.aspx?val=" + obj.CandidateID.ToString();
            }


        }

        protected void lvFormRequest_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            string searchText = txtSearchText.Text;
            int sessionId = Convert.ToInt32(ddlSession.SelectedValue);
            if ((string.IsNullOrEmpty(searchText.Trim())) && sessionId > 0)
            {
                lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                LoadListView(null, sessionId);
            }
            else if ((!string.IsNullOrEmpty(searchText.Trim())) && sessionId < 1)
            {
                lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                LoadListView(searchText, null);
            }
            else
            {
                lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                LoadListView();
            }
        }


        protected void lvFormRequest_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "PurchaseReciept")
            {
                long paymentId = -1;
                paymentId = Int64.Parse(e.CommandArgument.ToString());

                //objects for log
                string oldLogCPayment = null;
                string newLogCPayment = null;

                DAL.CandidatePayment cPayment = null;
                DAL.BasicInfo candidate = null;

                if (paymentId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        var maxacaCalid = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == paymentId).Max(a => a.AcaCalID);

                        cPayment = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == paymentId && c.AcaCalID == maxacaCalid)
                            .FirstOrDefault();
                        oldLogCPayment = ObjectToString.ConvertToString(cPayment);
                    }
                }
                try
                {
                    if (cPayment != null)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
                        }


                        if (cPayment.IsPaid == true) //if candidate has paid already...do not update candidatePayment.
                        {
                            lblMessageLv.Text = "Candidate with PaymentID: " + cPayment.PaymentId + " has already paid.";
                            lblMessageLv.ForeColor = Color.Crimson;
                        }
                        else if (cPayment.IsPaid == false) // candidate did not pay..hence update.
                        {
                            //add to transaction history
                            string oldLogTransactionHistory = null;

                            DAL.TransactionHistory th = new DAL.TransactionHistory();
                            th.IsInHouseCashTransaction = true;
                            th.Amount = cPayment.Amount.ToString();
                            th.TransactionDate = DateTime.Now.ToString();
                            th.CandidateID = cPayment.CandidateID.ToString();
                            th.ApiConnect = "N/A";
                            th.BankTransactionID = "N/A";
                            th.CreatedOn = DateTime.Now;
                            th.DateManualInsert = null;
                            th.Status = "VALID";
                            th.StoreAmount = cPayment.Amount.ToString();
                            th.TransactionID = "N/A";
                            th.ValidationID = "N/A";
                            th.ValueA = cPayment.CandidateID.ToString();
                            if (candidate != null)
                            {
                                th.ValueB = candidate.FirstName;
                            }
                            th.ValueC = cPayment.Amount.ToString();
                            th.ValueD = "-1";

                            using (var db = new OfficeDataManager())
                            {
                                db.Insert<DAL.TransactionHistory>(th);
                                oldLogTransactionHistory = ObjectToString.ConvertToString(th);
                            }
                            //log data log for transaction history
                            DAL_Log.DataLog thDLog = new DAL_Log.DataLog();
                            thDLog.DateCreated = DateTime.Now;
                            thDLog.DateTime = DateTime.Now;
                            thDLog.EventName = "lvFormRequest_ItemCommand.CommandName(PurchaseReciept)";
                            thDLog.HostName = Request.UserHostName;
                            thDLog.IpAddress = Request.UserHostAddress;
                            thDLog.NewData = null;
                            thDLog.OldData = oldLogTransactionHistory;
                            thDLog.PageName = "FormRequest.aspx";
                            thDLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + "; "
                                + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                            thDLog.UserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);

                            LogWriter.DataLogWriter(thDLog);
                            //----

                            // update candidate payment object.
                            cPayment.IsPaid = true;
                            cPayment.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
                            cPayment.DateModified = DateTime.Now;

                            using (var db = new CandidateDataManager())
                            {
                                db.Update<DAL.CandidatePayment>(cPayment);
                                newLogCPayment = ObjectToString.ConvertToString(cPayment);
                            }

                            //insert data log for candidate payment
                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateCreated = DateTime.Now;
                            dLog.DateTime = DateTime.Now;
                            dLog.EventName = "lvFormRequest_ItemCommand.CommandName(PurchaseReciept)";
                            dLog.HostName = Request.UserHostName;
                            dLog.IpAddress = Request.UserHostAddress;
                            dLog.NewData = newLogCPayment;
                            dLog.OldData = oldLogCPayment;
                            dLog.PageName = "FormRequest.aspx";
                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) + "; "
                                + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                            dLog.UserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);

                            LogWriter.DataLogWriter(dLog);
                            #region N/A
                            //----

                            //SendEmail(candidate);

                            ////---- SMS Sending
                            //GetSendingInfo(candidate.ID); 
                            #endregion

                            lblMessageLv.Text = "Candidate Payment Updated";
                            lblMessageLv.ForeColor = Color.Green;


                            GetSendingInfo(cPayment.CandidateID);

                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessageLv.Text = "Error occured while updating candidate payment";
                    lblMessageLv.ForeColor = Color.Crimson;
                    lblMessageLv.Font.Bold = true;
                }
            }
        }

        protected List<FormRequesAndReceiveFormtListViewObject> GetAllCandidateInfo()
        {
            List<FormRequesAndReceiveFormtListViewObject> list = null;
            using (var db = new CandidateDataManager())
            {

                list = (from candPayment in db.AdmissionDB.CandidatePayments
                            //join candFormSl in db.AdmissionDB.CandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
                        join candidate in db.AdmissionDB.BasicInfoes on candPayment.CandidateID equals candidate.ID
                        //join admSetup in db.AdmissionDB.AdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
                        //join admUnit in db.AdmissionDB.AdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
                        where candPayment.IsPaid == false
                        orderby candPayment.PaymentId descending
                        select new FormRequesAndReceiveFormtListViewObject
                        {
                            CandidateID = candidate.ID,
                            Name = candidate.FirstName + " " + candidate.LastName,
                            //CandidateFormSerialID = candFormSl.ID,
                            //FormSerial = candFormSl.FormSerial,
                            CandidatePaymentID = candPayment.ID,
                            PaymentId = candPayment.PaymentId,
                            IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
                            Mobile = candidate.SMSPhone,
                            //AdmissionSetupID = admSetup.ID,
                            //AdmissionUnitID = admUnit.ID,
                            //UNIT = admUnit.UnitName,
                            DateApplied = candidate.DateCreated,
                            AcaCalId = candPayment.AcaCalID
                        }).ToList();  //.Take(500)
            }

            return list;
        }


        private void GetSendingInfo(long? candidateId)
        {
            if (candidateId != null)
            {
                if (candidateId > 0)
                {
                    DAL.BasicInfo candidate = null;
                    DAL.CandidateUser candidateUser = null;

                    string candidateUsername = null;
                    string candidatePassword = null;
                    string candidateSmsPhone = null;
                    string candidateEmail = null;
                    string candidateName = null;

                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                    }

                    if (candidate != null)
                    {
                        candidateEmail = candidate.Email;
                        candidateSmsPhone = candidate.SMSPhone;
                        candidateName = candidate.FirstName;
                        using (var db = new CandidateDataManager())
                        {
                            candidateUser = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                        }
                    }

                    if (candidateUser != null)
                    {
                        candidateUsername = candidateUser.UsernameLoginId;
                        candidatePassword = candidateUser.Password;
                    }

                    if (!string.IsNullOrEmpty(candidateUsername) && !string.IsNullOrEmpty(candidatePassword) &&
                        !string.IsNullOrEmpty(candidateSmsPhone) && !string.IsNullOrEmpty(candidateEmail))
                    {
                        SendSms(candidateSmsPhone, candidateUsername, candidatePassword, candidate.ID);
                        SendEmail(candidateName, candidateEmail, candidateUsername, candidatePassword, candidate.ID);
                    }
                }
            }
        }

        private void SendSms(string smsPhone, string username, string password, long candidateId)
        {
            if (!string.IsNullOrEmpty(smsPhone) && smsPhone.Count() == 14 && smsPhone.Contains("+"))
            {
                //string messageBody = "BUP Admission: Username: " + username + " ; Password: " + password + " ";
                string messageBody = "BUP Admission. Login to https://admission.bup.edu.bd/Admission/Login . Username: " + username + " ; Password: " + password + " ";

                string stringData = SMSUtility.Send(smsPhone, messageBody);

                string statusT = JObject.Parse(stringData)["statusCode"].ToString();

                if (statusT != "200") //if sms sending fails
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending failed in IPNListener.aspx";
                    smsLog.Attribute2 = "Failed";
                    smsLog.Attribute3 = "IPNListener.aspx";
                    smsLog.CreatedBy = candidateId;
                    smsLog.CreatedDate = DateTime.Now;
                    smsLog.CurrentSMSReferenceNo = stringData;
                    smsLog.Message = messageBody;
                    smsLog.StudentId = candidateId;
                    smsLog.PhoneNo = smsPhone;
                    smsLog.SenderUserId = -99;
                    smsLog.SentReferenceId = null;
                    smsLog.SentSMSId = null;
                    smsLog.SmsSendDate = DateTime.Now;
                    smsLog.SmsType = -1;

                    LogWriter.SmsLog(smsLog);
                }
                else //if sms sending passed
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending successful IPNListener.aspx";
                    smsLog.Attribute2 = "Success";
                    smsLog.Attribute3 = "IPNListener.aspx";
                    smsLog.CreatedBy = candidateId;
                    smsLog.CreatedDate = DateTime.Now;
                    smsLog.CurrentSMSReferenceNo = stringData;
                    smsLog.Message = messageBody;
                    smsLog.StudentId = candidateId;
                    smsLog.PhoneNo = smsPhone;
                    smsLog.SenderUserId = -99;
                    smsLog.SentReferenceId = null;
                    smsLog.SentSMSId = null;
                    smsLog.SmsSendDate = DateTime.Now;
                    smsLog.SmsType = -1;

                    LogWriter.SmsLog(smsLog);
                }
            }
        }

        private void SendEmail(string candidateName, string email, string username, string password, long candidateId)
        {
            string mailbody = "<p>Dear " + candidateName + ",</p>" +
                        "<p>Please check your username and password given below: </p>" +
                        "<p><strong>Username:</strong> " + username + "<br/>" +
                        "<strong>Password:</strong> " + password + "<br/></p>" +
                        "<br/> <p><strong>Bangladesh University of Professionals</strong></p>";

            bool isEmailSent = EmailUtility.SendMail(email, "no-reply-2@bup.edu.bd", "BUP Admission", "Username and Password", mailbody);

            if (isEmailSent == true)
            {
                DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                eLog.MessageBody = mailbody;
                eLog.MessageSubject = "Username and Password";
                eLog.Page = "IPNListener.aspx";
                eLog.SentBy = candidateId.ToString();
                eLog.StudentId = candidateId;
                eLog.ToAddress = email;
                eLog.ToName = candidateName;
                eLog.DateSent = DateTime.Now;
                eLog.FromAddress = "no-reply-2@bup.edu.bd";
                eLog.FromName = "BUP Admission";
                eLog.Attribute1 = "Success";

                LogWriter.EmailLog(eLog);

            }
            else if (isEmailSent == false)
            {
                DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                eLog.MessageBody = mailbody;
                eLog.MessageSubject = "Username and Password";
                eLog.Page = "IPNListener.aspx";
                eLog.SentBy = candidateId.ToString();
                eLog.StudentId = candidateId;
                eLog.ToAddress = email;
                eLog.ToName = candidateName;
                eLog.DateSent = DateTime.Now;
                eLog.FromAddress = "no-reply-2@bup.edu.bd";
                eLog.FromName = "BUP Admission";
                eLog.Attribute1 = "Failed";

                LogWriter.EmailLog(eLog);
            }
        }
    }
}