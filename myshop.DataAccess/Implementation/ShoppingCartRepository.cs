using myshop.Enteties.Models;
using myshop.Enteties.Repositories;
using myshop.Enteties.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            
                _context = context;
         
        }

        public int DecreaseCount(ShoppingCart shoppingcart, int Count)
        {
            shoppingcart.Count -= Count;
            return shoppingcart.Count;
        }

        public int IncreaseCount(ShoppingCart shoppingcart, int Count)
        {
            shoppingcart.Count += Count;
            return shoppingcart.Count;
        }
    }
}

