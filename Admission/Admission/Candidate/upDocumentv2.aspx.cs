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
    public partial class upDocumentv2 : System.Web.UI.Page
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
                    ViewUploadedNID();
                    ViewUploadedPolice();
                    ViewUploadedGrant();
                    ViewHealth();
                    ViewDeposite();
                    ViewTestimonial();
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }

        private void ViewUploadedNID()
        {
            try
            {
                long cId = -1;

                lblnational.NavigateUrl = "";

                lblnational.Visible = false;

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
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 12); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblnational.Visible = true;
                            lblnational.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void ViewUploadedPolice()
        {
            try
            {
                long cId = -1;

                lblpolice.NavigateUrl = "";
                lblpolice.Visible = false;


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
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 13);
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblpolice.Visible = true;
                            lblpolice.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewUploadedGrant()
        {
            try
            {
                long cId = -1;

                lblscholarhip.NavigateUrl = "";

                lblscholarhip.Visible = false;

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
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 14); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblscholarhip.Visible = true;
                            lblscholarhip.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewHealth()
        {
            try
            {
                long cId = -1;

                lblhealth.NavigateUrl = "";

                lblhealth.Visible = false;

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
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 15); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lblhealth.Visible = true;
                            lblhealth.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewDeposite()
        {
            try
            {
                long cId = -1;

                lbldeposite.NavigateUrl = "";

                lbldeposite.Visible = false;

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
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 21); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lbldeposite.Visible = true;
                            lbldeposite.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewTestimonial()
        {
            try
            {
                long cId = -1;

                lbltestimonial.NavigateUrl = "";

                lbltestimonial.Visible = false;

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
                        documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 17); //2 = Image/Photo
                    }

                    DAL.DocumentDetail documentDetailObj = null;
                    using (var dbDocumentDetails = new CandidateDataManager())
                    {
                        documentDetailObj = dbDocumentDetails.GetDocumentDetailByID_ND(documentObj.DocumentDetailsID);

                        if (documentDetailObj != null)
                        {
                            lbltestimonial.Visible = true;
                            lbltestimonial.NavigateUrl = documentDetailObj.URL + "?v=" + DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void bttnSave_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admission/Candidate/foreignStudentApplicationDeclaration.aspx", false);
        }
        

        protected void btnNID_Click(object sender, EventArgs e)
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
            if (FileUploadNID.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadNID.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadNID.PostedFile.ContentLength.ToString());
                string fileName = "C-NId-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-NId-" + cId + fileExtension; // C for candidate
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

                            FileUploadNID.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 12); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 12); // 8= CV
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
                                    newDocumentObj.DocumentTypeID = 12; //8 = CV
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

                            FileUploadNID.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 12); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 12); //8 = CV
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
                                    newDocumentObj.DocumentTypeID = 12; //8 = CV
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

                        ViewUploadedNID();
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

        protected void btnHealth_Click(object sender, EventArgs e)
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
            if (FileUploadHealth.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadHealth.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadHealth.PostedFile.ContentLength.ToString());
                string fileName = "C-Hc-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Hc-" + cId + fileExtension; // C for candidate
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

                            FileUploadHealth.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 15); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 15); // 8= CV
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
                                    newDocumentObj.DocumentTypeID = 15; //8 = CV
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

                            FileUploadHealth.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 15); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 15); //8 = CV
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
                                    newDocumentObj.DocumentTypeID = 15; //8 = CV
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

                        ViewHealth();
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

        protected void btnPolice_Click(object sender, EventArgs e)
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
            if (FileUploadPolice.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadPolice.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadPolice.PostedFile.ContentLength.ToString());
                string fileName = "C-Pc-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Pc-" + cId + fileExtension; // C for candidate
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

                            FileUploadPolice.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 13); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 13); // 8= CV
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
                                    newDocumentObj.DocumentTypeID = 13; //8 = CV
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

                            FileUploadPolice.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 13); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 13); //8 = CV
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
                                    newDocumentObj.DocumentTypeID = 13; //8 = CV
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

                        ViewUploadedPolice();
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

        protected void btnDeposit_Click(object sender, EventArgs e)
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
            if (FileUploadDeposit.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadDeposit.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadDeposit.PostedFile.ContentLength.ToString());
                string fileName = "C-Sf-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Sf-" + cId + fileExtension; // C for candidate
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

                            FileUploadDeposit.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 21); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 21); // 8= CV
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
                                    newDocumentObj.DocumentTypeID = 21; //8 = CV
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

                            FileUploadDeposit.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 21); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 21); //8 = CV
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
                                    newDocumentObj.DocumentTypeID = 21; //8 = CV
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

                        ViewDeposite();
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

        protected void btnGrant_Click(object sender, EventArgs e)
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
            if (FileUploadGrant.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadGrant.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadGrant.PostedFile.ContentLength.ToString());
                string fileName = "C-Gt-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Gt-" + cId + fileExtension; // C for candidate
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

                            FileUploadGrant.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 14); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 14); // 8= CV
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
                                    newDocumentObj.DocumentTypeID = 14; //8 = CV
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

                            FileUploadGrant.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 14); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 14); //8 = CV
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
                                    newDocumentObj.DocumentTypeID = 14; //8 = CV
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

                        ViewUploadedGrant();
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

        protected void btnTestimonial_Click(object sender, EventArgs e)
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
            if (FileUploadTestimonial.HasFile)
            {
                String fileExtension = Path.GetExtension(FileUploadTestimonial.PostedFile.FileName).ToLower();
                int contentlength = int.Parse(FileUploadTestimonial.PostedFile.ContentLength.ToString());
                string fileName = "C-Tm-" + cId + fileExtension; // C for candidate
                string filePath = "~/Upload/Candidate/Others/";
                string tempFileName = "Temp-C-Tm-" + cId + fileExtension; // C for candidate
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

                            FileUploadTestimonial.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 17); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 17); // 8= CV
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
                                    newDocumentObj.DocumentTypeID = 17; //8 = CV
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

                            FileUploadTestimonial.SaveAs(Server.MapPath(filePath + fileName));

                            DAL.Document documentObj = null;
                            using (var dbDocument = new CandidateDataManager())
                            {
                                documentObj = dbDocument.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 17); //8 = CV
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
                                            documentObjToBeUpdated = dbGetDocumentObj.GetDocumentByCandidateIDDocumentTypeID_ND(cId, 17); //8 = CV
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
                                    newDocumentObj.DocumentTypeID = 17; //8 = CV
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

                        ViewTestimonial();
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