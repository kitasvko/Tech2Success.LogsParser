using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tech2Success.LogsParser.Export;
using Tech2Success.LogsParser.Models;

namespace Tech2Success.LogsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var logPath = Directory.GetCurrentDirectory() + "/logs";
            var logs = new List<OutputLog>();

            foreach (var file in Directory.GetFiles(logPath))
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var logDate = fileName.Substring(fileName.Length - 10, 10);
                var parser = new LogParser($"{logPath}/DmsBusinessTasks_general-{logDate}.log");
                logs.AddRange(parser.Parse());
                WriteLogs(logs);
            }

            var exporter = new ExcelExporter($"{logPath}/result.csv");
            exporter.ExportToExcel(logs);
        }
        private static void WriteLogs(IEnumerable<OutputLog> logs)
        {
            foreach (var log in logs)
            {
                Console.WriteLine(log);
            }
            Console.WriteLine("");
        }
    }
}
