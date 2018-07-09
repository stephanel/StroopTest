using System;

namespace StroopTest.Models
{
    public class StepModel
    {
        public int StepNumber { get; set; }

        public bool SameColor { get; set; }
        
        public long ElapsedTime { get; set; }

        public decimal TimeInSeconds 
        {
            get
            {
                return Math.Round((decimal)ElapsedTime / 1000, 2);
            }
        }


        public ColorsModel Colors { get; set; }
    }
}