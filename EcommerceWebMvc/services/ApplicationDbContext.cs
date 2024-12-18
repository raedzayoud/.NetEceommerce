using EcommerceWebMvc.Models;
using EcommerceWebMvc.Models.BestStoreMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebMvc.services
{
    //This class used to manipulate the data  
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        //we use migration to create the tables

        public DbSet<Product>Products { get; set; }
        public DbSet<Order> Orders { get; set; }


	}
}
