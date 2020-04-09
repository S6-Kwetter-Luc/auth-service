using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using auth_service.Domain;
using auth_service.Helpers;
using authenticationservice.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IHasher _hasher;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(IUserRepository repository, IHasher hasher, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task Fill()
        {
            var list = await _repository.Get();
            if (list.Count != 0) return;

            await _repository.Create(new User()
            {
                Name = "Test 1",
                Email = "test1@gmail.com"
            });

            await _repository.Create(new User()
            {
                Name = "Test 2",
                Email = "test2@gmail.com"
            });

            await _repository.Create(new User()
            {
                Name = "Test 3",
                Email = "test3@gmail.com"
            });

        }

        // TODO Custom Exceptions
        public async Task<User> Authenticate(string viewEmail, string viewPassword)
        {
            var user = await _repository.Get(viewEmail);
            if (user == null) throw new AppException("There is no user with this email");

            if (!await _hasher.VerifyHash(viewPassword, user.Salt, user.Password))
                throw new AppException("The password is not correct");

            user.Token = _tokenGenerator.CreateToken(user.Id);

            return user.RemovePassword().RemoveSalt();
        }

        public async Task<User> Create(string viewName, string viewEmail, string viewPassword)
        {
            var emailuser = await _repository.Get(viewEmail);
            if (emailuser != null) throw new AppException("Email \"" + viewEmail + "\" is already being used");

            var salt = _hasher.CreateSalt();
            var password = await _hasher.HashPassword(viewPassword, salt);
            var user = new User()
            {
                Email = viewEmail,
                Name = viewName,
                Salt = salt,
                Password = password,
                OauthIssuer = "none",
            };

            return (await _repository.Create(user)).RemovePassword().RemoveSalt();
        }

        public async Task<List<User>> GetAll()
        {
            return await _repository.Get();
        }
    }
}