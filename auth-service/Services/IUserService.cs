using System.Collections.Generic;
using System.Threading.Tasks;
using auth_service.Domain;

namespace auth_service.Services
{
    public interface IUserService
    {
        // Task Update(User user, string password = null);
        // Task Delete(int id);

        /// <summary>
        /// Used to log the user in
        /// </summary>
        /// <param name="email">Email-address of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Authenticated user</returns>
        Task<User> Authenticate(string viewEmail, string viewPassword);

        /// <summary>
        /// Used to create new users
        /// </summary>
        /// <param name="viewName">Account name</param>
        /// <param name="viewEmail"></param>
        /// <param name="viewPassword"></param>
        /// <returns>user without password</returns>
        Task<User> Create(string viewName, string viewEmail, string viewPassword);

        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        Task<List<User>> GetAll();

        /// <summary>
        /// Add 3 new users if the db is empty
        /// </summary>
        /// <returns></returns>
        Task Fill();
    }
}