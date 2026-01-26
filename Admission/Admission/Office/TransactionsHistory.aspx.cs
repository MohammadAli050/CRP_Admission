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

namespace Admission.Admission.Office
{
    public partial class TransactionsHistory : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

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


            if (!IsPostBack)
            {
                LoadListView(null);
            }
        }

        private void LoadListView(string paymentId)
        {
            List<DAL.TransactionHistory> th = null;
            if (string.IsNullOrEmpty(paymentId)) //if no search string is provided, then only get the latest 30 transaction histories.
            {
                using (var db = new OfficeDataManager())
                {
                    th = db.AdmissionDB.TransactionHistories.Take(50).OrderByDescending(c => c.ID).ToList();
                }
                if (th.Count() > 0)
                {
                    lvTransHistory.DataSource = th;
                    lblCount.Text = th.Count().ToString();
                }
                else
                {
                    lvTransHistory.DataSource = null;
                    lblCount.Text = "0";
                }
                lvTransHistory.DataBind();
                lblPanelHeading.Text = "Showing the latest" + th.Count().ToString() + "transactions.";
            }
            else if (!string.IsNullOrEmpty(paymentId)) //if search string is provided.
            {
                using (var db = new OfficeDataManager())
                {
                    th = db.AdmissionDB.TransactionHistories
                        .Where(c => c.TransactionID == paymentId)
                        .OrderByDescending(c => c.ID)
                        .ToList();
                }
                if (th.Count() > 0)
                {
                    lvTransHistory.DataSource = th;
                    lblCount.Text = th.Count().ToString();
                }
                else
                {
                    lvTransHistory.DataSource = null;
                    lblCount.Text = "0";
                }
                lvTransHistory.DataBind();
                lblPanelHeading.Text = "Showing transaction details for:" + txtPaymentId.Text.Trim();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string paymentId = null;
            paymentId = txtPaymentId.Text.Trim();

            if (paymentId != null)
            {
                LoadListView(paymentId);
            }
            else
            {
                LoadListView(null);
            }
        }

        protected void lvTransHistory_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.TransactionHistory th = (DAL.TransactionHistory)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");

                Label lblCandidateName = (Label)currentItem.FindControl("lblCandidateName");
                Label lblStatus = (Label)currentItem.FindControl("lblStatus");
                Label lblTransDate = (Label)currentItem.FindControl("lblTransDate");
                Label lblTransactionId = (Label)currentItem.FindControl("lblTransactionId");
                Label lblAmount = (Label)currentItem.FindControl("lblAmount");
                Label lblBankTransId = (Label)currentItem.FindControl("lblBankTransId");
                Label lblCardType = (Label)currentItem.FindControl("lblCardType");
                Label lblCardNumber = (Label)currentItem.FindControl("lblCardNumber");
                Label lblCardBrand = (Label)currentItem.FindControl("lblCardBrand");
                Label lblCardIssuerCountry = (Label)currentItem.FindControl("lblCardIssuerCountry");
                Label lblIsManualInsert = (Label)currentItem.FindControl("lblIsManualInsert");
                //Label lblManualInsertDate = (Label)currentItem.FindControl("lblManualInsertDate");

                lblCandidateName.Text = th.ValueB;
                lblStatus.Text = th.Status;
                lblTransDate.Text = th.TransactionDate;
                lblTransactionId.Text = th.TransactionID;
                lblAmount.Text = th.Amount;
                lblBankTransId.Text = th.BankTransactionID;
                lblCardType.Text = th.CardType;
                lblCardNumber.Text = th.CardNumber;
                lblCardBrand.Text = th.CardBrand;
                lblCardIssuerCountry.Text = th.CardIssuerCountry;
                if(th.IsManualInsert == true)
                {
                    lblIsManualInsert.Text = "Yes";
                    lblIsManualInsert.ForeColor = Color.Green;
                }
                else
                {
                    lblIsManualInsert.Text = "No";
                    lblIsManualInsert.ForeColor = Color.Crimson;
                }

                //if(th.DateManualInsert == null)
                //{
                //    lblManualInsertDate.Text = "N/A";
                //}
                //else
                //{
                //    lblManualInsertDate.Text = th.DateManualInsert.ToString();
                //}
            }
        }
    }
}