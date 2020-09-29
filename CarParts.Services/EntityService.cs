using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using CarParts.Repository;

namespace CarParts.Services
{
    public class EntityService<T> : IEntityService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public EntityService(DbContext context)
        {
            _repository = new GenericRepository<T>(context);
        }

        public virtual bool DoesExist(Expression<Func<T, bool>> expression)
        {
            return _repository.DoesExist(expression);
        }

        public virtual void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public virtual T Save(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            return entity;
        }


        public virtual bool Update(T entity, List<object> avoidedProperties)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Edit(entity, avoidedProperties);
            return true;

        }

        public virtual bool Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            return true;
        }

        public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("entity");
            _repository.DeleteRange(predicate);
            return true;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual T Details(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("entity");
            return _repository.Details(predicate);
        }
    }

    public interface IEntityService<T> where T : class
    {
        T Save(T entity);
        T Details(Expression<Func<T, bool>> predicate);
        bool Delete(T entity);
        bool DeleteRange(Expression<Func<T, bool>> predicate);
        bool DoesExist(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        bool Update(T entity, List<object> avoidedProperties);
        void SaveChanges();
    }
}
