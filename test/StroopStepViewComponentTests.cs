using Microsoft.AspNetCore.Mvc.ViewComponents;
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
    
    public class StroopStepViewComponentShould
    {
        [Fact]
        public void Return_View_Component_With_Step_Model()
        {
            // Arrange
            var viewComponent = new StroopStepViewComponent();
            var expectedModel = new StepModel()
            {
                Colors = new ColorsModel()
                {
                    ColorAsHex = "#ff0000",
                    ColorAsWord = Color.Red
                },
                StepNumber = 3
            };

            // Act
            var actual = viewComponent.Invoke(expectedModel);

            // Assert
            var model = Assert.IsType<ViewViewComponentResult>(actual).ViewData.Model as StepModel;
            Assert.Equal(expectedModel.StepNumber, model.StepNumber);
            Assert.Equal(expectedModel.Colors, model.Colors);
        }
    }
}