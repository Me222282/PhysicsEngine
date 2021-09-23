using PhysicsEngine.ObjectClasses.Core;
using SkiaSharp;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.GameObjects
{
    public class LightObject
    {
        public LightObject(Color colour, int range, double x, double y)
        {
            X = x;
            Y = y;

            Range = range;

            Colour = colour;
        }

        public LightObject(Color colour, int range, IGraphicsObject track)
        {
            X = 0;
            Y = 0;

            Track = track;

            Range = range;

            Colour = colour;
        }

        public SKBitmap LightImage { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public int Size
        {
            get
            {
                return Range * 2;
            }
        }

        public int Range { get; set; }

        public Color Colour { get; set; }

        public SKFilterQuality Quality { get; set; } = SKFilterQuality.None;

        public bool TrackObject { get; set; } = false;

        private IGraphicsObject tracking = null;
        public IGraphicsObject Track
        {
            get
            {
                return tracking;
            }
            set
            {
                tracking = value;

                if (value == null)
                {
                    TrackObject = false;

                    return;
                }

                TrackObject = true;
            }
        }

        public LightState ObjectState { get; } = new LightState();

        public void UpdateTracking()
        {
            if (TrackObject)
            {
                X = Track.X + (Track.Width / 2) - Range;
                Y = Track.Y + (Track.Height / 2) - Range;

                if (Track.CenterFromDisplaySize)
                {
                    X += Track.Display.GetDisplayOffset().X;
                    Y += Track.Display.GetDisplayOffset().Y;
                }
            }
        }
    }
}
