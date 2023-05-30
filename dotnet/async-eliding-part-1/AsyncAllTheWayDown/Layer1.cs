using System.Threading.Tasks;

namespace async_eliding_part_1.AsyncAllTheWayDown
{
    public class Layer1
    {
        private readonly Layer2 _layer2;

        public Layer1() => _layer2 = new Layer2();

        public async Task<int> GimmeDataAsync() => await _layer2.GimmeDataAsync();
    }
}
