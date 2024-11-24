//using Microsoft.AspNetCore.Mvc;
//using Parking.Data;
//using Parking.Models;
//using System.Linq;

//namespace Parking.Controllers
//{
//    public class ClientController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public ClientController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index()
//        {
//            var clients = _context.Clients.ToList();

//            if (clients == null || !clients.Any())
//            {
//                // Логирование, если список пустой
//                Console.WriteLine("Список клиентов пуст.");
//            }
//            else
//            {
//                Console.WriteLine($"Найдено {clients.Count} клиентов.");
//            }

//            return View(clients);
//        }

//    }
//}

using Microsoft.AspNetCore.Mvc;
using Parking.Data;
using Parking.Models;
using System.Linq;

namespace Parking.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clients = _context.Clients.ToList();

            if (clients == null || !clients.Any())
            {
                // Логирование, если список пустой
                Console.WriteLine("Список клиентов пуст.");
            }
            else
            {
                Console.WriteLine($"Найдено {clients.Count} клиентов:");
                foreach (var client in clients)
                {
                    // Выведите информацию о каждом клиенте в консоль
                    Console.WriteLine($"Клиент ID: {client.ClientId}, Имя: {client.FirstName}, Фамилия: {client.LastName}, Телефон: {client.Phone}, Адрес: {client.Address}");
                }
            }

            return View(clients);
        }
    }
}

