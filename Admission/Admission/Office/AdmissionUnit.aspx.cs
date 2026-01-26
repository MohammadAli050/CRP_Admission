using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using DATAMANAGER;
using System.Drawing;
using CommonUtility;
using Admission.App_Start;

namespace Admission.Admission.Office
{
    public partial class AdmissionUnit : PageBase
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
                messagePanel.Visible = false;
                lblMessage.Text = string.Empty;
                LoadListView();
            }
        }

        private void LoadListView()
        {
            using (var db = new GeneralDataManager())
            {
                var list = db.AdmissionDB.AdmissionUnits.OrderByDescending(a => a.UnitName).ToList();

                list = list.OrderBy(x => x.UnitCode1).OrderBy(y=> y.UnitCode2).ToList();
                lvAdmissionUnit.DataSource = list.ToList();
                lvAdmissionUnit.DataBind();
                lblCount.Text = list.Count().ToString();
            }
        }

        private void Clear()
        {
            txtUnitCode1.Text = string.Empty;
            txtUnitCode2.Text = string.Empty;
            txtUnitName.Text = string.Empty;
            txtShortName.Text = string.Empty;
            ckbxIsActive.Checked = false;
            chkboxForeign.Checked = false;
            CurrentAdmissionUnitID = 0;

            lblMessage.Text = string.Empty;
            messagePanel.Visible = false;
        }

        public long CurrentAdmissionUnitID
        {
            get
            {
                if (ViewState["CurrentAdmissionUnitID"] == null)
                    return 0;
                else
                    return Convert.ToInt64(ViewState["CurrentAdmissionUnitID"].ToString());
            }
            set
            {
                ViewState["CurrentAdmissionUnitID"] = value;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long id = -1;
            DAL.AdmissionUnit obj = new DAL.AdmissionUnit();

            obj.Attribute1 = txtShortName.Text; //unit short name in Attribute1
            obj.Attribute2 = null;
            obj.IsActive = ckbxIsActive.Checked;
            obj.UnitCode1 = txtUnitCode1.Text;
            obj.UnitCode2 = txtUnitCode2.Text;
            obj.UnitName = txtUnitName.Text;
            if (chkboxForeign.Checked)
                obj.Attribute3 = "Active";
            else
                obj.Attribute3 = "InActive";

            obj.DateCreated = DateTime.Now;
            obj.CreatedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK
            obj.DateModified = null;
            obj.ModifiedBy = null;

            obj.ID = CurrentAdmissionUnitID;
            try
            {
                if (obj.ID > 0) //update
                {
                    using (var db = new GeneralDataManager())
                    {
                        obj.DateModified = DateTime.Now;
                        obj.ModifiedBy = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId);//PK

                        db.Update<DAL.AdmissionUnit>(obj);
                    }
                    lblMessage.Text = "Admission unit updated successfully.";
                    messagePanel.CssClass = "alert alert-success";
                    messagePanel.Visible = true;
                }
                else //create new
                {
                    using (var db = new GeneralDataManager())
                    {
                        db.Insert<DAL.AdmissionUnit>(obj);
                        id = obj.ID;
                        //List<DAL.SP_ProgramsGetAllFromUCAM_Result> obj1 = new List<SP_ProgramsGetAllFromUCAM_Result>();
                        //obj1 = db.AdmissionDB.SP_ProgramsGetAllFromUCAM().ToList();
                    }
                    if (id > 0)
                    {
                        lblMessage.Text = "Admission unit saved successfully.";
                        messagePanel.CssClass = "alert alert-success";
                        messagePanel.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unable to save/update data.";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
            btnSave.Text = "Save";
            Clear();
            LoadListView();
            //updatePanelListView.Update();
        }

        protected void lvAdmissionUnit_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.AdmissionUnit admissionUnit = (DAL.AdmissionUnit)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                Label lblUnitName = (Label)currentItem.FindControl("lblUnitName");
                Label lblShortName = (Label)currentItem.FindControl("lblShortName");
                Label lblUnitCode1 = (Label)currentItem.FindControl("lblUnitCode1");
                Label lblUnitCode2 = (Label)currentItem.FindControl("lblUnitCode2");
                Label lblIsActive = (Label)currentItem.FindControl("lblIsActive");


                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                lblSerial.Text = (e.Item.DisplayIndex + 1).ToString();
                lblUnitCode1.Text = admissionUnit.UnitCode1;//for payment id
                lblUnitCode2.Text = admissionUnit.UnitCode2;//for test roll generation
                lblUnitName.Text = admissionUnit.UnitName;
                lblShortName.Text = admissionUnit.Attribute1;//unit short name

                if (admissionUnit.IsActive == true)
                {
                    lblIsActive.Text = "✓";
                    lblIsActive.ForeColor = Color.Green;
                }
                else
                {
                    lblIsActive.Text = "✕";
                    lblIsActive.Font.Bold = true;
                    lblIsActive.ForeColor = Color.Crimson;
                }


                lnkEdit.CommandName = "Update";
                lnkEdit.CommandArgument = admissionUnit.ID.ToString();

                lnkDelete.CommandName = "Delete";
                lnkDelete.CommandArgument = admissionUnit.ID.ToString();
            }
        }

        protected void lvAdmissionUnit_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //DAL.AdmissionUnit admissionUnit = new DAL.AdmissionUnit();
            if (e.CommandName == "Delete")
            {
                try
                {
                    using (var db = new GeneralDataManager())
                    {
                        var objectToDelete = db.AdmissionDB.AdmissionUnits.Find(Convert.ToInt64(e.CommandArgument));
                        db.Delete<DAL.AdmissionUnit>(objectToDelete);
                        CurrentAdmissionUnitID = 0;
                    }
                    lblMessage.Text = "Admission unit deleted successfully.";
                    messagePanel.CssClass = "alert alert-warning";
                    messagePanel.Visible = true;
                }
                catch(Exception ex)
                {
                    lblMessage.Text = "Unable to delete.";
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                LoadListView();
            }
            else if (e.CommandName == "Update")
            {
                btnSave.Text = "Update";
                using (var db = new GeneralDataManager())
                {
                    var objectToUpdate = db.AdmissionDB.AdmissionUnits.Find(Convert.ToInt64(e.CommandArgument));
                    Clear();
                    txtUnitCode1.Text = objectToUpdate.UnitCode1;
                    txtUnitCode2.Text = objectToUpdate.UnitCode2;
                    txtUnitName.Text = objectToUpdate.UnitName;
                    txtShortName.Text = objectToUpdate.Attribute1;
                    if (objectToUpdate.IsActive == true)
                    {
                        ckbxIsActive.Checked = true;
                    }
                    else
                    {
                        ckbxIsActive.Checked = false;
                    }
                    if (objectToUpdate.Attribute3 != null && objectToUpdate.Attribute3.ToLower()=="active")
                    {
                        chkboxForeign.Checked = true;
                    }
                    else
                    {
                        chkboxForeign.Checked = false;
                    }

                    CurrentAdmissionUnitID = objectToUpdate.ID;
                }
            }
            //LoadListView();
        }

        protected void lvAdmissionUnit_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvAdmissionUnit_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadListView();
        }
    }
}