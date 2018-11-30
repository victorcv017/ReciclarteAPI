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
using System.Text.RegularExpressions;
using ReciclarteAPI.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Globalization;
using ReciclarteAPI.Models.Info;

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
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _emailSender = emailSender;
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> CreateUser([FromBody] SignupInfo model)
        {
            if (ModelState.IsValid)
            {
                string aux = "", aux2 = "";
                aux = (model.Surname.Substring(0, 2) + 
                    model.Surname.Split(" ")[1].Substring(0,1) + 
                    model.Name.Split(" ")[0].Substring(0,1)).ToUpper();
                if(model.Name.Split(" ").Length > 1)
                {
                    aux2 = (model.Surname.Substring(0, 2) +
                    model.Surname.Split(" ")[1].Substring(0, 1) +
                    model.Name.Split(" ")[1].Substring(0, 1)).ToUpper();
                }
                if(model.Curp.Substring(0,4) != aux && model.Curp.Substring(0,4) != aux2)
                {
                    return BadRequest("Curp Inválido");
                }

                DateTime date = DateTime.ParseExact(
                    model.Curp.Substring(4, 6),
                    "yyMMdd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
                char gender = model.Curp.ElementAt(10);

                var user = new Users {
                    UserName = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Curp = model.Curp,
                    BirthDate = date,
                    Gender = gender
                };
                TryValidateModel(user);
                if (!ModelState.IsValid) return BadRequest(ModelState);
                try {
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.RouteUrl(
                            "ConfirmEmail",
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(model.Email, "Confirmación de Correo",
                            $"<h3>El equipo de Reciclarte te da la Bienvenida</h3> Por favor confirma tu cuenta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>en el siguiente enlace</a>.");

                        return Ok("Registro Exitoso. Por favor confirma tu correo.");
                    }
                    else
                    {
                        return BadRequest("Usuario o Contraseña Invalidos");
                    }

                }
                catch(Exception e)
                {
                    return BadRequest("Curp Inválida");
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

            var expiration = DateTime.UtcNow.AddDays(3);

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

        [HttpPut("User/Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyUser")]
        public async Task<IActionResult> UpdateProfile([FromBody] UsersProfileInfo profile)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (user is null)
            {
                return NotFound();
            }
            var result = await _userManager.ChangePasswordAsync(user, profile.CurrentPassword, profile.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest("La contraseña no cumple los requerimientos");
            }

            user.Address = profile.Address;
            return Ok("Cambios Exitosos");
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

            return Ok(new UsersTransactionsInfo {
                Sales = _context.Sales
                .Include(x => x.Material)
                .Include(x => x.Center)
                .Include(x => x.Transaction)
                .Where(x => x.Transaction.UserId == user.Id)
                .ToList()
                .Select(e => new UserSalesInfo
                {
                    Id = e.Id,
                    Center = e.Center.Name,
                    Weight = e.Weight,
                    Material = e.Material.Material,
                    Coins = e.Weight * e.Material.Price,
                    Date = e.Transaction.Date
                    
                }),
                Purchases = _context.Purchases
                .Include(x => x.Item)
                .Include(x => x.Transaction)
                .Include(x => x.Item.Office.Enterprise)
                .Where(x => x.Transaction.UserId == user.Id)
                .ToList()
                .Select(e => new UserPurchasesInfo
                {
                    Id = e.Id,
                    Enterprise = e.Item.Office.Enterprise.Logo,
                    Quantity = e.Quantity,
                    Item = e.Item.Name,
                    Coins = e.Quantity * e.Item.Value,
                    Date = e.Transaction.Date
                })
            });
        }



        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "Los datos proveeidos son inválidos");
                return BadRequest(ModelState);
            }
            var user = _context.Users.Find(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Ok("Correo Confirmado. Ya puedes iniciar sesión en la Aplicación");
            }
            else
            {
                return BadRequest(result);
            }
        }

    }
}