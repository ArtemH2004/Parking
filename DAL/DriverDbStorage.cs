using Microsoft.EntityFrameworkCore;
using Parking.Data;
using Parking.Models;

namespace Parking.Services
{
    public class DriverDbStorage
    {
        private readonly ApplicationDbContext _context;

        public DriverDbStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Driver>> GetAllDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<Driver?> GetDriverById(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task AddDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDriver(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
        }
    }
}
