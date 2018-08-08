using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StroopTest.Models
{
    public class TestPhaseModel
    {
        public int PhaseNumber { get; set; }
        public List<StepModel> StepModels { get; set; }
    }
}
