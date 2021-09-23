using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using PhysicsEngine.ObjectClasses.Core;
using SkiaSharp;
using System.Drawing;

namespace PhysicsEngine
{
    public class Candle : PhysicalObject
    {
        public Candle(PointD position, byte lightLevel, int range, GameEngine engine)
            : base(new RectangleD(position, new SizeD(28, 49)), new PhysicsProperty(50), CandleDisplay.Copy())
        {
            LightObject = new LightObject(Color.FromArgb(lightLevel, LightColour), range, this);

            engine.AddObject(this);
            engine.AddLight(LightObject);
        }

        public Candle(double x, double y, byte lightLevel, int range, GameEngine engine)
            : base(new RectangleD(x, y, 28, 49), new PhysicsProperty(50), CandleDisplay.Copy())
        {
            LightObject = new LightObject(Color.FromArgb(lightLevel, LightColour), range, this);

            UseDisplaySize = true;
            LayerHeight = 2;

            engine.AddObject(this);
            engine.AddLight(LightObject);
        }

        public LightObject LightObject { get; }

        public static void CreateCandleDisplay()
        {
            SKBitmap GraphicsReferance = SKBitmap.Decode("Graphics.png");

            SKBitmap c1 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c1, new SKRectI(132, 153, 140, 174));
            SKBitmap c2 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c2, new SKRectI(148, 153, 156, 174));
            SKBitmap c3 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c3, new SKRectI(164, 153, 172, 174));
            SKBitmap c4 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c4, new SKRectI(180, 153, 188, 174));
            SKBitmap c5 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c5, new SKRectI(196, 153, 204, 174));
            SKBitmap c6 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c6, new SKRectI(212, 153, 220, 174));
            SKBitmap c7 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c7, new SKRectI(228, 153, 236, 174));
            SKBitmap c8 = new SKBitmap();
            GraphicsReferance.ExtractSubset(c8, new SKRectI(244, 153, 252, 174));

            CandleFrames.Add(c1, "Frame1");
            CandleFrames.Add(c2, "Frame2");
            CandleFrames.Add(c3, "Frame3");
            CandleFrames.Add(c4, "Frame4");
            CandleFrames.Add(c5, "Frame5");
            CandleFrames.Add(c6, "Frame6");
            CandleFrames.Add(c7, "Frame7");
            CandleFrames.Add(c8, "Frame8");

            CandleDisplay = new AnimatedDisplay(new SKBitmap[]
            {
                c1,
                c2,
                c3,
                c4,
                c5,
                c6,
                c7,
                c8
            })
            {
                GameFramePerFrame = 4,
                RandomOrder = true,
                DisplayOffset = new RectangleD(0, -24.5, 28, 73.5)
            };
        }

        public static Color LightColour { get; set; } = Color.FromArgb(255, 240, 210);

        private static AnimatedDisplay CandleDisplay;

        private static readonly Collection<SKBitmap> CandleFrames = new Collection<SKBitmap>(); 
    }
}
