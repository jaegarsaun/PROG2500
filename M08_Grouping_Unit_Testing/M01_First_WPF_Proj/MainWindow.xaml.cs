using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using System.Xml.Linq;

namespace M01_First_WPF_Proj
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        public void MyDotMethod(int xLoc, int yLoc, int size = 2)
        {
            var pixel = new Rectangle();
            pixel.Fill = Brushes.Black;
            pixel.Width = size;
            pixel.Height = size;

            // add pixel to the canvas
            Canvas.SetLeft(pixel, xLoc);
            Canvas.SetTop(pixel, yLoc);
            this.myCanvas.Children.Add(pixel);
        }
        public void MyDotMethod(Point p, int size = 2)
        {
            var pixel = new Rectangle();
            pixel.Fill = Brushes.Black;
            pixel.Width = size;
            pixel.Height = size;

            // add pixel to the canvas
            Canvas.SetLeft(pixel, p.X);
            Canvas.SetTop(pixel, p.Y);
            this.myCanvas.Children.Add(pixel);
        }
        public void MyLineMethod(int xLoc1, int yLoc1, int xLoc2, int yLoc2)
        {
            // Add line to Grid
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = xLoc1;
            myLine.Y1 = yLoc1;
            myLine.X2 = xLoc2 + 50;
            myLine.Y2 = yLoc2 + 50;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            myCanvas.Children.Add(myLine);
        }
        public void MyLineMethod(Point p1, Point p2)
        {
            // Add line to Grid
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = p1.X;
            myLine.Y1 = p1.Y;
            myLine.X2 = p2.X;
            myLine.Y2 = p2.Y;
            myLine.StrokeThickness = 2;
            myCanvas.Children.Add(myLine);
        }

        public void MyCircleMethod(Point p, int size = 100)
        {
            // make symetric ellipse as a circle
            Ellipse e = new Ellipse();
            e.Height = size;
            e.Width = size;
            e.StrokeThickness = 3;
            e.Stroke = System.Windows.Media.Brushes.Red;

            // add pixel to the canvas
            Canvas.SetLeft(e, p.X - size / 2);
            Canvas.SetTop(e, p.Y - size / 2);
            this.myCanvas.Children.Add(e);
        }


        /// <summary>
        /// Random Number generator for line positions.
        /// </summary>
        Random random = new Random();

        /// <summary>
        /// Event Handler for button.  Not renamed, so lots of work to 
        /// name it properly now (but not impossible).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            int rx1 = random.Next(10, 200);
            int ry1 = random.Next(10, 200);
            int rx2 = random.Next(10, 200);
            int ry2 = random.Next(10, 200);

            MyDotMethod(rx1, ry1, 2);

        }

        /// <summary>
        /// Just drag a checkbox onto the grid and double-click it to
        /// create this event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            int rx1 = random.Next(10, 200);
            int ry1 = random.Next(10, 200);
            int rx2 = random.Next(10, 200);
            int ry2 = random.Next(10, 200);
            MyLineMethod(rx1, ry1, rx2, ry2);

        }



        int distanceThreshold = 100;
        int num_of_points = 40;
        Grouping g = null;

        private void Do_Grouping_Click(object sender, RoutedEventArgs e)
        {
            if ((g!=null) && (g.applyThreseholdsMakeGroups() != null))
            {

                // each sublist is a grouping
                List<List<Point>> sublists = g.applyThreseholdsMakeGroups();


                foreach (List<Point> l in sublists)
                {
                    for (int i = 0; i < l.Count - 1; i++)
                    {
                        MyLineMethod(l[i], l[i + 1]);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Dots Made Yet ... Press \"Make Dots\"");
            }
        }

        private void Make_Dots_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Make_Dots_Click was pressed...");

            g = new Grouping(distanceThreshold, num_of_points);  // size of master list generated
            foreach (Point p in g.MasterList)
            {
                MyDotMethod(p);
            }

            //// draw lines from each grouped point to another
            //foreach (Point p in g.MasterList)
            //{
            //    MyCircleMethod(p, distanceThreshold);
            //}

        }

    }
}
