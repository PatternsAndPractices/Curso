using PnP.Patterns.Factory;
using System;
using System.Configuration;
using System.Data;

namespace FactoryTest
{
    internal class Program
    {
        //Campos o Atributos

        #region Campos o Atributos

        //Cadena de conexion a la Base de datos
        private static string strCnx;

        //Variable para manejar el enlace a la base de datos
        private static DBHelper db;

        //Variable para manejar la conexion a la base de datos
        private static IDbConnection cnx;

        #endregion Campos o Atributos

        private static void Main(string[] args)
        {
            ProviderType provider = ProviderType.MySql;

            string stringName = string.Empty;

            switch (provider)
            {
                case ProviderType.MySql:
                    stringName = "MySqlCnx";
                    break;
                case ProviderType.Odbc:
                    stringName = "OdbcCnx";
                    break;
                case ProviderType.Oracle:
                    stringName = "OracleCnx";
                    break;
                case ProviderType.SqlServer:
                    stringName = "SqlServerCnx";
                    break;
                default:
                    break;
            }

            //Incicialiar la cadena de conexion
            strCnx = ConfigurationManager.ConnectionStrings[stringName].ConnectionString;

            //Establecer la conexion a la base de datos
            db = new DBHelper(provider, strCnx);

            //Crear la Conexion
            cnx = db.CreateAndOpenConnection();

            //Crear el comando de consulta
            IDbCommand cmd = db.CreateCommand(cnx);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT
                                    Person_Id,
                                    FirstName, 
                                    LastName, 
                                    BirthDay, 
                                    Sex
                                FROM Persons";

            //Ejecutar la consulta
            IDataReader dr = db.ExecuteReader(cnx, cmd);

            //Valores
            long personId;
            string firstName;
            string lastName;
            DateTime birthDay;
            char sex;

            //Obtener los datos
            while(dr.Read())
            {
                personId = Convert.ToInt64(dr["Person_Id"].ToString());
                firstName = dr["FirstName"].ToString();
                lastName = dr["LastName"].ToString();
                birthDay = Convert.ToDateTime(dr["BirthDay"].ToString());
                sex = dr["Sex"].ToString()[0];

                Console.WriteLine($"{personId}, {firstName} {lastName}, {birthDay}, {sex}");
            }

            //Cerar la Conexion
            db.CloseConnection(cnx);

            Console.ReadLine();
        }
    }
}