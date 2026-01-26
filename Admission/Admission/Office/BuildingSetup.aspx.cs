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
    public partial class BuildingSetup : PageBase
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
            }

        }

        private int CurrentBuildingID
        {
            get
            {
                if (ViewState["CurrentBuildingID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["CurrentBuildingID"].ToString());
            }
            set
            {
                ViewState["CurrentBuildingID"] = value;
            }
        }

        private void ClearFields()
        {

            ddlCampus.SelectedIndex = 0;
            txtBuildingName.Text = string.Empty;
            txtNumber.Text = "0";
            txtPriority.Text = "0";
            ckbxIsActive.Checked = false;
        }

        private void LoadDDL()
        {
            using(var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.Campu>(ddlCampus, db.AdmissionDB.Campus.Where(c => c.IsActive == true).ToList(), "CampusName", "ID", EnumCollection.ListItemType.Select);
            }
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.Building> list = db.AdmissionDB.Buildings.Where(d => d.IsActive == true).ToList();

                if (list != null)
                {
                    lvBuilding.DataSource = list.OrderBy(x => x.CampusID).ThenBy(x=> x.BuildingPriority).ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvBuilding.DataSource = null;
                    lblCount.Text = "0";
                }
                lvBuilding.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DAL.Building obj = new DAL.Building();

            try
            {
                DAL.Building existingBuilding = null;
                if (CurrentBuildingID > 0)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            existingBuilding = db.AdmissionDB.Buildings.Find(CurrentBuildingID);
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Unable to find existing building";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }


                if (existingBuilding != null && CurrentBuildingID > 0) //update existing
                {
                    obj = existingBuilding;

                    obj.CampusID = Convert.ToInt32(ddlCampus.SelectedValue);
                    obj.BuildingNumber = txtNumber.Text;
                    obj.BuildingPriority = Convert.ToInt32(txtPriority.Text);
                    obj.BuildingName = txtBuildingName.Text;
                    obj.IsActive = ckbxIsActive.Checked;

                    obj.ModifiedBy = uId;
                    obj.DateModified = DateTime.Now;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Update<DAL.Building>(obj);
                        }

                        lblMessageSave.Text = "Updated Successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to update";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    CurrentBuildingID = 0;
                }
                else if (existingBuilding == null && CurrentBuildingID == 0) //create new
                {
                    obj = null;
                    obj = new DAL.Building();

                    obj.CampusID = Convert.ToInt32(ddlCampus.SelectedValue);
                    obj.BuildingNumber = txtNumber.Text;
                    obj.BuildingPriority = Convert.ToInt32(txtPriority.Text);
                    obj.BuildingName = txtBuildingName.Text;
                    obj.IsActive = ckbxIsActive.Checked;

                    obj.CreatedBy = uId;
                    obj.DateCreated = DateTime.Now;

                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Insert<DAL.Building>(obj);
                        }

                        lblMessageSave.Text = "Building created successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to create new building";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    CurrentBuildingID = 0;
                }

            }
            catch (Exception)
            {
                lblMessage.Text = "Error saving/updating building";
                lblMessage.ForeColor = Color.Crimson;
            }
            btnSubmit.Text = "Save";
            ClearFields();
            LoadListView();
        }

        protected void lvBuilding_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Building buil = (DAL.Building)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblBuildingName = (Label)currentItem.FindControl("lblBuildingName");
                Label lblCampusName = (Label)currentItem.FindControl("lblCampusName");
                Label lblNumber = (Label)currentItem.FindControl("lblNumber");
                Label lblPriority = (Label)currentItem.FindControl("lblPriority");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                lblBuildingName.Text = buil.BuildingName;
                lblNumber.Text = buil.BuildingNumber;
                lblPriority.Text = buil.BuildingPriority.ToString();

                try
                {
                    DAL.Campu campus = null;
                    int campusId = Convert.ToInt32(buil.CampusID);
                    using(var db = new OfficeDataManager())
                    {
                        campus = db.AdmissionDB.Campus.Find(campusId);
                    }

                    if(campus != null)
                    {
                        lblCampusName.Text = campus.CampusName;
                    }
                    else
                    {
                        lblCampusName.Text = " - ";
                    }
                }
                catch (Exception)
                {

                }

                if (buil.IsActive == true)
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
                lnkEdit.CommandArgument = buil.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = buil.ID.ToString();
            }
        }

        protected void lvBuilding_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.Buildings.Find(id);
                        db.Delete<DAL.Building>(objToDelete);
                        CurrentBuildingID = 0;
                    }
                    LoadListView();

                    lblMessage.Text = "Building deleted successfully.";
                    lblMessage.ForeColor = Color.Green;
                    messagePanel.CssClass = "alert alert-warning";
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error deleting building.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }
            else if (e.CommandName == "Update")
            {
                DAL.Building building = null;
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (var db = new OfficeDataManager())
                    {
                        building = db.AdmissionDB.Buildings.Find(id);
                    }
                    if (building != null)
                    {
                        CurrentBuildingID = building.ID;

                        txtBuildingName.Text = building.BuildingName;
                        txtNumber.Text = building.BuildingNumber;
                        txtPriority.Text = building.BuildingPriority.ToString();
                        ddlCampus.SelectedValue = building.CampusID.ToString();
                        if (building.IsActive == true)
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
                    lblMessage.Text = "Error getting building for update.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }

            }
        }

        protected void lvBuilding_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvBuilding_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }
    }
}