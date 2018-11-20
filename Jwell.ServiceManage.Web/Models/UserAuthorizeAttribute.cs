﻿using Jwell.Modules.Configure;
using Jwell.ServiceManage.Web.Common;
using System.Linq;
using System.Web.Mvc;

namespace Jwell.ServiceManage.Web.Models
{
    /// <summary>
    /// token验证
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 进行页面验证，查看Token是否存在
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(false).Any(g => g.GetType() == typeof(AllowAnonymousAttribute)))
            {
                base.OnAuthorization(filterContext);
                return;
            }
            var token = filterContext.HttpContext.Request.Cookies[JwellConfig.GetAppSetting("AccessToken")];
            if (token != null && !string.IsNullOrEmpty(token.Value)) return;
            var returnUri = filterContext.HttpContext.Request.Url.ToString();
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(OAuthHelper.GenerateLoginUrl(urlHelper.Action("AuthorizationCallBack", "OAuth", null, filterContext.HttpContext.Request.Url.Scheme), returnUri));
        }
    }
}