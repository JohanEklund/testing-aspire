using App2.Domain;
using App2.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App2.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(App2DbContext dbContext, IApp1Client client) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Order>> Get()
        {
            return Ok(await dbContext.Orders.ToListAsync());
        }

        [HttpGet("user-and-order")]
        public async Task<ActionResult> GetUserAndOrders()
        {
            var users = await client.GetUsersAsync();
            var orders = await dbContext.Orders.ToListAsync();

            var result = users.Join(orders,
                user => user.Id,
                order => order.UserId,
                (user, order) => new
                {
                    UserId = user.Id,
                    UserName = $"{user.FirstName} {user.LastName}",
                    order.OrderNumber,
                    order.OrderDate
                });

            return Ok(result);
        }
    }
}
