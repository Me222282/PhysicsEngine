using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.Graphics;
using System.Drawing;

namespace PhysicsEngine.ObjectClasses.GameObjects
{
    public delegate void ObjectMoveEventHandler();

    public interface IGraphicsObject
    {
        public DefaultProperty Default { get; }

        public void SetDefaults(DefaultProperty Default);

        public ObjectState ObjectState { get; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public RectangleD SizeLocation { get; set; }

        public PointD Location { get; set; }

        public SizeD Size { get; set; }

        public IDisplayable Display { get; set; }

        //public bool CreateShadows { get; set; }

        public bool Glow { get; set; }

        public bool UseDisplaySize { get; set; }

        public bool CenterFromDisplaySize { get; set; }

        public int LayerHeight { get; set; }

        public bool DrawBorder { get; set; }

        public Pen Border { get; set; }

        public void SetDefaults();
    }
}
