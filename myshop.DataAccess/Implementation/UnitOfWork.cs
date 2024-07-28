using myshop.Enteties.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository product { get; private set; }
        public IShoppingCartRepository shoppingCart { get; private set; }
		public IOrderHeaderRepository orderHeader { get; private set; }

		public IOrderDetailsRepository orderDetails { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }



        public UnitOfWork(ApplicationDbContext context) 
        {
            
            _context = context;
            Category=new CategoryRepository(context);
            product=new ProductRepository(context);
            shoppingCart= new ShoppingCartRepository(context);
            orderHeader= new OrderHeaderRepository(context);
            orderDetails= new OrderDetailsRepository(context);
            ApplicationUser=new ApplicationUserRepository(context);

            

        }
       

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
