using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Text.xUnit
{
    public class ProgramTest
    {
        [Fact]
        public void Main_ConsoleOutput_HelloWorld()
        {
            //Arrange
            var main = typeof(Program).Assembly.EntryPoint;
            TextWriter consoleWriter;
            consoleWriter = Console.Out;

            using StringWriter writer = new StringWriter();
            Console.SetOut(writer);

            //Act
            main.Invoke(null, new object[] { Array.Empty<string>() });

            //Assert
            Assert.Equal("Hello, World!\r\n", writer.ToString());
            Console.SetOut(consoleWriter);
        }


    }
}
