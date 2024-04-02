/// Demonstration of Kaprekar's Routine
/// Enter any 4 digit number
/// There must be at least 2 unique numbers, the input can't be 3333, but it can be 3331.
/// The final result will always be 6174
/// 
/// https://en.wikipedia.org/wiki/6174

string input = Kaprekar.GetInput();
Kaprekar.Calculate(input);

internal static class Kaprekar
{
    public static string GetInput()
    {
        string result = string.Empty;
        bool isValid = false;
        while (!isValid)
        {
            Console.Write("Enter 4 digit number: ");
            string? input = Console.ReadLine();
            if (ValidateInput(input))
            {
                isValid = true;
                result = input!;
            }
            else
            {
                PrintInvalidInput();
            }
        }
        return result;
    }

    public static void Calculate(string input)
    {
        char[] x = input.ToCharArray();
        string desc = string.Concat(x.OrderByDescending(c => c));
        string asc = string.Concat(x.OrderBy(c => c));
        int result = int.Parse(desc) - int.Parse(asc);
        Print(desc, asc, result);
        if (result != 6174m) { Calculate(result.ToString("0000")); }
        else { Console.WriteLine("done!"); }
    }

    private static bool ValidateInput(string? input)
    {
        if (input is null) return false;
        if (input.Length != 4) return false;
        if (!int.TryParse(input, out int _)) return false;
        var test = input.ToCharArray().Distinct().Count();
        if (input.ToCharArray().Distinct().Count() < 2) return false;
        return true;
    }

    private static void PrintInvalidInput()
    {
        Console.WriteLine("Invalid Input");
        Console.WriteLine("Try Again");
        Console.WriteLine("\n");
    }

    private static void Print(string desc, string asc, int result)
    {
        Console.WriteLine("  " + desc);
        Console.WriteLine("- " + asc);
        Console.WriteLine("-------");
        Console.WriteLine("  " + result);
        Console.WriteLine("\n");
    }
}
