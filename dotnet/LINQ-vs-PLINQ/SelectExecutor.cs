using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQ_vs_PLINQ
{
    public class SelectExecutor
    {
        private readonly Random _random = new Random();
        private readonly List<int> _list;

        public SelectExecutor(int size)
        {
            _list = new List<int>();
            for (int i = 0; i < size; i++)
                _list.Add(_random.Next(100, 200));
        }

        public double SelectLINQ() =>
             _list
                .Select(x => Math.Sqrt(x))
                .Select(x => Math.Tan(x))
                .Select(x => Math.Cos(x))
                .Select(x => Math.Sin(x))
                .Select(x => Math.Log10(x))
                .Max();

        public double SelectPLINQ(int degree) =>
            _list
                .AsParallel()
                .WithDegreeOfParallelism(degree)
                .Select(x => Math.Sqrt(x))
                .Select(x => Math.Tan(x))
                .Select(x => Math.Cos(x))
                .Select(x => Math.Sin(x))
                .Select(x => Math.Log10(x))
                .Max();
    }
}
