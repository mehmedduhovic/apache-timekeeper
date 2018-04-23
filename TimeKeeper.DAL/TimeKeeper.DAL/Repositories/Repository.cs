using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TimeKeeper.DAL.Repositories
{
    public class Repository<T, K> : IRepository<T, K> where T : class
    {
        protected TimeKeeperDbContext context;
        protected DbSet<T> dbSet;

        public Repository(TimeKeeperDbContext _context)
        {
            context = _context;
            dbSet = context.Set<T>();
        }

        public void Delete(T entity)
        {
            Utility.Log($"FROM REPOSITORY: Delete() called.", "INFO");
            dbSet.Remove(entity);
        }

        public void Delete(K id)
        {
            Utility.Log($"FROM REPOSITORY: Delete() with {id} called.", "INFO");
            dbSet.Remove(Get(id));
        }

        public IQueryable<T> Get()
        {
            Utility.Log("FROM REPOSITORY: Get() called.", "INFO");
            return dbSet;
        }

        public T Get(K id)
        {
            Utility.Log($"FROM REPOSITORY: Get() with {id} called.", "INFO");
            return dbSet.Find(id);
        }

        public List<T> GetList(Func<T, bool> where)
        {
            Utility.Log($"FROM REPOSITORY: Get() with function called.", "INFO");
            return dbSet.Where(where).ToList();
        }

        public virtual void Insert(T entity)
        {
            Utility.Log("FROM REPOSITORY: Insert() called.", "INFO");
            dbSet.Add(entity);
        }

        public virtual void Update(T entity, K id)
        {
            T old = Get(id);

            if (old != null)
            {
                Utility.Log("FROM REPOSITORY: Update() called.", "INFO");

                context.Entry(old).CurrentValues.SetValues(entity);
            }
            else
            {
                Utility.Log("FROM REPOSITORY: Update() cannot be called ond this ID.", "ERROR");
            }
        }
    }
}
