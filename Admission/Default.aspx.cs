using Admission.App_Start;
using CommonUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission
{
    public partial class _Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{0}/Admission/Home.aspx", AppPath.ApplicationPath));
        }
    }
}