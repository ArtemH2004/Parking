namespace Parking.Models
{
    public class Guard
    {
        public int GuardId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public int ParkingLotId { get; set; }
        public ParkingLot ParkingLot { get; set; }
    }
}
