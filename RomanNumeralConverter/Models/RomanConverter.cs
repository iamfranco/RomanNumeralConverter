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

        public static bool HasInvalidRomanCharacter(string roman)
        {
            string validRomans = symbolFives + symbolOnes;
            return roman.Any(r => !validRomans.Contains(r));
        }

        public static int ConvertToInteger(string roman)
        {
            if (HasInvalidRomanCharacter(roman))
                return -1;

            string[] romanSplitted = SplitRomanStringIntoArrayByUnit(roman);
            int sum = romanSplitted[0].Length * units[0];

            for (int i = 0; i < 3; i++)
            {
                string romanDigit = romanSplitted[i + 1];

                char symbolOne = symbolOnes[2 - i];
                char symbolFive = symbolFives[2 - i];
                char symbolTen = symbolOnes[3 - i];

                sum += ConvertOneDigitToInteger(romanDigit, symbolOne, symbolFive, symbolTen) * units[i + 1];
            }

            return sum;
        }

        public static string[] SplitRomanStringIntoArrayByUnit(string roman)
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
                char symbolOne = symbolOnes[3 - i];
                char symbolFive = symbolFives[3 - i];
                char symbolTen = symbolOnes[4 - i];
                string[] romanNineToOne = GetRomanZeroToNineDigit(symbolOne, symbolFive, symbolTen).Reverse().SkipLast(0).ToArray();

                romanSplitted[i] = romanNineToOne.FirstOrDefault(r => roman.StartsWith(r), "");
                roman = roman[romanSplitted[i].Length..];
            }

            if (roman.Any())
                throw new ArgumentException("Input is not valid Roman Numeral");

            return romanSplitted;
        }

        public static string ConvertToRoman(int number)
        {
            if (number < 0)
                return "";

            string romanNumeral = "";

            for (int i = 0; i < 3; i++)
            {
                char symbolOne = symbolOnes[i];
                char symbolFive = symbolFives[i];
                char symbolTen = symbolOnes[i + 1];

                int remainder = number % 10;
                number /= 10;

                string romanNumeralToPrepend = ConvertOneDigitToRoman(remainder, symbolOne, symbolFive, symbolTen);
                romanNumeral = romanNumeralToPrepend + romanNumeral;
            }

            string MToPrepend = new string(symbolOnes.Last(), number);
            romanNumeral = MToPrepend + romanNumeral;

            return romanNumeral;
        }

        private static string ConvertOneDigitToRoman(int number, char symbolOne, char symbolFive, char symbolTen)
        {
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

        private static string[] GetRomanZeroToNineDigit(char symbolOne, char symbolFive, char symbolTen)
        {
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

        private static int ConvertOneDigitToInteger(string romanDigit, char symbolOne, char symbolFive, char symbolTen)
        {
            string[] zeroToNineRomanDigit = GetRomanZeroToNineDigit(symbolOne, symbolFive, symbolTen);
            return Array.IndexOf(zeroToNineRomanDigit, romanDigit);
        }
    }
}
