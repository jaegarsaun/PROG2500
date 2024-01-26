﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Controls;

namespace M01_First_WPF_Proj
{
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
                imageControls = new Dictionary<string, Image>
                {
                    { "hair", hairImage },
                    { "eyes", eyesImage },
                    { "nose", noseImage },
                    { "mouth", mouthImage },
                    { "face", mouthImage }

                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            foreach (string filePath in Directory.GetFiles(imagesDirectory, "*.png"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string[] parts = fileName.Split(new char[] { '1', '2', '3', '4' }, 2); // Splitting by potential indices
                if (parts.Length != 2)
                {
            
                    // Handle error or continue with the next file
                    continue;
                }

                string type = parts[0].ToLower();
                if (!int.TryParse(parts[1], out int index))
                {
                    // Handle error or continue with the next file
                    continue;
                }

                index -= 1; // Subtract 1 to convert to 0-based index

                BitmapImage image = new BitmapImage(new Uri(filePath, UriKind.Absolute));

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


        private void randomize(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                int num;
                BitmapImage newImage;

                switch (clickedButton.Name)
                {
                    case "btn1":
                        // Randomize Hair
                        num = random.Next(hairImages.Length);
                        newImage = hairImages[num];
                        setImage(newImage, "hair");
                        break;
                    case "btn2":
                        // Randomize Eyes
                        num = random.Next(eyeImages.Length);
                        newImage = eyeImages[num];
                        setImage(newImage, "eyes");
                        break;
                    case "btn3":
                        // Randomize Nose
                        num = random.Next(noseImages.Length);
                        newImage = noseImages[num];
                        setImage(newImage, "nose");
                        break;
                    case "btn4":
                        // Randomize Mouth
                        num = random.Next(mouthImages.Length);
                        newImage = mouthImages[num];
                        setImage(newImage, "mouth");
                        break;
                    case "btn5":
                        // Randomize Face
                        // Randomize Mouth
                        num = random.Next(faceImages.Length);
                        newImage = faceImages[num];
                        setImage(newImage, "face");
                        break;
                    default:
                        break;
                }
            }
        }

        // Method to set image
        private void setImage(BitmapImage image, string type)
        {
            if (imageControls.TryGetValue(type, out Image control))
            {
                control.Source = image;
            }
        }
    }
}

