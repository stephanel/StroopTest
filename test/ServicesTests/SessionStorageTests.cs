using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using StroopTest.Interfaces;
using StroopTest.Models;
using StroopTest.Services;
using StroopTest.Tests.Utils;
using System.Collections.Generic;
using Xunit;

namespace StroopTest.Tests.ServicesTests
{
    public class SessionStorageShould
    {
        [Fact]
        [Trait("Category", "Services")]
        public void Return()
        {
            // Arrange
            string key = "key";

            var expectedData = TestDatas.GetFakeTestPhaseModels();

            byte[] sessionValue = null;
            Mock<ISession> mockedSession = new Mock<ISession>();
            mockedSession
                .Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback((string k, byte[] v) => sessionValue = v);

            mockedSession
                .Setup(x => x.TryGetValue(It.IsAny<string>(), out sessionValue))
                .OutCallback((string k, out byte[] v) => v = sessionValue)
                .Returns(true);


            Mock<HttpContext> mockedHttpContext = new Mock<HttpContext>();
            mockedHttpContext.Setup(x => x.Session)
                .Returns(mockedSession.Object);

            var _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContextAccessor
                .Setup(x => x.HttpContext)
                .Returns(mockedHttpContext.Object);

            ISessionStorage _sessionStorage = new SessionStorage(_mockHttpContextAccessor.Object);

            // Act
            _sessionStorage.SetObjectAsJson(key, expectedData);
            var actual = _sessionStorage.GetObjectFromJson<List<TestPhaseModel>>(key);

            // Assert
            //Assert.Equal(expectedData, actual);
            actual.Should().BeEquivalentTo(expectedData);
        }
    }
}
