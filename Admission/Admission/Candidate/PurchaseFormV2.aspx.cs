using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class PurchaseFormV2 : System.Web.UI.Page
    {


        string SessionLoginCaptcha = "SessionLoginCaptcha";
        int oLevelExamType = -1;
        int aLevelExamType = -1;

        protected void Page_Load(object sender, EventArgs e)
        {

            //btnSubmit.Visible = false;

            if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                using (var db = new OfficeDataManager())
                {
                    long admissionSetupId = Convert.ToInt64(Request.QueryString["asi"]);
                    DAL.AdmissionSetup admissionSetup = db.AdmissionDB.AdmissionSetups.Find(admissionSetupId);
                    if (admissionSetup.StartDate >= DateTime.Now && admissionSetup.EndDate <= DateTime.Now)
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                    }

                    if (admissionSetup.EducationCategoryID == 4)
                    {
                        if (Session["CandidateFormSerial_Session"] == null ||
                            Session["CandidatePayment_Session"] == null ||
                            Session["AdmUnitObj_Session"] == null)
                        {
                            Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request. You can not access this page right now. Please go to Home page and do the process again.&type=purchaseForm", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                    }

                }
            }

            if (!IsPostBack)
            {
                Session["Eligible_NotEligible_List"] = null;

                string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];
                if (!string.IsNullOrEmpty(isLiveServerString) && isLiveServerString == "0")
                {
                    btnAdminAssignTestValue.Visible = true;
                }
                else
                {
                    btnAdminAssignTestValue.Visible = false;
                }



                    LoadCaptcha();
                LoadDDL();

                txtName.Text = String.Empty;
                //txtDateOfBirth.Text = String.Empty;
                txtEmail.Text = String.Empty;
                txtSmsMobile.Text = String.Empty;
                txtGuardianMobile.Text = String.Empty;
                ddlGender.SelectedIndex = -1;
                //ddlQuota.SelectedIndex = -1;
                //ddlPassingYear.SelectedIndex = -1;
                ddlExamTypeSSC.SelectedIndex = -1;
                ddlGroupSSC.SelectedIndex = -1;
                txtGPASSC.Text = String.Empty;
                ddlPassYearSSC.SelectedIndex = -1;
                ddlExamTypeHSC.SelectedIndex = -1;
                ddlGroupHSC.SelectedIndex = -1;
                txtGPAHSC.Text = String.Empty;
                ddlPassYearHSC.SelectedIndex = -1;

                ddlDay.SelectedIndex = -1;
                ddlMonth.SelectedIndex = -1;
                ddlYear.SelectedIndex = -1;
            }

        }

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessageTop.Text = string.Empty;
                lblMessageTop.Text = msg.ToString();
                lblMessageTop.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessageTop.Attributes.CssStyle.Add("color", "green");

                messagePanelTop.Visible = true;
                messagePanelTop.CssClass = "alert alert-success";

                //====================================================
                //====================================================

                //lblMessageBottom.Text = string.Empty;
                //lblMessageBottom.Text = msg.ToString();
                //lblMessageBottom.Attributes.CssStyle.Add("font-weight", "bold");
                //lblMessageBottom.Attributes.CssStyle.Add("color", "green");

                //messagePanelBottom.Visible = true;
                //messagePanelBottom.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessageTop.Text = string.Empty;
                lblMessageTop.Text = msg.ToString();
                lblMessageTop.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessageTop.Attributes.CssStyle.Add("color", "crimson");

                messagePanelTop.Visible = true;
                messagePanelTop.CssClass = "alert alert-danger";

                //====================================================
                //====================================================

                lblMessageBottom.Text = string.Empty;
                lblMessageBottom.Text = msg.ToString();
                lblMessageBottom.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessageBottom.Attributes.CssStyle.Add("color", "crimson");

                messagePanelBottom.Visible = true;
                messagePanelBottom.CssClass = "alert alert-danger";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);

            }
            else if (status == "clear")
            {
                lblMessageTop.Text = string.Empty;
                messagePanelTop.Visible = false;

                //====================================================
                //====================================================

                lblMessageBottom.Text = string.Empty;
                messagePanelBottom.Visible = false;

            }

        }

        private void LoadCaptcha()
        {
            Captcha captchaObj = new Captcha();

            SessionSGD.SaveObjToSession<string>(Captcha.GetStringForCaptcha(), SessionLoginCaptcha);
            //Bitmap bm = captchaObj.MakeCaptchaImage(Captcha.GetStringForCaptcha(), 160, 80, "Arial");
            Bitmap bm = captchaObj.MakeCaptchaImage(SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha), 160, 80, "Arial");

            MemoryStream ms = new MemoryStream();
            bm.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            imgCtrl.Src = "data:image/gif;base64," + base64Data;

            txtCaptcha.Text = "";
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                //List<DAL.GroupOrSubject> groupSubjectList = db.AdmissionDB.GroupOrSubjects.Where(a => a.IsActive == true).ToList();
                //List<DAL.EducationBoard> educationBoardList = db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true).ToList();
                //List<DAL.ResultDivision> resultDivisionList = db.AdmissionDB.ResultDivisions.Where(a => a.IsActive == true).ToList();

                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);

                DDLHelper.Bind<DAL.EducationBoard>(ddlBoardSSC, db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true && a.IsVisual == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                DDLHelper.Bind<DAL.EducationBoard>(ddlBoardHSC, db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true && a.IsVisual == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);

                //DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(x => x.CreatedBy).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);

                List<DAL.SSCHSCPassingYearSetup> sscHscPassingYearList = db.AdmissionDB.SSCHSCPassingYearSetups.Where(x => x.IsActive == true).ToList();
                if (sscHscPassingYearList != null && sscHscPassingYearList.Count > 0)
                {
                    int sscStartYear = (int)sscHscPassingYearList.Where(x => x.ExamTypeId == 1).Select(x => x.StartYear).FirstOrDefault();
                    int sscEndYear = (int)sscHscPassingYearList.Where(x => x.ExamTypeId == 1).Select(x => x.EndYear).FirstOrDefault();

                    ddlPassYearSSC.Items.Clear();
                    ddlPassYearSSC.Items.Add(new ListItem("Select", "-1"));
                    ddlPassYearSSC.AppendDataBoundItems = true;
                    for (int i = sscEndYear; i >= sscStartYear; i--)
                    {
                        ddlPassYearSSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }


                    int hscStartYear = (int)sscHscPassingYearList.Where(x => x.ExamTypeId == 2).Select(x => x.StartYear).FirstOrDefault();
                    int hscEndYear = (int)sscHscPassingYearList.Where(x => x.ExamTypeId == 2).Select(x => x.EndYear).FirstOrDefault();

                    ddlPassYearHSC.Items.Clear();
                    ddlPassYearHSC.Items.Add(new ListItem("Select", "-1"));
                    ddlPassYearHSC.AppendDataBoundItems = true;
                    for (int i = hscEndYear; i >= hscStartYear; i--)
                    {
                        ddlPassYearHSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                }

            }

            DAL.AdmissionSetup admissionSetup = null;
            DAL.AdmissionUnit admissionUnit = null;

            long admissionSetupIDLong = -1;
            long admissionUnitIDLong = -1;

            if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                admissionSetupIDLong = Convert.ToInt64(Request.QueryString["asi"]);
                admissionUnitIDLong = Convert.ToInt64(Request.QueryString["aui"]);
            }
            try
            {
                using (var db = new OfficeDataManager())
                {
                    admissionSetup = db.AdmissionDB.AdmissionSetups.Find(admissionSetupIDLong);
                    admissionUnit = db.AdmissionDB.AdmissionUnits.Find(admissionUnitIDLong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }//end inner try-catch 1

            //if (admissionSetup != null)
            //{
            //    if (admissionSetup.EducationCategoryID == 4) //for bachelors candidate.
            //    {
            //        //hidePassingYear.Attributes.Add("style", "display:none");

            //        hidePassingYear.Visible = false;

            //        LoadPassingYearDDLForBachelors();
            //    }
            //    else if (admissionSetup.EducationCategoryID == 6) //for masters candidate.
            //    {
            //        //hideCalculater.Attributes.Add("style", "display:none");
            //        //hideSSC.Attributes.Add("style", "display:none");
            //        //hideHSC.Attributes.Add("style", "display:none");


            //        //hideCalculater.Visible = false;
            //        hideSSC.Visible = false;
            //        hideHSC.Visible = false;

            //        LoadPassingYearDDLForMasters();
            //    }
            //}

            #region Load Day for BirthDate
            ddlDay.Items.Clear();
            ddlDay.Items.Add(new ListItem("--Day--", "-1"));
            ddlDay.AppendDataBoundItems = true;

            for (int i = 1; i <= 31; i++)
            {
                string d = string.Empty;
                if (i < 10)
                {
                    d = "0" + i.ToString();
                }
                else
                {
                    d = i.ToString();
                }

                ddlDay.Items.Add(new ListItem(d.ToString(), d.ToString()));
            }

            #endregion


            #region Load Month for BirthDate
            ddlMonth.Items.Clear();
            ddlMonth.Items.Add(new ListItem("--Month--", "-1"));
            ddlMonth.AppendDataBoundItems = true;
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < months.Length - 1; i++)
            {
                int j = i + 1;

                string m = string.Empty;
                if (j < 10)
                {
                    m = "0" + j.ToString();
                }
                else
                {
                    m = j.ToString();
                }

                ddlMonth.Items.Add(new ListItem(months[i], m.ToString()));
            }
            #endregion


            #region Load Year for BirthDate
            ddlYear.Items.Clear();
            ddlYear.Items.Add(new ListItem("--Year--", "-1"));
            ddlYear.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            #endregion

        }

        #region N/A
        //private void LoadPassingYearDDLForBachelors()
        //{
        //    ddlPassingYear.Items.Clear();
        //    ddlPassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlPassingYear.AppendDataBoundItems = true;
        //    for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 2; i--)
        //    {
        //        ddlPassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }
        //}

        //private void LoadPassingYearDDLForMasters()
        //{
        //    ddlPassingYear.Items.Clear();
        //    ddlPassingYear.Items.Add(new ListItem("Select", "-1"));
        //    ddlPassingYear.AppendDataBoundItems = true;
        //    for (int i = DateTime.Now.Year; i > 1950; i--)
        //    {
        //        ddlPassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }
        //}

        //private void EligibleMessage(string msg, string css)
        //{
        //    massageHiddenTopId.Visible = true;
        //    massageHiddenBottomId.Visible = true;
        //    lblEligibleMsg.Text = msg;
        //    lblEligibleMsgBottom.Text = msg;
        //    lblEligibleMsg.CssClass = css;
        //    lblEligibleMsgBottom.CssClass = css;
        //    lblEligibleMsg.Focus();
        //    lblEligibleMsgBottom.Focus();
        //} 
        #endregion

        protected string DateFormateing()
        {
            string ddlDayId = ddlDay.SelectedValue;
            string ddlMonthId = ddlMonth.SelectedValue;
            string ddlYearId = ddlYear.SelectedValue;

            //if (Convert.ToInt32(ddlDayId) < 10)
            //{
            //    ddlDayId = "0" + ddlDayId;
            //}

            //if (Convert.ToInt32(ddlMonthId) < 10)
            //{
            //    ddlMonthId = "0" + ddlMonthId;
            //}


            string result = ddlDayId.ToString() + "/" + ddlMonthId.ToString() + "/" + ddlYearId.ToString();

            return result;

        }

        private void PurchaseApplicationForm()
        {

            //DAL.BasicInfo candExist = null;

            string stringDateOfBirth = DateFormateing();
            DateTime dob = DateTime.ParseExact(stringDateOfBirth.ToString(), "dd/MM/yyyy", null);
            int genderId = Int32.Parse(ddlGender.SelectedValue);
            //int quotaId = -1; // Int32.Parse(ddlQuota.SelectedValue);

            string smsMobile = txtCountryCodeSMSMobile.Text + txtSmsMobile.Text.Trim().ToString();
            string guardianMobile = txtCountryCodeGuardianMobile.Text + txtGuardianMobile.Text.Trim().ToString();

            #region N/A
            //using (var db = new CandidateDataManager())
            //{
            //    candExist = db.AdmissionDB.BasicInfoes
            //        .Where(c =>
            //            c.DateOfBirth == dob &&
            //            c.SMSPhone == smsMobile.ToString()
            //            )
            //        .FirstOrDefault();
            //}

            //if (candExist != null) //if candidate exist, do not proceed.
            //{
            //    if (candExist.DateOfBirth.Equals(dob) && candExist.SMSPhone.Equals(txtSmsMobile.Text.Trim()))
            //    {
            //        string message = "Candidate already exist. If you like to purchase additional application, please login and click 'Purchase More Application'." +
            //            "If you have already applied, attempted payment and like to complete your payment procedure, then go to <a href='http://admission.bup.edu.bd/Admission/VerifyPayment'>Complete Payment Process</a> from homepage. ";
            //       EligibleMessage(message, "text-danger");
            //    }
            //}
            //else //proceed with purchase.
            //{ 
            #endregion

            try
            {
                //---------------------------------------------------------------------------------
                DAL.AdmissionSetup admissionSetup = null;
                DAL.AdmissionUnit admissionUnit = null;

                long admissionSetupIDLong = -1;
                long admissionUnitIDLong = -1;

                if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                }
                else
                {
                    admissionSetupIDLong = Convert.ToInt64(Request.QueryString["asi"]);
                    admissionUnitIDLong = Convert.ToInt64(Request.QueryString["aui"]);
                }
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admissionSetup = db.AdmissionDB.AdmissionSetups.Find(admissionSetupIDLong);
                        admissionUnit = db.AdmissionDB.AdmissionUnits.Find(admissionUnitIDLong);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }//end inner try-catch 1

                #region N/A
                //if (admissionSetup != null)
                //{
                //    if (admissionSetup.EducationCategoryID == 4) //for bachelors candidate.
                //    {
                //        LoadPassingYearDDLForBachelors();
                //    }
                //    else if (admissionSetup.EducationCategoryID == 6) //for masters candidate.
                //    {
                //        LoadPassingYearDDLForMasters();
                //    }
                //} 
                #endregion

                #region CandidateUser/BasicInfo

                //---------------------------------------------------------------------------------
                //insert candidate user
                //---------------------------------------------------------------------------------
                DAL.CandidateUser candidateUser = new DAL.CandidateUser();
                
                candidateUser.UsernameLoginId = CommonLogic.AdmissionLoginId();
                candidateUser.Password = CommonLogic.AdmissionPassword();
                candidateUser.IsConfirmed = false;
                candidateUser.IsLocked = true;
                candidateUser.ValidTill = DateTime.Now.AddMonths(4);
                candidateUser.IsSentSms = false;
                candidateUser.IsSentEmail = false;
                candidateUser.RoleID = 2;
                candidateUser.IsActive = true;
                candidateUser.CreatedBy = -99;
                candidateUser.DateCreated = DateTime.Now;

                long candidateUserIdLong = -1;
                using (var db = new CandidateDataManager())
                {
                    db.Insert<DAL.CandidateUser>(candidateUser);
                    candidateUserIdLong = candidateUser.ID;
                }

                //---------------------------------------------------------------------------------
                //insert candidate basic info
                //---------------------------------------------------------------------------------
                DAL.BasicInfo candidate = new DAL.BasicInfo();
                candidate.FirstName = txtName.Text.ToUpper();
                //candidate.MiddleName = ;
                //candidate.LastName = ;
                candidate.Mobile = smsMobile.Trim().ToString(); //txtSmsMobile.Text.Trim();
                candidate.SMSPhone = smsMobile.Trim().ToString(); //txtSmsMobile.Text.Trim();
                candidate.Email = txtEmail.Text.Trim();
                candidate.DateOfBirth = DateTime.ParseExact(stringDateOfBirth.ToString(), "dd/MM/yyyy", null);
                candidate.CandidateUserID = candidateUserIdLong;
                candidate.IsActive = false;
                candidate.UniqueIdentifier = Guid.NewGuid();
                candidate.GenderID = Convert.ToInt32(ddlGender.SelectedValue);
                //candidate.QuotaID = quotaId; //Convert.ToInt32(ddlQuota.SelectedValue);
                candidate.CreatedBy = -99;
                candidate.DateCreated = DateTime.Now;
                candidate.GuardianPhone = guardianMobile.Trim().ToString(); //txtGuardianMobile.Text.Trim();

                if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 1)
                {
                    candidate.NationalIdNumber = txtNationalIdOrBirthRegistration.Text.Trim();
                    candidate.AttributeInt1 = 1;
                    candidate.BirthRegistrationNo = null;
                }
                else if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) == 2)
                {
                    candidate.BirthRegistrationNo = txtNationalIdOrBirthRegistration.Text.Trim();
                    candidate.AttributeInt1 = 2;
                    candidate.NationalIdNumber = null;
                }
                else { }

                long candidateIdLong = -1;
                using (var db = new CandidateDataManager())
                {
                    db.Insert<DAL.BasicInfo>(candidate);
                    candidateIdLong = candidate.ID;

                    try
                    {
                        if (candidateIdLong > 0 && candidateUserIdLong > 0)
                        {
                            DAL.CandidateUser cu = db.AdmissionDB.CandidateUsers.Where(x => x.ID == candidateUserIdLong).FirstOrDefault();
                            if (cu != null)
                            {
                                string userName = cu.UsernameLoginId;
                                cu.UsernameLoginId = userName + candidateIdLong.ToString();
                                db.Update<DAL.CandidateUser>(cu);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }

                }

                #endregion



                #region CandidatePayment/CandidateFormSerial N/A

                //---------------------------------------------------------------------------------
                //insert candidate payment
                //---------------------------------------------------------------------------------
                //long candidatePaymentIDLong = -1;
                //if (admissionSetup != null)
                //{
                //    using (var db = new CandidateDataManager())
                //    {
                //        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                //        db.AdmissionDB.SPCandidatePaymentInsert(id_param, candidateIdLong, null, admissionSetup.AcaCalID, Convert.ToInt32(admissionUnit.UnitCode1), false, admissionSetup.Fee, -99, DateTime.Now);
                //        candidatePaymentIDLong = Convert.ToInt64(id_param.Value);
                //    }
                //}

                //---------------------------------------------------------------------------------
                //insert candidate form serial
                //---------------------------------------------------------------------------------
                //long candidateFormSerialIDLong = -1;
                //if (admissionSetup != null)
                //{
                //    using (var db = new CandidateDataManager())
                //    {
                //        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                //        db.AdmissionDB.SPCandidateFormSlInsert(id_param, candidateIdLong, admissionSetup.ID, admissionSetup.AcaCalID, Convert.ToInt32(admissionUnit.UnitCode1), null, candidatePaymentIDLong, DateTime.Now, -99);
                //        candidateFormSerialIDLong = Convert.ToInt64(id_param.Value);
                //    }
                //}
                #endregion
                //---------------------------------------------------------------------------------
                //insert Candidate ExamDetails
                //---------------------------------------------------------------------------------
                #region Insert Candidate ExamDetails N/A

                //int sscExamTypeT = -1;
                //int sscGroupT = -1;
                //decimal sscGPAT = 0.00M;
                //int sscPassingYearT = -1;

                //int hscExamTypeT = -1;
                //int hscGroupT = -1;
                //decimal hscGPAT = 0.00M;
                //int hscPassingYearT = -1;


                //sscExamTypeT = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
                //sscGroupT = Convert.ToInt32(ddlGroupSSC.SelectedValue);
                //sscGPAT = Convert.ToDecimal(txtGPASSC.Text);
                //sscPassingYearT = Convert.ToInt32(ddlPassYearSSC.SelectedValue);

                //hscExamTypeT = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
                //hscGroupT = Convert.ToInt32(ddlGroupHSC.SelectedValue);
                //hscGPAT = Convert.ToDecimal(txtGPAHSC.Text);
                //hscPassingYearT = Convert.ToInt32(ddlPassYearHSC.SelectedValue);



                ////-------Inserting SSC Information
                //DAL.ExamDetail examDetailSSC = new DAL.ExamDetail();
                //examDetailSSC.GroupOrSubjectID = sscGroupT;
                //examDetailSSC.GPA = sscGPAT;
                //examDetailSSC.PassingYear = sscPassingYearT;
                //examDetailSSC.DateCreated = DateTime.Now;
                //examDetailSSC.CreatedBy = -99;
                //long examDetailsSSCId = -1;
                //using (var db = new CandidateDataManager())
                //{
                //    db.Insert<DAL.ExamDetail>(examDetailSSC);
                //    examDetailsSSCId = examDetailSSC.ID;
                //}



                ////-------Inserting HSC Information
                //DAL.ExamDetail examDetailHSC = new DAL.ExamDetail();
                //examDetailHSC.GroupOrSubjectID = hscGroupT;
                //examDetailHSC.GPA = hscGPAT;
                //examDetailHSC.PassingYear = hscPassingYearT;
                //examDetailHSC.DateCreated = DateTime.Now;
                //examDetailHSC.CreatedBy = -99;


                //long examDetailsHSCId = -1;
                //using (var db = new CandidateDataManager())
                //{
                //    db.Insert<DAL.ExamDetail>(examDetailHSC);
                //    examDetailsHSCId = examDetailHSC.ID;
                //}


                //if (examDetailsSSCId > 0)
                //{
                //    DAL.Exam examSSC = new DAL.Exam();
                //    examSSC.CandidateID = candidateIdLong;
                //    examSSC.ExamTypeID = sscExamTypeT;
                //    examSSC.ExamDetailsID = examDetailsSSCId;
                //    examSSC.DateCreated = DateTime.Now;
                //    examSSC.CreatedBy = -99;

                //    long examSSCId = -1;
                //    using (var db = new CandidateDataManager())
                //    {
                //        db.Insert<DAL.Exam>(examSSC);
                //        examSSCId = examSSC.ID;
                //    }
                //}


                //if (examDetailsHSCId > 0)
                //{
                //    DAL.Exam examHSC = new DAL.Exam();
                //    examHSC.CandidateID = candidateIdLong;
                //    examHSC.ExamTypeID = hscExamTypeT;
                //    examHSC.ExamDetailsID = examDetailsHSCId;
                //    examHSC.DateCreated = DateTime.Now;
                //    examHSC.CreatedBy = -99;

                //    long examHSCId = -1;
                //    using (var db = new CandidateDataManager())
                //    {
                //        db.Insert<DAL.Exam>(examHSC);
                //        examHSCId = examHSC.ID;
                //    }
                //}



                #endregion




                //---------------------------------------------------------------------------------
                //insert Candidate SSC HSC O-Level A-Level Points
                //---------------------------------------------------------------------------------
                #region Candidate SSC HSC O-Level A-Level Points
                //Here (oLevelExamType == 5 && aLevelExamType == 7)    // O/A Level
                
                long candidateID = 0;
                decimal sscGPA = 0.00M;
                decimal hscGPA = 0.00M;
                decimal olevelPoint = 0.00M;
                decimal alevelPoint = 0.00M;
                int examTypeSSC = -1;
                int examTypeHSC = -1;
                int groupSSC = -1;
                int groupHSC = -1;
                int passingYearSSC = 2;
                int passingYearHSC = 2;
                int boardSSC = -1;
                int boardHSC = -1;
                string instituteSSC = string.Empty;
                string instituteHSC = string.Empty;


                candidateID = candidateIdLong;
                sscGPA = Convert.ToDecimal(txtGPASSC.Text);  //hfOLevelConvertedSscGPA.Value != "" ? Convert.ToDecimal(hfOLevelConvertedSscGPA.Value) : Convert.ToDecimal(txtGPASSC.Text);
                hscGPA = Convert.ToDecimal(txtGPAHSC.Text); //hfALevelConvertedHscGPA.Value != "" ? Convert.ToDecimal(hfALevelConvertedHscGPA.Value) : Convert.ToDecimal(txtGPAHSC.Text);
                olevelPoint = lblOLevelResult.Text != "" ? Convert.ToDecimal(lblOLevelResult.Text) : olevelPoint;
                alevelPoint = lblALevelResult.Text != "" ? Convert.ToDecimal(lblALevelResult.Text) : alevelPoint;
                examTypeSSC = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
                examTypeHSC = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
                groupSSC = Convert.ToInt32(ddlGroupSSC.SelectedValue);
                groupHSC = Convert.ToInt32(ddlGroupHSC.SelectedValue);
                passingYearSSC = Convert.ToInt32(ddlPassYearSSC.SelectedValue);
                passingYearHSC = Convert.ToInt32(ddlPassYearHSC.SelectedValue);
                boardSSC = Convert.ToInt32(ddlBoardSSC.SelectedValue);
                boardHSC = Convert.ToInt32(ddlBoardHSC.SelectedValue);
                instituteSSC = txtInstituteSSC.Text;
                instituteHSC = txtInstituteHSC.Text;



                //....SSC -> O-Level Insert
                if (candidateID > 0 && (sscGPA > 0 || olevelPoint > 0) && examTypeSSC > 0 && groupSSC > 0 && passingYearSSC > 0 && boardSSC > 0 && !string.IsNullOrEmpty(instituteSSC))
                {
                    #region N/A
                    //DAL.CandidateOALevelGPA a = new DAL.CandidateOALevelGPA();
                    //a.CandidateID = candidateID;
                    //a.SscAndHscGpa = sscGPA;
                    //a.OAndALevelPoint = olevelPoint;
                    //a.ExamTypeID = examTypeSSC;
                    //a.GroupID = groupSSC;
                    //a.PassingYear = passingYearSSC;

                    //int candidateSSCOALevelGPAID = -1;
                    //using (var db = new CandidateDataManager())
                    //{
                    //    db.Insert<DAL.CandidateOALevelGPA>(a);
                    //    candidateSSCOALevelGPAID = a.ID;
                    //} 
                    #endregion

                    //---------------------------------------------------------------------------------------------
                    //insert secondary / o-level / alim
                    //---------------------------------------------------------------------------------------------

                    #region Secondary/O-Level/Alim

                    DAL.ExamDetail secondaryExamDetails = new DAL.ExamDetail();

                    secondaryExamDetails.EducationBoardID = boardSSC; //EducationBoardID = 2 = DHAKA //Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);
                    secondaryExamDetails.Institute = instituteSSC; //txtSecInstitute.Text;
                    secondaryExamDetails.UndgradGradProgID = 1; // N/A
                    secondaryExamDetails.GroupOrSubjectID = groupSSC; //Convert.ToInt32(ddlSec_GrpOrSub.SelectedValue);
                    secondaryExamDetails.ResultDivisionID = 5; //Convert.ToInt32(ddlSec_DivClass.SelectedValue); // Note: 5 = GPA;  //;

                    if (examTypeSSC == 5)
                    {
                        if (sscGPA > 0)
                        {
                            secondaryExamDetails.GPA = Convert.ToDecimal(sscGPA);
                        }

                        if (olevelPoint > 0)
                        {
                            //... Marks is used for O-Level Marks
                            secondaryExamDetails.Marks = Convert.ToDecimal(olevelPoint);
                        }
                    }
                    else
                    {
                        if (sscGPA > 0)
                        {
                            secondaryExamDetails.GPA = Convert.ToDecimal(sscGPA);
                        }
                    }

                    secondaryExamDetails.PassingYear = Convert.ToInt32(passingYearSSC);
                    secondaryExamDetails.CreatedBy = -99;
                    secondaryExamDetails.DateCreated = DateTime.Now;

                    long secondaryExamDetailsIDLong = -1;
                    using (var dbSecExmDet = new CandidateDataManager())
                    {
                        dbSecExmDet.Insert<DAL.ExamDetail>(secondaryExamDetails);
                        secondaryExamDetailsIDLong = secondaryExamDetails.ID;
                    }
                    //-------------------------------
                    DAL.Exam secondaryExam = new DAL.Exam();
                    secondaryExam.ExamTypeID = examTypeSSC; //int.Parse(ddlSec_ExamType.SelectedValue);
                    secondaryExam.CandidateID = candidateIdLong;
                    secondaryExam.ExamDetailsID = secondaryExamDetailsIDLong;
                    secondaryExam.CreatedBy = -99;
                    secondaryExam.DateCreated = DateTime.Now;

                    long secondaryExamIDLong = -1;
                    using (var dbSecExm = new CandidateDataManager())
                    {
                        dbSecExm.Insert<DAL.Exam>(secondaryExam);
                        secondaryExamIDLong = secondaryExam.ID;
                    }
                    #endregion

                }

                //....HSC -> A-Level Insert
                if (candidateID > 0 && (hscGPA > 0 || alevelPoint > 0) && examTypeHSC > 0 && groupHSC > 0 && passingYearHSC > 0 && boardHSC > 0 && !string.IsNullOrEmpty(instituteHSC))
                {
                    #region N/A
                    //DAL.CandidateOALevelGPA a = new DAL.CandidateOALevelGPA();
                    //a.CandidateID = candidateID;
                    //a.SscAndHscGpa = hscGPA;
                    //a.OAndALevelPoint = alevelPoint;
                    //a.ExamTypeID = examTypeHSC;
                    //a.GroupID = groupHSC;
                    //a.PassingYear = passingYearHSC;

                    //int candidateHSCOALevelGPAID = -1;
                    //using (var db = new CandidateDataManager())
                    //{
                    //    db.Insert<DAL.CandidateOALevelGPA>(a);
                    //    candidateHSCOALevelGPAID = a.ID;
                    //} 
                    #endregion

                    //---------------------------------------------------------------------------------------------
                    //insert higher secondary / a-level / dakhil
                    //---------------------------------------------------------------------------------------------

                    #region HigherSecondary/A-Level/Dakhil

                    DAL.ExamDetail higherSecondaryExamDetails = new DAL.ExamDetail();
                    #region N/A
                    //higherSecondaryExamDetails.RollNo = txtHighSecRollNo.Text; 
                    #endregion
                    higherSecondaryExamDetails.EducationBoardID = boardHSC; //EducationBoardID = 2 = DHAKA //Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);
                    higherSecondaryExamDetails.Institute = instituteHSC; //txtHighSecInstitute.Text;
                    higherSecondaryExamDetails.UndgradGradProgID = 1; // N/A
                    higherSecondaryExamDetails.GroupOrSubjectID = groupHSC; //Convert.ToInt32(ddlHigherSec_GrpOrSub.SelectedValue);
                    higherSecondaryExamDetails.ResultDivisionID = 5; //Convert.ToInt32(ddlHigherSec_DivClass.SelectedValue); // Note: 5 = GPA; //
                    
                    if (examTypeHSC == 7)
                    {
                        if (hscGPA > 0)
                        {
                            higherSecondaryExamDetails.GPA = Convert.ToDecimal(hscGPA);
                        }

                        if (alevelPoint > 0)
                        {
                            //... Marks is used for A-Level Marks
                            higherSecondaryExamDetails.Marks = Convert.ToDecimal(alevelPoint);
                        }
                    }
                    else
                    {
                        if (hscGPA > 0)
                        {
                            higherSecondaryExamDetails.GPA = Convert.ToDecimal(hscGPA);
                        }
                    }
                    

                    higherSecondaryExamDetails.PassingYear = passingYearHSC; //Convert.ToInt32(ddlHigherSec_PassingYear.SelectedValue);
                    higherSecondaryExamDetails.CreatedBy = -99;
                    higherSecondaryExamDetails.DateCreated = DateTime.Now;

                    long higherSecondaryExamDetailsIDLong = -1;
                    using (var dbHighSecExmDtl = new CandidateDataManager())
                    {
                        dbHighSecExmDtl.Insert<DAL.ExamDetail>(higherSecondaryExamDetails);
                        higherSecondaryExamDetailsIDLong = higherSecondaryExamDetails.ID;
                    }
                    //-------------------------------
                    DAL.Exam higherSecondaryExam = new DAL.Exam();
                    higherSecondaryExam.ExamTypeID = examTypeHSC; //int.Parse(ddlHigherSec_ExamType.SelectedValue);
                    higherSecondaryExam.CandidateID = candidateIdLong;
                    higherSecondaryExam.ExamDetailsID = higherSecondaryExamDetailsIDLong;
                    higherSecondaryExam.CreatedBy = -99;
                    higherSecondaryExam.DateCreated = DateTime.Now;

                    long higherSecondaryExamIDLong = -1;
                    using (var dbHighSecExm = new CandidateDataManager())
                    {
                        dbHighSecExm.Insert<DAL.Exam>(higherSecondaryExam);
                        higherSecondaryExamIDLong = higherSecondaryExam.ID;
                    }
                    #endregion



                }

                
                #endregion





                #region Multiple Form Purchase CandidatePayment/CandidateFormSerial
                //---------------------------------------------------------------------------------
                //Multiple Form Purchase insert candidate payment and candidate form serial
                //---------------------------------------------------------------------------------
                long candidatePaymentIDLong = -1;
                long candidateFormSerialIDLong = -1;
                List<long> candidateFormSerialIDList = new List<long>();

                DAL.CandidatePayment candidatePaymentT = new DAL.CandidatePayment();
                List<DAL.CandidateFormSl> candidateFormSlT = new List<DAL.CandidateFormSl>();
                List<DAL.AdmissionUnit> admUnitObjT = new List<DAL.AdmissionUnit>();


                if (Session["CandidateFormSerial_Session"] != null && Session["CandidatePayment_Session"] != null &&
                Session["AdmUnitObj_Session"] != null)
                {

                    candidatePaymentT = (DAL.CandidatePayment)Session["CandidatePayment_Session"];

                    candidateFormSlT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];
                    //DAL.CandidateFormSl candidateFormSlTT = new DAL.CandidateFormSl();
                    //candidateFormSlTT = candidateFormSlT.FirstOrDefault();

                    admUnitObjT = (List<DAL.AdmissionUnit>)Session["AdmUnitObj_Session"];
                    //DAL.AdmissionUnit admUnitObjTT = new DAL.AdmissionUnit();
                    //admUnitObjTT = admUnitObjT.FirstOrDefault();
                }


                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                        //db.AdmissionDB.SPCandidatePaymentInsert(id_param, candidateIdLong, null, candidatePayment.AcaCalID, Convert.ToInt32(admUnitObj.UnitCode1), candidatePayment.IsPaid, candidatePayment.Amount, cId, candidatePayment.DateCreated);
                        db.AdmissionDB.SPCandidatePaymentInsert(id_param, candidateIdLong, null, admissionSetup.AcaCalID, Convert.ToInt32(admissionUnit.UnitCode1), false, candidatePaymentT.Amount, -99, DateTime.Now);
                        candidatePaymentIDLong = Convert.ToInt64(id_param.Value);
                        if (candidatePaymentIDLong > 0)
                        {
                            if (candidateFormSlT.Count() > 0)
                            {
                                foreach (DAL.CandidateFormSl item in candidateFormSlT)
                                {
                                    item.CandidatePaymentID = candidatePaymentIDLong; //add candidatePaymentID to candidateFormSerial as foreign key.

                                    using (var dbInsertFsl = new CandidateDataManager())
                                    {
                                        ObjectParameter id_param1 = new ObjectParameter("iD", typeof(long));
                                        //db.AdmissionDB.SPCandidateFormSlInsert(id_param1, candidateIdLong, admissionSetup.ID, admissionSetup.AcaCalID, null, candidatePaymentIDLong, DateTime.Now, -99);
                                        //dbInsertFsl.AdmissionDB.SPCandidateFormSlInsert(id_param1, item.CandidateID, item.AdmissionSetupID, item.AcaCalID, Convert.ToInt32(item.Attribute2), null, candidatePaymentIDLong, DateTime.Now, cId);
                                        db.AdmissionDB.SPCandidateFormSlInsert(id_param, candidateIdLong, item.AdmissionSetupID, item.AcaCalID, Convert.ToInt32(item.Attribute2), null, candidatePaymentIDLong, DateTime.Now, -99);
                                        candidateFormSerialIDLong = Convert.ToInt64(id_param.Value);
                                        candidateFormSerialIDList.Add(candidateFormSerialIDLong);
                                    }
                                }//end foreach
                            }// end if (candidateFormSl.Count() > 0)
                        }// end if (candidatePaymentIDLong > 0)
                    }
                }
                catch (Exception)
                {
                    //messagePanel.Visible = true;
                    //lblMessage1.Text = "Error saving Payment ID.";
                    //messagePanel.CssClass = "alert alert-danger";
                    //return;
                }
                #endregion



                if (candidateIdLong > 0 && candidateUserIdLong > 0 &&
                    candidatePaymentIDLong > 0 && candidateFormSerialIDLong > 0)
                {

                    #region N/A
                    //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                    //    + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
                    //    + admissionSetup.EducationCategoryID + ";";

                    //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                    //            + -1 + ";1;" + -1 + ";"
                    //            + admissionSetup.EducationCategoryID + ";"; 
                    #endregion


                    Session["CandidateFormSerial_Session"] = null;
                    Session["CandidatePayment_Session"] = null;
                    Session["AdmUnitObj_Session"] = null;


                    /// <summary>
                    /// 1) CandidateId
                    /// 2) CandidatePaymentID
                    /// 3) CandidateFormSerialID
                    /// 4) IsMultiple
                    /// 5) AdmUnitId
                    /// 6) EducationCategoryId
                    /// </summary>

                    string urlParam =   candidateIdLong + ";"
                                        + candidatePaymentIDLong + ";"
                                        + -1 
                                        + ";1;" 
                                        + -1 + ";"
                                        + 4 + ";";

                    SendSMSAndEmail(candidateIdLong, urlParam);

                    Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
                }
                else
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code PF01X001IE?type=danger", false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code PF01X002TC?type=danger", false);
            }
            //}
        }

        protected void SendSMSAndEmail(long candidateId, string paramValue)
        {
            try
            {

                DAL.CandidatePayment cp = null;
                DAL.CandidatePayment candidatePaymentObj = null;
                string admUnitProgramStr = string.Empty;
                using (var db = new CandidateDataManager())
                {
                    cp = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == candidateId).FirstOrDefault();
                    candidatePaymentObj = db.GetCandidatePaymentByID_AD(cp.ID);
                    if (candidatePaymentObj != null)
                    {
                        List<DAL.CandidateFormSl> candFormSerialList = candidatePaymentObj.CandidateFormSls.ToList();
                        List<DAL.AdmissionSetup> admSetupList = new List<DAL.AdmissionSetup>();
                        List<DAL.AdmissionUnit> admUnitList = new List<DAL.AdmissionUnit>();


                        bool showBkashButton = true;
                        if (candFormSerialList.Count() > 0)
                        {
                            foreach (var item in candFormSerialList)
                            {
                                DAL.AdmissionSetup admSetupItem = db.AdmissionDB.AdmissionSetups.Find(item.AdmissionSetupID);
                                if (admSetupItem != null)
                                {
                                    admSetupList.Add(admSetupItem);
                                    if (admSetupItem.Attribute1 == null)
                                    {
                                        showBkashButton = false;
                                    }
                                }
                            }
                        }


                        if (admSetupList.Count() > 0)
                        {
                            foreach (var item in admSetupList)
                            {
                                List<DAL.AdmissionUnit> admUnitListForEachAdmSetup = db.AdmissionDB.AdmissionUnits
                                    .Where(a => a.ID == item.AdmissionUnitID).ToList();
                                if (admUnitListForEachAdmSetup.Count() > 0)
                                {
                                    admUnitList.AddRange(admUnitListForEachAdmSetup);
                                }
                            }
                        }


                        if (admUnitList.Count() > 0)
                        {
                            foreach (var item in admUnitList)
                            {
                                admUnitProgramStr += item.UnitName + "<br />";
                            }
                        }


                    }

                    #region Send EMAIL

                    try
                    {
                        string mailBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + ", <br/><br/>" +
                            "Thank you for applying at Bangladesh University of Professionals (BUP). Please proceed for payment or you can pay later following the link : " +  //"Thank you for applying at Bangladesh University of Professionals (BUP). Please check the information below about your payment: " +
                            "<br/><br/>" +
                            "<b>Link : </b><a href='http://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + paramValue + "'> http://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=value </a>" +
                            "<strong style='color: Tomato;'>Payment ID: " + candidatePaymentObj.PaymentId.ToString() + "</strong>" +
                            "<br/><br/>" +
                            "Applied For :" + "<br/>" + admUnitProgramStr + ". <br/>" +
                            "<br/>" +
                            "</p>" +
                            "<p style='color: green;'>Important : You must login, fill up and submit the Application Form after successful Payment.</p>" +
                            "<p>" +
                            "<br/>" +
                            "Regards," +
                            "<br/>" +
                            "Bangladesh University of Professionals (BUP)" +
                            "</p>";
                        //"Regards, <br/>" ICT Centre,
                        //"Admin" +
                        //"<br/>" +
                        string fromAddress = "no-reply@bup.edu.bd";
                        string senderName = "BUP Admission";
                        string subject = "Payment ID for Your Application";

                        //bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);
                        bool isSentEmail = EmailUtility.SendMailbynoreplymail(candidatePaymentObj.BasicInfo.Email, fromAddress, senderName, subject, mailBody);

                        if (isSentEmail == true)
                        {
                            DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                            eLog.MessageBody = mailBody;
                            eLog.MessageSubject = subject;
                            eLog.Page = "PurchaseFormBachelor.aspx";
                            eLog.SentBy = "System";
                            eLog.StudentId = candidatePaymentObj.CandidateID;
                            eLog.ToAddress = candidatePaymentObj.BasicInfo.Email;
                            eLog.ToName = candidatePaymentObj.BasicInfo.FirstName;
                            eLog.DateSent = DateTime.Now;
                            eLog.FromAddress = fromAddress;
                            eLog.FromName = senderName;
                            eLog.Attribute1 = "Success";

                            LogWriter.EmailLog(eLog);
                        }
                        else if (isSentEmail == false)
                        {
                            DAL_Log.EmailLog eLog = new DAL_Log.EmailLog();
                            eLog.MessageBody = mailBody;
                            eLog.MessageSubject = subject;
                            eLog.Page = "PurchaseFormBachelor.aspx";
                            eLog.SentBy = "System";
                            eLog.StudentId = candidatePaymentObj.CandidateID;
                            eLog.ToAddress = candidatePaymentObj.BasicInfo.Email;
                            eLog.ToName = candidatePaymentObj.BasicInfo.FirstName;
                            eLog.DateSent = DateTime.Now;
                            eLog.FromAddress = fromAddress;
                            eLog.FromName = senderName;
                            eLog.Attribute1 = "Failed";

                            LogWriter.EmailLog(eLog);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion

                    #region Send SMS
                    try
                    {
                        GetSendingInfo(candidatePaymentObj.CandidateID, paramValue, candidatePaymentObj);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {

            }
        }
        private void GetSendingInfo(long? candidateId, string value, DAL.CandidatePayment candidatePaymentObj)
        {
            if (candidateId != null)
            {
                if (candidateId > 0)
                {
                    DAL.BasicInfo candidate = null;
                    DAL.CandidateUser candidateUser = null;

                    string candidateUsername = null;
                    string candidatePassword = null;
                    string candidateSmsPhone = null;
                    string candidateEmail = null;

                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(candidateId);
                    }

                    if (candidate != null)
                    {
                        candidateEmail = candidate.Email;
                        candidateSmsPhone = candidate.SMSPhone;
                        using (var db = new CandidateDataManager())
                        {
                            candidateUser = db.AdmissionDB.CandidateUsers.Find(candidate.CandidateUserID);
                        }
                    }

                    if (candidateUser != null)
                    {
                        candidateUsername = candidateUser.UsernameLoginId;
                        candidatePassword = candidateUser.Password;
                    }

                    if (!string.IsNullOrEmpty(candidateUsername) && !string.IsNullOrEmpty(candidatePassword) &&
                        !string.IsNullOrEmpty(candidateSmsPhone) && !string.IsNullOrEmpty(candidateEmail))
                    {
                        SendSms(candidateSmsPhone, candidate.ID, value, candidatePaymentObj);
                        //SendEmail(candidateEmail, candidateUsername, candidatePassword, candidate.ID);
                    }
                }
            }
        }
        private void SendSms(string smsPhone, long candidateId, string value, DAL.CandidatePayment candidatePaymentObj)
        {
            if (!string.IsNullOrEmpty(smsPhone) && smsPhone.Count() == 14 && smsPhone.Contains("+"))
            {
                //string messageBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + "," + "\n" +
                //                    "Payment ID : " + candidatePaymentObj.PaymentId.ToString() + "\n" +
                //                    "and Payment Link : http://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value + "\n" +
                //                    "BUP";

                string messageBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + "," +
                                    " Payment ID : " + candidatePaymentObj.PaymentId.ToString() +
                                    " and Payment Link : http://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value +
                                    " BUP";

                string stringData = SMSUtility.Send(smsPhone, messageBody);

                string statusT = JObject.Parse(stringData)["statusCode"].ToString();

                if (statusT != "200") //if sms sending fails
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending failed in PurchaseFormBachelor.aspx";
                    smsLog.Attribute2 = "Failed";
                    smsLog.Attribute3 = null;
                    smsLog.CreatedBy = candidateId;
                    smsLog.CreatedDate = DateTime.Now;
                    smsLog.CurrentSMSReferenceNo = stringData;
                    smsLog.Message = messageBody;
                    smsLog.StudentId = candidateId;
                    smsLog.PhoneNo = smsPhone;
                    smsLog.SenderUserId = -99;
                    smsLog.SentReferenceId = null;
                    smsLog.SentSMSId = null;
                    smsLog.SmsSendDate = DateTime.Now;
                    smsLog.SmsType = -1;

                    LogWriter.SmsLog(smsLog);
                }
                else //if sms sending passed
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending successful PurchaseFormBachelor.aspx";
                    smsLog.Attribute2 = "Success";
                    smsLog.Attribute3 = null;
                    smsLog.CreatedBy = candidateId;
                    smsLog.CreatedDate = DateTime.Now;
                    smsLog.CurrentSMSReferenceNo = stringData;
                    smsLog.Message = messageBody;
                    smsLog.StudentId = candidateId;
                    smsLog.PhoneNo = smsPhone;
                    smsLog.SenderUserId = -99;
                    smsLog.SentReferenceId = null;
                    smsLog.SentSMSId = null;
                    smsLog.SmsSendDate = DateTime.Now;
                    smsLog.SmsType = -1;

                    LogWriter.SmsLog(smsLog);
                }
            }
        }

        private bool IsNumeric(string s)
        {
            Regex r = new Regex(@"^\d{0,2}(\.\d{1,2})?$");

            return r.IsMatch(s);
        }


        private Dictionary<int, string> ValidateAllFieldIsGiven()
        {

            Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
            int i = 1;

            try
            {
                if (txtName.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Name is Empty !");
                }

                if (ddlDay.SelectedValue == "-1" || ddlMonth.SelectedValue == "-1" || ddlYear.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "Date of Birth hasn't Selected !");
                }

                if (txtEmail.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Email is Empty !");
                }

                if (txtSmsMobile.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Mobile No. for SMS is Empty !");
                }

                if (txtGuardianMobile.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "Guardian's Mobile No. is Empty !");
                }

                if (ddlGender.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "Gender hasn't Selected !");
                }

                if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) > 0)
                {
                    if (txtNationalIdOrBirthRegistration.Text.Trim().ToString() == "")
                    {
                        dictErrorList.Add(i++, "National ID / Birth Registration Number is Empty !");
                    }

                    try
                    {
                        if (Convert.ToInt32(ddlNationalIdOrBirthRegistration.SelectedValue) > 0)
                        {
                            
                        }
                        else
                        {
                            dictErrorList.Add(i++, "Invalie formate of National ID / Birth Registration Number !");
                        }
                    }
                    catch (Exception ex1)
                    {

                        dictErrorList.Add(i++, "Invalie formate of National ID / Birth Registration Number !");
                    }
                }
                else
                {
                    dictErrorList.Add(i++, "National ID / Birth Registration Number hasn't Selected !");
                }


                // ==== SSC
                if (txtInstituteSSC.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "SSC Institute is Empty !!");
                }

                if (ddlBoardSSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "SSC Board hasn't Selected !!");
                }

                if (ddlExamTypeSSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "SSC Exam Type hasn't Selected !!");
                }

                if (ddlGroupSSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "SSC Group hasn't Selected !!");
                }

                //if (ddlSec_DivClass.SelectedValue == "-1")
                //{
                //    dictErrorList.Add(i++, "SSC Class hasn't Selected !!");
                //}

                if (txtGPASSC.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "SSC GPA is Empty !!");
                }

                if (ddlPassYearSSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "SSC Passing Year hasn't Selected !!");
                }

                if (!string.IsNullOrEmpty(txtGPASSC.Text))
                {
                    decimal sscCGPA = Convert.ToDecimal(txtGPASSC.Text);

                    if (sscCGPA > 0 && sscCGPA <= 5.00M)
                    {

                    }
                    else
                    {
                        dictErrorList.Add(i++, "SSC GPA should be within 5  !!");
                    }
                }

       


                // ==== HSC
                if (txtInstituteHSC.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "HSC Institute is Empty !!");
                }

                if (ddlBoardHSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "HSC Board hasn't Selected !!");
                }

                if (ddlExamTypeHSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "HSC Exam Type hasn't Selected !!");
                }

                if (ddlGroupHSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "HSC Group hasn't Selected !!");
                }

                //if (ddlHigherSec_DivClass.SelectedValue == "-1")
                //{
                //    dictErrorList.Add(i++, "HSC Class hasn't Selected !!");
                //}

                if (txtGPAHSC.Text.ToString() == "")
                {
                    dictErrorList.Add(i++, "HSC GPA is Empty !!");
                }

                if (ddlPassYearHSC.SelectedValue == "-1")
                {
                    dictErrorList.Add(i++, "HSC Passing Year hasn't Selected !!");
                }


                if (!string.IsNullOrEmpty(txtGPAHSC.Text))
                {
                    decimal hscCGPA = Convert.ToDecimal(txtGPAHSC.Text);

                    if (hscCGPA > 0 && hscCGPA <= 5.00M)
                    {

                    }
                    else
                    {
                        dictErrorList.Add(i++, "HSC GPA should be within 5  !!");
                    }
                }

                #region Program Priority Is Selected or Not
                //int countSelectedPriority = 0;
                //bool firstPriority = false;
                //int programPriorityId = -1;
                //foreach (GridViewRow gvrow in gvProgramPriority.Rows)
                //{
                //    DropDownList ddlPriorityId = (DropDownList)gvrow.FindControl("ddlPriority");

                //    programPriorityId = Convert.ToInt32(ddlPriorityId.SelectedValue);

                //    if (programPriorityId > 0)
                //    {
                //        countSelectedPriority++;
                //        if (programPriorityId == 1)
                //        {
                //            firstPriority = true;
                //        }

                //    }

                //}

                //if (countSelectedPriority == 0)
                //{
                //    dictErrorList.Add(i++, "Please Select Program Priority !!");

                //}
                //else
                //{
                //    if (firstPriority == false)
                //    {
                //        dictErrorList.Add(i++, "You Must Select Any Program As 1st Priority !!");
                //        panel_Massage.Visible = true;
                //        programPriorityMassage.Text = "**You Must Select Any Program As 1st Priority !!";
                //        programPriorityMassage.ForeColor = Color.Crimson;
                //    }
                //}
                #endregion


                return dictErrorList;
            }
            catch (Exception ex)
            {

                dictErrorList.Add(i++, "Exception: " + ex.Message.ToString());

                return dictErrorList;
            }

        }


        protected MessageModel CheckEligibleValidation(int sscExamType, int sscGroup, decimal sscGPA, int hscExamType, int hscGroup, decimal hscGPA)
        {
            MessageModel mModel = new MessageModel();

            //bool result = true;

            int i = 1;
            Dictionary<int, string> messageList = new Dictionary<int, string>();

            try
            {

                #region Get Data From Session (Admission Unit, Payment, Form Serial)
                DAL.CandidatePayment candidatePaymentT = new DAL.CandidatePayment();
                List<DAL.CandidateFormSl> candidateFormSlT = new List<DAL.CandidateFormSl>();
                List<DAL.AdmissionUnit> admUnitObjT = new List<DAL.AdmissionUnit>();

                if (Session["CandidateFormSerial_Session"] != null &&
                    Session["CandidatePayment_Session"] != null &&
                    Session["AdmUnitObj_Session"] != null)
                {
                    candidatePaymentT = (DAL.CandidatePayment)Session["CandidatePayment_Session"];

                    candidateFormSlT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];

                    admUnitObjT = (List<DAL.AdmissionUnit>)Session["AdmUnitObj_Session"];
                }
                #endregion


                decimal totalGPAPointSetup = 0.00M;
                List<DAL.AdmissionUnit> admUnitObjForRemovedList = new List<DAL.AdmissionUnit>();

                #region New Validation Check for SSC & HSC
                try
                {
                    if ((sscExamType > 0 && sscGroup > 0 && sscGPA > 0) && (hscExamType > 0 && hscGroup > 0 && hscGPA > 0))
                    {
                        foreach (DAL.AdmissionUnit item in admUnitObjT)
                        {
                            #region Get Total GPA Point for Single Faculty
                            DAL.AdmissionSscHscGpaSetup ashgs = null;
                            using (var db = new OfficeDataManager())
                            {
                                ashgs = db.AdmissionDB.AdmissionSscHscGpaSetups.Where(x => x.AdmissionUnitID == item.ID && x.ExamTypeID == hscExamType && x.GroupID == hscGroup).FirstOrDefault();
                            }

                            if (ashgs != null && ashgs.TotalGPAPoint != null && !string.IsNullOrEmpty(ashgs.TotalGPAPoint.ToString()))
                            {
                                totalGPAPointSetup = Convert.ToDecimal(ashgs.TotalGPAPoint);
                            }
                            #endregion

                            if (totalGPAPointSetup > 0)
                            {
                                decimal sumSSCHSCGPAPoint = sscGPA + hscGPA;


                                /// <summary>
                                /// (Sum SSC-HSC GPA Point) ta Setup ar point the boro or equal hole she ai faculty te Eligible
                                /// Na hoile ai faculty te Not-Eligible
                                /// </summary>
                                if (sumSSCHSCGPAPoint >= totalGPAPointSetup)
                                {

                                    /// <summary>
                                    /// IF Setup Point and SUM Point is ok 
                                    /// Then Check (SSC-HSC), Faculty-ExamType-Group-GPA is Eligible or not
                                    /// IF Not, Then This Faculty is Not-Eligible
                                    /// </summary>

                                    DAL.AdmissionSscHscGpaSetup sscASHGS = null;
                                    DAL.AdmissionSscHscGpaSetup hscASHGS = null;
                                    using (var db = new OfficeDataManager())
                                    {
                                        sscASHGS = db.AdmissionDB.AdmissionSscHscGpaSetups.Where(x => x.AdmissionUnitID == item.ID
                                                                                                   && x.ExamTypeID == sscExamType
                                                                                                   && x.GroupID == sscGroup
                                                                                                   && (sscGPA >= x.GPA && sscGPA <= 5)).FirstOrDefault();

                                        hscASHGS = db.AdmissionDB.AdmissionSscHscGpaSetups.Where(x => x.AdmissionUnitID == item.ID
                                                                                                   && x.ExamTypeID == hscExamType
                                                                                                   && x.GroupID == hscGroup
                                                                                                   && (hscGPA >= x.GPA && hscGPA <= 5)).FirstOrDefault();
                                    }

                                    if (sscASHGS != null && hscASHGS != null)
                                    {

                                    }
                                    else
                                    {
                                        // Listed Removed Facultys
                                        admUnitObjForRemovedList.Add(item);
                                    }

                                }
                                else
                                {
                                    // Listed Removed Facultys
                                    admUnitObjForRemovedList.Add(item);
                                }
                            }
                            else
                            {
                                messageList.Add(i++, "Total GPA Point is not Setup for Faculty: " + item.UnitName.ToString());
                            }

                        }
                    }
                    else
                    {
                        messageList.Add(i++, "Please provide SSC and HSC all input values !!");
                    }
                }
                catch (Exception ex)
                {
                    messageList.Add(i++, "Exception: Validation Check for SSC & HSC; Error: " + ex.Message.ToString());
                }
                #endregion



                if (messageList != null && messageList.Count > 0)
                {
                    string message = "";
                    foreach (var msg in messageList)
                    {
                        message = message + msg.Key.ToString() + ". " + msg.Value + "<br/>";
                    }

                    mModel.MessageCode = 2;
                    mModel.MessageBody = message;
                    mModel.MessageBoolean = false;
                }
                else
                {
                    DAL.AdmissionSetup admSetupTemp = new DAL.AdmissionSetup();
                    DAL.CandidateFormSl canForTemp = new DAL.CandidateFormSl();

                    List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleAndNotEligibleList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();

                    #region New Delete Not Eligible Faculty from Selected Faculty
                    try
                    {
                        if (admUnitObjForRemovedList != null && admUnitObjForRemovedList.Count > 0)
                        {

                            #region N/A -- Preparing a View List for showing Eligible & Not-Eligible List
                            //try
                            //{
                            //    // Attribute2 = 1 = Eligible
                            //    // Attribute2 = 2 = Not Eligible

                            //    foreach (var tData in admUnitObjT)
                            //    {
                            //        DAL.AdmissionUnit au = admUnitObjForRemovedList.Where(x => x.ID == tData.ID).FirstOrDefault();
                            //        if (au != null)
                            //        {
                            //            eligibleAndNotEligibleList.Add(
                            //                new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result
                            //                {
                            //                    Attribute1 = tData.UnitName,
                            //                    Attribute2 = "2",
                            //                    Attribute3 = "Not Eligible"
                            //                });
                            //        }
                            //        else
                            //        {
                            //            eligibleAndNotEligibleList.Add(
                            //                new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result
                            //                {
                            //                    Attribute1 = tData.UnitName,
                            //                    Attribute2 = "1",
                            //                    Attribute3 = "Eligible"
                            //                });
                            //        }

                            //    } //END: Foreach

                            //    if (eligibleAndNotEligibleList != null && eligibleAndNotEligibleList.Count > 0)
                            //    {
                            //        Session["Eligible_NotEligible_List"] = null;
                            //        Session["Eligible_NotEligible_List"] = eligibleAndNotEligibleList;
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //}
                            #endregion


                            #region Remove Faculty from Selected List
                            foreach (var item in admUnitObjForRemovedList)
                            {
                                canForTemp = candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault();
                                try
                                {
                                    using (var db = new OfficeDataManager())
                                    {
                                        admSetupTemp = db.AdmissionDB.AdmissionSetups.Find(canForTemp.AdmissionSetupID);
                                    }
                                }
                                catch (Exception)
                                {

                                }



                                //----removing faculty which one is not eligible
                                candidateFormSlT.Remove(candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault());



                                //--- removing faculty amount which one is not wligible
                                candidatePaymentT.Amount = candidatePaymentT.Amount - admSetupTemp.Fee;
                            }
                            #endregion

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion



                    Session["CandidatePayment_Session"] = candidatePaymentT;
                    DAL.CandidatePayment candidatePaymentTT = new DAL.CandidatePayment();
                    candidatePaymentTT = (DAL.CandidatePayment)Session["CandidatePayment_Session"];

                    Session["CandidateFormSerial_Session"] = candidateFormSlT;
                    List<DAL.CandidateFormSl> candidateFormSlTT = new List<DAL.CandidateFormSl>();
                    candidateFormSlTT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];


                    if (candidateFormSlTT.Count > 0 && candidatePaymentTT.Amount > 0)
                    {
                        mModel.MessageCode = 1;
                        mModel.MessageBody = "";
                        mModel.MessageBoolean = true;
                    }
                    else
                    {
                        mModel.MessageCode = 2;
                        mModel.MessageBody = "You are not Eligible to Apply..!!";
                        mModel.MessageBoolean = false;
                    }
                }


                return mModel;

            }
            catch (Exception ex)
            {
                mModel.MessageCode = 2;
                mModel.MessageBody = "Exception: Eligible Validation; Error: " + ex.Message.ToString();
                mModel.MessageBoolean = false;

                return mModel;
            }


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
            {
                LoadCaptcha();
                captchaMsg.Visible = true;
                return;
            }

            
            #region gets admissionsetupid and admissionunitid

            int admissionSetupIDLong = -1;
            int admissionUnitIDLong = -1;
            if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                admissionSetupIDLong = Convert.ToInt32(Request.QueryString["asi"]);
                admissionUnitIDLong = Convert.ToInt32(Request.QueryString["aui"]);
            }

            #endregion


            DAL.AdmissionSetup admSet = new DAL.AdmissionSetup();
            using (var db = new GeneralDataManager())
            {
                admSet = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == admissionSetupIDLong).FirstOrDefault();
            }

            Dictionary<int, string> dictErrorList = new Dictionary<int, string>();

            if (admSet != null && admSet.EducationCategoryID == 4)
            {
                #region Check all field is fillup in Form
                dictErrorList = ValidateAllFieldIsGiven();

                if (dictErrorList.Count > 0)
                {
                    string massageError = "";
                    foreach (var tData in dictErrorList)
                    {
                        massageError = massageError + tData.Key.ToString() + ") " + tData.Value.ToString() + "<br/>";
                    }

                    MessageView(massageError, "fail");
                    return;
                }
                #endregion


                #region Get Values From Field
                int sscExamType = -1;
                int sscGroup = -1;
                decimal sscGPA = 0.00M;
                int sscPassingYear = -1;

                int hscExamType = -1;
                int hscGroup = -1;
                decimal hscGPA = 0.00M;
                int hscPassingYear = -1;



                sscExamType = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
                sscGroup = Convert.ToInt32(ddlGroupSSC.SelectedValue);
                sscGPA = Convert.ToDecimal(txtGPASSC.Text);
                sscPassingYear = Convert.ToInt32(ddlPassYearSSC.SelectedValue);

                hscExamType = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
                hscGroup = Convert.ToInt32(ddlGroupHSC.SelectedValue);
                hscGPA = Convert.ToDecimal(txtGPAHSC.Text);
                hscPassingYear = Convert.ToInt32(ddlPassYearHSC.SelectedValue);
                #endregion


                /// <summary>
                /// Only SSC & HSC
                /// AND O-Level & A-Level
                /// Are allowed to apply
                /// </summary>

                if (sscExamType == 5 && hscExamType == 7)    // O/A Level
                {
                    oLevelExamType = 5;
                    aLevelExamType = 7;

                    PurchaseApplicationForm();
                }
                else if (
                    (sscExamType == 1 || sscExamType == 6 || sscExamType == 12) && (hscExamType == 2 || hscExamType == 8 || hscExamType == 13)
                ) // SSC & HSC, Dakhil & Alim, SSC (Vocational) & HSC (Vocational)
                {
                    //.... Check Eligible Validation for Faculty Wise
                    MessageModel resultCheckEligibleValidation = CheckEligibleValidation(sscExamType, sscGroup, sscGPA, hscExamType, hscGroup, hscGPA);

                    if (resultCheckEligibleValidation != null && resultCheckEligibleValidation.MessageBoolean == true)
                    {
                        PurchaseApplicationForm();
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                        MessageView(resultCheckEligibleValidation.MessageBody, "fail");
                    }
                }
                else
                {
                    btnSubmit.Visible = false;
                    MessageView("You are not allowed to apply. You are requested to contact with BUP Admission Helpline (09666 790 790)", "fail");
                }

                #region N/A
                //else if (sscExamType == 6 && hscExamType == 8)    // Dakhil/Alim
                //{
                //    PurchaseApplicationForm();
                //}
                //else if (sscExamType == 1 && hscExamType == 8)    // SSC / Alim
                //{
                //    PurchaseApplicationForm();
                //}
                //else if (sscExamType == 6 && hscExamType == 2)    //Dakhil / HSC
                //{
                //    PurchaseApplicationForm();
                //}

                //else
                //{
                //    btnSubmit.Visible = false;
                //    EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
                //} 
                #endregion
            
            }
            else
            {
                Response.Redirect("~/Admission/Message.aspx?message=You don't have Permission to acccess that Page&type=danger", false);
            }

            #region N/A
            //if (admissionUnitIDLong == 2 || admissionUnitIDLong == 3 || admissionUnitIDLong == 4 || admissionUnitIDLong == 5)
            //{
            //    int sscExamType = -1;
            //    int sscGroup = -1;
            //    decimal sscGPA = 0.00M;
            //    int sscPassingYear = -1;

            //    int hscExamType = -1;
            //    int hscGroup = -1;
            //    decimal hscGPA = 0.00M;
            //    int hscPassingYear = -1;

            //    bool sscIsDecimal = false;
            //    bool hscIsDecimal = false;


            //    sscExamType = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
            //    sscGroup = Convert.ToInt32(ddlGroupSSC.SelectedValue);
            //    sscGPA = Convert.ToDecimal(txtGPASSC.Text);
            //    sscPassingYear = Convert.ToInt32(ddlPassYearSSC.SelectedValue);

            //    hscExamType = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
            //    hscGroup = Convert.ToInt32(ddlGroupHSC.SelectedValue);
            //    hscGPA = Convert.ToDecimal(txtGPAHSC.Text);
            //    hscPassingYear = Convert.ToInt32(ddlPassYearHSC.SelectedValue);


            //    List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admSscList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();
            //    List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admHscList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();


            //    sscIsDecimal = IsNumeric(sscGPA.ToString());
            //    hscIsDecimal = IsNumeric(hscGPA.ToString());
            //    if (sscIsDecimal == false || hscIsDecimal == false)
            //    {
            //        MessageView("Your SSC or HSC GPA is not in Correct Format..!!", "fail");
            //        return;
            //    }

            //    DAL.CandidatePayment candidatePaymentT = new DAL.CandidatePayment();
            //    List<DAL.CandidateFormSl> candidateFormSlT = new List<DAL.CandidateFormSl>();
            //    List<DAL.AdmissionUnit> admUnitObjT = new List<DAL.AdmissionUnit>();

            //    if (Session["CandidateFormSerial_Session"] != null && Session["CandidatePayment_Session"] != null &&
            //    Session["AdmUnitObj_Session"] != null)
            //    {

            //        candidatePaymentT = (DAL.CandidatePayment)Session["CandidatePayment_Session"];

            //        candidateFormSlT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];
            //        //DAL.CandidateFormSl candidateFormSlTT = new DAL.CandidateFormSl();
            //        //candidateFormSlTT = candidateFormSlT.FirstOrDefault();

            //        admUnitObjT = (List<DAL.AdmissionUnit>)Session["AdmUnitObj_Session"];
            //        //DAL.AdmissionUnit admUnitObjTT = new DAL.AdmissionUnit();
            //        //admUnitObjTT = admUnitObjT.FirstOrDefault();
            //    }


            //    List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();
            //    //List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleHSCList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();





            //    if (sscExamType == 5 && hscExamType == 7)    // O/A Level
            //    {
            //        oLevelExamType = 5;
            //        aLevelExamType = 7;
            //        PurchaseApplicationForm();
            //    }
            //    //if (admSscList.Count > 0 && admHscList.Count > 0)
            //    else //if (sscExamType == 1 && hscExamType == 2)       // SSC / HSC
            //    {

            //        #region Validation Check For SSC
            //        if (admissionUnitIDLong > 0 && sscExamType > 0 && sscGroup > 0 && sscGPA > 0)
            //        {
            //            try
            //            {
            //                //using (var db = new OfficeDataManager())
            //                //{
            //                //    admSscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(admissionUnitIDLong, sscExamType, sscGroup, sscGPA).ToList();
            //                //}

            //                if (admUnitObjT.Count() > 0)
            //                {
            //                    foreach (DAL.AdmissionUnit item in admUnitObjT)
            //                    {
            //                        DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result tempEligibleSSCList = new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result();
            //                        using (var db = new OfficeDataManager())
            //                        {

            //                            admSscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(item.ID, sscExamType, sscGroup, sscGPA).ToList();
            //                            if (admSscList.Count > 0)
            //                            {
            //                                tempEligibleSSCList.ID = admSscList[0].ID;
            //                                tempEligibleSSCList.AdmissionUnitID = admSscList[0].AdmissionUnitID;
            //                                tempEligibleSSCList.ExamTypeID = admSscList[0].ExamTypeID;
            //                                tempEligibleSSCList.GroupID = admSscList[0].GroupID;
            //                                tempEligibleSSCList.GPA = admSscList[0].GPA;

            //                                eligibleList.Add(tempEligibleSSCList);
            //                            }

            //                        }

            //                    }//end foreach
            //                }// end if (admUnitObjT.Count() > 0)

            //            }
            //            catch (Exception ex)
            //            {
            //                //Console.WriteLine(ex.StackTrace);
            //            }
            //        }
            //        #endregion

            //        #region Validation Check HSC
            //        if (admissionUnitIDLong > 0 && hscExamType > 0 && hscGroup > 0 && hscGPA > 0)
            //        {
            //            try
            //            {
            //                //using (var db = new OfficeDataManager())
            //                //{
            //                //    admHscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(admissionUnitIDLong, hscExamType, hscGroup, hscGPA).ToList();
            //                //}


            //                if (admUnitObjT.Count() > 0)
            //                {
            //                    foreach (DAL.AdmissionUnit item in admUnitObjT)
            //                    {
            //                        DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result tempEligibleHSCList = new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result();
            //                        using (var db = new OfficeDataManager())
            //                        {

            //                            admHscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(item.ID, hscExamType, hscGroup, hscGPA).ToList();
            //                            if (admHscList.Count > 0)
            //                            {
            //                                tempEligibleHSCList.ID = admHscList[0].ID;
            //                                tempEligibleHSCList.AdmissionUnitID = admHscList[0].AdmissionUnitID;
            //                                tempEligibleHSCList.ExamTypeID = admHscList[0].ExamTypeID;
            //                                tempEligibleHSCList.GroupID = admHscList[0].GroupID;
            //                                tempEligibleHSCList.GPA = admHscList[0].GPA;

            //                                eligibleList.Add(tempEligibleHSCList);
            //                            }

            //                        }

            //                    }//end foreach
            //                }// end if (admUnitObjT.Count() > 0)



            //            }
            //            catch (Exception ex)
            //            {
            //                //Console.WriteLine(ex.StackTrace);
            //            }
            //        } 
            //        #endregion



            //        DAL.AdmissionSetup admSetupTemp = new DAL.AdmissionSetup();
            //        DAL.CandidateFormSl canForTemp = new DAL.CandidateFormSl();


            //        foreach (DAL.AdmissionUnit item in admUnitObjT)
            //        {
            //            if (eligibleList.Where(x => x.AdmissionUnitID == item.ID).ToList().Count == 2)
            //            {

            //            }
            //            else
            //            {

            //                canForTemp = candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault();
            //                try
            //                {
            //                    using (var db = new OfficeDataManager())
            //                    {
            //                        admSetupTemp = db.AdmissionDB.AdmissionSetups.Find(canForTemp.AdmissionSetupID);
            //                    }
            //                }
            //                catch (Exception)
            //                {

            //                }



            //                //----removing faculty which one is not eligible
            //                candidateFormSlT.Remove(candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault());



            //                //--- removing faculty amount which one is not wligible
            //                candidatePaymentT.Amount = candidatePaymentT.Amount - admSetupTemp.Fee;
            //            }
            //        }


            //        Session["CandidatePayment_Session"] = candidatePaymentT;
            //        DAL.CandidatePayment candidatePaymentTT = new DAL.CandidatePayment();
            //        candidatePaymentTT = (DAL.CandidatePayment)Session["CandidatePayment_Session"];

            //        Session["CandidateFormSerial_Session"] = candidateFormSlT;
            //        List<DAL.CandidateFormSl> candidateFormSlTT = new List<DAL.CandidateFormSl>();
            //        candidateFormSlTT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];


            //        if (candidateFormSlTT.Count > 0 && candidatePaymentTT.Amount > 0)
            //        {
            //            PurchaseApplicationForm();
            //        }
            //        else
            //        {
            //            btnSubmit.Visible = false;
            //            MessageView("You are not Eligible to Apply..!!", "fail");
            //        }

            //    }
            //    #region N/A
            //    //else if (sscExamType == 6 && hscExamType == 8)    // Dakhil/Alim
            //    //{
            //    //    PurchaseApplicationForm();
            //    //}
            //    //else if (sscExamType == 1 && hscExamType == 8)    // SSC / Alim
            //    //{
            //    //    PurchaseApplicationForm();
            //    //}
            //    //else if (sscExamType == 6 && hscExamType == 2)    //Dakhil / HSC
            //    //{
            //    //    PurchaseApplicationForm();
            //    //}

            //    //else
            //    //{
            //    //    btnSubmit.Visible = false;
            //    //    EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
            //    //} 
            //    #endregion
            //}
            //else
            //{
            //    PurchaseApplicationForm();
            //} 
            #endregion

        }

        protected void btnShowPopupClicked(object sender, EventArgs e)
        {
            ModalPopupExtender.Show();
        }

        #region N/A

        //protected void btnCalculateOLevel_Click(object sender, EventArgs e)
        //{
        //    decimal subject1 = Convert.ToDecimal(ddlOLevelSubject1.SelectedValue);
        //    decimal subject2 = Convert.ToDecimal(ddlOLevelSubject2.SelectedValue);
        //    decimal subject3 = Convert.ToDecimal(ddlOLevelSubject3.SelectedValue);
        //    decimal subject4 = Convert.ToDecimal(ddlOLevelSubject4.SelectedValue);
        //    decimal subject5 = Convert.ToDecimal(ddlOLevelSubject5.SelectedValue);

        //    decimal result = ((subject1 + subject2 + subject3 + subject4 + subject5) / 5);

        //    lblOLevelResult.Text = result.ToString();

        //    ModalPopupExtender.Show();
        //}


        //protected void btnCalculateALevel_Click(object sender, EventArgs e)
        //{
        //    decimal subject1 = Convert.ToDecimal(ddlALevelSubject1.SelectedValue);
        //    decimal subject2 = Convert.ToDecimal(ddlALevelSubject2.SelectedValue);

        //    decimal result = ((subject1 + subject2) / 2);

        //    lblALevelResult.Text = result.ToString();

        //    ModalPopupExtender.Show();
        //}

        //protected void btnCalculateOAndALevel_Click(object sender, EventArgs e)
        //{
        //    decimal result = 0.00M;
        //    decimal result2 = 0.00M;
        //    decimal totalPoint = 0.00M;
        //    decimal totalPointToCheck = 26.5M;
        //    lblOLevelResult.Text = "";
        //    lblALevelResult.Text = "";
        //    lblMassage.Text = "";

        //    decimal subject1 = Convert.ToDecimal(ddlOLevelSubject1.SelectedValue);
        //    decimal subject2 = Convert.ToDecimal(ddlOLevelSubject2.SelectedValue);
        //    decimal subject3 = Convert.ToDecimal(ddlOLevelSubject3.SelectedValue);
        //    decimal subject4 = Convert.ToDecimal(ddlOLevelSubject4.SelectedValue);
        //    decimal subject5 = Convert.ToDecimal(ddlOLevelSubject5.SelectedValue);

        //    result = ((subject1 + subject2 + subject3 + subject4 + subject5) / 5);

        //    lblOLevelResult.Text = result.ToString();


        //    decimal subject6 = Convert.ToDecimal(ddlALevelSubject1.SelectedValue);
        //    decimal subject7 = Convert.ToDecimal(ddlALevelSubject2.SelectedValue);

        //    result2 = ((subject6 + subject7) / 2);

        //    lblALevelResult.Text = result2.ToString();

        //    totalPoint = subject1 + subject2 + subject3 + subject4 + subject5 + subject6 + subject7;

        //    if (totalPoint >= totalPointToCheck)
        //    {
        //        lblTotalPoints.Text = totalPoint.ToString();
        //    }
        //    else
        //    {
        //        lblMassage.Text = "You are not eligible for apply. You must have minimum 26.5 points in total subjects";
        //    }


        //    ModalPopupExtender.Show();
        //}

        #endregion

        protected void btnCalculateOAndALevel_Click(object sender, EventArgs e)
        {


            decimal result = 0.00M;
            decimal result2 = 0.00M;
            decimal totalPoint = 0.00M;
            decimal totalPointToCheck = 26.5M;
            lblOLevelResult.Text = "";
            lblALevelResult.Text = "";
            lblMassage.Text = "";

            decimal subject1 = Decimal.Parse(ddlOLevelSubject1.SelectedValue.ToString());
            decimal subject2 = Decimal.Parse(ddlOLevelSubject2.SelectedValue.ToString());
            decimal subject3 = Decimal.Parse(ddlOLevelSubject3.SelectedValue.ToString());
            decimal subject4 = Decimal.Parse(ddlOLevelSubject4.SelectedValue.ToString());
            decimal subject5 = Decimal.Parse(ddlOLevelSubject5.SelectedValue.ToString());

            result = (subject1 + subject2 + subject3 + subject4 + subject5);

            lblOLevelResult.Text = result.ToString();
            hfOLevelConvertedSscGPA.Value = (result / 5).ToString();
            txtGPASSC.Text = (result / 5).ToString(); //result.ToString();

            decimal subject6 = Decimal.Parse(ddlALevelSubject1.SelectedValue.ToString());
            decimal subject7 = Decimal.Parse(ddlALevelSubject2.SelectedValue.ToString());

            result2 = (subject6 + subject7);


            lblALevelResult.Text = result2.ToString();
            hfALevelConvertedHscGPA.Value = (result2 / 2).ToString();
            txtGPAHSC.Text = (result2 / 2).ToString(); //lblALevelResult.Text;

            totalPoint = subject1 + subject2 + subject3 + subject4 + subject5 + subject6 + subject7;
            decimal[] point = { subject1, subject2, subject3, subject4, subject5, subject6, subject7 };

            int Ccount = 0;
            int count = 0;



            if (totalPoint >= totalPointToCheck)
            {
                lblTotalPoints.Text = totalPoint.ToString();

                #region N/A
                //for (int i = 0; i < point.Length; i++)
                //{
                //    if (point[i] == 3.50M)
                //    {
                //        Ccount++;
                //    }

                //    if (point[i] < 3.50M)
                //    {
                //        count++;
                //    }

                //}
                //if (Ccount <= 3 && count < 1)
                //{
                //    lblTotalPoints.Text = totalPoint.ToString();

                //}
                //else
                //{
                //    lblTotalPoints.Text = totalPoint.ToString();

                //    HiddenField1.Value = "1";

                //    lblMassage.Text = "You are not eligible to apply. Requirement is minimum B Grade in 04(four) Subjects and minimum C Grade in 03(three) subjects";
                //} 
                #endregion

            }
            else
            {
                lblTotalPoints.Text = totalPoint.ToString();

                HiddenField1.Value = "1";

                lblMassage.Text = "You are not eligible to apply. Minimum requirement is 26.5 points";
            }


            ModalPopupExtender.Show();
        }


        protected void btnCancleModel_Click(object sender, EventArgs e)
        {


            if (HiddenField1.Value == "1")
            {
                btnSubmit.Visible = false;
                MessageView("You are not Eligible to Apply..!!", "fail");
                ModalPopupExtender.Hide();
            }
            else
            {
                //txtGPASSC.Text = lblOLevelResult.Text;
                txtGPASSC.Enabled = false;

                //txtGPAHSC.Text = lblALevelResult.Text;
                txtGPAHSC.Enabled = false;

                ddlGroupSSC.SelectedValue = "2";
                ddlGroupHSC.SelectedValue = "2";
                ddlGroupSSC.Enabled = false;
                ddlGroupHSC.Enabled = false;

                ModalPopupExtender.Hide();
            }

        }


        protected void myListDropDown_Change(object sender, EventArgs e)
        {
            int temp = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
            if (temp == 5) // for O-Level
            {
                //Button1.Visible = true;
                ddlExamTypeHSC.SelectedValue = "7";
                ddlGroupSSCComV.Visible = false;
                RegularExpressionValidator2.Visible = false;
                ddlGroupHSCComV.Visible = false;
                YourRegularExpressionValidator.Visible = false;
                ModalPopupExtender.Show();
            }
            else
            {
                ddlExamTypeHSC.SelectedValue = "-1";
                ddlGroupSSCComV.Visible = true;
                RegularExpressionValidator2.Visible = true;
                ddlGroupHSCComV.Visible = true;
                YourRegularExpressionValidator.Visible = true;
                txtGPASSC.Text = "";
                txtGPAHSC.Text = "";
                txtGPASSC.Enabled = true;
                txtGPAHSC.Enabled = true;
                ddlGroupSSC.Enabled = true;
                ddlGroupHSC.Enabled = true;
            }
        }

        //protected void TextBox1_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DateTime dateValue = new DateTime();
        //        DateTime dobT = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
        //        if (DateTime.TryParse(dobT.Date.ToString(), out dateValue))
        //        {
        //            txtDateOfBirthValidateMassage.InnerText = "";

        //            //assign value to another text box here.
        //            DateTime dob = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
        //            var date = dob.Day;
        //            var month = dob.Month;
        //            var year = dob.Year;
        //            //var diff2 = DateTime.Now.AddYears(-14);
        //            var diff = DateTime.Now - dob; //DateTime.Now.AddYears(-14);



        //            if (diff.Days < 5475)
        //            {
        //                txtDateOfBirthValidateMassage.InnerText = "Sorry, You are not eligible to apply. Minimum age 15 years !!";
        //                txtDateOfBirth.Text = string.Empty;
        //            }
        //            else
        //            {
        //                txtDateOfBirthValidateMassage.InnerText = "";
        //            }
        //        }
        //        else
        //        {
        //            txtDateOfBirthValidateMassage.InnerText = "Sorry, input formate is not correct !!";
        //            txtDateOfBirth.Text = string.Empty;
        //            txtDateOfBirth.Focus();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        txtDateOfBirthValidateMassage.InnerText = "Sorry, input formate is not correct !!";
        //        txtDateOfBirth.Text = string.Empty;
        //        txtDateOfBirth.Focus();
        //    }


        //}

        protected void CheckValidationForPurchaseApplication()
        {

        }

        protected void ddlBirthDate_SelectedIndexChanged(object sender, EventArgs e)
        {

            string ddlDayId = ddlDay.SelectedValue;
            string ddlMonthId = ddlMonth.SelectedValue;
            string ddlYearId = ddlYear.SelectedValue;

            if (Convert.ToInt32(ddlDayId) > 0 && Convert.ToInt32(ddlMonthId) > 0 && Convert.ToInt32(ddlYearId) > 0)
            {
                try
                {
                    string stringDateOfBirth = DateFormateing();


                    DateTime dateValue = new DateTime();
                    DateTime dobT = DateTime.ParseExact(stringDateOfBirth.ToString(), "dd/MM/yyyy", null);
                    if (DateTime.TryParse(dobT.Date.ToString(), out dateValue))
                    {
                        txtDateOfBirthValidateMassage.InnerText = "";

                        //assign value to another text box here.
                        DateTime dob = DateTime.ParseExact(stringDateOfBirth.ToString(), "dd/MM/yyyy", null);
                        var date = dob.Day;
                        var month = dob.Month;
                        var year = dob.Year;
                        //var diff2 = DateTime.Now.AddYears(-14);
                        var diff = DateTime.Now - dob; //DateTime.Now.AddYears(-14);



                        if (diff.Days < 5475)
                        {
                            txtDateOfBirthValidateMassage.InnerText = "Sorry, You are not eligible to apply. Minimum age 15 years !!";
                            ddlDay.SelectedValue = "-1";
                            ddlMonth.SelectedValue = "-1";
                            ddlYear.SelectedValue = "-1";
                        }
                        else
                        {
                            txtDateOfBirthValidateMassage.InnerText = "";
                        }
                    }
                    else
                    {
                        txtDateOfBirthValidateMassage.InnerText = "Sorry, input formate is not correct !!";
                        ddlDay.SelectedValue = "-1";
                        ddlMonth.SelectedValue = "-1";
                        ddlYear.SelectedValue = "-1";
                    }
                }
                catch (Exception ex)
                {
                    txtDateOfBirthValidateMassage.InnerText = "Sorry, input formate is not correct !!";
                    ddlDay.SelectedValue = "-1";
                    ddlMonth.SelectedValue = "-1";
                    ddlYear.SelectedValue = "-1";
                }
            }


        }


        protected void btnAdminAssignTestValue_Click(object sender, EventArgs e)
        {
            txtName.Text = "Test Name";
            //txtDateOfBirth.Text = "01/01/1995";

            ddlDay.SelectedValue = "01";
            ddlMonth.SelectedValue = "01";
            ddlYear.SelectedValue = "1995";

            txtEmail.Text = "ariqrahman.office@gmail.com";

            txtSmsMobile.Text = "01676675257";
            txtGuardianMobile.Text = "01676675257";

            txtNationalIdOrBirthRegistration.Text = "123456";

            ddlGender.SelectedValue = "2";
            //ddlQuota.SelectedValue = "3";

            txtInstituteSSC.Text = "XXX Institute";
            ddlBoardSSC.SelectedValue = "2";
            ddlExamTypeSSC.SelectedValue = "1";
            ddlGroupSSC.SelectedValue = "3";
            //ddlSec_DivClass.SelectedValue = "5";
            txtGPASSC.Text = "5";
            ddlPassYearSSC.SelectedValue = "2017";


            txtInstituteHSC.Text = "YYY Institute";
            ddlBoardHSC.SelectedValue = "2";
            ddlExamTypeHSC.SelectedValue = "2";
            ddlGroupHSC.SelectedValue = "3";
            //ddlHigherSec_DivClass.SelectedValue = "5";
            txtGPAHSC.Text = "5";
            ddlPassYearHSC.SelectedValue = "2019";




        }



    }
}