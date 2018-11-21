using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReciclarteAPI.Models;
using ReciclarteAPI.Models.Info;

namespace ReciclarteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CentersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Centers
        [HttpGet]
        public IEnumerable<CentersInfo> GetCenters()
        {
            return _context.Centers
                .Include(x => x.Address)
                .Include(x => x.MaterialsPerCenters)
                    .ThenInclude(m => m.Material)
                .ToList()
                .Select(e => new CentersInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Schedule = e.Schedule,
                    Point = e.Point,
                    Logo = e.Logo,
                    Address = e.Address
                });
        }

        // GET: api/Centers/Materials
        [HttpGet("Materials")]
        public IEnumerable<CentersInfo> GetCentersAndMaterials()
        {
            return _context.Centers
                .Include(x => x.Address)
                .Include(x => x.MaterialsPerCenters)
                    .ThenInclude(m => m.Material)
                .ToList()
                .Select(e => new CentersInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Schedule = e.Schedule,
                    Point = e.Point,
                    Logo = e.Logo,
                    Address = e.Address,
                    MaterialsPerCenters = e.MaterialsPerCenters
                });
        }

        // GET: api/Centers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCenters([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var center = await _context.Centers.FindAsync(id);

            if (center == null)
            {
                return NotFound();
            }

            return Ok(new CentersInfo()
            {
                Name = center.Name,
                Schedule = center.Schedule,
                Point = center.Point,
                Address = center.Address

            });
        }

        // GET: api/Centers/id/Materials
        [HttpGet("{id}/Materials")]
        public ActionResult GetMaterials([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<MaterialsPerCenter> materials = _context.MaterialsPerCenter.Where(c => c.CenterId == id).Include(m => m.Material).ToList();

            if (materials == null)
            {
                return NotFound();
            }

            return Ok(materials);
        }

        private bool CentersExists(string id)
        {
            return _context.Centers.Any(e => e.Id == id);
        }

        [HttpPost("MyCenter/Buy")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyCenter")]
        public ActionResult Buy([FromBody] SalesInfo model)
        {

            var user = _context.Users.Find(model.UserId);
            if (user is null) return BadRequest();
            var transaction = new Transactions() { Date = DateTime.Now, User = user };
            var center = _context.Centers.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (center is null) return BadRequest();
            double amount = 0;
            foreach (var pair in model.Materials)
            {
                try
                {
                    var sales = new Sales()
                    {
                        Center = center,
                        Material = _context.Materials.Find(pair.Key),
                        Transaction = transaction,
                        Weight = pair.Value
                    };
                    amount += pair.Value;
                    _context.Sales.Add(sales);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            transaction.Amount = amount;
            user.Balance = user.Balance + amount;
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return Ok();

        }

        [HttpGet("MyCenter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyCenter")]
        public ActionResult GetMyCenter()
        {
            return Ok(_context.Centers
                .Where( x => x.Email == User.Identity.Name)
                .Include(x => x.Address)
                .Include(x => x.MaterialsPerCenters)
                    .ThenInclude(m => m.Material)
                .ToList()
                .Select(e => new CentersInfo
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Schedule = e.Schedule,
                    Point = e.Point,
                    Logo = e.Logo,
                    Address = e.Address,
                    MaterialsPerCenters = e.MaterialsPerCenters
                }));

        }

        [HttpPut("MyCenter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyCenter")]
        public ActionResult UpdateMyCenter([FromBody] CentersInfo model) 
        {
            var center = _context.Centers.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (!(model.Schedule is null)) center.Schedule = model.Schedule;
            if (!(model.Logo is null)) center.Logo = model.Logo;
            if (!(model.Point is null)) center.Point = model.Point;
            if (!(model.Address is null)) center.Address = model.Address;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("MyCenter/Material")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyCenter")]
        public ActionResult AddMaterial([FromBody] long []arg)
        {
            var center = _context.Centers.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (center is null) return BadRequest();
            foreach (long id in arg)
            {
                var material = _context.Materials.Find(id);
                var mat = _context.MaterialsPerCenter.Find(center.Id, material.Id);
                if (!(mat is null)) continue;
                var materialPerCenter = new MaterialsPerCenter() { Center = center, Material = material };
                _context.MaterialsPerCenter.Add(materialPerCenter);

            }
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpDelete("MyCenter/Material")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyCenter")]
        public ActionResult DeleteMaterial([FromBody] long[] arg)
        {
            var center = _context.Centers.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (center is null) return BadRequest();
            foreach (long id in arg)
            {
                var material = _context.Materials.Find(id);
                var mat = _context.MaterialsPerCenter.Find(center.Id, material.Id);
                _context.MaterialsPerCenter.Remove(mat);

            }
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("MyCenter/Transactions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyCenter")]
        public ActionResult Transactions()
        {
            var center = _context.Centers.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (center is null) return BadRequest();
            return Ok(_context.Sales
                .Include(x => x.Transaction)
                .Where(x => x.CenterId == center.Id)
                .Select(e => new CentersTransactionsInfo {
                    Id = e.Id,
                    Weight = e.Weight,
                    Total = e.Weight * e.Material.Price,
                    Date = e.Transaction.Date,
                    Material = e.Material,
                    User = new UsersInfo
                    {
                        Id = e.Transaction.User.Id,
                        Name = e.Transaction.User.Name,
                        Surname = e.Transaction.User.Surname,
                        Email = e.Transaction.User.Email

                    }
                })
                
                );
        }

    }
}