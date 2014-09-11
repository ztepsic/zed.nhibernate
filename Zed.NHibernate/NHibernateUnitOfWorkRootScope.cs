using NHibernate;
using NHibernate.Context;

namespace Zed.NHibernate {
    /// <summary>
    /// NHibernate Unit of Work Root Scope
    /// </summary>
    /// <remarks>Based on article: http://www.planetgeek.ch/2012/05/05/what-is-that-all-about-the-repository-anti-pattern/ </remarks>
    class NHibernateUnitOfWorkRootScope : NHibernateUnitOfWorkScope {

        #region Fields and Properties
        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate unit of work root scope
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        public NHibernateUnitOfWorkRootScope(ISessionFactory sessionFactory) : base(sessionFactory) { }

        #endregion

        #region Methods

        public override void BeginTransaction() {
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            base.BeginTransaction();
        }

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
