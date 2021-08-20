using System;
using System.Collections.Generic;
using Tech2Success.LogsParser.Export;
using Tech2Success.LogsParser.Models;

namespace Tech2Success.LogsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new LogParser("2021-08-18.log");
            var logs = parser.Parse();
            WriteLogs(logs);
            var exporter = new ExcelExporter("logs.csv");
            exporter.ExportToExcel(logs);
        }
        private static void WriteLogs(IEnumerable<Log> logs)
        {
            foreach (var log in logs)
            {
                Console.WriteLine(log);
            }
        }
    }
}
