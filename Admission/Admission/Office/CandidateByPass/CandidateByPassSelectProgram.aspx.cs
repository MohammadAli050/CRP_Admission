using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Admission.Admission.Office.CandidateByPass
{
    public partial class CandidateByPassSelectProgram : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnApply1.Visible = false;

                Session["CandidatePayment_Session"] = null;
                Session["CandidateFormSerial_Session"] = null;
                Session["AdmUnitObj_Session"] = null;

                //schoolIdList = null;
                int educationCategory = Convert.ToInt32(Request.QueryString["ecat"]);
                if (educationCategory == 4 || educationCategory == 6 || educationCategory == 8)
                {
                    LoadListView(educationCategory);
                    if (educationCategory == 4)
                    {
                        lblEducationCat.Text = "Undergraduate Programs";
                        //panelOfflineMasters.Visible = false;
                    }
                    else if (educationCategory == 6)
                    {
                        lblEducationCat.Text = "Masters Program";
                        //panelOfflineMasters.Visible = true;
                    }
                    else if (educationCategory == 8)
                    {
                        lblEducationCat.Text = "Certificate/Professional Course";
                        //panelOfflineMasters.Visible = false;
                    }
                }
            }
        }

        private void LoadListView(int educataionCategoryId)
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.AdmissionSetup> list = db.GetAllActiveAdmissionSetupByEducationCategoryID(educataionCategoryId, true)
                    .OrderBy(a => a.AdmissionUnit.UnitName)
                    .ToList();  //.Where(a => a.StartDate.Date <= DateTime.Now.Date && a.EndDate.Date >= DateTime.Now.Date)

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


        //-------Hidden by Ariq because new button added for going PurchaseForm.aspx page

        //protected void lvAdmSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    //TODO: implement code to allow access during maintenance

        //    if (e.CommandName == "ViewDetails")
        //    {

        //        long admissionSetupId = Convert.ToInt64(e.CommandArgument.ToString());
        //        using (var db = new OfficeDataManager())
        //        {
        //            DAL.AdmissionSetup obj = db.AdmissionDB.AdmissionSetups.Find(admissionSetupId);
        //            if (obj != null && obj.ID > 0 && obj.IsActive == true)
        //            {
        //                //Session["sp_asi_selectProgram"] = obj.ID;
        //                //Session["sp_aui_selectProgram"] = obj.AdmissionUnitID;
        //                Response.Redirect("~/Admission/Candidate/ProgramInformation.aspx?asi=" + obj.ID + "&aui=" + obj.AdmissionUnitID, false);
        //            }
        //            else
        //            {
        //                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
        //            }
        //        }
        //    }
        //}





        #region This section work when student has a Payment ID and want to pay his form.
        //-----This section work when student has a Payment ID and want to pay his form.
        protected void btnNext_Click(object sender, EventArgs e)
        {
            long paymentId = -1;
            DAL.CandidatePayment cPaymentObj = null;
            DAL.BasicInfo candidate = null;
            DAL.CandidateFormSl cFormSlObj = null;
            List<DAL.CandidateFormSl> cFormSlList = null;
            DAL.AdmissionSetup admSetup = null;
            List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
            DAL.AdmissionUnit admUnit = null;
            List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();
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

            #region CANDIDATE PAYMENT
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
            #endregion

            #region CANDIDATE & CANDIDATE FORMSERIAL
            if (cPaymentObj != null)
            {
                long candidateId = -1;
                candidateId = Int64.Parse(cPaymentObj.CandidateID.ToString());
                if (candidateId > 0)
                {
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            candidate = db.GetCandidateBasicInfoByID_AD(candidateId);
                        }
                    }
                    catch (Exception ex)
                    {
                        candidate = null;
                    }

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cFormSlList = db.AdmissionDB.CandidateFormSls
                                .Where(c => c.CandidatePaymentID == cPaymentObj.ID && c.CandidateID == candidateId)
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        cFormSlList = null;
                    }
                }
            } // end if (cPaymentObj != null)
            #endregion

            #region ADMISSION SETUP AND ADMISSION UNIT
            if (cFormSlList.Count() == 1)
            {
                // if only one form exists for this paymentId (could be single or multiple). 
                //Also Education category could be Bachelors or Marters.
                foreach (var item in cFormSlList)
                {
                    cFormSlObj = item;
                }
                if (cFormSlObj != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admSetup = db.AdmissionDB.AdmissionSetups.Find(cFormSlObj.AdmissionSetupID);
                            educationCategoryId = admSetup.EducationCategoryID; // since this could be either bachelors or masters, hence get the education category.
                        }
                    }
                    catch (Exception ex)
                    {
                        admSetup = null;
                    }
                }

                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admUnit = db.AdmissionDB.AdmissionUnits.Find(admSetup.AdmissionUnitID);
                        }
                    }
                    catch (Exception ex)
                    {
                        admUnit = null;
                    }
                }
            }
            // else if multiple form exists for this paymentId (multiple purchase varification). 
            // Should be Bachelors only. If Masters comes here, then there is something wrong in MultipleApplication.aspx.cs.
            else if (cFormSlList.Count() > 1)
            {
                try
                {
                    foreach (var item in cFormSlList)
                    {
                        using (var db = new OfficeDataManager())
                        {
                            DAL.AdmissionSetup _admSetupTemp = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                            if (_admSetupTemp != null)
                            {
                                admSetupList.Add(_admSetupTemp);
                                // only candidates applying for Bachelors purchasing multiple applicaiton should be here.
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    admSetupList = null;
                }

                if (admSetupList.Count() > 1)
                {
                    try
                    {
                        foreach (var item in admSetupList)
                        {
                            using (var db = new OfficeDataManager())
                            {
                                DAL.AdmissionUnit _admUnitTemp = db.AdmissionDB.AdmissionUnits.Find(item.AdmissionUnitID);
                                if (_admUnitTemp != null)
                                {
                                    admUnitList.Add(_admUnitTemp);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        admUnitList = null;
                    }
                }
            } // end if-else cFormSls.Count()
            #endregion

            // Only One form means this could be either Bachelor or Masters.
            if (cPaymentObj != null && candidate != null && cFormSlObj != null && (cFormSlList.Count() == 1) && admSetup != null && admUnit != null)
            {
                //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                //            + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
                //            + admissionSetup.EducationCategoryID + ";";
                string urlParam = candidate.ID + ";" + cPaymentObj.ID + ";"
                            + cFormSlObj.ID + ";0;" + admUnit.ID + ";"
                            + admSetup.EducationCategoryID + ";";

                //TODO: Insert Log Here

                Response.Redirect("~/Admission/Office/CandidateByPass/CandidateByPassPurchaseNotification.aspx?value=" + urlParam, false);
            }
            // More than one form exist for one payment Id, so this is Bachelor application, and most likely was a multiple purchase.
            else if (cPaymentObj != null && candidate != null && (cFormSlList.Count() > 1) && (admSetupList.Count() > 1) && (admSetupList.Count() > 1))
            {
                //string urlParam = cId + ";" + candidatePaymentIDLong + ";"
                //                + -1 + ";1;" + -1 + ";"
                //                + 4 + ";";
                string urlParam = candidate.ID + ";" + cPaymentObj.ID + ";"
                                + -1 + ";1;" + -1 + ";"
                                + 4 + ";";

                //TODO: Insert Log Here

                Response.Redirect("~/Admission/Office/CandidateByPass/CandidateByPassPurchaseNotification.aspx?value=" + urlParam, false);
            }

        }
        #endregion






        #region CANDIDATE MULTIPLE FORM PURCHASE
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

                            Response.Redirect("~/Admission/Office/CandidateByPass/CandidateByPassPurchaseForm.aspx?asi=" + admissionSetup.ID + "&aui=" + admissionUnit.ID, false);

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
        #endregion


        protected void ckbxSelectedSchool_CheckedChanged(object sender, EventArgs e)
        {
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
                btnApply1.Visible = true;
            }

            if (noOfSelectedSchool == 0)
            {
                lblTotal.Text = "";
                lblNoOfSelSchool.Text = "";
                btnApply1.Visible = false;
            }




        }


    }
}