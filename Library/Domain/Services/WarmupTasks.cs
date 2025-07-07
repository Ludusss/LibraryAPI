using System.Text;

namespace Domain.Services;

public static class WarmupTasks
{
    public static bool IsBookIdPowerOfTwo(int bookId)
    {
        if (bookId <= 0)
            return false;

        return (bookId & (bookId - 1)) == 0;
    }

    public static string ReverseBookTitle(string bookTitle)
    {
        if (string.IsNullOrEmpty(bookTitle))
            return bookTitle;

        var chars = bookTitle.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    public static string GenerateBookTitleReplicas(string bookTitle, int replicaCount)
    {
        if (string.IsNullOrEmpty(bookTitle))
            return string.Empty;

        if (replicaCount <= 0)
            return string.Empty;

        var sb = new StringBuilder();
        for (int i = 0; i < replicaCount; i++)
        {
            sb.Append(bookTitle);
        }

        return sb.ToString();
    }

    public static List<int> GetOddNumberedBookIds()
    {
        var oddIds = new List<int>();

        for (int i = 1; i <= 100; i += 2)
        {
            oddIds.Add(i);
        }

        return oddIds;
    }

    public static void PrintOddNumberedBookIds()
    {
        Console.WriteLine("Odd-numbered Book IDs between 0 and 100:");
        for (int i = 1; i <= 100; i += 2)
        {
            Console.WriteLine($"Book ID: {i}");
        }
    }
}
