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

        [HttpPost("Buy")]
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
            _context.Transactions.Add(transaction);
            //_context.SaveChanges();
            return Ok(transaction);

        }

    }
}