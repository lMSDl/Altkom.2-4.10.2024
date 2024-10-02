using AutoFixture;

namespace ConsoleApp.Text.xUnit
{
    public class GardenTest
    {
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
            Garden garden = new Garden(MINIMAL_VALID_SIZE);

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
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            garden.Plant(validPlantName);

            //Act
            var result = garden.Plant(validPlantName);

            //Assert
            Assert.False(result);
        }

        [Fact]
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
        }
    }
}