using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopControllersExample.Database;
using ShopControllersExample.Models;

namespace ShopControllersExample.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ShopContext _context;

        public OrderController(ShopContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var loginedUser = HttpContext.User.Claims
                .Where(
                    w => w.Type == "Id"
                ).FirstOrDefault();
            
            var orders = await _context.Orders
                .Where(w => w.UserId == Convert.ToInt32(loginedUser.Value))
                .Select(t => new OrderReadDto()
            {
                Id = t.Id,
                CreatedAt = t.CreatedAt,
                Number = t.Number,
                ClientName = t.Client.Name, 
                UserName = t.User.Name,
                    Position = t.Structures.Select(p => new OrderCompositionDto()
                    {
                        GoodsId = p.GoodsId,
                        Count = p.Count,
                        Price = p.Price,
                        GoodsName = p.Goods.Name
                    }).ToList()
                    //UserName = t.Client.Name.Where()
                }).ToListAsync();
            return Ok(orders);
        }



        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderWriteDto orderDto, [FromHeader] string? token)
        {
            var loginedUser = _context.Users.SingleOrDefault(w => (w.Name + w.Id) == token);
            if (loginedUser == null) return Unauthorized();

            var orders = _context.Orders.Where(w => loginedUser.Id == w.Id); /*!!!!!!!!!!!Я здесь, делаю добавление*/
            var orderPut = new Order()
            {
                CreatedAt = DateTime.Now,
                Number = orderDto.Number,
                ClientId = orderDto.ClientId,
                UserId = loginedUser.Id
            
            };
            _context.Orders.Add(orderPut);
            _context.SaveChanges();

            var goodsIds = orderDto.Position.Select(t=>t.GoodsId).ToList();
            var goods = await _context.Goods.Where(x=> goodsIds.Contains(x.Id)).ToListAsync();

            foreach(var item in orderDto.Position)
            {
                var findgoods = goods.Where(w => w.Id == item.GoodsId).FirstOrDefault();

                if (findgoods!= null)
                {
                    var position = new OrderComposition()
                    {
                        GoodsId = item.GoodsId,
                        Count = item.Count,
                        OrderId = orderPut.Id,
                        Price = findgoods.Price

                    };
                    _context.orderStructures.Add(position);
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id, [FromHeader] string? token)
        {
            var loginedUser = _context.Users
                            .SingleOrDefault(w => (w.Name + w.Id) == token);

            var order = await _context.Orders.SingleOrDefaultAsync(x => x.Id == id);
            if (order == null) return NotFound();

            return Ok(order);
        }

        //[HttpPut]
        //public async 




        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromHeader] int id)
        {
            var loginedUser = HttpContext.User.Claims
                            .Where(w => w.Type == "Id")
                            .FirstOrDefault();
            if (loginedUser == null) return Unauthorized();

           var order = await _context.Orders.SingleOrDefaultAsync(x => x.Id == id);
            _context.Orders.Remove(order);
            _context.SaveChanges();

            return Ok();

        }


    }

}
