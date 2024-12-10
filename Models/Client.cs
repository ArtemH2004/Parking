using Microsoft.Extensions.Hosting;

namespace Parking.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public byte[]? Photo { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}