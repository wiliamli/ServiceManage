using Jwell.Modules.Cache;
using Jwell.Modules.DSFClient.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Jwell.Modules.DSFClient.BLL
{
    public class DSFClientBL
    {       
        public static string GetMD5(string input)
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

        /// <summary>
        /// 获取参数信息的字典
        /// </summary>
        /// <param name="paramInfo"></param>
        /// <returns></returns>
        private static Dictionary<string,string> GetParamDic(string paramInfo)
        {
            if (string.IsNullOrEmpty(paramInfo)) return null;

            paramInfo = RemoveCurlyBraces(paramInfo);

            if (string.IsNullOrEmpty(paramInfo)) return null;

            var nameTypePair = paramInfo.Split(',');

            return nameTypePair
                .Select(item => item.Split(':'))
                .ToDictionary(
                    nameTypeArray => nameTypeArray[0].Replace("\"", string.Empty).Replace("\"", string.Empty),
                    nameTypeArray => nameTypeArray[1].Replace("\"", string.Empty).Replace("\"", string.Empty)
                );
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="paramData">调用网关时传入的dynamic参数</param>
        /// <param name="paramInfo">从缓存或数据库中获取的参数信息</param>
        public static bool CheckParamInfo(dynamic paramData,string paramInfo)
        {
            var paramDic = GetParamDic(paramInfo);

            IDictionary<string, JToken> dict = paramData;

            foreach (var item in paramDic)
            {
                if (!dict.ContainsKey(item.Key)) return false;

                var value = dict[item.Key].ToString();
                switch (item.Value)
                {
                    case "integer":
                        if (!int.TryParse(value, out _)) return false;
                        break;
                    case "boolean":
                        if (!bool.TryParse(value, out _)) return false;
                        break;
                    case "float":
                        if (!float.TryParse(value, out _)) return false;
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 去除paramInfo左右两端的花括号
        /// </summary>
        /// <param name="paramInfo"></param>
        /// <returns></returns>
        private static string RemoveCurlyBraces(string paramInfo)
        {
            paramInfo = paramInfo.Substring(1, paramInfo.Length - 1);
            paramInfo = paramInfo.Substring(0, paramInfo.Length - 1);
            return paramInfo;
        }

        /// <summary>
        /// 解析Get请求参数
        /// </summary>
        /// <param name="url">被访问服务的URL</param>
        /// <param name="paramInfo">访问参数</param>
        /// <returns></returns>
        public static string ParseGetRequest(string url,string paramInfo)
        {
            if (!string.IsNullOrEmpty(paramInfo))
            {
                paramInfo = RemoveCurlyBraces(paramInfo);

                if (!string.IsNullOrEmpty(paramInfo))
                {
                    var properties = paramInfo.Split(',');

                    url += "?";

                    foreach (var property in properties)
                    {
                        var keyValuePair = property.Split(':');
                        url += keyValuePair[0] + "=" + keyValuePair[1] + "&";
                    }

                    //去除最后一个"&"
                    url = url.Substring(0, url.Length - 1);
                    //去除换行符
                    url = url.Replace(Environment.NewLine, string.Empty);
                    //去除双引号
                    url = url.Replace("\"", string.Empty);
                    //去除空格
                    url = url.Replace(" ", string.Empty);
                }                
            }                                           
            return url;
        }
    }
}