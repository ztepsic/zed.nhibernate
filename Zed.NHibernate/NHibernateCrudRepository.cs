using NHibernate;
using Zed.Domain;

namespace Zed.NHibernate {

    /// <summary>
    /// NHibernate repository with CRUD operations for Entity/Aggregate root with int as identifier type.
    /// </summary>
    /// <typeparam name="TEntity">Entity/Aggregate root type</typeparam>
    public class NHibernateCrudRepository<TEntity> :
        NHibernateCrudRepository<TEntity, int>,
        ICrudRepository<TEntity> where TEntity : Entity {

        /// <summary>
        /// Creates NHibernate CRUD repository
        /// </summary>
        /// <param name="sessionFactory">Session factory</param>
        public NHibernateCrudRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

    }

    /// <summary>
    /// NHibernate repository with CRUD operations for Entity/Aggregate root
    /// </summary>
    /// <typeparam name="TEntity">Entity/Aggregate root type</typeparam>
    /// <typeparam name="TId">Entity/Aggregate root identifier type</typeparam>
    public class NHibernateCrudRepository<TEntity, TId> :
        NHibernateReadOnlyRepository<TEntity, TId>,
        ICrudRepository<TEntity, TId> where TEntity : Entity<TId> {

        #region Members
        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate CRUD repository
        /// </summary>
        /// <param name="sessionFactory">Session factory</param>
        public NHibernateCrudRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

        #endregion

        #region Methods

        /// <summary>
        /// Saves a new or updates an existing entity/aggregate from the repository
        /// </summary>
        /// <param name="entity">Entity/aggregate root which is saved or updated</param>
        public void SaveOrUpdate(TEntity entity) {
            Session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Deletes the provided entity/aggregate root from the repository.
        /// </summary>
        /// <param name="entity">Entity/aggregate which needs to be deleted.</param>
        public void Delete(TEntity entity) {
            Session.Delete(entity);
        }

        #endregion


    }
}
