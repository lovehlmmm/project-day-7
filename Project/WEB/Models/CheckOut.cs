using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class CheckOut
    {
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public CreditCard CreditCard { get; set; }

    }
}