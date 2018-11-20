using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.Entities.ServiceMgr
{
    [Table("ServiceDelegates")]
    public class ServiceDelegate : BaseEntity
    {
        [Required]
        [MaxLength(16)]
        public string Owner { get; set; }

        [Required]
        [MaxLength(36)]
        public string ServiceNumber { get; set; }

        [Required]
        [MaxLength(16)]
        public string Manager { get; set; }

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
    }
}