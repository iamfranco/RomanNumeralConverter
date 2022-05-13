using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanNumeralConverter.Models
{
    public static class RomanConverter
    {
        private static readonly int[] units = new int[] { 1000, 100, 10, 1 };
        private static readonly string symbolOnes = "IXCM";
        private static readonly string symbolFives = "VLD";

        public static bool HasInvalidRomanCharacter(string roman) => roman.Any(r => !string.Concat(symbolFives, symbolOnes).Contains(r));

        public static int ConvertToInteger(string roman)
        {
            if (HasInvalidRomanCharacter(roman))
                throw new ArgumentException("Input is not valid Roman Numeral");

            string[] romanSplitted = SplitRomanStringIntoArrayByUnit(roman);
            int sum = romanSplitted[0].Length * units[0];

            for (int i = 0; i < 3; i++)
            {
                string romanDigit = romanSplitted[i + 1];
                string symbolOneFiveTen = GetSymbolOneFiveTen(2 - i);

                int integerDigit = ConvertOneDigitToInteger(romanDigit, symbolOneFiveTen);
                sum += integerDigit * units[i + 1];
            }

            return sum;
        }

        public static string ConvertToRoman(int number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), "Input cannot be below zero.");

            string romanNumeral = "";

            for (int i = 0; i < 3; i++)
            {
                int remainder = number % 10;
                number /= 10;

                string symbolOneFiveTen = GetSymbolOneFiveTen(i);

                string romanNumeralToPrepend = ConvertOneDigitToRoman(remainder, symbolOneFiveTen);
                romanNumeral = romanNumeralToPrepend + romanNumeral;
            }

            string MToPrepend = new(symbolOnes.Last(), number);
            romanNumeral = MToPrepend + romanNumeral;

            return romanNumeral;
        }

        private static string GetSymbolOneFiveTen(int i) => string.Concat(symbolOnes[i], symbolFives[i], symbolOnes[i + 1]);

        private static (char, char, char) SplitSymbolOneFiveTenToChar(string symbolOneFiveTen) => (symbolOneFiveTen[0], symbolOneFiveTen[1], symbolOneFiveTen[2]);

        private static string[] SplitRomanStringIntoArrayByUnit(string roman)
        {
            string[] romanSplitted = new string[4];

            if (roman.StartsWith(symbolOnes.Last()))
            {
                int firstNonThousandIndex = roman.ToList().FindIndex(r => r != symbolOnes.Last());
                if (firstNonThousandIndex != -1)
                {
                    romanSplitted[0] = roman[..firstNonThousandIndex];
                    roman = roman[firstNonThousandIndex..];
                }
                else
                {
                    romanSplitted = new string[] { roman, "", "", "" };
                    return romanSplitted;
                }
            }
            else
            {
                romanSplitted[0] = "";
            }


            for (int i = 1; i <= 3; i++)
            {
                string symbolOneFiveTen = GetSymbolOneFiveTen(3 - i);
                string[] romanNineToOne = GetRomanZeroToNineDigits(symbolOneFiveTen).Reverse().SkipLast(0).ToArray();

                romanSplitted[i] = romanNineToOne.FirstOrDefault(r => roman.StartsWith(r), "");
                roman = roman[romanSplitted[i].Length..];
            }

            if (roman.Any())
                throw new ArgumentException("Input is not valid Roman Numeral");

            return romanSplitted;
        }

        private static string ConvertOneDigitToRoman(int number, string symbolOneFiveTen)
        {
            (char symbolOne, char symbolFive, char symbolTen) = SplitSymbolOneFiveTenToChar(symbolOneFiveTen);

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

        private static string[] GetRomanZeroToNineDigits(string symbolOneFiveTen)
        {
            (char symbolOne, char symbolFive, char symbolTen) = SplitSymbolOneFiveTenToChar(symbolOneFiveTen);

            return new string[]
            {
                "",
                new string(symbolOne, 1),
                new string(symbolOne, 2),
                new string(symbolOne, 3),
                string.Concat(symbolOne, symbolFive),
                new string(symbolFive, 1),
                string.Concat(symbolFive, new string(symbolOne, 1)),
                string.Concat(symbolFive, new string(symbolOne, 2)),
                string.Concat(symbolFive, new string(symbolOne, 3)),
                string.Concat(symbolOne, symbolTen)
            };
        }

        private static int ConvertOneDigitToInteger(string romanDigit, string symbolOneFiveTen)
        {
            string[] romanZeroToNineDigits = GetRomanZeroToNineDigits(symbolOneFiveTen);
            return Array.IndexOf(romanZeroToNineDigits, romanDigit);
        }
    }
}
