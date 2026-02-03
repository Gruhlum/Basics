using System;
using System.Text;

public readonly struct RomanNumeral
{
    private static readonly (int Value, string Symbol)[] _map = new[]
    {
        (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
        (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
        (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")
    };

    public int Value { get; }

    public RomanNumeral(int value)
    {
        if (value <= 0 || value > 3999)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 1 and 3999.");
        Value = value;
    }

    public override string ToString()
    {
        int remaining = Value;
        var sb = new StringBuilder();

        foreach (var (val, symbol) in _map)
        {
            while (remaining >= val)
            {
                sb.Append(symbol);
                remaining -= val;
            }
        }

        return sb.ToString();
    }

    public static RomanNumeral Parse(string roman)
    {
        if (string.IsNullOrWhiteSpace(roman))
            throw new ArgumentException("Input cannot be null or empty.", nameof(roman));

        roman = roman.ToUpperInvariant();
        int i = 0, result = 0;

        while (i < roman.Length)
        {
            bool matched = false;

            foreach (var (val, symbol) in _map)
            {
                if (roman.Substring(i).StartsWith(symbol))
                {
                    result += val;
                    i += symbol.Length;
                    matched = true;
                    break;
                }
            }

            if (!matched)
                throw new FormatException($"Invalid Roman numeral at position {i}: '{roman[i]}'");
        }

        return new RomanNumeral(result);
    }

    public static implicit operator RomanNumeral(int value) => new RomanNumeral(value);
    public static implicit operator int(RomanNumeral numeral) => numeral.Value;
}
