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
    public partial class SeatPlanGeneration : PageBase
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
            }

        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c=>c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.DistrictForSeatPlan>(ddlDistrict, db.AdmissionDB.DistrictForSeatPlans.OrderBy(c => c.ID).ToList(), "Name", "ID", EnumCollection.ListItemType.Select);
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

            long admUnitId = -1;
            int acaCalId = -1;
            int districtId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            districtId = Convert.ToInt32(ddlDistrict.SelectedValue);

            if (admUnitId > 0 && acaCalId > 0 && districtId > 0)
            {
                List<DAL.SPGetSeatPlanByAcaCalIDAdmUnitID_Result> list = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        list = db.AdmissionDB.SPGetSeatPlanByAcaCalIDAdmUnitID(acaCalId, admUnitId, districtId).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading seat plan";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }

                if (list != null && list.Count > 0)
                {
                    gvSeatPlan.DataSource = list.ToList();
                    lblCount.Text = list.Count.ToString();
                }
                else
                {
                    gvSeatPlan.DataSource = null;
                    lblCount.Text = "0";
                }
                gvSeatPlan.DataBind();
            }
            else
            {
                MessageView("Select Faculty, Session and Venue !!", "fail");
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            long admUnitId = -1;
            int acaCalId = -1;
            int districtID = Convert.ToInt32(ddlDistrict.SelectedValue);
            if(districtID <= 0)
            {
                //lblMessage.Text = "Select Venue For Seat Plan Generate";
                //lblMessage.ForeColor = Color.Crimson;
                //messagePanel.CssClass = "alert alert-danger";
                MessageView("Select Venue For Seat Plan Generate !!", "fail");
                return;
            }

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            if (admUnitId > 0 && acaCalId > 0)
            {
                int minTestRoll = 0;
                int maxTestRoll = 0;
                string unitBatch = "00000";

                List<DAL.AdmissionTestRoll> tempAdmissionTestRollList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        tempAdmissionTestRollList = db.AdmissionDB.AdmissionTestRolls
                            .Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == acaCalId && c.Attribute1==districtID.ToString())
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    //lblMessage.Text = "Error getting test rolls.";
                    //lblMessage.ForeColor = Color.Crimson;
                    //messagePanel.CssClass = "alert alert-danger";
                    MessageView("Exception getting test rolls. !! Error: " + ex.Message.ToString(), "fail");
                    return;
                }

                if (tempAdmissionTestRollList != null && tempAdmissionTestRollList.Count > 0)
                {
                    //minTestRoll = Convert.ToInt32(tempAdmissionTestRollList.Min(x => x.TestRoll).Substring(6, 4));
                    //maxTestRoll = Convert.ToInt32(tempAdmissionTestRollList.Max(x => x.TestRoll).Substring(6, 4));
                    //unitBatch = tempAdmissionTestRollList[0].TestRoll.Substring(0, 6);

                    minTestRoll = Convert.ToInt32(tempAdmissionTestRollList.Min(x => x.TestRoll).Substring(5, 5));
                    maxTestRoll = Convert.ToInt32(tempAdmissionTestRollList.Max(x => x.TestRoll).Substring(5, 5));
                    unitBatch = tempAdmissionTestRollList[0].TestRoll.Substring(0, 5);

                    if (minTestRoll != 0 && maxTestRoll != 0)
                    {
                        //List<DAL.ProgramRoomPriority> tempProgramRoomPriorityList = null;
                        List<DAL.SPGetProgramRoomPriorityForSeatPlanGenerateDISTRICTwise_Result> tempProgramRoomPriorityList = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                tempProgramRoomPriorityList = db.AdmissionDB.SPGetProgramRoomPriorityForSeatPlanGenerateDISTRICTwise(admUnitId, acaCalId, districtID)
                                    .ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            //lblMessage.Text = "Error getting room priority.";
                            //lblMessage.ForeColor = Color.Crimson;
                            //messagePanel.CssClass = "alert alert-danger";
                            MessageView("Exception getting room priority. !! Error: " + ex.Message.ToString(), "fail");
                            return;
                        }

                        if (tempProgramRoomPriorityList != null && tempProgramRoomPriorityList.Count > 0)
                        {
                            int maxTestForQuery = Convert.ToInt32(tempProgramRoomPriorityList.Max(x => x.EndRoll == null ? "00000" : x.EndRoll.Substring(5, 5)));
                            if (maxTestForQuery != 0)
                            {
                                string maxTestForQuerySpecial = unitBatch + maxTestForQuery.ToString().PadLeft(5, '0');
                                DAL.SPGetProgramRoomPriorityForSeatPlanGenerateDISTRICTwise_Result tempPRP = tempProgramRoomPriorityList.Where(x => x.EndRoll == maxTestForQuerySpecial).SingleOrDefault();
                                int tStartRoll = Convert.ToInt32(tempPRP.StartRoll.Substring(5, 5));//--3
                                int tEndRoll = Convert.ToInt32(tempPRP.EndRoll.Substring(5, 5));//--5
                                int? pendingSeat = tempPRP.Capacity - (tEndRoll - tStartRoll + 1);//8 - (5 - 3 + 1) = 5
                                if (maxTestRoll > maxTestForQuery + pendingSeat)//15 >= 10
                                {
                                    tempPRP.EndRoll = unitBatch + (tEndRoll + pendingSeat).ToString().PadLeft(5, '0');
                                    minTestRoll = Convert.ToInt32(tEndRoll + pendingSeat + 1);
                                }
                                else
                                {
                                    tempPRP.EndRoll = unitBatch + maxTestRoll.ToString().PadLeft(5, '0');
                                    minTestRoll = 0;
                                    maxTestRoll = 0;
                                }

                                try
                                {
                                    using (var db = new OfficeDataManager())
                                    {
                                        DAL.ProgramRoomPriority prp = new DAL.ProgramRoomPriority();
                                        prp.AcaCalID = tempPRP.AcaCalID;
                                        prp.AdmissionUnitID = tempPRP.AdmissionUnitID;
                                        prp.AdmissionUnitname = tempPRP.AdmissionUnitname;
                                        prp.BuildingID = tempPRP.BuildingID;
                                        prp.BuildingName = tempPRP.BuildingName;
                                        prp.Capacity = tempPRP.Capacity;
                                        prp.CreatedBy = tempPRP.CreatedBy;
                                        prp.DateCreated = tempPRP.DateCreated;
                                        prp.EndRoll = tempPRP.EndRoll;
                                        prp.FloorNo = tempPRP.FloorNo;
                                        prp.ID = tempPRP.ID;
                                        prp.Priority = tempPRP.Priority;
                                        prp.ProgBuildPriorityID = tempPRP.ProgBuildPriorityID;
                                        prp.ProgramID = tempPRP.ProgramID;
                                        prp.ProgramName = tempPRP.ProgramName;
                                        prp.RoomID = tempPRP.RoomID;
                                        prp.RoomName = tempPRP.RoomName;
                                        prp.RoomNumber = tempPRP.RoomNumber;
                                        prp.StartRoll = tempPRP.StartRoll;

                                        db.Update<DAL.ProgramRoomPriority>(prp);
                                    }

                                    try
                                    {
                                        using (var db = new OfficeDataManager())
                                        {
                                            //int _startRoll = Convert.ToInt32(tempPRP.StartRoll.Substring(4, 6));
                                            //int _endRoll = Convert.ToInt32(tempPRP.EndRoll.Substring(4, 6));

                                            string _startRoll = tempPRP.StartRoll;
                                            string _endRoll = tempPRP.EndRoll;

                                            db.AdmissionDB.SPAdmissionTestRollUpdateRoomUsingTestRollDISTRICTwise(admUnitId, null, acaCalId, tempPRP.ID, _startRoll, _endRoll, districtID);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //lblMessage.Text = "Error updating adm test roll with room.";
                                        //lblMessage.ForeColor = Color.Crimson;
                                        //messagePanel.CssClass = "alert alert-danger";

                                        MessageView("Exception updating adm test roll with room. !! Error: " + ex.Message.ToString(), "fail");
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Error updating temp Prog Room Prior.";
                                    //lblMessage.ForeColor = Color.Crimson;
                                    //messagePanel.CssClass = "alert alert-danger";

                                    MessageView("Exception updating temp Prog Room Prior. !! Error: " + ex.Message.ToString(), "fail");
                                    return;
                                }

                            }

                            int count = 0;

                            foreach (DAL.SPGetProgramRoomPriorityForSeatPlanGenerateDISTRICTwise_Result tempProgramRoomPriority in tempProgramRoomPriorityList)
                            {

                                if ((tempProgramRoomPriority.StartRoll == null && tempProgramRoomPriority.EndRoll == null) || (tempProgramRoomPriority.StartRoll == "" && tempProgramRoomPriority.EndRoll == ""))
                                {
                                    if (minTestRoll != 0 && maxTestRoll != 0)
                                    {
                                        tempProgramRoomPriority.StartRoll = "";
                                        tempProgramRoomPriority.EndRoll = "";
                                        if ((minTestRoll + tempProgramRoomPriority.Capacity - 1) < maxTestRoll)
                                        {
                                            tempProgramRoomPriority.StartRoll = unitBatch + minTestRoll.ToString().PadLeft(5, '0');
                                            tempProgramRoomPriority.EndRoll = unitBatch + (minTestRoll + tempProgramRoomPriority.Capacity - 1).ToString().PadLeft(5, '0');
                                            minTestRoll = Convert.ToInt32(minTestRoll + tempProgramRoomPriority.Capacity);
                                        }
                                        else
                                        {
                                            tempProgramRoomPriority.StartRoll = unitBatch + minTestRoll.ToString().PadLeft(5, '0');
                                            tempProgramRoomPriority.EndRoll = unitBatch + maxTestRoll.ToString().PadLeft(5, '0');

                                            minTestRoll = 0;
                                            maxTestRoll = 0;
                                        }

                                        try
                                        {
                                            using (var db = new OfficeDataManager())
                                            {
                                                DAL.ProgramRoomPriority prp_1 = new DAL.ProgramRoomPriority();
                                                prp_1.AcaCalID = tempProgramRoomPriority.AcaCalID;
                                                prp_1.AdmissionUnitID = tempProgramRoomPriority.AdmissionUnitID;
                                                prp_1.AdmissionUnitname = tempProgramRoomPriority.AdmissionUnitname;
                                                prp_1.BuildingID = tempProgramRoomPriority.BuildingID;
                                                prp_1.BuildingName = tempProgramRoomPriority.BuildingName;
                                                prp_1.Capacity = tempProgramRoomPriority.Capacity;
                                                prp_1.CreatedBy = tempProgramRoomPriority.CreatedBy;
                                                prp_1.DateCreated = tempProgramRoomPriority.DateCreated;
                                                prp_1.EndRoll = tempProgramRoomPriority.EndRoll;
                                                prp_1.FloorNo = tempProgramRoomPriority.FloorNo;
                                                prp_1.ID = tempProgramRoomPriority.ID;
                                                prp_1.Priority = tempProgramRoomPriority.Priority;
                                                prp_1.ProgBuildPriorityID = tempProgramRoomPriority.ProgBuildPriorityID;
                                                prp_1.ProgramID = tempProgramRoomPriority.ProgramID;
                                                prp_1.ProgramName = tempProgramRoomPriority.ProgramName;
                                                prp_1.RoomID = tempProgramRoomPriority.RoomID;
                                                prp_1.RoomName = tempProgramRoomPriority.RoomName;
                                                prp_1.RoomNumber = tempProgramRoomPriority.RoomNumber;
                                                prp_1.StartRoll = tempProgramRoomPriority.StartRoll;

                                                db.Update<DAL.ProgramRoomPriority>(prp_1);

                                                count++;
                                            }

                                            try
                                            {
                                                using (var db = new OfficeDataManager())
                                                {
                                                    //int _startRoll = Convert.ToInt32(tempProgramRoomPriority.StartRoll.Substring(4, 6));
                                                    //int _endRoll = Convert.ToInt32(tempProgramRoomPriority.EndRoll.Substring(4, 6));

                                                    string _startRoll = tempProgramRoomPriority.StartRoll;
                                                    string _endRoll = tempProgramRoomPriority.EndRoll;

                                                    db.AdmissionDB.SPAdmissionTestRollUpdateRoomUsingTestRollDISTRICTwise(admUnitId, null, acaCalId, tempProgramRoomPriority.ID, _startRoll, _endRoll, districtID);
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                //lblMessage.Text = "Error updating adm test roll with room.";
                                                //lblMessage.ForeColor = Color.Crimson;
                                                //messagePanel.CssClass = "alert alert-danger";

                                                MessageView("Error updating adm test roll with room !!", "fail");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            //lblMessage.Text = "Error updating temp Prog Room Prior.";
                                            //lblMessage.ForeColor = Color.Crimson;
                                            //messagePanel.CssClass = "alert alert-danger";

                                            MessageView("Error updating temp Prog Room Prior !!", "fail");
                                        }

                                        #region N/A
                                        //bool result = ManagerAdmission.ProgramWiseRoomPriority_Manager.Update(tempProgramRoomPriority);
                                        //if (result)
                                        //{
                                        //    //Your Work
                                        //    //int resultcount = ManagerAdmission.AdmissionTestRoll_Manager
                                        //          .UpdateRoomUsingTestRoll(programId, acaCalId, tempProgramRoomPriority.ProgRoomPriorityID,
                                        //                                  Int32.Parse(tempProgramRoomPriority.StartRoll), 
                                        //                                  Int32.Parse(tempProgramRoomPriority.EndRoll));
                                        //    bool resultcount = ManagerAdmission.AdmissionTestRoll_Manager
                                        //      .UpdateRoomUsingTestRoll(programId, acaCalId, 
                                        //                              tempProgramRoomPriority.ProgRoomPriorityID,
                                        //                              Convert.ToInt32(tempProgramRoomPriority.StartRoll.Substring(6, 4)), 
                                        //                              Convert.ToInt32(tempProgramRoomPriority.EndRoll.Substring(6, 4)));
                                        //} 
                                        #endregion
                                    }
                                }
                            } // END: foreach()



                            if (count > 0)
                            {
                                MessageView("Generated", "success");
                            }
                            else
                            {
                                MessageView("Failed to Generate !!", "fail");
                            }


                        }//if (tempProgramRoomPriorityList != null)
                        else
                        {
                            MessageView("No Program Room Priority is Found !!", "fail");
                        }
                    }//if (minTestRoll != 0 && maxTestRoll != 0)
                    else
                    {
                        MessageView("No Min and Max Test Roll Found !!", "fail");
                    }
                }
                else
                {
                    MessageView("No Test Roll is Generated !!", "fail");
                }

                //lblMessage.Text = "Generated.";
                //lblMessage.ForeColor = Color.Crimson;
                //messagePanel.CssClass = "alert alert-success";
            }
            else
            {
                MessageView("Select Faculty and Session !!", "fail");
            }
        }
    }
}