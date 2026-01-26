using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace Admission.Admission.HelpDesk
{
    public partial class HD_PrintCandidateAdmitCard : PageBase
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
                //LoadDDL();
            }
        }

        //private void LoadDDL()
        //{
        //    try
        //    {
        //        using (var db = new GeneralDataManager())
        //        {
        //            DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, db.AdmissionDB.AdmissionUnits.OrderBy(a => a.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        private void LoadAdmitCard(long candidateId, long admUnitId)
        {
            MessageView("", "clear");

            try
            {
                bool instructionImageHS = false;
                bool importantInstructionHS = false;

                int programId = -1;
                if (candidateId > 0)
                {
                    List<DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result> admitCardDetailsList = null;

                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admitCardDetailsList = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateIDForAdmin(candidateId).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        //lblMessage.Text = "Error getting admit card info: " + ex.Message;
                        //messagePanel.CssClass = "alert alert-danger";
                        //messagePanel.Visible = true;

                        MessageView("Error getting admit card info: " + ex.Message, "fail");
                        return;
                    }


                    DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result admitCardDetails = null;
                    try
                    {
                        if (admitCardDetailsList != null && admitCardDetailsList.Count > 0)
                        {
                            using (var db = new GeneralDataManager())
                            {
                                List<DAL.AdmissionUnit> admUnitList = (from acdl in admitCardDetailsList
                                                                       join admUnit in db.AdmissionDB.AdmissionUnits on acdl.admUnitID equals admUnit.ID
                                                                       orderby admUnit.UnitName
                                                                       select admUnit).ToList();


                                if (admUnitList != null && admUnitList.Count > 0)
                                {
                                    DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmissionUnit, admUnitList.OrderBy(a => a.UnitName).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
                                }
                            }


                            if (admUnitId > 0)
                            {
                                admitCardDetails = admitCardDetailsList.Where(x => x.admUnitID == admUnitId).FirstOrDefault();
                            }
                            else
                            {
                                admitCardDetails = admitCardDetailsList.Where(x => x.TestRoll != null).FirstOrDefault();
                            }


                        }
                    }
                    catch (Exception)
                    {

                    }




                    //DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result admitCardDetails = null;
                    //try
                    //{
                    //    using (var db = new OfficeDataManager())
                    //    {
                    //        admitCardDetails = db.AdmissionDB.SPGetDetailsForAdmitCardByCandidateIDForAdmin(candidateId).FirstOrDefault();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    lblMessage.Text = "Error getting admit card info: " + ex.Message;
                    //    messagePanel.CssClass = "alert alert-danger";
                    //    messagePanel.Visible = true;
                    //}

                    List<DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result> list =
                        new List<DAL.SPGetDetailsForAdmitCardByCandidateIDForAdmin_Result>();

                    list.Add(admitCardDetails);

                    if (admitCardDetails != null)
                    {

                        DAL.CandidateFormSl formSL = null;
                        DAL.AdmissionSetup admSet = null;
                        DAL.AdmissionUnitProgram admUnitProg = null;
                        using (var db = new GeneralDataManager())
                        {
                            formSL = db.AdmissionDB.CandidateFormSls.Where(x => x.FormSerial == admitCardDetails.FormSerial).FirstOrDefault();
                            if (formSL != null)
                            {
                                admSet = db.AdmissionDB.AdmissionSetups.Where(x => x.ID == formSL.AdmissionSetupID).FirstOrDefault();
                                if (admSet != null)
                                {
                                    admUnitProg = db.AdmissionDB.AdmissionUnitPrograms.Where(x => x.IsActive == true
                                                                                              && x.EducationCategoryID == admSet.EducationCategoryID
                                                                                              && x.AcaCalID == admSet.AcaCalID
                                                                                              && x.AdmissionUnitID == admSet.AdmissionUnitID).FirstOrDefault();

                                    if (admUnitProg != null)
                                    {
                                        programId = admUnitProg.ProgramID;
                                    }
                                }
                            }
                        }

                        if (programId > 0 && programId == 7) // EMBA Pro. = 7; jnno Image Isntruction ta dekhabe
                        {
                            instructionImageHS = true;
                            importantInstructionHS = false;
                        }
                        else
                        {
                            instructionImageHS = false;
                            importantInstructionHS = true;
                        }


                        string msgData = string.Empty;

                        if (string.IsNullOrEmpty(admitCardDetails.TestRoll))
                        {
                            msgData += "Test roll not available; ";
                        }
                        if (admitCardDetails.IsFinalSubmit == false)
                        {
                            msgData += "Not final submitted.";
                        }

                        if (!string.IsNullOrEmpty(msgData))
                        {
                            lblMessage.Text = msgData;
                            messagePanel.CssClass = "alert alert-danger";
                            messagePanel.Visible = true;
                        }

                        try
                        {
                            ReportViewer1.LocalReport.EnableExternalImages = true;

                            ReportDataSource rds = new ReportDataSource("DataSet1", list);

                            string imgUrl = new Uri(Server.MapPath(admitCardDetails.PhotoPath)).AbsoluteUri;
                            string SignatureUrl = new Uri(Server.MapPath(admitCardDetails.SignPath)).AbsoluteUri;
                            //string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                            //string examCenter = admitCardDetails.campusName;



                            DAL.Room roomModel = null;
                            string floorName = "-";
                            using (var db = new GeneralDataManager())
                            {
                                roomModel = db.AdmissionDB.Rooms.Where(x => x.ID == admitCardDetails.prpRoomID).FirstOrDefault();
                            }
                            if (roomModel != null)
                            {
                                floorName = roomModel.FloorNumber.ToString();
                            }


                            //string examCenter = admitCardDetails.prpRoomName + ", " + floorName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;
                            //string examCenter = admitCardDetails.campusName;
                            string examCenter = admitCardDetails.prpRoomName + ", " + admitCardDetails.buildingName + ", " + admitCardDetails.campusName;



                            DateTime examDate = Convert.ToDateTime(admitCardDetails.ExamDate);

                            IList<ReportParameter> param1 = new List<ReportParameter>();
                            param1.Add(new ReportParameter("FacultyName", admitCardDetails.UnitName));
                            param1.Add(new ReportParameter("CandidateName", admitCardDetails.FirstName));
                            param1.Add(new ReportParameter("CandidateImagePath", imgUrl));
                            param1.Add(new ReportParameter("CandidateSignPath", SignatureUrl));
                            param1.Add(new ReportParameter("FatherName", admitCardDetails.FatherName != null ? admitCardDetails.FatherName.ToUpper() : ""));
                            param1.Add(new ReportParameter("RollNumber", admitCardDetails.TestRoll != null ? admitCardDetails.TestRoll : ""));
                            param1.Add(new ReportParameter("ExamCenter", examCenter));
                            param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
                            param1.Add(new ReportParameter("ExamDateTime", examDate.ToString("dd-MMM-yyyy") + ", " + admitCardDetails.ExamTime.ToString()));
                            param1.Add(new ReportParameter("PrintTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")));
                            param1.Add(new ReportParameter("InstructionImageHS", instructionImageHS.ToString()));
                            param1.Add(new ReportParameter("ImportantInstructionHS", importantInstructionHS.ToString()));


                            #region Admit Card Instruction Parameter


                            try
                            {
                                using (var db = new GeneralDataManager())
                                {
                                    var insSetupObj = db.AdmissionDB.AdmitCardInstructions.Where(x => x.AcacalId == admitCardDetails.AcaCalID
                                    && x.AdmissionUnitId == admitCardDetails.admUnitID).FirstOrDefault();

                                    if (insSetupObj != null)
                                    {
                                        param1.Add(new ReportParameter("Instruction", insSetupObj.InstructionDetails.ToString()));
                                    }
                                    else
                                    {
                                        param1.Add(new ReportParameter("Instruction", ""));
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                            }
                            #endregion

                            ReportViewer1.LocalReport.SetParameters(param1);

                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rds);
                            ReportViewer1.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            lblMessage1.Text = ex.Message;
                            messagePanel1.CssClass = "alert alert-danger";
                            messagePanel1.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage1.Text = ex.Message;
                messagePanel1.CssClass = "alert alert-danger";
                messagePanel1.Visible = true;
            }
        }

        protected void btnMobileSearch_Click(object sender, EventArgs e)
        {
            string mobileNo = null;
            mobileNo = txtMobile.Text.Trim();

            List<DAL.BasicInfo> candidateList = null;
            if (!string.IsNullOrEmpty(mobileNo))
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        candidateList = db.AdmissionDB.BasicInfoes
                            .Where(c => c.SMSPhone == mobileNo)
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting candidate object: " + ex.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }

            DAL.BasicInfo candidateObj = null;
            if (candidateList != null)
            {
                if (candidateList.Count > 1)
                {
                    lblMessage.Text = "Multiple candidate exist with mobile number " + txtMobile.Text;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                else if (candidateList.Count == 1)
                {
                    foreach (DAL.BasicInfo item in candidateList)
                    {
                        candidateObj = item;
                    }
                }
            }
            else
            {
                lblMessage.Text = "Candidate does not exist with mobile number " + txtMobile.Text;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
                return;
            }

            if (candidateObj != null)
            {
                LoadAdmitCard(candidateObj.ID, 0);
            }

        }

        protected void btnPaymentIdSearch_Click(object sender, EventArgs e)
        {
            long paymentId = -1;
            paymentId = Convert.ToInt64(txtPaymentId.Text);

            DAL.CandidatePayment cPayment = null;
            if (paymentId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        cPayment = db.AdmissionDB.CandidatePayments
                            .Where(c => c.PaymentId == paymentId)
                            .FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting candidate payment object: " + ex.Message;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
            }

            if (cPayment != null)
            {
                LoadAdmitCard(Convert.ToInt64(cPayment.CandidateID), 0);
            }
            else
            {
                lblMessage.Text = "Candidate payment does not exist for payment id: " + txtPaymentId.Text;
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
                return;
            }
        }

        protected void ddlAdmissionUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long admUnitId = -1;
                if (!string.IsNullOrEmpty(ddlAdmissionUnit.SelectedItem.Text))
                {
                    admUnitId = Convert.ToInt64(ddlAdmissionUnit.SelectedValue);
                }


                long paymentId = -1;
                paymentId = Convert.ToInt64(txtPaymentId.Text);

                DAL.CandidatePayment cPayment = null;
                if (paymentId > 0)
                {
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cPayment = db.AdmissionDB.CandidatePayments
                                .Where(c => c.PaymentId == paymentId)
                                .FirstOrDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting candidate payment object: " + ex.Message;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }

                if (cPayment != null && admUnitId > 0)
                {
                    LoadAdmitCard(Convert.ToInt64(cPayment.CandidateID), admUnitId);
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}