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
    public partial class CandApplicationAttachmentOld : PageBase
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
                cId = Int64.Parse(Request.QueryString["val"].ToString());


                hrefAppAdditional.NavigateUrl = "CandApplicationAdditional.aspx?val=" + cId;
                hrefAppAddress.NavigateUrl = "CandApplicationAddress.aspx?val=" + cId;
                hrefAppAttachment.NavigateUrl = "CandApplicationAttachment.aspx?val=" + cId;
                hrefAppBasic.NavigateUrl = "CandApplicationBasic.aspx?val=" + cId;
                hrefAppEducation.NavigateUrl = "CandApplicationEducation.aspx?val=" + cId;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                hrefAppPriority.NavigateUrl = "CandApplicationPriority.aspx?val=" + cId;
                hrefAppRelation.NavigateUrl = "CandApplicationRelation.aspx?val=" + cId;
                hrefAppDeclaration.NavigateUrl = "CandApplicationDeclaration.aspx?val=" + cId;
            }

            if (!IsPostBack)
            {
                base.OnLoad(e);

                FileUploadPhoto.Attributes["onchange"] = "UploadPhoto(this)";
                FileUploadSignature.Attributes["onchange"] = "UploadSignature(this)";
                //FileUploadFinGuarSignature.Attributes["onchange"] = "UploadFGSign(this)";

                uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
                uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);



                if (!IsPostBack)
                {
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
                            ImagePhoto.ImageUrl = photoDocDetailObj.URL;
                        }

                        if (signDocDetailObj != null)
                        {
                            ImageSignature.ImageUrl = signDocDetailObj.URL;
                        }

                    }
                }
            }//end if (cId > 0)
        }

        protected void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            DAL.BasicInfo basicInfo = null;
            DAL.CandidatePayment candidatePayment = null;

            long cId = -1;
            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                cId = Int64.Parse(Request.QueryString["val"].ToString());

                using (var db = new CandidateDataManager())
                {
                    basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                    candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                }
            }

            if (cId > 0 && uId > 0)
            {

                if (FileUploadPhoto.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUploadPhoto.PostedFile.FileName).ToLower();
                    int contentlength = int.Parse(FileUploadPhoto.PostedFile.ContentLength.ToString());
                    string fileName = "C-Ph-" + cId + fileExtension; // C for candidate, Ph for Photo
                    string filePath = "~/Upload/Candidate/Photo/";
                    string tempFileName = "Temp-C-Ph-" + cId + fileExtension; // C for candidate, Ph for Photo
                    string tempFilePath = "~/Upload/Candidate/TEMP/Photo/";

                    if (contentlength < 204800)
                    {
                        try
                        {
                            #region Image Validation Check 

                            bool ValidImage = false;
                            try
                            {
                                System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(FileUploadPhoto.PostedFile.InputStream);
                                ValidImage = true;
                            }
                            catch (Exception ex)
                            {
                                ValidImage = false;
                            }

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

                                FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));

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
                                        documentDetailObj.ModifiedBy = uId;
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
                                            dLog.EventName = "Attachment Info Update (Photo) (Admin)";
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

                                    }
                                    else //document detail does not exist, insert new document detail, then update document
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
                                            dLog.EventName = "Attachment Info Update (Photo) (Admin)";
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
                                        dLog.EventName = "Attachment Info Update (Photo) (Admin)";
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
                                FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));

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
                                        documentDetailObj.ModifiedBy = uId;
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
                                            dLog.EventName = "Attachment Info Update (Photo) (Admin)";
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

                                    }
                                    else //document detail does not exist, insert new document detail, then update document
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
                                            dLog.EventName = "Attachment Info Update (Photo) (Admin)";
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
                                        dLog.EventName = "Attachment Info Insert (Photo) (Admin)";
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

                                }//end if-else

                            }//end if-else
                            #endregion

                            LoadCandidateData(uId);

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
            else
            {
                lblMessage.Text = "Invalid Request!";
                lblMessage.ForeColor = Color.Crimson;
            }
        }

        protected void btnUploadSignature_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            DAL.BasicInfo basicInfo = null;
            DAL.CandidatePayment candidatePayment = null;

            long cId = -1;
            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                cId = Int64.Parse(Request.QueryString["val"].ToString());

                using (var db = new CandidateDataManager())
                {
                    basicInfo = db.AdmissionDB.BasicInfoes.Where(x => x.ID == cId).FirstOrDefault();
                    candidatePayment = db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == cId).FirstOrDefault();
                }

            }

            if (cId > 0 && uId > 0)
            {
                string logOldObject = string.Empty;
                string logNewObject = string.Empty;

                if (FileUploadSignature.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUploadSignature.PostedFile.FileName).ToLower();
                    int contentlength = int.Parse(FileUploadSignature.PostedFile.ContentLength.ToString());
                    string fileName = "C-Sn-" + cId + fileExtension; // C for candidate, Sn for Signature
                    string filePath = "~/Upload/Candidate/Signature/";
                    string tempFileName = "Temp-C-Sn-" + cId + fileExtension; // C for candidate, Sn for Signature
                    string tempFilePath = "~/Upload/Candidate/TEMP/Signature/";

                    if (contentlength < 204800)
                    {
                        try
                        {
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

                                FileUploadSignature.SaveAs(Server.MapPath(filePath + fileName));

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
                                        documentDetailObj.ModifiedBy = uId;
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
                                            dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                            dLog.PageName = "CandApplicationAttachment.aspx";
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

                                    }
                                    else //document detail does not exist, insert new document detail, then update document
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
                                            dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                            dLog.PageName = "CandApplicationAttachment.aspx";
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
                                        dLog.EventName = "Attachment Info Insert (Signature) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
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

                                FileUploadSignature.SaveAs(Server.MapPath(filePath + fileName));

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
                                        documentDetailObj.ModifiedBy = uId;
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
                                            dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                            dLog.PageName = "CandApplicationAttachment.aspx";
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

                                    }
                                    else //document detail does not exist, insert new document detail, then update document
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
                                            dLog.EventName = "Attachment Info Update (Signature) (Admin)";
                                            dLog.PageName = "CandApplicationAttachment.aspx";
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
                                        dLog.EventName = "Attachment Info Insert (Signature) (Admin)";
                                        dLog.PageName = "CandApplicationAttachment.aspx";
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

                                }//end if-else





                            }//end if-else
                            #endregion

                            LoadCandidateData(uId);

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
            else
            {
                lblMessage.Text = "Invalid Request!";
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

            //Tutorial URL: https://codepedia.info/how-to-resize-image-while-uploading-in-asp-net-using-c

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
                            System.Drawing.Image objImage = ScaleImage(bmpPostedImage, 300); //
                            // Saving image in jpeg format
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
                        lblmsg.Text = "Invaild Format!";
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