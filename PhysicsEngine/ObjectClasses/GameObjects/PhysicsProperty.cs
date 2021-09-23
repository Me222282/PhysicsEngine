using PhysicsEngine.ObjectClasses.Core;

namespace PhysicsEngine.ObjectClasses.GameObjects
{
    public class PhysicsProperty
    {
        public PhysicsProperty()
        {
            
        }

        public PhysicsProperty(int friction)
        {
            Friction = friction.SetRangeLimit(0, 100);
        }

        public PhysicsProperty(int friction, double density)
        {
            Friction = friction.SetRangeLimit(0, 100);
            Density = density;
        }

        public PhysicsProperty(double density)
        {
            Density = density;
        }

        private double friction = 0;
        /// <summary>
        /// Firction property as a percentage
        /// </summary>
        /// <returns>
        /// Friction as a decimal
        /// </returns>
        public double Friction
        {
            get
            {
                return friction;
            }
            set
            {
                friction = 1 - (value.SetRangeLimitD(0, 100) / 100);
            }
        }

        /// <summary>
        /// The weight per pixel.
        /// </summary>
        /// <remarks>
        /// Default is 1
        /// </remarks>
        public double Density { get; set; } = 1;

        /// <returns>
        /// Friction as a percentage
        /// </returns>
        public double GetFrictionPercentage()
        {
            return friction * 100;
        }
    }
}
