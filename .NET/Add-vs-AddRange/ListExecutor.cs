using System.Collections.Generic;
using System.Linq;

namespace Add_vs_AddRange
{
    public class ListExecutor
    {
        public List<int> List { get; }

        public ListExecutor(int size)
        {
            List = Enumerable.Range(0, size).ToList();
        }

        public List<int> AddOnebyOne(List<int> list)
        {
            foreach (var item in list)
                List.Add(item);

            return List;
        }

        public List<int> AddRange(List<int> list)
        {
            List.AddRange(list);

            return List;
        }
    }
}
