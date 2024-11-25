using Parking.Models;

namespace Parking.Models
{
    public class VehicleViewModel
    {
        public int VehicleId { get; set; }
        public string LicensePlate { get; set; }
        public int Year { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ClientId { get; set; }
    }
}
