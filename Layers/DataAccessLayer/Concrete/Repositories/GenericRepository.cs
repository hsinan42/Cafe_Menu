using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        Context c = new Context();
        DbSet<T> _object;

        public GenericRepository()
        {
            _object = c.Set<T>();
        }

        public void Add(T item)
        {
            var addEntity = c.Entry(item);
            addEntity.State = EntityState.Added;
            c.SaveChanges();
        }

        public void Delete(T item)
        {
            var deleteEntity = c.Entry(item);
            deleteEntity.State = EntityState.Deleted;
            c.SaveChanges();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return _object.FirstOrDefault(filter);
        }

        public List<T> List()
        {
            return _object.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter).ToList();
        }

        public void Update(T item)
        {
            var updateEntity = c.Entry(item);
            updateEntity.State = EntityState.Modified;
            c.SaveChanges();
        }
    }
}
