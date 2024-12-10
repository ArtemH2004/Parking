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

        public async Task UpdateGuard(Guard guard)
        {
            _context.Guards.Update(guard);
            await _context.SaveChangesAsync();
        }

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
