using System.Data.SQLite;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using Zed.NHibernate.Test;
using Zed.NHibernate.Tests.Model;

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
            .SetProperty(Environment.CurrentSessionContextClass, "thread_static")
            //.SetProperty("show_sql", "true")
            ;

            var modelMapper = new ModelMapper();
            modelMapper.AddMapping<TagMapping>();
            Configuration.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());

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

        protected override void SetupNHibernateSession() {
            TestConnectionProvider.CloseDatabase();
            NHibernateSessionProvider.Init();
            BuildSchema();
        }

        protected override void TearDownNHibernateSession() {
            TestConnectionProvider.CloseDatabase();
        }


        [Test]
        public void Start_CreatesNHibernateUnitOfWorkAndBeginsWithTwoScopesOnOneTransaction_FirstScopeClosesSession() {
            // Arrange
            Tag tag1 = Tag.CreateBaseTag("tag1");
            Tag tag2 = Tag.CreateBaseTag("tag2");
            Tag result1;
            Tag result2;

            // Act
            var unitOfWork = new NHibernateUnitOfWork(SessionFactory);
            using (var unitOfWorkRootScope = unitOfWork.Start()) {
                Session.SaveOrUpdate(tag1);

                using (var unitOfWorkScope = unitOfWork.Start()) {
                    Session.SaveOrUpdate(tag2);
                    unitOfWorkScope.Rollback();
                }

                unitOfWorkRootScope.Commit();
            }

            using(var unitOfWorkRootScope = unitOfWork.Start()) {
                result1 = Session.Get<Tag>(1);
                result2 = Session.Get<Tag>(2);
            }


            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(tag1, result1);

            Assert.IsNotNull(result2);
            Assert.AreEqual(tag2, result2);
        }

        [Test]
        public async Task StartAsync_CreatesNHibernateUnitOfWorkAndBeginsWithTwoScopesOnOneTransaction_FirstScopeClosesSession() {
            // Arrange
            Tag tag1 = Tag.CreateBaseTag("tag1");
            Tag tag2 = Tag.CreateBaseTag("tag2");
            Tag result1;
            Tag result2;

            // Act
            var unitOfWork = new NHibernateUnitOfWork(SessionFactory);
            using (var unitOfWorkRootScope = await unitOfWork.StartAsync()) {
                Session.SaveOrUpdate(tag1);

                using (var unitOfWorkScope = await unitOfWork.StartAsync()) {
                    Session.SaveOrUpdate(tag2);
                    unitOfWorkScope.Rollback();
                }

                unitOfWorkRootScope.Commit();
            }

            using (var unitOfWorkRootScope = await unitOfWork.StartAsync()) {
                result1 = Session.Get<Tag>(1);
                result2 = Session.Get<Tag>(2);
            }


            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(tag1, result1);

            Assert.IsNotNull(result2);
            Assert.AreEqual(tag2, result2);
        }

        [Test]
        public void StartAsync_CreatesNHibernateUnitOfWorkAndBeginsWithOneScopesOnOneTransaction() {
            // Arrange
            Tag tag1 = Tag.CreateBaseTag("tag1");
            Tag tag2 = Tag.CreateBaseTag("tag2");
            Tag result1;
            Tag result2;

            // Act
            var unitOfWork = new NHibernateUnitOfWork(SessionFactory, true);
            using (var unitOfWorkRootScope = unitOfWork.Start()) {
                Session.SaveOrUpdate(tag1);
                unitOfWorkRootScope.Commit();

                Session.SaveOrUpdate(tag2);
                unitOfWorkRootScope.Rollback();


            }

            using (var unitOfWorkRootScope = unitOfWork.Start()) {
                result1 = Session.Get<Tag>(1);
                result2 = Session.Get<Tag>(2);
            }


            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(tag1, result1);

            Assert.IsNull(result2);

            
        }
    }
}
