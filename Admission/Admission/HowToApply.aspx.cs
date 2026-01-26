using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class HowToApply : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadListView();
            }
        }

        private void LoadListView()
        {
            List<DAL.HowToApply> list = null;
            using(var db = new OfficeDataManager())
            {
                list = db.AdmissionDB.HowToApplies.Where(c => c.IsActive == true).ToList();
            }
            if (list.Count() > 0)
            {
                lvInsHta.DataSource = list;
            }
            else
            {
                lvInsHta.DataSource = null;
            }
            lvInsHta.DataBind();
        }
    }
}