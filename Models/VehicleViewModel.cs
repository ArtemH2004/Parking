using Parking.Models;
using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    public class VehicleViewModel
    {
        public int VehicleId { get; set; }
        [Required]
        public string LicensePlate { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int ClientId { get; set; }
    }
}
