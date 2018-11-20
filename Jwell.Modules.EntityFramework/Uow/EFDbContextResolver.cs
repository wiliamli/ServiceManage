using Jwell.Framework.Domain.Uow;
using Jwell.Framework.Ioc;
using System;
using System.Data.Entity;

namespace Jwell.Modules.EntityFramework.Uow
{
    [Singleton]
    public class EFDbContextResolver : IDbContextResolver
    {
        private readonly ICurrentUnitOfWork _currentUnitOfWork;

        public EFDbContextResolver(ICurrentUnitOfWork currentUnitOfWork)
        {
            _currentUnitOfWork = currentUnitOfWork;
        }    

        public T ResolveDbContext<T>() where T : DbContext
        {
            IUnitOfWork uow = _currentUnitOfWork.Current;

            if (uow == null)
            {
                throw new InvalidOperationException("Please call the method in a UnitOfWork scope");
            }
            if (!(uow is EFUnitOfWork))
            {
                throw new InvalidOperationException("The current UnitOfWork is not an instance of EFUnitOfWork");
            }

            return ((EFUnitOfWork)uow).GetOrCreateContext<T>();
        }      
    }
}
