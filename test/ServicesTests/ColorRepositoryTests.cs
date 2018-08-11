using StroopTest.Interfaces;
using StroopTest.Models;
using StroopTest.Services;
using Xunit;

namespace StroopTest.Tests
{
    public class ColorRepositoryShould
    {
        private readonly IColorRepository _colorRepository;

        public ColorRepositoryShould()
        {
            _colorRepository = new ColorRepository();
        }

        [Fact]
        [Trait("Category", "Services")]
        public void ReturnArrayOfColor_WhenGetColorsIsCalled()
        {
            // Arrange
            // Act
            var actual = _colorRepository.GetColors();

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<Color[]>(actual);
        }
    }
}
