using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Admission.Admission.Candidate
{
    public partial class SelectProgramPhd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }


        private void LoadData()
        {
            try
            {
                List<DAL.SPGetAllAdmissionSetupOtherPrograms_Result> list = null;
                using (var db = new OfficeDataManager())
                {
                    list = db.AdmissionDB.SPGetAllAdmissionSetupOtherPrograms(2, null, null).ToList();
                }

                if (list != null && list.Count > 0)
                {
                    lvAdmSetup.DataSource = list.OrderBy(x=> x.SerialNo).ToList();
                    lvAdmSetup.DataBind();
                }
                else
                {
                    lvAdmSetup.DataSource = null;
                    lvAdmSetup.DataBind();
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        protected void lvAdmSetup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.SPGetAllAdmissionSetupOtherPrograms_Result admSetup = (DAL.SPGetAllAdmissionSetupOtherPrograms_Result)((ListViewDataItem)(e.Item)).DataItem;

               
                Label lblTitle = (Label)currentItem.FindControl("lblTitle");

                HyperLink hlViewFile = (HyperLink)currentItem.FindControl("hlViewFile");

                lblTitle.Text = admSetup.ProgramDetailsName;

                if (admSetup.FileTypeId == 1 || admSetup.FileTypeId == 3)
                {
                    hlViewFile.Text = "View";
                    hlViewFile.NavigateUrl = admSetup.FileURL;
                    hlViewFile.Target = "_blank";
                    hlViewFile.CssClass = "btn btn-info";
                    hlViewFile.Attributes.CssStyle.Add("width", "125px");

                    hlViewFile.Visible = true;
                }
                else if (admSetup.FileTypeId == 2)
                {
                    hlViewFile.Text = "Form";
                    hlViewFile.NavigateUrl = admSetup.FileURL;
                    hlViewFile.Target = "_blank";
                    hlViewFile.CssClass = "btn btn-success";
                    hlViewFile.Attributes.CssStyle.Add("width", "125px");

                    hlViewFile.Visible = true;
                }
                else
                {
                    hlViewFile.Visible = false;
                }


                
            }
        }


    }
}