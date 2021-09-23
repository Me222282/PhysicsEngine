using System;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Graphics
{
    public class RectangleD
    {
        public RectangleD(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleD(PointD location, SizeD size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// X position
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y position
        /// </summary>
        public double Y { get; set; }
        private double width;
        /// <summary>
        /// Width
        /// </summary>
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value < 1)
                {
                    width = 1;
                }
                else
                {
                    width = value;
                }
            }
        }
        private double height;
        /// <summary>
        /// Height
        /// </summary>
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value < 1)
                {
                    height = 1;
                }
                else
                {
                    height = value;
                }
            }
        }

        /// <summary>
        /// Position of the left side
        /// </summary>
        public double Left
        {
            get
            {
                return X;
            }
        }
        /// <summary>
        /// Position of the right side
        /// </summary>
        public double Right
        {
            get
            {
                return X + Width;
            }
        }
        /// <summary>
        /// Position of the top
        /// </summary>
        public double Top
        {
            get
            {
                return Y;
            }
        }
        /// <summary>
        /// Position of the bottom
        /// </summary>
        public double Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        /// <summary>
        /// The location fot the rectangle
        /// </summary>
        public PointD Location
        {
            get
            {
                return new PointD(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        /// <summary>
        /// The size of the rectanlge
        /// </summary>
        public SizeD Size
        {
            get
            {
                return new SizeD(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// The top left point
        /// </summary>
        public PointD TopLeft
        {
            get
            {
                return new PointD(X, Y);
            }
        }
        /// <summary>
        /// The top right point
        /// </summary>
        public PointD TopRight
        {
            get
            {
                return new PointD(Right, Y);
            }
        }
        /// <summary>
        /// The bottom left point
        /// </summary>
        public PointD BottomLeft
        {
            get
            {
                return new PointD(X, Bottom);
            }
        }
        /// <summary>
        /// The bottom right point
        /// </summary>
        public PointD BottomRight
        {
            get
            {
                return new PointD(Right, Bottom);
            }
        }

        /// <summary>
        /// Is this <see cref="RectangleD"/> within the bounds of <paramref name="bounds"/>.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public bool WithinBounds(RectangleD bounds)
        {
            if (Right < bounds.Left || Bottom < bounds.Top || Left > bounds.Right || Top > bounds.Bottom)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a <see cref="RectangleD"/> with the location relative to <paramref name="point"/>.
        /// </summary>
        /// <param name="point"></param>
        public RectangleD RelativeToPoint(PointD point)
        {
            return new RectangleD(X - point.X, Y - point.Y, Width, Height);
        }

        /// <summary>
        /// Gets this <see cref="RectangleD"/> as a <see cref="RectangleF"/>
        /// </summary>
        /// <returns></returns>
        public RectangleF ToFloat()
        {
            return new RectangleF((float)X, (float)Y, (float)Width, (float)Height);
        }

        /// <summary>
        /// Gets this <see cref="RectangleD"/> as a <see cref="Rectangle"/>
        /// </summary>
        /// <returns></returns>
        public Rectangle ToInt()
        {
            return new Rectangle((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Width), (int)Math.Round(Height));
        }
    }
}
