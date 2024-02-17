using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;

using InputKeyEventArgs = System.Windows.Input.KeyEventArgs;


namespace M01_First_WPF_Proj
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(HandleKeyBindings);
            try
            {
                LoadImages();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HandleKeyBindings(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.H && Keyboard.Modifiers == ModifierKeys.Control)
            {
                RandomizeFeature("hair");
            }
            else if(e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                RandomizeFeature("nose");
            }
            else if (e.Key == Key.M && Keyboard.Modifiers == ModifierKeys.Control)
            {
                RandomizeFeature("mouth");
            }
            else if (e.Key == Key.E && Keyboard.Modifiers == ModifierKeys.Control)
            {
                RandomizeFeature("eyes");
            }
            else if (e.Key == Key.J && Keyboard.Modifiers == ModifierKeys.Control)
            {
                RandomizeFeature("face");
            }
            else if (e.Key == Key.G && Keyboard.Modifiers == ModifierKeys.Control)
            {
                RandomizeFeature("nose");
                RandomizeFeature("face");
                RandomizeFeature("eyes");
                RandomizeFeature("mouth");
                RandomizeFeature("hair");
            }
            
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            Help.ShowHelp(null, "./faceMakerHelpDoc.chm", HelpNavigator.Topic, "Introduction/Home.chm");
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



        private void menuItemClick(object sender, EventArgs e)
        {
            if (sender is System.Windows.Controls.MenuItem menuItem)
            {
                var feature = menuItem.Tag?.ToString();

                if(feature == "face")
                {
                    RandomizeFeature("hair");
                    RandomizeFeature("eyes");
                    RandomizeFeature("mouth");
                    RandomizeFeature("nose");
                    RandomizeFeature("face");
                }
                else
                {
                    RandomizeFeature(feature);
                }
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
}






















