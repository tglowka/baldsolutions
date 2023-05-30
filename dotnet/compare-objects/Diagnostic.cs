using System;
using System.Diagnostics;

namespace compare_objects
{
    public class Diagnostic
    {
        private readonly (ParticleWithEquals, ParticleWithEquals) _particlesWithEqualsToCompare;
        private readonly (ParticleWithoutEquals, ParticleWithoutEquals) _particlesWithoutEqualsToCompare;
        private readonly int _equalsOperationCount;

        public Diagnostic(
            (ParticleWithEquals, ParticleWithEquals) particlesWithEqualsToCompare,
            (ParticleWithoutEquals, ParticleWithoutEquals) particlesWithoutEqualsToCompare,
            int equalsOperationCount
        )
        {
            _particlesWithEqualsToCompare = particlesWithEqualsToCompare;
            _particlesWithoutEqualsToCompare = particlesWithoutEqualsToCompare;
            _equalsOperationCount = equalsOperationCount;
        }

        public void MeasureExecutionTimeWithEquals()
        {
            Console.WriteLine("Structs with Equals method overridden");

            int i = 0;

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            while (i < _equalsOperationCount)
            {
                _particlesWithEqualsToCompare.Item1.Equals(_particlesWithEqualsToCompare.Item2);
                ++i;
            }

            stopwatch.Stop();

            Console.WriteLine("Elapsed={0}", stopwatch.Elapsed);
        }

        public void MeasureExecutionTimeWithoutEquals()
        {
            Console.WriteLine("Structs without Equals method overridden");

            int i = 0;

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            while (i < _equalsOperationCount)
            {
                _particlesWithoutEqualsToCompare.Item1.Equals(_particlesWithoutEqualsToCompare.Item2);
                ++i;
            }

            stopwatch.Stop();

            Console.WriteLine("Elapsed={0}", stopwatch.Elapsed);
        }
    }
}
