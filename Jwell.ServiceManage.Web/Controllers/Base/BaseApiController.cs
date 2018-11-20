using System;
using System.Web.Http;
using Jwell.Framework.Mvc;

namespace Jwell.ServiceManage.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        [HttpPost]
        public StandardJsonResult StandardAction(Action action)
        {
            var result = new StandardJsonResult();
            result.StandardAction(action);
            return result;
        }

        [HttpPost]
        public StandardJsonResult<T> StandardAction<T>(Func<T> action)
        {
            var result = new StandardJsonResult<T>();
            result.StandardAction(() =>
            {
                result.Data = action();
            });
            return result;
        }
    }
}