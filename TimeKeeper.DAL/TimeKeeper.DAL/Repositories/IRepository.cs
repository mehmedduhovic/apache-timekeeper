using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Repositories
{
    public interface IRepository<T, K>
    {
        IQueryable<T> Get();
        List<T> GetList(Func<T, bool> where);
        T Get(K id);

        void Insert(T entity);
        void Update(T entity, K id);
        void Delete(T entity);
        void Delete(K id);
    }
}
