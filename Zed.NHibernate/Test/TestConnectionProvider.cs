using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Connection;

namespace Zed.NHibernate.Test {
    /// <summary>
    /// SQLite test connection provider
    /// </summary>
    public class TestConnectionProvider : ConnectionProvider {

        #region Fields and Properties

        private static DbConnection connection;

        /// <summary>
        /// Create connection func
        /// </summary>
        public static Func<string, DbConnection> CreateConnectionFunc { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get an open <see cref="T:System.Data.IDbConnection"/>.
        /// </summary>
        /// <returns>
        /// An open <see cref="T:System.Data.IDbConnection"/>.
        /// </returns>
        public override DbConnection GetConnection() {
            if (connection == null) {
                // new connection
                connection = CreateConnectionFunc(ConnectionString);
            }

            if (connection.State != ConnectionState.Open) {
                connection.Open();
            }

            return connection;

        }

        /// <summary>
        /// Get an open System.Data.Common.DbConnection.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
        /// <returns> An open System.Data.Common.DbConnection.</returns>
        public override async Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (connection == null) {
                // new connection
                connection = CreateConnectionFunc(ConnectionString);
            }

            if (connection.State != ConnectionState.Open) {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            return connection;
        }

        /// <summary>
        /// Close database connection
        /// </summary>
        /// <param name="conn"></param>
        public override void CloseConnection(DbConnection conn) {
            // ignore closing the connection
            // connection'll be closed by calling CloseDatabase by the and of TestFixture
        }

        /// <summary>
        /// Close database
        /// </summary>
        public static void CloseDatabase() {
            connection?.Close();
        }

        #endregion
    }
}
