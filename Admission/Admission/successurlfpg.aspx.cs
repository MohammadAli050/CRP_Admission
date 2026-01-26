using CommonUtility;
using DATAMANAGER;
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
    public partial class successurlfpg : System.Web.UI.Page
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

                //string[] keys = Request.Params.AllKeys;
                string[] keys = Request.Form.AllKeys;

                for (int i = 0; i < keys.Length; i++)
                {
                    //builder.Append(keys[i] + ": " + Request.Params[keys[i]] + ";");
                    builder.Append(keys[i] + ": " + Request.Form[keys[i]] + ";");
                }

                dataReceived = builder.ToString();

                if (!String.IsNullOrEmpty(dataReceived))
                {
                    #region TempTable
                    try
                    {
                        //////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_1 tmp1 = new DAL.TemporaryTable_1(); // { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived };
                        tmp1.Note = "tmp1_SuccessURLFPG; kyes[]:___" + builder.ToString() + "_____URL:" + Request.Url.ToString();
                        tmp1.DateTimeNow = DateTime.Now;
                        using (var db = new GeneralDataManager())                        //
                        {                                                               //
                            db.Insert<DAL.TemporaryTable_1>(tmp1);                 //
                        }                                                               //
                        //////////////////////////////////////////////////////////////////
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }
                else
                {
                    hfData.Value = "No data";

                    #region TempTable
                    try
                    {
                        //////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_1 tmp2 = new DAL.TemporaryTable_1(); // { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived };
                        tmp2.Note = "tmp2_SuccessURLFPG; No Data Received!;";
                        tmp2.DateTimeNow = DateTime.Now;
                        using (var db = new GeneralDataManager())                        //
                        {                                                               //
                            db.Insert<DAL.TemporaryTable_1>(tmp2);                 //
                        }                                                               //
                        //////////////////////////////////////////////////////////////////
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    return;
                }

                #region REQUEST POST DATA
                //string requestForm_MerchantTxnNo = Request.Params["MerchantTxnNo"].ToString();
                //string requestForm_TxnResponse = Request.Params["TxnResponse"].ToString();
                //string requestForm_TxnAmount = Request.Params["TxnAmount"].ToString();
                //string requestForm_hashkey = Request.Params["hashkey"].ToString();
                //string requestForm_OrderNo = Request.Params["OrderNo"].ToString();
                //string requestForm_fosterid = Request.Params["fosterid"].ToString();
                //string requestForm_Currency = Request.Params["Currency"].ToString();
                //string requestForm_ConvertionRate = Request.Params["ConvertionRate"].ToString();


                string requestForm_MerchantTxnNo = Request.Form["MerchantTxnNo"].ToString();
                string requestForm_TxnResponse = Request.Form["TxnResponse"].ToString();
                string requestForm_TxnAmount = Request.Form["TxnAmount"].ToString();
                string requestForm_hashkey = Request.Form["hashkey"].ToString();
                string requestForm_OrderNo = Request.Form["OrderNo"].ToString();
                string requestForm_fosterid = Request.Form["fosterid"].ToString();
                string requestForm_Currency = Request.Form["Currency"].ToString();
                string requestForm_ConvertionRate = Request.Form["ConvertionRate"].ToString();
                #endregion

                #region GET CANDIDATE PAYMENT
                DAL.CandidatePayment candidatePaymentObj = null;
                DAL.BasicInfo candidateObj = null;
                DAL.CandidateUser candidateUserObj = null;
                if (string.IsNullOrEmpty(requestForm_OrderNo))
                {
                    #region TempTable
                    //hfData.Value += "Order no is null or empty ; ";
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 tmp3 = new DAL.TemporaryTable_1(); //{ DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Order no is null or empty ;" };
                    tmp3.Note = "tmp3_SuccessURLFPG; Error: FPG OrderNo is null or empty; DataReceived: " + dataReceived + ";";
                    tmp3.DateTimeNow = DateTime.Now;
                    using (var db = new GeneralDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(tmp3);                     //
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
                        candidatePaymentObj = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == candidatePaymentId).FirstOrDefault();
                    }

                    if (candidatePaymentObj != null)
                    {
                        systemAmount = Convert.ToDecimal(candidatePaymentObj.Amount);
                        using (var db = new CandidateDataManager())
                        {
                            candidateObj = db.GetCandidateBasicInfoByID_ND(Convert.ToInt64(candidatePaymentObj.CandidateID));
                        }
                    }
                    else
                    {
                        #region TempTable
                        //hfData.Value += "cp is null ;";
                        //////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_1 tmp4 = new DAL.TemporaryTable_1();// { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; CandidatePayment Obj is null ;"};
                        tmp4.Note = "tmp4_SuccessURLFPG; Error: CandidatePayment Obj is null; DataReceived: " + dataReceived + ";";
                        tmp4.DateTimeNow = DateTime.Now;
                        using (var db = new GeneralDataManager())                        //
                        {                                                               //
                           db.Insert<DAL.TemporaryTable_1>(tmp4);                      //
                        }                                                               //
                        //////////////////////////////////////////////////////////////////
                        #endregion
                    }
                }

                if(candidateObj == null)
                {
                    #region TempTable
                    //hfData.Value += "c is null ; ";
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 tmp5 = new DAL.TemporaryTable_1();// { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Candidate Obj is null ;" };
                    tmp5.Note = "tmp5_SuccessURLFPG; Error: candidateObj is null;";
                    tmp5.DateTimeNow = DateTime.Now;
                    using (var db = new GeneralDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(tmp5);                      //
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

                        try
                        {
                            #region Updating CandidatePayment Table for Sussessful Pay
                            CandidatePaymentUpdate(candidatePaymentObj, candidateObj);
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            #region TempTable
                            //hfData.Value += "c is null ; ";
                            //////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_1 tmp8 = new DAL.TemporaryTable_1();// { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Candidate Obj is null ;" };
                            tmp8.Note = "tmp8_SuccessURLFPG; Exception: " + ex.Message.ToString() + "; InnerException: " + ex.InnerException.Message.ToString() + ";";
                            tmp8.DateTimeNow = DateTime.Now;
                            using (var db = new GeneralDataManager())                        //
                            {                                                               //
                                db.Insert<DAL.TemporaryTable_1>(tmp8);                      //
                            }                                                               //
                                                                                            //////////////////////////////////////////////////////////////////
                            #endregion


                        }


                        #region GET STORE INFORMATION
                        List<DAL.CandidateFormSl> candidateFormSerials = null;
                        DAL.StoreFoster storeFoster = null;
                        DAL.AdmissionSetup admissionSetup = null;

                        if (candidatePaymentObj != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                candidateFormSerials = db.AdmissionDB.CandidateFormSls
                                    .Where(c => c.CandidatePaymentID == candidatePaymentObj.ID).ToList();
                            }
                        }

                        if (candidateFormSerials != null)
                        {
                            if (candidateFormSerials.Count == 1)
                            {
                                //BOTH SINGLE OR MULTIPLE APPLICATION POSSIBILITY

                                DAL.CandidateFormSl candidateFormSerialObj = null;
                                candidateFormSerialObj = candidateFormSerials.FirstOrDefault();
                                using (var db = new CandidateDataManager())
                                {
                                    admissionSetup = db.AdmissionDB.AdmissionSetups.Find(candidateFormSerialObj.AdmissionSetupID);
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
                            #region TempTable
                            //////////////////////////////////////////////////////////////////
                            DAL.TemporaryTable_1 tmp9 = new DAL.TemporaryTable_1();//{ DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; candidateFormSerials is null ;" };
                            tmp9.Note = "tmp9_SuccessURLFPG; candidateFormSerials is null;";
                            tmp9.DateTimeNow = DateTime.Now;
                            using (var db = new GeneralDataManager())                        //
                            {                                                               //
                                db.Insert<DAL.TemporaryTable_1>(tmp9);                      //
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

                                #region N/A -- GET CANDIDATE USER
                                //using (var db = new CandidateDataManager())
                                //{
                                //    candidateUserObj = db.AdmissionDB.CandidateUsers.Find(candidateObj.CandidateUserID);
                                //}

                                //if(candidateUserObj != null)
                                //{
                                //    //SendSms(candidateObj.SMSPhone, candidateUserObj.UsernameLoginId, candidateUserObj.Password, candidateObj.ID);
                                //}
                                //else
                                //{
                                //    #region TempTable
                                //    //////////////////////////////////////////////////////////////////
                                //    DAL.TemporaryTable_1 tmp11 = new DAL.TemporaryTable_1();// { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Candidate User Obj is null ;" };
                                //    tmp11.Note = "tmp11_SuccessURLFPG; Candidate User Obj is null";
                                //    tmp11.DateTimeNow = DateTime.Now;
                                //    using (var db = new GeneralDataManager())                        //
                                //    {                                                               //
                                //        db.Insert<DAL.TemporaryTable_1>(tmp11);                    //
                                //    }                                                               //
                                //    //////////////////////////////////////////////////////////////////
                                //    #endregion
                                //}
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
                            DAL.TemporaryTable_1 tmp11 = new DAL.TemporaryTable_1(); // { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Store Obj is null ;" };
                            tmp11.Note = "tmp11_SuccessURLFPG; Store Obj is null";
                            using (var db = new GeneralDataManager())                        //
                            {                                                               //
                                db.Insert<DAL.TemporaryTable_1>(tmp11);                     //
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
                        DAL.TemporaryTable_1 tmp12 = new DAL.TemporaryTable_1(); // { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; PaidAmount: " + paidAmount + "; SystemAmount:" + systemAmount };
                        tmp12.Note = "tmp12_SuccessURLFPG; No Store Found; PaidAmount: " + paidAmount + "; SystemAmount:" + systemAmount + ";";
                        using (var db = new GeneralDataManager())                        //
                        {                                                               //
                            db.Insert<DAL.TemporaryTable_1>(tmp12);              //
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
                DAL.TemporaryTable_1 tmp13 = new DAL.TemporaryTable_1(); //{ DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Exception: " + ex.Message + "; InnerException:" + ex.InnerException.Message };
                tmp13.Note = "tmp13_SuccessURLFPG; Exception: " + ex.Message + "; InnerException:" + ex.InnerException.Message + ";";
                using (var db = new GeneralDataManager())                        //
                {                                                               //
                    db.Insert<DAL.TemporaryTable_1>(tmp13);                     //
                }                                                               //
                //////////////////////////////////////////////////////////////////
                #endregion
            }
        }



        private void CandidatePaymentUpdate(DAL.CandidatePayment candidatePayment, DAL.BasicInfo candidateObj)
        {
            DAL.CandidateUser candidateUserObj = new DAL.CandidateUser();

            //update CandidatePayment
            if (candidatePayment != null && candidatePayment.IsPaid == false)
            {
                candidatePayment.IsPaid = true;
                candidatePayment.ModifiedBy = candidatePayment.CandidateID;
                candidatePayment.DateModified = DateTime.Now;
                try
                {
                    long candidatePaymentUpdateID = -1;

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            db.Update<DAL.CandidatePayment>(candidatePayment);
                            candidatePaymentUpdateID = candidatePayment.ID;
                        }

                    }
                    catch (Exception ex)
                    {
                        #region TempTable
                        //////////////////////////////////////////////////////////////////
                        DAL.TemporaryTable_1 tmp6 = new DAL.TemporaryTable_1();// { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Candidate Obj is null ;" };
                        tmp6.Note = "tmp6_SuccessURLFPG; Error: CandidatePaymentUpdate() => CandidatePayment; Payment Update Failed; PaymentID: " + candidatePayment.PaymentId + "; CandidateId: "+ candidatePayment.CandidateID + "; Exception: " + ex.Message + "; InnerException: " + ex.InnerException.Message + ";";
                        tmp6.DateTimeNow = DateTime.Now;
                        using (var db = new GeneralDataManager())                        //
                        {                                                               //
                            db.Insert<DAL.TemporaryTable_1>(tmp6);                      //
                        }                                                               //
                        //////////////////////////////////////////////////////////////////
                        #endregion
                    }


                    using (var db = new CandidateDataManager())
                    {
                        candidateUserObj = db.AdmissionDB.CandidateUsers.Find(candidateObj.CandidateUserID);
                    }

                    if (candidatePaymentUpdateID > 0 && candidateUserObj != null)
                    {
                        SendSms(candidateObj.SMSPhone, candidateUserObj.UsernameLoginId, candidateUserObj.Password, candidateObj.ID);
                    }

                }
                catch (Exception ex)
                {
                    #region TempTable
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 tmp7 = new DAL.TemporaryTable_1();// { DateTimeNow = DateTime.Now, Note = "DataReceived: " + dataReceived + " ; Candidate Obj is null ;" };
                    tmp7.Note = "tmp7_SuccessURLFPG; Error: CandidatePaymentUpdate(); Payment Update Failed; PaymentID: " + candidatePayment.PaymentId + "; CandidateId: " + candidatePayment.CandidateID + "; Exception: " + ex.Message + "; InnerException: " + ex.InnerException.Message + ";";
                    tmp7.DateTimeNow = DateTime.Now;
                    using (var db = new GeneralDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(tmp7);                      //
                    }                                                               //
                                                                                    //////////////////////////////////////////////////////////////////
                    #endregion
                }
            }
            

        }





        private void SavePayment(DAL.BasicInfo candidate, DAL.CandidatePayment candidatePayment, DAL.TransactionHistoryFPG transactionHistory)
        {
            

            //insert into TransactionHistoryFPG
            if(transactionHistory != null)
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
                    DAL.TemporaryTable_1 tmp10 = new DAL.TemporaryTable_1(); // { DateTimeNow = DateTime.Now, Note = "SavePaymentTransaction: " + candidatePayment.PaymentId + "; " + candidatePayment.CandidateID + "; Exception: " + ex.Message + " ; InnerException: " + ex.InnerException.Message };
                    tmp10.Note = "tmp10_SuccessURLFPG; SavePaymentTransaction: " + candidatePayment.PaymentId + "; " + candidatePayment.CandidateID + "; Exception: " + ex.Message + " ; InnerException: " + ex.InnerException.Message;
                    tmp10.DateTimeNow = DateTime.Now;
                    using (var db = new GeneralDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(tmp10);          //
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




        private void SendSms(string smsPhone, string username, string password, long candidateId)
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
                    smsLog.Attribute1 = "Sms sending failed in successUrlfpg.aspx";
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

                    LogWriter.SmsLog(smsLog);
                }
                else //if sms sending passed
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending successful successUrlfpg.aspx";
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

                    LogWriter.SmsLog(smsLog);
                }
            }
        }//end SendSMS



        



    }
}