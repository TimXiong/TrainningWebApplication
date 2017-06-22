using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainningData;

namespace TrainningWebApplication
{
    public class ShoppingCart
    {
		public List<LineItem> LineItems { get; set; } = new List<LineItem>();

		public string FormattedGradTotal
		{
			get
			{
				return $"{ LineItems.Sum(i => i.TotalCost):C}";
			}
		}

		public static ShoppingCart GetFormSession(ISession session)
		{
			byte[] data = null;
			ShoppingCart cart = null;
			try
			{
				bool b = session?.TryGetValue("ShoppingCart", out data) ?? false;
				if (b)
				{
					cart = JsonConvert.DeserializeObject<ShoppingCart>(Encoding.UTF8.GetString(data));
				}
			}
			catch { }
			return cart ?? new ShoppingCart();
		}

		public static void StoreInSession(ShoppingCart cart, ISession session)
		{
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cart));
			session.Set("ShoppingCart", data);
		}

		public class LineItem
		{
			public Product Product { get; set; }

			public int Quantity { get; set; }

			public decimal TotalCost
			{
				get
				{
					return this.Product?.UnitPrice.Value * Quantity ?? 0;
				}	 
			}
		}
    }
}
