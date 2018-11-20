using System.Web.Mvc;

namespace Jwell.ServiceManage.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // filters.Add(new ExceptionHandleAttribute());
        }
    }
}
