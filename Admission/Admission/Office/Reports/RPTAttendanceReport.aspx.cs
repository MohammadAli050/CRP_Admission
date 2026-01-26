using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTAttendanceReport : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        string userName = "";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

            using (var db = new OfficeDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if (!IsPostBack)
            {
                LoadDDL();

                //hfCandidateID.Value = "0";
                //Session["AdmitCardFileName"] = null;
                //Session["AdmitCardData"] = null;


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

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                
            }
        }

        protected void CheckFacultyAndSessionIsSelected()
        {
            MessageView("", "clear");

            try
            {
                
                if (Convert.ToInt32(ddlAdmUnit.SelectedValue) > 0 && Convert.ToInt32(ddlSession.SelectedValue) > 0)
                {
                    panelFilterLoad.Visible = true;

                    long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                    using (var db = new OfficeDataManager())
                    {
                        DDLHelper.Bind<DAL.ProgramDistrictPriority>(ddlVenue, db.AdmissionDB.ProgramDistrictPriorities.Where(x=> x.AcaCalID == acaCalId && x.AdmissionUnitID == facultyId).ToList(), "DistrictName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else if (Convert.ToInt32(ddlAdmUnit.SelectedValue) == -1)
                {
                    panelFilterLoad.Visible = false;
                    MessageView("Please Select Faculty!", "fail");
                }
                else if (Convert.ToInt32(ddlSession.SelectedValue) == -1)
                {
                    panelFilterLoad.Visible = false;
                    MessageView("Please Select Session!", "fail");
                }
                else
                {
                    panelFilterLoad.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckFacultyAndSessionIsSelected();
        }

        protected void ddlAdmUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckFacultyAndSessionIsSelected();
        }

        

        protected void ddlVenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                List<DAL.ProgramCampusPriority> pcpList = null;
                if (Convert.ToInt32(ddlVenue.SelectedValue) > 0 && facultyId > 0 && acaCalId > 0)
                {
                    int venuePriorityId = Convert.ToInt32(ddlVenue.SelectedValue);

                    using (var db = new OfficeDataManager())
                    {
                        pcpList = db.AdmissionDB.ProgramCampusPriorities.Where(x => x.AcaCalID == acaCalId
                                                                                && x.AdmissionUnitID == facultyId
                                                                                && x.ProgramDistrictPriorityId == venuePriorityId).ToList();

                        DDLHelper.Bind<DAL.ProgramCampusPriority>(ddlCampus, pcpList.OrderBy(x=> x.CampusPriority).ToList(), "CampusName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else
                {
                    DDLHelper.Bind<DAL.ProgramCampusPriority>(ddlCampus, pcpList, "CampusName", "ID", EnumCollection.ListItemType.Select);
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int venuePriorityId = Convert.ToInt32(ddlVenue.SelectedValue);
                int campusPriorityId = Convert.ToInt32(ddlCampus.SelectedValue);

                List<DAL.ProgramBuildingPriority> pbpList = null;
                if (facultyId > 0 && acaCalId > 0 && venuePriorityId > 0 && campusPriorityId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        pbpList = db.AdmissionDB.ProgramBuildingPriorities.Where(x => x.AcaCalID == acaCalId
                                                                                && x.AdmissionUnitID == facultyId
                                                                                && x.ProgramCampusPriorityID == campusPriorityId).ToList();

                        DDLHelper.Bind<DAL.ProgramBuildingPriority>(ddlBuilding, pbpList.OrderBy(x => x.BuildingPriority).ToList(), "BuildingName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else
                {
                    DDLHelper.Bind<DAL.ProgramBuildingPriority>(ddlBuilding, pbpList, "BuildingName", "ID", EnumCollection.ListItemType.Select);
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void ddlBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int venuePriorityId = Convert.ToInt32(ddlVenue.SelectedValue);
                int campusPriorityId = Convert.ToInt32(ddlCampus.SelectedValue);
                int buildinfPriorityId = Convert.ToInt32(ddlBuilding.SelectedValue);

                List<DAL.ProgramRoomPriority> pbpList = null;
                if (facultyId > 0 && acaCalId > 0 && venuePriorityId > 0 && campusPriorityId > 0 && buildinfPriorityId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        pbpList = db.AdmissionDB.ProgramRoomPriorities.Where(x => x.AcaCalID == acaCalId
                                                                                && x.AdmissionUnitID == facultyId
                                                                                && x.ProgBuildPriorityID == buildinfPriorityId).ToList();

                        DDLHelper.Bind<DAL.ProgramRoomPriority>(ddlRoom, pbpList.OrderBy(x => x.Priority).ToList(), "RoomName", "ID", EnumCollection.ListItemType.Select);
                    }
                }
                else
                {
                    DDLHelper.Bind<DAL.ProgramRoomPriority>(ddlRoom, pbpList, "RoomName", "ID", EnumCollection.ListItemType.Select);
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlAdmUnit.SelectedValue = "-1";
            ddlSession.SelectedValue = "-1";
            
            panelFilterLoad.Visible = false;

            if (!string.IsNullOrEmpty(ddlVenue.SelectedValue))
            {
                ddlVenue.SelectedValue = "-1";
            }

            if (!string.IsNullOrEmpty(ddlCampus.SelectedValue))
            {
                ddlCampus.SelectedValue = "-1";
            }

            if (!string.IsNullOrEmpty(ddlBuilding.SelectedValue))
            {
                ddlBuilding.SelectedValue = "-1";
            }

            if (!string.IsNullOrEmpty(ddlRoom.SelectedValue))
            {
                ddlRoom.SelectedValue = "-1";
            }

            ReportViewer1.Visible = false;
        }


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                if (facultyId > 0 && acaCalId > 0)
                {
                    int venuePriorityIdT = Convert.ToInt32(ddlVenue.SelectedValue);
                    int campusPriorityIdT = !string.IsNullOrEmpty(ddlCampus.SelectedValue) ? Convert.ToInt32(ddlCampus.SelectedValue) : -1;
                    int buildingPriorityIdT = !string.IsNullOrEmpty(ddlBuilding.SelectedValue) ? Convert.ToInt32(ddlBuilding.SelectedValue) : -1;
                    int roomPriorityIdT = !string.IsNullOrEmpty(ddlRoom.SelectedValue) ? Convert.ToInt32(ddlRoom.SelectedValue) : -1;

                    int? venuePriorityId = null;
                    if (venuePriorityIdT > 0)
                    {
                        venuePriorityId = venuePriorityIdT;
                    }

                    int? campusPriorityId = null;
                    if (campusPriorityIdT > 0)
                    {
                        campusPriorityId = campusPriorityIdT;
                    }

                    int? buildingPriorityId = null;
                    if (buildingPriorityIdT > 0)
                    {
                        buildingPriorityId = buildingPriorityIdT;
                    }

                    int? roomPriorityId = null;
                    if (roomPriorityIdT > 0)
                    {
                        roomPriorityId = roomPriorityIdT;
                    }

                    List<DAL.SPAttendanceReport_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPAttendanceReport(acaCalId,
                                                                 facultyId,
                                                                 venuePriorityId,
                                                                 campusPriorityId,
                                                                 buildingPriorityId,
                                                                 roomPriorityId).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        ReportViewer1.LocalReport.EnableExternalImages = true;
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportDataSource rds = new ReportDataSource("DataSet1", list);
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Admission/Office/Reports/RPTAttendanceRoomWiseV3.rdlc");

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                        param1.Add(new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text));
                        param1.Add(new ReportParameter("ExamDateTime", Convert.ToDateTime(list.FirstOrDefault().ExamDate).ToString("dd-MMM-yyyy") + ", " + list.FirstOrDefault().ExamTime)); //
                        param1.Add(new ReportParameter("CampusName", ""));
                        param1.Add(new ReportParameter("BuildingName", ""));
                        param1.Add(new ReportParameter("RoomName", ""));
                        param1.Add(new ReportParameter("RoomCap", ""));

                        ReportViewer1.LocalReport.SetParameters(param1);
                        ReportViewer1.LocalReport.DisplayName = "Report" + ddlSession.SelectedItem.Text.ToString();
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                    }
                }
                else
                {
                    MessageView("Please Select Faculty and Session for Load Report!", "fail");
                    ReportViewer1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString()+"_"+ex.InnerException.Message, "fail");
                ReportViewer1.Visible = false;
            }
        }

        
    }
}