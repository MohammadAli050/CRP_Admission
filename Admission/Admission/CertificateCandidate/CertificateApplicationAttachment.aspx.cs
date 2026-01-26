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

namespace Admission.Admission.CertificateCandidate
{
    public partial class CertificateApplicationAttachment : PageBase
    {

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);

                hrefAppAdditional.NavigateUrl = "CertificateApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "CertificateApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "CertificateApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "CertificateApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "CertificateApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                //hrefAppPriority.NavigateUrl = "CertificateApplicationPriority.aspx?val=" + cId;
                hrefAppRelation.NavigateUrl = "CertificateApplicationRelation.aspx?val=" + queryVal;
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
                    List<DAL.CertificateDocument> documentList = db.AdmissionDB.CertificateDocuments.Where(x=> x.CandidateID == cId).ToList();//db.GetAllDocumentByCandidateID_AD(cId);
                    if (documentList != null)
                    {
                        DAL.CertificateDocument photoDocDetailObjTemp = documentList.Where(c => c.DocumentTypeID == 2).FirstOrDefault();
                        DAL.CertificateDocument signDocDetailObjTemp = documentList.Where(c => c.DocumentTypeID == 3).FirstOrDefault();

                        DAL.CertificateDocumentDetail photoDocDetailObj = new DAL.CertificateDocumentDetail();
                        DAL.CertificateDocumentDetail signDocDetailObj = new DAL.CertificateDocumentDetail();

                        if (photoDocDetailObjTemp != null)
                        {
                            photoDocDetailObj = db.AdmissionDB.CertificateDocumentDetails.Where(x => x.ID == photoDocDetailObjTemp.DocumentDetailsID).FirstOrDefault();
                        }

                        if (signDocDetailObjTemp != null)
                        {
                            signDocDetailObj = db.AdmissionDB.CertificateDocumentDetails.Where(x => x.ID == signDocDetailObjTemp.DocumentDetailsID).FirstOrDefault();
                        }



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
            long cId = -1;
            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);
            }
            if (FileUploadPhoto.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadPhoto.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadPhoto.PostedFile.ContentLength.ToString());
                string fileName = "CC-Ph-" + cId + fileExtension; // CC for certificate candidate, Ph for Photo
                string filePath = "~/Upload/CertificateCandidate/Photo/";
                string tempFileName = "Temp-CC-Ph-" + cId + fileExtension; // CC for certificate candidate, Ph for Photo
                string tempFilePath = "~/Upload/CertificateCandidate/TEMP/Photo/";

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

                            FileUploadPhoto.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.CertificateDocument documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.AdmissionDB.CertificateDocuments
                                            .Where(c => c.CandidateID == cId && c.DocumentTypeID == 2)
                                            .FirstOrDefault(); //dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.CertificateDocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.AdmissionDB.CertificateDocumentDetails.Find(documentObj.DocumentDetailsID); //dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.CertificateDocumentDetail>(documentDetailObj);
                                    }
                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                        dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.CertificateDocument documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.AdmissionDB.CertificateDocuments
                                                                    .Where(c => c.CandidateID == cId && c.DocumentTypeID == 2)
                                                                    .FirstOrDefault(); //dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = image/photo
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.CertificateDocument>(documentObjToBeUpdated);
                                            }
                                        }
                                    }
                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                    dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.CertificateDocument newDocumentObj = new DAL.CertificateDocument();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 2; //2 = Image;
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.CertificateDocument>(newDocumentObj);
                                    }
                                }
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

                            DAL.CertificateDocument documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.AdmissionDB.CertificateDocuments
                                            .Where(c => c.CandidateID == cId && c.DocumentTypeID == 2)
                                            .FirstOrDefault();//dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.CertificateDocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.AdmissionDB.CertificateDocumentDetails.Find(documentObj.DocumentDetailsID); //dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.CertificateDocumentDetail>(documentDetailObj);
                                    }
                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                        dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.CertificateDocument documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.AdmissionDB.CertificateDocuments
                                                                    .Where(c => c.CandidateID == cId && c.DocumentTypeID == 2)
                                                                    .FirstOrDefault();//dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = image/photo
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.CertificateDocument>(documentObjToBeUpdated);
                                            }
                                        }
                                    }
                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                    dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.CertificateDocument newDocumentObj = new DAL.CertificateDocument();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 2; //2 = Image;
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.CertificateDocument>(newDocumentObj);
                                    }
                                }
                            }//end if-else

                        }//end if-else
                        #endregion

                        LoadCandidateData(cId);

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
            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                string queryVal = Request.QueryString["val"].ToString();
                string decryptedQueryVal = Decrypt.DecryptString(queryVal);
                cId = Int64.Parse(decryptedQueryVal);
            }

            if (FileUploadSignature.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadSignature.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadSignature.PostedFile.ContentLength.ToString());
                string fileName = "CC-Sn-" + cId + fileExtension; // CC for certificate candidate, Sn for Signature
                string filePath = "~/Upload/CertificateCandidate/Signature/";
                string tempFileName = "Temp-CC-Sn-" + cId + fileExtension; // CC for certificate candidate, Sn for Signature
                string tempFilePath = "~/Upload/CertificateCandidate/TEMP/Signature/";

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

                            DAL.CertificateDocument documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.AdmissionDB.CertificateDocuments
                                            .Where(c => c.CandidateID == cId && c.DocumentTypeID == 3)
                                            .FirstOrDefault();//dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.CertificateDocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.AdmissionDB.CertificateDocumentDetails.Find(documentObj.DocumentDetailsID);//dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.CertificateDocumentDetail>(documentDetailObj);
                                    }
                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                        dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.CertificateDocument documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.AdmissionDB.CertificateDocuments
                                                                    .Where(c => c.CandidateID == cId && c.DocumentTypeID == 3)
                                                                    .FirstOrDefault();//dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.CertificateDocument>(documentObjToBeUpdated);
                                            }
                                        }
                                    }
                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                    dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.CertificateDocument newDocumentObj = new DAL.CertificateDocument();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 3; //3 = signature
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.CertificateDocument>(newDocumentObj);
                                    }
                                }
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

                            DAL.CertificateDocument documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.AdmissionDB.CertificateDocuments
                                                .Where(c => c.CandidateID == cId && c.DocumentTypeID == 3)
                                                .FirstOrDefault();//dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                            }
                            if (documentObj != null) //do not update document, document exists, only update document details
                            {
                                DAL.CertificateDocumentDetail documentDetailObj = null;
                                using (var dbDocumentDetails = new CandidateDataManager())
                                {
                                    documentDetailObj = dbDocumentDetails.AdmissionDB.CertificateDocumentDetails.Find(documentObj.DocumentDetailsID);//dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);
                                }
                                if (documentDetailObj != null) //document detail exists, update document details
                                {
                                    documentDetailObj.URL = filePath + fileName;
                                    documentDetailObj.Name = fileName;
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.CertificateDocumentDetail>(documentDetailObj);
                                    }
                                }
                                else //document detail does not exist, insert new document detail, then update document
                                {
                                    DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                        dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                        newDocumentDetailID = newDocumentDetailObj.ID;
                                    }
                                    if (newDocumentDetailID > 0)
                                    {
                                        DAL.CertificateDocument documentObjToBeUpdated = null;
                                        using (var dbGetDocumentObj = new CandidateDataManager())
                                        {
                                            documentObjToBeUpdated = dbGetDocumentObj.AdmissionDB.CertificateDocuments
                                                                    .Where(c => c.CandidateID == cId && c.DocumentTypeID == 3)
                                                                    .FirstOrDefault();//dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //3 = Signature
                                        }
                                        if (documentObjToBeUpdated != null)
                                        {
                                            documentObjToBeUpdated.DocumentDetailsID = newDocumentDetailID;
                                            using (var dbUpdateDocumentObj = new CandidateDataManager())
                                            {
                                                dbUpdateDocumentObj.Update<DAL.CertificateDocument>(documentObjToBeUpdated);
                                            }
                                        }
                                    }
                                }//end if-else
                            }//end if
                            else //insert document  (first document details, then document)
                            {
                                DAL.CertificateDocumentDetail newDocumentDetailObj = new DAL.CertificateDocumentDetail();
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
                                    dbInsertDocumentDetail.Insert<DAL.CertificateDocumentDetail>(newDocumentDetailObj);
                                    newDocumentDetailID = newDocumentDetailObj.ID;
                                }

                                if (newDocumentDetailID > 0)
                                {
                                    DAL.CertificateDocument newDocumentObj = new DAL.CertificateDocument();
                                    newDocumentObj.CandidateID = cId;
                                    newDocumentObj.DocumentDetailsID = newDocumentDetailID;
                                    newDocumentObj.DocumentTypeID = 3; //3 = Signature
                                    newDocumentObj.Attribute1 = null;
                                    newDocumentObj.Attribute2 = null;
                                    newDocumentObj.DateCreated = DateTime.Now;
                                    newDocumentObj.CreatedBy = cId;

                                    using (var dbInsertDocument = new CandidateDataManager())
                                    {
                                        dbInsertDocument.Insert<DAL.CertificateDocument>(newDocumentObj);
                                    }
                                }
                            }//end if-else





                        }//end if-else
                        #endregion

                        LoadCandidateData(cId);

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


    }
}