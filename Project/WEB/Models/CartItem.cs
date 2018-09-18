using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities;

namespace WEB.Models
{
    public class CartItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public string Image { get; set; }
    }
}