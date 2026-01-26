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
    public partial class CampusSetup : PageBase
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

        public class DistrictSeatPlanSetupVM
        {
            public int ID { get; set; }
            public string DistrictName { get; set; }
        }

        private void LoadDDL()
        {
            List<DistrictSeatPlanSetupVM> listFinal = new List<DistrictSeatPlanSetupVM>();
            using (var db = new OfficeDataManager())
            {

                int acaCalId = db.AdmissionDB.AdmissionSetups.Where(x => x.IsActive == true && x.EducationCategoryID == 4).Select(x => x.AcaCalID).FirstOrDefault();

                List<DAL.DistrictSeatPlanSetup> list = db.AdmissionDB.DistrictSeatPlanSetups.Where(c => c.IsActive == true).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (var tData in list)
                    {
                        DAL.ViewModels.DistrictSeatLimitVM districtName = db.GetDistrictSeatLimitVM(acaCalId).Where(x => x.DistrictId == tData.DistrictForSeatPlanId).FirstOrDefault();

                        if( districtName!= null)
                        {
                            listFinal.Add(new DistrictSeatPlanSetupVM()
                            {
                                ID = tData.ID,
                                DistrictName = districtName.DistrictName
                            });
                        }
                        
                    }
                }

                DDLHelper.Bind<DistrictSeatPlanSetupVM>(ddlDistrictSeatPlanSetup, listFinal, "DistrictName", "ID", EnumCollection.ListItemType.Select);
            }
        }

        private int CurrentCampusID
        {
            get
            {
                if (ViewState["CurrentCampusID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["CurrentCampusID"].ToString());
            }
            set
            {
                ViewState["CurrentCampusID"] = value;
            }
        }

        private void ClearFields()
        {
            txtAddress.Text = string.Empty;
            txtCampusName.Text = string.Empty;
            txtCampusNumber.Text = "0";
            txtPriority.Text = "0";
            ckbxIsActive.Checked = false;
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.Campu> list = db.AdmissionDB.Campus.Where(d => d.IsActive == true).ToList();

                if (list != null)
                {
                    lvCampus.DataSource = list.OrderBy(x=> x.DistrictSeatPlanSetupId).ThenBy(x=> x.ID);
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

            DAL.Campu obj = new DAL.Campu();

            try
            {
                DAL.Campu existingCampus = null;
                if (CurrentCampusID > 0)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            existingCampus = db.AdmissionDB.Campus.Find(CurrentCampusID);
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Unable to find existing campus";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }


                if (existingCampus != null && CurrentCampusID > 0) //update existing
                {
                    obj = existingCampus;
                    if (!string.IsNullOrEmpty(txtAddress.Text))
                    {
                        obj.AddressLine = txtAddress.Text;
                    }
                    obj.CampusName = txtCampusName.Text;
                    obj.CampusNumber = txtCampusNumber.Text;
                    obj.CampusPriority = Convert.ToInt32(txtPriority.Text);
                    obj.DistrictSeatPlanSetupId = Convert.ToInt32(ddlDistrictSeatPlanSetup.SelectedValue);
                    obj.IsActive = ckbxIsActive.Checked;

                    obj.ModifiedBy = uId;
                    obj.DateModified = DateTime.Now;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Update<DAL.Campu>(obj);
                        }

                        lblMessageSave.Text = "Updated Successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to update";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    CurrentCampusID = 0;
                }
                else if (existingCampus == null && CurrentCampusID == 0) //create new
                {
                    obj = null;
                    obj = new DAL.Campu();

                    if (!string.IsNullOrEmpty(txtAddress.Text))
                    {
                        obj.AddressLine = txtAddress.Text;
                    }
                    obj.CampusName = txtCampusName.Text;
                    obj.CampusNumber = txtCampusNumber.Text;
                    obj.CampusPriority = Convert.ToInt32(txtPriority.Text);
                    obj.DistrictSeatPlanSetupId = Convert.ToInt32(ddlDistrictSeatPlanSetup.SelectedValue);
                    obj.IsActive = ckbxIsActive.Checked;

                    obj.CreatedBy = uId;
                    obj.DateCreated = DateTime.Now;

                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Insert<DAL.Campu>(obj);
                        }

                        lblMessageSave.Text = "Campus created successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to create new campus";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    CurrentCampusID = 0;
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
                DAL.Campu campus = (DAL.Campu)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblDistrictName = (Label)currentItem.FindControl("lblDistrictName");
                Label lblCampusname = (Label)currentItem.FindControl("lblCampusname");
                Label lblCampusNumber = (Label)currentItem.FindControl("lblCampusNumber");
                Label lblPriority = (Label)currentItem.FindControl("lblPriority");
                Label lblAddress = (Label)currentItem.FindControl("lblAddress");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                lblCampusname.Text = campus.CampusName;
                lblCampusNumber.Text = campus.CampusNumber;
                lblPriority.Text = campus.CampusPriority.ToString();
                lblAddress.Text = campus.AddressLine;

                if (campus.IsActive == true)
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

                try
                {
                    using (var db = new GeneralDataManager())
                    {
                        var districtName = (from a in db.AdmissionDB.Campus
                                            join b in db.AdmissionDB.DistrictSeatPlanSetups on a.DistrictSeatPlanSetupId equals b.ID
                                            where a.ID == campus.ID
                                            select new { b.Attribute1 }).FirstOrDefault();
                        lblDistrictName.Text = districtName.Attribute1;

                    }
                }
                catch (Exception ex)
                {
                    
                }



                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = campus.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = campus.ID.ToString();
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
                        var objToDelete = db.AdmissionDB.Campus.Find(id);
                        db.Delete<DAL.Campu>(objToDelete);
                        CurrentCampusID = 0;
                    }
                    LoadListView();

                    lblMessage.Text = "Campus deleted successfully.";
                    lblMessage.ForeColor = Color.Green;
                    messagePanel.CssClass = "alert alert-warning";
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error deleting campus.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }
            else if (e.CommandName == "Update")
            {
                DAL.Campu campus = null;
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (var db = new OfficeDataManager())
                    {
                        campus = db.AdmissionDB.Campus.Find(id);
                    }
                    if(campus != null)
                    {
                        CurrentCampusID = campus.ID;

                        txtAddress.Text = campus.AddressLine;
                        txtCampusName.Text = campus.CampusName;
                        txtCampusNumber.Text = campus.CampusNumber;
                        txtPriority.Text = campus.CampusPriority.ToString();

                        try
                        {
                            if (!string.IsNullOrEmpty(campus.DistrictSeatPlanSetupId.ToString()))
                            {
                                ddlDistrictSeatPlanSetup.SelectedValue = campus.DistrictSeatPlanSetupId.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        
                        if (campus.IsActive == true)
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