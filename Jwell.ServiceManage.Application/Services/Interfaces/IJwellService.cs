using System.Collections.Generic;
using Jwell.Framework.Application.Service;
using Jwell.Framework.Paging;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Application.Services.Params;

namespace Jwell.ServiceManage.Application.Services.Interfaces
{
    public interface IJwellService: IApplicationService
    {
        bool Register(RegisterDto dto);
       
        int AddService(ServiceDto service);

        object TryRequest(InvokeParaDto dto);

        List<ServiceVersionDto> GetVersionsBySign(string sign);

        List<ManagerDto> GetManagerInfoBySign(string sign);

        Dictionary<string, Dictionary<string, List<string>>> GetActionInfoBySign(string sign);

        List<ActionDetailInfoDto> GetActionDetailInfo(ActionDetailParaDto dto);
        
        PageResult<ServiceDto> GetServiceByPage(PageParam pageParam);

        PageResult<ServiceDto> GetServices(SearchServiceParam page);

        PageResult<ServiceVersionDto> GetVersions(SearchVersionParam page);

        PageResult<ManageInfoDto> GetServiceInfos(SearchManageInfoParam pageParam);

        List<AuthInfoDto> GetAuthInfoBySign(string sign);

        AuthInfoDto Authorize(AuthAddParaDto dto);

        bool SetAuthState(AuthStateParaDto dto);

        bool AddServiceManager(string sign, string employeeId);

        List<ManagerDto> GetManagerInfoByNameOrNo(string no, string name);

        PageResult<InvokeRecordDto> GetInvokeRecordsBySign(SearchInvokeInfoParam pageParam);

        RequestInfoDto GetRequestInfo(string svid);

        bool UpdateInvokeRecord(InvokeRecordDto dto);

        PageResult<EmployeeInfoDto> GetEmployeeInfoByPage(PageParam pageParam);

        IEnumerable<ServiceDto> GetAllServiceInfo();
    }
}