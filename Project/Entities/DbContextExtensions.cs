using Repositories.SqlServerNotifier;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public static class DbContextExtensions
    {
        public static ObjectContext UnderlyingContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }

        public static NotifierEntity GetNotifierEntity<TEntity>(this DbContext dbContext, IQueryable iQueryable) where TEntity : EntityBase
        {
            var objectQuery = dbContext.GetObjectQuery<TEntity>(iQueryable);
            return new NotifierEntity()
            {
                SqlQuery = objectQuery.ToTraceString(),
                SqlConnectionString = objectQuery.SqlConnectionString(),
                SqlParameters = objectQuery.SqlParameters()
            };
        }

        public static ObjectQuery GetObjectQuery<TEntity>(this DbContext dbContext, IQueryable query) where TEntity : EntityBase
        {
            if (query is ObjectQuery)
                return query as ObjectQuery;

            if (dbContext == null)
                throw new ArgumentException("dbContext cannot be null");

            var objectSet = dbContext.UnderlyingContext().CreateObjectSet<TEntity>();
            var iQueryProvider = ((IQueryable)objectSet).Provider;

            // Use the provider and expression to create the ObjectQuery.
            return (ObjectQuery)iQueryProvider.CreateQuery(query.Expression);
        }
    }
}
