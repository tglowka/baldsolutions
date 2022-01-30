using System.Threading.Tasks;

namespace async_eliding_part_1
{
    public class Executor
    {
        private readonly AsyncAllTheWayDown.Layer1 _layer1AsyncAllTheWayDown;
        private readonly AsyncEliding.Layer1 _layer1AsyncEliding;

        public Executor()
        {
            _layer1AsyncAllTheWayDown = new AsyncAllTheWayDown.Layer1();
            _layer1AsyncEliding = new AsyncEliding.Layer1();
        }
        public async Task<int> ExecuteAsyncAllTheWayDown() => await _layer1AsyncAllTheWayDown.GimmeDataAsync();
        public async Task<int> ExecuteAsyncEliding() => await _layer1AsyncEliding.GimmeDataAsync();
    }
}
