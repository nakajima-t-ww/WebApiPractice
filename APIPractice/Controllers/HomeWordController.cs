using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        // /HelloWorld にアクセスした時の処理
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = $"こんにちは！{name}さん";
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}