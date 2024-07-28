using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myshop.Enteties.Models;
using myshop.Enteties.ViewModel;


namespace myshop.DataAccess
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ):base(options) 
        {
            
        }

        public  DbSet<Category> categories { get; set; }
        public  DbSet<Product> products { get; set; }
        public  DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public  DbSet<ShoppingCart> shoppingCarts { get; set; }
        public  DbSet<OrderHeader> OrderHeaders { get; set; }
        public  DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
