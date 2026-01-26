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
    public partial class TempAdmin : PageBase
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
                

                lblMessage.Text = "  ";
                messagePanel.CssClass = "alert alert-success";
                messagePanel.Visible = true;

                lblMessage.Text = "  ";
                messagePanel.CssClass = "alert alert-danger";
                messagePanel.Visible = true;
            }
        }

    }
}