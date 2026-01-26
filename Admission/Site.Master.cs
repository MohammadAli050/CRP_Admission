using CommonUtility;
using DATAMANAGER;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission
{
    public partial class SiteMaster : MasterPage
    {
        string loginID = SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID); //the user name
        string roleName = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();

            long uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK

            using (var db = new GeneralDataManager())
            {
                DAL.Institute institute = db.AdmissionDB.Institutes.FirstOrDefault();
                if (institute != null)
                {
                    bannerImage.ImageUrl = institute.BannerUrl;
                }
            }

            if (uId > 0)
            {
                using (var db = new GeneralDataManager())
                {
                    if ((roleName == null || roleName != "Candidate") && roleName != "CertificateCandidate" && roleName != "PostgraduateDiploma")
                    {
                        var systemUser = db.AdmissionDB.SystemUsers.Find(uId);
                        if (systemUser != null && systemUser.ID > 0)
                        {
                            if (systemUser.IsActive == true && systemUser.IsSA == true)
                            {
                                lblUserName.Text = loginID;
                                //lbtnLogin.Visible = false;
                                btnLogin.Visible = false;
                                btnLogout.Visible = true;

                                //AdminPanel.Visible = true;
                                //OfficePanel.Visible = false;
                                //CandidatePanel.Visible = false;
                                //GeneralPanel.Visible = false;

                                hrefHome.Visible = true;
                                hrefAdmin.Visible = true;
                                hrefOffice.Visible = true;
                                hrefCandidate.Visible = false;
                            }
                            else if (systemUser.IsActive == true && systemUser.IsSA == false)
                            {
                                lblUserName.Text = loginID;
                                //lbtnLogin.Visible = false;
                                btnLogin.Visible = false;
                                btnLogout.Visible = true;

                                //AdminPanel.Visible = false;
                                //OfficePanel.Visible = true;
                                //CandidatePanel.Visible = false;
                                //GeneralPanel.Visible = false;

                                hrefHome.Visible = true;
                                hrefAdmin.Visible = false;
                                hrefOffice.Visible = true;
                                hrefCandidate.Visible = false;
                            }
                        }
                    }
                    else if (roleName == "Candidate")
                    {
                        string candidateName = null;
                        using (var dbCandidate = new CandidateDataManager())
                        {
                            DAL.BasicInfo candidate = dbCandidate.GetCandidateBasicInfoByUserID_ND(uId);
                            if (candidate != null)
                            {
                                candidateName = candidate.FirstName;
                            }
                        }

                        lblUserName.Text = candidateName ;
                        //lbtnLogin.Visible = false;
                        btnLogin.Visible = false;
                        btnLogout.Visible = true;

                        //AdminPanel.Visible = false;
                        //OfficePanel.Visible = false;
                        //CandidatePanel.Visible = true;
                        //GeneralPanel.Visible = false;
                        hrefHome.Visible = true;
                        hrefAdmin.Visible = false;
                        hrefOffice.Visible = false;
                        hrefCandidate.Visible = true;
                    }
                    else if (roleName == "CertificateCandidate")
                    {
                        string candidateName = null;
                        using (var dbCandidate = new CandidateDataManager())
                        {
                            DAL.CertificateBasicInfo candidate = db.AdmissionDB.CertificateBasicInfoes.Where(x => x.CandidateUserID == uId).FirstOrDefault();
                            if (candidate != null)
                            {
                                candidateName = candidate.FirstName;
                            }
                        }

                        lblUserName.Text = candidateName ;
                        //lbtnLogin.Visible = false;
                        btnLogin.Visible = false;
                        btnLogout.Visible = true;

                        //AdminPanel.Visible = false;
                        //OfficePanel.Visible = false;
                        //CandidatePanel.Visible = true;
                        //GeneralPanel.Visible = false;
                        hrefHome.Visible = false;
                        hrefAdmin.Visible = false;
                        hrefOffice.Visible = false;
                        hrefCandidate.Visible = false;
                    }
                    else if (roleName == "PostgraduateDiploma")
                    {
                        string candidateName = null;
                        using (var dbCandidate = new CandidateDataManager())
                        {
                            DAL.PostgraduateDiplomaBasicInfo candidate = db.AdmissionDB.PostgraduateDiplomaBasicInfoes.Where(x => x.CandidateUserID == uId).FirstOrDefault();
                            if (candidate != null)
                            {
                                candidateName = candidate.FirstName;
                            }
                        }

                        lblUserName.Text = candidateName ;
                        //lbtnLogin.Visible = false;
                        btnLogin.Visible = false;
                        btnLogout.Visible = true;

                        //AdminPanel.Visible = false;
                        //OfficePanel.Visible = false;
                        //CandidatePanel.Visible = true;
                        //GeneralPanel.Visible = false;
                        hrefHome.Visible = false;
                        hrefAdmin.Visible = false;
                        hrefOffice.Visible = false;
                        hrefCandidate.Visible = false;
                    }
                }
            }
            else
            {
                //AdminPanel.Visible = false;
                //OfficePanel.Visible = false;
                //CandidatePanel.Visible = false;
                //GeneralPanel.Visible = true;
                hrefHome.Visible = true;
                hrefAdmin.Visible = false;
                hrefOffice.Visible = false;
                hrefCandidate.Visible = false;

            }

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            lblUserName.Text = string.Empty;
            lblUserName.Visible = false;
            btnLogin.Visible = true;
            //lbtnLogin.Visible = true;
            btnLogout.Visible = false;
            SessionSGD.DeleteFromSession(SessionName.Common_UserId);
            SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
            SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
            SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
            SessionSGD.DeleteFromSession(SessionName.Common_UserG);
            Response.Redirect("~/Admission/Home.aspx", false);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admission/Login.aspx", false);
        }

    }
}