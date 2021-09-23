using SkiaSharp;
using System;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    /// <summary>
    /// A colour based <see cref="IDisplayable"/>.
    /// </summary>
    public class ColourDisplay : IDisplayable
    {
        /// <summary>
        /// Creates a colour based <see cref="IDisplayable"/>.
        /// </summary>
        /// <param name="colour">The <see cref="Color"/> of this display.</param>
        public ColourDisplay(Color colour)
        {
            Colour = colour;
        }

        /// <summary>
        /// Whether this is sourced from a colour or a bitmap.
        /// </summary>
        public bool FromColour { get; } = true;

        /// <summary>
        /// Irrelevant to this <see cref="IDisplayable"/> type.
        /// </summary>
        public SKFilterQuality Quality { get; set; }

        /// <summary>
        /// The <see cref="Color"/> of this display.
        /// </summary>
        public Color Colour { get; set; }

        /// <summary>
        /// The offset of the display from its source.
        /// </summary>
        public RectangleD DisplayOffset { get; set; } = new RectangleD(0, 0, 0, 0);

        /// <summary>
        /// Gets the <see cref="Color"/> of this display
        /// </summary>
        /// <returns></returns>
        public Color GetColourDisplay()
        {
            return Colour;
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
        /// Gets the <see cref="SKBitmap"/> of this display.
        /// </summary>
        /// <returns>An empty <see cref="SKBitmap"/>.</returns>
        public SKBitmap GetBitmapDisplay()
        {
            return new SKBitmap(0, 0);
        }

        /// <summary>
        /// Irrelevent to this <see cref="IDisplayable"/> type.
        /// </summary>
        public void AutoSize()
        {
            throw new NotImplementedException();
        }
    }
}
