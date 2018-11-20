using Jwell.Framework.Application.Service;

namespace Jwell.ServiceManage.Application.Services.Interfaces
{
    public interface IGateWayCacheSyncService : IApplicationService
    {
        bool GatewayCacheInfoSync();

        bool GatewayInvokeCacheInfoSync();
    }

}