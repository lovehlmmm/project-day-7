﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBankService
    {
        BankCreditCard CheckCard(string number, string expire, string cvc);
        bool Transaction(string number,decimal amount);
    }
}
