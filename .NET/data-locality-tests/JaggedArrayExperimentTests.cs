using data_locality;
using Xunit;

namespace data_locality_tests
{
    public class JaggedArrayExperimentTests
    {
        [Fact]
        public void IterateWriteSequential_AllArrayFieldsSetToFive()
        {
            // Arrange
            var expectedValue = 5;

            var size = 7;
            var arrayExperiment = new JaggedArrayExperiment(size);

            // Act
            arrayExperiment.IterateWriteSequential();

            // Assert
            AssertJaggedArray(arrayExperiment.Array, size, expectedValue);
        }

        [Fact]
        public void IterateWriteNonSequential_AllArrayFieldsSetToFive()
        {
            // Arrange
            var expectedValue = 5;

            var size = 7;
            var arrayExperiment = new JaggedArrayExperiment(size);

            // Act
            arrayExperiment.IterateWriteNonSequential();

            // Assert
            AssertJaggedArray(arrayExperiment.Array, size, expectedValue);
        }

        private void AssertJaggedArray(int [][] array, int size, int expectedValue)
        {
            Assert.Equal(size, array.Length);

            foreach (var item in array)
            {
                Assert.Equal(size, item.Length);
                Assert.All(item, (x) => Assert.Equal(expectedValue, x));
            }
        }
    }
}
