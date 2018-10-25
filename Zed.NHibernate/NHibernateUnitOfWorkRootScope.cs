using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Context;

namespace Zed.NHibernate {
    /// <summary>
    /// NHibernate Unit of Work Root Scope
    /// </summary>
    /// <remarks>Based on article: http://www.planetgeek.ch/2012/05/05/what-is-that-all-about-the-repository-anti-pattern/ </remarks>
    public class NHibernateUnitOfWorkRootScope : NHibernateUnitOfWorkScope {

        #region Fields and Properties
        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate unit of work root scope
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        /// <param name="isImplicitTransactionsEnabled">An indication if implicit transactions are enabled. Default is false.</param>
        public NHibernateUnitOfWorkRootScope(ISessionFactory sessionFactory, bool isImplicitTransactionsEnabled = false) : base(sessionFactory, isImplicitTransactionsEnabled) { }

        #endregion

        #region Methods

        /// <summary>
        /// Begins/starts with transaction
        /// </summary>
        public override void BeginTransaction() {
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            base.BeginTransaction();
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="BeginTransaction"/>.
        /// This method invokes the virtual method <see cref="BeginTransactionAsync()"/> with CancellationToken.None.
        /// Begins/starts with transaction
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task BeginTransactionAsync() {
            await base.BeginTransactionAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="BeginTransaction"/>.
        /// Begins/starts with transaction
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task BeginTransactionAsync(CancellationToken cancellationToken) {
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            await base.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                ISession session = CurrentSessionContext.Unbind(SessionFactory);
                if (session != null) {
                    session.Close();
                    session.Dispose();    
                }
            }
        }

        #endregion

    }
}
