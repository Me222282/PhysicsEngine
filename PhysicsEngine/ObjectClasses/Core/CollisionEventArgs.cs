using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using System;

namespace PhysicsEngine.ObjectClasses.Core
{
    public class CollisionEventArgs : EventArgs
    {
        public CollisionEventArgs(RectangleD location, Collisions.CollisionArea collisionArea, PhysicalObject collisionObj, double vX, double vY)
        {
            Location = location;
            CollisionArea = collisionArea;
            CollisionObj = collisionObj;
            VelocityX = vX;
            VelocityY = vY;
        }

        public PhysicalObject CollisionObj { get; }

        public Collisions.CollisionArea CollisionArea { get; }

        public RectangleD Location { get; }

        public double VelocityX { get; }

        public double VelocityY { get; }
    }
}
