using System;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

namespace Admission.Admission
{
    public partial class successurlfpgCertificate : System.Web.UI.Page
    {


        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected void Page_Load(object sender, EventArgs e)
        {
            hfData.Value = "";
            String dataReceived = String.Empty;

            decimal paidAmount = -1;
            decimal systemAmount = -1;

            try
            {
                //MerchantTxnNo: Txn20160218122740;TxnResponse: 2;TxnAmount: 1.00;
                //hashkey: 947820d9ff1f47f3999d17a0cc34bd9b,947820d9ff1f47f3999d17a0cc34bd9b;
                //OrderNo: 001;fosterid: BUP516dc38.93489790;Currency: BDT;ConvertionRate: 1.00;
                //String TxnResponse = "2"; //success
                //String MerchantTxnNo = String.Empty;

                StringBuilder builder = new StringBuilder();

                string[] keys = Request.Params.AllKeys;
                //string[] keys = Request.From.AllKeys;

                for (int i = 0; i < keys.Length; i++)
                {
                    builder.Append(keys[i] + ": " + Request.Params[keys[i]] + ";");
                    //builder.Append(keys[i] + ": " + Request.Form[keys[i]] + ";");
                }

                dataReceived = builder.ToString();

                if (!String.IsNullOrEmpty(dataReceived))
                {
                    #region TempTable
                    try
                    {
                        //////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_1 ttdatarec = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived };
                        using (var db = new OfficeDataManager())                        //
                        {                                                               //
                            db.Insert<DAL.TemporaryTable_1>(ttdatarec);                 //
                        }                                                               //
                                                                                        //////////////////////////////////////////////////////////////////
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }
                else
                    hfData.Value = "No data";

                #region REQUEST POST DATA
                string requestForm_MerchantTxnNo = Request.Params["MerchantTxnNo"].ToString();
                string requestForm_TxnResponse = Request.Params["TxnResponse"].ToString();
                string requestForm_TxnAmount = Request.Params["TxnAmount"].ToString();
                string requestForm_hashkey = Request.Params["hashkey"].ToString();
                string requestForm_OrderNo = Request.Params["OrderNo"].ToString();
                string requestForm_fosterid = Request.Params["fosterid"].ToString();
                string requestForm_Currency = Request.Params["Currency"].ToString();
                string requestForm_ConvertionRate = Request.Params["ConvertionRate"].ToString();


                //string requestForm_MerchantTxnNo = Request.Form["MerchantTxnNo"].ToString();
                //string requestForm_TxnResponse = Request.Form["TxnResponse"].ToString();
                //string requestForm_TxnAmount = Request.Form["TxnAmount"].ToString();
                //string requestForm_hashkey = Request.Form["hashkey"].ToString();
                //string requestForm_OrderNo = Request.Form["OrderNo"].ToString();
                //string requestForm_fosterid = Request.Form["fosterid"].ToString();
                //string requestForm_Currency = Request.Form["Currency"].ToString();
                //string requestForm_ConvertionRate = Request.Form["ConvertionRate"].ToString();
                #endregion

                #region GET CANDIDATE PAYMENT
                DAL.CertificateCandidatePayment candidatePaymentObj = null;
                DAL.CertificateBasicInfo candidateObj = null;
                DAL.CandidateUser candidateUserObj = null;
                if (string.IsNullOrEmpty(requestForm_OrderNo))
                {
                    #region TempTable
                    //hfData.Value += "Order no is null or empty ; ";
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 ordno = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Order no is null or empty ;" };
                    using (var db = new OfficeDataManager())                        //
                    {                                                               //
                        //db.Insert<DAL.TemporaryTable_1>(ordno);                     //
                    }                                                               //
                    //////////////////////////////////////////////////////////////////
                    #endregion
                }
                else
                {
                    long candidatePaymentId = -1;
                    candidatePaymentId = Int64.Parse(requestForm_OrderNo);
                    using (var db = new CandidateDataManager())
                    {
                        candidatePaymentObj = db.AdmissionDB.CertificateCandidatePayments
                            .Where(c => c.PaymentId == candidatePaymentId).FirstOrDefault();
                    }

                    if (candidatePaymentObj != null)
                    {
                        systemAmount = Convert.ToDecimal(candidatePaymentObj.Amount);
                        using (var db = new CandidateDataManager())
                        {
                            //candidateObj = db.GetCandidateBasicInfoByID_ND(Convert.ToInt64(candidatePaymentObj.CandidateID));
                            candidateObj = db.AdmissionDB.CertificateBasicInfoes.Find(Convert.ToInt64(candidatePaymentObj.CandidateID));
                        }
                    }
                    else
                    {
                        #region TempTable
                        //hfData.Value += "cp is null ;";
                        //////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_1 cpay = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; CandidatePayment Obj is null ;" };
                        using (var db = new OfficeDataManager())                        //
                        {                                                               //
                                                                                        // db.Insert<DAL.TemporaryTable_1>(cpay);                      //
                        }                                                               //
                        //////////////////////////////////////////////////////////////////
                        #endregion
                    }
                }

                if (candidateObj == null)
                {
                    #region TempTable
                    //hfData.Value += "c is null ; ";
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 cObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Candidate Obj is null ;" };
                    using (var db = new OfficeDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(cObj);                      //
                    }                                                               //
                    //////////////////////////////////////////////////////////////////
                    #endregion
                }
                #endregion

                paidAmount = Convert.ToDecimal(requestForm_TxnAmount);
                if (paidAmount > 0 && systemAmount > 0)
                {
                    if (paidAmount >= systemAmount)
                    {

                        #region Updating CandidatePayment Table for Sussessful Pay
                        CandidatePaymentUpdate(candidatePaymentObj, candidateObj);
                        #endregion


                        #region GET STORE INFORMATION
                        List<DAL.CertificateCandidateFormSl> candidateFormSerials = null;
                        DAL.StoreFoster storeFoster = null;
                        DAL.CertificateAdmissionSetup admissionSetup = null;

                        if (candidatePaymentObj != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                candidateFormSerials = db.AdmissionDB.CertificateCandidateFormSls
                                    .Where(c => c.CandidatePaymentID == candidatePaymentObj.ID).ToList();
                            }
                        }

                        if (candidateFormSerials != null)
                        {
                            if (candidateFormSerials.Count == 1)
                            {
                                //BOTH SINGLE OR MULTIPLE APPLICATION POSSIBILITY

                                DAL.CertificateCandidateFormSl candidateFormSerialObj = null;
                                candidateFormSerialObj = candidateFormSerials.FirstOrDefault();
                                using (var db = new CandidateDataManager())
                                {
                                    admissionSetup = db.AdmissionDB.CertificateAdmissionSetups.Find(candidateFormSerialObj.AdmissionSetupID);
                                }

                                if (admissionSetup != null)
                                {
                                    using (var db = new OfficeDataManager())
                                    {
                                        storeFoster = db.GetFPGStoreByID(admissionSetup.StoreID);
                                    }
                                }
                            }
                            else if (candidateFormSerials.Count > 1)
                            {
                                //MULTIPLE APPLICATION POSSIBILITY
                                using (var db = new OfficeDataManager())
                                {
                                    storeFoster = db.GetFPGStoreActiveMultiplePurchaseStore(true, true);
                                }
                            }
                        }
                        else
                        {
                            //hfData.Value += "candidateFormSerials is null ; ";
                            #region TempTable
                            //////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_1 cfObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; candidateFormSerials is null ;" };
                            using (var db = new OfficeDataManager())                        //
                            {                                                               //
                                db.Insert<DAL.TemporaryTable_1>(cfObj);                     //
                            }                                                               //
                                                                                            //////////////////////////////////////////////////////////////////
                            #endregion
                        }
                        #endregion

                        #region VALIDATE
                        if (storeFoster != null)
                        {
                            string sys_TxnResponse = "2"; //success
                            string sys_MerchantTxnNo = storeFoster.MerchantShortName + candidatePaymentObj.PaymentId.ToString();

                            String phpmd5convertcsharp = FosterPaymentGateway.Convertmd5(string.Concat(string.Concat(sys_TxnResponse, sys_MerchantTxnNo, storeFoster.SecurityKey))).ToLower();

                            if (phpmd5convertcsharp.Equals(requestForm_hashkey))
                            {
                                DAL.TransactionHistoryFPG transactionHistory = new DAL.TransactionHistoryFPG();

                                if (candidateObj != null && candidatePaymentObj != null)
                                {
                                    transactionHistory.CandidateID = Convert.ToInt64(candidatePaymentObj.CandidateID);
                                    transactionHistory.CandidateName = candidateObj.FirstName;
                                    transactionHistory.CandidatePaymentID = Convert.ToInt64(candidatePaymentObj.PaymentId);
                                    transactionHistory.ConversionRate = requestForm_ConvertionRate;
                                    transactionHistory.CreatedBy = Convert.ToInt64(candidatePaymentObj.CandidateID);
                                    transactionHistory.Currency = requestForm_Currency;
                                    transactionHistory.DateCreated = DateTime.Now;
                                    transactionHistory.DateModified = null;
                                    transactionHistory.FosterId = requestForm_fosterid;
                                    transactionHistory.Hashkey = requestForm_hashkey;
                                    transactionHistory.IsManualInsert = false;
                                    transactionHistory.ManualInsertBy = null;
                                    transactionHistory.MerchantTransactionNo = requestForm_MerchantTxnNo;
                                    transactionHistory.MerchantTransactionResponse = requestForm_TxnResponse;
                                    transactionHistory.ModifiedBy = null;
                                    transactionHistory.OrderNo = requestForm_OrderNo;
                                    transactionHistory.Status = "Success";
                                    transactionHistory.SystemGeneratedHashKey = phpmd5convertcsharp;
                                    transactionHistory.TransactionAmount = requestForm_TxnAmount;
                                }

                                SavePayment(candidateObj, candidatePaymentObj, transactionHistory);

                                #region GET CANDIDATE USER
                                using (var db = new CandidateDataManager())
                                {
                                    candidateUserObj = db.AdmissionDB.CandidateUsers.Find(candidateObj.CandidateUserID);
                                }

                                if (candidateUserObj != null)
                                {
                                    //SendSms(candidateObj.SMSPhone, candidateUserObj.UsernameLoginId, candidateUserObj.Password, candidateObj.ID);
                                }
                                else
                                {
                                    #region TempTable
                                    //////////////////////////////////////////////////////////////////
                                    DAL.TemporaryTable_1 cuNull = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Candidate User Obj is null ;" };
                                    using (var db = new OfficeDataManager())                        //
                                    {                                                               //
                                        db.Insert<DAL.TemporaryTable_1>(cuNull);                    //
                                    }                                                               //
                                    //////////////////////////////////////////////////////////////////
                                    #endregion
                                }
                                #endregion

                            }
                            else
                            {
                                //return message
                            }
                        }
                        else
                        {
                            //hfData.Value += "store is null ; ";
                            #region TempTable
                            //////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_1 stObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Store Obj is null ;" };
                            using (var db = new OfficeDataManager())                        //
                            {                                                               //
                                db.Insert<DAL.TemporaryTable_1>(stObj);                     //
                            }                                                               //
                            //////////////////////////////////////////////////////////////////
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region TempTable
                        //////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_1 lessAmntPaid = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; PaidAmount: " + paidAmount + "; SystemAmount:" + systemAmount };
                        using (var db = new OfficeDataManager())                        //
                        {                                                               //
                            db.Insert<DAL.TemporaryTable_1>(lessAmntPaid);              //
                        }                                                               //
                        //////////////////////////////////////////////////////////////////
                        #endregion
                        Response.Redirect("~/Admission/failurlfpg.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                //hfData.Value = "ex: " + ex.Message + "; " + ex.InnerException.Message + " ; ";
                #region TempTable
                //////////////////////////////////////////////////////////////////
                DAL.TemporaryTable_1 exObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Exception: " + ex.Message + "; InnerException:" + ex.InnerException.Message };
                using (var db = new OfficeDataManager())                        //
                {                                                               //
                    db.Insert<DAL.TemporaryTable_1>(exObj);                     //
                }                                                               //
                //////////////////////////////////////////////////////////////////
                #endregion
            }
        }



        private void CandidatePaymentUpdate(DAL.CertificateCandidatePayment candidatePayment, DAL.CertificateBasicInfo candidateObj)
        {
            DAL.CertificateCandidateUser candidateUserObj = new DAL.CertificateCandidateUser();

            //update CandidatePayment
            if (candidatePayment != null)
            {
                candidatePayment.IsPaid = true;
                candidatePayment.ModifiedBy = candidatePayment.CandidateID;
                candidatePayment.DateModified = DateTime.Now;
                try
                {
                    long candidatePaymentUpdateID = -1;
                    using (var db = new CandidateDataManager())
                    {
                        db.Update<DAL.CertificateCandidatePayment>(candidatePayment);
                        candidatePaymentUpdateID = candidatePayment.ID;
                    }


                    using (var db = new CandidateDataManager())
                    {
                        candidateUserObj = db.AdmissionDB.CertificateCandidateUsers.Find(candidateObj.CandidateUserID);
                    }

                    if (candidatePaymentUpdateID > 0 && candidateUserObj != null)
                    {
                        SendSms(candidateObj.SMSPhone, candidateUserObj.UsernameLoginId, candidateUserObj.Password, candidateObj.ID); //, candidateUserObj.UsernameLoginId, candidateUserObj.Password,
                    }

                }
                catch (Exception ex)
                {
                    //hfData.Value += "save cp: " + ex.Message + "; " + ex.InnerException.Message + " ; ";
                    #region TempTable
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 savePayexObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "SavePayment: " + candidatePayment.PaymentId + "; " + candidatePayment.CandidateID + "; Exception: " + ex.Message + " ; InnerException: " + ex.InnerException.Message };
                    using (var db = new OfficeDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(savePayexObj);              //
                    }                                                               //
                    //////////////////////////////////////////////////////////////////
                    #endregion
                }
            }


        }





        private void SavePayment(DAL.CertificateBasicInfo candidate, DAL.CertificateCandidatePayment candidatePayment, DAL.TransactionHistoryFPG transactionHistory)
        {


            //insert into TransactionHistoryFPG
            if (transactionHistory != null)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        db.Insert<DAL.TransactionHistoryFPG>(transactionHistory);
                    }
                }
                catch (Exception ex)
                {
                    //hfData.Value += "save th: " + ex.Message + "; " + ex.InnerException.Message + " ; ";
                    #region TempTable
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 savePayTranexObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "SavePaymentTransaction: " + candidatePayment.PaymentId + "; " + candidatePayment.CandidateID + "; Exception: " + ex.Message + " ; InnerException: " + ex.InnerException.Message };
                    using (var db = new OfficeDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(savePayTranexObj);          //
                    }                                                               //
                    //////////////////////////////////////////////////////////////////
                    #endregion
                }
            }
            lblName.Text = transactionHistory.CandidateName;
            lblPaymentId.Text = transactionHistory.CandidatePaymentID.ToString();
        } //end method SavePayment




