using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities;

namespace WEB.Models
{
    public class CartItem
    {
        public string ImageTitle { get; set; }
        public int Quantity { get; set; }
        public int Option { get; set; }
        public Product Product { get; set; }
        public string Image { get; set; }
        public string ImageType { get; set; }
        public Material Material { get; set; }
    }
}