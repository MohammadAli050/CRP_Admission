using Admission.App_Start;
using CommonUtility;
using DAL;
using DAL.ViewModels;
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
    public partial class AdmissionSscHscGpa : PageBase
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
            }
        }

        private int CurrentAdmissionUnitProgramID
        {
            get
            {
                if (ViewState["CurrentAdmissionUnitProgramID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["CurrentAdmissionUnitProgramID"].ToString());
            }
            set
            {
                ViewState["CurrentAdmissionUnitProgramID"] = value;
            }
        }

        private void ClearFields()
        {
            ddlAdmissionUnit.SelectedIndex = -1;
            ddlExamType.SelectedIndex = -1;
            ddlGroup.SelectedIndex = -1;
            txtGPA.Text = string.Empty;

        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {

                //List<SPGetAllBachelorFacultys_Result> bachelorFacultys = db.AdmissionDB.SPGetAllBachelorFacultys().ToList();
                //DDLHelper.Bind<SPGetAllBachelorFacultys_Result>(ddlAdmissionUnit, bachelorFacultys.OrderBy(a => a.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);

                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.ToList(), "UnitName", "ID", EnumCollection.ListItemType.Select);

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            bool isChecked = false;
            DAL.AdmissionSscHscGpaSetup obj = new DAL.AdmissionSscHscGpaSetup();
            DAL.AdmissionSscHscGpaSetup objSHGS = new DAL.AdmissionSscHscGpaSetup();

            int facultyId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);
            int examTypeId = Convert.ToInt32(ddlExamType.SelectedValue);
            int groupId = Convert.ToInt32(ddlGroup.SelectedValue);
            decimal gpa = Convert.ToDecimal(txtGPA.Text);
            decimal totalGPAPoint = Convert.ToDecimal(txtTotalGPAPoint.Text);

            List<DAL.SPGetAdmissionSscHscGpaSetupByAdmissionUnitIDExamTypeIDGroupID_Result> admSscHscGpaSetupList = null;

            if (facultyId > 0 && examTypeId > 0 && groupId > 0 && gpa > 0 && totalGPAPoint > 0)
            {
                obj.ID = CurrentAdmissionUnitProgramID;

                try
                {
                    if (obj.ID > 0) //update
                    {
                        using (var db1 = new GeneralDataManager())
                        {
                            DAL.AdmissionSscHscGpaSetup tempObj = new DAL.AdmissionSscHscGpaSetup();
                            tempObj = db1.AdmissionDB.AdmissionSscHscGpaSetups.Find(obj.ID);

                            if (tempObj != null)
                            {

                                obj.AdmissionUnitID = facultyId;
                                obj.ExamTypeID = examTypeId;
                                obj.GroupID = groupId;
                                obj.GPA = gpa;
                                obj.TotalGPAPoint = totalGPAPoint;

                                obj.DateCreated = Convert.ToDateTime(tempObj.DateCreated);
                                obj.CreatedBy = Convert.ToInt32(tempObj.CreatedBy);
                            }
                        }
                        using (var db = new GeneralDataManager())
                        {
                            obj.DateModified = DateTime.Now;
                            obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                            db.Update<DAL.AdmissionSscHscGpaSetup>(obj);
                        }
                        lblMessage.Text = "Updated successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                    else //create new.
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admSscHscGpaSetupList = db.AdmissionDB.SPGetAdmissionSscHscGpaSetupByAdmissionUnitIDExamTypeIDGroupID(facultyId, examTypeId, groupId).ToList();

                        }

                        if (admSscHscGpaSetupList.Count > 0) //admission unit program already exist for given params, do not proceed.
                        {
                            lblMessage.Text = "Already exist with the same Faculty, Group.";
                            messagePanel.CssClass = "alert alert-danger";
                            messagePanel.Visible = true;
                            return;
                        }
                        else
                        {
                            using (var db = new OfficeDataManager())
                            {
                                objSHGS.AdmissionUnitID = facultyId;
                                objSHGS.ExamTypeID = examTypeId;
                                objSHGS.GroupID = groupId;
                                objSHGS.GPA = gpa;
                                objSHGS.TotalGPAPoint = totalGPAPoint;
                                objSHGS.IsActive = isChecked;
                                objSHGS.DateCreated = DateTime.Now;
                                objSHGS.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                                objSHGS.DateModified = null;
                                objSHGS.ModifiedBy = null;
                                db.Insert<DAL.AdmissionSscHscGpaSetup>(objSHGS);
                                id = objSHGS.ID;
                            }
                            if (id > 0)
                            {
                                lblMessage.Text = "Saved successfully.";
                                messagePanel.CssClass = "alert alert-success";
                                messagePanel.Visible = true;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to save/update data.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }
            btnSave.Text = "Save";
            ClearFields();
            LoadListView();

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            lblMessage.Text = string.Empty;
            messagePanel.CssClass = string.Empty;
            messagePanel.Visible = false;
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {


                List<DAL.SPAdmissionSscHscGpaSetupGetAll_Result> list = db.AdmissionDB.SPAdmissionSscHscGpaSetupGetAll().ToList();

                if (list != null)
                {
                    lvAdmissionUnitProgram.DataSource = list.ToList();
                    lblCount.Text = list.Count().ToString();
                }
                else
                {
                    lvAdmissionUnitProgram.DataSource = null;
                    lblCount.Text = list.Count().ToString();
                }
                lvAdmissionUnitProgram.DataBind();
            }
        }

        protected void lvAdmissionUnitProgram_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPAdmissionSscHscGpaSetupGetAll_Result admSSCHSCGPAProg = (DAL.SPAdmissionSscHscGpaSetupGetAll_Result)((ListViewDataItem)(e.Item)).DataItem;


                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblFacultyName = (Label)currentItem.FindControl("lblFacultyName");
                Label lblExamType = (Label)currentItem.FindControl("lblExamType");
                Label lblGroupName = (Label)currentItem.FindControl("lblGroupName");
                Label lblGPA = (Label)currentItem.FindControl("lblGPA");
                Label lblTotalGPAPoint = (Label)currentItem.FindControl("lblTotalGPAPoint");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                lblFacultyName.Text = admSSCHSCGPAProg.FacultyName;

                lblExamType.Text = admSSCHSCGPAProg.ExamTypeName;

                lblGroupName.Text = admSSCHSCGPAProg.GroupName;

                lblGPA.Text = admSSCHSCGPAProg.GPA.ToString();

                string totalGPAPoint = string.Empty;
                DAL.AdmissionSscHscGpaSetup ashs = null;
                using (var db = new OfficeDataManager())
                {
                    ashs = db.AdmissionDB.AdmissionSscHscGpaSetups.Where(x => x.ID == admSSCHSCGPAProg.ID).FirstOrDefault();
                }
                if (ashs != null && !string.IsNullOrEmpty(ashs.TotalGPAPoint.ToString()))
                {
                    totalGPAPoint = ashs.TotalGPAPoint.ToString();
                }
                lblTotalGPAPoint.Text = totalGPAPoint;


                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = admSSCHSCGPAProg.ID.ToString();

            }
        }

        protected void lvAdmissionUnitProgram_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                using (var db = new GeneralDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.AdmissionSscHscGpaSetups.Find(Convert.ToInt64(e.CommandArgument));
                    ClearFields();
                    ddlAdmissionUnit.SelectedValue = objectToUpdate.AdmissionUnitID.ToString();
                    ddlExamType.SelectedValue = objectToUpdate.ExamTypeID.ToString();
                    ddlGroup.SelectedValue = objectToUpdate.GroupID.ToString();
                    txtGPA.Text = objectToUpdate.GPA.ToString();
                    txtTotalGPAPoint.Text = objectToUpdate.TotalGPAPoint.ToString();

                    CurrentAdmissionUnitProgramID = objectToUpdate.ID;

                    ddlAdmissionUnit.Focus();

                    btnSave.Text = "Update";
                }
            }
            //LoadListView();
        }

        protected void lvAdmissionUnitProgram_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvAdmissionUnitProgram_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }







        #region New Setup Code -- Backend -- Work later
        //private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        //long uId = 0;
        //string uRole = string.Empty;
        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
        //    uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

        //    if (uRole.ToLower() == "candidate")
        //    {

        //        SessionSGD.DeleteFromSession(SessionName.Common_UserId);
        //        SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
        //        SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
        //        SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
        //        SessionSGD.DeleteFromSession(SessionName.Common_UserG);
        //        Response.Redirect("~/Admission/Home.aspx", false);
        //    }

        //    if (!IsPostBack)
        //    {
        //        LoadDDL();
        //        LoadData();
        //    }
        //}

        //protected void MessageView(string msg, string status)
        //{

        //    if (status == "success")
        //    {
        //        lblMessage.Text = string.Empty;
        //        lblMessage.Text = msg.ToString();
        //        lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
        //        lblMessage.Attributes.CssStyle.Add("color", "green");

        //        messagePanel.Visible = true;
        //        messagePanel.CssClass = "alert alert-success";


        //    }
        //    else if (status == "fail")
        //    {
        //        lblMessage.Text = string.Empty;
        //        lblMessage.Text = msg.ToString();
        //        lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
        //        lblMessage.Attributes.CssStyle.Add("color", "crimson");

        //        messagePanel.Visible = true;
        //        messagePanel.CssClass = "alert alert-danger";
        //    }
        //    else if (status == "clear")
        //    {
        //        lblMessage.Text = string.Empty;
        //        messagePanel.Visible = false;
        //    }

        //}


        //private int AdmissionSscHscGpaID
        //{
        //    get
        //    {
        //        if (ViewState["AdmissionSscHscGpaID"] == null)
        //            return 0;
        //        else
        //            return Convert.ToInt32(ViewState["AdmissionSscHscGpaID"].ToString());
        //    }
        //    set
        //    {
        //        ViewState["AdmissionSscHscGpaID"] = value;
        //    }
        //}

        //private void ClearFields()
        //{
        //    ddlAdmissionUnit.SelectedValue = "-1";
        //    ddlEducationCategory.SelectedValue = "-1";
        //    ddlProgram.Items.Clear();
        //    ddlExamType.SelectedValue = "-1";
        //    ddlGroup.SelectedValue = "-1";
        //    ddlResultDivision.SelectedValue = "1";
        //    txtGPAFrom.Text = string.Empty;
        //    txtGPATo.Text = string.Empty;
        //    txtTotalGPA.Text = string.Empty;
        //    txtGPAPoint.Text = string.Empty;

        //    AdmissionSscHscGpaID = 0;
        //    btnSave.Text = "Save";
        //}

        //private void LoadDDL()
        //{
        //    using (var db = new GeneralDataManager())
        //    {
        //        DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.ToList(), "UnitName", "ID", EnumCollection.ListItemType.Select);
        //    }

        //}

        //protected void ddlEducationCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (var db = new GeneralDataManager())
        //        {
        //            List<DAL.AdmissionUnitProgram> list = new List<AdmissionUnitProgram>();

        //            if (Convert.ToInt32(ddlEducationCategory.SelectedValue) > 0 && Convert.ToInt32(ddlEducationCategory.SelectedValue) == 6)
        //            {
        //                ddlProgram.Enabled = true;
        //                CompareValidator2.Enabled = true;

        //                ddlExamType.Enabled = true;
        //                CompareValidator3.Enabled = true;

        //                ddlGroup.Enabled = true;
        //                CompareValidator4.Enabled = true;

        //                list = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.IsActive == true && x.EducationCategoryID == 6).ToList();
        //            }
        //            else
        //            {
        //                ddlProgram.Enabled = false;
        //                CompareValidator2.Enabled = false;

        //                ddlExamType.Enabled = false;
        //                CompareValidator3.Enabled = false;

        //                ddlGroup.Enabled = false;
        //                CompareValidator4.Enabled = false;

        //                list = new List<AdmissionUnitProgram>();
        //            }

        //            DDLHelper.Bind<DAL.AdmissionUnitProgram>(ddlProgram, list, "ProgramName", "ProgramID", EnumCollection.ListItemType.Program);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}


        //private void LoadData()
        //{
        //    MessageView("", "clear");

        //    try
        //    {
        //        List<DAL.SPAdmissionSscHscGpaSetupGetAll_Result> list = null;
        //        using (var db = new OfficeDataManager())
        //        {
        //            list = db.AdmissionDB.SPAdmissionSscHscGpaSetupGetAll().ToList();
        //        }

        //        if (list != null && list.Count > 0)
        //        {
        //            gvGPASetup.DataSource = list.ToList();
        //            gvGPASetup.DataBind();

        //            lblTotal.Text = list.Count().ToString();
        //        }
        //        else
        //        {
        //            gvGPASetup.DataSource = null;
        //            gvGPASetup.DataBind();

        //            lblTotal.Text = "0";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
        //    }
        //}



        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    DAL.AdmissionSscHscGpaSetup obj = new DAL.AdmissionSscHscGpaSetup();
        //    DAL.AdmissionSscHscGpaSetup objSHGS = new DAL.AdmissionSscHscGpaSetup();


        //    MessageView("", "clear");

        //    try
        //    {

        //        if (string.IsNullOrEmpty(txtGPAFrom.Text.Trim()) && Convert.ToDecimal(txtGPAFrom.Text.Trim()) <= 0)
        //        {
        //            MessageView("GPA From input is invalid!", "fail");
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(txtGPATo.Text.Trim()) && Convert.ToDecimal(txtGPATo.Text.Trim()) <= 0)
        //        {
        //            MessageView("GPA To input is invalid!", "fail");
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(txtTotalGPA.Text.Trim()) && Convert.ToDecimal(txtTotalGPA.Text.Trim()) <= 0)
        //        {
        //            MessageView("Total GPA input is invalid!", "fail");
        //            return;
        //        }

        //        int facultyId = Convert.ToInt32(ddlAdmissionUnit.SelectedValue);

        //        int examTypeId = Convert.ToInt32(ddlExamType.SelectedValue);

        //        int educationCategoryId = Convert.ToInt32(ddlEducationCategory.SelectedValue);

        //        int? programId = null;

        //        int groupId = Convert.ToInt32(ddlGroup.SelectedValue);

        //        decimal gpaFrom = Convert.ToDecimal(txtGPAFrom.Text.Trim());

        //        decimal gpaTo = Convert.ToDecimal(txtGPATo.Text.Trim());

        //        decimal totalGPA = Convert.ToDecimal(txtTotalGPA.Text.Trim());

        //        if (facultyId > 0 && examTypeId > 0 && educationCategoryId > 0 && groupId > 0 && facultyId > 0)
        //        {
        //            //6 = Masters
        //            if (educationCategoryId == 6)
        //            {
        //                programId = Convert.ToInt32(ddlProgram.SelectedValue);
        //            }

        //            int admissionSscHscGpaId = AdmissionSscHscGpaID;

        //            DAL.AdmissionSscHscGpaSetup modelExist = null;
        //            using (var db = new OfficeDataManager())
        //            {
        //                modelExist = db.AdmissionDB.AdmissionSscHscGpaSetups.Where(x => x.ID == admissionSscHscGpaId).FirstOrDefault();
        //            }

        //            //if (modelExist != null)
        //            //{
        //            //    modelExist.
        //            //}




        //            List<DAL.SPGetAdmissionSscHscGpaSetupByAdmissionUnitIDExamTypeIDGroupID_Result> admSscHscGpaSetupList = null;

        //            obj.ID = AdmissionSscHscGpaID;

        //            try
        //            {
        //                if (obj.ID > 0) //update
        //                {
        //                    using (var db1 = new GeneralDataManager())
        //                    {
        //                        DAL.AdmissionSscHscGpaSetup tempObj = new DAL.AdmissionSscHscGpaSetup();
        //                        tempObj = db1.AdmissionDB.AdmissionSscHscGpaSetups.Find(obj.ID);

        //                        if (tempObj != null)
        //                        {

        //                            obj.AdmissionUnitID = facultyId;
        //                            obj.EducationCategoryId = educationCategoryId;
        //                            obj.ProgramId = programId;
        //                            obj.ExamTypeID = examTypeId;
        //                            obj.GroupID = groupId;
        //                            obj.GPAFrom = gpaFrom;
        //                            obj.GPATo = gpaTo;
        //                            obj.TotalGPA = totalGPA;

        //                            if (!string.IsNullOrEmpty(txtGPAPoint.Text.Trim()) && Convert.ToDecimal(txtGPAPoint.Text.Trim()) > 0)
        //                            {
        //                                obj.GPAPoint = Convert.ToDecimal(txtGPAPoint.Text.Trim());
        //                            }
        //                            else
        //                            {
        //                                obj.GPAPoint = 0;
        //                            }

        //                            obj.CreatedBy = Convert.ToInt32(tempObj.CreatedBy);
        //                            obj.DateCreated = Convert.ToDateTime(tempObj.DateCreated);
        //                        }
        //                    }
        //                    using (var db = new GeneralDataManager())
        //                    {
        //                        obj.DateModified = DateTime.Now;
        //                        obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
        //                        db.Update<DAL.AdmissionSscHscGpaSetup>(obj);
        //                    }
        //                    //lblMessage.Text = "Updated successfully.";
        //                    //messagePanel.CssClass = "alert alert-success";
        //                    //messagePanel.Visible = true;

        //                    btnSave.Text = "Save";
        //                    ClearFields();
        //                    LoadData();

        //                    MessageView("Updated successfully", "success");
        //                }
        //                else //create new.
        //                {
        //                    using (var db = new OfficeDataManager())
        //                    {
        //                        admSscHscGpaSetupList = db.AdmissionDB.SPGetAdmissionSscHscGpaSetupByAdmissionUnitIDExamTypeIDGroupID(facultyId, examTypeId, groupId).ToList();

        //                    }

        //                    if (admSscHscGpaSetupList != null && admSscHscGpaSetupList.Count > 0) //admission unit program already exist for given params, do not proceed.
        //                    {
        //                        //lblMessage.Text = "Already exist with the same Faculty, Group.";
        //                        //messagePanel.CssClass = "alert alert-danger";
        //                        //messagePanel.Visible = true;

        //                        MessageView("Already exist with the same Faculty, Group.", "fail");
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        int id = -1;
        //                        using (var db = new OfficeDataManager())
        //                        {
        //                            objSHGS.AdmissionUnitID = facultyId;
        //                            objSHGS.ExamTypeID = examTypeId;
        //                            objSHGS.GroupID = groupId;
        //                            objSHGS.GPAFrom = gpaFrom;
        //                            objSHGS.GPATo = gpaTo;
        //                            objSHGS.TotalGPA = totalGPA;
        //                            if (!string.IsNullOrEmpty(txtGPAPoint.Text.Trim()) && Convert.ToDecimal(txtGPAPoint.Text.Trim()) > 0)
        //                            {
        //                                objSHGS.GPAPoint = Convert.ToDecimal(txtGPAPoint.Text.Trim());
        //                            }
        //                            else
        //                            {
        //                                objSHGS.GPAPoint = 0;
        //                            }
        //                            objSHGS.IsActive = true;
        //                            objSHGS.DateCreated = DateTime.Now;
        //                            objSHGS.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
        //                            objSHGS.DateModified = null;
        //                            objSHGS.ModifiedBy = null;
        //                            db.Insert<DAL.AdmissionSscHscGpaSetup>(objSHGS);
        //                            id = objSHGS.ID;
        //                        }

        //                        if (id > 0)
        //                        {
        //                            //lblMessage.Text = "Saved successfully.";
        //                            //messagePanel.CssClass = "alert alert-success";
        //                            //messagePanel.Visible = true;

        //                            btnSave.Text = "Save";
        //                            ClearFields();
        //                            LoadData();

        //                            MessageView("Saved successfully", "success");
        //                        }
        //                    }

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //lblMessage.Text = "Unable to save/update data.";
        //                //messagePanel.CssClass = "alert alert-danger";
        //                //messagePanel.Visible = true;

        //                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
        //            }






        //        }
        //        else
        //        {
        //            MessageView("Please provide all required inputs!", "fail");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
        //    }









        //}

        //protected void btnEdit_Click(object sender, EventArgs e)
        //{
        //    MessageView("", "clear");

        //    try
        //    {
        //        GridViewRow row = (GridViewRow)(((Button)sender)).NamingContainer;

        //        Label lblAdmissionSscHscGpaSetupId = (Label)row.FindControl("lblAdmissionSscHscGpaSetupId");

        //        if (!string.IsNullOrEmpty(lblAdmissionSscHscGpaSetupId.Text) && Convert.ToInt32(lblAdmissionSscHscGpaSetupId.Text) > 0)
        //        {

        //            int admissionSscHscGpaSetupId = Convert.ToInt32(lblAdmissionSscHscGpaSetupId.Text);

        //            DAL.AdmissionSscHscGpaSetup model = null;
        //            using (var db = new OfficeDataManager())
        //            {
        //                model = db.AdmissionDB.AdmissionSscHscGpaSetups.Where(x => x.ID == admissionSscHscGpaSetupId).FirstOrDefault();
        //            }

        //            if (model != null)
        //            {
        //                btnSave.Text = "Update";

        //                AdmissionSscHscGpaID = model.ID;

        //                ddlAdmissionUnit.SelectedValue = model.AdmissionUnitID.ToString();

        //                ddlEducationCategory.SelectedValue = model.EducationCategoryId.ToString();

        //                if (model.EducationCategoryId != null)
        //                {
        //                    ddlEducationCategory_SelectedIndexChanged(null, null);

        //                    if (model.EducationCategoryId == 6)
        //                    {
        //                        ddlProgram.SelectedValue = model.ProgramId.ToString();

        //                        ddlExamType.SelectedValue = model.ExamTypeID.ToString();

        //                        ddlGroup.SelectedValue = model.GroupID.ToString();
        //                    }
        //                }

        //                ddlResultDivision.SelectedValue = model.ResultDivisionId.ToString();

        //                txtGPAFrom.Text = model.GPAFrom.ToString();

        //                txtGPATo.Text = model.GPATo.ToString();

        //                txtTotalGPA.Text = model.TotalGPA.ToString();

        //                txtGPAPoint.Text = model.GPAPoint.ToString();
        //            }
        //            else
        //            {
        //                MessageView("No Data Found!", "fail");
        //            }
        //        }
        //        else
        //        {
        //            MessageView("Invalid Request!", "fail");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
        //    }
        //}

        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    ClearFields();
        //    MessageView("", "clear");
        //} 
        #endregion













    }
}