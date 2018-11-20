using System.Collections.Generic;
using System.Web.Http;
using Jwell.Framework.Mvc;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Application.Services.Interfaces;

namespace Jwell.ServiceManage.Web.Controllers
{
    public class TasksController : BaseApiController
    {
        private readonly IEmployeeInfoService EmployeeInfoService;
    
        public TasksController(IEmployeeInfoService employeeInfoService)
        {
            EmployeeInfoService = employeeInfoService;
        }


        /// <summary>
        /// 初始化员工信息
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public StandardJsonResult<IEnumerable<EmployeeInfoDto>> EmployeeInit()
        //{
        //    return StandardAction(() => EmployeeInfoService.EmployeeInfoInit());
        //}
    }
}