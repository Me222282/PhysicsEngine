using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.Graphics;

namespace PhysicsEngine.ObjectClasses.GameObjects
{
    public abstract class PhysicalObject : GraphicsObject
    {
        public PhysicalObject(RectangleD sizeLocation, PhysicsProperty properties, IDisplayable display)
            : base(sizeLocation, display)
        {
            x = sizeLocation.X;
            y = sizeLocation.Y;
            width = sizeLocation.Width;
            height = sizeLocation.Height;

            Properties = properties;

            UpdateDefaults();
        }

        public PhysicalObject(RectangleD sizeLocation, PhysicsProperty properties)
            : base(sizeLocation, new EmptyDisplay())
        {
            // Set the values for 
            x = sizeLocation.X;
            y = sizeLocation.Y;
            width = sizeLocation.Width;
            height = sizeLocation.Height;

            Properties = properties;

            UpdateDefaults();
        }

        /// <summary>
        /// Properties like friction
        /// </summary>
        public PhysicsProperty Properties { get; set; }

        public double Mass
        {
            get
            {
                return width * height * Properties.Density;
            }
        }

        private double x;
        private double NewX = 0;
        private bool XSet = false;
        /// <summary>
        /// X position
        /// </summary>
        public override double X
        {
            get
            {
                return x;
            }
            set
            {
                NewX = value;
                XSet = true;
            }
        }

        private double y;
        private double NewY = 0;
        private bool YSet = false;
        /// <summary>
        /// Y position
        /// </summary>
        public override double Y
        {
            get
            {
                return y;
            }
            set
            {
                NewY = value;
                YSet = true;
            }
        }

        private double NewWidth = 0;
        private bool WidthSet = false;
        /// <summary>
        /// The width of this.
        /// </summary>
        public override double Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value < 1)
                {
                    NewWidth = 1;
                }
                else
                {
                    NewWidth = value;
                }
                WidthSet = true;
            }
        }

        private double NewHeight = 0;
        private bool HeightSet = false;
        /// <summary>
        /// The height of this.
        /// </summary>
        public override double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value < 1)
                {
                    NewHeight = 1;
                }
                else
                {
                    NewHeight = value;
                }
                HeightSet = true;
            }
        }

        public virtual void UpdateProperties()
        {
            // Set width the new set value
            if (WidthSet == true)
            {
                width = NewWidth;

                WidthSet = false;
            }

            // Set height the new set value
            if (HeightSet == true)
            {
                height = NewHeight;

                HeightSet = false;
            }

            // Set x the new set value
            if (XSet == true)
            {
                x = NewX;

                XSet = false;
            }

            // Set y the new set value
            if (YSet == true)
            {
                y = NewY;

                YSet = false;
            }
        }

        public override void UpdateDefaults()
        {
            Default = new DefaultProperty(new RectangleD(X, Y, Width, Height), Display, Properties);
        }
    }
}
