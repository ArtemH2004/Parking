using System.Collections.Generic;

namespace Parking.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string LicensePlate { get; set; }
        public int Year { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        //public int ClientId { get; set; }
        //public virtual Client Client { get; set; }
        //public virtual Contract Contract { get; set; }
    }
}
