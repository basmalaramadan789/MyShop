using myshop.Enteties.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Enteties.Repositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
