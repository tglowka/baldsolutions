namespace nsubstitute_1
{
    public class CustomClass
    {
        private readonly ICustomService _customService;

        public CustomClass(ICustomService customService)
        {
            _customService = customService;
        }

        public void RunProcess(int param_1, int param_2, int param_3)
        {
            _customService.Run(
                descriptorBuilder => descriptorBuilder
                    .BuildMyProperty_1(param_1)
                    .BuildMyProperty_2(param_2)
                    .BuildMyProperty_3(param_3)
            );
        }
    }
}
