using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonUtility.FileConversion;

namespace Admission.Admission.Office.Reports
{
    public partial class RPTDetailsForVivaUndergraduate : PageBase
    {
        FileConversion aFileConverterObj = new FileConversion();
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                LoadDDL();
                DeleteSession();

                txtTotalNumber.Enabled = false;
            }
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }

            ddlDownloadMode.Items.Clear();
            //ddlDownloadMode.Items.Add(new ListItem("ALL", "-1"));
            //ddlDownloadMode.Items.Add(new ListItem("Merit", "1"));
            //ddlDownloadMode.Items.Add(new ListItem("Military, Senate and Syndicate Members", "2"));
            //ddlDownloadMode.Items.Add(new ListItem("Freedom Fighter", "3"));
            //ddlDownloadMode.Items.Add(new ListItem("Tribal", "4"));

            //ddlDownloadMode.Items.Add(new ListItem("ALL", "-1"));
            //ddlDownloadMode.Items.Add(new ListItem("Merit", "1"));
            //ddlDownloadMode.Items.Add(new ListItem("Special Quota", "2"));
            //ddlDownloadMode.Items.Add(new ListItem("Freedom Fighter", "3"));
            //ddlDownloadMode.Items.Add(new ListItem("Tribal", "4"));

            ddlDownloadMode.Items.Add(new ListItem("ALL", "-1"));
            ddlDownloadMode.Items.Add(new ListItem("Non-Quota", "1"));
            ddlDownloadMode.Items.Add(new ListItem("Special Quota", "2"));
            ddlDownloadMode.Items.Add(new ListItem("Freedom Fighter", "3"));
            ddlDownloadMode.Items.Add(new ListItem("Ethnic Minority", "4"));
        }

        protected void ddlDownloadMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDownloadMode.SelectedValue == "-1")
            {
                txtTotalNumber.Enabled = false;
            }
            else
            {
                txtTotalNumber.Enabled = true;
            }
        }

        private void DeleteSession()
        {
            SessionSGD.DeleteFromSession("list");
            SessionSGD.DeleteFromSession("fileName");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            #region Validation & Required input value get and check 
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            int examMarks = -1;

            if (!string.IsNullOrEmpty(txtTotalMarks.Text))
            {
                examMarks = Convert.ToInt32(txtTotalMarks.Text);
            }
            else
            {
                lblMessage.Text = "Total Marks required";
                lblMessage.ForeColor = Color.Crimson;
                return;
            }

            if (admUnitId < 0 || acaCalId < 0)
            {
                lblMessage.Text = "Select Session and Faculty";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
                return;
            }
            #endregion

            // Session & lebel Reset
            lblMessage.Text = "";
            DeleteSession();

            #region Delete Information From Table
            using (var db = new OfficeDataManager())
            {
                db.AdmissionDB.TruncateExcelTestRollAndScoreTemp();
            }
            #endregion

            if (fuExcel.HasFile)
            {
                //Process Start
                try
                {
                    string FileName = Path.GetFileName(fuExcel.FileName);
                    //Save File to Session
                    SessionSGD.SaveObjToSession(FileName, "fileName");
                    if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                    {
                        //File Upload to Upload Temp folder Start
                        string UploadFileLocation = string.Empty;
                        UploadFileLocation = Server.MapPath("~/Upload/TEMP/");
                        try
                        {

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

                            //List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> candidateScoreList =
                            //    new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                            //using (var db = new OfficeDataManager())
                            //{
                            //var watch = System.Diagnostics.Stopwatch.StartNew();
                            //for (int i = 0; i < dt.Rows.Count; i++)

                            #region N/A
                            //using (var db1 = new OfficeDataManager())
                            //{
                            ////db1.AdmissionDB.ExcelTestRollAndScoreTemps.RemoveRange(db1.AdmissionDB.ExcelTestRollAndScoreTemps);
                            ////db1.AdmissionDB.SaveChanges();

                            //    db1.AdmissionDB.TruncateExcelTestRollAndScoreTemp();
                            //} 
                            #endregion


                            foreach (DataRow dtRow in dt.Rows)
                            {
                                string testRoll = null;
                                string score = null;
                                //// Gets Column values 



                                testRoll = dtRow.ItemArray[0].ToString().Trim();
                                score = dtRow.ItemArray[1].ToString().Trim();

                                if (!string.IsNullOrEmpty(testRoll))
                                {
                                    DAL.ExcelTestRollAndScoreTemp etrastModel = new DAL.ExcelTestRollAndScoreTemp();
                                    etrastModel.TestRoll = testRoll;
                                    etrastModel.Score = score;
                                    etrastModel.AcaCalId = acaCalId;
                                    etrastModel.AdmissionUnitId = admUnitId;
                                    etrastModel.ExamMark = examMarks;
                                    etrastModel.CreatedBy = uId;
                                    etrastModel.DateCreated = DateTime.Now;

                                    try
                                    {

                                        using (var db2 = new OfficeDataManager())
                                        {
                                            db2.Insert<DAL.ExcelTestRollAndScoreTemp>(etrastModel);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        lblMessage.Text = "Error-Insert: testRoll: " + testRoll + "; score: " + score + "; " + ex.Message + "; " + ex.InnerException.Message;
                                        messagePanel.CssClass = "alert alert-danger";
                                        messagePanel.Visible = true;
                                        return;
                                    }


                                    #region N/A
                                    //DAL.ExcelTestRollAndScoreTemp etrstC = new DAL.ExcelTestRollAndScoreTemp();
                                    //using (var db1 = new OfficeDataManager())
                                    //{
                                    //    etrstC = db1.AdmissionDB.ExcelTestRollAndScoreTemps.Where(x => x.TestRoll == testRoll && x.AcaCalId == acaCalId && x.AdmissionUnitId == admUnitId).FirstOrDefault();
                                    //}

                                    //if (etrstC != null)
                                    //{

                                    //    etrstC.TestRoll = testRoll;
                                    //    etrstC.Score = score;
                                    //    etrstC.ExamMark = examMarks;
                                    //    etrstC.ModifiedBy = uId;
                                    //    etrstC.DateModified = DateTime.Now;

                                    //    try
                                    //    {

                                    //        using (var db2 = new OfficeDataManager())
                                    //        {
                                    //            db2.Update<DAL.ExcelTestRollAndScoreTemp>(etrstC);
                                    //        }
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        lblMessage.Text = "Error-Update: testRoll: " + testRoll + "; score: " + score + "; " + ex.Message + "; " + ex.InnerException.Message;
                                    //        messagePanel.CssClass = "alert alert-danger";
                                    //        messagePanel.Visible = true;
                                    //        return;
                                    //    }
                                    //}
                                    //else
                                    //{

                                    //} 
                                    #endregion

                                    #region N/A
                                    //Console.WriteLine("Test Roll - Row: " + i.ToString() + testRoll);
                                    //Console.WriteLine("Score - Row: " + i.ToString() + score);

                                    ////for (int j = 0; j < dt.Columns.Count; j++)
                                    ////{
                                    ////    if (j == 0)
                                    ////    {
                                    ////        testRoll = dt.Rows[i][j].ToString();
                                    ////    }
                                    ////    if (j == 1)
                                    ////    {
                                    ////        score = dt.Rows[i][j].ToString();
                                    ////    }
                                    ////} 
                                    #endregion

                                    #region N/A
                                    //DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result candidateScoreObj = null;
                                    //try
                                    //{
                                    //    candidateScoreObj = db.AdmissionDB.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate(testRoll, score, admUnitId, acaCalId, true, examMarks).FirstOrDefault();
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    lblMessage.Text = "Error1: testRoll: " + testRoll + "; score: " + score + "; " + ex.Message + "; " + ex.InnerException.Message;
                                    //    messagePanel.CssClass = "alert alert-danger";
                                    //    messagePanel.Visible = true;
                                    //    return;
                                    //}
                                    //if (candidateScoreObj != null)
                                    //{
                                    //    candidateScoreList.Add(candidateScoreObj);
                                    //}
                                    //else
                                    //{
                                    //    DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result newObj
                                    //        = new DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result();
                                    //    newObj.TestRoll = testRoll;
                                    //    newObj.MarksObtained = score;

                                    //    candidateScoreList.Add(newObj);

                                    //    lblUpl.Text += "Some Test Rolls did not return any information;  ";
                                    //    lblUpl.ForeColor = Color.Crimson;
                                    //} 
                                    #endregion
                                }

                            }

                            #region N/A
                            //watch.Stop();
                            //Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
                            //}

                            //SessionSGD.SaveListToSession(candidateScoreList, "list");


                            //if (dt.Rows.Count > 0)
                            //{
                            //    if (System.IO.File.Exists(savePath))
                            //    {
                            //        System.IO.File.Delete(savePath);
                            //    }
                            //    lblMessage.Text = "Upload Complete";
                            //    messagePanel.CssClass = "alert alert-success";
                            //    btnDownload.Enabled = true;
                            //} 
                            #endregion


                            lblMessage.Text = "Upload Complete";
                            messagePanel.CssClass = "alert alert-success";
                            btnDownload.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error2: " + ex.Message + "; " + ex.InnerException.Message;
                            messagePanel.CssClass = "alert alert-danger";
                            messagePanel.Visible = true;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Wrong File format. Please upload excel file with .xls or .xlsx extension.";
                    }


                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error3: " + ex.Message + "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "";


                long facultyId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
                int acaCald = Convert.ToInt32(ddlSession.SelectedValue);

                if (facultyId > 0 && acaCald > 0)
                {
                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> candidateScoreList = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();
                    //candidateScoreList = SessionSGD.GetListFromSession<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>("list");

                    candidateScoreList = LoadCandidateInformation(facultyId, acaCald);

                    ////Order the list is descending order of TotalWeighted value
                    candidateScoreList = candidateScoreList.OrderByDescending(c => c.TotalWeighted).ToList();



                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> meritList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> specialQuotaList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> freedomFighterList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> tribalList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    if (!string.IsNullOrEmpty(txtTotalNumber.Text))
                    {
                        int totalNumber = Convert.ToInt32(txtTotalNumber.Text);

                        //int numberToBeTakenMerit = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.67 * totalNumber)));
                        //int numberToBeTakenMilitary = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.30 * totalNumber)));
                        //int numberToBeTakenFF = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.02 * totalNumber)));
                        //int numberToBeTakenTribal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.01 * totalNumber)));

                        int numberToBeTakenMerit = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.60 * totalNumber)));
                        int numberToBeTakenSpecialQuota = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.37 * totalNumber)));
                        int numberToBeTakenFF = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.02 * totalNumber)));
                        int numberToBeTakenTribal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.01 * totalNumber)));

                        meritList = candidateScoreList.Take(numberToBeTakenMerit).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> _tempListWithoutMerit
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                        _tempListWithoutMerit = candidateScoreList.Except(meritList).ToList();

                        specialQuotaList = _tempListWithoutMerit.OrderByDescending(c => c.TotalWeighted)
                            .Where(c => c.quota == "Special Quota").Take(numberToBeTakenSpecialQuota).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> _tempListWithoutSpecialQuota
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                        _tempListWithoutSpecialQuota = _tempListWithoutMerit.Except(specialQuotaList).ToList();

                        freedomFighterList = _tempListWithoutSpecialQuota.OrderByDescending(c => c.TotalWeighted)
                            .Where(c => c.quota == "Freedom Fighter").Take(numberToBeTakenFF).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> _tempListWithoutMeritMilitaryFreedomFighter
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                        _tempListWithoutMeritMilitaryFreedomFighter = _tempListWithoutSpecialQuota.Except(freedomFighterList).ToList();

                        tribalList = _tempListWithoutMeritMilitaryFreedomFighter.OrderByDescending(c => c.TotalWeighted)
                            .Where(c => c.quota == "Ethnic Minority").Take(numberToBeTakenTribal).ToList();
                    }

                    if (ddlDownloadMode.SelectedValue == "-1") // all selected
                    {
                        if (candidateScoreList.Count > 0)
                        {
                            string count = candidateScoreList.Count.ToString() != null ? candidateScoreList.Count.ToString() : "0";
                            string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                            ReportDataSource rds = new ReportDataSource("DataSet1", candidateScoreList);

                            ReportParameter p1 = new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text.ToString());
                            ReportParameter p2 = new ReportParameter("Count", count);
                            ReportParameter p3 = new ReportParameter("PrintDate", printDate);


                            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        else
                        {
                            ReportDataSource rds = new ReportDataSource("DataSet1");
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = false;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "1") //merit selected
                    {
                        if (meritList.Count > 0)
                        {
                            string count = meritList.Count.ToString() != null ? meritList.Count.ToString() : "0";
                            string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                            ReportDataSource rds = new ReportDataSource("DataSet1", meritList);

                            ReportParameter p1 = new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text.ToString());
                            ReportParameter p2 = new ReportParameter("Count", count);
                            ReportParameter p3 = new ReportParameter("PrintDate", printDate);


                            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        else
                        {
                            ReportDataSource rds = new ReportDataSource("DataSet1");
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = false;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "2") //Special Quota
                    {
                        if (specialQuotaList.Count > 0)
                        {
                            string count = specialQuotaList.Count.ToString() != null ? specialQuotaList.Count.ToString() : "0";
                            string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                            ReportDataSource rds = new ReportDataSource("DataSet1", specialQuotaList);

                            ReportParameter p1 = new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text.ToString());
                            ReportParameter p2 = new ReportParameter("Count", count);
                            ReportParameter p3 = new ReportParameter("PrintDate", printDate);


                            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        else
                        {
                            ReportDataSource rds = new ReportDataSource("DataSet1");
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = false;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "3") // freedom fighter selected
                    {
                        if (freedomFighterList.Count > 0)
                        {
                            string count = freedomFighterList.Count.ToString() != null ? freedomFighterList.Count.ToString() : "0";
                            string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                            ReportDataSource rds = new ReportDataSource("DataSet1", freedomFighterList);

                            ReportParameter p1 = new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text.ToString());
                            ReportParameter p2 = new ReportParameter("Count", count);
                            ReportParameter p3 = new ReportParameter("PrintDate", printDate);


                            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        else
                        {
                            ReportDataSource rds = new ReportDataSource("DataSet1");
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = false;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "4") //tribal selected
                    {
                        if (tribalList.Count > 0)
                        {
                            string count = tribalList.Count.ToString() != null ? tribalList.Count.ToString() : "0";
                            string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                            ReportDataSource rds = new ReportDataSource("DataSet1", tribalList);

                            ReportParameter p1 = new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text.ToString());
                            ReportParameter p2 = new ReportParameter("Count", count);
                            ReportParameter p3 = new ReportParameter("PrintDate", printDate);


                            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        else
                        {
                            ReportDataSource rds = new ReportDataSource("DataSet1");
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = false;
                        }
                    }
                }
                else
                {
                    lblMessage.Text = "Please Select Faculty and Session for Load Data !!";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }



            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error4: " + ex.Message + "; " + ex.InnerException.Message;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            if (admUnitId > 0 && acaCalId > 0)
            {


                string admUnitStr = string.Empty;
                string sessionStr = string.Empty;

                string admUnitWithoutSpaces = ddlAdmUnit.SelectedItem.Text.Replace(" ", "_").Replace("&", "And");
                admUnitStr = admUnitWithoutSpaces;

                string sessionStrWithoutHyphenSpace = ddlSession.SelectedItem.Text.Replace(" - ", "_").Replace(" ", "_");
                sessionStr = sessionStrWithoutHyphenSpace;

                try
                {
                    lblMessage.Text = "";

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> candidateScoreList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();
                    //candidateScoreList = SessionSGD.GetListFromSession<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>("list");

                    candidateScoreList = LoadCandidateInformation(admUnitId, acaCalId);

                    //Order the list is descending order of TotalWeighted value
                    //candidateScoreList = candidateScoreList.OrderByDescending(c => c.TotalWeighted).ToList();
                    //candidateScoreList = candidateScoreList.OrderByDescending(c => c.TotalWeighted).ThenByDescending(x=> x.MarksObtained)
                    //                                                                               .ThenByDescending(x=> x.sscResult)
                    //                                                                               .ThenByDescending(x=> x.hscResult)
                    //                                                                               .ThenBy(x=> x.TestRoll).ToList();

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> meritList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> specialQuotaList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> freedomFighterList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> tribalList
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                    if (!string.IsNullOrEmpty(txtTotalNumber.Text))
                    {
                        int totalNumber = Convert.ToInt32(txtTotalNumber.Text);

                        //int numberToBeTakenMerit = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.67 * totalNumber)));
                        //int numberToBeTakenMilitary = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.30 * totalNumber)));
                        //int numberToBeTakenFF = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.02 * totalNumber)));
                        //int numberToBeTakenTribal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.01 * totalNumber)));

                        int numberToBeTakenMerit = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.60 * totalNumber)));
                        int numberToBeTakenSpecialQuota = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.37 * totalNumber)));
                        int numberToBeTakenFF = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.02 * totalNumber)));
                        int numberToBeTakenTribal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(0.01 * totalNumber)));

                        meritList = candidateScoreList.Take(numberToBeTakenMerit).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> _tempListWithoutMerit
                        = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                        _tempListWithoutMerit = candidateScoreList.Except(meritList).ToList();

                        specialQuotaList = _tempListWithoutMerit.OrderByDescending(c => c.TotalWeighted)
                            .Where(c => c.quota == "Special Quota").Take(numberToBeTakenSpecialQuota).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> _tempListWithoutSpecialQuota
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                        _tempListWithoutSpecialQuota = _tempListWithoutMerit.Except(specialQuotaList).ToList();

                        freedomFighterList = _tempListWithoutSpecialQuota.OrderByDescending(c => c.TotalWeighted)
                            .Where(c => c.quota == "Freedom Fighter").Take(numberToBeTakenFF).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> _tempListWithoutMeritMilitaryFreedomFighter
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

                        _tempListWithoutMeritMilitaryFreedomFighter = _tempListWithoutSpecialQuota.Except(freedomFighterList).ToList();

                        tribalList = _tempListWithoutMeritMilitaryFreedomFighter.OrderByDescending(c => c.TotalWeighted)
                            .Where(c => c.quota == "Ethnic Minority").Take(numberToBeTakenTribal).ToList();
                    }

                    if (ddlDownloadMode.SelectedValue == "-1") // all selected
                    {
                        if (candidateScoreList.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = ListToDataTableManager.ToDataTable(candidateScoreList);

                            dt.Columns.Remove("acaCalID");
                            dt.Columns.Remove("admUnitID");
                            dt.Columns.Remove("IsPaid");
                            dt.Columns.Remove("sscResultGpaW4s");
                            dt.Columns.Remove("hscResultGpaW4s");
                            dt.Columns.Remove("sscMarks");
                            dt.Columns.Remove("hscMarks");
                            dt.Columns.Remove("hscInstitute");
                            dt.Columns.Remove("sscInstitute");

                            /*
                             * 
                             * public string TestRoll { get; set; }
                                public string MarksObtained { get; set; }
                                public Nullable<decimal> MarksObtainedWeighted { get; set; }
                                public Nullable<int> acaCalID { get; set; }
                                public Nullable<long> admUnitID { get; set; }
                                public Nullable<long> PaymentId { get; set; }
                                public Nullable<bool> IsPaid { get; set; }
                                public string isApproved { get; set; }
                                public string candidateName { get; set; }
                                public string candidateSmsPhone { get; set; }
                                public string quota { get; set; }
                                public string sscExamTypeCode { get; set; }
                                public string sscEduBoard { get; set; }
                                public string sscInstitute { get; set; }
                                public string sscRollNo { get; set; }
                                public string sscGroupOrSub { get; set; }
                                public string sscResultDivision { get; set; }
                                public Nullable<decimal> sscResult { get; set; }
                                public Nullable<decimal> sscResultWeighted { get; set; }
                                public Nullable<decimal> sscResultGpaW4s { get; set; }
                                public Nullable<decimal> sscMarks { get; set; }
                                public Nullable<int> sscPassingYear { get; set; }
                                public string hscExamTypeCode { get; set; }
                                public string hscEduBoard { get; set; }
                                public string hscInstitute { get; set; }
                                public string hscRollNo { get; set; }
                                public string hscGroupOrSub { get; set; }
                                public string hscResultDivision { get; set; }
                                public Nullable<decimal> hscResult { get; set; }
                                public Nullable<decimal> hscResultWeighted { get; set; }
                                public Nullable<decimal> hscResultGpaW4s { get; set; }
                                public Nullable<decimal> hscMarks { get; set; }
                                public Nullable<int> hscPassingYear { get; set; }
                                public string ChoiceStr1 { get; set; }
                                public string ChoiceStr2 { get; set; }
                                public string ChoiceStr3 { get; set; }
                                public string ChoiceStr4 { get; set; }
                                public string ChoiceStr5 { get; set; }
                                public string ChoiceStr6 { get; set; }
                                public string fatherName { get; set; }
                                public Nullable<decimal> TotalWeighted { get; set; }
                             * 
                             * */

                            string fileName = admUnitStr + "_" + sessionStr + "_All" + ".xlsx";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.AddWorksheet(dt, "All_" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt").Replace(":", "-").Replace(" ", "-"));
                                //wb.Worksheets.Add(dt);

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);

                                }
                                Response.Flush();
                                Response.Close();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "All : No data found.";
                            lblMessage.ForeColor = Color.Crimson;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "1") //merit selected
                    {
                        if (meritList.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = ListToDataTableManager.ToDataTable(meritList);

                            dt.Columns.Remove("acaCalID");
                            dt.Columns.Remove("admUnitID");
                            dt.Columns.Remove("IsPaid");
                            dt.Columns.Remove("sscResultGpaW4s");
                            dt.Columns.Remove("hscResultGpaW4s");
                            dt.Columns.Remove("sscMarks");
                            dt.Columns.Remove("hscMarks");
                            dt.Columns.Remove("hscInstitute");
                            dt.Columns.Remove("sscInstitute");

                            string fileName = admUnitStr + "_" + sessionStr + "_NQ" + ".xlsx";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.AddWorksheet(dt, "NQ_" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt").Replace(":", "-").Replace(" ", "-"));
                                //wb.Worksheets.Add(dt);

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);

                                }
                                Response.Flush();
                                Response.Close();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Merit : No data found.";
                            lblMessage.ForeColor = Color.Crimson;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "2") //Special Quota selected
                    {
                        if (specialQuotaList.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = ListToDataTableManager.ToDataTable(specialQuotaList);

                            dt.Columns.Remove("acaCalID");
                            dt.Columns.Remove("admUnitID");
                            dt.Columns.Remove("IsPaid");
                            dt.Columns.Remove("sscResultGpaW4s");
                            dt.Columns.Remove("hscResultGpaW4s");
                            dt.Columns.Remove("sscMarks");
                            dt.Columns.Remove("hscMarks");
                            dt.Columns.Remove("hscInstitute");
                            dt.Columns.Remove("sscInstitute");

                            string fileName = admUnitStr + "_" + sessionStr + "_special_quota" + ".xlsx";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.AddWorksheet(dt, "SQ_" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt").Replace(":", "-").Replace(" ", "-"));
                                //wb.Worksheets.Add(dt);

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);

                                }
                                Response.Flush();
                                Response.Close();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Special Quota : No data found.";
                            lblMessage.ForeColor = Color.Crimson;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "3") // freedom fighter selected
                    {
                        if (freedomFighterList.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = ListToDataTableManager.ToDataTable(freedomFighterList);

                            dt.Columns.Remove("acaCalID");
                            dt.Columns.Remove("admUnitID");
                            dt.Columns.Remove("IsPaid");
                            dt.Columns.Remove("sscResultGpaW4s");
                            dt.Columns.Remove("hscResultGpaW4s");
                            dt.Columns.Remove("sscMarks");
                            dt.Columns.Remove("hscMarks");
                            dt.Columns.Remove("hscInstitute");
                            dt.Columns.Remove("sscInstitute");

                            string fileName = admUnitStr + "_" + sessionStr + "_FF" + ".xlsx";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.AddWorksheet(dt, "FF_" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt").Replace(":", "-").Replace(" ", "-"));
                                //wb.Worksheets.Add(dt);

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);

                                }
                                Response.Flush();
                                Response.Close();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Freedom Fighter : No data found.";
                            lblMessage.ForeColor = Color.Crimson;
                        }
                    }
                    else if (ddlDownloadMode.SelectedValue == "4") //tribal selected
                    {
                        if (tribalList.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = ListToDataTableManager.ToDataTable(tribalList);

                            dt.Columns.Remove("acaCalID");
                            dt.Columns.Remove("admUnitID");
                            dt.Columns.Remove("IsPaid");
                            dt.Columns.Remove("sscResultGpaW4s");
                            dt.Columns.Remove("hscResultGpaW4s");
                            dt.Columns.Remove("sscMarks");
                            dt.Columns.Remove("hscMarks");
                            dt.Columns.Remove("hscInstitute");
                            dt.Columns.Remove("sscInstitute");

                            string fileName = admUnitStr + "_" + sessionStr + "_EM" + ".xlsx";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.AddWorksheet(dt, "EM_" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt").Replace(":", "-").Replace(" ", "-"));
                                //wb.Worksheets.Add(dt);

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);

                                }
                                Response.Flush();
                                Response.Close();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Ethnic Minority : No data found.";
                            lblMessage.ForeColor = Color.Crimson;
                        }
                    }

                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error5: " + ex.Message + "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
            else
            {
                lblMessage.Text = "Please Select Faculty and Session for Load Data !!";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }


        }

        private List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> LoadCandidateInformation(long facultyId, int acaCalId)
        {
            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result> candidateScoreList = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result>();

            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduateV2_Result> candidateScoreListTemp = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduateV2_Result>();
            using (var db = new CandidateDataManager())
            {
                candidateScoreListTemp = db.AdmissionDB.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduateV2(facultyId, acaCalId).ToList();
            }

            if (candidateScoreListTemp.Count > 0)
            {
                foreach (var tempData in candidateScoreListTemp)
                {
                    DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result spTemp = new DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result();
                    spTemp.TestRoll = tempData.TestRoll;
                    spTemp.MarksObtained = tempData.MarksObtained;
                    spTemp.MarksObtainedWeighted = tempData.MarksObtainedWeighted;
                    spTemp.acaCalID = tempData.acaCalID;
                    spTemp.admUnitID = tempData.admUnitID;
                    spTemp.PaymentId = tempData.PaymentId;
                    spTemp.IsPaid = tempData.IsPaid;
                    spTemp.isApproved = tempData.isApproved;
                    spTemp.candidateName = tempData.candidateName;
                    spTemp.candidateSmsPhone = tempData.candidateSmsPhone;
                    spTemp.quota = tempData.quota;
                    spTemp.sscExamTypeCode = tempData.sscExamTypeCode;
                    spTemp.sscEduBoard = tempData.sscEduBoard;
                    spTemp.sscInstitute = tempData.sscInstitute;
                    spTemp.sscRollNo = tempData.sscRollNo;
                    spTemp.sscGroupOrSub = tempData.sscGroupOrSub;
                    spTemp.sscResultDivision = tempData.sscResultDivision;
                    spTemp.sscResult = tempData.sscResult;
                    spTemp.sscResultWeighted = tempData.sscResultWeighted;
                    spTemp.sscResultGpaW4s = tempData.sscResultGpaW4s;
                    spTemp.sscMarks = tempData.sscMarks;
                    spTemp.sscPassingYear = tempData.sscPassingYear;
                    spTemp.hscExamTypeCode = tempData.hscExamTypeCode;
                    spTemp.hscEduBoard = tempData.hscEduBoard;
                    spTemp.hscInstitute = tempData.hscInstitute;
                    spTemp.hscRollNo = tempData.hscRollNo;
                    spTemp.hscGroupOrSub = tempData.hscGroupOrSub;
                    spTemp.hscResultDivision = tempData.hscResultDivision;
                    spTemp.hscResult = tempData.hscResult;
                    spTemp.hscResultWeighted = tempData.hscResultWeighted;
                    spTemp.hscResultGpaW4s = tempData.hscResultGpaW4s;
                    spTemp.hscPassingYear = tempData.hscPassingYear;
                    spTemp.ChoiceStr1 = tempData.ChoiceStr1;
                    spTemp.ChoiceStr2 = tempData.ChoiceStr2;
                    spTemp.ChoiceStr3 = tempData.ChoiceStr3;
                    spTemp.ChoiceStr4 = tempData.ChoiceStr4;
                    spTemp.ChoiceStr5 = tempData.ChoiceStr5;
                    spTemp.ChoiceStr6 = tempData.ChoiceStr6;
                    spTemp.fatherName = tempData.fatherName;
                    spTemp.fatherMobile = tempData.fatherMobile;
                    spTemp.TotalWeighted = tempData.TotalWeighted;


                    candidateScoreList.Add(spTemp);
                }
            }


            return candidateScoreList;
        }




    }
}