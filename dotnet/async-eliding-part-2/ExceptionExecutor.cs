using System.Threading.Tasks;

namespace async_eliding_part_2
{
    public class ExceptionExecutor
    {
        public async Task<int> DivideWithoutEliding(int dividend, int divider)
        {
            var result = dividend / divider;
            return await Task.Run(() => result);
        }

        public Task<int> DivideWithEliding(int dividend, int divider)
        {
            var result = dividend / divider;
            return Task.Run(() => result);
        }

        public async Task<Task<int>> DivideMany_WithoutEliding(int dividend, int divider)
        {
            var task1 = DivideWithoutEliding(dividend, divider);
            var task2 = DivideWithoutEliding(dividend, divider);
            var task3 = DivideWithoutEliding(dividend, divider);

            return await Task.WhenAny(task1, task2, task3);
        }

        public async Task<Task<int>> DivideMany_WithOneEliding(int dividend, int divider)
        {
            var task1 = DivideWithoutEliding(dividend, divider);
            var task2 = DivideWithoutEliding(dividend, divider);
            var task3 = DivideWithEliding(dividend, divider);

            return await Task.WhenAny(task1, task2, task3);
        }
    }
}