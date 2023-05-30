namespace data_locality
{
    public class MultidimensionalArrayExperiment
    {
        private readonly int _size;
        private readonly int[,] _array;

        public int[,] Array => _array;

        public MultidimensionalArrayExperiment(int size)
        {
            _size = size;
            _array = new int[_size, _size];
        }

        public void IterateWriteSequential()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _array[i, j] = 5;
                }
            }
        }

        public void IterateWriteNonSequential()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _array[j, i] = 5;
                }
            }
        }
    }
}
