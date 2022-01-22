using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data.Common;

namespace Zed.NHibernate {
    /// <summary>
    /// NHibernate user type for dateTime2 SQL Server type
    /// </summary>
    [Obsolete("Not used anymore. From NHibernate v5.0 DateTime does not more cut fractal seconds.", false)]
    [Serializable]
    public class DateTimeUserType : IUserType {

        #region Fields and Properties

        private static readonly SqlType[] sqlTypes = { NHibernateUtil.DateTime.SqlType };

        /// <summary>
        /// The SQL types for the columns mapped by this type. 
        /// </summary>
        public SqlType[] SqlTypes {
            get { return sqlTypes; }
        }

        /// <summary>
        /// The type returned by <c>NullSafeGet()</c>
        /// </summary>
        public Type ReturnedType {
            get { return typeof(DateTimeUserType); }
        }

        /// <summary>
        /// Are objects of this type mutable?
        /// </summary>
        public bool IsMutable {
            get { return false; }
        }

        #endregion

        #region Constructors and Init
        #endregion

        #region Methods

        /// <summary>
        /// Retrieve an instance of the mapped class from an ADO resultset. Implementors
        /// should handle possibility of null values.
        /// </summary>
        /// <param name="rs">a DbDataReader</param>
        /// <param name="names">column names</param>
        /// <param name="session">
        /// The session for which the operation is done. Allows access to Factory.Dialect
        /// and Factory.ConnectionProvider.Driver for adjusting to database or data provider
        /// capabilities.
        /// </param>
        /// <param name="owner">the containing entity</param>
        /// <returns>The value.</returns>
        /// <exception cref="T:NHibernate.HibernateException">HibernateException</exception>
        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner) {
            object obj = NHibernateUtil.DateTime.NullSafeGet(rs, names[0], session, owner);

            return (DateTime?) obj;
        }

       
        /// <summary>
        /// Write an instance of the mapped class to a prepared statement. Implementors should
        /// handle possibility of null values. A multi-column type should be written to parameters
        /// starting from index.
        /// </summary>
        /// <param name="cmd">a DbCommand</param>
        /// <param name="value">the object to write</param>
        /// <param name="index">command parameter index</param>
        /// <param name="session">
        /// The session for which the operation is done. Allows access to Factory.Dialect
        /// and Factory.ConnectionProvider.Driver for adjusting to database or data provider
        /// capabilities.
        /// </param>
        /// <exception cref="T:NHibernate.HibernateException">HibernateException</exception>
        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session) {
            if (value == null) {
                cmd.Parameters[index].Value = DBNull.Value;
            } else {
                cmd.Parameters[index].Value = (DateTime)value;
            }
        }

        /// <summary>
        /// During merge, replace the existing (<paramref name="target"/>) value in the entity
        /// we are merging to with a new (<paramref name="original"/>) value from the detached
        /// entity we are merging. For immutable objects, or null values, it is safe to simply
        /// return the first parameter. For mutable objects, it is safe to return a copy of the
        /// first parameter. For objects with component values, it might make sense to
        /// recursively replace component values.
        /// </summary>
        /// <param name="original">the value from the detached entity being merged</param><param name="target">the value in the managed entity</param><param name="owner">the managed entity</param>
        /// <returns>
        /// the value to be merged
        /// </returns>
        public object Replace(object original, object target, object owner) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reconstruct an object from the cacheable representation. At the very least this
        /// method should perform a deep copy if the type is mutable. (optional operation)
        /// </summary>
        /// <param name="cached">the object to be cached</param><param name="owner">the owner of the cached object</param>
        /// <returns>
        /// a reconstructed object from the cachable representation
        /// </returns>
        public object Assemble(object cached, object owner) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Transform the object into its cacheable representation. At the very least this
        /// method should perform a deep copy if the type is mutable. That may not be enough
        /// for some implementations, however; for example, associations must be cached as
        /// identifier values. (optional operation)
        /// </summary>
        /// <param name="value">the object to be cached</param>
        /// <returns>
        /// a cacheable representation of the object
        /// </returns>
        public object Disassemble(object value) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a deep copy of the persistent state, stopping at entities and at collections.
        /// </summary>
        /// <param name="value">generally a collection element or entity field</param>
        /// <returns>
        /// a copy
        /// </returns>
        public object DeepCopy(object value) {
            return value;
        }

        /// <summary>
        /// Compare two instances of the class mapped by this type for persistent "equality"
        /// ie. equality of persistent state
        /// </summary>
        /// <param name="x"/><param name="y"/>
        /// <returns/>
        public new bool Equals(object x, object y) {
            if (x == y) {
                return true;
            }

            if (x == null || y == null) {
                return false;
            }

            return x.Equals(y);
        }

        /// <summary>
        /// Get a hashcode for the instance, consistent with persistence "equality"
        /// </summary>
        public int GetHashCode(object x) {
            return x.GetHashCode();
        }

        #endregion
    }
}
