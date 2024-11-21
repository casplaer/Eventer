using Eventer.Application.Interfaces.Auth;

namespace Eventer.Infrastructure.Data
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GenerateHash(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
