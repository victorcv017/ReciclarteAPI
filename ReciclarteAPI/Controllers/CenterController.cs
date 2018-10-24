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
        
        [HttpGet]
        // GET: api/Center/Basic
        [HttpGet("{id}", Name = "GetBasicInfoCenter")]
        public ActionResult GetBasicInfo(long id)
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

            request.Add(location.City);
            
            request.Add(location.Township);
            request.Add(location.Street);
            request.Add(location.Number.ToString());
            request.Add(location.PC.ToString());

            


            return Json(new 
            {
                Schedule=request[0], 
                Materials=request[1],
                City=request[2],
                Township=request[3],
                Street=request[4],
                Number=request[5],
                PC=request[6]
            }, JsonRequestBehavior.AllowGet);
            
        }

        

        
       


       

       
    }
}