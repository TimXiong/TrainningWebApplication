using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainningData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TrainningWebApplication.Controllers
{
    public class ProductController : Controller
    {
		DataContext datacontext;

		public ProductController(DataContext context)
		{
			datacontext = context;
		}

        public IActionResult Index()
        {
            return View();
        }

		[Route("product/{id:int}")]
		[Authorize(Policy="AdminsOnly")]
		public IActionResult Detail(int id)
		{
			var product = datacontext.Products.Include(p => p.Supplier).FirstOrDefault(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		[HttpPost]
		public IActionResult AddToCart(int id, int quantity)
		{
			var product = datacontext.Products.FirstOrDefault(p => p.Id == id);
			var totalCost = quantity * product.UnitPrice;
			string message = $"You added { product.ProductName } (* { quantity }) to your cart at a total cost of { totalCost?.ToString("C")?? "0" }. ";
			var cart = ShoppingCart.GetFormSession(HttpContext.Session);
			var lineItem = cart?.LineItems?.FirstOrDefault(i => i?.Product?.Id == id);
			if (lineItem != null)
			{
				lineItem.Quantity += quantity;
			}
			else
			{
				cart.LineItems.Add(new ShoppingCart.LineItem { Product= product, Quantity= quantity });
			}
			ShoppingCart.StoreInSession(cart, HttpContext.Session);
			return PartialView("_AddedToCart", message);
		}

    }

	public class ProductList : ViewComponent
	{
		DataContext datacontext;

		public ProductList(DataContext context)
		{
			datacontext = context;
		}

		public IViewComponentResult Invoke()
		{
			var model = datacontext.Products.ToList();
			return View("~/Views/Shared/_ProductList.cshtml", model);
		}
	}
}