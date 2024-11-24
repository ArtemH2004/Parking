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
        //public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        //public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        //TODO
        //public string? ApplicationUserId { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }

    }
}