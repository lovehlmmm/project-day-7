﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services
{
    public class OrderService : IOrderService
    {
        public OrderService()
        {
        }
        public bool TransactionPayment(Order order, IEnumerable<OrderDetail> orderDetails, BankCreditCard bankCreditCard)
        {
            try
            {

                using (var bankContext = new BankContext())
                {
                    using (var transaction = bankContext.Database.BeginTransaction())
                    {
                        try
                        {
                            bankContext.Entry(bankCreditCard).State = System.Data.Entity.EntityState.Modified;
                            bankContext.SaveChanges();
                            using (var pOPContext = new POPContext())
                            {
                                pOPContext.Orders.Add(order);
                                pOPContext.SaveChanges();
                                foreach (var item in orderDetails)
                                {
                                    item.OrderId = order.OrderId;
                                    pOPContext.OrderDetails.Add(item);
                                }
                                pOPContext.OrderDetails.AddRange(orderDetails);
                                //Save and discard changes
                                pOPContext.SaveChanges();
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }
    }
}