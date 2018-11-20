using System.Collections.Generic;
using Jwell.Framework.Application.Service;
using Jwell.ServiceManage.Application.Services.Dtos;

namespace Jwell.ServiceManage.Application.Services.Interfaces
{
    public interface IEmployeeInfoService: IApplicationService
    {
        IEnumerable<EmployeeInfoDto> EmployeeInfoInit();
    }
}
