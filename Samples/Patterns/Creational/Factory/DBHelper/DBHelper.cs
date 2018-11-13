using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Threading;

namespace PnP.Patterns.Factory
{
    //Enumeraciones

    #region Enumeraciones

    /// <summary>
    /// Enumeración del tipo de proveedor
    /// </summary>
    public enum ProviderType
    {
        /// <summary>
        /// Soporte para el proveedor MySql
        /// </summary>
        MySql,

        /// <summary>
        /// Soporte para el proveedor ODBC
        /// </summary>
        Odbc,

        /// <summary>
        /// Soporte para el proveedor Oracle
        /// </summary>
        Oracle,

        /// <summary>
        /// Soporte para el proveedor SqlServer
        /// </summary>
        SqlServer
    }

    #endregion Enumeraciones

    /// <summary>
    /// Clase para definir la base de datos
    /// </summary>
    public class DBHelper : IDBHelper
    {
        //Constantes

        #region Constantes

        //Nuemro de reintentos por defecto que se realizan para efectuar la conexion
        private static readonly int DEFAULT_NUMRETRY = 10;

        //Tiempo por de defecto que se esperan entre cada reintento en milisegundos
        private static readonly int DEFAULT_TIMEOUTRETRY = 1000;

        #endregion Constantes

        //Campos o Atributos

        #region Campos o Atributos

        //Tipo de Provedeor para identificar la base de datos
        private ProviderType providerType;

        //Cadena de Conexion
        private string connectionString;

        //Numero de reintentos para hacer la conexion
        private int numRetry;

        //Timepo de espera entre los reintentos en milisegundos
        private int timeOutRetry;

        #endregion Campos o Atributos

        //Propiedades

        #region Propiedades

        /// <summary>
        /// Cadena de conexion a la base de datos
        /// </summary>
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        /// <summary>
        /// Numero de reintentos para establecer la conexion a la base de datos
        /// </summary>
        public int NumRetry { get => numRetry; set => numRetry = value; }

        /// <summary>
        /// Tiempo de espera entre reintentos en milisegundos
        /// </summary>
        public int TimeOutRetry { get => timeOutRetry; set => timeOutRetry = value; }

        #endregion Propiedades

        //Constructores

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DBHelper()
        {
            //Inicializar los valores por defecto
            this.providerType = ProviderType.Oracle;
            this.connectionString = "";
            this.numRetry = DEFAULT_NUMRETRY;
            this.timeOutRetry = DEFAULT_TIMEOUTRETRY;
        }

        /// <summary>
        /// Constructor con asignacion de proveedor y cadena de conexion
        /// </summary>
        /// <param name="providerType">Tipo de proveedor de base de datos</param>
        /// <param name="connectionString">Cadena de Conexion</param>
        public DBHelper(ProviderType providerType, string connectionString)
        {
            this.providerType = providerType;
            this.connectionString = connectionString;
            this.numRetry = DEFAULT_NUMRETRY;
            this.timeOutRetry = DEFAULT_TIMEOUTRETRY;
        }

        /// <summary>
        /// Constructor que configura las base de datos
        /// </summary>
        /// <param name="providerType">Tipo de Proveedor</param>
        /// <param name="connectionString">CAdena de conexion</param>
        /// <param name="numRetry">Numero de retintentos para la conexion</param>
        /// <param name="timeOutRetry">Tiempo de espera entre reintentos en milisegundos</param>
        public DBHelper(ProviderType providerType, string connectionString, int numRetry, int timeOutRetry)
        {
            //asignar los valores a los campos
            this.providerType = providerType;
            this.connectionString = connectionString;
            this.numRetry = numRetry;
            this.timeOutRetry = timeOutRetry;
        }

        #endregion Constructores

        //Metodos

        #region Metodos

        //Publicos

        #region Publicos

        //General

        #region General

        /// <summary>
        /// Obtiene el identificador de parámetro usado por el <seealso cref="ProviderType"/> para delimitar en la base de datos
        /// </summary>
        public string ParameterToken()
        {
            string parameterToken = "";

            switch (providerType)
            {
                case ProviderType.MySql: parameterToken = "@"; break;
                case ProviderType.Odbc: parameterToken = "?"; break;
                case ProviderType.Oracle: parameterToken = ":"; break;
                case ProviderType.SqlServer: parameterToken = "@"; break;
                //Agregue un nuevo parameterToken aquí
                default: parameterToken = ""; break;
            }

            return parameterToken;
        }

        #endregion General

        //Metodos de  conexion

        #region Metodos de Conexion

