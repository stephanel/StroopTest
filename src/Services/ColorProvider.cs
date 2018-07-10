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
            Color[] wordsColors = _colorRepository.GetWordsColors();
            Color[] inksColors = _colorRepository.GetInksColors();

            int wordIndex = _randomGenerator.GetRandomInt(0, wordsColors.Length);
            int inkIndex = _randomGenerator.GetRandomInt(0, inksColors.Length);
            
            return new ColorsModel()
            {
                ColorAsWord = wordsColors[wordIndex],
                ColorAsHex = inksColors[inkIndex].ToHex()
            };
        }

    }
}