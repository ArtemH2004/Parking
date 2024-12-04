using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.Models;
using Parking.Services;
using System.Security.Claims;

namespace Parking.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly VehicleDbStorage _vehicleDbStorage;

        public VehicleController(VehicleDbStorage vehicleDbStorage)
        {
            _vehicleDbStorage = vehicleDbStorage;
        }

        public async Task<IActionResult> Index()
        {
            // Получаем все автомобили
            var vehicles = await _vehicleDbStorage.GetAllVehicles();
            return View(vehicles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel model)
        {
            if (ModelState.IsValid)
            {              
                var vehicle = new Vehicle
                {
                    LicensePlate = model.LicensePlate,
                    Year = model.Year,
                    Brand = model.Brand,
                    Model = model.Model,
                    ClientId = model.ClientId,
                };
                await _vehicleDbStorage.AddVehicle(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
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
                Model = vehicle.Model
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vehicle = new Vehicle
                {
                    VehicleId = model.VehicleId,
                    LicensePlate = model.LicensePlate,
                    Year = model.Year,
                    Brand = model.Brand,
                    Model = model.Model,
                    ClientId = model.ClientId,
                };
                await _vehicleDbStorage.UpdateVehicle(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _vehicleDbStorage.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleDbStorage.DeleteVehicle(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
