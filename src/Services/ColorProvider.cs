using StroopTest.Interfaces;
using StroopTest.Extensions;
using StroopTest.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        
        public ColorsModel GetNeutralColor()
        {
            Color[] wordsColors = _colorRepository.GetColors();

            int wordIndex = _randomGenerator.GetRandomInt(0, wordsColors.Length);

            Color randomColor = wordsColors[wordIndex];

            return new ColorsModel()
            {
                ColorAsWord = randomColor,
                ColorAsHex = Color.Black.ToHex()
            };
        }

        public ColorsModel GetCongruentColor()
        {
            Color[] wordsColors = _colorRepository.GetColors();

            int wordIndex = _randomGenerator.GetRandomInt(0, wordsColors.Length);

            Color randomColor = wordsColors[wordIndex];

            return new ColorsModel()
            {
                ColorAsWord = randomColor,
                ColorAsHex = randomColor.ToHex()
            };

        }

        public ColorsModel GetIncongruentColor()
        {
            Color[] wordsColors = _colorRepository.GetColors();
            int wordIndex = _randomGenerator.GetRandomInt(0, wordsColors.Length);

            List<Color> inksColors = wordsColors.ToList();
            inksColors.RemoveAt(wordIndex);

            int inkIndex = _randomGenerator.GetRandomInt(0, inksColors.Count);

            return new ColorsModel()
            {
                ColorAsWord = wordsColors[wordIndex],
                ColorAsHex = inksColors[inkIndex].ToHex()
            };
        }

    }
}