using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Admission.Admission.Office
{
    public partial class ApproveCandidateGrad : PageBase
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
            }
        }

        private void LoadDDL()
        {
            //ddlSortBy.Items.Clear();
            //ddlSortBy.Items.Add(new ListItem("Select", "-1"));
            //ddlSortBy.Items.Add(new ListItem("Secondary", "1"));
            //ddlSortBy.Items.Add(new ListItem("Higher Secondary", "2"));
            //ddlSortBy.Items.Add(new ListItem("Seconday + Higher Secondary", "3"));

            ddlIsApproved.Items.Clear();
            ddlIsApproved.Items.Add(new ListItem("Select", "-1"));
            ddlIsApproved.Items.Add(new ListItem("Yes", "1"));
            ddlIsApproved.Items.Add(new ListItem("No", "0"));

            ddlIsFinalSubmit.Items.Clear();
            ddlIsFinalSubmit.Items.Add(new ListItem("Select", "-1"));
            ddlIsFinalSubmit.Items.Add(new ListItem("Yes", "1"));
            ddlIsFinalSubmit.Items.Add(new ListItem("No", "0"));

            using (var db = new OfficeDataManager())
            {
                //DDLHelper.Bind<DAL.EducationMedium>(ddlMediumType, db.AdmissionDB.EducationMediums.Where(c=>c.IsActive == true).ToList(), "MediumName", "ID", EnumCollection.ListItemType.EducationMedium);
                DDLHelper.Bind<DAL.GroupOrSubject>(ddlSSCGrpSub, db.AdmissionDB.GroupOrSubjects.Where(c => c.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                DDLHelper.Bind<DAL.GroupOrSubject>(ddlHSCGrpSub, db.AdmissionDB.GroupOrSubjects.Where(c => c.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlSchoolProgram, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.Select);

                List<DAL.SPAcademicCalendarGetAll_Result> sessionList = new List<DAL.SPAcademicCalendarGetAll_Result>();
                sessionList = db.AdmissionDB.SPAcademicCalendarGetAll().ToList();
                if (sessionList.Count() > 0)
                {
                    DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, sessionList.OrderByDescending(c => c.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            long selectedAdmissionUnitID = -1; //faculty/program
            long selectedEducationCategoryID = -1;
            int selectedSession = -1; // acaCalId

            selectedAdmissionUnitID = Int64.Parse(ddlSchoolProgram.SelectedValue);
            selectedSession = Int32.Parse(ddlSession.SelectedValue);

            if (selectedAdmissionUnitID > 0)
            {
                using (var db = new OfficeDataManager())
                {
                    selectedEducationCategoryID = db.GetEducationCategoryByAdmissionUnitID_ND(selectedAdmissionUnitID);
                }
            }

            if (selectedEducationCategoryID == 6) //6 == Masters/Graduate
            {
                LoadGraduateGrid(selectedAdmissionUnitID, selectedSession, selectedEducationCategoryID);
            }
        }

        private void LoadGraduateGrid(long selectedAdmissionUnitID, int acaCalId, long selectedEducationCategoryID)
        {
            graduatePanelForGridView.Visible = true;
            graduatePanelForGridView.Enabled = true;
            btnApprovePG.Visible = true;

            lblType.Text = "Approve(Graduate) ";

            decimal sscGpa = 0.00M;
            decimal hscGpa = 0.00M;
            decimal sscMarks = 0.00M;
            decimal hscMarks = 0.00M;

            if (!string.IsNullOrEmpty(txtSSCGpa.Text.Trim()))
            {
                sscGpa = Convert.ToDecimal(txtSSCGpa.Text);
            }
            if (!string.IsNullOrEmpty(txtHSCGpa.Text.Trim()))
            {
                hscGpa = Convert.ToDecimal(txtHSCGpa.Text);
            }
            if (!string.IsNullOrEmpty(txtSSCMarks.Text))
            {
                sscMarks = Convert.ToDecimal(txtSSCMarks.Text);
            }
            if (!string.IsNullOrEmpty(txtHSCMarks.Text))
            {
                hscMarks = Convert.ToDecimal(txtHSCMarks.Text);
            }

            decimal undGpa = 0.00M;
            decimal gradGpa = 0.00M;

            if (!string.IsNullOrEmpty(txtUndergradGpa.Text))
            {
                undGpa = Convert.ToDecimal(txtUndergradGpa.Text);
            }
            if (!string.IsNullOrEmpty(txtGraduateGpa.Text))
            {
                gradGpa = Convert.ToDecimal(txtGraduateGpa.Text);
            }

            int? sscGroupSub = -1;
            int? hscGroupSub = -1;

            if (ddlSSCGrpSub.SelectedValue == "-1")
            {
                sscGroupSub = null;
            }
            else
            {
                sscGroupSub = Convert.ToInt32(ddlSSCGrpSub.SelectedValue);
            }

            if (ddlHSCGrpSub.SelectedValue == "-1")
            {
                hscGroupSub = null;
            }
            else
            {
                hscGroupSub = Convert.ToInt32(ddlSSCGrpSub.SelectedValue);
            }

            bool? isApproved = null;
            bool? isFinalSubmit = null;

            if (ddlIsApproved.SelectedValue == "-1")
            {
                isApproved = null;
            }
            else if (ddlIsApproved.SelectedValue == "0")
            {
                isApproved = false;
            }
            else if (ddlIsApproved.SelectedValue == "1")
            {
                isApproved = true;
            }

            if (ddlIsFinalSubmit.SelectedValue == "-1")
            {
                isFinalSubmit = null;
            }
            else if (ddlIsFinalSubmit.SelectedValue == "0")
            {
                isFinalSubmit = false;
            }
            else if (ddlIsFinalSubmit.SelectedValue == "1")
            {
                isFinalSubmit = true;
            }

            try
            {
                List<DAL.SPGetCandidateListForApproval_Result> list = null;
                using (var db = new CandidateDataManager())
                {
                    list = db.AdmissionDB.SPGetCandidateListForApproval(selectedAdmissionUnitID, acaCalId).ToList();
                }
                if (list.Count() > 0)
                {
                    if (sscGpa > 0.00M && hscGpa == 0.00M)
                    {
                        list = list
                            .Where(c => (c.sscResult == "1st Division" || Convert.ToDecimal(c.sscResult) >= sscGpa) &&
                            (c.hscResult == "2nd Division" || Convert.ToDecimal(hscGpa) >= hscGpa)).ToList();
                    }
                    else if (sscGpa == 0.00M && hscGpa > 0.00M)
                    {
                        list = list.Where(c => (c.sscResult == "1st Division" || Convert.ToDecimal(c.sscResult) >= sscGpa) &&
                            (c.hscResult == "2nd Division" || Convert.ToDecimal(hscGpa) >= hscGpa)).ToList();
                    }
                    else if (sscGpa > 0.00M && hscGpa > 0.00M)
                    {
                        list = list.Where(c => (c.sscResult == "1st Division" || Convert.ToDecimal(c.sscResult) >= sscGpa) &&
                            (c.hscResult == "2nd Division" || Convert.ToDecimal(hscGpa) >= hscGpa)).ToList();
                    }

                    if (sscMarks >= 0.00M && hscMarks >= 0.00M)
                    {
                        list = list.Where(c => Convert.ToDecimal(c.sscMarks) >= sscMarks && Convert.ToDecimal(hscMarks) >= hscMarks).ToList();
                    }

                    if (undGpa >= 0.00M & gradGpa >= 0.00M)
                    {
                        list = list
                            .Where(c => (c.undResult == "1st Division" || c.undResult == "2nd Division" || c.undResult == "CGPA N/A" || Convert.ToDecimal(c.undResult) >= undGpa) &&
                                (c.grdResult == "1st Division" || c.grdResult == "2nd Division" || c.grdResult == "CGPA N/A" || Convert.ToDecimal(c.grdResult) >= gradGpa))
                            .ToList();
                    }

                    if (isApproved != null)
                    {
                        list = list.Where(c => c.IsApproved == isApproved).ToList();
                    }

                    if (isFinalSubmit != null)
                    {
                        list = list.Where(c => c.IsFinalSubmit == isFinalSubmit).ToList();
                    }

                    if (ddlSSCGrpSub.SelectedValue == "-1" && ddlHSCGrpSub.SelectedValue == "-1")
                    {
                        gvGraduateApplicantInfo.DataSource = list.ToList();
                        lblCount.Text = list.Count().ToString();
                    }
                    else if (ddlSSCGrpSub.SelectedValue != "-1" && ddlHSCGrpSub.SelectedValue == "-1")
                    {
                        List<DAL.SPGetCandidateListForApproval_Result> _temp1 = list
                            .Where(c => c.sscGroupOrSubjectID == Convert.ToInt32(ddlSSCGrpSub.SelectedValue)).ToList();
                        gvGraduateApplicantInfo.DataSource = _temp1.ToList();
                        lblCount.Text = _temp1.Count().ToString();
                    }
                    else if (ddlSSCGrpSub.SelectedValue == "-1" && ddlHSCGrpSub.SelectedValue != "-1")
                    {
                        List<DAL.SPGetCandidateListForApproval_Result> _temp2 = list
                            .Where(c => c.hscGroupOrSubjectID == Convert.ToInt32(ddlHSCGrpSub.SelectedValue)).ToList();
                        gvGraduateApplicantInfo.DataSource = _temp2.ToList();
                        lblCount.Text = _temp2.Count().ToString();
                    }
                    else if (ddlSSCGrpSub.SelectedValue != "-1" && ddlHSCGrpSub.SelectedValue != "-1")
                    {
                        List<DAL.SPGetCandidateListForApproval_Result> _temp3 = list
                            .Where(c => c.sscGroupOrSubjectID == Convert.ToInt32(ddlSSCGrpSub.SelectedValue) &&
                                c.hscGroupOrSubjectID == Convert.ToInt32(ddlHSCGrpSub.SelectedValue)).ToList();
                        gvGraduateApplicantInfo.DataSource = _temp3.ToList();
                        lblCount.Text = _temp3.Count().ToString();
                    }
                }
                else
                {
                    gvGraduateApplicantInfo.DataSource = null;
                    lblCount.Text = "0";
                }
                gvGraduateApplicantInfo.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading graduate info: " + ex.Message + "; " + ex.InnerException.Message;
                lblMessage.ForeColor = Color.Crimson;
            }
        }

        protected void gvGraduateApplicantInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //----
                Label lblPGQuota = (Label)e.Row.FindControl("lblPGQuota");
                if (lblPGQuota.Text == "N/A")
                {
                    //lblPGQuota.ForeColor = Color.Crimson;
                    lblPGQuota.Font.Bold = true;
                    e.Row.ForeColor = Color.Crimson;
                }
                //----
                Label lblPGSSCGpa = (Label)e.Row.FindControl("lblPGSSCGpa");

                if (lblPGSSCGpa.Text == "CGPA N/A")
                {
                    //lblPGSSCGpa.ForeColor = Color.Crimson;
                    lblPGSSCGpa.Font.Bold = true;
                    e.Row.ForeColor = Color.Crimson;
                }

                Label lblPGHSCGpa = (Label)e.Row.FindControl("lblPGHSCGpa");

                if (lblPGHSCGpa.Text == "CGPA N/A")
                {
                    //lblPGHSCGpa.ForeColor = Color.Crimson;
                    lblPGHSCGpa.Font.Bold = true;
                    e.Row.ForeColor = Color.Crimson;
                }

                Label lblPGUndergradGpa = (Label)e.Row.FindControl("lblPGUndergradGpa");

                if (lblPGUndergradGpa.Text.Equals("CGPA N/A") || lblPGUndergradGpa.Text.Equals(" - "))
                {
                    //lblPGUndergradGpa.ForeColor = Color.Crimson;
                    lblPGUndergradGpa.Font.Bold = true;
                    e.Row.ForeColor = Color.Crimson;
                }
                //----
                CheckBox appCheck = (CheckBox)e.Row.FindControl("chkPGApprove");

                Label lblCandidateID = (Label)e.Row.FindControl("lblPGCandidateId");
                Label lblAdmUnitId = (Label)e.Row.FindControl("lblPGAdmUnitId");
                Label lblAcaCalId = (Label)e.Row.FindControl("lblPGAcaCalId");

                long? candidateId = Convert.ToInt64(lblCandidateID.Text);
                long? admUnitId = Convert.ToInt64(lblAdmUnitId.Text);
                int? acaCalId = Convert.ToInt32(lblAcaCalId.Text);

                DAL.ApprovedList obj = null;
                if (candidateId > 0 && admUnitId > 0 && acaCalId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        obj = db.AdmissionDB.ApprovedLists
                            .Where(c => c.CandidateID == candidateId && c.AdmissionUnitID == admUnitId && c.AcaCalID == acaCalId)
                            .FirstOrDefault();
                    }
                }

                if (obj != null) //obj exist, but isApproved could be null or true or false
                {
                    if (obj.IsApproved == null)
                    {
                        appCheck.Checked = false;
                    }
                    else if (obj.IsApproved == false)
                    {
                        appCheck.Checked = false;
                    }
                    else if (obj.IsApproved == true)
                    {
                        appCheck.Checked = true;
                    }
                }
                else //obj does not exist.
                {
                    appCheck.Checked = false;
                }
                //----

            }
        }

        protected void chkPGSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                chk.Text = "<b> Unselect All</b>";
            }
            else
            {
                chk.Text = "<b> Select All</b>";
                chk.Font.Bold = true;
            }
            foreach (GridViewRow row in gvGraduateApplicantInfo.Rows)
            {
                CheckBox ckBox = (CheckBox)row.FindControl("chkPGApprove");
                ckBox.Checked = chk.Checked;
            }
        }

        protected void btnApprovePG_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            int numberSaved = 0;
            int numberUpdated = 0;
            foreach (GridViewRow row in gvGraduateApplicantInfo.Rows)
            {
                CheckBox isApprove = (CheckBox)row.FindControl("chkPGApprove");
                try
                {
                    Label lblCandidateId = (Label)row.FindControl("lblPGCandidateId");
                    Label lblAdmUnitID = (Label)row.FindControl("lblPGAdmUnitId");
                    Label lblAcaCalId = (Label)row.FindControl("lblPGAcaCalId");
                    Label lblAdmSetupID = (Label)row.FindControl("lblPGAdmSetupId");
                    Label lblPGFormSl = (Label)row.FindControl("lblPGFormSl");
                    Label lblPGPaymentID = (Label)row.FindControl("lblPGPaymentID");
                    //Label lblCandidateId = (Label)row.FindControl("");
                    //Label lblCandidateId = (Label)row.FindControl("");

                    DAL.ApprovedList approveObj = new DAL.ApprovedList();
                    approveObj.CandidateID = Convert.ToInt64(lblCandidateId.Text);
                    approveObj.FormSL = Convert.ToInt64(lblPGFormSl.Text);
                    approveObj.AdmissionUnitID = Convert.ToInt32(lblAdmUnitID.Text);
                    approveObj.ProgramID = null;
                    approveObj.IsApproved = isApprove.Checked;
                    approveObj.PaymentID = Convert.ToInt64(lblPGPaymentID.Text);
                    approveObj.AcaCalID = Convert.ToInt32(lblAcaCalId.Text);
                    approveObj.AdmissionSetupID = Convert.ToInt64(lblAdmSetupID.Text);
                    approveObj.CreatedBy = uId;
                    approveObj.DateCreated = DateTime.Now;
                    approveObj.ModifiedBy = null;
                    approveObj.DateModified = null;

                    try
                    {
                        DAL.ApprovedList objExist = null;
                        using (var db = new CandidateDataManager())
                        {
                            objExist = db.AdmissionDB.ApprovedLists
                                .Where(c => c.CandidateID == approveObj.CandidateID &&
                                    c.PaymentID == approveObj.PaymentID &&
                                    c.FormSL == approveObj.FormSL &&
                                    c.AdmissionSetupID == approveObj.AdmissionSetupID &&
                                    c.AdmissionUnitID == approveObj.AdmissionUnitID)
                                .FirstOrDefault();
                        }
                        if (objExist != null)  //candidate already exist in approve list..then update the object.
                        {
                            objExist.IsApproved = isApprove.Checked;
                            objExist.ModifiedBy = uId;
                            objExist.DateModified = DateTime.Now;

                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.ApprovedList>(objExist);
                                numberUpdated++;
                            }
                        }
                        else  //candidate does not exist in the approve list...insert new object.
                        {
                            if (isApprove.Checked)
                            {
                                using (var dbInsert = new CandidateDataManager())
                                {
                                    dbInsert.Insert<DAL.ApprovedList>(approveObj);
                                    numberSaved++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error Saving/Updating Masters Candidate. " + ex.Message + "; " + ex.InnerException.Message;
                        lblMessage.ForeColor = Color.Crimson;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Saving/Updating Masters Candidate.(1) " + ex.Message + "; " + ex.InnerException.Message;
                    lblMessage.ForeColor = Color.Crimson;
                    return;
                }

                lblMessage.Text = "Success. Number Saved: " + numberSaved + "; Number Updated: " + numberUpdated;
                lblMessage.ForeColor = Color.Green;
                lblMessage.Focus();

            }
        }
    }
}