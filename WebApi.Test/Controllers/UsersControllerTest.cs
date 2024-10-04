using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApi.Test.Controllers
{
    public class UsersControllerTest
    {
        [Fact]
        public async void Get_OkWithAllUsers()
        {
            //Arrange
            var userService = new Mock<ICrudService<User>>();
            var users = new Fixture().CreateMany<User>().ToList();
            userService.Setup(x => x.ReadAsync()).ReturnsAsync(users);

            var controller = new UsersController(userService.Object);

            //Act
            var result = await controller.Get();

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultList = Assert.IsAssignableFrom<IEnumerable<User>>(actionResult.Value);
            Assert.Equal(users, resultList);
        }

        [Fact]
        public async void Get_ExistingId_OkWithUser()
        {
            //Arrange
            var userService = new Mock<ICrudService<User>>();
            var user = new Fixture().Create<User>();
            userService.Setup(x => x.ReadAsync(user.Id)).ReturnsAsync(user);

            var controller = new UsersController(userService.Object);

            //Act
            var result = await controller.Get(user.Id);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultUser = Assert.IsAssignableFrom<User>(actionResult.Value);
            Assert.Equal(user, resultUser);
        }

        [Fact]
        public async void Get_NotExistingId_NotFound()
        {
            /*//Arrange
            var userService = new Mock<ICrudService<User>>();
            var userId = new Fixture().Create<int>();

            var controller = new UsersController(userService.Object);

            //Act
            var result = await controller.Get(userId);

            //Assert
            Assert.IsType<NotFoundResult>(result);*/

            await ReturnsNotFound((controller, id) => controller.Get(id));
        }

        [Fact]
        public async void Put_NotExistingId_NotFound()
        {
            await ReturnsNotFound((controller, id) => controller.Put(id, default));
        }

        [Fact]
        public async void Delete_NotExistingId_NotFound()
        {
            await ReturnsNotFound((controller, id) => controller.Delete(id));
        }


        private async Task ReturnsNotFound(Func<UsersController, int, Task<IActionResult>> func)
        {
            //Arrange
            var userService = new Mock<ICrudService<User>>();
            var userId = new Fixture().Create<int>();

            var controller = new UsersController(userService.Object);

            //Act
            var result = await func(controller, userId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
