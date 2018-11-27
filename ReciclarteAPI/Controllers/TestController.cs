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
            /*
            var dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68070, Number = 10, Street = "Azucenas" };
            var user = new Users { Name = "Jose Antonio", Surname = "Hernández Hernández", UserName = "hernandunogames@gmail.com", Email = "hernandunogames@gmail.com", Curp = "HEHA960319HOCRRN02", Address = dir };
            user.Signature = user.Id;
            var oxxo = new Enterprises() { UserName = "contacto@oxxo.com", Name = "Oxxo", Email = "contacto@oxxo.com", Logo = "https://upload.wikimedia.org/wikipedia/commons/6/66/Oxxo_Logo.svg" };
            var chedraui = new Enterprises() { UserName = "contacto@chedraui.com", Name = "Chedraui", Email = "contacto@chedraui.com", Logo = "https://seeklogo.com/images/C/Chedraui-logo-5C8594E079-seeklogo.com.png" };
            var aurrera = new Enterprises() { UserName = "contacto@aurrera.com", Name = "Bodega Aurrera", Email = "contacto@aurrera.com", Logo = "http://techpad.mx/wp/wp-content/uploads/2017/07/Bodega-aurrera.png" };
            await _userManager.CreateAsync(user, "Aa12345SDFHDZ.6!!");
            await _userManager.CreateAsync(oxxo, "Aa12345SDFXXO.6!!");
            await _userManager.CreateAsync(chedraui, "Aa12345SDFCHED.6!!");
            await _userManager.CreateAsync(aurrera, "Aa12345SDFAURRE.6!!");

            //---TRANSACTIONS
            var transaction = new Transactions() { Date = DateTime.Now, User = user };
            _context.Transactions.Add(transaction);
            double sum = 0;

            //-----------OXXO-----------------
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68083, Number = 322, Street ="5 Privada"};
            var office = new Offices { UserName = "oxxo1@oxxo.com", Email = "oxxo1@oxxo.com", Address = dir, Enterprise = oxxo, Point = new Point() { Lat = 17.052563, Long = -96.720312 }, Schedule = new Schedule() { Lu = "07:00–23:00", Ma = "07:00–23:00" , Mi = "07:00–23:00", Ju = "07:00–23:00", Vi= "07:00–23:00", Sa= "07:00–23:00", Do= "07:00–23:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFXXO.6!!");
            var item = new Items() { Name = "Aceite La Posada" , Value = 21, Office = office };
            item = new Items() { Name = "Azúcar Zulka" , Value = 17, Office = office };
            _context.Add(item);
            var purchase = new Purchases() { Quantity = 1, Item = item, Transaction = transaction };
            sum += 17;
            _context.Purchases.Add(purchase);
            item = new Items() { Name = "Detergente Ace" , Value = 25, Office = office };
            _context.Add(item);
            purchase = new Purchases() { Quantity = 1, Item = item, Transaction = transaction };
            sum += 25;
            _context.Purchases.Add(purchase);
            transaction.Amount = sum;
            _context.SaveChanges();


            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68120, Number = 101, Street = "Prol.De La Noria" };
            office = new Offices { UserName = "oxxo2@oxxo.com", Email = "oxxo2@oxxo.com", Address = dir, Enterprise = oxxo, Point = new Point() { Lat = 17.054187, Long = -96.712938 }, Schedule = new Schedule() { Lu = "07:00–23:00", Ma = "07:00–23:00", Mi = "07:00–23:00", Ju = "07:00–23:00", Vi = "07:00–23:00", Sa = "07:00–23:00", Do = "07:00–23:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFXXO.6!!");
            item = new Items() { Name = "Suavel Jumbo", Value = 29, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Skittles", Value = 25, Office = office };
            _context.Add(item);

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68090, Number = 3600, Street = "Artículo 123" };
            office = new Offices { UserName = "oxxo3@oxxo.com", Email = "oxxo3@oxxo.com", Address = dir, Enterprise = oxxo, Point = new Point() { Lat = 17.058063, Long = -96.712562 }, Schedule = new Schedule() { Lu = "07:00–23:00", Ma = "07:00–23:00", Mi = "07:00–23:00", Ju = "07:00–23:00", Vi = "07:00–23:00", Sa = "07:00–23:00", Do = "07:00–23:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFXXO.6!!");
            item = new Items() { Name = "Andatti Capuchino Chico", Value = 20, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Suavel Jumbo", Value = 29, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Detergente Ace", Value = 25, Office = office };
            _context.Add(item);

           
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 71233, Number = 400, Street = "Avenida Universidad" };
            office = new Offices { UserName = "oxxo4@oxxo.com", Email = "oxxo4@oxxo.com", Address = dir, Enterprise = oxxo, Point = new Point() { Lat = 17.058063, Long = -96.712562 }, Schedule = new Schedule() { Lu = "07:00–23:00", Ma = "07:00–23:00", Mi = "07:00–23:00", Ju = "07:00–23:00", Vi = "07:00–23:00", Sa = "07:00–23:00", Do = "07:00–23:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFXXO.6!!");
            item = new Items() { Name = "Andatti Capuchino Chico", Value = 20, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Andatti Capuchino Mediano", Value = 29, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Andatti Capuchino Grande", Value = 34, Office = office };
            _context.Add(item);

            //-------CHEDRAUI------
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68120, Number = 300, Street = "Periférico" };
            office = new Offices { UserName = "chedraui1@chedraui.com", Email = "chedraui1@chedraui.com", Address = dir, Enterprise = chedraui, Point = new Point() { Lat = 17.052438, Long = -96.717188 }, Schedule = new Schedule() { Lu = "07:00–23:00", Ma = "07:00–23:00", Mi = "07:00–23:00", Ju = "07:00–23:00", Vi = "07:00–23:00", Sa = "07:00–23:00", Do = "07:00–23:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFCHED.6!!");
            item = new Items() { Name = "Lavatrastres Salvo Polvo Limón 250 gr", Value = 9.90, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Limpiador Flash Lavanda Botella elimina", Value = 5.50, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Limpiador Multiusos Fabuloso Complete Pi", Value = 8, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Pastilla Chedraui Lavanda 70Grs", Value = 6.90, Office = office };
            _context.Add(item);

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 71230, Number = 215, Street = "Guadalupe Hinojosa de Murat" };
            office = new Offices { UserName = "chedraui2@chedraui.com", Email = "chedraui2@chedraui.com", Address = dir, Enterprise = chedraui, Point = new Point() { Lat = 17.025187, Long = -96.728812 }, Schedule = new Schedule() { Lu = "08:00–23:00", Ma = "08:00–23:00", Mi = "08:00–23:00", Ju = "08:00–23:00", Vi = "08:00–23:00", Sa = "08:00–23:00", Do = "08:00–23:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFCHED.6!!");
            item = new Items() { Name = "Jerga Fina Home Line 179 43X58", Value = 8.90, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Acido Muriatico La Anita 500Ml", Value = 9.05, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Pastilla Arom Dorosol Varios 70 Grs", Value = 9.30, Office = office };
            _context.Add(item);

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68050, Number = 917, Street = "Heroica Escuela Naval Militar" };
            office = new Offices { UserName = "chedraui3@chedraui.com", Email = "chedraui3@chedraui.com", Address = dir, Enterprise = chedraui, Point = new Point() { Lat = 17.078313, Long = -96.708688 }, Schedule = new Schedule() { Lu = "07:00–23:00", Ma = "07:00–23:00", Mi = "07:00–23:00", Ju = "07:00–23:00", Vi = "07:00–23:00", Sa = "07:00–23:00", Do = "07:00–23:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFCHED.6!!");
            item = new Items() { Name = "Pastilla Para Baño dorosol Floral 1pza 7", Value = 9.30, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Cera Liq Nugget 60ml, Blanco", Value = 9.50, Office = office };
            _context.Add(item);

            //---AURRERA
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 71230, Number = 162, Street = "Oaxaca-Zimatlán De Álvarez" };
            office = new Offices { UserName = "aurrera1@bodegaaurrera.com", Email = "aurrera1@bodegaaurrera.com", Address = dir, Enterprise = aurrera, Point = new Point() { Lat = 17.045188, Long = -96.730313 }, Schedule = new Schedule() { Lu = "08:00–22:00", Ma = "08:00–22:00", Mi = "08:00–22:00", Ju = "08:00–22:00", Vi = "08:00–22:00", Sa = "08:00–22:00", Do = "08:00–22:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFAURRE.6!!");
            item = new Items() { Name = "Detergente líquido Persil", Value = 25, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Ariel en polvo", Value = 25, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Pinol lavanda", Value = 25, Office = office };
            _context.Add(item);

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68050, Number = 1010, Street = "Naranjos" };
            office = new Offices { UserName = "aurrera2@bodegaaurrera.com", Email = "aurrera2@bodegaaurrera.com", Address = dir, Enterprise = aurrera, Point = new Point() { Lat = 17.080063, Long = -96.710687 }, Schedule = new Schedule() { Lu = "08:00–22:00", Ma = "08:00–22:00", Mi = "08:00–22:00", Ju = "08:00–22:00", Vi = "08:00–22:00", Sa = "08:00–22:00", Do = "08:00–22:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFAURRE.6!!");
            item = new Items() { Name = "Maizena Nuez", Value = 6.30, Office = office };
            _context.Add(item);
            item = new Items() { Name = "Galletas Ritz paquete", Value = 8, Office = office };
            _context.Add(item);


            //---------CENTROS
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68276, Number = 100, Street = "Sabinos" };
            var center = new Centers() {UserName = "reciclaoax@reciclapapel.com", Email = "reciclaoax@reciclapapel.com", Name = "Recicla Papel Oaxaca", Schedule = new Schedule() { Lu = "08:00–18:00", Ma = "08:00–18:00", Mi = "08:00–18:00", Ju = "08:00–18:00", Vi = "08:00–18:00", Sa = "08:00–18:00", Do = "Cerrado" } , Address = dir, Point = new Point() { Lat = 17.050562, Long = -96.713812 } };
            await _userManager.CreateAsync(center, "Aa12345SDFCENT.6!!");
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68740, Number = 1, Street = "San Rafael Lote 2" };
            var center2 = new Centers() { UserName = "reciclaje@corecicladora.com", Email = "reciclaje@corecicladora.com", Name = "Comercial Recicladora", Schedule = new Schedule() { Lu = "08:00–18:00", Ma = "08:00–18:00", Mi = "08:00–18:00", Ju = "08:00–18:00", Vi = "08:00–18:00", Sa = "08:00–18:00", Do = "Cerrado" }, Address = dir, Point = new Point() { Lat = 17.050562, Long = -96.713812 } };
            await _userManager.CreateAsync(center2, "Aa12345SDFCENT2.6!!");
            var material1 = new Materials() { Material = "Plástico", Price = 10 };
            var material2 = new Materials() { Material = "Cartón", Price = 9 };
            var material3 = new Materials() { Material = "Papel", Price = 5 };

            var mc = new MaterialsPerCenter() { Center = center, Material = material3 };
            _context.MaterialsPerCenter.Add(mc);

            mc = new MaterialsPerCenter() { Center = center2, Material = material1 };
            _context.MaterialsPerCenter.Add(mc);
            mc = new MaterialsPerCenter() { Center = center2, Material = material2 };
            _context.MaterialsPerCenter.Add(mc);
            mc = new MaterialsPerCenter() { Center = center2, Material = material3 };
            _context.MaterialsPerCenter.Add(mc);

            transaction = new Transactions() { Date = DateTime.Now, User = user };
            _context.Transactions.Add(transaction);
            var sal = new Sales() { Weight = 1, Transaction = transaction, Center = center, Material =  material3};
            sal.Transaction = transaction;
            transaction.Amount = 5;
            _context.Sales.Add(sal);
            _context.SaveChanges();

            return Ok();
            */

            var dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68030, Number = 125, Street = "Francisco I. Madero" };
            var center = new Centers() { UserName = "reciclarte@ito.com", Email = "reciclarte@ito.com", Name = "Instituto Tecnológico de Oaxaca", Schedule = new Schedule() { Lu = "08:00–20:00", Ma = "08:00–20:00", Mi = "08:00–20:00", Ju = "08:00–20:00", Vi = "08:00–20:00", Sa = "08:00–20:00", Do = "Cerrado" }, Address = dir, Point = new Point() { Lat = 17.077313, Long = -96.744437 } };
            await _userManager.CreateAsync(center, "Aa12345SDFCENT.6!!");

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68000, Number = 1, Street = "Macedonio Alcalá" };
            center = new Centers() { UserName = "reciclarte@santodomingo.com", Email = "reciclarte@santodomingo.com", Name = "Templo de Santo Domingo de Guzmán", Schedule = new Schedule() { Lu = "08:00–14:00", Ma = "08:00–14:00", Mi = "08:00–14:00", Ju = "08:00–14:00", Vi = "08:00–14:00", Sa = "08:00–14:00", Do = "08:00–14:00" }, Address = dir, Point = new Point() { Lat = 17.065688, Long = -96.723188 } };
            await _userManager.CreateAsync(center, "Aa12345SDFCENT.6!!");

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68000, Number = 1, Street = "Portal del Palacio" };
            center = new Centers() { UserName = "reciclarte@zocalo.com", Email = "reciclarte@zocalo.com", Name = "Zócalo Oaxaca", Schedule = new Schedule() { Lu = "8:00–16:00", Ma = "8:00–16:00", Mi = "8:00–16:00", Ju = "8:00–16:00", Vi = "8:00–16:00", Sa = "8:00–16:00", Do = "8:00–16:00" }, Address = dir, Point = new Point() { Lat = 17.060563, Long = -96.725313 } };
            await _userManager.CreateAsync(center, "Aa12345SDFCENT.6!!");

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68000, Number = 1, Street = "Portal del Palacio" };
            center = new Centers() { UserName = "reciclarte@zocalo.com", Email = "reciclarte@zocalo.com", Name = "Zócalo Oaxaca", Schedule = new Schedule() { Lu = "8:00–16:00", Ma = "8:00–16:00", Mi = "8:00–16:00", Ju = "8:00–16:00", Vi = "8:00–16:00", Sa = "8:00–16:00", Do = "8:00–16:00" }, Address = dir, Point = new Point() { Lat = 17.060563, Long = -96.725313 } };
            await _userManager.CreateAsync(center, "Aa12345SDFCENT.6!!");

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68000, Number = 1, Street = "Portal del Palacio" };
            center = new Centers() { UserName = "reciclarte@zocalo.com", Email = "reciclarte@zocalo.com", Name = "Zócalo Oaxaca", Schedule = new Schedule() { Lu = "8:00–16:00", Ma = "8:00–16:00", Mi = "8:00–16:00", Ju = "8:00–16:00", Vi = "8:00–16:00", Sa = "8:00–16:00", Do = "8:00–16:00" }, Address = dir, Point = new Point() { Lat = 17.060563, Long = -96.725313 } };
            await _userManager.CreateAsync(center, "Aa12345SDFCENT.6!!");


            var proveedora = new Enterprises() { UserName = "contacto@proveedora.com", Name = "Proveedora Escolar", Email = "contacto@proveedora.com", Logo = "https://scontent.fpbc2-1.fna.fbcdn.net/v/t1.0-9/13697252_1034012846636070_2010601010670017102_n.png?_nc_cat=105&_nc_ht=scontent.fpbc2-1.fna&oh=e8bab7478d7783dd626b41d1125880b3&oe=5CB0DDB0" };
            await _userManager.CreateAsync(proveedora, "Aa12345SDFCENT.6!!");

            var asun = new Enterprises() { UserName = "contacto@asuncion.com", Name = "La Asunción", Email = "contacto@asuncion.com", Logo = "http://asuncion.com.mx/img/la-asuncion-ferreteria-y-madereria-logo-1444064293.jpg" };
            await _userManager.CreateAsync(asun, "Aa12345SDFCENT.6!!");

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68000, Number = 1001, Street = "Av. Independencia" };
            var office = new Offices { UserName = "proveedora1@proveedora.com", Email = "proveedora1@proveedora.com", Address = dir, Enterprise = proveedora, Point = new Point() { Lat = 17.061563, Long = -96.722438 }, Schedule = new Schedule() { Lu = "09:00–20:00", Ma = "09:00–20:00", Mi = "09:00–20:00", Ju = "09:00–20:00", Vi = "09:00–20:00", Sa = "09:00–20:00", Do = "09:00–16:00" } };
            await _userManager.CreateAsync(office, "Aa12345SDFAURRE.6!!");

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68000, Number = 101, Street = "Nicolás del Puerto " };
            office = new Offices { UserName = "proveedora2@proveedora.com", Email = "proveedora2@proveedora.com", Address = dir, Enterprise = proveedora, Point = new Point() { Lat = 17.060563, Long = -96.715938 }, Schedule = new Schedule() { Lu = "09:00–20:00", Ma = "09:00–20:00", Mi = "09:00–20:00", Ju = "09:00–20:00", Vi = "09:00–20:00", Sa = "09:00–20:00", Do = "Cerrado" } };
            await _userManager.CreateAsync(office, "Aa12345SDFAURRE.6!!");

            
            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 71228, Number = 1814, Street = "Internacional" };
            office = new Offices { UserName = "asuncion1@asuncion.com", Email = "asuncion1@asuncion.com", Address = dir, Enterprise = asun, Point = new Point() { Lat = 17.069199, Long = -96.700624 }, Schedule = new Schedule() { Lu = "08:00–19:00", Ma = "08:00–19:00", Mi = "08:00–19:00", Ju = "08:00–19:00", Vi = "08:00–19:00", Sa = "08:00–18:00", Do = "Cerrado" } };
            await _userManager.CreateAsync(office, "Aa12345SDFAURRE.6!!");

            dir = new Addresses() { City = "Oaxaca", Township = "Oaxaca de Juarez", PC = 68090, Number = 1, Street = "Libertad " };
            office = new Offices { UserName = "asuncion2@asuncion.com", Email = "asuncion2@asuncion.com", Address = dir, Enterprise = proveedora, Point = new Point() { Lat = 17.062688, Long =  -96.738063 }, Schedule = new Schedule() { Lu = "08:00–19:00", Ma = "08:00–19:00", Mi = "08:00–19:00", Ju = "08:00–19:00", Vi = "08:00–19:00", Sa = "08:00–19:00", Do = "Cerrado" } };
            await _userManager.CreateAsync(office, "Aa12345SDFAURRE.6!!");


            var oxxo = _context.Enterprises.FirstOrDefault(x => x.Email == "contacto@oxxo.com");
            oxxo.Logo = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/66/Oxxo_Logo.svg/1200px-Oxxo_Logo.svg.png";
            _context.SaveChanges();
            return Ok();
        }
    }
}