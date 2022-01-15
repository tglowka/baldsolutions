using NSubstitute;
using NSubstitute.Extensions;
using nsubstitute_1;
using System;
using Xunit;

namespace nsubstitute_tests
{
    public class CustomClassTests
    {
        [Fact]
        public void RunProcess_CustomServiceExecutedWithPassedParameters_Success()
        {
            //arrange
            var callParameters = (111, 22, 3333);

            var mockDescriptor = Substitute.For<IDescriptor>();
            mockDescriptor.ReturnsForAll<IDescriptor>(mockDescriptor); // Each method within IDescriptor that returns IDescriptor returns mockDescriptor object.

            var customService = Substitute.For<ICustomService>();
            CustomClass customClass = new CustomClass(customService);

            //act
            customClass.RunProcess(
                param_1: callParameters.Item1,
                param_2: callParameters.Item2,
                param_3: callParameters.Item3
            );

            //assert
            customService
                .Received(1)
                .Run(Arg.Is<Func<IDescriptor, IDescriptor>>(
                        descriptorBuilder => AssertDescriptorFunc(
                            descriptorBuilder,
                            mockDescriptor,
                            callParameters
                        )
                    )
                );
        }

        private bool AssertDescriptorFunc(
            Func<IDescriptor, IDescriptor> descriptorFunc,
            IDescriptor mockDescriptor,
            (int, int, int) callParameters
        )
        {
            descriptorFunc(mockDescriptor);

            mockDescriptor.Received(1).BuildMyProperty_1(callParameters.Item1);
            mockDescriptor.Received(1).BuildMyProperty_2(callParameters.Item2);
            mockDescriptor.Received(1).BuildMyProperty_3(callParameters.Item3);

            return true;
        }
    }
}
