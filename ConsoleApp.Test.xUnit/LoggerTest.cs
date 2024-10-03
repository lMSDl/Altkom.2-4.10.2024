using AutoFixture;

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

    }
}
