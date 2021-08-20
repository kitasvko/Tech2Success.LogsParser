﻿using System;
using System.Collections.Generic;
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
                if (Regex.IsMatch(_logs[i], Pattern))
                    processedLogs.Add(ProcessLog(_logs[i]));

            foreach (var logsGroup in processedLogs.GroupBy(x => x.ThreadId))
            {
                var firstLog = logsGroup.FirstOrDefault(x => x.Status == "started.");
                foreach (var log in logsGroup)
                {
                    if(log.Status == "stopped.")
                    {
                        firstLog.Duration = log.Time - firstLog.Time;
                        parsedLogs.Add(firstLog);
                    }
                    if(log.Status == "started.")
                    {
                        firstLog = log;
                    }
                }
            }
            return parsedLogs.OrderBy(x => x.Time).ToList();
        }
        private Log ProcessLog(string line)
        {
            var splittedLog = line.Split(' ');
            var log = new Log();

            if (DateTime.TryParse(splittedLog[0], out DateTime date))
                log.Date = date.Date;
            if (TimeSpan.TryParse(splittedLog[1], out TimeSpan time))
                log.Time = time;

            log.ThreadId = int.Parse(splittedLog[2].Trim(new char[] { '[', ']' }));
            log.Status = splittedLog[splittedLog.Length - 1];
            return log;
        }
    }
}