using PhysicsEngine.ObjectClasses.GameObjects;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    public class Camera
    {
        public Camera()
        {
            X = 0;
            Y = 0;
            Width = double.NaN;
            Height = double.NaN;
        }

        public Camera(double x, double y, double width = double.NaN, double height = double.NaN)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Camera(double width, double height)
        {
            X = 0;
            Y = 0;
            Width = width;
            Height = height;
        }

        public Camera(IGraphicsObject tracking)
        {
            Width = double.NaN;
            Height = double.NaN;

            Track = tracking;

            UpdateTracking();
        }

        /// <summary>
        /// X location
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y location
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The scale of the width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The scale of the height.
        /// </summary>
        public double Height { get; set; }

        public bool TrackObject { get; set; } = false;

        private IGraphicsObject tracker = null;
        /// <summary>
        /// An object that will be placed in the center of the screen at all times.
        /// </summary>
        /// <remarks>
        /// Note: Setting this property will set <see cref="TrackObject"/> to true unless being set to null.
        /// </remarks>
        public IGraphicsObject Track
        {
            get
            {
                return tracker;
            }
            set
            {
                tracker = value;

                if (value == null)
                {
                    TrackObject = false;

                    return;
                }

                TrackObject = true;
            }
        }

        /// <summary>
        /// Will cause the <see cref="X"/> and <see cref="Y"/> to be set to the <see cref="IGraphicsObject.X"/> and <see cref="IGraphicsObject.Y"/> of <see cref="Track"/>.
        /// </summary>
        public void UpdateTracking()
        {
            if (TrackObject)
            {
                X = Track.X + (Track.Width / 2);
                Y = Track.Y + (Track.Height / 2);

                if (Track.CenterFromDisplaySize)
                {
                    X += Track.Display.GetDisplayOffset().X;
                    Y += Track.Display.GetDisplayOffset().Y;
                }
            }
        }
    }
}
