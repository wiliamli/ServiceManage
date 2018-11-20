using Jwell.Framework.Application.Service;
using Jwell.Framework.Paging;
using Jwell.Modules.Cache;
using Jwell.Modules.MessageQueue.Redis;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Application.Services.Interfaces;
using Jwell.ServiceManage.Application.Services.Params;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;
using Jwell.ServiceManage.Integration.Services;
using Jwell.ServiceManage.Repository.Context;
using Jwell.ServiceManage.Repository.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RequestInfoDto = Jwell.ServiceManage.Application.Services.Dtos.RequestInfoDto;

namespace Jwell.ServiceManage.Application.Services.Concretes
{
    public class JwellService: ApplicationService, IJwellService
    {
        private readonly IHCacheClient _hCacheClient;

        private readonly ICacheClient _cacheClient;

        private readonly IServiceRepository _serviceRepository;

        private readonly IServiceVersionRepository _versionRepository;       

        private readonly IServiceDelegateRepository _delegateRepository;

        private readonly IServiceAuthorityRepository _authorityRepository;

        private readonly IServiceInvokeRecordRepository _invokeRecordRepository;

        private readonly IServiceClassfyRepository _classfyRepository;

        private readonly IRedisMQ _redisMQ;

        public JwellService(IServiceRepository serviceRepository,
            IServiceClassfyRepository classfyRepository,
            IServiceVersionRepository versionRepository,
            IServiceAuthorityRepository authorityRepository,
            IServiceDelegateRepository delegateService,
            IServiceInvokeRecordRepository invokeRecordRepository,
            IHCacheClient hCacheClient,
            ICacheClient cacheClient,
            IRedisMQ redisMQ)
        {
            _serviceRepository = serviceRepository;
            _classfyRepository = classfyRepository;
            _versionRepository = versionRepository;           
            _delegateRepository = delegateService;
            _authorityRepository = authorityRepository;
            _invokeRecordRepository = invokeRecordRepository;
            _hCacheClient = hCacheClient;
            _cacheClient = cacheClient;
            _hCacheClient.DB = ApplicationConstant.GATEWAYCACHEDB;
            _cacheClient.DB = ApplicationConstant.EMPLOYEECACHEDB;
            _redisMQ = redisMQ;
        }

        public Service GetServiceBySign(string sign)
        {
            return _serviceRepository.Queryable().FirstOrDefault(s => s.ServiceSign == sign);
        }

        public bool Register(RegisterDto registerDto)
        {
            var service = registerDto.GetService();

            if (registerDto.ClassifyId == 5)
            {
                //表明注册的是授权服务
                var versions = registerDto.GetServiceVersions();
                var versionHistory = registerDto.GetVersionHistory();

                //1.写入数据库
                using (var context = new JwellDbContext())
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Services.Add(service);                      
                        context.ServiceVersions.AddRange(versions);
                        context.VersionHistories.Add(versionHistory);
                        context.SaveChanges();
                        trans.Commit();

                        //2.写入缓存
                        WriteToCache(service, versions);

                        //3.发布服务所属组织架构的信息
                        _redisMQ.Publish("TeamInfo", GetTeamInfoDto(service));

                        return true;
                    }
                    catch (Exception)//DbEntityValidationException)
                    {                    
                        trans.Rollback();                       
                        return false;
                    }
                }               
            }

            //站点系统或公共组件
            _serviceRepository.Add(service);

