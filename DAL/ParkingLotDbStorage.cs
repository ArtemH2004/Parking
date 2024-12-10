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

        public async Task UpdateParkingLot(ParkingLot parkingLot)
        {
            _context.ParkingLots.Update(parkingLot);
            await _context.SaveChangesAsync();
        }

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
