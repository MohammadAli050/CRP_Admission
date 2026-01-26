using Admission.App_Start;
using CommonUtility;
using DAL;
using DAL.ViewModels.EkPayModel;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static Admission.Controllers.EkPayPaymentGatewayController;

namespace Admission.Admission.HelpDesk
{
    public partial class CheckPaymentFromGatewayHelpDesk : PageBase
    {



        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {

            }
        }

        //private void ClearMessage()
        //{
        //    lblMessageSsl.Text = "";
        //}

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

        private void ClearSslPanel()
        {
            //txtPaymentIdSsl.Text = string.Empty;

            //panel_Message1.Visible = false;

            lblTranId.Text = string.Empty;
            lblCandidateName.Text = string.Empty;
            lblStatus.Text = string.Empty;
            lblTransDate.Text = string.Empty;
            lblBankTranId.Text = string.Empty;
            lblValId.Text = string.Empty;
            lblAmount.Text = string.Empty;
            lblReceivable.Text = string.Empty;
            lblCardType.Text = string.Empty;
            lblCardNumber.Text = string.Empty;
            lblIssuerBankCountry.Text = string.Empty;
            lblCurrency.Text = string.Empty;
        }

        private void ClearBkashPanel()
        {
            //txtTrxIdBkash.Text = null;

            //panel_Message2.Visible = false;

            //lblNameB.Text = string.Empty;
            //lblSysAmntB.Text = string.Empty;
            //lblStatusCodeB.Text = string.Empty;
            //lblStatusDetailsB.Text = string.Empty;
            //lblTrxIdB.Text = string.Empty;
            //lblReferenceNoB.Text = string.Empty;
            //lblCounterB.Text = string.Empty;
            //lblPaidAmntB.Text = string.Empty;
            //lblSenderB.Text = string.Empty;
            //lblServiceB.Text = string.Empty;
            //lblCurrencyB.Text = string.Empty;
            //lblReceiverB.Text = string.Empty;
        }

        protected void btnLoadSsl_Click(object sender, EventArgs e)
        {
            lblSearchMsg.Text = string.Empty;

            try
            {
                long paymentId = -1;
                if (!string.IsNullOrEmpty(txtPaymentIdSsl.Text.Trim()))
                {
                    paymentId = Int64.Parse(txtPaymentIdSsl.Text);
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
                                .Where(c => c.TransactionID == txtPaymentIdSsl.Text.Trim())
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
   
            }



            #region N/A
            //MessageView("", "clear");
            ////ClearMessage();
            //ClearSslPanel();


            //long paymentId = -1;
            //if (!string.IsNullOrEmpty(txtPaymentIdSsl.Text.Trim()))
            //{
            //    paymentId = Int64.Parse(txtPaymentIdSsl.Text);
            //}

            //DAL.CandidatePayment cPayment = null;
            //try
            //{
            //    using (var db = new OfficeDataManager())
            //    {
            //        cPayment = db.AdmissionDB.CandidatePayments
            //            .Where(c => c.PaymentId == paymentId)
            //            .FirstOrDefault();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //panel_Message1.Visible = true;
            //    //lblMessageSsl.Text = "Error getting candidate payment " + ex.Message;
            //    //lblMessageSsl.ForeColor = Color.Crimson;
            //    //lblMessageSsl.Font.Bold = true;


            //    return;
            //}

            //if (cPayment != null)
            //{
            //    DAL.BasicInfo candidate = null;
            //    try
            //    {
            //        using (var db = new CandidateDataManager())
            //        {
            //            candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //panel_Message1.Visible = true;
            //        //lblMessageSsl.Text = "Error getting candidate " + ex.Message;
            //        //lblMessageSsl.ForeColor = Color.Crimson;
            //        //lblMessageSsl.Font.Bold = true;
            //        MessageView("Error getting candidate " + ex.Message, "fail");
            //        return;
            //    }

            //    if (cPayment.IsPaid == true)
            //    {
            //        //panel_Message1.Visible = true;
            //        //lblMessageSsl.Text = "Candidate found. Paid : Yes";
            //        //lblMessageSsl.ForeColor = Color.Green;
            //        MessageView("Candidate found. Paid : Yes", "success");
            //        if (candidate != null)
            //        {
            //            lblCandidateName.Text = candidate.FirstName;
            //        }
            //        else
            //        {
            //            lblCandidateName.Text = "N/A";
            //            lblCandidateName.ForeColor = Color.Crimson;
            //        }
            //        GetDataFromSSL(cPayment);
            //    }
            //    else if (cPayment.IsPaid == false)
            //    {
            //        //panel_Message1.Visible = true;
            //        //lblMessageSsl.Text = "Candidate found. Paid : NO";
            //        //lblMessageSsl.ForeColor = Color.Crimson;
            //        MessageView("Candidate found. Paid : NO", "fail");
            //        if (candidate != null)
            //        {
            //            lblCandidateName.Text = candidate.FirstName;
            //        }
            //        else
            //        {
            //            lblCandidateName.Text = "N/A";
            //            lblCandidateName.ForeColor = Color.Crimson;
            //        }
            //        GetDataFromSSL(cPayment);
            //    }
            //}
            //else
            //{
            //    //panel_Message1.Visible = true;
            //    //lblMessageSsl.Text = "Candidate with PaymentID " + paymentId + " not found";
            //    //lblMessageSsl.ForeColor = Color.Crimson;
            //    MessageView("Candidate with PaymentID " + paymentId + " not found", "fail");
            //    return;
            //} 
            #endregion

        }

        private void GetDataFromSSL(CandidatePayment cPayment)
        {
            lblSearchMsg.Text = string.Empty;

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
                        lblSearchMsg.Text = "Error getting store information.";
                        lblSearchMsg.ForeColor = Color.Crimson;
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
                        lblSearchMsg.Text = "Error getting StoreId and Password.";
                        lblSearchMsg.ForeColor = Color.Crimson;
                        return;
                    }
                }
                else
                {
                    lblSearchMsg.Text = "Error: store not found.";
                    lblSearchMsg.ForeColor = Color.Crimson;
                    return;
                }
                

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
                            //btnSave.Visible = true;

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
                            lblSearchMsg.Text = "Candidate PaymentID from system does not match with Gateway PaymentID(1).";
                            lblSearchMsg.ForeColor = Color.Crimson;
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

