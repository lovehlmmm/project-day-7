using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Helpers
{
    public class SessionHelper
    {
        public static bool IsExist(string sessionName)
        {
            
            return HttpContext.Current.Session != null && HttpContext.Current.Session[sessionName] != null;
        }

        public static void SetSession(object session,string sessionName)
        {
            HttpContext.Current.Session[sessionName] = session;
        }

        public static object GetSession(string sessionName)
        {
            var session = HttpContext.Current.Session[sessionName];
            return session;
        }
        public static void Delete(string sessionName)
        {
            HttpContext.Current.Session.Remove(sessionName);
        }
    }
}
