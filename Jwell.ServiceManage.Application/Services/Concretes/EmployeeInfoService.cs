using System;
using System.Collections.Generic;
using Jwell.Framework.Application.Service;
using Jwell.Modules.Cache;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Application.Services.Interfaces;
using Jwell.ServiceManage.Repository.Repositories.Interfaces;

namespace Jwell.ServiceManage.Application.Services.Concretes
{
    public class EmployeeInfoService : ApplicationService, IEmployeeInfoService
    {
        private readonly IEmployeeInfoRepository _repository;

        private readonly ICacheClient _cacheClient;

        public EmployeeInfoService(IEmployeeInfoRepository repository,ICacheClient cacheClient)
        {
            _repository = repository;
            _cacheClient = cacheClient;
        }

        public IEnumerable<EmployeeInfoDto> EmployeeInfoInit()
        {
            IEnumerable<EmployeeInfoDto> employees;

            try
            {
                _cacheClient.DB = ApplicationConstant.EMPLOYEECACHEDB;

                if (_cacheClient.IsExist(ApplicationConstant.EMPLOYEEKEY))
                {
                    employees = _cacheClient.GetCache<IEnumerable<EmployeeInfoDto>>(ApplicationConstant.EMPLOYEEKEY);
                }
                else
                {
                    employees = _repository.GetEmployeeInfos().ToDtos();

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        //缓存24小时
                        _cacheClient.SetCache(ApplicationConstant.EMPLOYEEKEY, employees, 3600 * 24);
                    });
                }
            }
            catch(Exception ex)
            {
                //TODO：写入日志
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }

            return employees;
        }
    }
}
