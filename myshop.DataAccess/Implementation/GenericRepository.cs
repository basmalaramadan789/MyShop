using Microsoft.EntityFrameworkCore;
using myshop.Enteties.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet=_context.Set<T>();
        }

        public void AddOne(T entity)
        {
            //_context.Add(Category)
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? pridicate = null, string? Includeword=null)
        {
            IQueryable<T> query = _dbSet;
            if(pridicate != null)
            {
                query = query.Where(pridicate);
            }
            if(Includeword != null)
            {
                //context.category.include("prodect,users,logos")
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(item);

                }

             
            }
            return query.ToList();

        }

        public T GetOne(Expression<Func<T, bool>>? pridicate = null, string? Includeword = null)
        {
            IQueryable<T> query = _dbSet;
            if (pridicate != null)
            {
                query = query.Where(pridicate);
            }
            if (Includeword != null)
            {
                //context.category.include("prodect,users,logos")
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);

                }


            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
