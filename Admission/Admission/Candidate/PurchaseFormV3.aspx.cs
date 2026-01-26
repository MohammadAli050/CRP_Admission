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
    public partial class PurchaseFormV3 : System.Web.UI.Page
    {
        string SessionLoginCaptcha = "SessionLoginCaptcha";

        protected void Page_Load(object sender, EventArgs e)
        {
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
                Session["BackFromPurchaseForm_Session"] = "1";

                //Session["Eligible_NotEligible_List"] = null;

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

                //txtName.Text = String.Empty;
                ////txtDateOfBirth.Text = String.Empty;
                //txtEmail.Text = String.Empty;
                //txtSmsMobile.Text = String.Empty;
                //txtGuardianMobile.Text = String.Empty;
                //ddlGender.SelectedIndex = -1;
                ////ddlQuota.SelectedIndex = -1;
                ////ddlPassingYear.SelectedIndex = -1;
                //ddlExamTypeSSC.SelectedIndex = -1;
                //ddlGroupSSC.SelectedIndex = -1;
                //txtGPASSC.Text = String.Empty;
                //ddlPassYearSSC.SelectedIndex = -1;
                //ddlExamTypeHSC.SelectedIndex = -1;
                //ddlGroupHSC.SelectedIndex = -1;
                //txtGPAHSC.Text = String.Empty;
                //ddlPassYearHSC.SelectedIndex = -1;

                //ddlDay.SelectedIndex = -1;
                //ddlMonth.SelectedIndex = -1;
                //ddlYear.SelectedIndex = -1;
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

        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
            //{
            //    LoadCaptcha();
            //    captchaMsg.Visible = true;
            //    return;
            //}


            //#region gets admissionsetupid and admissionunitid

            //int admissionSetupIDLong = -1;
            //int admissionUnitIDLong = -1;
            //if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
            //{
            //    Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            //}
            //else
            //{
            //    admissionSetupIDLong = Convert.ToInt32(Request.QueryString["asi"]);
            //    admissionUnitIDLong = Convert.ToInt32(Request.QueryString["aui"]);
            //}

            //#endregion


            //DAL.AdmissionSetup admSet = new DAL.AdmissionSetup();
            //using (var db = new GeneralDataManager())
            //{
            //    admSet = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == admissionSetupIDLong).FirstOrDefault();
            //}

            //Dictionary<int, string> dictErrorList = new Dictionary<int, string>();

            //if (admSet != null && admSet.EducationCategoryID == 4)
            //{
            //    #region Check all field is fillup in Form
            //    dictErrorList = ValidateAllFieldIsGiven();

            //    if (dictErrorList.Count > 0)
            //    {
            //        string massageError = "";
            //        foreach (var tData in dictErrorList)
            //        {
            //            massageError = massageError + tData.Key.ToString() + ") " + tData.Value.ToString() + "<br/>";
            //        }

            //        MessageView(massageError, "fail");
            //        return;
            //    }
            //    #endregion


            //    #region Get Values From Field
            //    int sscExamType = -1;
            //    int sscGroup = -1;
            //    decimal sscGPA = 0.00M;
            //    int sscPassingYear = -1;

            //    int hscExamType = -1;
            //    int hscGroup = -1;
            //    decimal hscGPA = 0.00M;
            //    int hscPassingYear = -1;



            //    sscExamType = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
            //    sscGroup = Convert.ToInt32(ddlGroupSSC.SelectedValue);
            //    sscGPA = Convert.ToDecimal(txtGPASSC.Text);
            //    sscPassingYear = Convert.ToInt32(ddlPassYearSSC.SelectedValue);

            //    hscExamType = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
            //    hscGroup = Convert.ToInt32(ddlGroupHSC.SelectedValue);
            //    hscGPA = Convert.ToDecimal(txtGPAHSC.Text);
            //    hscPassingYear = Convert.ToInt32(ddlPassYearHSC.SelectedValue);
            //    #endregion


            //    if (sscExamType == 5 && hscExamType == 7)    // O/A Level
            //    {
            //        oLevelExamType = 5;
            //        aLevelExamType = 7;

            //        PurchaseApplicationForm();
            //    }
            //    else //if (sscExamType == 1 && hscExamType == 2)       // SSC / HSC
            //    {
            //        //.... Check Eligible Validation for Faculty Wise
            //        MessageModel resultCheckEligibleValidation = CheckEligibleValidation(sscExamType, sscGroup, sscGPA, hscExamType, hscGroup, hscGPA);

            //        if (resultCheckEligibleValidation != null && resultCheckEligibleValidation.MessageBoolean == true)
            //        {
            //            PurchaseApplicationForm();
            //        }
            //        else
            //        {
            //            btnSubmit.Visible = false;
            //            MessageView(resultCheckEligibleValidation.MessageBody, "fail");
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
            //    Response.Redirect("~/Admission/Message.aspx?message=You don't have Permission to acccess that Page&type=danger", false);
            //}

            ////if (admissionUnitIDLong == 2 || admissionUnitIDLong == 3 || admissionUnitIDLong == 4 || admissionUnitIDLong == 5)
            ////{
            ////    int sscExamType = -1;
            ////    int sscGroup = -1;
            ////    decimal sscGPA = 0.00M;
            ////    int sscPassingYear = -1;

            ////    int hscExamType = -1;
            ////    int hscGroup = -1;
            ////    decimal hscGPA = 0.00M;
            ////    int hscPassingYear = -1;

            ////    bool sscIsDecimal = false;
            ////    bool hscIsDecimal = false;


            ////    sscExamType = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
            ////    sscGroup = Convert.ToInt32(ddlGroupSSC.SelectedValue);
            ////    sscGPA = Convert.ToDecimal(txtGPASSC.Text);
            ////    sscPassingYear = Convert.ToInt32(ddlPassYearSSC.SelectedValue);

            ////    hscExamType = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
            ////    hscGroup = Convert.ToInt32(ddlGroupHSC.SelectedValue);
            ////    hscGPA = Convert.ToDecimal(txtGPAHSC.Text);
            ////    hscPassingYear = Convert.ToInt32(ddlPassYearHSC.SelectedValue);


            ////    List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admSscList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();
            ////    List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admHscList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();


            ////    sscIsDecimal = IsNumeric(sscGPA.ToString());
            ////    hscIsDecimal = IsNumeric(hscGPA.ToString());
            ////    if (sscIsDecimal == false || hscIsDecimal == false)
            ////    {
            ////        MessageView("Your SSC or HSC GPA is not in Correct Format..!!", "fail");
            ////        return;
            ////    }

            ////    DAL.CandidatePayment candidatePaymentT = new DAL.CandidatePayment();
            ////    List<DAL.CandidateFormSl> candidateFormSlT = new List<DAL.CandidateFormSl>();
            ////    List<DAL.AdmissionUnit> admUnitObjT = new List<DAL.AdmissionUnit>();

            ////    if (Session["CandidateFormSerial_Session"] != null && Session["CandidatePayment_Session"] != null &&
            ////    Session["AdmUnitObj_Session"] != null)
            ////    {

            ////        candidatePaymentT = (DAL.CandidatePayment)Session["CandidatePayment_Session"];

            ////        candidateFormSlT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];
            ////        //DAL.CandidateFormSl candidateFormSlTT = new DAL.CandidateFormSl();
            ////        //candidateFormSlTT = candidateFormSlT.FirstOrDefault();

            ////        admUnitObjT = (List<DAL.AdmissionUnit>)Session["AdmUnitObj_Session"];
            ////        //DAL.AdmissionUnit admUnitObjTT = new DAL.AdmissionUnit();
            ////        //admUnitObjTT = admUnitObjT.FirstOrDefault();
            ////    }


            ////    List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();
            ////    //List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> eligibleHSCList = new List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result>();





            ////    if (sscExamType == 5 && hscExamType == 7)    // O/A Level
            ////    {
            ////        oLevelExamType = 5;
            ////        aLevelExamType = 7;
            ////        PurchaseApplicationForm();
            ////    }
            ////    //if (admSscList.Count > 0 && admHscList.Count > 0)
            ////    else //if (sscExamType == 1 && hscExamType == 2)       // SSC / HSC
            ////    {

            ////        #region Validation Check For SSC
            ////        if (admissionUnitIDLong > 0 && sscExamType > 0 && sscGroup > 0 && sscGPA > 0)
            ////        {
            ////            try
            ////            {
            ////                //using (var db = new OfficeDataManager())
            ////                //{
            ////                //    admSscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(admissionUnitIDLong, sscExamType, sscGroup, sscGPA).ToList();
            ////                //}

            ////                if (admUnitObjT.Count() > 0)
            ////                {
            ////                    foreach (DAL.AdmissionUnit item in admUnitObjT)
            ////                    {
            ////                        DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result tempEligibleSSCList = new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result();
            ////                        using (var db = new OfficeDataManager())
            ////                        {

            ////                            admSscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(item.ID, sscExamType, sscGroup, sscGPA).ToList();
            ////                            if (admSscList.Count > 0)
            ////                            {
            ////                                tempEligibleSSCList.ID = admSscList[0].ID;
            ////                                tempEligibleSSCList.AdmissionUnitID = admSscList[0].AdmissionUnitID;
            ////                                tempEligibleSSCList.ExamTypeID = admSscList[0].ExamTypeID;
            ////                                tempEligibleSSCList.GroupID = admSscList[0].GroupID;
            ////                                tempEligibleSSCList.GPA = admSscList[0].GPA;

            ////                                eligibleList.Add(tempEligibleSSCList);
            ////                            }

            ////                        }

            ////                    }//end foreach
            ////                }// end if (admUnitObjT.Count() > 0)

            ////            }
            ////            catch (Exception ex)
            ////            {
            ////                //Console.WriteLine(ex.StackTrace);
            ////            }
            ////        }
            ////        #endregion

            ////        #region Validation Check HSC
            ////        if (admissionUnitIDLong > 0 && hscExamType > 0 && hscGroup > 0 && hscGPA > 0)
            ////        {
            ////            try
            ////            {
            ////                //using (var db = new OfficeDataManager())
            ////                //{
            ////                //    admHscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(admissionUnitIDLong, hscExamType, hscGroup, hscGPA).ToList();
            ////                //}


            ////                if (admUnitObjT.Count() > 0)
            ////                {
            ////                    foreach (DAL.AdmissionUnit item in admUnitObjT)
            ////                    {
            ////                        DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result tempEligibleHSCList = new DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result();
            ////                        using (var db = new OfficeDataManager())
            ////                        {

            ////                            admHscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(item.ID, hscExamType, hscGroup, hscGPA).ToList();
            ////                            if (admHscList.Count > 0)
            ////                            {
            ////                                tempEligibleHSCList.ID = admHscList[0].ID;
            ////                                tempEligibleHSCList.AdmissionUnitID = admHscList[0].AdmissionUnitID;
            ////                                tempEligibleHSCList.ExamTypeID = admHscList[0].ExamTypeID;
            ////                                tempEligibleHSCList.GroupID = admHscList[0].GroupID;
            ////                                tempEligibleHSCList.GPA = admHscList[0].GPA;

            ////                                eligibleList.Add(tempEligibleHSCList);
            ////                            }

            ////                        }

            ////                    }//end foreach
            ////                }// end if (admUnitObjT.Count() > 0)



            ////            }
            ////            catch (Exception ex)
            ////            {
            ////                //Console.WriteLine(ex.StackTrace);
            ////            }
            ////        } 
            ////        #endregion



            ////        DAL.AdmissionSetup admSetupTemp = new DAL.AdmissionSetup();
            ////        DAL.CandidateFormSl canForTemp = new DAL.CandidateFormSl();


            ////        foreach (DAL.AdmissionUnit item in admUnitObjT)
            ////        {
            ////            if (eligibleList.Where(x => x.AdmissionUnitID == item.ID).ToList().Count == 2)
            ////            {

            ////            }
            ////            else
            ////            {

            ////                canForTemp = candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault();
            ////                try
            ////                {
            ////                    using (var db = new OfficeDataManager())
            ////                    {
            ////                        admSetupTemp = db.AdmissionDB.AdmissionSetups.Find(canForTemp.AdmissionSetupID);
            ////                    }
            ////                }
            ////                catch (Exception)
            ////                {

            ////                }



            ////                //----removing faculty which one is not eligible
            ////                candidateFormSlT.Remove(candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault());



            ////                //--- removing faculty amount which one is not wligible
            ////                candidatePaymentT.Amount = candidatePaymentT.Amount - admSetupTemp.Fee;
            ////            }
            ////        }


            ////        Session["CandidatePayment_Session"] = candidatePaymentT;
            ////        DAL.CandidatePayment candidatePaymentTT = new DAL.CandidatePayment();
            ////        candidatePaymentTT = (DAL.CandidatePayment)Session["CandidatePayment_Session"];

            ////        Session["CandidateFormSerial_Session"] = candidateFormSlT;
            ////        List<DAL.CandidateFormSl> candidateFormSlTT = new List<DAL.CandidateFormSl>();
            ////        candidateFormSlTT = (List<DAL.CandidateFormSl>)Session["CandidateFormSerial_Session"];


            ////        if (candidateFormSlTT.Count > 0 && candidatePaymentTT.Amount > 0)
            ////        {
            ////            PurchaseApplicationForm();
            ////        }
            ////        else
            ////        {
            ////            btnSubmit.Visible = false;
            ////            MessageView("You are not Eligible to Apply..!!", "fail");
            ////        }

            ////    }
            ////    #region N/A
            ////    //else if (sscExamType == 6 && hscExamType == 8)    // Dakhil/Alim
            ////    //{
            ////    //    PurchaseApplicationForm();
            ////    //}
            ////    //else if (sscExamType == 1 && hscExamType == 8)    // SSC / Alim
            ////    //{
            ////    //    PurchaseApplicationForm();
            ////    //}
            ////    //else if (sscExamType == 6 && hscExamType == 2)    //Dakhil / HSC
            ////    //{
            ////    //    PurchaseApplicationForm();
            ////    //}

            ////    //else
            ////    //{
            ////    //    btnSubmit.Visible = false;
            ////    //    EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
            ////    //} 
            ////    #endregion
            ////}
            ////else
            ////{
            ////    PurchaseApplicationForm();
            ////}

        }

    }
}