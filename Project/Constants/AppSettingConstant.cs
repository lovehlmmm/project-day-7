using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class AppSettingConstant
    {
        public const string PasswordHash = "team6";
        public const int SaltLength = 8;
        public const string initVector = "team123456789987";
        public const string LoginSessionAdmin = "loggedAdmin";
        public const string LoginSessionCustomer = "loggedCustomer";
        public const string CartSession = "cartSession";
        public const string CheckOutSession = "checkOutSession";
        public const string LoginCookieCustomer = "loggedCustomer";
        public static List<string> Options = new List<string> { "Delete Background", "Increase Quality", "Blur The Background" };

        public const string NotifiSession = "notifiSession";
    }
}
