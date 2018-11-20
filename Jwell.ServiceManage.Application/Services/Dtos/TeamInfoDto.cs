using System.Collections.Generic;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class TeamInfoDto
    {
        public string LeaderId { get; set; }

        public string LeaderName { get; set; }

        public string TeamCode { get; set; }

        public List<ServiceInfoDto> ServiceInfoes { get; set; }

        public TeamInfoDto()
        {
            ServiceInfoes = new List<ServiceInfoDto>();
        }
    }

}