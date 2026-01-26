using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Admission.Admission.Office
{
    public partial class RoomSetup : PageBase
    {

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
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

        private int CurrentRoomID
        {
            get
            {
                if (ViewState["CurrentRoomID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["CurrentRoomID"].ToString());
            }
            set
            {
                ViewState["CurrentRoomID"] = value;
            }
        }

        private void ClearFields()
        {

            ddlBuilding.SelectedIndex = 0;
            txtFloorNumber.Text = string.Empty;
            txtPriority.Text = "0";
            txtRoomCapacity.Text = "0";
            txtRoomName.Text = string.Empty;
            txtRoomNumber.Text = string.Empty;
            ckbxIsActive.Checked = false;
        }

        private void LoadDDL()
        {
            //using (var db = new OfficeDataManager())
            //{
            //    DDLHelper.Bind<DAL.Building>(ddlBuilding, db.AdmissionDB.Buildings.Where(c => c.IsActive == true).ToList();, "BuildingName", "ID", EnumCollection.ListItemType.Select);
            //}
            

            using (var db = new GeneralDataManager())
            {
                List<DAL.ViewModels.BuildingVM> list = (from building in db.AdmissionDB.Buildings
                                                        join campus in db.AdmissionDB.Campus on building.CampusID equals campus.ID
                                                        where building.IsActive == true
                                                        select new DAL.ViewModels.BuildingVM
                                                        {
                                                            BuildingID = building.ID,
                                                            CampusID = building.CampusID,
                                                            BuildingPriority = building.BuildingPriority,
                                                            BuildingWithCampusName = building.BuildingName + " [" + campus.CampusName + "]"
                                                        }).ToList();

                DDLHelper.Bind<DAL.ViewModels.BuildingVM>(ddlBuilding, list.OrderBy(x=> x.CampusID).ThenBy(x=> x.BuildingPriority).ToList(), "BuildingWithCampusName", "BuildingID", EnumCollection.ListItemType.Select);

            }

        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.Room> list = db.AdmissionDB.Rooms.Where(d => d.IsActive == true).ToList();

                if (list != null)
                {   

                    lvRoom.DataSource = list.OrderBy(x=> x.Building.Campu.DistrictSeatPlanSetupId)
                        .ThenBy(x=>x.Building.Campu.CampusPriority)
                        .ThenBy(x=>x.Building.BuildingPriority)
                        .ThenBy(x=> x.RoomPriority).ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvRoom.DataSource = null;
                    lblCount.Text = "0";
                }
                lvRoom.DataBind();
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DAL.Room obj = new DAL.Room();

            try
            {
                DAL.Room existingRoom = null;
                if (CurrentRoomID > 0)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            existingRoom = db.AdmissionDB.Rooms.Find(CurrentRoomID);
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Unable to find existing room";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }


                if (existingRoom != null && CurrentRoomID > 0) //update existing
                {
                    obj = existingRoom;

                    obj.BuildingID = Convert.ToInt32(ddlBuilding.SelectedValue);
                    obj.Capacity = Convert.ToInt32(txtRoomCapacity.Text);
                    obj.FloorNumber = txtFloorNumber.Text;
                    obj.RoomName = txtRoomName.Text;
                    obj.RoomNumber = txtRoomNumber.Text;
                    obj.RoomPriority = Convert.ToInt32(txtPriority.Text);
                    obj.IsActive = ckbxIsActive.Checked;

                    obj.ModifiedBy = uId;
                    obj.DateModified = DateTime.Now;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Update<DAL.Room>(obj);
                        }

                        lblMessageSave.Text = "Updated Successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to update";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    CurrentRoomID = 0;
                }
                else if (existingRoom == null && CurrentRoomID == 0) //create new
                {
                    obj = null;
                    obj = new DAL.Room();

                    obj.BuildingID = Convert.ToInt32(ddlBuilding.SelectedValue);
                    obj.Capacity = Convert.ToInt32(txtRoomCapacity.Text);
                    obj.FloorNumber = txtFloorNumber.Text;
                    obj.RoomName = txtRoomName.Text;
                    obj.RoomNumber = txtRoomNumber.Text;
                    obj.RoomPriority = Convert.ToInt32(txtPriority.Text);
                    obj.IsActive = ckbxIsActive.Checked;

                    obj.CreatedBy = uId;
                    obj.DateCreated = DateTime.Now;

                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            db.Insert<DAL.Room>(obj);
                        }

                        lblMessageSave.Text = "Room created successfully";
                        lblMessageSave.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblMessageSave.Text = "Unable to create new room";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                    CurrentRoomID = 0;
                }

            }
            catch (Exception)
            {
                lblMessage.Text = "Error saving/updating room";
                lblMessage.ForeColor = Color.Crimson;
            }
            btnSubmit.Text = "Save";
            ClearFields();
            LoadListView();
        }

        protected void lvRoom_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Room room = (DAL.Room)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblRoomName = (Label)currentItem.FindControl("lblRoomName");
                Label lblBuildingName = (Label)currentItem.FindControl("lblBuildingName");
                Label lblRoomNumber = (Label)currentItem.FindControl("lblRoomNumber");
                Label lblFloorNumber = (Label)currentItem.FindControl("lblFloorNumber");
                Label lblCapacity = (Label)currentItem.FindControl("lblCapacity");
                Label lblPriority = (Label)currentItem.FindControl("lblPriority");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                lblRoomName.Text = room.RoomName;
                lblRoomNumber.Text = room.RoomNumber;
                lblFloorNumber.Text = room.FloorNumber;
                lblCapacity.Text = room.Capacity.ToString();
                lblPriority.Text = room.RoomPriority.ToString();

                try
                {
                    int buildingId = Convert.ToInt32(room.BuildingID);
                    using (var db = new GeneralDataManager())
                    {
                        List<DAL.ViewModels.BuildingVM> list = (from buildingT in db.AdmissionDB.Buildings
                                                                join campus in db.AdmissionDB.Campus on buildingT.CampusID equals campus.ID
                                                                where buildingT.IsActive == true && buildingT.ID == buildingId
                                                                select new DAL.ViewModels.BuildingVM
                                                                {
                                                                    BuildingID = buildingT.ID,
                                                                    CampusID = buildingT.CampusID,
                                                                    BuildingPriority = buildingT.BuildingPriority,
                                                                    BuildingWithCampusName = buildingT.BuildingName + " [" + campus.CampusName + "]"
                                                                }).ToList();

                        if (list != null && list.Count > 0)
                        {
                            lblBuildingName.Text = list.FirstOrDefault().BuildingWithCampusName;
                        }
                        else
                        {
                            lblBuildingName.Text = " - ";
                        }

                    }

                    //DAL.Building building = null;
                    //int buildingId = Convert.ToInt32(room.BuildingID);
                    //using (var db = new OfficeDataManager())
                    //{
                    //    building = db.AdmissionDB.Buildings.Find(buildingId);
                    //}

                    //if (building != null)
                    //{
                    //    lblBuildingName.Text = building.BuildingName;
                    //}
                    //else
                    //{
                    //    lblBuildingName.Text = " - ";
                    //}
                }
                catch (Exception)
                {

                }

                if (room.IsActive == true)
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
                lnkEdit.CommandArgument = room.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = room.ID.ToString();
            }
        }

        protected void lvRoom_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (var db = new OfficeDataManager())
                    {
                        var objToDelete = db.AdmissionDB.Rooms.Find(id);
                        db.Delete<DAL.Room>(objToDelete);
                        CurrentRoomID = 0;
                    }
                    LoadListView();

                    lblMessage.Text = "Room deleted successfully.";
                    lblMessage.ForeColor = Color.Green;
                    messagePanel.CssClass = "alert alert-warning";
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error deleting room.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }
            else if (e.CommandName == "Update")
            {
                DAL.Room room = null;
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (var db = new OfficeDataManager())
                    {
                        room = db.AdmissionDB.Rooms.Find(id);
                    }
                    if (room != null)
                    {
                        CurrentRoomID = room.ID;

                        txtFloorNumber.Text = room.FloorNumber;
                        txtRoomCapacity.Text = room.Capacity.ToString();
                        txtRoomName.Text = room.RoomName;
                        txtRoomNumber.Text = room.RoomNumber;
                        txtPriority.Text = room.RoomPriority.ToString();
                        ddlBuilding.SelectedValue = room.BuildingID.ToString();
                        if (room.IsActive == true)
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
                    lblMessage.Text = "Error getting room for update.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }

            }
        }

        protected void lvRoom_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvRoom_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            #region N/A
            //if (FileUploadRoom.HasFile)
            //{
            //    string excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HRD=YES;IMEX=1'", FileUploadRoom.PostedFile.FileName);
            //    using (OleDbConnection connection = new OleDbConnection(excelConnectionString))
            //    {
            //        OleDbCommand command = new OleDbCommand(("Select * FROM [Sheet1$]"), connection);
            //        connection.Open();
            //        DataTable dtExcelSchema;
            //        dtExcelSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //        foreach (DataRow row in dtExcelSchema.Rows)
            //        {

            //        }

            //        //using (DbDataReader dr = command.ExecuteReader())
            //        //{
            //        //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy("your_Connection_string"))
            //        //    {
            //        //        bulkCopy.DestinationTableName = "Member";
            //        //        bulkCopy.ColumnMappings.Add("Member Name", "member_name");
            //        //        bulkCopy.WriteToServer(dr);
            //        //    }
            //        //}



            //    }
            //} 
            #endregion



            try
            {

                if (FileUploadRoom.HasFile == false)
                {
                    //lblMsg.Text = "Please select a File and try again!";
                    lblMessage.Text = "Please select a File and try again!";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
                else
                {

                    int buildingId = Convert.ToInt32(ddlBuilding.SelectedValue);
                    if (buildingId > 0)
                    {
                        // lblMsg.Text = "";
                        string saveFolder = "~/Upload/RoomFileUpload/";
                        string filename = FileUploadRoom.FileName;
                        string filePath = Path.Combine(saveFolder, FileUploadRoom.FileName);
                        string excelpath = Server.MapPath(filePath);

                        if (File.Exists(excelpath))
                        {
                            System.IO.File.Delete(excelpath);
                            FileUploadRoom.SaveAs(excelpath);
                        }
                        else
                        {
                            FileUploadRoom.SaveAs(excelpath);
                        }


                        System.Data.OleDb.OleDbConnection MyConnection;
                        System.Data.DataTable DtTable;
                        System.Data.OleDb.OleDbDataAdapter MyCommand;
                        MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelpath + ";Extended Properties=Excel 12.0 xml;");
                        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                        MyCommand.TableMappings.Add("Table", "TestTable");
                        DtTable = new System.Data.DataTable();
                        MyCommand.Fill(DtTable);
                        //GridView1.DataSource = DtTable;
                        //GridView1.DataBind();

                        PopulateData(DtTable, buildingId);
                        MyConnection.Close();
                    }
                    else
                    {
                        lblMessage.Text = "Please select Building";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                    }
                    

                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error : " + ex.Message.ToString();
                lblMessage.ForeColor = Color.Crimson;
                messagePanel.CssClass = "alert alert-danger";
            }


        }

        public class RoomDataFromExcel
        {
            public string RoomName { get; set; }
            public string RoomNumber { get; set; }
            public string FloorNumber { get; set; }
            public string Capacity { get; set; }
            public string RoomPriority { get; set; }
        }

        private void PopulateData(DataTable dtTable, int buildingId)
        {
            try
            {

                List<DAL.Room> roomList = null;
                using (var db = new OfficeDataManager())
                {
                    roomList = db.AdmissionDB.Rooms.Where(x => x.BuildingID == buildingId).ToList();
                }

                if (roomList != null && roomList.Count > 0)
                {
                    List<RoomDataFromExcel> rdfeList = new List<RoomDataFromExcel>();

                    // Update
                    #region Get data from Excel
                    try
                    {
                        foreach (DataRow row in dtTable.Rows)
                        {
                            string roomName = row[0].ToString();// == "" ? "0" : row[0].ToString();
                            string roomNumber = row[1].ToString();// == "" ? "0" : row[1].ToString();
                            string floorNumber = row[2].ToString();// == "" ? "0" : row[2].ToString();
                            string capacity = row[3].ToString();// == "" ? "0" : row[3].ToString();
                            string roomPriority = row[4].ToString();// == "" ? "0" : row[4].ToString();


                            if (
                                !string.IsNullOrEmpty(roomName) &&
                                !string.IsNullOrEmpty(roomNumber) &&
                                !string.IsNullOrEmpty(floorNumber) &&
                                !string.IsNullOrEmpty(capacity) &&
                                !string.IsNullOrEmpty(roomPriority)
                                )
                            {
                                rdfeList.Add(new RoomDataFromExcel
                                {
                                    RoomName = roomName,
                                    RoomNumber = roomNumber,
                                    FloorNumber = floorNumber,
                                    Capacity = capacity,
                                    RoomPriority = roomPriority
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessageSave.Text = "Exception; Error: " + ex.Message.ToString();
                        lblMessageSave.ForeColor = Color.Crimson;
                    }
                    #endregion

                    if (rdfeList != null && rdfeList.Count > 0)
                    {
                        int countInsertOrUpdate = 0;

                        foreach(var tData in rdfeList)
                        {
                            DAL.Room existRoomModel = roomList.Where(x => x.RoomName.ToLower() == tData.RoomName.ToLower()).FirstOrDefault();
                            if (existRoomModel != null)
                            {
                                #region Update
                                // Update
                                existRoomModel.RoomName = tData.RoomName;
                                existRoomModel.RoomNumber = tData.RoomNumber;
                                existRoomModel.FloorNumber = tData.FloorNumber;
                                existRoomModel.Capacity = Convert.ToInt32(tData.Capacity);
                                existRoomModel.RoomPriority = Convert.ToInt32(tData.RoomPriority);
                                existRoomModel.ModifiedBy = uId;
                                existRoomModel.DateModified = DateTime.Now;

                                try
                                {
                                    using (var db = new OfficeDataManager())
                                    {
                                        db.Update<DAL.Room>(existRoomModel);
                                        countInsertOrUpdate++;
                                    }
                                }
                                catch (Exception ex)
                                {

                                } 
                                #endregion

                            }
                            else
                            {
                                #region Create
                                // Create
                                DAL.Room obj = new DAL.Room();
                                obj.BuildingID = buildingId;
                                obj.Capacity = Convert.ToInt32(tData.Capacity);
                                obj.FloorNumber = tData.FloorNumber;
                                obj.RoomName = tData.RoomName;
                                obj.RoomNumber = tData.RoomNumber;
                                obj.RoomPriority = Convert.ToInt32(tData.RoomPriority);
                                obj.IsActive = true; //Convert.ToBoolean(isActive);
                                obj.CreatedBy = uId;
                                obj.DateCreated = DateTime.Now;

                                try
                                {
                                    using (var db = new OfficeDataManager())
                                    {
                                        db.Insert<DAL.Room>(obj);
                                        countInsertOrUpdate++;
                                    }

                                }
                                catch (Exception ex)
                                {

                                } 
                                #endregion
                            }

                        }


                        if (countInsertOrUpdate > 0)
                        {
                            lblMessageSave.Text = "Room Update successfully";
                            lblMessageSave.ForeColor = Color.Green;

                            LoadListView();
                        }
                        else
                        {
                            lblMessageSave.Text = "Unable to Update room";
                            lblMessageSave.ForeColor = Color.Crimson;
                        }

                    }
                    else
                    {
                        lblMessageSave.Text = "No data found from excel !!";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }

                }
                else
                {
                    //RoomName BuildingID  RoomNumber FloorNumber Capacity RoomPriority

                    // Newly Insert

                    List<RoomDataFromExcel> rdfeList = new List<RoomDataFromExcel>();

                    #region Get data from Excel
                    try
                    {
                        foreach (DataRow row in dtTable.Rows)
                        {
                            string roomName = row[0].ToString();// == "" ? "0" : row[0].ToString();
                            string roomNumber = row[1].ToString();// == "" ? "0" : row[1].ToString();
                            string floorNumber = row[2].ToString();// == "" ? "0" : row[2].ToString();
                            string capacity = row[3].ToString();// == "" ? "0" : row[3].ToString();
                            string roomPriority = row[4].ToString();// == "" ? "0" : row[4].ToString();


                            if (
                                !string.IsNullOrEmpty(roomName) &&
                                !string.IsNullOrEmpty(roomNumber) &&
                                !string.IsNullOrEmpty(floorNumber) &&
                                !string.IsNullOrEmpty(capacity) &&
                                !string.IsNullOrEmpty(roomPriority)
                                )
                            {
                                rdfeList.Add(new RoomDataFromExcel
                                {
                                    RoomName = roomName,
                                    RoomNumber = roomNumber,
                                    FloorNumber = floorNumber,
                                    Capacity = capacity,
                                    RoomPriority = roomPriority
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessageSave.Text = "Exception; Error: " + ex.Message.ToString();
                        lblMessageSave.ForeColor = Color.Crimson;
                    }
                    #endregion


                    int countInsert = 0;
                    if (rdfeList != null && rdfeList.Count > 0)
                    {
                        foreach(var tData in rdfeList)
                        {
                            DAL.Room obj = new DAL.Room();
                            obj.BuildingID = buildingId;
                            obj.Capacity = Convert.ToInt32(tData.Capacity);
                            obj.FloorNumber = tData.FloorNumber;
                            obj.RoomName = tData.RoomName;
                            obj.RoomNumber = tData.RoomNumber;
                            obj.RoomPriority = Convert.ToInt32(tData.RoomPriority);
                            obj.IsActive = true; //Convert.ToBoolean(isActive);

                            obj.CreatedBy = uId;
                            obj.DateCreated = DateTime.Now;

                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Insert<DAL.Room>(obj);
                                    countInsert++;
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }

                        if (countInsert > 0)
                        {
                            lblMessageSave.Text = "Room created successfully";
                            lblMessageSave.ForeColor = Color.Green;

                            LoadListView();
                        }
                        else
                        {
                            lblMessageSave.Text = "Unable to create new room";
                            lblMessageSave.ForeColor = Color.Crimson;
                        }
                    }
                    else
                    {
                        lblMessageSave.Text = "No data found from excel !!";
                        lblMessageSave.ForeColor = Color.Crimson;
                    }



                    

                }


                #region N/A
                //try
                //{
                //    using (var db = new OfficeDataManager())
                //    {
                //        var all = from c in db.AdmissionDB.Rooms.ToList() select c;
                //        db.AdmissionDB.Rooms.RemoveRange(all);
                //        db.AdmissionDB.SaveChanges();
                //    }

                //}
                //catch (Exception ex)
                //{
                //    lblMessageSave.Text = ex.Message.ToString();
                //    lblMessageSave.ForeColor = Color.Crimson;
                //}

                //foreach (DataRow row in dtTable.Rows)
                //{
                //    string roomName = row[0].ToString() == "" ? "0" : row[0].ToString();
                //    string buildingId = row[1].ToString() == "" ? "0" : row[1].ToString();
                //    string roomNumber = row[2].ToString() == "" ? "0" : row[2].ToString();
                //    string floorNumber = row[3].ToString() == "" ? "0" : row[3].ToString();
                //    string capacity = row[4].ToString() == "" ? "0" : row[4].ToString();
                //    string roomPriority = row[5].ToString() == "" ? "0" : row[5].ToString();
                //    string isActive = row[6].ToString() == "" ? "0" : row[6].ToString();


                //    DAL.Room obj = new DAL.Room();

                //    //obj = null;
                //    //obj = new DAL.Room();

                //    obj.BuildingID = Convert.ToInt32(buildingId);
                //    obj.Capacity = Convert.ToInt32(capacity);
                //    obj.FloorNumber = floorNumber;
                //    obj.RoomName = roomName;
                //    obj.RoomNumber = roomNumber;
                //    obj.RoomPriority = Convert.ToInt32(roomPriority);
                //    obj.IsActive = true; //Convert.ToBoolean(isActive);

                //    obj.CreatedBy = uId;
                //    obj.DateCreated = DateTime.Now;

                //    try
                //    {
                //        using (var db = new OfficeDataManager())
                //        {
                //            db.Insert<DAL.Room>(obj);
                //        }

                //        lblMessageSave.Text = "Room created successfully";
                //        lblMessageSave.ForeColor = Color.Green;
                //    }
                //    catch (Exception)
                //    {
                //        lblMessageSave.Text = "Unable to create new room";
                //        lblMessageSave.ForeColor = Color.Crimson;
                //    }



                //} 
                #endregion
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error : " + ex.Message.ToString();
                lblMessage.ForeColor = Color.Crimson;
                messagePanel.CssClass = "alert alert-danger";
            }
        }






    }
}