using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainningData;
using TrainningWebApplication.Controllers;
using Xunit;

namespace TrainingXUnitTestProject
{
	public class UnitTest
    {
        [Fact]
        public void PassingTest()
        {
			Assert.Equal(4, (2 + 2));
        }

		[Fact]
		public void ProductDetails()
		{
			var controller = new ProductController(CreateStubContext());
			var result = controller.Detail(2);
			Assert.IsAssignableFrom<ViewResult>(result);
			var vr = result as ViewResult;
			Assert.IsAssignableFrom<Product>(vr.Model);
			var model = vr.Model as Product;
			Assert.Equal("Bread", model.ProductName);
		}

		DataContext CreateStubContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			optionsBuilder.UseInMemoryDatabase();
			var context = new DataContext(optionsBuilder.Options);
			context.Products.Add(new Product { Id = 1, ProductName = "Milk", UnitPrice = 2.5M });
			context.Products.Add(new Product { Id = 2, ProductName = "Bread", UnitPrice = 3.5M });
			context.SaveChanges();
			return context;
		}
	}
}
