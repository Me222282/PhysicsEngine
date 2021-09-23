using SkiaSharp;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    /// <summary>
    /// A bitmap based <see cref="IDisplayable"/>.
    /// </summary>
    public class BitmapDisplay : IDisplayable
    {
        /// <summary>
        /// Creates a bitmap based <see cref="IDisplayable"/> from a <see cref="SKBitmap"/>.
        /// </summary>
        /// <param name="bitmap">The <see cref="SKBitmap"/> to be displayed.</param>
        public BitmapDisplay(SKBitmap bitmap)
        {
            Bitmap = bitmap;
        }

        /// <summary>
        /// Creates a bitmap based <see cref="IDisplayable"/> from a source file.
        /// </summary>
        /// <param name="source">The source of the bitmap.</param>
        public BitmapDisplay(string source)
        {
            Bitmap = SKBitmap.Decode(source);
        }

        /// <summary>
        /// The <see cref="SKBitmap"/> to be displayed.
        /// </summary>
        public SKBitmap Bitmap { get; set; }

        /// <summary>
        /// Whether this is sourced from a colour or a bitmap.
        /// </summary>
        public bool FromColour { get; } = false;

        /// <summary>
        /// The quality of the bitmap displayed.
        /// </summary>
        public SKFilterQuality Quality { get; set; } = SKFilterQuality.None;

        /// <summary>
        /// The offset of the display from its source.
        /// </summary>
        public RectangleD DisplayOffset { get; set; } = new RectangleD(0, 0, 0, 0);

        /// <summary>
        /// Gets the <see cref="SKBitmap"/> of this display.
        /// </summary>
        /// <returns></returns>
        public SKBitmap GetBitmapDisplay()
        {
            return Bitmap.Copy();
        }

        /// <summary>
        /// Gets the <see cref="Color"/> of this display
        /// </summary>
        /// <returns><see cref="Color.Empty"/> as this is a bitmap display</returns>
        public Color GetColourDisplay()
        {
            return Color.Empty;
        }

        /// <summary>
        /// Gets the size and location of this dDisplay.
        /// </summary>
        /// <returns></returns>
        public RectangleD GetDisplayOffset()
        {
            return DisplayOffset;
        }

        /// <summary>
        /// Size the <see cref="DisplayOffset"/> to match the width and height of the <see cref="Bitmap"/>.
        /// </summary>
        public void AutoSize()
        {
            DisplayOffset.Width = Bitmap.Width;
            DisplayOffset.Height = Bitmap.Height;
        }
    }
}
