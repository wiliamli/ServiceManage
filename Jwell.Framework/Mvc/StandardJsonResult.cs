using Jwell.Framework.Utilities;
using System;
using System.Text;
using System.Web.Mvc;

namespace Jwell.Framework.Mvc
{
    public class StandardJsonResult : ActionResult, IStandardResult
    {
        public string ContentType { get; set; }

        #region  统一JSON数据结构

        public bool Success { get; set; }

        public string Message { get; set; }

        public StandardJsonResult()
        {
            ContentType = "application/json";
        }


        public void StandardAction(Action action)
        {
            try
            {
                action();
                Success = true;
            }
            catch
            {
                Success = false;
                throw;
            }
        }

        #endregion

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            response.ContentEncoding = Encoding.UTF8;
            response.Write(Serializer.ToJson(ToJsonObject()));
        }

        protected virtual IStandardResult ToJsonObject()
        {
            return new StandardResult
            {
                Success = Success,
                Message = Message
            };
        }
    }

    public class StandardJsonResult<T> : StandardJsonResult, IStandardResult<T>
    {
        public T Data { get; set; }

        protected override IStandardResult ToJsonObject()
        {
            return new StandardResult<T>
            {
                Success = Success,
                Message = Message,
                Data = Data
            };
        }
    }
}