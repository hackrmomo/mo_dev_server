using System.Linq;
using System;
using MoDev.Entities;
using BC = BCrypt.Net;
namespace MoDev.Server.Services
{
    public interface IAuthService
    {
        public bool ValidateToken(string token);
        public AuthToken GetTokenDetails(string token);
        public bool CheckPassword(string password);
        public AuthToken CreateToken();
    }
    public class AuthService : BaseService, IAuthService
    {
        public bool ValidateToken(string token)
        {
            var resolvedToken = GetTokenDetails(token);
            if (resolvedToken != null && DateTime.UtcNow.CompareTo(resolvedToken.TimeStamp.AddMonths(1)) < 0)
            {
                return true;
            }
            return false;
        }

        public AuthToken GetTokenDetails(string token)
        {
            return Context.AuthTokens.Where(a => a.Token == token).First();
        }

        public bool CheckPassword(string password)
        {
            var hashedPass = Environment.GetEnvironmentVariable("HASHED_ADMIN_PASS");
            if (!string.IsNullOrEmpty(hashedPass))
            {
                return BC.BCrypt.Verify(text: password, hash: hashedPass);
            }
            return false;
        }

        public AuthToken CreateToken()
        {
            try
            {
                var generatedToken = new AuthToken {
                    TimeStamp = DateTime.UtcNow,
                    Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                };
                Context.AuthTokens.Add(generatedToken);
                Context.SaveChanges();
                return generatedToken;
            }
            catch
            {
                return null;
            }
        }
    }
}