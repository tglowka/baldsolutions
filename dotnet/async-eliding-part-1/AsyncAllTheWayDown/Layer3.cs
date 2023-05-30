using System.Threading.Tasks;

namespace async_eliding_part_1.AsyncAllTheWayDown
{
    public class Layer3
    {
        public async Task<int> GimmeDataAsync() => await Task.FromResult(999);
    }
}
