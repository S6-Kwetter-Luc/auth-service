using System;
using System.Threading.Tasks;
using auth_service.Controllers;
using auth_service.Helpers;
using auth_service.Models;
using auth_service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace auth_service_test2.controllers
{
    [TestFixture]
    public class UsersControllerTest
    {
        [Test]
        public void test()
        {

        }

        private UsersController _controller;
        private Hasher _hasher;


        [Test]
        public async Task Create_Succesful()
        {
            var userService = new Mock<IUserService>();
            _hasher = new Hasher();

            var salt = _hasher.CreateSalt();
            var hashedPassword = await _hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            // userService.Setup(p => p.Create("test1", "test@test.nl", "testtest"))
            //     .Returns(async () => new User()
            //     {
            //         Id = guid,
            //         Email = "test@test.nl",
            //         Name = "test1",
            //         Salt = salt,
            //         Password = hashedPassword,
            //         OauthIssuer = "none",
            //         Token = "",
            //         OauthSubject = ""
            //     });
            _controller = new UsersController(userService.Object);

            IActionResult result = _controller.Register(new RegisterModel()
                { Name = "test1", Email = "test@test.nl", Password = "testtest" });


            Assert.IsInstanceOf<OkResult>(result);

            // Assert.IsType<OkResult>(result);
        }

        public async Task Create_Exception()
        {
            var userService = new Mock<IUserService>();
            _controller = new UsersController(userService.Object);

            userService.Setup(p => p.Create("test1", "test@test.nl", "testtest"))
                .Throws(new AppException("Something went wrong"));

            IActionResult result = _controller.Register(new RegisterModel()
                { Name = "test1", Email = "test@test.nl", Password = "testtest" });


            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}