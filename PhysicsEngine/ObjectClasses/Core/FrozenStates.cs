using PhysicsEngine.ObjectClasses.GameObjects;
using SkiaSharp;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Core
{
    public class ObjectState
    {
        public ObjectState()
        {

        }

        public SKBitmap Display { get; set; }

        private SKBitmap Origin = null;

        private double Width = 0;

        private double Height = 0;

        private SKFilterQuality Quality = 0;

        public bool CheckForRemake(SKBitmap bitmap, double width, double height, SKFilterQuality quality)
        {
            bool output = false;

            if (bitmap != Origin)
            {
                Origin = bitmap;
                output = true;
            }
            if (width != Width)
            {
                Width = width;
                output = true;
            }
            if (height != Height)
            {
                Height = height;
                output = true;
            }
            if (quality != Quality)
            {
                Quality = quality;
                output = true;
            }

            return output;
        }
    }

    public class LightState
    {
        public LightState()
        {

        }

        public SKBitmap Display { get; set; }

        private double Width = 0;

        private double Height = 0;

        private int Size = 0;

        private Color Colour = Color.Empty;

        private double LightQuality = 0;

        private SKFilterQuality Quality = 0;

        public bool CheckForRemake(LightObject referance, double width, double height, double lightquality)
        {
            bool output = false;

            int size = referance.Range;
            Color colour = referance.Colour;
            SKFilterQuality quality = referance.Quality;

            if (width != Width)
            {
                Width = width;
                output = true;
            }
            if (height != Height)
            {
                Height = height;
                output = true;
            }
            if (size != Size)
            {
                Size = size;
                output = true;
            }
            if (colour != Colour)
            {
                Colour = colour;
                output = true;
            }
            if (lightquality != LightQuality)
            {
                LightQuality = lightquality;
                output = true;
            }
            if (quality != Quality)
            {
                Quality = quality;
                output = true;
            }

            return output;
        }
    }
}
