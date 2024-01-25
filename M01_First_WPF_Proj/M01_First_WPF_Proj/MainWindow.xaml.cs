using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace M01_First_WPF_Proj
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Random variable
        Random random = new Random();

        // Image array definition
        BitmapImage[] faceImages = new BitmapImage[4];
        BitmapImage[] eyeImages = new BitmapImage[3];
        BitmapImage[] noseImages = new BitmapImage[3];
        BitmapImage[] noseImages = new BitmapImage[3];

        string imagesDirectory = "path_to_images_directory";


        // Loop through the images folder and add each file to its respected array
        foreach (string filePath in Directory.GetFiles(imagesDirectory, "*.png"))
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string[] parts = fileName.Split(new char[] { '1', '2', '3', '4' }, 2); // Splitting by potential indices
            string type = parts[0].ToLower();
            int index = int.Parse(parts[1]) - 1; // Subtract 1 to convert to 0-based index

            BitmapImage image = new BitmapImage(new Uri(filePath, UriKind.Absolute));

            switch (type)
            {
                 case "face":
                    if (index<faceImages.Length) faceImages[index] = image;
                    break;
                case "eye":
                    if (index<eyeImages.Length) eyeImages[index] = image;
                    break;
                case "nose":
                    if (index<noseImages.Length) noseImages[index] = image;
                    break;
                case "mouth":
                    if (index<mouthImages.Length) mouthImages[index] = image;
                    break;
            }
        }

        

        private void randomize(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                switch (clickedButton.Name)
                {
                    case "btn1":
                        // Randomize Hair

                        break;
                    case "btn2":
                        // Randomize Eyes
                        break;
                    case "btn3":
                        // Randomize Nose
                        break;
                    case "btn4":
                        // Randomize Mouth
                        break;
                    case "btn5":
                        // Randomize Entire Face
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
