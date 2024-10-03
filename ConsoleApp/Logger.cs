
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ConsoleApp.Test.xUnit")]

namespace ConsoleApp
{
    public class Logger : ILogger
    {
        private Dictionary<DateTime, string> _logs = new Dictionary<DateTime, string>();

        public event EventHandler<EventArgs>? MessageLogged;

        public void Log(string message)
        {
            var dateTime = DateTime.Now;
            _logs[dateTime] = message;

            throw new Exception();

            MessageLogged?.Invoke(this, new LoggerEventArgs(dateTime, message));
        }

        public Task<string> GetLogsAsync(DateTime from, DateTime to)
        {
            return Task.Run(() => string.Join("\n", _logs.Where(x => x.Key >= from).Where(x => x.Key <= to)
            .Select(x => $"{x.Key.ToShortDateString()} {x.Key.ToShortTimeString()}: {x.Value}")));
        }

        public bool DoSthEnded { get; private set; }
        internal async Task DoSthInternal()
        {
            DoSthEnded = false;
            await Task.Delay(2000);
            DoSthEnded = true;
        }
        //by przetestować metodę async void (np. jeśli jest ona wymuszona implementacją interfejsu)
        //tworzymy metodę internal async Task i udostępniamy ją projektowi testującemu
        public async void DoSth()
        {
            await DoSthInternal();
        }

        public class LoggerEventArgs(DateTime dateTime, string message) : EventArgs
        {
            public DateTime DateTime => dateTime;
            public string Message => message;
        }
    }
}
