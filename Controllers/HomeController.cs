using KaloriApp.Models;
using KaloriApp.ApiHelper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace KaloriApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        String uri = "http://localhost:3000/calculate_calorie_content/";
        HttpClient client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = new Uri(uri);
        }

        /// <summary>
        /// Call the api of a food item and return
        /// </summary>
        /// <param name="food">Get information about this food</param>
        /// <returns>Information about a food item</returns>
        private CalorieContent GetAPI(string food)
        {
            CalorieContent foodItem = new CalorieContent();

            // Call the api and handle exceptions
            try
            {
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + food).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<CalorieContent>(responseBody);
                    if (data is not null) { foodItem = data; }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return foodItem;
        }

        //Verify that string is not null and is lowercase
        private string verifyInput(string test)
        {
            string verified = "";

            //If not null lower case
            if (test is not null) 
            {
                verified = test.ToLower();
            }

            return verified;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Calculates calories on a particular food
        /// </summary>
        /// <param name="caloriesViewModel">A food and its weight</param>
        /// <returns>Calorie content of a food</returns>
        public IActionResult Calories(CaloriesViewModel caloriesViewModel)
        {
            string food = verifyInput(caloriesViewModel.Food);
            CalorieContent foodItem = GetAPI(food);
            
            // Calculate and set the calorie content of a food
            float kcal = (caloriesViewModel.Gram) * ( (float)foodItem.kcal / (float)100 );
            Console.WriteLine(kcal);
            caloriesViewModel.Calorie = (int)kcal;

            return View(caloriesViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}