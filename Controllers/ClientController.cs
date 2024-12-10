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

            var vehicle = new Vehicle
            {
                VehicleId = models.VehicleId,
                LicensePlate = models.LicensePlate,
                Year = models.Year,
                Brand = models.Brand,
                Model = models.Model,
                ClientId = models.ClientId
            };

            await _vehicleDbStorage.UpdateVehicle(vehicle);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _vehicleDbStorage.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
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
