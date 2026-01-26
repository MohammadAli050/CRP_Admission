using Admission.App_Start;
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
    public partial class Listofdocument : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

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
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admission/Candidate/UploadDocumentsv1.aspx", false);
            }
            catch (Exception ex)
            {
            }
        }
    }
}