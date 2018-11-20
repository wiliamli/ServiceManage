using Jwell.Framework.Application.Service;
using Jwell.Modules.Cache;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Application.Services.Interfaces;
using Jwell.ServiceManage.Repository.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwell.ServiceManage.Application.Services.Concretes
{
    public class GatewayCacheSyncService : ApplicationService, IGateWayCacheSyncService
    {
        private readonly IHCacheClient _cacheClient;

        private readonly IServiceRepository _serviceRepository;

        private readonly IServiceInvokeRecordRepository _serviceInvokeRecordRepository;

        private readonly IServiceVersionRepository _serviceVersionRepository;

        public GatewayCacheSyncService(IServiceRepository serviceRepository,
            IServiceVersionRepository serviceVersionRepository,
            IServiceInvokeRecordRepository serviceInvokeRecordRepository,
            IHCacheClient cacheClient)
        {
            _serviceRepository = serviceRepository;
            _serviceVersionRepository = serviceVersionRepository;
            _serviceInvokeRecordRepository = serviceInvokeRecordRepository;
            _cacheClient = cacheClient;
            _cacheClient.DB = ApplicationConstant.GATEWAYCACHEDB;
        }

        private  string GetMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }     

        public bool GatewayCacheInfoSync()
        {
            try
            {
                var gatewayCacheInfoDtos = (from service in _serviceRepository.Queryable()
                    join version in _serviceVersionRepository.Queryable()
                        on service.ServiceNumber equals version.ServiceNumber
                    select new GatewayCacheInfoDto
                    {
                        SVID = version.SVId,
                        ServiceSign = service.ServiceSign,
                        VersionNumber = version.Version,
                        ControllerName = version.Tag,
                        ActionName = version.URL,
                        Domain = service.Domain,
                        HttpOption = version.HttpOption,
                        ParamInfo = version.ParamInfo,
                        ServiceNumber = service.ServiceNumber,
                    }).ToList();
                

                if (gatewayCacheInfoDtos.Count>0)
                {
                    Task.Run(() =>
                    {
                        var dict = new ConcurrentDictionary<string, string>();
                        foreach (var dto in gatewayCacheInfoDtos)
                        {                                                     
                            var keys = new[] { "Domain", "HttpOption", "ParamInfo","ServiceNumber" };                            
                            if (_cacheClient.IsExistH(dto.SVID)) _cacheClient.ReomveHKS(dto.SVID, keys);
                            dict["Domain"] = dto.Domain;
                            dict["HttpOption"] = dto.HttpOption;
                            dict["ParamInfo"] = dto.ParamInfo;
                            dict["ServiceNumber"] = dto.ServiceNumber;
                            _cacheClient.SetHT(dto.SVID, dict);
                            dict.Clear();
                        }
                    });
                }         
                
                return true;
            }
            catch (Exception e)
            {
                //TODO:写日志
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }            
        }       

        public bool GatewayInvokeCacheInfoSync()
        {
            try
            {              
                var invokeRecords = (from record in _serviceInvokeRecordRepository.Queryable()
                    join service in _serviceRepository.Queryable() on record.InvokeNumber equals service.ServiceNumber
                    select new InvokeRecordDto
                    {
                        InvokeServiceSign = service.ServiceSign,
                        TotalCount = record.TotalCount,
                        FailedCount = record.FailedCount,
                        SuccessCount = record.SuccessCount,
                        InvokeDateTime = record.InvokeDateTime,
                        SVID = record.SVId
                    }).ToList();

                if (invokeRecords.Count > 0)
                {
                    Task.Run(() =>
                    {
                        var dict = new ConcurrentDictionary<string, string>();                    
                        foreach (var invokeCache in invokeRecords)
                        {
                            var array = new[]
                            {
                                invokeCache.InvokeServiceSign,
                                invokeCache.SVID
                            };
                            var hashId = GetMD5(string.Join("_", array));
                            dict["TotalCount"] = invokeCache.TotalCount.ToString();
                            dict["FaildCount"] = invokeCache.FailedCount.ToString();
                            dict["SuccessCount"] = invokeCache.SuccessCount.ToString();
                            dict["InvokeCount"] = invokeCache.InvokeCount.ToString();
                            dict["InvokeDateTime"] = invokeCache.InvokeDateTime.ToString("yyyy-MM-dd HH:mm:ss:ms");
                            _cacheClient.SetHT(hashId, dict);                           
                            dict.Clear();
                        }
                    });
                }
                return true;
            }
            catch (Exception e)
            {
                //TODO 写日志
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }
        }
    }
}