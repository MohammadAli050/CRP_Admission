using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class successurlRocket : System.Web.UI.Page
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _basePath = ConfigurationManager.AppSettings["dbblTransactionPath"];
        string logInID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    logInID = !string.IsNullOrEmpty(Request.Cookies["un"].Value) ? CommonUtility.Decrypt.DecryptString(Request.Cookies["un"].Value) : "";
            //}
            //catch (Exception ex)
            //{
            //    logInID = "";
            //}

            //#region log_insert
            //LogGeneralManager.Insert(
            //    DateTime.Now,
            //    "",
            //    "",
            //    logInID,
            //    "",
            //    "",
            //    "Online_Payment",
            //    "Success page arrived.",
            //    logInID + " is Load Page",
            //     "",
            //    "Success.aspx",
            //    _pageUrl,
            //    "");
            //#endregion

            try
            {
                if (!IsPostBack)
                {
                    string paymentIdString = string.Empty;

                    var result = new trans_result();

                    if (Request.Cookies["trans_id"] != null)
                    {
                        var trans_id = Request.Cookies["trans_id"].Value;
                        DAL.CollectionOnlineAttempt onlinePaymentCollection = null; // CollectionOnlineAttemptManager.GetByTranxId(trans_id);
                        using (var db = new CandidateDataManager())
                        {
                            onlinePaymentCollection = db.AdmissionDB.CollectionOnlineAttempts.Where(x => x.TransactionId == trans_id).FirstOrDefault();
                        }

                        if (onlinePaymentCollection != null)
                        {
                            DAL.CandidatePayment cp = null;
                            DAL.BasicInfo basicInfo = null;
                            using (var db = new CandidateDataManager())
                            {
                                cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == onlinePaymentCollection.PaymentId).FirstOrDefault();
                                basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == onlinePaymentCollection.CandidateId).FirstOrDefault();
                            }

                            if (cp != null && cp.IsPaid == false)
                            {

                                paymentIdString = cp.PaymentId.ToString();

                                string fundType = onlinePaymentCollection.FundTypeFacultyId; //billHistoryMaster != null ? billHistoryMaster.FundId.ToString() : "";

                                var response = HttpClientApi(onlinePaymentCollection.TransactionId, onlinePaymentCollection.PaymentId.ToString(), fundType);
                                
                                if (response.status.Equals("ok"))
                                {
                                    var message = response.message;

                                    result = DecodeTransactionResult(message);

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        string logNewObject = "1_PaymentID: " + paymentIdString + "; Response Status: " + response.status + "; Response message: " + response.message;

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Online_Payment_Rocket";
                                        dLog.PageName = "successurlRocket.aspx";
                                        dLog.OldData = null;
                                        dLog.NewData = logNewObject;
                                        dLog.UserId = (long)cp.CandidateID;
                                        dLog.SessionInformation = _pageUrl;
                                        dLog.DateTime = DateTime.Now;

                                        dLog.Attribute1 = "Success";

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                }
                                else
                                {

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        string logNewObject = "2_PaymentID: " + paymentIdString + "; Response Failed. Becuase of # " + response.message.ToString();

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Online_Payment_Rocket";
                                        dLog.PageName = "successurlRocket.aspx";
                                        dLog.OldData = null;
                                        dLog.NewData = logNewObject;
                                        dLog.UserId = (long)cp.CandidateID;
                                        dLog.SessionInformation = _pageUrl;
                                        dLog.DateTime = DateTime.Now;

                                        dLog.Attribute1 = "Failed";

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion
                                }

                                if (result.result.Equals("OK") && result.result_code.Equals("000"))
                                {
                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        string logNewObject = "3_PaymentID: " + paymentIdString + "; Verification successful.";

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Online_Payment_Rocket";
                                        dLog.PageName = "successurlRocket.aspx";
                                        dLog.OldData = null;
                                        dLog.NewData = logNewObject;
                                        dLog.UserId = (long)cp.CandidateID;
                                        dLog.SessionInformation = _pageUrl;
                                        dLog.DateTime = DateTime.Now;

                                        dLog.Attribute1 = "Success";

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion


                                    #region Candidate Payment Update
                                    cp.IsPaid = true;
                                    cp.ModifiedBy = cp.CandidateID;
                                    cp.DateModified = DateTime.Now;

                                    using (var db = new CandidateDataManager())
                                    {
                                        db.Update<DAL.CandidatePayment>(cp);
                                    }

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        string logNewObject = "4_PaymentID: " + paymentIdString + "; Candidate Payment Updated Successfully.";

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Online_Payment_Rocket";
                                        dLog.PageName = "successurlRocket.aspx";
                                        dLog.OldData = null;
                                        dLog.NewData = logNewObject;
                                        dLog.UserId = (long)cp.CandidateID;
                                        dLog.SessionInformation = _pageUrl;
                                        dLog.DateTime = DateTime.Now;

                                        dLog.Attribute1 = "Success";

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion 
                                    #endregion



                                    #region Collection Online Attempt Update
                                    var logmsg = "PaymentID:" + paymentIdString +
                                                                    "; TransactionID:" + trans_id +
                                                                    "; Result:" + result.result +
                                                                    "; ResultCode:" + result.result_code +
                                                                    "; ApprovedCode:" + result.approved_code +
                                                                    "; CardName:" + result.card_name +
                                                                    "; CardNumber:" + result.card_number +
                                                                    "; TransDateTime:" + result.trans_date +
                                                                    "; Amount:" + result.amount +
                                                                    "; BankInvoiceNumber:" + result.rrn;


                                    onlinePaymentCollection.ResultCode = result.result_code;
                                    onlinePaymentCollection.TransactionEndDate = DateTime.Now;
                                    onlinePaymentCollection.IsPaid = true;
                                    onlinePaymentCollection.ModifiedBy = (long)cp.CandidateID;
                                    onlinePaymentCollection.ModifiedDate = DateTime.Now;

                                    using (var db = new CandidateDataManager())
                                    {
                                        db.Update<DAL.CollectionOnlineAttempt>(onlinePaymentCollection);
                                    }

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        string logNewObject = "5_PaymentID: " + paymentIdString + "; Collection Online Attempt Update Successfully. LogMsg: " + logmsg;

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Online_Payment_Rocket";
                                        dLog.PageName = "successurlRocket.aspx";
                                        dLog.OldData = null;
                                        dLog.NewData = logNewObject;
                                        dLog.UserId = (long)cp.CandidateID;
                                        dLog.SessionInformation = _pageUrl;
                                        dLog.DateTime = DateTime.Now;

                                        dLog.Attribute1 = "Success";

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion 
                                    #endregion


                                    lblPaymentStatus.Text = "Payment successfull!";
                                    lblPaymentStatus.ForeColor = System.Drawing.Color.Green;
                                    lblName.Text = basicInfo.FirstName;

                                    
                                }
                                else
                                {
                                    var logmsg = "PaymentID:" + paymentIdString +
                                        "; TransactionID:" + trans_id +
                                        "; Result:" + result.result +
                                        "; ResultCode:" + result.result_code +
                                        "; ApprovedCode:" + result.approved_code +
                                        "; CardName:" + result.card_name +
                                        "; CardNumber:" + result.card_number +
                                        "; TransDateTime:" + result.trans_date +
                                        "; Amount:" + result.amount +
                                        "; BankInvoiceNumber:" + result.rrn;

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        string logNewObject = "6_PaymentID: " + paymentIdString + "; Response & Payment Failed. LogMsg: " + logmsg;

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Online_Payment_Rocket";
                                        dLog.PageName = "successurlRocket.aspx";
                                        dLog.OldData = null;
                                        dLog.NewData = logNewObject;
                                        dLog.UserId = (long)cp.CandidateID;
                                        dLog.SessionInformation = _pageUrl;
                                        dLog.DateTime = DateTime.Now;

                                        dLog.Attribute1 = "Failed";

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    lblPaymentStatus.Text = "Payment failed! If your payment amount deducted from your account then check after 12:00 AM";
                                    lblPaymentStatus.ForeColor = System.Drawing.Color.Red;
                                    lblName.Text = basicInfo.FirstName;
                                    
                                }
                            }
                            else
                            {
                                lblPaymentStatus.Text = "Invalid Request! Please contact with administrator.";
                                lblPaymentStatus.ForeColor = System.Drawing.Color.Green;
                                lblName.Text = basicInfo.FirstName;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    string logNewObject = "7_No Candidate Found.";

                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "Online_Payment_Rocket";
                                    dLog.PageName = "successurlRocket.aspx";
                                    dLog.OldData = null;
                                    dLog.NewData = logNewObject;
                                    dLog.UserId = (long)cp.CandidateID;
                                    dLog.SessionInformation = _pageUrl;
                                    dLog.DateTime = DateTime.Now;

                                    dLog.Attribute1 = "Failed";

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            lblPaymentStatus.Text = "Payment failed! Because no transaction information found.";
                            lblPaymentStatus.ForeColor = System.Drawing.Color.Red;
                            lblName.Text = "N/A";

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                string logNewObject = "8_Payment failed. Because of transaction id not found. Transaction ID :: " + trans_id;

                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.EventName = "Online_Payment_Rocket";
                                dLog.PageName = "successurlRocket.aspx";
                                dLog.OldData = null;
                                dLog.NewData = logNewObject;
                                //dLog.UserId = (long)cp.CandidateID;
                                dLog.SessionInformation = _pageUrl;
                                dLog.DateTime = DateTime.Now;

                                dLog.Attribute1 = "Failed";

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion
                            
                        }

                        Response.Cookies["trans_id"].Expires = DateTime.Now.AddDays(-1);
                    }
                    else
                    {
                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            string logNewObject = "Land in SuccessURLRocket Page and No TransID Found.";

                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.EventName = "Online_Payment_Rocket";
                            dLog.PageName = "successurlRocket.aspx";
                            dLog.OldData = null;
                            dLog.NewData = logNewObject;
                            //dLog.UserId = (long)cp.CandidateID;
                            dLog.SessionInformation = _pageUrl;
                            dLog.DateTime = DateTime.Now;

                            dLog.Attribute1 = "Success";

                            LogWriter.DataLogWriter(dLog);
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                lblPaymentStatus.Text = "Payment failed! If your payment amount deducted from your account then check after 12:00 AM";
                lblPaymentStatus.ForeColor = System.Drawing.Color.Red;
                lblName.Text = "N/A";

                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                try
                {
                    string logNewObject = "9_Exception: " + ex.Message;

                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "Online_Payment_Rocket";
                    dLog.PageName = "successurlRocket.aspx";
                    dLog.OldData = null;
                    dLog.NewData = logNewObject;
                    //dLog.UserId = (long)cp.CandidateID;
                    dLog.SessionInformation = _pageUrl;
                    dLog.DateTime = DateTime.Now;

                    dLog.Attribute1 = "Failed";

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ex2)
                {
                }
                #endregion
                
            }
        }


        //private void SavePayment(string StudentID, string trans_Id, string orderId, string Amount, string transDate, string resultCode, string bankInvoiceNo, string log_msg)
        //{
        //    //try
        //    //{
        //    //    var userObj = UserManager.GetByLogInId(logInID);
        //    //    string studentRoll = StudentID.Trim();
        //    //    LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetByRoll(studentRoll);

        //    //    decimal paidAmount;
        //    //    bool isSuccess = decimal.TryParse(Amount, out paidAmount);
        //    //    if (isSuccess)
        //    //    {
        //    //        DateTime collectionDate = DateTime.Now;
        //    //        BillHistoryOrder bho = BillHistoryMasterManager.InsertCollectionHistoryFromOrderId(bankInvoiceNo, Convert.ToInt32(orderId), Convert.ToDecimal(Amount), collectionDate, "rocket");

        //    //        if (bho.StatusId == 1)
        //    //        {
        //    //            #region update_payment_attempt
        //    //            var paymentAttempt = CollectionOnlineAttemptManager.GetByTranxId(trans_Id);

        //    //            if (paymentAttempt != null)
        //    //            {
        //    //                paymentAttempt.ResultCode = resultCode;
        //    //                paymentAttempt.TransactionEndDate = DateTime.Now;
        //    //                paymentAttempt.IsSuccess = true;
        //    //                paymentAttempt.ModifiedBy = paymentAttempt.CreatedBy;
        //    //                paymentAttempt.ModifiedDate = DateTime.Now;

        //    //                CollectionOnlineAttemptManager.Update(paymentAttempt);
        //    //            }
        //    //            #endregion

        //    //            #region Log Insert - Success
        //    //            try
        //    //            {
        //    //                LogGeneralManager.Insert(
        //    //                        DateTime.Now,
        //    //                        "",
        //    //                        "",
        //    //                        StudentID,
        //    //                        "",
        //    //                        "",
        //    //                        "Rocket Payment",
        //    //                        "Online Payment Successfull For # " + log_msg,
        //    //                        "normal",
        //    //                        "",
        //    //                        "Success.aspx",
        //    //                        _pageUrl,
        //    //                        studentObj.Roll);
        //    //            }
        //    //            catch (Exception ex)
        //    //            { }
        //    //            #endregion
        //    //        }
        //    //        else
        //    //        {
        //    //            #region Log Insert - Failed
        //    //            try
        //    //            {
        //    //                LogGeneralManager.Insert(
        //    //                        DateTime.Now,
        //    //                        "",
        //    //                        "",
        //    //                        StudentID,
        //    //                        "",
        //    //                        "",
        //    //                        "Rocket Payment",
        //    //                        "Online Payment Failed For # " + log_msg,
        //    //                        "normal",
        //    //                        "",
        //    //                        "Success.aspx",
        //    //                        _pageUrl,
        //    //                        studentObj.Roll);
        //    //            }
        //    //            catch (Exception ex)
        //    //            { }
        //    //            #endregion
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        #region Log Insert
        //    //        try
        //    //        {
        //    //            LogGeneralManager.Insert(
        //    //                DateTime.Now,
        //    //                "",
        //    //                "",
        //    //                StudentID,
        //    //                "",
        //    //                "",
        //    //                "Rocket Payment",
        //    //                "Online Payment Failed # Because Amount Not Valid : " + " For # " + log_msg,
        //    //                "normal",
        //    //                "",
        //    //                "Success.aspx",
        //    //                _pageUrl,
        //    //                studentRoll);
        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            Console.WriteLine(ex.Message);
        //    //        }
        //    //        #endregion
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    #region Log Insert
        //    //    LogGeneralManager.Insert(
        //    //        DateTime.Now,
        //    //        "",
        //    //        "",
        //    //        StudentID,
        //    //        "",
        //    //        "",
        //    //        "Rocket Payment",
        //    //        "Online payment failed. Because of # " + ex.Message,
        //    //        "error",
        //    //        "",
        //    //        "Success.aspx",
        //    //        _pageUrl,
        //    //        StudentID);
        //    //    #endregion
        //    //}
        //}


        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        private trans_detail HttpClientApi(string transId, string refNum, string fundType)
        {
            var urlPath = _basePath + "GetTransactionResult";
            using (var client = new WebClient())
            {
                client.Headers.Add("secretKey", "4b4d40f4-3349-4c10-9a67-cc92813cf4ba");
                client.Headers.Add("authority", "bup");
                client.Headers.Add("fundType", fundType.ToString());

                var values = new NameValueCollection();
                values["transid"] = transId;
                values["clintip"] = GetIPAddress(); //"103.109.52.2";
                values["txnrefnum"] = refNum;

                var response = client.UploadValues(urlPath, values);

                var responseString = Encoding.Default.GetString(response);
                var jsonSerializer = new JavaScriptSerializer();
                var responseMessage = jsonSerializer.Deserialize<trans_detail>(responseString);

                return responseMessage;
            }
        }

        private trans_result DecodeTransactionResult(string resultString)
        {
            try
            {
                var result = new trans_result();

                var resultPart = resultString.Split('^');

                foreach (var item in resultPart)
                {
                    string[] allpart = item.Split('>');
                    string part1 = allpart[0].Trim();
                    string part2 = allpart[1].Trim();

                    if (part1.Equals("RESULT"))
                    {
                        result.result = part2;
                    }

                    if (part1.Equals("RESULT_CODE"))
                    {
                        result.result_code = part2;
                    }

                    if (part1.Equals("APPROVAL_CODE"))
                    {
                        result.approved_code = part2;
                    }

                    if (part1.Equals("CARD_NUMBER"))
                    {
                        result.card_number = part2;
                    }

                    if (part1.Equals("AMOUNT"))
                    {
                        result.amount = (Convert.ToDecimal(part2) / 100).ToString();
                    }

                    if (part1.Equals("TRANS_DATE"))
                    {
                        result.trans_date = part2;
                    }

                    if (part1.Equals("DESCRIPTION"))
                    {
                        result.description = part2;
                    }

                    if (part1.Equals("RRN"))
                    {
                        result.rrn = part2;
                    }

                    if (part1.Equals("CARDNAME"))
                    {
                        result.card_name = part2;
                    }

                    if (part1.Equals("3DSECURE"))
                    {
                        result.secure_3d = part2;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class trans_detail
        {
            public string status { get; set; }
            public string message { get; set; }
        }

        public class trans_result
        {
            public string result { get; set; }
            public string result_code { get; set; }
            public string approved_code { get; set; }
            public string secure_3d { get; set; }
            public string rrn { get; set; }
            public string card_name { get; set; }
            public string card_number { get; set; }
            public string amount { get; set; }
            public string trans_date { get; set; }
            public string description { get; set; }

        }












    }
}