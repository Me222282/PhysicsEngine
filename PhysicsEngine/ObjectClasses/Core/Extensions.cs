using PhysicsEngine.ObjectClasses.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the value if inside a set range or confinds it to that range if outside.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inclusiveMinimum"></param>
        /// <param name="inclusiveMaximum"></param>
        /// <returns></returns>
        public static int SetRangeLimit(this int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }

        /// <summary>
        /// Returns the value if inside a set range or confinds it to that range if outside.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inclusiveMinimum"></param>
        /// <param name="inclusiveMaximum"></param>
        /// <returns></returns>
        public static double SetRangeLimitD(this double value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }

        /// <summary>
        /// Rotates a bitmap by a given degrees
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static SKBitmap Rotate(this SKBitmap bitmap, float degrees, PointF rotationPoint = new PointF())
        {
            SKBitmap rotated = new SKBitmap(bitmap.Width, bitmap.Height);

            using (SKCanvas surface = new SKCanvas(rotated))
            {
                if (rotationPoint == PointF.Empty)
                {
                    surface.RotateDegrees(degrees, rotated.Width / 2, rotated.Height / 2);
                }
                else
                {
                    surface.RotateDegrees(degrees, rotationPoint.X, rotationPoint.Y);
                }
                surface.DrawBitmap(bitmap, 0, 0);
            }

            return rotated;
        }

        /// <summary>
        /// Filps a bitmap on the X axis.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static SKBitmap FlipX(this SKBitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            SKBitmap rotated = new SKBitmap(width, height);

            using (SKCanvas surface = new SKCanvas(rotated))
            {
                float xCenter = width / 2;
                float yCenter = height / 2;

                // Translate center to origin
                SKMatrix matrix = SKMatrix.CreateTranslation(-xCenter, -yCenter);

                SKMatrix44 matrix44 = SKMatrix44.CreateIdentity();
                matrix44.PostConcat(SKMatrix44.CreateRotationDegrees(0, 1, 0, 180));

                SKMatrix44 perspectiveMatrix = SKMatrix44.CreateIdentity();
                perspectiveMatrix[3, 2] = -1 / 1;
                matrix44.PostConcat(perspectiveMatrix);

                // Concatenate with 2D matrix
                //SKMatrix.PostConcat(ref matrix, matrix44.Matrix);
                matrix.PostConcat(matrix44.Matrix);

                // Translate back to center
                //SKMatrix.PostConcat(ref matrix,
                //    SKMatrix.CreateTranslation(xCenter, yCenter));
                matrix.PostConcat(SKMatrix.CreateTranslation(xCenter, yCenter));

                // Set the matrix and display the text
                surface.SetMatrix(matrix);

                surface.DrawBitmap(bitmap, 0, 0);
            }

            return rotated;
        }

        /// <summary>
        /// Filps a bitmap on thY axis.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static SKBitmap FlipY(this SKBitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            SKBitmap rotated = new SKBitmap(width, height);

            using (SKCanvas surface = new SKCanvas(rotated))
            {
                float xCenter = width / 2;
                float yCenter = height / 2;

                // Translate center to origin
                SKMatrix matrix = SKMatrix.CreateTranslation(-xCenter, -yCenter);

                SKMatrix44 matrix44 = SKMatrix44.CreateIdentity();
                matrix44.PostConcat(SKMatrix44.CreateRotationDegrees(1, 0, 0, 180));

                SKMatrix44 perspectiveMatrix = SKMatrix44.CreateIdentity();
                perspectiveMatrix[3, 2] = -1 / 1;
                matrix44.PostConcat(perspectiveMatrix);

                // Concatenate with 2D matrix
                //SKMatrix.PostConcat(ref matrix, matrix44.Matrix);
                matrix.PostConcat(matrix44.Matrix);

                // Translate back to center
                //SKMatrix.PostConcat(ref matrix,
                //    SKMatrix.CreateTranslation(xCenter, yCenter));
                matrix.PostConcat(SKMatrix.CreateTranslation(xCenter, yCenter));

                // Set the matrix and display the text
                surface.SetMatrix(matrix);

                surface.DrawBitmap(bitmap, 0, 0);
            }

            return rotated;
        }

        private static readonly Random Random = new Random();

        /// <summary>
        /// Gets a random item from <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this T[] items)
        {
            int i = Random.Next(items.Length - 1);

            return items[i];
        }

        /// <summary>
        /// Gets a random item from <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> items)
        {
            int i = Random.Next(items.Count - 1);

            return items[i];
        }

        /// <summary>
        /// Draws a polygon from <paramref name="points"/> to this <see cref="SKCanvas"/>.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="paint"></param>
        public static void DrawPolygon(this SKCanvas canvas, SKPoint[] points, SKPaint paint)
        {
            SKPath polygon = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };

            polygon.MoveTo(points[0]);
            polygon.AddPoly(points);

            canvas.DrawPath(polygon, paint);
        }

        public static void Rotate(this Point[] positionList, double radians, Point center)
        {
            float centerX = center.X;
            float centerY = center.Y;

            float sin = (float)Math.Sin(radians);
            float cos = (float)Math.Cos(radians);

            for (int i = 0; i < positionList.Length; i++)
            {
                float x = positionList[i].X;
                float y = positionList[i].Y;

                float localX = x - centerX;
                float localY = y - centerY;

                float newLocalX = (float)((localX * cos) - (localY * sin));
                float newLocalY = (float)((localX * sin) + (localY * cos));

                float newX = newLocalX + centerX;
                float newY = newLocalY + centerY;

                positionList[i] = new Point((int)Math.Round(newX), (int)Math.Round(newY));
            }
        }

        public static void Rotate(this PointF[] positionList, double radians, PointF center)
        {
            float centerX = center.X;
            float centerY = center.Y;

            float sin = (float)Math.Sin(radians);
            float cos = (float)Math.Cos(radians);

            for (int i = 0; i < positionList.Length; i++)
            {
                float x = positionList[i].X;
                float y = positionList[i].Y;

                float localX = x - centerX;
                float localY = y - centerY;

                float newLocalX = (float)((localX * cos) - (localY * sin));
                float newLocalY = (float)((localX * sin) + (localY * cos));

                float newX = newLocalX + centerX;
                float newY = newLocalY + centerY;

                positionList[i] = new PointF(newX, newY);
            }
        }

        public static void Rotate(this PointD[] positionList, double radians, PointD center)
        {
            double centerX = center.X;
            double centerY = center.Y;

            double sin = (double)Math.Sin(radians);
            double cos = (double)Math.Cos(radians);

            for (int i = 0; i < positionList.Length; i++)
            {
                double x = positionList[i].X;
                double y = positionList[i].Y;

                double localX = x - centerX;
                double localY = y - centerY;

                double newLocalX = (double)((localX * cos) - (localY * sin));
                double newLocalY = (double)((localX * sin) + (localY * cos));

                double newX = newLocalX + centerX;
                double newY = newLocalY + centerY;

                positionList[i] = new PointD(newX, newY);
            }
        }

        public static void Rotate(this SKPoint[] positionList, double radians, SKPoint center)
        {
            float centerX = center.X;
            float centerY = center.Y;

            float sin = (float)Math.Sin(radians);
            float cos = (float)Math.Cos(radians);

            for (int i = 0; i < positionList.Length; i++)
            {
                float x = positionList[i].X;
                float y = positionList[i].Y;

                float localX = x - centerX;
                float localY = y - centerY;

                float newLocalX = (float)((localX * cos) - (localY * sin));
                float newLocalY = (float)((localX * sin) + (localY * cos));

                float newX = newLocalX + centerX;
                float newY = newLocalY + centerY;

                positionList[i] = new SKPoint(newX, newY);
            }
        }

        public static double ToDegrees(this double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToRadians(this double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
