using Jwell.Modules.EntityFramework.Repositories;
using Jwell.Modules.EntityFramework.Uow;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;
using Jwell.ServiceManage.Repository.Context;
using Jwell.ServiceManage.Repository.Repositories.Interfaces;

namespace Jwell.ServiceManage.Repository.Repositories.Concretes
{
    public class ServiceInvokeRecordRepository: RepositoryBase<ServiceInvokeRecord, JwellDbContext, long>, IServiceInvokeRecordRepository
    {
        public ServiceInvokeRecordRepository(IDbContextResolver dbContextResolver) : base(dbContextResolver)
        {
        }
    }
}