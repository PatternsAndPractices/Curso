using System.Data;
using System.Data.Common;

namespace PnP.Patterns.Factory
{
    public interface IDBHelper
    {
        string ConnectionString { get; set; }
        int NumRetry { get; set; }
        int TimeOutRetry { get; set; }

        void AddCommandParameter(IDbCommand command, string name, object value, DbType dbType);

        void AddCommandParameter(IDbCommand command, string name, ParameterDirection direction, object value, DbType dbType);

        IDataParameter AddCommandParameterReturnValueType(IDbCommand command);

        IDataParameter AddCommandRefCurParameter(IDbCommand command, string name, ParameterDirection direction, object value);

        void AddInParameter(IDbCommand command, string name, object value, int size, DbType dbType);

        void AddInParameter(IDbCommand command, string name, object value, DbType dbType);

        void AddIOParameter(IDbCommand command, string name, object value, DbType dbType);

        void AddOutParameter(IDbCommand command, string name, object value, int size, DbType dbType);

        void AddOutParameter(IDbCommand command, string name, object value, DbType dbType);

        DbTransaction BeginTransaction(DbConnection connection);

        void CloseConnection(IDbConnection connection);

        void CommitTransaction(DbTransaction tran);

        IDbConnection CreateAndOpenConnection();

        IDbCommand CreateCommand(IDbConnection connection, CommandType commandType, string commandText);

        IDbCommand CreateCommand(IDbConnection connection);

        IDbConnection CreateConnection();

        DataSet ExecuteDataSet(IDbConnection connection, IDbCommand command);

        DataSet ExecuteDataSet(IDbConnection connection, string query);

        DataTable ExecuteDataTable(IDbConnection connection, IDbCommand command);

        DataTable ExecuteDataTable(IDbConnection connection, string query);

        int ExecuteNonQuery(IDbConnection connection, string query);

        int ExecuteNonQuery(IDbConnection connection, IDbCommand command);

        IDataReader ExecuteReader(IDbConnection connection, string query);

        IDataReader ExecuteReader(IDbConnection connection, IDbCommand command);

        object ExecuteScalar(IDbConnection connection, IDbCommand command);

        object ExecuteScalar(IDbConnection connection, string query);

        IDbCommand GetStoredProcCommand(IDbConnection connection, string storedProcedureName);

        IDbCommand GetStoredProcCommand(IDbConnection connection, string storedProcedureName, params object[] parameterValues);

        bool IsConnectionOpen(IDbConnection connection);

        void OpenConnection(IDbConnection connection);

        string ParameterToken();

        void RollbackTransaction(DbTransaction tran);

        void SetParameterValue(DbCommand command, string parameterName, object value);
    }
}