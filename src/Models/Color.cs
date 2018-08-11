using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StroopTest.Models
{
    public class Color
    {
        public string HexValue { get; set; }
        public short R { get; set; }
        public short G { get; set; }
        public short B { get; set; }
        public String Name { get; set; }


        public static readonly Color BLACK = new Color()
        {
            HexValue = "#000000",
            R = 0,
            G = 0,
            B = 0,
            Name = "Black"
        };

        public static readonly Color BLUE = new Color()
        {
            HexValue = "#0000FF",
            R = 0,
            G = 0,
            B = 255,
            Name = "Blue"
        };

        public static readonly Color RED = new Color()
        {
            HexValue = "#FF0000",
            R = 255,
            G = 0,
            B = 0,
            Name = "Red"
        };

        public static readonly Color YELLOW = new Color()
        {
            HexValue = "#FFFF00",
            R = 255,
            G = 255,
            B = 0,
            Name = "Yellow"
        };

        public static readonly Color GREEN = new Color()
        {
            HexValue = "#00FF00",
            R = 0,
            G = 255,
            B = 0,
            Name = "Green"
        };

    }
}
