using StroopTest.Interfaces;
using System.Drawing;

namespace StroopTest.Services
{
    public class ColorRepository : IColorRepository
    {
        public Color[] GetColors()
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