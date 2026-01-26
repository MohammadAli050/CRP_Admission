using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Admission.Admission.Office
{
    public partial class ApproveCandidateV3 : PageBase
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

            ddlIsFinalSubmit.Items.Clear();
            ddlIsFinalSubmit.Items.Add(new ListItem("Select", "-1"));
            ddlIsFinalSubmit.Items.Add(new ListItem("Yes", "1"));
            ddlIsFinalSubmit.Items.Add(new ListItem("No", "2"));

            ddlIsFinalSubmit.SelectedValue = "1";

            ddlIsApproved.Items.Clear();
            ddlIsApproved.Items.Add(new ListItem("Select", "-1"));
            ddlIsApproved.Items.Add(new ListItem("Yes", "1"));
            ddlIsApproved.Items.Add(new ListItem("No", "2"));

            ddlIsApproved.SelectedValue = "2";



            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlFaculty, db.GetAllAdmissionUnit(), "UnitName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.EducationCategory>(ddlEducationCategory, db.AdmissionDB.EducationCategories.Where(a => a.IsActive == true).ToList(), "CategoryName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(a => a.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);

                DDLHelper.Bind<DAL.GroupOrSubject>(ddlSSCGrpSub, db.AdmissionDB.GroupOrSubjects.Where(c => c.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                DDLHelper.Bind<DAL.GroupOrSubject>(ddlHSCGrpSub, db.AdmissionDB.GroupOrSubjects.Where(c => c.IsActive == true).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.GroupOrSubject);
                DDLHelper.Bind<DAL.Quota>(ddlQuota, db.AdmissionDB.Quotas.Where(c => c.IsActive == true).ToList(), "Remarks", "ID", EnumCollection.ListItemType.All);

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



        protected void btnLoad_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long admUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                int eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);

                //int? eduCatId = null;
                decimal? sscGPA = null;
                decimal? hscGPA = null;
                decimal? undergradeCGPA = null;
                decimal? sscHscTotalMarks = null;
                int? sscGroup = null;
                int? hscGroup = null;
                bool? isFinalSubmit = null;
                bool? isApproved = null;
                bool InvalidStatusId = false; /// Map with AttributeBit2 field.For FOE+FST Faculty there are some students who applied without grade checking
                                              /// So, to include them in the list we are using this flag.

                if (admUnitId > 0 && eduCatId > 0 && acaCalId > 0)
                {
                    #region N/A -- Get Education Category
                    //if (Convert.ToInt32(ddlEducationCategory.SelectedValue) > 0)
                    //{
                    //    eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                    //}
                    //else
                    //{
                    //    eduCatId = null;
                    //}
                    #endregion

                    #region SSC/O-Level/Dakhil GPA
                    if (!string.IsNullOrEmpty(txtSSCGpa.Text.Trim()) && Convert.ToDecimal(txtSSCGpa.Text.Trim()) > 0)
                    {
                        sscGPA = Convert.ToDecimal(txtSSCGpa.Text.Trim());
                    }
                    else
                    {
                        sscGPA = null;
                    }
                    #endregion

                    #region HSC/A-Level/Alim GPA
                    if (!string.IsNullOrEmpty(txtHSCGpa.Text.Trim()) && Convert.ToDecimal(txtHSCGpa.Text.Trim()) > 0)
                    {
                        hscGPA = Convert.ToDecimal(txtHSCGpa.Text.Trim());
                    }
                    else
                    {
                        hscGPA = null;
                    }
                    #endregion

                    #region Undergrad GPA
                    if (!string.IsNullOrEmpty(txtUndergradGpa.Text.Trim()) && Convert.ToDecimal(txtUndergradGpa.Text.Trim()) > 0)
                    {
                        undergradeCGPA = Convert.ToDecimal(txtUndergradGpa.Text.Trim());
                    }
                    else
                    {
                        undergradeCGPA = null;
                    }
                    #endregion

                    #region HSC/A-Level/Alim GPA
                    if (!string.IsNullOrEmpty(txtHSCGpa.Text.Trim()) && Convert.ToDecimal(txtHSCGpa.Text.Trim()) > 0)
                    {
                        hscGPA = Convert.ToDecimal(txtHSCGpa.Text.Trim());
                    }
                    else
                    {
                        hscGPA = null;
                    }
                    #endregion

                    #region SSC/O-Level/Dakhil Group/Subject
                    if (Convert.ToInt32(ddlSSCGrpSub.SelectedValue) > 0)
                    {
                        sscGroup = Convert.ToInt32(ddlSSCGrpSub.SelectedValue);
                    }
                    else
                    {
                        sscGroup = null;
                    }
                    #endregion

                    #region HSC/A-Level/Alim Group/Subject
                    if (Convert.ToInt32(ddlHSCGrpSub.SelectedValue) > 0)
                    {
                        hscGroup = Convert.ToInt32(ddlHSCGrpSub.SelectedValue);
                    }
                    else
                    {
                        hscGroup = null;
                    }
                    #endregion

                    #region Final Submit
                    if (Convert.ToInt32(ddlIsFinalSubmit.SelectedValue) > 0)
                    {
                        if (Convert.ToInt32(ddlIsFinalSubmit.SelectedValue) == 1)
                        {
                            isFinalSubmit = true;
                        }
                        else
                        {
                            isFinalSubmit = false;
                        }
                    }
                    else
                    {
                        isFinalSubmit = null;
                    }
                    #endregion

                    #region Approved
                    if (Convert.ToInt32(ddlIsApproved.SelectedValue) > 0)
                    {
                        if (Convert.ToInt32(ddlIsApproved.SelectedValue) == 1)
                        {
                            isApproved = true;
                        }
                        else
                        {
                            isApproved = false;
                        }
                    }
                    else
                    {
                        isApproved = null;
                    }
                    #endregion

                    #region Status

                    if (Convert.ToInt32(ddlStatus.SelectedValue) > 0)
                    {
                        if (Convert.ToInt32(ddlStatus.SelectedValue) == 1)
                        {
                            InvalidStatusId = true;
                        }
                        else
                        {
                            InvalidStatusId = false;
                        }

                    }

                    #endregion

                    string QuotaName = "";
                    int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);
                    if (quotaId != -1)
                    {
                        QuotaName = ddlQuota.SelectedItem.ToString();
                    }

                    List<DAL.SPGetCandidateListForApprovalV3New_Result> list = null;
                    using (var db = new GeneralDataManager())
                    {
                        list = db.AdmissionDB.SPGetCandidateListForApprovalV3New(acaCalId,
                                                                                admUnitId,
                                                                                eduCatId,
                                                                                isFinalSubmit,
                                                                                isApproved,
                                                                                sscGPA,
                                                                                hscGPA,
                                                                                undergradeCGPA,
                                                                                null,
                                                                                sscHscTotalMarks,
                                                                                sscGroup,
                                                                                hscGroup,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                "",
                                                                                InvalidStatusId,
                                                                                QuotaName
                                                                                ).ToList();
                    }


                    if (list != null && list.Count > 0)
                    {

                        if (eduCatId == 4)
                        {

                            btnApproveUG.Visible = true;
                            btnApprovePG.Visible = false;

                            panelBachelor.Visible = true;
                            lvBachelor.DataSource = list.OrderByDescending(c => c.SSC_HSC_Total_GPA).ToList();
                            lvBachelor.DataBind();
                            lblCountBachelor.Text = list.Count().ToString();

                            panelMasters.Visible = false;
                            lvMasters.DataSource = null;
                            lvMasters.DataBind();
                            lblCountMasters.Text = "0";


                        }
                        else
                        {
                            btnApproveUG.Visible = false;
                            btnApprovePG.Visible = true;

                            panelBachelor.Visible = false;
                            lvBachelor.DataSource = null;
                            lvBachelor.DataBind();
                            lblCountBachelor.Text = "0";


                            panelMasters.Visible = true;
                            lvMasters.DataSource = list.OrderByDescending(c => c.SSC_HSC_Total_GPA).ToList();
                            lvMasters.DataBind();
                            lblCountMasters.Text = list.Count().ToString();

                        }
                    }
                    else
                    {
                        btnApproveUG.Visible = false;
                        btnApprovePG.Visible = false;

                        panelBachelor.Visible = false;
                        lvBachelor.DataSource = null;
                        lvBachelor.DataBind();
                        lblCountBachelor.Text = "0";

                        panelMasters.Visible = false;
                        lvMasters.DataSource = null;
                        lvMasters.DataBind();
                        lblCountMasters.Text = "0";
                    }

                }
                else
                {
                    MessageView("Please Select Faculty, Education Category and Session!", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString() + " Error1", "fail");
            }

        }

        #region Bachelor List View

        protected void lvBachelor_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetCandidateListForApprovalV3New_Result obj = (DAL.SPGetCandidateListForApprovalV3New_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblMobile = (Label)currentItem.FindControl("lblMobile");
                Label lblEmail = (Label)currentItem.FindControl("lblEmail");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
                Label lblFaculty = (Label)currentItem.FindControl("lblFaculty");

                Label lblPhotoSignature = (Label)currentItem.FindControl("lblPhotoSignature");

                Label lblSSCOLevelDakhilGPA = (Label)currentItem.FindControl("lblSSCOLevelDakhilGPA");
                Label lblSSCOLevelDakhilExamType = (Label)currentItem.FindControl("lblSSCOLevelDakhilExamType");
                Label lblSSCOLevelDakhilGroup = (Label)currentItem.FindControl("lblSSCOLevelDakhilGroup");
                Label lblSSCOLevelDakhilYear = (Label)currentItem.FindControl("lblSSCOLevelDakhilYear");

                Label lblHSCALevelAlimGPA = (Label)currentItem.FindControl("lblHSCALevelAlimGPA");
                Label lblHSCALevelAlimExamType = (Label)currentItem.FindControl("lblHSCALevelAlimExamType");
                Label lblHSCALevelAlimGroup = (Label)currentItem.FindControl("lblHSCALevelAlimGroup");
                Label lblHSCALevelAlimYear = (Label)currentItem.FindControl("lblHSCALevelAlimYear");

                Label lblTotalGPA = (Label)currentItem.FindControl("lblTotalGPA");
                Label lblQuotaName = (Label)currentItem.FindControl("lblQuotaName");

                System.Web.UI.WebControls.Image Photo1 = (System.Web.UI.WebControls.Image)currentItem.FindControl("Photo1");
                System.Web.UI.WebControls.Image Sign1 = (System.Web.UI.WebControls.Image)currentItem.FindControl("Sign1");



                CheckBox cbSingleBachelor = (CheckBox)currentItem.FindControl("cbSingleBachelor");

                HiddenField hfBachelorCandidateId = (HiddenField)currentItem.FindControl("hfBachelorCandidateId");
                HiddenField hfBachelorAcaCalId = (HiddenField)currentItem.FindControl("hfBachelorAcaCalId");
                HiddenField hfBachelorPaymentId = (HiddenField)currentItem.FindControl("hfBachelorPaymentId");
                HiddenField hfBachelorFormSerial = (HiddenField)currentItem.FindControl("hfBachelorFormSerial");
                HiddenField hfBachelorAdmissionSetupId = (HiddenField)currentItem.FindControl("hfBachelorAdmissionSetupId");
                HiddenField hfBachelorAdmissionUnitId = (HiddenField)currentItem.FindControl("hfBachelorAdmissionUnitId");

                hfBachelorCandidateId.Value = obj.CandidateID.ToString();
                hfBachelorAcaCalId.Value = obj.AcaCalID.ToString();
                hfBachelorPaymentId.Value = obj.PaymentId.ToString();
                hfBachelorFormSerial.Value = obj.FormSerial.ToString();
                hfBachelorAdmissionSetupId.Value = obj.AdmissionSetupID.ToString();
                hfBachelorAdmissionUnitId.Value = obj.AdmissionUnitID.ToString();


                lblName.Text = obj.CandidateName;
                lblMobile.Text = obj.Mobile;
                lblEmail.Text = obj.Email;
                lblFaculty.Text = obj.UnitName;
                lblPaymentId.Text = obj.PaymentId.ToString();

                lblPhotoSignature.Text = obj.CandidateImageSignature;

                lblSSCOLevelDakhilGPA.Text = obj.SSC_GPA.ToString();
                lblSSCOLevelDakhilExamType.Text = obj.SSC_ExamTypeName;
                lblSSCOLevelDakhilGroup.Text = obj.SSC_GroupOrSubjectName;
                lblSSCOLevelDakhilYear.Text = obj.SSC_PassingYear.ToString();

                lblHSCALevelAlimGPA.Text = obj.HSC_GPA.ToString();
                lblHSCALevelAlimExamType.Text = obj.HSC_ExamTypeName;
                lblHSCALevelAlimGroup.Text = obj.HSC_GroupOrSubjectName;
                lblHSCALevelAlimYear.Text = obj.HSC_PassingYear.ToString();

                lblTotalGPA.Text = obj.SSC_HSC_Total_GPA.ToString();

                lblQuotaName.Text = obj.QuotaName;

                cbSingleBachelor.Checked = Convert.ToBoolean(obj.IsApproved);


                #region Show Candidate Image And Signature


                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.Document> documentList = db.GetAllDocumentByCandidateID_AD(Convert.ToInt64(obj.CandidateID));
                        if (documentList != null && documentList.Any())
                        {
                            var Photo = documentList.Where(x => x.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                            var Sign = documentList.Where(x => x.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();

                            if (Photo != null)
                                Photo1.ImageUrl = Photo.URL + "?v=" + DateTime.Now;
                            else
                                Photo1.ImageUrl = "";

                            if (Sign != null)
                                Sign1.ImageUrl = Sign.URL + "?v=" + DateTime.Now;
                            else
                                Sign1.ImageUrl = "";
                        }
                        else
                        {
                            Photo1.ImageUrl = "";
                            Sign1.ImageUrl = "";

                        }
                    }
                }
                catch (Exception ex)
                {
                }

                #endregion

            }

        }

        protected void lvBachelor_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

            DataPagerBachelor.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            btnLoad_Click(null, null);
        }

        protected void cbSelectAllBachelor_CheckedChanged(object sender, EventArgs e)
        {
            bool isCheckAll = false;
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                isCheckAll = true;
            }


            for (int i = 0; i < lvBachelor.Items.Count; i++)
            {
                ListViewItem lvi = lvBachelor.Items[i];

                CheckBox cbSingleBachelor = (CheckBox)lvi.FindControl("cbSingleBachelor");

                cbSingleBachelor.Checked = isCheckAll;

            }
        }
        #endregion



        #region Masters List View
        protected void lvMasters_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetCandidateListForApprovalV3New_Result obj = (DAL.SPGetCandidateListForApprovalV3New_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblMobile = (Label)currentItem.FindControl("lblMobile");
                Label lblEmail = (Label)currentItem.FindControl("lblEmail");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");

                Label lblFaculty = (Label)currentItem.FindControl("lblFaculty");

                Label lblPhotoSignature = (Label)currentItem.FindControl("lblPhotoSignature");

                Label lblSSCOLevelDakhilGPA = (Label)currentItem.FindControl("lblSSCOLevelDakhilGPA");
                Label lblSSCOLevelDakhilExamType = (Label)currentItem.FindControl("lblSSCOLevelDakhilExamType");
                Label lblSSCOLevelDakhilGroup = (Label)currentItem.FindControl("lblSSCOLevelDakhilGroup");
                Label lblSSCOLevelDakhilYear = (Label)currentItem.FindControl("lblSSCOLevelDakhilYear");

                Label lblHSCALevelAlimGPA = (Label)currentItem.FindControl("lblHSCALevelAlimGPA");
                Label lblHSCALevelAlimExamType = (Label)currentItem.FindControl("lblHSCALevelAlimExamType");
                Label lblHSCALevelAlimGroup = (Label)currentItem.FindControl("lblHSCALevelAlimGroup");
                Label lblHSCALevelAlimYear = (Label)currentItem.FindControl("lblHSCALevelAlimYear");

                Label lblUndergradeGPA = (Label)currentItem.FindControl("lblUndergradeGPA");
                Label lblUndergradeProgram = (Label)currentItem.FindControl("lblUndergradeProgram");
                Label lblUndergradeInstitute = (Label)currentItem.FindControl("lblUndergradeInstitute");
                Label lblUndergradeYear = (Label)currentItem.FindControl("lblUndergradeYear");

                Label lblTotalGPA = (Label)currentItem.FindControl("lblTotalGPA");
                Label lblQuotaName = (Label)currentItem.FindControl("lblQuotaName");


                System.Web.UI.WebControls.Image Photo2 = (System.Web.UI.WebControls.Image)currentItem.FindControl("Photo2");
                System.Web.UI.WebControls.Image Sign2 = (System.Web.UI.WebControls.Image)currentItem.FindControl("Sign2");


                CheckBox cbSingleMasters = (CheckBox)currentItem.FindControl("cbSingleMasters");

                HiddenField hfMastersCandidateId = (HiddenField)currentItem.FindControl("hfMastersCandidateId");
                HiddenField hfMastersAcaCalId = (HiddenField)currentItem.FindControl("hfMastersAcaCalId");
                HiddenField hfMastersPaymentId = (HiddenField)currentItem.FindControl("hfMastersPaymentId");
                HiddenField hfMastersFormSerial = (HiddenField)currentItem.FindControl("hfMastersFormSerial");
                HiddenField hfMastersAdmissionSetupId = (HiddenField)currentItem.FindControl("hfMastersAdmissionSetupId");
                HiddenField hfMastersAdmissionUnitId = (HiddenField)currentItem.FindControl("hfMastersAdmissionUnitId");

                hfMastersCandidateId.Value = obj.CandidateID.ToString();
                hfMastersAcaCalId.Value = obj.AcaCalID.ToString();
                hfMastersPaymentId.Value = obj.PaymentId.ToString();
                hfMastersFormSerial.Value = obj.FormSerial.ToString();
                hfMastersAdmissionSetupId.Value = obj.AdmissionSetupID.ToString();
                hfMastersAdmissionUnitId.Value = obj.AdmissionUnitID.ToString();


                lblName.Text = obj.CandidateName;
                lblMobile.Text = obj.Mobile;
                lblEmail.Text = obj.Email;
                lblFaculty.Text = obj.UnitName;
                lblPaymentId.Text = obj.PaymentId.ToString();

                lblPhotoSignature.Text = obj.CandidateImageSignature;

                lblSSCOLevelDakhilGPA.Text = obj.SSC_GPA.ToString();
                lblSSCOLevelDakhilExamType.Text = obj.SSC_ExamTypeName;
                lblSSCOLevelDakhilGroup.Text = obj.SSC_GroupOrSubjectName;
                lblSSCOLevelDakhilYear.Text = obj.SSC_PassingYear.ToString();

                lblHSCALevelAlimGPA.Text = obj.HSC_GPA.ToString();
                lblHSCALevelAlimExamType.Text = obj.HSC_ExamTypeName;
                lblHSCALevelAlimGroup.Text = obj.HSC_GroupOrSubjectName;
                lblHSCALevelAlimYear.Text = obj.HSC_PassingYear.ToString();

                lblUndergradeGPA.Text = obj.Undergrade_CGPA.ToString();
                lblUndergradeProgram.Text = obj.Undergrade_ProgramName;
                lblUndergradeInstitute.Text = obj.Undergrade_Institute;
                lblUndergradeYear.Text = obj.Undergrade_PassingYear.ToString();

                lblTotalGPA.Text = obj.SSC_HSC_Total_GPA.ToString();

                lblQuotaName.Text = obj.QuotaName;

                cbSingleMasters.Checked = Convert.ToBoolean(obj.IsApproved);



                #region Show Candidate Image And Signature


                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        List<DAL.Document> documentList = db.GetAllDocumentByCandidateID_AD(Convert.ToInt64(obj.CandidateID));
                        if (documentList != null && documentList.Any())
                        {
                            var Photo = documentList.Where(x => x.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                            var Sign = documentList.Where(x => x.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();

                            if (Photo != null)
                                Photo2.ImageUrl = Photo.URL + "?v=" + DateTime.Now;
                            else
                                Photo2.ImageUrl = "";

                            if (Sign != null)
                                Sign2.ImageUrl = Sign.URL + "?v=" + DateTime.Now;
                            else
                                Sign2.ImageUrl = "";
                        }
                        else
                        {
                            Photo2.ImageUrl = "";
                            Sign2.ImageUrl = "";

                        }
                    }
                }
                catch (Exception ex)
                {
                }

                #endregion
            }

        }

        protected void lvMasters_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

            DataPagerMasters.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            btnLoad_Click(null, null);
        }

        protected void cbSelectAllMasters_CheckedChanged(object sender, EventArgs e)
        {
            bool isCheckAll = false;
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                isCheckAll = true;
            }


            for (int i = 0; i < lvMasters.Items.Count; i++)
            {
                ListViewItem lvi = lvMasters.Items[i];

                CheckBox cbSingleMasters = (CheckBox)lvi.FindControl("cbSingleMasters");

                cbSingleMasters.Checked = isCheckAll;

            }
        }



        #endregion

        protected void btnApproveUG_Click(object sender, EventArgs e)
        {
            try
            {
                int numberSaved = 0;
                int numberUpdated = 0;
                for (int i = 0; i < lvBachelor.Items.Count; i++)
                {
                    ListViewItem row = lvBachelor.Items[i];

                    CheckBox cbSingleBachelor = (CheckBox)row.FindControl("cbSingleBachelor");
                    try
                    {
                        HiddenField hfBachelorCandidateId = (HiddenField)row.FindControl("hfBachelorCandidateId");
                        HiddenField hfBachelorPaymentId = (HiddenField)row.FindControl("hfBachelorPaymentId");
                        HiddenField hfBachelorFormSerial = (HiddenField)row.FindControl("hfBachelorFormSerial");
                        HiddenField hfBachelorAcaCalId = (HiddenField)row.FindControl("hfBachelorAcaCalId");
                        HiddenField hfBachelorAdmissionUnitId = (HiddenField)row.FindControl("hfBachelorAdmissionUnitId");
                        HiddenField hfBachelorAdmissionSetupId = (HiddenField)row.FindControl("hfBachelorAdmissionSetupId");

                        DAL.ApprovedList approveObj = new DAL.ApprovedList();
                        approveObj.CandidateID = Convert.ToInt64(hfBachelorCandidateId.Value);
                        approveObj.FormSL = Convert.ToInt64(hfBachelorFormSerial.Value);
                        approveObj.AdmissionUnitID = Convert.ToInt32(hfBachelorAdmissionUnitId.Value);
                        approveObj.ProgramID = null;
                        approveObj.IsApproved = cbSingleBachelor.Checked;
                        approveObj.PaymentID = Convert.ToInt64(hfBachelorPaymentId.Value);
                        approveObj.AcaCalID = Convert.ToInt32(hfBachelorAcaCalId.Value);
                        approveObj.AdmissionSetupID = Convert.ToInt64(hfBachelorAdmissionSetupId.Value);
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
                                objExist.IsApproved = cbSingleBachelor.Checked;
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
                                if (cbSingleBachelor.Checked)
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
                            //lblMessage.Text = "Error Saving/Updating Bachelor Candidate. " + ex.Message + "; " + ex.InnerException.Message;
                            //lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                            //lblMessage.ForeColor = Color.Crimson;

                            MessageView("Error: Saving/Updating Bachelor Candidate! Exception: " + ex.Message.ToString(), "fail");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        //lblMessage.Text = "Error Saving/Updating Bachelor Candidate.(1) " + ex.Message + "; " + ex.InnerException.Message;
                        //lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                        //lblMessage.ForeColor = Color.Crimson;
                        MessageView("Error: Saving/Updating Bachelor Candidate(1)! Exception: " + ex.Message.ToString(), "fail");
                        return;
                    }

                    //lblMessage.Text = "Success. Number Saved: " + numberSaved + "; Number Updated: " + numberUpdated;
                    //lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                    //lblMessage.ForeColor = Color.Green;
                    //lblMessage.Focus();

                    MessageView("Success. Number Saved: " + numberSaved + "; Number Updated: " + numberUpdated, "success");
                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnApprovePG_Click(object sender, EventArgs e)
        {
            try
            {
                int numberSaved = 0;
                int numberUpdated = 0;
                for (int i = 0; i < lvMasters.Items.Count; i++)
                {
                    ListViewItem row = lvMasters.Items[i];

                    CheckBox cbSingleMasters = (CheckBox)row.FindControl("cbSingleMasters");
                    try
                    {
                        HiddenField hfMastersCandidateId = (HiddenField)row.FindControl("hfMastersCandidateId");
                        HiddenField hfMastersPaymentId = (HiddenField)row.FindControl("hfMastersPaymentId");
                        HiddenField hfMastersFormSerial = (HiddenField)row.FindControl("hfMastersFormSerial");
                        HiddenField hfMastersAcaCalId = (HiddenField)row.FindControl("hfMastersAcaCalId");
                        HiddenField hfMastersAdmissionUnitId = (HiddenField)row.FindControl("hfMastersAdmissionUnitId");
                        HiddenField hfMastersAdmissionSetupId = (HiddenField)row.FindControl("hfMastersAdmissionSetupId");

                        DAL.ApprovedList approveObj = new DAL.ApprovedList();
                        approveObj.CandidateID = Convert.ToInt64(hfMastersCandidateId.Value);
                        approveObj.FormSL = Convert.ToInt64(hfMastersFormSerial.Value);
                        approveObj.AdmissionUnitID = Convert.ToInt32(hfMastersAdmissionUnitId.Value);
                        approveObj.ProgramID = null;
                        approveObj.IsApproved = cbSingleMasters.Checked;
                        approveObj.PaymentID = Convert.ToInt64(hfMastersPaymentId.Value);
                        approveObj.AcaCalID = Convert.ToInt32(hfMastersAcaCalId.Value);
                        approveObj.AdmissionSetupID = Convert.ToInt64(hfMastersAdmissionSetupId.Value);
                        approveObj.CreatedBy = uId;
                        approveObj.DateCreated = DateTime.Now;
                        approveObj.ModifiedBy = null;
                        approveObj.DateModified = null;

                        try
                        {
                            #region Approve Candidate
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
                                objExist.IsApproved = cbSingleMasters.Checked;
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
                                if (cbSingleMasters.Checked)
                                {
                                    using (var dbInsert = new CandidateDataManager())
                                    {
                                        dbInsert.Insert<DAL.ApprovedList>(approveObj);
                                        numberSaved++;
                                    }
                                }
                            }
                            #endregion


                            try
                            {
                                long candidateId = Convert.ToInt64(hfMastersCandidateId.Value);
                                long admSetId = Convert.ToInt64(hfMastersAdmissionSetupId.Value);
                                if (candidateId > 0 && admSetId > 0)
                                {
                                    DAL.CandidateFacultyWiseDistrictSeat cfwds = null;
                                    using (var db = new CandidateDataManager())
                                    {
                                        cfwds = db.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == candidateId).FirstOrDefault();
                                    }

                                    if (cfwds != null)
                                    {
                                        if (cbSingleMasters.Checked == false)
                                        {
                                            /// <summary>
                                            /// First time approved
                                            /// then change to not approve
                                            /// In this case delete candidate from CandidateFacultyWiseDistrictSeat Tabel
                                            /// </summary>

                                            using (var db = new CandidateDataManager())
                                            {
                                                db.Delete<DAL.CandidateFacultyWiseDistrictSeat>(cfwds);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (cbSingleMasters.Checked)
                                        {
                                            DAL.CandidateFacultyWiseDistrictSeat cfwdsModel = new DAL.CandidateFacultyWiseDistrictSeat();
                                            cfwdsModel.CandidateId = candidateId;
                                            cfwdsModel.AdmissionSetupId = admSetId;
                                            cfwdsModel.DistrictId = 1; // 1 = Dhaka
                                            cfwdsModel.Remarks = "Masters";
                                            cfwdsModel.CreatedBy = uId;
                                            cfwdsModel.CreatedDate = DateTime.Now;

                                            using (var db = new CandidateDataManager())
                                            {
                                                db.Insert<DAL.CandidateFacultyWiseDistrictSeat>(cfwdsModel);
                                            }
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        catch (Exception ex)
                        {
                            //lblMessage.Text = "Error Saving/Updating Masters Candidate. " + ex.Message + "; " + ex.InnerException.Message;
                            //lblMessage.ForeColor = Color.Crimson;
                            MessageView("Error Saving/Updating Masters Candidate. Exception: " + ex.Message.ToString(), "fail");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        //lblMessage.Text = "Error Saving/Updating Masters Candidate.(1) " + ex.Message + "; " + ex.InnerException.Message;
                        //lblMessage.ForeColor = Color.Crimson;
                        MessageView("Error Saving/Updating Masters Candidate(1). Exception: " + ex.Message.ToString(), "fail");
                        return;
                    }

                    //lblMessage.Text = "Success. Number Saved: " + numberSaved + "; Number Updated: " + numberUpdated;
                    //lblMessage.ForeColor = Color.Green;
                    //lblMessage.Focus();

                    MessageView("Success. Number Saved: " + numberSaved + "; Number Updated: " + numberUpdated, "success");

                }
            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
            }
        }




        protected void btnFilterClear_Click(object sender, EventArgs e)
        {
            int count = 0;
            #region Check Filter Input Is Given
            if (!string.IsNullOrEmpty(txtFilterName.Text.Trim()))
            {
                count++;
            }
            if (!string.IsNullOrEmpty(txtFilterMobile.Text.Trim()))
            {
                count++;
            }
            if (!string.IsNullOrEmpty(txtFilterEmail.Text.Trim()))
            {
                count++;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtFilterPaymentID.Text.Trim()) && Convert.ToInt64(txtFilterPaymentID.Text.Trim()) > 0)
                {
                    count++;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            if (count > 0)
            {
                hfIsFilterClick.Value = "0";
                ClearFilter();
            }
        }
        protected void ClearFilter()
        {
            txtFilterName.Text = string.Empty;
            txtFilterMobile.Text = string.Empty;
            txtFilterEmail.Text = string.Empty;
            txtFilterPaymentID.Text = string.Empty;
            txtFilterUserID.Text = string.Empty;
        }

        protected void btnFilterData_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                long? admUnitId = null;
                int? acaCalId = null;
                int? eduCatId = null;


                try
                {
                    if (Convert.ToInt64(ddlFaculty.SelectedValue) > 0)
                    {
                        admUnitId = Convert.ToInt64(ddlFaculty.SelectedValue);
                    }
                    if (Convert.ToInt32(ddlSession.SelectedValue) > 0)
                    {
                        acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                    }
                    if (Convert.ToInt32(ddlEducationCategory.SelectedValue) > 0)
                    {
                        eduCatId = Convert.ToInt32(ddlEducationCategory.SelectedValue);
                    }
                }
                catch (Exception ex)
                {
                }

                //int? eduCatId = null;
                decimal? sscGPA = null;
                decimal? hscGPA = null;
                decimal? undergradeCGPA = null;
                decimal? sscHscTotalMarks = null;
                int? sscGroup = null;
                int? hscGroup = null;
                bool? isFinalSubmit = null;
                bool? isApproved = null;


                #region SSC/O-Level/Dakhil GPA
                if (!string.IsNullOrEmpty(txtSSCGpa.Text.Trim()) && Convert.ToDecimal(txtSSCGpa.Text.Trim()) > 0)
                {
                    sscGPA = Convert.ToDecimal(txtSSCGpa.Text.Trim());
                }
                else
                {
                    sscGPA = null;
                }
                #endregion

                #region HSC/A-Level/Alim GPA
                if (!string.IsNullOrEmpty(txtHSCGpa.Text.Trim()) && Convert.ToDecimal(txtHSCGpa.Text.Trim()) > 0)
                {
                    hscGPA = Convert.ToDecimal(txtHSCGpa.Text.Trim());
                }
                else
                {
                    hscGPA = null;
                }
                #endregion

                #region Undergrad GPA
                if (!string.IsNullOrEmpty(txtUndergradGpa.Text.Trim()) && Convert.ToDecimal(txtUndergradGpa.Text.Trim()) > 0)
                {
                    undergradeCGPA = Convert.ToDecimal(txtUndergradGpa.Text.Trim());
                }
                else
                {
                    undergradeCGPA = null;
                }
                #endregion

                #region HSC/A-Level/Alim GPA
                if (!string.IsNullOrEmpty(txtHSCGpa.Text.Trim()) && Convert.ToDecimal(txtHSCGpa.Text.Trim()) > 0)
                {
                    hscGPA = Convert.ToDecimal(txtHSCGpa.Text.Trim());
                }
                else
                {
                    hscGPA = null;
                }
                #endregion

                #region SSC/O-Level/Dakhil Group/Subject
                if (Convert.ToInt32(ddlSSCGrpSub.SelectedValue) > 0)
                {
                    sscGroup = Convert.ToInt32(ddlSSCGrpSub.SelectedValue);
                }
                else
                {
                    sscGroup = null;
                }
                #endregion

                #region HSC/A-Level/Alim Group/Subject
                if (Convert.ToInt32(ddlHSCGrpSub.SelectedValue) > 0)
                {
                    hscGroup = Convert.ToInt32(ddlHSCGrpSub.SelectedValue);
                }
                else
                {
                    hscGroup = null;
                }
                #endregion

                #region Final Submit
                if (Convert.ToInt32(ddlIsFinalSubmit.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(ddlIsFinalSubmit.SelectedValue) == 1)
                    {
                        isFinalSubmit = true;
                    }
                    else
                    {
                        isFinalSubmit = false;
                    }
                }
                else
                {
                    isFinalSubmit = null;
                }
                #endregion

                #region Approved
                if (Convert.ToInt32(ddlIsApproved.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(ddlIsApproved.SelectedValue) == 1)
                    {
                        isApproved = true;
                    }
                    else
                    {
                        isApproved = false;
                    }
                }
                else
                {
                    isApproved = null;
                }
                #endregion

                string candidateName = null;
                string candidateMobile = null;
                string candidateEmail = null;
                long? candidatePaymentId = null;
                string userLoginId = "";
                bool InvalidStatusId = false; /// Map with AttributeBit2 field.For FOE+FST Faculty there are some students who applied without grade checking
                                              /// So, to include them in the list we are using this flag.

                #region Candidate Name
                if (!string.IsNullOrEmpty(txtFilterName.Text.Trim()))
                {
                    candidateName = txtFilterName.Text.Trim();
                }
                #endregion

                #region Candidate Mobile
                if (!string.IsNullOrEmpty(txtFilterMobile.Text.Trim()))
                {
                    candidateMobile = txtFilterMobile.Text.Trim();
                }
                #endregion

                #region Candidate Email
                if (!string.IsNullOrEmpty(txtFilterEmail.Text.Trim()))
                {
                    candidateEmail = txtFilterEmail.Text.Trim();
                }
                #endregion

                #region Candidate PaymentID
                try
                {
                    if (!string.IsNullOrEmpty(txtFilterPaymentID.Text.Trim()) && Convert.ToInt64(txtFilterPaymentID.Text.Trim()) > 0)
                    {
                        candidatePaymentId = Convert.ToInt64(txtFilterPaymentID.Text.Trim());
                    }
                }
                catch (Exception ex)
                {
                    candidatePaymentId = null;
                }
                #endregion

                #region Cadiddate UserID
                try
                {
                    userLoginId = txtFilterUserID.Text.Trim();
                }
                catch (Exception ex)
                {
                }
                #endregion

                #region Status

                if (Convert.ToInt32(ddlStatus.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(ddlStatus.SelectedValue) == 1)
                    {
                        InvalidStatusId = true;
                    }
                    else
                    {
                        InvalidStatusId = false;
                    }

                }

                #endregion

                string QuotaName = "";
                int quotaId = Convert.ToInt32(ddlQuota.SelectedValue);
                if (quotaId != -1)
                {
                    QuotaName = ddlQuota.SelectedItem.ToString();
                }

                List<DAL.SPGetCandidateListForApprovalV3New_Result> list = null;
                using (var db = new GeneralDataManager())
                {
                    list = db.AdmissionDB.SPGetCandidateListForApprovalV3New(acaCalId,
                                                                            admUnitId,
                                                                            eduCatId,
                                                                            isFinalSubmit,
                                                                            isApproved,
                                                                            sscGPA,
                                                                            hscGPA,
                                                                            undergradeCGPA,
                                                                            null,
                                                                            sscHscTotalMarks,
                                                                            sscGroup,
                                                                            hscGroup,
                                                                            candidatePaymentId,
                                                                            candidateMobile,
                                                                            candidateEmail,
                                                                            candidateName,
                                                                            userLoginId,
                                                                            InvalidStatusId,
                                                                            QuotaName
                                                                            ).ToList();
                }

                if (list != null && list.Count > 0)
                {

                    if (eduCatId == 4)
                    {

                        btnApproveUG.Visible = true;
                        btnApprovePG.Visible = false;

                        panelBachelor.Visible = true;
                        lvBachelor.DataSource = list.OrderByDescending(c => c.SSC_HSC_Total_GPA).ToList();
                        lvBachelor.DataBind();
                        lblCountBachelor.Text = list.Count().ToString();

                        panelMasters.Visible = false;
                        lvMasters.DataSource = null;
                        lvMasters.DataBind();
                        lblCountMasters.Text = "0";
                    }
                    else
                    {
                        btnApproveUG.Visible = false;
                        btnApprovePG.Visible = true;

                        panelBachelor.Visible = false;
                        lvBachelor.DataSource = null;
                        lvBachelor.DataBind();
                        lblCountBachelor.Text = "0";


                        panelMasters.Visible = true;
                        lvMasters.DataSource = list.OrderByDescending(c => c.SSC_HSC_Total_GPA).ToList();
                        lvMasters.DataBind();
                        lblCountMasters.Text = list.Count().ToString();

                    }
                }
                else
                {
                    btnApproveUG.Visible = false;
                    btnApprovePG.Visible = false;

                    panelBachelor.Visible = false;
                    lvBachelor.DataSource = null;
                    lvBachelor.DataBind();
                    lblCountBachelor.Text = "0";

                    panelMasters.Visible = false;
                    lvMasters.DataSource = null;
                    lvMasters.DataBind();
                    lblCountMasters.Text = "0";
                }

            }
            catch (Exception ex)
            {
                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString() + " Error2", "fail");
            }
        }
    }
}