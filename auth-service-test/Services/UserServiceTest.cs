using System;
using System.Threading.Tasks;
using auth_service.Domain;
using auth_service.Helpers;
using auth_service.Services;
using authenticationservice.Repositories;
using MongoDB.Bson;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace auth_service_test.Services
{
    public class UserServiceTest
    {

        private readonly ITestOutputHelper _testOutputHelper;
        private IUserService _service;

        public UserServiceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        #region Authentication Tests
        [Fact]
        public async Task Authenticate_Successful()
        {
            var repository = new Mock<IUserRepository>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            var salt = hasher.CreateSalt();
            var hashedPassword = await hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            repository.Setup(r => r.Get("test@test.nl")).Returns(async () => new User()
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

            tokenGenerator.Setup(t => t.CreateToken(guid)).Returns("");

            _service = new UserService(repository.Object, hasher, tokenGenerator.Object);

            var result = await _service.Authenticate("test@test.nl", "testtest");


            var expectedUser = new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Name = "test1",
                Salt = null,
                Password = null,
                OauthIssuer = "none",
                Token = "",
                OauthSubject = ""
            };

            _testOutputHelper.WriteLine(result.ToJson());
            _testOutputHelper.WriteLine(expectedUser.ToJson());

            Assert.Equal(expectedUser.ToJson(), result.ToJson());
        }

        [Fact]
        public async Task Authenticate_WrongPassword()
        {
            var repository = new Mock<IUserRepository>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            var salt = hasher.CreateSalt();
            var hashedPassword = await hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            repository.Setup(r => r.Get("test@test.nl")).Returns(async () => new User()
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

            tokenGenerator.Setup(t => t.CreateToken(guid)).Returns("");


            _service = new UserService(repository.Object, hasher, tokenGenerator.Object);

            Exception ex = await Assert.ThrowsAsync<AppException>(() => _service.Authenticate("test@test.nl", "wrongpassword"));

            Assert.Equal("The password is not correct", ex.Message);
        }

        [Fact]
        public async Task Authenticate_NoUser()
        {
            var repository = new Mock<IUserRepository>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            _service = new UserService(repository.Object, hasher, tokenGenerator.Object);

            Exception ex = await Assert.ThrowsAsync<AppException>(() => _service.Authenticate("test@test.nl", "testtest"));

            Assert.Equal("There is no user with this email", ex.Message);
        }
        #endregion
    }
}