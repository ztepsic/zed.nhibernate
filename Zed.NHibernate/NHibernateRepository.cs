using NHibernate;

namespace Zed.NHibernate {
    /// <summary>
    /// Base NHibernate repository
    /// Contains NHibernate session
    /// </summary>
    public abstract class NHibernateRepository {

        #region Fields and Properties

        /// <summary>
        /// Session factory
        /// </summary>
        private readonly ISessionFactory sessionFactory;

        /// <summary>
        /// Gets Session
        /// </summary>
        protected ISession Session { get { return sessionFactory.GetCurrentSession(); } }

        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate repository
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        protected NHibernateRepository(ISessionFactory sessionFactory) {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region Methods

        #endregion

    }
}
