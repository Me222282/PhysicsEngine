using System;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    public struct PointD
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// X position
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y position
        /// </summary>
        public double Y { get; set; }

        public double Distance(PointD point)
        {
            double xD = point.X - X;
            double yD = point.Y - Y;

            double output = Math.Sqrt((xD * xD) + (yD * yD));

            if (output < 0)
            {
                output *= -1;
            }

            return output;
        }
    }
}
