using Microsoft.AspNetCore.Mvc;
using Parking.Data;
using Parking.Models;
using System.Linq;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }


    }
}

