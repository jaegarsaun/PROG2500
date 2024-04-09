
namespace DBCRUD;

using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using DBCRUD.models;



/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DbConnector db;

    public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
    
    public MainWindow()
    {
        InitializeComponent();
        db = new DbConnector("localhost", "dbcrud", "root", "");
        LoadUserData();

        DataContext = this;
    }

    private void LoadUserData()
    {
        Dispatcher.Invoke(() => Users.Clear());
        db.ExecuteQuery("SELECT * FROM users", new Dictionary<string, object>(), reader =>
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
                    
                    eyeimage = reader["eyeimage"] != DBNull.Value ? reader["eyeimage"].ToString() : null,
                    hairimage = reader["hairimage"] != DBNull.Value ? reader["hairimage"].ToString() : null,
                    headimage = reader["headimage"] != DBNull.Value ? reader["headimage"].ToString() : null,
                    mouthimage = reader["mouthimage"] != DBNull.Value ? reader["mouthimage"].ToString() : null,
                    noseimage = reader["noseimage"] != DBNull.Value ? reader["noseimage"].ToString() : null,
                };
                Dispatcher.Invoke(() => Users.Add(user));
            }
        });
    }
    
    private int currentIndex = 0;

    private void Previous_Click(object sender, RoutedEventArgs e)
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateCarouselPosition();
        }
    }

    private void Next_Click(object sender, RoutedEventArgs e)
    {
        if (currentIndex < Users.Count - 1)
        {
            currentIndex++;
            UpdateCarouselPosition();
        }
    }

    private void UpdateCarouselPosition()
    {
        if (currentIndex >= 0 && currentIndex < Users.Count)
        {
            var user = Users[currentIndex];
            

            // Calculate the new horizontal offset for the ScrollViewer.
            var newOffset = currentIndex * CarouselScrollViewer.ActualWidth;
            CarouselScrollViewer.ScrollToHorizontalOffset(newOffset);
        }
    }
    
    private void AddNewUser_Click(object sender, RoutedEventArgs e)
    {
        AddUserWindow addUserWindow = new AddUserWindow();
        addUserWindow.ShowDialog(); // ShowDialog makes the window modal
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

            // Refresh the user list to reflect the deletion
            Dispatcher.Invoke(() =>
            {
                Users.Clear();
                LoadUserData();
            });
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

    private void RefreshPage_Click(object sender, RoutedEventArgs e)
    {
        LoadUserData();
    }

    private void SearchUser_Click(object sender, RoutedEventArgs e)
    {

            SearchUsersWindow searchUsersWindow = new SearchUsersWindow();
            searchUsersWindow.ShowDialog(); // ShowDialog makes the window modal
        
    }
        
}
    

