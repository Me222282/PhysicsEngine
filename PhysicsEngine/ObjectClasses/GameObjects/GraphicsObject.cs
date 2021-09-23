using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.Graphics;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.GameObjects
{
    public class GraphicsObject : IGraphicsObject
    {
        public GraphicsObject(RectangleD sizeLocation, IDisplayable display)
        {
            Display = display;

            X = sizeLocation.X;
            Y = sizeLocation.Y;
            Width = sizeLocation.Width;
            Height = sizeLocation.Height;

            UpdateDefaults();
        }

        /// <summary>
        /// The original properties of the object.
        /// </summary>
        public DefaultProperty Default { get; protected set; }

        /// <summary>
        /// Set new defaults
        /// </summary>
        /// <param name="Default"></param>
        public void SetDefaults(DefaultProperty defaultProperty)
        {
            Default = defaultProperty;
        }

        /// <summary>
        /// Sets the defaults to the currents.
        /// </summary>
        public virtual void UpdateDefaults()
        {
            Default = new DefaultProperty(SizeLocation, Display);
        }

        public ObjectState ObjectState { get; } = new ObjectState();

        // 
        // Position & Size
        // 

        /// <summary>
        /// X position
        /// </summary>
        public virtual double X { get; set; }
        /// <summary>
        /// Y position
        /// </summary>
        public virtual double Y { get; set; }
        protected double width;
        /// <summary>
        /// Width
        /// </summary>
        public virtual double Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value < 1)
                {
                    width = 1;
                }
                else
                {
                    width = value;
                }
            }
        }
        protected double height;
        /// <summary>
        /// Height
        /// </summary>
        public virtual double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value < 1)
                {
                    height = 1;
                }
                else
                {
                    height = value;
                }
            }
        }

        public RectangleD SizeLocation
        {
            get
            {
                return new RectangleD(X, Y, Width, Height);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Width = value.Width;
                Height = value.Height;
            }
        }

        public PointD Location
        {
            get
            {
                return new PointD(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public SizeD Size
        {
            get
            {
                return new SizeD(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Position of the left side
        /// </summary>
        public double Left
        {
            get
            {
                return X;
            }
        }
        /// <summary>
        /// Position of the right side
        /// </summary>
        public double Right
        {
            get
            {
                return X + Width;
            }
        }
        /// <summary>
        /// Position of the top
        /// </summary>
        public double Top
        {
            get
            {
                return Y;
            }
        }
        /// <summary>
        /// Position of the bottom
        /// </summary>
        public double Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        public IDisplayable Display { get; set; }

        //public bool CreateShadows { get; set; } = true;

        public bool Glow { get; set; } = false;

        public bool UseDisplaySize { get; set; } = false;

        public bool CenterFromDisplaySize { get; set; } = true;

        public int LayerHeight { get; set; }

        /// <summary>
        /// Is a border drawn or not.
        /// </summary>
        public bool DrawBorder { get; set; } = false;
        /// <summary>
        /// Set the border colour and thickness.
        /// </summary>
        public Pen Border { get; set; } = new Pen(Color.Black, 1);

        /// <summary>
        /// Set all properties to their original value
        /// </summary>
        public virtual void SetDefaults()
        {
            X = Default.X;
            Y = Default.Y;
            Width = Default.Width;
            Height = Default.Height;
            Display = Default.Display;
        }
    }
}
