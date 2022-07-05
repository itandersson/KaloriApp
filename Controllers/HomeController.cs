using KaloriApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KaloriApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calories(CaloriesViewModel calories)
        {
            //CaloriesViewModel calories = new CaloriesViewModel();
            //calories.Gram = 100;
            //calories.Food = "Socker";
            calories.Calorie = 400;

            return View(calories);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}