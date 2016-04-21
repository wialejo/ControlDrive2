using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Repositorios
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
            where T : class,  new()
    {

        private ApplicationDbContext dataContext;

        #region Properties
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected ApplicationDbContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        public EntityBaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        #endregion
        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }
        public virtual IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }
        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public virtual IQueryable<T> FindByIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(predicate);
        }
        //public T GetSingle(int id)
        //{
        //    return GetAll().FirstOrDefault(x => x. == id);
        //}
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        public virtual T Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            DbContext.Set<T>().Add(entity);
            return entity;
        }

        //public virtual void Add(T entity)
        //{
        //    DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
        //    DbContext.Set<T>().Add(entity);
        //}
        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
        public virtual void Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            //dbEntityEntry.State = EntityState.Modified; --- I cannot do this.
            DbContext.Set<T>().Attach(entity);
            if (properties.Length > 0)
            {
                foreach (var propertyAccessor in properties)
                {
                    DbContext.Entry(entity).Property(propertyAccessor).IsModified = true;
                }
            }
            else
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            //var dbEntityEntry = DbContext.Entry(entity);
            //if (updatedProperties.Any())
            //{
            //    //update explicitly mentioned properties
            //    foreach (var property in updatedProperties)
            //    {
            //        dbEntityEntry.Property(property).IsModified = true;
            //    }
            //}
            //else
            //{
            //    //no items mentioned, so find out the updated entries
            //    foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
            //    {
            //        var original = dbEntityEntry.OriginalValues.GetValue<object>(property);
            //        var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
            //        if (original != null && !original.Equals(current))
            //            dbEntityEntry.Property(property).IsModified = true;
            //    }
            //}
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }
    }

    public interface IEntityBaseRepository<T> where T : class, new()
    {
        IQueryable<T> FindByIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> All { get; }
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Update(T entity, params Expression<Func<T, object>>[] updatedProperties);
    }
}
