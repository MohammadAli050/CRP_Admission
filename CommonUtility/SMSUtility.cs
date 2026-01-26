using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CommonUtility
{
    public class SMSUtility
    {


        public static string Send(string phoneNo, string message)
        {

            // Checking If it is a Local Server then don't Send Email
            // 0 is for Local Server
            // 1 is for Live Server
            string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

            if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
            {

                var firstThreeChar = phoneNo.Substring(0, 3);
                phoneNo = firstThreeChar.Equals("+88") ? phoneNo.Remove(0, 3) : phoneNo;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://gpcmp.grameenphone.com/ecmapigw/webresources/ecmapigw.v2");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"username\":\"BUPadmin\"," +
                                  "\"password\":\"Bup@Api&20\"," +
                                  "\"apicode\":\"1\"," +
                                  "\"msisdn\":\"" + phoneNo + "\"," +
                                  "\"countrycode\":\"880\"," +
                                  "\"cli\":\"BUP\"," +
                                  "\"messagetype\":\"1\"," +
                                  "\"message\":\"" + message + "\"," +
                                  "\"messageid\":\"0\"}";

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(httpResponse.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                httpResponse.Close();

                return responseString;
            }
            else
            {
                return "{\"statusCode\":400}";
            }
        }

        public static string GetGrameenPhoneSmsBalance()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://gpcmp.grameenphone.com/ecmapigw/webresources/ecmapigw.v2");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"username\":\"BUPadmin\"," +
                              "\"password\":\"Bup@Api&20\"," +
                              "\"apicode\":\"3\"," +
                              "\"msisdn\":\"0\"," +
                              "\"countrycode\":\"0\"," +
                              "\"cli\":\"0\"," +
                              "\"messagetype\":\"0\"," +
                              "\"message\":\"0\"," +
                              "\"messageid\":\"0\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(httpResponse.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            httpResponse.Close();

            return responseString;
        }

        #region N/A
        //private static string userName = "bup789";
        //private static string password = "01769021586";
        //private static string sender = "BUP";

        //public static string Send(string phoneNo, string message)
        //{

        //    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://app.planetgroupbd.com/api/v3/sendsms/plain?user="
        //        + userName + "&password=" + password + "&sender=BUP"
        //        + "&SMSText=" + System.Web.HttpUtility.UrlEncode(message) + "&GSM=" + phoneNo + "&type=longSMS");

        //    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
        //    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
        //    string responseString = respStreamReader.ReadToEnd();
        //    respStreamReader.Close();
        //    myResp.Close();
        //    return responseString;



        //    #region N/A -- SendSmsByGrameenPhone
        //    //var firstThreeChar = phoneNo.Substring(0, 3);

        //    //phoneNo = firstThreeChar.Equals("+88") ? phoneNo.Remove(0, 3) : phoneNo;

        //    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    //var myReq = (HttpWebRequest)WebRequest.Create("https://gpcmp.grameenphone.com/ecmapigw/webresources/ecmapigw.v2");
        //    //myReq.ContentType = "application/json";
        //    //myReq.Method = "POST";

        //    //using (var streamWriter = new StreamWriter(myReq.GetRequestStream()))
        //    //{
        //    //    string json = "{\"username\":\"BUPadmin\"," +
        //    //                  "\"password\":\"Bup@Api&20\"," +
        //    //                  "\"apicode\":\"1\"," +
        //    //                  "\"msisdn\":\"" + phoneNo + "\"," +
        //    //                  "\"countrycode\":\"880\"," +
        //    //                  "\"cli\":\"BUP\"," +
        //    //                  "\"messagetype\":\"1\"," +
        //    //                  "\"message\":\"" + message + "\"," +
        //    //                  "\"messageid\":\"0\"}";

        //    //    streamWriter.Write(json);
        //    //}

        //    //HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
        //    //System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
        //    //string responseString = respStreamReader.ReadToEnd();
        //    //respStreamReader.Close();
        //    //myResp.Close();

        //    //return responseString;
        //    #endregion



        //} 
        #endregion

    }
}