        /// <summary>
        /// Crea una conexión hacia la base de datos
        /// </summary>
        /// <returns>Conexión a la base de datos</returns>
        public IDbConnection CreateConnection()
        {
            try
            {
                //Validar el Tipo de Proveedor
                switch (providerType)
                {
                    //Retornar la conexion segun el tipo de proveedor
                    case ProviderType.MySql: return new MySqlConnection(connectionString);
                    case ProviderType.Odbc: return new OdbcConnection(connectionString);
                    case ProviderType.Oracle: return new OracleConnection(connectionString);
                    case ProviderType.SqlServer: return new SqlConnection(connectionString);
                    default: throw new Exception("Error al crear la conexión, El proveedor" + providerType.ToString() + " no está soportado");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Crea y abre una conexión
        /// </summary>
        /// <returns>Conexión a la base de datos</returns>
        public IDbConnection CreateAndOpenConnection()
        {
            //Crear la conexion
            IDbConnection connection = this.CreateConnection();
            //Abrir la Conexion
            this.OpenConnection(connection);
            //Retornar la Conexion lista par asignar
            return connection;
        }

        /// <summary>
        /// Abre una conexión
        /// </summary>
        /// <param name="connection">La conexión</param>
        public void OpenConnection(IDbConnection connection)
        {
            //Contador de reintentos
            int countRetry = 1;
            try
            {
                //Ciclo para contar los reintentos y validar si la conexion no esta abierta
                while ((countRetry <= numRetry) && (connection.State != ConnectionState.Open))
                {
                    try
                    {
                        //Intentar abrir la conexion
                        connection.Open();
                    }
                    catch (Exception)
                    {
                        if (countRetry < NumRetry)
                        {
                            //Validar el estado de la conexion
                            if (connection.State != ConnectionState.Open)
                            {
                                //Incrementar el Contador
                                countRetry++;
                                //Esperar para intentar nuevamente
                                Thread.Sleep(timeOutRetry);
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Pasar la Excepcion
                throw new Exception("Error al Abrir la conexión: " + ex.Message);
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        /// <param name="connection">Conexión a cerrar</param>
        public void CloseConnection(IDbConnection connection)
        {
            //Contador de reintentos
            int countRetry = 1;

            try
            {
                //Ciclo para Intentar cerrar la conexion mientra no este cerrada
                while ((countRetry <= numRetry) && (connection.State != ConnectionState.Closed))
                {
                    try
                    {
                        //Intentar Cerrar la conexion
                        connection.Close();
                    }
                    catch (Exception)
                    {
                        if (countRetry < NumRetry)
                        {
                            //Validar el Estado
                            if (connection.State != ConnectionState.Closed)
                            {
                                //Incrementar el contador reintentos
                                countRetry++;
                                //Esperar para intentar nuevamente
                                Thread.Sleep(timeOutRetry);
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Pasar la Exscepcion
                throw new Exception("Error al cerrar la conexión: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        /// <summary>
        /// Evalúa si la conexión está abierta
        /// </summary>
        /// <param name="connection">la conexión a evaluar</param>
        /// <returns>Si está conectado <see langword="true"/>, de lo contrario <see langword="false"/>.</returns>
        public bool IsConnectionOpen(IDbConnection connection)
        {
            bool result = false;
            //Validar la conexion
            if (connection.State == ConnectionState.Open)
            {
                result = true;
            }

            return result;
        }

        #endregion Metodos de Conexion

        //Metodos de Comando

        #region Métodos de comando

        /// <summary>
        /// Crea y retorna un comando asociado a la conexión específica
        /// </summary>
        /// <param name="connection">conexión activa</param>
        /// <returns>Objeto Command</returns>
        public IDbCommand CreateCommand(IDbConnection connection)
        {
            try
            {
                //Validar si la conexion esta abierta
                if (IsConnectionOpen(connection))
                {
                    //retornar el comando
                    return connection.CreateCommand();
                }
                else
                {
                    //Generar una excepcion por que la conexion no esta abierta
                    throw new Exception("La conexión no está abierta");
                }
            }
            catch (Exception ex)
            {
                //Crear una excepcion por que no se puede crear el comando
                throw new Exception("Error al crear el comando: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea y retorna un comando asociado a la conexión específica
        /// </summary>
        /// <param name="connection">conexión activa</param>
        /// <param name="commandType">tipo de comando</param>
        /// <param name="commandText">texto del comando</param>
        /// <returns>Objeto Command</returns>
        public IDbCommand CreateCommand(IDbConnection connection, CommandType commandType, string commandText)
        {
            try
            {
                //Crear un nuevo comando
                IDbCommand command = connection.CreateCommand();
                //asignar las propiedades del comando
                command.CommandText = commandText;
                command.CommandType = commandType;
                return command;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el comando: " + ex.Message);
            }
        }

        #endregion Métodos de comando

        //Métodos de ExcecuteNonQuery

        #region Métodos de ExcecuteNonQuery

        /// <summary>
        /// Ejecuta un comando y retorna el número de filas afectadas
        /// </summary>
        /// <param name="connection">Conexión activa</param>
        /// <param name="command">Comando</param>
        /// <returns>El número de filas afectadas por el query</returns>
        public int ExecuteNonQuery(IDbConnection connection, IDbCommand command)
        {
            int rowsAffects = -1;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexion
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }

                    //Ejecutar el comando
                    rowsAffects = command.ExecuteNonQuery();
                    //command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintenrar
                    if (countRetry < numRetry)
                    {
                        //Increment countRetry
                        countRetry++;
                        //Sleep timeOutRetry
                        Thread.Sleep(this.timeOutRetry);
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }

                        throw new Exception("Error ejecutando el método ExecuteNonQuery: " + ex.Message);
                    }
                }
            }//while

            return rowsAffects;
        }

        /// <summary>
        /// Ejecuta una cadena Query y retorna el número de filas afectadas
        /// </summary>
        /// <param name="connection">Conexión</param>
        /// <param name="query">Query</param>
        /// <returns>El número de filas afectadas por el query</returns>
        public int ExecuteNonQuery(IDbConnection connection, string query)
        {
            int rowsAffects = -1;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validate Connection State
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }

                    //Exceute Query
                    IDbCommand command = CreateCommand(connection);
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    //Get Rows Affected
                    rowsAffects = command.ExecuteNonQuery();

                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Retry
                    if (countRetry < numRetry)
                    {
                        //Increment Numretry
                        countRetry++;
                        //Sleep TimeOutretry
                        Thread.Sleep(this.timeOutRetry);
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }

                        throw new Exception("Error ejecutando el método ExecuteNonQuery: " + ex.Message);
                    }
                }
            }//while

            return rowsAffects;
        }

        #endregion Métodos de ExcecuteNonQuery

        //Métodos de ExcecuteReader

        #region Métodos de ExcecuteReader

        /// <summary>
        /// Ejecuta un texto de comando y retorna un <see cref="IDataReader"/>
        /// </summary>
        /// <param name="connection">Conexión activa</param>
        /// <param name="command">Comando</param>
        /// <returns>un DataReader</returns>
        public IDataReader ExecuteReader(IDbConnection connection, IDbCommand command)
        {
            //Inicializa el DataReader
            IDataReader dr = null;

            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }
                    //Ejecuta el comando
                    dr = command.ExecuteReader();
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintentar
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar Numretry
                        Thread.Sleep(this.timeOutRetry);//Esperar TimeOutRetry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }

                        throw new Exception("Error al ejecutar el método ExecuteReader: " + ex);
                    }
                }
            }//while

            return dr;
        }

        /// <summary>
        /// Ejecuta una consulta y retorna un DataReader
        /// </summary>
        /// <example>
        /// <code>
        /// /// <summary>
        /// /// Un ejemplo de un query string estándar
        /// /// </summary>
        ///private static void DataSelectWithQueryString()
        ///{
        ///    IDataReader dr = dataBase.ExecuteReader(cnx, "SELECT * FROM Address");
        ///
        ///    while (dr.Read())
        ///        Console.WriteLine(dr.GetValue(1));
        ///
        ///    dataBase.CloseConnection(cnx);
        ///}
        /// </code>
        /// </example>
        /// <param name="connection">Conexión</param>
        /// <param name="query">El Query o consulta SQL</param>
        /// <returns>Un DataReader</returns>
        public IDataReader ExecuteReader(IDbConnection connection, string query)
        {
            IDataReader dr = null;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }
                    //Ejecutar el query
                    IDbCommand command = CreateCommand(connection);
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    //Obtener el DataReader
                    dr = command.ExecuteReader();
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintentar
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar Numretry
                        Thread.Sleep(this.timeOutRetry);//Esperar TimeOutretry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }
                        throw new Exception("Error al ejecutar el método ExecuteReader: " + ex);
                    }
                }
            }//while

            return dr;
        }

        #endregion Métodos de ExcecuteReader

        //Métodos de DataAdapter

        #region Métodos de DataAdapter

        /// <summary>
        /// Crea y retorna un DataAdapter dependiendo del <see cref="ProviderType"/> definido para esta base de datos
        /// </summary>
        /// <returns>Un IDbDataAdapter</returns>
        private IDbDataAdapter CreateDataAdapter()
        {
            try
            {
                //Validar el Adaptador
                switch (providerType)
                {
                    case ProviderType.MySql: return (IDbDataAdapter)(new MySqlDataAdapter());
                    case ProviderType.Odbc: return (IDbDataAdapter)(new OdbcDataAdapter());
                    case ProviderType.Oracle: return (IDbDataAdapter)(new OracleDataAdapter());
                    case ProviderType.SqlServer: return (IDbDataAdapter)(new SqlDataAdapter());
                    default: throw new Exception("Error creando el IDbDataAdapter, El proveedor: " + providerType.ToString() + " no está soportado");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Métodos de DataAdapter

        //Métodos de ExecuteDataSet

        #region Métodos de ExecuteDataSet

        /// <summary>
        /// Ejecuta el comando espeficicado y retorna un DataSet
        /// </summary>
        /// <param name="connection">La conexión</param>
        /// <param name="command">El comando</param>
        /// <returns>Un DataSet que contiene el resultado del comando</returns>
        public DataSet ExecuteDataSet(IDbConnection connection, IDbCommand command)
        {
            DataSet ds = null;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }
                    //Excecutar el comando
                    IDbDataAdapter adapter = CreateDataAdapter();
                    adapter.SelectCommand = command;
                    ds = new DataSet();
                    //Llenar el DataSet
                    adapter.Fill(ds);
                    CloseConnection(connection);
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintentar
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar countRetry
                        Thread.Sleep(this.timeOutRetry);//Esperar TimeOutretry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }
                        throw new Exception("Error ejecutando el método ExecuteDataSet: " + ex);
                    }
                }
            }//while

            return ds;
        }

        /// <summary>
        /// Ejecuta una consulta y retorna un DataSet
        /// </summary>
        /// <param name="connection">La conexion</param>
        /// <param name="query">El Query o consulta SQL</param>
        /// <returns>Un DataSet que contiene el resultado del comando</returns>
        public DataSet ExecuteDataSet(IDbConnection connection, string query)
        {
            DataSet ds = null;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }
                    //Ejecutar el Query
                    IDbCommand command = connection.CreateCommand();
                    command.CommandText = query;
                    IDbDataAdapter adapter = CreateDataAdapter();
                    adapter.SelectCommand = command;
                    ds = new DataSet();
                    //llenar el DataSet
                    adapter.Fill(ds);
                    CloseConnection(connection);
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintentar
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar Numretry
                        Thread.Sleep(this.timeOutRetry);//Esperar TimeOutretry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }
                        throw new Exception("Error ejecutando el método ExecuteDataSet: " + ex);
                    }
                }
            }//while

