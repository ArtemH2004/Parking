using Parking.Data;
using Parking.Models;


namespace Parking.DAL
{
    internal class ClientDbStorage
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

        public Client? GetByUserId(string userId)
        {
            return _context.Clients.FirstOrDefault(x => x.ApplicationUserId == userId);
        }
    }
}