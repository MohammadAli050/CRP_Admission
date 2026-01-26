using CommonUtility;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Admission.Admission
{
    public partial class VerifyPayment : System.Web.UI.Page
    {
        private readonly string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //empty


                //btnVerifyBkashTrans.Visible = false;

                //TODO: 
                //disable buttons if turned off from office.
            }
        }

        /// <summary>
        /// Button click action for payment made by SSL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            long paymentId = -1;
            DAL.CandidatePayment cPaymentObj = null;
            DAL.BasicInfo candidate = null;
            DAL.CandidateFormSl cFormSlObj = null;
            List<DAL.CandidateFormSl> cFormSlList = null;
            DAL.AdmissionSetup admSetup = null;
            List<DAL.AdmissionSetup> admSetupList = null;
            DAL.AdmissionUnit admUnit = null;
            List<DAL.AdmissionUnit> admUnitList = null;
            int educationCategoryId = -1;

            try
            {
                paymentId = Int64.Parse(txtPaymentId.Text.Trim());
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Invalid Payment ID";
                lblMessage.ForeColor = Color.Crimson;
                return;
            }

            #region CANDIDATE PAYMENT
            if (paymentId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        cPaymentObj = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == paymentId)
                            .FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    cPaymentObj = null;
                }
            }//end if (paymentId > 0)
            #endregion

            #region CANDIDATE & CANDIDATE FORMSERIAL
            if (cPaymentObj != null)
            {
                long candidateId = -1;
                candidateId = Int64.Parse(cPaymentObj.CandidateID.ToString());
                if (candidateId > 0)
                {
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            candidate = db.GetCandidateBasicInfoByID_AD(candidateId);
                        }
                    }
                    catch (Exception ex)
                    {
                        candidate = null;
                    }

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cFormSlList = db.AdmissionDB.CandidateFormSls
                                .Where(c => c.CandidatePaymentID == cPaymentObj.ID && c.CandidateID == candidateId)
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        cFormSlList = null;
                    }
                }
            } // end if (cPaymentObj != null)
            #endregion

            #region ADMISSION SETUP AND ADMISSION UNIT
            if (cFormSlList.Count() == 1) 
            {
                // if only one form exists for this paymentId (could be single or multiple). 
                //Also Education category could be Bachelors or Marters.
                foreach (var item in cFormSlList)
                {
                    cFormSlObj = item;
                }
                if (cFormSlObj != null) {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admSetup = db.AdmissionDB.AdmissionSetups.Find(cFormSlObj.AdmissionSetupID);
                            educationCategoryId = admSetup.EducationCategoryID; // since this could be either bachelors or masters, hence get the education category.
                        }
                    }
                    catch (Exception ex)
                    {
                        admSetup = null;
                    }
                }

                if(admSetup != null)
                {
                    try
                    {
                        using(var db = new OfficeDataManager())
                        {
                            admUnit = db.AdmissionDB.AdmissionUnits.Find(admSetup.AdmissionUnitID);
                        }
                    }
                    catch (Exception ex)
                    {
                        admUnit = null;
                    }
                }
            }
            // else if multiple form exists for this paymentId (multiple purchase varification). 
            // Should be Bachelors only. If Masters comes here, then there is something wrong in MultipleApplication.aspx.cs.
            else if (cFormSlList.Count() > 1) 
            {
                try
                {
                    foreach(var item in cFormSlList)
                    {
                        using(var db = new OfficeDataManager())
                        {
                            DAL.AdmissionSetup _admSetupTemp = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                            if(_admSetupTemp != null)
                            {
                                admSetupList.Add(_admSetupTemp);
                                // only candidates applying for Bachelors purchasing multiple applicaiton should be here.
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    admSetupList = null;
                }

                if(admSetupList.Count() > 1)
                {
                    try
                    {
                        foreach (var item in admSetupList)
                        {
                            using (var db = new OfficeDataManager())
                            {
                                DAL.AdmissionUnit _admUnitTemp = db.AdmissionDB.AdmissionUnits.Find(item.AdmissionUnitID);
                                if (_admUnitTemp != null)
                                {
                                    admUnitList.Add(_admUnitTemp); 
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        admUnitList = null;
                    }
                }
            } // end if-else cFormSls.Count()
            #endregion

            // Only One form means this could be either Bachelor or Masters.
            if(cPaymentObj != null && candidate != null && cFormSlObj != null && (cFormSlList.Count() == 1) && admSetup != null && admUnit != null)
            {
                //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                //            + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
                //            + admissionSetup.EducationCategoryID + ";";
                string urlParam = candidate.ID + ";" + cPaymentObj.ID + ";"
                            + cFormSlObj.ID + ";0;" + admUnit.ID + ";"
                            + admSetup.EducationCategoryID + ";";

                //TODO: Insert Log Here

                Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
            }
            // More than one form exist for one payment Id, so this is Bachelor application, and most likely was a multiple purchase.
            else if (cPaymentObj != null && candidate != null && (cFormSlList.Count() > 1) && (admSetupList.Count() > 1) && (admSetupList.Count() > 1))
            {
                //string urlParam = cId + ";" + candidatePaymentIDLong + ";"
                //                + -1 + ";1;" + -1 + ";"
                //                + 4 + ";";
                string urlParam = candidate.ID + ";" + cPaymentObj.ID + ";"
                                + -1 + ";1;" + -1 + ";"
                                + 4 + ";";

                //TODO: Insert Log Here
                
                Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
            }

        }

        /// <summary>
        /// Button click action for verification of Bkash Payment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


        //protected void btnVerifyBkashTrans_Click(object sender, EventArgs e)
        //{
        //    string trxId = null;
        //    trxId = txtTrxId.Text.Trim();

        //    string user = "BUPTWO";
        //    string pass = "BDunipro02@2";
        //    string msisdn = "01769028780";

        //    if (!string.IsNullOrEmpty(trxId))
        //    {
        //        string url = null;

        //        url = String.Format(@"https://www.bkashcluster.com:9081/dreamwave/merchant/trxcheck/sendmsg?user={0}&pass={1}&msisdn={2}&trxid={3}"
        //                    , user, pass, msisdn, trxId);

        //        if (!string.IsNullOrEmpty(url))
        //        {
        //            var xmlData = new WebClient().DownloadString(url);

        //            //var xdoc = XDocument.Parse(xml);
        //            //var items = xdoc.Descendants("Item")
        //            //                .ToDictionary(i => (string)i.Attribute("Key"),
        //            //                              i => (string)i.Attribute("Value"));


        //            var xDoc = XDocument.Parse(xmlData);

        //            string trxStatus = xDoc.Descendants().First(node => node.Name == "trxStatus").Value.ToString();

        //            //string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
        //            //string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
        //            //string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
        //            //string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
        //            //string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
        //            //string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
        //            //string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
        //            //string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
        //            //string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();


        //            #region trxStatus.Equals("0000") (SUCCESSFUL)
        //            if (trxStatus.Equals("0000"))//successfull
        //            {
        //                string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();
        //                string service = xDoc.Descendants().First(node => node.Name == "service").Value.ToString();
        //                string sender_1 = xDoc.Descendants().First(node => node.Name == "sender").Value.ToString();
        //                string receiver = xDoc.Descendants().First(node => node.Name == "receiver").Value.ToString();
        //                string currency = xDoc.Descendants().First(node => node.Name == "currency").Value.ToString();
        //                string amount = xDoc.Descendants().First(node => node.Name == "amount").Value.ToString();
        //                string reference = xDoc.Descendants().First(node => node.Name == "reference").Value.ToString();
        //                string counter = xDoc.Descendants().First(node => node.Name == "counter").Value.ToString();
        //                string trxTimestamp = xDoc.Descendants().First(node => node.Name == "trxTimestamp").Value.ToString();

        //                DAL.CandidatePayment cPayment = null;
        //                #region GetCandidatePayment
        //                long paymentId = Convert.ToInt64(reference);
        //                if (paymentId > 0) {
        //                    try
        //                    {
        //                        using (var db = new CandidateDataManager())
        //                        {
        //                            cPayment = db.AdmissionDB.CandidatePayments.Where(c => c.PaymentId == paymentId).FirstOrDefault();
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        lblbKashMsg.Text = "Error getting candidate payment information.";
        //                        panelBkashMsg.CssClass = "alert alert-danger";
        //                        panelBkashMsg.Visible = true;
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    lblbKashMsg.Text = "Error getting reference from bKash.";
        //                    panelBkashMsg.CssClass = "alert alert-danger";
        //                    panelBkashMsg.Visible = true;
        //                    return;
        //                }
        //                #endregion

        //                if (cPayment != null)
        //                {
        //                    #region GetBasicInfo
        //                    DAL.BasicInfo candidate = null;
        //                    try
        //                    {
        //                        using(var db = new CandidateDataManager())
        //                        {
        //                            candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        lblbKashMsg.Text = "Error getting candidate information.";
        //                        panelBkashMsg.CssClass = "alert alert-danger";
        //                        panelBkashMsg.Visible = true;
        //                        return;
        //                    }
        //                    #endregion

        //                    int amountToBePaid = -1;
        //                    double amount_1 = -0.0;
        //                    //string paymentIdStr = null;

        //                    amount_1 = Convert.ToDouble(cPayment.Amount) + (Convert.ToDouble(cPayment.Amount) * (1.5 / 100));
        //                    amountToBePaid = Convert.ToInt32(Math.Ceiling(amount_1));

        //                    int amountFromGateway = (int)Convert.ToDouble(amount);
        //                    long referenceFromGateway = Int64.Parse(reference);
                            
        //                    if((amountToBePaid <= amountFromGateway) && (referenceFromGateway == cPayment.PaymentId))//if paid amount does not match
        //                    {
        //                        #region GetExistingTransactionBkash
        //                        DAL.TransactionHistoryBKash existingTrx = null;
        //                        try
        //                        {
        //                            using(var db = new CandidateDataManager())
        //                            {
        //                                existingTrx = db.AdmissionDB.TransactionHistoryBKashes
        //                                    .Where(c => c.CandidateID == cPayment.CandidateID &&
        //                                        c.ReferencePaymentID == cPayment.PaymentId.ToString() &&
        //                                        c.TrxID == trx_Id &&
        //                                        c.TrxStatus == trxStatus).FirstOrDefault();
        //                            }
        //                        }
        //                        catch (Exception)
        //                        {
        //                            lblbKashMsg.Text = "Error getting existing bKash transaction";
        //                            panelBkashMsg.CssClass = "alert alert-danger";
        //                            panelBkashMsg.Visible = true;
        //                            return;
        //                        }
        //                        #endregion

        //                        if (existingTrx == null)
        //                        {
        //                            try
        //                            {
        //                                #region InsertTransactionHistoryBkash
        //                                DAL.TransactionHistoryBKash sTrx = new DAL.TransactionHistoryBKash(); // sTrx = successfull Transaction
        //                                sTrx.Amount = amount;
        //                                sTrx.CandidateID = cPayment.CandidateID;
        //                                sTrx.Counter = counter;
        //                                sTrx.CreatedOn = DateTime.Now;
        //                                sTrx.Currency = currency;
        //                                sTrx.DateManualInsert = null;
        //                                sTrx.IsManualInsert = false;
        //                                sTrx.ManualInsertBy = null;
        //                                sTrx.Receiver = receiver;
        //                                sTrx.ReferencePaymentID = reference;
        //                                sTrx.Reversed = "No";
        //                                sTrx.Sender = sender_1;
        //                                sTrx.Service = service;
        //                                sTrx.TrxID = trx_Id;
        //                                sTrx.TrxStatus = trxStatus;
        //                                sTrx.TrxTimestamp = trxTimestamp;
        //                                if (candidate != null)
        //                                {
        //                                    sTrx.ValueA = candidate.ID.ToString();
        //                                    sTrx.ValueB = candidate.FirstName;
        //                                }
        //                                sTrx.ValueC = cPayment.Amount.ToString();
        //                                sTrx.Attribute1 = "Success";

        //                                using (var dbsTrx = new CandidateDataManager())
        //                                {
        //                                    dbsTrx.Insert<DAL.TransactionHistoryBKash>(sTrx);
        //                                }
        //                                #endregion

        //                                #region InsertIntoTransactionHistory
        //                                DAL.TransactionHistory trx = new DAL.TransactionHistory();
        //                                trx.Amount = amount;
        //                                trx.ApiConnect = "done: bKash platform connector";
        //                                trx.Attribute1 = null;
        //                                trx.Attribute2 = null;
        //                                trx.Attribute3 = null;
        //                                trx.Attribute4 = null;
        //                                trx.BankTransactionID = trx_Id;
        //                                trx.BaseFair = null;
        //                                trx.CandidateID = cPayment.CandidateID.ToString();
        //                                trx.CardBrand = "bKash";
        //                                trx.CardIssuer = "bKash";
        //                                trx.CardIssuerCountry = "Bangladesh";
        //                                trx.CardIssuerCountryCode = "BD";
        //                                trx.CardNumber = "trx_id";
        //                                trx.CardType = "bKash Mobile Banking";
        //                                trx.CreatedOn = DateTime.Now;
        //                                trx.Currency = "BDT";
        //                                trx.CurrencyAmount = amountToBePaid.ToString();
        //                                trx.CurrencyRate = "1.0";
        //                                trx.CurrencyType = "BDT";
        //                                trx.DateManualInsert = null;
        //                                trx.GwVersion = null;
        //                                trx.IsInHouseCashTransaction = false;
        //                                trx.IsManualInsert = false;
        //                                trx.ManualInsertBy = null;
        //                                trx.RiskLevel = "Unknown";
        //                                trx.RiskTitle = "Unknown";
        //                                trx.Status = "VALID";
        //                                trx.StoreAmount = cPayment.Amount.ToString();
        //                                trx.TransactionDate = trxTimestamp;
        //                                trx.TransactionID = reference;
        //                                trx.ValidatedOn = DateTime.Now.ToString();
        //                                trx.ValidationID = null;
        //                                trx.ValueA = cPayment.CandidateID.ToString();
        //                                trx.ValueB = candidate.FirstName;
        //                                trx.ValueC = cPayment.Amount.ToString();
        //                                trx.ValueD = null;

        //                                try
        //                                {
        //                                    using(var db = new OfficeDataManager())
        //                                    {
        //                                        db.Insert<DAL.TransactionHistory>(trx);
        //                                    }
        //                                }
        //                                catch (Exception)
        //                                {
        //                                    lblbKashMsg.Text = "Error saving transaction.";
        //                                    panelBkashMsg.CssClass = "alert alert-danger";
        //                                    panelBkashMsg.Visible = true;
        //                                    return;
        //                                }

        //                                #endregion

        //                                #region UpdateCandidatePayment

        //                                cPayment.IsPaid = true;
        //                                cPayment.DateModified = DateTime.Now;
        //                                cPayment.ModifiedBy = cPayment.CandidateID;

        //                                try
        //                                {
        //                                    using(var dbCpayUpdate = new CandidateDataManager())
        //                                    {
        //                                        dbCpayUpdate.Update<DAL.CandidatePayment>(cPayment);
        //                                    }
        //                                }
        //                                catch (Exception)
        //                                {
        //                                    lblbKashMsg.Text = "Error updating candidate payment";
        //                                    panelBkashMsg.CssClass = "alert alert-danger";
        //                                    panelBkashMsg.Visible = true;
        //                                    return;
        //                                }

        //                                #endregion

        //                                #region SendSms

        //                                GetSendingInfo(cPayment.CandidateID);

        //                                #endregion

        //                                lblbKashMsg.Text = "Transaction verified successfully.";
        //                                panelBkashMsg.CssClass = "alert alert-success";
        //                                panelBkashMsg.Visible = true;
        //                                return;
        //                            }
        //                            catch (Exception)
        //                            {
        //                                lblbKashMsg.Text = "Error saving bKash transaction";
        //                                panelBkashMsg.CssClass = "alert alert-danger";
        //                                panelBkashMsg.Visible = true;
        //                                return;
        //                            }
        //                        }

        //                        //lblbKashMsg.Text = "";
        //                        //panelBkashMsg.CssClass = "alert alert-success";
        //                        //panelBkashMsg.Visible = true;
        //                        //return;

        //                    }
        //                    else
        //                    {
        //                        #region InsertTransactionHistoryBkash IF AmountLessPaid
        //                        DAL.TransactionHistoryBKash sTrx = new DAL.TransactionHistoryBKash(); // sTrx = successfull Transaction
        //                        sTrx.Amount = amount;
        //                        sTrx.CandidateID = cPayment.CandidateID;
        //                        sTrx.Counter = counter;
        //                        sTrx.CreatedOn = DateTime.Now;
        //                        sTrx.Currency = currency;
        //                        sTrx.DateManualInsert = null;
        //                        sTrx.IsManualInsert = false;
        //                        sTrx.ManualInsertBy = null;
        //                        sTrx.Receiver = receiver;
        //                        sTrx.ReferencePaymentID = reference;
        //                        sTrx.Reversed = "No";
        //                        sTrx.Sender = sender_1;
        //                        sTrx.Service = service;
        //                        sTrx.TrxID = trx_Id;
        //                        sTrx.TrxStatus = "Less Amount Paid";
        //                        sTrx.TrxTimestamp = trxTimestamp;
        //                        if (candidate != null)
        //                        {
        //                            sTrx.ValueA = candidate.ID.ToString();
        //                            sTrx.ValueB = candidate.FirstName;
        //                        }
        //                        sTrx.ValueC = cPayment.Amount.ToString();
        //                        sTrx.Attribute1 = "Less Amount Paid";

        //                        using (var dbsTrx = new CandidateDataManager())
        //                        {
        //                            dbsTrx.Insert<DAL.TransactionHistoryBKash>(sTrx);
        //                        }
        //                        #endregion

        //                        lblbKashMsg.Text = "Your amount does not match. You were suppose to pay " + amountToBePaid + " BDT, but found " + amount + " BDT paid. <br/>"
        //                            + "Reference No. " + reference;
        //                        panelBkashMsg.CssClass = "alert alert-danger";
        //                        panelBkashMsg.Visible = true;
        //                        return;
        //                    }
                            
        //                }
        //                else
        //                {
        //                    lblbKashMsg.Text = "Candidate payment does not exist in database.";
        //                    panelBkashMsg.CssClass = "alert alert-danger";
        //                    panelBkashMsg.Visible = true;
        //                    return;
        //                }
        //            } //end if(trxStatus.Equals("0000"))
        //            #endregion
        //            //-----------------------------------
        //            #region if(trxStatus != "0000")
        //            else
        //            {
        //                string trx_Id = xDoc.Descendants().First(node => node.Name == "trxId").Value.ToString();

        //                DAL.BkashTrxStatusCode btsCode = null;
        //                try
        //                {
        //                    if (!trxStatus.Equals("0000"))
        //                    {
        //                        using (var db = new OfficeDataManager())
        //                        {
        //                            btsCode = db.AdmissionDB.BkashTrxStatusCodes.Where(c => c.Code == trxStatus).FirstOrDefault();
        //                        }
        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    lblbKashMsg.Text = "Error getting bKash Transaction Status Codes.";
        //                    panelBkashMsg.CssClass = "alert alert-danger";
        //                    panelBkashMsg.Visible = true;
        //                    return;
        //                }

        //                if(btsCode != null)
        //                {
        //                    #region InsertTransactionHistoryBkash IF FAILED
        //                    DAL.TransactionHistoryBKash sTrx = new DAL.TransactionHistoryBKash(); // sTrx = successfull Transaction
        //                    sTrx.Amount = null;
        //                    sTrx.CandidateID = null;
        //                    sTrx.Counter = null;
        //                    sTrx.CreatedOn = DateTime.Now;
        //                    sTrx.Currency = null;
        //                    sTrx.DateManualInsert = null;
        //                    sTrx.IsManualInsert = false;
        //                    sTrx.ManualInsertBy = null;
        //                    sTrx.Receiver = null;
        //                    sTrx.ReferencePaymentID = null;
        //                    sTrx.Reversed = "No";
        //                    sTrx.Sender = null;
        //                    sTrx.Service = null;
        //                    sTrx.TrxID = trx_Id;
        //                    sTrx.TrxStatus = trxStatus;
        //                    sTrx.TrxTimestamp = null;
        //                        sTrx.ValueA = null;
        //                        sTrx.ValueB = null;
        //                    sTrx.ValueC = null;
        //                    sTrx.Attribute1 = btsCode.Interpretation + "; " + btsCode.Message;

        //                    using (var dbsTrx = new CandidateDataManager())
        //                    {
        //                        dbsTrx.Insert<DAL.TransactionHistoryBKash>(sTrx);
        //                    }
        //                    #endregion

        //                    lblbKashMsg.Text = trxStatus + ": " + btsCode.Interpretation + "; "+btsCode.Message;
        //                    panelBkashMsg.CssClass = "alert alert-danger";
        //                    panelBkashMsg.Visible = true;
        //                    return;
        //                }
        //                else
        //                {
        //                    lblbKashMsg.Text = trxStatus + ": Please try again after 5 minutes.";
        //                    panelBkashMsg.CssClass = "alert alert-danger";
        //                    panelBkashMsg.Visible = true;
        //                    return;
        //                }
        //            }
        //            #endregion

        //        }
        //    }
        //    else
        //    {
        //        lblbKashMsg.Text = "Please provide your Transaction ID.";
        //        panelBkashMsg.CssClass = "alert alert-danger";
        //        panelBkashMsg.Visible = true;
        //    }
        //}

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

                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                    }

                    if (candidate != null)
                    {
                        candidateEmail = candidate.Email;
                        candidateSmsPhone = candidate.SMSPhone;
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
                        //SendEmail(candidateEmail, candidateUsername, candidatePassword, candidate.ID);
                    }
                }
            }
        }

        private void SendSms(string smsPhone, string username, string password, long candidateId)
        {
            if (!string.IsNullOrEmpty(smsPhone) && smsPhone.Count() == 14 && smsPhone.Contains("+"))
            {
                string messageBody = "BUP Admission. Login to https://admission.bup.edu.bd/Admission/Login. Username: " + username + " ; Password: " + password + " ";
                string stringData = SMSUtility.Send(smsPhone, messageBody);

                string statusT = JObject.Parse(stringData)["statusCode"].ToString();

                if (statusT != "200") //if sms sending fails
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending failed in VerifyPayment.aspx";
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
                    smsLog.Attribute1 = "Sms sending successful VerifyPayment.aspx";
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
        }
    }
}