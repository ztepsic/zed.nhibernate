using System;
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
        protected readonly ISessionFactory SessionFactory;

        /// <summary>
        /// Gets Session
        /// </summary>
        protected virtual ISession Session { get { return SessionFactory.GetCurrentSession(); } }

        #endregion

        #region Constructors and Init

        /// <summary>
        /// Creates NHibernate repository
        /// </summary>
        /// <param name="sessionFactory">NHibernate session factory</param>
        protected NHibernateRepository(ISessionFactory sessionFactory) {
            if (sessionFactory == null) {
                throw new ArgumentNullException("sessionFactory");
            }

            this.SessionFactory = sessionFactory;
        }

        #endregion

        #region Methods

        #endregion

    }
}
