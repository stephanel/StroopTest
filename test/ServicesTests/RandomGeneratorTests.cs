using StroopTest.Interfaces;
using StroopTest.Services;
using Xunit;

namespace StroopTest.Tests.ServicesTests
{
    public class RandomGeneratorShould
    {
        private readonly IRandomGenerator _randomGenertor;

        public RandomGeneratorShould()
        {
            _randomGenertor = new RandomGenerator();
        }

        [Theory]
        [Trait("Category", "Services")]
        [InlineData(0,1)]
        [InlineData(0, 2)]
        [InlineData(0, 3)]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(5, 33)]
        public void Return(int min, int max)
        {
            // Arrange
            // Act
            var actual = _randomGenertor.GetRandomInt(min, max);

            // Assert
            Assert.True(actual >= min);
            Assert.True(actual <= max);
        }
    }
}
