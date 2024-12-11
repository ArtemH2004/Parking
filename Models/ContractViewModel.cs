using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    public class ContractViewModel
    {
        public int ContractId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int ParkingLotId { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        public int GuardId { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
