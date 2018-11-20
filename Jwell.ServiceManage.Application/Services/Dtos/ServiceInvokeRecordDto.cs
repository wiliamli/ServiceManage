using System;
using System.Linq;
using Jwell.Framework.Paging;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ServiceInvokeRecordDto
    {     
        public string SVId { get; set; }
     
        public string ControllerName { get; set; }
      
        public string ActionName { get; set; }
      
        public int TotalCount { get; set; }
      
        public int FailedCount { get; set; }
     
        public int SuccessCount { get; set; }
     
        public DateTime InvokeDateTime { get; set; }

        public string InvokeNumber { get; set; }
       
        public string CreatedBy { get; set; }
     
        public DateTime CreatedTime { get; set; }
      
        public string ModifiedBy { get; set; }
      
        public DateTime ModifiedTime { get; set; }
    }

    public static class ServiceInvokeRecordDtoExt
    {
        public static IQueryable<ServiceInvokeRecordDto> ToDtos(this IQueryable<ServiceInvokeRecord> query)
        {
            return from q in query
                select new ServiceInvokeRecordDto
                {
                    SVId = q.SVId,
                    ActionName = q.ActionName,
                    ControllerName = q.ControllerName,                  
                    FailedCount = q.FailedCount,
                    InvokeDateTime = q.InvokeDateTime,
                    SuccessCount = q.SuccessCount,
                    TotalCount = q.TotalCount,
                    InvokeNumber = q.InvokeNumber,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    ModifiedBy = q.ModifiedBy,
                    ModifiedTime = q.ModifiedTime
                };
        }

        public static PageResult<ServiceInvokeRecordDto> ToDtos(this PageResult<ServiceInvokeRecord> query)
        {
            var queryDto =(from q in query.Pager
                select new ServiceInvokeRecordDto
                {
                    SVId = q.SVId,
                    ActionName = q.ActionName,
                    ControllerName = q.ControllerName,                   
                    FailedCount = q.FailedCount,
                    InvokeDateTime = q.InvokeDateTime,
                    SuccessCount = q.SuccessCount,
                    TotalCount = q.TotalCount,
                    InvokeNumber = q.InvokeNumber,
                    CreatedBy = q.CreatedBy,
                    CreatedTime = q.CreatedTime,
                    ModifiedBy = q.ModifiedBy,
                    ModifiedTime = q.ModifiedTime,
                }).ToList();
            return new PageResult<ServiceInvokeRecordDto>(queryDto, query.PageIndex, query.PageSize, query.TotalCount);
        }

        public static ServiceInvokeRecordDto ToDto(this ServiceInvokeRecord entity)
        {
            ServiceInvokeRecordDto dto = null;
            if (entity != null)
            {
               dto = new ServiceInvokeRecordDto
               {
                   SVId = entity.SVId,
                   ActionName = entity.ActionName,
                   ControllerName = entity.ControllerName,
                   TotalCount = entity.TotalCount,
                   FailedCount = entity.FailedCount,
                   SuccessCount = entity.SuccessCount,                 
                   InvokeDateTime = entity.InvokeDateTime,
                   InvokeNumber = entity.InvokeNumber,
                   CreatedBy = entity.CreatedBy,
                   CreatedTime = entity.CreatedTime,
                   ModifiedBy = entity.ModifiedBy,
                   ModifiedTime = entity.ModifiedTime,
               };
            }

            return dto;
        }

        public static ServiceInvokeRecord ToEntity(this ServiceInvokeRecordDto dto)
        {
            ServiceInvokeRecord serviceInvokeRecord = null;
            if (dto != null)
            {
                serviceInvokeRecord = new ServiceInvokeRecord
                {
                    SVId = dto.SVId,
                    ActionName = dto.ActionName,
                    ControllerName = dto.ControllerName,
                    TotalCount = dto.TotalCount,
                    FailedCount = dto.FailedCount,
                    SuccessCount = dto.SuccessCount,                  
                    InvokeDateTime = dto.InvokeDateTime,
                    InvokeNumber = dto.InvokeNumber,
                    CreatedBy = dto.CreatedBy,
                    CreatedTime = dto.CreatedTime,
                    ModifiedBy = dto.ModifiedBy,
                    ModifiedTime = dto.ModifiedTime,
                };
            }
            return serviceInvokeRecord;
        }
    }
}