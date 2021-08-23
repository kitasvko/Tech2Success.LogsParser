using System;
using System.Collections.Generic;
using System.IO;
using Tech2Success.LogsParser.Models;

namespace Tech2Success.LogsParser
{
    public class LogParser
    {
        private string[] _logs;

        public LogParser(string fileName)
        {
            _logs = File.ReadAllLines(fileName);
        }

        public List<OutputLog> Parse()
        {
            var processedLogs = new List<InputLog>();
            var parsedLogs = new List<OutputLog>();

            for (int i = 0; i < _logs.Length - 1; i++)
                if (_logs[i].EndsWith("OldCIProcessingTask OldCIProcessingTask started.")
                    || _logs[i].Contains("OldCIProcessingTask Start upload CI report processing for batch #1, batchSize")
                    || _logs[i].EndsWith("OldCIProcessingTask OldCIProcessingTask stopped."))
                    processedLogs.Add(ProcessLog(_logs[i]));

            for (var i = 0; i <= processedLogs.Count - 1;)
            {
                var currentLog = processedLogs[i];
                var threadId = currentLog.ThreadId;
                var log = new OutputLog();

                if (currentLog.Status == "started.")
                {
                    log.StartDate = currentLog.Date;
                    log.ThreadId = threadId;
                }

                i++;

                if (i <= processedLogs.Count - 1)
                {
                    var nextLog = processedLogs[i];

                    if (nextLog.ThreadId != threadId)
                    {
                        parsedLogs.Add(log);
                        continue;
                    }

                    if (nextLog.Status != "stopped." && nextLog.Status != "started.")
                    {
                        log.BatchSize = nextLog.BatchSize;
                        i++;

                        if (i <= processedLogs.Count - 1)
                        {
                            var crntLog = processedLogs[i];

                            if (crntLog.Status == "stopped.")
                            {
                                log.EndDate = crntLog.Date;
                                log.Duration = log.EndDate - currentLog.Date;
                            }
                        }
                    }
                    i++;
                }

                parsedLogs.Add(log);
            }
            return parsedLogs;
        }

        private InputLog ProcessLog(string line)
        {
            var splittedLog = line.Split(' ');
            var log = new InputLog();

            _ = DateTime.TryParseExact($"{splittedLog[0]} {splittedLog[1]}", new string[] { "yyyy-MM-dd HH:mm:ss,fff" },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime date);

            log.Date = date;
            log.ThreadId = int.Parse(splittedLog[2].Trim(new char[] { '[', ']' }));
            log.Status = splittedLog[splittedLog.Length - 1];

            if (splittedLog.Length == 17)
            {
                var batchSize = splittedLog[16];
                log.BatchSize = int.Parse(batchSize.Remove(batchSize.Length - 1));
            }
            return log;
        }
    }
}
