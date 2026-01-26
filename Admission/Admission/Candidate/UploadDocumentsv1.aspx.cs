using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class UploadDocumentsv1 : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        string userName = "";

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //FileUploadFinGuarSignature.Attributes["onchange"] = "UploadFGSign(this)";

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            using (var db = new CandidateDataManager())
            {
                userName = db.GetSysterUserNameByID_ND(uId);
            }
            using (var db = new CandidateDataManager())
            {
                DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                if (obj != null && obj.ID > 0)
                {
                    cId = obj.ID;
                    //paymentId = (long)db.AdmissionDB.CandidatePayments.Where(x => x.CandidateID == obj.ID && x.IsPaid == true).Select(x => x.PaymentId).FirstOrDefault();
                }// end if(obj != null && obj.ID > 0)
            }// end using

            if (!IsPostBack)
            {

                using (var db = new CandidateDataManager())
                {
                    var AdditionObj = db.AdmissionDB.AdditionalInfoes.Where(x => x.CandidateID == cId && x.IsFinalSubmit == true).FirstOrDefault();

                    if (AdditionObj == null)
                    {
                        divMain.Visible = true;
                        divFinalSubmit.Visible = false;

                    }
                    else
                    {
                        divFinalSubmit.Visible = true;
                        divMain.Visible = false;
                    }
                }

                if (uId > 0)
                {
                    ViewUploadedPhoto();
                    ViewUploadedcurriculam();
                    ViewUploadedSign();
                    ViewMedicalcertificate();
                    ViewAcademicCertificate();
                    ViewEnglish();
                    ViewBSC();
                    ViewPassport();
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void ViewUploadedPhoto()
        {
            try
            {
                long cId = -1;

                lblPhotoURL.NavigateUrl = "";

                lblPhotoURL.Visible = false;

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

                if(cId>0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 2); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if(documentDetailObj!=null)
                        {
                            lblPhotoURL.Visible = true;
                            lblPhotoURL.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void ViewUploadedcurriculam()
        {
            try
            {
                long cId = -1;

                lblcurriculamURL.NavigateUrl = "";
                lblcurriculamURL.Visible = false;


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

                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 8); 
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblcurriculamURL.Visible = true;
                            lblcurriculamURL.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewUploadedSign()
        {
            try
            {
                long cId = -1;

                lblupsign.NavigateUrl = "";

                lblupsign.Visible = false;

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

                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 3); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblupsign.Visible = true;
                            lblupsign.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewMedicalcertificate()
        {
            try
            {
                long cId = -1;

                lblmedicalcertificate.NavigateUrl = "";

                lblmedicalcertificate.Visible = false;

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

                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 18); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblmedicalcertificate.Visible = true;
                            lblmedicalcertificate.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewAcademicCertificate()
        {
            try
            {
                long cId = -1;

                lblacademic.NavigateUrl = "";

                lblacademic.Visible = false;

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

                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 10); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblacademic.Visible = true;
                            lblacademic.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewEnglish()
        {
            try
            {
                long cId = -1;

                lblEnglish.NavigateUrl = "";

                lblEnglish.Visible = false;

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

                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 11); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblEnglish.Visible = true;
                            lblEnglish.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewBSC()
        {
            try
            {
                long cId = -1;

                lblbachelorcer.NavigateUrl = "";

                lblbachelorcer.Visible = false;

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

                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 19); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblbachelorcer.Visible = true;
                            lblbachelorcer.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewPassport()
        {
            try
            {
                long cId = -1;

                lblPassport.NavigateUrl = "";

                lblPassport.Visible = false;

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

                if (cId > 0)
                {
                    DAL.Document documentObj = null;
                    using (var dbDocument = new CandidateDataManager())
                    {
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 20); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblPassport.Visible = true;
                            lblPassport.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void bttnsave_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admission/Candidate/upDocumentv2.aspx", false);
        }

        
        protected void bttnsave_Click1(object sender, EventArgs e)
        {

        }



        protected void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            try
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
                                        documentDetailObj.ModifiedBy = cId;
                                        documentDetailObj.DateModified = DateTime.Now;

                                        using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                        {
                                            dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                        }

                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
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
                                        documentDetailObj.ModifiedBy = cId;
                                        documentDetailObj.DateModified = DateTime.Now;

                                        using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                        {
                                            dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                        }


                                        #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                        try
                                        {
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
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion


                                }//end if-else

                            }//end if-else
                            #endregion

                            ViewUploadedPhoto();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                    }
                }// end if (FileUploadBanner.HasFile)
                else
                {
                    //Response.Redirect("~/Admission/Message.aspx?message=" + "Something is not right... Please contact administrator.&type=danger", false);
                    //lblMessage.Text = "Please select a file (Candidate's Photo) to upload";
                    //lblMessage.ForeColor = Color.Crimson;
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnSignature_Click(object sender, EventArgs e)
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
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
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
                                    documentDetailObj.ModifiedBy = cId;
                                    documentDetailObj.DateModified = DateTime.Now;

                                    using (var dbUpdateDocumentDetail = new CandidateDataManager())
                                    {
                                        dbUpdateDocumentDetail.Update<DAL.DocumentDetail>(documentDetailObj);
                                    }


                                    #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                                    try
                                    {
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
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion



                            }//end if-else
                        }//end if-else
                        #endregion
                        ViewUploadedSign();


                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
            }


        }

        protected void btnCVUpload_Click(object sender, EventArgs e)
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
            if (FileUploadCV.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadCV.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadCV.PostedFile.ContentLength.ToString());
                string fileName = "C-Cv-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Cv-" + cId + fileExtension; // C for candidate
                string tempFilePath = "~/Upload/Candidate/TEMP/Others/";

                if (contentlength < 2097152)
                {
                    try
                    {

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            FileUploadCV.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 8); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 8); // 8= CV
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
                                    newDocumentObj.DocumentTypeID = 8; //8 = CV
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

                            FileUploadCV.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 8); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 8); //8 = CV
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
                                    newDocumentObj.DocumentTypeID = 8; //8 = CV
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
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion



                            }//end if-else
                        }//end if-else
                        #endregion

                        ViewUploadedcurriculam();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
            }
        }

        protected void btnHsc_Click(object sender, EventArgs e)
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
            if (FileUploadMedical.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadMedical.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadMedical.PostedFile.ContentLength.ToString());
                string fileName = "C-Hs-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Hs-" + cId + fileExtension; // C for candidate
                string tempFilePath = "~/Upload/Candidate/TEMP/Others/";

                if (contentlength < 2097152)
                {
                    try
                    {

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            FileUploadMedical.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 18); //9 = HSC
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 18); // 9= HSC
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
                                    newDocumentObj.DocumentTypeID = 18; //9 = HSC
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

                            FileUploadMedical.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 18); //9 = HSC
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 18); //9 = HSC
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
                                    newDocumentObj.DocumentTypeID = 18; //9 = HSC
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
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion



                            }//end if-else
                        }//end if-else
                        #endregion
                        ViewMedicalcertificate();


                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
            }
        }

        protected void btnAcademic_Click(object sender, EventArgs e)
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
            if (FileUploadAcademic.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadAcademic.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadAcademic.PostedFile.ContentLength.ToString());
                string fileName = "C-Ad-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Ad-" + cId + fileExtension; // C for candidate
                string tempFilePath = "~/Upload/Candidate/TEMP/Others/";

                if (contentlength < 2097152)
                {
                    try
                    {

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            FileUploadAcademic.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 10); 
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 10); 
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
                                    newDocumentObj.DocumentTypeID = 10; 
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

                            FileUploadAcademic.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 10); 
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 10); 
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
                                    newDocumentObj.DocumentTypeID = 10; //10 = Academic
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
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion



                            }//end if-else
                        }//end if-else
                        #endregion
                        ViewAcademicCertificate();


                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
            }
        }

        protected void btnEnglishCertificate_Click(object sender, EventArgs e)
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
            if (FileUploadEnglishCertificate.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadEnglishCertificate.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadEnglishCertificate.PostedFile.ContentLength.ToString());
                string fileName = "C-En-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-En-" + cId + fileExtension; // C for candidate
                string tempFilePath = "~/Upload/Candidate/TEMP/Others/";

                if (contentlength < 2097152)
                {
                    try
                    {

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            FileUploadEnglishCertificate.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 11);
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 11);
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
                                    newDocumentObj.DocumentTypeID = 11;
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

                            FileUploadEnglishCertificate.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 11);
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 11);
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
                                    newDocumentObj.DocumentTypeID = 11; //11 = English Certificate
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
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion



                            }//end if-else
                        }//end if-else
                        #endregion
                        ViewEnglish();


                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
            }
        }

        protected void btnbachelor_Click(object sender, EventArgs e)
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
            if (FileUploadBechlor.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadBechlor.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadBechlor.PostedFile.ContentLength.ToString());
                string fileName = "C-Bs-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Bs-" + cId + fileExtension; // C for candidate
                string tempFilePath = "~/Upload/Candidate/TEMP/Others/";

                if (contentlength < 2097152)
                {
                    try
                    {

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            FileUploadBechlor.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 19); //9 = HSC
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 19); // 9= HSC
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
                                    newDocumentObj.DocumentTypeID = 19; //9 = HSC
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

                            FileUploadBechlor.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 19); //9 = HSC
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 19); //9 = HSC
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
                                    newDocumentObj.DocumentTypeID = 19; //9 = HSC
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
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion



                            }//end if-else
                        }//end if-else
                        #endregion
                        ViewBSC();


                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
            }
        }

        protected void btnpassport_Click(object sender, EventArgs e)
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
            if (FileUploadPassport.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadPassport.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadPassport.PostedFile.ContentLength.ToString());
                string fileName = "C-Ps-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Ps-" + cId + fileExtension; // C for candidate
                string tempFilePath = "~/Upload/Candidate/TEMP/Others/";

                if (contentlength < 2097152)
                {
                    try
                    {

                        #region IF FILE EXISTS
                        //check if file exists
                        if (File.Exists(Server.MapPath(filePath + fileName)))
                        {
                            //move the file to TEMP
                            File.Move(Server.MapPath(filePath + fileName), Server.MapPath(tempFilePath + tempFileName));
                            //delete the original file
                            File.Delete(Server.MapPath(filePath + fileName));

                            FileUploadPassport.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 20); 
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 20); 
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
                                    newDocumentObj.DocumentTypeID = 20; 
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

                            FileUploadPassport.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 20); 
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 20); 
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
                                    newDocumentObj.DocumentTypeID = 20; 
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
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion



                            }//end if-else
                        }//end if-else
                        #endregion
                        ViewPassport();


                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                }
            }// end if (FileUploadBanner.HasFile)
            else
            {
            }
        }
    }
}