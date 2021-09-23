using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.Graphics;
using System;
using System.Collections.Generic;

namespace PhysicsEngine.ObjectClasses.GameObjects
{
    public class DynamicObject : PhysicalObject
    {
        public DynamicObject(RectangleD sizeLocation, PhysicsProperty properties, IDisplayable display)
            : base(sizeLocation, properties, display)
        {
            
        }

        public bool ImitateStaticObject { get; set; }

        public bool UseLocalGravity { get; set; } = false;

        private double gravity;
        public double LocalGravity
        {
            get
            {
                return gravity;
            }
            set
            {
                gravity = value;

                UseLocalGravity = true;
            }
        }

        // 
        // Velocity Stuff
        // 

        /// <summary>
        /// Used to fix a bug with gravity on free objects
        /// </summary>
        protected bool[] UserFYInput = { false, false };

        /// <summary>
        /// Used to fix a bug with gravity on free objects
        /// </summary>
        protected bool[] UserYInput = { false, false };

        /// <summary>
        /// X velocity
        /// </summary>
        public double VelocityX { get; protected set; } = 0;
        private double NewVX = 0;
        private bool VXSet = false;
        /// <summary>
        /// Change the speed of the object X
        /// </summary>
        public double SetVelocityX
        {
            get
            {
                return VelocityX;
            }
            set
            {
                NewVX = value;
                VXSet = true;
            }
        }
        protected double NewFX = 0;
        protected bool FXSet = false;
        /// <summary>
        /// Add a force to the X of the object
        /// </summary>
        public double SetForceX
        {
            get
            {
                return VelocityX;
            }
            set
            {
                NewFX = value;
                FXSet = true;
            }
        }

        /// <summary>
        /// Y velocity
        /// </summary>
        public double VelocityY { get; protected set; } = 0;
        private double NewVY = 0;
        private bool VYSet = false;
        /// <summary>
        /// Change the speed of the object Y
        /// </summary>
        public double SetVelocityY
        {
            get
            {
                return VelocityY;
            }
            set
            {
                NewVY = value;
                VYSet = true;
                if (NewVY < 0)
                {
                    UserYInput[0] = true;
                }
            }
        }
        protected double NewFY = 0;
        protected bool FYSet = false;
        /// <summary>
        /// Add a force to the Y of the object
        /// </summary>
        public double SetForceY
        {
            get
            {
                return VelocityY;
            }
            set
            {
                NewFY = value;
                FYSet = true;
                if (NewFY < 0)
                {
                    UserFYInput[0] = true;
                }
            }
        }

        // 
        // Collision Stuff
        // 

        public event CollisionEventHandler OnCollision;

        /// <summary>
        /// List of all objects colliding horizontally
        /// </summary>
        public PhysicalObject CollisionX { get; private set; }

        /// <summary>
        /// List of all objects colliding vertically
        /// </summary>
        public PhysicalObject CollisionY { get; private set; }

        // 
        // Process stuff
        // 

        /// <summary>
        /// Used by the <see cref="GameEngine"/> to process physics for each frame.
        /// </summary>
        public void CreateFramePhysics()
        {
            #region Physics

            double VX = VelocityX;
            double VY = VelocityY;

            // Set force X a new value if it is not 0;
            if (FXSet == true)
            {
                VX = NewFX;
            }
            // Set force Y a new value if it is not 0;
            if (FYSet == true)
            {
                VY = NewFY;
            }

            // Calculating air resistance
            //double XAirResistance = 1 - height / ((height + width) * 2);
            //double YAirResistance = 1 - width / ((height + width) * 2);

            // Apply friction
            VX *= Properties.Friction;
            //VY *= Properties.Friction;
            /*
            if (BottomCollision.Count == 0 && !(TopCollision.Count > 0 && (UserYInput[1] || UserFYInput[1])) && !(UserYInput[1] && NewVY < 0))
            {
                VY += gravity;
            }*/

            VY += gravity;

            if (VX > -0.1 && VX < 0.1)
            {
                VX = 0;
            }
            if (VY > -0.1 && VY < 0.1)
            {
                VY = 0;
            }

            // Set velocity X a new value if it is not 0;
            if (VXSet)
            {
                VX = NewVX;
            }
            // Set velocity Y a new value if it is not 0;
            if (VYSet)
            {
                VY = NewVY;
            }

            #endregion

            // Round off any inaccurate calculations
            VelocityX = Math.Round(VX, 3);
            VelocityY = Math.Round(VY, 3);
            // Making sure set forces and velocities are not set again
            VXSet = false;
            VYSet = false;
            FXSet = false;
            FYSet = false;
        }

        /// <summary>
        /// Used by the <see cref="GameEngine"/> to calculate physics for each frame.
        /// </summary>
        public void CreateFrameCollisions(List<PhysicalObject> collisions)
        {
            Collisions.CollisionOut collision1 = Collisions.CheckCollision(this, collisions);

            double xMultiplier = 1;
            double yMultiplier = 1;

            bool isCollision1 = false;
            Collisions.CollisionArea cArea1 = 0;
            PhysicalObject collisionObj1 = null;

            bool isCollision2 = false;
            Collisions.CollisionArea cArea2 = 0;
            PhysicalObject collisionObj2 = null;

            if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Left)
            {
                Collisions.CollisionOut collision2 = Collisions.CheckCollision(
                    X + (VelocityX * collision1.Collision.Percent),
                    Y + (VelocityY * collision1.Collision.Percent),
                    Width,
                    Height,
                    0,
                    VelocityY * (1 - collision1.Collision.Percent),
                    collision1.AvailableCollision,
                    this);

                xMultiplier = collision1.Collision.Percent;
                yMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);

