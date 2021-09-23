using SkiaSharp;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    /// <summary>
    /// Stores all the basic information about displaying graphics.
    /// </summary>
    public interface IDisplayable
    {
        public bool FromColour { get; }

        public SKFilterQuality Quality { get; set; }

        public SKBitmap GetBitmapDisplay();

        public Color GetColourDisplay();

        public RectangleD GetDisplayOffset();

        public void AutoSize();
    }
}
