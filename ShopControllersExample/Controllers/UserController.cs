using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopControllersExample.Database;
using ShopControllersExample.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShopControllersExample.Controllers
{
    [Route("api/users")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly ShopContext _context;

        public UserController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetUsers([FromQuery] string? filter)
        {

            var usersQuery = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                usersQuery = usersQuery.Where(w => w.Name.ToLower() == filter.ToLower());
            }
            var users = await usersQuery.Select(t => new UserDto()
            {
                Name = t.Name,
                Login = t.Login,
                Password = t.Password
            }).ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AddUsers([FromBody] UserDto userDto)
        {
            if (userDto == null) BadRequest();
            var user = new User()
            {
                Name = userDto.Name,
                Password = userDto.Password,
                Login = userDto.Login
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);

        }

        [HttpGet("{login}")]

        public async Task<IActionResult> LoginUser(string login, [FromQuery] string password)
        {
            var userInfo = _context.Users.SingleOrDefault(w => w.Login == login && w.Password == password);
            if (login == null && password == null)
                return BadRequest();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userInfo.Name), new Claim("Id", userInfo.Id.ToString()) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(999)),
                    signingCredentials: new SigningCredentials(AuthOptions
                                        .GetSymmetricSecurityKey(),
                                        SecurityAlgorithms.HmacSha256));

            return Ok($"Bearer {new JwtSecurityTokenHandler().WriteToken(jwt)}");

        }
    }
}
