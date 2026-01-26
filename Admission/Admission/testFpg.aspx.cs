using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Admission.Admission
{
    public partial class testFpg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DAL.StoreFoster storeFoster = null;

            using (var db = new OfficeDataManager())
            {
                storeFoster = db.AdmissionDB.StoreFosters.Where(c => c.AccessCode == "171109122568").FirstOrDefault();
            }

            //string secretKey = "754f788b67607c148844ba70a5d4784c";
            string urlParamHashed = FosterPaymentGateway.GenerateHashValue(storeFoster.AccessCode, "Txn20160218122740", storeFoster.MerchantShortName, "001", storeFoster.ShopId, "10", "BDT", storeFoster.SecurityKey);

            string uri = "http://demo.fosterpayments.com/fosterpayments/receivemerchantpaymentrequestwsfc.php?";

            String TxnResponse = "2";
            String MerchantTxnNo = "Txn20160218122740";
            String phpmd5convertcsharp = FosterPaymentGateway.Convertmd5(string.Concat(string.Concat(TxnResponse, MerchantTxnNo, storeFoster.SecurityKey)));

            NameValueCollection urlParam = new NameValueCollection
            {
                {"mcnt_TxnNo", "Txn20160218122740"},
                {"mcnt_ShortName", storeFoster.MerchantShortName},
                {"mcnt_OrderNo", "001"},
                {"mcnt_ShopId", storeFoster.ShopId},
                {"mcnt_Amount", "10"},
                {"mcnt_Currency", "BDT"},
                {"cust_InvoiceTo", "Test Customer name"},
                {"cust_CustomerServiceName", "onlinepayments"},
                {"cust_CustomerName", "Test"},
                {"cust_CustomerEmail", "no-reply-2@bup.edu.bd"},
                {"cust_CustomerAddress", "Dhaka"},
                {"cust_CustomerContact", "01759438066"},
                {"cust_CustomerGender", ""},
                {"cust_CustomerCity", "Dhaka"},
                {"cust_CustomerState", "Dhaka"},
                {"cust_CustomerPostcode", "1216"},
                {"cust_CustomerCountry", "Bangladesh"},
                {"cust_Billingaddress", "Bangladesh"},
                {"cust_ShippingAddress", "Bangladesh"},
                {"cust_orderitems", "Txn20160218122740"},
                {"success_url", storeFoster.SuccessUrl},
                {"cancel_url", storeFoster.CancelUrl},
                {"fail_url", storeFoster.FailUrl},
                {"merchentdomainname", "admission.bup.edu.bd"},
                {"merchentip", "202.79.20.181"},
                {"mcnt_SecureHashValue", FosterPaymentGateway
                                        .GenerateHashValue(storeFoster.AccessCode, "Txn20160218122740", storeFoster.MerchantShortName, "001", storeFoster.ShopId, "10", "BDT", storeFoster.SecurityKey)
                                        .ToLower()}
            };

            string queryString = FosterPaymentGateway.NameValueCollectionToString(urlParam);
            string response = uri + queryString;
            Response.Redirect(response);
        }
    }
}