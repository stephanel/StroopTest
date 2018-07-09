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

namespace test
{
    public class HomeControllerShould
    {
        [Fact]
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
        public void Redirect_To_NextStep_When_GoToNextStep_Is_Called_With_Step_0_In_Model()
        {
            // Arrange
            var mockSessionStorage = new Mock<ISessionStorage>();
            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings() { StepsCount = 1 };

            var mockTempData = new Mock<ITempDataDictionary>();

            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);
            controller.TempData = mockTempData.Object;

            StepModel model = new StepModel()
            {
                Step = 0
            };

            // Act
            var actual = controller.GoToNextStep(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("NextStep", redirectResult.ActionName);
        }

        [Fact]
        public void Update_Model_In_Session_Then_Redirect_To_NextStep_When_GoToNextStep_Is_Called_With_Step_1_In_Model()
        {
            // Arrange
            var postedModel = new StepModel()
            {
                Step = 1,
                ElapsedTime = 1000,
                SameColor = true,                
            };
            var expectedModel = new StepModel() { Step = 1 };

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
        }

        [Fact]
        public void Return_A_View_Result_When_NextStep_Called()
        {
            // Arrange
            var expectedColorModel = new ColorsModel() {
                ColorAsHex = "#ff0000",
                ColorAsWord = Color.Blue
            };
            var expectedModel = new StepModel() { Step = 1 };

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<StepModel>>(It.IsAny<string>()))
                .Returns(new List<StepModel>() { expectedModel });

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings() { StepsCount = 10 };

            var mockTempData = new Mock<ITempDataDictionary>();
            mockTempData
                .SetupGet(x => x["step"])
                .Returns(1);

            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);
            controller.TempData = mockTempData.Object;

            mockColorProvider
                .Setup(x => x.GetRandomColor())
                .Returns(expectedColorModel);

            // Act
            var actual = controller.NextStep();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsType<StepModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Step);
            Assert.Equal(expectedColorModel.ColorAsHex, model.Colors.ColorAsHex);
            Assert.Equal(expectedColorModel.ColorAsWord, model.Colors.ColorAsWord);
        }

        [Fact]
        public void Redirect_To_Finish_When_NextStep_Is_Called_With_Step_Number_Equal_To_Steps_Count()
        {
            // Arrange
            var expectedColorModel = new ColorsModel() {
                ColorAsHex = "#ff0000",
                ColorAsWord = Color.Blue
            };
            var expectedModel = new StepModel() { Step = 2 };

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<StepModel>>(It.IsAny<string>()))
                .Returns(new List<StepModel>() { expectedModel });

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings() { StepsCount = 2 };

            var mockTempData = new Mock<ITempDataDictionary>();
            mockTempData
                .SetupGet(x => x["step"])
                .Returns(2);

            var controller = new HomeController(mockSessionStorage.Object, mockColorProvider.Object, settings);
            controller.TempData = mockTempData.Object;

            mockColorProvider
                .Setup(x => x.GetRandomColor())
                .Returns(expectedColorModel);

            // Act
            var actual = controller.NextStep();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Finish", redirectResult.ActionName);
        }
    }
}
