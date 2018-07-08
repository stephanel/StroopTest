using StroopTest.Interfaces;
using System;

namespace StroopTest.Services
{
    public class RandomGenerator : IRandomGenerator
    {
        private Random _random;
        public RandomGenerator()
        {
            _random = new Random();
        }

        public int GetRandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}