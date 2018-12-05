using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class OfficesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfficesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Offices
        [HttpGet]
        public IEnumerable<Offices> GetOffices()
        {
            return _context.Offices;
        }

        // GET: api/Offices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOffices([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var offices = await _context.Offices.FindAsync(id);

            if (offices == null)
            {
                return NotFound();
            }

            return Ok(offices);
        }

        // GET: api/Offices/Items
        [HttpGet("Items")]
        public IEnumerable<Offices> GetOfficesItems()
        {
            return _context.Offices.Include(x => x.Items).ToList();
        }
        /*
        // GET: api/Offices/5/Items
        [HttpGet("{id}/Items")]
        public IEnumerable<Items> GetOfficesItemsOfId([FromRoute] string id)
        {
            var office = _context.Offices.Where(o => o.Id == id).Include(i => i.Items).ToList();
            var items = office[0].Items;
            return items;
        }
        */
        // PUT: api/Offices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffices([FromRoute] string id, [FromBody] Offices offices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != offices.Id)
            {
                return BadRequest();
            }

            _context.Entry(offices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficesExists(id))
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

        // POST: api/Offices
        [HttpPost]
        public async Task<IActionResult> PostOffices([FromBody] Offices offices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Offices.Add(offices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOffices", new { id = offices.Id }, offices);
        }

        // DELETE: api/Offices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffices([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var offices = await _context.Offices.FindAsync(id);
            if (offices == null)
            {
                return NotFound();
            }

            _context.Offices.Remove(offices);
            await _context.SaveChangesAsync();

            return Ok(offices);
        }

        private bool OfficesExists(string id)
        {
            return _context.Offices.Any(e => e.Id == id);
        }

        [HttpPost("MyOffice/sale")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyOffice")]
        public ActionResult Sale([FromBody] BuyInfo model)
        {
            var user = _context.Users.Find(model.UserId);
            if (user is null) return BadRequest("Usuario inválido");
            var transaction = new Transactions() { Date = DateTime.Now, User = user };
            var office = _context.Offices.Include(x => x.Enterprise).FirstOrDefault(x => x.Email == User.Identity.Name);
            if (office is null) return BadRequest("Error del sistema");
            double amount = 0;
            foreach (var pair in model.Items)
            {
                try
                {
                    var item = _context.Items.Find(pair.Key);
                    var purchase = new Purchases()
                    {
                        Item = item,
                        Transaction = transaction,
                        Quantity = pair.Value
                    };
                    amount += pair.Value * item.Value;
                    _context.Purchases.Add(purchase);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            transaction.Amount = amount;
            if (user.Balance - amount < 0)
            {
                return BadRequest("Saldo Insuficiente");
            }

            office.Enterprise.Balance = office.Enterprise.Balance + amount;
            user.Balance = user.Balance - amount;
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return Ok();

        }

        [HttpGet("MyOffice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyOffice")]
        public ActionResult GetMyItems()
        {

            var office = _context.Offices
                .Include(x => x.Items)
                .Where(x => x.Email == User.Identity.Name);
            if (office is null) return BadRequest();
            return Ok(office.Select(
                 e => new OfficesInfo
                 {
                     Id = e.Id,
                     EnterpriseId = e.EnterpriseId,
                     Address = e.Address,
                     Point = e.Point,
                     Schedule = e.Schedule,
                     Items = e.Items

                 }
                ));

        }

        [HttpGet("MyOffice/Transactions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "PolicyOffice")]
        public ActionResult Transactions()
        {

            var office = _context.Offices.Include(x => x.Items).FirstOrDefault(x => x.Email == User.Identity.Name);
            if (office is null) return BadRequest();
            return Ok(_context.Purchases
                .Include(x => x.Transaction)
                    .ThenInclude(x => x.User)
                .Include(x => x.Item)
                    .ThenInclude(x => x.Office)
                    .Where(x => x.Item.OfficesId == office.Id)
                .Select(e => new OfficesTransactionsInfo
                {
                    Id = e.Id,
                    Quantity = e.Quantity,
                    Total = e.Quantity * e.Item.Value,
                    Date = e.Transaction.Date,
                    Item = e.Item,
                    User = e.Transaction.User.Name
                })

           );

        }
    }
}