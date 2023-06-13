using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        public T? GetById(int id);
        public IEnumerable<T> GetAll();
        public T Add(T entity);
        public T Update(T entity);
        public bool Delete(int id);
        public void SaveChanges();
    }

}
