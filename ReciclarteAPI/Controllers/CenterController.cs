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
    public class CenterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        

        // GET: api/Center/Basic
        [HttpGet("{id}", Name = "GetBasicInfoCenter")]
        public ActionResult<List<string>> GetBasicInfo(long id)
        {

            var request = new List<string>();
            var center = _context.Centers.Find(id);
            if (center == null)
            {
                return NotFound();
            }

           request.Add(center.Schedule); 
            var Mats = _context.Materials.Find(_context.MaterialPerCenters.Find(id).MaterialId).Material;

            request.Add(Mats);
            

            var location = _context.Addresses.Find(center.AddressId);

            request.Add(_context.Addresses.Find(center.AddressId).City);
            request.Add(_context.Addresses.Find(center.AddressId).Township);
            request.Add(_context.Addresses.Find(center.AddressId).Street);
            request.Add(_context.Addresses.Find(center.AddressId).Number.ToString());
            request.Add(_context.Addresses.Find(center.AddressId).PC.ToString());

            


            return request;
            
        }

        

        
       


       

       
    }
}