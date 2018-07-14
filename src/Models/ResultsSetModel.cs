using System.Collections.Generic;
using StroopTest.Extensions;
using System.Linq;

namespace StroopTest.Models
{
    public class ResultsSetModel
    {
        public List<StepModel> Results { get; set; }

        public int WrongAnswers
        {
            get
            {
                return Results.Where(x => !(x.SameColor == x.Colors.ColorAsHex.Equals(x.Colors.ColorAsWord.ToHex()))).Count();
            }
        }

        public decimal AverageTime
        {
            get
            {
                return TotalTime / Results.Count;
            }
        }

        public decimal TotalTime
        {
            get
            {
                return Results.Sum(x => x.TimeInSeconds);
            }
        }

    }
}
