using StroopTest.Extensions;
using System;
using System.Drawing;

namespace StroopTest.Models
{
    public class StepModel
    {
        public int PhaseNumber { get; set; }

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

        public bool IsNeutral
        {
            get
            {
                return Color.Black.ToHex().Equals(Colors.ColorAsHex);
            }
        }

        public bool IsCongruent
        {
            get
            {
                if (IsNeutral)
                    return false;

                return Colors.ColorAsWord.ToHex().Equals(Colors.ColorAsHex);
            }
        }

        public bool IsIncongruent
        {
            get
            {
                if (IsNeutral)
                    return false;

                return !Colors.ColorAsWord.ToHex().Equals(Colors.ColorAsHex);
            }
        }

        public ColorsModel Colors { get; set; }

        public bool ResponseIsCorrect
        {
            get
            {
                return SameColor && Colors.ColorAsHex.Equals(Colors.ColorAsWord.ToHex());
            }
        }
    }
}