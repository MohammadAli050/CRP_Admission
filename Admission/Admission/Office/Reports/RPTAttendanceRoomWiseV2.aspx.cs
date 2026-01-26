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
using System.Drawing;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTAttendanceRoomWiseV2 : PageBase
    {


        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

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
                List<DAL.Building> list = null;
                list = db.AdmissionDB.Buildings.ToList();

                if (list != null)
                {
                    DDLHelper.Bind<DAL.Building>(ddlBuilding, list.ToList(), "BuildingName", "ID", EnumCollection.ListItemType.Select);
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {

            long admUnitId = -1;
            int acaCalId = -1;
            long buildingsId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            buildingsId = Convert.ToInt32(ddlBuilding.SelectedValue);

            //string admUnitName = ddlAdmUnit.SelectedItem.Text;
            //string acaCalName = ddlSession.SelectedItem.Text;
            //string buildingsName = ddlBuilding.SelectedItem.Text;
            //string reportName = "Atn_" + admUnitName.Replace(" ", "_") + "_" + buildingsName.Replace(" ", "_");

            if (admUnitId > 0 && acaCalId > 0)
            {

                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AcaCalID == acaCalId &&
                                    c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                        //
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting admission exam. " + ex.Message; //+ "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    return;
                }

                List<DAL.ProgramRoomPriority> progRoomObj = new List<DAL.ProgramRoomPriority>();
                if (buildingsId > 0)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            progRoomObj = db.AdmissionDB.ProgramRoomPriorities.Where(x => x.BuildingID == buildingsId && x.AcaCalID == acaCalId && x.AdmissionUnitID == admUnitId).ToList();
                            progRoomObj = progRoomObj.OrderBy(x => x.Priority).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting room priority. " + ex.Message; // + "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }
                }

                if (progRoomObj != null)
                {
                    string startRoll = progRoomObj.FirstOrDefault().StartRoll;
                    progRoomObj = progRoomObj.Where(y => y.StartRoll != null && y.EndRoll != null).OrderByDescending(x => x.Priority).ToList();
                    string endRoll = progRoomObj.FirstOrDefault().EndRoll;

                    DAL.ProgramBuildingPriority progBuildPrior = null;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            progBuildPrior = db.AdmissionDB.ProgramBuildingPriorities.Find(progRoomObj[0].ProgBuildPriorityID);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting building priority. " + ex.Message; //+ "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }

                    //List<DAL.SPRptAttendanceRoomWise_Result> attendanceList = null;
                    List<DAL.SPRptAttendanceRoomWiseV2_Result> attendanceList = null;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            //attendanceList = db.AdmissionDB.SPRptAttendanceRoomWise(acaCalId, admUnitId, startRoll, endRoll).ToList();
                            attendanceList = db.AdmissionDB.SPRptAttendanceRoomWiseV2(acaCalId, admUnitId, startRoll, endRoll).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting roomwise attendance list. " + ex.Message; //+ "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }


                    if (attendanceList != null)
                    {
                        if (attendanceList.Count() > 0)
                        {
                            ReportViewer1.LocalReport.EnableExternalImages = true;

                            ReportDataSource rds = new ReportDataSource("DataSet1", attendanceList);

                            IList<ReportParameter> param1 = new List<ReportParameter>();
                            param1.Add(new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text));
                            param1.Add(new ReportParameter("CampusName", progBuildPrior.CampusName)); //
                            param1.Add(new ReportParameter("BuildingName", progRoomObj[0].BuildingName)); //
                            param1.Add(new ReportParameter("RoomName", progRoomObj[0].RoomName)); //
                            param1.Add(new ReportParameter("RoomCap", progRoomObj.Capacity.ToString()));
                            param1.Add(new ReportParameter("ExamDateTime", Convert.ToDateTime(admSetup.ExamDate).ToString("dd-MMM-yyyy") + ", " + admSetup.ExamTime)); //
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

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                long admUnitId = -1;
                int acaCalId = -1;
                long buildingsId = -1;

                admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                buildingsId = Convert.ToInt32(ddlBuilding.SelectedValue);

                string admUnitName = ddlAdmUnit.SelectedItem.Text;
                string acaCalName = ddlSession.SelectedItem.Text;
                string buildingsName = ddlBuilding.SelectedItem.Text;
                string reportName = "Atn_" + admUnitName.Replace(" ", "_") + "_" + acaCalName.Replace(" ", "_") + "_" + buildingsName.Replace(" ", "_");

                if (admUnitId > 0 && acaCalId > 0 && buildingsId > 0)
                {


                    DAL.AdmissionSetup admSetup = null;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admSetup = db.AdmissionDB.AdmissionSetups
                                .Where(c => c.AcaCalID == acaCalId &&
                                        c.AdmissionUnitID == admUnitId && c.IsActive == true)
                                .FirstOrDefault();
                            //
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting admission exam. " + ex.Message; //+ "; " + ex.InnerException.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }

                    List<DAL.ProgramRoomPriority> progRoomObj = new List<DAL.ProgramRoomPriority>();
                    if (buildingsId > 0)
                    {
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                progRoomObj = db.AdmissionDB.ProgramRoomPriorities.Where(x => x.BuildingID == buildingsId && x.AcaCalID == acaCalId && x.AdmissionUnitID == admUnitId).ToList();
                                progRoomObj = progRoomObj.OrderBy(x => x.Priority).ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error getting room priority. " + ex.Message; // + "; " + ex.InnerException.Message;
                            messagePanel.CssClass = "alert alert-danger";
                            return;
                        }
                    }

                    if (progRoomObj != null)
                    {
                        string startRoll = progRoomObj.FirstOrDefault().StartRoll;
                        progRoomObj = progRoomObj.Where(y => y.StartRoll != null && y.EndRoll != null).OrderByDescending(x => x.Priority).ToList();
                        string endRoll = progRoomObj.FirstOrDefault().EndRoll;

                        DAL.ProgramBuildingPriority progBuildPrior = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                progBuildPrior = db.AdmissionDB.ProgramBuildingPriorities.Find(progRoomObj[0].ProgBuildPriorityID);
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error getting building priority. " + ex.Message; // + "; " + ex.InnerException.Message;
                            messagePanel.CssClass = "alert alert-danger";
                            return;
                        }


                        List<DAL.SPRptAttendanceRoomWiseV2_Result> attendanceList = null;
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                //attendanceList = db.AdmissionDB.SPRptAttendanceRoomWise(acaCalId, admUnitId, startRoll, endRoll).ToList();
                                attendanceList = db.AdmissionDB.SPRptAttendanceRoomWiseV2(acaCalId, admUnitId, startRoll, endRoll).ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error getting roomwise attendance list. " + ex.Message; // + "; " + ex.InnerException.Message;
                            messagePanel.CssClass = "alert alert-danger";
                            return;
                        }

                        if (attendanceList != null && attendanceList.Count > 0)
                        {
                            ReportViewer1.LocalReport.EnableExternalImages = true;

                            ReportDataSource rds = new ReportDataSource("DataSet1", attendanceList);

                            IList<ReportParameter> param1 = new List<ReportParameter>();
                            param1.Add(new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text));
                            param1.Add(new ReportParameter("CampusName", progBuildPrior.CampusName)); //
                            param1.Add(new ReportParameter("BuildingName", progRoomObj[0].BuildingName)); //
                            param1.Add(new ReportParameter("RoomName", progRoomObj[0].RoomName)); //
                            param1.Add(new ReportParameter("RoomCap", progRoomObj.Capacity.ToString()));
                            param1.Add(new ReportParameter("ExamDateTime", Convert.ToDateTime(admSetup.ExamDate).ToString("dd-MMM-yyyy") + ", " + admSetup.ExamTime)); //
                            param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));

                            ReportViewer1.LocalReport.SetParameters(param1);

                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;

                            #region Download Attendance Room Wise
                            try
                            {

                                Warning[] warnings;
                                string[] streamids;
                                string mimeType;
                                string encoding;
                                string filenameExtension;

                                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                ReportViewer1.LocalReport.Refresh();

                                using (FileStream fs = new FileStream(Server.MapPath("~/Upload/Attendance/" + reportName + ".pdf"), FileMode.Create))
                                {
                                    fs.Write(bytes, 0, bytes.Length);
                                }

                                //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                                //Response.ClearHeaders();
                                //Response.ClearContent();
                                //Response.Buffer = true;
                                //Response.Clear();
                                //Response.ContentType = "application/pdf";
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + reportName + ".pdf");
                                //Response.TransmitFile(Server.MapPath("~/Upload/Attendance/" + reportName + ".pdf"));
                                //Response.Flush();
                                ////Response.Close();
                                //Response.End();

                                ShowAlertMessage("Download Successfully.");

                            }
                            catch (Exception ex)
                            {
                                lblMessage.Text = "Exception: Unable to download AttendanceRoomWise; Error: " + ex.Message.ToString();
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                                messagePanel.Visible = true;

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.EventName = "AttendanceRoomWise";
                                    dLog.PageName = "RPTAttendanceRoomWiseV2.aspx";
                                    dLog.NewData = "Exception Unable to download AttendanceRoomWise; Error: " + ex.Message.ToString();
                                    dLog.UserId = uId;
                                    dLog.Attribute1 = "Failed";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + ";  U-ID:" + uId;
                                    dLog.DateCreated = DateTime.Now;
                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ext)
                                {
                                }
                                #endregion

                                //return;
                            }
                            //finally
                            //{
                            //    try
                            //    {
                            //        if (File.Exists(Server.MapPath("~/Upload/TEMP/" + reportName + ".pdf")))
                            //        {
                            //            File.Delete(Server.MapPath("~/Upload/TEMP/" + reportName + ".pdf"));
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            //        try
                            //        {
                            //            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                            //            dLog.EventName = "AttendanceRoomWise";
                            //            dLog.PageName = "RPTAttendanceRoomWiseV2.aspx";
                            //            dLog.NewData = "Exception Failed to delete exist file AttendanceRoomWise; Error: " + ex.Message.ToString();
                            //            dLog.UserId = uId;
                            //            dLog.Attribute1 = "Failed";
                            //            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            //                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; U-ID:" + uId;
                            //            dLog.DateCreated = DateTime.Now;
                            //            LogWriter.DataLogWriter(dLog);
                            //        }
                            //        catch (Exception ext)
                            //        {
                            //        }
                            //        #endregion
                            //    }
                            //}
                            #endregion
                        }
                        else
                        {
                            ShowAlertMessage("No Data Found !");
                        }
                    }
                    else
                    {
                        ShowAlertMessage("No Room Found !");
                    }
                }
                else
                {
                    ShowAlertMessage("Provide Faculty, Session and Building !");
                }

            }
            catch (Exception ex)
            {
                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "AttendanceRoomWise";
                    dLog.PageName = "RPTAttendanceRoomWiseV2.aspx";
                    dLog.NewData = "Exception: " + ex.Message.ToString(); // + " InnerException: " + ex.InnerException.Message.ToString();
                    dLog.UserId = uId;
                    dLog.Attribute1 = "Failed";
                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; U-ID:" + uId;
                    dLog.DateCreated = DateTime.Now;
                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ext)
                {
                }
                #endregion
            }
        }
    }
}