using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrainningData;

namespace TrainningWebApplication.Controllers
{
    public class HomeController : Controller
    {
		DataContext dbContext;

		public HomeController(DataContext context)
		{
			this.dbContext = context;
		}

		public IActionResult Index()
		{
			return View();
			//return Content("From my index. ");
			//return Content("<h1>From</h1> my index. ", "text/html");
			//return Content("{ \"hello \" : \" Tim \" }", "application/json");
			//return Content("{ \"hello \" : \" Tim \" }");
			//return Content($" Number of product : {dbContext.Products.Count()}");
		}

		//public IActionResult Index()
		//{
		//	return View(dbContext.Products.ToList());
		//}

		public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

		public IActionResult Cart()
		{
			var cart = ShoppingCart.GetFormSession(HttpContext.Session);
			var cartView = new CartViewModel() { Car = cart };
			return View(cartView);
		}

		[HttpPost]
		public IActionResult Checkout(CartViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Car = ShoppingCart.GetFormSession(HttpContext.Session);
				return View("Cart", model);
			}
			HttpContext.Session.Clear();
			return View("Thanks");
		}
	}
}
