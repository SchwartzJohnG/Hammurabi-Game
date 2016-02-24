using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace CoderCamps.Data.Repository {

    public class Repository : IRepository {

        private DbContext _db;

        public Repository(DbContext db) {
            _db = db;
        }

        /// <summary>
        /// Query the specified set
        /// </summary>
        public IQueryable<T> Query<T>() where T : class {
            return _db.Set<T>().AsQueryable();
        }

        /// <summary>
        /// Adds the specified entity to the correct set. Crud
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add<T>(T entity) where T : class {
            _db.Set<T>().Add(entity);
        }

        /// <summary>
        /// Finds the specified entity. cRud
        /// </summary>
        /// <param name="keyValues">The value of the key fields.</param>
        public T Find<T>(params object[] keyValues) where T : class {
            return _db.Set<T>().Find(keyValues);
        }

        /// <summary>
        /// Saves the changes. crUd
        /// </summary>
        public void SaveChanges() {
            try {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException e) {
                var firstError = e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
                throw new ValidationException(firstError, e);
            }
        }

        /// <summary>
        /// Finds and deletes the specified entity. cruD
        /// </summary>
        public void Delete<T>(params object[] keyValues) where T : class {
            var entity = Find<T>(keyValues);
            Delete(entity);
        }

        /// <summary>
        /// Deletes the specified entity. cruD
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete<T>(T entity) where T : class {
            _db.Set<T>().Remove(entity);
        }

        public void Dispose() {
            _db.Dispose();
        }
    }

    public static class GenericRepositoryExtensions {
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> queryable, Expression<Func<T, TProperty>> relation) {
            return System.Data.Entity.QueryableExtensions.Include<T, TProperty>(queryable, relation);
        }
    }

    public interface IRepository : IDisposable {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Delete<T>(params object[] keyValues) where T : class;
        T Find<T>(params object[] keyValues) where T : class;
        IQueryable<T> Query<T>() where T : class;
        void SaveChanges();
    }
}