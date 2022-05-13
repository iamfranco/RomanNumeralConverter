using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanNumeralConverter.Models
{
    public static class RomanConverter
    {
        private static readonly int[] _units = new int[] { 1, 10, 100, 1000 };
        private static readonly int _unitFactor = 10;
        private static readonly string _symbolOnes = "IXCM";
        private static readonly string _symbolFives = "VLD";

        public static bool HasInvalidRomanCharacter(string roman) =>
            roman.Any(r => !string.Concat(_symbolFives, _symbolOnes).Contains(r));

        public static int ConvertToInteger(string roman)
        {
            if (HasInvalidRomanCharacter(roman))
                throw new ArgumentException("Input is not valid Roman Numeral");

            // separate "MMDCCCXCIV" into {"IV", "XC", "DCCC", "MM"}
            string[] romanSubstringsByUnits = SeparateRomanStringIntoFourPartsByUnits(roman);

            int sum = 0;
            for (int tenToThePowerOf = 0; tenToThePowerOf <= 3; tenToThePowerOf++)
            {
                string romanSubstring = romanSubstringsByUnits[tenToThePowerOf];

                int integerDigit = ConvertRomanSubstringToInteger(romanSubstring, tenToThePowerOf);
                sum += integerDigit * _units[tenToThePowerOf];
            }

            return sum;
        }

        public static string ConvertToRoman(int number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), "Input cannot be below zero.");

            string romanString = "";

            for (int tenToThePowerOf = 0; tenToThePowerOf < 3; tenToThePowerOf++)
            {
                int remainder = number % _unitFactor;
                number /= _unitFactor;

                string romanSubstringToPrepend = ConvertOneDigitToRomanSubstring(remainder, tenToThePowerOf);
                romanString = romanSubstringToPrepend + romanString;
            }

            // prepend M's for 1000's place
            string MToPrepend = new(_symbolOnes.Last(), number);
            romanString = MToPrepend + romanString;

            return romanString;
        }

        private static string GetSymbolOneFiveTenAsString(int tenToThePowerOf)
        {
            if (tenToThePowerOf == 3)
                return "";

            return string.Concat(
                _symbolOnes[tenToThePowerOf],
                _symbolFives[tenToThePowerOf],
                _symbolOnes[tenToThePowerOf + 1]);
        }

        private static (char, char, char) GetSymbolOneFiveTenAsCharTuple(int tenToThePowerOf)
        {
            string symbolOneFiveTen = GetSymbolOneFiveTenAsString(tenToThePowerOf);
            return (symbolOneFiveTen[0], symbolOneFiveTen[1], symbolOneFiveTen[2]);
        }

        private static string[] SeparateRomanStringIntoFourPartsByUnits(string roman)
        {
            string[] romanArray = Enumerable.Repeat("", 4).ToArray();

            string romanSubstringForThousands = GetRomanSubstringAtThousandsPlace(roman);
            romanArray[3] = romanSubstringForThousands;
            roman = roman[romanSubstringForThousands.Length..];

            for (int tenTothePowerOf = 2; tenTothePowerOf >= 0; tenTothePowerOf--)
            {
                string currentRomanSubstring = GetRomanSubstringAtTenToThePowerOf(roman, tenTothePowerOf);
                romanArray[tenTothePowerOf] = currentRomanSubstring;
                roman = roman[currentRomanSubstring.Length..];
            }

            if (roman.Any())
                throw new ArgumentException("Input is not valid Roman Numeral");

            return romanArray;
        }

        private static string GetRomanSubstringAtThousandsPlace(string roman)
        {
            if (!roman.StartsWith(_symbolOnes.Last()))
                return "";

            int firstIndexForNotM = roman.ToList().FindIndex(r => r != _symbolOnes.Last());

            if (firstIndexForNotM == -1)
                firstIndexForNotM = roman.Length;

            return roman[..firstIndexForNotM];
        }

        private static string GetRomanSubstringAtTenToThePowerOf(string roman, int tenTothePowerOf)
        {
            string[] romanNineToOne = GetRomanZeroToNine(tenTothePowerOf).Reverse().SkipLast(0).ToArray();

            return romanNineToOne.FirstOrDefault(r => roman.StartsWith(r), "");
        }

        private static string ConvertOneDigitToRomanSubstring(int number, int tenToThePowerOf)
        {
            (char symbolOne, char symbolFive, char symbolTen) = GetSymbolOneFiveTenAsCharTuple(tenToThePowerOf);

            return number switch
            {
                0 => "",
                < 4 => new string(symbolOne, number),
                4 => string.Concat(symbolOne, symbolFive),
                < 9 => string.Concat(symbolFive, new string(symbolOne, number - 5)),
                9 => string.Concat(symbolOne, symbolTen),
                _ => ""
            };
        }

        private static int ConvertRomanSubstringToInteger(string romanSubstring, int tenToThePowerOf)
        {
            bool substringOnlyHasMs = romanSubstring.All(r => r.ToString() == _symbolOnes.Last().ToString());
            if (substringOnlyHasMs)
                return romanSubstring.Length;

            string[] romanZeroToNine = GetRomanZeroToNine(tenToThePowerOf);
            return Array.IndexOf(romanZeroToNine, romanSubstring);
        }

        private static string[] GetRomanZeroToNine(int tenToThePowerOf)
        {
            (char symbolOne, char symbolFive, char symbolTen) = GetSymbolOneFiveTenAsCharTuple(tenToThePowerOf);

            return new string[]
            {
                "",                                                     // 0
                new string(symbolOne, 1),                               // 1
                new string(symbolOne, 2),                               // 2
                new string(symbolOne, 3),                               // 3
                string.Concat(symbolOne, symbolFive),                   // 4
                new string(symbolFive, 1),                              // 5
                string.Concat(symbolFive, new string(symbolOne, 1)),    // 6
                string.Concat(symbolFive, new string(symbolOne, 2)),    // 7
                string.Concat(symbolFive, new string(symbolOne, 3)),    // 8
                string.Concat(symbolOne, symbolTen)                     // 9
            };
        }
    }
}
