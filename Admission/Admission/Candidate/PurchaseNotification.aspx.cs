using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using DAL.ViewModels.EkPayModel;

namespace Admission.Admission.Candidate
{
    public partial class PurchaseNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["value"]))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator." +
                    " Note : Please do not make any payment without contacting the administrator. Error Code = PN01X001PL ?type=danger", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    MessageView("", "clear");
                    try
                    {
                        bool? isMultiple = null;

                        //string value = Decrypt.DecryptString(this.Request.QueryString["value"].ToString());
                        //string[] values = value.Split(';');
                        string value = Request.QueryString["value"].ToString();
                        string[] values = value.Split(';');
                        long candidateId = Convert.ToInt64(values[0]);
                        long candidatePaymentID = Convert.ToInt64(values[1]);
                        long? candidateFormSerialID = Convert.ToInt64(values[2]);
                        if (values[3] == "0")
                        {
                            isMultiple = false;
                        }
                        else if (values[3] == "1")
                        {
                            isMultiple = true;
                        }

                        long admUnitId = Convert.ToInt64(values[4]);
                        int educationCategoryId = Convert.ToInt32(values[5]);

                        string facultyName = "";
                        string candidateName = "";
                        string candidateEmail = "";

                        if (candidatePaymentID > 0)
                        {
                            DAL.CandidatePayment cp = null;
                            using (var db = new CandidateDataManager())
                            {
                                cp = db.AdmissionDB.CandidatePayments.Where(x => x.ID == candidatePaymentID).FirstOrDefault();
                            }
                            if (cp != null)
                            {
                                panelEkPay.Visible = false;
                                int sslpanelVisibility = Convert.ToInt32(CommonEnum.PropertyType.SSLPaymentPanel);
                                if (educationCategoryId == 4)
                                {
                                    DAL.PropertySetup bachelorPropertySetupSSL = null;
                                    using(var dbOfc=new OfficeDataManager())
                                    {
                                        bachelorPropertySetupSSL = dbOfc.AdmissionDB.PropertySetups.Where(x => x.PropertyTypeID == sslpanelVisibility && x.EducationCategoryID == educationCategoryId && x.IsActive==true).FirstOrDefault();

                                    }
                                    if (bachelorPropertySetupSSL != null)
                                    {
                                        if (bachelorPropertySetupSSL.IsVisible == true)
                                        {
                                            panelSSL.Visible = true;
                                        }
                                        else
                                        {
                                            panelSSL.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        panelSSL.Visible = false;
                                    }
                                    
                                }
                                else
                                {

                                    DAL.CandidateFormSl cfsl = null;
                                    using (var db = new CandidateDataManager())
                                    {
                                        cfsl = db.GetCandidateFormSlByCandID_AD(Convert.ToInt64(cp.CandidateID));
                                    }
                                    int? programId = 0;
                                    try
                                    {
                                        programId = cfsl.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.Where(x=>x.IsActive==true && x.AcaCalID==cfsl.AcaCalID).FirstOrDefault().ProgramID;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    DAL.PropertySetup mastersPropertySetupSSL = null;
                                    using (var dbOfc = new OfficeDataManager())
                                    {
                                        mastersPropertySetupSSL = dbOfc.AdmissionDB.PropertySetups.Where(x => x.PropertyTypeID == sslpanelVisibility && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId && x.IsActive == true).FirstOrDefault(); ;

                                    }
                                    if (mastersPropertySetupSSL != null)
                                    {
                                        if (mastersPropertySetupSSL.IsVisible == true)
                                        {
                                            panelSSL.Visible = true;
                                        }
                                        else
                                        {
                                            panelSSL.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        panelSSL.Visible = false;
                                    }

                                    

                                    //if (cfsl != null && cfsl.AdmissionSetup.EducationCategoryID == 6 &&
                                    //    cfsl.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.FirstOrDefault().ProgramID == 1)
                                    //{
                                    //    panelEkPay.Visible = false;
                                    //    panelSSL.Visible = false;
                                    //}
                                    //else
                                    //{
                                    //    panelEkPay.Visible = false;
                                    //    panelSSL.Visible = true;
                                    //}


                                }

                                List<DAL.CandidateFormSl> facultyList = null;
                                using (var db = new CandidateDataManager())
                                {
                                    facultyList = db.GetAllCandidateFormSlByCandID_AD(Convert.ToInt64(cp.CandidateID));
                                }
                                if (facultyList != null && facultyList.Count > 0)
                                {
                                    foreach (var tData in facultyList)
                                    {
                                        facultyName += tData.AdmissionSetup.AdmissionUnit.UnitName + "<br />";
                                    }
                                }

                                DAL.BasicInfo bi = null;
                                using (var db = new CandidateDataManager())
                                {
                                    bi = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cp.CandidateID).FirstOrDefault();
                                }
                                if (bi != null)
                                {
                                    candidateName = bi.FirstName.ToString();
                                    candidateEmail = bi.Email;
                                }


                                if (!string.IsNullOrEmpty(facultyName) && !string.IsNullOrEmpty(candidateName))
                                {
                                    lblCandidateName.Text = candidateName;
                                    lblPaymentID.Text = cp.PaymentId.ToString();
                                    lblAmount.Text = cp.Amount.ToString();
                                    lblPrograms.Text = facultyName;
                                    lblEmail.Text = candidateEmail;
                                }
                                else
                                {
                                    Response.Redirect("~/Admission/Message.aspx?message=Invalid Requist. Please contact the administrator.?type=danger", false);
                                }

                            }
                            else
                            {
                                Response.Redirect("~/Admission/Message.aspx?message=Invalid Requist. Please contact the administrator.?type=danger", false);
                            }
                        }
                        else
                        {
                            Response.Redirect("~/Admission/Message.aspx?message=Invalid Requist. Please contact the administrator.?type=danger", false);
                        }


                    }
                    catch (Exception ex)
                    {
                        //MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
                        string msg = "Exception: " + ex.Message.ToString();
                        Response.Redirect("~/Admission/Message.aspx?message=" + msg + "?type=danger", false);
                    }



                    //if (isMultiple == false) //if not multiple purchase...single form purchase from purchaseForm.aspx.
                    //{
                    //    if (admUnitId > 0)
                    //    {
                    //        using (var db = new CandidateDataManager())
                    //        {
                    //            DAL.AdmissionUnit admUnit = db.AdmissionDB.AdmissionUnits.Find(admUnitId);
                    //            DAL.CandidatePayment candidatePaymentObj = db.GetCandidatePaymentByID_AD(candidatePaymentID);
                    //            DAL.CandidateFormSl candidateFormSlObj = db.GetCandidateFormSlByID_ND(Convert.ToInt64(candidateFormSerialID));
                    //            if (candidatePaymentObj != null && admUnit != null && candidateFormSlObj != null)
                    //            {

                    //                //DAL.AdmissionSetup admSetupObj = null;
                    //                //using (var dbAdmSetup = new OfficeDataManager())
                    //                //{
                    //                //    admSetupObj = dbAdmSetup.AdmissionDB.AdmissionSetups.Find(candidateFormSlObj.AdmissionSetupID);
                    //                //}

                    //                //if (admSetupObj != null)
                    //                //{
                    //                //    if (admSetupObj.Attribute1 == null)
                    //                //    {
                    //                //        btnSubmit_Bkash.Visible = false;
                    //                //        btnSubmit.Visible = true;
                    //                //    }
                    //                //    else if (admSetupObj.Attribute1 != null && (admSetupObj.Attribute1 == "bKash" || admSetupObj.Attribute1 == "01769028780"))
                    //                //    {
                    //                //        btnSubmit_Bkash.Visible = false; //btnSubmit_Bkash.Visible = true; its change to false, Because aggriment with BKash is canceled.
                    //                //        btnSubmit.Visible = true;

                    //                //    }
                    //                //    else if (admSetupObj.Attribute1 != null && admSetupObj.Attribute1 == "fpg")
                    //                //    {
                    //                //        btnSubmit_Bkash.Visible = false; //btnSubmit_Bkash.Visible = true; its change to false, Because aggriment with BKash is canceled.
                    //                //                                         //btnSubmit_Fpg.Visible = true;

                    //                //    }
                    //                //}

                    //                lblCandidateName.Text = candidatePaymentObj.BasicInfo.FirstName;
                    //                string candidateName = candidatePaymentObj.BasicInfo.FirstName;
                    //                string candidateEmail = candidatePaymentObj.BasicInfo.Email;
                    //                lblPaymentID.Text = candidatePaymentObj.PaymentId.ToString();
                    //                lblAmount.Text = candidatePaymentObj.Amount.ToString();
                    //                lblPrograms.Text = admUnit.UnitName;
                    //                //lblNote.Text = "Please note down your Payment ID and save this number for future reference. " +
                    //                //    "An email will be sent to <strong>" + candidateEmail + "</strong> containing your Payment ID. " +
                    //                //    "Using this Payment ID you can pay later.";
                    //                lblEmail.Text = candidateEmail;


                    //                //if (admUnitId == 11 || admUnitId == 12)
                    //                //{
                    //                //    PanelDBBLRocket.Visible = true;
                    //                //    PanelOnlineGateway.Visible = false;
                    //                //}
                    //                //else
                    //                //{
                    //                //    PanelDBBLRocket.Visible = false;
                    //                //    PanelOnlineGateway.Visible = true;
                    //                //}


                    //                #region N/A -- Email & SMS
                    //                //List<DAL.SPGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification_Result> result = null;
                    //                //using (var db1 = new OfficeDataManager())
                    //                //{
                    //                //    result = db1.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification(candidatePaymentObj.PaymentId, null).ToList();
                    //                //}

                    //                //if (result[0].Count_SMS > 0 && result[0].Count_Email > 0)
                    //                //{
                    //                //    //....Candidate already have Email and SMS. dont send again
                    //                //}
                    //                //else
                    //                //{
                    //                //    string mailBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + ", <br/><br/>" +
                    //                //    "Thank you for applying at Bangladesh University of Professionals (BUP). Please check the information below about your payment: " +
                    //                //    "<br/>" + "Faculty: " + admUnit.UnitName + ". " +
                    //                //    "<strong style='color: Tomato;'> Payment ID: " + candidatePaymentObj.PaymentId.ToString() + "</strong>. Date: " + candidatePaymentObj.DateCreated +
                    //                //    "<br/>" +
                    //                //    "</p>" +
                    //                //    "<p>" +
                    //                //    "Regards, <br/>" +
                    //                //    "ICT Centre, Bangladesh University of Professionals (BUP)" +
                    //                //    "</p>";

                    //                //    string fromAddress = "no-reply-2@bup.edu.bd";
                    //                //    string senderName = "BUP Admission";
                    //                //    string subject = "Payment ID for Your Application";

                    //                //    bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);

                    //                //    if (isSentEmail == true)
                    //                //    {
                    //                //        DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    //                //        eLog.MessageBody = mailBody;
                    //                //        eLog.MessageSubject = subject;
                    //                //        eLog.Page = "PurchaseNotification.aspx";
                    //                //        eLog.SentBy = "System";
                    //                //        eLog.StudentId = candidatePaymentObj.CandidateID;
                    //                //        eLog.ToAddress = candidateEmail;
                    //                //        eLog.ToName = candidateName;
                    //                //        eLog.DateSent = DateTime.Now;
                    //                //        eLog.FromAddress = fromAddress;
                    //                //        eLog.FromName = senderName;
                    //                //        eLog.Attribute1 = "Success";

                    //                //        LogWriter.EmailLog(eLog);
                    //                //    }
                    //                //    else if (isSentEmail == false)
                    //                //    {
                    //                //        DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    //                //        eLog.MessageBody = mailBody;
                    //                //        eLog.MessageSubject = subject;
                    //                //        eLog.Page = "PurchaseNotification.aspx";
                    //                //        eLog.SentBy = "System";
                    //                //        eLog.StudentId = candidatePaymentObj.CandidateID;
                    //                //        eLog.ToAddress = candidateEmail;
                    //                //        eLog.ToName = candidateName;
                    //                //        eLog.DateSent = DateTime.Now;
                    //                //        eLog.FromAddress = fromAddress;
                    //                //        eLog.FromName = senderName;
                    //                //        eLog.Attribute1 = "Failed";

                    //                //        LogWriter.EmailLog(eLog);
                    //                //    }

                    //                //    GetSendingInfo(candidatePaymentObj.CandidateID, value, candidatePaymentObj);
                    //                //}

                    //                #endregion

                    //            }
                    //            else
                    //            {
                    //                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code = PN01X002IE ?type=danger", false);
                    //            }
                    //        }//end using


                    //    }
                    //}
                    //else if (isMultiple == true) //if multiple purchase...multiple form purchase from MultipleApplication.aspx.
                    //{
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        DAL.CandidatePayment candidatePaymentObj = db.GetCandidatePaymentByID_AD(candidatePaymentID);
                    //        if (candidatePaymentObj != null)
                    //        {
                    //            List<DAL.CandidateFormSl> candFormSerialList = candidatePaymentObj.CandidateFormSls.ToList();
                    //            List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
                    //            List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();


                    //            //bool showBkashButton = true;
                    //            //if (candFormSerialList.Count() > 0)
                    //            //{
                    //            //    foreach (var item in candFormSerialList)
                    //            //    {
                    //            //        DAL.AdmissionSetup admSetupItem = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                    //            //        if (admSetupItem != null)
                    //            //        {
                    //            //            admSetupList.Add(admSetupItem);
                    //            //            if (admSetupItem.Attribute1 == null)
                    //            //            {
                    //            //                showBkashButton = false;
                    //            //            }
                    //            //        }
                    //            //    }
                    //            //}

                    //            //if (showBkashButton == false)
                    //            //{
                    //            //    btnSubmit_Bkash.Visible = false;
                    //            //}


                    //            if (admSetupList.Count() > 0)
                    //            {
                    //                foreach (var item in admSetupList)
                    //                {
                    //                    List<DAL.AdmissionUnit> admUnitListForEachAdmSetup = db.AdmissionDB.AdmissionUnits
                    //                        .Where(a => a.ID == item.AdmissionUnitID).ToList();
                    //                    if (admUnitListForEachAdmSetup.Count() > 0)
                    //                    {
                    //                        admUnitList.AddRange(admUnitListForEachAdmSetup);
                    //                    }
                    //                }
                    //            }

                    //            string admUnitProgramStr = string.Empty;
                    //            if (admUnitList.Count() > 0)
                    //            {
                    //                foreach (var item in admUnitList)
                    //                {
                    //                    admUnitProgramStr += item.UnitName + "<br />";
                    //                }
                    //            }


                    //            lblCandidateName.Text = candidatePaymentObj.BasicInfo.FirstName;
                    //            string candidateName = candidatePaymentObj.BasicInfo.FirstName;
                    //            string candidateEmail = candidatePaymentObj.BasicInfo.Email;
                    //            lblPaymentID.Text = candidatePaymentObj.PaymentId.ToString();
                    //            lblAmount.Text = candidatePaymentObj.Amount.ToString();
                    //            lblPrograms.Text = admUnitProgramStr;
                    //            //lblNote.Text = "Please note down your Payment ID and save this number for future reference. " +
                    //            //    "An email will be sent to <strong>" + candidateEmail + "</strong> containing your Payment ID.<br/>" +
                    //            //    "Using this Payment ID you can pay later.";
                    //            lblEmail.Text = candidateEmail;





                    //            #region N/A -- Email & SMS
                    //            //List<DAL.SPGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification_Result> result = null;
                    //            //using (var db1 = new OfficeDataManager())
                    //            //{
                    //            //    result = db1.AdmissionDB.SPGetCandidateSMSEmailCntByPmtIdMobPurchaseNotification(candidatePaymentObj.PaymentId, null).ToList();
                    //            //}

                    //            //if (result[0].Count_SMS > 0 && result[0].Count_Email > 0)
                    //            //{
                    //            //    //....Candidate already have Email and SMS. dont send again
                    //            //}
                    //            //else
                    //            //{
                    //            //    //....Candidate dont have Email and SMS. Sending First time Email and SMS

                    //            //    string mailBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + ", <br/><br/>" +
                    //            //        "Thank you for applying at Bangladesh University of Professionals (BUP). Please proceed for payment or you can pay later following the link : " +  //"Thank you for applying at Bangladesh University of Professionals (BUP). Please check the information below about your payment: " +
                    //            //        "<br/><br/>" +
                    //            //        "<b>Link : </b><a href='https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value + "'> https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=value </a>" +
                    //            //        "<strong style='color: Tomato;'>Payment ID: " + candidatePaymentObj.PaymentId.ToString() + "</strong>" +
                    //            //        "<br/><br/>" +
                    //            //        "Applied For :" + "<br/>" + admUnitProgramStr + ". <br/>" +
                    //            //        "<br/>" +
                    //            //        "</p>" +
                    //            //        "<p style='color: green;'>Important : You must login, fill up and submit the Application Form after successful Payment.</p>" +
                    //            //        "<p>" +
                    //            //        "<br/>" +
                    //            //        "Regards," +
                    //            //        "<br/>" +
                    //            //        "Bangladesh University of Professionals (BUP)" +
                    //            //        "</p>";
                    //            //    //"Regards, <br/>" ICT Centre,
                    //            //    //"Admin" +
                    //            //    //"<br/>" +
                    //            //    string fromAddress = "no-reply-2@bup.edu.bd";
                    //            //    string senderName = "BUP Admission";
                    //            //    string subject = "Payment ID for Your Application";

                    //            //    //bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);
                    //            //    bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);

                    //            //    if (isSentEmail == true)
                    //            //    {
                    //            //        DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    //            //        eLog.MessageBody = mailBody;
                    //            //        eLog.MessageSubject = subject;
                    //            //        eLog.Page = "PurchaseNotification.aspx";
                    //            //        eLog.SentBy = "System";
                    //            //        eLog.StudentId = candidatePaymentObj.CandidateID;
                    //            //        eLog.ToAddress = candidateEmail;
                    //            //        eLog.ToName = candidateName;
                    //            //        eLog.DateSent = DateTime.Now;
                    //            //        eLog.FromAddress = fromAddress;
                    //            //        eLog.FromName = senderName;
                    //            //        eLog.Attribute1 = "Success";

                    //            //        LogWriter.EmailLog(eLog);
                    //            //    }
                    //            //    else if (isSentEmail == false)
                    //            //    {
                    //            //        DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                    //            //        eLog.MessageBody = mailBody;
                    //            //        eLog.MessageSubject = subject;
                    //            //        eLog.Page = "PurchaseNotification.aspx";
                    //            //        eLog.SentBy = "System";
                    //            //        eLog.StudentId = candidatePaymentObj.CandidateID;
                    //            //        eLog.ToAddress = candidateEmail;
                    //            //        eLog.ToName = candidateName;
                    //            //        eLog.DateSent = DateTime.Now;
                    //            //        eLog.FromAddress = fromAddress;
                    //            //        eLog.FromName = senderName;
                    //            //        eLog.Attribute1 = "Failed";

                    //            //        LogWriter.EmailLog(eLog);
                    //            //    }

                    //            //    GetSendingInfo(candidatePaymentObj.CandidateID, value, candidatePaymentObj);
                    //            //}

                    //            #endregion

                    //        }
                    //        else
                    //        {
                    //            Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code = PN01X002IE ?type=danger", false);
                    //        }
                    //    }//end using
                    //}//end if-else isMultiple
                }
            }
        }

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        //payment submit for bkash
        protected void btnSubmit_Bkash_Click(object sender, EventArgs e)
        {
            bool? isMultiple = null;

            string value = Request.QueryString["value"].ToString();
            string[] values = value.Split(';');
            long candidateId = Convert.ToInt64(values[0]);
            long candidatePaymentID = Convert.ToInt64(values[1]);
            long? candidateFormSerialID = Convert.ToInt64(values[2]);
            if (values[3] == "0")
            {
                isMultiple = false;
            }
            else if (values[3] == "1")
            {
                isMultiple = true;
            }

            SessionSGD.SaveObjToSession<string>("PurchaseNotification", SessionName.Common_PreviousPage);
            Response.Redirect("~/Admission/Candidate/PurchaseByBkash.aspx?referenceNo=" + candidatePaymentID, false);
        }

        //payment submit for ssl/foster
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            //CheckEkPay();


            //DAL.BasicInfo candidate = null;
            //DAL.CandidateFormSl cFormSl = null;
            //DAL.CandidatePayment cPayment = null;
            //DAL.Store store = null;
            //DAL.StoreFoster storeFoster = null;
            //DAL.StoreEkPay storeEkPay = null;
            //DAL.AdmissionSetup admSetup = null;
            //List<DAL.CandidateFormSl> cFormSlMultiple = null;
            //List<DAL.AdmissionSetup> admSetupMultiple = null;

            if (string.IsNullOrEmpty(Request.QueryString["value"]))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator." +
                    "Note : Please do not make any payment without contacting the administrator.[Error Code = PN01X001PL]?type=danger", false);
            }
            else //if url value is present.
            {
                //TODO decrypt url parameter
                //string value = Decrypt.DecryptString(this.Request.QueryString["value"].ToString());
                //string[] values = value.Split(';');
                string value = Request.QueryString["value"].ToString();
                string[] values = value.Split(';');

                bool? isMultiple = null;

                long candidateId = Convert.ToInt64(values[0]);
                long candidatePaymentID = Convert.ToInt64(values[1]);
                long candidateFormSerialID = Convert.ToInt64(values[2]);
                if (values[3] == "0")
                {
                    isMultiple = false;
                }
                else if (values[3] == "1")
                {
                    isMultiple = true;
                }
                string admUnitName = values[4].ToString();
                int educationCategoryId = Convert.ToInt32(values[5]);



                DAL.BasicInfo candidate = null;
                DAL.CandidateFormSl cFormSl = null;
                DAL.CandidatePayment cPayment = null;
                DAL.Store store = null;
                DAL.StoreFoster storeFoster = null;
                DAL.StoreEkPay storeEkPay = null;
                DAL.AdmissionSetup admSetup = null;
                List<DAL.CandidateFormSl> cFormSlMultiple = null;
                List<DAL.AdmissionSetup> admSetupMultiple = null;

                string successUrlStr = null;
                string failUrlStr = null;
                string cancelUrlStr = null;
                string ipnUrlStr = "https://admission.bup.edu.bd/Admission/IPNListener";


                try
                {
                    if (candidatePaymentID > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                            candidate = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cPayment.CandidateID).FirstOrDefault();
                            cFormSl = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cPayment.CandidateID && x.AcaCalID == cPayment.AcaCalID).FirstOrDefault();
                            admSetup = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == cFormSl.AdmissionSetupID && x.AcaCalID == cPayment.AcaCalID && x.IsActive == true).FirstOrDefault();
                        }

                        if (admSetup.Attribute2 == "SSL")
                        {
                            using (var db = new OfficeDataManager())
                            {
                                store = db.AdmissionDB.Stores.Where(x => x.ID == admSetup.StoreID && x.IsActive == true).FirstOrDefault(); //db.GetActiveSSLStoreForMultiplepurchase(true, true);
                            }

                            #region SSL Gateway
                            if (cPayment != null && candidate != null && store != null)
                            {
                                successUrlStr = store.SuccessUrl;
                                failUrlStr = store.FailedUrl;
                                cancelUrlStr = store.CancelledUrl;

                                if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                                {
                                    //{"version", "3.00"},
                                    string storeId = Decrypt.DecryptString(store.StoreId);
                                    string password = Decrypt.DecryptString(store.StorePass);

                                    NameValueCollection PostData = new NameValueCollection {
                                                {"store_id", storeId},
                                                {"store_passwd", password},
                                                {"total_amount", cPayment.Amount.ToString()},
                                                {"currency", "BDT"},
                                                {"tran_id", cPayment.PaymentId.ToString()},
                                                {"success_url", successUrlStr},
                                                {"fail_url", failUrlStr},
                                                {"cancel_url", cancelUrlStr},
                                                {"ipn_url", ipnUrlStr},
                                                {"cus_name", candidate.FirstName},
                                                {"cus_email", candidate.Email},
                                                {"cus_phone", candidate.SMSPhone},
                                                {"value_a", candidate.ID.ToString()},
                                                {"value_b", candidate.FirstName},
                                                {"value_c", cPayment.Amount.ToString()},
                                                {"value_d", store.ID.ToString()} };



                                    if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                                    {
                                        SSLCommerz sslcz = new SSLCommerz(storeId, password);
                                        String response = sslcz.InitiateTransaction(PostData);
                                        Response.Redirect(response);
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (admSetup.Attribute2 == "FPG")
                        {

                            using (var db = new OfficeDataManager())
                            {
                                storeFoster = db.GetFPGStoreActiveMultiplePurchaseStore(true, true);
                            }

                            #region Foster Gateway
                            if (cPayment != null && candidate != null && storeFoster != null)
                            {
                                string secretKey = storeFoster.SecurityKey;
                                string mcnt_AccessCode = storeFoster.AccessCode;
                                string mcnt_TxnNo = storeFoster.MerchantShortName + cPayment.PaymentId.ToString();
                                string mcnt_ShortName = storeFoster.MerchantShortName;
                                string mcnt_OrderNo = cPayment.PaymentId.ToString();
                                string mcnt_ShopId = storeFoster.ShopId;
                                string mcnt_Amount = cPayment.Amount.ToString();
                                string mcnt_Currency = "BDT";

                                string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                                    mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey);

                                NameValueCollection urlParam = new NameValueCollection
                                                {
                                                    {"mcnt_TxnNo", mcnt_TxnNo},
                                                    {"mcnt_ShortName", mcnt_ShortName},
                                                    {"mcnt_OrderNo", mcnt_OrderNo},
                                                    {"mcnt_ShopId", mcnt_ShopId},
                                                    {"mcnt_Amount", mcnt_Amount},
                                                    {"mcnt_Currency", mcnt_Currency},
                                                    {"cust_InvoiceTo", candidate.FirstName},
                                                    {"cust_CustomerServiceName", "BUP Admission"},
                                                    {"cust_CustomerName", candidate.FirstName},
                                                    {"cust_CustomerEmail", candidate.Email},
                                                    {"cust_CustomerAddress", "Dhaka"},
                                                    {"cust_CustomerContact", candidate.SMSPhone },
                                                    {"cust_CustomerGender", candidate.GenderID.ToString()},
                                                    {"cust_CustomerCity", "Dhaka"},
                                                    {"cust_CustomerState", "Dhaka"},
                                                    {"cust_CustomerPostcode", "1216"},
                                                    {"cust_CustomerCountry", "Bangladesh"},
                                                    {"cust_Billingaddress", "Bangladesh"},
                                                    {"cust_ShippingAddress", "Bangladesh"},
                                                    {"cust_orderitems", cPayment.PaymentId.ToString()},
                                                    {"success_url", storeFoster.SuccessUrl},
                                                    {"cancel_url", storeFoster.CancelUrl},
                                                    {"fail_url", storeFoster.FailUrl},
                                                    {"merchentdomainname", "admission.bup.edu.bd"},
                                                    {"merchentip", "202.79.20.181"},
                                                    {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                                                };

                                string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                                string response = storeFoster.Uri + queryString;
                                Response.Redirect(response);
                            }
                            #endregion
                        }
                        else if (admSetup.Attribute2 == "EkPay")
                        {
                            using (var db = new OfficeDataManager())
                            {
                                storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.ID == admSetup.StoreID && x.IsActive == true).FirstOrDefault();
                            }

                            #region EkPay Gateway
                            if (cPayment != null && candidate != null && storeEkPay != null)
                            {
                                EkPayPaymentGateway(storeEkPay, cPayment, candidate);
                            }
                            #endregion

                        }//end if-else
                        else
                        {
                            MessageView("Failed to get Payment Gateway Info!", "fail");
                        }
                    }
                    else
                    {
                        MessageView("Invalid Request!", "fail");
                    }


                }
                catch (Exception ex)
                {
                    MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
                }









                #region N/A
                //string successUrlStr = null;
                //string failUrlStr = null;
                //string cancelUrlStr = null;
                //string ipnUrlStr = "https://admission.bup.edu.bd/Admission/IPNListener";

                //#region isMultiple False
                //if (isMultiple == false)//not a multiple purchase, so it can be for undergrad or masters.
                //{

                //    if (candidateId > 0 && candidatePaymentID > 0 && candidateFormSerialID > 0)
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                //            cFormSl = db.AdmissionDB.CandidateFormSls.Find(candidateFormSerialID);
                //            cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                //            admSetup = db.AdmissionDB.AdmissionSetups.Find(cFormSl.AdmissionSetupID);

                //            if (admSetup != null)
                //            {
                //                if (admSetup.Attribute2 == "SSL")
                //                {
                //                    store = db.AdmissionDB.Stores.Find(admSetup.StoreID);
                //                    storeFoster = null;
                //                }
                //                else if (admSetup.Attribute2 == "FPG")
                //                {
                //                    store = null;
                //                    storeFoster = db.AdmissionDB.StoreFosters
                //                        .Where(c => c.ID == admSetup.StoreID && c.IsActive == true)
                //                        .FirstOrDefault();
                //                }
                //            }
                //        }
                //    }

                //    if (educationCategoryId == 4) //if bachelors
                //    {
                //        #region Bachelor
                //        if (candidate != null && cFormSl != null && cPayment != null && admSetup != null)
                //        {
                //            if (store != null && storeFoster == null) //if SSL
                //            {
                //                #region SSL Gateway
                //                successUrlStr = store.SuccessUrl;
                //                failUrlStr = store.FailedUrl;
                //                cancelUrlStr = store.CancelledUrl;

                //                if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                //                {
                //                    if (store.IsMultipleAllowed == true)
                //                    {
                //                        string storeId = Decrypt.DecryptString(store.StoreId);
                //                        string password = Decrypt.DecryptString(store.StorePass);

                //                        //{"version", "3.00"},
                //                        NameValueCollection PostData = new NameValueCollection {
                //                            {"store_id", storeId},
                //                            {"store_passwd", password},
                //                            {"total_amount", cPayment.Amount.ToString()},
                //                            {"currency", "BDT"},
                //                            {"tran_id", cPayment.PaymentId.ToString()},
                //                            {"success_url", successUrlStr},
                //                            {"fail_url", failUrlStr },
                //                            {"cancel_url", cancelUrlStr},
                //                            {"ipn_url", ipnUrlStr},
                //                            {"cus_name", candidate.FirstName},
                //                            {"cus_email", candidate.Email},
                //                            {"cus_phone", candidate.SMSPhone},
                //                            {"value_a", candidate.ID.ToString()},
                //                            {"value_b", candidate.FirstName},
                //                            {"value_c", cPayment.Amount.ToString()},
                //                            { "value_d", store.ID.ToString()}
                //                        };


                //                        if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                //                        {
                //                            SSLCommerz sslcz = new SSLCommerz(storeId, password);
                //                            String response = sslcz.InitiateTransaction(PostData);
                //                            Response.Redirect(response);
                //                        }
                //                    }
                //                }
                //                #endregion
                //            }
                //            else if (store == null && storeFoster != null) //if foster
                //            {
                //                #region Foster Gateway
                //                if (storeFoster.IsMultipleAllowed == true)
                //                {
                //                    string secretKey = storeFoster.SecurityKey;
                //                    string mcnt_AccessCode = storeFoster.AccessCode;
                //                    string mcnt_TxnNo = storeFoster.MerchantShortName + cPayment.PaymentId.ToString();
                //                    string mcnt_ShortName = storeFoster.MerchantShortName;
                //                    string mcnt_OrderNo = cPayment.PaymentId.ToString();
                //                    string mcnt_ShopId = storeFoster.ShopId;
                //                    string mcnt_Amount = cPayment.Amount.ToString();
                //                    string mcnt_Currency = "BDT";

                //                    string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                //                        mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey);

                //                    NameValueCollection urlParam = new NameValueCollection
                //                    {
                //                        {"mcnt_TxnNo", mcnt_TxnNo},
                //                        {"mcnt_ShortName", mcnt_ShortName},
                //                        {"mcnt_OrderNo", mcnt_OrderNo},
                //                        {"mcnt_ShopId", mcnt_ShopId},
                //                        {"mcnt_Amount", mcnt_Amount},
                //                        {"mcnt_Currency", mcnt_Currency},
                //                        {"cust_InvoiceTo", candidate.FirstName},
                //                        {"cust_CustomerServiceName", "BUP Admission"},
                //                        {"cust_CustomerName", candidate.FirstName},
                //                        {"cust_CustomerEmail", candidate.Email},
                //                        {"cust_CustomerAddress", "Dhaka"},
                //                        {"cust_CustomerContact", candidate.SMSPhone },
                //                        {"cust_CustomerGender", candidate.GenderID.ToString()},
                //                        {"cust_CustomerCity", "Dhaka"},
                //                        {"cust_CustomerState", "Dhaka"},
                //                        {"cust_CustomerPostcode", "1216"},
                //                        {"cust_CustomerCountry", "Bangladesh"},
                //                        {"cust_Billingaddress", "Bangladesh"},
                //                        {"cust_ShippingAddress", "Bangladesh"},
                //                        {"cust_orderitems", cPayment.PaymentId.ToString()},
                //                        {"success_url", storeFoster.SuccessUrl},
                //                        {"cancel_url", storeFoster.CancelUrl},
                //                        {"fail_url", storeFoster.FailUrl},
                //                        {"merchentdomainname", "admission.bup.edu.bd"},
                //                        {"merchentip", "202.79.20.181"},
                //                        {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                //                    };

                //                    string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                //                    string response = storeFoster.Uri + queryString;
                //                    Response.Redirect(response);
                //                }
                //                #endregion
                //            }
                //        }
                //        #endregion
                //    }
                //    else if (educationCategoryId == 6) //if masters
                //    {
                //        #region Masters
                //        if (store != null && storeFoster == null) //if ssl
                //        {
                //            #region SSL Gateway
                //            successUrlStr = store.SuccessUrl;
                //            failUrlStr = store.FailedUrl;
                //            cancelUrlStr = store.CancelledUrl;

                //            if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                //            {
                //                if (store.IsMultipleAllowed == false)
                //                {

                //                    #region N/A -- Old Process
                //                    //NameValueCollection PostData = new NameValueCollection
                //                    //{
                //                    //    {"total_amount", cPayment.Amount.ToString()},
                //                    //    {"tran_id", cPayment.PaymentId.ToString()},
                //                    //    {"success_url", store.SuccessUrl},
                //                    //    {"fail_url", store.FailedUrl},
                //                    //    {"cancel_url", store.CancelledUrl},
                //                    //    {"version", "3.00"},
                //                    //    {"cus_name", candidate.FirstName},
                //                    //    {"cus_email", candidate.Email},
                //                    //    {"cus_phone", candidate.SMSPhone},
                //                    //    {"value_a", candidate.ID.ToString()},
                //                    //    {"value_b", candidate.FirstName},
                //                    //    {"value_c", cPayment.Amount.ToString()},
                //                    //    {"value_d", store.ID.ToString()}
                //                    //};

                //                    //string storeId = Decrypt.DecryptString(store.StoreId);
                //                    //string password = Decrypt.DecryptString(store.StorePass); 
                //                    #endregion


                //                    string storeId = Decrypt.DecryptString(store.StoreId);
                //                    string password = Decrypt.DecryptString(store.StorePass);

                //                    NameValueCollection PostData = new NameValueCollection {
                //                        {"store_id", storeId},
                //                        {"store_passwd", password},
                //                        {"total_amount", cPayment.Amount.ToString()},
                //                        {"currency", "BDT"},
                //                        {"tran_id", cPayment.PaymentId.ToString()},
                //                        {"success_url", successUrlStr},
                //                        {"fail_url", failUrlStr },
                //                        {"cancel_url", cancelUrlStr},
                //                        {"ipn_url", ipnUrlStr},
                //                        {"cus_name", candidate.FirstName},
                //                        {"cus_email", candidate.Email},
                //                        {"cus_phone", candidate.SMSPhone},
                //                        {"value_a", candidate.ID.ToString()},
                //                        {"value_b", candidate.FirstName},
                //                        {"value_c", cPayment.Amount.ToString()},
                //                        { "value_d", store.ID.ToString()}
                //                    };


                //                    if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                //                    {
                //                        SSLCommerz sslcz = new SSLCommerz(storeId, password);
                //                        String response = sslcz.InitiateTransaction(PostData);
                //                        Response.Redirect(response);
                //                    }
                //                }
                //            }
                //            #endregion
                //        }
                //        else if (store == null && storeFoster != null) // if fpg
                //        {
                //            #region Foster Gateway
                //            if (storeFoster.IsMultipleAllowed == true)  //(false) was set because MSC cand buy multiple form. but for now its set true
                //            {
                //                string secretKey = storeFoster.SecurityKey;
                //                string mcnt_AccessCode = storeFoster.AccessCode;
                //                string mcnt_TxnNo = storeFoster.MerchantShortName + cPayment.PaymentId.ToString();
                //                string mcnt_ShortName = storeFoster.MerchantShortName;
                //                string mcnt_OrderNo = cPayment.PaymentId.ToString();
                //                string mcnt_ShopId = storeFoster.ShopId;
                //                string mcnt_Amount = cPayment.Amount.ToString();
                //                string mcnt_Currency = "BDT";

                //                string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                //                    mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower();

                //                NameValueCollection urlParam = new NameValueCollection
                //                    {
                //                        {"mcnt_TxnNo", mcnt_TxnNo},
                //                        {"mcnt_ShortName", mcnt_ShortName},
                //                        {"mcnt_OrderNo", mcnt_OrderNo},
                //                        {"mcnt_ShopId", mcnt_ShopId},
                //                        {"mcnt_Amount", mcnt_Amount},
                //                        {"mcnt_Currency", mcnt_Currency},
                //                        {"cust_InvoiceTo", candidate.FirstName},
                //                        {"cust_CustomerServiceName", "BUP Admission"},
                //                        {"cust_CustomerName", candidate.FirstName},
                //                        {"cust_CustomerEmail", candidate.Email},
                //                        {"cust_CustomerAddress", "Dhaka"},
                //                        {"cust_CustomerContact", candidate.SMSPhone },
                //                        {"cust_CustomerGender", candidate.GenderID.ToString()},
                //                        {"cust_CustomerCity", "Dhaka"},
                //                        {"cust_CustomerState", "Dhaka"},
                //                        {"cust_CustomerPostcode", "1216"},
                //                        {"cust_CustomerCountry", "Bangladesh"},
                //                        {"cust_Billingaddress", "Bangladesh"},
                //                        {"cust_ShippingAddress", "Bangladesh"},
                //                        {"cust_orderitems", cPayment.PaymentId.ToString()},
                //                        {"success_url", storeFoster.SuccessUrl},
                //                        {"cancel_url", storeFoster.CancelUrl},
                //                        {"fail_url", storeFoster.FailUrl},
                //                        {"merchentdomainname", "admission.bup.edu.bd"},
                //                        {"merchentip", "202.79.20.181"},
                //                        {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                //                    };

                //                string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                //                string response = storeFoster.Uri + queryString;
                //                Response.Redirect(response);
                //            }
                //            else if (storeFoster.IsMultipleAllowed == false) //---for those who IsMultiple false set in AdmissionSetup
                //            {
                //                string secretKey = storeFoster.SecurityKey;
                //                string mcnt_AccessCode = storeFoster.AccessCode;
                //                string mcnt_TxnNo = storeFoster.MerchantShortName + cPayment.PaymentId.ToString();
                //                string mcnt_ShortName = storeFoster.MerchantShortName;
                //                string mcnt_OrderNo = cPayment.PaymentId.ToString();
                //                string mcnt_ShopId = storeFoster.ShopId;
                //                string mcnt_Amount = cPayment.Amount.ToString();
                //                string mcnt_Currency = "BDT";

                //                string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                //                    mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower();

                //                NameValueCollection urlParam = new NameValueCollection
                //                    {
                //                        {"mcnt_TxnNo", mcnt_TxnNo},
                //                        {"mcnt_ShortName", mcnt_ShortName},
                //                        {"mcnt_OrderNo", mcnt_OrderNo},
                //                        {"mcnt_ShopId", mcnt_ShopId},
                //                        {"mcnt_Amount", mcnt_Amount},
                //                        {"mcnt_Currency", mcnt_Currency},
                //                        {"cust_InvoiceTo", candidate.FirstName},
                //                        {"cust_CustomerServiceName", "BUP Admission"},
                //                        {"cust_CustomerName", candidate.FirstName},
                //                        {"cust_CustomerEmail", candidate.Email},
                //                        {"cust_CustomerAddress", "Dhaka"},
                //                        {"cust_CustomerContact", candidate.SMSPhone },
                //                        {"cust_CustomerGender", candidate.GenderID.ToString()},
                //                        {"cust_CustomerCity", "Dhaka"},
                //                        {"cust_CustomerState", "Dhaka"},
                //                        {"cust_CustomerPostcode", "1216"},
                //                        {"cust_CustomerCountry", "Bangladesh"},
                //                        {"cust_Billingaddress", "Bangladesh"},
                //                        {"cust_ShippingAddress", "Bangladesh"},
                //                        {"cust_orderitems", cPayment.PaymentId.ToString()},
                //                        {"success_url", storeFoster.SuccessUrl},
                //                        {"cancel_url", storeFoster.CancelUrl},
                //                        {"fail_url", storeFoster.FailUrl},
                //                        {"merchentdomainname", "admission.bup.edu.bd"},
                //                        {"merchentip", "202.79.20.181"},
                //                        {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                //                    };

                //                string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                //                string response = storeFoster.Uri + queryString;
                //                Response.Redirect(response);
                //            }
                //            #endregion
                //        }
                //        #endregion
                //    }

                //}
                //#endregion
                ////-----------------------------------------
                //#region isMultiple true
                //else if (isMultiple == true)
                //{
                //    DAL.Store sslStoreMultiple = null;
                //    DAL.StoreFoster fosterStoreMultiple = null;


                //    if (candidateId > 0 && candidatePaymentID > 0)
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                //            cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                //            cFormSlMultiple = db.AdmissionDB.CandidateFormSls.Where(c => c.CandidatePaymentID == candidatePaymentID).ToList();
                //        }
                //    }

                //    admSetupMultiple = new List<DAL.AdmissionSetup>();
                //    if (cFormSlMultiple != null && cFormSlMultiple.Count > 0)
                //    {
                //        foreach (var cform in cFormSlMultiple)
                //        {
                //            DAL.AdmissionSetup tempAdmSetup = null;
                //            using (var db = new OfficeDataManager())
                //            {
                //                tempAdmSetup = db.AdmissionDB.AdmissionSetups.Find(cform.AdmissionSetupID);
                //            }
                //            if (tempAdmSetup != null)
                //            {
                //                admSetupMultiple.Add(tempAdmSetup);
                //            }
                //        }
                //    }

                //    if (admSetupMultiple != null && admSetupMultiple.Count > 0)
                //    {
                //        if (admSetupMultiple.Where(c => c.Attribute2 == "SSL").ToList().Count > 0)
                //        {
                //            using (var db = new OfficeDataManager())
                //            {
                //                sslStoreMultiple = db.GetActiveSSLStoreForMultiplepurchase(true, true);
                //            }
                //        }
                //        else if (admSetupMultiple.Where(c => c.Attribute2 == "FPG").ToList().Count > 0)
                //        {
                //            using (var db = new OfficeDataManager())
                //            {
                //                fosterStoreMultiple = db.GetFPGStoreActiveMultiplePurchaseStore(true, true);
                //            }
                //        }
                //        else if (admSetupMultiple.Where(c => c.Attribute2 == "EkPay").ToList().Count > 0)
                //        {
                //            using (var db = new OfficeDataManager())
                //            {
                //                storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.IsActive == true).FirstOrDefault();
                //            }
                //        }//end if-else
                //    }//end if admSetupMultiple != null

                //    if (sslStoreMultiple != null && fosterStoreMultiple == null) //if ssl
                //    {
                //        #region SSL Gateway
                //        if (cPayment != null && sslStoreMultiple != null && candidate != null)
                //        {
                //            successUrlStr = sslStoreMultiple.SuccessUrl;
                //            failUrlStr = sslStoreMultiple.FailedUrl;
                //            cancelUrlStr = sslStoreMultiple.CancelledUrl;

                //            if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                //            {
                //                if (sslStoreMultiple.IsMultipleAllowed == true)
                //                {
                //                    //{"version", "3.00"},
                //                    string storeId = Decrypt.DecryptString(sslStoreMultiple.StoreId);
                //                    string password = Decrypt.DecryptString(sslStoreMultiple.StorePass);

                //                    NameValueCollection PostData = new NameValueCollection {
                //                    {"store_id", storeId},
                //                    {"store_passwd", password},
                //                    {"total_amount", cPayment.Amount.ToString()},
                //                    {"currency", "BDT"},
                //                    {"tran_id", cPayment.PaymentId.ToString()},
                //                    {"success_url", sslStoreMultiple.SuccessUrl},
                //                    {"fail_url", sslStoreMultiple.FailedUrl},
                //                    {"cancel_url", sslStoreMultiple.CancelledUrl},
                //                    {"ipn_url", ipnUrlStr},
                //                    {"cus_name", candidate.FirstName},
                //                    {"cus_email", candidate.Email},
                //                    {"cus_phone", candidate.SMSPhone},
                //                    {"value_a", candidate.ID.ToString()},
                //                    {"value_b", candidate.FirstName},
                //                    {"value_c", cPayment.Amount.ToString()},
                //                    {"value_d", sslStoreMultiple.ID.ToString()} };



                //                    if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                //                    {
                //                        SSLCommerz sslcz = new SSLCommerz(storeId, password);
                //                        String response = sslcz.InitiateTransaction(PostData);
                //                        Response.Redirect(response);
                //                    }
                //                }
                //            }
                //        }
                //        #endregion
                //    }
                //    else if (sslStoreMultiple == null && fosterStoreMultiple != null) //if fpg
                //    {
                //        #region Foster Gateway
                //        if (cPayment != null && fosterStoreMultiple != null && candidate != null)
                //        {
                //            if (fosterStoreMultiple.IsMultipleAllowed == true)
                //            {
                //                string secretKey = fosterStoreMultiple.SecurityKey;
                //                string mcnt_AccessCode = fosterStoreMultiple.AccessCode;
                //                string mcnt_TxnNo = fosterStoreMultiple.MerchantShortName + cPayment.PaymentId.ToString();
                //                string mcnt_ShortName = fosterStoreMultiple.MerchantShortName;
                //                string mcnt_OrderNo = cPayment.PaymentId.ToString();
                //                string mcnt_ShopId = fosterStoreMultiple.ShopId;
                //                string mcnt_Amount = cPayment.Amount.ToString();
                //                string mcnt_Currency = "BDT";

                //                string urlParamHashed = FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName,
                //                    mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey);

                //                NameValueCollection urlParam = new NameValueCollection
                //                    {
                //                        {"mcnt_TxnNo", mcnt_TxnNo},
                //                        {"mcnt_ShortName", mcnt_ShortName},
                //                        {"mcnt_OrderNo", mcnt_OrderNo},
                //                        {"mcnt_ShopId", mcnt_ShopId},
                //                        {"mcnt_Amount", mcnt_Amount},
                //                        {"mcnt_Currency", mcnt_Currency},
                //                        {"cust_InvoiceTo", candidate.FirstName},
                //                        {"cust_CustomerServiceName", "BUP Admission"},
                //                        {"cust_CustomerName", candidate.FirstName},
                //                        {"cust_CustomerEmail", candidate.Email},
                //                        {"cust_CustomerAddress", "Dhaka"},
                //                        {"cust_CustomerContact", candidate.SMSPhone },
                //                        {"cust_CustomerGender", candidate.GenderID.ToString()},
                //                        {"cust_CustomerCity", "Dhaka"},
                //                        {"cust_CustomerState", "Dhaka"},
                //                        {"cust_CustomerPostcode", "1216"},
                //                        {"cust_CustomerCountry", "Bangladesh"},
                //                        {"cust_Billingaddress", "Bangladesh"},
                //                        {"cust_ShippingAddress", "Bangladesh"},
                //                        {"cust_orderitems", cPayment.PaymentId.ToString()},
                //                        {"success_url", fosterStoreMultiple.SuccessUrl},
                //                        {"cancel_url", fosterStoreMultiple.CancelUrl},
                //                        {"fail_url", fosterStoreMultiple.FailUrl},
                //                        {"merchentdomainname", "admission.bup.edu.bd"},
                //                        {"merchentip", "202.79.20.181"},
                //                        {"mcnt_SecureHashValue", FosterPaymentGateway.GenerateHashValue(mcnt_AccessCode, mcnt_TxnNo, mcnt_ShortName, mcnt_OrderNo, mcnt_ShopId, mcnt_Amount, mcnt_Currency, secretKey).ToLower()}
                //                    };

                //                string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
                //                string response = fosterStoreMultiple.Uri + queryString;
                //                Response.Redirect(response);
                //            }
                //        }
                //        #endregion
                //    }
                //    else if (storeEkPay != null && storeEkPay == null) //if EkPay
                //    {
                //        #region EkPay Gateway
                //        CheckEkPay(storeEkPay, cPayment, candidate);
                //        #endregion
                //    }
                //    else
                //    {

                //    }


                //    #region N/A
                //    //if (candidatePaymentID > 0 && educationCategoryId == 4)//for bachelors only.
                //    //{
                //    //    using (var db = new CandidateDataManager())
                //    //    {
                //    //        cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                //    //        store = db.AdmissionDB.Stores.Where(c => c.IsMultipleAllowed == true).FirstOrDefault();
                //    //    }

                //    //    if (cPayment != null)
                //    //    {
                //    //        using (var db = new CandidateDataManager())
                //    //        {
                //    //            candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                //    //        }
                //    //    }
                //    //}

                //    //if (cPayment != null && store != null && candidate != null)
                //    //{
                //    //    successUrlStr = store.SuccessUrl;
                //    //    failUrlStr = store.FailedUrl;
                //    //    cancelUrlStr = store.CancelledUrl;

                //    //    if (successUrlStr != null && failUrlStr != null && cancelUrlStr != null)
                //    //    {
                //    //        if (store.IsMultipleAllowed == true)
                //    //        {

                //    //            NameValueCollection PostData = new NameValueCollection {
                //    //            {"total_amount", cPayment.Amount.ToString()},
                //    //            {"tran_id", cPayment.PaymentId.ToString()},
                //    //            {"success_url", store.SuccessUrl},
                //    //            {"fail_url", store.FailedUrl},
                //    //            {"cancel_url", store.CancelledUrl},
                //    //            {"version", "3.00"},
                //    //            {"cus_name", candidate.FirstName},
                //    //            {"cus_email", candidate.Email},
                //    //            {"cus_phone", candidate.SMSPhone},
                //    //            {"value_a", candidate.ID.ToString()},
                //    //            {"value_b", candidate.FirstName},
                //    //            {"value_c", cPayment.Amount.ToString()},
                //    //            {"value_d", store.ID.ToString()} };

                //    //            string storeId = Decrypt.DecryptString(store.StoreId);
                //    //            string password = Decrypt.DecryptString(store.StorePass);

                //    //            if (!string.IsNullOrEmpty(storeId) && !string.IsNullOrEmpty(password))
                //    //            {
                //    //                SSLCommerz sslcz = new SSLCommerz(storeId, password);
                //    //                String response = sslcz.InitiateTransaction(PostData);
                //    //                Response.Redirect(response);
                //    //            }
                //    //        }
                //    //    }
                //    //} 
                //    #endregion

                //} //end if-else isMultiple
                //#endregion 
                #endregion

            }//end if-else check url param
        }

        protected void btnSubmitPayLater_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://admission.bup.edu.bd/Admission/Home", true);
        }


        #region N/A -- Send SMS
        //private void GetSendingInfo(long? candidateId, string value, DAL.CandidatePayment candidatePaymentObj)
        //{
        //    if (candidateId != null)
        //    {
        //        if (candidateId > 0)
        //        {
        //            DAL.BasicInfo candidate = null;
        //            DAL.CandidateUser candidateUser = null;

        //            string candidateUsername = null;
        //            string candidatePassword = null;
        //            string candidateSmsPhone = null;
        //            string candidateEmail = null;

        //            using (var db = new CandidateDataManager())
        //            {
        //                candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
        //            }

        //            if (candidate != null)
        //            {
        //                candidateEmail = candidate.Email;
        //                candidateSmsPhone = candidate.SMSPhone;
        //                using (var db = new CandidateDataManager())
        //                {
        //                    candidateUser = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
        //                }
        //            }

        //            if (candidateUser != null)
        //            {
        //                candidateUsername = candidateUser.UsernameLoginId;
        //                candidatePassword = candidateUser.Password;
        //            }

        //            if (!string.IsNullOrEmpty(candidateUsername) && !string.IsNullOrEmpty(candidatePassword) &&
        //                !string.IsNullOrEmpty(candidateSmsPhone) && !string.IsNullOrEmpty(candidateEmail))
        //            {
        //                SendSms(candidateSmsPhone, candidate.ID, value, candidatePaymentObj);
        //                //SendEmail(candidateEmail, candidateUsername, candidatePassword, candidate.ID);
        //            }
        //        }
        //    }
        //}


        //#region N/A -- Sending SMS
        ////private static string userName = "bup789";
        ////private static string password = "01769021586";
        ////private static string sender = "BUP";

        ////public static string Send(string phoneNo, string message)
        ////{

        ////    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://app.planetgroupbd.com/api/v3/sendsms/plain?user="
        ////        + userName + "&password=" + password + "&sender=BUP"
        ////        + "&SMSText=" + System.Web.HttpUtility.UrlEncode(message) + "&GSM=" + phoneNo + "&type=longSMS");

        ////    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
        ////    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
        ////    string responseString = respStreamReader.ReadToEnd();
        ////    respStreamReader.Close();
        ////    myResp.Close();
        ////    return responseString;
        ////}
        //#endregion

        //private void SendSms(string smsPhone, long candidateId, string value, DAL.CandidatePayment candidatePaymentObj)
        //{
        //    if (!string.IsNullOrEmpty(smsPhone) && smsPhone.Count() == 14 && smsPhone.Contains("+"))
        //    {
        //        string messageBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + "," + "\n" +
        //                            "Payment ID : " + candidatePaymentObj.PaymentId.ToString() + "\n" +
        //                            "and Payment Link : https://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value + "\n" +
        //                            "BUP";

        //        string stringData = SMSUtility.Send(smsPhone, messageBody);

        //        string statusT = JObject.Parse(stringData)["statusCode"].ToString();

        //        if (statusT != "200") //if sms sending fails
        //        {
        //            DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
        //            smsLog.AcaCalId = null;
        //            smsLog.Attribute1 = "Sms sending failed in PurchaseNotification.aspx";
        //            smsLog.Attribute2 = "Failed";
        //            smsLog.Attribute3 = null;
        //            smsLog.CreatedBy = candidateId;
        //            smsLog.CreatedDate = DateTime.Now;
        //            smsLog.CurrentSMSReferenceNo = stringData;
        //            smsLog.Message = messageBody;
        //            smsLog.StudentId = candidateId;
        //            smsLog.PhoneNo = smsPhone;
        //            smsLog.SenderUserId = -99;
        //            smsLog.SentReferenceId = null;
        //            smsLog.SentSMSId = null;
        //            smsLog.SmsSendDate = DateTime.Now;
        //            smsLog.SmsType = -1;

        //            LogWriter.SmsLog(smsLog);
        //        }
        //        else //if sms sending passed
        //        {
        //            DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
        //            smsLog.AcaCalId = null;
        //            smsLog.Attribute1 = "Sms sending successful PurchaseNotification.aspx";
        //            smsLog.Attribute2 = "Success";
        //            smsLog.Attribute3 = null;
        //            smsLog.CreatedBy = candidateId;
        //            smsLog.CreatedDate = DateTime.Now;
        //            smsLog.CurrentSMSReferenceNo = stringData;
        //            smsLog.Message = messageBody;
        //            smsLog.StudentId = candidateId;
        //            smsLog.PhoneNo = smsPhone;
        //            smsLog.SenderUserId = -99;
        //            smsLog.SentReferenceId = null;
        //            smsLog.SentSMSId = null;
        //            smsLog.SmsSendDate = DateTime.Now;
        //            smsLog.SmsType = -1;

        //            LogWriter.SmsLog(smsLog);
        //        }
        //    }
        //}

        ////public static bool SendMail(string toAddress, string fromAddress, string name, string subject, string body)
        ////{
        ////    MailMessage msg = new MailMessage();
        ////    msg.To.Add(new MailAddress(toAddress));
        ////    msg.From = new MailAddress(fromAddress, "BUP Admission");
        ////    msg.Subject = subject;
        ////    msg.Body = body;
        ////    msg.IsBodyHtml = true;

        ////    //use app password instead of account password.

        ////    SmtpClient client = new SmtpClient();
        ////    client.UseDefaultCredentials = false;
        ////    client.Credentials = new System.Net.NetworkCredential("no-reply-2@bup.edu.bd", "B@up#-2018"); //ADM@2017
        ////    client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
        ////    client.Host = "smtp.office365.com";
        ////    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        ////    client.EnableSsl = true;

        ////    //SmtpClient client = new SmtpClient();
        ////    //client.UseDefaultCredentials = false;
        ////    //client.Credentials = new System.Net.NetworkCredential("","");
        ////    //client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
        ////    //client.Host = "smtp.gmail.com";
        ////    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
        ////    //client.EnableSsl = true;
        ////    try
        ////    {
        ////        client.Send(msg);
        ////        return true;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return false;
        ////    }
        ////} 
        #endregion


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

        public class trans_detail
        {
            public string status { get; set; }
            public string message { get; set; }
        }

        private trans_detail RocketHttpClientApi(string amount, string refNum, string fundType)
        {
            string _dbblCardType = ConfigurationManager.AppSettings["dbblCardType"];
            string _basePath = ConfigurationManager.AppSettings["dbblTransactionPath"];
            var urlPath = _basePath + "GetSubMerchantTransactionId";

            using (var client = new WebClient())
            {
                client.Headers.Add("secretKey", "4b4d40f4-3349-4c10-9a67-cc92813cf4ba");
                client.Headers.Add("authority", "bup");
                client.Headers.Add("fundType", fundType.ToString());

                var values = new NameValueCollection();
                values["amount"] = amount;
                values["cardtype"] = _dbblCardType;
                values["txnrefnum"] = refNum;
                values["clientip"] = GetIPAddress();

                var response = client.UploadValues(urlPath, values);

                var responseString = Encoding.Default.GetString(response);
                var jsonSerializer = new JavaScriptSerializer();
                var responseMessage = jsonSerializer.Deserialize<trans_detail>(responseString);

                return responseMessage;
            }
        }


        private void PayByRocket(DAL.CandidatePayment cPayment, string fundTypeT)
        {
            //var student = StudentManager.GetById(studentId);
            try
            {
                //var billHistoryMasterObj = BillHistoryMasterManager.GetById(billHistoryMasterId);
                var fundType = fundTypeT; // FundTypeManager.GetById(billHistoryMasterObj.FundId);

                string _dbblEcomUrl = ConfigurationManager.AppSettings["dbblGatewayUrl"];
                string _dbblCardType = ConfigurationManager.AppSettings["dbblCardType"];

                var paidAmount = Convert.ToDouble(cPayment.Amount);

                var rocketComission = paidAmount * 0.01; //rocket comission is 1%
                paidAmount += rocketComission;

                string amountInPaisa = (paidAmount * 100).ToString();
                string refNum = cPayment.PaymentId.ToString();
                string clientIp = GetIPAddress();
                string trans_id = string.Empty;

                var transDetail = RocketHttpClientApi(amountInPaisa, refNum, fundType.ToString());

                if (transDetail != null)
                {
                    if (transDetail.status.Equals("ok"))
                    {
                        string[] trnsFullPart = null;
                        if (transDetail.message.Contains("TRANSACTION_ID"))
                        {
                            trnsFullPart = transDetail.message.Split(':');
                            trans_id = trnsFullPart[1].Trim();

                            Response.Cookies["trans_id"].Value = trans_id;
                            Response.Cookies["un"].Value = CommonUtility.Encrypt.EncryptString(cPayment.PaymentId.ToString());


                            #region Online_Payment_Attempt
                            DAL.CollectionOnlineAttempt paymentAttempt = new DAL.CollectionOnlineAttempt();
                            paymentAttempt.CandidateId = cPayment.CandidateID;
                            paymentAttempt.PaymentId = cPayment.PaymentId;
                            paymentAttempt.CounterId = 7; // 7 = Rocket; From UCAM Table: BillCounter
                            paymentAttempt.PaymentType = "Rocket";
                            paymentAttempt.TransactionId = trans_id;
                            paymentAttempt.TransactionStartDate = DateTime.Now;
                            paymentAttempt.PaymentAmount = Convert.ToDecimal(paidAmount);
                            paymentAttempt.FundTypeFacultyId = fundTypeT;
                            paymentAttempt.IPAddress = clientIp;
                            paymentAttempt.IsPaid = false;
                            paymentAttempt.IsDelete = false;
                            paymentAttempt.CreatedBy = (long)cPayment.CandidateID;
                            paymentAttempt.CreatedDate = DateTime.Now;

                            int collectionOnlineAttemptId = -1;
                            using (var db = new CandidateDataManager())
                            {
                                db.Insert<DAL.CollectionOnlineAttempt>(paymentAttempt);
                                collectionOnlineAttemptId = paymentAttempt.ID;
                            }

                            #endregion


                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                string logNewObject = "2_PaymentId: " + cPayment.PaymentId + "; Messagë: Rocket TransactionID: " + trans_id + "; PaymentId: " + cPayment.PaymentId + " try to pay the " + cPayment.Amount.ToString() + " using Rocket Online Payment.";

                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.EventName = "btnDBBLRocket_Click";
                                dLog.PageName = "PurchaseNotification.aspx";
                                dLog.OldData = null;
                                dLog.NewData = logNewObject;
                                dLog.UserId = (long)cPayment.CandidateID;
                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                                    + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                                dLog.DateTime = DateTime.Now;

                                dLog.Attribute1 = "Success";

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            //###################only rocket is allowed. rocket card type = [6]################
                            //if you want to allowed all type of card you have to create the system of your own.
                            //NEXUS Debit. Card Type Value 1
                            //DBBL Master Debit. Card Type Value 2
                            //DBBL VISA Debit. Card Type Value 3
                            //Any Bank VISA Credit. Card Type Value 4
                            //Any Bank Master Credit. Card Type Value 5
                            //Rocket Mobile Banking. Card Type Value 6
                            string urlWithData = _dbblEcomUrl + "?card_type=" + _dbblCardType + "&trans_id=" + HttpContext.Current.Server.UrlEncode(trans_id);
                            Response.Redirect(urlWithData);
                        }
                        else
                        {

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                string logNewObject = "3_PaymentId: " + cPayment.PaymentId + "; Messagë: Rocket TransactionID not Found; PaymentId: " + cPayment.PaymentId + " try to pay the " + cPayment.Amount.ToString() + " using Rocket Online Payment.";

                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.EventName = "btnDBBLRocket_Click";
                                dLog.PageName = "PurchaseNotification.aspx";
                                dLog.OldData = null;
                                dLog.NewData = logNewObject;
                                dLog.UserId = (long)cPayment.CandidateID;
                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                                    + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                                dLog.DateTime = DateTime.Now;

                                dLog.Attribute1 = "Failed";

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            MessageView("No transaction id found. Please try again.", "fail");
                        }
                    }
                    else
                    {
                        MessageView("Sometings went wrong. Contact administrator.", "fail");
                    }
                }
                else
                {
                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        string logNewObject = "4_PaymentId: " + cPayment.PaymentId + "; Messagë: No Transaction Details Found; PaymentId: " + cPayment.PaymentId + " try to pay the " + cPayment.Amount.ToString() + " using Rocket Online Payment.";

                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "btnDBBLRocket_Click";
                        dLog.PageName = "PurchaseNotification.aspx";
                        dLog.OldData = null;
                        dLog.NewData = logNewObject;
                        dLog.UserId = (long)cPayment.CandidateID;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                            + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                        dLog.DateTime = DateTime.Now;

                        dLog.Attribute1 = "Failed";

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    MessageView("No transaction details found. Contact administrator.", "fail");

                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Failed to go to Rocket Gateway. Exception: " + ex.Message.ToString(), "fail");

                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                try
                {
                    string logNewObject = "5_PaymentId: " + cPayment.PaymentId + "; Exception: " + ex.Message.ToString() + "; PaymentId: " + cPayment.PaymentId + " try to pay the " + cPayment.Amount.ToString() + " using Rocket Online Payment.";

                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "btnDBBLRocket_Click";
                    dLog.PageName = "PurchaseNotification.aspx";
                    dLog.OldData = null;
                    dLog.NewData = logNewObject;
                    dLog.UserId = (long)cPayment.CandidateID;
                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                        + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
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

        protected void btnDBBLRocket_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                DAL.CandidatePayment cPayment = null;
                DAL.CandidateFormSl cFormSl = null;
                DAL.AdmissionSetup admSetup = null;

                if (string.IsNullOrEmpty(Request.QueryString["value"]))
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator." +
                        "Note : Please do not make any payment without contacting the administrator.[Error Code = PN01X001PL]?type=danger", false);
                }
                else //if url value is present.
                {

                    string value = Request.QueryString["value"].ToString();
                    string[] values = value.Split(';');


                    long candidateId = Convert.ToInt64(values[0]);
                    long candidatePaymentID = Convert.ToInt64(values[1]);
                    long candidateFormSerialID = Convert.ToInt64(values[2]);

                    #region N/A
                    //bool? isMultiple = null;
                    //if (values[3] == "0")
                    //{
                    //    isMultiple = false;
                    //}
                    //else if (values[3] == "1")
                    //{
                    //    isMultiple = true;
                    //}

                    //string admUnitName = values[4].ToString(); 
                    #endregion

                    int educationCategoryId = Convert.ToInt32(values[5]);

                    string fundTypeT = string.Empty;

                    if (candidateId > 0 && candidatePaymentID > 0 && candidateFormSerialID > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                            cFormSl = db.AdmissionDB.CandidateFormSls.Find(candidateFormSerialID);
                            admSetup = db.AdmissionDB.AdmissionSetups.Find(cFormSl.AdmissionSetupID);

                            if (admSetup != null)
                            {
                                if (admSetup.AdmissionUnitID == 11)
                                {
                                    fundTypeT = "5"; // MISS
                                }
                                else if (admSetup.AdmissionUnitID == 12)
                                {
                                    fundTypeT = "6"; //MICT
                                }
                            }
                        }


                        if (cPayment != null && Convert.ToBoolean(cPayment.IsPaid) == false && !string.IsNullOrEmpty(fundTypeT))
                        {
                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                string logNewObject = "1_PaymentId: " + cPayment.PaymentId + "; Click Pay By Rocket and try to pay the " + cPayment.Amount.ToString() + " using Rocket Online Payment.";

                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.EventName = "btnDBBLRocket_Click";
                                dLog.PageName = "PurchaseNotification.aspx";
                                dLog.OldData = null;
                                dLog.NewData = logNewObject;
                                dLog.UserId = (long)cPayment.CandidateID;
                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                                    + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                                dLog.DateTime = DateTime.Now;

                                dLog.Attribute1 = "Success";

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            PayByRocket(cPayment, fundTypeT);
                        }
                        else
                        {
                            MessageView("Candidate or fund type not found.", "fail");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }


        private void EkPayPaymentGateway(DAL.StoreEkPay storeEkPay, DAL.CandidatePayment cPayment, DAL.BasicInfo candidate)
        {
            MessageView("", "clear");
            try
            {
                //ID StoreName   StoreId StorePass   IsActive IsMultipleAllowed   BaseURL SubURL  URLVersion SuccessUrl  FailedUrl CancelledUrl    IPNUrl SendBoxStoreId  SendBoxStorePass SendBoxBaseURL  SendBoxSubURL SendBoxURLVersion   SendBoxSuccessUrl SendBoxFailedUrl    SendBoxCancelledUrl SendBoxIPNUrl   Attribute1 Attribute2  CreatedBy DateCreated ModifiedBy DateModified
                //1   Undergrade NULL    NULL    1   NULL https://pg.ekpay.gov.bd/	ekpaypg/	v1	https://admission.bup.edu.bd/Admission/EkPaySuccess.aspx	https://admission.bup.edu.bd/Admission/EkPayFailed.aspx	https://admission.bup.edu.bd/Admission/EkPayCanceled.aspx	http://localhost:17124/Admission/EkPayIPNListener.aspx	bup_test	BunivP@tsT1	https://sandbox.ekpay.gov.bd/	ekpaypg/	v1	http://localhost:17124/Admission/EkPaySuccess.aspx	http://localhost:17124/Admission/EkPayFailed.aspx	http://localhost:17124/Admission/EkPayCanceled.aspx	http://localhost:17124/Admission/EkPayIPNListener.aspx	NULL	NULL	-99	2022-02-04 00:00:00.000	NULL	NULL

                // Checking If it is a Local Server then don't Send Email
                // 0 is for Local Server
                // 1 is for Live Server
                string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

                string ekPayStoreId = "";
                string ekPayStorePass = "";

                string ekPayBaseURL = "";
                string ekPaySubURL = "";
                string ekPayURLVersion = "";

                string ekPaySuccessURL = "";
                string ekPayFailedURL = "";
                string ekPayCanceledURL = "";
                string ekPayIPNListenerURL = "";
                string ekPayMacAddress = "";

                if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
                {
                    ekPayStoreId = storeEkPay.StoreId.ToString().Trim();
                    ekPayStorePass = storeEkPay.StorePass.ToString().Trim();

                    ekPayBaseURL = storeEkPay.BaseURL.ToString().Trim();
                    ekPaySubURL = storeEkPay.SubURL.ToString().Trim();
                    ekPayURLVersion = storeEkPay.URLVersion.ToString().Trim();

                    ekPaySuccessURL = storeEkPay.SuccessUrl.ToString().Trim();
                    ekPayFailedURL = storeEkPay.FailedUrl.ToString().Trim();
                    ekPayCanceledURL = storeEkPay.CancelledUrl.ToString().Trim();
                    ekPayIPNListenerURL = storeEkPay.IPNUrl.ToString().Trim();

                    ekPayMacAddress = storeEkPay.MacAddress.ToString().Trim();
                }
                else
                {
                    ekPayStoreId = storeEkPay.SendBoxStoreId.ToString().Trim();
                    ekPayStorePass = storeEkPay.SendBoxStorePass.ToString().Trim();

                    ekPayBaseURL = storeEkPay.SendBoxBaseURL.ToString().Trim();
                    ekPaySubURL = storeEkPay.SendBoxSubURL.ToString().Trim();
                    ekPayURLVersion = storeEkPay.SendBoxURLVersion.ToString().Trim();

                    ekPaySuccessURL = storeEkPay.SendBoxSuccessUrl.ToString().Trim();
                    ekPayFailedURL = storeEkPay.SendBoxFailedUrl.ToString().Trim();
                    ekPayCanceledURL = storeEkPay.SendBoxCancelledUrl.ToString().Trim();
                    ekPayIPNListenerURL = storeEkPay.SendBoxIPNUrl.ToString().Trim();

                    ekPayMacAddress = storeEkPay.SendBoxMacAddress.ToString().Trim();
                }

                ek_pay ekPay = new ek_pay();

                mer_info merInfo = new mer_info();
                merInfo.mer_reg_id = ekPayStoreId;
                merInfo.mer_pas_key = ekPayStorePass;
                ekPay.mer_info = merInfo;

                ekPay.req_timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GMT+6"; //"2020-11-08 18:14:00 GMT+6";

                feed_uri feedUri = new feed_uri();
                feedUri.s_uri = ekPaySuccessURL;
                feedUri.f_uri = ekPayFailedURL;
                feedUri.c_uri = ekPayCanceledURL;
                ekPay.feed_uri = feedUri;

                cust_info custInfo = new cust_info();
                custInfo.cust_id = cPayment.PaymentId.ToString(); //DateTime.Now.ToString("yyyyMMddHHmmss"); //"2201171429";
                custInfo.cust_name = candidate.FirstName; //"Ariq Rahman";
                custInfo.cust_mobo_no = candidate.SMSPhone; //"+8801676675257";
                custInfo.cust_email = candidate.Email; //"ariqrahman.office@gmail.com";
                custInfo.cust_mail_addr = cPayment.CandidateID.ToString(); //CandidateID
                ekPay.cust_info = custInfo;

                string transactionId = CreateTransactionID(cPayment.PaymentId.ToString()); //DateTime.Now.ToString("yyyyMMddHHmmss");
                trns_info trnsInfo = new trns_info();
                trnsInfo.trnx_id = transactionId; // this transaction will be different id in each request
                trnsInfo.trnx_amt = cPayment.Amount.ToString(); // "10";
                trnsInfo.trnx_currency = "BDT";
                trnsInfo.ord_id = cPayment.PaymentId.ToString(); //payment id
                trnsInfo.ord_det = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GMT+6"; // "2020-11-08 18:14:00 GMT+6";
                ekPay.trns_info = trnsInfo;

                ipn_info ipnInfo = new ipn_info();
                ipnInfo.ipn_channel = "3";
                ipnInfo.ipn_email = "no-reply-2@bup.edu.bd";
                ipnInfo.ipn_uri = ekPayIPNListenerURL;
                ekPay.ipn_info = ipnInfo;

                ekPay.mac_addr = ekPayMacAddress; //"1.1.1.1"; //BUP r Live IP Address will be set in this MAC Address field

                EkPayPaymentGateway ekPayPG = new EkPayPaymentGateway(ekPayBaseURL, ekPaySubURL, ekPayURLVersion, "/merchant-api");
                ResponseEkPay responseEkPay = ekPayPG.GetSecureToken(ekPay);


                if (responseEkPay.ResponseCode == 200)
                {
                    SuccessResponse sr = (SuccessResponse)responseEkPay.ResponseData;

                    string urlParamiters = "?";
                    urlParamiters += "sToken=" + sr.secure_token + "&";
                    urlParamiters += "trnsID=" + transactionId;

                    string url = ekPayBaseURL + ekPaySubURL + ekPayURLVersion + urlParamiters;

                    #region Online_Payment_Attempt
                    try
                    {
                        var paymentAttempt = new DAL.OnlineCollectionAttempt()
                        {
                            CandidateId = cPayment.CandidateID,
                            PaymentId = cPayment.PaymentId,
                            TransactionId = transactionId,
                            PaymentAmount = cPayment.Amount,
                            IsPaid = false,
                            CreatedBy = (long)cPayment.CandidateID,
                            CreatedDate = DateTime.Now
                        };
                        using (var db = new CandidateDataManager())
                        {
                            db.Insert<DAL.OnlineCollectionAttempt>(paymentAttempt);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "EkPayPaymentGateway";
                        dLog.PageName = "PurchaseNotification.aspx";
                        dLog.OldData = null;
                        dLog.CandidateId = cPayment.CandidateID;
                        dLog.NewData = "Calling EkPay Payment Gateway; URL: " + url;
                        dLog.UserId = (long)cPayment.CandidateID;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                            + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                        dLog.DateTime = DateTime.Now;

                        //dLog.Attribute1 = cPayment.PaymentId.ToString();
                        //dLog.Attribute2 = cPayment.PaymentId.ToString();
                        //dLog.Attribute3 = cPayment.CandidateID.ToString();

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion


                    Response.Redirect(url, false);

                }
                else if (responseEkPay.ResponseCode == 400)
                {
                    SuccessResponse sr = (SuccessResponse)responseEkPay.ResponseData;

                    string msg = "";
                    msg += "No Token Generate, " +
                            "msg_code: " + sr.msg_code + ", " +
                            "msg_det: " + sr.msg_det + ", " +
                            "ack_tstamp: " + sr.ack_timestamp;

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "EkPayPaymentGateway";
                        dLog.PageName = "PurchaseNotification.aspx";
                        dLog.OldData = null;
                        dLog.CandidateId = cPayment.CandidateID;
                        dLog.NewData = "Calling EkPay But Not Get Secure Token; MSG: " + msg;
                        dLog.UserId = (long)cPayment.CandidateID;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                            + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                        dLog.DateTime = DateTime.Now;

                        //dLog.Attribute1 = cPayment.PaymentId.ToString();
                        //dLog.Attribute2 = cPayment.PaymentId.ToString();
                        //dLog.Attribute3 = cPayment.CandidateID.ToString();

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    MessageView("Something went wrong! MSG: " + msg.ToString(), "fail");

                }
                else if (responseEkPay.ResponseCode == 500)
                {
                    UnsuccessResponse ur = (UnsuccessResponse)responseEkPay.ResponseData;

                    string msg = "";
                    msg += "API Calling Failed, " +
                            "responseCode: " + ur.responseCode + ", " +
                            "responseMessage: " + ur.responseMessage;


                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "EkPayPaymentGateway";
                        dLog.PageName = "PurchaseNotification.aspx";
                        dLog.OldData = null;
                        dLog.CandidateId = cPayment.CandidateID;
                        dLog.NewData = "Invalid format/Invalid field value; MSG: " + msg;
                        dLog.UserId = (long)cPayment.CandidateID;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                            + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                        dLog.DateTime = DateTime.Now;

                        //dLog.Attribute1 = cPayment.PaymentId.ToString();
                        //dLog.Attribute2 = cPayment.PaymentId.ToString();
                        //dLog.Attribute3 = cPayment.CandidateID.ToString();

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    MessageView("Something went wrong! MSG: " + msg.ToString(), "fail");
                }
                else
                {

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "EkPayPaymentGateway";
                        dLog.PageName = "PurchaseNotification.aspx";
                        dLog.OldData = null;
                        dLog.CandidateId = cPayment.CandidateID;
                        dLog.NewData = "Exception occurred while calling EkPay; MSG: " + responseEkPay.ResponseMessage;
                        dLog.UserId = (long)cPayment.CandidateID;
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                            + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
                        dLog.DateTime = DateTime.Now;

                        //dLog.Attribute1 = cPayment.PaymentId.ToString();
                        //dLog.Attribute2 = cPayment.PaymentId.ToString();
                        //dLog.Attribute3 = cPayment.CandidateID.ToString();

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    MessageView(responseEkPay.ResponseMessage, "fail");
                }


            }
            catch (Exception ex)
            {
                MessageView("Something went wrong! Exception: " + ex.Message.ToString(), "fail");
            }
        }

        private string CreateTransactionID(string paymentId)
        {
            //var date = DateTime.Now;
            //string datetime = "DT" + String.Format("{0:ddMMyyyyhhmmss}", date);
            //string randomNumber = Guid.NewGuid().ToString().Substring(0, 2).ToUpper();

            string datetime = "DT" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string refCode = paymentId + "EKPAY" + datetime;

            return refCode;
        }

        protected void btnPayByEkPay_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                if (string.IsNullOrEmpty(Request.QueryString["value"]))
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator." +
                        "Note : Please do not make any payment without contacting the administrator.[Error Code = PN01X001PL]?type=danger", false);
                }
                else //if url value is present.
                {
                    //TODO decrypt url parameter
                    //string value = Decrypt.DecryptString(this.Request.QueryString["value"].ToString());
                    //string[] values = value.Split(';');
                    string value = Request.QueryString["value"].ToString();
                    string[] values = value.Split(';');

                    bool? isMultiple = null;

                    long candidateId = Convert.ToInt64(values[0]);
                    long candidatePaymentID = Convert.ToInt64(values[1]);
                    long candidateFormSerialID = Convert.ToInt64(values[2]);
                    if (values[3] == "0")
                    {
                        isMultiple = false;
                    }
                    else if (values[3] == "1")
                    {
                        isMultiple = true;
                    }
                    string admUnitName = values[4].ToString();
                    int educationCategoryId = Convert.ToInt32(values[5]);

                    if (candidatePaymentID > 0)
                    {
                        DAL.BasicInfo candidate = null;
                        DAL.CandidatePayment cPayment = null;

                        DAL.StoreEkPay storeEkPay = null;

                        using (var db = new CandidateDataManager())
                        {
                            cPayment = db.AdmissionDB.CandidatePayments.Find(candidatePaymentID);
                            candidate = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cPayment.CandidateID).FirstOrDefault();
                        }

                        using (var db = new OfficeDataManager())
                        {
                            storeEkPay = db.AdmissionDB.StoreEkPays.Where(x => x.ID == 1 && x.IsActive == true).FirstOrDefault(); // 1 = Undergrade
                        }

                        #region EkPay Gateway
                        if (cPayment != null && candidate != null && storeEkPay != null)
                        {
                            EkPayPaymentGateway(storeEkPay, cPayment, candidate);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }
    }
}