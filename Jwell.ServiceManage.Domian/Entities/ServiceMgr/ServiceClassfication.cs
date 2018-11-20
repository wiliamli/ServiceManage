using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jwell.ServiceManage.Domain.Entities.Base;

namespace Jwell.ServiceManage.Domain.Entities.ServiceMgr
{
    [Table("ServiceClassfications")]
    public class ServiceClassfication : BaseEntity
    {     
        [MaxLength(8)]
        public string Name { get; set; }      
    }
}