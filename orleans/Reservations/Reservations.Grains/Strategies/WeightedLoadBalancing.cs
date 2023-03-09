using Reservations.Interfaces.Grains;
using Reservations.Interfaces.Strategies;

namespace Reservations.Grains.Strategies;

public class WeightedLoadBalancing<T> : IWeightedLoadBalancing<T>
{
    public Task<int> GetIndex(IManagerState<T> managerState)
    {
        var index = 0;

        var random = new Random();
        var number = random.Next(0, managerState.Weights.Sum());

        var left = 0;

        for (int i = 0; i < managerState.Weights.Count; i++)
        {
            var right = left + managerState.Weights[i];

            if (left >= number && number < right)
            {
                index  = i;
                break;
            }

            left = right;
        }

        return Task.FromResult(index);
    }
}
