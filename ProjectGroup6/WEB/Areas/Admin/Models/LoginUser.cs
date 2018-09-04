using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Areas.Admin.Models
{
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}