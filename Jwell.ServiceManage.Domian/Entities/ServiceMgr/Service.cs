using Jwell.ServiceManage.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jwell.ServiceManage.Domain.Entities.ServiceMgr
{
    [Table("Services")]
    public class Service:BaseEntity
    {
        /// <summary>
        /// 服务的GUID编号
        /// </summary>
        [MaxLength(36)]
        [Required]
        public string ServiceNumber { get; set; }

        /// <summary>
        /// 服务的中文名称
        /// </summary>
        [MaxLength(32)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 服务的英文标识,例如:Jwell.Sample
        /// </summary>
        [MaxLength(512)]
        [Required]
        public string ServiceSign { get; set; }

        /// <summary>
        ///代表拥有此服务并对此负责的人,他或她可以将此服务授权给其他服务。
        /// 一般而言，它指的是服务的开发人员。
        /// </summary>
        [MaxLength(16)]
        [Required]
        public string Owner { get; set; }

        /// <summary>
        ///服务代码所在的SVN服务器中的访问路径,用作后面的自动化部署
        /// </summary>
        [MaxLength(512)]
        [Required]
        public string SVNPath { get; set; }

        /// <summary>
        /// 发布后的SWAGGER接口说明文档的访问URL,要求用户手动填写
        /// 例如 http://localhost:53664/swagger/docs/v1
        /// </summary>
        [MaxLength(1024)]
        [Required]
        public string DocPath { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Domain { get; set; }

        [Required]
        [MaxLength(32)]
        public string TeamCode { get; set; }

        [Required]
        [MaxLength(16)]
        public string LeaderId { get; set; }

        /// <summary>
        /// 服务注册人员
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 修改人员
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Required]
        public DateTime ModifiedTime { get; set; } 
        
        /// <summary>
        /// 分类Id
        /// </summary>
        [Required]
        public long ClassfyId { get; set; }
    }
}