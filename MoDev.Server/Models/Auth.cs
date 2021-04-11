using MoDev.Entities;


namespace MoDev.Server.Models
{
    public class ValidatedAuthResult
    {
        public bool IsAuthenticated { get; set; }
        public AuthToken AuthToken { get; set; }
    }
}