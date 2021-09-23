using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using SkiaSharp;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Core
{
    public class DefaultProperty
    {
        public DefaultProperty(RectangleD sizeLocation, PhysicsProperty properties = null)
        {
            X = sizeLocation.X;
            Y = sizeLocation.Y;
            Width = sizeLocation.Width;
            Height = sizeLocation.Height;

            Properties = properties;

            Display = new Graphics.EmptyDisplay();
        }

        public DefaultProperty(RectangleD sizeLocation, Color colour, PhysicsProperty properties = null)
        {
            X = sizeLocation.X;
            Y = sizeLocation.Y;
            Width = sizeLocation.Width;
            Height = sizeLocation.Height;

            Properties = properties;

            Display = new ColourDisplay(colour);
        }

        public DefaultProperty(RectangleD sizeLocation, SKBitmap image, PhysicsProperty properties = null)
        {
            X = sizeLocation.X;
            Y = sizeLocation.Y;
            Width = sizeLocation.Width;
            Height = sizeLocation.Height;

            Properties = properties;

            Display = new BitmapDisplay(image);
        }

        public DefaultProperty(RectangleD sizeLocation, IDisplayable display, PhysicsProperty properties = null)
        {
            X = sizeLocation.X;
            Y = sizeLocation.Y;
            Width = sizeLocation.Width;
            Height = sizeLocation.Height;

            Properties = properties;

            Display = display;
        }

        /// <summary>
        /// Default property of type Rectangle
        /// </summary>
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        /// <summary>
        /// Default property of type PhysicsProperty
        /// </summary>
        public PhysicsProperty Properties { get; }

        public IDisplayable Display { get; }
    }
}
