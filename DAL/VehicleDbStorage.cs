using Microsoft.EntityFrameworkCore;
using Parking.Data;
using Parking.Models;

namespace Parking.Services
{
    public class VehicleDbStorage
    {
        private readonly ApplicationDbContext _context;

        public VehicleDbStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vehicle>> GetAllVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleById(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task AddVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateVehicle(Vehicle vehicle)
        //{
        //    _context.Vehicles.Update(vehicle);
        //    await _context.SaveChangesAsync();
        //}

        public async Task UpdateVehicle(Vehicle vehicle)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(vehicle.VehicleId);
            if (existingVehicle != null)
            {
                existingVehicle.LicensePlate = vehicle.LicensePlate;
                existingVehicle.Year = vehicle.Year;
                existingVehicle.Brand = vehicle.Brand;
                existingVehicle.Model = vehicle.Model;
                existingVehicle.ClientId = vehicle.ClientId;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
