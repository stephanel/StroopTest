using System.Drawing;

namespace StroopTest.Interfaces
{
    public interface IColorRepository
    {
        Color[] GetWordsColors();
        Color[] GetInksColors();
    }
}