using System.Web.Mvc;

namespace TestTarget.AspNetMvc
{
    public abstract class BaseController : Controller
    {
    }

    public class MyController1 : BaseController
    {
        public ActionResult MyAction()
        {
            return Content("aaa");
        }
    }

    public class NotAController
    {
        public ActionResult MyAction()
        {
            return null;
        }
    }
}
