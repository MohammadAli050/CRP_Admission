using Admission.App_Start;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Admin
{
    public partial class InstituteImageUpload : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FileUploadBanner.Attributes["onchange"] = "UploadFile(this)";
            if (!IsPostBack)
            {
                if ((!string.IsNullOrEmpty(Request.QueryString["insId"])))
                {
                    int instituteId = Int32.Parse(Request.QueryString["insId"]);
                    if(instituteId > 0)
                    {
                        using(var db = new GeneralDataManager())
                        {
                            DAL.Institute institute = db.AdmissionDB.Institutes.Find(instituteId);
                            if(institute != null)
                            {
                                imageBanner.ImageUrl = institute.BannerUrl;
                            }
                            else
                            {
                                imageBanner.ImageUrl = "~/Images/AppImg/img1.png";
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/Admission/Message.aspx?message=" + "Illegal Page Request.&type=danger", false);
                }
            }
        }

        protected void btnUploadBanner_Click(object sender, EventArgs e)
        {
            int instituteId = Int32.Parse(Request.QueryString["insId"]);
            DAL.Institute institute = null;
            using (var db = new GeneralDataManager())
            {
                institute = db.AdmissionDB.Institutes.Find(instituteId);
            }
            if (institute != null)
            {
                if (FileUploadBanner.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUploadBanner.PostedFile.FileName).ToLower();
                    try
                    {
                        lblMessageBanner.Text = "Please wait.";
                        lblMessageBanner.ForeColor = Color.OrangeRed;

                        //determine if file exist
                        if (File.Exists(Server.MapPath("~/ApplicationDocs/Institute/" + institute.ShortName + "_banner" + fileExtension)))
                        {
                            //delete existing file
                            File.Delete(Server.MapPath("~/ApplicationDocs/Institute/" + institute.ShortName + "_banner" + fileExtension));
                        }
                        FileUploadBanner.SaveAs(Server.MapPath("~/ApplicationDocs/Institute/" + institute.ShortName + "_banner" + fileExtension));

                        lblMessageBanner.Text = "File Upload Complete";
                        lblMessageBanner.ForeColor = Color.Green;

                        DAL.Institute instituteObjToUpdate = new DAL.Institute();
                        using (var db = new GeneralDataManager())
                        {
                            instituteObjToUpdate = db.AdmissionDB.Institutes.Find(instituteId);
                            if(instituteObjToUpdate != null){
                                instituteObjToUpdate.BannerUrl = "~/ApplicationDocs/Institute/" + institute.ShortName + "_banner" + fileExtension;
                            }
                        }
                        using(var dbUpdateInstitute = new GeneralDataManager())
                        {
                            dbUpdateInstitute.Update<DAL.Institute>(instituteObjToUpdate);
                        }



                    }
                    catch (Exception ex)
                    {
                        lblMessageBanner.Text = "Unable to upload file.";
                        lblMessageBanner.ForeColor = Color.Crimson;
                    }
                }//if (FileUploadBanner.HasFile)
            }//if (institute != null)
            else
            {
                Response.Redirect("~/Admission/Message.aspx?message=" + "Something is not right... Contact Administrator.&type=danger", false);
            }
        }
    }
}