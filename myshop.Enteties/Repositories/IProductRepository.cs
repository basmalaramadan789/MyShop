using myshop.Enteties.Models;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Enteties.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
