using System;
using NHibernate;
using NHibernate.Cfg;

namespace Zed.NHibernate {
    /// <summary>
    /// NHibernate session provider
    /// </summary>
    public static class NHibernateSessionProvider {

        #region Fields and Properties

        /// <summary>
        /// NHibernate configuration
        /// </summary>
        private static readonly Configuration configuration;

        /// <summary>
        /// Gets NHibernate configuration
        /// </summary>
        public static Configuration Configuration { get { return configuration; } }

        /// <summary>
        /// Session factory
        /// </summary>
        private static ISessionFactory sessionFactory;

        /// <summary>
        /// Gets session factory
        /// </summary>
        public static ISessionFactory SessionFactory {
            get {
                if(sessionFactory == null) Init();
                return sessionFactory;
            }
        }

        /// <summary>
        /// Gets the current session
        /// </summary>
        public static ISession CurrentSession { get { return SessionFactory.GetCurrentSession(); } }

        #endregion

        #region Constructors and Init

        static NHibernateSessionProvider() { configuration = new Configuration(); }

        /// <summary>
        /// Initialize NHibernate session provider with NHibernate configuration
        /// </summary>
        /// <param name="configFunc">NHibernate configuration function</param>
        public static void Init(Action<Configuration> configFunc) {
            configFunc(configuration);
            Init();
        }

        /// <summary>
        /// Initialize NHibernate session provider with previously configured NHibernate configuration
        /// </summary>
        public static void Init() { sessionFactory = configuration.BuildSessionFactory(); }

        #endregion

        #region Methods

        #endregion

    }
}
