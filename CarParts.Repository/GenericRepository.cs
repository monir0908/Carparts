using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CarParts.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext Entities;
        protected readonly IDbSet<T> Dbset;

        public GenericRepository(DbContext context)
        {
            Entities = context;
            Dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Dbset.AsEnumerable<T>();
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = Dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Details(Expression<Func<T, bool>> predicate)
        {
            return Dbset.FirstOrDefault(predicate);
        }

        public virtual bool DoesExist(Expression<Func<T, bool>> expression)
        {
            return Dbset.AsNoTracking().FirstOrDefault(expression) != null;
        }

        public virtual T Add(T entity)
        {
            return Dbset.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Entities.Entry(entity).State = EntityState.Deleted;
        }

        private void AvoidPropertyToModify(T entity, List<object> avoidedProperties)
        {
            if (avoidedProperties != null)
            {
                foreach (var item in avoidedProperties)
                {
                    foreach (PropertyInfo p in item.GetType().GetProperties())
                        Entities.Entry<T>(entity).Property(p.Name).IsModified = false;
                }
            }
        }

        public virtual void DeleteRange(Expression<Func<T, bool>> predicate)
        {
            var details = Dbset.Where(predicate).AsEnumerable();
            if (details.Count() > 0)
            {
                Entities.Set<T>().RemoveRange(details);
            }
        }

        public virtual void Edit(T entity, List<object> avoidedProperties)
        {
            Entities.Entry(entity).State = EntityState.Modified;
            AvoidPropertyToModify(entity, avoidedProperties);
        }

        public virtual void SaveChanges()
        {
            Entities.SaveChanges();
        }
    }

    public interface IGenericRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Details(Expression<Func<T, bool>> predicate);
        bool DoesExist(Expression<Func<T, bool>> expression);
        void DeleteRange(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Delete(T entity);
        void Edit(T entity, List<object> avoidedProperties);
        void SaveChanges();
    }
}
