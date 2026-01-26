using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class failurlfpg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dataReceived = String.Empty;
            StringBuilder builder = new StringBuilder();

            string[] keys = Request.Form.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                builder.Append(keys[i] + ": " + Request.Form[keys[i]] + ";");
            }
            dataReceived = builder.ToString();

            if (!String.IsNullOrEmpty(dataReceived))
            {
                #region TempTable
                //////////////////////////////////////////////////////////////////
                DAL.TemporaryTable_1 ttdatarec = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "Failed: DataReceived: " + dataReceived };
                using (var db = new OfficeDataManager())                        //
                {                                                               //
                    db.Insert<DAL.TemporaryTable_1>(ttdatarec);                 //
                }                                                               //
                //////////////////////////////////////////////////////////////////
                #endregion
            }
            else
                hfData.Value = "No data";

            #region REQUEST POST DATA
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
            if (string.IsNullOrEmpty(requestForm_OrderNo))
            {
                #region TempTable
                //hfData.Value += "Order no is null or empty ; ";
                //////////////////////////////////////////////////////////////////
                DAL.TemporaryTable_1 ordno = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "Failed: DataReceived: " + dataReceived + " ; Order no is null or empty ;" };
                using (var db = new OfficeDataManager())                        //
                {                                                               //
                    db.Insert<DAL.TemporaryTable_1>(ordno);                     //
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
                    //systemAmount = candidatePaymentObj.Amount;
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
                    DAL.TemporaryTable_1 cpay = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "Failed: DataReceived: " + dataReceived + " ; CandidatePayment Obj is null ;" };
                    using (var db = new OfficeDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(cpay);                      //
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
                DAL.TemporaryTable_1 cObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "Failed: DataReceived: " + dataReceived + " ; Candidate Obj is null ;" };
                using (var db = new OfficeDataManager())                        //
                {                                                               //
                    db.Insert<DAL.TemporaryTable_1>(cObj);                      //
                }                                                               //
                //////////////////////////////////////////////////////////////////
                #endregion
            }
            #endregion

            #region SAVE TRANSACTION HISTORY
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
                transactionHistory.Status = "Failed";
                transactionHistory.SystemGeneratedHashKey = null;
                transactionHistory.TransactionAmount = requestForm_TxnAmount;
            }

            SavePayment(transactionHistory);
            #endregion
        }

        private void SavePayment(DAL.TransactionHistoryFPG transactionHistory)
        {
            //insert into TransactionHistoryFPG
            long transactionHistoryID = -1;
            if (transactionHistory != null)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        db.Insert<DAL.TransactionHistoryFPG>(transactionHistory);
                        transactionHistoryID = transactionHistory.ID;
                    }
                }
                catch (Exception ex)
                {
                    //hfData.Value += "save th: " + ex.Message + "; " + ex.InnerException.Message + " ; ";
                    #region TempTable
                    //////////////////////////////////////////////////////////////////
                    DAL.TemporaryTable_1 savePayTranexObj = new DAL.TemporaryTable_1 { DateTimeNow = DateTime.Now, Note = "Failed: SavePaymentTransaction: Exception: " + ex.Message + " ; InnerException: " + ex.InnerException.Message };
                    using (var db = new OfficeDataManager())                        //
                    {                                                               //
                        db.Insert<DAL.TemporaryTable_1>(savePayTranexObj);          //
                    }                                                               //
                    //////////////////////////////////////////////////////////////////
                    #endregion
                }
            }
            //lblName.Text = transactionHistory.CandidateName;
            //lblPaymentId.Text = transactionHistory.CandidatePaymentID.ToString();
        } //end method SavePayment
    }
}