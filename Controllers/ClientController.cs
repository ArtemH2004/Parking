using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.Data;
using Parking.Models;
using Parking.Services;
using System.Security.Claims;

namespace Parking.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly VehicleDbStorage _vehicleDbStorage;

        public ClientController(ApplicationDbContext context, VehicleDbStorage vehicleDbStorage)
        {
            _context = context;
            _vehicleDbStorage = vehicleDbStorage;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _context.Clients
                .Include(c => c.Vehicles)
                .Include(c => c.Contracts)
                .ThenInclude(c => c.ParkingLot)
                    .ThenInclude(pl => pl.Drivers)
                .Include(c => c.Contracts)
                    .ThenInclude(c => c.ParkingLot)
                        .ThenInclude(pl => pl.Guards)
                .FirstOrDefault(c => c.ApplicationUserId == userId);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAvatar(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

                if (client != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        client.Photo = memoryStream.ToArray();
                    }

                    _context.Clients.Update(client);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateVehicle()
        {
            var model = new VehicleViewModel { ClientId = GetCurrentClientId() };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicle(VehicleViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var vehicle = new Vehicle
            {
                LicensePlate = models.LicensePlate,
                Year = models.Year,
                Brand = models.Brand,
                Model = models.Model,
                ClientId = models.ClientId
            };

            await _vehicleDbStorage.AddVehicle(vehicle);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditVehicle(int id)
        {
            var vehicle = await _vehicleDbStorage.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var model = new VehicleViewModel
            {
                VehicleId = vehicle.VehicleId,
                LicensePlate = vehicle.LicensePlate,
                Year = vehicle.Year,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ClientId = vehicle.ClientId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicle(VehicleViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var vehicle = await _vehicleDbStorage.GetVehicleById(models.VehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            vehicle.LicensePlate = models.LicensePlate;
            vehicle.Year = models.Year;
            vehicle.Brand = models.Brand;
            vehicle.Model = models.Model;
            vehicle.ClientId = models.ClientId;

            if (models.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await models.Photo.CopyToAsync(memoryStream);
                    vehicle.Photo = memoryStream.ToArray();
                }
            }

            await _vehicleDbStorage.UpdateVehicle(vehicle);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("DeleteVehicle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicleConfirmed(int id)
        {
            await _vehicleDbStorage.DeleteVehicle(id);
            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentClientId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);
            return client?.ClientId ?? 0;
        }
    }

}
