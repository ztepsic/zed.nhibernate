using System.Data.SQLite;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using Zed.NHibernate.Test;

namespace Zed.NHibernate.Tests {
    [TestFixture]
    public class NHibernateUnitOfWorkTests : NHibernateTestFixture {

        private const string CONNECTION_STRING = "Data Source=:memory:;Version=3;New=True;";

        static NHibernateUnitOfWorkTests() {
            TestConnectionProvider.CreateConnectionFunc = connString => new SQLiteConnection(connString);
            Configuration.DataBaseIntegration(db => {
                db.Dialect<SQLiteDialect>();
                db.Driver<SQLite20Driver>();
                db.ConnectionProvider<TestConnectionProvider>();
                db.ConnectionString = CONNECTION_STRING;
            })
            .SetProperty(Environment.CurrentSessionContextClass, "thread_static");

            var configProperties = Configuration.Properties;
            if (configProperties.ContainsKey(Environment.ConnectionStringName)) {
                configProperties.Remove(Environment.ConnectionStringName);
            }
        }

        [TestFixtureSetUp]
        public void FixtureSetup() { OnFixtureSetup(); }

        [TestFixtureTearDown]
        public void FixtureTearDown() { OnFixtureTeardown(); }

        [SetUp]
        public void Setup() { OnSetup(); }

        [TearDown]
        public void TearDown() { OnTeardown(); }

        [Test]
        public void Start_CreatesNHibernateUnitOfWorkAndBeginsWithTwoTransaction() {
            // Arrange

            // Act
            var unitOfWork = new NHibernateUnitOfWork(SessionFactory);
            using (var unitOfWorkRootScope = unitOfWork.Start()) {
                using (var unitOfWorkScope = unitOfWork.Start()) {
                    unitOfWorkScope.Rollback();
                }

                unitOfWorkRootScope.Commit();
            }


            // Assert
        }
    }
}
