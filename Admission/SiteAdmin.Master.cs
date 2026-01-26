using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission
{
    public partial class SiteAdmin : System.Web.UI.MasterPage
    {
        string loginID = SessionSGD.GetObjFromSession<string>(SessionName.Common_LoginID); //the user name
        string roleName = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();

            long uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);

            if (uId > 0)
            {

                lblUserName.Text = loginID;
                //btnLogin.Visible = false;
                hrefLogin.Visible = false;
                btnLogout.Visible = true;
                //TODO get menus for a particular role
                if (roleName == "Admin")
                {
                    //using (var db = new OfficeDataManager())
                    //{
                    //    List<DAL.SPMenuGetAllMenuByRoleName_Result> list =
                    //        db.AdmissionDB.SPMenuGetAllMenuByRoleName(roleName)
                    //        .Where(a => a.ParentMenuID == null)
                    //        .OrderBy(a => a.MenuOrder)
                    //        .ToList();
                    //    if (list.Any())
                    //    {
                    //        PopulateMenu(list, null, null);
                    //    }
                    //}
                    menuAdmin.Items.Clear();
                    menuAdmin.Items.Add(new MenuItem { Text = "Home", Value = "Home", NavigateUrl = "~/Admission/Home" });
                    menuAdmin.Items.Add(new MenuItem { Text = "AdminHome", Value = "Admin Home", NavigateUrl = "~/Admission/Admin/AdminHome" });
                    
                    MenuItem sysSetupMenu = new MenuItem("SystemSetup", "SystemSetup");
                    sysSetupMenu.ChildItems.Add(new MenuItem { Text = "Admin SETUP - Institute", Value = "Admin SETUP - Institute", NavigateUrl = "~/Admission/Admin/Institute" });
                    sysSetupMenu.ChildItems.Add(new MenuItem { Text = "Admin SETUP - Menu", Value = "Admin SETUP - Menu", NavigateUrl = "~/Admission/Admin/Menu" });
                    sysSetupMenu.ChildItems.Add(new MenuItem { Text = "Admin SETUP - Store", Value = "Admin SETUP - Store", NavigateUrl = "~/Admission/Admin/StoreSetup" });
                    sysSetupMenu.ChildItems.Add(new MenuItem { Text = "Teletalk Data", Value = "Teletalk Data", NavigateUrl = "~/Admission/Office/SSCHSCVarification.aspx" });
                    sysSetupMenu.ChildItems.Add(new MenuItem { Text = "Admin SETUP - Foster Store", Value = "Admin SETUP - Foster Store", NavigateUrl = "~/Admission/Admin/FosterStoreSetup" });
                    sysSetupMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(sysSetupMenu);

                    MenuItem noticeMenu = new MenuItem("Notice", "Notice");
                    noticeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Notice", Value = "Office SETUP - Notice", NavigateUrl = "~/Admission/Office/Notice" });
                    noticeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Important Notice For Home", Value = "Office SETUP - Important Notice For Home", NavigateUrl = "~/Admission/Office/NoticeForHomeImportant" });
                    noticeMenu.ChildItems.Add(new MenuItem { Text = "View All Notice", Value = "View All Notice", NavigateUrl = "~/Admission/NoticeAll" });
                    noticeMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(noticeMenu);

                    MenuItem officeMenu = new MenuItem("Setup", "Setup");
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Admission Unit/Faculty/Program", Value = "Office SETUP - Admission Unit/Faculty/Program", NavigateUrl = "~/Admission/Office/AdmissionUnit" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Admission Unit Program", Value = "Office SETUP - Admission Unit Program", NavigateUrl = "~/Admission/Office/AdmissionUnitPrograms" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Admission Setup", Value = "Office SETUP - Admission Exam", NavigateUrl = "~/Admission/Office/AdmissionSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Admission Setup Of Other MSC Programs", Value = "Office SETUP - Admission Setup Of Other MSC Programs", NavigateUrl = "~/Admission/Office/AdmissionSetupOfOtherMSCPrograms" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Admission Setup Of MPhil And PhD Programs", Value = "Office SETUP - Admission Setup Of MPhil And PhD Programs", NavigateUrl = "~/Admission/Office/AdmissionSetupOfMPhilAndPhdPrograms" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - District Seat Plan", Value = "Office SETUP - District Seat Plan", NavigateUrl = "~/Admission/Office/DistrictForSeatPlanSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Campus", Value = "Office SETUP - Campus", NavigateUrl = "~/Admission/Office/CampusSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Building", Value = "Office SETUP - Building", NavigateUrl = "~/Admission/Office/BuildingSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Room", Value = "Office SETUP - Room", NavigateUrl = "~/Admission/Office/RoomSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Admission SSC HSC GPA", Value = "Office SETUP - SSC HSC GPA", NavigateUrl = "~/Admission/Office/AdmissionSscHscGpa" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Unit District Prior", Value = "Office SETUP - Unit District Prior", NavigateUrl = "~/Admission/Office/UnitWiseDistrictSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Unit Campus Prior", Value = "Office SETUP - Unit Campus Prior", NavigateUrl = "~/Admission/Office/UnitWiseCampusSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Unit Building Prior", Value = "Office SETUP - Unit Building Prior", NavigateUrl = "~/Admission/Office/UnitWiseBuildingSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Unit Room Prior", Value = "Office SETUP - Unit Room Prior", NavigateUrl = "~/Admission/Office/UnitWiseRoomSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Admission Property Setup", Value = "Office SETUP - Admission Property Setup", NavigateUrl = "~/Admission/Office/AdmissionPropertySetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - District Seat Limit Setup", Value = "District Seat Limit Setup", NavigateUrl = "~/Admission/Office/DistrictSeatLimitSetup" });

                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - SSC - HSC Passing Year Setup", Value = "SSC - HSC Passing Year Setup", NavigateUrl = "~/Admission/Office/AdmissionSSCHSCPassingYearSetup" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Image Transfer From Admission To UCAM", Value = "SSC - HSC Passing Year Setup", NavigateUrl = "~/Admission/Office/ImageTransferFromAdmissionToUCAM.aspx" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Image Transfer Manual Admission To Folder", Value = "SSC - HSC Passing Year Setup", NavigateUrl = "~/Admission/Office/ImageTransferManualAdmissionToFolder.aspx" });


                    officeMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "~/Admission/Office/" });
                    officeMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "~/Admission/Office/" });
                           
                    menuAdmin.Items.Add(officeMenu);

                    MenuItem formsMenu = new MenuItem("Forms", "Forms");
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Unpaid Forms", Value = "Office - Unpaid Forms", NavigateUrl = "~/Admission/Office/FormRequest" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Paid Forms", Value = "Office - Paid Forms", NavigateUrl = "~/Admission/Office/ReceiveForm" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Approve Bachelor Candidates", Value = "Office - Approve", NavigateUrl = "~/Admission/Office/ApproveCandidate" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Approve Masters Candidates", Value = "Office - Approve", NavigateUrl = "~/Admission/Office/ApproveCandidateGrad" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Delete Approve N/A", Value = "Office - Delete Approve N/A", NavigateUrl = "" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Generate - Test Roll", Value = "Office - Generate - Test Roll", NavigateUrl = "~/Admission/Office/TestRollGeneration" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Delete - Test Roll", Value = "Office - Delete - Test Roll", NavigateUrl = "~/Admission/Office/TestRollDelete", });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Generate - Seat Plan", Value = "Office - Generate - Seat Plan", NavigateUrl = "~/Admission/Office/SeatPlanGeneration" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Delete - Seat Plan", Value = "Office - Delete - Seat Plan", NavigateUrl = "~/Admission/Office/SeatPlanDelete" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Candidate Room Change", Value = "Office - Candidate Room Change", NavigateUrl = "~/Admission/Office/CandidateRoomChange" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Unpaid Forms (Foreign)", Value = "Office - Unpaid Forms (Foreign)", NavigateUrl = "~/Admission/Office/FormRequestForeign" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "Office - Paid Forms (Foreign)", Value = "Office - Paid Forms (Foreign)", NavigateUrl = "~/Admission/Office/ReceiveFormForeign" });

                    formsMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    formsMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(formsMenu);

                    MenuItem helpDeskMenu = new MenuItem("HelpDesk", "HelpDesk");
                    helpDeskMenu.ChildItems.Add(new MenuItem { Text = "Check Payment From Gateway", Value = "Check Payment From Gateway", NavigateUrl = "~/Admission/HelpDesk/CheckPaymentFromGatewayHelpDesk" });
                    helpDeskMenu.ChildItems.Add(new MenuItem { Text = "Unpaid Forms", Value = "Unpaid Forms", NavigateUrl = "~/Admission/HelpDesk/FormRequestHelpDesk" });
                    helpDeskMenu.ChildItems.Add(new MenuItem { Text = "Paid Forms", Value = "Paid Forms", NavigateUrl = "~/Admission/HelpDesk/ReceiveFormHelpDesk" });
                    helpDeskMenu.ChildItems.Add(new MenuItem { Text = "Resend SMS", Value = "Resend SMS", NavigateUrl = "~/Admission/HelpDesk/ResendSmsHelpDesk" });
                    helpDeskMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(helpDeskMenu);

                    MenuItem smsEmailMenu = new MenuItem("Sms&Email", "Sms&Email");
                    smsEmailMenu.ChildItems.Add(new MenuItem { Text = "Resend Candidate SMS", Value = "Resend Candidate SMS", NavigateUrl = "~/Admission/Office/ResendSms" });
                    smsEmailMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    smsEmailMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(smsEmailMenu);

                    MenuItem transacMenu = new MenuItem("Transaction", "Transaction");
                    transacMenu.ChildItems.Add(new MenuItem { Text = "Add Candidate Transaction", Value = "Add Candidate Transaction", NavigateUrl = "~/Admission/Office/CandidateTransaction" });
                    transacMenu.ChildItems.Add(new MenuItem { Text = "Transaction History", Value = "Transaction History", NavigateUrl = "~/Admission/Office/TransactionsHistory" });
                    transacMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    transacMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(transacMenu);

                    MenuItem reportMenu = new MenuItem("Reports", "Reports");
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Admit Card", Value = "Office Rpt - Admit Card", NavigateUrl = "~/Admission/Office/Reports/RPTPrintCandidateAdmitCard" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Room wise Attendance Sheet", Value = "Office Rpt - Room wise Attendance Sheet", NavigateUrl = "~/Admission/Office/Reports/RPTAttendanceRoomWise" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Graduate Details for Viva", Value = "Office Rpt - Graduate Details for Viva", NavigateUrl = "~/Admission/Office/Reports/RPTDetailsForVivaGraduate" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Undergraduate Details for Viva", Value = "Office Rpt - Undergraduate Details for Viva", NavigateUrl = "~/Admission/Office/Reports/RPTDetailsForVivaUndergraduate" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Approved Candidates", Value = "Office Rpt - Approved Candidates", NavigateUrl = "~/Admission/Office/Reports/RPTApprovedCandidatesWithTestRoll" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Gender Count", Value = "Office Rpt - Gender Count", NavigateUrl = "" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Campus Wise Seat Plan", Value = "Office Rpt - Campus Wise Seat Plan", NavigateUrl = "~/Admission/Office/Reports/RPTCampusWiseSeatPlan" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office Rpt - Campus Wise Seat Plan All Campus", Value = "Office Rpt - Campus Wise Seat Plan All Campus", NavigateUrl = "~/Admission/Office/Reports/RPTCampusWiseSeatPlanAllCampus" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Bulk Admit Card Generate", Value = "Office - Bulk Admit Card Generate", NavigateUrl = "~/Admission/Office/Reports/BulkAdmitCardGenerate" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Bulk Admit Card File Manager", Value = "Office - Bulk Admit Card File Manager", NavigateUrl = "~/Admission/Office/BulkAdmitCardFileManager" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Faculty Wise Quota Count", Value = "Office - Faculty Wise Quota Count", NavigateUrl = "~/Admission/Office/Reports/FacultyWiseQuotaCount" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Viva Info For Selected Candidate", Value = "Office - Viva Info For Selected Candidate", NavigateUrl = "~/Admission/Office/VivaInfoForSelectedCandidate" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Attendance Report", Value = "Office - Attendance Report", NavigateUrl = "~/Admission/Office/Reports/RPTAttendanceReport" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Status Report", Value = "Office - Status Report", NavigateUrl = "~/Admission/Office/Reports/RPTStatusReport" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Quota Info", Value = "Office - Quota Info", NavigateUrl = "~/Admission/Office/FacultyWiseQuota" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - TeleTalk Verification", Value = "Office - TeleTalk Verification", NavigateUrl = "~/Admission/Office/Reports/RPTTeleTalkBoardResult" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Candidate Info", Value = "Office - Candidate Info", NavigateUrl = "~/Admission/Office/Reports/RPTPaidandFinalSumbitReport" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "Office - Mark Excel Upload", Value = "Office - Mark Excel Upload", NavigateUrl = "~/Admission/Office/AdmissionTestMarkExcelUpload.aspx" });
                    reportMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(reportMenu);

                    MenuItem userMenu = new MenuItem("User&Role", "User&Role");
                    userMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - User", Value = "Office SETUP - User", NavigateUrl = "~/Admission/Office/User" });
                    userMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Role", Value = "Office SETUP - Role", NavigateUrl = "~/Admission/Office/Role" });
                    userMenu.ChildItems.Add(new MenuItem { Text = "Office SETUP - Menu In Role", Value = "Office SETUP - Menu In Role", NavigateUrl = "~/Admission/Office/MenuInRole" });
                    userMenu.ChildItems.Add(new MenuItem { Text = "Office - Candidate Login Cred", Value = "Office - Candidate Login Cred", NavigateUrl = "~/Admission/Office/CandidateLoginCredentials" });
                    userMenu.ChildItems.Add(new MenuItem { Text = "Office - Change User Password", Value = "Office - Change User Password", NavigateUrl = "~/Admission/Office/UserPasswordChange" });
                    userMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(userMenu);

                    MenuItem backupMenu = new MenuItem("Backup", "Backup");
                    backupMenu.ChildItems.Add(new MenuItem { Text = "Photo Signature Backup", Value = "Photo Signature Backup", NavigateUrl = "~/Admission/Admin/BackupCandidateDocuments" });
                    backupMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(backupMenu);

                    MenuItem logMenu = new MenuItem("Logs", "Logs");
                    logMenu.ChildItems.Add(new MenuItem { Text = "SMS Logs", Value = "SMS Logs", NavigateUrl = "" });
                    logMenu.ChildItems.Add(new MenuItem { Text = "Email Logs", Value = "Email Logs", NavigateUrl = "" });
                    logMenu.ChildItems.Add(new MenuItem { Text = "Logs", Value = "Logs", NavigateUrl = "~/Admission/Office/GeneralDataLog" });
                    logMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    logMenu.ChildItems.Add(new MenuItem { Text = "", Value = "", NavigateUrl = "" });
                    menuAdmin.Items.Add(logMenu);

                }
                else if (roleName != "Admin")
                {
                    using (var db = new OfficeDataManager())
                    {
                        List<DAL.SPMenuGetAllMenuByRoleName_Result> list =
                            db.AdmissionDB.SPMenuGetAllMenuByRoleName(roleName)
                            .Where(a => a.ParentMenuID == null && a.IsAdmin == false)
                            .OrderBy(a => a.MenuOrder)
                            .ToList();
                        if (list.Any())
                        {
                            PopulateMenu(list, null, null);
                        }
                        else
                        {
                            menuAdmin.Items.Clear();
                            menuAdmin.Items.Add
                                (
                                    new MenuItem { Text = "Home", Value = "Home", NavigateUrl = "~/Admission/Home" }
                                );
                        }
                    }
                }
                
            }

            //List<DAL.Menu> list = new List<DAL.Menu>();
            //using (var db = new GeneralDataManager())
            //{
            //    list = db.AdmissionDB.Menus
            //        .Where(a => a.ParentMenuID == null)
            //        .OrderBy(a => a.MenuOrder)
            //        .ToList();
            //}
            //if (list.Any())
            //{
            //    PopulateMenu(list, null, null);
            //}
            //else
            //{
            //    menuAdmin.Items.Clear();
            //    menuAdmin.Items.Add
            //        (
            //            new MenuItem { Text = "Home", Value = "Home", NavigateUrl = "~/Admission/Home" }
            //        );
            //}

        }

        private void PopulateMenu(List<DAL.SPMenuGetAllMenuByRoleName_Result> menuItemsList, long? parentMenuId, MenuItem parentMenuItem)
        {
            //string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
            //MenuItem menuItem = new MenuItem();
            foreach (var item in menuItemsList)
            {
                MenuItem menuItem = new MenuItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Name,
                    NavigateUrl = item.URL
                };
                if (parentMenuId == null)
                {
                    menuAdmin.Items.Add(menuItem);
                    using (var db = new GeneralDataManager())
                    {
                        long parentMenuIdLong = Convert.ToInt64(menuItem.Value);
                        List<DAL.SPMenuGetAllMenuByRoleName_Result> childItems = db.AdmissionDB.SPMenuGetAllMenuByRoleName(roleName)
                            .Where(a => a.ParentMenuID == parentMenuIdLong)
                            .OrderBy(a => a.MenuOrder)
                            .ToList();
                        PopulateMenu(childItems, Convert.ToInt64(menuItem.Value), menuItem);
                    }

                }
                else
                {
                    parentMenuItem.ChildItems.Add(menuItem);
                }
            }
        }

        //private void PopulateMenu(List<DAL.Menu> menuItemsList, long? parentMenuId, MenuItem parentMenuItem)
        //{
        //    //string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
        //    foreach(var item in menuItemsList)
        //    {
        //        MenuItem menuItem = new MenuItem
        //        {
        //            Value = item.ID.ToString(),
        //            Text = item.Name,
        //            NavigateUrl = item.URL
        //        };
        //        if(parentMenuId == null)
        //        {
        //            menuAdmin.Items.Add(menuItem);
        //            using(var db = new GeneralDataManager())
        //            {
        //                long parentMenuIdLong = Convert.ToInt64(menuItem.Value);
        //                List<DAL.Menu> childItems = db.AdmissionDB.Menus
        //                    .Where(a => a.ParentMenuID == parentMenuIdLong)
        //                    .OrderBy(a=>a.MenuOrder)
        //                    .ToList();
        //                PopulateMenu(childItems, Convert.ToInt64(menuItem.Value), menuItem);
        //            }

        //        }
        //        else
        //        {
        //            parentMenuItem.ChildItems.Add(menuItem);
        //        }
        //    }
        //}

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            lblUserName.Text = string.Empty;
            lblUserName.Visible = false;
            //btnLogin.Visible = true;
            hrefLogin.Visible = true;
            btnLogout.Visible = false;
            SessionSGD.DeleteFromSession(SessionName.Common_UserId);
            SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
            SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
            SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
            SessionSGD.DeleteFromSession(SessionName.Common_UserG);
            Response.Redirect("~/Admission/Home.aspx", false);
        }
    }
}
