using Moq;
using StroopTest.Extensions;
using StroopTest.Interfaces;
using StroopTest.Models;
using StroopTest.Services;
using Xunit;

namespace StroopTest.Tests
{
    public class ColorProviderShould
    {
        [Fact]
        [Trait("Category", "Services")]
        public void Return_Congruent_ColorsModel_When_GetCongruentColor_Is_Called()
        {
            // Arrange
            var mockedColorRepository = new Mock<IColorRepository>();
            mockedColorRepository
                .Setup(x => x.GetColors()).Returns(new Color[] { Color.YELLOW });

            var mockedRandomGenerator = new Mock<IRandomGenerator>();
            mockedRandomGenerator
                .Setup(x => x.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var provider = new ColorProvider(mockedColorRepository.Object, mockedRandomGenerator.Object);

            // Act
            var actual = provider.GetCongruentColor();

            // Assert
            Assert.Equal("#FFFF00", actual.ColorAsHex);
            Assert.Equal(Color.YELLOW, actual.ColorAsWord);
        }

        [Fact]
        [Trait("Category", "Services")]
        public void Return_Incongruent_ColorsModel_When_GetIncongruentColor_Is_Called()
        {
            // Arrange
            var mockedColorRepository = new Mock<IColorRepository>();
            mockedColorRepository
                .Setup(x => x.GetColors()).Returns(new Color[] { Color.YELLOW, Color.BLUE });

            var mockedRandomGenerator = new Mock<IRandomGenerator>();
            mockedRandomGenerator
                .Setup(x => x.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var provider = new ColorProvider(mockedColorRepository.Object, mockedRandomGenerator.Object);

            // Act
            var actual = provider.GetIncongruentColor();

            // Assert
            Assert.Equal(Color.YELLOW, actual.ColorAsWord);
            Assert.NotEqual(actual.ColorAsHex, actual.ColorAsWord.ToHex());
        }

        [Fact]
        [Trait("Category", "Services")]
        public void Return_Neutral_ColorsModel_When_GetNeutralColor_Is_Called()
        {
            // Arrange
            var mockedColorRepository = new Mock<IColorRepository>();
            mockedColorRepository
                .Setup(x => x.GetColors()).Returns(new Color[] { Color.YELLOW });

            var mockedRandomGenerator = new Mock<IRandomGenerator>();
            mockedRandomGenerator
                .Setup(x => x.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var provider = new ColorProvider(mockedColorRepository.Object, mockedRandomGenerator.Object);

            // Act
            var actual = provider.GetNeutralColor();

            // Assert
            Assert.Equal("#000000", actual.ColorAsHex);
            Assert.Equal(Color.YELLOW, actual.ColorAsWord);
        }
    }
}
