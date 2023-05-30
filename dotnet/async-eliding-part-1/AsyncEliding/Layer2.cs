using System.Threading.Tasks;

namespace async_eliding_part_1.AsyncEliding
{
    public class Layer2
    {
        private readonly Layer3 _layer3;

        public Layer2() => _layer3 = new Layer3();

        public Task<int> GimmeDataAsync() => _layer3.GimmeDataAsync();
    }
}
