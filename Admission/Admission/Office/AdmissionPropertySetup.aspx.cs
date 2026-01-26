using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class AdmissionPropertySetup : PageBase
    {

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
                LoadDDL();

                LoadListView();

                //btnSave.Visible = false;
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


        private long CurrentTablePrimaryID
        {
            get
            {
                if (ViewState["CurrentTablePrimaryID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentTablePrimaryID"].ToString());
            }
            set
            {
                ViewState["CurrentTablePrimaryID"] = value;
            }
        }

        private void ClearFields()
        {
            ddlPropertyType.SelectedIndex = -1;
            ddlEducationCategory.SelectedIndex = -1;
            ddlProgram.SelectedIndex = -1;
            //txtNameText.Text = string.Empty;
            //txtURL.Text = string.Empty;
            //txtRemarks.Text = string.Empty;
            //txtSerialNo.Text = string.Empty;
            cbIsVisible.Checked = false;
            //cbIsActive.Checked = false;


            CurrentTablePrimaryID = 0;

            btnSave.Text = "Save";

            panelProgramDDL.Visible = false;
        }


        protected void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.PropertyType>(ddlPropertyType, db.AdmissionDB.PropertyTypes.Where(a => a.IsActive == true).ToList(), "PropertyName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.ID == 4 || a.ID == 6).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.Select);

                List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();
                DDLHelper.Bind<DAL.SPProgramsGetAllFromUCAM_Result>(ddlProgram, programs.OrderBy(a => a.DetailName).ToList(), "DetailNShortName", "ProgramID", EnumCollection.ListItemType.Program);

            }

        }


        private void LoadListView()
        {
            List<DAL.PropertySetup> list = null;
            using (var db = new OfficeDataManager())
            {
                list = db.AdmissionDB.PropertySetups
                    .OrderBy(a => a.EducationCategoryID)
                    .ThenBy(x=> x.ProgramId)
                    .ThenBy(x=> x.PropertyTypeID)
                    .ToList();
            }

            if (list != null && list.Count > 0)
            {
                lvPropertySetup.DataSource = list.ToList();
                lblCount.Text = list.Count().ToString();
            }
            else
            {
                lvPropertySetup.DataSource = null;
                lblCount.Text = list.Count().ToString();
            }
            lvPropertySetup.DataBind();
        }

        protected void lvPropertySetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.PropertySetup propertySetupModel = (DAL.PropertySetup)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblPropertyTypeName = (Label)currentItem.FindControl("lblPropertyTypeName");
                Label lblEducationCategoryName = (Label)currentItem.FindControl("lblEducationCategoryName");
                Label lblProgramName = (Label)currentItem.FindControl("lblProgramName");
                CheckBox cbListIsVisible = (CheckBox)currentItem.FindControl("cbListIsVisible");
                HiddenField hfcbIsVisibleID = (HiddenField)currentItem.FindControl("hfcbIsVisibleID");

                //Label lblPropertyNameText = (Label)currentItem.FindControl("lblPropertyNameText");
                //Label lblPropertyURL = (Label)currentItem.FindControl("lblPropertyURL");
                //Label lblPropertyRemark = (Label)currentItem.FindControl("lblPropertyRemark");
                //Label lblPropertySerialNo = (Label)currentItem.FindControl("lblPropertySerialNo");
                Label lblIsVisible = (Label)currentItem.FindControl("lblIsVisible");
                //Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");



                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                #region Get Property Type
                List<DAL.PropertyType> ptList = null;
                List<DAL.EducationCategory> eduCatList = null;
                using (var db = new GeneralDataManager())
                {
                    ptList = db.AdmissionDB.PropertyTypes.Where(x => x.IsActive == true).ToList();
                    eduCatList = db.AdmissionDB.EducationCategories.Where(x => x.IsActive == true).ToList();
                }
                #endregion

                if (ptList != null && ptList.Count > 0)
                {
                    lblPropertyTypeName.Text = ptList.Where(x => x.ID == propertySetupModel.PropertyTypeID).Select(x => x.ID).FirstOrDefault() > 0 ? ptList.Where(x => x.ID == propertySetupModel.PropertyTypeID).Select(x => x.PropertyName).FirstOrDefault() : "";

                }

                if (eduCatList != null && eduCatList.Count > 0)
                {
                    lblEducationCategoryName.Text = eduCatList.Where(x => x.ID == propertySetupModel.EducationCategoryID).Select(x => x.ID).FirstOrDefault() > 0 ? eduCatList.Where(x => x.ID == propertySetupModel.EducationCategoryID).Select(x => x.CategoryName).FirstOrDefault() : "";

                }

                if (propertySetupModel != null && Convert.ToInt32(propertySetupModel.ProgramId) > 0)
                {
                    DAL.SPProgramsGetByIdFromUCAM_Result programModel = null;
                    using (var db = new GeneralDataManager())
                    {
                        programModel = db.AdmissionDB.SPProgramsGetByIdFromUCAM(Convert.ToInt32(propertySetupModel.ProgramId)).FirstOrDefault();
                    }

                    if (programModel != null)
                    {
                        lblProgramName.Text = programModel.DetailNShortName;
                    }

                }



                //lblPropertyNameText.Text = propertySetupModel.Name;
                //lblPropertyURL.Text = propertySetupModel.URL;
                //lblPropertyRemark.Text = propertySetupModel.Remark;
                //lblPropertySerialNo.Text = propertySetupModel.SerialNo.ToString();


                if (propertySetupModel.IsVisible == true)
                {
                    lblIsVisible.Text = "YES";
                    //lblIsVisible.ForeColor = Color.Green;
                    lblIsVisible.CssClass = "label label-success";

                    cbListIsVisible.Checked = true;
                }
                else
                {
                    lblIsVisible.Text = "NO";
                    //lblIsVisible.ForeColor = Color.Crimson;
                    lblIsVisible.CssClass = "label label-danger";

                    cbListIsVisible.Checked = false;
                }

       

                //if (propertySetupModel.IsActive == true)
                //{
                //    lblIsActive.Text = "YES";
                //    lblIsActive.ForeColor = Color.Green;
                //}
                //else
                //{
                //    lblIsActive.Text = "NO";
                //    lblIsActive.ForeColor = Color.Crimson;
                //}



                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = propertySetupModel.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = propertySetupModel.ID.ToString();

                hfcbIsVisibleID.Value = propertySetupModel.ID.ToString();


            }
        }

        protected void lvPropertySetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            MessageView("", "clear");

            if (e.CommandName == "Delete")
            {

                try
                {
                    using (var db = new GeneralDataManager())
                    {
                        var objectToDelete = db.AdmissionDB.PropertySetups.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.PropertySetup>(objectToDelete);
                        CurrentTablePrimaryID = 0;
                    }

                    LoadListView();

                    MessageView("Data deleted successfully.", "success");
                }
                catch (Exception ex)
                {
                    MessageView("Unable to delete !!", "fail");
                }
            }
            else if (e.CommandName == "Update")
            {
                using (var db = new GeneralDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.PropertySetups.Find(Convert.ToInt64(e.CommandArgument));

                    ClearFields();

                    LoadDDL();

                    ddlPropertyType.SelectedValue = objectToUpdate.PropertyTypeID.ToString();
                    ddlEducationCategory.SelectedValue = objectToUpdate.EducationCategoryID.ToString();

                    //txtNameText.Text = objectToUpdate.Name;
                    //txtURL.Text = objectToUpdate.URL;
                    //txtRemarks.Text = objectToUpdate.Remark;
                    //txtSerialNo.Text = objectToUpdate.SerialNo.ToString();

                    if (objectToUpdate.ProgramId != null)
                    {
                        ddlProgram.SelectedValue = objectToUpdate.ProgramId.ToString();
                        panelProgramDDL.Visible = true;
                    }
                    else
                    {
                        panelProgramDDL.Visible = false;
                    }

                    if (objectToUpdate.IsVisible == true)
                    {
                        cbIsVisible.Checked = true;
                    }
                    else
                    {
                        cbIsVisible.Checked = false;
                    }

                    //if (objectToUpdate.IsActive == true)
                    //{
                    //    cbIsActive.Checked = true;
                    //}
                    //else
                    //{
                    //    cbIsActive.Checked = false;
                    //}


                    if (objectToUpdate != null && Convert.ToInt32(objectToUpdate.ProgramId) > 0)
                    {
                        ddlProgram.SelectedValue = objectToUpdate.ProgramId.ToString();

                    }


                    CurrentTablePrimaryID = objectToUpdate.ID;
                    
                    btnSave.Text = "Update";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenCollapse", "openCollapse()", true);

                }
            }
        }

        protected void lvPropertySetup_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }

        protected void lvPropertySetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            DAL.PropertySetup obj = new DAL.PropertySetup();



            int propertyTypeId = Convert.ToInt32(ddlPropertyType.SelectedValue);
            int educationCategoryId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
            int programId = Convert.ToInt32(ddlProgram.SelectedValue);


            if (propertyTypeId > 0)
            {

                obj.PropertyTypeID = propertyTypeId;

                if (educationCategoryId > 0)
                {
                    obj.EducationCategoryID = educationCategoryId;
                }
                

                //if (!string.IsNullOrEmpty(txtNameText.Text))
                //{
                //    obj.Name = txtNameText.Text;
                //}


                //if (!string.IsNullOrEmpty(txtURL.Text))
                //{
                //    obj.URL = txtURL.Text;
                //}

                //if (!string.IsNullOrEmpty(txtRemarks.Text))
                //{
                //    obj.Remark = txtRemarks.Text;
                //}

                //if (!string.IsNullOrEmpty(txtSerialNo.Text))
                //{
                //    obj.SerialNo = Convert.ToInt32(txtSerialNo.Text);
                //}

                if (programId>0)
                {
                    obj.ProgramId = programId;
                }

                obj.IsVisible = cbIsVisible.Checked;
                obj.IsActive = true;


                obj.ID = CurrentTablePrimaryID;

                try
                {
                    if (obj.ID > 0) //update
                    {
                        using (var db1 = new GeneralDataManager())
                        {
                            DAL.PropertySetup tempObj = new DAL.PropertySetup();
                            tempObj = db1.AdmissionDB.PropertySetups.Find(obj.ID);

                            if (tempObj != null)
                            {
                                obj.DateCreated = tempObj.DateCreated;
                                obj.CreatedBy = tempObj.CreatedBy;
                            }
                        }
                        using (var db = new GeneralDataManager())
                        {
                            obj.DateModified = DateTime.Now;
                            obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                            db.Update<DAL.PropertySetup>(obj);
                        }

                        btnSave.Text = "Save";
                        btnSave.Visible = false;
                        ClearFields();
                        LoadListView();

                        MessageView("Data Updated successfully", "success");
                    }
                    else //create new.
                    {

                        obj.DateCreated = DateTime.Now;
                        obj.CreatedBy = uId;

                        using (var db = new GeneralDataManager())
                        {
                            db.Insert<DAL.PropertySetup>(obj);
                        }

                        btnSave.Text = "Save";
                        btnSave.Visible = false;
                        ClearFields();
                        LoadListView();

                        MessageView("Data Saved Successfully", "success");
                    }
                }
                catch (Exception ex)
                {
                    MessageView("Exception: " + ex.Message.ToString(), "fail");
                }


            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();

            MessageView("", "clear");
        }

        protected void ddlEducationCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");
            try
            {
                int eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                if (eduCatId > 0 && eduCatId == 6)
                {
                    panelProgramDDL.Visible = true;

                }
                else
                {
                    panelProgramDDL.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenCollapse", "openCollapse()", true);
            }
        }

        protected void cbListIsVisible_CheckedChanged(object sender, EventArgs e)
        {
            MessageView("", "clear");
            try
            {
                ListViewDataItem row = (ListViewDataItem)(((CheckBox)sender)).NamingContainer;

                CheckBox cbListIsVisible = (CheckBox)row.FindControl("cbListIsVisible");
                HiddenField hfcbIsVisibleID = (HiddenField)row.FindControl("hfcbIsVisibleID");

                Label lblEducationCategoryName = (Label)row.FindControl("lblEducationCategoryName");
                Label lblPropertyTypeName = (Label)row.FindControl("lblPropertyTypeName");

                if (!string.IsNullOrEmpty(hfcbIsVisibleID.Value))
                {
                    long propertySetupId = Convert.ToInt64(hfcbIsVisibleID.Value);

                    if (propertySetupId > 0)
                    {
                        using (var db = new GeneralDataManager())
                        {
                            var objectToUpdate = db.AdmissionDB.PropertySetups.Where(x=> x.ID == propertySetupId).FirstOrDefault();

                            if (objectToUpdate != null)
                            {
                                objectToUpdate.IsVisible = cbListIsVisible.Checked;

                                objectToUpdate.DateModified = DateTime.Now;
                                objectToUpdate.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                                db.Update<DAL.PropertySetup>(objectToUpdate);

                                ClearFields();
                                LoadListView();

                                string msg = "";
                                if (cbListIsVisible.Checked)
                                {
                                    msg += "Property Setup: " + lblEducationCategoryName.Text + " => " + lblPropertyTypeName.Text + " Visible On";
                                }
                                else
                                {
                                    msg += "Property Setup: " + lblEducationCategoryName.Text + " => " + lblPropertyTypeName.Text + " Visible Off";
                                }

                                //MessageView(msg, "success");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }
    }
}