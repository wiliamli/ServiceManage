using Jwell.Framework.Mvc;
using Jwell.Framework.Paging;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Application.Services.Interfaces;
using Jwell.ServiceManage.Application.Services.Params;
using System.Collections.Generic;
using System.Web.Http;

namespace Jwell.ServiceManage.Web.Controllers
{
    public class ServiceController:BaseApiController
    {
        private readonly IJwellService _jwellService;

        public ServiceController(IJwellService jwellService)
        {
            _jwellService = jwellService;
        }      

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<bool> Register(RegisterDto dto)
        {
            return StandardAction(() => _jwellService.Register(dto));
        }

        /// <summary>
        /// 获取所有服务信息,用于王吉伟那边同步
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<IEnumerable<ServiceDto>> GetAllServiceInfo()
        {
            return StandardAction(() => _jwellService.GetAllServiceInfo());
        }

        /// <summary>
        /// 分页获取服务
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<PageResult<ServiceDto>> GetServiceByPage(PageParam pageParam)
        {
            return StandardAction(() => _jwellService.GetServiceByPage(pageParam));
        }

        /// <summary>
        /// 根据服务标识获取版本
        /// </summary>
        /// <param name="sign">服务标识</param>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<List<ServiceVersionDto>> GetVersionsBySign(string sign)
        {
            return StandardAction(() => _jwellService.GetVersionsBySign(sign));
        }

        /// <summary>
        /// 根据服务标识获取版本和接口基本信息
        /// </summary>
        /// <param name="sign">服务标识</param>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<Dictionary<string, Dictionary<string, List<string>>>> GetActionInfoBySign(string sign)
        {
            return StandardAction(() => _jwellService.GetActionInfoBySign(sign));
        }

        /// <summary>
        /// 获取接口详细信息
        /// </summary>
        /// <remarks><paramref name="dto"/> 参数</remarks>
        /// <param name="dto">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<List<ActionDetailInfoDto>> GetActionDetailInfo(ActionDetailParaDto dto)
        {
            return StandardAction(() => _jwellService.GetActionDetailInfo(dto));
        }

        /// <summary>
        /// 根据服务拥有人获取服务信息
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<PageResult<ManageInfoDto>> GetServiceInfoByOwner(SearchManageInfoParam page)
        {
            return StandardAction(() => _jwellService.GetServiceInfos(page));
        }

        /// <summary>
        /// 根据服务标识获取协同管理人员信息
        /// </summary>
        /// <param name="sign">服务标识</param>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<List<ManagerDto>> GetManagerInfoBySign(string sign)
        {
            return StandardAction(() => _jwellService.GetManagerInfoBySign(sign));
        }

        /// <summary>
        /// 根据服务标识获取授权信息
        /// </summary>
        /// <param name="sign">服务标识</param>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<List<AuthInfoDto>> GetAuthInfoBySign(string sign)
        {
            return StandardAction(() => _jwellService.GetAuthInfoBySign(sign));
        }

       /// <summary>
       /// 添加服务授权
       /// </summary>
       /// <param name="authAddParaDto"></param>
       /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<AuthInfoDto> Authorize(AuthAddParaDto authAddParaDto)
        {
            return StandardAction(() => _jwellService.Authorize(authAddParaDto));
        }

        /// <summary>
        /// 设置授权状态
        /// </summary>
        /// <param name="authStateParaDto"></param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<bool> SetAuthorityState(AuthStateParaDto authStateParaDto)
        {
            return StandardAction(() => _jwellService.SetAuthState(authStateParaDto));
        }

        /// <summary>
        /// 根据工号或者姓名获取协助管理人信息
        /// </summary>
        /// <param name="no"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<List<ManagerDto>> GetManagerByNameOrNo(string no = "", string name = "")
        {
            return StandardAction(() => _jwellService.GetManagerInfoByNameOrNo(no, name));
        }

        /// <summary>
        /// 添加服务协助管理人员
        /// </summary>
        /// <param name="sign">协助管理的服务</param>
        /// <param name="employeeId">管理人员的工号</param>
        /// <returns></returns>
        [HttpGet]
        public StandardJsonResult<bool> AddServiceManager(string sign,string employeeId)
        {
            return StandardAction(() => _jwellService.AddServiceManager(sign, employeeId));
        }

        /// <summary>
        /// 根据服务标识获取其调用记录
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<PageResult<InvokeRecordDto>> GetInvokeRecordsBySign(SearchInvokeInfoParam pageParam)
        {
            return StandardAction(() => _jwellService.GetInvokeRecordsBySign(pageParam));
        }

        /// <summary>
        /// 根据服务标识、版本号和控制器名称和接口名称获取请求信息
        /// </summary>
        /// <param name="serviceSign">服务标识</param>
        /// <param name="versionNumber">版本号</param>
        /// <param name="url">API访问路由</param>
        /// <returns></returns>
        [HttpGet]
        public RequestInfoDto GetRequestInfo(string svid)
        {
            return _jwellService.GetRequestInfo(svid);
        }


        /// <summary>
        /// 更新调用记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<bool> UpdateInvokeRecord(InvokeRecordDto dto)
        {
            return StandardAction(() => _jwellService.UpdateInvokeRecord(dto));
        }

        /// <summary>
        /// 分页获取员工信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public StandardJsonResult<PageResult<EmployeeInfoDto>> GetEmployeeInfoByPage(PageParam pageParam)
        {
            return StandardAction(() => _jwellService.GetEmployeeInfoByPage(pageParam));
        }
    }
}