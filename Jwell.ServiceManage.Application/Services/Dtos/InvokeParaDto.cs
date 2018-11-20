using Newtonsoft.Json;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class InvokeParaDto
    {
        public string ServiceSign { get; set; }

        public string VersionNumber { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public dynamic ParamInfo { get; set; }
    }
}