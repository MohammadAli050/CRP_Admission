using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class GeneralInstructions : System.Web.UI.Page
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
            using(var db = new OfficeDataManager())
            {
                List<DAL.GeneralInstruction> list = db.AdmissionDB.GeneralInstructions.Where(c=>c.IsActive == true).ToList();
                if(list != null)
                {
                    lvInsR.DataSource = list;
                }
                else
                {
                    lvInsR.DataSource = null;
                }
                lvInsR.DataBind();
            }
        }

    }
}