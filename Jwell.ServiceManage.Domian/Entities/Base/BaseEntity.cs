using System.ComponentModel.DataAnnotations.Schema;
using Jwell.Framework.Domain.Entities;

namespace Jwell.ServiceManage.Domain.Entities.Base
{
    public class BaseEntity:Entity<long>
    {
        /// <summary>
        /// 主键自增Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long ID
        {
            get
            {
                return base.ID;
            }

            set
            {
                base.ID = value;
            }
        }
    }
}
