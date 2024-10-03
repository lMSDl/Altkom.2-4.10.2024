using AutoFixture;

namespace ConsoleApp.Test.NUnit
{
    public class GardenTest
    {
        private Garden Garden { get; set; }
        //BadPractice
        [SetUp]
        public void SetUp()
        {
            Garden = new Garden(int.MaxValue);
        }

        //BadPractice
        [TearDown]
        public void CelanUp()
        {
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

        [Test]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string validPlantName = new Fixture().Create<string>();
            Garden garden = CreateGardenCustomSize(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(validPlantName);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
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
            Assert.That(result, Is.False);
        }

        //[Theory]
        [TestCase(2, 3)]
        [TestCase(4, 5)]
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
            Assert.That(garden.GetPlants().Except(plants), Does.Contain(expectedName));
        }

        [Test]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            Garden garden = CreateGardenInsignificantSize();
            const string? NULL_PLANT_NAME = null;
            const string EXPECTED_PARAMETER_NAME = "name";

            //Act
            TestDelegate action = () => garden.Plant(NULL_PLANT_NAME);

            //Assert
            Assert.Throws(Is.InstanceOf<ArgumentNullException>().And
                .Property(nameof(ArgumentNullException.ParamName)).EqualTo(EXPECTED_PARAMETER_NAME), action);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r")]
        [TestCase(" ")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string name)
        {
            //Arrange
            Garden garden = CreateGardenInsignificantSize();
            const string EXPECTED_PARAMETER_NAME = "name";
            string expectedMessage = Properties.Resources.WhitespaceNameException;

            //Act
            TestDelegate action = () => garden.Plant(name);

            //Assert
            Assert.Throws(Is.InstanceOf<ArgumentException>().And
                .Property(nameof(ArgumentException.ParamName)).EqualTo(EXPECTED_PARAMETER_NAME).And
                .Property(nameof(ArgumentException.Message)).StartsWith(expectedMessage), action);
        }

        [Test]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            Garden garden = CreateGardenInsignificantSize();

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.That(result1, Is.Not.SameAs(result2));
        }
    }
}