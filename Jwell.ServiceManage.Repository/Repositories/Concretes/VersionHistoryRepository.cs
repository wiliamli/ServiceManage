using Jwell.Modules.EntityFramework.Repositories;
using Jwell.Modules.EntityFramework.Uow;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;
using Jwell.ServiceManage.Repository.Context;
using Jwell.ServiceManage.Repository.Repositories.Interfaces;

namespace Jwell.ServiceManage.Repository.Repositories.Concretes
{
    public class VersionHistoryRepository: RepositoryBase<VersionHistory, JwellDbContext, long>, IVersionHistoryRepository
    {
        public VersionHistoryRepository(IDbContextResolver dbContextResolver) : base(dbContextResolver)
        {
        }
    }
}