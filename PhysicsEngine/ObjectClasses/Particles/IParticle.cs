using PhysicsEngine.ObjectClasses.Core;
using SkiaSharp;

namespace PhysicsEngine.ObjectClasses.Particles
{
    public interface IParticle
    {
        public bool Active { get; }

        public void Trigger();

        public void Stop();

        public void DrawParticle(GameEngine engine, SKCanvas g);
    }
}
