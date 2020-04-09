using System.Threading.Tasks;

namespace auth_service.Helpers
{
    public interface IHasher
    {

        /// <summary>
        /// Creates hash from password and salt
        /// </summary>
        /// <param name="password">password in string</param>
        /// <param name="salt">salt created by the CreateSalt() method</param>
        /// <returns>Byte[] of hashed password</returns>
        Task<byte[]> HashPassword(string password, byte[] salt);

        /// <summary>
        /// Verifies Hash
        /// </summary>
        /// <param name="password">password in string</param>
        /// <param name="salt">salt byte[]</param>
        /// <param name="hash">hashed password byte[]</param>
        /// <returns></returns>
        Task<bool> VerifyHash(string password, byte[] salt, byte[] hash);

        /// <summary>
        /// Creates salt
        /// </summary>
        /// <returns>salt</returns>
        byte[] CreateSalt();
    }
}