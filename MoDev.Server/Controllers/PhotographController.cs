using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MoDev.Server.Models;
using MoDev.Server.Services;

namespace MoDev.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PhotographController : ControllerBase
    {
        private IAuthService _authService;
        private IPhotographService _service;
        public PhotographController(IAuthService authService, IPhotographService service)
        {
            _authService = authService;
            _service = service;
        }

        [HttpGet]
        [Route("posts")]
        public PhotographsList GetPhotographsList()
        {
            HttpContext.Response.StatusCode = 200;
            return new PhotographsList { Photographs = _service.GetPhotographs() };
        }

        [HttpPost]
        [Route("post")]
        public async Task<PhotographsList> InsertPhotograph([FromBody] PhotographInsert photographInsert)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var isAuthenticated = _authService.ValidateToken(token);
            if (isAuthenticated)
            {
                HttpContext.Response.StatusCode = 201;
                return new PhotographsList { Photographs = await _service.InsertNewPhotograph(photographInsert.Photograph, photographInsert.PhotographImageBase64) };
            }
            HttpContext.Response.StatusCode = 401;
            return new PhotographsList { Photographs = _service.GetPhotographs() };
        }
    }
}