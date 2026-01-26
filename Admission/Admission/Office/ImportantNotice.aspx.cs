using Admission.App_Start;
using CommonUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class ImportantNotice : PageBase
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
                string path = Server.MapPath("~/ApplicationOfflineData/");
                using (StreamReader reader = new StreamReader(path + "importantNotice.json"))
                {
                    string json = reader.ReadToEnd();
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(json);

                    string Title = jsonObject["Title"].ToString();
                    //DateTime NoticeDate = DateTime.Parse(jsonObject["NoticeDate"].ToString("dd/MM/yyyy"));
                    string Details = jsonObject["Details"].ToString();
                    //DateTime DateFrom = DateTime.ParseExact(jsonObject["DateFrom"].ToString(), "dd/MM/yyyy", null);
                    //DateTime DateTo = DateTime.ParseExact(jsonObject["DateTo"].ToString(), "dd/MM/yyyy", null);

                    txtNoticeTitle.Text = Title;
                    txtNoticeDate.Text = jsonObject["NoticeDate"].ToString("dd/MM/yyyy");
                    txtNoticeDetails.Text = Details;
                    txtDateFrom.Text = jsonObject["DateFrom"].ToString("dd/MM/yyyy");
                    txtDateTo.Text = jsonObject["DateTo"].ToString("dd/MM/yyyy");

                }
            }
        }

        private void ClearMessage()
        {
            lblMessage.Text = string.Empty;
            messagePanel.CssClass = string.Empty;
            messagePanel.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DAL.NonDbObjects.ImportantNoticeObj obj = new DAL.NonDbObjects.ImportantNoticeObj();
            obj.Title = txtNoticeTitle.Text;
            obj.NoticeDate = DateTime.ParseExact(txtNoticeDate.Text, "dd/MM/yyyy", null);
            obj.Details = txtNoticeDetails.Text;
            obj.DateFrom = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", null);
            obj.DateTo = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", null);

            obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
            obj.DateCreated = DateTime.Now;
            try
            {
                string path = Server.MapPath("~/ApplicationOfflineData/");
                //using (StreamWriter file = File.CreateText(path + "importantNotice.json"))
                //{
                //    JsonSerializer serializer = new JsonSerializer();
                //    serializer.Serialize(file, obj);
                //}
                string json = string.Empty;
                dynamic jsonObject;
                using (StreamReader reader = new StreamReader(path + "importantNotice.json"))
                {
                    json = reader.ReadToEnd();
                    jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
                }

                jsonObject["Title"] = txtNoticeTitle.Text;
                jsonObject["NoticeDate"] = DateTime.ParseExact(txtNoticeDate.Text, "dd/MM/yyyy", null).ToString();
                jsonObject["Details"] = txtNoticeDetails.Text;
                jsonObject["DateFrom"] = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", null).ToString();
                jsonObject["DateTo"] = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", null).ToString();
                jsonObject["CreatedBy"] = -1;
                jsonObject["DateCreated"] = DateTime.Now.ToString();

                using (StreamWriter file = File.CreateText(path + "importantNotice.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, obj);

                    lblMessage.Text = "Updated successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;
                }
                
                //Todo: Save to LogDB or DB
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unsuccessful.";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }
    }
}