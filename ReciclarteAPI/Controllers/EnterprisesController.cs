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
    public class EnterprisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnterprisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Enterprises
        [HttpGet]
        public IEnumerable<Enterprises> GetEnterprises()
        {
            return _context.Enterprises;
        }

        // GET: api/Enterprises/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnterprises([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var enterprises = await _context.Enterprises.FindAsync(id);

            if (enterprises == null)
            {
                return NotFound();
            }

            return Ok(enterprises);
        }

         // GET: api/Enterprises/id/Offices
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOficces([FromRoute] string id)
        {
            List<Offices> offices =  _context.Offices.Where(o => o.EnterpriseId == id).ToList();

            if (offices == null)
            {
                return NotFound();
            }

            return Ok(offices);



        }


        // GET: api/Enterprises/id/Offices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOffice([FromRoute] long id)
        {
            var office = await _context.Offices.FindAsync(id);

            if (office == null)
            {
                return NotFound();
            }

            return Ok(office);



        }

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

        // POST: api/Enterprises
        [HttpPost]
        public async Task<IActionResult> PostEnterprises([FromBody] Enterprises enterprises)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Enterprises.Add(enterprises);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnterprises", new { id = enterprises.Id }, enterprises);
        }

        // DELETE: api/Enterprises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnterprises([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var enterprises = await _context.Enterprises.FindAsync(id);
            if (enterprises == null)
            {
                return NotFound();
            }

            _context.Enterprises.Remove(enterprises);
            await _context.SaveChangesAsync();

            return Ok(enterprises);
        }

        private bool EnterprisesExists(string id)
        {
            return _context.Enterprises.Any(e => e.Id == id);
        }
    }
}