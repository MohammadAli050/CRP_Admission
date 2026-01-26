using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class MultipleApplication : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                DAL.GlobalAdmissionSetting gas = null;
                using (var db = new OfficeDataManager())
                {
                    gas = db.GetGAS();
                }
                if (gas != null)
                {
                    if (gas.IsMultipleApplicationAvailable == true)
                    {
                        LoadGridViewData();
                        gvMultipleApplications.Visible = true;
                    }
                    else
                    {
                        gvMultipleApplications.Visible = false;
                        lblMessage.Text = "Multiple application purchase is not available.";
                    }
                }

                //TODO: do not allow masters candidates to access this page...
                //List<DAL.CandidateFormSl> cFormSlList = null;

                btnNext.Visible = false;
            }
        }

        private void LoadGridViewData()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.SPGetSchoolForMultiplePurchase_Result> list = db.AdmissionDB.SPGetSchoolForMultiplePurchase()
                    .Where(a => a.admSetupStartDate.Date <= DateTime.Now.Date
                            && a.admSetupEndDate.Date >= DateTime.Now.Date).ToList();
                if (list.Count() > 0)
                {
                    gvMultipleApplications.DataSource = list;
                }
                else
                {
                    gvMultipleApplications.DataSource = null;
                }
                gvMultipleApplications.DataBind();
            }
        }

        protected void ckbxSelectedSchool_CheckedChanged(object sender, EventArgs e)
        {
            int total = 0;
            int noOfSelectedSchool = 0;
            foreach (GridViewRow row in gvMultipleApplications.Rows)
            {
                CheckBox ckbxSelected = (CheckBox)row.FindControl("ckbxSelectedSchool");

                if (ckbxSelected.Checked && ckbxSelected.Enabled == true) //only get those checkboxes that are checked and enabled.
                {
                    Label admSetupFee = (Label)row.FindControl("lblFee");
                    total += Int32.Parse(admSetupFee.Text.ToString());
                    noOfSelectedSchool += 1;
                }
            }

            if (total > 0)
            {
                lblTotal.Text = total.ToString();
                lblNoOfSelSchool.Text = noOfSelectedSchool.ToString();
                btnNext.Visible = true;
            }
            else if (total == 0)
            {
                lblTotal.Text = "0";
                lblNoOfSelSchool.Text = "0";
                btnNext.Visible = false;
            }
        }

        protected void gvMultipleApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long cId = -1;

                if (uId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        cId = db.GetCandidateIdByUserID_ND(uId);
                    }
                }

                CheckBox ckbxSelected = (CheckBox)e.Row.FindControl("ckbxSelectedSchool");

                Label hfAdmSetupId = (Label)e.Row.FindControl("hfasi");
                Label hfAdmUnitId = (Label)e.Row.FindControl("hfaui");
                Label hfAcaCalId = (Label)e.Row.FindControl("hfaci");

                long admSetupId = Int64.Parse(hfAdmSetupId.Text);
                long admUnitId = Int64.Parse(hfAdmUnitId.Text);
                int acaCalId = Int32.Parse(hfAcaCalId.Text);

                if (cId > 0)
                {
                    DAL.CandidateFormSl cFormSl = null;
                    using (var db = new CandidateDataManager())
                    {
                        cFormSl = db.GetAllCandidateFormSlByCandID_AD(cId).Where(c => c.AdmissionSetupID == admSetupId && c.CandidatePayment.IsPaid == true).FirstOrDefault();
                        if (cFormSl != null)
                        {
                            if (cFormSl.AdmissionSetupID == Int64.Parse(hfAdmSetupId.Text))
                            {
                                ckbxSelected.Checked = true;
                                ckbxSelected.Enabled = false;  //disable the checkbox if this school/faculty/program is already purchased.
                                e.Row.ForeColor = Color.Crimson;
                            }
                        }
                    }
                }

            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            long cId = -1;

            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
            }

            if (cId > 0)
            {
                DAL.CandidatePayment candidatePayment = new DAL.CandidatePayment();
                List<DAL.CandidateFormSl> candidateFormSl = new List<DAL.CandidateFormSl>();
                DAL.AdmissionUnit admUnitObj = null;

                int totalAmount = 0;

                foreach (GridViewRow row in gvMultipleApplications.Rows)
                {
                    CheckBox ckbxSelected = (CheckBox)row.FindControl("ckbxSelectedSchool");

                    Label hfAdmSetupId = (Label)row.FindControl("hfasi");
                    Label hfAdmUnitId = (Label)row.FindControl("hfaui");
                    Label hfAcaCalId = (Label)row.FindControl("hfaci");
                    Label admSetupFee = (Label)row.FindControl("lblFee");

                    if (ckbxSelected.Checked && ckbxSelected.Enabled == true) //only get those checkboxes that are checked and enabled.
                    {
                        candidatePayment.AcaCalID = Int32.Parse(hfAcaCalId.Text.ToString());
                        totalAmount = totalAmount + Int32.Parse(admSetupFee.Text.ToString());

                        DAL.CandidateFormSl formSl = new DAL.CandidateFormSl();

                        formSl.CandidateID = cId;
                        formSl.AdmissionSetupID = Int32.Parse(hfAdmSetupId.Text.ToString());
                        formSl.AcaCalID = Int32.Parse(hfAcaCalId.Text.ToString());
                        //formSl.CandidatePaymentID = ;
                        formSl.CreatedBy = cId;

                        DAL.AdmissionUnit admUnitTemp = null;
                        long admUnitId = Int64.Parse(hfAdmUnitId.Text.ToString());

                        try
                        {
                            using(var db = new OfficeDataManager())
                            {
                                admUnitTemp = db.AdmissionDB.AdmissionUnits.Find(admUnitId);
                            }
                        }
                        catch (Exception)
                        {
                            messagePanel.Visible = true;
                            lblMessage1.Text = "Error getting faculty info.";
                            messagePanel.CssClass = "alert alert-danger";
                            return;
                        }

                        if(admUnitTemp != null)
                        {
                            admUnitObj = admUnitTemp;
                            formSl.Attribute2 = admUnitTemp.UnitCode1.ToString();
                        }

                        candidateFormSl.Add(formSl);
                    }
                }
                candidatePayment.Amount = totalAmount;
                candidatePayment.CandidateID = cId;
                candidatePayment.IsPaid = false;
                candidatePayment.DateCreated = DateTime.Now;
                candidatePayment.CreatedBy = cId;

                long candidatePaymentIDLong = -1;
                long candidateFormSerialIDLong = -1;
                List<long> candidateFormSerialIDList = new List<long>();

                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                        db.AdmissionDB.SPCandidatePaymentInsert(id_param, candidatePayment.CandidateID, null, candidatePayment.AcaCalID, Convert.ToInt32(admUnitObj.UnitCode1), candidatePayment.IsPaid, candidatePayment.Amount, cId, candidatePayment.DateCreated);
                        candidatePaymentIDLong = Convert.ToInt64(id_param.Value);
                        if (candidatePaymentIDLong > 0)
                        {
                            if (candidateFormSl.Count() > 0)
                            {
                                foreach (DAL.CandidateFormSl item in candidateFormSl)
                                {
                                    item.CandidatePaymentID = candidatePaymentIDLong; //add candidatePaymentID to candidateFormSerial as foreign key.

                                    using (var dbInsertFsl = new CandidateDataManager())
                                    {
                                        ObjectParameter id_param1 = new ObjectParameter("iD", typeof(long));
                                        //db.AdmissionDB.SPCandidateFormSlInsert(id_param1, candidateIdLong, admissionSetup.ID, admissionSetup.AcaCalID, null, candidatePaymentIDLong, DateTime.Now, -99);
                                        dbInsertFsl.AdmissionDB.SPCandidateFormSlInsert(id_param1, item.CandidateID, item.AdmissionSetupID, item.AcaCalID, Convert.ToInt32(item.Attribute2), null, candidatePaymentIDLong, DateTime.Now, cId);
                                        candidateFormSerialIDLong = Convert.ToInt64(id_param1.Value);
                                        candidateFormSerialIDList.Add(candidateFormSerialIDLong);
                                    }
                                }//end foreach
                            }// end if (candidateFormSl.Count() > 0)
                        }// end if (candidatePaymentIDLong > 0)
                    }
                }
                catch (Exception)
                {
                    messagePanel.Visible = true;
                    lblMessage1.Text = "Error saving Payment ID.";
                    messagePanel.CssClass = "alert alert-danger";
                    return;
                }
                

                //
                if (candidatePaymentIDLong > 0)
                {
                    if (candidateFormSl.Count().Equals(candidateFormSerialIDList.Count())) //check whether number of candidate form serial object inserted is the same as candidate form serial id return from db.
                    {
                        string urlParam = cId + ";" + candidatePaymentIDLong + ";"
                                + -1 + ";1;" + -1 + ";"
                                + 4 + ";";
                        Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
                    }

                }


            }
        }
    }
}