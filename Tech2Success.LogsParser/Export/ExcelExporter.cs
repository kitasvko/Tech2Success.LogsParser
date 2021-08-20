using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tech2Success.LogsParser.Models;

namespace Tech2Success.LogsParser.Export
{
    public class ExcelExporter
    {
        private readonly string _path;
        public ExcelExporter(string path)
        {
            _path = path;
        }

        public void ExportToExcel(IList<Log> logs)
        {
            var csv = new StringBuilder();

            for (int i = 0; i < logs.Count; i++)
            {
                var newLine = string.Format("{0},{1:hh\\:mm\\:ss\\:fff},{2},{3:hh\\:mm\\:ss\\:fff}", 
                    logs[i].Date.ToShortDateString(), logs[i].Time, logs[i].ThreadId, logs[i].Duration);
                csv.AppendLine(newLine);
            }

            File.WriteAllText(_path, csv.ToString());
        }
    }
}
