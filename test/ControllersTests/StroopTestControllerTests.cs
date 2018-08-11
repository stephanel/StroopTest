using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using StroopTest.Configuration;
using StroopTest.Controllers;
using StroopTest.Interfaces;
using StroopTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StroopTest.Tests
{
    public class HomeControllerShould
    {
        [Fact]
        [Trait("Category", "Controllers")]
        public void ReturnAViewResult_WhenIndexIsCalled()
        {
            // Arrange
            var mockSessionStorage = new Mock<ISessionStorage>();
            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(actual);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void StoreColorsModels_WhenStartIsCalled()
        {
            // Arrange
            List<TestPhaseModel> model = new List<TestPhaseModel>();

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()))
                .Returns(model);

            var mockColorProvider = new Mock<IColorProvider>();

            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.Start();

            // Assert
            mockColorProvider.Verify(x => x.GetCongruentColor(), Times.Exactly(20));
            mockColorProvider.Verify(x => x.GetIncongruentColor(), Times.Exactly(20));
 
            mockSessionStorage.Verify(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()), Times.Once);

            Assert.Equal(2, model.Count);

            var modelPhase1 = model[0];
            Assert.Equal(1, modelPhase1.PhaseNumber);
            Assert.Equal(20, modelPhase1.StepModels.Count);

            var modelPhase2 = model[1];
            Assert.Equal(2, modelPhase2.PhaseNumber);
            Assert.Equal(20, modelPhase2.StepModels.Count);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void RedirectToStartTestPhase_WhenStartIsCalled()
        {
            // Arrange
            var expectedModels = TestDatas.GetFakeTestPhaseModels();
            string expectedRedirectAction = "StartTestPhase";

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()))
                .Returns(expectedModels);

            var mockColorProvider = new Mock<IColorProvider>();

            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.Start();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(expectedRedirectAction, redirectResult.ActionName);

            mockSessionStorage.Verify(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [Trait("Category", "Controllers")]
        [InlineData(1, "PHASE 1", "CONGRUENT WORDS", 1)]
        [InlineData(2, "PHASE 2", "INCONGRUENT WORDS", 2)]
        public void ReturnAViewResult_WhenStartTestPhaseIsCalled(int phaseNumber, string expectedPhaseTitle, string expectedPhaseInfos, int expectedPhaseNumber)
        {
            // Arrange
            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.SetString(It.IsAny<string>(), It.IsAny<string>()));

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.StartTestPhase(phaseNumber);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Equal(expectedPhaseTitle, viewResult.ViewData["PhaseTitle"]);
            Assert.Equal(expectedPhaseInfos, viewResult.ViewData["PhaseInfos"]);
            Assert.Equal(expectedPhaseNumber, viewResult.ViewData["PhaseNumber"]);

            mockSessionStorage.Verify(x => x.SetString(StroopTestController.SESSION_PHASE_NUMBER_KEY, It.IsAny<string>()), Times.Once);
            mockSessionStorage.Verify(x => x.SetString(StroopTestController.SESSION_STEP_NUMBER_KEY, It.IsAny<string>()), Times.Once);
        }

        [Theory]
        //[InlineData(1, 0, 1, 1)]
        //[InlineData(1, 1, 1, 2)]
        [InlineData(1, 19, 1, 20)]
        //[InlineData(2, 0, 2, 1)]
        //[InlineData(2, 1, 2, 2)]
        //[InlineData(2, 19, 2, 20)]
        [Trait("Category", "Controllers")]
        public void ReturnAViewResult_WhenNextStepIsCalled(int phaseNumber, int stepNumber, int expectedPhaseNumber, int expectedStepNumber)
        {
            // Arrange
            var expectedColorModel = new ColorsModel()
            {
                ColorAsHex = "#FF0000",
                ColorAsWord = Color.BLUE
            };
            var expectedModels = TestDatas.GetFakeTestPhaseModels();

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()))
                .Returns(expectedModels);
            mockSessionStorage
                .Setup(x => x.GetString(StroopTestController.SESSION_PHASE_NUMBER_KEY))
                .Returns(phaseNumber.ToString());

            mockSessionStorage
                .Setup(x => x.GetString(StroopTestController.SESSION_STEP_NUMBER_KEY))
                .Returns(stepNumber.ToString());

            var mockColorProvider = new Mock<IColorProvider>();

            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.NextStep();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsType<StepModel>(viewResult.ViewData.Model);
            Assert.Equal(expectedPhaseNumber, model.PhaseNumber);
            Assert.Equal(expectedStepNumber, model.StepNumber);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void RedirectToStartTestPhase_WhenNextStepIsCalled()
        {
            // Arrange
            int phaseNumber = 1;
            int stepNumber = 20;
            string expectedRedirectAction = "StartTestPhase";
            int expectedPhaseNumberRouteValue = 2;

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetString(StroopTestController.SESSION_PHASE_NUMBER_KEY))
                .Returns(phaseNumber.ToString());
            mockSessionStorage
                .Setup(x => x.GetString(StroopTestController.SESSION_STEP_NUMBER_KEY))
                .Returns(stepNumber.ToString());

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.NextStep();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(expectedRedirectAction, redirectResult.ActionName);
            Assert.Equal(expectedPhaseNumberRouteValue, redirectResult.RouteValues["PhaseNumber"]);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void RedirectToFinish_WhenNextStepIsCalled()
        {
            // Arrange
            int phaseNumber = 2;
            int stepNumber = 20;
            string expectedRedirectAction = "Finish";

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetString(StroopTestController.SESSION_PHASE_NUMBER_KEY))
                .Returns(phaseNumber.ToString());
            mockSessionStorage
                .Setup(x => x.GetString(StroopTestController.SESSION_STEP_NUMBER_KEY))
                .Returns(stepNumber.ToString());

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.NextStep();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(expectedRedirectAction, redirectResult.ActionName);
        }

        [Fact]
        [Trait("Category", "Controllers")]
        public void UpdateModelInSession_ThenRedirectToNextStep_WhenGoToNextStepIsCalled()
        {
            // Arrange
            var postedModel = new StepModel()
            {
                PhaseNumber = 1,
                StepNumber = 1,
                ElapsedTime = 1000,
                SameColor = true,                
            };

            var expectedModel = TestDatas.GetFakeTestPhaseModels();

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()))
                .Returns(expectedModel);
            mockSessionStorage
                .Setup(x => x.SetObjectAsJson(It.IsAny<string>(), It.IsAny<List<TestPhaseModel>>()));
            mockSessionStorage
                .Setup(x => x.SetString(It.IsAny<string>(), It.IsAny<string>()));

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.GoToNextStep(postedModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("NextStep", redirectResult.ActionName);

            mockSessionStorage.Verify(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()), Times.Once);
            mockSessionStorage.Verify(x => x.SetObjectAsJson(It.IsAny<string>(), It.IsAny<List<TestPhaseModel>>()), Times.Once);
            mockSessionStorage.Verify(x => x.SetString(StroopTestController.SESSION_STEP_NUMBER_KEY, It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        [Trait("Category", "Controllers")]
        public void ReturnAViewResult_WhenFinishIsCalled()
        {
            // Arrange
            var expectedModels = TestDatas.GetFakeTestPhaseModels();
            var expectedCongruentsWords = expectedModels
                .Where(x => 1.Equals(x.PhaseNumber))
                .SelectMany(x => x.StepModels)
                .ToList();
            var expectedIncongruentsWords = expectedModels
                .Where(x => 2.Equals(x.PhaseNumber))
                .SelectMany(x => x.StepModels)
                .ToList();

            var mockSessionStorage = new Mock<ISessionStorage>();
            mockSessionStorage
                .Setup(x => x.GetObjectFromJson<List<TestPhaseModel>>(It.IsAny<string>()))
                .Returns(expectedModels);

            var mockColorProvider = new Mock<IColorProvider>();
            var settings = new StroopTestSettings();

            var controller = new StroopTestController(mockSessionStorage.Object, mockColorProvider.Object, settings);

            // Act
            var actual = controller.Finish();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.IsType<ResultsModel>(viewResult.ViewData.Model);

            var models = viewResult.ViewData.Model as ResultsModel;
            Assert.Equal(expectedCongruentsWords.Count(), models.CongruentWords.Results.Count);
            Assert.Equal(expectedIncongruentsWords.Count(), models.IncongruentWords.Results.Count);

        }

    }
}
