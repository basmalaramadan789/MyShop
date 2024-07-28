using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Enteties.Repositories
{
    public  interface IGenericRepository<T> where T : class
    {//if i want to use include or where
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? pridicate=null,string? Includeword= null);
        //if i want use firstordefault
        T GetOne(Expression<Func<T, bool>>? pridicate=null, string? Includeword= null);

        void AddOne(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
