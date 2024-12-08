using Microsoft.EntityFrameworkCore;
using Parking.Data;
using Parking.Models;


namespace Parking.DAL
{
    public class ClientDbStorage
    {
        private readonly ApplicationDbContext _context;
        public ClientDbStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public async Task DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Client>> GetAllUsers()
        {
            return await _context.Clients.ToListAsync();
        }

        public Client? GetByUserId(string userId)
        {
            return _context.Clients.FirstOrDefault(x => x.ApplicationUserId == userId);
        }
    }
}