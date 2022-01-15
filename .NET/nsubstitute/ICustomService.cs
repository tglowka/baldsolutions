using System;

namespace nsubstitute_1
{
    public interface ICustomService
    {
        void Run(Func<IDescriptor, IDescriptor> descriptorBuilder);
    }
}
