using Microsoft.EntityFrameworkCore;
using Parking.Data;
using Parking.Models;

namespace Parking.Services
{
    public class ParkingLotDbStorage
    {
        private readonly ApplicationDbContext _context;

        public ParkingLotDbStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ParkingLot>> GetAllParkingLots()
        {
            return await _context.ParkingLots.ToListAsync();
        }

        public async Task<ParkingLot?> GetParkingLotById(int id)
        {
            return await _context.ParkingLots.FindAsync(id);
        }

        public async Task AddParkingLot(ParkingLot parkingLot)
        {
            _context.ParkingLots.Add(parkingLot);
            await _context.SaveChangesAsync();
        }


        //TODO


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


        public async Task DeleteParkingLot(int id)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(id);
            if (parkingLot != null)
            {
                _context.ParkingLots.Remove(parkingLot);
                await _context.SaveChangesAsync();
            }
        }
    }
}
