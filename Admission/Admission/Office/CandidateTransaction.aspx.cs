using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using static Admission.Controllers.EkPayPaymentGatewayController;
using DAL.ViewModels.EkPayModel;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Reporting.WebForms;

namespace Admission.Admission.Office
{
    public partial class CandidateTransaction : PageBase
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
            }
        }

        private void LoadDDL()
        {

            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(a => a.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);

            }
        }


        private void ClearFields()
        {
            txtAmount.Text = string.Empty;
            txtBankTransactionId.Text = string.Empty;
            //txtCardHolderName.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
            txtCardType.Text = string.Empty;
            //txtCurrency.Text = string.Empty;
            txtIssuerBankCountry.Text = string.Empty;
            txtPaymentId.Text = string.Empty;
            //txtStatus.Text = string.Empty;
            txtStoreAmount.Text = string.Empty;
            txtTransactionDate.Text = string.Empty;
            txtValidationId.Text = string.Empty;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblSearchMsg.Text = string.Empty;

            try
            {
                long paymentId = -1;
                if (!string.IsNullOrEmpty(txtPaymentId.Text.Trim()))
                {
                    paymentId = Int64.Parse(txtPaymentId.Text);
                }

                if (paymentId > 0)
                {
                    DAL.CandidatePayment cPayment = null;
                    using (var db = new CandidateDataManager())
                    {
                        cPayment = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == paymentId)
                            .FirstOrDefault();
                        //&& c.IsPaid == false
                    }



                    if (cPayment != null && cPayment.IsPaid == true)
                    {
                        string msg = "";
                        msg += "Candidate Payment is Updated in Admission System ✔️";

                        DAL.TransactionHistory th = null;
                        using (var db = new OfficeDataManager())
                        {
                            th = db.AdmissionDB.TransactionHistories
                                .Where(c => c.TransactionID == txtPaymentId.Text.Trim())
                                .FirstOrDefault();
                        }
                        if (th != null)
                        {
                            msg += "<br/>";
                            msg += "Candidate Transaction Info Found ✔️";
                        }
                        else
                        {
                            msg += "<br/>";
                            msg += "Candidate Transaction Info Not Found ✖️";

                        }

                        lblSearchMsg.Text = msg;
                        lblSearchMsg.ForeColor = Color.Green;
                        //btnSave.Visible = false;
                    }
                    else
                    {
                        string msg = "";
                        msg += "Candidate Payment is Not Updated in Admission System ✖️";

                        lblSearchMsg.Text = msg;
                        lblSearchMsg.ForeColor = Color.Crimson;


                        GetDataFromSSL(cPayment);
                    }

                    #region N/A
                    //DAL.TransactionHistory th = null;
                    //using (var db = new OfficeDataManager())
                    //{
                    //    th = db.AdmissionDB.TransactionHistories
                    //        .Where(c => c.TransactionID == txtPaymentId.Text.Trim())
                    //        .FirstOrDefault();
                    //}

                    //if (cPayment != null && th == null)
                    //{
                    //    GetDataFromSSL(cPayment);

                    //    lblSearchMsg.Text = "Candidate Paid: NO  ✖️<br/> Candidate Transaction Info Not Found ✖️";
                    //    lblSearchMsg.ForeColor = Color.Green;
                    //    btnSave.Visible = true;
                    //}
                    //else if (cPayment != null && th != null)
                    //{
                    //    GetDataFromSSL(cPayment);

                    //    lblSearchMsg.Text = "Candidate Paid: NO  ✖️<br/> Candidate Transaction Info Found ✔️";
                    //    lblSearchMsg.ForeColor = Color.Green;
                    //    btnSave.Visible = false;
                    //}
                    //else
                    //{
                    //    lblSearchMsg.Text = "No such candidate payment or transaction found ✖️<br/> Or Paid Candidate Payment Already Exist ✖️";
                    //    lblSearchMsg.ForeColor = Color.Crimson;
                    //    btnSave.Visible = false;
                    //} 
                    #endregion
                }
            }
            catch (Exception ex)
            {
                lblSearchMsg.Text = "Something went wrong! Exception: " + ex.Message.ToString();
                lblSearchMsg.ForeColor = Color.Crimson;
                btnSave.Visible = false;
            }
        }
        public  class PaymentList
        {
            public long ID { get; set; }
            public Nullable<long> CandidateID { get; set; }
            public Nullable<long> PaymentId { get; set; }
            public Nullable<int> AcaCalID { get; set; }
            public Nullable<decimal> Amount { get; set; }
            public string FirstName { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
        }
        protected void LoadPaymentfail_Click(object sender, EventArgs e)
        {
            List<PaymentList> payment = new List<PaymentList>();

            DateTime fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime todate = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null);
            using (var db = new CandidateDataManager())
            {
                var list = db.AdmissionDB.SPGetUnpayRollByFromDateAndTodate(fromdate, todate).ToList();
                foreach (var item in list)
                {

                    try
                    {
                        //List<DAL.CandidateFormSl> cFormSlList = null;
                        //List<DAL.AdmissionSetup> admSetupList = null;
                        //DAL.AdmissionSetup admSetup = null;
                        DAL.Store store = null;
                        string url = null;




                        DAL.CandidateFormSl cf = null;
                        using (var dbb = new CandidateDataManager())
                        {
                            long cid = Convert.ToInt64(item.CandidateID);
                            cf = dbb.GetCandidateFormSlByCandID_AD(cid);
                        }

                        int storePrimaryId = cf.AdmissionSetup.StoreID;
                        using (var dbbb = new CandidateDataManager())
                        {
                            store = dbbb.AdmissionDB.Stores.Where(x => x.ID == storePrimaryId && x.IsActive == true).FirstOrDefault();
                        }

                        if (store != null)
                        {
                            string storeId = null;
                            string password = null;

                            try
                            {
                                storeId = Decrypt.DecryptString(store.StoreId);
                                password = Decrypt.DecryptString(store.StorePass);
                            }
                            catch (Exception)
                            {
                                lblMessage1.Text = "Error getting store information.";
                                lblMessage1.ForeColor = Color.Crimson;
                                return;
                            }

                            if (storeId != null && password != null)
                            {
                                //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
                                //    , val_id, plainText_storeId, plainText_password);

                                //OLD Style
                                //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?sessionkey={0}&store_id={1}&store_passwd={2}&v=3&format=json"
                                //    , cPayment.PaymentId.ToString(), storeId, password);


                                //NEW Style
                                url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?tran_id={0}&store_id={1}&store_passwd={2}"
                                    , item.PaymentId.ToString(), storeId, password);


                            }
                            else
                            {
                                lblMessage1.Text = "Error getting StoreId and Password.";
                                lblMessage1.ForeColor = Color.Crimson;
                                return;
                            }
                        }
                        else
                        {
                            lblMessage1.Text = "Error: store not found.";
                            lblMessage1.ForeColor = Color.Crimson;
                            return;
                        }




                      
                        if (url != null)
                        {
                            HttpClient client = new HttpClient();

                            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                                    delegate (object senderr, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                                            System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                            System.Net.Security.SslPolicyErrors sslPolicyErrors)
                                    {
                                        return true;
                                    };

                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var responseTask = client.GetAsync(url);
                            responseTask.Wait();

                            var result = responseTask.Result;
                            var contentString = result.Content.ReadAsStringAsync();

                            JObject rss = JObject.Parse(contentString.Result);

                            //OLD Style
                            //var jsonData = new WebClient().DownloadString(url);
                            //JObject rss = JObject.Parse(jsonData);

                            string paymentIDFromGateway = (string)rss["element"][0]["tran_id"];

                            string paymentStatus1 = rss["element"][0]["status"].ToString();

                            if (paymentStatus1.ToUpper().Equals("VALID") || paymentStatus1.ToUpper().Equals("VALIDATED"))
                            {
                                if (paymentIDFromGateway.Equals(item.PaymentId.ToString()))
                                {


                                    #region Response Data from SSL
                                    string paymentStatus = rss["element"][0]["status"].ToString();
                                    string paymentValue_a = rss["element"][0]["value_a"].ToString();
                                    string paymentAmount = rss["element"][0]["amount"].ToString();
                                    string storeAmount = rss["element"][0]["store_amount"].ToString();
                                    //string paymentVal_id = outputArray["val_id"].ToString();
                                    string paymentTran_id = rss["element"][0]["tran_id"].ToString();
                                    string paymentTran_date = rss["element"][0]["tran_date"].ToString();
                                    string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
                                    string paymentCard_type = rss["element"][0]["card_type"].ToString();
                                    string cardNumber = rss["element"][0]["card_no"].ToString();
                                    string currencyType = rss["element"][0]["currency_type"].ToString();
                                    string currencyAmount = rss["element"][0]["currency_amount"].ToString();
                                    string cardBrand = rss["element"][0]["card_brand"].ToString();
                                    string cardIssuer = rss["element"][0]["card_issuer"].ToString();
                                    string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
                                    string value_b = rss["element"][0]["value_b"].ToString();
                                    string value_c = rss["element"][0]["value_c"].ToString();
                                    string value_d = rss["element"][0]["value_d"].ToString();
                                    //string paymentValidated_on = outputArray["validated_on"].ToString();
                                    //string paymentRisk_title = outputArray["risk_title"].ToString();

                                    //decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                                    string paymentStatusUpper = paymentStatus.ToUpper();

                                    //lblAmount.Text = paymentAmount.ToString();
                                    //lblBankTranId.Text = paymentBank_tran_id;
                                    //lblCardNumber.Text = cardNumber;
                                    //lblCardType.Text = paymentCard_type;
                                    //lblCurrency.Text = currencyType;
                                    //lblIssuerBankCountry.Text = cardIssuer + " " + cardIssuerCountry;
                                    //lblReceivable.Text = storeAmount;
                                    if (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED"))
                                    {

                                        PaymentList paymentitem = new PaymentList();
                                        paymentitem.ID = item.ID;
                                        paymentitem.CandidateID = item.CandidateID;
                                        paymentitem.PaymentId = item.PaymentId;
                                        paymentitem.AcaCalID = item.AcaCalID;
                                        paymentitem.Amount = item.Amount;
                                        paymentitem.FirstName = item.FirstName;
                                        paymentitem.Mobile = item.Mobile;
                                        paymentitem.Email = item.Email;


                                        payment.Add(paymentitem);


                                    }
                                    else
                                    {

                                    }

                                    #endregion

                                }
                                else
                                {
                                    //lblMessage1.Text = "Candidate PaymentID from system does not match with Gateway PaymentID(1).";
                                    //lblMessage1.ForeColor = Color.Crimson;
                                    //return;
                                }
                            }






                        }


                    }
                    catch (Exception ex)
                    {
                        //lblSearchMsg.Text = "Error: Something went wrong! Exception: " + ex.Message.ToString();
                        //lblSearchMsg.ForeColor = Color.Crimson;
                        //btnSave.Visible = false;
                    }

                }
                if (payment != null)
                {
                    gvRegisteredCourse.DataSource = payment.ToList();
                    //lblCount.Text = list.Count().ToString();
                }
                else
                {
                    gvRegisteredCourse.DataSource = null;
                    //lblCount.Text = list.Count().ToString();
                }
                gvRegisteredCourse.DataBind();
            }

            //using (var db = new CandidateDataManager())
            //{
            //    var list = db.SPGetUnpayRollByFromDateAndTodate
            //        }
        }



        /// <summary>
        /// Get payment details from Payment Gateway provider.
        /// </summary>
        /// <param name="cPayment"></param>
        private void GetDataFromSSL(CandidatePayment cPayment)
        {
            //lblSearchMsg.Text = string.Empty;

            try
            {
                //List<DAL.CandidateFormSl> cFormSlList = null;
                //List<DAL.AdmissionSetup> admSetupList = null;
                //DAL.AdmissionSetup admSetup = null;
                DAL.Store store = null;
                string url = null;




                DAL.CandidateFormSl cf = null;
                using (var db = new CandidateDataManager())
                {
                    long cid = Convert.ToInt64(cPayment.CandidateID);
                    cf = db.GetCandidateFormSlByCandID_AD(cid);
                }

                int storePrimaryId = cf.AdmissionSetup.StoreID;
                using (var db = new CandidateDataManager())
                {
                    store = db.AdmissionDB.Stores.Where(x => x.ID == storePrimaryId && x.IsActive == true).FirstOrDefault();
                }

                if (store != null)
                {
                    string storeId = null;
                    string password = null;

                    try
                    {
                        storeId = Decrypt.DecryptString(store.StoreId);
                        password = Decrypt.DecryptString(store.StorePass);
                    }
                    catch (Exception)
                    {
                        lblMessage1.Text = "Error getting store information.";
                        lblMessage1.ForeColor = Color.Crimson;
                        return;
                    }

                    if (storeId != null && password != null)
                    {
                        //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
                        //    , val_id, plainText_storeId, plainText_password);

                        //OLD Style
                        //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?sessionkey={0}&store_id={1}&store_passwd={2}&v=3&format=json"
                        //    , cPayment.PaymentId.ToString(), storeId, password);


                        //NEW Style
                        url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?tran_id={0}&store_id={1}&store_passwd={2}"
                            , cPayment.PaymentId.ToString(), storeId, password);


                    }
                    else
                    {
                        lblMessage1.Text = "Error getting StoreId and Password.";
                        lblMessage1.ForeColor = Color.Crimson;
                        return;
                    }
                }
                else
                {
                    lblMessage1.Text = "Error: store not found.";
                    lblMessage1.ForeColor = Color.Crimson;
                    return;
                }

                #region N/A
                //if (!string.IsNullOrEmpty(url))
                //{
                //    var jsonData = new WebClient().DownloadString(url);


                //    dynamic outputArray = JsonConvert.DeserializeObject<dynamic>(jsonData);

                //    string paymentStatus = outputArray["status"].ToString();
                //    string paymentValue_a = outputArray["value_a"].ToString();  //get candidate id for validation
                //    string paymentAmount = outputArray["amount"].ToString();
                //    string paymentVal_id = outputArray["val_id"].ToString();

                //    //===========================================
                //    string paymentTran_id = outputArray["tran_id"].ToString();
                //    string paymentTran_date = outputArray["tran_date"].ToString();
                //    string paymentBank_tran_id = outputArray["bank_tran_id"].ToString();
                //    string paymentCard_type = outputArray["card_type"].ToString();
                //    string paymentValidated_on = outputArray["validated_on"].ToString();
                //    string paymentRisk_title = outputArray["risk_title"].ToString();

                //    //decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                //    string paymentStatusUpper = paymentStatus.ToUpper();



                //    if (
                //        (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED")) &&
                //        paymentTran_id.Equals(cPayment.PaymentId.ToString())
                //       )
                //    {

                //        //string paymentStatus = rss["element"][0]["status"].ToString();
                //        //string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
                //        //string paymentAmount = rss["element"][0]["amount"].ToString();
                //        string storeAmount = outputArray["store_amount"].ToString();
                //        //string paymentVal_id = outputArray["val_id"].ToString();
                //        //string paymentTran_id = rss["element"][0]["tran_id"].ToString();
                //        //string paymentTran_date = rss["element"][0]["tran_date"].ToString();
                //        //string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
                //        //string paymentCard_type = rss["element"][0]["card_type"].ToString();
                //        string cardNumber = outputArray["card_no"].ToString();
                //        string currencyType = outputArray["currency_type"].ToString();
                //        string currencyAmount = outputArray["currency_amount"].ToString();
                //        string cardBrand = outputArray["card_brand"].ToString();
                //        string cardIssuer = outputArray["card_issuer"].ToString();
                //        string cardIssuerCountry = outputArray["card_issuer_country"].ToString();
                //        string value_b = outputArray["value_b"].ToString();
                //        string value_c = outputArray["value_c"].ToString();
                //        string value_d = outputArray["value_d"].ToString();
                //        //string paymentValidated_on = outputArray["validated_on"].ToString();
                //        //string paymentRisk_title = outputArray["risk_title"].ToString();

                //        //decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                //        //string paymentStatusUpper = paymentStatus.ToUpper();

                //        lblAmount.Text = paymentAmount.ToString();
                //        lblBankTranId.Text = paymentBank_tran_id;
                //        lblCardNumber.Text = cardNumber;
                //        lblCardType.Text = paymentCard_type;
                //        lblCurrency.Text = currencyType;
                //        lblIssuerBankCountry.Text = cardIssuer + " " + cardIssuerCountry;
                //        lblReceivable.Text = storeAmount;
                //        //if (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED"))
                //        //{
                //        //    lblStatus.Text = paymentStatusUpper;
                //        //    lblStatus.ForeColor = Color.Green;
                //        //    btnCopy.Visible = true;
                //        //}
                //        //else
                //        //{
                //        //    lblStatus.Text = paymentStatusUpper;
                //        //    lblStatus.ForeColor = Color.Crimson;
                //        //    btnCopy.Visible = false;
                //        //}
                //        lblTranId.Text = paymentTran_id;
                //        lblTransDate.Text = paymentTran_date;


                //        lblStatus.Text = paymentStatusUpper;
                //        lblStatus.ForeColor = Color.Green;
                //        btnCopy.Visible = true;

                //    }
                //    else
                //    {

                //        lblStatus.Text = paymentStatusUpper;
                //        lblStatus.ForeColor = Color.Crimson;
                //        btnCopy.Visible = false;

                //    }

                //}
                //else
                //{

                //    lblSearchMsg.Text = "SSL Validate URL is Null";
                //    lblSearchMsg.ForeColor = Color.Crimson;
                //    btnSave.Visible = false;

                //} 
                #endregion


                #region SSL Payment Validation Check
                if (url != null)
                {
                    HttpClient client = new HttpClient();

                    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                            delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                            {
                                return true;
                            };

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    var contentString = result.Content.ReadAsStringAsync();

                    JObject rss = JObject.Parse(contentString.Result);

                    //OLD Style
                    //var jsonData = new WebClient().DownloadString(url);
                    //JObject rss = JObject.Parse(jsonData);

                    string paymentIDFromGateway = (string)rss["element"][0]["tran_id"];

                    string paymentStatus1 = rss["element"][0]["status"].ToString();

                    if (paymentStatus1.ToUpper().Equals("VALID") || paymentStatus1.ToUpper().Equals("VALIDATED"))
                    {
                        if (paymentIDFromGateway.Equals(cPayment.PaymentId.ToString()))
                        {


                            #region Response Data from SSL
                            string paymentStatus = rss["element"][0]["status"].ToString();
                            string paymentValue_a = rss["element"][0]["value_a"].ToString();
                            string paymentAmount = rss["element"][0]["amount"].ToString();
                            string storeAmount = rss["element"][0]["store_amount"].ToString();
                            //string paymentVal_id = outputArray["val_id"].ToString();
                            string paymentTran_id = rss["element"][0]["tran_id"].ToString();
                            string paymentTran_date = rss["element"][0]["tran_date"].ToString();
                            string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
                            string paymentCard_type = rss["element"][0]["card_type"].ToString();
                            string cardNumber = rss["element"][0]["card_no"].ToString();
                            string currencyType = rss["element"][0]["currency_type"].ToString();
                            string currencyAmount = rss["element"][0]["currency_amount"].ToString();
                            string cardBrand = rss["element"][0]["card_brand"].ToString();
                            string cardIssuer = rss["element"][0]["card_issuer"].ToString();
                            string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
                            string value_b = rss["element"][0]["value_b"].ToString();
                            string value_c = rss["element"][0]["value_c"].ToString();
                            string value_d = rss["element"][0]["value_d"].ToString();
                            //string paymentValidated_on = outputArray["validated_on"].ToString();
                            //string paymentRisk_title = outputArray["risk_title"].ToString();

                            //decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                            string paymentStatusUpper = paymentStatus.ToUpper();

                            lblAmount.Text = paymentAmount.ToString();
                            lblBankTranId.Text = paymentBank_tran_id;
                            lblCardNumber.Text = cardNumber;
                            lblCardType.Text = paymentCard_type;
                            lblCurrency.Text = currencyType;
                            lblIssuerBankCountry.Text = cardIssuer + " " + cardIssuerCountry;
                            lblReceivable.Text = storeAmount;
                            if (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED"))
                            {
                                lblStatus.Text = paymentStatusUpper;
                                lblStatus.ForeColor = Color.Green;
                                //btnCopy.Visible = true;

                                btnSave.Visible = true;
                            }
                            else
                            {
                                lblStatus.Text = paymentStatusUpper;
                                lblStatus.ForeColor = Color.Crimson;
                                //btnCopy.Visible = false;
                            }
                            lblTranId.Text = paymentTran_id;
                            lblTransDate.Text = paymentTran_date;
                            #endregion

                        }
                        else
                        {
                            lblMessage1.Text = "Candidate PaymentID from system does not match with Gateway PaymentID(1).";
                            lblMessage1.ForeColor = Color.Crimson;
                            return;
                        }
                    }
                    else //if transaction is not valid
                    {

                        //btnSave.Visible = false;

                        //string currency = rss["element"][0]["currency"].ToString();
                        //string val_id = rss["element"][0]["val_id"].ToString();
                        //string status = rss["element"][0]["status"].ToString();
                        //string validated_on = rss["element"][0]["validated_on"].ToString();
                        string currency_type = rss["element"][0]["currency_type"].ToString();
                        //string currency_amount = rss["element"][0]["currency_amount"].ToString();
                        //string currency_rate = rss["element"][0]["currency_rate"].ToString();
                        //string base_fair = rss["element"][0]["base_fair"].ToString();
                        //string value_a = rss["element"][0]["value_a"].ToString();
                        string value_b = rss["element"][0]["value_b"].ToString();
                        //string value_c = rss["element"][0]["value_c"].ToString();
                        //string value_d = rss["element"][0]["value_d"].ToString();
                        //string discount_percentage = rss["element"][0]["discount_percentage"].ToString();
                        //string currencyType = rss["element"][0]["currency_type"].ToString();
                        //string currencyAmount = rss["element"][0]["currency_amount"].ToString();
                        //string cardBrand = rss["element"][0]["card_brand"].ToString();
                        //string cardIssuer = rss["element"][0]["card_issuer"].ToString();
                        //string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();

                        string amount = rss["element"][0]["amount"].ToString();

                        string error = rss["element"][0]["error"].ToString();

                        lblTranId.Text = paymentIDFromGateway;

                        lblCandidateName.Text = value_b;

                        lblStatus.Text = error;
                        lblStatus.ForeColor = Color.Crimson;

                        lblAmount.Text = amount;

                        lblCurrency.Text = currency_type;

                        lblTransDate.Text = string.Empty;
                        lblBankTranId.Text = string.Empty;
                        lblValId.Text = string.Empty;

                        lblCardNumber.Text = string.Empty;
                        lblCardType.Text = string.Empty;

                        lblIssuerBankCountry.Text = string.Empty;
                        lblReceivable.Text = string.Empty;


                        #region N/A
                        //lblAmount.Text = null;
                        //lblBankTranId.Text = null;
                        //lblCardNumber.Text = null;
                        //lblCardType.Text = null;
                        //lblCurrency.Text = null;
                        //lblIssuerBankCountry.Text = null;
                        //lblReceivable.Text = null;
                        ////if (paymentStatus1.Equals("VALID") || paymentStatus1.Equals("VALIDATED"))
                        ////{
                        ////    lblStatus.Text = paymentStatus1.ToUpper();
                        ////    lblStatus.ForeColor = Color.Green;
                        ////    btnCopy.Visible = true;
                        ////}
                        ////else
                        ////{
                        ////    lblStatus.Text = paymentStatus1.ToUpper();
                        ////    lblStatus.ForeColor = Color.Crimson;
                        ////    btnCopy.Visible = false;
                        ////}
                        //lblTranId.Text = paymentIDFromGateway;
                        //lblTransDate.Text = null; 
                        #endregion

                        lblMessage1.Text = error;
                        lblMessage1.ForeColor = Color.Crimson;
                        return;
                    }

                }//end if (url != null) 
                #endregion








                #region N/A

                ////============================================
                ////============================================
                ////1. Get form serial objects.
                //try
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            cFormSlList = db.AdmissionDB.CandidateFormSls
                //                .Where(c => c.CandidatePaymentID == cPayment.ID).ToList();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        lblMessage1.Text = "Error getting candidate form serial information.";
                //        lblMessage1.ForeColor = Color.Crimson;
                //        return;
                //    }



                ////2. Figure out what the number of form serials indicates.
                //if (cFormSlList.Count() > 1) // more than one candidate form serial means multiple form purchase. hence this is undergraduate candidate.
                //{
                //    bool isMultiple = true;

                //    // get admission setup for each form serial object.
                //    foreach (var item in cFormSlList)
                //    {
                //        DAL.AdmissionSetup _temp = null;
                //        try
                //        {
                //            using (var db = new CandidateDataManager())
                //            {
                //                _temp = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                //            }
                //            if (_temp.EducationCategoryID == 4) // only get the undergraduate admission setup.
                //            {
                //                admSetupList.Add(_temp);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage1.Text = "Error getting admission setup.";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }
                //    }

                //    if (admSetupList.Count() > 0)
                //    {
                //        foreach (var item in admSetupList)
                //        {
                //            if (item.EducationCategoryID == 6) // if graduate detected.
                //            {
                //                isMultiple = false;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        lblMessage1.Text = "Error: admission setup not found.";
                //        lblMessage1.ForeColor = Color.Crimson;
                //        return;
                //    }

                //    if (isMultiple == true)
                //    {
                //        try
                //        {
                //            using (var db = new OfficeDataManager())
                //            {
                //                store = db.AdmissionDB.Stores.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage1.Text = "Error getting store.";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }
                //    }

                //    if (store != null)
                //    {
                //        string storeId = null;
                //        string password = null;

                //        try
                //        {
                //            storeId = Decrypt.DecryptString(store.StoreId);
                //            password = Decrypt.DecryptString(store.StorePass);
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage1.Text = "Error getting store information.";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }

                //        if (storeId != null && password != null)
                //        {
                //            //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
                //            //    , val_id, plainText_storeId, plainText_password);

                //            url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?tran_id={0}&store_id={1}&store_passwd={2}&v=3&format=json"
                //                , cPayment.PaymentId.ToString(), storeId, password);
                //        }
                //        else
                //        {
                //            lblMessage1.Text = "Error getting StoreId and Password.";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        lblMessage1.Text = "Error: store not found.";
                //        lblMessage1.ForeColor = Color.Crimson;
                //        return;
                //    }

                //    if (url != null)
                //    {
                //        var jsonData = new WebClient().DownloadString(url);

                //        JObject rss = JObject.Parse(jsonData);

                //        string paymentIDFromGateway = (string)rss["element"][0]["tran_id"];

                //        string paymentStatus1 = rss["element"][0]["status"].ToString();

                //        if (paymentStatus1.ToUpper().Equals("VALID") || paymentStatus1.ToUpper().Equals("VALIDATED")) //if transaction is valid or validated
                //        {
                //            if (paymentIDFromGateway.Equals(cPayment.PaymentId.ToString())) //if payment id matches
                //            {

                //                #region
                //                string paymentStatus = rss["element"][0]["status"].ToString();
                //                string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
                //                string paymentAmount = rss["element"][0]["amount"].ToString();
                //                string storeAmount = rss["element"][0]["store_amount"].ToString();
                //                //string paymentVal_id = outputArray["val_id"].ToString();
                //                string paymentTran_id = rss["element"][0]["tran_id"].ToString();
                //                string paymentTran_date = rss["element"][0]["tran_date"].ToString();
                //                string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
                //                string paymentCard_type = rss["element"][0]["card_type"].ToString();
                //                string cardNumber = rss["element"][0]["card_no"].ToString();
                //                string currencyType = rss["element"][0]["currency_type"].ToString();
                //                string currencyAmount = rss["element"][0]["currency_amount"].ToString();
                //                string cardBrand = rss["element"][0]["card_brand"].ToString();
                //                string cardIssuer = rss["element"][0]["card_issuer"].ToString();
                //                string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
                //                string value_b = rss["element"][0]["value_b"].ToString();
                //                string value_c = rss["element"][0]["value_c"].ToString();
                //                string value_d = rss["element"][0]["value_d"].ToString();
                //                //string paymentValidated_on = outputArray["validated_on"].ToString();
                //                //string paymentRisk_title = outputArray["risk_title"].ToString();

                //                decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                //                string paymentStatusUpper = paymentStatus.ToUpper();

                //                lblAmount.Text = validateResultAmount.ToString();
                //                lblBankTranId.Text = paymentBank_tran_id;
                //                lblCardNumber.Text = cardNumber;
                //                lblCardType.Text = paymentCard_type;
                //                lblCurrency.Text = currencyType;
                //                lblIssuerBankCountry.Text = cardIssuer + " " + cardIssuerCountry;
                //                lblReceivable.Text = storeAmount;
                //                if (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED"))
                //                {
                //                    lblStatus.Text = paymentStatusUpper;
                //                    lblStatus.ForeColor = Color.Green;
                //                    btnCopy.Visible = true;
                //                }
                //                else
                //                {
                //                    lblStatus.Text = paymentStatusUpper;
                //                    lblStatus.ForeColor = Color.Crimson;
                //                    btnCopy.Visible = false;
                //                }
                //                lblTranId.Text = paymentTran_id;
                //                lblTransDate.Text = paymentTran_date;
                //                #endregion

                //            }
                //            else
                //            {
                //                lblMessage1.Text = "Candidate PaymentID from system does not match with Gateway PaymentID(1).";
                //                lblMessage1.ForeColor = Color.Crimson;
                //                return;
                //            }
                //        }
                //        else //if transaction is not valid
                //        {
                //            lblAmount.Text = null;
                //            lblBankTranId.Text = null;
                //            lblCardNumber.Text = null;
                //            lblCardType.Text = null;
                //            lblCurrency.Text = null;
                //            lblIssuerBankCountry.Text = null;
                //            lblReceivable.Text = null;
                //            if (paymentStatus1.Equals("VALID") || paymentStatus1.Equals("VALIDATED"))
                //            {
                //                lblStatus.Text = paymentStatus1.ToUpper();
                //                lblStatus.ForeColor = Color.Green;
                //                btnCopy.Visible = true;
                //            }
                //            else
                //            {
                //                lblStatus.Text = paymentStatus1.ToUpper();
                //                lblStatus.ForeColor = Color.Crimson;
                //                btnCopy.Visible = false;
                //            }
                //            lblTranId.Text = paymentIDFromGateway;
                //            lblTransDate.Text = null;
                //        }

                //    }//end if (url != null)


                //}
                //else if (cFormSlList.Count() == 1) // only one candidate form serial means either multiple or single form purchase. This could be either graduate or undergraduate.
                //{

                //    // get admission setup for each form serial object.
                //    foreach (var item in cFormSlList)
                //    {
                //        try
                //        {
                //            using (var db = new CandidateDataManager())
                //            {
                //                admSetup = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage1.Text = "Error getting admission setup(1).";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }
                //    }

                //    if (admSetup != null)
                //    {
                //        try
                //        {
                //            using (var db = new OfficeDataManager())
                //            {
                //                store = db.AdmissionDB.Stores.Find(admSetup.StoreID);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage1.Text = "Error getting store(1).";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        lblMessage1.Text = "Error: admission setup not found(1).";
                //        lblMessage1.ForeColor = Color.Crimson;
                //        return;
                //    }


                //    if (store != null)
                //    {
                //        string storeId = null;
                //        string password = null;

                //        try
                //        {
                //            storeId = Decrypt.DecryptString(store.StoreId);
                //            password = Decrypt.DecryptString(store.StorePass);
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage1.Text = "Error getting store information(1).";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }

                //        if (storeId != null && password != null)
                //        {
                //            //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
                //            //    , val_id, plainText_storeId, plainText_password);

                //            url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?tran_id={0}&store_id={1}&store_passwd={2}&v=3&format=json"
                //                , cPayment.PaymentId.ToString(), storeId, password);
                //        }
                //        else
                //        {
                //            lblMessage1.Text = "Error getting StoreId and Password(1).";
                //            lblMessage1.ForeColor = Color.Crimson;
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        lblMessage1.Text = "Error: store not found(1).";
                //        lblMessage1.ForeColor = Color.Crimson;
                //        return;
                //    }

                //    if (url != null)
                //    {
                //        var jsonData = new WebClient().DownloadString(url);

                //        JObject rss = JObject.Parse(jsonData);

                //        string paymentIDFromGateway = (string)rss["element"][0]["tran_id"];
                //        string paymentStatus1 = rss["element"][0]["status"].ToString();

                //        if (paymentStatus1.Equals("VALID") || paymentStatus1.Equals("VALIDATED")) //if transation is valid or validated
                //        {
                //            if (paymentIDFromGateway.Equals(cPayment.PaymentId.ToString())) //if payment id matches
                //            {

                //                #region
                //                string paymentStatus = rss["element"][0]["status"].ToString();
                //                string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
                //                string paymentAmount = rss["element"][0]["amount"].ToString();
                //                string storeAmount = rss["element"][0]["store_amount"].ToString();
                //                //string paymentVal_id = outputArray["val_id"].ToString();
                //                string paymentTran_id = rss["element"][0]["tran_id"].ToString();
                //                string paymentTran_date = rss["element"][0]["tran_date"].ToString();
                //                string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
                //                string paymentCard_type = rss["element"][0]["card_type"].ToString();
                //                string cardNumber = rss["element"][0]["card_no"].ToString();
                //                string currencyType = rss["element"][0]["currency_type"].ToString();
                //                string currencyAmount = rss["element"][0]["currency_amount"].ToString();
                //                string cardBrand = rss["element"][0]["card_brand"].ToString();
                //                string cardIssuer = rss["element"][0]["card_issuer"].ToString();
                //                string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
                //                string value_b = rss["element"][0]["value_b"].ToString();
                //                string value_c = rss["element"][0]["value_c"].ToString();
                //                string value_d = rss["element"][0]["value_d"].ToString();
                //                //string paymentValidated_on = outputArray["validated_on"].ToString();
                //                //string paymentRisk_title = outputArray["risk_title"].ToString();

                //                decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                //                string paymentStatusUpper = paymentStatus.ToUpper();

                //                lblAmount.Text = validateResultAmount.ToString();
                //                lblBankTranId.Text = paymentBank_tran_id;
                //                lblCardNumber.Text = cardNumber;
                //                lblCardType.Text = paymentCard_type;
                //                lblCurrency.Text = currencyType;
                //                lblIssuerBankCountry.Text = cardIssuer + " " + cardIssuerCountry;
                //                lblReceivable.Text = storeAmount;
                //                if (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED"))
                //                {
                //                    lblStatus.Text = paymentStatusUpper;
                //                    lblStatus.ForeColor = Color.Green;
                //                    btnCopy.Visible = true;
                //                }
                //                else
                //                {
                //                    lblStatus.Text = paymentStatusUpper;
                //                    lblStatus.ForeColor = Color.Crimson;
                //                    btnCopy.Visible = false;
                //                }
                //                lblTranId.Text = paymentTran_id;
                //                lblTransDate.Text = paymentTran_date;
                //                #endregion

                //            }
                //            else
                //            {
                //                lblMessage1.Text = "Candidate PaymentID from system does not match with Gateway PaymentID(1).";
                //                lblMessage1.ForeColor = Color.Crimson;
                //                return;
                //            }
                //        }
                //        else //if transaction is invalid
                //        {
                //            lblAmount.Text = null;
                //            lblBankTranId.Text = null;
                //            lblCardNumber.Text = null;
                //            lblCardType.Text = null;
                //            lblCurrency.Text = null;
                //            lblIssuerBankCountry.Text = null;
                //            lblReceivable.Text = null;
                //            if (paymentStatus1.Equals("VALID") || paymentStatus1.Equals("VALIDATED"))
                //            {
                //                lblStatus.Text = paymentStatus1.ToUpper();
                //                lblStatus.ForeColor = Color.Green;
                //                btnCopy.Visible = true;
                //            }
                //            else
                //            {
                //                lblStatus.Text = paymentStatus1.ToUpper();
                //                lblStatus.ForeColor = Color.Crimson;
                //                btnCopy.Visible = false;
                //            }
                //            lblTranId.Text = paymentIDFromGateway;
                //            lblTransDate.Text = null;
                //        }

                //    }//end if (url != null)


                //}
                //else // candidate serial not available. could be an error.
                //{
                //    lblMessage.Text = "Candidate Form serial not found.";
                //    lblMessage.ForeColor = Color.Crimson;
                //    return;
                //} 
                #endregion
            }
            catch (Exception ex)
            {
                lblSearchMsg.Text = "Error: Something went wrong! Exception: " + ex.Message.ToString();
                lblSearchMsg.ForeColor = Color.Crimson;
                btnSave.Visible = false;
            }
        }

        //private static IEnumerable<JToken> AllChildren(JToken json)
        //{
        //    foreach (var c in json.Children())
        //    {
        //        yield return c;
        //        foreach (var cc in AllChildren(c))
        //        {
        //            yield return cc;
        //        }
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long paymentId = -1;
            if (!string.IsNullOrEmpty(txtPaymentId.Text.Trim()))
            {
                paymentId = Int64.Parse(txtPaymentId.Text);
            }

            DAL.CandidatePayment cPaymentObj = null;
            using (var db = new CandidateDataManager())
            {
                cPaymentObj = db.AdmissionDB.CandidatePayments
                        .Where(c => c.PaymentId == paymentId && c.IsPaid == false)
                        .FirstOrDefault();
            }

            DAL.BasicInfo candidate = null;
            using (var db = new CandidateDataManager())
            {
                candidate = db.AdmissionDB.BasicInfoes.Find(cPaymentObj.CandidateID);
            }

            if (cPaymentObj != null && candidate != null)
            {

                DAL.TransactionHistory thExist = null;
                using (var db = new OfficeDataManager())
                {
                    thExist = db.AdmissionDB.TransactionHistories
                            .Where(c => c.TransactionID == txtPaymentId.Text.Trim() && c.CandidateID == cPaymentObj.CandidateID.ToString())
                            .FirstOrDefault();
                }

                if (thExist == null)
                {
                    DAL.TransactionHistory th = new DAL.TransactionHistory();
                    th.CandidateID = cPaymentObj.CandidateID.ToString();

                    if (string.IsNullOrEmpty(txtStatus.Text))
                    {
                        th.Status = null;
                    }
                    else
                    {
                        th.Status = txtStatus.Text;
                    }

                    if (string.IsNullOrEmpty(txtTransactionDate.Text))
                    {
                        th.TransactionDate = null;
                    }
                    else
                    {
                        th.TransactionDate = txtTransactionDate.Text;
                    }

                    if (string.IsNullOrEmpty(txtAmount.Text))
                    {
                        th.Amount = null;
                    }
                    else
                    {
                        th.Amount = txtAmount.Text;
                    }

                    if (string.IsNullOrEmpty(txtBankTransactionId.Text))
                    {
                        th.BankTransactionID = null;
                    }
                    else
                    {
                        th.BankTransactionID = txtBankTransactionId.Text;
                    }

                    if (string.IsNullOrEmpty(txtCurrency.Text))
                    {
                        th.Currency = null;
                    }
                    else
                    {
                        th.Currency = txtCurrency.Text;
                    }

                    if (string.IsNullOrEmpty(txtPaymentId.Text))
                    {
                        th.TransactionID = null;
                    }
                    else
                    {
                        th.TransactionID = txtPaymentId.Text;
                    }

                    if (string.IsNullOrEmpty(txtValidationId.Text))
                    {
                        th.ValidationID = null;
                    }
                    else
                    {
                        th.ValidationID = txtValidationId.Text;
                    }

                    if (string.IsNullOrEmpty(txtStoreAmount.Text))
                    {
                        th.StoreAmount = null;
                    }
                    else
                    {
                        th.StoreAmount = txtStoreAmount.Text;
                    }

                    if (string.IsNullOrEmpty(txtCardType.Text))
                    {
                        th.CardType = null;
                    }
                    else
                    {
                        th.CardType = txtCardType.Text;
                    }

                    if (string.IsNullOrEmpty(txtCardNumber.Text))
                    {
                        th.CardNumber = null;
                    }
                    else
                    {
                        th.CardNumber = txtCardNumber.Text;
                    }

                    if (string.IsNullOrEmpty(txtIssuerBankCountry.Text))
                    {
                        th.CardIssuer = null;
                        th.CardIssuerCountry = null;
                    }
                    else
                    {
                        th.CardIssuer = txtIssuerBankCountry.Text;
                        th.CardIssuerCountry = txtIssuerBankCountry.Text;
                    }

                    th.ValueA = candidate.ID.ToString();
                    th.ValueB = candidate.FirstName;
                    th.ValueC = cPaymentObj.Amount.ToString();

                    th.IsManualInsert = true;
                    th.ManualInsertBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                    th.DateManualInsert = DateTime.Now;

                    long transactionIdLong = -1;

                    using (var db = new OfficeDataManager())
                    {
                        db.Insert<DAL.TransactionHistory>(th);
                        transactionIdLong = th.ID;
                    }

                    if (transactionIdLong > 0) //update candidate payment.
                    {
                        DAL.CandidatePayment updCPayment = null;
                        using (var db = new CandidateDataManager())
                        {
                            updCPayment = db.AdmissionDB.CandidatePayments
                                .Where(c => c.PaymentId == paymentId)
                                .FirstOrDefault();
                        }
                        if (updCPayment != null && updCPayment.IsPaid == false)
                        {
                            updCPayment.IsPaid = true;
                            updCPayment.DateModified = DateTime.Now;
                            updCPayment.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                            using (var db = new CandidateDataManager())
                            {
                                db.Update<DAL.CandidatePayment>(updCPayment);
                            }

                            GetSendingInfo(updCPayment.CandidateID);
                        }
                    }

                    lblSearchMsg.Text = string.Empty;
                    ClearFields();

                    lblMessage.Text = "Transaction and Candidate Payment Updated.";
                    lblMessage.ForeColor = Color.Green;
                    panel_Message.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Transaction already exist.";
                    lblMessage.ForeColor = Color.Crimson;
                    panel_Message.Visible = true;

                    ClearFields();
                }//end if-else (thExist == null)
            }

        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            txtAmount.Text = lblAmount.Text;
            txtBankTransactionId.Text = lblBankTranId.Text;
            txtCardNumber.Text = lblCardNumber.Text;
            txtCardType.Text = lblCardType.Text;
            txtCurrency.Text = lblCurrency.Text;
            txtIssuerBankCountry.Text = lblIssuerBankCountry.Text;
            txtStoreAmount.Text = lblReceivable.Text;
            txtTransactionDate.Text = lblTransDate.Text;
        }

        /// <summary>
        /// Button action to load bkash transaction details from gateway
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLoadB_Click(object sender, EventArgs e)
        {
            string trxId = null;
            trxId = txtTrxIdB.Text.Trim();

            string user = "BUPTWO";
            string pass = "BDunipro02@2";
            string msisdn = "01769028780";

            if (!string.IsNullOrEmpty(trxId))
            {
                string url = null;

                url = String.Format(@"https://www.bkashcluster.com:9081/dreamwave/merchant/trxcheck/sendmsg?user={0}&pass={1}&msisdn={2}&trxid={3}"
                            , user, pass, msisdn, trxId);

                if (!string.IsNullOrEmpty(url))
                {
                    var xmlData = new WebClient().DownloadString(url);

                    //var xdoc = XDocument.Parse(xml);
                    //var items = xdoc.Descendants("Item")
                    //                .ToDictionary(i => (string)i.Attribute("Key"),
                    //                              i => (string)i.Attribute("Value"));


                    var xDoc = XDocument.Parse(xmlData);

                    string trxStatus = xDoc.Descendants().First(node => node.Name == "trxStatus").Value.ToString();

                    //string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
                    //string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
                    //string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
                    //string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
                    //string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
                    //string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
                    //string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
                    //string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
                    //string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();


                    #region trxStatus.Equals("0000") (SUCCESSFUL)
                    if (trxStatus.Equals("0000"))//successfull
                    {
                        string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
                        string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
                        string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
                        string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
                        string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
                        string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
                        string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
                        string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
                        string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();

                        lblTrxIdB.Text = trx_Id;
                        lblReferenceNoB.Text = reference;
                        lblCounterB.Text = counter;
                        lblPaidAmntB.Text = amount;
                        lblSenderB.Text = sender_1;
                        lblServiceB.Text = service;
                        lblCurrencyB.Text = currency;
                        lblReceiverB.Text = receiver;

                        txtTrxIdB_1.Text = trx_Id;
                        txtPaymentIdB.Text = reference;
                        txtCounterB.Text = counter;
                        txtPaidAmountB.Text = amount;
                        txtSenderB.Text = sender_1;
                        txtServiceB.Text = service;
                        txtCurrency.Text = currency;
                        txtReceiverB.Text = receiver;
                        txtTrxTimestampB.Text = trxTimestamp;
                        txtCurrencyB.Text = currency;

                        #region GetBkashStatusMessageFor"0000"

                        DAL.BkashTrxStatusCode btsCodeSucces = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                btsCodeSucces = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
                            }
                        }
                        catch (Exception)
                        {
                            lblMessageB.Text = "Error getting bKash Status Message.";
                            panelMessageB.CssClass = "alert alert-danger";
                            panelMessageB.Visible = true;
                            return;
                        }

                        if (btsCodeSucces != null)
                        {
                            lblStatusCodeB.Text = trxStatus;
                            lblStatusCodeB.ForeColor = Color.Green;
                            lblStatusDetailsB.Text = btsCodeSucces.Message + "; " + btsCodeSucces.Interpretation;
                            lblStatusDetailsB.ForeColor = Color.Green;

                            txtStatusCodeB.Text = trxStatus;
                            txtStatusDetailsB.Text = btsCodeSucces.Message + "; " + btsCodeSucces.Interpretation;
                        }
                        else
                        {
                            lblStatusCodeB.Text = trxStatus;
                            lblStatusDetailsB.Text = "Unknown";
                            lblStatusDetailsB.ForeColor = Color.Crimson;

                            txtStatusCodeB.Text = trxStatus;
                            txtStatusDetailsB.Text = "Unknown";
                        }

                        #endregion

                        DAL.CandidatePayment cPayment = null;
                        #region GetCandidatePayment
                        long paymentId = Convert.ToInt64(reference);
                        if (paymentId > 0)
                        {
                            try
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    cPayment = db.AdmissionDB.CandidatePayments.Where(c => c.PaymentId == paymentId).FirstOrDefault();
                                }
                            }
                            catch (Exception ex)
                            {
                                lblMessageB.Text = "Error getting candidate payment information. " + ex.Message + "; " + ex.InnerException.Message;
                                panelMessageB.CssClass = "alert alert-danger";
                                panelMessageB.Visible = true;
                                return;
                            }
                        }
                        #endregion

                        if (cPayment != null)
                        {

                            #region GetBasicInfo
                            DAL.BasicInfo candidate = null;
                            try
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
                                }
                            }
                            catch (Exception)
                            {
                                lblMessageB.Text = "Error getting candidate information.";
                                panelMessageB.CssClass = "alert alert-danger";
                                panelMessageB.Visible = true;
                                return;
                            }

                            if (candidate != null)
                            {
                                lblNameB.Text = candidate.FirstName;
                                txtCandidateNameB.Text = candidate.FirstName;
                            }
                            else
                            {
                                lblNameB.Text = "Unknown";
                                lblNameB.ForeColor = Color.Crimson;
                            }

                            #endregion

                            int amountToBePaid = -1;
                            double amount_1 = -0.0;
                            //string paymentIdStr = null;

                            amount_1 = Convert.ToDouble(cPayment.Amount) + (Convert.ToDouble(cPayment.Amount) * (1.5 / 100));
                            amountToBePaid = Convert.ToInt32(Math.Ceiling(amount_1));

                            int amountFromGateway = (int)Convert.ToDouble(amount);
                            long referenceFromGateway = Int64.Parse(reference);

                            lblSysAmntB.Text = cPayment.Amount.ToString();
                            txtSystemAmountB.Text = cPayment.Amount.ToString();
                            if (Convert.ToDouble(amount) <= Convert.ToDouble(cPayment.Amount))
                            {
                                lblPaidAmntB.ForeColor = Color.Crimson;
                                txtPaidAmountB.ForeColor = Color.Crimson;
                            }
                        }
                        else
                        {
                            lblSysAmntB.Text = "Unknown";
                            lblSysAmntB.ForeColor = Color.Crimson;

                            txtSystemAmountB.Text = "Unknown";
                            txtSystemAmountB.ForeColor = Color.Crimson;
                        }
                        btnSaveB.Visible = true;

                    } //end if(trxStatus.Equals("0000"))
                    #endregion
                    //-----------------------------------
                    #region if(trxStatus != "0000")
                    else
                    {
                        string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();

                        DAL.BkashTrxStatusCode btsCode = null;
                        try
                        {
                            if (!trxStatus.Equals("0000"))
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    btsCode = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
                                }
                            }
                        }
                        catch (Exception)
                        {
                            lblMessageB.Text = "Error getting bKash Transaction Status Codes.";
                            panelMessageB.CssClass = "alert alert-danger";
                            panelMessageB.Visible = true;
                            return;
                        }

                        if (btsCode != null)
                        {
                            lblStatusCodeB.Text = trxStatus;
                            lblStatusDetailsB.Text = btsCode.Message + "; " + btsCode.Interpretation;
                            lblStatusDetailsB.ForeColor = Color.Crimson;

                            txtStatusCodeB.Text = trxStatus;
                            txtStatusDetailsB.Text = btsCode.Message + "; " + btsCode.Interpretation;
                        }
                        else
                        {
                            lblStatusCodeB.Text = trxStatus;
                            lblStatusDetailsB.Text = "Unknown";
                            lblStatusDetailsB.ForeColor = Color.Crimson;

                            txtStatusCodeB.Text = trxStatus;
                            txtStatusDetailsB.Text = "Unknown";
                        }

                        lblTrxIdB.Text = trx_Id;
                        lblReferenceNoB.Text = string.Empty;
                        lblCounterB.Text = string.Empty;
                        lblPaidAmntB.Text = string.Empty;
                        lblSenderB.Text = string.Empty;
                        lblServiceB.Text = string.Empty;
                        lblCurrencyB.Text = string.Empty;
                        lblReceiverB.Text = string.Empty;
                        lblNameB.Text = string.Empty;
                        lblSysAmntB.Text = string.Empty;

                        txtTrxIdB_1.Text = trx_Id;
                        txtPaymentIdB.Text = string.Empty;
                        txtCounterB.Text = string.Empty;
                        txtPaidAmountB.Text = string.Empty;
                        txtSenderB.Text = string.Empty;
                        txtServiceB.Text = string.Empty;
                        txtCurrency.Text = string.Empty;
                        txtReceiverB.Text = string.Empty;
                        txtTrxTimestampB.Text = string.Empty;
                    }
                    #endregion

                }
            }
            else
            {
                lblMessageB.Text = "Please provide your Trx ID.";
                panelMessageB.CssClass = "alert alert-danger";
                panelMessageB.Visible = true;
            }

            txtTrxIdB.Focus();
        }

        /// <summary>
        /// Button action to save Bkash Transaction History
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveB_Click(object sender, EventArgs e)
        {

            #region txtStatusCodeB.Equals("0000") (SUCCESSFUL)
            if (txtStatusCodeB.Text.Equals("0000"))//successfull
            {


                //#region GetBkashStatusMessageFor"0000"

                //DAL.BkashTrxStatusCode btsCodeSucces = null;
                //try
                //{
                //    using (var db = new OfficeDataManager())
                //    {
                //        btsCodeSucces = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    lblMessageB.Text = "Error getting bKash Status Message. " + ex.Message + "; " + ex.InnerException.Message;
                //    panelMessageB.CssClass = "alert alert-danger";
                //    panelMessageB.Visible = true;
                //    return;
                //}

                //if (btsCodeSucces != null)
                //{
                //    lblStatusCodeB.Text = trxStatus;
                //    lblStatusCodeB.ForeColor = Color.Green;
                //    lblStatusDetailsB.Text = btsCodeSucces.Message + "; " + btsCodeSucces.Interpretation;
                //    lblStatusDetailsB.ForeColor = Color.Green;
                //}
                //else
                //{
                //    lblStatusCodeB.Text = trxStatus;
                //    lblStatusDetailsB.Text = "Unknown";
                //    lblStatusDetailsB.ForeColor = Color.Crimson;
                //}

                //#endregion

                DAL.CandidatePayment cPayment = null;
                #region GetCandidatePayment
                long paymentId = Convert.ToInt64(txtPaymentIdB.Text);
                if (paymentId > 0)
                {
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cPayment = db.AdmissionDB.CandidatePayments.Where(c => c.PaymentId == paymentId).FirstOrDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessageB.Text = "Error getting candidate payment information. " + ex.Message + "; " + ex.InnerException.Message;
                        panelMessageB.CssClass = "alert alert-danger";
                        panelMessageB.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblMessageB.Text = "Error getting reference/paymentId from bKash.";
                    panelMessageB.CssClass = "alert alert-danger";
                    panelMessageB.Visible = true;
                    return;
                }
                #endregion

                if (cPayment != null)
                {
                    #region GetBasicInfo
                    DAL.BasicInfo candidate = null;
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessageB.Text = "Error getting candidate information. " + ex.Message + "; " + ex.InnerException.Message;
                        panelMessageB.CssClass = "alert alert-danger";
                        panelMessageB.Visible = true;
                        return;
                    }

                    if (candidate != null)
                    {
                        lblNameB.Text = candidate.FirstName;
                    }
                    else
                    {
                        lblNameB.Text = "Unknown";
                        lblNameB.ForeColor = Color.Crimson;
                    }
                    #endregion

                    int amountToBePaid = -1;
                    double amount_1 = -0.0;
                    //string paymentIdStr = null;

                    amount_1 = Convert.ToDouble(cPayment.Amount) + (Convert.ToDouble(cPayment.Amount) * (1.5 / 100));
                    amountToBePaid = Convert.ToInt32(Math.Ceiling(amount_1));

                    int amountFromGateway = (int)Convert.ToDouble(txtPaidAmountB.Text);
                    long referenceFromGateway = Int64.Parse(txtPaymentIdB.Text);

                    if (referenceFromGateway == cPayment.PaymentId)//for admin no need to check whether less paid amount.
                    {
                        #region GetExistingTransactionBkash & GetExistingTransactionHistory
                        DAL.TransactionHistoryBKash existingTrx = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                existingTrx = db.AdmissionDB.TransactionHistoryBKashes
                                    .Where(c => c.CandidateID == cPayment.CandidateID &&
                                        c.ReferencePaymentID == cPayment.PaymentId.ToString() &&
                                        c.TrxID == txtTrxIdB_1.Text &&
                                        c.TrxStatus == txtStatus.Text).FirstOrDefault();
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessageB.Text = "Error getting existing bKash transaction. " + ex.Message + "; " + ex.InnerException.Message;
                            panelMessageB.CssClass = "alert alert-danger";
                            panelMessageB.Visible = true;
                            return;
                        }

                        DAL.TransactionHistory existingTrxHist = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                existingTrxHist = db.AdmissionDB.TransactionHistories
                                    .Where(c => c.CandidateID == cPayment.CandidateID.ToString() &&
                                        c.Status.Equals("VALID") &&
                                        c.TransactionID == cPayment.PaymentId.ToString()).FirstOrDefault();
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessageB.Text = "Error getting existing transaction. " + ex.Message + "; " + ex.InnerException.Message;
                            panelMessageB.CssClass = "alert alert-danger";
                            panelMessageB.Visible = true;
                            return;
                        }

                        #endregion

                        if (existingTrx == null && existingTrxHist == null)
                        {
                            try
                            {
                                #region InsertTransactionHistoryBkash
                                DAL.TransactionHistoryBKash sTrx = new DAL.TransactionHistoryBKash(); // sTrx = successfull Transaction
                                sTrx.Amount = txtPaidAmountB.Text;
                                sTrx.CandidateID = cPayment.CandidateID;
                                sTrx.Counter = txtCounterB.Text;
                                sTrx.CreatedOn = DateTime.Now;
                                sTrx.Currency = txtCurrencyB.Text;
                                sTrx.DateManualInsert = DateTime.Now;
                                sTrx.IsManualInsert = true;
                                sTrx.ManualInsertBy = uId;
                                sTrx.Receiver = txtReceiverB.Text;
                                sTrx.ReferencePaymentID = txtPaymentIdB.Text;
                                sTrx.Reversed = txtReversedB.Text;
                                sTrx.Sender = txtSenderB.Text;
                                sTrx.Service = txtServiceB.Text;
                                sTrx.TrxID = txtTrxIdB_1.Text;
                                sTrx.TrxStatus = txtStatusCodeB.Text;
                                sTrx.TrxTimestamp = txtTrxTimestampB.Text;
                                if (candidate != null)
                                {
                                    sTrx.ValueA = candidate.ID.ToString();
                                    sTrx.ValueB = candidate.FirstName;
                                }
                                sTrx.ValueC = cPayment.Amount.ToString();
                                sTrx.Attribute1 = "Success";

                                using (var dbsTrx = new CandidateDataManager())
                                {
                                    dbsTrx.Insert<DAL.TransactionHistoryBKash>(sTrx);
                                }
                                #endregion

                                #region InsertIntoTransactionHistory
                                DAL.TransactionHistory trx = new DAL.TransactionHistory();
                                trx.Amount = txtPaidAmountB.Text;
                                trx.ApiConnect = "done: bKash platform connector";
                                trx.Attribute1 = null;
                                trx.Attribute2 = null;
                                trx.Attribute3 = null;
                                trx.Attribute4 = null;
                                trx.BankTransactionID = txtTrxIdB_1.Text;
                                trx.BaseFair = null;
                                trx.CandidateID = cPayment.CandidateID.ToString();
                                trx.CardBrand = "bKash";
                                trx.CardIssuer = "bKash";
                                trx.CardIssuerCountry = "Bangladesh";
                                trx.CardIssuerCountryCode = "BD";
                                trx.CardNumber = "trx_id";
                                trx.CardType = "bKash Mobile Banking";
                                trx.CreatedOn = DateTime.Now;
                                trx.Currency = "BDT";
                                trx.CurrencyAmount = amountToBePaid.ToString();
                                trx.CurrencyRate = "1.0";
                                trx.CurrencyType = "BDT";
                                trx.DateManualInsert = DateTime.Now;
                                trx.GwVersion = null;
                                trx.IsInHouseCashTransaction = false;
                                trx.IsManualInsert = true;
                                trx.ManualInsertBy = uId;
                                trx.RiskLevel = "Unknown";
                                trx.RiskTitle = "Unknown";
                                trx.Status = "VALID";
                                trx.StoreAmount = cPayment.Amount.ToString();
                                trx.TransactionDate = txtTrxTimestampB.Text;
                                trx.TransactionID = txtPaymentIdB.Text;
                                trx.ValidatedOn = DateTime.Now.ToString();
                                trx.ValidationID = null;
                                trx.ValueA = cPayment.CandidateID.ToString();
                                trx.ValueB = candidate.FirstName;
                                trx.ValueC = cPayment.Amount.ToString();
                                trx.ValueD = null;

                                try
                                {
                                    using (var db = new OfficeDataManager())
                                    {
                                        db.Insert<DAL.TransactionHistory>(trx);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblMessageB.Text = "Error saving transaction. " + ex.Message + "; " + ex.InnerException.Message;
                                    panelMessageB.CssClass = "alert alert-danger";
                                    panelMessageB.Visible = true;
                                    return;
                                }

                                #endregion

                                #region UpdateCandidatePayment

                                cPayment.IsPaid = true;
                                cPayment.DateModified = DateTime.Now;
                                cPayment.ModifiedBy = cPayment.CandidateID;

                                try
                                {
                                    using (var dbCpayUpdate = new CandidateDataManager())
                                    {
                                        dbCpayUpdate.Update<DAL.CandidatePayment>(cPayment);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblMessageB.Text = "Error updating candidate payment. " + ex.Message + "; " + ex.InnerException.Message;
                                    panelMessageB.CssClass = "alert alert-danger";
                                    panelMessageB.Visible = true;
                                    return;
                                }

                                #endregion

                                #region SendSms  -- No need here

                                //GetSendingInfo(cPayment.CandidateID);

                                #endregion

                                lblMessageB.Text = "Transaction verified successfully.";
                                panelMessageB.CssClass = "alert alert-success";
                                panelMessageB.Visible = true;
                                return;
                            }
                            catch (Exception ex)
                            {
                                lblMessageB.Text = "Error saving bKash transaction. " + ex.Message + "; " + ex.InnerException.Message;
                                panelMessageB.CssClass = "alert alert-danger";
                                panelMessageB.Visible = true;
                                return;
                            }
                        }

                        //lblbKashMsg.Text = "";
                        //panelBkashMsg.CssClass = "alert alert-success";
                        //panelBkashMsg.Visible = true;
                        //return;

                    }
                    else
                    {
                        #region InsertTransactionHistoryBkash IF AmountLessPaid
                        DAL.TransactionHistoryBKash sTrx = new DAL.TransactionHistoryBKash(); // sTrx = successfull Transaction
                        sTrx.Amount = txtPaidAmountB.Text;
                        sTrx.CandidateID = cPayment.CandidateID;
                        sTrx.Counter = txtCounterB.Text;
                        sTrx.CreatedOn = DateTime.Now;
                        sTrx.Currency = txtCurrencyB.Text;
                        sTrx.DateManualInsert = null;
                        sTrx.IsManualInsert = false;
                        sTrx.ManualInsertBy = null;
                        sTrx.Receiver = txtReceiverB.Text;
                        sTrx.ReferencePaymentID = txtPaymentIdB.Text;
                        sTrx.Reversed = "No";
                        sTrx.Sender = txtSenderB.Text;
                        sTrx.Service = txtServiceB.Text;
                        sTrx.TrxID = txtTrxIdB_1.Text;
                        sTrx.TrxStatus = "Less Amount Paid";
                        sTrx.TrxTimestamp = txtTrxTimestampB.Text;
                        if (candidate != null)
                        {
                            sTrx.ValueA = candidate.ID.ToString();
                            sTrx.ValueB = candidate.FirstName;
                        }
                        sTrx.ValueC = cPayment.Amount.ToString();
                        sTrx.Attribute1 = "Less Amount Paid";

                        using (var dbsTrx = new CandidateDataManager())
                        {
                            dbsTrx.Insert<DAL.TransactionHistoryBKash>(sTrx);
                        }
                        #endregion

                        lblMessageB.Text = "Your amount does not match. You were suppose to pay " + amountToBePaid + " BDT, but found " + txtPaidAmountB.Text + " BDT paid. <br/>"
                            + "Reference No. " + txtPaymentIdB.Text;
                        panelMessageB.CssClass = "alert alert-danger";
                        panelMessageB.Visible = true;
                        return;
                    }

                }
                else
                {
                    lblMessageB.Text = "Candidate payment does not exist in database.";
                    panelMessageB.CssClass = "alert alert-danger";
                    panelMessageB.Visible = true;
                    return;
                }
            } //end if(trxStatusCodeB.Equals("0000"))
            #endregion
            //-----------------------------------
            #region if(trxStatusCodeB.Text != "0000")
            else
            {
                string trx_Id = txtTrxIdB_1.Text;

                DAL.BkashTrxStatusCode btsCode = null;
                try
                {
                    if (!txtStatusCodeB.Equals("0000"))
                    {
                        using (var db = new OfficeDataManager())
                        {
                            string _statusCode = txtStatusCodeB.Text;
                            btsCode = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == _statusCode).FirstOrDefault();
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessageB.Text = "Error getting bKash Transaction Status Codes. " + ex.Message + "; " + ex.InnerException.Message;
                    panelMessageB.CssClass = "alert alert-danger";
                    panelMessageB.Visible = true;
                    return;
                }

                if (btsCode != null)
                {
                    #region InsertTransactionHistoryBkash IF FAILED
                    DAL.TransactionHistoryBKash sTrx = new DAL.TransactionHistoryBKash(); // sTrx = successfull Transaction
                    sTrx.Amount = null;
                    sTrx.CandidateID = null;
                    sTrx.Counter = null;
                    sTrx.CreatedOn = DateTime.Now;
                    sTrx.Currency = null;
                    sTrx.DateManualInsert = null;
                    sTrx.IsManualInsert = false;
                    sTrx.ManualInsertBy = null;
                    sTrx.Receiver = null;
                    sTrx.ReferencePaymentID = null;
                    sTrx.Reversed = null;
                    sTrx.Sender = null;
                    sTrx.Service = null;
                    sTrx.TrxID = trx_Id;
                    sTrx.TrxStatus = txtStatusCodeB.Text;
                    sTrx.TrxTimestamp = null;
                    sTrx.ValueA = null;
                    sTrx.ValueB = null;
                    sTrx.ValueC = null;
                    sTrx.Attribute1 = btsCode.Interpretation + "; " + btsCode.Message;

                    using (var dbsTrx = new CandidateDataManager())
                    {
                        dbsTrx.Insert<DAL.TransactionHistoryBKash>(sTrx);
                    }
                    #endregion

                    lblMessageB.Text = txtStatusCodeB.Text + ": " + btsCode.Interpretation + "; " + btsCode.Message;
                    panelMessageB.CssClass = "alert alert-danger";
                    panelMessageB.Visible = true;
                    return;
                }
                else
                {
                    lblMessageB.Text = txtStatusCodeB.Text + ": Please try again later.";
                    panelMessageB.CssClass = "alert alert-danger";
                    panelMessageB.Visible = true;
                    return;
                }
            }
            #endregion

        }


        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessageEkPay.Text = string.Empty;
                lblMessageEkPay.Text = msg.ToString();
                lblMessageEkPay.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessageEkPay.Attributes.CssStyle.Add("color", "green");

                messagePanelEkPay.Visible = true;
                messagePanelEkPay.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessageEkPay.Text = string.Empty;
                lblMessageEkPay.Text = msg.ToString();
                lblMessageEkPay.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessageEkPay.Attributes.CssStyle.Add("color", "crimson");

                messagePanelEkPay.Visible = true;
                messagePanelEkPay.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessageEkPay.Text = string.Empty;
                messagePanelEkPay.Visible = false;
            }

        }

        protected void ClearFieldEkPay()
        {
            lblEkPayPaymentId.Text = string.Empty;
            lblEkPayTransactionId.Text = string.Empty;

            lblEkPayName.Text = string.Empty;
            lblEkPayPhone.Text = string.Empty;
            lblEkPayEmail.Text = string.Empty;

            lblEkPayPaymentDate.Text = string.Empty;
            lblEkPayPaymentType.Text = string.Empty;
            lblEkPayPaymentGateway.Text = string.Empty;
        }

        protected void btnSearchByEkPay_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            string ekPaySearchPaymentIdString = txtEkPaySearchPaymentId.Text.Trim();
            try
            {
                ClearFieldEkPay();
                panelEkPayGetInfoView.Visible = false;

                if (!string.IsNullOrEmpty(ekPaySearchPaymentIdString) && Convert.ToInt64(ekPaySearchPaymentIdString) > 0)
                {


                    long paymentId = Convert.ToInt64(ekPaySearchPaymentIdString);
                    DAL.CandidatePayment cp = null;
                    using (var db = new CandidateDataManager())
                    {
                        cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == paymentId).FirstOrDefault();
                    }

                    if (cp != null && cp.IsPaid == true)
                    {
                        #region Log Insert EkPaySearch_01
                        try
                        {


                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                            //dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                            dLog.CandidateId = cp.CandidateID;
                            dLog.EventName = "ekpaysearch";
                            dLog.PageName = "CandidateTransaction.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "EkPaySearch_01: Payment is Updated in Admission System; Data: " + JsonConvert.SerializeObject(cp).ToString();
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion

                        MessageView("Candidate payment is already updated.", "success");
                    }
                    else
                    {

                        #region Log Insert EkPaySearch_02
                        try
                        {


                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                            //dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                            dLog.CandidateId = cp.CandidateID;
                            dLog.EventName = "ekpaysearch";
                            dLog.PageName = "CandidateTransaction.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "EkPaySearch_02: Payment is Not Updated in Admission System; Data: " + JsonConvert.SerializeObject(cp).ToString();
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion


                        DAL.OnlineCollectionAttempt oca = null;

                        string ekPaySearchTransactionIdString = txtEkPaySearchTransactionId.Text.Trim();
                        if (!string.IsNullOrEmpty(ekPaySearchTransactionIdString))
                        {
                            using (var db = new CandidateDataManager())
                            {
                                //== Getting the last transaction id from online attem
                                oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.PaymentId == paymentId && x.TransactionId == ekPaySearchTransactionIdString).OrderByDescending(x => x.ID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            using (var db = new CandidateDataManager())
                            {
                                //== Getting the last transaction id from online attem
                                oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.PaymentId == paymentId).OrderByDescending(x => x.ID).FirstOrDefault();
                            }
                        }


                        if (oca != null)
                        {

                            #region Log Insert EkPaySearch_03
                            try
                            {


                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaysearch";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPaySearch_03: Get Online Collection Attempt Transaction Data; Data: " + JsonConvert.SerializeObject(oca).ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion


                            #region Get Store Info
                            string ekPayBaseURL = "";
                            string ekPaySubURL = "";
                            string ekPayURLVersion = "";


                            DAL.StoreEkPay storeEkPay = null;
                            using (var db = new OfficeDataManager())
                            {
                                storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                            }

                            #region Log Insert EkPaySearch_04
                            try
                            {


                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaysearch";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPaySearch_04: Get EkPay Store Info; Data: " + JsonConvert.SerializeObject(storeEkPay).ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            ekPayBaseURL = storeEkPay.BaseURL.ToString();
                            ekPaySubURL = storeEkPay.SubURL.ToString();
                            ekPayURLVersion = storeEkPay.URLVersion.ToString();

                            #endregion

                            EkPayGetModel getModel = new EkPayGetModel();
                            getModel.trnx_id = oca.TransactionId;
                            getModel.trans_date = Convert.ToDateTime(oca.CreatedDate).ToString("yyyy-MM-dd"); //DateTime.Now.ToString("yyyy-MM-dd"); // token newer time a jei datetime disi oita dite hobe

                            EkPayPaymentGateway ekPayPG = new EkPayPaymentGateway(ekPayBaseURL, ekPaySubURL, ekPayURLVersion, "/get-status");
                            ResponseEkPay responseEkPay = ekPayPG.SearchByTransactionId(getModel);

                            #region Log Insert EkPaySearch_05
                            try
                            {


                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaysearch";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPaySearch_05: Payment Response From EkPay; Data: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            if (responseEkPay.ResponseCode == 200)
                            {
                                var data = (JObject)JsonConvert.DeserializeObject(responseEkPay.ResponseData.ToString());

                                string msgcode = data["msg_code"].ToString();
                                string msgdet = data["msg_det"].ToString();
                                //string custname = data["cust_info"].Value<string>("cust_name");

                                if (msgcode == "1020")
                                {
                                    #region Log Insert EkPaySearch_06
                                    try
                                    {


                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                        dLog.Attribute2 = oca.TransactionId;
                                        dLog.CandidateId = cp.CandidateID;
                                        dLog.EventName = "ekpaysearch";
                                        dLog.PageName = "CandidateTransaction.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "EkPaySearch_06: Payment Is Updated In EkPay";
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    panelEkPayGetInfoView.Visible = true;

                                    MessageView("Payment is updated in EkPay.", "success");

                                    string custname = data["cust_info"].Value<string>("cust_name");
                                    string custmobono = data["cust_info"].Value<string>("cust_mobo_no");
                                    string custemail = data["cust_info"].Value<string>("cust_email");

                                    string totalpablamt = data["trnx_info"].Value<string>("total_pabl_amt");

                                    string paytimestamp = data["pi_det_info"].Value<string>("pay_timestamp");
                                    string piname = data["pi_det_info"].Value<string>("pi_name");
                                    string pitype = data["pi_det_info"].Value<string>("pi_type");
                                    string pinumber = data["pi_det_info"].Value<string>("pi_number");
                                    string pigateway = data["pi_det_info"].Value<string>("pi_gateway");



                                    lblEkPayPaymentId.Text = paymentId.ToString();
                                    lblEkPayTransactionId.Text = oca.TransactionId.ToString();

                                    lblEkPayName.Text = custname;
                                    lblEkPayPhone.Text = custmobono;
                                    lblEkPayEmail.Text = custemail;

                                    lblEkPayPaymentDate.Text = paytimestamp;
                                    lblEkPayPaymentType.Text = pitype;
                                    lblEkPayPaymentGateway.Text = pigateway;
                                }
                                else
                                {
                                    #region Log Insert EkPaySearch_07
                                    try
                                    {


                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                        dLog.Attribute2 = oca.TransactionId;
                                        dLog.CandidateId = cp.CandidateID;
                                        dLog.EventName = "ekpaysearch";
                                        dLog.PageName = "CandidateTransaction.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "EkPaySearch_07: Payment Is Not Updated In EkPay; Data: " + JsonConvert.SerializeObject(data).ToString();
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //payment is not updated in EkPay
                                    MessageView("Payment is not updated in EkPay! Error: " + msgdet, "fail");
                                }


                            }
                            else
                            {
                                #region Log Insert EkPaySearch_08
                                try
                                {


                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                    dLog.Attribute2 = oca.TransactionId;
                                    dLog.CandidateId = cp.CandidateID;
                                    dLog.EventName = "ekpaysearch";
                                    dLog.PageName = "CandidateTransaction.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "EkPaySearch_08: Payment success not found from EkPay; Data: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //payment is not updated in EkPay
                                if (responseEkPay.ResponseCode == 600)
                                {
                                    MessageView(responseEkPay.ResponseMessage, "fail");
                                }
                                else
                                {
                                    MessageView(JsonConvert.SerializeObject(responseEkPay.ResponseData).ToString(), "fail");
                                }
                            }

                        }
                        else
                        {
                            #region Log Insert EkPaySearch_09
                            try
                            {


                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaysearch";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPaySearch_09: No transaction id found for payment id; PaymentID: " + paymentId.ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            MessageView("No transaction id found for payment id !", "fail");
                        }
                    }
                }
                else
                {
                    #region Log Insert EkPaySearch_10
                    try
                    {


                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = uId;
                        dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                        //dLog.Attribute2 = oca.TransactionId;
                        //dLog.CandidateId = cp.CandidateID;
                        dLog.EventName = "ekpaysearch";
                        dLog.PageName = "CandidateTransaction.aspx";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "EkPaySearch_10: Please provide valid payment id; PaymentID: " + ekPaySearchPaymentIdString.ToString();
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                    MessageView("Please provide valid payment id!", "fail");
                }
            }
            catch (Exception ex)
            {
                #region Log Insert EkPaySearch_11
                try
                {


                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.DateTime = DateTime.Now;
                    dLog.DateCreated = DateTime.Now;
                    dLog.UserId = uId;
                    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                    //dLog.Attribute2 = oca.TransactionId;
                    //dLog.CandidateId = cp.CandidateID;
                    dLog.EventName = "ekpaysearch";
                    dLog.PageName = "CandidateTransaction.aspx";
                    //dLog.OldData = logOldObject;
                    dLog.NewData = "EkPaySearch_11: Exception: " + ex.Message.ToString();
                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ex1)
                {
                }
                #endregion

                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnUpdatePaymentFromEkPay_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            string ekPaySearchPaymentIdString = txtEkPaySearchPaymentId.Text.Trim();
            try
            {
                if (!string.IsNullOrEmpty(ekPaySearchPaymentIdString) && Convert.ToInt64(ekPaySearchPaymentIdString) > 0)
                {
                    #region Log Insert EkPayPaymentUpdate_01
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = uId;
                        dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                        //dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                        //dLog.CandidateId = cp.CandidateID;
                        dLog.EventName = "ekpaypaymentupdate";
                        dLog.PageName = "CandidateTransaction.aspx";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "EkPayPaymentUpdate_01: Request for payment update;";
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion



                    long paymentId = Convert.ToInt64(ekPaySearchPaymentIdString);
                    DAL.CandidatePayment cp = null;
                    using (var db = new CandidateDataManager())
                    {
                        cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == paymentId).FirstOrDefault();
                    }

                    if (cp != null && cp.IsPaid == true)
                    {
                        #region Log Insert EkPayPaymentUpdate_02
                        try
                        {
                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = uId;
                            dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                            //dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                            dLog.CandidateId = cp.CandidateID;
                            dLog.EventName = "ekpaypaymentupdate";
                            dLog.PageName = "CandidateTransaction.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "EkPayPaymentUpdate_02: Payment is already Updated;";
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion

                        MessageView("Candidate payment is already updated.", "success");
                        return;
                    }
                    else
                    {
                        DAL.OnlineCollectionAttempt oca = null;

                        string ekPaySearchTransactionIdString = txtEkPaySearchTransactionId.Text.Trim();
                        if (!string.IsNullOrEmpty(ekPaySearchTransactionIdString))
                        {
                            using (var db = new CandidateDataManager())
                            {
                                //== Getting the last transaction id from online attem
                                oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.PaymentId == paymentId && x.TransactionId == ekPaySearchTransactionIdString).OrderByDescending(x => x.ID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            using (var db = new CandidateDataManager())
                            {
                                //== Getting the last transaction id from online attem
                                oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.PaymentId == paymentId).OrderByDescending(x => x.ID).FirstOrDefault();
                            }
                        }
                        if (oca != null)
                        {
                            #region Log Insert EkPayPaymentUpdate_03
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaypaymentupdate";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPayPaymentUpdate_03: Get Online Collection Attempts Transaction ID; Data: " + JsonConvert.SerializeObject(oca).ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            #region Get Store Info
                            string ekPayBaseURL = "";
                            string ekPaySubURL = "";
                            string ekPayURLVersion = "";


                            DAL.StoreEkPay storeEkPay = null;
                            using (var db = new OfficeDataManager())
                            {
                                storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                            }


                            #region Log Insert EkPayPaymentUpdate_04
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaypaymentupdate";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPayPaymentUpdate_04: EkPay Store Info; Data: " + JsonConvert.SerializeObject(storeEkPay).ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion



                            ekPayBaseURL = storeEkPay.BaseURL.ToString();
                            ekPaySubURL = storeEkPay.SubURL.ToString();
                            ekPayURLVersion = storeEkPay.URLVersion.ToString();

                            #endregion

                            EkPayGetModel getModel = new EkPayGetModel();
                            getModel.trnx_id = oca.TransactionId;
                            getModel.trans_date = Convert.ToDateTime(oca.CreatedDate).ToString("yyyy-MM-dd"); //DateTime.Now.ToString("yyyy-MM-dd"); // token newer time a jei datetime disi oita dite hobe

                            EkPayPaymentGateway ekPayPG = new EkPayPaymentGateway(ekPayBaseURL, ekPaySubURL, ekPayURLVersion, "/get-status");
                            ResponseEkPay responseEkPay = ekPayPG.SearchByTransactionId(getModel);


                            #region Log Insert EkPayPaymentUpdate_05
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaypaymentupdate";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPayPaymentUpdate_05: EkPay API Called and Response; Data: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            if (responseEkPay.ResponseCode == 200)
                            {
                                var data = (JObject)JsonConvert.DeserializeObject(responseEkPay.ResponseData.ToString());

                                string msgcode = data["msg_code"].ToString();
                                string msgdet = data["msg_det"].ToString();
                                //string custname = data["cust_info"].Value<string>("cust_name");

                                if (msgcode == "1020")
                                {
                                    //panelEkPayGetInfoView.Visible = true;

                                    #region Log Insert EkPayPaymentUpdate_06
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                        dLog.Attribute2 = oca.TransactionId;
                                        dLog.CandidateId = cp.CandidateID;
                                        dLog.EventName = "ekpaypaymentupdate";
                                        dLog.PageName = "CandidateTransaction.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "EkPayPaymentUpdate_06: Payment is Updated in EkPay; Data: " + JsonConvert.SerializeObject(data).ToString();
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion


                                    string custname = data["cust_info"].Value<string>("cust_name");
                                    string custmobono = data["cust_info"].Value<string>("cust_mobo_no");
                                    string custemail = data["cust_info"].Value<string>("cust_email");

                                    string totalpablamt = data["trnx_info"].Value<string>("total_pabl_amt");
                                    string curr = data["trnx_info"].Value<string>("curr");

                                    string paytimestamp = data["pi_det_info"].Value<string>("pay_timestamp");
                                    string piname = data["pi_det_info"].Value<string>("pi_name");
                                    string pitype = data["pi_det_info"].Value<string>("pi_type");
                                    string pinumber = data["pi_det_info"].Value<string>("pi_number");
                                    string pigateway = data["pi_det_info"].Value<string>("pi_gateway");

                                    if (cp != null && cp.IsPaid == false)
                                    {
                                        #region Candodate Payment Update
                                        try
                                        {
                                            cp.IsPaid = true;
                                            cp.ModifiedBy = cp.CandidateID;
                                            cp.DateModified = DateTime.Now;

                                            using (var db = new CandidateDataManager())
                                            {
                                                db.Update<DAL.CandidatePayment>(cp);
                                            }

                                            #region Log Insert EkPayPaymentUpdate_07
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                                dLog.Attribute2 = oca.TransactionId;
                                                dLog.CandidateId = cp.CandidateID;
                                                dLog.EventName = "ekpaypaymentupdate";
                                                dLog.PageName = "CandidateTransaction.aspx";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "EkPayPaymentUpdate_07: Payment is Updated in Admission System; Data: " + JsonConvert.SerializeObject(cp).ToString();
                                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            #endregion

                                            MessageView("Payment is Updated in Admission System", "success");

                                            GetSendingInfo(cp.CandidateID);
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Log Insert EkPayPaymentUpdate_08
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                                dLog.Attribute2 = oca.TransactionId;
                                                dLog.CandidateId = cp.CandidateID;
                                                dLog.EventName = "ekpaypaymentupdate";
                                                dLog.PageName = "CandidateTransaction.aspx";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "EkPayPaymentUpdate_08: Payment Updated Exception; Exception: " + ex.Message.ToString();
                                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex1)
                                            {
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        #region Online Collection Attempt Update
                                        try
                                        {
                                            if (oca != null)
                                            {
                                                oca.IsPaid = true;
                                                oca.ModifiedBy = cp.CandidateID;
                                                oca.ModifiedDate = DateTime.Now;

                                                using (var db = new CandidateDataManager())
                                                {
                                                    db.Update<DAL.OnlineCollectionAttempt>(oca);
                                                }

                                                #region Log Insert EkPayPaymentUpdate_09
                                                try
                                                {
                                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                    dLog.DateTime = DateTime.Now;
                                                    dLog.DateCreated = DateTime.Now;
                                                    dLog.UserId = uId;
                                                    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                                    dLog.Attribute2 = oca.TransactionId;
                                                    dLog.CandidateId = cp.CandidateID;
                                                    dLog.EventName = "ekpaypaymentupdate";
                                                    dLog.PageName = "CandidateTransaction.aspx";
                                                    //dLog.OldData = logOldObject;
                                                    dLog.NewData = "EkPayPaymentUpdate_09: Online Collection Attempt is Updated in Admission System; Data: " + JsonConvert.SerializeObject(oca).ToString();
                                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                    LogWriter.DataLogWriter(dLog);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                #endregion
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Log Insert EkPayPaymentUpdate_10
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                                dLog.Attribute2 = oca.TransactionId;
                                                dLog.CandidateId = cp.CandidateID;
                                                dLog.EventName = "ekpaypaymentupdate";
                                                dLog.PageName = "CandidateTransaction.aspx";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "EkPayPaymentUpdate_10: Online Collection Attempt Update Exception; Exception: " + ex.Message.ToString();
                                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex1)
                                            {
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        #region INSERT TRANSACTION HISTORY
                                        try
                                        {


                                            DAL.TransactionHistory thExist = null;
                                            using (var db = new OfficeDataManager())
                                            {
                                                thExist = db.AdmissionDB.TransactionHistories
                                                    .Where(c => c.TransactionID == cp.PaymentId.ToString())
                                                    .FirstOrDefault();
                                            }

                                            if (thExist == null) //insert transaction history if it does not exist.
                                            {
                                                DAL.TransactionHistory transactionHistory = new DAL.TransactionHistory();
                                                transactionHistory.CandidateID = cp.CandidateID.ToString();
                                                transactionHistory.Status = "VALID";
                                                transactionHistory.TransactionDate = paytimestamp.ToString();
                                                transactionHistory.TransactionID = cp.PaymentId.ToString();
                                                transactionHistory.ValidationID = oca.TransactionId.ToString();
                                                transactionHistory.Amount = cp.Amount.ToString();
                                                transactionHistory.StoreAmount = cp.Amount.ToString();
                                                transactionHistory.Currency = curr.ToString();
                                                transactionHistory.BankTransactionID = pinumber.ToString();
                                                transactionHistory.CardType = pitype.ToString();
                                                transactionHistory.CardNumber = pinumber.ToString();
                                                transactionHistory.CardIssuer = piname.ToString();
                                                transactionHistory.CardBrand = pigateway.ToString();
                                                transactionHistory.CardIssuerCountry = "BANGLADESH";
                                                transactionHistory.CardIssuerCountryCode = "BD";
                                                transactionHistory.CurrencyType = curr.ToString();
                                                transactionHistory.CurrencyAmount = cp.Amount.ToString();
                                                transactionHistory.CurrencyRate = null;
                                                transactionHistory.BaseFair = null;
                                                transactionHistory.Attribute1 = "EkPay";
                                                //transactionHistory.ValueA = outputArray["value_a"].ToString();
                                                //transactionHistory.ValueA = outputArray["value_a"].ToString();
                                                //transactionHistory.ValueB = outputArray["value_b"].ToString();
                                                //transactionHistory.ValueC = outputArray["value_c"].ToString();
                                                //transactionHistory.ValueD = outputArray["value_d"].ToString();
                                                transactionHistory.RiskTitle = "Safe";
                                                transactionHistory.RiskLevel = "0";
                                                transactionHistory.ApiConnect = "DONE";
                                                transactionHistory.ValidatedOn = paytimestamp.ToString();
                                                transactionHistory.GwVersion = null;
                                                transactionHistory.CreatedOn = DateTime.Now;
                                                transactionHistory.IsInHouseCashTransaction = true;
                                                transactionHistory.IsManualInsert = true;
                                                transactionHistory.ManualInsertBy = uId;
                                                transactionHistory.DateManualInsert = DateTime.Now;

                                                using (var db = new OfficeDataManager())
                                                {
                                                    db.Insert<DAL.TransactionHistory>(transactionHistory);
                                                }

                                                #region Log Insert EkPayPaymentUpdate_11
                                                try
                                                {
                                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                    dLog.DateTime = DateTime.Now;
                                                    dLog.DateCreated = DateTime.Now;
                                                    dLog.UserId = uId;
                                                    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                                    dLog.Attribute2 = oca.TransactionId;
                                                    dLog.CandidateId = cp.CandidateID;
                                                    dLog.EventName = "ekpaypaymentupdate";
                                                    dLog.PageName = "CandidateTransaction.aspx";
                                                    //dLog.OldData = logOldObject;
                                                    dLog.NewData = "EkPayPaymentUpdate_11: Transaction History Insert; Data: " + JsonConvert.SerializeObject(transactionHistory).ToString();
                                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                    LogWriter.DataLogWriter(dLog);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                #endregion

                                            }


                                        }
                                        catch (Exception ex)
                                        {

                                            #region Log Insert EkPayPaymentUpdate_12
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                                dLog.Attribute2 = oca.TransactionId;
                                                dLog.CandidateId = cp.CandidateID;
                                                dLog.EventName = "ekpaypaymentupdate";
                                                dLog.PageName = "CandidateTransaction.aspx";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "EkPayPaymentUpdate_12: Transaction History Insert Exception; Exception: " + ex.Message.ToString();
                                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex1)
                                            {
                                            }
                                            #endregion

                                            //MessageView("Error: Something went wrong! Exception: " + ex.Message.ToString(), "fail");

                                        }
                                        #endregion
                                    }
                                    else
                                    {

                                        #region Log Insert EkPayPaymentUpdate_13
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = uId;
                                            dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                            dLog.Attribute2 = oca.TransactionId;
                                            dLog.CandidateId = cp.CandidateID;
                                            dLog.EventName = "ekpaypaymentupdate";
                                            dLog.PageName = "CandidateTransaction.aspx";
                                            //dLog.OldData = logOldObject;
                                            dLog.NewData = "EkPayPaymentUpdate_13: Payment is already updated in Admission System; Data: " + JsonConvert.SerializeObject(cp).ToString();
                                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        MessageView("Payment is already updated in Admission System !", "success");

                                    }





                                }
                                else
                                {

                                    #region Log Insert EkPayPaymentUpdate_14
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                        dLog.Attribute2 = oca.TransactionId;
                                        dLog.CandidateId = cp.CandidateID;
                                        dLog.EventName = "ekpaypaymentupdate";
                                        dLog.PageName = "CandidateTransaction.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "EkPayPaymentUpdate_14: Payment is not updated in EkPay; Data: " + JsonConvert.SerializeObject(data).ToString();
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //payment is not updated in EkPay
                                    MessageView("Payment is not updated in EkPay !", "fail");
                                }


                            }
                            else
                            {
                                #region Log Insert EkPayPaymentUpdate_15
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                    dLog.Attribute2 = oca.TransactionId;
                                    dLog.CandidateId = cp.CandidateID;
                                    dLog.EventName = "ekpaypaymentupdate";
                                    dLog.PageName = "CandidateTransaction.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "EkPayPaymentUpdate_15: EkPay Response; Data: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion


                                //payment is not updated in EkPay
                                if (responseEkPay.ResponseCode == 600)
                                {
                                    MessageView(responseEkPay.ResponseMessage, "fail");
                                }
                                else
                                {
                                    MessageView(JsonConvert.SerializeObject(responseEkPay.ResponseData).ToString(), "fail");
                                }
                            }

                        }
                        else
                        {
                            #region Log Insert EkPayPaymentUpdate_16
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                //dLog.Attribute2 = oca.TransactionId;
                                dLog.CandidateId = cp.CandidateID;
                                dLog.EventName = "ekpaypaymentupdate";
                                dLog.PageName = "CandidateTransaction.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPayPaymentUpdate_16: No transaction id found for payment id; PaymentId: " + paymentId.ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            MessageView("No transaction id found for payment id !", "fail");
                        }
                    }
                }
                else
                {
                    #region Log Insert EkPayPaymentUpdate_17
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = uId;
                        dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                        //dLog.Attribute2 = oca.TransactionId;
                        //dLog.CandidateId = cp.CandidateID;
                        dLog.EventName = "ekpaypaymentupdate";
                        dLog.PageName = "CandidateTransaction.aspx";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "EkPayPaymentUpdate_17: Please provide valid payment id; PaymentId: " + ekPaySearchPaymentIdString.ToString();
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    MessageView("Please provide valid payment id!", "fail");
                }
            }
            catch (Exception ex)
            {
                #region Log Insert EkPayPaymentUpdate_18
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.DateTime = DateTime.Now;
                    dLog.DateCreated = DateTime.Now;
                    dLog.UserId = uId;
                    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                    //dLog.Attribute2 = oca.TransactionId;
                    //dLog.CandidateId = cp.CandidateID;
                    dLog.EventName = "ekpaypaymentupdate";
                    dLog.PageName = "CandidateTransaction.aspx";
                    //dLog.OldData = logOldObject;
                    dLog.NewData = "EkPayPaymentUpdate_18: Error: Something went wrong.Exception: " + ex.Message.ToString();
                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ex1)
                {
                }
                #endregion

                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
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
                string messageBody = "BUP Admission. Login to https://admission.bup.edu.bd/Admission/Login. Username: " + username + " ; Password: " + password + " ";

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
            //"<p>Please check your username and password given below: </p>" +

            string mailbody = "<p>Dear " + candidateName + ",</p>" +
                        "<p>Login to https://admission.bup.edu.bd/Admission/Login .</p>" + "<br/>" +

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

        protected void btnSearchByEkPayTransactionId_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                string ekPaySearchTransactionIdString = txtEkPaySearchTransactionId.Text.Trim();
                if (!string.IsNullOrEmpty(ekPaySearchTransactionIdString))
                {
                    DAL.OnlineCollectionAttempt ocaModel = null;
                    using (var db = new CandidateDataManager())
                    {
                        ocaModel = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.TransactionId == ekPaySearchTransactionIdString).FirstOrDefault();
                    }

                    if (ocaModel != null)
                    {
                        txtEkPaySearchPaymentId.Text = ocaModel.PaymentId.ToString();
                        btnSearchByEkPay_Click(null, null);
                    }
                    else
                    {
                        MessageView("No TransactionID found", "fail");
                    }
                }
                else
                {
                    MessageView("Please provide valid TransactionID", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnEkPayUnPaidCandidate_Click(object sender, EventArgs e)
        {

            MessageView("", "clear");

            try
            {

                panelCheckEkPayPaymentUpdate.Visible = false;

                if (Convert.ToInt32(ddlSession.SelectedValue) > 0)
                {
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                    List<DAL.SPGetUnPaidCandidateEkPay_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPGetUnPaidCandidateEkPay(acaCalId).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        panelCheckEkPayPaymentUpdate.Visible = true;

                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        ReportDataSource rds = new ReportDataSource("DataSet1", list);

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("TotalCandidate", list.Count().ToString()));

                        ReportViewer1.LocalReport.SetParameters(param1);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        MessageView("No data found!", "fail");
                    }
                }
                else
                {
                    MessageView("Please select Session!", "fail");
                }


            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }



        }

        protected void btnCheckForPaymentUpdateInEkPay_Click(object sender, EventArgs e)
        {

            MessageView("", "clear");

            try
            {
                if (Convert.ToInt32(ddlSession.SelectedValue) > 0)
                {
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                    List<DAL.SPGetUnPaidCandidateEkPay_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPGetUnPaidCandidateEkPay(acaCalId).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {

                        #region Check Payment is updated in EkPay or not
                        try
                        {
                            #region Get Store Info
                            string ekPayBaseURL = "";
                            string ekPaySubURL = "";
                            string ekPayURLVersion = "";


                            DAL.StoreEkPay storeEkPay = null;
                            using (var db = new OfficeDataManager())
                            {
                                storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                            }

                            ekPayBaseURL = storeEkPay.BaseURL.ToString();
                            ekPaySubURL = storeEkPay.SubURL.ToString();
                            ekPayURLVersion = storeEkPay.URLVersion.ToString();

                            #endregion


                            foreach (var tData in list)
                            {
                                EkPayGetModel getModel = new EkPayGetModel();
                                getModel.trnx_id = tData.TransactionId;
                                getModel.trans_date = Convert.ToDateTime(tData.TransactionDate).ToString("yyyy-MM-dd"); // token newer time a jei datetime disi oita dite hobe

                                EkPayPaymentGateway ekPayPG = new EkPayPaymentGateway(ekPayBaseURL, ekPaySubURL, ekPayURLVersion, "/get-status");
                                ResponseEkPay responseEkPay = ekPayPG.SearchByTransactionId(getModel);


                                if (responseEkPay.ResponseCode == 200)
                                {
                                    var data = (JObject)JsonConvert.DeserializeObject(responseEkPay.ResponseData.ToString());

                                    string msgcode = data["msg_code"].ToString();
                                    string msgdet = data["msg_det"].ToString();
                                    //string custname = data["cust_info"].Value<string>("cust_name");

                                    if (msgcode == "1020")
                                    {
                                        tData.IsEKPayUpdate = "Yes";
                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion



                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        ReportDataSource rds = new ReportDataSource("DataSet1", list);

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("TotalCandidate", list.Count().ToString()));

                        ReportViewer1.LocalReport.SetParameters(param1);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        MessageView("No data found!", "fail");
                    }
                }
                else
                {
                    MessageView("Please select Session!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

    }


}
