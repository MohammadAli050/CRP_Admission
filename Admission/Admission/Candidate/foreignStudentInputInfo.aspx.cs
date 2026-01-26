using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Candidate
{
    public partial class foreignStudentInputInfo : System.Web.UI.Page
    {
        int CategoryId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            CategoryId = Convert.ToInt32(Request.QueryString["ecat"]);
            if (!IsPostBack)
            {
            }

        }

        protected void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                lblmsg.Text = string.Empty;
                if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtEmail.Text))
                {
                    using (var db = new CandidateDataManager())
                    {
                        DAL.BasicInfo bi = db.AdmissionDB.BasicInfoes.Where(x => x.Email == txtEmail.Text.Trim()).FirstOrDefault();

                        if (bi != null)
                        {
                            lblmsg.Text = "You have already applied! If you want to apply to more program then login with your credential and add more program.";
                            return;
                        }
                    }
                    #region CandidateUser/BasicInfo

                    //---------------------------------------------------------------------------------
                    //insert candidate user
                    //---------------------------------------------------------------------------------
                    DAL.CandidateUser candidateUser = new DAL.CandidateUser();

                    candidateUser.UsernameLoginId = CommonLogic.AdmissionLoginId();
                    candidateUser.Password = CommonLogic.AdmissionPassword();
                    candidateUser.IsConfirmed = false;
                    candidateUser.IsLocked = true;
                    candidateUser.ValidTill = DateTime.Now.AddMonths(4);
                    candidateUser.IsSentSms = false;
                    candidateUser.IsSentEmail = false;
                    candidateUser.RoleID = 2;
                    candidateUser.IsActive = true;
                    candidateUser.CreatedBy = -99;
                    candidateUser.DateCreated = DateTime.Now;

                    long candidateUserIdLong = -1;
                    using (var db = new CandidateDataManager())
                    {
                        db.Insert<DAL.CandidateUser>(candidateUser);
                        candidateUserIdLong = candidateUser.ID;

                        //---------------------------------------------------------------------------------
                        //insert candidate basic info
                        //---------------------------------------------------------------------------------
                        DAL.BasicInfo candidate = new DAL.BasicInfo();
                        candidate.FirstName = txtName.Text.ToUpper();
                        candidate.MiddleName = txtNamem.Text;
                        candidate.LastName = TextBoxl.Text;

                        candidate.Email = txtEmail.Text.Trim();
                        candidate.CandidateUserID = candidateUserIdLong;
                        candidate.IsActive = false;
                        candidate.UniqueIdentifier = Guid.NewGuid();
                        candidate.CreatedBy = -99;
                        candidate.DateCreated = DateTime.Now;
                        candidate.SMSPhone = "";
                        candidate.DateOfBirth = DateTime.Now;
                        candidate.Mobile = "";
                        candidate.AttributeInt2 = CategoryId;

                        long candidateIdLong = -1;

                        db.Insert<DAL.BasicInfo>(candidate);
                        candidateIdLong = candidate.ID;

                        try
                        {
                            if (candidateIdLong > 0 && candidateUserIdLong > 0)
                            {
                                DAL.CandidateUser cu = db.AdmissionDB.CandidateUsers.Where(x => x.ID == candidateUserIdLong).FirstOrDefault();
                                if (cu != null)
                                {
                                    string userName = cu.UsernameLoginId;
                                    cu.UsernameLoginId = userName + candidateIdLong.ToString();
                                    db.Update<DAL.CandidateUser>(cu);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        #region Additional Info

                        DAL.AdditionalInfo ExistingaddInfo = db.GetAdditionalInfoByCandidateID_ND(candidateIdLong);
                        if (ExistingaddInfo == null)
                        {
                            DAL.AdditionalInfo addInfo = new DAL.AdditionalInfo();
                            addInfo.CandidateID = candidateIdLong;
                            addInfo.IsEnrolled = false;
                            addInfo.IsFinalSubmit = false;
                            addInfo.IsForeignStudent = true;
                            addInfo.CreatedBy = -99;
                            addInfo.DateCreated = DateTime.Now;

                            db.Insert<DAL.AdditionalInfo>(addInfo);
                        }
                        #endregion

                        #region Email Send
                        bool IsSent = SendEmail(txtName.Text.Trim() + " " + txtNamem.Text + " " + TextBoxl.Text, txtEmail.Text.Trim(), candidateUser.UsernameLoginId, candidateUser.Password, candidateIdLong);
                        #endregion

                        if (IsSent)
                        {
                            lblmsg.Text = "You have applied successfully! Please check your email.";
                        }
                    }


                    #endregion
                }

                else
                    lblmsg.Text = "Please Enter Your Name and Email Address";
            }
            catch (Exception ex)
            {
            }
        }

        private bool SendEmail(string candidateName, string email, string username, string password, long candidateId)
        {

            //"<p>Please check your username and password given below: </p>" +
            string mailbody = "<p>Dear " + candidateName + ",</p>" +
                        "<p>Login to https://admission.bup.edu.bd/Admission/Login .</p>" + "<br/>" +

                        "<p><strong>Username:</strong> " + username + "<br/>" +
                        "<strong>Password:</strong> " + password + "<br/></p>" +
                        "<br/> <p><strong>Bangladesh University of Professionals</strong></p>";

            bool isEmailSent = EmailUtility.SendMail(email, "no-reply-2@bup.edu.bd", "BUP Admission", "Username and Password", mailbody);

            return isEmailSent;
        }
    }
}
