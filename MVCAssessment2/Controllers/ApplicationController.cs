using Microsoft.AspNetCore.Mvc;

namespace MVCAssessment2.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Application()
        {
            return View();
        }

        public IActionResult Successful()
        {
            return View();
        }

        public IActionResult Unsuccessful()
        {
            return View();
        }
    }
}
