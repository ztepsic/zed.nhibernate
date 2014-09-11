using System;
using NHibernate;
using Zed.Transaction;

namespace Zed.NHibernate {
    /// <summary>
    /// NHibernate Unit Of Work scope
    /// </summary>
    /// <remarks>Based on article: http://www.planetgeek.ch/2012/05/05/what-is-that-all-about-the-repository-anti-pattern/ </remarks>
    class NHibernateUnitOfWorkScope : IUnitOfWorkScope {

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

        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate unit of work scope
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        public NHibernateUnitOfWorkScope(ISessionFactory sessionFactory) {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region Methods

        public virtual void BeginTransaction() {
            if (Transaction != null && !Transaction.IsActive) {
                isTransactionCreated = true;
                Session.BeginTransaction();
            }
        }

        public virtual void Commit() {
            isScopeCompleted = true;
            if (isTransactionCreated) {
                Transaction.Commit();
            }
        }

        public virtual void Rollback() {
            isScopeCompleted = true;
            if (isTransactionCreated) {
                Transaction.Rollback();    
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
