using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;

namespace CommonUtility
{
    public static class TeletalkEducationBoard
    {

        public static string GetData(string examType, string board, string roll, string passingYear, string RegNumber)
        {
            string api_key = System.Web.Configuration.WebConfigurationManager.AppSettings["TeletalkEducationBoardAPIKEY"];

            string result = "";

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var myReq = (HttpWebRequest)WebRequest.Create("http://ebapi.teletalk.com.bd//v3.0/ebapi.php");
                myReq.ContentType = "application/json";
                myReq.Headers["APIKEY"] = api_key;
                myReq.Method = "POST";

                using (var streamWriter = new System.IO.StreamWriter(myReq.GetRequestStream()))
                {
                    //string json = "{\"commandID\":\"getDetailsResult\"," +
                    //              "\"exam\":\"hsc\"," +
                    //              "\"board\":\"dhaka\"," +
                    //              "\"rollNo\":\"123456\"," +
                    //              "\"year\":\"2018\"}";

                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        commandID = "getDetailsResult",
                        exam = examType,
                        board = board,
                        rollNo = roll,
                        year = passingYear,
                        regNo = RegNumber
                    });


                    streamWriter.Write(json);
                }

                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();

                //dynamic resp = JsonConvert.DeserializeObject<dynamic>(responseString);

                result = responseString;

                return result;
            }
            catch (Exception ex)
            {
                result = new JavaScriptSerializer().Serialize(new
                {
                    responseCode = "-99",
                    responseDesc = ex.Message.ToString()
                });

                return result;
            }


        }



    }
}
