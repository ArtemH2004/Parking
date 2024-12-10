using Parking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.Data;
using Parking.Services;
using Parking.DAL;
using System.Diagnostics.Contracts;

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

            if (models.Photo != null && models.Photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await models.Photo.CopyToAsync(memoryStream);
                    parkingLot.Photo = memoryStream.ToArray();
                }
            }

            await _parkingLotDbStorage.AddParkingLot(parkingLot);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditParkingLot(int id)
        {
            var parkingLot = await _parkingLotDbStorage.GetParkingLotById(id);
            if (parkingLot == null)
            {
                return NotFound();
            }

            var model = new ParkingLotViewModel
            {
                ParkingLotId = parkingLot.ParkingLotId,
                Name = parkingLot.Name,
                Address = parkingLot.Address,
                QuantitySpace = parkingLot.QuantitySpace
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditParkingLot(ParkingLotViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var parkingLot = await _parkingLotDbStorage.GetParkingLotById(models.ParkingLotId);
            if (parkingLot == null)
            {
                return NotFound();
            }

            parkingLot.Name = models.Name;
            parkingLot.Address = models.Address;
            parkingLot.QuantitySpace = models.QuantitySpace;

            if (models.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await models.Photo.CopyToAsync(memoryStream);
                    parkingLot.Photo = memoryStream.ToArray();
                }
            }

            await _parkingLotDbStorage.UpdateParkingLot(parkingLot);

            return RedirectToAction(nameof(Index));
        }


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
                ClientId = models.ClientId,
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

        public async Task<IActionResult> EditDriver(int id)
        {
            var driver = await _driverDbStorage.GetDriverById(id);
            if (driver == null)
            {
                return NotFound();
            }

            var model = new DriverViewModel
            {
                DriverId = driver.DriverId,
                LastName = driver.LastName,
                FirstName = driver.FirstName,
                MiddleName = driver.MiddleName,
                Phone = driver.Phone,
                ExperienceYears = driver.ExperienceYears,
                Salary = driver.Salary,
                OpenCategory = driver.OpenCategory,
                ParkingLotId = driver.ParkingLotId
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDriver(DriverViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var driver = await _driverDbStorage.GetDriverById(models.DriverId);
            if (driver == null)
            {
                return NotFound();
            }

            driver.LastName = models.LastName;
            driver.FirstName = models.FirstName;
            driver.MiddleName = models.MiddleName;
            driver.Phone = models.Phone;
            driver.ExperienceYears = models.ExperienceYears;
            driver.Salary = models.Salary;
            driver.OpenCategory = models.OpenCategory;
            driver.ParkingLotId = models.ParkingLotId;

            await _driverDbStorage.UpdateDriver(driver);

            return RedirectToAction(nameof(Drivers));
        }

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

        public async Task<IActionResult> EditGuard(int id)
        {
            var guard = await _guardDbStorage.GetGuardById(id);
            if (guard == null)
            {
                return NotFound();
            }

            var model = new GuardViewModel
            {
                GuardId = guard.GuardId,
                LastName = guard.LastName,
                FirstName = guard.FirstName,
                MiddleName = guard.MiddleName,
                Phone = guard.Phone,
                Address = guard.Address,
                Salary = guard.Salary,
                ParkingLotId = guard.ParkingLotId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGuard(GuardViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var guard = await _guardDbStorage.GetGuardById(models.GuardId);
            if (guard == null)
            {
                return NotFound();
            }

            guard.LastName = models.LastName;
            guard.FirstName = models.FirstName;
            guard.MiddleName = models.MiddleName;
            guard.Phone = models.Phone;
            guard.Address = models.Address;
            guard.Salary = models.Salary;
            guard.ParkingLotId = models.ParkingLotId;

            await _guardDbStorage.UpdateGuard(guard);

            return RedirectToAction(nameof(Guards));
        }

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

        //public IActionResult CreateContract()
        //{
        //    var model = new ContractViewModel();
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateContract(ContractViewModel models)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(models);
        //    }

        //    var contract = new Contract
        //    {
        //        StartDate = DateTime.Now,
        //        EndDate = models.EndDate,
        //        Amount = models.Amount,
        //        ParkingLotId = models.ParkingLotId,
        //        VehicleId = models.VehicleId,
        //        ClientId = models.ClientId,
        //        DriverId = models.DriverId,
        //        GuardId = models.GuardId
        //    };

        //    await _contractDbStorage.AddContract(contract);
        //    return RedirectToAction(nameof(Contracts));
        //}

        public async Task<IActionResult> EditContract(int id)
        {
            var contract = await _contractDbStorage.GetContractById(id);
            if (contract == null)
            {
                return NotFound();
            }

            var model = new ContractViewModel
            {
                ContractId = contract.ContractId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Amount = contract.Amount,
                ParkingLotId = contract.ParkingLotId,
                VehicleId = contract.VehicleId,
                ClientId = contract.ClientId,
                DriverId = contract.DriverId,
                GuardId = contract.GuardId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContract(ContractViewModel models)
        {
            if (!ModelState.IsValid)
            {
                return View(models);
            }

            var contract = await _contractDbStorage.GetContractById(models.ContractId);
            if (contract == null)
            {
                return NotFound();
            }

            contract.StartDate = models.StartDate;
            contract.EndDate = models.EndDate;
            contract.Amount = models.Amount;
            contract.ParkingLotId = models.ParkingLotId;
            contract.VehicleId = models.VehicleId;
            contract.ClientId = models.ClientId;
            contract.DriverId = models.DriverId;
            contract.GuardId = models.GuardId;

            await _contractDbStorage.UpdateContract(contract);

            return RedirectToAction(nameof(Contracts));
        }

        [HttpPost, ActionName("DeleteContract")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContractConfirmed(int id)
        {
            await _contractDbStorage.DeleteContract(id);
            return RedirectToAction(nameof(Contracts));
        }

    }
}