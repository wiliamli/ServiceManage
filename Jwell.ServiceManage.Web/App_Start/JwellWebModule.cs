using Jwell.Framework.Modules;
using Jwell.Modules.MessageQueue;
using Jwell.Modules.MVC;
using Jwell.Modules.WebApi;
using Jwell.ServiceManage.Application;
using Jwell.ServiceManage.Repository;

namespace Jwell.ServiceManage.Web
{
    [DependOn(typeof(MvcModule),typeof(WebApiModule),typeof(JwellApplicationModule),typeof(JwellRepositoryModule),typeof(JwellMessageQueueModule))]
    public class JwellWebModule : JwellModule
    {
    }
}