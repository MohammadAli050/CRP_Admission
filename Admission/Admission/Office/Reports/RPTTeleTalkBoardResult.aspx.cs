using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DAL.ViewModels;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonUtility.FileConversion;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTTeleTalkBoardResult : PageBase
    {
        FileConversion aFileConverterObj = new FileConversion();
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Page.Form.Enctype = "multipart/form-data";

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                LoadDDL();
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
                DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.ID == 4 || a.ID == 6 || a.ID == 8).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.Select);

            }
        }

        protected void btnGetDataFromTeleTalk_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                if (acaCalId > 0 && eduCatId > 0)
                {
                    long? admUnitId = null;
                    if (Convert.ToInt64(ddlAdmUnit.SelectedValue) > 0)
                    {
                        admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                    }
                    else
                    {
                        admUnitId = null;
                    }

                    List<DAL.SPGetTeleTalkBoardResultData_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPGetTeleTalkBoardResultData(acaCalId, admUnitId, eduCatId).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        //list = list.Where(x => x.json == false).ToList();

                        //if (list.Count() == 0)
                        //{
                        //    MessageView("All candidate data is already fetch from TeleTalk. Only Click Load for Load Data.", "success");
                        //    return;
                        //}

                        Dictionary<int, string> dictErrorList = new Dictionary<int, string>();
                        int i = 1;
                        foreach (var tData in list)
                        {
                            try
                            {

                                #region SSC
                                try
                                {
                                    DAL.TeleTalkBoardResult ttbrSSCModel = null;
                                    using (var db = new CandidateDataManager())
                                    {
                                        ttbrSSCModel = db.AdmissionDB.TeleTalkBoardResults.Where(x => x.CandidateId == tData.CandidateID && x.AcaCalId == acaCalId).FirstOrDefault();
                                    }

                                    if (ttbrSSCModel != null && !string.IsNullOrEmpty(ttbrSSCModel.SSCBoardResultInJson))
                                    {

                                    }
                                    else
                                    {
                                        string sscExamType = tData.SSC_ExamTypeName_TTAPI;
                                        string sscBoard = tData.SSC_BoardName;
                                        string sscRoll = tData.SSC_RollNo;
                                        string sscPassingYear = tData.SSC_PassingYear.ToString();

                                        string sscResultTER = null;

                                        #region Teletalk API
                                        //string sscResultTER = TeletalkEducationBoard.GetData(sscExamType, sscBoard, sscRoll, sscPassingYear,"");
                                        #endregion

                                        #region Database Teletalk Data
                                        using (var db = new GeneralDataManager())
                                        {
                                            try
                                            {
                                                ObjectResult<string> Result = db.AdmissionDB.GetStudentTeletalkSSCData(sscBoard, sscRoll, "", sscPassingYear);

                                                var sscObj = Result.FirstOrDefault();

                                                if (sscObj != null)
                                                    sscResultTER = sscObj;
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            

                                        }
                                        #endregion

                                        //"{\"responseCode\":\"1\",\"responseDesc\":\"Success\",\"board\":\"DHAKA\",\"rollNo\":\"123456\",\"passYear\":\"2016\",\"name\":\"MD. SHAKIL AHAMED\",\"father\":\"MD. JAMAL UDDIN\",\"mother\":\"MST. SHAHANAJ\",\"regNo\":\"1310561161\",\"gender\":\"MALE\",\"result\":\"P\",\"gpa\":\"4.28\",\"gpaExc4th\":\"4.06\",\"studGroup\":\"SCIENCE\",\"eiin\":\"109327\",\"dob\":\"06052000\",\"totalObtMark\":\"0789\",\"totalExc4TH\":\"0759\",\"iName\":\"SINGARDIGHI HIGH SCHOOL\",\"cCode\":\"148\",\"cName\":\"MOWNA\",\"thana\":\"SREEPUR\",\"sub4thCode\":\"126\",\"subject\":[{\"subCode\":\"101\",\"subName\":\"BANGLA\",\"grade\":\"B\",\"gpoint\":\"3.00\",\"mark\":null},{\"subCode\":\"107\",\"subName\":\"ENGLISH\",\"grade\":\"A-\",\"gpoint\":\"3.50\",\"mark\":null},{\"subCode\":\"109\",\"subName\":\"MATHEMATICS\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":null},{\"subCode\":\"150\",\"subName\":\"BANGLADESH AND GLOBAL STUDIES\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":null},{\"subCode\":\"111\",\"subName\":\"ISLAM AND MORAL EDUCATION\",\"grade\":\"A-\",\"gpoint\":\"3.50\",\"mark\":null},{\"subCode\":\"136\",\"subName\":\"PHYSICS\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":null},{\"subCode\":\"137\",\"subName\":\"CHEMISTRY\",\"grade\":\"A-\",\"gpoint\":\"3.50\",\"mark\":null},{\"subCode\":\"138\",\"subName\":\"BIOLOGY\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":null},{\"subCode\":\"147\",\"subName\":\"PHYSICAL EDUCATION\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":null},{\"subCode\":\"126\",\"subName\":\"HIGHER MATHEMATICS\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":null}]}"

                                        if (!string.IsNullOrEmpty(sscResultTER))
                                        {
                                            TelitalkEducationResultModelSSC sscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelSSC>(sscResultTER);

                                            if (sscResultModel.responseCode == 1)
                                            {

                                                #region Insert / Update SSC Board Result
                                                try
                                                {
                                                    //DAL.TeleTalkBoardResult ttbrSSCModel = null;
                                                    //using (var db = new CandidateDataManager())
                                                    //{
                                                    //    ttbrSSCModel = db.AdmissionDB.TeleTalkBoardResults.Where(x => x.CandidateId == tData.CandidateID && x.AcaCalId == acaCalId).FirstOrDefault();
                                                    //}
                                                    if (ttbrSSCModel != null)
                                                    {
                                                        ttbrSSCModel.SSCBoardResultInJson = sscResultTER;
                                                        ttbrSSCModel.IsVerifiedSSC = false;
                                                        using (var db = new CandidateDataManager())
                                                        {
                                                            db.Update<DAL.TeleTalkBoardResult>(ttbrSSCModel);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DAL.TeleTalkBoardResult ttbrSSCNewModel = new DAL.TeleTalkBoardResult();
                                                        ttbrSSCNewModel.CandidateId = tData.CandidateID;
                                                        ttbrSSCNewModel.AcaCalId = acaCalId;
                                                        ttbrSSCNewModel.SSCBoardResultInJson = sscResultTER;
                                                        ttbrSSCNewModel.IsVerifiedSSC = false;
                                                        using (var db = new CandidateDataManager())
                                                        {
                                                            db.Insert<DAL.TeleTalkBoardResult>(ttbrSSCNewModel);
                                                        }

                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    string msg = "PaymentId: " + tData.PaymentId.ToString() + "; Exception: " + ex.Message.ToString() + "; Res: " + sscResultTER;
                                                    dictErrorList.Add(i++, msg);
                                                }
                                                #endregion


                                            }
                                            else
                                            {
                                                //MessageView("Failed to GET SSC Education Data !! Error: " + sscResultModel.responseDesc.ToString(), "fail");
                                                //return;
                                                string msg = "PaymentId: " + tData.PaymentId.ToString() + "; MSG: SSC Teletalk API Failed To GET Data; Res: " + sscResultTER;
                                                dictErrorList.Add(i++, msg);
                                            }
                                        }
                                        else
                                        {
                                            //MessageView("Failed to GET SSC Education Data", "fail");
                                            //return;

                                            string msg = "PaymentId: " + tData.PaymentId.ToString() + "; MSG: SSC Teletalk API Response Failed; Res: " + sscResultTER;
                                            dictErrorList.Add(i++, msg);
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
                                    DAL.TeleTalkBoardResult ttbrHSCModel = null;
                                    using (var db = new CandidateDataManager())
                                    {
                                        ttbrHSCModel = db.AdmissionDB.TeleTalkBoardResults.Where(x => x.CandidateId == tData.CandidateID && x.AcaCalId == acaCalId).FirstOrDefault();
                                    }

                                    if (ttbrHSCModel != null && !string.IsNullOrEmpty(ttbrHSCModel.HSCBoardResultInJson))
                                    {

                                    }
                                    else
                                    {

                                        string hscExamType = tData.HSC_ExamTypeName_TTAPI;
                                        string hscBoard = tData.HSC_BoardName;
                                        string hscRoll = tData.HSC_RollNo;
                                        string hscPassingYear = tData.HSC_PassingYear.ToString();

                                        string hscResultTER = null;

                                        #region Teletalk API
                                        //string hscResultTER = TeletalkEducationBoard.GetData(hscExamType, hscBoard, hscRoll, hscPassingYear,"");
                                        #endregion

                                        #region Database Teletalk Data
                                        using (var db = new GeneralDataManager())
                                        {
                                            try
                                            {
                                                ObjectResult<string> Result = db.AdmissionDB.GetStudentTeletalkHSCData(hscBoard, hscRoll, "", hscPassingYear);

                                                var hscObj = Result.FirstOrDefault();

                                                if (hscObj != null)
                                                    hscResultTER = hscObj;
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            

                                        }
                                        #endregion

                                        //"{\"responseCode\":\"1\",\"responseDesc\":\"Success\",\"board\":\"DHAKA\",\"rollNo\":\"123456\",\"passYear\":\"2018\",\"name\":\"BADIUL ALAM BHUIYAN\",\"father\":\"AKTEROZZAMAN BHUIYAN\",\"mother\":\"RAHANA AKTER\",\"regNo\":\"1310539095\",\"gender\":\"MALE\",\"result\":\"P\",\"gpa\":\"4.83\",\"gpaExc4th\":\"4.33\",\"sscBoard\":\"DHAKA\",\"sscPassYear\":\"2016\",\"sscRoll\":\"127688\",\"sscRegNo\":\"1310539095\",\"studGroup\":\"SCIENCE\",\"eiin\":\"133965\",\"totalObtMark\":\"0922\",\"totalExc4TH\":\"0838\",\"iName\":\"DR. MAHBUBUR RAHMAN MOLLAH COLLEGE\",\"cCode\":\"120\",\"cName\":\"DHAKA - 23, SHAYMPUR MODEL SCHOOL & COLLEGE\",\"thana\":\"DEMRA\",\"sub4thCode\":\"178\",\"subject\":[{\"subCode\":\"101\",\"subName\":\"BANGLA\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"157\"},{\"subCode\":\"107\",\"subName\":\"ENGLISH\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"142\"},{\"subCode\":\"275\",\"subName\":\"INFORMATION & COMMUNICATION TECHNOLOGY\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"082\"},{\"subCode\":\"174\",\"subName\":\"PHYSICS\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"151\"},{\"subCode\":\"176\",\"subName\":\"CHEMISTRY\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"142\"},{\"subCode\":\"265\",\"subName\":\"HIGHER MATHEMATICS\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"164\"},{\"subCode\":\"178\",\"subName\":\"BIOLOGY\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"164\"}]}"

                                        if (!string.IsNullOrEmpty(hscResultTER)) //!string.IsNullOrEmpty(sscResultTER) && 
                                        {
                                            TelitalkEducationResultModelHSC hscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelHSC>(hscResultTER);

                                            if (hscResultModel.responseCode == 1) //sscResultModel.responseCode == 1 &&
                                            {
                                                #region Insert / Update HSC Board Result
                                                try
                                                {

                                                    if (ttbrHSCModel != null)
                                                    {
                                                        ttbrHSCModel.HSCBoardResultInJson = hscResultTER;
                                                        ttbrHSCModel.IsVerifiedHSC = false;
                                                        using (var db = new CandidateDataManager())
                                                        {
                                                            db.Update<DAL.TeleTalkBoardResult>(ttbrHSCModel);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DAL.TeleTalkBoardResult ttbrHSCNewModel = new DAL.TeleTalkBoardResult();
                                                        ttbrHSCNewModel.CandidateId = tData.CandidateID;
                                                        ttbrHSCNewModel.AcaCalId = acaCalId;
                                                        ttbrHSCNewModel.HSCBoardResultInJson = hscResultTER;
                                                        ttbrHSCNewModel.IsVerifiedHSC = false;
                                                        using (var db = new CandidateDataManager())
                                                        {
                                                            db.Insert<DAL.TeleTalkBoardResult>(ttbrHSCNewModel);
                                                        }
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    string msg = "PaymentId: " + tData.PaymentId.ToString() + "; Exception: " + ex.Message.ToString() + "; Res: " + hscResultTER;
                                                    dictErrorList.Add(i++, msg);
                                                }
                                                #endregion

                                            }
                                            else
                                            {
                                                //MessageView("Failed to GET HSC Education Data !! Error: " + hscResultModel.responseDesc.ToString(), "fail");
                                                //return;

                                                string msg = "PaymentId: " + tData.PaymentId.ToString() + "; MSG: HSC Teletalk API Failed To GET Data; Res: " + hscResultTER;
                                                dictErrorList.Add(i++, msg);
                                            }

                                        }
                                        else
                                        {
                                            //MessageView("Error: Failed to GET HSC Education Data", "fail");
                                            //return;
                                            string msg = "PaymentId: " + tData.PaymentId.ToString() + "; MSG: HSC Teletalk API Response Failed; Res: " + hscResultTER;
                                            dictErrorList.Add(i++, msg);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                            }




                        } // END FOR


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
                    }
                    else
                    {
                        MessageView("No data found!", "fail");
                    }
                }
                else
                {
                    MessageView("Please select session and category!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                if (acaCalId > 0 && eduCatId > 0)
                {
                    long? admUnitId = null;
                    if (Convert.ToInt64(ddlAdmUnit.SelectedValue) > 0)
                    {
                        admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                    }
                    else
                    {
                        admUnitId = null;
                    }

                    List<DAL.SPGetTeleTalkBoardResultVerification_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPGetTeleTalkBoardResultVerification(acaCalId, admUnitId, eduCatId).ToList();
                    }

                    if (list != null && list.Count > 0)
                    {
                        foreach(var tData in list)
                        {
                            try
                            {
                                if(tData.CandidateID== 376537)
                                {

                                }

                                if (!string.IsNullOrEmpty(tData.SSCBoardResultInJson) && tData.SSCBoardResultInJson != "---")
                                {
                                    TelitalkEducationResultModelSSC sscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelSSC>(tData.SSCBoardResultInJson);
                                    if (sscResultModel != null)
                                    {
                                        tData.TT_FatherName = !string.IsNullOrEmpty(sscResultModel.father) ? sscResultModel.father : "---";
                                        tData.TT_MotherName = !string.IsNullOrEmpty(sscResultModel.mother) ? sscResultModel.mother : "---";
                                        tData.TT_SSC_ExamTypeName = "SSC";
                                        tData.TT_SSC_GroupOrSubjectName = !string.IsNullOrEmpty(sscResultModel.studGroup) ? sscResultModel.studGroup : "---";
                                        tData.TT_SSC_GPA = !string.IsNullOrEmpty(sscResultModel.gpa) ? Convert.ToDecimal(sscResultModel.gpa) : 0;
                                        tData.TT_SSC_PassingYear = !string.IsNullOrEmpty(sscResultModel.passYear) ? Convert.ToInt32(sscResultModel.passYear) : 0;
                                    }
                                }

                                if (!string.IsNullOrEmpty(tData.HSCBoardResultInJson) && tData.HSCBoardResultInJson != "---")
                                {
                                    TelitalkEducationResultModelHSC hscResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TelitalkEducationResultModelHSC>(tData.HSCBoardResultInJson);
                                    if (hscResultModel != null)
                                    {

                                        tData.TT_HSC_ExamTypeName = "HSC";
                                        tData.TT_HSC_GroupOrSubjectName = !string.IsNullOrEmpty(hscResultModel.studGroup) ? hscResultModel.studGroup : "---";
                                        tData.TT_HSC_GPA = !string.IsNullOrEmpty(hscResultModel.studGroup) ? Convert.ToDecimal(hscResultModel.gpa) : 0;
                                        tData.TT_HSC_PassingYear = !string.IsNullOrEmpty(hscResultModel.studGroup) ? Convert.ToInt32(hscResultModel.passYear) : 0;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            
                        }


                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        ReportDataSource rds = new ReportDataSource("DataSet1", list);

                        IList<ReportParameter> param1 = new List<ReportParameter>();
                        param1.Add(new ReportParameter("Faculty", ddlAdmUnit.SelectedItem.Text));
                        param1.Add(new ReportParameter("Session", ddlSession.SelectedItem.Text));

                        ReportViewer1.LocalReport.SetParameters(param1);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        MessageView("No data found!", "fail");
                    }

                }
                else
                {
                    MessageView("Please select session and category!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnViewProperty_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                panelViewResult.Visible = true;
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnUploadEligibleRoll_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            try
            {

                long admUnitId = -1;
                int acaCalId = -1;

                admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                acaCalId = Convert.ToInt32(ddlSession.SelectedValue);



                if (fuExcel.HasFile)
                {
                    string FileName = Path.GetFileName(fuExcel.FileName);
                    try
                    {
                        
                        if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                        {
                            //File Upload to Upload Temp folder Start
                            string UploadFileLocation = string.Empty;
                            UploadFileLocation = Server.MapPath("~/Upload/TEMP/");
                            try
                            {

                                try
                                {
                                    string path = Server.MapPath("~/Upload/TEMP/") + FileName;
                                    File.Delete(path);
                                }
                                catch (Exception ex)
                                {
                                    
                                }


                                string savePath = UploadFileLocation + FileName;
                                fuExcel.SaveAs(savePath);
                                //File Upload to Upload Temp folder End

                                //Read file from Upload temp folder
                                SheetName sheet = aFileConverterObj.PassFileName(savePath).FirstOrDefault();
                                //Populate datatable from excel sheet
                                DataTable dt = aFileConverterObj.ReadExcelFileDOM(savePath, FileName, sheet.Id);

                                //Remove First Row (Header)
                                DataRow row = dt.Rows[0];
                                dt.Rows.Remove(row);

                                int count = 0;
                                foreach (DataRow dtRow in dt.Rows)
                                {
                                    string testRoll = null;
                                    string score = null;
                                    //// Gets Column values 



                                    testRoll = dtRow.ItemArray[0].ToString().Trim();

                                    if (!string.IsNullOrEmpty(testRoll))
                                    {

                                        DAL.EligibleTestRollWrittenExam existData = null;
                                        using (var db = new CandidateDataManager())
                                        {
                                            existData = db.AdmissionDB.EligibleTestRollWrittenExams.Where(x => x.TestRoll == testRoll
                                                                                                        && x.AcaCalId == acaCalId
                                                                                                        && x.AdmUnitId == admUnitId).FirstOrDefault();
                                        }

                                        if (existData != null)
                                        {
                                            // Already Exist
                                        }
                                        else
                                        {
                                            DAL.EligibleTestRollWrittenExam etrastModel = new DAL.EligibleTestRollWrittenExam();
                                            etrastModel.TestRoll = testRoll;
                                            etrastModel.AcaCalId = acaCalId;
                                            etrastModel.AdmUnitId = admUnitId;

                                            try
                                            {

                                                using (var db2 = new CandidateDataManager())
                                                {
                                                    db2.Insert<DAL.EligibleTestRollWrittenExam>(etrastModel);
                                                    count++;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //lblMessage.Text = "Error-Insert: testRoll: " + testRoll + "; score: " + score + "; " + ex.Message + "; " + ex.InnerException.Message;
                                                //messagePanel.CssClass = "alert alert-danger";
                                                //messagePanel.Visible = true;
                                                MessageView("Error-Insert: testRoll: " + testRoll + "; score: " + score + "; " + ex.Message + "; " + ex.InnerException.Message, "fail");
                                                return;
                                            }
                                        }


                                            


                                    }

                                }

                                //lblFileUploadMsg.Text = "Total Roll: " + count + ", Uploaded Successfully.";
                                MessageView("Total Roll: " + count + ", Uploaded Successfully.", "success");

                                //lblMessage.Text = "Upload Complete";
                                //messagePanel.CssClass = "alert alert-success";
                                //btnDownload.Enabled = true;
                            }
                            catch (Exception ex)
                            {
                                //lblMessage.Text = "Error2: " + ex.Message + "; " + ex.InnerException.Message;
                                //messagePanel.CssClass = "alert alert-danger";
                                //messagePanel.Visible = true;
                                MessageView("Error2: " + ex.Message + "; " + ex.InnerException.Message, "fail");
                            }
                        }
                        else
                        {
                            //lblMessage.Text = "Wrong File format. Please upload excel file with .xls or .xlsx extension.";
                            MessageView("Wrong File format. Please upload excel file with .xls or .xlsx extension.", "fail");
                        }


                    }
                    catch (Exception ex)
                    {
                        //lblMessage.Text = "Error3: " + ex.Message + "; " + ex.InnerException.Message;
                        //messagePanel.CssClass = "alert alert-danger";
                        //messagePanel.Visible = true;
                        MessageView("Error3: " + ex.Message + "; " + ex.InnerException.Message, "fail");
                    }
                    //finally
                    //{
                    //    //string path = Server.MapPath("~/Upload/TEMP/") + FileName;
                    //    //File.Delete(path);
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }
    }
}