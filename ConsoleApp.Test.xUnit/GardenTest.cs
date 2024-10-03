using AutoFixture;
using ConsoleApp.Properties;
using FluentAssertions;
using Moq;

namespace ConsoleApp.Test.xUnit
{
    public class GardenTest : IDisposable
    {
        private Garden Garden { get; }
        //xUnit wykorzystuje konstuktor jako metodê SetUp [BadPractice]
        public GardenTest()
        {
            var stubLogger = new Mock<ILogger>();
            Garden = new Garden(int.MaxValue, stubLogger.Object);
        }

        //xUnit wykorzystuje Dispose jako metodê TearDown [BadPractice]
        public void Dispose()
        {
        }

        //zamiast metod SetUp i TearDown zaleca siê twrzonenie przywatnych metod (najlepiej opisuj¹cych intencje)
        private Garden CreateGardenInsignificantSize()
        {
            var stubLogger = new Mock<ILogger>();
            return new Garden(0, stubLogger.Object);
        }


        private Garden CreateGardenUnlimitedSpace()
        {
            var stubLogger = new Mock<ILogger>();
            return new Garden(int.MaxValue, stubLogger.Object);
        }

        private Garden CreateGardenCustomSize(int size)
        {
            var stubLogger = new Mock<ILogger>();
            return new Garden(size, stubLogger.Object);
        }


        [Fact]
        //public void Plant_GivesTrueWhenNameIsValid()
        //<nazmwa metody>_<scenariusz>_<oczekiwany rezultat>
        //public void Plant_PassValidName_ReturnsTrue()
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1; //opisujemy swoje intencje
            //const string VALID_PLANT_NAME = "0"; //piszemy testy z minimalnym przekazem (parametry o jak najmniejszym polem do interpretacji)
            string validPlantName = new Fixture().Create<string>();
            Garden garden = CreateGardenCustomSize(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(validPlantName);

            //Assert
            //spawdzamy tylko jedn¹ rzecz
            Assert.True(result);
        }

        [Fact]
        //public void Plant_NoMoreSpaceInGarden_ResturnsFalse()
        public void Plant_GardenFull_False ()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string validPlantName = new Fixture().Create<string>();
            Garden garden = CreateGardenCustomSize(MINIMAL_VALID_SIZE);
            garden.Plant(validPlantName);

            //Act
            var result = garden.Plant(validPlantName);

            //Assert
            Assert.False(result);
        }

        /*[Fact]
       // public void Plant_DuplicatedName_ChangedName()
        public void Plant_DuplicatedName_DuplicationCounterAddedToName()
        {
            //Arrange
            const int GARDEN_SIZE = int.MaxValue;
            string duplicatedName = new Fixture().Create<string>();
            string expectedName = duplicatedName + "2";
            Garden garden = new Garden(GARDEN_SIZE);
            garden.Plant(duplicatedName);

            //Act
            garden.Plant(duplicatedName);

            //Assert
            Assert.Contains(expectedName, garden.GetPlants());
        }

        [Fact]
        // public void Plant_DuplicatedName_ChangedName()
        public void Plant_DuplicatedName_DuplicationCounterAddedToName_x4()
        {
            //Arrange
            const int GARDEN_SIZE = int.MaxValue;
            string duplicatedName = new Fixture().Create<string>();
            string expectedName = duplicatedName + "4";
            Garden garden = new Garden(GARDEN_SIZE);
            garden.Plant(duplicatedName);
            garden.Plant(duplicatedName);
            garden.Plant(duplicatedName);

            //Act
            garden.Plant(duplicatedName);

            //Assert
            Assert.Contains(expectedName, garden.GetPlants());
        }*/

        [Theory]
        [InlineData(2, 3)]
        [InlineData(4, 5)]
        public void Plant_DuplicatedName_DuplicationCounterAddedToName(int numberOfCopies, int expectedCounter)
        {
            //Arrange
            //const int GARDEN_SIZE = int.MaxValue;
            string duplicatedName = new Fixture().Create<string>();
            string expectedName = duplicatedName + expectedCounter;
            //Garden garden = new Garden(GARDEN_SIZE);
            Garden garden = CreateGardenUnlimitedSpace();
            Enumerable.Repeat(duplicatedName, numberOfCopies).ToList()
                .ForEach(x => garden.Plant(x));
            var plants = garden.GetPlants();

            //Act
            garden.Plant(duplicatedName);

            //Assert
            Assert.Contains(expectedName, garden.GetPlants().Except(plants));
        }

        [Fact]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            //const int GARDEN_SIZE = 0;
            //Garden garden = new Garden(GARDEN_SIZE);
            Garden garden = CreateGardenInsignificantSize();
            const string? NULL_PLANT_NAME = null;
            const string EXPECTED_PARAMETER_NAME = "name";

