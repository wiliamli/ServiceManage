using System;
using System.Linq;
using Jwell.Framework.Paging;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ServiceAuthorityDto
    {
        public long Id { get; set; }        

        public string Version { get; set; }

        public string No { get; set; }

        public string InvokeNo { get; set; }

        public string Owner { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedTime { get; set; }

        public byte IsEnabled { get; set; }
    }

    public static class ServiceAuthorityDtoExt
    {
        public static IQueryable<ServiceAuthorityDto> ToDtos(this IQueryable<ServiceAuthority> query)
        {
            return from q in query
                select new ServiceAuthorityDto
                {
                    Id=q.Id,                  
                    Version=q.Version,
                    InvokeNo = q.InvokeNo,
                    Owner = q.Owner,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    IsEnabled = q.IsEnabled,
                    No =  q.No
                };
        }

        public static PageResult<ServiceAuthorityDto> ToDtos(this PageResult<ServiceAuthority> query)
        {
            var dto = (from q in query.Pager
                select new ServiceAuthorityDto
                {
                    Id = q.Id,                   
                    Version = q.Version,
                    InvokeNo = q.InvokeNo,
                    Owner = q.Owner,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    IsEnabled = q.IsEnabled,
                    No = q.No
                }).ToList();
            return new PageResult<ServiceAuthorityDto>(dto,query.PageIndex,query.PageSize,query.TotalCount);
        }

        public static ServiceAuthorityDto ToDto(this ServiceAuthority entity)
        {
            ServiceAuthorityDto dto = null;
            if (entity != null)
            {
                dto = new ServiceAuthorityDto
                {
                    Id = entity.Id,                  
                    Version = entity.Version,
                    InvokeNo = entity.InvokeNo,
                    Owner = entity.Owner,
                    CreatedBy = entity.CreatedBy,
                    CreatedTime = entity.CreatedTime,
                    IsEnabled = entity.IsEnabled,
                    No = entity.No
                };
            }

            return dto;
        }

        public static ServiceAuthority ToEntity(this ServiceAuthorityDto dto)
        {
            ServiceAuthority entity = null;
            if (dto != null)
            {
                entity = new ServiceAuthority
                {
                    Id = dto.Id,                   
                    Version = dto.Version,
                    InvokeNo = dto.InvokeNo,
                    Owner = dto.Owner,
                    CreatedBy = dto.CreatedBy,
                    CreatedTime = dto.CreatedTime,
                    IsEnabled = dto.IsEnabled,
                    No = dto.No
                };
            }

            return entity;
        }
    }
}