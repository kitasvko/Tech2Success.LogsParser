using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tech2Success.LogsParser.Models;

namespace Tech2Success.LogsParser
{
    public class LogParser
    {
        private const string Pattern = @"^(\d{4}-\d{2}-\d{2})\s(\d{2}:\d{2}:\d{2},\d{3})\s(\[(\d|\d\d)\])\s(INFO)\s\s(\D*\.$)";
        private string[] _logs;
        public LogParser(string fileName)
        {
            _logs = File.ReadAllLines(fileName);
            Parse();
        }
        public List<Log> Parse()
        {
            var processedLogs = new List<Log>();
            var parsedLogs = new List<Log>();
            
            for (int i = 0; i < _logs.Length - 1; i++)
                if (Regex.IsMatch(_logs[i], Pattern) && CheckTitleTask(_logs[i]))
                    processedLogs.Add(ProcessLog(_logs[i]));

            foreach (var logsGroup in processedLogs.GroupBy(x => x.ThreadId))
            {
                var firstLog = logsGroup.FirstOrDefault(x => x.Status == "started.");
                foreach (var log in logsGroup)
                {
                    if(log.Status == "stopped.")
                    {
                        firstLog.EndDate = log.StartDate;
                        firstLog.Duration = firstLog.EndDate - firstLog.StartDate;
                        parsedLogs.Add(firstLog);
                    }
                    if(log.Status == "started.")
                    {
                        firstLog = log;
                    }
                }
            }
            return parsedLogs.OrderBy(x => x.EndDate).ToList();
        }
        private bool CheckTitleTask(string line)
        {
            var splittedLog = line.Split(' ');
            if (splittedLog[5].StartsWith("OldCIProcessingTask"))
                return true;
            return false;
        }
        private Log ProcessLog(string line)
        {
            var splittedLog = line.Split(' ');
            var log = new Log();

            _ = DateTime.TryParseExact($"{splittedLog[0]} {splittedLog[1]}", new string[] { "yyyy-MM-dd HH:mm:ss,fff" },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime date);

                log.StartDate = date;

            log.ThreadId = int.Parse(splittedLog[2].Trim(new char[] { '[', ']' }));
            log.Title = splittedLog[5];
            log.Status = splittedLog[splittedLog.Length - 1];
            return log;
        }
    }
}
