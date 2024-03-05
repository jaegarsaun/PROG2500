using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading; // For Thread
using System.Collections.Generic; // For List

namespace threads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dorthy dorthy = new Dorthy();
        public MainWindow()
        {
            InitializeComponent();
            dorthy.FavoriteColorChanged += Dorthy_FavoriteColorChanged;
            dorthy.FavoriteCharacterChanged += Dorthy_FavoriteCharacterChanged;
            StartThreads();
        }
        private void StartThreads()
        {
            for (int i = 0; i < 100; i++)
            {
                new Thread(() => { dorthy.FavoriteCharacter = "Tin Man"; dorthy.FavoriteColor = Color.Silver; Thread.Sleep(1000);}).Start();
                new Thread(() => { dorthy.FavoriteCharacter = "Scarecrow"; dorthy.FavoriteColor = Color.Brown;
                    Thread.Sleep(1000);
                }).Start();
                new Thread(() => { dorthy.FavoriteCharacter = "Cowardly Lion"; dorthy.FavoriteColor = Color.Yellow;
                    Thread.Sleep(1000);
                }).Start();
            }
        }
        
        private void Dorthy_FavoriteColorChanged(string color)
        {
            Dispatcher.Invoke(() => favoriteColorTextBox.Text = color);
        }

        private void Dorthy_FavoriteCharacterChanged(string character)
        {
            Dispatcher.Invoke(() => favoriteCharacterTextBox.Text = character);
        }
        
    }
}
