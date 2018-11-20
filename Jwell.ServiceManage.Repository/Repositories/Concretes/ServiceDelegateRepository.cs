using System.Collections.Generic;
using System.Data;
using Jwell.Modules.EntityFramework.Repositories;
using Jwell.Modules.EntityFramework.Uow;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;
using Jwell.ServiceManage.Repository.Context;
using Jwell.ServiceManage.Repository.Repositories.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace Jwell.ServiceManage.Repository.Repositories.Concretes
{
    public class ServiceDelegateRepository : RepositoryBase<ServiceDelegate, JwellDbContext, long>,IServiceDelegateRepository
    {
        public ServiceDelegateRepository(IDbContextResolver dbContextResolver) : base(dbContextResolver)
        {
        }     
    }
}