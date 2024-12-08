using System.Collections.Generic;

namespace Parking.Models
{
    public class ParkingSpace
    {
        public int ParkingSpaceId { get; set; }
        public int SpaceNumber { get; set; }
        public decimal DailyPricePerDay { get; set; }
        public bool IsOccupied { get; set; }
        public int ParkingLotId { get; set; }
        public ParkingLot ParkingLot { get; set; }
        public Contract Contract { get; set; }
    }
}
