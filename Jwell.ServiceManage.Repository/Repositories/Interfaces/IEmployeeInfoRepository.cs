using System.Collections.Generic;
using Jwell.Framework.Domain.Repositories;
using Jwell.ServiceManage.Domain.Entities;

namespace Jwell.ServiceManage.Repository.Repositories.Interfaces
{
    public interface IEmployeeInfoRepository: IRepository<EmployeeInfo, long>
    {
        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<EmployeeInfo> GetEmployeeInfos();
    }
}
