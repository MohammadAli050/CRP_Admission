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
    public partial class TestRollGeneration : PageBase
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
            using(var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                //DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        protected void ddlAdmUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            long? admUnitId = null;
            if(ddlAdmUnit.SelectedValue == "-1")
            {
                admUnitId = null;
            }
            else
            {
                admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            }

            List<DAL.AdmissionSetup> admSetupList = null;
            if (admUnitId > 0 && admUnitId != null)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetupList = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .ToList();
                    }
                }
                catch (Exception)
                {
                    
                }
                
            }

            List<DAL.SPAcademicCalendarGetAll_Result> sessionList = new List<DAL.SPAcademicCalendarGetAll_Result>();
            if(admSetupList.Count() > 0)
            {
                try
                {
                    
                    foreach(var item in admSetupList)
                    {
                        int acaCalId = item.AcaCalID;
                        DAL.SPAcademicCalendarGetAll_Result selectedSession = null;
                        using (var db = new OfficeDataManager())
                        {
                            selectedSession = db.AdmissionDB.SPAcademicCalendarGetAll().Where(c => c.AcademicCalenderID == acaCalId).FirstOrDefault();
                        }
                        if(selectedSession != null)
                        {
                            sessionList.Add(selectedSession);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            if(sessionList.Count() > 0)
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, sessionList.ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }
        

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            long admUnitId = -1;
            int acaCalId = -1;
            int? programId = null; // not required for faculty wise exam.

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            if(admUnitId > 0 && acaCalId > 0)
            {

                try
                {
                    List<DAL.ProgramDistrictPriority> pdpList = null;
                    using (var db = new OfficeDataManager())
                    {
                        pdpList = db.AdmissionDB.ProgramDistrictPriorities.Where(x => x.AcaCalID == acaCalId && x.AdmissionUnitID == admUnitId).ToList();
                    }

                    if (pdpList != null && pdpList.Count > 0)
                    {
                        foreach (var tData in pdpList.OrderBy(x => x.DistrictPriority).ToList())
                        {
                            int districtId = (int)tData.DistrictSeatPlanSetupID;

                            int testRoll = 0;
                            string progBatch = "0000"; // "000000";
                            int districtCode = -1; // "000000";

                            #region Get District, Faculty, Session wise Max Test Roll
                            DAL.SPCandidateTestRollMaxV2District_Result tempAdmissionTestRoll = null;
                            try
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    tempAdmissionTestRoll = db.AdmissionDB.SPCandidateTestRollMaxV2District(acaCalId, admUnitId, districtId).FirstOrDefault();
                                }
                            }
                            catch (Exception ex)
                            {
                                lblMessage.Text = "Error loading test rolls 1. " + ex.Message + "; " + ex.InnerException.Message;
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                                return;
                            }
                            #endregion


                            if (tempAdmissionTestRoll != null)
                            {
                                //List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result> messageView = new List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result>();

                                #region Update
                                ////testRoll = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(6, 4));
                                ////progBatch = tempAdmissionTestRoll.TestRoll.Substring(0, 6);

                                #region Old Substring Pattern
                                ///// <summary>
                                ///// Substring Array Index
                                ///// ---------------01-23-456789
                                ///// Previous Roll: 10-21-000001
                                ///// Substring: (0,4) means (0) index thke (4) digit roll thk nibo
                                ///// -------------------------
                                ///// -------------------------
                                ///// Substring Array Index
                                ///// ---------------01-23-456789
                                ///// Previous Roll: 10-21-000001
                                ///// Substring: (4,6) means (4) index thke (6) digit roll thk nibo
                                ///// </summary>

                                //testRoll = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(4, 6));
                                //progBatch = tempAdmissionTestRoll.TestRoll.Substring(0, 4); 
                                #endregion


                                /// <summary>
                                /// Substring Array Index
                                /// ---------------01-23-4-5678910
                                /// Previous Roll: 10-21-1-000001
                                /// Substring: (0,4) means (0) index thke (4) digit roll thk nibo
                                /// -------------------------
                                /// -------------------------
                                /// Substring Array Index
                                /// ---------------01-23-4-5678910
                                /// Previous Roll: 10-21-1-000001
                                /// Substring: (4,1) means (4) index thke (1) digit roll thk nibo
                                /// -------------------------
                                /// -------------------------
                                /// Substring Array Index
                                /// ---------------01-23-4-5678910
                                /// Previous Roll: 10-21-1-000001
                                /// Substring: (5,6) means (5) index thke (6) digit roll thk nibo

                                /// </summary>


                                progBatch = tempAdmissionTestRoll.TestRoll.Substring(0, 4);
                                districtCode = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(4, 1));
                                testRoll = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(5, 5));

                                List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerateDistrict_Result> tempNewCandidateList = null;
                                try
                                {
                                    using (var db = new CandidateDataManager())
                                    {
                                        tempNewCandidateList = db.AdmissionDB.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerateDistrict(acaCalId, admUnitId).ToList();
                                    }

                                    // ==== Filtered by District ID
                                    if (tempNewCandidateList != null && tempNewCandidateList.Count > 0)
                                    {
                                        tempNewCandidateList = tempNewCandidateList.Where(x => x.DistrictId == districtId).ToList();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "Error loading test rolls 3. " + ex.Message + "; " + ex.InnerException.Message;
                                    lblMessage.ForeColor = Color.Crimson;
                                    messagePanel.CssClass = "alert alert-danger";
                                    return;
                                }

                                if (tempNewCandidateList != null && tempNewCandidateList.Count() > 0)
                                {
                                    foreach (var item in tempNewCandidateList)
                                    {
                                        if (item.cTestRoll == "0")
                                        {
                                            DAL.AdmissionTestRoll admTestRoll = new DAL.AdmissionTestRoll();
                                            admTestRoll.AcaCalID = item.admSetupAcaCalID;
                                            admTestRoll.AdmissionUnitID = item.admSetupAdmUnitID;
                                            admTestRoll.CandidateID = item.candidateID;
                                            admTestRoll.FormSL = item.cFormFormSerial;
                                            //admTestRoll.TestRoll = progBatch + (++testRoll).ToString().PadLeft(6, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                                            admTestRoll.TestRoll = progBatch + item.DistrictId.ToString() + (++testRoll).ToString().PadLeft(5, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                                            admTestRoll.Attribute1 = item.DistrictId.ToString();
                                            admTestRoll.CreatedBy = -99;
                                            admTestRoll.CreatedDate = DateTime.Now;

                                            try
                                            {
                                                using (var db = new CandidateDataManager())
                                                {
                                                    db.Insert<DAL.AdmissionTestRoll>(admTestRoll);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                lblMessage.Text = "Error adding test rolls 2. " + ex.Message + "; " + ex.InnerException.Message;
                                                lblMessage.ForeColor = Color.Crimson;
                                                messagePanel.CssClass = "alert alert-danger";
                                                return;
                                            }

                                        }
                                    }
                                }
                                #endregion

                                #region N/A
                                //if (messageView != null && messageView.Count > 0)
                                //{

                                //    string massageError = "Failed to generate test roll for few candidate <br/>";
                                //    int i = 1;
                                //    foreach (var tData in messageView)
                                //    {
                                //        massageError = massageError + i.ToString() + ") " + tData.cPaymentPaymentId.ToString() + "<br/>";
                                //        i++;
                                //    }

                                //    lblMessage.Text = massageError;
                                //    lblMessage.ForeColor = Color.Crimson;
                                //    messagePanel.CssClass = "alert alert-danger";
                                //    return;
                                //} 
                                #endregion
                            }
                            else
                            {
                                //List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result> messageView = new List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result>();

                                #region Create New
                                string unitCode = "00";
                                string sessionCode = "00";//"0000";

                                DAL.AdmissionUnit admUnitObj = null;
                                DAL.SPAcademicCalendarGetById_Result acaCal = null;
                                try
                                {
                                    using (var db = new OfficeDataManager())
                                    {
                                        admUnitObj = db.AdmissionDB.AdmissionUnits.Find(admUnitId);
                                        acaCal = db.AdmissionDB.SPAcademicCalendarGetById(acaCalId).FirstOrDefault();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "Error getting faculty and session. " + ex.Message + "; " + ex.InnerException.Message;
                                    lblMessage.ForeColor = Color.Crimson;
                                    messagePanel.CssClass = "alert alert-danger";
                                    return;
                                }

                                if (admUnitObj != null)
                                {
                                    unitCode = admUnitObj.UnitCode2;
                                }

                                if (acaCal != null)
                                {
                                    sessionCode = (acaCal.Year % 100).ToString();//((acaCal.Year % 100) - 1).ToString() + "" + (acaCal.Year % 100).ToString();
                                }

                                progBatch = unitCode + sessionCode;
                                testRoll = 0;

                                List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerateDistrict_Result> tempCandidateList = null;
                                try
                                {
                                    using (var db = new CandidateDataManager())
                                    {
                                        tempCandidateList = db.AdmissionDB.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerateDistrict(acaCalId, admUnitId).ToList();
                                    }

                                    // ==== Filtered by District ID
                                    if (tempCandidateList != null && tempCandidateList.Count > 0)
                                    {
                                        tempCandidateList = tempCandidateList.Where(x => x.DistrictId == districtId).ToList();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "Error loading test rolls 2. " + ex.Message + "; " + ex.InnerException.Message;
                                    lblMessage.ForeColor = Color.Crimson;
                                    messagePanel.CssClass = "alert alert-danger";
                                    return;
                                }

                                if (tempCandidateList != null && tempCandidateList.Count() > 0)
                                {
                                    foreach (var item in tempCandidateList)
                                    {
                                        if (item.cTestRoll == "0")
                                        {
                                            DAL.AdmissionTestRoll admTestRoll = new DAL.AdmissionTestRoll();
                                            admTestRoll.AcaCalID = item.admSetupAcaCalID;
                                            admTestRoll.AdmissionUnitID = item.admSetupAdmUnitID;
                                            admTestRoll.CandidateID = item.candidateID;
                                            admTestRoll.FormSL = item.cFormFormSerial;
                                            //admTestRoll.TestRoll = progBatch + (++testRoll).ToString().PadLeft(6, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                                            admTestRoll.TestRoll = progBatch + item.DistrictId.ToString() + (++testRoll).ToString().PadLeft(5, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                                            admTestRoll.Attribute1 = item.DistrictId.ToString();
                                            admTestRoll.CreatedBy = -99;
                                            admTestRoll.CreatedDate = DateTime.Now;

                                            try
                                            {
                                                using (var db = new CandidateDataManager())
                                                {
                                                    db.Insert<DAL.AdmissionTestRoll>(admTestRoll);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                lblMessage.Text = "Error adding test rolls" + ex.Message + "; " + ex.InnerException.Message;
                                                lblMessage.ForeColor = Color.Crimson;
                                                messagePanel.CssClass = "alert alert-danger";
                                                return;
                                            }


                                            #region N/A
                                            //DAL.CandidateFacultyWiseDistrictSeat cfwds = null;
                                            //try
                                            //{
                                            //    using (var db = new CandidateDataManager())
                                            //    {
                                            //        cfwds = db.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == item.candidateID
                                            //                                                                        && x.AdmissionSetupId == item.admSetupID).FirstOrDefault();
                                            //    }
                                            //}
                                            //catch (Exception ex)
                                            //{

                                            //}

                                            //if (cfwds != null)
                                            //{

                                            //}
                                            //else
                                            //{
                                            //    messageView.Add(item);
                                            //} 
                                            #endregion
                                        }
                                    }
                                }//end if(tempCandidateList != null)

                                #region N/A
                                //if (messageView != null && messageView.Count > 0)
                                //{

                                //    string massageError = "Failed to generate test roll for few candidate <br/>";
                                //    int i = 1;
                                //    foreach (var tData in messageView)
                                //    {
                                //        massageError = massageError + i.ToString() + ") " + tData.cPaymentPaymentId.ToString() + "<br/>";
                                //        i++;
                                //    }

                                //    lblMessage.Text = massageError;
                                //    lblMessage.ForeColor = Color.Crimson;
                                //    messagePanel.CssClass = "alert alert-danger";
                                //    return;
                                //} 
                                #endregion

                                lblMessage.Text = "Generated";
                                lblMessage.ForeColor = Color.Green;
                                messagePanel.CssClass = "alert alert-success";
                                #endregion

                            }


                        }
                    }
                    else
                    {
                        lblMessage.Text = "ProgramDistrictPriority is not setup yet !!";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Exception; Error: " + ex.Message.ToString();
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                    return;
                }

                #region N/A
                ////----delete any existing.
                //List<DAL.AdmissionTestRoll> existingTestRoll = null;
                //try
                //{
                //    using(var db = new OfficeDataManager())
                //    {
                //        existingTestRoll = db.AdmissionDB.AdmissionTestRolls
                //            .Where(c => c.AcaCalID == acaCalId && c.AdmissionUnitID == admUnitId)
                //            .ToList();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    lblMessage.Text = "Error getting existing test rolls. " + ex.Message + "; " + ex.InnerException.Message;
                //    lblMessage.ForeColor = Color.Crimson;
                //    messagePanel.CssClass = "alert alert-danger";
                //}

                //#region N/A
                ////if(existingTestRoll != null)
                ////{
                ////    if(existingTestRoll.Count > 0)
                ////    {
                ////        foreach(DAL.AdmissionTestRoll item in existingTestRoll)
                ////        {
                ////            try
                ////            {
                ////                using(var db = new OfficeDataManager())
                ////                {
                ////                    //db.Delete<DAL.AdmissionTestRoll>(item);
                ////                }
                ////            }
                ////            catch (Exception)
                ////            {
                ////                lblMessage.Text = "Error deleting existing test rolls";
                ////                lblMessage.ForeColor = Color.Crimson;
                ////                messagePanel.CssClass = "alert alert-danger";
                ////            }
                ////        }
                ////    }
                ////}
                ////---- 
                //#endregion

                //int testRoll = 0;
                //string progBatch = "0000"; // "000000";
                //int districtCode = -1; // "000000";

                //DAL.SPCandidateTestRollMaxV2_Result tempAdmissionTestRoll = null;
                //try
                //{
                //    using(var db = new CandidateDataManager())
                //    {
                //        tempAdmissionTestRoll = db.AdmissionDB.SPCandidateTestRollMaxV2(acaCalId, admUnitId).FirstOrDefault();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    lblMessage.Text = "Error loading test rolls 1. " + ex.Message + "; " + ex.InnerException.Message;
                //    lblMessage.ForeColor = Color.Crimson;
                //    messagePanel.CssClass = "alert alert-danger";
                //    return;
                //}

                //if(tempAdmissionTestRoll != null)
                //{
                //    List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result> messageView = new List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result>();

                //    #region Update
                //    //testRoll = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(6, 4));
                //    //progBatch = tempAdmissionTestRoll.TestRoll.Substring(0, 6);

                //    #region Old Substring Pattern
                //    ///// <summary>
                //    ///// Substring Array Index
                //    ///// ---------------01-23-456789
                //    ///// Previous Roll: 10-21-000001
                //    ///// Substring: (0,4) means (0) index thke (4) digit roll thk nibo
                //    ///// -------------------------
                //    ///// -------------------------
                //    ///// Substring Array Index
                //    ///// ---------------01-23-456789
                //    ///// Previous Roll: 10-21-000001
                //    ///// Substring: (4,6) means (4) index thke (6) digit roll thk nibo
                //    ///// </summary>

                //    //testRoll = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(4, 6));
                //    //progBatch = tempAdmissionTestRoll.TestRoll.Substring(0, 4); 
                //    #endregion


                //    /// <summary>
                //    /// Substring Array Index
                //    /// ---------------01-23-4-5678910
                //    /// Previous Roll: 10-21-1-000001
                //    /// Substring: (0,4) means (0) index thke (4) digit roll thk nibo
                //    /// -------------------------
                //    /// -------------------------
                //    /// Substring Array Index
                //    /// ---------------01-23-4-5678910
                //    /// Previous Roll: 10-21-1-000001
                //    /// Substring: (4,1) means (4) index thke (1) digit roll thk nibo
                //    /// -------------------------
                //    /// -------------------------
                //    /// Substring Array Index
                //    /// ---------------01-23-4-5678910
                //    /// Previous Roll: 10-21-1-000001
                //    /// Substring: (5,6) means (5) index thke (6) digit roll thk nibo

                //    /// </summary>


                //    progBatch = tempAdmissionTestRoll.TestRoll.Substring(0, 4);
                //    districtCode = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(4, 1));
                //    testRoll = Convert.ToInt32(tempAdmissionTestRoll.TestRoll.Substring(5, 6));

                //    List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result> tempNewCandidateList = null;
                //    try
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            tempNewCandidateList = db.AdmissionDB.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate(acaCalId, admUnitId).ToList();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        lblMessage.Text = "Error loading test rolls 3. " + ex.Message + "; " + ex.InnerException.Message;
                //        lblMessage.ForeColor = Color.Crimson;
                //        messagePanel.CssClass = "alert alert-danger";
                //        return;
                //    }

                //    if (tempNewCandidateList != null && tempNewCandidateList.Count() > 0)
                //    {
                //        foreach (var item in tempNewCandidateList)
                //        {
                //            if (item.cTestRoll == "0")
                //            {
                //                DAL.CandidateFacultyWiseDistrictSeat cfwds = null;
                //                try
                //                {
                //                    using (var db = new CandidateDataManager())
                //                    {
                //                        cfwds = db.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == item.candidateID
                //                                                                                        && x.AdmissionSetupId == item.admSetupID).FirstOrDefault();
                //                    }
                //                }
                //                catch (Exception ex)
                //                {

                //                }

                //                if (cfwds != null)
                //                {
                //                    DAL.AdmissionTestRoll admTestRoll = new DAL.AdmissionTestRoll();
                //                    admTestRoll.AcaCalID = item.admSetupAcaCalID;
                //                    admTestRoll.AdmissionUnitID = item.admSetupAdmUnitID;
                //                    admTestRoll.CandidateID = item.candidateID;
                //                    admTestRoll.FormSL = item.cFormFormSerial;
                //                    //admTestRoll.TestRoll = progBatch + (++testRoll).ToString().PadLeft(6, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                //                    admTestRoll.TestRoll = progBatch + cfwds.DistrictId.ToString() + (++testRoll).ToString().PadLeft(6, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                //                    admTestRoll.CreatedBy = -99;
                //                    admTestRoll.CreatedDate = DateTime.Now;

                //                    try
                //                    {
                //                        using (var db = new CandidateDataManager())
                //                        {
                //                            db.Insert<DAL.AdmissionTestRoll>(admTestRoll);
                //                        }
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        lblMessage.Text = "Error adding test rolls 2. " + ex.Message + "; " + ex.InnerException.Message;
                //                        lblMessage.ForeColor = Color.Crimson;
                //                        messagePanel.CssClass = "alert alert-danger";
                //                        return;
                //                    }
                //                }
                //                else
                //                {
                //                    messageView.Add(item);
                //                }

                //            }
                //        }
                //    }
                //    #endregion

                //    if (messageView != null && messageView.Count > 0)
                //    {

                //        string massageError = "Failed to generate test roll for few candidate <br/>";
                //        int i = 1;
                //        foreach (var tData in messageView)
                //        {
                //            massageError = massageError + i.ToString() + ") " + tData.cPaymentPaymentId.ToString() + "<br/>";
                //            i++;
                //        }

                //        lblMessage.Text = massageError;
                //        lblMessage.ForeColor = Color.Crimson;
                //        messagePanel.CssClass = "alert alert-danger";
                //        return;
                //    }
                //}
                //else
                //{
                //    List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result> messageView = new List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result>();

                //    #region Create New
                //    string unitCode = "00";
                //    string sessionCode = "00";//"0000";

                //    DAL.AdmissionUnit admUnitObj = null;
                //    DAL.SPAcademicCalendarGetAll_Result acaCal = null;
                //    try
                //    {
                //        using (var db = new OfficeDataManager())
                //        {
                //            admUnitObj = db.AdmissionDB.AdmissionUnits.Find(admUnitId);
                //            acaCal = db.AdmissionDB.SPAcademicCalendarGetAll().Where(c => c.AcademicCalenderID == acaCalId).FirstOrDefault();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        lblMessage.Text = "Error getting faculty and session. " + ex.Message + "; " + ex.InnerException.Message;
                //        lblMessage.ForeColor = Color.Crimson;
                //        messagePanel.CssClass = "alert alert-danger";
                //        return;
                //    }

                //    if (admUnitObj != null)
                //    {
                //        unitCode = admUnitObj.UnitCode2;
                //    }

                //    if (acaCal != null)
                //    {
                //        sessionCode = (acaCal.Year % 100).ToString();//((acaCal.Year % 100) - 1).ToString() + "" + (acaCal.Year % 100).ToString();
                //    }

                //    progBatch = unitCode + sessionCode;
                //    testRoll = 0;

                //    List<DAL.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate_Result> tempCandidateList = null;
                //    try
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            tempCandidateList = db.AdmissionDB.SPCandidateGetAllByAcaCalIdAdmUnitIdForTestRollGenerate(acaCalId, admUnitId).ToList();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        lblMessage.Text = "Error loading test rolls 2. " + ex.Message + "; " + ex.InnerException.Message;
                //        lblMessage.ForeColor = Color.Crimson;
                //        messagePanel.CssClass = "alert alert-danger";
                //        return;
                //    }

                //    if (tempCandidateList != null && tempCandidateList.Count() > 0)
                //    {
                //        foreach (var item in tempCandidateList)
                //        {
                //            if (item.cTestRoll == "0")
                //            {
                //                DAL.CandidateFacultyWiseDistrictSeat cfwds = null;
                //                try
                //                {
                //                    using (var db = new CandidateDataManager())
                //                    {
                //                        cfwds = db.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == item.candidateID
                //                                                                                        && x.AdmissionSetupId == item.admSetupID).FirstOrDefault();
                //                    }
                //                }
                //                catch (Exception ex)
                //                {

                //                }

                //                if (cfwds != null)
                //                {
                //                    DAL.AdmissionTestRoll admTestRoll = new DAL.AdmissionTestRoll();
                //                    admTestRoll.AcaCalID = item.admSetupAcaCalID;
                //                    admTestRoll.AdmissionUnitID = item.admSetupAdmUnitID;
                //                    admTestRoll.CandidateID = item.candidateID;
                //                    admTestRoll.FormSL = item.cFormFormSerial;
                //                    //admTestRoll.TestRoll = progBatch + (++testRoll).ToString().PadLeft(6, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                //                    admTestRoll.TestRoll = progBatch + cfwds.DistrictId.ToString() + (++testRoll).ToString().PadLeft(6, '0'); //progBatch + (++testRoll).ToString().PadLeft(4, '0');
                //                    admTestRoll.Attribute1 = cfwds.DistrictId.ToString();
                //                    admTestRoll.CreatedBy = -99;
                //                    admTestRoll.CreatedDate = DateTime.Now;

                //                    try
                //                    {
                //                        using (var db = new CandidateDataManager())
                //                        {
                //                            db.Insert<DAL.AdmissionTestRoll>(admTestRoll);
                //                        }
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        lblMessage.Text = "Error adding test rolls" + ex.Message + "; " + ex.InnerException.Message;
                //                        lblMessage.ForeColor = Color.Crimson;
                //                        messagePanel.CssClass = "alert alert-danger";
                //                        return;
                //                    }
                //                }
                //                else
                //                {
                //                    messageView.Add(item);
                //                }
                //            }
                //        }
                //    }//end if(tempCandidateList != null)

                //    if (messageView != null && messageView.Count > 0)
                //    {

                //        string massageError = "Failed to generate test roll for few candidate <br/>";
                //        int i = 1;
                //        foreach (var tData in messageView)
                //        {
                //            massageError = massageError + i.ToString() + ") " + tData.cPaymentPaymentId.ToString() + "<br/>";
                //            i++;
                //        }

                //        lblMessage.Text = massageError;
                //        lblMessage.ForeColor = Color.Crimson;
                //        messagePanel.CssClass = "alert alert-danger";
                //        return;
                //    }

                //    lblMessage.Text = "Generated";
                //    lblMessage.ForeColor = Color.Green;
                //    messagePanel.CssClass = "alert alert-success"; 
                //    #endregion

                //} 
                #endregion
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            List<DAL.SPGetTestRollByAcaCalIDAdmUnitID_Result> list = null;
            try
            {
                using(var db = new OfficeDataManager())
                {
                    list = db.AdmissionDB.SPGetTestRollByAcaCalIDAdmUnitID(acaCalId, admUnitId).ToList();
                }
            }
            catch (Exception)
            {
                lblMessage.Text = "Error loading test rolls";
                lblMessage.ForeColor = Color.Crimson;
                messagePanel.CssClass = "alert alert-danger";
            }

            if(list != null)
            {
                if(list.Count > 0)
                {
                    gvTestRoll.DataSource = list.OrderBy(c => c.testRoll).ToList();
                    lblCount.Text = list.Count.ToString();
                }
            }
            else
            {
                gvTestRoll.DataSource = null;
                lblCount.Text = "0";
            }
            gvTestRoll.DataBind();
        }
    }
}