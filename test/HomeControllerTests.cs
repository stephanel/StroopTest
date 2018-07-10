using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using StroopTest.Configuration;
using StroopTest.Controllers;
using StroopTest.Interfaces;
using StroopTest.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace StroopTest.Tests
{
    public class HomeControllerShould
    {
        [Fact]
        [Trait("Category", "Controllers")]
        public void Return_A_View_Result_When_Index_Is_Called()
        {
            // Arrange
            var mockSessionStorage = new Mock<ISessionStorage>();
            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings();

            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(actual);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void Redirect_To_NextStep_When_Start_IsCalled()
        {
            // Arrange
            var expectedColorModel = new ColorsModel()
            {
                ColorAsHex = "#ff0000",
                ColorAsWord = Color.Blue
            };
            var expectedModel = new StepModel() { StepNumber = 1 };

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<StepModel>>(It.IsAny<string>()))
                .Returns(new List<StepModel>() { expectedModel });

            var mockColorProvider = new Mock<IColorProvider>();
            mockColorProvider
                .Setup(x => x.GetRandomColor())
                .Returns(expectedColorModel);

            var settings = new StroopTestSettings() { StepsCount = 1 };

            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.Start();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("NextStep", redirectResult.ActionName);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void Return_A_View_Result_When_NextStep_Called()
        {
            // Arrange
            var expectedColorModel = new ColorsModel()
            {
                ColorAsHex = "#ff0000",
                ColorAsWord = Color.Blue
            };
            var expectedModel = new StepModel()
            {
                Colors = expectedColorModel,
                StepNumber = 1,
            };

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<StepModel>>(It.IsAny<string>()))
                .Returns(new List<StepModel>() { expectedModel });

            var mockColorProvider = new Mock<IColorProvider>();
            mockColorProvider
                .Setup(x => x.GetRandomColor())
                .Returns(expectedColorModel);

            var settings = new StroopTestSettings() { StepsCount = 10 };

            var mockTempData = new Mock<ITempDataDictionary>();
            mockTempData
                .SetupGet(x => x[It.IsAny<string>()])
                .Returns(0);

            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);
            controller.TempData = mockTempData.Object;
            
            // Act
            var actual = controller.NextStep();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsType<StepModel>(viewResult.ViewData.Model);
            Assert.Equal(1, model.StepNumber);
            Assert.Equal(expectedColorModel.ColorAsHex, model.Colors.ColorAsHex);
            Assert.Equal(expectedColorModel.ColorAsWord, model.Colors.ColorAsWord);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void Update_Model_In_Session_Then_Redirect_To_NextStep_When_GoToNextStep_Is_Called()
        {
            // Arrange
            var postedModel = new StepModel()
            {
                StepNumber = 1,
                ElapsedTime = 1000,
                SameColor = true,                
            };
            var expectedModel = new StepModel() { StepNumber = 1 };

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<StepModel>>(It.IsAny<string>()))
                .Returns(new List<StepModel>() { expectedModel });

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings() { StepsCount = 1 };

            var mockTempData = new Mock<ITempDataDictionary>();
                
            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);
            controller.TempData = mockTempData.Object;

            // Act
            var actual = controller.GoToNextStep(postedModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("NextStep", redirectResult.ActionName);
            Assert.Equal(expectedModel.ElapsedTime, postedModel.ElapsedTime);
            Assert.Equal(expectedModel.SameColor, postedModel.SameColor);
            mockSessionStorage.Verify(x => x.SetObjectAsJson(It.IsAny<string>(), It.IsAny<List<StepModel>>()));
        }
        
        [Fact]
        [Trait("Category", "Controllers")]
        public void Redirect_To_Finish_When_NextStep_Is_Called_With_Step_Number_Equal_To_Steps_Count()
        {
            // Arrange
            var expectedColorModel = new ColorsModel() {
                ColorAsHex = "#ff0000",
                ColorAsWord = Color.Blue
            };
            var expectedModel = new StepModel() { StepNumber = 2 };

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<StepModel>>(It.IsAny<string>()))
                .Returns(new List<StepModel>() { expectedModel });

            var mockColorProvider = new Mock<IColorProvider>();
            mockColorProvider
                .Setup(x => x.GetRandomColor())
                .Returns(expectedColorModel);

            var settings = new StroopTestSettings() { StepsCount = 2 };

            var mockTempData = new Mock<ITempDataDictionary>();
            mockTempData
                .SetupGet(x => x[It.IsAny<string>()])
                .Returns(2);

            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);
            controller.TempData = mockTempData.Object;

            // Act
            var actual = controller.NextStep();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Finish", redirectResult.ActionName);
        }
    }
}
