using StroopTest.Models;
using System.Collections.Generic;

namespace StroopTest.Tests
{
    public class TestDatas
    {
        public static List<TestPhaseModel> GetFakeTestPhaseModels()
        {
            var testPhaseModel1 = new TestPhaseModel()
            {
                PhaseNumber = 1,
                StepModels = new List<StepModel>()
            };

            var testPhaseModel2 = new TestPhaseModel()
            {
                PhaseNumber = 2,
                StepModels = new List<StepModel>()
            };

            // populate steps on phase 1
            for (var i = 1; i <= 20; i++)
            {
                var stepModel = GetStepModel(testPhaseModel1, i, Color.BLUE.HexValue, Color.BLUE);
                testPhaseModel1.StepModels.Add(stepModel);
            }

            // populate steps on phase 2
            for (var i = 1; i <= 20; i++)
            {
                var stepModel = GetStepModel(testPhaseModel2, i, Color.BLUE.HexValue, Color.RED);
                testPhaseModel2.StepModels.Add(stepModel);
            }

            return new List<TestPhaseModel>(){
                testPhaseModel1,
                testPhaseModel2
            };
        }

        private static StepModel GetStepModel(TestPhaseModel testPhaseModel, int stepNumber, string colorHex, Color colorAsWord)
        {
            return new StepModel()
            {
                PhaseNumber = testPhaseModel.PhaseNumber,
                StepNumber = stepNumber,
                Colors = new ColorsModel()
                {
                    ColorAsHex = colorHex,
                    ColorAsWord = colorAsWord
                }
            };
        }
    }
}
