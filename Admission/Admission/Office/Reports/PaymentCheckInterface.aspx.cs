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
    public partial class PaymentCheckInterface : PageBase
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

            #region Validation & Required input value get and check 
            long admUnitId = -1;
            int acaCalId = -1;

           

            int examMarks = -1;

            //if (!string.IsNullOrEmpty(txtTotalMarks.Text))
            //{
            //    examMarks = Convert.ToInt32(txtTotalMarks.Text);
            //}
            //else
            //{
            //    lblMessage.Text = "Total Marks required";
            //    lblMessage.ForeColor = Color.Crimson;
            //    return;
            //}

            //if (admUnitId < 0 || acaCalId < 0)
            //{
            //    lblMessage.Text = "Select Session and Faculty";
            //    messagePanel.CssClass = "alert alert-danger";
            //    messagePanel.Visible = true;
            //    return;
            //}
            #endregion

            // Session & lebel Reset
            lblMessage.Text = "";
          

            #region Delete Information From Table
            //using (var db = new OfficeDataManager())
            //{
            //    db.AdmissionDB.TruncateExcelTestRollAndScoreTemp();
            //}
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

                            DataRow row = dt.Rows[0];
                            dt.Rows.Remove(row);


                            List<DAL.CandidatePayment> paymentstudentList = new List<DAL.CandidatePayment>();

                            foreach (DataRow dtRow in dt.Rows)
                            {
                                long testRoll = 0;
                              



                                testRoll = Convert.ToInt64(dtRow.ItemArray[0].ToString().Trim());
                                // score = dtRow.ItemArray[1].ToString().Trim();

                             
                                if (testRoll > 0)
                                {
                                   
                                    using (var db = new OfficeDataManager())
                                    {
                                      var candentpayment =   db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId ==  testRoll && (x.IsPaid == false || x.IsPaid == null ) ) .FirstOrDefault();
                                        if(candentpayment != null)
                                        {
                                            paymentstudentList.Add(candentpayment);
                                        }
                                       
                                    }


                                    if (paymentstudentList != null)
                                    {
                                        gvRegisteredCourse.DataSource = paymentstudentList.ToList();
                                        //lblCount.Text = list.Count().ToString();
                                    }
                                    else
                                    {
                                        gvRegisteredCourse.DataSource = null;
                                        //lblCount.Text = list.Count().ToString();
                                    }
                                    gvRegisteredCourse.DataBind();


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
                catch (Exception ex)
                {
                    lblMessage.Text = "Error3: " + ex.Message + "; " + ex.InnerException.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
        }

      

  





    }
}