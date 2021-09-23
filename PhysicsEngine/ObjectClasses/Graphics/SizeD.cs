namespace PhysicsEngine.ObjectClasses.Graphics
{
    public struct SizeD
    {
        public SizeD(double width, double height)
        {
            this.width = width;
            this.height = height;
            Width = width;
            Height = height;
        }

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
    }
}
