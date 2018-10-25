using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using Zed.Transaction;

namespace Zed.NHibernate {
    /// <summary>
    /// NHibernate Unit Of Work scope
    /// </summary>
    /// <remarks>Based on article: http://www.planetgeek.ch/2012/05/05/what-is-that-all-about-the-repository-anti-pattern/ </remarks>
    public class NHibernateUnitOfWorkScope : IUnitOfWorkScope {

        #region Fields and Properties

        /// <summary>
        /// NHibernate session factory
        /// </summary>
        private readonly ISessionFactory sessionFactory;

        /// <summary>
        /// Gets NHibernate session factory
        /// </summary>
        protected ISessionFactory SessionFactory { get { return sessionFactory; } }

        /// <summary>
        /// Gets NHibernate current session
        /// </summary>
        protected ISession Session { get { return sessionFactory.GetCurrentSession(); } }

        /// <summary>
        /// Gets NHibernate transaction
        /// </summary>
        protected ITransaction Transaction { get { return Session.Transaction; } }

        /// <summary>
        /// Indicates if transaction is created
        /// </summary>
        private bool isTransactionCreated;

        /// <summary>
        /// Indicates if transaction scope is completed
        /// </summary>
        private bool isScopeCompleted;

        /// <summary>
        /// An indicator if transaction is active or not
        /// </summary>
        /// <returns></returns>
        public bool IsTransactionActive => Transaction.IsActive;

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
        /// Creates NHibernate unit of work scope
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        /// <param name="isImplicitTransactionsEnabled">An indication if implicit transactions are enabled. Default is false.</param>
        public NHibernateUnitOfWorkScope(ISessionFactory sessionFactory, bool isImplicitTransactionsEnabled = false) {
            this.sessionFactory = sessionFactory;
            this.isImplicitTransactionsEnabled = isImplicitTransactionsEnabled;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Begins/starts with transaction
        /// </summary>
        public virtual void BeginTransaction() {
            if (Transaction != null && !Transaction.IsActive) {
                isTransactionCreated = true;
                Session.BeginTransaction();
            }
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="BeginTransaction"/>.
        /// This method invokes the virtual method <see cref="BeginTransactionAsync()"/> with CancellationToken.None.
        /// Begins/starts with transaction
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task BeginTransactionAsync() {
            await BeginTransactionAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="BeginTransaction"/>.
        /// Begins/starts with transaction
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            BeginTransaction();
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Commits transaction
        /// </summary>
        public virtual void Commit() {
            isScopeCompleted = true;
            if (isTransactionCreated) {
                Transaction.Commit();

                if (isImplicitTransactionsEnabled) {
                    isScopeCompleted = false;
                    BeginTransaction();
                }
            }
        }

        /// <summary>
        /// Commits transaction
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task CommitAsync(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            isScopeCompleted = true;
            if (isTransactionCreated) {
                await Transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

                if (isImplicitTransactionsEnabled) {
                    isScopeCompleted = false;
                    await BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Commits transaction
        /// This method invokes the virtual method <see cref="CommitAsync()"/> with CancellationToken.None.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task CommitAsync() {
            await CommitAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Rollbacks transaction
        /// </summary>
        public virtual void Rollback() {
            isScopeCompleted = true;
            if (isTransactionCreated) {
                Transaction.Rollback();

                if (isImplicitTransactionsEnabled) {
                    isScopeCompleted = false;
                    BeginTransaction();
                }
            }
        }

        /// <summary>
        /// Rollbacks transaction
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task RollbackAsync(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            isScopeCompleted = true;
            if (isTransactionCreated) {
                await Transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);

                if (isImplicitTransactionsEnabled) {
                    isScopeCompleted = false;
                    await BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Rollbacks transaction
        /// This method invokes the virtual method <see cref="RollbackAsync()"/> with CancellationToken.None.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task RollbackAsync() {
            await RollbackAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (!isScopeCompleted && Transaction.IsActive) {
                    Rollback();
                }

                if (isTransactionCreated) {
                    Transaction.Dispose();
                }
            }
        }

        #endregion

    }
}
