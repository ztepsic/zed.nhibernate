﻿using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using System.Data.SQLite;
using System.Threading.Tasks;
using Zed.NHibernate.Test;

namespace Zed.NHibernate.Tests.Test {
    [TestFixture]
    public class NHibernateTestFixtureTests : NHibernateTestFixture {

        private const string CONNECTION_STRING = "Data Source=:memory:;Version=3;New=True;";

        static NHibernateTestFixtureTests() {
            TestConnectionProvider.CreateConnectionFunc = connString => new SQLiteConnection(connString);
            Configuration.Proxy(p => p.ProxyFactoryFactory<StaticProxyFactoryFactory>())
                .DataBaseIntegration(db => {
                    db.Dialect<SQLiteDialect>();
                    db.Driver<SQLite20Driver>();
                    db.ConnectionProvider<TestConnectionProvider>();
                    db.ConnectionString = CONNECTION_STRING;
                })
                .SetProperty(Environment.CurrentSessionContextClass, "thread_static")
                .SetProperty("show_sql", "true");

            var configProperties = Configuration.Properties;
            if (configProperties.ContainsKey(Environment.ConnectionStringName)) {
                configProperties.Remove(Environment.ConnectionStringName);
            }
        }

        [OneTimeSetUp]
        public void FixtureSetup() { OnFixtureSetup(); }

        [OneTimeTearDown]
        public void FixtureTearDown() { OnFixtureTeardown(); }

        [SetUp]
        public void Setup() { OnSetup(); }

        [TearDown]
        public void TearDown() { OnTeardown(); }

        [Test]
        public void OpenSessionAndTransaction_TransactionRolledback() {
            // Arrange

            // Act
            using (var trx = Session.BeginTransaction()) {
                trx.Rollback();
            }

            // Assert
        }

        [Test]
        public async Task OpenSessionAndTransaction_TransactionRolledbackAsync() {
            // Arrange

            // Act
            using (var trx = Session.BeginTransaction()) {
                await trx.RollbackAsync();
            }

            // Assert
        }

    }
}
