using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;

namespace Cube3D
{
    public class Dot
    {
        /// <summary>
        /// Location 
        /// </summary>
        private Double x;
        private Double y;
        private Double z;

        private Vector3D v;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
        public Vector3D V { get => v; set => v = value; }
        public GeometryModel3D Model3D { get => Model3D1; }

        private readonly GeometryModel3D Model3D1 = null;

        public Dot(double x = 0.0, double y = 0.0, double z = 0.0)
        {

            // local copy
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.v = new Vector3D(x,y,z);

            // import model from file
            var importer = new HelixToolkit.Wpf.ModelImporter();
            var point = importer.Load("dot.obj");
            Model3D1 = point.Children[0] as GeometryModel3D;
            Model3D1.Transform = new TranslateTransform3D(x, y, z);

            // default color
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Cyan));
            Model3D1.Material = material;

        }

        /// <summary>
        /// Distance between this 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Double Distance(Dot d)
        {
            Double dd = 00.0;
            Vector3D thisDot = new Vector3D(this.X, this.Y, this.Z);
            Vector3D thatDot = new Vector3D(d.X, d.Y, d.Z);
            Vector3D dist = (thisDot-thatDot);
            dd = dist.Length;
            return dd;
        }
    }
}
