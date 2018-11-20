using Jwell.Modules.DSFClient.Proxy;

namespace Jwell.ServiceManage.Integration.Services
{
    public class RequestIntegration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="version"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="paramInfo"></param>
        /// <returns></returns>
        public static object TryRequest(string sign, string version, string controllerName, string actionName, dynamic paramInfo)
        {
            var proxFactory = new ServiceProxyFactory();
            var dsfParam = new DSFParam<dynamic> {Data = paramInfo};
            return proxFactory.Create<object, dynamic>(sign, version, controllerName, actionName, dsfParam).Data;
        }
    }
}