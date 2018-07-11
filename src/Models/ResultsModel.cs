using System.Collections.Generic;

namespace StroopTest.Models
{
    public class ResultsModel
    {
        public List<StepModel> CongruentWords { get; set; }
        public List<StepModel> IncongruentWords { get; set; }
        public List<StepModel> NeutralWords { get; set; }
    }
}
