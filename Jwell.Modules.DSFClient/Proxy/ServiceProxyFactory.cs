using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jwell.Modules.Cache;
using Jwell.Modules.DSFClient.BLL;
using Jwell.Modules.DSFClient.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Jwell.Modules.DSFClient.Proxy
{
    public class ServiceProxyFactory : IServiceProxyFactory
    {
        /// <summary>
        /// 调用源标识
        /// 如果是通过dll调用,其值就为 1;
        /// 如果是通过服务管理平台调用,其值则为 0
        /// </summary>
        private readonly int _invokeSource;
       
        /// <summary>
        /// 缓存客户端
        /// </summary>
        private readonly IHCacheClient _hCacheClient;

        /// <summary>
        /// 通过服务管理平台调用时上层调用栈名称
        /// </summary>
        private const string CALLERNAME = "Jwell.ServiceManage.Integration.Services.RequestIntegration";       

        public ServiceProxyFactory()
        {
            //获取上层调用栈的名称
            var method = new StackFrame(1).GetMethod();
            if (method != null && method.DeclaringType != null 
                && method.DeclaringType.FullName != CALLERNAME)
            {
                //如果是通过引用 dll 的方式来调用服务的,那么就将标识调用源的变量赋值为 1
                _invokeSource = 1;

                //那么就需要读取引用dll的程序集中的Setup.Json的文件以获取调用者的服务标识                
                //_invokeServiceSign = SetupConfig.SetupConfig.ServiceSign;              
            }
            //初始化缓存客户端
            _hCacheClient = new HCacheClient
            {
                DB = ApplicationContant.GATEWAYCACHEDB
            };
        }

        private void CheckAct(DSFParam dsfParam) 
        {
            try
            {
                //如果是通过引用 dll 的方式来调用服务的, 那么就要检查其证书            
                if (_invokeSource == 1 && dsfParam.Act == null)
                {                   
                    dsfParam.Act = new Act
                    (
                        SetupConfig.SetupConfig.ServiceSign,
                        SetupConfig.SetupConfig.ServiceNumber
                    );
                }
            }
            catch (Exception)
            {               
                throw new Exception("未能获取到调用证书!");
            }          
        }

        private Dictionary<string,object> GetParameterProperties<TParameter>(TParameter parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter.GetType() == typeof(JObject))
            {
                var jsonObj = JObject.Parse(parameter.ToString());
                return jsonObj.ToObject<Dictionary<string, object>>();
            }

            var properties = parameter.GetType().GetProperties();

            return properties.ToDictionary(property => property.Name, property => property.GetValue(parameter));
        }

        public DSFResponse<TReturn> Create<TReturn, TParameter>(string sign, string version, 
            string controller, string action,DSFParam<TParameter> dsfParam) 
            where TReturn : new() 
            where TParameter : new()
        {
            
            //1.检查调用证书
            CheckAct(dsfParam);

            //2.获取属性字典
            var propertyDic = GetParameterProperties(dsfParam.Data);

            //3.获取SVID
            var svid = GetSVID(sign, version, controller, action, propertyDic);

            //4.调用控制
            if (_invokeSource == 1)
            {
                //对来自网关的请求进行调用控制
                var invokeRecrod = GetInvokeRecordFromCache(dsfParam.Act.ServiceSign, svid);
                if (invokeRecrod == null) throw new Exception($"当前服务还未被授权访问 {sign} 服务");
                if (invokeRecrod.TotalCount != -1 && invokeRecrod.TotalCount < 1)
                    throw new Exception("调用次数已用尽,请重新申请调用次数!");
            }

            //5.获取调用接口信息;
            var apiInfo = GetApiInfo(svid);
            if (apiInfo == null) throw new Exception("未能获取到调用接口的信息,请核对传入的参数信息");

            //6.解析参数
            var domain = apiInfo.Domain;
            var url = apiInfo.URL;
            var httpVerbs = apiInfo.HttpOption;
            var apiParameters = JsonConvert.DeserializeObject<ApiParameter[]>(apiInfo.ParamInfo);

            //7.接口调用
            var retValue = Request<TReturn>(domain, url, httpVerbs, propertyDic, apiParameters);

            //6.更新调用记录
            if (_invokeSource == 1)
            {
                UpdateRecord(dsfParam.Act.ServiceSign, svid, retValue.Success);
            }

            return retValue;
           
        }

        private string GetSVID(string sign, string version,string controller, string action,Dictionary<string,object> parameterDic)
        {
            var array = new List<string> { sign, version, controller, action };
            //当然存在那些不需要参数的接口,所以这里需要判断
            if (parameterDic == null) return DSFClientBL.GetMD5(string.Join("_", array));
            foreach (var name in parameterDic.Keys)
            {
                array.Add(name.ToLower());
            }
            return DSFClientBL.GetMD5(string.Join("_", array));
        }      

        private DSFResponse<TReturn> Request<TReturn>(string domain, string url, string httpOption,
            Dictionary<string,object> parameterDic, ApiParameter[] apiParameters) where TReturn : new()
        {
            var httpMethod = GetHttpMethod(httpOption);
            var client = new RestClient(domain);
            var request = new RestRequest(url, httpMethod);
            if (apiParameters == null)
            {
                var response = client.Execute<TReturn>(request);
                return new DSFResponse<TReturn>
                {
                    Data = response.Data,
                    Error = null,
                    Success = response.IsSuccessful,
                    Message = response.StatusDescription
                };
            }
            else
            {
                foreach (var apiParameter in apiParameters)
                {
                    var property = parameterDic.FirstOrDefault(p => string.Equals(p.Key, apiParameter.Name,
                        StringComparison.CurrentCultureIgnoreCase));

                    switch (apiParameter.ParameterType)
                    {
                        case "path":
                            request.AddUrlSegment(apiParameter.Name, property.Value);
                            break;
                        case "query":
                            request.AddParameter(apiParameter.Name, property.Value, ParameterType.QueryString);
                            break;
                        case "body":
                            request.AddParameter("text/json", property.Value, ParameterType.RequestBody);
                            break;
                    }
                }

                var response = client.Execute<TReturn>(request);

                return new DSFResponse<TReturn>
                {
                    Data = response.Data,
                    Success = response.IsSuccessful,
                    Message = response.StatusDescription
                };
            }
        }

        //public DSFResponse<TReturn> Create<TReturn, TParameter>(string sign, string version, 
        //    string controller, string action, TParameter parameter) 
        //    where TParameter : new() 
        //    where TReturn : new()
        //{
        //    try
        //    {              
        //        //1.获取被调用服务接口的SVID;
        //        var type = typeof(TParameter);
        //        var paramProperties = type.GetProperties();
        //        var array = new List<string> {sign, version, controller, action};
        //        //当然存在那些不需要参数的接口,所以这里需要判断
        //        if (parameter != null)
        //        {
        //            array.AddRange(paramProperties.Select(p => p.Name.ToLower()));
        //        }
        //        var svid = DSFClientBL.GetMD5(string.Join("_", array));

        //        //2.调用控制
        //        if (_invokeSource == 1)
        //        {
        //            //对来自网关的请求进行调用控制
        //            var invokeRecrod = GetInvokeRecordFromCache(_invokeServiceSign, svid);
        //            if (invokeRecrod.TotalCount != -1 && invokeRecrod.TotalCount < 1)
        //                throw new Exception("调用次数已用尽,请重新申请调用次数!");
        //        }

        //        //3.获取调用接口信息;
        //        var apiInfo = GetApiInfo(svid);
        //        if(apiInfo == null) throw new Exception("未能获取到调用接口的信息,请核对传入的参数信息");

        //        //4.解析参数
        //        var domain = apiInfo.Domain;
        //        var url = apiInfo.URL;
        //        var httpVerbs = apiInfo.HttpOption;
        //        var apiParameters = JsonConvert.DeserializeObject<ApiParameter[]>(apiInfo.ParamInfo);

        //        //5.调用服务
        //        var retValue = Request<TReturn, TParameter>(domain, url, httpVerbs, parameter, apiParameters);

        //        //6.更新调用记录
        //        if (_invokeSource == 1)
        //        {
        //            UpdateRecord(_invokeServiceSign, svid, retValue.Success);
        //        }                
        //        return retValue;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }          
        //}

        //private DSFResponse<TReturn> Request<TReturn,TParameter>(string domain,string url,string httpOption,
        //    TParameter parameter, ApiParameter[] apiParameters) where TReturn:new()
        //{
        //    var httpMethod = GetHttpMethod(httpOption);
        //    var client = new RestClient(domain);
        //    var request = new RestRequest(url, httpMethod);
        //    var parameters = parameter.GetType().GetProperties();

        //    if (apiParameters == null)
        //    {
        //        var response = client.Execute<TReturn>(request);
        //        return new DSFResponse<TReturn>
        //        {
        //            Data = response.Data,
        //            Error = null,
        //            Success = response.IsSuccessful,
        //            Message = response.StatusDescription
        //        };
        //    }
        //    else
        //    {
        //        foreach (var apiParameter in apiParameters)
        //        {
        //            var property = parameters.FirstOrDefault(p =>string.Equals(p.Name, apiParameter.Name, 
        //                StringComparison.CurrentCultureIgnoreCase));

        //            var value = property?.GetValue(parameter);
        //            switch (apiParameter.ParameterType)
        //            {
        //                case "path":
        //                    request.AddUrlSegment(apiParameter.Name, value);
        //                    break;
        //                case "query":
        //                    request.AddParameter(apiParameter.Name, value);
        //                    break;
        //                case "body":
        //                    request.AddObject(value);
        //                    break;
        //            }
        //        }

        //        var response = client.Execute<TReturn>(request);

        //        return new DSFResponse<TReturn>
        //        {
        //            Data = response.Data,
        //            Error = null,
        //            Success = response.IsSuccessful,
        //            Message = response.StatusDescription
        //        };
        //    }           
        //}

        /// <summary>
        /// 匹配 Method 枚举
        /// </summary>
        /// <param name="httpOption"></param>
        /// <returns></returns>
        private Method GetHttpMethod(string httpOption)
        {
            switch (httpOption)
            {
                case "get": return Method.GET;
                case "put": return Method.PUT;
                case "post": return Method.POST;
                case "delete": return Method.DELETE;
                case "options": return Method.OPTIONS;
                case "head": return Method.HEAD;
                case "patch": return Method.PATCH;
                default:throw new Exception("不支持的Http方法");
            }
        }

        /// <summary>
        /// 通过缓存获取网关数据
        /// </summary>
        /// <param name="svid">被访问服务接口标识</param>
        /// <returns></returns>
        public ApiInfo GetApiInfo(string svid)
        {          
            if (_hCacheClient.IsExistH(svid))
            {
                return new ApiInfo
                {
                    HttpOption = _hCacheClient.GetHV(svid, "HttpOption"),
                    Domain = _hCacheClient.GetHV(svid, "Domain"),
                    ParamInfo = _hCacheClient.GetHV(svid, "ParamInfo"),
                    URL = _hCacheClient.GetHV(svid, "URL")
                };
            }

            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:53665/"),
                Timeout = TimeSpan.FromSeconds(30)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var accessUrl = $"api/Service/GetRequestInfo?svid={svid}";
            var message = client.GetAsync(accessUrl, HttpCompletionOption.ResponseContentRead).Result;
            var result = message.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ApiInfo>(result);          
        }

        /// <summary>
        /// 更新调用记录
        /// </summary>
        /// <param name="invokeServiceSign">调用者服务标识</param>
        /// <param name="svid">被调用服务接口Id</param>
        /// <param name="isSuccess">调用是否成功</param>
        private void UpdateRecord(string invokeServiceSign,string svid, bool isSuccess)
        {
            if (_invokeSource != 1) return;
            Task.Run(() =>
            {
                UpdateRecordCache(invokeServiceSign, svid, isSuccess);
                UpdateRecordDatabase(invokeServiceSign, svid, isSuccess);
            });
        }

        /// <summary>
        /// 将调用记录更新到数据库
        /// </summary>
        private void UpdateRecordDatabase(string invokeServiceSign,string svid,bool isSuccess)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:53665/");
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var invokeRecordDto = new InvokeRecordDto {InvokeServiceSign = invokeServiceSign, SVID = svid};
                if (isSuccess)
                {
                    invokeRecordDto.SuccessCount = 1;
                    invokeRecordDto.TotalCount = -1;                  
                    invokeRecordDto.FailedCount = 0;
                    invokeRecordDto.InvokeDateTime = DateTime.Now;
                }
                else
                {
                    invokeRecordDto.SuccessCount = 0;
                    invokeRecordDto.TotalCount = 0;                   
                    invokeRecordDto.FailedCount = 1;
                    invokeRecordDto.InvokeDateTime = DateTime.Now;
                }
                try
                {
                    var mesage = httpClient.PostAsJsonAsync("api/Service/UpdateInvokeRecord", invokeRecordDto).Result;
                    if (mesage.StatusCode != HttpStatusCode.OK)
                    {
                        //TODO 写日志
                    }
                }
                catch (Exception e)
                {
                   //TODO 写日志
                    // ReSharper disable once PossibleIntendedRethrow
                    throw e;
                }                           
            }
        }

        /// <summary>
        /// 将调用记录更新到缓存
        /// </summary>
        private void UpdateRecordCache(string invokeServiceSign,string svid,bool isSuccss)
        {
            try
            {
                var hashId = DSFClientBL.GetMD5(invokeServiceSign + "_" + svid);
                var cacheInvokeRecord = GetInvokeRecordFromCache(hashId);
                if (cacheInvokeRecord != null)
                {
                    if (isSuccss)
                    {
                        cacheInvokeRecord.SuccessCount += 1;
                        cacheInvokeRecord.TotalCount = cacheInvokeRecord.TotalCount == -1
                            ? cacheInvokeRecord.TotalCount
                            : cacheInvokeRecord.TotalCount - 1;
                        cacheInvokeRecord.InvokeDateTime = DateTime.Now;                       
                        _hCacheClient.SetHV(hashId, "SuccessCount", cacheInvokeRecord.SuccessCount.ToString());
                        _hCacheClient.SetHV(hashId, "TotalCount", cacheInvokeRecord.TotalCount.ToString());
                        _hCacheClient.SetHV(hashId, "InvokeDateTime", cacheInvokeRecord.InvokeDateTime.ToString("yyyy-MM-dd HH:mm:ss:ms"));
                    }
                    else
                    {
                        cacheInvokeRecord.FailedCount += 1;
                        cacheInvokeRecord.InvokeDateTime = DateTime.Now;
                        _hCacheClient.SetHV(hashId, "FaildCount", cacheInvokeRecord.FailedCount.ToString());
                        _hCacheClient.SetHV(hashId, "InvokeDateTime", cacheInvokeRecord.InvokeDateTime.ToString("yyyy-MM-dd HH:mm:ss:ms"));
                    }
                }
            }
            catch (Exception e)
            {
                //TODO 写日志
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }           
        }

        /// <summary>
        /// 从缓存中获取调用记录
        /// </summary>
        /// <param name="hashId">缓存hashId</param>
        /// <returns></returns>
        private CacheInvokeRecord GetInvokeRecordFromCache(string hashId)
        {
            CacheInvokeRecord cacheInvokeRecord = null;
            if (_hCacheClient.IsExistH(hashId))
            {
                cacheInvokeRecord = new CacheInvokeRecord
                {
                    TotalCount = int.Parse(_hCacheClient.GetHV(hashId, "TotalCount")),
                    FailedCount = int.Parse(_hCacheClient.GetHV(hashId, "FaildCount")),
                    SuccessCount = int.Parse(_hCacheClient.GetHV(hashId, "SuccessCount")),
                    //InvokeDateTime = DateTime.Parse(_hCacheClient.GetHV(hashId, "InvokeDateTime"))
                };
            }
            return cacheInvokeRecord;
        }

        /// <summary>
        /// 从缓存中获取调用记录
        /// </summary>
        /// <param name="invokeServiceSign">调用者服务标识</param>
        /// <param name="svid">调用服务接口SVID</param>
        /// <returns></returns>
        private CacheInvokeRecord GetInvokeRecordFromCache(string invokeServiceSign,string svid)
        {                      
            var hashId = DSFClientBL.GetMD5(invokeServiceSign + "_" + svid);
            return GetInvokeRecordFromCache(hashId);
        }       
    }
}
