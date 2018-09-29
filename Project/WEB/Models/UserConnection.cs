using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class UserConnection
    {
        public string UserName { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}