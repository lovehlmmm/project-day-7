using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
namespace Services
{
    public class BankService : IBankService
    {
        private readonly BankContext _bankContext;
        public BankService()
        {
            _bankContext = new BankContext();
        }
        public bool CheckCard(string number, string expire, string cvc)
        {
            var check=_bankContext.BankCreditCards.SingleOrDefault(c => c.CVC.Equals(cvc) & c.Expire.Equals(expire) & c.CreditNumber.Equals(number));
            return check != null;
        }

        public bool Transaction(string number,decimal amount)
        {
            var check = _bankContext.BankCreditCards.SingleOrDefault(c => c.CreditNumber.Equals(number));
            if (check!=null)
            {
                if (check.Balance>amount)
                {
                    check.Balance -= amount;
                    _bankContext.Entry(check).State = System.Data.Entity.EntityState.Modified;
                    var result = _bankContext.SaveChanges()>0;
                    if (result)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
