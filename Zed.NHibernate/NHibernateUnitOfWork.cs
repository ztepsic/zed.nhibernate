using NHibernate;
using NHibernate.Context;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zed.Transaction;

namespace Zed.NHibernate {
    /// <summary>
    /// NHibernate unit of work
    /// </summary>
    /// <remarks>Based on article: http://www.planetgeek.ch/2012/05/05/what-is-that-all-about-the-repository-anti-pattern/ </remarks>
    public class NHibernateUnitOfWork : IUnitOfWork {

        #region Fields and Properties

        private readonly ISessionFactory sessionFactory;
        private readonly Func<IUnitOfWorkScope> rootScopeFactory;
        private readonly Func<IUnitOfWorkScope> dependentScopeFactory;

        /// <summary>
        /// An indication if implicit transactions are enabled
        /// </summary>
        private readonly bool isImplicitTransactionsEnabled;

        /// <summary>
        /// Gets an indication if implicit transactions are enabled
        /// </summary>
        public bool IsImplicitTransactionsEnabled => isImplicitTransactionsEnabled;

        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate Unit of Work with default root and dependent scopes
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        /// <param name="isImplicitTransactionsEnabled">An indication if implicit transactions are enabled. Default is false.</param>
        public NHibernateUnitOfWork(ISessionFactory sessionFactory, bool isImplicitTransactionsEnabled = false) 
            : this(sessionFactory,
            () => new NHibernateUnitOfWorkRootScope(sessionFactory, isImplicitTransactionsEnabled),
            () => new NHibernateUnitOfWorkScope(sessionFactory, isImplicitTransactionsEnabled),
            isImplicitTransactionsEnabled) { }

        /// <summary>
        /// Creates NHibernate Unit of Work
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        /// <param name="rootScopeFactory">Root transaction scope</param>
        /// <param name="dependentScopeFactory">Dependant transaction scope</param>
        /// <param name="isImplicitTransactionsEnabled">An indication if implicit transactions are enabled. Default is false.</param>
        public NHibernateUnitOfWork(ISessionFactory sessionFactory, Func<IUnitOfWorkScope> rootScopeFactory, Func<IUnitOfWorkScope> dependentScopeFactory, bool isImplicitTransactionsEnabled = false) {
            this.sessionFactory = sessionFactory;
            this.rootScopeFactory = rootScopeFactory;
            this.dependentScopeFactory = dependentScopeFactory;
            this.isImplicitTransactionsEnabled = isImplicitTransactionsEnabled;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts unit of work scope
        /// </summary>
        /// <returns>Unit of work scope</returns>
        public IUnitOfWorkScope Start() {
            IUnitOfWorkScope scope = !CurrentSessionContext.HasBind(sessionFactory)
                ? rootScopeFactory()
                : dependentScopeFactory();

            //if (IsImplicitTransactionsEnabled) { scope.BeginTransaction(); }
            scope.BeginTransaction();

            return scope;
        }

        /// <summary>
        /// Starts async unit of work scope
        /// </summary>
        /// <returns>Unit of work scope</returns>
        public async Task<IUnitOfWorkScope> StartAsync() {
            return await StartAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts async unit of work scope
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Unit of work scope</returns>
        public async Task<IUnitOfWorkScope> StartAsync(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            IUnitOfWorkScope scope = !CurrentSessionContext.HasBind(sessionFactory)
                ? rootScopeFactory()
                : dependentScopeFactory();

            await scope.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            return scope;
        }

        #endregion

    }
}
