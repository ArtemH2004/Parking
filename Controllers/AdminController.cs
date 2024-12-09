using Parking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.Data;
using Parking.Services;
using Parking.DAL;

namespace Parking.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ParkingLotDbStorage _parkingLotDbStorage;
        private readonly VehicleDbStorage _vehicleDbStorage;
        private readonly ClientDbStorage _clientDbStorage; 
        private readonly DriverDbStorage _driverDbStorage;
        private readonly GuardDbStorage _guardDbStorage;
        private readonly ContractDbStorage _contractDbStorage;


        public AdminController(ApplicationDbContext context, VehicleDbStorage vehicleDbStorage, ClientDbStorage clientDbStorage, ParkingLotDbStorage parkingLotDbStorage, DriverDbStorage driverDbStorage, GuardDbStorage guardDbStorage, ContractDbStorage contractDbStorage)
        {
            _context = context;
            _parkingLotDbStorage = parkingLotDbStorage;
            _vehicleDbStorage = vehicleDbStorage;
            _clientDbStorage = clientDbStorage;
            _driverDbStorage = driverDbStorage;
            _guardDbStorage = guardDbStorage;   
            _contractDbStorage = contractDbStorage;
        }


        
        [Authorize(Roles = "Admin")]

        // ParkingLots
        public async Task<IActionResult> Index()
        {
            var parkingLots = await _parkingLotDbStorage.GetAllParkingLots();
            return View(parkingLots);
        }

        public IActionResult CreateParkingLot()
        {
            var model = new ParkingLotViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateParkingLot(ParkingLotViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var parkingLot = new ParkingLot
            {
                Name = models.Name,
                Address = models.Address,
                QuantitySpace = models.QuantitySpace
            };

            await _parkingLotDbStorage.AddParkingLot(parkingLot);
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> EditVehicle(int id)
        //{
        //    var vehicle = await _vehicleDbStorage.GetVehicleById(id);
        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VehicleViewModel
        //    {
        //        VehicleId = vehicle.VehicleId,
        //        LicensePlate = vehicle.LicensePlate,
        //        Year = vehicle.Year,
        //        Brand = vehicle.Brand,
        //        Model = vehicle.Model,
        //        ClientId = vehicle.ClientId
        //    };

        //    return View(model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditVehicle(VehicleViewModel models)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(models);
        //    }

        //    var vehicle = new Vehicle
        //    {
        //        VehicleId = models.VehicleId,
        //        LicensePlate = models.LicensePlate,
        //        Year = models.Year,
        //        Brand = models.Brand,
        //        Model = models.Model,
        //        ClientId = models.ClientId
        //    };

        //    await _vehicleDbStorage.UpdateVehicle(vehicle);
        //    return RedirectToAction(nameof(Vehicles));
        //}


        [HttpPost, ActionName("DeleteParkingLot")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteParkingLotConfirmed(int id)
        {
            await _parkingLotDbStorage.DeleteParkingLot(id);
            return RedirectToAction(nameof(Index));
        }


        // Clients
        public async Task<IActionResult> Clients()
        {
            var vehicles = await _clientDbStorage.GetAllUsers();
            return View(vehicles);
        }

        [HttpPost, ActionName("DeleteClient")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClientConfirmed(int id)
        {
            await _clientDbStorage.DeleteClient(id);
            return RedirectToAction(nameof(Clients));
        }


        // Vehicles
        public async Task<IActionResult> Vehicles()
        {
            var vehicles = await _vehicleDbStorage.GetAllVehicles();
            return View(vehicles);
        }

        public IActionResult CreateVehicle()
        {
            var model = new VehicleViewModel();
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
            return RedirectToAction(nameof(Vehicles));
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
            return RedirectToAction(nameof(Vehicles));
        }


        [HttpPost, ActionName("DeleteVehicle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicleConfirmed(int id)
        {
            await _vehicleDbStorage.DeleteVehicle(id);
            return RedirectToAction(nameof(Vehicles)); 
        }

        // Drivers
        public async Task<IActionResult> Drivers()
        {
            var drivers = await _driverDbStorage.GetAllDrivers();
            return View(drivers);
        }

        public IActionResult CreateDriver()
        {
            var model = new DriverViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDriver(DriverViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var driver = new Driver
            {
                LastName = models.LastName,
                FirstName = models.FirstName,
                MiddleName = models.MiddleName,
                Phone = models.Phone,
                ExperienceYears = models.ExperienceYears,
                Salary = models.Salary,
                OpenCategory = models.OpenCategory,
                ParkingLotId = models.ParkingLotId
            };

            await _driverDbStorage.AddDriver(driver);
            return RedirectToAction(nameof(Drivers));
        }

        //public async Task<IActionResult> EditVehicle(int id)
        //{
        //    var vehicle = await _vehicleDbStorage.GetVehicleById(id);
        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VehicleViewModel
        //    {
        //        VehicleId = vehicle.VehicleId,
        //        LicensePlate = vehicle.LicensePlate,
        //        Year = vehicle.Year,
        //        Brand = vehicle.Brand,
        //        Model = vehicle.Model,
        //        ClientId = vehicle.ClientId
        //    };

        //    return View(model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditVehicle(VehicleViewModel models)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(models);
        //    }

        //    var vehicle = new Vehicle
        //    {
        //        VehicleId = models.VehicleId,
        //        LicensePlate = models.LicensePlate,
        //        Year = models.Year,
        //        Brand = models.Brand,
        //        Model = models.Model,
        //        ClientId = models.ClientId
        //    };

        //    await _vehicleDbStorage.UpdateVehicle(vehicle);
        //    return RedirectToAction(nameof(Vehicles));
        //}


        [HttpPost, ActionName("DeleteDriver")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDriverConfirmed(int id)
        {
            await _driverDbStorage.DeleteDriver(id);
            return RedirectToAction(nameof(Drivers));
        }


        // Guards
        public async Task<IActionResult> Guards()
        {
            var guards = await _guardDbStorage.GetAllGuards();
            return View(guards);
        }

        public IActionResult CreateGuard()
        {
            var model = new GuardViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGuard(GuardViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var guard = new Guard
            {
                LastName = models.LastName,
                FirstName = models.FirstName,
                MiddleName = models.MiddleName,
                Phone = models.Phone,
                Address = models.Address,
                Salary = models.Salary,
                ParkingLotId = models.ParkingLotId
            };

            await _guardDbStorage.AddGuard(guard);
            return RedirectToAction(nameof(Guards));
        }

        //public async Task<IActionResult> EditVehicle(int id)
        //{
        //    var vehicle = await _vehicleDbStorage.GetVehicleById(id);
        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VehicleViewModel
        //    {
        //        VehicleId = vehicle.VehicleId,
        //        LicensePlate = vehicle.LicensePlate,
        //        Year = vehicle.Year,
        //        Brand = vehicle.Brand,
        //        Model = vehicle.Model,
        //        ClientId = vehicle.ClientId
        //    };

        //    return View(model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditVehicle(VehicleViewModel models)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(models);
        //    }

        //    var vehicle = new Vehicle
        //    {
        //        VehicleId = models.VehicleId,
        //        LicensePlate = models.LicensePlate,
        //        Year = models.Year,
        //        Brand = models.Brand,
        //        Model = models.Model,
        //        ClientId = models.ClientId
        //    };

        //    await _vehicleDbStorage.UpdateVehicle(vehicle);
        //    return RedirectToAction(nameof(Vehicles));
        //}


        [HttpPost, ActionName("DeleteGuard")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGuardConfirmed(int id)
        {
            await _guardDbStorage.DeleteGuard(id);
            return RedirectToAction(nameof(Guards));
        }

        // Contracts
        public async Task<IActionResult> Contracts()
        {
            var contracts = await _contractDbStorage.GetAllContracts();
            return View(contracts);
        }

        public IActionResult CreateContract()
        {
            var model = new ContractViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContract(ContractViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var contract = new Contract
            {
                StartDate = DateTime.Now,
                EndDate = models.EndDate,
                Amount = models.Amount,
                ParkingLotId = models.ParkingLotId > 0 ? models.ParkingLotId : 1,
                VehicleId = models.VehicleId > 0 ? models.VehicleId : 1,
                ClientId = models.ClientId > 0 ? models.ClientId : 2,
                DriverId = models.DriverId > 0 ? models.DriverId : 2,
                GuardId = models.GuardId > 0 ? models.GuardId : 1
            };

            await _contractDbStorage.AddContract(contract);
            return RedirectToAction(nameof(Contracts));
        }

        //public async Task<IActionResult> EditVehicle(int id)
        //{
        //    var vehicle = await _vehicleDbStorage.GetVehicleById(id);
        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VehicleViewModel
        //    {
        //        VehicleId = vehicle.VehicleId,
        //        LicensePlate = vehicle.LicensePlate,
        //        Year = vehicle.Year,
        //        Brand = vehicle.Brand,
        //        Model = vehicle.Model,
        //        ClientId = vehicle.ClientId
        //    };

        //    return View(model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditVehicle(VehicleViewModel models)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(models);
        //    }

        //    var vehicle = new Vehicle
        //    {
        //        VehicleId = models.VehicleId,
        //        LicensePlate = models.LicensePlate,
        //        Year = models.Year,
        //        Brand = models.Brand,
        //        Model = models.Model,
        //        ClientId = models.ClientId
        //    };

        //    await _vehicleDbStorage.UpdateVehicle(vehicle);
        //    return RedirectToAction(nameof(Vehicles));
        //}


        [HttpPost, ActionName("DeleteContract")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContractConfirmed(int id)
        {
            await _contractDbStorage.DeleteContract(id);
            return RedirectToAction(nameof(Contracts));
        }

    }
}