using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    public class RentalViewModel
    {
        public int ParkingLotId { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required] 
        public int GuardId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public decimal Amount { get; set; }
        public List<SelectListItem> Vehicles { get; set; }
    }
}
