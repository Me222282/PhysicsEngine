using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using System;
using System.Collections.Generic;

namespace PhysicsEngine.ObjectClasses.Core
{
    public delegate void CollisionEventHandler(object sender, CollisionEventArgs e);

    public static class Collisions
    {
        public static CollisionOut CheckCollision(double x, double y, double width, double height, double velocityX, double velocityY, List<PhysicalObject> collisions, DynamicObject sender = null)
        {
            double pointX = x + velocityX;
            double pointY = y + velocityY;

            double nextX = x;
            double nextY = y;

            double l = Math.Min(x, pointX);
            double r = Math.Max(x + width, pointX + width);
            double t = Math.Min(y, pointY);
            double b = Math.Max(y + height, pointY + height);

            double left = x;
            double right = x + width;
            double top = y;
            double bottom = y + height;

            List<CollisionDetection> collide = new List<CollisionDetection>();

            List<PhysicalObject> avalableCollide = new List<PhysicalObject>();

            bool movingX = velocityX != 0;
            bool movingY = velocityY != 0;

            bool PositiveVX = velocityX > 0;
            bool PositiveVY = velocityY > 0;
            bool NegativeVX = velocityX < 0;
            bool NegativeVY = velocityY < 0;

            if (!movingX || !movingY)
            {
                for (int i = 0; i < collisions.Count; i++)
                {
                    PhysicalObject rect = collisions[i];

                    if (rect == sender)
                    {
                        continue;
                    }

                    double rL = rect.Left;
                    double rR = rect.Right;
                    double rT = rect.Top;
                    double rB = rect.Bottom;
                    
                    bool inX;
                    bool inY;

                    if (PositiveVX)
                    {
                        inX = (rR >= left) && (rL < right);
                    }
                    else if (NegativeVX)
                    {
                        inX = (rR > left) && (rL <= right);
                    }
                    else
                    {
                        inX = (rR >= left) && (rL <= right);
                    }
                    if (PositiveVY)
                    {
                        inY = (rT < bottom) && (rB >= top);
                    }
                    else if (NegativeVY)
                    {
                        inY = (rT <= bottom) && (rB > top);
                    }
                    else
                    {
                        inY = (rT <= bottom) && (rB >= top);
                    }

                    if (inX && inY)
                    {
                        continue;
                    }

                    bool outX;
                    bool outY;

                    if (movingX)
                    {
                        outX = (rR < l) || (rL > r);
                        outY = (rT >= b) || (rB <= t);
                    }
                    else
                    {
                        outX = (rR <= l) || (rL >= r);
                        outY = (rT > b) || (rB < t);
                    }

                    // Return if outside the minimum bounding box of the cast.
                    if (outX || outY)
                    {
                        continue;
                    }

                    avalableCollide.Add(rect);


                    CollisionArea collisionArea;

                    double p1;
                    double p2;

                    double distance;

                    if (movingX)
                    {
                        distance = velocityX;

                        if (PositiveVX)
                        {
                            p1 = right;
                            p2 = rL;

                            collisionArea = CollisionArea.Right;
                        }
                        else
                        {
                            p1 = left;
                            p2 = rR;

                            collisionArea = CollisionArea.Left;
                        }
                    }
                    else
                    {
                        distance = velocityY;

                        if (velocityY > 0)
                        {
                            p1 = bottom;
                            p2 = rT;

                            collisionArea = CollisionArea.Bottom;
                        }
                        else
                        {
                            p1 = top;
                            p2 = rB;

                            collisionArea = CollisionArea.Top;
                        }
                    }

                    double percent = (p2 - p1) / distance;

                    collide.Add(new CollisionDetection(rect, percent, collisionArea));
                }
            }
            else
            {
                PointD tLineP1;
                PointD tLineP2;

                PointD bLineP1;
                PointD bLineP2;

                double xModifer = 0;
                double yModifer = 0;

                if (NegativeVX)
                {
                    xModifer = width;
                }
                if (!NegativeVY)
                {
                    bLineP1 = new PointD(x + xModifer, y + height - yModifer);
                    bLineP2 = new PointD(pointX + xModifer, pointY + height - yModifer);

                    tLineP1 = new PointD(x + width - xModifer, y + yModifer);
                    tLineP2 = new PointD(pointX + width - xModifer, pointY + yModifer);
                }
                else
                {
                    yModifer = height;

                    tLineP1 = new PointD(x + xModifer, y + height - yModifer);
                    tLineP2 = new PointD(pointX + xModifer, pointY + height - yModifer);

                    bLineP1 = new PointD(x + width - xModifer, y + yModifer);
                    bLineP2 = new PointD(pointX + width - xModifer, pointY + yModifer);
                }

                PointD mLine = new PointD(x + width - xModifer, y + height - yModifer);

                double gradient = (double)(pointY - y) / (double)(pointX - x);

                double yCeptT = tLineP1.Y - (gradient * tLineP1.X);
                double yCeptB = bLineP1.Y - (gradient * bLineP1.X);

                double findTopY(double x)
                {
                    return (gradient * x) + yCeptT;
                }
                double findBottomY(double x)
                {
                    return (gradient * x) + yCeptB;
                }

                double findTopX(double y)
                {
                    return (y - yCeptT) / gradient;
                }
                double findBottomX(double y)
                {
                    return (y - yCeptB) / gradient;
                }

                double yCeptM = mLine.Y - (gradient * mLine.X);
                double findMidY(double x)
                {
                    return (gradient * x) + yCeptM;
                }

                double distance = Math.Sqrt(Math.Pow(velocityX, 2) + Math.Pow(velocityY, 2));

                for (int i = 0; i < collisions.Count; i++)
                {
                    PhysicalObject rect = collisions[i];

                    if (rect == sender)
                    {
                        continue;
                    }

                    double rL = rect.Left;
                    double rR = rect.Right;
                    double rT = rect.Top;
                    double rB = rect.Bottom;
                    
                    bool inX;
                    bool inY;

                    if (PositiveVX)
                    {
                        inX = (rR >= left) && (rL < right);
                    }
                    else if (NegativeVX)
                    {
                        inX = (rR > left) && (rL <= right);
                    }
                    else
                    {
                        inX = (rR >= left) && (rL <= right);
                    }
                    if (PositiveVY)
                    {
                        inY = (rT < bottom) && (rB >= top);
                    }
                    else if (NegativeVY)
                    {
                        inY = (rT <= bottom) && (rB > top);
                    }
                    else
                    {
                        inY = (rT <= bottom) && (rB >= top);
                    }

                    if (inX && inY)
                    {
                        continue;
                    }

                    // Return if outside the minimum bounding box of the cast.
                    if ((rR < l) || (rL > r) || (rT > b) || (rB < t))
                    {
                        continue;
                    }

                    avalableCollide.Add(rect);

                    if ((findTopY(rL) <= rB || findTopY(rR) <= rB) && (findBottomY(rL) >= rT || findBottomY(rR) >= rT))
                    {
                        double thisX;
                        double thisY;

                        double sX;
                        double sY;

                        double cPointX;

                        CollisionArea collisionArea;

                        if (NegativeVX)
                        {
                            cPointX = rR;
                            collisionArea = CollisionArea.Left;
                        }
                        else
                        {
                            cPointX = rL;
                            collisionArea = CollisionArea.Right;
                        }
                        if (NegativeVY)
                        {
                            if (findMidY(cPointX) < rB)
                            {
                                thisX = cPointX;
                                thisY = findBottomY(cPointX);

                                sX = bLineP1.X;
                                sY = bLineP1.Y;
                            }
                            else
                            {
                                thisX = findTopX(rB);
                                thisY = rB;

                                sX = tLineP1.X;
                                sY = tLineP1.Y;

                                collisionArea = CollisionArea.Top;
                            }
                        }
                        else
                        {
                            if (findMidY(cPointX) > rT)
                            {
                                thisX = cPointX;
                                thisY = findTopY(cPointX);

                                sX = tLineP1.X;
                                sY = tLineP1.Y;
                            }
                            else
                            {
                                thisX = findBottomX(rT);
                                thisY = rT;

                                sX = bLineP1.X;
                                sY = bLineP1.Y;

                                collisionArea = CollisionArea.Bottom;
                            }
                        }

                        double ba = sX - thisX;
                        double hi = sY - thisY;

                        double tDistance = Math.Sqrt((ba * ba) + (hi * hi));

                        double percent = tDistance / distance;

                        collide.Add(new CollisionDetection(rect, percent, collisionArea));
                    }
                }
            }

            if (collide.Count != 0)
            {
                int CompareClosser(CollisionDetection x, CollisionDetection y)
                {
                    if (x == null)
                    {
                        if (y == null)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (y == null)
                        {
                            return 1;
                        }
                        else
                        {
                            return x.Percent.CompareTo(y.Percent);
                        }
                    }
                }

                collide.Sort(CompareClosser);

                #region Fix inaccuracies

                double d = collide[0].Percent;

                d *= 1000;

                if (d > 0)
                {
                    collide[0].Percent = Math.Floor(d) / 1000;
                }
                else if (d < 0)
                {
                    collide[0].Percent = Math.Ceiling(d) / 1000;
                }

                #endregion

                avalableCollide.Remove(collide[0].Object);

                return new CollisionOut(collide[0], avalableCollide);
            }

            return new CollisionOut(CollisionDetection.None, avalableCollide);
        }

        public static CollisionOut CheckCollision(DynamicObject sender, List<PhysicalObject> collisions)
        {
            return CheckCollision(sender.X, sender.Y, sender.Width, sender.Height, sender.VelocityX, sender.VelocityY, collisions, sender);
        }

        public enum CollisionArea
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 3,
            Bottom = 4
        }

        public class CollisionDetection
        {
            public CollisionDetection(PhysicalObject o, double p, CollisionArea c)
            {
                Object = o;
                Percent = p;
                CollisionArea = c;
            }

            public PhysicalObject Object;

            public double Percent;

            public CollisionArea CollisionArea;

            public static CollisionDetection None = new CollisionDetection(null, 1, CollisionArea.None);
        }

        public class CollisionOut
        {
            public CollisionOut(CollisionDetection c, List<PhysicalObject> l)
            {
                Collision = c;
                AvailableCollision = l;
            }

            public CollisionDetection Collision;

            public List<PhysicalObject> AvailableCollision;
        }
    }
}
