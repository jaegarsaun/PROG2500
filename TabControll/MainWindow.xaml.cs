
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Imaging;


using System.IO;


namespace TabControll;

public partial class MainWindow : Window
{
    // Dictionary to map type strings to Image controls
    private Dictionary<string, Image> imageControls;
    public MainWindow()
    {
        InitializeComponent();
        try
        {
            LoadImages();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        var radioButton = sender as RadioButton;
        selectedRadioButtonText.Text = $"Animal Preference: {radioButton.Content}";
    }
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
                    break;
                case "eyes":
                    num = random.Next(eyeImages.Length);
                    newImage = eyeImages[num];
                    break;
                case "nose":
                    num = random.Next(noseImages.Length);
                    newImage = noseImages[num];
                    break;
                case "mouth":
                    num = random.Next(mouthImages.Length);
                    newImage = mouthImages[num];
                    break;
                case "face":
                    num = random.Next(faceImages.Length);
                    newImage = faceImages[num];
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
                    faceImage_maker.Source = image;
                    faceImage.Source = image;
                    break;
                case "mouth":
                    mouthImage_maker.Source = image;
                    mouthImage.Source = image;
                    break;
                case "nose":
                    noseImage_maker.Source = image;
                    noseImage.Source = image;
                    break;
                case "eyes":
                    eyesImage_maker.Source = image;
                    eyesImage.Source = image;
                    break;
                case "hair":
                    hairImage_maker.Source = image;
                    hairImage.Source = image;
                    break;
            }
        }
    }