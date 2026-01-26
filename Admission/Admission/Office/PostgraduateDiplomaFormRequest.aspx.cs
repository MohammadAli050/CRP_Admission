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


namespace Admission.Admission.Office
{
    public partial class PostgraduateDiplomaFormRequest : PageBase
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                //LoadDDL();
                LoadListView();
            }
        }

        //private void LoadDDL()
        //{
        //    using (var db = new OfficeDataManager())
        //    {
        //        DDLHelper.Bind<DAL.AdmissionUnit>(ddlUnitProgram, db.GetAllAdmissionUnit(), "UnitName", "ID", EnumCollection.ListItemType.SelectAll);
        //        DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.IsActive == true).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.SelectAll);
        //        DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(a => a.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
        //    }
        //}
        
        private void LoadListView()
        {
            using (var db = new CandidateDataManager())
            {
                List<FormRequestListViewObject> list = (from candPayment in db.AdmissionDB.PostgraduateDiplomaCandidatePayments
                                                        join candFormSl in db.AdmissionDB.PostgraduateDiplomaCandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
                                                        join candidate in db.AdmissionDB.PostgraduateDiplomaBasicInfoes on candFormSl.CandidateID equals candidate.ID
                                                        join admSetup in db.AdmissionDB.PostgraduateDiplomaAdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
                                                        join admUnit in db.AdmissionDB.PostgraduateDiplomaAdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
                                                        //where candPayment.IsPaid == false
                                                        select new FormRequestListViewObject
                                                        {
                                                            CandidateID = candidate.ID,
                                                            Name = candidate.FirstName,
                                                            CandidateFormSerialID = candFormSl.ID,
                                                            FormSerial = candFormSl.FormSerial,
                                                            CandidatePaymentID = candPayment.ID,
                                                            PaymentId = candPayment.PaymentId,
                                                            IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
                                                            Mobile = candidate.SMSPhone,
                                                            Email = candidate.Email,
                                                            AdmissionSetupID = admSetup.ID,
                                                            AdmissionUnitID = admUnit.ID,
                                                            UNIT = admUnit.UnitName,
                                                            DateApplied = candidate.DateCreated
                                                        }).ToList();
                if (list.Count() > 0)
                {
                    lvFormRequest.DataSource = list.OrderByDescending(c => c.CandidateFormSerialID).ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvFormRequest.DataSource = null;
                    lblCount.Text = "0";
                }
            }
            lvFormRequest.DataBind();
        }

        private void LoadListView(string searchText, int? acaCalId)
        {
            if (searchText != null)//search using search text
            {
                using (var db = new CandidateDataManager())
                {
                    List<DAL.SPPostgraduateDiplomaFormRequestGetBySearchText_Result> list = db.AdmissionDB.SPPostgraduateDiplomaFormRequestGetBySearchText(searchText, false).ToList();
                    if (list.Any())
                    {
                        List<FormRequestListViewObject> lvObjList = new List<FormRequestListViewObject>();
                        foreach (var item in list)
                        {
                            FormRequestListViewObject obj = new FormRequestListViewObject();
                            obj.CandidateID = item.CandidateID;
                            obj.Name = item.Name;
                            obj.CandidateFormSerialID = item.CandidateFormSerialID;
                            obj.FormSerial = item.FormSerial;
                            obj.CandidatePaymentID = item.CandidatePaymentID;
                            obj.PaymentId = item.PaymentId;
                            obj.Mobile = item.Mobile;
                            obj.Email = item.Email;
                            obj.AdmissionSetupID = item.AdmissionSetupID;
                            obj.AdmissionUnitID = item.AdmissionUnitID;
                            obj.UNIT = item.UNIT;
                            obj.DateApplied = item.DateApplied;
                            if (item.IsPaid == true)
                            {
                                obj.IsPaid = "Yes";
                            }
                            else
                            {
                                obj.IsPaid = "No";
                            }
                            lvObjList.Add(obj);
                        }
                        if (lvObjList.Any())
                        {
                            lvFormRequest.DataSource = lvObjList.OrderByDescending(c => c.CandidateFormSerialID).ToList();
                            lblCount.Text = lvObjList.Count.ToString();
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
        }

        //protected void btnLoad_Click(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(ddlSession.SelectedValue) > 0)
        //    {
        //        LoadListView(null, Convert.ToInt32(ddlSession.SelectedValue));
        //    }
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchText.Text))
            {
                LoadListView(txtSearchText.Text, null);
            }
        }

        protected void lvFormRequest_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                FormRequestListViewObject obj = (FormRequestListViewObject)((ListViewDataItem)(e.Item)).DataItem;

                //Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblFormSerial = (Label)currentItem.FindControl("lblFormSerial");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
                Label lblMobile = (Label)currentItem.FindControl("lblMobile");
                Label lblEmail = (Label)currentItem.FindControl("lblEmail");
                Label lblUnit = (Label)currentItem.FindControl("lblUnit");
                Label lblDateApplied = (Label)currentItem.FindControl("lblDateApplied");
                Label lblPaid = (Label)currentItem.FindControl("lblPaid");

                LinkButton lbForm = (LinkButton)currentItem.FindControl("lbForm");

                //lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblName.Text = obj.Name;
                lblFormSerial.Text = obj.FormSerial.ToString();
                lblPaymentId.Text = obj.PaymentId.ToString();
                lblMobile.Text = obj.Mobile;
                lblEmail.Text = obj.Email;
                lblUnit.Text = obj.UNIT;
                lblDateApplied.Text = obj.DateApplied.HasValue ? obj.DateApplied.Value.ToString("dd/MM/yyyy") : "N/A";
                if (obj.IsPaid == "Yes")
                {
                    lblPaid.Text = "✓";
                    lblPaid.ForeColor = Color.Green;
                }
                else
                {
                    lblPaid.Text = "✕";
                    lblPaid.Font.Bold = true;
                    lblPaid.ForeColor = Color.Crimson;
                }

                //if (!string.IsNullOrEmpty(txtSearchText.Text))
                //{
                //    if (lblPaymentId.Text.Equals(txtSearchText.Text))
                //    {
                //        lblPaymentId.BackColor = Color.LightGray;
                //    }
                //    else if (lblMobile.Text.Equals(txtSearchText.Text))
                //    {
                //        lblMobile.BackColor = Color.LightGray;
                //    }
                //    else if (lblEmail.Text.Equals(txtSearchText.Text))
                //    {
                //        lblEmail.BackColor = Color.LightGray;
                //    }
                //}

                lbForm.CommandName = "Form";
                lbForm.CommandArgument = obj.CandidateID.ToString();

            }
        }

        protected void lvFormRequest_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            string searchText = txtSearchText.Text;
            int sessionId = -1; //Convert.ToInt32(ddlSession.SelectedValue);
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
            if (e.CommandName == "Form")
            {

                long candidateId = Int64.Parse(e.CommandArgument.ToString());
                string cipherCandidateId = candidateId.ToString(); //Encrypt.EncryptString(candidateId.ToString());

                Response.Redirect("~/Admission/PostgraduateDiploma/PDCandidateInfo/CandApplicationBasic.aspx?val=" + cipherCandidateId, false);
            }
        }



    }
}