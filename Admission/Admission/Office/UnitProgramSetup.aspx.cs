using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class UnitProgramSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDDL();
                LoadListView();
            }
        }

        //private DAL.UnitProgramDetail UnitProgramDetailObj { get; set; }

        private long CurrentUnitProgramDetailsID
        {
            get
            {
                if (ViewState["CurrentUnitProgramDetailsID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentUnitProgramDetailsID"].ToString());
            }
            set
            {
                ViewState["CurrentUnitProgramDetailsID"] = value;
            }
        }

        private void ClearFields()
        {
            ddlAdmissionUnit.SelectedIndex = -1;
            txtIntake.Text = string.Empty;
            txtClassStart.Text = string.Empty;
            txtMethodOfApplication.Text = string.Empty;
            txtNoOfSeat.Text = string.Empty;
            txtDurationSemester.Text = string.Empty;
            txtDurationYear.Text = string.Empty;
            txtTotalCredit.Text = string.Empty;
            txtTotalCourseFee.Text = string.Empty;
            txtAdmissionEligibility.Text = string.Empty;
            txtAdmissionSyllabus.Text = string.Empty;
            txtWeightage.Text = string.Empty;
            txtExamType.Text = string.Empty;
            //infoFile = new FileUpload();
            //admissionFormFile = new FileUpload();
            txtContactInfo.Text = string.Empty;
        }

        private void ClearMessage()
        {
            lblMessage.Text = string.Empty;
            messagePanel.CssClass = string.Empty;
            messagePanel.Visible = false;
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            DAL.UnitProgramDetail obj = new DAL.UnitProgramDetail();

            obj.AdmissionUnitID = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
            obj.Intake = txtIntake.Text;
            obj.ClassStart = txtClassStart.Text;
            obj.MethodOfApplication = txtMethodOfApplication.Text;
            obj.NoOfSeats = txtNoOfSeat.Text;
            obj.DurationSemester = Convert.ToInt32(txtDurationSemester.Text);
            obj.DurationYear = Convert.ToInt32(txtDurationYear.Text);
            obj.TotalCredit = Convert.ToInt32(txtTotalCredit.Text);
            obj.TotalCourseFee = Convert.ToInt32(txtTotalCourseFee.Text);
            obj.AdmissionEligibility = txtAdmissionEligibility.Text;
            obj.AdmissionTestSyllabus = txtAdmissionSyllabus.Text;
            obj.Weightage = txtWeightage.Text;
            obj.ExamType = txtExamType.Text;
            //infoFile = new FileUpload();
            //admissionFormFile = new FileUpload();
            obj.ContactInfo = txtContactInfo.Text;

            obj.ID = CurrentUnitProgramDetailsID;
            try
            {
                if (obj.ID > 0 && CurrentUnitProgramDetailsID > 0)
                {
                    using(var db1 = new OfficeDataManager())
                    {
                        var tempObj = db1.AdmissionDB.UnitProgramDetails.Find(Convert.ToInt64(CurrentUnitProgramDetailsID));
                        if(tempObj.ID > 0)
                        {
                            obj.CreatedBy = tempObj.CreatedBy;
                            obj.DateCreated = tempObj.DateCreated;
                        }
                    }
                    obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                    obj.DateModified = DateTime.Now;
                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.UnitProgramDetail>(obj);
                    }
                    lblMessage.Text = "Unit program details updated successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;
                }
                else if(obj.ID == 0 && CurrentUnitProgramDetailsID == 0)
                {
                    obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
                    obj.DateCreated = DateTime.Now;
                    obj.ModifiedBy = null;
                    obj.DateModified = null;
                    using (var db = new OfficeDataManager())
                    {
                        db.Insert<DAL.UnitProgramDetail>(obj);
                        id = obj.ID;
                    }
                    if (id > 0)
                    {
                        lblMessage.Text = "Unit program details saved successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unable to save/update data.";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
            btnSave.Text = "Save";
            ClearFields();
            LoadListView();
            //updatePanelListView.Update();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.UnitProgramDetail> list = db.GetAllUnitProgramDetail_AD().ToList();
                if (list != null)
                {
                    lvUnitPrograms.DataSource = list.ToList();
                    lblCount.Text = list.Count.ToString();
                }
                else
                {
                    lvUnitPrograms.DataSource = null;
                    lblCount.Text = "0";
                }
                lvUnitPrograms.DataBind();
            }
        }

        protected void lvUnitPrograms_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.UnitProgramDetail unitProgramDetails = (DAL.UnitProgramDetail)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
                
                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblUnitName.Text = unitProgramDetails.AdmissionUnit.UnitName;

                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = unitProgramDetails.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = unitProgramDetails.ID.ToString();
            }
        }

        protected void lvUnitPrograms_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //DAL.AdmissionUnit admissionUnit = new DAL.AdmissionUnit();
            if (e.CommandName == "Delete")
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        var objectToDelete = db.AdmissionDB.UnitProgramDetails.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.UnitProgramDetail>(objectToDelete);
                        CurrentUnitProgramDetailsID = 0;
                    }
                    lblMessage.Text = "Unit program details deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Unable to delete.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                LoadListView();
            }
            else if (e.CommandName == "Update")
            {
                btnSave.Text = "Update";
                using (var db = new OfficeDataManager())
                {
                    var obj = db.AdmissionDB.UnitProgramDetails.Find(Convert.ToInt64(e.CommandArgument));
                    ClearFields();

                    ddlAdmissionUnit.SelectedValue = obj.AdmissionUnitID.ToString();
                    txtIntake.Text = obj.Intake;
                    txtClassStart.Text = obj.ClassStart;
                    txtMethodOfApplication.Text = obj.MethodOfApplication;
                    txtNoOfSeat.Text = obj.NoOfSeats;
                    txtDurationSemester.Text = obj.DurationSemester.ToString();
                    txtDurationYear.Text = obj.DurationYear.ToString();
                    txtTotalCredit.Text = obj.TotalCredit.ToString();
                    txtTotalCourseFee.Text = obj.TotalCourseFee.ToString();
                    txtAdmissionEligibility.Text = obj.AdmissionEligibility;
                    txtAdmissionSyllabus.Text = obj.AdmissionTestSyllabus;
                    txtWeightage.Text = obj.Weightage;
                    txtExamType.Text = obj.ExamType;
                    //infoFile = new FileUpload();
                    //admissionFormFile = new FileUpload();
                    txtContactInfo.Text = obj.ContactInfo;

                    CurrentUnitProgramDetailsID = obj.ID;
                    btnSave.Text = "Update";
                }
            }
            //LoadListView();
        }

        protected void lvUnitPrograms_ItemUpdating(object sender, ListViewUpdateEventArgs e) { }

        protected void lvUnitPrograms_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }
    }
}