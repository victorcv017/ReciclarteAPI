using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReciclarteAPI.Models;

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
    }
}