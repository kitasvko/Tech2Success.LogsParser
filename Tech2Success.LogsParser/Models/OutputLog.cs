using System;

namespace Tech2Success.LogsParser.Models
{
    public class OutputLog
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ThreadId { get; set; }
        public TimeSpan? Duration { get; set; }
        public int BatchSize { get; set; }

        public override string ToString()
        {
            return $"StartDate: {StartDate} EndDate: {EndDate} ThreadId: {ThreadId} Duration: {GetDurationString()} BatchSize: {BatchSize}";
        }

        private string GetDurationString()
        {
            if (EndDate == null)
            {
                return "not finished";
            }

            return $"{Duration:hh\\:mm\\:ss\\:fff}";
        }
    }
}
