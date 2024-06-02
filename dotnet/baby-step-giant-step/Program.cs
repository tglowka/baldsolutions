// See https://aka.ms/new-console-template for more information

using System.Numerics;
using baby_step_giant_step;

BigInteger mod = BigInteger.Parse("5210644015679228794060694325390955853335898483908056458352183851018372555735221");
BigInteger @base = BigInteger.Parse("598156793758099511761753839885471060009647209443751599122360246998383254800634");
BigInteger result = BigInteger.Parse("3522197356311839067406761763558521917536353254804586391157901835191894911474192");
long limit = 855649200;

long solution = 2139123;

var babyStepGiantStepResult = BabyStepGianStepAlgorithm.Compute(@base, result, mod, limit);
var bruteForceResult = BruteForceAlgorithm.Compute(@base, result, mod, limit);

Console.WriteLine(solution == babyStepGiantStepResult);
Console.WriteLine(solution == bruteForceResult);
Console.WriteLine(result == BigInteger.ModPow(@base, solution, mod));
Console.WriteLine(result == BigInteger.ModPow(@base, babyStepGiantStepResult, mod));
Console.WriteLine(result == BigInteger.ModPow(@base, bruteForceResult, mod));