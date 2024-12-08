namespace Parking.Models
{
    public class ParkingType
    {
        public int ParkingTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public ParkingLot ParkingLot { get; set; }
    }
}
