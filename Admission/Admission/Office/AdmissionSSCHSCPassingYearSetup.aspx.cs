using Admission.App_Start;
using CommonUtility;
using DAL;
using DAL.ViewModels;
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
    public partial class AdmissionSSCHSCPassingYearSetup : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
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
                LoadListView();
            }
        }

        private int SSCHSCPassingYearSetupID
        {
            get
            {
                if (ViewState["SSCHSCPassingYearSetupID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["SSCHSCPassingYearSetupID"].ToString());
            }
            set
            {
                ViewState["SSCHSCPassingYearSetupID"] = value;
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

        private void ClearFields()
        {
            ddlExamType.SelectedValue = "-1";
            txtStarPassingYear.Text = string.Empty;
            txtEndPassingYear.Text = string.Empty;

            ckbxIsActive.Checked = false;
            SSCHSCPassingYearSetupID = 0;

            btnSave.Text = "Create Setup";
        }


        private void LoadListView()
        {
            List<DAL.SSCHSCPassingYearSetup> list = null;
            using (var db = new OfficeDataManager())
            {
                list = db.AdmissionDB.SSCHSCPassingYearSetups.OrderBy(a => a.ExamTypeId).ToList();

            }

            if (list != null && list.Count > 0)
            {
                lvSSCHSCPassingYearSetup.DataSource = list;
                lblCount.Text = list.Count().ToString();
            }
            else
            {
                lvSSCHSCPassingYearSetup.DataSource = null;
                lblCount.Text = "0";
            }
            lvSSCHSCPassingYearSetup.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            if ((Convert.ToInt32(ddlExamType.SelectedValue) > 0) 
                && (!string.IsNullOrEmpty(txtStarPassingYear.Text.Trim()) && Convert.ToInt32(txtStarPassingYear.Text.Trim()) > 0)
                && (!string.IsNullOrEmpty(txtEndPassingYear.Text.Trim()) && Convert.ToInt32(txtEndPassingYear.Text.Trim()) > 0)
                )
            {
                int id = -1;
                DAL.SSCHSCPassingYearSetup obj = new DAL.SSCHSCPassingYearSetup();

                obj.ExamTypeId = Convert.ToInt32(ddlExamType.SelectedValue);
                obj.StartYear = Convert.ToInt32(txtStarPassingYear.Text.Trim());
                obj.EndYear = Convert.ToInt32(txtEndPassingYear.Text.Trim());

                obj.IsActive = ckbxIsActive.Checked;

                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = uId;

                obj.ID = SSCHSCPassingYearSetupID;

                try
                {
                    if (obj.ID > 0) //update
                    {
                        int flag = 1;
                        #region Dublicate Update Check
                        using (var db = new OfficeDataManager())
                        {
                            var existData = db.AdmissionDB.SSCHSCPassingYearSetups.Where(x => x.ID != obj.ID && x.ExamTypeId == obj.ExamTypeId).FirstOrDefault();
                            if (existData != null)
                            {
                                flag = -1;
                            }
                        }
                        #endregion

                        if (flag > 0)
                        {

                            DAL.SSCHSCPassingYearSetup objectToUpdate = null;
                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    objectToUpdate = db.AdmissionDB.SSCHSCPassingYearSetups.Find(obj.ID);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageView("Error: " + ex.Message, "fail");
                                return;
                            }

                            if (objectToUpdate != null)
                            {
                                objectToUpdate.StartYear = Convert.ToInt32(txtStarPassingYear.Text.Trim());
                                objectToUpdate.EndYear = Convert.ToInt32(txtEndPassingYear.Text.Trim());
                                objectToUpdate.IsActive = ckbxIsActive.Checked;

                                objectToUpdate.ModifiedBy = uId;
                                objectToUpdate.ModifiedDate = DateTime.Now;
                            }


                            using (var db = new GeneralDataManager())
                            {
                                db.Update<DAL.SSCHSCPassingYearSetup>(objectToUpdate);
                            }


                            MessageView("Setup updated Successfully.", "success");
                        }
                        else
                        {
                            MessageView("A similar setup has already been created!", "fail");
                            return;

                        }
                    }
                    else //create new
                    {
                        
                        using (var db = new GeneralDataManager())
                        {
                            var existData = db.AdmissionDB.SSCHSCPassingYearSetups.Where(x => x.ExamTypeId == obj.ExamTypeId).FirstOrDefault();

                            if (existData != null)
                            {
                                id = -1;
                            }
                            else
                            {
                                db.Insert<DAL.SSCHSCPassingYearSetup>(obj);
                                id = obj.ID;
                            }
                            
                        }

                        if (id > 0)
                        {
                            MessageView("Setup saved Successfully.", "success");
                        }
                        else
                        {
                            MessageView("A similar setup has already been created!", "fail");
                            return;

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageView("Unable to save/update Setup. Error: " + ex.Message, "fail");
                    return;
                }


                btnSave.Text = "Create Setup";

                ClearFields();
                SSCHSCPassingYearSetupID = 0;
                LoadListView();
            }
            else
            {
                MessageView("Please Fill Up All Required Fields!", "fail");
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void lvSSCHSCPassingYearSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SSCHSCPassingYearSetup modelData = (DAL.SSCHSCPassingYearSetup)((ListViewDataItem)(e.Item)).DataItem;

                Label lblExamTypeName = (Label)currentItem.FindControl("lblExamTypeName");
                Label lblStartYear = (Label)currentItem.FindControl("lblStartYear");
                Label lblEndYear = (Label)currentItem.FindControl("lblEndYear");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblExamTypeName.Text = modelData.ExamTypeId == 1 ? "SSC" : "HSC";
                lblStartYear.Text = modelData.StartYear.ToString();
                lblEndYear.Text = modelData.EndYear.ToString();

                if (modelData.IsActive == true)
                {
                    lblIsActive.Text = "YES";
                    lblIsActive.ForeColor = System.Drawing.Color.Green;
                    lblIsActive.Attributes.CssStyle.Add("font-weight", "bold");
                }
                else
                {
                    lblIsActive.Text = "NO";
                    lblIsActive.ForeColor = System.Drawing.Color.Crimson;
                    lblIsActive.Attributes.CssStyle.Add("font-weight", "bold");
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = modelData.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = modelData.ID.ToString();

                //lnkDetails.CommandName = "Details";
                //lnkDetails.CommandArgument = notice.ID.ToString();
            }
        }

        protected void lvSSCHSCPassingYearSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                MessageView("", "clear");

                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.SSCHSCPassingYearSetups.Find(Convert.ToInt32(e.CommandArgument));
                        db.Delete<DAL.SSCHSCPassingYearSetup>(objToDelete);
                        SSCHSCPassingYearSetupID = 0;
                    }

                    LoadListView();

                    MessageView("Setup deleted successfully.", "success");
                }
                catch (Exception ex)
                {
                    MessageView("Unable to delete !!", "fail");
                }
            }
            else if (e.CommandName == "Update")
            {
                MessageView("", "clear");

                using (var db = new OfficeDataManager())
                {
                    var modelData = db.AdmissionDB.SSCHSCPassingYearSetups.Find(Convert.ToInt32(e.CommandArgument));
                    ClearFields();
                    if (modelData != null && modelData.ID > 0)
                    {
                        SSCHSCPassingYearSetupID = modelData.ID;

                        ddlExamType.SelectedValue = modelData.ExamTypeId.ToString();
                        txtStarPassingYear.Text = modelData.StartYear.ToString();
                        txtEndPassingYear.Text = modelData.EndYear.ToString();
                        
                        if (modelData.IsActive == true)
                        {
                            ckbxIsActive.Checked = true;
                        }
                        else
                        {
                            ckbxIsActive.Checked = false;
                        }

                        btnSave.Text = "Update Setup";
                    }
                }
                
            }
        }

        protected void lvSSCHSCPassingYearSetup_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

        protected void lvSSCHSCPassingYearSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }




    }
}