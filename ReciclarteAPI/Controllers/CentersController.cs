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
    public class CentersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CentersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Centers
        [HttpGet]
        public IEnumerable<Centers> GetCenters()
        {
            return _context.Centers
                .Include(x => x.Address)
                .Include(x => x.MaterialsPerCenters).ThenInclude(m => m.Material);
        }

        // GET: api/Centers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCenters([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var centers = await _context.Centers.FindAsync(id);

            if (centers == null)
            {
                return NotFound();
            }

            return Ok(centers);
        }

        // PUT: api/Centers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCenters([FromRoute] long id, [FromBody] Centers centers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != centers.Id)
            {
                return BadRequest();
            }

            _context.Entry(centers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CentersExists(id))
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

        // POST: api/Centers
        [HttpPost]
        public async Task<IActionResult> PostCenters([FromBody] Centers centers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Centers.Add(centers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCenters", new { id = centers.Id }, centers);
        }

        // DELETE: api/Centers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCenters([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var centers = await _context.Centers.FindAsync(id);
            if (centers == null)
            {
                return NotFound();
            }

            _context.Centers.Remove(centers);
            await _context.SaveChangesAsync();

            return Ok(centers);
        }

        private bool CentersExists(long id)
        {
            return _context.Centers.Any(e => e.Id == id);
        }
    }
}