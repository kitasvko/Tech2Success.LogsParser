using System;

namespace Tech2Success.LogsParser.Models
{
    public class Log
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int ThreadId { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"Date: {Date.ToShortDateString()} Time: {Time:hh\\:mm\\:ss\\:fff} ThreadId: {ThreadId} Duration: {Duration:hh\\:mm\\:ss\\:fff}";
        }
    }
}
