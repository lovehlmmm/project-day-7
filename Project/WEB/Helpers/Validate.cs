using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WEB.Helpers
{
    public static class Validate
    {
        public static bool ValidPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasChars = new Regex(@".{5,10}");
            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasChars.IsMatch(password);
        }
    }
}