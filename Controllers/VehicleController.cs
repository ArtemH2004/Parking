using Microsoft.AspNetCore.Mvc;
using Parking.Data;
using Parking.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parking.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (client == null)
            {
                return NotFound("Клиент не найден.");
            }

            var viewModel = new VehicleViewModel { ClientId = client.ClientId };
            return PartialView("_VehicleForm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VehicleViewModel model)
         {
            Debug.WriteLine($"Brand: {model.Brand}, Model: {model.Model}, LicensePlate: {model.LicensePlate}, Year: {model.Year}");
            if (ModelState.IsValid)
            {
                var vehicle = new Vehicle
                {
                    LicensePlate = model.LicensePlate,
                    Year = model.Year,
                    Brand = model.Brand,
                    Model = model.Model,
                    ClientId = model.ClientId
                };

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToArray();
                return Json(new { success = false, error = errors });
            }


            return Json(new { success = false, error = "Некоторые поля формы недостаточно валидны." });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var viewModel = new VehicleViewModel
            {
                VehicleId = vehicle.VehicleId,
                LicensePlate = vehicle.LicensePlate,
                Year = vehicle.Year,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ClientId = vehicle.ClientId
            };

            return PartialView("_VehicleForm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
                if (vehicle != null)
                {
                    vehicle.LicensePlate = model.LicensePlate;
                    vehicle.Year = model.Year;
                    vehicle.Brand = model.Brand;
                    vehicle.Model = model.Model;

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Client");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Client");
        }
    }
}
