using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using DocumentFormat.OpenXml.Drawing.Charts;
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
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class PurchaseFormBachelor : System.Web.UI.Page
    {
        /// <summary>
        /// =========================================
        /// Tetitalk Api Intigation Application Form
        /// =========================================
        /// </summary>

        string SessionLoginCaptcha = "SessionLoginCaptcha";
        int oLevelExamType = -1;
        int aLevelExamType = -1;
        private object JsonConvert;

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

                    if (admissionSetup.EducationCategoryID == 4 || admissionSetup.EducationCategoryID == 6)
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
                Session["SSC_TelitalkEducationResult"] = null;
                Session["HSC_TelitalkEducationResult"] = null;

                Session["SSC_TelitalkEducationResult_JSON"] = null;
                Session["HSC_TelitalkEducationResult_JSON"] = null;


                Session["Eligible_NotEligible_List"] = null;

                LoadCaptcha();
                LoadDDL();

                btnSSCHSC_Click(null, null);

                #region N/A
                //txtName.Text = String.Empty;
                //txtDateOfBirth.Text = String.Empty;
                //txtEmail.Text = String.Empty;
                //txtSmsMobile.Text = String.Empty;
                //txtGuardianMobile.Text = String.Empty;
                //ddlGender.SelectedIndex = -1;
                //ddlQuota.SelectedIndex = -1;
                //ddlPassingYear.SelectedIndex = -1;
                //ddlExamTypeSSC.SelectedIndex = -1;
                //ddlGroupSSC.SelectedIndex = -1;
                //txtGPASSC.Text = String.Empty;
                ////ddlPassYearSSC.SelectedIndex = -1;
                //ddlExamTypeHSC.SelectedIndex = -1;
                //ddlGroupHSC.SelectedIndex = -1;
                //txtGPAHSC.Text = String.Empty;
                //ddlPassYearHSC.SelectedIndex = -1; 
                #endregion
            }

            //TeletalkEducationBoard tebModel = new TeletalkEducationBoard();
            //string tebResult = TeletalkEducationBoard.GetData("a", "b", "c", "d");

        }


        protected void btnAdminAssignTestValue_Click(object sender, EventArgs e)
        {
            txtName.Text = "Test Name";
            //txtDateOfBirth.Text = "01/01/1995";
            ddlDay.SelectedValue = "-1";
            ddlMonth.SelectedValue = "-1";
            ddlYear.SelectedValue = "-1";
            txtEmail.Text = "a@gmail.com";
            txtSmsMobile.Text = "01723143412";
            txtGuardianMobile.Text = "01723143412";
            ddlGender.SelectedValue = "2";
            //ddlQuota.SelectedValue = "3";
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
                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                //DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(x => x.CreatedBy).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);


                List<DAL.EducationBoard> educationBoardList = db.GetAllEducationBoard_ND();
                if (educationBoardList != null && educationBoardList.Any())
                {
                    DDLHelper.Bind<DAL.EducationBoard>(ddlOLevelEducationBoard, educationBoardList.Where(a => a.IsActive == true && (a.ID == 11 || a.ID == 12 || a.ID == 13)).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                    DDLHelper.Bind<DAL.EducationBoard>(ddlALevelEducationBoard, educationBoardList.Where(a => a.IsActive == true && (a.ID == 11 || a.ID == 12 || a.ID == 13)).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);

                    DDLHelper.Bind<DAL.EducationBoard>(ddlBoardSSC, educationBoardList.Where(a => a.IsActive == true && a.IsVisual == true).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                    DDLHelper.Bind<DAL.EducationBoard>(ddlBoardHSC, educationBoardList.Where(a => a.IsActive == true && a.IsVisual == true).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                    //DDLHelper.Bind<DAL.EducationBoard>(ddlIBEducationBoard, educationBoardList.Where(a => a.IsActive == true && a.IsVisual == true).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);

                    DDLHelper.Bind<DAL.EducationBoard>(ddlOLevelAppearedEducationBoard, educationBoardList.Where(a => a.IsActive == true && (a.ID == 11 || a.ID == 12 || a.ID == 13)).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                    DDLHelper.Bind<DAL.EducationBoard>(ddlALevelAppearedEducationBoard, educationBoardList.Where(a => a.IsActive == true && (a.ID == 11 || a.ID == 12 || a.ID == 13)).OrderBy(x => x.SerialNo).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);

                    if (ddlALevelAppearedEducationBoard.Items.Count > 0)
                        ddlALevelAppearedEducationBoard.SelectedValue = "11";

                }
            }

            //-------- SSC Pass Year----------------------
            //ddlPassYearSSC.Items.Clear();
            //ddlPassYearSSC.Items.Add(new ListItem("--Select Year--", "-1"));
            //ddlPassYearSSC.AppendDataBoundItems = true;
            //for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 6; i--)
            //{
            //    ddlPassYearSSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}

            //-------- HSC Pass Year----------------------
            //ddlPassYearHSC.Items.Clear();
            //ddlPassYearHSC.Items.Add(new ListItem("--Select Year--", "-1"));
            //ddlPassYearHSC.AppendDataBoundItems = true;
            //for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 2; i--)
            //{
            //    ddlPassYearHSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}



            //-------- O-Level Pass Year----------------------
            ddlPassYearOLevel.Items.Clear();
            ddlPassYearOLevel.Items.Add(new ListItem("--Select Year--", "-1"));
            ddlPassYearOLevel.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 6; i--)
            {
                ddlPassYearOLevel.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            //-------- A-Level Pass Year----------------------
            ddlPassYearALevel.Items.Clear();
            ddlPassYearALevel.Items.Add(new ListItem("--Select Year--", "-1"));
            ddlPassYearALevel.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 4; i--)
            {
                ddlPassYearALevel.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }


            //-------- O-Level Pass Year----------------------
            ddlPassYearOLevelAppeared.Items.Clear();
            ddlPassYearOLevelAppeared.Items.Add(new ListItem("--Select Year--", "-1"));
            ddlPassYearOLevelAppeared.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 6; i--)
            {
                ddlPassYearOLevelAppeared.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            //-------- A-Level Pass Year----------------------
            ddlPassYearALevelAppeared.Items.Clear();
            ddlPassYearALevelAppeared.Items.Add(new ListItem("--Select Year--", "-1"));
            ddlPassYearALevelAppeared.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 4; i--)
            {
                ddlPassYearALevelAppeared.Items.Add(new ListItem(i.ToString(), i.ToString()));


                //if (i == 2023)
                //{
                //    ddlPassYearALevelAppeared.Items.Add(new ListItem(i.ToString(), i.ToString()));

                //    ddlPassYearALevelAppeared.SelectedValue = "2023";
                //}
            }
            ddlPassYearALevelAppeared.SelectedValue = DateTime.Now.Year.ToString();

            //-------- International Baccalaureate Pass Year----------------------
            ddlIBPassingYear.Items.Clear();
            ddlIBPassingYear.Items.Add(new ListItem("--Select Year--", "-1"));
            ddlIBPassingYear.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 6; i--)
            {
                ddlIBPassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }


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

        protected void ClearSSCHSCSection()
        {
            //ddlExamTypeSSC.SelectedValue = "-1";
            //ddlPassYearSSC.SelectedValue = "-1";
            //ddlBoardSSC.SelectedValue = "-1";
            //txtRollSSC.Text = string.Empty;

            //----------------------------------

            ddlExamTypeHSC.SelectedValue = "-1";
            ddlPassYearHSC.SelectedValue = "-1";
            ddlBoardHSC.SelectedValue = "-1";
            ddlBoardSSC.SelectedValue = "-1";
            txtRollHSC.Text = string.Empty;
        }

        protected void ClearBasicInfoSection()
        {
            txtName.Text = string.Empty;
            txtName.Enabled = true;

            ddlDay.SelectedValue = "-1";
            ddlDay.Enabled = true;

            ddlMonth.SelectedValue = "-1";
            ddlMonth.Enabled = true;

            ddlYear.SelectedValue = "-1";
            ddlYear.Enabled = true;

            txtEmail.Text = string.Empty;
            txtSmsMobile.Text = string.Empty;
            txtGuardianMobile.Text = string.Empty;

            ddlGender.SelectedValue = "-1";
            ddlGender.Enabled = true;

            //ddlQuota.SelectedValue = "-1";

        }

        #region N/A -- DateOfBirth
        //protected void DateOfBirth_TextChanged(object sender, EventArgs e)
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
        #endregion

        protected void rblEducationChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            int educationChoiceId = Convert.ToInt32(rblEducationChoice.SelectedValue);

            MessageView("", "clear");

            if (educationChoiceId == 1)
            {
                //....SSC, SSC (Vocational), HSC, HSC (Vocational), Dakhil, Alim
                panelSSCHSCInputSection.Visible = true;

                //....Hide Modal for O-Level, A-Level
                panelOALevelInputSection.Visible = false;

                //....Hide Basic Info
                ClearBasicInfoSection();
                panelBasicInfo.Visible = false;

                //....Hide O/A-Level Input Section
                panelOALevelInputSection.Visible = false;

                //....Hide International Baccalaureate Input Section
                panelIBInputSection.Visible = false;

                //....Hide Button Section
                panelSubmitButtonSection.Visible = false;

                //....Hide O/A-Level Appeared Input Section
                panelOALevelAppearedInputSection.Visible = false;

            }
            else if (educationChoiceId == 2)
            {
                //....SSC, SSC (Vocational), HSC, HSC (Vocational), Dakhil, Alim
                panelSSCHSCInputSection.Visible = false;

                //....Clear SSC HSC Section
                //ClearSSCHSCSection();

                //....O-Level, A-Level
                panelOALevelInputSection.Visible = true;

                //....Show Basic Info
                ClearBasicInfoSection();
                panelBasicInfo.Visible = true;

                //....Hide O/A-Level Input Section
                panelOALevelInputSection.Visible = true;

                //....Hide International Baccalaureate Input Section
                panelIBInputSection.Visible = false;

                //....Hide Button Section
                panelSubmitButtonSection.Visible = false;

                //....Hide O/A-Level Appeared Input Section
                panelOALevelAppearedInputSection.Visible = false;
            }
            else if (educationChoiceId == 3)
            {
                //....SSC, SSC (Vocational), HSC, HSC (Vocational), Dakhil, Alim
                panelSSCHSCInputSection.Visible = false;

                //....Clear SSC HSC Section
                //ClearSSCHSCSection();

                //....O-Level, A-Level
                panelOALevelInputSection.Visible = false;

                //....Show Basic Info
                ClearBasicInfoSection();
                panelBasicInfo.Visible = true;

                //....Hide O/A-Level Input Section
                panelOALevelInputSection.Visible = false;

                //....Show International Baccalaureate Input Section
                panelIBInputSection.Visible = true;

                //....Hide Button Section
                panelSubmitButtonSection.Visible = false;

                //....Hide O/A-Level Appeared Input Section
                panelOALevelAppearedInputSection.Visible = false;
            }
            else if (educationChoiceId == 4)
            {
                //....SSC, SSC (Vocational), HSC, HSC (Vocational), Dakhil, Alim
                panelSSCHSCInputSection.Visible = false;

                //....O-Level, A-Level
                panelOALevelInputSection.Visible = false;

                //....Show Basic Info
                ClearBasicInfoSection();
                panelBasicInfo.Visible = true;

                //....Hide O/A-Level Input Section
                panelOALevelInputSection.Visible = false;

                //....Hide International Baccalaureate Input Section
                panelIBInputSection.Visible = false;

                //....Hide Button Section
                panelSubmitButtonSection.Visible = false;

                //....Hide O/A-Level Appeared Input Section
                panelOALevelAppearedInputSection.Visible = true;
            }
        }

        private bool IsNumeric(string s)
        {
            Regex r = new Regex(@"^\d{0,2}(\.\d{1,2})?$");

            return r.IsMatch(s);
        }

        protected void btnCalculateOAndALevel_Click(object sender, EventArgs e)
        {
            try
            {
                decimal resultOLevel = 0.00M;
                decimal resultALevel = 0.00M;
                decimal totalPoint = 0.00M;

                decimal totalPointToCheck = 26.5M;

                lblOLevelResult.Text = string.Empty;
                lblALevelResult.Text = string.Empty;

                lblMassageOALevel.Text = string.Empty;
                messagePanelOALevel.Visible = false;


                //-------------------------- O-Level -------------------------------
                decimal subject1 = Decimal.Parse(ddlOLevelSubject1.SelectedValue.ToString());
                decimal subject2 = Decimal.Parse(ddlOLevelSubject2.SelectedValue.ToString());
                decimal subject3 = Decimal.Parse(ddlOLevelSubject3.SelectedValue.ToString());
                decimal subject4 = Decimal.Parse(ddlOLevelSubject4.SelectedValue.ToString());
                decimal subject5 = Decimal.Parse(ddlOLevelSubject5.SelectedValue.ToString());

                resultOLevel = (subject1 + subject2 + subject3 + subject4 + subject5);

                lblOLevelResult.Text = resultOLevel.ToString();
                hfOLevelConvertedSscGPA.Value = (resultOLevel / 5).ToString();


                //-------------------------- A-Level -------------------------------
                decimal subject6 = Decimal.Parse(ddlALevelSubject1.SelectedValue.ToString());
                decimal subject7 = Decimal.Parse(ddlALevelSubject2.SelectedValue.ToString());

                resultALevel = (subject6 + subject7);

                lblALevelResult.Text = resultALevel.ToString();
                hfALevelConvertedHscGPA.Value = (resultALevel / 2).ToString();


                //-------------------------- Total Point To Check -------------------------------
                totalPoint = subject1 + subject2 + subject3 + subject4 + subject5 + subject6 + subject7;
                decimal[] point = { subject1, subject2, subject3, subject4, subject5, subject6, subject7 };

                if (totalPoint >= totalPointToCheck)
                {
                    lblTotalPoints.Text = totalPoint.ToString();

                    //....Show Button Section
                    panelSubmitButtonSection.Visible = true;
                }
                else
                {
                    //....Hide Button Section
                    panelSubmitButtonSection.Visible = false;

                    lblTotalPoints.Text = totalPoint.ToString();

                    lblMassageOALevel.Text = "You are not Eligible to apply. Minimum requirement is 26.5 points";
                    lblMassageOALevel.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMassageOALevel.Attributes.CssStyle.Add("color", "crimson");

                    messagePanelOALevel.Visible = true;
                    messagePanelOALevel.CssClass = "alert alert-danger";

                    btnSubmit.Visible = false;
                    MessageView("You are not Eligible to Apply..!!", "fail");
                }

            }
            catch (Exception ex)
            {
                lblMassageOALevel.Text = "Message: " + ex.Message.ToString();
                lblMassageOALevel.Attributes.CssStyle.Add("font-weight", "bold");
                lblMassageOALevel.Attributes.CssStyle.Add("color", "crimson");

                messagePanelOALevel.Visible = true;
                messagePanelOALevel.CssClass = "alert alert-danger";

                btnSubmit.Visible = false;
            }
        }

        protected MessageModel CheckInputFieldValidationSSCHSC()
        {
            MessageModel mModel = new MessageModel();
            //bool result = true;

            Dictionary<int, string> messageList = new Dictionary<int, string>();

            int i = 1;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                messageList.Add(i++, "Full Name is Empty");
            }

            //if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            //{
            //    messageList.Add(i++, "Date Of Birth is Empty");
            //}

            if (ddlDay.SelectedValue == "-1" || ddlMonth.SelectedValue == "-1" || ddlYear.SelectedValue == "-1")
            {
                messageList.Add(i++, "Date of Birth hasn't Selected !!");
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                messageList.Add(i++, "Email is Empty");
            }

            if (string.IsNullOrEmpty(txtSmsMobile.Text))
            {
                messageList.Add(i++, "Mobile is Empty");
            }

            if (string.IsNullOrEmpty(txtGuardianMobile.Text))
            {
                messageList.Add(i++, "Guardian Mobile is Empty");
            }

            if (ddlGender.SelectedValue == "-1")
            {
                messageList.Add(i++, "Gender is not Selected");
            }

            //if (ddlQuota.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "Quota is not Selected");
            //}


            ////----------------SSC------------------
            //if (ddlExamTypeSSC.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "SSC Examination is not Selected");
            //}
            //if (ddlPassYearSSC.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "SSC Passing Year is not Selected");
            //}
            //if (ddlBoardSSC.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "SSC Board is not Selected");
            //}
            //if (string.IsNullOrEmpty(txtRollSSC.Text))
            //{
            //    messageList.Add(i++, "SSC Roll is Empty");
            //}

            ////----------------HSC------------------

            //if (ddlExamTypeHSC.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "HSC Examination is not Selected");
            //}
            //if (ddlPassYearHSC.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "HSC Passing Year is not Selected");
            //}
            //if (ddlBoardHSC.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "HSC Board is not Selected");
            //}
            //if (string.IsNullOrEmpty(txtRollHSC.Text))
            //{
            //    messageList.Add(i++, "HSC Roll is Empty");
            //}




            if (messageList != null && messageList.Count > 0)
            {
                string message = "";
                foreach (var msg in messageList)
                {
                    message = message + msg.Key.ToString() + ". " + msg.Value + "<br/>";
                }

                //MessageView(message, "fail");
                //result = false;

                mModel.MessageCode = 2;
                mModel.MessageBody = message;
                mModel.MessageBoolean = false;
            }
            else
            {
                mModel.MessageCode = 1;
                mModel.MessageBody = "";
                mModel.MessageBoolean = true;
            }


            return mModel;

        }

        protected MessageModel CheckInputFieldValidationOALevel()
        {
            MessageModel mModel = new MessageModel();

            //bool result = true;

            Dictionary<int, string> messageList = new Dictionary<int, string>();

            int i = 1;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                messageList.Add(i++, "Full Name is Empty");
            }

            //if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            //{
            //    messageList.Add(i++, "Date Of Birth is Empty");
            //}

            if (ddlDay.SelectedValue == "-1" || ddlMonth.SelectedValue == "-1" || ddlYear.SelectedValue == "-1")
            {
                messageList.Add(i++, "Date of Birth hasn't Selected !!");
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                messageList.Add(i++, "Email is Empty");
            }

            if (string.IsNullOrEmpty(txtSmsMobile.Text))
            {
                messageList.Add(i++, "Mobile is Empty");
            }

            if (string.IsNullOrEmpty(txtGuardianMobile.Text))
            {
                messageList.Add(i++, "Guardian Mobile is Empty");
            }

            if (ddlGender.SelectedValue == "-1")
            {
                messageList.Add(i++, "Gender is not Selected");
            }

            //if (ddlQuota.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "Quota is not Selected");
            //}


            //----------------O-Level------------------
            if (string.IsNullOrEmpty(txtOLevelInstitute.Text))
            {
                messageList.Add(i++, "O-Level Institute is Empty");
            }
            if (ddlOLevelEducationBoard.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Education Board is not Selected");
            }
            if (ddlPassYearOLevel.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Passing Year is not Selected");
            }
            if (ddlOLevelSubject1.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject1 is not Selected");
            }
            if (ddlOLevelSubject2.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject2 is not Selected");
            }
            if (ddlOLevelSubject3.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject3 is not Selected");
            }
            if (ddlOLevelSubject4.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject4 is not Selected");
            }
            if (ddlOLevelSubject5.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject is not Selected");
            }

            //----------------A-Level------------------
            if (string.IsNullOrEmpty(txtALevelInstitute.Text))
            {
                messageList.Add(i++, "A-Level Institute is Empty");
            }
            if (ddlALevelEducationBoard.SelectedValue == "-1")
            {
                messageList.Add(i++, "A-Level Education Board is not Selected");
            }
            if (ddlPassYearALevel.SelectedValue == "-1")
            {
                messageList.Add(i++, "A-Level Passing Year is not Selected");
            }
            if (ddlALevelSubject1.SelectedValue == "-1")
            {
                messageList.Add(i++, "A-Level Subject1 is not Selected");
            }
            if (ddlALevelSubject2.SelectedValue == "-1")
            {
                messageList.Add(i++, "A-Level Subject2 is not Selected");
            }




            if (messageList != null && messageList.Count > 0)
            {
                string message = "";
                foreach (var msg in messageList)
                {
                    message = message + msg.Key.ToString() + ". " + msg.Value + "<br/>";
                }

                //MessageView(message, "fail");
                //result = false;

                mModel.MessageCode = 2;
                mModel.MessageBody = message;
                mModel.MessageBoolean = false;
            }
            else
            {
                mModel.MessageCode = 1;
                mModel.MessageBody = "";
                mModel.MessageBoolean = true;
            }


            return mModel;

        }

        protected MessageModel CheckInputFieldValidationIB()
        {
            MessageModel mModel = new MessageModel();

            //bool result = true;

            Dictionary<int, string> messageList = new Dictionary<int, string>();

            int i = 1;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                messageList.Add(i++, "Full Name is Empty");
            }

            //if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            //{
            //    messageList.Add(i++, "Date Of Birth is Empty");
            //}

            if (ddlDay.SelectedValue == "-1" || ddlMonth.SelectedValue == "-1" || ddlYear.SelectedValue == "-1")
            {
                messageList.Add(i++, "Date of Birth hasn't Selected !!");
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                messageList.Add(i++, "Email is Empty");
            }

            if (string.IsNullOrEmpty(txtSmsMobile.Text))
            {
                messageList.Add(i++, "Mobile is Empty");
            }

            if (string.IsNullOrEmpty(txtGuardianMobile.Text))
            {
                messageList.Add(i++, "Guardian Mobile is Empty");
            }

            if (ddlGender.SelectedValue == "-1")
            {
                messageList.Add(i++, "Gender is not Selected");
            }


            //----------------IB------------------
            if (string.IsNullOrEmpty(txtIBInstitute.Text))
            {
                messageList.Add(i++, "IB Institute is Empty");
            }
            //if (ddlIBEducationBoard.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "IB Education Board is not Selected");
            //}
            if (ddlIBPassingYear.SelectedValue == "-1")
            {
                messageList.Add(i++, "IB Passing Year is not Selected");
            }
            if (ddlIBSubject1.SelectedValue == "-1")
            {
                messageList.Add(i++, "IB Subject-1 is not Selected");
            }
            if (ddlIBSubject2.SelectedValue == "-1")
            {
                messageList.Add(i++, "IB Subject-2 is not Selected");
            }
            if (ddlIBSubject3.SelectedValue == "-1")
            {
                messageList.Add(i++, "IB Subject-3 is not Selected");
            }
            if (ddlIBSubject4.SelectedValue == "-1")
            {
                messageList.Add(i++, "IB Subject-4 is not Selected");
            }
            if (ddlIBSubject5.SelectedValue == "-1")
            {
                messageList.Add(i++, "IB Subject-5 is not Selected");
            }
            if (ddlIBSubject6.SelectedValue == "-1")
            {
                messageList.Add(i++, "IB Subject-6 is not Selected");
            }


            if (messageList != null && messageList.Count > 0)
            {
                string message = "";
                foreach (var msg in messageList)
                {
                    message = message + msg.Key.ToString() + ". " + msg.Value + "<br/>";
                }

                //MessageView(message, "fail");
                //result = false;

                mModel.MessageCode = 2;
                mModel.MessageBody = message;
                mModel.MessageBoolean = false;
            }
            else
            {
                mModel.MessageCode = 1;
                mModel.MessageBody = "";
                mModel.MessageBoolean = true;
            }


            return mModel;

        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
                {
                    LoadCaptcha();
                    captchaMsg.Visible = true;
                    return;
                }

                int educationChoiceId = Convert.ToInt32(rblEducationChoice.SelectedValue);

                if (educationChoiceId == 1)
                {
                    //MessageModel resultCheckInputFieldValidation = CheckInputFieldValidationSSCHSC();

                    //if (resultCheckInputFieldValidation != null && resultCheckInputFieldValidation.MessageBoolean == true)
                    //{
                    PurchaseApplicationForm();
                    //}
                    //else
                    //{
                    //    MessageView(resultCheckInputFieldValidation.MessageBody, "fail");
                    //}
                }
                else if (educationChoiceId == 2)
                {
                    #region O-Level, A-Level

                    MessageModel resultCheckInputFieldValidation = CheckInputFieldValidationOALevel();

                    if (resultCheckInputFieldValidation != null && resultCheckInputFieldValidation.MessageBoolean == true)
                    {
                        PurchaseApplicationForm();
                    }
                    else
                    {
                        MessageView(resultCheckInputFieldValidation.MessageBody, "fail");
                    }

                    #endregion
                }
                else if (educationChoiceId == 3)
                {
                    #region International Baccalaureate

                    MessageModel resultCheckInputFieldValidation = CheckInputFieldValidationIB();

                    if (resultCheckInputFieldValidation != null && resultCheckInputFieldValidation.MessageBoolean == true)
                    {
                        PurchaseApplicationForm();
                    }
                    else
                    {
                        MessageView(resultCheckInputFieldValidation.MessageBody, "fail");
                    }

                    #endregion
                }
                else if (educationChoiceId == 4)
                {
                    #region O-Level, A-Level (Appeared)

                    MessageModel resultCheckInputFieldValidation = CheckInputFieldValidationOALevelAppeared();

                    if (resultCheckInputFieldValidation != null && resultCheckInputFieldValidation.MessageBoolean == true)
                    {
                        PurchaseApplicationForm();
                    }
                    else
                    {
                        MessageView(resultCheckInputFieldValidation.MessageBody, "fail");
                    }

                    #endregion
                }
                else
                {
                    MessageView("Please Select Education.", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Message: " + ex.Message.ToString(), "fail");
            }

        }


        private void PurchaseApplicationForm()
        {
            hdnCandidateUserId.Value = "0";
            string stringDateOfBirth = DateFormateing();
            DateTime dob = DateTime.ParseExact(stringDateOfBirth.ToString(), "dd/MM/yyyy", null);
            int genderId = Int32.Parse(ddlGender.SelectedValue);
            int quotaId = -1; //Int32.Parse(ddlQuota.SelectedValue);

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

                int educationChoiceId = Convert.ToInt32(rblEducationChoice.SelectedValue);



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

                #region CandidateUser

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

                #endregion

                #region BasicInfo
                //---------------------------------------------------------------------------------
                //insert candidate basic info
                //---------------------------------------------------------------------------------
                DAL.BasicInfo candidate = new DAL.BasicInfo();

                candidate.FirstName = txtName.Text.ToUpper();


                candidate.DateOfBirth = DateTime.ParseExact(stringDateOfBirth.ToString(), "dd/MM/yyyy", null);


                //candidate.MiddleName = ;
                //candidate.LastName = ;
                candidate.Mobile = smsMobile.Trim().ToString(); //txtSmsMobile.Text.Trim();
                candidate.SMSPhone = smsMobile.Trim().ToString(); //txtSmsMobile.Text.Trim();
                candidate.Email = txtEmail.Text.Trim();

                candidate.CandidateUserID = candidateUserIdLong;
                candidate.IsActive = false;
                candidate.UniqueIdentifier = Guid.NewGuid();
                candidate.GenderID = Convert.ToInt32(ddlGender.SelectedValue);
                //candidate.QuotaID = quotaId; //Convert.ToInt32(ddlQuota.SelectedValue);
                candidate.CreatedBy = -99;
                candidate.DateCreated = DateTime.Now;
                candidate.GuardianPhone = guardianMobile.Trim().ToString(); //txtGuardianMobile.Text.Trim();

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


                #region -- N/A CandidatePayment/CandidateFormSerial 

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

                #region -- N/A Insert Candidate ExamDetails 
                //---------------------------------------------------------------------------------
                //insert Candidate ExamDetails
                //---------------------------------------------------------------------------------

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


                #region O-Level, A-Level
                if (educationChoiceId == 2)
                {
                    int oLevelExamTypeId = 5; //O-Level
                    int aLevelExamTypeId = 7; //A-Level

                    int oLevelGroupOrSubjectID = 2; //Other = 2
                    int aLevelGroupOrSubjectID = 2; //Other = 2

                    int oLevelResultDivisionID = 5; //GPA = 5
                    int aLevelResultDivisionID = 5; //GPA = 5


                    string oLevelInstitute = txtOLevelInstitute.Text;
                    string aLevelInstitute = txtALevelInstitute.Text;

                    int oLevelEducationBoardId = Convert.ToInt32(ddlOLevelEducationBoard.SelectedValue);
                    int aLevelEducationBoardId = Convert.ToInt32(ddlALevelEducationBoard.SelectedValue);

                    int oLevelPassYearID = Convert.ToInt32(ddlPassYearOLevel.SelectedValue);
                    int aLevelPassYearID = Convert.ToInt32(ddlPassYearALevel.SelectedValue);

                    #region O-Level

                    DAL.ExamDetail secondaryExamDetails = new DAL.ExamDetail();

                    secondaryExamDetails.EducationBoardID = oLevelEducationBoardId;
                    secondaryExamDetails.Institute = oLevelInstitute;
                    secondaryExamDetails.UndgradGradProgID = 1; // N/A
                    secondaryExamDetails.GroupOrSubjectID = oLevelGroupOrSubjectID;
                    secondaryExamDetails.ResultDivisionID = oLevelResultDivisionID;

                    secondaryExamDetails.GPA = Convert.ToDecimal(hfOLevelConvertedSscGPA.Value);
                    //secondaryExamDetails.Marks = Convert.ToDecimal(lblOLevelResult.Text);


                    secondaryExamDetails.PassingYear = oLevelPassYearID;
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

                    secondaryExam.ExamTypeID = oLevelExamTypeId;
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

                    #region A-Level

                    DAL.ExamDetail higherSecondaryExamDetails = new DAL.ExamDetail();

                    higherSecondaryExamDetails.EducationBoardID = aLevelEducationBoardId;
                    higherSecondaryExamDetails.Institute = aLevelInstitute;
                    higherSecondaryExamDetails.UndgradGradProgID = 1; // N/A
                    higherSecondaryExamDetails.GroupOrSubjectID = aLevelGroupOrSubjectID;
                    higherSecondaryExamDetails.ResultDivisionID = aLevelResultDivisionID;

                    higherSecondaryExamDetails.GPA = Convert.ToDecimal(hfALevelConvertedHscGPA.Value);
                    //higherSecondaryExamDetails.Marks = Convert.ToDecimal(lblALevelResult.Text);

                    higherSecondaryExamDetails.PassingYear = aLevelPassYearID;
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
                    higherSecondaryExam.ExamTypeID = aLevelExamTypeId; //int.Parse(ddlHigherSec_ExamType.SelectedValue);
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

                #region International Baccalaureate
                if (educationChoiceId == 3)
                {
                    //int oLevelExamTypeId = 5; //O-Level
                    //int aLevelExamTypeId = 7; //A-Level

                    //int oLevelGroupOrSubjectID = 2; //Other = 2
                    //int aLevelGroupOrSubjectID = 2; //Other = 2

                    //int oLevelResultDivisionID = 5; //GPA = 5
                    //int aLevelResultDivisionID = 5; //GPA = 5


                    //string oLevelInstitute = txtOLevelInstitute.Text;
                    //string aLevelInstitute = txtALevelInstitute.Text;

                    //int oLevelEducationBoardId = Convert.ToInt32(ddlOLevelEducationBoard.SelectedValue);
                    //int aLevelEducationBoardId = Convert.ToInt32(ddlALevelEducationBoard.SelectedValue);

                    //int oLevelPassYearID = Convert.ToInt32(ddlPassYearOLevel.SelectedValue);
                    //int aLevelPassYearID = Convert.ToInt32(ddlPassYearALevel.SelectedValue);

                    #region IB (SSC Equivalent)

                    DAL.ExamDetail secondaryExamDetails = new DAL.ExamDetail();

                    secondaryExamDetails.EducationBoardID = 11; // Other
                    secondaryExamDetails.Attribute1 = txtIBEducationBoard.Text; // Education Board Name
                    secondaryExamDetails.Institute = txtIBInstitute.Text;
                    secondaryExamDetails.UndgradGradProgID = 1; // N/A
                    secondaryExamDetails.GroupOrSubjectID = 2; // Other
                    secondaryExamDetails.ResultDivisionID = 5; // GPA

                    //secondaryExamDetails.GPA = Convert.ToDecimal(hfOLevelConvertedSscGPA.Value);
                    //secondaryExamDetails.Marks = Convert.ToDecimal(lblOLevelResult.Text);


                    secondaryExamDetails.PassingYear = Convert.ToInt32(ddlIBPassingYear.SelectedValue);

                    secondaryExamDetails.CreatedBy = -99;
                    secondaryExamDetails.DateCreated = DateTime.Now;

                    long secondaryExamDetailsIDLong = -1;
                    using (var dbSecExmDet = new CandidateDataManager())
                    {
                        dbSecExmDet.Insert<DAL.ExamDetail>(secondaryExamDetails);
                        secondaryExamDetailsIDLong = secondaryExamDetails.ID;
                    }

                    ////-------------------------------

                    DAL.Exam secondaryExam = new DAL.Exam();

                    secondaryExam.ExamTypeID = 14; // IB (SSC Equivalent)
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



                    #region IB (HSC Equivalent)

                    DAL.ExamDetail higherSecondaryExamDetails = new DAL.ExamDetail();

                    higherSecondaryExamDetails.EducationBoardID = 11; // Other
                    higherSecondaryExamDetails.Attribute1 = txtIBEducationBoard.Text; // Education Board Name
                    higherSecondaryExamDetails.Institute = txtIBInstitute.Text;
                    higherSecondaryExamDetails.UndgradGradProgID = 1; // N/A
                    higherSecondaryExamDetails.GroupOrSubjectID = 2; // Other
                    higherSecondaryExamDetails.ResultDivisionID = 5; // GPA

                    //higherSecondaryExamDetails.GPA = Convert.ToDecimal(hfALevelConvertedHscGPA.Value);
                    //higherSecondaryExamDetails.Marks = Convert.ToDecimal(lblALevelResult.Text);

                    higherSecondaryExamDetails.PassingYear = Convert.ToInt32(ddlIBPassingYear.SelectedValue);
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
                    higherSecondaryExam.ExamTypeID = 15; // IB (SSC Equivalent)
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




                #region O-Level, A-Level (2023 Appeared)
                if (educationChoiceId == 4)
                {
                    int oLevelExamTypeId = 5; //O-Level
                    int aLevelExamTypeId = 7; //A-Level

                    int oLevelGroupOrSubjectID = 2; //Other = 2
                    int aLevelGroupOrSubjectID = 2; //Other = 2

                    int oLevelResultDivisionID = 5; //GPA = 5
                    int aLevelResultDivisionID = 5; //GPA = 5


                    string oLevelInstitute = txtOLevelAppearedInstitute.Text;
                    string aLevelInstitute = txtALevelAppearedInstitute.Text;

                    int oLevelEducationBoardId = Convert.ToInt32(ddlOLevelAppearedEducationBoard.SelectedValue);
                    int aLevelEducationBoardId = Convert.ToInt32(ddlALevelAppearedEducationBoard.SelectedValue);

                    int oLevelPassYearID = Convert.ToInt32(ddlPassYearOLevelAppeared.SelectedValue);
                    int aLevelPassYearID = Convert.ToInt32(ddlPassYearALevelAppeared.SelectedValue);

                    #region O-Level

                    DAL.ExamDetail secondaryExamDetails = new DAL.ExamDetail();

                    secondaryExamDetails.EducationBoardID = oLevelEducationBoardId;
                    secondaryExamDetails.Institute = oLevelInstitute;
                    secondaryExamDetails.UndgradGradProgID = 1; // N/A
                    secondaryExamDetails.GroupOrSubjectID = oLevelGroupOrSubjectID;
                    secondaryExamDetails.ResultDivisionID = oLevelResultDivisionID;

                    secondaryExamDetails.GPA = Convert.ToDecimal(hfOLevelAppearedConvertedSscGPA.Value);
                    //secondaryExamDetails.Marks = Convert.ToDecimal(lblOLevelResult.Text);


                    secondaryExamDetails.PassingYear = oLevelPassYearID;
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

                    secondaryExam.ExamTypeID = oLevelExamTypeId;
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

                    #region A-Level

                    DAL.ExamDetail higherSecondaryExamDetails = new DAL.ExamDetail();

                    higherSecondaryExamDetails.EducationBoardID = aLevelEducationBoardId;
                    higherSecondaryExamDetails.Institute = aLevelInstitute;
                    higherSecondaryExamDetails.UndgradGradProgID = 1; // N/A
                    higherSecondaryExamDetails.GroupOrSubjectID = aLevelGroupOrSubjectID;
                    higherSecondaryExamDetails.ResultDivisionID = aLevelResultDivisionID;

                    higherSecondaryExamDetails.GPA = Convert.ToDecimal(hfALevelAppearedConvertedHscGPA.Value);
                    //higherSecondaryExamDetails.Marks = Convert.ToDecimal(lblALevelResult.Text);

                    higherSecondaryExamDetails.PassingYear = aLevelPassYearID;
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
                    higherSecondaryExam.ExamTypeID = aLevelExamTypeId; //int.Parse(ddlHigherSec_ExamType.SelectedValue);
                    higherSecondaryExam.CandidateID = candidateIdLong;
                    higherSecondaryExam.ExamDetailsID = higherSecondaryExamDetailsIDLong;
                    higherSecondaryExam.Attribute1 = "Appeared";
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

                    admUnitObjT = (List<DAL.AdmissionUnit>)Session["AdmUnitObj_Session"];
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


                    Session["SSC_TelitalkEducationResult"] = null;
                    Session["HSC_TelitalkEducationResult"] = null;

                    Session["SSC_TelitalkEducationResult_JSON"] = null;
                    Session["HSC_TelitalkEducationResult_JSON"] = null;

                    Session["CandidateFormSerial_Session"] = null;
                    Session["CandidatePayment_Session"] = null;
                    Session["AdmUnitObj_Session"] = null;

                    Session["Eligible_NotEligible_List"] = null;

                    #region N/A
                    //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                    //    + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
                    //    + admissionSetup.EducationCategoryID + ";";

                    //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                    //            + -1 + ";1;" + -1 + ";"
                    //            + admissionSetup.EducationCategoryID + ";"; 
                    #endregion

                    //// 1) CandidateId
                    //// 2) CandidatePaymentID
                    //// 3) CandidateFormSerialID
                    //// 4) IsMultiple
                    //// 5) AdmUnitId
                    //// 6) EducationCategoryId

                    /// Summary : WebConfigurationManager.AppSettings["AllStepInOneTime"]
                    /// This variable set on Web.config file.
                    /// If the value is 1 then do not need to pay first . All information fill up first then final submit and payment
                    /// If the value is 0 then the process will same as previous.
                    /// 
                    int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                    if (AllStepInOneTime == 1)
                    {
                        string CandidatePaymentId = "";
                        using (var db = new CandidateDataManager())
                        {
                            var CandidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.ID == candidatePaymentIDLong).FirstOrDefault();
                            if (CandidatePayment != null)
                                CandidatePaymentId = CandidatePayment.PaymentId.ToString();
                        }

                        string UserName = "",Password = "";
                        using (var db = new CandidateDataManager())
                        {
                            var candidateObj = db.AdmissionDB.BasicInfoes.Where(x => x.ID == candidateIdLong).FirstOrDefault();
                            if (candidateObj != null)
                            {
                                var candidateUserObj = db.AdmissionDB.CandidateUsers.Where(x => x.ID == candidateObj.CandidateUserID).FirstOrDefault();
                                if (candidateUserObj != null)
                                {
                                    UserName = candidateUserObj.UsernameLoginId;
                                    Password = candidateUserObj.Password;
                                }
                            }
                        }


                        lblAlertName.Text = "";
                        lblAlertPaymentId.Text = "";
                        lblAlertMessage.Text = "";

                        lblAlertName.Text = candidate.FirstName;
                        lblAlertPaymentId.Text ="Payment Id : "+ CandidatePaymentId +"<br /><br/>" +
                            "<b>Login Credentials:</b><br/>" +
                            "Username: " + UserName + "<br/>" +
                            "Password: " + Password + "<br/>";
                        lblAlertMessage.Text = "Please note down your payment Id, Credentials and save this number for future reference.Please fill up your all information and make your payment";

                        hdnCandidateUserId.Value = candidateUser.ID.ToString();

                        modalPopupAlert.Show();

                        string urlParam = candidateIdLong + ";"
                                            + candidatePaymentIDLong + ";"
                                            + -1
                                            + ";1;"
                                            + -1 + ";"
                                            + 4 + ";";

                        SendSMSAndEmail(candidateIdLong, urlParam);

                        // Scroll and focus
                        string script = @"
        setTimeout(function() {
            window.scrollTo({ top: 0, behavior: 'smooth' });
            var btnOk = document.getElementById('" + btnOk.ClientID + @"');
            if (btnOk) {
                btnOk.focus();
            }
        }, 150);
    ";

                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                            "focusModal", script, true);

                    }
                    else
                    {
                        string urlParam = candidateIdLong + ";"
                                            + candidatePaymentIDLong + ";"
                                            + -1
                                            + ";1;"
                                            + -1 + ";"
                                            + 4 + ";";

                        SendSMSAndEmail(candidateIdLong, urlParam);

                        Response.Redirect("~/Admission/Candidate/PurchaseNotification.aspx?value=" + urlParam, false);
                    }
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
                        string UserName="",password="";
                        var candidateObj = db.AdmissionDB.BasicInfoes.Where(x => x.ID == candidateId).FirstOrDefault();

                        if (candidateObj != null)
                        {
                            var candidateUserObj = db.AdmissionDB.CandidateUsers.Where(x => x.ID == candidateObj.CandidateUserID).FirstOrDefault();
                            if (candidateUserObj != null)
                            {
                                UserName = candidateUserObj.UsernameLoginId;
                                password = candidateUserObj.Password;
                            }
                        }

                        string mailBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + ", <br/><br/>" +
                       "Thank you for your interest in the <b>Bangladesh Health Professions Institute (BHPI)</b>. To move forward with your admission process, please proceed with the payment using the link provided below: " +
                       "<br/><br/>" +
                       "<div style='padding: 15px; background-color: #f9f9f9; border-left: 5px solid #8a151b;'>" +
                       "<b>Payment Link: </b><a href='http://admission.bhpi.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + paramValue + "'>Click here to pay</a><br/>" +
                       "<span style='color: #8a151b; font-size: 16px;'><b>Payment ID: " + candidatePaymentObj.PaymentId.ToString() + "</b></span>" +
                          "<br/><br/>" +
                          "<b>Login Credentials:</b><br/>" +
                            "Username: " + UserName + "<br/>" +
                            "Password: " + password + "<br/>" +

                       "</div>" +
                       "<br/>" +
                       "<b>Applied For:</b><br/>" +
                       admUnitProgramStr + ". <br/>" +
                       "<br/>" +
                       "<p style='color: #006a4e; font-weight: bold;'>Important: After a successful payment, you must login to the portal, complete the full application form, and click submit.</p>" +
                       "<p>" +
                       "Regards," +
                       "<br/>" +
                       "<b>Admission Office</b><br/>" +
                       "Bangladesh Health Professions Institute (BHPI)<br/>" +
                       "<small>Academic Wing of CRP</small>" +
                       "</p>";

                        string fromAddress = "bhpiadmission@gmail.com"; // Updated to BHPI domain
                        string senderName = "BHPI Admission";       // Updated sender name
                        string subject = "Your BHPI Application Payment ID"; // Updated subject
                                                                             //bool isSentEmail = EmailUtility.SendMail(candidateEmail, fromAddress, senderName, subject, mailBody);
                        bool isSentEmail = EmailUtility.SendMail(candidatePaymentObj.BasicInfo.Email, fromAddress, senderName, subject, mailBody);


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
                        //GetSendingInfo(candidatePaymentObj.CandidateID, paramValue, candidatePaymentObj);
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




        protected void FillUpInformation(TelitalkEducationResultModelSSC sscResultModel, TelitalkEducationResultModelHSC hscResultModel)
        {
            try
            {
                txtName.Text = sscResultModel.name;
                txtName.Enabled = false;

                #region Day, Month, Year
                try
                {
                    string sentence = sscResultModel.dob;
                    char[] charArr = sentence.ToCharArray();

                    if (charArr != null && charArr.Length > 0)
                    {
                        string day = string.Empty;
                        string month = string.Empty;
                        string year = string.Empty;

                        day = charArr[0].ToString() + charArr[1].ToString();
                        month = charArr[2].ToString() + charArr[3].ToString();
                        year = charArr[4].ToString() + charArr[5].ToString() + charArr[6].ToString() + charArr[7].ToString();


                        if (!string.IsNullOrEmpty(day))
                        {
                            ddlDay.SelectedValue = day;
                            ddlDay.Enabled = false;
                        }
                        if (!string.IsNullOrEmpty(month))
                        {
                            ddlMonth.SelectedValue = month;
                            ddlMonth.Enabled = false;
                        }
                        if (!string.IsNullOrEmpty(year))
                        {
                            ddlYear.SelectedValue = year;
                            ddlYear.Enabled = false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ddlDay.SelectedValue = "-1";
                    ddlMonth.SelectedValue = "-1";
                    ddlYear.SelectedValue = "-1";

                    ddlDay.Enabled = true;
                    ddlMonth.Enabled = true;
                    ddlYear.Enabled = true;
                }
                #endregion

                #region Gender
                if (sscResultModel.gender.ToLower() == "male")
                {
                    ddlGender.SelectedValue = "2"; // 2 = Male
                    ddlGender.Enabled = false;
                }
                else if (sscResultModel.gender.ToLower() == "female")
                {
                    ddlGender.SelectedValue = "3"; // 3 = Female
                    ddlGender.Enabled = false;
                }
                else
                {
                    ddlGender.Enabled = true;
                }
                #endregion

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSSCHSC_Click(object sender, EventArgs e)
        {
            try
            {
                //BindSSCHSCYear();

                rblEducationChoice.ClearSelection();
                rblEducationChoice.SelectedValue = "1";
                //rblEducationChoice_SelectedIndexChanged(null, null);

                panelSubmitButtonSection.Visible = true;

                ClearSSCHSCSection();
                panelSSCHSCInputSection.Visible = false;

                panelBasicInfo.Visible = true;

            }
            catch (Exception ex)
            {

            }
        }

        private void BindSSCHSCYear()
        {
            try
            {
                ddlPassYearSSC.Items.Clear();
                ddlPassYearSSC.Items.Add(new ListItem("--Select Passing Year--", "-1"));
                ddlPassYearSSC.AppendDataBoundItems = true;

                ddlPassYearHSC.Items.Clear();
                ddlPassYearHSC.Items.Add(new ListItem("--Select Passing Year--", "-1"));
                ddlPassYearHSC.AppendDataBoundItems = true;


                List<DAL.SSCHSCPassingYearSetup> setupList = null;
                using (var db = new OfficeDataManager())
                {
                    setupList = db.AdmissionDB.SSCHSCPassingYearSetups.Where(x => x.IsActive == true).ToList();
                    if (setupList != null && setupList.Any())
                    {
                        var SSC = setupList.Where(c => c.ExamTypeId == 1).FirstOrDefault(); // 1 Is For SSC
                        var HSC = setupList.Where(c => c.ExamTypeId == 2).FirstOrDefault(); // 2 Is For HSC

                        if (SSC != null)
                        {
                            int StartYear = Convert.ToInt32(SSC.StartYear);
                            int EndYear = Convert.ToInt32(SSC.EndYear);

                            for (int i = StartYear; i <= EndYear; i++)
                            {
                                ddlPassYearSSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            }
                        }
                        if (HSC != null)
                        {
                            int StartYear = Convert.ToInt32(HSC.StartYear);
                            int EndYear = Convert.ToInt32(HSC.EndYear);
                            for (int i = StartYear; i <= EndYear; i++)
                            {
                                ddlPassYearHSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnOALevel_Click(object sender, EventArgs e)
        {
            try
            {
                rblEducationChoice.ClearSelection();
                rblEducationChoice.SelectedValue = "2";
                rblEducationChoice_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnInternationalBaccalaureate_Click(object sender, EventArgs e)
        {
            try
            {
                rblEducationChoice.ClearSelection();
                rblEducationChoice.SelectedValue = "3";
                rblEducationChoice_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCalculatIB_Click(object sender, EventArgs e)
        {
            try
            {
                decimal resultIB = 0.00M;
                decimal totalPoint = 0.00M;

                decimal totalPointToCheck = 30.5M;

                lblOLevelResult.Text = string.Empty;
                lblALevelResult.Text = string.Empty;

                lblMassageIB.Text = string.Empty;
                messagePanelIB.Visible = false;


                //-------------------------- International Baccalaureate -------------------------------
                decimal subject1 = Decimal.Parse(ddlIBSubject1.SelectedValue.ToString());
                decimal subject2 = Decimal.Parse(ddlIBSubject2.SelectedValue.ToString());
                decimal subject3 = Decimal.Parse(ddlIBSubject3.SelectedValue.ToString());
                decimal subject4 = Decimal.Parse(ddlIBSubject4.SelectedValue.ToString());
                decimal subject5 = Decimal.Parse(ddlIBSubject5.SelectedValue.ToString());
                decimal subject6 = Decimal.Parse(ddlIBSubject6.SelectedValue.ToString());

                resultIB = (subject1 + subject2 + subject3 + subject4 + subject5 + subject6);

                lblIBResult.Text = resultIB.ToString();
                hfIBConvertedSscGPA.Value = (resultIB / 5).ToString();


                //-------------------------- Total Point To Check -------------------------------
                totalPoint = (subject1 + subject2 + subject3 + subject4 + subject5 + subject6);
                //decimal[] point = { subject1, subject2, subject3, subject4, subject5, subject6 };

                if (totalPoint >= totalPointToCheck)
                {
                    //lblTotalPoints.Text = totalPoint.ToString();

                    //....Show Button Section
                    panelSubmitButtonSection.Visible = true;
                }
                else
                {
                    //....Hide Button Section
                    panelSubmitButtonSection.Visible = false;

                    lblIBResult.Text = totalPoint.ToString();

                    lblMassageIB.Text = "You are not Eligible to apply. Minimum requirement is 26.5 points";
                    lblMassageIB.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMassageIB.Attributes.CssStyle.Add("color", "crimson");

                    messagePanelIB.Visible = true;
                    messagePanelIB.CssClass = "alert alert-danger";

                    btnSubmit.Visible = false;
                    MessageView("You are not Eligible to Apply..!!", "fail");
                }

            }
            catch (Exception ex)
            {
                lblMassageIB.Text = "Message: " + ex.Message.ToString();
                lblMassageIB.Attributes.CssStyle.Add("font-weight", "bold");
                lblMassageIB.Attributes.CssStyle.Add("color", "crimson");

                messagePanelIB.Visible = true;
                messagePanelIB.CssClass = "alert alert-danger";

                btnSubmit.Visible = false;
            }
        }






        protected MessageModel CheckEligibleValidation(TelitalkEducationResultModelSSC sscResultModel, TelitalkEducationResultModelHSC hscResultModel)
        {
            MessageModel mModel = new MessageModel();

            //bool result = true;

            int i = 1;
            Dictionary<int, string> messageList = new Dictionary<int, string>();

            try
            {
                int educationChoiceId = Convert.ToInt32(rblEducationChoice.SelectedValue);

                #region SSC, SSC (Vocational), HSC, HSC (Vocational), Dakhil, Alim
                if (educationChoiceId == 1)
                {


                    int sscExamType = 1; // SSC
                    int hscExamType = 2; // HSC

                    decimal sscGPA = 0.00M;
                    decimal hscGPA = 0.00M;

                    #region SSC GPA
                    try
                    {
                        if (!string.IsNullOrEmpty(sscResultModel.gpa))
                        {
                            sscGPA = Convert.ToDecimal(sscResultModel.gpa);
                        }
                        else
                        {
                            sscGPA = 0.00M;
                            messageList.Add(i++, "SSC GPA not Found !!");
                        }
                    }
                    catch (Exception ex)
                    {
                        sscGPA = 0.00M;
                        messageList.Add(i++, "Exception: SSC GPA unable to convert; Message: " + ex.Message.ToString());
                    }
                    #endregion

                    #region HSC GPA
                    try
                    {
                        if (!string.IsNullOrEmpty(hscResultModel.gpa))
                        {
                            hscGPA = Convert.ToDecimal(hscResultModel.gpa);
                        }
                        else
                        {
                            hscGPA = 0.00M;
                            messageList.Add(i++, "HSC GPA not Found !!");
                        }
                    }
                    catch (Exception ex)
                    {
                        hscGPA = 0.00M;
                        messageList.Add(i++, "Exception: HSC GPA unable to convert; Message: " + ex.Message.ToString());
                    }
                    #endregion


                    //Group
                    //3   Science -- SCIENCE
                    //4   Humanities
                    //5   Business Studies

                    int sscGroup = -1;

                    #region SSC Group
                    try
                    {
                        if (sscResultModel.studGroup.ToUpper() == "SCIENCE")
                        {
                            sscGroup = 3;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "HUMANITIES")
                        {
                            sscGroup = 4;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "BUSINESS STUDIES")
                        {
                            sscGroup = 5;
                        }

                        else if (sscResultModel.studGroup.ToUpper() == "GENERAL")
                        {
                            sscGroup = 6;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "71-DRESS MAKING")
                        {
                            sscGroup = 7;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "ACCOUNTING")
                        {
                            sscGroup = 8;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "COMPUTER OPERATION")
                        {
                            sscGroup = 9;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "68-COMPUTER & INFORM")
                        {
                            sscGroup = 10;
                        }

                        else if (sscResultModel.studGroup.ToUpper() == "COMMERCE")
                        {
                            sscGroup = 11;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "90-GENERAL ELECTRICA")
                        {
                            sscGroup = 12;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "62-GENERAL ELECTRONI")
                        {
                            sscGroup = 13;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "COMPUTER OPERATION A")
                        {
                            sscGroup = 14;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "96-WELDING AND FABR")
                        {
                            sscGroup = 15;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "MUZABBID")
                        {
                            sscGroup = 16;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "64-BUILDING MAINTENA")
                        {
                            sscGroup = 17;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "BANKING")
                        {
                            sscGroup = 18;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "67-CIVIL CONSTRUCTIO")
                        {
                            sscGroup = 19;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "HUMAN RESOURCE MANAG")
                        {
                            sscGroup = 20;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "AGRO BASED FOOD")
                        {
                            sscGroup = 21;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "ARCHITECTURAL DRAFTING WITH AUTOCAD")
                        {
                            sscGroup = 23;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "AUTOMOTIVE")
                        {
                            sscGroup = 25;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "BUILDING MAINTENANCE")
                        {
                            sscGroup = 26;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "CIVIL CONSTRUCTION")
                        {
                            sscGroup = 28;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "CIVIL DRAFTING WITH CAD")
                        {
                            sscGroup = 29;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "COMPUTER & INFORMATION TECHNOLOGY")
                        {
                            sscGroup = 31;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "DRESS MAKING")
                        {
                            sscGroup = 36;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "DYING, PRINTING AND FINISHING")
                        {
                            sscGroup = 37;
                        }

                        else if (sscResultModel.studGroup.ToUpper() == "ELECTRICAL MAINTENANCE WORKS")
                        {
                            sscGroup = 39;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "FARM MACHINERY")
                        {
                            sscGroup = 43;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "FISH CULTURE AND BREEDING")
                        {
                            sscGroup = 45;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "FOOD PROCESSING AND PRESERVATION")
                        {
                            sscGroup = 46;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "FRUIT AND VEGETABLE CULTIVATION")
                        {
                            sscGroup = 47;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "GENERAL ELECTRICAL WORKS")
                        {
                            sscGroup = 48;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "GENERAL ELECTRONICS")
                        {
                            sscGroup = 49;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "GENERAL MECHANICS")
                        {
                            sscGroup = 50;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "KNITTING")
                        {
                            sscGroup = 54;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "LIVESTOCK REARING AND FARMING")
                        {
                            sscGroup = 55;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "MACHINE TOOLS OPERATION")
                        {
                            sscGroup = 56;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "MECHANICAL DRAFTING WITH CAD")
                        {
                            sscGroup = 58;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "PATIENT CARE TECHNIQUE")
                        {
                            sscGroup = 59;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "PLUMBING AND PIPE FITTING")
                        {
                            sscGroup = 60;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "POULTRY REARING AND FARMING")
                        {
                            sscGroup = 61;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "REFRIGERATION AND AIR CONDITIONING")
                        {
                            sscGroup = 62;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "REFRIGERATION AND AIR-CONDITIONING")
                        {
                            sscGroup = 63;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "SHRIMP CULTURE AND BREEDING")
                        {
                            sscGroup = 66;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "WEAVING")
                        {
                            sscGroup = 67;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "WELDING  AND FABRICATION")
                        {
                            sscGroup = 68;
                        }
                        else if (sscResultModel.studGroup.ToUpper() == "WOOD WORKING")
                        {
                            sscGroup = 70;
                        }
                        else
                        {
                            sscGroup = -1;
                            messageList.Add(i++, "SSC Student Group did not match !!");
                        }
                    }
                    catch (Exception ex)
                    {
                        sscGroup = -1;
                        messageList.Add(i++, "Exception: SSC Student Group did not match; Message: " + ex.Message.ToString());
                    }
                    #endregion


                    int hscGroup = -1;

                    #region HSC Group
                    try
                    {
                        if (hscResultModel.studGroup.ToUpper() == "SCIENCE")
                        {
                            hscGroup = 3;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "HUMANITIES")
                        {
                            hscGroup = 4;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "BUSINESS STUDIES")
                        {
                            hscGroup = 5;
                        }

                        else if (hscResultModel.studGroup.ToUpper() == "GENERAL")
                        {
                            hscGroup = 6;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "71-DRESS MAKING")
                        {
                            hscGroup = 7;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "ACCOUNTING")
                        {
                            hscGroup = 8;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "COMPUTER OPERATION")
                        {
                            hscGroup = 9;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "68-COMPUTER & INFORM")
                        {
                            hscGroup = 10;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "COMMERCE")
                        {
                            hscGroup = 11;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "90-GENERAL ELECTRICA")
                        {
                            hscGroup = 12;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "62-GENERAL ELECTRONI")
                        {
                            hscGroup = 13;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "COMPUTER OPERATION A")
                        {
                            hscGroup = 14;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "96-WELDING AND FABR")
                        {
                            hscGroup = 15;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "MUZABBID")
                        {
                            hscGroup = 16;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "64-BUILDING MAINTENA")
                        {
                            hscGroup = 17;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "BANKING")
                        {
                            hscGroup = 18;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "67-CIVIL CONSTRUCTIO")
                        {
                            hscGroup = 19;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "HUMAN RESOURCE MANAG")
                        {
                            hscGroup = 20;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "AGRO MACHINERY")
                        {
                            hscGroup = 22;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "AUTOMOBILE")
                        {
                            hscGroup = 24;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "BUILDING MAINTENANCE AND CONSTRUCTION")
                        {
                            hscGroup = 27;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "CLOTHING AND GARMENTS FINISHING")
                        {
                            hscGroup = 30;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "COMPUTER OPERATION AND MAINTENANCE")
                        {
                            hscGroup = 32;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "COMPUTERIZED ACCOUNTING SYSTEM")
                        {
                            hscGroup = 33;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "DIGITAL TECHNOLOGY IN BUSINESS")
                        {
                            hscGroup = 34;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "DRAFTING CIVIL")
                        {
                            hscGroup = 35;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "E-BUSINESS")
                        {
                            hscGroup = 38;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "ELECTRICAL WORKS AND MAINTENANCE")
                        {
                            hscGroup = 40;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "ELECTRONIC CONTROL AND COMMUNICATION")
                        {
                            hscGroup = 41;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "ENTREPRENEURSHIP DEVELOPMENT")
                        {
                            hscGroup = 42;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "FINANCIAL PRACTICES")
                        {
                            hscGroup = 44;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "FISH CULTURE AND BREEDING")
                        {
                            hscGroup = 45;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "HUMAN RESOURCE DEVELOPMENT")
                        {
                            hscGroup = 51;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "HUMAN RESOURCE MANAGEMENT")
                        {
                            hscGroup = 52;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "INDUSTRIAL WOOD WORKING")
                        {
                            hscGroup = 53;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "MACHINE TOOLS OPERATION AND MAINTENANCE")
                        {
                            hscGroup = 57;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "POULTRY REARING AND FARMING")
                        {
                            hscGroup = 61;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "REFRIGERATION AND AIR-CONDITIONING")
                        {
                            hscGroup = 63;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "SECRETARIAL SCIENCE")
                        {
                            hscGroup = 64;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "SHORTHAND")
                        {
                            hscGroup = 65;
                        }
                        else if (hscResultModel.studGroup.ToUpper() == "WELDING AND FABRICATION")
                        {
                            hscGroup = 69;
                        }
                        else
                        {
                            hscGroup = -1;
                            messageList.Add(i++, "HSC Student Group did not match !!");
                        }
                    }
                    catch (Exception ex)
                    {
                        hscGroup = -1;
                        messageList.Add(i++, "Exception: HSC Student Group did not match; Message: " + ex.Message.ToString());
                    }
                    #endregion



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

                    #region N/A -- Variable Declares
                    //List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admSscList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();
                    //List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admHscList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();



                    //List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();
                    //List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> notEligibleList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>(); 
                    #endregion


                    #region N/A -- Validation Check For SSC
                    //if (sscExamType > 0 && sscGroup > 0 && sscGPA > 0)
                    //{
                    //    try
                    //    {
                    //        if (admUnitObjT.Count() > 0)
                    //        {
                    //            foreach (DAL.AdmissionUnit item in admUnitObjT)
                    //            {
                    //                DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result tempEligibleSSCList = new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result();
                    //                using (var db = new OfficeDataManager())
                    //                {

                    //                    admSscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(item.ID, sscExamType, sscGroup, sscGPA).ToList();
                    //                    if (admSscList.Count > 0)
                    //                    {
                    //                        tempEligibleSSCList.ID = admSscList[0].ID;
                    //                        tempEligibleSSCList.AdmissionUnitID = admSscList[0].AdmissionUnitID;
                    //                        tempEligibleSSCList.ExamTypeID = admSscList[0].ExamTypeID;
                    //                        tempEligibleSSCList.GroupID = admSscList[0].GroupID;
                    //                        tempEligibleSSCList.GPA = admSscList[0].GPA;

                    //                        eligibleList.Add(tempEligibleSSCList);
                    //                    }

                    //                }

                    //            }//end foreach
                    //        }// end if (admUnitObjT.Count() > 0)

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //Console.WriteLine(ex.StackTrace);
                    //        messageList.Add(i++, "Exception: While Validating SSC Information; Message: " + ex.Message.ToString());
                    //    }
                    //}
                    //else
                    //{
                    //    messageList.Add(i++, "SSC ExamType, Group and GPA is not in proper Format !!");
                    //}
                    #endregion

                    #region N/A -- Validation Check HSC
                    //if (hscExamType > 0 && hscGroup > 0 && hscGPA > 0)
                    //{
                    //    try
                    //    {

                    //        if (admUnitObjT.Count() > 0)
                    //        {
                    //            foreach (DAL.AdmissionUnit item in admUnitObjT)
                    //            {
                    //                DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result tempEligibleHSCList = new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result();
                    //                using (var db = new OfficeDataManager())
                    //                {

                    //                    admHscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(item.ID, hscExamType, hscGroup, hscGPA).ToList();
                    //                    if (admHscList.Count > 0)
                    //                    {
                    //                        tempEligibleHSCList.ID = admHscList[0].ID;
                    //                        tempEligibleHSCList.AdmissionUnitID = admHscList[0].AdmissionUnitID;
                    //                        tempEligibleHSCList.ExamTypeID = admHscList[0].ExamTypeID;
                    //                        tempEligibleHSCList.GroupID = admHscList[0].GroupID;
                    //                        tempEligibleHSCList.GPA = admHscList[0].GPA;

                    //                        eligibleList.Add(tempEligibleHSCList);
                    //                    }

                    //                }

                    //            }//end foreach
                    //        }// end if (admUnitObjT.Count() > 0)



                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        messageList.Add(i++, "Exception: While Validating HSC Information; Message: " + ex.Message.ToString());
                    //    }
                    //}
                    //else
                    //{
                    //    messageList.Add(i++, "HSC ExamType, Group and GPA is not in proper Format");
                    //}
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

                                    lblErrormsg.Text = "Total Required Point:" + totalGPAPointSetup + " | Your Total Point:" + sumSSCHSCGPAPoint;

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

                                        if (sscASHGS == null)
                                            lblErrormsg.Text = sscASHGS + " NUll";

                                        if (hscASHGS == null)
                                            lblErrormsg.Text = hscASHGS + " NUll";

                                        if (sscASHGS != null && hscASHGS != null)
                                        {
                                            #region Only For FST+FOE Faculty

                                            if (item.ID == 26)
                                            {
                                                /// Currently Two Faculty is merged.One Is FST and Another one is FOE.
                                                /// Need to check some subject wise grade condition to eligible this faculty.
                                                /// This condition is checked only for HSC subjects.
                                                /// FOR FOE Minimum A grade in Physics, Chemistry, Mathematics and minimum A- (A minus) grade in English at the HSC level.
                                                /// FOR FST Minimum A grade in Physics, Chemistry, Biology and minimum A- (A minus) grade in English at the HSC level.


                                                List<TelitalkEducationSubjectModel> hscsubjects = new List<TelitalkEducationSubjectModel>();
                                                if (hscResultModel != null)
                                                {
                                                    hscsubjects = hscResultModel.subject.ToList();
                                                }

                                                if (hscsubjects != null && hscsubjects.Any())
                                                {
                                                    TelitalkEducationSubjectModel hscPhy = null;
                                                    TelitalkEducationSubjectModel hscChem = null;
                                                    TelitalkEducationSubjectModel hscBiology = null;
                                                    TelitalkEducationSubjectModel hscHigherMath = null;
                                                    TelitalkEducationSubjectModel hscEng = null;

                                                    hscPhy = hscsubjects.Where(x => x.subCode == "224+225" || x.subCode == "81422" || x.subCode == "174").FirstOrDefault();
                                                    hscChem = hscsubjects.Where(x => x.subCode == "226+227" || x.subCode == "81423" || x.subCode == "176").FirstOrDefault();
                                                    hscBiology = hscsubjects.Where(x => x.subCode == "178" || x.subCode == "230" || x.subCode == "230+231").FirstOrDefault();
                                                    hscHigherMath = hscsubjects.Where(x => x.subCode == "265" || x.subCode == "228" || x.subCode == "228+229" || x.subCode == "81421").FirstOrDefault();
                                                    hscEng = hscsubjects.Where(x => x.subCode == "107" || x.subCode == "238" || x.subCode == "238+239" || x.subCode == "81112" || x.subCode == "81122" || x.subCode == "21812" || x.subCode == "21822").FirstOrDefault();

                                                    /// FOR FOE Minimum A grade in Physics, Chemistry, Mathematics and minimum A- (A minus) grade in English at the HSC level.
                                                    /// FOR FST Minimum A grade in Physics, Chemistry, Biology and minimum A- (A minus) grade in English at the HSC level.

                                                    // need to check this 2 condition

                                                    if (hscPhy != null && hscChem != null && hscEng != null)
                                                    {

                                                        if (
                                                            (hscPhy.grade.ToUpper() == "A" || hscPhy.grade.ToUpper() == "A+") &&
                                                            (hscChem.grade.ToUpper() == "A" || hscChem.grade.ToUpper() == "A+") &&
                                                            (hscEng.grade.ToUpper() == "A-" || hscEng.grade.ToUpper() == "A" || hscEng.grade.ToUpper() == "A+") &&
                                                            (
                                                                (hscHigherMath != null && (hscHigherMath.grade.ToUpper() == "A" || hscHigherMath.grade.ToUpper() == "A+")) ||
                                                                (hscBiology != null && (hscBiology.grade.ToUpper() == "A" || hscBiology.grade.ToUpper() == "A+"))
                                                            )
                                                            )
                                                        {
                                                            // Eligible for merged faculty FST+FOE
                                                        }
                                                        else
                                                        {
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
                                                    // Listed Removed Facultys
                                                    admUnitObjForRemovedList.Add(item);
                                                }

                                            }

                                            #endregion

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
                            messageList.Add(i++, "SSC or HSC Examtype OR SSC or HSC Group OR SSC or HSC GPA not found or zero(0)");
                        }
                    }
                    catch (Exception ex)
                    {
                        messageList.Add(i++, "Exception: Validation Check for SSC & HSC; Message: " + ex.Message.ToString());
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

                        #region N/A -- Delete Not Eligible Faculty from Selected Faculty

                        //foreach (DAL.AdmissionUnit item in admUnitObjT)
                        //{
                        //    if (eligibleList.Where(x => x.AdmissionUnitID == item.ID).ToList().Count == 2)
                        //    {

                        //    }
                        //    else
                        //    {

                        //        canForTemp = candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault();
                        //        try
                        //        {
                        //            using (var db = new OfficeDataManager())
                        //            {
                        //                admSetupTemp = db.AdmissionDB.AdmissionSetups.Find(canForTemp.AdmissionSetupID);
                        //            }
                        //        }
                        //        catch (Exception)
                        //        {

                        //        }



                        //        //----removing faculty which one is not eligible
                        //        candidateFormSlT.Remove(candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault());



                        //        //--- removing faculty amount which one is not wligible
                        //        candidatePaymentT.Amount = candidatePaymentT.Amount - admSetupTemp.Fee;
                        //    }
                        //}
                        #endregion

                        List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleAndNotEligibleList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();

                        #region New Delete Not Eligible Faculty from Selected Faculty
                        try
                        {
                            if (admUnitObjForRemovedList != null && admUnitObjForRemovedList.Count > 0)
                            {
                                #region Preparing a View List for showing Eligible & Not-Eligible List
                                try
                                {
                                    // Attribute2 = 1 = Eligible
                                    // Attribute2 = 2 = Not Eligible

                                    foreach (var tData in admUnitObjT)
                                    {
                                        DAL.AdmissionUnit au = admUnitObjForRemovedList.Where(x => x.ID == tData.ID).FirstOrDefault();
                                        if (au != null)
                                        {
                                            eligibleAndNotEligibleList.Add(
                                                new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result
                                                {
                                                    Attribute1 = tData.UnitName,
                                                    Attribute2 = "2",
                                                    Attribute3 = "Not Eligible"
                                                });
                                        }
                                        else
                                        {
                                            eligibleAndNotEligibleList.Add(
                                                new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result
                                                {
                                                    Attribute1 = tData.UnitName,
                                                    Attribute2 = "1",
                                                    Attribute3 = "Eligible"
                                                });
                                        }

                                    } //END: Foreach

                                    if (eligibleAndNotEligibleList != null && eligibleAndNotEligibleList.Count > 0)
                                    {
                                        Session["Eligible_NotEligible_List"] = eligibleAndNotEligibleList;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
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

                }//.... END: if (educationChoiceId == 1) 
                #endregion


                return mModel;

            }
            catch (Exception ex)
            {
                mModel.MessageCode = 2;
                mModel.MessageBody = "Exception: Eligible Validation; Message: " + ex.Message.ToString();
                mModel.MessageBoolean = false;

                return mModel;
            }


        }
        protected void btnVerifyInformation_Click(object sender, EventArgs e)
        {
            try
            {
                MessageView("", "clear");

                PanelMessageEligibleNotEligibleList.Visible = false;
                LabelMessageEligibleNotEligible.Text = string.Empty;


                #region SSC, SSC (Vocational), HSC, HSC (Vocational), Dakhil, Alim

                //MessageModel resultCheckInputFieldValidation = CheckInputFieldValidationSSCHSC();

                //if (resultCheckInputFieldValidation != null && resultCheckInputFieldValidation.MessageBoolean == true)
                //{

                #region EducationBoard (SSC-HSC)
                DAL.EducationBoard sscEducationBoard = null;
                DAL.EducationBoard hscEducationBoard = null;

                int sscEducationBoardId = Convert.ToInt32(ddlBoardSSC.SelectedValue);
                int hscEducationBoardId = Convert.ToInt32(ddlBoardHSC.SelectedValue);

                using (var db = new GeneralDataManager())
                {
                    sscEducationBoard = db.AdmissionDB.EducationBoards.Where(x => x.ID == sscEducationBoardId).FirstOrDefault();
                    hscEducationBoard = db.AdmissionDB.EducationBoards.Where(x => x.ID == hscEducationBoardId).FirstOrDefault();
                }

                string sscBoardT = "";
                if (sscEducationBoard != null)
                {
                    sscBoardT = sscEducationBoard.ShortName.Trim();
                }

                string hscBoardT = "";
                if (hscEducationBoard != null)
                {
                    hscBoardT = hscEducationBoard.ShortName.Trim();
                }
                #endregion

                #region Process
                string hscExamType = ddlExamTypeHSC.SelectedValue;
                string hscPassingYear = ddlPassYearHSC.SelectedValue;
                string hscBoard = hscBoardT;
                string hscRoll = txtRollHSC.Text;
                string hscRegNumber = txtRegHSC.Text;


                string sscPassingYearInput = ddlPassYearSSC.SelectedValue.ToString();
                string sscExamType = "ssc";
                string sscPassingYear = ddlPassYearSSC.SelectedValue;
                string sscBoard = sscBoardT;
                string sscRoll = txtRollSSC.Text;
                string sscReg = txtRegSSC.Text;

                string hscResultTER = null, sscResultTER = null;

                #region HSC

                #region Teletalk API
                //hscResultTER = TeletalkEducationBoard.GetData(hscExamType, hscBoard, hscRoll, hscPassingYear, hscRegNumber);
                #endregion

                #region Database Teletalk Data
                using (var db = new GeneralDataManager())
                {
                    try
                    {
                        ObjectResult<string> Result = db.AdmissionDB.GetStudentTeletalkHSCData(hscBoard, hscRoll, hscRegNumber, hscPassingYear);

                        var hscObj = Result.FirstOrDefault();

                        if (hscObj != null)
                            hscResultTER = hscObj;
                    }
                    catch (Exception ex)
                    {
                    }


                }
                #endregion

                #endregion

                #region SSC

                #region Teletalk API
                //sscResultTER = TeletalkEducationBoard.GetData(sscExamType, sscBoard, sscRoll, sscPassingYear, sscReg);
                #endregion

                #region Database Teletalk Data
                using (var db = new GeneralDataManager())
                {
                    try
                    {
                        ObjectResult<string> Result = db.AdmissionDB.GetStudentTeletalkSSCData(sscBoard, sscRoll, sscReg, sscPassingYear);

                        var sscObj = Result.FirstOrDefault();

                        if (sscObj != null)
                            sscResultTER = sscObj;
                    }
                    catch (Exception ex)
                    {
                    }


                }
                #endregion

                #endregion



                #region Check This Candidate Already Enrolled


                int AllStepInOneTime = Convert.ToInt32(WebConfigurationManager.AppSettings["AllStepInOneTime"]);

                if (AllStepInOneTime == 1)
                {
                    using (var db = new CandidateDataManager())
                    {
                        try
                        {
                            long admissionSetupIDLong = -1;

                            if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
                            {
                                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                            }
                            else
                            {
                                admissionSetupIDLong = Convert.ToInt64(Request.QueryString["asi"]);
                            }


                            #region Check This Candidate Already Enrolled.If yes then redirect to Basic Info page

                            var AdmissionSetupObj = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == admissionSetupIDLong).FirstOrDefault();

                            int AcacalId = 0;

                            if (AdmissionSetupObj != null)
                                AcacalId = AdmissionSetupObj.AcaCalID;

                            bool IsExists = false;
                            long CandidateID = 0;

                            var FormList = db.AdmissionDB.GetCandidateFormSerialInfoBySSCInfoAndAcacal(sscRoll, sscReg, sscPassingYear, sscEducationBoardId, AcacalId).ToList();

                            if (FormList != null && FormList.Any())
                            {
                                IsExists = true;

                                TelitalkEducationResultModelHSC hscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelHSC>(hscResultTER);
                                TelitalkEducationResultModelSSC sscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelSSC>(sscResultTER);

                                CheckEligibleValidation(sscResultModel, hscResultModel);

                                List<DAL.CandidateFormSl> checkedFormSlList = new List<DAL.CandidateFormSl>();
                                List<DAL.CandidateFormSl> remainingFormSlList = new List<DAL.CandidateFormSl>();

                                try
                                {
                                    if (Session["CandidateFormSerial_Session"] != null)
                                    {
                                        checkedFormSlList = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];
                                        remainingFormSlList = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                // Remove Already Applied Form List From CheckList
                                foreach (var form in FormList)
                                {
                                    try
                                    {
                                        remainingFormSlList = remainingFormSlList.Where(x => x.AdmissionSetupID != form.AdmissionSetupID).ToList();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                // New Faculty Found.
                                if (remainingFormSlList != null && remainingFormSlList.Any())
                                {
                                    #region New Add

                                    // Get Unpaid Form
                                    var unPaidFormObj = FormList.Where(x => x.IsPaid == false).FirstOrDefault();

                                    // If Some Faculty Found In Remaining List but there is no unpaid form then need to apply as a new one
                                    if (unPaidFormObj == null)
                                    {
                                        IsExists = false;
                                    }
                                    else// If Some Faculty Found In Remaining List and there is an unpaid form then need to add new faculty with existing candidate Id and Increase the form Amount
                                    {
                                        decimal AddeddAmount = 0;

                                        long CandidatePaymentID = unPaidFormObj.CandidatePaymentID;
                                        CandidateID = unPaidFormObj.CandidateID;



                                        using (var dbupdate = new CandidateDataManager())
                                        {
                                            var CandidatePaymentObj = dbupdate.AdmissionDB.CandidatePayments.Where(x => x.ID == CandidatePaymentID).FirstOrDefault();

                                            if (CandidatePaymentObj != null && CandidatePaymentObj.IsPaid == false)
                                            {
                                                #region New Faculty Insert

                                                foreach (var newFaculty in remainingFormSlList)
                                                {
                                                    ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                                                    long candidateFormSerialIDLong = -1;
                                                    try
                                                    {

                                                        using (var dbInsertFsl = new CandidateDataManager())
                                                        {
                                                            ObjectParameter id_param1 = new ObjectParameter("iD", typeof(long));
                                                            dbupdate.AdmissionDB.SPCandidateFormSlInsert(id_param, CandidateID, newFaculty.AdmissionSetupID, newFaculty.AcaCalID, Convert.ToInt32(newFaculty.Attribute2), null, CandidatePaymentID, DateTime.Now, -99);
                                                            candidateFormSerialIDLong = Convert.ToInt64(id_param.Value);

                                                            if (candidateFormSerialIDLong > 0)
                                                            {
                                                                var AdmissionSetupObjNew = dbupdate.AdmissionDB.AdmissionSetups.Where(x => x.ID == newFaculty.AdmissionSetupID).FirstOrDefault();
                                                                if (AdmissionSetupObjNew != null)
                                                                    AddeddAmount = AddeddAmount + AdmissionSetupObjNew.Fee;
                                                            }

                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }

                                                #endregion

                                                if (AddeddAmount > 0)
                                                {
                                                    CandidatePaymentObj.Attribute2 = "Amount Update From : " + CandidatePaymentObj.Amount + " to " + (CandidatePaymentObj.Amount + AddeddAmount);
                                                    CandidatePaymentObj.Amount = (CandidatePaymentObj.Amount) + AddeddAmount;
                                                    dbupdate.Update<DAL.CandidatePayment>(CandidatePaymentObj);
                                                }
                                            }

                                        }
                                    }
                                    #endregion

                                }

                                else // No new faculty found.So need to redirect to basic info page
                                {
                                    foreach (var checkedItem in checkedFormSlList)
                                    {
                                        try
                                        {
                                            var FormObj = FormList.Where(x => x.AdmissionSetupID == checkedItem.AdmissionSetupID && x.AcaCalID == checkedItem.AcaCalID).FirstOrDefault();
                                            if (FormObj != null)
                                            {
                                                CandidateID = FormObj.CandidateID;
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }

                            }

                            #endregion

                            if (IsExists)
                            {
                                var CandidateBasic = db.AdmissionDB.BasicInfoes.Where(x => x.ID == CandidateID).FirstOrDefault();
                                if (CandidateBasic != null)
                                {
                                    DAL.CandidateUser candidateUser = db.GetCandidateUserByID_ND(CandidateBasic.CandidateUserID);
                                    if (candidateUser != null)
                                    {
                                        SessionSGD.SaveObjToSession<long>(candidateUser.ID, SessionName.Common_UserId); //this is candidate user primary key
                                        SessionSGD.SaveObjToSession<string>(candidateUser.UsernameLoginId, SessionName.Common_LoginID); //this is the username
                                        SessionSGD.SaveObjToSession<string>("Candidate", SessionName.Common_RoleName);
                                        Response.Redirect("~/Admission/Candidate/ApplicationBasic.aspx", false);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                #endregion

                //if (hscRegNumber != sscReg)
                //{
                //    MessageView("Message : SSC and HSC registration number not match", "fail");
                //    return;
                //}



                //"{\"responseCode\":\"1\",\"responseDesc\":\"Success\",\"board\":\"DHAKA\",\"rollNo\":\"123456\",\"passYear\":\"2018\",\"name\":\"BADIUL ALAM BHUIYAN\",\"father\":\"AKTEROZZAMAN BHUIYAN\",\"mother\":\"RAHANA AKTER\",\"regNo\":\"1310539095\",\"gender\":\"MALE\",\"result\":\"P\",\"gpa\":\"4.83\",\"gpaExc4th\":\"4.33\",\"sscBoard\":\"DHAKA\",\"sscPassYear\":\"2016\",\"sscRoll\":\"127688\",\"sscRegNo\":\"1310539095\",\"studGroup\":\"SCIENCE\",\"eiin\":\"133965\",\"totalObtMark\":\"0922\",\"totalExc4TH\":\"0838\",\"iName\":\"DR. MAHBUBUR RAHMAN MOLLAH COLLEGE\",\"cCode\":\"120\",\"cName\":\"DHAKA - 23, SHAYMPUR MODEL SCHOOL & COLLEGE\",\"thana\":\"DEMRA\",\"sub4thCode\":\"178\",\"subject\":[{\"subCode\":\"101\",\"subName\":\"BANGLA\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"157\"},{\"subCode\":\"107\",\"subName\":\"ENGLISH\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"142\"},{\"subCode\":\"275\",\"subName\":\"INFORMATION & COMMUNICATION TECHNOLOGY\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"082\"},{\"subCode\":\"174\",\"subName\":\"PHYSICS\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"151\"},{\"subCode\":\"176\",\"subName\":\"CHEMISTRY\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"142\"},{\"subCode\":\"265\",\"subName\":\"HIGHER MATHEMATICS\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"164\"},{\"subCode\":\"178\",\"subName\":\"BIOLOGY\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"164\"}]}"

                if (!string.IsNullOrEmpty(hscResultTER)) //!string.IsNullOrEmpty(sscResultTER) && 
                {
                    Session["HSC_TelitalkEducationResult_JSON"] = null;
                    Session["HSC_TelitalkEducationResult_JSON"] = hscResultTER;

                    TelitalkEducationResultModelHSC hscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelHSC>(hscResultTER);

                    if (hscResultModel.responseCode == 1) //sscResultModel.responseCode == 1 &&
                    {
                        Session["HSC_TelitalkEducationResult"] = null;
                        Session["HSC_TelitalkEducationResult"] = hscResultModel;

                        #region SSC


                        //"{\"responseCode\":\"1\",\"responseDesc\":\"Success\",\"board\":\"DHAKA\",\"rollNo\":\"123456\",\"passYear\":\"2016\",\"name\":\"MD. SHAKIL AHAMED\",\"father\":\"MD. JAMAL UDDIN\",\"mother\":\"MST. SHAHANAJ\",\"regNo\":\"1310561161\",\"gender\":\"MALE\",\"result\":\"P\",\"gpa\":\"4.28\",\"gpaExc4th\":\"4.06\",\"studGroup\":\"SCIENCE\",\"eiin\":\"109327\",\"dob\":\"06052000\",\"totalObtMark\":\"0789\",\"totalExc4TH\":\"0759\",\"iName\":\"SINGARDIGHI HIGH SCHOOL\",\"cCode\":\"148\",\"cName\":\"MOWNA\",\"thana\":\"SREEPUR\",\"sub4thCode\":\"126\",\"subject\":[{\"subCode\":\"101\",\"subName\":\"BANGLA\",\"grade\":\"B\",\"gpoint\":\"3.00\",\"mark\":null},{\"subCode\":\"107\",\"subName\":\"ENGLISH\",\"grade\":\"A-\",\"gpoint\":\"3.50\",\"mark\":null},{\"subCode\":\"109\",\"subName\":\"MATHEMATICS\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":null},{\"subCode\":\"150\",\"subName\":\"BANGLADESH AND GLOBAL STUDIES\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":null},{\"subCode\":\"111\",\"subName\":\"ISLAM AND MORAL EDUCATION\",\"grade\":\"A-\",\"gpoint\":\"3.50\",\"mark\":null},{\"subCode\":\"136\",\"subName\":\"PHYSICS\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":null},{\"subCode\":\"137\",\"subName\":\"CHEMISTRY\",\"grade\":\"A-\",\"gpoint\":\"3.50\",\"mark\":null},{\"subCode\":\"138\",\"subName\":\"BIOLOGY\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":null},{\"subCode\":\"147\",\"subName\":\"PHYSICAL EDUCATION\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":null},{\"subCode\":\"126\",\"subName\":\"HIGHER MATHEMATICS\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":null}]}"

                        if (!string.IsNullOrEmpty(sscResultTER))
                        {
                            Session["SSC_TelitalkEducationResult_JSON"] = null;
                            Session["SSC_TelitalkEducationResult_JSON"] = sscResultTER;

                            TelitalkEducationResultModelSSC sscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelSSC>(sscResultTER);

                            if (sscResultModel.responseCode == 1)
                            {
                                Session["SSC_TelitalkEducationResult"] = null;
                                Session["SSC_TelitalkEducationResult"] = sscResultModel;


                                bool isSSCBoardNameMatched = true;
                                bool isHSCBoardNameMatched = true;

                                List<DAL.EducationBoard> ebList = null;
                                using (var db = new GeneralDataManager())
                                {
                                    ebList = db.AdmissionDB.EducationBoards.Where(x => x.IsActive == true).ToList();
                                }

                                if (ebList != null && ebList.Count > 0)
                                {
                                    if (ebList.Where(x => x.BoardName.ToLower() == sscResultModel.board.ToLower()).FirstOrDefault() == null)
                                    {
                                        isSSCBoardNameMatched = false;
                                    }
                                    if (ebList.Where(x => x.BoardName.ToLower() == hscResultModel.board.ToLower()).FirstOrDefault() == null)
                                    {
                                        isHSCBoardNameMatched = false;
                                    }
                                }

                                if (isSSCBoardNameMatched == true && isHSCBoardNameMatched == true)
                                {
                                    //.... Check Eligible Validation for Faculty Wise
                                    MessageModel resultCheckEligibleValidation = CheckEligibleValidation(sscResultModel, hscResultModel);

                                    if (resultCheckEligibleValidation != null && resultCheckEligibleValidation.MessageBoolean == true)
                                    {

                                        ////DAL.ExamDetail sscED = null;
                                        ////DAL.ExamDetail hscED = null;
                                        //using (var db = new GeneralDataManager())
                                        //{
                                        //    //sscED = db.AdmissionDB.ExamDetails.Where(x=> x.RollNo == sscResultModel.rollNo && x.bo)

                                        //    var sscED = (from ed in db.AdmissionDB.ExamDetails
                                        //                 join board in db.AdmissionDB.EducationBoards on ed.EducationBoardID equals board.ID
                                        //                 where ed.RollNo == sscResultModel.rollNo
                                        //                 && ed.PassingYear.ToString() == sscResultModel.passYear
                                        //                 && board.BoardName.ToLower() == sscResultModel.board.ToLower()
                                        //                 select new { ed }).FirstOrDefault();

                                        //    var hscED = (from ed in db.AdmissionDB.ExamDetails
                                        //                 join board in db.AdmissionDB.EducationBoards on ed.EducationBoardID equals board.ID
                                        //                 where ed.RollNo == hscResultModel.rollNo
                                        //                 && ed.PassingYear.ToString() == hscResultModel.passYear
                                        //                 && board.BoardName.ToLower() == hscResultModel.board.ToLower()
                                        //                 select new { ed }).FirstOrDefault();

                                        //    if (sscED != null && hscED != null)
                                        //    {
                                        //        string messageBody = "Dear " + sscResultModel.name + "<br/>" +
                                        //                            "You have already purchase an application and fill up your application.<br/>" +
                                        //                            "If you want to buy other Faculty forms, for that you need to login again with your credential in Admission system.<br/>" +
                                        //                            "Then go to Purchase More Application Section and you will able to buy other faculties. <br/> Thanks.";
                                        //        MessageView(messageBody, "success");
                                        //        return;
                                        //    }
                                        //}


                                        //....Show Button Section
                                        panelSubmitButtonSection.Visible = true;

                                        ClearSSCHSCSection();
                                        panelSSCHSCInputSection.Visible = false;

                                        panelBasicInfo.Visible = true;

                                        //PurchaseApplicationForm();
                                        FillUpInformation(sscResultModel, hscResultModel);


                                        MessageView("Your education information is verified. Please continue to fill up your application.", "success");


                                        #region View Message: IF there is any Eligible & Not-Eligible List

                                        List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleNotEligibleList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();

                                        if (Session["Eligible_NotEligible_List"] != null)
                                        {
                                            eligibleNotEligibleList = (List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>)Session["Eligible_NotEligible_List"];
                                        }

                                        if (eligibleNotEligibleList != null && eligibleNotEligibleList.Count > 0)
                                        {

                                            string messageBody = "<h1>Eligible & Not Eligible List</h1> <br/> <ul>";

                                            foreach (var tData in eligibleNotEligibleList)
                                            {
                                                messageBody = "<li style='color: crimson; font-weight:bold; '>" + tData.Attribute1 + " (" + tData.Attribute3 + ")";
                                            }
                                            messageBody += "</ul>";

                                            //PanelMessageEligibleNotEligibleList.Visible = true;
                                            //LabelMessageEligibleNotEligible.Text = messageBody;
                                            //PanelMessageEligibleNotEligibleList.CssClass = "alert alert-info";
                                        }
                                        #endregion

                                    }
                                    else
                                    {
                                        btnSubmit.Visible = false;
                                        MessageView(resultCheckEligibleValidation.MessageBody, "fail");
                                    }
                                }
                                else
                                {
                                    MessageView("SSC and HSC Board name did not match with our system. Please contact with Administrator !!", "fail");
                                    return;
                                }

                            }
                            else
                            {
                                MessageView("Failed to GET SSC Education Data !! Message: " + sscResultModel.responseDesc.ToString(), "fail");
                                return;
                            }
                        }
                        else
                        {
                            MessageView("Failed to GET SSC Education Data", "fail");
                            return;
                        }
                        #endregion




                    }
                    #region N/A
                    //else if (sscResultModel.responseCode == -99 || hscResultModel.responseCode == -99)
                    //{
                    //    string msg = "";
                    //    if (sscResultModel.responseCode == -99 && !string.IsNullOrEmpty(sscResultModel.responseDesc))
                    //    {
                    //        msg = msg + "Error(SSC) : " + sscResultModel.responseDesc.ToString() + "; ";
                    //    }

                    //    if (hscResultModel.responseCode == -99 && !string.IsNullOrEmpty(hscResultModel.responseDesc))
                    //    {
                    //        msg = msg + "Error(HSC) : " + hscResultModel.responseDesc.ToString();
                    //    }

                    //    MessageView("Exception: Failed to GET Education Data; " + msg.ToString(), "fail");
                    //} 
                    #endregion
                    else
                    {
                        MessageView("Failed to GET HSC Education Data !! Message: " + hscResultModel.responseDesc.ToString(), "fail");
                        return;
                    }

                }
                else
                {
                    MessageView("Message: Failed to GET HSC Education Data", "fail");
                    return;
                }
                #endregion

                //}//... END: if (resultCheckInputFieldValidation == true) 
                //else
                //{
                //    MessageView(resultCheckInputFieldValidation.MessageBody, "fail");
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageView("Exception: " + ex.Message.ToString(), "fail");
                return;
            }

        }



        #region O Level / A Level (2023) Appeared
        protected void btnOALevelAppeared_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    rblEducationChoice.ClearSelection();
                    rblEducationChoice.SelectedValue = "4"; // A Level Appeared
                    rblEducationChoice_SelectedIndexChanged(null, null);
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnCalculateALevelAppeared_Click(object sender, EventArgs e)
        {
            try
            {
                decimal resultOLevel = 0.00M;
                decimal resultALevel = 0.00M;
                decimal totalPoint = 0.00M;

                decimal totalPointToCheck = 0.00M;

                lblOLevelAppearedResult.Text = string.Empty;
                lblALevelAppearedResult.Text = string.Empty;

                lblMassageOALevelAppeared.Text = string.Empty;
                messagePanelOALevelAppeared.Visible = false;


                //-------------------------- O-Level -------------------------------
                decimal subject1 = Decimal.Parse(ddlOLevelAppearedSubject1.SelectedValue.ToString());
                decimal subject2 = Decimal.Parse(ddlOLevelAppearedSubject2.SelectedValue.ToString());
                decimal subject3 = Decimal.Parse(ddlOLevelAppearedSubject3.SelectedValue.ToString());
                decimal subject4 = Decimal.Parse(ddlOLevelAppearedSubject4.SelectedValue.ToString());
                decimal subject5 = Decimal.Parse(ddlOLevelAppearedSubject5.SelectedValue.ToString());

                resultOLevel = (subject1 + subject2 + subject3 + subject4 + subject5);

                lblOLevelAppearedResult.Text = resultOLevel.ToString();
                hfOLevelAppearedConvertedSscGPA.Value = (resultOLevel / 5).ToString();


                //-------------------------- A-Level -------------------------------
                decimal subject6 = 0, subject7 = 0;
                if (Convert.ToInt32(ddlALevelAppearedSubject1.SelectedValue) != -1)
                    Decimal.Parse(ddlALevelAppearedSubject1.SelectedValue.ToString());

                if (Convert.ToInt32(ddlALevelAppearedSubject2.SelectedValue) != -1)
                    subject7 = Decimal.Parse(ddlALevelAppearedSubject2.SelectedValue.ToString());

                resultALevel = (subject6 + subject7);

                lblALevelAppearedResult.Text = resultALevel.ToString();
                hfALevelAppearedConvertedHscGPA.Value = (resultALevel / 2).ToString();


                //-------------------------- Total Point To Check -------------------------------
                totalPoint = subject1 + subject2 + subject3 + subject4 + subject5 + subject6 + subject7;
                decimal[] point = { subject1, subject2, subject3, subject4, subject5, subject6, subject7 };

                if (totalPoint >= totalPointToCheck)
                {
                    lblTotalPointsAppeared.Text = totalPoint.ToString();

                    //....Show Button Section
                    panelSubmitButtonSection.Visible = true;
                }
                else
                {
                    //....Hide Button Section
                    panelSubmitButtonSection.Visible = false;

                    lblTotalPointsAppeared.Text = totalPoint.ToString();

                    lblMassageOALevelAppeared.Text = "You are not Eligible to apply. Minimum requirement is 0 points";
                    lblMassageOALevelAppeared.Attributes.CssStyle.Add("font-weight", "bold");
                    lblMassageOALevelAppeared.Attributes.CssStyle.Add("color", "crimson");

                    messagePanelOALevelAppeared.Visible = true;
                    messagePanelOALevelAppeared.CssClass = "alert alert-danger";

                    btnSubmit.Visible = false;
                    MessageView("You are not Eligible to Apply..!!", "fail");
                }

            }
            catch (Exception ex)
            {
                lblMassageOALevelAppeared.Text = "Message: " + ex.Message.ToString();
                lblMassageOALevelAppeared.Attributes.CssStyle.Add("font-weight", "bold");
                lblMassageOALevelAppeared.Attributes.CssStyle.Add("color", "crimson");

                messagePanelOALevelAppeared.Visible = true;
                messagePanelOALevelAppeared.CssClass = "alert alert-danger";

                btnSubmit.Visible = false;
            }
        }
        protected MessageModel CheckInputFieldValidationOALevelAppeared()
        {
            MessageModel mModel = new MessageModel();

            //bool result = true;

            Dictionary<int, string> messageList = new Dictionary<int, string>();

            int i = 1;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                messageList.Add(i++, "Full Name is Empty");
            }

            //if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            //{
            //    messageList.Add(i++, "Date Of Birth is Empty");
            //}

            if (ddlDay.SelectedValue == "-1" || ddlMonth.SelectedValue == "-1" || ddlYear.SelectedValue == "-1")
            {
                messageList.Add(i++, "Date of Birth hasn't Selected !!");
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                messageList.Add(i++, "Email is Empty");
            }

            if (string.IsNullOrEmpty(txtSmsMobile.Text))
            {
                messageList.Add(i++, "Mobile is Empty");
            }

            if (string.IsNullOrEmpty(txtGuardianMobile.Text))
            {
                messageList.Add(i++, "Guardian Mobile is Empty");
            }

            if (ddlGender.SelectedValue == "-1")
            {
                messageList.Add(i++, "Gender is not Selected");
            }

            //if (ddlQuota.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "Quota is not Selected");
            //}


            //----------------O-Level------------------
            if (string.IsNullOrEmpty(txtOLevelAppearedInstitute.Text))
            {
                messageList.Add(i++, "O-Level Institute is Empty");
            }
            if (ddlOLevelAppearedEducationBoard.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Education Board is not Selected");
            }
            if (ddlPassYearOLevelAppeared.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Passing Year is not Selected");
            }
            if (ddlOLevelAppearedSubject1.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject1 is not Selected");
            }
            if (ddlOLevelAppearedSubject2.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject2 is not Selected");
            }
            if (ddlOLevelAppearedSubject3.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject3 is not Selected");
            }
            if (ddlOLevelAppearedSubject4.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject4 is not Selected");
            }
            if (ddlOLevelAppearedSubject5.SelectedValue == "-1")
            {
                messageList.Add(i++, "O-Level Subject5 is not Selected");
            }

            //----------------A-Level------------------
            if (string.IsNullOrEmpty(txtALevelAppearedInstitute.Text))
            {
                messageList.Add(i++, "A-Level Institute is Empty");
            }
            if (ddlALevelAppearedEducationBoard.SelectedValue == "-1")
            {
                messageList.Add(i++, "A-Level Education Board is not Selected");
            }
            if (ddlPassYearALevelAppeared.SelectedValue == "-1")
            {
                messageList.Add(i++, "A-Level Passing Year is not Selected");
            }
            //if (ddlALevelSubject1.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "A-Level Subject1 is not Selected");
            //}
            //if (ddlALevelSubject2.SelectedValue == "-1")
            //{
            //    messageList.Add(i++, "A-Level Subject2 is not Selected");
            //}




            if (messageList != null && messageList.Count > 0)
            {
                string message = "";
                foreach (var msg in messageList)
                {
                    message = message + msg.Key.ToString() + ". " + msg.Value + "<br/>";
                }

                //MessageView(message, "fail");
                //result = false;

                mModel.MessageCode = 2;
                mModel.MessageBody = message;
                mModel.MessageBoolean = false;
            }
            else
            {
                mModel.MessageCode = 1;
                mModel.MessageBody = "";
                mModel.MessageBoolean = true;
            }


            return mModel;

        }

        #endregion

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                long CandidateUserId = Convert.ToInt64(hdnCandidateUserId.Value);
                hdnCandidateUserId.Value = "0";

                if (CandidateUserId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.CandidateUser candidateUser = db.GetCandidateUserByID_ND(CandidateUserId);

                        if (candidateUser != null)
                        {
                            SessionSGD.SaveObjToSession<long>(candidateUser.ID, SessionName.Common_UserId); //this is candidate user primary key
                            SessionSGD.SaveObjToSession<string>(candidateUser.UsernameLoginId, SessionName.Common_LoginID); //this is the username
                            SessionSGD.SaveObjToSession<string>("Candidate", SessionName.Common_RoleName);
                            Response.Redirect("~/Admission/Candidate/ApplicationBasic.aspx", false);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            modalPopupAlert.Show();
        }
    }
}