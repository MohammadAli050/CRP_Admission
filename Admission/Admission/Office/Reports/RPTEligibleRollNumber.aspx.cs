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
    public partial class RPTEligibleRollNumber : PageBase
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

            }
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {

            // Session & lebel Reset
            lblMessage.Text = "";


            //#region Delete Information From Table
            ////using (var db = new OfficeDataManager())
            ////{
            ////    db.AdmissionDB.TruncateEligibleRollNumber();

            ////}

            //#endregion


            //Process Start
            try
            {
                if (this.fuExcel.HasFile)
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


                            foreach (DataRow dtRow in dt.Rows)
                            {
                                string testRoll = "";
                                testRoll = dtRow.ItemArray[0].ToString();

                                if (testRoll.Trim() != "")
                                {
                                    DAL.EligibleRollNumber ern = null;
                                    using (var db = new OfficeDataManager())
                                    {
                                        ern = db.AdmissionDB.EligibleRollNumbers.Where(x => x.EligibleRollNo.Trim() == testRoll.Trim()).FirstOrDefault();
                                    }

                                    if (ern == null)
                                    {
                                        DAL.EligibleRollNumber etrastModel = new DAL.EligibleRollNumber();
                                        etrastModel.EligibleRollNo = testRoll.Trim();
                                        etrastModel.IsEligible = true;
                                        etrastModel.CreatedBy = uId;
                                        etrastModel.DateCreated = DateTime.Now;

                                        try
                                        {

                                            using (var db2 = new OfficeDataManager())
                                            {
                                                db2.Insert<DAL.EligibleRollNumber>(etrastModel);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            lblMessage.Text = "Error-Insert: " + ex.Message + "; ";
                                            messagePanel.CssClass = "alert alert-danger";
                                            messagePanel.Visible = true;
                                            return;
                                        }
                                    }

                                        
                                }
                            }



                            lblMessage.Text = "Upload Complete";
                            messagePanel.CssClass = "alert alert-success";
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

            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error3: " + ex.Message + "; ";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            List<DAL.EligibleRollNumber> ernList = new List<DAL.EligibleRollNumber>();
            using (var db = new OfficeDataManager())
            {
                ernList = db.AdmissionDB.EligibleRollNumbers.ToList();
            }

            if (ernList.Count > 0)
            {
                lvEligibleRollNumber.DataSource = ernList;
                lblCount.Text = ernList.Count().ToString();
            }
            else
            {
                lvEligibleRollNumber.DataSource = null;
                lblCount.Text = "0";
            }
            lvEligibleRollNumber.DataBind();

        }


        protected void lvEligibleRollNumber_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.EligibleRollNumber ern = (DAL.EligibleRollNumber)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblEligibleRollNumber = (Label)currentItem.FindControl("lblEligibleRollNumber");
                Label lblIsEligible = (Label)currentItem.FindControl("lblIsEligible");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                lblEligibleRollNumber.Text = ern.EligibleRollNo;

                if (ern.IsEligible == true)
                {
                    lblIsEligible.Text = "YES";
                    lblIsEligible.ForeColor = Color.Green;
                    lblIsEligible.Font.Bold = true;
                }
                else
                {
                    lblIsEligible.Text = "NO";
                    lblIsEligible.ForeColor = Color.Crimson;
                }

            }
        }

    }
}