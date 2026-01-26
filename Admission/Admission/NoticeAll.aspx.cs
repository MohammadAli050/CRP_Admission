using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class NoticeAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNoticeListView();
            }
        }

        private void LoadNoticeListView()
        {
            using (var db = new OfficeDataManager())
            {
                List<DAL.Notice> list = db.GetAllNotice().Where(c => c.IsActive == true).OrderByDescending(a => a.NoticeDate).ToList();
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

                Label lblNewNotice = (Label)currentItem.FindControl("lblNewNotice");
                Label lblNoticeDate = (Label)currentItem.FindControl("lblNoticeDate");
                Label lblNoticeTitle = (Label)currentItem.FindControl("lblNoticeTitle");
                Label lblNoticeDetails = (Label)currentItem.FindControl("lblNoticeDetails");
                HyperLink hrefExternalUrl = (HyperLink)currentItem.FindControl("hrefExternalUrl");

                //LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                //LinkButton lnkDelete = (LinkButton)currentItem.FindControl("lnkDelete");

                //DateTime todayDate = DateTime.Today;
                //DateTime endDate = Convert.ToDateTime(notice.CreatedDate);

                //int noOfDays = Convert.ToInt32((endDate - todayDate).TotalDays);

                //if(noOfDays > 0 && noOfDays < 8)
                //{
                //    lblNewNotice.Visible = true;
                //}
                //else
                //{
                //    lblNewNotice.Visible = false;
                //}
                lblNewNotice.Visible = false;

                lblNoticeDate.Text = Convert.ToDateTime(notice.NoticeDate).ToString("dd/MM/yyyy");
                lblNoticeTitle.Text = notice.NoticeTitle;

                if (!string.IsNullOrEmpty(notice.NoticeDetails))
                {
                    lblNoticeDetails.Text = notice.NoticeDetails;
                    lblNoticeDetails.Visible = true;

                    lblNoticeTitle.Font.Bold = true;
                }
                else
                {
                    lblNoticeDetails.Text = string.Empty;
                    lblNoticeDetails.Visible = false;

                    lblNoticeTitle.Font.Bold = false;
                }

                if (!string.IsNullOrEmpty(notice.ID.ToString()))
                {
                    hrefExternalUrl.Visible = true;
                    hrefExternalUrl.NavigateUrl = notice.EID;
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

        protected void lvNotice_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            lvDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            LoadNoticeListView();
        }
    }
}