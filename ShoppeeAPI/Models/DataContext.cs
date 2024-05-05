using Microsoft.EntityFrameworkCore;

namespace ShoppeeAPI.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {            
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new List<Product>
            {
                new Product { Id = 1,Name="Pepsi",Price=15,Description="Default",Quanity=200,Image="pepsi.jpg",Status=0},
                new Product { Id = 2,Name="Coca",Price=10,Description="Default",Quanity=255,Image="coca.jpg",Status=0},
                new Product { Id = 3,Name="Headphone",Price=444,Description="Default",Quanity=30,Image="headphone.jpg",Status=0},
                new Product { Id = 4,Name="Jacket",Price=70,Description="Default",Quanity=122,Image="jacket.jpg",Status=0},
            });
            modelBuilder.Entity<Customer>().HasData(new List<Customer>
            {
                new Customer{ Id=1,Fullname="Van A",Username="customer1",Password="123",Address="HCM",PhoneNumber="0707xxxx",Status=0},
                new Customer{ Id=2,Fullname="Van B",Username="customer2",Password="123",Address="HN",PhoneNumber="0722xxxx",Status=0},
                new Customer{ Id=3,Fullname="Van C",Username="customer3",Password="123",Address="DN",PhoneNumber="0377xxxx",Status=0},
            });
        }
    }
}
