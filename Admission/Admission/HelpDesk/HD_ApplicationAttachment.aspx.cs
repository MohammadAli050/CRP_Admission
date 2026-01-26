using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.HelpDesk
{
    public partial class HD_ApplicationAttachment : System.Web.UI.Page
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

                hrefAppAdditional.NavigateUrl = "HD_ApplicationAdditional.aspx?val=" + queryVal;
                hrefAppAddress.NavigateUrl = "HD_ApplicationAddress.aspx?val=" + queryVal;
                hrefAppAttachment.NavigateUrl = "HD_ApplicationAttachment.aspx?val=" + queryVal;
                hrefAppBasic.NavigateUrl = "HD_ApplicationBasic.aspx?val=" + queryVal;
                hrefAppEducation.NavigateUrl = "HD_ApplicationEducation.aspx?val=" + queryVal;
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + cId;
                hrefAppPriority.NavigateUrl = "HD_ApplicationPriority.aspx?val=" + queryVal;
                hrefAppRelation.NavigateUrl = "HD_ApplicationRelation.aspx?val=" + queryVal;
            }

            if (!IsPostBack)
            {
                base.OnLoad(e);

                //FileUploadPhoto.Attributes["onchange"] = "UploadPhoto(this)";
                //FileUploadSignature.Attributes["onchange"] = "UploadSignature(this)";
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
    }
}