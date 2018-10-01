using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Areas.Admin.Models
{
    public class EmailAppModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Stmp { get; set; }
    }
}