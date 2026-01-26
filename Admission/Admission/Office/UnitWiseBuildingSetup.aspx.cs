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
    public partial class UnitWiseBuildingSetup : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                LoadDDL();
            }

        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
            }
        }

        protected void ddlAdmUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            long admUnitId = -1;
            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

            if (admUnitId > 0)
            {
                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                if (admSetup != null)
                {
                    using (var db = new OfficeDataManager())
                    {
                        DDLHelper.Bind<DAL.ProgramCampusPriority>(ddlCampus,
                            db.AdmissionDB.ProgramCampusPriorities
                                .Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == admSetup.AcaCalID)
                                .OrderBy(c => c.ProgramDistrictPriorityId).ThenBy(x=> x.CampusPriority).ToList(),
                            "CampusName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

            long progCampPriorId = -1;
            progCampPriorId = Convert.ToInt64(ddlCampus.SelectedValue);

            if (admUnitId > 0 && progCampPriorId > 0)
            {

                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                List<DAL.ProgramBuildingPriority> list = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            list = db.AdmissionDB.ProgramBuildingPriorities
                                .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId && c.ProgramCampusPriorityID == progCampPriorId)
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting all building priorities. " + ex.Message.ToString();
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                    }

                }

                if (list != null)
                {
                    if (list.Count() > 0)
                    {
                        gvProgBuildingPrior.DataSource = list.OrderBy(c => c.BuildingPriority).ToList();
                    }
                }
                else
                {
                    gvProgBuildingPrior.DataSource = null;
                }
                gvProgBuildingPrior.DataBind();
                GridRebind();

            }//end if(admUnitId > 0)
            else if(admUnitId > 0 && progCampPriorId < 0)
            {
                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                List<DAL.ProgramBuildingPriority> list = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            list = db.AdmissionDB.ProgramBuildingPriorities
                                .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId)
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting all building priorities(1)." + ex.Message.ToString();
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                    }

                }

                if (list != null)
                {
                    if (list.Count() > 0)
                    {
                        gvProgBuildingPrior.DataSource = list.OrderBy(c => c.CampusID).ThenBy(x=> x.BuildingID).ThenBy(x=> x.BuildingPriority).ToList();
                    }
                }
                else
                {
                    gvProgBuildingPrior.DataSource = null;
                }
                gvProgBuildingPrior.DataBind();
                GridRebind();
            }
        }

        private void GridRebind()
        {
            try
            {
                foreach (GridViewRow row in gvProgBuildingPrior.Rows)
                {
                    DropDownList ddlPriority = (DropDownList)row.FindControl("ddlBuildingPriority");
                    Label lblProgramCampusPriorityId = (Label)row.FindControl("lblProgramCampusPriorityId");
                    Label lblVenueName = (Label)row.FindControl("lblVenueName");
                    Label lblPriority = (Label)row.FindControl("hfBuildingPriority");

                    ddlPriority.AppendDataBoundItems = true;
                    for (int priority = 1; priority <= 50; priority++)
                    {
                        ddlPriority.Items.Add(new ListItem(Convert.ToString(priority), Convert.ToString(priority)));
                    }
                    ddlPriority.DataBind();
                    ddlPriority.SelectedValue = lblPriority.Text;

                    long programCampusPrioritieId = Convert.ToInt64(lblProgramCampusPriorityId.Text);
                    using (var db = new GeneralDataManager())
                    {
                        var districtName = (from a in db.AdmissionDB.ProgramCampusPriorities
                                            join b in db.AdmissionDB.ProgramDistrictPriorities on a.ProgramDistrictPriorityId equals b.ID
                                            where a.ID == programCampusPrioritieId
                                            select new { b.DistrictName }).FirstOrDefault();
                        lblVenueName.Text = districtName.DistrictName;

                    }


                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

            long progCampPriorId = -1;
            progCampPriorId = Convert.ToInt64(ddlCampus.SelectedValue);

            if (admUnitId > 0 && progCampPriorId > 0) //if a single campus is selected
            {
                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting admission setup " + ex.Message + "; " + ex.InnerException.Message;
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }

                DAL.ProgramCampusPriority progCampPriorObj = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            progCampPriorObj = db.AdmissionDB.ProgramCampusPriorities.Find(progCampPriorId);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting Campus Priorities " + ex.Message + "; " + ex.InnerException.Message;
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }

                List<DAL.Building> buildingList = null;
                if (progCampPriorObj != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            buildingList = db.AdmissionDB.Buildings
                                .Where(c => c.CampusID == progCampPriorObj.CampusID)
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting buildings " + ex.Message + "; " + ex.InnerException.Message;
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }

                List<DAL.ProgramBuildingPriority> existingPrgBldPrList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        existingPrgBldPrList = db.AdmissionDB.ProgramBuildingPriorities
                            .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId && c.ProgramCampusPriorityID == progCampPriorObj.ID).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting existing building priorities " + ex.Message + "; " + ex.InnerException.Message;
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                if (existingPrgBldPrList != null)
                {
                    if (existingPrgBldPrList.Count() > 0)
                    {
                        foreach (DAL.ProgramBuildingPriority item in existingPrgBldPrList)
                        {
                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Delete<DAL.ProgramBuildingPriority>(item);
                                }
                            }
                            catch (Exception ex)
                            {
                                lblMessage.Text = "Error deleting existing buildings. " + ex.Message + "; " + ex.InnerException.Message;
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }
                    }
                }


                if (buildingList != null)
                {
                    if (buildingList.Count() > 0)
                    {
                        foreach (var item in buildingList)
                        {
                            DAL.ProgramBuildingPriority obj = new DAL.ProgramBuildingPriority();
                            obj.AcaCalID = admSetup.AcaCalID;
                            obj.AdmissionUnitID = admUnitId;
                            obj.AdmissionUnitName = ddlAdmUnit.SelectedItem.Text;
                            obj.BuildingID = item.ID;
                            obj.BuildingName = item.BuildingName;
                            obj.BuildingPriority = item.BuildingPriority;
                            obj.CampusID = item.CampusID;
                            try
                            {
                                DAL.Campu campusObj = null;
                                using (var db = new OfficeDataManager())
                                {
                                    campusObj = db.AdmissionDB.Campus.Find(item.CampusID);
                                }
                                if (campusObj != null)
                                {
                                    obj.CampusName = campusObj.CampusName;
                                }
                            }
                            catch (Exception ex)
                            {
                                lblMessage.Text = "Error getting campus. " + ex.Message + "; " + ex.InnerException.Message;
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                            obj.ProgramCampusPriorityID = progCampPriorObj.ID;
                            obj.ProgramID = null;
                            obj.ProgramName = null;

                            obj.CreatedBy = uId;
                            obj.DateCreated = DateTime.Now;

                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Insert<DAL.ProgramBuildingPriority>(obj);
                                }
                                lblMessage.Text = "Generated";
                                messagePanel.CssClass = "alert alert-success";
                            }
                            catch (Exception ex)
                            {
                                lblMessage.Text = "Error generating building for faculty. " + ex.Message + "; " + ex.InnerException.Message;
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }
                    }
                }
            } //if(admUnitId > 0 && progCampPriorId > 0)
            else if (admUnitId > 0 && progCampPriorId < 0) //if all campus is selected
            {
                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                
                List<DAL.Building> buildingList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        buildingList = db.AdmissionDB.Buildings.ToList();
                    }
                }
                catch (Exception)
                {

                    throw;
                }


                List<DAL.ProgramBuildingPriority> existingPrgBldPrList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        existingPrgBldPrList = db.AdmissionDB.ProgramBuildingPriorities
                            .Where(c => c.AcaCalID == admSetup.AcaCalID && 
                                c.AdmissionUnitID == admUnitId).ToList();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                if (existingPrgBldPrList != null)
                {
                    if (existingPrgBldPrList.Count() > 0)
                    {
                        foreach (DAL.ProgramBuildingPriority item in existingPrgBldPrList)
                        {
                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Delete<DAL.ProgramBuildingPriority>(item);
                                }
                            }
                            catch (Exception)
                            {
                                lblMessage.Text = "Error deleting all existing buildings.";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }
                    }
                }

                if (buildingList != null)
                {
                    if (buildingList.Count() > 0)
                    {
                        foreach (var item in buildingList)
                        {
                            DAL.ProgramBuildingPriority obj = new DAL.ProgramBuildingPriority();
                            obj.AcaCalID = admSetup.AcaCalID;
                            obj.AdmissionUnitID = admUnitId;
                            obj.AdmissionUnitName = ddlAdmUnit.SelectedItem.Text;
                            obj.BuildingID = item.ID;
                            obj.BuildingName = item.BuildingName;
                            obj.BuildingPriority = item.BuildingPriority;
                            obj.CampusID = item.CampusID;
                            try
                            {
                                DAL.Campu campusObj = null;
                                using (var db = new OfficeDataManager())
                                {
                                    campusObj = db.AdmissionDB.Campus.Find(item.CampusID);
                                }
                                if (campusObj != null)
                                {
                                    obj.CampusName = campusObj.CampusName;
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            //obj.ProgramCampusPriorityID = ;

                            try
                            {
                                DAL.ProgramCampusPriority prgCampusPrior = null;
                                using(var db = new OfficeDataManager())
                                {
                                    prgCampusPrior = db.AdmissionDB.ProgramCampusPriorities
                                        .Where(c => c.AcaCalID == admSetup.AcaCalID 
                                            && c.AdmissionUnitID == admUnitId 
                                            && c.CampusID == item.CampusID).First();
                                }
                                if(prgCampusPrior != null)
                                {
                                    obj.ProgramCampusPriorityID = prgCampusPrior.ID;
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                            obj.ProgramID = null;
                            obj.ProgramName = null;

                            obj.CreatedBy = uId;
                            obj.DateCreated = DateTime.Now;

                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Insert<DAL.ProgramBuildingPriority>(obj);
                                }
                                lblMessage.Text = "Generated";
                                messagePanel.CssClass = "alert alert-success";
                            }
                            catch (Exception)
                            {
                                lblMessage.Text = "Error generating building for faculty.";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }
                    }
                    lblMessage.Text = "Generated.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-success";
                }
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton linkButton = new LinkButton();
            linkButton = (LinkButton)sender;
            long id = Convert.ToInt64(linkButton.CommandArgument);

            DAL.ProgramBuildingPriority obj = null;
            if (id > 0)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        obj = db.AdmissionDB.ProgramBuildingPriorities.Find(id);
                    }
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error getting building for update.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }

            if (obj != null)
            {
                DropDownList ddlPriority = linkButton.NamingContainer.FindControl("ddlBuildingPriority") as DropDownList;
                obj.BuildingPriority = Convert.ToInt32(ddlPriority.SelectedValue);
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.ProgramBuildingPriority>(obj);
                    }
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error updating building priority.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }
        }
    }
}