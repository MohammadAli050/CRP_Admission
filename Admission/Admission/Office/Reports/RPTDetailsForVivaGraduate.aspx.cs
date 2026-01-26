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
    public partial class RPTDetailsForVivaGraduate : PageBase
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
            //ddlDownloadMode.Items.Add(new ListItem("Military", "2")); //2
            //ddlDownloadMode.Items.Add(new ListItem("Freedom Fighter", "3")); //3
            //ddlDownloadMode.Items.Add(new ListItem("Tribal", "4")); //4

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

        private void DeleteSession()
        {
            SessionSGD.DeleteFromSession("list");
            SessionSGD.DeleteFromSession("fileName");
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            int examMarks = -1;

            if (!string.IsNullOrEmpty(txtTotalMarks.Text.Trim()))
            {
                examMarks = Convert.ToInt32(txtTotalMarks.Text.Trim());
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
                try
                {
                    string FileName = Path.GetFileName(fuExcel.FileName);
                    SessionSGD.SaveObjToSession(FileName, "fileName");
                    if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                    {
                        string UploadFileLocation = string.Empty;
                        UploadFileLocation = Server.MapPath("~/Upload/TEMP");
                        try
                        {
                            string savePath = UploadFileLocation + FileName;
                            fuExcel.SaveAs(savePath);


                            SheetName sheet = aFileConverterObj.PassFileName(savePath).FirstOrDefault();
                            DataTable dt = aFileConverterObj.ReadExcelFileDOM(savePath, FileName, sheet.Id);
                            DataRow row = dt.Rows[0];
                            dt.Rows.Remove(row);

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
                                }
                            }

                            lblMessage.Text = "Upload Complete";
                            messagePanel.CssClass = "alert alert-success";
                            btnDownload.Enabled = true;



                            #region N/A
                            //List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> candidateScoreList =
                            //    new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{
                            //    string testRoll = null;
                            //    string score = null;
                            //    for (int j = 0; j < dt.Columns.Count; j++)
                            //    {
                            //        if (j == 0)
                            //        {
                            //            testRoll = dt.Rows[i][j].ToString().Trim();
                            //        }
                            //        if (j == 1)
                            //        {
                            //            score = dt.Rows[i][j].ToString().Trim();
                            //        }
                            //    }

                            //    if (!string.IsNullOrEmpty(testRoll))
                            //    {
                            //        DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result candidateScoreObj = null;
                            //        try
                            //        {
                            //            using (var db = new OfficeDataManager())
                            //            {
                            //                candidateScoreObj = db.AdmissionDB.SPRptGetCandidateDetailsByTestRollFromExcel(testRoll, score, admUnitId, acaCalId, examMarks).FirstOrDefault();
                            //            }
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            lblMessage.Text = "Error1: " + ex.Message + "; " + ex.InnerException.Message + "; TestRoll: " + testRoll + ", Score: " + score;
                            //            messagePanel.CssClass = "alert alert-danger";
                            //            messagePanel.Visible = true;
                            //            return;
                            //        }
                            //        if (candidateScoreObj != null)
                            //        {
                            //            candidateScoreList.Add(candidateScoreObj);
                            //        }
                            //        else
                            //        {
                            //            DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result newObj = new DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result();
                            //            newObj.TestRoll = testRoll;
                            //            newObj.MarksObtained = score;

                            //            candidateScoreList.Add(newObj);

                            //            lblUpl.Text += "Some Test Rolls did not return any information;  ";
                            //            lblUpl.ForeColor = Color.Crimson;
                            //        }
                            //    }

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

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> candidateScoreList = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                    //candidateScoreList = SessionSGD.GetListFromSession<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>("list");

                    candidateScoreList = LoadCandidateInformationOld(facultyId, acaCald);

                    if (candidateScoreList != null && candidateScoreList.Count > 0)
                    {

                        //Order the list is descending order of TotalWeighted value
                        candidateScoreList = candidateScoreList.OrderByDescending(c => c.TotalPointsWeighted).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> meritList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> militaryList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> freedomFighterList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> tribalList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();


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

                            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> _tempListWithoutMerit
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                            _tempListWithoutMerit = candidateScoreList.Except(meritList).ToList();

                            militaryList = _tempListWithoutMerit.OrderByDescending(c => c.TotalPointsWeighted)
                                .Where(c => c.quota == "Special Quota").Take(numberToBeTakenSpecialQuota).ToList();

                            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> _tempListWithoutMeritMilitary
                                = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                            _tempListWithoutMeritMilitary = _tempListWithoutMerit.Except(militaryList).ToList();

                            freedomFighterList = _tempListWithoutMeritMilitary.OrderByDescending(c => c.TotalPointsWeighted)
                                .Where(c => c.quota == "Freedom Fighter").Take(numberToBeTakenFF).ToList();

                            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> _tempListWithoutMeritMilitaryFreedomFighter
                                = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                            _tempListWithoutMeritMilitaryFreedomFighter = _tempListWithoutMeritMilitary.Except(freedomFighterList).ToList();

                            tribalList = _tempListWithoutMeritMilitaryFreedomFighter.OrderByDescending(c => c.TotalPointsWeighted)
                                .Where(c => c.quota == "Ethnic Minority").Take(numberToBeTakenTribal).ToList();
                        }

                        if (ddlDownloadMode.SelectedValue == "-1") // all selected
                        {
                            if (candidateScoreList.Count > 0)
                            {
                                string count = candidateScoreList.Count.ToString() != null ? candidateScoreList.Count.ToString() : "0";
                                string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                                ReportDataSource rds = new ReportDataSource("DataSet", candidateScoreList);

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
                                ReportDataSource rds = new ReportDataSource("DataSet");
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

                                ReportDataSource rds = new ReportDataSource("DataSet", meritList);

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
                                ReportDataSource rds = new ReportDataSource("DataSet");
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(rds);
                                ReportViewer1.Visible = false;
                            }
                        }
                        else if (ddlDownloadMode.SelectedValue == "2") //Special Quota selected
                        {
                            if (militaryList.Count > 0)
                            {
                                string count = militaryList.Count.ToString() != null ? militaryList.Count.ToString() : "0";
                                string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                                ReportDataSource rds = new ReportDataSource("DataSet", militaryList);

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
                                ReportDataSource rds = new ReportDataSource("DataSet");
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

                                ReportDataSource rds = new ReportDataSource("DataSet", freedomFighterList);

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
                                ReportDataSource rds = new ReportDataSource("DataSet");
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

                                ReportDataSource rds = new ReportDataSource("DataSet", tribalList);

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
                                ReportDataSource rds = new ReportDataSource("DataSet");
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(rds);
                                ReportViewer1.Visible = false;
                            }
                        }


                        #region N/A
                        //if (candidateScoreList.Count > 0)
                        //{
                        //    string count = candidateScoreList.Count.ToString() != null ? candidateScoreList.Count.ToString() : "0";
                        //    string printDate = DateTime.Now.ToString("dd/MM/yyyy");

                        //    ReportDataSource rds = new ReportDataSource("DataSet", candidateScoreList);

                        //    ReportParameter p1 = new ReportParameter("ProgramName", ddlAdmUnit.SelectedItem.Text.ToString());
                        //    ReportParameter p2 = new ReportParameter("Count", count);
                        //    ReportParameter p3 = new ReportParameter("PrintDate", printDate);


                        //    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                        //    ReportViewer1.LocalReport.DataSources.Clear();
                        //    ReportViewer1.LocalReport.DataSources.Add(rds);
                        //    ReportViewer1.Visible = true;
                        //}
                        //else
                        //{
                        //    ReportDataSource rds = new ReportDataSource("DataSet");
                        //    ReportViewer1.LocalReport.DataSources.Clear();
                        //    ReportViewer1.LocalReport.DataSources.Add(rds);
                        //    ReportViewer1.Visible = false;
                        //} 
                        #endregion
                    }
                    else
                    {
                        lblMessage.Text = "No data found for process!";
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
            catch (Exception ex)
            {
                lblMessage.Text = "Error3: " + ex.Message + "; " + ex.InnerException.Message;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }

        #region Old Methods


        protected void btnExportToExcel_ClickOld(object sender, EventArgs e)
        {
            long admUnitId = -1;
            int acaCalId = -1;

            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);
            acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            string admUnitStr = string.Empty;
            string sessionStr = string.Empty;

            string admUnitWithoutSpaces = ddlAdmUnit.SelectedItem.Text.Replace(" ", "_").Replace("&", "And");
            admUnitStr = admUnitWithoutSpaces;

            string sessionStrWithoutHyphenSpace = ddlSession.SelectedItem.Text.Replace(" - ", "_").Replace(" ", "_");
            sessionStr = sessionStrWithoutHyphenSpace;

            try
            {

                if (admUnitId > 0 && acaCalId > 0)
                {
                    lblMessage.Text = "";

                    List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> candidateScoreList = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                    //candidateScoreList = SessionSGD.GetListFromSession<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>("list");

                    candidateScoreList = LoadCandidateInformationOld(admUnitId, acaCalId);

                    if (candidateScoreList != null && candidateScoreList.Count > 0)
                    {
                        
                        //Order the list is descending order of TotalWeighted value
                        candidateScoreList = candidateScoreList.OrderByDescending(c => c.TotalPointsWeighted).ToList();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> meritList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> militaryList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> freedomFighterList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                        List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> tribalList
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

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

                            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> _tempListWithoutMerit
                            = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                            _tempListWithoutMerit = candidateScoreList.Except(meritList).ToList();

                            militaryList = _tempListWithoutMerit.OrderByDescending(c => c.TotalPointsWeighted)
                                .Where(c => c.quota == "Special Quota").Take(numberToBeTakenSpecialQuota).ToList();

                            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> _tempListWithoutMeritMilitary
                                = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                            _tempListWithoutMeritMilitary = _tempListWithoutMerit.Except(militaryList).ToList();

                            freedomFighterList = _tempListWithoutMeritMilitary.OrderByDescending(c => c.TotalPointsWeighted)
                                .Where(c => c.quota == "Freedom Fighter").Take(numberToBeTakenFF).ToList();

                            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> _tempListWithoutMeritMilitaryFreedomFighter
                                = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();

                            _tempListWithoutMeritMilitaryFreedomFighter = _tempListWithoutMeritMilitary.Except(freedomFighterList).ToList();

                            tribalList = _tempListWithoutMeritMilitaryFreedomFighter.OrderByDescending(c => c.TotalPointsWeighted)
                                .Where(c => c.quota == "Ethnic Minority").Take(numberToBeTakenTribal).ToList();
                        }

                        if (ddlDownloadMode.SelectedValue == "-1") // all selected
                        {
                            if (candidateScoreList.Count > 0)
                            {
                                try
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

                                    #region N/A
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
                                    #endregion

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
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "All : No data found. Exception: " + ex.Message.ToString();
                                    lblMessage.ForeColor = Color.Crimson;
                                    return;
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
                                try
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
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "Non-Quota : No data found. Exception: " + ex.Message.ToString();
                                    lblMessage.ForeColor = Color.Crimson;
                                    return;
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Non-Quota : No data found.";
                                lblMessage.ForeColor = Color.Crimson;
                            }
                        }
                        else if (ddlDownloadMode.SelectedValue == "2") //Special Quota selected
                        {
                            if (militaryList.Count > 0)
                            {
                                try
                                {
                                    DataTable dt = new DataTable();
                                    dt = ListToDataTableManager.ToDataTable(militaryList);

                                    dt.Columns.Remove("acaCalID");
                                    dt.Columns.Remove("admUnitID");
                                    dt.Columns.Remove("IsPaid");
                                    dt.Columns.Remove("sscResultGpaW4s");
                                    dt.Columns.Remove("hscResultGpaW4s");
                                    dt.Columns.Remove("sscMarks");
                                    dt.Columns.Remove("hscMarks");
                                    dt.Columns.Remove("hscInstitute");
                                    dt.Columns.Remove("sscInstitute");

                                    string fileName = admUnitStr + "_" + sessionStr + "_SQ" + ".xlsx";

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
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "Special Quota : No data found. Exception: " + ex.Message.ToString();
                                    lblMessage.ForeColor = Color.Crimson;
                                    return;
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
                                try
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
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "Freedom Fighter : No data found. Exception: " + ex.Message.ToString();
                                    lblMessage.ForeColor = Color.Crimson;
                                    return;
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
                                try
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
                                catch (Exception ex)
                                {
                                    lblMessage.Text = "Ethnic Minority : No data found. Exception: " + ex.Message.ToString();
                                    lblMessage.ForeColor = Color.Crimson;
                                    return;
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Ethnic Minority : No data found.";
                                lblMessage.ForeColor = Color.Crimson;
                            }
                        }
                    }
                    else
                    {
                        lblMessage.Text = "No data found for process!";
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }
                else
                {
                    lblMessage.Text = "Please select Faculty & Session";
                    lblMessage.ForeColor = Color.Crimson;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error5: " + ex.Message + "; " + ex.InnerException.Message;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
                return;
            }
        }


        private List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> LoadCandidateInformationOld(long facultyId, int acaCalId)
        {
            List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result> candidateScoreList = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result>();


            List<DAL.ExcelTestRollAndScoreTemp> etrastList = null;
            using (var db = new GeneralDataManager())
            {
                etrastList = db.AdmissionDB.ExcelTestRollAndScoreTemps.Where(x => x.AdmissionUnitId == facultyId && x.AcaCalId == acaCalId).ToList();
                if (etrastList != null && etrastList.Count > 0)
                {
                    foreach (var tData in etrastList)
                    {
                        DAL.SPRptGetCandidateDetailsByTestRollFromExcel_Result candidateScoreObj = null;
                        try
                        {

                            candidateScoreObj = db.AdmissionDB.SPRptGetCandidateDetailsByTestRollFromExcel(tData.TestRoll, tData.Score, tData.AdmissionUnitId, tData.AcaCalId, tData.ExamMark).FirstOrDefault();

                            if (candidateScoreObj != null)
                            {
                                candidateScoreList.Add(candidateScoreObj);
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                    }
                }
            }


            #region N/A
            //if (candidateScoreListTemp.Count > 0)
            //{
            //    foreach (var tempData in candidateScoreListTemp)
            //    {
            //        DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result spTemp = new DAL.SPRptGetCandidateDetailsByTestRollFromExcelUndergraduate_Result();
            //        spTemp.TestRoll = tempData.TestRoll;
            //        spTemp.MarksObtained = tempData.MarksObtained;
            //        spTemp.MarksObtainedWeighted = tempData.MarksObtainedWeighted;
            //        spTemp.acaCalID = tempData.acaCalID;
            //        spTemp.admUnitID = tempData.admUnitID;
            //        spTemp.PaymentId = tempData.PaymentId;
            //        spTemp.IsPaid = tempData.IsPaid;
            //        spTemp.isApproved = tempData.isApproved;
            //        spTemp.candidateName = tempData.candidateName;
            //        spTemp.candidateSmsPhone = tempData.candidateSmsPhone;
            //        spTemp.quota = tempData.quota;
            //        spTemp.sscExamTypeCode = tempData.sscExamTypeCode;
            //        spTemp.sscEduBoard = tempData.sscEduBoard;
            //        spTemp.sscInstitute = tempData.sscInstitute;
            //        spTemp.sscRollNo = tempData.sscRollNo;
            //        spTemp.sscGroupOrSub = tempData.sscGroupOrSub;
            //        spTemp.sscResultDivision = tempData.sscResultDivision;
            //        spTemp.sscResult = tempData.sscResult;
            //        spTemp.sscResultWeighted = tempData.sscResultWeighted;
            //        spTemp.sscResultGpaW4s = tempData.sscResultGpaW4s;
            //        spTemp.sscMarks = tempData.sscMarks;
            //        spTemp.sscPassingYear = tempData.sscPassingYear;
            //        spTemp.hscExamTypeCode = tempData.hscExamTypeCode;
            //        spTemp.hscEduBoard = tempData.hscEduBoard;
            //        spTemp.hscInstitute = tempData.hscInstitute;
            //        spTemp.hscRollNo = tempData.hscRollNo;
            //        spTemp.hscGroupOrSub = tempData.hscGroupOrSub;
            //        spTemp.hscResultDivision = tempData.hscResultDivision;
            //        spTemp.hscResult = tempData.hscResult;
            //        spTemp.hscResultWeighted = tempData.hscResultWeighted;
            //        spTemp.hscResultGpaW4s = tempData.hscResultGpaW4s;
            //        spTemp.hscPassingYear = tempData.hscPassingYear;
            //        spTemp.ChoiceStr1 = tempData.ChoiceStr1;
            //        spTemp.ChoiceStr2 = tempData.ChoiceStr2;
            //        spTemp.ChoiceStr3 = tempData.ChoiceStr3;
            //        spTemp.ChoiceStr4 = tempData.ChoiceStr4;
            //        spTemp.ChoiceStr5 = tempData.ChoiceStr5;
            //        spTemp.ChoiceStr6 = tempData.ChoiceStr6;
            //        spTemp.fatherName = tempData.fatherName;
            //        spTemp.TotalWeighted = tempData.TotalWeighted;


            //        candidateScoreList.Add(spTemp);
            //    }
            //} 
            #endregion


            return candidateScoreList;
        }

        #endregion

        #region New Download Methods

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            // 1. Validate Inputs
            long admUnitId;
            int acaCalId;

            if (!long.TryParse(ddlAdmUnit.SelectedValue, out admUnitId) ||
                !int.TryParse(ddlSession.SelectedValue, out acaCalId) ||
                admUnitId <= 0 || acaCalId <= 0)
            {
                lblMessage.Text = "Please select Faculty & Session";
                lblMessage.ForeColor = Color.Crimson;
                return;
            }

            string admUnitStr = ddlAdmUnit.SelectedItem.Text.Replace(" ", "_").Replace("&", "And");
            string sessionStr = ddlSession.SelectedItem.Text.Replace(" - ", "_").Replace(" ", "_");

            try
            {
                lblMessage.Text = "";

                // 2. Load Data (Make sure you implemented Step 1 above!)
                var candidateScoreList = LoadCandidateInformation(admUnitId, acaCalId);

                if (candidateScoreList == null || candidateScoreList.Count == 0)
                {
                    lblMessage.Text = "No data found for process!";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                    return;
                }

                // 3. Process Logic (In Memory)
                // Order the master list once
                var masterList = candidateScoreList.OrderByDescending(c => c.TotalPointsWeighted).ToList();

                List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelBulk_Result> finalExportList = new List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelBulk_Result>();
                string suffix = "";

                int totalNumber = 0;
                int.TryParse(txtTotalNumber.Text, out totalNumber);

                // Determine which list to export
                if (ddlDownloadMode.SelectedValue == "-1") // All
                {
                    finalExportList = masterList;
                    suffix = "All";
                }
                else if (totalNumber > 0) // Calculation Logic
                {
                    // Pre-calculate counts
                    int countMerit = (int)Math.Ceiling(totalNumber * 0.60);
                    int countSpecial = (int)Math.Ceiling(totalNumber * 0.37);
                    int countFF = (int)Math.Ceiling(totalNumber * 0.02);
                    int countTribal = (int)Math.Ceiling(totalNumber * 0.01);

                    // OPTIMIZATION: Use Skip/Take instead of Except (Except is very slow on large objects)

                    // A. Merit
                    var meritList = masterList.Take(countMerit).ToList();

                    // Get Remainder (Efficiently)
                    var remainderPool = masterList.Skip(countMerit).ToList();

                    if (ddlDownloadMode.SelectedValue == "1") // Merit
                    {
                        finalExportList = meritList;
                        suffix = "NQ";
                    }
                    else
                    {
                        // B. Special Quota (From Remainder)
                        var militaryList = remainderPool
                            .Where(c => c.quota == "Special Quota")
                            .OrderByDescending(c => c.TotalPointsWeighted)
                            .Take(countSpecial)
                            .ToList();

                        if (ddlDownloadMode.SelectedValue == "2") // Special Quota
                        {
                            finalExportList = militaryList;
                            suffix = "SQ";
                        }
                        else
                        {
                            // Remove selected military from pool to avoid duplicates in next quota
                            // Note: We use a HashSet for O(1) lookups during removal
                            var militaryIds = new HashSet<string>(militaryList.Select(x => x.TestRoll));
                            remainderPool.RemoveAll(x => militaryIds.Contains(x.TestRoll));

                            // C. Freedom Fighter
                            var ffList = remainderPool
                                .Where(c => c.quota == "Freedom Fighter")
                                .OrderByDescending(c => c.TotalPointsWeighted)
                                .Take(countFF)
                                .ToList();

                            if (ddlDownloadMode.SelectedValue == "3") // FF
                            {
                                finalExportList = ffList;
                                suffix = "FF";
                            }
                            else
                            {
                                var ffIds = new HashSet<string>(ffList.Select(x => x.TestRoll));
                                remainderPool.RemoveAll(x => ffIds.Contains(x.TestRoll));

                                // D. Tribal / Ethnic
                                var tribalList = remainderPool
                                    .Where(c => c.quota == "Ethnic Minority")
                                    .OrderByDescending(c => c.TotalPointsWeighted)
                                    .Take(countTribal)
                                    .ToList();

                                if (ddlDownloadMode.SelectedValue == "4") // Tribal
                                {
                                    finalExportList = tribalList;
                                    suffix = "EM";
                                }
                            }
                        }
                    }
                }

                // 4. Export to Excel
                if (finalExportList.Count > 0)
                {
                    GenerateExcel(finalExportList, admUnitStr, sessionStr, suffix);
                }
                else
                {
                    lblMessage.Text = $"{suffix} : No data found.";
                    lblMessage.ForeColor = Color.Crimson;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }

        private List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelBulk_Result> LoadCandidateInformation(long facultyId, int acaCalId)
        {
            // OPTIMIZATION: Fetch ALL data in a single database call.
            // Do NOT loop here. 
            using (var db = new GeneralDataManager())
            {
                // Ideally, create a new Stored Procedure in SQL Server like: "SPRptGetCandidateDetailsByTestRollFromExcelBulk"

                // Assuming you create a stored procedure that accepts UnitId and AcaCalId 
                // and returns the JOINED result immediately:
                return db.AdmissionDB.SPRptGetCandidateDetailsByTestRollFromExcelBulk(facultyId, acaCalId,"").ToList();

            }
        }

        // Helper method to keep code clean and remove DataTable conversion overhead
        private void GenerateExcel(List<DAL.SPRptGetCandidateDetailsByTestRollFromExcelBulk_Result> data, string unit, string session, string type)
        {
            string fileName = $"{unit}_{session}_{type}.xlsx";
            string sheetName = $"{type}_{DateTime.Now:dd-MM-yyyy}";

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(sheetName);

                // OPTIMIZATION: EPPlus LoadFromCollection is faster than converting to DataTable
                // It handles nullable types automatically.
                ws.Cell(1, 1).InsertTable(data);

                // Optional: Hide columns you don't want using ws.Column(x).Hide() or creating a ViewModel
                // If you strictly need to remove columns, use a Select() projection before calling this method.

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                }
                Response.Flush();
                Response.SuppressContent = true; // Better than Close/End
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        #endregion
    }
}