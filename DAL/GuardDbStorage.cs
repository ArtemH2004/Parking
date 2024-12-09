using Microsoft.EntityFrameworkCore;
using Parking.Data;
using Parking.Models;

namespace Parking.Services
{
    public class GuardDbStorage
    {
        private readonly ApplicationDbContext _context;

        public GuardDbStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Guard>> GetAllGuards()
        {
            return await _context.Guards.ToListAsync();
        }

        public async Task<Guard?> GetGuardById(int id)
        {
            return await _context.Guards.FindAsync(id);
        }

        public async Task AddGuard(Guard guard)
        {
            _context.Guards.Add(guard);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateVehicle(Vehicle vehicle)
        //{
        //    _context.Vehicles.Update(vehicle);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateVehicle(Vehicle vehicle)
        //{
        //    var existingVehicle = await _context.Vehicles.FindAsync(vehicle.VehicleId);
        //    if (existingVehicle != null)
        //    {
        //        existingVehicle.LicensePlate = vehicle.LicensePlate;
        //        existingVehicle.Year = vehicle.Year;
        //        existingVehicle.Brand = vehicle.Brand;
        //        existingVehicle.Model = vehicle.Model;
        //        existingVehicle.ClientId = vehicle.ClientId;

        //        await _context.SaveChangesAsync();
        //    }
        //}


        public async Task DeleteGuard(int id)
        {
            var guard = await _context.Guards.FindAsync(id);
            if (guard != null)
            {
                _context.Guards.Remove(guard);
                await _context.SaveChangesAsync();
            }
        }
    }
}
