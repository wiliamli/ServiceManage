using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.Entities.ServiceMgr
{
    [Table("ServiceVersions")]
    public class ServiceVersion : BaseEntity
    {
        /// <summary>
        /// api唯一标识
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string SVId { get; set; }

        /// <summary>
        /// 对应服务的编号
        /// </summary>
        [Required]       
        [MaxLength(36)]
        public string ServiceNumber { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Version { get; set; }

        /// <summary>
        /// 服务调用的参数信息
        /// </summary>
        [MaxLength(1024)]
        public string ParamInfo { get; set; }

        /// <summary>
        /// 访问路径，是指发布之后的访问路径，根路径，
        /// 例如 http://localhost/Jwell/api/Service/Post,
        ///http://localhost/Jwell/api/Service/Get
        /// </summary>
        [Required]
        [MaxLength(512)]
        public string URL { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Required]
        public DateTime PublishTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(512)]
        [Required]
        public string Remark { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        [Required]
        public byte IsClosed { get; set; }

        /// <summary>
        /// get or post
        /// </summary>
        [Required]
        public string HttpOption { get; set; }

        /// <summary>
        /// 标签，SWAGGER里面的Tags，指代服务中API的Controller
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Tag { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string ActionName { get; set; }      

        /// <summary>
        /// 创建人员
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 修改人员
        /// </summary>
        [MaxLength(16)]
        [Required]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Required]
        public DateTime ModifiedTime { get; set; }
    }
}