using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    public class DriverViewModel
    {
        public int DriverId { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int ExperienceYears { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public char OpenCategory { get; set; }
        [Required]
        public int ParkingLotId { get; set; }
    }
}
