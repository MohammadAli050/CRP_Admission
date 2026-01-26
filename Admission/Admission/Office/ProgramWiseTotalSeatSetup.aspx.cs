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
    public partial class ProgramWiseTotalSeatSetup : PageBase
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
                LoadTotalSeatandMeritSeatList(0, 0, 0);
                //btnSave.Visible = false;
            }
        }

        protected void LoadDDL()
        {
            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSessionAddUpdate, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
            }

            ddlProgram.Items.Add(new ListItem("-Select-", "-1"));
            ddlFaculty.Items.Add(new ListItem("-Select-", "-1"));

            ddlFacultyAddUpdate.Items.Add(new ListItem("-Select-", "-1"));
            ddlProgramAddUpdate.Items.Add(new ListItem("-Select-", "-1"));
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


                    admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive==true).ToList();  // only for Bachelors

                    if (admSetup != null && admSetup.Count > 0)
                    {
                        foreach (var item in admSetup)
                        {
                            ddlFaculty.Items.Add(new ListItem(item.AdmissionUnit.UnitName, item.ID.ToString()));
                            ddlFacultyAddUpdate.Items.Add(new ListItem(item.AdmissionUnit.UnitName, item.ID.ToString()));

                            //ddlProgram.Items.Add(new ListItem(item.AdmissionUnit.AdmissionUnitPrograms, item.AdmissionUnit.pro))
                        }
                    }
                }

                LoadProgram();
                lvTotalSeatsetup.DataSource = null;
                lvTotalSeatsetup.DataBind();
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProgram();
            lvTotalSeatsetup.DataSource = null;
            lvTotalSeatsetup.DataBind();
        }

        protected void LoadProgram()
        {
            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int facultyId = Convert.ToInt32(ddlFaculty.SelectedValue);

                List<DAL.AdmissionUnitProgram> admUp = new List<DAL.AdmissionUnitProgram>();
                List<DAL.AdmissionSetup> admSetup = new List<DAL.AdmissionSetup>();


                ddlProgram.Items.Clear();
                ddlProgram.Items.Add(new ListItem("-All-", "0"));
                ddlProgram.AppendDataBoundItems = true;


                using (var db = new OfficeDataManager())
                {
                    if (facultyId != 0)
                    {
                        var setUp = db.AdmissionDB.AdmissionSetups.Where(c => c.ID == facultyId).FirstOrDefault();
                        admUp = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == setUp.AdmissionUnitID && c.IsActive == true && c.EducationCategoryID == 4).ToList();

                        if (admUp != null && admUp.Count > 0)
                        {
                            foreach (var item in admUp)
                            {
                                ddlProgram.Items.Add(new ListItem(item.ProgramName, item.ProgramID.ToString()));
                            }
                        }
                    }
                    else
                    {
                        admSetup = db.GetAllAdmissionSetup_AD();
                        admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive==true ).ToList();  // only for Bachelors

                        foreach (var sUps in admSetup)
                        {
                            admUp = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == sUps.AdmissionUnitID && c.IsActive == true && c.EducationCategoryID == 4).ToList();

                            if (admUp != null && admUp.Count > 0)
                            {
                                foreach (var unitProg in admUp)
                                {
                                    ddlProgram.Items.Add(new ListItem(unitProg.ProgramName, unitProg.ProgramID.ToString()));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void LoadProgram1()
        {
            try
            {
                int acaCalId = Convert.ToInt32(ddlSessionAddUpdate.SelectedValue);
                int facultyId = Convert.ToInt32(ddlFacultyAddUpdate.SelectedValue);

                List<DAL.AdmissionUnitProgram> admUp = new List<DAL.AdmissionUnitProgram>();
                List<DAL.AdmissionSetup> admSetup = new List<DAL.AdmissionSetup>();


                ddlProgramAddUpdate.Items.Clear();
                ddlProgramAddUpdate.Items.Add(new ListItem("-All-", "0"));
                ddlProgramAddUpdate.AppendDataBoundItems = true;


                using (var db = new OfficeDataManager())
                {
                    if (facultyId != 0)
                    {
                        var setUp = db.AdmissionDB.AdmissionSetups.Where(c => c.ID == facultyId).FirstOrDefault();
                        admUp = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == setUp.AdmissionUnitID && c.IsActive == true && c.EducationCategoryID == 4).ToList();

                        if (admUp != null && admUp.Count > 0)
                        {
                            foreach (var item in admUp)
                            {
                                ddlProgramAddUpdate.Items.Add(new ListItem(item.ProgramName, item.ProgramID.ToString()));
                            }
                        }
                    }
                    else
                    {
                        admSetup = db.GetAllAdmissionSetup_AD();
                        admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive==true).ToList();  // only for Bachelors

                        foreach (var sUps in admSetup)
                        {
                            admUp = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == sUps.AdmissionUnitID && c.IsActive == true && c.EducationCategoryID == 4).ToList();

                            if (admUp != null && admUp.Count > 0)
                            {
                                foreach (var unitProg in admUp)
                                {
                                    ddlProgramAddUpdate.Items.Add(new ListItem(unitProg.ProgramName, unitProg.ProgramID.ToString()));
                                }
                            }
                        }
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
            int programId = Convert.ToInt32(ddlProgram.SelectedValue);

            if (acaCalId == -1)
                acaCalId = 0;
            if (facultyId == -1)
                facultyId = 0;
            if (programId == -1)
                programId = 0;


            LoadTotalSeatandMeritSeatList(acaCalId, facultyId, programId);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ddlFacultyAddUpdate.Items.Clear();
                ddlProgramAddUpdate.Items.Clear();
                ddlFacultyAddUpdate.Items.Add(new ListItem("-Select-", "-1"));
                ddlProgramAddUpdate.Items.Add(new ListItem("-Select-", "-1"));

                ddlProgramAddUpdate.Enabled = true;
                ddlFacultyAddUpdate.Enabled = true;
                ddlSessionAddUpdate.Enabled = true;

                txtMeritSeat.Value = string.Empty;
                txtTotalSeat.Value = string.Empty;

                ddlSessionAddUpdate.SelectedValue = "-1";

                btnAddUpdate.Text = "Save";

                pnlAddUpdate.Visible = true;
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void btnAddUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int AcaCalId = Convert.ToInt32(ddlSessionAddUpdate.SelectedValue);
                int FacultyId = Convert.ToInt32(ddlFacultyAddUpdate.SelectedValue);
                int ProgramId = Convert.ToInt32(ddlProgramAddUpdate.SelectedValue);
                int MeritSeat = Convert.ToInt32(txtMeritSeat.Value);
                int TotalSeat = Convert.ToInt32(txtTotalSeat.Value);

                string btnTxt = btnAddUpdate.Text;

                DAL.ProgramWiseTotalSeat NewObj = new DAL.ProgramWiseTotalSeat();

                if (AcaCalId == -1)
                {
                    showAlert("Please Select Session");
                    return;
                }
                if (TotalSeat < MeritSeat)
                {
                    showAlert("Merit Seat Exceeded than Total Seat");
                    return;
                }
                using (var db = new OfficeDataManager())
                {
                    DAL.ProgramWiseTotalSeat existingObj = db.AdmissionDB.ProgramWiseTotalSeats.Where(x => x.AcaCalId == AcaCalId && x.AdmissionSetupId == FacultyId && x.ProgramId == ProgramId).FirstOrDefault();

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
                                if (ProgramId == 0)
                                {
                                    foreach (ListItem item1 in ddlFacultyAddUpdate.Items)
                                    {
                                        if (item1.Value != "0")
                                        {
                                            int adSetupId = Convert.ToInt32(item1.Value);
                                            var setUp = db.AdmissionDB.AdmissionSetups.Where(c => c.ID == adSetupId).FirstOrDefault();
                                            var admUp = db.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == setUp.AdmissionUnitID && c.IsActive == true && c.EducationCategoryID == 4).ToList();
                                            foreach (var item2 in admUp)
                                            {
                                                NewObj.AcaCalId = AcaCalId;
                                                NewObj.AdmissionSetupId = Convert.ToInt32(item1.Value);
                                                NewObj.ProgramId = item2.ProgramID;
                                                NewObj.TotalSeat = TotalSeat;
                                                NewObj.MeritSeat = MeritSeat;
                                                NewObj.CreatedBy = Convert.ToInt32(uId);
                                                NewObj.CreatedDate = DateTime.Now;

                                                db.Insert<DAL.ProgramWiseTotalSeat>(NewObj);

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
                                            NewObj.ProgramId = ProgramId;
                                            NewObj.TotalSeat = TotalSeat;
                                            NewObj.MeritSeat = MeritSeat;
                                            NewObj.CreatedBy = Convert.ToInt32(uId);
                                            NewObj.CreatedDate = DateTime.Now;


                                            db.Insert<DAL.ProgramWiseTotalSeat>(NewObj);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ProgramId == 0)
                                {
                                    foreach (ListItem item in ddlProgramAddUpdate.Items)
                                    {
                                        if (item.Value != "0")
                                        {
                                            NewObj.AcaCalId = AcaCalId;
                                            NewObj.AdmissionSetupId = FacultyId;
                                            NewObj.ProgramId = Convert.ToInt32(item.Value); ;
                                            NewObj.TotalSeat = TotalSeat;
                                            NewObj.MeritSeat = MeritSeat;
                                            NewObj.CreatedBy = Convert.ToInt32(uId);
                                            NewObj.CreatedDate = DateTime.Now;

                                            db.Insert<DAL.ProgramWiseTotalSeat>(NewObj);
                                        }
                                    }
                                }
                                else
                                {
                                    NewObj.AcaCalId = AcaCalId;
                                    NewObj.AdmissionSetupId = FacultyId;
                                    NewObj.ProgramId = ProgramId;
                                    NewObj.TotalSeat = TotalSeat;
                                    NewObj.MeritSeat = MeritSeat;
                                    NewObj.CreatedBy = Convert.ToInt32(uId);
                                    NewObj.CreatedDate = DateTime.Now;

                                    db.Insert<DAL.ProgramWiseTotalSeat>(NewObj);
                                }
                            }
                        }

                        showAlert("Seats Saved");
                    }
                    else
                    {
                        //update code here

                        if (existingObj != null)
                        {
                            existingObj.AcaCalId = AcaCalId;
                            existingObj.AdmissionSetupId = FacultyId;
                            existingObj.ProgramId = ProgramId;
                            existingObj.TotalSeat = TotalSeat;
                            existingObj.MeritSeat = MeritSeat;
                            //existingObj.CreatedBy = Convert.ToInt32(uId);
                            //existingObj.CreatedDate = DateTime.Now;
                            existingObj.ModifiedBy = Convert.ToInt32(uId);
                            existingObj.ModifiedDate = DateTime.Now;

                            db.Update<DAL.ProgramWiseTotalSeat>(existingObj);
                            showAlert("Seats Updated");
                            btnAddUpdate.Text = "Save";
                        }
                    }
                }
                LoadTotalSeatandMeritSeatList(0, 0, 0);
                pnlAddUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void ddlFacultyAddUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProgram1();
        }

        protected void LoadTotalSeatandMeritSeatList(int acaCalId, int facultyId, int programId)
        {
            try
            {
                List<DAL.ProgramWiseTotalSeat> list = new List<DAL.ProgramWiseTotalSeat>();
                using (var db = new GeneralDataManager())
                {
                    if (acaCalId != 0 && facultyId != 0 && programId != 0)
                    {
                        list = db.AdmissionDB.ProgramWiseTotalSeats.Where(c => c.AcaCalId == acaCalId && c.AdmissionSetupId == facultyId && c.ProgramId == programId).ToList();
                    }
                    else if (acaCalId != 0 && facultyId != 0)
                    {
                        list = db.AdmissionDB.ProgramWiseTotalSeats.Where(c => c.AcaCalId == acaCalId && c.AdmissionSetupId == facultyId).ToList();
                    }
                    else if (acaCalId != 0)
                    {
                        list = db.AdmissionDB.ProgramWiseTotalSeats.Where(c => c.AcaCalId == acaCalId).ToList();
                    }
                    else
                    {
                        list = db.AdmissionDB.ProgramWiseTotalSeats.ToList();
                    }


                    if (list != null && list.Count > 0)
                    {
                        lvTotalSeatsetup.DataSource = list.OrderByDescending(c => c.AcaCalId).ThenBy(c => c.AdmissionSetupId);
                    }
                    else
                    {
                        lvTotalSeatsetup.DataSource = null;
                    }

                }

                lvTotalSeatsetup.DataBind();
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void lvTotalSeatsetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                    DAL.ProgramWiseTotalSeat progTotal = (DAL.ProgramWiseTotalSeat)((ListViewDataItem)(e.Item)).DataItem;

                    Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                    Label lblSession = (Label)currentItem.FindControl("lblSession");
                    Label lblFaculty = (Label)currentItem.FindControl("lblFaculty");
                    Label lblProgram = (Label)currentItem.FindControl("lblProgram");
                    Label lblTotalSeat = (Label)currentItem.FindControl("lblTotalSeat");
                    Label lblMeritSeat = (Label)currentItem.FindControl("lblMeritSeat");
                    Label lblRemaining = (Label)currentItem.FindControl("lblRemaining");

                    LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                    LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                    lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                    lblTotalSeat.Text = progTotal.TotalSeat.ToString();
                    lblMeritSeat.Text = progTotal.MeritSeat.ToString();
                    lblRemaining.Text = (progTotal.TotalSeat - progTotal.MeritSeat).ToString();

                    using (var dbSession = new GeneralDataManager())
                    {
                        var session = dbSession.AdmissionDB.SPAcademicCalendarGetAll().Where(c => c.AcademicCalenderID == progTotal.AcaCalId).FirstOrDefault();
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
                        var faculty = dbFaculty.AdmissionDB.AdmissionSetups.Where(c => c.ID == progTotal.AdmissionSetupId && c.AcaCalID == progTotal.AcaCalId).FirstOrDefault();
                        if (faculty != null)
                        {
                            if (faculty.AcaCalID > 0)
                            {
                                lblFaculty.Text = faculty.AdmissionUnit.UnitName;
                            }
                        }
                    }

                    using (var dbProgram = new OfficeDataManager())
                    {
                        var admsetup = dbProgram.AdmissionDB.AdmissionSetups.Where(c => c.ID == progTotal.AdmissionSetupId).FirstOrDefault();

                        if (admsetup != null)
                        {
                            var admPU = dbProgram.AdmissionDB.AdmissionUnitPrograms.Where(c => c.AdmissionUnitID == admsetup.AdmissionUnitID && c.EducationCategoryID == 4 && c.ProgramID == progTotal.ProgramId && c.IsActive == true).FirstOrDefault();

                            if (admPU != null)
                            {
                                lblProgram.Text = admPU.ShortCode;
                            }
                        }
                    }

                    lnkEdit.CommandName = "Update";
                    lnkEdit.CommandArgument = progTotal.Id.ToString();

                    lnkDelete.CommandName = "Delete";
                    lnkDelete.CommandArgument = progTotal.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void lvTotalSeatsetup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    using (var db = new OfficeDataManager())
                    {
                        var ProgramTotalMeritSeatObj = db.AdmissionDB.ProgramWiseTotalSeats.Find(Convert.ToInt64(e.CommandArgument));
                        int pid= Convert.ToInt32(e.CommandArgument);
                        var QuotaMeritSeat = db.AdmissionDB.ProgramandQuotaWiseSeats.Where(x => x.ProgramWiseTotalSeatId == pid).ToList();
                        if (QuotaMeritSeat != null && QuotaMeritSeat.Count > 0)
                        {
                            showAlert("Can not Delete");
                            return;
                        }
                        if (ProgramTotalMeritSeatObj != null)
                        {
                            db.Delete(ProgramTotalMeritSeatObj);
                        }
                        showAlert("Seat has been deleted");
                    }
                    LoadTotalSeatandMeritSeatList(0, 0, 0);
                }
                else if (e.CommandName == "Update")
                {
                    pnlAddUpdate.Visible = true;
                    btnAddUpdate.Text = "Update";

                    using (var db = new OfficeDataManager())
                    {
                        var ProgramTotalMeritSeatObj = db.AdmissionDB.ProgramWiseTotalSeats.Find(Convert.ToInt32(e.CommandArgument));
                        if (ProgramTotalMeritSeatObj != null)
                        {
                            ddlSessionAddUpdate.SelectedValue = ProgramTotalMeritSeatObj.AcaCalId.ToString();
                            ddlSessionAddUpdate_SelectedIndexChanged(this, EventArgs.Empty);
                            ddlFacultyAddUpdate.SelectedValue = ProgramTotalMeritSeatObj.AdmissionSetupId.ToString();
                            LoadProgram1();
                            ddlProgramAddUpdate.SelectedValue = ProgramTotalMeritSeatObj.ProgramId.ToString();
                            txtMeritSeat.Value = ProgramTotalMeritSeatObj.MeritSeat.ToString();
                            txtTotalSeat.Value = ProgramTotalMeritSeatObj.TotalSeat.ToString();


                            ddlProgramAddUpdate.Enabled = false;
                            ddlFacultyAddUpdate.Enabled = false;
                            ddlSessionAddUpdate.Enabled = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void lvTotalSeatsetup_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvTotalSeatsetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
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


                    admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive==true).ToList();  // only for Bachelors

                    if (admSetup != null && admSetup.Count > 0)
                    {
                        foreach (var item in admSetup)
                        {
                            ddlFacultyAddUpdate.Items.Add(new ListItem(item.AdmissionUnit.UnitName, item.ID.ToString()));
                        }
                    }
                }

                LoadProgram1();
            }
            catch (Exception ex)
            {
                showAlert(ex.Message);
            }
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvTotalSeatsetup.DataSource = null;
            lvTotalSeatsetup.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlAddUpdate.Visible = false;
        }
    }
}