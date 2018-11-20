using Jwell.Framework.Mvc;
using Jwell.ServiceManage.Application.Services.Interfaces;
using System.Web.Http;

namespace Jwell.ServiceManage.Web.Controllers
{
    public class CacheSyncController : BaseApiController
    {
        private readonly IGateWayCacheSyncService _gateWayCacheSyncService;

        public CacheSyncController(IGateWayCacheSyncService gateWayCacheSyncService)
        {
            _gateWayCacheSyncService = gateWayCacheSyncService;
        }

        /// <summary>
        /// 初始化网关缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<bool> GatewayCacheInfoSync()
        {
            return StandardAction(() => _gateWayCacheSyncService.GatewayCacheInfoSync());
        }

        /// <summary>
        /// 服务API调用记录缓存初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<bool> GatewayInvokeCacheInfoSync()
        {
            return StandardAction(() => _gateWayCacheSyncService.GatewayInvokeCacheInfoSync());
        }
    }
}