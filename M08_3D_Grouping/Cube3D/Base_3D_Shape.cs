using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Threading;


namespace Grouping3D
{
    /// <summary>
    /// Desctiption:  This is the base 3D class.  
    /// 
    /// After a creating an object, say newShape3D,
    /// set proprties position, velocty, and accelleration.
    /// 
    /// Then call newShape3D.Start() to start the thread
    /// and get the 3D object moving.
    /// 
    /// Author: Russell Shanahan
    /// </summary>
    abstract class Base_3D_Shape
    {
        public MeshGeometry3D mesh3D;
        public GeometryModel3D gModel3D = new GeometryModel3D();

        // Zero vector in 3D as constant (readonly)
        public static readonly Vector3D Zero = new Vector3D(0, 0, 0);

        // Boundary in each direction -/+
        const double bound = 5;

        // Motion
        private Vector3D _position = Zero;
        private Vector3D _velocity = Zero;
        private Vector3D _acceleration = Zero;

        // Encapsulated fields using properties
        public Vector3D Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public Vector3D Velocity
        {
            get
            {
                return _velocity;
            }

            set
            {
                _velocity = value;
            }
        }

        public Vector3D Acceleration
        {
            get
            {
                return _acceleration;
            }

            set
            {
                _acceleration = value;
            }
        }


        /// <summary>
        /// Construct the mesh and model of the base shape.
        /// Even though we don't know the type of shape, 
        /// we do know it needs these objects.
        /// </summary>
        public Base_3D_Shape()
        {
            // Init the base mesh
            mesh3D = new MeshGeometry3D();
            gModel3D.Geometry = mesh3D;
            gModel3D.Material = new DiffuseMaterial(
                      new SolidColorBrush(Colors.Purple));
        }


        // Override in shape
        protected void RunOneIteration()
        {

            // move per acc, velocity
            Position = Position + Velocity;
            Velocity = Velocity + Acceleration;

            // calculate collision with walls (turn around)
            if (Math.Abs(Position.X) > bound) { _velocity.X = -_velocity.X; }
            if (Math.Abs(Position.Y) > bound) { _velocity.Y = -_velocity.Y; }
            if (Math.Abs(Position.Z) > bound) { _velocity.Z = -_velocity.Z; }
        }

        /// <summary>
        /// Abstract method for changing shape's mesh/model
        /// to be offset by it's position.
        /// </summary>
        protected abstract void mesh_offset_by_position();

        /// <summary>
        /// This is the outside call to this object
        /// (via a timer?), to increment it's position
        /// and then recalculate the mesh/model in the 
        /// new position offset.
        /// </summary>
        public abstract void update_Position();


    }
}