            return true;
        }

        private TeamInfoDto GetTeamInfoDto(Service service)
        {                   
            var teamInfoDto = new TeamInfoDto
            {
                LeaderId = service.LeaderId,
                TeamCode = service.TeamCode
            };

            teamInfoDto.ServiceInfoes.Add(new ServiceInfoDto
            {
                ServiceNumber = service.ServiceNumber,
                ServiceName = service.Name,
                OwnerId = service.Owner,
                ServiceSign = service.ServiceSign
            });

            var employees = _cacheClient.GetCache<List<ManagerDto>>(ApplicationConstant.EMPLOYEEKEY);
            if (employees == null) throw new Exception("Can not get employee informations from cache!");

            var teamLeader = employees.FirstOrDefault(e => e.EmployeeID == teamInfoDto.LeaderId);
            var owner = employees.FirstOrDefault(e => e.EmployeeID == teamInfoDto.ServiceInfoes[0].OwnerId);
            teamInfoDto.LeaderName = teamLeader == null ? "" : teamLeader.UserName;
            teamInfoDto.ServiceInfoes[0].OwnerName = owner == null ? "" : owner.UserName;
            return teamInfoDto;
        }            

        private void WriteToCache(Service service, IEnumerable<ServiceVersion> versions)
        {
            var dic = new ConcurrentDictionary<string, string>();
            foreach (var version in versions)
            {              
                dic["Domain"] = service.Domain;
                dic["HttpOption"] = version.HttpOption;
                dic["ParamInfo"] = version.ParamInfo;
                dic["URL"] = version.URL;
                dic["ServiceNumber"] = version.ServiceNumber;                
                _hCacheClient.SetHT(version.SVId, dic);
                dic.Clear();
            }
        }     

        public int AddService(ServiceDto dto)
        {
           return _serviceRepository.Add(dto.ToEntity());
        }

        public List<ServiceVersionDto> GetVersionsBySign(string sign)
        {
            return (from v in _versionRepository.Queryable()
                join s in _serviceRepository.Queryable() on v.ServiceNumber equals s.ServiceNumber
                where s.ServiceSign == sign
                select new ServiceVersionDto
                {
                    Id = v.ID,
                    SVId = v.SVId,
                    ServiceNumber = v.ServiceNumber,
                    VersionNumber = v.Version,
                    URL = v.URL,
                    ParamInfo = v.ParamInfo,
                    Remark = v.Remark,
                    Tag = v.Tag,
                    PublishTime = v.PublishTime,
                    IsClosed = v.IsClosed,
                    CreatedBy = v.CreatedBy,
                    CreatedTime = v.CreatedTime,
                    ModifiedBy = v.ModifiedBy,
                    ModifiedTime = v.ModifiedTime,
                    HttpOption = v.HttpOption,
                    ActionName = v.ActionName
                }).ToList();
        }

        public List<ManagerDto> GetManagerInfoBySign(string sign)
        {
            var managers = (from service in _serviceRepository.Queryable()
                join del in _delegateRepository.Queryable() on service.ServiceNumber equals del.ServiceNumber
                where service.ServiceSign == sign
                select new
                {
                    del.Manager
                }).ToList();
            var employees = _cacheClient.GetCache<List<ManagerDto>>(ApplicationConstant.EMPLOYEEKEY);
            if (employees == null) throw new Exception("Can not get employee informations from cache!");
            return employees.FindAll(e => managers.Exists(m => m.Manager == e.EmployeeID));
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetActionInfoBySign(string sign)
        {
            var service = _serviceRepository.Queryable().FirstOrDefault(s => s.ServiceSign == sign);
            if (service == null) return null;

            var versions = _versionRepository.Queryable().Where(v => v.ServiceNumber == service.ServiceNumber).ToList();
            if (versions.Count == 0) return null;    

            var actionInfoDic = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (var item in versions)
            {
                if (!actionInfoDic.Keys.Contains(item.Version))
                {
                    actionInfoDic.Add(item.Version, new Dictionary<string, List<string>>());
                }

                if (!actionInfoDic[item.Version].Keys.Contains(item.Tag))
                {
                    actionInfoDic[item.Version].Add(item.Tag, new List<string>());
                }               

                actionInfoDic[item.Version][item.Tag].Add(item.ActionName);
            }

            return actionInfoDic;
        }

        public List<ActionDetailInfoDto> GetActionDetailInfo(ActionDetailParaDto dto)
        {           
            return (from v in _versionRepository.Queryable()
                join s in _serviceRepository.Queryable() on v.ServiceNumber equals s.ServiceNumber
                where v.ServiceNumber == s.ServiceNumber &&
                      v.Version == dto.VersionNumber &&
                      v.Tag == dto.Tag &&
                      v.ActionName == dto.ActionName
                select new ActionDetailInfoDto
                {
                    Name = dto.ActionName,
                    HttpOption = v.HttpOption,
                    Desc = v.Remark,
                    Owner = s.Owner,
                    paramInfo = v.ParamInfo,
                    PublishedTime = v.PublishTime,
                    Uri = v.URL
                }).ToList();
        }

        public PageResult<ServiceDto> GetServiceByPage(PageParam pageParam)
        {
            return _serviceRepository.Queryable().ToDtos().ToPageResult(pageParam);
        }

        public PageResult<ServiceDto> GetServices(SearchServiceParam page)
        {
            var query = _serviceRepository.Queryable();

            if (!string.IsNullOrEmpty(page.Sign))
            {
                query = query.Where(s => s.ServiceSign == page.Sign);
            }

            return query.ToDtos().ToPageResult(page);
        }

        public PageResult<ServiceVersionDto> GetVersions(SearchVersionParam page)
        {
            var service = GetServiceBySign(page.Sign);

            if (service != null)
            {
                var query = _versionRepository.Queryable();

                if (!string.IsNullOrEmpty(page.Sign))
                {
                    query = query.Where(v => v.ServiceNumber == service.ServiceNumber);
                }

                if (!string.IsNullOrEmpty(page.Version))
                {
                    query = query.Where(v => v.Version == service.ServiceNumber);
                }

                return query.ToDtos().ToPageResult(page);
            }

            return null;
        }

        public PageResult<ManageInfoDto> GetServiceInfos(SearchManageInfoParam pageParam)
        {            
            var query = (from service in _serviceRepository.Queryable()
                join classfy in _classfyRepository.Queryable() on service.ClassfyId equals classfy.ID
                join version in _versionRepository.Queryable() on service.ServiceNumber equals version.ServiceNumber
                where service.Owner == pageParam.Owner && classfy.Name == "授权服务"
                select new ManageInfoDto
                {
                    Name = service.Name,
                    ServiceSign = service.ServiceSign,
                    URI = service.Domain,
                    PublishTime = version.PublishTime,
                    VersionNumber = version.Version,
                    State = version.IsClosed
                }).Distinct();
            return query.ToPageResult(pageParam);
        }

        public List<AuthInfoDto> GetAuthInfoBySign(string sign)
        {
            return (from s in _serviceRepository.Queryable()
                join a in _authorityRepository.Queryable() on s.ServiceNumber equals a.ServiceNumber
                join s1 in _serviceRepository.Queryable() on a.InvokeNumber equals s1.ServiceNumber
                where s.ServiceSign == sign
                select new AuthInfoDto
                {
                    ServiceNumber = s1.ServiceNumber,
                    ServiceSign = s1.ServiceSign,
                    Owner = s1.Owner,
                    State = a.IsEnabled
                }).ToList();
        }

        public AuthInfoDto Authorize(AuthAddParaDto dto)
        {
            //服务不能自己调用自己
            if (dto.ServiceSign == dto.InvokeSign) return null;

            //找到调用者服务
            var invokeService = _serviceRepository.Queryable().FirstOrDefault(s => s.ServiceSign == dto.InvokeSign);
            if (invokeService == null) return null;

            //找到被调用服务
            var service = _serviceRepository.Queryable().FirstOrDefault(s => s.ServiceSign == dto.ServiceSign);
            if (service == null) return null;           

            var recordDtos = (from v in _versionRepository.Queryable()
                where v.ServiceNumber == service.ServiceNumber && v.Version == dto.Version
                select new ServiceInvokeRecordDto
                {
                    SVId = v.SVId,
                    ControllerName = v.Tag,
                    ActionName = v.ActionName,
                    TotalCount = -1,
                    FailedCount = 0,
                    SuccessCount = 0,
                    InvokeNumber = invokeService.ServiceNumber,
                    CreatedBy = "tzj",
                    CreatedTime = DateTime.Now,
                    ModifiedBy = "tzj",
                    ModifiedTime = DateTime.Now,
                }).ToList();

            var record = new List<ServiceInvokeRecord>();
            foreach (var item in recordDtos)
            {
                record.Add(item.ToEntity());
            }           
          
            var authInfo = new ServiceAuthority
            {
                CreatedBy = "tzj",
                CreatedTime = DateTime.Now,
                InvokeNumber = invokeService.ServiceNumber,
                IsEnabled = 1,
                ModifiedBy = "tzj",
                ModifiedTime = DateTime.Now,
                ServiceNumber = service.ServiceNumber,
                Owner = service.Owner,
                Version = dto.Version,
                Applicant = dto.Applicant
            };          

            if (record.Count == 0) return null;            

            using (var context = new JwellDbContext())
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.ServiceAuthorities.Add(authInfo);
                    context.ServiceInvokeRecords.AddRange(record);
                    context.SaveChanges();
                    trans.Commit();
                    return new AuthInfoDto
                    {
                        ServiceNumber = invokeService.ServiceNumber,
                        ServiceSign = invokeService.ServiceSign,
                        Owner = invokeService.Owner,
                        State = authInfo.IsEnabled
                    };
                }
                catch (Exception e)
                {
                    //TODO 写日志
                    trans.Rollback();

                    // ReSharper disable once PossibleIntendedRethrow
                    throw e;
                }
            }                                     
        }

        public bool SetAuthState(AuthStateParaDto dto)
        {
            var auth = _authorityRepository.Queryable().FirstOrDefault(a =>
                a.ServiceNumber == dto.ServiceNumber &&
                a.InvokeNumber == dto.InvokeNumber &&
                a.Version == dto.VersionNumber);

            if (auth == null) return false;

            auth.IsEnabled = dto.IsEnabled;
            _authorityRepository.Update(auth);

            return true;            
        }

        public bool AddServiceManager(string sign, string employeeId)
        {
            var delegateDto = (from s in _serviceRepository.Queryable()
                where s.ServiceSign == sign
                select new ServiceDelegateDto
                {
                    Owner = s.Owner,
                    ServiceNumber = s.ServiceNumber,
                    CreatedTime = DateTime.Now,
                    CreatedBy = "tzj",
                    Manager = employeeId,
                    ModifiedTime = DateTime.Now,
                    ModifiedBy = "tzj"
                }).FirstOrDefault();

            if (delegateDto == null) return false;
            _delegateRepository.Add(delegateDto.ToEntity());
            return true;
        }

        public List<ManagerDto> GetManagerInfoByNameOrNo(string no, string name)
        {
            var employees = _cacheClient.GetCache<List<ManagerDto>>(ApplicationConstant.EMPLOYEEKEY);

            if (!string.IsNullOrEmpty(no))
            {
                employees = employees.FindAll(e => e.EmployeeID == no);
                return employees;
            }
                
            if (!string.IsNullOrEmpty(name))
                employees = employees.FindAll(e => e.UserName == name);

            return employees;
        }

        public PageResult<InvokeRecordDto> GetInvokeRecordsBySign(SearchInvokeInfoParam pageParam)
        {
            var invokeService = _serviceRepository.Queryable()
                .FirstOrDefault(s => s.ServiceNumber == pageParam.ServiceSign);

            if (invokeService == null) return null;           

            var records = (from service in _serviceRepository.Queryable()
                join version in _versionRepository.Queryable() on service.ServiceNumber equals version.ServiceNumber
                join record in _invokeRecordRepository.Queryable() on version.SVId equals record.SVId
                where record.InvokeNumber == invokeService.ServiceNumber
                select new InvokeRecordDto
                {
                    InvokeServiceSign = pageParam.ServiceSign,                   
                    TotalCount = record.TotalCount,
                    FailedCount = record.FailedCount,
                    SuccessCount = record.SuccessCount,
                    InvokeDateTime = record.InvokeDateTime
                }).ToList();

            return records.ToPageResult(pageParam.PageIndex, pageParam.PageSize);
        }

        public RequestInfoDto GetRequestInfo(string svid)
        {
            return (from service in _serviceRepository.Queryable()
                join version in _versionRepository.Queryable() on service.ServiceNumber equals version.ServiceNumber
                where version.SVId == svid
                select new RequestInfoDto
                {
                    Domain = service.Domain,
                    HttpOption = version.HttpOption,
                    ParamInfo = version.ParamInfo,
                    URL = version.URL
                }).ToList().FirstOrDefault();
        }

        public bool UpdateInvokeRecord(InvokeRecordDto dto)
        {
            try
            {
                var invokeService = _serviceRepository.Queryable()
                    .FirstOrDefault(s => s.ServiceSign == dto.InvokeServiceSign);
                if (invokeService == null) return false;
                var invokeServiceNumber = invokeService.ServiceNumber;           

                var invokeRecord = _invokeRecordRepository.Queryable()
                    .FirstOrDefault(i => i.SVId == dto.SVID && i.InvokeNumber == invokeServiceNumber);
                if (invokeRecord == null) return false;
                invokeRecord.FailedCount += dto.FailedCount;
                invokeRecord.TotalCount += invokeRecord.TotalCount == -1 ? 0 : dto.TotalCount;
                invokeRecord.SuccessCount += dto.SuccessCount;
                invokeRecord.InvokeDateTime = dto.InvokeDateTime;
                _invokeRecordRepository.Update(invokeRecord);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }                   
        }

        public PageResult<EmployeeInfoDto> GetEmployeeInfoByPage(PageParam pageParam)
        {
            if (!_cacheClient.IsExist(ApplicationConstant.EMPLOYEEKEY)) return null;
            var employees = _cacheClient.GetCache<IEnumerable<EmployeeInfoDto>>(ApplicationConstant.EMPLOYEEKEY);
            return employees.ToPageResult(pageParam.PageIndex, pageParam.PageSize);
        }

        public IEnumerable<ServiceDto> GetAllServiceInfo()
        {
            return _serviceRepository.Queryable().ToDtos().ToList();
        }

        public object TryRequest(InvokeParaDto dto)
        {
            return RequestIntegration.TryRequest(dto.ServiceSign, dto.VersionNumber,
                dto.ControllerName, dto.ActionName, dto.ParamInfo);
        }
    }
}