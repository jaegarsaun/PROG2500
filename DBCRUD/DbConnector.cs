namespace DBCRUD;
using MySql.Data.MySqlClient;
using System;

public class DbConnector
{
    private string connectionString;

    public string ConnectionString
    {
        get { return connectionString; }
    }

    public DbConnector(string server, string database, string user, string pass)
    {
        connectionString = $"server={server}; database={database}; user={user}; password={pass}";
    }

    public void ExecuteQuery(string query, Dictionary<string, object> parameters, Action<MySqlDataReader> handleData)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    using (var reader = command.ExecuteReader())
                    {
                        handleData(reader);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    
    public void ExecuteNonQuery(string query, Dictionary<string, object> parameters)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    // Add parameters to the command.
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception, possibly by showing a message to the user
            }
        }
    }
}