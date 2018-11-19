using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReciclarteAPI.Models;
using ReciclarteAPI.Models.Info;

namespace ReciclarteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterprisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public EnterprisesController(ApplicationDbContext context,
                            UserManager<IdentityUser> userManager,
                            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: api/Enterprises
        [HttpGet]
        public IEnumerable<EnterpriseInfo> GetEnterprises()
        {
            return _context.Enterprises
                .ToList()
                .Select(e => new EnterpriseInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Logo = e.Logo
                });
        }

        // GET: api/Enterprises/5
        [HttpGet("{id}")]
        public ActionResult GetEnterprises([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var enterprises = _context.Enterprises
                .ToList()
                .Select(e => new EnterpriseInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Logo = e.Logo
                })
                .FirstOrDefault(x => x.Id == id);

            if (enterprises == null)
            {
                return NotFound();
            }

            return Ok(enterprises);
        }

        // GET: api/Enterprises/Offices
        [HttpGet("Offices")]
        public ActionResult GetEnterprisesAndOffices()
        {
            return Ok(_context.Enterprises
                .Include(x => x.Offices)
                .ToList()
                .Select(e => new EnterpriseInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Logo = e.Logo,
                    Offices = e.Offices.Select(o => new OfficesInfo
                    {
                        Id = o.Id,
                        Schedule = o.Schedule,
                        Point = o.Point,
                        Address = o.Address

                    })
                }));
        }

        // GET: api/Enterprises/id/Offices
        [HttpGet("{id}/Offices")]
        public ActionResult GetOficces([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var offices = _context.Offices.
                Where(o => o.EnterpriseId == id)
                .Include(x => x.Address)
                .ToList()
                .Select(e => new OfficesInfo {
                    Id = e.Id,
                    Schedule = e.Schedule,
                    Point = e.Point,
                    Address = e.Address
                });

            if (offices == null)
            {
                return NotFound();
            }

            return Ok(offices);
        }

        //for edit verify autenthication and change route
        // PUT: api/Enterprises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnterprises([FromRoute] string id, [FromBody] Enterprises enterprises)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != enterprises.Id)
            {
                return BadRequest();
            }

            _context.Entry(enterprises).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnterprisesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool EnterprisesExists(string id)
        {
            return _context.Enterprises.Any(e => e.Id == id);
        }

        // GET: api/Enterprises/offices/items
        [HttpGet("offices/items")]
        public ActionResult GetEnterprisesWithOfficesAndItems()
        {
            //return Ok(_context.Offices.Include(o => o.Items));
            return Ok(_context.Enterprises
                .Include(x => x.Offices)
                    .ThenInclude(o => o.Items)
                .ToList()
                .Select(e => new EnterpriseInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Logo = e.Logo,
                    Offices = e.Offices.Select(o => new OfficesInfo
                    {
                        Id = o.Id,
                        Schedule = o.Schedule,
                        Point = o.Point,
                        Address = o.Address,
                        Items = o.Items

                    })
                }));
        }

        // GET: api/Enterprises/myEnterprise
        [HttpGet("myEnterprise")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public IEnumerable<EnterpriseInfo> GetMyEnterprise()
        {
            return _context.Enterprises
                .Where(e => e.Email == User.Identity.Name)
                .ToList()
                .Select(e => new EnterpriseInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Logo = e.Logo
                });
        }

        [HttpPost("CreateOffice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeAux model)
        {
            if (ModelState.IsValid)
            {
                var list = _context.Enterprises.Where(e => e.Email == User.Identity.Name).ToList();
                if(list.Count != 1)
                {
                    return NotFound();
                }

                var enterprises = list[0];
                if (enterprises == null)
                {
                    return NotFound();
                }

                var office = new Offices { UserName = model.Email, Email = model.Email, Address = model.Address,
                                           Enterprise = enterprises, Point = model.Point, Schedule = model.Schedule };
                var result = await _userManager.CreateAsync(office, model.Password);
                if (result.Succeeded)
                {
                    return BuildToken(new LoginInfo() { Email = model.Email, Password = model.Password }, "Office");
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
    }
    
    public class OfficeAux : Offices
    {
        public string Password;
    }
}