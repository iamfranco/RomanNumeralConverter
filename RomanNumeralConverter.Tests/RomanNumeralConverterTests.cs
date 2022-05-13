using FluentAssertions;
using NUnit.Framework;
using RomanNumeralConverter.Models;

namespace RomanNumeralConverter.Tests
{
    public class Tests
    {
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertToInteger_One_To_Ten_Should_Return_Correct_Result()
        {
            string[] romanOneToTen = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            for (int i=0; i<10; i++)
            {
                int expectedResult = i + 1;
                RomanConverter.ConvertToInteger(romanOneToTen[i]).Should().Be(expectedResult);
            }
        }

        [Test]
        public void ConvertToRoman_And_ConvertToInteger_Should_Be_Inverse_Of_Each_Other_From_One_To_Ten_Thousand()
        {
            for (int i=1; i<=10000; i++)
            {
                string roman = RomanConverter.ConvertToRoman(i);
                RomanConverter.ConvertToInteger(roman).Should().Be(i);
            }
        }
    }
}