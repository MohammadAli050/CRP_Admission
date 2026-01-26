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
    public partial class DistrictForSeatPlanSetup : PageBase
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
                LoadDDL();
                LoadListView();
                DistrictForSeatPlanSetupID = 0;
            }

        }
        private void LoadDDL()
        {
            try
            {
                using (var db = new GeneralDataManager())
                {
                    DDLHelper.Bind<DAL.DistrictForSeatPlan>(ddlDistrict, db.AdmissionDB.DistrictForSeatPlans.Where(x => x.IsActive == true).ToList(), "Name", "ID", EnumCollection.ListItemType.District);
                }

            }
            catch (Exception ex)
            {
            }
        }
        private int DistrictForSeatPlanSetupID
        {
            get
            {
                if (ViewState["DistrictForSeatPlanSetupID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["DistrictForSeatPlanSetupID"].ToString());
            }
            set
            {
                ViewState["DistrictForSeatPlanSetupID"] = value;
            }
        }

        private void ClearFields()
        {
            ddlDistrict.SelectedValue = "-1";
            txtDistrictNumber.Text = string.Empty;
            txtDistrictPriority.Text = string.Empty;
            ckbxIsActive.Checked = false;
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.DistrictSeatPlanSetup> list = db.AdmissionDB.DistrictSeatPlanSetups.Where(d => d.IsActive == true).ToList();

                if (list != null)
                {
                    lvCampus.DataSource = list;
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvCampus.DataSource = null;
                    lblCount.Text = "0";
                }
                lvCampus.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessageSave.Text = null;

            DAL.DistrictSeatPlanSetup obj = new DAL.DistrictSeatPlanSetup();

            try
            {
                DAL.DistrictSeatPlanSetup existingCampus = null;
                if (DistrictForSeatPlanSetupID > 0)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            existingCampus = db.AdmissionDB.DistrictSeatPlanSetups.Find(DistrictForSeatPlanSetupID);
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Unable to find existing District";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }


                if (existingCampus != null && DistrictForSeatPlanSetupID > 0) //update existing
                {
                    obj = existingCampus;
                    obj.DistrictForSeatPlanId = Convert.ToInt32(ddlDistrict.SelectedValue);
                    obj.DistrictNumber = txtDistrictNumber.Text;
                    obj.DistrictPriority = Convert.ToInt32(txtDistrictPriority.Text);
                    obj.Attribute1 = ddlDistrict.SelectedItem.Text;
                    obj.IsActive = ckbxIsActive.Checked;

                    obj.ModifiedBy = uId;
                    obj.DateModified = DateTime.Now;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Update<DAL.DistrictSeatPlanSetup>(obj);
                        }

                        lblMessageSave.Text = "Updated Successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to update";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    DistrictForSeatPlanSetupID = 0;
                }
                else if (existingCampus == null && DistrictForSeatPlanSetupID == 0) //create new
                {
                    DAL.DistrictSeatPlanSetup model = new DAL.DistrictSeatPlanSetup();


                    model.DistrictForSeatPlanId = Convert.ToInt32(ddlDistrict.SelectedValue);
                    model.DistrictNumber = txtDistrictNumber.Text;
                    model.DistrictPriority = Convert.ToInt32(txtDistrictPriority.Text);
                    model.Attribute1 = ddlDistrict.SelectedItem.Text;
                    model.IsActive = ckbxIsActive.Checked;

                    model.CreatedBy = uId;
                    model.DateCreated = DateTime.Now;

                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Insert<DAL.DistrictSeatPlanSetup>(model);
                        }

                        lblMessageSave.Text = "District created successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to create new District";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    DistrictForSeatPlanSetupID = 0;
                }

            }
            catch (Exception)
            {
                lblMessage.Text = "Error saving/updating campus";
                lblMessage.ForeColor = Color.Crimson;
            }
            btnSubmit.Text = "Save";
            ClearFields();
            LoadListView();
        }

        protected void lvCampus_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.DistrictSeatPlanSetup obj = (DAL.DistrictSeatPlanSetup)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblDistrictName = (Label)currentItem.FindControl("lblDistrictName");
                Label lblDistrictNumber = (Label)currentItem.FindControl("lblDistrictNumber");
                Label lblDistrictPriority = (Label)currentItem.FindControl("lblDistrictPriority");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                //DAL.DistrictForSeatPlan dfsp = null;
                //using (var db = new GeneralDataManager())
                //{
                //    dfsp = db.AdmissionDB.DistrictForSeatPlans.Where(x => x.ID == obj.DistrictForSeatPlanId).FirstOrDefault();
                //}
                //if (dfsp != null)
                //{
                //    lblDistrictName.Text = dfsp.Name;
                //}
                //else
                //{
                //    lblDistrictName.Text = "";
                //}
                lblDistrictName.Text = obj.Attribute1;
                lblDistrictNumber.Text = obj.DistrictNumber;
                lblDistrictPriority.Text = obj.DistrictPriority.ToString();

                if (obj.IsActive == true)
                {
                    lblIsActive.Text = "YES";
                    lblIsActive.ForeColor = Color.Green;
                    lblIsActive.Font.Bold = true;
                }
                else
                {
                    lblIsActive.Text = "NO";
                    lblIsActive.ForeColor = Color.Crimson;
                }

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = obj.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = obj.ID.ToString();
            }
        }

        protected void lvCampus_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            lblMessageSave.Text = null;

            lblMessage.Text = null;

            if (e.CommandName == "Delete")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.DistrictSeatPlanSetups.Find(id);
                        db.Delete<DAL.DistrictSeatPlanSetup>(objToDelete);
                        //DistrictForSeatPlanSetupID = 0;
                    }
                    LoadListView();

                    lblMessage.Text = "Venue deleted successfully.";
                    lblMessage.ForeColor = Color.Green;
                    messagePanel.CssClass = "alert alert-warning";
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error deleting Venue.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }
            else if (e.CommandName == "Update")
            {
                DAL.DistrictSeatPlanSetup DistrictSeatPlanSetups = null;
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (var db = new OfficeDataManager())
                    {
                        DistrictSeatPlanSetups = db.AdmissionDB.DistrictSeatPlanSetups.Find(id);
                    }
                    if (DistrictSeatPlanSetups != null)
                    {
                        DistrictForSeatPlanSetupID = DistrictSeatPlanSetups.ID;
                        
                        ddlDistrict.SelectedValue = DistrictForSeatPlanSetupID.ToString();
                        txtDistrictNumber.Text=DistrictSeatPlanSetups.DistrictNumber.ToString();
                        txtDistrictPriority.Text=DistrictSeatPlanSetups.DistrictPriority.ToString();
                        //model.Attribute1 = ddlDistrict.SelectedItem.Text;
                        if (DistrictSeatPlanSetups.IsActive == true)
                        {
                            ckbxIsActive.Checked = true;
                        }
                        else
                        {
                            ckbxIsActive.Checked = false;
                        }
                        btnSubmit.Text = "Update";
                    }
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error getting campus for update.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }

            }
        }

        protected void lvCampus_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvCampus_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }
    }
}