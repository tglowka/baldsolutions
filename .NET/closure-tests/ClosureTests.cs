using System;
using Xunit;

namespace closure_tests
{
    public class ClosureTests
    {
        [Fact]
        public void GetFuncWithClosure_InvokeOnce_GetExpectedResult()
        {
            //arrange
            var func = Closure.GetFuncWithClosure(10);

            //act
            var result = func();

            //assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetFuncWithClosure_InvokeMoreThanOnce_GetUnexpectedResult()
        {
            //arrange
            var func = Closure.GetFuncWithClosure(10);

            //act & assert
            var result = func();
            Assert.Equal(2, result);

            Assert.Throws<DivideByZeroException>(() => func());
        }
        
        [Fact]
        public void GetFuncWithoutClosure_InvokeOnce_GetExpectedResult()
        {
            //arrange
            var func = Closure.GetFuncWithoutClosure();

            //act
            var result = func(10);

            //assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetFuncWithoutClosure_InvokeMoreThanOnce_ExpectedResult()
        {
            //arrange
            var func = Closure.GetFuncWithoutClosure();

            //act
            var result = func(10);
            var result2 = func(10);
            
            //assert
            Assert.Equal(2, result);
            Assert.Equal(2, result2);
        }
    }
}