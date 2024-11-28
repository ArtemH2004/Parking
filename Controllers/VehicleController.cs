//using Microsoft.AspNetCore.Mvc;
//using Parking.Data;
//using Parking.Models;
//using System.Diagnostics;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Parking.Controllers
//{
//    public class VehicleController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public VehicleController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public IActionResult Create()
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);

//            if (client == null)
//            {
//                return NotFound("Клиент не найден.");
//            }

//            // Создаем модель с ClientId
//            var viewModel = new VehicleViewModel { ClientId = client.ClientId };
//            return PartialView("_VehicleForm", viewModel); // Передаем viewModel в представление
//        }


//        //[HttpPost]
//        //public async Task<IActionResult> Create([FromForm] VehicleViewModel model)
//        //{
//        //    Debug.WriteLine($"Brand: {model.Brand}, Model: {model.Model}, LicensePlate: {model.LicensePlate}");

//        //    //if (ModelState.IsValid)
//        //    //{
//        //        var vehicle = new Vehicle
//        //        {
//        //            LicensePlate = model.LicensePlate,
//        //            Year = model.Year,
//        //            Brand = model.Brand,
//        //            Model = model.Model,
//        //            ClientId = model.ClientId
//        //        };

//        //        _context.Vehicles.Add(vehicle);
//        //        await _context.SaveChangesAsync();
//        //        return Json(new { success = true });
//        //    //}

//        //    if (!ModelState.IsValid)
//        //    {
//        //        var errors = ModelState.Values.SelectMany(v => v.Errors);
//        //        return Json(new { success = false, error = errors.Select(e => e.ErrorMessage).ToArray() });
//        //    }


//        //    return Json(new { success = false, error = "Некоторые поля формы недостаточно валидны." });
//        //}

//        [HttpPost]
//        public async Task<IActionResult> Create([FromForm] VehicleViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                // Если модель не валидна, вернем ошибки
//                var errors = ModelState.Values.SelectMany(v => v.Errors)
//                                               .Select(e => e.ErrorMessage).ToArray();
//                return Json(new { success = false, errors });
//            }

//            var vehicle = new Vehicle
//            {
//                LicensePlate = model.LicensePlate,
//                Year = model.Year,
//                Brand = model.Brand,
//                Model = model.Model,
//                ClientId = model.ClientId
//            };


//            _context.Vehicles.Add(vehicle);
//            await _context.SaveChangesAsync();
//            return Json(new { success = true });


//        }


//        [HttpGet]
//        public IActionResult Edit(int id)
//        {
//            var vehicle = _context.Vehicles.Find(id);
//            if (vehicle == null)
//            {
//                return NotFound();
//            }

//            var viewModel = new VehicleViewModel
//            {
//                VehicleId = vehicle.VehicleId,
//                LicensePlate = vehicle.LicensePlate,
//                Year = vehicle.Year,
//                Brand = vehicle.Brand,
//                Model = vehicle.Model,
//                ClientId = vehicle.ClientId
//            };

//            return PartialView("_VehicleForm", viewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(VehicleViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
//                if (vehicle != null)
//                {
//                    vehicle.LicensePlate = model.LicensePlate;
//                    vehicle.Year = model.Year;
//                    vehicle.Brand = model.Brand;
//                    vehicle.Model = model.Model;

//                    await _context.SaveChangesAsync();
//                    return RedirectToAction("Index", "Client");
//                }
//            }

//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var vehicle = await _context.Vehicles.FindAsync(id);
//            if (vehicle != null)
//            {
//                _context.Vehicles.Remove(vehicle);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction("Index", "Client");
//        }
//    }
//}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.Data;
using Parking.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parking.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicle/Index
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (client == null)
            {
                return NotFound();
            }

            var vehicles = _context.Vehicles.Where(v => v.ClientId == client.ClientId).ToList();
            return View(vehicles);
        }

        // GET: Vehicle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);

                if (client != null)
                {
                    var vehicle = new Vehicle
                    {
                        LicensePlate = model.LicensePlate,
                        Year = model.Year,
                        Brand = model.Brand,
                        Model = model.Model,
                        ClientId = client.ClientId
                    };

                    _context.Vehicles.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        // GET: Vehicle/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null || vehicle.ClientId != GetClientId())
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

        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleViewModel model)
        {
            if (id != model.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var vehicle = await _context.Vehicles.FindAsync(id);
                if (vehicle == null || vehicle.ClientId != GetClientId())
                {
                    return NotFound();
                }

                vehicle.LicensePlate = model.LicensePlate;
                vehicle.Year = model.Year;
                vehicle.Brand = model.Brand;
                vehicle.Model = model.Model;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Vehicle/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null || vehicle.ClientId != GetClientId())
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null || vehicle.ClientId != GetClientId())
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private int GetClientId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _context.Clients.FirstOrDefault(c => c.ApplicationUserId == userId);
            return client?.ClientId ?? 0; // Возвращаем 0, если клиента не найдено
        }
    }
}
