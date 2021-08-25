using System;
using System.IO;
using System.Collections.Generic;
using Tech2Success.LogsParser.Export;
using Tech2Success.LogsParser.Models;
using Tech2Success.LogsParser.Parsers;

namespace Tech2Success.LogsParser.Managers
{
    public class LogParserManager
    {
        private readonly string _filesPath;
        private readonly ExcelExporter _excelExporter;

        public LogParserManager(string filesPath, string saveToPath)
        {
            _filesPath = filesPath;
            _excelExporter = new ExcelExporter(saveToPath);
        }

        public void Start()
        {
            var logs = new List<OutputLog>();
            var files = Directory.GetFiles(_filesPath, "*.log");

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(files[i]));
                Console.WriteLine($"Текущий {i}, из {files.Length}");

                var parsedLogs = Parse(files[i]);
                WriteLogs(parsedLogs);
                logs.AddRange(parsedLogs);

                Console.WriteLine($"Count logs {parsedLogs.Count}");
            }
            _excelExporter.ExportToExcel(logs);
        }

        private List<OutputLog> Parse(string file)
        {
            var logDate = GetFileDate(file);
            var parser = new LogParser(Path.Combine(_filesPath, $"DmsBusinessTasks_general-{logDate}.log"));
            return parser.Parse();
        }

        private string GetFileDate(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            return fileName.Substring(fileName.Length - 10, 10);
        }

        private void WriteLogs(List<OutputLog> logs)
        {
            foreach (var log in logs)
            {
                Console.WriteLine(log);
            }
        }
    }
}
