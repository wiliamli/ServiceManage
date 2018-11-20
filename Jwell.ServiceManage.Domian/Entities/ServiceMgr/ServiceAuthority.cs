using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.Entities.ServiceMgr
{
    [Table("ServiceAuthorities")]
    public class ServiceAuthority : BaseEntity
    {  
        [MaxLength(16)]
        [Required]
        public string Owner { get; set; }      

        /// <summary>
        /// 被调用服务版本
        /// </summary>
        [MaxLength(64)]
        [Required]
        public string Version { get; set; }

        /// <summary>
        /// 被调用服务编号
        /// </summary>
        [MaxLength(36)]
        [Required]
        public string ServiceNumber { get; set; }

        /// <summary>
        /// 调用服务编号
        /// </summary>
        [MaxLength(36)]
        [Required]
        public string InvokeNumber { get; set; }

        public byte IsEnabled { get; set; }

        [MaxLength(16)]
        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        [Required]
        [MaxLength(16)]
        public string ModifiedBy { get; set; }

        [Required]
        public DateTime ModifiedTime { get; set; }

        [Required]
        [MaxLength(16)]
        public string Applicant { get; set; }
    }
}