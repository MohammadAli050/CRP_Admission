using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.CandidateInfo
{
    public partial class CandApplicationDeclaration : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        string userName = "";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key

            using (var db = new OfficeDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppAdditional.NavigateUrl = "CandApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "CandApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "CandApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "CandApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "CandApplicationEducation.aspx?val=" + queryVal;
                hrefAppPriority.NavigateUrl = "CandApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "CandApplicationRelation.aspx?val=" + queryVal;
                hrefAppDeclaration.NavigateUrl = "CandApplicationDeclaration.aspx?val=" + queryVal;
            }

            if (!IsPostBack)
            {
                //LoadDDL();
                LoadCandidateData(cId);
                LoadInstituteInfo();
            }
        }

        private void LoadCandidateData(long cId)
        {
            MessageView("", "clear");
            if (cId > 0)
            {
                DAL.AdditionalInfo additionalInfoObjT = null;
                using (var db = new CandidateDataManager())
                {
                    additionalInfoObjT = db.GetAdditionalInfoByCandidateID_ND(cId);
                }

                if (additionalInfoObjT != null && Convert.ToBoolean(additionalInfoObjT.IsFinalSubmit) == true)
                {
                    MessageView("Final submission is already done.", "success");
                    btnSave_Declaration.Visible = false;
                }
                else
                {
                    btnSave_Declaration.Visible = true;
                }
            }
            else
            {
                MessageView("Invalid Request!", "fail");
            }
        }

        private void LoadInstituteInfo()
        {
            DAL.Institute institute = null;
            using (var db = new OfficeDataManager())
            {
                institute = db.AdmissionDB.Institutes.Find(1);
            }

            if (institute != null)
            {
                lblUniName1.Text = institute.Name;
                lblUniShortName1.Text = institute.ShortName;
                lblUniShortName2.Text = institute.ShortName;
                lblUniShortName3.Text = institute.ShortName;
                lblUniShortName4.Text = institute.ShortName;
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

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        protected void btnSave_Declaration_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            ////btnSave_Declaration.Enabled = false;

            //long cId = -1;
            //if (uId > 0)
            //{
            //    using (var db = new CandidateDataManager())
            //    {
            //        cId = db.GetCandidateIdByUserID_ND(uId);
            //    }
            //}

            if (chbxAgreed.Checked == false)
            {
                //lblMessage.Text = "You must agree to the terms.";
                //return;

                MessageView("You must agree to the terms!", "fail");
            }
            else
            {
                if (cId > 0 && uId > 0)
                {


                    #region Get EducationCategoryId & ProgramId
                    int educationCategoryId = -1;
                    int programId = -1;
                    long? paymentID = -1;
                    using (var db = new CandidateDataManager())
                    {
                        paymentID = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).Select(x => x.PaymentId).FirstOrDefault();

                        educationCategoryId = db.GetCandidateEducationCategoryID(cId);

                        if (educationCategoryId != 4)
                        {
                            DAL.CandidateFormSl formSerial = db.GetCandidateFormSlByCandID_AD(cId);

                            if (formSerial != null && formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms != null)
                            {
                                programId = formSerial.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms.FirstOrDefault().ProgramID;
                            }
                        }
                    }
                    #endregion


                    #region BASIC INFO

                    string basicStr = string.Empty;

                    DAL.BasicInfo candidate = null;
                    using (var db = new CandidateDataManager())
                    {
                        candidate = db.AdmissionDB.BasicInfoes.Find(cId);
                    }

                    if (candidate != null)
                    {
                        if (
                            string.IsNullOrEmpty(candidate.FirstName) ||
                            candidate.DateOfBirth == null ||
                            candidate.GenderID == null ||
                            candidate.BloodGroupID == null ||
                            string.IsNullOrEmpty(candidate.Email) ||
                            string.IsNullOrEmpty(candidate.Mobile) ||
                            candidate.ReligionID == null
                            )
                        {
                            basicStr = "Some basic candidate information is missing.";
                        }
                        else
                        {
                            basicStr = string.Empty;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Error getting candidate info.";
                        return;
                    }

                    #endregion

                    #region PRIORITY

                    string choiceStr = string.Empty;

                    if (educationCategoryId == 4)
                    {
                        List<DAL.CandidateFormSl> cformserList = null;
                        using (var db = new CandidateDataManager())
                        {
                            cformserList = db.AdmissionDB.CandidateFormSls.Where(x=> x.CandidateID == cId).ToList();

                            if (cformserList != null && cformserList.Count > 0)
                            {
                                foreach (var tData in cformserList)
                                {
                                    DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == tData.CandidateID
                                                                                                      && x.AdmissionSetupID == tData.AdmissionSetupID
                                                                                                      && x.Priority == 1).FirstOrDefault();

                                    if (pp == null)
                                    {
                                        string msg = "Faculty: " + tData.AdmissionSetup.AdmissionUnit.UnitName + " need a 1st Choice!";
                                        choiceStr += msg;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        choiceStr = string.Empty;
                    }


                    #region N/A
                    //List<DAL.CandidateFormSl> candidateFormSlList = null;
                    //DAL.AdmissionSetup admSetup = null;

                    //using (var db = new CandidateDataManager())
                    //{
                    //    candidateFormSlList = db.GetAllCandidateFormSlByCandID_AD(cId);
                    //    if (candidateFormSlList != null && candidateFormSlList.Count > 0)
                    //    {
                    //        ////get only admSetup for masters.
                    //        //admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6).Select(c => c.AdmissionSetup).FirstOrDefault();

                    //        admSetup = candidateFormSlList.Select(c => c.AdmissionSetup).FirstOrDefault();
                    //    }
                    //}

                    //if (admSetup != null && admSetup.EducationCategoryID == 4)
                    //{
                    //    if (candidateFormSlList != null && candidateFormSlList.Count > 0)
                    //    {

                    //        if (candidateFormSlList.FirstOrDefault().AdmissionSetup.EducationCategoryID == 4)
                    //        {
                    //            List<DAL.ProgramPriority> choices = null;
                    //            DAL.ProgramPriority firstChoice = null;

                    //            using (var db = new CandidateDataManager())
                    //            {
                    //                choices = db.AdmissionDB.ProgramPriorities.Where(c => c.CandidateID == cId).ToList();
                    //            }

                    //            if (choices != null)
                    //            {
                    //                if (choices.Count > 0)
                    //                {
                    //                    firstChoice = choices.Where(c => c.Priority == 1).FirstOrDefault();
                    //                }
                    //            }

                    //            if (firstChoice == null)
                    //            {
                    //                choiceStr = "You have not selected any program as 1st Choice.";
                    //            }
                    //            else
                    //            {
                    //                choiceStr = string.Empty;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            choiceStr = string.Empty;
                    //        }

                    //    }
                    //}
                    //else
                    //{
                    //    choiceStr = string.Empty;
                    //} 
                    #endregion


                    #endregion

                    #region EDUCATION

                    string educationStr = string.Empty;

                    List<DAL.Exam> exam = null;
                    using (var db = new CandidateDataManager())
                    {
                        exam = db.GetAllExamByCandidateID_AD(cId);
                    }

                    if (exam != null)
                    {
                        DAL.ExamDetail ssc = exam.Where(c => c.ExamTypeID == 1 || c.ExamTypeID == 5 || c.ExamTypeID == 6 || c.ExamTypeID == 12 || c.ExamTypeID == 14).Select(c => c.ExamDetail).FirstOrDefault();
                        DAL.ExamDetail hsc = exam.Where(c => c.ExamTypeID == 2 || c.ExamTypeID == 7 || c.ExamTypeID == 8 || c.ExamTypeID == 9 || c.ExamTypeID == 13 || c.ExamTypeID == 15).Select(c => c.ExamDetail).FirstOrDefault();

                        if (ssc != null)
                        {
                            if (
                                ssc.EducationBoardID < 0 ||
                                string.IsNullOrEmpty(ssc.Institute) ||
                                string.IsNullOrEmpty(ssc.RollNo) ||
                                ssc.GroupOrSubjectID == null ||
                                (ssc.ResultDivisionID == null || (ssc.ResultDivisionID == 5 && string.IsNullOrEmpty(ssc.GPA.ToString()))) ||
                                string.IsNullOrEmpty(ssc.PassingYear.ToString())
                                )
                            {
                                educationStr += "Some SSC/O-Level/Dakhil information missing.";
                            }
                            else
                            {
                                educationStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting education info (SSC/O-Level/Dakhil)";
                            return;
                        }

                        if (hsc != null)
                        {
                            if (
                                hsc.EducationBoardID < 0 ||
                                string.IsNullOrEmpty(hsc.Institute) ||
                                string.IsNullOrEmpty(hsc.RollNo) ||
                                hsc.GroupOrSubjectID == null ||
                                (hsc.ResultDivisionID == null || (hsc.ResultDivisionID == 5 && string.IsNullOrEmpty(hsc.GPA.ToString()))) ||
                                string.IsNullOrEmpty(hsc.PassingYear.ToString())
                                )
                            {
                                educationStr += "Some HSC/A-Level/Alim information missing.";
                            }
                            else
                            {
                                educationStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting education info (HSC/A-Level/Alim)";
                            return;
                        }

                        //TODO: Check for undergraduate...then check for information in undergrad and grad...
                    }
                    else
                    {
                        lblMessage.Text = "Error getting education info.";
                        return;
                    }

                    #endregion

                    #region PARENT/GUARDIAN

                    string relationStr = string.Empty;

                    List<DAL.Relation> relation = null;
                    using (var db = new CandidateDataManager())
                    {
                        relation = db.GetAllRelationByCandidateID_AD(cId);
                    }

                    if (relation != null)
                    {
                        DAL.RelationDetail father = relation.Where(c => c.RelationTypeID == 2).Select(c => c.RelationDetail).FirstOrDefault();
                        DAL.RelationDetail mother = relation.Where(c => c.RelationTypeID == 3).Select(c => c.RelationDetail).FirstOrDefault();
                        DAL.RelationDetail guardian = relation.Where(c => c.RelationTypeID == 5).Select(c => c.RelationDetail).FirstOrDefault();

                        if (father != null)
                        {
                            //if ((father.IsLate == null || father.IsLate == false) &&
                            //    (string.IsNullOrEmpty(father.Name) || string.IsNullOrEmpty(father.Occupation) ||
                            //    string.IsNullOrEmpty(father.Mobile))
                            //    )
                            //{
                            //    relationStr += "Some information about your father is missing.";
                            //}
                            //else if (father.IsLate == true)
                            //{
                            //    if (string.IsNullOrEmpty(father.Name))
                            //    {
                            //        relationStr += "father name is missing.";
                            //    }
                            //}
                            //else
                            //{
                            //    relationStr += string.Empty;
                            //}
                            if (string.IsNullOrEmpty(father.Name))
                            {
                                relationStr += "Father name is missing.";
                            }
                            else
                            {
                                relationStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting Parent/Guardian (Father) information.";
                            return;
                        }

                        if (mother != null)
                        {
                            //if ( (mother.IsLate == null || mother.IsLate == false) &&
                            //    (string.IsNullOrEmpty(mother.Name) || string.IsNullOrEmpty(mother.Occupation) ||
                            //    string.IsNullOrEmpty(mother.Mobile))
                            //    )
                            //{
                            //    relationStr += "Some information about your mother is missing.";
                            //}
                            //else if(mother.IsLate == true)
                            //{
                            //    if (string.IsNullOrEmpty(mother.Name))
                            //    {
                            //        relationStr += "Mother name is missing.";
                            //    }
                            //}
                            //else
                            //{
                            //    relationStr += string.Empty;
                            //}
                            if (string.IsNullOrEmpty(mother.Name))
                            {
                                relationStr += "Mother name is missing.";
                            }
                            else
                            {
                                relationStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting Parent/Guardian (Mother) information.";
                            return;
                        }

                        //if (guardian != null)
                        //{
                        //    if (
                        //        string.IsNullOrEmpty(guardian.Name) ||
                        //        string.IsNullOrEmpty(guardian.Mobile) ||
                        //        string.IsNullOrEmpty(guardian.RelationWithGuardian) ||
                        //        string.IsNullOrEmpty(guardian.MailingAddress)
                        //        )
                        //    {
                        //        relationStr += "Some information about your guardian is missing.";
                        //    }
                        //    else
                        //    {
                        //        relationStr += string.Empty;
                        //    }
                        //}
                        //else
                        //{
                        //    lblMessage.Text = "Error getting Parent/Guardian (Guardian) information.";
                        //    return;
                        //}
                    }
                    else
                    {
                        lblMessage.Text = "Error getting Parent/Guardian information.";
                        return;
                    }

                    #endregion

                    #region ADDRESS

                    string addressStr = string.Empty;

                    List<DAL.Address> address = null;
                    using (var db = new CandidateDataManager())
                    {
                        address = db.GetAllAddressByCandidateID_AD(cId);
                    }

                    if (address != null)
                    {
                        DAL.AddressDetail prs = address.Where(c => c.AddressTypeID == 2).Select(c => c.AddressDetail).FirstOrDefault();
                        DAL.AddressDetail prm = address.Where(c => c.AddressTypeID == 3).Select(c => c.AddressDetail).FirstOrDefault();

                        if (prs != null)
                        {
                            if (
                                string.IsNullOrEmpty(prs.AddressLine) ||
                                prs.DistrictID == null ||
                                prs.DivisionID == null ||
                                prs.CountryID == null ||
                                string.IsNullOrEmpty(prs.PostCode)
                                )
                            {
                                addressStr += "Some present address infromation missing.";
                            }
                            else
                            {
                                addressStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting present address";
                            return;
                        }

                        if (prm != null)
                        {
                            if (
                                string.IsNullOrEmpty(prm.AddressLine) ||
                                prm.DistrictID == null ||
                                prm.DivisionID == null ||
                                prm.CountryID == null ||
                                string.IsNullOrEmpty(prm.PostCode)
                                )
                            {
                                addressStr += "Some permanent address infromation missing.";
                            }
                            else
                            {
                                addressStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Error getting permanent address.";
                            return;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Error getting address.";
                        return;
                    }

                    #endregion

                    #region ADDITIONAL

                    string additionalStr = string.Empty;

                    DAL.AdditionalInfo additional = null;
                    using (var db = new CandidateDataManager())
                    {
                        additional = db.GetAdditionalInfoByCandidateID_ND(cId);
                    }

                    //if (additional != null)
                    //{
                    //    if (
                    //        additional.IsEnrolled == null || (additional.IsEnrolled == true && string.IsNullOrEmpty(additional.CurrentStudentId))
                    //        )
                    //    {
                    //        additionalStr += "Some additional information missing.";
                    //    }
                    //    else
                    //    {
                    //        additionalStr += string.Empty;
                    //    }
                    //}
                    //else
                    //{
                    //    lblMessage.Text = "Error getting additional information.";
                    //    return;
                    //}

                    //TODO: check for masters, then check work experience info.

                    #endregion

                    #region FINANCIAL GUARANTOR

                    //string fingStr = string.Empty;

                    //DAL.FinancialGuarantorInfo fing = null;
                    //using(var db = new CandidateDataManager())
                    //{
                    //    fing = db.GetFinancialGuarantorByCandidateID_ND(cId);
                    //}

                    //if(fing != null)
                    //{
                    //    if (
                    //        string.IsNullOrEmpty(fing.Name) ||
                    //        string.IsNullOrEmpty(fing.RelationWithCandidate) ||
                    //        string.IsNullOrEmpty(fing.Occupation) ||
                    //        string.IsNullOrEmpty(fing.Organization) ||
                    //        string.IsNullOrEmpty(fing.Designation) ||
                    //        string.IsNullOrEmpty(fing.MailingAddress) ||
                    //        string.IsNullOrEmpty(fing.Email) ||
                    //        string.IsNullOrEmpty(fing.Mobile) ||
                    //        string.IsNullOrEmpty(fing.SourceOfFund) 
                    //        )
                    //    {
                    //        fingStr += "Some Financial Guarantor information missing.";
                    //    }
                    //    else
                    //    {
                    //        fingStr += string.Empty;
                    //    }
                    //}
                    //else
                    //{
                    //    lblMessage.Text = "Error getting Financial Guarantor.";
                    //    return;
                    //}

                    #endregion

                    #region ATTACHMENT

                    string attachStr = string.Empty;

                    List<DAL.Document> doc = null;
                    using (var db = new CandidateDataManager())
                    {
                        doc = db.GetAllDocumentByCandidateID_AD(cId);
                    }

                    if (doc != null)
                    {
                        DAL.DocumentDetail photo = doc.Where(c => c.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                        DAL.DocumentDetail sign = doc.Where(c => c.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();
                        //DAL.DocumentDetail fgsig = doc.Where(c => c.DocumentTypeID == 7).Select(c => c.DocumentDetail).FirstOrDefault();

                        if (photo != null)
                        {
                            if (
                                string.IsNullOrEmpty(photo.URL) ||
                                string.IsNullOrEmpty(photo.Name)
                                )
                            {
                                attachStr += "Photo is missing";
                            }
                            else
                            {
                                attachStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Photo is not available.";
                            return;
                        }

                        if (sign != null)
                        {
                            if (
                                string.IsNullOrEmpty(sign.URL) ||
                                string.IsNullOrEmpty(sign.Name)
                                )
                            {
                                attachStr += "Signature is missing";
                            }
                            else
                            {
                                attachStr += string.Empty;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Signature is not available.";
                            return;
                        }

                        //if (fgsig != null)
                        //{
                        //    if (
                        //        string.IsNullOrEmpty(fgsig.URL) ||
                        //        string.IsNullOrEmpty(fgsig.Name)
                        //        )
                        //    {
                        //        attachStr += "Financial Guarantor signature is missing.";
                        //    }
                        //    else
                        //    {
                        //        attachStr += string.Empty;
                        //    }
                        //}
                        //else
                        //{
                        //    lblMessage.Text = "Error getting financial guarantor signature.";
                        //    return;
                        //}
                    }
                    else
                    {
                        lblMessage.Text = "Photo and signature is not available. Please upload photo and signature.";
                        return;
                    }

                    #endregion

                    #region Exam Venue Selection
                    string examVenueSelectionStr = string.Empty;
                    try
                    {
                        //int educationCategoryId = -1;
                        //using (var db = new CandidateDataManager())
                        //{
                        //    educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                        //}
                        if (educationCategoryId == 4)
                        {
                            int notSelectedVenueCount = 0;
                            List<DAL.CandidateFacultyWiseDistrictSeat> fwdsList = null;
                            List<DAL.CandidateFormSl> formSerialList = null;
                            using (var db = new CandidateDataManager())
                            {
                                fwdsList = db.AdmissionDB.CandidateFacultyWiseDistrictSeats.Where(x => x.CandidateId == cId).ToList();
                                formSerialList = db.GetAllCandidateFormSlByCandID_AD(cId).ToList();
                            }

                            if (fwdsList != null && fwdsList.Count > 0)
                            {
                                foreach (var tData in formSerialList)
                                {
                                    DAL.CandidateFacultyWiseDistrictSeat model = fwdsList.Where(x => x.AdmissionSetupId == tData.AdmissionSetupID).FirstOrDefault();

                                    if (model != null)
                                    {

                                    }
                                    else
                                    {
                                        notSelectedVenueCount++;

                                    }
                                }

                                if (notSelectedVenueCount > 0)
                                {
                                    examVenueSelectionStr = "Please select exam venue selection information for all faculties in Basic information section.";
                                }
                                else
                                {
                                    examVenueSelectionStr = string.Empty;
                                }
                            }
                            else
                            {
                                examVenueSelectionStr = "Please select exam venue selection information for all faculties in Basic information section.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        examVenueSelectionStr = "Failed to get exam venue selection info";
                    }
                    #endregion


                    //-------------------------------
                    //TODO: NEED TO ADD PRIORITY
                    //if one or more string is empty, then dont update
                    if (
                        !string.IsNullOrEmpty(basicStr) ||
                        !string.IsNullOrEmpty(educationStr) ||
                        !string.IsNullOrEmpty(choiceStr) ||
                        !string.IsNullOrEmpty(relationStr) ||
                        !string.IsNullOrEmpty(addressStr) ||
                        !string.IsNullOrEmpty(additionalStr) ||
                        !string.IsNullOrEmpty(attachStr) ||
                        !string.IsNullOrEmpty(examVenueSelectionStr)
                        )
                    {

                        string msg = "";

                        msg += "<strong>Your application form is not complete.<strong><br/>";
                        if (!string.IsNullOrEmpty(basicStr))
                        {
                            msg += "* " + basicStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(choiceStr))
                        {
                            msg += "* " + choiceStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(educationStr))
                        {
                            msg += "* " + educationStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(relationStr))
                        {
                            msg += "* " + relationStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(addressStr))
                        {
                            msg += "* " + addressStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(additionalStr))
                        {
                            msg += "* " + additionalStr + "<br/>";
                        }
                        //if (!string.IsNullOrEmpty(fingStr))
                        //{
                        //    lblMessage.Text += "* " + fingStr + "<br/>";
                        //}
                        if (!string.IsNullOrEmpty(attachStr))
                        {
                            msg += "* " + attachStr + "<br/>";
                        }
                        if (!string.IsNullOrEmpty(examVenueSelectionStr))
                        {
                            msg += "* " + examVenueSelectionStr + "<br/>";
                        }



                        MessageView(msg, "fail");

                    }
                    else // strings are empty..update.
                    {
                        DAL.AdditionalInfo additionalInfoObj = null;
                        using (var db = new CandidateDataManager())
                        {
                            additionalInfoObj = db.GetAdditionalInfoByCandidateID_ND(cId);
                        }

                        if (additionalInfoObj != null)
                        {
                            additionalInfoObj.IsFinalSubmit = true;

                            additionalInfoObj.DateModified = DateTime.Now;
                            additionalInfoObj.ModifiedBy = uId;
                            try
                            {
                                long additionalInfo = -1;
                                using (var dbUpdate = new CandidateDataManager())
                                {
                                    dbUpdate.Update<DAL.AdditionalInfo>(additionalInfoObj);
                                    additionalInfo = additionalInfoObj.ID;
                                }

                                if (additionalInfo > 0)
                                {

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Final Submit (Admin)";
                                        //dLog.PageName = "CandApplicationDeclaration.aspx";
                                        //dLog.NewData = "User: " + userName + "; Final Submit to ," + "Candidate: " + candidate.FirstName.ToString() + "; PaymentID: " + paymentID.ToString() + "; CandidateID: " + cId.ToString();
                                        //dLog.UserId = uId;
                                        //dLog.Attribute1 = "Success";
                                        //dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        //    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        //LogWriter.DataLogWriter(dLog);

                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Final Submit (Admin)";
                                        dLog.PageName = "CandApplicationDeclaration.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Final Submit to ," + "Candidate: " + candidate.FirstName.ToString() + "; PaymentID: " + paymentID.ToString() + "; CandidateID: " + cId.ToString() + "; Final Submitted.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion


                                    #region Send SMS
                                    string msgBody = "Dear " + candidate.FirstName +
                                                                                  "Your application has been successfully submitted." +
                                                                                  "BUP";
                                    string smsRespose = SMSUtility.Send(candidate.SMSPhone, msgBody);

                                    string statusT = JObject.Parse(smsRespose)["statusCode"].ToString();

                                    if (statusT != "200") //if sms sending fails
                                    {
                                        DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                                        smsLog.AcaCalId = null;
                                        smsLog.Attribute1 = "Sms sending failed in CandApplicationDeclaration.aspx";
                                        smsLog.Attribute2 = "Failed";
                                        smsLog.Attribute3 = null;
                                        smsLog.CreatedBy = candidate.ID;
                                        smsLog.CreatedDate = DateTime.Now;
                                        smsLog.CurrentSMSReferenceNo = smsRespose;
                                        smsLog.Message = msgBody;
                                        smsLog.StudentId = candidate.ID;
                                        smsLog.PhoneNo = candidate.SMSPhone;
                                        smsLog.SenderUserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                                        smsLog.SentReferenceId = null;
                                        smsLog.SentSMSId = null;
                                        smsLog.SmsSendDate = DateTime.Now;
                                        smsLog.SmsType = -1;

                                        //LogWriter.SmsLog(smsLog);

                                        //lblMessageLv.Text = "SMS sending failed.";
                                        //lblMessageLv.ForeColor = Color.Crimson;
                                    }
                                    else //if sms sending passed
                                    {
                                        DAL_Log.SmsLog smsLog = new DAL_Log.SmsLog();
                                        smsLog.AcaCalId = null;
                                        smsLog.Attribute1 = "Sms sending successful CandApplicationDeclaration.aspx";
                                        smsLog.Attribute2 = "Success";
                                        smsLog.Attribute3 = null;
                                        smsLog.CreatedBy = candidate.ID;
                                        smsLog.CreatedDate = DateTime.Now;
                                        smsLog.CurrentSMSReferenceNo = smsRespose;
                                        smsLog.Message = msgBody;
                                        smsLog.StudentId = candidate.ID;
                                        smsLog.PhoneNo = candidate.SMSPhone;
                                        smsLog.SenderUserId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                                        smsLog.SentReferenceId = null;
                                        smsLog.SentSMSId = null;
                                        smsLog.SmsSendDate = DateTime.Now;
                                        smsLog.SmsType = -1;

                                        LogWriter.SmsLog(smsLog);

                                        //lblMessageLv.Text = "SMS sent.";
                                        //lblMessageLv.ForeColor = Color.Green;
                                    } 
                                    #endregion

                                    LoadCandidateData(cId);
                                    MessageView("Final Submission Done.", "success");

                                }

                                ////logout user with message.
                                //SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                                //SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                                //SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                                //SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                                //SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                                ////Response.Redirect("~/Admission/Home.aspx", false);
                                //Response.Redirect("~/Admission/Message.aspx?message=Successful. User Logged Out.&type=success", false);
                            }
                            catch (Exception ex)
                            {
                                MessageView("Error: Something went wrong. Exception: " + ex.Message.ToString(), "fail");
                            }
                        }
                    }

                } //end if (cid > 0 && uid > 0)
                else
                {
                    MessageView("Invalid Request!", "fail");
                }
            }// end if else (chbxAgree.Checked)
            //btnSave_Declaration.Enabled = true;
        }
        
        #region N/A
        //private void LoadInstituteData()
        //{
        //    using (var db = new GeneralDataManager())
        //    {
        //        DAL.Institute institute = new DAL.Institute();
        //        institute = db.AdmissionDB.Institutes.Find(1);
        //        if (institute != null || institute.ID > 0)
        //        {
        //            lblUniShortName.Text = institute.ShortName;
        //        }
        //    }
        //}

        //private void LoadCandidateData(long cId)
        //{
        //    if (cId > 0)
        //    {

        //        List<DAL.CandidateFormSl> candidateFormSlList = null;
        //        DAL.AdmissionSetup admSetup = null;

        //        using (var db = new CandidateDataManager())
        //        {
        //            candidateFormSlList = db.GetAllCandidateFormSlByCandID_AD(cId);
        //            if (candidateFormSlList != null)
        //            {
        //                //get only admSetup for masters.
        //                admSetup = candidateFormSlList.Where(c => c.AdmissionSetup.EducationCategoryID == 6).Select(c => c.AdmissionSetup).FirstOrDefault();
        //            }
        //        }

        //        if (admSetup != null) //if it is a masters candidate, then show occupation details and other info.
        //        {
        //            txtCandidateAnnualIncome.Enabled = true;
        //            ddlAdmittedBefore.Enabled = true;
        //            panel_Occupation.Visible = true;
        //        }
        //        else //bachelors candidate, do not show occupation details and other info
        //        {
        //            txtCandidateAnnualIncome.Enabled = false;
        //            ddlAdmittedBefore.SelectedItem.Text = "No";
        //            ddlAdmittedBefore.Enabled = false;
        //            panel_Occupation.Visible = false;
        //        }

        //        //additional info
        //        using (var db = new CandidateDataManager())
        //        {
        //            DAL.AdditionalInfo addInfo = db.GetAdditionalInfoByCandidateID_ND(cId);

        //            if (addInfo != null)
        //            {
        //                if (addInfo.CurrentStudentId == null)
        //                {
        //                    ddlAdmittedBefore.SelectedValue = "No";
        //                    txtCurrentStudentId.Text = null;
        //                    txtCurrentStudentId.Enabled = false;
        //                }
        //                else if (addInfo.CurrentStudentId != null)
        //                {
        //                    ddlAdmittedBefore.SelectedValue = "Yes";
        //                    txtCurrentStudentId.Text = addInfo.CurrentStudentId;
        //                    txtCurrentStudentId.Enabled = true;
        //                }
        //                else
        //                {
        //                    ddlAdmittedBefore.SelectedValue = "-1";
        //                    txtCurrentStudentId.Text = null;
        //                    txtCurrentStudentId.Enabled = false;
        //                }

        //                if (addInfo.CandidateIncome != null)
        //                {
        //                    txtCandidateAnnualIncome.Text = addInfo.CandidateIncome.ToString();
        //                }
        //                else
        //                {
        //                    txtCandidateAnnualIncome.Text = null;
        //                }
        //                if (addInfo.FatherAnnualIncome != null)
        //                {
        //                    txtFatherAnnualIncome.Text = addInfo.FatherAnnualIncome.ToString();
        //                }
        //                else
        //                {
        //                    txtFatherAnnualIncome.Text = null;
        //                }
        //                if (addInfo.MotherAnnualIncome != null)
        //                {
        //                    txtMotherAnnualIncome.Text = addInfo.MotherAnnualIncome.ToString();
        //                }
        //                else
        //                {
        //                    txtMotherAnnualIncome.Text = null;
        //                }
        //            }
        //        }// end using additional info

        //        //work experience
        //        using (var db = new CandidateDataManager())
        //        {
        //            DAL.WorkExperience workExpObj = db.GetWorkExperienceByCandidateID_ND(cId);
        //            if (workExpObj != null)
        //            {
        //                txtWorkDesignation.Text = workExpObj.Designation;
        //                txtWorkOrganization.Text = workExpObj.Organization;
        //                txtWorkAddress.Text = workExpObj.OrgAddress;
        //                txtStartDateWE.Text = workExpObj.StartDate != null ? workExpObj.StartDate.Value.ToString("dd/MM/yyyy") : null;
        //                txtEndDateWE.Text = workExpObj.EndDate != null ? workExpObj.EndDate.Value.ToString("dd/MM/yyyy") : null;
        //            }

        //        } // end using work experience

        //        //extra curricular activity
        //        using (var db = new CandidateDataManager())
        //        {
        //            List<DAL.ExtraCurricularActivity> extraCurActList = db.GetAllExtraCurricularActivityByCandidateID_ND(cId);
        //            if (extraCurActList != null)
        //            {
        //                DAL.ExtraCurricularActivity activity1 = extraCurActList.Where(c => c.Attribute1 == "1").FirstOrDefault();
        //                DAL.ExtraCurricularActivity activity2 = extraCurActList.Where(c => c.Attribute1 == "2").FirstOrDefault();

        //                if (activity1 != null)
        //                {
        //                    txtActivity1.Text = activity1.Activity;
        //                    txtAward1.Text = activity1.Award;
        //                    txtEcaDate1.Text = activity1.DateRecieved != null ? activity1.DateRecieved.Value.ToString("dd/MM/yyyy") : null;
        //                }

        //                if (activity2 != null)
        //                {
        //                    txtActivity2.Text = activity2.Activity;
        //                    txtAward2.Text = activity2.Award;
        //                    txtEcaDate2.Text = activity2.DateRecieved != null ? activity2.DateRecieved.Value.ToString("dd/MM/yyyy") : null;
        //                }
        //            }
        //        } //end using extra curricular activity

        //    }// if(cId > 0)
        //}

        //private void LoadDDL()
        //{
        //    ddlAdmittedBefore.Items.Clear();
        //    ddlAdmittedBefore.Items.Add(new ListItem("Select", "-1"));
        //    ddlAdmittedBefore.Items.Add(new ListItem("Yes", "Yes"));
        //    ddlAdmittedBefore.Items.Add(new ListItem("No", "No"));
        //}

        //protected void ddlAdmittedBefore_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlAdmittedBefore.SelectedValue == "Yes")
        //    {
        //        txtCurrentStudentId.Enabled = true;
        //    }
        //    else
        //    {
        //        txtCurrentStudentId.Enabled = false;
        //        txtCurrentStudentId.Text = null;
        //    }
        //}

        //protected void btnSave_Additional_Click(object sender, EventArgs e)
        //{
        //    long cId = -1;
        //    if ((string.IsNullOrEmpty(Request.QueryString["val"])))
        //    {
        //        Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
        //    }
        //    else
        //    {
        //        cId = Int64.Parse(Request.QueryString["val"].ToString());
        //    }

        //    try
        //    {
        //        if (cId > 0)
        //        {
        //            DAL.AdditionalInfo addInfoObj = null;
        //            using (var db = new CandidateDataManager())
        //            {
        //                addInfoObj = db.GetAdditionalInfoByCandidateID_ND(cId);
        //            }
        //            if (addInfoObj != null)  //additional info exist. Update add info
        //            {
        //                if (ddlAdmittedBefore.SelectedValue == "Yes")
        //                {
        //                    addInfoObj.CurrentStudentId = txtCurrentStudentId.Text.Trim();
        //                    addInfoObj.IsEnrolled = true;
        //                }
        //                else if (ddlAdmittedBefore.SelectedValue == "No")
        //                {
        //                    addInfoObj.CurrentStudentId = null;
        //                    addInfoObj.IsEnrolled = false;
        //                }

        //                if (!string.IsNullOrEmpty(txtCandidateAnnualIncome.Text.Trim()))
        //                {
        //                    addInfoObj.CandidateIncome = Decimal.Parse(txtCandidateAnnualIncome.Text.Trim());
        //                }
        //                else
        //                {
        //                    addInfoObj.CandidateIncome = null;
        //                }
        //                if (!string.IsNullOrEmpty(txtFatherAnnualIncome.Text.Trim()))
        //                {
        //                    addInfoObj.FatherAnnualIncome = Decimal.Parse(txtFatherAnnualIncome.Text.Trim());
        //                }
        //                else
        //                {
        //                    addInfoObj.FatherAnnualIncome = null;
        //                }
        //                if (!string.IsNullOrEmpty(txtMotherAnnualIncome.Text.Trim()))
        //                {
        //                    addInfoObj.MotherAnnualIncome = Decimal.Parse(txtMotherAnnualIncome.Text.Trim());
        //                }
        //                else
        //                {
        //                    addInfoObj.MotherAnnualIncome = null;
        //                }

        //                addInfoObj.DateModified = DateTime.Now;
        //                addInfoObj.ModifiedBy = cId;

        //                using (var dbUpdate = new CandidateDataManager())
        //                {
        //                    dbUpdate.Update<DAL.AdditionalInfo>(addInfoObj);
        //                }
        //            }
        //            else //additional info does not exist. Create new.
        //            {
        //                DAL.AdditionalInfo _newAddInfo = new DAL.AdditionalInfo();

        //                if (ddlAdmittedBefore.SelectedValue == "Yes")
        //                {
        //                    _newAddInfo.CurrentStudentId = txtCurrentStudentId.Text.Trim();
        //                    _newAddInfo.IsEnrolled = true;
        //                }
        //                else if (ddlAdmittedBefore.SelectedValue == "No")
        //                {
        //                    _newAddInfo.CurrentStudentId = null;
        //                    _newAddInfo.IsEnrolled = false;
        //                }

        //                if (!string.IsNullOrEmpty(txtCandidateAnnualIncome.Text.Trim()))
        //                {
        //                    _newAddInfo.CandidateIncome = Decimal.Parse(txtCandidateAnnualIncome.Text.Trim());
        //                }
        //                else
        //                {
        //                    _newAddInfo.CandidateIncome = null;
        //                }
        //                if (!string.IsNullOrEmpty(txtFatherAnnualIncome.Text.Trim()))
        //                {
        //                    _newAddInfo.FatherAnnualIncome = Decimal.Parse(txtFatherAnnualIncome.Text.Trim());
        //                }
        //                else
        //                {
        //                    _newAddInfo.FatherAnnualIncome = null;
        //                }
        //                if (!string.IsNullOrEmpty(txtMotherAnnualIncome.Text.Trim()))
        //                {
        //                    _newAddInfo.MotherAnnualIncome = Decimal.Parse(txtMotherAnnualIncome.Text.Trim());
        //                }
        //                else
        //                {
        //                    _newAddInfo.MotherAnnualIncome = null;
        //                }

        //                _newAddInfo.CandidateID = cId;
        //                _newAddInfo.CreatedBy = cId;
        //                _newAddInfo.DateCreated = DateTime.Now;

        //                using (var dbInsert = new CandidateDataManager())
        //                {
        //                    dbInsert.Insert<DAL.AdditionalInfo>(_newAddInfo);
        //                }
        //            }

        //            //for OccupationDetails
        //            DAL.WorkExperience workExpObj = null;
        //            using (var db = new CandidateDataManager())
        //            {
        //                workExpObj = db.GetWorkExperienceByCandidateID_ND(cId);
        //            }
        //            if (workExpObj != null) // work exp exist, update work exp
        //            {
        //                if (!string.IsNullOrEmpty(txtWorkDesignation.Text.Trim()) && !string.IsNullOrEmpty(txtWorkOrganization.Text.Trim()))
        //                {
        //                    workExpObj.Designation = txtWorkDesignation.Text.Trim();
        //                    workExpObj.Organization = txtWorkOrganization.Text.Trim();
        //                    workExpObj.OrgAddress = txtWorkAddress.Text.Trim();

        //                    if (txtStartDateWE.Text.Trim() != "")
        //                    {
        //                        workExpObj.StartDate = DateTime.ParseExact(txtStartDateWE.Text, "dd/MM/yyyy", null);
        //                    }
        //                    else
        //                    {
        //                        workExpObj.StartDate = null;
        //                    }

        //                    if (txtEndDateWE.Text.Trim() != "")
        //                    {
        //                        workExpObj.EndDate = DateTime.ParseExact(txtEndDateWE.Text, "dd/MM/yyyy", null);
        //                    }
        //                    else
        //                    {
        //                        workExpObj.EndDate = null;
        //                    }

        //                    workExpObj.ModifiedBy = cId;
        //                    workExpObj.DateModified = DateTime.Now;

        //                    using (var dbUpdate = new CandidateDataManager())
        //                    {
        //                        dbUpdate.Update<DAL.WorkExperience>(workExpObj);
        //                    }
        //                }
        //            }
        //            else // work exp does not exist, create new
        //            {
        //                if (!string.IsNullOrEmpty(txtWorkDesignation.Text.Trim()) && !string.IsNullOrEmpty(txtWorkOrganization.Text.Trim()))
        //                {
        //                    DAL.WorkExperience newWorkExpObj = new DAL.WorkExperience();
        //                    newWorkExpObj.Designation = txtWorkDesignation.Text.Trim();
        //                    newWorkExpObj.Organization = txtWorkOrganization.Text.Trim();
        //                    newWorkExpObj.OrgAddress = txtWorkAddress.Text.Trim();

        //                    if (txtStartDateWE.Text.Trim() != "")
        //                    {
        //                        newWorkExpObj.StartDate = DateTime.ParseExact(txtStartDateWE.Text, "dd/MM/yyyy", null);
        //                    }
        //                    else
        //                    {
        //                        newWorkExpObj.StartDate = null;
        //                    }

        //                    if (txtEndDateWE.Text.Trim() != "")
        //                    {
        //                        newWorkExpObj.EndDate = DateTime.ParseExact(txtEndDateWE.Text, "dd/MM/yyyy", null);
        //                    }
        //                    else
        //                    {
        //                        newWorkExpObj.EndDate = null;
        //                    }

        //                    newWorkExpObj.CandidateID = cId;
        //                    newWorkExpObj.CreatedBy = cId;
        //                    newWorkExpObj.DateCreated = DateTime.Now;

        //                    using (var dbInsert = new CandidateDataManager())
        //                    {
        //                        dbInsert.Insert<DAL.WorkExperience>(newWorkExpObj);
        //                    }
        //                }
        //            }

        //            //for Extracurricular Activity
        //            List<DAL.ExtraCurricularActivity> extraCurricularList = null;

        //            DAL.ExtraCurricularActivity extraCurricularAct1 = null;
        //            DAL.ExtraCurricularActivity extraCurricularAct2 = null;

        //            using (var db = new CandidateDataManager())
        //            {
        //                extraCurricularList = db.GetAllExtraCurricularActivityByCandidateID_ND(cId);
        //                if (extraCurricularList != null)
        //                {
        //                    extraCurricularAct1 = extraCurricularList.Where(c => c.Attribute1 == "1").FirstOrDefault();
        //                    extraCurricularAct2 = extraCurricularList.Where(c => c.Attribute1 == "2").FirstOrDefault();
        //                }
        //            }

        //            //activity 1
        //            if (!string.IsNullOrEmpty(txtActivity1.Text.Trim()))
        //            {
        //                if (extraCurricularAct1 != null) //extra curricular activity 1 exists, update.
        //                {
        //                    extraCurricularAct1.Activity = string.IsNullOrEmpty(txtActivity1.Text) == true ? null : txtActivity1.Text;
        //                    extraCurricularAct1.Award = string.IsNullOrEmpty(txtAward1.Text) == true ? null : txtAward1.Text;
        //                    extraCurricularAct1.DateRecieved = DateTime.ParseExact(txtEcaDate1.Text, "dd/MM/yyyy", null);

        //                    extraCurricularAct1.DateModified = DateTime.Now;
        //                    extraCurricularAct1.ModifiedBy = cId;

        //                    using (var dbUpdateECA1 = new CandidateDataManager())
        //                    {
        //                        dbUpdateECA1.Update<DAL.ExtraCurricularActivity>(extraCurricularAct1);
        //                    }
        //                }
        //                else //extra curricula activity 1 does not exist. create new.
        //                {
        //                    DAL.ExtraCurricularActivity newExtraCurricularAct1 = new DAL.ExtraCurricularActivity();

        //                    newExtraCurricularAct1.Activity = string.IsNullOrEmpty(txtActivity1.Text) == true ? null : txtActivity1.Text;
        //                    newExtraCurricularAct1.Award = string.IsNullOrEmpty(txtAward1.Text) == true ? null : txtAward1.Text;
        //                    newExtraCurricularAct1.DateRecieved = DateTime.ParseExact(txtEcaDate1.Text, "dd/MM/yyyy", null);
        //                    newExtraCurricularAct1.Attribute1 = "1";

        //                    newExtraCurricularAct1.CandidateID = cId;
        //                    newExtraCurricularAct1.CreatedBy = cId;
        //                    newExtraCurricularAct1.DateCreated = DateTime.Now;

        //                    if (!string.IsNullOrEmpty(txtActivity1.Text))
        //                    {
        //                        using (var dbInsertECA1 = new CandidateDataManager())
        //                        {
        //                            dbInsertECA1.Insert<DAL.ExtraCurricularActivity>(newExtraCurricularAct1);
        //                        }
        //                    }
        //                }
        //            }


        //            //activity2
        //            if (!string.IsNullOrEmpty(txtActivity2.Text.Trim()))
        //            {
        //                if (extraCurricularAct2 != null) //extra curricular activity 2 exists, update.
        //                {
        //                    extraCurricularAct2.Activity = string.IsNullOrEmpty(txtActivity2.Text) == true ? null : txtActivity2.Text;
        //                    extraCurricularAct2.Award = string.IsNullOrEmpty(txtAward2.Text) == true ? null : txtAward2.Text;
        //                    extraCurricularAct2.DateRecieved = DateTime.ParseExact(txtEcaDate2.Text, "dd/MM/yyyy", null);

        //                    extraCurricularAct2.DateModified = DateTime.Now;
        //                    extraCurricularAct2.ModifiedBy = cId;

        //                    using (var dbUpdateECA2 = new CandidateDataManager())
        //                    {
        //                        dbUpdateECA2.Update<DAL.ExtraCurricularActivity>(extraCurricularAct2);
        //                    }
        //                }
        //                else //extra curricula activity 2 does not exist. create new.
        //                {
        //                    DAL.ExtraCurricularActivity newExtraCurricularAct2 = new DAL.ExtraCurricularActivity();

        //                    newExtraCurricularAct2.Activity = string.IsNullOrEmpty(txtActivity2.Text) == true ? null : txtActivity2.Text;
        //                    newExtraCurricularAct2.Award = string.IsNullOrEmpty(txtAward2.Text) == true ? null : txtAward2.Text;
        //                    newExtraCurricularAct2.DateRecieved = DateTime.ParseExact(txtEcaDate2.Text, "dd/MM/yyyy", null);
        //                    newExtraCurricularAct2.Attribute1 = "2";

        //                    newExtraCurricularAct2.CandidateID = cId;
        //                    newExtraCurricularAct2.CreatedBy = cId;
        //                    newExtraCurricularAct2.DateCreated = DateTime.Now;

        //                    if (!string.IsNullOrEmpty(txtActivity1.Text))
        //                    {
        //                        using (var dbInsertECA2 = new CandidateDataManager())
        //                        {
        //                            dbInsertECA2.Insert<DAL.ExtraCurricularActivity>(newExtraCurricularAct2);
        //                        }
        //                    }
        //                }
        //            }

        //        }

        //        lblMessageAdditional.Text = "[Admin] Info Updated successfully.";
        //        messagePanel_Additional.CssClass = "alert alert-success";
        //        messagePanel_Additional.Visible = true;


        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Redirect("~/Admission/Message.aspx?message=Unable to save/update candidate information. Error Code : F01X012TC&type=danger", false);
        //        lblMessageAdditional.Text = "[Admin] Unable to save/update candidate information. " + ex.Message;
        //        messagePanel_Additional.CssClass = "alert alert-danger";
        //        messagePanel_Additional.Visible = true;
        //    }
        //} 
        #endregion

    }
}