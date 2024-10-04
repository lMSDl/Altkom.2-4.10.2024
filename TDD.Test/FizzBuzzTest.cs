using AutoFixture;

namespace TDD.Test
{
    public class FizzBuzzTest
    {
        [Fact]
        public void Print_ReturnsNNumberOfResults()
        {
            //Arrange
            var fixxBuzz = new FizzBuzz();
            var input = new Fixture().Create<int>();

            //Act
            var output = fixxBuzz.Print(input);

            //Assert
            Assert.Equal(input, output.Split(" ").Length);
        }

        [Theory]
        [InlineData(1, "Fizz", 0)]
        [InlineData(15, "Fizz", 5)]
        [InlineData(100, "Fizz", 33)]
        public void Print_ReturnsFizz(int n, string fizz, int numberOfFizz)
        {
            //Arrange
            var fixxBuzz = new FizzBuzz();

            //Act
            var output = fixxBuzz.Print(n);

            //Assert
            var resultCount = output.Split(" ").Count(x => x.Contains(fizz));
            Assert.Equal(numberOfFizz, resultCount);
        }

        [Theory]
        [InlineData(1, "Buzz", 0)]
        [InlineData(15, "Buzz", 3)]
        [InlineData(100, "Buzz", 20)]
        public void Print_ReturnsBuzz(int n, string buzz, int numberOfBuzz)
        {
            //Arrange
            var fixxBuzz = new FizzBuzz();

            //Act
            var output = fixxBuzz.Print(n);

            //Assert
            var resultCount = output.Split(" ").Count(x => x.Contains(buzz));
            Assert.Equal(numberOfBuzz, resultCount);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(15)]
        [InlineData(100)]
        public void Print_ReturnsNumbers(int count)
        {
            //Arrange
            const string FIZZ = "Fizz";
            const string BUZZ = "Buzz";
            var expected = Enumerable.Range(1, count).Select(x => x.ToString()).ToList();
            var fixxBuzz = new FizzBuzz();

            //Act
            var result = fixxBuzz.Print(count);

            //Assert
            var zip = result.Split(" ").Zip(expected);
            Assert.True(zip.Where(x => !x.First.Contains(FIZZ)).Where(x => !x.First.Contains(BUZZ)).All(x => x.First.Equals(x.Second)));
        }
    }
}