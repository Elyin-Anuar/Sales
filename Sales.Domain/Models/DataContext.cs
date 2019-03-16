namespace Sales.Domain.Models
{
    using System.Data.Entity;
    using Sales.Common.Models;

    public class DataContext: DbContext
    {
        public DataContext(): base("DefaultConnection")
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
