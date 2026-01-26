using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Script.Serialization;
using System.IO;


namespace Admission.Admission.Candidate
{
    public partial class previewForm : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;
        long paymentId = -1;

        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //candidate user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            this.Page.Form.Enctype = "multipart/form-data";

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
                if (uId > 0)
                {
                    LoadDDL();
                    LoadCandidateData(uId);

                    LoadAcademicDDL();
                    LoadData();


                    ViewUploadedPhoto();
                    ViewUploadedcurriculam();
                    ViewUploadedSign();
                    ViewMedicalcertificate();
                    ViewAcademicCertificate();
                    ViewEnglish();
                    ViewBSC();
                    ViewPassport();

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

                if (cId > 0)
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

                        if (documentDetailObj != null)
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




        private void LoadCandidateData(long uId)
        {
            try
            {
                if (uId > 0)
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.BasicInfo obj = db.GetCandidateBasicInfoByUserID_ND(uId);
                        if (obj != null && obj.ID > 0)
                        {
                            cId = obj.ID;
                        }// end if(obj != null && obj.ID > 0)
                    }// end using
                    if (cId > 0)
                    {
                        using (var db = new CandidateDataManager())
                        {
                            //basic info-------------------------------------------------------
                            DAL.BasicInfo candidate = db.GetCandidateBasicInfoByID_ND(cId);
                            if (candidate != null && candidate.ID > 0)
                            {
                                txtName.Text = candidate.FirstName;
                                txtMiddleName.Text = candidate.MiddleName == null ? "" : candidate.MiddleName;
                                txtLastName.Text = candidate.LastName == null ? "" : candidate.LastName;

                                txtDateOfBirth.Text = candidate.DateOfBirth.ToString("dd/MM/yyyy");
                                ddlGender.SelectedValue = candidate.GenderID.ToString();
                                txtEmail.Text = candidate.Email.ToLower();


                                txtIntMobile.Text = candidate.Mobile; // not needed now
                                txtPhone.Text = candidate.SMSPhone;

                                ddlReligion.SelectedValue = candidate.ReligionID.ToString();
                                ddlCountry.SelectedValue = candidate.NationalityID.ToString();

                                DAL.AdditionalInfo additionalinfo = new DAL.AdditionalInfo();

                                additionalinfo = db.GetAdditionalInfoByCandidateID_ND((long)cId);

                                if (additionalinfo != null)
                                {
                                    txtPassportNo.Text = additionalinfo.PassportNumber == null ? "" : additionalinfo.PassportNumber.ToString();
                                }
                                if (candidate.AttributeStr1 != null && candidate.AttributeStr1 != "")
                                {
                                    RadioButtonList1.SelectedValue = "1";
                                    divSc.Visible = true;
                                    lbldonorname.Text = candidate.AttributeStr1;

                                }
                                if (candidate.AttributeStr2 != null)
                                {
                                    divSc.Visible = false;
                                    RadioButtonList1.SelectedValue = "0";
                                    lblreference.Text = candidate.AttributeStr2;
                                }

                                #region Relation Info


                                List<DAL.Relation> RelationInfoList = new List<DAL.Relation>();

                                DAL.RelationDetail Father = null;
                                DAL.RelationDetail Mother = null;

                                RelationInfoList = db.GetAllRelationByCandidateID_AD((long)cId);

                                if (RelationInfoList != null && RelationInfoList.Any())
                                {
                                    Father = RelationInfoList.Where(x => x.RelationTypeID == 2).Select(x => x.RelationDetail).FirstOrDefault();
                                    Mother = RelationInfoList.Where(x => x.RelationTypeID == 3).Select(x => x.RelationDetail).FirstOrDefault();

                                    if (Father != null)
                                        txtFatherName.Text = Father.Name;
                                    if (Mother != null)
                                        txtMotherName.Text = Mother.Name;

                                }

                                #endregion

                                #region Parents Info


                                List<DAL.Address> addressList = new List<DAL.Address>();

                                DAL.AddressDetail PresentAddress = null;
                                DAL.AddressDetail ParmanentAddress = null;

                                addressList = db.GetAllAddressByCandidateID_ND((long)cId);

                                if (addressList != null && addressList.Any())
                                {
                                    PresentAddress = addressList.Where(x => x.AddressTypeID == 2).Select(x => x.AddressDetail).FirstOrDefault();
                                    ParmanentAddress = addressList.Where(x => x.AddressTypeID == 3).Select(x => x.AddressDetail).FirstOrDefault();

                                    if (PresentAddress != null)
                                        txtPresentAddress.Text = PresentAddress.AddressLine;
                                    if (ParmanentAddress != null)
                                        txtParmamentAddress.Text = ParmanentAddress.AddressLine;

                                }

                                #endregion

                            }

                            bool hasExamTypeSSCHSC = db.IsCandidateHasExamTypeSSCHSC(cId);
                            if (hasExamTypeSSCHSC == true)
                            {
                                ddlGender.Enabled = false;
                            }
                            else
                            {
                                ddlGender.Enabled = true;
                            }


                        }// end using

                    }
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadDDL()
        {
            using (var db = new GeneralDataManager())
            {
                DDLHelper.Bind<DAL.Religion>(ddlReligion, db.AdmissionDB.Religions.Where(a => a.IsActive == true).OrderBy(a => a.ID).ToList(), "ReligionName", "ID", EnumCollection.ListItemType.Religion);
                DDLHelper.Bind<DAL.Gender>(ddlGender, db.AdmissionDB.Genders.Where(a => a.IsActive == true).OrderBy(a => a.GenderName).ToList(), "GenderName", "ID", EnumCollection.ListItemType.Gender);
                DDLHelper.Bind<DAL.Country>(ddlCountry, db.AdmissionDB.Countries.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList(), "Name", "ID", EnumCollection.ListItemType.Country);


            }
        }



        private void LoadAcademicDDL()
        {
            //try
            //{
            //    using (var db = new GeneralDataManager())
            //    {
            //        DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.OrderBy(a => a.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);

            //        List<DAL.SPProgramsGetAllFromUCAM_Result> programs = db.AdmissionDB.SPProgramsGetAllFromUCAM().ToList();
            //        DDLHelper.Bind<DAL.SPProgramsGetAllFromUCAM_Result>(ddlProgram, programs.OrderBy(a => a.DetailName).ToList(), "DetailNShortName", "ProgramID", EnumCollection.ListItemType.Program);

            //    }


            //}
            //catch (Exception ex)
            //{
            //}
        }


        private void LoadData()
        {
            try
            {
                using (var db = new CandidateDataManager())
                {

                    DAL.ProgramPriority pp = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId
                                                                                             && x.Priority == 1).FirstOrDefault();

                    List<DAL.ProgramPriority> pplist = db.AdmissionDB.ProgramPriorities.Where(x => x.CandidateID == cId).ToList();

                    List<TempObj> programList = new List<TempObj>();

                    if (pplist != null && pplist.Any())
                    {
                        List<DAL.SPAcademicCalendarGetAll_Result> sessions = db.AdmissionDB.SPAcademicCalendarGetAll().ToList();

                        int i = 0;
                        foreach (var item in pplist)
                        {
                            i = i + 1;
                            TempObj NewObj = new TempObj();

                            NewObj.SL = i;
                            NewObj.ProgramPriorityId = item.ID;
                            NewObj.ProgramName = item.ProgramName;
                            NewObj.Priority = item.Priority.ToString();
                            if (sessions != null && sessions.Any())
                            {
                                var Session = sessions.Where(x => x.AcademicCalenderID == item.AcaCalID).FirstOrDefault();
                                if (Session != null)
                                    NewObj.Session = Session.FullCode;
                            }

                            var PaymentObj = db.AdmissionDB.ForeignCandidatePaymentInformations.Where(x => x.ProgramPriorityId == item.ID).FirstOrDefault();

                            if (PaymentObj != null && PaymentObj.IsPaid == true)
                            {
                                NewObj.PaymentStatus = "Paid";
                                NewObj.FilePath = PaymentObj.FileName;
                                NewObj.BankName = PaymentObj.BankName;
                                NewObj.TransactionId = PaymentObj.TransactionId;
                                NewObj.PaymentDate = PaymentObj.PaymentDate == null ? "" : Convert.ToDateTime(PaymentObj.PaymentDate).ToString("dd/MM/yyyy");
                            }

                            programList.Add(NewObj);

                        }

                        GridViewProgramList.DataSource = programList.OrderBy(x => x.Priority);
                        GridViewProgramList.DataBind();
                    }
                    else
                    {
                        GridViewProgramList.DataSource = null;
                        GridViewProgramList.DataBind();
                    }

                    BindEducationGridView();


                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSessionDDL(int programID)
        {
            //if (programID > 0)
            //{
            //    using (var db = new GeneralDataManager())
            //    {
            //        var sessions = db.AdmissionDB.SPAcademicCalendarGetAllByProgram(programID).ToList();
            //        if (sessions != null && sessions.Any())
            //        {

            //            var ActiveAdmission = sessions.Where(x => x.IsActiveAdmission == true).OrderBy(x => x.AcademicCalenderID).FirstOrDefault();
            //            if (ActiveAdmission != null)
            //            {
            //                sessions = sessions.Where(x => x.AcademicCalenderID >= ActiveAdmission.AcademicCalenderID).ToList();
            //            }
            //            sessions = sessions.OrderByDescending(z => z.AcademicCalenderID).ToList();
            //            DDLHelper.Bind<DAL.SPAcademicCalendarGetAllByProgram_Result>(ddlSession, sessions, "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Session);
            //        }
            //    }
            //}
            //else
            //{
            //    ddlSession.Items.Clear();
            //    ddlSession.Items.Add(new ListItem("N/A", "-1"));
            //}
        }



        private void BindEducationGridView()
        {
            try
            {
                List<TempInfo> list = new List<TempInfo>();

                using (var db = new CandidateDataManager())
                {
                    List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);
                    int j = 0;
                    foreach (var Examitem in examList)
                    {
                        j = j + 1;
                        TempInfo NewObj = new TempInfo();

                        var ExamTypeObj = db.AdmissionDB.ExamTypes.Where(x => x.ID == Examitem.ExamTypeID).FirstOrDefault();
                        if (ExamTypeObj != null)
                        {
                            NewObj.SL = j;
                            NewObj.ExamId = ExamTypeObj.ID;
                            NewObj.ExamName = ExamTypeObj.ExamTypeName;

                            if (examList != null && examList.Any())
                            {
                                var ExamObj = examList.Where(x => x.ExamTypeID == Examitem.ExamTypeID).FirstOrDefault();

                                if (ExamObj != null)
                                {
                                    if (ExamObj.ExamDetail != null)
                                    {
                                        NewObj.InstituteName = ExamObj.ExamDetail.Institute;
                                        NewObj.PassingYear = ExamObj.ExamDetail.PassingYear.ToString();
                                        NewObj.Division = ExamObj.ExamDetail.ResultDivisionID.ToString();
                                        NewObj.GpaMarks = ExamObj.ExamDetail.GPA == null ? ExamObj.ExamDetail.Marks.ToString() : ExamObj.ExamDetail.GPA.ToString();
                                        NewObj.Grade = ExamObj.ExamDetail.Attribute2;

                                        if (ExamObj.ExamDetail.JsonDataObject != null)
                                            NewObj.ExamName = ExamObj.ExamDetail.JsonDataObject;
                                    }

                                }

                            }

                            list.Add(NewObj);
                        }
                    }

                    gvEducationList.DataSource = list;
                    gvEducationList.DataBind();

                }
            }
            catch (Exception ex)
            {

            }
        }



        protected void lblbtnSave_Click(object sender, EventArgs e)
        {

            #region Personal Information

            try
            {
                DAL.BasicInfo obj = new DAL.BasicInfo();
                using (var db = new CandidateDataManager())
                {
                    obj = db.GetCandidateBasicInfoByUserID_ND(uId);

                    cId = obj.ID;

                }// end using

                if (obj != null)
                {
                    obj.FirstName = txtName.Text.Trim();
                    obj.MiddleName = txtMiddleName.Text;
                    obj.LastName = txtLastName.Text;

                    obj.Email = txtEmail.Text.Trim();
                    obj.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "dd/MM/yyyy", null);
                    obj.ReligionID = ddlReligion.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlReligion.SelectedValue);
                    obj.GenderID = ddlGender.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlGender.SelectedValue);
                    obj.Mobile = txtIntMobile.Text.Trim();
                    obj.SMSPhone = txtPhone.Text.Trim();
                    obj.NationalityID = ddlCountry.SelectedValue == "-1" ? (int?)null : Int32.Parse(ddlCountry.SelectedValue);
                    if (!string.IsNullOrEmpty(lbldonorname.Text))
                        obj.AttributeStr1 = lbldonorname.Text.Trim();
                    if (!string.IsNullOrEmpty(lblreference.Text))
                        obj.AttributeStr2 = lblreference.Text.Trim();

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            db.Update<DAL.BasicInfo>(obj);

                        }// end using

                    }
                    catch (Exception ex)
                    {
                    }


                    try
                    {
                        DAL.AdditionalInfo additionalinfo = new DAL.AdditionalInfo();
                        using (var db = new CandidateDataManager())
                        {
                            additionalinfo = db.GetAdditionalInfoByCandidateID_ND((long)cId);

                            if (additionalinfo != null)
                            {
                                additionalinfo.PassportNumber = txtPassportNo.Text.Trim();
                                additionalinfo.ModifiedBy = uId;
                                additionalinfo.DateModified = DateTime.Now;

                                db.Update<DAL.AdditionalInfo>(additionalinfo);
                            }

                        }// end using
                    }
                    catch (Exception ex)
                    {
                    }


                    #region Father Mother Information 


                    List<DAL.Relation> RelationInfoList = new List<DAL.Relation>();
                    using (var db = new CandidateDataManager())
                    {
                        try
                        {
                            DAL.Relation Father = null;
                            DAL.Relation Mother = null;


                            RelationInfoList = db.GetAllRelationByCandidateID_AD((long)cId);

                            if (RelationInfoList != null && RelationInfoList.Any())
                            {

                                Father = RelationInfoList.Where(x => x.RelationTypeID == 2).FirstOrDefault();
                                Mother = RelationInfoList.Where(x => x.RelationTypeID == 3).FirstOrDefault();

                            }

                            if (Father != null)
                            {
                                var FatherObj = Father.RelationDetail;

                                if (FatherObj != null)
                                {
                                    FatherObj.Name = txtFatherName.Text.Trim();
                                    FatherObj.RelationWithGuardian = "Father";
                                    FatherObj.ModifiedBy = uId;
                                    FatherObj.DateModified = DateTime.Now;

                                    db.Update<DAL.RelationDetail>(FatherObj);
                                }

                            }
                            else
                            {
                                DAL.RelationDetail FatherDetailObj = new DAL.RelationDetail();

                                FatherDetailObj.Name = txtFatherName.Text.Trim();
                                FatherDetailObj.RelationWithGuardian = "Father";
                                FatherDetailObj.CreatedBy = uId;
                                FatherDetailObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.RelationDetail>(FatherDetailObj);

                                long Id = FatherDetailObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Relation NewObj = new DAL.Relation();

                                    NewObj.CandidateID = cId;
                                    NewObj.RelationTypeID = 2;
                                    NewObj.RelationDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Relation>(NewObj);

                                }

                            }

                            if (Mother != null)
                            {
                                var MotherObj = Mother.RelationDetail;

                                if (MotherObj != null)
                                {
                                    MotherObj.Name = txtMotherName.Text.Trim();
                                    MotherObj.RelationWithGuardian = "Mother";
                                    MotherObj.ModifiedBy = uId;
                                    MotherObj.DateModified = DateTime.Now;

                                    db.Update<DAL.RelationDetail>(MotherObj);
                                }
                            }
                            else
                            {
                                DAL.RelationDetail MotherDetailObj = new DAL.RelationDetail();

                                MotherDetailObj.Name = txtMotherName.Text.Trim();
                                MotherDetailObj.RelationWithGuardian = "Mother";
                                MotherDetailObj.CreatedBy = uId;
                                MotherDetailObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.RelationDetail>(MotherDetailObj);

                                long Id = MotherDetailObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Relation NewObj = new DAL.Relation();

                                    NewObj.CandidateID = cId;
                                    NewObj.RelationTypeID = 3;
                                    NewObj.RelationDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Relation>(NewObj);

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }// end using
                    #endregion


                    #region Address Information

                    List<DAL.Address> AddressDetailList = new List<DAL.Address>();
                    using (var db = new CandidateDataManager())
                    {
                        try
                        {
                            DAL.AddressDetail PresentAddress = null;
                            DAL.AddressDetail ParmanentAddress = null;

                            AddressDetailList = db.GetAllAddressByCandidateID_ND((long)cId);

                            if (AddressDetailList != null && AddressDetailList.Any())
                            {
                                PresentAddress = AddressDetailList.Where(x => x.AddressTypeID == 2).Select(x => x.AddressDetail).FirstOrDefault();
                                ParmanentAddress = AddressDetailList.Where(x => x.AddressTypeID == 3).Select(x => x.AddressDetail).FirstOrDefault();
                            }


                            if (PresentAddress != null)
                            {
                                PresentAddress.AddressLine = txtPresentAddress.Text.Trim();
                                PresentAddress.ModifiedBy = uId;
                                PresentAddress.DateModified = DateTime.Now;

                                db.Update<DAL.AddressDetail>(PresentAddress);

                            }
                            else
                            {
                                DAL.AddressDetail PresentObj = new DAL.AddressDetail();

                                PresentObj.AddressLine = txtPresentAddress.Text.Trim();
                                PresentObj.CreatedBy = uId;
                                PresentObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.AddressDetail>(PresentObj);

                                long Id = PresentObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Address NewObj = new DAL.Address();

                                    NewObj.CandidateID = cId;
                                    NewObj.AddressTypeID = 2;
                                    NewObj.AddressDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Address>(NewObj);

                                }
                            }

                            if (ParmanentAddress != null)
                            {
                                ParmanentAddress.AddressLine = txtParmamentAddress.Text.Trim();
                                ParmanentAddress.ModifiedBy = uId;
                                ParmanentAddress.DateModified = DateTime.Now;

                                db.Update<DAL.AddressDetail>(ParmanentAddress);
                            }
                            else
                            {
                                DAL.AddressDetail ParmanentObj = new DAL.AddressDetail();

                                ParmanentObj.AddressLine = txtParmamentAddress.Text.Trim();
                                ParmanentObj.CreatedBy = uId;
                                ParmanentObj.DateCreated = DateTime.Now;

                                db.Insert<DAL.AddressDetail>(ParmanentObj);

                                long Id = ParmanentObj.ID;
                                if (Id > 0)
                                {
                                    DAL.Address NewObj = new DAL.Address();

                                    NewObj.CandidateID = cId;
                                    NewObj.AddressTypeID = 3;
                                    NewObj.AddressDetailsID = Id;
                                    NewObj.CreatedBy = uId;
                                    NewObj.DateCreated = DateTime.Now;
                                    db.Insert<DAL.Address>(NewObj);

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }


                    #endregion


                }
                

            }
            catch (Exception ex)
            {
            }

            #endregion


            #region Academic Information

            try
            {
                using (var db = new CandidateDataManager())
                {
                    #region Save Educational Information



                    List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);



                    foreach (GridViewRow row in gvEducationList.Rows)
                    {

                        DAL.Exam ExamObj = null;


                        HiddenField hdnExamId = (HiddenField)row.FindControl("ExamTypeId");
                        HiddenField hdnYear = (HiddenField)row.FindControl("hdnYear");
                        HiddenField hdnDivisionId = (HiddenField)row.FindControl("hdnDivisionId");

                        TextBox txtins = (TextBox)row.FindControl("txtInst");
                        TextBox GPAMarks = (TextBox)row.FindControl("txtGpa");
                        TextBox Grade = (TextBox)row.FindControl("txtLgD");


                        string Ins = txtins.Text == null ? "" : txtins.Text;


                        DropDownList ddlDivision = (DropDownList)row.FindControl("ddlDvision");
                        DropDownList ddlyear = (DropDownList)row.FindControl("ddlPassingYear");


                        TextBox lblExam = (TextBox)row.FindControl("lblExam");

                        int DivisionId = Int32.Parse(ddlDivision.SelectedValue);
                        int YearId = Int32.Parse(ddlyear.SelectedValue);


                        int ExamId = Convert.ToInt32(hdnExamId.Value);

                        if (examList != null)
                        {
                            ExamObj = examList.Where(x => x.ExamTypeID == ExamId).FirstOrDefault();
                        }

                        if (ExamObj != null) // Update
                        {
                            var ExamDetailsObj = db.AdmissionDB.ExamDetails.AsNoTracking().Where(x => x.ID == ExamObj.ExamDetailsID).FirstOrDefault();
                            if (ExamDetailsObj != null)
                            {
                                ExamDetailsObj.Institute = Ins;
                                ExamDetailsObj.ResultDivisionID = DivisionId;
                                ExamDetailsObj.PassingYear = YearId;

                                try
                                {
                                    if (!string.IsNullOrEmpty(GPAMarks.Text))
                                        ExamDetailsObj.Marks = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);

                                }
                                catch (Exception ex)
                                {
                                }
                                try
                                {
                                    if (!string.IsNullOrEmpty(GPAMarks.Text))
                                    {
                                        ExamDetailsObj.GPA = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Grade.Text))
                                    {
                                        ExamDetailsObj.Attribute2 = Grade.Text == null ? null : Grade.Text;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                try
                                {
                                    if (!string.IsNullOrEmpty(lblExam.Text))
                                    {
                                        ExamDetailsObj.JsonDataObject = lblExam.Text;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                ExamDetailsObj.ModifiedBy = uId;
                                ExamDetailsObj.DateModified = DateTime.Now;

                                using (var dbUpdateExamDetails = new CandidateDataManager())
                                {
                                    dbUpdateExamDetails.Update<DAL.ExamDetail>(ExamDetailsObj);
                                }
                            }
                        }
                        else // Insert
                        {

                            #region Detail Insert

                            DAL.ExamDetail ExmDtlObj = new DAL.ExamDetail();

                            ExmDtlObj.EducationBoardID = 1;
                            ExmDtlObj.Institute = Ins;
                            ExmDtlObj.UndgradGradProgID = 1;
                            ExmDtlObj.ResultDivisionID = DivisionId;
                            ExmDtlObj.PassingYear = YearId;


                            try
                            {
                                if (!string.IsNullOrEmpty(GPAMarks.Text))
                                    ExmDtlObj.Marks = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);
                                else
                                    ExmDtlObj.Marks = null;
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(GPAMarks.Text))
                                    ExmDtlObj.GPA = GPAMarks.Text == null ? 0 : decimal.Parse(GPAMarks.Text);
                                else
                                    ExmDtlObj.GPA = null;
                            }
                            catch (Exception ex)
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Grade.Text))
                                    ExmDtlObj.Attribute2 = Grade.Text == null ? null : Grade.Text;
                                else
                                    ExmDtlObj.Attribute2 = null;
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(lblExam.Text))
                                {
                                    ExmDtlObj.JsonDataObject = lblExam.Text;
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            ExmDtlObj.DateCreated = DateTime.Now;
                            ExmDtlObj.CreatedBy = cId;

                            long Id = 0;

                            using (var dbInsertSecExamDtl = new CandidateDataManager())
                            {
                                dbInsertSecExamDtl.Insert<DAL.ExamDetail>(ExmDtlObj);
                                Id = ExmDtlObj.ID;
                            }

                            #endregion

                            #region Master Insert

                            if (Id > 0)
                            {


                                DAL.Exam newExamObj = new DAL.Exam();
                                newExamObj.ExamDetailsID = Id;
                                newExamObj.CandidateID = cId;
                                newExamObj.ExamTypeID = ExamId;
                                newExamObj.CreatedBy = cId;
                                newExamObj.DateCreated = DateTime.Now;

                                using (var dbInsertSecExam = new CandidateDataManager())
                                {
                                    dbInsertSecExam.Insert<DAL.Exam>(newExamObj);
                                }

                            }

                            #endregion

                        }


                    }



                    #endregion
                }
            }
            catch (Exception ex)
            {
            }

            #endregion

            Response.Redirect("~/Admission/Candidate/foreignStudentApplicationDeclaration.aspx", false);


        }
        

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int progId = -1;
            //progId = Convert.ToInt32(ddlProgram.SelectedValue);
            //if (progId > 0)
            //{
            //    LoadSessionDDL(progId);

            //    BindEducationGridView();
            //}
            //else
            //{
            //    ddlSession.Items.Clear();
            //    ddlSession.Items.Add(new ListItem("N/A", "-1"));
            //}
        }

        protected void gvEducationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var ddl = e.Row.FindControl("ddlPassingYear") as DropDownList;
                    ddl.Items.Clear();
                    ddl.AppendDataBoundItems = true;

                    int startYear = 2000;
                    var YearList = Enumerable.Range(startYear, DateTime.Now.Year - startYear + 1).ToList();
                    ddl.DataSource = YearList.OrderByDescending(x => x.ToString());
                    ddl.DataBind();

                    #region Selected Year And Division
                    using (var db = new CandidateDataManager())
                    {

                        List<DAL.Exam> examList = db.GetAllExamByCandidateID_AD(cId);

                        HiddenField hdnExamId = (HiddenField)e.Row.FindControl("ExamTypeId");

                        DropDownList ddlDivision = (DropDownList)e.Row.FindControl("ddlDvision");
                        DropDownList ddlyear = (DropDownList)e.Row.FindControl("ddlPassingYear");


                        try
                        {
                            int ExamTypeId = Convert.ToInt32(hdnExamId.Value);
                            if (ExamTypeId > 0)
                            {
                                if (examList != null && examList.Any())
                                {

                                    var ExistingObj = examList.Where(x => x.ExamTypeID == ExamTypeId).FirstOrDefault();
                                    if (ExistingObj != null)
                                    {
                                        var ExamdetailExam = ExistingObj.ExamDetail;
                                        if (ExamdetailExam != null)
                                        {

                                            ddlDivision.SelectedValue = ExamdetailExam.ResultDivisionID.ToString();
                                            ddlyear.SelectedValue = ExamdetailExam.PassingYear.ToString();

                                        }
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }



                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
            }
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


        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(RadioButtonList1.SelectedValue);

            if (Id == 0)
            {
                divSc.Visible = false;
            }
            else
            {
                divSc.Visible = true;

            }
        }

        public class TempInfo
        {
            public int SL { get; set; }
            public int ExamId { get; set; }
            public string ExamName { get; set; }
            public string PassingYear { get; set; }
            public string InstituteName { get; set; }
            public string Grade { get; set; }
            public string Division { get; set; }

            public string GpaMarks { get; set; }



        }

        public class TempObj
        {
            public int SL { get; set; }
            public long ProgramPriorityId { get; set; }
            public string ProgramName { get; set; }
            public string Priority { get; set; }
            public string Session { get; set; }
            public string PaymentStatus { get; set; }
            public string FilePath { get; set; }
            public string BankName { get; set; }
            public string TransactionId { get; set; }
            public string PaymentDate { get; set; }
        }
    }
}