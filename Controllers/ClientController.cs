using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ContractDbStorage _contractDbStorage;

        public ClientController(ApplicationDbContext context, VehicleDbStorage vehicleDbStorage, ContractDbStorage contractDbStorage)
        {
            _context = context;
            _vehicleDbStorage = vehicleDbStorage;
            _contractDbStorage = contractDbStorage;
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

            if (models.Photo != null && models.Photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await models.Photo.CopyToAsync(memoryStream);
                    vehicle.Photo = memoryStream.ToArray();
                }
            }

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

        public async Task<IActionResult> ParkingLots()
        {
            var parkingLots = await _context.ParkingLots.ToListAsync();
            return View(parkingLots);
        }

        //public async Task<IActionResult> RentParking(int id)
        //{
        //    var parkingLot = await _context.ParkingLots
        //        .Include(pl => pl.Drivers)
        //        .Include(pl => pl.Guards)
        //        .FirstOrDefaultAsync(pl => pl.ParkingLotId == id);

        //    if (parkingLot == null)
        //    {
        //        return NotFound();
        //    }

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var client = await _context.Clients
        //        .Include(c => c.Vehicles)
        //        .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

        //    if (client == null)
        //    {
        //        return NotFound();
        //    }

        //    if (client.Vehicles == null || !client.Vehicles.Any())
        //    {
        //        ModelState.AddModelError("", "У вас нет автомобилей для аренды.");
        //        return View(new RentalViewModel { ParkingLotId = id });
        //    }

        //    var model = new RentalViewModel
        //    {
        //        ParkingLotId = id,
        //        Vehicles = client.Vehicles.Select(v => new SelectListItem
        //        {
        //            Value = v.VehicleId.ToString(),
        //            Text = $"{v.Brand} {v.Model} ({v.LicensePlate})"
        //        }).ToList(),
        //        StartTime = DateTime.Now,
        //        EndTime = DateTime.Now,
        //        Amount = 0,
        //        DriverId = GetRandomDriver(parkingLot.Drivers),
        //        GuardId = GetRandomGuard(parkingLot.Guards),
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RentParking(RentalViewModel models)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(models);
        //    }

        //    var rentalDuration = models.EndTime - models.StartTime;
        //    var price = CalculatePrice(rentalDuration); 

        //    var rentalContract = new Contract
        //    {
        //        ParkingLotId = models.ParkingLotId,
        //        VehicleId = models.VehicleId,
        //        StartDate = models.StartTime,
        //        EndDate = models.EndTime,
        //        Amount = price,
        //        DriverId = models.DriverId,
        //        GuardId = models.GuardId
        //    };

        //    await _contractDbStorage.AddContract(rentalContract);

        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> RentParking(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await _context.Clients
                .Include(c => c.Vehicles)
                .Include(c => c.Contracts)
                    .ThenInclude(c => c.ParkingLot)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (client == null || (client.Vehicles == null || !client.Vehicles.Any()))
            {
                ModelState.AddModelError("", "У вас нет автомобилей для аренды.");
                return View(new ContractViewModel { ParkingLotId = id });
            }

            var parkingLot = await _context.ParkingLots
                .Include(pl => pl.Drivers)
                .Include(pl => pl.Guards)
                .FirstOrDefaultAsync(pl => pl.ParkingLotId == id);

            if (parkingLot == null)
            {
                return NotFound();
            }

            var model = new ContractViewModel
            {
                ParkingLotId = id,
                ClientId = client.ClientId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                Amount = CalculatePrice(DateTime.Now.AddHours(1) - DateTime.Now),
                Vehicles = client.Vehicles,
                DriverId = GetRandomDriver(parkingLot.Drivers),
                GuardId = GetRandomGuard(parkingLot.Guards),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentParking(ContractViewModel models)
        {
            if (!ModelState.IsValid || models.EndDate <= models.StartDate)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                    return View(models);
            }

            var contract = new Contract
            {
                StartDate = models.StartDate,
                EndDate = models.EndDate,
                Amount = CalculatePrice(models.EndDate - models.StartDate),
                ParkingLotId = models.ParkingLotId,
                VehicleId = models.VehicleId,
                ClientId = models.ClientId,
                DriverId = models.DriverId,
                GuardId = models.GuardId
            };

            try
            {
                await _contractDbStorage.AddContract(contract);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка при создании контракта: " + ex.Message);
                return View(models);
            }

            return RedirectToAction(nameof(Index));
        }


        private int GetRandomDriver(IEnumerable<Driver> drivers)
        {
            var random = new Random();
            return drivers.ElementAt(random.Next(drivers.Count())).DriverId;
        }

        private int GetRandomGuard(IEnumerable<Guard> guards)
        {
            var random = new Random();
            return guards.ElementAt(random.Next(guards.Count())).GuardId;
        }

        private decimal CalculatePrice(TimeSpan duration)
        {
            return (decimal)duration.TotalHours * 100;
        }

        private int GetCurrentClientId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);
            return client?.ClientId ?? 0;
        }
    }

}
