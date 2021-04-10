using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MoDev.Server.Models;
using MoDev.Server.Services;

namespace MoDev.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("validate/{token}")]
        public ValidatedAuthResult ValidateToken(string token)
        {
            if (_service.ValidateToken(token))
            {
                HttpContext.Response.StatusCode = 200;
                return new ValidatedAuthResult { IsAuthenticated = true, AuthToken = _service.GetTokenDetails(token) };
            }
            HttpContext.Response.StatusCode = 401;
            return new ValidatedAuthResult { IsAuthenticated = false };
        }

        [HttpPost]
        [Route("login")]
        public ValidatedAuthResult AddToken([FromQuery] string password)
        {
            if (_service.CheckPassword(password))
            {
                HttpContext.Response.StatusCode = 201;
                return new ValidatedAuthResult { IsAuthenticated = true, AuthToken = _service.CreateToken() };
            }
            HttpContext.Response.StatusCode = 401;
            return new ValidatedAuthResult { IsAuthenticated = false };
        }
    }
}