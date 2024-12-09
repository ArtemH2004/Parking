using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    public class GuardViewModel
    {
        public int GuardId { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public int ParkingLotId { get; set; }
    }
}
