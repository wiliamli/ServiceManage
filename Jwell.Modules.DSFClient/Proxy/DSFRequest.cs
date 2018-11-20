namespace Jwell.Modules.DSFClient.Proxy
{
    public abstract class DSFParam 
    {
        public Act Act { get; set; }       
    }

    public class DSFParam<T> : DSFParam where T : new()
    {
        /// <summary>
        /// 参数
        /// </summary>
        public T Data { get; set; }
    }
}
