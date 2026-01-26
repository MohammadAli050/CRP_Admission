using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class ApplicationAttachment : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        string userName = "";

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //older version without auto crop
            FileUploadPhoto.Attributes["onchange"] = "UploadPhoto(this)";
            FileUploadSignature.Attributes["onchange"] = "UploadSignature(this)";
            //FileUploadFinGuarSignature.Attributes["onchange"] = "UploadFGSign(this)";

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            using (var db = new CandidateDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }

            if (!IsPostBack)
            {
                if (uId > 0)
                {
                    LoadCandidateData(uId);
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void LoadCandidateData(long uId)
        {
            long cId = -1;

            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);
                }// end using
            }
            if (cId > 0)
            {
                #region Get EducationCategoryId & ProgramId
                int educationCategoryId = -1;
                int programId = -1;
                using (var db = new CandidateDataManager())
                {
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

                #region Get IsFinalSubmit
                bool isFinalSubmit = false;
                DAL.AdditionalInfo additionalInfo = null;
                using (var db = new CandidateDataManager())
                {
                    additionalInfo = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                }
                if (additionalInfo != null)
                {
                    isFinalSubmit = Convert.ToBoolean(additionalInfo.IsFinalSubmit);
                }
                #endregion

                #region Property Setup (Candidate Submit Button Show/Hide)
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
                        #region Bachelor

                        #region Candidate Submit Button Show/Hide
                        try
                        {
                            var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)).FirstOrDefault();
                            if (candidateSubmitButtonSetup != null)
                            {
                                bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);

                                if (showHide == true)
                                {
                                    bool IsApproved = false;
                                    using (var db = new OfficeDataManager())
                                    {
                                        try
                                        {
                                            var ApprovedObj = db.AdmissionDB.ApprovedLists.Where(x => x.CandidateID == cId && x.IsApproved == true).FirstOrDefault();
                                            if (ApprovedObj != null)
                                                IsApproved = true;
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    if (isFinalSubmit && !IsApproved)
                                    {
                                        FileUploadPhoto.Visible = isFinalSubmit;
                                        FileUploadSignature.Visible = isFinalSubmit;
                                    }
                                    else
                                    {
                                        FileUploadPhoto.Visible = !isFinalSubmit;
                                        FileUploadSignature.Visible = !isFinalSubmit;
                                    }

                                }
                                else
                                {
                                    FileUploadPhoto.Visible = false;
                                    FileUploadSignature.Visible = false;
                                }

                            }
                            else
                            {
                                FileUploadPhoto.Visible = false;
                                FileUploadSignature.Visible = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            FileUploadPhoto.Visible = false;
                            FileUploadSignature.Visible = false;
                        }
                        #endregion


                        #endregion
                    }
                    else
                    {
                        #region Masters

                        #region Candidate Submit Button Show/Hide
                        try
                        {
                            var candidateSubmitButtonSetup = propertySetupList.Where(x => x.PropertyTypeID == Convert.ToInt32(CommonEnum.PropertyType.CandidateSubmitButton)
                                                                                        && x.ProgramId == programId).FirstOrDefault();
                            if (candidateSubmitButtonSetup != null)
                            {
                                bool showHide = Convert.ToBoolean(candidateSubmitButtonSetup.IsVisible);
                                if (showHide == true)
                                {
                                    FileUploadPhoto.Visible = !isFinalSubmit;
                                    FileUploadSignature.Visible = !isFinalSubmit;
                                }
                                else
                                {
                                    FileUploadPhoto.Visible = false;
                                    FileUploadSignature.Visible = false;
                                }
                            }
                            else
                            {
                                FileUploadPhoto.Visible = false;
                                FileUploadSignature.Visible = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            FileUploadPhoto.Visible = false;
                            FileUploadSignature.Visible = false;
                        }
                        #endregion

                        #endregion
                    }
                }
                else
                {
                    FileUploadPhoto.Visible = false;
                    FileUploadSignature.Visible = false;
                }
                #endregion

                #region Breadcrumbs for Bachelor and Masters
                if (educationCategoryId == 4)
                {
                    bachelorsBreadcrumb.Visible = true;
                    mastersBreadcrumb.Visible = false;
                }
                else
                {
                    bachelorsBreadcrumb.Visible = false;
                    mastersBreadcrumb.Visible = true;
                }
                #endregion

                using (var db = new CandidateDataManager())
                {
                    List<DAL.Document> documentList = db.GetAllDocumentByCandidateID_AD(cId);
                    if (documentList != null)
                    {
                        DAL.DocumentDetail photoDocDetailObj = documentList.Where(c => c.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                        DAL.DocumentDetail signDocDetailObj = documentList.Where(c => c.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();
                        DAL.DocumentDetail finGuarSignDocDetailObj = documentList.Where(c => c.DocumentTypeID == 7).Select(c => c.DocumentDetail).FirstOrDefault();

                        if (photoDocDetailObj != null)
                        {
                            ImagePhoto.ImageUrl = photoDocDetailObj.URL + "?v=" + DateTime.Now.Ticks;
                        }

                        if (signDocDetailObj != null)
                        {
                            ImageSignature.ImageUrl = signDocDetailObj.URL + "?v=" + DateTime.Now.Ticks;
                        }

                        //if (finGuarSignDocDetailObj != null)
                        //{
                        //    ImageFinGuarSignature.ImageUrl = finGuarSignDocDetailObj.URL;
                        //}
                    }
                }


                #region N/A -- Photo and Signature Upload off for Bachelor Candidates

                //DAL.CandidateFormSl cfsl = new DAL.CandidateFormSl();
                //DAL.AdmissionSetup aS = new DAL.AdmissionSetup();

                //using (var db = new CandidateDataManager())
                //{
                //    cfsl = db.AdmissionDB.CandidateFormSls.Where(x => x.CandidateID == cId).FirstOrDefault();
                //}

                //if (cfsl != null)
                //{
                //    using (var db = new CandidateDataManager())
                //    {
                //        aS = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == cfsl.AdmissionSetupID).FirstOrDefault();
                //    }
                //}

                //if (aS != null)
                //{
                //    if (aS.EducationCategoryID == 4)
                //    {
                //        FileUploadPhoto.Visible = false;
                //        FileUploadSignature.Visible = false;
                //    }
                //}

                #endregion

                #region N/A -- Prevent Save if IsFinalSubmit or IsApproved
                //try
                //{
                //    List<DAL.SPGetCandidateEducationCategoryByCandidateID_Result> list = null;
                //    using (var db = new CandidateDataManager())
                //    {
                //        list = db.AdmissionDB.SPGetCandidateEducationCategoryByCandidateID(cId).ToList();
                //    }

                //    if (list != null && list.Count > 0)
                //    {
                //        #region Bachelors
                //        DAL.SPGetCandidateEducationCategoryByCandidateID_Result undergradCandidate =
                //                            list.Where(c => c.EduCatID == 4).Take(1).FirstOrDefault();

                //        if (undergradCandidate != null)
                //        {
                //            #region N/A
                //            //if (undergradCandidate.IsApproved != null)
                //            //{
                //            //    if (undergradCandidate.IsApproved == true)
                //            //    {
                //            //        btnUploadPhoto.Enabled = false;
                //            //        btnUploadSignature.Enabled = false;

                //            //        btnUploadPhoto.Visible = false;
                //            //        btnUploadSignature.Visible = false;

                //            //        FileUploadPhoto.Enabled = false;
                //            //        FileUploadSignature.Enabled = false;

                //            //        FileUploadPhoto.Visible = false;
                //            //        FileUploadSignature.Visible = false;
                //            //    }
                //            //    else
                //            //    {
                //            //        btnUploadPhoto.Enabled = true;
                //            //        btnUploadSignature.Enabled = true;

                //            //        btnUploadPhoto.Visible = true;
                //            //        btnUploadSignature.Visible = true;

                //            //        FileUploadPhoto.Enabled = true;
                //            //        FileUploadSignature.Enabled = true;

                //            //        FileUploadPhoto.Visible = true;
                //            //        FileUploadSignature.Visible = true;
                //            //    }
                //            //}
                //            //else
                //            //{
                //            //    btnUploadPhoto.Enabled = true;
                //            //    btnUploadSignature.Enabled = true;

                //            //    btnUploadPhoto.Visible = true;
                //            //    btnUploadSignature.Visible = true;

                //            //    FileUploadPhoto.Enabled = true;
                //            //    FileUploadSignature.Enabled = true;

                //            //    FileUploadPhoto.Visible = true;
                //            //    FileUploadSignature.Visible = true;
                //            //} 
                //            #endregion

                //            if (undergradCandidate.IsFinalSubmit != null)
                //            {
                //                if (undergradCandidate.IsFinalSubmit == true)
                //                {
                //                    //    btnSave_Education.Enabled = false;
                //                    //    btnSave_Education.Visible = false;

                //                    FileUploadPhoto.Visible = false;
                //                    FileUploadSignature.Visible = false;
                //                }
                //                else
                //                {
                //                    //    btnSave_Education.Enabled = true;
                //                    //    btnSave_Education.Visible = true;

                //                    FileUploadPhoto.Visible = true;
                //                    FileUploadSignature.Visible = true;

                //                    FileUploadPhoto.Enabled = true;
                //                    FileUploadSignature.Enabled = true;
                //                }
                //            }
                //            else
                //            {
                //                //    btnSave_Education.Enabled = true;
                //                //    btnSave_Education.Visible = true;

                //                FileUploadPhoto.Visible = true;
                //                FileUploadSignature.Visible = true;

                //                FileUploadPhoto.Enabled = true;
                //                FileUploadSignature.Enabled = true;
                //            }
                //        }
                //        #endregion

                //        #region Masters
                //        DAL.SPGetCandidateEducationCategoryByCandidateID_Result gradCandidate =
                //                            list.Where(c => c.EduCatID == 6).Take(1).FirstOrDefault();

                //        if (gradCandidate != null)
                //        {
                //            if (gradCandidate.IsApproved != null)
                //            {
                //                if (gradCandidate.IsApproved == true)
                //                {
                //                    btnUploadPhoto.Enabled = false;
                //                    btnUploadSignature.Enabled = false;

                //                    btnUploadPhoto.Visible = false;
                //                    btnUploadSignature.Visible = false;

                //                    FileUploadPhoto.Enabled = false;
                //                    FileUploadSignature.Enabled = false;

                //                    FileUploadPhoto.Visible = false;
                //                    FileUploadSignature.Visible = false;
                //                }
                //                else
                //                {
                //                    btnUploadPhoto.Enabled = true;
                //                    btnUploadSignature.Enabled = true;

                //                    btnUploadPhoto.Visible = true;
                //                    btnUploadSignature.Visible = true;

                //                    FileUploadPhoto.Enabled = true;
                //                    FileUploadSignature.Enabled = true;

                //                    FileUploadPhoto.Visible = true;
                //                    FileUploadSignature.Visible = true;
                //                }
                //            }
                //            else
                //            {
                //                btnUploadPhoto.Enabled = true;
                //                btnUploadSignature.Enabled = true;

                //                btnUploadPhoto.Visible = true;
                //                btnUploadSignature.Visible = true;

                //                FileUploadPhoto.Enabled = true;
                //                FileUploadSignature.Enabled = true;

                //                FileUploadPhoto.Visible = true;
                //                FileUploadSignature.Visible = true;
                //            }

                //            //if (gradCandidate.IsFinalSubmit != null)
                //            //{
                //            //    if (gradCandidate.IsFinalSubmit == true)
                //            //    {
                //            //        btnSave_Education.Enabled = false;
                //            //        btnSave_Education.Visible = false;
                //            //    }
                //            //    else
                //            //    {
                //            //        btnSave_Education.Enabled = true;
                //            //        btnSave_Education.Visible = true;
                //            //    }
                //            //}
                //        }
                //        #endregion




                //        #region N/A Hide Save and Next Button for Bachelor Program Because Admission is closed
                //        //if (list.FirstOrDefault().EduCatID == 4)
                //        //{
                //        //    FileUploadPhoto.Visible = false;
                //        //    btnUploadPhoto.Visible = false;

                //        //    FileUploadSignature.Visible = false;
                //        //    btnUploadSignature.Visible = false;

                //        //    btnNext.Visible = false;
                //        //}
                //        #endregion

                //    }
                //}
                //catch (Exception ex)
                //{

                //    throw;
                //}
                #endregion

                #region N/A -- Hide Save and Next Button for Bachelor Program Because Admission is closed
                //try
                //{
                //    List<DAL.PropertySetup> propertySetupList = null; //new DAL.CandidateFormSl();
                //    int educationCategoryId = -1;
                //    using (var db = new GeneralDataManager())
                //    {
                //        propertySetupList = db.AdmissionDB.PropertySetups.Where(x => x.IsActive == true).ToList();
                //    }
                //    using (var db = new CandidateDataManager())
                //    {
                //        educationCategoryId = db.GetCandidateEducationCategoryID(cId);
                //    }

                //    ///<summary>
                //    ///
                //    /// IsActive == true && IsVisible == false
                //    /// Kew Submit Button Dekte prbe na.
                //    /// jokon admission date sas hoea jbe
                //    /// 
                //    /// 
                //    /// IsActive == true && IsVisible == true 
                //    /// sober jnno Open thkbe. Final Submit Dileo
                //    /// 
                //    /// 
                //    /// IsActive == false && IsVisible == any
                //    /// Sober jnno Open but final Submit dile r Show korbe na tader jnno
                //    /// 
                //    /// </summary>

                //    DAL.AdditionalInfo addFsModel = null;
                //    using (var db = new CandidateDataManager())
                //    {
                //        addFsModel = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                //    }

                //    if (addFsModel != null && Convert.ToBoolean(addFsModel.IsFinalSubmit) == true)
                //    {
                //        FileUploadPhoto.Visible = false;
                //        FileUploadSignature.Visible = false;
                //    }
                //    else
                //    {
                //        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).ToList().Count > 0)
                //        {
                //            if (educationCategoryId == 4)
                //            {
                //                FileUploadPhoto.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
                //                FileUploadSignature.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).Select(x => x.IsVisible).FirstOrDefault();
                //            }
                //            else
                //            {
                //                DAL.CandidateFormSl formSl = null;
                //                using (var db = new CandidateDataManager())
                //                {
                //                    formSl = db.GetCandidateFormSlByCandID_AD(cId);
                //                }

                //                if (formSl != null && formSl.AdmissionSetup != null)
                //                {
                //                    DAL.AdmissionUnitProgram admUnitProg = null;
                //                    using (var db = new OfficeDataManager())
                //                    {
                //                        admUnitProg = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.AcaCalID == formSl.AcaCalID
                //                                                                                    && x.EducationCategoryID == educationCategoryId
                //                                                                                    && x.AdmissionUnitID == formSl.AdmissionSetup.AdmissionUnitID
                //                                                                                    && x.IsActive == true).FirstOrDefault();
                //                    }

                //                    if (admUnitProg != null)
                //                    {
                //                        int programId = admUnitProg.ProgramID;

                //                        if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).FirstOrDefault() != null)
                //                        {
                //                            FileUploadPhoto.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                //                            FileUploadSignature.Visible = (bool)propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId && x.ProgramId == programId).Select(x => x.IsVisible).FirstOrDefault();
                //                        }
                //                        else
                //                        {
                //                            FileUploadPhoto.Visible = false;
                //                            FileUploadSignature.Visible = false;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        FileUploadPhoto.Visible = false;
                //                        FileUploadSignature.Visible = false;
                //                    }
                //                }
                //                else
                //                {
                //                    FileUploadPhoto.Visible = false;
                //                    FileUploadSignature.Visible = false;
                //                }
                //            }
                //        }
                //    }



                //    #region N/a
                //    ////... Save Button
                //    //if (propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).FirstOrDefault() != null)
                //    //{
                //    //    using (var db = new CandidateDataManager())
                //    //    {
                //    //        DAL.AdditionalInfo addFsModel = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId).FirstOrDefault();
                //    //        if ((addFsModel != null && Convert.ToBoolean(addFsModel.IsFinalSubmit) == false) || addFsModel == null)
                //    //        {
                //    //            DAL.CandidateFormSl fslModel = db.GetCandidateFormSlByCandID_AD(cId);
                //    //            if (fslModel.AdmissionSetup.AdmissionUnitID == 6) //Master of Business Administration (Professional).
                //    //            {
                //    //                FileUploadPhoto.Visible = true;
                //    //                FileUploadSignature.Visible = true;
                //    //            }
                //    //            else
                //    //            {
                //    //                bool isVisible = Convert.ToBoolean(propertySetupList.Where(x => x.PropertyTypeID == 4 && x.EducationCategoryID == educationCategoryId).FirstOrDefault().IsVisible);
                //    //                FileUploadPhoto.Visible = isVisible;
                //    //                FileUploadSignature.Visible = isVisible;
                //    //            }
                //    //        }
                //    //        else
                //    //        {
                //    //            FileUploadPhoto.Visible = false;
                //    //            FileUploadSignature.Visible = false;
                //    //        }
                //    //    }
                //    //} 
                //    #endregion


                //}
                //catch (Exception ex)
                //{
                //}
                #endregion


            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect("ApplicationDeclaration.aspx", false);
        }

        protected void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            long cId = -1;

            DAL.BasicInfo basicInfo = null;
            DAL.CandidatePayment candidatePayment = null;

            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);

                    basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                    candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();

                }// end using
            }


            //string base64String = croppedImageBase64.Value;
            // Check if we have processed passport photo data
            string base64String = croppedImageBase64.Value;
            bool hasProcessedPhoto = !string.IsNullOrEmpty(base64String);
            if (hasProcessedPhoto && !IsValidBase64String(base64String))
            {
                lblMessagePhoto.Text = "Invalid processed image data. Please try uploading again.";
                lblMessagePhoto.ForeColor = Color.Crimson;
                return;
            }
            //updated version with manual cropping
            if (hasProcessedPhoto || FileUploadPhoto.HasFile)
            {
                string fileName = "C-Ph-" + cId + ".jpg"; // Always use .jpg for cropped photos
                string filePath = "~/Upload/Candidate/Photo/";
                string tempFileName = "Temp-C-Ph-" + cId + ".jpg";
                string tempFilePath = "~/Upload/Candidate/TEMP/Photo/";
                int contentlength;

                // If we have cropped photo, use it; otherwise use original file
                if (hasProcessedPhoto)
                {
                    // Convert base64 to byte array for cropped photo
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    contentlength = imageBytes.Length;
                }
                else
                {
                    // Use original file upload
                    contentlength = int.Parse(FileUploadPhoto.PostedFile.ContentLength.ToString());
                }
                //updated version end

                //older version start without auto crop
                //if (FileUploadPhoto.HasFile) //!string.IsNullOrEmpty(base64String)
                //{
                //    String fileExtension = Path.GetExtension(FileUploadPhoto.PostedFile.FileName).ToLower();
                //    int contentlength = int.Parse(FileUploadPhoto.PostedFile.ContentLength.ToString());
                //    string fileName = "C-Ph-" + cId + fileExtension; // C for candidate, Ph for Photo
                //    string filePath = "~/Upload/Candidate/Photo/";
                //    string tempFileName = "Temp-C-Ph-" + cId + fileExtension; // C for candidate, Ph for Photo
                //    string tempFilePath = "~/Upload/Candidate/TEMP/Photo/";
                //olderversion end
                //base64String = base64String.Replace("data:image/jpeg;base64,", "");

                //// Convert base64 to byte array
                //byte[] imageBytes = Convert.FromBase64String(base64String);



                //string fileExtension = ".jpg"; // Adjust if needed
                //string fileName = "C-Ph-" + cId + fileExtension;
                //string filePath = "~/Upload/Candidate/Photo/";
                //string tempFileName = "Temp-C-Ph-" + cId + fileExtension; // C for candidate, Ph for Photo
                //string tempFilePath = "~/Upload/Candidate/TEMP/Photo/";

                if (contentlength < 204800)
                {
                    try
                    {

                        #region Image Validation Check 

                        bool ValidImage = false;
                        //try
                        //{
                        //    //System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(FileUploadPhoto.PostedFile.InputStream);
                        //    //ValidImage = true;

                        //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes))
                        //    {
                        //        //System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(ms);

                        //        // Save the image to a file, replace if it already exists 

                        //        //bmpPostedImage.Save("output.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        //        //if(File.Exists(Server.MapPath(filePath + fileName)))
                        //        //{
                        //        //    File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                        //        //    //delete the original file
                        //        //    File.Delete(Server.MapPath(filePath + fileName));

                        //        //    FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));
                        //        //}
                        //        //bmpPostedImage.Save(filePath + fileName);

                        //        //System.Drawing.Image image = System.Drawing.Image.FromStream(ms); // For example, save the image to a file
                        //        //image.Save(filePath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                        //        //using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                        //        //{
                        //        //    // Save the image to a file, replace if it already exists 
                        //        //    using (FileStream fs = new FileStream(filePath, FileMode.Create)) 
                        //        //    {
                        //        //        img.Save(fs,);
                        //        //    } 
                        //        //}
                        //        using (var stream = System.IO.File.Create(Server.MapPath(filePath + fileName)))
                        //        {
                        //            ms.CopyTo(stream);
                        //        }
                        //        ValidImage = true;
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    ValidImage = false;
                        //}
                        //older version
                        //try
                        //{
                        //    System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(FileUploadPhoto.PostedFile.InputStream);
                        //    ValidImage = true;
                        //}
                        //catch (Exception ex)
                        //{
                        //    ValidImage = false;
                        //}

                        //updated version with manual cropping
                        try
                        {
                            if (hasProcessedPhoto)
                            {
                                // Validate cropped image
                                byte[] imageBytes = Convert.FromBase64String(base64String);
                                using (var ms = new MemoryStream(imageBytes))
                                {
                                    System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(ms);
                                    ValidImage = true;
                                }
                            }
                            else
                            {
                                // Validate original file
                                System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(FileUploadPhoto.PostedFile.InputStream);
                                ValidImage = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            ValidImage = false;
                        }
                        //updated version end
                        if (!ValidImage)
                        {
                            lblMessagePhoto.Text = "Invalid Photo.Please upload another photo";
                            lblMessagePhoto.ForeColor = Color.Crimson;
                            return;
                        }


                        lblMessagePhoto.Text = "Please wait...";
                        lblMessagePhoto.ForeColor = Color.Crimson;

                        if (!ValidImage)
                        {
                            lblMessagePhoto.Text = "Invalid Photo.Please upload another photo";
                            lblMessagePhoto.ForeColor = Color.Crimson;
                            return;
                        }

                        #endregion
                        lblMessagePhoto.Text = "Please wait...";
                        lblMessagePhoto.ForeColor = Color.Crimson;

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));
                            //older version
                            //FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));
                            //updated version with manual cropping
                            if (hasProcessedPhoto)
                            {
                                // Save cropped photo
                                byte[] imageBytes = Convert.FromBase64String(base64String);
                                File.WriteAllBytes(Server.MapPath(filePath + fileName), imageBytes);
                            }
                            else
                            {
                                // Save original file
                                FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));
                            }
                            //updated version end
                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }

                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        //dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Photo.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = cId;
                                    newDocumentDetailObj.DateCreated = DateTime.Now;
                                    newDocumentDetailObj.ModifiedBy = null;
                                    newDocumentDetailObj.DateModified = null;

                                    long newDocumentDetailID = -1;
                                    using (var dbInsertDocumentDetail = new CandidateDataManager())
                                    {
                                        dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.Document documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = image/photo
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.Document>(documentObjToBeUpdated);
                                            }
                                        }
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        //dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Photo.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = cId;
                                newDocumentDetailObj.DateCreated = DateTime.Now;
                                newDocumentDetailObj.ModifiedBy = null;
                                newDocumentDetailObj.DateModified = null;

                                long newDocumentDetailID = -1;
                                using (var dbInsertDocumentDetail = new CandidateDataManager())
                                {
                                    dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.Document newDocumentObj = new DAL.Document();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 2; //2 = Image;
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }


                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //dLog.DateCreated = DateTime.Now;
                                    //dLog.EventName = "Attachment Info Insert (Photo) (Candidate)";
                                    //dLog.PageName = "ApplicationAttachment.aspx";
                                    //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Insert Attachment Information.";
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
                                    dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                    dLog.PageName = "ApplicationAttachment.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Photo.";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //try
                                //{
                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.EventName = "Attachment Info Insert (Photo) (Candidate)";
                                //    dLog.PageName = "ApplicationAttachment.aspx";
                                //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Attachment Information.";
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = "Success";
                                //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion

                            }//end if-else

                            //delete the temp file
                            if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
                            {
                                File.Delete(Server.MapPath(tempFilePath + tempFileName));
                            }

                        }//end if
                        #endregion
                        #region IF FILE DOES NOT EXIST
                        //else if file does not exist
                        else
                        {   //old one
                            //FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));
                            //start new
                            if (hasProcessedPhoto)
                            {
                                // Save cropped photo
                                byte[] imageBytes = Convert.FromBase64String(base64String);
                                File.WriteAllBytes(Server.MapPath(filePath + fileName), imageBytes);
                            }
                            else
                            {
                                // Save original file
                                FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));
                            }
                            //end new
                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        //dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Photo.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion


                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion


                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = cId;
                                    newDocumentDetailObj.DateCreated = DateTime.Now;
                                    newDocumentDetailObj.ModifiedBy = null;
                                    newDocumentDetailObj.DateModified = null;

                                    long newDocumentDetailID = -1;
                                    using (var dbInsertDocumentDetail = new CandidateDataManager())
                                    {
                                        dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.Document documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = image/photo
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.Document>(documentObjToBeUpdated);
                                            }
                                        }
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        //dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Update Photo.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Photo) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion


                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = cId;
                                newDocumentDetailObj.DateCreated = DateTime.Now;
                                newDocumentDetailObj.ModifiedBy = null;
                                newDocumentDetailObj.DateModified = null;

                                long newDocumentDetailID = -1;
                                using (var dbInsertDocumentDetail = new CandidateDataManager())
                                {
                                    dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.Document newDocumentObj = new DAL.Document();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 2; //2 = Image;
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }


                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //dLog.DateCreated = DateTime.Now;
                                    //dLog.EventName = "Attachment Info Insert (Photo) (Candidate)";
                                    //dLog.PageName = "ApplicationAttachment.aspx";
                                    //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Insert Attachment Information.";
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
                                    dLog.EventName = "Attachment Info Insert (Photo) (Candidate)";
                                    dLog.PageName = "ApplicationAttachment.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.ToString() + "; Upload Photo.";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //try
                                //{
                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.EventName = "Attachment Info Insert (Photo) (Candidate)";
                                //    dLog.PageName = "ApplicationAttachment.aspx";
                                //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Attachment Information.";
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = "Success";
                                //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion

                            }//end if-else

                        }//end if-else
                        #endregion

                        LoadCandidateData(uId);

                        // Add cache-busting parameter to force browser to reload the image
                        ImagePhoto.ImageUrl = filePath + fileName + "?v=" + DateTime.Now.Ticks;

                        lblMessagePhoto.Text = "Photo uploaded";
                        lblMessagePhoto.ForeColor = Color.Green;

                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Unable to upload photo.";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }
                else
                {
                    lblMessagePhoto.Text = "File size larger than 200kb.";
                    lblMessagePhoto.ForeColor = Color.Crimson;
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
                //Response.Redirect("~/Admission/Message.aspx?message=" + "Something is not right... Please contact administrator.&type=danger", false);
                lblMessage.Text = "Please select a file (Candidate's Photo) to upload";
                lblMessage.ForeColor = Color.Crimson;
            }
        }

        protected void btnUploadSignature_Click(object sender, EventArgs e)
        {
            long cId = -1;

            DAL.BasicInfo basicInfo = null;
            DAL.CandidatePayment candidatePayment = null;

            if (uId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    cId = db.GetCandidateIdByUserID_ND(uId);

                    basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                    candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();

                }// end using
            }
            //old version start
            //if (FileUploadSignature.HasFile)
            //{
            //    String fileExtension = Path.GetExtension(FileUploadSignature.PostedFile.FileName).ToLower();
            //    int contentlength = int.Parse(FileUploadSignature.PostedFile.ContentLength.ToString());
            //    string fileName = "C-Sn-" + cId + fileExtension; // C for candidate, Sn for Signature
            //    string filePath = "~/Upload/Candidate/Signature/";
            //    string tempFileName = "Temp-C-Sn-" + cId + fileExtension; // C for candidate, Sn for Signature
            //    string tempFilePath = "~/Upload/Candidate/TEMP/Signature/";
            //old version end
            

            // new version start
            string base64SignatureString = croppedSignatureBase64.Value;
            bool hasProcessedSignature = !string.IsNullOrEmpty(base64SignatureString);

            if (hasProcessedSignature && !IsValidBase64String(base64SignatureString))
            {
                lblMessageSign.Text = "Invalid processed signature data. Please try uploading again.";
                lblMessageSign.ForeColor = Color.Crimson;
                return;
            }

            if (hasProcessedSignature || FileUploadSignature.HasFile)
            {
                string fileName = "C-Sn-" + cId + ".jpg"; // Always use .jpg for cropped signatures
                string filePath = "~/Upload/Candidate/Signature/";
                string tempFileName = "Temp-C-Sn-" + cId + ".jpg";
                string tempFilePath = "~/Upload/Candidate/TEMP/Signature/";
                int contentlength;

                // If we have cropped signature, use it; otherwise use original file
                if (hasProcessedSignature)
                {
                    // Convert base64 to byte array for cropped signature
                    byte[] signatureBytes = Convert.FromBase64String(base64SignatureString);
                    contentlength = signatureBytes.Length;
                }
                else
                {
                    // Use original file upload
                    contentlength = int.Parse(FileUploadSignature.PostedFile.ContentLength.ToString());
                }
                //new version end
                if (contentlength < 204800)
                {
                    try
                    {
                        #region Image Validation Check for Signature
                        bool ValidSignatureImage = false;
                        try
                        {
                            if (hasProcessedSignature)
                            {
                                // Validate cropped signature
                                byte[] signatureBytes = Convert.FromBase64String(base64SignatureString);
                                using (var ms = new MemoryStream(signatureBytes))
                                {
                                    System.Drawing.Bitmap bmpPostedSignature = new System.Drawing.Bitmap(ms);
                                    ValidSignatureImage = true;
                                }
                            }
                            else
                            {
                                // Validate original file
                                System.Drawing.Bitmap bmpPostedSignature = new System.Drawing.Bitmap(FileUploadSignature.PostedFile.InputStream);
                                ValidSignatureImage = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            ValidSignatureImage = false;
                        }

                        if (!ValidSignatureImage)
                        {
                            lblMessageSign.Text = "Invalid Signature. Please upload another signature";
                            lblMessageSign.ForeColor = Color.Crimson;
                            return;
                        }
                        #endregion

                        lblMessageSign.Text = "Please wait...";
                        lblMessageSign.ForeColor = Color.Crimson;

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));
                            //old version
                            //FileUploadSignature.SaveAs(Server.MapPath(filePath + fileName));
                            //new version
                            if (hasProcessedSignature)
                            {
                                // Save cropped signature
                                byte[] signatureBytes = Convert.FromBase64String(base64SignatureString);
                                File.WriteAllBytes(Server.MapPath(filePath + fileName), signatureBytes);
                            }
                            else
                            {
                                // Save original file
                                FileUploadSignature.SaveAs(Server.MapPath(filePath + fileName));
                            }

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        //dLog.PageName = "CandApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Signature.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = cId;
                                    newDocumentDetailObj.DateCreated = DateTime.Now;
                                    newDocumentDetailObj.ModifiedBy = null;
                                    newDocumentDetailObj.DateModified = null;

                                    long newDocumentDetailID = -1;
                                    using (var dbInsertDocumentDetail = new CandidateDataManager())
                                    {
                                        dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.Document documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.Document>(documentObjToBeUpdated);
                                            }
                                        }
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        //dLog.PageName = "CandApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Signature.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = cId;
                                newDocumentDetailObj.DateCreated = DateTime.Now;
                                newDocumentDetailObj.ModifiedBy = null;
                                newDocumentDetailObj.DateModified = null;

                                long newDocumentDetailID = -1;
                                using (var dbInsertDocumentDetail = new CandidateDataManager())
                                {
                                    dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.Document newDocumentObj = new DAL.Document();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 3; //3 = signature
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }


                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //dLog.DateCreated = DateTime.Now;
                                    //dLog.EventName = "Attachment Info Insert (Signature) (Admin)";
                                    //dLog.PageName = "CandApplicationAttachment.aspx";
                                    //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Attachment Information.";
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
                                    dLog.EventName = "Attachment Info Insert (Signature) (Candidate)";
                                    dLog.PageName = "ApplicationAttachment.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Upload Signature.";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //try
                                //{
                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.EventName = "Attachment Info Insert (Signature) (Candidate)";
                                //    dLog.PageName = "ApplicationAttachment.aspx";
                                //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Attachment Information.";
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = "Success";
                                //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion


                            }//end if-else

                            //delete the temp file
                            if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
                            {
                                File.Delete(Server.MapPath(tempFilePath + tempFileName));
                            }

                        }//end if
                        #endregion
                        #region IF FILE DOES NOT EXIST
                        //else if file does not exist
                        else
                        {
                            //old one
                            //FileUploadSignature.SaveAs(Server.MapPath(filePath + fileName));
                            //start new
                            if (hasProcessedSignature)
                            {
                                // Save cropped signature
                                byte[] signatureBytes = Convert.FromBase64String(base64SignatureString);
                                File.WriteAllBytes(Server.MapPath(filePath + fileName), signatureBytes);
                            }
                            else
                            {
                                // Save original file
                                FileUploadSignature.SaveAs(Server.MapPath(filePath + fileName));
                            }
                            //end new
                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        //dLog.PageName = "CandApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Signature.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = cId;
                                    newDocumentDetailObj.DateCreated = DateTime.Now;
                                    newDocumentDetailObj.ModifiedBy = null;
                                    newDocumentDetailObj.DateModified = null;

                                    long newDocumentDetailID = -1;
                                    using (var dbInsertDocumentDetail = new CandidateDataManager())
                                    {
                                        dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.Document documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.Document>(documentObjToBeUpdated);
                                            }
                                        }
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
                                        //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        //dLog.DateCreated = DateTime.Now;
                                        //dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        //dLog.PageName = "CandApplicationAttachment.aspx";
                                        //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
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
                                        dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                        dLog.PageName = "ApplicationAttachment.aspx";
                                        //dLog.OldData = logOldObject;
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Signature.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion

                                    //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    //try
                                    //{
                                    //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //    dLog.DateCreated = DateTime.Now;
                                    //    dLog.EventName = "Attachment Info Update (Signature) (Candidate)";
                                    //    dLog.PageName = "ApplicationAttachment.aspx";
                                    //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Update Attachment Information.";
                                    //    dLog.UserId = uId;
                                    //    dLog.Attribute1 = "Success";
                                    //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                    //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                    //    LogWriter.DataLogWriter(dLog);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //#endregion

                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = cId;
                                newDocumentDetailObj.DateCreated = DateTime.Now;
                                newDocumentDetailObj.ModifiedBy = null;
                                newDocumentDetailObj.DateModified = null;

                                long newDocumentDetailID = -1;
                                using (var dbInsertDocumentDetail = new CandidateDataManager())
                                {
                                    dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.Document newDocumentObj = new DAL.Document();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 3; //3 = Signature
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }


                                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                try
                                {
                                    //DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    //dLog.DateCreated = DateTime.Now;
                                    //dLog.EventName = "Attachment Info Insert (Signature) (Admin)";
                                    //dLog.PageName = "CandApplicationAttachment.aspx";
                                    //dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Attachment Information.";
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
                                    dLog.EventName = "Attachment Info Insert (Signature) (Candidate)";
                                    dLog.PageName = "ApplicationAttachment.aspx";
                                    //dLog.OldData = logOldObject;
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Upload Signature.";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion

                                //#region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                //try
                                //{
                                //    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                //    dLog.DateCreated = DateTime.Now;
                                //    dLog.EventName = "Attachment Info Insert (Signature) (Candidate)";
                                //    dLog.PageName = "ApplicationAttachment.aspx";
                                //    dLog.NewData = "Candidate: " + basicInfo.FirstName.ToString() + "; PaymentID: " + candidatePayment.PaymentId.ToString() + "; CandidateID: " + candidatePayment.CandidateID.ToString() + "; Insert Attachment Information.";
                                //    dLog.UserId = uId;
                                //    dLog.Attribute1 = "Success";
                                //    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                //        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;
                                //    LogWriter.DataLogWriter(dLog);
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                //#endregion


                            }//end if-else
                        }//end if-else
                        #endregion

                        LoadCandidateData(uId);
                        ImageSignature.ImageUrl = filePath + fileName + "?v=" + DateTime.Now.Ticks;
                        lblMessageSign.Text = "Signature uploaded";
                        lblMessageSign.ForeColor = Color.Green;

                    }
                    catch (Exception ex)
                    {
                        lblMessageSign.Text = "Unable to upload signature.";
                        lblMessageSign.ForeColor = Color.Crimson;
                    }
                }
                else
                {
                    lblMessageSign.Text = "File size larger than 200kb.";
                    lblMessageSign.ForeColor = Color.Crimson;
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
                //Response.Redirect("~/Admission/Message.aspx?message=" + "Something is not right... Please contact administrator.&type=danger", false);
                lblMessage.Text = "Please select a file (Candidate's Signature with Name) to upload";
                lblMessage.ForeColor = Color.Crimson;
            }
        }

        
        private bool IsValidBase64String(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return false;

            try
            {
                // Remove any data URL prefix if present
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }

                // Check if it's valid base64
                byte[] data = Convert.FromBase64String(base64String);
                return data.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        #region N/A
        //protected void btnUploadFinGuarSignature_Click(object sender, EventArgs e)
        //{
        //    long cId = -1;

        //    if (uId > 0)
        //    {
        //        using (var db = new CandidateDataManager())
        //        {
        //            cId = db.GetCandidateIdByUserID_ND(uId);
        //        }// end using
        //    }
        //    if (FileUploadFinGuarSignature.HasFile)
        //    {
        //        String fileExtension = Path.GetExtension(FileUploadFinGuarSignature.PostedFile.FileName).ToLower();
        //        int contentlength = int.Parse(FileUploadFinGuarSignature.PostedFile.ContentLength.ToString());
        //        string fileName = "FG-Sn-" + cId + fileExtension; // FG for Financial Guarantor, Sn for Signature
        //        string filePath = "~/Upload/FinancialGuarantor/Signature/";
        //        string tempFileName = "Temp-FG-Sn-" + cId + fileExtension; // FG for Financial Guarantor, Sn for Signature
        //        string tempFilePath = "~/Upload/FinancialGuarantor/TEMP/Signature/";

        //        if (contentlength < 204800)
        //        {
        //            try
        //            {
        //                lblMessagePhoto.Text = "Please wait...";
        //                lblMessagePhoto.ForeColor = Color.Crimson;

        //                #region IF FILE EXISTS
        //                //check if file exists
        //                if (File.Exists(Server.MapPath(filePath + fileName)))
        //                {

        //                    //move the file to TEMP
        //                    File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
        //                    //delete the original file
        //                    File.Delete(Server.MapPath(filePath + fileName));

        //                    //save file
        //                    FileUploadFinGuarSignature.SaveAs(Server.MapPath(filePath + fileName));

        //                    DAL.Document documentObj = null;
        //                    using (var dbDocument = new CandidateDataManager())
        //                    {
        //                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 7); //7 = FG Signature
        //                    }
        //                    if (documentObj != null) //do not update document, document exists, only update document details
        //                    {
        //                        DAL.DocumentDetail documentDetailObj = null;
        //                        using (var dbDocumentDetails = new CandidateDataManager())
        //                        {
        //                            documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
        //                        }
        //                        if (documentDetailObj != null) //document detail exists, update document details
        //                        {
        //                            documentDetailObj.URL = filePath + fileName;
        //                            documentDetailObj.Name = fileName;
        //                            documentDetailObj.ModifiedBy = cId;
        //                            documentDetailObj.DateModified = DateTime.Now;

        //                            using (var dbUpdateDocumentDetail = new CandidateDataManager())
        //                            {
        //                                dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
        //                            }
        //                        }
        //                        else //document detail does not exist, insert new document detail, then update document
        //                        {
        //                            DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
        //                            newDocumentDetailObj.URL = filePath + fileName;
        //                            newDocumentDetailObj.Name = fileName;
        //                            newDocumentDetailObj.Description = null;
        //                            newDocumentDetailObj.Attribute1 = null;
        //                            newDocumentDetailObj.Attribute2 = null;
        //                            newDocumentDetailObj.CreatedBy = cId;
        //                            newDocumentDetailObj.DateCreated = DateTime.Now;
        //                            newDocumentDetailObj.ModifiedBy = null;
        //                            newDocumentDetailObj.DateModified = null;

        //                            long newDocumentDetailID = -1;
        //                            using (var dbInsertDocumentDetail = new CandidateDataManager())
        //                            {
        //                                dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
        //                                newDocumentDetailID = newDocumentDetailObj.ID;
        //                            }
        //                            if (newDocumentDetailID > 0)
        //                            {
        //                                DAL.Document documentObjToBeUpdated = null;
        //                                using (var dbGetDocumentObj = new CandidateDataManager())
        //                                {
        //                                    documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 7); //7 = FG Signature
        //                                }
        //                                if (documentObjToBeUpdated != null)
        //                                {
        //                                    documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
        //                                    using (var dbUpdateDocumentObj = new CandidateDataManager())
        //                                    {
        //                                        dbUpdateDocumentObj.Update<DAL.Document>(documentObjToBeUpdated);
        //                                    }
        //                                }
        //                            }
        //                        }//end if-else
        //                    }//end if
        //                    else //insert document  (first document details, then document)
        //                    {
        //                        DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
        //                        newDocumentDetailObj.URL = filePath + fileName;
        //                        newDocumentDetailObj.Name = fileName;
        //                        newDocumentDetailObj.Description = null;
        //                        newDocumentDetailObj.Attribute1 = null;
        //                        newDocumentDetailObj.Attribute2 = null;
        //                        newDocumentDetailObj.CreatedBy = cId;
        //                        newDocumentDetailObj.DateCreated = DateTime.Now;
        //                        newDocumentDetailObj.ModifiedBy = null;
        //                        newDocumentDetailObj.DateModified = null;

        //                        long newDocumentDetailID = -1;
        //                        using (var dbInsertDocumentDetail = new CandidateDataManager())
        //                        {
        //                            dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
        //                            newDocumentDetailID = newDocumentDetailObj.ID;
        //                        }

        //                        if (newDocumentDetailID > 0)
        //                        {
        //                            DAL.Document newDocumentObj = new DAL.Document();
        //                            newDocumentObj.CandidateID = cId;
        //                            newDocumentObj.DocumentDetailsID = newDocumentDetailID;
        //                            newDocumentObj.DocumentTypeID = 7; //7 = fg signature
        //                            newDocumentObj.Attribute1 = null;
        //                            newDocumentObj.Attribute2 = null;
        //                            newDocumentObj.DateCreated = DateTime.Now;
        //                            newDocumentObj.CreatedBy = cId;

        //                            using (var dbInsertDocument = new CandidateDataManager())
        //                            {
        //                                dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
        //                            }
        //                        }
        //                    }//end if-else

        //                    //delete the temp file
        //                    if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
        //                    {
        //                        File.Delete(Server.MapPath(tempFilePath + tempFileName));
        //                    }

        //                }//end if
        //                #endregion
        //                #region IF FILE DOES NOT EXIST
        //                //else if file does not exist
        //                else
        //                {

        //                    FileUploadFinGuarSignature.SaveAs(Server.MapPath(filePath + fileName));

        //                    DAL.Document documentObj = null;
        //                    using (var dbDocument = new CandidateDataManager())
        //                    {
        //                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 7); //7 = FG Signature
        //                    }
        //                    if (documentObj != null) //do not update document, document exists, only update document details
        //                    {
        //                        DAL.DocumentDetail documentDetailObj = null;
        //                        using (var dbDocumentDetails = new CandidateDataManager())
        //                        {
        //                            documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
        //                        }
        //                        if (documentDetailObj != null) //document detail exists, update document details
        //                        {
        //                            documentDetailObj.URL = filePath + fileName;
        //                            documentDetailObj.Name = fileName;
        //                            documentDetailObj.ModifiedBy = cId;
        //                            documentDetailObj.DateModified = DateTime.Now;

        //                            using (var dbUpdateDocumentDetail = new CandidateDataManager())
        //                            {
        //                                dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
        //                            }
        //                        }
        //                        else //document detail does not exist, insert new document detail, then update document
        //                        {
        //                            DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
        //                            newDocumentDetailObj.URL = filePath + fileName;
        //                            newDocumentDetailObj.Name = fileName;
        //                            newDocumentDetailObj.Description = null;
        //                            newDocumentDetailObj.Attribute1 = null;
        //                            newDocumentDetailObj.Attribute2 = null;
        //                            newDocumentDetailObj.CreatedBy = cId;
        //                            newDocumentDetailObj.DateCreated = DateTime.Now;
        //                            newDocumentDetailObj.ModifiedBy = null;
        //                            newDocumentDetailObj.DateModified = null;

        //                            long newDocumentDetailID = -1;
        //                            using (var dbInsertDocumentDetail = new CandidateDataManager())
        //                            {
        //                                dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
        //                                newDocumentDetailID = newDocumentDetailObj.ID;
        //                            }
        //                            if (newDocumentDetailID > 0)
        //                            {
        //                                DAL.Document documentObjToBeUpdated = null;
        //                                using (var dbGetDocumentObj = new CandidateDataManager())
        //                                {
        //                                    documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 7); //7 = FG Signature
        //                                }
        //                                if (documentObjToBeUpdated != null)
        //                                {
        //                                    documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
        //                                    using (var dbUpdateDocumentObj = new CandidateDataManager())
        //                                    {
        //                                        dbUpdateDocumentObj.Update<DAL.Document>(documentObjToBeUpdated);
        //                                    }
        //                                }
        //                            }
        //                        }//end if-else
        //                    }//end if
        //                    else //insert document  (first document details, then document)
        //                    {
        //                        DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
        //                        newDocumentDetailObj.URL = filePath + fileName;
        //                        newDocumentDetailObj.Name = fileName;
        //                        newDocumentDetailObj.Description = null;
        //                        newDocumentDetailObj.Attribute1 = null;
        //                        newDocumentDetailObj.Attribute2 = null;
        //                        newDocumentDetailObj.CreatedBy = cId;
        //                        newDocumentDetailObj.DateCreated = DateTime.Now;
        //                        newDocumentDetailObj.ModifiedBy = null;
        //                        newDocumentDetailObj.DateModified = null;

        //                        long newDocumentDetailID = -1;
        //                        using (var dbInsertDocumentDetail = new CandidateDataManager())
        //                        {
        //                            dbInsertDocumentDetail.Insert<DAL.DocumentDetail>(newDocumentDetailObj);
        //                            newDocumentDetailID = newDocumentDetailObj.ID;
        //                        }

        //                        if (newDocumentDetailID > 0)
        //                        {
        //                            DAL.Document newDocumentObj = new DAL.Document();
        //                            newDocumentObj.CandidateID = cId;
        //                            newDocumentObj.DocumentDetailsID = newDocumentDetailID;
        //                            newDocumentObj.DocumentTypeID = 7; //7 = FG Signature
        //                            newDocumentObj.Attribute1 = null;
        //                            newDocumentObj.Attribute2 = null;
        //                            newDocumentObj.DateCreated = DateTime.Now;
        //                            newDocumentObj.CreatedBy = cId;

        //                            using (var dbInsertDocument = new CandidateDataManager())
        //                            {
        //                                dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
        //                            }
        //                        }
        //                    }//end if-else





        //                }//end if-else
        //                #endregion

        //                LoadCandidateData(uId);

        //                lblMessageFinGuarSign.Text = "Financial guarantor's signature uploaded";
        //                lblMessageFinGuarSign.ForeColor = Color.Green;

        //            }
        //            catch (Exception ex)
        //            {
        //                lblMessageFinGuarSign.Text = "Unable to upload financial guarantor's signature.";
        //                lblMessageFinGuarSign.ForeColor = Color.Crimson;
        //            }
        //        }
        //        else
        //        {
        //            lblMessageFinGuarSign.Text = "File size larger than 200kb.";
        //            lblMessageFinGuarSign.ForeColor = Color.Crimson;
        //        }
        //    }// end if (FileUploadBanner.HasFile)
        //    else
        //    {
        //        //Response.Redirect("~/Admission/Message.aspx?message=" + "Something is not right... Please contact administrator.&type=danger", false);
        //        lblMessage.Text = "Please select a file (Financial Guarantor's Signature) to upload";
        //        lblMessage.ForeColor = Color.Crimson;
        //    }
        //} 
        #endregion
    }


}