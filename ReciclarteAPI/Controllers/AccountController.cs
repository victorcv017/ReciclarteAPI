using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReciclarteAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ReciclarteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> CreateUser([FromBody] SignupInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users { UserName = model.Email, Email = model.Email, Curp = model.Curp };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return BuildToken(new LoginInfo() { Email = model.Email , Password = model.Password} , "User");
                }
                else
                {
                    return BadRequest("Usuario o Contraseña Invalidos");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost("User/Login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginInfo loginInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(loginInfo, "User");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento inválido.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Enterprise/Login")]
        public async Task<IActionResult> EnterpriseLogin([FromBody] LoginInfo loginInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(loginInfo, "Enterprise");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento Inválido.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Center/Login")]
        public async Task<IActionResult> CenterLogin([FromBody] LoginInfo loginInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(loginInfo, "Center");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento Inválido.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Office/Login")]
        public async Task<IActionResult> OfficeLogin([FromBody] LoginInfo loginInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(loginInfo, "Office");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento Inválido.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        private IActionResult BuildToken(LoginInfo loginInfo, string type)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.UniqueName, loginInfo.Email),
                    new Claim("Type", type),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Llave"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "yourdomain.com",
               audience: "yourdomain.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            });

        }

        [HttpGet("User/Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyUser")]
        public async Task<IActionResult> UserProfile()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(_context.Users
                .Include(x => x.Address)
                .Where(x => x.Id == user.Id)
                .ToList()
                .Select(e => new UsersInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    Curp = e.Curp,
                    Email = e.Email,
                    Phone = e.PhoneNumber,
                    Balance = e.Balance,
                    BirthDate = e.BirthDate,
                    Gender = e.Gender,
                    Address = e.Address
                }));
        }

        [HttpGet("User/Transactions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyUser")]
        public async Task<IActionResult> UserTransactions()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(_context.Users
                .Include(x => x.Transactions).ThenInclude(t => t.Sales).ThenInclude(s => s.Center)
                .Include(x => x.Transactions).ThenInclude(t => t.Purchases).ThenInclude(p => p.Item.Office.Enterprise)
                .Where(x => x.Id == user.Id)
                .ToList()
                .Select(e => new UsersInfo
                {
                    Transactions = e.Transactions
                })
                );
        }

    }
}