using StroopTest.Models;

namespace StroopTest.Interfaces
{
    public interface IColorProvider
    {
        ColorsModel GetCongruentColor();
        ColorsModel GetIncongruentColor();
        ColorsModel GetNeutralColor();
    }
}