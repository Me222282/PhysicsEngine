using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    /// <summary>
    /// A bitmap based <see cref="IDisplayable"/> that displays an animation from an array of bitmaps.
    /// </summary>
    public class AnimatedDisplay : IDisplayable
    {
        /// <summary>
        /// Creates a bitmap based <see cref="IDisplayable"/> that displays an animation from an array of <see cref="SKBitmap"/>.
        /// </summary>
        /// <param name="frames">An array of <see cref="SKBitmap"/> in the order of the animation.</param>
        /// <param name="loop">Whether the animation should loop automatically.</param>
        public AnimatedDisplay(SKBitmap[] frames, bool loop = true)
        {
            LoopAnimation = loop;

            Bitmaps.AddRange(frames);
        }

        /// <summary>
        /// Creates a bitmap based <see cref="IDisplayable"/> that displays an animation from an array of sources.
        /// </summary>
        /// <param name="frameSources">An array of bitmap sources in the order of the animation.</param>
        /// <param name="loop">Whether the animation should loop automatically.</param>
        public AnimatedDisplay(string[] frameSources, bool loop = true)
        {
            LoopAnimation = loop;

            for (int i = 0; i < frameSources.Length; i++)
            {
                Bitmaps.Add(SKBitmap.Decode(frameSources[i])); ;
            }
        }

        private AnimatedDisplay(List<SKBitmap> frames, int gfpf, bool randomOrder, bool loop, SKFilterQuality quality, RectangleD sizeLocation)
        {
            Bitmaps = frames;
            GameFramePerFrame = gfpf;
            RandomOrder = randomOrder;
            LoopAnimation = loop;
            Quality = quality;
            DisplayOffset = sizeLocation;
        }

        /// <summary>
        /// A list of all <see cref="SKBitmap"/> frames.
        /// </summary>
        public List<SKBitmap> Bitmaps { get; } = new List<SKBitmap>();

        /// <summary>
        /// The current frame number.
        /// </summary>
        public int FramePosition { get; private set; } = 0;

        /// <summary>
        /// The number of games frames per bitmap frame.
        /// </summary>
        public int GameFramePerFrame { get; set; } = 1;

        public int frameCount = 1;

        /// <summary>
        /// If the order of the frames is random.
        /// </summary>
        public bool RandomOrder { get; set; } = false;

        /// <summary>
        /// Whether the animation should loop automatically.
        /// </summary>
        public bool LoopAnimation { get; set; }

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
        /// <returns>The current bitmap in <see cref="Bitmaps"/>.</returns>
        public SKBitmap GetBitmapDisplay()
        {
            Random r = new Random();

            int fp = FramePosition;

            if (frameCount < GameFramePerFrame)
            {
                frameCount++;
            }
            else
            {
                frameCount = 1;

                if (!RandomOrder)
                {
                    if (FramePosition < Bitmaps.Count - 1)
                    {
                        FramePosition++;
                    }
                    else if (LoopAnimation)
                    {
                        FramePosition = 0;
                    }
                }
                else
                {
                    fp = r.Next(Bitmaps.Count - 1);
                    FramePosition = fp;
                }
            }

            return Bitmaps[fp].Copy();
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
        /// Sets the <see cref="FramePosition"/> to 0.
        /// </summary>
        public void RestartAnimation()
        {
            FramePosition = 0;
        }

        /// <summary>
        /// Size the <see cref="DisplayOffset"/> to match the width and height of the current bitmap.
        /// </summary>
        public void AutoSize()
        {
            DisplayOffset.Width = Bitmaps[FramePosition].Width;
            DisplayOffset.Height = Bitmaps[FramePosition].Height;
        }

        /// <summary>
        /// Creates a new <see cref="AnimatedDisplay"/> with the same referance as this <see cref="AnimatedDisplay"/>.
        /// </summary>
        /// <remarks>
        /// Useful for seperating <see cref="AnimatedDisplay"/> for differnet objects so they don't interfere with frame counting.
        /// </remarks>
        /// <returns></returns>
        public AnimatedDisplay Copy()
        {
            return new AnimatedDisplay(Bitmaps, GameFramePerFrame, RandomOrder, LoopAnimation, Quality, DisplayOffset);
        }
    }
}
