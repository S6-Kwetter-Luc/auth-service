using System;
using System.Threading.Tasks;
using auth_service.Controllers;
using auth_service.Domain;
using auth_service.Helpers;
using auth_service.Models;
using auth_service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace auth_service_test.Controllers
{
    public class UsersControllerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private UsersController _controller;
        private Hasher _hasher;

        public UsersControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _hasher = new Hasher();
        }

        [Fact]
        public async Task Create_Successful()
        {
            var userService = new Mock<IUserService>();

            _controller = new UsersController(userService.Object);

            var result = _controller.Register(new RegisterModel()
                {Name = "test1", Email = "test@test.nl", Password = "testtest"});

            _testOutputHelper.WriteLine(result.ToString());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Create_Badresult()
        {
            var userService = new Mock<IUserService>();
            _controller = new UsersController(userService.Object);

            userService.Setup(p => p.Create("test1", "test@test.nl", "testtest"))
                .Throws(new AppException("Something went wrong"));

            var result = _controller.Register(new RegisterModel()
                {Name = "test1", Email = "test@test.nl", Password = "testtest"});

            _testOutputHelper.WriteLine(result.ToString());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Authenticate_Successful()
        {
            var userService = new Mock<IUserService>();
            _controller = new UsersController(userService.Object);

            var salt = _hasher.CreateSalt();
            var hashedPassword = await _hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            userService.Setup(p => p.Authenticate("test@test.nl", "testtest"))
                .Returns(async () => new User()
                {
                    Id = guid,
                    Email = "test@test.nl",
                    Name = "test1",
                    Salt = salt,
                    Password = hashedPassword,
                    OauthIssuer = "none",
                    Token = "",
                    OauthSubject = ""
                });

            var result = await _controller.Authenticate(new AuthenticateModel()
            {
                Email = "test@test.nl",
                Password = "testtest"
            });

            _testOutputHelper.WriteLine(result.ToString());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Authenticate_Badresult()
        {
            var userService = new Mock<IUserService>();
            _controller = new UsersController(userService.Object);

            var result = await _controller.Authenticate(new AuthenticateModel()
            {
                Email = "test@test.nl",
                Password = "testtest"
            });

            _testOutputHelper.WriteLine(result.ToString());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}