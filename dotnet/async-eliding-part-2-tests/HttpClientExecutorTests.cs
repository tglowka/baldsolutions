using async_eliding_part_2;
using System.Threading.Tasks;
using Xunit;

namespace async_eliding_part_2_tests
{
    public class HttpClientExecutorTests
    {
        [Fact]
        public async Task GetWithUsingAndWithoutEliding_Success()
        {
            // Arrange
            var httpClientExecutor = new HttpClientExecutor();

            // Act
            var result = await httpClientExecutor.GetWithUsingAndWithoutEliding();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task GetWithUsingAndWithEliding_ThrowsTaskCanceledException()
        {
            // Arrange
            var httpClientExecutor = new HttpClientExecutor();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(
                async () => await httpClientExecutor.GetWithUsingAndWithEliding()
            );
        }
    }
}
