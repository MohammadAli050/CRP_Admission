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
    public partial class UnitWiseRoomSetup : PageBase
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

                ddlProgBuildingPrior.Items.Add(new ListItem("Select", "-1"));
                ddlProgCampusPrior.Items.Add(new ListItem("Select", "-1"));
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

            ddlProgCampusPrior.Items.Clear();

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
                        DDLHelper.Bind<DAL.ProgramCampusPriority>(ddlProgCampusPrior,
                            db.AdmissionDB.ProgramCampusPriorities
                                .Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == admSetup.AcaCalID)
                                .OrderBy(c => c.ProgramDistrictPriorityId).ThenBy(x => x.CampusPriority).ToList(),
                            "CampusName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
            }
            else
            {
                ddlProgCampusPrior.Items.Clear();
                ddlProgCampusPrior.Items.Add(new ListItem("Select", "-1"));
            }
        }

        protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            long progCampusPriorId = -1;
            progCampusPriorId = Convert.ToInt64(ddlProgCampusPrior.SelectedValue);

            ddlProgBuildingPrior.Items.Clear();

            if (progCampusPriorId > 0)
            {
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind<DAL.ProgramBuildingPriority>(ddlProgBuildingPrior,
                        db.AdmissionDB.ProgramBuildingPriorities
                            .Where(c => c.ProgramCampusPriorityID == progCampusPriorId)
                            .OrderBy(c => c.BuildingPriority).ToList(),
                        "BuildingName", "ID", EnumCollection.ListItemType.Select);
                }
            }
            else
            {
                ddlProgBuildingPrior.Items.Clear();
                ddlProgBuildingPrior.Items.Add(new ListItem("Select", "-1"));
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

            long progCampPriorId = -1;
            progCampPriorId = Convert.ToInt64(ddlProgCampusPrior.SelectedValue);

            long progBuilPriorId = -1;
            progBuilPriorId = Convert.ToInt32(ddlProgBuildingPrior.SelectedValue);

            if (admUnitId > 0 && progCampPriorId > 0 && progBuilPriorId > 0)
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

                List<DAL.ProgramRoomPriority> list = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            list = db.AdmissionDB.ProgramRoomPriorities
                                .Where(c => c.AcaCalID == admSetup.AcaCalID &&
                                    c.AdmissionUnitID == admUnitId &&
                                    c.ProgBuildPriorityID == progBuilPriorId)
                                .ToList();
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Error getting all room priorities.";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                    }

                }

                if (list != null)
                {
                    if (list.Count() > 0)
                    {
                        gvProgRoomPrior.DataSource = list.OrderBy(c => c.BuildingID).ThenBy(x=> x.RoomID).ThenBy(x=> x.Priority).ToList();
                    }
                }
                else
                {
                    gvProgRoomPrior.DataSource = null;
                }
                gvProgRoomPrior.DataBind();
                GridRebind();

            }//end if(admUnitId > 0)
            else if (admUnitId > 0 && progCampPriorId < 0 && progBuilPriorId < 0)
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

                List<DAL.ProgramRoomPriority> list = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            list = db.AdmissionDB.ProgramRoomPriorities
                                .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId)
                                .ToList();
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Error getting all room priorities(1).";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                    }

                }

                if (list != null)
                {
                    if (list.Count() > 0)
                    {
                        gvProgRoomPrior.DataSource = list.OrderBy(c => c.Priority).ToList();
                    }
                }
                else
                {
                    gvProgRoomPrior.DataSource = null;
                }
                gvProgRoomPrior.DataBind();
                GridRebind();
            }

        }

        private void GridRebind()
        {
            try
            {
                foreach (GridViewRow row in gvProgRoomPrior.Rows)
                {
                    DropDownList ddlPriority = (DropDownList)row.FindControl("ddlRoomPriority");
                    Label lblProgramBuildingPriorityId = (Label)row.FindControl("lblProgramBuildingPriorityId");
                    Label lblVenueName = (Label)row.FindControl("lblVenueName");
                    Label lblPriority = (Label)row.FindControl("hfRoomPriority");
                    ddlPriority.AppendDataBoundItems = true;
                    for (int priority = 1; priority <= 251; priority++)
                    {
                        ddlPriority.Items.Add(new ListItem(Convert.ToString(priority), Convert.ToString(priority)));
                    }
                    ddlPriority.DataBind();
                    ddlPriority.SelectedValue = lblPriority.Text;


                    TextBox txtRoomCapacity = (TextBox)row.FindControl("txtRoomCapacity");
                    Label lblRoomCapacity = (Label)row.FindControl("hfRoomCapacity");
                    txtRoomCapacity.Text = lblRoomCapacity.Text;


                    long programBuildingPriorityId = Convert.ToInt64(lblProgramBuildingPriorityId.Text);
                    using (var db = new GeneralDataManager())
                    {
                        var districtName = (from a in db.AdmissionDB.ProgramBuildingPriorities
                                            join b in db.AdmissionDB.ProgramCampusPriorities on a.ProgramCampusPriorityID equals b.ID
                                            join c in db.AdmissionDB.ProgramDistrictPriorities on b.ProgramDistrictPriorityId equals c.ID
                                            where a.ID == programBuildingPriorityId
                                            select new { c.DistrictName }).FirstOrDefault();
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
            progCampPriorId = Convert.ToInt64(ddlProgCampusPrior.SelectedValue);

            long progBuilPriorId = -1;
            progBuilPriorId = Convert.ToInt64(ddlProgBuildingPrior.SelectedValue);

            if (admUnitId > 0 && progBuilPriorId > 0) //if a single building is selected
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

                DAL.ProgramBuildingPriority progBuilPriorObj = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            progBuilPriorObj = db.AdmissionDB.ProgramBuildingPriorities.Find(progBuilPriorId);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                List<DAL.Room> roomList = null;
                if (progBuilPriorObj != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            roomList = db.AdmissionDB.Rooms
                                .Where(c => c.BuildingID == progBuilPriorObj.BuildingID)
                                .ToList();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                List<DAL.ProgramRoomPriority> existingPrgRoomPrList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        existingPrgRoomPrList = db.AdmissionDB.ProgramRoomPriorities
                            .Where(c => c.AcaCalID == admSetup.AcaCalID &&
                                    c.AdmissionUnitID == admUnitId &&
                                    c.ProgBuildPriorityID == progBuilPriorObj.ID).ToList();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                if (existingPrgRoomPrList != null)
                {
                    if (existingPrgRoomPrList.Count() > 0)
                    {
                        foreach (DAL.ProgramRoomPriority item in existingPrgRoomPrList)
                        {
                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Delete<DAL.ProgramRoomPriority>(item);
                                }
                            }
                            catch (Exception)
                            {
                                lblMessage.Text = "Error deleting existing buildings.";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }
                    }
                }


                if (roomList != null)
                {
                    if (roomList.Count() > 0)
                    {
                        foreach (var item in roomList)
                        {
                            DAL.ProgramRoomPriority obj = new DAL.ProgramRoomPriority();
                            obj.AcaCalID = admSetup.AcaCalID;
                            obj.AdmissionUnitID = admUnitId;
                            obj.AdmissionUnitname = ddlAdmUnit.SelectedItem.Text;
                            obj.BuildingID = item.BuildingID;
                            try
                            {
                                DAL.Building buildingObj = null;
                                using (var db = new OfficeDataManager())
                                {
                                    buildingObj = db.AdmissionDB.Buildings.Find(item.BuildingID);
                                }
                                if (buildingObj != null)
                                {
                                    obj.BuildingName = buildingObj.BuildingName;
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            obj.Capacity = item.Capacity;
                            obj.EndRoll = null;
                            obj.FloorNo = item.FloorNumber;
                            obj.Priority = item.RoomPriority;
                            obj.ProgBuildPriorityID = progBuilPriorObj.ID;
                            obj.ProgramID = null;
                            obj.ProgramName = null;
                            obj.RoomID = item.ID;
                            obj.RoomName = item.RoomName;
                            obj.RoomNumber = item.RoomNumber;
                            obj.StartRoll = null;

                            obj.CreatedBy = uId;
                            obj.DateCreated = DateTime.Now;

                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Insert<DAL.ProgramRoomPriority>(obj);
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
                }
            } //if(admUnitId > 0 && progCampPriorId > 0)
            else if (admUnitId > 0 && progBuilPriorId < 0) //if all buil is selected
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

                List<DAL.Room> roomList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        roomList = db.AdmissionDB.Rooms.ToList();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                List<DAL.ProgramRoomPriority> existingPrgRoomPrList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        existingPrgRoomPrList = db.AdmissionDB.ProgramRoomPriorities
                            .Where(c => c.AcaCalID == admSetup.AcaCalID &&
                                    c.AdmissionUnitID == admUnitId).ToList();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                if (existingPrgRoomPrList != null)
                {
                    if (existingPrgRoomPrList.Count() > 0)
                    {
                        foreach (DAL.ProgramRoomPriority item in existingPrgRoomPrList)
                        {
                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Delete<DAL.ProgramRoomPriority>(item);
                                }
                            }
                            catch (Exception)
                            {
                                lblMessage.Text = "Error deleting existing buildings.";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }
                    }
                }

                if (roomList != null)
                {
                    if (roomList.Count() > 0)
                    {
                        foreach (var item in roomList)
                        {
                            DAL.ProgramRoomPriority obj = new DAL.ProgramRoomPriority();
                            obj.AcaCalID = admSetup.AcaCalID;
                            obj.AdmissionUnitID = admUnitId;
                            obj.AdmissionUnitname = ddlAdmUnit.SelectedItem.Text;
                            obj.BuildingID = item.BuildingID;
                            try
                            {
                                DAL.Building buildingObj = null;
                                using (var db = new OfficeDataManager())
                                {
                                    buildingObj = db.AdmissionDB.Buildings.Find(item.BuildingID);
                                }
                                if (buildingObj != null)
                                {
                                    obj.BuildingName = buildingObj.BuildingName;
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            obj.Capacity = item.Capacity;
                            obj.EndRoll = null;
                            obj.FloorNo = item.FloorNumber;
                            obj.Priority = item.RoomPriority;
                            //obj.ProgBuildPriorityID = progBuilPriorObj.ID;

                            try
                            {
                                DAL.ProgramBuildingPriority prgBuildingPrior = null;
                                using(var db = new OfficeDataManager())
                                {
                                    prgBuildingPrior = db.AdmissionDB.ProgramBuildingPriorities
                                        .Where(c => c.AcaCalID == admSetup.AcaCalID &&
                                            c.AdmissionUnitID == admUnitId &&
                                            c.BuildingID == item.BuildingID).First();
                                }
                                if(prgBuildingPrior != null)
                                {
                                    obj.ProgBuildPriorityID = prgBuildingPrior.ID;
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                            obj.ProgramID = null;
                            obj.ProgramName = null;
                            obj.RoomID = item.ID;
                            obj.RoomName = item.RoomName;
                            obj.RoomNumber = item.RoomNumber;
                            obj.StartRoll = null;

                            obj.CreatedBy = uId;
                            obj.DateCreated = DateTime.Now;

                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Insert<DAL.ProgramRoomPriority>(obj);
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

            DAL.ProgramRoomPriority obj = null;
            if (id > 0)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        obj = db.AdmissionDB.ProgramRoomPriorities.Find(id);
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
                DropDownList ddlPriority = linkButton.NamingContainer.FindControl("ddlRoomPriority") as DropDownList;
                obj.Priority = Convert.ToInt32(ddlPriority.SelectedValue);

                TextBox txtRoomCapacity = linkButton.NamingContainer.FindControl("txtRoomCapacity") as TextBox;
                obj.Capacity = Convert.ToInt32(txtRoomCapacity.Text);
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.ProgramRoomPriority>(obj);
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