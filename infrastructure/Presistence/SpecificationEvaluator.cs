using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence
{
    public class SpecificationEvaluator 
    {
        public static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec) where TEntity : class
        {
            var query = inputQuery;
            #region Criteria
            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }
            #endregion
            #region Sorting
            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            #endregion
            #region Includes
            if (spec.IncludeExpressions is not null && spec.IncludeExpressions.Count > 0)
            {
                query = spec.IncludeExpressions.Aggregate(query, (current, include) => current.Include(include));
            }
            #endregion
            #region Pagination

            if (spec.IsPagination)
            {
                if (spec.Skip.HasValue)
                {
                    query = query.Skip(spec.Skip.Value);
                }
                if (spec.Take.HasValue)
                {
                    query = query.Take(spec.Take.Value);
                }
            }

            #endregion
            return query;

        }
    }
}
