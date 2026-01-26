using Admission.App_Start;
using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.HelpDesk
{
    public partial class HD_ApplicationPriority : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppAdditional.NavigateUrl = "HD_ApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "HD_ApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "HD_ApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "HD_ApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "HD_ApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                hrefAppPriority.NavigateUrl = "HD_ApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "HD_ApplicationRelation.aspx?val=" + queryVal;
            }

            bool isUndergradCandidate = true;

            if (!IsPostBack)
            {
                if (cId > 0)
                {


                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                                = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, true).ToList();

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
                if (isUndergradCandidate == true)
                {
                    Panel_GridView.Visible = true;
                    Panel_Master.Visible = false;
                    lblMessage_Masters.Text = string.Empty;
                    //LoadListView();
                    LoadDDL(cId);
                    LoadCandidateData(cId);
                }
                else
                {
                    Panel_GridView.Visible = false;
                    Panel_Master.Visible = true;
                    lblMessage_Masters.Text = "Program priority/choice is for only candidates applying for bachelors degree.<br/><br/>";
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

        private void LoadDDL(long cId)
        {
            if (cId > 0)
            {
                List<DAL.CandidateFormSl> cFormList = null;

                try
                {
                    using (var db = new CandidateDataManager())
                    {
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

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {
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

                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        lvProgramPriority.DataSource = list.OrderBy(c => c.cP_Priority).OrderBy(c => c.admUnit_Name).ToList();
                        lvProgramPriority.DataBind();
                    }
                    else
                    {
                        lvProgramPriority.DataSource = null;
                        lvProgramPriority.DataBind();
                    }
                }
                else
                {
                    lvProgramPriority.DataSource = null;
                    lvProgramPriority.DataBind();
                }
            }//end if (cId > 0)
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {


            //ddlProgram.Items.Clear();
            //long admUnitId = -1;
            //int acaCalId = 0;

            //admUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);

            //List<DAL.AdmissionUnitProgram> list = null;

            //using (var db = new CandidateDataManager())
            //{
            //    cId = Int64.Parse(Request.QueryString["val"].ToString()); //db.GetCandidateIdByUserID_ND(uId);
            //    if (cId > 0)
            //    {
            //        DAL.CandidatePayment _tempCandidatePaymentObj = db.GetCandidatePaymentByCandidateID(cId);
            //        if (_tempCandidatePaymentObj != null)
            //        {
            //            acaCalId = Convert.ToInt32(_tempCandidatePaymentObj.AcaCalID);
            //        }
            //    }
            //}

            //if (admUnitId > 0)
            //{
            //    try
            //    {
            //        using (var db = new OfficeDataManager())
            //        {
            //            list = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == acaCalId && c.EducationCategoryID == 4).ToList();
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        lblMessage_Masters.Text = "Error: Unable to get programs (1).";
            //        Panel_Master.CssClass = "alert alert-danger";
            //        Panel_Master.Visible = true;
            //        return;
            //    }

            //    if (list != null)
            //    {
            //        if (list.Count > 0)
            //        {
            //            DDLHelper.Bind<DAL.AdmissionUnitProgram>(ddlProgram, list.OrderBy(c => c.ProgramName).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Select);
            //        }
            //    }
            //    else
            //    {
            //        ddlProgram.Items.Clear();
            //        ddlProgram.Items.Add(new ListItem("No Programs!", "-1"));
            //    }
            //}
            //else
            //{
            //    ddlProgram.Items.Clear();
            //    ddlProgram.Items.Add(new ListItem("Error!", "-1"));
            //}



            #region Hidden By Ariq Because New code added above
            //ddlProgram.Items.Clear();
            //long admUnitId = -1;

            //admUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);

            //List<DAL.AdmissionUnitProgram> list = null;

            //if (admUnitId > 0)
            //{
            //    try
            //    {
            //        using (var db = new OfficeDataManager())
            //        {
            //            list = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == admUnitId && c.EducationCategoryID == 4).ToList();
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        lblMessage_Masters.Text = "Error: Unable to get programs (1).";
            //        Panel_Master.CssClass = "alert alert-danger";
            //        Panel_Master.Visible = true;
            //        return;
            //    }

            //    if (list != null)
            //    {
            //        if (list.Count > 0)
            //        {
            //            DDLHelper.Bind<DAL.AdmissionUnitProgram>(ddlProgram, list.OrderBy(c => c.ProgramName).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Select);
            //        }
            //    }
            //    else
            //    {
            //        ddlProgram.Items.Clear();
            //        ddlProgram.Items.Add(new ListItem("No Programs!", "-1"));
            //    }
            //}
            //else
            //{
            //    ddlProgram.Items.Clear();
            //    ddlProgram.Items.Add(new ListItem("Error!", "-1"));
            //}
            #endregion


            #region New Code BY Rafi. Becasue new Requirement


            ddlProgram.Items.Clear();
            long admUnitId = -1;
            int acaCalId = 0;

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
                        list = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == admUnitId
                                                                            && c.AcaCalID == acaCalId
                                                                            && c.EducationCategoryID == 4
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

                    #region New Logic Implement to filter Program

                    using (var db = new CandidateDataManager())
                    {
                        try
                        {
                            DAL.Exam secondaryExam = db.GetSecondaryExamByCandidateID_AD(cId);
                            DAL.Exam higherSecondaryExam = db.GetHigherSecdExamByCandidateID_AD(cId);
                            DAL.ExamDetail secExamDetail = db.GetExamDetailByID_ND(secondaryExam.ExamDetailsID);
                            DAL.ExamDetail higherSecExamDetail = db.GetExamDetailByID_ND(higherSecondaryExam.ExamDetailsID);

                            List<TelitalkEducationSubjectModel> hscsubjects = new List<TelitalkEducationSubjectModel>();
                            List<TelitalkEducationSubjectModel> sscsubjects = new List<TelitalkEducationSubjectModel>();

                            #region SSC and HSC Education Board Result


                            if (higherSecExamDetail != null && higherSecExamDetail.JsonDataObject != null)
                            {
                                try
                                {
                                    TelitalkEducationResultModelHSC hscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelHSC>(higherSecExamDetail.JsonDataObject);
                                    if (hscResultModel != null)
                                    {
                                        hscsubjects = hscResultModel.subject.ToList();
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            if (secExamDetail != null && secExamDetail.JsonDataObject != null)
                            {
                                try
                                {
                                    TelitalkEducationResultModelHSC sscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelHSC>(secExamDetail.JsonDataObject);
                                    if (sscResultModel != null)
                                    {
                                        sscsubjects = sscResultModel.subject.ToList();
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            #endregion

                            bool OLevel = false, ALevel = false;

                            if (secondaryExam != null && secondaryExam.ExamTypeID == 5)// O Level
                                OLevel = true;
                            if (higherSecondaryExam != null && higherSecondaryExam.ExamTypeID == 7)// A Level
                                ALevel = true;

                            TelitalkEducationSubjectModel hscHigherMath = null;
                            TelitalkEducationSubjectModel hscStat = null;
                            TelitalkEducationSubjectModel hscEco = null;
                            TelitalkEducationSubjectModel hscAcc = null;
                            TelitalkEducationSubjectModel hscEng = null;

                            TelitalkEducationSubjectModel hscMath = null;
                            TelitalkEducationSubjectModel hscBiology = null;



                            TelitalkEducationSubjectModel sscHigherMath = null;
                            TelitalkEducationSubjectModel sscEco = null;
                            TelitalkEducationSubjectModel sscAcc = null;
                            TelitalkEducationSubjectModel sscEng = null;

                            if (OLevel == false && ALevel == false)
                            {

                                if (hscsubjects != null && hscsubjects.Any())
                                {
                                    hscHigherMath = hscsubjects.Where(x => x.subCode == "265" || x.subCode == "228").FirstOrDefault();
                                    hscStat = hscsubjects.Where(x => x.subCode == "129").FirstOrDefault();
                                    hscEco = hscsubjects.Where(x => x.subCode == "109" || x.subCode == "213").FirstOrDefault();
                                    hscAcc = hscsubjects.Where(x => x.subCode == "253" || x.subCode == "21815" || x.subCode == "21816 ").FirstOrDefault();
                                    hscEng = hscsubjects.Where(x => x.subCode == "107" || x.subCode == "238" || x.subCode == "81112" || x.subCode == "81122" || x.subCode == "21812" || x.subCode == "21822 ").FirstOrDefault();

                                    hscMath = hscsubjects.Where(x => x.subCode == "265" || x.subCode == "127" || x.subCode == "228" || x.subCode == "81411 " || x.subCode == "81421").FirstOrDefault();
                                    hscBiology = hscsubjects.Where(x => x.subCode == "178" || x.subCode == "230").FirstOrDefault();


                                }

                                if (sscsubjects != null && sscsubjects.Any())
                                {
                                    sscHigherMath = sscsubjects.Where(x => x.subCode == "126" || x.subCode == "1321" || x.subCode == "1923" || x.subCode == "1311").FirstOrDefault();
                                    sscEco = sscsubjects.Where(x => x.subCode == "141" || x.subCode == "115").FirstOrDefault();
                                    sscAcc = sscsubjects.Where(x => x.subCode == "146" || x.subCode == "1322 " || x.subCode == "1312").FirstOrDefault();
                                    sscEng = sscsubjects.Where(x => x.subCode == "107" || x.subCode == "136" || x.subCode == "1922 " || x.subCode == "1912 ").FirstOrDefault();


                                }

                                if (admUnitId == 3) // FASS
                                {
                                    bool IsDMREligible = false, IsEcoEligible = false, IsEnglishEligible = false;

                                    #region DMR

                                    if (hscHigherMath != null && (hscHigherMath.grade == "A-" || hscHigherMath.grade == "A" || hscHigherMath.grade == "A+"))
                                        IsDMREligible = true;

                                    if (IsDMREligible == false && hscStat != null && (hscStat.grade == "A-" || hscStat.grade == "A" || hscStat.grade == "A+"))
                                        IsDMREligible = true;

                                    if (!IsDMREligible)
                                    {
                                        var DMRObj = list.Where(x => x.ProgramID == 50).FirstOrDefault();
                                        if (DMRObj != null)
                                        {
                                            list = list.Where(x => x.ProgramID != 50).ToList(); // DMR program exclude


                                            DMRObj.Attribute3 = "Candidates applying for admission in the Department of Disaster Management and Resilience must have studied Higher Mathematics/ Statistics in HSC / equivalent level and scored minimum 'A-'(A Minus) grade in the examination.";

                                            NotEligibleProgramList.Add(DMRObj);
                                        }
                                    }
                                    #endregion

                                    #region Economics

                                    if (hscHigherMath != null && (hscHigherMath.grade == "A-" || hscHigherMath.grade == "A" || hscHigherMath.grade == "A+"))
                                        IsEcoEligible = true;

                                    if (IsEcoEligible == false && hscStat != null && (hscStat.grade == "A-" || hscStat.grade == "A" || hscStat.grade == "A+"))
                                        IsEcoEligible = true;

                                    if (IsEcoEligible == false && hscEco != null && (hscEco.grade == "A-" || hscEco.grade == "A" || hscEco.grade == "A+"))
                                        IsEcoEligible = true;

                                    if (IsEcoEligible == false && hscAcc != null && (hscAcc.grade == "A-" || hscAcc.grade == "A" || hscAcc.grade == "A+"))
                                        IsEcoEligible = true;

                                    if (IsEcoEligible == false && sscHigherMath != null && (sscHigherMath.grade == "A-" || sscHigherMath.grade == "A" || sscHigherMath.grade == "A+"))
                                        IsEcoEligible = true;

                                    if (IsEcoEligible == false && sscEco != null && (sscEco.grade == "A-" || sscEco.grade == "A" || sscEco.grade == "A+"))
                                        IsEcoEligible = true;

                                    if (IsEcoEligible == false && sscAcc != null && (sscAcc.grade == "A-" || sscAcc.grade == "A" || sscAcc.grade == "A+"))
                                        IsEcoEligible = true;


                                    if (!IsEcoEligible)
                                    {
                                        var ECOObj = list.Where(x => x.ProgramID == 19).FirstOrDefault();
                                        if (ECOObj != null)
                                        {
                                            list = list.Where(x => x.ProgramID != 19).ToList(); // ECO program exclude

                                            ECOObj.Attribute3 = "Candidates applying for admission in the Department of Economics must have minimum 'A-'(A Minus) grade in Economics / Accounting / Statistics / Higher Mathematics either in SSC / equivalent or HSC/ equivalent examination.";

                                            NotEligibleProgramList.Add(ECOObj);
                                        }
                                    }

                                    #endregion

                                    #region English

                                    if (hscEng != null && (hscEng.grade == "A-" || hscEng.grade == "A" || hscEng.grade == "A+") && sscEng != null && (sscEng.grade == "A-" || sscEng.grade == "A" || sscEng.grade == "A+"))
                                        IsEnglishEligible = true;

                                    if (!IsEnglishEligible)
                                    {
                                        var ENGObj = list.Where(x => x.ProgramID == 8).FirstOrDefault();
                                        if (ENGObj != null)
                                        {
                                            list = list.Where(x => x.ProgramID != 8).ToList(); // English program exclude

                                            ENGObj.Attribute3 = "Candidates applying for admission in the Department of English must have minimum 'A-'(A Minus) grade in English both in SSC / equivalent and HSC / equivalent examinations.";

                                            NotEligibleProgramList.Add(ENGObj);
                                        }
                                    }

                                    #endregion

                                }
                                else if (admUnitId == 5) // FST
                                {
                                    bool IsICECSEEligible = false, IsESEligible = false;

                                    #region ICE AND CSE

                                    if (hscMath != null)
                                        IsICECSEEligible = true;


                                    if (!IsICECSEEligible)
                                    {
                                        var ICECSEList = list.Where(x => x.ProgramID == 18 || x.ProgramID == 48).ToList();
                                        if (ICECSEList != null && ICECSEList.Any())
                                        {
                                            list = list.Where(x => x.ProgramID != 18 && x.ProgramID != 48).ToList(); // ICE AND CSE program exclude

                                            ICECSEList.Where(w => w.AdmissionUnitID == admUnitId).ToList().ForEach(u =>
                                            {
                                                u.Attribute3 = "For ICE & CSE candidate must have Mathmatics subject in HSC";
                                            });

                                            NotEligibleProgramList.AddRange(ICECSEList);
                                        }
                                    }
                                    #endregion

                                    #region ES

                                    if (hscBiology != null)
                                        IsESEligible = true;

                                    if (!IsESEligible)
                                    {
                                        var ESObj = list.Where(x => x.ProgramID == 26).FirstOrDefault();
                                        if (ESObj != null)
                                        {
                                            list = list.Where(x => x.ProgramID != 26).ToList(); // ES program exclude

                                            ESObj.Attribute3 = "For ES candidate must have Biology subject in HSC";

                                            NotEligibleProgramList.Add(ESObj);
                                        }
                                    }

                                    #endregion

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }


                    }
                    #endregion

                    DDLHelper.Bind<DAL.AdmissionUnitProgram>(ddlProgram, list.OrderBy(c => c.ProgramName).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Select);


                    if (NotEligibleProgramList != null && NotEligibleProgramList.Any())
                    {
                        //divNotEligible.Visible = true;
                        //gvNotEligibleProgramList.DataSource = NotEligibleProgramList;
                        //gvNotEligibleProgramList.DataBind();
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


            #endregion


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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string logNewObject = string.Empty;
            int acaCalId = 0;
            long cId = -1;

            if (uId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        string queryVal = Request.QueryString["val"].ToString();
                        string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                        cId = Int64.Parse(decryptedQueryVal);
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

                #region Hidden Code
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

                DAL.CandidatePayment _tempCandidatePaymentObj = null;
                using (var db = new CandidateDataManager())
                {
                    _tempCandidatePaymentObj = db.GetCandidatePaymentByCandidateID(cId);
                }

                if (_tempCandidatePaymentObj != null)
                {
                    acaCalId = Convert.ToInt32(_tempCandidatePaymentObj.AcaCalID);
                }


                DAL.ProgramPriority objToCreate = new DAL.ProgramPriority();

                DAL.AdmissionSetup _tempAdm = null;
                try
                {
                    long _tempAdmUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                    using (var db = new OfficeDataManager())
                    {
                        _tempAdm = db.AdmissionDB.AdmissionSetups.Where(c => c.AdmissionUnitID == _tempAdmUnitId && c.AcaCalID == acaCalId).FirstOrDefault();
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

                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                            try
                            {
                                logNewObject = ObjectToString.ConvertToString(objToCreate);

                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                dLog.EventName = "btnSave_Click";
                                dLog.PageName = "HD_ApplicationPriority.aspx; " + _pageUrl;
                                dLog.NewData = logNewObject;
                                dLog.UserId = uId;
                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                LogWriter.DataLogWriter(dLog);
                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            lblMessage_Masters.Text = "Successful";
                            Panel_Master.CssClass = "alert alert-success";
                            Panel_Master.Visible = true;
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

                //btnSave.Text = "Add";

                //ddlFaculty.SelectedValue = "-1";
                ddlProgram.SelectedValue = "-1";
                //ddlChoice.SelectedValue = "-1";

                LoadCandidateData();

                CurrentProgramPriorityID = 0;



                #region Hidden by Ariq Rahman because new code added above
                //long cId = -1;
                //if ((string.IsNullOrEmpty(Request.QueryString["val"])))
                //{
                //    Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                //}
                //else
                //{
                //    cId = Int64.Parse(Request.QueryString["val"].ToString());
                //}

                //if (cId > 0)
                //{
                //    if (CurrentProgramPriorityID > 0) //update
                //    {
                //        DAL.ProgramPriority objToUpdate = null;

                //        long id = CurrentProgramPriorityID;

                //        try
                //        {
                //            using (var db = new CandidateDataManager())
                //            {
                //                objToUpdate = db.AdmissionDB.ProgramPriorities.Find(id);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage_Masters.Text = "Error: Unable to get candidate choice for update.";
                //            Panel_Master.CssClass = "alert alert-danger";
                //            Panel_Master.Visible = true;
                //            return;
                //        }

                //        if (objToUpdate != null)
                //        {
                //            DAL.AdmissionSetup _temp = null;
                //            try
                //            {
                //                long _tempAdmUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                //                using (var db = new OfficeDataManager())
                //                {
                //                    _temp = db.AdmissionDB.AdmissionSetups.Where(c => c.AdmissionUnitID == _tempAdmUnitId).FirstOrDefault();
                //                }
                //            }
                //            catch (Exception)
                //            {
                //                lblMessage_Masters.Text = "Error: Unable to get exam.";
                //                Panel_Master.CssClass = "alert alert-danger";
                //                Panel_Master.Visible = true;
                //                return;
                //            }

                //            DAL.AdmissionUnitProgram _tempProg = null;
                //            try
                //            {
                //                long _tempAdmUnitProgId = Convert.ToInt64(ddlProgram.SelectedValue);
                //                using (var db = new OfficeDataManager())
                //                {
                //                    _tempProg = db.AdmissionDB.AdmissionUnitPrograms.Find(_tempAdmUnitProgId);
                //                }
                //            }
                //            catch (Exception)
                //            {
                //                lblMessage_Masters.Text = "Error: Unable to get program.";
                //                Panel_Master.CssClass = "alert alert-danger";
                //                Panel_Master.Visible = true;
                //                return;
                //            }
                //            if (_temp != null && _tempProg != null)
                //            {
                //                objToUpdate.AdmissionUnitID = Convert.ToInt64(ddlFaculty.SelectedValue);
                //                objToUpdate.AdmissionUnitProgramID = Convert.ToInt64(ddlProgram.SelectedValue);
                //                objToUpdate.AdmissionSetupID = _temp.ID;
                //                objToUpdate.BatchID = _tempProg.BatchID;
                //                objToUpdate.AcaCalID = _tempProg.AcaCalID;
                //                objToUpdate.ProgramID = _tempProg.ProgramID;
                //                objToUpdate.ProgramName = _tempProg.ProgramName;
                //                objToUpdate.ShortName = _tempProg.ShortCode;
                //                objToUpdate.Priority = Convert.ToInt32(ddlChoice.SelectedValue);
                //                objToUpdate.DateModified = DateTime.Now;
                //                objToUpdate.ModifiedBy = cId;

                //                DAL.ProgramPriority _tempExistingObj = null;
                //                try
                //                {
                //                    using (var db = new CandidateDataManager())
                //                    {
                //                        _tempExistingObj = db.AdmissionDB.ProgramPriorities
                //                            .Where(c => c.CandidateID == cId &&
                //                                    c.AdmissionUnitID == objToUpdate.AdmissionUnitID &&
                //                                    c.AdmissionUnitProgramID == objToUpdate.AdmissionUnitProgramID &&
                //                                    c.AdmissionSetupID == objToUpdate.AdmissionSetupID &&
                //                                    c.Priority == objToUpdate.Priority).FirstOrDefault();
                //                    }
                //                }
                //                catch (Exception)
                //                {
                //                    lblMessage_Masters.Text = "Error: Unable to get existing choice.";
                //                    Panel_Master.CssClass = "alert alert-danger";
                //                    Panel_Master.Visible = true;
                //                    return;
                //                }

                //                if (_tempExistingObj == null) //update if not found
                //                {
                //                    try
                //                    {
                //                        using (var dbUpdate = new CandidateDataManager())
                //                        {
                //                            dbUpdate.Update<DAL.ProgramPriority>(objToUpdate);
                //                        }

                //                        lblMessage_Masters.Text = "Update Successful";
                //                        Panel_Master.CssClass = "alert alert-success";
                //                        Panel_Master.Visible = true;
                //                    }
                //                    catch (Exception)
                //                    {
                //                        lblMessage_Masters.Text = "Error: Unable to update choice.";
                //                        Panel_Master.CssClass = "alert alert-danger";
                //                        Panel_Master.Visible = true;
                //                        return;
                //                    }
                //                }
                //                else
                //                {
                //                    lblMessage_Masters.Text = "Another choice exist with same Faculty, Program and Choice. Please select a different choice.";
                //                    Panel_Master.CssClass = "alert alert-warning";
                //                    Panel_Master.Visible = true;
                //                    return;
                //                } //end if-else _tempExistingObj == null
                //            }
                //            else
                //            {
                //                lblMessage_Masters.Text = "Error: Unable to get load exam and program.";
                //                Panel_Master.CssClass = "alert alert-danger";
                //                Panel_Master.Visible = true;
                //                return;
                //            } //end if-else _temp != null && _tempProg != null
                //        }
                //    }
                //    else //create new
                //    {
                //        DAL.ProgramPriority objToCreate = new DAL.ProgramPriority();

                //        DAL.AdmissionSetup _tempAdm = null;
                //        try
                //        {
                //            long _tempAdmUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                //            using (var db = new OfficeDataManager())
                //            {
                //                _tempAdm = db.AdmissionDB.AdmissionSetups.Where(c => c.AdmissionUnitID == _tempAdmUnitId).FirstOrDefault();
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage_Masters.Text = "Error: Unable to get exam. 1";
                //            Panel_Master.CssClass = "alert alert-danger";
                //            Panel_Master.Visible = true;
                //            return;
                //        }

                //        DAL.AdmissionUnitProgram _tempAdmProg = null;
                //        try
                //        {
                //            long _tempAdmUnitProgId = Convert.ToInt64(ddlProgram.SelectedValue);
                //            using (var db = new OfficeDataManager())
                //            {
                //                _tempAdmProg = db.AdmissionDB.AdmissionUnitPrograms.Find(_tempAdmUnitProgId);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            lblMessage_Masters.Text = "Error: Unable to get program. 1";
                //            Panel_Master.CssClass = "alert alert-danger";
                //            Panel_Master.Visible = true;
                //            return;
                //        }
                //        if (_tempAdm != null && _tempAdmProg != null)
                //        {
                //            objToCreate.AdmissionUnitID = Convert.ToInt64(ddlFaculty.SelectedValue);
                //            objToCreate.AdmissionUnitProgramID = Convert.ToInt64(ddlProgram.SelectedValue);
                //            objToCreate.AdmissionSetupID = _tempAdm.ID;
                //            objToCreate.BatchID = _tempAdmProg.BatchID;
                //            objToCreate.AcaCalID = _tempAdmProg.AcaCalID;
                //            objToCreate.ProgramID = _tempAdmProg.ProgramID;
                //            objToCreate.ProgramName = _tempAdmProg.ProgramName;
                //            objToCreate.ShortName = _tempAdmProg.ShortCode;
                //            objToCreate.Priority = Convert.ToInt32(ddlChoice.SelectedValue);
                //            objToCreate.DateModified = null;
                //            objToCreate.ModifiedBy = null;

                //            objToCreate.CandidateID = cId;
                //            objToCreate.DateCreated = DateTime.Now;
                //            objToCreate.CreatedBy = cId;

                //            DAL.ProgramPriority _tempExistingObj = null;
                //            try
                //            {
                //                using (var db = new CandidateDataManager())
                //                {
                //                    _tempExistingObj = db.AdmissionDB.ProgramPriorities
                //                        .Where(c => c.CandidateID == cId &&
                //                                c.AdmissionUnitID == objToCreate.AdmissionUnitID &&
                //                                c.AdmissionUnitProgramID == objToCreate.AdmissionUnitProgramID &&
                //                                c.AdmissionSetupID == objToCreate.AdmissionSetupID).FirstOrDefault();
                //                }
                //            }
                //            catch (Exception)
                //            {
                //                lblMessage_Masters.Text = "Error: Unable to get existing choice.";
                //                Panel_Master.CssClass = "alert alert-danger";
                //                Panel_Master.Visible = true;
                //                return;
                //            }

                //            if (_tempExistingObj == null) //insert if not found
                //            {
                //                try
                //                {
                //                    using (var dbInsert = new CandidateDataManager())
                //                    {
                //                        dbInsert.Insert<DAL.ProgramPriority>(objToCreate);
                //                    }

                //                    lblMessage_Masters.Text = "Successful";
                //                    Panel_Master.CssClass = "alert alert-success";
                //                    Panel_Master.Visible = true;
                //                }
                //                catch (Exception)
                //                {
                //                    lblMessage_Masters.Text = "Error: Unable to save choice.";
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
                //            lblMessage_Masters.Text = "Error: Unable to get load exam and program. 1";
                //            Panel_Master.CssClass = "alert alert-danger";
                //            Panel_Master.Visible = true;
                //            return;
                //        } //end if-else _tempAdm != null && _tempAdmProg != null
                //    } //end if-else CurrentProgramPriorityID > 0

                //    btnSave.Text = "Add";

                //    ddlFaculty.SelectedValue = "-1";
                //    ddlProgram.SelectedValue = "-1";
                //    ddlChoice.SelectedValue = "-1";

                //    LoadCandidateData(cId);
                //    CurrentProgramPriorityID = 0;
                #endregion


            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            long cId = -1;

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);
            }

            bool isUndergradCandidate = true;

            if (cId > 0)
            {

                using (var db = new CandidateDataManager())
                {
                    List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList
                            = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, true).ToList();

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
                    List<DAL.ProgramPriority> list = null;

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            list = db.AdmissionDB.ProgramPriorities.Where(c => c.CandidateID == cId).ToList();
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage_Masters.Text = "Error: Unable to get candidate choice list from database.";
                        Panel_Master.CssClass = "alert alert-danger";
                        Panel_Master.Visible = true;
                        return;
                    }

                    if (list == null)//if no choices found (choice 1 and 2)
                    {
                        lblMessage_Masters.Text = "Choices not found. Please save at least two choices (choice 1 & choice 2) to proceed.";
                        Panel_Master.CssClass = "alert alert-danger";
                        Panel_Master.Visible = true;
                        return;
                    }

                    else
                    {
                        DAL.ProgramPriority choice1 = null;
                        DAL.ProgramPriority choice2 = null;

                        choice1 = list.Where(c => c.Priority == 1).FirstOrDefault();
                        choice2 = list.Where(c => c.Priority == 2).FirstOrDefault();

                        if (choice1 == null && choice2 != null)
                        {
                            lblMessage_Masters.Text = "Choice 1 not found. Please save a program as your choice 1 (first choice) to proceed.";
                            Panel_Master.CssClass = "alert alert-danger";
                            Panel_Master.Visible = true;
                            return;
                        }
                        else if (choice1 != null && choice2 == null)
                        {
                            lblMessage_Masters.Text = "Choice 2 not found. Please save a program as your choice 2 (second choice) to proceed.";
                            Panel_Master.CssClass = "alert alert-danger";
                            Panel_Master.Visible = true;
                            return;
                        }
                        else if (choice1 == null && choice2 == null)
                        {
                            lblMessage_Masters.Text = "Choices not found. Please save at least two choices (choice 1 & choice 2) to proceed.";
                            Panel_Master.CssClass = "alert alert-danger";
                            Panel_Master.Visible = true;
                            return;
                        }
                        else if (choice1 != null && choice2 != null)
                        {
                            Response.Redirect("ApplicationEducation.aspx", false);
                        }
                    } //end if-else list == null
                }
                else //else if masters then proceed without checking.
                {
                    Response.Redirect("ApplicationEducation.aspx", false);
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

                //LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                //LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                LinkButton lnkRemove = (LinkButton)currentItem.FindControl("lnkRemove");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                //lblUnitName.Text = progP.admUnit_Name;
                lblProgramName.Text = progP.admUnitProgZ_ProgramName;
                lblChoice.Text = progP.cP_Priority.ToString();


                lnkRemove.CommandName = "Remove";
                lnkRemove.CommandArgument = progP.cP_ID.ToString();


                //lnkEdit.CommandName = "Update";
                //lnkEdit.CommandArgument = progP.cP_ID.ToString();

                //lnkDelete.CommandName = "Delete";
                //lnkDelete.CommandArgument = progP.cP_ID.ToString();
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



            #region Hidden by Ariq Rahman Because new code added above
            //if (e.CommandName == "Update")
            //{
            //    long cId = -1;
            //    if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            //    {
            //        Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            //    }
            //    else
            //    {
            //        cId = Int64.Parse(Request.QueryString["val"].ToString());
            //    }

            //    long id = Convert.ToInt64(e.CommandArgument);
            //    btnSave.Text = "Update";
            //    using (var db = new GeneralDataManager())
            //    {
            //        var objectToUpdate = db.AdmissionDB.ProgramPriorities.Find(id);
            //        LoadDDL(cId);
            //        ddlFaculty.SelectedValue = objectToUpdate.AdmissionUnitID.ToString();

            //        ddlFaculty_SelectedIndexChanged(sender, e);
            //        ddlProgram.SelectedValue = objectToUpdate.AdmissionUnitProgramID.ToString();

            //        ddlProgram_SelectedIndexChanged(sender, e);
            //        ddlChoice.SelectedValue = objectToUpdate.Priority.ToString();
            //        CurrentProgramPriorityID = objectToUpdate.ID;
            //    }
            //}
            //else if(e.CommandName == "Delete")
            //{
            //    try
            //    {
            //        using (var db = new GeneralDataManager())
            //        {
            //            var objectToDelete = db.AdmissionDB.ProgramPriorities.Find(Convert.ToInt64(e.CommandArgument));
            //            db.Delete<DAL.ProgramPriority>(objectToDelete);
            //            CurrentProgramPriorityID = 0;
            //        }
            //        lblMessage_Masters.Text = "Successful.";
            //        Panel_Master.CssClass = "alert alert-success";
            //        Panel_Master.Visible = true;

            //    }
            //    catch (Exception ex)
            //    {
            //        lblMessage_Masters.Text = "Unable to delete. " + ex.Message +"; " + ex.InnerException.Message;
            //        Panel_Master.CssClass = "alert alert-danger";
            //        Panel_Master.Visible = true;
            //    }

            //    if (!string.IsNullOrEmpty(Request.QueryString["val"]))
            //    {
            //        cId = Int64.Parse(Request.QueryString["val"].ToString());
            //        if (cId > 0)
            //        {
            //            LoadCandidateData(cId);
            //        }
            //    }

            //}
            #endregion


        }
        

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
                sNewRow = "<tr style='background-color: gainsboro;'><td colspan='2'><h4>" + currentValue +
                             "</h4></td><td colspan='2' style='vertical-align: middle; font-weight: bold;'>Choice</td></tr>";
                return sNewRow;
            }
            else
            {
                return "";
            }
        }




        private void LoadCandidateData()
        {
            lvProgramPriority.DataSource = null;
            lvProgramPriority.DataBind();
            btnNext.Visible = false;

            long cId = -1;

            if (uId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        string queryVal = Request.QueryString["val"].ToString();
                        string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                        cId = Int64.Parse(decryptedQueryVal);
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

                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        lvProgramPriority.DataSource = list.OrderBy(c => c.cP_Priority).OrderBy(c => c.admUnit_Name).ToList();
                        lvProgramPriority.DataBind();
                        btnNext.Visible = true;
                    }
                    else
                    {
                        lvProgramPriority.DataSource = null;
                        lvProgramPriority.DataBind();
                        btnNext.Visible = false;
                    }
                }
                else
                {
                    lvProgramPriority.DataSource = null;
                    lvProgramPriority.DataBind();
                    btnNext.Visible = false;
                }
            }
        }


        private void AdjustProgramPriority(long candidateId, int acacalId, long admissionUnitId, int priority)
        {


            int currentProgramPriority = Convert.ToInt32(priority);
            using (var db = new GeneralDataManager())
            {
                List<DAL.ProgramPriority> list = db.AdmissionDB.ProgramPriorities.Where(d => d.CandidateID == candidateId && d.AdmissionUnitID == admissionUnitId && d.AcaCalID == acacalId && d.Priority > priority).OrderBy(d => d.Priority).ToList(); //    
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






        #region N/A -- Old Code
        //private void LoadCandidateData(long cId)
        //{
        //    if (cId > 0)
        //    {
        //        List<DAL.SPGetCandidateProgramPriorities_Result> list = null;

        //        try
        //        {
        //            using (var db = new CandidateDataManager())
        //            {
        //                list = db.AdmissionDB.SPGetCandidateProgramPriorities(cId).ToList();
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            lblMessage_Masters.Text = "Error: Unable to get candidate choices.";
        //            Panel_Master.CssClass = "alert alert-danger";
        //            Panel_Master.Visible = true;
        //            return;
        //        }

        //        if (list != null)
        //        {
        //            if (list.Count > 0)
        //            {
        //                lvProgramPriority.DataSource = list.OrderBy(c => c.cP_Priority).OrderBy(c => c.admUnit_Name).ToList();
        //                lvProgramPriority.DataBind();
        //            }
        //            else
        //            {
        //                lvProgramPriority.DataSource = null;
        //                lvProgramPriority.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            lvProgramPriority.DataSource = null;
        //            lvProgramPriority.DataBind();
        //        }
        //    }//end if (cId > 0)
        //}

        //protected void lvProgramPriority_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListViewItemType.DataItem)
        //    {
        //        ListViewDataItem currentItem = (ListViewDataItem)e.Item;
        //        DAL.SPGetCandidateProgramPriorities_Result progP = (DAL.SPGetCandidateProgramPriorities_Result)((ListViewDataItem)(e.Item)).DataItem;

        //        Label lblSerial = (Label)currentItem.FindControl("lblSerial");
        //        Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
        //        Label lblProgramName = (Label)currentItem.FindControl("lblProgramName");
        //        Label lblChoice = (Label)currentItem.FindControl("lblChoice");

        //        LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");

        //        lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
        //        //lblUnitName.Text = progP.admUnit_Name;
        //        lblProgramName.Text = progP.admUnitProgZ_ProgramName;
        //        lblChoice.Text = progP.cP_Priority.ToString();

        //        lnkEdit.CommandName = "Update";
        //        lnkEdit.CommandArgument = progP.cP_ID.ToString();
        //    }
        //}

        //protected void lvProgramPriority_ItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Update")
        //    {
        //        //long id = Convert.ToInt64(e.CommandArgument);
        //        //btnSave.Text = "Update";
        //        //using (var db = new GeneralDataManager())
        //        //{
        //        //    var objectToUpdate = db.AdmissionDB.ProgramPriorities.Find(id);
        //        //    LoadDDL();
        //        //    ddlFaculty.SelectedValue = objectToUpdate.AdmissionUnitID.ToString();

        //        //    ddlFaculty_SelectedIndexChanged(sender, e);
        //        //    ddlProgram.SelectedValue = objectToUpdate.AdmissionUnitProgramID.ToString();

        //        //    ddlProgram_SelectedIndexChanged(sender, e);
        //        //    ddlChoice.SelectedValue = objectToUpdate.Priority.ToString();
        //        //    CurrentProgramPriorityID = objectToUpdate.ID;
        //        //}
        //    }
        //}

        //protected void lvProgramPriority_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        //{

        //}

        //protected void lvProgramPriority_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        //{

        //}

        //string lastValue = "";
        //protected string AddGroupingHeader()
        //{

        //    //Get the data field value of interest for this row
        //    string currentValue = Eval("admUnit_Name").ToString();


        //    //Specify name to display if dataFieldValue is a database NULL
        //    if (currentValue.Length == 0)
        //    {
        //        currentValue = "";
        //    }

        //    string sNewRow = "";
        //    //See if there's been a change in value
        //    if (lastValue != currentValue)
        //    {
        //        //There's been a change! Record the change and emit the header
        //        lastValue = currentValue;
        //        sNewRow = "<tr style='background-color: gainsboro;'><td colspan='2'><h4>" + currentValue +
        //                     "</h4></td><td colspan='2' style='vertical-align: middle; font-weight: bold;'>Choice</td></tr>";
        //        return sNewRow;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //} 
        #endregion
    }
}