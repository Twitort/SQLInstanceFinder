using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System;

namespace SQLInstanceFinder
{
    public class SQLServerInstanceFinder
    {
        public List<string> GetDatabaseConnectionStrings()
        {
            List<string> connectionStrings = new List<string>();

            // Get the list of available SQL Server instances
            DataTable serverInstances = SqlDataSourceEnumerator.Instance.GetDataSources();

            foreach (DataRow row in serverInstances.Rows)
            {
                string serverName = row["ServerName"].ToString();
                string instanceName = row["InstanceName"].ToString();
                string connectionString = GetConnectionString(serverName, instanceName);
                connectionStrings.Add(connectionString);
            }

            return connectionStrings;
        }

        private string GetConnectionString(string serverName, string instanceName)
        {
            // Modify the connection string as per your requirements
            // You may need to change the authentication mode, username, password, etc.

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = serverName;

            if (!string.IsNullOrEmpty(instanceName))
            {
                builder.DataSource += "\\" + instanceName;
            }

            builder.IntegratedSecurity = true; // Use Windows Authentication

            // Add any other required connection properties
            // builder.UserID = "your_username";
            // builder.Password = "your_password";

            return builder.ConnectionString;
        }

        public List<string> GetDatabases(string connectionString)
        {
            List<string> databases = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Retrieve the list of databases
                    DataTable databaseTable = connection.GetSchema("Databases");

                    foreach (DataRow row in databaseTable.Rows)
                    {
                        string databaseName = row["database_name"].ToString();

                        if (!IsSystemDatabase(databaseName))
                            databases.Add(databaseName);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the database retrieval process
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return databases;
        }

        private bool IsSystemDatabase(string databaseName)
        {
            // Add any additional system databases to exclude from the list
            string[] systemDatabases = { "master", "tempdb", "model", "msdb" };

            return Array.Exists(systemDatabases, db => db.Equals(databaseName, StringComparison.OrdinalIgnoreCase));
        }
    }


}
