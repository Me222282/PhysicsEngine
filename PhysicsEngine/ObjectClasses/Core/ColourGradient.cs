using System;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Core
{
    public class ColourGradient
    {
        public ColourGradient(Color start, Color end, int times)
        {
            R = new Gradient(start.R, end.R, times);
            G = new Gradient(start.G, end.G, times);
            B = new Gradient(start.B, end.B, times);
            A = new Gradient(start.A, end.A, times);
        }

        private readonly Gradient R;
        private readonly Gradient G;
        private readonly Gradient B;
        private readonly Gradient A;

        public bool GradientEnd { get; private set; }

        public Color GetNext()
        {
            byte r = (byte)Math.Round(R.GetNext());
            byte g = (byte)Math.Round(G.GetNext());
            byte b = (byte)Math.Round(B.GetNext());
            byte a = (byte)Math.Round(A.GetNext());

            if (R.GradientEnd || B.GradientEnd || G.GradientEnd || A.GradientEnd)
            {
                GradientEnd = true;
            }

            return Color.FromArgb(a, r, g, b);
        }

        public void Reset()
        {
            GradientEnd = false;
            R.Reset();
            G.Reset();
            B.Reset();
            A.Reset();
        }
    }
}
