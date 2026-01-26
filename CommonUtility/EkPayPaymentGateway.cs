using DAL.ViewModels.EkPayModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class EkPayPaymentGateway
    {

        public string base_url { get; set; }
        public string sub_url { get; set; }
        public string url_version { get; set; }
        public string api_controller { get; set; }

        //protected string Base_URL = "https://pg.ekpay.gov.bd/";
        //protected string Sub_URL = "ekpaypg/v1/";
        //protected string Validation_URL = "validator/api/validationserverAPI.php";
        //protected string Checking_URL = "validator/api/merchantTransIDvalidationAPI.php";


        public EkPayPaymentGateway(string _baseUrl, string _subUrl, string _url_version, string _apiController)
        {
            base_url = _baseUrl;
            sub_url = _subUrl;
            url_version = _url_version;
            api_controller = _apiController;
        }

        public ResponseEkPay GetSecureToken<T>(T obj)        {            ResponseEkPay responseEkPay = new ResponseEkPay();            try
            {
                var url = base_url + sub_url + url_version + api_controller;
                var json = JsonConvert.SerializeObject(obj);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                        delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                var response = client.PostAsync(url, data);
                response.Wait();

                var result = response.Result;

                var contentString = result.Content.ReadAsStringAsync();

                int statusCode = (int)result.StatusCode;

                if (result.IsSuccessStatusCode == true)
                {
                    SuccessResponse resp = JsonConvert.DeserializeObject<SuccessResponse>(contentString.Result);









                    /// <summary>                    /// 1000 = Secure token generated successfully                    /// 1020 = Transaction completed successfully                    /// </summary>                    if (statusCode == 200 && resp.msg_code == "1000") //( || resp.msg_code == "1020")
                    {
                        // Success with secure_token
                        responseEkPay.ResponseCode = 200;
                        responseEkPay.ResponseStatus = "Success";
                        responseEkPay.ResponseMessage = "";
                        responseEkPay.ResponseData = resp;
                    }
                    else
                    {
                        // Api call is success but, Unable to generate security token
                        responseEkPay.ResponseCode = 400;
                        responseEkPay.ResponseStatus = "Failed";
                        responseEkPay.ResponseMessage = "";
                        responseEkPay.ResponseData = resp;
                    }

                    //return contentString.Result;
                }
                else
                {
                    UnsuccessResponse resp = JsonConvert.DeserializeObject<UnsuccessResponse>(contentString.Result);
                    // Api call failed
                    responseEkPay.ResponseCode = 500;
                    responseEkPay.ResponseStatus = "Failed";
                    responseEkPay.ResponseMessage = "";
                    responseEkPay.ResponseData = resp;

                    //return contentString.Result;
                }
            }
            catch (Exception ex)
            {
                responseEkPay.ResponseCode = 600;
                responseEkPay.ResponseStatus = "Failed";
                responseEkPay.ResponseMessage = "Error: Something went wrong; Exception: " + ex.Message.ToString();
                responseEkPay.ResponseData = "";
            }

            return responseEkPay;
        }



        public ResponseEkPay SearchByTransactionId<T>(T obj)        {            ResponseEkPay responseEkPay = new ResponseEkPay();            try
            {
                var url = base_url + sub_url + url_version + api_controller;
                var json = JsonConvert.SerializeObject(obj);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                        delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                var response = client.PostAsync(url, data);
                response.Wait();

                var result = response.Result;

                var contentString = result.Content.ReadAsStringAsync();

                int statusCode = (int)result.StatusCode;

                if (result.IsSuccessStatusCode == true)
                {
                    var resp = contentString.Result;//(JObject)JsonConvert.DeserializeObject(contentString.Result);

                    

                    /// <summary>                    /// 1000 = Secure token generated successfully                    /// 1020 = Transaction completed successfully                    /// </summary>                    if (statusCode == 200) //&& (resp.msg_code == "1000" || resp.msg_code == "1020")
                    {
                        // Success with secure_token
                        responseEkPay.ResponseCode = 200;
                        responseEkPay.ResponseStatus = "Success";
                        responseEkPay.ResponseMessage = "";
                        responseEkPay.ResponseData = resp;
                    }
                    else
                    {
                        // Api call is success but, Unable to generate security token
                        responseEkPay.ResponseCode = 400;
                        responseEkPay.ResponseStatus = "Failed";
                        responseEkPay.ResponseMessage = "";
                        responseEkPay.ResponseData = resp;
                    }

                    //return contentString.Result;
                }
                else
                {
                    UnsuccessResponse resp = JsonConvert.DeserializeObject<UnsuccessResponse>(contentString.Result);
                    // Api call failed
                    responseEkPay.ResponseCode = 500;
                    responseEkPay.ResponseStatus = "Failed";
                    responseEkPay.ResponseMessage = "";
                    responseEkPay.ResponseData = resp;

                    //return contentString.Result;
                }
            }
            catch (Exception ex)
            {
                responseEkPay.ResponseCode = 600;
                responseEkPay.ResponseStatus = "Failed";
                responseEkPay.ResponseMessage = "Exception: " + ex.Message.ToString();
                responseEkPay.ResponseData = "";
            }

            return responseEkPay;
        }









        //===============================================

        public string GetMethodApiList()        {            var url = base_url + sub_url + url_version + api_controller;            HttpClient client = new HttpClient();            System.Net.ServicePointManager.ServerCertificateValidationCallback +=                    delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,                                            System.Security.Cryptography.X509Certificates.X509Chain chain,                                            System.Net.Security.SslPolicyErrors sslPolicyErrors)                    {                        return true;                    };            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            var responseTask = client.GetAsync(url);            responseTask.Wait();            var result = responseTask.Result;            if (result.IsSuccessStatusCode)            {                var contentString = result.Content.ReadAsStringAsync();
                //var values = JsonConvert.DeserializeObject<dynamic>(contentString);
                return contentString.Result;            }            else            {                return null;            }        }




    }
}
