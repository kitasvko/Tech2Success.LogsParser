using System;

namespace Tech2Success.LogsParser.Models
{
    public class Log
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ThreadId { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return $"StartDate: {StartDate} EndDate: {EndDate} ThreadId: {ThreadId} Duration: {Duration:hh\\:mm\\:ss\\:fff}";
        }
    }
}