                        lblSearchMsg.Text = error;
                        lblSearchMsg.ForeColor = Color.Crimson;
                        return;
                    }

                }//end if (url != null) 
                #endregion






                
            }
            catch (Exception ex)
            {
                lblSearchMsg.Text = "Error: Something went wrong! Exception: " + ex.Message.ToString();
                lblSearchMsg.ForeColor = Color.Crimson;
     
            }
        }

        #region N/A --GetDataFromSSL
        ///// <summary>
        ///// Get payment details from Payment Gateway provider.
        ///// </summary>
        ///// <param name="cPayment"></param>
        //private void GetDataFromSSL(DAL.CandidatePayment cPayment)
        //{
        //    //ClearBkashPanel();

        //    string candidateName = string.Empty;
        //    DAL.BasicInfo basicInfo = null;
        //    List<DAL.CandidateFormSl> cFormSlList = null;
        //    List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
        //    DAL.AdmissionSetup admSetup = null;
        //    DAL.Store store = null;
        //    DAL.StoreFoster storeFoster = null;
        //    string url = null;

        //    //1. Get form serial objects.
        //    try
        //    {
        //        using (var db = new CandidateDataManager())
        //        {
        //            cFormSlList = db.AdmissionDB.CandidateFormSls
        //                .Where(c => c.CandidatePaymentID == cPayment.ID).ToList();

        //            basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cPayment.CandidateID).FirstOrDefault();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //panel_Message1.Visible = true;
        //        //lblMessageSsl.Text = "Error getting candidate form serial information. " + ex.Message;
        //        //lblMessageSsl.ForeColor = Color.Crimson;
        //        MessageView("Error getting candidate form serial information. " + ex.Message, "fail");
        //        return;
        //    }


        //    if (basicInfo != null)
        //    {
        //        candidateName = basicInfo.FirstName;
        //    }


        //    //2. Figure out what the number of form serials indicates.
        //    if (cFormSlList.Count() > 1) // more than one candidate form serial means multiple form purchase. hence this is undergraduate candidate.
        //    {
        //        bool isMultiple = true;

        //        // get admission setup for each form serial object.
        //        foreach (var item in cFormSlList)
        //        {
        //            DAL.AdmissionSetup _temp = null;
        //            try
        //            {
        //                using (var db = new CandidateDataManager())
        //                {
        //                    _temp = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
        //                }
        //                if (_temp.EducationCategoryID == 4) // only get the undergraduate admission setup.
        //                {
        //                    admSetupList.Add(_temp);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting admission setup. " + ex.Message;
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting admission setup. " + ex.Message, "fail");
        //                return;
        //            }
        //        }

        //        if (admSetupList.Count() > 0)
        //        {
        //            foreach (var item in admSetupList)
        //            {
        //                if (item.EducationCategoryID == 6) // if graduate detected.
        //                {
        //                    isMultiple = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //panel_Message1.Visible = true;
        //            //lblMessageSsl.Text = "Error: admission setup not found.";
        //            //lblMessageSsl.ForeColor = Color.Crimson;
        //            MessageView("Error: admission setup not found.", "fail");
        //            return;
        //        }

        //        if (isMultiple == true)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    store = db.AdmissionDB.Stores.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();

        //                    storeFoster = db.AdmissionDB.StoreFosters.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting store." + ex.Message;
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting store." + ex.Message, "fail");
        //                return;
        //            }
        //        }

        //        if (storeFoster != null)
        //        {
        //            string marchentTransId = null;
        //            string securityKey = null;
        //            string md5encryptKey = null;

        //            try
        //            {
        //                //storeId = Decrypt.DecryptString(store.StoreId);
        //                //password = Decrypt.DecryptString(store.StorePass);
        //                if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //                {
        //                    string input = Convert.ToString(storeFoster.SecurityKey) + "" + Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId);
        //                    marchentTransId = Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId);
        //                    securityKey = Convert.ToString(storeFoster.SecurityKey);
        //                    md5encryptKey = MD5Hash(input);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting store information. " + ex.Message;
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting store information. " + ex.Message, "fail");
        //                return;
        //            }

        //            if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //            {
        //                //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
        //                //    , val_id, plainText_storeId, plainText_password);

        //                //url = String.Format(@"https://payment.fosterpayments.com/fosterpayments/TransactionStatus/txstatus.php?mcnt_TxnNo={0}&mcnt_SecureHashValue={1}"
        //                //    , marchentTransId, md5encryptKey);

        //                url = String.Format(@"https://payment.fosterpayments.com.bd/fosterpayments/TransactionStatus/transactionStatusApi1.2.php?mcnt_TxnNo={0}&mcnt_SecureHashValue={1}"
        //                        , marchentTransId, md5encryptKey);

        //                //string HtmlResult = null;

        //                //string URI = "https://payment.fosterpayments.com/fosterpayments/TransactionStatus/txstatus.php?";
        //                //string myParameters = "mcnt_TxnNo=" + marchentTransId + "&mcnt_SecureHashValue=" + md5encryptKey;
        //                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; ;//SecurityProtocolType.Tls12;
        //                //using (WebClient wc = new WebClient())
        //                //{
        //                //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //                //    HtmlResult = wc.UploadString(URI, myParameters);
        //                //    //Console.Write(HtmlResult); 
        //                //    var xml = XElement.Parse(Convert.ToString(HtmlResult));
        //                //    var flights = xml.DescendantsAndSelf("TxnResponse").FirstOrDefault();
        //                //}
        //            }
        //            else
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting Store.";
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting Store.", "fail");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            //panel_Message1.Visible = true;
        //            //lblMessageSsl.Text = "Error: store not found.";
        //            //lblMessageSsl.ForeColor = Color.Crimson;
        //            MessageView("Error: store not found.", "fail");
        //            return;
        //        }

        //        if (url != null)
        //        {
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; ;//SecurityProtocolType.Tls12;
        //            WebClient n = new WebClient();
        //            var jsonData = n.DownloadString(url);
        //            string[] jsonParts = Regex.Matches(jsonData, @"\{.*?\}").Cast<Match>().Select(m => m.Value).ToArray();
        //            string valueOriginal = Convert.ToString(jsonData);
        //            valueOriginal = valueOriginal.Replace("[", "");
        //            valueOriginal = valueOriginal.Replace("]", "");


        //            //https://payment.fosterpayments.com.bd/fosterpayments/TransactionStatus/transactionStatusApi1.2.php?mcnt_TxnNo=BUPUG2102401863&mcnt_SecureHashValue=b4f225da48f3b919a9431451540c2d8f
        //            //// ==== Success Response:
        //            /// <summary>
        //            /// "[{\
        //            /// "status\":\"200\",\
        //            /// "message\":\"Transaction Successfully.\",\
        //            /// "merchantTxnNo\":\"BUPUG2102401863\",\
        //            /// "txnResponse\":\"2\",\
        //            /// "txnAmount\":\"761.63\",\
        //            /// "currency\":\"BDT\",\
        //            /// "convertionRate\":\"1\",\
        //            /// "orderNo\":\"2102401863\",\
        //            /// "fosterId\":\"BUPUa58dc26.33601705\",\
        //            /// "hashkey\":\"aedeadc42de71fd886d8f0a8d9120953\",\
        //            /// "bankName\":\"bKash\",\
        //            /// "cardNumber\":\"MHBD8861612327634011\",\
        //            /// "cardHolderName\":null,\
        //            /// "cardType\":\"bKash API Mobile Banking\",\
        //            /// "bankAuthorizeId\":\"8B39AMRGJ5\",\
        //            /// "bankTransactionTime\":\"2021-02-03 10:48:02\",\
        //            /// "serviceCharge\":\"11.63000\",\
        //            /// "storeAmount\":\"750.00000\
        //            /// "}]"
        //            /// </summary>

        //            dynamic data = JObject.Parse(valueOriginal);

        //            string returned_Status = JObject.Parse(valueOriginal)["status"].ToString();
        //            string returned_Message = JObject.Parse(valueOriginal)["message"].ToString();
        //            string returned_MerchantTxnNo = JObject.Parse(valueOriginal)["merchantTxnNo"].ToString();
        //            string returned_TxnResponse = JObject.Parse(valueOriginal)["txnResponse"].ToString();
        //            string returned_Currency = JObject.Parse(valueOriginal)["currency"].ToString();
        //            string returned_ConvertionRate = JObject.Parse(valueOriginal)["convertionRate"].ToString();
        //            string returned_OrderNo = JObject.Parse(valueOriginal)["orderNo"].ToString();
        //            string returned_FosterId = JObject.Parse(valueOriginal)["fosterId"].ToString();
        //            string returned_Hashkey = JObject.Parse(valueOriginal)["hashkey"].ToString();
        //            string returned_BankName = JObject.Parse(valueOriginal)["bankName"].ToString();
        //            string returned_CardNumber = JObject.Parse(valueOriginal)["cardNumber"].ToString();
        //            string returned_CardHolderName = JObject.Parse(valueOriginal)["cardHolderName"].ToString();
        //            string returned_CardType = JObject.Parse(valueOriginal)["cardType"].ToString();
        //            string returned_BankAuthorizeId = JObject.Parse(valueOriginal)["bankAuthorizeId"].ToString();
        //            string returned_BankTransactionTime = JObject.Parse(valueOriginal)["bankTransactionTime"].ToString();
        //            string returned_ServiceCharge = JObject.Parse(valueOriginal)["serviceCharge"].ToString();
        //            string returned_StoreAmount = JObject.Parse(valueOriginal)["storeAmount"].ToString();

        //            if (returned_Status == "200")
        //            {
        //                lblTranId.Text = cPayment.PaymentId.ToString();
        //                lblCandidateName.Text = candidateName;
        //                lblStatus.Text = returned_Message;
        //                lblTransDate.Text = Convert.ToDateTime(returned_BankTransactionTime).ToString("dd/MM/yyyy");
        //                //lblBankTranId.Text = returned_CardNumber;
        //                if (returned_TxnResponse == "2")
        //                {
        //                    lblValId.Text = "Success";
        //                }
        //                else if (returned_TxnResponse == "3")
        //                {
        //                    lblValId.Text = "Fail";
        //                }
        //                else if (returned_TxnResponse == "7")
        //                {
        //                    lblValId.Text = "Cancel";
        //                }
        //                else
        //                {
        //                    lblValId.Text = "";
        //                }
        //                lblAmount.Text = returned_StoreAmount;
        //                lblReceivable.Text = "";
        //                lblCardType.Text = returned_CardType;
        //                lblCardNumber.Text = returned_CardNumber;
        //                lblIssuerBankCountry.Text = returned_BankName;
        //                lblCurrency.Text = returned_Currency;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //            }
        //            else
        //            {
        //                MessageView(returned_Message, "fail");
        //            }


        //            #region N/A
        //            //decimal systemAmount = -1;
        //            //dynamic stuff = null;

        //            //for (int i = 0; i < jsonParts.Count(); i++)
        //            //{
        //            //    dynamic temp_stuff = null;
        //            //    temp_stuff = JObject.Parse(jsonParts[i]);
        //            //    if (temp_stuff != null)
        //            //    {
        //            //        if (!string.IsNullOrEmpty(Convert.ToString(temp_stuff.TxnResponse)))
        //            //        {
        //            //            if (Convert.ToInt32(temp_stuff.TxnResponse) == 2)
        //            //            {
        //            //                stuff = temp_stuff;
        //            //            }
        //            //            else { temp_stuff = null; }
        //            //        }
        //            //        else { temp_stuff = null; }
        //            //    }
        //            //    else { temp_stuff = null; }
        //            //}

        //            //if (stuff != null)
        //            //{
        //            //    DAL.CandidatePayment candidatePaymentObj = null;
        //            //    DAL.BasicInfo candidateObj = null;

        //            //    string paymentTran_id = stuff.MerchantTxnNo;
        //            //    string paymentStatus = stuff.TxnResponse;
        //            //    string paymentAmount = stuff.TxnAmount;
        //            //    string currencyType = stuff.Currency;
        //            //    string paymentId = stuff.OrderNo;
        //            //    string foster_id = stuff.fosterid;
        //            //    string haskey = stuff.hashkey;
        //            //    string message = stuff.message;
        //            //    long candidatePaymentId = -1;
        //            //    candidatePaymentId = Int64.Parse(paymentId);
        //            //    using (var db = new CandidateDataManager())
        //            //    {
        //            //        candidatePaymentObj = db.AdmissionDB.CandidatePayments
        //            //            .Where(c => c.PaymentId == candidatePaymentId).FirstOrDefault();
        //            //    }

        //            //    if (candidatePaymentObj != null)
        //            //    {
        //            //        systemAmount = Convert.ToDecimal(candidatePaymentObj.Amount);
        //            //        using (var db = new CandidateDataManager())
        //            //        {
        //            //            candidateObj = db.GetCandidateBasicInfoByID_ND(Convert.ToInt64(candidatePaymentObj.CandidateID));
        //            //        }
        //            //    }
        //            //    else
        //            //    {

        //            //    }

        //            //    #region SSL Data
        //            //    //string paymentStatus = rss["element"][0]["status"].ToString();
        //            //    //string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
        //            //    //string paymentAmount = rss["element"][0]["amount"].ToString();
        //            //    //string storeAmount = rss["element"][0]["store_amount"].ToString();
        //            //    ////string paymentVal_id = outputArray["val_id"].ToString();
        //            //    //string paymentTran_id = rss["element"][0]["tran_id"].ToString();
        //            //    //string paymentTran_date = rss["element"][0]["tran_date"].ToString();
        //            //    //string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
        //            //    //string paymentCard_type = rss["element"][0]["card_type"].ToString();
        //            //    //string cardNumber = rss["element"][0]["card_no"].ToString();
        //            //    //string currencyType = rss["element"][0]["currency_type"].ToString();
        //            //    //string currencyAmount = rss["element"][0]["currency_amount"].ToString();
        //            //    //string cardBrand = rss["element"][0]["card_brand"].ToString();
        //            //    //string cardIssuer = rss["element"][0]["card_issuer"].ToString();
        //            //    //string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
        //            //    //string value_b = rss["element"][0]["value_b"].ToString();
        //            //    //string value_c = rss["element"][0]["value_c"].ToString();
        //            //    //string value_d = rss["element"][0]["value_d"].ToString();
        //            //    //string paymentValidated_on = outputArray["validated_on"].ToString();
        //            //    //string paymentRisk_title = outputArray["risk_title"].ToString();
        //            //    #endregion

        //            //    decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
        //            //    //if (Convert.ToInt32(paymentStatus) == 2)
        //            //    //{
        //            //    //    string paymentStatusUpper = message;
        //            //    //}

        //            //    lblAmount.Text = validateResultAmount.ToString();
        //            //    lblBankTranId.Text = null; //paymentBank_tran_id;
        //            //    lblCardNumber.Text = null; //cardNumber;
        //            //    lblCardType.Text = null; //paymentCard_type;
        //            //    lblCurrency.Text = currencyType;
        //            //    lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //            //    lblReceivable.Text = paymentAmount;
        //            //    if (Convert.ToInt32(paymentStatus) == 2 && Convert.ToDecimal(paymentAmount) >= systemAmount)
        //            //    {
        //            //        lblStatus.Text = message;
        //            //        lblStatus.ForeColor = Color.Green;
        //            //    }
        //            //    else
        //            //    {
        //            //        lblStatus.Text = message;
        //            //        lblStatus.ForeColor = Color.Crimson;
        //            //    }
        //            //    lblTranId.Text = Convert.ToString(cPayment.PaymentId);
        //            //    lblTransDate.Text = null;
        //            //}
        //            //else
        //            //{
        //            //    lblAmount.Text = null;
        //            //    lblBankTranId.Text = null; //paymentBank_tran_id;
        //            //    lblCardNumber.Text = null; //cardNumber;
        //            //    lblCardType.Text = null; //paymentCard_type;
        //            //    lblCurrency.Text = null;
        //            //    lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //            //    lblReceivable.Text = null;
        //            //    lblTranId.Text = Convert.ToString(cPayment.PaymentId);
        //            //    lblTransDate.Text = null;
        //            //    lblStatus.Text = "Transaction Failed";
        //            //    lblStatus.ForeColor = Color.Crimson;
        //            //    return;
        //            //} 
        //            #endregion
        //        }
        //        else {

        //            MessageView("No Data Found !!", "fail");
        //            return;
        //        }

        //    }
        //    else if (cFormSlList.Count() == 1) // only one candidate form serial means either multiple or single form purchase. This could be either graduate or undergraduate.
        //    {

        //        // get admission setup for each form serial object.
        //        foreach (var item in cFormSlList)
        //        {
        //            try
        //            {
        //                using (var db = new CandidateDataManager())
        //                {
        //                    admSetup = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting admission setup(1). " + ex.Message;
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting admission setup(1). " + ex.Message, "fail");
        //                return;
        //            }
        //        }

        //        if (admSetup != null)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    store = db.AdmissionDB.Stores.Find(admSetup.StoreID);

        //                    storeFoster = db.AdmissionDB.StoreFosters.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting store(1). " + ex.Message;
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting store(1). " + ex.Message, "fail");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            //panel_Message1.Visible = true;
        //            //lblMessageSsl.Text = "Error: admission setup not found(1).";
        //            //lblMessageSsl.ForeColor = Color.Crimson;
        //            MessageView("Error: admission setup not found(1).", "fail");
        //            return;
        //        }


        //        if (storeFoster != null)
        //        {
        //            string marchentTransId = null;
        //            string securityKey = null;
        //            string md5encryptKey = null;

        //            try
        //            {
        //                //storeId = Decrypt.DecryptString(store.StoreId);
        //                //password = Decrypt.DecryptString(store.StorePass);
        //                if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //                {
        //                    string input = Convert.ToString(storeFoster.SecurityKey) + "" + Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId);
        //                    marchentTransId = Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId);
        //                    securityKey = Convert.ToString(storeFoster.SecurityKey);
        //                    md5encryptKey = MD5Hash(input);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting store information. " + ex.Message;
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting store information. " + ex.Message, "fail");
        //                return;
        //            }

        //            if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //            {
        //                //url = String.Format(@"https://payment.fosterpayments.com/fosterpayments/TransactionStatus/txstatus.php?mcnt_TxnNo={0}&mcnt_SecureHashValue={1}"
        //                //    , marchentTransId, md5encryptKey);

        //                url = String.Format(@"https://payment.fosterpayments.com.bd/fosterpayments/TransactionStatus/transactionStatusApi1.2.php?mcnt_TxnNo={0}&mcnt_SecureHashValue={1}"
        //                        , marchentTransId, md5encryptKey);
        //            }
        //            else
        //            {
        //                //panel_Message1.Visible = true;
        //                //lblMessageSsl.Text = "Error getting Store.";
        //                //lblMessageSsl.ForeColor = Color.Crimson;
        //                MessageView("Error getting Store.", "fail");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            //panel_Message1.Visible = true;
        //            //lblMessageSsl.Text = "Error: store not found(1).";
        //            //lblMessageSsl.ForeColor = Color.Crimson;
        //            MessageView("Error: store not found(1).", "fail");
        //            return;
        //        }

        //        if (url != null)
        //        {
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; ;//SecurityProtocolType.Tls12;
        //            WebClient n = new WebClient();
        //            var jsonData = n.DownloadString(url);
        //            string[] jsonParts = Regex.Matches(jsonData, @"\{.*?\}").Cast<Match>().Select(m => m.Value).ToArray();
        //            string valueOriginal = Convert.ToString(jsonData);
        //            valueOriginal = valueOriginal.Replace("[", "");
        //            valueOriginal = valueOriginal.Replace("]", "");




        //            //https://payment.fosterpayments.com.bd/fosterpayments/TransactionStatus/transactionStatusApi1.2.php?mcnt_TxnNo=BUPUG2102401863&mcnt_SecureHashValue=b4f225da48f3b919a9431451540c2d8f
        //            //// ==== Success Response:
        //            /// <summary>
        //            /// "[{\
        //            /// "status\":\"200\",\
        //            /// "message\":\"Transaction Successfully.\",\
        //            /// "merchantTxnNo\":\"BUPUG2102401863\",\
        //            /// "txnResponse\":\"2\",\
        //            /// "txnAmount\":\"761.63\",\
        //            /// "currency\":\"BDT\",\
        //            /// "convertionRate\":\"1\",\
        //            /// "orderNo\":\"2102401863\",\
        //            /// "fosterId\":\"BUPUa58dc26.33601705\",\
        //            /// "hashkey\":\"aedeadc42de71fd886d8f0a8d9120953\",\
        //            /// "bankName\":\"bKash\",\
        //            /// "cardNumber\":\"MHBD8861612327634011\",\
        //            /// "cardHolderName\":null,\
        //            /// "cardType\":\"bKash API Mobile Banking\",\
        //            /// "bankAuthorizeId\":\"8B39AMRGJ5\",\
        //            /// "bankTransactionTime\":\"2021-02-03 10:48:02\",\
        //            /// "serviceCharge\":\"11.63000\",\
        //            /// "storeAmount\":\"750.00000\
        //            /// "}]"
        //            /// </summary>

        //            dynamic data = JObject.Parse(valueOriginal);

        //            string returned_Status = JObject.Parse(valueOriginal)["status"].ToString();
        //            string returned_Message = JObject.Parse(valueOriginal)["message"].ToString();
        //            string returned_MerchantTxnNo = JObject.Parse(valueOriginal)["merchantTxnNo"].ToString();
        //            string returned_TxnResponse = JObject.Parse(valueOriginal)["txnResponse"].ToString();
        //            string returned_Currency = JObject.Parse(valueOriginal)["currency"].ToString();
        //            string returned_ConvertionRate = JObject.Parse(valueOriginal)["convertionRate"].ToString();
        //            string returned_OrderNo = JObject.Parse(valueOriginal)["orderNo"].ToString();
        //            string returned_FosterId = JObject.Parse(valueOriginal)["fosterId"].ToString();
        //            string returned_Hashkey = JObject.Parse(valueOriginal)["hashkey"].ToString();
        //            string returned_BankName = JObject.Parse(valueOriginal)["bankName"].ToString();
        //            string returned_CardNumber = JObject.Parse(valueOriginal)["cardNumber"].ToString();
        //            string returned_CardHolderName = JObject.Parse(valueOriginal)["cardHolderName"].ToString();
        //            string returned_CardType = JObject.Parse(valueOriginal)["cardType"].ToString();
        //            string returned_BankAuthorizeId = JObject.Parse(valueOriginal)["bankAuthorizeId"].ToString();
        //            string returned_BankTransactionTime = JObject.Parse(valueOriginal)["bankTransactionTime"].ToString();
        //            string returned_ServiceCharge = JObject.Parse(valueOriginal)["serviceCharge"].ToString();
        //            string returned_StoreAmount = JObject.Parse(valueOriginal)["storeAmount"].ToString();

        //            if (returned_Status == "200")
        //            {
        //                lblTranId.Text = cPayment.PaymentId.ToString();
        //                lblCandidateName.Text = candidateName;
        //                lblStatus.Text = returned_Message;
        //                lblTransDate.Text = Convert.ToDateTime(returned_BankTransactionTime).ToString("dd/MM/yyyy");
        //                //lblBankTranId.Text = returned_CardNumber;
        //                if (returned_TxnResponse == "2")
        //                {
        //                    lblValId.Text = "Success";
        //                }
        //                else if (returned_TxnResponse == "3")
        //                {
        //                    lblValId.Text = "Fail";
        //                }
        //                else if (returned_TxnResponse == "7")
        //                {
        //                    lblValId.Text = "Cancel";
        //                }
        //                else
        //                {
        //                    lblValId.Text = "";
        //                }
        //                lblAmount.Text = returned_StoreAmount;
        //                lblReceivable.Text = "";
        //                lblCardType.Text = returned_CardType;
        //                lblCardNumber.Text = returned_CardNumber;
        //                lblIssuerBankCountry.Text = returned_BankName;
        //                lblCurrency.Text = returned_Currency;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //                //lblStatus.Text = returned_Message;
        //            }
        //            else
        //            {
        //                MessageView(returned_Message, "fail");
        //            }



        //            #region N/A
        //            //decimal systemAmount = -1;
        //            //dynamic stuff = null;
        //            //for (int i = 0; i < jsonParts.Count(); i++)
        //            //{
        //            //    dynamic temp_stuff = null;
        //            //    temp_stuff = JObject.Parse(jsonParts[i]);
        //            //    if (temp_stuff != null)
        //            //    {
        //            //        if (!string.IsNullOrEmpty(Convert.ToString(temp_stuff.TxnResponse)))
        //            //        {
        //            //            if (Convert.ToInt32(temp_stuff.TxnResponse) == 2)
        //            //            {
        //            //                stuff = temp_stuff;
        //            //            }
        //            //            else { temp_stuff = null; }
        //            //        }
        //            //        else { temp_stuff = null; }
        //            //    }
        //            //    else { temp_stuff = null; }
        //            //}

        //            //if (stuff != null)
        //            //{
        //            //    #region

        //            //    DAL.CandidatePayment candidatePaymentObj = null;
        //            //    DAL.BasicInfo candidateObj = null;

        //            //    string paymentTran_id = stuff.MerchantTxnNo;
        //            //    string paymentStatus = stuff.TxnResponse;
        //            //    string paymentAmount = stuff.TxnAmount;
        //            //    string currencyType = stuff.Currency;
        //            //    string paymentId = stuff.OrderNo;
        //            //    string foster_id = stuff.fosterid;
        //            //    string haskey = stuff.hashkey;
        //            //    string message = stuff.message;
        //            //    long candidatePaymentId = -1;

        //            //    candidatePaymentId = Int64.Parse(paymentId);
        //            //    using (var db = new CandidateDataManager())
        //            //    {
        //            //        candidatePaymentObj = db.AdmissionDB.CandidatePayments
        //            //            .Where(c => c.PaymentId == candidatePaymentId).FirstOrDefault();
        //            //    }

        //            //    if (candidatePaymentObj != null)
        //            //    {
        //            //        systemAmount = Convert.ToDecimal(candidatePaymentObj.Amount);
        //            //        using (var db = new CandidateDataManager())
        //            //        {
        //            //            candidateObj = db.GetCandidateBasicInfoByID_ND(Convert.ToInt64(candidatePaymentObj.CandidateID));
        //            //        }
        //            //    }


        //            //    #region SSL Data
        //            //    //string paymentStatus = rss["element"][0]["status"].ToString();
        //            //    //string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
        //            //    //string paymentAmount = rss["element"][0]["amount"].ToString();
        //            //    //string storeAmount = rss["element"][0]["store_amount"].ToString();
        //            //    ////string paymentVal_id = outputArray["val_id"].ToString();
        //            //    //string paymentTran_id = rss["element"][0]["tran_id"].ToString();
        //            //    //string paymentTran_date = rss["element"][0]["tran_date"].ToString();
        //            //    //string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
        //            //    //string paymentCard_type = rss["element"][0]["card_type"].ToString();
        //            //    //string cardNumber = rss["element"][0]["card_no"].ToString();
        //            //    //string currencyType = rss["element"][0]["currency_type"].ToString();
        //            //    //string currencyAmount = rss["element"][0]["currency_amount"].ToString();
        //            //    //string cardBrand = rss["element"][0]["card_brand"].ToString();
        //            //    //string cardIssuer = rss["element"][0]["card_issuer"].ToString();
        //            //    //string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
        //            //    //string value_b = rss["element"][0]["value_b"].ToString();
        //            //    //string value_c = rss["element"][0]["value_c"].ToString();
        //            //    //string value_d = rss["element"][0]["value_d"].ToString();
        //            //    //string paymentValidated_on = outputArray["validated_on"].ToString();
        //            //    //string paymentRisk_title = outputArray["risk_title"].ToString();
        //            //    #endregion

        //            //    decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
        //            //    if (Convert.ToInt32(paymentStatus) == 2)
        //            //    {
        //            //        string paymentStatusUpper = message;
        //            //    }

        //            //    lblAmount.Text = validateResultAmount.ToString();
        //            //    lblBankTranId.Text = null; //paymentBank_tran_id;
        //            //    lblCardNumber.Text = null; //cardNumber;
        //            //    lblCardType.Text = null; //paymentCard_type;
        //            //    lblCurrency.Text = currencyType;
        //            //    lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //            //    lblReceivable.Text = paymentAmount;
        //            //    if (Convert.ToInt32(paymentStatus) == 2 && Convert.ToDecimal(paymentAmount) >= systemAmount)
        //            //    {
        //            //        lblStatus.Text = message;
        //            //        lblStatus.ForeColor = Color.Green;
        //            //    }
        //            //    else
        //            //    {
        //            //        lblStatus.Text = message;
        //            //        lblStatus.ForeColor = Color.Crimson;
        //            //    }
        //            //    lblTranId.Text = Convert.ToString(cPayment.PaymentId);
        //            //    lblTransDate.Text = null;
        //            //    #endregion
        //            //}
        //            //else
        //            //{
        //            //    lblAmount.Text = null;
        //            //    lblBankTranId.Text = null; //paymentBank_tran_id;
        //            //    lblCardNumber.Text = null; //cardNumber;
        //            //    lblCardType.Text = null; //paymentCard_type;
        //            //    lblCurrency.Text = null;
        //            //    lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //            //    lblReceivable.Text = null;
        //            //    lblTranId.Text = Convert.ToString(cPayment.PaymentId);
        //            //    lblTransDate.Text = null;
        //            //    lblStatus.Text = "Transaction Failed";
        //            //    lblStatus.ForeColor = Color.Crimson;
        //            //    return;
        //            //} 
        //            #endregion
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    else // candidate serial not available. could be an error.
        //    {
        //        //panel_Message1.Visible = true;
        //        //lblMessageSsl.Text = "Candidate Form serial not found.";
        //        //lblMessageSsl.ForeColor = Color.Crimson;
        //        MessageView("Candidate Form serial not found.", "fail");
        //        return;
        //    }
        //} 
        #endregion

        private string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        protected void btnLoadPaymentBkash_Click(object sender, EventArgs e)
        {

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
                        //#region Log Insert EkPaySearch_HD_01
                        //try
                        //{


                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        //    dLog.DateTime = DateTime.Now;
                        //    dLog.DateCreated = DateTime.Now;
                        //    dLog.UserId = uId;
                        //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                        //    //dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                        //    dLog.CandidateId = cp.CandidateID;
                        //    dLog.EventName = "ekpaysearch_hd";
                        //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                        //    //dLog.OldData = logOldObject;
                        //    dLog.NewData = "EkPaySearch_HD_01: Payment is Updated in Admission System; Data: " + JsonConvert.SerializeObject(cp).ToString();
                        //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        //    LogWriter.DataLogWriter(dLog);
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //#endregion

                        MessageView("Candidate payment is already updated.", "success");
                    }
                    else
                    {

                        //#region Log Insert EkPaySearch_02
                        //try
                        //{


                        //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        //    dLog.DateTime = DateTime.Now;
                        //    dLog.DateCreated = DateTime.Now;
                        //    dLog.UserId = uId;
                        //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                        //    //dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                        //    dLog.CandidateId = cp.CandidateID;
                        //    dLog.EventName = "ekpaysearch_hd";
                        //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                        //    //dLog.OldData = logOldObject;
                        //    dLog.NewData = "EkPaySearch_HD_02: Payment is Not Updated in Admission System; Data: " + JsonConvert.SerializeObject(cp).ToString();
                        //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        //    LogWriter.DataLogWriter(dLog);
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //#endregion


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

                            //#region Log Insert EkPaySearch_03
                            //try
                            //{


                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //    dLog.DateTime = DateTime.Now;
                            //    dLog.DateCreated = DateTime.Now;
                            //    dLog.UserId = uId;
                            //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                            //    dLog.Attribute2 = oca.TransactionId;
                            //    dLog.CandidateId = cp.CandidateID;
                            //    dLog.EventName = "ekpaysearch_hd";
                            //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                            //    //dLog.OldData = logOldObject;
                            //    dLog.NewData = "EkPaySearch_HD_03: Get Online Collection Attempt Transaction Data; Data: " + JsonConvert.SerializeObject(oca).ToString();
                            //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            //    LogWriter.DataLogWriter(dLog);
                            //}
                            //catch (Exception ex)
                            //{
                            //}
                            //#endregion


                            #region Get Store Info
                            string ekPayBaseURL = "";
                            string ekPaySubURL = "";
                            string ekPayURLVersion = "";


                            DAL.StoreEkPay storeEkPay = null;
                            using (var db = new OfficeDataManager())
                            {
                                storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                            }

                            //#region Log Insert EkPaySearch_04
                            //try
                            //{


                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //    dLog.DateTime = DateTime.Now;
                            //    dLog.DateCreated = DateTime.Now;
                            //    dLog.UserId = uId;
                            //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                            //    dLog.Attribute2 = oca.TransactionId;
                            //    dLog.CandidateId = cp.CandidateID;
                            //    dLog.EventName = "ekpaysearch_hd";
                            //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                            //    //dLog.OldData = logOldObject;
                            //    dLog.NewData = "EkPaySearch_HD_04: Get EkPay Store Info; Data: " + JsonConvert.SerializeObject(storeEkPay).ToString();
                            //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            //    LogWriter.DataLogWriter(dLog);
                            //}
                            //catch (Exception ex)
                            //{
                            //}
                            //#endregion

                            ekPayBaseURL = storeEkPay.BaseURL.ToString();
                            ekPaySubURL = storeEkPay.SubURL.ToString();
                            ekPayURLVersion = storeEkPay.URLVersion.ToString();

                            #endregion

                            EkPayGetModel getModel = new EkPayGetModel();
                            getModel.trnx_id = oca.TransactionId;
                            getModel.trans_date = getModel.trans_date = Convert.ToDateTime(oca.CreatedDate).ToString("yyyy-MM-dd"); //DateTime.Now.ToString("yyyy-MM-dd"); // token newer time a jei datetime disi oita dite hobe

                            EkPayPaymentGateway ekPayPG = new EkPayPaymentGateway(ekPayBaseURL, ekPaySubURL, ekPayURLVersion, "/get-status");
                            ResponseEkPay responseEkPay = ekPayPG.SearchByTransactionId(getModel);

                            //#region Log Insert EkPaySearch_05
                            //try
                            //{


                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //    dLog.DateTime = DateTime.Now;
                            //    dLog.DateCreated = DateTime.Now;
                            //    dLog.UserId = uId;
                            //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                            //    dLog.Attribute2 = oca.TransactionId;
                            //    dLog.CandidateId = cp.CandidateID;
                            //    dLog.EventName = "ekpaysearch_hd";
                            //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                            //    //dLog.OldData = logOldObject;
                            //    dLog.NewData = "EkPaySearch_HD_05: Payment Response From EkPay; Data: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                            //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            //    LogWriter.DataLogWriter(dLog);
                            //}
                            //catch (Exception ex)
                            //{
                            //}
                            //#endregion

                            if (responseEkPay.ResponseCode == 200)
                            {
                                var data = (JObject)JsonConvert.DeserializeObject(responseEkPay.ResponseData.ToString());

                                string msgcode = data["msg_code"].ToString();
                                string msgdet = data["msg_det"].ToString();
                                //string custname = data["cust_info"].Value<string>("cust_name");

                                if (msgcode == "1020")
                                {
                                    //#region Log Insert EkPaySearch_06
                                    //try
                                    //{


                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateTime = DateTime.Now;
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                    //    dLog.Attribute2 = oca.TransactionId;
                                    //    dLog.CandidateId = cp.CandidateID;
                                    //    dLog.EventName = "ekpaysearch_hd";
                                    //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                                    //    //dLog.OldData = logOldObject;
                                    //    dLog.NewData = "EkPaySearch_HD_06: Payment Is Updated In EkPay";
                                    //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

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
                                    //#region Log Insert EkPaySearch_07
                                    //try
                                    //{


                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateTime = DateTime.Now;
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                    //    dLog.Attribute2 = oca.TransactionId;
                                    //    dLog.CandidateId = cp.CandidateID;
                                    //    dLog.EventName = "ekpaysearch_hd";
                                    //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                                    //    //dLog.OldData = logOldObject;
                                    //    dLog.NewData = "EkPaySearch_HD_07: Payment Is Not Updated In EkPay; Data: " + JsonConvert.SerializeObject(data).ToString();
                                    //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

                                    //payment is not updated in EkPay
                                    MessageView("Payment is not updated in EkPay! Error: " + msgdet, "fail");
                                }


                            }
                            else
                            {
                                //#region Log Insert EkPaySearch_08
                                //try
                                //{


                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateTime = DateTime.Now;
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                                //    dLog.Attribute2 = oca.TransactionId;
                                //    dLog.CandidateId = cp.CandidateID;
                                //    dLog.EventName = "ekpaysearch_hd";
                                //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                                //    //dLog.OldData = logOldObject;
                                //    dLog.NewData = "EkPaySearch_HD_08: Payment success not found from EkPay; Data: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                                //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion

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
                            //#region Log Insert EkPaySearch_09
                            //try
                            //{


                            //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //    dLog.DateTime = DateTime.Now;
                            //    dLog.DateCreated = DateTime.Now;
                            //    dLog.UserId = uId;
                            //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                            //    dLog.Attribute2 = oca.TransactionId;
                            //    dLog.CandidateId = cp.CandidateID;
                            //    dLog.EventName = "ekpaysearch_hd";
                            //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                            //    //dLog.OldData = logOldObject;
                            //    dLog.NewData = "EkPaySearch_HD_09: No transaction id found for payment id; PaymentID: " + paymentId.ToString();
                            //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            //    LogWriter.DataLogWriter(dLog);
                            //}
                            //catch (Exception ex)
                            //{
                            //}
                            //#endregion

                            MessageView("No transaction id found for payment id !", "fail");
                        }
                    }
                }
                else
                {
                    //#region Log Insert EkPaySearch_10
                    //try
                    //{


                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    //    dLog.DateTime = DateTime.Now;
                    //    dLog.DateCreated = DateTime.Now;
                    //    dLog.UserId = uId;
                    //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                    //    //dLog.Attribute2 = oca.TransactionId;
                    //    //dLog.CandidateId = cp.CandidateID;
                    //    dLog.EventName = "ekpaysearch_hd";
                    //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                    //    //dLog.OldData = logOldObject;
                    //    dLog.NewData = "EkPaySearch_HD_10: Please provide valid payment id; PaymentID: " + ekPaySearchPaymentIdString.ToString();
                    //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                    //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    //    LogWriter.DataLogWriter(dLog);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //#endregion
                    MessageView("Please provide valid payment id!", "fail");
                }
            }
            catch (Exception ex)
            {
                //#region Log Insert EkPaySearch_11
                //try
                //{


                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                //    dLog.DateTime = DateTime.Now;
                //    dLog.DateCreated = DateTime.Now;
                //    dLog.UserId = uId;
                //    dLog.Attribute1 = ekPaySearchPaymentIdString.ToString();
                //    //dLog.Attribute2 = oca.TransactionId;
                //    //dLog.CandidateId = cp.CandidateID;
                //    dLog.EventName = "ekpaysearch_hd";
                //    dLog.PageName = "CheckPaymentFromGatewayHelpDesk.aspx";
                //    //dLog.OldData = logOldObject;
                //    dLog.NewData = "EkPaySearch_HD_11: Exception: " + ex.Message.ToString();
                //    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                //    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                //    LogWriter.DataLogWriter(dLog);
                //}
                //catch (Exception ex1)
                //{
                //}
                //#endregion

                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }










        #region N/A
        //private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        //long uId = 0;
        //string uRole = string.Empty;

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
        //    uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

        //    if (!IsPostBack)
        //    {

        //    }
        //}

        //private void ClearMessage()
        //{
        //    lblMessageSsl.Text = "";
        //}

        //private void ClearSslPanel()
        //{
        //    //txtPaymentIdSsl.Text = string.Empty;

        //    panel_Message1.Visible = false;

        //    lblTranId.Text = string.Empty;
        //    lblCandidateName.Text = string.Empty;
        //    lblStatus.Text = string.Empty;
        //    lblTransDate.Text = string.Empty;
        //    lblBankTranId.Text = string.Empty;
        //    lblValId.Text = string.Empty;
        //    lblAmount.Text = string.Empty;
        //    lblReceivable.Text = string.Empty;
        //    lblCardType.Text = string.Empty;
        //    lblCardNumber.Text = string.Empty;
        //    lblIssuerBankCountry.Text = string.Empty;
        //    lblCurrency.Text = string.Empty;
        //}

        //private void ClearBkashPanel()
        //{
        //    //txtTrxIdBkash.Text = null;

        //    //panel_Message2.Visible = false;

        //    //lblNameB.Text = string.Empty;
        //    //lblSysAmntB.Text = string.Empty;
        //    //lblStatusCodeB.Text = string.Empty;
        //    //lblStatusDetailsB.Text = string.Empty;
        //    //lblTrxIdB.Text = string.Empty;
        //    //lblReferenceNoB.Text = string.Empty;
        //    //lblCounterB.Text = string.Empty;
        //    //lblPaidAmntB.Text = string.Empty;
        //    //lblSenderB.Text = string.Empty;
        //    //lblServiceB.Text = string.Empty;
        //    //lblCurrencyB.Text = string.Empty;
        //    //lblReceiverB.Text = string.Empty;
        //}

        //protected void btnLoadSsl_Click(object sender, EventArgs e)
        //{
        //    ClearMessage();
        //    ClearSslPanel();


        //    long paymentId = -1;
        //    if (!string.IsNullOrEmpty(txtPaymentIdSsl.Text.Trim()))
        //    {
        //        paymentId = Int64.Parse(txtPaymentIdSsl.Text);
        //    }

        //    DAL.CandidatePayment cPayment = null;
        //    try
        //    {
        //        using (var db = new OfficeDataManager())
        //        {
        //            cPayment = db.AdmissionDB.CandidatePayments
        //                .Where(c => c.PaymentId == paymentId)
        //                .FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        panel_Message1.Visible = true;
        //        lblMessageSsl.Text = "Error getting candidate payment " + ex.Message;
        //        lblMessageSsl.ForeColor = Color.Crimson;
        //        lblMessageSsl.Font.Bold = true;
        //        return;
        //    }

        //    if (cPayment != null)
        //    {
        //        DAL.BasicInfo candidate = null;
        //        try
        //        {
        //            using (var db = new CandidateDataManager())
        //            {
        //                candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            panel_Message1.Visible = true;
        //            lblMessageSsl.Text = "Error getting candidate " + ex.Message;
        //            lblMessageSsl.ForeColor = Color.Crimson;
        //            lblMessageSsl.Font.Bold = true;
        //            return;
        //        }

        //        if (cPayment.IsPaid == true)
        //        {
        //            panel_Message1.Visible = true;
        //            lblMessageSsl.Text = "Candidate found. Paid : Yes";
        //            lblMessageSsl.ForeColor = Color.Green;
        //            if (candidate != null)
        //            {
        //                lblCandidateName.Text = candidate.FirstName;
        //            }
        //            else
        //            {
        //                lblCandidateName.Text = "N/A";
        //                lblCandidateName.ForeColor = Color.Crimson;
        //            }
        //            GetDataFromSSL(cPayment);
        //        }
        //        else if (cPayment.IsPaid == false)
        //        {
        //            panel_Message1.Visible = true;
        //            lblMessageSsl.Text = "Candidate found. Paid : NO";
        //            lblMessageSsl.ForeColor = Color.Crimson;
        //            if (candidate != null)
        //            {
        //                lblCandidateName.Text = candidate.FirstName;
        //            }
        //            else
        //            {
        //                lblCandidateName.Text = "N/A";
        //                lblCandidateName.ForeColor = Color.Crimson;
        //            }
        //            GetDataFromSSL(cPayment);
        //        }
        //    }
        //    else
        //    {
        //        panel_Message1.Visible = true;
        //        lblMessageSsl.Text = "Candidate with PaymentID " + paymentId + " not found";
        //        lblMessageSsl.ForeColor = Color.Crimson;
        //        return;
        //    }

        //}

        ///// <summary>
        ///// Get payment details from Payment Gateway provider.
        ///// </summary>
        ///// <param name="cPayment"></param>
        //private void GetDataFromSSL(DAL.CandidatePayment cPayment)
        //{
        //    ClearBkashPanel();

        //    List<DAL.CandidateFormSl> cFormSlList = null;
        //    List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
        //    DAL.AdmissionSetup admSetup = null;
        //    DAL.Store store = null;
        //    DAL.StoreFoster storeFoster = null;
        //    string url = null;

        //    //1. Get form serial objects.
        //    try
        //    {
        //        using (var db = new CandidateDataManager())
        //        {
        //            cFormSlList = db.AdmissionDB.CandidateFormSls
        //                .Where(c => c.CandidatePaymentID == cPayment.ID).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        panel_Message1.Visible = true;
        //        lblMessageSsl.Text = "Error getting candidate form serial information. " + ex.Message;
        //        lblMessageSsl.ForeColor = Color.Crimson;
        //        return;
        //    }

        //    //2. Figure out what the number of form serials indicates.
        //    if (cFormSlList.Count() > 1) // more than one candidate form serial means multiple form purchase. hence this is undergraduate candidate.
        //    {
        //        bool isMultiple = true;

        //        // get admission setup for each form serial object.
        //        foreach (var item in cFormSlList)
        //        {
        //            DAL.AdmissionSetup _temp = null;
        //            try
        //            {
        //                using (var db = new CandidateDataManager())
        //                {
        //                    _temp = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
        //                }
        //                if (_temp.EducationCategoryID == 4) // only get the undergraduate admission setup.
        //                {
        //                    admSetupList.Add(_temp);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting admission setup. " + ex.Message;
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }

        //        if (admSetupList.Count() > 0)
        //        {
        //            foreach (var item in admSetupList)
        //            {
        //                if (item.EducationCategoryID == 6) // if graduate detected.
        //                {
        //                    isMultiple = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            panel_Message1.Visible = true;
        //            lblMessageSsl.Text = "Error: admission setup not found.";
        //            lblMessageSsl.ForeColor = Color.Crimson;
        //            return;
        //        }

        //        if (isMultiple == true)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    store = db.AdmissionDB.Stores.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();

        //                    storeFoster = db.AdmissionDB.StoreFosters.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting store." + ex.Message;
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }

        //        if (storeFoster != null)
        //        {
        //            string marchentTransId = null;
        //            string securityKey = null;
        //            string md5encryptKey = null;

        //            try
        //            {
        //                //storeId = Decrypt.DecryptString(store.StoreId);
        //                //password = Decrypt.DecryptString(store.StorePass);
        //                if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //                {
        //                    string input = Convert.ToString(storeFoster.SecurityKey) + "" + Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId);
        //                    marchentTransId = Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId);
        //                    securityKey = Convert.ToString(storeFoster.SecurityKey);
        //                    md5encryptKey = MD5Hash(input);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting store information. " + ex.Message;
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }

        //            if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //            {
        //                //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
        //                //    , val_id, plainText_storeId, plainText_password);

        //                url = String.Format(@"https://payment.fosterpayments.com/fosterpayments/TransactionStatus/txstatus.php?mcnt_TxnNo={0}&mcnt_SecureHashValue={1}"
        //                    , marchentTransId, md5encryptKey);

        //                //string HtmlResult = null;

        //                //string URI = "https://payment.fosterpayments.com/fosterpayments/TransactionStatus/txstatus.php?";
        //                //string myParameters = "mcnt_TxnNo=" + marchentTransId + "&mcnt_SecureHashValue=" + md5encryptKey;
        //                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; ;//SecurityProtocolType.Tls12;
        //                //using (WebClient wc = new WebClient())
        //                //{
        //                //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //                //    HtmlResult = wc.UploadString(URI, myParameters);
        //                //    //Console.Write(HtmlResult); 
        //                //    var xml = XElement.Parse(Convert.ToString(HtmlResult));
        //                //    var flights = xml.DescendantsAndSelf("TxnResponse").FirstOrDefault();
        //                //}
        //            }
        //            else
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting Store.";
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            panel_Message1.Visible = true;
        //            lblMessageSsl.Text = "Error: store not found.";
        //            lblMessageSsl.ForeColor = Color.Crimson;
        //            return;
        //        }

        //        if (url != null)
        //        {
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; ;//SecurityProtocolType.Tls12;
        //            WebClient n = new WebClient();
        //            var jsonData = n.DownloadString(url);
        //            string[] jsonParts = Regex.Matches(jsonData, @"\{.*?\}").Cast<Match>().Select(m => m.Value).ToArray();
        //            string valueOriginal = Convert.ToString(jsonData);
        //            valueOriginal = valueOriginal.Replace("[", "");
        //            valueOriginal = valueOriginal.Replace("]", "");

        //            decimal systemAmount = -1;
        //            dynamic stuff = null;

        //            for (int i = 0; i < jsonParts.Count(); i++)
        //            {
        //                dynamic temp_stuff = null;
        //                temp_stuff = JObject.Parse(jsonParts[i]);
        //                if (temp_stuff != null)
        //                {
        //                    if (!string.IsNullOrEmpty(Convert.ToString(temp_stuff.TxnResponse)))
        //                    {
        //                        if (Convert.ToInt32(temp_stuff.TxnResponse) == 2)
        //                        {
        //                            stuff = temp_stuff;
        //                        }
        //                        else { temp_stuff = null; }
        //                    }
        //                    else { temp_stuff = null; }
        //                }
        //                else { temp_stuff = null; }
        //            }

        //            if (stuff != null)
        //            {
        //                DAL.CandidatePayment candidatePaymentObj = null;
        //                DAL.BasicInfo candidateObj = null;

        //                string paymentTran_id = stuff.MerchantTxnNo;
        //                string paymentStatus = stuff.TxnResponse;
        //                string paymentAmount = stuff.TxnAmount;
        //                string currencyType = stuff.Currency;
        //                string paymentId = stuff.OrderNo;
        //                string foster_id = stuff.fosterid;
        //                string haskey = stuff.hashkey;
        //                string message = stuff.message;
        //                long candidatePaymentId = -1;
        //                candidatePaymentId = Int64.Parse(paymentId);
        //                using (var db = new CandidateDataManager())
        //                {
        //                    candidatePaymentObj = db.AdmissionDB.CandidatePayments
        //                        .Where(c => c.PaymentId == candidatePaymentId).FirstOrDefault();
        //                }

        //                if (candidatePaymentObj != null)
        //                {
        //                    systemAmount = Convert.ToDecimal(candidatePaymentObj.Amount);
        //                    using (var db = new CandidateDataManager())
        //                    {
        //                        candidateObj = db.GetCandidateBasicInfoByID_ND(Convert.ToInt64(candidatePaymentObj.CandidateID));
        //                    }
        //                }
        //                else
        //                {

        //                }

        //                #region SSL Data
        //                //string paymentStatus = rss["element"][0]["status"].ToString();
        //                //string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
        //                //string paymentAmount = rss["element"][0]["amount"].ToString();
        //                //string storeAmount = rss["element"][0]["store_amount"].ToString();
        //                ////string paymentVal_id = outputArray["val_id"].ToString();
        //                //string paymentTran_id = rss["element"][0]["tran_id"].ToString();
        //                //string paymentTran_date = rss["element"][0]["tran_date"].ToString();
        //                //string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
        //                //string paymentCard_type = rss["element"][0]["card_type"].ToString();
        //                //string cardNumber = rss["element"][0]["card_no"].ToString();
        //                //string currencyType = rss["element"][0]["currency_type"].ToString();
        //                //string currencyAmount = rss["element"][0]["currency_amount"].ToString();
        //                //string cardBrand = rss["element"][0]["card_brand"].ToString();
        //                //string cardIssuer = rss["element"][0]["card_issuer"].ToString();
        //                //string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
        //                //string value_b = rss["element"][0]["value_b"].ToString();
        //                //string value_c = rss["element"][0]["value_c"].ToString();
        //                //string value_d = rss["element"][0]["value_d"].ToString();
        //                //string paymentValidated_on = outputArray["validated_on"].ToString();
        //                //string paymentRisk_title = outputArray["risk_title"].ToString();
        //                #endregion

        //                decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
        //                //if (Convert.ToInt32(paymentStatus) == 2)
        //                //{
        //                //    string paymentStatusUpper = message;
        //                //}

        //                lblAmount.Text = validateResultAmount.ToString();
        //                lblBankTranId.Text = null; //paymentBank_tran_id;
        //                lblCardNumber.Text = null; //cardNumber;
        //                lblCardType.Text = null; //paymentCard_type;
        //                lblCurrency.Text = currencyType;
        //                lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //                lblReceivable.Text = paymentAmount;
        //                if (Convert.ToInt32(paymentStatus) == 2 && Convert.ToDecimal(paymentAmount) >= systemAmount)
        //                {
        //                    lblStatus.Text = message;
        //                    lblStatus.ForeColor = Color.Green;
        //                }
        //                else
        //                {
        //                    lblStatus.Text = message;
        //                    lblStatus.ForeColor = Color.Crimson;
        //                }
        //                lblTranId.Text = paymentTran_id;
        //                lblTransDate.Text = null;
        //            }
        //            else
        //            {
        //                lblAmount.Text = null;
        //                lblBankTranId.Text = null; //paymentBank_tran_id;
        //                lblCardNumber.Text = null; //cardNumber;
        //                lblCardType.Text = null; //paymentCard_type;
        //                lblCurrency.Text = null;
        //                lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //                lblReceivable.Text = null;
        //                lblTranId.Text = Convert.ToString(cPayment.PaymentId);
        //                lblTransDate.Text = null;
        //                lblStatus.Text = "Transaction Failed";
        //                lblStatus.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }
        //        else { return; }
        //    }
        //    else if (cFormSlList.Count() == 1) // only one candidate form serial means either multiple or single form purchase. This could be either graduate or undergraduate.
        //    {

        //        // get admission setup for each form serial object.
        //        foreach (var item in cFormSlList)
        //        {
        //            try
        //            {
        //                using (var db = new CandidateDataManager())
        //                {
        //                    admSetup = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting admission setup(1). " + ex.Message;
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }

        //        if (admSetup != null)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    store = db.AdmissionDB.Stores.Find(admSetup.StoreID);

        //                    storeFoster = db.AdmissionDB.StoreFosters.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting store(1). " + ex.Message;
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            panel_Message1.Visible = true;
        //            lblMessageSsl.Text = "Error: admission setup not found(1).";
        //            lblMessageSsl.ForeColor = Color.Crimson;
        //            return;
        //        }


        //        if (storeFoster != null)
        //        {
        //            string marchentTransId = null;
        //            string securityKey = null;
        //            string md5encryptKey = null;

        //            try
        //            {
        //                //storeId = Decrypt.DecryptString(store.StoreId);
        //                //password = Decrypt.DecryptString(store.StorePass);
        //                if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //                {
        //                    string input = Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId) + "" + Convert.ToString(storeFoster.SecurityKey);
        //                    marchentTransId = Convert.ToString(storeFoster.MerchantShortName) + "" + Convert.ToString(cPayment.PaymentId);
        //                    securityKey = Convert.ToString(storeFoster.SecurityKey);
        //                    md5encryptKey = MD5Hash(input);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting store information. " + ex.Message;
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }

        //            if (!string.IsNullOrEmpty(storeFoster.SecurityKey) && cPayment.PaymentId > 0)
        //            {
        //                url = String.Format(@"https://payment.fosterpayments.com/fosterpayments/TransactionStatus/txstatus.php?mcnt_TxnNo={0}&mcnt_SecureHashValue={1}"
        //                    , marchentTransId, md5encryptKey);
        //            }
        //            else
        //            {
        //                panel_Message1.Visible = true;
        //                lblMessageSsl.Text = "Error getting Store.";
        //                lblMessageSsl.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            panel_Message1.Visible = true;
        //            lblMessageSsl.Text = "Error: store not found(1).";
        //            lblMessageSsl.ForeColor = Color.Crimson;
        //            return;
        //        }

        //        if (url != null)
        //        {
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; ;//SecurityProtocolType.Tls12;
        //            WebClient n = new WebClient();
        //            var jsonData = n.DownloadString(url);
        //            string[] jsonParts = Regex.Matches(jsonData, @"\{.*?\}").Cast<Match>().Select(m => m.Value).ToArray();
        //            string valueOriginal = Convert.ToString(jsonData);
        //            valueOriginal = valueOriginal.Replace("[", "");
        //            valueOriginal = valueOriginal.Replace("]", "");

        //            decimal systemAmount = -1;
        //            dynamic stuff = null;

        //            for (int i = 0; i < jsonParts.Count(); i++)
        //            {
        //                dynamic temp_stuff = null;
        //                temp_stuff = JObject.Parse(jsonParts[i]);
        //                if (temp_stuff != null)
        //                {
        //                    if (!string.IsNullOrEmpty(Convert.ToString(temp_stuff.TxnResponse)))
        //                    {
        //                        if (Convert.ToInt32(temp_stuff.TxnResponse) == 2)
        //                        {
        //                            stuff = temp_stuff;
        //                        }
        //                        else { temp_stuff = null; }
        //                    }
        //                    else { temp_stuff = null; }
        //                }
        //                else { temp_stuff = null; }
        //            }

        //            if (stuff != null)
        //            {
        //                #region

        //                DAL.CandidatePayment candidatePaymentObj = null;
        //                DAL.BasicInfo candidateObj = null;

        //                string paymentTran_id = stuff.MerchantTxnNo;
        //                string paymentStatus = stuff.TxnResponse;
        //                string paymentAmount = stuff.TxnAmount;
        //                string currencyType = stuff.Currency;
        //                string paymentId = stuff.OrderNo;
        //                string foster_id = stuff.fosterid;
        //                string haskey = stuff.hashkey;
        //                string message = stuff.message;
        //                long candidatePaymentId = -1;

        //                candidatePaymentId = Int64.Parse(paymentId);
        //                using (var db = new CandidateDataManager())
        //                {
        //                    candidatePaymentObj = db.AdmissionDB.CandidatePayments
        //                        .Where(c => c.PaymentId == candidatePaymentId).FirstOrDefault();
        //                }

        //                if (candidatePaymentObj != null)
        //                {
        //                    systemAmount = Convert.ToDecimal(candidatePaymentObj.Amount);
        //                    using (var db = new CandidateDataManager())
        //                    {
        //                        candidateObj = db.GetCandidateBasicInfoByID_ND(Convert.ToInt64(candidatePaymentObj.CandidateID));
        //                    }
        //                }


        //                #region SSL Data
        //                //string paymentStatus = rss["element"][0]["status"].ToString();
        //                //string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
        //                //string paymentAmount = rss["element"][0]["amount"].ToString();
        //                //string storeAmount = rss["element"][0]["store_amount"].ToString();
        //                ////string paymentVal_id = outputArray["val_id"].ToString();
        //                //string paymentTran_id = rss["element"][0]["tran_id"].ToString();
        //                //string paymentTran_date = rss["element"][0]["tran_date"].ToString();
        //                //string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
        //                //string paymentCard_type = rss["element"][0]["card_type"].ToString();
        //                //string cardNumber = rss["element"][0]["card_no"].ToString();
        //                //string currencyType = rss["element"][0]["currency_type"].ToString();
        //                //string currencyAmount = rss["element"][0]["currency_amount"].ToString();
        //                //string cardBrand = rss["element"][0]["card_brand"].ToString();
        //                //string cardIssuer = rss["element"][0]["card_issuer"].ToString();
        //                //string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
        //                //string value_b = rss["element"][0]["value_b"].ToString();
        //                //string value_c = rss["element"][0]["value_c"].ToString();
        //                //string value_d = rss["element"][0]["value_d"].ToString();
        //                //string paymentValidated_on = outputArray["validated_on"].ToString();
        //                //string paymentRisk_title = outputArray["risk_title"].ToString();
        //                #endregion

        //                decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
        //                if (Convert.ToInt32(paymentStatus) == 2)
        //                {
        //                    string paymentStatusUpper = message;
        //                }

        //                lblAmount.Text = validateResultAmount.ToString();
        //                lblBankTranId.Text = null; //paymentBank_tran_id;
        //                lblCardNumber.Text = null; //cardNumber;
        //                lblCardType.Text = null; //paymentCard_type;
        //                lblCurrency.Text = currencyType;
        //                lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //                lblReceivable.Text = paymentAmount;
        //                if (Convert.ToInt32(paymentStatus) == 2 && Convert.ToDecimal(paymentAmount) >= systemAmount)
        //                {
        //                    lblStatus.Text = message;
        //                    lblStatus.ForeColor = Color.Green;
        //                }
        //                else
        //                {
        //                    lblStatus.Text = message;
        //                    lblStatus.ForeColor = Color.Crimson;
        //                }
        //                lblTranId.Text = paymentTran_id;
        //                lblTransDate.Text = null;
        //                #endregion
        //            }
        //            else
        //            {
        //                lblAmount.Text = null;
        //                lblBankTranId.Text = null; //paymentBank_tran_id;
        //                lblCardNumber.Text = null; //cardNumber;
        //                lblCardType.Text = null; //paymentCard_type;
        //                lblCurrency.Text = null;
        //                lblIssuerBankCountry.Text = null; //cardIssuer + " " + cardIssuerCountry;
        //                lblReceivable.Text = null;
        //                lblTranId.Text = Convert.ToString(cPayment.PaymentId);
        //                lblTransDate.Text = null;
        //                lblStatus.Text = "Transaction Failed";
        //                lblStatus.ForeColor = Color.Crimson;
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    else // candidate serial not available. could be an error.
        //    {
        //        panel_Message1.Visible = true;
        //        lblMessageSsl.Text = "Candidate Form serial not found.";
        //        lblMessageSsl.ForeColor = Color.Crimson;
        //        return;
        //    }
        //}

        //private string MD5Hash(string input)
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}

        //protected void btnLoadPaymentBkash_Click(object sender, EventArgs e)
        //{

        //}

        ///// <summary>
        ///// Check payment details from bKash
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        ////protected void btnLoadTrxBkash_Click(object sender, EventArgs e)
        ////{
        ////    ClearSslPanel();

        ////    string trxId = null;
        ////    trxId = txtTrxIdBkash.Text.Trim();

        ////    string user = "BUPTWO";
        ////    string pass = "BDunipro02@2";
        ////    string msisdn = "01769028780";

        ////    if (!string.IsNullOrEmpty(trxId))
        ////    {
        ////        string url = null;

        ////        url = String.Format(@"https://www.bkashcluster.com:9081/dreamwave/merchant/trxcheck/sendmsg?user={0}&pass={1}&msisdn={2}&trxid={3}"
        ////                    , user, pass, msisdn, trxId);

        ////        if (!string.IsNullOrEmpty(url))
        ////        {
        ////            var xmlData = new WebClient().DownloadString(url);

        ////            //var xdoc = XDocument.Parse(xml);
        ////            //var items = xdoc.Descendants("Item")
        ////            //                .ToDictionary(i => (string)i.Attribute("Key"),
        ////            //                              i => (string)i.Attribute("Value"));


        ////            var xDoc = XDocument.Parse(xmlData);

        ////            string trxStatus = xDoc.Descendants().First(node => node.Name == "trxStatus").Value.ToString();

        ////            //string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
        ////            //string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
        ////            //string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
        ////            //string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
        ////            //string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
        ////            //string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
        ////            //string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
        ////            //string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
        ////            //string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();


        ////            #region trxStatus.Equals("0000") (SUCCESSFUL)
        ////            if (trxStatus.Equals("0000"))//successfull
        ////            {
        ////                string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
        ////                string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
        ////                string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
        ////                string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
        ////                string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
        ////                string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
        ////                string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
        ////                string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
        ////                string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();

        ////                lblTrxIdB.Text = trx_Id;
        ////                lblReferenceNoB.Text = reference;
        ////                lblCounterB.Text = counter;
        ////                lblPaidAmntB.Text = amount;
        ////                lblSenderB.Text = sender_1;
        ////                lblServiceB.Text = service;
        ////                lblCurrencyB.Text = currency;
        ////                lblReceiverB.Text = receiver;

        ////                #region GetBkashStatusMessageFor"0000"

        ////                DAL.BkashTrxStatusCode btsCodeSucces = null;
        ////                try
        ////                {
        ////                    using(var db = new OfficeDataManager())
        ////                    {
        ////                        btsCodeSucces = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
        ////                    }
        ////                }
        ////                catch (Exception)
        ////                {
        ////                    lblMessageBkash.Text = "Error getting bKash Status Message.";
        ////                    panel_Message2.CssClass = "alert alert-danger";
        ////                    panel_Message2.Visible = true;
        ////                    return;
        ////                }

        ////                if(btsCodeSucces != null)
        ////                {
        ////                    lblStatusCodeB.Text = trxStatus;
        ////                    lblStatusCodeB.ForeColor = Color.Green;
        ////                    lblStatusDetailsB.Text = btsCodeSucces.Message + "; " + btsCodeSucces.Interpretation;
        ////                    lblStatusDetailsB.ForeColor = Color.Green;
        ////                }
        ////                else
        ////                {
        ////                    lblStatusCodeB.Text = trxStatus;
        ////                    lblStatusDetailsB.Text = "Unknown";
        ////                    lblStatusDetailsB.ForeColor = Color.Crimson;
        ////                }

        ////                #endregion

        ////                DAL.CandidatePayment cPayment = null;
        ////                #region GetCandidatePayment
        ////                long paymentId = Convert.ToInt64(reference);
        ////                if (paymentId > 0)
        ////                {
        ////                    try
        ////                    {
        ////                        using (var db = new CandidateDataManager())
        ////                        {
        ////                            cPayment = db.AdmissionDB.CandidatePayments.Where(c => c.PaymentId == paymentId).FirstOrDefault();
        ////                        }
        ////                    }
        ////                    catch (Exception)
        ////                    {
        ////                        lblMessageBkash.Text = "Error getting candidate payment information.";
        ////                        panel_Message2.CssClass = "alert alert-danger";
        ////                        panel_Message2.Visible = true;
        ////                        return;
        ////                    }
        ////                }
        ////                //else
        ////                //{
        ////                //    lblMessageBkash.Text = "Error getting reference from bKash.";
        ////                //    panel_Message2.CssClass = "alert alert-danger";
        ////                //    panel_Message2.Visible = true;
        ////                //    return;
        ////                //}
        ////                #endregion

        ////                if (cPayment != null)
        ////                {
        ////                    #region GetBasicInfo
        ////                    DAL.BasicInfo candidate = null;
        ////                    try
        ////                    {
        ////                        using (var db = new CandidateDataManager())
        ////                        {
        ////                            candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
        ////                        }
        ////                    }
        ////                    catch (Exception)
        ////                    {
        ////                        lblMessageBkash.Text = "Error getting candidate information.";
        ////                        panel_Message2.CssClass = "alert alert-danger";
        ////                        panel_Message2.Visible = true;
        ////                        return;
        ////                    }

        ////                    if(candidate != null)
        ////                    {
        ////                        lblNameB.Text = candidate.FirstName;
        ////                    }
        ////                    else
        ////                    {
        ////                        lblNameB.Text = "Unknown";
        ////                        lblNameB.ForeColor = Color.Crimson;
        ////                    }

        ////                    #endregion

        ////                    int amountToBePaid = -1;
        ////                    double amount_1 = -0.0;
        ////                    //string paymentIdStr = null;

        ////                    amount_1 = Convert.ToDouble(cPayment.Amount) + (Convert.ToDouble(cPayment.Amount) * (1.5 / 100));
        ////                    amountToBePaid = Convert.ToInt32(Math.Ceiling(amount_1));

        ////                    int amountFromGateway = (int)Convert.ToDouble(amount);
        ////                    long referenceFromGateway = Int64.Parse(reference);

        ////                    lblSysAmntB.Text = cPayment.Amount.ToString();
        ////                    if(Convert.ToDouble(amount) <= Convert.ToDouble(cPayment.Amount))
        ////                    {
        ////                        lblPaidAmntB.ForeColor = Color.Crimson;
        ////                    }
        ////                }
        ////                else
        ////                {
        ////                    lblSysAmntB.Text = "Unknown";
        ////                    lblSysAmntB.ForeColor = Color.Crimson;
        ////                }

        ////            } //end if(trxStatus.Equals("0000"))
        ////            #endregion
        ////            //-----------------------------------
        ////            #region if(trxStatus != "0000")
        ////            else
        ////            {
        ////                string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();

        ////                DAL.BkashTrxStatusCode btsCode = null;
        ////                try
        ////                {
        ////                    if (!trxStatus.Equals("0000"))
        ////                    {
        ////                        using (var db = new OfficeDataManager())
        ////                        {
        ////                            btsCode = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
        ////                        }
        ////                    }
        ////                }
        ////                catch (Exception)
        ////                {
        ////                    lblMessageBkash.Text = "Error getting bKash Transaction Status Codes.";
        ////                    panel_Message2.CssClass = "alert alert-danger";
        ////                    panel_Message2.Visible = true;
        ////                    return;
        ////                }

        ////                if (btsCode != null)
        ////                {
        ////                    lblStatusCodeB.Text = trxStatus;
        ////                    lblStatusDetailsB.Text = btsCode.Message + "; " + btsCode.Interpretation;
        ////                    lblStatusDetailsB.ForeColor = Color.Crimson;
        ////                }
        ////                else
        ////                {
        ////                    lblStatusCodeB.Text = trxStatus;
        ////                    lblStatusDetailsB.Text = "Unknown";
        ////                    lblStatusDetailsB.ForeColor = Color.Crimson;
        ////                }

        ////                lblTrxIdB.Text = trx_Id;
        ////                lblReferenceNoB.Text = string.Empty;
        ////                lblCounterB.Text = string.Empty;
        ////                lblPaidAmntB.Text = string.Empty;
        ////                lblSenderB.Text = string.Empty;
        ////                lblServiceB.Text = string.Empty;
        ////                lblCurrencyB.Text = string.Empty;
        ////                lblReceiverB.Text = string.Empty;
        ////                lblNameB.Text = string.Empty;
        ////                lblSysAmntB.Text = string.Empty;
        ////            }
        ////            #endregion

        ////        }
        ////    }
        ////    else
        ////    {
        ////        lblMessageBkash.Text = "Please provide your Trx ID.";
        ////        panel_Message2.CssClass = "alert alert-danger";
        ////        panel_Message2.Visible = true;
        ////    }
        ////}











        ////private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        ////long uId = 0;
        ////string uRole = string.Empty;

        ////protected override void OnLoad(EventArgs e)
        ////{
        ////    base.OnLoad(e);
        ////    uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
        ////    uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

        ////    if (!IsPostBack)
        ////    {

        ////    }
        ////}

        ////private void ClearMessage()
        ////{
        ////    lblMessageSsl.Text = "";
        ////}

        ////private void ClearSslPanel()
        ////{
        ////    txtPaymentIdSsl.Text = string.Empty;

        ////    panel_Message1.Visible = false;

        ////    lblTranId.Text = string.Empty;
        ////    lblCandidateName.Text = string.Empty;
        ////    lblStatus.Text = string.Empty;
        ////    lblTransDate.Text = string.Empty;
        ////    lblBankTranId.Text = string.Empty;
        ////    lblValId.Text = string.Empty;
        ////    lblAmount.Text = string.Empty;
        ////    lblReceivable.Text = string.Empty;
        ////    lblCardType.Text = string.Empty;
        ////    lblCardNumber.Text = string.Empty;
        ////    lblIssuerBankCountry.Text = string.Empty;
        ////    lblCurrency.Text = string.Empty;
        ////}

        ////private void ClearBkashPanel()
        ////{
        ////    //txtTrxIdBkash.Text = null;

        ////    //panel_Message2.Visible = false;

        ////    //lblNameB.Text = string.Empty;
        ////    //lblSysAmntB.Text = string.Empty;
        ////    //lblStatusCodeB.Text = string.Empty;
        ////    //lblStatusDetailsB.Text = string.Empty;
        ////    //lblTrxIdB.Text = string.Empty;
        ////    //lblReferenceNoB.Text = string.Empty;
        ////    //lblCounterB.Text = string.Empty;
        ////    //lblPaidAmntB.Text = string.Empty;
        ////    //lblSenderB.Text = string.Empty;
        ////    //lblServiceB.Text = string.Empty;
        ////    //lblCurrencyB.Text = string.Empty;
        ////    //lblReceiverB.Text = string.Empty;
        ////}

        ////protected void btnLoadSsl_Click(object sender, EventArgs e)
        ////{
        ////    ClearMessage();

        ////    long paymentId = -1;
        ////    if (!string.IsNullOrEmpty(txtPaymentIdSsl.Text.Trim()))
        ////    {
        ////        paymentId = Int64.Parse(txtPaymentIdSsl.Text);
        ////    }

        ////    DAL.CandidatePayment cPayment = null;
        ////    try
        ////    {
        ////        using (var db = new OfficeDataManager())
        ////        {
        ////            cPayment = db.AdmissionDB.CandidatePayments
        ////                .Where(c => c.PaymentId == paymentId)
        ////                .FirstOrDefault();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        lblMessageSsl.Text = "Error getting candidate payment " + ex.Message;
        ////        lblMessageSsl.ForeColor = Color.Crimson;
        ////        lblMessageSsl.Font.Bold = true;
        ////        return;
        ////    }

        ////    if (cPayment != null)
        ////    {
        ////        DAL.BasicInfo candidate = null;
        ////        try
        ////        {
        ////            using (var db = new CandidateDataManager())
        ////            {
        ////                candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
        ////            }
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            lblMessageSsl.Text = "Error getting candidate " + ex.Message;
        ////            lblMessageSsl.ForeColor = Color.Crimson;
        ////            lblMessageSsl.Font.Bold = true;
        ////            return;
        ////        }

        ////        if (cPayment.IsPaid == true)
        ////        {
        ////            lblMessageSsl.Text = "Candidate found. Paid : Yes";
        ////            lblMessageSsl.ForeColor = Color.Green;
        ////            if (candidate != null)
        ////            {
        ////                lblCandidateName.Text = candidate.FirstName;
        ////            }
        ////            else
        ////            {
        ////                lblCandidateName.Text = "N/A";
        ////                lblCandidateName.ForeColor = Color.Crimson;
        ////            }
        ////            GetDataFromSSL(cPayment);
        ////        }
        ////        else if (cPayment.IsPaid == false)
        ////        {
        ////            lblMessageSsl.Text = "Candidate found. Paid : NO";
        ////            lblMessageSsl.ForeColor = Color.Crimson;
        ////            if (candidate != null)
        ////            {
        ////                lblCandidateName.Text = candidate.FirstName;
        ////            }
        ////            else
        ////            {
        ////                lblCandidateName.Text = "N/A";
        ////                lblCandidateName.ForeColor = Color.Crimson;
        ////            }
        ////            GetDataFromSSL(cPayment);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        lblMessageSsl.Text = "Candidate with PaymentID " + paymentId + " not found";
        ////        lblMessageSsl.ForeColor = Color.Crimson;
        ////        return;
        ////    }

        ////}

        /////// <summary>
        /////// Get payment details from Payment Gateway provider.
        /////// </summary>
        /////// <param name="cPayment"></param>
        ////private void GetDataFromSSL(DAL.CandidatePayment cPayment)
        ////{
        ////    ClearBkashPanel();

        ////    List<DAL.CandidateFormSl> cFormSlList = null;
        ////    List<DAL.AdmissionSetup> admSetupList = null;
        ////    DAL.AdmissionSetup admSetup = null;
        ////    DAL.Store store = null;
        ////    string url = null;

        ////    //1. Get form serial objects.
        ////    try
        ////    {
        ////        using (var db = new CandidateDataManager())
        ////        {
        ////            cFormSlList = db.AdmissionDB.CandidateFormSls
        ////                .Where(c => c.CandidatePaymentID == cPayment.ID).ToList();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        lblMessageSsl.Text = "Error getting candidate form serial information. " + ex.Message;
        ////        lblMessageSsl.ForeColor = Color.Crimson;
        ////        return;
        ////    }

        ////    //2. Figure out what the number of form serials indicates.
        ////    if (cFormSlList.Count() > 1) // more than one candidate form serial means multiple form purchase. hence this is undergraduate candidate.
        ////    {
        ////        bool isMultiple = true;

        ////        // get admission setup for each form serial object.
        ////        foreach (var item in cFormSlList)
        ////        {
        ////            DAL.AdmissionSetup _temp = null;
        ////            try
        ////            {
        ////                using (var db = new CandidateDataManager())
        ////                {
        ////                    _temp = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
        ////                }
        ////                if (_temp.EducationCategoryID == 4) // only get the undergraduate admission setup.
        ////                {
        ////                    admSetupList.Add(_temp);
        ////                }
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                lblMessageSsl.Text = "Error getting admission setup. " + ex.Message;
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }
        ////        }

        ////        if (admSetupList.Count() > 0)
        ////        {
        ////            foreach (var item in admSetupList)
        ////            {
        ////                if (item.EducationCategoryID == 6) // if graduate detected.
        ////                {
        ////                    isMultiple = false;
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            lblMessageSsl.Text = "Error: admission setup not found.";
        ////            lblMessageSsl.ForeColor = Color.Crimson;
        ////            return;
        ////        }

        ////        if (isMultiple == true)
        ////        {
        ////            try
        ////            {
        ////                using (var db = new OfficeDataManager())
        ////                {
        ////                    store = db.AdmissionDB.Stores.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
        ////                }
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                lblMessageSsl.Text = "Error getting store." + ex.Message;
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }
        ////        }

        ////        if (store != null)
        ////        {
        ////            string storeId = null;
        ////            string password = null;

        ////            try
        ////            {
        ////                storeId = Decrypt.DecryptString(store.StoreId);
        ////                password = Decrypt.DecryptString(store.StorePass);
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                lblMessageSsl.Text = "Error getting store information. " + ex.Message;
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }

        ////            if (storeId != null && password != null)
        ////            {
        ////                //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
        ////                //    , val_id, plainText_storeId, plainText_password);

        ////                url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?tran_id={0}&store_id={1}&store_passwd={2}&v=3&format=json"
        ////                    , cPayment.PaymentId.ToString(), storeId, password);
        ////            }
        ////            else
        ////            {
        ////                lblMessageSsl.Text = "Error getting Store.";
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            lblMessageSsl.Text = "Error: store not found.";
        ////            lblMessageSsl.ForeColor = Color.Crimson;
        ////            return;
        ////        }

        ////        if (url != null)
        ////        {
        ////            var jsonData = new WebClient().DownloadString(url);

        ////            JObject rss = JObject.Parse(jsonData);

        ////            string paymentIDFromGateway = (string)rss["element"][0]["tran_id"];

        ////            if (paymentIDFromGateway.Equals(cPayment.PaymentId.ToString())) //if payment id matches
        ////            {

        ////                #region
        ////                string paymentStatus = rss["element"][0]["status"].ToString();
        ////                string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
        ////                string paymentAmount = rss["element"][0]["amount"].ToString();
        ////                string storeAmount = rss["element"][0]["store_amount"].ToString();
        ////                //string paymentVal_id = outputArray["val_id"].ToString();
        ////                string paymentTran_id = rss["element"][0]["tran_id"].ToString();
        ////                string paymentTran_date = rss["element"][0]["tran_date"].ToString();
        ////                string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
        ////                string paymentCard_type = rss["element"][0]["card_type"].ToString();
        ////                string cardNumber = rss["element"][0]["card_no"].ToString();
        ////                string currencyType = rss["element"][0]["currency_type"].ToString();
        ////                string currencyAmount = rss["element"][0]["currency_amount"].ToString();
        ////                string cardBrand = rss["element"][0]["card_brand"].ToString();
        ////                string cardIssuer = rss["element"][0]["card_issuer"].ToString();
        ////                string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
        ////                string value_b = rss["element"][0]["value_b"].ToString();
        ////                string value_c = rss["element"][0]["value_c"].ToString();
        ////                string value_d = rss["element"][0]["value_d"].ToString();
        ////                //string paymentValidated_on = outputArray["validated_on"].ToString();
        ////                //string paymentRisk_title = outputArray["risk_title"].ToString();

        ////                decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
        ////                string paymentStatusUpper = paymentStatus.ToUpper();

        ////                lblAmount.Text = validateResultAmount.ToString();
        ////                lblBankTranId.Text = paymentBank_tran_id;
        ////                lblCardNumber.Text = cardNumber;
        ////                lblCardType.Text = paymentCard_type;
        ////                lblCurrency.Text = currencyType;
        ////                lblIssuerBankCountry.Text = cardIssuer + " " + cardIssuerCountry;
        ////                lblReceivable.Text = storeAmount;
        ////                if (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED"))
        ////                {
        ////                    lblStatus.Text = paymentStatusUpper;
        ////                    lblStatus.ForeColor = Color.Green;
        ////                }
        ////                else
        ////                {
        ////                    lblStatus.Text = paymentStatusUpper;
        ////                    lblStatus.ForeColor = Color.Crimson;
        ////                }
        ////                lblTranId.Text = paymentTran_id;
        ////                lblTransDate.Text = paymentTran_date;
        ////                #endregion

        ////            }
        ////            else
        ////            {
        ////                lblMessageSsl.Text = "Candidate PaymentID from system does not match with Gateway PaymentID(1).";
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }

        ////        }//end if (url != null)


        ////    }
        ////    else if (cFormSlList.Count() == 1) // only one candidate form serial means either multiple or single form purchase. This could be either graduate or undergraduate.
        ////    {

        ////        // get admission setup for each form serial object.
        ////        foreach (var item in cFormSlList)
        ////        {
        ////            try
        ////            {
        ////                using (var db = new CandidateDataManager())
        ////                {
        ////                    admSetup = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
        ////                }
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                lblMessageSsl.Text = "Error getting admission setup(1). " + ex.Message;
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }
        ////        }

        ////        if (admSetup != null)
        ////        {
        ////            try
        ////            {
        ////                using (var db = new OfficeDataManager())
        ////                {
        ////                    store = db.AdmissionDB.Stores.Find(admSetup.StoreID);
        ////                }
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                lblMessageSsl.Text = "Error getting store(1). " + ex.Message;
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            lblMessageSsl.Text = "Error: admission setup not found(1).";
        ////            lblMessageSsl.ForeColor = Color.Crimson;
        ////            return;
        ////        }


        ////        if (store != null)
        ////        {
        ////            string storeId = null;
        ////            string password = null;

        ////            try
        ////            {
        ////                storeId = Decrypt.DecryptString(store.StoreId);
        ////                password = Decrypt.DecryptString(store.StorePass);
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                lblMessageSsl.Text = "Error getting store information(1). " + ex.Message;
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }

        ////            if (storeId != null && password != null)
        ////            {
        ////                //url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
        ////                //    , val_id, plainText_storeId, plainText_password);

        ////                url = String.Format(@"https://securepay.sslcommerz.com/validator/api/merchantTransIDvalidationAPI.php?tran_id={0}&store_id={1}&store_passwd={2}&v=3&format=json"
        ////                    , cPayment.PaymentId.ToString(), storeId, password);
        ////            }
        ////            else
        ////            {
        ////                lblMessageSsl.Text = "Error getting StoreId and Password(1).";
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            lblMessageSsl.Text = "Error: store not found(1).";
        ////            lblMessageSsl.ForeColor = Color.Crimson;
        ////            return;
        ////        }

        ////        if (url != null)
        ////        {
        ////            var jsonData = new WebClient().DownloadString(url);

        ////            JObject rss = JObject.Parse(jsonData);

        ////            string paymentIDFromGateway = (string)rss["element"][0]["tran_id"];

        ////            if (paymentIDFromGateway.Equals(cPayment.PaymentId.ToString())) //if payment id matches
        ////            {

        ////                #region
        ////                string paymentStatus = rss["element"][0]["status"].ToString();
        ////                string paymentValue_a = rss["element"][0]["value_a"].ToString();  //get candidate id for validation
        ////                string paymentAmount = rss["element"][0]["amount"].ToString();
        ////                string storeAmount = rss["element"][0]["store_amount"].ToString();
        ////                //string paymentVal_id = outputArray["val_id"].ToString();
        ////                string paymentTran_id = rss["element"][0]["tran_id"].ToString();
        ////                string paymentTran_date = rss["element"][0]["tran_date"].ToString();
        ////                string paymentBank_tran_id = rss["element"][0]["bank_tran_id"].ToString();
        ////                string paymentCard_type = rss["element"][0]["card_type"].ToString();
        ////                string cardNumber = rss["element"][0]["card_no"].ToString();
        ////                string currencyType = rss["element"][0]["currency_type"].ToString();
        ////                string currencyAmount = rss["element"][0]["currency_amount"].ToString();
        ////                string cardBrand = rss["element"][0]["card_brand"].ToString();
        ////                string cardIssuer = rss["element"][0]["card_issuer"].ToString();
        ////                string cardIssuerCountry = rss["element"][0]["card_issuer_country"].ToString();
        ////                string value_b = rss["element"][0]["value_b"].ToString();
        ////                string value_c = rss["element"][0]["value_c"].ToString();
        ////                string value_d = rss["element"][0]["value_d"].ToString();
        ////                //string paymentValidated_on = outputArray["validated_on"].ToString();
        ////                //string paymentRisk_title = outputArray["risk_title"].ToString();

        ////                decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
        ////                string paymentStatusUpper = paymentStatus.ToUpper();

        ////                lblAmount.Text = validateResultAmount.ToString();
        ////                lblBankTranId.Text = paymentBank_tran_id;
        ////                lblCardNumber.Text = cardNumber;
        ////                lblCardType.Text = paymentCard_type;
        ////                lblCurrency.Text = currencyType;
        ////                lblIssuerBankCountry.Text = cardIssuer + " " + cardIssuerCountry;
        ////                lblReceivable.Text = storeAmount;
        ////                if (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED"))
        ////                {
        ////                    lblStatus.Text = paymentStatusUpper;
        ////                    lblStatus.ForeColor = Color.Green;
        ////                }
        ////                else
        ////                {
        ////                    lblStatus.Text = paymentStatusUpper;
        ////                    lblStatus.ForeColor = Color.Crimson;
        ////                }
        ////                lblTranId.Text = paymentTran_id;
        ////                lblTransDate.Text = paymentTran_date;
        ////                #endregion

        ////            }
        ////            else
        ////            {
        ////                lblMessageSsl.Text = "Candidate PaymentID from system does not match with Gateway PaymentID(1).";
        ////                lblMessageSsl.ForeColor = Color.Crimson;
        ////                return;
        ////            }

        ////        }//end if (url != null)


        ////    }
        ////    else // candidate serial not available. could be an error.
        ////    {
        ////        lblMessageSsl.Text = "Candidate Form serial not found.";
        ////        lblMessageSsl.ForeColor = Color.Crimson;
        ////        return;
        ////    }
        ////}

        ////protected void btnLoadPaymentBkash_Click(object sender, EventArgs e)
        ////{

        ////}

        /////// <summary>
        /////// Check payment details from bKash
        /////// </summary>
        /////// <param name="sender"></param>
        /////// <param name="e"></param>
        /////// 

        //////protected void btnLoadTrxBkash_Click(object sender, EventArgs e)
        //////{
        //////    ClearSslPanel();

        //////    string trxId = null;
        //////    trxId = txtTrxIdBkash.Text.Trim();

        //////    string user = "BUPTWO";
        //////    string pass = "BDunipro02@2";
        //////    string msisdn = "01769028780";

        //////    if (!string.IsNullOrEmpty(trxId))
        //////    {
        //////        string url = null;

        //////        url = String.Format(@"https://www.bkashcluster.com:9081/dreamwave/merchant/trxcheck/sendmsg?user={0}&pass={1}&msisdn={2}&trxid={3}"
        //////                    , user, pass, msisdn, trxId);

        //////        if (!string.IsNullOrEmpty(url))
        //////        {
        //////            var xmlData = new WebClient().DownloadString(url);

        //////            //var xdoc = XDocument.Parse(xml);
        //////            //var items = xdoc.Descendants("Item")
        //////            //                .ToDictionary(i => (string)i.Attribute("Key"),
        //////            //                              i => (string)i.Attribute("Value"));


        //////            var xDoc = XDocument.Parse(xmlData);

        //////            string trxStatus = xDoc.Descendants().First(node => node.Name == "trxStatus").Value.ToString();

        //////            //string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
        //////            //string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
        //////            //string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
        //////            //string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
        //////            //string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
        //////            //string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
        //////            //string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
        //////            //string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
        //////            //string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();


        //////            #region trxStatus.Equals("0000") (SUCCESSFUL)
        //////            if (trxStatus.Equals("0000"))//successfull
        //////            {
        //////                string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
        //////                string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
        //////                string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
        //////                string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
        //////                string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
        //////                string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
        //////                string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
        //////                string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
        //////                string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();

        //////                lblTrxIdB.Text = trx_Id;
        //////                lblReferenceNoB.Text = reference;
        //////                lblCounterB.Text = counter;
        //////                lblPaidAmntB.Text = amount;
        //////                lblSenderB.Text = sender_1;
        //////                lblServiceB.Text = service;
        //////                lblCurrencyB.Text = currency;
        //////                lblReceiverB.Text = receiver;

        //////                #region GetBkashStatusMessageFor"0000"

        //////                DAL.BkashTrxStatusCode btsCodeSucces = null;
        //////                try
        //////                {
        //////                    using(var db = new OfficeDataManager())
        //////                    {
        //////                        btsCodeSucces = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
        //////                    }
        //////                }
        //////                catch (Exception)
        //////                {
        //////                    lblMessageBkash.Text = "Error getting bKash Status Message.";
        //////                    panel_Message2.CssClass = "alert alert-danger";
        //////                    panel_Message2.Visible = true;
        //////                    return;
        //////                }

        //////                if(btsCodeSucces != null)
        //////                {
        //////                    lblStatusCodeB.Text = trxStatus;
        //////                    lblStatusCodeB.ForeColor = Color.Green;
        //////                    lblStatusDetailsB.Text = btsCodeSucces.Message + "; " + btsCodeSucces.Interpretation;
        //////                    lblStatusDetailsB.ForeColor = Color.Green;
        //////                }
        //////                else
        //////                {
        //////                    lblStatusCodeB.Text = trxStatus;
        //////                    lblStatusDetailsB.Text = "Unknown";
        //////                    lblStatusDetailsB.ForeColor = Color.Crimson;
        //////                }

        //////                #endregion

        //////                DAL.CandidatePayment cPayment = null;
        //////                #region GetCandidatePayment
        //////                long paymentId = Convert.ToInt64(reference);
        //////                if (paymentId > 0)
        //////                {
        //////                    try
        //////                    {
        //////                        using (var db = new CandidateDataManager())
        //////                        {
        //////                            cPayment = db.AdmissionDB.CandidatePayments.Where(c => c.PaymentId == paymentId).FirstOrDefault();
        //////                        }
        //////                    }
        //////                    catch (Exception)
        //////                    {
        //////                        lblMessageBkash.Text = "Error getting candidate payment information.";
        //////                        panel_Message2.CssClass = "alert alert-danger";
        //////                        panel_Message2.Visible = true;
        //////                        return;
        //////                    }
        //////                }
        //////                //else
        //////                //{
        //////                //    lblMessageBkash.Text = "Error getting reference from bKash.";
        //////                //    panel_Message2.CssClass = "alert alert-danger";
        //////                //    panel_Message2.Visible = true;
        //////                //    return;
        //////                //}
        //////                #endregion

        //////                if (cPayment != null)
        //////                {
        //////                    #region GetBasicInfo
        //////                    DAL.BasicInfo candidate = null;
        //////                    try
        //////                    {
        //////                        using (var db = new CandidateDataManager())
        //////                        {
        //////                            candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
        //////                        }
        //////                    }
        //////                    catch (Exception)
        //////                    {
        //////                        lblMessageBkash.Text = "Error getting candidate information.";
        //////                        panel_Message2.CssClass = "alert alert-danger";
        //////                        panel_Message2.Visible = true;
        //////                        return;
        //////                    }

        //////                    if(candidate != null)
        //////                    {
        //////                        lblNameB.Text = candidate.FirstName;
        //////                    }
        //////                    else
        //////                    {
        //////                        lblNameB.Text = "Unknown";
        //////                        lblNameB.ForeColor = Color.Crimson;
        //////                    }

        //////                    #endregion

        //////                    int amountToBePaid = -1;
        //////                    double amount_1 = -0.0;
        //////                    //string paymentIdStr = null;

        //////                    amount_1 = Convert.ToDouble(cPayment.Amount) + (Convert.ToDouble(cPayment.Amount) * (1.5 / 100));
        //////                    amountToBePaid = Convert.ToInt32(Math.Ceiling(amount_1));

        //////                    int amountFromGateway = (int)Convert.ToDouble(amount);
        //////                    long referenceFromGateway = Int64.Parse(reference);

        //////                    lblSysAmntB.Text = cPayment.Amount.ToString();
        //////                    if(Convert.ToDouble(amount) <= Convert.ToDouble(cPayment.Amount))
        //////                    {
        //////                        lblPaidAmntB.ForeColor = Color.Crimson;
        //////                    }
        //////                }
        //////                else
        //////                {
        //////                    lblSysAmntB.Text = "Unknown";
        //////                    lblSysAmntB.ForeColor = Color.Crimson;
        //////                }

        //////            } //end if(trxStatus.Equals("0000"))
        //////            #endregion
        //////            //-----------------------------------
        //////            #region if(trxStatus != "0000")
        //////            else
        //////            {
        //////                string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();

        //////                DAL.BkashTrxStatusCode btsCode = null;
        //////                try
        //////                {
        //////                    if (!trxStatus.Equals("0000"))
        //////                    {
        //////                        using (var db = new OfficeDataManager())
        //////                        {
        //////                            btsCode = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
        //////                        }
        //////                    }
        //////                }
        //////                catch (Exception)
        //////                {
        //////                    lblMessageBkash.Text = "Error getting bKash Transaction Status Codes.";
        //////                    panel_Message2.CssClass = "alert alert-danger";
        //////                    panel_Message2.Visible = true;
        //////                    return;
        //////                }

        //////                if (btsCode != null)
        //////                {
        //////                    lblStatusCodeB.Text = trxStatus;
        //////                    lblStatusDetailsB.Text = btsCode.Message + "; " + btsCode.Interpretation;
        //////                    lblStatusDetailsB.ForeColor = Color.Crimson;
        //////                }
        //////                else
        //////                {
        //////                    lblStatusCodeB.Text = trxStatus;
        //////                    lblStatusDetailsB.Text = "Unknown";
        //////                    lblStatusDetailsB.ForeColor = Color.Crimson;
        //////                }

        //////                lblTrxIdB.Text = trx_Id;
        //////                lblReferenceNoB.Text = string.Empty;
        //////                lblCounterB.Text = string.Empty;
        //////                lblPaidAmntB.Text = string.Empty;
        //////                lblSenderB.Text = string.Empty;
        //////                lblServiceB.Text = string.Empty;
        //////                lblCurrencyB.Text = string.Empty;
        //////                lblReceiverB.Text = string.Empty;
        //////                lblNameB.Text = string.Empty;
        //////                lblSysAmntB.Text = string.Empty;
        //////            }
        //////            #endregion

        //////        }
        //////    }
        //////    else
        //////    {
        //////        lblMessageBkash.Text = "Please provide your Trx ID.";
        //////        panel_Message2.CssClass = "alert alert-danger";
        //////        panel_Message2.Visible = true;
        //////    }
        //////} 
        #endregion
    }
}