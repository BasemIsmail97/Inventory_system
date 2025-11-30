using Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class BaseSpecifcation<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        protected  BaseSpecifcation(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }

        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }

        #region Sortong
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
        #endregion

        #region Pagination
        public int? Take { get; private set; }

        public int? Skip { get; private set; }

        public bool IsPagination { get; private set; }
        protected void ApplyPagination(int PageIndex, int PageSize)
        {
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
            IsPagination = true;
        }
        #endregion
    }
}
