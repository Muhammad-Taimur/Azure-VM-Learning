using ColorWebsite.Data.Services;
using System;
using System.Web.Mvc;

namespace ColorWebsite.Controllers
{
    public class HomeController : Controller
    {
        private ColorService _colorService;

        public HomeController()
        {
            _colorService = new ColorService();
        }

        public ActionResult Index()
        {
            var model =_colorService.GetAllColors();
            
            return View(model);
        }
    }
}