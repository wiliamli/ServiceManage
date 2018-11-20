using System;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ActionDetailInfoDto
    {
        public string Name { get; set; }

        public string Uri { get; set; }

        public string Desc { get; set; }

        public DateTime PublishedTime { get; set; }

        public string HttpOption { get; set; }

        public string paramInfo { get; set; }

        public string Owner { get; set; }
    }
}