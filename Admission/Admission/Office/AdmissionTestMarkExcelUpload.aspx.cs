using Admission.Admission.Admin;
using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DAL;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonUtility.FileConversion;

namespace Admission.Admission.Office
{
    public partial class AdmissionTestMarkExcelUpload : PageBase
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

            if (uRole.ToLower() == "candidate")
            {

                SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                Response.Redirect("~/Admission/Home.aspx", false);
            }

            if (!IsPostBack)
            {
                GvStudent.Visible = false;
                GVNotUploadedStudentList.Visible = false;
                GVTotalStudentList.Visible = false;
                LoadDDL();
                LoadSSCHSCPercentageList(0, 0, 0);
                //btnSave.Visible = false;
            }
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                //DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSessionAddUpdate, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                GvStudent.Visible = false;
                GVNotUploadedStudentList.Visible = false;
                GVTotalStudentList.Visible = false;

                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                List<DAL.AdmissionSetup> admSetup = new List<DAL.AdmissionSetup>();

                ddlFaculty.Items.Clear();
                ddlFaculty.Items.Add(new ListItem("-All-", "0"));
                ddlFaculty.AppendDataBoundItems = true;

                //ddlFacultyAddUpdate.Items.Clear();
                //ddlFacultyAddUpdate.Items.Add(new ListItem("-All-", "0"));
                //ddlFacultyAddUpdate.AppendDataBoundItems = true;

                using (var db = new OfficeDataManager())
                {
                    admSetup = db.GetAllAdmissionSetup_AD();


                    admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive == true).ToList();  // only for Bachelors

