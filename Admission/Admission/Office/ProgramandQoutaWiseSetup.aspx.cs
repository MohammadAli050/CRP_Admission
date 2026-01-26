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
    public partial class ProgramandQoutaWiseSetup : PageBase
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

            }

            ddlProgram.Items.Add(new ListItem("-Select-", "-1"));
            ddlFaculty.Items.Add(new ListItem("-Select-", "-1"));
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

                using (var db = new OfficeDataManager())
                {
                    admSetup = db.GetAllAdmissionSetup_AD();


                    admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive == true).ToList();  // only for Bachelors

                    if (admSetup != null && admSetup.Count > 0)
                    {
                        foreach (var item in admSetup)
                        {
                            ddlFaculty.Items.Add(new ListItem(item.AdmissionUnit.UnitName, item.ID.ToString()));
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
                        admSetup = admSetup.Where(x => x.AcaCalID == acaCalId && x.EducationCategoryID == 4 && x.IsActive == true).ToList();  // only for Bachelors

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

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProgram();
            lvTotalSeatsetup.DataSource = null;
            lvTotalSeatsetup.DataBind();
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
                    else if(acaCalId !=0 && facultyId != 0)
                    {
                        list = db.AdmissionDB.ProgramWiseTotalSeats.Where(c => c.AcaCalId == acaCalId && c.AdmissionSetupId == facultyId).ToList();
                    }
                    else if(acaCalId != 0)
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

                    ListBox lbQouta = (ListBox)currentItem.FindControl("lbQouta");


                    LinkButton lnkAddUpdate = (LinkButton)currentItem.FindControl("lnkAddUpdate");
                    LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                    LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                    lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();

                    lblTotalSeat.Text = progTotal.TotalSeat.ToString();
                    lblMeritSeat.Text = progTotal.MeritSeat.ToString();

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

                    using (var dbQouta = new OfficeDataManager())
                    {
                        var adpQouta = dbQouta.AdmissionDB.ProgramandQuotaWiseSeats.Where(c => c.ProgramWiseTotalSeatId == progTotal.Id).ToList();
                        if (adpQouta != null && adpQouta.Count > 0)
                        {
                            foreach(var item in adpQouta)
                            {
                                var qouta = dbQouta.AdmissionDB.Quotas.Where(c => c.ID == item.QuotaId && c.ID != 7).FirstOrDefault();

                                string listItemTxt = qouta.QuotaName + " : " + item.Seat;

                                lbQouta.Items.Add(listItemTxt);
                            }
                        }

                    }

                    lnkEdit.CommandName = "Update";
                    lnkEdit.CommandArgument = progTotal.Id.ToString();

                    lnkAddUpdate.CommandName = "Qouta Seat";
                    lnkAddUpdate.CommandArgument = progTotal.Id.ToString();

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
                        int progId = Convert.ToInt32(e.CommandArgument);
                        
                        var ProgramTotalMeritSeatObj = db.AdmissionDB.ProgramandQuotaWiseSeats.Where(x=> x.ProgramWiseTotalSeatId == progId).ToList();
                        if (ProgramTotalMeritSeatObj != null)
                        {
                            foreach(var item in ProgramTotalMeritSeatObj)
                            {
                                db.Delete(item);
                            }
                        }
                    }
                    LoadTotalSeatandMeritSeatList(0, 0, 0);
                    showAlert("Quota Seat Deleted");
                }
                else if (e.CommandName == "Update")
                {
                    //btnAddUpdate.Text = "Update";

                    //using (var db = new OfficeDataManager())
                    //{
                    //    var ProgramTotalMeritSeatObj = db.AdmissionDB.ProgramWiseTotalSeats.Find(Convert.ToInt32(e.CommandArgument));
                    //    if (ProgramTotalMeritSeatObj != null)
                    //    {
                    //        ddlSessionAddUpdate.SelectedValue = ProgramTotalMeritSeatObj.AcaCalId.ToString();
                    //        ddlFacultyAddUpdate.SelectedValue = ProgramTotalMeritSeatObj.AdmissionSetupId.ToString();
                    //        ddlProgramAddUpdate.SelectedValue = ProgramTotalMeritSeatObj.ProgramId.ToString();
                    //        txtMeritSeat.Value = ProgramTotalMeritSeatObj.MeritSeat.ToString();
                    //        txtTotalSeat.Value = ProgramTotalMeritSeatObj.TotalSeat.ToString();


                    //        ddlProgramAddUpdate.Enabled = false;
                    //        ddlFacultyAddUpdate.Enabled = false;
                    //        ddlSessionAddUpdate.Enabled = false;

                    //    }
                    //}
                }
                else if (e.CommandName == "Qouta Seat")
                {
                    hdnProgramSeatId.Value = e.CommandArgument.ToString();

                    try
                    {
                        using(var db = new OfficeDataManager())
                        {
                            int progId = Convert.ToInt32(e.CommandArgument);

                            txtFreedomFighter.Text = "0";
                            txtSpecialQuota.Text = "0";
                            txtTribal.Text = "0";
                            txtDisable.Text = "0";

                            List<DAL.ProgramandQuotaWiseSeat> quotaSeat = db.AdmissionDB.ProgramandQuotaWiseSeats.Where(x => x.ProgramWiseTotalSeatId == progId).ToList();
                            
                            if (quotaSeat != null && quotaSeat.Count > 0)
                            {
                                foreach(var item in quotaSeat)
                                {
                                    if(item.QuotaId == 2)
                                    {
                                        txtFreedomFighter.Text = item.Seat.ToString();
                                    }
                                    else if(item.QuotaId == 4)
                                    {
                                        txtSpecialQuota.Text = item.Seat.ToString();
                                    }
                                    else if(item.QuotaId == 6)
                                    {
                                        txtTribal.Text = item.Seat.ToString();
                                    }
                                    else if(item.QuotaId == 8)
                                    {
                                        txtDisable.Text = item.Seat.ToString();
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        throw;
                    }
                    ModalPopupExtender2.Show();

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

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void btnAddUpdateQouta_Click(object sender, EventArgs e)
        {
            try
            {
                int programSeatId = Convert.ToInt32(hdnProgramSeatId.Value);

                int ffSeat = Convert.ToInt32(txtFreedomFighter.Text.Trim());
                int sqSeat = Convert.ToInt32(txtSpecialQuota.Text.Trim());
                int dSeat = Convert.ToInt32(txtDisable.Text.Trim());
                int tSeat = Convert.ToInt32(txtTribal.Text.Trim());

                int totalQuotaSeat = ffSeat + sqSeat + dSeat + tSeat;

                using (var db = new OfficeDataManager())
                {

                    DAL.ProgramWiseTotalSeat progSeat = db.AdmissionDB.ProgramWiseTotalSeats.Find(programSeatId);

                    if((progSeat.TotalSeat-progSeat.MeritSeat) < totalQuotaSeat)
                    {
                        showAlert("Seat exceeds total seat");
                        return;
                    }

                    List<DAL.ProgramandQuotaWiseSeat> existingObje = db.AdmissionDB.ProgramandQuotaWiseSeats.Where(c => c.ProgramWiseTotalSeatId == programSeatId).ToList();

                    if (existingObje != null && existingObje.Count > 0)
                    {
                        //DAL.ProgramandQuotaWiseSeat existingObj = new DAL.ProgramandQuotaWiseSeat();
                        //NewObj.ProgramWiseTotalSeatId = programSeatId;
                        //NewObj.CreatedBy = Convert.ToInt32(uId);
                        //NewObj.CreatedDate = DateTime.Now;

                        foreach (var item in existingObje)
                        {
                            if (item.QuotaId == 2)
                            {
                                item.Seat = ffSeat;
                            }
                            else if (item.QuotaId == 4)
                            {
                                item.Seat = sqSeat;
                            }
                            else if (item.QuotaId == 6)
                            {
                                item.Seat = tSeat;
                            }                            
                            else if (item.QuotaId == 8)
                            {
                                item.Seat = dSeat;
                            }

                            item.ModifiedBy = Convert.ToInt32(uId);
                            item.ModifiedDate = DateTime.Now;

                            db.Update<DAL.ProgramandQuotaWiseSeat>(item);
                        }

                        showAlert("Quota Seat Updated");

                    }
                    else
                    {
                        

                        var qoutaType = db.AdmissionDB.Quotas.Where(c => c.IsActive == true && c.ID != 7).ToList();

                        if (qoutaType != null && qoutaType.Count > 0)
                        {
                            foreach (var item in qoutaType)
                            {
                                DAL.ProgramandQuotaWiseSeat NewObj = new DAL.ProgramandQuotaWiseSeat();
                                NewObj.ProgramWiseTotalSeatId = programSeatId;
                                NewObj.CreatedBy = Convert.ToInt32(uId);
                                NewObj.CreatedDate = DateTime.Now;

                                if (item.ID == 2)
                                {
                                    NewObj.QuotaId = item.ID;
                                    NewObj.Seat = ffSeat;
                                }
                                else if (item.ID == 4)
                                {
                                    NewObj.QuotaId = item.ID;
                                    NewObj.Seat = sqSeat;
                                }
                                else if (item.ID == 6)
                                {
                                    NewObj.QuotaId = item.ID;
                                    NewObj.Seat = tSeat;
                                }
                                else if (item.ID == 8)
                                {
                                    NewObj.QuotaId = item.ID;
                                    NewObj.Seat = dSeat;
                                }
                                db.Insert<DAL.ProgramandQuotaWiseSeat>(NewObj);
                            }

                            showAlert("Quota Seat Inserted");
                        }

                    }
                }
                LoadTotalSeatandMeritSeatList(0, 0, 0);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            int facultyId = Convert.ToInt32(ddlFaculty.SelectedValue);
            int programId = Convert.ToInt32(ddlProgram.SelectedValue);


            LoadTotalSeatandMeritSeatList(acaCalId, facultyId, programId);
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvTotalSeatsetup.DataSource = null;
            lvTotalSeatsetup.DataBind();
        }
    }
}