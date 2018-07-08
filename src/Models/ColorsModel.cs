using StroopTest.Extensions;
using System.Drawing;

namespace StroopTest.Models
{
    public class ColorsModel
    {
        public Color ColorAsWord {get; set; }
        public string ColorAsHex {get; set; }

        public bool AreTheSame {
            get {
                return ColorAsHex != null && ColorAsHex.Equals(ColorAsWord.ToHex());
            }
        }
    }
}