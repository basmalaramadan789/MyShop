using myshop.Enteties.Models;
using myshop.Enteties.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Enteties.Repositories
{
    public interface IShoppingCartRepository:IGenericRepository<ShoppingCart>
    {
        int IncreaseCount(ShoppingCart shoppingcart,int Count);
        int DecreaseCount(ShoppingCart shoppingcart,int Count);
    }
}
