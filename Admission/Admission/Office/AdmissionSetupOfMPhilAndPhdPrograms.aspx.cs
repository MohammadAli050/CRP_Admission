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
    public partial class AdmissionSetupOfMPhilAndPhdPrograms : PageBase
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



        private void ClearFields()
        {
            txtTitle.Text = "";
            ddlFileType.SelectedValue = "-1";
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {

                string title = txtTitle.Text.Trim();


                int fileTypeId = Convert.ToInt32(ddlFileType.SelectedValue);
                string fileTypeNameString = ddlFileType.SelectedItem.Text.ToString();
                fileTypeNameString = fileTypeNameString.Replace(" ", "-");


                if (!string.IsNullOrEmpty(title) && fileTypeId > 0)
                {

                    if (FileUploadDocument.HasFile)
                    {
                        String fileExtension = Path.GetExtension(FileUploadDocument.PostedFile.FileName).ToLower();
                        string uploadFileName = title.Replace(" ", "_");
                        if (fileExtension.ToLower() == ".pdf" || fileExtension.ToLower() == ".docx") 
                        {
                            string timestamp = DateTime.Now.ToString("ddMMyyyyhhmmss");
                            int contentlength = int.Parse(FileUploadDocument.PostedFile.ContentLength.ToString());
                            string fileName = "MPhilAndPhDProgram-"+ uploadFileName+ "-" + fileTypeNameString + "-" + timestamp + fileExtension;
                            string filePath = "~/ApplicationDocs/";

                            if (contentlength < 20480000)  //20480000 = kilobites
                            {
                                try
                                {
                                    FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                    //int flagIsFileSaved = 0;


                                    //#region Save File
                                    //if (File.Exists(Server.MapPath(filePath + fileName)))
                                    //{
                                    //    try
                                    //    {
                                    //        ////move the file to TEMP
                                    //        //File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));

                                    //        //delete the original file
                                    //        File.Delete(Server.MapPath(filePath + fileName));

                                    //        FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                    //        flagIsFileSaved = 1;

                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        flagIsFileSaved = 0;
                                    //    }

                                    //}//end if
                                    //else
                                    //{
                                    //    try
                                    //    {
                                    //        FileUploadDocument.SaveAs(Server.MapPath(filePath + fileName));

                                    //        flagIsFileSaved = 1;
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        flagIsFileSaved = 0;
                                    //    }

                                    //}//end if-else 
                                    //#endregion


                                    //if (flagIsFileSaved == 1)
                                    //{

                                    //DAL.AdmissionSetupOtherProgram documentObj = null;
                                    //using (var db = new CandidateDataManager())
                                    //{
                                    //    documentObj = db.AdmissionDB.AdmissionSetupOtherPrograms.Where(x => x.ProgramTypeId == 2
                                    //                                                                     && x.FileTypeId == fileTypeId).FirstOrDefault();
                                    //}

                                    //if (documentObj != null) //do not update document, document exists, only update document details
                                    //{
                                    //    #region Update
                                    //    documentObj.ProgramDetailsName = title;
                                    //    documentObj.FileName = fileName;
                                    //    documentObj.FileURL = filePath + fileName;
                                    //    documentObj.ModifiedBy = uId;
                                    //    documentObj.ModifiedDate = DateTime.Now;

                                    //    using (var db = new OfficeDataManager())
                                    //    {
                                    //        db.Update<DAL.AdmissionSetupOtherProgram>(documentObj);
                                    //    }
                                    //    #endregion


                                    //    ClearFields();
                                    //    LoadData();
                                    //    MessageView("File Updated Successfully", "success");

                                    //}
                                    //else
                                    //{
                                    #region Insert
                                    DAL.AdmissionSetupOtherProgram newDocumentDetailObj = new DAL.AdmissionSetupOtherProgram();

                                        newDocumentDetailObj.ProgramTypeId = 2;
                                        newDocumentDetailObj.FileTypeId = fileTypeId;
                                        newDocumentDetailObj.FileName = fileName;
                                        newDocumentDetailObj.FileURL = filePath + fileName;
                                        //newDocumentDetailObj.ProgramId = ucamProgram.ProgramID;
                                        //newDocumentDetailObj.ProgramShortName = ucamProgram.ShortName;
                                        newDocumentDetailObj.ProgramDetailsName = title;

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

                                        //}//end if-else
                                    //}
                                    //else
                                    //{
                                    //    MessageView("Failed to Upload File!", "fail");
                                    //}



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
                    MessageView("Please provide all required fields!", "fail");
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
                    list = db.AdmissionDB.SPGetAllAdmissionSetupOtherPrograms(2, null, null).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    lvAdmSetup.DataSource = list.OrderBy(x => x.SerialNo).ToList();
                    lvAdmSetup.DataBind();
                }
                else
                {
                    lvAdmSetup.DataSource = null;
                    lvAdmSetup.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageView("Exception: Something went wrong. Error: " + ex.Message.ToString(), "fail");
            }

        }



        protected void lvAdmSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetAllAdmissionSetupOtherPrograms_Result admSetup = (DAL.SPGetAllAdmissionSetupOtherPrograms_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblId = (Label)currentItem.FindControl("lblId");
                Label lblTitle = (Label)currentItem.FindControl("lblTitle");
                Label lblFileType = (Label)currentItem.FindControl("lblFileType");
                DropDownList ddlSerialNo = (DropDownList)currentItem.FindControl("ddlSerialNo");


                HyperLink hlViewFile = (HyperLink)currentItem.FindControl("hlViewFile");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                lblId.Text = admSetup.ID.ToString();

                lblTitle.Text = admSetup.ProgramDetailsName;
                lblFileType.Text = admSetup.FileTypeName;

                if (admSetup.FileTypeId == 1 || admSetup.FileTypeId == 3)
                {
                    hlViewFile.Text = "View";
                    hlViewFile.NavigateUrl = admSetup.FileURL;
                    hlViewFile.Target = "_blank";
                    hlViewFile.CssClass = "btn btn-info";
                    hlViewFile.Attributes.CssStyle.Add("width", "125px");

                    hlViewFile.Visible = true;
                }
                else if (admSetup.FileTypeId == 2)
                {
                    hlViewFile.Text = "Form";
                    hlViewFile.NavigateUrl = admSetup.FileURL;
                    hlViewFile.Target = "_blank";
                    hlViewFile.CssClass = "btn btn-success";

                    hlViewFile.Visible = true;
                }
                else
                {
                    hlViewFile.Visible = false;
                }


                List<int> serialNoList = new List<int>();
                for (int i = 1; i <= 10; i++)
                {
                    serialNoList.Add(i);
                }
                ddlSerialNo.Items.Clear();
                ddlSerialNo.Items.Add(new ListItem("--Serial No--", "-1"));
                ddlSerialNo.AppendDataBoundItems = true;
                ddlSerialNo.DataSource = serialNoList.ToList();
                ddlSerialNo.DataBind();

                if (admSetup.SerialNo > 0)
                {
                    ddlSerialNo.SelectedValue = admSetup.SerialNo.ToString();
                }

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = admSetup.ID.ToString();
            }
        }

        protected void lvAdmSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
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

        protected void lvAdmSetup_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

        protected void lvAdmSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }

        protected void ddlSerialNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ////=== Style One to get that Row data
                ListViewDataItem item = (ListViewDataItem)(((DropDownList)sender)).NamingContainer;

                ////=== Style Two to get that Row data
                //DropDownList dropDownList = (DropDownList)sender;
                //ListViewItem item = (ListViewItem)dropDownList.NamingContainer;

                Label lblSerial = (Label)item.FindControl("lblSerial");
                DropDownList ddlSerialNo = (DropDownList)item.FindControl("ddlSerialNo");

                Label lblDDLMessage = (Label)item.FindControl("lblDDLMessage");

                lblDDLMessage.Text = "";

                int selectedRow = (!string.IsNullOrEmpty(lblSerial.Text) && Convert.ToInt32(lblSerial.Text) > 0) ? Convert.ToInt32(lblSerial.Text) : -1;
                int ddlSerialNoId = Convert.ToInt32(ddlSerialNo.SelectedValue);

                if (!string.IsNullOrEmpty(ddlSerialNo.SelectedValue) && Convert.ToInt32(ddlSerialNo.SelectedValue) > 0)
                {
                    foreach (ListViewDataItem dataItem in lvAdmSetup.Items)
                    {
                        Label lblSerialRow = (Label)dataItem.FindControl("lblSerial");
                        DropDownList ddlSerialNoRow = (DropDownList)dataItem.FindControl("ddlSerialNo");

                        int eachRow = (!string.IsNullOrEmpty(lblSerialRow.Text) && Convert.ToInt32(lblSerialRow.Text) > 0) ? Convert.ToInt32(lblSerialRow.Text) : -1;
                        int ddlSerialNoRowId = Convert.ToInt32(ddlSerialNoRow.SelectedValue);

                        if (eachRow != selectedRow)
                        {
                            if (ddlSerialNoId == ddlSerialNoRowId)
                            {
                                lblDDLMessage.Text = "Serial No is already exist";
                                lblDDLMessage.Attributes.CssStyle.Add("color", "crimson");
                                ddlSerialNo.SelectedValue = "-1";
                                return;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSaveSerialNo_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListViewDataItem dataItem in lvAdmSetup.Items)
                {
                    //Label lblSerialRow = (Label)dataItem.FindControl("lblSerial");
                    DropDownList ddlSerialNoRow = (DropDownList)dataItem.FindControl("ddlSerialNo");
                    Label lblDDLMessage = (Label)dataItem.FindControl("lblDDLMessage");

                    Label lblId = (Label)dataItem.FindControl("lblId");

                    lblDDLMessage.Text = "";

                    //int eachRow = (!string.IsNullOrEmpty(lblSerialRow.Text) && Convert.ToInt32(lblSerialRow.Text) > 0) ? Convert.ToInt32(lblSerialRow.Text) : -1;
                    int ddlSerialNoRowId = Convert.ToInt32(ddlSerialNoRow.SelectedValue);
                    int id = Convert.ToInt32(lblId.Text);


                    DAL.AdmissionSetupOtherProgram objExist = null;
                    using (var db = new OfficeDataManager())
                    {
                        objExist = db.AdmissionDB.AdmissionSetupOtherPrograms.Where(x => x.ID == id).FirstOrDefault();
                    }

                    if (objExist != null)
                    {
                        //Update Serial No
                        if (ddlSerialNoRowId > 0)
                        {
                            objExist.SerialNo = ddlSerialNoRowId;
                        }
                        else
                        {
                            objExist.SerialNo = null;
                        }
                        objExist.ModifiedBy = uId;
                        objExist.ModifiedDate = DateTime.Now;
                        using (var db = new OfficeDataManager())
                        {
                            db.Update<DAL.AdmissionSetupOtherProgram>(objExist);
                        }

                        count++;
                    }
                    else
                    {

                    }

                }

                if (count > 0)
                {
                    LoadData();
                    MessageView("Data Updated Successfully", "success");
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}