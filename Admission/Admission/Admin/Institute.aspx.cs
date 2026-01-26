using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using Admission.App_Start;

namespace Admission.Admission.Admin
{
    public partial class Institute : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                LoadListView();
            }
        }

        public int CurrentInstituteID
        {
            get
            {
                if (ViewState["CurrentInstituteID"] == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(ViewState["CurrentInstituteID"].ToString());
                }
            }
            set
            {
                ViewState["CurrentInstituteID"] = value;
            }
        }

        public int CurrentInstituteIDEdit
        {
            get
            {
                if (ViewState["CurrentInstituteIDEdit"] == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(ViewState["CurrentInstituteIDEdit"].ToString());
                }
            }
            set
            {
                ViewState["CurrentInstituteIDEdit"] = value;
            }
        }

        #region LISTVIEW METHODS

        private void LoadListView()
        {
            using (var db = new GeneralDataManager())
            {
                List<DAL.Institute> instituteList = new List<DAL.Institute>();
                instituteList = db.AdmissionDB.Institutes.ToList();

                lvInstituteList.DataSource = instituteList;
                lvInstituteList.DataBind();

                if(instituteList.Count == 1)
                {
                    btnSave.Enabled = false;
                }
            }
        }

        protected void lvInstituteList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Institute institute = (DAL.Institute)((ListViewDataItem)(e.Item)).DataItem;

                //lvRowCount += 1;
                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblShortName = (Label)currentItem.FindControl("lblShortName");
                Label lblAddress = (Label)currentItem.FindControl("lblAddress");
                Label lblPostCode = (Label)currentItem.FindControl("lblPostCode");
                Label lblTel1 = (Label)currentItem.FindControl("lblTel1");
                Label lblTel2 = (Label)currentItem.FindControl("lblTel2");
                Label lblFax = (Label)currentItem.FindControl("lblFax");
                Label lblMobile = (Label)currentItem.FindControl("lblMobile");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");
                LinkButton lnkUpload = (LinkButton)currentItem.FindControl("lnkUpload");

                lblName.Text = institute.Name;
                lblShortName.Text = institute.ShortName;
                lblAddress.Text = institute.Address;
                lblPostCode.Text = institute.PostCode;
                lblTel1.Text = institute.Telephone1;
                lblTel2.Text = institute.Telephone2;
                lblFax.Text = institute.Fax;
                lblMobile.Text = institute.Mobile;

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = institute.ID.ToString();

                lnkUpload.CommandName = "Upload";
                lnkUpload.CommandArgument = institute.ID.ToString();

                lnkEdit.CommandName = "Edit";
                lnkEdit.CommandArgument = institute.ID.ToString();
            }
        }

        protected void lvInstituteList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var institute = new DAL.Institute();
            if (e.CommandName == "Upload")
            {
                Response.Redirect("~/Admission/Admin/InstituteImageUpload.aspx?insId=" + e.CommandArgument);
            }
            else if (e.CommandName == "Delete")
            {
                using (var db = new GeneralDataManager())
                {
                    var objectToDelete = db.AdmissionDB.Institutes.Find(Convert.ToInt32(e.CommandArgument));
                    db.Delete<DAL.Institute>(objectToDelete);
                    CurrentInstituteID = 0;
                    LoadListView();

                }
            }
            else if (e.CommandName == "Update")
            {
                //btnSave.Text = "Update";
                //btnSave.Enabled = true;
                //using (var db = new GeneralDataManager())
                //{
                //    var instituteObjLoad = db.AdmissionDB.Institutes.Find(Convert.ToInt64(e.CommandArgument));

                //    txtInstituteName.Text = instituteObjLoad.Address;
                //    instituteObjLoad.Fax;
                //    instituteObjLoad.Mobile;
                //    instituteObjLoad.Name;
                //    instituteObjLoad.PostCode;
                //    instituteObjLoad.ShortName;
                //    instituteObjLoad.Telephone1;
                //    instituteObjLoad.Telephone2;
                //    instituteObjLoad.Type;
                //    instituteObjLoad.Email;
                //    instituteObjLoad.Website;
                    
                //    CurrentInstituteIDEdit = instituteObjLoad.ID;
                //}
            }
        }

        protected void lvInstituteList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvInstituteList_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //bool isLogoUploaded = false;
            //bool isBannerUploaded = false;
            //string logoFilePath = null;
            //string bannerFilePath = null;
            //if (fileUploadLogo.HasFile)
            //{
            //    try
            //    {
            //        string filename = Path.GetFileName(fileUploadLogo.FileName);
            //        fileUploadLogo.SaveAs(Server.MapPath("~/ApplicationDocs/Institute/" + filename));
            //        isLogoUploaded = true;
            //        logoFilePath = "~/ApplicationDocs/Institute/" + filename;
            //    }
            //    catch (IOException ioEx)
            //    {
            //        isLogoUploaded = false;
            //        lblMessage.Text = "Logo not uploaded.";
            //        lblMessage.CssClass = "label label-danger";
            //    }
            //}
            //if (fileUploadBanner.HasFile)
            //{
            //    try
            //    {
            //        string filename = Path.GetFileName(fileUploadBanner.FileName);
            //        fileUploadBanner.SaveAs(Server.MapPath("~/ApplicationDocs/Institute/" + filename));
            //        isBannerUploaded = true;
            //        bannerFilePath = "~/ApplicationDocs/Institute/" + filename;
            //    }
            //    catch (IOException ioEx)
            //    {
            //        isBannerUploaded = false;
            //        lblMessage.Text = "Banner not uploaded.";
            //        lblMessage.CssClass = "label label-danger";
            //    }
            //}

            //if (isLogoUploaded && isBannerUploaded)

            DAL.Institute instituteObj = new DAL.Institute();

            instituteObj.Address = txtAddress.Text;
            instituteObj.Attribute1 = null; //nothing for now
            instituteObj.Attribute2 = null; //nothing for now
            instituteObj.Fax = txtFax.Text;
            instituteObj.Mobile = txtMobile.Text;
            instituteObj.Name = txtInstituteName.Text;
            instituteObj.PostCode = txtPostCode.Text;
            instituteObj.ShortName = txtShortName.Text;
            instituteObj.Telephone1 = txtTelephone1.Text;
            instituteObj.Telephone2 = txtTelephone2.Text;
            instituteObj.Type = "School/College/University";
            instituteObj.Email = txtEmail.Text;
            instituteObj.Website = txtWebsite.Text;
            //instituteObj.LogoUrl = logoFilePath;
            //instituteObj.BannerUrl = "";

            int instituteID = 0;
            using (var db = new GeneralDataManager())
            {
                db.Insert<DAL.Institute>(instituteObj);
                instituteID = instituteObj.ID;
                if (instituteID > 0)
                {
                    lblMessage.Text = "Institute added successfully.";
                    lblMessage.CssClass = "alert alert-success";
                }
            }
            LoadListView();
        }
    }
}