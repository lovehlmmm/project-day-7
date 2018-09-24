using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class BankContext: DbContext
    {
        public BankContext()
            : base("name=BankConnectionStringServer")
        {

        }
    }
}
