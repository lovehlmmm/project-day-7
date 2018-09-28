using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class PredicateExtension
    {
        public static Expression<T> Compose<T>(this Expression<T> leftExpression, Expression<T> rightExpression, Func<Expression, Expression, Expression> merge)
        {
            var map = leftExpression.Parameters.Select((left, i) => new
            {
                left,
                right = rightExpression.Parameters[i]
            }).ToDictionary(p => p.right, p => p.left);

            var rightBody = ExpressionRebinder.ReplacementExpression(map, rightExpression.Body);

            return Expression.Lambda<T>(merge(leftExpression.Body, rightBody), leftExpression.Parameters);
        }

        /// <summary>
        /// Performs an "AND" operation
        /// </summary>
        /// <typeparam name="T">Param Type</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Returns the expression</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.And);
        }

        /// <summary>
        /// Performs an "OR" operation
        /// </summary>
        /// <typeparam name="T">Param Type</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Returns the expression</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.Or);
        }
    }
}
