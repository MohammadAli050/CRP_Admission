using Admission.App_Start;
using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class ApplicationPriorityS : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        int noOfPrograms = -1;
        string userName = "";

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            using (var db = new CandidateDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);

                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                }
            }

            bool isUndergradCandidate = true;

            if (!IsPostBack)
            {
                divNotEligible.Visible = false;

                btnNext.Visible = false;
                //btnSave.Enabled = true;
                if (uId > 0)
                {
                    long candidateId = -1;
                    using (var db = new CandidateDataManager())
                    {
                        candidateId = db.GetCandidateIdByUserID_ND(uId);
                    }
                    if (candidateId > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                                    = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(candidateId, false).ToList();

                            if (programsAppliedList.Count() > 0)
                            {
                                foreach (var item in programsAppliedList)
                                {
                                    if (item.EducationCategoryID == 6)
                                    {
                                        isUndergradCandidate = false;
                                    }
                                }
                            }
                        }
                    }
                    //if (isUndergradCandidate == true)
                    //{
                        Panel_GridView.Visible = true;
                        Panel_Master.Visible = false;
                        lblMessage_Masters.Text = string.Empty;
                        LoadDDL();
                        LoadCandidateData();
                        CheckAllEligiblePriority();
                    //}
                    //else
                    //{
                    //    Panel_GridView.Visible = false;
                    //    Panel_Master.Visible = true;
                    //    lblMessage_Masters.Text = "Program priority/choice is for only candidates applying for bachelors degree.<br/><br/>";
                    //}
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        public long CurrentProgramPriorityID
        {
            get
            {
                if (ViewState["CurrentProgramPriorityID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentProgramPriorityID"].ToString());
            }
            set
            {
                ViewState["CurrentProgramPriorityID"] = value;
            }
        }

        private void LoadDDL()
        {
            long cId = -1;

            if (uId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        cId = db.GetCandidateIdByUserID_ND(uId);
                    }
                }
                catch (Exception ex)
                {
                    lblMessage_Masters.Text = "Error: Unable to get candidate.";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }
            }

            if (cId > 0)
            {
                List<DAL.CandidateFormSl> cFormList = null;

                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                        if (AllStepInOneTime == 1)
                        {
                            bool Ispaid = false;
                            var CandidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                            if (CandidatePayment != null && CandidatePayment.IsPaid != null && Convert.ToBoolean(CandidatePayment.IsPaid))
                                Ispaid = true;

                            cFormList = db.GetAllCandidateFormSlByCandIDIsPaid_AD(cId, Ispaid).ToList();
                        }
                        else
                            cFormList = db.GetAllCandidateFormSlByCandIDIsPaid_AD(cId, true).ToList();

                        //cFormList = db.GetAllCandidateFormSlByCandIDIsPaid_AD(cId, false).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage_Masters.Text = "Error: Unable to get candidate form serial.";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }

                List<DAL.AdmissionSetup> admSetupList = null;

                if (cFormList != null)
                {
                    if (cFormList.Count > 0)
                    {
                        admSetupList = cFormList.Select(c => c.AdmissionSetup).ToList();
                    }
                }

                List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();

                if (admSetupList != null)
                {
                    if (admSetupList.Count > 0)
                    {
                        foreach (var item in admSetupList)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                List<DAL.AdmissionUnit> _temp = new List<DAL.AdmissionUnit>();
                                _temp = db.AdmissionDB.AdmissionUnits.Where(c => c.ID == item.AdmissionUnit.ID).ToList();
                                if (_temp != null && _temp.Count > 0)
                                {
                                    admUnitList.AddRange(_temp);
                                }
                            }
                        }//end foreach
                    }
                }//end admSetupList != null

                if (admUnitList != null)
                {
                    if (admUnitList.Count() > 0)
                    {
                        DDLHelper.Bind<DAL.AdmissionUnit>(ddlFaculty, admUnitList.OrderBy(c => c.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.Select);
                    }
                }

                ddlProgram.Items.Add(new ListItem("Select Faculty", "-1"));
                ddlChoice.Items.Add(new ListItem("Select", "-1"));

            }
            else
            {
                lblMessage_Masters.Text = "Error: Unable to get candidate (1).";
                Panel_Master.CssClass = "alert alert-danger";
                Panel_Master.Visible = true;
                return;
            }
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProgram.Items.Clear();
            long admUnitId = -1;
            int acaCalId = 0;

            gvNotEligibleProgramList.DataSource = null;
            gvNotEligibleProgramList.DataBind();
            divNotEligible.Visible = false;

            admUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);

            List<DAL.AdmissionUnitProgram> list = null;

            using (var db = new CandidateDataManager())
            {
                //cId = db.GetCandidateIdByUserID_ND(uId);
                if (cId > 0)
                {
                    DAL.CandidatePayment _tempCandidatePaymentObj = db.GetCandidatePaymentByCandidateID(cId);
                    if (_tempCandidatePaymentObj != null)
                    {
                        acaCalId = Convert.ToInt32(_tempCandidatePaymentObj.AcaCalID);
                    }
                }
            }

            if (admUnitId > 0)
            {
                try
                {


                    using (var db = new OfficeDataManager())
                    {
                        List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                                   = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, false).ToList();

                        int categoryId = -1;
                        if (programsAppliedList != null && programsAppliedList.Count() > 0)
                        {
                            var programObj = programsAppliedList.Where(x => x.AdmissionUnitID == admUnitId).FirstOrDefault();
                            if (programObj != null)
                            {
                                categoryId = Convert.ToInt32(programObj.EducationCategoryID);
                            }
                        }

                        list = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == admUnitId
                                                                            && c.AcaCalID == acaCalId
                                                                            && c.EducationCategoryID == categoryId
                                                                            && c.IsActive == true).ToList();
                    }
                }
                catch (Exception)
                {
                    lblMessage_Masters.Text = "Error: Unable to get programs (1).";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }

                if (list != null && list.Count > 0)
                {
                    List<DAL.AdmissionUnitProgram> NotEligibleProgramList = new List<DAL.AdmissionUnitProgram>();

                    #region New Logic

                    /// For FSSS (Candidate get all program)
                    ///

                    /// For FASS (All Program Except DMR,Economics And English). Some condition for these three program
                    ///

                    ///     For DMR ..Candidates applying for admission in the Department of Disaster
                    /// Management and Resilience must have studied Higher Mathematics/ Statistics
                    /// in HSC / equivalent level and scored minimum 'A-'(A Minus) grade in the examination.

                    ///     For Economics ..Candidates applying for admission in the Department of Economics must
                    /// have minimum 'A-'(A Minus) grade in Economics / Accounting / Statistics / Higher
                    /// Mathematics either in SSC / equivalent or HSC/ equivalent examination.

                    ///     For English ..Candidates applying for admission in the Department of English must
                    /// have minimum 'A-'(A Minus) grade in English both in SSC / equivalent and
                    /// HSC / equivalent examinations.

                    /// For FST (All Program Except ICE,CSE And ES). Some condition for these three program
                    ///     For ICE & CSE candidate must have Mathmatics subject in HSC
                    ///     
                    ///     For ES candidate must have Biology subject in HSC


                    #endregion


                    #region Faculty Info from Database

                    //ID  UnitName                                        Attribute1
                    //3   Faculty of Arts & Social Science                    FASS
                    //4   Faculty of Security & Strategic Studies             FSSS
                    //5   Faculty of Science & Technology                     FST
                    #endregion

                  

                    DDLHelper.Bind<DAL.AdmissionUnitProgram>(ddlProgram, list.OrderBy(c => c.ProgramName).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Select);


                    if (NotEligibleProgramList != null && NotEligibleProgramList.Any())
                    {
                        divNotEligible.Visible = true;
                        gvNotEligibleProgramList.DataSource = NotEligibleProgramList;
                        gvNotEligibleProgramList.DataBind();
                    }


                }
                else
                {
                    ddlProgram.Items.Clear();
                    ddlProgram.Items.Add(new ListItem("No Programs!", "-1"));
                }
            }
            else
            {
                ddlProgram.Items.Clear();
                ddlProgram.Items.Add(new ListItem("Error!", "-1"));
            }
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlChoice.Items.Clear();
            ddlChoice.Items.Add(new ListItem("Select", "-1"));
            ddlChoice.AppendDataBoundItems = true;

            long admUnitProgID = -1;

            admUnitProgID = Convert.ToInt64(ddlProgram.SelectedValue);

            if (admUnitProgID > 0)
            {
                for (int i = 1; i < 21; i++)
                {
                    ddlChoice.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            else
            {
                ddlChoice.Items.Clear();
                ddlChoice.Items.Add(new ListItem("Select", "-1"));
            }
        }

        private void LoadCandidateData()
        {
            lvProgramPriority.DataSource = null;
            lvProgramPriority.DataBind();
            btnNext.Visible = false;

            //long cId = -1;

            //if (uId > 0)
            //{
            //    try
            //    {
            //        using (var db = new CandidateDataManager())
            //        {
            //            cId = db.GetCandidateIdByUserID_ND(uId);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        lblMessage_Masters.Text = "Error: Unable to get candidate.";
            //        Panel_Master.CssClass = "alert alert-danger";
            //        Panel_Master.Visible = true;
            //        return;
            //    }
            //}

            if (uId > 0 && cId > 0)
            {

                #region Get EducationCategoryId & ProgramId
                int educationCategoryId = -1;
                int programId = -1;
                using (var db = new CandidateDataManager())
                {
                    educationCategoryId = db.GetCandidateEducationCategoryID(cId);

                    if (educationCategoryId != 4)
                    {
                        DAL.CandidateFormSl formSerial = db.GetCandidateFormSlByCandID_AD(cId);

                        if (formSerial != null && formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms != null)
                        {
                            programId = formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.FirstOrDefault().ProgramID;
                        }
                    }
                }
                #endregion

                #region Get IsFinalSubmit
                bool isFinalSubmit = false;
                DAL.AdditionalInfo additionalInfo = null;
                using (var db = new CandidateDataManager())
                {
                    additionalInfo = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                }
                if (additionalInfo != null)
                {
                    isFinalSubmit = Convert.ToBoolean(additionalInfo.IsFinalSubmit);
                }
                #endregion

                #region Property Setup (Candidate Submit Button Show/Hide)
                List<DAL.PropertySetup> propertySetupList = null;
                using (var db = new OfficeDataManager())
                {
                    propertySetupList = db.AdmissionDB.PropertySetups.Where(x => x.IsActive == true
                                                                                && x.EducationCategoryID == educationCategoryId).ToList();
                }

                if (propertySetupList != null && propertySetupList.Count > 0)
                {
                    if (educationCategoryId == 4)
                    {
                        #region Bachelor

                        #region Candidate Submit Button Show/Hide
                        try
                        {
                            var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)).FirstOrDefault();
                            if (candidateSubmitButtonSetup != null)
                            {
                                bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);

                                if (showHide == true)
                                {
                                    //btnSave.Visible = !isFinalSubmit;
                                    btnSave.Visible = true;

                                }
                                else
                                {
                                    //btnSave.Visible = false;
                                    btnSave.Visible = true;
                                }

                            }
                            else
                            {
                                //btnSave.Visible = false;
                                btnSave.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            btnSave.Visible = false;
                        }
                        #endregion


                        #endregion
                    }
                    else
                    {
                        #region Masters

                        #region Candidate Submit Button Show/Hide
                        try
                        {
                            var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)
                                                                                        && x.ProgramId == programId).FirstOrDefault();
                            if (candidateSubmitButtonSetup != null)
                            {
                                bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);
                                if (showHide == true)
                                {
                                    //btnSave.Visible = !isFinalSubmit;
                                    btnSave.Visible = true;
                                }
                                else
                                {
                                    //btnSave.Visible = false;
                                    btnSave.Visible = true;
                                }
                            }
                            else
                            {
                                //btnSave.Visible = false;
                                btnSave.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            btnSave.Visible = false;
                        }
                        #endregion

                        #endregion
                    }
                }
                else
                {
                    btnSave.Visible = false;
                }
                #endregion

                #region Load List View Data
                List<DAL.SPGetCandidateProgramPriorities_Result> list = null;

                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        list = db.AdmissionDB.SPGetCandidateProgramPriorities(cId).ToList();
                    }
                }
                catch (Exception)
                {
                    lblMessage_Masters.Text = "Error: Unable to get candidate choices.";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }

                if (list != null && list.Count > 0)
                {
                    lvProgramPriority.DataSource = list.OrderBy(c => c.cP_Priority).OrderBy(c => c.admUnit_Name).ToList();
                    lvProgramPriority.DataBind();
                    //btnNext.Visible = true;
                }
                else
                {
                    lvProgramPriority.DataSource = null;
                    lvProgramPriority.DataBind();
                    btnNext.Visible = false;
                }
                #endregion


                #region N/A -- Prevent Save if IsApproved from Admin
                //try
                //{
                //    //List<DAL.SPGetCandidateEducationCategoryByCandidateID_Result> list1 = null;
                //    List<DAL.ApprovedList> approvedList = null;
                //    using (var db = new GeneralDataManager())
                //    {
                //        //list1 = db.AdmissionDB.SPGetCandidateEducationCategoryByCandidateID(cId).ToList();
                //        approvedList = db.AdmissionDB.ApprovedLists.Where(x=> x.CandidateID == cId).ToList();

                //        if (approvedList.Count > 0)
                //        {
                //            DAL.ApprovedList al = approvedList.FirstOrDefault();
                //            if (al != null)
                //            {
                //                btnSave.Enabled = false;
                //                btnSave.Visible = false;
                //            }
                //            else
                //            {
                //                btnSave.Enabled = true;
                //                btnSave.Visible = true;
                //            }
                //        }
                //        else
                //        {
                //            btnSave.Enabled = true;
                //            btnSave.Visible = true;
                //        }

                //    }

                //    #region Candidate Can't change ProgramPriority After Final Submission
                //    #region N/A
                //    ////if (list1 != null)
                //    ////{
                //    ////    DAL.SPGetCandidateEducationCategoryByCandidateID_Result undergradCandidate =
                //    ////        list1.Where(c => c.EduCatID == 4).Take(1).FirstOrDefault();

                //    ////    if (undergradCandidate != null)
                //    ////    {
                //    ////        btnNext.Enabled = false;
                //    ////        btnNext.Visible = false;

                //    ////        btnSave.Enabled = false;
                //    ////        btnSave.Visible = false;

                //    ////        if (undergradCandidate.IsApproved != null)
                //    ////        {
                //    ////            if (undergradCandidate.IsApproved == true)
                //    ////            {
                //    ////                btnNext.Enabled = false;
                //    ////                btnNext.Visible = false;

                //    ////                btnSave.Enabled = false;
                //    ////                btnSave.Visible = false;
                //    ////            }
                //    ////            else
                //    ////            {
                //    ////                btnNext.Enabled = true;
                //    ////                btnNext.Visible = true;

                //    ////                btnSave.Enabled = true;
                //    ////                btnSave.Visible = true;
                //    ////            }
                //    ////        }
                //    ////        else
                //    ////        {
                //    ////            btnNext.Enabled = true;
                //    ////            btnNext.Visible = true;

                //    ////            btnSave.Enabled = true;
                //    ////            btnSave.Visible = true;
                //    ////        }

                //    ////        if (undergradCandidate.IsFinalSubmit != null)
                //    ////        {
                //    ////            if (undergradCandidate.IsFinalSubmit == true)
                //    ////            {
                //    ////                btnNext.Enabled = false;
                //    ////                btnNext.Visible = false;

                //    ////                btnSave.Enabled = false;
                //    ////                btnSave.Visible = false;
                //    ////            }
                //    ////            else
                //    ////            {
                //    ////                btnNext.Enabled = true;
                //    ////                btnNext.Visible = true;

                //    ////                btnSave.Enabled = true;
                //    ////                btnSave.Visible = true;
                //    ////            }
                //    ////        }
                //    ////        else
                //    ////        {
                //    ////            btnNext.Enabled = true;
                //    ////            btnNext.Visible = true;

                //    ////            btnSave.Enabled = true;
                //    ////            btnSave.Visible = true;
                //    ////        }
                //    ////    }

                //    ////    DAL.SPGetCandidateEducationCategoryByCandidateID_Result gradCandidate =
                //    ////        list1.Where(c => c.EduCatID == 6).Take(1).FirstOrDefault();

                //    ////    if (gradCandidate != null)
                //    ////    {
                //    ////        if (gradCandidate.IsApproved != null)
                //    ////        {
                //    ////            //if (gradCandidate.IsApproved == true)
                //    ////            //{
                //    ////            //    btnNext.Enabled = false;
                //    ////            //    btnNext.Visible = false;
                //    ////            //}
                //    ////            //else
                //    ////            //{
                //    ////            //    btnNext.Enabled = true;
                //    ////            //    btnNext.Visible = true;
                //    ////            //}
                //    ////        }

                //    ////        if (gradCandidate.IsFinalSubmit != null)
                //    ////        {
                //    ////            //if (gradCandidate.IsFinalSubmit == true)
                //    ////            //{
                //    ////            //    btnNext.Enabled = false;
                //    ////            //    btnNext.Visible = false;
                //    ////            //}
                //    ////            //else
                //    ////            //{
                //    ////            //    btnNext.Enabled = true;
                //    ////            //    btnNext.Visible = true;
                //    ////            //}
                //    ////        }
                //    ////    }

                //    ////}  
                //    #endregion

                //    try
                //    {
                //        DAL.AdditionalInfo adiInfo = null;
                //        using (var db = new CandidateDataManager())
                //        {
                //            adiInfo = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                //        }

                //        if (adiInfo != null && Convert.ToBoolean(adiInfo.IsFinalSubmit) == true)
                //        {
                //            btnSave.Enabled = false;
                //            btnSave.Visible = false;
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //    }


                //    #endregion


                //}
                //catch (Exception)
                //{

                //    throw;
                //}
                #endregion






                #region N/A -- Hide Save and Next Button for Bachelor Program Because Admission is closed
                //try
                //{
                //    DAL.CandidateFormSl cfs = null;
                //    using (var db = new CandidateDataManager())
                //    {
                //        cfs = db.GetCandidateFormSlByCandID_AD(cId);
                //    }

                //    if (cfs != null && cfs.AdmissionSetup != null)
                //    {
                //        List<DAL.PropertySetup> propertySetupList = null; //new DAL.CandidateFormSl();
                //        using (var db = new OfficeDataManager())
                //        {
                //            propertySetupList = db.AdmissionDB.PropertySetups.Where(x => x.IsActive == true).ToList();
                //        }

                //        ///<summary>
                //        ///
                //        /// IsActive == true && IsVisible == false
                //        /// Kew Submit Button Dekte prbe na.
                //        /// jokon admission date sas hoea jbe
                //        /// 
                //        /// 
                //        /// IsActive == true && IsVisible == true 
                //        /// sober jnno Open thkbe. Final Submit Dileo
                //        /// 
                //        /// 
                //        /// IsActive == false && IsVisible == any
                //        /// Sober jnno Open but final Submit dile r Show korbe na tader jnno
                //        /// 
                //        /// </summary>

                //        //... Save Button
                //        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == cfs.AdmissionSetup.EducationCategoryID).FirstOrDefault() != null)
                //        {
                //            bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == cfs.AdmissionSetup.EducationCategoryID).FirstOrDefault().IsVisible);

                //            btnSave.Visible = isVisible;
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                #endregion




            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long cId = -1;

            DAL.BasicInfo basicInfo = null;
            DAL.CandidatePayment candidatePayment = null;

            if (uId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        cId = db.GetCandidateIdByUserID_ND(uId);

                        basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                        candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage_Masters.Text = "Error: Unable to get candidate 2.";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }
            }

            if (cId > 0)
            {
                #region N/A
                //if (CurrentProgramPriorityID > 0) //update
                //{
                //    DAL.ProgramPriority objToUpdate = null;

                //    long id = CurrentProgramPriorityID;

                //    try
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            objToUpdate = db.AdmissionDB.ProgramPriorities.Find(id);
                //        }
                //    }
                //    catch (Exception)
                //    {
                //        lblMessage_Masters.Text = "Error: Unable to get candidate choice for update.";
                //        Panel_Master.CssClass = "alert alert-danger";
                //        Panel_Master.Visible = true;
                //        return;
                //    }

                //    if (objToUpdate != null)
                //    {
                //        DAL.AdmissionSetup _temp = null;
                //        try
                //        {
                //            long _tempAdmUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                //            using (var db = new OfficeDataManager())
                //            {
                //                _temp = db.AdmissionDB.AdmissionSetups.Where(c => c.AdmissionUnitID == _tempAdmUnitId).FirstOrDefault();
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage_Masters.Text = "Error: Unable to get exam.";
                //            Panel_Master.CssClass = "alert alert-danger";
                //            Panel_Master.Visible = true;
                //            return;
                //        }

                //        DAL.AdmissionUnitProgram _tempProg = null;
                //        try
                //        {
                //            long _tempAdmUnitProgId = Convert.ToInt64(ddlProgram.SelectedValue);
                //            using (var db = new OfficeDataManager())
                //            {
                //                _tempProg = db.AdmissionDB.AdmissionUnitPrograms.Find(_tempAdmUnitProgId);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage_Masters.Text = "Error: Unable to get program.";
                //            Panel_Master.CssClass = "alert alert-danger";
                //            Panel_Master.Visible = true;
                //            return;
                //        }


                //        if (_temp != null && _tempProg != null)
                //        {
                //            objToUpdate.AdmissionUnitID = Convert.ToInt64(ddlFaculty.SelectedValue);
                //            objToUpdate.AdmissionUnitProgramID = Convert.ToInt64(ddlProgram.SelectedValue);
                //            objToUpdate.AdmissionSetupID = _temp.ID;
                //            objToUpdate.BatchID = _tempProg.BatchID;
                //            objToUpdate.AcaCalID = _tempProg.AcaCalID;
                //            objToUpdate.ProgramID = _tempProg.ProgramID;
                //            objToUpdate.ProgramName = _tempProg.ProgramName;
                //            objToUpdate.ShortName = _tempProg.ShortCode;
                //            objToUpdate.Priority = Convert.ToInt32(ddlChoice.SelectedValue);
                //            objToUpdate.DateModified = DateTime.Now;
                //            objToUpdate.ModifiedBy = cId;

                //            DAL.ProgramPriority _tempExistingObj = null;
                //            try
                //            {
                //                using (var db = new CandidateDataManager())
                //                {
                //                    _tempExistingObj = db.AdmissionDB.ProgramPriorities
                //                        .Where(c => c.CandidateID == cId &&
                //                                c.AdmissionUnitID == objToUpdate.AdmissionUnitID &&
                //                                c.AdmissionUnitProgramID == objToUpdate.AdmissionUnitProgramID &&
                //                                c.AdmissionSetupID == objToUpdate.AdmissionSetupID &&
                //                                c.Priority == objToUpdate.Priority).FirstOrDefault();
                //                }
                //            }
                //            catch (Exception)
                //            {
                //                lblMessage_Masters.Text = "Error: Unable to get existing choice.";
                //                Panel_Master.CssClass = "alert alert-danger";
                //                Panel_Master.Visible = true;
                //                return;
                //            }

                //            if (_tempExistingObj == null) //update if not found
                //            {
                //                try
                //                {
                //                    using (var dbUpdate = new CandidateDataManager())
                //                    {
                //                        dbUpdate.Delete<DAL.ProgramPriority>(objToUpdate);
                //                    }

                //                    lblMessage_Masters.Text = "Update Successful";
                //                    Panel_Master.CssClass = "alert alert-success";
                //                    Panel_Master.Visible = true;
                //                }
                //                catch (Exception)
                //                {
                //                    lblMessage_Masters.Text = "Error: Unable to update choice.";
                //                    Panel_Master.CssClass = "alert alert-danger";
                //                    Panel_Master.Visible = true;
                //                    return;
                //                }
                //            }
                //            else
                //            {
                //                lblMessage_Masters.Text = "Another choice exist with same Faculty, Program and Choice. Please select a different choice.";
                //                Panel_Master.CssClass = "alert alert-warning";
                //                Panel_Master.Visible = true;
                //                return;
                //            } //end if-else _tempExistingObj == null
                //        }
                //        else
                //        {
                //            lblMessage_Masters.Text = "Error: Unable to get load exam and program.";
                //            Panel_Master.CssClass = "alert alert-danger";
                //            Panel_Master.Visible = true;
                //            return;
                //        } //end if-else _temp != null && _tempProg != null
                //    }
                //}

                //if (cId > 0) //create new
                //{ 
                #endregion
                DAL.ProgramPriority objToCreate = new DAL.ProgramPriority();

                DAL.AdmissionSetup _tempAdm = null;
                try
                {
                    long _tempAdmUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                    using (var db = new OfficeDataManager())
                    {
                        _tempAdm = db.AdmissionDB.AdmissionSetups.Where(c => c.AdmissionUnitID == _tempAdmUnitId && c.IsActive == true).FirstOrDefault();
                    }
                }
                catch (Exception)
                {
                    lblMessage_Masters.Text = "Error: Unable to get exam. 1";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }

                DAL.AdmissionUnitProgram _tempAdmProg = null;
                try
                {
                    long _tempAdmUnitProgId = Convert.ToInt64(ddlProgram.SelectedValue);
                    using (var db = new OfficeDataManager())
                    {
                        _tempAdmProg = db.AdmissionDB.AdmissionUnitPrograms.Find(_tempAdmUnitProgId);
                    }
                }
                catch (Exception)
                {
                    lblMessage_Masters.Text = "Error: Unable to get program. 1";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }




                if (_tempAdm != null && _tempAdmProg != null)
                {

                    int maxPriority = 0;
                    List<DAL.ProgramPriority> programPrioritylist = new List<DAL.ProgramPriority>();
                    long admUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            programPrioritylist = db.AdmissionDB.ProgramPriorities.Where(d => d.CandidateID == cId && d.AdmissionUnitID == admUnitId && d.AcaCalID == _tempAdmProg.AcaCalID).ToList();
                            if (programPrioritylist != null)
                            {
                                maxPriority = Convert.ToInt32(programPrioritylist.Select(d => d.Priority).Max());
                                maxPriority = maxPriority + 1;
                            }
                            else
                            {
                                maxPriority = 1;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage_Masters.Text = "Error: Unable to get priority. 1";
                        Panel_Master.CssClass = "alert alert-danger";
                        Panel_Master.Visible = true;
                        return;
                    }

                    objToCreate.AdmissionUnitID = Convert.ToInt64(ddlFaculty.SelectedValue);
                    objToCreate.AdmissionUnitProgramID = Convert.ToInt64(ddlProgram.SelectedValue);
                    objToCreate.AdmissionSetupID = _tempAdm.ID;
                    objToCreate.BatchID = _tempAdmProg.BatchID;
                    objToCreate.AcaCalID = _tempAdmProg.AcaCalID;
                    objToCreate.ProgramID = _tempAdmProg.ProgramID;
                    objToCreate.ProgramName = _tempAdmProg.ProgramName;
                    objToCreate.ShortName = _tempAdmProg.ShortCode;
                    objToCreate.Priority = maxPriority;//Convert.ToInt32(ddlChoice.SelectedValue);
                    objToCreate.DateModified = null;
                    objToCreate.ModifiedBy = null;

                    objToCreate.CandidateID = cId;
                    objToCreate.DateCreated = DateTime.Now;
                    objToCreate.CreatedBy = uId;

                    DAL.ProgramPriority _tempExistingObj = null;
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            _tempExistingObj = db.AdmissionDB.ProgramPriorities
                                .Where(c => c.CandidateID == cId &&
                                        c.AdmissionUnitID == objToCreate.AdmissionUnitID &&
                                        c.AdmissionUnitProgramID == objToCreate.AdmissionUnitProgramID &&
                                        c.AdmissionSetupID == objToCreate.AdmissionSetupID).FirstOrDefault();
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage_Masters.Text = "Error: Unable to get existing choice.";
                        Panel_Master.CssClass = "alert alert-danger";
                        Panel_Master.Visible = true;
                        return;
                    }

                    if (_tempExistingObj == null) //insert if not found
                    {
                        try
                        {
                            using (var dbInsert = new CandidateDataManager())
                            {
                                dbInsert.Insert<DAL.ProgramPriority>(objToCreate);
                            }

                            lblMessage_Masters.Text = "Successful";
                            Panel_Master.CssClass = "alert alert-success";
                            Panel_Master.Visible = true;


                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //dLog.DateCreated = DateTime.Now;
                                //dLog.EventName = "ProgramPriority Info Insert (Admin)";
                                //dLog.PageName = "CandApplicationEducation.aspx";
                                //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert ProgramPriority Information.";
                                //dLog.UserId = uId;
                                //dLog.Attribute1 = "Success";
                                //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //LogWriter.DataLogWriter(dLog);

                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "ProgramPriority Info Insert (Candidate)";
                                    dLog.PageName = "ApplicationPriority.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert ProgramPriority Information.";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion



                        }
                        catch (Exception)
                        {
                            lblMessage_Masters.Text = "Error: Unable to save choice.";
                            Panel_Master.CssClass = "alert alert-danger";
                            Panel_Master.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblMessage_Masters.Text = "Another choice exist with same Faculty, Program and Choice. Please select a different choice.";
                        Panel_Master.CssClass = "alert alert-warning";
                        Panel_Master.Visible = true;
                        return;
                    } //end if-else _tempExistingObj == null
                }
                else
                {
                    lblMessage_Masters.Text = "Error: Unable to get load exam and program. 1";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                } //end if-else _tempAdm != null && _tempAdmProg != null
                //} //end if-else CurrentProgramPriorityID > 0

                btnSave.Text = "Add";

                ddlFaculty.SelectedValue = "-1";
                ddlProgram.SelectedValue = "-1";
                ddlChoice.SelectedValue = "-1";

                LoadCandidateData();

                CheckAllEligiblePriority();

                CurrentProgramPriorityID = 0;
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            long cId = -1;

            bool isUndergradCandidate = true;

            if (uId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        cId = db.GetCandidateIdByUserID_ND(uId);
                    }
                }
                catch (Exception ex)
                {
                    lblMessage_Masters.Text = "Error: Unable to get candidate 2.";
                    Panel_Master.CssClass = "alert alert-danger";
                    Panel_Master.Visible = true;
                    return;
                }
            }


            if (cId > 0)
            {

                using (var db = new CandidateDataManager())
                {
                    List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                            = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, false).ToList();

                    if (programsAppliedList.Count() > 0)
                    {
                        foreach (var item in programsAppliedList)
                        {
                            if (item.EducationCategoryID == 6)
                            {
                                isUndergradCandidate = false;
                            }
                        }
                    }
                }

                if (isUndergradCandidate == true) //is bachelors then check for priority
                {

                    try
                    {
                        int countAllFacultyPriorityOneIsGiven = 0;
                        List<DAL.CandidateFormSl> cformserList = null;
                        using (var db = new CandidateDataManager())
                        {
                            cformserList = db.GetAllCandidateFormSlByCandID_AD(cId);

                            if (cformserList != null && cformserList.Count > 0)
                            {
                                foreach (var tData in cformserList)
                                {
                                    DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId
                                                                                                      && x.AdmissionSetupID == tData.AdmissionSetupID
                                                                                                      && x.Priority == 1).FirstOrDefault();

                                    if (pp == null)
                                    {
                                        countAllFacultyPriorityOneIsGiven++;
                                    }
                                }
                            }
                        }


                        if (countAllFacultyPriorityOneIsGiven > 0)
                        {
                            lblMessage_Masters.Text = "Please provide Choice 1 for Eache Faculty!";
                            Panel_Master.CssClass = "alert alert-danger";
                            Panel_Master.Visible = true;
                            return;
                        }
                        else
                        {
                            Response.Redirect("ApplicationRelation.aspx", false);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    //List<DAL.ProgramPriority> list = null;

                    //try
                    //{
                    //    using (var db = new CandidateDataManager())
                    //    {
                    //        list = db.AdmissionDB.ProgramPriorities.Where(c => c.CandidateID == cId).ToList();
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //    lblMessage_Masters.Text = "Error: Unable to get candidate choice list from database.";
                    //    Panel_Master.CssClass = "alert alert-danger";
                    //    Panel_Master.Visible = true;
                    //    return;
                    //}

                    //if (list == null)//if no choices found (choice 1 and 2)
                    //{
                    //    lblMessage_Masters.Text = "Choices not found. Please save at least two choices (choice 1 & choice 2) to proceed.";
                    //    Panel_Master.CssClass = "alert alert-danger";
                    //    Panel_Master.Visible = true;
                    //    return;
                    //}

                    //else
                    //{
                    //    DAL.ProgramPriority choice1 = null;
                    //    DAL.ProgramPriority choice2 = null;

                    //    choice1 = list.Where(c => c.Priority == 1).FirstOrDefault();
                    //    choice2 = list.Where(c => c.Priority == 2).FirstOrDefault();

                    //    if (choice1 == null && choice2 != null)
                    //    {
                    //        lblMessage_Masters.Text = "Choice 1 not found. Please save a program as your choice 1 (first choice) to proceed.";
                    //        Panel_Master.CssClass = "alert alert-danger";
                    //        Panel_Master.Visible = true;
                    //        return;
                    //    }
                    //    else if (choice1 != null && choice2 == null)
                    //    {
                    //        lblMessage_Masters.Text = "Choice 2 not found. Please save a program as your choice 2 (second choice) to proceed.";
                    //        Panel_Master.CssClass = "alert alert-danger";
                    //        Panel_Master.Visible = true;
                    //        return;
                    //    }
                    //    else if (choice1 == null && choice2 == null)
                    //    {
                    //        lblMessage_Masters.Text = "Choices not found. Please save at least two choices (choice 1 & choice 2) to proceed.";
                    //        Panel_Master.CssClass = "alert alert-danger";
                    //        Panel_Master.Visible = true;
                    //        return;
                    //    }
                    //    else if (choice1 != null && choice2 != null)
                    //    {
                    //        Response.Redirect("ApplicationEducation.aspx", false);
                    //    }
                    //} //end if-else list == null
                }
                else //else if masters then proceed without checking.
                {
                    Response.Redirect("ApplicationRelation.aspx", false);
                } //end if-else isUndergradCandidate == true
            }
        }

        protected void lvProgramPriority_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetCandidateProgramPriorities_Result progP = (DAL.SPGetCandidateProgramPriorities_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
                Label lblProgramName = (Label)currentItem.FindControl("lblProgramName");
                Label lblChoice = (Label)currentItem.FindControl("lblChoice");

                LinkButton lnkRemove = (LinkButton)currentItem.FindControl("lnkRemove");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                //lblUnitName.Text = progP.admUnit_Name;
                lblProgramName.Text = progP.admUnitProgZ_ProgramName;
                lblChoice.Text = progP.cP_Priority.ToString();

                lnkRemove.CommandName = "Remove";
                lnkRemove.CommandArgument = progP.cP_ID.ToString();

                bool isFinalSubmit = false;
                DAL.AdditionalInfo additionalInfo = null;
                using (var db = new CandidateDataManager())
                {
                    additionalInfo = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                }
                if (additionalInfo != null)
                {
                    isFinalSubmit = Convert.ToBoolean(additionalInfo.IsFinalSubmit);
                }

                //lnkRemove.Visible = !isFinalSubmit;
                lnkRemove.Visible = true; // Button Always Visible. Req giben By Rifa(26_02_2023)




            }
        }

        protected void lvProgramPriority_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                //btnSave.Text = "Update";
                using (var db = new GeneralDataManager())
                {
                    var objectToRemove = db.AdmissionDB.ProgramPriorities.Find(id);

                    if (objectToRemove != null)
                    {
                        long candidateId = Convert.ToInt64(objectToRemove.CandidateID);
                        int acacalId = Convert.ToInt32(objectToRemove.AcaCalID);
                        long admissionUnitId = Convert.ToInt64(objectToRemove.AdmissionUnitID);
                        int priority = Convert.ToInt32(objectToRemove.Priority);

                        //DAL.ProgramPriority adjustObj = objectToRemove;
                        db.Delete<DAL.ProgramPriority>(objectToRemove);

                        AdjustProgramPriority(candidateId, acacalId, admissionUnitId, priority);
                        LoadCandidateData();
                    }

                    //LoadDDL();
                    //ddlFaculty.SelectedValue = objectToUpdate.AdmissionUnitID.ToString();

                    //ddlFaculty_SelectedIndexChanged(sender, e);
                    //ddlProgram.SelectedValue = objectToUpdate.AdmissionUnitProgramID.ToString();

                    //ddlProgram_SelectedIndexChanged(sender, e);
                    //ddlChoice.SelectedValue = objectToUpdate.Priority.ToString();

                    CurrentProgramPriorityID = objectToRemove.ID;
                }
            }
        }

        private void AdjustProgramPriority(long candidateId, int acacalId, long admissionUnitId, int priority)
        {


            int currentProgramPriority = Convert.ToInt32(priority);
            using (var db = new GeneralDataManager())
            {
                List<DAL.ProgramPriority> list = db.AdmissionDB.ProgramPriorities.Where(d => d.CandidateID == candidateId && d.AdmissionUnitID == admissionUnitId && d.AcaCalID == acacalId && d.Priority > priority).OrderBy(d => d.Priority).ToList();
                //
                //list = db.AdmissionDB.SPGetCandidateProgramPriorities(cId).Where(d => d.admUnit_ID = programPriorityObj.ad).ToList();
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        long listMaxProgramPriority = Convert.ToInt64(list.Select(d => d.Priority).Max());
                        if (listMaxProgramPriority > 0)
                        {
                            DAL.ProgramPriority programPriorityObjUpdate = list.Where(d => d.Priority == listMaxProgramPriority).FirstOrDefault();
                            if (programPriorityObjUpdate != null)
                            {
                                programPriorityObjUpdate.Priority = currentProgramPriority;
                                programPriorityObjUpdate.DateModified = DateTime.Now;
                                programPriorityObjUpdate.ModifiedBy = uId;
                                db.Update<DAL.ProgramPriority>(programPriorityObjUpdate);
                            }
                        }
                        currentProgramPriority = currentProgramPriority + 1;
                    }
                }
            }
        }

        protected void lvProgramPriority_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }

        protected void lvProgramPriority_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        { }

        string lastValue = "";
        protected string AddGroupingHeader()
        {


            //Get the data field value of interest for this row
            string currentValue = Eval("admUnit_Name").ToString();


            //Specify name to display if dataFieldValue is a database NULL
            if (currentValue.Length == 0)
            {
                currentValue = "";
            }

            string sNewRow = "";
            //See if there's been a change in value
            if (lastValue != currentValue)
            {
                //There's been a change! Record the change and emit the header
                lastValue = currentValue;
                sNewRow = "<tr style='background-color: gainsboro;'>" +
                            "<td colspan='2'><h4>" + currentValue + "</h4></td>" +
                            "<td colspan='1'></td>" +
                            "<td colspan='2' style='vertical-align: middle; font-weight: bold;'>Choice</td>" +
                            "<td colspan='1'></td>" +
                          "</tr>";
                return sNewRow;
            }
            else
            {
                return "";
            }
        }

        private void CheckAllEligiblePriority()
        {
            try
            {

                #region facultylogic

                long cId = -1;

                if (uId > 0)
                {
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cId = db.GetCandidateIdByUserID_ND(uId);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage_Masters.Text = "Error: Unable to get candidate.";
                        Panel_Master.CssClass = "alert alert-danger";
                        Panel_Master.Visible = true;
                        return;
                    }
                }

                if (cId > 0)
                {
                    List<DAL.CandidateFormSl> cFormList = null;

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                            if (AllStepInOneTime == 1)
                            {
                                bool Ispaid = false;
                                var CandidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                                if (CandidatePayment != null && CandidatePayment.IsPaid != null && Convert.ToBoolean(CandidatePayment.IsPaid))
                                    Ispaid = true;

                                cFormList = db.GetAllCandidateFormSlByCandIDIsPaid_AD(cId, Ispaid).ToList();
                            }
                            else
                                cFormList = db.GetAllCandidateFormSlByCandIDIsPaid_AD(cId, true).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage_Masters.Text = "Error: Unable to get candidate form serial.";
                        Panel_Master.CssClass = "alert alert-danger";
                        Panel_Master.Visible = true;
                        return;
                    }

                    List<DAL.AdmissionSetup> admSetupList = null;

                    if (cFormList != null)
                    {
                        if (cFormList.Count > 0)
                        {
                            admSetupList = cFormList.Select(c => c.AdmissionSetup).ToList();
                        }
                    }

                    List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();

                    if (admSetupList != null)
                    {
                        if (admSetupList.Count > 0)
                        {
                            foreach (var item in admSetupList)
                            {
                                using (var db = new CandidateDataManager())
                                {
                                    List<DAL.AdmissionUnit> _temp = new List<DAL.AdmissionUnit>();
                                    _temp = db.AdmissionDB.AdmissionUnits.Where(c => c.ID == item.AdmissionUnit.ID).ToList();
                                    if (_temp != null && _temp.Count > 0)
                                    {
                                        admUnitList.AddRange(_temp);
                                    }
                                }
                            }//end foreach
                        }
                    }//end admSetupList != null

                    List<DAL.AdmissionUnitProgram> list = new List<DAL.AdmissionUnitProgram>();
                    List<DAL.AdmissionUnitProgram> _temp2 = null;
                    List<DAL.AdmissionUnitProgram> NotEligibleProgramList = new List<DAL.AdmissionUnitProgram>();
                    int acaCalId = 0;
                    using (var db = new CandidateDataManager())
                    {
                        //cId = db.GetCandidateIdByUserID_ND(uId);
                        if (cId > 0)
                        {
                            DAL.CandidatePayment _tempCandidatePaymentObj = db.GetCandidatePaymentByCandidateID(cId);
                            if (_tempCandidatePaymentObj != null)
                            {
                                acaCalId = Convert.ToInt32(_tempCandidatePaymentObj.AcaCalID);
                            }
                        }
                    }


                    if (admUnitList != null)
                    {
                        if (admUnitList.Count() > 0)
                        {
                            int categoryId = -1;
                            using (var db = new CandidateDataManager())
                            {
                                List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                                    = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, false).ToList();

                                if (programsAppliedList != null && programsAppliedList.Count() > 0)
                                {
                                    var programObj = programsAppliedList.Where(x => x.AdmissionUnitID == admUnitList.FirstOrDefault().ID).FirstOrDefault();
                                    if (programObj != null)
                                    {
                                        categoryId = Convert.ToInt32(programObj.EducationCategoryID);
                                    }
                                }
                            }

                            foreach (var item in admUnitList)
                            {
                                using (var db = new OfficeDataManager())
                                {


                                    _temp2 = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == item.ID
                                                                                        && c.AcaCalID == acaCalId
                                                                                        && c.EducationCategoryID == categoryId
                                                                                        && c.IsActive == true).ToList();
                                }
                                if (_temp2 != null && _temp2.Count > 0)
                                {
                                    list.AddRange(_temp2);
                                }
                            }
                        }
                    }

                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.SPGetCandidateProgramPriorities_Result> list1 = null;

                        list1 = db.AdmissionDB.SPGetCandidateProgramPriorities(cId).ToList();

                        if (list1 != null && list1.Count > 0)
                        {
                            if (list.Count == list1.Count)
                            {
                                btnNext.Visible = true;
                            }
                            else
                            {
                                btnNext.Visible = false;
                            }
                        }
                        else
                            btnNext.Visible = false;
                    }
                }
                else
                {
                    //lblMessage_Masters.Text = "Error: Unable to get candidate (1).";
                    //Panel_Master.CssClass = "alert alert-danger";
                    //Panel_Master.Visible = true;
                    //return;
                }

                #endregion

            }
            catch (Exception ex)
            {

            }
        }



    }
}