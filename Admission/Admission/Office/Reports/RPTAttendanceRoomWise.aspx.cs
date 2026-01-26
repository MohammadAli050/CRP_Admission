using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTAttendanceRoomWise : PageBase
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
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            using (var db = new OfficeDataManager())
            {
                List<DAL.ProgramRoomPriority> list = null;
                list = db.AdmissionDB.ProgramRoomPriorities
                    .Where(c => c.AcaCalID == acaCalId && c.AdmissionUnitID == admUnitId)
                    .ToList();

                if(list != null)
                {
                    DDLHelper.Bind<DAL.ProgramRoomPriority>(ddlRoom, list.ToList(), "RoomName", "ID", EnumCollection.ListItemType.Select);
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;
            long progRoomPriorId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            progRoomPriorId = Convert.ToInt32(ddlRoom.SelectedValue);

            if(admUnitId > 0 && acaCalId > 0)
            {

                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using(var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AcaCalID == acaCalId &&
                                    c.AdmissionUnitID == admUnitId &&
                                    c.IsActive == true)
                            .FirstOrDefault();
                            
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting admission exam. " + ex.Message + "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    return;
                }
                
                DAL.ProgramRoomPriority progRoomObj = null;
                if(progRoomPriorId > 0)
                {
                    try
                    {
                        using(var db = new OfficeDataManager())
                        {
                            progRoomObj = db.AdmissionDB.ProgramRoomPriorities.Find(progRoomPriorId);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting room priority. " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }
                }

                if(progRoomObj != null)
                {
                    string startRoll = progRoomObj.StartRoll;
                    string endRoll = progRoomObj.EndRoll;

                    DAL.ProgramBuildingPriority progBuildPrior = null;
                    try
                    {
                        using(var db = new OfficeDataManager())
                        {
                            progBuildPrior = db.AdmissionDB.ProgramBuildingPriorities.Find(progRoomObj.ProgBuildPriorityID);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting building priority. " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }

                    List<DAL.SPRptAttendanceRoomWise_Result> attendanceList = null;
                    try
                    {
                        using(var db = new OfficeDataManager())
                        {
                            attendanceList = db.AdmissionDB.SPRptAttendanceRoomWise(acaCalId, admUnitId, startRoll, endRoll).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting roomwise attendance list. " + ex.Message + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }


                    if(attendanceList != null)
                    {
                        if(attendanceList.Count() > 0)
                        {
                            ReportViewer1.LocalReport.EnableExternalImages = true;

                            ReportDataSource rds = new ReportDataSource("DataSet1", attendanceList);

                            IList<ReportParameter> param1 = new List<ReportParameter>();
                            param1.Add(new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text));
                            param1.Add(new ReportParameter("CampusName", progBuildPrior.CampusName));
                            param1.Add(new ReportParameter("BuildingName", progRoomObj.BuildingName));
                            param1.Add(new ReportParameter("RoomName", progRoomObj.RoomName));
                            param1.Add(new ReportParameter("RoomCap", progRoomObj.Capacity.ToString()));
                            param1.Add(new ReportParameter("ExamDateTime", Convert.ToDateTime(admSetup.ExamDate).ToString("dd-MMM-yyyy") + ", " + admSetup.ExamTime));
                            param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));
                            
                            ReportViewer1.LocalReport.SetParameters(param1);

                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        else
                        {

                        }
                    }
                }
            }
        }

        
    }
}