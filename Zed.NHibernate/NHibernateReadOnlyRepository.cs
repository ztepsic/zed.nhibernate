using System.Collections.Generic;
using NHibernate;
using Zed.Domain;

namespace Zed.NHibernate {

    /// <summary>
    /// NHibernate read-only repository for Entity/Aggregate root with int as identifier type.
    /// </summary>
    /// <typeparam name="TEntity">Entity/Aggregate root type</typeparam>
    public class NHibernateReadOnlyRepository<TEntity> :
        NHibernateReadOnlyRepository<TEntity, int>,
        IReadOnlyRepository<TEntity> where TEntity : Entity {

        /// <summary>
        /// Creates NHibernate read-only repository
        /// </summary>
        /// <param name="sessionFactory">Session factory</param>
        public NHibernateReadOnlyRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }
    }

    /// <summary>
    /// NHibernate read-only repository
    /// </summary>
    /// <typeparam name="TEntity">Entity/Aggregate root type</typeparam>
    /// <typeparam name="TId">Entity/Aggregate root identifier type</typeparam>
    public class NHibernateReadOnlyRepository<TEntity, TId> :
        NHibernateRepository, IReadOnlyRepository<TEntity, TId> where TEntity : Entity<TId> {

        #region Fields and Properties
        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate read-only repository
        /// </summary>
        /// <param name="sessionFactory">Session factory</param>
        public NHibernateReadOnlyRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all persisted entities/aggregate roots
        /// </summary>
        /// <returns>All persisted entities/aggregate roots</returns>
        public virtual IEnumerable<TEntity> GetAll() {
            ICriteria criteria = Session.CreateCriteria(typeof(TEntity));
            return criteria.List<TEntity>();
        }

        /// <summary>
        /// Gets entity/aggregate root bases on it's identity.
        /// </summary>
        /// <param name="id">Entity/Aggregat root identifier</param>
        /// <returns>Entity/aggregate root</returns>
        public virtual TEntity GetById(TId id) {
            return Session.Get<TEntity>(id);
        }

        #endregion

    }
}