                    if (admSetup != null && admSetup.Count > 0)
                    {
                        foreach (var item in admSetup)
                        {
                            var admUnit = db.GetAllAdmissionUnit().Where(x => x.ID == item.AdmissionUnitID).FirstOrDefault();
                            ddlFaculty.Items.Add(new ListItem(admUnit.UnitName, item.ID.ToString()));
                            //ddlFacultyAddUpdate.Items.Add(new ListItem(admUnit.UnitName, item.ID.ToString()));
                        }

                        //ddlExamType.Enabled = true;
                    }

                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }
        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        //protected void btnLoad_Click(object sender, EventArgs e)
        //{
        //    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
        //    int facultyId = Convert.ToInt32(ddlFaculty.SelectedValue);
        //    //int examTypeId = Convert.ToInt32(ddlExamType.SelectedValue);


        //    LoadSSCHSCPercentageList(acaCalId, facultyId, examTypeId);
        //}



        protected void btnAddUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void LoadSSCHSCPercentageList(int acaCal, int facultyId, int exmTypeId)
        {

        }










        protected void lnkSampleExcel_Click(object sender, EventArgs e)
        {
            try
            {

                string fileName = "Bup Admission Merit List Excel sample.xlsx";

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.TransmitFile(Server.MapPath("~/Upload/SampleExcel/" + fileName));

                Response.Flush();
                Response.SuppressContent = true;
                Response.End();

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {

                GVNotUploadedStudentList.Visible = false;
                GVTotalStudentList.Visible = false;

                GvStudent.Visible = true;
                lblTotalStudent.Visible = false;
                GVTotalStudentList.Visible = false;


                GVNotUploadedStudentList.Visible = false;
                lblNotMigratedStudent.Visible = false;

                lblMessage.Text = "";


                #region Get data from DB
                int sessionID = Convert.ToInt32(ddlSession.SelectedValue);
                int unitID = 0;
                if (ddlFaculty.SelectedValue == "")
                {
                    unitID = 0;
                }
                else
                {
                    unitID = Convert.ToInt32(ddlFaculty.SelectedValue);
                }


                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter { ParameterName = "@SessionID", SqlDbType = System.Data.SqlDbType.Int, Value = sessionID });
                parameters.Add(new SqlParameter { ParameterName = "@AdmissionUnitID", SqlDbType = System.Data.SqlDbType.Int, Value = unitID });
                DataTable dt = DataTableManager.GetDataFromQuery("GetAdmissionExamMarkBySessionIdAndUnitId", parameters);



                if (dt != null && dt.Rows.Count > 0)
                {

                    GvStudent.DataSource = dt;
                    GvStudent.DataBind();
                    // 1. Store the full DataTable in Session for subsequent paging
                    Session["GvStudentData"] = dt;
                }
                else
                {
                    showAlert("No data found");
                    return;
                }

                #endregion


            }
            catch { }

        }

        protected void GvStudent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 1. Set the new page index
            GvStudent.PageIndex = e.NewPageIndex;

            // 2. Retrieve the full DataTable from the Session
            DataTable dt = Session["GvStudentData"] as DataTable;

            // 3. Rebind the GridView to display the new page
            if (dt != null)
            {
                GvStudent.DataSource = dt;
                GvStudent.DataBind();
            }
        }
        protected void lnkStudentMigrateButtonDirect_ClickOld(object sender, EventArgs e)
        {
            //#region Validation & Required input value get and check 

            int sessionID = Convert.ToInt32(ddlSession.SelectedValue);
            int unitID = 0;
            if (ddlFaculty.SelectedValue == "")
            {
                unitID = 0;
            }
            else
            {
                unitID = Convert.ToInt32(ddlFaculty.SelectedValue);
            }

            // decimal convertedTo = decimal.Parse(txtConvertMark.Text.Trim());
            // decimal ExamTaken = decimal.Parse(txtTotalMark.Text.Trim());


            //if (admUnitId < 0 || acaCalId < 0)
            //{
            //    lblMessage.Text = "Select Session and Faculty";
            //    messagePanel.CssClass = "alert alert-danger";
            //    messagePanel.Visible = true;
            //    return;
            //}
            //#endregion

            // Session & lebel Reset
            lblMessage.Text = "";
            // DeleteSession();

            //#region Delete Information From Table
            //using (var db = new OfficeDataManager())
            //{
            //    db.AdmissionDB.TruncateExcelTestRollAndScoreTemp();
            //}
            //#endregion

            if (ExcelUpload.HasFile)
            {
                //Process Start
                try
                {
                    string FileName = Path.GetFileName(ExcelUpload.FileName);
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
                            ExcelUpload.SaveAs(savePath);
                            //File Upload to Upload Temp folder End

                            //Read file from Upload temp folder
                            SheetName sheet = aFileConverterObj.PassFileName(savePath).FirstOrDefault();
                            //Populate datatable from excel sheet
                            DataTable dt = aFileConverterObj.ReadExcelFileDOM(savePath, FileName, sheet.Id);

                            //Remove First Row (Header)
                            DataRow row = dt.Rows[0];
                            dt.Rows.Remove(row);
                            int insertcount = 0;

                            if (sessionID > 0 && unitID > 0 && !string.IsNullOrEmpty(txtTotalMark.Text) && !string.IsNullOrEmpty(txtConvertMark.Text))
                            {
                                // DataTable StudentList = (DataTable)Session["DataTableExcelUpload"];

                                if (dt != null && dt.Rows.Count > 0)
                                {

                                    #region Delete Existing Data



                                    List<DAL.AdmissionExamMark> deletedStudent = new List<AdmissionExamMark>();
                                    using (var db = new GeneralDataManager())
                                    {
                                        deletedStudent = db.AdmissionDB.AdmissionExamMarks.Where(x => x.SessionID == sessionID && x.AdmissionUnitID == unitID).ToList();
                                        if (deletedStudent != null && deletedStudent.Any())
                                        {
                                            db.AdmissionDB.AdmissionExamMarks.RemoveRange(deletedStudent);
                                            db.AdmissionDB.SaveChanges();
                                        }
                                    }


                                    #endregion

                                    //  Total = StudentList.Rows.Count;

                                    #region Data Migration Process


                                    foreach (DataRow dtRow in dt.Rows)
                                    {

                                        #region Data Insert into Different Table


                                        StudentNotUpload stdObj = new StudentNotUpload();

                                        string Roll = "", Subject_1 = "", Subject_2 = "", Subject_3 = "", Subject_4 = "", Subject_5 = "", Subject_6 = "", Subject_7 = "", Subject_8 = "",
                                            Subject_9 = "", Subject_10 = "", studentMark = "";




                                        if (!string.IsNullOrEmpty(dtRow[0].ToString()))
                                            Roll = dtRow[0].ToString().Trim();

                                        if (!string.IsNullOrEmpty(dtRow[1].ToString()))
                                            Subject_1 = dtRow[1].ToString().Trim();

                                        if (!string.IsNullOrEmpty(dtRow[2].ToString()))
                                            Subject_2 = dtRow[2].ToString().Trim();



                                        if (!string.IsNullOrEmpty(dtRow[3].ToString()))
                                            Subject_3 = dtRow[3].ToString().Trim();

                                        if (!string.IsNullOrEmpty(dtRow[4].ToString()))
                                            Subject_4 = dtRow[4].ToString().Trim();

                                        if (!string.IsNullOrEmpty(dtRow[5].ToString()))
                                            Subject_5 = dtRow[5].ToString().Trim();

                                        //if (!string.IsNullOrEmpty(dtRow[6].ToString()))
                                        //    Subject_6 = dtRow[6].ToString().Trim();

                                        //if (!string.IsNullOrEmpty(dtRow[7].ToString()))
                                        //    Subject_7 = dtRow[7].ToString().Trim();

                                        //if (!string.IsNullOrEmpty(dtRow[8].ToString()))
                                        //    Subject_8 = dtRow[8].ToString().Trim();

                                        //if (!string.IsNullOrEmpty(dtRow[9].ToString()))
                                        //    Subject_9 = dtRow[9].ToString().Trim();

                                        //if (!string.IsNullOrEmpty(dtRow[10].ToString()))
                                        //    Subject_10 = dtRow[10].ToString().Trim();


                                        //new code start
                                        // Get the value from the DataRow
                                        studentMark = dtRow[11].ToString().Trim();

                                        // Split the string by spaces or parentheses (depending on the exact format)
                                        string[] parts = studentMark.Split(new[] { ' ', '(', ')', '+' }, StringSplitOptions.RemoveEmptyEntries);

                                        // Assuming the last part is the number
                                        studentMark = parts.LastOrDefault();

                                        if (string.IsNullOrEmpty(studentMark))
                                        {
                                            // Handle case where no valid number is found
                                            studentMark = string.Empty;
                                        }


                                        //ends here 



                                        //if (!string.IsNullOrEmpty(dtRow[11].ToString()))
                                        //    studentMark = dtRow[11].ToString().Trim();


                                        decimal Subject_1_val = 0, Subject_2_val = 0, Subject_3_val = 0, Subject_4_val = 0, Subject_5_val = 0;

                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Subject_1))
                                                Subject_1_val = Convert.ToDecimal(Subject_1);
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Subject_2))
                                                Subject_2_val = Convert.ToDecimal(Subject_2);

                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Subject_3))
                                                Subject_3_val = Convert.ToDecimal(Subject_3);
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Subject_4))
                                                Subject_4_val = Convert.ToDecimal(Subject_4);
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Subject_5))
                                                Subject_5_val = Convert.ToDecimal(Subject_5);
                                        }
                                        catch (Exception ex)
                                        {
                                        }


                                        //string numericPart1 = new string(Subject_1.Where(char.IsDigit).ToArray());
                                        //decimal Subject_1_val = !string.IsNullOrEmpty(numericPart1) ? decimal.Parse(numericPart1) : 0;

                                        //string numericPart2 = new string(Subject_2.Where(char.IsDigit).ToArray());
                                        //decimal Subject_2_val = !string.IsNullOrEmpty(numericPart2) ? decimal.Parse(numericPart2) : 0;

                                        //string numericPart3 = new string(Subject_3.Where(char.IsDigit).ToArray());
                                        //decimal Subject_3_val = !string.IsNullOrEmpty(numericPart3) ? decimal.Parse(numericPart3) : 0;

                                        //string numericPart4 = new string(Subject_4.Where(char.IsDigit).ToArray());
                                        //decimal Subject_4_val = !string.IsNullOrEmpty(numericPart4) ? decimal.Parse(numericPart4) : 0;

                                        //string numericPart5 = new string(Subject_5.Where(char.IsDigit).ToArray());
                                        //decimal Subject_5_val = !string.IsNullOrEmpty(numericPart5) ? decimal.Parse(numericPart5) : 0;

                                        //string numericPart6 = new string(Subject_6.Where(char.IsDigit).ToArray());
                                        //decimal Subject_6_val = !string.IsNullOrEmpty(numericPart6) ? decimal.Parse(numericPart6) : 0;

                                        //string numericPart7 = new string(Subject_7.Where(char.IsDigit).ToArray());
                                        //decimal Subject_7_val = !string.IsNullOrEmpty(numericPart7) ? decimal.Parse(numericPart7) : 0;

                                        //string numericPart8 = new string(Subject_8.Where(char.IsDigit).ToArray());
                                        //decimal Subject_8_val = !string.IsNullOrEmpty(numericPart8) ? decimal.Parse(numericPart8) : 0;

                                        //string numericPart9 = new string(Subject_9.Where(char.IsDigit).ToArray());
                                        //decimal Subject_9_val = !string.IsNullOrEmpty(numericPart9) ? decimal.Parse(numericPart9) : 0;

                                        //string numericPart10 = new string(Subject_10.Where(char.IsDigit).ToArray());
                                        //decimal Subject_10_val = !string.IsNullOrEmpty(numericPart10) ? decimal.Parse(numericPart10) : 0;


                                        //string numericPart11 = new string(studentMark.Where(char.IsDigit).ToArray());
                                        //decimal studentMark_val = !string.IsNullOrEmpty(numericPart11) ? decimal.Parse(numericPart11) : 0;

                                        decimal studentMark_val = 0;

                                        if (!string.IsNullOrEmpty(studentMark))
                                            studentMark_val = Convert.ToDecimal(studentMark);

                                        decimal convertedTo = decimal.Parse(txtConvertMark.Text.Trim());
                                        decimal ExamTaken = decimal.Parse(txtTotalMark.Text.Trim());



                                        decimal convertedTotalMark = (studentMark_val * convertedTo) / ExamTaken;


                                        DAL.AdmissionExamMark obj = new DAL.AdmissionExamMark();


                                        if (Roll != "" && Roll != null)
                                        {
                                            using (var db = new GeneralDataManager())
                                            {
                                                obj = db.AdmissionDB.AdmissionExamMarks.Where(x => x.StudentTestRoll == Roll).FirstOrDefault();
                                            }
                                            int studentId = 0;

                                            if (obj == null)
                                            {

                                                AdmissionExamMark newobjInsert = new AdmissionExamMark();

                                                newobjInsert.AdmissionUnitID = unitID;
                                                newobjInsert.SessionID = sessionID;
                                                newobjInsert.StudentTestRoll = Roll;
                                                newobjInsert.Subject1 = Subject_1_val;
                                                newobjInsert.Subject2 = Subject_2_val;
                                                newobjInsert.Subject3 = Subject_3_val;
                                                newobjInsert.Subject4 = Subject_4_val;
                                                newobjInsert.Subject5 = Subject_5_val;
                                                //newobjInsert.Subject6 = Subject_6_val;
                                                //newobjInsert.Subject7 = Subject_7_val;
                                                //newobjInsert.Subject8 = Subject_8_val;
                                                //newobjInsert.Subject9 = Subject_9_val;
                                                //newobjInsert.Subject10 = Subject_10_val;
                                                newobjInsert.ConvertedMarkPercentage = decimal.Parse(txtConvertMark.Text.Trim());
                                                newobjInsert.ConvertedTotalMark = convertedTotalMark;
                                                newobjInsert.TotalMark = decimal.Parse(txtTotalMark.Text.Trim());
                                                newobjInsert.StudentMark = studentMark_val;
                                                newobjInsert.CreatedBy = 1;
                                                newobjInsert.CreatedDate = DateTime.Now;


                                                try
                                                {
                                                    using (var db = new GeneralDataManager())
                                                    {

                                                        db.Insert<DAL.AdmissionExamMark>(newobjInsert);
                                                        if (newobjInsert.ID > 0)
                                                        {
                                                            insertcount += 1;
                                                        }
                                                    }


                                                }
                                                catch (Exception ex)
                                                {
                                                }

                                            }
                                            else
                                            {
                                                obj.AdmissionUnitID = unitID;
                                                obj.SessionID = sessionID;
                                                obj.StudentTestRoll = Roll;
                                                obj.Subject1 = Subject_1_val;
                                                obj.Subject2 = Subject_2_val;
                                                obj.Subject3 = Subject_3_val;
                                                obj.Subject4 = Subject_4_val;
                                                obj.Subject5 = Subject_5_val;
                                                //obj.Subject6 = Subject_6_val;
                                                //obj.Subject7 = Subject_7_val;
                                                //obj.Subject8 = Subject_8_val;
                                                //obj.Subject9 = Subject_9_val;
                                                //obj.Subject10 = Subject_10_val;
                                                obj.ConvertedMarkPercentage = decimal.Parse(txtConvertMark.Text.Trim());
                                                obj.ConvertedTotalMark = convertedTotalMark;
                                                obj.TotalMark = decimal.Parse(txtTotalMark.Text.Trim());
                                                obj.StudentMark = studentMark_val;
                                                obj.ModifiedBy = 1;


                                                try
                                                {
                                                    using (var db = new GeneralDataManager())
                                                    {
                                                        db.Update<DAL.AdmissionExamMark>(obj);

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                }

                                            }
                                        }


                                        //  }
                                        //else // Student Already Exists.
                                        //{

                                        //    stdObj.Roll = Roll;

                                        //    stdObj.Reason = "Student Roll Number Not Exists in Admission Test Roll";

                                        //    //  notUploadedstudentList.Add(stdObj);
                                        //}
                                    }



                                    #endregion









                                    GvStudent.Visible = false;



                                }


                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                #endregion

                                Session["DataTableExcelUpload"] = null;
                                lblMessage.Text = "Upload Complete.Total Inserted: " + insertcount;
                                messagePanel.CssClass = "alert alert-success";

                            }


                            else
                            {
                                lblMessage.Text = "Please Select Session,Faculty,Converted To and Exam Taken  Before Upload";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                Session["DataTableExcelUpload"] = null;
                            }






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
            else
            {
                lblMessage.Text = "Please Select a file first for Upload";
                messagePanel.CssClass = "alert alert-danger";
            }

        }

        protected void lnkStudentMigrateButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (Session["DataTableExcelUpload"] == null)
                {
                    lblMessage.Text = "Please Select a file before migrate";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                //GVNotUploadedStudentList.Visible = true;
                //lblNotMigratedStudent.Visible = true;




                GvStudent.Visible = false;
                lblTotalStudent.Visible = true;
                GVTotalStudentList.Visible = true;

                Session["NotMigratedStudentList"] = null;
                int sessionID = Convert.ToInt32(ddlSession.SelectedValue);
                int unitID = 0;
                if (ddlFaculty.SelectedValue == "")
                {
                    unitID = 0;
                }
                else
                {
                    unitID = Convert.ToInt32(ddlFaculty.SelectedValue);
                }



                int Total = 0, NewInserted = 0;

                string Message = "";

                List<StudentNotUpload> notUploadedstudentList = new List<StudentNotUpload>();

                if (sessionID > 0 && unitID > 0 && !string.IsNullOrEmpty(txtTotalMark.Text) && !string.IsNullOrEmpty(txtConvertMark.Text))
                {
                    DataTable StudentList = (DataTable)Session["DataTableExcelUpload"];

                    if (StudentList != null && StudentList.Rows.Count > 0)
                    {

                        #region Delete Existing Data



                        List<DAL.AdmissionExamMark> deletedStudent = new List<AdmissionExamMark>();
                        using (var db = new GeneralDataManager())
                        {
                            deletedStudent = db.AdmissionDB.AdmissionExamMarks.Where(x => x.SessionID == sessionID && x.AdmissionUnitID == unitID).ToList();
                            if (deletedStudent != null && deletedStudent.Any())
                            {
                                db.AdmissionDB.AdmissionExamMarks.RemoveRange(deletedStudent);
                                db.AdmissionDB.SaveChanges();
                            }
                        }


                        #endregion

                        Total = StudentList.Rows.Count;

                        #region Data Migration Process


                        foreach (DataRow row in StudentList.Rows)
                        {

                            #region Data Insert into Different Table


                            StudentNotUpload stdObj = new StudentNotUpload();

                            string Roll = "", Subject_1 = "", Subject_2 = "", Subject_3 = "", Subject_4 = "", Subject_5 = "", Subject_6 = "", Subject_7 = "", Subject_8 = "",
                                Subject_9 = "", Subject_10 = "", studentMark = "";




                            if (!string.IsNullOrEmpty(row[0].ToString()))
                                Roll = row[0].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[1].ToString()))
                                Subject_1 = row[1].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[2].ToString()))
                                Subject_2 = row[2].ToString().Trim();



                            if (!string.IsNullOrEmpty(row[3].ToString()))
                                Subject_3 = row[3].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[4].ToString()))
                                Subject_4 = row[4].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[5].ToString()))
                                Subject_5 = row[5].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[6].ToString()))
                                Subject_6 = row[6].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[7].ToString()))
                                Subject_7 = row[7].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[8].ToString()))
                                Subject_8 = row[8].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[9].ToString()))
                                Subject_9 = row[9].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[10].ToString()))
                                Subject_10 = row[10].ToString().Trim();

                            if (!string.IsNullOrEmpty(row[11].ToString()))
                                studentMark = row[11].ToString().Trim();


                            string numericPart1 = new string(Subject_1.Where(char.IsDigit).ToArray());
                            decimal Subject_1_val = !string.IsNullOrEmpty(numericPart1) ? decimal.Parse(numericPart1) : 0;

                            string numericPart2 = new string(Subject_2.Where(char.IsDigit).ToArray());
                            decimal Subject_2_val = !string.IsNullOrEmpty(numericPart2) ? decimal.Parse(numericPart2) : 0;

                            string numericPart3 = new string(Subject_3.Where(char.IsDigit).ToArray());
                            decimal Subject_3_val = !string.IsNullOrEmpty(numericPart3) ? decimal.Parse(numericPart3) : 0;

                            string numericPart4 = new string(Subject_4.Where(char.IsDigit).ToArray());
                            decimal Subject_4_val = !string.IsNullOrEmpty(numericPart4) ? decimal.Parse(numericPart4) : 0;

                            string numericPart5 = new string(Subject_5.Where(char.IsDigit).ToArray());
                            decimal Subject_5_val = !string.IsNullOrEmpty(numericPart5) ? decimal.Parse(numericPart5) : 0;

                            string numericPart6 = new string(Subject_6.Where(char.IsDigit).ToArray());
                            decimal Subject_6_val = !string.IsNullOrEmpty(numericPart6) ? decimal.Parse(numericPart6) : 0;

                            string numericPart7 = new string(Subject_7.Where(char.IsDigit).ToArray());
                            decimal Subject_7_val = !string.IsNullOrEmpty(numericPart7) ? decimal.Parse(numericPart7) : 0;

                            string numericPart8 = new string(Subject_8.Where(char.IsDigit).ToArray());
                            decimal Subject_8_val = !string.IsNullOrEmpty(numericPart8) ? decimal.Parse(numericPart8) : 0;

                            string numericPart9 = new string(Subject_9.Where(char.IsDigit).ToArray());
                            decimal Subject_9_val = !string.IsNullOrEmpty(numericPart9) ? decimal.Parse(numericPart9) : 0;

                            string numericPart10 = new string(Subject_10.Where(char.IsDigit).ToArray());
                            decimal Subject_10_val = !string.IsNullOrEmpty(numericPart10) ? decimal.Parse(numericPart10) : 0;


                            string numericPart11 = new string(studentMark.Where(char.IsDigit).ToArray());
                            decimal studentMark_val = !string.IsNullOrEmpty(numericPart11) ? decimal.Parse(numericPart11) : 0;

                            decimal convertedTo = decimal.Parse(txtConvertMark.Text.Trim());
                            decimal ExamTaken = decimal.Parse(txtTotalMark.Text.Trim());



                            decimal convertedTotalMark = (studentMark_val * convertedTo) / ExamTaken;
                            //new code for read images 
                            // Workbook workbook = new Workbook();
                            //// workbook.LoadFromFile(@"C:\Users\Hredoy\Downloads\affiliated institute Student migration Excel sample.xlsx");
                            // Worksheet sheet = workbook.Worksheets[0];
                            // ExcelPicture picture = sheet.Pictures[0];
                            // picture.Picture.Save(@"E:\BUP_Affiliated_Master.git\trunk\Trunk\UIO\EMS\StudentManagement\photo\pic.png", ImageFormat.Png);



                            //end code for read images



                            DAL.AdmissionTestRoll ExistStudnet = new DAL.AdmissionTestRoll();
                            using (var db = new GeneralDataManager())
                            {
                                ExistStudnet = db.AdmissionDB.AdmissionTestRolls.Where(x => x.TestRoll == Roll).FirstOrDefault();
                            }




                            //var InstituteInfo = ucamEntites.AffiliatedInstitutions.Where(x => x.Name == Institute).FirstOrDefault();


                            //var Student =  StudentManager.GetByRoll(Roll);

                            if (ExistStudnet != null) //Insert New Student
                            {
                                //var RegistrationInfo = ucamEntites.StudentRegistrations.Where(x => x.RegistrationNo == Reg).FirstOrDefault();

                                CultureInfo provider = CultureInfo.InvariantCulture;

                                DAL.AdmissionExamMark obj = new DAL.AdmissionExamMark();
                                using (var db = new GeneralDataManager())
                                {
                                    obj = db.AdmissionDB.AdmissionExamMarks.Where(x => x.StudentTestRoll == Roll).FirstOrDefault();
                                }

                                #region delete single student
                                //if(obj != null)
                                //{
                                //    try
                                //    {
                                //        using (var db = new GeneralDataManager())
                                //        {
                                //            db.Delete<DAL.AdmissionExamMark>(obj);

                                //        }

                                //    }
                                //    catch(Exception ex)
                                //    {

                                //    }
                                //}

                                #endregion


                                using (var db = new GeneralDataManager())
                                {
                                    obj = db.AdmissionDB.AdmissionExamMarks.Where(x => x.StudentTestRoll == Roll).FirstOrDefault();
                                }
                                int studentId = 0;

                                if (obj == null)
                                {

                                    AdmissionExamMark newobjInsert = new AdmissionExamMark();

                                    newobjInsert.AdmissionUnitID = unitID;
                                    newobjInsert.SessionID = sessionID;
                                    newobjInsert.StudentTestRoll = Roll;
                                    newobjInsert.Subject1 = Subject_1_val;
                                    newobjInsert.Subject2 = Subject_2_val;
                                    newobjInsert.Subject3 = Subject_3_val;
                                    newobjInsert.Subject4 = Subject_4_val;
                                    newobjInsert.Subject5 = Subject_5_val;
                                    newobjInsert.Subject6 = Subject_6_val;
                                    newobjInsert.Subject7 = Subject_7_val;
                                    newobjInsert.Subject8 = Subject_8_val;
                                    newobjInsert.Subject9 = Subject_9_val;
                                    newobjInsert.Subject10 = Subject_10_val;
                                    newobjInsert.ConvertedMarkPercentage = decimal.Parse(txtConvertMark.Text.Trim());
                                    newobjInsert.ConvertedTotalMark = convertedTotalMark;
                                    newobjInsert.TotalMark = decimal.Parse(txtTotalMark.Text.Trim());
                                    newobjInsert.StudentMark = studentMark_val;
                                    newobjInsert.CreatedBy = 1;
                                    newobjInsert.CreatedDate = DateTime.Now;

                                    try
                                    {
                                        using (var db = new GeneralDataManager())
                                        {
                                            db.Insert<DAL.AdmissionExamMark>(newobjInsert);

                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                }
                                else
                                {
                                    obj.AdmissionUnitID = unitID;
                                    obj.SessionID = sessionID;
                                    obj.StudentTestRoll = Roll;
                                    obj.Subject1 = Subject_1_val;
                                    obj.Subject2 = Subject_2_val;
                                    obj.Subject3 = Subject_3_val;
                                    obj.Subject4 = Subject_4_val;
                                    obj.Subject5 = Subject_5_val;
                                    obj.Subject6 = Subject_6_val;
                                    obj.Subject7 = Subject_7_val;
                                    obj.Subject8 = Subject_8_val;
                                    obj.Subject9 = Subject_9_val;
                                    obj.Subject10 = Subject_10_val;
                                    obj.ConvertedMarkPercentage = decimal.Parse(txtConvertMark.Text.Trim());
                                    obj.ConvertedTotalMark = convertedTotalMark;
                                    obj.TotalMark = decimal.Parse(txtTotalMark.Text.Trim());
                                    obj.StudentMark = studentMark_val;
                                    obj.ModifiedBy = 1;


                                    try
                                    {
                                        using (var db = new GeneralDataManager())
                                        {
                                            db.Update<DAL.AdmissionExamMark>(obj);

                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                }


                            }
                            else // Student Already Exists.
                            {

                                stdObj.Roll = Roll;

                                stdObj.Reason = "Student Roll Number Not Exists in Admission Test Roll";

                                notUploadedstudentList.Add(stdObj);
                            }
                        }



                        #endregion




                        Message = "Total Students : " + Total;

                        if (NewInserted > 0)
                            Message = Message + ". New Inserted Students : " + NewInserted;

                        if (notUploadedstudentList != null && notUploadedstudentList.Count > 0)
                        {
                            Message = Message + ". Not Uploaded Student's Mark  : " + notUploadedstudentList.Count;

                            lblNotMigratedStudent.Text = "Not Uploaded Student's Mark : " + notUploadedstudentList.Count;

                            // lblMsg.Text = "Migrated Student: "+





                            GVNotUploadedStudentList.DataSource = notUploadedstudentList;
                            GVNotUploadedStudentList.DataBind();





                            Session["NotMigratedStudentList"] = notUploadedstudentList;



                            DivNotUploadedStudent.Visible = true;
                            GvStudent.Visible = false;


                            //if (DivNotUploadedStudent.Visible == true)
                            //   // lnkDownloadExcel.Visible = true;
                            //else
                            //   // lnkDownloadExcel.Visible = false;
                        }

                        //if (notUploadedstudentList == null || notUploadedstudentList.Count == 0)
                        //{
                        //    lnkViewInformation_Click(null, null);
                        //}
                        //lnkDownloadExcel.Visible = false;
                        lblMessage.Text = Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        #endregion

                        Session["DataTableExcelUpload"] = null;

                    }
                    else
                    {
                        lblMessage.Text = "No Data Found for Upload";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        Session["DataTableExcelUpload"] = null;
                    }

                }
                else
                {
                    lblMessage.Text = "Please Select Session,Faculty,Converted To and Exam Taken  Before Upload";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    Session["DataTableExcelUpload"] = null;
                }

            }
            catch (Exception ex)
            {

            }
        }

        public class StudentNotUpload
        {

            public string Roll { get; set; }

            public string Reason { get; set; }
        }

        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        protected void lnkDownloadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<StudentNotUpload> StudentList = (List<StudentNotUpload>)Session["NotMigratedStudentList"];


                if (StudentList != null && StudentList.Count > 0)
                {
                    DataTable stdlist = ListToDataTable(StudentList);

                    if (stdlist != null && stdlist.Rows.Count > 0)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {

                            IXLWorksheet sheet2;
                            sheet2 = wb.AddWorksheet(stdlist, "Sheet");

                            sheet2.Table("Table1").ShowAutoFilter = false;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=" + "NotUploadedStudentList.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);

                                //HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                                //cookie.Value = "Flag";
                                //cookie.Expires = DateTime.Now.AddDays(1);
                                //Response.AppendCookie(cookie);

                                Response.Flush();
                                Response.SuppressContent = true;
                                // Response.Close();
                                Response.End();
                            }
                        }
                        ;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnExcelUpload_Click(object sender, EventArgs e)
        {
            try
            {
                GvStudent.Visible = false;

                ClearExcelGrid();
                //  GVNotUploadedStudentList.Visible = false;
                // GVTotalStudentList.Visible = false;
                //lblTotalStudent.Visible = true;
                //GVTotalStudentList.Visible = true;
                //GvStudent.Visible = false;

                Session["DataTableExcelUpload"] = null;
                Session["NotMigratedStudentList"] = null;
                lblMessage.Text = "";
                if (ExcelUpload.HasFile)
                {
                    string saveFolder = "~/Upload/SampleExcel/";
                    string filename = ExcelUpload.FileName;
                    string filePath = Path.Combine(saveFolder, ExcelUpload.FileName);
                    string excelpath = Server.MapPath(filePath);

                    if (File.Exists(excelpath))
                    {
                        System.IO.File.Delete(excelpath);
                        ExcelUpload.SaveAs(excelpath);
                    }
                    else
                    {
                        ExcelUpload.SaveAs(excelpath);
                    }

                    try
                    {
                        System.Data.OleDb.OleDbConnection MyConnection;
                        System.Data.DataTable DtTable;
                        System.Data.OleDb.OleDbDataAdapter MyCommand; ;
                        MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelpath + ";Extended Properties=Excel 12.0 xml;");
                        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                        MyCommand.TableMappings.Add("Table", "TestTable");
                        DtTable = new System.Data.DataTable();
                        MyCommand.Fill(DtTable);
                        //PopulateData(DtTable, excelpath);
                        Session["DataTableExcelUpload"] = DtTable;

                        MyConnection.Close();
                        if (DtTable.Rows.Count > 0)
                        {

                            //foreach (var column in DtTable.Columns.Cast<DataColumn>().ToArray())
                            //{
                            //    if (DtTable.AsEnumerable().All(dr => dr.IsNull(column)))
                            //        DtTable.Columns.Remove(column);
                            //}

                            for (int i = DtTable.Rows.Count - 1; i >= 0; i--)
                            {
                                if (DtTable.Rows[i][0].ToString() == String.Empty)
                                {
                                    DtTable.Rows.RemoveAt(i);
                                }
                            }
                            if (DtTable.Columns.Count == 12)
                            {
                                DataColumnCollection columns = DtTable.Columns;


                                GVTotalStudentList.DataSource = DtTable;
                                GVTotalStudentList.DataBind();

                                DivTotalStudent.Visible = true;
                                lblTotalStudent.Text = "Total Students List : " + DtTable.Rows.Count;
                            }
                            else
                            {
                                lblMessage.Text = "Please Upload Excel With Proper Format";
                                lblMessage.ForeColor = System.Drawing.Color.Red;

                            }

                            GVTotalStudentList.Visible = true;
                            lnkStudentMigrateButton.Visible = true;
                            System.IO.File.Delete(excelpath);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Please select file with proper format or Change the excel sheet name with Sheet1";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        System.IO.File.Delete(excelpath);
                    }
                }

                else
                {
                    lblMessage.Text = "Please Select an Excel File with Proper Format";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }

            }
            catch (Exception ex)
            {

            }

        }

        private void ClearExcelGrid()
        {
            try
            {
                // lblMsg.Text = string.Empty;
                GVTotalStudentList.DataSource = null;
                GVTotalStudentList.DataBind();

                //GvStudent.Visible = false;

                GVNotUploadedStudentList.DataSource = null;
                GVNotUploadedStudentList.DataBind();

                DivTotalStudent.Visible = false;
                DivNotUploadedStudent.Visible = false;

                // lnkStudentMigrateButton.Visible = false;


                // lnkDownloadExcel.Visible = false;

                lblTotalStudent.Text = string.Empty;
                lblNotMigratedStudent.Text = string.Empty;


            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            GvStudent.Visible = false;
            GVNotUploadedStudentList.Visible = false;
            GVTotalStudentList.Visible = false;
        }


        #region Direct Excel Upload New Methods


        protected void lnkStudentMigrateButtonDirect_Click(object sender, EventArgs e)
        {
            // Clear previous messages
            lblMessage.Text = "";
            messagePanel.Visible = false;

            // --- 1. Validation & Input Fetch ---
            if (!ExcelUpload.HasFile || string.IsNullOrEmpty(ddlSession.SelectedValue) ||
                string.IsNullOrEmpty(ddlFaculty.SelectedValue) || string.IsNullOrEmpty(txtTotalMark.Text) ||
                string.IsNullOrEmpty(txtConvertMark.Text))
            {
                showAlert("Please select a file, Session, Faculty, Converted To, and Exam Taken before upload.");
                
                return;
            }
            decimal convertedTo = decimal.Parse(txtConvertMark.Text.Trim());
            decimal ExamTaken = decimal.Parse(txtTotalMark.Text.Trim());



            int sessionID = int.Parse(ddlSession.SelectedValue);
            int unitID = int.Parse(ddlFaculty.SelectedValue);

            if (!(ExcelUpload.FileName.ToUpper().EndsWith("XLS") || ExcelUpload.FileName.ToUpper().EndsWith("XLSX")))
            {
                showAlert("Wrong file format. Please upload an Excel file (.xls or .xlsx).");
                
                return;
            }

            // --- 2. File Handling ---
            string fileName = Path.GetFileName(ExcelUpload.FileName);
            string uploadFileLocation = Server.MapPath("~/Upload/TEMP/");
            string savePath = Path.Combine(uploadFileLocation, fileName);

            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(uploadFileLocation);

                // Save the file
                ExcelUpload.SaveAs(savePath);

                // --- 3. Read Excel Data ---
                // Assuming aFileConverterObj is available and works to read the file into a DataTable
                SheetName sheet = aFileConverterObj.PassFileName(savePath).FirstOrDefault();
                DataTable dt = aFileConverterObj.ReadExcelFileDOM(savePath, fileName, sheet.Id);

                if (dt == null || dt.Rows.Count <= 1) // Check for header row and at least one data row
                {
                    showAlert("The uploaded file is empty or contains only a header.");
                    return;
                }

                // Remove the header row (assuming it's the first row, index 0)
                dt.Rows.RemoveAt(0);

                // --- 4. Bulk Processing (The Key Optimization!) ---
                int insertCount = BulkInsertMarks(dt, sessionID, unitID, convertedTo, ExamTaken);

                showAlert($"Upload Complete. Total Records Processed: {dt.Rows.Count}. Total Inserted: {insertCount}");
                GvStudent.Visible = false;

                // Clean up the uploaded file (Optional, but recommended)
                File.Delete(savePath);
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error during file processing/database operation: {ex.Message}";
                if (ex.InnerException != null)
                {
                    lblMessage.Text += $"; Inner Exception: {ex.InnerException.Message}";
                }
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }

        // This method should be implemented within your Data Access Layer (e.g., GeneralDataManager)
        public int BulkInsertMarks(DataTable dt, int sessionID, int unitID, decimal convertedTo, decimal examTaken)
        {
            // The list to hold the structured objects for insertion
            List<TempAdmissionExamMark> marksToInsert = new List<TempAdmissionExamMark>();

            // --- Data Transformation (Fast, in-memory processing) ---
            foreach (DataRow dtRow in dt.Rows)
            {
                // Safely extract and convert data from the DataTable row
                // Using Trim() and null/empty checks for string values
                string roll = dtRow[0]?.ToString().Trim();
                string studentMarkStr = dtRow[11]?.ToString().Trim();

                #region Data Validation

                // Define variables outside the loop/checks
                decimal subject1Val = 0;
                decimal subject2Val = 0;
                decimal subject3Val = 0;
                decimal subject4Val = 0;
                decimal subject5Val = 0;
                decimal subject6Val = 0;
                decimal subject7Val = 0;
                decimal subject8Val = 0;
                decimal subject9Val = 0;
                decimal subject10Val = 0;

                // --- Modified Parsing Logic using decimal.Parse ---

                // Subject 1
                string subject1Str = dtRow[1]?.ToString();
                if (!string.IsNullOrEmpty(subject1Str))
                {
                    // The Trim() is important to remove whitespace
                    subject1Val = decimal.Parse(subject1Str.Trim());
                }

                // Subject 2
                string subject2Str = dtRow[2]?.ToString();
                if (!string.IsNullOrEmpty(subject2Str))
                {
                    subject2Val = decimal.Parse(subject2Str.Trim());
                }

                // Subject 3
                string subject3Str = dtRow[3]?.ToString();
                if (!string.IsNullOrEmpty(subject3Str))
                {
                    subject3Val = decimal.Parse(subject3Str.Trim());
                }

                // Subject 4
                string subject4Str = dtRow[4]?.ToString();
                if (!string.IsNullOrEmpty(subject4Str))
                {
                    subject4Val = decimal.Parse(subject4Str.Trim());
                }

                // Subject 5
                string subject5Str = dtRow[5]?.ToString();
                if (!string.IsNullOrEmpty(subject5Str))
                {
                    subject5Val = decimal.Parse(subject5Str.Trim());
                }

                // Subject 6
                string subject6Str = dtRow[6]?.ToString();
                if (!string.IsNullOrEmpty(subject6Str))
                {
                    subject6Val = decimal.Parse(subject6Str.Trim());
                }
                // Subject 7
                string subject7Str = dtRow[7]?.ToString();
                if (!string.IsNullOrEmpty(subject7Str))
                {
                    subject7Val = decimal.Parse(subject7Str.Trim());
                }
                // Subject 8
                string subject8Str = dtRow[8]?.ToString();
                if (!string.IsNullOrEmpty(subject8Str))
                {
                    subject8Val = decimal.Parse(subject8Str.Trim());
                }
                // Subject 9
                string subject9Str = dtRow[9]?.ToString();
                if (!string.IsNullOrEmpty(subject9Str))
                {
                    subject9Val = decimal.Parse(subject9Str.Trim());
                }
                // Subject 10
                string subject10Str = dtRow[10]?.ToString();
                if (!string.IsNullOrEmpty(subject10Str))
                {
                    subject10Val = decimal.Parse(subject10Str.Trim());
                }

                #endregion
                // --- End of Modified Parsing Logic ---

                // Handle extraction of the final number from the studentMark string
                decimal studentMarkVal = 0;
                if (!string.IsNullOrEmpty(studentMarkStr))
                {
                    // New code logic for extracting the mark from the string (kept from original)
                    string[] parts = studentMarkStr.Split(new[] { ' ', '(', ')', '+' }, StringSplitOptions.RemoveEmptyEntries);
                    string finalMarkPart = parts.LastOrDefault();
                    decimal.TryParse(finalMarkPart, out studentMarkVal);
                }

                // Only process rows that have a valid Roll and a valid mark greater than 0
                if (string.IsNullOrEmpty(roll) || studentMarkVal <= 0)
                {
                    continue; // Skip invalid or empty rows
                }

                // Calculate the converted total mark
                decimal convertedTotalMark = (studentMarkVal * convertedTo) / examTaken;
                // Create the object
                TempAdmissionExamMark newObj = new TempAdmissionExamMark
                {
                    AdmissionUnitID = unitID,
                    SessionID = sessionID,
                    StudentTestRoll = roll,
                    Subject1 = subject1Val,
                    Subject2 = subject2Val,
                    Subject3 = subject3Val,
                    Subject4 = subject4Val,
                    Subject5 = subject5Val,
                    Subject6 = subject6Val,
                    Subject7 = subject7Val,
                    Subject8 = subject8Val,
                    Subject9 = subject9Val,
                    Subject10 = subject10Val,
                    ConvertedMarkPercentage = convertedTo,
                    TotalMark = examTaken,
                    StudentMark = studentMarkVal,
                    ConvertedTotalMark = convertedTotalMark,
                    CreatedBy = 1, // Or get the actual user ID
                    CreatedDate = DateTime.Now
                };

                marksToInsert.Add(newObj);
            }
            // --- End Data Transformation ---

            if (!marksToInsert.Any())
            {
                return 0;
            }

            // --- Bulk Database Operations (The Fastest Part) ---
            using (var db = new GeneralDataManager())
            {
                int insertCount = 0;

                DataTable marksToInsertDataTable = ListToDataTableManager.ToDataTable(marksToInsert);

                try
                {
                    // The C# code that calls the stored procedure:

                    List<SqlParameter> parameters = new List<SqlParameter>();

                    // Add parameters for DELETE logic
                    parameters.Add(new SqlParameter { ParameterName = "@SessionId", SqlDbType = System.Data.SqlDbType.Int, Value = sessionID });
                    parameters.Add(new SqlParameter { ParameterName = "@AdmissionUnitId", SqlDbType = System.Data.SqlDbType.Int, Value = unitID });

                    // Add the TVP
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "@CandidateMarksList",
                        SqlDbType = System.Data.SqlDbType.Structured,
                        Value = marksToInsertDataTable, // IMPORTANT: Ensure 'marksToInsertDataTable' is converted to a DataTable if your GetDataFromQuery expects it.
                        TypeName = "dbo.AdmissionMarkTableType"
                    });

                    // Assuming your GetDataFromQuery handles the conversion of marksToInsert internally, 
                    // OR that marksToInsert is already a DataTable ready for the TVP.
                    // If marksToInsert is a List<AdmissionExamMark>, you MUST ensure your DataTableManager handles the List<T> to DataTable conversion before creating the SqlParameter.
                    DataTable dtUpdatedList = DataTableManager.GetDataFromQuery("InsertExcelUploadedMarks", parameters);

                    if (dtUpdatedList != null && dtUpdatedList.Rows.Count > 0)
                    {
                        // Read the result from the stored procedure output:
                        // This assumes the stored procedure returns a DataTable with one column named "InsertedCount"
                        insertCount = Convert.ToInt32(dtUpdatedList.Rows[0]["InsertedCount"]);
                    }

                    // Update your UI message with insertedCount
                    // lblMessage.Text = "Upload Complete.Total Inserted: " + insertedCount;

                }
                catch (Exception ex)
                {
                }


                return insertCount;
            }
        }

        public class TempAdmissionExamMark
        {
            public string StudentTestRoll { get; set; }
            public Nullable<long> SessionID { get; set; }
            public Nullable<long> AdmissionUnitID { get; set; }
            public Nullable<decimal> Subject1 { get; set; }
            public Nullable<decimal> Subject2 { get; set; }
            public Nullable<decimal> Subject3 { get; set; }
            public Nullable<decimal> Subject4 { get; set; }
            public Nullable<decimal> Subject5 { get; set; }
            public Nullable<decimal> Subject6 { get; set; }
            public Nullable<decimal> Subject7 { get; set; }
            public Nullable<decimal> Subject8 { get; set; }
            public Nullable<decimal> Subject9 { get; set; }
            public Nullable<decimal> Subject10 { get; set; }

            public Nullable<decimal> ConvertedTotalMark { get; set; }
            public Nullable<decimal> ConvertedMarkPercentage { get; set; }
            public Nullable<decimal> StudentMark { get; set; }
            public Nullable<decimal> TotalMark { get; set; }
            public Nullable<long> CreatedBy { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
        }

        #endregion

    }

}