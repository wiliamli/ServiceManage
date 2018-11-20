using System.Collections.Generic;
using System.Linq;
using Jwell.ServiceManage.Domain.Entities;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class EmployeeInfoDto
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeID { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 员工头像
        /// </summary>
        public string Department { get; set; }
    }

    /// <summary>
    /// Mapper
    /// </summary>
    public static class EmployeeInfoDtoExt
    {
        public static IEnumerable<EmployeeInfoDto> ToDtos(this IEnumerable<EmployeeInfo> query)
        {
            return from a in query
                   select new EmployeeInfoDto()
                   {
                       UserID = a.ID,
                       DepartmentCode = a.DepartmentCode,
                       Department = a.Department,
                       EmployeeID = a.EmployeeID,
                       Password = a.Password,
                       UserName = a.UserName
                   };
        }
    }
}