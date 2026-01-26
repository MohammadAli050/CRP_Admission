using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CommonUtility
{
    public static class EmailUtility
    {
        //bhpiadmission @gmail.com
        //EdusoftConsultant@2026

        public static bool SendMail(string toAddress, string fromAddress, string name, string subject, string body)
        {
            // Checking If it is a Local Server then don't Send Email
            // 0 is for Local Server
            // 1 is for Live Server

            // Checking If it is a Local Server then don't Send Email
            // 0 is for Local Server, 1 is for Live Server
            string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

            if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
            {
                // Web.config theke app password collect kora (Gmail-er 2FA App Password use koro)
                string emailPass = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailPass"];

                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(toAddress));
                msg.From = new MailAddress("bhpiadmission@gmail.com", "CRP Admission"); // Match with Credentials
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com"; // Updated to Gmail SMTP
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("bhpiadmission@gmail.com", emailPass);

                // Explicitly setting Security Protocol to TLS 1.2 or 1.3
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                try
                {
                    client.Send(msg);
                    return true;
                }
                catch (Exception ex)
                {
                    // Debugging er jonno ex.Message check korte paro
                    return false;
                }
                finally
                {
                    // Resource clean up
                    msg.Dispose();
                    client.Dispose();
                }
            }
            else
            {
                return false;
            }
        }
        public static bool SendMailbynoreplymail(string toAddress, string fromAddress, string name, string subject, string body)
        {
            // Checking If it is a Local Server then don't Send Email
            // 0 is for Local Server
            // 1 is for Live Server

            // Checking If it is a Local Server then don't Send Email
            // 0 is for Local Server, 1 is for Live Server
            string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];

            if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "1")
            {
                // Web.config theke app password collect kora (Gmail-er 2FA App Password use koro)
                string emailPass = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailPass"];

                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(toAddress));
                msg.From = new MailAddress("bhpiadmission@gmail.com", "CRP Admission"); // Match with Credentials
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com"; // Updated to Gmail SMTP
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("bhpiadmission@gmail.com", emailPass);

                // Explicitly setting Security Protocol to TLS 1.2 or 1.3
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                try
                {
                    client.Send(msg);
                    return true;
                }
                catch (Exception ex)
                {
                    // Debugging er jonno ex.Message check korte paro
                    return false;
                }
                finally
                {
                    // Resource clean up
                    msg.Dispose();
                    client.Dispose();
                }
            }
            else
            {
                return false;
            }
        }

    }
}
