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
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }
        //create an enterprise and a center
        public ActionResult<string> Test()
        {

            //var enterprise = new Enterprises() { Name = "Empresa 1" , Email = "empresa1@gmail.com" };
            var dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68100, Number = 10, Street = "Lazaro Cardenas" };
            //var center = new Centers() { Name = "Centro 1", Schedule =  @"{ ""L"" : ""13:00-18:00"" , ""M"" : ""14:00-13:00""}", Address = dir };
            //_context.Add(enterprise);
            //_context.Add(dir);
            //_context.Add(center);
            //_context.SaveChanges();
            var enterprise = _context.Enterprises.Find("5a0020f6-e65a-4e3d-87fb-b81c9656654a");
            //var office = new Offices() { Name = "Sucursal 2", Schedule = @"{ ""L"" : ""15:00-18:00"" , ""M"" : ""12:00-13:00""}", Point = "15.2,59.99", Enterprise = enterprise, Address = dir };
            //_context.Add(office);
            //_context.Add(dir);
            //_context.SaveChanges();
            return Ok(enterprise);
        }
    }
}