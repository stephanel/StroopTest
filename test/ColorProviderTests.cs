using Moq;
using StroopTest.Interfaces;
using StroopTest.Services;
using System.Drawing;
using Xunit;

namespace StroopTest.Tests
{
    public class ColorProviderShould
    {
        [Fact]
        [Trait("Category", "Services")]
        public void Test()
        {
            // Arrange
            var mockedColorRepository = new Mock<IColorRepository>();
            mockedColorRepository
                .Setup(x => x.GetInksColors()).Returns(new Color[] { Color.Yellow });
            mockedColorRepository
                .Setup(x => x.GetWordsColors()).Returns(new Color[] { Color.Green });

            var mockedRandomGenerator = new Mock<IRandomGenerator>();
            mockedRandomGenerator
                .Setup(x => x.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var provider = new ColorProvider(mockedColorRepository.Object, mockedRandomGenerator.Object);

            // Act
            var actual = provider.GetRandomColor();

            // Assert
            Assert.Equal("#FFFF00", actual.ColorAsHex);
            Assert.Equal(Color.Green, actual.ColorAsWord);
        }
    }
}
