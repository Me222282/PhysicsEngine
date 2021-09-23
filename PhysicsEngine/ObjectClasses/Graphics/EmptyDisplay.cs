using SkiaSharp;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    /// <summary>
    /// An empty <see cref="IDisplayable"/>
    /// </summary>
    public class EmptyDisplay : IDisplayable
    {
        public EmptyDisplay()
        {
            Colour = Color.Empty;
            Bitmap = new SKBitmap(0, 0);
        }

        public bool FromColour { get; set; } = true;

        public SKFilterQuality Quality { get; set; }

        public RectangleD DisplayOffset { get; set; } = new RectangleD(0, 0, 0, 0);

        public Color Colour { get; set; }

        public SKBitmap Bitmap { get; set; }

        public void AutoSize()
        {
            DisplayOffset.Width = Bitmap.Width;
            DisplayOffset.Height = Bitmap.Height;
        }

        public SKBitmap GetBitmapDisplay()
        {
            return Bitmap;
        }

        public Color GetColourDisplay()
        {
            return Colour;
        }

        public RectangleD GetDisplayOffset()
        {
            return DisplayOffset;
        }
    }
}
