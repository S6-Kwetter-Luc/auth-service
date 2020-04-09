using System.Collections.Generic;
using System.Linq;
using auth_service.Domain;

namespace auth_service.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users) {
            return users.Select(x => x.RemovePassword());
        }

        public static User RemovePassword(this User user) {
            user.Password = null;
            return user;
        }

        public static User RemoveSalt(this User user) {
            user.Salt = null;
            return user;
        }
    }
}