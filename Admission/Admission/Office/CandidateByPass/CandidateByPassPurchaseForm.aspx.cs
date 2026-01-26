using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.CandidateByPass
{
    public partial class CandidateByPassPurchaseForm : System.Web.UI.Page
    {



        string SessionLoginCaptcha = "SessionLoginCaptcha";
        int oLevelExamType = -1;
        int aLevelExamType = -1;
        long uId = 0;

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
                }
            }

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key

            if (!IsPostBack)
            {
                

                LoadCaptcha();
                LoadDDL();

                txtName.Text = String.Empty;
                txtDateOfBirth.Text = String.Empty;
                txtEmail.Text = String.Empty;
                txtSmsMobile.Text = String.Empty;
                txtGuardianMobile.Text = String.Empty;
                ddlGender.SelectedIndex = -1;
                //ddlQuota.SelectedIndex = -1;
                //ddlPassingYear.SelectedIndex = -1;
                ddlExamTypeSSC.SelectedIndex = -1;
                ddlGroupSSC.SelectedIndex = -1;
                txtGPASSC.Text = String.Empty;
                //txtPassYearSSC.Text = String.Empty;
                ddlExamTypeHSC.SelectedIndex = -1;
                ddlGroupHSC.SelectedIndex = -1;
                txtGPAHSC.Text = String.Empty;
                //txtPassYearHSC.Text = String.Empty;
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
                List<DAL.GroupOrSubject> groupSubjectList = db.AdmissionDB.GroupOrSubjects.Where(a => a.IsActive == true).ToList();
                List<DAL.EducationBoard> educationBoardList = db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true).ToList();
                List<DAL.ResultDivision> resultDivisionList = db.AdmissionDB.ResultDivisions.Where(a => a.IsActive == true).ToList();

                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                //DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(a => a.IsActive == true).OrderBy(x => x.CreatedBy).ToList(), "QuotaName", "ID", EnumCollection.ListItemType.Quota);

                DDLHelper.Bind<DAL.EducationBoard>(ddlBoardSSC, db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true && a.IsVisual == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);
                DDLHelper.Bind<DAL.EducationBoard>(ddlBoardHSC, db.AdmissionDB.EducationBoards.Where(a => a.IsActive == true && a.IsVisual == true).ToList(), "BoardName", "ID", EnumCollection.ListItemType.EducationBoard);

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

            //        //LoadPassingYearDDLForBachelors();
            //    }
            //    else if (admissionSetup.EducationCategoryID == 6) //for masters candidate.
            //    {
            //        //hideCalculater.Attributes.Add("style", "display:none");
            //        //hideSSC.Attributes.Add("style", "display:none");
            //        //hideHSC.Attributes.Add("style", "display:none");


            //        //hideCalculater.Visible = false;
            //        hideSSC.Visible = false;
            //        hideHSC.Visible = false;

            //        //LoadPassingYearDDLForMasters();
            //    }
            //}


            ddlPassYearSSC.Items.Clear();
            ddlPassYearSSC.Items.Add(new ListItem("Select", "-1"));
            ddlPassYearSSC.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                ddlPassYearSSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddlPassYearHSC.Items.Clear();
            ddlPassYearHSC.Items.Add(new ListItem("Select", "-1"));
            ddlPassYearHSC.AppendDataBoundItems = true;
            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                ddlPassYearHSC.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

        }

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

        private void EligibleMessage(string msg, string css)
        {
            massageHiddenTopId.Visible = true;
            massageHiddenBottomId.Visible = true;
            lblEligibleMsg.Text = msg;
            lblEligibleMsgBottom.Text = msg;
            lblEligibleMsg.CssClass = css;
            lblEligibleMsgBottom.CssClass = css;
            lblEligibleMsg.Focus();
            lblEligibleMsgBottom.Focus();
        }

        private void PurchaseApplicationForm()
        {

            DAL.BasicInfo candExist = null;

            DateTime dob = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
            int genderId = Int32.Parse(ddlGender.SelectedValue);
            //int quotaId = Int32.Parse(ddlQuota.SelectedValue);

            using (var db = new CandidateDataManager())
            {
                candExist = db.AdmissionDB.BasicInfoes
                    .Where(c =>
                        c.DateOfBirth == dob &&
                        c.SMSPhone == txtSmsMobile.Text
                        )
                    .FirstOrDefault();
            }

            //if (candExist != null) //if candidate exist, do not proceed.
            //{
            //    if (candExist.DateOfBirth.Equals(dob) && candExist.SMSPhone.Equals(txtSmsMobile.Text.Trim()))
            //    {
            //        string message = "Candidate already exist. If you like to purchase additional application, please login and click 'Purchase More Application'." +
            //            "If you have already applied, attempted payment and like to complete your payment procedure, then go to <a href='https://admission.bup.edu.bd/Admission/VerifyPayment'>Complete Payment Process</a> from homepage. ";
            //       EligibleMessage(message, "text-danger");
            //    }
            //}
            //else //proceed with purchase.
            //{

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
                candidate.Mobile = txtSmsMobile.Text.Trim();
                candidate.SMSPhone = txtSmsMobile.Text.Trim();
                candidate.Email = txtEmail.Text.Trim();
                candidate.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
                candidate.CandidateUserID = candidateUserIdLong;
                candidate.IsActive = false;
                candidate.UniqueIdentifier = Guid.NewGuid();
                candidate.GenderID = Convert.ToInt32(ddlGender.SelectedValue);
                //candidate.QuotaID = Convert.ToInt32(ddlQuota.SelectedValue);
                candidate.CreatedBy = -99;
                candidate.DateCreated = DateTime.Now;
                candidate.GuardianPhone = txtGuardianMobile.Text.Trim();

                long candidateIdLong = -1;
                using (var db = new CandidateDataManager())
                {
                    db.Insert<DAL.BasicInfo>(candidate);
                    candidateIdLong = candidate.ID;
                }

                #endregion



                #region CandidatePayment/CandidateFormSerial

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
                #region Insert Candidate ExamDetails - N/A

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
                #region Candidate SSC HSC O-Level A-Level Points -- N/A
                ////if (oLevelExamType == 5 && aLevelExamType == 7)    // O/A Level
                ////{
                //long candidateID = 0;
                //decimal sscGPA = 0.00M;
                //decimal hscGPA = 0.00M;
                //decimal olevelPoint = 0.00M;
                //decimal alevelPoint = 0.00M;
                //int examTypeSSC = -1;
                //int examTypeHSC = -1;
                //int groupSSC = -1;
                //int groupHSC = -1;
                //int passingYearSSC = 2;
                //int passingYearHSC = 2;

                //candidateID = candidateIdLong;
                //sscGPA = hfOLevelConvertedSscGPA.Value != "" ? Convert.ToDecimal(hfOLevelConvertedSscGPA.Value) : Convert.ToDecimal(txtGPASSC.Text);
                //hscGPA = hfALevelConvertedHscGPA.Value != "" ? Convert.ToDecimal(hfALevelConvertedHscGPA.Value) : Convert.ToDecimal(txtGPAHSC.Text);
                //olevelPoint = lblOLevelResult.Text != "" ? Convert.ToDecimal(lblOLevelResult.Text) : olevelPoint;
                //alevelPoint = lblALevelResult.Text != "" ? Convert.ToDecimal(lblALevelResult.Text) : alevelPoint;
                //examTypeSSC = Convert.ToInt32(ddlExamTypeSSC.SelectedValue);
                //examTypeHSC = Convert.ToInt32(ddlExamTypeHSC.SelectedValue);
                //groupSSC = Convert.ToInt32(ddlGroupSSC.SelectedValue);
                //groupHSC = Convert.ToInt32(ddlGroupHSC.SelectedValue);
                //passingYearSSC = Convert.ToInt32(txtPassYearSSC.Text);
                //passingYearHSC = Convert.ToInt32(txtPassYearHSC.Text);

                ////....SSC -> O-Level Insert
                //if (candidateID > 0 && (sscGPA > 0 || olevelPoint > 0) && examTypeSSC > 0 && groupSSC > 0 && passingYearSSC > 0)
                //{
                //    DAL.CandidateOALevelGPA a = new DAL.CandidateOALevelGPA();
                //    a.CandidateID = candidateID;
                //    a.SscAndHscGpa = sscGPA;
                //    a.OAndALevelPoint = olevelPoint;
                //    a.ExamTypeID = examTypeSSC;
                //    a.GroupID = groupSSC;
                //    a.PassingYear = passingYearSSC;

                //    int candidateSSCOALevelGPAID = -1;
                //    using (var db = new CandidateDataManager())
                //    {
                //        db.Insert<DAL.CandidateOALevelGPA>(a);
                //        candidateSSCOALevelGPAID = a.ID;
                //    }
                //}

                ////....HSC -> A-Level Insert
                //if (candidateID > 0 && (hscGPA > 0 || alevelPoint > 0) && examTypeHSC > 0 && groupHSC > 0 && passingYearHSC > 0)
                //{
                //    DAL.CandidateOALevelGPA a = new DAL.CandidateOALevelGPA();
                //    a.CandidateID = candidateID;
                //    a.SscAndHscGpa = hscGPA;
                //    a.OAndALevelPoint = alevelPoint;
                //    a.ExamTypeID = examTypeHSC;
                //    a.GroupID = groupHSC;
                //    a.PassingYear = passingYearHSC;

                //    int candidateHSCOALevelGPAID = -1;
                //    using (var db = new CandidateDataManager())
                //    {
                //        db.Insert<DAL.CandidateOALevelGPA>(a);
                //        candidateHSCOALevelGPAID = a.ID;
                //    }
                //}

                ////}
                #endregion


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

                    //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                    //    + candidateFormSerialIDLong + ";0;" + admissionUnit.ID + ";"
                    //    + admissionSetup.EducationCategoryID + ";";

                    //string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                    //            + -1 + ";1;" + -1 + ";"
                    //            + admissionSetup.EducationCategoryID + ";";

                    CandidateByPassLogInsert(candidateIdLong, candidatePaymentIDLong);

                    string urlParam = candidateIdLong + ";" + candidatePaymentIDLong + ";"
                                + -1 + ";1;" + -1 + ";"
                                + 4 + ";";
                    Response.Redirect("~/Admission/Office/CandidateByPass/CandidateByPassPurchaseNotification.aspx?value=" + urlParam, false);
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


        private void CandidateByPassLogInsert(long candidateIdLong, long candidatePaymentIDLong)
        {
            DAL_Log.CandidateByPassLog cbpLog = new DAL_Log.CandidateByPassLog();
            cbpLog.SystemUserID = uId;
            cbpLog.CandidateID = candidateIdLong;
            cbpLog.CandidatePaymentID = candidatePaymentIDLong;
            cbpLog.Page = "CandidateByPassPurchaseForm.aspx";
            cbpLog.CandidateApplyDate = DateTime.Now;
            cbpLog.Attribute1 = "Lobbying student or student not eligible in our system or any other force for applying student.";
            cbpLog.CreatedBy = uId;
            cbpLog.CreatedDate = DateTime.Now;

            LogWriter.CandidateByPassLog(cbpLog);
        }


        private bool IsNumeric(string s)
        {
            Regex r = new Regex(@"^\d{0,2}(\.\d{1,2})?$");

            return r.IsMatch(s);
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

            if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            {
                messageList.Add(i++, "Date of Birth is Empty !!");
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
            if (string.IsNullOrEmpty(txtInstituteSSC.Text))
            {
                messageList.Add(i++, "Institute is Empty");
            }
            if (ddlExamTypeSSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Examination is not Selected");
            }
            if (ddlPassYearSSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Passing Year is not Selected");
            }
            if (ddlBoardSSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "SSC Board is not Selected");
            }
            if (string.IsNullOrEmpty(txtGPASSC.Text))
            {
                messageList.Add(i++, "SSC GPA is Empty");
            }

            ////----------------HSC------------------
            if (string.IsNullOrEmpty(txtInstituteHSC.Text))
            {
                messageList.Add(i++, "Institute is Empty");
            }
            if (ddlExamTypeHSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Examination is not Selected");
            }
            if (ddlPassYearHSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Passing Year is not Selected");
            }
            if (ddlBoardHSC.SelectedValue == "-1")
            {
                messageList.Add(i++, "HSC Board is not Selected");
            }
            if (string.IsNullOrEmpty(txtGPAHSC.Text))
            {
                messageList.Add(i++, "HSC GPA is Empty");
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

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtCaptcha.Text != SessionSGD.GetObjFromSession<string>(SessionLoginCaptcha))
            {
                LoadCaptcha();
                captchaMsg.Visible = true;
                return;
            }


            MessageModel resultCheckInputFieldValidation = CheckInputFieldValidationSSCHSC();

            if (resultCheckInputFieldValidation != null && resultCheckInputFieldValidation.MessageBoolean == true)
            {
                PurchaseApplicationForm();
            }
            else
            {
                ShowAlertMessage(resultCheckInputFieldValidation.MessageBody);
                return;
            }


            


            #region N/A
            //int admissionSetupIDLong = -1;
            //int admissionUnitIDLong = -1;

            //admissionSetupIDLong = Convert.ToInt32(Request.QueryString["asi"]);
            //admissionUnitIDLong = Convert.ToInt32(Request.QueryString["aui"]);

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
            //        EligibleMessage("Your SSC or HSC GPA is not in Correct Format..!!", "text-danger");
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






            //    //if (admSscList.Count > 0 && admHscList.Count > 0)
            //    if (sscExamType == 1 && hscExamType == 2)       // SSC / HSC
            //    {

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
            //                //candidateFormSlT.Remove(candidateFormSlT.Where(x => x.Attribute1 == item.ID.ToString()).FirstOrDefault());



            //                //--- removing faculty amount which one is not wligible
            //                //candidatePaymentT.Amount = candidatePaymentT.Amount - admSetupTemp.Fee;
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
            //            EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
            //        }



            //    }
            //    else if (sscExamType == 5 && hscExamType == 7)    // O/A Level
            //    {
            //        oLevelExamType = 5;
            //        aLevelExamType = 7;
            //        PurchaseApplicationForm();
            //    }
            //    else if (sscExamType == 6 && hscExamType == 8)    // Dakhil/Alim
            //    {
            //        PurchaseApplicationForm();
            //    }
            //    else if (sscExamType == 1 && hscExamType == 8)    // SSC / Alim
            //    {
            //        PurchaseApplicationForm();
            //    }
            //    else if (sscExamType == 6 && hscExamType == 2)    //Dakhil / HSC
            //    {
            //        PurchaseApplicationForm();
            //    }
            //    else if (sscExamType == 1 && hscExamType == 7)     //SSC / A-Level
            //    {
            //        PurchaseApplicationForm();
            //    }
            //    else if (sscExamType == 5 && hscExamType == 2)     //O-Level / HSC
            //    {
            //        PurchaseApplicationForm();
            //    }
            //    else
            //    {
            //        btnSubmit.Visible = false;
            //        EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
            //    }
            //}
            //else
            //{
            //    btnSubmit.Visible = false;
            //    EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
            //} 
            #endregion

        }





        protected void btnShowPopupClicked(object sender, EventArgs e)
        {
            ModalPopupExtender.Show();
        }


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
            lblOLevelConvertedSscGPA.Text = (result / 5).ToString();
            txtGPASSC.Text = (result / 5).ToString(); //result.ToString();


            decimal subject6 = Decimal.Parse(ddlALevelSubject1.SelectedValue.ToString());
            decimal subject7 = Decimal.Parse(ddlALevelSubject2.SelectedValue.ToString());

            result2 = (subject6 + subject7);


            lblALevelResult.Text = result2.ToString();
            hfALevelConvertedHscGPA.Value = (result2 / 2).ToString();
            lblALevelConvertedHscGPA.Text = (result2 / 2).ToString();
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
                EligibleMessage("You are not Eligible to Apply..!!", "text-danger");
                ModalPopupExtender.Hide();
            }
            else
            {
                txtGPASSC.Text = hfOLevelConvertedSscGPA.Value;
                //txtGPASSC.Enabled = false;
                txtGPAHSC.Text = hfALevelConvertedHscGPA.Value;
                //txtGPAHSC.Enabled = false;

                ddlGroupSSC.SelectedValue = "2";
                ddlGroupHSC.SelectedValue = "2";
                //ddlGroupSSC.Enabled = false;
                //ddlGroupHSC.Enabled = false;

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


    }
}