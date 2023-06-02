using Microsoft.AspNetCore.Mvc;
using MVCAssessment2.Models;

namespace MVCAssessment2.Controllers
{
    public class ApplicationController : Controller
    {
        public Applicant application { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
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
