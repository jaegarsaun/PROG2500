using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DBCRUD.models;

namespace DBCRUD;

public partial class SearchUsersWindow : Window
{
    private DbConnector db;
    public ObservableCollection<User> SearchResults { get; set; } = new ObservableCollection<User>();

    public SearchUsersWindow()
    {
        InitializeComponent();
        db = new DbConnector("localhost", "dbcrud", "root", "");
        DataContext = this; 
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear previous search results
        SearchResults.Clear();

        // Get the last name from the TextBox
        string lastNameToSearch = LastNameSearchBox.Text.Trim();

        // Build the query
        string query = "SELECT * FROM users WHERE lname LIKE @lname";
        var parameters = new Dictionary<string, object>
        {
            { "@lname", $"%{lastNameToSearch}%" } // Use the LIKE operator for partial matches
        };

        // Execute the query
        db.ExecuteQuery(query, parameters, reader =>
        {
            while (reader.Read())
            {
                var user = new User
                {
                    userid = Convert.ToInt32(reader["userid"]),
                    fname = reader["fname"].ToString(),
                    lname = reader["lname"].ToString(),
                    hobby = reader["hobby"].ToString(),
                    occupation = reader["occupation"].ToString(),
                    eyeimage = reader["eyeimage"].ToString(),
                    hairimage = reader["hairimage"].ToString(),
                    headimage = reader["headimage"].ToString(),
                    mouthimage = reader["mouthimage"].ToString(),
                    noseimage = reader["noseimage"].ToString(),
                };
                Dispatcher.Invoke(() => SearchResults.Add(user));
            }
        });
    }
    
    private void DeleteUser_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.Tag is int userId)
        {
            var query = "DELETE FROM users WHERE userid = @userid";
            var parameters = new Dictionary<string, object>
            {
                { "@userid", userId }
            };

            db.ExecuteNonQuery(query, parameters);

            
            MessageBox.Show($"User with ID: {userId} has been deleted.", "User Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
    }

    private void UpdateUser_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.Tag is int userId)
        {
            UpdateUserWindow updateUserWindow = new UpdateUserWindow(userId);
            updateUserWindow.ShowDialog(); // ShowDialog makes the window modal
        }
    }
}