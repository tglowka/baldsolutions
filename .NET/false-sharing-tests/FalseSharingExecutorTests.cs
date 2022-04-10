using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace false_sharing_tests
{
    public class FalseSharingExecutorTests
    {
        [Fact]
        public async Task ExecuteWithFalseSharing_SumOfInternalListElementsIsCorrect()
        {
            // Arrange
            var processorCount = Environment.ProcessorCount;
            var iterations = 100;

            var falseSharingExecutor = new FalseSharingExecutor(iterations);

            // Act
            await falseSharingExecutor.ExecuteWithFalseSharing();

            // Assert
            Assert.Equal(iterations * processorCount, falseSharingExecutor._list.Sum());
        }

        [Fact]
        public async Task ExecuteFalseSharingWithImprovemenet_SumOfInternalListElementsIsCorrect()
        {
            // Arrange
            var processorCount = Environment.ProcessorCount;
            var iterations = 100;

            var falseSharingExecutor = new FalseSharingExecutor(iterations);

            // Act
            await falseSharingExecutor.ExecuteFalseSharingWithImprovement();

            // Assert
            Assert.Equal(iterations * processorCount, falseSharingExecutor._list2.Sum());
        }

        [Fact]
        public async Task ExecuteFalseSharingWithBetterImprovemenet_SumOfInternalListElementsIsCorrect()
        {
            // Arrange
            var processorCount = Environment.ProcessorCount;
            var iterations = 100;

            var falseSharingExecutor = new FalseSharingExecutor(iterations);

            // Act
            await falseSharingExecutor.ExecuteFalseSharingWithBetterImprovement();

            // Assert
            Assert.Equal(iterations * processorCount, falseSharingExecutor._list3.Sum());
        }
    }
}