using System;
using log4net;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Zed.NHibernate.Test {
    /// <summary>
    /// NHibernate nUnit fixture
    /// </summary>
    public abstract class NHibernateTestFixture {

        #region Fields and Properties

        /// <summary>
        /// Log
        /// </summary>
        protected static ILog Log = new Func<ILog>(() => {
            log4net.Config.XmlConfigurator.Configure();
            return LogManager.GetLogger(typeof(NHibernateTestFixture));
        }).Invoke();

        /// <summary>
        /// Configuration
        /// </summary>
        protected static Configuration Configuration { get { return NHibernateSessionProvider.Configuration; } }

        /// <summary>
        /// Gets session factory
        /// </summary>
        protected static ISessionFactory SessionFactory { get { return NHibernateSessionProvider.SessionFactory; } }

        /// <summary>
        /// Gets current session
        /// </summary>
        protected ISession Session { get { return SessionFactory.GetCurrentSession(); } }

        #endregion

        #region Constructors and Init
        #endregion

        #region Methods

        /// <summary>
        /// On fixture setup
        /// </summary>
        protected virtual void OnFixtureSetup() { }

        /// <summary>
        /// On fixture tear down
        /// </summary>
        protected virtual void OnFixtureTeardown() { }

        /// <summary>
        /// On Setup
        /// </summary>
        protected virtual void OnSetup() { SetupNHibernateSession(); }

        /// <summary>
        /// On Teardown
        /// </summary>
        protected virtual void OnTeardown() { TearDownNHibernateSession(); }

        /// <summary>
        /// Setup NHibernate session
        /// </summary>
        protected virtual void SetupNHibernateSession() {
            TestConnectionProvider.CloseDatabase();
            NHibernateSessionProvider.Init();
            setupContextualSession();
            BuildSchema();
        }

        /// <summary>
        /// Tear down NHibernate session
        /// </summary>
        protected virtual void TearDownNHibernateSession() {
            tearDownContextualSession();
            TestConnectionProvider.CloseDatabase();
        }

        private void setupContextualSession() {
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        private void tearDownContextualSession() {
            var sessionFactory = NHibernateSessionProvider.SessionFactory;
            var session = CurrentSessionContext.Unbind(sessionFactory);
            session.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void BuildSchema() {
            var cfg = Configuration;
            var schemaExport = new SchemaExport(cfg);
            ////schemaExport.Create(false, true);
            //schemaExport.Execute(false, true, false, Session.Connection, null);
            schemaExport.Execute(false, true, false);
        }

        #endregion

    }
}
