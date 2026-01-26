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
    public partial class AdmissionHSCSSCPercentageSetup : PageBase
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
                ddlSession_SelectedIndexChanged(null, null);
                LoadSSCHSCPercentageList(0, 0, 0);
                //btnSave.Visible = false;
            }
        }

        private void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSessionAddUpdate, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                List<DAL.AdmissionSetup> admSetup = new List<DAL.AdmissionSetup>();

                ddlFaculty.Items.Clear();
                ddlFaculty.Items.Add(new ListItem("-All-", "0"));
                ddlFaculty.AppendDataBoundItems = true;

                ddlFacultyAddUpdate.Items.Clear();
                ddlFacultyAddUpdate.Items.Add(new ListItem("-All-", "0"));
                ddlFacultyAddUpdate.AppendDataBoundItems = true;

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
                            ddlFacultyAddUpdate.Items.Add(new ListItem(admUnit.UnitName, item.ID.ToString()));
                        }

                        ddlExamType.Enabled = true;
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

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            int facultyId = Convert.ToInt32(ddlFaculty.SelectedValue);
            int examTypeId = Convert.ToInt32(ddlExamType.SelectedValue);


            LoadSSCHSCPercentageList(acaCalId, facultyId, examTypeId);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                pnlAddUpdate.Visible = true;

                ddlSessionAddUpdate.SelectedValue = "-1";
                ddlSessionAddUpdate_SelectedIndexChanged(null, null);
                ddlFacultyAddUpdate.SelectedValue = "0";
                ddlExamTypeAddUpdate.SelectedValue = "0";
                txtPercentage.Value = "";
                txtNumberSubject.Value = "";


                ddlExamTypeAddUpdate.Enabled = true;
                ddlFacultyAddUpdate.Enabled = true;
                ddlSessionAddUpdate.Enabled = true;

            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void btnAddUpdate_Click(object sender, EventArgs e)
        {
            int AcaCalId = Convert.ToInt32(ddlSessionAddUpdate.SelectedValue);
            int FacultyId = Convert.ToInt32(ddlFacultyAddUpdate.SelectedValue);
            int ExamTypeId = Convert.ToInt32(ddlExamTypeAddUpdate.SelectedValue);
            decimal MarkPercent = Convert.ToDecimal(txtPercentage.Value);
            int NoSubjects = Convert.ToInt32(txtNumberSubject.Value);

            string btnTxt = btnAddUpdate.Text;

            DAL.AdmissionHSCSSCPercentageSetup NewObj = new DAL.AdmissionHSCSSCPercentageSetup();

            if (AcaCalId == -1)
            {
                showAlert("Please Select Session");
                return;
            }

            using (var db = new OfficeDataManager())
            {
                DAL.AdmissionHSCSSCPercentageSetup existingObj = db.AdmissionDB.AdmissionHSCSSCPercentageSetups.FirstOrDefault(x => x.AcaCalId == AcaCalId && x.AdmissionSetupId == FacultyId && x.ExamtypeId == ExamTypeId);

                if (btnTxt == "Save")
                {
                    if (existingObj != null)
                    {
                        showAlert("Already Exists!");
                    }
                    else
                    {

                        if (FacultyId == 0)
                        {
                            if (ExamTypeId == 0)
                            {
                                foreach (ListItem item1 in ddlFacultyAddUpdate.Items)
                                {
                                    foreach (ListItem item2 in ddlExamTypeAddUpdate.Items)
                                    {
                                        if (item1.Value != "0" && item2.Value != "0")
                                        {
                                            NewObj.AcaCalId = AcaCalId;
                                            NewObj.AdmissionSetupId = Convert.ToInt32(item1.Value);
                                            NewObj.ExamtypeId = Convert.ToInt32(item2.Value);
                                            NewObj.Percentage = MarkPercent;
                                            NewObj.NumberOfSubject = NoSubjects;
                                            NewObj.CreatedBy = Convert.ToInt32(uId);
                                            NewObj.CreatedDate = DateTime.Now;
                                            //NewObj.ModifiedBy = null;
                                            //NewObj.ModifiedBy = null;

                                            db.Insert<DAL.AdmissionHSCSSCPercentageSetup>(NewObj);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (ListItem item in ddlFacultyAddUpdate.Items)
                                {
                                    if (item.Value != "0")
                                    {
                                        NewObj.AcaCalId = AcaCalId;
                                        NewObj.AdmissionSetupId = Convert.ToInt32(item.Value);
                                        NewObj.ExamtypeId = ExamTypeId;
                                        NewObj.Percentage = MarkPercent;
                                        NewObj.NumberOfSubject = NoSubjects;
                                        NewObj.CreatedBy = Convert.ToInt32(uId);
                                        NewObj.CreatedDate = DateTime.Now;
                                        //NewObj.ModifiedBy = null;
                                        //NewObj.ModifiedBy = null;

                                        db.Insert<DAL.AdmissionHSCSSCPercentageSetup>(NewObj);
                                    }
                                }

                            }
                        }

                        else
                        {
                            if (ExamTypeId == 0)
                            {
                                foreach (ListItem item in ddlExamTypeAddUpdate.Items)
                                {
                                    if (item.Value != "0")
                                    {
                                        NewObj.AcaCalId = AcaCalId;
                                        NewObj.AdmissionSetupId = FacultyId;
                                        NewObj.ExamtypeId = Convert.ToInt32(item.Value);
                                        NewObj.Percentage = MarkPercent;
                                        NewObj.NumberOfSubject = NoSubjects;
                                        NewObj.CreatedBy = Convert.ToInt32(uId);
                                        NewObj.CreatedDate = DateTime.Now;
                                        //NewObj.ModifiedBy = null;
                                        //NewObj.ModifiedBy = null;

                                        db.Insert<DAL.AdmissionHSCSSCPercentageSetup>(NewObj);
                                    }
                                }
                            }
                            else
                            {
                                NewObj.AcaCalId = AcaCalId;
                                NewObj.AdmissionSetupId = FacultyId;
                                NewObj.ExamtypeId = ExamTypeId;
                                NewObj.Percentage = MarkPercent;
                                NewObj.NumberOfSubject = NoSubjects;
                                NewObj.CreatedBy = Convert.ToInt32(uId);
                                NewObj.CreatedDate = DateTime.Now;
                                //NewObj.ModifiedBy = null;
                                //NewObj.ModifiedBy = null;

                                db.Insert<DAL.AdmissionHSCSSCPercentageSetup>(NewObj);
                            }
                        }

                        showAlert("Mark Inserted");
                    }
                }
                else
                {
                    if (existingObj != null)
                    {
                        existingObj.AcaCalId = AcaCalId;
                        existingObj.AdmissionSetupId = FacultyId;
                        existingObj.ExamtypeId = ExamTypeId;
                        existingObj.Percentage = MarkPercent;
                        existingObj.NumberOfSubject = NoSubjects;
                        //existingObj.CreatedBy = Convert.ToInt32(uId);
                        //existingObj.CreatedDate = DateTime.Now;
                        existingObj.ModifiedBy = Convert.ToInt32(uId);
                        existingObj.ModifiedDate = DateTime.Now;

                        db.Update<DAL.AdmissionHSCSSCPercentageSetup>(existingObj);
                        showAlert("Mark Updated");
                        btnAddUpdate.Text = "Save";
                    }
                }
                LoadSSCHSCPercentageList(0, 0, 0);
                pnlAddUpdate.Visible = false;

            }
        }

        protected void LoadSSCHSCPercentageList(int acaCal, int facultyId, int exmTypeId)
        {
            try
            {
                List<DAL.AdmissionHSCSSCPercentageSetup> list = new List<DAL.AdmissionHSCSSCPercentageSetup>();
                using (var db = new GeneralDataManager())
                {
                    if (acaCal != 0 && facultyId != 0 && exmTypeId != 0)
                    {
                        list = db.AdmissionDB.AdmissionHSCSSCPercentageSetups.Where(c => c.AcaCalId == acaCal && c.AdmissionSetupId == facultyId && c.ExamtypeId == exmTypeId).ToList();
                    }
                    else
                    {
                        list = db.AdmissionDB.AdmissionHSCSSCPercentageSetups.ToList();
                    }


                    if (list != null && list.Count > 0)
                    {
                        lvPercentSetup.DataSource = list.OrderByDescending(c => c.AcaCalId).ThenBy(c => c.AdmissionSetupId);
                    }
                    else
                    {
                        lvPercentSetup.DataSource = null;
                    }

                }

                lvPercentSetup.DataBind();
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void lvPercentSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                    DAL.AdmissionHSCSSCPercentageSetup admPercent = (DAL.AdmissionHSCSSCPercentageSetup)((ListViewDataItem)(e.Item)).DataItem;

                    Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                    Label lblSession = (Label)currentItem.FindControl("lblSession");
                    Label lblFaculty = (Label)currentItem.FindControl("lblFaculty");
                    Label lblExamType = (Label)currentItem.FindControl("lblExamType");
                    Label lblPercentage = (Label)currentItem.FindControl("lblPercentage");
                    Label lblNoSubject = (Label)currentItem.FindControl("lblNoSubject");

                    LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                    LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                    lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                    lblPercentage.Text = admPercent.Percentage.ToString();
                    lblNoSubject.Text = admPercent.NumberOfSubject.ToString();

                    using (var dbSession = new GeneralDataManager())
                    {
                        var session = dbSession.AdmissionDB.SPAcademicCalendarGetAll().Where(c => c.AcademicCalenderID == admPercent.AcaCalId).FirstOrDefault();
                        if (session != null)
                        {
                            if (session.AcademicCalenderID > 0)
                            {
                                lblSession.Text = session.FullCode;
                            }
                        }
                    }

                    using (var dbFaculty = new GeneralDataManager())
                    {
                        var faculty = dbFaculty.AdmissionDB.AdmissionSetups.Where(c => c.ID == admPercent.AdmissionSetupId && c.AcaCalID == admPercent.AcaCalId).FirstOrDefault();
                        if (faculty != null)
                        {
                            if (faculty.AcaCalID > 0)
                            {
                                lblFaculty.Text = faculty.AdmissionUnit.UnitName;
                            }
                        }
                    }

                    using (var dbExamType = new GeneralDataManager())
                    {
                        var examType = dbExamType.AdmissionDB.ExamTypes.Where(c => c.ID == admPercent.ExamtypeId).FirstOrDefault();
                        if (examType != null)
                        {
                            lblExamType.Text = examType.ExamTypeName;
                        }
                    }


                    lnkEdit.CommandName = "Update";
                    lnkEdit.CommandArgument = admPercent.Id.ToString();

                    lnkDelete.CommandName = "Delete";
                    lnkDelete.CommandArgument = admPercent.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void lvPercentSetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    using (var db = new OfficeDataManager())
                    {
                        var AdmissionHSCSSCPercentageObj = db.AdmissionDB.AdmissionHSCSSCPercentageSetups.Find(Convert.ToInt32(e.CommandArgument));
                        if (AdmissionHSCSSCPercentageObj != null)
                        {
                            db.Delete(AdmissionHSCSSCPercentageObj);
                            showAlert("Delete Successfully");
                        }
                    }
                    LoadSSCHSCPercentageList(0, 0, 0);
                }
                else if (e.CommandName == "Update")
                {
                    btnAddUpdate.Text = "Update";

                    using (var db = new OfficeDataManager())
                    {
                        var AdmissionHSCSSCPercentageObj = db.AdmissionDB.AdmissionHSCSSCPercentageSetups.Find(Convert.ToInt32(e.CommandArgument));
                        if (AdmissionHSCSSCPercentageObj != null)
                        {
                            ddlSessionAddUpdate.SelectedValue = AdmissionHSCSSCPercentageObj.AcaCalId.ToString();
                            ddlSessionAddUpdate_SelectedIndexChanged(null, null);
                            ddlFacultyAddUpdate.SelectedValue = AdmissionHSCSSCPercentageObj.AdmissionSetupId.ToString();
                            ddlExamTypeAddUpdate.SelectedValue = AdmissionHSCSSCPercentageObj.ExamtypeId.ToString();
                            txtPercentage.Value = AdmissionHSCSSCPercentageObj.Percentage.ToString();
                            txtNumberSubject.Value = AdmissionHSCSSCPercentageObj.NumberOfSubject.ToString();


                            ddlExamTypeAddUpdate.Enabled = false;
                            ddlFacultyAddUpdate.Enabled = false;
                            ddlSessionAddUpdate.Enabled = false;
                            pnlAddUpdate.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void lvPercentSetup_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvPercentSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void ddlSessionAddUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                int acaCalId = Convert.ToInt32(ddlSessionAddUpdate.SelectedValue);
                List<DAL.AdmissionSetup> admSetup = new List<DAL.AdmissionSetup>();

                ddlFacultyAddUpdate.Items.Clear();
                ddlFacultyAddUpdate.Items.Add(new ListItem("-All-", "0"));
                ddlFacultyAddUpdate.AppendDataBoundItems = true;

                using (var db = new OfficeDataManager())
                {
                    admSetup = db.GetAllAdmissionSetup_AD();


                    admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive == true).ToList();  // only for Bachelors

                    if (admSetup != null && admSetup.Count > 0)
                    {
                        foreach (var item in admSetup)
                        {
                            var admUnit = db.GetAllAdmissionUnit().Where(x => x.ID == item.AdmissionUnitID).FirstOrDefault();
                            ddlFacultyAddUpdate.Items.Add(new ListItem(admUnit.UnitName, item.ID.ToString()));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }
    }
}