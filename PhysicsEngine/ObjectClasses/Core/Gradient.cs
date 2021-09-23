namespace PhysicsEngine.ObjectClasses.Core
{
    public class Gradient
    {
        public Gradient(double start, double end, int times)
        {
            if (end > start) { PositiveDirect = true; }
            else { PositiveDirect = false; }

            Curent = start;
            Start = start;
            End = end;
            Changer = (end - start) / times;
        }

        private double Curent;

        private readonly double Start;

        private readonly double End;

        private readonly double Changer;

        private readonly bool PositiveDirect;

        public bool GradientEnd { get; private set; }

        public double GetNext()
        {
            Curent += Changer;

            if (PositiveDirect)
            {
                if (Curent >= End)
                {
                    Curent = End;
                    GradientEnd = true;
                    return End;
                }
            }
            else
            {
                if (Curent <= End)
                {
                    Curent = End;
                    GradientEnd = true;
                    return End;
                }
            }

            return Curent;
        }

        public void Reset()
        {
            GradientEnd = false;
            Curent = Start;
        }
    }
}
