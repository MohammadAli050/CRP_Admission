using CommonUtility;
using DAL.ViewModels.EkPayModel;
using DAL.ViewModels.EkPayModel.IPNModels;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace Admission.Controllers
{
    [RoutePrefix("api/EkPayPaymentGateway")]
    public class EkPayPaymentGatewayController : ApiController
    {
        string url = System.Web.HttpContext.Current.Request.RawUrl;
        string clientIpAddress = HttpContext.Current.Request.UserHostAddress;
        string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

        public class EkPayGetModel
        {
            public string trnx_id { get; set; }
            public string trans_date { get; set; }
        }

        [HttpPost]
        [Route("EkPayPost")]
        public IHttpActionResult EkPayPost(EkPayIPNModel model)
        {


            long candidateId = Convert.ToInt64(model.cust_info.cust_mail_addr.ToString().Trim());
            string ekPayTransactionId = model.trnx_info.trnx_id.ToString().Trim();
            string transactionId = model.trnx_info.mer_trnx_id.ToString().Trim();
            long paymentId = Convert.ToInt64(model.cust_info.cust_id.ToString().Trim());

            try
            {

                if (paymentId > 0 && candidateId > 0 && !string.IsNullOrEmpty(transactionId) && !string.IsNullOrEmpty(ekPayTransactionId))
                {
                    #region Log Insert EkPay_01
                    try
                    {
                        string msg = "ClientIpAddress: " + clientIpAddress.ToString().Trim() + "; " +
                                    "PaymentId: " + paymentId.ToString().Trim() + "; " +
                                    "CandidateId: " + candidateId.ToString().Trim() + "; " +
                                    "EkPayTransactionId: " + ekPayTransactionId.ToString().Trim();

                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = 0;
                        dLog.Attribute1 = paymentId.ToString();
                        dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                        dLog.CandidateId = candidateId;
                        dLog.EventName = "ekpay";
                        dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "EkPay_01: EkPay Post Called; Parameter: " + msg.ToString();
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

                    if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
                    {
                        //// Live
                        List<DAL.ClientIPAddress> clientIpList = null;
                        using (var db = new GeneralDataManager())
                        {
                            clientIpList = db.AdmissionDB.ClientIPAddresses.Where(x => x.IPAddress.Trim() == clientIpAddress.Trim()).ToList();
                        }

                        if (clientIpList != null && clientIpList.Count > 0)
                        {

                            DAL.OnlineCollectionAttempt oca = null;
                            using (var db = new CandidateDataManager())
                            {
                                oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.TransactionId == transactionId
                                                                                        && x.CandidateId == candidateId).FirstOrDefault();
                            }


                            #region Get Store Info
                            string ekPayStoreId = "";
                            string ekPayStorePass = "";

                            string ekPayBaseURL = "";
                            string ekPaySubURL = "";
                            string ekPayURLVersion = "";

                            string ekPaySuccessURL = "";
                            string ekPayFailedURL = "";
                            string ekPayCanceledURL = "";
                            string ekPayIPNListenerURL = "";

                            DAL.StoreEkPay storeEkPay = null;
                            using (var db = new OfficeDataManager())
                            {
                                storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                            }

                            if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
                            {
                                ekPayStoreId = storeEkPay.StoreId.ToString(); //WebConfigurationManager.AppSettings["ekPayStoreId"].Trim();
                                ekPayStorePass = storeEkPay.StorePass.ToString();//WebConfigurationManager.AppSettings["ekPayStorePass"].Trim();

                                ekPayBaseURL = storeEkPay.BaseURL.ToString(); //WebConfigurationManager.AppSettings["ekPayBaseURL"].Trim();
                                ekPaySubURL = storeEkPay.SubURL.ToString(); //WebConfigurationManager.AppSettings["ekPaySubURL"].Trim();
                                ekPayURLVersion = storeEkPay.URLVersion.ToString();

                                ekPaySuccessURL = storeEkPay.SuccessUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySuccessURL"].Trim();
                                ekPayFailedURL = storeEkPay.FailedUrl.ToString(); //WebConfigurationManager.AppSettings["ekPayFailedURL"].Trim();
                                ekPayCanceledURL = storeEkPay.CancelledUrl.ToString(); //WebConfigurationManager.AppSettings["ekPayCanceledURL"].Trim();
                                ekPayIPNListenerURL = storeEkPay.IPNUrl.ToString(); //WebConfigurationManager.AppSettings["ekPayIPNListenerURL"].Trim();
                            }
                            else
                            {
                                ekPayStoreId = storeEkPay.SendBoxStoreId.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxStoreId"].Trim();
                                ekPayStorePass = storeEkPay.SendBoxStorePass.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxStorePass"].Trim();

                                ekPayBaseURL = storeEkPay.SendBoxBaseURL.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxBaseURL"].Trim();
                                ekPaySubURL = storeEkPay.SendBoxSubURL.ToString(); //WebConfigurationManager.AppSettings["ekPaySubURL"].Trim();
                                ekPayURLVersion = storeEkPay.SendBoxURLVersion.ToString(); //storeEkPay.URLVersion.ToString();

                                ekPaySuccessURL = storeEkPay.SendBoxSuccessUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxSuccessURL"].Trim();
                                ekPayFailedURL = storeEkPay.SendBoxFailedUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxFailedURL"].Trim();
                                ekPayCanceledURL = storeEkPay.SendBoxCancelledUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxCanceledURL"].Trim();
                                ekPayIPNListenerURL = storeEkPay.SendBoxIPNUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxIPNListenerURL"].Trim();
                            }
                            #endregion

                            EkPayGetModel getModel = new EkPayGetModel();
                            getModel.trnx_id = transactionId;
                            getModel.trans_date = Convert.ToDateTime(oca.CreatedDate).ToString("yyyy-MM-dd"); //DateTime.Now.ToString("yyyy-MM-dd"); // token newer time a jei datetime disi oita dite hobe

                            EkPayPaymentGateway ekPayPG = new EkPayPaymentGateway(ekPayBaseURL, ekPaySubURL, ekPayURLVersion, "/get-status");
                            ResponseEkPay responseEkPay = ekPayPG.SearchByTransactionId(getModel);

                            #region Log Insert EkPay_02
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = 0;
                                dLog.Attribute1 = paymentId.ToString();
                                dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                dLog.CandidateId = candidateId;
                                dLog.EventName = "ekpay";
                                dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPay_02: Payment Response From EkPay; Data: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex1)
                            {
                            }
                            #endregion

                            if (responseEkPay.ResponseCode == 200)
                            {
                                //SuccessResponse sr = (SuccessResponse)responseEkPay.ResponseData;
                                var data = (JObject)JsonConvert.DeserializeObject(responseEkPay.ResponseData.ToString());

                                string msgcode = data["msg_code"].ToString();
                                string msgdet = data["msg_det"].ToString();
                                //string custname = data["cust_info"].Value<string>("cust_name");

                                if (msgcode == "1020")
                                {

                                    #region Update Payment

                                    #region Log Insert EkPay_03
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = 0;
                                        dLog.Attribute1 = paymentId.ToString();
                                        dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                        dLog.CandidateId = candidateId;
                                        dLog.EventName = "ekpay";
                                        dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "EkPay_03: Payment Is Updated In EkPay;" ;
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex1)
                                    {
                                    }
                                    #endregion


                                    DAL.CandidatePayment cp = null;
                                    //DAL.OnlineCollectionAttempt oca = null;
                                    using (var db = new CandidateDataManager())
                                    {
                                        cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == paymentId).FirstOrDefault();

                                        //oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.TransactionId == transactionId
                                        //                                                       && x.CandidateId == candidateId).FirstOrDefault();
                                    }

                                    if (cp != null && cp.IsPaid == false)
                                    {
                                        cp.IsPaid = true;
                                        cp.ModifiedBy = cp.CandidateID;
                                        cp.DateModified = DateTime.Now;

                                        try
                                        {
                                            using (var db = new CandidateDataManager())
                                            {
                                                db.Update<DAL.CandidatePayment>(cp);
                                            }

                                            #region Log Insert EkPay_04
                                            try
                                            {

                                                #region MSG
                                                string msg = "secure_token: " + model.secure_token + "; " +
                                                                                          "msg_code: " + model.msg_code + "; " +
                                                                                          "msg_det: " + model.msg_det + "; " +
                                                                                          "req_timestamp: " + model.req_timestamp + "; " +

                                                                                          "mer_reg_id: " + model.basic_Info.mer_reg_id + "; " +
                                                                                          "ipn_info: " + model.basic_Info.ipn_info + "; " +
                                                                                        "redirect_to: " + model.basic_Info.redirect_to + "; " +
                                                                                        "dgtl_sign: " + model.basic_Info.dgtl_sign + "; " +
                                                                                        "ord_desc: " + model.basic_Info.mer_reg_id + "; " +
                                                                                        "remarks: " + model.basic_Info.remarks + "; " +

                                                                                      "cust_id: " + model.cust_info.cust_id + "; " +
                                                                                        "cust_name: " + model.cust_info.cust_name + "; " +
                                                                                        "cust_mobo_no: " + model.cust_info.cust_mobo_no + "; " +
                                                                                        "cust_email: " + model.cust_info.cust_email + "; " +
                                                                                        "cust_mail_addr: " + model.cust_info.cust_mail_addr + "; " +

                                                                                      "trnx_amt: " + model.trnx_info.trnx_amt + "; " +
                                                                                        "trnx_id: " + model.trnx_info.trnx_id + "; " +
                                                                                        "mer_trnx_id: " + model.trnx_info.mer_trnx_id + "; " +
                                                                                        "curr: " + model.trnx_info.curr + "; " +
                                                                                        "pi_trnx_id: " + model.trnx_info.pi_trnx_id + "; " +
                                                                                        "pi_charge: " + model.trnx_info.pi_charge + "; " +
                                                                                        "ekpay_charge: " + model.trnx_info.ekpay_charge + "; " +
                                                                                        "pi_discount: " + model.trnx_info.pi_discount + "; " +
                                                                                        "total_ser_chrg: " + model.trnx_info.total_ser_chrg + "; " +
                                                                                        "discount: " + model.trnx_info.discount + "; " +
                                                                                        "promo_discount: " + model.trnx_info.promo_discount + "; " +
                                                                                        "total_pabl_amt: " + model.trnx_info.total_pabl_amt + "; " +

                                                                                      "pay_timestamp: " + model.pi_det_info.pay_timestamp + "; " +
                                                                                        "pi_name: " + model.pi_det_info.pi_name + "; " +
                                                                                        "pi_type: " + model.pi_det_info.pi_type + "; " +
                                                                                        "card_holder_name: " + model.pi_det_info.card_holder_name + "; " +
                                                                                        "pi_number: " + model.pi_det_info.pi_number + "; " +
                                                                                        "pi_gateway: " + model.pi_det_info.pi_gateway;
                                                #endregion

                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = 0;
                                                dLog.Attribute1 = paymentId.ToString();
                                                dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                                dLog.CandidateId = candidateId;
                                                dLog.EventName = "ekpay";
                                                dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "EkPay_04: Payment Updated; MSG: " + msg.ToString();
                                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            #endregion

                                            #region Online Collection Attempt Update
                                            try
                                            {
                                                if (oca != null)
                                                {
                                                    oca.IsPaid = true;
                                                    oca.ModifiedBy = candidateId;
                                                    oca.ModifiedDate = DateTime.Now;

                                                    using (var db = new CandidateDataManager())
                                                    {
                                                        db.Update<DAL.OnlineCollectionAttempt>(oca);
                                                    }

                                                    #region Log Insert EkPay_05
                                                    try
                                                    {
                                                        
                                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        dLog.DateTime = DateTime.Now;
                                                        dLog.DateCreated = DateTime.Now;
                                                        dLog.UserId = 0;
                                                        dLog.Attribute1 = paymentId.ToString();
                                                        dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                                        dLog.CandidateId = candidateId;
                                                        dLog.EventName = "ekpay";
                                                        dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                                        //dLog.OldData = logOldObject;
                                                        dLog.NewData = "EkPay_05: Online Collection Attempt Payment Updated; Data: " + JsonConvert.SerializeObject(oca).ToString();
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

                                            }
                                            #endregion

                                            #region INSERT TRANSACTION HISTORY

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
                                                transactionHistory.TransactionDate = model.pi_det_info.pay_timestamp;
                                                transactionHistory.TransactionID = cp.PaymentId.ToString();
                                                transactionHistory.ValidationID = oca.TransactionId.ToString();
                                                transactionHistory.Amount = cp.Amount.ToString();
                                                transactionHistory.StoreAmount = cp.Amount.ToString();
                                                transactionHistory.Currency = model.trnx_info.curr.ToString();
                                                transactionHistory.BankTransactionID = model.pi_det_info.pi_number.ToString();
                                                transactionHistory.CardType = model.pi_det_info.pi_type.ToString();
                                                transactionHistory.CardNumber = model.pi_det_info.pi_number.ToString();
                                                transactionHistory.CardIssuer = model.pi_det_info.pi_name.ToString();
                                                transactionHistory.CardBrand = model.pi_det_info.pi_gateway.ToString();
                                                transactionHistory.CardIssuerCountry = "BANGLADESH";
                                                transactionHistory.CardIssuerCountryCode = "BD";
                                                transactionHistory.CurrencyType = model.trnx_info.curr.ToString();
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
                                                transactionHistory.ValidatedOn = model.pi_det_info.pay_timestamp;
                                                transactionHistory.GwVersion = null;
                                                transactionHistory.CreatedOn = DateTime.Now;
                                                transactionHistory.IsManualInsert = false;
                                                transactionHistory.IsInHouseCashTransaction = false;

                                                try
                                                {
                                                    using (var db = new OfficeDataManager())
                                                    {
                                                        db.Insert<DAL.TransactionHistory>(transactionHistory);
                                                    }

                                                    #region Log Insert EkPay_06
                                                    try
                                                    {
                                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        dLog.DateTime = DateTime.Now;
                                                        dLog.DateCreated = DateTime.Now;
                                                        dLog.UserId = 0;
                                                        dLog.Attribute1 = cp.PaymentId.ToString();
                                                        dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                                        dLog.CandidateId = candidateId;
                                                        dLog.EventName = "ekpay";
                                                        dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                                        //dLog.OldData = logOldObject;
                                                        dLog.NewData = "EkPay_06: Payment Transaction History Saved";
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
                                                    #region Log Insert EkPay_07
                                                    try
                                                    {
                                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                        dLog.DateTime = DateTime.Now;
                                                        dLog.DateCreated = DateTime.Now;
                                                        dLog.UserId = 0;
                                                        dLog.Attribute1 = cp.PaymentId.ToString();
                                                        dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                                        dLog.CandidateId = candidateId;
                                                        dLog.EventName = "ekpay";
                                                        dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                                        //dLog.OldData = logOldObject;
                                                        dLog.NewData = "EkPay_07: Payment Transaction History Saved Failed; Exception: " + ex.Message.ToString();
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

                                            GetSendingInfo(cp.CandidateID);

                                            return Content(HttpStatusCode.OK, "Payment Updated Successfully.");

                                        }
                                        catch (Exception ex)
                                        {

                                            #region Log Insert EkPay_08
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.DateTime = DateTime.Now;
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.UserId = 0;
                                                dLog.Attribute1 = paymentId.ToString();
                                                dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                                dLog.CandidateId = candidateId;
                                                dLog.EventName = "ekpay";
                                                dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                                //dLog.OldData = logOldObject;
                                                dLog.NewData = "EkPay_08: Payment Updated Exception: " + ex.Message.ToString();
                                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ex1)
                                            {
                                            }
                                            #endregion

                                            return Content(HttpStatusCode.Forbidden, "Payment update failed.");

                                        }
                                    }
                                    else
                                    {
                                        // Message
                                        #region Log Insert EkPay_09
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.DateTime = DateTime.Now;
                                            dLog.DateCreated = DateTime.Now;
                                            dLog.UserId = 0;
                                            dLog.Attribute1 = paymentId.ToString();
                                            dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                            dLog.CandidateId = candidateId;
                                            dLog.EventName = "ekpay";
                                            dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                            //dLog.OldData = logOldObject;
                                            dLog.NewData = "EkPay_09: Payment is already Updated in EduSoft";
                                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex1)
                                        {
                                        }
                                        #endregion

                                        return Content(HttpStatusCode.OK, "Payment is already updated.");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Log Insert EkPay_10
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = 0;
                                        dLog.Attribute1 = paymentId.ToString();
                                        dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                        dLog.CandidateId = candidateId;
                                        dLog.EventName = "ekpay";
                                        dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "EkPay_10: Payment is not Updated in EkPay; Data: " + JsonConvert.SerializeObject(data).ToString();
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex1)
                                    {
                                    }
                                    #endregion

                                    return Content(HttpStatusCode.Forbidden, "Payment is not updated in EkPay.");
                                }


                            }
                            else
                            {
                                #region Log Insert EkPay_11
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = 0;
                                    dLog.Attribute1 = paymentId.ToString();
                                    dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                    dLog.CandidateId = candidateId;
                                    dLog.EventName = "ekpay";
                                    dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "EkPay_11: Payment success not found from EkPay; MSG: " + JsonConvert.SerializeObject(responseEkPay).ToString();
                                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex1)
                                {
                                }
                                #endregion

                                return Content(HttpStatusCode.ExpectationFailed, "Payment is not updated in EkPay.");
                            }


                        }
                        else
                        {
                            #region Log Insert EkPay_12
                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = 0;
                                dLog.Attribute1 = paymentId.ToString();
                                dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                                dLog.CandidateId = candidateId;
                                dLog.EventName = "ekpay";
                                dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                                //dLog.OldData = logOldObject;
                                dLog.NewData = "EkPay_12: Ip address not in whitelist";
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex1)
                            {
                            }
                            #endregion

                            return Content(HttpStatusCode.Unauthorized, "Ip address not in whitelist");
                        }
                    }
                    else
                    {
                        #region Log Insert EkPay_13
                        try
                        {
                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.DateTime = DateTime.Now;
                            dLog.DateCreated = DateTime.Now;
                            dLog.UserId = 0;
                            dLog.Attribute1 = paymentId.ToString();
                            dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                            dLog.CandidateId = candidateId;
                            dLog.EventName = "ekpay";
                            dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                            //dLog.OldData = logOldObject;
                            dLog.NewData = "EkPay_13: Unauthorized; Request is not from Live.";
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex1)
                        {
                        }
                        #endregion

                        return Content(HttpStatusCode.Unauthorized, "Unauthorized");
                    }

                }
                else
                {
                    #region Log Insert EkPay_14
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.DateTime = DateTime.Now;
                        dLog.DateCreated = DateTime.Now;
                        dLog.UserId = 0;
                        dLog.Attribute1 = paymentId.ToString();
                        dLog.Attribute2 = transactionId;//Request.Form["tran_id"].ToString();
                        dLog.CandidateId = candidateId;
                        dLog.EventName = "ekpay";
                        dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                        //dLog.OldData = logOldObject;
                        dLog.NewData = "EkPay_14: Invalid Parameters";
                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex1)
                    {
                    }
                    #endregion

                    return Content(HttpStatusCode.ExpectationFailed, "Invalid Parameters!");
                }
            }
            catch (Exception ex)
            {
                #region Log Insert EkPay_15
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.DateTime = DateTime.Now;
                    dLog.DateCreated = DateTime.Now;
                    dLog.UserId = 0;
                    dLog.Attribute1 = paymentId.ToString();
                    dLog.Attribute2 = transactionId;
                    dLog.CandidateId = candidateId;
                    dLog.EventName = "ekpay";
                    dLog.PageName = "EkPayPaymentGatewayControllerAPI";
                    //dLog.OldData = logOldObject;
                    dLog.NewData = "EkPay_15: Exception: " + ex.Message.ToString();
                    //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                    //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ex1)
                {
                }
                #endregion

                return Content(HttpStatusCode.InternalServerError, "Exception: Something went wrong!");
                //return Ok("Exception: " + ex.Message.ToString()); //Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Exception: " + ex.Message.ToString());
            }

        }


        [HttpPost]
        [Route("TestAPIJsonObject")]
        public IHttpActionResult TestAPIJsonObject(EkPayGetModel model)
        {
            try
            {
                //string jsonObject = "{\"secure_token\":\"EEwJ6tF9x5WCIZDYzyZGaz6Khbw7raYRIBVTTTWxVvgmsG\",\"msg_code\":\"1020\",\"msg_det\":\"Transactioncompletedsuccessfully\",\"req_timestamp\":\"2021-02-2415:22:00GMT+06:00\",\"basic_Info\":{\"mer_reg_id\":\"ekPayWeb\",\"ipn_info\":\"ipn_chanel=3;ipn_uri=www.ekpay/ipnlistener/v1/SendIPN\",\"redirect_to\":\"www.ekpay.gov.bd/v1/billPaidSuccesss?trnx_id=105d\",\"dgtl_sign\":\"UsingGoogleChrome;Windows1064bit;ip172.16.16.114;dhakaBD\",\"ord_desc\":\"WasaBillPaymentforthemonthofJan-18\",\"remarks\":\"\"},\"cust_info\":{\"cust_name\":\"Mr.Payee\",\"cust_email\":\"payee@gmail.com\",\"cust_mobile\":\"01738564978\",\"cust_address\":\"Farmgate,Dhaka\"},\"trnx_info\":{\"trnx_amt\":\"150\",\"trnx_id\":\"56987\",\"mer_trnx_id\":\"1234\",\"curr\":\"BDT\",\"pi_trnx_id\":\"B3h4e+O6hEnMJQH1Ev5NsSMUjeI=\",\"pi_charge\":\"3\",\"ekpay_charge\":\"5\",\"pi_discount\":\"0\",\"total_ser_chrg\":\"8\",\"discount\":\"0\",\"promo_discount\":\"8\",\"total_pabl_amt\":\"158\"},\"pi_det_info\":{\"pay_timestamp\":\"2021-02-2415:22:00GMT+06:00\",\"pi_name\":\"DutchBanglaLimited\",\"pi_type\":\"Card-Nexus\",\"card_holder_name\":\"Mr.X\",\"pi_number\":\"484096******3114\",\"pi_gateway\":\"DBBL-NexusNetwork\"}}";

                //var data = (JObject)JsonConvert.DeserializeObject(jsonObject);

                //string msgcode = data["msg_code"].ToString();
                //string msgdet = data["msg_det"].ToString();
                //string custname = data["cust_info"].Value<string>("cust_name");


                DAL.CandidatePayment cp = null;
                DAL.OnlineCollectionAttempt oca = null;

                string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

                #region Get Store Info
                string ekPayStoreId = "";
                string ekPayStorePass = "";

                string ekPayBaseURL = "";
                string ekPaySubURL = "";
                string ekPayURLVersion = "";

                string ekPaySuccessURL = "";
                string ekPayFailedURL = "";
                string ekPayCanceledURL = "";
                string ekPayIPNListenerURL = "";

                DAL.StoreEkPay storeEkPay = null;
                using (var db = new OfficeDataManager())
                {
                    storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
                {
                    ekPayStoreId = storeEkPay.StoreId.ToString(); //WebConfigurationManager.AppSettings["ekPayStoreId"].Trim();
                    ekPayStorePass = storeEkPay.StorePass.ToString();//WebConfigurationManager.AppSettings["ekPayStorePass"].Trim();

                    ekPayBaseURL = storeEkPay.BaseURL.ToString(); //WebConfigurationManager.AppSettings["ekPayBaseURL"].Trim();
                    ekPaySubURL = storeEkPay.SubURL.ToString(); //WebConfigurationManager.AppSettings["ekPaySubURL"].Trim();
                    ekPayURLVersion = storeEkPay.URLVersion.ToString();

                    ekPaySuccessURL = storeEkPay.SuccessUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySuccessURL"].Trim();
                    ekPayFailedURL = storeEkPay.FailedUrl.ToString(); //WebConfigurationManager.AppSettings["ekPayFailedURL"].Trim();
                    ekPayCanceledURL = storeEkPay.CancelledUrl.ToString(); //WebConfigurationManager.AppSettings["ekPayCanceledURL"].Trim();
                    ekPayIPNListenerURL = storeEkPay.IPNUrl.ToString(); //WebConfigurationManager.AppSettings["ekPayIPNListenerURL"].Trim();
                }
                else
                {
                    ekPayStoreId = storeEkPay.SendBoxStoreId.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxStoreId"].Trim();
                    ekPayStorePass = storeEkPay.SendBoxStorePass.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxStorePass"].Trim();

                    ekPayBaseURL = storeEkPay.SendBoxBaseURL.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxBaseURL"].Trim();
                    ekPaySubURL = storeEkPay.SendBoxSubURL.ToString(); //WebConfigurationManager.AppSettings["ekPaySubURL"].Trim();
                    ekPayURLVersion = storeEkPay.SendBoxURLVersion.ToString(); //storeEkPay.URLVersion.ToString();

                    ekPaySuccessURL = storeEkPay.SendBoxSuccessUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxSuccessURL"].Trim();
                    ekPayFailedURL = storeEkPay.SendBoxFailedUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxFailedURL"].Trim();
                    ekPayCanceledURL = storeEkPay.SendBoxCancelledUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxCanceledURL"].Trim();
                    ekPayIPNListenerURL = storeEkPay.SendBoxIPNUrl.ToString(); //WebConfigurationManager.AppSettings["ekPaySendBoxIPNListenerURL"].Trim();
                }
                #endregion


                EkPayGetModel getModel = new EkPayGetModel();
                getModel.trnx_id = model.trnx_id; //"2202171438EKPAYDT13022022154736";
                getModel.trans_date = model.trans_date; //"2022-02-13"; //DateTime.Now.ToString("yyyy-MM-dd"); // token newer time a jei datetime disi oita dite hobe

                EkPayPaymentGateway ekPayPG = new EkPayPaymentGateway(ekPayBaseURL, ekPaySubURL, ekPayURLVersion, "/get-status");
                ResponseEkPay responseEkPay = ekPayPG.SearchByTransactionId(getModel);



                if (responseEkPay.ResponseCode == 200)
                {
                    //SuccessResponse sr = (SuccessResponse)responseEkPay.ResponseData;
                    var data = (JObject)JsonConvert.DeserializeObject(responseEkPay.ResponseData.ToString());

                    string msgcode = data["msg_code"].ToString();
                    string msgdet = data["msg_det"].ToString();

                    if (msgcode == "1020")
                    {

                        #region Update Payment

                        using (var db = new CandidateDataManager())
                        {
                            oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.TransactionId == model.trnx_id).FirstOrDefault();
                            if (oca != null)
                            {
                                cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == oca.PaymentId).FirstOrDefault();
                            }
                        }

                        if (cp != null && cp.IsPaid == false)
                        {
                            cp.IsPaid = true;
                            cp.ModifiedBy = cp.CandidateID;
                            cp.DateModified = DateTime.Now;

                            try
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    db.Update<DAL.CandidatePayment>(cp);
                                }


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
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                #endregion


                                return Content(HttpStatusCode.OK, data);

                            }
                            catch (Exception ex)
                            {

                                return Content(HttpStatusCode.Forbidden, "Payment update failed.");

                            }
                        }
                        else
                        {
                            return Content(HttpStatusCode.OK, "Payment is already updated.");
                        }
                        #endregion
                    }
                    else
                    {
                        return Content(HttpStatusCode.Forbidden, "Payment is not updated in EkPay.");
                    }
                }
                else
                {
                    return Content(HttpStatusCode.OK, "Payment Not Updated.");
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.OK, "Payment Updated Successfully.");
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
