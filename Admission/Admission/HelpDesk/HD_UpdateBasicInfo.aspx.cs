using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using Newtonsoft.Json.Linq;


namespace Admission.Admission.HelpDesk
{
    public partial class HD_UpdateBasicInfo : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
            }

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                long PaymentId = 0;

                ClearGridView();

                if (string.IsNullOrEmpty(txtPaymentId.Text))
                {
                    showAlert("Please enter a paymentId");
                    return;
                }

                PaymentId = Convert.ToInt64(txtPaymentId.Text.Trim());


                using (var db = new OfficeDataManager())
                {
                    var CandidatePaymentObj = db.AdmissionDB.CandidatePayments.Where(x=>x.PaymentId== PaymentId).FirstOrDefault();

                    if (CandidatePaymentObj != null)
                    {
                        var CandidateBasicInfo = db.AdmissionDB.BasicInfoes.Find(CandidatePaymentObj.CandidateID);

                        if (CandidateBasicInfo != null && CandidateBasicInfo != null)
                        {
                            List<DAL.BasicInfo> list = new List<DAL.BasicInfo>();

                            list.Add(CandidateBasicInfo);

                            gvCandidateInfo.DataSource = list;
                            gvCandidateInfo.DataBind();
                        }
                        else
                        {
                            showAlert("Candidate basic information not found");
                            return;
                        }
                    }
                    else
                    {
                        showAlert("Please enter a valid paymentId");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearGridView()
        {
            try
            {
                gvCandidateInfo.DataSource = null;
                gvCandidateInfo.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);

        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton linkButton = new LinkButton();
                linkButton = (LinkButton)sender;
                long CandidateId = Convert.ToInt64(linkButton.CommandArgument);

                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;

                if (CandidateId > 0)
                {

                    string Email = "", Phone = "";

                    TextBox txtEmail = (TextBox)gvrow.FindControl("txtEmail");
                    TextBox txtphone = (TextBox)gvrow.FindControl("txtMobile");

                    if (txtEmail.Text == null || txtEmail.Text == "" || txtphone.Text == null || txtphone.Text == "")
                    {
                        showAlert("Please enter email and phone number");
                        return;
                    }

                    Email = txtEmail.Text.Trim();
                    Phone = txtphone.Text.Trim();

                    using (var db = new OfficeDataManager())
                    {
                        var CandidateObj = db.AdmissionDB.BasicInfoes.Find(CandidateId);

                        if (CandidateObj != null)
                        {
                            string PrevInfo = "";
                            try
                            {
                                PrevInfo = "Email : " + CandidateObj.Email + " , Mobile : " + CandidateObj.Mobile;
                            }
                            catch (Exception ex)
                            {
                            }

                            string NewInfo = "Email : " + Email + " , Mobile : " + Phone;

                            CandidateObj.Email = Email;
                            CandidateObj.SMSPhone = Phone;
                            CandidateObj.Mobile = Phone;

                            CandidateObj.ModifiedBy = uId;
                            CandidateObj.DateModified = DateTime.Now;

                            db.Update<DAL.BasicInfo>(CandidateObj);

                            try
                            {
                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.DateTime = DateTime.Now;
                                dLog.DateCreated = DateTime.Now;
                                dLog.UserId = uId;
                                dLog.CandidateId = CandidateObj.ID;
                                dLog.EventName = "Update Basic Info (Help Desk)";
                                dLog.PageName = "ApplicationBasic.aspx";
                                dLog.OldData = PrevInfo;
                                dLog.NewData = NewInfo;
                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + CandidateObj.ID;

                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }


                        }

                        showAlert("Updated Successfully");
                        btnLoad_Click(null, null);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void txtPaymentId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
            }
            catch (Exception ex)
            {
            }
        }
    }
}