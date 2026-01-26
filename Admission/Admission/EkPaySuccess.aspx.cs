using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class EkPaySuccess : System.Web.UI.Page
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        string url = System.Web.HttpContext.Current.Request.RawUrl;
        string clientIpAddress = HttpContext.Current.Request.UserHostAddress;

        //string trnsId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlSuccessIcon.Visible = false;
            pnlFailICon.Visible = false;

            try
            {
                if ((string.IsNullOrEmpty(Request.QueryString["transId"])))
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                }
                else
                {
                    if (!IsPostBack)
                    {
                        
                        string trnsId = Request.QueryString["transId"].ToString();

                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                        try
                        {
                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            dLog.EventName = "EkPaySuccess";
                            dLog.PageName = "EkPaySuccess.aspx";
                            dLog.OldData = null;
                            //dLog.CandidateId = cPayment.CandidateID;
                            dLog.NewData = "Landed In Success Page; ClientIpAddress: " + clientIpAddress + "; URL: " + url + ";";
                            dLog.UserId = 0;
                            //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; "
                            //    + SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cPayment.CandidateID;
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

                        DAL.OnlineCollectionAttempt oca = null;
                        using (var db = new CandidateDataManager())
                        {
                            oca = db.AdmissionDB.OnlineCollectionAttempts.Where(x => x.TransactionId == trnsId).FirstOrDefault();
                        }
                        if (oca != null)
                        {
                            DAL.CandidatePayment cp = null;
                            DAL.BasicInfo bi = null;
                            using (var db = new CandidateDataManager())
                            {
                                cp = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == oca.CandidateId).FirstOrDefault();
                                bi = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cp.CandidateID).FirstOrDefault();
                            }

                            if (cp != null && cp.IsPaid == true)
                            {
                                // payment is successful
                                messagePanel.Visible = true;
                                messagePanel.CssClass = "jumbotron alert alert-success";
                                lblMsg.Text = "Payment Successful";
                                lblName.Text = bi.FirstName.ToString();
                                lblPaymentId.Text = cp.PaymentId.ToString();

                                pnlSuccessIcon.Visible = true;
                                pnlFailICon.Visible = false;

                                panelReloadMessage.Visible = false;
                                panelSuccessNote.Visible = true;

                            }
                            else
                            {
                                //redirect to failed page
                                messagePanel.Visible = true;
                                messagePanel.CssClass = "jumbotron alert alert-warning";
                                lblMsg.Text = "Payment is not updated!";
                                lblName.Text = bi.FirstName.ToString();
                                lblPaymentId.Text = cp.PaymentId.ToString();

                                pnlSuccessIcon.Visible = false;
                                pnlFailICon.Visible = true;

                                panelReloadMessage.Visible = true;
                                panelSuccessNote.Visible = false;
                            }
                        }
                        else
                        {
                            //no teansaction id found
                            //Response.Redirect("~/Admission/EkPayFailed.aspx", false);
                            messagePanel.Visible = true;
                            messagePanel.CssClass = "jumbotron alert alert-danger";
                            lblMsg.Text = "No Transaction ID Found!";
                            lblName.Text = "---";
                            lblPaymentId.Text = "---";

                            pnlSuccessIcon.Visible = false;
                            pnlFailICon.Visible = true;
                            panelReloadMessage.Visible = true;
                            panelSuccessNote.Visible = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // exception message
                messagePanel.Visible = true;
                messagePanel.CssClass = "jumbotron alert alert-danger";
                lblMsg.Text = "Exception: Something went wrong!";
                lblName.Text = "---";
                lblPaymentId.Text = "---";

                pnlSuccessIcon.Visible = false;
                pnlFailICon.Visible = true;
                panelReloadMessage.Visible = true;
                panelSuccessNote.Visible = false;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {

            }
        }
    }
}