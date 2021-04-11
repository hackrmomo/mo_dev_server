using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MoDev.Server.Models;
using MoDev.Server.Services;

namespace MoDev.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private IAuthService _authService;
        private IPortfolioService _service;
        public PortfolioController(IAuthService authService, IPortfolioService service)
        {
            _authService = authService;
            _service = service;
        }

        [HttpGet]
        [Route("posts")]
        public PortfolioItemsList GetPortfolioItems()
        {
            HttpContext.Response.StatusCode = 200;
            return new PortfolioItemsList { Items = _service.GetPortfolioItems() };
        }

        [HttpPost]
        [Route("post")]
        public async Task<PortfolioItemsList> InsertPortfolioItem([FromBody] PortfolioItemInsertOrUpdate item)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var isAuthenticated = _authService.ValidateToken(token);
            if (isAuthenticated) 
            {
                HttpContext.Response.StatusCode = 201;
                return new PortfolioItemsList { Items = await _service.InsertNewPortfolioItem(item.Item, item.PortfolioImageBase64) };
            }
            HttpContext.Response.StatusCode = 401;
            return new PortfolioItemsList { Items = _service.GetPortfolioItems() };

        }
    }
}