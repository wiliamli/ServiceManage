using Jwell.Framework.Mvc;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Application.Services.Interfaces;
using System.Web.Http;

namespace Jwell.ServiceManage.Web.Controllers
{
    public class RequestController : BaseApiController
    {
        private readonly IJwellService _jwellService;

        public RequestController(IJwellService jwellService)
        {
            _jwellService = jwellService;
        }

        /// <summary>
        /// 访问测试
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<object> TryRequest(InvokeParaDto request)
        {
            //return StandardAction(() => { return _jwellService.TryRequest(request);});
            return StandardAction(() => _jwellService.TryRequest(request));
        }
    }
}
