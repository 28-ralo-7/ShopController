using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopControllersExample.Database;
using ShopControllersExample.Models;

namespace ShopControllersExample.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ShopContext _context;

        public OrderController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromHeader] string? token)
        {
            
            var loginedUser = _context.Users
                .SingleOrDefault(
                    w => (w.Name + w.Id) == token
                );
            
            var orders = await _context.Orders
                .Where(w => w.UserId == loginedUser.Id)
                .Select(t => new OrderReadDto()
            {
                CreatedAt = t.CreatedAt,
                Number = t.Number,
                ClientId = t.ClientId,
                UserId = (int)t.UserId
            }).ToListAsync();
            return Ok(orders);
        }

        [HttpPost]
        //public async Task<IActionResult> PostOrder([FromBody] OrderWriteDto orderDto, [FromHeader] string? token)
        //{
        //    var loginedUser = _context.Users.SingleOrDefault( w => (w.Name + w.Id) == token);
        //    if(loginedUser==null) return Unauthorized();

        //    var orders = _context.Orders.Where(w => loginedUser.Id == w.Id);                                              !!!!!!!!!!!Я здесь, делаю добавление
        //    var orderPut = 



        //    //if (orderDto == null) BadRequest();
        //    //var loginedUser = _context.Users
        //    //                .SingleOrDefault( w => (w.Name + w.Id) == token);
        //    //var order = new Order()
        //    //{
        //    //    CreatedAt = DateTime.Now,
        //    //    Number = orderDto.Number,
        //    //    ClientId = orderDto.ClientId,
        //    //    UserId = loginedUser.Id
        //    //};
        //    //_context.Orders.Add(order);
        //    //_context.SaveChanges();

        //    //return Ok(order);
        //}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id, [FromHeader] string? token)
        {
            var loginedUser = _context.Users
                            .SingleOrDefault(w => (w.Name + w.Id) == token);

            var order = await _context.Orders.SingleOrDefaultAsync(x => x.Id == id);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromHeader] int id, [FromHeader] string token)
        {
            var loginedUser = _context.Users
                            .Where(w => (w.Name + w.Id) == token).SingleOrDefaultAsync();


            if (loginedUser == null) return Unauthorized();

            var order = await _context.Orders
                .Where(w => w.Id == id)
                .Select(t => new OrderReadDto()
                {
                    Id = t.Id,
                    CreatedAt = t.CreatedAt,
                    UserId = t.UserId,
                    Number = t.Number,
                    ClientId = t.ClientId,
                    ClientName = t.Client.Name,
                    UserName = t.User.Name,
                    Position = t.Structures.Select(p => new OrderCompositionDto()
                    {
                        GoodsId = p.GoodsId,
                        Count = p.Count,
                        Price = p.Price,
                        GoodsName = p.Goods.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if(order==null) return NotFound();

            return Ok(order);

        }
        //[HttpPut("{id}")]
        //public async Task<IActionResult> EditOrder()
        //{
        //    var order = _context.Orders.SingleOrDefault(x => x.Id == id);
        //    if (order == null) return NotFound();
            
        //}

    }

}