            //Act
            Action action = () => garden.Plant(NULL_PLANT_NAME);

            //Assert
            //ThrowsAny - uwzglêdnia dziedziczenie klas wyj¹tków
            //var exception = Assert.ThrowsAny<ArgumentException>(action);
            //Throws - sprawdza konkretny typ
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal(EXPECTED_PARAMETER_NAME, exception.ParamName);
        }

        [Fact(Skip = "Replaced by theory")]
        public void Plant_EmptyName_ArgumentException()
        {
            //Arrange
            //const int GARDEN_SIZE = 0;
            //Garden garden = new Garden(GARDEN_SIZE);
            Garden garden = CreateGardenInsignificantSize();
            const string EMPTY_PLANT_NAME = "";
            const string EXPECTED_PARAMETER_NAME = "name";
            string expectedMessage = Properties.Resources.WhitespaceNameException;

            //Act
            var exception = Record.Exception(() => garden.Plant(EMPTY_PLANT_NAME));

            //Assert
            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);
            Assert.Equal(EXPECTED_PARAMETER_NAME, argumentException.ParamName);
            Assert.Contains(expectedMessage, argumentException.Message);
        }

        [Fact(Skip = "Replaced by theory")]
        public void Plant_WhitespaceName_ArgumentException()
        {
            //Arrange
            //const int GARDEN_SIZE = 0;
            //Garden garden = new Garden(GARDEN_SIZE);
            Garden garden = CreateGardenInsignificantSize();
            const string WHITESPACE_PLANT_NAME = " ";
            const string EXPECTED_PARAMETER_NAME = "name";
            string expectedMessage = Properties.Resources.WhitespaceNameException;

            //Act
            var exception = Record.Exception(() => garden.Plant(WHITESPACE_PLANT_NAME));

            //Assert
            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);
            Assert.Equal(EXPECTED_PARAMETER_NAME, argumentException.ParamName);
            Assert.Contains(expectedMessage, argumentException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData(" ")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string name)
        {
            //Arrange
            //const int GARDEN_SIZE = 0;
            //Garden garden = new Garden(GARDEN_SIZE);
            Garden garden = CreateGardenInsignificantSize();
            const string EXPECTED_PARAMETER_NAME = "name";
            string expectedMessage = Properties.Resources.WhitespaceNameException;

            //Act
            var exception = Record.Exception(() => garden.Plant(name));

            //Assert
            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);
            Assert.Equal(EXPECTED_PARAMETER_NAME, argumentException.ParamName);
            Assert.Contains(expectedMessage, argumentException.Message);
        }

        [Fact]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            //const int GARDEN_SIZE = 0;
            //Garden garden = new Garden(GARDEN_SIZE);
            Garden garden = CreateGardenInsignificantSize();

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.NotSame(result1, result2);
        }


        [Fact]
        public void Plant_ValidName_MessageLogged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1; //opisujemy swoje intencje
            string validPlantName = new Fixture().Create<string>();
            var mockLogger = new Mock<ILogger>();
            //mockLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable(Times.Once);
            mockLogger.Setup(x => x.Log(string.Format(Resources.PlantAddedToGarden, validPlantName))).Verifiable(Times.Once);

            Garden garden = new Garden(MINIMAL_VALID_SIZE, mockLogger.Object);

            //Act
            garden.Plant(validPlantName);

            //Assert
            mockLogger.Verify();
        }

        [Fact]
        public void Plant_DuplicatedName_MessageLogged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 2;
            string duplicatedName = new Fixture().Create<string>();
            string newPlantName = duplicatedName + 2;
            var mockLogger = new Mock<ILogger>();

            Garden garden = new Garden(MINIMAL_VALID_SIZE, mockLogger.Object);
            garden.Plant(duplicatedName);

            //Act
            garden.Plant(duplicatedName);

            //Assert
            mockLogger.Verify(x => x.Log(string.Format(Resources.PlantNameChanged, duplicatedName, newPlantName)), Times.Once);
            //mockLogger.Verify(x => x.Log(It.Is<string>(xx => xx.Contains(duplicatedName))), Times.Exactly(3));
            //mockLogger.Verify(x => x.Log(It.Is<string>(xx => xx.Contains(newPlantName))), Times.Exactly(2));
        }

        [Fact]
        public void GetLastLog_LastLog()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            var logs = new Fixture().CreateMany<string>(2).ToList();
            var stubLogger = new Mock<ILogger>();

            stubLogger.Setup(x => x.GetLogsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(string.Join("\n", logs));

            Garden garden = new Garden(MINIMAL_VALID_SIZE, stubLogger.Object);


            //Act
            var result = garden.GetLastLog();

            //Assert
            result.Should().Be(logs.Last());
            //mockLogger.Verify(x => x.GetLogsAsync(new DateTime(), It.IsAny<DateTime>()));
        }
    }
}