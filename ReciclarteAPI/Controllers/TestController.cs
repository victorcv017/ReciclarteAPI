using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Test()
        {
           //Create an enterprise and a user
            var dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68100, Number = 10, Street = "Lazaro Cardenas" };
            _context.Add(dir);
            var user = new Users { Name = "Usuario", Surname = "Apelidos", UserName = "usuario@gmail.com", Email = "usuario@gmail.com", Curp = "BADD110313HCMLNS09", Address = dir };
            var enterprise = new Enterprises() { UserName = "empresa1@gmail.com", Name = "Empresa 1", Email = "empresa1@gmail.com" , Logo = "https://d500.epimg.net/cincodias/imagenes/2015/05/08/pyme/1431098283_691735_1431098420_noticia_normal.jpg" };
            var enterprise2 = new Enterprises() { UserName = "empresa2@gmail.com", Name = "Empresa 2", Email = "empresa2@gmail.com" , Logo = "https://image.freepik.com/psd-gratis/empresa-comunicacion-vector-logo-plantilla_63-2568.jpg"};
            await _userManager.CreateAsync(user, "Aa12345SDFUS.6!!");
            await _userManager.CreateAsync(enterprise, "Aa12345SDFENTER.6!!");
            await _userManager.CreateAsync(enterprise2, "Aa12345SDFENTER2.6!!");
            int i;
            var office = new Offices();
            for (i = 0; i < 10; i++)
            {
                dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68100 + i, Number = i, Street = "Lazaro Cardenas numero " + i };
                office = new Offices { Address = dir, Enterprise = enterprise, Point = new Point() { Lat = 70 + i , Long = 80 + i}, Schedule = new Schedule() { Lu = "13:00-18:00" , Ma = "14:00-13:00"} };
                _context.Add(office);
            }
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68300, Number = 1, Street = "Murgia numero " + i };
            var center = new Centers() { Name = "Centro 1", Schedule = new Schedule() { Lu = "13:00-18:00", Ma = "14:00-13:00" } , Address = dir, Point = new Point() { Lat = 12.58, Long = 87.59 } };
            for (; i < 20 ; i++)
            {
                dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68100 + i, Number = i, Street = "Lazaro Cardenas numero " + i };
                office = new Offices { Address = dir, Enterprise = enterprise2, Point = new Point() { Lat = 90 + i, Long = 100 + i }, Schedule = new Schedule() { Lu = "9:00-18:00", Ma = "10:00-13:00" }  };
                _context.Add(office);
            }
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68400, Number = 2, Street = "Alcala numero " + i };
            var center2 = new Centers() { Name = "Centro 2", Schedule = new Schedule() { Lu = "11:00-18:00", Ma = "12:00-19:00" } , Address = dir , Point = new Point() { Lat = 15.44, Long = 18.55 } };
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

            //-----------Purchases
            var item = new Items();
            for (i = 1; i <= 5; i++)
            {
                item = new Items() { Name = "Item " + i, Value = i, Office = office };
                _context.Items.Add(item);
            }
            _context.SaveChanges();
            var transaction = new Transactions() { Date = DateTime.Now, User = user };
            _context.Transactions.Add(transaction);
            double sum = 0;
            for (i = 1; i <= 5; i++)
            {
                item = _context.Items.Find((long)i);
                var purchase = new Purchases() { Quantity = 1, Item = item, Transaction = transaction };
                sum += i;
                _context.Purchases.Add(purchase);
            }
            transaction.Amount = sum;
            _context.SaveChanges();

            //Sales
            transaction = new Transactions() { Date = DateTime.Now, User = user };
            _context.Transactions.Add(transaction);
            var sal = new Sales() { Weight = 1, Transaction = transaction, Center = _context.Centers.Find((long)1), Material = _context.Materials.Find((long)1) };
            sal.Transaction = transaction;
            transaction.Amount = 1;
            _context.Sales.Add(sal);
            _context.SaveChanges();

=======
            return Ok();
            
        }
    }
}