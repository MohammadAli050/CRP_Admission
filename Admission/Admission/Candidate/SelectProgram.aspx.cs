using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class SelectProgram : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                int educationCategory = 0;

                int certiEducationCategory = Convert.ToInt32(Request.QueryString["ecat"]);

                Session["certiEducationCategory"] = certiEducationCategory;

                if (certiEducationCategory == 0)
                {
                    educationCategory = 6;
                }
                else
                    educationCategory = certiEducationCategory;

                if (educationCategory == 4 || educationCategory == 6 || educationCategory == 8)
                {
                    LoadListView(educationCategory);
                }

                panelOtherPrograms.Visible = false;
                LoadOtherProgramsData();
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

                try
                {
                    int Category = Convert.ToInt32(Session["certiEducationCategory"]);
                    if (Category == 0)
                        list = list.Where(x => x.AdmissionUnitID == 24 || x.AdmissionUnitID == 25).ToList(); // Only For Certificate
                    else
                        list = list.Where(x => x.AdmissionUnitID != 24 && x.AdmissionUnitID != 25).ToList();

                }
                catch (Exception ex)
                {
                }

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
                    DAL.AdmissionSetup obj = db.AdmissionDB.AdmissionSetups.Find(admissionSetupId);
                    if (obj != null && obj.ID > 0 && obj.IsActive == true)
                    {

                        //Session["sp_asi_selectProgram"] = obj.ID;
                        //Session["sp_aui_selectProgram"] = obj.AdmissionUnitID;
                        Response.Redirect("~/Admission/Candidate/ProgramInformation.aspx?asi=" + obj.ID + "&aui=" + obj.AdmissionUnitID, false);
                    }
                    else
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
                    }
                }
            }
        }

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


                    using (var db = new OfficeDataManager())
                    {
                        if (cPaymentObj != null)
                        {

                            List<DAL.SPAdmissionSetupByPaymentID_Result> aData = new List<DAL.SPAdmissionSetupByPaymentID_Result>();
                            using (var db1 = new OfficeDataManager())
                            {
                                aData = db1.AdmissionDB.SPAdmissionSetupByPaymentID(paymentId).ToList();
                            }

                            if (aData[0].EducationCategoryID != 6)
                            {
                                lblOLevelResult.Text = "Invalid Payment ID";
                                lblOLevelResult.ForeColor = Color.Crimson;
                                txtPaymentId.Text = "";
                                return;
                            }
                        }
                        else
                        {
                            lblOLevelResult.Text = "No Data Found !!";
                            lblOLevelResult.ForeColor = Color.Crimson;
                            txtPaymentId.Text = "";
                            return;
                        }
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

                Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
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

                Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
            }

        }
        #endregion









        private void LoadOtherProgramsData()
        {
            try
            {
                List<DAL.SPGetAllAdmissionSetupOtherPrograms_Result> list = null;
                using (var db = new OfficeDataManager())
                {
                    list = db.AdmissionDB.SPGetAllAdmissionSetupOtherPrograms(1, null, null).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    panelOtherPrograms.Visible = true;
                    lvProgramPriority.DataSource = list.OrderBy(x => x.ProgramId).ToList();
                    lvProgramPriority.DataBind();
                }
                else
                {
                    panelOtherPrograms.Visible = false;
                    lvProgramPriority.DataSource = null;
                    lvProgramPriority.DataBind();
                }
            }
            catch (Exception ex)
            {
                panelOtherPrograms.Visible = false;
                lvProgramPriority.DataSource = null;
                lvProgramPriority.DataBind();
            }

        }

        protected void lvProgramPriority_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetAllAdmissionSetupOtherPrograms_Result progP = (DAL.SPGetAllAdmissionSetupOtherPrograms_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblFileTypeName = (Label)currentItem.FindControl("lblFileTypeName");

                HyperLink hlBtn = (HyperLink)currentItem.FindControl("hlBtn");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblFileTypeName.Text = progP.FileTypeName;

                if (progP.FileTypeId == 1)
                {
                    hlBtn.Text = "Notice View";
                    hlBtn.NavigateUrl = progP.FileURL;
                    hlBtn.Target = "_blank";
                    hlBtn.CssClass = "btn btn-info";
                    hlBtn.Attributes.CssStyle.Add("width", "125px");

                    hlBtn.Visible = true;
                }
                else if (progP.FileTypeId == 2)
                {
                    hlBtn.Text = "Form Download";
                    hlBtn.NavigateUrl = progP.FileURL;
                    hlBtn.Target = "_blank";
                    hlBtn.CssClass = "btn btn-success";

                    hlBtn.Visible = true;
                }
                else
                {
                    hlBtn.Visible = false;
                }


            }
        }

        protected void lvProgramPriority_ItemCommand(object sender, ListViewCommandEventArgs e)
        { }

        protected void lvProgramPriority_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        { }

        protected void lvProgramPriority_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        { }

        string lastValue = "";
        protected string AddGroupingHeader()
        {

            string currentValue = "";

            //Get the data field value of interest for this row
            //string programShortName = Eval("ProgramShortName").ToString();
            string programDetailsName = Eval("ProgramDetailsName").ToString();
            currentValue = programDetailsName; // + " (" + programShortName + ")";

            //Specify name to display if dataFieldValue is a database NULL
            if (currentValue.Length == 0)
            {
                currentValue = "";
            }

            string sNewRow = "";
            //See if there's been a change in value
            if (lastValue != currentValue)
            {
                //There's been a change! Record the change and emit the header
                lastValue = currentValue;
                sNewRow = "<tr style='background-color: gainsboro;'>" +
                            "<td colspan='3'><h4>" + currentValue + "</h4></td>" +
                          "</tr>";
                return sNewRow;
            }
            else
            {
                return "";
            }
        }









    }
}