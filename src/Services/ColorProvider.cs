using StroopTest.Interfaces;
using StroopTest.Extensions;
using StroopTest.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace StroopTest.Services
{    public class ColorProvider : IColorProvider
    {
        private readonly IColorRepository _colorRepository;
        private readonly IRandomGenerator _randomGenerator;

        public ColorProvider(IColorRepository colorRepository, IRandomGenerator randomGenerator)
        {
            _colorRepository = colorRepository;
            _randomGenerator = randomGenerator;
        }
        
        public ColorsModel GetRandomColor()
        {
            Color[] colors = _colorRepository.GetColors();

            int indexName = _randomGenerator.GetRandomInt(0, colors.Length);
            int indexValue = _randomGenerator.GetRandomInt(0, colors.Length);
            
            return new ColorsModel()
            {
                ColorAsWord = colors[indexName],
                ColorAsHex = colors[indexValue].ToHex()
            };
        }

    }
}