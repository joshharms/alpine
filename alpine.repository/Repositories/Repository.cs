using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using alpine.database.Models;

namespace alpine.repository
{
     public class Repository<T> : IRepository<T> where T : class
     {
          protected readonly alpineContext context;
          protected DbSet<T> DbSet;

          public Repository(alpineContext alpineContext)
          {
               context = alpineContext;
               DbSet = context.Set<T>();
          }

          public void Add(T entity)
          {
               context.Set<T>().Add(entity);

               Save();
          }

          public T Get<TKey>(TKey id)
          {
               return DbSet.Find(id);
          }

          public IQueryable<T> GetAll()
          {
               return DbSet;
          }

          public void Update(T entity)
          {
               Save();
          }

          private void Save()
          {
               context.SaveChanges();
          }

          public void Remove<TKey>(TKey id)
          {
               context.Remove(DbSet.Find(id));

               Save();
          }
     }
}
