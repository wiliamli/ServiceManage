using Jwell.Framework.Paging;

namespace Jwell.ServiceManage.Application.Services.Params
{
    public class SearchVersionParam:PageParam
    {
        public string Sign { get; set; }

        public string Version { get; set; }
    }
}