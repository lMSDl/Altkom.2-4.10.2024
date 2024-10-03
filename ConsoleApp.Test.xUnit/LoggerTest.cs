using AutoFixture;
using System.Globalization;

namespace ConsoleApp.Test.xUnit
{
    public class LoggerTest
    {
        [Fact]
        public void Log_AnyInput_EventInvokedOnce()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            int result = 0;
            int expectedResult = 1;

            logger.MessageLogged += (s, e) => { result++; };

            //Act
            logger.Log(log);


            //Assert
            Assert.Equal(expectedResult, result); 
        }

        [Fact]
        public void Log_AnyInput_ValidLoggerEventArgs()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            object? sender = null;
            Logger.LoggerEventArgs? loggerEventArgs = null;

            logger.MessageLogged += (s, e) => { sender = s; loggerEventArgs = e as Logger.LoggerEventArgs; };
            DateTime timeStart = DateTime.Now;

            //Act
            logger.Log(log);

            //Assert
            DateTime timeStop = DateTime.Now;
            Assert.Equal(logger, sender);
            Assert.NotNull(loggerEventArgs);
            Assert.Equal(log, loggerEventArgs.Message);
            Assert.InRange(loggerEventArgs.DateTime, timeStart, timeStop);
        }

        [Fact]
        public async void GetLogAsync_DateRange_LoggerMessage()
        {
            //Arrange
            const int LOG_SPLIT_COUNT = 2;
            var log = new Fixture().Create<string>();
            var logger = new Logger();
            DateTime timeStart = DateTime.Now;
            logger.Log(log);
            DateTime timeStop = DateTime.Now;

            //Act
            var result = await logger.GetLogsAsync(timeStart, timeStop);

            //Assert
            var splitted = result.Split(": ");
            Assert.Equal(LOG_SPLIT_COUNT, splitted.Length);
            Assert.Equal(log, splitted[1]);
            Assert.True(DateOnly.TryParseExact(splitted[0], "dd.MM.yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }

        [Fact]
        public async void DoSth()
        {
            //Arrange
            var logger = new Logger();

            //Act
            await logger.DoSthInternal();

            //Assert
            Assert.True(logger.DoSthEnded);
        }

    }
}