        #region Sending SMS

        //private static string userName = "bup789";
        //private static string password = "01769021586";
        //private static string sender = "BUP";

        //public static string Send(string phoneNo, string message)
        //{

        //    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://app.planetgroupbd.com/api/v3/sendsms/plain?user="
        //        + userName + "&password=" + password + "&sender=BUP"
        //        + "&SMSText=" + System.Web.HttpUtility.UrlEncode(message) + "&GSM=" + phoneNo + "&type=longSMS");

        //    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
        //    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
        //    string responseString = respStreamReader.ReadToEnd();
        //    respStreamReader.Close();
        //    myResp.Close();
        //    return responseString;
        //}

        #endregion




        private void SendSms(string smsPhone, string username, string password, long candidateId) //string username, string password,
        {
            if (!string.IsNullOrEmpty(smsPhone) && smsPhone.Count() == 14 && smsPhone.Contains("+"))
            {
                string messageBody = "BUP Admission. Login to https://admission.bup.edu.bd. Username: " + username + " ; Password: " + password + " ";
                string stringData = SMSUtility.Send(smsPhone, messageBody);

                string statusT = JObject.Parse(stringData)["statusCode"].ToString();

                if (statusT != "200") //if sms sending fails
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending failed in successUrlfpgCertificate.aspx";
                    smsLog.Attribute2 = null;
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

                    LogWriter.SmsLog(smsLog);
                }
                else //if sms sending passed
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending successful successUrlfpgCertificate.aspx";
                    smsLog.Attribute2 = null;
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

                    LogWriter.SmsLog(smsLog);
                }
            }
        }//end SendSMS






    }
}