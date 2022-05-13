using RomanNumeralConverter.Models;

bool IsValidInput = false;

while (!IsValidInput)
{
    Console.Write("Please enter a Roman Numeral: ");
    string? input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
        continue;

    if (RomanConverter.HasInvalidRomanCharacter(input))
    {
        Console.WriteLine("Input contains invalid character (valid characters are: {I, V, X, L, C, D, M})");
        continue;
    }

    int output;
    try
    {
        output = RomanConverter.ConvertToInteger(input);
    } 
    catch (ArgumentException)
    {
        Console.WriteLine("Input is not valid Roman Numeral");
        continue;
    }

    Console.WriteLine(output);
    IsValidInput = true;
}
