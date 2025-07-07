using System.Text.RegularExpressions;
using Domain.Common;

namespace Domain.ValueObjects;

public class Isbn : ValueObject
{
    public string Value { get; }

    private Isbn(string value)
    {
        Value = value;
    }

    public static Isbn Create(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new ArgumentException("Invalid ISBN format.", nameof(isbn));
        }

        var cleanIsbn = isbn.Replace("-", "").Replace(" ", "");

        if (!IsValidIsbn(cleanIsbn))
        {
            throw new ArgumentException("Invalid ISBN format.", nameof(isbn));
        }

        // Store original format to preserve user input
        return new Isbn(isbn);
    }

    private static bool IsValidIsbn(string isbn)
    {
        return IsValidIsbn10(isbn) || IsValidIsbn13(isbn);
    }

    private static bool IsValidIsbn10(string isbn)
    {
        if (isbn.Length != 10)
        {
            return false;
        }

        if (!Regex.IsMatch(isbn, @"^[0-9]{9}[0-9X]$"))
        {
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(isbn[i].ToString()) * (10 - i);
        }

        char checkDigit = isbn[9];
        int checkValue = checkDigit == 'X' ? 10 : int.Parse(checkDigit.ToString());

        return (sum + checkValue) % 11 == 0;
    }

    private static bool IsValidIsbn13(string isbn)
    {
        if (isbn.Length != 13)
        {
            return false;
        }

        if (!Regex.IsMatch(isbn, @"^[0-9]{13}$"))
        {
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = int.Parse(isbn[i].ToString());
            sum += digit * (i % 2 == 0 ? 1 : 3);
        }

        int checkDigit = int.Parse(isbn[12].ToString());
        int calculatedCheckDigit = (10 - (sum % 10)) % 10;

        return checkDigit == calculatedCheckDigit;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Compare cleaned versions for equality
        yield return Value.Replace("-", "").Replace(" ", "");
    }

    public override string ToString() => Value;
}
