using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanNumeralConverter.Models
{
    public static class RomanConverter
    {
        private static readonly List<RomanUnit> _romanUnits = new()
        {
            new("M", 1000),
            new("CM", 900),
            new("D", 500),
            new("CD", 400),
            new("C", 100),
            new("XC", 90),
            new("L", 50),
            new("XL", 40),
            new("X", 10),
            new("IX", 9),
            new("V", 5),
            new("IV", 4),
            new("I", 1)
        };

        private class RomanUnit
        {
            public string Roman { get; }
            public int Unit { get; }
            public RomanUnit(string roman, int unit)
            {
                Roman = roman;
                Unit = unit;
            }
        }

        public static int ConvertToInteger(string roman)
        {
            string romanCopied = roman;

            int sum = 0;
            while (roman.Length > 0)
            {
                RomanUnit romanUnit = GetRomanUnitMatchingStartOfString(roman);

                sum += romanUnit.Unit;
                roman = roman[romanUnit.Roman.Length..];
            }

            if (ConvertToRoman(sum) != romanCopied)
                ThrowInvalidRomanNumeralException();

            return sum;
        }

        public static string ConvertToRoman(int number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), "Input cannot be below zero.");

            string romanString = "";
            while (number > 0)
            {
                RomanUnit romanUnit = GetRomanUnitLessThanOrEqual(number);

                romanString += romanUnit.Roman;
                number -= romanUnit.Unit;
            }

            return romanString;
        }

        private static RomanUnit GetRomanUnitMatchingStartOfString(string roman)
        {
            int romanUnitsMatchIndex = _romanUnits.FindIndex(romanUnit => roman.StartsWith(romanUnit.Roman));
            if (romanUnitsMatchIndex == -1)
                ThrowInvalidRomanNumeralException();

            return _romanUnits[romanUnitsMatchIndex];
        }

        private static RomanUnit GetRomanUnitLessThanOrEqual(int number) => _romanUnits.First(romanUnit => number >= romanUnit.Unit);

        private static void ThrowInvalidRomanNumeralException()
        {
            throw new ArgumentException("Input is not valid Roman Numeral");
        }
    }
}
