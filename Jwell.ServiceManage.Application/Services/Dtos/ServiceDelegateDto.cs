using System;
using System.Linq;
using Jwell.Framework.Paging;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ServiceDelegateDto
    {      
        public long Id { get; set; }

        public string ServiceNumber { get; set; }
     
        public string Manager { get; set; }
       
        public string Owner { get; set; }
      
        public string CreatedBy { get; set; }
     
        public DateTime CreatedTime { get; set; }
     
        public string ModifiedBy { get; set; }
       
        public DateTime ModifiedTime { get; set; }
    }

    public static class ServiceDelegateDtoExt
    {
        public static IQueryable<ServiceDelegateDto> ToDtos(this IQueryable<ServiceDelegate> query)
        {
            return from q in query
                select new ServiceDelegateDto
                {
                    Id = q.ID,
                    ServiceNumber = q.ServiceNumber,
                    Manager = q.Manager,
                    Owner = q.Owner,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    ModifiedBy = q.ModifiedBy,
                    ModifiedTime = q.ModifiedTime
                };
        }

        public static PageResult<ServiceDelegateDto> ToDtos(this PageResult<ServiceDelegate> query)
        {
            var queryDto = (from q in query.Pager
                select new ServiceDelegateDto
                {
                    Id = q.ID,
                    ServiceNumber = q.ServiceNumber,
                    Manager = q.Manager,
                    Owner = q.Owner,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    ModifiedBy = q.ModifiedBy,
                    ModifiedTime = q.ModifiedTime
                }).ToList();
            return new PageResult<ServiceDelegateDto>(queryDto,query.PageIndex,query.PageSize,query.TotalCount);
        }

        public static ServiceDelegateDto ToDto(this ServiceDelegate entity)
        {
            ServiceDelegateDto dto = null;
            if (entity != null)
            {
                dto = new ServiceDelegateDto
                {
                    Id = entity.ID,
                    ServiceNumber = entity.ServiceNumber,
                    Manager = entity.Manager,
                    Owner = entity.Owner,
                    CreatedBy = entity.CreatedBy,
                    CreatedTime = entity.CreatedTime,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedTime = entity.ModifiedTime
                };
            }

            return dto;
        }

        public static ServiceDelegate ToEntity(this ServiceDelegateDto dto)
        {
            ServiceDelegate entity = null;
            if (dto != null)
            {
                entity = new ServiceDelegate
                {
                    ID = dto.Id,
                    ServiceNumber = dto.ServiceNumber,
                    Manager = dto.Manager,
                    Owner = dto.Owner,
                    CreatedBy = dto.CreatedBy,
                    CreatedTime = dto.CreatedTime,
                    ModifiedBy = dto.ModifiedBy,
                    ModifiedTime = dto.ModifiedTime
                };
            }
            return entity;
        }
    }

}