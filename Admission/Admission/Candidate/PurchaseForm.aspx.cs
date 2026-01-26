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
    public partial class PurchaseForm : System.Web.UI.Page
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
                        return;
                    }

                    if (admissionSetup.EducationCategoryID == 4)
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
                        return;
                    }

                }
            }

            if (!IsPostBack)
            {

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
                //undergraduateInfoPanel.Visible = ShowUndergraduatePanel();
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


        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.GroupOrSubject> groupSubjectList = db.AdmissionDB.GroupOrSubjects.Where(a => a.IsActive == true).ToList();
                List<DAL.EducationBoard> educationBoardList = db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true).ToList();

                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).ToList(), "Remarks", "ID", EnumCollection.ListItemType.Quota);

                DDLHelper.Bind<DAL.EducationBoard>(ddlBoardSSC, db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true && a.IsVisual == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                DDLHelper.Bind<DAL.EducationBoard>(ddlBoardHSC, db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true && a.IsVisual == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                List<DAL.UndergradGradProgram> undergradGradProgramList = db.GetAllUndergradGradProgram_ND();
                if (undergradGradProgramList != null && undergradGradProgramList.Any())
                {
                    DDLHelper.Bind<DAL.UndergradGradProgram>(ddlUndergradProgramDegree, undergradGradProgramList.Where(a => a.EducationCategoryID == 4 && a.IsActive == true).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Program);
                    DDLHelper.Bind<DAL.UndergradGradProgram>(ddlGradProgramDegree, undergradGradProgramList.Where(a => a.EducationCategoryID == 6 && a.IsActive == true).ToList(), "ProgramName", "ID", EnumCollection.ListItemType.Program);
                }

                List<DAL.ResultDivision> resultDivisionList = db.AdmissionDB.ResultDivisions.Where(a => a.IsActive == true && a.ID != 4).ToList();

                if (resultDivisionList != null && resultDivisionList.Count > 0)
                {
                    DDLHelper.Bind<DAL.ResultDivision>(ddlSSCDivisionClass, resultDivisionList.Where(a => a.IsActive == true && a.ID != 6).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlHSCDivisionClass, resultDivisionList.Where(a => a.IsActive == true && a.ID != 6).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlUndergradDivisionClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                    DDLHelper.Bind<DAL.ResultDivision>(ddlGradDivisionClass, resultDivisionList.Where(a => a.IsActive == true).ToList(), "ResultDivisionName", "ID", EnumCollection.ListItemType.Select);
                }

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

            #region SSC, HSC, Undergrad, Grad Passing Year
            ddlPassYearSSC.Items.Clear();
            ddlPassYearSSC.Items.Add(new ListItem("Select", "-1"));
            ddlPassYearSSC.AppendDataBoundItems = true;

            ddlPassYearHSC.Items.Clear();
            ddlPassYearHSC.Items.Add(new ListItem("Select", "-1"));
            ddlPassYearHSC.AppendDataBoundItems = true;

            ddlUndergradPassingYear.Items.Clear();
            ddlUndergradPassingYear.Items.Add(new ListItem("Select", "-1"));
            ddlUndergradPassingYear.AppendDataBoundItems = true;

            ddlGradPassingYear.Items.Clear();
            ddlGradPassingYear.Items.Add(new ListItem("Select", "-1"));
            ddlGradPassingYear.AppendDataBoundItems = true;

            for (int i = DateTime.Now.Year; i >= 1980; i--)
            {
                ddlPassYearSSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlPassYearHSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlUndergradPassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlGradPassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            #endregion


            //DAL.AdmissionSetup admissionSetup = null;
            //DAL.AdmissionUnit admissionUnit = null;

            //long admissionSetupIDLong = -1;
            //long admissionUnitIDLong = -1;

            //if ((string.IsNullOrEmpty(Request.QueryString["asi"])) || (string.IsNullOrEmpty(Request.QueryString["aui"])))
            //{
            //    Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            //}
            //else
            //{
            //    admissionSetupIDLong = Convert.ToInt64(Request.QueryString["asi"]);
            //    admissionUnitIDLong = Convert.ToInt64(Request.QueryString["aui"]);
            //}
            //try
            //{
            //    using (var db = new OfficeDataManager())
            //    {
            //        admissionSetup = db.AdmissionDB.AdmissionSetups.Find(admissionSetupIDLong);
            //        admissionUnit = db.AdmissionDB.AdmissionUnits.Find(admissionUnitIDLong);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}//end inner try-catch 1

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


            //        hideCalculater.Visible = false;
            //        hideSSC.Visible = false;
            //        hideHSC.Visible = false;

            //        LoadPassingYearDDLForMasters();
            //    }
            //}

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

        protected void ddlExamTypeSSC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int temp = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
                if (temp == 5) // for O-Level
                {
                    ddlExamTypeHSC.SelectedValue = "7";
                    ddlGroupSSCComV.Visible = false;
                    //RegularExpressionValidator2.Visible = false;
                    ddlGroupHSCComV.Visible = false;
                    //YourRegularExpressionValidator.Visible = false;
                    ModalPopupExtender.Show();
                }
                else
                {
                    ddlExamTypeHSC.SelectedValue = "-1";
                    ddlGroupSSCComV.Visible = true;
                    //RegularExpressionValidator2.Visible = true;
                    ddlGroupHSCComV.Visible = true;
                    //YourRegularExpressionValidator.Visible = true;
                    txtGPASSC.Text = "";
                    txtGPAHSC.Text = "";
                    txtGPASSC.Enabled = true;
                    txtGPAHSC.Enabled = true;
                    ddlGroupSSC.Enabled = true;
                    ddlGroupHSC.Enabled = true;
                }
            }
            catch (Exception ex)
            {

            }
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


            //----------------SSC------------------
            if (string.IsNullOrEmpty(txtInstituteSSC.Text.Trim()))
            {
                messageList.Add(i++, "SSC Institute is Empty");
            }
            if (ddlBoardSSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Board is not Selected");
            }
            if (ddlExamTypeSSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Exam Type is not Selected");
            }
            if (ddlGroupSSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Group is not Selected");
            }

            if (ddlSSCDivisionClass.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Division/Class is not Selected");
            }

            if (Convert.ToInt32(ddlSSCDivisionClass.SelectedValue) > 0 && Convert.ToInt32(ddlSSCDivisionClass.SelectedValue) == 5)
            {
                if (txtGPASSC.Text.ToString() == "")
                {
                    messageList.Add(i++, "SSC GPA is Empty !!");
                }

                if (!string.IsNullOrEmpty(txtGPASSC.Text))
                {
                    decimal sscCGPA = Convert.ToDecimal(txtGPASSC.Text);

                    if (sscCGPA > 0 && sscCGPA <= 5.00M)
                    {

                    }
                    else
                    {
                        messageList.Add(i++, "SSC GPA should be within 5 !");
                    }
                }
            }

            if (ddlPassYearSSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Passing Year is not Selected");
            }


            //----------------HSC------------------
            if (string.IsNullOrEmpty(txtInstituteHSC.Text.Trim()))
            {
                messageList.Add(i++, "HSC Institute is Empty");
            }
            if (ddlBoardHSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Board is not Selected");
            }
            if (ddlExamTypeHSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Examination is not Selected");
            }
            if (ddlGroupHSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Group is not Selected");
            }
            if (ddlHSCDivisionClass.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Division/Class is not Selected");
            }

            if (Convert.ToInt32(ddlHSCDivisionClass.SelectedValue) > 0 && Convert.ToInt32(ddlHSCDivisionClass.SelectedValue) == 5)
            {
                if (txtGPAHSC.Text.Trim().ToString() == "")
                {
                    messageList.Add(i++, "HSC GPA is Empty !!");
                }

                if (!string.IsNullOrEmpty(txtGPAHSC.Text.Trim()))
                {
                    decimal hscCGPA = Convert.ToDecimal(txtGPAHSC.Text.Trim());

                    if (hscCGPA > 0 && hscCGPA <= 5.00M)
                    {

                    }
                    else
                    {
                        messageList.Add(i++, "HSC GPA should be within 5 !");
                    }
                }
            }
            if (ddlPassYearHSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Passing Year is not Selected");
            }


            //----------------Undergrad------------------
            if (string.IsNullOrEmpty(txtUndergradInstitute.Text.Trim()))
            {
                messageList.Add(i++, "Bachelor Institute is Empty");
            }
            if (ddlUndergradProgramDegree.SelectedValue == "-1")
            {
                messageList.Add(i++, "Bachelor Program/Degree is not Selected");
            }
            if (Convert.ToInt32(ddlUndergradProgramDegree.SelectedValue) > 0 && Convert.ToInt32(ddlUndergradProgramDegree.SelectedValue) == 55)
            {
                if (string.IsNullOrEmpty(txtUndergradProgOthers.Text.Trim()))
                {
                    messageList.Add(i++, "Bachelor Others is Empty");
                }
            }
            if (ddlHSCDivisionClass.SelectedValue == "-1")
            {
                messageList.Add(i++, "Bachelor Division/Class is not Selected");
            }

            if (Convert.ToInt32(ddlUndergradDivisionClass.SelectedValue) > 0 && Convert.ToInt32(ddlUndergradDivisionClass.SelectedValue) == 5)
            {
                if (txtUndergradCGPA.Text.Trim().ToString() == "")
                {
                    messageList.Add(i++, "Bachelor CGPA is Empty !!");
                }

                if (!string.IsNullOrEmpty(txtUndergradCGPA.Text.Trim()))
                {
                    decimal undergradCGPA = Convert.ToDecimal(txtUndergradCGPA.Text.Trim());

                    if (undergradCGPA > 0 && undergradCGPA <= 4.00M)
                    {

                    }
                    else
                    {
                        messageList.Add(i++, "Bachelor CGPA should be within 5 !");
                    }
                }
            }
            if (ddlUndergradPassingYear.SelectedValue == "-1")
            {
                messageList.Add(i++, "Bachelor Passing Year is not Selected");
            }



            //----------------Grad------------------
            if (!string.IsNullOrEmpty(txtGradInstitute.Text.Trim()))
            {
                if (string.IsNullOrEmpty(txtGradInstitute.Text.Trim()))
                {
                    messageList.Add(i++, "Masters Institute is Empty");
                }
                if (ddlGradProgramDegree.SelectedValue == "-1")
                {
                    messageList.Add(i++, "Masters Program/Degree is not Selected");
                }
                if (Convert.ToInt32(ddlGradProgramDegree.SelectedValue) > 0 && Convert.ToInt32(ddlGradProgramDegree.SelectedValue) == 100)
                {
                    if (string.IsNullOrEmpty(txtGradProgOthers.Text.Trim()))
                    {
                        messageList.Add(i++, "Masters Others is Empty");
                    }
                }
                if (ddlGradDivisionClass.SelectedValue == "-1")
                {
                    messageList.Add(i++, "Masters Division/Class is not Selected");
                }

                if (Convert.ToInt32(ddlGradDivisionClass.SelectedValue) > 0 && Convert.ToInt32(ddlGradDivisionClass.SelectedValue) == 5)
                {
                    if (txtGradCGPA.Text.Trim().ToString() == "")
                    {
                        messageList.Add(i++, "Masters CGPA is Empty !!");
                    }

                    if (!string.IsNullOrEmpty(txtGradCGPA.Text.Trim()))
                    {
                        decimal gradCGPA = Convert.ToDecimal(txtGradCGPA.Text.Trim());

                        if (gradCGPA > 0 && gradCGPA <= 4.00M)
                        {

                        }
                        else
                        {
                            messageList.Add(i++, "Masters CGPA should be within 5 !");
                        }
                    }
                }
                if (ddlGradPassingYear.SelectedValue == "-1")
                {
                    messageList.Add(i++, "Masters Passing Year is not Selected");
                }
            }




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
                mModel.MessageCode = 1;
                mModel.MessageBody = "";
                mModel.MessageBoolean = true;
            }


            return mModel;

        }



        #region Get Points
        protected int GetSSCHSCResultPoint(decimal gpa, int divisionId)
        {
            int result = -1;

            if (divisionId == 5)
            {
                // SSC & HSC GPA
                if (gpa >= 3.50M && gpa <= 5.00M)
                {
                    result = 3;
                }
                else if (gpa >= 3.00M && gpa <= 3.49M)
                {
                    result = 2;
                }
                else if (gpa >= 2.50M && gpa <= 2.99M)
                {
                    result = 1;
                }
                else if (gpa <= 2.49M)
                {
                    result = 0;
                }
            }
            else if (divisionId == 2)
            {
                // SSC & HSC 1st Division
                result = 3;
            }
            else if (divisionId == 3)
            {
                // SSC & HSC 2nd Division
                result = 2;
            }
            else
            {
                result = 0;
            }

            return result;
        }

        protected int GetBachelorResultPoint(decimal cgpa, int divisionId, int bachelorOrBachelorPass)
        {
            int result = -1;

            if (divisionId == 5)
            {
                if (bachelorOrBachelorPass == 1)
                {
                    // Undergrad CGPA
                    if (cgpa >= 3.50M && cgpa <= 4.00M)
                    {
                        result = 5;
                    }
                    else if (cgpa >= 3.00M && cgpa <= 3.49M)
                    {
                        result = 4;
                    }
                    else if (cgpa >= 2.50M && cgpa <= 2.99M)
                    {
                        result = 3;
                    }
                    else if (cgpa >= 2.00M && cgpa <= 2.49M)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else
                {
                    // Undergrad (Pass) CGPA
                    if (cgpa >= 3.50M && cgpa <= 4.00M)
                    {
                        result = 4;
                    }
                    else if (cgpa >= 3.00M && cgpa <= 3.49M)
                    {
                        result = 3;
                    }
                    else if (cgpa >= 2.50M && cgpa <= 2.99M)
                    {
                        result = 2;
                    }
                    else if (cgpa >= 2.00M && cgpa <= 2.49M)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }

            }
            else if (divisionId == 2)
            {
                // Undergrad 1st Division
                result = 5;
            }
            else if (divisionId == 3)
            {
                // Undergrad 2nd Division
                result = 4;
            }
            else if (divisionId == 6)
            {
                // Passed
                result = 5;
            }
            else
            {
                result = 0;
            }

            return result;
        }
        #endregion


        protected MessageModel EligibleCheckCandidateResult()
        {

            MessageModel mModel = new MessageModel();


            Dictionary<int, string> messageList = new Dictionary<int, string>();

            int i = 1;

            #region Get Values From Field
            int sscExamType = -1;
            //int sscGroup = -1;
            int sscDivision = -1;
            decimal sscGPA = 0.00M;
            //int sscPassingYear = -1;

            int hscExamType = -1;
            //int hscGroup = -1;
            int hscDivision = -1;
            decimal hscGPA = 0.00M;
            //int hscPassingYear = -1;

            int undergradDivision = -1;
            int bachelorOrBachelorPass = -1;
            decimal undergradGPA = 0.00M;

            int gradDivision = -1;
            decimal gradGPA = 0.00M;



            sscExamType = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
            //sscGroup = Convert.ToInt32(ddlGroupSSC.SelectedValue);
            sscDivision = Convert.ToInt32(ddlSSCDivisionClass.SelectedValue);

            sscGPA = (!string.IsNullOrEmpty(txtGPASSC.Text.Trim()) && Convert.ToDecimal(txtGPASSC.Text.Trim()) > 0) ? Convert.ToDecimal(txtGPASSC.Text.Trim()) : 0.00M;
            //sscPassingYear = Convert.ToInt32(ddlPassYearSSC.SelectedValue);


            hscExamType = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
            //hscGroup = Convert.ToInt32(ddlGroupHSC.SelectedValue);
            hscDivision = Convert.ToInt32(ddlHSCDivisionClass.SelectedValue);

            hscGPA = (!string.IsNullOrEmpty(txtGPAHSC.Text.Trim()) && Convert.ToDecimal(txtGPAHSC.Text.Trim()) > 0) ? Convert.ToDecimal(txtGPAHSC.Text.Trim()) : 0.00M;
            //hscPassingYear = Convert.ToInt32(ddlPassYearHSC.SelectedValue);

            bachelorOrBachelorPass = Convert.ToInt32(rbBachelorOrBachelorPass.SelectedValue);
            undergradDivision = Convert.ToInt32(ddlUndergradDivisionClass.SelectedValue);

            undergradGPA = (!string.IsNullOrEmpty(txtUndergradCGPA.Text.Trim()) && Convert.ToDecimal(txtUndergradCGPA.Text.Trim()) > 0) ? Convert.ToDecimal(txtUndergradCGPA.Text.Trim()) : 0.00M;


            int flagHasGradeValue = -1;
            try
            {
                if (!string.IsNullOrEmpty(txtGradInstitute.Text.Trim()) &&
                (!string.IsNullOrEmpty(txtGradCGPA.Text.Trim())) && Convert.ToDecimal(txtGradCGPA.Text.Trim()) > 0)
                {
                    gradDivision = Convert.ToInt32(ddlGradDivisionClass.SelectedValue);
                    gradGPA = Convert.ToDecimal(txtGradCGPA.Text.Trim());

                    flagHasGradeValue = 1;
                }
            }
            catch (Exception ex)
            {
                flagHasGradeValue = -1;
            }
            #endregion


            int flag = 0;

            #region Check SSC & HSC GPA is less then 2.49, then not eligible
            try
            {
                if (sscDivision == 5 && hscDivision == 5)
                {
                    if (sscGPA <= 2.49M || hscGPA <= 2.49M)
                    {
                        //MessageView("You are not eligible for apply.", "fail");
                        //return;
                        messageList.Add(i++, "You are not eligible for program.");
                        flag = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                messageList.Add(i++, "Exception: " + ex.Message.ToString());
                flag = 1;
            }
            #endregion

            if (flag == 0)
            {
                int sscPoint = 0;
                int hscPoint = 0;
                int undergradPoint = 0;
                int gradPoint = 0;

                #region Get SSC & HSC Point
                try
                {
                    #region N/A
                    //if (sscExamType == 5 && hscExamType == 7)
                    //{
                    //    // O-Level & A-Level

                    //}
                    //else if ((sscExamType == 1 || sscExamType == 5 || sscExamType == 6 || sscExamType == 12 || sscExamType == 14) &&
                    //    (hscExamType == 2 || hscExamType == 7 || hscExamType == 8 || hscExamType == 9 || hscExamType == 13 || hscExamType == 15))
                    //{

                    //    sscPoint = GetSSCHSCResultPoint(sscGPA, sscDivision);
                    //    hscPoint = GetSSCHSCResultPoint(hscGPA, hscDivision);
                    //}
                    //else
                    //{
                    //    sscPoint = 0;
                    //    hscPoint = 0;
                    //} 
                    #endregion

                    sscPoint = GetSSCHSCResultPoint(sscGPA, sscDivision);
                    hscPoint = GetSSCHSCResultPoint(hscGPA, hscDivision);

                }
                catch (Exception ex)
                {
                    sscPoint = 0;
                    hscPoint = 0;
                }
                #endregion

                #region Get Undergrad Point
                try
                {
                    undergradPoint = GetBachelorResultPoint(undergradGPA, undergradDivision, bachelorOrBachelorPass);
                }
                catch (Exception ex)
                {
                    undergradPoint = 0;
                }
                #endregion

                #region Get Grad Point
                try
                {
                    if (flagHasGradeValue == 1)
                    {
                        gradPoint = 1;
                    }
                }
                catch (Exception ex)
                {
                    gradPoint = 0;
                }
                #endregion


                try
                {
                    decimal sumPoint = Convert.ToDecimal((sscPoint + hscPoint + undergradPoint + gradPoint));

                    if (sumPoint >= 8.00M)
                    {

                    }
                    else
                    {
                        messageList.Add(i++, "You are not eligible for program.");
                    }
                }
                catch (Exception ex)
                {
                    messageList.Add(i++, "Exception: " + ex.Message.ToString());
                }

            }


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
                mModel.MessageCode = 1;
                mModel.MessageBody = "";
                mModel.MessageBoolean = true;
            }


            return mModel;
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

            if (admSet != null)
            {
                if (admSet.EducationCategoryID == 4)
                {
                    Response.Redirect("~/Admission/Message.aspx?message=You don't have Permission to acccess that Page&type=danger", false);
                }
                else
                {
                    #region Check all field is fillup in Form
                    MessageModel resultCheckInputFieldValidation = CheckInputFieldValidationSSCHSC();

                    if (resultCheckInputFieldValidation != null && resultCheckInputFieldValidation.MessageBoolean == true)
                    {
                        MessageModel resultEligibleCheckCandidateResult = EligibleCheckCandidateResult();

                        if (resultEligibleCheckCandidateResult != null && resultEligibleCheckCandidateResult.MessageBoolean == true)
                        {
                            PurchaseApplicationForm();

                        }
                        else
                        {
                            MessageView(resultEligibleCheckCandidateResult.MessageBody, "fail");
                            return;
                        }
                    }
                    else
                    {
                        MessageView(resultCheckInputFieldValidation.MessageBody, "fail");
                        return;
                    }
                    #endregion





                    //#region Get Values From Field
                    //int sscExamType = -1;
                    //int sscGroup = -1;
                    //decimal sscGPA = 0.00M;
                    //int sscPassingYear = -1;

                    //int hscExamType = -1;
                    //int hscGroup = -1;
                    //decimal hscGPA = 0.00M;
                    //int hscPassingYear = -1;



                    //sscExamType = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
                    //sscGroup = Convert.ToInt32(ddlGroupSSC.SelectedValue);
                    //sscGPA = Convert.ToDecimal(txtGPASSC.Text);
                    //sscPassingYear = Convert.ToInt32(ddlPassYearSSC.SelectedValue);

                    //hscExamType = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
                    //hscGroup = Convert.ToInt32(ddlGroupHSC.SelectedValue);
                    //hscGPA = Convert.ToDecimal(txtGPAHSC.Text);
                    //hscPassingYear = Convert.ToInt32(ddlPassYearHSC.SelectedValue);
                    //#endregion


                    ///// <summary>
                    ///// Only SSC & HSC
                    ///// AND O-Level & A-Level
                    ///// Are allowed to apply
                    ///// </summary>

                    //if (sscExamType == 5 && hscExamType == 7)    // O/A Level
                    //{
                    //    oLevelExamType = 5;
                    //    aLevelExamType = 7;

                    //    PurchaseApplicationForm();
                    //}
                    //else if (
                    //    (sscExamType == 1 || sscExamType == 6 || sscExamType == 12) && (hscExamType == 2 || hscExamType == 8 || hscExamType == 13)
                    //) // SSC & HSC, Dakhil & Alim, SSC (Vocational) & HSC (Vocational)
                    //{
                    //    //.... Check Eligible Validation for Faculty Wise
                    //    MessageModel resultCheckEligibleValidation = CheckEligibleValidation(sscExamType, sscGroup, sscGPA, hscExamType, hscGroup, hscGPA);

                    //    if (resultCheckEligibleValidation != null && resultCheckEligibleValidation.MessageBoolean == true)
                    //    {
                    //        PurchaseApplicationForm();
                    //    }
                    //    else
                    //    {
                    //        btnSubmit.Visible = false;
                    //        MessageView(resultCheckEligibleValidation.MessageBody, "fail");
                    //    }
                    //}
                    //else
                    //{
                    //    btnSubmit.Visible = false;
                    //    MessageView("You are not allowed to apply. You are requested to contact with BUP Admission Helpline (09666 790 790)", "fail");
                    //}

                    //#region N/A
                    ////else if (sscExamType == 6 && hscExamType == 8)    // Dakhil/Alim
                    ////{
                    ////    PurchaseApplicationForm();
                    ////}
                    ////else if (sscExamType == 1 && hscExamType == 8)    // SSC / Alim
                    ////{
                    ////    PurchaseApplicationForm();
                    ////}
                    ////else if (sscExamType == 6 && hscExamType == 2)    //Dakhil / HSC
                    ////{
                    ////    PurchaseApplicationForm();
                    ////}

                    ////else
                    ////{
                    ////    btnSubmit.Visible = false;
                    ////    EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
                    ////} 
                    //#endregion



                }

            }
            else
            {
                Response.Redirect("~/Admission/Message.aspx?message=You don't have Permission to acccess that Page&type=danger", false);

            }

            //#region N/A
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
            //#endregion

        }

        protected void btnShowPopupClicked(object sender, EventArgs e)
        {
            ModalPopupExtender.Show();
        }

        protected void ddlUndergradProgramDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int undergradProgramDegreeId = Convert.ToInt32(ddlUndergradProgramDegree.SelectedValue);
                if (undergradProgramDegreeId > 0 && undergradProgramDegreeId == 55)
                {
                    txtUndergradProgOthers.Enabled = true;
                    rfvUndergradProgOthers.Enabled = true;
                }
                else
                {
                    txtUndergradProgOthers.Enabled = false;
                    rfvUndergradProgOthers.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlUndergradDivisionClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int undergradDivisionClassId = Convert.ToInt32(ddlUndergradDivisionClass.SelectedValue);
                if (undergradDivisionClassId > 0 && undergradDivisionClassId == 5)
                {
                    txtUndergradCGPA.Enabled = true;
                    revUndergradCGPA.Enabled = true;
                    rfvUndergradCGPA.Enabled = true;
                }
                else
                {
                    txtUndergradCGPA.Enabled = false;
                    revUndergradCGPA.Enabled = false;
                    rfvUndergradCGPA.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlGradProgramDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int gradProgramDegreeId = Convert.ToInt32(ddlGradProgramDegree.SelectedValue);
                if (gradProgramDegreeId > 0 && gradProgramDegreeId == 100)
                {
                    txtGradProgOthers.Enabled = true;
                    //rfvGradProgOthers.Enabled = true;
                }
                else
                {
                    txtGradProgOthers.Enabled = false;
                    //rfvGradProgOthers.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void ddlGradDivisionClass_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                int gradDivisionClassId = Convert.ToInt32(ddlGradDivisionClass.SelectedValue);
                if (gradDivisionClassId > 0 && gradDivisionClassId == 5)
                {
                    txtGradCGPA.Enabled = true;
                    //revGradCGPA.Enabled = true;
                    //rfvGradCGPA.Enabled = true;
                }
                else
                {
                    txtGradCGPA.Enabled = false;
                    //revGradCGPA.Enabled = false;
                    //rfvGradCGPA.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void ddlSSCDivisionClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int sSCDivisionClassId = Convert.ToInt32(ddlSSCDivisionClass.SelectedValue);
                if (sSCDivisionClassId > 0 && sSCDivisionClassId == 5)
                {
                    txtGPASSC.Enabled = true;
                    revGPASSC.Enabled = true;
                    rfvGPASSC.Enabled = true;
                }
                else
                {
                    txtGPASSC.Enabled = false;
                    revGPASSC.Enabled = false;
                    rfvGPASSC.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHSCDivisionClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int hSCDivisionClassId = Convert.ToInt32(ddlHSCDivisionClass.SelectedValue);
                if (hSCDivisionClassId > 0 && hSCDivisionClassId == 5)
                {
                    txtGPAHSC.Enabled = true;
                    revGPAHSC.Enabled = true;
                    rfvGPAHSC.Enabled = true;
                }
                else
                {
                    txtGPAHSC.Enabled = false;
                    revGPAHSC.Enabled = false;
                    rfvGPAHSC.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
        }










        protected void btnAdminAssignTestValue_Click(object sender, EventArgs e)
        {
            txtName.Text = "Test Name";
            //txtDateOfBirth.Text = "01/01/1995";

            ddlDay.SelectedValue = "01";
            ddlMonth.SelectedValue = "01";
            ddlYear.SelectedValue = "1995";

            txtEmail.Text = "mali131050@gmail.com";

            txtSmsMobile.Text = "01615040699";
            txtGuardianMobile.Text = "01615040699";

            //txtNationalIdOrBirthRegistration.Text = "123456";

            ddlGender.SelectedValue = "2";
            ddlQuota.SelectedValue = "7";

            txtInstituteSSC.Text = "XXX Institute";
            ddlBoardSSC.SelectedValue = "2";
            ddlExamTypeSSC.SelectedValue = "1";
            ddlGroupSSC.SelectedValue = "3";
            ddlSSCDivisionClass.SelectedValue = "5";
            txtGPASSC.Text = "5";
            ddlPassYearSSC.SelectedValue = "2010";


            txtInstituteHSC.Text = "YYY Institute";
            ddlBoardHSC.SelectedValue = "2";
            ddlExamTypeHSC.SelectedValue = "2";
            ddlGroupHSC.SelectedValue = "3";
            ddlHSCDivisionClass.SelectedValue = "5";
            txtGPAHSC.Text = "5";
            ddlPassYearHSC.SelectedValue = "2012";


            txtUndergradInstitute.Text = "PQR Institute";
            ddlUndergradProgramDegree.SelectedValue = "16";
            ddlUndergradDivisionClass.SelectedValue = "5";
            txtUndergradCGPA.Text = "4";
            ddlUndergradPassingYear.SelectedValue = "2017";


            txtGradInstitute.Text = "ABC Institute";
            ddlGradProgramDegree.SelectedValue = "72";
            ddlGradDivisionClass.SelectedValue = "5";
            txtGradCGPA.Text = "4";
            ddlGradPassingYear.SelectedValue = "2020";

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
        //    lblEligibleMsg.Text = msg;
        //    lblEligibleMsg.CssClass = css;
        //    lblEligibleMsg.Focus();
        //} 
        #endregion

        private void PurchaseApplicationForm()
        {

            //DAL.BasicInfo candExist = null;
            string stringDateOfBirth = DateFormateing();
            DateTime dob = DateTime.ParseExact(stringDateOfBirth, "dd/MM/yyyy", null);
            int genderId = Int32.Parse(ddlGender.SelectedValue);
            int quotaId = Int32.Parse(ddlQuota.SelectedValue);

            string smsMobile = txtCountryCodeSMSMobile.Text + txtSmsMobile.Text;

            //using (var db = new CandidateDataManager())
            //{
            //    candExist = db.AdmissionDB.BasicInfoes
            //        .Where(c =>
            //            c.DateOfBirth == dob &&
            //            c.SMSPhone == smsMobile.Trim().ToString()
            //            )
            //        .FirstOrDefault();
            //}

            #region N/A
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
                candidate.DateOfBirth = DateTime.ParseExact(stringDateOfBirth, "dd/MM/yyyy", null);
                candidate.CandidateUserID = candidateUserIdLong;
                candidate.IsActive = false;
                candidate.UniqueIdentifier = Guid.NewGuid();
                candidate.GenderID = Convert.ToInt32(ddlGender.SelectedValue);
                candidate.QuotaID = Convert.ToInt32(ddlQuota.SelectedValue) == 0 ? 1 : Convert.ToInt32(ddlGender.SelectedValue);
                candidate.CreatedBy = -99;
                candidate.DateCreated = DateTime.Now;

                string guardianPhone = txtCountryCodeGuardianMobile.Text + txtGuardianMobile.Text;
                candidate.GuardianPhone = guardianPhone.Trim().ToString(); //txtGuardianMobile.Text.Trim();

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


                #region Candidate Education


                long candidateID = 0;

                string instituteSSC = string.Empty;
                int boardSSC = -1;
                int examTypeSSC = -1;
                int groupSSC = -1;
                int divisionSSC = -1;
                decimal sscGPA = 0.00M;
                decimal olevelPoint = 0.00M;
                int passingYearSSC = -1;


                string instituteHSC = string.Empty;
                int boardHSC = -1;
                int examTypeHSC = -1;
                int groupHSC = -1;
                int divisionHSC = -1;
                decimal hscGPA = 0.00M;
                decimal alevelPoint = 0.00M;
                int passingYearHSC = -1;


                string instituteUndergrad = string.Empty;
                int programDegreeUndergrad = -1;
                string programDegreeOtherUndergrad = string.Empty;
                int divisionUndergrad = -1;
                decimal undergradCGPA = 0.00M;
                int passingYearUndergrad = -1;
                int bachelorOrBachelorPass = -1;


                string instituteGrad = string.Empty;
                int programDegreeGrad = -1;
                string programDegreeOtherGrad = string.Empty;
                int divisionGrad = -1;
                decimal gradCGPA = 0.00M;
                int passingYearGrad = -1;



                candidateID = candidateIdLong;


                //=== SSC
                instituteSSC = txtInstituteSSC.Text.Trim();
                boardSSC = Convert.ToInt32(ddlBoardSSC.SelectedValue);
                examTypeSSC = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
                groupSSC = Convert.ToInt32(ddlGroupSSC.SelectedValue);
                divisionSSC = Convert.ToInt32(ddlSSCDivisionClass.SelectedValue);
                sscGPA = (!string.IsNullOrEmpty(txtGPASSC.Text.Trim()) && Convert.ToDecimal(txtGPASSC.Text.Trim()) > 0) ? Convert.ToDecimal(txtGPASSC.Text.Trim()) : 0.00M;
                olevelPoint = lblOLevelResult.Text != "" ? Convert.ToDecimal(lblOLevelResult.Text) : olevelPoint;
                passingYearSSC = Convert.ToInt32(ddlPassYearSSC.SelectedValue);


                //=== HSC
                instituteHSC = txtInstituteHSC.Text.Trim();
                boardHSC = Convert.ToInt32(ddlBoardHSC.SelectedValue);
                examTypeHSC = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
                groupHSC = Convert.ToInt32(ddlGroupHSC.SelectedValue);
                divisionHSC = Convert.ToInt32(ddlHSCDivisionClass.SelectedValue);
                hscGPA = (!string.IsNullOrEmpty(txtGPAHSC.Text.Trim()) && Convert.ToDecimal(txtGPAHSC.Text.Trim()) > 0) ? Convert.ToDecimal(txtGPAHSC.Text.Trim()) : 0.00M;
                alevelPoint = lblALevelResult.Text != "" ? Convert.ToDecimal(lblALevelResult.Text) : alevelPoint;
                passingYearHSC = Convert.ToInt32(ddlPassYearHSC.SelectedValue);


                //=== Undergrad
                instituteUndergrad = txtUndergradInstitute.Text.Trim();
                programDegreeUndergrad = Convert.ToInt32(ddlUndergradProgramDegree.SelectedValue);
                if (Convert.ToInt32(ddlUndergradProgramDegree.SelectedValue) == 55)
                {
                    programDegreeOtherUndergrad = txtUndergradProgOthers.Text.Trim();
                }
                divisionUndergrad = Convert.ToInt32(ddlUndergradDivisionClass.SelectedValue);
                undergradCGPA = (!string.IsNullOrEmpty(txtUndergradCGPA.Text.Trim()) && Convert.ToDecimal(txtUndergradCGPA.Text.Trim()) > 0) ? Convert.ToDecimal(txtUndergradCGPA.Text.Trim()) : 0.00M;
                passingYearUndergrad = Convert.ToInt32(ddlUndergradPassingYear.SelectedValue);
                bachelorOrBachelorPass = Convert.ToInt32(rbBachelorOrBachelorPass.SelectedValue);


                //=== Grad
                instituteGrad = txtGradInstitute.Text.Trim();
                programDegreeGrad = Convert.ToInt32(ddlGradProgramDegree.SelectedValue);
                if (Convert.ToInt32(ddlGradProgramDegree.SelectedValue) == 100)
                {
                    programDegreeOtherGrad = txtGradProgOthers.Text.Trim();
                }
                divisionGrad = Convert.ToInt32(ddlGradDivisionClass.SelectedValue);

                gradCGPA = (!string.IsNullOrEmpty(txtGradCGPA.Text.Trim()) && Convert.ToDecimal(txtGradCGPA.Text.Trim()) > 0) ? Convert.ToDecimal(txtGradCGPA.Text.Trim()) : 0.00M;

                passingYearGrad = Convert.ToInt32(ddlGradPassingYear.SelectedValue);



                #region SSC
                try
                {
                    //(sscGPA > 0 || olevelPoint > 0) &&
                    if (candidateID > 0 && examTypeSSC > 0 && groupSSC > 0 && passingYearSSC > 0 && boardSSC > 0 && !string.IsNullOrEmpty(instituteSSC))
                    {

                        DAL.ExamDetail secondaryExamDetails = new DAL.ExamDetail();

                        secondaryExamDetails.Institute = instituteSSC; //txtSecInstitute.Text;
                        secondaryExamDetails.EducationBoardID = boardSSC; //EducationBoardID = 2 = DHAKA //Convert.ToInt32(ddlSec_EducationBrd.SelectedValue);
                        secondaryExamDetails.UndgradGradProgID = 1; // N/A
                        secondaryExamDetails.GroupOrSubjectID = groupSSC; //Convert.ToInt32(ddlSec_GrpOrSub.SelectedValue);
                        secondaryExamDetails.ResultDivisionID = divisionSSC; //Convert.ToInt32(ddlSec_DivClass.SelectedValue); // Note: 5 = GPA;  //;

                        if (divisionSSC == 5)
                        {
                            secondaryExamDetails.GPA = Convert.ToDecimal(sscGPA);
                        }

                        #region N/A
                        //if (examTypeSSC == 5)
                        //{
                        //    if (sscGPA > 0)
                        //    {
                        //        secondaryExamDetails.GPA = Convert.ToDecimal(sscGPA);
                        //    }

                        //    if (olevelPoint > 0)
                        //    {
                        //        //... Marks is used for O-Level Marks
                        //        secondaryExamDetails.Marks = Convert.ToDecimal(olevelPoint);
                        //    }
                        //}
                        //else
                        //{
                        //    if (sscGPA > 0)
                        //    {
                        //        secondaryExamDetails.GPA = Convert.ToDecimal(sscGPA);
                        //    }
                        //} 
                        #endregion

                        secondaryExamDetails.PassingYear = Convert.ToInt32(passingYearSSC);
                        secondaryExamDetails.CreatedBy = -99;
                        secondaryExamDetails.DateCreated = DateTime.Now;

                        long secondaryExamDetailsIDLong = -1;
                        using (var db = new CandidateDataManager())
                        {
                            db.Insert<DAL.ExamDetail>(secondaryExamDetails);
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
                        using (var db = new CandidateDataManager())
                        {
                            db.Insert<DAL.Exam>(secondaryExam);
                            secondaryExamIDLong = secondaryExam.ID;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion


                #region HSC
                try
                {
                    //&& (hscGPA > 0 || alevelPoint > 0)
                    if (candidateID > 0 && examTypeHSC > 0 && groupHSC > 0 && passingYearHSC > 0 && boardHSC > 0 && !string.IsNullOrEmpty(instituteHSC))
                    {
                        DAL.ExamDetail higherSecondaryExamDetails = new DAL.ExamDetail();

                        higherSecondaryExamDetails.EducationBoardID = boardHSC; //EducationBoardID = 2 = DHAKA //Convert.ToInt32(ddlHigherSec_EducationBrd.SelectedValue);
                        higherSecondaryExamDetails.Institute = instituteHSC; //txtHighSecInstitute.Text;
                        higherSecondaryExamDetails.UndgradGradProgID = 1; // N/A
                        higherSecondaryExamDetails.GroupOrSubjectID = groupHSC; //Convert.ToInt32(ddlHigherSec_GrpOrSub.SelectedValue);
                        higherSecondaryExamDetails.ResultDivisionID = divisionHSC; //Convert.ToInt32(ddlHigherSec_DivClass.SelectedValue); // Note: 5 = GPA; //

                        if (divisionHSC == 5)
                        {
                            higherSecondaryExamDetails.GPA = Convert.ToDecimal(hscGPA);
                        }

                        #region N/A
                        //if (examTypeHSC == 7)
                        //{
                        //    if (hscGPA > 0)
                        //    {
                        //        higherSecondaryExamDetails.GPA = Convert.ToDecimal(hscGPA);
                        //    }

                        //    if (alevelPoint > 0)
                        //    {
                        //        //... Marks is used for A-Level Marks
                        //        higherSecondaryExamDetails.Marks = Convert.ToDecimal(alevelPoint);
                        //    }
                        //}
                        //else
                        //{
                        //    if (hscGPA > 0)
                        //    {
                        //        higherSecondaryExamDetails.GPA = Convert.ToDecimal(hscGPA);
                        //    }
                        //} 
                        #endregion

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
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion


                #region Undergrad
                try
                {
                    //&& undergradCGPA > 0
                    if (candidateID > 0 && !string.IsNullOrEmpty(instituteUndergrad) && programDegreeUndergrad > 0
                    && divisionUndergrad > 0 && passingYearUndergrad > 0)
                    {

                        DAL.ExamDetail undergradExamDetails = new DAL.ExamDetail();

                        undergradExamDetails.Institute = instituteUndergrad;
                        undergradExamDetails.EducationBoardID = 1; //N/A
                        undergradExamDetails.UndgradGradProgID = programDegreeUndergrad;
                        undergradExamDetails.GroupOrSubjectID = 1; //N/A
                        undergradExamDetails.ResultDivisionID = divisionUndergrad; //Convert.ToInt32(ddlHigherSec_DivClass.SelectedValue); // Note: 5 = GPA; //

                        if (divisionUndergrad == 5)
                        {
                            undergradExamDetails.CGPA = Convert.ToDecimal(undergradCGPA);
                        }

                        if (Convert.ToInt32(ddlUndergradProgramDegree.SelectedValue) == 55)
                        {
                            undergradExamDetails.OtherProgram = programDegreeOtherUndergrad;
                        }

                        undergradExamDetails.PassingYear = passingYearUndergrad;
                        undergradExamDetails.CreatedBy = -99;
                        undergradExamDetails.DateCreated = DateTime.Now;

                        undergradExamDetails.AttributeInt1 = bachelorOrBachelorPass;

                        long undergradExamDetailsIDLong = -1;
                        using (var db = new CandidateDataManager())
                        {
                            db.Insert<DAL.ExamDetail>(undergradExamDetails);
                            undergradExamDetailsIDLong = undergradExamDetails.ID;
                        }

                        //-------------------------------
                        DAL.Exam undergradExam = new DAL.Exam();
                        undergradExam.ExamTypeID = 3;
                        undergradExam.CandidateID = candidateIdLong;
                        undergradExam.ExamDetailsID = undergradExamDetailsIDLong;
                        undergradExam.CreatedBy = -99;
                        undergradExam.DateCreated = DateTime.Now;

                        long undergradExamIDLong = -1;
                        using (var db = new CandidateDataManager())
                        {
                            db.Insert<DAL.Exam>(undergradExam);
                            undergradExamIDLong = undergradExam.ID;
                        }

                    }
                }
                catch (Exception ex)
                {

                }
                #endregion


                #region Grad
                try
                {
                    //&& gradCGPA > 0

                    if (candidateID > 0 && !string.IsNullOrEmpty(instituteGrad) && programDegreeGrad > 0
                    && divisionGrad > 0 && passingYearGrad > 0)
                    {

                        DAL.ExamDetail gradExamDetails = new DAL.ExamDetail();

                        gradExamDetails.Institute = instituteGrad;
                        gradExamDetails.EducationBoardID = 1; //N/A
                        gradExamDetails.UndgradGradProgID = programDegreeGrad;
                        gradExamDetails.GroupOrSubjectID = 1; //N/A
                        gradExamDetails.ResultDivisionID = divisionGrad; //Convert.ToInt32(ddlHigherSec_DivClass.SelectedValue); // Note: 5 = GPA; //

                        if (divisionGrad == 5)
                        {
                            gradExamDetails.CGPA = Convert.ToDecimal(gradCGPA);
                        }

                        if (Convert.ToInt32(ddlGradProgramDegree.SelectedValue) == 100)
                        {
                            gradExamDetails.OtherProgram = programDegreeOtherGrad;
                        }

                        gradExamDetails.PassingYear = passingYearGrad;
                        gradExamDetails.CreatedBy = -99;
                        gradExamDetails.DateCreated = DateTime.Now;

                        long gradExamDetailsIDLong = -1;
                        using (var db = new CandidateDataManager())
                        {
                            db.Insert<DAL.ExamDetail>(gradExamDetails);
                            gradExamDetailsIDLong = gradExamDetails.ID;
                        }

                        //-------------------------------
                        DAL.Exam gradExam = new DAL.Exam();
                        gradExam.ExamTypeID = 4;
                        gradExam.CandidateID = candidateIdLong;
                        gradExam.ExamDetailsID = gradExamDetailsIDLong;
                        gradExam.CreatedBy = -99;
                        gradExam.DateCreated = DateTime.Now;

                        long gradExamIDLong = -1;
                        using (var db = new CandidateDataManager())
                        {
                            db.Insert<DAL.Exam>(gradExam);
                            gradExamIDLong = gradExam.ID;
                        }

                    }
                }
                catch (Exception ex)
                {

                }
                #endregion



                #endregion







                #region CandidatePayment/CandidateFormSerial

                //---------------------------------------------------------------------------------
                //insert candidate payment
                //---------------------------------------------------------------------------------
                long candidatePaymentIDLong = -1;
                if (admissionSetup != null)
                {
                    using (var db = new CandidateDataManager())
                    {
                        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                        db.AdmissionDB.SPCandidatePaymentInsert(id_param, candidateIdLong, null, admissionSetup.AcaCalID, Convert.ToInt32(admissionUnit.UnitCode1), false, admissionSetup.Fee, -99, DateTime.Now);
                        candidatePaymentIDLong = Convert.ToInt64(id_param.Value);
                    }
                }

                //---------------------------------------------------------------------------------
                //insert candidate form serial
                //---------------------------------------------------------------------------------
                long candidateFormSerialIDLong = -1;
                if (admissionSetup != null)
                {
                    using (var db = new CandidateDataManager())
                    {
                        ObjectParameter id_param = new ObjectParameter("iD", typeof(long));
                        db.AdmissionDB.SPCandidateFormSlInsert(id_param, candidateIdLong, admissionSetup.ID, admissionSetup.AcaCalID, Convert.ToInt32(admissionUnit.UnitCode1), null, candidatePaymentIDLong, DateTime.Now, -99);
                        candidateFormSerialIDLong = Convert.ToInt64(id_param.Value);
                    }
                }
                #endregion


                #region insert Program Priority

                try
                {
                    if (admissionUnit != null && admissionSetup != null)
                    {
                        List<DAL.SPProgramsGetAllFromUCAM_Result> prgList = new List<DAL.SPProgramsGetAllFromUCAM_Result>();
                        using (var db = new OfficeDataManager())
                        {
                            prgList = db.AdmissionDB.SPProgramsGetAllFromUCAM()
                                .ToList();
                        }

                        using (var db = new CandidateDataManager())
                        {
                            var admissionUnitProgram = db.AdmissionDB.AdmissionUnitPrograms
                                .Where(aup => aup.AdmissionUnitID == admissionUnit.ID
                                    && aup.IsActive == true)
                                .FirstOrDefault();
                            if (admissionUnitProgram != null)
                            {
                                var program = prgList.Where(p => p.ProgramID == admissionUnitProgram.ProgramID).FirstOrDefault();

                                DAL.ProgramPriority candidateProgramPriority = new DAL.ProgramPriority();
                                candidateProgramPriority.CandidateID = candidateIdLong;
                                candidateProgramPriority.AdmissionUnitID = admissionUnit.ID;
                                candidateProgramPriority.AdmissionSetupID = admissionSetup.ID;
                                candidateProgramPriority.AdmissionUnitProgramID = admissionUnitProgram.ID;
                                candidateProgramPriority.BatchID = admissionUnitProgram.BatchID;
                                candidateProgramPriority.ProgramID = admissionUnitProgram.ProgramID;
                                if (program != null)
                                {
                                    candidateProgramPriority.ShortName = program.ShortName;
                                    candidateProgramPriority.ProgramName = program.DetailName;
                                }
                                else
                                {
                                    candidateProgramPriority.ShortName = "";
                                    candidateProgramPriority.ProgramName = "";
                                }
                                candidateProgramPriority.AcaCalID = admissionSetup.AcaCalID;
                                candidateProgramPriority.Priority = 1;
                                candidateProgramPriority.CreatedBy = candidateIdLong;
                                candidateProgramPriority.DateCreated = DateTime.Now;
                                db.Insert<DAL.ProgramPriority>(candidateProgramPriority);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                #endregion

                if (candidateIdLong > 0 && candidateUserIdLong > 0 &&
                    candidatePaymentIDLong > 0 && candidateFormSerialIDLong > 0)
                {

                    string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                        + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
                        + admissionSetup.EducationCategoryID + ";";

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
                Label1.Text = ex.Message;

                Console.WriteLine(ex.StackTrace);
                Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact the administrator. Error Code PF01X002TC?type=danger", false);
            }
            //}
        }

        #region N/A
        //private bool IsNumeric(string s)
        //{
        //    Regex r = new Regex(@"^\d{0,2}(\.\d{1,2})?$");

        //    return r.IsMatch(s);
        //}

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
        //    {
        //        LoadCaptcha();
        //        captchaMsg.Visible = true;
        //        return;
        //    }

        //    int admissionSetupIDLong = -1;
        //    int admissionUnitIDLong = -1;

        //    admissionSetupIDLong = Convert.ToInt32(Request.QueryString["asi"]);
        //    admissionUnitIDLong = Convert.ToInt32(Request.QueryString["aui"]);

        //    if (admissionUnitIDLong == 2 || admissionUnitIDLong == 3 || admissionUnitIDLong == 4 || admissionUnitIDLong == 5)
        //    {
        //        int sscExamType = -1;
        //        int sscGroup = -1;
        //        decimal sscGPA = 0.00M;
        //        int sscPassingYear = -1;

        //        int hscExamType = -1;
        //        int hscGroup = -1;
        //        decimal hscGPA = 0.00M;
        //        int hscPassingYear = -1;

        //        bool sscIsDecimal = false;
        //        bool hscIsDecimal = false;


        //        sscExamType = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
        //        sscGroup = Convert.ToInt32(ddlGroupSSC.SelectedValue);
        //        sscGPA = Convert.ToDecimal(txtGPASSC.Text);
        //        sscPassingYear = Convert.ToInt32(ddlPassYearSSC.SelectedValue);

        //        hscExamType = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
        //        hscGroup = Convert.ToInt32(ddlGroupHSC.SelectedValue);
        //        hscGPA = Convert.ToDecimal(txtGPAHSC.Text);
        //        hscPassingYear = Convert.ToInt32(ddlPassYearHSC.SelectedValue);


        //        List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admSscList = null;
        //        List<DAL.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId_Result> admHscList = null;


        //        sscIsDecimal = IsNumeric(sscGPA.ToString());
        //        hscIsDecimal = IsNumeric(hscGPA.ToString());
        //        if (sscIsDecimal == false || hscIsDecimal == false)
        //        {
        //            EligibleMessage("Your SSC or HSC GPA is not in Correct Format..!!", "text-danger");
        //            return;
        //        }




        //        if (admissionUnitIDLong > 0 && sscExamType > 0 && sscGroup > 0 && sscGPA > 0)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    admSscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(admissionUnitIDLong, sscExamType, sscGroup, sscGPA).ToList();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //Console.WriteLine(ex.StackTrace);
        //            }
        //        }

        //        if (admissionUnitIDLong > 0 && hscExamType > 0 && hscGroup > 0 && hscGPA > 0)
        //        {
        //            try
        //            {
        //                using (var db = new OfficeDataManager())
        //                {
        //                    admHscList = db.AdmissionDB.SPAdmissionSscHscGpaSetupByUnitExamTypeGroupGpaId(admissionUnitIDLong, hscExamType, hscGroup, hscGPA).ToList();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //Console.WriteLine(ex.StackTrace);
        //            }
        //        }

        //        if (admSscList.Count > 0 && admHscList.Count > 0)
        //        {
        //            PurchaseApplicationForm();

        //        }
        //        else if (sscExamType == 3 && hscExamType == 4)
        //        {
        //            PurchaseApplicationForm();
        //        }
        //        else if (sscExamType == 5 && hscExamType == 5)
        //        {
        //            PurchaseApplicationForm();
        //        }
        //        else
        //        {
        //            EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
        //        }
        //    }
        //    else
        //    {
        //        PurchaseApplicationForm();
        //    }

        //}





        //protected void btnShowPopupClicked(object sender, EventArgs e)
        //{
        //    ModalPopupExtender.Show();
        //}


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


        //protected void btnCancleModel_Click(object sender, EventArgs e)
        //{
        //    ModalPopupExtender.Hide();
        //} 
        #endregion




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
                        "Thank you for your interest in the <b>Bangladesh Health Professions Institute (BHPI)</b>. To move forward with your admission process, please proceed with the payment using the link provided below: " +
                        "<br/><br/>" +
                        "<div style='padding: 15px; background-color: #f9f9f9; border-left: 5px solid #8a151b;'>" +
                        "<b>Payment Link: </b><a href='http://admission.bhpi.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + paramValue + "'>Click here to pay</a><br/>" +
                        "<span style='color: #8a151b; font-size: 16px;'><b>Payment ID: " + candidatePaymentObj.PaymentId.ToString() + "</b></span>" +
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
                            eLog.Page = "PurchaseNotification.aspx";
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
                            eLog.Page = "PurchaseNotification.aspx";
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
                string messageBody = "Dear " + candidatePaymentObj.BasicInfo.FirstName + ", " +
                                    "Payment ID : " + candidatePaymentObj.PaymentId.ToString() +
                                    "and Payment Link : http://admission.bup.edu.bd/Admission/Candidate/PurchaseNotification.aspx?value=" + value +
                                    " BUP";

                string stringData = SMSUtility.Send(smsPhone, messageBody);

                string statusT = JObject.Parse(stringData)["statusCode"].ToString();

                if (statusT != "200") //if sms sending fails
                {
                    DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                    smsLog.AcaCalId = null;
                    smsLog.Attribute1 = "Sms sending failed in PurchaseForm.aspx";
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
                    smsLog.Attribute1 = "Sms sending successful PurchaseForm.aspx";
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





    }
}