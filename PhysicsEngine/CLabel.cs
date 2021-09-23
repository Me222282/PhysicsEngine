using System.Drawing;
using System.Windows.Forms;

namespace PhysicsEngine
{
    public class CLabel : Label
    {
        public CLabel()
        {
            MouseDoubleClick += ResetLocation;
            MouseDown += MDown;
            MouseMove += MMove;
            MouseUp += MUp;

            UserMove = new DragAndDrop(this);

            DefualtPos = new Point(0, 0);
            Location = DefualtPos;
        }

        public CLabel(Point defualtPos)
        {
            MouseDoubleClick += ResetLocation;
            MouseDown += MDown;
            MouseMove += MMove;
            MouseUp += MUp;

            UserMove = new DragAndDrop(this);

            DefualtPos = defualtPos;
            Location = DefualtPos;
        }

        private readonly DragAndDrop UserMove;

        public Point DefualtPos { get; set; }

        public bool IsDefualtPos { get; set; } = true;

        public static Size BoundarySize = new Size(1000, 1000);

        private void ResetLocation(object sender, MouseEventArgs mouse)
        {
            IsDefualtPos = true;
            Location = DefualtPos;
        }

        private void MDown(object sender, MouseEventArgs mouse)
        {
            BringToFront();
            UserMove.StartDragAndDrop();
        }

        private void MMove(object sender, MouseEventArgs mouse)
        {
            if (UserMove.IsDragAndDrop)
            {
                UserMove.ApplyDragAndDrop(0, 0, BoundarySize.Width, BoundarySize.Height, Width, Height);
                IsDefualtPos = false;
            }
        }

        private void MUp(object sender, MouseEventArgs mouse)
        {
            UserMove.EndDragAndDrop();
        }
    }

    public class DragAndDrop
    {
        public DragAndDrop(Control aplliedObject)
        {
            AplliedObject = aplliedObject;
        }

        private Point mouseWas = new Point();

        private Point labelWas = new Point();

        private readonly Control AplliedObject = new Control();

        public bool IsDragAndDrop { get; set; } = false;

        private bool MouseHover()
        {
            Rectangle pos = AplliedObject.RectangleToScreen(new Rectangle(new Point(0, 0), AplliedObject.Size));

            Point mPos = Cursor.Position;

            if ((pos.Left > mPos.X) || (pos.Right < mPos.X) || (pos.Top > mPos.Y) || (pos.Bottom < mPos.Y))
            {
                return false;
            }

            return true;
        }

        public void StartDragAndDrop()
        {
            if (Control.MouseButtons != MouseButtons.Left)
            {
                return;
            }

            mouseWas = Control.MousePosition;

            labelWas = AplliedObject.Location;

            IsDragAndDrop = true;
        }

        public void ApplyDragAndDrop
            (
            int boundaryStartX,
            int boundaryStartY,
            int boundaryWidth,
            int boundaryHeight,
            int objectSizeX,
            int objectSizeY
            )
        {
            if (!IsDragAndDrop)
            {
                return;
            }

            // Calculating distance to move object to mouse.
            Point newLocation = new Point(labelWas.X + Control.MousePosition.X - mouseWas.X, labelWas.Y + Control.MousePosition.Y - mouseWas.Y);

            // Restricting movement to given boundarys.
            if (newLocation.X > boundaryStartX + boundaryWidth - objectSizeX) // Right
            {
                newLocation.X = boundaryStartY + boundaryWidth - objectSizeX;

                if (MouseHover())
                {
                    mouseWas.X = Control.MousePosition.X;

                    labelWas.X = AplliedObject.Location.X;
                }
            }
            if (newLocation.X < boundaryStartX) // Left
            {
                newLocation.X = boundaryStartX;

                if (MouseHover())
                {
                    mouseWas.X = Control.MousePosition.X;

                    labelWas.X = AplliedObject.Location.X;
                }
            }
            if (newLocation.Y < boundaryStartY) // Top
            {
                newLocation.Y = boundaryStartY;

                if (MouseHover())
                {
                    mouseWas.Y = Control.MousePosition.Y;

                    labelWas.Y = AplliedObject.Location.Y;
                }
            }
            if (newLocation.Y > boundaryStartY + boundaryHeight - objectSizeY) // Bottom
            {
                newLocation.Y = boundaryStartY + boundaryHeight - objectSizeY;

                if (MouseHover())
                {
                    mouseWas.Y = Control.MousePosition.Y;

                    labelWas.Y = AplliedObject.Location.Y;
                }
            }

            // Applying movement to object.
            AplliedObject.Location = newLocation;
        }

        public void EndDragAndDrop()
        {
            IsDragAndDrop = false;
        }
    }
}
