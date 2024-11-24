using System.Collections.Generic;

namespace Parking.Models
{
    public class ParkingSpace
    {
        public int ParkingSpaceId { get; set; }
        public int SpaceNumber { get; set; }
        public decimal DailyPricePerDay { get; set; }
        public bool IsOccupied { get; set; }
        //public int ParkingLotId { get; set; }
        //public virtual ParkingLot ParkingLot { get; set; }
        //public virtual Contract Contract { get; set; }
    }
}
