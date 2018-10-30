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

        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users { UserName = model.Email, Email = model.Email , Curp = model.Curp , Address = model.Address , Name = model.Name , Surname = model.Surname};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return BuildTokenUser(model);
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

        [Route("CreateEnterprise")]
        [HttpPost]
        public async Task<IActionResult> CreateEnterprise([FromBody] EnterpriseInfo model)
        {
            if (ModelState.IsValid)
            {
                var enterprise = new Enterprises { Name = model.Name, RFC = model.RFC, UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(enterprise, model.Password);
                if (result.Succeeded)
                {
                    return BuildTokenEnterprise(model);
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


        [HttpPost]
        [Route("User/Login")]
        public async Task<IActionResult> UserLogin([FromBody] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildTokenUser(userInfo);
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

        [Route("Enterprise/Login")]
        public async Task<IActionResult> EnterpriseLogin([FromBody] EnterpriseInfo enterpriseInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(enterpriseInfo.Email, enterpriseInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildTokenEnterprise(enterpriseInfo);
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

        private IActionResult BuildTokenUser(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("Type", "User"),
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

        private IActionResult BuildTokenEnterprise(EnterpriseInfo enterpriseInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, enterpriseInfo.Email),
                new Claim("Type", "Enterprise"),
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

        [Route("user/profile")]
        [HttpGet]
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
                .ToList()
                .Select(e => new UserInfo
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
                })
                .FirstOrDefault(x => x.Id == user.Id));
        }


        [Route("user/transactions")]
        [HttpGet]
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
                .Include(x => x.Transactions)
                .ToList()
                .Select(e => new UserInfo
                {
                    Id = e.Id,
                    Transactions = e.Transactions
                })
                .FirstOrDefault(x => x.Id == user.Id));
        }

    }
}