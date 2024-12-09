using System.Collections.Generic;

namespace Parking.Models
{
    public class ParkingLot
    {
        public int ParkingLotId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int QuantitySpace { get; set; }
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
        public ICollection<Driver> Drivers { get; set; } = new List<Driver>();
        public ICollection<Guard> Guards { get; set; } = new List<Guard>();
    }
}
