using PhysicsEngine.ObjectClasses.Graphics;

namespace PhysicsEngine.ObjectClasses.GameObjects
{
    public class StaticObject : PhysicalObject
    {
        public StaticObject(RectangleD sizeLocation, PhysicsProperty properties)
            : base(sizeLocation, properties)
        {
            
        }

        public StaticObject(RectangleD sizeLocation, PhysicsProperty properties, IDisplayable display)
            : base(sizeLocation, properties, display)
        {
            
        }
    }
}
