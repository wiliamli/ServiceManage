using System;
using System.Linq;
using Jwell.Framework.Paging;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ServiceVersionDto
    {     
        public long Id { get; set; }

        public string SVId { get; set; }
    
        public string ServiceNumber { get; set; }
     
        public string VersionNumber { get; set; }
     
        public string URL { get; set; }
      
        public string ParamInfo { get; set; }    
      
        public string Remark { get; set; }

        public string HttpOption { get; set; }

        public string Tag { get; set; }

        public string ActionName { get; set; }
       
        public DateTime PublishTime { get; set; }
      
        public byte IsClosed { get; set; }
       
        public string CreatedBy { get; set; }
        
        public DateTime CreatedTime { get; set; }
       
        public string ModifiedBy { get; set; }
      
        public DateTime ModifiedTime { get; set; }
    }

    public static class ServiceVersionDtoExt
    {
        public static IQueryable<ServiceVersionDto> ToDtos(this IQueryable<ServiceVersion> query)
        {
            return from q in query
                select new ServiceVersionDto
                {
                    Id = q.ID,
                    SVId = q.SVId,
                    ServiceNumber = q.ServiceNumber,
                    VersionNumber = q.Version,
                    URL = q.URL,
                    ParamInfo = q.ParamInfo,
                    Remark = q.Remark,
                    Tag = q.Tag,
                    PublishTime = q.PublishTime,
                    IsClosed = q.IsClosed,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    ModifiedBy = q.ModifiedBy,
                    ModifiedTime = q.ModifiedTime,
                    HttpOption = q.HttpOption,
                    ActionName = q.ActionName
                };
        }

        public static PageResult<ServiceVersionDto> ToDtos(this PageResult<ServiceVersion> query)
        {
            var queryDto = (from q in query.Pager
                select new ServiceVersionDto
                {
                    Id = q.ID,
                    SVId = q.SVId,
                    ServiceNumber = q.ServiceNumber,
                    VersionNumber = q.Version,
                    URL = q.URL,
                    ParamInfo = q.ParamInfo,
                    Remark = q.Remark,
                    Tag = q.Tag,
                    PublishTime = q.PublishTime,
                    IsClosed = q.IsClosed,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    ModifiedBy = q.ModifiedBy,
                    ModifiedTime = q.ModifiedTime,
                    HttpOption = q.HttpOption,
                    ActionName = q.ActionName
                }).ToList();
            return new PageResult<ServiceVersionDto>(queryDto, query.PageIndex, query.PageSize, query.TotalCount);
        }

        public static ServiceVersionDto ToDto(this ServiceVersion entity)
        {
            ServiceVersionDto dto = null;
            if (entity != null)
            {
                dto = new ServiceVersionDto
                {
                    Id = entity.ID,
                    SVId = entity.SVId,
                    ServiceNumber = entity.ServiceNumber,
                    VersionNumber = entity.Version,
                    URL = entity.URL,
                    ParamInfo = entity.ParamInfo,
                    Remark = entity.Remark,
                    Tag = entity.Tag,
                    PublishTime = entity.PublishTime,
                    IsClosed = entity.IsClosed,
                    CreatedBy = entity.CreatedBy,
                    CreatedTime = entity.CreatedTime,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedTime = entity.ModifiedTime,
                    HttpOption = entity.HttpOption,
                    ActionName = entity.ActionName
                };
            }
            return dto;
        }

        public static ServiceVersion ToEntity(this ServiceVersionDto dto)
        {
            ServiceVersion serviceVersion = null;
            if (dto != null)
            {
                serviceVersion = new ServiceVersion
                {
                    ID = dto.Id,
                    SVId = dto.SVId,
                    ServiceNumber = dto.ServiceNumber,
                    Version = dto.VersionNumber,
                    URL = dto.URL,
                    ParamInfo = dto.ParamInfo,
                    Remark = dto.Remark,
                    Tag = dto.Tag,
                    PublishTime = dto.PublishTime,
                    IsClosed = dto.IsClosed,
                    CreatedBy = dto.CreatedBy,
                    CreatedTime = dto.CreatedTime,
                    ModifiedBy = dto.ModifiedBy,
                    ModifiedTime = dto.ModifiedTime,
                    HttpOption = dto.HttpOption,
                    ActionName = dto.ActionName
                };
            }

            return serviceVersion;
        }
    }
}
