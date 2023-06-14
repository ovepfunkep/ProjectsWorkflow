using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private DbSet<T> _entities;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<T>();
        }

        public virtual T? GetById(int id) => _entities.Find(id);

        public virtual IEnumerable<T> GetAll() => _entities.ToList();

        public virtual T Add(T entity)
        {
            _entities.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public virtual T Update(T entity)
        {
            var entry = _entities.Update(entity);
            _dbContext.SaveChanges();
            return entry.Entity;
        }

        public virtual bool Delete(int id)
        {
            var entity = _entities.Find(id);
            if (entity == null)
            {
                return false;
            }

            _entities.Remove(entity);
            int affectedRows = _dbContext.SaveChanges();
            return affectedRows > 0;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
