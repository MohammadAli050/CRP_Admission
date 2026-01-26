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

namespace Admission.Admission.Office
{
    public partial class CandidateRoomChange : PageBase
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

            if (uRole.ToLower() == "candidate")
            {

                SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                Response.Redirect("~/Admission/Home.aspx", false);
            }


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

        //protected void CheckFacultyAndSessionIsSelected()
        //{
        //    MessageView("", "clear");

        //    try
        //    {

        //        if (Convert.ToInt32(ddlAdmUnit.SelectedValue) > 0 && Convert.ToInt32(ddlSession.SelectedValue) > 0)
        //        {
        //            panelFilterLoad.Visible = true;

        //            long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
        //            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

        //            using (var db = new OfficeDataManager())
        //            {
        //                DDLHelper.Bind<DAL.ProgramDistrictPriority>(ddlVenue, db.AdmissionDB.ProgramDistrictPriorities.Where(x => x.AcaCalID == acaCalId && x.AdmissionUnitID == facultyId).ToList(), "DistrictName", "ID", EnumCollection.ListItemType.Select);
        //            }
        //        }
        //        else if (Convert.ToInt32(ddlAdmUnit.SelectedValue) == -1)
        //        {
        //            panelFilterLoad.Visible = false;
        //            MessageView("Please Select Faculty!", "fail");
        //        }
        //        else if (Convert.ToInt32(ddlSession.SelectedValue) == -1)
        //        {
        //            panelFilterLoad.Visible = false;
        //            MessageView("Please Select Session!", "fail");
        //        }
        //        else
        //        {
        //            panelFilterLoad.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
        //    }
        //}

        //protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CheckFacultyAndSessionIsSelected();
        //}

        //protected void ddlAdmUnit_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CheckFacultyAndSessionIsSelected();
        //}



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

