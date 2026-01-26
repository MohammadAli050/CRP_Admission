using Admission.App_Start;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class ImageTransferManualAdmissionToFolder : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnImg_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                List<DAL.SPImageTransferAfterServerChange_Result> list = null;
                using (var db = new CandidateDataManager())
                {
                    list = db.AdmissionDB.SPImageTransferAfterServerChange(null, null, 2).ToList();
                }
                if (list != null && list.Count > 0)  
                {
                    string admissiPhotoPath = @"C:\Publish\BUP_Admission\Upload\Candidate\Photo";
                    string uCAMPhotoPath = @"C:\Publish\BUP_Admission\Upload\Candidate\TPhoto\";
                    DirectoryInfo dir = new DirectoryInfo(admissiPhotoPath); //MapPath("~/Images/StudentImage/arch")
                    FileInfo[] file = dir.GetFiles();
                    foreach (var tData in list)
                    {
                        foreach (FileInfo file2 in file)
                        {
                            if (file2.Name.ToLower() == tData.AdmissionImageName.ToLower())
                            {
                                if (file2.Extension.ToLower() == ".jpg" || file2.Extension.ToLower() == ".jpeg" || file2.Extension.ToLower() == ".gif" || file2.Extension.ToLower() == ".png")
                                {
                                    //string NewName = (tData.PersonID.ToString() + ".jpg");
                                    string destinationPath = uCAMPhotoPath + tData.Roll + file2.Extension.ToLower(); //@"E:\Publish\UCAM\Upload\Avatar\Teacher\";
                                                                                                                     //string destinationPath = @"E:\Deploy\UCAM\BUFT\Upload\Avatar\Teacher\";


                                    //string destinationPath = @"D:\WORKSTATION\UAP_UCAM\UAP_UCAM\UAP_UCAM\UIO\EMS\Upload\Avatar\";
                                    string finalPath = destinationPath;
                                    string fullfileName = tData.Roll + file2.Extension.ToLower();

                                    if (!File.Exists(finalPath))
                                    {
                                        file2.CopyTo(finalPath);
                                        count++;
                                    }
                                    using (var db2 = new CandidateDataManager())
                                    {
                                        db2.AdmissionDB.SPUpdatePersonAndStudentDocTableWithPhotoPath(tData.PersonID, 12, fullfileName, 1);
                                    }

                                }
                            }


                        }
                    }

                    if (count > 0)
                    {
                        lblMsg.Text = "Transferred: " + count;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnSign_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                List<DAL.SPImageTransferAfterServerChange_Result> list = null;
                using (var db = new CandidateDataManager())
                {
                    list = db.AdmissionDB.SPImageTransferAfterServerChange(null, null, 3).ToList();
                }
                if (list != null && list.Count > 0)
                {
                    string admissiPhotoPath = @"C:\Publish\BUP_Admission\Upload\Candidate\Signature";
                    string uCAMPhotoPath = @"C:\Publish\BUP_Admission\Upload\Candidate\TSignature\";
                    DirectoryInfo dir = new DirectoryInfo(admissiPhotoPath); //MapPath("~/Images/StudentImage/arch")
                    FileInfo[] file = dir.GetFiles();
                    foreach (var tData in list)
                    {
                        foreach (FileInfo file2 in file)
                        {
                            if (file2.Name.ToLower() == tData.AdmissionImageName.ToLower())
                            {
                                if (file2.Extension.ToLower() == ".jpg" || file2.Extension.ToLower() == ".jpeg" || file2.Extension.ToLower() == ".gif" || file2.Extension.ToLower() == ".png")
                                {
                                    //string NewName = (tData.PersonID.ToString() + ".jpg");
                                    string destinationPath = uCAMPhotoPath + tData.Roll + file2.Extension.ToLower(); //@"E:\Publish\UCAM\Upload\Avatar\Teacher\";
                                                                                                                     //string destinationPath = @"E:\Deploy\UCAM\BUFT\Upload\Avatar\Teacher\";


                                    //string destinationPath = @"D:\WORKSTATION\UAP_UCAM\UAP_UCAM\UAP_UCAM\UIO\EMS\Upload\Avatar\";
                                    string finalPath = destinationPath;
                                    string fullfileName = tData.Roll + file2.Extension.ToLower();

                                    if (!File.Exists(finalPath))
                                    {
                                        file2.CopyTo(finalPath);
                                        count++;
                                    }
                                    using (var db2 = new CandidateDataManager())
                                    {
                                        db2.AdmissionDB.SPUpdatePersonAndStudentDocTableWithPhotoPath(tData.PersonID, 14, fullfileName, 0);
                                    }

                                }
                            }


                        }
                    }

                    if (count > 0)
                    {
                        lblMsg.Text = "Transferred: " + count;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }
    }
}