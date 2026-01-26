using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class ProgramInformation : System.Web.UI.Page
    {

        long admissionUnitId = -1;
        long admissionSetupId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["asi"]) && !string.IsNullOrEmpty(Request.QueryString["aui"]))
                {
                    admissionSetupId = Int64.Parse(Request.QueryString["asi"].ToString());
                    admissionUnitId = Int64.Parse(Request.QueryString["aui"].ToString());
                    if (admissionSetupId > 0 && admissionUnitId > 0)
                    {
                        //LoadProgramInformation(admissionUnitId);
                        LoadProgramInformationV2(admissionSetupId);
                    }
                }
                else
                {
                    Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=danger", false);
                }
            }
        }

        private void LoadProgramInformationV2(long admissionSetupId)
        {
            using (var db = new OfficeDataManager())
            {
                DAL.AdmissionSetup admissionSetupObj = db.GetAdmissionSetupByID_ND(admissionSetupId);
                if (admissionSetupObj != null)
                {
                    List<DAL.AdmissionUnitProgram> admUnitProgList = db.GetAllAdmissionUnitProgramByAdmUnitIDEducCatID_ND(admissionSetupObj.AdmissionUnitID, admissionSetupObj.EducationCategoryID);
                    admUnitProgList = admUnitProgList.Where(x => x.IsActive == true).ToList();

                    #region Filter Out Regular And Certificate

                    if (Session["certiEducationCategory"] != null)
                    {
                        try
                        {
                            int certiEducationCategory = Convert.ToInt32(Session["certiEducationCategory"]);
                            if (certiEducationCategory == 0) //Certificate Education Category ID is 0
                            {

                                admUnitProgList = admUnitProgList.Where(x => x.ProgramID == 65 || x.ProgramID == 66).ToList();
                            }
                            else
                            {
                                admUnitProgList = admUnitProgList.Where(x => x.ProgramID != 65 && x.ProgramID != 66).ToList();
                            }
                        }
                        catch (Exception)
                        {
                            // Handle exception if necessary
                            admUnitProgList = admUnitProgList.Where(x => x.ProgramID != 65 && x.ProgramID != 66).ToList();
                        }
                    }
                    else
                        admUnitProgList = admUnitProgList.Where(x => x.ProgramID != 65 && x.ProgramID != 66).ToList();

                    #endregion

                    if (admUnitProgList.Any())
                    {
                        btnApply.Visible = true;
                        lvPrograms.DataSource = admUnitProgList.ToList();
                    }
                    else
                    {
                        btnApply.Visible = false;
                        lvPrograms.DataSource = null;
                    }
                    lvPrograms.DataBind();
                }
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["asi"]) && !string.IsNullOrEmpty(Request.QueryString["aui"]))
            {
                admissionSetupId = Int64.Parse(Request.QueryString["asi"].ToString());
                admissionUnitId = Int64.Parse(Request.QueryString["aui"].ToString());
            }

            if (admissionSetupId > 0 && admissionUnitId > 0)
            {
                using (var db = new OfficeDataManager())
                {
                    DAL.AdmissionSetup admissionSetup = db.AdmissionDB.AdmissionSetups.Find(admissionSetupId);
                    if (admissionSetup.ID > 0 && admissionSetup.IsActive == true)
                    {
                        DAL.AdmissionUnit admissionUnit = db.AdmissionDB.AdmissionUnits.Find(admissionUnitId);
                        if (admissionUnit.ID > 0 && admissionUnit.IsActive == true)
                        {
                            //Session["pi_asi"] = admissionSetup.ID;
                            //Session["pi_aui"] = admissionUnit;
                            Response.Redirect("~/Admission/Candidate/PurchaseForm.aspx?asi=" + admissionSetup.ID + "&aui=" + admissionUnit.ID, false);
                            //Response.Redirect("~/Admission/Candidate/PurchaseForm.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Admission/Message.aspx?message=Something went wrong. Please contact administrator.&type=danger", false);
                    }
                }
            }
            else
            {
                //TODO illegal page request
            }
        }

        protected void lvPrograms_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.AdmissionUnitProgram admUnitProg = (DAL.AdmissionUnitProgram)((ListViewDataItem)(e.Item)).DataItem;

                //Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblPrograms = (Label)currentItem.FindControl("lblPrograms");

                lblPrograms.Text = admUnitProg.ProgramName;
            }
        }

    }
}