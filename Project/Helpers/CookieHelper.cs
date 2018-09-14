using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Helpers
{
    public class CookieHelper
    {

        public static HttpCookie Create(string name, string value, DateTime expire)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Value =  value;
            cookie.Expires = expire;
            return cookie;
        }
    }
}
