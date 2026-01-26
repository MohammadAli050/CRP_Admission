using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DATAMANAGER
{
    public static class DataTableManager
    {
        // Use 'readonly' since the value is set once by the static constructor.
        private static readonly string _connectionString;

        // =========================================================
        // 💡 STATIC CONSTRUCTOR: This runs once, on first access.
        // It safely reads the configuration value.
        // =========================================================
        static DataTableManager()
        {
            // The exact name as defined in the <add name="..."> tag
            const string connName = "AdmissionDB";

            // Attempt to retrieve the settings object
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connName];

            if (settings == null)
            {
                // Throw an exception that clearly tells the developer what's missing
                throw new InvalidOperationException($"Configuration Error: Connection string '{connName}' not found in the config file. Please verify the name and placement inside <connectionStrings>.");
            }

            // Successfully retrieved the connection string value
            _connectionString = settings.ConnectionString;
        }
        public static DataTable GetDataFromQuery(string SPName)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrWhiteSpace(SPName))
            {
                return dt;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(SPName, connection);
                command.CommandType = System.Data.CommandType.Text;
                try
                {
                    connection.Open();
                    SqlDataReader reader;
                    reader = command.ExecuteReader();
                    dt.Load(reader);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

        public static DataTable GetDataFromQuery(string SPName, List<SqlParameter> parameterList)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrWhiteSpace(SPName))
            {
                return dt;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(SPName, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (var item in parameterList)
                {
                    command.Parameters.Add(item);
                }
                try
                {
                    connection.Open();
                    SqlDataReader reader;
                    reader = command.ExecuteReader();
                    dt.Load(reader);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

    }
}
