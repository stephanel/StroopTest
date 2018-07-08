using StroopTest.Models;

namespace StroopTest.Interfaces
{
    public interface IColorProvider
    {
        ColorsModel GetRandomColor();
    }
}