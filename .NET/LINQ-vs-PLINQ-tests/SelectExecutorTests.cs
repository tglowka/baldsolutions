using LINQ_vs_PLINQ;
using Xunit;

namespace LINQ_vs_PLINQ_tests
{
    public class SelectExecutorTests
    {
        [Fact]
        public void SelectLINQ_Equals_SelectPLINQ()
        {
            // Arrange
            var size = 100;
            var selectExecutor = new SelectExecutor(size);

            // Act
            var selectLINQresult = selectExecutor.SelectLINQ();
            var selectPLINQresult = selectExecutor.SelectPLINQ(4);

            // Assert
            Assert.Equal(selectLINQresult, selectPLINQresult); 
        }
    }
}
