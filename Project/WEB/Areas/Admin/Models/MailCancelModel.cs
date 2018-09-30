using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Areas.Admin.Models
{
    public class MailCancelModel
    {
        public Order Order { get; set; }
        public string Reason { get; set; }
    }
}