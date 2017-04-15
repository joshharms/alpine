using System;
using System.Collections.Generic;
using System.Linq;

namespace alpine.repository
{
     public interface IRepository<T>
     {
          T Get<TKey>(TKey id);
          IQueryable<T> GetAll();
          void Add(T entity);
          void Update(T entity);
          void Remove<TKey>(TKey id);
     }
}
