using DATAMANAGER;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class SelectProgramV4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["BackFromPurchaseForm_Session"] != null)
                {
                    int backFromPurchaseForm = Convert.ToInt32(Session["BackFromPurchaseForm_Session"]);
                    if (backFromPurchaseForm > 0)
                    {
                        Session["BackFromPurchaseForm_Session"] = "0";

                        Page.Response.Redirect(Page.Request.Url.ToString(), true);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            if (!IsPostBack)
            {
                

                btnApply.Visible = false;

                Session["CandidatePayment_Session"] = null;
                Session["CandidateFormSerial_Session"] = null;
                Session["AdmUnitObj_Session"] = null;

                //schoolIdList = null;
                int educationCategory = Convert.ToInt32(Request.QueryString["ecat"]);
                if (educationCategory == 4) //|| educationCategory == 6 || educationCategory == 8
                {
                    LoadListView(educationCategory);
                    //CleanCheckboxFromListView();

                    #region N/A
                    //if (educationCategory == 4)
                    //{
                    //    lblEducationCat.Text = "Undergraduate Programs";
                    //    //panelOfflineMasters.Visible = false;
                    //}
                    //else if (educationCategory == 6)
                    //{
                    //    lblEducationCat.Text = "Masters Program";
                    //    //panelOfflineMasters.Visible = true;
                    //}
                    //else if (educationCategory == 8)
                    //{
                    //    lblEducationCat.Text = "Certificate/Professional Course";
                    //    //panelOfflineMasters.Visible = false;
                    //} 
                    #endregion
                }
            }
        }



        private void LoadListView(int educataionCategoryId)
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.AdmissionSetup> list = db.GetAllActiveAdmissionSetupByEducationCategoryID(educataionCategoryId, true)
                    .Where(a => a.StartDate.Date <= DateTime.Now.Date
                            && a.EndDate.Date >= DateTime.Now.Date)
                    .OrderBy(a => a.AdmissionUnit.UnitName)
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
                DAL.AdmissionSetup admissionSetup = (DAL.AdmissionSetup)((ListViewDataItem)(e.Item)).DataItem;

                Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
                Label lblStartDate = (Label)currentItem.FindControl("lblStartDate");
                Label lblEndDate = (Label)currentItem.FindControl("lblEndDate");
                Label lblFee = (Label)currentItem.FindControl("lblFee");
                LinkButton lnkViewDetails = (LinkButton)currentItem.FindControl("lnkViewDetails");
                //LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkApply");

                lblUnitName.Text = admissionSetup.AdmissionUnit.UnitName;
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

                //-------Hidden by Ariq because new button added for going PurchaseForm.aspx page
                //lnkViewDetails.CommandName = "ViewDetails";
                //lnkViewDetails.CommandArgument = admissionSetup.ID.ToString();


            }
        }

        protected void ckbxSelectedSchool_CheckedChanged(object sender, EventArgs e)
        {
            #region N/A
            //int total = 0;
            //int noOfSelectedSchool = 0;
            //foreach (GridViewRow row in gvMultipleApplications.Rows)
            //{
            //    CheckBox ckbxSelected = (CheckBox)row.FindControl("ckbxSelectedSchool");

            //    if (ckbxSelected.Checked && ckbxSelected.Enabled == true) //only get those checkboxes that are checked and enabled.
            //    {
            //        Label admSetupFee = (Label)row.FindControl("lblFee");
            //        total += Int32.Parse(admSetupFee.Text.ToString());
            //        noOfSelectedSchool += 1;
            //    }
            //}

            //if (total > 0)
            //{
            //    lblTotal.Text = total.ToString();
            //    lblNoOfSelSchool.Text = noOfSelectedSchool.ToString();
            //    btnNext.Visible = true;
            //}
            //else if (total == 0)
            //{
            //    lblTotal.Text = "0";
            //    lblNoOfSelSchool.Text = "0";
            //    btnNext.Visible = false;
            //} 
            #endregion




            int totalAmount = 0;
            int noOfSelectedSchool = 0;
            int uncheckCount = 0;
            for (int i = 0; i < lvAdmSetup.Items.Count; i++)
            {
                ListViewItem lvi = lvAdmSetup.Items[i];

                CheckBox ch = (CheckBox)lvi.FindControl("CheckBox1");

                HiddenField admSetupID = (HiddenField)lvi.FindControl("HiddenField1");
                HiddenField admSetupAdmUnitID = (HiddenField)lvi.FindControl("HiddenField2");
                HiddenField admSetupAcaCalId = (HiddenField)lvi.FindControl("HiddenField3");
                HiddenField admSetupFee = (HiddenField)lvi.FindControl("HiddenField4");

                if (ch.Checked)
                {
                    totalAmount = totalAmount + Convert.ToInt32(admSetupFee.Value);
                    noOfSelectedSchool += 1;
                }
                else
                {
                    uncheckCount++;
                }


            }

            if (totalAmount > 0)
            {
                lblTotal.Text = "BDT. " + totalAmount.ToString();
                lblNoOfSelSchool.Text = noOfSelectedSchool.ToString();
                btnApply.Visible = true;

                lblTotal.Attributes.CssStyle.Add("font-weight", "bold");
                lblTotal.Attributes.CssStyle.Add("color", "crimson");

                lblNoOfSelSchool.Attributes.CssStyle.Add("font-weight", "bold");
                lblNoOfSelSchool.Attributes.CssStyle.Add("color", "crimson");
            }

            if (uncheckCount == 4)
            {
                lblTotal.Text = "";
                lblNoOfSelSchool.Text = "";
                btnApply.Visible = false;
            }
        }




        #region This section work when student has a Payment ID and want to pay his form.
        //-----This section work when student has a Payment ID and want to pay his form.
        protected void btnGoForPayment_Click(object sender, EventArgs e)
        {
            lblOLevelResult.Text = string.Empty;

            long paymentId = -1;
            DAL.CandidatePayment cPaymentObj = null;
            List<DAL.CandidateFormSl> cFormSlList = null;
            int educationCategoryId = -1;

            try
            {
                paymentId = Int64.Parse(txtPaymentId.Text.Trim());
            }
            catch (Exception ex)
            {
                lblOLevelResult.Text = "Invalid Payment ID";
                lblOLevelResult.ForeColor = Color.Crimson;
                return;
            }

            if (paymentId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        cPaymentObj = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == paymentId)
                            .FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    cPaymentObj = null;
                }
            }//end if (paymentId > 0)

            if (cPaymentObj != null)
            {
                using (var db = new CandidateDataManager())
                {
                    cFormSlList = db.GetAllCandidateFormSlByCandID_AD(Convert.ToInt64(cPaymentObj.CandidateID));
                }

                if ((cFormSlList != null && cFormSlList.Count > 0) && cFormSlList.FirstOrDefault().AdmissionSetup != null)
                {
                    educationCategoryId = cFormSlList.FirstOrDefault().AdmissionSetup.EducationCategoryID;
                }

                if (educationCategoryId == 4 && (cFormSlList != null && cFormSlList.Count > 0))
                {
                    int countAdmissionIsOpen = -1;
                    foreach(var tData in cFormSlList)
                    {
                        if (tData.AdmissionSetup.StartDate.Date <= DateTime.Now.Date 
                            && tData.AdmissionSetup.EndDate.Date >= DateTime.Now.Date)
                        {
                            countAdmissionIsOpen++;
                        }
                    }

                    if (countAdmissionIsOpen > 0)
                    {
                        //string urlParam = cId + ";" + candidatePaymentIDLong + ";"
                        //                + -1 + ";1;" + -1 + ";"
                        //                + 4 + ";";
                        string urlParam = cPaymentObj.CandidateID.ToString() + ";" + cPaymentObj.ID.ToString() + ";"
                                    + -1 + ";1;" + -1 + ";"
                                    + 4 + ";";

            

                        Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
                    }
                    else
                    {
                        lblOLevelResult.Text = "Admission is closed!";
                        lblOLevelResult.Attributes.CssStyle.Add("font-weight", "bold");
                        lblOLevelResult.Attributes.CssStyle.Add("color", "crimson");
                        return;
                    }


                }
                else
                {
                    lblOLevelResult.Text = "Only for Bachelors Program !";
                    lblOLevelResult.Attributes.CssStyle.Add("font-weight", "bold");
                    lblOLevelResult.Attributes.CssStyle.Add("color", "crimson");
                    return;

                }


            } // end if (cPaymentObj != null)
            else
            {
                lblOLevelResult.Text = "Invalid Request!";
                lblOLevelResult.ForeColor = Color.Crimson;
                return;
            }

            
            

        }
        #endregion


        protected void btnApply_Click(object sender, EventArgs e)
        {
            Session["CandidatePayment_Session"] = null;
            Session["CandidateFormSerial_Session"] = null;
            Session["AdmUnitObj_Session"] = null;
            //List<long> schoolIdList = new List<long>();

            DAL.CandidatePayment candidatePayment = new DAL.CandidatePayment();
            List<DAL.CandidateFormSl> candidateFormSl = new List<DAL.CandidateFormSl>();
            List<DAL.AdmissionUnit> admUnitObj = new List<DAL.AdmissionUnit>(); ;
            //DAL.AdmissionUnit admUnitObj = new DAL.AdmissionUnit(); ;

            int totalAmount = 0;

            for (int i = 0; i < lvAdmSetup.Items.Count; i++)
            {
                ListViewItem lvi = lvAdmSetup.Items[i];

                CheckBox ch = (CheckBox)lvi.FindControl("CheckBox1");

                HiddenField admSetupID = (HiddenField)lvi.FindControl("HiddenField1");
                HiddenField admSetupAdmUnitID = (HiddenField)lvi.FindControl("HiddenField2");
                HiddenField admSetupAcaCalId = (HiddenField)lvi.FindControl("HiddenField3");
                HiddenField admSetupFee = (HiddenField)lvi.FindControl("HiddenField4");

                if (ch.Checked)
                {
                    //long schoolId = Convert.ToInt64(lblSchoolId.Value);
                    //schoolIdList.Add(schoolId);

                    candidatePayment.AcaCalID = Convert.ToInt32(admSetupAcaCalId.Value);
                    totalAmount = totalAmount + Convert.ToInt32(admSetupFee.Value);

                    DAL.CandidateFormSl formSl = new DAL.CandidateFormSl();

                    //formSl.CandidateID = cId;
                    formSl.AdmissionSetupID = Convert.ToInt32(admSetupID.Value);
                    formSl.AcaCalID = Convert.ToInt32(admSetupAcaCalId.Value);
                    //formSl.CreatedBy = cId;

                    DAL.AdmissionUnit admUnitTemp = null;
                    long admUnitId = Convert.ToInt64(admSetupAdmUnitID.Value);

                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admUnitTemp = db.AdmissionDB.AdmissionUnits.Find(admUnitId);
                        }
                    }
                    catch (Exception)
                    {
                        lblOLevelResult.Text = "Error getting AdmissionUnits info.";
                        lblOLevelResult.ForeColor = Color.Crimson;
                        return;
                    }

                    if (admUnitTemp != null)
                    {
                        admUnitObj.Add(admUnitTemp);
                        formSl.Attribute1 = admUnitTemp.ID.ToString();
                        formSl.Attribute2 = admUnitTemp.UnitCode1.ToString();
                    }

                    candidateFormSl.Add(formSl);


                }

                Session["CandidateFormSerial_Session"] = candidateFormSl;
                Session["AdmUnitObj_Session"] = admUnitObj;

            }


            candidatePayment.Amount = totalAmount;
            //candidatePayment.CandidateID = cId;
            candidatePayment.IsPaid = false;
            //candidatePayment.DateCreated = DateTime.Now;
            //candidatePayment.CreatedBy = cId;

            Session["CandidatePayment_Session"] = candidatePayment;



            if (Session["CandidateFormSerial_Session"] != null && Session["CandidatePayment_Session"] != null &&
                Session["AdmUnitObj_Session"] != null)
            {

                List<DAL.CandidateFormSl> candidateFormSlT = new List<DAL.CandidateFormSl>();
                List<DAL.AdmissionUnit> admUnitObjT = new List<DAL.AdmissionUnit>();

                candidateFormSlT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];
                DAL.CandidateFormSl candidateFormSlTT = new DAL.CandidateFormSl();
                candidateFormSlTT = candidateFormSlT.FirstOrDefault();

                admUnitObjT = (List<DAL.AdmissionUnit>)Session["AdmUnitObj_Session"];
                DAL.AdmissionUnit admUnitObjTT = new DAL.AdmissionUnit();
                admUnitObjTT = admUnitObjT.FirstOrDefault();



                using (var db = new OfficeDataManager())
                {
                    DAL.AdmissionSetup admissionSetup = db.AdmissionDB.AdmissionSetups.Find(candidateFormSlTT.AdmissionSetupID);
                    if (admissionSetup.ID > 0 && admissionSetup.IsActive == true)
                    {
                        DAL.AdmissionUnit admissionUnit = db.AdmissionDB.AdmissionUnits.Find(admUnitObjTT.ID);
                        if (admissionUnit.ID > 0 && admissionUnit.IsActive == true)
                        {

                            Response.Redirect("~/Admission/Candidate/PurchaseFormV3.aspx?asi=" + admissionSetup.ID + "&aui=" + admissionUnit.ID, false);

                            //// ==== Tetitalk Api Intigation Application Form
                            //Response.Redirect("~/Admission/Candidate/PurchaseFormBachelor.aspx?asi=" + admissionSetup.ID + "&aui=" + admissionUnit.ID, false);

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


    }
}