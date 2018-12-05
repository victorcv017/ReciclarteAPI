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
        public IEnumerable<EnterprisesInfo> GetEnterprises()
        {
            return _context.Enterprises
                .ToList()
                .Select(e => new EnterprisesInfo
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
                .Select(e => new EnterprisesInfo
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
                .ThenInclude(x => x.Address)
                .ToList()
                .Select(e => new EnterprisesInfo
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

        // GET: api/Enterprises/Offices/Items
        [HttpGet("Offices/Items")]
        public ActionResult GetEnterprisesWithOfficesAndItems()
        {
            //return Ok(_context.Offices.Include(o => o.Items));
            return Ok(_context.Enterprises
                .Include(x => x.Offices)
                    .ThenInclude(o => o.Items)
                .ToList()
                .Select(e => new EnterprisesInfo
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

        // GET: api/Enterprises/MyEnterprise
        [HttpGet("MyEnterprise")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public IEnumerable<EnterprisesInfo> GetMyEnterprise()
        {
            return _context.Enterprises
                .Where(e => e.Email == User.Identity.Name)
                .ToList()
                .Select(e => new EnterprisesInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Logo = e.Logo
                });
        }

        // GET: api/Enterprises/MyEnterprise/Offices
        [HttpGet("MyEnterprise/Offices")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult GetOffices()
        {
            var enterprise = _context.Enterprises
                .Include(e => e.Offices)
                    .ThenInclude(e => e.Address)
                .FirstOrDefault(e => e.Email == User.Identity.Name);
            if (enterprise is null) return null;
            var offices = enterprise.Offices.ToList().Select(o => new OfficesInfo
            {
                Id = o.Id,
                Schedule = o.Schedule,
                Point = o.Point,
                Address = o.Address
            });
            return Ok(offices);
        }

        // GET: api/Enterprises/MyEnterprise/Offices/Items
        [HttpGet("MyEnterprise/Offices/Items")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult GetOfficesWithItems()
        {
            var enterprise = _context.Enterprises
                .Include(e => e.Offices)
                    .ThenInclude(e => e.Address)
                .Include(e => e.Offices)
                    .ThenInclude(o => o.Items)
                .FirstOrDefault(e => e.Email == User.Identity.Name);
            if (enterprise is null) return null;
            var offices = enterprise.Offices.ToList().Select(o => new OfficesInfo
            {
                Id = o.Id,
                Schedule = o.Schedule,
                Point = o.Point,
                Address = o.Address,
                Items = o.Items
            });
            if (offices == null)
            {
                return NotFound();
            }
            return Ok(offices);
        }

        // POST: api/Enterprises/MyEnterprise/Offices
        [HttpPost("MyEnterprise/Offices")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeAux model)
        {
            if (ModelState.IsValid)
            {
                if(model.Password is null)
                {
                    return BadRequest();
                }
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
                    return Ok();
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

        // PUT: api/Enterprises/MyEnterprise/Offices
        [HttpPut("MyEnterprise/Offices")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult UpdateOffice([FromBody] OfficeAux model)
        {
            var enterprise = _context.Enterprises.Include(e => e.Offices).FirstOrDefault(e => e.Email == User.Identity.Name);
            if (enterprise is null) return BadRequest();
            var office = enterprise.Offices.FirstOrDefault(x => x.Id == model.Id);
            if (office is null) return BadRequest();
            if (!(model.Schedule is null)) office.Schedule = model.Schedule;
            if (!(model.Point is null)) office.Point = model.Point;
            if (!(model.Address is null)) office.Address = model.Address;
            if (!(model.Password is null))
            {
                if (model.Email is null) return BadRequest();
                var user = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user is null) return BadRequest();
                _userManager.RemovePasswordAsync(user);
                _userManager.AddPasswordAsync(user, model.Password);
            }
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: api/Enterprises/MyEnterprise/Offices
        [HttpDelete("MyEnterprise/Offices")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult DeleteOffice([FromBody] Offices model)
        {
            var enterprise = _context.Enterprises.Include(e => e.Offices).FirstOrDefault(e => e.Email == User.Identity.Name);
            if (enterprise is null) return BadRequest();
            var office = enterprise.Offices.FirstOrDefault(o => o.Id == model.Id);
            if (office is null) return BadRequest();
            _context.Offices.Remove(office);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return BadRequest("Office no eliminada");
            }
            return Ok();
        }

        // POST: api/Enterprises/MyEnterprise/Items
        [HttpPost("MyEnterprise/Items")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public async Task<IActionResult> CreateItem([FromBody] Items model)
        {
            if (ModelState.IsValid)
            {
                var myEnterprise = GetMyEnterprise();
                var office = _context.Offices.Find(model.OfficesId);
                if (office == null)
                {
                    return NotFound();
                }
                if (office.EnterpriseId != myEnterprise.FirstOrDefault().Id)
                {
                    return BadRequest(ModelState);
                }
                model.Office = office;
                _context.Items.Add(model);
                await _context.SaveChangesAsync();

                return Ok(CreatedAtAction("GetItems", new { id = model.Id }, model));
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // PUT: api/Enterprises/MyEnterprise/Items
        [HttpPut("MyEnterprise/Items")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult UpdateItem([FromBody] Items model)
        {
            var enterprise = _context.Enterprises.Include(e => e.Offices)
                                                 .ThenInclude(o => o.Items)
                                                 .FirstOrDefault(e => e.Email == User.Identity.Name);
            if (enterprise is null) return BadRequest();
            var office = enterprise.Offices.FirstOrDefault(o => o.Id == model.OfficesId);
            if (office is null) return BadRequest();
            var item = office.Items.Find(i => i.Id == model.Id);
            if (item is null) return BadRequest();
            if (!(model.Name is null)) item.Name = model.Name;
            if (!(model.Value is 0)) item.Value = model.Value;
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: api/Enterprises/MyEnterprise/Items
        [HttpDelete("MyEnterprise/Items")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult DeleteItem([FromBody] Items model)
        {
            var enterprise = _context.Enterprises.Include(e => e.Offices)
                                                 .ThenInclude(o => o.Items)
                                                 .FirstOrDefault(e => e.Email == User.Identity.Name);
            if (enterprise is null) return BadRequest();
            var office = enterprise.Offices.FirstOrDefault(o => o.Id == model.OfficesId);
            if (office is null) return BadRequest();
            var item = office.Items.Find(i => i.Id == model.Id);
            if (item is null) return BadRequest();
            _context.Items.Remove(item);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return BadRequest("Item no eliminado");
            }
            return Ok();
        }

        // GET: api/Enterprises/MyEnterprise/Items
        [HttpGet("MyEnterprise/Items")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult GetItems()
        {
            var enterprise = _context.Enterprises.Include(e => e.Offices).ThenInclude(o => o.Items).FirstOrDefault(e => e.Email == User.Identity.Name);
            if (enterprise is null) return null;
            List<Items> items = new List<Items>();
            foreach (var office in enterprise.Offices)
                items.AddRange(office.Items.ToList());
            return Ok(items);
        }

        // GET: api/Enterprises/MyEnterprise/Transactions
        [HttpGet("MyEnterprise/Transactions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyEnterprise")]
        public ActionResult Transactions()
        {
            var enterprise = _context.Enterprises.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (enterprise is null) return BadRequest();
            return Ok(_context.Purchases
                .Include(x => x.Item)
                .ThenInclude(x => x.Office)
                .Where(x => x.Item.Office.EnterpriseId == enterprise.Id)
                .Select(e => new EnterprisesTransactionsInfo
                {
                    Id = e.Id,
                    Quantity = e.Quantity,
                    TransactionId = e.TransactionId,
                    Date = e.Transaction.Date,
                    Item = e.Item
                }));
        }
    }
    
    public class OfficeAux : Offices
    {
        public string Password;
    }
}