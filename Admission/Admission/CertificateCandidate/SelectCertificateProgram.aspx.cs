using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Admission.Admission.CertificateCandidate
{
    public partial class SelectCertificateProgram : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                int educationCategory = Convert.ToInt32(Request.QueryString["ecat"]);
                if (educationCategory == 1)
                {
                    LoadListView(educationCategory);
                    if (educationCategory == 1)
                    {
                        lblEducationCat.Text = "Certificate Program";
                        //panelOfflineMasters.Visible = false;
                    }
                    else { }
                }
            }
        }

        private void LoadListView(int educataionCategoryId)
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.CertificateAdmissionSetup> list = db.GetAllCertificateActiveAdmissionSetupByEducationCategoryID(educataionCategoryId, true)
                    .Where(a => a.StartDate.Date <= DateTime.Now.Date
                            && a.EndDate.Date >= DateTime.Now.Date)
                    .OrderBy(a => a.ID)
                    .ToList();
                if (list != null)
                {
                    lvAdmSetup.DataSource = list;
                }
                else
                {
                    lvAdmSetup.DataSource = null;
                }
                lvAdmSetup.DataBind();
            }
        }

        protected void lvAdmSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.CertificateAdmissionSetup admissionSetup = (DAL.CertificateAdmissionSetup)((ListViewDataItem)(e.Item)).DataItem;

                Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
                Label lblStartDate = (Label)currentItem.FindControl("lblStartDate");
                Label lblEndDate = (Label)currentItem.FindControl("lblEndDate");
                Label lblFee = (Label)currentItem.FindControl("lblFee");
                LinkButton lnkViewDetails = (LinkButton)currentItem.FindControl("lnkViewDetails");
                //LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkApply");

                lblUnitName.Text = admissionSetup.AdmissionUnitID == 1 ? "Renewable Energy & Environment" : "Energy Management & Conservation";
                lblStartDate.Text = admissionSetup.StartDate.ToString("dd/MM/yyyy");
                lblEndDate.Text = admissionSetup.EndDate.ToString("dd/MM/yyyy");
                lblFee.Text = admissionSetup.Fee.ToString() + " (+ Charge)";


                DateTime todayDate = DateTime.Today;
                DateTime endDate = admissionSetup.EndDate;

                int noOfDaysLeft = Convert.ToInt32((endDate - todayDate).TotalDays);

                if (noOfDaysLeft > 0)
                {
                    if (noOfDaysLeft <= 5)
                    {
                        lblEndDate.ForeColor = Color.Crimson;
                        lblEndDate.Text += " (" + noOfDaysLeft + " days left)";
                    }
                }
                else if (noOfDaysLeft == 0)
                {
                    lblEndDate.ForeColor = Color.Crimson;
                    lblEndDate.Text += " (" + noOfDaysLeft + " days left)";
                }

                lnkViewDetails.CommandName = "ViewDetails";
                lnkViewDetails.CommandArgument = admissionSetup.ID.ToString();

                //lnkDelete.CommandName = "Apply";
                //lnkDelete.CommandArgument = admissionSetup.ID.ToString();
            }
        }

        protected void lvAdmSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //TODO: implement code to allow access during maintenance

            if (e.CommandName == "ViewDetails")
            {
                long admissionSetupId = Convert.ToInt64(e.CommandArgument.ToString());
                using (var db = new OfficeDataManager())
                {
                    DAL.CertificateAdmissionSetup admissionSetup = db.AdmissionDB.CertificateAdmissionSetups.Find(admissionSetupId);

                    if (admissionSetup.ID > 0 && admissionSetup.IsActive == true)
                    {
                        DAL.CertificateAdmissionUnit admissionUnit = db.AdmissionDB.CertificateAdmissionUnits.Find(admissionSetup.AdmissionUnitID);
                        if (admissionUnit.ID > 0 && admissionUnit.IsActive == true)
                        {
                            Response.Redirect("~/Admission/CertificateCandidate/CertificatePurchaseForm.aspx?asi=" + admissionSetup.ID + "&aui=" + admissionUnit.ID, false);
                        }
                        else
                        {
                            Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
                    }
                    
                }
            }
        }

        #region This section work when student has a Payment ID and want to pay his form.
        ////-----This section work when student has a Payment ID and want to pay his form.
        //protected void btnNext_Click(object sender, EventArgs e)
        //{
        //    long paymentId = -1;
        //    DAL.CandidatePayment cPaymentObj = null;
        //    DAL.BasicInfo candidate = null;
        //    DAL.CandidateFormSl cFormSlObj = null;
        //    List<DAL.CandidateFormSl> cFormSlList = null;
        //    DAL.AdmissionSetup admSetup = null;
        //    List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
        //    DAL.AdmissionUnit admUnit = null;
        //    List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();
        //    int educationCategoryId = -1;

        //    try
        //    {
        //        paymentId = Int64.Parse(txtPaymentId.Text.Trim());
        //    }
        //    catch (Exception ex)
        //    {
        //        lblOLevelResult.Text = "Invalid Payment ID";
        //        lblOLevelResult.ForeColor = Color.Crimson;
        //        return;
        //    }

        //    #region CANDIDATE PAYMENT
        //    if (paymentId > 0)
        //    {
        //        try
        //        {
        //            using (var db = new CandidateDataManager())
        //            {
        //                cPaymentObj = db.AdmissionDB.CandidatePayments
        //                    .Where(c => c.PaymentId == paymentId)
        //                    .FirstOrDefault();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            cPaymentObj = null;
        //        }
        //    }//end if (paymentId > 0)
        //    #endregion

        //    #region CANDIDATE & CANDIDATE FORMSERIAL
        //    if (cPaymentObj != null)
        //    {
        //        long candidateId = -1;
        //        candidateId = Int64.Parse(cPaymentObj.CandidateID.ToString());
        //        if (candidateId > 0)
        //        {
        //            try
        //            {
        //                using (var db = new CandidateDataManager())
        //                {
        //                    candidate = db.GetCandidateBasicInfoByID_AD(candidateId);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                candidate = null;
        //            }

        //            try
        //            {
        //                using (var db = new CandidateDataManager())
        //                {
        //                    cFormSlList = db.AdmissionDB.CandidateFormSls
        //                        .Where(c => c.CandidatePaymentID == cPaymentObj.ID && c.CandidateID == candidateId)
        //                        .ToList();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                cFormSlList = null;
        //            }
        //        }
        //    } // end if (cPaymentObj != null)
        //    #endregion

        //    #region ADMISSION SETUP AND ADMISSION UNIT
        //    if (cFormSlList.Count() == 1)
        //    {
        //        // if only one form exists for this paymentId (could be single or multiple). 
        //        //Also Education category could be Bachelors or Marters.
        //        foreach (var item in cFormSlList)
        //        {
        //            cFormSlObj = item;
        //        }
        //        if (cFormSlObj != null)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    admSetup = db.AdmissionDB.AdmissionSetups.Find(cFormSlObj.AdmissionSetupID);
        //                    educationCategoryId = admSetup.EducationCategoryID; // since this could be either bachelors or masters, hence get the education category.
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                admSetup = null;
        //            }
        //        }

        //        if (admSetup != null)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    admUnit = db.AdmissionDB.AdmissionUnits.Find(admSetup.AdmissionUnitID);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                admUnit = null;
        //            }
        //        }
        //    }
        //    // else if multiple form exists for this paymentId (multiple purchase varification). 
        //    // Should be Bachelors only. If Masters comes here, then there is something wrong in MultipleApplication.aspx.cs.
        //    else if (cFormSlList.Count() > 1)
        //    {
        //        try
        //        {
        //            foreach (var item in cFormSlList)
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    DAL.AdmissionSetup _admSetupTemp = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
        //                    if (_admSetupTemp != null)
        //                    {
        //                        admSetupList.Add(_admSetupTemp);
        //                        // only candidates applying for Bachelors purchasing multiple applicaiton should be here.
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            admSetupList = null;
        //        }

        //        if (admSetupList.Count() > 1)
        //        {
        //            try
        //            {
        //                foreach (var item in admSetupList)
        //                {
        //                    using (var db = new OfficeDataManager())
        //                    {
        //                        DAL.AdmissionUnit _admUnitTemp = db.AdmissionDB.AdmissionUnits.Find(item.AdmissionUnitID);
        //                        if (_admUnitTemp != null)
        //                        {
        //                            admUnitList.Add(_admUnitTemp);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                admUnitList = null;
        //            }
        //        }
        //    } // end if-else cFormSls.Count()
        //    #endregion

        //    // Only One form means this could be either Bachelor or Masters.
        //    if (cPaymentObj != null && candidate != null && cFormSlObj != null && (cFormSlList.Count() == 1) && admSetup != null && admUnit != null)
        //    {
        //        //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
        //        //            + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
        //        //            + admissionSetup.EducationCategoryID + ";";
        //        string urlParam = candidate.ID + ";" + cPaymentObj.ID + ";"
        //                    + cFormSlObj.ID + ";0;" + admUnit.ID + ";"
        //                    + admSetup.EducationCategoryID + ";";

        //        //TODO: Insert Log Here

        //        Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
        //    }
        //    // More than one form exist for one payment Id, so this is Bachelor application, and most likely was a multiple purchase.
        //    else if (cPaymentObj != null && candidate != null && (cFormSlList.Count() > 1) && (admSetupList.Count() > 1) && (admSetupList.Count() > 1))
        //    {
        //        //string urlParam = cId + ";" + candidatePaymentIDLong + ";"
        //        //                + -1 + ";1;" + -1 + ";"
        //        //                + 4 + ";";
        //        string urlParam = candidate.ID + ";" + cPaymentObj.ID + ";"
        //                        + -1 + ";1;" + -1 + ";"
        //                        + 4 + ";";

        //        //TODO: Insert Log Here

        //        Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
        //    }

        //}
        #endregion



    }
}