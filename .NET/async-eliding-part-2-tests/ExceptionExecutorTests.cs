using async_eliding_part_2;
using System;
using System.Threading.Tasks;
using Xunit;

namespace async_eliding_part_2_tests
{
    public class ExceptionExecutorTests
    {
        private readonly ExceptionExecutor exceptionExecutor = new ExceptionExecutor();

        [Fact]
        public async Task DivideWithoutEliding_ReturnsDivided()
        {
            // Arrange
            var dividend = 10;
            var divider = 5;
            var expected = 2;

            // Act
            var result = await exceptionExecutor.DivideWithoutEliding(dividend, divider);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task DivideWithoutEliding_DividerIsZeroAndAwaitTheInvocation_ThrowsDivideByZeroException()
        {
            // Arrange
            var dividend = 10;
            var divider = 0;

            // Act & Assert
            await Assert.ThrowsAsync<DivideByZeroException>(
                async () => await exceptionExecutor.DivideWithoutEliding(dividend, divider)
            );
        }

        [Fact]
        public async Task DivideWithoutEliding_DividerIsZeroAndAndAssignTaskToTheVariableFirst_WhenAwaitThrowsDivideByZeroException()
        {
            // Arrange
            var dividend = 10;
            var divider = 0;

            // Act
            var task = exceptionExecutor.DivideWithoutEliding(dividend, divider);

            // Assert
            await Assert.ThrowsAsync<DivideByZeroException>(
                async () => await task
            );
        }

        [Fact]
        public async Task DivideWithEliding_ReturnsDivided()
        {
            // Arrange
            var dividend = 10;
            var divider = 5;
            var expected = 2;

            // Act
            var result = await exceptionExecutor.DivideWithEliding(dividend, divider);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task DivideWithEliding_DividerIsZeroAndAwaitTheInvocation_ThrowsDivideByZeroException()
        {
            // Arrange
            var dividend = 10;
            var divider = 0;

            // Act & Assert
            await Assert.ThrowsAsync<DivideByZeroException>(
                async () => await exceptionExecutor.DivideWithEliding(dividend, divider)
            );
        }

        [Fact]
        public void DivideWithEliding_DividerIsZeroAndAssignTaskToTheVariableFirst_WhenAssignToTheVariableThrowsDivideByZeroException()
        {
            // Arrange
            var dividend = 10;
            var divider = 0;

            var exceptionExecutor = new ExceptionExecutor();

            // Act & Assert
            Assert.Throws<DivideByZeroException>(
                    () =>
                    {
                        var task = exceptionExecutor.DivideWithEliding(dividend, divider);
                    }
            );
        }

        [Fact]
        public async Task DivideMany_WithoutEliding_DividerIsZero_WhenAwaitSecondThrowsDivideByZeroException()
        {
            // Arrange
            var dividend = 10;
            var divider = 0;

            // Act
            var result = await exceptionExecutor.DivideMany_WithoutEliding(dividend, divider);

            // Assert
            await Assert.ThrowsAsync<DivideByZeroException>(
       async () => await result
            );
        }

        [Fact]
        public async Task DivideMany_WithOneEliding_DividerIsZero_WhenFirstAwaitThrowsDivideByZeroException()
        {
            // Arrange
            var dividend = 10;
            var divider = 0;

            // Act & Assert
            await Assert.ThrowsAsync<DivideByZeroException>(
                   async () =>
                   {
                       var result = await exceptionExecutor.DivideMany_WithOneEliding(dividend, divider);
                   }
            );
        }
    }
}
