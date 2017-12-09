using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        /// This is the asynchronous version of <see cref="GetAll()"/>.
        /// Gets all persisted entities/aggregate roots
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>All persisted entities/aggregate roots</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            ICriteria criteria = Session.CreateCriteria(typeof(TEntity));
            return await criteria.ListAsync<TEntity>(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="GetAll()"/>.
        /// Gets all persisted entities/aggregate roots
        /// </summary>
        /// <returns>All persisted entities/aggregate roots</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() {
            return await GetAllAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets entity/aggregate root bases on it's identity.
        /// </summary>
        /// <param name="id">Entity/Aggregat root identifier</param>
        /// <returns>Entity/aggregate root</returns>
        public virtual TEntity GetById(TId id) {
            return Session.Get<TEntity>(id);
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="GetById"/>.
        /// Gets entity/aggregate root bases on it's identity.
        /// </summary>
        /// <param name="id">Entity/Aggregat root identifier</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Entity/aggregate root</returns>
        public virtual async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken) {
            return await Session.GetAsync<TEntity>(id, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets entity/aggregate root bases on it's identity.
        /// </summary>
        /// <param name="id">Entity/Aggregat root identifier</param>
        /// <returns>Entity/aggregate root</returns>
        public virtual async Task<TEntity> GetByIdAsync(TId id) {
            return await GetByIdAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        #endregion

    }
}