                        DDLHelper.Bind<DAL.ProgramCampusPriority>(ddlCampus, pcpList.OrderBy(x => x.CampusPriority).ToList(), "CampusName", "ID", EnumCollection.ListItemType.Select);
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
            
        }


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                if (admUnitId > 0 && acaCalId > 0)
                {
                    panelFilterLoad.Visible = true;

                    #region Load Venue
                    using (var db = new OfficeDataManager())
                    {
                        DDLHelper.Bind<DAL.ProgramDistrictPriority>(ddlVenue, db.AdmissionDB.ProgramDistrictPriorities.Where(x => x.AcaCalID == acaCalId && x.AdmissionUnitID == admUnitId).ToList(), "DistrictName", "ID", EnumCollection.ListItemType.Select);
                    }
                    #endregion


                    List<DAL.SPGetAllCandidateSeatPlanInfo_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPGetAllCandidateSeatPlanInfo(acaCalId,
                                                                        admUnitId,
                                                                        null,
                                                                        null,
                                                                        null,
                                                                        null,
                                                                        null,
                                                                        null).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        lblCount.Text = list.Count.ToString();
                        lvRoomInfo.DataSource = list;
                        lvRoomInfo.DataBind();


                    }
                    else
                    {
                        lblCount.Text = "0";
                        lvRoomInfo.DataSource = null;
                        lvRoomInfo.DataBind();
                    }
                }
                else
                {
                    panelFilterLoad.Visible = false;
                    MessageView("Please Select Session and Faculty", "fail");
                }

                
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }


        protected void lvRoomInfo_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetAllCandidateSeatPlanInfo_Result obj = (DAL.SPGetAllCandidateSeatPlanInfo_Result)((ListViewDataItem)(e.Item)).DataItem;


                Label lblTestRoll = (Label)currentItem.FindControl("lblTestRoll");
                Label lblCandidateName = (Label)currentItem.FindControl("lblCandidateName");
                Label lblCandidatePaymentId = (Label)currentItem.FindControl("lblCandidatePaymentId");
                Label lblPhone = (Label)currentItem.FindControl("lblPhone");
                Label lblEmail = (Label)currentItem.FindControl("lblEmail");

                Label lblVenue = (Label)currentItem.FindControl("lblVenue");
                Label lblCampus = (Label)currentItem.FindControl("lblCampus");
                Label lblBuilding = (Label)currentItem.FindControl("lblBuilding");
                Label lblRoom = (Label)currentItem.FindControl("lblRoom");


                //...Hidden Filed
                HiddenField hfPaymentID = (HiddenField)currentItem.FindControl("hfPaymentID");
                //...Hidden Filed
                HiddenField hfCandidateID = (HiddenField)currentItem.FindControl("hfCandidateID");
                //...Hidden Filed
                HiddenField hfAcaCalID = (HiddenField)currentItem.FindControl("hfAcaCalID");
                //...Hidden Filed
                HiddenField hfAdmissionUnitID = (HiddenField)currentItem.FindControl("hfAdmissionUnitID");
                //...Hidden Filed
                HiddenField hfAdmissionTestRollID = (HiddenField)currentItem.FindControl("hfAdmissionTestRollID");
                //...Hidden Filed
                HiddenField hfAdmissionTestRollProgramRoomPriorityID = (HiddenField)currentItem.FindControl("hfAdmissionTestRollProgramRoomPriorityID");
                //...Hidden Filed
                HiddenField hfVenueID = (HiddenField)currentItem.FindControl("hfVenueID");



                #region Hidden Field
                //...Hidden Filed
                hfPaymentID.Value = obj.PaymentId.ToString();
                //...Hidden Filed
                hfCandidateID.Value = obj.CandidateID.ToString();
                //...Hidden Filed
                hfAcaCalID.Value = obj.AcaCalID.ToString();
                //...Hidden Filed
                hfAdmissionUnitID.Value = obj.AdmissionUnitID.ToString();
                //...Hidden Filed
                hfAdmissionTestRollID.Value = obj.AdmissionTestRollId.ToString();
                //...Hidden Filed
                hfAdmissionTestRollProgramRoomPriorityID.Value = obj.AdmissionTestRollProgramRoomPriorityID.ToString();
                //...Hidden Filed
                hfVenueID.Value = obj.VenueId.ToString();
                #endregion


                
                lblTestRoll.Text = obj.TestRoll;
                lblCandidateName.Text = obj.CandidateName.ToString();
                lblCandidatePaymentId.Text = obj.PaymentId.ToString();
                lblPhone.Text = obj.SMSPhone.ToString();
                lblEmail.Text = obj.Email.ToString();

                lblVenue.Text = obj.VenueName.ToString();
                lblCampus.Text = obj.CampusName.ToString();
                lblBuilding.Text = obj.BuildingName.ToString();
                lblRoom.Text = obj.RoomName.ToString();
                
            }
        }

        protected void lvRoomInfo_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            //if (e.CommandName == "SMSDetails")
            //{
            //    #region SMSDetails
            //    long candidateId = Int64.Parse(e.CommandArgument.ToString());


            //    DAL.CandidateFormSl formSerial = null;
            //    using (var db = new CandidateDataManager())
            //    {
            //        formSerial = db.GetCandidateFormSlByCandID_AD(candidateId);
            //    }


            //    List<DAL.ManualSendSMSToCandidate> mssmstcList = null;
            //    if (formSerial != null && formSerial.AdmissionSetup != null)
            //    {
            //        using (var db = new OfficeDataManager())
            //        {
            //            mssmstcList = db.AdmissionDB.ManualSendSMSToCandidates.Where(x => x.CandidateID == candidateId && x.AcaCalID == formSerial.AdmissionSetup.AcaCalID).ToList();
            //        }
            //    }



            //    if (mssmstcList != null && mssmstcList.Count > 0)
            //    {
            //        lvSMSLog.DataSource = mssmstcList.OrderByDescending(x => x.DateCreated).ToList();
            //        lbllvpopupCount.Text = mssmstcList.Count().ToString();
            //    }
            //    else
            //    {
            //        lvSMSLog.DataSource = null;
            //        lbllvpopupCount.Text = mssmstcList.Count().ToString();
            //    }
            //    lvSMSLog.DataBind();


            //    ModalPopupExtender1.Show();
            //    #endregion
            //}
            //else if (e.CommandName == "UnDo")
            //{
            //    MessageViewGrid("", "clear");

            //    #region Undo
            //    long forwardCandidateToDepartmentAndApproveID = Int64.Parse(e.CommandArgument.ToString());

            //    if (forwardCandidateToDepartmentAndApproveID > 0)
            //    {
            //        DAL.ForwardCandidateToDepartmentAndApprove fctdapModel = null;
            //        using (var db = new OfficeDataManager())
            //        {
            //            fctdapModel = db.AdmissionDB.ForwardCandidateToDepartmentAndApproves.Where(x => x.ID == forwardCandidateToDepartmentAndApproveID).FirstOrDefault();
            //        }

            //        if (fctdapModel != null && fctdapModel.DepartmentForwardStatusTypeID == 1)
            //        {
            //            #region Check Which One Is Clicked For Load Data (Filter / Search)
            //            if (!string.IsNullOrEmpty(hfWhichOneIsClickedForLoadData.Value))
            //            {
            //                /// <summary>
            //                /// If Load Data is by (2.Search), Then Don't Show Check All CheckBox
            //                /// Else Show Check All CheckBox
            //                /// </summary>
            //                if (Convert.ToInt32(hfWhichOneIsClickedForLoadData.Value) == 2)
            //                {
            //                    btnSearch_Click(null, null);
            //                }
            //                else
            //                {
            //                    btnLoad_Click(null, null);
            //                }
            //            }
            //            #endregion

            //            MessageViewGrid("Candidate Is Already Approved. You can not Undo now !!", "fail");
            //        }
            //        else
            //        {
            //            using (var db = new OfficeDataManager())
            //            {
            //                db.Delete<DAL.ForwardCandidateToDepartmentAndApprove>(fctdapModel);
            //            }

            //            #region Check Which One Is Clicked For Load Data (Filter / Search)
            //            if (!string.IsNullOrEmpty(hfWhichOneIsClickedForLoadData.Value))
            //            {
            //                /// <summary>
            //                /// If Load Data is by (2.Search), Then Don't Show Check All CheckBox
            //                /// Else Show Check All CheckBox
            //                /// </summary>
            //                if (Convert.ToInt32(hfWhichOneIsClickedForLoadData.Value) == 2)
            //                {
            //                    btnSearch_Click(null, null);
            //                }
            //                else
            //                {
            //                    btnLoad_Click(null, null);
            //                }
            //            }
            //            #endregion

            //            MessageViewGrid("Data Undo Successful", "success");
            //        }

            //    }



            //    #endregion
            //}
            
        }


        protected void lvRoomInfo_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            
            lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);    
            btnLoad_Click(null, null);
        }

        protected void cbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool isCheckAll = false;
                CheckBox cb = sender as CheckBox;
                if (cb.Checked)
                {
                    isCheckAll = true;
                }

                for (int i = 0; i < lvRoomInfo.Items.Count; i++)
                {
                    ListViewItem lvi = lvRoomInfo.Items[i];

                    CheckBox cbSingle = (CheckBox)lvi.FindControl("cbSingle");


                    cbSingle.Checked = isCheckAll;

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnChangeRoom_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int venuePriorityIdT = Convert.ToInt32(ddlVenue.SelectedValue);
                int campusPriorityIdT = !string.IsNullOrEmpty(ddlCampus.SelectedValue) ? Convert.ToInt32(ddlCampus.SelectedValue) : -1;
                int buildingPriorityIdT = !string.IsNullOrEmpty(ddlBuilding.SelectedValue) ? Convert.ToInt32(ddlBuilding.SelectedValue) : -1;
                int roomPriorityIdT = !string.IsNullOrEmpty(ddlRoom.SelectedValue) ? Convert.ToInt32(ddlRoom.SelectedValue) : -1;

                int count = 0;

                if (venuePriorityIdT > 0 &&
                    campusPriorityIdT > 0 &&
                    buildingPriorityIdT > 0 &&
                    roomPriorityIdT > 0)
                {
                    for (int i = 0; i < lvRoomInfo.Items.Count; i++)
                    {
                        ListViewItem lvi = lvRoomInfo.Items[i];

                        CheckBox cbSingle = (CheckBox)lvi.FindControl("cbSingle");

                        Label lblTestRoll = (Label)lvi.FindControl("lblTestRoll");
                        HiddenField hfCandidateID = (HiddenField)lvi.FindControl("hfCandidateID");
                        HiddenField hfAcaCalID = (HiddenField)lvi.FindControl("hfAcaCalID");
                        HiddenField hfAdmissionUnitID = (HiddenField)lvi.FindControl("hfAdmissionUnitID");
                        HiddenField hfVenueID = (HiddenField)lvi.FindControl("hfVenueID");

                        if (cbSingle.Checked == true)
                        {
                            int acaCalId = !string.IsNullOrEmpty(hfAcaCalID.Value) ? Convert.ToInt32(hfAcaCalID.Value) : -1;
                            long admUnitId = !string.IsNullOrEmpty(hfAdmissionUnitID.Value) ? Convert.ToInt64(hfAdmissionUnitID.Value) : -1;
                            long candidateId = !string.IsNullOrEmpty(hfCandidateID.Value) ? Convert.ToInt64(hfCandidateID.Value) : -1;
                            int venueId = !string.IsNullOrEmpty(hfVenueID.Value) ? Convert.ToInt32(hfVenueID.Value) : -1;
                            string testRoll = lblTestRoll.Text;


                            if (acaCalId > 0 && admUnitId > 0 && candidateId > 0 && venueId > 0 && !string.IsNullOrEmpty(testRoll))
                            {
                                count++;

                                DAL.AdmissionTestRoll admTestRollModel = null;
                                using (var db = new CandidateDataManager())
                                {
                                    admTestRollModel = db.AdmissionDB.AdmissionTestRolls.Where(x => x.AcaCalID == acaCalId
                                                                                                && x.AdmissionUnitID == admUnitId
                                                                                                && x.CandidateID == candidateId
                                                                                                && x.TestRoll == testRoll).FirstOrDefault();
                                }

                                if (admTestRollModel != null)
                                {
                                    admTestRollModel.ProgramRoomPriorityID = roomPriorityIdT;
                                    admTestRollModel.Attribute1 = venueId.ToString();

                                    using (var db = new CandidateDataManager())
                                    {
                                        db.Update<DAL.AdmissionTestRoll>(admTestRollModel);
                                    }
                                }
                            }
                        }
                    }

                    if (count > 0)
                    {
                        btnLoad_Click(null, null);
                        btnClear_Click(null, null);

                        MessageView("Data Updated Successfully.", "success");

                    }
                    else
                    {
                        MessageView("Please check a checkbox of Candidate, for make change", "fail");
                    }

                }
                else
                {
                    MessageView("Please select Venue, Campus, Building and Room for Change", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }








    }
}