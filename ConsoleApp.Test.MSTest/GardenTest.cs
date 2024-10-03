using AutoFixture;

namespace ConsoleApp.Test.MSTest
{
    [TestClass]
    public class GardenTest
    {
        private Garden Garden { get; set; }

        [TestInitialize] //BadPractice
        public void SetUpGarden()
        {
            Garden = new Garden(int.MaxValue);
        }

        [TestCleanup] //BadPractice
        public void CleanUp()
        {
            Garden = null;
        }

        private Garden CreateGardenInsignificantSize()
        {
            return new Garden(0);
        }

        private Garden CreateGardenUnlimitedSpace()
        {
            return new Garden(int.MaxValue);
        }

        private Garden CreateGardenCustomSize(int size)
        {
            return new Garden(size);
        }


        [TestMethod]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string validPlantName = new Fixture().Create<string>();
            Garden garden = CreateGardenCustomSize(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(validPlantName);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Plant_GardenFull_False()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string validPlantName = new Fixture().Create<string>();
            Garden garden = CreateGardenCustomSize(MINIMAL_VALID_SIZE);
            garden.Plant(validPlantName);

            //Act
            var result = garden.Plant(validPlantName);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(2, 3)]
        [DataRow(4, 5)]
        public void Plant_DuplicatedName_DuplicationCounterAddedToName(int numberOfCopies, int expectedCounter)
        {
            //Arrange
            string duplicatedName = new Fixture().Create<string>();
            string expectedName = duplicatedName + expectedCounter;
            Garden garden = CreateGardenUnlimitedSpace();
            Enumerable.Repeat(duplicatedName, numberOfCopies).ToList()
                .ForEach(x => garden.Plant(x));
            var plants = garden.GetPlants();

            //Act
            garden.Plant(duplicatedName);

            //Assert
            Assert.IsTrue(garden.GetPlants().Except(plants).Contains(expectedName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            Garden garden = CreateGardenInsignificantSize();
            const string? NULL_PLANT_NAME = null;

            //Act
            garden.Plant(NULL_PLANT_NAME);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\n")]
        [DataRow("\r")]
        [DataRow(" ")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string name)
        {
            //Arrange
            //const int GARDEN_SIZE = 0;
            //Garden garden = new Garden(GARDEN_SIZE);
            Garden garden = CreateGardenInsignificantSize();
            const string EXPECTED_PARAMETER_NAME = "name";
            string expectedMessage = Properties.Resources.WhitespaceNameException;

            //Act
            Action action = () => garden.Plant(name);

            //Assert
            var exception = Assert.ThrowsException<ArgumentException>(action);
            Assert.AreEqual(EXPECTED_PARAMETER_NAME, exception.ParamName);
            Assert.IsTrue(exception.Message.StartsWith(expectedMessage));
        }

        [TestMethod]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            Garden garden = CreateGardenInsignificantSize();

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.AreNotSame(result1, result2);
        }
    }
}