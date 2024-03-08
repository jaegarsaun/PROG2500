using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Grouping3D
{
    class Rect_3D : Base_3D_Shape
    {
        /// <summary>
        /// Fields of a box (rectangular prism)
        /// </summary>
        private Double height;
        private Double width;
        private Double length;

        /// <summary>
        /// Constructor with default parameters.  If you are
        /// missing parameters, you get the default.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        public Rect_3D(Double height = 1, Double width = 1, Double length = 1)
        {
            Create_Rect_3D(position: Base_3D_Shape.Zero, velocity: Base_3D_Shape.Zero, acceleration: Base_3D_Shape.Zero, height: height, width: width, length: length);
        }

        /// <summary>
        /// Overload constructor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        public Rect_3D(Vector3D position, Double height = 1, Double width = 1, Double length = 1)
        {
            Create_Rect_3D(position: position, velocity: Base_3D_Shape.Zero, acceleration: Base_3D_Shape.Zero, height: height, width: width, length: length);
        }


        /// <summary>
        /// Constructor overlaoded again.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        public Rect_3D(Vector3D position, Vector3D velocity, Double height = 1, Double width = 1, Double length = 1)
        {
            Create_Rect_3D(position: position, velocity: velocity, acceleration: Base_3D_Shape.Zero, height: height, width: width, length: length);
        }

        /// <summary>
        /// This constructor has it all!
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        public Rect_3D(Vector3D position, Vector3D velocity, Vector3D acceleration, Double height = 1, Double width = 1, Double length = 1)
        {
            Create_Rect_3D(position: position, velocity: velocity, acceleration: acceleration, height: height, width: width, length: length);
        }



        /// <summary>
        /// The actual method for creating the box.  All constructors
        /// are meant to call here.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        private void Create_Rect_3D(Vector3D position, Vector3D velocity, Vector3D acceleration, Double height = 1, Double width = 1, Double length = 1)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.height = height;
            this.width = width;
            this.length = length;

            // set up mesh positions 
            mesh_offset_by_position();
        }

        /// <summary>
        /// Set up mesh using position offset
        /// </summary>
        protected override void mesh_offset_by_position()
        {

            Point3DCollection corners = new
                            Point3DCollection();
            corners.Add(new Point3D(length + Position.X, width + Position.Y, height + Position.Z));
            corners.Add(new Point3D(-length + Position.X, width + Position.Y, height + Position.Z));
            corners.Add(new Point3D(-length + Position.X, -width + Position.Y, height + Position.Z));
            corners.Add(new Point3D(length + Position.X, -width + Position.Y, height + Position.Z));
            corners.Add(new Point3D(length + Position.X, width + Position.Y, -height + Position.Z));
            corners.Add(new Point3D(-length + Position.X, width + Position.Y, -height + Position.Z));
            corners.Add(new Point3D(-length + Position.X, -width + Position.Y, -height + Position.Z));
            corners.Add(new Point3D(length + Position.X, -width + Position.Y, -height + Position.Z));


            ///Every mesh stores the array of 
            ///points it uses in its Positions collection:
            mesh3D.Positions = corners;

            /// It is easier to first initialize an
            /// array with the required values:
            Int32[] indices ={
                //front
                0,1,2,
                0,2,3,
                //back
                4,7,6,
                4,6,5,
                //Right
                4,0,3,
                4,3,7,
                //Left
                1,5,6,
                1,6,2,
                //Top
                1,0,4,
                1,4,5,
                //Bottom
                2,6,7,
                2,7,3
            };

            ///and then transfer this using a 
            ///loop to the triangle collection:
            Int32Collection Triangles =
                                  new Int32Collection();
            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }

            ///Finally, set the cube’s TriangleIndices
            mesh3D.TriangleIndices = Triangles;
        }

        /// <summary>
        /// This is the outside call to this object
        /// (via a timer?), to increment it's position
        /// and then recalculate the mesh/model in the 
        /// new position offset.
        /// </summary>
        public override void update_Position()
        {
            RunOneIteration();
            mesh_offset_by_position();
        }
    }
}
