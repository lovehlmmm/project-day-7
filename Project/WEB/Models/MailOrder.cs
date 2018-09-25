using Entities;
using System.Collections.Generic;
namespace WEB.Models
{
    public class MailOrder
    {
        public MailOrder(List<CartItem> orderItem, long orderId, string mail, string address, string name, string creditCard, string folder, decimal total,string phone)
        {
            OrderId = orderId;
            OrderItem = orderItem;
            Mail = mail;
            Address = address;
            Name = name;
            CreditCard = creditCard;
            Folder = folder;
            Total = total;
            Phone = phone;
        }
        public List<CartItem> OrderItem { get; set; }
        public long OrderId { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string CreditCard { get; set; }
        public string Folder { get; set; }
        public decimal Total { get; set; }
    }
}