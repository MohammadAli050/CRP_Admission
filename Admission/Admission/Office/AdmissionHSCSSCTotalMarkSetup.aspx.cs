using Admission.App_Start;
using ClosedXML.Excel;
using CommonUtility;
using DAL;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class AdmissionHSCSSCTotalMarkSetup : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (uRole.ToLower() == "candidate")
            {

                SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                Response.Redirect("~/Admission/Home.aspx", false);
            }

            if (!IsPostBack)
            {
                divSetup.Visible = false;
                divCancel.Visible = false;
                btnLoad.Visible = true;
                hdnSetupId.Value = "0";
                LoadDDL();
                LoadPassingYear();
                btnLoad_Click(null, null);
            }
        }

        private void LoadPassingYear()
        {
            try
            {
                ddlPassingYear.Items.Clear();
                ddlPassingYear.AppendDataBoundItems = true;
                ddlPassingYear.Items.Add(new ListItem("-Select-", "0"));

                int EndYear = DateTime.Now.Year;

                int StartYear = 1950;

                for (int i = EndYear; i >= StartYear; i--)
                {
                    ddlPassingYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadDDL()
        {
            try
            {
                using (var db = new OfficeDataManager())
                {
                    DDLHelper.Bind<DAL.ExamType>(ddlExamType, db.AdmissionDB.ExamTypes.Where(x => x.IsActive != false && x.EducationMedium_ID == 2).OrderBy(c => c.ExamTypeName).ToList(), "Code", "ID", EnumCollection.ListItemType.All);
                    DDLHelper.Bind<DAL.EducationBoard>(ddlEducationBoard, db.AdmissionDB.EducationBoards.Where(x => x.IsActive != false && x.ShortName != null).OrderBy(c => c.BoardName).ToList(), "BoardName", "ID", EnumCollection.ListItemType.All);
                    DDLHelper.Bind<DAL.GroupOrSubject>(ddlGroup, db.AdmissionDB.GroupOrSubjects.Where(x => x.IsActive != false).OrderBy(c => c.GroupOrSubjectName).ToList(), "GroupOrSubjectName", "ID", EnumCollection.ListItemType.All);

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            btnLoad_Click(null, null);
        }

        protected void ddlEducationBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            btnLoad_Click(null, null);
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            btnLoad_Click(null, null);
        }

        protected void ddlPassingYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            btnLoad_Click(null, null);
        }

        private void ClearGridView()
        {
            try
            {
                gvSetupList.DataSource = null;
                gvSetupList.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                hdnSetupId.Value = "0";

                gvSetupList.DataSource = null;
                gvSetupList.DataBind();

                int ExamType = Convert.ToInt32(ddlExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlEducationBoard.SelectedValue);
                int Group = Convert.ToInt32(ddlGroup.SelectedValue);
                int PassingYear = Convert.ToInt32(ddlPassingYear.SelectedValue);
                using (var db = new OfficeDataManager())
                {
                    var SetupList = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.ToList();

                    if (SetupList != null && SetupList.Any())
                    {
                        if (ExamType != -1)
                            SetupList = SetupList.Where(x => x.ExamTypeID == ExamType).ToList();
                        if (EducationBoard != -1)
                            SetupList = SetupList.Where(x => x.EducationBoardID == EducationBoard).ToList();
                        if (Group != -1)
                            SetupList = SetupList.Where(x => x.GroupOrSubjectID == Group).ToList();
                        if (PassingYear != 0)
                            SetupList = SetupList.Where(x => x.Year == PassingYear).ToList();

                    }

                    List<ExamType> ExamTypeList = new List<DAL.ExamType>();
                    List<EducationBoard> EducationBoardList = new List<DAL.EducationBoard>();
                    List<GroupOrSubject> GroupOrSubjectList = new List<DAL.GroupOrSubject>();

                    ExamTypeList = db.AdmissionDB.ExamTypes.Where(x => x.IsActive != false && x.EducationMedium_ID == 2).ToList();
                    EducationBoardList = db.AdmissionDB.EducationBoards.Where(x => x.IsActive != false && x.ShortName != null).ToList();
                    GroupOrSubjectList = db.AdmissionDB.GroupOrSubjects.Where(x => x.IsActive != false).ToList();

                    if (SetupList != null && SetupList.Any())
                    {

                        var DistinctType = SetupList.Select(x => x.ExamTypeID).Distinct().ToList();
                        var DistinctBoard = SetupList.Select(x => x.EducationBoardID).Distinct().ToList();
                        var DistinctGroup = SetupList.Select(x => x.GroupOrSubjectID).Distinct().ToList();

                        #region Type

                        if (DistinctType != null && DistinctType.Any())
                        {
                            foreach (var item in DistinctType)
                            {
                                string Name = "";
                                var TypeObj = ExamTypeList.Where(x => x.ID == item).FirstOrDefault();
                                if (TypeObj != null)
                                    Name = TypeObj.Code;

                                SetupList.Where(w => w.ExamTypeID == item).ToList().ForEach(u =>
                                {
                                    u.Attribute3 = Name;
                                });
                            }
                        }
                        #endregion

                        #region Board

                        if (DistinctBoard != null && DistinctBoard.Any())
                        {
                            foreach (var item in DistinctBoard)
                            {
                                string Name = "";
                                var TypeObj = EducationBoardList.Where(x => x.ID == item).FirstOrDefault();
                                if (TypeObj != null)
                                    Name = TypeObj.BoardName;

                                SetupList.Where(w => w.EducationBoardID == item).ToList().ForEach(u =>
                                {
                                    u.Attribute4 = Name;
                                });
                            }
                        }
                        #endregion

                        #region Group

                        if (DistinctGroup != null && DistinctGroup.Any())
                        {
                            foreach (var item in DistinctGroup)
                            {
                                string Name = "";
                                var TypeObj = GroupOrSubjectList.Where(x => x.ID == item).FirstOrDefault();
                                if (TypeObj != null)
                                    Name = TypeObj.GroupOrSubjectName;

                                SetupList.Where(w => w.GroupOrSubjectID == item).ToList().ForEach(u =>
                                {
                                    u.Remarks = Name;
                                });
                            }
                        }
                        #endregion


                        if (SetupList.Any())
                        {
                            divSetup.Visible = true;
                            gvSetupList.DataSource = SetupList;
                            gvSetupList.DataBind();
                        }
                    }


                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int ExamType = Convert.ToInt32(ddlExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlEducationBoard.SelectedValue);
                int Group = Convert.ToInt32(ddlGroup.SelectedValue);
                int PassingYear = Convert.ToInt32(ddlPassingYear.SelectedValue);

                decimal Mark = 0;

                int InsertCount = 0;

                int SetupId = Convert.ToInt32(hdnSetupId.Value);

                if (PassingYear == 0)
                {
                    showAlert("Please select a passing year");
                    return;
                }

                if (string.IsNullOrEmpty(txtTotalMark.Text))
                {
                    showAlert("Please Enter Total Mark");
                    return;
                }

                Mark = Convert.ToDecimal(txtTotalMark.Text.Trim());

                if (SetupId == 0) //New Entry
                {
                    using (var db = new OfficeDataManager())
                    {
                        List<ExamType> ExamTypeList = new List<DAL.ExamType>();
                        List<EducationBoard> EducationBoardList = new List<DAL.EducationBoard>();
                        List<GroupOrSubject> GroupOrSubjectList = new List<DAL.GroupOrSubject>();

                        #region Exam Type 

                        if (ExamType == -1)
                        {
                            ExamTypeList = db.AdmissionDB.ExamTypes.Where(x => x.IsActive != false).ToList();
                        }
                        else
                        {
                            var ExamTypeObj = db.AdmissionDB.ExamTypes.Where(x => x.ID == ExamType).FirstOrDefault();
                            if (ExamTypeObj != null)
                            {
                                ExamTypeList.Add(ExamTypeObj);
                            }
                        }
                        #endregion

                        #region Education Board

                        if (EducationBoard == -1)
                        {
                            EducationBoardList = db.AdmissionDB.EducationBoards.Where(x => x.IsActive != false && x.ShortName != null).ToList();
                        }
                        else
                        {
                            var ExamBoardObj = db.AdmissionDB.EducationBoards.Where(x => x.ID == EducationBoard).FirstOrDefault();
                            if (ExamBoardObj != null)
                            {
                                EducationBoardList.Add(ExamBoardObj);
                            }
                        }
                        #endregion

                        #region Education Group

                        if (Group == -1)
                        {
                            GroupOrSubjectList = db.AdmissionDB.GroupOrSubjects.Where(x => x.IsActive != false).ToList();
                        }
                        else
                        {
                            var ExamGroupObj = db.AdmissionDB.GroupOrSubjects.Where(x => x.ID == Group).FirstOrDefault();
                            if (ExamGroupObj != null)
                            {
                                GroupOrSubjectList.Add(ExamGroupObj);
                            }
                        }
                        #endregion

                        #region Insert Process


                        foreach (var ExamTypeItem in ExamTypeList)
                        {
                            foreach (var EducationBoardItem in EducationBoardList)
                            {
                                foreach (var EducationGroupItem in GroupOrSubjectList)
                                {
                                    try
                                    {
                                        int TypeId = Convert.ToInt32(ExamTypeItem.ID);
                                        int BoardId = Convert.ToInt32(EducationBoardItem.ID);
                                        int GroupId = Convert.ToInt32(EducationGroupItem.ID);

                                        var ExistingObj = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.Where(x => x.ExamTypeID == TypeId
                                          && x.EducationBoardID == BoardId && x.GroupOrSubjectID == GroupId && x.Year == PassingYear).FirstOrDefault();

                                        if (ExistingObj == null)
                                        {

                                            DAL.ExamTypeWiseTotalMarksInformation NewObj = new ExamTypeWiseTotalMarksInformation();

                                            NewObj.ExamTypeID = TypeId;
                                            NewObj.EducationBoardID = BoardId;
                                            NewObj.GroupOrSubjectID = GroupId;
                                            NewObj.Year = PassingYear;
                                            NewObj.TotalMarks = Mark;
                                            NewObj.CreatedBy = Convert.ToInt32(uId);
                                            NewObj.CreatedDate = DateTime.Now;

                                            db.Insert<DAL.ExamTypeWiseTotalMarksInformation>(NewObj);

                                            InsertCount++;
                                        }
                                        else
                                        {
                                            ExistingObj.TotalMarks = Mark;
                                            ExistingObj.CreatedBy = Convert.ToInt32(uId);
                                            ExistingObj.CreatedDate = DateTime.Now;

                                            db.Update<DAL.ExamTypeWiseTotalMarksInformation>(ExistingObj);

                                            InsertCount++;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }

                        #endregion


                        if (InsertCount > 0)
                        {
                            showAlert("Information saved successfully");
                            ClearData();
                            btnLoad_Click(null, null);
                            return;
                        }
                        else
                        {
                            showAlert("Information save failed");
                            ClearData();
                            btnLoad_Click(null, null);
                            return;
                        }
                    }
                }
                else
                {
                    if (SetupId > 0)
                    {
                        hdnSetupId.Value = "0";
                        using (var db = new OfficeDataManager())
                        {
                            var ExistingObj = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.Where(x => x.ExamTypeID == ExamType
                              && x.EducationBoardID == EducationBoard && x.GroupOrSubjectID == Group && x.Year == PassingYear
                              && x.Id != SetupId).FirstOrDefault();

                            if (ExistingObj == null)
                            {
                                var UpdatingObj = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.Find(SetupId);
                                if (UpdatingObj != null)
                                {
                                    ClearData();

                                    UpdatingObj.ExamTypeID = ExamType;
                                    UpdatingObj.EducationBoardID = EducationBoard;
                                    UpdatingObj.GroupOrSubjectID = Group;
                                    UpdatingObj.Year = PassingYear;
                                    UpdatingObj.TotalMarks = Mark;
                                    UpdatingObj.ModifiedBy = Convert.ToInt32(uId);
                                    UpdatingObj.ModifiedDate = DateTime.Now;

                                    db.Update<DAL.ExamTypeWiseTotalMarksInformation>(UpdatingObj);

                                    showAlert("Information Updated Successfully");
                                    divCancel.Visible = false;
                                    btnLoad.Visible = true;

                                    ddlExamType.SelectedValue = ExamType.ToString();
                                    ddlEducationBoard.SelectedValue = EducationBoard.ToString();
                                    ddlGroup.SelectedValue = Group.ToString();
                                    ddlPassingYear.SelectedValue = PassingYear.ToString();

                                    btnLoad_Click(null, null);
                                    return;

                                }
                            }
                            else
                            {
                                showAlert("Same setup already exists");
                                return;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearData()
        {
            ddlExamType.SelectedValue = "-1";
            ddlEducationBoard.SelectedValue = "-1";
            ddlGroup.SelectedValue = "-1";
            ddlPassingYear.SelectedValue = "0";
            txtTotalMark.Text = string.Empty;
        }

        protected void EditItem_Click(object sender, EventArgs e)
        {
            try
            {
                hdnSetupId.Value = "0";
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);

                if (SetupId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        ClearData();
                        var ExistingObj = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.Find(SetupId);
                        if (ExistingObj != null)
                        {
                            hdnSetupId.Value = SetupId.ToString();
                            ddlExamType.SelectedValue = ExistingObj.ExamTypeID.ToString();
                            ddlEducationBoard.SelectedValue = ExistingObj.EducationBoardID.ToString();
                            ddlGroup.SelectedValue = ExistingObj.GroupOrSubjectID.ToString();
                            ddlPassingYear.SelectedValue = ExistingObj.Year.ToString();
                            txtTotalMark.Text = ExistingObj.TotalMarks.ToString();

                            divCancel.Visible = true;
                            btnLoad.Visible = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
            btnLoad.Visible = true;
            divCancel.Visible = false;
        }

        protected void DeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);

                if (SetupId > 0)
                {
                    using (var db = new OfficeDataManager())
                    {
                        var ExistingObj = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.Find(SetupId);
                        if (ExistingObj != null)
                        {
                            db.Delete<DAL.ExamTypeWiseTotalMarksInformation>(ExistingObj);

                            showAlert("Setup deleted successfully");

                            btnLoad_Click(null, null);

                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        #region Excel Download Methods

        protected void lnkDownloadExcel_Click(object sender, EventArgs e)
        {

            try
            {
                hdnSetupId.Value = "0";

                gvSetupList.DataSource = null;
                gvSetupList.DataBind();

                int ExamType = Convert.ToInt32(ddlExamType.SelectedValue);
                int EducationBoard = Convert.ToInt32(ddlEducationBoard.SelectedValue);
                int Group = Convert.ToInt32(ddlGroup.SelectedValue);
                int PassingYear = Convert.ToInt32(ddlPassingYear.SelectedValue);
                using (var db = new OfficeDataManager())
                {
                    var SetupList = db.AdmissionDB.ExamTypeWiseTotalMarksInformations.ToList();

                    if (SetupList != null && SetupList.Any())
                    {
                        if (ExamType != -1)
                            SetupList = SetupList.Where(x => x.ExamTypeID == ExamType).ToList();
                        if (EducationBoard != -1)
                            SetupList = SetupList.Where(x => x.EducationBoardID == EducationBoard).ToList();
                        if (Group != -1)
                            SetupList = SetupList.Where(x => x.GroupOrSubjectID == Group).ToList();
                        if (PassingYear != 0)
                            SetupList = SetupList.Where(x => x.Year == PassingYear).ToList();

                    }

                    List<ExamType> ExamTypeList = new List<DAL.ExamType>();
                    List<EducationBoard> EducationBoardList = new List<DAL.EducationBoard>();
                    List<GroupOrSubject> GroupOrSubjectList = new List<DAL.GroupOrSubject>();

                    ExamTypeList = db.AdmissionDB.ExamTypes.Where(x => x.IsActive != false && x.EducationMedium_ID == 2).ToList();
                    EducationBoardList = db.AdmissionDB.EducationBoards.Where(x => x.IsActive != false && x.ShortName != null).ToList();
                    GroupOrSubjectList = db.AdmissionDB.GroupOrSubjects.Where(x => x.IsActive != false).ToList();

                    if (SetupList != null && SetupList.Any())
                    {

                        var DistinctType = SetupList.Select(x => x.ExamTypeID).Distinct().ToList();
                        var DistinctBoard = SetupList.Select(x => x.EducationBoardID).Distinct().ToList();
                        var DistinctGroup = SetupList.Select(x => x.GroupOrSubjectID).Distinct().ToList();

                        #region Type

                        if (DistinctType != null && DistinctType.Any())
                        {
                            foreach (var item in DistinctType)
                            {
                                string Name = "";
                                var TypeObj = ExamTypeList.Where(x => x.ID == item).FirstOrDefault();
                                if (TypeObj != null)
                                    Name = TypeObj.Code;

                                SetupList.Where(w => w.ExamTypeID == item).ToList().ForEach(u =>
                                {
                                    u.Attribute3 = Name;
                                });
                            }
                        }
                        #endregion

                        #region Board

                        if (DistinctBoard != null && DistinctBoard.Any())
                        {
                            foreach (var item in DistinctBoard)
                            {
                                string Name = "";
                                var TypeObj = EducationBoardList.Where(x => x.ID == item).FirstOrDefault();
                                if (TypeObj != null)
                                    Name = TypeObj.BoardName;

                                SetupList.Where(w => w.EducationBoardID == item).ToList().ForEach(u =>
                                {
                                    u.Attribute4 = Name;
                                });
                            }
                        }
                        #endregion

                        #region Group

                        if (DistinctGroup != null && DistinctGroup.Any())
                        {
                            foreach (var item in DistinctGroup)
                            {
                                string Name = "";
                                var TypeObj = GroupOrSubjectList.Where(x => x.ID == item).FirstOrDefault();
                                if (TypeObj != null)
                                    Name = TypeObj.GroupOrSubjectName;

                                SetupList.Where(w => w.GroupOrSubjectID == item).ToList().ForEach(u =>
                                {
                                    u.Remarks = Name;
                                });
                            }
                        }
                        #endregion


                        if (SetupList.Any())
                        {

                            var NewList = SetupList
                            .Select(obj => new
                            {
                                ExamType = obj.Attribute3,
                                Board = obj.Attribute4,
                                Group = obj.Remarks,
                                PassingYear = obj.Year,
                                TotalMarks = obj.TotalMarks
                            })
                            .ToList();

                            DataTable dtExcel = ListToDataTableManager.ToDataTable(NewList);

                            if (dtExcel != null && dtExcel.Rows.Count > 0)
                            {


                                using (XLWorkbook wb = new XLWorkbook())
                                {

                                    IXLWorksheet sheet2;
                                    sheet2 = wb.AddWorksheet(dtExcel, "Sheet");

                                    sheet2.Table("Table1").ShowAutoFilter = false;

                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=" + "TotalMarks.xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);

                                        HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                                        cookie.Value = "Flag";
                                        cookie.Expires = DateTime.Now.AddDays(1);
                                        Response.AppendCookie(cookie);

                                        Response.Flush();
                                        Response.SuppressContent = true;
                                    }
                                }

                            };

                        }
                    }


                }

            }
            catch (Exception ex)
            {
                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);
            }
        }

        #endregion
    }
}