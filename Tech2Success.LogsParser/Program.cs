using System.IO;
using Tech2Success.LogsParser.Managers;

namespace Tech2Success.LogsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var logsPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            var saveToPath = Path.Combine(logsPath, $"result.csv");

            var manager = new LogParserManager(logsPath, saveToPath);
            manager.Start();
        }
    }
}
