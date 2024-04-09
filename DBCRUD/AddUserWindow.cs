using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;

namespace DBCRUD;

public partial class AddUserWindow : Window
{
    // Dictionary to map type strings to Image controls
    private Dictionary<string, Image> imageControls;
    private DbConnector db;
    private string _hairImageDB;
    public string HairImageDB
    {
        get { return _hairImageDB; }
        set { _hairImageDB = value; } // You can add custom logic for validation or modification here.
    }

    private string _eyeImageDB;
    public string EyeImageDB
    {
        get { return _eyeImageDB; }
        set { _eyeImageDB = value; } // Custom logic for setting the value.
    }

    private string _noseImageDB;
    public string NoseImageDB
    {
        get { return _noseImageDB; }
        set { _noseImageDB = value; }
    }

    private string _mouthImageDB;
    public string MouthImageDB
    {
        get { return _mouthImageDB; }
        set { _mouthImageDB = value; }
    }

    private string _headImageDB;
    public string HeadImageDB
    {
        get { return _headImageDB; }
        set { _headImageDB = value; }
    }

    public AddUserWindow()
    {
        InitializeComponent();
        db = new DbConnector("localhost", "dbcrud", "root", "");
        
        try
        {
            LoadImages();
            LoadHobbiesFromDatabase();
            LoadJobsFromDatabase();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void LoadHobbiesFromDatabase()
    {
        string query = "SELECT * FROM hobbies"; 
        try
        {
            db.ExecuteQuery(query, new Dictionary<string, object>(), reader =>
            {
                Dispatcher.Invoke(() =>
                {
                    while (reader.Read())
                    {
                        string hobby = reader["type"].ToString(); 
                        cbHobby.Items.Add(hobby);
                    }
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to load hobbies from database: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void LoadJobsFromDatabase()
    {
        string query = "SELECT * FROM occupations"; 
        try
        {
            db.ExecuteQuery(query, new Dictionary<string, object>(), reader =>
            {
                Dispatcher.Invoke(() =>
                {
                    while (reader.Read())
                    {
                        string job = reader["type"].ToString(); 
                        cbJob.Items.Add(job);
                    }
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to load hobbies from database: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    // Get the image fields
    public string GetHairImageDB()
    {
        return HairImageDB;
    }
    // Get the image fields
    public string GetEyeImageDB()
    {
        return EyeImageDB;
    }
    // Get the image fields
    public string GetNoseImageDB()
    {
        return NoseImageDB;
    }
    // Get the image fields
    public string GetMouthImageDB()
    {
        return MouthImageDB;
    }
    // Get the image fields
    public string GetHeadImageDB()
    {
        return HeadImageDB;
    }

private void SubmitButton_Click(object sender, RoutedEventArgs e)
{
    // Initialize a list to keep track of any missing fields.
    List<string> missingFields = new List<string>();
    
    // Get all elements from the form
    string fname = !string.IsNullOrWhiteSpace(txtFname.Text) && txtFname.Text != "First Name" ? txtFname.Text : null;
    // If the above statement returned null, then put it into the missingFields list
    if (fname == null) missingFields.Add("First Name");

    string lname = !string.IsNullOrWhiteSpace(txtLname.Text) && txtLname.Text != "Last Name" ? txtLname.Text : null;
    if (lname == null) missingFields.Add("Last Name");
    
    string hobby = cbHobby.SelectedItem?.ToString();
    if (string.IsNullOrWhiteSpace(hobby)) missingFields.Add("Hobby");
    
    string occupation = cbJob.SelectedItem?.ToString();
    if (string.IsNullOrWhiteSpace(occupation)) missingFields.Add("Occupation");

    if (string.IsNullOrWhiteSpace(HairImageDB)) missingFields.Add("Hair Image");

    if (string.IsNullOrWhiteSpace(EyeImageDB)) missingFields.Add("Eyes Image");

    if (string.IsNullOrWhiteSpace(NoseImageDB)) missingFields.Add("Nose Image");

    if (string.IsNullOrWhiteSpace(MouthImageDB)) missingFields.Add("Mouth Image");

    if (string.IsNullOrWhiteSpace(HeadImageDB)) missingFields.Add("Face Image");
    

    // If there are any missing fields, show an error message and return early.
    if (missingFields.Count > 0)
    {
        string missing = string.Join(", ", missingFields);
        MessageBox.Show($"Please fill in the following fields: {missing}", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
    }
    
    

    // If all fields are filled, do the database insertion.
    try
    {
        string commandText = "INSERT INTO users (fname, lname, hobby, occupation, hairimage, eyeimage, noseimage, mouthimage, headimage) " +
                             "VALUES (@fname, @lname, @hobby, @occupation, @hairimage, @eyeimage, @noseimage, @mouthimage, @headimage);";

        // Use the ExecuteNonQuery method with parameters.
        db.ExecuteNonQuery(commandText, new Dictionary<string, object>
        {
            { "@fname", fname },
            { "@lname", lname },
            { "@hobby", hobby },
            { "@occupation", occupation },
            { "@hairimage", GetHairImageDB() },
            { "@eyeimage", GetEyeImageDB() },
            { "@noseimage", GetNoseImageDB() },
            { "@mouthimage", GetMouthImageDB() },
            { "@headimage", GetHeadImageDB() }
        });

        // Close the window after the user has been successfully added.
        this.Close();
    }
    catch (Exception ex)
    {
        MessageBox.Show("An error occurred while adding the user to the database: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

    
    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        if (textBox.Foreground == Brushes.Gray)
        {
            textBox.Text = string.Empty;
            textBox.Foreground = Brushes.Black;
        }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            // Set placeholder text if the TextBox is empty
            textBox.Foreground = Brushes.Gray;
            string placeholder = textBox.Tag as string; 
            if (placeholder != null)
            {
                textBox.Text = placeholder;
            }
            else
            {
                // Handle the case where no Tag was set, or set it to a default value
                textBox.Text = "Default Placeholder";
            }
        }
    }
    
        // FACE MAKER STUFF
        // Random variable
        Random random = new Random();

        // Image array definition
        BitmapImage[] faceImages = new BitmapImage[4];
        BitmapImage[] eyeImages = new BitmapImage[3];
        BitmapImage[] hairImages = new BitmapImage[3];
        BitmapImage[] noseImages = new BitmapImage[3];
        BitmapImage[] mouthImages = new BitmapImage[3];

        string imagesDirectory = "./images";

        // Method to load images
        private void LoadImages()
        {
            // Get the absolute path to the images directory
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string absoluteImagesDirectory = Path.Combine(basePath, imagesDirectory);

            foreach (string filePath in Directory.GetFiles(absoluteImagesDirectory, "*.png"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                // Find the first occurrence of any splitter character
                int splitIndex = fileName.IndexOfAny(new char[] { '1', '2', '3', '4' });

                string[] parts;
                if (splitIndex != -1 && splitIndex < fileName.Length - 1)
                {
                    // If a splitter is found and it's not the last character, split normally
                    parts = new[] { fileName.Substring(0, splitIndex), fileName.Substring(splitIndex + 1) };
                }
                else if (splitIndex != -1)
                {
                    // If the splitter is the last character
                    parts = new[] { fileName.Substring(0, splitIndex), fileName[splitIndex].ToString() };
                }
                else
                {
                    // If no splitter is found, perhaps handle as an error or special case
                    continue; 
                }

                string type = parts[0].ToLower();
                if (!int.TryParse(parts[1], out int index))
                {
                    // Handle error or continue with the next file
                    continue;
                }

                index -= 1; // Subtract 1 to convert to 0-based index

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(filePath, UriKind.Absolute);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                switch (type)
                {
                    case "face":
                        if (index < faceImages.Length) faceImages[index] = image;
                        break;
                    case "eyes":
                        if (index < eyeImages.Length) eyeImages[index] = image;
                        break;
                    case "nose":
                        if (index < noseImages.Length) noseImages[index] = image;
                        break;
                    case "mouth":
                        if (index < mouthImages.Length) mouthImages[index] = image;
                        break;
                    case "hair":
                        if (index < hairImages.Length) hairImages[index] = image;
                        break;
                }
            }
        }



        private void btnRandomFace_Click(object sender, RoutedEventArgs e)
        {
            if (chkHair.IsChecked == true)
            {
                RandomizeFeature("hair");
            }
            if (chkEyes.IsChecked == true)
            {
                RandomizeFeature("eyes");
            }
            if (chkNose.IsChecked == true)
            {
                RandomizeFeature("nose");
            }
            if (chkFace.IsChecked == true)
            {
                RandomizeFeature("face");
            }
            if (chkMouth.IsChecked == true)
            {
                RandomizeFeature("mouth");
            }

            if (!(chkMouth.IsChecked == true || chkEyes.IsChecked == true || chkHair.IsChecked == true || chkFace.IsChecked == true || chkNose.IsChecked == true))
            {
                // randomize whole face
                RandomizeFeature("hair");
                RandomizeFeature("eyes");
                RandomizeFeature("nose");
                RandomizeFeature("face");
                RandomizeFeature("mouth");
            }
        }

        private void RandomizeFeature(string feature)
        {
            int num;
            BitmapImage newImage = null;

            switch (feature)
            {
                case "hair":
                    num = random.Next(hairImages.Length);
                    newImage = hairImages[num];
                    num += 1;
                    HairImageDB = $"hair{num}";
                    break;
                case "eyes":
                    num = random.Next(eyeImages.Length);
                    newImage = eyeImages[num];
                    num += 1;
                    EyeImageDB = $"eyes{num}";
                    break;
                case "nose":
                    num = random.Next(noseImages.Length);
                    newImage = noseImages[num];
                    num += 1;
                    NoseImageDB = $"nose{num}";
                    break;
                case "mouth":
                    num = random.Next(mouthImages.Length);
                    newImage = mouthImages[num];
                    num += 1;
                    MouthImageDB = $"mouth{num}";
                    break;
                case "face":
                    num = random.Next(faceImages.Length);
                    newImage = faceImages[num];
                    num += 1;
                    HeadImageDB = $"face{num}";
                    break;


            }

            if (newImage != null)
            {
                setImage(newImage, feature);
            }
        }


        // Method to set image
        private void setImage(BitmapImage image, string type)
        {

            switch (type)
            {
                case "face":
                    
                    faceImage.Source = image;
                    break;
                case "mouth":
                    
                    mouthImage.Source = image;
                    break;
                case "nose":
                    
                    noseImage.Source = image;
                    break;
                case "eyes":
                    
                    eyesImage.Source = image;
                    break;
                case "hair":
                    
                    hairImage.Source = image;
                    break;
            }
        }


}
