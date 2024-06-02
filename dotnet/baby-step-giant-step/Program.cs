// See https://aka.ms/new-console-template for more information

using System.Numerics;

var A = BigInteger.Parse("21397");
var M = BigInteger.Parse("5210644015679228794060694325390955853335898483908056458352183851018372555735221");
var I = BigInteger.Parse("598156793758099511761753839885471060009647209443751599122360246998383254800634");
var R = BigInteger.Parse("3522197356311839067406761763558521917536353254804586391157901835191894911474192");

// Console.WriteLine(BigInteger.ModPow(I, A, M));
// Console.WriteLine(BabyStepGianStepAlgorithm.Compute(I, R, M, 2139123 * 400));
Console.WriteLine(BruteForceAlgorithm.Compute(I, R, M, 2139123 * 400));
// Console.WriteLine(2139123 * 400);


return;