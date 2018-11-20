using Jwell.Modules.EntityFramework.Repositories;
using Jwell.Modules.EntityFramework.Uow;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;
using Jwell.ServiceManage.Repository.Context;
using Jwell.ServiceManage.Repository.Repositories.Interfaces;

namespace Jwell.ServiceManage.Repository.Repositories.Concretes
{
    public class ServiceAuthorityRepository:RepositoryBase<ServiceAuthority,JwellDbContext,long>,IServiceAuthorityRepository
    {
        public ServiceAuthorityRepository(IDbContextResolver dbContextResolver) : base(dbContextResolver)
        {
        }
    }
}