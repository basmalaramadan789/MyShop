using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Enteties.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository product { get; }
        IShoppingCartRepository shoppingCart { get; }
        IOrderHeaderRepository orderHeader { get; }
        IOrderDetailsRepository orderDetails { get; }
        IApplicationUserRepository ApplicationUser { get; }
        int Complete();
    }
}
