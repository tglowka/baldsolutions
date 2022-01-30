using async_eliding_part_1;
using System.Threading.Tasks;
using Xunit;

namespace async_eliding_part_1_tests
{
    public class ExecutorTests
    {
        private readonly Executor _executor = new Executor();

        [Fact]
        public async Task ExecuteAsyncAllTheWayDown_Returns999()
        {
            // Arrange
            var expected = 999;

            // Act
            var actual = await _executor.ExecuteAsyncAllTheWayDown();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ExecuteAsyncEliding_Returns999()
        {
            // Arrange
            var expected = 999;

            // Act
            var actual = await _executor.ExecuteAsyncEliding();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
