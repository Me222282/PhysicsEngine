using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.Graphics;
using SkiaSharp;
using System;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Particles
{
    public class SquareParticle : IParticle
    {
        public SquareParticle(PointD startPoint, PointD endPoint, int time, Color colour)
        {
            Active = false;

            Colour = colour;
            Size = new SizeD(20, 20);
            Time = time;

            double o = endPoint.Y - startPoint.Y;
            double a = endPoint.X - startPoint.X;
            double h = Math.Sqrt((o * o) + (a * a));

            Radians = Math.Sin(o / h);

            FadeGradient = new Gradient(255, 0, time / 2);
            MovementGradientX = new Gradient(startPoint.X, endPoint.X, time);
            MovementGradientY = new Gradient(startPoint.Y, endPoint.Y, time);
        }

        public Color Colour { get; set; }

        public SizeD Size { get; set; }

        private readonly Gradient FadeGradient;

        private Gradient MovementGradientX;
        private Gradient MovementGradientY;

        private double Radians;

        private readonly int Time;

        public void SetPoints(PointD startPoint, PointD endPoint)
        {
            Active = false;
            double o = endPoint.Y - startPoint.Y;
            double a = endPoint.X - startPoint.X;
            double h = Math.Sqrt((o * o) + (a * a));

            Radians = Math.Sin(o / h);

            if (a < 0)
            {
                Radians *= -1;
            }

            MovementGradientX = new Gradient(startPoint.X, endPoint.X, Time);
            MovementGradientY = new Gradient(startPoint.Y, endPoint.Y, Time);
        }

        public bool Active { get; private set; }

        public void Trigger()
        {
            Active = true;
            FadeGradient.Reset();
            MovementGradientX.Reset();
            MovementGradientY.Reset();
        }

        public void Stop()
        {
            Active = false;
            FadeGradient.Reset();
            MovementGradientX.Reset();
            MovementGradientY.Reset();
        }

        public void DrawParticle(GameEngine engine, SKCanvas g)
        {
            double alpha = FadeGradient.GetNext();

            RectangleD SL = new RectangleD(MovementGradientX.GetNext() - Size.Width / 2, MovementGradientY.GetNext() - Size.Height / 2, Size.Width, Size.Height);

            SL = engine.ShiftToScreen(SL);

            float x = (float)SL.X;
            float y = (float)SL.Y;

            float w = (float)SL.Width;
            float h = (float)SL.Height;

            SKPoint[] points = new SKPoint[] { new SKPoint(x, y), new SKPoint(x + w, y), new SKPoint(x + w, y + h), new SKPoint(x, y + h) };

            points.Rotate(Radians, new SKPoint(x + (w / 2), y + (h / 2)));

            g.DrawPolygon(points, new SKPaint() { Color = new SKColor(Colour.R, Colour.G, Colour.B, (byte)Math.Round(alpha)) });

            if (FadeGradient.GradientEnd)
            {
                Stop();
            }
        }
    }
}
