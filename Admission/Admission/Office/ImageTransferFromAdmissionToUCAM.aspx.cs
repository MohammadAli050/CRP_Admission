using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using DATAMANAGER;
using System.Drawing;
using CommonUtility;
using Admission.App_Start;
using System.IO;
using System.Web.Configuration;

namespace Admission.Admission.Office
{
    public partial class ImageTransferFromAdmissionToUCAM : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                MessageView("", "clear");

                LoadDDL();
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
            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
            }

        }

        private void LoadDDL()
        {

            ddlDocumentType.Items.Clear();
            ddlDocumentType.Items.Add(new ListItem("--Select--", "-1"));
            ddlDocumentType.Items.Add(new ListItem("Image", "2"));
            ddlDocumentType.Items.Add(new ListItem("Signature", "3"));
            

            using (var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlFaculty, db.GetAllAdmissionUnit(), "UnitName", "ID", EnumCollection.ListItemType.Select);
                DDLHelper.Bind<DAL.SPAcademicCalendarGetAll_Result>(ddlSession, db.AdmissionDB.SPAcademicCalendarGetAll().OrderByDescending(a => a.AcademicCalenderID).ToList(), "FullCode", "AcademicCalenderID", EnumCollection.ListItemType.Select);
                
            }
        }




        protected void btnSingleImageTransfer_Click(object sender, EventArgs e)
        {
            //MessageView("", "clear");

            //string candidatePaymentString = txtPaymentId.Text.Trim();
            //long paymentId = -1;
            //long candidateId = -1;
            //int countImageTransfer = -1;
            //if (candidatePaymentString != "")
            //{
            //    List<DAL.Document> documentPhotoSignatureList = null;
            //    DAL.CandidatePayment cp = null;
            //    paymentId = Convert.ToInt64(candidatePaymentString);
            //    using (var db = new CandidateDataManager())
            //    {
            //        cp = db.AdmissionDB.CandidatePayments.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            //        if (cp != null)
            //        {
            //            candidateId = Convert.ToInt64(cp.CandidateID);
            //            documentPhotoSignatureList = db.GetAllCandidateDocumentByCandID_AD(Convert.ToInt64(cp.CandidateID));
            //        }
            //        else
            //        {
            //            MessageView("Invalid Payment ID !!", "fail");
            //        }

            //    }

            //    DAL_UCAM.Student ucamStudent = null;
            //    using (var db = new UCAMDataManager())
            //    {
            //        ucamStudent = db.UCAMDb.Students.Where(x => x.CandidateID == candidateId).FirstOrDefault();
            //    }

            //    if (ucamStudent != null)
            //    {
            //        if (documentPhotoSignatureList != null && documentPhotoSignatureList.Count > 0)
            //        {
            //            //... Only for Picture
            //            DAL.DocumentDetail dd = documentPhotoSignatureList.Where(x => x.DocumentTypeID == 2).Select(x => x.DocumentDetail).FirstOrDefault();

            //            if (dd != null)
            //            {
            //                countImageTransfer = SingleCopyImageAdmissionToUCAM(candidateId, dd, ucamStudent);


            //                MessageView(countImageTransfer.ToString() + " Image Transfered.", "success");
            //            }
            //            else
            //            {
            //                MessageView("No Data Found !!", "fail");
            //            }
            //        }
            //        else
            //        {
            //            MessageView("No Data Found !!", "fail");
            //        }
            //    }
            //    else
            //    {
            //        MessageView("No Candidate Found in UCAM !!", "fail");
            //    }


            //}
            //else
            //{
            //    MessageView("Please provide Valid Payment ID !!", "fail");
            //}


        }


        //protected int SingleCopyImageAdmissionToUCAM(long candidateId, DAL.DocumentDetail dd, DAL_UCAM.Student ucamStudent)
        //{
        //    int count = 0;

        //    //string admissiPhotoPath = string.Empty;
        //    //string uCAMPhotoPath = string.Empty;

        //    //List<DAL.URLProperty> urlPropertyList = null;
        //    //using (var db = new GeneralDataManager())
        //    //{
        //    //    urlPropertyList = db.AdmissionDB.URLProperties.Where(x => x.IsActive == true).ToList();
        //    //}
        //    //if (urlPropertyList != null && urlPropertyList.Count > 0)
        //    //{
        //    //    admissiPhotoPath = urlPropertyList.Where(x => x.URLType == 2 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoRootPath
        //    //    uCAMPhotoPath = urlPropertyList.Where(x => x.URLType == 3 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoDestinationPath
        //    //}

        //    //string rootFolderPath = admissiPhotoPath; //@"E:\Deploy\BUFT_Admission\Upload\Candidate\Photo";

        //    ////string rootFolderPath = @"D:\WORKSTATION\BUFT_Admission\BUFT_Admission_New\trunk\Admission\Upload\Candidate\Photo";

        //    //DirectoryInfo dir = new DirectoryInfo(rootFolderPath); //MapPath("~/Images/StudentImage/arch")
        //    //FileInfo[] file = dir.GetFiles();

        //    //foreach (FileInfo file2 in file)
        //    //{
        //    //    if (file2.Name.ToLower() == dd.Name.ToLower())
        //    //    {
        //    //        if (file2.Extension.ToLower() == ".jpg" || file2.Extension.ToLower() == ".jpeg" || file2.Extension.ToLower() == ".gif" || file2.Extension.ToLower() == ".png")
        //    //        {

        //    //            string NewName = (ucamStudent.PersonID.ToString() + ".jpg");
        //    //            string destinationPath = uCAMPhotoPath; //@"E:\Deploy\UCAM\BUFT\Upload\Avatar\Teacher\";
        //    //            //string destinationPath = @"D:\WORKSTATION\UAP_UCAM\UAP_UCAM\UAP_UCAM\UIO\EMS\Upload\Avatar\";
        //    //            string finalPath = destinationPath + NewName;

        //    //            if (!File.Exists(finalPath))
        //    //            {
        //    //                file2.CopyTo(finalPath);
        //    //                count++;
        //    //            }


        //    //        }
        //    //    }

        //    //    //

        //    //    //string name = arr[0].ToString().Substring(5);
        //    //    //int i = Convert.ToInt32(name);


        //    //}
        //    ////}

        //    return count;
        //}


        protected void btnAllImageTransfer_Click(object sender, EventArgs e)
        {
            //MessageView("", "clear");
            //int count = -1;
            //List<DAL.SPGetAdmissionAndUCAMImageInformation_Result> list = null;
            //using (var db = new CandidateDataManager())
            //{
            //    list = db.AdmissionDB.SPGetAdmissionAndUCAMImageInformation().ToList();
            //}

            //if (list != null && list.Count > 0)
            //{
            //    //string admissiPhotoPath = WebConfigurationManager.AppSettings["AdmissiPhotoPath"];
            //    //string uCAMPhotoPath = WebConfigurationManager.AppSettings["UCAMPhotoPath"];

            //    string admissiPhotoPath = string.Empty;
            //    string uCAMPhotoPath = string.Empty;

            //    List<DAL.URLProperty> urlPropertyList = null;
            //    using (var db = new GeneralDataManager())
            //    {
            //        urlPropertyList = db.AdmissionDB.URLProperties.Where(x => x.IsActive == true).ToList();
            //    }
            //    if (urlPropertyList != null && urlPropertyList.Count > 0)
            //    {
            //        admissiPhotoPath = urlPropertyList.Where(x => x.URLType == 2 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoRootPath
            //        uCAMPhotoPath = urlPropertyList.Where(x => x.URLType == 3 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoDestinationPath
            //    }

            //    string rootFolderPath = admissiPhotoPath; //@"E:\Publish\BUFT_Admission\Upload\Candidate\Photo";
            //    //string rootFolderPath = @"E:\Deploy\BUFT_Admission\Upload\Candidate\Photo";
            //    //string rootFolderPath = @"D:\WORKSTATION\BUFT_Admission\BUFT_Admission_New\trunk\Admission\Upload\Candidate\Photo";

            //    DirectoryInfo dir = new DirectoryInfo(rootFolderPath); //MapPath("~/Images/StudentImage/arch")
            //    FileInfo[] file = dir.GetFiles();

            //    foreach (var tData in list)
            //    {
            //        foreach (FileInfo file2 in file)
            //        {
            //            if (file2.Name.ToLower() == tData.AdmissionImageName.ToLower())
            //            {
            //                if (file2.Extension.ToLower() == ".jpg" || file2.Extension.ToLower() == ".jpeg" || file2.Extension.ToLower() == ".gif" || file2.Extension.ToLower() == ".png")
            //                {
            //                    string NewName = (tData.PersonID.ToString() + ".jpg");
            //                    string destinationPath = uCAMPhotoPath; //@"E:\Publish\UCAM\Upload\Avatar\Teacher\";
            //                                                            //string destinationPath = @"E:\Deploy\UCAM\BUFT\Upload\Avatar\Teacher\";

            //                    //string destinationPath = @"D:\WORKSTATION\UAP_UCAM\UAP_UCAM\UAP_UCAM\UIO\EMS\Upload\Avatar\";
            //                    string finalPath = destinationPath + NewName;

            //                    if (!File.Exists(finalPath))
            //                    {
            //                        file2.CopyTo(finalPath);
            //                        count++;
            //                    }


            //                }
            //            }


            //        }
            //    }

            //    if (count > 0)
            //    {
            //        MessageView(count.ToString() + " Image Transfered", "success");
            //    }
            //}
            //else
            //{
            //    MessageView("No Data found !!", "fail");
            //}

        }



        //**** UIU Admission Image Name Change CandidateId to Roll
        protected void btnAllImageTransfer2_Click(object sender, EventArgs e)
        {
            //MessageView("", "clear");
            //int count = -1;



            //List<DAL.SPGetUIUAdmissionAndUCAMImageInformation_Result> list = null;
            //using (var db = new CandidateDataManager())
            //{
            //    list = db.AdmissionDB.SPGetUIUAdmissionAndUCAMImageInformation().ToList();
            //}

            //if (list != null && list.Count > 0)
            //{
            //    string rootFolderPath = @"E:\Image\Image\Image";
            //    //string rootFolderPath = @"E:\Image\Signature\Signature";

            //    //** Get All the Image form the Folder
            //    DirectoryInfo dir = new DirectoryInfo(rootFolderPath); //MapPath("~/Images/StudentImage/arch")
            //    FileInfo[] file = dir.GetFiles();

            //    foreach (var tData in list)
            //    {
            //        foreach (FileInfo file2 in file)
            //        {
            //            //** check the image in folder is match with Database image
            //            if (file2.Name.ToLower() == tData.ImageNameAdmission.ToLower())
            //            {
            //                if (file2.Extension.ToLower() == ".jpg" || file2.Extension.ToLower() == ".jpeg" || file2.Extension.ToLower() == ".gif" || file2.Extension.ToLower() == ".png" || file2.Extension.ToLower() == ".bmp")
            //                {
            //                    string NewName = (tData.Roll.ToString() + ".jpg");
            //                    string destinationPath = @"E:\Image\Image\NewImage\";
            //                    //string destinationPath = @"E:\Image\Signature\NewSignature\";

            //                    string finalPath = destinationPath + NewName;

            //                    if (!File.Exists(finalPath))
            //                    {
            //                        file2.CopyTo(finalPath);
            //                        count++;
            //                    }


            //                }
            //            }


            //        }
            //    }

            //    if (count > 0)
            //    {
            //        MessageView(count.ToString() + " Image Transfered", "success");
            //    }
            //}
            //else
            //{
            //    MessageView("No Data found !!", "fail");
            //}

        }

        protected void btnTransferDocument_Click(object sender, EventArgs e)
        {
            try
            {
                MessageView("", "clear");
                int count = -1;

                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                long facultyId = Convert.ToInt64(ddlFaculty.SelectedValue);
                int docTypeId = Convert.ToInt32(ddlDocumentType.SelectedValue);

                List<DAL.SPGetApprovedListDocument_Result> list = null;
                using (var db = new CandidateDataManager())
                {
                    list = db.AdmissionDB.SPGetApprovedListDocument(acaCalId, facultyId, docTypeId).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    string admissiPhotoPath = WebConfigurationManager.AppSettings["AdmissiPhotoPath"];
                    string uCAMPhotoPath = WebConfigurationManager.AppSettings["UCAMPhotoPath"];

                    //string admissiPhotoPath = string.Empty;
                    //string uCAMPhotoPath = string.Empty;

                    //List<DAL.URLProperty> urlPropertyList = null;
                    //using (var db = new GeneralDataManager())
                    //{
                    //    urlPropertyList = db.AdmissionDB.URLProperties.Where(x => x.IsActive == true).ToList();
                    //}
                    //if (urlPropertyList != null && urlPropertyList.Count > 0)
                    //{
                    //    admissiPhotoPath = urlPropertyList.Where(x => x.URLType == 2 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoRootPath
                    //    uCAMPhotoPath = urlPropertyList.Where(x => x.URLType == 3 && x.IsActive == true).Select(x => x.URLName).FirstOrDefault(); //PhotoDestinationPath
                    //}

                    string rootFolderPath = admissiPhotoPath; //@"E:\Publish\BUFT_Admission\Upload\Candidate\Photo";
                    //string rootFolderPath = @"E:\Deploy\BUFT_Admission\Upload\Candidate\Photo";
                    //string rootFolderPath = @"D:\WORKSTATION\BUFT_Admission\BUFT_Admission_New\trunk\Admission\Upload\Candidate\Photo";

                    DirectoryInfo dir = new DirectoryInfo(rootFolderPath); //MapPath("~/Images/StudentImage/arch")
                    FileInfo[] file = dir.GetFiles();

                    foreach (var tData in list)
                    {
                        foreach (FileInfo file2 in file)
                        {
                            if (file2.Name.ToLower() == tData.PhotoName.ToLower())
                            {
                                if (file2.Extension.ToLower() == ".jpg" || file2.Extension.ToLower() == ".jpeg" || file2.Extension.ToLower() == ".gif" || file2.Extension.ToLower() == ".png")
                                {
                                    //string NewName = (tData.PersonID.ToString() + ".jpg");
                                    string destinationPath = uCAMPhotoPath; //@"E:\Publish\UCAM\Upload\Avatar\Teacher\";
                                                                            //string destinationPath = @"E:\Deploy\UCAM\BUFT\Upload\Avatar\Teacher\";

                                    //string destinationPath = @"D:\WORKSTATION\UAP_UCAM\UAP_UCAM\UAP_UCAM\UIO\EMS\Upload\Avatar\";
                                    string finalPath = destinationPath + tData.PhotoName;

                                    if (!File.Exists(finalPath))
                                    {
                                        file2.CopyTo(finalPath);
                                        count++;
                                    }


                                }
                            }


                        }
                    }

                    if (count > 0)
                    {
                        MessageView(count.ToString() + " Image Transfered", "success");
                    }
                }
                else
                {
                    MessageView("No Data found !!", "fail");
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}