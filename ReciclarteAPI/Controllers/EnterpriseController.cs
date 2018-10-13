using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReciclarteAPI.Models;

namespace ReciclarteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnterpriseController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.Enterprises.Count() == 0)
            {
                _context.Enterprises.Add(new Enterprise { Name = "Enterprise1" });
                _context.SaveChanges();
            }
        }

        // GET: api/Enterprise
        [HttpGet]
        public ActionResult<List<Enterprise>> GetAll()
        {
            return _context.Enterprises.ToList();
        }

        // GET: api/Enterprise/5
        [HttpGet("{id}", Name = "GetEnterprise")]
        public ActionResult<Enterprise> GetById(long id)
        {
            var item = _context.Enterprises.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        // POST: api/Enterprise
        [HttpPost]
        public IActionResult Create(Enterprise item)
        {
            _context.Enterprises.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetEnterprise", new { id = item.Id }, item);
        }

        // PUT: api/Enterprise/5
        [HttpPut("{id}")]
        public IActionResult Update(long id, Enterprise item)
        {
            var enterprise = _context.Enterprises.Find(id);
            if (enterprise == null)
            {
                return NotFound();
            }
            
            enterprise.Name = item.Name;
            enterprise.Email = item.Email;
            enterprise.Password = item.Password;
            enterprise.Balance = item.Balance;

            _context.Enterprises.Update(enterprise);
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.Enterprises.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Enterprises.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
