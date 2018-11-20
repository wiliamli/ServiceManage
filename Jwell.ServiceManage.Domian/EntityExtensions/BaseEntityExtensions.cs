﻿using System.Collections.Generic;
using System.Linq;
using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.EntityExtensions
{
    public static class BaseEntityExtensions
    {
        public static T Get<T>(this IQueryable<T> query, long id) where T : BaseEntity
        {
            return query.FirstOrDefault(x => x.ID == id);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, IEnumerable<long> ids) where T : BaseEntity
        {
            return query.Where(x => ids.Contains(x.ID));
        }
    }
}
