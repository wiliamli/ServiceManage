using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.Entities
{
    public class EmployeeInfo : BaseEntity
    {
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
}
