using System.Collections.Generic;
using System.Linq;

namespace for_vs_foreach
{
    public class CollectionsHolder
    {
        private readonly List<int> _list;
        private readonly int[] _array;

        public CollectionsHolder(int count)
        {
            _list = new List<int>(Enumerable.Range(0, count));
            _array = new int[count];
        }

        public void ForLoopList()
        {
            for (int i = 0; i < _list.Count; i++) { }
        }

        public void ForeachLoopList()
        {
            foreach (var item in _list) { }
        }

        public void ForLoopArray()
        {
            for (int i = 0; i < _array.Length; i++) { }
        }

        public void ForeachLoopArray()
        {
            foreach (var item in _array) { }
        }
    }
}
