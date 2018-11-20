using System;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ManageInfoDto
    {
        public string Name { get; set; }

        public string ServiceSign { get; set; }

        public string URI { get; set; }

        public DateTime PublishTime { get; set; }

        public string VersionNumber { get; set; }           

        public byte? State { get; set; }
    }
}
