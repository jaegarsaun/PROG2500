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
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Collections;
using Cube3D;

namespace Grouping3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// WPF The Easy 3D Way
    /// http://www.i-programmer.info/projects/38-windows/273-easy-3d.html
    /// 
    /// Updated to use more boxes, moving independantly.
    /// </summary>
    public partial class MainWindow : Window
    {

        // list of shapes to display
        ArrayList myShapes;

        // model group used to assign all shapes for display
        Model3DGroup modelGroup;

        /// <summary>
        /// Main window of Shape3D.  
        /// 
        /// Renders 3 boxes, but can add more.  Plus
        /// Base_3D_Shape can be extended to other shapes.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            myShapes = new ArrayList();

            // Update the window on timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += timer_Tick;
            timer.Start();

            // Setup
            this.Env3D_Setup();
        }

        /// <summary>
        /// Timer updates shapes positions each iteration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            // loop instead of hard coded shapes
            for (int i = 0; i < myShapes.Count; i++)
            {
                ((Base_3D_Shape)myShapes[i]).update_Position();
            }

            // Force update of window (though seems unnecessary)
            this.Canvas1.InvalidateVisual();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Do nothing....but this gets called when window closes
            // as defined in XAML
        }



        PerspectiveCamera Camera1 = new PerspectiveCamera();
        /// <summary>
        /// Set up light, camera, and other 3D settings
        /// for the 3D viewport. Shapes are expected to
        /// be in the arraylist and are already added to
        /// the 3D display by being added to the modelGroup
        /// object.
        /// </summary>
        private void Env3D_Setup()
        {

            //Light
            DirectionalLight DirLight1 = new DirectionalLight();
            DirLight1.Color = Colors.White;
            DirLight1.Direction = new Vector3D(-1, -1, -1);

            // Camera
            Camera1 = new PerspectiveCamera();
            Camera1.FarPlaneDistance = 440;
            Camera1.NearPlaneDistance = 1;
            Camera1.FieldOfView = 50;
            //Camera1.Position = new Point3D(200, 200, 200);
            //Camera1.LookDirection = new Vector3D(-20, -20, -20);
            Camera1.UpDirection = new Vector3D(0, 1, 0);

            setCameraPosition(Camera_Position_X_Pos, Camera_Position_Y_Pos, Camera_Position_Z_Pos);

            // add boxes to Model3DGroup object for 3D display
            modelGroup = new Model3DGroup();

            // Model3DGroup object ...just add light, Shapes already added
            modelGroup.Children.Add(DirLight1);

            // Model3DGroup assigned to a ModelVisual3D:
            ModelVisual3D modelsVisual_2 = new ModelVisual3D();
            modelsVisual_2.Content = modelGroup;

            // Veiwport3D
            Viewport3D myViewport = new Viewport3D();
            myViewport.Camera = Camera1;
            myViewport.Children.Add(modelsVisual_2);

            // Canvas
            this.Canvas1.Children.Add(myViewport);

            myViewport.Height = 900;
            myViewport.Width = 900;
            Canvas.SetTop(myViewport, 50);
            Canvas.SetLeft(myViewport, 50);
            this.Width = myViewport.Width;
            this.Height = myViewport.Height;

        }

        // Get better random numbers with one Random generator
        Random r = new Random();
        int inc = 0;
        /// <summary>
        /// When user clicks New Box button, create a 
        /// random box (random position, size, ...)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Box_Click(object sender, RoutedEventArgs e)
        {
            // Call constructor of Rect_3D to create boxes
            Rect_3D newBox = new Rect_3D(position: new Vector3D(r.NextDouble() - 0.5, r.NextDouble() - 0.5, r.NextDouble() - 0.5),
                velocity: new Vector3D(r.NextDouble() - 0.5, r.NextDouble() - 0.5, r.NextDouble() - 0.5),
                height: r.NextDouble() - 0.5, width: r.NextDouble() - 0.5, length: r.NextDouble() - 0.5);

            // Add new box to arraylist
            myShapes.Add(newBox);

            // Add new box to the model
            modelGroup.Children.Add(newBox.gModel3D);


        }

        // Variables for the Grouping
        int distanceThreshold = 25;
        int num_of_points = 40;
        Grouping g = null;
        int selected = 0;
        int numGroups = 0;
        List<List<Dot>> sublists = null;

        private void Do_Grouping_Click(object sender, RoutedEventArgs e)
        {
            if (g != null)
            {

                // each sublist is a grouping
                sublists = g.applyThreseholdsMakeGroups();
                numGroups = sublists.Count;

                for (int i = 0; i < sublists[selected].Count ; i++)
                {
                    //MyLineMethod(l[i], l[i + 1]);    // do something to each point in group
                    // default color
                    Dot d = sublists[selected][i];
                    DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
                    d.Model3D.Material = material;
                }

                // dump groups to console
                Console.WriteLine("# of Groups = " + numGroups);
                for (int i = 0; i < sublists.Count; i++)
                {
                    Console.WriteLine("   Group[" + i + "]");
                    for (int j = 0; j < sublists[i].Count; j++)
                    //foreach (Dot dot in sublists[i])
                    {
                        Dot dot = sublists[i][j];
                        Console.WriteLine("       Dot["+ j+ "] = " + dot.X + "  " + dot.Y + "  " + dot.Z);
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
            foreach (Dot p in g.MasterList)
            {
                modelGroup.Children.Add(p.Model3D);
                // MyDotMethod(p);    // make
            }
        }

        private void Next_Group_Click(object sender, RoutedEventArgs e)
        {

            if ((g != null) && (sublists != null) )
            {

                // reset back to Cyan
                for (int i = 0; i < sublists[selected].Count ; i++)
                {
                    //MyLineMethod(l[i], l[i + 1]);    // do something to each point in group
                    // default color
                    Dot d = sublists[selected][i];
                    DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Cyan));
                    d.Model3D.Material = material;
                }

                // next Group
                int sublist_count = sublists.Count; // number of groups
                selected = (selected+1) % (sublist_count);

                // Selected is Red
                for (int i = 0; i < sublists[selected].Count; i++)
                {
                    //MyLineMethod(l[i], l[i + 1]);    // do something to each point in group
                    // default color
                    Dot d = sublists[selected][i];
                    DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
                    d.Model3D.Material = material;
                }

            }
            else
            {
                MessageBox.Show("No Dots Made Yet ... Press \"Make Dots\"");
            }

            // These dots should now be Red
            Console.WriteLine("Selected = " + selected);
        }

        double Camera_Position_X_base = 100;
        double Camera_Position_Y_base = 100;
        double Camera_Position_Z_base = 100;

        double Camera_Position_X_Pos = 100;
        double Camera_Position_Y_Pos = 100;
        double Camera_Position_Z_Pos = 100;

        private void sliderX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Camera_Position_X_Pos = Camera_Position_X_base + (2*(double)e.NewValue - 50);
            setCameraPosition(Camera_Position_X_Pos, Camera_Position_Y_Pos, Camera_Position_Z_Pos);
        }

        private void sliderY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Camera_Position_Y_Pos = Camera_Position_Y_base + (2*(double)e.NewValue - 50);
            setCameraPosition(Camera_Position_X_Pos, Camera_Position_Y_Pos, Camera_Position_Z_Pos);
        }

        private void sliderZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Camera_Position_Z_Pos = Camera_Position_Z_base + (2 * (double)e.NewValue - 50);
            setCameraPosition(Camera_Position_X_Pos, Camera_Position_Y_Pos, Camera_Position_Z_Pos);
        }

        /// <summary>
        /// Move camera position, but then set look_at to point to origin
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        private void setCameraPosition(double X, double Y, double Z) {

            // find opposite direction to look at origin
            Vector3D v = new Vector3D(Camera1.Position.X, Camera1.Position.Y, Camera1.Position.Z); 
            v.Normalize();
            Vector3D point_to_origin = -v;  // negative opposite direction of position 

            // set camera
            Camera1.Position = new Point3D(X, Y, Z);
            Camera1.LookDirection = point_to_origin;
        }


    }
}
