using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class CandidateLoginCredentials : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        long urlCId = -1;
        long uId = 0;
        string uRole = string.Empty;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (uRole.ToLower() == "candidate")
            {

                SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                Response.Redirect("~/Admission/Home.aspx", false);
            }

            if (Request.QueryString["val"] != null)
            {
                string urlCalue = Request.QueryString["val"].ToString();
                if (urlCalue != "")
                {
                    urlCId = Int64.Parse(urlCalue);

                    if (urlCId > 0)
                    {
                        DAL.CandidatePayment cp = null;
                        using (var db = new CandidateDataManager())
                        {
                            cp = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == urlCId).FirstOrDefault();
                        }

                        if (cp != null)
                        {
                            LoadCandidateData(null, cp.PaymentId);
                        }

                    }
                }
            }

            if (!IsPostBack)
            {
                
            }
        }

        #region N/A
        //private void LoadCandidateData(long paymentId)
        //{
        //    using(var db = new CandidateDataManager())
        //    {
        //        var list = db.GetCandidateUserByPaymentId_JOIN(paymentId);
        //        if(list != null)
        //        {
        //            lvCandidateCred.DataSource = list;
        //        }
        //        else
        //        {
        //            lvCandidateCred.DataSource = null;
        //        }
        //    }
        //    lvCandidateCred.DataBind();
        //} 
        #endregion

        private void LoadCandidateData(string mobile, long? paymentId)
        {
            using (var db = new CandidateDataManager())
            {
                var list = db.GetCandidateUserByPaymentId_JOIN(paymentId, mobile);
                if (list != null)
                {
                    lvCandidateCred.DataSource = list;
                }
                else
                {
                    lvCandidateCred.DataSource = null;
                }
            }
            lvCandidateCred.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            long paymentId = -1;

            paymentId = Convert.ToInt64(txtPaymentId.Text);

            txtMobileNo.Text = "+88";

            if(paymentId > 0)
            {
                LoadCandidateData(null, paymentId);
            }
        }

        protected void btnSearchMobile_Click(object sender, EventArgs e)
        {
            string mobile = "";

            txtPaymentId.Text = string.Empty;

            mobile = txtMobileNo.Text.Trim();

            if (!string.IsNullOrEmpty(mobile))
            {
                LoadCandidateData(mobile, null);
            }
        }
    }
}