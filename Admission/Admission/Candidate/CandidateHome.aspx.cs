using Admission.App_Start;
using CommonUtility;
using DAL;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class CandidateHome : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        long cId = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                quotarow.Visible = false;
                if (uId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.BasicInfo candidateObj = db.GetCandidateBasicInfoByUserID_ND(uId);
                        if (candidateObj != null)
                        {
                            cId = candidateObj.ID;
                            LoadCandidateData(candidateObj);
                        }
                        else
                        {
                            Response.Redirect("~/Admission/Login.aspx");
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void LoadCandidateData(DAL.BasicInfo candidateObj)
        {
            string paymentid = "";
            int educationCategoryId = -1;
            int programId = -1;
            DAL.CandidatePayment cp = null;

            using (var db = new CandidateDataManager())
            {
                cp = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == candidateObj.ID).FirstOrDefault();

                if (cp != null)
                {
                    paymentid = cp.PaymentId.ToString();

                    hrefPrintAdmitCard.NavigateUrl = "~/Admission/Candidate/Prints/AdmitCardV2.aspx?val=" + cp.CandidateID.ToString();

                    educationCategoryId = db.GetCandidateEducationCategoryID(Convert.ToInt64(cp.CandidateID));

                    if (educationCategoryId != 4)
                    {
                        DAL.CandidateFormSl formSerial = db.GetCandidateFormSlByCandID_AD(Convert.ToInt64(cp.CandidateID));

                        if (formSerial != null && formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms != null)
                        {
                            try
                            {
                                programId = formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.Where(x => x.IsActive == true && x.AcaCalID == formSerial.AcaCalID).FirstOrDefault().ProgramID;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }


            lblPaymentId.Text = paymentid;
            lblCandidateName.Text = candidateObj.FirstName;
            lblDateOfBirth.Text = Convert.ToDateTime(candidateObj.DateOfBirth).ToString("dd/MM/yyyy"); //.ToShortDateString();
            if (candidateObj.QuotaID == null) { lblQuota.Text = null; }
            else { lblQuota.Text = candidateObj.Quota.Remarks; }

            bool isUndergradCandidate = true;

            lblPrograms.Text = string.Empty;

            if (candidateObj.QuotaID != null && candidateObj.QuotaID != 0 && candidateObj.QuotaID != 7)
            {
                //quotarow.Visible = true;
                string status = "Not Verified";
                DAL.QuotaInfo qiModel = null;
                using (var db = new CandidateDataManager())
                {
                    qiModel = db.AdmissionDB.QuotaInfoes.Where(x => x.CandidateID == candidateObj.ID).FirstOrDefault();
                    if (qiModel != null && qiModel.IsVerifiedDocument != null && qiModel.IsVerifiedDocument == true)
                        status = "Verified";
                }
                lblQuotaStatus.Text = status;

                lblQuotaStatus.ForeColor = status == "Verified" ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            }

            #region Property Setup (Fill-UP Application Form & Admit Card Show/Hide)
            List<DAL.PropertySetup> propertySetupList = null;
            using (var db = new OfficeDataManager())
            {
                propertySetupList = db.AdmissionDB.PropertySetups.Where(x => x.IsActive == true
                                                                            && x.EducationCategoryID == educationCategoryId).ToList();
            }

            if (propertySetupList != null && propertySetupList.Count > 0)
            {
                if (educationCategoryId == 4)
                {

                    panelFacultyWiseProgramPriority.Visible = true;

                    #region Bachelor

                    #region Fill-UP Application Form Show/Hide
                    try
                    {
                        var fillupApplicationFormSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.FillUpApplicationForm)).FirstOrDefault();
                        if (fillupApplicationFormSetup != null)
                        {
                            bool showHide = Convert.ToBoolean(fillupApplicationFormSetup.IsVisible);
                            btnApplicationForm.Visible = showHide;
                        }
                        else
                        {
                            btnApplicationForm.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        btnApplicationForm.Visible = false;
                    }
                    #endregion

                    #region Admit Card Show/Hide
                    try
                    {
                        var admitCardSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.AdmitCard)).FirstOrDefault();
                        if (admitCardSetup != null)
                        {

                            /// <summary>
                            /// Check While Download Admit Card
                            /// Candidate is a Current Active Session Candidate
                            /// If No a Current session candidate
                            /// then dont show download button
                            /// If Yes then can download admit card
                            /// </summary> 
                            DAL.AdmissionSetup admSetModel = null;
                            using (var db = new OfficeDataManager())
                            {
                                admSetModel = db.AdmissionDB.AdmissionSetups.Where(x => x.AcaCalID == cp.AcaCalID && x.IsActive == true).FirstOrDefault();
                            }

                            if (admSetModel != null)
                            {
                                bool showHide = Convert.ToBoolean(admitCardSetup.IsVisible);
                                hrefPrintAdmitCard.Visible = showHide;
                            }
                            else
                            {
                                hrefPrintAdmitCard.Visible = false;
                            }


                        }
                        else
                        {
                            hrefPrintAdmitCard.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        hrefPrintAdmitCard.Visible = false;
                    }
                    #endregion 

                    #endregion
                }
                else
                {
                    panelFacultyWiseProgramPriority.Visible = false;

                    #region Masters

                    #region Fill-UP Application Form Show/Hide
                    try
                    {
                        var fillupApplicationFormSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.FillUpApplicationForm)
                                                                                    && x.ProgramId == programId).FirstOrDefault();
                        if (fillupApplicationFormSetup != null)
                        {
                            bool showHide = Convert.ToBoolean(fillupApplicationFormSetup.IsVisible);
                            btnApplicationForm.Visible = showHide;
                        }
                        else
                        {
                            btnApplicationForm.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        btnApplicationForm.Visible = false;
                    }
                    #endregion

                    #region Admit Card Show/Hide
                    try
                    {
                        var admitCardSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.AdmitCard)
                                                                        && x.ProgramId == programId).FirstOrDefault();
                        if (admitCardSetup != null)
                        {
                            bool showHide = Convert.ToBoolean(admitCardSetup.IsVisible);
                            hrefPrintAdmitCard.Visible = showHide;
                        }
                        else
                        {
                            hrefPrintAdmitCard.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        hrefPrintAdmitCard.Visible = false;
                    }
                    #endregion 

                    #endregion
                }
            }
            else
            {
                btnApplicationForm.Visible = false;
                hrefPrintAdmitCard.Visible = false;
            }
            #endregion



            //int programId = -1;


            using (var db = new CandidateDataManager())
            {
                #region N/A
                //if (propertySetupList != null && propertySetupList.Count > 0)
                //{
                //    DAL.CandidateFormSl cf = null; //new DAL.CandidateFormSl();
                //    cf = db.GetCandidateFormSlByCandID_AD(candidateObj.ID);



                //    if (cf != null && cf.AdmissionSetup != null)
                //    {

                //        int eduCatIdT = cf.AdmissionSetup.EducationCategoryID;

                //        if (eduCatIdT == 4)
                //        {
                //            List<DAL.PropertySetup> propertySetupListB = propertySetupList.Where(x => x.EducationCategoryID == 4).ToList();

                //            #region Admit Card

                //            // == PropertyTypeID => 1 => Admit Card
                //            if (propertySetupListB.Where(x => x.PropertyTypeID == 1).FirstOrDefault() != null)
                //            {
                //                bool isVisible = Convert.ToBoolean(propertySetupListB.Where(x => x.PropertyTypeID == 1).FirstOrDefault().IsVisible);

                //                hrefPrintAdmitCard.Visible = isVisible;
                //            }
                //            else
                //            {
                //                hrefPrintAdmitCard.Visible = false;
                //            }
                //            #endregion

                //            #region Fill Up Application Form

                //            // == PropertyTypeID => 2 => Fill Up Application Form
                //            if (propertySetupListB.Where(x => x.PropertyTypeID == 2).FirstOrDefault() != null)
                //            {
                //                bool isVisible = Convert.ToBoolean(propertySetupListB.Where(x => x.PropertyTypeID == 2).FirstOrDefault().IsVisible);

                //                btnApplicationForm.Visible = isVisible;
                //            }
                //            else
                //            {
                //                btnApplicationForm.Visible = false;
                //            }
                //            #endregion

                //        }
                //        else
                //        {
                //            #region Get Program ID
                //            DAL.AdmissionUnitProgram admUnitProg = null;
                //            using (var db2 = new OfficeDataManager())
                //            {
                //                admUnitProg = db2.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == cf.CandidatePayment.AcaCalID
                //                                                                            && x.EducationCategoryID == eduCatIdT
                //                                                                            && x.AdmissionUnitID == cf.AdmissionSetup.AdmissionUnitID
                //                                                                            && x.IsActive == true).FirstOrDefault();
                //            }

                //            if (admUnitProg != null)
                //            {
                //                programId = admUnitProg.ProgramID;
                //            }
                //            else
                //            {
                //                programId = -1;
                //            }
                //            #endregion

                //            List<DAL.PropertySetup> propertySetupListM = propertySetupList.Where(x => x.EducationCategoryID == 6).ToList();

                //            // == PropertyTypeID => 1 => Admit Card

                //            if (programId > 0)
                //            {
                //                #region Admit Card

                //                // == PropertyTypeID => 1 => Admit Card
                //                if (propertySetupListM.Where(x => x.PropertyTypeID == 1 && x.ProgramId == programId).FirstOrDefault() != null)
                //                {
                //                    bool isVisible = Convert.ToBoolean(propertySetupListM.Where(x => x.PropertyTypeID == 1 && x.ProgramId == programId).FirstOrDefault().IsVisible);

                //                    hrefPrintAdmitCard.Visible = isVisible;
                //                }
                //                else
                //                {
                //                    hrefPrintAdmitCard.Visible = false;
                //                }
                //                #endregion

                //                #region Fill Up Application Form

                //                // == PropertyTypeID => 2 => Fill Up Application Form
                //                if (propertySetupListM.Where(x => x.PropertyTypeID == 2 && x.ProgramId == programId).FirstOrDefault() != null)
                //                {
                //                    bool isVisible = Convert.ToBoolean(propertySetupListM.Where(x => x.PropertyTypeID == 2 && x.ProgramId == programId).FirstOrDefault().IsVisible);

                //                    btnApplicationForm.Visible = isVisible;
                //                }
                //                else
                //                {
                //                    btnApplicationForm.Visible = false;
                //                }
                //                #endregion
                //            }
                //            else
                //            {
                //                hrefPrintAdmitCard.Visible = false;
                //                btnApplicationForm.Visible = false;
                //            }

                //        }



                //        #region N/A
                //        //    //... Admit Card
                //        //    if (propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID).FirstOrDefault() != null)
                //        //{
                //        //    bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID).FirstOrDefault().IsVisible);

                //        //    hrefPrintAdmitCard.Visible = isVisible;
                //        //}
                //        //else
                //        //{
                //        //    hrefPrintAdmitCard.Visible = false;
                //        //}


                //        //// === 28 => LLM (Professional)
                //        //if (propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID && x.ProgramId == 28).FirstOrDefault() != null)
                //        //{
                //        //    bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID && x.ProgramId == 28).FirstOrDefault().IsVisible);

                //        //    hrefPrintAdmitCard.Visible = isVisible;
                //        //}
                //        //else
                //        //{
                //        //    hrefPrintAdmitCard.Visible = false;
                //        //}

                //        //// === 7 => MBA (Professional)
                //        //if (propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID && x.ProgramId == 7).FirstOrDefault() != null)
                //        //{
                //        //    bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID && x.ProgramId == 7).FirstOrDefault().IsVisible);

                //        //    hrefPrintAdmitCard.Visible = isVisible;
                //        //}
                //        //else
                //        //{
                //        //    hrefPrintAdmitCard.Visible = false;
                //        //}

                //        //// === 29 => MPCHRS
                //        //if (propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID && x.ProgramId == 29).FirstOrDefault() != null)
                //        //{
                //        //    bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 1 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID && x.ProgramId == 29).FirstOrDefault().IsVisible);

                //        //    hrefPrintAdmitCard.Visible = isVisible;
                //        //}
                //        //else
                //        //{
                //        //    hrefPrintAdmitCard.Visible = false;
                //        //}



                //        ////... Fill Up Application Form
                //        //if (propertySetupList.Where(x => x.PropertyTypeID == 2 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID).FirstOrDefault() != null)
                //        //{
                //        //    bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 2 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID).FirstOrDefault().IsVisible);


                //        //    btnApplicationForm.Visible = isVisible;
                //        //}
                //        //else
                //        //{
                //        //    btnApplicationForm.Visible = false;
                //        //}


                //        ////... Purchase More Application
                //        //if (propertySetupList.Where(x => x.PropertyTypeID == 3 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID).FirstOrDefault() != null)
                //        //{
                //        //    bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 2 && x.EducationCategoryID == cf.AdmissionSetup.EducationCategoryID).FirstOrDefault().IsVisible);


                //        //    hrefMultiApp.Visible = isVisible;
                //        //}
                //        //else
                //        //{
                //        //    hrefMultiApp.Visible = false;
                //        //} 
                //        #endregion


                //        //... Faculty Wise Program Priority
                //        if (cf.AdmissionSetup.EducationCategoryID == 4)
                //        {

                //            //btnApplicationForm.Visible = false;
                //            //hrefMultiApp.Visible = false;
                //            panelFacultyWiseProgramPriority.Visible = true;
                //        }
                //        else
                //        {
                //            //btnApplicationForm.Visible = true;
                //            //hrefMultiApp.Visible = false;
                //            panelFacultyWiseProgramPriority.Visible = false;
                //        }

                //    }
                //} 
                #endregion

                #region Get Candidate Faculty wise Program Priority
                List<DAL.SPGetCandidateProgramPriorities_Result> list = null;

                try
                {
                    list = db.AdmissionDB.SPGetCandidateProgramPriorities(candidateObj.ID).ToList();
                }
                catch (Exception)
                {
                    lblPrograms.Text = "Error: Unable to get candidate choices.";
                    lblPrograms.Attributes.CssStyle.Add("font-weight", "bold");
                    lblPrograms.Attributes.CssStyle.Add("color", "crimson");
                    return;
                }

                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        lvProgramPriority.DataSource = list.OrderBy(c => c.cP_Priority).OrderBy(c => c.admUnit_Name).ToList();
                        lvProgramPriority.DataBind();

                    }
                    else
                    {
                        lvProgramPriority.DataSource = null;
                        lvProgramPriority.DataBind();

                        lblPrograms.Text = "Please Enter your program choice in PROGRAM PRIORITY section of the Fill Up Application Form";
                        lblPrograms.Attributes.CssStyle.Add("font-weight", "bold");
                        lblPrograms.Attributes.CssStyle.Add("color", "crimson");


                    }
                }
                else
                {
                    lvProgramPriority.DataSource = null;
                    lvProgramPriority.DataBind();

                    lblPrograms.Text = "Please Enter your program choice in PRIORITY section of the FORM";
                    lblPrograms.Attributes.CssStyle.Add("font-weight", "bold");
                    lblPrograms.Attributes.CssStyle.Add("color", "crimson");

                }
                #endregion

                #region Get Candidate Image
                DAL.Document photoDocumentObj = new DAL.Document();

                photoDocumentObj = db.GetDocumentByCandidateIDDocumentTypeID_AD(candidateObj.ID, 2); // 2 = Image/Photo

                if (photoDocumentObj != null && photoDocumentObj.DocumentDetail != null)
                {
                    imgCtrl.Src = photoDocumentObj.DocumentDetail.URL;
                    lblPhoto.Visible = false;
                }
                else
                {
                    imgCtrl.Visible = false;
                    lblPhoto.Visible = true;
                }
                #endregion

                #region Get Candidate Additional Info
                DAL.AdditionalInfo candAddInfo = null;

                candAddInfo = db.GetAdditionalInfoByCandidateID_ND(candidateObj.ID);
                if (candAddInfo != null)
                {
                    if (candAddInfo.IsFinalSubmit == true)
                    {
                        lblApplicationFormStatus.Text = "✔️";
                        lblApplicationFormStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblApplicationFormStatus.Text = "✖️";
                        lblApplicationFormStatus.ForeColor = Color.Crimson;
                    }
                }
                else
                {
                    lblApplicationFormStatus.Text = "✖️";
                    lblApplicationFormStatus.ForeColor = Color.Crimson;
                }
                #endregion
            }
        }

        protected void btnApplicationForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admission/Candidate/ApplicationBasic.aspx", false);
        }


        string lastValue = "";
        protected string AddGroupingHeader()
        {


            //Get the data field value of interest for this row
            string currentValue = Eval("admUnit_Name").ToString();


            //Specify name to display if dataFieldValue is a database NULL
            if (currentValue.Length == 0)
            {
                currentValue = "";
            }

            string sNewRow = "";
            //See if there's been a change in value
            if (lastValue != currentValue)
            {
                //There's been a change! Record the change and emit the header
                lastValue = currentValue;
                sNewRow = "<tr style='background-color: gainsboro;'><td colspan='3'><h4>" + currentValue +
                             "</h4></td><td colspan='2' style='vertical-align: middle; font-weight: bold;'>Choice</td></tr>";
                return sNewRow;
            }
            else
            {
                return "";
            }
        }

        protected void lvProgramPriority_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetCandidateProgramPriorities_Result progP = (DAL.SPGetCandidateProgramPriorities_Result)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
                Label lblProgramName = (Label)currentItem.FindControl("lblProgramName");
                Label lblChoice = (Label)currentItem.FindControl("lblChoice");

                LinkButton lnkRemove = (LinkButton)currentItem.FindControl("lnkRemove");
                if (lblSerial != null)
                    lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblUnitName.Text = progP.admUnit_Name;
                lblProgramName.Text = progP.admUnitProgZ_ProgramName;
                lblChoice.Text = progP.cP_Priority.ToString();

                //lnkRemove.CommandName = "Remove";
                //lnkRemove.CommandArgument = progP.cP_ID.ToString();
            }
        }



    }
}