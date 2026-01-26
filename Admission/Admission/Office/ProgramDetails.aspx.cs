using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class ProgramDetails : System.Web.UI.Page
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
            infoFile = new FileUpload();
            admissionFormFile = new FileUpload();
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
            using(var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
            }
        }

        private void LoadListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.UnitProgramDetail> list = db.GetAllUnitProgramDetail_AD().ToList();
                if (list != null && list.Any())
                {
                    lvProgramDetails.DataSource = list;
                    lblCount.Text = list.Count.ToString();
                }
                else
                {
                    lvProgramDetails.DataSource = null;
                    lblCount.Text = "0";
                }
                lvProgramDetails.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void lvProgramDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        protected void lvProgramDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

        protected void lvProgramDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e){ }

        protected void lvProgramDetails_ItemUpdating(object sender, ListViewUpdateEventArgs e){ }
    }
}