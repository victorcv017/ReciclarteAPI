using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReciclarteAPI.Models;

namespace ReciclarteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TestController(ApplicationDbContext context , UserManager<IdentityUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }
        //create an enterprise and a center
        public async Task<IActionResult> Test()
        {
            //Usuarios
            var dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68100, Number = 10, Street = "Lazaro Cardenas" };
            _context.Add(dir);
            var user = new Users { Name = "Usuario", Surname = "Apelidos", UserName = "usuario@gmail.com", Email = "usuario@gmail.com", Curp = "BADD110313HCMLNS09", Address = dir };
            var enterprise = new Enterprises() { UserName = "empresa1@gmail.com", Name = "Empresa 1", Email = "empresa1@gmail.com" };
            var enterprise2 = new Enterprises() { UserName = "empresa2@gmail.com", Name = "Empresa 2", Email = "empresa2@gmail.com" };
            await _userManager.CreateAsync(user, "Aa12345SDFUS.6!!");
            await _userManager.CreateAsync(enterprise, "Aa12345SDFENTER.6!!");
            await _userManager.CreateAsync(enterprise2, "Aa12345SDFENTER2.6!!");
            int i;
            for (i = 0; i < 10; i++)
            {
                dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68100 + i, Number = i, Street = "Lazaro Cardenas numero " + i };
                var office = new Offices { Address = dir, Enterprise = enterprise, Point = "" + (70 + i) + "," + (80 + i), Schedule = @"{ ""L"" : ""13:00-18:00"" , ""M"" : ""14:00-13:00""}" };
                _context.Add(office);
            }
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68300, Number = 1, Street = "Murgia numero " + i };
            var center = new Centers() { Name = "Centro 1", Schedule = @"{ ""L"" : ""13:00-18:00"" , ""M"" : ""14:00-13:00""}", Address = dir };
            for (; i < 20 ; i++)
            {
                dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68100 + i, Number = i, Street = "Lazaro Cardenas numero " + i };
                var office = new Offices { Address = dir, Enterprise = enterprise2, Point = "" + (90 + i) + "," + (100 + i), Schedule = @"{ ""L"" : ""9:00-18:00"" , ""M"" : ""10:00-13:00""}" };
                _context.Add(office);
            }
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68400, Number = 2, Street = "Alcala numero " + i };
            var center2 = new Centers() { Name = "Centro 2", Schedule =  @"{ ""L"" : ""11:00-18:00"" , ""M"" : ""18:00-13:00""}", Address = dir };
            var material1 = new Materials() { Material = "Plástico", Price = 10 };
            var material2 = new Materials() { Material = "Cartón", Price = 9 };
            var material3 = new Materials() { Material = "Papel", Price = 5 };
            
            var mc = new MaterialsPerCenter() { Center = center, Material = material1 };
            _context.MaterialsPerCenter.Add(mc);
            mc = new MaterialsPerCenter() { Center = center, Material = material2 };
            _context.MaterialsPerCenter.Add(mc);
            mc = new MaterialsPerCenter() { Center = center2, Material = material1 };
            _context.MaterialsPerCenter.Add(mc);
            mc = new MaterialsPerCenter() { Center = center2, Material = material2 };
            _context.MaterialsPerCenter.Add(mc);
            mc = new MaterialsPerCenter() { Center = center2, Material = material3 };
            _context.MaterialsPerCenter.Add(mc);
            _context.SaveChanges();
            return Ok();
        }
    }
}