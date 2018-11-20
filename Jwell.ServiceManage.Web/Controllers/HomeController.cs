using System.Web.Mvc;

namespace Jwell.ServiceManage.Web.Controllers
{
    public class HomeController : BaseController
    {      
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }      
    }
}
