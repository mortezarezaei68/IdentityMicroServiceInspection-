using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Security.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Domain.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private AppDbContext _Context;
        private bool disposed = false;

        public Repository(AppDbContext Context)
        {
            _Context = Context;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Delete(object id)
        {
            T t = _Context.Set<T>().Find(id);
            _Context.Set<T>().Remove(t);
        }

        public T Get(object id)
        {
            return _Context.Set<T>().Find(id);
        }

        public IEnumerable<T> getList()
        {
            return _Context.Set<T>().ToList();
        }

        public void insert(T t)
        {
            _Context.Set<T>().Add(t);
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        public void Update(T t)
        {
            _Context.Entry(t).State = EntityState.Modified;
        }
    }
}
