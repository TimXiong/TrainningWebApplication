using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TrainningData
{
	public class DataContext : DbContext
    {
		public DataContext(DbContextOptions options)
			:base(options)
		{ }

		public DbSet<Customer> Customers { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderItem> Items { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Supplier> Suppliers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				entity.Relational().TableName = entity.DisplayName();
			}
			base.OnModelCreating(modelBuilder);
		}
	}
}
