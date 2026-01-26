using CommonUtility;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class ipnsuccessurl : System.Web.UI.Page
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = "";
            //decimal number;
            decimal? systemAmount;
            string storeIdStr = null;

            try
            {
                if (
                (!String.IsNullOrEmpty(Request.Form["amount"])) &&
                (!String.IsNullOrEmpty(Request.Form["val_id"])) &&
                (!String.IsNullOrEmpty(Request.Form["status"])))
                {
                    StringBuilder builder = new StringBuilder();

                    string[] keys = Request.Form.AllKeys;

                    for (int i = 0; i < keys.Length; i++)
                    {
                        builder.Append(keys[i] + ": " + Request.Form[keys[i]] + ";");
                    }

                    /////////////////////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_2 tmp1 = new DAL.TemporaryTable_2();                        //
                    tmp1.Note = "tmp1 kyes[]:___" + builder.ToString();                            //
                    tmp1.DateTimeNow = DateTime.Now;                                               //
                    using (var db = new GeneralDataManager())                                      //
                    {                                                                              //
                        db.Insert<DAL.TemporaryTable_2>(tmp1);                                   //
                    }                                                                              //
                    /////////////////////////////////////////////////////////////////////////////////

                    #region Post Request Data
                    string amount = Request.Form["amount"].ToString(); // Amount with service charge
                    string status = Request.Form["status"].ToString(); //payment status
                    string trans_id = Request.Form["tran_id"].ToString(); // payment id system is sending
                    string val_id = Request.Form["val_id"].ToString(); // SSL ID
                    string value_a = Request.Form["value_a"].ToString(); // candidate ID
                    string value_b = Request.Form["value_b"].ToString(); //candidate first name
                    string value_c = Request.Form["value_c"].ToString(); // Amount from system.
                    string value_d = Request.Form["value_d"].ToString(); // store id

                    /////////////////////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_2 tmp2 = new DAL.TemporaryTable_2();                        //
                    tmp2.Note = "tmp2 post request data:___" + amount + "; " + status + "; " + trans_id + "; " + val_id + "; " + value_a + "; "
                         + value_b + "; " + value_c + "; " + value_d + "; ";                       //
                    tmp2.DateTimeNow = DateTime.Now;                                               //
                    using (var db = new GeneralDataManager())                                      //
                    {                                                                              //
                        db.Insert<DAL.TemporaryTable_2>(tmp2);                                   //
                    }                                                                              //
                    /////////////////////////////////////////////////////////////////////////////////

                    str = amount;
                    #endregion

                    #region GET CANDIDATE PAYMENT

                    DAL.CandidatePayment cPaymentObj = null;
                    if (string.IsNullOrEmpty(trans_id))
                    {

                        /////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp3 = new DAL.TemporaryTable_2();                        //
                        tmp3.Note = "tmp3 string.IsNullOrEmpty(trans_id)___" + trans_id;               //
                        tmp3.DateTimeNow = DateTime.Now;                                               //
                        using (var db = new GeneralDataManager())                                      //
                        {                                                                              //
                            db.Insert<DAL.TemporaryTable_2>(tmp3);                                   //
                        }                                                                              //
                        /////////////////////////////////////////////////////////////////////////////////

                    }
                    else
                    {
                        long candidatePaymentId = -1;
                        candidatePaymentId = Int64.Parse(trans_id);

                        /////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp4 = new DAL.TemporaryTable_2();                        //
                        tmp4.Note = "tmp4 candidatePaymentId___" + candidatePaymentId;                 //
                        tmp4.DateTimeNow = DateTime.Now;                                               //
                        using (var db = new GeneralDataManager())                                      //
                        {                                                                              //
                            db.Insert<DAL.TemporaryTable_2>(tmp4);                                   //
                        }                                                                              //
                        /////////////////////////////////////////////////////////////////////////////////

                        if (candidatePaymentId > 0)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                cPaymentObj = db.AdmissionDB.CandidatePayments
                                    .Where(c => c.PaymentId == candidatePaymentId)
                                    .FirstOrDefault();
                            }
                        }

                        if (cPaymentObj != null)
                        {
                            systemAmount = cPaymentObj.Amount;

                            /////////////////////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_2 tmp5 = new DAL.TemporaryTable_2();                        //
                            tmp5.Note = "tmp5 cPaymentObj != null____" + systemAmount;                     //
                            tmp5.DateTimeNow = DateTime.Now;                                               //
                            using (var db = new GeneralDataManager())                                      //
                            {                                                                              //
                                db.Insert<DAL.TemporaryTable_2>(tmp5);                                   //
                            }                                                                              //
                            /////////////////////////////////////////////////////////////////////////////////

                        }
                        else
                        {

                            /////////////////////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_2 tmp6 = new DAL.TemporaryTable_2();                        //
                            tmp6.Note = "tmp6 cPaymentObj else____ cPaymentObj is null";                   //
                            tmp6.DateTimeNow = DateTime.Now;                                               //
                            using (var db = new GeneralDataManager())                                      //
                            {                                                                              //
                                db.Insert<DAL.TemporaryTable_2>(tmp6);                                   //
                            }                                                                              //
                            /////////////////////////////////////////////////////////////////////////////////

                        }
                    }

                    #endregion

                    #region GET STORE INFO

                    DAL.Store storeObj = null;

                    if (string.IsNullOrEmpty(value_d))
                    {

                        /////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp7 = new DAL.TemporaryTable_2();                        //
                        tmp7.Note = "tmp7 string.IsNullOrEmpty(value_d)____" + value_d;                //
                        tmp7.DateTimeNow = DateTime.Now;                                               //
                        using (var db = new GeneralDataManager())                                      //
                        {                                                                              //
                            db.Insert<DAL.TemporaryTable_2>(tmp7);                                     //
                        }                                                                              //
                        /////////////////////////////////////////////////////////////////////////////////

                    }
                    else
                    {
                        int storeId = 0;
                        storeIdStr = value_d;
                        storeId = Int32.Parse(storeIdStr);

                        /////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp8 = new DAL.TemporaryTable_2();                        //
                        tmp8.Note = "tmp8 storeIdStr___" + storeId + ";" + storeIdStr + "; " + value_d.ToString() + "; " + Int32.Parse(storeIdStr) + "; " + Int32.Parse(value_d);
                        tmp8.DateTimeNow = DateTime.Now;                                               //
                        using (var db = new GeneralDataManager())                                      //
                        {                                                                              //
                            db.Insert<DAL.TemporaryTable_2>(tmp8);                                   //
                        }                                                                              //
                        /////////////////////////////////////////////////////////////////////////////////

                        if (storeId > 0)
                        {

                            /////////////////////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_2 tmp9 = new DAL.TemporaryTable_2();                        //
                            tmp9.Note = "tmp9 storeId > 0___" + storeId;                                   //
                            tmp9.DateTimeNow = DateTime.Now;                                               //
                            using (var db = new GeneralDataManager())                                      //
                            {                                                                              //
                                db.Insert<DAL.TemporaryTable_2>(tmp9);                                   //
                            }                                                                              //
                            /////////////////////////////////////////////////////////////////////////////////

                            using (var db = new OfficeDataManager())
                            {
                                storeObj = db.AdmissionDB.Stores.Find(storeId);
                            }

                            /////////////////////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_2 tmp10 = new DAL.TemporaryTable_2();                       //
                            tmp10.Note = "tmp10 storeObj____" + storeObj.StoreName + "; " + storeObj.StoreId + "; " + storeObj.StorePass + "; " + Decrypt.DecryptString(storeObj.StoreId) + Decrypt.DecryptString(storeObj.StorePass);
                            tmp10.DateTimeNow = DateTime.Now;                                              //
                            using (var db = new GeneralDataManager())                                      //
                            {                                                                              //
                                db.Insert<DAL.TemporaryTable_2>(tmp10);                                    //
                            }                                                                              //
                            /////////////////////////////////////////////////////////////////////////////////

                        }
                    }

                    #endregion

                    #region VALIDATE

                    string plainText_storeId = null;
                    string plainText_password = null;

                    if (storeObj != null)
                    {
                        plainText_storeId = Decrypt.DecryptString(storeObj.StoreId);
                        plainText_password = Decrypt.DecryptString(storeObj.StorePass);

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp11 = new DAL.TemporaryTable_2();                                    //
                        tmp11.Note = "tmp11 storeObj != null____" + plainText_storeId + "  " + plainText_password;  //
                        tmp11.DateTimeNow = DateTime.Now;                                                           //
                        using (var db = new GeneralDataManager())                                                   //
                        {                                                                                           //
                            db.Insert<DAL.TemporaryTable_2>(tmp11);                                                 //
                        }                                                                                           //
                        //////////////////////////////////////////////////////////////////////////////////////////////

                    }

                    string url = null;

                    if (string.IsNullOrEmpty(plainText_storeId) && string.IsNullOrEmpty(plainText_password))
                    {
                        url = null;

                        ////////////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp12 = new DAL.TemporaryTable_2();                              //
                        tmp12.Note = "tmp12 string.IsNullOrEmpty(plainText_storeId) && string.IsNullOrEmpty(plainText_password)____ url = null";
                        tmp12.DateTimeNow = DateTime.Now;                                                     //
                        using (var db = new GeneralDataManager())                                             //
                        {                                                                                     //
                            db.Insert<DAL.TemporaryTable_2>(tmp12);                                           //
                        }                                                                                     //
                        ////////////////////////////////////////////////////////////////////////////////////////

                        Response.Redirect("~/Admission/Message.aspx?message=Error occured while getting validation information. Please contact administrator.&type=danger", false);
                    }
                    else
                    {
                        url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
                            , val_id, plainText_storeId, plainText_password);

                        ///////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp13 = new DAL.TemporaryTable_2();                     //
                        tmp13.Note = "tmp13 url____" + url;                                          //
                        tmp13.DateTimeNow = DateTime.Now;                                            //
                        using (var db = new GeneralDataManager())                                    //
                        {                                                                            //
                            db.Insert<DAL.TemporaryTable_2>(tmp13);                                  //
                        }                                                                            //
                        ///////////////////////////////////////////////////////////////////////////////

                    }

                    if (!string.IsNullOrEmpty(url))
                    {
                        var jsonData = new WebClient().DownloadString(url);


                        ///////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp14 = new DAL.TemporaryTable_2();                     //
                        tmp14.Note = "tmp14 jsonData____" + jsonData;                                //
                        tmp14.DateTimeNow = DateTime.Now;                                            //
                        using (var db = new GeneralDataManager())                                    //
                        {                                                                            //
                            db.Insert<DAL.TemporaryTable_2>(tmp14);                                  //
                        }                                                                            //
                        ///////////////////////////////////////////////////////////////////////////////

                        dynamic outputArray = JsonConvert.DeserializeObject<dynamic>(jsonData);

                        ///////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp15 = new DAL.TemporaryTable_2();                     //
                        tmp15.Note = "tmp15 outputArray____" + outputArray;                          //
                        tmp15.DateTimeNow = DateTime.Now;                                            //
                        using (var db = new GeneralDataManager())                                    //
                        {                                                                            //
                            db.Insert<DAL.TemporaryTable_2>(tmp15);                                  //
                        }                                                                            //
                        ///////////////////////////////////////////////////////////////////////////////

                        string paymentStatus = outputArray["status"].ToString();
                        string paymentValue_a = outputArray["value_a"].ToString();  //get candidate id for validation
                        string paymentAmount = outputArray["amount"].ToString();
                        string paymentVal_id = outputArray["val_id"].ToString();

                        //===========================================
                        string paymentTran_id = outputArray["tran_id"].ToString();
                        string paymentTran_date = outputArray["tran_date"].ToString();
                        string paymentBank_tran_id = outputArray["bank_tran_id"].ToString();
                        string paymentCard_type = outputArray["card_type"].ToString();
                        string paymentValidated_on = outputArray["validated_on"].ToString();
                        string paymentRisk_title = outputArray["risk_title"].ToString();

                        decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                        string paymentStatusUpper = paymentStatus.ToUpper();

                        if ((paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED")) && paymentTran_id.Equals(trans_id))
                        {

                            /////////////////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_2 tmp16 = new DAL.TemporaryTable_2();                   //
                            tmp16.Note = "tmp16 !string.IsNullOrEmpty(url)____" + paymentStatus + "; " + paymentValue_a + "; " + paymentAmount + "; "
                                + paymentVal_id + "; " + paymentTran_id + "; " + paymentTran_date + "; " + paymentBank_tran_id + "; "
                                + paymentCard_type + "; " + paymentValidated_on + "; " + paymentRisk_title + "; ";
                            tmp16.DateTimeNow = DateTime.Now;                                          //
                            using (var db = new GeneralDataManager())                                  //
                            {                                                                          //
                                db.Insert<DAL.TemporaryTable_2>(tmp16);                                //
                            }                                                                          //
                            /////////////////////////////////////////////////////////////////////////////

                            SavePayment(outputArray);

                            if (cPaymentObj != null)
                            {
                                GetSendingInfo(cPaymentObj.CandidateID);
                            }

                        }

                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                /////////////////////////////////////////////////////////////////////////////
                DAL.TemporaryTable_2 tmp99Catch = new DAL.TemporaryTable_2();                   //
                tmp99Catch.Note = "tmp16 !string.IsNullOrEmpty(url)____Error occured while saving success information";
                tmp99Catch.DateTimeNow = DateTime.Now;                                          //
                using (var db = new GeneralDataManager())                                  //
                {                                                                          //
                    db.Insert<DAL.TemporaryTable_2>(tmp99Catch);                                //
                }                                                                          //
                /////////////////////////////////////////////////////////////////////////////
                
                //Response.Redirect("~/Admission/Message.aspx?message=Error occured while saving success information. Please contact administrator.&type=danger", false);
            }
        }

        private void SavePayment(dynamic outputArray)
        {
            try
            {
                long intTransId = Int64.Parse(outputArray["tran_id"].ToString());
                string strTransId = outputArray["tran_id"].ToString();

                DAL.CandidatePayment cPayment = null;

                if (intTransId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        cPayment = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == intTransId)
                            .FirstOrDefault();
                    }
                }

                //TODO:
                //save priority list.

                #region UPDATE CANDIDATE PAYMENT
                if (cPayment != null)
                {
                    DAL.CandidatePayment newCPayment = new DAL.CandidatePayment();
                    newCPayment = cPayment;
                    newCPayment.IsPaid = true;
                    newCPayment.DateModified = DateTime.Now;
                    newCPayment.ModifiedBy = cPayment.CandidateID;
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            db.Update<DAL.CandidatePayment>(newCPayment);
                        }
                    }
                    catch (Exception ex)
                    {

                        /////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp17 = new DAL.TemporaryTable_2();                       //
                        tmp17.Note = "tmp17 Error updating CandidatePayment____" + ex.Message.ToString();
                        tmp17.DateTimeNow = DateTime.Now;                                              //
                        using (var db = new GeneralDataManager())                                      //
                        {                                                                              //
                            db.Insert<DAL.TemporaryTable_2>(tmp17);                                    //
                        }                                                                              //
                        /////////////////////////////////////////////////////////////////////////////////

                    }
                }
                #endregion

                #region INSERT TRANSACTION HISTORY

                DAL.TransactionHistory thExist = null;
                using (var db = new OfficeDataManager())
                {
                    thExist = db.AdmissionDB.TransactionHistories
                        .Where(c => c.TransactionID == strTransId)
                        .FirstOrDefault();
                }

                if (thExist == null) //insert transaction history if it does not exist.
                {
                    ///////////////////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_2 tmp18 = new DAL.TemporaryTable_2();                     //
                    tmp18.Note = "tmp18 thExist____ is null " + intTransId.ToString();           //
                    tmp18.DateTimeNow = DateTime.Now;                                            //
                    using (var db = new GeneralDataManager())                                    //
                    {                                                                            //
                        db.Insert<DAL.TemporaryTable_2>(tmp18);                                  //
                    }                                                                            //
                    ///////////////////////////////////////////////////////////////////////////////

                    DAL.TransactionHistory transactionHistory = new DAL.TransactionHistory();
                    transactionHistory.CandidateID = outputArray["value_a"].ToString();
                    //transactionHistory.CandidateID = candidateId.ToString();
                    transactionHistory.Status = outputArray["status"].ToString();
                    transactionHistory.TransactionDate = outputArray["tran_date"].ToString();
                    transactionHistory.TransactionID = outputArray["tran_id"].ToString();
                    transactionHistory.ValidationID = outputArray["val_id"].ToString();
                    transactionHistory.Amount = outputArray["amount"].ToString();
                    transactionHistory.StoreAmount = outputArray["store_amount"].ToString();
                    transactionHistory.Currency = outputArray["currency"].ToString();
                    transactionHistory.BankTransactionID = outputArray["bank_tran_id"].ToString();
                    transactionHistory.CardType = outputArray["card_type"].ToString();
                    transactionHistory.CardNumber = outputArray["card_no"].ToString();
                    transactionHistory.CardIssuer = outputArray["card_issuer"].ToString();
                    transactionHistory.CardBrand = outputArray["card_brand"].ToString();
                    transactionHistory.CardIssuerCountry = outputArray["card_issuer_country"].ToString();
                    transactionHistory.CardIssuerCountryCode = outputArray["card_issuer_country_code"].ToString();
                    transactionHistory.CurrencyType = outputArray["currency_type"].ToString();
                    transactionHistory.CurrencyAmount = outputArray["currency_amount"].ToString();
                    transactionHistory.CurrencyRate = outputArray["currency_rate"].ToString();
                    transactionHistory.BaseFair = outputArray["base_fair"].ToString();
                    //transactionHistory.ValueA = outputArray["value_a"].ToString();
                    transactionHistory.ValueA = outputArray["value_a"].ToString();
                    transactionHistory.ValueB = outputArray["value_b"].ToString();
                    transactionHistory.ValueC = outputArray["value_c"].ToString();
                    transactionHistory.ValueD = outputArray["value_d"].ToString();
                    transactionHistory.RiskTitle = outputArray["risk_title"].ToString();
                    transactionHistory.RiskLevel = outputArray["risk_level"].ToString();
                    transactionHistory.ApiConnect = outputArray["APIConnect"].ToString();
                    transactionHistory.ValidatedOn = outputArray["validated_on"].ToString();
                    transactionHistory.GwVersion = outputArray["gw_version"].ToString();
                    transactionHistory.CreatedOn = DateTime.Now;
                    transactionHistory.IsManualInsert = false;
                    transactionHistory.IsInHouseCashTransaction = false;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Insert<DAL.TransactionHistory>(transactionHistory);
                        }
                    }
                    catch (Exception ex)
                    {

                        /////////////////////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_2 tmp19 = new DAL.TemporaryTable_2();                       //
                        tmp19.Note = "tmp12 Error inserting TransactionHistory____" + ex.Message.ToString();
                        tmp19.DateTimeNow = DateTime.Now;                                              //
                        using (var db = new GeneralDataManager())                                      //
                        {                                                                              //
                            db.Insert<DAL.TemporaryTable_2>(tmp19);                                    //
                        }                                                                              //
                        /////////////////////////////////////////////////////////////////////////////////

                    }

                }

                #endregion

                //lblName.Text = outputArray["value_b"].ToString();
                //lblPaymentId.Text = outputArray["tran_id"].ToString();

            }
            catch (Exception ex)
            {
                /////////////////////////////////////////////////////////////////////////////////
                DAL.TemporaryTable_2 tmp20 = new DAL.TemporaryTable_2();                       //
                tmp20.Note = "tmp20 Exception savePayment()____" + ex.Message.ToString();      //
                tmp20.DateTimeNow = DateTime.Now;                                              //
                using (var db = new GeneralDataManager())                                      //
                {                                                                              //
                    db.Insert<DAL.TemporaryTable_2>(tmp20);                                    //
                }                                                                              //
                /////////////////////////////////////////////////////////////////////////////////
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
                        //SendSms(candidateSmsPhone, candidateUsername, candidatePassword, candidate.ID);
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
                    smsLog.Attribute1 = "Sms sending failed in IPNSuccessUrl.aspx";
                    smsLog.Attribute2 = "Failed";
                    smsLog.Attribute3 = null;
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
                }
                else //if sms sending passed
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending successful IPNSuccessUrl.aspx";
                    smsLog.Attribute2 = "Success";
                    smsLog.Attribute3 = null;
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
                eLog.Page = "ipnsuccessurl.aspx";
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
                eLog.Page = "ipnsuccessurl.aspx";
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