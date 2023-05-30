namespace Vectorization_tests
{
    public class VectorizationTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void Vectorization_Max_Vectorized(IEnumerable<int> array, int expectedResult)
        {

            //act
            var result = array.Max_Vectorized();

            //assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(array.Max(), result);
        }

        public static IEnumerable<object[]> GetData()
        {
            var array1 = Enumerable.Repeat(1, 14).Append(2).ToArray();
            var array2 = Enumerable.Repeat(1, 15).Append(2).ToArray();
            var array3 = Enumerable.Repeat(1, 16).Append(2).ToArray();

            var data = new List<object[]>
            {
                new object[]{ array1, 2},
                new object[]{ array2, 2},
                new object[]{ array3, 2}
            };

            return data;
        }
    }
}