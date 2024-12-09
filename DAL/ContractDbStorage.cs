using Microsoft.EntityFrameworkCore;
using Parking.Data;
using Parking.Models;

namespace Parking.Services
{
    public class ContractDbStorage
    {
        private readonly ApplicationDbContext _context;

        public ContractDbStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contract>> GetAllContracts()
        {
            return await _context.Contracts.ToListAsync();
        }

        public async Task<Contract?> GetContractById(int id)
        {
            return await _context.Contracts.FindAsync(id);
        }

        public async Task AddContract(Contract contract)
        {
            _context.Contracts.Add(contract);
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


        public async Task DeleteContract(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }
    }
}
