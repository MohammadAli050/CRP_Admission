using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace Admission.Controllers
{
    public class PaymentCheckApiController : ApiController
    {

        protected string FPG_URL = "https://payment.fosterpayments.com.bd/";
        protected string Submit_URL = "fosterpayments/TransactionStatus/transactionStatusApi1.2.php";

        //public class ResponseAPI
        //{
        //    public int ResponseCode { get; set; }
        //    public string ResponseStatus { get; set; }
        //    public string ResponseMessage { get; set; }
        //    public object ResponseData { get; set; }
        //}


        //API_URL: http://localhost:17124/api/PaymentCheckApi/GetById?id=2102200483

        // GET api/Books/5
        //[ResponseType(typeof(CandidatePaymentModel))]

        [HttpGet]
        [Route("api/PaymentCheckApi/GetPaymentInfo")]
        public IHttpActionResult GetPaymentInfo(long? id)
        {

            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                if (id == 0 || id == null)
                {
                    responseAPI.ResponseCode = 400;
                    responseAPI.ResponseStatus = "Failed";
                    responseAPI.ResponseMessage = "Invalid Request !";
                    responseAPI.ResponseData = "";

                    return Ok(responseAPI);
                }

                DAL.CandidatePayment cpModel = null;
                using (var db = new CandidateDataManager())
                {
                    cpModel = db.GetCandidatePaymentByPaymentID_AD(Convert.ToInt64(id));   //Checking student is exits or not by PaymentID
                }


                if (cpModel == null)
                {
                    responseAPI.ResponseCode = 400;
                    responseAPI.ResponseStatus = "Failed";
                    responseAPI.ResponseMessage = "No Data Found !";
                    responseAPI.ResponseData = "";

                    return Ok(responseAPI);
                }


                List<DAL.SPAcademicCalendarGetAll_Result> acaCalList = null;
                DAL.SPAcademicCalendarGetAll_Result acaCalFList = null;
                using (var db = new OfficeDataManager())
                {
                    acaCalList = db.AdmissionDB.SPAcademicCalendarGetAll().ToList();

                    if (acaCalList.Count > 0)
                    {
                        acaCalFList = acaCalList.Where(x => x.AcademicCalenderID == cpModel.AcaCalID).FirstOrDefault();  //Getting Session for applied student
                    }
                    else
                    {
                        responseAPI.ResponseCode = 400;
                        responseAPI.ResponseStatus = "Failed";
                        responseAPI.ResponseMessage = "No Data Found !";
                        responseAPI.ResponseData = "";

                        return Ok(responseAPI);
                    }

                }

                CandidatePaymentModel list2 = new CandidatePaymentModel();

                list2.PaymentId = cpModel.PaymentId;
                list2.StudentName = cpModel.BasicInfo.FirstName;
                list2.Session = acaCalFList.FullCode;
                list2.FormAppliedDate = cpModel.DateCreated;
                list2.IsPaid = Convert.ToBoolean(cpModel.IsPaid);
                list2.PaymentUpdateStatus = cpModel.IsPaid == true ? "Payment Update Successful." : "Payment not Updated..!!";

                responseAPI.ResponseCode = 200;
                responseAPI.ResponseStatus = "Success";
                responseAPI.ResponseMessage = "";
                responseAPI.ResponseData = list2;

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.ResponseCode = 400;
                responseAPI.ResponseStatus = "Failed";
                responseAPI.ResponseMessage = "Exception; Error: " + ex.Message.ToString();
                responseAPI.ResponseData = "";

                return Ok(responseAPI);
            }

        }


        [HttpGet]
        [Route("api/PaymentCheckApi/PaymentUpdate")]
        public IHttpActionResult PaymentUpdate(long? id)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                if (id == 0 || id == null)
                {
                    responseAPI.ResponseCode = 400;
                    responseAPI.ResponseStatus = "Failed";
                    responseAPI.ResponseMessage = "Invalid Request !";
                    responseAPI.ResponseData = "";

                    return Ok(responseAPI);
                }



                DAL.CandidatePayment cpModel = null;
                using (var db = new CandidateDataManager())
                {
                    cpModel = db.GetCandidatePaymentByPaymentID_AD(Convert.ToInt64(id));   //Checking student is exits or not by PaymentID
                }


                if (cpModel == null)
                {
                    responseAPI.ResponseCode = 400;
                    responseAPI.ResponseStatus = "Failed";
                    responseAPI.ResponseMessage = "No Data Found !";
                    responseAPI.ResponseData = "";

                    return Ok(responseAPI);
                }


                DAL.BasicInfo candidateObj = null;
                using (var db = new CandidateDataManager())
                {
                    candidateObj = db.GetCandidateBasicInfoByID_ND(Convert.ToInt64(cpModel.CandidateID));
                }


                if (Convert.ToBoolean(cpModel.IsPaid) == true)
                {
                    responseAPI.ResponseCode = 200;
                    responseAPI.ResponseStatus = "Success";
                    responseAPI.ResponseMessage = "Payment has already been Updated.";
                    responseAPI.ResponseData = "";

                    return Ok(responseAPI);
                }
                else
                {
                    #region Get Setup Information
                    DAL.StoreFoster storeFoster = null;
                    DAL.AdmissionSetup admissionSetup = null;
                    using (var db = new CandidateDataManager())
                    {
                        admissionSetup = db.AdmissionDB.AdmissionSetups.Find(cpModel.CandidateFormSls.FirstOrDefault().AdmissionSetupID);
                    }

                    if (admissionSetup != null)
                    {
                        using (var db = new OfficeDataManager())
                        {
                            storeFoster = db.GetFPGStoreByID(admissionSetup.StoreID);
                        }
                    }
                    #endregion


                    string json = string.Empty;
                    string sys_MerchantTxnNo = storeFoster.MerchantShortName + cpModel.PaymentId.ToString();
                    string sys_MerchantSecureHashValue = FosterPaymentGateway.Convertmd5(string.Concat(string.Concat(storeFoster.SecurityKey, sys_MerchantTxnNo))).ToLower();

                    string validate_url = this.FPG_URL + this.Submit_URL + "?mcnt_TxnNo=" + sys_MerchantTxnNo + "&mcnt_SecureHashValue=" + sys_MerchantSecureHashValue;

                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(validate_url);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream resStream = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(resStream))
                        {
                            json = reader.ReadToEnd();
                        }

                        if (json != "")
                        {
                            dynamic resp = JsonConvert.DeserializeObject<dynamic>(json);
                            
                            if (resp[0]["txnResponse"].Value.ToString() == "2")
                            {
                                CandidatePaymentUpdate(cpModel, candidateObj, resp, sys_MerchantTxnNo, sys_MerchantSecureHashValue);

                                responseAPI.ResponseCode = 200;
                                responseAPI.ResponseStatus = "Success";
                                responseAPI.ResponseMessage = "Payment Updated Successful.";
                                responseAPI.ResponseData = "";

                                return Ok(responseAPI);
                            }
                            else
                            {
                                responseAPI.ResponseCode = 400;
                                responseAPI.ResponseStatus = "Failed";
                                responseAPI.ResponseMessage = "Payment is not Updated in FPG !";
                                responseAPI.ResponseData = "";

                                return Ok(responseAPI);
                            }
                        }
                        else
                        {
                            responseAPI.ResponseCode = 400;
                            responseAPI.ResponseStatus = "Failed";
                            responseAPI.ResponseMessage = "Unable to Get Response from FPG !";
                            responseAPI.ResponseData = "";

                            return Ok(responseAPI);
                        }
                    }
                    catch (Exception ex)
                    {
                        responseAPI.ResponseCode = 400;
                        responseAPI.ResponseStatus = "Failed";
                        responseAPI.ResponseMessage = "Payment is not Updated in FPG !";
                        responseAPI.ResponseData = "";

                        return Ok(responseAPI);
                    }
                }
            }
            catch (Exception ex)
            {
                responseAPI.ResponseCode = 400;
                responseAPI.ResponseStatus = "Failed";
                responseAPI.ResponseMessage = "Exception; Error: " + ex.Message.ToString();
                responseAPI.ResponseData = "";

                return Ok(responseAPI);
            }
            
        }


        private void CandidatePaymentUpdate(DAL.CandidatePayment candidatePayment, DAL.BasicInfo candidateObj, dynamic resp, string sys_MerchantTxnNo, string sys_MerchantSecureHashValue)
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
                    using (var db = new CandidateDataManager())
                    {
                        db.Update<DAL.CandidatePayment>(candidatePayment);
                        candidatePaymentUpdateID = candidatePayment.ID;
                    }


                    using (var db = new CandidateDataManager())
                    {
                        candidateUserObj = db.AdmissionDB.CandidateUsers.Find(candidateObj.CandidateUserID);
                    }

                    if (candidatePaymentUpdateID > 0 && candidateUserObj != null)
                    {
                        #region Transaction History
                        DAL.TransactionHistoryFPG transactionHistory = new DAL.TransactionHistoryFPG();

                        transactionHistory.CandidateID = Convert.ToInt64(candidatePayment.CandidateID);
                        transactionHistory.CandidateName = candidateObj.FirstName;
                        transactionHistory.CandidatePaymentID = Convert.ToInt64(candidatePayment.PaymentId);
                        transactionHistory.CreatedBy = Convert.ToInt64(candidatePayment.CandidateID);
                        transactionHistory.DateCreated = DateTime.Now;
                        transactionHistory.DateModified = null;
                        transactionHistory.IsManualInsert = false;
                        transactionHistory.ManualInsertBy = null;
                        transactionHistory.ModifiedBy = null;
                        transactionHistory.Status = "Success";

                        transactionHistory.ConversionRate = resp[0]["convertionRate"].Value.ToString();
                        transactionHistory.Currency = resp[0]["currency"].Value.ToString();
                        transactionHistory.FosterId = resp[0]["fosterId"].Value.ToString();
                        transactionHistory.Hashkey = resp[0]["hashkey"].Value.ToString();
                        transactionHistory.MerchantTransactionNo = sys_MerchantTxnNo;
                        transactionHistory.MerchantTransactionResponse = resp[0]["txnResponse"].Value.ToString();
                        transactionHistory.OrderNo = resp[0]["orderNo"].Value.ToString();
                        transactionHistory.SystemGeneratedHashKey = sys_MerchantSecureHashValue;
                        transactionHistory.TransactionAmount = resp[0]["txnAmount"].Value.ToString();

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
                        #endregion


                        SendSms(candidateObj.SMSPhone, candidateUserObj.UsernameLoginId, candidateUserObj.Password, candidateObj.ID);
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
                    smsLog.Attribute1 = "Sms sending failed in PaymentCheckApi Controller";
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
                    smsLog.Attribute1 = "Sms sending successful PaymentCheckApi Controller";
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




        // GET api/<controller>

        //List<Product> products = new List<Product>()
        //{
        //    new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
        //    new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
        //    new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        //};


        //public List<Product> GetAll()
        //{
        //    return products;

        //}

        //public List<DAL.CandidatePayment> GetAll()
        //{
        //    //return products;


        //    List<DAL.CandidatePayment> list = null;
        //    using (var db = new CandidateDataManager())
        //    {
        //        list = db.AdmissionDB.CandidatePayments.ToList();

        //    }

        //    return list;

        //}






        //public IHttpActionResult GetById(int id)
        //{
        //    //return products;

        //    DAL.CandidatePayment cPayment = null;
        //    //DAL.CandidatePayment list = new DAL.CandidatePayment();
        //    using (var db = new CandidateDataManager())
        //    {

        //        cPayment = db.AdmissionDB.CandidatePayments
        //                    .Where(c => c.PaymentId == id)
        //                    .FirstOrDefault();

        //        //var queryLondonCustomers = from cust in DAL.CandidatePayment
        //        //                           where cust.City == "London"
        //        //                           select cust;



        //        //var result = (from cp in db.AdmissionDB.CandidatePayments
        //        //            where cp.PaymentId == id
        //        //            select new
        //        //            {
        //        //                ID = cp.ID,
        //        //                PaymentId = cp.PaymentId,
        //        //                AcaCalID = cp.AcaCalID,
        //        //                IsPaid = cp.IsPaid,
        //        //                Amount = cp.Amount
        //        //            }).ToList();

        //        //var result = (from cp in db.AdmissionDB.CandidatePayments
        //        //              where cp.PaymentId == id
        //        //              select new 
        //        //              {
        //        //                  ID = cp.ID,
        //        //                  PaymentId = cp.PaymentId,
        //        //                  AcaCalID = cp.AcaCalID,
        //        //                  IsPaid = cp.IsPaid,
        //        //                  Amount = cp.Amount
        //        //              }).ToList();


        //        //var data = (from c in db.AdmissionDB.CandidatePayments
        //        //           where c.PaymentId == id
        //        //           select c).ToList();

        //        if (cPayment != null)
        //        {
        //            return Ok(cPayment);
        //        }
        //        else
        //        {
        //            return null;
        //        }


        //    }



        //}







        //public DAL.CandidatePayment GetById(int id)
        //{
        //    //return products;


        //    DAL.CandidatePayment list = null;
        //    using (var db = new CandidateDataManager())
        //    {
        //        list = db.GetCandidatePaymentByID_AD(id);

        //    }

        //    return list;

        //}

        //public IHttpActionResult GetProduct(int id)
        //{
        //    var product = products.FirstOrDefault((p) => p.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}
    }
}