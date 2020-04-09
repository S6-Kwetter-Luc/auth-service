using System;

namespace auth_service.Helpers
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generates token for a certain Id
        /// </summary>
        /// <param name="id">Id of user</param>
        /// <returns>token</returns>
        string CreateToken(Guid id);
    }
}