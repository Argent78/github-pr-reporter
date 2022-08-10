using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper;

namespace GitHub.PullRequest.ConsoleApp.Commands;

internal abstract class ConsoleCommand : ConsoleAppBase
{

    protected static void WriteToCsv<T>(
        string owner, 
        string repo, IEnumerable<T> lifetime, 
        [CallerMemberName] string? suffix = default) where T : class
    {
        var filename = $"{owner}-{repo}-{suffix?.ToLower()}.csv";
        Console.WriteLine($"Writing data to file '{filename}'");
        using var writer = new StreamWriter(filename);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(lifetime);
    }
}