                if (collision1.Collision.Object != CollisionX)
                {
                    isCollision1 = true;
                    cArea1 = collision1.Collision.CollisionArea;
                    collisionObj1 = collision1.Collision.Object;
                    CollisionX = collisionObj1;
                }

                if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Top || collision1.Collision.CollisionArea == Collisions.CollisionArea.Bottom)
                    && collision2.Collision.Object != CollisionY)
                {
                    isCollision2 = true;
                    cArea2 = collision2.Collision.CollisionArea;
                    collisionObj2 = collision2.Collision.Object;
                    CollisionY = collisionObj2;
                }
            }
            else if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Right)
            {
                Collisions.CollisionOut collision2 = Collisions.CheckCollision(
                    X + (VelocityX * collision1.Collision.Percent),
                    Y + (VelocityY * collision1.Collision.Percent),
                    Width,
                    Height,
                    0,
                    VelocityY * (1 - collision1.Collision.Percent),
                    collision1.AvailableCollision,
                    this);

                xMultiplier = collision1.Collision.Percent;
                yMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);

                if (collision1.Collision.Object != CollisionX)
                {
                    isCollision1 = true;
                    cArea1 = collision1.Collision.CollisionArea;
                    collisionObj1 = collision1.Collision.Object;
                    CollisionX = collisionObj1;
                }

                if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Top || collision1.Collision.CollisionArea == Collisions.CollisionArea.Bottom)
                    && collision2.Collision.Object != CollisionY)
                {
                    isCollision2 = true;
                    cArea2 = collision2.Collision.CollisionArea;
                    collisionObj2 = collision2.Collision.Object;
                    CollisionY = collisionObj2;
                }
            }
            else if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Top)
            {
                Collisions.CollisionOut collision2 = Collisions.CheckCollision(
                    X + (VelocityX * collision1.Collision.Percent),
                    Y + (VelocityY * collision1.Collision.Percent),
                    Width,
                    Height,
                    VelocityX * (1 - collision1.Collision.Percent),
                    0,
                    collision1.AvailableCollision,
                    this);

                xMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);
                yMultiplier = collision1.Collision.Percent;

                if (collision1.Collision.Object != CollisionY)
                {
                    isCollision1 = true;
                    cArea1 = collision1.Collision.CollisionArea;
                    collisionObj1 = collision1.Collision.Object;
                    CollisionY = collisionObj1;
                }

                if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Left || collision1.Collision.CollisionArea == Collisions.CollisionArea.Right)
                    && collision2.Collision.Object != CollisionX)
                {
                    isCollision2 = true;
                    cArea2 = collision2.Collision.CollisionArea;
                    collisionObj2 = collision2.Collision.Object;
                    CollisionX = collisionObj2;
                }
            }
            else if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Bottom)
            {
                Collisions.CollisionOut collision2 = Collisions.CheckCollision(
                    X + (VelocityX * collision1.Collision.Percent),
                    Y + (VelocityY * collision1.Collision.Percent),
                    Width,
                    Height,
                    VelocityX * (1 - collision1.Collision.Percent),
                    0,
                    collision1.AvailableCollision,
                    this);

                xMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);
                yMultiplier = collision1.Collision.Percent;

                if (collision1.Collision.Object != CollisionY)
                {
                    isCollision1 = true;
                    cArea1 = collision1.Collision.CollisionArea;
                    collisionObj1 = collision1.Collision.Object;
                    CollisionY = collisionObj1;
                }

                if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Left || collision1.Collision.CollisionArea == Collisions.CollisionArea.Right)
                    && collision2.Collision.Object != CollisionX)
                {
                    isCollision2 = true;
                    cArea2 = collision2.Collision.CollisionArea;
                    collisionObj2 = collision2.Collision.Object;
                    CollisionX = collisionObj1;
                }
            }
            else
            {
                if (CollisionX != null)
                {
                    CollisionX = null;
                }
                if (CollisionY != null)
                {
                    CollisionY = null;
                }
            }

            double newX = X + (VelocityX * xMultiplier);
            double newY = Y + (VelocityY * yMultiplier);

            // Apply Location
            X = newX;
            Y = newY;

            // Trigger Collision Events
            if (isCollision1)
            {
                OnCollision?.Invoke(this, new CollisionEventArgs(new RectangleD(newX, newY, Width, height), cArea1, collisionObj1, VelocityX, VelocityY));
            }
            if (isCollision2)
            {
                OnCollision?.Invoke(this, new CollisionEventArgs(new RectangleD(newX, newY, Width, height), cArea2, collisionObj2, VelocityX, VelocityY));
            }

            // Futher Physics Calculations
            if (xMultiplier != 1)
            {
                VelocityX = 0;
            }
            if (yMultiplier != 1)
            {
                VelocityY = 0;
            }
        }

        /// <summary>
        /// Set all properties to their original state
        /// </summary>
        public override void SetDefaults()
        {
            base.SetDefaults();
            VelocityX = 0;
            VelocityY = 0;
        }
    }
}
