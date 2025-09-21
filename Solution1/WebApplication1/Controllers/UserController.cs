using App1.Domain;
using App1.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(AppDbContext dbContext, ILogger<UserController> logger) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<User>> Get()
        {
            return Ok(await dbContext.Users.ToListAsync());
        }
    }
}
