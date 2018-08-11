using StroopTest.Interfaces;
using StroopTest.Models;

namespace StroopTest.Services
{
    public class ColorRepository : IColorRepository
    {
        public Color[] GetColors()
        {
            return new Color[]
            {
                Color.RED,
                Color.BLUE,
                Color.YELLOW,
                Color.GREEN
            };
        }
    }
}