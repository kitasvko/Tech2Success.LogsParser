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
            var logDate = "2021-07-21";
            var parser = new LogParser($"DmsBusinessTasks_general-{logDate}.log");
            var logs = parser.Parse();
            WriteLogs(logs);
            var exporter = new ExcelExporter($"logs_{logDate}.csv");
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
