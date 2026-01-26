using Admission.App_Start;
using CommonUtility;
using DAL.ViewModels;
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
    public partial class SeatPlanDelete : PageBase
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
                btnDelete.Visible = false;
                LoadDDL();
            }
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.ToList(), "UnitName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        private void LoadListView(long admUnitId, int acaCalId)
        {
            #region Candidate.AdmissionTestRoll
            List<DAL.SPGetTestRollByAcaCalIDAdmUnitID_Result> list = null;
            try
            {
                using (var db = new OfficeDataManager())
                {
                    list = db.AdmissionDB.SPGetTestRollByAcaCalIDAdmUnitID(acaCalId, admUnitId).ToList();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading test rolls. " + ex.Message + "; " + ex.InnerException.Message ;
                lblMessage.ForeColor = Color.Crimson;
                panelMessage.CssClass = "alert alert-danger";
                return;
            }

            if (list != null)
            {
                if (list.Count > 0)
                {
                    gvTestRoll.DataSource = list.OrderBy(c => c.testRoll).ToList();
                    lblCount.Text = list.Count.ToString();
                    //CurrentTestRollList = list;
                    btnDeleteTr.Visible = true;
                }
            }
            else
            {
                gvTestRoll.DataSource = null;
                lblCount.Text = "0";
                //CurrentTestRollList = null;
                btnDeleteTr.Visible = false;
            }
            gvTestRoll.DataBind();
            #endregion

            #region Office.ProgramRoomPriority
            List<SeatPlanDeleteProgramRoomPriorityObject> prpList = null;
            try
            {
                using (var db = new OfficeDataManager())
                {
                    prpList = (from prp in db.AdmissionDB.ProgramRoomPriorities
                               join admUnit in db.AdmissionDB.AdmissionUnits on prp.AdmissionUnitID equals admUnit.ID
                               where prp.AdmissionUnitID == admUnitId && prp.AcaCalID == acaCalId
                               select new SeatPlanDeleteProgramRoomPriorityObject
                               {
                                   ID = prp.ID,
                                   AdmUnitName = admUnit.UnitName,
                                   AdmUnitID = (long)prp.AdmissionUnitID,
                                   AcaCalID = (int)prp.AcaCalID,
                                   StartRoll = prp.StartRoll,
                                   EndRoll = prp.EndRoll,
                                   BuildingName = prp.BuildingName,
                                   RoomName = prp.RoomName,
                                   Priority = (int)prp.Priority
                               }).OrderBy(c=>c.Priority).ToList();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading test rolls. " + ex.Message + "; " + ex.InnerException.Message;
                lblMessage.ForeColor = Color.Crimson;
                panelMessage.CssClass = "alert alert-danger";
                return;
            }

            if(prpList != null)
            {
                if(prpList.Count > 0)
                {
                    gvProgramRoomPriority.DataSource = prpList.OrderBy(c => c.Priority).ToList();
                    btnDelete.Visible = true;
                }
                else
                {
                    gvProgramRoomPriority.DataSource = null;
                    btnDelete.Visible = false;
                }
            }
            else
            {
                gvProgramRoomPriority.DataSource = null;
                btnDelete.Visible = false;
            }
            gvProgramRoomPriority.DataBind();
            #endregion
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            LoadListView(admUnitId, acaCalId);
        }

        /// <summary>
        /// Delete button event for deleting/updating ProgramRoomPriority
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            if(admUnitId > 0 && acaCalId > 0)
            {
                List<DAL.ProgramRoomPriority> prpList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        prpList = db.AdmissionDB.ProgramRoomPriorities
                            .Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == acaCalId)
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblDeleteMsg.Text = "Error getting PRP list. " + ex.Message + "; " + ex.InnerException.Message;
                }

                if(prpList != null)
                {
                    if(prpList.Count() > 0)
                    {
                        foreach(DAL.ProgramRoomPriority prp in prpList)
                        {
                            prp.StartRoll = null;
                            prp.EndRoll = null;

                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Update<DAL.ProgramRoomPriority>(prp);
                                }
                            }
                            catch (Exception ex)
                            {
                                lblDeleteMsg.Text = "Error deleting PRP. " + ex.Message + "; " + ex.InnerException.Message;
                            }
                        } //end foreach
                    }
                } //end if(prpList != null)
                LoadListView(admUnitId, acaCalId);
            } //end if(admUnitId > 0 && acaCalId > 0)
        }

        /// <summary>
        /// Delete button event for deleting or updating ProgramRoomPriorityID in Admission Test Roll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteTr_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            if(admUnitId > 0 && acaCalId > 0)
            {
                List<DAL.AdmissionTestRoll> atrList = null;
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        atrList = db.AdmissionDB.AdmissionTestRolls
                            .Where(c => c.AdmissionUnitID == admUnitId && acaCalId == acaCalId)
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblDeleteTrMsg.Text = "Error loading test roll. " + ex.Message + "; " + ex.InnerException.Message;
                }

                if(atrList != null)
                {
                    if(atrList.Count() > 0)
                    {
                        foreach(DAL.AdmissionTestRoll atr in atrList)
                        {
                            atr.ProgramRoomPriorityID = null;

                            try
                            {
                                using(var db = new CandidateDataManager())
                                {
                                    db.Update<DAL.AdmissionTestRoll>(atr);
                                }
                            }
                            catch (Exception ex)
                            {
                                lblDeleteTrMsg.Text = "Error deleting atr. " + ex.Message + "; " + ex.InnerException.Message;
                            }
                        } //end foreach
                    }
                } //end if(atrList != null)
                LoadListView(admUnitId, acaCalId);
            } //if(admUnitId > 0 && acaCalId > 0)
        }
    }
}