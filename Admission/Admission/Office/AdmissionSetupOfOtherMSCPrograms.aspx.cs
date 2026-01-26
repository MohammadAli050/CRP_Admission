using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class AdmissionSetupOfOtherMSCPrograms : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Page.Form.Enctype = "multipart/form-data";

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
                LoadDDL();
                LoadData();
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPProgramsGetAllFromUCAM_Result>(ddlProgram, db.AdmissionDB.SPProgramsGetAllFromUCAM().Where(a => a.ProgramTypeID != 1).ToList(), "DetailNShortName", "ProgramID", EnumCollection.ListItemType.Program);
            }




        }

        private void ClearFields()
        {
            ddlProgram.SelectedValue = "-1";
            ddlFileType.SelectedValue = "-1";
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int programId = Convert.ToInt32(ddlProgram.SelectedValue);
                //string programNameString = ddlProgram.SelectedItem.Text.ToString();
                //programNameString = programNameString.Replace(" ", "-");

                int fileTypeId = Convert.ToInt32(ddlFileType.SelectedValue);
                string fileTypeNameString = ddlFileType.SelectedItem.Text.ToString();
                fileTypeNameString = fileTypeNameString.Replace(" ", "-");


                if (programId > 0 && fileTypeId > 0)
                {
                    DAL.SPProgramsGetByIdFromUCAM_Result ucamProgram = null;
                    using (var db = new OfficeDataManager())
                    {
                        ucamProgram = db.AdmissionDB.SPProgramsGetByIdFromUCAM(programId).FirstOrDefault();
                    }

                    if (FileUploadDocument.HasFile && ucamProgram != null)
                    {

                        string programNameString = ucamProgram.DetailName;
                        programNameString = programNameString.Replace(" ", "-");
                        programNameString = programNameString.Replace("/", "-");


                        String fileExtension = Path.GetExtension(FileUploadDocument.PostedFile.FileName).ToLower();

                        if (fileExtension.ToLower() == ".pdf")
                        {
                            int contentlength = int.Parse(FileUploadDocument.PostedFile.ContentLength.ToString());
                            string fileName = programNameString + "-" + fileTypeNameString + fileExtension;
                            string filePath = "~/ApplicationDocs/";

                            if (contentlength < 20480000)  //20480000 = kilobites
                            {
                                try
                                {
                                    int flagIsFileSaved = 0;


                                    #region Save File
                                    if (File.Exists(Server.MapPath(filePath + fileName)))
                                    {
                                        try
                                        {
                                            ////move the file to TEMP
                                            //File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));

                                            //delete the original file
                                            File.Delete(Server.MapPath(filePath + fileName));

                                            FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                            flagIsFileSaved = 1;

                                        }
                                        catch (Exception ex)
                                        {
                                            flagIsFileSaved = 0;
                                        }

                                    }//end if
                                    else
                                    {
                                        try
                                        {
                                            FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                            flagIsFileSaved = 1;
                                        }
                                        catch (Exception ex)
                                        {
                                            flagIsFileSaved = 0;
                                        }

                                    }//end if-else 
                                    #endregion


                                    if (flagIsFileSaved == 1)
                                    {

                                        DAL.AdmissionSetupOtherProgram documentObj = null;
                                        using (var db = new CandidateDataManager())
                                        {
                                            documentObj = db.AdmissionDB.AdmissionSetupOtherPrograms.Where(x => x.ProgramTypeId == 1
                                                                                                            && x.ProgramId == programId
                                                                                                             && x.FileTypeId == fileTypeId).FirstOrDefault();
                                        }

                                        if (documentObj != null) //do not update document, document exists, only update document details
                                        {
                                            #region Update
                                            documentObj.FileName = fileName;
                                            documentObj.FileURL = filePath + fileName;
                                            documentObj.ModifiedBy = uId;
                                            documentObj.ModifiedDate = DateTime.Now;

                                            using (var db = new OfficeDataManager())
                                            {
                                                db.Update<DAL.AdmissionSetupOtherProgram>(documentObj);
                                            }
                                            #endregion


                                            ClearFields();
                                            LoadData();
                                            MessageView("File Updated Successfully", "success");

                                        }
                                        else
                                        {
                                            #region Insert
                                            DAL.AdmissionSetupOtherProgram newDocumentDetailObj = new DAL.AdmissionSetupOtherProgram();

                                            newDocumentDetailObj.ProgramTypeId = 1;
                                            newDocumentDetailObj.FileTypeId = fileTypeId;
                                            newDocumentDetailObj.FileName = fileName;
                                            newDocumentDetailObj.FileURL = filePath + fileName;
                                            newDocumentDetailObj.ProgramId = ucamProgram.ProgramID;
                                            newDocumentDetailObj.ProgramShortName = ucamProgram.ShortName;
                                            newDocumentDetailObj.ProgramDetailsName = ucamProgram.DetailName;

                                            newDocumentDetailObj.CreatedBy = uId;
                                            newDocumentDetailObj.CreatedDate = DateTime.Now;

                                            long newDocumentDetailID = -1;
                                            using (var dbInsertDocumentDetail = new OfficeDataManager())
                                            {
                                                dbInsertDocumentDetail.Insert<DAL.AdmissionSetupOtherProgram>(newDocumentDetailObj);
                                                newDocumentDetailID = newDocumentDetailObj.ID;
                                            }
                                            #endregion

                                            ClearFields();
                                            LoadData();
                                            MessageView("File Uploaded Successfully", "success");

                                        }//end if-else
                                    }
                                    else
                                    {
                                        MessageView("Failed to Upload File!", "fail");
                                    }



                                }
                                catch (Exception ex)
                                {
                                    //lblMessage.Text = "Unable to upload photo.";
                                    //lblMessage.ForeColor = Color.Crimson;

                                    MessageView("Exception: Failed to Upload File !! Error: " + ex.Message.ToString(), "fail");
                                }
                            }
                            else
                            {
                                MessageView("File size is to larger. !!", "fail");
                            }
                        }
                        else
                        {
                            MessageView("Only PDF File Is Allowed!", "fail");
                        }

                    }// end if (FileUploadBanner.HasFile)
                    else
                    {
                        MessageView("No File is Selected !!", "fail");
                    }
                }
                else
                {
                    MessageView("Please select DocumentType & FileNo !!", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }
        }

        private void LoadData()
        {
            MessageView("", "clear");

            try
            {
                List<DAL.SPGetAllAdmissionSetupOtherPrograms_Result> list = null;
                using (var db = new OfficeDataManager())
                {
                    list = db.AdmissionDB.SPGetAllAdmissionSetupOtherPrograms(1, null, null).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    lvProgramPriority.DataSource = list.OrderBy(x => x.ProgramId).ToList();
                    lvProgramPriority.DataBind();
                }
                else
                {
                    lvProgramPriority.DataSource = null;
                    lvProgramPriority.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }

        }

        protected void lvProgramPriority_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetAllAdmissionSetupOtherPrograms_Result progP = (DAL.SPGetAllAdmissionSetupOtherPrograms_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblFileTypeName = (Label)currentItem.FindControl("lblFileTypeName");

                LinkButton lbDelete = (LinkButton)currentItem.FindControl("lbDelete");
                HyperLink hlBtn = (HyperLink)currentItem.FindControl("hlBtn");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblFileTypeName.Text = progP.FileTypeName;

                if (progP.FileTypeId == 1)
                {
                    hlBtn.Text = "View";
                    hlBtn.NavigateUrl = progP.FileURL;
                    hlBtn.Target = "_blank";
                    hlBtn.CssClass = "btn btn-info";
                    hlBtn.Attributes.CssStyle.Add("width", "125px");

                    hlBtn.Visible = true;
                }
                else if (progP.FileTypeId == 2)
                {
                    hlBtn.Text = "Form";
                    hlBtn.NavigateUrl = progP.FileURL;
                    hlBtn.Target = "_blank";
                    hlBtn.CssClass = "btn btn-success";
                    hlBtn.Attributes.CssStyle.Add("width", "125px");

                    hlBtn.Visible = true;
                }
                else
                {
                    hlBtn.Visible = false;
                }

                lbDelete.CommandName = "Delete";
                lbDelete.CommandArgument = progP.ID.ToString();
            }
        }

        protected void lvProgramPriority_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            MessageView("", "clear");

            if (e.CommandName == "Delete")
            {
                try
                {
                    long id = Convert.ToInt64(e.CommandArgument);

                    using (var db = new OfficeDataManager())
                    {
                        var objectToRemove = db.AdmissionDB.AdmissionSetupOtherPrograms.Where(x => x.ID == id).FirstOrDefault();

                        if (objectToRemove != null)
                        {
                            try
                            {
                                if (File.Exists(Server.MapPath(objectToRemove.FileURL)))
                                {
                                    try
                                    {
                                        //delete the original file
                                        File.Delete(Server.MapPath(objectToRemove.FileURL));

                                    }
                                    catch (Exception ex)
                                    {
                                        
                                    }

                                }//end if
                            }
                            catch (Exception ex)
                            {
                                
                            }


                            db.Delete<DAL.AdmissionSetupOtherProgram>(objectToRemove);
                            LoadData();

                            MessageView("Data Deleted Successfully.", "success");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
                }
            }
        }

        protected void lvProgramPriority_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        { }

        protected void lvProgramPriority_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        { }

        string lastValue = "";
        protected string AddGroupingHeader()
        {

            string currentValue = "";

            //Get the data field value of interest for this row
            //string programShortName = Eval("ProgramShortName").ToString();
            string programDetailsName = Eval("ProgramDetailsName").ToString();
            currentValue = programDetailsName; // + " (" + programShortName + ")";

            //Specify name to display if dataFieldValue is a database NULL
            if (currentValue.Length == 0)
            {
                currentValue = "";
            }

            string sNewRow = "";
            //See if there's been a change in value
            if (lastValue != currentValue)
            {
                //There's been a change! Record the change and emit the header
                lastValue = currentValue;
                sNewRow = "<tr style='background-color: gainsboro;'>" +
                            "<td colspan='4'><h4>" + currentValue + "</h4></td>" +
                          "</tr>";
                return sNewRow;
            }
            else
            {
                return "";
            }
        }
    }
}