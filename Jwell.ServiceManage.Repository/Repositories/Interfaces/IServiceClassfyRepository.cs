using Jwell.Framework.Domain.Repositories;
using Jwell.Modules.EntityFramework.Repositories;
using Jwell.ServiceManage.Domain.Entities;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;
using Jwell.ServiceManage.Repository.Context;

namespace Jwell.ServiceManage.Repository.Repositories.Interfaces
{
    public interface IServiceClassfyRepository: IRepository<ServiceClassfication, long>
    {
        
    }
}