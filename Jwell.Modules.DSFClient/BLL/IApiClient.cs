namespace Jwell.Modules.DSFClient.BLL
{
    public interface IApiClient
    {      
        string Request<T>(string url, MethodEnum method, T value = default(T));
    }
}