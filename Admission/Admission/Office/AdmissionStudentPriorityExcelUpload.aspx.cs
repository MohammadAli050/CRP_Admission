using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DAL;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class AdmissionStudentPriorityExcelUpload : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        int noOfPrograms = -1;
        string userName = "";


        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            using (var db = new CandidateDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);

                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                }
            }

            if (!IsPostBack)
            {
                if (uId > 0)
                {
                    LoadDDL();
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        protected void LoadDDL()
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

                //GvStudent.Visible = false;
                //GVNotUploadedStudentList.Visible = false;
                //GVTotalStudentList.Visible = false;

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
                            var admUnit = db.GetAllAdmissionUnit().Where(x => x.ID == item.AdmissionUnitID && x.IsActive == true).FirstOrDefault();
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

        protected void lnkExcelUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Session["DataTableExcelUpload"] = null;

                int AcaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                int FacultyId = Convert.ToInt32(ddlFaculty.SelectedValue);

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
                        System.Data.OleDb.OleDbDataAdapter MyCommand;
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

                            for (int i = DtTable.Rows.Count - 1; i >= 0; i--)
                            {
                                if (DtTable.Rows[i][0].ToString() == String.Empty)
                                {
                                    DtTable.Rows.RemoveAt(i);
                                }
                            }
                            using (var db = new CandidateDataManager())
                            {

                                foreach (DataRow row in DtTable.Rows)
                                {
                                    try
                                    {
                                        string testRoll = row["TestRoll"].ToString();
                                        int status = Convert.ToInt32(row["Status"]); // Call your method with the extracted parameters 

                                        var res = db.AdmissionDB.UpdateCandidatePriorityFromExcel(AcaCalId, FacultyId, testRoll, status);

                                        if (res != null)
                                        {
                                            try
                                            {
                                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.EventName = "btnExcelUpload";
                                                dLog.PageName = "AdmissionSetup.aspx";
                                                dLog.OldData = "Candidate " + testRoll + "With Existing priority";
                                                dLog.NewData = "Candidate with changed Priority with Status " + status;
                                                dLog.UserId = uId;
                                                dLog.SessionInformation = "SU-ID: " + SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; UserRole: " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);
                                                dLog.DateCreated = DateTime.Now;
                                                dLog.IpAddress = null;
                                                dLog.DateTime = DateTime.Now;
                                                dLog.HostName = Request.UserHostAddress + Request.UserHostName;
                                                LogWriter.DataLogWriter(dLog);
                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                showAlert("Data uploaded successfully");
                                GvStudent.DataSource = DtTable;
                                GvStudent.DataBind();
                            }

                            System.IO.File.Delete(excelpath);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.Delete(excelpath);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}