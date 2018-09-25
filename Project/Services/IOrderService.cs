﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderService
    {
        bool TransactionPayment(Order order, IEnumerable<OrderDetail> orderDetails, BankCreditCard bankCreditCard);
    }
}
