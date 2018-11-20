
namespace Jwell.Modules.DSFClient.Proxy
{
    public interface IServiceProxyFactory
    {
        /// <summary>
        /// 创建服务对象
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <typeparam name="TParameter">参数类型</typeparam>
        /// <param name="sign">服务标识</param>
        /// <param name="version">服务版本号</param>
        /// <param name="controller">控制器名称</param>
        /// <param name="action">接口名称</param>
        /// <param name="dsfParam">参数</param>
        /// <returns></returns>
        DSFResponse<TReturn> Create<TReturn, TParameter>(string sign, string version, 
            string controller,string action, DSFParam<TParameter>  dsfParam)
            where TParameter : new() 
            where TReturn : new();              
    }
}
