using CommonUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admission.App_Start
{
    public class PageBase : System.Web.UI.Page
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (!this.Page.Request.RawUrl.ToLower().Contains(".aspx"))
            //    Response.Redirect(string.Format("{0}/Admission/Home.aspx", AppPath.ApplicationPath));

            SessionSGD.SaveObjToSession<string>(Request.AppRelativeCurrentExecutionFilePath.ToString(), SessionName.Common_RedirectPage);

            if (!IsAuthenticate())
            {
                Response.Redirect(string.Format("{0}/Admission/Login.aspx", AppPath.ApplicationPath));
            }
        }

        public bool IsAuthenticate()
        {
            return SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId) != 0 ? true : false;
        }

    }
}