using System.Linq;
using Jwell.Framework.Paging;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ServiceDto
    {           
        public string ServiceNumber { get; set; }
       
        public string Name { get; set; }
        
        public string ServiceSign { get; set; }
       
        public string Domain { get; set; }
     
        public string SVNPath { get; set; }
      
        public string DocPath { get; set; }
       
        public string Owner { get; set; }

        public long ClassfyId { get; set; }

        public string TeamCode { get; set; }

        public string LeaderId { get; set; }
    }

    public static class ServiceDtoExt
    {
        public static IQueryable<ServiceDto> ToDtos(this IQueryable<Service> query)
        {
            return from q in query 
                select new ServiceDto
                {                   
                    ServiceNumber = q.ServiceNumber,
                    Name = q.Name,
                    ServiceSign = q.ServiceSign,
                    Owner = q.Owner,
                    SVNPath = q.SVNPath,
                    DocPath = q.DocPath,
                    Domain = q.Domain,
                    ClassfyId = q.ClassfyId,
                    TeamCode = q.TeamCode,
                    LeaderId = q.LeaderId
                };
        }

        public static PageResult<ServiceDto> ToDtos(this PageResult<Service> query)
        {
            var queryDto = (from q in query.Pager
                select new ServiceDto
                {                  
                    ServiceNumber = q.ServiceNumber,
                    Name = q.Name,
                    ServiceSign = q.ServiceSign,
                    Owner = q.Owner,
                    SVNPath = q.SVNPath,
                    DocPath = q.DocPath,
                    Domain = q.Domain,
                    ClassfyId = q.ClassfyId,
                    TeamCode = q.TeamCode,
                    LeaderId = q.LeaderId
                }).ToList();
            return new PageResult<ServiceDto>(queryDto, query.PageIndex, query.PageSize, query.TotalCount);
        }

        public static ServiceDto ToDto(this Service service)
        {
            ServiceDto dto = null;
            if (service != null)
            {
                dto = new ServiceDto
                {                  
                    ServiceNumber = service.ServiceNumber,
                    Name = service.Name,
                    ServiceSign = service.ServiceSign,
                    Owner = service.Owner,
                    SVNPath = service.SVNPath,
                    DocPath = service.DocPath,
                    Domain = service.Domain,
                    ClassfyId = service.ClassfyId,
                    TeamCode = service.TeamCode,
                    LeaderId = service.LeaderId
                };
            }
            return dto;
        }
        public static Service ToEntity(this ServiceDto serviceDto)
        {
            Service service = null;
            if (serviceDto != null)
            {
                service = new Service
                {                    
                    ServiceNumber = serviceDto.ServiceNumber,
                    Name = serviceDto.Name,
                    ServiceSign = serviceDto.ServiceSign,
                    Owner = serviceDto.Owner,
                    SVNPath = serviceDto.SVNPath,
                    DocPath = serviceDto.DocPath,
                    Domain = serviceDto.Domain,
                    ClassfyId = serviceDto.ClassfyId,
                    TeamCode = serviceDto.TeamCode,
                    LeaderId = serviceDto.LeaderId
                };               
            }
            return service;
        }
    }
}