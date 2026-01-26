using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public static class FosterPaymentGateway
    {
        public static string GenerateHashValue(string mcnt_AccessCode, string mcnt_TxnNo, string mcnt_ShortName,
            string mcnt_OrderNo, string mcnt_ShopId, string mcnt_Amount, string mcnt_Currency, string secretKey)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["mcnt_AccessCode"] = mcnt_AccessCode;
            queryString["mcnt_TxnNo"] = mcnt_TxnNo;
            queryString["mcnt_ShortName"] = mcnt_ShortName;
            queryString["mcnt_OrderNo"] = mcnt_OrderNo;
            queryString["mcnt_ShopId"] = mcnt_ShopId;
            queryString["mcnt_Amount"] = mcnt_Amount;
            queryString["mcnt_Currency"] = mcnt_Currency;

            //string hashValue = CreateSHA256(queryString.ToString(), "854f788b67607c148844ba70a5d4784d");
            string hashValue = CreateSHA256(queryString.ToString(), secretKey);
            string s = queryString.ToString();
            return hashValue;
        }

        public static string CreateSHA256(string message, string secretKey)
        {
            string key = secretKey.ToUpper();
            string checkKey1 = "";
            string checkKey2 = "";
            string checkKey3 = "";
            string checkKey4 = "";
            string checkKey = "";
            checkKey1 += Convert.ToString(Convert.ToInt32(key.Substring(0, 8), 16), 2);
            checkKey2 += Convert.ToString(Convert.ToInt32(key.Substring(8, 8), 16), 2);
            checkKey3 += Convert.ToString(Convert.ToInt32(key.Substring(16, 8), 16), 2);
            checkKey4 += Convert.ToString(Convert.ToInt32(key.Substring(24, 8), 16), 2);

            checkKey = (checkKey1.Length == 32 ? checkKey1 : "0" + checkKey1) + (checkKey2.Length == 32 ? checkKey2 : "0" + checkKey2) + (checkKey3.Length == 32 ? checkKey3 : "0" + checkKey3) + (checkKey4.Length == 32 ? checkKey4 : "0" + checkKey4);

            Console.WriteLine("checkKey1:" + (checkKey1.Length == 32 ? checkKey1 : "0" + checkKey1));
            Console.WriteLine("checkKey2:" + (checkKey2.Length == 32 ? checkKey2 : "0" + checkKey2));
            Console.WriteLine("checkKey3:" + (checkKey3.Length == 32 ? checkKey3 : "0" + checkKey3));
            Console.WriteLine("checkKey4:" + (checkKey4.Length == 32 ? checkKey4 : "0" + checkKey4));


            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] keyByte = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] posA = { 128, 64, 32, 16, 8, 4, 2, 1 };
            byte zero = 0;
            for (int ikb = 0; ikb < keyByte.Length; ikb++)
            {
                //keyByte[ikb]
                int pos = 0;
                for (int ick = ikb * 8; ick < ((ikb + 1) * 8); ick++, pos++)
                {
                    byte nv = checkKey.Substring(ick, 1) == "0" ? zero : posA[pos];
                    keyByte[ikb] += nv;
                }
                // Console.WriteLine("BA:"+ikb+"-"+keyByte[ikb]+"-");
            }

            //System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            //byte[] keyByte = enc.GetBytes(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = enc.GetBytes(message);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return ByteToString(hashmessage);
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        public static string Convertmd5(string a)
        {
            //string hash="";
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputbytes = System.Text.Encoding.ASCII.GetBytes(a);
            byte[] hash = md5.ComputeHash(inputbytes);

            return ByteToString(hash);
        }

        public static string NameValueCollectionToString(NameValueCollection nvc)
        {
            List<string> items = new List<string>();

            foreach (string name in nvc)
                items.Add(string.Concat(name, "=", System.Web.HttpUtility.UrlEncode(nvc[name])));

            return string.Join("&", items.ToArray());
        }
    }
}
