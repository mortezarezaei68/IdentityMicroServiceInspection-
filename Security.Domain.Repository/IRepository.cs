using Security.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Repository
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        T Get(object id);
        IEnumerable<T> getList();
        void insert(T t);
        void Delete(object id);
        void Update(T t);
        void Save();
    }
}
