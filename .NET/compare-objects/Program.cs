using System;

namespace compare_objects
{
    class Program
    {
        static void Main(string[] args)
        {
            var equalsOperationCount = 1_000_000_000;

            var obj = new InternalData();
            var particlesWithEquals = (new ParticleWithEquals(1, obj), new ParticleWithEquals(1, obj));
            var particlesWithoutEquals = (new ParticleWithoutEquals(1, obj), new ParticleWithoutEquals(1, obj));

            var diagnostic = new Diagnostic(particlesWithEquals, particlesWithoutEquals, equalsOperationCount);
            diagnostic.MeasureExecutionTimeWithEquals();
            diagnostic.MeasureExecutionTimeWithoutEquals();
        }
    }
}