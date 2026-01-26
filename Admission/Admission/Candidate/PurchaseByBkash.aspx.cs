using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class PurchaseByBkash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["referenceNo"]))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator." +
                    " Note : Please do not make any payment without contacting the administrator. Error Code = PN01X001PL ?type=danger", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    string referenceNo = Request.QueryString["referenceNo"].ToString();

                    string paymentId = null;
                    string amountToPayStr = null;
                    string candidateName = null;
                    string candidateEmail = null;
                    string merchantAccountNo = null;
                    long candidateId = -1;

                    long cPaymentId = -1;
                    if (!string.IsNullOrEmpty(referenceNo))
                    {
                        try
                        {
                            cPaymentId = Convert.ToInt64(referenceNo);
                        }
                        catch (Exception)
                        {
                            contentPanel.Visible = false;
                            messagePanel.Visible = true;
                            lblMessage.Text = "Unable to get payment id.";
                            messagePanel.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {
                        cPaymentId = -1;
                        contentPanel.Visible = false;
                        messagePanel.Visible = true;
                        lblMessage.Text = "Unable to get payment id(1).";
                        messagePanel.CssClass = "alert alert-danger";
                    }

                    List<DAL.CandidatePayment> candidatePaymentList = null;

                    if(cPaymentId > 0)
                    {
                        try
                        {
                            using(var db = new CandidateDataManager())
                            {
                                candidatePaymentList = db.AdmissionDB.CandidatePayments.Where(c => c.ID == cPaymentId).ToList();
                            }
                        }
                        catch (Exception)
                        {
                            contentPanel.Visible = false;
                            messagePanel.Visible = true;
                            lblMessage.Text = "Error getting candidate payment information.";
                            messagePanel.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {
                        contentPanel.Visible = false;
                        messagePanel.Visible = true;
                        lblMessage.Text = "Payment Id not available.";
                        messagePanel.CssClass = "alert alert-danger";
                    }

                    if(candidatePaymentList != null)
                    {
                        if(candidatePaymentList.Count == 1)
                        {
                            DAL.CandidatePayment cPayment = new DAL.CandidatePayment();
                            cPayment = candidatePaymentList.First();

                            if(cPayment.ID == cPaymentId)
                            {
                                int amountToBePaid = -1;
                                double amount = -0.0;
                                //string paymentIdStr = null;

                                amount = Convert.ToDouble(cPayment.Amount) + (Convert.ToDouble(cPayment.Amount) * (1.5 / 100)); //if cPayment.Amount=1000, then amount is 1015
                                amountToBePaid = Convert.ToInt32(Math.Ceiling(amount));

                                lblPaymentId.Text = cPayment.PaymentId.ToString();
                                lblAmount.Text = amountToBePaid.ToString();

                                paymentId = cPayment.PaymentId.ToString();
                                amountToPayStr = amountToBePaid.ToString();
                                //merchantAccountNo = "01769028780";


                                #region Get Merchant Account No.
                                DAL.CandidateFormSl cForm = null;
                                try
                                {
                                    using (var db = new CandidateDataManager())
                                    {
                                        cForm = db.AdmissionDB.CandidateFormSls.Where(c => c.CandidatePaymentID == cPayment.ID).FirstOrDefault();
                                    }
                                }
                                catch (Exception)
                                {
                                    contentPanel.Visible = false;
                                    messagePanel.Visible = true;
                                    lblMessage.Text = "Error getting candidate form information.";
                                    messagePanel.CssClass = "alert alert-danger";
                                }

                                DAL.AdmissionSetup admSetup = null;
                                if(cForm != null)
                                {
                                    try
                                    {
                                        using (var db = new OfficeDataManager())
                                        {
                                            admSetup = db.AdmissionDB.AdmissionSetups.Find(cForm.AdmissionSetupID);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        contentPanel.Visible = false;
                                        messagePanel.Visible = true;
                                        lblMessage.Text = "Error getting exam.";
                                        messagePanel.CssClass = "alert alert-danger";
                                    }
                                }

                                if(admSetup != null)
                                {
                                    merchantAccountNo = admSetup.Attribute1;
                                }
                                else
                                {
                                    merchantAccountNo = "01769028780";
                                }
                                #endregion

                            }

                            DAL.BasicInfo candidate = null;
                            try
                            {
                                using(var db = new CandidateDataManager())
                                {
                                    candidate = db.AdmissionDB.BasicInfoes.Find(cPayment.CandidateID);
                                }
                            }
                            catch (Exception)
                            {
                                contentPanel.Visible = false;
                                messagePanel.Visible = true;
                                lblMessage.Text = "Error getting candidate information.";
                                messagePanel.CssClass = "alert alert-danger";
                            }

                            if(candidate != null)
                            {
                                candidateName = candidate.FirstName;
                                candidateEmail = candidate.Email;
                                candidateId = candidate.ID;
                            }
                            else
                            {
                                candidateName = null;
                                candidateEmail = null;
                                candidateId = -1;
                            }
                        }
                        else if(candidatePaymentList.Count > 1)
                        {
                            paymentId = null;
                            amountToPayStr = null;
                            merchantAccountNo = null;

                            contentPanel.Visible = false;
                            messagePanel.Visible = true;
                            lblMessage.Text = "Multiple payment id found. Please contact system administrator for further information.";
                            messagePanel.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {
                        contentPanel.Visible = false;
                        messagePanel.Visible = true;
                        lblMessage.Text = "Candidate payment information not found Please contact system administrator for further information.";
                        messagePanel.CssClass = "alert alert-danger";
                    }

                    #region Send Email
                    if (candidateName != null && candidateEmail != null && amountToPayStr != null
                            && merchantAccountNo != null && paymentId != null && candidateId > 0)
                    {
                        string mailBody = "Dear " + candidateName + ", <br/><br/>" +
                                "<p>Please check the information below about how to pay using bKash: </p>" +
                                "<br/>" +
                                "<p>" +
                                    "1. Dial * 247#<br/>" +
                                    "2. Select 'Payment' option.<br/>" +
                                    "3. Enter Merchant bKash Account No: " + merchantAccountNo + "<br/>" +
                                    "4. Enter amount:" + amountToPayStr + "<br/>" +
                                    "5. Enter reference: " + paymentId + "<br/>" +
                                    "6. Enter counter number:  1 <br/>" +
                                    "7. Enter your PIN number.<br/>" +
                                "</p>" +
                                "<p>" +
                                    "* Once you have successfully made the payment, you will receive an SMS with your payment details.<br/>" +
                                    "* Go to Admission Home Page and click  Verify / Complete Payment .<br/>" +
                                    "* Enter your TrxID, that you have received via SMS, in the Verify or Complete Payment Made By bKash  section on the right. <br/>" +
                                    "* Click Verify bKash Transaction to verify your payment.<br/>" +
                                "</p>" +
                                "<p>" +
                                "<strong>Important</strong><br/>" +
                                    "* Transactions can only be done using Personal bKash Account.<br/>" +
                                    "* The transaction has to be made using the Payment option from bKash menu.<br/>" +
                                    "* 10 digits Payment ID numbers must be used in the Reference section. No symbols, space or punctuation marks can be used.<br/>" +
                                    "* For counter number, individuals always has to input “1” in the designated section.<br/>" +
                                "</p>" +
                                "<br/>" +
                                "Regards, <br/>" +
                                "ICT Centre, Bangladesh University of Professionals (BUP)" +
                                "";

                        string fromAddress = "no-reply-2@bup.edu.bd";
                        string senderName = "BUP Admission";
                        string subject = "Instruction. How to Pay using bKash.";

                        bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);

                        if (isSentEmail == true)
                        {
                            DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                            eLog.MessageBody = mailBody;
                            eLog.MessageSubject = subject;
                            eLog.Page = "PurchaseByBkash.aspx";
                            eLog.SentBy = "System";
                            eLog.StudentId = candidateId;
                            eLog.ToAddress = candidateEmail;
                            eLog.ToName = candidateName;
                            eLog.DateSent = DateTime.Now;
                            eLog.FromAddress = fromAddress;
                            eLog.FromName = senderName;
                            eLog.Attribute1 = "Success";

                            LogWriter.EmailLog(eLog);
                        }
                        else if (isSentEmail == false)
                        {
                            DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                            eLog.MessageBody = mailBody;
                            eLog.MessageSubject = subject;
                            eLog.Page = "PurchaseByBkash.aspx";
                            eLog.SentBy = "System";
                            eLog.StudentId = candidateId;
                            eLog.ToAddress = candidateEmail;
                            eLog.ToName = candidateName;
                            eLog.DateSent = DateTime.Now;
                            eLog.FromAddress = fromAddress;
                            eLog.FromName = senderName;
                            eLog.Attribute1 = "Failed";

                            LogWriter.EmailLog(eLog);
                        }
                    }
                    else
                    {
                        contentPanel.Visible = false;
                        messagePanel.Visible = true;
                        lblMessage.Text = "Unable to send email. One or more info is missing.";
                        messagePanel.CssClass = "alert alert-danger";
                    }
                    #endregion
                }
            }
        }//end OnLoad
    }
}