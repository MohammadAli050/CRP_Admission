using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate.Prints
{
    public partial class AdmitCard : PageBase
    {

        //For Bachelor
        //=========================
        //Important Instructions:
        //1. Use black pen only.
        //2. Use of cell phone, calculator, watch and other electronic gadget in exam hall is strictly prohibited.
        //3. Bring colour print out of ADMIT CARD.
        //4. Consider road condition and traffic jam at Mirpur-12 to reach exam venue in time.



        //MBA(Professional):
        //========================
        //You have to write an essay for MBA(Professional) Program Admission September 2020 which should cover the following issues sequentially(Use separate para for each of the four components.You do not need to give any title before each para):

        //1. You should mention your name, Admission Test Roll number, Educational Back ground and Work experience.
        //2. You should write why you want to pursue MBA in BUP and how you perceive that it will add value in your professional life.
        //3. You should write a potential social business idea of yours in the post Covid19 period.Your business idea should help us to achieve SDGs (one or more than one SDGs) and should serve the causes of humanity.Please put maximum emphasize in this section.Try to provide details of your idea within word limit.
        //4. You should write why you think yourself to be the most appropriate candidate for MBA (Professional) program offered in BUP.

        //Format:
        //You can write maximum 1500 words.More than 1500 words will negatively impact your credentials.Font size should be 11 and Times New Roman Font should be used. Line space should be 1.5.
        //Use Header with your Name and Admission ID and Use Footer with page number. Convert it into pdf format.Rename your files with your Admission ID.

        //Mail both word doc and pdf file at mbaprofessionalbup @gmail.com within 29th July 2020. Your email subject line should be 'Essay_Your roll number_Your Name'. Short listed candidates will be called for viva online through mobile text.

        //===================================================================

//        ভর্তি পরীক্ষায় অংশগ্রহণকারী শিক্ষার্থীদের জন্য জরুরী নোটিশ

// ১। অত্র ফ্যাকাল্টির অধীনে পরিচালিত এমবিএ(প্রফেশনাল) প্রোগ্রাম এর মে-২০২১ ট্রাইমেস্টারের ভর্তি পরীক্ষা আগামী ২৭ মার্চ ২০২১ সকাল ১০০০ ঘটিকায় অনুষ্ঠিত হবে। নিম্নে ভর্তি পরীক্ষার দিক নিদের্শনা প্রদান করা হলোঃ


//ক.ভর্তি পরীক্ষা Zoom ভিডিও স্ট্রিমিং এর মাধ্যমে পরিচালিত হবে। 

//খ.ভর্তি পরীক্ষায় অংশগ্রহনের জন্য সকল পরীক্ষার্থীর নিজস্ব Gmail Account থাকতে হবে। না থাকলে Gmail Account Open করে আগামী ২৪ মার্চ ২০২১ তারিখ সকাল ১০০০ ঘটিকার মধ্যে রোল নম্বরসহ(শুধুমাত্র যাদের Gmail Account নাই) রোল নং দিয়ে ফাইল সেভ করে mbaprofessionalbup @gmail.com এ প্রেরণ করতে হবে।


//গ.ভর্তি পরীক্ষার জন্য Google Link এর মাধ্যমে প্রশ্নপত্র যথাসময়ে প্রদান করা হবে। ভর্তি পরীক্ষা সংক্রান্ত বিভিন্ন তথ্য সংগ্রহের জন্য নিয়মিত ইমেইল Check করায় জন্য পরামর্শ দেয়া হলো।


//ঘ.ভর্তি পরীক্ষার জন্য মোবাইল ব্যবহার করলে ২টি ডিভাইজ (একটিতে Zoom অন্যটিতে প্রশ্নের উত্তর প্রদান) প্রয়োজন হবে। অন্যথায় Laptop অথবা Webcam সম্বলিত Desktop হলে ১টি ডিভাইজ-এ পরীক্ষা দেয়া যাবে ।

//ঙ.পরীক্ষার্থীকে অবশ্যই পরীক্ষা শুরু হবার নূন্যতম ৩০ মিনিট পূর্বে রোল নং অনুযায়ী নির্দিষ্ট Zoom Meeting Room এ সংযুক্ত হতে হবে।
 
//চ.সংশ্লিষ্ট রুম আইডির ইনভিজিলেটরগণ পরীক্ষার ১৫ মিনিট পূর্বে Zoomlink Open করবেন ও ছবিযুক্ত নামীয় তালিকার রোল মোতাবেক উপস্থিতি নিশ্চিত করবেন।

//ছ.MCQ Type প্রশ্নপত্রে ৫০টি প্রশ্ন থাকবে যার জন্য সময় হবে ৪৫ মিনিট। ভুল উত্তর এর জন্য ০.২৫ নাম্বার কাঁটা যাবে।

//জ.নির্ধারিত সময়ে প্রশ্নপত্র পাওয়া যাবে এবং সময় শেষে স্বয়ংক্রিয়ভাবে বন্ধ হয়ে যাবে। তাই অবশ্যই যথাসময়ে উত্তরপত্র Submit করতে হবে।

//ঝ.উত্তরপত্র Submission এর পর পরীক্ষার পর্যবেক্ষকগণ না বলা পর্যন্ত Zoom ত্যাগ করা যাবে না।

//ঞ.ইন্টারনেট সংযোগের অসুবিধার ক্ষেত্রে শিক্ষার্থীদের জরুরী সহায়তা হিসাবে বিকল্প ইন্টারনেট সংযোগ(মোবাইল ডাটা) ও মোবাইল অথবা ল্যাপটপে পরিপূর্ণ চার্জ রাখতে পরামর্শ দেয়া যাচ্ছে।

//ট.পরীক্ষার সময় যদি বিদ্যুৎ অথবা ইন্টারনেট সংযোগ বিচ্ছিন্ন হয়ে যায় তবে তাৎক্ষণিকভাবে তা অবশ্যই Zoom Meeting-এ সংশিষ্ট শিক্ষক/পরিদর্শক কে অবহিত করতে হবে।

//ঠ.পরীক্ষা চলাকালীন সময়ে মার্জিত পোষাক পরিধান করতে হবে।

//ড.বিশেষ কোন সমস্যা দেখা দিলে সংশ্লিষ্ট পর্যবেক্ষকের নাম্বার দেওয়া থাকবে তার সাথে যোগাযোগ করতে হবে ।

        



        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;
        long cIdVal = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cIdVal = Int64.Parse(decryptedQueryVal);
            }


            if (!IsPostBack)
            {
                LoadCandidateData();
            }
        }


        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        private void LoadCandidateData()
        {
            long cId = -1;

            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
            }
            else
            {
                Response.Redirect("~/Admission/Home.aspx", false);
            }

            if (cId == cIdVal)
            {
                if (cId > 0)
                {
                    string validityCheck = string.Empty;

                    //might be needed in the future for undergrad admission
                    #region N/A -- Temporary Block 

                    //List<DAL.SP_Test_Result> tempList = null;
                    //try
                    //{
                    //    using (var db = new OfficeDataManager())
                    //    {
                    //        tempList = db.AdmissionDB.SP_Test().Where(c => c.ID == cId && c.IsApproved == false).ToList();
                    //    }
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}

                    //if (tempList != null)
                    //{
                    //    if (tempList.Count() > 0)
                    //    {
                    //        lblTemp.Text = string.Empty;
                    //        foreach (DAL.SP_Test_Result item in tempList)
                    //        {
                    //            lblTemp.Text += "You are not eligible for " + item.UnitName + ".<br/>";
                    //        }
                    //        if (!string.IsNullOrEmpty(lblTemp.Text))
                    //        {
                    //            lblTemp.Text += "Sorry for Inconvenience.";
                    //            navUrl.Visible = true;
                    //        }
                    //        else
                    //        {
                    //            lblTemp.Text = string.Empty;
                    //            //navUrl.Visible = false;
                    //        }
                    //    }
                    //}


                    #endregion

                    #region Father Name check

                    try
                    {
                        DAL.Relation fRelation = null;
                        DAL.RelationDetail fRelationDtls = null;
                        using (var db = new CandidateDataManager())
                        {
                            fRelation = db.AdmissionDB.Relations.Where(c => c.CandidateID == cId && c.RelationTypeID == 2).FirstOrDefault();
                        }

                        if (fRelation != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                fRelationDtls = db.AdmissionDB.RelationDetails.Where(c => c.ID == fRelation.RelationDetailsID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            validityCheck += "** Parent/Guardian info does not exist.<br/>";
                        }

                        if (fRelationDtls != null)
                        {
                            if (string.IsNullOrEmpty(fRelationDtls.Name))
                            {
                                validityCheck += "* Father name not available.<br/>";
                            }
                        }
                        else
                        {
                            validityCheck += "** Father info does not exist.<br/>";
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    #endregion

                    #region Photo & Signature Check

                    try
                    {
                        DAL.Document photoDoc = null;
                        DAL.Document signDoc = null;
                        DAL.DocumentDetail photoDocDetail = null;
                        DAL.DocumentDetail signDocDetail = null;

                        using (var db = new CandidateDataManager())
                        {
                            photoDoc = db.AdmissionDB.Documents.Where(c => c.CandidateID == cId && c.DocumentTypeID == 2).FirstOrDefault();
                            signDoc = db.AdmissionDB.Documents.Where(c => c.CandidateID == cId && c.DocumentTypeID == 3).FirstOrDefault();
                        }

                        if (photoDoc != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                photoDocDetail = db.AdmissionDB.DocumentDetails.Where(c => c.ID == photoDoc.DocumentDetailsID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            validityCheck += "** Photo does not exist.<br/>";
                        }

                        if (photoDocDetail != null)
                        {
                            if (string.IsNullOrEmpty(photoDocDetail.URL))
                            {
                                validityCheck += "* Photo not found. Please upload photo.<br/>";
                            }
                        }
                        else
                        {
                            validityCheck += "** Photo details does not exist.<br/>";
                        }

                        if (signDoc != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                signDocDetail = db.AdmissionDB.DocumentDetails.Where(c => c.ID == signDoc.DocumentDetailsID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            validityCheck += "** Signature does not exist.<br/>";
                        }

                        if (signDocDetail != null)
                        {
                            if (string.IsNullOrEmpty(signDocDetail.URL))
                            {
                                validityCheck += "* Signature not found. Please upload signature.<br/>";
                            }
                        }
                        else
                        {
                            validityCheck += "** Signature details does not exist.<br/>";
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    #endregion

                    List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list = null;
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            list = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateID(cId, null, null).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error: Unable to fetch details";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        hfMessage.Value = ex.Message;
                    }

                    if (string.IsNullOrEmpty(validityCheck))
                    {

                        #region Bind GridView
                        if (list != null)
                        {
                            if (list.Count() > 0)
                            {
                                gvAppliedProgs.DataSource = list;
                            }
                            else
                            {
                                gvAppliedProgs.DataSource = null;
                            }
                        }
                        else
                        {
                            gvAppliedProgs.DataSource = null;
                        }
                        gvAppliedProgs.DataBind();
                        #endregion

                    }
                    else
                    {
                        lblMessage.Text = validityCheck;
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }
            }
            else if (uRole == "ICT Center")
            {
                cId = cIdVal;

                if (cId > 0)
                {
                    string validityCheck = string.Empty;

                    //might be needed in the future for undergrad admission
                    #region Temporary Block 

                    //List<DAL.SP_Test_Result> tempList = null;
                    //try
                    //{
                    //    using (var db = new OfficeDataManager())
                    //    {
                    //        tempList = db.AdmissionDB.SP_Test().Where(c => c.ID == cId && c.IsApproved == false).ToList();
                    //    }
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}

                    //if (tempList != null)
                    //{
                    //    if (tempList.Count() > 0)
                    //    {
                    //        lblTemp.Text = string.Empty;
                    //        foreach (DAL.SP_Test_Result item in tempList)
                    //        {
                    //            lblTemp.Text += "You are not eligible for " + item.UnitName + ".<br/>";
                    //        }
                    //        if (!string.IsNullOrEmpty(lblTemp.Text))
                    //        {
                    //            lblTemp.Text += "Sorry for Inconvenience.";
                    //            navUrl.Visible = true;
                    //        }
                    //        else
                    //        {
                    //            lblTemp.Text = string.Empty;
                    //            //navUrl.Visible = false;
                    //        }
                    //    }
                    //}


                    #endregion

                    #region Father Name check

                    try
                    {
                        DAL.Relation fRelation = null;
                        DAL.RelationDetail fRelationDtls = null;
                        using (var db = new CandidateDataManager())
                        {
                            fRelation = db.AdmissionDB.Relations.Where(c => c.CandidateID == cId && c.RelationTypeID == 2).FirstOrDefault();
                        }

                        if (fRelation != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                fRelationDtls = db.AdmissionDB.RelationDetails.Where(c => c.ID == fRelation.RelationDetailsID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            validityCheck += "** Parent/Guardian info does not exist.<br/>";
                        }

                        if (fRelationDtls != null)
                        {
                            if (string.IsNullOrEmpty(fRelationDtls.Name))
                            {
                                validityCheck += "* Father name not available.<br/>";
                            }
                        }
                        else
                        {
                            validityCheck += "** Father info does not exist.<br/>";
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    #endregion

                    #region Photo & Signature Check

                    try
                    {
                        DAL.Document photoDoc = null;
                        DAL.Document signDoc = null;
                        DAL.DocumentDetail photoDocDetail = null;
                        DAL.DocumentDetail signDocDetail = null;

                        using (var db = new CandidateDataManager())
                        {
                            photoDoc = db.AdmissionDB.Documents.Where(c => c.CandidateID == cId && c.DocumentTypeID == 2).FirstOrDefault();
                            signDoc = db.AdmissionDB.Documents.Where(c => c.CandidateID == cId && c.DocumentTypeID == 3).FirstOrDefault();
                        }

                        if (photoDoc != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                photoDocDetail = db.AdmissionDB.DocumentDetails.Where(c => c.ID == photoDoc.DocumentDetailsID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            validityCheck += "** Photo does not exist.<br/>";
                        }

                        if (photoDocDetail != null)
                        {
                            if (string.IsNullOrEmpty(photoDocDetail.URL))
                            {
                                validityCheck += "* Photo not found. Please upload photo.<br/>";
                            }
                        }
                        else
                        {
                            validityCheck += "** Photo details does not exist.<br/>";
                        }

                        if (signDoc != null)
                        {
                            using (var db = new CandidateDataManager())
                            {
                                signDocDetail = db.AdmissionDB.DocumentDetails.Where(c => c.ID == signDoc.DocumentDetailsID).FirstOrDefault();
                            }
                        }
                        else
                        {
                            validityCheck += "** Signature does not exist.<br/>";
                        }

                        if (signDocDetail != null)
                        {
                            if (string.IsNullOrEmpty(signDocDetail.URL))
                            {
                                validityCheck += "* Signature not found. Please upload signature.<br/>";
                            }
                        }
                        else
                        {
                            validityCheck += "** Signature details does not exist.<br/>";
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowAlertMessage("Exception; Error: " + ex.Message.ToString());
                        return;
                    }

                    #endregion

                    List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list = null;
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            list = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateID(cId, null, null).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error: Unable to fetch details";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        hfMessage.Value = ex.Message;
                    }

                    if (string.IsNullOrEmpty(validityCheck))
                    {

                        #region Bind GridView
                        if (list != null)
                        {
                            if (list.Count() > 0)
                            {
                                gvAppliedProgs.DataSource = list;
                            }
                            else
                            {
                                gvAppliedProgs.DataSource = null;
                            }
                        }
                        else
                        {
                            gvAppliedProgs.DataSource = null;
                        }
                        gvAppliedProgs.DataBind();
                        #endregion

                    }
                    else
                    {
                        lblMessage.Text = validityCheck;
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }
            }
            else
            {
                ShowAlertMessage("Access unauthorized !!");
            }


        }

        protected void btnDownloadAdmitCard_Click(object sender, EventArgs e)
        {
            long cId = -1;

            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }
            }
            else
            {
                Response.Redirect("~/Admission/Home.aspx", false);
            }//end if-else (uId > 0)

            if (cId == cIdVal)
            {
                #region Candidate Download
                if (cId > 0)
                {
                    try
                    {

                        messagePanel.Visible = false;

                        LinkButton lnkBtn = (LinkButton)sender;
                        long paymentId = Convert.ToInt64(lnkBtn.CommandArgument.ToString());

                        GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;

                        Label lblAcaCalId = (Label)gvrow.FindControl("lblAcaCalId");
                        Label lblAdmUnitId = (Label)gvrow.FindControl("lblAdmUnitId");
                        Label lblTestRoll = (Label)gvrow.FindControl("lblTestRoll");

                        int acaCalId = Convert.ToInt32(lblAcaCalId.Text);
                        long admUnitId = Convert.ToInt64(lblAdmUnitId.Text);
                        string testRoll = lblTestRoll.Text;

                        List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list = null;
                        try
                        {
                            using (var db = new CandidateDataManager())
                            {
                                list = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateID(cId, null, null).ToList();
                            }
                        }
                        catch (Exception)
                        {

                        }

                        DAL.SPGetDetailsForAdmitCardByCandidateID_Result admitCardDetails = null;
                        if (list != null && list.Count > 0)
                        {
                            admitCardDetails = list.Where(c => c.AcaCalID == acaCalId &&
                                                                c.admUnitID == admUnitId &&
                                                                c.TestRoll.Equals(testRoll) &&
                                                                c.IsPaid == true &&
                                                                c.IsApproved == true)
                                                        .FirstOrDefault();
                        }
                        
                        if (admitCardDetails != null)
                        {

                            bool instructionImageHS = false;
                            bool importantInstructionHS = false;
                            int programId = -1;
                            long admSetId = -1;
                            
                            DAL.CandidateFormSl formSL = null;
                            DAL.AdmissionSetup admSet = null;
                            DAL.AdmissionUnitProgram admUnitProg = null;
                            using (var db = new GeneralDataManager())
                            {
                                formSL = db.AdmissionDB.CandidateFormSls.Where(x => x.FormSerial == admitCardDetails.FormSerial).FirstOrDefault();
                                if (formSL != null)
                                {
                                    admSet = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == formSL.AdmissionSetupID).FirstOrDefault();
                                    if (admSet != null)
                                    {
                                        admSetId = admSet.ID;

                                        admUnitProg = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.IsActive == true
                                                                                                  && x.EducationCategoryID == admSet.EducationCategoryID
                                                                                                  && x.AcaCalID == admSet.AcaCalID
                                                                                                  && x.AdmissionUnitID == admSet.AdmissionUnitID).FirstOrDefault();

                                        if (admUnitProg != null)
                                        {
                                            programId = admUnitProg.ProgramID;
                                        }
                                    }
                                }
                            }

                            if (programId > 0 && programId == 7) // EMBA Pro. = 7; jnno Image Isntruction ta dekhabe
                            {
                                instructionImageHS = true;
                                importantInstructionHS = false;
                            }
                            else
                            {
                                instructionImageHS = false;
                                importantInstructionHS = true;
                            }



                            List<DAL.ProgramPriority> choices = null;
                            using (var db = new CandidateDataManager())
                            {
                                choices = db.AdmissionDB.ProgramPriorities
                                    .Where(c => c.CandidateID == cId && c.Priority == 1 &&
                                        c.AdmissionUnitID == admitCardDetails.admUnitID &&
                                        admitCardDetails.EducationCategoryID == 4)
                                        .ToList();
                            }

                            if (choices != null)
                            {
                                if (choices.Count() > 1)
                                {
                                    lblMessage.Text = "More than 1 program is selected as Choice 1. Please select only 1 choice for this faculty.";
                                    lblMessage.ForeColor = Color.Crimson;
                                    messagePanel.CssClass = "alert alert-danger";
                                    messagePanel.Visible = true;
                                    return;
                                }
                                else if (choices.Count() == 1)
                                {

                                    #region Insurt Log when Candidate Click Download Button for Admit Card
                                    int educationCategoryId = admitCardDetails.EducationCategoryID;
                                    AdmitCardDownloadClickLog(paymentId, acaCalId, admUnitId, testRoll, educationCategoryId, cId, admSetId);
                                    #endregion


                                    #region Load Report and Download
                                    ReportViewer1.LocalReport.EnableExternalImages = true;
                                    ReportViewerMasters.LocalReport.EnableExternalImages = true;

                                    ReportDataSource rds = new ReportDataSource("DataSet1", list.Where(c => c.AcaCalID == acaCalId &&
                                                                                                    c.admUnitID == admUnitId &&
                                                                                                    c.TestRoll.Equals(testRoll) &&
                                                                                                    c.IsPaid == true &&
                                                                                                    c.IsApproved == true).ToList());

                                    string imgUrl = new Uri(Server.MapPath(admitCardDetails.PhotoPath)).AbsoluteUri;
                                    string SignatureUrl = new Uri(Server.MapPath(admitCardDetails.SignPath)).AbsoluteUri;
                                    //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;

                                    DAL.Room roomModel = null;
                                    string floorName = "-";
                                    using (var db = new GeneralDataManager())
                                    {
                                        roomModel = db.AdmissionDB.Rooms.Where(x => x.ID == admitCardDetails.prpRoomID).FirstOrDefault();
                                    }
                                    if (roomModel != null)
                                    {
                                        floorName = roomModel.FloorNumber.ToString();
                                    }


                                    //string examCenter = admitCardDetails.prpRoomName + ", " + floorName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                                    string examCenter = admitCardDetails.campusName;

                                    DateTime examDate = Convert.ToDateTime(admitCardDetails.ExamDate);

                                    IList<ReportParameter> param1 = new List<ReportParameter>();
                                    param1.Add(new ReportParameter("FacultyName", admitCardDetails.UnitName));
                                    param1.Add(new ReportParameter("CandidateName", admitCardDetails.FirstName));
                                    param1.Add(new ReportParameter("CandidateImagePath", imgUrl));
                                    param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl));
                                    param1.Add(new ReportParameter("FatherName", admitCardDetails.FatherName != null ? admitCardDetails.FatherName.ToUpper() : ""));
                                    param1.Add(new ReportParameter("RollNumber", admitCardDetails.TestRoll != null ? admitCardDetails.TestRoll : ""));
                                    param1.Add(new ReportParameter("ExamCenter", examCenter));
                                    param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
                                    param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));
                                    param1.Add(new ReportParameter("InstructionImageHS", instructionImageHS.ToString()));
                                    param1.Add(new ReportParameter("ImportantInstructionHS", importantInstructionHS.ToString()));


                                    int eduCatId = -1;
                                    using (var db = new CandidateDataManager())
                                    {
                                        eduCatId = db.GetCandidateEducationCategoryID(cId);
                                    }
                                    if (eduCatId > 0)
                                    {
                                        if (eduCatId == 4)
                                        {
                                            ReportViewer1.LocalReport.SetParameters(param1);

                                            ReportViewer1.LocalReport.DataSources.Clear();
                                            ReportViewer1.LocalReport.DataSources.Add(rds);
                                            ReportViewer1.Visible = true;

                                            ReportViewerMasters.Visible = false;
                                        }
                                        else
                                        {
                                            ReportViewerMasters.LocalReport.SetParameters(param1);

                                            ReportViewerMasters.LocalReport.DataSources.Clear();
                                            ReportViewerMasters.LocalReport.DataSources.Add(rds);
                                            ReportViewerMasters.Visible = true;

                                            ReportViewer1.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        ReportViewer1.LocalReport.SetParameters(param1);

                                        ReportViewer1.LocalReport.DataSources.Clear();
                                        ReportViewer1.LocalReport.DataSources.Add(rds);
                                        ReportViewer1.Visible = true;

                                        ReportViewerMasters.Visible = false;
                                    }
                                    

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card";
                                        dLog.PageName = "AdmitCard.aspx";
                                        dLog.NewData = "Admit Card Download Successful. Name:" + admitCardDetails.FirstName + ";" +
                                                                                        "PaymentId: " + admitCardDetails.PaymentId + ";" +
                                                                                        "FacultyName: " + admitCardDetails.UnitName + ";" +
                                                                                        "FormSerial: " + admitCardDetails.FormSerial + ";";
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Success";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion


                                    try
                                    {

                                        Warning[] warnings;
                                        string[] streamids;
                                        string mimeType;
                                        string encoding;
                                        string filenameExtension;

                                        byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                        ReportViewer1.LocalReport.Refresh();

                                        using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"), FileMode.Create))
                                        {
                                            fs.Write(bytes, 0, bytes.Length);
                                        }

                                        System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                        Response.ClearHeaders();
                                        Response.ClearContent();
                                        Response.Buffer = true;
                                        Response.Clear();
                                        Response.ContentType = "application/pdf";
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                        Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                        Response.Flush();
                                        //Response.Close();
                                        Response.End();


                                    }
                                    catch (Exception ex)
                                    {
                                        lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                        lblMessage.ForeColor = Color.Crimson;
                                        messagePanel.CssClass = "alert alert-danger";
                                        messagePanel.Visible = true;

                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.EventName = "Download Admit Card";
                                            dLog.PageName = "AdmitCard.aspx";
                                            dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                            dLog.UserId = uId;
                                            dLog.Attribute1 = "Failed";
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            dLog.DateCreated = DateTime.Now;
                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ext)
                                        {
                                        }
                                        #endregion

                                        return;
                                    }
                                    finally
                                    {
                                        try
                                        {
                                            if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                            {
                                                File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.EventName = "Download Admit Card";
                                                dLog.PageName = "AdmitCard.aspx";
                                                dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = "Failed";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                dLog.DateCreated = DateTime.Now;
                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ext)
                                            {
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                else if (choices.Count() == 0)
                                {
                                    #region Load Report and Download  MASTERS

                                    ReportViewer1.LocalReport.EnableExternalImages = true;

                                    ReportViewerMasters.LocalReport.EnableExternalImages = true;

                                    ReportDataSource rds = new ReportDataSource("DataSet1", list.Where(c => c.AcaCalID == acaCalId &&
                                                                                                    c.admUnitID == admUnitId &&
                                                                                                    c.TestRoll.Equals(testRoll) &&
                                                                                                    c.IsPaid == true &&
                                                                                                    c.IsApproved == true).ToList());

                                    string imgUrl = new Uri(Server.MapPath(admitCardDetails.PhotoPath)).AbsoluteUri;
                                    string SignatureUrl = new Uri(Server.MapPath(admitCardDetails.SignPath)).AbsoluteUri;
                                    //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                                    string examCenter = admitCardDetails.campusName;


                                    DateTime examDate = Convert.ToDateTime(admitCardDetails.ExamDate);

                                    IList<ReportParameter> param1 = new List<ReportParameter>();
                                    param1.Add(new ReportParameter("FacultyName", admitCardDetails.UnitName));
                                    param1.Add(new ReportParameter("CandidateName", admitCardDetails.FirstName));
                                    param1.Add(new ReportParameter("CandidateImagePath", imgUrl));
                                    param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl));
                                    param1.Add(new ReportParameter("FatherName", admitCardDetails.FatherName.ToUpper()));
                                    param1.Add(new ReportParameter("RollNumber", admitCardDetails.TestRoll.ToString()));
                                    param1.Add(new ReportParameter("ExamCenter", examCenter));
                                    param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
                                    param1.Add(new ReportParameter("InstructionImageHS", instructionImageHS.ToString()));
                                    param1.Add(new ReportParameter("ImportantInstructionHS", importantInstructionHS.ToString()));

                                    if (admUnitId == 10) //MBA (Regular) program will be shown the bachelor Program Admit Card
                                    {
                                        ReportViewer1.LocalReport.SetParameters(param1);

                                        ReportViewer1.LocalReport.DataSources.Clear();
                                        ReportViewer1.LocalReport.DataSources.Add(rds);
                                        ReportViewer1.Visible = true;

                                        ReportViewerMasters.Visible = false;


                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.EventName = "Download Admit Card";
                                            dLog.PageName = "AdmitCard.aspx";
                                            dLog.NewData = "Admit Card Download Successful. Name:" + admitCardDetails.FirstName + ";" +
                                                                                            "PaymentId: " + admitCardDetails.PaymentId + ";" +
                                                                                            "FacultyName: " + admitCardDetails.UnitName + ";" +
                                                                                            "FormSerial: " + admitCardDetails.FormSerial + ";";
                                            dLog.UserId = uId;
                                            dLog.Attribute1 = "Success";
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            dLog.DateCreated = DateTime.Now;
                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion


                                        try
                                        {

                                            Warning[] warnings;
                                            string[] streamids;
                                            string mimeType;
                                            string encoding;
                                            string filenameExtension;

                                            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                            ReportViewer1.LocalReport.Refresh();

                                            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"), FileMode.Create))
                                            {
                                                fs.Write(bytes, 0, bytes.Length);
                                            }

                                            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                            Response.ClearHeaders();
                                            Response.ClearContent();
                                            Response.Buffer = true;
                                            Response.Clear();
                                            Response.ContentType = "application/pdf";
                                            Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                            Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                            Response.Flush();
                                            //Response.Close();
                                            Response.End();


                                        }
                                        catch (Exception ex)
                                        {
                                            lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                            lblMessage.ForeColor = Color.Crimson;
                                            messagePanel.CssClass = "alert alert-danger";
                                            messagePanel.Visible = true;

                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.EventName = "Download Admit Card";
                                                dLog.PageName = "AdmitCard.aspx";
                                                dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = "Failed";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                dLog.DateCreated = DateTime.Now;
                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ext)
                                            {
                                            }
                                            #endregion

                                            return;
                                        }
                                        finally
                                        {
                                            try
                                            {
                                                if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                                {
                                                    File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                try
                                                {
                                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                    dLog.EventName = "Download Admit Card";
                                                    dLog.PageName = "AdmitCard.aspx";
                                                    dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                    dLog.UserId = uId;
                                                    dLog.Attribute1 = "Failed";
                                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                    dLog.DateCreated = DateTime.Now;
                                                    LogWriter.DataLogWriter(dLog);
                                                }
                                                catch (Exception ext)
                                                {
                                                }
                                                #endregion
                                            }
                                        }

                                    }
                                    else
                                    {
                                        ReportViewerMasters.LocalReport.SetParameters(param1);

                                        ReportViewerMasters.LocalReport.DataSources.Clear();
                                        ReportViewerMasters.LocalReport.DataSources.Add(rds);
                                        ReportViewerMasters.Visible = true;

                                        ReportViewer1.Visible = false;




                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.EventName = "Download Admit Card";
                                            dLog.PageName = "AdmitCard.aspx";
                                            dLog.NewData = "Admit Card Download Successful. Name:" + admitCardDetails.FirstName + ";" +
                                                                                            "PaymentId: " + admitCardDetails.PaymentId + ";" +
                                                                                            "FacultyName: " + admitCardDetails.UnitName + ";" +
                                                                                            "FormSerial: " + admitCardDetails.FormSerial + ";";
                                            dLog.UserId = uId;
                                            dLog.Attribute1 = "Success";
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            dLog.DateCreated = DateTime.Now;
                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        try
                                        {

                                            Warning[] warnings;
                                            string[] streamids;
                                            string mimeType;
                                            string encoding;
                                            string filenameExtension;

                                            byte[] bytes = ReportViewerMasters.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                            ReportViewerMasters.LocalReport.Refresh();

                                            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"), FileMode.Create))
                                            {
                                                fs.Write(bytes, 0, bytes.Length);
                                            }

                                            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                            Response.ClearHeaders();
                                            Response.ClearContent();
                                            Response.Buffer = true;
                                            Response.Clear();
                                            Response.ContentType = "application/pdf";
                                            Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                            Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                            Response.Flush();
                                            //Response.Close();
                                            Response.End();

                                        }
                                        catch (Exception ex)
                                        {
                                            lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                            lblMessage.ForeColor = Color.Crimson;
                                            messagePanel.CssClass = "alert alert-danger";
                                            messagePanel.Visible = true;

                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.EventName = "Download Admit Card";
                                                dLog.PageName = "AdmitCard.aspx";
                                                dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = "Failed";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                dLog.DateCreated = DateTime.Now;
                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ext)
                                            {
                                            }
                                            #endregion

                                            return;
                                        }
                                        finally
                                        {
                                            try
                                            {
                                                if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                                {
                                                    File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                try
                                                {
                                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                    dLog.EventName = "Download Admit Card";
                                                    dLog.PageName = "AdmitCard.aspx";
                                                    dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                    dLog.UserId = uId;
                                                    dLog.Attribute1 = "Failed";
                                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                    dLog.DateCreated = DateTime.Now;
                                                    LogWriter.DataLogWriter(dLog);
                                                }
                                                catch (Exception ext)
                                                {
                                                }
                                                #endregion
                                            }
                                        }
                                    }

                                    

                                    
                                    #endregion
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Error: Unable to get candidate programs";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                                messagePanel.Visible = true;
                                return;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Exception: Unable to generate admit card; Error: " + ex.Message.ToString();
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        return;
                    }
                }//end if (cId > 0) 
                #endregion
            }
            else if (uRole == "ICT Center")
            {
                #region Admin Download
                cId = cIdVal;

                if (cId > 0)
                {
                    try
                    {

                        messagePanel.Visible = false;

                        LinkButton lnkBtn = (LinkButton)sender;
                        long paymentId = Convert.ToInt64(lnkBtn.CommandArgument.ToString());

                        GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;

                        Label lblAcaCalId = (Label)gvrow.FindControl("lblAcaCalId");
                        Label lblAdmUnitId = (Label)gvrow.FindControl("lblAdmUnitId");
                        Label lblTestRoll = (Label)gvrow.FindControl("lblTestRoll");

                        int acaCalId = Convert.ToInt32(lblAcaCalId.Text);
                        long admUnitId = Convert.ToInt64(lblAdmUnitId.Text);
                        string testRoll = lblTestRoll.Text;

                        List<DAL.SPGetDetailsForAdmitCardByCandidateID_Result> list = null;
                        try
                        {
                            using (var db = new CandidateDataManager())
                            {
                                list = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateID(cId, null, null).ToList();
                            }
                        }
                        catch (Exception)
                        {

                        }

                        DAL.SPGetDetailsForAdmitCardByCandidateID_Result admitCardDetails = null;
                        if (list != null && list.Count > 0)
                        {
                            admitCardDetails = list.Where(c => c.AcaCalID == acaCalId &&
                                                                c.admUnitID == admUnitId &&
                                                                c.TestRoll.Equals(testRoll) &&
                                                                c.IsPaid == true &&
                                                                c.IsApproved == true)
                                                        .FirstOrDefault();
                        }

                        if (admitCardDetails != null)
                        {

                            bool instructionImageHS = false;
                            bool importantInstructionHS = false;
                            int programId = -1;
                            long admSetId = -1;
                            DAL.CandidateFormSl formSL = null;
                            DAL.AdmissionSetup admSet = null;
                            DAL.AdmissionUnitProgram admUnitProg = null;
                            using (var db = new GeneralDataManager())
                            {
                                formSL = db.AdmissionDB.CandidateFormSls.Where(x => x.FormSerial == admitCardDetails.FormSerial).FirstOrDefault();
                                if (formSL != null)
                                {
                                    admSet = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == formSL.AdmissionSetupID).FirstOrDefault();
                                    if (admSet != null)
                                    {
                                        admSetId = admSet.ID;
                                        admUnitProg = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.IsActive == true
                                                                                                  && x.EducationCategoryID == admSet.EducationCategoryID
                                                                                                  && x.AcaCalID == admSet.AcaCalID
                                                                                                  && x.AdmissionUnitID == admSet.AdmissionUnitID).FirstOrDefault();

                                        if (admUnitProg != null)
                                        {
                                            programId = admUnitProg.ProgramID;
                                        }
                                    }
                                }
                            }

                            if (programId > 0 && programId == 7) // EMBA Pro. = 7; jnno Image Isntruction ta dekhabe
                            {
                                instructionImageHS = true;
                                importantInstructionHS = false;
                            }
                            else
                            {
                                instructionImageHS = false;
                                importantInstructionHS = true;
                            }



                            List<DAL.ProgramPriority> choices = null;
                            using (var db = new CandidateDataManager())
                            {
                                choices = db.AdmissionDB.ProgramPriorities
                                    .Where(c => c.CandidateID == cId && c.Priority == 1 &&
                                        c.AdmissionUnitID == admitCardDetails.admUnitID &&
                                        admitCardDetails.EducationCategoryID == 4)
                                        .ToList();
                            }

                            if (choices != null)
                            {
                                if (choices.Count() > 1)
                                {
                                    lblMessage.Text = "More than 1 program is selected as Choice 1. Please select only 1 choice for this faculty.";
                                    lblMessage.ForeColor = Color.Crimson;
                                    messagePanel.CssClass = "alert alert-danger";
                                    messagePanel.Visible = true;
                                    return;
                                }
                                else if (choices.Count() == 1)
                                {

                                    #region Insurt Log when Candidate Click Download Button for Admit Card
                                    int educationCategoryId = admitCardDetails.EducationCategoryID;
                                    AdmitCardDownloadClickLog(paymentId, acaCalId, admUnitId, testRoll, educationCategoryId, cId, admSetId);
                                    #endregion


                                    #region Load Report and Download
                                    ReportViewer1.LocalReport.EnableExternalImages = true;
                                    ReportViewerMasters.LocalReport.EnableExternalImages = true;

                                    ReportDataSource rds = new ReportDataSource("DataSet1", list.Where(c => c.AcaCalID == acaCalId &&
                                                                                                    c.admUnitID == admUnitId &&
                                                                                                    c.TestRoll.Equals(testRoll) &&
                                                                                                    c.IsPaid == true &&
                                                                                                    c.IsApproved == true).ToList());

                                    string imgUrl = new Uri(Server.MapPath(admitCardDetails.PhotoPath)).AbsoluteUri;
                                    string SignatureUrl = new Uri(Server.MapPath(admitCardDetails.SignPath)).AbsoluteUri;
                                    //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;

                                    DAL.Room roomModel = null;
                                    string floorName = "-";
                                    using (var db = new GeneralDataManager())
                                    {
                                        roomModel = db.AdmissionDB.Rooms.Where(x => x.ID == admitCardDetails.prpRoomID).FirstOrDefault();
                                    }
                                    if (roomModel != null)
                                    {
                                        floorName = roomModel.FloorNumber.ToString();
                                    }


                                    //string examCenter = admitCardDetails.prpRoomName + ", " + floorName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                                    string examCenter = admitCardDetails.campusName;

                                    DateTime examDate = Convert.ToDateTime(admitCardDetails.ExamDate);

                                    IList<ReportParameter> param1 = new List<ReportParameter>();
                                    param1.Add(new ReportParameter("FacultyName", admitCardDetails.UnitName));
                                    param1.Add(new ReportParameter("CandidateName", admitCardDetails.FirstName));
                                    param1.Add(new ReportParameter("CandidateImagePath", imgUrl));
                                    param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl));
                                    param1.Add(new ReportParameter("FatherName", admitCardDetails.FatherName != null ? admitCardDetails.FatherName.ToUpper() : ""));
                                    param1.Add(new ReportParameter("RollNumber", admitCardDetails.TestRoll != null ? admitCardDetails.TestRoll : ""));
                                    param1.Add(new ReportParameter("ExamCenter", examCenter));
                                    param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
                                    param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));
                                    param1.Add(new ReportParameter("InstructionImageHS", instructionImageHS.ToString()));
                                    param1.Add(new ReportParameter("ImportantInstructionHS", importantInstructionHS.ToString()));


                                    int eduCatId = -1;
                                    using (var db = new CandidateDataManager())
                                    {
                                        eduCatId = db.GetCandidateEducationCategoryID(cId);
                                    }
                                    if (eduCatId > 0)
                                    {
                                        if (eduCatId == 4)
                                        {
                                            ReportViewer1.LocalReport.SetParameters(param1);

                                            ReportViewer1.LocalReport.DataSources.Clear();
                                            ReportViewer1.LocalReport.DataSources.Add(rds);
                                            ReportViewer1.Visible = true;

                                            ReportViewerMasters.Visible = false;
                                        }
                                        else
                                        {
                                            ReportViewerMasters.LocalReport.SetParameters(param1);

                                            ReportViewerMasters.LocalReport.DataSources.Clear();
                                            ReportViewerMasters.LocalReport.DataSources.Add(rds);
                                            ReportViewerMasters.Visible = true;

                                            ReportViewer1.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        ReportViewer1.LocalReport.SetParameters(param1);

                                        ReportViewer1.LocalReport.DataSources.Clear();
                                        ReportViewer1.LocalReport.DataSources.Add(rds);
                                        ReportViewer1.Visible = true;

                                        ReportViewerMasters.Visible = false;
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.EventName = "Download Admit Card";
                                        dLog.PageName = "AdmitCard.aspx";
                                        dLog.NewData = "Admit Card Download Successful. Name:" + admitCardDetails.FirstName + ";" +
                                                                                        "PaymentId: " + admitCardDetails.PaymentId + ";" +
                                                                                        "FacultyName: " + admitCardDetails.UnitName + ";" +
                                                                                        "FormSerial: " + admitCardDetails.FormSerial + ";";
                                        dLog.UserId = uId;
                                        dLog.Attribute1 = "Success";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                        dLog.DateCreated = DateTime.Now;
                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    try
                                    {

                                        Warning[] warnings;
                                        string[] streamids;
                                        string mimeType;
                                        string encoding;
                                        string filenameExtension;

                                        byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                        ReportViewer1.LocalReport.Refresh();

                                        using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"), FileMode.Create))
                                        {
                                            fs.Write(bytes, 0, bytes.Length);
                                        }

                                        System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                        Response.ClearHeaders();
                                        Response.ClearContent();
                                        Response.Buffer = true;
                                        Response.Clear();
                                        Response.ContentType = "application/pdf";
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                        Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                        Response.Flush();
                                        //Response.Close();
                                        Response.End();

                                    }
                                    catch (Exception ex)
                                    {
                                        lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                        lblMessage.ForeColor = Color.Crimson;
                                        messagePanel.CssClass = "alert alert-danger";
                                        messagePanel.Visible = true;

                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.EventName = "Download Admit Card";
                                            dLog.PageName = "AdmitCard.aspx";
                                            dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                            dLog.UserId = uId;
                                            dLog.Attribute1 = "Failed";
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            dLog.DateCreated = DateTime.Now;
                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ext)
                                        {
                                        }
                                        #endregion

                                        return;
                                    }
                                    finally
                                    {
                                        try
                                        {
                                            if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                            {
                                                File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.EventName = "Download Admit Card";
                                                dLog.PageName = "AdmitCard.aspx";
                                                dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = "Failed";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                dLog.DateCreated = DateTime.Now;
                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ext)
                                            {
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                else if (choices.Count() == 0)
                                {
                                    #region Load Report and Download  MASTERS

                                    ReportViewer1.LocalReport.EnableExternalImages = true;

                                    ReportViewerMasters.LocalReport.EnableExternalImages = true;

                                    ReportDataSource rds = new ReportDataSource("DataSet1", list.Where(c => c.AcaCalID == acaCalId &&
                                                                                                    c.admUnitID == admUnitId &&
                                                                                                    c.TestRoll.Equals(testRoll) &&
                                                                                                    c.IsPaid == true &&
                                                                                                    c.IsApproved == true).ToList());

                                    string imgUrl = new Uri(Server.MapPath(admitCardDetails.PhotoPath)).AbsoluteUri;
                                    string SignatureUrl = new Uri(Server.MapPath(admitCardDetails.SignPath)).AbsoluteUri;
                                    //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                                    string examCenter = admitCardDetails.campusName;


                                    DateTime examDate = Convert.ToDateTime(admitCardDetails.ExamDate);

                                    IList<ReportParameter> param1 = new List<ReportParameter>();
                                    param1.Add(new ReportParameter("FacultyName", admitCardDetails.UnitName));
                                    param1.Add(new ReportParameter("CandidateName", admitCardDetails.FirstName));
                                    param1.Add(new ReportParameter("CandidateImagePath", imgUrl));
                                    param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl));
                                    param1.Add(new ReportParameter("FatherName", admitCardDetails.FatherName.ToUpper()));
                                    param1.Add(new ReportParameter("RollNumber", admitCardDetails.TestRoll.ToString()));
                                    param1.Add(new ReportParameter("ExamCenter", examCenter));
                                    param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
                                    param1.Add(new ReportParameter("InstructionImageHS", instructionImageHS.ToString()));
                                    param1.Add(new ReportParameter("ImportantInstructionHS", importantInstructionHS.ToString()));

                                    if (admUnitId == 10) //MBA (Regular) program will be shown the bachelor Program Admit Card
                                    {
                                        ReportViewer1.LocalReport.SetParameters(param1);

                                        ReportViewer1.LocalReport.DataSources.Clear();
                                        ReportViewer1.LocalReport.DataSources.Add(rds);
                                        ReportViewer1.Visible = true;

                                        ReportViewerMasters.Visible = false;


                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.EventName = "Download Admit Card";
                                            dLog.PageName = "AdmitCard.aspx";
                                            dLog.NewData = "Admit Card Download Successful. Name:" + admitCardDetails.FirstName + ";" +
                                                                                            "PaymentId: " + admitCardDetails.PaymentId + ";" +
                                                                                            "FacultyName: " + admitCardDetails.UnitName + ";" +
                                                                                            "FormSerial: " + admitCardDetails.FormSerial + ";";
                                            dLog.UserId = uId;
                                            dLog.Attribute1 = "Success";
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            dLog.DateCreated = DateTime.Now;
                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion


                                        try
                                        {

                                            Warning[] warnings;
                                            string[] streamids;
                                            string mimeType;
                                            string encoding;
                                            string filenameExtension;

                                            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                            ReportViewer1.LocalReport.Refresh();

                                            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"), FileMode.Create))
                                            {
                                                fs.Write(bytes, 0, bytes.Length);
                                            }

                                            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                            Response.ClearHeaders();
                                            Response.ClearContent();
                                            Response.Buffer = true;
                                            Response.Clear();
                                            Response.ContentType = "application/pdf";
                                            Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                            Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                            Response.Flush();
                                            //Response.Close();
                                            Response.End();


                                        }
                                        catch (Exception ex)
                                        {
                                            lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                            lblMessage.ForeColor = Color.Crimson;
                                            messagePanel.CssClass = "alert alert-danger";
                                            messagePanel.Visible = true;

                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.EventName = "Download Admit Card";
                                                dLog.PageName = "AdmitCard.aspx";
                                                dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = "Failed";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                dLog.DateCreated = DateTime.Now;
                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ext)
                                            {
                                            }
                                            #endregion

                                            return;
                                        }
                                        finally
                                        {
                                            try
                                            {
                                                if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                                {
                                                    File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                try
                                                {
                                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                    dLog.EventName = "Download Admit Card";
                                                    dLog.PageName = "AdmitCard.aspx";
                                                    dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                    dLog.UserId = uId;
                                                    dLog.Attribute1 = "Failed";
                                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                    dLog.DateCreated = DateTime.Now;
                                                    LogWriter.DataLogWriter(dLog);
                                                }
                                                catch (Exception ext)
                                                {
                                                }
                                                #endregion
                                            }
                                        }

                                    }
                                    else
                                    {
                                        ReportViewerMasters.LocalReport.SetParameters(param1);

                                        ReportViewerMasters.LocalReport.DataSources.Clear();
                                        ReportViewerMasters.LocalReport.DataSources.Add(rds);
                                        ReportViewerMasters.Visible = true;

                                        ReportViewer1.Visible = false;




                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
                                            DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                            dLog.EventName = "Download Admit Card";
                                            dLog.PageName = "AdmitCard.aspx";
                                            dLog.NewData = "Admit Card Download Successful. Name:" + admitCardDetails.FirstName + ";" +
                                                                                            "PaymentId: " + admitCardDetails.PaymentId + ";" +
                                                                                            "FacultyName: " + admitCardDetails.UnitName + ";" +
                                                                                            "FormSerial: " + admitCardDetails.FormSerial + ";";
                                            dLog.UserId = uId;
                                            dLog.Attribute1 = "Success";
                                            dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                            dLog.DateCreated = DateTime.Now;
                                            LogWriter.DataLogWriter(dLog);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        #endregion

                                        try
                                        {

                                            Warning[] warnings;
                                            string[] streamids;
                                            string mimeType;
                                            string encoding;
                                            string filenameExtension;

                                            byte[] bytes = ReportViewerMasters.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                                            ReportViewerMasters.LocalReport.Refresh();

                                            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"), FileMode.Create))
                                            {
                                                fs.Write(bytes, 0, bytes.Length);
                                            }

                                            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                                            Response.ClearHeaders();
                                            Response.ClearContent();
                                            Response.Buffer = true;
                                            Response.Clear();
                                            Response.ContentType = "application/pdf";
                                            Response.AddHeader("Content-Disposition", "attachment; filename=" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf");
                                            Response.TransmitFile(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                            Response.Flush();
                                            //Response.Close();
                                            Response.End();

                                        }
                                        catch (Exception ex)
                                        {
                                            lblMessage.Text = "Exception: Unable to download admit card; Error: " + ex.Message.ToString();
                                            lblMessage.ForeColor = Color.Crimson;
                                            messagePanel.CssClass = "alert alert-danger";
                                            messagePanel.Visible = true;

                                            #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                            try
                                            {
                                                DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                dLog.EventName = "Download Admit Card";
                                                dLog.PageName = "AdmitCard.aspx";
                                                dLog.NewData = "Exception Unable to download admit card; Error: " + ex.Message.ToString();
                                                dLog.UserId = uId;
                                                dLog.Attribute1 = "Failed";
                                                dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                    SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                dLog.DateCreated = DateTime.Now;
                                                LogWriter.DataLogWriter(dLog);
                                            }
                                            catch (Exception ext)
                                            {
                                            }
                                            #endregion

                                            return;
                                        }
                                        finally
                                        {
                                            try
                                            {
                                                if (File.Exists(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf")))
                                                {
                                                    File.Delete(Server.MapPath("~/Upload/TempAdmitCardDownload/" + "BUPAdmitCard_" + admitCardDetails.AcaCalID + "_" + admitCardDetails.admUnitID + "_" + admitCardDetails.TestRoll + ".pdf"));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                                try
                                                {
                                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                                    dLog.EventName = "Download Admit Card";
                                                    dLog.PageName = "AdmitCard.aspx";
                                                    dLog.NewData = "Exception Failed to delete exist file; Error: " + ex.Message.ToString();
                                                    dLog.UserId = uId;
                                                    dLog.Attribute1 = "Failed";
                                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                                    dLog.DateCreated = DateTime.Now;
                                                    LogWriter.DataLogWriter(dLog);
                                                }
                                                catch (Exception ext)
                                                {
                                                }
                                                #endregion
                                            }
                                        }
                                    }




                                    #endregion
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Error: Unable to get candidate programs";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                                messagePanel.Visible = true;
                                return;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Exception: Unable to generate admit card; Error: " + ex.Message.ToString();
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                        return;
                    }
                }//end if (cId > 0) 
                #endregion
            }
            else
            {
                ShowAlertMessage("Access unauthorized !!");
            }

        }

        protected void gvAppliedProgs_RowCreated(object sender, GridViewRowEventArgs e)
        {
            LinkButton btnAdd = (LinkButton)e.Row.Cells[0].FindControl("btnDownloadAdmitCard");
            if (btnAdd != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnAdd);
            }
        }

        private void AdmitCardDownloadClickLog(long paymentId, int acaCalId, long admUnitId, string testRoll, int educationCategoryId, long cId, long admSetId)
        {
            try
            {
                if (paymentId > 0 && acaCalId > 0 && admUnitId > 0 && testRoll != "" && educationCategoryId > 0 && admSetId > 0)
                {
                    DAL_Log.CandidateAdmitCardDownloadClickLog cacdcl = new DAL_Log.CandidateAdmitCardDownloadClickLog();
                    cacdcl.PaymentId = paymentId;
                    cacdcl.AcaCalId = acaCalId;
                    cacdcl.AdmissionUnitId = admUnitId;
                    cacdcl.TestRoll = testRoll;
                    cacdcl.EducationCategoryId = educationCategoryId;
                    cacdcl.CreatedBy = Convert.ToInt32(cId);
                    cacdcl.CreatedDate = DateTime.Now;
                    cacdcl.AdmissionSetupId = admSetId;

                    using (var db = new LogDataManager())
                    {
                        db.Insert<DAL_Log.CandidateAdmitCardDownloadClickLog>(cacdcl);
                    }

                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                    try
                    {
                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                        dLog.EventName = "Download Admit Card";
                        dLog.PageName = "AdmitCard.aspx";
                        dLog.NewData = "AdmitCardDownloadClickLog()";
                        dLog.UserId = uId;
                        dLog.Attribute1 = "Success";
                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                        LogWriter.DataLogWriter(dLog);
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "Download Admit Card";
                    dLog.PageName = "AdmitCard.aspx";
                    dLog.NewData = "AdmitCardDownloadClickLog(); Error: " + ex.Message.ToString();
                    dLog.UserId = uId;
                    dLog.Attribute1 = "Failed";
                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ext)
                {
                }
                #endregion
            }
        }



    }
}