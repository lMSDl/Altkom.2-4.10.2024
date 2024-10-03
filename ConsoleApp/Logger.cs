﻿namespace ConsoleApp
{
    public class Logger
    {
        private Dictionary<DateTime, string> _logs = new Dictionary<DateTime, string>();

        public event EventHandler<EventArgs>? MessageLogged;

        public void Log(string message)
        {
            var dateTime = DateTime.Now;
            _logs[dateTime] = message;

            MessageLogged?.Invoke(this, new LoggerEventArgs(dateTime, message));
        }

        public Task<string> GetLogsAsync(DateTime from, DateTime to)
        {
            return Task.Run(() => string.Join("\n", _logs.Where(x => x.Key >= from).Where(x => x.Key <= to)
            .Select(x => $"{x.Key.ToShortDateString()} {x.Key.ToShortTimeString()}: {x.Value}")));
        }

        public class LoggerEventArgs(DateTime dateTime, string message) : EventArgs
        {
            public DateTime DateTime => dateTime;
            public string Message => message;
        }
    }
}