            return ds;
        }

        #endregion Métodos de ExecuteDataSet

        //Métodos de ExecuteDataTable

        #region Métodos de ExecuteDataTable

        /// <summary>
        /// Ejecuta el comando especificado y retorna un DataTable
        /// </summary>
        /// <param name="connection">La conexión</param>
        /// <param name="command">El comando</param>
        /// <returns>Un DataTable que contiene el resultado del comando</returns>
        public DataTable ExecuteDataTable(IDbConnection connection, IDbCommand command)
        {
            DataTable dt = null;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }

                    //Ejecutar el comando
                    DbDataAdapter adapter = (DbDataAdapter)CreateDataAdapter();
                    adapter.SelectCommand = (DbCommand)command;
                    dt = new DataTable();
                    //Llenar el DataSet
                    adapter.Fill(dt);
                    CloseConnection(connection);
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Retry
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar countRetry
                        Thread.Sleep(this.timeOutRetry);//Esperar TimeOutretry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }
                        throw new Exception("Error ejecutando el método ExecuteDataTable: " + ex);
                    }
                }
            }//while

            return dt;
        }

        /// <summary>
        /// Ejecuta la consulta y retorna un <see cref="DataTable"/>
        /// </summary>
        /// <example>
        /// <code>
        ///private static void StoreProcedureSelectSampleNoParametersDataSet()
        ///{
        ///    IDbCommand cmd = dataBase.GetStoredProcCommand(cnx, "sprocAddressList");
        ///    DataTable dt = dataBase.ExecuteDataTable(cnx, cmd);
        ///
        ///    for (int i=0;i != dt.Rows.Count;i++)
        ///       Console.WriteLine(dt.Rows[i][1].ToString());
        ///}
        /// </code>
        /// </example>
        /// <param name="connection">La conexión</param>
        /// <param name="query">EL Query o consulta SQL</param>
        /// <returns>Un DataTable que contiene el resultado del comando</returns>
        public DataTable ExecuteDataTable(IDbConnection connection, string query)
        {
            DataTable dt = null;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }

                    //Executar el Query
                    DbCommand command = (DbCommand)connection.CreateCommand();
                    command.CommandText = query;
                    DbDataAdapter adapter = (DbDataAdapter)CreateDataAdapter();
                    adapter.SelectCommand = command;
                    dt = new DataTable();
                    //Llenar el DataSet
                    adapter.Fill(dt);
                    CloseConnection(connection);
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintentar
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar Numretry
                        Thread.Sleep(this.timeOutRetry);//Esperar TimeOutretry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }

                        throw new Exception("Error ejecutando el método ExecuteDataTable: " + ex);
                    }
                }
            }//while

            return dt;
        }

        #endregion Métodos de ExecuteDataTable

        //Métodos de ExcecuteScalar

        #region Métodos de ExcecuteScalar

        /// <summary>
        /// Ejecuta el comando y retorna la primera columna de la
        /// primer fila del conjunto de datos retornados por el comando
        /// </summary>
        /// <param name="connection">La conexión</param>
        /// <param name="command">El comando</param>
        /// <returns>La primer columna de la primer fila</returns>
        public object ExecuteScalar(IDbConnection connection, IDbCommand command)
        {
            object obj = null;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }
                    //Ejecutar el comando
                    obj = command.ExecuteScalar();
                    CloseConnection(connection);
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintentar
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar countRetry
                        Thread.Sleep(this.timeOutRetry);//Esperar timeOutRetry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }
                        throw new Exception("Error ejecutando el método ExecuteScalar: " + ex);
                    }
                }
            }//while

            return obj;
        }

        /// <summary>
        /// Ejecuta un query y retorna la primera columna de la
        /// primer fila del conjunto de datos retornados por el comando
        /// </summary>
        /// <param name="connection">La conexión</param>
        /// <param name="query">El Query o consulta SQL</param>
        /// <returns>La primer columna de la primer fila</returns>
        public object ExecuteScalar(IDbConnection connection, string query)
        {
            object obj = null;
            int countRetry = 1;

            while (countRetry <= numRetry)
            {
                try
                {
                    //Validar el estado de la conexión
                    if (connection.State != ConnectionState.Open)
                    {
                        OpenConnection(connection);
                    }
                    //Ejecutar el Query
                    IDbCommand command = CreateCommand(connection);
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    //Obtener las filas afectadas
                    obj = command.ExecuteScalar();
                    CloseConnection(connection);
                    command.Dispose();

                    break;
                }
                catch (Exception ex)
                {
                    //Reintenar
                    if (countRetry <= numRetry)
                    {
                        countRetry++;//Incrementar countRetry
                        Thread.Sleep(this.timeOutRetry);//Esperar timeOutRetry
                    }
                    else
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            CloseConnection(connection);
                        }
                        throw new Exception("Error ejecutando el método ExecuteScalar: " + ex);
                    }
                }
            }//while

            return obj;
        }

        #endregion Métodos de ExcecuteScalar

        //Métodos de parametros de comando

        #region Métodos de parametros de comando

        /// <summary>
        /// Retorna una nueva instancia de la clase del proveedor que implementa la clase <see cref="System.Data.Common.DbParameter"/>
        /// </summary>
        /// <returns>un objeto DbParameter</returns>
        private DbParameter CreateParameter()
        {
            try
            {
                switch (providerType)
                {
                    case ProviderType.MySql: return (DbParameter)(new MySqlParameter());
                    case ProviderType.Odbc: return (DbParameter)(new OdbcParameter());
                    case ProviderType.Oracle: return (OracleParameter)(new OracleParameter());
                    case ProviderType.SqlServer: return (DbParameter)(new SqlParameter());
                    default: throw new Exception("Error creando el parámetro, El proveedor: " + providerType.ToString() + " no está soportado");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Agrega el parámetro al comando especificado
        /// </summary>
        /// <example>
        /// <code>
        /// /// <summary>
        /// /// Un procedimiento insert que muestra el valor de la llave creada por el identity
        /// /// </summary>
        ///private static void StoreProcedureInsertSampleParameters()
        ///{
        ///    IDbCommand cmd = dataBase.GetStoredProcCommand(cnx, "sprocAddressInsertUpdateSingleItem");
        ///
        ///    dataBase.AddCommandParameter(cmd, "id", ParameterDirection.Input, DBNull.Value, DbType.Int32, false);
        ///    dataBase.AddCommandParameter(cmd, "street", ParameterDirection.Input, "calle luna calle sol", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "houseNumber", ParameterDirection.Input, "123456", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "zipCode", ParameterDirection.Input, "0000", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "city", ParameterDirection.Input, "Santiago de Cali", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "country", ParameterDirection.Input, "Colombia", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "addressType", ParameterDirection.Input, 1, DbType.Int32, false);
        ///    dataBase.AddCommandParameter(cmd, "contactPersonId", ParameterDirection.Input, 26, DbType.Int32, false);
        ///    IDataParameter returnValue = dataBase.AddCommandParameterReturnValueType(cmd);
        ///
        ///    cmd.ExecuteNonQuery();
        ///
        ///    Console.WriteLine("Addres Inserted with Id {0}", (int)returnValue.Value);
        ///
        ///}
        /// </code>
        /// </example>
        /// <param name="command">El comando</param>
        /// <param name="name">El nombre del parámetro</param>
        /// <param name="direction">La dirección del parámetro</param>
        /// <param name="value">El valor del parámetro</param>
        /// <param name="dbType">El tipo de parámetro</param>
        public void AddCommandParameter(IDbCommand command, string name, ParameterDirection direction, object value, DbType dbType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }
            try
            {
                //Crear y configurar el parámetro
                DbParameter parameter = CreateParameter();
                parameter.Direction = direction;
                parameter.ParameterName = this.ParameterToken() + name;
                parameter.DbType = dbType;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                //Agregar el parámetro
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un parámetro al comando especificado
        /// </summary>
        /// <param name="command">El comando</param>
        /// <param name="name">El nombre del parámetro</param>
        /// <param name="value">El valor</param>
        /// <param name="dbType">El tipo de datos</param>
        public void AddCommandParameter(IDbCommand command, string name, Object value, DbType dbType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }
            try
            {
                //Crear y configurar el parámetro
                DbParameter parameter = CreateParameter();
                parameter.Direction = ParameterDirection.Input;
                parameter.ParameterName = this.ParameterToken() + name;
                parameter.DbType = dbType;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                //Agregar el parámetro
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un parámetro de entrada al comando especificado
        /// </summary>
        /// <param name="command">El comando</param>
        /// <param name="name">El nombre del parámetro</param>
        /// <param name="value">El valor</param>
        /// <param name="dbType">El tipo de datos</param>
        public void AddInParameter(IDbCommand command, string name, Object value, DbType dbType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }
            try
            {
                //Crear y configurar el parámetro
                DbParameter parameter = CreateParameter();
                parameter.Direction = ParameterDirection.Input;
                parameter.ParameterName = this.ParameterToken() + name;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                parameter.DbType = dbType;

                //Agregar el parámetro
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un parámetro de entrada al comando especificado
        /// </summary>
        /// <param name="command">El comando</param>
        /// <param name="name">El nombre del parámetro</param>
        /// <param name="value">El valor</param>
        /// <param name="size">longitud del campo</param>
        /// <param name="dbType">El tipo de datos</param>
        public void AddInParameter(IDbCommand command, string name, Object value, int size, DbType dbType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }
            try
            {
                //Crear y configurar el parámetro
                DbParameter parameter = CreateParameter();
                parameter.Direction = ParameterDirection.Input;
                parameter.ParameterName = this.ParameterToken() + name;
                parameter.Size = size;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                parameter.DbType = dbType;

                //Agregar el parámetro
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un parámetro de salida al comando especificado
        /// </summary>
        /// <param name="command">El comando</param>
        /// <param name="name">El nombre del parámetro</param>
        /// <param name="value">El valor</param>
        /// <param name="dbType">El tipo de datos</param>
        public void AddOutParameter(IDbCommand command, string name, Object value, DbType dbType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }

            try
            {
                //Crear y configurar el parámetro
                DbParameter parameter = CreateParameter();
                parameter.Direction = ParameterDirection.Output;
                parameter.ParameterName = this.ParameterToken() + name;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                parameter.DbType = dbType;

                //Agregar el parámetro
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un parámetro de salida al comando especificado
        /// </summary>
        /// <param name="command">El comando</param>
        /// <param name="name">El nombre del parámetro</param>
        /// <param name="value">El valor</param>
        /// <param name="size">longitud del campo</param>
        /// <param name="dbType">El tipo de datos</param>
        public void AddOutParameter(IDbCommand command, string name, Object value, int size, DbType dbType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }

            try
            {
                //Crear y configurar el parámetro
                DbParameter parameter = CreateParameter();
                parameter.Direction = ParameterDirection.Output;
                parameter.ParameterName = this.ParameterToken() + name;
                parameter.Size = size;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                parameter.DbType = dbType;

                //Agregar el parámetro
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un parámetro de entrada y salida al comando especificado
        /// </summary>
        /// <param name="command">El comando</param>
        /// <param name="name">El nombre del parámetro</param>
        /// <param name="value">El valor</param>
        /// <param name="dbType">El tipo de datos</param>
        public void AddIOParameter(IDbCommand command, string name, Object value, DbType dbType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }
            try
            {
                //Crear y configurar el parámetro
                DbParameter parameter = CreateParameter();
                parameter.Direction = ParameterDirection.InputOutput;
                parameter.ParameterName = this.ParameterToken() + name;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                parameter.DbType = dbType;

                //Agregar el parámetro
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un parámetro que contendrá el valor retornado desde el procedimiento almacenado
        /// </summary>
        /// <example>
        /// <code>
        /// /// <summary>
        /// /// Un procedimiento insert que muestra el valor de la llave creada por el identity
        /// /// </summary>
        ///private static void StoreProcedureInsertSampleParameters()
        ///{
        ///    IDbCommand cmd = dataBase.GetStoredProcCommand(cnx, "sprocAddressInsertUpdateSingleItem");
        ///
        ///    dataBase.AddCommandParameter(cmd, "id", ParameterDirection.Input, DBNull.Value, DbType.Int32, false);
        ///    dataBase.AddCommandParameter(cmd, "street", ParameterDirection.Input, "calle luna calle sol", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "houseNumber", ParameterDirection.Input, "123456", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "zipCode", ParameterDirection.Input, "0000", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "city", ParameterDirection.Input, "Santiago de Cali", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "country", ParameterDirection.Input, "Colombia", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "addressType", ParameterDirection.Input, 1, DbType.Int32, false);
        ///    dataBase.AddCommandParameter(cmd, "contactPersonId", ParameterDirection.Input, 26, DbType.Int32, false);
        ///    IDataParameter returnValue = dataBase.AddCommandParameterReturnValueType(cmd);
        ///
        ///    cmd.ExecuteNonQuery();
        ///
        ///    Console.WriteLine("Addres Inserted with Id {0}", (int)returnValue.Value);
        ///
        ///}
        /// </code>
        /// </example>
        /// <param name="command">El comando</param>
        /// <returns>un IDataParameter</returns>
        public IDataParameter AddCommandParameterReturnValueType(IDbCommand command)
        {
            try
            {
                IDataParameter returnValue = command.CreateParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea un Parametro de tipo Cursor Referenciado
        /// </summary>
        /// <remarks>Solo es valido para ORACLE</remarks>
        /// <param name="command">Comando de tipo OracleCommand</param>
        /// <param name="name">Nobre del Parametro</param>
        /// <param name="direction">Direccion del parametro o cursor referenciado</param>
        /// <param name="value">Valor del Cursor referenciado (null cuando es de salida)</param>
        /// <returns>El Parametro para obtener el cursor referenciado</returns>
        public IDataParameter AddCommandRefCurParameter(IDbCommand command, string name, ParameterDirection direction, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El nombre del parámetro está vacio");
            }
            try
            {
                //Crear y configurar el parámetro
                OracleParameter parameter = CreateParameter() as OracleParameter;
                parameter.Direction = direction;
                parameter.ParameterName = this.ParameterToken() + name;
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }

                //Asigna el tipo
                parameter.OracleDbType = OracleDbType.RefCursor;
                //Adiciona el Parametro
                command.Parameters.Add(parameter);

                //retornar el parametro asignado al cursor referenciado
                return parameter;
            }
            catch (Exception ex)
            {
                throw new Exception("Error agregando el parámetro: " + ex.Message);
            }
        }

        #endregion Métodos de parametros de comando

        //Métodos de StoredProcedure

        #region Métodos de StoredProcedure

        /// <summary>
        /// <para>Crea un <see cref="DbCommand"/> para un procedimiento almacenado</para>
        /// </summary>
        /// <example>
        /// <code>
        /// /// <summary>
        /// /// Un procedimiento insert que muestra el valor de la llave creada por el identity
        /// /// </summary>
        ///private static void StoreProcedureInsertSampleParameters()
        ///{
        ///    IDbCommand cmd = dataBase.GetStoredProcCommand(cnx, "sprocAddressInsertUpdateSingleItem");
        ///
        ///    dataBase.AddCommandParameter(cmd, "id", ParameterDirection.Input, DBNull.Value, DbType.Int32, false);
        ///    dataBase.AddCommandParameter(cmd, "street", ParameterDirection.Input, "calle luna calle sol", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "houseNumber", ParameterDirection.Input, "123456", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "zipCode", ParameterDirection.Input, "0000", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "city", ParameterDirection.Input, "Santiago de Cali", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "country", ParameterDirection.Input, "Colombia", DbType.String, false);
        ///    dataBase.AddCommandParameter(cmd, "addressType", ParameterDirection.Input, 1, DbType.Int32, false);
        ///    dataBase.AddCommandParameter(cmd, "contactPersonId", ParameterDirection.Input, 26, DbType.Int32, false);
        ///    IDataParameter returnValue = dataBase.AddCommandParameterReturnValueType(cmd);
        ///
        ///    cmd.ExecuteNonQuery();
        ///
        ///    Console.WriteLine("Address Inserted with Id {0}", (int)returnValue.Value);
        ///
        ///}
        /// </code>
        /// </example>
        /// <param name="connection">La conexión</param>
        /// <param name="storedProcedureName"><para>El nombre del procedimiento almacenado</para></param>
        /// <returns><para>El <see cref="IDbCommand"/> for the stored procedure.</para></returns>
        public virtual IDbCommand GetStoredProcCommand(IDbConnection connection, string storedProcedureName)
        {
            try
            {
                if (string.IsNullOrEmpty(storedProcedureName))
                {
                    throw new Exception("El nombre del procedimiento almacenado está vacio");
                }
                return CreateCommand(connection, CommandType.StoredProcedure, storedProcedureName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el comando: " + ex.Message);
            }
        }

        /// <summary>
        /// <para>Crea un <see cref="DbCommand"/> para un procedimiento.</para>
        /// </summary>
        /// <param name="connection">la Conexión</param>
        /// <param name="storedProcedureName"><para>El nombre del procedimiento almacenado</para></param>
        /// <param name="parameterValues"><para>La lista de parámetros del procedimiento almacenado</para></param>
        /// <returns><para>El <see cref="DbCommand"/> para el procedimiento almacenado</para></returns>
        /// <remarks>
        /// <para>Los parámetros para el procedimiento almacenado serán descubiertos y seran asignados en el orden posicional.</para>
        /// </remarks>
        public virtual IDbCommand GetStoredProcCommand(IDbConnection connection, string storedProcedureName, params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new Exception("El nombre del procedimiento almacenado está vacio");
            }

            try
            {
                DbCommand command = (DbCommand)CreateCommand(connection);
                this.AssignParameterValues(command, parameterValues);
                return command;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el comando: " + ex.Message);
            }
        }

        /// <summary>
        /// Configurar el valor del un parámetro
        /// </summary>
        /// <param name="command">El comando con el parámetro</param>
        /// <param name="parameterName">El nombre del parámetro</param>
        /// <param name="value">El valor del parámetro</param>
        public virtual void SetParameterValue(DbCommand command, string parameterName, object value)
        {
            command.Parameters[parameterName].Value = (value == null) ? DBNull.Value : value;
        }

        #endregion Métodos de StoredProcedure

        //Métodos de soporte de Transaction

        #region Métodos de soporte de Transaction

        /// <summary>
        /// Comienza una transacción
        /// </summary>
        /// <param name="connection">La conexión</param>
        /// <returns>una DbTransaction</returns>
        public DbTransaction BeginTransaction(DbConnection connection)
        {
            DbTransaction tran = connection.BeginTransaction();
            return tran;
        }

        /// <summary>
        /// Devuelve una transacción
        /// </summary>
        /// <param name="tran">La transacción a devolver</param>
        public void RollbackTransaction(DbTransaction tran)
        {
            tran.Rollback();
        }

        /// <summary>
        /// Confirma la transacción (Commit)
        /// </summary>
        /// <param name="tran">la transacción a confirmar (Commit)</param>
        public void CommitTransaction(DbTransaction tran)
        {
            tran.Commit();
        }

        #endregion Métodos de soporte de Transaction

        #endregion Publicos

        //Privados

        #region Privados

        /// <summary>
        /// Asignar valores a los procedimientos
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="values">Un array de valores de objeto</param>
        private void AssignParameterValues(DbCommand command, object[] values)
        {
            int parameterIndexShift = 0;    // DONE magic number, depends on the database
            for (int i = 0; i < values.Length; i++)
            {
                DbParameter parameter = (DbParameter)command.Parameters[i + parameterIndexShift];
                SetParameterValue(command, parameter.ParameterName, values[i]);
            }
        }

        #endregion Privados

        #endregion Metodos
    }
}