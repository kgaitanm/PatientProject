using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Infrastructure.Database
{
    public sealed class DatabaseConnection
    {
        private static readonly Lazy<DatabaseConnection> _instance =
         new Lazy<DatabaseConnection>(() => throw new InvalidOperationException("Use the parameterized constructor to create an instance."));

        private readonly SqlConnection _connection;
        private DatabaseConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public static DatabaseConnection CreateInstance(string connectionString)
        {
            return new DatabaseConnection(connectionString);
        }

        public static DatabaseConnection Instance => _instance.Value;

        public SqlConnection GetConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
    }
}
