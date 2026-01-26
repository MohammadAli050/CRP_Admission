using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office.CandidateInfo
{
    public partial class CandApplicationAttachment : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        string userName = "";

        private const int MAX_FILE_SIZE = 153600; // 150KB in bytes
        private static readonly string[] ALLOWED_EXTENSIONS = { ".jpg", ".jpeg" };

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
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


                // Set navigation URLs
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
                uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
                uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

                if (cId > 0)
                {
                    LoadCandidateData(cId);
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void LoadCandidateData(long cId)
        {
            if (cId > 0)
            {
                using (var db = new CandidateDataManager())
                {
                    List<DAL.Document> documentList = db.GetAllDocumentByCandidateID_AD(cId);
                    if (documentList != null)
                    {
                        DAL.DocumentDetail photoDocDetailObj = documentList.Where(c => c.DocumentTypeID == 2).Select(c => c.DocumentDetail).FirstOrDefault();
                        DAL.DocumentDetail signDocDetailObj = documentList.Where(c => c.DocumentTypeID == 3).Select(c => c.DocumentDetail).FirstOrDefault();

                        if (photoDocDetailObj != null)
                        {
                            ImagePhoto.ImageUrl = photoDocDetailObj.URL + "?v=" + DateTime.Now.Ticks;
                        }

                        if (signDocDetailObj != null)
                        {
                            ImageSignature.ImageUrl = signDocDetailObj.URL + "?v=" + DateTime.Now.Ticks;
                        }
                    }
                }
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

        protected void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            lblMessagePhoto.Text = string.Empty;

            if (cId <= 0 || uId <= 0)
            {
                lblMessage.Text = "Invalid Request!";
                lblMessage.ForeColor = Color.Crimson;
                return;
            }

            DAL.BasicInfo basicInfo = null;
            DAL.CandidatePayment candidatePayment = null;

            using (var db = new CandidateDataManager())
            {
                basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
            }

            // Check if we have cropped photo data
            string base64String = croppedImageBase64.Value;
            bool hasProcessedPhoto = !string.IsNullOrEmpty(base64String);

            if (hasProcessedPhoto && !IsValidBase64String(base64String))
            {
                lblMessagePhoto.Text = "Invalid processed image data. Please try uploading again.";
                lblMessagePhoto.ForeColor = Color.Crimson;
                return;
            }

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

                if (contentlength < 204800)
                {
                    try
                    {
                        #region Image Validation Check
                        bool ValidImage = false;
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

                        if (!ValidImage)
                        {
                            lblMessagePhoto.Text = "Invalid Photo. Please upload another photo";
                            lblMessagePhoto.ForeColor = Color.Crimson;
                            return;
                        }
                        #endregion

                        lblMessagePhoto.Text = "Please wait...";
                        lblMessagePhoto.ForeColor = Color.Crimson;

                        #region IF FILE EXISTS
                        // Check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            // Create temp directory if it doesn't exist
                            string tempDir = Server.MapPath(tempFilePath);
                            if (!Directory.Exists(tempDir))
                                Directory.CreateDirectory(tempDir);

                            // Move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            // Delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            // Save new file
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

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                            }

                            if (documentObj != null) // Do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }

                                if (documentDetailObj != null) // Document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = uId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Photo) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Photo.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion
                                }
                                else // Document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = uId;
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

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Photo) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Photo.";
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
                            else // Insert document (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = uId;
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
                                    newDocumentObj.CreatedBy = uId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }

                                #region LOG
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "Attachment Info Insert (Photo) (Admin)";
                                    dLog.PageName = "CandApplicationAttachment.aspx";
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Upload Photo.";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion
                            }

                            // Delete the temp file
                            if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
                            {
                                File.Delete(Server.MapPath(tempFilePath + tempFileName));
                            }
                        }
                        #endregion

                        #region IF FILE DOES NOT EXIST
                        // Else if file does not exist
                        else
                        {
                            // Create upload directory if it doesn't exist
                            string uploadDir = Server.MapPath(filePath);
                            if (!Directory.Exists(uploadDir))
                                Directory.CreateDirectory(uploadDir);

                            // Save new file
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

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                            }

                            if (documentObj != null) // Do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }

                                if (documentDetailObj != null) // Document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = uId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Photo) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Photo.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion
                                }
                                else // Document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = uId;
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

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Photo) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Photo.";
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
                            else // Insert document (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = uId;
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
                                    newDocumentObj.CreatedBy = uId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }

                                #region LOG
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "Attachment Info Insert (Photo) (Admin)";
                                    dLog.PageName = "CandApplicationAttachment.aspx";
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Upload Photo.";
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
                        #endregion

                        LoadCandidateData(cId);

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
            }
            else
            {
                lblMessage.Text = "Please select a file (Candidate's Photo) to upload";
                lblMessage.ForeColor = Color.Crimson;
            }
        }

        protected void btnUploadSignature_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            lblMessageSign.Text = string.Empty;

            if (cId <= 0 || uId <= 0)
            {
                lblMessage.Text = "Invalid Request!";
                lblMessage.ForeColor = Color.Crimson;
                return;
            }

            DAL.BasicInfo basicInfo = null;
            DAL.CandidatePayment candidatePayment = null;

            using (var db = new CandidateDataManager())
            {
                basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
            }

            // Check if we have cropped signature data
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
                        // Check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            // Create temp directory if it doesn't exist
                            string tempDir = Server.MapPath(tempFilePath);
                            if (!Directory.Exists(tempDir))
                                Directory.CreateDirectory(tempDir);

                            // Move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            // Delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            // Save new file
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

                            if (documentObj != null) // Do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }

                                if (documentDetailObj != null) // Document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = uId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Signature.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion
                                }
                                else // Document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = uId;
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

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Signature.";
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
                            else // Insert document (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = uId;
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
                                    newDocumentObj.CreatedBy = uId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }

                                #region LOG
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "Attachment Info Insert (Signature) (Admin)";
                                    dLog.PageName = "CandApplicationAttachment.aspx";
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Upload Signature.";
                                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                    LogWriter.DataLogWriter(dLog);
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion
                            }

                            // Delete the temp file
                            if (File.Exists(Server.MapPath(tempFilePath + tempFileName)))
                            {
                                File.Delete(Server.MapPath(tempFilePath + tempFileName));
                            }
                        }
                        #endregion

                        #region IF FILE DOES NOT EXIST
                        // Else if file does not exist
                        else
                        {
                            // Create upload directory if it doesn't exist
                            string uploadDir = Server.MapPath(filePath);
                            if (!Directory.Exists(uploadDir))
                                Directory.CreateDirectory(uploadDir);

                            // Save new file
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

                            if (documentObj != null) // Do not update document, document exists, only update document details
                            {
                                DAL.DocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }

                                if (documentDetailObj != null) // Document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = uId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Signature.";
                                        dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                                            SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; C-ID:" + cId;

                                        LogWriter.DataLogWriter(dLog);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion
                                }
                                else // Document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                    newDocumentDetailObj.URL = filePath + fileName;
                                    newDocumentDetailObj.Name = fileName;
                                    newDocumentDetailObj.Description = null;
                                    newDocumentDetailObj.Attribute1 = null;
                                    newDocumentDetailObj.Attribute2 = null;
                                    newDocumentDetailObj.CreatedBy = uId;
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

                                    #region LOG
                                    try
                                    {
                                        DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                        dLog.DateTime = DateTime.Now;
                                        dLog.DateCreated = DateTime.Now;
                                        dLog.UserId = uId;
                                        dLog.CandidateId = cId;
                                        dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
                                        dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Update Signature.";
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
                            else // Insert document (first document details, then document)
                            {
                                DAL.DocumentDetail newDocumentDetailObj = new DAL.DocumentDetail();
                                newDocumentDetailObj.URL = filePath + fileName;
                                newDocumentDetailObj.Name = fileName;
                                newDocumentDetailObj.Description = null;
                                newDocumentDetailObj.Attribute1 = null;
                                newDocumentDetailObj.Attribute2 = null;
                                newDocumentDetailObj.CreatedBy = uId;
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
                                    newDocumentObj.CreatedBy = uId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.Document>(newDocumentObj);
                                    }
                                }

                                #region LOG
                                try
                                {
                                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                                    dLog.DateTime = DateTime.Now;
                                    dLog.DateCreated = DateTime.Now;
                                    dLog.UserId = uId;
                                    dLog.CandidateId = cId;
                                    dLog.EventName = "Attachment Info Insert (Signature) (Admin)";
                                    dLog.PageName = "CandApplicationAttachment.aspx";
                                    dLog.NewData = "User: " + userName + "; Candidate: " + basicInfo?.FirstName + "; PaymentID: " + candidatePayment?.PaymentId + "; Upload Signature.";
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
                        #endregion

                        LoadCandidateData(cId);
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
            }
            else
            {
                lblMessage.Text = "Please select a file (Candidate's Signature with Name) to upload";
                lblMessage.ForeColor = Color.Crimson;
            }
        }

        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxHeight)
        {
            var ratio = (double)maxHeight / image.Height;
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            lblmsg.Text = "";
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
                Guid uid = Guid.NewGuid();
                string fn = System.IO.Path.GetFileName(File1.PostedFile.FileName);
                string SaveLocation = Server.MapPath("~/Upload/Candidate/TEMP/Photo/") + "" + uid + fn;
                try
                {
                    string fileExtention = File1.PostedFile.ContentType;
                    int fileLenght = File1.PostedFile.ContentLength;
                    if (fileExtention == "image/png" || fileExtention == "image/jpeg" || fileExtention == "image/x-png")
                    {
                        if (fileLenght <= 1048576)
                        {
                            System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(File1.PostedFile.InputStream);
                            System.Drawing.Image objImage = ScaleImage(bmpPostedImage, 300);
                            objImage.Save(SaveLocation, ImageFormat.Jpeg);
                            lblmsg.Text = "The file has been uploaded.";
                            lblmsg.Style.Add("Color", "Green");
                        }
                        else
                        {
                            lblmsg.Text = "Image size cannot be more then 1 MB.";
                            lblmsg.Style.Add("Color", "Red");
                        }
                    }
                    else
                    {
                        lblmsg.Text = "Invalid Format!";
                        lblmsg.Style.Add("Color", "Red");
                    }
                }
                catch (Exception ex)
                {
                    lblmsg.Text = "Error: " + ex.Message;
                    lblmsg.Style.Add("Color", "Red");
                }
            }
        }
    }
}