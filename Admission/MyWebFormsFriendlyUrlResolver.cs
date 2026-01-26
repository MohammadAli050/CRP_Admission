using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Admission
{
    public class MyWebFormsFriendlyUrlResolver : Microsoft.AspNet.FriendlyUrls.Resolvers.WebFormsFriendlyUrlResolver
    {

        //Disable the site mobile master
        //Reference Link: https://stackoverflow.com/questions/23081059/disable-aspnet-friendlyurls-mobile-redirect-for-tablets
        protected override bool TrySetMobileMasterPage(HttpContextBase httpContext, Page page, String mobileSuffix)
        {
            if (mobileSuffix == "Mobile")
            {
                return false;
            }
            else
            {
                return base.TrySetMobileMasterPage(httpContext, page, mobileSuffix);
            }
        }
    }
}