using CommonUtility;
using DAL.ViewModels.EkPayModel;
using DATAMANAGER;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new GeneralDataManager())
            {
                int id = 1;
                var institute = db.AdmissionDB.Institutes.Find(id);
                if (institute != null) //
                {
                    lblInsName.Text = institute.Name + " (" + institute.ShortName + ")"; //"Bangladesh University of Professionals" + "(BUP)"; //
                    lblInsAddress.Text = institute.Address + "-" + institute.PostCode; //"Mirpur Cantonment, Dhaka - 1216"; //
                    lblInsTel1.Text = institute.Telephone1 + ", " + institute.Telephone2; //"88 - 02 - 8000368,  PABX 8000261-4"; //
                    lblInsMobile.Text = institute.Mobile; //""; //
                    lblInsFax.Text = institute.Fax; //"8000443"; //
                }
                else
                {
                    lblInsName.Text = string.Empty;
                    lblInsAddress.Text = string.Empty;
                    lblInsTel1.Text = string.Empty;
                    lblInsMobile.Text = string.Empty;
                    lblInsFax.Text = string.Empty;
                }
            }

            if (!IsPostBack)
            {
                LoadNoticeListView();
                LoadImportantNoticeListView();
            }

            int i = 0;
            int j = 0;
            j = i++;


            //CheckEkPay();

            string year = DateTime.Now.ToString("ddMMyyyyHHmmss");
        }


        private void CheckEkPay()
        {
        }



        private void LoadImportantNoticeListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.Notice> list = db.AdmissionDB.Notices.Where(c => c.NoticeType == 2 && c.IsActive == true).ToList();
                if (list != null && list.Count > 0)
                {
                    panelImportantNotice.Visible = true;
                    lvImportantNotices.DataSource = list;
                }
                else
                {
                    panelImportantNotice.Visible = false;
                    lvImportantNotices.DataSource = null;
                }
                lvImportantNotices.DataBind();
            }
        }

        protected void lvImportantNotices_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Notice notice = (DAL.Notice)((ListViewDataItem)(e.Item)).DataItem;

                Label lblNoticeTitle = (Label)currentItem.FindControl("lblNoticeTitle");
                Label lblNoticeDetails = (Label)currentItem.FindControl("lblNoticeDetails");

                lblNoticeTitle.Text = notice.NoticeTitle;
                lblNoticeDetails.Text = notice.NoticeDetails;

            }
        }


        private void LoadNoticeListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.Notice> list = db.AdmissionDB.Notices.Where(c => c.NoticeType == 1 && c.IsActive == true).OrderByDescending(a => a.NoticeDate).Take(20).ToList();
                if (list.Any() && list != null)
                {
                    lvNotice.DataSource = list;
                }
                else
                {
                    lvNotice.DataSource = null;
                }
                lvNotice.DataBind();
            }
        }

        protected void lvNotice_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.Notice notice = (DAL.Notice)((ListViewDataItem)(e.Item)).DataItem;

                LinkButton lbNoticeTitle = (LinkButton)currentItem.FindControl("lbNoticeTitle");
                HyperLink hrefExternalUrl = (HyperLink)currentItem.FindControl("hrefExternalUrl");

                //LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                //LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");


                lbNoticeTitle.Text = notice.NoticeTitle;
                lbNoticeTitle.CommandName = "ViewNotice";
                lbNoticeTitle.CommandArgument = notice.ID.ToString();

                if (!string.IsNullOrEmpty(notice.Attachment))
                {
                    hrefExternalUrl.Visible = true;
                    hrefExternalUrl.NavigateUrl = notice.Attachment;
                    hrefExternalUrl.Text = "Download";
                }
                else
                {
                    hrefExternalUrl.Visible = false;
                    hrefExternalUrl.NavigateUrl = string.Empty;
                }

            }
        }

        protected void lvNotice_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }
    }
}