using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TransactionRepository
    {
        public static bool GenericSafeTransaction<T>(Action<T> action) where T : DbContext, new()
        {
            using (var context = new T())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        action(context);
                        dbContextTransaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        // Log Error
                        return false;
                    }
                }
            }
        }
    }
}
