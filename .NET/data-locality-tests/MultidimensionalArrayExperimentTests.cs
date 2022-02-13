using data_locality;
using Xunit;

namespace data_locality_tests
{
    public class MultidimensionalArrayExperimentTests
    {
        [Fact]
        public void IterateWriteSequential_AllArrayFieldsSetToFive()
        {
            // Arrange
            var expectedValue = 5;

            var size = 5;
            var arrayExperiment = new MultidimensionalArrayExperiment(size);

            // Act
            arrayExperiment.IterateWriteSequential();

            // Assert
            AssertMultidimensonalArray(arrayExperiment.Array, size, expectedValue);
        }

        [Fact]
        public void IterateWriteNonSequential_AllArrayFieldsSetToFive()
        {
            // Arrange
            var expectedValue = 5;

            var size = 5;
            var arrayExperiment = new MultidimensionalArrayExperiment(size);

            // Act
            arrayExperiment.IterateWriteNonSequential();

            // Assert
            AssertMultidimensonalArray(arrayExperiment.Array, size, expectedValue);
        }

        private void AssertMultidimensonalArray(int[,] array, int size, int expectedValue)
        {
            Assert.Equal(size * size, array.Length);

            foreach (var item in array)
            {
                Assert.Equal(expectedValue, item);
            }
        }
    }
}
