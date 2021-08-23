using System;

namespace Tech2Success.LogsParser.Models
{
    public class InputLog
    {
        public DateTime Date { get; set; }
        public int ThreadId { get; set; }
        public int BatchSize { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"Date: {Date} ThreadId: {ThreadId} BatchSize: {BatchSize}";
        }
    }
}
