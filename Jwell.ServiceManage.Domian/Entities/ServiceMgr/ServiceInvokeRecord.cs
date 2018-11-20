using System;
using System.ComponentModel.DataAnnotations;
using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.Entities.ServiceMgr
{
    public class ServiceInvokeRecord:BaseEntity
    {
        /// <summary>
        /// 接口API标识
        /// </summary>
        [MaxLength(36)]
        [Required]
        public string SVId { get; set; }

        /// <summary>
        /// Controller名称
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string ControllerName { get; set; }

        /// <summary>
        /// Action名称
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string ActionName { get; set; }

        /// <summary>
        /// 总允许调用次数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 调用失败次数
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 调用成功次数
        /// </summary>
        public int SuccessCount { get; set; }      

        /// <summary>
        /// 最近调用时间
        /// </summary>
        public DateTime InvokeDateTime { get; set; }

        [Required]
        [MaxLength(36)]
        public string InvokeNumber { get; set; }

        /// <summary>
        /// 服务创建人员
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 记录修改人
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 记录修改时间
        /// </summary>
        [Required]
        public DateTime ModifiedTime { get; set; }       
    }
}