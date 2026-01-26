using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CommonUtility
{
    public class AppPath : System.Web.UI.Page
    {
        public static string ApplicationPath
        {
            get
            {
                string path = string.Empty;
                path = HttpContext.Current.Request.ApplicationPath + string.Empty;
                path = path.Length > 1 ? path.ToLower() : string.Empty;
                return path;
            }
        }

        public static string BaseUrl
        {
            get
            {
                return HttpContext.Current.Request.ApplicationPath.ToLower();
            }
        }
        public static string FullBaseUrl
        {
            get
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri.Replace(
                  HttpContext.Current.Request.Url.PathAndQuery, "") + BaseUrl.ToLower();
                if (url.EndsWith("/"))
                    url = url.Substring(0, url.Length - 1);

                return url;
            }
        }
        public static string BaseFullUrl
        {
            get
            {
                string url = (HttpContext.Current.Request.Url.AbsoluteUri.Replace(
                   HttpContext.Current.Request.Url.PathAndQuery, "") + HttpContext.Current.Request.ApplicationPath).ToLower();
                if (url.EndsWith("/"))
                    url = url.Substring(0, url.Length - 1);
                return url;
            }
        }
        public static bool IsLocalhost
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("localhost") ? true : false;
            }
        }
    }
}
