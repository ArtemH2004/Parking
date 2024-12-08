using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    public class ParkingLotViewModel
    {
        public int ParkingLotId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int QuantitySpace { get; set; }
    }
}
