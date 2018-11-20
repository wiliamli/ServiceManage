using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Jwell.ServiceManage.Application.ApiSpecTools;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;
using Newtonsoft.Json;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class RegisterDto
    {
        public string Name { get; set; }

        public string ServiceSign { get; set; }

        public string VersionNumber { get; set; }

        public string Domain { get; set; }

        public string SVNPath { get; set; }

        public string JsonUrl { get; set; }

        public string Owner { get; set; }

        public string Remark { get; set; }

        public long ClassifyId { get; set; }

        public string TeamCode { get; set; }

        public string LeaderId { get; set; }

        public byte IsClosed { get; set; }

        public DateTime PublishTime { get; set; }
    }

    public static class ServiceAndVersionDtoExt
    {
        private static string _guid;

        private static string _docJson;

        public static Service GetService(this RegisterDto dto)
        {
            _guid = Guid.NewGuid().ToString("N");
            var service = new Service
            {
                ServiceNumber = _guid,
                Name = dto.Name,
                ServiceSign = dto.ServiceSign,
                Owner = dto.Owner,
                SVNPath = dto.SVNPath,
                DocPath = dto.JsonUrl,
                Domain = dto.Domain,
                TeamCode = dto.TeamCode,
                LeaderId = dto.LeaderId,
                ClassfyId = dto.ClassifyId,
                CreatedBy = "tzj",
                ModifiedTime = DateTime.Now,
                ModifiedBy = "tzj",
                CreatedTime = DateTime.Now
            };
            return service;
        }

        public static List<ServiceVersion> GetServiceVersions(this RegisterDto dto)
        {
            SetDocJson(dto.JsonUrl);

            var settings = new JsonSerializerSettings()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };
            var swaggerDocument = JsonConvert.DeserializeObject<SwaggerDocument>(_docJson, settings);

            var versions = new List<ServiceVersion>();

            foreach (var pathItem in swaggerDocument.paths)
            {
                foreach (var item in pathItem.Value.GetOperationDic(swaggerDocument.definitions))
                {
                    var url = pathItem.Key;
                    var httpOption = item.Key.name;
                    var remark = item.Key.summary;
                    var tag = item.Key.tags[0];
                    var paraInfo = JsonConvert.SerializeObject(item.Value);
                    var paraNames = item.Value?.Select(apiParameter => apiParameter.Name.ToLower()).ToList();

                    string actionName;
                    if (!pathItem.Key.Contains("{"))
                    {
                        var maybeActionName = pathItem.Key.Split('/').Last();
                        actionName = maybeActionName == tag ? item.Key.name : maybeActionName;
                    }
                    else
                    {
                        actionName = item.Key.name;
                    }

                    switch (actionName)
                    {
                        case "get":
                            actionName = "Get";
                            break;
                        case "post":
                            actionName = "Post";
                            break;
                        case "delete":
                            actionName = "Delete";
                            break;
                        case "put":
                            actionName = "Put";
                            break;
                    }

                    var array = new List<string> {dto.ServiceSign, dto.VersionNumber, tag, actionName};
                    if (paraNames != null) array.AddRange(paraNames);

                    versions.Add(new ServiceVersion
                    {
                        ServiceNumber = _guid,
                        Version = dto.VersionNumber,
                        URL = url,
                        HttpOption = httpOption,
                        Remark = remark,
                        Tag = tag,
                        ParamInfo = paraInfo,
                        ActionName = actionName,
                        SVId = GetMD5(string.Join("_", array)),
                        CreatedBy = "tzj",
                        CreatedTime = DateTime.Now,
                        PublishTime = dto.PublishTime,
                        ModifiedBy = "tzj",
                        ModifiedTime = DateTime.Now,
                    });
                }
            }          
          
            return versions;          
        }

        public static string GetMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
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


        public static VersionHistory GetVersionHistory(this RegisterDto dto)
        {
            return new VersionHistory
            {
                ServiceNumber = _guid,
                Version = dto.VersionNumber,
                DocJson = _docJson
            };
        }
                   
        private static void SetDocJson(string path)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    _docJson = client.DownloadString(path);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }          
        }                     
    }
}