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

namespace M01_First_WPF_Proj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Random random = new Random();

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
