using System.Collections.Generic;
using Jwell.Framework.Domain.Entities;
using System.Linq;

namespace Jwell.Framework.Domain.Repositories
{
    public interface IRepository<T,TPrimaryKey> : IRepository where T : Entity<TPrimaryKey>
    {
        IQueryable<T> Queryable();

        int Add(T entity);

        int AddRange(IEnumerable<T> entities);

        int Update(T entity);

        int Delete(T entity);
    }
}
