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
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class IPNListener : System.Web.UI.Page
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



                    #region Log Insert IPN_01
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = 0;
                        dLog.Attribute2 = Request.Form["tran_id"].ToString();
                        dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                        dLog.EventName = "sslcommerz";
                        dLog.PageName = "IPNListener.aspx";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "IPN_01: All Retrun Parameters: " + builder.ToString();
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion


                    #region N/A
                    ///////////////////////////////////////////////////////////////////////////////////
                    //DAL.TemporaryTable_2 tmp1 = new DAL.TemporaryTable_2();                        //
                    //tmp1.Note = "IPN_tmp1 kyes[]:___" + builder.ToString();                            //
                    //tmp1.DateTimeNow = DateTime.Now;                                               //
                    //using (var db = new GeneralDataManager())                                      //
                    //{                                                                              //
                    //    db.Insert<DAL.TemporaryTable_2>(tmp1);                                   //
                    //}                                                                              //
                    /////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Post Request Data
                    string amount = Request.Form["amount"].ToString(); // Amount with service charge
                    string status = Request.Form["status"].ToString(); //payment status
                    string trans_id = Request.Form["tran_id"].ToString(); // payment id system is sending
                    string val_id = Request.Form["val_id"].ToString(); // SSL ID
                    string value_a = Request.Form["value_a"].ToString(); // candidate ID
                    string value_b = Request.Form["value_b"].ToString(); //candidate first name
                    string value_c = Request.Form["value_c"].ToString(); // Amount from system.
                    string value_d = Request.Form["value_d"].ToString(); // store id
                    str = amount;

                    #region N/A
                    ///////////////////////////////////////////////////////////////////////////////////
                    //DAL.TemporaryTable_2 tmp2 = new DAL.TemporaryTable_2();                        //
                    //tmp2.Note = "IPN_tmp2 post request data:___" + amount + "; " + status + "; " + trans_id + "; " + val_id + "; " + value_a + "; "
                    //     + value_b + "; " + value_c + "; " + value_d + "; ";                       //
                    //tmp2.DateTimeNow = DateTime.Now;                                               //
                    //using (var db = new GeneralDataManager())                                      //
                    //{                                                                              //
                    //    db.Insert<DAL.TemporaryTable_2>(tmp2);                                   //
                    //}                                                                              //
                    /////////////////////////////////////////////////////////////////////////////////// 
                    #endregion


                    #region Log Insert IPN_02
                    try
                    {

                        string msg = "amount: " + amount + ";" +
                                     "status:" + status + ";" +
                                     "trans_id: " + trans_id + ";" +
                                     "val_id: " + val_id + ";" +
                                     "value_a: " + value_a + ";" +
                                     "value_b: " + value_b + ";" +
                                     "value_c: " + value_c + ";" +
                                     "value_d: " + value_d + ";";

                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = 0;
                        dLog.Attribute2 = Request.Form["tran_id"].ToString();
                        dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                        dLog.EventName = "sslcommerz";
                        dLog.PageName = "IPNListener.aspx";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "IPN_02: Data: " + msg;
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion Log Insert IPN_02

                    #endregion Post Request Data

                    //(!string.IsNullOrEmpty(amount) && Convert.ToDecimal(amount) > 0) &&
                    //    !string.IsNullOrEmpty(status) &&
                    //    (!string.IsNullOrEmpty(trans_id) && Convert.ToInt64(trans_id) > 0) &&
                    //    !string.IsNullOrEmpty(val_id) &&
                    //    (!string.IsNullOrEmpty(value_a) && Convert.ToInt64(value_a) > 0) &&
                    //    !string.IsNullOrEmpty(value_b) &&
                    //    (!string.IsNullOrEmpty(value_c) && Convert.ToInt64(value_c) > 0) &&
                    //    (!string.IsNullOrEmpty(value_d) && Convert.ToInt64(value_d) > 0)

                    if (!string.IsNullOrEmpty(trans_id))
                    {
                        DAL.CandidatePayment cPaymentObj = null;
                        try
                        {
                            long candidatePaymentId = Convert.ToInt64(trans_id);//Int64.Parse(trans_id);
                            using (var db = new CandidateDataManager())
                            {
                                cPaymentObj = db.AdmissionDB.CandidatePayments
                                    .Where(c => c.PaymentId == candidatePaymentId)
                                    .FirstOrDefault();
                            }
                        }
                        catch (Exception ex)
                        {
                            #region Log Insert IPN_02.1
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = 0;
                                dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                dLog.EventName = "sslcommerz";
                                dLog.PageName = "IPNListener.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "IPN_02.1: Exception: " + ex.Message.ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex1)
                            {
                            }
                            #endregion
                        }

                        if (cPaymentObj != null && cPaymentObj.IsPaid == false)
                        {
                            #region Log Insert IPN_03
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = 0;
                                dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                dLog.EventName = "sslcommerz";
                                dLog.PageName = "IPNListener.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "IPN_03: PaymentId: " + cPaymentObj.PaymentId.ToString() + "; CandidateId: " + cPaymentObj.CandidateID + "; Amount: " + cPaymentObj.Amount.ToString() + "; IsPaid: No";
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            int storeId = 0;
                            storeId = Int32.Parse(value_d);
                            DAL.Store storeObj = null;
                            using (var db = new OfficeDataManager())
                            {
                                storeObj = db.AdmissionDB.Stores.Find(storeId);
                            }

                            if (storeObj != null && (!string.IsNullOrEmpty(Decrypt.DecryptString(storeObj.StoreId)) && !string.IsNullOrEmpty(Decrypt.DecryptString(storeObj.StorePass))))
                            {
                                #region Log Insert IPN_04
                                try
                                {
                                    string msg = "StoreName: " + storeObj.StoreName + "; " +
                                                 "IsActive: " + storeObj.IsActive + "; " +
                                                 "IsMultipleAllowed: " + storeObj.IsMultipleAllowed + ";";

                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = 0;
                                    dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                    dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                    dLog.EventName = "sslcommerz";
                                    dLog.PageName = "IPNListener.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "IPN_04: StoreInfo: " + msg;
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion


                                #region VALIDATE

                                string plainText_storeId = null;
                                string plainText_password = null;

                                plainText_storeId = Decrypt.DecryptString(storeObj.StoreId);
                                plainText_password = Decrypt.DecryptString(storeObj.StorePass);

                                string url = null;


                                url = String.Format(@"https://securepay.sslcommerz.com/validator/api/validationserverAPI.php?val_id={0}&store_id={1}&store_passwd={2}"
                                        , val_id, plainText_storeId, plainText_password);

                                #region N/A
                                /////////////////////////////////////////////////////////////////////////////////
                                //DAL.TemporaryTable_2 tmp13 = new DAL.TemporaryTable_2();                     //
                                //tmp13.Note = "IPN_tmp13 url____" + url;                                          //
                                //tmp13.DateTimeNow = DateTime.Now;                                            //
                                //using (var db = new GeneralDataManager())                                    //
                                //{                                                                            //
                                //    db.Insert<DAL.TemporaryTable_2>(tmp13);                                  //
                                //}                                                                            //
                                //                                                                             ///////////////////////////////////////////////////////////////////////////////



                                //if (string.IsNullOrEmpty(plainText_storeId) && string.IsNullOrEmpty(plainText_password))
                                //{
                                //    url = null;

                                //    ////////////////////////////////////////////////////////////////////////////////////////
                                //    DAL.TemporaryTable_2 tmp12 = new DAL.TemporaryTable_2();                              //
                                //    tmp12.Note = "IPN_tmp12 string.IsNullOrEmpty(plainText_storeId) && string.IsNullOrEmpty(plainText_password)____ url = null";
                                //    tmp12.DateTimeNow = DateTime.Now;                                                     //
                                //    using (var db = new GeneralDataManager())                                             //
                                //    {                                                                                     //
                                //        db.Insert<DAL.TemporaryTable_2>(tmp12);                                           //
                                //    }                                                                                     //
                                //                                                                                          ////////////////////////////////////////////////////////////////////////////////////////

                                //    Response.Redirect("~/Admission/Message.aspx?message=Error occured while getting validation information. Please contact administrator.&type=danger", false);
                                //}
                                //else
                                //{

                                //} 
                                #endregion

                                if (!string.IsNullOrEmpty(url))
                                {
                                    var jsonData = new WebClient().DownloadString(url);

                                    #region Log Insert IPN_05
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = 0;
                                        dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                        dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                        dLog.EventName = "sslcommerz";
                                        dLog.PageName = "IPNListener.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "IPN_05: SSL API Response Data; Data: " + jsonData;
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion


                                    #region N/A
                                    /////////////////////////////////////////////////////////////////////////////////
                                    //DAL.TemporaryTable_2 tmp14 = new DAL.TemporaryTable_2();                     //
                                    //tmp14.Note = "IPN_tmp14 jsonData____" + jsonData;                                //
                                    //tmp14.DateTimeNow = DateTime.Now;                                            //
                                    //using (var db = new GeneralDataManager())                                    //
                                    //{                                                                            //
                                    //    db.Insert<DAL.TemporaryTable_2>(tmp14);                                  //
                                    //}                                                                            //
                                    //                                                                             ///////////////////////////////////////////////////////////////////////////////


                                    /////////////////////////////////////////////////////////////////////////////////
                                    //DAL.TemporaryTable_2 tmp15 = new DAL.TemporaryTable_2();                     //
                                    //tmp15.Note = "IPN_tmp15 outputArray____" + outputArray;                          //
                                    //tmp15.DateTimeNow = DateTime.Now;                                            //
                                    //using (var db = new GeneralDataManager())                                    //
                                    //{                                                                            //
                                    //    db.Insert<DAL.TemporaryTable_2>(tmp15);                                  //
                                    //}                                                                            //
                                    //                                                                             ///////////////////////////////////////////////////////////////////////////////


                                    #endregion

                                    dynamic outputArray = JsonConvert.DeserializeObject<dynamic>(jsonData);

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

                                    //decimal validateResultAmount = Convert.ToDecimal(paymentAmount);
                                    string paymentStatusUpper = paymentStatus.ToUpper();



                                    #region Log Insert IPN_06
                                    try
                                    {
                                        string msg = "Status :: " + paymentStatus +
                                                    "; Amount :: " + amount +
                                                    "; Transaction ID :: " + paymentTran_id +
                                                    "; SSL ID :: " + val_id +
                                                    "; Payment Attempt ID :: " + value_a +
                                                    "; Tran date :: " + paymentTran_date +
                                                    "; Bank tran ID :: " + paymentBank_tran_id +
                                                    "; Card type :: " + paymentCard_type +
                                                    "; Validated ON :: " + paymentValidated_on +
                                                    "; Risk title :: " + paymentRisk_title;

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = 0;
                                        dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                        dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                        dLog.EventName = "sslcommerz";
                                        dLog.PageName = "IPNListener.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "IPN_06: SSL API Called: " + msg;
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion




                                    if (
                                        (paymentStatusUpper.Equals("VALID") || paymentStatusUpper.Equals("VALIDATED")) &&
                                        paymentTran_id.Equals(trans_id)
                                       )
                                    {

                                        #region N/A
                                        ///////////////////////////////////////////////////////////////////////////////
                                        //DAL.TemporaryTable_2 tmp16 = new DAL.TemporaryTable_2();                   //
                                        //tmp16.Note = "IPN_tmp16 !string.IsNullOrEmpty(url)____" + paymentStatus + "; " + paymentValue_a + "; " + paymentAmount + "; "
                                        //    + paymentVal_id + "; " + paymentTran_id + "; " + paymentTran_date + "; " + paymentBank_tran_id + "; "
                                        //    + paymentCard_type + "; " + paymentValidated_on + "; " + paymentRisk_title + "; ";
                                        //tmp16.DateTimeNow = DateTime.Now;                                          //
                                        //using (var db = new GeneralDataManager())                                  //
                                        //{                                                                          //
                                        //    db.Insert<DAL.TemporaryTable_2>(tmp16);                                //
                                        //}                                                                          //
                                        //                                                                           ///////////////////////////////////////////////////////////////////////////// 
                                        #endregion

                                        SavePayment(outputArray);


                                    }
                                    else
                                    {
                                        // SSL API Validation Failed

                                        #region Log Insert IPN_13
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = 0;
                                            dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                            dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                            dLog.EventName = "sslcommerz";
                                            dLog.PageName = "IPNListener.aspx";
                                            //dLog.OldData = logOldObject;
                                            dLog.NewData = "IPN_13: SSL API Validation Failed";
                                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex1)
                                        {
                                        }
                                        #endregion
                                    }

                                }
                                else
                                {
                                    // SSL Validate URL is Null

                                    #region Log Insert IPN_14
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = 0;
                                        dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                        dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                        dLog.EventName = "sslcommerz";
                                        dLog.PageName = "IPNListener.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "IPN_14: SSL Validate URL is Null";
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


                            }
                            else
                            {
                                // No Store Info Found

                                #region Log Insert IPN_15
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = 0;
                                    dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                    dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                    dLog.EventName = "sslcommerz";
                                    dLog.PageName = "IPNListener.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "IPN_15: Store Info Not Found";
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex1)
                                {
                                }
                                #endregion
                            }



                            #region N/A
                            //#region GET STORE INFO



                            //if (string.IsNullOrEmpty(value_d))
                            //{

                            //    /////////////////////////////////////////////////////////////////////////////////
                            //    DAL.TemporaryTable_2 tmp7 = new DAL.TemporaryTable_2();                        //
                            //    tmp7.Note = "IPN_tmp7 string.IsNullOrEmpty(value_d)____" + value_d;                //
                            //    tmp7.DateTimeNow = DateTime.Now;                                               //
                            //    using (var db = new GeneralDataManager())                                      //
                            //    {                                                                              //
                            //        db.Insert<DAL.TemporaryTable_2>(tmp7);                                     //
                            //    }                                                                              //
                            //                                                                                   /////////////////////////////////////////////////////////////////////////////////

                            //}
                            //else
                            //{


                            //    /////////////////////////////////////////////////////////////////////////////////
                            //    DAL.TemporaryTable_2 tmp8 = new DAL.TemporaryTable_2();                        //
                            //    tmp8.Note = "IPN_tmp8 storeIdStr___" + storeId + ";" + storeIdStr + "; " + value_d.ToString() + "; " + Int32.Parse(storeIdStr) + "; " + Int32.Parse(value_d);
                            //    tmp8.DateTimeNow = DateTime.Now;                                               //
                            //    using (var db = new GeneralDataManager())                                      //
                            //    {                                                                              //
                            //        db.Insert<DAL.TemporaryTable_2>(tmp8);                                   //
                            //    }                                                                              //
                            //                                                                                   /////////////////////////////////////////////////////////////////////////////////

                            //    if (storeId > 0)
                            //    {

                            //        /////////////////////////////////////////////////////////////////////////////////
                            //        DAL.TemporaryTable_2 tmp9 = new DAL.TemporaryTable_2();                        //
                            //        tmp9.Note = "IPN_tmp9 storeId > 0___" + storeId;                                   //
                            //        tmp9.DateTimeNow = DateTime.Now;                                               //
                            //        using (var db = new GeneralDataManager())                                      //
                            //        {                                                                              //
                            //            db.Insert<DAL.TemporaryTable_2>(tmp9);                                   //
                            //        }                                                                              //
                            //                                                                                       /////////////////////////////////////////////////////////////////////////////////



                            //        /////////////////////////////////////////////////////////////////////////////////
                            //        DAL.TemporaryTable_2 tmp10 = new DAL.TemporaryTable_2();                       //
                            //        tmp10.Note = "IPN_tmp10 storeObj____" + storeObj.StoreName + "; " + storeObj.StoreId + "; " + storeObj.StorePass + "; " + Decrypt.DecryptString(storeObj.StoreId) + Decrypt.DecryptString(storeObj.StorePass);
                            //        tmp10.DateTimeNow = DateTime.Now;                                              //
                            //        using (var db = new GeneralDataManager())                                      //
                            //        {                                                                              //
                            //            db.Insert<DAL.TemporaryTable_2>(tmp10);                                    //
                            //        }                                                                              //
                            //                                                                                       /////////////////////////////////////////////////////////////////////////////////

                            //    }
                            //}

                            //#endregion 
                            #endregion


                        }
                        else
                        {
                            // This Payment ID is Already Paid

                            #region Log Insert IPN_16
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = 0;
                                dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                dLog.EventName = "sslcommerz";
                                dLog.PageName = "IPNListener.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "IPN_16: Payment is already updated; PaymentId: " + cPaymentObj.PaymentId.ToString() + "; CandidateId: " + cPaymentObj.CandidateID + "; Amount: " + cPaymentObj.Amount.ToString() + "; IsPaid: Yes";
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex1)
                            {
                            }
                            #endregion
                        }



                        #region N/A
                        //#region GET CANDIDATE PAYMENT


                        //if (string.IsNullOrEmpty(trans_id))
                        //{

                        //    ///////////////////////////////////////////////////////////////////////////////////
                        //    //DAL.TemporaryTable_2 tmp3 = new DAL.TemporaryTable_2();                        //
                        //    //tmp3.Note = "IPN_tmp3 string.IsNullOrEmpty(trans_id)___" + trans_id;               //
                        //    //tmp3.DateTimeNow = DateTime.Now;                                               //
                        //    //using (var db = new GeneralDataManager())                                      //
                        //    //{                                                                              //
                        //    //    db.Insert<DAL.TemporaryTable_2>(tmp3);                                   //
                        //    //}                                                                              //
                        //    ///////////////////////////////////////////////////////////////////////////////////




                        //}
                        //else
                        //{
                        //    //long candidatePaymentId = -1;
                        //    //candidatePaymentId = Int64.Parse(trans_id);


                        //    //if (candidatePaymentId > 0)
                        //    //{
                        //    //    using (var db = new CandidateDataManager())
                        //    //    {
                        //    //        cPaymentObj = db.AdmissionDB.CandidatePayments
                        //    //            .Where(c => c.PaymentId == candidatePaymentId)
                        //    //            .FirstOrDefault();
                        //    //    }
                        //    //}
                        //    //else
                        //    //{

                        //    //}


                        //    /////////////////////////////////////////////////////////////////////////////////////
                        //    ////DAL.TemporaryTable_2 tmp4 = new DAL.TemporaryTable_2();                        //
                        //    ////tmp4.Note = "IPN_tmp4 candidatePaymentId___" + candidatePaymentId;                 //
                        //    ////tmp4.DateTimeNow = DateTime.Now;                                               //
                        //    ////using (var db = new GeneralDataManager())                                      //
                        //    ////{                                                                              //
                        //    ////    db.Insert<DAL.TemporaryTable_2>(tmp4);                                   //
                        //    ////}                                                                              //
                        //    /////////////////////////////////////////////////////////////////////////////////////



                        //    //if (cPaymentObj != null)
                        //    //{
                        //    //    systemAmount = cPaymentObj.Amount;

                        //    //    /////////////////////////////////////////////////////////////////////////////////
                        //    //    DAL.TemporaryTable_2 tmp5 = new DAL.TemporaryTable_2();                        //
                        //    //    tmp5.Note = "IPN_tmp5 cPaymentObj != null____" + systemAmount;                     //
                        //    //    tmp5.DateTimeNow = DateTime.Now;                                               //
                        //    //    using (var db = new GeneralDataManager())                                      //
                        //    //    {                                                                              //
                        //    //        db.Insert<DAL.TemporaryTable_2>(tmp5);                                   //
                        //    //    }                                                                              //
                        //    //    /////////////////////////////////////////////////////////////////////////////////

                        //    //}
                        //    //else
                        //    //{

                        //    //    /////////////////////////////////////////////////////////////////////////////////
                        //    //    DAL.TemporaryTable_2 tmp6 = new DAL.TemporaryTable_2();                        //
                        //    //    tmp6.Note = "IPN_tmp6 cPaymentObj else____ cPaymentObj is null";                   //
                        //    //    tmp6.DateTimeNow = DateTime.Now;                                               //
                        //    //    using (var db = new GeneralDataManager())                                      //
                        //    //    {                                                                              //
                        //    //        db.Insert<DAL.TemporaryTable_2>(tmp6);                                   //
                        //    //    }                                                                              //
                        //    //    /////////////////////////////////////////////////////////////////////////////////

                        //    //}
                        //}

                        //#endregion 
                        #endregion


                    }
                    else
                    {
                        // Requested Paramiter Issue

                        #region Log Insert IPN_17
                        try
                        {
                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = 0;
                            dLog.Attribute2 = Request.Form["tran_id"].ToString();
                            dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                            dLog.EventName = "sslcommerz";
                            dLog.PageName = "IPNListener.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "IPN_17: Invalid Request.";
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex1)
                        {
                        }
                        #endregion

                    }
                }
                else
                {
                    #region Log Insert IPN_18
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = 0;
                        dLog.Attribute2 = Request.Form["tran_id"].ToString();
                        dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                        dLog.EventName = "sslcommerz";
                        dLog.PageName = "IPNListener.aspx";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "IPN_18: Invalid Request.";
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex1)
                    {
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region N/A
                ///////////////////////////////////////////////////////////////////////////////
                //DAL.TemporaryTable_2 tmp99Catch = new DAL.TemporaryTable_2();                   //
                //tmp99Catch.Note = "tmp16 !string.IsNullOrEmpty(url)____Error occured while saving success information";
                //tmp99Catch.DateTimeNow = DateTime.Now;                                          //
                //using (var db = new GeneralDataManager())                                  //
                //{                                                                          //
                //    db.Insert<DAL.TemporaryTable_2>(tmp99Catch);                                //
                //}                                                                          //
                //                                                                           /////////////////////////////////////////////////////////////////////////////

                ////Response.Redirect("~/Admission/Message.aspx?message=Error occured while saving success information. Please contact administrator.&type=danger", false); 
                #endregion


                #region Log Insert IPN_19
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.DateTime = DateTime.Now;
                    dLog.DateCreated = DateTime.Now;
                    dLog.UserId = 0;
                    dLog.Attribute2 = Request.Form["tran_id"].ToString();
                    dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                    dLog.EventName = "sslcommerz";
                    dLog.PageName = "IPNListener.aspx";
                    //dLog.OldData = logOldObject;
                    dLog.NewData = "IPN_19: Exception: " + ex.Message.ToString();
                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ex1)
                {
                }
                #endregion


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
                if (cPayment != null && cPayment.IsPaid == false)
                {
                    #region Update Payment
                    //DAL.CandidatePayment newCPayment = new DAL.CandidatePayment();
                    //newCPayment = cPayment;
                    //newCPayment.IsPaid = true;
                    //newCPayment.ModifiedBy = cPayment.CandidateID;
                    //newCPayment.DateModified = DateTime.Now;

                    cPayment.IsPaid = true;
                    cPayment.ModifiedBy = cPayment.CandidateID;
                    cPayment.DateModified = DateTime.Now;

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            db.Update<DAL.CandidatePayment>(cPayment);
                        }


                        #region Is Final Submit

                        try
                        {
                            int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                            if (AllStepInOneTime == 1)
                            {
                                if (cPayment != null)
                                {
                                    #region Is Final Submit

                                    DAL.AdditionalInfo additionalInfoObj = null;
                                    using (var db = new CandidateDataManager())
                                    {
                                        long cId = Convert.ToInt64(cPayment.CandidateID);
                                        additionalInfoObj = db.GetAdditionalInfoByCandidateID_ND(cId);

                                        if (additionalInfoObj != null)
                                        {
                                            additionalInfoObj.IsFinalSubmit = true;
                                            additionalInfoObj.DateModified = DateTime.Now;
                                            additionalInfoObj.ModifiedBy = cId;
                                            using (var dbUpdate = new CandidateDataManager())
                                            {
                                                dbUpdate.Update<DAL.AdditionalInfo>(additionalInfoObj);

                                                try
                                                {
                                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                    dLog.DateTime = DateTime.Now;
                                                    dLog.DateCreated = DateTime.Now;
                                                    dLog.UserId = 0;
                                                    dLog.CandidateId = cId;
                                                    dLog.EventName = "Final Submit (Candidate)";
                                                    dLog.PageName = "successurl.aspx";
                                                    //dLog.OldData = logOldObject;
                                                    dLog.NewData = " Final Submit to ," + " PaymentID: " + cPayment.PaymentId.ToString() + "; CandidateID: " + cId.ToString() + "; Final Submitted.";
                                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                    LogWriter.DataLogWriter(dLog);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        
                        #endregion

                        #region Log Insert IPN_07
                        try
                        {
                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = 0;
                            dLog.Attribute2 = Request.Form["tran_id"].ToString();
                            dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                            dLog.EventName = "sslcommerz";
                            dLog.PageName = "IPNListener.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "IPN_07: Payment Updated Successfully; PaymentId: " + cPayment.PaymentId + "; CandidateId: " + cPayment.CandidateID + "; IsPaid: Yes";
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {

                        #region N/A
                        ///////////////////////////////////////////////////////////////////////////////////
                        //DAL.TemporaryTable_2 tmp17 = new DAL.TemporaryTable_2();                       //
                        //tmp17.Note = "tmp17 Error updating CandidatePayment____" + ex.Message.ToString();
                        //tmp17.DateTimeNow = DateTime.Now;                                              //
                        //using (var db = new GeneralDataManager())                                      //
                        //{                                                                              //
                        //    db.Insert<DAL.TemporaryTable_2>(tmp17);                                    //
                        //}                                                                              //
                        /////////////////////////////////////////////////////////////////////////////////// 
                        #endregion


                        #region Log Insert IPN_08
                        try
                        {
                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = 0;
                            dLog.Attribute2 = Request.Form["tran_id"].ToString();
                            dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                            dLog.EventName = "sslcommerz";
                            dLog.PageName = "IPNListener.aspx";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "IPN_08: Payment Updated Exception: " + ex.Message.ToString();
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

                    DAL.TransactionHistory thExist = null;
                    using (var db = new OfficeDataManager())
                    {
                        thExist = db.AdmissionDB.TransactionHistories
                            .Where(c => c.TransactionID == strTransId)
                            .FirstOrDefault();
                    }

                    if (thExist == null) //insert transaction history if it does not exist.
                    {
                        #region N/A
                        /////////////////////////////////////////////////////////////////////////////////
                        //DAL.TemporaryTable_2 tmp18 = new DAL.TemporaryTable_2();                     //
                        //tmp18.Note = "IPN_tmp18 thExist____ is null " + intTransId.ToString();       //
                        //tmp18.DateTimeNow = DateTime.Now;                                            //
                        //using (var db = new GeneralDataManager())                                    //
                        //{                                                                            //
                        //    db.Insert<DAL.TemporaryTable_2>(tmp18);                                  //
                        //}                                                                            //
                        ///////////////////////////////////////////////////////////////////////////////// 
                        #endregion

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

                            #region Log Insert IPN_09
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = 0;
                                dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                dLog.EventName = "sslcommerz";
                                dLog.PageName = "IPNListener.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "IPN_09: Transaction History Inserted";
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex1)
                            {
                            }
                            #endregion

                        }
                        catch (Exception ex)
                        {

                            #region N/A
                            ///////////////////////////////////////////////////////////////////////////////////
                            //DAL.TemporaryTable_2 tmp19 = new DAL.TemporaryTable_2();                       //
                            //tmp19.Note = "IPN_tmp12 Error inserting TransactionHistory____" + ex.Message.ToString();
                            //tmp19.DateTimeNow = DateTime.Now;                                              //
                            //using (var db = new GeneralDataManager())                                      //
                            //{                                                                              //
                            //    db.Insert<DAL.TemporaryTable_2>(tmp19);                                    //
                            //}                                                                              //
                            /////////////////////////////////////////////////////////////////////////////////// 
                            #endregion

                            #region Log Insert IPN_10
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = 0;
                                dLog.Attribute2 = Request.Form["tran_id"].ToString();
                                dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                                dLog.EventName = "sslcommerz";
                                dLog.PageName = "IPNListener.aspx";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "IPN_10: Transaction History Insert Exception: " + ex.Message.ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex1)
                            {
                            }
                            #endregion

                        }

                    }

                    #endregion

                    GetSendingInfo(cPayment.CandidateID);
                }
                else
                {
                    // Payment is already updated

                    #region Log Insert IPN_11
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = 0;
                        dLog.Attribute2 = Request.Form["tran_id"].ToString();
                        dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                        dLog.EventName = "sslcommerz";
                        dLog.PageName = "IPNListener.aspx";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "IPN_11: Candidate Payment is already Updated.";
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



                //lblName.Text = outputArray["value_b"].ToString();
                //lblPaymentId.Text = outputArray["tran_id"].ToString();

            }
            catch (Exception ex)
            {
                #region N/A
                ///////////////////////////////////////////////////////////////////////////////////
                //DAL.TemporaryTable_2 tmp20 = new DAL.TemporaryTable_2();                       //
                //tmp20.Note = "IPN_tmp20 Exception savePayment()____" + ex.Message.ToString();      //
                //tmp20.DateTimeNow = DateTime.Now;                                              //
                //using (var db = new GeneralDataManager())                                      //
                //{                                                                              //
                //    db.Insert<DAL.TemporaryTable_2>(tmp20);                                    //
                //}                                                                              //
                /////////////////////////////////////////////////////////////////////////////////// 
                #endregion

                #region Log Insert IPN_12
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.DateTime = DateTime.Now;
                    dLog.DateCreated = DateTime.Now;
                    dLog.UserId = 0;
                    dLog.Attribute2 = Request.Form["tran_id"].ToString();
                    dLog.CandidateId = Convert.ToInt64(Request.Form["value_a"].ToString());
                    dLog.EventName = "sslcommerz";
                    dLog.PageName = "IPNListener.aspx";
                    //dLog.OldData = logOldObject;
                    dLog.NewData = "IPN_12: Exception: " + ex.Message.ToString();
                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ex1)
                {
                }
                #endregion
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
    }
}