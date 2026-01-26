using Admission.App_Start;
using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class GeneralDataLog : PageBase
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
                //LoadDDL();
                //LoadListView();

                //hfIsFilterClick.Value = "0";
            }
        }

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                if (string.IsNullOrEmpty(txtPaymentId.Text.Trim()) &&
                    string.IsNullOrEmpty(txtUser.Text.Trim()) &&
                    string.IsNullOrEmpty(txtStartDate.Text.Trim()) &&
                    string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    MessageView("Please provide an input field for load data!", "fail");
                    return;
                }

                if (!string.IsNullOrEmpty(txtStartDate.Text) && string.IsNullOrEmpty(txtEndDate.Text)) // StratDate = YES & EndDate = NO
                {
                    MessageView("Please Provide Start Date and End Date !!", "fail");
                    return;
                }
                else if (string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text)) // StratDate = NO & EndDate = YES
                {
                    MessageView("Please Provide Start Date and End Date !!", "fail");
                    return;
                }
                else { }


                long? paymentId = null;
                if (!string.IsNullOrEmpty(txtPaymentId.Text.Trim()))
                {
                    paymentId = Convert.ToInt64(txtPaymentId.Text.Trim());
                }

                string userName = null;
                if (!string.IsNullOrEmpty(txtUser.Text.Trim()))
                {
                    userName = txtUser.Text.Trim();
                }

                DateTime? stratDate = null;
                DateTime? endDate = null;
                if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    stratDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                    endDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                }
                


                List<DAL.SPGetGeneralDataLogInfo_Result> list = null;
                using (var db = new GeneralDataManager())
                {
                    list = db.AdmissionDB.SPGetGeneralDataLogInfo(null, null, paymentId, userName, stratDate, endDate).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    lvAdmSetup.DataSource = list;
                    lvAdmSetup.DataBind();
                }
                else
                {
                    lvAdmSetup.DataSource = null;
                    lvAdmSetup.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                txtPaymentId.Text = string.Empty;
                txtUser.Text = string.Empty;
                txtStartDate.Text = string.Empty;
                txtEndDate.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }
        

        protected void lvAdmSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetGeneralDataLogInfo_Result obj = (DAL.SPGetGeneralDataLogInfo_Result)((ListViewDataItem)(e.Item)).DataItem;

                //Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblDate = (Label)currentItem.FindControl("lblDate");
                Label lblUser = (Label)currentItem.FindControl("lblUser");
                Label lblEventName = (Label)currentItem.FindControl("lblEventName");
                Label lblOldData = (Label)currentItem.FindControl("lblOldData");
                Label lblNewData = (Label)currentItem.FindControl("lblNewData");


                try
                {
                    lblDate.Text = Convert.ToDateTime(obj.CreatedDate).ToString("dd-MM-yyyy HH:mm tt");
                }
                catch (Exception ex)
                {
                    lblDate.Text = "---";
                }

                lblUser.Text = obj.UserName;
                lblEventName.Text = obj.EventName;
                lblOldData.Text = obj.OldData;
                lblNewData.Text = obj.NewData;

            }
        }

        protected void lvAdmSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //if (e.CommandName == "Delete")
            //{
            //    try
            //    {
            //        long id = Convert.ToInt64(e.CommandArgument);

            //        using (var db = new OfficeDataManager())
            //        {
            //            var objectToRemove = db.AdmissionDB.AdmissionSetupOtherPrograms.Where(x => x.ID == id).FirstOrDefault();

            //            if (objectToRemove != null)
            //            {
            //                try
            //                {
            //                    if (File.Exists(Server.MapPath(objectToRemove.FileURL)))
            //                    {
            //                        try
            //                        {
            //                            //delete the original file
            //                            File.Delete(Server.MapPath(objectToRemove.FileURL));

            //                        }
            //                        catch (Exception ex)
            //                        {

            //                        }

            //                    }//end if
            //                }
            //                catch (Exception ex)
            //                {

            //                }


            //                db.Delete<DAL.AdmissionSetupOtherProgram>(objectToRemove);
            //                LoadData();

            //                MessageView("Data Deleted Successfully.", "success");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            //    }
            //}
        }

        protected void lvAdmSetup_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

        protected void lvAdmSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }











    }
}