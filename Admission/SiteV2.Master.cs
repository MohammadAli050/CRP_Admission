using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Admission
{
    public partial class SiteV2 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
        }


        protected void btnGoForPayment_Click(object sender, EventArgs e)
        {
            lblOLevelResult.Text = string.Empty;

            ModalPopupExtender1.Show();

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
                    foreach (var tData in cFormSlList)
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



    }
}