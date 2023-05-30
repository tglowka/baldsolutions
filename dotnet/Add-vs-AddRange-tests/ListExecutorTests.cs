using Add_vs_AddRange;
using System.Collections.Generic;
using Xunit;

namespace Add_vs_AddRange_tests
{
    public class ListExecutorTests
    {
        [Fact]
        public void Ctor_CreatesListOfConsecutiveIntsOfPassedSize()
        {
            // Arrange
            var expectedList = new List<int> { 0, 1, 2, 3, 4};

            var count = 5;

            // Act
            var listExecutor = new ListExecutor(count);

            // Assert
            Assert.Equal(expectedList, listExecutor.List);
        }

        [Fact]
        public void AddOnebyOne_AddsElementsToTheList()
        {
            // Arrange
            var expectedList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var count = 5;
            var listExecutor = new ListExecutor(count);

            var listToAdd = new List<int> { 5, 6, 7, 8, 9 };

            // Act
            listExecutor.AddOnebyOne(listToAdd);

            // Assert
            Assert.Equal(expectedList, listExecutor.List);
        }

        [Fact]
        public void AddRange_AddsElementsToTheList()
        {
            // Arrange
            var expectedList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var count = 5;
            var listExecutor = new ListExecutor(count);

            var listToAdd = new List<int> { 5, 6, 7, 8, 9 };

            // Act
            listExecutor.AddRange(listToAdd);

            // Assert
            Assert.Equal(expectedList, listExecutor.List);
        }
    }
}
