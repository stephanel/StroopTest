using StroopTest.Interfaces;
using System.Drawing;

namespace StroopTest.Services
{
    public class ColorRepository : IColorRepository
    {
        public Color[] GetInksColors()
        {
            return new Color[]
            {
                Color.Red,
                Color.Blue,
                Color.Yellow,
                Color.Green,
                Color.Black
            };
        }

        public Color[] GetWordsColors()
        {
            return new Color[]
            {
                Color.Red,
                Color.Blue,
                Color.Yellow,
                Color.Green
            };
        }
    }
}