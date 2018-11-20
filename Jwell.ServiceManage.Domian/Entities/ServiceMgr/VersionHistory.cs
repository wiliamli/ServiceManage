using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.Entities.ServiceMgr
{
    [Table("VersionHistory")]
    public class VersionHistory:BaseEntity
    {
        /// <summary>
        /// 服务编号
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string ServiceNumber { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Version { get; set; }

        /// <summary>
        /// API说明文档
        /// </summary>
        public string DocJson { get; set; }
    }
}