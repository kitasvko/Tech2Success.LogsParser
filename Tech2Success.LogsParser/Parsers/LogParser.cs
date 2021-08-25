using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using Tech2Success.LogsParser.Models;

namespace Tech2Success.LogsParser.Parsers
{
    public class LogParser
    {
        private readonly string _path;

        public LogParser(string path)
        {
            _path = path;
        }

        public List<OutputLog> Parse()
        {
            var logs = File.ReadAllLines(_path);

            var processedLogs = ProcessLogs(logs);

            var parsedLogs = ParseLogs(processedLogs);

            return parsedLogs;
        }

        private List<InputLog> ProcessLogs(string[] lines)
        {
            var processedLogs = new List<InputLog>();

            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (IsMatched(lines[i]))
                {
                    processedLogs.Add(ProcessLog(lines[i]));
                }
            }
            return processedLogs;
        }

        private InputLog ProcessLog(string line)
        {
            var splittedLog = line.Split(' ');
            var log = new InputLog();

            _ = DateTime.TryParseExact($"{splittedLog[0]} {splittedLog[1]}", new string[] { "yyyy-MM-dd HH:mm:ss,fff" },
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

            log.Date = date;
            log.ThreadId = int.Parse(splittedLog[2].Trim(new char[] { '[', ']' }));
            log.Type = ParseLogType(splittedLog[splittedLog.Length - 1]);

            if (line.Contains("batchSize"))
            {
                var batchSize = splittedLog[16];
                log.BatchSize = int.Parse(batchSize.Remove(batchSize.Length - 1));
            }

            return log;
        }

        private List<OutputLog> ParseLogs(List<InputLog> logs)
        {
            var parsedLogs = new List<OutputLog>();

            for (var i = 0; i <= logs.Count - 1;)
            {
                var currentLog = logs[i];
                var threadId = currentLog.ThreadId;
                var log = new OutputLog();

                if (currentLog.Type == LogType.Started)
                {
                    log.StartDate = currentLog.Date;
                    log.ThreadId = threadId;
                }

                while (++i <= logs.Count - 1)
                {
                    var nextLog = logs[i];

                    if (nextLog.ThreadId != threadId)
                    {
                        break;
                    }

                    if (nextLog.Type == LogType.Batch)
                    {
                        log.BatchSize = nextLog.BatchSize;
                        continue;
                    }

                    if (nextLog.Type == LogType.Stopped)
                    {
                        log.EndDate = nextLog.Date;
                        log.Duration = log.EndDate - currentLog.Date;
                        i++;
                        break;
                    }
                }

                parsedLogs.Add(log);
            }
            return parsedLogs;
        }

        private LogType ParseLogType(string line)
        {
            if (line.StartsWith("started"))
            {
                return LogType.Started;
            }
            else if (line.StartsWith("stopped"))
            {
                return LogType.Stopped;
            }
            else return LogType.Batch;
        }

        private bool IsMatched(string line)
        {
            return line.EndsWith("OldCIProcessingTask OldCIProcessingTask started.")
                    || line.Contains("OldCIProcessingTask Start upload CI report processing for batch #1, batchSize")
                        || line.EndsWith("OldCIProcessingTask OldCIProcessingTask stopped.");
        }
    }
}
